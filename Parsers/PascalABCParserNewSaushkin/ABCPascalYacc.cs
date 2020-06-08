// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  DESKTOP-G8V08V4
// DateTime: 08.06.2020 21:28:01
// UserName: ?????????
// Input file <D:\PABC_Git\Parsers\PascalABCParserNewSaushkin\ABCPascal.y>

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
    tkOverride=145,tkVirtual=146,tkExtensionMethod=147,tkInteger=148,tkFloat=149,tkHex=150,
    tkUnknown=151};

// Abstract base class for GPLEX scanners
public abstract class ScanBase : AbstractScanner<PascalABCSavParser.Union,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

public partial class GPPGParser: ShiftReduceParser<PascalABCSavParser.Union, LexLocation>
{
  // Verbatim content from D:\PABC_Git\Parsers\PascalABCParserNewSaushkin\ABCPascal.y
// ��� ���������� ����������� � ����� GPPGParser, �������������� ����� ������, ������������ �������� gppg
    public syntax_tree_node root; // �������� ���� ��������������� ������ 

    public List<Error> errors;
    public string current_file_name;
    public int max_errors = 10;
    public PT parsertools;
    public List<compiler_directive> CompilerDirectives;
	public ParserLambdaHelper lambdaHelper = new ParserLambdaHelper();
	
    public GPPGParser(AbstractScanner<PascalABCSavParser.Union, LexLocation> scanner) : base(scanner) { }
  // End verbatim content from D:\PABC_Git\Parsers\PascalABCParserNewSaushkin\ABCPascal.y

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[961];
  private static State[] states = new State[1590];
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
      "enumeration_id_list", "const_simple_expr", "term", "term1", "simple_term", 
      "typed_const", "typed_const_plus", "typed_var_init_expression", "expr", 
      "expr_with_func_decl_lambda", "const_expr", "elem", "range_expr", "const_elem", 
      "array_const", "factor", "relop_expr", "expr_dq", "expr_l1", "expr_l1_func_decl_lambda", 
      "expr_l1_for_lambda", "simple_expr", "range_term", "range_factor", "external_directive_ident", 
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
      "optional_property_initialization", "proc_call", "proc_func_constr_destr_decl", 
      "proc_func_decl", "inclass_proc_func_decl", "inclass_proc_func_decl_noclass", 
      "constr_destr_decl", "inclass_constr_destr_decl", "method_decl", "proc_func_constr_destr_decl_with_attr", 
      "proc_func_decl_noclass", "method_header", "proc_type_decl", "procedural_type_kind", 
      "proc_header", "procedural_type", "constr_destr_header", "proc_func_header", 
      "func_header", "method_procfunc_header", "int_func_header", "int_proc_header", 
      "property_interface", "program_file", "program_header", "parameter_decl", 
      "parameter_decl_list", "property_parameter_list", "const_set", "question_expr", 
      "question_constexpr", "new_question_expr", "record_const", "const_field_list_1", 
      "const_field_list", "const_field", "repeat_stmt", "raise_stmt", "pointer_type", 
      "attribute_declaration", "one_or_some_attribute", "stmt_list", "else_case", 
      "exception_block_else_branch", "compound_stmt", "string_type", "sizeof_expr", 
      "simple_property_definition", "stmt_or_expression", "unlabelled_stmt", 
      "stmt", "case_item", "set_type", "as_is_expr", "as_is_constexpr", "is_type_expr", 
      "as_expr", "power_expr", "power_constexpr", "unsized_array_type", "simple_type_or_", 
      "simple_type", "simple_type_question", "foreach_stmt_ident_dype_opt", "fptype", 
      "type_ref", "fptype_noproctype", "array_type", "template_param", "template_empty_param", 
      "structured_type", "unpacked_structured_type", "empty_template_type_reference", 
      "simple_or_template_type_reference", "type_ref_or_secific", "for_stmt_decl_or_assign", 
      "type_decl_type", "type_ref_and_secific_list", "type_decl_sect", "try_handler", 
      "class_or_interface_keyword", "optional_tk_do", "keyword", "reserved_keyword", 
      "typeof_expr", "simple_fp_sect", "template_param_list", "template_empty_param_list", 
      "template_type_params", "template_type_empty_params", "template_type", 
      "try_stmt", "uses_clause", "used_units_list", "unit_file", "used_unit_name", 
      "unit_header", "var_decl_sect", "var_decl", "var_decl_part", "field_definition", 
      "var_decl_with_assign_var_tuple", "var_stmt", "where_part", "where_part_list", 
      "optional_where_section", "while_stmt", "with_stmt", "variable_as_type", 
      "dotted_identifier", "func_decl_lambda", "expl_func_decl_lambda", "lambda_type_ref", 
      "lambda_type_ref_noproctype", "full_lambda_fp_list", "lambda_simple_fp_sect", 
      "lambda_function_body", "lambda_procedure_body", "common_lambda_body", 
      "optional_full_lambda_fp_list", "field_in_unnamed_object", "list_fields_in_unnamed_object", 
      "func_class_name_ident_list", "rem_lambda", "variable_list", "var_ident_list", 
      "tkAssignOrEqual", "const_pattern_expression", "pattern", "deconstruction_or_const_pattern", 
      "pattern_optional_var", "collection_pattern", "tuple_pattern", "collection_pattern_list_item", 
      "tuple_pattern_item", "collection_pattern_var_item", "match_with", "pattern_case", 
      "pattern_cases", "pattern_out_param", "pattern_out_param_optional_var", 
      "pattern_out_param_list", "pattern_out_param_list_optional_var", "collection_pattern_expr_list", 
      "tuple_pattern_item_list", "const_pattern_expr_list", "$accept", };

  static GPPGParser() {
    states[0] = new State(new int[]{58,1497,11,672,82,1572,84,1577,83,1584,3,-25,49,-25,85,-25,56,-25,26,-25,64,-25,47,-25,50,-25,59,-25,41,-25,34,-25,25,-25,23,-25,27,-25,28,-25,99,-197,100,-197,103,-197},new int[]{-1,1,-224,3,-225,4,-295,1509,-6,1510,-240,1033,-165,1571});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,1493,49,-12,85,-12,56,-12,26,-12,64,-12,47,-12,50,-12,59,-12,11,-12,41,-12,34,-12,25,-12,23,-12,27,-12,28,-12},new int[]{-175,5,-176,1491,-174,1496});
    states[5] = new State(-36,new int[]{-293,6});
    states[6] = new State(new int[]{49,14,56,-60,26,-60,64,-60,47,-60,50,-60,59,-60,11,-60,41,-60,34,-60,25,-60,23,-60,27,-60,28,-60,85,-60},new int[]{-17,7,-34,114,-38,1428,-39,1429});
    states[7] = new State(new int[]{7,9,10,10,5,11,94,12,6,13,2,-24},new int[]{-178,8});
    states[8] = new State(-18);
    states[9] = new State(-19);
    states[10] = new State(-20);
    states[11] = new State(-21);
    states[12] = new State(-22);
    states[13] = new State(-23);
    states[14] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,86,64,30,65,31,66,24,67,98,68,95,69,32,70,33,71,34,72,37,73,38,74,39,75,97,76,40,77,41,78,43,79,44,80,45,81,91,82,46,83,96,84,47,85,25,86,48,87,68,88,92,89,49,90,50,91,51,92,52,93,53,94,54,95,55,96,56,97,58,98,99,99,100,100,103,101,101,102,102,103,59,104,72,105,35,106,36,107,42,109},new int[]{-294,15,-296,113,-146,19,-127,112,-136,22,-140,24,-141,27,-283,30,-139,31,-284,108});
    states[15] = new State(new int[]{10,16,94,17});
    states[16] = new State(-37);
    states[17] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,86,64,30,65,31,66,24,67,98,68,95,69,32,70,33,71,34,72,37,73,38,74,39,75,97,76,40,77,41,78,43,79,44,80,45,81,91,82,46,83,96,84,47,85,25,86,48,87,68,88,92,89,49,90,50,91,51,92,52,93,53,94,54,95,55,96,56,97,58,98,99,99,100,100,103,101,101,102,102,103,59,104,72,105,35,106,36,107,42,109},new int[]{-296,18,-146,19,-127,112,-136,22,-140,24,-141,27,-283,30,-139,31,-284,108});
    states[18] = new State(-39);
    states[19] = new State(new int[]{7,20,131,110,10,-40,94,-40});
    states[20] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,86,64,30,65,31,66,24,67,98,68,95,69,32,70,33,71,34,72,37,73,38,74,39,75,97,76,40,77,41,78,43,79,44,80,45,81,91,82,46,83,96,84,47,85,25,86,48,87,68,88,92,89,49,90,50,91,51,92,52,93,53,94,54,95,55,96,56,97,58,98,99,99,100,100,103,101,101,102,102,103,59,104,72,105,35,106,36,107,42,109},new int[]{-127,21,-136,22,-140,24,-141,27,-283,30,-139,31,-284,108});
    states[21] = new State(-35);
    states[22] = new State(-795);
    states[23] = new State(-792);
    states[24] = new State(-793);
    states[25] = new State(-811);
    states[26] = new State(-812);
    states[27] = new State(-794);
    states[28] = new State(-813);
    states[29] = new State(-814);
    states[30] = new State(-796);
    states[31] = new State(-819);
    states[32] = new State(-815);
    states[33] = new State(-816);
    states[34] = new State(-817);
    states[35] = new State(-818);
    states[36] = new State(-820);
    states[37] = new State(-821);
    states[38] = new State(-822);
    states[39] = new State(-823);
    states[40] = new State(-824);
    states[41] = new State(-825);
    states[42] = new State(-826);
    states[43] = new State(-827);
    states[44] = new State(-828);
    states[45] = new State(-829);
    states[46] = new State(-830);
    states[47] = new State(-831);
    states[48] = new State(-832);
    states[49] = new State(-833);
    states[50] = new State(-834);
    states[51] = new State(-835);
    states[52] = new State(-836);
    states[53] = new State(-837);
    states[54] = new State(-838);
    states[55] = new State(-839);
    states[56] = new State(-840);
    states[57] = new State(-841);
    states[58] = new State(-842);
    states[59] = new State(-843);
    states[60] = new State(-844);
    states[61] = new State(-845);
    states[62] = new State(-846);
    states[63] = new State(-847);
    states[64] = new State(-848);
    states[65] = new State(-849);
    states[66] = new State(-850);
    states[67] = new State(-851);
    states[68] = new State(-852);
    states[69] = new State(-853);
    states[70] = new State(-854);
    states[71] = new State(-855);
    states[72] = new State(-856);
    states[73] = new State(-857);
    states[74] = new State(-858);
    states[75] = new State(-859);
    states[76] = new State(-860);
    states[77] = new State(-861);
    states[78] = new State(-862);
    states[79] = new State(-863);
    states[80] = new State(-864);
    states[81] = new State(-865);
    states[82] = new State(-866);
    states[83] = new State(-867);
    states[84] = new State(-868);
    states[85] = new State(-869);
    states[86] = new State(-870);
    states[87] = new State(-871);
    states[88] = new State(-872);
    states[89] = new State(-873);
    states[90] = new State(-874);
    states[91] = new State(-875);
    states[92] = new State(-876);
    states[93] = new State(-877);
    states[94] = new State(-878);
    states[95] = new State(-879);
    states[96] = new State(-880);
    states[97] = new State(-881);
    states[98] = new State(-882);
    states[99] = new State(-883);
    states[100] = new State(-884);
    states[101] = new State(-885);
    states[102] = new State(-886);
    states[103] = new State(-887);
    states[104] = new State(-888);
    states[105] = new State(-889);
    states[106] = new State(-890);
    states[107] = new State(-891);
    states[108] = new State(-797);
    states[109] = new State(-892);
    states[110] = new State(new int[]{138,111});
    states[111] = new State(-41);
    states[112] = new State(-34);
    states[113] = new State(-38);
    states[114] = new State(new int[]{85,116},new int[]{-245,115});
    states[115] = new State(-32);
    states[116] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476},new int[]{-242,117,-251,767,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[117] = new State(new int[]{86,118,10,119});
    states[118] = new State(-513);
    states[119] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476},new int[]{-251,120,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[120] = new State(-515);
    states[121] = new State(-474);
    states[122] = new State(-477);
    states[123] = new State(new int[]{104,495,105,496,106,497,107,498,108,499,86,-511,10,-511,92,-511,95,-511,30,-511,98,-511,94,-511,12,-511,9,-511,93,-511,29,-511,81,-511,80,-511,2,-511,79,-511,78,-511,77,-511,76,-511},new int[]{-184,124});
    states[124] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,5,646,34,686,41,692},new int[]{-83,125,-82,126,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645,-311,684,-312,685});
    states[125] = new State(-504);
    states[126] = new State(-578);
    states[127] = new State(-580);
    states[128] = new State(new int[]{16,129,86,-582,10,-582,92,-582,95,-582,30,-582,98,-582,94,-582,12,-582,9,-582,93,-582,29,-582,81,-582,80,-582,2,-582,79,-582,78,-582,77,-582,76,-582,6,-582,5,-582,48,-582,55,-582,135,-582,137,-582,75,-582,73,-582,42,-582,39,-582,8,-582,18,-582,19,-582,138,-582,140,-582,139,-582,148,-582,150,-582,149,-582,54,-582,85,-582,37,-582,22,-582,91,-582,51,-582,32,-582,52,-582,96,-582,44,-582,33,-582,50,-582,57,-582,72,-582,70,-582,35,-582,68,-582,69,-582,13,-585});
    states[129] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251},new int[]{-90,130,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627});
    states[130] = new State(new int[]{114,290,119,291,117,292,115,293,118,294,116,295,131,296,16,-595,86,-595,10,-595,92,-595,95,-595,30,-595,98,-595,94,-595,12,-595,9,-595,93,-595,29,-595,81,-595,80,-595,2,-595,79,-595,78,-595,77,-595,76,-595,13,-595,6,-595,5,-595,48,-595,55,-595,135,-595,137,-595,75,-595,73,-595,42,-595,39,-595,8,-595,18,-595,19,-595,138,-595,140,-595,139,-595,148,-595,150,-595,149,-595,54,-595,85,-595,37,-595,22,-595,91,-595,51,-595,32,-595,52,-595,96,-595,44,-595,33,-595,50,-595,57,-595,72,-595,70,-595,35,-595,68,-595,69,-595,110,-595,109,-595,122,-595,123,-595,120,-595,132,-595,130,-595,112,-595,111,-595,125,-595,126,-595,127,-595,128,-595,124,-595},new int[]{-186,131});
    states[131] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-95,132,-232,1427,-77,302,-76,308,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,650,-257,627});
    states[132] = new State(new int[]{6,133,114,-618,119,-618,117,-618,115,-618,118,-618,116,-618,131,-618,16,-618,86,-618,10,-618,92,-618,95,-618,30,-618,98,-618,94,-618,12,-618,9,-618,93,-618,29,-618,81,-618,80,-618,2,-618,79,-618,78,-618,77,-618,76,-618,13,-618,5,-618,48,-618,55,-618,135,-618,137,-618,75,-618,73,-618,42,-618,39,-618,8,-618,18,-618,19,-618,138,-618,140,-618,139,-618,148,-618,150,-618,149,-618,54,-618,85,-618,37,-618,22,-618,91,-618,51,-618,32,-618,52,-618,96,-618,44,-618,33,-618,50,-618,57,-618,72,-618,70,-618,35,-618,68,-618,69,-618,110,-618,109,-618,122,-618,123,-618,120,-618,132,-618,130,-618,112,-618,111,-618,125,-618,126,-618,127,-618,128,-618,124,-618});
    states[133] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251},new int[]{-77,134,-76,308,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,650,-257,627});
    states[134] = new State(new int[]{110,303,109,304,122,305,123,306,120,307,6,-698,5,-698,114,-698,119,-698,117,-698,115,-698,118,-698,116,-698,131,-698,16,-698,86,-698,10,-698,92,-698,95,-698,30,-698,98,-698,94,-698,12,-698,9,-698,93,-698,29,-698,81,-698,80,-698,2,-698,79,-698,78,-698,77,-698,76,-698,13,-698,48,-698,55,-698,135,-698,137,-698,75,-698,73,-698,42,-698,39,-698,8,-698,18,-698,19,-698,138,-698,140,-698,139,-698,148,-698,150,-698,149,-698,54,-698,85,-698,37,-698,22,-698,91,-698,51,-698,32,-698,52,-698,96,-698,44,-698,33,-698,50,-698,57,-698,72,-698,70,-698,35,-698,68,-698,69,-698,132,-698,130,-698,112,-698,111,-698,125,-698,126,-698,127,-698,128,-698,124,-698},new int[]{-187,135});
    states[135] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-76,136,-232,1426,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,650,-257,627});
    states[136] = new State(new int[]{132,309,130,311,112,313,111,314,125,315,126,316,127,317,128,318,124,319,110,-700,109,-700,122,-700,123,-700,120,-700,6,-700,5,-700,114,-700,119,-700,117,-700,115,-700,118,-700,116,-700,131,-700,16,-700,86,-700,10,-700,92,-700,95,-700,30,-700,98,-700,94,-700,12,-700,9,-700,93,-700,29,-700,81,-700,80,-700,2,-700,79,-700,78,-700,77,-700,76,-700,13,-700,48,-700,55,-700,135,-700,137,-700,75,-700,73,-700,42,-700,39,-700,8,-700,18,-700,19,-700,138,-700,140,-700,139,-700,148,-700,150,-700,149,-700,54,-700,85,-700,37,-700,22,-700,91,-700,51,-700,32,-700,52,-700,96,-700,44,-700,33,-700,50,-700,57,-700,72,-700,70,-700,35,-700,68,-700,69,-700},new int[]{-188,137});
    states[137] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-89,138,-258,139,-232,140,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-78,532});
    states[138] = new State(new int[]{132,-718,130,-718,112,-718,111,-718,125,-718,126,-718,127,-718,128,-718,124,-718,110,-718,109,-718,122,-718,123,-718,120,-718,6,-718,5,-718,114,-718,119,-718,117,-718,115,-718,118,-718,116,-718,131,-718,16,-718,86,-718,10,-718,92,-718,95,-718,30,-718,98,-718,94,-718,12,-718,9,-718,93,-718,29,-718,81,-718,80,-718,2,-718,79,-718,78,-718,77,-718,76,-718,13,-718,48,-718,55,-718,135,-718,137,-718,75,-718,73,-718,42,-718,39,-718,8,-718,18,-718,19,-718,138,-718,140,-718,139,-718,148,-718,150,-718,149,-718,54,-718,85,-718,37,-718,22,-718,91,-718,51,-718,32,-718,52,-718,96,-718,44,-718,33,-718,50,-718,57,-718,72,-718,70,-718,35,-718,68,-718,69,-718,113,-713});
    states[139] = new State(-719);
    states[140] = new State(-720);
    states[141] = new State(-731);
    states[142] = new State(new int[]{7,143,132,-732,130,-732,112,-732,111,-732,125,-732,126,-732,127,-732,128,-732,124,-732,110,-732,109,-732,122,-732,123,-732,120,-732,6,-732,5,-732,114,-732,119,-732,117,-732,115,-732,118,-732,116,-732,131,-732,16,-732,86,-732,10,-732,92,-732,95,-732,30,-732,98,-732,94,-732,12,-732,9,-732,93,-732,29,-732,81,-732,80,-732,2,-732,79,-732,78,-732,77,-732,76,-732,13,-732,113,-732,48,-732,55,-732,135,-732,137,-732,75,-732,73,-732,42,-732,39,-732,8,-732,18,-732,19,-732,138,-732,140,-732,139,-732,148,-732,150,-732,149,-732,54,-732,85,-732,37,-732,22,-732,91,-732,51,-732,32,-732,52,-732,96,-732,44,-732,33,-732,50,-732,57,-732,72,-732,70,-732,35,-732,68,-732,69,-732,11,-756});
    states[143] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,86,64,30,65,31,66,24,67,98,68,95,69,32,70,33,71,34,72,37,73,38,74,39,75,97,76,40,77,41,78,43,79,44,80,45,81,91,82,46,83,96,84,47,85,25,86,48,87,68,88,92,89,49,90,50,91,51,92,52,93,53,94,54,95,55,96,56,97,58,98,99,99,100,100,103,101,101,102,102,103,59,104,72,105,35,106,36,107,42,109},new int[]{-127,144,-136,22,-140,24,-141,27,-283,30,-139,31,-284,108});
    states[144] = new State(-763);
    states[145] = new State(-740);
    states[146] = new State(new int[]{138,148,140,149,7,-781,11,-781,132,-781,130,-781,112,-781,111,-781,125,-781,126,-781,127,-781,128,-781,124,-781,110,-781,109,-781,122,-781,123,-781,120,-781,6,-781,5,-781,114,-781,119,-781,117,-781,115,-781,118,-781,116,-781,131,-781,16,-781,86,-781,10,-781,92,-781,95,-781,30,-781,98,-781,94,-781,12,-781,9,-781,93,-781,29,-781,81,-781,80,-781,2,-781,79,-781,78,-781,77,-781,76,-781,13,-781,113,-781,48,-781,55,-781,135,-781,137,-781,75,-781,73,-781,42,-781,39,-781,8,-781,18,-781,19,-781,139,-781,148,-781,150,-781,149,-781,54,-781,85,-781,37,-781,22,-781,91,-781,51,-781,32,-781,52,-781,96,-781,44,-781,33,-781,50,-781,57,-781,72,-781,70,-781,35,-781,68,-781,69,-781,121,-781,104,-781,4,-781,136,-781},new int[]{-155,147});
    states[147] = new State(-784);
    states[148] = new State(-779);
    states[149] = new State(-780);
    states[150] = new State(-783);
    states[151] = new State(-782);
    states[152] = new State(-741);
    states[153] = new State(-176);
    states[154] = new State(-177);
    states[155] = new State(-178);
    states[156] = new State(-733);
    states[157] = new State(new int[]{8,158});
    states[158] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-274,159,-170,161,-136,195,-140,24,-141,27});
    states[159] = new State(new int[]{9,160});
    states[160] = new State(-729);
    states[161] = new State(new int[]{7,162,4,165,117,167,9,-603,130,-603,132,-603,112,-603,111,-603,125,-603,126,-603,127,-603,128,-603,124,-603,110,-603,109,-603,122,-603,123,-603,114,-603,119,-603,115,-603,118,-603,116,-603,131,-603,13,-603,6,-603,94,-603,12,-603,5,-603,86,-603,10,-603,92,-603,95,-603,30,-603,98,-603,93,-603,29,-603,81,-603,80,-603,2,-603,79,-603,78,-603,77,-603,76,-603,11,-603,8,-603,120,-603,16,-603,48,-603,55,-603,135,-603,137,-603,75,-603,73,-603,42,-603,39,-603,18,-603,19,-603,138,-603,140,-603,139,-603,148,-603,150,-603,149,-603,54,-603,85,-603,37,-603,22,-603,91,-603,51,-603,32,-603,52,-603,96,-603,44,-603,33,-603,50,-603,57,-603,72,-603,70,-603,35,-603,68,-603,69,-603,113,-603},new int[]{-289,164});
    states[162] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,86,64,30,65,31,66,24,67,98,68,95,69,32,70,33,71,34,72,37,73,38,74,39,75,97,76,40,77,41,78,43,79,44,80,45,81,91,82,46,83,96,84,47,85,25,86,48,87,68,88,92,89,49,90,50,91,51,92,52,93,53,94,54,95,55,96,56,97,58,98,99,99,100,100,103,101,101,102,102,103,59,104,72,105,35,106,36,107,42,109},new int[]{-127,163,-136,22,-140,24,-141,27,-283,30,-139,31,-284,108});
    states[163] = new State(-246);
    states[164] = new State(-604);
    states[165] = new State(new int[]{117,167},new int[]{-289,166});
    states[166] = new State(-605);
    states[167] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-287,168,-269,265,-262,172,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-271,1374,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,1375,-214,568,-213,569,-291,1376});
    states[168] = new State(new int[]{115,169,94,170});
    states[169] = new State(-220);
    states[170] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-269,171,-262,172,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-271,1374,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,1375,-214,568,-213,569,-291,1376});
    states[171] = new State(-224);
    states[172] = new State(new int[]{13,173,115,-228,94,-228,114,-228,9,-228,10,-228,121,-228,104,-228,86,-228,92,-228,95,-228,30,-228,98,-228,12,-228,93,-228,29,-228,81,-228,80,-228,2,-228,79,-228,78,-228,77,-228,76,-228,131,-228});
    states[173] = new State(-229);
    states[174] = new State(new int[]{6,1424,110,1413,109,1414,122,1415,123,1416,13,-233,115,-233,94,-233,114,-233,9,-233,10,-233,121,-233,104,-233,86,-233,92,-233,95,-233,30,-233,98,-233,12,-233,93,-233,29,-233,81,-233,80,-233,2,-233,79,-233,78,-233,77,-233,76,-233,131,-233},new int[]{-183,175});
    states[175] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151},new int[]{-96,176,-97,267,-170,388,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150});
    states[176] = new State(new int[]{112,217,111,218,125,219,126,220,127,221,128,222,124,223,6,-237,110,-237,109,-237,122,-237,123,-237,13,-237,115,-237,94,-237,114,-237,9,-237,10,-237,121,-237,104,-237,86,-237,92,-237,95,-237,30,-237,98,-237,12,-237,93,-237,29,-237,81,-237,80,-237,2,-237,79,-237,78,-237,77,-237,76,-237,131,-237},new int[]{-185,177});
    states[177] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151},new int[]{-97,178,-170,388,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150});
    states[178] = new State(new int[]{8,179,112,-239,111,-239,125,-239,126,-239,127,-239,128,-239,124,-239,6,-239,110,-239,109,-239,122,-239,123,-239,13,-239,115,-239,94,-239,114,-239,9,-239,10,-239,121,-239,104,-239,86,-239,92,-239,95,-239,30,-239,98,-239,12,-239,93,-239,29,-239,81,-239,80,-239,2,-239,79,-239,78,-239,77,-239,76,-239,131,-239});
    states[179] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356,9,-171},new int[]{-69,180,-67,182,-87,370,-84,185,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[180] = new State(new int[]{9,181});
    states[181] = new State(-244);
    states[182] = new State(new int[]{94,183,9,-170,12,-170});
    states[183] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-87,184,-84,185,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[184] = new State(-173);
    states[185] = new State(new int[]{13,186,6,1397,94,-174,9,-174,12,-174,5,-174});
    states[186] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-84,187,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[187] = new State(new int[]{5,188,13,186});
    states[188] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-84,189,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[189] = new State(new int[]{13,186,6,-116,94,-116,9,-116,12,-116,5,-116,86,-116,10,-116,92,-116,95,-116,30,-116,98,-116,93,-116,29,-116,81,-116,80,-116,2,-116,79,-116,78,-116,77,-116,76,-116});
    states[190] = new State(new int[]{110,1413,109,1414,122,1415,123,1416,114,1417,119,1418,117,1419,115,1420,118,1421,116,1422,131,1423,13,-113,6,-113,94,-113,9,-113,12,-113,5,-113,86,-113,10,-113,92,-113,95,-113,30,-113,98,-113,93,-113,29,-113,81,-113,80,-113,2,-113,79,-113,78,-113,77,-113,76,-113},new int[]{-183,191,-182,1411});
    states[191] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-12,192,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383});
    states[192] = new State(new int[]{130,215,132,216,112,217,111,218,125,219,126,220,127,221,128,222,124,223,110,-125,109,-125,122,-125,123,-125,114,-125,119,-125,117,-125,115,-125,118,-125,116,-125,131,-125,13,-125,6,-125,94,-125,9,-125,12,-125,5,-125,86,-125,10,-125,92,-125,95,-125,30,-125,98,-125,93,-125,29,-125,81,-125,80,-125,2,-125,79,-125,78,-125,77,-125,76,-125},new int[]{-191,193,-185,196});
    states[193] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-274,194,-170,161,-136,195,-140,24,-141,27});
    states[194] = new State(-130);
    states[195] = new State(-245);
    states[196] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-10,197,-259,1410,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381});
    states[197] = new State(new int[]{113,198,130,-135,132,-135,112,-135,111,-135,125,-135,126,-135,127,-135,128,-135,124,-135,110,-135,109,-135,122,-135,123,-135,114,-135,119,-135,117,-135,115,-135,118,-135,116,-135,131,-135,13,-135,6,-135,94,-135,9,-135,12,-135,5,-135,86,-135,10,-135,92,-135,95,-135,30,-135,98,-135,93,-135,29,-135,81,-135,80,-135,2,-135,79,-135,78,-135,77,-135,76,-135});
    states[198] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-10,199,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381});
    states[199] = new State(-131);
    states[200] = new State(new int[]{4,202,11,204,7,1403,136,1405,8,1406,113,-144,130,-144,132,-144,112,-144,111,-144,125,-144,126,-144,127,-144,128,-144,124,-144,110,-144,109,-144,122,-144,123,-144,114,-144,119,-144,117,-144,115,-144,118,-144,116,-144,131,-144,13,-144,6,-144,94,-144,9,-144,12,-144,5,-144,86,-144,10,-144,92,-144,95,-144,30,-144,98,-144,93,-144,29,-144,81,-144,80,-144,2,-144,79,-144,78,-144,77,-144,76,-144},new int[]{-11,201});
    states[201] = new State(-161);
    states[202] = new State(new int[]{117,167},new int[]{-289,203});
    states[203] = new State(-162);
    states[204] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356,5,1399,12,-171},new int[]{-110,205,-69,207,-84,209,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384,-67,182,-87,370});
    states[205] = new State(new int[]{12,206});
    states[206] = new State(-163);
    states[207] = new State(new int[]{12,208});
    states[208] = new State(-167);
    states[209] = new State(new int[]{5,210,13,186,6,1397,94,-174,12,-174});
    states[210] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356,5,-681,12,-681},new int[]{-111,211,-84,1396,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[211] = new State(new int[]{5,212,12,-686});
    states[212] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-84,213,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[213] = new State(new int[]{13,186,12,-688});
    states[214] = new State(new int[]{130,215,132,216,112,217,111,218,125,219,126,220,127,221,128,222,124,223,110,-124,109,-124,122,-124,123,-124,114,-124,119,-124,117,-124,115,-124,118,-124,116,-124,131,-124,13,-124,6,-124,94,-124,9,-124,12,-124,5,-124,86,-124,10,-124,92,-124,95,-124,30,-124,98,-124,93,-124,29,-124,81,-124,80,-124,2,-124,79,-124,78,-124,77,-124,76,-124},new int[]{-191,193,-185,196});
    states[215] = new State(-707);
    states[216] = new State(-708);
    states[217] = new State(-137);
    states[218] = new State(-138);
    states[219] = new State(-139);
    states[220] = new State(-140);
    states[221] = new State(-141);
    states[222] = new State(-142);
    states[223] = new State(-143);
    states[224] = new State(new int[]{113,198,130,-132,132,-132,112,-132,111,-132,125,-132,126,-132,127,-132,128,-132,124,-132,110,-132,109,-132,122,-132,123,-132,114,-132,119,-132,117,-132,115,-132,118,-132,116,-132,131,-132,13,-132,6,-132,94,-132,9,-132,12,-132,5,-132,86,-132,10,-132,92,-132,95,-132,30,-132,98,-132,93,-132,29,-132,81,-132,80,-132,2,-132,79,-132,78,-132,77,-132,76,-132});
    states[225] = new State(-155);
    states[226] = new State(new int[]{23,1385,137,23,80,25,81,26,75,28,73,29,17,-814,8,-814,7,-814,136,-814,4,-814,15,-814,104,-814,105,-814,106,-814,107,-814,108,-814,86,-814,10,-814,11,-814,5,-814,92,-814,95,-814,30,-814,98,-814,121,-814,132,-814,130,-814,112,-814,111,-814,125,-814,126,-814,127,-814,128,-814,124,-814,110,-814,109,-814,122,-814,123,-814,120,-814,6,-814,114,-814,119,-814,117,-814,115,-814,118,-814,116,-814,131,-814,16,-814,94,-814,12,-814,9,-814,93,-814,29,-814,2,-814,79,-814,78,-814,77,-814,76,-814,13,-814,113,-814,48,-814,55,-814,135,-814,42,-814,39,-814,18,-814,19,-814,138,-814,140,-814,139,-814,148,-814,150,-814,149,-814,54,-814,85,-814,37,-814,22,-814,91,-814,51,-814,32,-814,52,-814,96,-814,44,-814,33,-814,50,-814,57,-814,72,-814,70,-814,35,-814,68,-814,69,-814},new int[]{-274,227,-170,161,-136,195,-140,24,-141,27});
    states[227] = new State(new int[]{11,229,8,681,86,-615,10,-615,92,-615,95,-615,30,-615,98,-615,132,-615,130,-615,112,-615,111,-615,125,-615,126,-615,127,-615,128,-615,124,-615,110,-615,109,-615,122,-615,123,-615,120,-615,6,-615,5,-615,114,-615,119,-615,117,-615,115,-615,118,-615,116,-615,131,-615,16,-615,94,-615,12,-615,9,-615,93,-615,29,-615,81,-615,80,-615,2,-615,79,-615,78,-615,77,-615,76,-615,13,-615,48,-615,55,-615,135,-615,137,-615,75,-615,73,-615,42,-615,39,-615,18,-615,19,-615,138,-615,140,-615,139,-615,148,-615,150,-615,149,-615,54,-615,85,-615,37,-615,22,-615,91,-615,51,-615,32,-615,52,-615,96,-615,44,-615,33,-615,50,-615,57,-615,72,-615,70,-615,35,-615,68,-615,69,-615,113,-615},new int[]{-65,228});
    states[228] = new State(-608);
    states[229] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,5,646,34,686,41,692,12,-772},new int[]{-63,230,-66,461,-83,522,-82,126,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645,-311,684,-312,685});
    states[230] = new State(new int[]{12,231});
    states[231] = new State(new int[]{8,233,86,-607,10,-607,92,-607,95,-607,30,-607,98,-607,132,-607,130,-607,112,-607,111,-607,125,-607,126,-607,127,-607,128,-607,124,-607,110,-607,109,-607,122,-607,123,-607,120,-607,6,-607,5,-607,114,-607,119,-607,117,-607,115,-607,118,-607,116,-607,131,-607,16,-607,94,-607,12,-607,9,-607,93,-607,29,-607,81,-607,80,-607,2,-607,79,-607,78,-607,77,-607,76,-607,13,-607,48,-607,55,-607,135,-607,137,-607,75,-607,73,-607,42,-607,39,-607,18,-607,19,-607,138,-607,140,-607,139,-607,148,-607,150,-607,149,-607,54,-607,85,-607,37,-607,22,-607,91,-607,51,-607,32,-607,52,-607,96,-607,44,-607,33,-607,50,-607,57,-607,72,-607,70,-607,35,-607,68,-607,69,-607,113,-607},new int[]{-5,232});
    states[232] = new State(-609);
    states[233] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,931,129,377,110,355,109,356,60,157,9,-183},new int[]{-62,234,-61,236,-80,934,-79,239,-84,240,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384,-88,935,-233,936,-53,937});
    states[234] = new State(new int[]{9,235});
    states[235] = new State(-606);
    states[236] = new State(new int[]{94,237,9,-184});
    states[237] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,931,129,377,110,355,109,356,60,157},new int[]{-80,238,-79,239,-84,240,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384,-88,935,-233,936,-53,937});
    states[238] = new State(-186);
    states[239] = new State(-404);
    states[240] = new State(new int[]{13,186,94,-179,9,-179,86,-179,10,-179,92,-179,95,-179,30,-179,98,-179,12,-179,93,-179,29,-179,81,-179,80,-179,2,-179,79,-179,78,-179,77,-179,76,-179});
    states[241] = new State(-156);
    states[242] = new State(-157);
    states[243] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,244,-140,24,-141,27});
    states[244] = new State(-158);
    states[245] = new State(-159);
    states[246] = new State(new int[]{8,247});
    states[247] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-274,248,-170,161,-136,195,-140,24,-141,27});
    states[248] = new State(new int[]{9,249});
    states[249] = new State(-596);
    states[250] = new State(-160);
    states[251] = new State(new int[]{8,252});
    states[252] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-274,253,-273,255,-170,257,-136,195,-140,24,-141,27});
    states[253] = new State(new int[]{9,254});
    states[254] = new State(-597);
    states[255] = new State(new int[]{9,256});
    states[256] = new State(-598);
    states[257] = new State(new int[]{7,162,4,258,117,260,119,1383,9,-603},new int[]{-289,164,-290,1384});
    states[258] = new State(new int[]{117,260,119,1383},new int[]{-289,166,-290,259});
    states[259] = new State(-602);
    states[260] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598,115,-227,94,-227},new int[]{-287,168,-288,261,-269,265,-262,172,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-271,1374,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,1375,-214,568,-213,569,-291,1376,-270,1382});
    states[261] = new State(new int[]{115,262,94,263});
    states[262] = new State(-222);
    states[263] = new State(-227,new int[]{-270,264});
    states[264] = new State(-226);
    states[265] = new State(-223);
    states[266] = new State(new int[]{112,217,111,218,125,219,126,220,127,221,128,222,124,223,6,-236,110,-236,109,-236,122,-236,123,-236,13,-236,115,-236,94,-236,114,-236,9,-236,10,-236,121,-236,104,-236,86,-236,92,-236,95,-236,30,-236,98,-236,12,-236,93,-236,29,-236,81,-236,80,-236,2,-236,79,-236,78,-236,77,-236,76,-236,131,-236},new int[]{-185,177});
    states[267] = new State(new int[]{8,179,112,-238,111,-238,125,-238,126,-238,127,-238,128,-238,124,-238,6,-238,110,-238,109,-238,122,-238,123,-238,13,-238,115,-238,94,-238,114,-238,9,-238,10,-238,121,-238,104,-238,86,-238,92,-238,95,-238,30,-238,98,-238,12,-238,93,-238,29,-238,81,-238,80,-238,2,-238,79,-238,78,-238,77,-238,76,-238,131,-238});
    states[268] = new State(new int[]{7,162,121,269,117,167,8,-240,112,-240,111,-240,125,-240,126,-240,127,-240,128,-240,124,-240,6,-240,110,-240,109,-240,122,-240,123,-240,13,-240,115,-240,94,-240,114,-240,9,-240,10,-240,104,-240,86,-240,92,-240,95,-240,30,-240,98,-240,12,-240,93,-240,29,-240,81,-240,80,-240,2,-240,79,-240,78,-240,77,-240,76,-240,131,-240},new int[]{-289,680});
    states[269] = new State(new int[]{8,271,137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-269,270,-262,172,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-271,1374,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,1375,-214,568,-213,569,-291,1376});
    states[270] = new State(-275);
    states[271] = new State(new int[]{9,272,137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-74,277,-72,283,-266,286,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[272] = new State(new int[]{121,273,115,-279,94,-279,114,-279,9,-279,10,-279,104,-279,86,-279,92,-279,95,-279,30,-279,98,-279,12,-279,93,-279,29,-279,81,-279,80,-279,2,-279,79,-279,78,-279,77,-279,76,-279,131,-279});
    states[273] = new State(new int[]{8,275,137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-269,274,-262,172,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-271,1374,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,1375,-214,568,-213,569,-291,1376});
    states[274] = new State(-277);
    states[275] = new State(new int[]{9,276,137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-74,277,-72,283,-266,286,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[276] = new State(new int[]{121,273,115,-281,94,-281,114,-281,9,-281,10,-281,104,-281,86,-281,92,-281,95,-281,30,-281,98,-281,12,-281,93,-281,29,-281,81,-281,80,-281,2,-281,79,-281,78,-281,77,-281,76,-281,131,-281});
    states[277] = new State(new int[]{9,278,94,1045});
    states[278] = new State(new int[]{121,279,13,-235,115,-235,94,-235,114,-235,9,-235,10,-235,104,-235,86,-235,92,-235,95,-235,30,-235,98,-235,12,-235,93,-235,29,-235,81,-235,80,-235,2,-235,79,-235,78,-235,77,-235,76,-235,131,-235});
    states[279] = new State(new int[]{8,281,137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-269,280,-262,172,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-271,1374,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,1375,-214,568,-213,569,-291,1376});
    states[280] = new State(-278);
    states[281] = new State(new int[]{9,282,137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-74,277,-72,283,-266,286,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[282] = new State(new int[]{121,273,115,-282,94,-282,114,-282,9,-282,10,-282,104,-282,86,-282,92,-282,95,-282,30,-282,98,-282,12,-282,93,-282,29,-282,81,-282,80,-282,2,-282,79,-282,78,-282,77,-282,76,-282,131,-282});
    states[283] = new State(new int[]{94,284});
    states[284] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-72,285,-266,286,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[285] = new State(-247);
    states[286] = new State(new int[]{114,287,94,-249,9,-249});
    states[287] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,288,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[288] = new State(-250);
    states[289] = new State(new int[]{114,290,119,291,117,292,115,293,118,294,116,295,131,296,16,-594,86,-594,10,-594,92,-594,95,-594,30,-594,98,-594,94,-594,12,-594,9,-594,93,-594,29,-594,81,-594,80,-594,2,-594,79,-594,78,-594,77,-594,76,-594,13,-594,6,-594,5,-594,48,-594,55,-594,135,-594,137,-594,75,-594,73,-594,42,-594,39,-594,8,-594,18,-594,19,-594,138,-594,140,-594,139,-594,148,-594,150,-594,149,-594,54,-594,85,-594,37,-594,22,-594,91,-594,51,-594,32,-594,52,-594,96,-594,44,-594,33,-594,50,-594,57,-594,72,-594,70,-594,35,-594,68,-594,69,-594,110,-594,109,-594,122,-594,123,-594,120,-594,132,-594,130,-594,112,-594,111,-594,125,-594,126,-594,127,-594,128,-594,124,-594},new int[]{-186,131});
    states[290] = new State(-690);
    states[291] = new State(-691);
    states[292] = new State(-692);
    states[293] = new State(-693);
    states[294] = new State(-694);
    states[295] = new State(-695);
    states[296] = new State(-696);
    states[297] = new State(new int[]{6,133,5,298,114,-617,119,-617,117,-617,115,-617,118,-617,116,-617,131,-617,16,-617,86,-617,10,-617,92,-617,95,-617,30,-617,98,-617,94,-617,12,-617,9,-617,93,-617,29,-617,81,-617,80,-617,2,-617,79,-617,78,-617,77,-617,76,-617,13,-617});
    states[298] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,5,-679,86,-679,10,-679,92,-679,95,-679,30,-679,98,-679,94,-679,12,-679,9,-679,93,-679,29,-679,2,-679,79,-679,78,-679,77,-679,76,-679,6,-679},new int[]{-104,299,-95,651,-77,302,-76,308,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,650,-257,627});
    states[299] = new State(new int[]{5,300,86,-682,10,-682,92,-682,95,-682,30,-682,98,-682,94,-682,12,-682,9,-682,93,-682,29,-682,81,-682,80,-682,2,-682,79,-682,78,-682,77,-682,76,-682,6,-682});
    states[300] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251},new int[]{-95,301,-77,302,-76,308,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,650,-257,627});
    states[301] = new State(new int[]{6,133,86,-684,10,-684,92,-684,95,-684,30,-684,98,-684,94,-684,12,-684,9,-684,93,-684,29,-684,81,-684,80,-684,2,-684,79,-684,78,-684,77,-684,76,-684});
    states[302] = new State(new int[]{110,303,109,304,122,305,123,306,120,307,6,-697,5,-697,114,-697,119,-697,117,-697,115,-697,118,-697,116,-697,131,-697,16,-697,86,-697,10,-697,92,-697,95,-697,30,-697,98,-697,94,-697,12,-697,9,-697,93,-697,29,-697,81,-697,80,-697,2,-697,79,-697,78,-697,77,-697,76,-697,13,-697,48,-697,55,-697,135,-697,137,-697,75,-697,73,-697,42,-697,39,-697,8,-697,18,-697,19,-697,138,-697,140,-697,139,-697,148,-697,150,-697,149,-697,54,-697,85,-697,37,-697,22,-697,91,-697,51,-697,32,-697,52,-697,96,-697,44,-697,33,-697,50,-697,57,-697,72,-697,70,-697,35,-697,68,-697,69,-697,132,-697,130,-697,112,-697,111,-697,125,-697,126,-697,127,-697,128,-697,124,-697},new int[]{-187,135});
    states[303] = new State(-702);
    states[304] = new State(-703);
    states[305] = new State(-704);
    states[306] = new State(-705);
    states[307] = new State(-706);
    states[308] = new State(new int[]{132,309,130,311,112,313,111,314,125,315,126,316,127,317,128,318,124,319,114,-699,119,-699,117,-699,115,-699,118,-699,116,-699,131,-699,16,-699,86,-699,10,-699,92,-699,95,-699,30,-699,98,-699,94,-699,12,-699,9,-699,93,-699,29,-699,81,-699,80,-699,2,-699,79,-699,78,-699,77,-699,76,-699,13,-699,6,-699,5,-699,48,-699,55,-699,135,-699,137,-699,75,-699,73,-699,42,-699,39,-699,8,-699,18,-699,19,-699,138,-699,140,-699,139,-699,148,-699,150,-699,149,-699,54,-699,85,-699,37,-699,22,-699,91,-699,51,-699,32,-699,52,-699,96,-699,44,-699,33,-699,50,-699,57,-699,72,-699,70,-699,35,-699,68,-699,69,-699,110,-699,109,-699,122,-699,123,-699,120,-699},new int[]{-188,137});
    states[309] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-274,310,-170,161,-136,195,-140,24,-141,27});
    states[310] = new State(-712);
    states[311] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-274,312,-170,161,-136,195,-140,24,-141,27});
    states[312] = new State(-711);
    states[313] = new State(-722);
    states[314] = new State(-723);
    states[315] = new State(-724);
    states[316] = new State(-725);
    states[317] = new State(-726);
    states[318] = new State(-727);
    states[319] = new State(-728);
    states[320] = new State(new int[]{132,-715,130,-715,112,-715,111,-715,125,-715,126,-715,127,-715,128,-715,124,-715,110,-715,109,-715,122,-715,123,-715,120,-715,6,-715,5,-715,114,-715,119,-715,117,-715,115,-715,118,-715,116,-715,131,-715,16,-715,86,-715,10,-715,92,-715,95,-715,30,-715,98,-715,94,-715,12,-715,9,-715,93,-715,29,-715,81,-715,80,-715,2,-715,79,-715,78,-715,77,-715,76,-715,13,-715,48,-715,55,-715,135,-715,137,-715,75,-715,73,-715,42,-715,39,-715,8,-715,18,-715,19,-715,138,-715,140,-715,139,-715,148,-715,150,-715,149,-715,54,-715,85,-715,37,-715,22,-715,91,-715,51,-715,32,-715,52,-715,96,-715,44,-715,33,-715,50,-715,57,-715,72,-715,70,-715,35,-715,68,-715,69,-715,113,-713});
    states[321] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646,12,-774},new int[]{-64,322,-71,324,-85,1381,-82,327,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[322] = new State(new int[]{12,323});
    states[323] = new State(-734);
    states[324] = new State(new int[]{94,325,12,-773});
    states[325] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-85,326,-82,327,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[326] = new State(-776);
    states[327] = new State(new int[]{6,328,94,-777,12,-777});
    states[328] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,329,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[329] = new State(-778);
    states[330] = new State(new int[]{132,331,130,311,112,313,111,314,125,315,126,316,127,317,128,318,124,319,110,-699,109,-699,122,-699,123,-699,120,-699,6,-699,5,-699,114,-699,119,-699,117,-699,115,-699,118,-699,116,-699,131,-699,16,-699,86,-699,10,-699,92,-699,95,-699,30,-699,98,-699,94,-699,12,-699,9,-699,93,-699,29,-699,81,-699,80,-699,2,-699,79,-699,78,-699,77,-699,76,-699,13,-699,48,-699,55,-699,135,-699,137,-699,75,-699,73,-699,42,-699,39,-699,8,-699,18,-699,19,-699,138,-699,140,-699,139,-699,148,-699,150,-699,149,-699,54,-699,85,-699,37,-699,22,-699,91,-699,51,-699,32,-699,52,-699,96,-699,44,-699,33,-699,50,-699,57,-699,72,-699,70,-699,35,-699,68,-699,69,-699},new int[]{-188,137});
    states[331] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,11,334,8,611},new int[]{-274,310,-332,332,-333,333,-170,161,-136,195,-140,24,-141,27});
    states[332] = new State(-621);
    states[333] = new State(-622);
    states[334] = new State(new int[]{138,148,140,149,139,151,148,153,150,154,149,155,50,341,14,343,137,23,80,25,81,26,75,28,73,29,11,334,8,611,6,1379},new int[]{-344,335,-334,1380,-14,339,-154,145,-156,146,-155,150,-15,152,-336,340,-331,344,-274,345,-170,161,-136,195,-140,24,-141,27,-332,1377,-333,1378});
    states[335] = new State(new int[]{12,336,94,337});
    states[336] = new State(-634);
    states[337] = new State(new int[]{138,148,140,149,139,151,148,153,150,154,149,155,50,341,14,343,137,23,80,25,81,26,75,28,73,29,11,334,8,611,6,1379},new int[]{-334,338,-14,339,-154,145,-156,146,-155,150,-15,152,-336,340,-331,344,-274,345,-170,161,-136,195,-140,24,-141,27,-332,1377,-333,1378});
    states[338] = new State(-636);
    states[339] = new State(-637);
    states[340] = new State(-638);
    states[341] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,342,-140,24,-141,27});
    states[342] = new State(-644);
    states[343] = new State(-639);
    states[344] = new State(-640);
    states[345] = new State(new int[]{8,346});
    states[346] = new State(new int[]{14,351,138,148,140,149,139,151,148,153,150,154,149,155,110,355,109,356,137,23,80,25,81,26,75,28,73,29,50,881,11,334,8,611},new int[]{-343,347,-341,888,-14,352,-154,145,-156,146,-155,150,-15,152,-189,353,-136,357,-140,24,-141,27,-331,885,-274,345,-170,161,-332,886,-333,887});
    states[347] = new State(new int[]{9,348,10,349,94,879});
    states[348] = new State(-624);
    states[349] = new State(new int[]{14,351,138,148,140,149,139,151,148,153,150,154,149,155,110,355,109,356,137,23,80,25,81,26,75,28,73,29,50,881,11,334,8,611},new int[]{-341,350,-14,352,-154,145,-156,146,-155,150,-15,152,-189,353,-136,357,-140,24,-141,27,-331,885,-274,345,-170,161,-332,886,-333,887});
    states[350] = new State(-656);
    states[351] = new State(-668);
    states[352] = new State(-669);
    states[353] = new State(new int[]{138,148,140,149,139,151,148,153,150,154,149,155},new int[]{-14,354,-154,145,-156,146,-155,150,-15,152});
    states[354] = new State(-670);
    states[355] = new State(-153);
    states[356] = new State(-154);
    states[357] = new State(new int[]{5,358,9,-672,10,-672,94,-672,7,-245,4,-245,117,-245,8,-245});
    states[358] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,359,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[359] = new State(-671);
    states[360] = new State(new int[]{13,361,114,-212,94,-212,9,-212,10,-212,115,-212,121,-212,104,-212,86,-212,92,-212,95,-212,30,-212,98,-212,12,-212,93,-212,29,-212,81,-212,80,-212,2,-212,79,-212,78,-212,77,-212,76,-212,131,-212});
    states[361] = new State(-210);
    states[362] = new State(new int[]{11,363,7,-792,121,-792,117,-792,8,-792,112,-792,111,-792,125,-792,126,-792,127,-792,128,-792,124,-792,6,-792,110,-792,109,-792,122,-792,123,-792,13,-792,114,-792,94,-792,9,-792,10,-792,115,-792,104,-792,86,-792,92,-792,95,-792,30,-792,98,-792,12,-792,93,-792,29,-792,81,-792,80,-792,2,-792,79,-792,78,-792,77,-792,76,-792,131,-792});
    states[363] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-84,364,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[364] = new State(new int[]{12,365,13,186});
    states[365] = new State(-270);
    states[366] = new State(-145);
    states[367] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356,12,-171},new int[]{-69,368,-67,182,-87,370,-84,185,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[368] = new State(new int[]{12,369});
    states[369] = new State(-152);
    states[370] = new State(-172);
    states[371] = new State(-146);
    states[372] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-10,373,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381});
    states[373] = new State(-147);
    states[374] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-84,375,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[375] = new State(new int[]{9,376,13,186});
    states[376] = new State(-148);
    states[377] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-10,378,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381});
    states[378] = new State(-149);
    states[379] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-10,380,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381});
    states[380] = new State(-150);
    states[381] = new State(-151);
    states[382] = new State(-133);
    states[383] = new State(-134);
    states[384] = new State(-115);
    states[385] = new State(-241);
    states[386] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151},new int[]{-97,387,-170,388,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150});
    states[387] = new State(new int[]{8,179,112,-242,111,-242,125,-242,126,-242,127,-242,128,-242,124,-242,6,-242,110,-242,109,-242,122,-242,123,-242,13,-242,115,-242,94,-242,114,-242,9,-242,10,-242,121,-242,104,-242,86,-242,92,-242,95,-242,30,-242,98,-242,12,-242,93,-242,29,-242,81,-242,80,-242,2,-242,79,-242,78,-242,77,-242,76,-242,131,-242});
    states[388] = new State(new int[]{7,162,8,-240,112,-240,111,-240,125,-240,126,-240,127,-240,128,-240,124,-240,6,-240,110,-240,109,-240,122,-240,123,-240,13,-240,115,-240,94,-240,114,-240,9,-240,10,-240,121,-240,104,-240,86,-240,92,-240,95,-240,30,-240,98,-240,12,-240,93,-240,29,-240,81,-240,80,-240,2,-240,79,-240,78,-240,77,-240,76,-240,131,-240});
    states[389] = new State(-243);
    states[390] = new State(new int[]{9,391,137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-74,277,-72,283,-266,286,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[391] = new State(new int[]{121,273});
    states[392] = new State(-213);
    states[393] = new State(new int[]{13,394,121,395,114,-218,94,-218,9,-218,10,-218,115,-218,104,-218,86,-218,92,-218,95,-218,30,-218,98,-218,12,-218,93,-218,29,-218,81,-218,80,-218,2,-218,79,-218,78,-218,77,-218,76,-218,131,-218});
    states[394] = new State(-211);
    states[395] = new State(new int[]{8,397,137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-269,396,-262,172,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-271,1374,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,1375,-214,568,-213,569,-291,1376});
    states[396] = new State(-276);
    states[397] = new State(new int[]{9,398,137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-74,277,-72,283,-266,286,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[398] = new State(new int[]{121,273,115,-280,94,-280,114,-280,9,-280,10,-280,104,-280,86,-280,92,-280,95,-280,30,-280,98,-280,12,-280,93,-280,29,-280,81,-280,80,-280,2,-280,79,-280,78,-280,77,-280,76,-280,131,-280});
    states[399] = new State(-214);
    states[400] = new State(-215);
    states[401] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,402,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[402] = new State(-251);
    states[403] = new State(-468);
    states[404] = new State(-216);
    states[405] = new State(-252);
    states[406] = new State(-254);
    states[407] = new State(new int[]{11,408,55,1372});
    states[408] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,1042,12,-266,94,-266},new int[]{-153,409,-261,1371,-262,1370,-86,174,-96,266,-97,267,-170,388,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150});
    states[409] = new State(new int[]{12,410,94,1368});
    states[410] = new State(new int[]{55,411});
    states[411] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,412,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[412] = new State(-260);
    states[413] = new State(-261);
    states[414] = new State(-255);
    states[415] = new State(new int[]{8,1242,20,-302,11,-302,86,-302,79,-302,78,-302,77,-302,76,-302,26,-302,137,-302,80,-302,81,-302,75,-302,73,-302,59,-302,25,-302,23,-302,41,-302,34,-302,27,-302,28,-302,43,-302,24,-302},new int[]{-173,416});
    states[416] = new State(new int[]{20,1233,11,-309,86,-309,79,-309,78,-309,77,-309,76,-309,26,-309,137,-309,80,-309,81,-309,75,-309,73,-309,59,-309,25,-309,23,-309,41,-309,34,-309,27,-309,28,-309,43,-309,24,-309},new int[]{-306,417,-305,1231,-304,1253});
    states[417] = new State(new int[]{11,672,86,-326,79,-326,78,-326,77,-326,76,-326,26,-197,137,-197,80,-197,81,-197,75,-197,73,-197,59,-197,25,-197,23,-197,41,-197,34,-197,27,-197,28,-197,43,-197,24,-197},new int[]{-22,418,-29,1211,-31,422,-41,1212,-6,1213,-240,1033,-30,1324,-50,1326,-49,428,-51,1325});
    states[418] = new State(new int[]{86,419,79,1207,78,1208,77,1209,76,1210},new int[]{-7,420});
    states[419] = new State(-284);
    states[420] = new State(new int[]{11,672,86,-326,79,-326,78,-326,77,-326,76,-326,26,-197,137,-197,80,-197,81,-197,75,-197,73,-197,59,-197,25,-197,23,-197,41,-197,34,-197,27,-197,28,-197,43,-197,24,-197},new int[]{-29,421,-31,422,-41,1212,-6,1213,-240,1033,-30,1324,-50,1326,-49,428,-51,1325});
    states[421] = new State(-321);
    states[422] = new State(new int[]{10,424,86,-332,79,-332,78,-332,77,-332,76,-332},new int[]{-180,423});
    states[423] = new State(-327);
    states[424] = new State(new int[]{11,672,86,-333,79,-333,78,-333,77,-333,76,-333,26,-197,137,-197,80,-197,81,-197,75,-197,73,-197,59,-197,25,-197,23,-197,41,-197,34,-197,27,-197,28,-197,43,-197,24,-197},new int[]{-41,425,-30,426,-6,1213,-240,1033,-50,1326,-49,428,-51,1325});
    states[425] = new State(-335);
    states[426] = new State(new int[]{11,672,86,-329,79,-329,78,-329,77,-329,76,-329,25,-197,23,-197,41,-197,34,-197,27,-197,28,-197,43,-197,24,-197},new int[]{-50,427,-49,428,-6,429,-240,1033,-51,1325});
    states[427] = new State(-338);
    states[428] = new State(-339);
    states[429] = new State(new int[]{25,1280,23,1281,41,1226,34,1261,27,1295,28,1302,11,672,43,1309,24,1318},new int[]{-212,430,-240,431,-209,432,-248,433,-3,434,-220,1282,-218,1155,-215,1225,-219,1260,-217,1283,-205,1306,-206,1307,-208,1308});
    states[430] = new State(-348);
    states[431] = new State(-196);
    states[432] = new State(-349);
    states[433] = new State(-367);
    states[434] = new State(new int[]{27,436,43,1104,24,1147,41,1226,34,1261},new int[]{-220,435,-206,1103,-218,1155,-215,1225,-219,1260});
    states[435] = new State(-352);
    states[436] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471,8,-362,104,-362,10,-362},new int[]{-161,437,-160,1086,-159,1087,-131,1088,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096,-323,1098,-138,1102});
    states[437] = new State(new int[]{8,572,104,-452,10,-452},new int[]{-117,438});
    states[438] = new State(new int[]{104,440,10,1075},new int[]{-197,439});
    states[439] = new State(-359);
    states[440] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476},new int[]{-250,441,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[441] = new State(new int[]{10,442});
    states[442] = new State(-411);
    states[443] = new State(new int[]{135,1074,137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,536,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155},new int[]{-101,444,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711});
    states[444] = new State(new int[]{17,445,8,458,7,702,136,704,4,705,104,-744,105,-744,106,-744,107,-744,108,-744,86,-744,10,-744,92,-744,95,-744,30,-744,98,-744,132,-744,130,-744,112,-744,111,-744,125,-744,126,-744,127,-744,128,-744,124,-744,110,-744,109,-744,122,-744,123,-744,120,-744,6,-744,5,-744,114,-744,119,-744,117,-744,115,-744,118,-744,116,-744,131,-744,16,-744,94,-744,12,-744,9,-744,93,-744,29,-744,81,-744,80,-744,2,-744,79,-744,78,-744,77,-744,76,-744,13,-744,113,-744,48,-744,55,-744,135,-744,137,-744,75,-744,73,-744,42,-744,39,-744,18,-744,19,-744,138,-744,140,-744,139,-744,148,-744,150,-744,149,-744,54,-744,85,-744,37,-744,22,-744,91,-744,51,-744,32,-744,52,-744,96,-744,44,-744,33,-744,50,-744,57,-744,72,-744,70,-744,35,-744,68,-744,69,-744,11,-755});
    states[445] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,5,646},new int[]{-109,446,-95,448,-77,302,-76,308,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,650,-257,627});
    states[446] = new State(new int[]{12,447});
    states[447] = new State(-765);
    states[448] = new State(new int[]{5,298,6,133});
    states[449] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,536,18,246,19,251},new int[]{-89,450,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529});
    states[450] = new State(-735);
    states[451] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,536,18,246,19,251},new int[]{-89,452,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529});
    states[452] = new State(-736);
    states[453] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,536,18,246,19,251},new int[]{-89,454,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529});
    states[454] = new State(-737);
    states[455] = new State(-738);
    states[456] = new State(-747);
    states[457] = new State(new int[]{17,445,8,458,7,702,136,704,4,705,15,707,132,-745,130,-745,112,-745,111,-745,125,-745,126,-745,127,-745,128,-745,124,-745,110,-745,109,-745,122,-745,123,-745,120,-745,6,-745,5,-745,114,-745,119,-745,117,-745,115,-745,118,-745,116,-745,131,-745,16,-745,86,-745,10,-745,92,-745,95,-745,30,-745,98,-745,94,-745,12,-745,9,-745,93,-745,29,-745,81,-745,80,-745,2,-745,79,-745,78,-745,77,-745,76,-745,13,-745,113,-745,48,-745,55,-745,135,-745,137,-745,75,-745,73,-745,42,-745,39,-745,18,-745,19,-745,138,-745,140,-745,139,-745,148,-745,150,-745,149,-745,54,-745,85,-745,37,-745,22,-745,91,-745,51,-745,32,-745,52,-745,96,-745,44,-745,33,-745,50,-745,57,-745,72,-745,70,-745,35,-745,68,-745,69,-745,11,-755});
    states[458] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,5,646,34,686,41,692,9,-772},new int[]{-63,459,-66,461,-83,522,-82,126,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645,-311,684,-312,685});
    states[459] = new State(new int[]{9,460});
    states[460] = new State(-766);
    states[461] = new State(new int[]{94,462,12,-771,9,-771});
    states[462] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,5,646,34,686,41,692},new int[]{-83,463,-82,126,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645,-311,684,-312,685});
    states[463] = new State(-575);
    states[464] = new State(new int[]{121,465,17,-757,8,-757,7,-757,136,-757,4,-757,15,-757,132,-757,130,-757,112,-757,111,-757,125,-757,126,-757,127,-757,128,-757,124,-757,110,-757,109,-757,122,-757,123,-757,120,-757,6,-757,5,-757,114,-757,119,-757,117,-757,115,-757,118,-757,116,-757,131,-757,16,-757,86,-757,10,-757,92,-757,95,-757,30,-757,98,-757,94,-757,12,-757,9,-757,93,-757,29,-757,81,-757,80,-757,2,-757,79,-757,78,-757,77,-757,76,-757,13,-757,113,-757,11,-757});
    states[465] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,466,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[466] = new State(-921);
    states[467] = new State(-956);
    states[468] = new State(new int[]{16,129,86,-591,10,-591,92,-591,95,-591,30,-591,98,-591,94,-591,12,-591,9,-591,93,-591,29,-591,81,-591,80,-591,2,-591,79,-591,78,-591,77,-591,76,-591,13,-585});
    states[469] = new State(new int[]{6,133,114,-617,119,-617,117,-617,115,-617,118,-617,116,-617,131,-617,16,-617,86,-617,10,-617,92,-617,95,-617,30,-617,98,-617,94,-617,12,-617,9,-617,93,-617,29,-617,81,-617,80,-617,2,-617,79,-617,78,-617,77,-617,76,-617,13,-617,5,-617,48,-617,55,-617,135,-617,137,-617,75,-617,73,-617,42,-617,39,-617,8,-617,18,-617,19,-617,138,-617,140,-617,139,-617,148,-617,150,-617,149,-617,54,-617,85,-617,37,-617,22,-617,91,-617,51,-617,32,-617,52,-617,96,-617,44,-617,33,-617,50,-617,57,-617,72,-617,70,-617,35,-617,68,-617,69,-617,110,-617,109,-617,122,-617,123,-617,120,-617,132,-617,130,-617,112,-617,111,-617,125,-617,126,-617,127,-617,128,-617,124,-617});
    states[470] = new State(-758);
    states[471] = new State(new int[]{109,473,110,474,111,475,112,476,114,477,115,478,116,479,117,480,118,481,119,482,122,483,123,484,124,485,125,486,126,487,127,488,128,489,129,490,131,491,133,492,134,493,104,495,105,496,106,497,107,498,108,499,113,500},new int[]{-190,472,-184,494});
    states[472] = new State(-785);
    states[473] = new State(-893);
    states[474] = new State(-894);
    states[475] = new State(-895);
    states[476] = new State(-896);
    states[477] = new State(-897);
    states[478] = new State(-898);
    states[479] = new State(-899);
    states[480] = new State(-900);
    states[481] = new State(-901);
    states[482] = new State(-902);
    states[483] = new State(-903);
    states[484] = new State(-904);
    states[485] = new State(-905);
    states[486] = new State(-906);
    states[487] = new State(-907);
    states[488] = new State(-908);
    states[489] = new State(-909);
    states[490] = new State(-910);
    states[491] = new State(-911);
    states[492] = new State(-912);
    states[493] = new State(-913);
    states[494] = new State(-914);
    states[495] = new State(-916);
    states[496] = new State(-917);
    states[497] = new State(-918);
    states[498] = new State(-919);
    states[499] = new State(-920);
    states[500] = new State(-915);
    states[501] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,502,-140,24,-141,27});
    states[502] = new State(-759);
    states[503] = new State(new int[]{9,1051,53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,504,-92,506,-136,1055,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[504] = new State(new int[]{9,505});
    states[505] = new State(-760);
    states[506] = new State(new int[]{94,507,9,-580});
    states[507] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-73,508,-92,1037,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[508] = new State(new int[]{94,1035,5,551,10,-940,9,-940},new int[]{-313,509});
    states[509] = new State(new int[]{10,543,9,-928},new int[]{-320,510});
    states[510] = new State(new int[]{9,511});
    states[511] = new State(new int[]{5,1038,7,-730,132,-730,130,-730,112,-730,111,-730,125,-730,126,-730,127,-730,128,-730,124,-730,110,-730,109,-730,122,-730,123,-730,120,-730,6,-730,114,-730,119,-730,117,-730,115,-730,118,-730,116,-730,131,-730,16,-730,86,-730,10,-730,92,-730,95,-730,30,-730,98,-730,94,-730,12,-730,9,-730,93,-730,29,-730,81,-730,80,-730,2,-730,79,-730,78,-730,77,-730,76,-730,13,-730,113,-730,121,-942},new int[]{-324,512,-314,513});
    states[512] = new State(-926);
    states[513] = new State(new int[]{121,514});
    states[514] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,515,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[515] = new State(-930);
    states[516] = new State(-761);
    states[517] = new State(-762);
    states[518] = new State(new int[]{11,519});
    states[519] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,5,646,34,686,41,692},new int[]{-66,520,-83,522,-82,126,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645,-311,684,-312,685});
    states[520] = new State(new int[]{12,521,94,462});
    states[521] = new State(-764);
    states[522] = new State(-574);
    states[523] = new State(new int[]{7,524,132,-739,130,-739,112,-739,111,-739,125,-739,126,-739,127,-739,128,-739,124,-739,110,-739,109,-739,122,-739,123,-739,120,-739,6,-739,5,-739,114,-739,119,-739,117,-739,115,-739,118,-739,116,-739,131,-739,16,-739,86,-739,10,-739,92,-739,95,-739,30,-739,98,-739,94,-739,12,-739,9,-739,93,-739,29,-739,81,-739,80,-739,2,-739,79,-739,78,-739,77,-739,76,-739,13,-739,113,-739,48,-739,55,-739,135,-739,137,-739,75,-739,73,-739,42,-739,39,-739,8,-739,18,-739,19,-739,138,-739,140,-739,139,-739,148,-739,150,-739,149,-739,54,-739,85,-739,37,-739,22,-739,91,-739,51,-739,32,-739,52,-739,96,-739,44,-739,33,-739,50,-739,57,-739,72,-739,70,-739,35,-739,68,-739,69,-739});
    states[524] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,86,64,30,65,31,66,24,67,98,68,95,69,32,70,33,71,34,72,37,73,38,74,39,75,97,76,40,77,41,78,43,79,44,80,45,81,91,82,46,83,96,84,47,85,25,86,48,87,68,88,92,89,49,90,50,91,51,92,52,93,53,94,54,95,55,96,56,97,58,98,99,99,100,100,103,101,101,102,102,103,59,104,72,105,35,106,36,107,42,471},new int[]{-137,525,-136,526,-140,24,-141,27,-283,527,-139,31,-181,528});
    states[525] = new State(-768);
    states[526] = new State(-798);
    states[527] = new State(-799);
    states[528] = new State(-800);
    states[529] = new State(-746);
    states[530] = new State(-716);
    states[531] = new State(-717);
    states[532] = new State(new int[]{113,533});
    states[533] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,536,18,246,19,251},new int[]{-89,534,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529});
    states[534] = new State(-714);
    states[535] = new State(-757);
    states[536] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,504,-92,537,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[537] = new State(new int[]{94,538,9,-580});
    states[538] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-73,539,-92,1037,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[539] = new State(new int[]{94,1035,5,551,10,-940,9,-940},new int[]{-313,540});
    states[540] = new State(new int[]{10,543,9,-928},new int[]{-320,541});
    states[541] = new State(new int[]{9,542});
    states[542] = new State(-730);
    states[543] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-315,544,-316,1016,-147,547,-136,824,-140,24,-141,27});
    states[544] = new State(new int[]{10,545,9,-929});
    states[545] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-316,546,-147,547,-136,824,-140,24,-141,27});
    states[546] = new State(-938);
    states[547] = new State(new int[]{94,549,5,551,10,-940,9,-940},new int[]{-313,548});
    states[548] = new State(-939);
    states[549] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,550,-140,24,-141,27});
    states[550] = new State(-331);
    states[551] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,552,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[552] = new State(-941);
    states[553] = new State(-256);
    states[554] = new State(new int[]{55,555});
    states[555] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,556,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[556] = new State(-267);
    states[557] = new State(-257);
    states[558] = new State(new int[]{55,559,115,-269,94,-269,114,-269,9,-269,10,-269,121,-269,104,-269,86,-269,92,-269,95,-269,30,-269,98,-269,12,-269,93,-269,29,-269,81,-269,80,-269,2,-269,79,-269,78,-269,77,-269,76,-269,131,-269});
    states[559] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,560,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[560] = new State(-268);
    states[561] = new State(-258);
    states[562] = new State(new int[]{55,563});
    states[563] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,564,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[564] = new State(-259);
    states[565] = new State(new int[]{21,407,45,415,46,554,31,558,71,562},new int[]{-272,566,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561});
    states[566] = new State(-253);
    states[567] = new State(-217);
    states[568] = new State(-271);
    states[569] = new State(-272);
    states[570] = new State(new int[]{8,572,115,-452,94,-452,114,-452,9,-452,10,-452,121,-452,104,-452,86,-452,92,-452,95,-452,30,-452,98,-452,12,-452,93,-452,29,-452,81,-452,80,-452,2,-452,79,-452,78,-452,77,-452,76,-452,131,-452},new int[]{-117,571});
    states[571] = new State(-273);
    states[572] = new State(new int[]{9,573,11,672,137,-197,80,-197,81,-197,75,-197,73,-197,50,-197,26,-197,102,-197},new int[]{-118,574,-52,1034,-6,578,-240,1033});
    states[573] = new State(-453);
    states[574] = new State(new int[]{9,575,10,576});
    states[575] = new State(-454);
    states[576] = new State(new int[]{11,672,137,-197,80,-197,81,-197,75,-197,73,-197,50,-197,26,-197,102,-197},new int[]{-52,577,-6,578,-240,1033});
    states[577] = new State(-456);
    states[578] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,50,656,26,662,102,668,11,672},new int[]{-286,579,-240,431,-148,580,-124,655,-136,654,-140,24,-141,27});
    states[579] = new State(-457);
    states[580] = new State(new int[]{5,581,94,652});
    states[581] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,582,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[582] = new State(new int[]{104,583,9,-458,10,-458});
    states[583] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,584,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[584] = new State(-462);
    states[585] = new State(-721);
    states[586] = new State(new int[]{8,587,132,-709,130,-709,112,-709,111,-709,125,-709,126,-709,127,-709,128,-709,124,-709,110,-709,109,-709,122,-709,123,-709,120,-709,6,-709,5,-709,114,-709,119,-709,117,-709,115,-709,118,-709,116,-709,131,-709,16,-709,86,-709,10,-709,92,-709,95,-709,30,-709,98,-709,94,-709,12,-709,9,-709,93,-709,29,-709,81,-709,80,-709,2,-709,79,-709,78,-709,77,-709,76,-709,13,-709,48,-709,55,-709,135,-709,137,-709,75,-709,73,-709,42,-709,39,-709,18,-709,19,-709,138,-709,140,-709,139,-709,148,-709,150,-709,149,-709,54,-709,85,-709,37,-709,22,-709,91,-709,51,-709,32,-709,52,-709,96,-709,44,-709,33,-709,50,-709,57,-709,72,-709,70,-709,35,-709,68,-709,69,-709});
    states[587] = new State(new int[]{14,592,138,148,140,149,139,151,148,153,150,154,149,155,50,594,137,23,80,25,81,26,75,28,73,29,11,334,8,611},new int[]{-342,588,-340,626,-14,593,-154,145,-156,146,-155,150,-15,152,-329,602,-274,603,-170,161,-136,195,-140,24,-141,27,-332,609,-333,610});
    states[588] = new State(new int[]{9,589,10,590,94,607});
    states[589] = new State(-620);
    states[590] = new State(new int[]{14,592,138,148,140,149,139,151,148,153,150,154,149,155,50,594,137,23,80,25,81,26,75,28,73,29,11,334,8,611},new int[]{-340,591,-14,593,-154,145,-156,146,-155,150,-15,152,-329,602,-274,603,-170,161,-136,195,-140,24,-141,27,-332,609,-333,610});
    states[591] = new State(-659);
    states[592] = new State(-661);
    states[593] = new State(-662);
    states[594] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,595,-140,24,-141,27});
    states[595] = new State(new int[]{5,596,9,-664,10,-664,94,-664});
    states[596] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,597,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[597] = new State(-663);
    states[598] = new State(new int[]{8,572,5,-452},new int[]{-117,599});
    states[599] = new State(new int[]{5,600});
    states[600] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,601,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[601] = new State(-274);
    states[602] = new State(-665);
    states[603] = new State(new int[]{8,604});
    states[604] = new State(new int[]{14,592,138,148,140,149,139,151,148,153,150,154,149,155,50,594,137,23,80,25,81,26,75,28,73,29,11,334,8,611},new int[]{-342,605,-340,626,-14,593,-154,145,-156,146,-155,150,-15,152,-329,602,-274,603,-170,161,-136,195,-140,24,-141,27,-332,609,-333,610});
    states[605] = new State(new int[]{9,606,10,590,94,607});
    states[606] = new State(-623);
    states[607] = new State(new int[]{14,592,138,148,140,149,139,151,148,153,150,154,149,155,50,594,137,23,80,25,81,26,75,28,73,29,11,334,8,611},new int[]{-340,608,-14,593,-154,145,-156,146,-155,150,-15,152,-329,602,-274,603,-170,161,-136,195,-140,24,-141,27,-332,609,-333,610});
    states[608] = new State(-660);
    states[609] = new State(-666);
    states[610] = new State(-667);
    states[611] = new State(new int[]{14,616,138,148,140,149,139,151,148,153,150,154,149,155,110,355,109,356,50,620,137,23,80,25,81,26,75,28,73,29,11,334,8,611},new int[]{-345,612,-335,625,-14,617,-154,145,-156,146,-155,150,-15,152,-189,618,-331,622,-274,345,-170,161,-136,195,-140,24,-141,27,-332,623,-333,624});
    states[612] = new State(new int[]{9,613,94,614});
    states[613] = new State(-645);
    states[614] = new State(new int[]{14,616,138,148,140,149,139,151,148,153,150,154,149,155,110,355,109,356,50,620,137,23,80,25,81,26,75,28,73,29,11,334,8,611},new int[]{-335,615,-14,617,-154,145,-156,146,-155,150,-15,152,-189,618,-331,622,-274,345,-170,161,-136,195,-140,24,-141,27,-332,623,-333,624});
    states[615] = new State(-654);
    states[616] = new State(-646);
    states[617] = new State(-647);
    states[618] = new State(new int[]{138,148,140,149,139,151,148,153,150,154,149,155},new int[]{-14,619,-154,145,-156,146,-155,150,-15,152});
    states[619] = new State(-648);
    states[620] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,621,-140,24,-141,27});
    states[621] = new State(-649);
    states[622] = new State(-650);
    states[623] = new State(-651);
    states[624] = new State(-652);
    states[625] = new State(-653);
    states[626] = new State(-658);
    states[627] = new State(-710);
    states[628] = new State(new int[]{86,-583,10,-583,92,-583,95,-583,30,-583,98,-583,94,-583,12,-583,9,-583,93,-583,29,-583,81,-583,80,-583,2,-583,79,-583,78,-583,77,-583,76,-583,6,-583,5,-583,48,-583,55,-583,135,-583,137,-583,75,-583,73,-583,42,-583,39,-583,8,-583,18,-583,19,-583,138,-583,140,-583,139,-583,148,-583,150,-583,149,-583,54,-583,85,-583,37,-583,22,-583,91,-583,51,-583,32,-583,52,-583,96,-583,44,-583,33,-583,50,-583,57,-583,72,-583,70,-583,35,-583,68,-583,69,-583,13,-586});
    states[629] = new State(new int[]{13,630});
    states[630] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251},new int[]{-106,631,-91,634,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,635});
    states[631] = new State(new int[]{5,632,13,630});
    states[632] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251},new int[]{-106,633,-91,634,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,635});
    states[633] = new State(new int[]{13,630,86,-599,10,-599,92,-599,95,-599,30,-599,98,-599,94,-599,12,-599,9,-599,93,-599,29,-599,81,-599,80,-599,2,-599,79,-599,78,-599,77,-599,76,-599,6,-599,5,-599,48,-599,55,-599,135,-599,137,-599,75,-599,73,-599,42,-599,39,-599,8,-599,18,-599,19,-599,138,-599,140,-599,139,-599,148,-599,150,-599,149,-599,54,-599,85,-599,37,-599,22,-599,91,-599,51,-599,32,-599,52,-599,96,-599,44,-599,33,-599,50,-599,57,-599,72,-599,70,-599,35,-599,68,-599,69,-599});
    states[634] = new State(new int[]{16,129,5,-585,13,-585,86,-585,10,-585,92,-585,95,-585,30,-585,98,-585,94,-585,12,-585,9,-585,93,-585,29,-585,81,-585,80,-585,2,-585,79,-585,78,-585,77,-585,76,-585,6,-585,48,-585,55,-585,135,-585,137,-585,75,-585,73,-585,42,-585,39,-585,8,-585,18,-585,19,-585,138,-585,140,-585,139,-585,148,-585,150,-585,149,-585,54,-585,85,-585,37,-585,22,-585,91,-585,51,-585,32,-585,52,-585,96,-585,44,-585,33,-585,50,-585,57,-585,72,-585,70,-585,35,-585,68,-585,69,-585});
    states[635] = new State(-586);
    states[636] = new State(-584);
    states[637] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-107,638,-91,643,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-232,644});
    states[638] = new State(new int[]{48,639});
    states[639] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-107,640,-91,643,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-232,644});
    states[640] = new State(new int[]{29,641});
    states[641] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-107,642,-91,643,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-232,644});
    states[642] = new State(-600);
    states[643] = new State(new int[]{16,129,48,-587,29,-587,114,-587,119,-587,117,-587,115,-587,118,-587,116,-587,131,-587,86,-587,10,-587,92,-587,95,-587,30,-587,98,-587,94,-587,12,-587,9,-587,93,-587,81,-587,80,-587,2,-587,79,-587,78,-587,77,-587,76,-587,13,-587,6,-587,5,-587,55,-587,135,-587,137,-587,75,-587,73,-587,42,-587,39,-587,8,-587,18,-587,19,-587,138,-587,140,-587,139,-587,148,-587,150,-587,149,-587,54,-587,85,-587,37,-587,22,-587,91,-587,51,-587,32,-587,52,-587,96,-587,44,-587,33,-587,50,-587,57,-587,72,-587,70,-587,35,-587,68,-587,69,-587,110,-587,109,-587,122,-587,123,-587,120,-587,132,-587,130,-587,112,-587,111,-587,125,-587,126,-587,127,-587,128,-587,124,-587});
    states[644] = new State(-588);
    states[645] = new State(-581);
    states[646] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,5,-679,86,-679,10,-679,92,-679,95,-679,30,-679,98,-679,94,-679,12,-679,9,-679,93,-679,29,-679,2,-679,79,-679,78,-679,77,-679,76,-679,6,-679},new int[]{-104,647,-95,651,-77,302,-76,308,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,650,-257,627});
    states[647] = new State(new int[]{5,648,86,-683,10,-683,92,-683,95,-683,30,-683,98,-683,94,-683,12,-683,9,-683,93,-683,29,-683,81,-683,80,-683,2,-683,79,-683,78,-683,77,-683,76,-683,6,-683});
    states[648] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251},new int[]{-95,649,-77,302,-76,308,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,650,-257,627});
    states[649] = new State(new int[]{6,133,86,-685,10,-685,92,-685,95,-685,30,-685,98,-685,94,-685,12,-685,9,-685,93,-685,29,-685,81,-685,80,-685,2,-685,79,-685,78,-685,77,-685,76,-685});
    states[650] = new State(-709);
    states[651] = new State(new int[]{6,133,5,-678,86,-678,10,-678,92,-678,95,-678,30,-678,98,-678,94,-678,12,-678,9,-678,93,-678,29,-678,81,-678,80,-678,2,-678,79,-678,78,-678,77,-678,76,-678});
    states[652] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-124,653,-136,654,-140,24,-141,27});
    states[653] = new State(-466);
    states[654] = new State(-467);
    states[655] = new State(-465);
    states[656] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-148,657,-124,655,-136,654,-140,24,-141,27});
    states[657] = new State(new int[]{5,658,94,652});
    states[658] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,659,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[659] = new State(new int[]{104,660,9,-459,10,-459});
    states[660] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,661,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[661] = new State(-463);
    states[662] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-148,663,-124,655,-136,654,-140,24,-141,27});
    states[663] = new State(new int[]{5,664,94,652});
    states[664] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,665,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[665] = new State(new int[]{104,666,9,-460,10,-460});
    states[666] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,667,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[667] = new State(-464);
    states[668] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-148,669,-124,655,-136,654,-140,24,-141,27});
    states[669] = new State(new int[]{5,670,94,652});
    states[670] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,671,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[671] = new State(-461);
    states[672] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-241,673,-8,1032,-9,677,-170,678,-136,1027,-140,24,-141,27,-291,1030});
    states[673] = new State(new int[]{12,674,94,675});
    states[674] = new State(-198);
    states[675] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-8,676,-9,677,-170,678,-136,1027,-140,24,-141,27,-291,1030});
    states[676] = new State(-200);
    states[677] = new State(-201);
    states[678] = new State(new int[]{7,162,8,681,117,167,12,-615,94,-615},new int[]{-65,679,-289,680});
    states[679] = new State(-749);
    states[680] = new State(-219);
    states[681] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,5,646,34,686,41,692,9,-772},new int[]{-63,682,-66,461,-83,522,-82,126,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645,-311,684,-312,685});
    states[682] = new State(new int[]{9,683});
    states[683] = new State(-616);
    states[684] = new State(-579);
    states[685] = new State(-927);
    states[686] = new State(new int[]{8,1017,5,551,121,-940},new int[]{-313,687});
    states[687] = new State(new int[]{121,688});
    states[688] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,689,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[689] = new State(-931);
    states[690] = new State(new int[]{86,-592,10,-592,92,-592,95,-592,30,-592,98,-592,94,-592,12,-592,9,-592,93,-592,29,-592,81,-592,80,-592,2,-592,79,-592,78,-592,77,-592,76,-592,13,-586});
    states[691] = new State(-593);
    states[692] = new State(new int[]{121,693,8,1008});
    states[693] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,712,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-318,694,-202,695,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-4,722,-319,723,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[694] = new State(-934);
    states[695] = new State(-958);
    states[696] = new State(new int[]{17,697,8,458,7,702,136,704,4,705,15,707,104,-745,105,-745,106,-745,107,-745,108,-745,86,-745,10,-745,92,-745,95,-745,30,-745,98,-745,94,-745,12,-745,9,-745,93,-745,29,-745,81,-745,80,-745,2,-745,79,-745,78,-745,77,-745,76,-745,132,-745,130,-745,112,-745,111,-745,125,-745,126,-745,127,-745,128,-745,124,-745,110,-745,109,-745,122,-745,123,-745,120,-745,6,-745,5,-745,114,-745,119,-745,117,-745,115,-745,118,-745,116,-745,131,-745,16,-745,13,-745,113,-745,11,-755});
    states[697] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,5,646},new int[]{-109,698,-95,448,-77,302,-76,308,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,650,-257,627});
    states[698] = new State(new int[]{12,699});
    states[699] = new State(new int[]{104,495,105,496,106,497,107,498,108,499,17,-765,8,-765,7,-765,136,-765,4,-765,15,-765,86,-765,10,-765,11,-765,92,-765,95,-765,30,-765,98,-765,94,-765,12,-765,9,-765,93,-765,29,-765,81,-765,80,-765,2,-765,79,-765,78,-765,77,-765,76,-765,132,-765,130,-765,112,-765,111,-765,125,-765,126,-765,127,-765,128,-765,124,-765,110,-765,109,-765,122,-765,123,-765,120,-765,6,-765,5,-765,114,-765,119,-765,117,-765,115,-765,118,-765,116,-765,131,-765,16,-765,13,-765,113,-765},new int[]{-184,700});
    states[700] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,701,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[701] = new State(-506);
    states[702] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,86,64,30,65,31,66,24,67,98,68,95,69,32,70,33,71,34,72,37,73,38,74,39,75,97,76,40,77,41,78,43,79,44,80,45,81,91,82,46,83,96,84,47,85,25,86,48,87,68,88,92,89,49,90,50,91,51,92,52,93,53,94,54,95,55,96,56,97,58,98,99,99,100,100,103,101,101,102,102,103,59,104,72,105,35,106,36,107,42,471},new int[]{-137,703,-136,526,-140,24,-141,27,-283,527,-139,31,-181,528});
    states[703] = new State(-767);
    states[704] = new State(-769);
    states[705] = new State(new int[]{117,167},new int[]{-289,706});
    states[706] = new State(-770);
    states[707] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,536,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155},new int[]{-101,708,-105,709,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711});
    states[708] = new State(new int[]{17,445,8,458,7,702,136,704,4,705,15,707,104,-742,105,-742,106,-742,107,-742,108,-742,86,-742,10,-742,92,-742,95,-742,30,-742,98,-742,132,-742,130,-742,112,-742,111,-742,125,-742,126,-742,127,-742,128,-742,124,-742,110,-742,109,-742,122,-742,123,-742,120,-742,6,-742,5,-742,114,-742,119,-742,117,-742,115,-742,118,-742,116,-742,131,-742,16,-742,94,-742,12,-742,9,-742,93,-742,29,-742,81,-742,80,-742,2,-742,79,-742,78,-742,77,-742,76,-742,13,-742,113,-742,48,-742,55,-742,135,-742,137,-742,75,-742,73,-742,42,-742,39,-742,18,-742,19,-742,138,-742,140,-742,139,-742,148,-742,150,-742,149,-742,54,-742,85,-742,37,-742,22,-742,91,-742,51,-742,32,-742,52,-742,96,-742,44,-742,33,-742,50,-742,57,-742,72,-742,70,-742,35,-742,68,-742,69,-742,11,-755});
    states[709] = new State(-743);
    states[710] = new State(new int[]{7,143,11,-756});
    states[711] = new State(new int[]{7,524});
    states[712] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,504,-92,537,-101,713,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[713] = new State(new int[]{94,714,17,445,8,458,7,702,136,704,4,705,15,707,132,-745,130,-745,112,-745,111,-745,125,-745,126,-745,127,-745,128,-745,124,-745,110,-745,109,-745,122,-745,123,-745,120,-745,6,-745,5,-745,114,-745,119,-745,117,-745,115,-745,118,-745,116,-745,131,-745,16,-745,9,-745,13,-745,113,-745,11,-755});
    states[714] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,536,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155},new int[]{-325,715,-101,721,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711});
    states[715] = new State(new int[]{9,716,94,719});
    states[716] = new State(new int[]{104,495,105,496,106,497,107,498,108,499},new int[]{-184,717});
    states[717] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,718,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[718] = new State(-505);
    states[719] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,536,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155},new int[]{-101,720,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711});
    states[720] = new State(new int[]{17,445,8,458,7,702,136,704,4,705,9,-508,94,-508,11,-755});
    states[721] = new State(new int[]{17,445,8,458,7,702,136,704,4,705,9,-507,94,-507,11,-755});
    states[722] = new State(-959);
    states[723] = new State(-960);
    states[724] = new State(-944);
    states[725] = new State(-945);
    states[726] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,727,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[727] = new State(new int[]{48,728});
    states[728] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,94,-476,12,-476,9,-476,93,-476,29,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-250,729,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[729] = new State(new int[]{29,730,86,-516,10,-516,92,-516,95,-516,30,-516,98,-516,94,-516,12,-516,9,-516,93,-516,81,-516,80,-516,2,-516,79,-516,78,-516,77,-516,76,-516});
    states[730] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,94,-476,12,-476,9,-476,93,-476,29,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-250,731,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[731] = new State(-517);
    states[732] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,86,-556,10,-556,92,-556,95,-556,30,-556,98,-556,94,-556,12,-556,9,-556,93,-556,29,-556,2,-556,79,-556,78,-556,77,-556,76,-556},new int[]{-136,502,-140,24,-141,27});
    states[733] = new State(new int[]{50,734,53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,504,-92,537,-101,713,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[734] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,735,-140,24,-141,27});
    states[735] = new State(new int[]{94,736});
    states[736] = new State(new int[]{50,744},new int[]{-326,737});
    states[737] = new State(new int[]{9,738,94,741});
    states[738] = new State(new int[]{104,739});
    states[739] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,740,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[740] = new State(-502);
    states[741] = new State(new int[]{50,742});
    states[742] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,743,-140,24,-141,27});
    states[743] = new State(-510);
    states[744] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,745,-140,24,-141,27});
    states[745] = new State(-509);
    states[746] = new State(-478);
    states[747] = new State(-479);
    states[748] = new State(new int[]{148,750,137,23,80,25,81,26,75,28,73,29},new int[]{-132,749,-136,751,-140,24,-141,27});
    states[749] = new State(-512);
    states[750] = new State(-92);
    states[751] = new State(-93);
    states[752] = new State(-480);
    states[753] = new State(-481);
    states[754] = new State(-482);
    states[755] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,756,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[756] = new State(new int[]{55,757});
    states[757] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356,29,765,86,-536},new int[]{-33,758,-243,1005,-252,1007,-68,998,-100,1004,-87,1003,-84,185,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[758] = new State(new int[]{10,761,29,765,86,-536},new int[]{-243,759});
    states[759] = new State(new int[]{86,760});
    states[760] = new State(-527);
    states[761] = new State(new int[]{29,765,137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356,86,-536},new int[]{-243,762,-252,764,-68,998,-100,1004,-87,1003,-84,185,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[762] = new State(new int[]{86,763});
    states[763] = new State(-528);
    states[764] = new State(-531);
    states[765] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476,86,-476},new int[]{-242,766,-251,767,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[766] = new State(new int[]{10,119,86,-537});
    states[767] = new State(-514);
    states[768] = new State(new int[]{17,-757,8,-757,7,-757,136,-757,4,-757,15,-757,104,-757,105,-757,106,-757,107,-757,108,-757,86,-757,10,-757,11,-757,92,-757,95,-757,30,-757,98,-757,5,-93});
    states[769] = new State(new int[]{7,-176,11,-176,5,-92});
    states[770] = new State(-483);
    states[771] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,92,-476,10,-476},new int[]{-242,772,-251,767,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[772] = new State(new int[]{92,773,10,119});
    states[773] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,774,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[774] = new State(-538);
    states[775] = new State(-484);
    states[776] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,777,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[777] = new State(new int[]{93,990,135,-541,137,-541,80,-541,81,-541,75,-541,73,-541,42,-541,39,-541,8,-541,18,-541,19,-541,138,-541,140,-541,139,-541,148,-541,150,-541,149,-541,54,-541,85,-541,37,-541,22,-541,91,-541,51,-541,32,-541,52,-541,96,-541,44,-541,33,-541,50,-541,57,-541,72,-541,70,-541,35,-541,86,-541,10,-541,92,-541,95,-541,30,-541,98,-541,94,-541,12,-541,9,-541,29,-541,2,-541,79,-541,78,-541,77,-541,76,-541},new int[]{-282,778});
    states[778] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,94,-476,12,-476,9,-476,93,-476,29,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-250,779,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[779] = new State(-539);
    states[780] = new State(-485);
    states[781] = new State(new int[]{50,997,137,-550,80,-550,81,-550,75,-550,73,-550},new int[]{-18,782});
    states[782] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,783,-140,24,-141,27});
    states[783] = new State(new int[]{104,993,5,994},new int[]{-276,784});
    states[784] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,785,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[785] = new State(new int[]{68,991,69,992},new int[]{-108,786});
    states[786] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,787,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[787] = new State(new int[]{93,990,135,-541,137,-541,80,-541,81,-541,75,-541,73,-541,42,-541,39,-541,8,-541,18,-541,19,-541,138,-541,140,-541,139,-541,148,-541,150,-541,149,-541,54,-541,85,-541,37,-541,22,-541,91,-541,51,-541,32,-541,52,-541,96,-541,44,-541,33,-541,50,-541,57,-541,72,-541,70,-541,35,-541,86,-541,10,-541,92,-541,95,-541,30,-541,98,-541,94,-541,12,-541,9,-541,29,-541,2,-541,79,-541,78,-541,77,-541,76,-541},new int[]{-282,788});
    states[788] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,94,-476,12,-476,9,-476,93,-476,29,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-250,789,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[789] = new State(-548);
    states[790] = new State(-486);
    states[791] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,5,646,34,686,41,692},new int[]{-66,792,-83,522,-82,126,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645,-311,684,-312,685});
    states[792] = new State(new int[]{93,793,94,462});
    states[793] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,94,-476,12,-476,9,-476,93,-476,29,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-250,794,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[794] = new State(-555);
    states[795] = new State(-487);
    states[796] = new State(-488);
    states[797] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476,95,-476,30,-476},new int[]{-242,798,-251,767,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[798] = new State(new int[]{10,119,95,800,30,968},new int[]{-280,799});
    states[799] = new State(-557);
    states[800] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476},new int[]{-242,801,-251,767,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[801] = new State(new int[]{86,802,10,119});
    states[802] = new State(-558);
    states[803] = new State(-489);
    states[804] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646,86,-572,10,-572,92,-572,95,-572,30,-572,98,-572,94,-572,12,-572,9,-572,93,-572,29,-572,2,-572,79,-572,78,-572,77,-572,76,-572},new int[]{-82,805,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[805] = new State(-573);
    states[806] = new State(-490);
    states[807] = new State(new int[]{50,953,137,23,80,25,81,26,75,28,73,29},new int[]{-136,808,-140,24,-141,27});
    states[808] = new State(new int[]{5,951,131,-547},new int[]{-264,809});
    states[809] = new State(new int[]{131,810});
    states[810] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,811,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[811] = new State(new int[]{93,812});
    states[812] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,94,-476,12,-476,9,-476,93,-476,29,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-250,813,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[813] = new State(-543);
    states[814] = new State(-491);
    states[815] = new State(new int[]{8,817,137,23,80,25,81,26,75,28,73,29},new int[]{-300,816,-147,825,-136,824,-140,24,-141,27});
    states[816] = new State(-501);
    states[817] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,818,-140,24,-141,27});
    states[818] = new State(new int[]{94,819});
    states[819] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-147,820,-136,824,-140,24,-141,27});
    states[820] = new State(new int[]{9,821,94,549});
    states[821] = new State(new int[]{104,822});
    states[822] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,823,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[823] = new State(-503);
    states[824] = new State(-330);
    states[825] = new State(new int[]{5,826,94,549,104,949});
    states[826] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,827,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[827] = new State(new int[]{104,947,114,948,86,-396,10,-396,92,-396,95,-396,30,-396,98,-396,94,-396,12,-396,9,-396,93,-396,29,-396,81,-396,80,-396,2,-396,79,-396,78,-396,77,-396,76,-396},new int[]{-327,828});
    states[828] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,918,129,377,110,355,109,356,60,157,34,686,41,692},new int[]{-81,829,-80,830,-79,239,-84,240,-75,190,-12,214,-10,224,-13,200,-136,831,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384,-88,935,-233,936,-53,937,-312,946});
    states[829] = new State(-398);
    states[830] = new State(-399);
    states[831] = new State(new int[]{121,832,4,-155,11,-155,7,-155,136,-155,8,-155,113,-155,130,-155,132,-155,112,-155,111,-155,125,-155,126,-155,127,-155,128,-155,124,-155,110,-155,109,-155,122,-155,123,-155,114,-155,119,-155,117,-155,115,-155,118,-155,116,-155,131,-155,13,-155,86,-155,10,-155,92,-155,95,-155,30,-155,98,-155,94,-155,12,-155,9,-155,93,-155,29,-155,81,-155,80,-155,2,-155,79,-155,78,-155,77,-155,76,-155});
    states[832] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,833,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[833] = new State(-401);
    states[834] = new State(-957);
    states[835] = new State(-946);
    states[836] = new State(-947);
    states[837] = new State(-948);
    states[838] = new State(-949);
    states[839] = new State(-950);
    states[840] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,841,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[841] = new State(new int[]{93,842});
    states[842] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,94,-476,12,-476,9,-476,93,-476,29,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-250,843,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[843] = new State(-498);
    states[844] = new State(-492);
    states[845] = new State(-576);
    states[846] = new State(-577);
    states[847] = new State(-493);
    states[848] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,849,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[849] = new State(new int[]{93,850});
    states[850] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,94,-476,12,-476,9,-476,93,-476,29,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-250,851,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[851] = new State(-542);
    states[852] = new State(-494);
    states[853] = new State(new int[]{71,855,53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,854,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[854] = new State(-499);
    states[855] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,856,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[856] = new State(-500);
    states[857] = new State(-495);
    states[858] = new State(-496);
    states[859] = new State(-497);
    states[860] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,861,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[861] = new State(new int[]{52,862});
    states[862] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,138,148,140,149,139,151,148,153,150,154,149,155,53,897,18,246,19,251,11,334,8,611},new int[]{-339,863,-338,911,-331,870,-274,875,-170,161,-136,195,-140,24,-141,27,-330,889,-346,892,-328,900,-14,895,-154,145,-156,146,-155,150,-15,152,-247,898,-285,899,-332,901,-333,904});
    states[863] = new State(new int[]{10,866,29,765,86,-536},new int[]{-243,864});
    states[864] = new State(new int[]{86,865});
    states[865] = new State(-518);
    states[866] = new State(new int[]{29,765,137,23,80,25,81,26,75,28,73,29,138,148,140,149,139,151,148,153,150,154,149,155,53,897,18,246,19,251,11,334,8,611,86,-536},new int[]{-243,867,-338,869,-331,870,-274,875,-170,161,-136,195,-140,24,-141,27,-330,889,-346,892,-328,900,-14,895,-154,145,-156,146,-155,150,-15,152,-247,898,-285,899,-332,901,-333,904});
    states[867] = new State(new int[]{86,868});
    states[868] = new State(-519);
    states[869] = new State(-521);
    states[870] = new State(new int[]{36,871});
    states[871] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,872,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[872] = new State(new int[]{5,873});
    states[873] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476,29,-476,86,-476},new int[]{-250,874,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[874] = new State(-522);
    states[875] = new State(new int[]{8,876,94,-630,5,-630});
    states[876] = new State(new int[]{14,351,138,148,140,149,139,151,148,153,150,154,149,155,110,355,109,356,137,23,80,25,81,26,75,28,73,29,50,881,11,334,8,611},new int[]{-343,877,-341,888,-14,352,-154,145,-156,146,-155,150,-15,152,-189,353,-136,357,-140,24,-141,27,-331,885,-274,345,-170,161,-332,886,-333,887});
    states[877] = new State(new int[]{9,878,10,349,94,879});
    states[878] = new State(new int[]{36,-624,5,-625});
    states[879] = new State(new int[]{14,351,138,148,140,149,139,151,148,153,150,154,149,155,110,355,109,356,137,23,80,25,81,26,75,28,73,29,50,881,11,334,8,611},new int[]{-341,880,-14,352,-154,145,-156,146,-155,150,-15,152,-189,353,-136,357,-140,24,-141,27,-331,885,-274,345,-170,161,-332,886,-333,887});
    states[880] = new State(-657);
    states[881] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,882,-140,24,-141,27});
    states[882] = new State(new int[]{5,883,9,-674,10,-674,94,-674});
    states[883] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,884,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[884] = new State(-673);
    states[885] = new State(-675);
    states[886] = new State(-676);
    states[887] = new State(-677);
    states[888] = new State(-655);
    states[889] = new State(new int[]{5,890});
    states[890] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476,29,-476,86,-476},new int[]{-250,891,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[891] = new State(-523);
    states[892] = new State(new int[]{94,893,5,-626});
    states[893] = new State(new int[]{138,148,140,149,139,151,148,153,150,154,149,155,137,23,80,25,81,26,75,28,73,29,53,897,18,246,19,251},new int[]{-328,894,-14,895,-154,145,-156,146,-155,150,-15,152,-274,896,-170,161,-136,195,-140,24,-141,27,-247,898,-285,899});
    states[894] = new State(-628);
    states[895] = new State(-629);
    states[896] = new State(-630);
    states[897] = new State(-631);
    states[898] = new State(-632);
    states[899] = new State(-633);
    states[900] = new State(-627);
    states[901] = new State(new int[]{5,902});
    states[902] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476,29,-476,86,-476},new int[]{-250,903,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[903] = new State(-524);
    states[904] = new State(new int[]{36,905,5,909});
    states[905] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,906,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[906] = new State(new int[]{5,907});
    states[907] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476,29,-476,86,-476},new int[]{-250,908,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[908] = new State(-525);
    states[909] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476,29,-476,86,-476},new int[]{-250,910,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[910] = new State(-526);
    states[911] = new State(-520);
    states[912] = new State(-951);
    states[913] = new State(-952);
    states[914] = new State(-953);
    states[915] = new State(-954);
    states[916] = new State(-955);
    states[917] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,854,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[918] = new State(new int[]{9,926,137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,931,129,377,110,355,109,356,60,157},new int[]{-84,919,-62,920,-235,924,-75,190,-12,214,-10,224,-13,200,-136,930,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384,-61,236,-80,934,-79,239,-88,935,-233,936,-53,937,-234,938,-236,945,-125,941});
    states[919] = new State(new int[]{9,376,13,186,94,-179});
    states[920] = new State(new int[]{9,921});
    states[921] = new State(new int[]{121,922,86,-182,10,-182,92,-182,95,-182,30,-182,98,-182,94,-182,12,-182,9,-182,93,-182,29,-182,81,-182,80,-182,2,-182,79,-182,78,-182,77,-182,76,-182});
    states[922] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,923,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[923] = new State(-403);
    states[924] = new State(new int[]{9,925});
    states[925] = new State(-187);
    states[926] = new State(new int[]{5,551,121,-940},new int[]{-313,927});
    states[927] = new State(new int[]{121,928});
    states[928] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,929,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[929] = new State(-402);
    states[930] = new State(new int[]{4,-155,11,-155,7,-155,136,-155,8,-155,113,-155,130,-155,132,-155,112,-155,111,-155,125,-155,126,-155,127,-155,128,-155,124,-155,110,-155,109,-155,122,-155,123,-155,114,-155,119,-155,117,-155,115,-155,118,-155,116,-155,131,-155,9,-155,13,-155,94,-155,5,-193});
    states[931] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,931,129,377,110,355,109,356,60,157,9,-183},new int[]{-84,919,-62,932,-235,924,-75,190,-12,214,-10,224,-13,200,-136,930,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384,-61,236,-80,934,-79,239,-88,935,-233,936,-53,937,-234,938,-236,945,-125,941});
    states[932] = new State(new int[]{9,933});
    states[933] = new State(-182);
    states[934] = new State(-185);
    states[935] = new State(-180);
    states[936] = new State(-181);
    states[937] = new State(-405);
    states[938] = new State(new int[]{10,939,9,-188});
    states[939] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,9,-189},new int[]{-236,940,-125,941,-136,944,-140,24,-141,27});
    states[940] = new State(-191);
    states[941] = new State(new int[]{5,942});
    states[942] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,931,129,377,110,355,109,356},new int[]{-79,943,-84,240,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384,-88,935,-233,936});
    states[943] = new State(-192);
    states[944] = new State(-193);
    states[945] = new State(-190);
    states[946] = new State(-400);
    states[947] = new State(-394);
    states[948] = new State(-395);
    states[949] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,950,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[950] = new State(-397);
    states[951] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,952,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[952] = new State(-546);
    states[953] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,954,-140,24,-141,27});
    states[954] = new State(new int[]{5,955,131,961});
    states[955] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,956,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[956] = new State(new int[]{131,957});
    states[957] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,958,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[958] = new State(new int[]{93,959});
    states[959] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,94,-476,12,-476,9,-476,93,-476,29,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-250,960,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[960] = new State(-544);
    states[961] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,962,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[962] = new State(new int[]{93,963});
    states[963] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476,94,-476,12,-476,9,-476,93,-476,29,-476,2,-476,79,-476,78,-476,77,-476,76,-476},new int[]{-250,964,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[964] = new State(-545);
    states[965] = new State(new int[]{5,966});
    states[966] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476,92,-476,95,-476,30,-476,98,-476},new int[]{-251,967,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[967] = new State(-475);
    states[968] = new State(new int[]{74,976,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476,86,-476},new int[]{-56,969,-59,971,-58,988,-242,989,-251,767,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[969] = new State(new int[]{86,970});
    states[970] = new State(-559);
    states[971] = new State(new int[]{10,973,29,986,86,-565},new int[]{-244,972});
    states[972] = new State(-560);
    states[973] = new State(new int[]{74,976,29,986,86,-565},new int[]{-58,974,-244,975});
    states[974] = new State(-564);
    states[975] = new State(-561);
    states[976] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-60,977,-169,980,-170,981,-136,982,-140,24,-141,27,-129,983});
    states[977] = new State(new int[]{93,978});
    states[978] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476,29,-476,86,-476},new int[]{-250,979,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[979] = new State(-567);
    states[980] = new State(-568);
    states[981] = new State(new int[]{7,162,93,-570});
    states[982] = new State(new int[]{7,-245,93,-245,5,-571});
    states[983] = new State(new int[]{5,984});
    states[984] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-169,985,-170,981,-136,195,-140,24,-141,27});
    states[985] = new State(-569);
    states[986] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476,86,-476},new int[]{-242,987,-251,767,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[987] = new State(new int[]{10,119,86,-566});
    states[988] = new State(-563);
    states[989] = new State(new int[]{10,119,86,-562});
    states[990] = new State(-540);
    states[991] = new State(-553);
    states[992] = new State(-554);
    states[993] = new State(-551);
    states[994] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-170,995,-136,195,-140,24,-141,27});
    states[995] = new State(new int[]{104,996,7,162});
    states[996] = new State(-552);
    states[997] = new State(-549);
    states[998] = new State(new int[]{5,999,94,1001});
    states[999] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476,29,-476,86,-476},new int[]{-250,1000,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[1000] = new State(-532);
    states[1001] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-100,1002,-87,1003,-84,185,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[1002] = new State(-534);
    states[1003] = new State(-535);
    states[1004] = new State(-533);
    states[1005] = new State(new int[]{86,1006});
    states[1006] = new State(-529);
    states[1007] = new State(-530);
    states[1008] = new State(new int[]{9,1009,137,23,80,25,81,26,75,28,73,29},new int[]{-315,1012,-316,1016,-147,547,-136,824,-140,24,-141,27});
    states[1009] = new State(new int[]{121,1010});
    states[1010] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,712,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-318,1011,-202,695,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-4,722,-319,723,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[1011] = new State(-935);
    states[1012] = new State(new int[]{9,1013,10,545});
    states[1013] = new State(new int[]{121,1014});
    states[1014] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,29,42,471,39,501,8,712,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-318,1015,-202,695,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-4,722,-319,723,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[1015] = new State(-936);
    states[1016] = new State(-937);
    states[1017] = new State(new int[]{9,1018,137,23,80,25,81,26,75,28,73,29},new int[]{-315,1022,-316,1016,-147,547,-136,824,-140,24,-141,27});
    states[1018] = new State(new int[]{5,551,121,-940},new int[]{-313,1019});
    states[1019] = new State(new int[]{121,1020});
    states[1020] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,1021,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[1021] = new State(-932);
    states[1022] = new State(new int[]{9,1023,10,545});
    states[1023] = new State(new int[]{5,551,121,-940},new int[]{-313,1024});
    states[1024] = new State(new int[]{121,1025});
    states[1025] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,1026,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[1026] = new State(-933);
    states[1027] = new State(new int[]{5,1028,7,-245,8,-245,117,-245,12,-245,94,-245});
    states[1028] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-9,1029,-170,678,-136,195,-140,24,-141,27,-291,1030});
    states[1029] = new State(-202);
    states[1030] = new State(new int[]{8,681,12,-615,94,-615},new int[]{-65,1031});
    states[1031] = new State(-750);
    states[1032] = new State(-199);
    states[1033] = new State(-195);
    states[1034] = new State(-455);
    states[1035] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,1036,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[1036] = new State(-112);
    states[1037] = new State(-111);
    states[1038] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,1042,136,401,21,407,45,415,46,554,31,558,71,562,62,565},new int[]{-267,1039,-262,1040,-86,174,-96,266,-97,267,-170,1041,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-246,1047,-239,1048,-271,1049,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-291,1050});
    states[1039] = new State(-943);
    states[1040] = new State(-469);
    states[1041] = new State(new int[]{7,162,117,167,8,-240,112,-240,111,-240,125,-240,126,-240,127,-240,128,-240,124,-240,6,-240,110,-240,109,-240,122,-240,123,-240,121,-240},new int[]{-289,680});
    states[1042] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-74,1043,-72,283,-266,286,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1043] = new State(new int[]{9,1044,94,1045});
    states[1044] = new State(-235);
    states[1045] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-72,1046,-266,286,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1046] = new State(-248);
    states[1047] = new State(-470);
    states[1048] = new State(-471);
    states[1049] = new State(-472);
    states[1050] = new State(-473);
    states[1051] = new State(new int[]{5,1038,121,-942},new int[]{-314,1052});
    states[1052] = new State(new int[]{121,1053});
    states[1053] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,1054,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[1054] = new State(-922);
    states[1055] = new State(new int[]{5,1056,10,1068,17,-757,8,-757,7,-757,136,-757,4,-757,15,-757,132,-757,130,-757,112,-757,111,-757,125,-757,126,-757,127,-757,128,-757,124,-757,110,-757,109,-757,122,-757,123,-757,120,-757,6,-757,114,-757,119,-757,117,-757,115,-757,118,-757,116,-757,131,-757,16,-757,94,-757,9,-757,13,-757,113,-757,11,-757});
    states[1056] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,1057,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1057] = new State(new int[]{9,1058,10,1062});
    states[1058] = new State(new int[]{5,1038,121,-942},new int[]{-314,1059});
    states[1059] = new State(new int[]{121,1060});
    states[1060] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,1061,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[1061] = new State(-923);
    states[1062] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-315,1063,-316,1016,-147,547,-136,824,-140,24,-141,27});
    states[1063] = new State(new int[]{9,1064,10,545});
    states[1064] = new State(new int[]{5,1038,121,-942},new int[]{-314,1065});
    states[1065] = new State(new int[]{121,1066});
    states[1066] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,1067,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[1067] = new State(-925);
    states[1068] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-315,1069,-316,1016,-147,547,-136,824,-140,24,-141,27});
    states[1069] = new State(new int[]{9,1070,10,545});
    states[1070] = new State(new int[]{5,1038,121,-942},new int[]{-314,1071});
    states[1071] = new State(new int[]{121,1072});
    states[1072] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,34,686,41,692,85,116,37,726,51,776,91,771,32,781,33,807,70,840,22,755,96,797,57,848,44,804,72,917},new int[]{-317,1073,-94,467,-91,468,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,690,-106,629,-311,691,-312,685,-319,834,-245,724,-142,725,-307,835,-237,836,-113,837,-112,838,-114,839,-32,912,-292,913,-158,914,-238,915,-115,916});
    states[1073] = new State(-924);
    states[1074] = new State(-748);
    states[1075] = new State(new int[]{141,1079,143,1080,144,1081,145,1082,147,1083,146,1084,101,-786,85,-786,56,-786,26,-786,64,-786,47,-786,50,-786,59,-786,11,-786,25,-786,23,-786,41,-786,34,-786,27,-786,28,-786,43,-786,24,-786,86,-786,79,-786,78,-786,77,-786,76,-786,20,-786,142,-786,38,-786},new int[]{-196,1076,-199,1085});
    states[1076] = new State(new int[]{10,1077});
    states[1077] = new State(new int[]{141,1079,143,1080,144,1081,145,1082,147,1083,146,1084,101,-787,85,-787,56,-787,26,-787,64,-787,47,-787,50,-787,59,-787,11,-787,25,-787,23,-787,41,-787,34,-787,27,-787,28,-787,43,-787,24,-787,86,-787,79,-787,78,-787,77,-787,76,-787,20,-787,142,-787,38,-787},new int[]{-199,1078});
    states[1078] = new State(-791);
    states[1079] = new State(-801);
    states[1080] = new State(-802);
    states[1081] = new State(-803);
    states[1082] = new State(-804);
    states[1083] = new State(-805);
    states[1084] = new State(-806);
    states[1085] = new State(-790);
    states[1086] = new State(-361);
    states[1087] = new State(-429);
    states[1088] = new State(-430);
    states[1089] = new State(new int[]{8,-435,104,-435,10,-435,5,-435,7,-432});
    states[1090] = new State(new int[]{117,1092,8,-438,104,-438,10,-438,7,-438,5,-438},new int[]{-144,1091});
    states[1091] = new State(-439);
    states[1092] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-147,1093,-136,824,-140,24,-141,27});
    states[1093] = new State(new int[]{115,1094,94,549});
    states[1094] = new State(-308);
    states[1095] = new State(-440);
    states[1096] = new State(new int[]{117,1092,8,-436,104,-436,10,-436,5,-436},new int[]{-144,1097});
    states[1097] = new State(-437);
    states[1098] = new State(new int[]{7,1099});
    states[1099] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471},new int[]{-131,1100,-138,1101,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096});
    states[1100] = new State(-431);
    states[1101] = new State(-434);
    states[1102] = new State(-433);
    states[1103] = new State(-422);
    states[1104] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35},new int[]{-162,1105,-136,1145,-140,24,-141,27,-139,1146});
    states[1105] = new State(new int[]{7,1130,11,1136,5,-379},new int[]{-223,1106,-228,1133});
    states[1106] = new State(new int[]{80,1119,81,1125,10,-386},new int[]{-192,1107});
    states[1107] = new State(new int[]{10,1108});
    states[1108] = new State(new int[]{60,1113,146,1115,145,1116,141,1117,144,1118,11,-376,25,-376,23,-376,41,-376,34,-376,27,-376,28,-376,43,-376,24,-376,86,-376,79,-376,78,-376,77,-376,76,-376},new int[]{-195,1109,-200,1110});
    states[1109] = new State(-370);
    states[1110] = new State(new int[]{10,1111});
    states[1111] = new State(new int[]{60,1113,11,-376,25,-376,23,-376,41,-376,34,-376,27,-376,28,-376,43,-376,24,-376,86,-376,79,-376,78,-376,77,-376,76,-376},new int[]{-195,1112});
    states[1112] = new State(-371);
    states[1113] = new State(new int[]{10,1114});
    states[1114] = new State(-377);
    states[1115] = new State(-807);
    states[1116] = new State(-808);
    states[1117] = new State(-809);
    states[1118] = new State(-810);
    states[1119] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,5,646,34,686,41,692,10,-385},new int[]{-103,1120,-83,1124,-82,126,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645,-311,684,-312,685});
    states[1120] = new State(new int[]{81,1122,10,-389},new int[]{-193,1121});
    states[1121] = new State(-387);
    states[1122] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476},new int[]{-250,1123,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[1123] = new State(-390);
    states[1124] = new State(-384);
    states[1125] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476},new int[]{-250,1126,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[1126] = new State(new int[]{80,1128,10,-391},new int[]{-194,1127});
    states[1127] = new State(-388);
    states[1128] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,5,646,34,686,41,692,10,-385},new int[]{-103,1129,-83,1124,-82,126,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645,-311,684,-312,685});
    states[1129] = new State(-392);
    states[1130] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35},new int[]{-136,1131,-139,1132,-140,24,-141,27});
    states[1131] = new State(-365);
    states[1132] = new State(-366);
    states[1133] = new State(new int[]{5,1134});
    states[1134] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,1135,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1135] = new State(-378);
    states[1136] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-227,1137,-226,1144,-147,1141,-136,824,-140,24,-141,27});
    states[1137] = new State(new int[]{12,1138,10,1139});
    states[1138] = new State(-380);
    states[1139] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-226,1140,-147,1141,-136,824,-140,24,-141,27});
    states[1140] = new State(-382);
    states[1141] = new State(new int[]{5,1142,94,549});
    states[1142] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,1143,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1143] = new State(-383);
    states[1144] = new State(-381);
    states[1145] = new State(-363);
    states[1146] = new State(-364);
    states[1147] = new State(new int[]{43,1148});
    states[1148] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35},new int[]{-162,1149,-136,1145,-140,24,-141,27,-139,1146});
    states[1149] = new State(new int[]{7,1130,11,1136,5,-379},new int[]{-223,1150,-228,1133});
    states[1150] = new State(new int[]{104,1153,10,-375},new int[]{-201,1151});
    states[1151] = new State(new int[]{10,1152});
    states[1152] = new State(-373);
    states[1153] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,1154,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[1154] = new State(-374);
    states[1155] = new State(new int[]{101,1286,11,-355,25,-355,23,-355,41,-355,34,-355,27,-355,28,-355,43,-355,24,-355,86,-355,79,-355,78,-355,77,-355,76,-355,56,-63,26,-63,64,-63,47,-63,50,-63,59,-63,85,-63},new int[]{-166,1156,-40,1157,-36,1160,-57,1285});
    states[1156] = new State(-423);
    states[1157] = new State(new int[]{85,116},new int[]{-245,1158});
    states[1158] = new State(new int[]{10,1159});
    states[1159] = new State(-450);
    states[1160] = new State(new int[]{56,1163,26,1184,64,1188,47,1349,50,1364,59,1366,85,-62},new int[]{-42,1161,-157,1162,-26,1169,-48,1186,-279,1190,-298,1351});
    states[1161] = new State(-64);
    states[1162] = new State(-80);
    states[1163] = new State(new int[]{148,750,137,23,80,25,81,26,75,28,73,29},new int[]{-145,1164,-132,1168,-136,751,-140,24,-141,27});
    states[1164] = new State(new int[]{10,1165,94,1166});
    states[1165] = new State(-89);
    states[1166] = new State(new int[]{148,750,137,23,80,25,81,26,75,28,73,29},new int[]{-132,1167,-136,751,-140,24,-141,27});
    states[1167] = new State(-91);
    states[1168] = new State(-90);
    states[1169] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,56,-81,26,-81,64,-81,47,-81,50,-81,59,-81,85,-81},new int[]{-24,1170,-25,1171,-130,1173,-136,1183,-140,24,-141,27});
    states[1170] = new State(-95);
    states[1171] = new State(new int[]{10,1172});
    states[1172] = new State(-105);
    states[1173] = new State(new int[]{114,1174,5,1179});
    states[1174] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,1177,129,377,110,355,109,356},new int[]{-99,1175,-84,1176,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384,-88,1178});
    states[1175] = new State(-106);
    states[1176] = new State(new int[]{13,186,10,-108,86,-108,79,-108,78,-108,77,-108,76,-108});
    states[1177] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,931,129,377,110,355,109,356,60,157,9,-183},new int[]{-84,919,-62,932,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384,-61,236,-80,934,-79,239,-88,935,-233,936,-53,937});
    states[1178] = new State(-109);
    states[1179] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,1180,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1180] = new State(new int[]{114,1181});
    states[1181] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,931,129,377,110,355,109,356},new int[]{-79,1182,-84,240,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384,-88,935,-233,936});
    states[1182] = new State(-107);
    states[1183] = new State(-110);
    states[1184] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-24,1185,-25,1171,-130,1173,-136,1183,-140,24,-141,27});
    states[1185] = new State(-94);
    states[1186] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,56,-82,26,-82,64,-82,47,-82,50,-82,59,-82,85,-82},new int[]{-24,1187,-25,1171,-130,1173,-136,1183,-140,24,-141,27});
    states[1187] = new State(-97);
    states[1188] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-24,1189,-25,1171,-130,1173,-136,1183,-140,24,-141,27});
    states[1189] = new State(-96);
    states[1190] = new State(new int[]{11,672,56,-83,26,-83,64,-83,47,-83,50,-83,59,-83,85,-83,137,-197,80,-197,81,-197,75,-197,73,-197},new int[]{-45,1191,-6,1192,-240,1033});
    states[1191] = new State(-99);
    states[1192] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,11,672},new int[]{-46,1193,-240,431,-133,1194,-136,1341,-140,24,-141,27,-134,1346});
    states[1193] = new State(-194);
    states[1194] = new State(new int[]{114,1195});
    states[1195] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598,66,1335,67,1336,141,1337,24,1338,25,1339,23,-290,40,-290,61,-290},new int[]{-277,1196,-266,1198,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569,-27,1199,-20,1200,-21,1333,-19,1340});
    states[1196] = new State(new int[]{10,1197});
    states[1197] = new State(-203);
    states[1198] = new State(-208);
    states[1199] = new State(-209);
    states[1200] = new State(new int[]{23,1327,40,1328,61,1329},new int[]{-281,1201});
    states[1201] = new State(new int[]{8,1242,20,-302,11,-302,86,-302,79,-302,78,-302,77,-302,76,-302,26,-302,137,-302,80,-302,81,-302,75,-302,73,-302,59,-302,25,-302,23,-302,41,-302,34,-302,27,-302,28,-302,43,-302,24,-302,10,-302},new int[]{-173,1202});
    states[1202] = new State(new int[]{20,1233,11,-309,86,-309,79,-309,78,-309,77,-309,76,-309,26,-309,137,-309,80,-309,81,-309,75,-309,73,-309,59,-309,25,-309,23,-309,41,-309,34,-309,27,-309,28,-309,43,-309,24,-309,10,-309},new int[]{-306,1203,-305,1231,-304,1253});
    states[1203] = new State(new int[]{11,672,10,-300,86,-326,79,-326,78,-326,77,-326,76,-326,26,-197,137,-197,80,-197,81,-197,75,-197,73,-197,59,-197,25,-197,23,-197,41,-197,34,-197,27,-197,28,-197,43,-197,24,-197},new int[]{-23,1204,-22,1205,-29,1211,-31,422,-41,1212,-6,1213,-240,1033,-30,1324,-50,1326,-49,428,-51,1325});
    states[1204] = new State(-283);
    states[1205] = new State(new int[]{86,1206,79,1207,78,1208,77,1209,76,1210},new int[]{-7,420});
    states[1206] = new State(-301);
    states[1207] = new State(-322);
    states[1208] = new State(-323);
    states[1209] = new State(-324);
    states[1210] = new State(-325);
    states[1211] = new State(-320);
    states[1212] = new State(-334);
    states[1213] = new State(new int[]{26,1215,137,23,80,25,81,26,75,28,73,29,59,1219,25,1280,23,1281,11,672,41,1226,34,1261,27,1295,28,1302,43,1309,24,1318},new int[]{-47,1214,-240,431,-212,430,-209,432,-248,433,-301,1217,-300,1218,-147,825,-136,824,-140,24,-141,27,-3,1223,-220,1282,-218,1155,-215,1225,-219,1260,-217,1283,-205,1306,-206,1307,-208,1308});
    states[1214] = new State(-336);
    states[1215] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-25,1216,-130,1173,-136,1183,-140,24,-141,27});
    states[1216] = new State(-341);
    states[1217] = new State(-342);
    states[1218] = new State(-346);
    states[1219] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-147,1220,-136,824,-140,24,-141,27});
    states[1220] = new State(new int[]{5,1221,94,549});
    states[1221] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,1222,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1222] = new State(-347);
    states[1223] = new State(new int[]{27,436,43,1104,24,1147,137,23,80,25,81,26,75,28,73,29,59,1219,41,1226,34,1261},new int[]{-301,1224,-220,435,-206,1103,-300,1218,-147,825,-136,824,-140,24,-141,27,-218,1155,-215,1225,-219,1260});
    states[1224] = new State(-343);
    states[1225] = new State(-356);
    states[1226] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471},new int[]{-160,1227,-159,1087,-131,1088,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096,-323,1098,-138,1102});
    states[1227] = new State(new int[]{8,572,10,-452,104,-452},new int[]{-117,1228});
    states[1228] = new State(new int[]{10,1258,104,-788},new int[]{-197,1229,-198,1254});
    states[1229] = new State(new int[]{20,1233,101,-309,85,-309,56,-309,26,-309,64,-309,47,-309,50,-309,59,-309,11,-309,25,-309,23,-309,41,-309,34,-309,27,-309,28,-309,43,-309,24,-309,86,-309,79,-309,78,-309,77,-309,76,-309,142,-309,38,-309},new int[]{-306,1230,-305,1231,-304,1253});
    states[1230] = new State(-441);
    states[1231] = new State(new int[]{20,1233,11,-310,86,-310,79,-310,78,-310,77,-310,76,-310,26,-310,137,-310,80,-310,81,-310,75,-310,73,-310,59,-310,25,-310,23,-310,41,-310,34,-310,27,-310,28,-310,43,-310,24,-310,10,-310,101,-310,85,-310,56,-310,64,-310,47,-310,50,-310,142,-310,38,-310},new int[]{-304,1232});
    states[1232] = new State(-312);
    states[1233] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-147,1234,-136,824,-140,24,-141,27});
    states[1234] = new State(new int[]{5,1235,94,549});
    states[1235] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,1241,46,554,31,558,71,562,62,565,41,570,34,598,23,1250,27,1251},new int[]{-278,1236,-275,1252,-266,1240,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1236] = new State(new int[]{10,1237,94,1238});
    states[1237] = new State(-313);
    states[1238] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,1241,46,554,31,558,71,562,62,565,41,570,34,598,23,1250,27,1251},new int[]{-275,1239,-266,1240,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1239] = new State(-315);
    states[1240] = new State(-316);
    states[1241] = new State(new int[]{8,1242,10,-318,94,-318,20,-302,11,-302,86,-302,79,-302,78,-302,77,-302,76,-302,26,-302,137,-302,80,-302,81,-302,75,-302,73,-302,59,-302,25,-302,23,-302,41,-302,34,-302,27,-302,28,-302,43,-302,24,-302},new int[]{-173,416});
    states[1242] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-172,1243,-171,1249,-170,1247,-136,195,-140,24,-141,27,-291,1248});
    states[1243] = new State(new int[]{9,1244,94,1245});
    states[1244] = new State(-303);
    states[1245] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-171,1246,-170,1247,-136,195,-140,24,-141,27,-291,1248});
    states[1246] = new State(-305);
    states[1247] = new State(new int[]{7,162,117,167,9,-306,94,-306},new int[]{-289,680});
    states[1248] = new State(-307);
    states[1249] = new State(-304);
    states[1250] = new State(-317);
    states[1251] = new State(-319);
    states[1252] = new State(-314);
    states[1253] = new State(-311);
    states[1254] = new State(new int[]{104,1255});
    states[1255] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476},new int[]{-250,1256,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[1256] = new State(new int[]{10,1257});
    states[1257] = new State(-426);
    states[1258] = new State(new int[]{141,1079,143,1080,144,1081,145,1082,147,1083,146,1084,20,-786,101,-786,85,-786,56,-786,26,-786,64,-786,47,-786,50,-786,59,-786,11,-786,25,-786,23,-786,41,-786,34,-786,27,-786,28,-786,43,-786,24,-786,86,-786,79,-786,78,-786,77,-786,76,-786,142,-786},new int[]{-196,1259,-199,1085});
    states[1259] = new State(new int[]{10,1077,104,-789});
    states[1260] = new State(-357);
    states[1261] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471},new int[]{-159,1262,-131,1088,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096,-323,1098,-138,1102});
    states[1262] = new State(new int[]{8,572,5,-452,10,-452,104,-452},new int[]{-117,1263});
    states[1263] = new State(new int[]{5,1266,10,1258,104,-788},new int[]{-197,1264,-198,1276});
    states[1264] = new State(new int[]{20,1233,101,-309,85,-309,56,-309,26,-309,64,-309,47,-309,50,-309,59,-309,11,-309,25,-309,23,-309,41,-309,34,-309,27,-309,28,-309,43,-309,24,-309,86,-309,79,-309,78,-309,77,-309,76,-309,142,-309,38,-309},new int[]{-306,1265,-305,1231,-304,1253});
    states[1265] = new State(-442);
    states[1266] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,1267,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1267] = new State(new int[]{10,1258,104,-788},new int[]{-197,1268,-198,1270});
    states[1268] = new State(new int[]{20,1233,101,-309,85,-309,56,-309,26,-309,64,-309,47,-309,50,-309,59,-309,11,-309,25,-309,23,-309,41,-309,34,-309,27,-309,28,-309,43,-309,24,-309,86,-309,79,-309,78,-309,77,-309,76,-309,142,-309,38,-309},new int[]{-306,1269,-305,1231,-304,1253});
    states[1269] = new State(-443);
    states[1270] = new State(new int[]{104,1271});
    states[1271] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,34,686,41,692},new int[]{-93,1272,-92,1274,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-311,1275,-312,685});
    states[1272] = new State(new int[]{10,1273});
    states[1273] = new State(-424);
    states[1274] = new State(-589);
    states[1275] = new State(-590);
    states[1276] = new State(new int[]{104,1277});
    states[1277] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,34,686,41,692},new int[]{-93,1278,-92,1274,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-311,1275,-312,685});
    states[1278] = new State(new int[]{10,1279});
    states[1279] = new State(-425);
    states[1280] = new State(-344);
    states[1281] = new State(-345);
    states[1282] = new State(-353);
    states[1283] = new State(new int[]{101,1286,11,-354,25,-354,23,-354,41,-354,34,-354,27,-354,28,-354,43,-354,24,-354,86,-354,79,-354,78,-354,77,-354,76,-354,56,-63,26,-63,64,-63,47,-63,50,-63,59,-63,85,-63},new int[]{-166,1284,-40,1157,-36,1160,-57,1285});
    states[1284] = new State(-409);
    states[1285] = new State(-451);
    states[1286] = new State(new int[]{10,1294,137,23,80,25,81,26,75,28,73,29,138,148,140,149,139,151},new int[]{-98,1287,-136,1291,-140,24,-141,27,-154,1292,-156,146,-155,150});
    states[1287] = new State(new int[]{75,1288,10,1293});
    states[1288] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,138,148,140,149,139,151},new int[]{-98,1289,-136,1291,-140,24,-141,27,-154,1292,-156,146,-155,150});
    states[1289] = new State(new int[]{10,1290});
    states[1290] = new State(-444);
    states[1291] = new State(-447);
    states[1292] = new State(-448);
    states[1293] = new State(-445);
    states[1294] = new State(-446);
    states[1295] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471,8,-362,104,-362,10,-362},new int[]{-161,1296,-160,1086,-159,1087,-131,1088,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096,-323,1098,-138,1102});
    states[1296] = new State(new int[]{8,572,104,-452,10,-452},new int[]{-117,1297});
    states[1297] = new State(new int[]{104,1299,10,1075},new int[]{-197,1298});
    states[1298] = new State(-358);
    states[1299] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476},new int[]{-250,1300,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[1300] = new State(new int[]{10,1301});
    states[1301] = new State(-410);
    states[1302] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471,8,-362,10,-362},new int[]{-161,1303,-160,1086,-159,1087,-131,1088,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096,-323,1098,-138,1102});
    states[1303] = new State(new int[]{8,572,10,-452},new int[]{-117,1304});
    states[1304] = new State(new int[]{10,1075},new int[]{-197,1305});
    states[1305] = new State(-360);
    states[1306] = new State(-350);
    states[1307] = new State(-421);
    states[1308] = new State(-351);
    states[1309] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35},new int[]{-162,1310,-136,1145,-140,24,-141,27,-139,1146});
    states[1310] = new State(new int[]{7,1130,11,1136,5,-379},new int[]{-223,1311,-228,1133});
    states[1311] = new State(new int[]{80,1119,81,1125,10,-386},new int[]{-192,1312});
    states[1312] = new State(new int[]{10,1313});
    states[1313] = new State(new int[]{60,1113,146,1115,145,1116,141,1117,144,1118,11,-376,25,-376,23,-376,41,-376,34,-376,27,-376,28,-376,43,-376,24,-376,86,-376,79,-376,78,-376,77,-376,76,-376},new int[]{-195,1314,-200,1315});
    states[1314] = new State(-368);
    states[1315] = new State(new int[]{10,1316});
    states[1316] = new State(new int[]{60,1113,11,-376,25,-376,23,-376,41,-376,34,-376,27,-376,28,-376,43,-376,24,-376,86,-376,79,-376,78,-376,77,-376,76,-376},new int[]{-195,1317});
    states[1317] = new State(-369);
    states[1318] = new State(new int[]{43,1319});
    states[1319] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35},new int[]{-162,1320,-136,1145,-140,24,-141,27,-139,1146});
    states[1320] = new State(new int[]{7,1130,11,1136,5,-379},new int[]{-223,1321,-228,1133});
    states[1321] = new State(new int[]{104,1153,10,-375},new int[]{-201,1322});
    states[1322] = new State(new int[]{10,1323});
    states[1323] = new State(-372);
    states[1324] = new State(new int[]{11,672,86,-328,79,-328,78,-328,77,-328,76,-328,25,-197,23,-197,41,-197,34,-197,27,-197,28,-197,43,-197,24,-197},new int[]{-50,427,-49,428,-6,429,-240,1033,-51,1325});
    states[1325] = new State(-340);
    states[1326] = new State(-337);
    states[1327] = new State(-294);
    states[1328] = new State(-295);
    states[1329] = new State(new int[]{23,1330,45,1331,40,1332,8,-296,20,-296,11,-296,86,-296,79,-296,78,-296,77,-296,76,-296,26,-296,137,-296,80,-296,81,-296,75,-296,73,-296,59,-296,25,-296,41,-296,34,-296,27,-296,28,-296,43,-296,24,-296,10,-296});
    states[1330] = new State(-297);
    states[1331] = new State(-298);
    states[1332] = new State(-299);
    states[1333] = new State(new int[]{66,1335,67,1336,141,1337,24,1338,25,1339,23,-291,40,-291,61,-291},new int[]{-19,1334});
    states[1334] = new State(-293);
    states[1335] = new State(-285);
    states[1336] = new State(-286);
    states[1337] = new State(-287);
    states[1338] = new State(-288);
    states[1339] = new State(-289);
    states[1340] = new State(-292);
    states[1341] = new State(new int[]{117,1343,114,-205},new int[]{-144,1342});
    states[1342] = new State(-206);
    states[1343] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-147,1344,-136,824,-140,24,-141,27});
    states[1344] = new State(new int[]{116,1345,115,1094,94,549});
    states[1345] = new State(-207);
    states[1346] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598,66,1335,67,1336,141,1337,24,1338,25,1339,23,-290,40,-290,61,-290},new int[]{-277,1347,-266,1198,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569,-27,1199,-20,1200,-21,1333,-19,1340});
    states[1347] = new State(new int[]{10,1348});
    states[1348] = new State(-204);
    states[1349] = new State(new int[]{11,672,137,-197,80,-197,81,-197,75,-197,73,-197},new int[]{-45,1350,-6,1192,-240,1033});
    states[1350] = new State(-98);
    states[1351] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,8,1356,56,-84,26,-84,64,-84,47,-84,50,-84,59,-84,85,-84},new int[]{-302,1352,-299,1353,-300,1354,-147,825,-136,824,-140,24,-141,27});
    states[1352] = new State(-104);
    states[1353] = new State(-100);
    states[1354] = new State(new int[]{10,1355});
    states[1355] = new State(-393);
    states[1356] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,1357,-140,24,-141,27});
    states[1357] = new State(new int[]{94,1358});
    states[1358] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-147,1359,-136,824,-140,24,-141,27});
    states[1359] = new State(new int[]{9,1360,94,549});
    states[1360] = new State(new int[]{104,1361});
    states[1361] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637},new int[]{-92,1362,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636});
    states[1362] = new State(new int[]{10,1363});
    states[1363] = new State(-101);
    states[1364] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,8,1356},new int[]{-302,1365,-299,1353,-300,1354,-147,825,-136,824,-140,24,-141,27});
    states[1365] = new State(-102);
    states[1366] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,8,1356},new int[]{-302,1367,-299,1353,-300,1354,-147,825,-136,824,-140,24,-141,27});
    states[1367] = new State(-103);
    states[1368] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,1042,12,-266,94,-266},new int[]{-261,1369,-262,1370,-86,174,-96,266,-97,267,-170,388,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150});
    states[1369] = new State(-264);
    states[1370] = new State(-265);
    states[1371] = new State(-263);
    states[1372] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-266,1373,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1373] = new State(-262);
    states[1374] = new State(-230);
    states[1375] = new State(-231);
    states[1376] = new State(new int[]{121,395,115,-232,94,-232,114,-232,9,-232,10,-232,104,-232,86,-232,92,-232,95,-232,30,-232,98,-232,12,-232,93,-232,29,-232,81,-232,80,-232,2,-232,79,-232,78,-232,77,-232,76,-232,131,-232});
    states[1377] = new State(-641);
    states[1378] = new State(-642);
    states[1379] = new State(-643);
    states[1380] = new State(-635);
    states[1381] = new State(-775);
    states[1382] = new State(-225);
    states[1383] = new State(-221);
    states[1384] = new State(-601);
    states[1385] = new State(new int[]{8,1386});
    states[1386] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,42,471,39,501,8,536,18,246,19,251},new int[]{-322,1387,-321,1395,-136,1391,-140,24,-141,27,-90,1394,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627});
    states[1387] = new State(new int[]{9,1388,94,1389});
    states[1388] = new State(-610);
    states[1389] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,42,471,39,501,8,536,18,246,19,251},new int[]{-321,1390,-136,1391,-140,24,-141,27,-90,1394,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627});
    states[1390] = new State(-614);
    states[1391] = new State(new int[]{104,1392,17,-757,8,-757,7,-757,136,-757,4,-757,15,-757,132,-757,130,-757,112,-757,111,-757,125,-757,126,-757,127,-757,128,-757,124,-757,110,-757,109,-757,122,-757,123,-757,120,-757,6,-757,114,-757,119,-757,117,-757,115,-757,118,-757,116,-757,131,-757,9,-757,94,-757,113,-757,11,-757});
    states[1392] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251},new int[]{-90,1393,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627});
    states[1393] = new State(new int[]{114,290,119,291,117,292,115,293,118,294,116,295,131,296,9,-611,94,-611},new int[]{-186,131});
    states[1394] = new State(new int[]{114,290,119,291,117,292,115,293,118,294,116,295,131,296,9,-612,94,-612},new int[]{-186,131});
    states[1395] = new State(-613);
    states[1396] = new State(new int[]{13,186,5,-680,12,-680});
    states[1397] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-84,1398,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[1398] = new State(new int[]{13,186,94,-175,9,-175,12,-175,5,-175});
    states[1399] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356,5,-681,12,-681},new int[]{-111,1400,-84,1396,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[1400] = new State(new int[]{5,1401,12,-687});
    states[1401] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-84,1402,-75,190,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383,-231,384});
    states[1402] = new State(new int[]{13,186,12,-689});
    states[1403] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,86,64,30,65,31,66,24,67,98,68,95,69,32,70,33,71,34,72,37,73,38,74,39,75,97,76,40,77,41,78,43,79,44,80,45,81,91,82,46,83,96,84,47,85,25,86,48,87,68,88,92,89,49,90,50,91,51,92,52,93,53,94,54,95,55,96,56,97,58,98,99,99,100,100,103,101,101,102,102,103,59,104,72,105,35,106,36,107,42,109},new int[]{-127,1404,-136,22,-140,24,-141,27,-283,30,-139,31,-284,108});
    states[1404] = new State(-164);
    states[1405] = new State(-165);
    states[1406] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,5,646,34,686,41,692,9,-169},new int[]{-70,1407,-66,1409,-83,522,-82,126,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645,-311,684,-312,685});
    states[1407] = new State(new int[]{9,1408});
    states[1408] = new State(-166);
    states[1409] = new State(new int[]{94,462,9,-168});
    states[1410] = new State(-136);
    states[1411] = new State(new int[]{137,23,80,25,81,26,75,28,73,226,138,148,140,149,139,151,148,153,150,154,149,155,39,243,18,246,19,251,11,367,53,371,135,372,8,374,129,377,110,355,109,356},new int[]{-75,1412,-12,214,-10,224,-13,200,-136,225,-140,24,-141,27,-154,241,-156,146,-155,150,-15,242,-247,245,-285,250,-229,366,-189,379,-163,381,-255,382,-259,383});
    states[1412] = new State(new int[]{110,1413,109,1414,122,1415,123,1416,13,-114,6,-114,94,-114,9,-114,12,-114,5,-114,86,-114,10,-114,92,-114,95,-114,30,-114,98,-114,93,-114,29,-114,81,-114,80,-114,2,-114,79,-114,78,-114,77,-114,76,-114},new int[]{-183,191});
    states[1413] = new State(-126);
    states[1414] = new State(-127);
    states[1415] = new State(-128);
    states[1416] = new State(-129);
    states[1417] = new State(-117);
    states[1418] = new State(-118);
    states[1419] = new State(-119);
    states[1420] = new State(-120);
    states[1421] = new State(-121);
    states[1422] = new State(-122);
    states[1423] = new State(-123);
    states[1424] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151},new int[]{-86,1425,-96,266,-97,267,-170,388,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150});
    states[1425] = new State(new int[]{110,1413,109,1414,122,1415,123,1416,13,-234,115,-234,94,-234,114,-234,9,-234,10,-234,121,-234,104,-234,86,-234,92,-234,95,-234,30,-234,98,-234,12,-234,93,-234,29,-234,81,-234,80,-234,2,-234,79,-234,78,-234,77,-234,76,-234,131,-234},new int[]{-183,175});
    states[1426] = new State(-701);
    states[1427] = new State(-619);
    states[1428] = new State(-33);
    states[1429] = new State(new int[]{56,1163,26,1184,64,1188,47,1349,50,1364,59,1366,11,672,85,-59,86,-59,97,-59,41,-197,34,-197,25,-197,23,-197,27,-197,28,-197},new int[]{-43,1430,-157,1431,-26,1432,-48,1433,-279,1434,-298,1435,-210,1436,-6,1437,-240,1033});
    states[1430] = new State(-61);
    states[1431] = new State(-71);
    states[1432] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,56,-72,26,-72,64,-72,47,-72,50,-72,59,-72,11,-72,41,-72,34,-72,25,-72,23,-72,27,-72,28,-72,85,-72,86,-72,97,-72},new int[]{-24,1170,-25,1171,-130,1173,-136,1183,-140,24,-141,27});
    states[1433] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,56,-73,26,-73,64,-73,47,-73,50,-73,59,-73,11,-73,41,-73,34,-73,25,-73,23,-73,27,-73,28,-73,85,-73,86,-73,97,-73},new int[]{-24,1187,-25,1171,-130,1173,-136,1183,-140,24,-141,27});
    states[1434] = new State(new int[]{11,672,56,-74,26,-74,64,-74,47,-74,50,-74,59,-74,41,-74,34,-74,25,-74,23,-74,27,-74,28,-74,85,-74,86,-74,97,-74,137,-197,80,-197,81,-197,75,-197,73,-197},new int[]{-45,1191,-6,1192,-240,1033});
    states[1435] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,8,1356,56,-75,26,-75,64,-75,47,-75,50,-75,59,-75,11,-75,41,-75,34,-75,25,-75,23,-75,27,-75,28,-75,85,-75,86,-75,97,-75},new int[]{-302,1352,-299,1353,-300,1354,-147,825,-136,824,-140,24,-141,27});
    states[1436] = new State(-76);
    states[1437] = new State(new int[]{41,1450,34,1457,25,1280,23,1281,27,1485,28,1302,11,672},new int[]{-203,1438,-240,431,-204,1439,-211,1440,-218,1441,-215,1225,-219,1260,-3,1474,-207,1482,-217,1483});
    states[1438] = new State(-79);
    states[1439] = new State(-77);
    states[1440] = new State(-412);
    states[1441] = new State(new int[]{142,1443,101,1286,56,-60,26,-60,64,-60,47,-60,50,-60,59,-60,11,-60,41,-60,34,-60,25,-60,23,-60,27,-60,28,-60,85,-60},new int[]{-168,1442,-167,1445,-38,1446,-39,1429,-57,1449});
    states[1442] = new State(-414);
    states[1443] = new State(new int[]{10,1444});
    states[1444] = new State(-420);
    states[1445] = new State(-427);
    states[1446] = new State(new int[]{85,116},new int[]{-245,1447});
    states[1447] = new State(new int[]{10,1448});
    states[1448] = new State(-449);
    states[1449] = new State(-428);
    states[1450] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471},new int[]{-160,1451,-159,1087,-131,1088,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096,-323,1098,-138,1102});
    states[1451] = new State(new int[]{8,572,10,-452,104,-452},new int[]{-117,1452});
    states[1452] = new State(new int[]{10,1258,104,-788},new int[]{-197,1229,-198,1453});
    states[1453] = new State(new int[]{104,1454});
    states[1454] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476},new int[]{-250,1455,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[1455] = new State(new int[]{10,1456});
    states[1456] = new State(-419);
    states[1457] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471},new int[]{-159,1458,-131,1088,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096,-323,1098,-138,1102});
    states[1458] = new State(new int[]{8,572,5,-452,10,-452,104,-452},new int[]{-117,1459});
    states[1459] = new State(new int[]{5,1460,10,1258,104,-788},new int[]{-197,1264,-198,1468});
    states[1460] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,1461,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1461] = new State(new int[]{10,1258,104,-788},new int[]{-197,1268,-198,1462});
    states[1462] = new State(new int[]{104,1463});
    states[1463] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,34,686,41,692},new int[]{-92,1464,-311,1466,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-312,685});
    states[1464] = new State(new int[]{10,1465});
    states[1465] = new State(-415);
    states[1466] = new State(new int[]{10,1467});
    states[1467] = new State(-417);
    states[1468] = new State(new int[]{104,1469});
    states[1469] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,503,18,246,19,251,37,637,34,686,41,692},new int[]{-92,1470,-311,1472,-91,128,-90,289,-95,469,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,464,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-312,685});
    states[1470] = new State(new int[]{10,1471});
    states[1471] = new State(-416);
    states[1472] = new State(new int[]{10,1473});
    states[1473] = new State(-418);
    states[1474] = new State(new int[]{27,1476,41,1450,34,1457},new int[]{-211,1475,-218,1441,-215,1225,-219,1260});
    states[1475] = new State(-413);
    states[1476] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471,8,-362,104,-362,10,-362},new int[]{-161,1477,-160,1086,-159,1087,-131,1088,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096,-323,1098,-138,1102});
    states[1477] = new State(new int[]{8,572,104,-452,10,-452},new int[]{-117,1478});
    states[1478] = new State(new int[]{104,1479,10,1075},new int[]{-197,439});
    states[1479] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476},new int[]{-250,1480,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[1480] = new State(new int[]{10,1481});
    states[1481] = new State(-408);
    states[1482] = new State(-78);
    states[1483] = new State(-60,new int[]{-167,1484,-38,1446,-39,1429});
    states[1484] = new State(-406);
    states[1485] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471,8,-362,104,-362,10,-362},new int[]{-161,1486,-160,1086,-159,1087,-131,1088,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096,-323,1098,-138,1102});
    states[1486] = new State(new int[]{8,572,104,-452,10,-452},new int[]{-117,1487});
    states[1487] = new State(new int[]{104,1488,10,1075},new int[]{-197,1298});
    states[1488] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,153,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,10,-476},new int[]{-250,1489,-4,122,-102,123,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859});
    states[1489] = new State(new int[]{10,1490});
    states[1490] = new State(-407);
    states[1491] = new State(new int[]{3,1493,49,-13,85,-13,56,-13,26,-13,64,-13,47,-13,50,-13,59,-13,11,-13,41,-13,34,-13,25,-13,23,-13,27,-13,28,-13,40,-13,86,-13,97,-13},new int[]{-174,1492});
    states[1492] = new State(-15);
    states[1493] = new State(new int[]{137,1494,138,1495});
    states[1494] = new State(-16);
    states[1495] = new State(-17);
    states[1496] = new State(-14);
    states[1497] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-136,1498,-140,24,-141,27});
    states[1498] = new State(new int[]{10,1500,8,1501},new int[]{-177,1499});
    states[1499] = new State(-26);
    states[1500] = new State(-27);
    states[1501] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-179,1502,-135,1508,-136,1507,-140,24,-141,27});
    states[1502] = new State(new int[]{9,1503,94,1505});
    states[1503] = new State(new int[]{10,1504});
    states[1504] = new State(-28);
    states[1505] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-135,1506,-136,1507,-140,24,-141,27});
    states[1506] = new State(-30);
    states[1507] = new State(-31);
    states[1508] = new State(-29);
    states[1509] = new State(-3);
    states[1510] = new State(new int[]{99,1565,100,1566,103,1567,11,672},new int[]{-297,1511,-240,431,-2,1560});
    states[1511] = new State(new int[]{40,1532,49,-36,56,-36,26,-36,64,-36,47,-36,50,-36,59,-36,11,-36,41,-36,34,-36,25,-36,23,-36,27,-36,28,-36,86,-36,97,-36,85,-36},new int[]{-151,1512,-152,1529,-293,1558});
    states[1512] = new State(new int[]{38,1526},new int[]{-150,1513});
    states[1513] = new State(new int[]{86,1516,97,1517,85,1523},new int[]{-143,1514});
    states[1514] = new State(new int[]{7,1515});
    states[1515] = new State(-42);
    states[1516] = new State(-52);
    states[1517] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,98,-476,10,-476},new int[]{-242,1518,-251,767,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[1518] = new State(new int[]{86,1519,98,1520,10,119});
    states[1519] = new State(-53);
    states[1520] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476},new int[]{-242,1521,-251,767,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[1521] = new State(new int[]{86,1522,10,119});
    states[1522] = new State(-54);
    states[1523] = new State(new int[]{135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,732,8,733,18,246,19,251,138,148,140,149,139,151,148,769,150,154,149,155,54,748,85,116,37,726,22,755,91,771,51,776,32,781,52,791,96,797,44,804,33,807,50,815,57,848,72,853,70,840,35,860,86,-476,10,-476},new int[]{-242,1524,-251,767,-250,121,-4,122,-102,123,-121,443,-101,696,-136,768,-140,24,-141,27,-181,470,-247,516,-285,517,-14,710,-154,145,-156,146,-155,150,-15,152,-16,518,-54,711,-105,529,-202,746,-122,747,-245,752,-142,753,-32,754,-237,770,-307,775,-113,780,-308,790,-149,795,-292,796,-238,803,-112,806,-303,814,-55,844,-164,845,-163,846,-158,847,-115,852,-116,857,-114,858,-337,859,-132,965});
    states[1524] = new State(new int[]{86,1525,10,119});
    states[1525] = new State(-55);
    states[1526] = new State(-36,new int[]{-293,1527});
    states[1527] = new State(new int[]{49,14,56,-60,26,-60,64,-60,47,-60,50,-60,59,-60,11,-60,41,-60,34,-60,25,-60,23,-60,27,-60,28,-60,86,-60,97,-60,85,-60},new int[]{-38,1528,-39,1429});
    states[1528] = new State(-50);
    states[1529] = new State(new int[]{86,1516,97,1517,85,1523},new int[]{-143,1530});
    states[1530] = new State(new int[]{7,1531});
    states[1531] = new State(-43);
    states[1532] = new State(-36,new int[]{-293,1533});
    states[1533] = new State(new int[]{49,14,26,-57,64,-57,47,-57,50,-57,59,-57,11,-57,41,-57,34,-57,38,-57},new int[]{-37,1534,-35,1535});
    states[1534] = new State(-49);
    states[1535] = new State(new int[]{26,1184,64,1188,47,1349,50,1364,59,1366,11,672,38,-56,41,-197,34,-197},new int[]{-44,1536,-26,1537,-48,1538,-279,1539,-298,1540,-222,1541,-6,1542,-240,1033,-221,1557});
    states[1536] = new State(-58);
    states[1537] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,26,-65,64,-65,47,-65,50,-65,59,-65,11,-65,41,-65,34,-65,38,-65},new int[]{-24,1170,-25,1171,-130,1173,-136,1183,-140,24,-141,27});
    states[1538] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,26,-66,64,-66,47,-66,50,-66,59,-66,11,-66,41,-66,34,-66,38,-66},new int[]{-24,1187,-25,1171,-130,1173,-136,1183,-140,24,-141,27});
    states[1539] = new State(new int[]{11,672,26,-67,64,-67,47,-67,50,-67,59,-67,41,-67,34,-67,38,-67,137,-197,80,-197,81,-197,75,-197,73,-197},new int[]{-45,1191,-6,1192,-240,1033});
    states[1540] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,8,1356,26,-68,64,-68,47,-68,50,-68,59,-68,11,-68,41,-68,34,-68,38,-68},new int[]{-302,1352,-299,1353,-300,1354,-147,825,-136,824,-140,24,-141,27});
    states[1541] = new State(-69);
    states[1542] = new State(new int[]{41,1549,11,672,34,1552},new int[]{-215,1543,-240,431,-219,1546});
    states[1543] = new State(new int[]{142,1544,26,-85,64,-85,47,-85,50,-85,59,-85,11,-85,41,-85,34,-85,38,-85});
    states[1544] = new State(new int[]{10,1545});
    states[1545] = new State(-86);
    states[1546] = new State(new int[]{142,1547,26,-87,64,-87,47,-87,50,-87,59,-87,11,-87,41,-87,34,-87,38,-87});
    states[1547] = new State(new int[]{10,1548});
    states[1548] = new State(-88);
    states[1549] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471},new int[]{-160,1550,-159,1087,-131,1088,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096,-323,1098,-138,1102});
    states[1550] = new State(new int[]{8,572,10,-452},new int[]{-117,1551});
    states[1551] = new State(new int[]{10,1075},new int[]{-197,1229});
    states[1552] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,42,471},new int[]{-159,1553,-131,1088,-126,1089,-123,1090,-136,1095,-140,24,-141,27,-181,1096,-323,1098,-138,1102});
    states[1553] = new State(new int[]{8,572,5,-452,10,-452},new int[]{-117,1554});
    states[1554] = new State(new int[]{5,1555,10,1075},new int[]{-197,1264});
    states[1555] = new State(new int[]{137,362,80,25,81,26,75,28,73,29,148,153,150,154,149,155,110,355,109,356,138,148,140,149,139,151,8,390,136,401,21,407,45,415,46,554,31,558,71,562,62,565,41,570,34,598},new int[]{-265,1556,-266,403,-262,360,-86,174,-96,266,-97,267,-170,268,-136,195,-140,24,-141,27,-15,385,-189,386,-154,389,-156,146,-155,150,-263,392,-291,393,-246,399,-239,400,-271,404,-272,405,-268,406,-260,413,-28,414,-253,553,-119,557,-120,561,-216,567,-214,568,-213,569});
    states[1556] = new State(new int[]{10,1075},new int[]{-197,1268});
    states[1557] = new State(-70);
    states[1558] = new State(new int[]{49,14,56,-60,26,-60,64,-60,47,-60,50,-60,59,-60,11,-60,41,-60,34,-60,25,-60,23,-60,27,-60,28,-60,86,-60,97,-60,85,-60},new int[]{-38,1559,-39,1429});
    states[1559] = new State(-51);
    states[1560] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-128,1561,-136,1564,-140,24,-141,27});
    states[1561] = new State(new int[]{10,1562});
    states[1562] = new State(new int[]{3,1493,40,-12,86,-12,97,-12,85,-12,49,-12,56,-12,26,-12,64,-12,47,-12,50,-12,59,-12,11,-12,41,-12,34,-12,25,-12,23,-12,27,-12,28,-12},new int[]{-175,1563,-176,1491,-174,1496});
    states[1563] = new State(-44);
    states[1564] = new State(-48);
    states[1565] = new State(-46);
    states[1566] = new State(-47);
    states[1567] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,86,64,30,65,31,66,24,67,98,68,95,69,32,70,33,71,34,72,37,73,38,74,39,75,97,76,40,77,41,78,43,79,44,80,45,81,91,82,46,83,96,84,47,85,25,86,48,87,68,88,92,89,49,90,50,91,51,92,52,93,53,94,54,95,55,96,56,97,58,98,99,99,100,100,103,101,101,102,102,103,59,104,72,105,35,106,36,107,42,109},new int[]{-146,1568,-127,112,-136,22,-140,24,-141,27,-283,30,-139,31,-284,108});
    states[1568] = new State(new int[]{10,1569,7,20});
    states[1569] = new State(new int[]{3,1493,40,-12,86,-12,97,-12,85,-12,49,-12,56,-12,26,-12,64,-12,47,-12,50,-12,59,-12,11,-12,41,-12,34,-12,25,-12,23,-12,27,-12,28,-12},new int[]{-175,1570,-176,1491,-174,1496});
    states[1570] = new State(-45);
    states[1571] = new State(-4);
    states[1572] = new State(new int[]{47,1574,53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,536,18,246,19,251,37,637,5,646},new int[]{-82,1573,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,455,-121,443,-101,457,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645});
    states[1573] = new State(-5);
    states[1574] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-133,1575,-136,1576,-140,24,-141,27});
    states[1575] = new State(-6);
    states[1576] = new State(new int[]{117,1092,2,-205},new int[]{-144,1342});
    states[1577] = new State(new int[]{137,23,80,25,81,26,75,28,73,29},new int[]{-309,1578,-310,1579,-136,1583,-140,24,-141,27});
    states[1578] = new State(-7);
    states[1579] = new State(new int[]{7,1580,117,167,2,-753},new int[]{-289,1582});
    states[1580] = new State(new int[]{137,23,80,25,81,26,75,28,73,29,79,32,78,33,77,34,76,35,66,36,61,37,122,38,19,39,18,40,60,41,20,42,123,43,124,44,125,45,126,46,127,47,128,48,129,49,130,50,131,51,132,52,21,53,71,54,85,55,22,56,23,57,26,58,27,59,28,60,69,61,93,62,29,63,86,64,30,65,31,66,24,67,98,68,95,69,32,70,33,71,34,72,37,73,38,74,39,75,97,76,40,77,41,78,43,79,44,80,45,81,91,82,46,83,96,84,47,85,25,86,48,87,68,88,92,89,49,90,50,91,51,92,52,93,53,94,54,95,55,96,56,97,58,98,99,99,100,100,103,101,101,102,102,103,59,104,72,105,35,106,36,107,42,109},new int[]{-127,1581,-136,22,-140,24,-141,27,-283,30,-139,31,-284,108});
    states[1581] = new State(-752);
    states[1582] = new State(-754);
    states[1583] = new State(-751);
    states[1584] = new State(new int[]{53,141,138,148,140,149,139,151,148,153,150,154,149,155,60,157,11,321,129,449,110,355,109,356,136,453,135,456,137,23,80,25,81,26,75,28,73,226,42,471,39,501,8,733,18,246,19,251,37,637,5,646,50,815},new int[]{-249,1585,-82,1586,-92,127,-91,128,-90,289,-95,297,-77,302,-76,330,-89,320,-14,142,-154,145,-156,146,-155,150,-15,152,-53,156,-189,451,-102,1587,-121,443,-101,696,-136,535,-140,24,-141,27,-181,470,-247,516,-285,517,-16,518,-54,523,-105,529,-163,530,-258,531,-78,532,-254,585,-256,586,-257,627,-230,628,-106,629,-232,636,-109,645,-4,1588,-303,1589});
    states[1585] = new State(-8);
    states[1586] = new State(-9);
    states[1587] = new State(new int[]{104,495,105,496,106,497,107,498,108,499,132,-738,130,-738,112,-738,111,-738,125,-738,126,-738,127,-738,128,-738,124,-738,110,-738,109,-738,122,-738,123,-738,120,-738,6,-738,5,-738,114,-738,119,-738,117,-738,115,-738,118,-738,116,-738,131,-738,16,-738,2,-738,13,-738,113,-738},new int[]{-184,124});
    states[1588] = new State(-10);
    states[1589] = new State(-11);

    rules[1] = new Rule(-347, new int[]{-1,2});
    rules[2] = new Rule(-1, new int[]{-224});
    rules[3] = new Rule(-1, new int[]{-295});
    rules[4] = new Rule(-1, new int[]{-165});
    rules[5] = new Rule(-165, new int[]{82,-82});
    rules[6] = new Rule(-165, new int[]{82,47,-133});
    rules[7] = new Rule(-165, new int[]{84,-309});
    rules[8] = new Rule(-165, new int[]{83,-249});
    rules[9] = new Rule(-249, new int[]{-82});
    rules[10] = new Rule(-249, new int[]{-4});
    rules[11] = new Rule(-249, new int[]{-303});
    rules[12] = new Rule(-175, new int[]{});
    rules[13] = new Rule(-175, new int[]{-176});
    rules[14] = new Rule(-176, new int[]{-174});
    rules[15] = new Rule(-176, new int[]{-176,-174});
    rules[16] = new Rule(-174, new int[]{3,137});
    rules[17] = new Rule(-174, new int[]{3,138});
    rules[18] = new Rule(-224, new int[]{-225,-175,-293,-17,-178});
    rules[19] = new Rule(-178, new int[]{7});
    rules[20] = new Rule(-178, new int[]{10});
    rules[21] = new Rule(-178, new int[]{5});
    rules[22] = new Rule(-178, new int[]{94});
    rules[23] = new Rule(-178, new int[]{6});
    rules[24] = new Rule(-178, new int[]{});
    rules[25] = new Rule(-225, new int[]{});
    rules[26] = new Rule(-225, new int[]{58,-136,-177});
    rules[27] = new Rule(-177, new int[]{10});
    rules[28] = new Rule(-177, new int[]{8,-179,9,10});
    rules[29] = new Rule(-179, new int[]{-135});
    rules[30] = new Rule(-179, new int[]{-179,94,-135});
    rules[31] = new Rule(-135, new int[]{-136});
    rules[32] = new Rule(-17, new int[]{-34,-245});
    rules[33] = new Rule(-34, new int[]{-38});
    rules[34] = new Rule(-146, new int[]{-127});
    rules[35] = new Rule(-146, new int[]{-146,7,-127});
    rules[36] = new Rule(-293, new int[]{});
    rules[37] = new Rule(-293, new int[]{-293,49,-294,10});
    rules[38] = new Rule(-294, new int[]{-296});
    rules[39] = new Rule(-294, new int[]{-294,94,-296});
    rules[40] = new Rule(-296, new int[]{-146});
    rules[41] = new Rule(-296, new int[]{-146,131,138});
    rules[42] = new Rule(-295, new int[]{-6,-297,-151,-150,-143,7});
    rules[43] = new Rule(-295, new int[]{-6,-297,-152,-143,7});
    rules[44] = new Rule(-297, new int[]{-2,-128,10,-175});
    rules[45] = new Rule(-297, new int[]{103,-146,10,-175});
    rules[46] = new Rule(-2, new int[]{99});
    rules[47] = new Rule(-2, new int[]{100});
    rules[48] = new Rule(-128, new int[]{-136});
    rules[49] = new Rule(-151, new int[]{40,-293,-37});
    rules[50] = new Rule(-150, new int[]{38,-293,-38});
    rules[51] = new Rule(-152, new int[]{-293,-38});
    rules[52] = new Rule(-143, new int[]{86});
    rules[53] = new Rule(-143, new int[]{97,-242,86});
    rules[54] = new Rule(-143, new int[]{97,-242,98,-242,86});
    rules[55] = new Rule(-143, new int[]{85,-242,86});
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
    rules[67] = new Rule(-44, new int[]{-279});
    rules[68] = new Rule(-44, new int[]{-298});
    rules[69] = new Rule(-44, new int[]{-222});
    rules[70] = new Rule(-44, new int[]{-221});
    rules[71] = new Rule(-43, new int[]{-157});
    rules[72] = new Rule(-43, new int[]{-26});
    rules[73] = new Rule(-43, new int[]{-48});
    rules[74] = new Rule(-43, new int[]{-279});
    rules[75] = new Rule(-43, new int[]{-298});
    rules[76] = new Rule(-43, new int[]{-210});
    rules[77] = new Rule(-203, new int[]{-204});
    rules[78] = new Rule(-203, new int[]{-207});
    rules[79] = new Rule(-210, new int[]{-6,-203});
    rules[80] = new Rule(-42, new int[]{-157});
    rules[81] = new Rule(-42, new int[]{-26});
    rules[82] = new Rule(-42, new int[]{-48});
    rules[83] = new Rule(-42, new int[]{-279});
    rules[84] = new Rule(-42, new int[]{-298});
    rules[85] = new Rule(-222, new int[]{-6,-215});
    rules[86] = new Rule(-222, new int[]{-6,-215,142,10});
    rules[87] = new Rule(-221, new int[]{-6,-219});
    rules[88] = new Rule(-221, new int[]{-6,-219,142,10});
    rules[89] = new Rule(-157, new int[]{56,-145,10});
    rules[90] = new Rule(-145, new int[]{-132});
    rules[91] = new Rule(-145, new int[]{-145,94,-132});
    rules[92] = new Rule(-132, new int[]{148});
    rules[93] = new Rule(-132, new int[]{-136});
    rules[94] = new Rule(-26, new int[]{26,-24});
    rules[95] = new Rule(-26, new int[]{-26,-24});
    rules[96] = new Rule(-48, new int[]{64,-24});
    rules[97] = new Rule(-48, new int[]{-48,-24});
    rules[98] = new Rule(-279, new int[]{47,-45});
    rules[99] = new Rule(-279, new int[]{-279,-45});
    rules[100] = new Rule(-302, new int[]{-299});
    rules[101] = new Rule(-302, new int[]{8,-136,94,-147,9,104,-92,10});
    rules[102] = new Rule(-298, new int[]{50,-302});
    rules[103] = new Rule(-298, new int[]{59,-302});
    rules[104] = new Rule(-298, new int[]{-298,-302});
    rules[105] = new Rule(-24, new int[]{-25,10});
    rules[106] = new Rule(-25, new int[]{-130,114,-99});
    rules[107] = new Rule(-25, new int[]{-130,5,-266,114,-79});
    rules[108] = new Rule(-99, new int[]{-84});
    rules[109] = new Rule(-99, new int[]{-88});
    rules[110] = new Rule(-130, new int[]{-136});
    rules[111] = new Rule(-73, new int[]{-92});
    rules[112] = new Rule(-73, new int[]{-73,94,-92});
    rules[113] = new Rule(-84, new int[]{-75});
    rules[114] = new Rule(-84, new int[]{-75,-182,-75});
    rules[115] = new Rule(-84, new int[]{-231});
    rules[116] = new Rule(-231, new int[]{-84,13,-84,5,-84});
    rules[117] = new Rule(-182, new int[]{114});
    rules[118] = new Rule(-182, new int[]{119});
    rules[119] = new Rule(-182, new int[]{117});
    rules[120] = new Rule(-182, new int[]{115});
    rules[121] = new Rule(-182, new int[]{118});
    rules[122] = new Rule(-182, new int[]{116});
    rules[123] = new Rule(-182, new int[]{131});
    rules[124] = new Rule(-75, new int[]{-12});
    rules[125] = new Rule(-75, new int[]{-75,-183,-12});
    rules[126] = new Rule(-183, new int[]{110});
    rules[127] = new Rule(-183, new int[]{109});
    rules[128] = new Rule(-183, new int[]{122});
    rules[129] = new Rule(-183, new int[]{123});
    rules[130] = new Rule(-255, new int[]{-12,-191,-274});
    rules[131] = new Rule(-259, new int[]{-10,113,-10});
    rules[132] = new Rule(-12, new int[]{-10});
    rules[133] = new Rule(-12, new int[]{-255});
    rules[134] = new Rule(-12, new int[]{-259});
    rules[135] = new Rule(-12, new int[]{-12,-185,-10});
    rules[136] = new Rule(-12, new int[]{-12,-185,-259});
    rules[137] = new Rule(-185, new int[]{112});
    rules[138] = new Rule(-185, new int[]{111});
    rules[139] = new Rule(-185, new int[]{125});
    rules[140] = new Rule(-185, new int[]{126});
    rules[141] = new Rule(-185, new int[]{127});
    rules[142] = new Rule(-185, new int[]{128});
    rules[143] = new Rule(-185, new int[]{124});
    rules[144] = new Rule(-10, new int[]{-13});
    rules[145] = new Rule(-10, new int[]{-229});
    rules[146] = new Rule(-10, new int[]{53});
    rules[147] = new Rule(-10, new int[]{135,-10});
    rules[148] = new Rule(-10, new int[]{8,-84,9});
    rules[149] = new Rule(-10, new int[]{129,-10});
    rules[150] = new Rule(-10, new int[]{-189,-10});
    rules[151] = new Rule(-10, new int[]{-163});
    rules[152] = new Rule(-229, new int[]{11,-69,12});
    rules[153] = new Rule(-189, new int[]{110});
    rules[154] = new Rule(-189, new int[]{109});
    rules[155] = new Rule(-13, new int[]{-136});
    rules[156] = new Rule(-13, new int[]{-154});
    rules[157] = new Rule(-13, new int[]{-15});
    rules[158] = new Rule(-13, new int[]{39,-136});
    rules[159] = new Rule(-13, new int[]{-247});
    rules[160] = new Rule(-13, new int[]{-285});
    rules[161] = new Rule(-13, new int[]{-13,-11});
    rules[162] = new Rule(-13, new int[]{-13,4,-289});
    rules[163] = new Rule(-13, new int[]{-13,11,-110,12});
    rules[164] = new Rule(-11, new int[]{7,-127});
    rules[165] = new Rule(-11, new int[]{136});
    rules[166] = new Rule(-11, new int[]{8,-70,9});
    rules[167] = new Rule(-11, new int[]{11,-69,12});
    rules[168] = new Rule(-70, new int[]{-66});
    rules[169] = new Rule(-70, new int[]{});
    rules[170] = new Rule(-69, new int[]{-67});
    rules[171] = new Rule(-69, new int[]{});
    rules[172] = new Rule(-67, new int[]{-87});
    rules[173] = new Rule(-67, new int[]{-67,94,-87});
    rules[174] = new Rule(-87, new int[]{-84});
    rules[175] = new Rule(-87, new int[]{-84,6,-84});
    rules[176] = new Rule(-15, new int[]{148});
    rules[177] = new Rule(-15, new int[]{150});
    rules[178] = new Rule(-15, new int[]{149});
    rules[179] = new Rule(-79, new int[]{-84});
    rules[180] = new Rule(-79, new int[]{-88});
    rules[181] = new Rule(-79, new int[]{-233});
    rules[182] = new Rule(-88, new int[]{8,-62,9});
    rules[183] = new Rule(-62, new int[]{});
    rules[184] = new Rule(-62, new int[]{-61});
    rules[185] = new Rule(-61, new int[]{-80});
    rules[186] = new Rule(-61, new int[]{-61,94,-80});
    rules[187] = new Rule(-233, new int[]{8,-235,9});
    rules[188] = new Rule(-235, new int[]{-234});
    rules[189] = new Rule(-235, new int[]{-234,10});
    rules[190] = new Rule(-234, new int[]{-236});
    rules[191] = new Rule(-234, new int[]{-234,10,-236});
    rules[192] = new Rule(-236, new int[]{-125,5,-79});
    rules[193] = new Rule(-125, new int[]{-136});
    rules[194] = new Rule(-45, new int[]{-6,-46});
    rules[195] = new Rule(-6, new int[]{-240});
    rules[196] = new Rule(-6, new int[]{-6,-240});
    rules[197] = new Rule(-6, new int[]{});
    rules[198] = new Rule(-240, new int[]{11,-241,12});
    rules[199] = new Rule(-241, new int[]{-8});
    rules[200] = new Rule(-241, new int[]{-241,94,-8});
    rules[201] = new Rule(-8, new int[]{-9});
    rules[202] = new Rule(-8, new int[]{-136,5,-9});
    rules[203] = new Rule(-46, new int[]{-133,114,-277,10});
    rules[204] = new Rule(-46, new int[]{-134,-277,10});
    rules[205] = new Rule(-133, new int[]{-136});
    rules[206] = new Rule(-133, new int[]{-136,-144});
    rules[207] = new Rule(-134, new int[]{-136,117,-147,116});
    rules[208] = new Rule(-277, new int[]{-266});
    rules[209] = new Rule(-277, new int[]{-27});
    rules[210] = new Rule(-263, new int[]{-262,13});
    rules[211] = new Rule(-263, new int[]{-291,13});
    rules[212] = new Rule(-266, new int[]{-262});
    rules[213] = new Rule(-266, new int[]{-263});
    rules[214] = new Rule(-266, new int[]{-246});
    rules[215] = new Rule(-266, new int[]{-239});
    rules[216] = new Rule(-266, new int[]{-271});
    rules[217] = new Rule(-266, new int[]{-216});
    rules[218] = new Rule(-266, new int[]{-291});
    rules[219] = new Rule(-291, new int[]{-170,-289});
    rules[220] = new Rule(-289, new int[]{117,-287,115});
    rules[221] = new Rule(-290, new int[]{119});
    rules[222] = new Rule(-290, new int[]{117,-288,115});
    rules[223] = new Rule(-287, new int[]{-269});
    rules[224] = new Rule(-287, new int[]{-287,94,-269});
    rules[225] = new Rule(-288, new int[]{-270});
    rules[226] = new Rule(-288, new int[]{-288,94,-270});
    rules[227] = new Rule(-270, new int[]{});
    rules[228] = new Rule(-269, new int[]{-262});
    rules[229] = new Rule(-269, new int[]{-262,13});
    rules[230] = new Rule(-269, new int[]{-271});
    rules[231] = new Rule(-269, new int[]{-216});
    rules[232] = new Rule(-269, new int[]{-291});
    rules[233] = new Rule(-262, new int[]{-86});
    rules[234] = new Rule(-262, new int[]{-86,6,-86});
    rules[235] = new Rule(-262, new int[]{8,-74,9});
    rules[236] = new Rule(-86, new int[]{-96});
    rules[237] = new Rule(-86, new int[]{-86,-183,-96});
    rules[238] = new Rule(-96, new int[]{-97});
    rules[239] = new Rule(-96, new int[]{-96,-185,-97});
    rules[240] = new Rule(-97, new int[]{-170});
    rules[241] = new Rule(-97, new int[]{-15});
    rules[242] = new Rule(-97, new int[]{-189,-97});
    rules[243] = new Rule(-97, new int[]{-154});
    rules[244] = new Rule(-97, new int[]{-97,8,-69,9});
    rules[245] = new Rule(-170, new int[]{-136});
    rules[246] = new Rule(-170, new int[]{-170,7,-127});
    rules[247] = new Rule(-74, new int[]{-72,94,-72});
    rules[248] = new Rule(-74, new int[]{-74,94,-72});
    rules[249] = new Rule(-72, new int[]{-266});
    rules[250] = new Rule(-72, new int[]{-266,114,-82});
    rules[251] = new Rule(-239, new int[]{136,-265});
    rules[252] = new Rule(-271, new int[]{-272});
    rules[253] = new Rule(-271, new int[]{62,-272});
    rules[254] = new Rule(-272, new int[]{-268});
    rules[255] = new Rule(-272, new int[]{-28});
    rules[256] = new Rule(-272, new int[]{-253});
    rules[257] = new Rule(-272, new int[]{-119});
    rules[258] = new Rule(-272, new int[]{-120});
    rules[259] = new Rule(-120, new int[]{71,55,-266});
    rules[260] = new Rule(-268, new int[]{21,11,-153,12,55,-266});
    rules[261] = new Rule(-268, new int[]{-260});
    rules[262] = new Rule(-260, new int[]{21,55,-266});
    rules[263] = new Rule(-153, new int[]{-261});
    rules[264] = new Rule(-153, new int[]{-153,94,-261});
    rules[265] = new Rule(-261, new int[]{-262});
    rules[266] = new Rule(-261, new int[]{});
    rules[267] = new Rule(-253, new int[]{46,55,-266});
    rules[268] = new Rule(-119, new int[]{31,55,-266});
    rules[269] = new Rule(-119, new int[]{31});
    rules[270] = new Rule(-246, new int[]{137,11,-84,12});
    rules[271] = new Rule(-216, new int[]{-214});
    rules[272] = new Rule(-214, new int[]{-213});
    rules[273] = new Rule(-213, new int[]{41,-117});
    rules[274] = new Rule(-213, new int[]{34,-117,5,-265});
    rules[275] = new Rule(-213, new int[]{-170,121,-269});
    rules[276] = new Rule(-213, new int[]{-291,121,-269});
    rules[277] = new Rule(-213, new int[]{8,9,121,-269});
    rules[278] = new Rule(-213, new int[]{8,-74,9,121,-269});
    rules[279] = new Rule(-213, new int[]{-170,121,8,9});
    rules[280] = new Rule(-213, new int[]{-291,121,8,9});
    rules[281] = new Rule(-213, new int[]{8,9,121,8,9});
    rules[282] = new Rule(-213, new int[]{8,-74,9,121,8,9});
    rules[283] = new Rule(-27, new int[]{-20,-281,-173,-306,-23});
    rules[284] = new Rule(-28, new int[]{45,-173,-306,-22,86});
    rules[285] = new Rule(-19, new int[]{66});
    rules[286] = new Rule(-19, new int[]{67});
    rules[287] = new Rule(-19, new int[]{141});
    rules[288] = new Rule(-19, new int[]{24});
    rules[289] = new Rule(-19, new int[]{25});
    rules[290] = new Rule(-20, new int[]{});
    rules[291] = new Rule(-20, new int[]{-21});
    rules[292] = new Rule(-21, new int[]{-19});
    rules[293] = new Rule(-21, new int[]{-21,-19});
    rules[294] = new Rule(-281, new int[]{23});
    rules[295] = new Rule(-281, new int[]{40});
    rules[296] = new Rule(-281, new int[]{61});
    rules[297] = new Rule(-281, new int[]{61,23});
    rules[298] = new Rule(-281, new int[]{61,45});
    rules[299] = new Rule(-281, new int[]{61,40});
    rules[300] = new Rule(-23, new int[]{});
    rules[301] = new Rule(-23, new int[]{-22,86});
    rules[302] = new Rule(-173, new int[]{});
    rules[303] = new Rule(-173, new int[]{8,-172,9});
    rules[304] = new Rule(-172, new int[]{-171});
    rules[305] = new Rule(-172, new int[]{-172,94,-171});
    rules[306] = new Rule(-171, new int[]{-170});
    rules[307] = new Rule(-171, new int[]{-291});
    rules[308] = new Rule(-144, new int[]{117,-147,115});
    rules[309] = new Rule(-306, new int[]{});
    rules[310] = new Rule(-306, new int[]{-305});
    rules[311] = new Rule(-305, new int[]{-304});
    rules[312] = new Rule(-305, new int[]{-305,-304});
    rules[313] = new Rule(-304, new int[]{20,-147,5,-278,10});
    rules[314] = new Rule(-278, new int[]{-275});
    rules[315] = new Rule(-278, new int[]{-278,94,-275});
    rules[316] = new Rule(-275, new int[]{-266});
    rules[317] = new Rule(-275, new int[]{23});
    rules[318] = new Rule(-275, new int[]{45});
    rules[319] = new Rule(-275, new int[]{27});
    rules[320] = new Rule(-22, new int[]{-29});
    rules[321] = new Rule(-22, new int[]{-22,-7,-29});
    rules[322] = new Rule(-7, new int[]{79});
    rules[323] = new Rule(-7, new int[]{78});
    rules[324] = new Rule(-7, new int[]{77});
    rules[325] = new Rule(-7, new int[]{76});
    rules[326] = new Rule(-29, new int[]{});
    rules[327] = new Rule(-29, new int[]{-31,-180});
    rules[328] = new Rule(-29, new int[]{-30});
    rules[329] = new Rule(-29, new int[]{-31,10,-30});
    rules[330] = new Rule(-147, new int[]{-136});
    rules[331] = new Rule(-147, new int[]{-147,94,-136});
    rules[332] = new Rule(-180, new int[]{});
    rules[333] = new Rule(-180, new int[]{10});
    rules[334] = new Rule(-31, new int[]{-41});
    rules[335] = new Rule(-31, new int[]{-31,10,-41});
    rules[336] = new Rule(-41, new int[]{-6,-47});
    rules[337] = new Rule(-30, new int[]{-50});
    rules[338] = new Rule(-30, new int[]{-30,-50});
    rules[339] = new Rule(-50, new int[]{-49});
    rules[340] = new Rule(-50, new int[]{-51});
    rules[341] = new Rule(-47, new int[]{26,-25});
    rules[342] = new Rule(-47, new int[]{-301});
    rules[343] = new Rule(-47, new int[]{-3,-301});
    rules[344] = new Rule(-3, new int[]{25});
    rules[345] = new Rule(-3, new int[]{23});
    rules[346] = new Rule(-301, new int[]{-300});
    rules[347] = new Rule(-301, new int[]{59,-147,5,-266});
    rules[348] = new Rule(-49, new int[]{-6,-212});
    rules[349] = new Rule(-49, new int[]{-6,-209});
    rules[350] = new Rule(-209, new int[]{-205});
    rules[351] = new Rule(-209, new int[]{-208});
    rules[352] = new Rule(-212, new int[]{-3,-220});
    rules[353] = new Rule(-212, new int[]{-220});
    rules[354] = new Rule(-212, new int[]{-217});
    rules[355] = new Rule(-220, new int[]{-218});
    rules[356] = new Rule(-218, new int[]{-215});
    rules[357] = new Rule(-218, new int[]{-219});
    rules[358] = new Rule(-217, new int[]{27,-161,-117,-197});
    rules[359] = new Rule(-217, new int[]{-3,27,-161,-117,-197});
    rules[360] = new Rule(-217, new int[]{28,-161,-117,-197});
    rules[361] = new Rule(-161, new int[]{-160});
    rules[362] = new Rule(-161, new int[]{});
    rules[363] = new Rule(-162, new int[]{-136});
    rules[364] = new Rule(-162, new int[]{-139});
    rules[365] = new Rule(-162, new int[]{-162,7,-136});
    rules[366] = new Rule(-162, new int[]{-162,7,-139});
    rules[367] = new Rule(-51, new int[]{-6,-248});
    rules[368] = new Rule(-248, new int[]{43,-162,-223,-192,10,-195});
    rules[369] = new Rule(-248, new int[]{43,-162,-223,-192,10,-200,10,-195});
    rules[370] = new Rule(-248, new int[]{-3,43,-162,-223,-192,10,-195});
    rules[371] = new Rule(-248, new int[]{-3,43,-162,-223,-192,10,-200,10,-195});
    rules[372] = new Rule(-248, new int[]{24,43,-162,-223,-201,10});
    rules[373] = new Rule(-248, new int[]{-3,24,43,-162,-223,-201,10});
    rules[374] = new Rule(-201, new int[]{104,-82});
    rules[375] = new Rule(-201, new int[]{});
    rules[376] = new Rule(-195, new int[]{});
    rules[377] = new Rule(-195, new int[]{60,10});
    rules[378] = new Rule(-223, new int[]{-228,5,-265});
    rules[379] = new Rule(-228, new int[]{});
    rules[380] = new Rule(-228, new int[]{11,-227,12});
    rules[381] = new Rule(-227, new int[]{-226});
    rules[382] = new Rule(-227, new int[]{-227,10,-226});
    rules[383] = new Rule(-226, new int[]{-147,5,-265});
    rules[384] = new Rule(-103, new int[]{-83});
    rules[385] = new Rule(-103, new int[]{});
    rules[386] = new Rule(-192, new int[]{});
    rules[387] = new Rule(-192, new int[]{80,-103,-193});
    rules[388] = new Rule(-192, new int[]{81,-250,-194});
    rules[389] = new Rule(-193, new int[]{});
    rules[390] = new Rule(-193, new int[]{81,-250});
    rules[391] = new Rule(-194, new int[]{});
    rules[392] = new Rule(-194, new int[]{80,-103});
    rules[393] = new Rule(-299, new int[]{-300,10});
    rules[394] = new Rule(-327, new int[]{104});
    rules[395] = new Rule(-327, new int[]{114});
    rules[396] = new Rule(-300, new int[]{-147,5,-266});
    rules[397] = new Rule(-300, new int[]{-147,104,-82});
    rules[398] = new Rule(-300, new int[]{-147,5,-266,-327,-81});
    rules[399] = new Rule(-81, new int[]{-80});
    rules[400] = new Rule(-81, new int[]{-312});
    rules[401] = new Rule(-81, new int[]{-136,121,-317});
    rules[402] = new Rule(-81, new int[]{8,9,-313,121,-317});
    rules[403] = new Rule(-81, new int[]{8,-62,9,121,-317});
    rules[404] = new Rule(-80, new int[]{-79});
    rules[405] = new Rule(-80, new int[]{-53});
    rules[406] = new Rule(-207, new int[]{-217,-167});
    rules[407] = new Rule(-207, new int[]{27,-161,-117,104,-250,10});
    rules[408] = new Rule(-207, new int[]{-3,27,-161,-117,104,-250,10});
    rules[409] = new Rule(-208, new int[]{-217,-166});
    rules[410] = new Rule(-208, new int[]{27,-161,-117,104,-250,10});
    rules[411] = new Rule(-208, new int[]{-3,27,-161,-117,104,-250,10});
    rules[412] = new Rule(-204, new int[]{-211});
    rules[413] = new Rule(-204, new int[]{-3,-211});
    rules[414] = new Rule(-211, new int[]{-218,-168});
    rules[415] = new Rule(-211, new int[]{34,-159,-117,5,-265,-198,104,-92,10});
    rules[416] = new Rule(-211, new int[]{34,-159,-117,-198,104,-92,10});
    rules[417] = new Rule(-211, new int[]{34,-159,-117,5,-265,-198,104,-311,10});
    rules[418] = new Rule(-211, new int[]{34,-159,-117,-198,104,-311,10});
    rules[419] = new Rule(-211, new int[]{41,-160,-117,-198,104,-250,10});
    rules[420] = new Rule(-211, new int[]{-218,142,10});
    rules[421] = new Rule(-205, new int[]{-206});
    rules[422] = new Rule(-205, new int[]{-3,-206});
    rules[423] = new Rule(-206, new int[]{-218,-166});
    rules[424] = new Rule(-206, new int[]{34,-159,-117,5,-265,-198,104,-93,10});
    rules[425] = new Rule(-206, new int[]{34,-159,-117,-198,104,-93,10});
    rules[426] = new Rule(-206, new int[]{41,-160,-117,-198,104,-250,10});
    rules[427] = new Rule(-168, new int[]{-167});
    rules[428] = new Rule(-168, new int[]{-57});
    rules[429] = new Rule(-160, new int[]{-159});
    rules[430] = new Rule(-159, new int[]{-131});
    rules[431] = new Rule(-159, new int[]{-323,7,-131});
    rules[432] = new Rule(-138, new int[]{-126});
    rules[433] = new Rule(-323, new int[]{-138});
    rules[434] = new Rule(-323, new int[]{-323,7,-138});
    rules[435] = new Rule(-131, new int[]{-126});
    rules[436] = new Rule(-131, new int[]{-181});
    rules[437] = new Rule(-131, new int[]{-181,-144});
    rules[438] = new Rule(-126, new int[]{-123});
    rules[439] = new Rule(-126, new int[]{-123,-144});
    rules[440] = new Rule(-123, new int[]{-136});
    rules[441] = new Rule(-215, new int[]{41,-160,-117,-197,-306});
    rules[442] = new Rule(-219, new int[]{34,-159,-117,-197,-306});
    rules[443] = new Rule(-219, new int[]{34,-159,-117,5,-265,-197,-306});
    rules[444] = new Rule(-57, new int[]{101,-98,75,-98,10});
    rules[445] = new Rule(-57, new int[]{101,-98,10});
    rules[446] = new Rule(-57, new int[]{101,10});
    rules[447] = new Rule(-98, new int[]{-136});
    rules[448] = new Rule(-98, new int[]{-154});
    rules[449] = new Rule(-167, new int[]{-38,-245,10});
    rules[450] = new Rule(-166, new int[]{-40,-245,10});
    rules[451] = new Rule(-166, new int[]{-57});
    rules[452] = new Rule(-117, new int[]{});
    rules[453] = new Rule(-117, new int[]{8,9});
    rules[454] = new Rule(-117, new int[]{8,-118,9});
    rules[455] = new Rule(-118, new int[]{-52});
    rules[456] = new Rule(-118, new int[]{-118,10,-52});
    rules[457] = new Rule(-52, new int[]{-6,-286});
    rules[458] = new Rule(-286, new int[]{-148,5,-265});
    rules[459] = new Rule(-286, new int[]{50,-148,5,-265});
    rules[460] = new Rule(-286, new int[]{26,-148,5,-265});
    rules[461] = new Rule(-286, new int[]{102,-148,5,-265});
    rules[462] = new Rule(-286, new int[]{-148,5,-265,104,-82});
    rules[463] = new Rule(-286, new int[]{50,-148,5,-265,104,-82});
    rules[464] = new Rule(-286, new int[]{26,-148,5,-265,104,-82});
    rules[465] = new Rule(-148, new int[]{-124});
    rules[466] = new Rule(-148, new int[]{-148,94,-124});
    rules[467] = new Rule(-124, new int[]{-136});
    rules[468] = new Rule(-265, new int[]{-266});
    rules[469] = new Rule(-267, new int[]{-262});
    rules[470] = new Rule(-267, new int[]{-246});
    rules[471] = new Rule(-267, new int[]{-239});
    rules[472] = new Rule(-267, new int[]{-271});
    rules[473] = new Rule(-267, new int[]{-291});
    rules[474] = new Rule(-251, new int[]{-250});
    rules[475] = new Rule(-251, new int[]{-132,5,-251});
    rules[476] = new Rule(-250, new int[]{});
    rules[477] = new Rule(-250, new int[]{-4});
    rules[478] = new Rule(-250, new int[]{-202});
    rules[479] = new Rule(-250, new int[]{-122});
    rules[480] = new Rule(-250, new int[]{-245});
    rules[481] = new Rule(-250, new int[]{-142});
    rules[482] = new Rule(-250, new int[]{-32});
    rules[483] = new Rule(-250, new int[]{-237});
    rules[484] = new Rule(-250, new int[]{-307});
    rules[485] = new Rule(-250, new int[]{-113});
    rules[486] = new Rule(-250, new int[]{-308});
    rules[487] = new Rule(-250, new int[]{-149});
    rules[488] = new Rule(-250, new int[]{-292});
    rules[489] = new Rule(-250, new int[]{-238});
    rules[490] = new Rule(-250, new int[]{-112});
    rules[491] = new Rule(-250, new int[]{-303});
    rules[492] = new Rule(-250, new int[]{-55});
    rules[493] = new Rule(-250, new int[]{-158});
    rules[494] = new Rule(-250, new int[]{-115});
    rules[495] = new Rule(-250, new int[]{-116});
    rules[496] = new Rule(-250, new int[]{-114});
    rules[497] = new Rule(-250, new int[]{-337});
    rules[498] = new Rule(-114, new int[]{70,-92,93,-250});
    rules[499] = new Rule(-115, new int[]{72,-92});
    rules[500] = new Rule(-116, new int[]{72,71,-92});
    rules[501] = new Rule(-303, new int[]{50,-300});
    rules[502] = new Rule(-303, new int[]{8,50,-136,94,-326,9,104,-82});
    rules[503] = new Rule(-303, new int[]{50,8,-136,94,-147,9,104,-82});
    rules[504] = new Rule(-4, new int[]{-102,-184,-83});
    rules[505] = new Rule(-4, new int[]{8,-101,94,-325,9,-184,-82});
    rules[506] = new Rule(-4, new int[]{-101,17,-109,12,-184,-82});
    rules[507] = new Rule(-325, new int[]{-101});
    rules[508] = new Rule(-325, new int[]{-325,94,-101});
    rules[509] = new Rule(-326, new int[]{50,-136});
    rules[510] = new Rule(-326, new int[]{-326,94,50,-136});
    rules[511] = new Rule(-202, new int[]{-102});
    rules[512] = new Rule(-122, new int[]{54,-132});
    rules[513] = new Rule(-245, new int[]{85,-242,86});
    rules[514] = new Rule(-242, new int[]{-251});
    rules[515] = new Rule(-242, new int[]{-242,10,-251});
    rules[516] = new Rule(-142, new int[]{37,-92,48,-250});
    rules[517] = new Rule(-142, new int[]{37,-92,48,-250,29,-250});
    rules[518] = new Rule(-337, new int[]{35,-92,52,-339,-243,86});
    rules[519] = new Rule(-337, new int[]{35,-92,52,-339,10,-243,86});
    rules[520] = new Rule(-339, new int[]{-338});
    rules[521] = new Rule(-339, new int[]{-339,10,-338});
    rules[522] = new Rule(-338, new int[]{-331,36,-92,5,-250});
    rules[523] = new Rule(-338, new int[]{-330,5,-250});
    rules[524] = new Rule(-338, new int[]{-332,5,-250});
    rules[525] = new Rule(-338, new int[]{-333,36,-92,5,-250});
    rules[526] = new Rule(-338, new int[]{-333,5,-250});
    rules[527] = new Rule(-32, new int[]{22,-92,55,-33,-243,86});
    rules[528] = new Rule(-32, new int[]{22,-92,55,-33,10,-243,86});
    rules[529] = new Rule(-32, new int[]{22,-92,55,-243,86});
    rules[530] = new Rule(-33, new int[]{-252});
    rules[531] = new Rule(-33, new int[]{-33,10,-252});
    rules[532] = new Rule(-252, new int[]{-68,5,-250});
    rules[533] = new Rule(-68, new int[]{-100});
    rules[534] = new Rule(-68, new int[]{-68,94,-100});
    rules[535] = new Rule(-100, new int[]{-87});
    rules[536] = new Rule(-243, new int[]{});
    rules[537] = new Rule(-243, new int[]{29,-242});
    rules[538] = new Rule(-237, new int[]{91,-242,92,-82});
    rules[539] = new Rule(-307, new int[]{51,-92,-282,-250});
    rules[540] = new Rule(-282, new int[]{93});
    rules[541] = new Rule(-282, new int[]{});
    rules[542] = new Rule(-158, new int[]{57,-92,93,-250});
    rules[543] = new Rule(-112, new int[]{33,-136,-264,131,-92,93,-250});
    rules[544] = new Rule(-112, new int[]{33,50,-136,5,-266,131,-92,93,-250});
    rules[545] = new Rule(-112, new int[]{33,50,-136,131,-92,93,-250});
    rules[546] = new Rule(-264, new int[]{5,-266});
    rules[547] = new Rule(-264, new int[]{});
    rules[548] = new Rule(-113, new int[]{32,-18,-136,-276,-92,-108,-92,-282,-250});
    rules[549] = new Rule(-18, new int[]{50});
    rules[550] = new Rule(-18, new int[]{});
    rules[551] = new Rule(-276, new int[]{104});
    rules[552] = new Rule(-276, new int[]{5,-170,104});
    rules[553] = new Rule(-108, new int[]{68});
    rules[554] = new Rule(-108, new int[]{69});
    rules[555] = new Rule(-308, new int[]{52,-66,93,-250});
    rules[556] = new Rule(-149, new int[]{39});
    rules[557] = new Rule(-292, new int[]{96,-242,-280});
    rules[558] = new Rule(-280, new int[]{95,-242,86});
    rules[559] = new Rule(-280, new int[]{30,-56,86});
    rules[560] = new Rule(-56, new int[]{-59,-244});
    rules[561] = new Rule(-56, new int[]{-59,10,-244});
    rules[562] = new Rule(-56, new int[]{-242});
    rules[563] = new Rule(-59, new int[]{-58});
    rules[564] = new Rule(-59, new int[]{-59,10,-58});
    rules[565] = new Rule(-244, new int[]{});
    rules[566] = new Rule(-244, new int[]{29,-242});
    rules[567] = new Rule(-58, new int[]{74,-60,93,-250});
    rules[568] = new Rule(-60, new int[]{-169});
    rules[569] = new Rule(-60, new int[]{-129,5,-169});
    rules[570] = new Rule(-169, new int[]{-170});
    rules[571] = new Rule(-129, new int[]{-136});
    rules[572] = new Rule(-238, new int[]{44});
    rules[573] = new Rule(-238, new int[]{44,-82});
    rules[574] = new Rule(-66, new int[]{-83});
    rules[575] = new Rule(-66, new int[]{-66,94,-83});
    rules[576] = new Rule(-55, new int[]{-164});
    rules[577] = new Rule(-164, new int[]{-163});
    rules[578] = new Rule(-83, new int[]{-82});
    rules[579] = new Rule(-83, new int[]{-311});
    rules[580] = new Rule(-82, new int[]{-92});
    rules[581] = new Rule(-82, new int[]{-109});
    rules[582] = new Rule(-92, new int[]{-91});
    rules[583] = new Rule(-92, new int[]{-230});
    rules[584] = new Rule(-92, new int[]{-232});
    rules[585] = new Rule(-106, new int[]{-91});
    rules[586] = new Rule(-106, new int[]{-230});
    rules[587] = new Rule(-107, new int[]{-91});
    rules[588] = new Rule(-107, new int[]{-232});
    rules[589] = new Rule(-93, new int[]{-92});
    rules[590] = new Rule(-93, new int[]{-311});
    rules[591] = new Rule(-94, new int[]{-91});
    rules[592] = new Rule(-94, new int[]{-230});
    rules[593] = new Rule(-94, new int[]{-311});
    rules[594] = new Rule(-91, new int[]{-90});
    rules[595] = new Rule(-91, new int[]{-91,16,-90});
    rules[596] = new Rule(-247, new int[]{18,8,-274,9});
    rules[597] = new Rule(-285, new int[]{19,8,-274,9});
    rules[598] = new Rule(-285, new int[]{19,8,-273,9});
    rules[599] = new Rule(-230, new int[]{-106,13,-106,5,-106});
    rules[600] = new Rule(-232, new int[]{37,-107,48,-107,29,-107});
    rules[601] = new Rule(-273, new int[]{-170,-290});
    rules[602] = new Rule(-273, new int[]{-170,4,-290});
    rules[603] = new Rule(-274, new int[]{-170});
    rules[604] = new Rule(-274, new int[]{-170,-289});
    rules[605] = new Rule(-274, new int[]{-170,4,-289});
    rules[606] = new Rule(-5, new int[]{8,-62,9});
    rules[607] = new Rule(-5, new int[]{});
    rules[608] = new Rule(-163, new int[]{73,-274,-65});
    rules[609] = new Rule(-163, new int[]{73,-274,11,-63,12,-5});
    rules[610] = new Rule(-163, new int[]{73,23,8,-322,9});
    rules[611] = new Rule(-321, new int[]{-136,104,-90});
    rules[612] = new Rule(-321, new int[]{-90});
    rules[613] = new Rule(-322, new int[]{-321});
    rules[614] = new Rule(-322, new int[]{-322,94,-321});
    rules[615] = new Rule(-65, new int[]{});
    rules[616] = new Rule(-65, new int[]{8,-63,9});
    rules[617] = new Rule(-90, new int[]{-95});
    rules[618] = new Rule(-90, new int[]{-90,-186,-95});
    rules[619] = new Rule(-90, new int[]{-90,-186,-232});
    rules[620] = new Rule(-90, new int[]{-256,8,-342,9});
    rules[621] = new Rule(-90, new int[]{-76,132,-332});
    rules[622] = new Rule(-90, new int[]{-76,132,-333});
    rules[623] = new Rule(-329, new int[]{-274,8,-342,9});
    rules[624] = new Rule(-331, new int[]{-274,8,-343,9});
    rules[625] = new Rule(-330, new int[]{-274,8,-343,9});
    rules[626] = new Rule(-330, new int[]{-346});
    rules[627] = new Rule(-346, new int[]{-328});
    rules[628] = new Rule(-346, new int[]{-346,94,-328});
    rules[629] = new Rule(-328, new int[]{-14});
    rules[630] = new Rule(-328, new int[]{-274});
    rules[631] = new Rule(-328, new int[]{53});
    rules[632] = new Rule(-328, new int[]{-247});
    rules[633] = new Rule(-328, new int[]{-285});
    rules[634] = new Rule(-332, new int[]{11,-344,12});
    rules[635] = new Rule(-344, new int[]{-334});
    rules[636] = new Rule(-344, new int[]{-344,94,-334});
    rules[637] = new Rule(-334, new int[]{-14});
    rules[638] = new Rule(-334, new int[]{-336});
    rules[639] = new Rule(-334, new int[]{14});
    rules[640] = new Rule(-334, new int[]{-331});
    rules[641] = new Rule(-334, new int[]{-332});
    rules[642] = new Rule(-334, new int[]{-333});
    rules[643] = new Rule(-334, new int[]{6});
    rules[644] = new Rule(-336, new int[]{50,-136});
    rules[645] = new Rule(-333, new int[]{8,-345,9});
    rules[646] = new Rule(-335, new int[]{14});
    rules[647] = new Rule(-335, new int[]{-14});
    rules[648] = new Rule(-335, new int[]{-189,-14});
    rules[649] = new Rule(-335, new int[]{50,-136});
    rules[650] = new Rule(-335, new int[]{-331});
    rules[651] = new Rule(-335, new int[]{-332});
    rules[652] = new Rule(-335, new int[]{-333});
    rules[653] = new Rule(-345, new int[]{-335});
    rules[654] = new Rule(-345, new int[]{-345,94,-335});
    rules[655] = new Rule(-343, new int[]{-341});
    rules[656] = new Rule(-343, new int[]{-343,10,-341});
    rules[657] = new Rule(-343, new int[]{-343,94,-341});
    rules[658] = new Rule(-342, new int[]{-340});
    rules[659] = new Rule(-342, new int[]{-342,10,-340});
    rules[660] = new Rule(-342, new int[]{-342,94,-340});
    rules[661] = new Rule(-340, new int[]{14});
    rules[662] = new Rule(-340, new int[]{-14});
    rules[663] = new Rule(-340, new int[]{50,-136,5,-266});
    rules[664] = new Rule(-340, new int[]{50,-136});
    rules[665] = new Rule(-340, new int[]{-329});
    rules[666] = new Rule(-340, new int[]{-332});
    rules[667] = new Rule(-340, new int[]{-333});
    rules[668] = new Rule(-341, new int[]{14});
    rules[669] = new Rule(-341, new int[]{-14});
    rules[670] = new Rule(-341, new int[]{-189,-14});
    rules[671] = new Rule(-341, new int[]{-136,5,-266});
    rules[672] = new Rule(-341, new int[]{-136});
    rules[673] = new Rule(-341, new int[]{50,-136,5,-266});
    rules[674] = new Rule(-341, new int[]{50,-136});
    rules[675] = new Rule(-341, new int[]{-331});
    rules[676] = new Rule(-341, new int[]{-332});
    rules[677] = new Rule(-341, new int[]{-333});
    rules[678] = new Rule(-104, new int[]{-95});
    rules[679] = new Rule(-104, new int[]{});
    rules[680] = new Rule(-111, new int[]{-84});
    rules[681] = new Rule(-111, new int[]{});
    rules[682] = new Rule(-109, new int[]{-95,5,-104});
    rules[683] = new Rule(-109, new int[]{5,-104});
    rules[684] = new Rule(-109, new int[]{-95,5,-104,5,-95});
    rules[685] = new Rule(-109, new int[]{5,-104,5,-95});
    rules[686] = new Rule(-110, new int[]{-84,5,-111});
    rules[687] = new Rule(-110, new int[]{5,-111});
    rules[688] = new Rule(-110, new int[]{-84,5,-111,5,-84});
    rules[689] = new Rule(-110, new int[]{5,-111,5,-84});
    rules[690] = new Rule(-186, new int[]{114});
    rules[691] = new Rule(-186, new int[]{119});
    rules[692] = new Rule(-186, new int[]{117});
    rules[693] = new Rule(-186, new int[]{115});
    rules[694] = new Rule(-186, new int[]{118});
    rules[695] = new Rule(-186, new int[]{116});
    rules[696] = new Rule(-186, new int[]{131});
    rules[697] = new Rule(-95, new int[]{-77});
    rules[698] = new Rule(-95, new int[]{-95,6,-77});
    rules[699] = new Rule(-77, new int[]{-76});
    rules[700] = new Rule(-77, new int[]{-77,-187,-76});
    rules[701] = new Rule(-77, new int[]{-77,-187,-232});
    rules[702] = new Rule(-187, new int[]{110});
    rules[703] = new Rule(-187, new int[]{109});
    rules[704] = new Rule(-187, new int[]{122});
    rules[705] = new Rule(-187, new int[]{123});
    rules[706] = new Rule(-187, new int[]{120});
    rules[707] = new Rule(-191, new int[]{130});
    rules[708] = new Rule(-191, new int[]{132});
    rules[709] = new Rule(-254, new int[]{-256});
    rules[710] = new Rule(-254, new int[]{-257});
    rules[711] = new Rule(-257, new int[]{-76,130,-274});
    rules[712] = new Rule(-256, new int[]{-76,132,-274});
    rules[713] = new Rule(-78, new int[]{-89});
    rules[714] = new Rule(-258, new int[]{-78,113,-89});
    rules[715] = new Rule(-76, new int[]{-89});
    rules[716] = new Rule(-76, new int[]{-163});
    rules[717] = new Rule(-76, new int[]{-258});
    rules[718] = new Rule(-76, new int[]{-76,-188,-89});
    rules[719] = new Rule(-76, new int[]{-76,-188,-258});
    rules[720] = new Rule(-76, new int[]{-76,-188,-232});
    rules[721] = new Rule(-76, new int[]{-254});
    rules[722] = new Rule(-188, new int[]{112});
    rules[723] = new Rule(-188, new int[]{111});
    rules[724] = new Rule(-188, new int[]{125});
    rules[725] = new Rule(-188, new int[]{126});
    rules[726] = new Rule(-188, new int[]{127});
    rules[727] = new Rule(-188, new int[]{128});
    rules[728] = new Rule(-188, new int[]{124});
    rules[729] = new Rule(-53, new int[]{60,8,-274,9});
    rules[730] = new Rule(-54, new int[]{8,-92,94,-73,-313,-320,9});
    rules[731] = new Rule(-89, new int[]{53});
    rules[732] = new Rule(-89, new int[]{-14});
    rules[733] = new Rule(-89, new int[]{-53});
    rules[734] = new Rule(-89, new int[]{11,-64,12});
    rules[735] = new Rule(-89, new int[]{129,-89});
    rules[736] = new Rule(-89, new int[]{-189,-89});
    rules[737] = new Rule(-89, new int[]{136,-89});
    rules[738] = new Rule(-89, new int[]{-102});
    rules[739] = new Rule(-89, new int[]{-54});
    rules[740] = new Rule(-14, new int[]{-154});
    rules[741] = new Rule(-14, new int[]{-15});
    rules[742] = new Rule(-105, new int[]{-101,15,-101});
    rules[743] = new Rule(-105, new int[]{-101,15,-105});
    rules[744] = new Rule(-102, new int[]{-121,-101});
    rules[745] = new Rule(-102, new int[]{-101});
    rules[746] = new Rule(-102, new int[]{-105});
    rules[747] = new Rule(-121, new int[]{135});
    rules[748] = new Rule(-121, new int[]{-121,135});
    rules[749] = new Rule(-9, new int[]{-170,-65});
    rules[750] = new Rule(-9, new int[]{-291,-65});
    rules[751] = new Rule(-310, new int[]{-136});
    rules[752] = new Rule(-310, new int[]{-310,7,-127});
    rules[753] = new Rule(-309, new int[]{-310});
    rules[754] = new Rule(-309, new int[]{-310,-289});
    rules[755] = new Rule(-16, new int[]{-101});
    rules[756] = new Rule(-16, new int[]{-14});
    rules[757] = new Rule(-101, new int[]{-136});
    rules[758] = new Rule(-101, new int[]{-181});
    rules[759] = new Rule(-101, new int[]{39,-136});
    rules[760] = new Rule(-101, new int[]{8,-82,9});
    rules[761] = new Rule(-101, new int[]{-247});
    rules[762] = new Rule(-101, new int[]{-285});
    rules[763] = new Rule(-101, new int[]{-14,7,-127});
    rules[764] = new Rule(-101, new int[]{-16,11,-66,12});
    rules[765] = new Rule(-101, new int[]{-101,17,-109,12});
    rules[766] = new Rule(-101, new int[]{-101,8,-63,9});
    rules[767] = new Rule(-101, new int[]{-101,7,-137});
    rules[768] = new Rule(-101, new int[]{-54,7,-137});
    rules[769] = new Rule(-101, new int[]{-101,136});
    rules[770] = new Rule(-101, new int[]{-101,4,-289});
    rules[771] = new Rule(-63, new int[]{-66});
    rules[772] = new Rule(-63, new int[]{});
    rules[773] = new Rule(-64, new int[]{-71});
    rules[774] = new Rule(-64, new int[]{});
    rules[775] = new Rule(-71, new int[]{-85});
    rules[776] = new Rule(-71, new int[]{-71,94,-85});
    rules[777] = new Rule(-85, new int[]{-82});
    rules[778] = new Rule(-85, new int[]{-82,6,-82});
    rules[779] = new Rule(-155, new int[]{138});
    rules[780] = new Rule(-155, new int[]{140});
    rules[781] = new Rule(-154, new int[]{-156});
    rules[782] = new Rule(-154, new int[]{139});
    rules[783] = new Rule(-156, new int[]{-155});
    rules[784] = new Rule(-156, new int[]{-156,-155});
    rules[785] = new Rule(-181, new int[]{42,-190});
    rules[786] = new Rule(-197, new int[]{10});
    rules[787] = new Rule(-197, new int[]{10,-196,10});
    rules[788] = new Rule(-198, new int[]{});
    rules[789] = new Rule(-198, new int[]{10,-196});
    rules[790] = new Rule(-196, new int[]{-199});
    rules[791] = new Rule(-196, new int[]{-196,10,-199});
    rules[792] = new Rule(-136, new int[]{137});
    rules[793] = new Rule(-136, new int[]{-140});
    rules[794] = new Rule(-136, new int[]{-141});
    rules[795] = new Rule(-127, new int[]{-136});
    rules[796] = new Rule(-127, new int[]{-283});
    rules[797] = new Rule(-127, new int[]{-284});
    rules[798] = new Rule(-137, new int[]{-136});
    rules[799] = new Rule(-137, new int[]{-283});
    rules[800] = new Rule(-137, new int[]{-181});
    rules[801] = new Rule(-199, new int[]{141});
    rules[802] = new Rule(-199, new int[]{143});
    rules[803] = new Rule(-199, new int[]{144});
    rules[804] = new Rule(-199, new int[]{145});
    rules[805] = new Rule(-199, new int[]{147});
    rules[806] = new Rule(-199, new int[]{146});
    rules[807] = new Rule(-200, new int[]{146});
    rules[808] = new Rule(-200, new int[]{145});
    rules[809] = new Rule(-200, new int[]{141});
    rules[810] = new Rule(-200, new int[]{144});
    rules[811] = new Rule(-140, new int[]{80});
    rules[812] = new Rule(-140, new int[]{81});
    rules[813] = new Rule(-141, new int[]{75});
    rules[814] = new Rule(-141, new int[]{73});
    rules[815] = new Rule(-139, new int[]{79});
    rules[816] = new Rule(-139, new int[]{78});
    rules[817] = new Rule(-139, new int[]{77});
    rules[818] = new Rule(-139, new int[]{76});
    rules[819] = new Rule(-283, new int[]{-139});
    rules[820] = new Rule(-283, new int[]{66});
    rules[821] = new Rule(-283, new int[]{61});
    rules[822] = new Rule(-283, new int[]{122});
    rules[823] = new Rule(-283, new int[]{19});
    rules[824] = new Rule(-283, new int[]{18});
    rules[825] = new Rule(-283, new int[]{60});
    rules[826] = new Rule(-283, new int[]{20});
    rules[827] = new Rule(-283, new int[]{123});
    rules[828] = new Rule(-283, new int[]{124});
    rules[829] = new Rule(-283, new int[]{125});
    rules[830] = new Rule(-283, new int[]{126});
    rules[831] = new Rule(-283, new int[]{127});
    rules[832] = new Rule(-283, new int[]{128});
    rules[833] = new Rule(-283, new int[]{129});
    rules[834] = new Rule(-283, new int[]{130});
    rules[835] = new Rule(-283, new int[]{131});
    rules[836] = new Rule(-283, new int[]{132});
    rules[837] = new Rule(-283, new int[]{21});
    rules[838] = new Rule(-283, new int[]{71});
    rules[839] = new Rule(-283, new int[]{85});
    rules[840] = new Rule(-283, new int[]{22});
    rules[841] = new Rule(-283, new int[]{23});
    rules[842] = new Rule(-283, new int[]{26});
    rules[843] = new Rule(-283, new int[]{27});
    rules[844] = new Rule(-283, new int[]{28});
    rules[845] = new Rule(-283, new int[]{69});
    rules[846] = new Rule(-283, new int[]{93});
    rules[847] = new Rule(-283, new int[]{29});
    rules[848] = new Rule(-283, new int[]{86});
    rules[849] = new Rule(-283, new int[]{30});
    rules[850] = new Rule(-283, new int[]{31});
    rules[851] = new Rule(-283, new int[]{24});
    rules[852] = new Rule(-283, new int[]{98});
    rules[853] = new Rule(-283, new int[]{95});
    rules[854] = new Rule(-283, new int[]{32});
    rules[855] = new Rule(-283, new int[]{33});
    rules[856] = new Rule(-283, new int[]{34});
    rules[857] = new Rule(-283, new int[]{37});
    rules[858] = new Rule(-283, new int[]{38});
    rules[859] = new Rule(-283, new int[]{39});
    rules[860] = new Rule(-283, new int[]{97});
    rules[861] = new Rule(-283, new int[]{40});
    rules[862] = new Rule(-283, new int[]{41});
    rules[863] = new Rule(-283, new int[]{43});
    rules[864] = new Rule(-283, new int[]{44});
    rules[865] = new Rule(-283, new int[]{45});
    rules[866] = new Rule(-283, new int[]{91});
    rules[867] = new Rule(-283, new int[]{46});
    rules[868] = new Rule(-283, new int[]{96});
    rules[869] = new Rule(-283, new int[]{47});
    rules[870] = new Rule(-283, new int[]{25});
    rules[871] = new Rule(-283, new int[]{48});
    rules[872] = new Rule(-283, new int[]{68});
    rules[873] = new Rule(-283, new int[]{92});
    rules[874] = new Rule(-283, new int[]{49});
    rules[875] = new Rule(-283, new int[]{50});
    rules[876] = new Rule(-283, new int[]{51});
    rules[877] = new Rule(-283, new int[]{52});
    rules[878] = new Rule(-283, new int[]{53});
    rules[879] = new Rule(-283, new int[]{54});
    rules[880] = new Rule(-283, new int[]{55});
    rules[881] = new Rule(-283, new int[]{56});
    rules[882] = new Rule(-283, new int[]{58});
    rules[883] = new Rule(-283, new int[]{99});
    rules[884] = new Rule(-283, new int[]{100});
    rules[885] = new Rule(-283, new int[]{103});
    rules[886] = new Rule(-283, new int[]{101});
    rules[887] = new Rule(-283, new int[]{102});
    rules[888] = new Rule(-283, new int[]{59});
    rules[889] = new Rule(-283, new int[]{72});
    rules[890] = new Rule(-283, new int[]{35});
    rules[891] = new Rule(-283, new int[]{36});
    rules[892] = new Rule(-284, new int[]{42});
    rules[893] = new Rule(-190, new int[]{109});
    rules[894] = new Rule(-190, new int[]{110});
    rules[895] = new Rule(-190, new int[]{111});
    rules[896] = new Rule(-190, new int[]{112});
    rules[897] = new Rule(-190, new int[]{114});
    rules[898] = new Rule(-190, new int[]{115});
    rules[899] = new Rule(-190, new int[]{116});
    rules[900] = new Rule(-190, new int[]{117});
    rules[901] = new Rule(-190, new int[]{118});
    rules[902] = new Rule(-190, new int[]{119});
    rules[903] = new Rule(-190, new int[]{122});
    rules[904] = new Rule(-190, new int[]{123});
    rules[905] = new Rule(-190, new int[]{124});
    rules[906] = new Rule(-190, new int[]{125});
    rules[907] = new Rule(-190, new int[]{126});
    rules[908] = new Rule(-190, new int[]{127});
    rules[909] = new Rule(-190, new int[]{128});
    rules[910] = new Rule(-190, new int[]{129});
    rules[911] = new Rule(-190, new int[]{131});
    rules[912] = new Rule(-190, new int[]{133});
    rules[913] = new Rule(-190, new int[]{134});
    rules[914] = new Rule(-190, new int[]{-184});
    rules[915] = new Rule(-190, new int[]{113});
    rules[916] = new Rule(-184, new int[]{104});
    rules[917] = new Rule(-184, new int[]{105});
    rules[918] = new Rule(-184, new int[]{106});
    rules[919] = new Rule(-184, new int[]{107});
    rules[920] = new Rule(-184, new int[]{108});
    rules[921] = new Rule(-311, new int[]{-136,121,-317});
    rules[922] = new Rule(-311, new int[]{8,9,-314,121,-317});
    rules[923] = new Rule(-311, new int[]{8,-136,5,-265,9,-314,121,-317});
    rules[924] = new Rule(-311, new int[]{8,-136,10,-315,9,-314,121,-317});
    rules[925] = new Rule(-311, new int[]{8,-136,5,-265,10,-315,9,-314,121,-317});
    rules[926] = new Rule(-311, new int[]{8,-92,94,-73,-313,-320,9,-324});
    rules[927] = new Rule(-311, new int[]{-312});
    rules[928] = new Rule(-320, new int[]{});
    rules[929] = new Rule(-320, new int[]{10,-315});
    rules[930] = new Rule(-324, new int[]{-314,121,-317});
    rules[931] = new Rule(-312, new int[]{34,-313,121,-317});
    rules[932] = new Rule(-312, new int[]{34,8,9,-313,121,-317});
    rules[933] = new Rule(-312, new int[]{34,8,-315,9,-313,121,-317});
    rules[934] = new Rule(-312, new int[]{41,121,-318});
    rules[935] = new Rule(-312, new int[]{41,8,9,121,-318});
    rules[936] = new Rule(-312, new int[]{41,8,-315,9,121,-318});
    rules[937] = new Rule(-315, new int[]{-316});
    rules[938] = new Rule(-315, new int[]{-315,10,-316});
    rules[939] = new Rule(-316, new int[]{-147,-313});
    rules[940] = new Rule(-313, new int[]{});
    rules[941] = new Rule(-313, new int[]{5,-265});
    rules[942] = new Rule(-314, new int[]{});
    rules[943] = new Rule(-314, new int[]{5,-267});
    rules[944] = new Rule(-319, new int[]{-245});
    rules[945] = new Rule(-319, new int[]{-142});
    rules[946] = new Rule(-319, new int[]{-307});
    rules[947] = new Rule(-319, new int[]{-237});
    rules[948] = new Rule(-319, new int[]{-113});
    rules[949] = new Rule(-319, new int[]{-112});
    rules[950] = new Rule(-319, new int[]{-114});
    rules[951] = new Rule(-319, new int[]{-32});
    rules[952] = new Rule(-319, new int[]{-292});
    rules[953] = new Rule(-319, new int[]{-158});
    rules[954] = new Rule(-319, new int[]{-238});
    rules[955] = new Rule(-319, new int[]{-115});
    rules[956] = new Rule(-317, new int[]{-94});
    rules[957] = new Rule(-317, new int[]{-319});
    rules[958] = new Rule(-318, new int[]{-202});
    rules[959] = new Rule(-318, new int[]{-4});
    rules[960] = new Rule(-318, new int[]{-319});
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
        	if (ValueStack[ValueStack.Depth-1].stn is char_const _cc)
        		ValueStack[ValueStack.Depth-1].stn = new string_const(_cc.cconst.ToString());
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
      case 93: // label_name -> identifier
{ 
			CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; 
		}
        break;
      case 94: // const_decl_sect -> tkConst, const_decl
{ 
			CurrentSemanticValue.stn = new consts_definitions_list(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 95: // const_decl_sect -> const_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as consts_definitions_list).Add(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 96: // res_str_decl_sect -> tkResourceString, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 97: // res_str_decl_sect -> res_str_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 98: // type_decl_sect -> tkType, type_decl
{ 
            CurrentSemanticValue.stn = new type_declarations(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 99: // type_decl_sect -> type_decl_sect, type_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as type_declarations).Add(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 100: // var_decl_with_assign_var_tuple -> var_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 101: // var_decl_with_assign_var_tuple -> tkRoundOpen, identifier, tkComma, ident_list, 
                //                                   tkRoundClose, tkAssign, expr_l1, 
                //                                   tkSemiColon
{
			(ValueStack[ValueStack.Depth-5].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-7].id);
			ValueStack[ValueStack.Depth-5].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]);
			CurrentSemanticValue.stn = new var_tuple_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
		}
        break;
      case 102: // var_decl_sect -> tkVar, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 103: // var_decl_sect -> tkEvent, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);                        
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).is_event = true;
        }
        break;
      case 104: // var_decl_sect -> var_decl_sect, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as variable_definitions).Add(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 105: // const_decl -> only_const_decl, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 106: // only_const_decl -> const_name, tkEqual, init_const_expr
{ 
			CurrentSemanticValue.stn = new simple_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 107: // only_const_decl -> const_name, tkColon, type_ref, tkEqual, typed_const
{ 
			CurrentSemanticValue.stn = new typed_const_definition(ValueStack[ValueStack.Depth-5].id, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-3].td, CurrentLocationSpan);
		}
        break;
      case 108: // init_const_expr -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 109: // init_const_expr -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 110: // const_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 111: // expr_l1_list -> expr_l1
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 112: // expr_l1_list -> expr_l1_list, tkComma, expr_l1
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 113: // const_expr -> const_simple_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 114: // const_expr -> const_simple_expr, const_relop, const_simple_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 115: // const_expr -> question_constexpr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 116: // question_constexpr -> const_expr, tkQuestion, const_expr, tkColon, const_expr
{ CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 117: // const_relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 118: // const_relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 119: // const_relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 120: // const_relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 121: // const_relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 122: // const_relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 123: // const_relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 124: // const_simple_expr -> const_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 125: // const_simple_expr -> const_simple_expr, const_addop, const_term
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 126: // const_addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 127: // const_addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 128: // const_addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 129: // const_addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 130: // as_is_constexpr -> const_term, typecast_op, simple_or_template_type_reference
{ 
			CurrentSemanticValue.ex = NewAsIsConstexpr(ValueStack[ValueStack.Depth-3].ex, (op_typecast)ValueStack[ValueStack.Depth-2].ob, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);                                
		}
        break;
      case 131: // power_constexpr -> const_factor, tkStarStar, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 132: // const_term -> const_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 133: // const_term -> as_is_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 134: // const_term -> power_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 135: // const_term -> const_term, const_mulop, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 136: // const_term -> const_term, const_mulop, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 137: // const_mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 138: // const_mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 139: // const_mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 140: // const_mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 141: // const_mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 142: // const_mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 143: // const_mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 144: // const_factor -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 145: // const_factor -> const_set
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 146: // const_factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 147: // const_factor -> tkAddressOf, const_factor
{ 
			CurrentSemanticValue.ex = new get_address(ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);  
		}
        break;
      case 148: // const_factor -> tkRoundOpen, const_expr, tkRoundClose
{ 
	 	    CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 149: // const_factor -> tkNot, const_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 150: // const_factor -> sign, const_factor
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
      case 151: // const_factor -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 152: // const_set -> tkSquareOpen, const_elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new pascal_set_constant(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 153: // sign -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 154: // sign -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 155: // const_variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 156: // const_variable -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 157: // const_variable -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 158: // const_variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 159: // const_variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 160: // const_variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 161: // const_variable -> const_variable, const_variable_2
{
			CurrentSemanticValue.ex = NewConstVariable(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 162: // const_variable -> const_variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 163: // const_variable -> const_variable, tkSquareOpen, format_const_expr, 
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
      case 164: // const_variable_2 -> tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.ex = new dot_node(null, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 165: // const_variable_2 -> tkDeref
{ 
			CurrentSemanticValue.ex = new roof_dereference();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 166: // const_variable_2 -> tkRoundOpen, optional_const_func_expr_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 167: // const_variable_2 -> tkSquareOpen, const_elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 168: // optional_const_func_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 169: // optional_const_func_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 170: // const_elem_list -> const_elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 172: // const_elem_list1 -> const_elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 173: // const_elem_list1 -> const_elem_list1, tkComma, const_elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 174: // const_elem -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 175: // const_elem -> const_expr, tkDotDot, const_expr
{ 
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 176: // unsigned_number -> tkInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 177: // unsigned_number -> tkHex
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 178: // unsigned_number -> tkFloat
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 179: // typed_const -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 180: // typed_const -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 181: // typed_const -> record_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 182: // array_const -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new array_const(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 184: // typed_const_list -> typed_const_list1
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 185: // typed_const_list1 -> typed_const_plus
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 186: // typed_const_list1 -> typed_const_list1, tkComma, typed_const_plus
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 187: // record_const -> tkRoundOpen, const_field_list, tkRoundClose
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 188: // const_field_list -> const_field_list_1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 189: // const_field_list -> const_field_list_1, tkSemiColon
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 190: // const_field_list_1 -> const_field
{ 
			CurrentSemanticValue.ex = new record_const(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 191: // const_field_list_1 -> const_field_list_1, tkSemiColon, const_field
{ 
			CurrentSemanticValue.ex = (ValueStack[ValueStack.Depth-3].ex as record_const).Add(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 192: // const_field -> const_field_name, tkColon, typed_const
{ 
			CurrentSemanticValue.stn = new record_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 193: // const_field_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 194: // type_decl -> attribute_declarations, simple_type_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = LocationStack[LocationStack.Depth-1];
        }
        break;
      case 195: // attribute_declarations -> attribute_declaration
{ 
			CurrentSemanticValue.stn = new attribute_list(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
    }
        break;
      case 196: // attribute_declarations -> attribute_declarations, attribute_declaration
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
		}
        break;
      case 197: // attribute_declarations -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 198: // attribute_declaration -> tkSquareOpen, one_or_some_attribute, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 199: // one_or_some_attribute -> one_attribute
{ 
			CurrentSemanticValue.stn = new simple_attribute_list(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 200: // one_or_some_attribute -> one_or_some_attribute, tkComma, one_attribute
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as simple_attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 201: // one_attribute -> attribute_variable
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 202: // one_attribute -> identifier, tkColon, attribute_variable
{  
			(ValueStack[ValueStack.Depth-1].stn as attribute).qualifier = ValueStack[ValueStack.Depth-3].id;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 203: // simple_type_decl -> type_decl_identifier, tkEqual, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-4].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 204: // simple_type_decl -> template_identifier_with_equal, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 205: // type_decl_identifier -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 206: // type_decl_identifier -> identifier, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 207: // template_identifier_with_equal -> identifier, tkLower, ident_list, 
                //                                   tkGreaterEqual
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-4].id.name, ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 208: // type_decl_type -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 209: // type_decl_type -> object_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 210: // simple_type_question -> simple_type, tkQuestion
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
      case 211: // simple_type_question -> template_type, tkQuestion
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
      case 212: // type_ref -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 213: // type_ref -> simple_type_question
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 214: // type_ref -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 215: // type_ref -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 216: // type_ref -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 217: // type_ref -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 218: // type_ref -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 219: // template_type -> simple_type_identifier, template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference(ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan); 
		}
        break;
      case 220: // template_type_params -> tkLower, template_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 221: // template_type_empty_params -> tkNotEqual
{
            var ntr = new named_type_reference(new ident(""), CurrentLocationSpan);
            
			CurrentSemanticValue.stn = new template_param_list(ntr, CurrentLocationSpan);
            ntr.source_context = new SourceContext(CurrentSemanticValue.stn.source_context.end_position.line_num, CurrentSemanticValue.stn.source_context.end_position.column_num, CurrentSemanticValue.stn.source_context.begin_position.line_num, CurrentSemanticValue.stn.source_context.begin_position.column_num);
		}
        break;
      case 222: // template_type_empty_params -> tkLower, template_empty_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 223: // template_param_list -> template_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 224: // template_param_list -> template_param_list, tkComma, template_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 225: // template_empty_param_list -> template_empty_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 226: // template_empty_param_list -> template_empty_param_list, tkComma, 
                //                              template_empty_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 227: // template_empty_param -> /* empty */
{ 
            CurrentSemanticValue.td = new named_type_reference(new ident(""), CurrentLocationSpan);
        }
        break;
      case 228: // template_param -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 229: // template_param -> simple_type, tkQuestion
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
      case 230: // template_param -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 231: // template_param -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 232: // template_param -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 233: // simple_type -> range_expr
{
	    	CurrentSemanticValue.td = parsertools.ConvertDotNodeOrIdentToNamedTypeReference(ValueStack[ValueStack.Depth-1].ex); 
	    }
        break;
      case 234: // simple_type -> range_expr, tkDotDot, range_expr
{ 
			CurrentSemanticValue.td = new diapason(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 235: // simple_type -> tkRoundOpen, enumeration_id_list, tkRoundClose
{ 
			CurrentSemanticValue.td = new enum_type_definition(ValueStack[ValueStack.Depth-2].stn as enumerator_list, CurrentLocationSpan);  
		}
        break;
      case 236: // range_expr -> range_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 237: // range_expr -> range_expr, const_addop, range_term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 238: // range_term -> range_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 239: // range_term -> range_term, const_mulop, range_factor
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 240: // range_factor -> simple_type_identifier
{ 
			CurrentSemanticValue.ex = parsertools.ConvertNamedTypeReferenceToDotNodeOrIdent(ValueStack[ValueStack.Depth-1].td as named_type_reference);
        }
        break;
      case 241: // range_factor -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 242: // range_factor -> sign, range_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 243: // range_factor -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 244: // range_factor -> range_factor, tkRoundOpen, const_elem_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value, ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 245: // simple_type_identifier -> identifier
{ 
			CurrentSemanticValue.td = new named_type_reference(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 246: // simple_type_identifier -> simple_type_identifier, tkPoint, 
                //                           identifier_or_keyword
{ 
			CurrentSemanticValue.td = (ValueStack[ValueStack.Depth-3].td as named_type_reference).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 247: // enumeration_id_list -> enumeration_id, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = new enumerator_list(ValueStack[ValueStack.Depth-3].stn as enumerator, CurrentLocationSpan);
			(CurrentSemanticValue.stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 248: // enumeration_id_list -> enumeration_id_list, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 249: // enumeration_id -> type_ref
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan); 
		}
        break;
      case 250: // enumeration_id -> type_ref, tkEqual, expr
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 251: // pointer_type -> tkDeref, fptype
{ 
			CurrentSemanticValue.td = new ref_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 252: // structured_type -> unpacked_structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 253: // structured_type -> tkPacked, unpacked_structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 254: // unpacked_structured_type -> array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 255: // unpacked_structured_type -> record_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 256: // unpacked_structured_type -> set_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 257: // unpacked_structured_type -> file_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 258: // unpacked_structured_type -> sequence_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 259: // sequence_type -> tkSequence, tkOf, type_ref
{
			CurrentSemanticValue.td = new sequence_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 260: // array_type -> tkArray, tkSquareOpen, simple_type_list, tkSquareClose, tkOf, 
                //               type_ref
{ 
			CurrentSemanticValue.td = new array_type(ValueStack[ValueStack.Depth-4].stn as indexers_types, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 261: // array_type -> unsized_array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 262: // unsized_array_type -> tkArray, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new array_type(null, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 263: // simple_type_list -> simple_type_or_
{ 
			CurrentSemanticValue.stn = new indexers_types(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 264: // simple_type_list -> simple_type_list, tkComma, simple_type_or_
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as indexers_types).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 265: // simple_type_or_ -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 266: // simple_type_or_ -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 267: // set_type -> tkSet, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new set_type_definition(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 268: // file_type -> tkFile, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new file_type(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 269: // file_type -> tkFile
{ 
			CurrentSemanticValue.td = new file_type();  
			CurrentSemanticValue.td.source_context = CurrentLocationSpan;
		}
        break;
      case 270: // string_type -> tkIdentifier, tkSquareOpen, const_expr, tkSquareClose
{ 
			CurrentSemanticValue.td = new string_num_definition(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-4].id, CurrentLocationSpan);
		}
        break;
      case 271: // procedural_type -> procedural_type_kind
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 272: // procedural_type_kind -> proc_type_decl
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 273: // proc_type_decl -> tkProcedure, fp_list
{ 
			CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-1].stn as formal_parameters,null,null,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 274: // proc_type_decl -> tkFunction, fp_list, tkColon, fptype
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, null, null, null, ValueStack[ValueStack.Depth-1].td as type_definition, CurrentLocationSpan);
        }
        break;
      case 275: // proc_type_decl -> simple_type_identifier, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 276: // proc_type_decl -> template_type, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 277: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 278: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-4].stn as enumerator_list,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 279: // proc_type_decl -> simple_type_identifier, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 280: // proc_type_decl -> template_type, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 281: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,null,CurrentLocationSpan);
    	}
        break;
      case 282: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-5].stn as enumerator_list,null,CurrentLocationSpan);
    	}
        break;
      case 283: // object_type -> class_attributes, class_or_interface_keyword, 
                //                optional_base_classes, optional_where_section, 
                //                optional_component_list_seq_end
{ 
            var cd = NewObjectType((class_attribute)ValueStack[ValueStack.Depth-5].ob, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as where_definition_list, ValueStack[ValueStack.Depth-1].stn as class_body_list, CurrentLocationSpan); 
			CurrentSemanticValue.td = cd;
		}
        break;
      case 284: // record_type -> tkRecord, optional_base_classes, optional_where_section, 
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
      case 285: // class_attribute -> tkSealed
{ CurrentSemanticValue.ob = class_attribute.Sealed; }
        break;
      case 286: // class_attribute -> tkPartial
{ CurrentSemanticValue.ob = class_attribute.Partial; }
        break;
      case 287: // class_attribute -> tkAbstract
{ CurrentSemanticValue.ob = class_attribute.Abstract; }
        break;
      case 288: // class_attribute -> tkAuto
{ CurrentSemanticValue.ob = class_attribute.Auto; }
        break;
      case 289: // class_attribute -> tkStatic
{ CurrentSemanticValue.ob = class_attribute.Static; }
        break;
      case 290: // class_attributes -> /* empty */
{ 
			CurrentSemanticValue.ob = class_attribute.None; 
		}
        break;
      case 291: // class_attributes -> class_attributes1
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 292: // class_attributes1 -> class_attribute
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 293: // class_attributes1 -> class_attributes1, class_attribute
{
            if (((class_attribute)ValueStack[ValueStack.Depth-2].ob & (class_attribute)ValueStack[ValueStack.Depth-1].ob) == (class_attribute)ValueStack[ValueStack.Depth-1].ob)
                parsertools.AddErrorFromResource("ATTRIBUTE_REDECLARED",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.ob  = ((class_attribute)ValueStack[ValueStack.Depth-2].ob) | ((class_attribute)ValueStack[ValueStack.Depth-1].ob);
			//$$ = $1;
		}
        break;
      case 294: // class_or_interface_keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 295: // class_or_interface_keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 296: // class_or_interface_keyword -> tkTemplate
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-1].ti);
		}
        break;
      case 297: // class_or_interface_keyword -> tkTemplate, tkClass
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "c", CurrentLocationSpan);
		}
        break;
      case 298: // class_or_interface_keyword -> tkTemplate, tkRecord
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "r", CurrentLocationSpan);
		}
        break;
      case 299: // class_or_interface_keyword -> tkTemplate, tkInterface
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "i", CurrentLocationSpan);
		}
        break;
      case 300: // optional_component_list_seq_end -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 301: // optional_component_list_seq_end -> member_list_section, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 303: // optional_base_classes -> tkRoundOpen, base_classes_names_list, tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 304: // base_classes_names_list -> base_class_name
{ 
			CurrentSemanticValue.stn = new named_type_reference_list(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 305: // base_classes_names_list -> base_classes_names_list, tkComma, base_class_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as named_type_reference_list).Add(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 306: // base_class_name -> simple_type_identifier
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 307: // base_class_name -> template_type
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 308: // template_arguments -> tkLower, ident_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 309: // optional_where_section -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 310: // optional_where_section -> where_part_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 311: // where_part_list -> where_part
{ 
			CurrentSemanticValue.stn = new where_definition_list(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 312: // where_part_list -> where_part_list, where_part
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as where_definition_list).Add(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 313: // where_part -> tkWhere, ident_list, tkColon, type_ref_and_secific_list, 
                //               tkSemiColon
{ 
			CurrentSemanticValue.stn = new where_definition(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-2].stn as where_type_specificator_list, CurrentLocationSpan); 
		}
        break;
      case 314: // type_ref_and_secific_list -> type_ref_or_secific
{ 
			CurrentSemanticValue.stn = new where_type_specificator_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 315: // type_ref_and_secific_list -> type_ref_and_secific_list, tkComma, 
                //                              type_ref_or_secific
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as where_type_specificator_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 316: // type_ref_or_secific -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 317: // type_ref_or_secific -> tkClass
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefClass, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 318: // type_ref_or_secific -> tkRecord
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 319: // type_ref_or_secific -> tkConstructor
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 320: // member_list_section -> member_list
{ 
			CurrentSemanticValue.stn = new class_body_list(ValueStack[ValueStack.Depth-1].stn as class_members, CurrentLocationSpan);
        }
        break;
      case 321: // member_list_section -> member_list_section, ot_visibility_specifier, 
                //                        member_list
{ 
		    (ValueStack[ValueStack.Depth-1].stn as class_members).access_mod = ValueStack[ValueStack.Depth-2].stn as access_modifer_node;
			(ValueStack[ValueStack.Depth-3].stn as class_body_list).Add(ValueStack[ValueStack.Depth-1].stn as class_members,CurrentLocationSpan);
			
			if ((ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks[0].Count == 0)
                (ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks.RemoveAt(0);
			
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 322: // ot_visibility_specifier -> tkInternal
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.internal_modifer, CurrentLocationSpan); }
        break;
      case 323: // ot_visibility_specifier -> tkPublic
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.public_modifer, CurrentLocationSpan); }
        break;
      case 324: // ot_visibility_specifier -> tkProtected
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.protected_modifer, CurrentLocationSpan); }
        break;
      case 325: // ot_visibility_specifier -> tkPrivate
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.private_modifer, CurrentLocationSpan); }
        break;
      case 326: // member_list -> /* empty */
{ CurrentSemanticValue.stn = new class_members(); }
        break;
      case 327: // member_list -> field_or_const_definition_list, optional_semicolon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 328: // member_list -> method_decl_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 329: // member_list -> field_or_const_definition_list, tkSemiColon, method_decl_list
{  
			(ValueStack[ValueStack.Depth-3].stn as class_members).members.AddRange((ValueStack[ValueStack.Depth-1].stn as class_members).members);
			(ValueStack[ValueStack.Depth-3].stn as class_members).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 330: // ident_list -> identifier
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 331: // ident_list -> ident_list, tkComma, identifier
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 332: // optional_semicolon -> /* empty */
{ CurrentSemanticValue.ob = null; }
        break;
      case 333: // optional_semicolon -> tkSemiColon
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 334: // field_or_const_definition_list -> field_or_const_definition
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 335: // field_or_const_definition_list -> field_or_const_definition_list, tkSemiColon, 
                //                                   field_or_const_definition
{   
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 336: // field_or_const_definition -> attribute_declarations, 
                //                              simple_field_or_const_definition
{  
		    (ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 337: // method_decl_list -> method_or_property_decl
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 338: // method_decl_list -> method_decl_list, method_or_property_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 339: // method_or_property_decl -> method_decl_withattr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 340: // method_or_property_decl -> property_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 341: // simple_field_or_const_definition -> tkConst, only_const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 342: // simple_field_or_const_definition -> field_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 343: // simple_field_or_const_definition -> class_or_static, field_definition
{ 
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).var_attr = definition_attribute.Static;
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 344: // class_or_static -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 345: // class_or_static -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 346: // field_definition -> var_decl_part
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 347: // field_definition -> tkEvent, ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, true, CurrentLocationSpan); 
        }
        break;
      case 348: // method_decl_withattr -> attribute_declarations, method_header
{  
			(ValueStack[ValueStack.Depth-1].td as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td;
        }
        break;
      case 349: // method_decl_withattr -> attribute_declarations, method_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
            if (ValueStack[ValueStack.Depth-1].stn is procedure_definition && (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
                (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
     }
        break;
      case 350: // method_decl -> inclass_proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 351: // method_decl -> inclass_constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 352: // method_header -> class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 353: // method_header -> method_procfunc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 354: // method_header -> constr_destr_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 355: // method_procfunc_header -> proc_func_header
{ 
			CurrentSemanticValue.td = NewProcfuncHeading(ValueStack[ValueStack.Depth-1].td as procedure_header);
		}
        break;
      case 356: // proc_func_header -> proc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 357: // proc_func_header -> func_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 358: // constr_destr_header -> tkConstructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 359: // constr_destr_header -> class_or_static, tkConstructor, optional_proc_name, 
                //                        fp_list, optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,true,null,null,CurrentLocationSpan);
        }
        break;
      case 360: // constr_destr_header -> tkDestructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new destructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name, false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 361: // optional_proc_name -> proc_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 362: // optional_proc_name -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 363: // qualified_identifier -> identifier
{ CurrentSemanticValue.stn = new method_name(null,null,ValueStack[ValueStack.Depth-1].id,null,CurrentLocationSpan); }
        break;
      case 364: // qualified_identifier -> visibility_specifier
{ CurrentSemanticValue.stn = new method_name(null,null,ValueStack[ValueStack.Depth-1].id,null,CurrentLocationSpan); }
        break;
      case 365: // qualified_identifier -> qualified_identifier, tkPoint, identifier
{
			CurrentSemanticValue.stn = NewQualifiedIdentifier(ValueStack[ValueStack.Depth-3].stn as method_name, ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
        }
        break;
      case 366: // qualified_identifier -> qualified_identifier, tkPoint, visibility_specifier
{
			CurrentSemanticValue.stn = NewQualifiedIdentifier(ValueStack[ValueStack.Depth-3].stn as method_name, ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
        }
        break;
      case 367: // property_definition -> attribute_declarations, simple_property_definition
{  
			CurrentSemanticValue.stn = NewPropertyDefinition(ValueStack[ValueStack.Depth-2].stn as attribute_list, ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 368: // simple_property_definition -> tkProperty, qualified_identifier, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        }
        break;
      case 369: // simple_property_definition -> tkProperty, qualified_identifier, 
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
      case 370: // simple_property_definition -> class_or_static, tkProperty, qualified_identifier, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        	(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
        }
        break;
      case 371: // simple_property_definition -> class_or_static, tkProperty, qualified_identifier, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, property_modificator, tkSemiColon, 
                //                               array_defaultproperty
{ 
			parsertools.AddErrorFromResource("STATIC_PROPERTIES_CANNOT_HAVE_ATTRBUTE_{0}",LocationStack[LocationStack.Depth-3],ValueStack[ValueStack.Depth-3].id.name);        	
        }
        break;
      case 372: // simple_property_definition -> tkAuto, tkProperty, qualified_identifier, 
                //                               property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 373: // simple_property_definition -> class_or_static, tkAuto, tkProperty, 
                //                               qualified_identifier, property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 374: // optional_property_initialization -> tkAssign, expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 375: // optional_property_initialization -> /* empty */
{ CurrentSemanticValue.ex = null; }
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
      case 506: // assignment -> variable, tkQuestionSquareOpen, format_expr, tkSquareClose, 
                //               assign_operator, expr
{
			var fe = ValueStack[ValueStack.Depth-4].ex as format_expr;
            if (!parsertools.build_tree_for_formatter)
            {
                if (fe.expr == null)
                    fe.expr = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-4]);
                if (fe.format1 == null)
                    fe.format1 = new int32_const(int.MaxValue,LocationStack[LocationStack.Depth-4]);
            }
      		var left = new slice_expr_question(ValueStack[ValueStack.Depth-6].ex as addressed_value,fe.expr,fe.format1,fe.format2,CurrentLocationSpan);
            CurrentSemanticValue.stn = new assign(left, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
		}
        break;
      case 507: // variable_list -> variable
{
		CurrentSemanticValue.ob = new addressed_value_list(ValueStack[ValueStack.Depth-1].ex as addressed_value,LocationStack[LocationStack.Depth-1]);
	}
        break;
      case 508: // variable_list -> variable_list, tkComma, variable
{
		(ValueStack[ValueStack.Depth-3].ob as addressed_value_list).Add(ValueStack[ValueStack.Depth-1].ex as addressed_value);
		(ValueStack[ValueStack.Depth-3].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
	}
        break;
      case 509: // var_ident_list -> tkVar, identifier
{
		CurrentSemanticValue.ob = new ident_list(ValueStack[ValueStack.Depth-1].id,CurrentLocationSpan);
	}
        break;
      case 510: // var_ident_list -> var_ident_list, tkComma, tkVar, identifier
{
		(ValueStack[ValueStack.Depth-4].ob as ident_list).Add(ValueStack[ValueStack.Depth-1].id);
		(ValueStack[ValueStack.Depth-4].ob as ident_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-4].ob;
	}
        break;
      case 511: // proc_call -> var_reference
{ 
			CurrentSemanticValue.stn = new procedure_call(ValueStack[ValueStack.Depth-1].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex is ident, CurrentLocationSpan); 
		}
        break;
      case 512: // goto_stmt -> tkGoto, label_name
{ 
			CurrentSemanticValue.stn = new goto_statement(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 513: // compound_stmt -> tkBegin, stmt_list, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			(CurrentSemanticValue.stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(CurrentSemanticValue.stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 514: // stmt_list -> stmt
{ 
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 515: // stmt_list -> stmt_list, tkSemiColon, stmt
{  
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as statement_list).Add(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 516: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan); 
        }
        break;
      case 517: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt, tkElse, unlabelled_stmt
{
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as statement, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 518: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, else_case, tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 519: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, tkSemiColon, else_case, 
                //               tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 520: // pattern_cases -> pattern_case
{
            CurrentSemanticValue.stn = new pattern_cases(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 521: // pattern_cases -> pattern_cases, tkSemiColon, pattern_case
{
            CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as pattern_cases).Add(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 522: // pattern_case -> pattern_optional_var, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 523: // pattern_case -> deconstruction_or_const_pattern, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 524: // pattern_case -> collection_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 525: // pattern_case -> tuple_pattern, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
		}
        break;
      case 526: // pattern_case -> tuple_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 527: // case_stmt -> tkCase, expr_l1, tkOf, case_list, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 528: // case_stmt -> tkCase, expr_l1, tkOf, case_list, tkSemiColon, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 529: // case_stmt -> tkCase, expr_l1, tkOf, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-4].ex, NewCaseItem(new empty_statement(), null), ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 530: // case_list -> case_item
{
			if (ValueStack[ValueStack.Depth-1].stn is empty_statement) 
				CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, null);
			else CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 531: // case_list -> case_list, tkSemiColon, case_item
{ 
			CurrentSemanticValue.stn = AddCaseItem(ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 532: // case_item -> case_label_list, tkColon, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new case_variant(ValueStack[ValueStack.Depth-3].stn as expression_list, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 533: // case_label_list -> case_label
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 534: // case_label_list -> case_label_list, tkComma, case_label
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 535: // case_label -> const_elem
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 536: // else_case -> /* empty */
{ CurrentSemanticValue.stn = null;}
        break;
      case 537: // else_case -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 538: // repeat_stmt -> tkRepeat, stmt_list, tkUntil, expr
{ 
		    CurrentSemanticValue.stn = new repeat_node(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-3].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-4].ti;
			(ValueStack[ValueStack.Depth-3].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-2].ti;
			ValueStack[ValueStack.Depth-3].stn.source_context = LocationStack[LocationStack.Depth-4].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 539: // while_stmt -> tkWhile, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewWhileStmt(ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);    
        }
        break;
      case 540: // optional_tk_do -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 541: // optional_tk_do -> /* empty */
{ CurrentSemanticValue.ti = null; }
        break;
      case 542: // lock_stmt -> tkLock, expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new lock_stmt(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 543: // foreach_stmt -> tkForeach, identifier, foreach_stmt_ident_dype_opt, tkIn, 
                //                 expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-6].id, ValueStack[ValueStack.Depth-5].td, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
            if (ValueStack[ValueStack.Depth-5].td == null)
                parsertools.AddWarningFromResource("USING_UNLOCAL_FOREACH_VARIABLE", ValueStack[ValueStack.Depth-6].id.source_context);
        }
        break;
      case 544: // foreach_stmt -> tkForeach, tkVar, identifier, tkColon, type_ref, tkIn, expr_l1, 
                //                 tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-5].td, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 545: // foreach_stmt -> tkForeach, tkVar, identifier, tkIn, expr_l1, tkDo, 
                //                 unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-5].id, new no_type_foreach(), ValueStack[ValueStack.Depth-3].ex, (statement)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 546: // foreach_stmt_ident_dype_opt -> tkColon, type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 548: // for_stmt -> tkFor, optional_var, identifier, for_stmt_decl_or_assign, expr_l1, 
                //             for_cycle_type, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-8].ob, ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-5].ex, (for_cycle_type)ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 549: // optional_var -> tkVar
{ CurrentSemanticValue.ob = true; }
        break;
      case 550: // optional_var -> /* empty */
{ CurrentSemanticValue.ob = false; }
        break;
      case 552: // for_stmt_decl_or_assign -> tkColon, simple_type_identifier, tkAssign
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-2].td; }
        break;
      case 553: // for_cycle_type -> tkTo
{ CurrentSemanticValue.ob = for_cycle_type.to; }
        break;
      case 554: // for_cycle_type -> tkDownto
{ CurrentSemanticValue.ob = for_cycle_type.downto; }
        break;
      case 555: // with_stmt -> tkWith, expr_list, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new with_statement(ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 556: // inherited_message -> tkInherited
{ 
			CurrentSemanticValue.stn = new inherited_message();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 557: // try_stmt -> tkTry, stmt_list, try_handler
{ 
			CurrentSemanticValue.stn = new try_stmt(ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].stn as try_handler, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			ValueStack[ValueStack.Depth-2].stn.source_context = LocationStack[LocationStack.Depth-3].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 558: // try_handler -> tkFinally, stmt_list, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_finally(ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan);
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(ValueStack[ValueStack.Depth-2].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
		}
        break;
      case 559: // try_handler -> tkExcept, exception_block, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_except((exception_block)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan);  
			if ((ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list != null)
			{
				(ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list.source_context = CurrentLocationSpan;
				(ValueStack[ValueStack.Depth-2].stn as exception_block).source_context = CurrentLocationSpan;
			}
		}
        break;
      case 560: // exception_block -> exception_handler_list, exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-2].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 561: // exception_block -> exception_handler_list, tkSemiColon, 
                //                    exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-3].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 562: // exception_block -> stmt_list
{ 
			CurrentSemanticValue.stn = new exception_block(ValueStack[ValueStack.Depth-1].stn as statement_list, null, null, LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 563: // exception_handler_list -> exception_handler
{ 
			CurrentSemanticValue.stn = new exception_handler_list(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 564: // exception_handler_list -> exception_handler_list, tkSemiColon, 
                //                           exception_handler
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as exception_handler_list).Add(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 565: // exception_block_else_branch -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 566: // exception_block_else_branch -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 567: // exception_handler -> tkOn, exception_identifier, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new exception_handler((ValueStack[ValueStack.Depth-3].stn as exception_ident).variable, (ValueStack[ValueStack.Depth-3].stn as exception_ident).type_name, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 568: // exception_identifier -> exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(null, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 569: // exception_identifier -> exception_variable, tkColon, 
                //                         exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(ValueStack[ValueStack.Depth-3].id, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 570: // exception_class_type_identifier -> simple_type_identifier
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 571: // exception_variable -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 572: // raise_stmt -> tkRaise
{ 
			CurrentSemanticValue.stn = new raise_stmt(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 573: // raise_stmt -> tkRaise, expr
{ 
			CurrentSemanticValue.stn = new raise_stmt(ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);  
		}
        break;
      case 574: // expr_list -> expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 575: // expr_list -> expr_list, tkComma, expr_with_func_decl_lambda
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 576: // expr_as_stmt -> allowable_expr_as_stmt
{ 
			CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 577: // allowable_expr_as_stmt -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 578: // expr_with_func_decl_lambda -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 579: // expr_with_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 580: // expr -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 581: // expr -> format_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 582: // expr_l1 -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 583: // expr_l1 -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 584: // expr_l1 -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 585: // expr_l1_for_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 586: // expr_l1_for_question_expr -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 587: // expr_l1_for_new_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 588: // expr_l1_for_new_question_expr -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 589: // expr_l1_func_decl_lambda -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 590: // expr_l1_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 591: // expr_l1_for_lambda -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 592: // expr_l1_for_lambda -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 593: // expr_l1_for_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 594: // expr_dq -> relop_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 595: // expr_dq -> expr_dq, tkDoubleQuestion, relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 596: // sizeof_expr -> tkSizeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new sizeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, null, CurrentLocationSpan);  
		}
        break;
      case 597: // typeof_expr -> tkTypeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 598: // typeof_expr -> tkTypeOf, tkRoundOpen, empty_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 599: // question_expr -> expr_l1_for_question_expr, tkQuestion, 
                //                  expr_l1_for_question_expr, tkColon, 
                //                  expr_l1_for_question_expr
{ 
            if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
            	parsertools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 600: // new_question_expr -> tkIf, expr_l1_for_new_question_expr, tkThen, 
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
      case 601: // empty_template_type_reference -> simple_type_identifier, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 602: // empty_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
        }
        break;
      case 603: // simple_or_template_type_reference -> simple_type_identifier
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 604: // simple_or_template_type_reference -> simple_type_identifier, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 605: // simple_or_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 606: // optional_array_initializer -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = new array_const((expression_list)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan); 
		}
        break;
      case 608: // new_expr -> tkNew, simple_or_template_type_reference, 
                //             optional_expr_list_with_bracket
{
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-2].td, ValueStack[ValueStack.Depth-1].stn as expression_list, false, null, CurrentLocationSpan);
        }
        break;
      case 609: // new_expr -> tkNew, simple_or_template_type_reference, tkSquareOpen, 
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
      case 610: // new_expr -> tkNew, tkClass, tkRoundOpen, list_fields_in_unnamed_object, 
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
      case 611: // field_in_unnamed_object -> identifier, tkAssign, relop_expr
{
		    if (ValueStack[ValueStack.Depth-1].ex is nil_const)
				parsertools.AddErrorFromResource("NIL_IN_UNNAMED_OBJECT",CurrentLocationSpan);		    
			CurrentSemanticValue.ob = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 612: // field_in_unnamed_object -> relop_expr
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
      case 613: // list_fields_in_unnamed_object -> field_in_unnamed_object
{
			var l = new name_assign_expr_list();
			CurrentSemanticValue.ob = l.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
		}
        break;
      case 614: // list_fields_in_unnamed_object -> list_fields_in_unnamed_object, tkComma, 
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
      case 615: // optional_expr_list_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 616: // optional_expr_list_with_bracket -> tkRoundOpen, optional_expr_list, 
                //                                    tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 617: // relop_expr -> simple_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 618: // relop_expr -> relop_expr, relop, simple_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 619: // relop_expr -> relop_expr, relop, new_question_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 620: // relop_expr -> is_type_expr, tkRoundOpen, pattern_out_param_list, tkRoundClose
{
            var isTypeCheck = ValueStack[ValueStack.Depth-4].ex as typecast_node;
            var deconstructorPattern = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, isTypeCheck.type_def, null, CurrentLocationSpan); 
            CurrentSemanticValue.ex = new is_pattern_expr(isTypeCheck.expr, deconstructorPattern, CurrentLocationSpan);
        }
        break;
      case 621: // relop_expr -> term, tkIs, collection_pattern
{
            CurrentSemanticValue.ex = new is_pattern_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 622: // relop_expr -> term, tkIs, tuple_pattern
{
            CurrentSemanticValue.ex = new is_pattern_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 623: // pattern -> simple_or_template_type_reference, tkRoundOpen, 
                //            pattern_out_param_list, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 624: // pattern_optional_var -> simple_or_template_type_reference, tkRoundOpen, 
                //                         pattern_out_param_list_optional_var, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 625: // deconstruction_or_const_pattern -> simple_or_template_type_reference, 
                //                                    tkRoundOpen, 
                //                                    pattern_out_param_list_optional_var, 
                //                                    tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 626: // deconstruction_or_const_pattern -> const_pattern_expr_list
{
		    CurrentSemanticValue.stn = new const_pattern(ValueStack[ValueStack.Depth-1].ob as List<syntax_tree_node>, CurrentLocationSpan); 
		}
        break;
      case 627: // const_pattern_expr_list -> const_pattern_expression
{ 
			CurrentSemanticValue.ob = new List<syntax_tree_node>(); 
			(CurrentSemanticValue.ob as List<syntax_tree_node>).Add(ValueStack[ValueStack.Depth-1].stn);
		}
        break;
      case 628: // const_pattern_expr_list -> const_pattern_expr_list, tkComma, 
                //                            const_pattern_expression
{ 
			var list = ValueStack[ValueStack.Depth-3].ob as List<syntax_tree_node>;
            list.Add(ValueStack[ValueStack.Depth-1].stn);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 629: // const_pattern_expression -> literal_or_number
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 630: // const_pattern_expression -> simple_or_template_type_reference
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 631: // const_pattern_expression -> tkNil
{ 
			CurrentSemanticValue.stn = new nil_const();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 632: // const_pattern_expression -> sizeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 633: // const_pattern_expression -> typeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 634: // collection_pattern -> tkSquareOpen, collection_pattern_expr_list, tkSquareClose
{
			CurrentSemanticValue.stn = new collection_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 635: // collection_pattern_expr_list -> collection_pattern_list_item
{
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 636: // collection_pattern_expr_list -> collection_pattern_expr_list, tkComma, 
                //                                 collection_pattern_list_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 637: // collection_pattern_list_item -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 638: // collection_pattern_list_item -> collection_pattern_var_item
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 639: // collection_pattern_list_item -> tkUnderscore
{
			CurrentSemanticValue.stn = new collection_pattern_wild_card(CurrentLocationSpan);
		}
        break;
      case 640: // collection_pattern_list_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 641: // collection_pattern_list_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 642: // collection_pattern_list_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 643: // collection_pattern_list_item -> tkDotDot
{
			CurrentSemanticValue.stn = new collection_pattern_gap_parameter(CurrentLocationSpan);
		}
        break;
      case 644: // collection_pattern_var_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new collection_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 645: // tuple_pattern -> tkRoundOpen, tuple_pattern_item_list, tkRoundClose
{
			if ((ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>).Count>6) 
				parsertools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
			CurrentSemanticValue.stn = new tuple_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 646: // tuple_pattern_item -> tkUnderscore
{ 
			CurrentSemanticValue.stn = new tuple_pattern_wild_card(CurrentLocationSpan); 
		}
        break;
      case 647: // tuple_pattern_item -> literal_or_number
{ 
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 648: // tuple_pattern_item -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 649: // tuple_pattern_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new tuple_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 650: // tuple_pattern_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 651: // tuple_pattern_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 652: // tuple_pattern_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 653: // tuple_pattern_item_list -> tuple_pattern_item
{ 
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 654: // tuple_pattern_item_list -> tuple_pattern_item_list, tkComma, tuple_pattern_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 655: // pattern_out_param_list_optional_var -> pattern_out_param_optional_var
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 656: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkSemiColon, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 657: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkComma, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 658: // pattern_out_param_list -> pattern_out_param
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 659: // pattern_out_param_list -> pattern_out_param_list, tkSemiColon, 
                //                           pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 660: // pattern_out_param_list -> pattern_out_param_list, tkComma, pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 661: // pattern_out_param -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 662: // pattern_out_param -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 663: // pattern_out_param -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 664: // pattern_out_param -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 665: // pattern_out_param -> pattern
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 666: // pattern_out_param -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 667: // pattern_out_param -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 668: // pattern_out_param_optional_var -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 669: // pattern_out_param_optional_var -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 670: // pattern_out_param_optional_var -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 671: // pattern_out_param_optional_var -> identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, false, CurrentLocationSpan);
        }
        break;
      case 672: // pattern_out_param_optional_var -> identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, false, CurrentLocationSpan);
        }
        break;
      case 673: // pattern_out_param_optional_var -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 674: // pattern_out_param_optional_var -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 675: // pattern_out_param_optional_var -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 676: // pattern_out_param_optional_var -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 677: // pattern_out_param_optional_var -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 678: // simple_expr_or_nothing -> simple_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 679: // simple_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 680: // const_expr_or_nothing -> const_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 681: // const_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 682: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing
{
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 683: // format_expr -> tkColon, simple_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 684: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing, tkColon, 
                //                simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 685: // format_expr -> tkColon, simple_expr_or_nothing, tkColon, simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 686: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 687: // format_const_expr -> tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 688: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing, tkColon, 
                //                      const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 689: // format_const_expr -> tkColon, const_expr_or_nothing, tkColon, const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 690: // relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 691: // relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 692: // relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 693: // relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 694: // relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 695: // relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 696: // relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 697: // simple_expr -> term1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 698: // simple_expr -> simple_expr, tkDotDot, term1
{ 
		if (parsertools.build_tree_for_formatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
	}
        break;
      case 699: // term1 -> term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 700: // term1 -> term1, addop, term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 701: // term1 -> term1, addop, new_question_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 702: // addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 703: // addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 704: // addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 705: // addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 706: // addop -> tkCSharpStyleOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 707: // typecast_op -> tkAs
{ 
			CurrentSemanticValue.ob = op_typecast.as_op; 
		}
        break;
      case 708: // typecast_op -> tkIs
{ 
			CurrentSemanticValue.ob = op_typecast.is_op; 
		}
        break;
      case 709: // as_is_expr -> is_type_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 710: // as_is_expr -> as_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 711: // as_expr -> term, tkAs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 712: // is_type_expr -> term, tkIs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 713: // simple_term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 714: // power_expr -> simple_term, tkStarStar, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 715: // term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 716: // term -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 717: // term -> power_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 718: // term -> term, mulop, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 719: // term -> term, mulop, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 720: // term -> term, mulop, new_question_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 721: // term -> as_is_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 722: // mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 723: // mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 724: // mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 725: // mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 726: // mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 727: // mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 728: // mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 729: // default_expr -> tkDefault, tkRoundOpen, simple_or_template_type_reference, 
                //                 tkRoundClose
{ 
			CurrentSemanticValue.ex = new default_operator(ValueStack[ValueStack.Depth-2].td as named_type_reference, CurrentLocationSpan);  
		}
        break;
      case 730: // tuple -> tkRoundOpen, expr_l1, tkComma, expr_l1_list, lambda_type_ref, 
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
      case 731: // factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 732: // factor -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 733: // factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 734: // factor -> tkSquareOpen, elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new pascal_set_constant(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 735: // factor -> tkNot, factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 736: // factor -> sign, factor
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
      case 737: // factor -> tkDeref, factor
{
            CurrentSemanticValue.ex = new index(ValueStack[ValueStack.Depth-1].ex, true, CurrentLocationSpan);
        }
        break;
      case 738: // factor -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 739: // factor -> tuple
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 740: // literal_or_number -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 741: // literal_or_number -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 742: // var_question_point -> variable, tkQuestionPoint, variable
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 743: // var_question_point -> variable, tkQuestionPoint, var_question_point
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 744: // var_reference -> var_address, variable
{
			CurrentSemanticValue.ex = NewVarReference(ValueStack[ValueStack.Depth-2].stn as get_address, ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);
		}
        break;
      case 745: // var_reference -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 746: // var_reference -> var_question_point
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 747: // var_address -> tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(CurrentLocationSpan);
		}
        break;
      case 748: // var_address -> var_address, tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(ValueStack[ValueStack.Depth-2].stn as get_address, CurrentLocationSpan);
		}
        break;
      case 749: // attribute_variable -> simple_type_identifier, optional_expr_list_with_bracket
{ 
			CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
		}
        break;
      case 750: // attribute_variable -> template_type, optional_expr_list_with_bracket
{
            CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 751: // dotted_identifier -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 752: // dotted_identifier -> dotted_identifier, tkPoint, identifier_or_keyword
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
		}
        break;
      case 753: // variable_as_type -> dotted_identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;}
        break;
      case 754: // variable_as_type -> dotted_identifier, template_type_params
{ CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-2].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);   }
        break;
      case 755: // variable_or_literal_or_number -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 756: // variable_or_literal_or_number -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 757: // variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 758: // variable -> operator_name_ident
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 759: // variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 760: // variable -> tkRoundOpen, expr, tkRoundClose
{
		    if (!parsertools.build_tree_for_formatter) 
            {
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
            } 
			else CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
        }
        break;
      case 761: // variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 762: // variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 763: // variable -> literal_or_number, tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 764: // variable -> variable_or_literal_or_number, tkSquareOpen, expr_list, 
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
			else CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-4].ex as addressed_value, el, CurrentLocationSpan);
        }
        break;
      case 765: // variable -> variable, tkQuestionSquareOpen, format_expr, tkSquareClose
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
      case 766: // variable -> variable, tkRoundOpen, optional_expr_list, tkRoundClose
{
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 767: // variable -> variable, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 768: // variable -> tuple, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 769: // variable -> variable, tkDeref
{
			CurrentSemanticValue.ex = new roof_dereference(ValueStack[ValueStack.Depth-2].ex as addressed_value,CurrentLocationSpan);
        }
        break;
      case 770: // variable -> variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 771: // optional_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 772: // optional_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 773: // elem_list -> elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 774: // elem_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 775: // elem_list1 -> elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 776: // elem_list1 -> elem_list1, tkComma, elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 777: // elem -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 778: // elem -> expr, tkDotDot, expr
{ CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 779: // one_literal -> tkStringLiteral
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 780: // one_literal -> tkAsciiChar
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 781: // literal -> literal_list
{ 
			CurrentSemanticValue.ex = NewLiteral(ValueStack[ValueStack.Depth-1].stn as literal_const_line);
        }
        break;
      case 782: // literal -> tkFormatStringLiteral
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
      case 783: // literal_list -> one_literal
{ 
			CurrentSemanticValue.stn = new literal_const_line(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 784: // literal_list -> literal_list, one_literal
{ 
        	var line = ValueStack[ValueStack.Depth-2].stn as literal_const_line;
            if (line.literals.Last() is string_const && ValueStack[ValueStack.Depth-1].ex is string_const)
            	parsertools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.stn = line.Add(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 785: // operator_name_ident -> tkOperator, overload_operator
{ 
			CurrentSemanticValue.ex = new operator_name_ident((ValueStack[ValueStack.Depth-1].op as op_type_node).text, (ValueStack[ValueStack.Depth-1].op as op_type_node).type, CurrentLocationSpan);
		}
        break;
      case 786: // optional_method_modificators -> tkSemiColon
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 787: // optional_method_modificators -> tkSemiColon, meth_modificators, tkSemiColon
{ 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 788: // optional_method_modificators1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 789: // optional_method_modificators1 -> tkSemiColon, meth_modificators
{ 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 790: // meth_modificators -> meth_modificator
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan); 
		}
        break;
      case 791: // meth_modificators -> meth_modificators, tkSemiColon, meth_modificator
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as procedure_attributes_list).Add(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan);  
		}
        break;
      case 792: // identifier -> tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 793: // identifier -> property_specifier_directives
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 794: // identifier -> non_reserved
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 795: // identifier_or_keyword -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 796: // identifier_or_keyword -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 797: // identifier_or_keyword -> reserved_keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 798: // identifier_keyword_operatorname -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 799: // identifier_keyword_operatorname -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 800: // identifier_keyword_operatorname -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 801: // meth_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 802: // meth_modificator -> tkOverload
{ 
            CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id;
            parsertools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", ValueStack[ValueStack.Depth-1].id.source_context);
        }
        break;
      case 803: // meth_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 804: // meth_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 805: // meth_modificator -> tkExtensionMethod
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 806: // meth_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 807: // property_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 808: // property_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 809: // property_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 810: // property_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 811: // property_specifier_directives -> tkRead
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 812: // property_specifier_directives -> tkWrite
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 813: // non_reserved -> tkName
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 814: // non_reserved -> tkNew
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 815: // visibility_specifier -> tkInternal
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 816: // visibility_specifier -> tkPublic
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 817: // visibility_specifier -> tkProtected
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 818: // visibility_specifier -> tkPrivate
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 819: // keyword -> visibility_specifier
{ 
			CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  
		}
        break;
      case 820: // keyword -> tkSealed
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 821: // keyword -> tkTemplate
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 822: // keyword -> tkOr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 823: // keyword -> tkTypeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 824: // keyword -> tkSizeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 825: // keyword -> tkDefault
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 826: // keyword -> tkWhere
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 827: // keyword -> tkXor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 828: // keyword -> tkAnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 829: // keyword -> tkDiv
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 830: // keyword -> tkMod
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 831: // keyword -> tkShl
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 832: // keyword -> tkShr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 833: // keyword -> tkNot
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 834: // keyword -> tkAs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 835: // keyword -> tkIn
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 836: // keyword -> tkIs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 837: // keyword -> tkArray
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 838: // keyword -> tkSequence
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 839: // keyword -> tkBegin
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 840: // keyword -> tkCase
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 841: // keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 842: // keyword -> tkConst
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 843: // keyword -> tkConstructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 844: // keyword -> tkDestructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 845: // keyword -> tkDownto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 846: // keyword -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 847: // keyword -> tkElse
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 848: // keyword -> tkEnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 849: // keyword -> tkExcept
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 850: // keyword -> tkFile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 851: // keyword -> tkAuto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 852: // keyword -> tkFinalization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 853: // keyword -> tkFinally
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 854: // keyword -> tkFor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 855: // keyword -> tkForeach
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 856: // keyword -> tkFunction
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 857: // keyword -> tkIf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 858: // keyword -> tkImplementation
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 859: // keyword -> tkInherited
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 860: // keyword -> tkInitialization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 861: // keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 862: // keyword -> tkProcedure
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 863: // keyword -> tkProperty
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 864: // keyword -> tkRaise
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 865: // keyword -> tkRecord
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 866: // keyword -> tkRepeat
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 867: // keyword -> tkSet
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 868: // keyword -> tkTry
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 869: // keyword -> tkType
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 870: // keyword -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 871: // keyword -> tkThen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 872: // keyword -> tkTo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 873: // keyword -> tkUntil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 874: // keyword -> tkUses
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 875: // keyword -> tkVar
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 876: // keyword -> tkWhile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 877: // keyword -> tkWith
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 878: // keyword -> tkNil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 879: // keyword -> tkGoto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 880: // keyword -> tkOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 881: // keyword -> tkLabel
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 882: // keyword -> tkProgram
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 883: // keyword -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 884: // keyword -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 885: // keyword -> tkNamespace
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 886: // keyword -> tkExternal
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 887: // keyword -> tkParams
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 888: // keyword -> tkEvent
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 889: // keyword -> tkYield
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 890: // keyword -> tkMatch
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 891: // keyword -> tkWhen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 892: // reserved_keyword -> tkOperator
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 893: // overload_operator -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 894: // overload_operator -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 895: // overload_operator -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 896: // overload_operator -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 897: // overload_operator -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 898: // overload_operator -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 899: // overload_operator -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 900: // overload_operator -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 901: // overload_operator -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 902: // overload_operator -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 903: // overload_operator -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 904: // overload_operator -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 905: // overload_operator -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 906: // overload_operator -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 907: // overload_operator -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 908: // overload_operator -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 909: // overload_operator -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 910: // overload_operator -> tkNot
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 911: // overload_operator -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 912: // overload_operator -> tkImplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 913: // overload_operator -> tkExplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 914: // overload_operator -> assign_operator
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 915: // overload_operator -> tkStarStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 916: // assign_operator -> tkAssign
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 917: // assign_operator -> tkPlusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 918: // assign_operator -> tkMinusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 919: // assign_operator -> tkMultEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 920: // assign_operator -> tkDivEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 921: // func_decl_lambda -> identifier, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 922: // func_decl_lambda -> tkRoundOpen, tkRoundClose, lambda_type_ref_noproctype, 
                //                     tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 923: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkRoundClose, 
                //                     lambda_type_ref_noproctype, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-7].id, LocationStack[LocationStack.Depth-7]); 
            var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]);
			var formalPars = new formal_parameters(new typed_parameters(idList, ValueStack[ValueStack.Depth-5].td, parametr_kind.none, null, loc), loc);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 924: // func_decl_lambda -> tkRoundOpen, identifier, tkSemiColon, full_lambda_fp_list, 
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
      case 925: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkSemiColon, 
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
      case 926: // func_decl_lambda -> tkRoundOpen, expr_l1, tkComma, expr_l1_list, 
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
      case 927: // func_decl_lambda -> expl_func_decl_lambda
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
		}
        break;
      case 928: // optional_full_lambda_fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 929: // optional_full_lambda_fp_list -> tkSemiColon, full_lambda_fp_list
{
		CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
	}
        break;
      case 930: // rem_lambda -> lambda_type_ref_noproctype, tkArrow, lambda_function_body
{ 
		    CurrentSemanticValue.ob = new pair_type_stlist(ValueStack[ValueStack.Depth-3].td,ValueStack[ValueStack.Depth-1].stn as statement_list);
		}
        break;
      case 931: // expl_func_decl_lambda -> tkFunction, lambda_type_ref, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 932: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, tkRoundClose, lambda_type_ref, 
                //                          tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 933: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, full_lambda_fp_list, 
                //                          tkRoundClose, lambda_type_ref, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 934: // expl_func_decl_lambda -> tkProcedure, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 935: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, tkRoundClose, tkArrow, 
                //                          lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 936: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, full_lambda_fp_list, 
                //                          tkRoundClose, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-4].stn as formal_parameters, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 937: // full_lambda_fp_list -> lambda_simple_fp_sect
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
      case 938: // full_lambda_fp_list -> full_lambda_fp_list, tkSemiColon, lambda_simple_fp_sect
{
			CurrentSemanticValue.stn =(ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
		}
        break;
      case 939: // lambda_simple_fp_sect -> ident_list, lambda_type_ref
{
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-2].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan);
		}
        break;
      case 940: // lambda_type_ref -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
		}
        break;
      case 941: // lambda_type_ref -> tkColon, fptype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 942: // lambda_type_ref_noproctype -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
		}
        break;
      case 943: // lambda_type_ref_noproctype -> tkColon, fptype_noproctype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 944: // common_lambda_body -> compound_stmt
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 945: // common_lambda_body -> if_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 946: // common_lambda_body -> while_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 947: // common_lambda_body -> repeat_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 948: // common_lambda_body -> for_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 949: // common_lambda_body -> foreach_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 950: // common_lambda_body -> loop_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 951: // common_lambda_body -> case_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 952: // common_lambda_body -> try_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 953: // common_lambda_body -> lock_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 954: // common_lambda_body -> raise_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 955: // common_lambda_body -> yield_stmt
{
			parsertools.AddErrorFromResource("YIELD_STATEMENT_CANNOT_BE_USED_IN_LAMBDA_BODY", CurrentLocationSpan);
		}
        break;
      case 956: // lambda_function_body -> expr_l1_for_lambda
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
      case 957: // lambda_function_body -> common_lambda_body
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 958: // lambda_procedure_body -> proc_call
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 959: // lambda_procedure_body -> assignment
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 960: // lambda_procedure_body -> common_lambda_body
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
