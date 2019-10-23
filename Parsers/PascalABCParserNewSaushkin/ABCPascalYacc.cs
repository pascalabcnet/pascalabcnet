// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  DESKTOP-7B4K9VB
// DateTime: 24.10.2019 2:05:35
// UserName: Bogdan
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
    tkIf=37,tkImplementation=38,tkInherited=39,tkInterface=40,tkTypeclass=41,tkInstance=42,
    tkProcedure=43,tkOperator=44,tkProperty=45,tkRaise=46,tkRecord=47,tkSet=48,
    tkType=49,tkThen=50,tkUses=51,tkVar=52,tkWhile=53,tkWith=54,
    tkNil=55,tkGoto=56,tkOf=57,tkLabel=58,tkLock=59,tkProgram=60,
    tkEvent=61,tkDefault=62,tkTemplate=63,tkPacked=64,tkExports=65,tkResourceString=66,
    tkThreadvar=67,tkSealed=68,tkPartial=69,tkTo=70,tkDownto=71,tkLoop=72,
    tkSequence=73,tkYield=74,tkNew=75,tkOn=76,tkName=77,tkPrivate=78,
    tkProtected=79,tkPublic=80,tkInternal=81,tkRead=82,tkWrite=83,tkParseModeExpression=84,
    tkParseModeStatement=85,tkParseModeType=86,tkBegin=87,tkEnd=88,tkAsmBody=89,tkILCode=90,
    tkError=91,INVISIBLE=92,tkRepeat=93,tkUntil=94,tkDo=95,tkComma=96,
    tkFinally=97,tkTry=98,tkInitialization=99,tkFinalization=100,tkUnit=101,tkLibrary=102,
    tkExternal=103,tkParams=104,tkNamespace=105,tkAssign=106,tkPlusEqual=107,tkMinusEqual=108,
    tkMultEqual=109,tkDivEqual=110,tkMinus=111,tkPlus=112,tkSlash=113,tkStar=114,
    tkStarStar=115,tkEqual=116,tkGreater=117,tkGreaterEqual=118,tkLower=119,tkLowerEqual=120,
    tkNotEqual=121,tkCSharpStyleOr=122,tkArrow=123,tkOr=124,tkXor=125,tkAnd=126,
    tkDiv=127,tkMod=128,tkShl=129,tkShr=130,tkNot=131,tkAs=132,
    tkIn=133,tkIs=134,tkImplicit=135,tkExplicit=136,tkAddressOf=137,tkDeref=138,
    tkIdentifier=139,tkStringLiteral=140,tkFormatStringLiteral=141,tkAsciiChar=142,tkAbstract=143,tkForward=144,
    tkOverload=145,tkReintroduce=146,tkOverride=147,tkVirtual=148,tkExtensionMethod=149,tkInteger=150,
    tkFloat=151,tkHex=152,tkUnknown=153};

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
  private static Rule[] rules = new Rule[950];
  private static State[] states = new State[1573];
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
      "non_reserved", "typeclass_restriction", "if_stmt", "initialization_part", 
      "template_arguments", "label_list", "ident_or_keyword_pointseparator_list", 
      "ident_list", "param_name_list", "inherited_message", "implementation_part", 
      "interface_part", "abc_interface_part", "simple_type_list", "literal", 
      "one_literal", "literal_list", "label_decl_sect", "lock_stmt", "func_name", 
      "proc_name", "optional_proc_name", "qualified_identifier", "new_expr", 
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
      "record_const", "const_field_list_1", "const_field_list", "const_field", 
      "repeat_stmt", "raise_stmt", "pointer_type", "attribute_declaration", "one_or_some_attribute", 
      "stmt_list", "else_case", "exception_block_else_branch", "compound_stmt", 
      "string_type", "sizeof_expr", "simple_property_definition", "stmt_or_expression", 
      "unlabelled_stmt", "stmt", "case_item", "set_type", "as_is_expr", "as_is_constexpr", 
      "is_type_expr", "as_expr", "power_expr", "power_constexpr", "unsized_array_type", 
      "simple_type_or_", "simple_type", "simple_type_question", "foreach_stmt_ident_dype_opt", 
      "fptype", "type_ref", "fptype_noproctype", "array_type", "template_param", 
      "template_empty_param", "structured_type", "unpacked_structured_type", 
      "empty_template_type_reference", "simple_or_template_type_reference", "type_ref_or_secific", 
      "for_stmt_decl_or_assign", "type_decl_type", "type_ref_and_secific_list", 
      "type_decl_sect", "try_handler", "class_or_interface_keyword", "optional_tk_do", 
      "keyword", "reserved_keyword", "typeof_expr", "simple_fp_sect", "template_param_list", 
      "template_empty_param_list", "template_type_params", "template_type_empty_params", 
      "template_type_or_typeclass_params", "typeclass_params", "template_type", 
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
    states[0] = new State(new int[]{60,1480,11,627,84,1555,86,1560,85,1567,3,-25,51,-25,87,-25,58,-25,26,-25,66,-25,49,-25,52,-25,61,-25,43,-25,34,-25,25,-25,23,-25,27,-25,28,-25,101,-197,102,-197,105,-197},new int[]{-1,1,-221,3,-222,4,-293,1492,-6,1493,-236,996,-162,1554});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,1476,51,-12,87,-12,58,-12,26,-12,66,-12,49,-12,52,-12,61,-12,11,-12,43,-12,34,-12,25,-12,23,-12,27,-12,28,-12},new int[]{-172,5,-173,1474,-171,1479});
    states[5] = new State(-36,new int[]{-291,6});
    states[6] = new State(new int[]{51,14,58,-60,26,-60,66,-60,49,-60,52,-60,61,-60,11,-60,43,-60,34,-60,25,-60,23,-60,27,-60,28,-60,87,-60},new int[]{-17,7,-34,115,-38,1411,-39,1412});
    states[7] = new State(new int[]{7,9,10,10,5,11,96,12,6,13,2,-24},new int[]{-175,8});
    states[8] = new State(-18);
    states[9] = new State(-19);
    states[10] = new State(-20);
    states[11] = new State(-21);
    states[12] = new State(-22);
    states[13] = new State(-23);
    states[14] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35,68,36,63,37,124,38,19,39,18,40,62,41,20,42,125,43,126,44,127,45,128,46,129,47,130,48,131,49,132,50,133,51,134,52,21,53,73,54,87,55,22,56,23,57,26,58,27,59,28,60,71,61,95,62,29,63,88,64,30,65,31,66,24,67,100,68,97,69,32,70,33,71,34,72,37,73,38,74,39,75,99,76,40,77,43,78,45,79,46,80,47,81,93,82,48,83,98,84,49,85,25,86,50,87,70,88,94,89,51,90,52,91,53,92,54,93,55,94,56,95,57,96,58,97,60,98,101,99,102,100,105,101,103,102,104,103,61,104,74,105,35,106,36,107,42,108,44,110},new int[]{-292,15,-294,114,-143,19,-123,113,-132,22,-136,24,-137,27,-279,30,-135,31,-280,109});
    states[15] = new State(new int[]{10,16,96,17});
    states[16] = new State(-37);
    states[17] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35,68,36,63,37,124,38,19,39,18,40,62,41,20,42,125,43,126,44,127,45,128,46,129,47,130,48,131,49,132,50,133,51,134,52,21,53,73,54,87,55,22,56,23,57,26,58,27,59,28,60,71,61,95,62,29,63,88,64,30,65,31,66,24,67,100,68,97,69,32,70,33,71,34,72,37,73,38,74,39,75,99,76,40,77,43,78,45,79,46,80,47,81,93,82,48,83,98,84,49,85,25,86,50,87,70,88,94,89,51,90,52,91,53,92,54,93,55,94,56,95,57,96,58,97,60,98,101,99,102,100,105,101,103,102,104,103,61,104,74,105,35,106,36,107,42,108,44,110},new int[]{-294,18,-143,19,-123,113,-132,22,-136,24,-137,27,-279,30,-135,31,-280,109});
    states[18] = new State(-39);
    states[19] = new State(new int[]{7,20,133,111,10,-40,96,-40});
    states[20] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35,68,36,63,37,124,38,19,39,18,40,62,41,20,42,125,43,126,44,127,45,128,46,129,47,130,48,131,49,132,50,133,51,134,52,21,53,73,54,87,55,22,56,23,57,26,58,27,59,28,60,71,61,95,62,29,63,88,64,30,65,31,66,24,67,100,68,97,69,32,70,33,71,34,72,37,73,38,74,39,75,99,76,40,77,43,78,45,79,46,80,47,81,93,82,48,83,98,84,49,85,25,86,50,87,70,88,94,89,51,90,52,91,53,92,54,93,55,94,56,95,57,96,58,97,60,98,101,99,102,100,105,101,103,102,104,103,61,104,74,105,35,106,36,107,42,108,44,110},new int[]{-123,21,-132,22,-136,24,-137,27,-279,30,-135,31,-280,109});
    states[21] = new State(-35);
    states[22] = new State(-784);
    states[23] = new State(-781);
    states[24] = new State(-782);
    states[25] = new State(-799);
    states[26] = new State(-800);
    states[27] = new State(-783);
    states[28] = new State(-801);
    states[29] = new State(-802);
    states[30] = new State(-785);
    states[31] = new State(-807);
    states[32] = new State(-803);
    states[33] = new State(-804);
    states[34] = new State(-805);
    states[35] = new State(-806);
    states[36] = new State(-808);
    states[37] = new State(-809);
    states[38] = new State(-810);
    states[39] = new State(-811);
    states[40] = new State(-812);
    states[41] = new State(-813);
    states[42] = new State(-814);
    states[43] = new State(-815);
    states[44] = new State(-816);
    states[45] = new State(-817);
    states[46] = new State(-818);
    states[47] = new State(-819);
    states[48] = new State(-820);
    states[49] = new State(-821);
    states[50] = new State(-822);
    states[51] = new State(-823);
    states[52] = new State(-824);
    states[53] = new State(-825);
    states[54] = new State(-826);
    states[55] = new State(-827);
    states[56] = new State(-828);
    states[57] = new State(-829);
    states[58] = new State(-830);
    states[59] = new State(-831);
    states[60] = new State(-832);
    states[61] = new State(-833);
    states[62] = new State(-834);
    states[63] = new State(-835);
    states[64] = new State(-836);
    states[65] = new State(-837);
    states[66] = new State(-838);
    states[67] = new State(-839);
    states[68] = new State(-840);
    states[69] = new State(-841);
    states[70] = new State(-842);
    states[71] = new State(-843);
    states[72] = new State(-844);
    states[73] = new State(-845);
    states[74] = new State(-846);
    states[75] = new State(-847);
    states[76] = new State(-848);
    states[77] = new State(-849);
    states[78] = new State(-850);
    states[79] = new State(-851);
    states[80] = new State(-852);
    states[81] = new State(-853);
    states[82] = new State(-854);
    states[83] = new State(-855);
    states[84] = new State(-856);
    states[85] = new State(-857);
    states[86] = new State(-858);
    states[87] = new State(-859);
    states[88] = new State(-860);
    states[89] = new State(-861);
    states[90] = new State(-862);
    states[91] = new State(-863);
    states[92] = new State(-864);
    states[93] = new State(-865);
    states[94] = new State(-866);
    states[95] = new State(-867);
    states[96] = new State(-868);
    states[97] = new State(-869);
    states[98] = new State(-870);
    states[99] = new State(-871);
    states[100] = new State(-872);
    states[101] = new State(-873);
    states[102] = new State(-874);
    states[103] = new State(-875);
    states[104] = new State(-876);
    states[105] = new State(-877);
    states[106] = new State(-878);
    states[107] = new State(-879);
    states[108] = new State(-880);
    states[109] = new State(-786);
    states[110] = new State(-881);
    states[111] = new State(new int[]{140,112});
    states[112] = new State(-41);
    states[113] = new State(-34);
    states[114] = new State(-38);
    states[115] = new State(new int[]{87,117},new int[]{-241,116});
    states[116] = new State(-32);
    states[117] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484},new int[]{-238,118,-247,715,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[118] = new State(new int[]{88,119,10,120});
    states[119] = new State(-520);
    states[120] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484},new int[]{-247,121,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[121] = new State(-522);
    states[122] = new State(-482);
    states[123] = new State(-485);
    states[124] = new State(new int[]{106,468,107,469,108,470,109,471,110,472,88,-518,10,-518,94,-518,97,-518,30,-518,100,-518,96,-518,12,-518,9,-518,95,-518,29,-518,83,-518,82,-518,2,-518,81,-518,80,-518,79,-518,78,-518},new int[]{-181,125});
    states[125] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,5,601,34,641,43,646},new int[]{-82,126,-81,127,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600,-309,639,-310,640});
    states[126] = new State(-512);
    states[127] = new State(-584);
    states[128] = new State(new int[]{13,129,88,-586,10,-586,94,-586,97,-586,30,-586,100,-586,96,-586,12,-586,9,-586,95,-586,29,-586,83,-586,82,-586,2,-586,81,-586,80,-586,79,-586,78,-586,6,-586});
    states[129] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,130,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[130] = new State(new int[]{5,131,13,129});
    states[131] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,132,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[132] = new State(new int[]{13,129,88,-597,10,-597,94,-597,97,-597,30,-597,100,-597,96,-597,12,-597,9,-597,95,-597,29,-597,83,-597,82,-597,2,-597,81,-597,80,-597,79,-597,78,-597,5,-597,6,-597,50,-597,57,-597,137,-597,139,-597,77,-597,75,-597,44,-597,39,-597,8,-597,18,-597,19,-597,140,-597,142,-597,141,-597,150,-597,152,-597,151,-597,56,-597,87,-597,37,-597,22,-597,93,-597,53,-597,32,-597,54,-597,98,-597,46,-597,33,-597,52,-597,59,-597,74,-597,72,-597,35,-597,70,-597,71,-597});
    states[133] = new State(new int[]{16,134,13,-588,88,-588,10,-588,94,-588,97,-588,30,-588,100,-588,96,-588,12,-588,9,-588,95,-588,29,-588,83,-588,82,-588,2,-588,81,-588,80,-588,79,-588,78,-588,5,-588,6,-588,50,-588,57,-588,137,-588,139,-588,77,-588,75,-588,44,-588,39,-588,8,-588,18,-588,19,-588,140,-588,142,-588,141,-588,150,-588,152,-588,151,-588,56,-588,87,-588,37,-588,22,-588,93,-588,53,-588,32,-588,54,-588,98,-588,46,-588,33,-588,52,-588,59,-588,74,-588,72,-588,35,-588,70,-588,71,-588});
    states[134] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-89,135,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598});
    states[135] = new State(new int[]{116,243,121,244,119,245,117,246,120,247,118,248,133,249,16,-593,13,-593,88,-593,10,-593,94,-593,97,-593,30,-593,100,-593,96,-593,12,-593,9,-593,95,-593,29,-593,83,-593,82,-593,2,-593,81,-593,80,-593,79,-593,78,-593,5,-593,6,-593,50,-593,57,-593,137,-593,139,-593,77,-593,75,-593,44,-593,39,-593,8,-593,18,-593,19,-593,140,-593,142,-593,141,-593,150,-593,152,-593,151,-593,56,-593,87,-593,37,-593,22,-593,93,-593,53,-593,32,-593,54,-593,98,-593,46,-593,33,-593,52,-593,59,-593,74,-593,72,-593,35,-593,70,-593,71,-593},new int[]{-183,136});
    states[136] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-93,137,-76,260,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,605,-253,598});
    states[137] = new State(new int[]{112,255,111,256,124,257,125,258,122,259,116,-615,121,-615,119,-615,117,-615,120,-615,118,-615,133,-615,16,-615,13,-615,88,-615,10,-615,94,-615,97,-615,30,-615,100,-615,96,-615,12,-615,9,-615,95,-615,29,-615,83,-615,82,-615,2,-615,81,-615,80,-615,79,-615,78,-615,5,-615,6,-615,50,-615,57,-615,137,-615,139,-615,77,-615,75,-615,44,-615,39,-615,8,-615,18,-615,19,-615,140,-615,142,-615,141,-615,150,-615,152,-615,151,-615,56,-615,87,-615,37,-615,22,-615,93,-615,53,-615,32,-615,54,-615,98,-615,46,-615,33,-615,52,-615,59,-615,74,-615,72,-615,35,-615,70,-615,71,-615},new int[]{-184,138});
    states[138] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-76,139,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,605,-253,598});
    states[139] = new State(new int[]{134,261,132,263,114,265,113,266,127,267,128,268,129,269,130,270,126,271,5,-692,112,-692,111,-692,124,-692,125,-692,122,-692,116,-692,121,-692,119,-692,117,-692,120,-692,118,-692,133,-692,16,-692,13,-692,88,-692,10,-692,94,-692,97,-692,30,-692,100,-692,96,-692,12,-692,9,-692,95,-692,29,-692,83,-692,82,-692,2,-692,81,-692,80,-692,79,-692,78,-692,6,-692,50,-692,57,-692,137,-692,139,-692,77,-692,75,-692,44,-692,39,-692,8,-692,18,-692,19,-692,140,-692,142,-692,141,-692,150,-692,152,-692,151,-692,56,-692,87,-692,37,-692,22,-692,93,-692,53,-692,32,-692,54,-692,98,-692,46,-692,33,-692,52,-692,59,-692,74,-692,72,-692,35,-692,70,-692,71,-692},new int[]{-185,140});
    states[140] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,29,44,444,39,474,8,509,18,339,19,344},new int[]{-88,141,-254,142,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-77,505});
    states[141] = new State(new int[]{134,-709,132,-709,114,-709,113,-709,127,-709,128,-709,129,-709,130,-709,126,-709,5,-709,112,-709,111,-709,124,-709,125,-709,122,-709,116,-709,121,-709,119,-709,117,-709,120,-709,118,-709,133,-709,16,-709,13,-709,88,-709,10,-709,94,-709,97,-709,30,-709,100,-709,96,-709,12,-709,9,-709,95,-709,29,-709,83,-709,82,-709,2,-709,81,-709,80,-709,79,-709,78,-709,6,-709,50,-709,57,-709,137,-709,139,-709,77,-709,75,-709,44,-709,39,-709,8,-709,18,-709,19,-709,140,-709,142,-709,141,-709,150,-709,152,-709,151,-709,56,-709,87,-709,37,-709,22,-709,93,-709,53,-709,32,-709,54,-709,98,-709,46,-709,33,-709,52,-709,59,-709,74,-709,72,-709,35,-709,70,-709,71,-709,115,-704});
    states[142] = new State(-710);
    states[143] = new State(-721);
    states[144] = new State(new int[]{7,145,134,-722,132,-722,114,-722,113,-722,127,-722,128,-722,129,-722,130,-722,126,-722,5,-722,112,-722,111,-722,124,-722,125,-722,122,-722,116,-722,121,-722,119,-722,117,-722,120,-722,118,-722,133,-722,16,-722,13,-722,88,-722,10,-722,94,-722,97,-722,30,-722,100,-722,96,-722,12,-722,9,-722,95,-722,29,-722,83,-722,82,-722,2,-722,81,-722,80,-722,79,-722,78,-722,115,-722,6,-722,50,-722,57,-722,137,-722,139,-722,77,-722,75,-722,44,-722,39,-722,8,-722,18,-722,19,-722,140,-722,142,-722,141,-722,150,-722,152,-722,151,-722,56,-722,87,-722,37,-722,22,-722,93,-722,53,-722,32,-722,54,-722,98,-722,46,-722,33,-722,52,-722,59,-722,74,-722,72,-722,35,-722,70,-722,71,-722,11,-745});
    states[145] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35,68,36,63,37,124,38,19,39,18,40,62,41,20,42,125,43,126,44,127,45,128,46,129,47,130,48,131,49,132,50,133,51,134,52,21,53,73,54,87,55,22,56,23,57,26,58,27,59,28,60,71,61,95,62,29,63,88,64,30,65,31,66,24,67,100,68,97,69,32,70,33,71,34,72,37,73,38,74,39,75,99,76,40,77,43,78,45,79,46,80,47,81,93,82,48,83,98,84,49,85,25,86,50,87,70,88,94,89,51,90,52,91,53,92,54,93,55,94,56,95,57,96,58,97,60,98,101,99,102,100,105,101,103,102,104,103,61,104,74,105,35,106,36,107,42,108,44,110},new int[]{-123,146,-132,22,-136,24,-137,27,-279,30,-135,31,-280,109});
    states[146] = new State(-752);
    states[147] = new State(-729);
    states[148] = new State(new int[]{140,150,142,151,7,-770,11,-770,134,-770,132,-770,114,-770,113,-770,127,-770,128,-770,129,-770,130,-770,126,-770,5,-770,112,-770,111,-770,124,-770,125,-770,122,-770,116,-770,121,-770,119,-770,117,-770,120,-770,118,-770,133,-770,16,-770,13,-770,88,-770,10,-770,94,-770,97,-770,30,-770,100,-770,96,-770,12,-770,9,-770,95,-770,29,-770,83,-770,82,-770,2,-770,81,-770,80,-770,79,-770,78,-770,115,-770,6,-770,50,-770,57,-770,137,-770,139,-770,77,-770,75,-770,44,-770,39,-770,8,-770,18,-770,19,-770,141,-770,150,-770,152,-770,151,-770,56,-770,87,-770,37,-770,22,-770,93,-770,53,-770,32,-770,54,-770,98,-770,46,-770,33,-770,52,-770,59,-770,74,-770,72,-770,35,-770,70,-770,71,-770,123,-770,106,-770,4,-770,138,-770},new int[]{-152,149});
    states[149] = new State(-773);
    states[150] = new State(-768);
    states[151] = new State(-769);
    states[152] = new State(-772);
    states[153] = new State(-771);
    states[154] = new State(-730);
    states[155] = new State(-176);
    states[156] = new State(-177);
    states[157] = new State(-178);
    states[158] = new State(-723);
    states[159] = new State(new int[]{8,160});
    states[160] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-270,161,-167,163,-132,198,-136,24,-137,27});
    states[161] = new State(new int[]{9,162});
    states[162] = new State(-719);
    states[163] = new State(new int[]{7,164,4,167,119,170,9,-600,132,-600,134,-600,114,-600,113,-600,127,-600,128,-600,129,-600,130,-600,126,-600,112,-600,111,-600,124,-600,125,-600,116,-600,121,-600,117,-600,120,-600,118,-600,133,-600,13,-600,6,-600,96,-600,12,-600,5,-600,88,-600,10,-600,94,-600,97,-600,30,-600,100,-600,95,-600,29,-600,83,-600,82,-600,2,-600,81,-600,80,-600,79,-600,78,-600,122,-600,16,-600,50,-600,57,-600,137,-600,139,-600,77,-600,75,-600,44,-600,39,-600,8,-600,18,-600,19,-600,140,-600,142,-600,141,-600,150,-600,152,-600,151,-600,56,-600,87,-600,37,-600,22,-600,93,-600,53,-600,32,-600,54,-600,98,-600,46,-600,33,-600,52,-600,59,-600,74,-600,72,-600,35,-600,70,-600,71,-600,11,-600,115,-600},new int[]{-285,166});
    states[164] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35,68,36,63,37,124,38,19,39,18,40,62,41,20,42,125,43,126,44,127,45,128,46,129,47,130,48,131,49,132,50,133,51,134,52,21,53,73,54,87,55,22,56,23,57,26,58,27,59,28,60,71,61,95,62,29,63,88,64,30,65,31,66,24,67,100,68,97,69,32,70,33,71,34,72,37,73,38,74,39,75,99,76,40,77,43,78,45,79,46,80,47,81,93,82,48,83,98,84,49,85,25,86,50,87,70,88,94,89,51,90,52,91,53,92,54,93,55,94,56,95,57,96,58,97,60,98,101,99,102,100,105,101,103,102,104,103,61,104,74,105,35,106,36,107,42,108,44,110},new int[]{-123,165,-132,22,-136,24,-137,27,-279,30,-135,31,-280,109});
    states[165] = new State(-252);
    states[166] = new State(-601);
    states[167] = new State(new int[]{119,170,11,208},new int[]{-287,168,-285,169,-288,207});
    states[168] = new State(-602);
    states[169] = new State(-209);
    states[170] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-283,171,-265,211,-258,175,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-267,1349,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,1350,-211,541,-210,542,-289,1351});
    states[171] = new State(new int[]{117,172,96,173});
    states[172] = new State(-226);
    states[173] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-265,174,-258,175,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-267,1349,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,1350,-211,541,-210,542,-289,1351});
    states[174] = new State(-230);
    states[175] = new State(new int[]{13,176,117,-234,96,-234,12,-234,116,-234,9,-234,10,-234,123,-234,106,-234,88,-234,94,-234,97,-234,30,-234,100,-234,95,-234,29,-234,83,-234,82,-234,2,-234,81,-234,80,-234,79,-234,78,-234,133,-234});
    states[176] = new State(-235);
    states[177] = new State(new int[]{6,1409,112,1398,111,1399,124,1400,125,1401,13,-239,117,-239,96,-239,12,-239,116,-239,9,-239,10,-239,123,-239,106,-239,88,-239,94,-239,97,-239,30,-239,100,-239,95,-239,29,-239,83,-239,82,-239,2,-239,81,-239,80,-239,79,-239,78,-239,133,-239},new int[]{-180,178});
    states[178] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153},new int[]{-94,179,-95,220,-167,361,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152});
    states[179] = new State(new int[]{114,213,113,214,127,215,128,216,129,217,130,218,126,219,6,-243,112,-243,111,-243,124,-243,125,-243,13,-243,117,-243,96,-243,12,-243,116,-243,9,-243,10,-243,123,-243,106,-243,88,-243,94,-243,97,-243,30,-243,100,-243,95,-243,29,-243,83,-243,82,-243,2,-243,81,-243,80,-243,79,-243,78,-243,133,-243},new int[]{-182,180});
    states[180] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153},new int[]{-95,181,-167,361,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152});
    states[181] = new State(new int[]{8,182,114,-245,113,-245,127,-245,128,-245,129,-245,130,-245,126,-245,6,-245,112,-245,111,-245,124,-245,125,-245,13,-245,117,-245,96,-245,12,-245,116,-245,9,-245,10,-245,123,-245,106,-245,88,-245,94,-245,97,-245,30,-245,100,-245,95,-245,29,-245,83,-245,82,-245,2,-245,81,-245,80,-245,79,-245,78,-245,133,-245});
    states[182] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363,9,-171},new int[]{-69,183,-67,185,-86,866,-83,188,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[183] = new State(new int[]{9,184});
    states[184] = new State(-250);
    states[185] = new State(new int[]{96,186,9,-170,12,-170});
    states[186] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-86,187,-83,188,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[187] = new State(-173);
    states[188] = new State(new int[]{13,189,6,1382,96,-174,9,-174,12,-174,5,-174});
    states[189] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-83,190,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[190] = new State(new int[]{5,191,13,189});
    states[191] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-83,192,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[192] = new State(new int[]{13,189,6,-116,96,-116,9,-116,12,-116,5,-116,88,-116,10,-116,94,-116,97,-116,30,-116,100,-116,95,-116,29,-116,83,-116,82,-116,2,-116,81,-116,80,-116,79,-116,78,-116});
    states[193] = new State(new int[]{112,1398,111,1399,124,1400,125,1401,116,1402,121,1403,119,1404,117,1405,120,1406,118,1407,133,1408,13,-113,6,-113,96,-113,9,-113,12,-113,5,-113,88,-113,10,-113,94,-113,97,-113,30,-113,100,-113,95,-113,29,-113,83,-113,82,-113,2,-113,81,-113,80,-113,79,-113,78,-113},new int[]{-180,194,-179,1396});
    states[194] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-12,195,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879});
    states[195] = new State(new int[]{132,315,134,316,114,213,113,214,127,215,128,216,129,217,130,218,126,219,112,-125,111,-125,124,-125,125,-125,116,-125,121,-125,119,-125,117,-125,120,-125,118,-125,133,-125,13,-125,6,-125,96,-125,9,-125,12,-125,5,-125,88,-125,10,-125,94,-125,97,-125,30,-125,100,-125,95,-125,29,-125,83,-125,82,-125,2,-125,81,-125,80,-125,79,-125,78,-125},new int[]{-188,196,-182,199});
    states[196] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-270,197,-167,163,-132,198,-136,24,-137,27});
    states[197] = new State(-130);
    states[198] = new State(-251);
    states[199] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-10,200,-255,1395,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877});
    states[200] = new State(new int[]{115,201,132,-135,134,-135,114,-135,113,-135,127,-135,128,-135,129,-135,130,-135,126,-135,112,-135,111,-135,124,-135,125,-135,116,-135,121,-135,119,-135,117,-135,120,-135,118,-135,133,-135,13,-135,6,-135,96,-135,9,-135,12,-135,5,-135,88,-135,10,-135,94,-135,97,-135,30,-135,100,-135,95,-135,29,-135,83,-135,82,-135,2,-135,81,-135,80,-135,79,-135,78,-135});
    states[201] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-10,202,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877});
    states[202] = new State(-131);
    states[203] = new State(new int[]{4,205,11,1371,7,1388,138,1390,8,1391,115,-144,132,-144,134,-144,114,-144,113,-144,127,-144,128,-144,129,-144,130,-144,126,-144,112,-144,111,-144,124,-144,125,-144,116,-144,121,-144,119,-144,117,-144,120,-144,118,-144,133,-144,13,-144,6,-144,96,-144,9,-144,12,-144,5,-144,88,-144,10,-144,94,-144,97,-144,30,-144,100,-144,95,-144,29,-144,83,-144,82,-144,2,-144,81,-144,80,-144,79,-144,78,-144},new int[]{-11,204});
    states[204] = new State(-161);
    states[205] = new State(new int[]{119,170,11,208},new int[]{-287,206,-285,169,-288,207});
    states[206] = new State(-162);
    states[207] = new State(-210);
    states[208] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-283,209,-265,211,-258,175,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-267,1349,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,1350,-211,541,-210,542,-289,1351});
    states[209] = new State(new int[]{12,210,96,173});
    states[210] = new State(-208);
    states[211] = new State(-229);
    states[212] = new State(new int[]{114,213,113,214,127,215,128,216,129,217,130,218,126,219,6,-242,112,-242,111,-242,124,-242,125,-242,13,-242,117,-242,96,-242,12,-242,116,-242,9,-242,10,-242,123,-242,106,-242,88,-242,94,-242,97,-242,30,-242,100,-242,95,-242,29,-242,83,-242,82,-242,2,-242,81,-242,80,-242,79,-242,78,-242,133,-242},new int[]{-182,180});
    states[213] = new State(-137);
    states[214] = new State(-138);
    states[215] = new State(-139);
    states[216] = new State(-140);
    states[217] = new State(-141);
    states[218] = new State(-142);
    states[219] = new State(-143);
    states[220] = new State(new int[]{8,182,114,-244,113,-244,127,-244,128,-244,129,-244,130,-244,126,-244,6,-244,112,-244,111,-244,124,-244,125,-244,13,-244,117,-244,96,-244,12,-244,116,-244,9,-244,10,-244,123,-244,106,-244,88,-244,94,-244,97,-244,30,-244,100,-244,95,-244,29,-244,83,-244,82,-244,2,-244,81,-244,80,-244,79,-244,78,-244,133,-244});
    states[221] = new State(new int[]{7,164,123,222,119,170,8,-246,114,-246,113,-246,127,-246,128,-246,129,-246,130,-246,126,-246,6,-246,112,-246,111,-246,124,-246,125,-246,13,-246,117,-246,96,-246,12,-246,116,-246,9,-246,10,-246,106,-246,88,-246,94,-246,97,-246,30,-246,100,-246,95,-246,29,-246,83,-246,82,-246,2,-246,81,-246,80,-246,79,-246,78,-246,133,-246},new int[]{-285,635});
    states[222] = new State(new int[]{8,224,139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-265,223,-258,175,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-267,1349,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,1350,-211,541,-210,542,-289,1351});
    states[223] = new State(-281);
    states[224] = new State(new int[]{9,225,139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-74,230,-72,236,-262,239,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[225] = new State(new int[]{123,226,117,-285,96,-285,12,-285,116,-285,9,-285,10,-285,106,-285,88,-285,94,-285,97,-285,30,-285,100,-285,95,-285,29,-285,83,-285,82,-285,2,-285,81,-285,80,-285,79,-285,78,-285,133,-285});
    states[226] = new State(new int[]{8,228,139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-265,227,-258,175,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-267,1349,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,1350,-211,541,-210,542,-289,1351});
    states[227] = new State(-283);
    states[228] = new State(new int[]{9,229,139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-74,230,-72,236,-262,239,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[229] = new State(new int[]{123,226,117,-287,96,-287,12,-287,116,-287,9,-287,10,-287,106,-287,88,-287,94,-287,97,-287,30,-287,100,-287,95,-287,29,-287,83,-287,82,-287,2,-287,81,-287,80,-287,79,-287,78,-287,133,-287});
    states[230] = new State(new int[]{9,231,96,1008});
    states[231] = new State(new int[]{123,232,13,-241,117,-241,96,-241,12,-241,116,-241,9,-241,10,-241,106,-241,88,-241,94,-241,97,-241,30,-241,100,-241,95,-241,29,-241,83,-241,82,-241,2,-241,81,-241,80,-241,79,-241,78,-241,133,-241});
    states[232] = new State(new int[]{8,234,139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-265,233,-258,175,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-267,1349,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,1350,-211,541,-210,542,-289,1351});
    states[233] = new State(-284);
    states[234] = new State(new int[]{9,235,139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-74,230,-72,236,-262,239,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[235] = new State(new int[]{123,226,117,-288,96,-288,12,-288,116,-288,9,-288,10,-288,106,-288,88,-288,94,-288,97,-288,30,-288,100,-288,95,-288,29,-288,83,-288,82,-288,2,-288,81,-288,80,-288,79,-288,78,-288,133,-288});
    states[236] = new State(new int[]{96,237});
    states[237] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-72,238,-262,239,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[238] = new State(-253);
    states[239] = new State(new int[]{116,240,96,-255,9,-255});
    states[240] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,241,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[241] = new State(-256);
    states[242] = new State(new int[]{116,243,121,244,119,245,117,246,120,247,118,248,133,249,16,-592,13,-592,88,-592,10,-592,94,-592,97,-592,30,-592,100,-592,96,-592,12,-592,9,-592,95,-592,29,-592,83,-592,82,-592,2,-592,81,-592,80,-592,79,-592,78,-592,5,-592,6,-592,50,-592,57,-592,137,-592,139,-592,77,-592,75,-592,44,-592,39,-592,8,-592,18,-592,19,-592,140,-592,142,-592,141,-592,150,-592,152,-592,151,-592,56,-592,87,-592,37,-592,22,-592,93,-592,53,-592,32,-592,54,-592,98,-592,46,-592,33,-592,52,-592,59,-592,74,-592,72,-592,35,-592,70,-592,71,-592},new int[]{-183,136});
    states[243] = new State(-684);
    states[244] = new State(-685);
    states[245] = new State(-686);
    states[246] = new State(-687);
    states[247] = new State(-688);
    states[248] = new State(-689);
    states[249] = new State(-690);
    states[250] = new State(new int[]{5,251,112,255,111,256,124,257,125,258,122,259,116,-614,121,-614,119,-614,117,-614,120,-614,118,-614,133,-614,16,-614,13,-614,88,-614,10,-614,94,-614,97,-614,30,-614,100,-614,96,-614,12,-614,9,-614,95,-614,29,-614,83,-614,82,-614,2,-614,81,-614,80,-614,79,-614,78,-614,6,-614},new int[]{-184,138});
    states[251] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,-673,88,-673,10,-673,94,-673,97,-673,30,-673,100,-673,96,-673,12,-673,9,-673,95,-673,29,-673,2,-673,81,-673,80,-673,79,-673,78,-673,6,-673},new int[]{-102,252,-93,606,-76,260,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,605,-253,598});
    states[252] = new State(new int[]{5,253,88,-676,10,-676,94,-676,97,-676,30,-676,100,-676,96,-676,12,-676,9,-676,95,-676,29,-676,83,-676,82,-676,2,-676,81,-676,80,-676,79,-676,78,-676,6,-676});
    states[253] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-93,254,-76,260,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,605,-253,598});
    states[254] = new State(new int[]{112,255,111,256,124,257,125,258,122,259,88,-678,10,-678,94,-678,97,-678,30,-678,100,-678,96,-678,12,-678,9,-678,95,-678,29,-678,83,-678,82,-678,2,-678,81,-678,80,-678,79,-678,78,-678,6,-678},new int[]{-184,138});
    states[255] = new State(-693);
    states[256] = new State(-694);
    states[257] = new State(-695);
    states[258] = new State(-696);
    states[259] = new State(-697);
    states[260] = new State(new int[]{134,261,132,263,114,265,113,266,127,267,128,268,129,269,130,270,126,271,112,-691,111,-691,124,-691,125,-691,122,-691,116,-691,121,-691,119,-691,117,-691,120,-691,118,-691,133,-691,16,-691,13,-691,88,-691,10,-691,94,-691,97,-691,30,-691,100,-691,96,-691,12,-691,9,-691,95,-691,29,-691,83,-691,82,-691,2,-691,81,-691,80,-691,79,-691,78,-691,5,-691,6,-691,50,-691,57,-691,137,-691,139,-691,77,-691,75,-691,44,-691,39,-691,8,-691,18,-691,19,-691,140,-691,142,-691,141,-691,150,-691,152,-691,151,-691,56,-691,87,-691,37,-691,22,-691,93,-691,53,-691,32,-691,54,-691,98,-691,46,-691,33,-691,52,-691,59,-691,74,-691,72,-691,35,-691,70,-691,71,-691},new int[]{-185,140});
    states[261] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-270,262,-167,163,-132,198,-136,24,-137,27});
    states[262] = new State(-703);
    states[263] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-270,264,-167,163,-132,198,-136,24,-137,27});
    states[264] = new State(-702);
    states[265] = new State(-712);
    states[266] = new State(-713);
    states[267] = new State(-714);
    states[268] = new State(-715);
    states[269] = new State(-716);
    states[270] = new State(-717);
    states[271] = new State(-718);
    states[272] = new State(new int[]{134,-706,132,-706,114,-706,113,-706,127,-706,128,-706,129,-706,130,-706,126,-706,5,-706,112,-706,111,-706,124,-706,125,-706,122,-706,116,-706,121,-706,119,-706,117,-706,120,-706,118,-706,133,-706,16,-706,13,-706,88,-706,10,-706,94,-706,97,-706,30,-706,100,-706,96,-706,12,-706,9,-706,95,-706,29,-706,83,-706,82,-706,2,-706,81,-706,80,-706,79,-706,78,-706,6,-706,50,-706,57,-706,137,-706,139,-706,77,-706,75,-706,44,-706,39,-706,8,-706,18,-706,19,-706,140,-706,142,-706,141,-706,150,-706,152,-706,151,-706,56,-706,87,-706,37,-706,22,-706,93,-706,53,-706,32,-706,54,-706,98,-706,46,-706,33,-706,52,-706,59,-706,74,-706,72,-706,35,-706,70,-706,71,-706,115,-704});
    states[273] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601,12,-763},new int[]{-64,274,-71,276,-84,1370,-81,279,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[274] = new State(new int[]{12,275});
    states[275] = new State(-724);
    states[276] = new State(new int[]{96,277,12,-762});
    states[277] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-84,278,-81,279,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[278] = new State(-765);
    states[279] = new State(new int[]{6,280,96,-766,12,-766});
    states[280] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,281,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[281] = new State(-767);
    states[282] = new State(new int[]{134,283,132,263,114,265,113,266,127,267,128,268,129,269,130,270,126,271,5,-691,112,-691,111,-691,124,-691,125,-691,122,-691,116,-691,121,-691,119,-691,117,-691,120,-691,118,-691,133,-691,16,-691,13,-691,88,-691,10,-691,94,-691,97,-691,30,-691,100,-691,96,-691,12,-691,9,-691,95,-691,29,-691,83,-691,82,-691,2,-691,81,-691,80,-691,79,-691,78,-691,6,-691,50,-691,57,-691,137,-691,139,-691,77,-691,75,-691,44,-691,39,-691,8,-691,18,-691,19,-691,140,-691,142,-691,141,-691,150,-691,152,-691,151,-691,56,-691,87,-691,37,-691,22,-691,93,-691,53,-691,32,-691,54,-691,98,-691,46,-691,33,-691,52,-691,59,-691,74,-691,72,-691,35,-691,70,-691,71,-691},new int[]{-185,140});
    states[283] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,11,286,8,584},new int[]{-270,262,-330,284,-331,285,-167,163,-132,198,-136,24,-137,27});
    states[284] = new State(-617);
    states[285] = new State(-618);
    states[286] = new State(new int[]{140,150,142,151,141,153,150,155,152,156,151,157,52,293,14,295,139,23,82,25,83,26,77,28,75,29,11,286,8,584,6,1368},new int[]{-342,287,-332,1369,-14,291,-151,147,-153,148,-152,152,-15,154,-334,292,-329,296,-270,297,-167,163,-132,198,-136,24,-137,27,-330,1366,-331,1367});
    states[287] = new State(new int[]{12,288,96,289});
    states[288] = new State(-630);
    states[289] = new State(new int[]{140,150,142,151,141,153,150,155,152,156,151,157,52,293,14,295,139,23,82,25,83,26,77,28,75,29,11,286,8,584,6,1368},new int[]{-332,290,-14,291,-151,147,-153,148,-152,152,-15,154,-334,292,-329,296,-270,297,-167,163,-132,198,-136,24,-137,27,-330,1366,-331,1367});
    states[290] = new State(-632);
    states[291] = new State(-633);
    states[292] = new State(-634);
    states[293] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,294,-136,24,-137,27});
    states[294] = new State(-640);
    states[295] = new State(-635);
    states[296] = new State(-636);
    states[297] = new State(new int[]{8,298});
    states[298] = new State(new int[]{14,303,140,150,142,151,141,153,150,155,152,156,151,157,139,23,82,25,83,26,77,28,75,29,52,829,11,286,8,584},new int[]{-341,299,-339,836,-14,304,-151,147,-153,148,-152,152,-15,154,-132,305,-136,24,-137,27,-329,833,-270,297,-167,163,-330,834,-331,835});
    states[299] = new State(new int[]{9,300,10,301,96,827});
    states[300] = new State(-620);
    states[301] = new State(new int[]{14,303,140,150,142,151,141,153,150,155,152,156,151,157,139,23,82,25,83,26,77,28,75,29,52,829,11,286,8,584},new int[]{-339,302,-14,304,-151,147,-153,148,-152,152,-15,154,-132,305,-136,24,-137,27,-329,833,-270,297,-167,163,-330,834,-331,835});
    states[302] = new State(-651);
    states[303] = new State(-663);
    states[304] = new State(-664);
    states[305] = new State(new int[]{5,306,9,-666,10,-666,96,-666,7,-251,4,-251,119,-251,8,-251});
    states[306] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,307,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[307] = new State(-665);
    states[308] = new State(new int[]{13,309,116,-218,96,-218,9,-218,10,-218,117,-218,12,-218,123,-218,106,-218,88,-218,94,-218,97,-218,30,-218,100,-218,95,-218,29,-218,83,-218,82,-218,2,-218,81,-218,80,-218,79,-218,78,-218,133,-218});
    states[309] = new State(-216);
    states[310] = new State(new int[]{11,311,7,-781,123,-781,119,-781,8,-781,114,-781,113,-781,127,-781,128,-781,129,-781,130,-781,126,-781,6,-781,112,-781,111,-781,124,-781,125,-781,13,-781,116,-781,96,-781,9,-781,10,-781,117,-781,12,-781,106,-781,88,-781,94,-781,97,-781,30,-781,100,-781,95,-781,29,-781,83,-781,82,-781,2,-781,81,-781,80,-781,79,-781,78,-781,133,-781});
    states[311] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-83,312,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[312] = new State(new int[]{12,313,13,189});
    states[313] = new State(-276);
    states[314] = new State(new int[]{132,315,134,316,114,213,113,214,127,215,128,216,129,217,130,218,126,219,112,-124,111,-124,124,-124,125,-124,116,-124,121,-124,119,-124,117,-124,120,-124,118,-124,133,-124,13,-124,6,-124,96,-124,9,-124,12,-124,5,-124,88,-124,10,-124,94,-124,97,-124,30,-124,100,-124,95,-124,29,-124,83,-124,82,-124,2,-124,81,-124,80,-124,79,-124,78,-124},new int[]{-188,196,-182,199});
    states[315] = new State(-698);
    states[316] = new State(-699);
    states[317] = new State(new int[]{115,201,132,-132,134,-132,114,-132,113,-132,127,-132,128,-132,129,-132,130,-132,126,-132,112,-132,111,-132,124,-132,125,-132,116,-132,121,-132,119,-132,117,-132,120,-132,118,-132,133,-132,13,-132,6,-132,96,-132,9,-132,12,-132,5,-132,88,-132,10,-132,94,-132,97,-132,30,-132,100,-132,95,-132,29,-132,83,-132,82,-132,2,-132,81,-132,80,-132,79,-132,78,-132});
    states[318] = new State(-155);
    states[319] = new State(new int[]{23,1355,139,23,82,25,83,26,77,28,75,29,17,-802,8,-802,7,-802,138,-802,4,-802,15,-802,106,-802,107,-802,108,-802,109,-802,110,-802,88,-802,10,-802,11,-802,5,-802,94,-802,97,-802,30,-802,100,-802,123,-802,134,-802,132,-802,114,-802,113,-802,127,-802,128,-802,129,-802,130,-802,126,-802,112,-802,111,-802,124,-802,125,-802,122,-802,116,-802,121,-802,119,-802,117,-802,120,-802,118,-802,133,-802,16,-802,13,-802,96,-802,12,-802,9,-802,95,-802,29,-802,2,-802,81,-802,80,-802,79,-802,78,-802,115,-802,6,-802,50,-802,57,-802,137,-802,44,-802,39,-802,18,-802,19,-802,140,-802,142,-802,141,-802,150,-802,152,-802,151,-802,56,-802,87,-802,37,-802,22,-802,93,-802,53,-802,32,-802,54,-802,98,-802,46,-802,33,-802,52,-802,59,-802,74,-802,72,-802,35,-802,70,-802,71,-802},new int[]{-270,320,-167,163,-132,198,-136,24,-137,27});
    states[320] = new State(new int[]{11,322,8,636,88,-612,10,-612,94,-612,97,-612,30,-612,100,-612,134,-612,132,-612,114,-612,113,-612,127,-612,128,-612,129,-612,130,-612,126,-612,5,-612,112,-612,111,-612,124,-612,125,-612,122,-612,116,-612,121,-612,119,-612,117,-612,120,-612,118,-612,133,-612,16,-612,13,-612,96,-612,12,-612,9,-612,95,-612,29,-612,83,-612,82,-612,2,-612,81,-612,80,-612,79,-612,78,-612,6,-612,50,-612,57,-612,137,-612,139,-612,77,-612,75,-612,44,-612,39,-612,18,-612,19,-612,140,-612,142,-612,141,-612,150,-612,152,-612,151,-612,56,-612,87,-612,37,-612,22,-612,93,-612,53,-612,32,-612,54,-612,98,-612,46,-612,33,-612,52,-612,59,-612,74,-612,72,-612,35,-612,70,-612,71,-612,115,-612},new int[]{-65,321});
    states[321] = new State(-605);
    states[322] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,5,601,34,641,43,646,12,-761},new int[]{-63,323,-66,434,-82,495,-81,127,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600,-309,639,-310,640});
    states[323] = new State(new int[]{12,324});
    states[324] = new State(new int[]{8,326,88,-604,10,-604,94,-604,97,-604,30,-604,100,-604,134,-604,132,-604,114,-604,113,-604,127,-604,128,-604,129,-604,130,-604,126,-604,5,-604,112,-604,111,-604,124,-604,125,-604,122,-604,116,-604,121,-604,119,-604,117,-604,120,-604,118,-604,133,-604,16,-604,13,-604,96,-604,12,-604,9,-604,95,-604,29,-604,83,-604,82,-604,2,-604,81,-604,80,-604,79,-604,78,-604,6,-604,50,-604,57,-604,137,-604,139,-604,77,-604,75,-604,44,-604,39,-604,18,-604,19,-604,140,-604,142,-604,141,-604,150,-604,152,-604,151,-604,56,-604,87,-604,37,-604,22,-604,93,-604,53,-604,32,-604,54,-604,98,-604,46,-604,33,-604,52,-604,59,-604,74,-604,72,-604,35,-604,70,-604,71,-604,115,-604},new int[]{-5,325});
    states[325] = new State(-606);
    states[326] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,894,131,873,112,362,111,363,62,159,9,-183},new int[]{-62,327,-61,329,-79,897,-78,332,-83,333,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880,-87,898,-229,899,-53,900});
    states[327] = new State(new int[]{9,328});
    states[328] = new State(-603);
    states[329] = new State(new int[]{96,330,9,-184});
    states[330] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,894,131,873,112,362,111,363,62,159},new int[]{-79,331,-78,332,-83,333,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880,-87,898,-229,899,-53,900});
    states[331] = new State(-186);
    states[332] = new State(-412);
    states[333] = new State(new int[]{13,189,96,-179,9,-179,88,-179,10,-179,94,-179,97,-179,30,-179,100,-179,12,-179,95,-179,29,-179,83,-179,82,-179,2,-179,81,-179,80,-179,79,-179,78,-179});
    states[334] = new State(-156);
    states[335] = new State(-157);
    states[336] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,337,-136,24,-137,27});
    states[337] = new State(-158);
    states[338] = new State(-159);
    states[339] = new State(new int[]{8,340});
    states[340] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-270,341,-167,163,-132,198,-136,24,-137,27});
    states[341] = new State(new int[]{9,342});
    states[342] = new State(-594);
    states[343] = new State(-160);
    states[344] = new State(new int[]{8,345});
    states[345] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-270,346,-269,348,-167,350,-132,198,-136,24,-137,27});
    states[346] = new State(new int[]{9,347});
    states[347] = new State(-595);
    states[348] = new State(new int[]{9,349});
    states[349] = new State(-596);
    states[350] = new State(new int[]{7,164,4,351,119,353,121,1353,9,-600},new int[]{-285,166,-286,1354});
    states[351] = new State(new int[]{119,353,11,208,121,1353},new int[]{-287,168,-286,352,-285,169,-288,207});
    states[352] = new State(-599);
    states[353] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571,117,-233,96,-233},new int[]{-283,171,-284,354,-265,211,-258,175,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-267,1349,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,1350,-211,541,-210,542,-289,1351,-266,1352});
    states[354] = new State(new int[]{117,355,96,356});
    states[355] = new State(-228);
    states[356] = new State(-233,new int[]{-266,357});
    states[357] = new State(-232);
    states[358] = new State(-247);
    states[359] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153},new int[]{-95,360,-167,361,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152});
    states[360] = new State(new int[]{8,182,114,-248,113,-248,127,-248,128,-248,129,-248,130,-248,126,-248,6,-248,112,-248,111,-248,124,-248,125,-248,13,-248,117,-248,96,-248,12,-248,116,-248,9,-248,10,-248,123,-248,106,-248,88,-248,94,-248,97,-248,30,-248,100,-248,95,-248,29,-248,83,-248,82,-248,2,-248,81,-248,80,-248,79,-248,78,-248,133,-248});
    states[361] = new State(new int[]{7,164,8,-246,114,-246,113,-246,127,-246,128,-246,129,-246,130,-246,126,-246,6,-246,112,-246,111,-246,124,-246,125,-246,13,-246,117,-246,96,-246,12,-246,116,-246,9,-246,10,-246,123,-246,106,-246,88,-246,94,-246,97,-246,30,-246,100,-246,95,-246,29,-246,83,-246,82,-246,2,-246,81,-246,80,-246,79,-246,78,-246,133,-246});
    states[362] = new State(-153);
    states[363] = new State(-154);
    states[364] = new State(-249);
    states[365] = new State(new int[]{9,366,139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-74,230,-72,236,-262,239,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[366] = new State(new int[]{123,226});
    states[367] = new State(-219);
    states[368] = new State(new int[]{13,369,123,370,116,-224,96,-224,9,-224,10,-224,117,-224,12,-224,106,-224,88,-224,94,-224,97,-224,30,-224,100,-224,95,-224,29,-224,83,-224,82,-224,2,-224,81,-224,80,-224,79,-224,78,-224,133,-224});
    states[369] = new State(-217);
    states[370] = new State(new int[]{8,372,139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-265,371,-258,175,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-267,1349,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,1350,-211,541,-210,542,-289,1351});
    states[371] = new State(-282);
    states[372] = new State(new int[]{9,373,139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-74,230,-72,236,-262,239,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[373] = new State(new int[]{123,226,117,-286,96,-286,12,-286,116,-286,9,-286,10,-286,106,-286,88,-286,94,-286,97,-286,30,-286,100,-286,95,-286,29,-286,83,-286,82,-286,2,-286,81,-286,80,-286,79,-286,78,-286,133,-286});
    states[374] = new State(-220);
    states[375] = new State(-221);
    states[376] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,377,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[377] = new State(-257);
    states[378] = new State(-476);
    states[379] = new State(-222);
    states[380] = new State(-258);
    states[381] = new State(-260);
    states[382] = new State(new int[]{11,383,57,1347});
    states[383] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,1005,12,-272,96,-272},new int[]{-150,384,-257,1346,-258,1345,-85,177,-94,212,-95,220,-167,361,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152});
    states[384] = new State(new int[]{12,385,96,1343});
    states[385] = new State(new int[]{57,386});
    states[386] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,387,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[387] = new State(-266);
    states[388] = new State(-267);
    states[389] = new State(-261);
    states[390] = new State(new int[]{8,1204,20,-308,11,-308,88,-308,81,-308,80,-308,79,-308,78,-308,26,-308,139,-308,82,-308,83,-308,77,-308,75,-308,61,-308,25,-308,23,-308,43,-308,34,-308,27,-308,28,-308,45,-308,24,-308},new int[]{-170,391});
    states[391] = new State(new int[]{20,1195,11,-316,88,-316,81,-316,80,-316,79,-316,78,-316,26,-316,139,-316,82,-316,83,-316,77,-316,75,-316,61,-316,25,-316,23,-316,43,-316,34,-316,27,-316,28,-316,45,-316,24,-316},new int[]{-304,392,-303,1193,-302,1221});
    states[392] = new State(new int[]{11,627,88,-334,81,-334,80,-334,79,-334,78,-334,26,-197,139,-197,82,-197,83,-197,77,-197,75,-197,61,-197,25,-197,23,-197,43,-197,34,-197,27,-197,28,-197,45,-197,24,-197},new int[]{-22,393,-29,1173,-31,397,-41,1174,-6,1175,-236,996,-30,1290,-50,1292,-49,403,-51,1291});
    states[393] = new State(new int[]{88,394,81,1169,80,1170,79,1171,78,1172},new int[]{-7,395});
    states[394] = new State(-290);
    states[395] = new State(new int[]{11,627,88,-334,81,-334,80,-334,79,-334,78,-334,26,-197,139,-197,82,-197,83,-197,77,-197,75,-197,61,-197,25,-197,23,-197,43,-197,34,-197,27,-197,28,-197,45,-197,24,-197},new int[]{-29,396,-31,397,-41,1174,-6,1175,-236,996,-30,1290,-50,1292,-49,403,-51,1291});
    states[396] = new State(-329);
    states[397] = new State(new int[]{10,399,88,-340,81,-340,80,-340,79,-340,78,-340},new int[]{-177,398});
    states[398] = new State(-335);
    states[399] = new State(new int[]{11,627,88,-341,81,-341,80,-341,79,-341,78,-341,26,-197,139,-197,82,-197,83,-197,77,-197,75,-197,61,-197,25,-197,23,-197,43,-197,34,-197,27,-197,28,-197,45,-197,24,-197},new int[]{-41,400,-30,401,-6,1175,-236,996,-50,1292,-49,403,-51,1291});
    states[400] = new State(-343);
    states[401] = new State(new int[]{11,627,88,-337,81,-337,80,-337,79,-337,78,-337,25,-197,23,-197,43,-197,34,-197,27,-197,28,-197,45,-197,24,-197},new int[]{-50,402,-49,403,-6,404,-236,996,-51,1291});
    states[402] = new State(-346);
    states[403] = new State(-347);
    states[404] = new State(new int[]{25,1246,23,1247,43,1188,34,1229,27,1261,28,1268,11,627,45,1275,24,1284},new int[]{-209,405,-236,406,-206,407,-244,408,-3,409,-217,1248,-215,1117,-212,1187,-216,1228,-214,1249,-202,1272,-203,1273,-205,1274});
    states[405] = new State(-356);
    states[406] = new State(-196);
    states[407] = new State(-357);
    states[408] = new State(-375);
    states[409] = new State(new int[]{27,411,45,1067,24,1109,43,1188,34,1229},new int[]{-217,410,-203,1066,-215,1117,-212,1187,-216,1228});
    states[410] = new State(-360);
    states[411] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444,8,-370,106,-370,10,-370},new int[]{-158,412,-157,1049,-156,1050,-127,1051,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059,-321,1061,-134,1065});
    states[412] = new State(new int[]{8,545,106,-460,10,-460},new int[]{-113,413});
    states[413] = new State(new int[]{106,415,10,1038},new int[]{-194,414});
    states[414] = new State(-367);
    states[415] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484},new int[]{-246,416,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[416] = new State(new int[]{10,417});
    states[417] = new State(-419);
    states[418] = new State(new int[]{137,1037,139,23,82,25,83,26,77,28,75,29,44,444,39,474,8,509,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157},new int[]{-99,419,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665});
    states[419] = new State(new int[]{17,420,8,431,7,659,138,661,4,662,106,-733,107,-733,108,-733,109,-733,110,-733,88,-733,10,-733,94,-733,97,-733,30,-733,100,-733,134,-733,132,-733,114,-733,113,-733,127,-733,128,-733,129,-733,130,-733,126,-733,5,-733,112,-733,111,-733,124,-733,125,-733,122,-733,116,-733,121,-733,119,-733,117,-733,120,-733,118,-733,133,-733,16,-733,13,-733,96,-733,12,-733,9,-733,95,-733,29,-733,83,-733,82,-733,2,-733,81,-733,80,-733,79,-733,78,-733,115,-733,6,-733,50,-733,57,-733,137,-733,139,-733,77,-733,75,-733,44,-733,39,-733,18,-733,19,-733,140,-733,142,-733,141,-733,150,-733,152,-733,151,-733,56,-733,87,-733,37,-733,22,-733,93,-733,53,-733,32,-733,54,-733,98,-733,46,-733,33,-733,52,-733,59,-733,74,-733,72,-733,35,-733,70,-733,71,-733,11,-744});
    states[420] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-105,421,-93,423,-76,260,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,605,-253,598});
    states[421] = new State(new int[]{12,422});
    states[422] = new State(-754);
    states[423] = new State(new int[]{5,251,112,255,111,256,124,257,125,258,122,259},new int[]{-184,138});
    states[424] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,29,44,444,39,474,8,509,18,339,19,344},new int[]{-88,425,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502});
    states[425] = new State(-725);
    states[426] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,29,44,444,39,474,8,509,18,339,19,344},new int[]{-88,427,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502});
    states[427] = new State(-726);
    states[428] = new State(-727);
    states[429] = new State(-736);
    states[430] = new State(new int[]{17,420,8,431,7,659,138,661,4,662,15,667,106,-734,107,-734,108,-734,109,-734,110,-734,88,-734,10,-734,94,-734,97,-734,30,-734,100,-734,134,-734,132,-734,114,-734,113,-734,127,-734,128,-734,129,-734,130,-734,126,-734,5,-734,112,-734,111,-734,124,-734,125,-734,122,-734,116,-734,121,-734,119,-734,117,-734,120,-734,118,-734,133,-734,16,-734,13,-734,96,-734,12,-734,9,-734,95,-734,29,-734,83,-734,82,-734,2,-734,81,-734,80,-734,79,-734,78,-734,115,-734,6,-734,50,-734,57,-734,137,-734,139,-734,77,-734,75,-734,44,-734,39,-734,18,-734,19,-734,140,-734,142,-734,141,-734,150,-734,152,-734,151,-734,56,-734,87,-734,37,-734,22,-734,93,-734,53,-734,32,-734,54,-734,98,-734,46,-734,33,-734,52,-734,59,-734,74,-734,72,-734,35,-734,70,-734,71,-734,11,-744});
    states[431] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,5,601,34,641,43,646,9,-761},new int[]{-63,432,-66,434,-82,495,-81,127,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600,-309,639,-310,640});
    states[432] = new State(new int[]{9,433});
    states[433] = new State(-755);
    states[434] = new State(new int[]{96,435,12,-760,9,-760});
    states[435] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,5,601,34,641,43,646},new int[]{-82,436,-81,127,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600,-309,639,-310,640});
    states[436] = new State(-581);
    states[437] = new State(new int[]{123,438,17,-746,8,-746,7,-746,138,-746,4,-746,15,-746,134,-746,132,-746,114,-746,113,-746,127,-746,128,-746,129,-746,130,-746,126,-746,5,-746,112,-746,111,-746,124,-746,125,-746,122,-746,116,-746,121,-746,119,-746,117,-746,120,-746,118,-746,133,-746,16,-746,13,-746,88,-746,10,-746,94,-746,97,-746,30,-746,100,-746,96,-746,12,-746,9,-746,95,-746,29,-746,83,-746,82,-746,2,-746,81,-746,80,-746,79,-746,78,-746,115,-746,11,-746});
    states[438] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,439,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[439] = new State(-910);
    states[440] = new State(-945);
    states[441] = new State(new int[]{13,129,88,-590,10,-590,94,-590,97,-590,30,-590,100,-590,96,-590,12,-590,9,-590,95,-590,29,-590,83,-590,82,-590,2,-590,81,-590,80,-590,79,-590,78,-590});
    states[442] = new State(new int[]{112,255,111,256,124,257,125,258,122,259,116,-614,121,-614,119,-614,117,-614,120,-614,118,-614,133,-614,16,-614,5,-614,13,-614,88,-614,10,-614,94,-614,97,-614,30,-614,100,-614,96,-614,12,-614,9,-614,95,-614,29,-614,83,-614,82,-614,2,-614,81,-614,80,-614,79,-614,78,-614,6,-614,50,-614,57,-614,137,-614,139,-614,77,-614,75,-614,44,-614,39,-614,8,-614,18,-614,19,-614,140,-614,142,-614,141,-614,150,-614,152,-614,151,-614,56,-614,87,-614,37,-614,22,-614,93,-614,53,-614,32,-614,54,-614,98,-614,46,-614,33,-614,52,-614,59,-614,74,-614,72,-614,35,-614,70,-614,71,-614},new int[]{-184,138});
    states[443] = new State(-747);
    states[444] = new State(new int[]{111,446,112,447,113,448,114,449,116,450,117,451,118,452,119,453,120,454,121,455,124,456,125,457,126,458,127,459,128,460,129,461,130,462,131,463,133,464,135,465,136,466,106,468,107,469,108,470,109,471,110,472,115,473},new int[]{-187,445,-181,467});
    states[445] = new State(-774);
    states[446] = new State(-882);
    states[447] = new State(-883);
    states[448] = new State(-884);
    states[449] = new State(-885);
    states[450] = new State(-886);
    states[451] = new State(-887);
    states[452] = new State(-888);
    states[453] = new State(-889);
    states[454] = new State(-890);
    states[455] = new State(-891);
    states[456] = new State(-892);
    states[457] = new State(-893);
    states[458] = new State(-894);
    states[459] = new State(-895);
    states[460] = new State(-896);
    states[461] = new State(-897);
    states[462] = new State(-898);
    states[463] = new State(-899);
    states[464] = new State(-900);
    states[465] = new State(-901);
    states[466] = new State(-902);
    states[467] = new State(-903);
    states[468] = new State(-905);
    states[469] = new State(-906);
    states[470] = new State(-907);
    states[471] = new State(-908);
    states[472] = new State(-909);
    states[473] = new State(-904);
    states[474] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,475,-136,24,-137,27});
    states[475] = new State(-748);
    states[476] = new State(new int[]{9,1014,55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,477,-91,479,-132,1018,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[477] = new State(new int[]{9,478});
    states[478] = new State(-749);
    states[479] = new State(new int[]{96,480,13,129,9,-586});
    states[480] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-73,481,-91,1000,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[481] = new State(new int[]{96,998,5,524,10,-929,9,-929},new int[]{-311,482});
    states[482] = new State(new int[]{10,516,9,-917},new int[]{-318,483});
    states[483] = new State(new int[]{9,484});
    states[484] = new State(new int[]{5,1001,7,-720,134,-720,132,-720,114,-720,113,-720,127,-720,128,-720,129,-720,130,-720,126,-720,112,-720,111,-720,124,-720,125,-720,122,-720,116,-720,121,-720,119,-720,117,-720,120,-720,118,-720,133,-720,16,-720,13,-720,88,-720,10,-720,94,-720,97,-720,30,-720,100,-720,96,-720,12,-720,9,-720,95,-720,29,-720,83,-720,82,-720,2,-720,81,-720,80,-720,79,-720,78,-720,115,-720,123,-931},new int[]{-322,485,-312,486});
    states[485] = new State(-915);
    states[486] = new State(new int[]{123,487});
    states[487] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,488,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[488] = new State(-919);
    states[489] = new State(-750);
    states[490] = new State(-751);
    states[491] = new State(new int[]{11,492});
    states[492] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,5,601,34,641,43,646},new int[]{-66,493,-82,495,-81,127,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600,-309,639,-310,640});
    states[493] = new State(new int[]{12,494,96,435});
    states[494] = new State(-753);
    states[495] = new State(-580);
    states[496] = new State(new int[]{7,497,134,-728,132,-728,114,-728,113,-728,127,-728,128,-728,129,-728,130,-728,126,-728,5,-728,112,-728,111,-728,124,-728,125,-728,122,-728,116,-728,121,-728,119,-728,117,-728,120,-728,118,-728,133,-728,16,-728,13,-728,88,-728,10,-728,94,-728,97,-728,30,-728,100,-728,96,-728,12,-728,9,-728,95,-728,29,-728,83,-728,82,-728,2,-728,81,-728,80,-728,79,-728,78,-728,115,-728,6,-728,50,-728,57,-728,137,-728,139,-728,77,-728,75,-728,44,-728,39,-728,8,-728,18,-728,19,-728,140,-728,142,-728,141,-728,150,-728,152,-728,151,-728,56,-728,87,-728,37,-728,22,-728,93,-728,53,-728,32,-728,54,-728,98,-728,46,-728,33,-728,52,-728,59,-728,74,-728,72,-728,35,-728,70,-728,71,-728});
    states[497] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35,68,36,63,37,124,38,19,39,18,40,62,41,20,42,125,43,126,44,127,45,128,46,129,47,130,48,131,49,132,50,133,51,134,52,21,53,73,54,87,55,22,56,23,57,26,58,27,59,28,60,71,61,95,62,29,63,88,64,30,65,31,66,24,67,100,68,97,69,32,70,33,71,34,72,37,73,38,74,39,75,99,76,40,77,43,78,45,79,46,80,47,81,93,82,48,83,98,84,49,85,25,86,50,87,70,88,94,89,51,90,52,91,53,92,54,93,55,94,56,95,57,96,58,97,60,98,101,99,102,100,105,101,103,102,104,103,61,104,74,105,35,106,36,107,42,108,44,444},new int[]{-133,498,-132,499,-136,24,-137,27,-279,500,-135,31,-178,501});
    states[498] = new State(-757);
    states[499] = new State(-787);
    states[500] = new State(-788);
    states[501] = new State(-789);
    states[502] = new State(-735);
    states[503] = new State(-707);
    states[504] = new State(-708);
    states[505] = new State(new int[]{115,506});
    states[506] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,29,44,444,39,474,8,509,18,339,19,344},new int[]{-88,507,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502});
    states[507] = new State(-705);
    states[508] = new State(-746);
    states[509] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,477,-91,510,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[510] = new State(new int[]{96,511,13,129,9,-586});
    states[511] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-73,512,-91,1000,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[512] = new State(new int[]{96,998,5,524,10,-929,9,-929},new int[]{-311,513});
    states[513] = new State(new int[]{10,516,9,-917},new int[]{-318,514});
    states[514] = new State(new int[]{9,515});
    states[515] = new State(-720);
    states[516] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-313,517,-314,979,-144,520,-132,772,-136,24,-137,27});
    states[517] = new State(new int[]{10,518,9,-918});
    states[518] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-314,519,-144,520,-132,772,-136,24,-137,27});
    states[519] = new State(-927);
    states[520] = new State(new int[]{96,522,5,524,10,-929,9,-929},new int[]{-311,521});
    states[521] = new State(-928);
    states[522] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,523,-136,24,-137,27});
    states[523] = new State(-339);
    states[524] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,525,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[525] = new State(-930);
    states[526] = new State(-262);
    states[527] = new State(new int[]{57,528});
    states[528] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,529,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[529] = new State(-273);
    states[530] = new State(-263);
    states[531] = new State(new int[]{57,532,117,-275,96,-275,12,-275,116,-275,9,-275,10,-275,123,-275,106,-275,88,-275,94,-275,97,-275,30,-275,100,-275,95,-275,29,-275,83,-275,82,-275,2,-275,81,-275,80,-275,79,-275,78,-275,133,-275});
    states[532] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,533,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[533] = new State(-274);
    states[534] = new State(-264);
    states[535] = new State(new int[]{57,536});
    states[536] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,537,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[537] = new State(-265);
    states[538] = new State(new int[]{21,382,47,390,48,527,31,531,73,535},new int[]{-268,539,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534});
    states[539] = new State(-259);
    states[540] = new State(-223);
    states[541] = new State(-277);
    states[542] = new State(-278);
    states[543] = new State(new int[]{8,545,117,-460,96,-460,12,-460,116,-460,9,-460,10,-460,123,-460,106,-460,88,-460,94,-460,97,-460,30,-460,100,-460,95,-460,29,-460,83,-460,82,-460,2,-460,81,-460,80,-460,79,-460,78,-460,133,-460},new int[]{-113,544});
    states[544] = new State(-279);
    states[545] = new State(new int[]{9,546,11,627,139,-197,82,-197,83,-197,77,-197,75,-197,52,-197,26,-197,104,-197},new int[]{-114,547,-52,997,-6,551,-236,996});
    states[546] = new State(-461);
    states[547] = new State(new int[]{9,548,10,549});
    states[548] = new State(-462);
    states[549] = new State(new int[]{11,627,139,-197,82,-197,83,-197,77,-197,75,-197,52,-197,26,-197,104,-197},new int[]{-52,550,-6,551,-236,996});
    states[550] = new State(-464);
    states[551] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,52,611,26,617,104,623,11,627},new int[]{-282,552,-236,406,-145,553,-120,610,-132,609,-136,24,-137,27});
    states[552] = new State(-465);
    states[553] = new State(new int[]{5,554,96,607});
    states[554] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,555,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[555] = new State(new int[]{106,556,9,-466,10,-466});
    states[556] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,557,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[557] = new State(-470);
    states[558] = new State(-711);
    states[559] = new State(new int[]{8,560,134,-700,132,-700,114,-700,113,-700,127,-700,128,-700,129,-700,130,-700,126,-700,5,-700,112,-700,111,-700,124,-700,125,-700,122,-700,116,-700,121,-700,119,-700,117,-700,120,-700,118,-700,133,-700,16,-700,13,-700,88,-700,10,-700,94,-700,97,-700,30,-700,100,-700,96,-700,12,-700,9,-700,95,-700,29,-700,83,-700,82,-700,2,-700,81,-700,80,-700,79,-700,78,-700,6,-700,50,-700,57,-700,137,-700,139,-700,77,-700,75,-700,44,-700,39,-700,18,-700,19,-700,140,-700,142,-700,141,-700,150,-700,152,-700,151,-700,56,-700,87,-700,37,-700,22,-700,93,-700,53,-700,32,-700,54,-700,98,-700,46,-700,33,-700,52,-700,59,-700,74,-700,72,-700,35,-700,70,-700,71,-700});
    states[560] = new State(new int[]{14,565,140,150,142,151,141,153,150,155,152,156,151,157,52,567,139,23,82,25,83,26,77,28,75,29,11,286,8,584},new int[]{-340,561,-338,597,-14,566,-151,147,-153,148,-152,152,-15,154,-327,575,-270,576,-167,163,-132,198,-136,24,-137,27,-330,582,-331,583});
    states[561] = new State(new int[]{9,562,10,563,96,580});
    states[562] = new State(-616);
    states[563] = new State(new int[]{14,565,140,150,142,151,141,153,150,155,152,156,151,157,52,567,139,23,82,25,83,26,77,28,75,29,11,286,8,584},new int[]{-338,564,-14,566,-151,147,-153,148,-152,152,-15,154,-327,575,-270,576,-167,163,-132,198,-136,24,-137,27,-330,582,-331,583});
    states[564] = new State(-654);
    states[565] = new State(-656);
    states[566] = new State(-657);
    states[567] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,568,-136,24,-137,27});
    states[568] = new State(new int[]{5,569,9,-659,10,-659,96,-659});
    states[569] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,570,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[570] = new State(-658);
    states[571] = new State(new int[]{8,545,5,-460},new int[]{-113,572});
    states[572] = new State(new int[]{5,573});
    states[573] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,574,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[574] = new State(-280);
    states[575] = new State(-660);
    states[576] = new State(new int[]{8,577});
    states[577] = new State(new int[]{14,565,140,150,142,151,141,153,150,155,152,156,151,157,52,567,139,23,82,25,83,26,77,28,75,29,11,286,8,584},new int[]{-340,578,-338,597,-14,566,-151,147,-153,148,-152,152,-15,154,-327,575,-270,576,-167,163,-132,198,-136,24,-137,27,-330,582,-331,583});
    states[578] = new State(new int[]{9,579,10,563,96,580});
    states[579] = new State(-619);
    states[580] = new State(new int[]{14,565,140,150,142,151,141,153,150,155,152,156,151,157,52,567,139,23,82,25,83,26,77,28,75,29,11,286,8,584},new int[]{-338,581,-14,566,-151,147,-153,148,-152,152,-15,154,-327,575,-270,576,-167,163,-132,198,-136,24,-137,27,-330,582,-331,583});
    states[581] = new State(-655);
    states[582] = new State(-661);
    states[583] = new State(-662);
    states[584] = new State(new int[]{14,589,140,150,142,151,141,153,150,155,152,156,151,157,52,591,139,23,82,25,83,26,77,28,75,29,11,286,8,584},new int[]{-343,585,-333,596,-14,590,-151,147,-153,148,-152,152,-15,154,-329,593,-270,297,-167,163,-132,198,-136,24,-137,27,-330,594,-331,595});
    states[585] = new State(new int[]{9,586,96,587});
    states[586] = new State(-641);
    states[587] = new State(new int[]{14,589,140,150,142,151,141,153,150,155,152,156,151,157,52,591,139,23,82,25,83,26,77,28,75,29,11,286,8,584},new int[]{-333,588,-14,590,-151,147,-153,148,-152,152,-15,154,-329,593,-270,297,-167,163,-132,198,-136,24,-137,27,-330,594,-331,595});
    states[588] = new State(-649);
    states[589] = new State(-642);
    states[590] = new State(-643);
    states[591] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,592,-136,24,-137,27});
    states[592] = new State(-644);
    states[593] = new State(-645);
    states[594] = new State(-646);
    states[595] = new State(-647);
    states[596] = new State(-648);
    states[597] = new State(-653);
    states[598] = new State(-701);
    states[599] = new State(-589);
    states[600] = new State(-587);
    states[601] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,-673,88,-673,10,-673,94,-673,97,-673,30,-673,100,-673,96,-673,12,-673,9,-673,95,-673,29,-673,2,-673,81,-673,80,-673,79,-673,78,-673,6,-673},new int[]{-102,602,-93,606,-76,260,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,605,-253,598});
    states[602] = new State(new int[]{5,603,88,-677,10,-677,94,-677,97,-677,30,-677,100,-677,96,-677,12,-677,9,-677,95,-677,29,-677,83,-677,82,-677,2,-677,81,-677,80,-677,79,-677,78,-677,6,-677});
    states[603] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-93,604,-76,260,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,605,-253,598});
    states[604] = new State(new int[]{112,255,111,256,124,257,125,258,122,259,88,-679,10,-679,94,-679,97,-679,30,-679,100,-679,96,-679,12,-679,9,-679,95,-679,29,-679,83,-679,82,-679,2,-679,81,-679,80,-679,79,-679,78,-679,6,-679},new int[]{-184,138});
    states[605] = new State(-700);
    states[606] = new State(new int[]{112,255,111,256,124,257,125,258,122,259,5,-672,88,-672,10,-672,94,-672,97,-672,30,-672,100,-672,96,-672,12,-672,9,-672,95,-672,29,-672,83,-672,82,-672,2,-672,81,-672,80,-672,79,-672,78,-672,6,-672},new int[]{-184,138});
    states[607] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-120,608,-132,609,-136,24,-137,27});
    states[608] = new State(-474);
    states[609] = new State(-475);
    states[610] = new State(-473);
    states[611] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-145,612,-120,610,-132,609,-136,24,-137,27});
    states[612] = new State(new int[]{5,613,96,607});
    states[613] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,614,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[614] = new State(new int[]{106,615,9,-467,10,-467});
    states[615] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,616,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[616] = new State(-471);
    states[617] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-145,618,-120,610,-132,609,-136,24,-137,27});
    states[618] = new State(new int[]{5,619,96,607});
    states[619] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,620,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[620] = new State(new int[]{106,621,9,-468,10,-468});
    states[621] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,622,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[622] = new State(-472);
    states[623] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-145,624,-120,610,-132,609,-136,24,-137,27});
    states[624] = new State(new int[]{5,625,96,607});
    states[625] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,626,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[626] = new State(-469);
    states[627] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-237,628,-8,995,-9,632,-167,633,-132,990,-136,24,-137,27,-289,993});
    states[628] = new State(new int[]{12,629,96,630});
    states[629] = new State(-198);
    states[630] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-8,631,-9,632,-167,633,-132,990,-136,24,-137,27,-289,993});
    states[631] = new State(-200);
    states[632] = new State(-201);
    states[633] = new State(new int[]{7,164,8,636,119,170,12,-612,96,-612},new int[]{-65,634,-285,635});
    states[634] = new State(-738);
    states[635] = new State(-225);
    states[636] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,5,601,34,641,43,646,9,-761},new int[]{-63,637,-66,434,-82,495,-81,127,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600,-309,639,-310,640});
    states[637] = new State(new int[]{9,638});
    states[638] = new State(-613);
    states[639] = new State(-585);
    states[640] = new State(-916);
    states[641] = new State(new int[]{8,980,5,524,123,-929},new int[]{-311,642});
    states[642] = new State(new int[]{123,643});
    states[643] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,644,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[644] = new State(-920);
    states[645] = new State(-591);
    states[646] = new State(new int[]{123,647,8,971});
    states[647] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,29,44,444,39,474,8,650,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-316,648,-199,649,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-4,670,-317,671,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[648] = new State(-923);
    states[649] = new State(-947);
    states[650] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,477,-91,510,-99,651,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[651] = new State(new int[]{96,652,17,420,8,431,7,659,138,661,4,662,15,667,134,-734,132,-734,114,-734,113,-734,127,-734,128,-734,129,-734,130,-734,126,-734,5,-734,112,-734,111,-734,124,-734,125,-734,122,-734,116,-734,121,-734,119,-734,117,-734,120,-734,118,-734,133,-734,16,-734,13,-734,9,-734,115,-734,11,-744});
    states[652] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444,39,474,8,509,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157},new int[]{-323,653,-99,666,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665});
    states[653] = new State(new int[]{9,654,96,657});
    states[654] = new State(new int[]{106,468,107,469,108,470,109,471,110,472},new int[]{-181,655});
    states[655] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,656,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[656] = new State(-513);
    states[657] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444,39,474,8,509,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157},new int[]{-99,658,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665});
    states[658] = new State(new int[]{17,420,8,431,7,659,138,661,4,662,9,-515,96,-515,11,-744});
    states[659] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35,68,36,63,37,124,38,19,39,18,40,62,41,20,42,125,43,126,44,127,45,128,46,129,47,130,48,131,49,132,50,133,51,134,52,21,53,73,54,87,55,22,56,23,57,26,58,27,59,28,60,71,61,95,62,29,63,88,64,30,65,31,66,24,67,100,68,97,69,32,70,33,71,34,72,37,73,38,74,39,75,99,76,40,77,43,78,45,79,46,80,47,81,93,82,48,83,98,84,49,85,25,86,50,87,70,88,94,89,51,90,52,91,53,92,54,93,55,94,56,95,57,96,58,97,60,98,101,99,102,100,105,101,103,102,104,103,61,104,74,105,35,106,36,107,42,108,44,444},new int[]{-133,660,-132,499,-136,24,-137,27,-279,500,-135,31,-178,501});
    states[660] = new State(-756);
    states[661] = new State(-758);
    states[662] = new State(new int[]{119,170,11,208},new int[]{-287,663,-285,169,-288,207});
    states[663] = new State(-759);
    states[664] = new State(new int[]{7,145,11,-745});
    states[665] = new State(new int[]{7,497});
    states[666] = new State(new int[]{17,420,8,431,7,659,138,661,4,662,9,-514,96,-514,11,-744});
    states[667] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444,39,474,8,509,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157},new int[]{-99,668,-103,669,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665});
    states[668] = new State(new int[]{17,420,8,431,7,659,138,661,4,662,15,667,106,-731,107,-731,108,-731,109,-731,110,-731,88,-731,10,-731,94,-731,97,-731,30,-731,100,-731,134,-731,132,-731,114,-731,113,-731,127,-731,128,-731,129,-731,130,-731,126,-731,5,-731,112,-731,111,-731,124,-731,125,-731,122,-731,116,-731,121,-731,119,-731,117,-731,120,-731,118,-731,133,-731,16,-731,13,-731,96,-731,12,-731,9,-731,95,-731,29,-731,83,-731,82,-731,2,-731,81,-731,80,-731,79,-731,78,-731,115,-731,6,-731,50,-731,57,-731,137,-731,139,-731,77,-731,75,-731,44,-731,39,-731,18,-731,19,-731,140,-731,142,-731,141,-731,150,-731,152,-731,151,-731,56,-731,87,-731,37,-731,22,-731,93,-731,53,-731,32,-731,54,-731,98,-731,46,-731,33,-731,52,-731,59,-731,74,-731,72,-731,35,-731,70,-731,71,-731,11,-744});
    states[669] = new State(-732);
    states[670] = new State(-948);
    states[671] = new State(-949);
    states[672] = new State(-933);
    states[673] = new State(-934);
    states[674] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,675,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[675] = new State(new int[]{50,676,13,129});
    states[676] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484,96,-484,12,-484,9,-484,95,-484,29,-484,2,-484,81,-484,80,-484,79,-484,78,-484},new int[]{-246,677,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[677] = new State(new int[]{29,678,88,-523,10,-523,94,-523,97,-523,30,-523,100,-523,96,-523,12,-523,9,-523,95,-523,83,-523,82,-523,2,-523,81,-523,80,-523,79,-523,78,-523});
    states[678] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484,96,-484,12,-484,9,-484,95,-484,29,-484,2,-484,81,-484,80,-484,79,-484,78,-484},new int[]{-246,679,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[679] = new State(-524);
    states[680] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,88,-562,10,-562,94,-562,97,-562,30,-562,100,-562,96,-562,12,-562,9,-562,95,-562,29,-562,2,-562,81,-562,80,-562,79,-562,78,-562},new int[]{-132,475,-136,24,-137,27});
    states[681] = new State(new int[]{52,682,55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,477,-91,510,-99,651,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[682] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,683,-136,24,-137,27});
    states[683] = new State(new int[]{96,684});
    states[684] = new State(new int[]{52,692},new int[]{-324,685});
    states[685] = new State(new int[]{9,686,96,689});
    states[686] = new State(new int[]{106,687});
    states[687] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,688,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[688] = new State(-510);
    states[689] = new State(new int[]{52,690});
    states[690] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,691,-136,24,-137,27});
    states[691] = new State(-517);
    states[692] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,693,-136,24,-137,27});
    states[693] = new State(-516);
    states[694] = new State(-486);
    states[695] = new State(-487);
    states[696] = new State(new int[]{150,698,139,23,82,25,83,26,77,28,75,29},new int[]{-128,697,-132,699,-136,24,-137,27});
    states[697] = new State(-519);
    states[698] = new State(-92);
    states[699] = new State(-93);
    states[700] = new State(-488);
    states[701] = new State(-489);
    states[702] = new State(-490);
    states[703] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,704,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[704] = new State(new int[]{57,705,13,129});
    states[705] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363,29,713,88,-542},new int[]{-33,706,-239,968,-248,970,-68,961,-98,967,-86,966,-83,188,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[706] = new State(new int[]{10,709,29,713,88,-542},new int[]{-239,707});
    states[707] = new State(new int[]{88,708});
    states[708] = new State(-533);
    states[709] = new State(new int[]{29,713,139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363,88,-542},new int[]{-239,710,-248,712,-68,961,-98,967,-86,966,-83,188,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[710] = new State(new int[]{88,711});
    states[711] = new State(-534);
    states[712] = new State(-537);
    states[713] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484,88,-484},new int[]{-238,714,-247,715,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[714] = new State(new int[]{10,120,88,-543});
    states[715] = new State(-521);
    states[716] = new State(new int[]{17,-746,8,-746,7,-746,138,-746,4,-746,15,-746,106,-746,107,-746,108,-746,109,-746,110,-746,88,-746,10,-746,11,-746,94,-746,97,-746,30,-746,100,-746,5,-93});
    states[717] = new State(new int[]{7,-176,11,-176,5,-92});
    states[718] = new State(-491);
    states[719] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,94,-484,10,-484},new int[]{-238,720,-247,715,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[720] = new State(new int[]{94,721,10,120});
    states[721] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,722,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[722] = new State(-544);
    states[723] = new State(-492);
    states[724] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,725,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[725] = new State(new int[]{13,129,95,953,137,-547,139,-547,82,-547,83,-547,77,-547,75,-547,44,-547,39,-547,8,-547,18,-547,19,-547,140,-547,142,-547,141,-547,150,-547,152,-547,151,-547,56,-547,87,-547,37,-547,22,-547,93,-547,53,-547,32,-547,54,-547,98,-547,46,-547,33,-547,52,-547,59,-547,74,-547,72,-547,35,-547,88,-547,10,-547,94,-547,97,-547,30,-547,100,-547,96,-547,12,-547,9,-547,29,-547,2,-547,81,-547,80,-547,79,-547,78,-547},new int[]{-278,726});
    states[726] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484,96,-484,12,-484,9,-484,95,-484,29,-484,2,-484,81,-484,80,-484,79,-484,78,-484},new int[]{-246,727,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[727] = new State(-545);
    states[728] = new State(-493);
    states[729] = new State(new int[]{52,960,139,-556,82,-556,83,-556,77,-556,75,-556},new int[]{-18,730});
    states[730] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,731,-136,24,-137,27});
    states[731] = new State(new int[]{106,956,5,957},new int[]{-272,732});
    states[732] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,733,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[733] = new State(new int[]{13,129,70,954,71,955},new int[]{-104,734});
    states[734] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,735,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[735] = new State(new int[]{13,129,95,953,137,-547,139,-547,82,-547,83,-547,77,-547,75,-547,44,-547,39,-547,8,-547,18,-547,19,-547,140,-547,142,-547,141,-547,150,-547,152,-547,151,-547,56,-547,87,-547,37,-547,22,-547,93,-547,53,-547,32,-547,54,-547,98,-547,46,-547,33,-547,52,-547,59,-547,74,-547,72,-547,35,-547,88,-547,10,-547,94,-547,97,-547,30,-547,100,-547,96,-547,12,-547,9,-547,29,-547,2,-547,81,-547,80,-547,79,-547,78,-547},new int[]{-278,736});
    states[736] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484,96,-484,12,-484,9,-484,95,-484,29,-484,2,-484,81,-484,80,-484,79,-484,78,-484},new int[]{-246,737,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[737] = new State(-554);
    states[738] = new State(-494);
    states[739] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,5,601,34,641,43,646},new int[]{-66,740,-82,495,-81,127,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600,-309,639,-310,640});
    states[740] = new State(new int[]{95,741,96,435});
    states[741] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484,96,-484,12,-484,9,-484,95,-484,29,-484,2,-484,81,-484,80,-484,79,-484,78,-484},new int[]{-246,742,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[742] = new State(-561);
    states[743] = new State(-495);
    states[744] = new State(-496);
    states[745] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484,97,-484,30,-484},new int[]{-238,746,-247,715,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[746] = new State(new int[]{10,120,97,748,30,931},new int[]{-276,747});
    states[747] = new State(-563);
    states[748] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484},new int[]{-238,749,-247,715,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[749] = new State(new int[]{88,750,10,120});
    states[750] = new State(-564);
    states[751] = new State(-497);
    states[752] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601,88,-578,10,-578,94,-578,97,-578,30,-578,100,-578,96,-578,12,-578,9,-578,95,-578,29,-578,2,-578,81,-578,80,-578,79,-578,78,-578},new int[]{-81,753,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[753] = new State(-579);
    states[754] = new State(-498);
    states[755] = new State(new int[]{52,916,139,23,82,25,83,26,77,28,75,29},new int[]{-132,756,-136,24,-137,27});
    states[756] = new State(new int[]{5,914,133,-553},new int[]{-260,757});
    states[757] = new State(new int[]{133,758});
    states[758] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,759,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[759] = new State(new int[]{95,760,13,129});
    states[760] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484,96,-484,12,-484,9,-484,95,-484,29,-484,2,-484,81,-484,80,-484,79,-484,78,-484},new int[]{-246,761,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[761] = new State(-549);
    states[762] = new State(-499);
    states[763] = new State(new int[]{8,765,139,23,82,25,83,26,77,28,75,29},new int[]{-298,764,-144,773,-132,772,-136,24,-137,27});
    states[764] = new State(-509);
    states[765] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,766,-136,24,-137,27});
    states[766] = new State(new int[]{96,767});
    states[767] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-144,768,-132,772,-136,24,-137,27});
    states[768] = new State(new int[]{9,769,96,522});
    states[769] = new State(new int[]{106,770});
    states[770] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,771,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[771] = new State(-511);
    states[772] = new State(-338);
    states[773] = new State(new int[]{5,774,96,522,106,912});
    states[774] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,775,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[775] = new State(new int[]{106,910,116,911,88,-404,10,-404,94,-404,97,-404,30,-404,100,-404,96,-404,12,-404,9,-404,95,-404,29,-404,83,-404,82,-404,2,-404,81,-404,80,-404,79,-404,78,-404},new int[]{-325,776});
    states[776] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,881,131,873,112,362,111,363,62,159,34,641,43,646},new int[]{-80,777,-79,778,-78,332,-83,333,-75,193,-12,314,-10,317,-13,203,-132,779,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880,-87,898,-229,899,-53,900,-310,909});
    states[777] = new State(-406);
    states[778] = new State(-407);
    states[779] = new State(new int[]{123,780,4,-155,11,-155,7,-155,138,-155,8,-155,115,-155,132,-155,134,-155,114,-155,113,-155,127,-155,128,-155,129,-155,130,-155,126,-155,112,-155,111,-155,124,-155,125,-155,116,-155,121,-155,119,-155,117,-155,120,-155,118,-155,133,-155,13,-155,88,-155,10,-155,94,-155,97,-155,30,-155,100,-155,96,-155,12,-155,9,-155,95,-155,29,-155,83,-155,82,-155,2,-155,81,-155,80,-155,79,-155,78,-155});
    states[780] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,781,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[781] = new State(-409);
    states[782] = new State(-946);
    states[783] = new State(-935);
    states[784] = new State(-936);
    states[785] = new State(-937);
    states[786] = new State(-938);
    states[787] = new State(-939);
    states[788] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,789,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[789] = new State(new int[]{95,790,13,129});
    states[790] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484,96,-484,12,-484,9,-484,95,-484,29,-484,2,-484,81,-484,80,-484,79,-484,78,-484},new int[]{-246,791,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[791] = new State(-506);
    states[792] = new State(-500);
    states[793] = new State(-582);
    states[794] = new State(-583);
    states[795] = new State(-501);
    states[796] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,797,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[797] = new State(new int[]{95,798,13,129});
    states[798] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484,96,-484,12,-484,9,-484,95,-484,29,-484,2,-484,81,-484,80,-484,79,-484,78,-484},new int[]{-246,799,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[799] = new State(-548);
    states[800] = new State(-502);
    states[801] = new State(new int[]{73,803,55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,802,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[802] = new State(new int[]{13,129,88,-507,10,-507,94,-507,97,-507,30,-507,100,-507,96,-507,12,-507,9,-507,95,-507,29,-507,83,-507,82,-507,2,-507,81,-507,80,-507,79,-507,78,-507});
    states[803] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,804,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[804] = new State(new int[]{13,129,88,-508,10,-508,94,-508,97,-508,30,-508,100,-508,96,-508,12,-508,9,-508,95,-508,29,-508,83,-508,82,-508,2,-508,81,-508,80,-508,79,-508,78,-508});
    states[805] = new State(-503);
    states[806] = new State(-504);
    states[807] = new State(-505);
    states[808] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,809,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[809] = new State(new int[]{54,810,13,129});
    states[810] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,140,150,142,151,141,153,150,155,152,156,151,157,55,845,18,339,19,344,11,286,8,584},new int[]{-337,811,-336,855,-329,818,-270,823,-167,163,-132,198,-136,24,-137,27,-328,837,-344,840,-326,848,-14,843,-151,147,-153,148,-152,152,-15,154,-243,846,-281,847,-330,849,-331,852});
    states[811] = new State(new int[]{10,814,29,713,88,-542},new int[]{-239,812});
    states[812] = new State(new int[]{88,813});
    states[813] = new State(-525);
    states[814] = new State(new int[]{29,713,139,23,82,25,83,26,77,28,75,29,140,150,142,151,141,153,150,155,152,156,151,157,55,845,18,339,19,344,11,286,8,584,88,-542},new int[]{-239,815,-336,817,-329,818,-270,823,-167,163,-132,198,-136,24,-137,27,-328,837,-344,840,-326,848,-14,843,-151,147,-153,148,-152,152,-15,154,-243,846,-281,847,-330,849,-331,852});
    states[815] = new State(new int[]{88,816});
    states[816] = new State(-526);
    states[817] = new State(-528);
    states[818] = new State(new int[]{36,819});
    states[819] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,820,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[820] = new State(new int[]{5,821,13,129});
    states[821] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484,29,-484,88,-484},new int[]{-246,822,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[822] = new State(-529);
    states[823] = new State(new int[]{8,824,96,-626,5,-626});
    states[824] = new State(new int[]{14,303,140,150,142,151,141,153,150,155,152,156,151,157,139,23,82,25,83,26,77,28,75,29,52,829,11,286,8,584},new int[]{-341,825,-339,836,-14,304,-151,147,-153,148,-152,152,-15,154,-132,305,-136,24,-137,27,-329,833,-270,297,-167,163,-330,834,-331,835});
    states[825] = new State(new int[]{9,826,10,301,96,827});
    states[826] = new State(new int[]{36,-620,5,-621});
    states[827] = new State(new int[]{14,303,140,150,142,151,141,153,150,155,152,156,151,157,139,23,82,25,83,26,77,28,75,29,52,829,11,286,8,584},new int[]{-339,828,-14,304,-151,147,-153,148,-152,152,-15,154,-132,305,-136,24,-137,27,-329,833,-270,297,-167,163,-330,834,-331,835});
    states[828] = new State(-652);
    states[829] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,830,-136,24,-137,27});
    states[830] = new State(new int[]{5,831,9,-668,10,-668,96,-668});
    states[831] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,832,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[832] = new State(-667);
    states[833] = new State(-669);
    states[834] = new State(-670);
    states[835] = new State(-671);
    states[836] = new State(-650);
    states[837] = new State(new int[]{5,838});
    states[838] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484,29,-484,88,-484},new int[]{-246,839,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[839] = new State(-530);
    states[840] = new State(new int[]{96,841,5,-622});
    states[841] = new State(new int[]{140,150,142,151,141,153,150,155,152,156,151,157,139,23,82,25,83,26,77,28,75,29,55,845,18,339,19,344},new int[]{-326,842,-14,843,-151,147,-153,148,-152,152,-15,154,-270,844,-167,163,-132,198,-136,24,-137,27,-243,846,-281,847});
    states[842] = new State(-624);
    states[843] = new State(-625);
    states[844] = new State(-626);
    states[845] = new State(-627);
    states[846] = new State(-628);
    states[847] = new State(-629);
    states[848] = new State(-623);
    states[849] = new State(new int[]{5,850});
    states[850] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484,29,-484,88,-484},new int[]{-246,851,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[851] = new State(-531);
    states[852] = new State(new int[]{5,853});
    states[853] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484,29,-484,88,-484},new int[]{-246,854,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[854] = new State(-532);
    states[855] = new State(-527);
    states[856] = new State(-940);
    states[857] = new State(-941);
    states[858] = new State(-942);
    states[859] = new State(-943);
    states[860] = new State(-944);
    states[861] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,802,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[862] = new State(-145);
    states[863] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363,12,-171},new int[]{-69,864,-67,185,-86,866,-83,188,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[864] = new State(new int[]{12,865});
    states[865] = new State(-152);
    states[866] = new State(-172);
    states[867] = new State(-146);
    states[868] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-10,869,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877});
    states[869] = new State(-147);
    states[870] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-83,871,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[871] = new State(new int[]{9,872,13,189});
    states[872] = new State(-148);
    states[873] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-10,874,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877});
    states[874] = new State(-149);
    states[875] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-10,876,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877});
    states[876] = new State(-150);
    states[877] = new State(-151);
    states[878] = new State(-133);
    states[879] = new State(-134);
    states[880] = new State(-115);
    states[881] = new State(new int[]{9,889,139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,894,131,873,112,362,111,363,62,159},new int[]{-83,882,-62,883,-231,887,-75,193,-12,314,-10,317,-13,203,-132,893,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880,-61,329,-79,897,-78,332,-87,898,-229,899,-53,900,-230,901,-232,908,-121,904});
    states[882] = new State(new int[]{9,872,13,189,96,-179});
    states[883] = new State(new int[]{9,884});
    states[884] = new State(new int[]{123,885,88,-182,10,-182,94,-182,97,-182,30,-182,100,-182,96,-182,12,-182,9,-182,95,-182,29,-182,83,-182,82,-182,2,-182,81,-182,80,-182,79,-182,78,-182});
    states[885] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,886,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[886] = new State(-411);
    states[887] = new State(new int[]{9,888});
    states[888] = new State(-187);
    states[889] = new State(new int[]{5,524,123,-929},new int[]{-311,890});
    states[890] = new State(new int[]{123,891});
    states[891] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,892,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[892] = new State(-410);
    states[893] = new State(new int[]{4,-155,11,-155,7,-155,138,-155,8,-155,115,-155,132,-155,134,-155,114,-155,113,-155,127,-155,128,-155,129,-155,130,-155,126,-155,112,-155,111,-155,124,-155,125,-155,116,-155,121,-155,119,-155,117,-155,120,-155,118,-155,133,-155,9,-155,13,-155,96,-155,5,-193});
    states[894] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,894,131,873,112,362,111,363,62,159,9,-183},new int[]{-83,882,-62,895,-231,887,-75,193,-12,314,-10,317,-13,203,-132,893,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880,-61,329,-79,897,-78,332,-87,898,-229,899,-53,900,-230,901,-232,908,-121,904});
    states[895] = new State(new int[]{9,896});
    states[896] = new State(-182);
    states[897] = new State(-185);
    states[898] = new State(-180);
    states[899] = new State(-181);
    states[900] = new State(-413);
    states[901] = new State(new int[]{10,902,9,-188});
    states[902] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,9,-189},new int[]{-232,903,-121,904,-132,907,-136,24,-137,27});
    states[903] = new State(-191);
    states[904] = new State(new int[]{5,905});
    states[905] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,894,131,873,112,362,111,363},new int[]{-78,906,-83,333,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880,-87,898,-229,899});
    states[906] = new State(-192);
    states[907] = new State(-193);
    states[908] = new State(-190);
    states[909] = new State(-408);
    states[910] = new State(-402);
    states[911] = new State(-403);
    states[912] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,913,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[913] = new State(-405);
    states[914] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,915,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[915] = new State(-552);
    states[916] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,917,-136,24,-137,27});
    states[917] = new State(new int[]{5,918,133,924});
    states[918] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,919,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[919] = new State(new int[]{133,920});
    states[920] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,921,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[921] = new State(new int[]{95,922,13,129});
    states[922] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484,96,-484,12,-484,9,-484,95,-484,29,-484,2,-484,81,-484,80,-484,79,-484,78,-484},new int[]{-246,923,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[923] = new State(-550);
    states[924] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,925,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[925] = new State(new int[]{95,926,13,129});
    states[926] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484,96,-484,12,-484,9,-484,95,-484,29,-484,2,-484,81,-484,80,-484,79,-484,78,-484},new int[]{-246,927,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[927] = new State(-551);
    states[928] = new State(new int[]{5,929});
    states[929] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484,94,-484,97,-484,30,-484,100,-484},new int[]{-247,930,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[930] = new State(-483);
    states[931] = new State(new int[]{76,939,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484,88,-484},new int[]{-56,932,-59,934,-58,951,-238,952,-247,715,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[932] = new State(new int[]{88,933});
    states[933] = new State(-565);
    states[934] = new State(new int[]{10,936,29,949,88,-571},new int[]{-240,935});
    states[935] = new State(-566);
    states[936] = new State(new int[]{76,939,29,949,88,-571},new int[]{-58,937,-240,938});
    states[937] = new State(-570);
    states[938] = new State(-567);
    states[939] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-60,940,-166,943,-167,944,-132,945,-136,24,-137,27,-125,946});
    states[940] = new State(new int[]{95,941});
    states[941] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484,29,-484,88,-484},new int[]{-246,942,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[942] = new State(-573);
    states[943] = new State(-574);
    states[944] = new State(new int[]{7,164,95,-576});
    states[945] = new State(new int[]{7,-251,95,-251,5,-577});
    states[946] = new State(new int[]{5,947});
    states[947] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-166,948,-167,944,-132,198,-136,24,-137,27});
    states[948] = new State(-575);
    states[949] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484,88,-484},new int[]{-238,950,-247,715,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[950] = new State(new int[]{10,120,88,-572});
    states[951] = new State(-569);
    states[952] = new State(new int[]{10,120,88,-568});
    states[953] = new State(-546);
    states[954] = new State(-559);
    states[955] = new State(-560);
    states[956] = new State(-557);
    states[957] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-167,958,-132,198,-136,24,-137,27});
    states[958] = new State(new int[]{106,959,7,164});
    states[959] = new State(-558);
    states[960] = new State(-555);
    states[961] = new State(new int[]{5,962,96,964});
    states[962] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484,29,-484,88,-484},new int[]{-246,963,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[963] = new State(-538);
    states[964] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-98,965,-86,966,-83,188,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[965] = new State(-540);
    states[966] = new State(-541);
    states[967] = new State(-539);
    states[968] = new State(new int[]{88,969});
    states[969] = new State(-535);
    states[970] = new State(-536);
    states[971] = new State(new int[]{9,972,139,23,82,25,83,26,77,28,75,29},new int[]{-313,975,-314,979,-144,520,-132,772,-136,24,-137,27});
    states[972] = new State(new int[]{123,973});
    states[973] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,29,44,444,39,474,8,650,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-316,974,-199,649,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-4,670,-317,671,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[974] = new State(-924);
    states[975] = new State(new int[]{9,976,10,518});
    states[976] = new State(new int[]{123,977});
    states[977] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,29,44,444,39,474,8,650,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-316,978,-199,649,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-4,670,-317,671,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[978] = new State(-925);
    states[979] = new State(-926);
    states[980] = new State(new int[]{9,981,139,23,82,25,83,26,77,28,75,29},new int[]{-313,985,-314,979,-144,520,-132,772,-136,24,-137,27});
    states[981] = new State(new int[]{5,524,123,-929},new int[]{-311,982});
    states[982] = new State(new int[]{123,983});
    states[983] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,984,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[984] = new State(-921);
    states[985] = new State(new int[]{9,986,10,518});
    states[986] = new State(new int[]{5,524,123,-929},new int[]{-311,987});
    states[987] = new State(new int[]{123,988});
    states[988] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,989,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[989] = new State(-922);
    states[990] = new State(new int[]{5,991,7,-251,8,-251,119,-251,12,-251,96,-251});
    states[991] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-9,992,-167,633,-132,198,-136,24,-137,27,-289,993});
    states[992] = new State(-202);
    states[993] = new State(new int[]{8,636,12,-612,96,-612},new int[]{-65,994});
    states[994] = new State(-739);
    states[995] = new State(-199);
    states[996] = new State(-195);
    states[997] = new State(-463);
    states[998] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,999,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[999] = new State(new int[]{13,129,96,-112,5,-112,10,-112,9,-112});
    states[1000] = new State(new int[]{13,129,96,-111,5,-111,10,-111,9,-111});
    states[1001] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,1005,138,376,21,382,47,390,48,527,31,531,73,535,64,538},new int[]{-263,1002,-258,1003,-85,177,-94,212,-95,220,-167,1004,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-242,1010,-235,1011,-267,1012,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-289,1013});
    states[1002] = new State(-932);
    states[1003] = new State(-477);
    states[1004] = new State(new int[]{7,164,119,170,8,-246,114,-246,113,-246,127,-246,128,-246,129,-246,130,-246,126,-246,6,-246,112,-246,111,-246,124,-246,125,-246,123,-246},new int[]{-285,635});
    states[1005] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-74,1006,-72,236,-262,239,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1006] = new State(new int[]{9,1007,96,1008});
    states[1007] = new State(-241);
    states[1008] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-72,1009,-262,239,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1009] = new State(-254);
    states[1010] = new State(-478);
    states[1011] = new State(-479);
    states[1012] = new State(-480);
    states[1013] = new State(-481);
    states[1014] = new State(new int[]{5,1001,123,-931},new int[]{-312,1015});
    states[1015] = new State(new int[]{123,1016});
    states[1016] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,1017,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[1017] = new State(-911);
    states[1018] = new State(new int[]{5,1019,10,1031,17,-746,8,-746,7,-746,138,-746,4,-746,15,-746,134,-746,132,-746,114,-746,113,-746,127,-746,128,-746,129,-746,130,-746,126,-746,112,-746,111,-746,124,-746,125,-746,122,-746,116,-746,121,-746,119,-746,117,-746,120,-746,118,-746,133,-746,16,-746,96,-746,13,-746,9,-746,115,-746,11,-746});
    states[1019] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,1020,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1020] = new State(new int[]{9,1021,10,1025});
    states[1021] = new State(new int[]{5,1001,123,-931},new int[]{-312,1022});
    states[1022] = new State(new int[]{123,1023});
    states[1023] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,1024,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[1024] = new State(-912);
    states[1025] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-313,1026,-314,979,-144,520,-132,772,-136,24,-137,27});
    states[1026] = new State(new int[]{9,1027,10,518});
    states[1027] = new State(new int[]{5,1001,123,-931},new int[]{-312,1028});
    states[1028] = new State(new int[]{123,1029});
    states[1029] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,1030,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[1030] = new State(-914);
    states[1031] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-313,1032,-314,979,-144,520,-132,772,-136,24,-137,27});
    states[1032] = new State(new int[]{9,1033,10,518});
    states[1033] = new State(new int[]{5,1001,123,-931},new int[]{-312,1034});
    states[1034] = new State(new int[]{123,1035});
    states[1035] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646,87,117,37,674,53,724,93,719,32,729,33,755,72,788,22,703,98,745,59,796,46,752,74,861},new int[]{-315,1036,-92,440,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640,-317,782,-241,672,-139,673,-305,783,-233,784,-109,785,-108,786,-110,787,-32,856,-290,857,-155,858,-234,859,-111,860});
    states[1036] = new State(-913);
    states[1037] = new State(-737);
    states[1038] = new State(new int[]{143,1042,145,1043,146,1044,147,1045,149,1046,148,1047,103,-775,87,-775,58,-775,26,-775,66,-775,49,-775,52,-775,61,-775,11,-775,25,-775,23,-775,43,-775,34,-775,27,-775,28,-775,45,-775,24,-775,88,-775,81,-775,80,-775,79,-775,78,-775,20,-775,144,-775,38,-775},new int[]{-193,1039,-196,1048});
    states[1039] = new State(new int[]{10,1040});
    states[1040] = new State(new int[]{143,1042,145,1043,146,1044,147,1045,149,1046,148,1047,103,-776,87,-776,58,-776,26,-776,66,-776,49,-776,52,-776,61,-776,11,-776,25,-776,23,-776,43,-776,34,-776,27,-776,28,-776,45,-776,24,-776,88,-776,81,-776,80,-776,79,-776,78,-776,20,-776,144,-776,38,-776},new int[]{-196,1041});
    states[1041] = new State(-780);
    states[1042] = new State(-790);
    states[1043] = new State(-791);
    states[1044] = new State(-792);
    states[1045] = new State(-793);
    states[1046] = new State(-794);
    states[1047] = new State(-795);
    states[1048] = new State(-779);
    states[1049] = new State(-369);
    states[1050] = new State(-437);
    states[1051] = new State(-438);
    states[1052] = new State(new int[]{8,-443,106,-443,10,-443,5,-443,7,-440});
    states[1053] = new State(new int[]{119,1055,8,-446,106,-446,10,-446,7,-446,5,-446},new int[]{-141,1054});
    states[1054] = new State(-447);
    states[1055] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-144,1056,-132,772,-136,24,-137,27});
    states[1056] = new State(new int[]{117,1057,96,522});
    states[1057] = new State(-315);
    states[1058] = new State(-448);
    states[1059] = new State(new int[]{119,1055,8,-444,106,-444,10,-444,5,-444},new int[]{-141,1060});
    states[1060] = new State(-445);
    states[1061] = new State(new int[]{7,1062});
    states[1062] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444},new int[]{-127,1063,-134,1064,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059});
    states[1063] = new State(-439);
    states[1064] = new State(-442);
    states[1065] = new State(-441);
    states[1066] = new State(-430);
    states[1067] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35},new int[]{-159,1068,-132,1107,-136,24,-137,27,-135,1108});
    states[1068] = new State(new int[]{7,1092,11,1098,5,-387},new int[]{-220,1069,-225,1095});
    states[1069] = new State(new int[]{82,1081,83,1087,10,-394},new int[]{-189,1070});
    states[1070] = new State(new int[]{10,1071});
    states[1071] = new State(new int[]{62,1076,148,1078,147,1079,143,1080,11,-384,25,-384,23,-384,43,-384,34,-384,27,-384,28,-384,45,-384,24,-384,88,-384,81,-384,80,-384,79,-384,78,-384},new int[]{-192,1072,-197,1073});
    states[1072] = new State(-378);
    states[1073] = new State(new int[]{10,1074});
    states[1074] = new State(new int[]{62,1076,11,-384,25,-384,23,-384,43,-384,34,-384,27,-384,28,-384,45,-384,24,-384,88,-384,81,-384,80,-384,79,-384,78,-384},new int[]{-192,1075});
    states[1075] = new State(-379);
    states[1076] = new State(new int[]{10,1077});
    states[1077] = new State(-385);
    states[1078] = new State(-796);
    states[1079] = new State(-797);
    states[1080] = new State(-798);
    states[1081] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,5,601,34,641,43,646,10,-393},new int[]{-101,1082,-82,1086,-81,127,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600,-309,639,-310,640});
    states[1082] = new State(new int[]{83,1084,10,-397},new int[]{-190,1083});
    states[1083] = new State(-395);
    states[1084] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484},new int[]{-246,1085,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[1085] = new State(-398);
    states[1086] = new State(-392);
    states[1087] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484},new int[]{-246,1088,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[1088] = new State(new int[]{82,1090,10,-399},new int[]{-191,1089});
    states[1089] = new State(-396);
    states[1090] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,5,601,34,641,43,646,10,-393},new int[]{-101,1091,-82,1086,-81,127,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600,-309,639,-310,640});
    states[1091] = new State(-400);
    states[1092] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35},new int[]{-132,1093,-135,1094,-136,24,-137,27});
    states[1093] = new State(-373);
    states[1094] = new State(-374);
    states[1095] = new State(new int[]{5,1096});
    states[1096] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,1097,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1097] = new State(-386);
    states[1098] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-224,1099,-223,1106,-144,1103,-132,772,-136,24,-137,27});
    states[1099] = new State(new int[]{12,1100,10,1101});
    states[1100] = new State(-388);
    states[1101] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-223,1102,-144,1103,-132,772,-136,24,-137,27});
    states[1102] = new State(-390);
    states[1103] = new State(new int[]{5,1104,96,522});
    states[1104] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,1105,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1105] = new State(-391);
    states[1106] = new State(-389);
    states[1107] = new State(-371);
    states[1108] = new State(-372);
    states[1109] = new State(new int[]{45,1110});
    states[1110] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35},new int[]{-159,1111,-132,1107,-136,24,-137,27,-135,1108});
    states[1111] = new State(new int[]{7,1092,11,1098,5,-387},new int[]{-220,1112,-225,1095});
    states[1112] = new State(new int[]{106,1115,10,-383},new int[]{-198,1113});
    states[1113] = new State(new int[]{10,1114});
    states[1114] = new State(-381);
    states[1115] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,1116,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[1116] = new State(-382);
    states[1117] = new State(new int[]{103,1252,11,-363,25,-363,23,-363,43,-363,34,-363,27,-363,28,-363,45,-363,24,-363,88,-363,81,-363,80,-363,79,-363,78,-363,58,-63,26,-63,66,-63,49,-63,52,-63,61,-63,87,-63},new int[]{-163,1118,-40,1119,-36,1122,-57,1251});
    states[1118] = new State(-431);
    states[1119] = new State(new int[]{87,117},new int[]{-241,1120});
    states[1120] = new State(new int[]{10,1121});
    states[1121] = new State(-458);
    states[1122] = new State(new int[]{58,1125,26,1146,66,1150,49,1324,52,1339,61,1341,87,-62},new int[]{-42,1123,-154,1124,-26,1131,-48,1148,-275,1152,-296,1326});
    states[1123] = new State(-64);
    states[1124] = new State(-80);
    states[1125] = new State(new int[]{150,698,139,23,82,25,83,26,77,28,75,29},new int[]{-142,1126,-128,1130,-132,699,-136,24,-137,27});
    states[1126] = new State(new int[]{10,1127,96,1128});
    states[1127] = new State(-89);
    states[1128] = new State(new int[]{150,698,139,23,82,25,83,26,77,28,75,29},new int[]{-128,1129,-132,699,-136,24,-137,27});
    states[1129] = new State(-91);
    states[1130] = new State(-90);
    states[1131] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,58,-81,26,-81,66,-81,49,-81,52,-81,61,-81,87,-81},new int[]{-24,1132,-25,1133,-126,1135,-132,1145,-136,24,-137,27});
    states[1132] = new State(-95);
    states[1133] = new State(new int[]{10,1134});
    states[1134] = new State(-105);
    states[1135] = new State(new int[]{116,1136,5,1141});
    states[1136] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,1139,131,873,112,362,111,363},new int[]{-97,1137,-83,1138,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880,-87,1140});
    states[1137] = new State(-106);
    states[1138] = new State(new int[]{13,189,10,-108,88,-108,81,-108,80,-108,79,-108,78,-108});
    states[1139] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,894,131,873,112,362,111,363,62,159,9,-183},new int[]{-83,882,-62,895,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880,-61,329,-79,897,-78,332,-87,898,-229,899,-53,900});
    states[1140] = new State(-109);
    states[1141] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,1142,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1142] = new State(new int[]{116,1143});
    states[1143] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,894,131,873,112,362,111,363},new int[]{-78,1144,-83,333,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880,-87,898,-229,899});
    states[1144] = new State(-107);
    states[1145] = new State(-110);
    states[1146] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-24,1147,-25,1133,-126,1135,-132,1145,-136,24,-137,27});
    states[1147] = new State(-94);
    states[1148] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,58,-82,26,-82,66,-82,49,-82,52,-82,61,-82,87,-82},new int[]{-24,1149,-25,1133,-126,1135,-132,1145,-136,24,-137,27});
    states[1149] = new State(-97);
    states[1150] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-24,1151,-25,1133,-126,1135,-132,1145,-136,24,-137,27});
    states[1151] = new State(-96);
    states[1152] = new State(new int[]{11,627,58,-83,26,-83,66,-83,49,-83,52,-83,61,-83,87,-83,139,-197,82,-197,83,-197,77,-197,75,-197},new int[]{-45,1153,-6,1154,-236,996});
    states[1153] = new State(-99);
    states[1154] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,11,627},new int[]{-46,1155,-236,406,-129,1156,-132,1307,-136,24,-137,27,-130,1312,-138,1315,-167,1220});
    states[1155] = new State(-194);
    states[1156] = new State(new int[]{116,1157});
    states[1157] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571,68,1301,69,1302,143,1303,24,1304,25,1305,23,-296,40,-296,63,-296},new int[]{-273,1158,-262,1160,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542,-27,1161,-20,1162,-21,1299,-19,1306});
    states[1158] = new State(new int[]{10,1159});
    states[1159] = new State(-203);
    states[1160] = new State(-214);
    states[1161] = new State(-215);
    states[1162] = new State(new int[]{23,1293,40,1294,63,1295},new int[]{-277,1163});
    states[1163] = new State(new int[]{8,1204,20,-308,11,-308,88,-308,81,-308,80,-308,79,-308,78,-308,26,-308,139,-308,82,-308,83,-308,77,-308,75,-308,61,-308,25,-308,23,-308,43,-308,34,-308,27,-308,28,-308,45,-308,24,-308,10,-308},new int[]{-170,1164});
    states[1164] = new State(new int[]{20,1195,11,-316,88,-316,81,-316,80,-316,79,-316,78,-316,26,-316,139,-316,82,-316,83,-316,77,-316,75,-316,61,-316,25,-316,23,-316,43,-316,34,-316,27,-316,28,-316,45,-316,24,-316,10,-316},new int[]{-304,1165,-303,1193,-302,1221});
    states[1165] = new State(new int[]{11,627,10,-306,88,-334,81,-334,80,-334,79,-334,78,-334,26,-197,139,-197,82,-197,83,-197,77,-197,75,-197,61,-197,25,-197,23,-197,43,-197,34,-197,27,-197,28,-197,45,-197,24,-197},new int[]{-23,1166,-22,1167,-29,1173,-31,397,-41,1174,-6,1175,-236,996,-30,1290,-50,1292,-49,403,-51,1291});
    states[1166] = new State(-289);
    states[1167] = new State(new int[]{88,1168,81,1169,80,1170,79,1171,78,1172},new int[]{-7,395});
    states[1168] = new State(-307);
    states[1169] = new State(-330);
    states[1170] = new State(-331);
    states[1171] = new State(-332);
    states[1172] = new State(-333);
    states[1173] = new State(-328);
    states[1174] = new State(-342);
    states[1175] = new State(new int[]{26,1177,139,23,82,25,83,26,77,28,75,29,61,1181,25,1246,23,1247,11,627,43,1188,34,1229,27,1261,28,1268,45,1275,24,1284},new int[]{-47,1176,-236,406,-209,405,-206,407,-244,408,-299,1179,-298,1180,-144,773,-132,772,-136,24,-137,27,-3,1185,-217,1248,-215,1117,-212,1187,-216,1228,-214,1249,-202,1272,-203,1273,-205,1274});
    states[1176] = new State(-344);
    states[1177] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-25,1178,-126,1135,-132,1145,-136,24,-137,27});
    states[1178] = new State(-349);
    states[1179] = new State(-350);
    states[1180] = new State(-354);
    states[1181] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-144,1182,-132,772,-136,24,-137,27});
    states[1182] = new State(new int[]{5,1183,96,522});
    states[1183] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,1184,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1184] = new State(-355);
    states[1185] = new State(new int[]{27,411,45,1067,24,1109,139,23,82,25,83,26,77,28,75,29,61,1181,43,1188,34,1229},new int[]{-299,1186,-217,410,-203,1066,-298,1180,-144,773,-132,772,-136,24,-137,27,-215,1117,-212,1187,-216,1228});
    states[1186] = new State(-351);
    states[1187] = new State(-364);
    states[1188] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444},new int[]{-157,1189,-156,1050,-127,1051,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059,-321,1061,-134,1065});
    states[1189] = new State(new int[]{8,545,10,-460,106,-460},new int[]{-113,1190});
    states[1190] = new State(new int[]{10,1226,106,-777},new int[]{-194,1191,-195,1222});
    states[1191] = new State(new int[]{20,1195,103,-316,87,-316,58,-316,26,-316,66,-316,49,-316,52,-316,61,-316,11,-316,25,-316,23,-316,43,-316,34,-316,27,-316,28,-316,45,-316,24,-316,88,-316,81,-316,80,-316,79,-316,78,-316,144,-316,38,-316},new int[]{-304,1192,-303,1193,-302,1221});
    states[1192] = new State(-449);
    states[1193] = new State(new int[]{20,1195,11,-317,88,-317,81,-317,80,-317,79,-317,78,-317,26,-317,139,-317,82,-317,83,-317,77,-317,75,-317,61,-317,25,-317,23,-317,43,-317,34,-317,27,-317,28,-317,45,-317,24,-317,10,-317,103,-317,87,-317,58,-317,66,-317,49,-317,52,-317,144,-317,38,-317},new int[]{-302,1194});
    states[1194] = new State(-319);
    states[1195] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-144,1196,-138,1217,-132,1219,-136,24,-137,27,-167,1220});
    states[1196] = new State(new int[]{5,1197,96,522});
    states[1197] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,1203,48,527,31,531,73,535,64,538,43,543,34,571,23,1214,27,1215},new int[]{-274,1198,-271,1216,-262,1202,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1198] = new State(new int[]{10,1199,96,1200});
    states[1199] = new State(-320);
    states[1200] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,1203,48,527,31,531,73,535,64,538,43,543,34,571,23,1214,27,1215},new int[]{-271,1201,-262,1202,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1201] = new State(-323);
    states[1202] = new State(-324);
    states[1203] = new State(new int[]{8,1204,10,-326,96,-326,20,-308,11,-308,88,-308,81,-308,80,-308,79,-308,78,-308,26,-308,139,-308,82,-308,83,-308,77,-308,75,-308,61,-308,25,-308,23,-308,43,-308,34,-308,27,-308,28,-308,45,-308,24,-308},new int[]{-170,391});
    states[1204] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-169,1205,-168,1213,-167,1209,-132,198,-136,24,-137,27,-289,1211,-138,1212});
    states[1205] = new State(new int[]{9,1206,96,1207});
    states[1206] = new State(-309);
    states[1207] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-168,1208,-167,1209,-132,198,-136,24,-137,27,-289,1211,-138,1212});
    states[1208] = new State(-311);
    states[1209] = new State(new int[]{7,164,119,170,11,208,9,-312,96,-312},new int[]{-285,635,-288,1210});
    states[1210] = new State(-207);
    states[1211] = new State(-313);
    states[1212] = new State(-314);
    states[1213] = new State(-310);
    states[1214] = new State(-325);
    states[1215] = new State(-327);
    states[1216] = new State(-322);
    states[1217] = new State(new int[]{10,1218});
    states[1218] = new State(-321);
    states[1219] = new State(new int[]{5,-338,96,-338,7,-251,11,-251});
    states[1220] = new State(new int[]{7,164,11,208},new int[]{-288,1210});
    states[1221] = new State(-318);
    states[1222] = new State(new int[]{106,1223});
    states[1223] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484},new int[]{-246,1224,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[1224] = new State(new int[]{10,1225});
    states[1225] = new State(-434);
    states[1226] = new State(new int[]{143,1042,145,1043,146,1044,147,1045,149,1046,148,1047,20,-775,103,-775,87,-775,58,-775,26,-775,66,-775,49,-775,52,-775,61,-775,11,-775,25,-775,23,-775,43,-775,34,-775,27,-775,28,-775,45,-775,24,-775,88,-775,81,-775,80,-775,79,-775,78,-775,144,-775},new int[]{-193,1227,-196,1048});
    states[1227] = new State(new int[]{10,1040,106,-778});
    states[1228] = new State(-365);
    states[1229] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444},new int[]{-156,1230,-127,1051,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059,-321,1061,-134,1065});
    states[1230] = new State(new int[]{8,545,5,-460,10,-460,106,-460},new int[]{-113,1231});
    states[1231] = new State(new int[]{5,1234,10,1226,106,-777},new int[]{-194,1232,-195,1242});
    states[1232] = new State(new int[]{20,1195,103,-316,87,-316,58,-316,26,-316,66,-316,49,-316,52,-316,61,-316,11,-316,25,-316,23,-316,43,-316,34,-316,27,-316,28,-316,45,-316,24,-316,88,-316,81,-316,80,-316,79,-316,78,-316,144,-316,38,-316},new int[]{-304,1233,-303,1193,-302,1221});
    states[1233] = new State(-450);
    states[1234] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,1235,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1235] = new State(new int[]{10,1226,106,-777},new int[]{-194,1236,-195,1238});
    states[1236] = new State(new int[]{20,1195,103,-316,87,-316,58,-316,26,-316,66,-316,49,-316,52,-316,61,-316,11,-316,25,-316,23,-316,43,-316,34,-316,27,-316,28,-316,45,-316,24,-316,88,-316,81,-316,80,-316,79,-316,78,-316,144,-316,38,-316},new int[]{-304,1237,-303,1193,-302,1221});
    states[1237] = new State(-451);
    states[1238] = new State(new int[]{106,1239});
    states[1239] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646},new int[]{-92,1240,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640});
    states[1240] = new State(new int[]{10,1241});
    states[1241] = new State(-432);
    states[1242] = new State(new int[]{106,1243});
    states[1243] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646},new int[]{-92,1244,-91,441,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-309,645,-310,640});
    states[1244] = new State(new int[]{10,1245});
    states[1245] = new State(-433);
    states[1246] = new State(-352);
    states[1247] = new State(-353);
    states[1248] = new State(-361);
    states[1249] = new State(new int[]{103,1252,11,-362,25,-362,23,-362,43,-362,34,-362,27,-362,28,-362,45,-362,24,-362,88,-362,81,-362,80,-362,79,-362,78,-362,58,-63,26,-63,66,-63,49,-63,52,-63,61,-63,87,-63},new int[]{-163,1250,-40,1119,-36,1122,-57,1251});
    states[1250] = new State(-417);
    states[1251] = new State(-459);
    states[1252] = new State(new int[]{10,1260,139,23,82,25,83,26,77,28,75,29,140,150,142,151,141,153},new int[]{-96,1253,-132,1257,-136,24,-137,27,-151,1258,-153,148,-152,152});
    states[1253] = new State(new int[]{77,1254,10,1259});
    states[1254] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,140,150,142,151,141,153},new int[]{-96,1255,-132,1257,-136,24,-137,27,-151,1258,-153,148,-152,152});
    states[1255] = new State(new int[]{10,1256});
    states[1256] = new State(-452);
    states[1257] = new State(-455);
    states[1258] = new State(-456);
    states[1259] = new State(-453);
    states[1260] = new State(-454);
    states[1261] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444,8,-370,106,-370,10,-370},new int[]{-158,1262,-157,1049,-156,1050,-127,1051,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059,-321,1061,-134,1065});
    states[1262] = new State(new int[]{8,545,106,-460,10,-460},new int[]{-113,1263});
    states[1263] = new State(new int[]{106,1265,10,1038},new int[]{-194,1264});
    states[1264] = new State(-366);
    states[1265] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484},new int[]{-246,1266,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[1266] = new State(new int[]{10,1267});
    states[1267] = new State(-418);
    states[1268] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444,8,-370,10,-370},new int[]{-158,1269,-157,1049,-156,1050,-127,1051,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059,-321,1061,-134,1065});
    states[1269] = new State(new int[]{8,545,10,-460},new int[]{-113,1270});
    states[1270] = new State(new int[]{10,1038},new int[]{-194,1271});
    states[1271] = new State(-368);
    states[1272] = new State(-358);
    states[1273] = new State(-429);
    states[1274] = new State(-359);
    states[1275] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35},new int[]{-159,1276,-132,1107,-136,24,-137,27,-135,1108});
    states[1276] = new State(new int[]{7,1092,11,1098,5,-387},new int[]{-220,1277,-225,1095});
    states[1277] = new State(new int[]{82,1081,83,1087,10,-394},new int[]{-189,1278});
    states[1278] = new State(new int[]{10,1279});
    states[1279] = new State(new int[]{62,1076,148,1078,147,1079,143,1080,11,-384,25,-384,23,-384,43,-384,34,-384,27,-384,28,-384,45,-384,24,-384,88,-384,81,-384,80,-384,79,-384,78,-384},new int[]{-192,1280,-197,1281});
    states[1280] = new State(-376);
    states[1281] = new State(new int[]{10,1282});
    states[1282] = new State(new int[]{62,1076,11,-384,25,-384,23,-384,43,-384,34,-384,27,-384,28,-384,45,-384,24,-384,88,-384,81,-384,80,-384,79,-384,78,-384},new int[]{-192,1283});
    states[1283] = new State(-377);
    states[1284] = new State(new int[]{45,1285});
    states[1285] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35},new int[]{-159,1286,-132,1107,-136,24,-137,27,-135,1108});
    states[1286] = new State(new int[]{7,1092,11,1098,5,-387},new int[]{-220,1287,-225,1095});
    states[1287] = new State(new int[]{106,1115,10,-383},new int[]{-198,1288});
    states[1288] = new State(new int[]{10,1289});
    states[1289] = new State(-380);
    states[1290] = new State(new int[]{11,627,88,-336,81,-336,80,-336,79,-336,78,-336,25,-197,23,-197,43,-197,34,-197,27,-197,28,-197,45,-197,24,-197},new int[]{-50,402,-49,403,-6,404,-236,996,-51,1291});
    states[1291] = new State(-348);
    states[1292] = new State(-345);
    states[1293] = new State(-300);
    states[1294] = new State(-301);
    states[1295] = new State(new int[]{23,1296,47,1297,40,1298,8,-302,20,-302,11,-302,88,-302,81,-302,80,-302,79,-302,78,-302,26,-302,139,-302,82,-302,83,-302,77,-302,75,-302,61,-302,25,-302,43,-302,34,-302,27,-302,28,-302,45,-302,24,-302,10,-302});
    states[1296] = new State(-303);
    states[1297] = new State(-304);
    states[1298] = new State(-305);
    states[1299] = new State(new int[]{68,1301,69,1302,143,1303,24,1304,25,1305,23,-297,40,-297,63,-297},new int[]{-19,1300});
    states[1300] = new State(-299);
    states[1301] = new State(-291);
    states[1302] = new State(-292);
    states[1303] = new State(-293);
    states[1304] = new State(-294);
    states[1305] = new State(-295);
    states[1306] = new State(-298);
    states[1307] = new State(new int[]{119,1309,116,-211,7,-251,11,-251},new int[]{-141,1308});
    states[1308] = new State(-212);
    states[1309] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-144,1310,-132,772,-136,24,-137,27});
    states[1310] = new State(new int[]{118,1311,117,1057,96,522});
    states[1311] = new State(-213);
    states[1312] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571,68,1301,69,1302,143,1303,24,1304,25,1305,23,-296,40,-296,63,-296},new int[]{-273,1313,-262,1160,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542,-27,1161,-20,1162,-21,1299,-19,1306});
    states[1313] = new State(new int[]{10,1314});
    states[1314] = new State(-204);
    states[1315] = new State(new int[]{116,1316});
    states[1316] = new State(new int[]{41,1317,42,1321});
    states[1317] = new State(new int[]{8,1204,11,-308,10,-308,88,-308,81,-308,80,-308,79,-308,78,-308,26,-308,139,-308,82,-308,83,-308,77,-308,75,-308,61,-308,25,-308,23,-308,43,-308,34,-308,27,-308,28,-308,45,-308,24,-308},new int[]{-170,1318});
    states[1318] = new State(new int[]{11,627,10,-306,88,-334,81,-334,80,-334,79,-334,78,-334,26,-197,139,-197,82,-197,83,-197,77,-197,75,-197,61,-197,25,-197,23,-197,43,-197,34,-197,27,-197,28,-197,45,-197,24,-197},new int[]{-23,1319,-22,1167,-29,1173,-31,397,-41,1174,-6,1175,-236,996,-30,1290,-50,1292,-49,403,-51,1291});
    states[1319] = new State(new int[]{10,1320});
    states[1320] = new State(-205);
    states[1321] = new State(new int[]{11,627,10,-306,88,-334,81,-334,80,-334,79,-334,78,-334,26,-197,139,-197,82,-197,83,-197,77,-197,75,-197,61,-197,25,-197,23,-197,43,-197,34,-197,27,-197,28,-197,45,-197,24,-197},new int[]{-23,1322,-22,1167,-29,1173,-31,397,-41,1174,-6,1175,-236,996,-30,1290,-50,1292,-49,403,-51,1291});
    states[1322] = new State(new int[]{10,1323});
    states[1323] = new State(-206);
    states[1324] = new State(new int[]{11,627,139,-197,82,-197,83,-197,77,-197,75,-197},new int[]{-45,1325,-6,1154,-236,996});
    states[1325] = new State(-98);
    states[1326] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,8,1331,58,-84,26,-84,66,-84,49,-84,52,-84,61,-84,87,-84},new int[]{-300,1327,-297,1328,-298,1329,-144,773,-132,772,-136,24,-137,27});
    states[1327] = new State(-104);
    states[1328] = new State(-100);
    states[1329] = new State(new int[]{10,1330});
    states[1330] = new State(-401);
    states[1331] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,1332,-136,24,-137,27});
    states[1332] = new State(new int[]{96,1333});
    states[1333] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-144,1334,-132,772,-136,24,-137,27});
    states[1334] = new State(new int[]{9,1335,96,522});
    states[1335] = new State(new int[]{106,1336});
    states[1336] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-91,1337,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599});
    states[1337] = new State(new int[]{10,1338,13,129});
    states[1338] = new State(-101);
    states[1339] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,8,1331},new int[]{-300,1340,-297,1328,-298,1329,-144,773,-132,772,-136,24,-137,27});
    states[1340] = new State(-102);
    states[1341] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,8,1331},new int[]{-300,1342,-297,1328,-298,1329,-144,773,-132,772,-136,24,-137,27});
    states[1342] = new State(-103);
    states[1343] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,1005,12,-272,96,-272},new int[]{-257,1344,-258,1345,-85,177,-94,212,-95,220,-167,361,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152});
    states[1344] = new State(-270);
    states[1345] = new State(-271);
    states[1346] = new State(-269);
    states[1347] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-262,1348,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1348] = new State(-268);
    states[1349] = new State(-236);
    states[1350] = new State(-237);
    states[1351] = new State(new int[]{123,370,117,-238,96,-238,12,-238,116,-238,9,-238,10,-238,106,-238,88,-238,94,-238,97,-238,30,-238,100,-238,95,-238,29,-238,83,-238,82,-238,2,-238,81,-238,80,-238,79,-238,78,-238,133,-238});
    states[1352] = new State(-231);
    states[1353] = new State(-227);
    states[1354] = new State(-598);
    states[1355] = new State(new int[]{8,1356});
    states[1356] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,44,444,39,474,8,509,18,339,19,344},new int[]{-320,1357,-319,1365,-132,1361,-136,24,-137,27,-89,1364,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598});
    states[1357] = new State(new int[]{9,1358,96,1359});
    states[1358] = new State(-607);
    states[1359] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,44,444,39,474,8,509,18,339,19,344},new int[]{-319,1360,-132,1361,-136,24,-137,27,-89,1364,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598});
    states[1360] = new State(-611);
    states[1361] = new State(new int[]{106,1362,17,-746,8,-746,7,-746,138,-746,4,-746,15,-746,134,-746,132,-746,114,-746,113,-746,127,-746,128,-746,129,-746,130,-746,126,-746,112,-746,111,-746,124,-746,125,-746,122,-746,116,-746,121,-746,119,-746,117,-746,120,-746,118,-746,133,-746,9,-746,96,-746,115,-746,11,-746});
    states[1362] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344},new int[]{-89,1363,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598});
    states[1363] = new State(new int[]{116,243,121,244,119,245,117,246,120,247,118,248,133,249,9,-608,96,-608},new int[]{-183,136});
    states[1364] = new State(new int[]{116,243,121,244,119,245,117,246,120,247,118,248,133,249,9,-609,96,-609},new int[]{-183,136});
    states[1365] = new State(-610);
    states[1366] = new State(-637);
    states[1367] = new State(-638);
    states[1368] = new State(-639);
    states[1369] = new State(-631);
    states[1370] = new State(-764);
    states[1371] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363,5,1384,12,-171},new int[]{-106,1372,-69,1374,-83,1376,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880,-67,185,-86,866});
    states[1372] = new State(new int[]{12,1373});
    states[1373] = new State(-163);
    states[1374] = new State(new int[]{12,1375});
    states[1375] = new State(-167);
    states[1376] = new State(new int[]{5,1377,13,189,6,1382,96,-174,12,-174});
    states[1377] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363,5,-675,12,-675},new int[]{-107,1378,-83,1381,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[1378] = new State(new int[]{5,1379,12,-680});
    states[1379] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-83,1380,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[1380] = new State(new int[]{13,189,12,-682});
    states[1381] = new State(new int[]{13,189,5,-674,12,-674});
    states[1382] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-83,1383,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[1383] = new State(new int[]{13,189,96,-175,9,-175,12,-175,5,-175});
    states[1384] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363,5,-675,12,-675},new int[]{-107,1385,-83,1381,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[1385] = new State(new int[]{5,1386,12,-681});
    states[1386] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-83,1387,-75,193,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879,-228,880});
    states[1387] = new State(new int[]{13,189,12,-683});
    states[1388] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35,68,36,63,37,124,38,19,39,18,40,62,41,20,42,125,43,126,44,127,45,128,46,129,47,130,48,131,49,132,50,133,51,134,52,21,53,73,54,87,55,22,56,23,57,26,58,27,59,28,60,71,61,95,62,29,63,88,64,30,65,31,66,24,67,100,68,97,69,32,70,33,71,34,72,37,73,38,74,39,75,99,76,40,77,43,78,45,79,46,80,47,81,93,82,48,83,98,84,49,85,25,86,50,87,70,88,94,89,51,90,52,91,53,92,54,93,55,94,56,95,57,96,58,97,60,98,101,99,102,100,105,101,103,102,104,103,61,104,74,105,35,106,36,107,42,108,44,110},new int[]{-123,1389,-132,22,-136,24,-137,27,-279,30,-135,31,-280,109});
    states[1389] = new State(-164);
    states[1390] = new State(-165);
    states[1391] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,5,601,34,641,43,646,9,-169},new int[]{-70,1392,-66,1394,-82,495,-81,127,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600,-309,639,-310,640});
    states[1392] = new State(new int[]{9,1393});
    states[1393] = new State(-166);
    states[1394] = new State(new int[]{96,435,9,-168});
    states[1395] = new State(-136);
    states[1396] = new State(new int[]{139,23,82,25,83,26,77,28,75,319,140,150,142,151,141,153,150,155,152,156,151,157,39,336,18,339,19,344,11,863,55,867,137,868,8,870,131,873,112,362,111,363},new int[]{-75,1397,-12,314,-10,317,-13,203,-132,318,-136,24,-137,27,-151,334,-153,148,-152,152,-15,335,-243,338,-281,343,-226,862,-186,875,-160,877,-251,878,-255,879});
    states[1397] = new State(new int[]{112,1398,111,1399,124,1400,125,1401,13,-114,6,-114,96,-114,9,-114,12,-114,5,-114,88,-114,10,-114,94,-114,97,-114,30,-114,100,-114,95,-114,29,-114,83,-114,82,-114,2,-114,81,-114,80,-114,79,-114,78,-114},new int[]{-180,194});
    states[1398] = new State(-126);
    states[1399] = new State(-127);
    states[1400] = new State(-128);
    states[1401] = new State(-129);
    states[1402] = new State(-117);
    states[1403] = new State(-118);
    states[1404] = new State(-119);
    states[1405] = new State(-120);
    states[1406] = new State(-121);
    states[1407] = new State(-122);
    states[1408] = new State(-123);
    states[1409] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153},new int[]{-85,1410,-94,212,-95,220,-167,361,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152});
    states[1410] = new State(new int[]{112,1398,111,1399,124,1400,125,1401,13,-240,117,-240,96,-240,12,-240,116,-240,9,-240,10,-240,123,-240,106,-240,88,-240,94,-240,97,-240,30,-240,100,-240,95,-240,29,-240,83,-240,82,-240,2,-240,81,-240,80,-240,79,-240,78,-240,133,-240},new int[]{-180,178});
    states[1411] = new State(-33);
    states[1412] = new State(new int[]{58,1125,26,1146,66,1150,49,1324,52,1339,61,1341,11,627,87,-59,88,-59,99,-59,43,-197,34,-197,25,-197,23,-197,27,-197,28,-197},new int[]{-43,1413,-154,1414,-26,1415,-48,1416,-275,1417,-296,1418,-207,1419,-6,1420,-236,996});
    states[1413] = new State(-61);
    states[1414] = new State(-71);
    states[1415] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,58,-72,26,-72,66,-72,49,-72,52,-72,61,-72,11,-72,43,-72,34,-72,25,-72,23,-72,27,-72,28,-72,87,-72,88,-72,99,-72},new int[]{-24,1132,-25,1133,-126,1135,-132,1145,-136,24,-137,27});
    states[1416] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,58,-73,26,-73,66,-73,49,-73,52,-73,61,-73,11,-73,43,-73,34,-73,25,-73,23,-73,27,-73,28,-73,87,-73,88,-73,99,-73},new int[]{-24,1149,-25,1133,-126,1135,-132,1145,-136,24,-137,27});
    states[1417] = new State(new int[]{11,627,58,-74,26,-74,66,-74,49,-74,52,-74,61,-74,43,-74,34,-74,25,-74,23,-74,27,-74,28,-74,87,-74,88,-74,99,-74,139,-197,82,-197,83,-197,77,-197,75,-197},new int[]{-45,1153,-6,1154,-236,996});
    states[1418] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,8,1331,58,-75,26,-75,66,-75,49,-75,52,-75,61,-75,11,-75,43,-75,34,-75,25,-75,23,-75,27,-75,28,-75,87,-75,88,-75,99,-75},new int[]{-300,1327,-297,1328,-298,1329,-144,773,-132,772,-136,24,-137,27});
    states[1419] = new State(-76);
    states[1420] = new State(new int[]{43,1433,34,1440,25,1246,23,1247,27,1468,28,1268,11,627},new int[]{-200,1421,-236,406,-201,1422,-208,1423,-215,1424,-212,1187,-216,1228,-3,1457,-204,1465,-214,1466});
    states[1421] = new State(-79);
    states[1422] = new State(-77);
    states[1423] = new State(-420);
    states[1424] = new State(new int[]{144,1426,103,1252,58,-60,26,-60,66,-60,49,-60,52,-60,61,-60,11,-60,43,-60,34,-60,25,-60,23,-60,27,-60,28,-60,87,-60},new int[]{-165,1425,-164,1428,-38,1429,-39,1412,-57,1432});
    states[1425] = new State(-422);
    states[1426] = new State(new int[]{10,1427});
    states[1427] = new State(-428);
    states[1428] = new State(-435);
    states[1429] = new State(new int[]{87,117},new int[]{-241,1430});
    states[1430] = new State(new int[]{10,1431});
    states[1431] = new State(-457);
    states[1432] = new State(-436);
    states[1433] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444},new int[]{-157,1434,-156,1050,-127,1051,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059,-321,1061,-134,1065});
    states[1434] = new State(new int[]{8,545,10,-460,106,-460},new int[]{-113,1435});
    states[1435] = new State(new int[]{10,1226,106,-777},new int[]{-194,1191,-195,1436});
    states[1436] = new State(new int[]{106,1437});
    states[1437] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484},new int[]{-246,1438,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[1438] = new State(new int[]{10,1439});
    states[1439] = new State(-427);
    states[1440] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444},new int[]{-156,1441,-127,1051,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059,-321,1061,-134,1065});
    states[1441] = new State(new int[]{8,545,5,-460,10,-460,106,-460},new int[]{-113,1442});
    states[1442] = new State(new int[]{5,1443,10,1226,106,-777},new int[]{-194,1232,-195,1451});
    states[1443] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,1444,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1444] = new State(new int[]{10,1226,106,-777},new int[]{-194,1236,-195,1445});
    states[1445] = new State(new int[]{106,1446});
    states[1446] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646},new int[]{-91,1447,-309,1449,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-310,640});
    states[1447] = new State(new int[]{10,1448,13,129});
    states[1448] = new State(-423);
    states[1449] = new State(new int[]{10,1450});
    states[1450] = new State(-425);
    states[1451] = new State(new int[]{106,1452});
    states[1452] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,476,18,339,19,344,34,641,43,646},new int[]{-91,1453,-309,1455,-90,133,-89,242,-93,442,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,437,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-310,640});
    states[1453] = new State(new int[]{10,1454,13,129});
    states[1454] = new State(-424);
    states[1455] = new State(new int[]{10,1456});
    states[1456] = new State(-426);
    states[1457] = new State(new int[]{27,1459,43,1433,34,1440},new int[]{-208,1458,-215,1424,-212,1187,-216,1228});
    states[1458] = new State(-421);
    states[1459] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444,8,-370,106,-370,10,-370},new int[]{-158,1460,-157,1049,-156,1050,-127,1051,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059,-321,1061,-134,1065});
    states[1460] = new State(new int[]{8,545,106,-460,10,-460},new int[]{-113,1461});
    states[1461] = new State(new int[]{106,1462,10,1038},new int[]{-194,414});
    states[1462] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484},new int[]{-246,1463,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[1463] = new State(new int[]{10,1464});
    states[1464] = new State(-416);
    states[1465] = new State(-78);
    states[1466] = new State(-60,new int[]{-164,1467,-38,1429,-39,1412});
    states[1467] = new State(-414);
    states[1468] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444,8,-370,106,-370,10,-370},new int[]{-158,1469,-157,1049,-156,1050,-127,1051,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059,-321,1061,-134,1065});
    states[1469] = new State(new int[]{8,545,106,-460,10,-460},new int[]{-113,1470});
    states[1470] = new State(new int[]{106,1471,10,1038},new int[]{-194,1264});
    states[1471] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,155,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,10,-484},new int[]{-246,1472,-4,123,-100,124,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807});
    states[1472] = new State(new int[]{10,1473});
    states[1473] = new State(-415);
    states[1474] = new State(new int[]{3,1476,51,-13,87,-13,58,-13,26,-13,66,-13,49,-13,52,-13,61,-13,11,-13,43,-13,34,-13,25,-13,23,-13,27,-13,28,-13,40,-13,88,-13,99,-13},new int[]{-171,1475});
    states[1475] = new State(-15);
    states[1476] = new State(new int[]{139,1477,140,1478});
    states[1477] = new State(-16);
    states[1478] = new State(-17);
    states[1479] = new State(-14);
    states[1480] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-132,1481,-136,24,-137,27});
    states[1481] = new State(new int[]{10,1483,8,1484},new int[]{-174,1482});
    states[1482] = new State(-26);
    states[1483] = new State(-27);
    states[1484] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-176,1485,-131,1491,-132,1490,-136,24,-137,27});
    states[1485] = new State(new int[]{9,1486,96,1488});
    states[1486] = new State(new int[]{10,1487});
    states[1487] = new State(-28);
    states[1488] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-131,1489,-132,1490,-136,24,-137,27});
    states[1489] = new State(-30);
    states[1490] = new State(-31);
    states[1491] = new State(-29);
    states[1492] = new State(-3);
    states[1493] = new State(new int[]{101,1548,102,1549,105,1550,11,627},new int[]{-295,1494,-236,406,-2,1543});
    states[1494] = new State(new int[]{40,1515,51,-36,58,-36,26,-36,66,-36,49,-36,52,-36,61,-36,11,-36,43,-36,34,-36,25,-36,23,-36,27,-36,28,-36,88,-36,99,-36,87,-36},new int[]{-148,1495,-149,1512,-291,1541});
    states[1495] = new State(new int[]{38,1509},new int[]{-147,1496});
    states[1496] = new State(new int[]{88,1499,99,1500,87,1506},new int[]{-140,1497});
    states[1497] = new State(new int[]{7,1498});
    states[1498] = new State(-42);
    states[1499] = new State(-52);
    states[1500] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,100,-484,10,-484},new int[]{-238,1501,-247,715,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[1501] = new State(new int[]{88,1502,100,1503,10,120});
    states[1502] = new State(-53);
    states[1503] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484},new int[]{-238,1504,-247,715,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[1504] = new State(new int[]{88,1505,10,120});
    states[1505] = new State(-54);
    states[1506] = new State(new int[]{137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,680,8,681,18,339,19,344,140,150,142,151,141,153,150,717,152,156,151,157,56,696,87,117,37,674,22,703,93,719,53,724,32,729,54,739,98,745,46,752,33,755,52,763,59,796,74,801,72,788,35,808,88,-484,10,-484},new int[]{-238,1507,-247,715,-246,122,-4,123,-100,124,-117,418,-99,430,-132,716,-136,24,-137,27,-178,443,-243,489,-281,490,-14,664,-151,147,-153,148,-152,152,-15,154,-16,491,-54,665,-103,502,-199,694,-118,695,-241,700,-139,701,-32,702,-233,718,-305,723,-109,728,-306,738,-146,743,-290,744,-234,751,-108,754,-301,762,-55,792,-161,793,-160,794,-155,795,-111,800,-112,805,-110,806,-335,807,-128,928});
    states[1507] = new State(new int[]{88,1508,10,120});
    states[1508] = new State(-55);
    states[1509] = new State(-36,new int[]{-291,1510});
    states[1510] = new State(new int[]{51,14,58,-60,26,-60,66,-60,49,-60,52,-60,61,-60,11,-60,43,-60,34,-60,25,-60,23,-60,27,-60,28,-60,88,-60,99,-60,87,-60},new int[]{-38,1511,-39,1412});
    states[1511] = new State(-50);
    states[1512] = new State(new int[]{88,1499,99,1500,87,1506},new int[]{-140,1513});
    states[1513] = new State(new int[]{7,1514});
    states[1514] = new State(-43);
    states[1515] = new State(-36,new int[]{-291,1516});
    states[1516] = new State(new int[]{51,14,26,-57,66,-57,49,-57,52,-57,61,-57,11,-57,43,-57,34,-57,38,-57},new int[]{-37,1517,-35,1518});
    states[1517] = new State(-49);
    states[1518] = new State(new int[]{26,1146,66,1150,49,1324,52,1339,61,1341,11,627,38,-56,43,-197,34,-197},new int[]{-44,1519,-26,1520,-48,1521,-275,1522,-296,1523,-219,1524,-6,1525,-236,996,-218,1540});
    states[1519] = new State(-58);
    states[1520] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,26,-65,66,-65,49,-65,52,-65,61,-65,11,-65,43,-65,34,-65,38,-65},new int[]{-24,1132,-25,1133,-126,1135,-132,1145,-136,24,-137,27});
    states[1521] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,26,-66,66,-66,49,-66,52,-66,61,-66,11,-66,43,-66,34,-66,38,-66},new int[]{-24,1149,-25,1133,-126,1135,-132,1145,-136,24,-137,27});
    states[1522] = new State(new int[]{11,627,26,-67,66,-67,49,-67,52,-67,61,-67,43,-67,34,-67,38,-67,139,-197,82,-197,83,-197,77,-197,75,-197},new int[]{-45,1153,-6,1154,-236,996});
    states[1523] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,8,1331,26,-68,66,-68,49,-68,52,-68,61,-68,11,-68,43,-68,34,-68,38,-68},new int[]{-300,1327,-297,1328,-298,1329,-144,773,-132,772,-136,24,-137,27});
    states[1524] = new State(-69);
    states[1525] = new State(new int[]{43,1532,11,627,34,1535},new int[]{-212,1526,-236,406,-216,1529});
    states[1526] = new State(new int[]{144,1527,26,-85,66,-85,49,-85,52,-85,61,-85,11,-85,43,-85,34,-85,38,-85});
    states[1527] = new State(new int[]{10,1528});
    states[1528] = new State(-86);
    states[1529] = new State(new int[]{144,1530,26,-87,66,-87,49,-87,52,-87,61,-87,11,-87,43,-87,34,-87,38,-87});
    states[1530] = new State(new int[]{10,1531});
    states[1531] = new State(-88);
    states[1532] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444},new int[]{-157,1533,-156,1050,-127,1051,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059,-321,1061,-134,1065});
    states[1533] = new State(new int[]{8,545,10,-460},new int[]{-113,1534});
    states[1534] = new State(new int[]{10,1038},new int[]{-194,1191});
    states[1535] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,44,444},new int[]{-156,1536,-127,1051,-122,1052,-119,1053,-132,1058,-136,24,-137,27,-178,1059,-321,1061,-134,1065});
    states[1536] = new State(new int[]{8,545,5,-460,10,-460},new int[]{-113,1537});
    states[1537] = new State(new int[]{5,1538,10,1038},new int[]{-194,1232});
    states[1538] = new State(new int[]{139,310,82,25,83,26,77,28,75,29,150,155,152,156,151,157,112,362,111,363,140,150,142,151,141,153,8,365,138,376,21,382,47,390,48,527,31,531,73,535,64,538,43,543,34,571},new int[]{-261,1539,-262,378,-258,308,-85,177,-94,212,-95,220,-167,221,-132,198,-136,24,-137,27,-15,358,-186,359,-151,364,-153,148,-152,152,-259,367,-289,368,-242,374,-235,375,-267,379,-268,380,-264,381,-256,388,-28,389,-249,526,-115,530,-116,534,-213,540,-211,541,-210,542});
    states[1539] = new State(new int[]{10,1038},new int[]{-194,1236});
    states[1540] = new State(-70);
    states[1541] = new State(new int[]{51,14,58,-60,26,-60,66,-60,49,-60,52,-60,61,-60,11,-60,43,-60,34,-60,25,-60,23,-60,27,-60,28,-60,88,-60,99,-60,87,-60},new int[]{-38,1542,-39,1412});
    states[1542] = new State(-51);
    states[1543] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-124,1544,-132,1547,-136,24,-137,27});
    states[1544] = new State(new int[]{10,1545});
    states[1545] = new State(new int[]{3,1476,40,-12,88,-12,99,-12,87,-12,51,-12,58,-12,26,-12,66,-12,49,-12,52,-12,61,-12,11,-12,43,-12,34,-12,25,-12,23,-12,27,-12,28,-12},new int[]{-172,1546,-173,1474,-171,1479});
    states[1546] = new State(-44);
    states[1547] = new State(-48);
    states[1548] = new State(-46);
    states[1549] = new State(-47);
    states[1550] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35,68,36,63,37,124,38,19,39,18,40,62,41,20,42,125,43,126,44,127,45,128,46,129,47,130,48,131,49,132,50,133,51,134,52,21,53,73,54,87,55,22,56,23,57,26,58,27,59,28,60,71,61,95,62,29,63,88,64,30,65,31,66,24,67,100,68,97,69,32,70,33,71,34,72,37,73,38,74,39,75,99,76,40,77,43,78,45,79,46,80,47,81,93,82,48,83,98,84,49,85,25,86,50,87,70,88,94,89,51,90,52,91,53,92,54,93,55,94,56,95,57,96,58,97,60,98,101,99,102,100,105,101,103,102,104,103,61,104,74,105,35,106,36,107,42,108,44,110},new int[]{-143,1551,-123,113,-132,22,-136,24,-137,27,-279,30,-135,31,-280,109});
    states[1551] = new State(new int[]{10,1552,7,20});
    states[1552] = new State(new int[]{3,1476,40,-12,88,-12,99,-12,87,-12,51,-12,58,-12,26,-12,66,-12,49,-12,52,-12,61,-12,11,-12,43,-12,34,-12,25,-12,23,-12,27,-12,28,-12},new int[]{-172,1553,-173,1474,-171,1479});
    states[1553] = new State(-45);
    states[1554] = new State(-4);
    states[1555] = new State(new int[]{49,1557,55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,509,18,339,19,344,5,601},new int[]{-81,1556,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,428,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600});
    states[1556] = new State(-5);
    states[1557] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-129,1558,-132,1559,-136,24,-137,27});
    states[1558] = new State(-6);
    states[1559] = new State(new int[]{119,1055,2,-211},new int[]{-141,1308});
    states[1560] = new State(new int[]{139,23,82,25,83,26,77,28,75,29},new int[]{-307,1561,-308,1562,-132,1566,-136,24,-137,27});
    states[1561] = new State(-7);
    states[1562] = new State(new int[]{7,1563,119,170,2,-742},new int[]{-285,1565});
    states[1563] = new State(new int[]{139,23,82,25,83,26,77,28,75,29,81,32,80,33,79,34,78,35,68,36,63,37,124,38,19,39,18,40,62,41,20,42,125,43,126,44,127,45,128,46,129,47,130,48,131,49,132,50,133,51,134,52,21,53,73,54,87,55,22,56,23,57,26,58,27,59,28,60,71,61,95,62,29,63,88,64,30,65,31,66,24,67,100,68,97,69,32,70,33,71,34,72,37,73,38,74,39,75,99,76,40,77,43,78,45,79,46,80,47,81,93,82,48,83,98,84,49,85,25,86,50,87,70,88,94,89,51,90,52,91,53,92,54,93,55,94,56,95,57,96,58,97,60,98,101,99,102,100,105,101,103,102,104,103,61,104,74,105,35,106,36,107,42,108,44,110},new int[]{-123,1564,-132,22,-136,24,-137,27,-279,30,-135,31,-280,109});
    states[1564] = new State(-741);
    states[1565] = new State(-743);
    states[1566] = new State(-740);
    states[1567] = new State(new int[]{55,143,140,150,142,151,141,153,150,155,152,156,151,157,62,159,11,273,131,424,112,362,111,363,137,429,139,23,82,25,83,26,77,28,75,319,44,444,39,474,8,681,18,339,19,344,5,601,52,763},new int[]{-245,1568,-81,1569,-91,128,-90,133,-89,242,-93,250,-76,282,-88,272,-14,144,-151,147,-153,148,-152,152,-15,154,-53,158,-186,426,-100,1570,-117,418,-99,430,-132,508,-136,24,-137,27,-178,443,-243,489,-281,490,-16,491,-54,496,-103,502,-160,503,-254,504,-77,505,-250,558,-252,559,-253,598,-227,599,-105,600,-4,1571,-301,1572});
    states[1568] = new State(-8);
    states[1569] = new State(-9);
    states[1570] = new State(new int[]{106,468,107,469,108,470,109,471,110,472,134,-727,132,-727,114,-727,113,-727,127,-727,128,-727,129,-727,130,-727,126,-727,5,-727,112,-727,111,-727,124,-727,125,-727,122,-727,116,-727,121,-727,119,-727,117,-727,120,-727,118,-727,133,-727,16,-727,13,-727,2,-727,115,-727},new int[]{-181,125});
    states[1571] = new State(-10);
    states[1572] = new State(-11);

    rules[1] = new Rule(-345, new int[]{-1,2});
    rules[2] = new Rule(-1, new int[]{-221});
    rules[3] = new Rule(-1, new int[]{-293});
    rules[4] = new Rule(-1, new int[]{-162});
    rules[5] = new Rule(-162, new int[]{84,-81});
    rules[6] = new Rule(-162, new int[]{84,49,-129});
    rules[7] = new Rule(-162, new int[]{86,-307});
    rules[8] = new Rule(-162, new int[]{85,-245});
    rules[9] = new Rule(-245, new int[]{-81});
    rules[10] = new Rule(-245, new int[]{-4});
    rules[11] = new Rule(-245, new int[]{-301});
    rules[12] = new Rule(-172, new int[]{});
    rules[13] = new Rule(-172, new int[]{-173});
    rules[14] = new Rule(-173, new int[]{-171});
    rules[15] = new Rule(-173, new int[]{-173,-171});
    rules[16] = new Rule(-171, new int[]{3,139});
    rules[17] = new Rule(-171, new int[]{3,140});
    rules[18] = new Rule(-221, new int[]{-222,-172,-291,-17,-175});
    rules[19] = new Rule(-175, new int[]{7});
    rules[20] = new Rule(-175, new int[]{10});
    rules[21] = new Rule(-175, new int[]{5});
    rules[22] = new Rule(-175, new int[]{96});
    rules[23] = new Rule(-175, new int[]{6});
    rules[24] = new Rule(-175, new int[]{});
    rules[25] = new Rule(-222, new int[]{});
    rules[26] = new Rule(-222, new int[]{60,-132,-174});
    rules[27] = new Rule(-174, new int[]{10});
    rules[28] = new Rule(-174, new int[]{8,-176,9,10});
    rules[29] = new Rule(-176, new int[]{-131});
    rules[30] = new Rule(-176, new int[]{-176,96,-131});
    rules[31] = new Rule(-131, new int[]{-132});
    rules[32] = new Rule(-17, new int[]{-34,-241});
    rules[33] = new Rule(-34, new int[]{-38});
    rules[34] = new Rule(-143, new int[]{-123});
    rules[35] = new Rule(-143, new int[]{-143,7,-123});
    rules[36] = new Rule(-291, new int[]{});
    rules[37] = new Rule(-291, new int[]{-291,51,-292,10});
    rules[38] = new Rule(-292, new int[]{-294});
    rules[39] = new Rule(-292, new int[]{-292,96,-294});
    rules[40] = new Rule(-294, new int[]{-143});
    rules[41] = new Rule(-294, new int[]{-143,133,140});
    rules[42] = new Rule(-293, new int[]{-6,-295,-148,-147,-140,7});
    rules[43] = new Rule(-293, new int[]{-6,-295,-149,-140,7});
    rules[44] = new Rule(-295, new int[]{-2,-124,10,-172});
    rules[45] = new Rule(-295, new int[]{105,-143,10,-172});
    rules[46] = new Rule(-2, new int[]{101});
    rules[47] = new Rule(-2, new int[]{102});
    rules[48] = new Rule(-124, new int[]{-132});
    rules[49] = new Rule(-148, new int[]{40,-291,-37});
    rules[50] = new Rule(-147, new int[]{38,-291,-38});
    rules[51] = new Rule(-149, new int[]{-291,-38});
    rules[52] = new Rule(-140, new int[]{88});
    rules[53] = new Rule(-140, new int[]{99,-238,88});
    rules[54] = new Rule(-140, new int[]{99,-238,100,-238,88});
    rules[55] = new Rule(-140, new int[]{87,-238,88});
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
    rules[67] = new Rule(-44, new int[]{-275});
    rules[68] = new Rule(-44, new int[]{-296});
    rules[69] = new Rule(-44, new int[]{-219});
    rules[70] = new Rule(-44, new int[]{-218});
    rules[71] = new Rule(-43, new int[]{-154});
    rules[72] = new Rule(-43, new int[]{-26});
    rules[73] = new Rule(-43, new int[]{-48});
    rules[74] = new Rule(-43, new int[]{-275});
    rules[75] = new Rule(-43, new int[]{-296});
    rules[76] = new Rule(-43, new int[]{-207});
    rules[77] = new Rule(-200, new int[]{-201});
    rules[78] = new Rule(-200, new int[]{-204});
    rules[79] = new Rule(-207, new int[]{-6,-200});
    rules[80] = new Rule(-42, new int[]{-154});
    rules[81] = new Rule(-42, new int[]{-26});
    rules[82] = new Rule(-42, new int[]{-48});
    rules[83] = new Rule(-42, new int[]{-275});
    rules[84] = new Rule(-42, new int[]{-296});
    rules[85] = new Rule(-219, new int[]{-6,-212});
    rules[86] = new Rule(-219, new int[]{-6,-212,144,10});
    rules[87] = new Rule(-218, new int[]{-6,-216});
    rules[88] = new Rule(-218, new int[]{-6,-216,144,10});
    rules[89] = new Rule(-154, new int[]{58,-142,10});
    rules[90] = new Rule(-142, new int[]{-128});
    rules[91] = new Rule(-142, new int[]{-142,96,-128});
    rules[92] = new Rule(-128, new int[]{150});
    rules[93] = new Rule(-128, new int[]{-132});
    rules[94] = new Rule(-26, new int[]{26,-24});
    rules[95] = new Rule(-26, new int[]{-26,-24});
    rules[96] = new Rule(-48, new int[]{66,-24});
    rules[97] = new Rule(-48, new int[]{-48,-24});
    rules[98] = new Rule(-275, new int[]{49,-45});
    rules[99] = new Rule(-275, new int[]{-275,-45});
    rules[100] = new Rule(-300, new int[]{-297});
    rules[101] = new Rule(-300, new int[]{8,-132,96,-144,9,106,-91,10});
    rules[102] = new Rule(-296, new int[]{52,-300});
    rules[103] = new Rule(-296, new int[]{61,-300});
    rules[104] = new Rule(-296, new int[]{-296,-300});
    rules[105] = new Rule(-24, new int[]{-25,10});
    rules[106] = new Rule(-25, new int[]{-126,116,-97});
    rules[107] = new Rule(-25, new int[]{-126,5,-262,116,-78});
    rules[108] = new Rule(-97, new int[]{-83});
    rules[109] = new Rule(-97, new int[]{-87});
    rules[110] = new Rule(-126, new int[]{-132});
    rules[111] = new Rule(-73, new int[]{-91});
    rules[112] = new Rule(-73, new int[]{-73,96,-91});
    rules[113] = new Rule(-83, new int[]{-75});
    rules[114] = new Rule(-83, new int[]{-75,-179,-75});
    rules[115] = new Rule(-83, new int[]{-228});
    rules[116] = new Rule(-228, new int[]{-83,13,-83,5,-83});
    rules[117] = new Rule(-179, new int[]{116});
    rules[118] = new Rule(-179, new int[]{121});
    rules[119] = new Rule(-179, new int[]{119});
    rules[120] = new Rule(-179, new int[]{117});
    rules[121] = new Rule(-179, new int[]{120});
    rules[122] = new Rule(-179, new int[]{118});
    rules[123] = new Rule(-179, new int[]{133});
    rules[124] = new Rule(-75, new int[]{-12});
    rules[125] = new Rule(-75, new int[]{-75,-180,-12});
    rules[126] = new Rule(-180, new int[]{112});
    rules[127] = new Rule(-180, new int[]{111});
    rules[128] = new Rule(-180, new int[]{124});
    rules[129] = new Rule(-180, new int[]{125});
    rules[130] = new Rule(-251, new int[]{-12,-188,-270});
    rules[131] = new Rule(-255, new int[]{-10,115,-10});
    rules[132] = new Rule(-12, new int[]{-10});
    rules[133] = new Rule(-12, new int[]{-251});
    rules[134] = new Rule(-12, new int[]{-255});
    rules[135] = new Rule(-12, new int[]{-12,-182,-10});
    rules[136] = new Rule(-12, new int[]{-12,-182,-255});
    rules[137] = new Rule(-182, new int[]{114});
    rules[138] = new Rule(-182, new int[]{113});
    rules[139] = new Rule(-182, new int[]{127});
    rules[140] = new Rule(-182, new int[]{128});
    rules[141] = new Rule(-182, new int[]{129});
    rules[142] = new Rule(-182, new int[]{130});
    rules[143] = new Rule(-182, new int[]{126});
    rules[144] = new Rule(-10, new int[]{-13});
    rules[145] = new Rule(-10, new int[]{-226});
    rules[146] = new Rule(-10, new int[]{55});
    rules[147] = new Rule(-10, new int[]{137,-10});
    rules[148] = new Rule(-10, new int[]{8,-83,9});
    rules[149] = new Rule(-10, new int[]{131,-10});
    rules[150] = new Rule(-10, new int[]{-186,-10});
    rules[151] = new Rule(-10, new int[]{-160});
    rules[152] = new Rule(-226, new int[]{11,-69,12});
    rules[153] = new Rule(-186, new int[]{112});
    rules[154] = new Rule(-186, new int[]{111});
    rules[155] = new Rule(-13, new int[]{-132});
    rules[156] = new Rule(-13, new int[]{-151});
    rules[157] = new Rule(-13, new int[]{-15});
    rules[158] = new Rule(-13, new int[]{39,-132});
    rules[159] = new Rule(-13, new int[]{-243});
    rules[160] = new Rule(-13, new int[]{-281});
    rules[161] = new Rule(-13, new int[]{-13,-11});
    rules[162] = new Rule(-13, new int[]{-13,4,-287});
    rules[163] = new Rule(-13, new int[]{-13,11,-106,12});
    rules[164] = new Rule(-11, new int[]{7,-123});
    rules[165] = new Rule(-11, new int[]{138});
    rules[166] = new Rule(-11, new int[]{8,-70,9});
    rules[167] = new Rule(-11, new int[]{11,-69,12});
    rules[168] = new Rule(-70, new int[]{-66});
    rules[169] = new Rule(-70, new int[]{});
    rules[170] = new Rule(-69, new int[]{-67});
    rules[171] = new Rule(-69, new int[]{});
    rules[172] = new Rule(-67, new int[]{-86});
    rules[173] = new Rule(-67, new int[]{-67,96,-86});
    rules[174] = new Rule(-86, new int[]{-83});
    rules[175] = new Rule(-86, new int[]{-83,6,-83});
    rules[176] = new Rule(-15, new int[]{150});
    rules[177] = new Rule(-15, new int[]{152});
    rules[178] = new Rule(-15, new int[]{151});
    rules[179] = new Rule(-78, new int[]{-83});
    rules[180] = new Rule(-78, new int[]{-87});
    rules[181] = new Rule(-78, new int[]{-229});
    rules[182] = new Rule(-87, new int[]{8,-62,9});
    rules[183] = new Rule(-62, new int[]{});
    rules[184] = new Rule(-62, new int[]{-61});
    rules[185] = new Rule(-61, new int[]{-79});
    rules[186] = new Rule(-61, new int[]{-61,96,-79});
    rules[187] = new Rule(-229, new int[]{8,-231,9});
    rules[188] = new Rule(-231, new int[]{-230});
    rules[189] = new Rule(-231, new int[]{-230,10});
    rules[190] = new Rule(-230, new int[]{-232});
    rules[191] = new Rule(-230, new int[]{-230,10,-232});
    rules[192] = new Rule(-232, new int[]{-121,5,-78});
    rules[193] = new Rule(-121, new int[]{-132});
    rules[194] = new Rule(-45, new int[]{-6,-46});
    rules[195] = new Rule(-6, new int[]{-236});
    rules[196] = new Rule(-6, new int[]{-6,-236});
    rules[197] = new Rule(-6, new int[]{});
    rules[198] = new Rule(-236, new int[]{11,-237,12});
    rules[199] = new Rule(-237, new int[]{-8});
    rules[200] = new Rule(-237, new int[]{-237,96,-8});
    rules[201] = new Rule(-8, new int[]{-9});
    rules[202] = new Rule(-8, new int[]{-132,5,-9});
    rules[203] = new Rule(-46, new int[]{-129,116,-273,10});
    rules[204] = new Rule(-46, new int[]{-130,-273,10});
    rules[205] = new Rule(-46, new int[]{-138,116,41,-170,-23,10});
    rules[206] = new Rule(-46, new int[]{-138,116,42,-23,10});
    rules[207] = new Rule(-138, new int[]{-167,-288});
    rules[208] = new Rule(-288, new int[]{11,-283,12});
    rules[209] = new Rule(-287, new int[]{-285});
    rules[210] = new Rule(-287, new int[]{-288});
    rules[211] = new Rule(-129, new int[]{-132});
    rules[212] = new Rule(-129, new int[]{-132,-141});
    rules[213] = new Rule(-130, new int[]{-132,119,-144,118});
    rules[214] = new Rule(-273, new int[]{-262});
    rules[215] = new Rule(-273, new int[]{-27});
    rules[216] = new Rule(-259, new int[]{-258,13});
    rules[217] = new Rule(-259, new int[]{-289,13});
    rules[218] = new Rule(-262, new int[]{-258});
    rules[219] = new Rule(-262, new int[]{-259});
    rules[220] = new Rule(-262, new int[]{-242});
    rules[221] = new Rule(-262, new int[]{-235});
    rules[222] = new Rule(-262, new int[]{-267});
    rules[223] = new Rule(-262, new int[]{-213});
    rules[224] = new Rule(-262, new int[]{-289});
    rules[225] = new Rule(-289, new int[]{-167,-285});
    rules[226] = new Rule(-285, new int[]{119,-283,117});
    rules[227] = new Rule(-286, new int[]{121});
    rules[228] = new Rule(-286, new int[]{119,-284,117});
    rules[229] = new Rule(-283, new int[]{-265});
    rules[230] = new Rule(-283, new int[]{-283,96,-265});
    rules[231] = new Rule(-284, new int[]{-266});
    rules[232] = new Rule(-284, new int[]{-284,96,-266});
    rules[233] = new Rule(-266, new int[]{});
    rules[234] = new Rule(-265, new int[]{-258});
    rules[235] = new Rule(-265, new int[]{-258,13});
    rules[236] = new Rule(-265, new int[]{-267});
    rules[237] = new Rule(-265, new int[]{-213});
    rules[238] = new Rule(-265, new int[]{-289});
    rules[239] = new Rule(-258, new int[]{-85});
    rules[240] = new Rule(-258, new int[]{-85,6,-85});
    rules[241] = new Rule(-258, new int[]{8,-74,9});
    rules[242] = new Rule(-85, new int[]{-94});
    rules[243] = new Rule(-85, new int[]{-85,-180,-94});
    rules[244] = new Rule(-94, new int[]{-95});
    rules[245] = new Rule(-94, new int[]{-94,-182,-95});
    rules[246] = new Rule(-95, new int[]{-167});
    rules[247] = new Rule(-95, new int[]{-15});
    rules[248] = new Rule(-95, new int[]{-186,-95});
    rules[249] = new Rule(-95, new int[]{-151});
    rules[250] = new Rule(-95, new int[]{-95,8,-69,9});
    rules[251] = new Rule(-167, new int[]{-132});
    rules[252] = new Rule(-167, new int[]{-167,7,-123});
    rules[253] = new Rule(-74, new int[]{-72,96,-72});
    rules[254] = new Rule(-74, new int[]{-74,96,-72});
    rules[255] = new Rule(-72, new int[]{-262});
    rules[256] = new Rule(-72, new int[]{-262,116,-81});
    rules[257] = new Rule(-235, new int[]{138,-261});
    rules[258] = new Rule(-267, new int[]{-268});
    rules[259] = new Rule(-267, new int[]{64,-268});
    rules[260] = new Rule(-268, new int[]{-264});
    rules[261] = new Rule(-268, new int[]{-28});
    rules[262] = new Rule(-268, new int[]{-249});
    rules[263] = new Rule(-268, new int[]{-115});
    rules[264] = new Rule(-268, new int[]{-116});
    rules[265] = new Rule(-116, new int[]{73,57,-262});
    rules[266] = new Rule(-264, new int[]{21,11,-150,12,57,-262});
    rules[267] = new Rule(-264, new int[]{-256});
    rules[268] = new Rule(-256, new int[]{21,57,-262});
    rules[269] = new Rule(-150, new int[]{-257});
    rules[270] = new Rule(-150, new int[]{-150,96,-257});
    rules[271] = new Rule(-257, new int[]{-258});
    rules[272] = new Rule(-257, new int[]{});
    rules[273] = new Rule(-249, new int[]{48,57,-262});
    rules[274] = new Rule(-115, new int[]{31,57,-262});
    rules[275] = new Rule(-115, new int[]{31});
    rules[276] = new Rule(-242, new int[]{139,11,-83,12});
    rules[277] = new Rule(-213, new int[]{-211});
    rules[278] = new Rule(-211, new int[]{-210});
    rules[279] = new Rule(-210, new int[]{43,-113});
    rules[280] = new Rule(-210, new int[]{34,-113,5,-261});
    rules[281] = new Rule(-210, new int[]{-167,123,-265});
    rules[282] = new Rule(-210, new int[]{-289,123,-265});
    rules[283] = new Rule(-210, new int[]{8,9,123,-265});
    rules[284] = new Rule(-210, new int[]{8,-74,9,123,-265});
    rules[285] = new Rule(-210, new int[]{-167,123,8,9});
    rules[286] = new Rule(-210, new int[]{-289,123,8,9});
    rules[287] = new Rule(-210, new int[]{8,9,123,8,9});
    rules[288] = new Rule(-210, new int[]{8,-74,9,123,8,9});
    rules[289] = new Rule(-27, new int[]{-20,-277,-170,-304,-23});
    rules[290] = new Rule(-28, new int[]{47,-170,-304,-22,88});
    rules[291] = new Rule(-19, new int[]{68});
    rules[292] = new Rule(-19, new int[]{69});
    rules[293] = new Rule(-19, new int[]{143});
    rules[294] = new Rule(-19, new int[]{24});
    rules[295] = new Rule(-19, new int[]{25});
    rules[296] = new Rule(-20, new int[]{});
    rules[297] = new Rule(-20, new int[]{-21});
    rules[298] = new Rule(-21, new int[]{-19});
    rules[299] = new Rule(-21, new int[]{-21,-19});
    rules[300] = new Rule(-277, new int[]{23});
    rules[301] = new Rule(-277, new int[]{40});
    rules[302] = new Rule(-277, new int[]{63});
    rules[303] = new Rule(-277, new int[]{63,23});
    rules[304] = new Rule(-277, new int[]{63,47});
    rules[305] = new Rule(-277, new int[]{63,40});
    rules[306] = new Rule(-23, new int[]{});
    rules[307] = new Rule(-23, new int[]{-22,88});
    rules[308] = new Rule(-170, new int[]{});
    rules[309] = new Rule(-170, new int[]{8,-169,9});
    rules[310] = new Rule(-169, new int[]{-168});
    rules[311] = new Rule(-169, new int[]{-169,96,-168});
    rules[312] = new Rule(-168, new int[]{-167});
    rules[313] = new Rule(-168, new int[]{-289});
    rules[314] = new Rule(-168, new int[]{-138});
    rules[315] = new Rule(-141, new int[]{119,-144,117});
    rules[316] = new Rule(-304, new int[]{});
    rules[317] = new Rule(-304, new int[]{-303});
    rules[318] = new Rule(-303, new int[]{-302});
    rules[319] = new Rule(-303, new int[]{-303,-302});
    rules[320] = new Rule(-302, new int[]{20,-144,5,-274,10});
    rules[321] = new Rule(-302, new int[]{20,-138,10});
    rules[322] = new Rule(-274, new int[]{-271});
    rules[323] = new Rule(-274, new int[]{-274,96,-271});
    rules[324] = new Rule(-271, new int[]{-262});
    rules[325] = new Rule(-271, new int[]{23});
    rules[326] = new Rule(-271, new int[]{47});
    rules[327] = new Rule(-271, new int[]{27});
    rules[328] = new Rule(-22, new int[]{-29});
    rules[329] = new Rule(-22, new int[]{-22,-7,-29});
    rules[330] = new Rule(-7, new int[]{81});
    rules[331] = new Rule(-7, new int[]{80});
    rules[332] = new Rule(-7, new int[]{79});
    rules[333] = new Rule(-7, new int[]{78});
    rules[334] = new Rule(-29, new int[]{});
    rules[335] = new Rule(-29, new int[]{-31,-177});
    rules[336] = new Rule(-29, new int[]{-30});
    rules[337] = new Rule(-29, new int[]{-31,10,-30});
    rules[338] = new Rule(-144, new int[]{-132});
    rules[339] = new Rule(-144, new int[]{-144,96,-132});
    rules[340] = new Rule(-177, new int[]{});
    rules[341] = new Rule(-177, new int[]{10});
    rules[342] = new Rule(-31, new int[]{-41});
    rules[343] = new Rule(-31, new int[]{-31,10,-41});
    rules[344] = new Rule(-41, new int[]{-6,-47});
    rules[345] = new Rule(-30, new int[]{-50});
    rules[346] = new Rule(-30, new int[]{-30,-50});
    rules[347] = new Rule(-50, new int[]{-49});
    rules[348] = new Rule(-50, new int[]{-51});
    rules[349] = new Rule(-47, new int[]{26,-25});
    rules[350] = new Rule(-47, new int[]{-299});
    rules[351] = new Rule(-47, new int[]{-3,-299});
    rules[352] = new Rule(-3, new int[]{25});
    rules[353] = new Rule(-3, new int[]{23});
    rules[354] = new Rule(-299, new int[]{-298});
    rules[355] = new Rule(-299, new int[]{61,-144,5,-262});
    rules[356] = new Rule(-49, new int[]{-6,-209});
    rules[357] = new Rule(-49, new int[]{-6,-206});
    rules[358] = new Rule(-206, new int[]{-202});
    rules[359] = new Rule(-206, new int[]{-205});
    rules[360] = new Rule(-209, new int[]{-3,-217});
    rules[361] = new Rule(-209, new int[]{-217});
    rules[362] = new Rule(-209, new int[]{-214});
    rules[363] = new Rule(-217, new int[]{-215});
    rules[364] = new Rule(-215, new int[]{-212});
    rules[365] = new Rule(-215, new int[]{-216});
    rules[366] = new Rule(-214, new int[]{27,-158,-113,-194});
    rules[367] = new Rule(-214, new int[]{-3,27,-158,-113,-194});
    rules[368] = new Rule(-214, new int[]{28,-158,-113,-194});
    rules[369] = new Rule(-158, new int[]{-157});
    rules[370] = new Rule(-158, new int[]{});
    rules[371] = new Rule(-159, new int[]{-132});
    rules[372] = new Rule(-159, new int[]{-135});
    rules[373] = new Rule(-159, new int[]{-159,7,-132});
    rules[374] = new Rule(-159, new int[]{-159,7,-135});
    rules[375] = new Rule(-51, new int[]{-6,-244});
    rules[376] = new Rule(-244, new int[]{45,-159,-220,-189,10,-192});
    rules[377] = new Rule(-244, new int[]{45,-159,-220,-189,10,-197,10,-192});
    rules[378] = new Rule(-244, new int[]{-3,45,-159,-220,-189,10,-192});
    rules[379] = new Rule(-244, new int[]{-3,45,-159,-220,-189,10,-197,10,-192});
    rules[380] = new Rule(-244, new int[]{24,45,-159,-220,-198,10});
    rules[381] = new Rule(-244, new int[]{-3,24,45,-159,-220,-198,10});
    rules[382] = new Rule(-198, new int[]{106,-81});
    rules[383] = new Rule(-198, new int[]{});
    rules[384] = new Rule(-192, new int[]{});
    rules[385] = new Rule(-192, new int[]{62,10});
    rules[386] = new Rule(-220, new int[]{-225,5,-261});
    rules[387] = new Rule(-225, new int[]{});
    rules[388] = new Rule(-225, new int[]{11,-224,12});
    rules[389] = new Rule(-224, new int[]{-223});
    rules[390] = new Rule(-224, new int[]{-224,10,-223});
    rules[391] = new Rule(-223, new int[]{-144,5,-261});
    rules[392] = new Rule(-101, new int[]{-82});
    rules[393] = new Rule(-101, new int[]{});
    rules[394] = new Rule(-189, new int[]{});
    rules[395] = new Rule(-189, new int[]{82,-101,-190});
    rules[396] = new Rule(-189, new int[]{83,-246,-191});
    rules[397] = new Rule(-190, new int[]{});
    rules[398] = new Rule(-190, new int[]{83,-246});
    rules[399] = new Rule(-191, new int[]{});
    rules[400] = new Rule(-191, new int[]{82,-101});
    rules[401] = new Rule(-297, new int[]{-298,10});
    rules[402] = new Rule(-325, new int[]{106});
    rules[403] = new Rule(-325, new int[]{116});
    rules[404] = new Rule(-298, new int[]{-144,5,-262});
    rules[405] = new Rule(-298, new int[]{-144,106,-81});
    rules[406] = new Rule(-298, new int[]{-144,5,-262,-325,-80});
    rules[407] = new Rule(-80, new int[]{-79});
    rules[408] = new Rule(-80, new int[]{-310});
    rules[409] = new Rule(-80, new int[]{-132,123,-315});
    rules[410] = new Rule(-80, new int[]{8,9,-311,123,-315});
    rules[411] = new Rule(-80, new int[]{8,-62,9,123,-315});
    rules[412] = new Rule(-79, new int[]{-78});
    rules[413] = new Rule(-79, new int[]{-53});
    rules[414] = new Rule(-204, new int[]{-214,-164});
    rules[415] = new Rule(-204, new int[]{27,-158,-113,106,-246,10});
    rules[416] = new Rule(-204, new int[]{-3,27,-158,-113,106,-246,10});
    rules[417] = new Rule(-205, new int[]{-214,-163});
    rules[418] = new Rule(-205, new int[]{27,-158,-113,106,-246,10});
    rules[419] = new Rule(-205, new int[]{-3,27,-158,-113,106,-246,10});
    rules[420] = new Rule(-201, new int[]{-208});
    rules[421] = new Rule(-201, new int[]{-3,-208});
    rules[422] = new Rule(-208, new int[]{-215,-165});
    rules[423] = new Rule(-208, new int[]{34,-156,-113,5,-261,-195,106,-91,10});
    rules[424] = new Rule(-208, new int[]{34,-156,-113,-195,106,-91,10});
    rules[425] = new Rule(-208, new int[]{34,-156,-113,5,-261,-195,106,-309,10});
    rules[426] = new Rule(-208, new int[]{34,-156,-113,-195,106,-309,10});
    rules[427] = new Rule(-208, new int[]{43,-157,-113,-195,106,-246,10});
    rules[428] = new Rule(-208, new int[]{-215,144,10});
    rules[429] = new Rule(-202, new int[]{-203});
    rules[430] = new Rule(-202, new int[]{-3,-203});
    rules[431] = new Rule(-203, new int[]{-215,-163});
    rules[432] = new Rule(-203, new int[]{34,-156,-113,5,-261,-195,106,-92,10});
    rules[433] = new Rule(-203, new int[]{34,-156,-113,-195,106,-92,10});
    rules[434] = new Rule(-203, new int[]{43,-157,-113,-195,106,-246,10});
    rules[435] = new Rule(-165, new int[]{-164});
    rules[436] = new Rule(-165, new int[]{-57});
    rules[437] = new Rule(-157, new int[]{-156});
    rules[438] = new Rule(-156, new int[]{-127});
    rules[439] = new Rule(-156, new int[]{-321,7,-127});
    rules[440] = new Rule(-134, new int[]{-122});
    rules[441] = new Rule(-321, new int[]{-134});
    rules[442] = new Rule(-321, new int[]{-321,7,-134});
    rules[443] = new Rule(-127, new int[]{-122});
    rules[444] = new Rule(-127, new int[]{-178});
    rules[445] = new Rule(-127, new int[]{-178,-141});
    rules[446] = new Rule(-122, new int[]{-119});
    rules[447] = new Rule(-122, new int[]{-119,-141});
    rules[448] = new Rule(-119, new int[]{-132});
    rules[449] = new Rule(-212, new int[]{43,-157,-113,-194,-304});
    rules[450] = new Rule(-216, new int[]{34,-156,-113,-194,-304});
    rules[451] = new Rule(-216, new int[]{34,-156,-113,5,-261,-194,-304});
    rules[452] = new Rule(-57, new int[]{103,-96,77,-96,10});
    rules[453] = new Rule(-57, new int[]{103,-96,10});
    rules[454] = new Rule(-57, new int[]{103,10});
    rules[455] = new Rule(-96, new int[]{-132});
    rules[456] = new Rule(-96, new int[]{-151});
    rules[457] = new Rule(-164, new int[]{-38,-241,10});
    rules[458] = new Rule(-163, new int[]{-40,-241,10});
    rules[459] = new Rule(-163, new int[]{-57});
    rules[460] = new Rule(-113, new int[]{});
    rules[461] = new Rule(-113, new int[]{8,9});
    rules[462] = new Rule(-113, new int[]{8,-114,9});
    rules[463] = new Rule(-114, new int[]{-52});
    rules[464] = new Rule(-114, new int[]{-114,10,-52});
    rules[465] = new Rule(-52, new int[]{-6,-282});
    rules[466] = new Rule(-282, new int[]{-145,5,-261});
    rules[467] = new Rule(-282, new int[]{52,-145,5,-261});
    rules[468] = new Rule(-282, new int[]{26,-145,5,-261});
    rules[469] = new Rule(-282, new int[]{104,-145,5,-261});
    rules[470] = new Rule(-282, new int[]{-145,5,-261,106,-81});
    rules[471] = new Rule(-282, new int[]{52,-145,5,-261,106,-81});
    rules[472] = new Rule(-282, new int[]{26,-145,5,-261,106,-81});
    rules[473] = new Rule(-145, new int[]{-120});
    rules[474] = new Rule(-145, new int[]{-145,96,-120});
    rules[475] = new Rule(-120, new int[]{-132});
    rules[476] = new Rule(-261, new int[]{-262});
    rules[477] = new Rule(-263, new int[]{-258});
    rules[478] = new Rule(-263, new int[]{-242});
    rules[479] = new Rule(-263, new int[]{-235});
    rules[480] = new Rule(-263, new int[]{-267});
    rules[481] = new Rule(-263, new int[]{-289});
    rules[482] = new Rule(-247, new int[]{-246});
    rules[483] = new Rule(-247, new int[]{-128,5,-247});
    rules[484] = new Rule(-246, new int[]{});
    rules[485] = new Rule(-246, new int[]{-4});
    rules[486] = new Rule(-246, new int[]{-199});
    rules[487] = new Rule(-246, new int[]{-118});
    rules[488] = new Rule(-246, new int[]{-241});
    rules[489] = new Rule(-246, new int[]{-139});
    rules[490] = new Rule(-246, new int[]{-32});
    rules[491] = new Rule(-246, new int[]{-233});
    rules[492] = new Rule(-246, new int[]{-305});
    rules[493] = new Rule(-246, new int[]{-109});
    rules[494] = new Rule(-246, new int[]{-306});
    rules[495] = new Rule(-246, new int[]{-146});
    rules[496] = new Rule(-246, new int[]{-290});
    rules[497] = new Rule(-246, new int[]{-234});
    rules[498] = new Rule(-246, new int[]{-108});
    rules[499] = new Rule(-246, new int[]{-301});
    rules[500] = new Rule(-246, new int[]{-55});
    rules[501] = new Rule(-246, new int[]{-155});
    rules[502] = new Rule(-246, new int[]{-111});
    rules[503] = new Rule(-246, new int[]{-112});
    rules[504] = new Rule(-246, new int[]{-110});
    rules[505] = new Rule(-246, new int[]{-335});
    rules[506] = new Rule(-110, new int[]{72,-91,95,-246});
    rules[507] = new Rule(-111, new int[]{74,-91});
    rules[508] = new Rule(-112, new int[]{74,73,-91});
    rules[509] = new Rule(-301, new int[]{52,-298});
    rules[510] = new Rule(-301, new int[]{8,52,-132,96,-324,9,106,-81});
    rules[511] = new Rule(-301, new int[]{52,8,-132,96,-144,9,106,-81});
    rules[512] = new Rule(-4, new int[]{-100,-181,-82});
    rules[513] = new Rule(-4, new int[]{8,-99,96,-323,9,-181,-81});
    rules[514] = new Rule(-323, new int[]{-99});
    rules[515] = new Rule(-323, new int[]{-323,96,-99});
    rules[516] = new Rule(-324, new int[]{52,-132});
    rules[517] = new Rule(-324, new int[]{-324,96,52,-132});
    rules[518] = new Rule(-199, new int[]{-100});
    rules[519] = new Rule(-118, new int[]{56,-128});
    rules[520] = new Rule(-241, new int[]{87,-238,88});
    rules[521] = new Rule(-238, new int[]{-247});
    rules[522] = new Rule(-238, new int[]{-238,10,-247});
    rules[523] = new Rule(-139, new int[]{37,-91,50,-246});
    rules[524] = new Rule(-139, new int[]{37,-91,50,-246,29,-246});
    rules[525] = new Rule(-335, new int[]{35,-91,54,-337,-239,88});
    rules[526] = new Rule(-335, new int[]{35,-91,54,-337,10,-239,88});
    rules[527] = new Rule(-337, new int[]{-336});
    rules[528] = new Rule(-337, new int[]{-337,10,-336});
    rules[529] = new Rule(-336, new int[]{-329,36,-91,5,-246});
    rules[530] = new Rule(-336, new int[]{-328,5,-246});
    rules[531] = new Rule(-336, new int[]{-330,5,-246});
    rules[532] = new Rule(-336, new int[]{-331,5,-246});
    rules[533] = new Rule(-32, new int[]{22,-91,57,-33,-239,88});
    rules[534] = new Rule(-32, new int[]{22,-91,57,-33,10,-239,88});
    rules[535] = new Rule(-32, new int[]{22,-91,57,-239,88});
    rules[536] = new Rule(-33, new int[]{-248});
    rules[537] = new Rule(-33, new int[]{-33,10,-248});
    rules[538] = new Rule(-248, new int[]{-68,5,-246});
    rules[539] = new Rule(-68, new int[]{-98});
    rules[540] = new Rule(-68, new int[]{-68,96,-98});
    rules[541] = new Rule(-98, new int[]{-86});
    rules[542] = new Rule(-239, new int[]{});
    rules[543] = new Rule(-239, new int[]{29,-238});
    rules[544] = new Rule(-233, new int[]{93,-238,94,-81});
    rules[545] = new Rule(-305, new int[]{53,-91,-278,-246});
    rules[546] = new Rule(-278, new int[]{95});
    rules[547] = new Rule(-278, new int[]{});
    rules[548] = new Rule(-155, new int[]{59,-91,95,-246});
    rules[549] = new Rule(-108, new int[]{33,-132,-260,133,-91,95,-246});
    rules[550] = new Rule(-108, new int[]{33,52,-132,5,-262,133,-91,95,-246});
    rules[551] = new Rule(-108, new int[]{33,52,-132,133,-91,95,-246});
    rules[552] = new Rule(-260, new int[]{5,-262});
    rules[553] = new Rule(-260, new int[]{});
    rules[554] = new Rule(-109, new int[]{32,-18,-132,-272,-91,-104,-91,-278,-246});
    rules[555] = new Rule(-18, new int[]{52});
    rules[556] = new Rule(-18, new int[]{});
    rules[557] = new Rule(-272, new int[]{106});
    rules[558] = new Rule(-272, new int[]{5,-167,106});
    rules[559] = new Rule(-104, new int[]{70});
    rules[560] = new Rule(-104, new int[]{71});
    rules[561] = new Rule(-306, new int[]{54,-66,95,-246});
    rules[562] = new Rule(-146, new int[]{39});
    rules[563] = new Rule(-290, new int[]{98,-238,-276});
    rules[564] = new Rule(-276, new int[]{97,-238,88});
    rules[565] = new Rule(-276, new int[]{30,-56,88});
    rules[566] = new Rule(-56, new int[]{-59,-240});
    rules[567] = new Rule(-56, new int[]{-59,10,-240});
    rules[568] = new Rule(-56, new int[]{-238});
    rules[569] = new Rule(-59, new int[]{-58});
    rules[570] = new Rule(-59, new int[]{-59,10,-58});
    rules[571] = new Rule(-240, new int[]{});
    rules[572] = new Rule(-240, new int[]{29,-238});
    rules[573] = new Rule(-58, new int[]{76,-60,95,-246});
    rules[574] = new Rule(-60, new int[]{-166});
    rules[575] = new Rule(-60, new int[]{-125,5,-166});
    rules[576] = new Rule(-166, new int[]{-167});
    rules[577] = new Rule(-125, new int[]{-132});
    rules[578] = new Rule(-234, new int[]{46});
    rules[579] = new Rule(-234, new int[]{46,-81});
    rules[580] = new Rule(-66, new int[]{-82});
    rules[581] = new Rule(-66, new int[]{-66,96,-82});
    rules[582] = new Rule(-55, new int[]{-161});
    rules[583] = new Rule(-161, new int[]{-160});
    rules[584] = new Rule(-82, new int[]{-81});
    rules[585] = new Rule(-82, new int[]{-309});
    rules[586] = new Rule(-81, new int[]{-91});
    rules[587] = new Rule(-81, new int[]{-105});
    rules[588] = new Rule(-91, new int[]{-90});
    rules[589] = new Rule(-91, new int[]{-227});
    rules[590] = new Rule(-92, new int[]{-91});
    rules[591] = new Rule(-92, new int[]{-309});
    rules[592] = new Rule(-90, new int[]{-89});
    rules[593] = new Rule(-90, new int[]{-90,16,-89});
    rules[594] = new Rule(-243, new int[]{18,8,-270,9});
    rules[595] = new Rule(-281, new int[]{19,8,-270,9});
    rules[596] = new Rule(-281, new int[]{19,8,-269,9});
    rules[597] = new Rule(-227, new int[]{-91,13,-91,5,-91});
    rules[598] = new Rule(-269, new int[]{-167,-286});
    rules[599] = new Rule(-269, new int[]{-167,4,-286});
    rules[600] = new Rule(-270, new int[]{-167});
    rules[601] = new Rule(-270, new int[]{-167,-285});
    rules[602] = new Rule(-270, new int[]{-167,4,-287});
    rules[603] = new Rule(-5, new int[]{8,-62,9});
    rules[604] = new Rule(-5, new int[]{});
    rules[605] = new Rule(-160, new int[]{75,-270,-65});
    rules[606] = new Rule(-160, new int[]{75,-270,11,-63,12,-5});
    rules[607] = new Rule(-160, new int[]{75,23,8,-320,9});
    rules[608] = new Rule(-319, new int[]{-132,106,-89});
    rules[609] = new Rule(-319, new int[]{-89});
    rules[610] = new Rule(-320, new int[]{-319});
    rules[611] = new Rule(-320, new int[]{-320,96,-319});
    rules[612] = new Rule(-65, new int[]{});
    rules[613] = new Rule(-65, new int[]{8,-63,9});
    rules[614] = new Rule(-89, new int[]{-93});
    rules[615] = new Rule(-89, new int[]{-89,-183,-93});
    rules[616] = new Rule(-89, new int[]{-252,8,-340,9});
    rules[617] = new Rule(-89, new int[]{-76,134,-330});
    rules[618] = new Rule(-89, new int[]{-76,134,-331});
    rules[619] = new Rule(-327, new int[]{-270,8,-340,9});
    rules[620] = new Rule(-329, new int[]{-270,8,-341,9});
    rules[621] = new Rule(-328, new int[]{-270,8,-341,9});
    rules[622] = new Rule(-328, new int[]{-344});
    rules[623] = new Rule(-344, new int[]{-326});
    rules[624] = new Rule(-344, new int[]{-344,96,-326});
    rules[625] = new Rule(-326, new int[]{-14});
    rules[626] = new Rule(-326, new int[]{-270});
    rules[627] = new Rule(-326, new int[]{55});
    rules[628] = new Rule(-326, new int[]{-243});
    rules[629] = new Rule(-326, new int[]{-281});
    rules[630] = new Rule(-330, new int[]{11,-342,12});
    rules[631] = new Rule(-342, new int[]{-332});
    rules[632] = new Rule(-342, new int[]{-342,96,-332});
    rules[633] = new Rule(-332, new int[]{-14});
    rules[634] = new Rule(-332, new int[]{-334});
    rules[635] = new Rule(-332, new int[]{14});
    rules[636] = new Rule(-332, new int[]{-329});
    rules[637] = new Rule(-332, new int[]{-330});
    rules[638] = new Rule(-332, new int[]{-331});
    rules[639] = new Rule(-332, new int[]{6});
    rules[640] = new Rule(-334, new int[]{52,-132});
    rules[641] = new Rule(-331, new int[]{8,-343,9});
    rules[642] = new Rule(-333, new int[]{14});
    rules[643] = new Rule(-333, new int[]{-14});
    rules[644] = new Rule(-333, new int[]{52,-132});
    rules[645] = new Rule(-333, new int[]{-329});
    rules[646] = new Rule(-333, new int[]{-330});
    rules[647] = new Rule(-333, new int[]{-331});
    rules[648] = new Rule(-343, new int[]{-333});
    rules[649] = new Rule(-343, new int[]{-343,96,-333});
    rules[650] = new Rule(-341, new int[]{-339});
    rules[651] = new Rule(-341, new int[]{-341,10,-339});
    rules[652] = new Rule(-341, new int[]{-341,96,-339});
    rules[653] = new Rule(-340, new int[]{-338});
    rules[654] = new Rule(-340, new int[]{-340,10,-338});
    rules[655] = new Rule(-340, new int[]{-340,96,-338});
    rules[656] = new Rule(-338, new int[]{14});
    rules[657] = new Rule(-338, new int[]{-14});
    rules[658] = new Rule(-338, new int[]{52,-132,5,-262});
    rules[659] = new Rule(-338, new int[]{52,-132});
    rules[660] = new Rule(-338, new int[]{-327});
    rules[661] = new Rule(-338, new int[]{-330});
    rules[662] = new Rule(-338, new int[]{-331});
    rules[663] = new Rule(-339, new int[]{14});
    rules[664] = new Rule(-339, new int[]{-14});
    rules[665] = new Rule(-339, new int[]{-132,5,-262});
    rules[666] = new Rule(-339, new int[]{-132});
    rules[667] = new Rule(-339, new int[]{52,-132,5,-262});
    rules[668] = new Rule(-339, new int[]{52,-132});
    rules[669] = new Rule(-339, new int[]{-329});
    rules[670] = new Rule(-339, new int[]{-330});
    rules[671] = new Rule(-339, new int[]{-331});
    rules[672] = new Rule(-102, new int[]{-93});
    rules[673] = new Rule(-102, new int[]{});
    rules[674] = new Rule(-107, new int[]{-83});
    rules[675] = new Rule(-107, new int[]{});
    rules[676] = new Rule(-105, new int[]{-93,5,-102});
    rules[677] = new Rule(-105, new int[]{5,-102});
    rules[678] = new Rule(-105, new int[]{-93,5,-102,5,-93});
    rules[679] = new Rule(-105, new int[]{5,-102,5,-93});
    rules[680] = new Rule(-106, new int[]{-83,5,-107});
    rules[681] = new Rule(-106, new int[]{5,-107});
    rules[682] = new Rule(-106, new int[]{-83,5,-107,5,-83});
    rules[683] = new Rule(-106, new int[]{5,-107,5,-83});
    rules[684] = new Rule(-183, new int[]{116});
    rules[685] = new Rule(-183, new int[]{121});
    rules[686] = new Rule(-183, new int[]{119});
    rules[687] = new Rule(-183, new int[]{117});
    rules[688] = new Rule(-183, new int[]{120});
    rules[689] = new Rule(-183, new int[]{118});
    rules[690] = new Rule(-183, new int[]{133});
    rules[691] = new Rule(-93, new int[]{-76});
    rules[692] = new Rule(-93, new int[]{-93,-184,-76});
    rules[693] = new Rule(-184, new int[]{112});
    rules[694] = new Rule(-184, new int[]{111});
    rules[695] = new Rule(-184, new int[]{124});
    rules[696] = new Rule(-184, new int[]{125});
    rules[697] = new Rule(-184, new int[]{122});
    rules[698] = new Rule(-188, new int[]{132});
    rules[699] = new Rule(-188, new int[]{134});
    rules[700] = new Rule(-250, new int[]{-252});
    rules[701] = new Rule(-250, new int[]{-253});
    rules[702] = new Rule(-253, new int[]{-76,132,-270});
    rules[703] = new Rule(-252, new int[]{-76,134,-270});
    rules[704] = new Rule(-77, new int[]{-88});
    rules[705] = new Rule(-254, new int[]{-77,115,-88});
    rules[706] = new Rule(-76, new int[]{-88});
    rules[707] = new Rule(-76, new int[]{-160});
    rules[708] = new Rule(-76, new int[]{-254});
    rules[709] = new Rule(-76, new int[]{-76,-185,-88});
    rules[710] = new Rule(-76, new int[]{-76,-185,-254});
    rules[711] = new Rule(-76, new int[]{-250});
    rules[712] = new Rule(-185, new int[]{114});
    rules[713] = new Rule(-185, new int[]{113});
    rules[714] = new Rule(-185, new int[]{127});
    rules[715] = new Rule(-185, new int[]{128});
    rules[716] = new Rule(-185, new int[]{129});
    rules[717] = new Rule(-185, new int[]{130});
    rules[718] = new Rule(-185, new int[]{126});
    rules[719] = new Rule(-53, new int[]{62,8,-270,9});
    rules[720] = new Rule(-54, new int[]{8,-91,96,-73,-311,-318,9});
    rules[721] = new Rule(-88, new int[]{55});
    rules[722] = new Rule(-88, new int[]{-14});
    rules[723] = new Rule(-88, new int[]{-53});
    rules[724] = new Rule(-88, new int[]{11,-64,12});
    rules[725] = new Rule(-88, new int[]{131,-88});
    rules[726] = new Rule(-88, new int[]{-186,-88});
    rules[727] = new Rule(-88, new int[]{-100});
    rules[728] = new Rule(-88, new int[]{-54});
    rules[729] = new Rule(-14, new int[]{-151});
    rules[730] = new Rule(-14, new int[]{-15});
    rules[731] = new Rule(-103, new int[]{-99,15,-99});
    rules[732] = new Rule(-103, new int[]{-99,15,-103});
    rules[733] = new Rule(-100, new int[]{-117,-99});
    rules[734] = new Rule(-100, new int[]{-99});
    rules[735] = new Rule(-100, new int[]{-103});
    rules[736] = new Rule(-117, new int[]{137});
    rules[737] = new Rule(-117, new int[]{-117,137});
    rules[738] = new Rule(-9, new int[]{-167,-65});
    rules[739] = new Rule(-9, new int[]{-289,-65});
    rules[740] = new Rule(-308, new int[]{-132});
    rules[741] = new Rule(-308, new int[]{-308,7,-123});
    rules[742] = new Rule(-307, new int[]{-308});
    rules[743] = new Rule(-307, new int[]{-308,-285});
    rules[744] = new Rule(-16, new int[]{-99});
    rules[745] = new Rule(-16, new int[]{-14});
    rules[746] = new Rule(-99, new int[]{-132});
    rules[747] = new Rule(-99, new int[]{-178});
    rules[748] = new Rule(-99, new int[]{39,-132});
    rules[749] = new Rule(-99, new int[]{8,-81,9});
    rules[750] = new Rule(-99, new int[]{-243});
    rules[751] = new Rule(-99, new int[]{-281});
    rules[752] = new Rule(-99, new int[]{-14,7,-123});
    rules[753] = new Rule(-99, new int[]{-16,11,-66,12});
    rules[754] = new Rule(-99, new int[]{-99,17,-105,12});
    rules[755] = new Rule(-99, new int[]{-99,8,-63,9});
    rules[756] = new Rule(-99, new int[]{-99,7,-133});
    rules[757] = new Rule(-99, new int[]{-54,7,-133});
    rules[758] = new Rule(-99, new int[]{-99,138});
    rules[759] = new Rule(-99, new int[]{-99,4,-287});
    rules[760] = new Rule(-63, new int[]{-66});
    rules[761] = new Rule(-63, new int[]{});
    rules[762] = new Rule(-64, new int[]{-71});
    rules[763] = new Rule(-64, new int[]{});
    rules[764] = new Rule(-71, new int[]{-84});
    rules[765] = new Rule(-71, new int[]{-71,96,-84});
    rules[766] = new Rule(-84, new int[]{-81});
    rules[767] = new Rule(-84, new int[]{-81,6,-81});
    rules[768] = new Rule(-152, new int[]{140});
    rules[769] = new Rule(-152, new int[]{142});
    rules[770] = new Rule(-151, new int[]{-153});
    rules[771] = new Rule(-151, new int[]{141});
    rules[772] = new Rule(-153, new int[]{-152});
    rules[773] = new Rule(-153, new int[]{-153,-152});
    rules[774] = new Rule(-178, new int[]{44,-187});
    rules[775] = new Rule(-194, new int[]{10});
    rules[776] = new Rule(-194, new int[]{10,-193,10});
    rules[777] = new Rule(-195, new int[]{});
    rules[778] = new Rule(-195, new int[]{10,-193});
    rules[779] = new Rule(-193, new int[]{-196});
    rules[780] = new Rule(-193, new int[]{-193,10,-196});
    rules[781] = new Rule(-132, new int[]{139});
    rules[782] = new Rule(-132, new int[]{-136});
    rules[783] = new Rule(-132, new int[]{-137});
    rules[784] = new Rule(-123, new int[]{-132});
    rules[785] = new Rule(-123, new int[]{-279});
    rules[786] = new Rule(-123, new int[]{-280});
    rules[787] = new Rule(-133, new int[]{-132});
    rules[788] = new Rule(-133, new int[]{-279});
    rules[789] = new Rule(-133, new int[]{-178});
    rules[790] = new Rule(-196, new int[]{143});
    rules[791] = new Rule(-196, new int[]{145});
    rules[792] = new Rule(-196, new int[]{146});
    rules[793] = new Rule(-196, new int[]{147});
    rules[794] = new Rule(-196, new int[]{149});
    rules[795] = new Rule(-196, new int[]{148});
    rules[796] = new Rule(-197, new int[]{148});
    rules[797] = new Rule(-197, new int[]{147});
    rules[798] = new Rule(-197, new int[]{143});
    rules[799] = new Rule(-136, new int[]{82});
    rules[800] = new Rule(-136, new int[]{83});
    rules[801] = new Rule(-137, new int[]{77});
    rules[802] = new Rule(-137, new int[]{75});
    rules[803] = new Rule(-135, new int[]{81});
    rules[804] = new Rule(-135, new int[]{80});
    rules[805] = new Rule(-135, new int[]{79});
    rules[806] = new Rule(-135, new int[]{78});
    rules[807] = new Rule(-279, new int[]{-135});
    rules[808] = new Rule(-279, new int[]{68});
    rules[809] = new Rule(-279, new int[]{63});
    rules[810] = new Rule(-279, new int[]{124});
    rules[811] = new Rule(-279, new int[]{19});
    rules[812] = new Rule(-279, new int[]{18});
    rules[813] = new Rule(-279, new int[]{62});
    rules[814] = new Rule(-279, new int[]{20});
    rules[815] = new Rule(-279, new int[]{125});
    rules[816] = new Rule(-279, new int[]{126});
    rules[817] = new Rule(-279, new int[]{127});
    rules[818] = new Rule(-279, new int[]{128});
    rules[819] = new Rule(-279, new int[]{129});
    rules[820] = new Rule(-279, new int[]{130});
    rules[821] = new Rule(-279, new int[]{131});
    rules[822] = new Rule(-279, new int[]{132});
    rules[823] = new Rule(-279, new int[]{133});
    rules[824] = new Rule(-279, new int[]{134});
    rules[825] = new Rule(-279, new int[]{21});
    rules[826] = new Rule(-279, new int[]{73});
    rules[827] = new Rule(-279, new int[]{87});
    rules[828] = new Rule(-279, new int[]{22});
    rules[829] = new Rule(-279, new int[]{23});
    rules[830] = new Rule(-279, new int[]{26});
    rules[831] = new Rule(-279, new int[]{27});
    rules[832] = new Rule(-279, new int[]{28});
    rules[833] = new Rule(-279, new int[]{71});
    rules[834] = new Rule(-279, new int[]{95});
    rules[835] = new Rule(-279, new int[]{29});
    rules[836] = new Rule(-279, new int[]{88});
    rules[837] = new Rule(-279, new int[]{30});
    rules[838] = new Rule(-279, new int[]{31});
    rules[839] = new Rule(-279, new int[]{24});
    rules[840] = new Rule(-279, new int[]{100});
    rules[841] = new Rule(-279, new int[]{97});
    rules[842] = new Rule(-279, new int[]{32});
    rules[843] = new Rule(-279, new int[]{33});
    rules[844] = new Rule(-279, new int[]{34});
    rules[845] = new Rule(-279, new int[]{37});
    rules[846] = new Rule(-279, new int[]{38});
    rules[847] = new Rule(-279, new int[]{39});
    rules[848] = new Rule(-279, new int[]{99});
    rules[849] = new Rule(-279, new int[]{40});
    rules[850] = new Rule(-279, new int[]{43});
    rules[851] = new Rule(-279, new int[]{45});
    rules[852] = new Rule(-279, new int[]{46});
    rules[853] = new Rule(-279, new int[]{47});
    rules[854] = new Rule(-279, new int[]{93});
    rules[855] = new Rule(-279, new int[]{48});
    rules[856] = new Rule(-279, new int[]{98});
    rules[857] = new Rule(-279, new int[]{49});
    rules[858] = new Rule(-279, new int[]{25});
    rules[859] = new Rule(-279, new int[]{50});
    rules[860] = new Rule(-279, new int[]{70});
    rules[861] = new Rule(-279, new int[]{94});
    rules[862] = new Rule(-279, new int[]{51});
    rules[863] = new Rule(-279, new int[]{52});
    rules[864] = new Rule(-279, new int[]{53});
    rules[865] = new Rule(-279, new int[]{54});
    rules[866] = new Rule(-279, new int[]{55});
    rules[867] = new Rule(-279, new int[]{56});
    rules[868] = new Rule(-279, new int[]{57});
    rules[869] = new Rule(-279, new int[]{58});
    rules[870] = new Rule(-279, new int[]{60});
    rules[871] = new Rule(-279, new int[]{101});
    rules[872] = new Rule(-279, new int[]{102});
    rules[873] = new Rule(-279, new int[]{105});
    rules[874] = new Rule(-279, new int[]{103});
    rules[875] = new Rule(-279, new int[]{104});
    rules[876] = new Rule(-279, new int[]{61});
    rules[877] = new Rule(-279, new int[]{74});
    rules[878] = new Rule(-279, new int[]{35});
    rules[879] = new Rule(-279, new int[]{36});
    rules[880] = new Rule(-279, new int[]{42});
    rules[881] = new Rule(-280, new int[]{44});
    rules[882] = new Rule(-187, new int[]{111});
    rules[883] = new Rule(-187, new int[]{112});
    rules[884] = new Rule(-187, new int[]{113});
    rules[885] = new Rule(-187, new int[]{114});
    rules[886] = new Rule(-187, new int[]{116});
    rules[887] = new Rule(-187, new int[]{117});
    rules[888] = new Rule(-187, new int[]{118});
    rules[889] = new Rule(-187, new int[]{119});
    rules[890] = new Rule(-187, new int[]{120});
    rules[891] = new Rule(-187, new int[]{121});
    rules[892] = new Rule(-187, new int[]{124});
    rules[893] = new Rule(-187, new int[]{125});
    rules[894] = new Rule(-187, new int[]{126});
    rules[895] = new Rule(-187, new int[]{127});
    rules[896] = new Rule(-187, new int[]{128});
    rules[897] = new Rule(-187, new int[]{129});
    rules[898] = new Rule(-187, new int[]{130});
    rules[899] = new Rule(-187, new int[]{131});
    rules[900] = new Rule(-187, new int[]{133});
    rules[901] = new Rule(-187, new int[]{135});
    rules[902] = new Rule(-187, new int[]{136});
    rules[903] = new Rule(-187, new int[]{-181});
    rules[904] = new Rule(-187, new int[]{115});
    rules[905] = new Rule(-181, new int[]{106});
    rules[906] = new Rule(-181, new int[]{107});
    rules[907] = new Rule(-181, new int[]{108});
    rules[908] = new Rule(-181, new int[]{109});
    rules[909] = new Rule(-181, new int[]{110});
    rules[910] = new Rule(-309, new int[]{-132,123,-315});
    rules[911] = new Rule(-309, new int[]{8,9,-312,123,-315});
    rules[912] = new Rule(-309, new int[]{8,-132,5,-261,9,-312,123,-315});
    rules[913] = new Rule(-309, new int[]{8,-132,10,-313,9,-312,123,-315});
    rules[914] = new Rule(-309, new int[]{8,-132,5,-261,10,-313,9,-312,123,-315});
    rules[915] = new Rule(-309, new int[]{8,-91,96,-73,-311,-318,9,-322});
    rules[916] = new Rule(-309, new int[]{-310});
    rules[917] = new Rule(-318, new int[]{});
    rules[918] = new Rule(-318, new int[]{10,-313});
    rules[919] = new Rule(-322, new int[]{-312,123,-315});
    rules[920] = new Rule(-310, new int[]{34,-311,123,-315});
    rules[921] = new Rule(-310, new int[]{34,8,9,-311,123,-315});
    rules[922] = new Rule(-310, new int[]{34,8,-313,9,-311,123,-315});
    rules[923] = new Rule(-310, new int[]{43,123,-316});
    rules[924] = new Rule(-310, new int[]{43,8,9,123,-316});
    rules[925] = new Rule(-310, new int[]{43,8,-313,9,123,-316});
    rules[926] = new Rule(-313, new int[]{-314});
    rules[927] = new Rule(-313, new int[]{-313,10,-314});
    rules[928] = new Rule(-314, new int[]{-144,-311});
    rules[929] = new Rule(-311, new int[]{});
    rules[930] = new Rule(-311, new int[]{5,-261});
    rules[931] = new Rule(-312, new int[]{});
    rules[932] = new Rule(-312, new int[]{5,-263});
    rules[933] = new Rule(-317, new int[]{-241});
    rules[934] = new Rule(-317, new int[]{-139});
    rules[935] = new Rule(-317, new int[]{-305});
    rules[936] = new Rule(-317, new int[]{-233});
    rules[937] = new Rule(-317, new int[]{-109});
    rules[938] = new Rule(-317, new int[]{-108});
    rules[939] = new Rule(-317, new int[]{-110});
    rules[940] = new Rule(-317, new int[]{-32});
    rules[941] = new Rule(-317, new int[]{-290});
    rules[942] = new Rule(-317, new int[]{-155});
    rules[943] = new Rule(-317, new int[]{-234});
    rules[944] = new Rule(-317, new int[]{-111});
    rules[945] = new Rule(-315, new int[]{-92});
    rules[946] = new Rule(-315, new int[]{-317});
    rules[947] = new Rule(-316, new int[]{-199});
    rules[948] = new Rule(-316, new int[]{-4});
    rules[949] = new Rule(-316, new int[]{-317});
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
      case 162: // const_variable -> const_variable, tkAmpersend, 
                //                   template_type_or_typeclass_params
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
      case 205: // simple_type_decl -> typeclass_restriction, tkEqual, tkTypeclass, 
                //                     optional_base_classes, optional_component_list_seq_end, 
                //                     tkSemiColon
{
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-6].id as typeclass_restriction, new typeclass_definition(ValueStack[ValueStack.Depth-3].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as class_body_list, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 206: // simple_type_decl -> typeclass_restriction, tkEqual, tkInstance, 
                //                     optional_component_list_seq_end, tkSemiColon
{
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-5].id as typeclass_restriction, new instance_definition(ValueStack[ValueStack.Depth-2].stn as class_body_list, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 207: // typeclass_restriction -> simple_type_identifier, typeclass_params
{
			CurrentSemanticValue.id = new typeclass_restriction((ValueStack[ValueStack.Depth-2].td as named_type_reference).ToString(), ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
		}
        break;
      case 208: // typeclass_params -> tkSquareOpen, template_param_list, tkSquareClose
{
			CurrentSemanticValue.stn = new typeclass_param_list(ValueStack[ValueStack.Depth-2].stn as template_param_list);
		}
        break;
      case 209: // template_type_or_typeclass_params -> template_type_params
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 210: // template_type_or_typeclass_params -> typeclass_params
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 211: // type_decl_identifier -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 212: // type_decl_identifier -> identifier, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 213: // template_identifier_with_equal -> identifier, tkLower, ident_list, 
                //                                   tkGreaterEqual
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-4].id.name, ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 214: // type_decl_type -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 215: // type_decl_type -> object_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 216: // simple_type_question -> simple_type, tkQuestion
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
      case 217: // simple_type_question -> template_type, tkQuestion
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
      case 218: // type_ref -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 219: // type_ref -> simple_type_question
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 220: // type_ref -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 221: // type_ref -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 222: // type_ref -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 223: // type_ref -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 224: // type_ref -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 225: // template_type -> simple_type_identifier, template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference(ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan); 
		}
        break;
      case 226: // template_type_params -> tkLower, template_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 227: // template_type_empty_params -> tkNotEqual
{
            var ntr = new named_type_reference(new ident(""), CurrentLocationSpan);
            
			CurrentSemanticValue.stn = new template_param_list(ntr, CurrentLocationSpan);
            ntr.source_context = new SourceContext(CurrentSemanticValue.stn.source_context.end_position.line_num, CurrentSemanticValue.stn.source_context.end_position.column_num, CurrentSemanticValue.stn.source_context.begin_position.line_num, CurrentSemanticValue.stn.source_context.begin_position.column_num);
		}
        break;
      case 228: // template_type_empty_params -> tkLower, template_empty_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 229: // template_param_list -> template_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 230: // template_param_list -> template_param_list, tkComma, template_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 231: // template_empty_param_list -> template_empty_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 232: // template_empty_param_list -> template_empty_param_list, tkComma, 
                //                              template_empty_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 233: // template_empty_param -> /* empty */
{ 
            CurrentSemanticValue.td = new named_type_reference(new ident(""), CurrentLocationSpan);
        }
        break;
      case 234: // template_param -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 235: // template_param -> simple_type, tkQuestion
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
      case 236: // template_param -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 237: // template_param -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 238: // template_param -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 239: // simple_type -> range_expr
{
	    	CurrentSemanticValue.td = parsertools.ConvertDotNodeOrIdentToNamedTypeReference(ValueStack[ValueStack.Depth-1].ex); 
	    }
        break;
      case 240: // simple_type -> range_expr, tkDotDot, range_expr
{ 
			CurrentSemanticValue.td = new diapason(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 241: // simple_type -> tkRoundOpen, enumeration_id_list, tkRoundClose
{ 
			CurrentSemanticValue.td = new enum_type_definition(ValueStack[ValueStack.Depth-2].stn as enumerator_list, CurrentLocationSpan);  
		}
        break;
      case 242: // range_expr -> range_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 243: // range_expr -> range_expr, const_addop, range_term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 244: // range_term -> range_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 245: // range_term -> range_term, const_mulop, range_factor
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 246: // range_factor -> simple_type_identifier
{ 
			CurrentSemanticValue.ex = parsertools.ConvertNamedTypeReferenceToDotNodeOrIdent(ValueStack[ValueStack.Depth-1].td as named_type_reference);
        }
        break;
      case 247: // range_factor -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 248: // range_factor -> sign, range_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 249: // range_factor -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 250: // range_factor -> range_factor, tkRoundOpen, const_elem_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value, ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 251: // simple_type_identifier -> identifier
{ 
			CurrentSemanticValue.td = new named_type_reference(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 252: // simple_type_identifier -> simple_type_identifier, tkPoint, 
                //                           identifier_or_keyword
{ 
			CurrentSemanticValue.td = (ValueStack[ValueStack.Depth-3].td as named_type_reference).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 253: // enumeration_id_list -> enumeration_id, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = new enumerator_list(ValueStack[ValueStack.Depth-3].stn as enumerator, CurrentLocationSpan);
			(CurrentSemanticValue.stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 254: // enumeration_id_list -> enumeration_id_list, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 255: // enumeration_id -> type_ref
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan); 
		}
        break;
      case 256: // enumeration_id -> type_ref, tkEqual, expr
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 257: // pointer_type -> tkDeref, fptype
{ 
			CurrentSemanticValue.td = new ref_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 258: // structured_type -> unpacked_structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 259: // structured_type -> tkPacked, unpacked_structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 260: // unpacked_structured_type -> array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 261: // unpacked_structured_type -> record_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 262: // unpacked_structured_type -> set_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 263: // unpacked_structured_type -> file_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 264: // unpacked_structured_type -> sequence_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 265: // sequence_type -> tkSequence, tkOf, type_ref
{
			CurrentSemanticValue.td = new sequence_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 266: // array_type -> tkArray, tkSquareOpen, simple_type_list, tkSquareClose, tkOf, 
                //               type_ref
{ 
			CurrentSemanticValue.td = new array_type(ValueStack[ValueStack.Depth-4].stn as indexers_types, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 267: // array_type -> unsized_array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 268: // unsized_array_type -> tkArray, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new array_type(null, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 269: // simple_type_list -> simple_type_or_
{ 
			CurrentSemanticValue.stn = new indexers_types(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 270: // simple_type_list -> simple_type_list, tkComma, simple_type_or_
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as indexers_types).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 271: // simple_type_or_ -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 272: // simple_type_or_ -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 273: // set_type -> tkSet, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new set_type_definition(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 274: // file_type -> tkFile, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new file_type(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 275: // file_type -> tkFile
{ 
			CurrentSemanticValue.td = new file_type();  
			CurrentSemanticValue.td.source_context = CurrentLocationSpan;
		}
        break;
      case 276: // string_type -> tkIdentifier, tkSquareOpen, const_expr, tkSquareClose
{ 
			CurrentSemanticValue.td = new string_num_definition(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-4].id, CurrentLocationSpan);
		}
        break;
      case 277: // procedural_type -> procedural_type_kind
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 278: // procedural_type_kind -> proc_type_decl
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 279: // proc_type_decl -> tkProcedure, fp_list
{ 
			CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-1].stn as formal_parameters,null,null,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 280: // proc_type_decl -> tkFunction, fp_list, tkColon, fptype
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, null, null, null, ValueStack[ValueStack.Depth-1].td as type_definition, CurrentLocationSpan);
        }
        break;
      case 281: // proc_type_decl -> simple_type_identifier, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 282: // proc_type_decl -> template_type, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 283: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 284: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-4].stn as enumerator_list,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 285: // proc_type_decl -> simple_type_identifier, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 286: // proc_type_decl -> template_type, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 287: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,null,CurrentLocationSpan);
    	}
        break;
      case 288: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-5].stn as enumerator_list,null,CurrentLocationSpan);
    	}
        break;
      case 289: // object_type -> class_attributes, class_or_interface_keyword, 
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
      case 290: // record_type -> tkRecord, optional_base_classes, optional_where_section, 
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
      case 291: // class_attribute -> tkSealed
{ CurrentSemanticValue.ob = class_attribute.Sealed; }
        break;
      case 292: // class_attribute -> tkPartial
{ CurrentSemanticValue.ob = class_attribute.Partial; }
        break;
      case 293: // class_attribute -> tkAbstract
{ CurrentSemanticValue.ob = class_attribute.Abstract; }
        break;
      case 294: // class_attribute -> tkAuto
{ CurrentSemanticValue.ob = class_attribute.Auto; }
        break;
      case 295: // class_attribute -> tkStatic
{ CurrentSemanticValue.ob = class_attribute.Static; }
        break;
      case 296: // class_attributes -> /* empty */
{ 
			CurrentSemanticValue.ob = class_attribute.None; 
		}
        break;
      case 297: // class_attributes -> class_attributes1
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 298: // class_attributes1 -> class_attribute
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 299: // class_attributes1 -> class_attributes1, class_attribute
{
            if (((class_attribute)ValueStack[ValueStack.Depth-2].ob & (class_attribute)ValueStack[ValueStack.Depth-1].ob) == (class_attribute)ValueStack[ValueStack.Depth-1].ob)
                parsertools.AddErrorFromResource("ATTRIBUTE_REDECLARED",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.ob  = ((class_attribute)ValueStack[ValueStack.Depth-2].ob) | ((class_attribute)ValueStack[ValueStack.Depth-1].ob);
			//$$ = $1;
		}
        break;
      case 300: // class_or_interface_keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 301: // class_or_interface_keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 302: // class_or_interface_keyword -> tkTemplate
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-1].ti);
		}
        break;
      case 303: // class_or_interface_keyword -> tkTemplate, tkClass
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "c", CurrentLocationSpan);
		}
        break;
      case 304: // class_or_interface_keyword -> tkTemplate, tkRecord
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "r", CurrentLocationSpan);
		}
        break;
      case 305: // class_or_interface_keyword -> tkTemplate, tkInterface
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "i", CurrentLocationSpan);
		}
        break;
      case 306: // optional_component_list_seq_end -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 307: // optional_component_list_seq_end -> member_list_section, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 309: // optional_base_classes -> tkRoundOpen, base_classes_names_list, tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 310: // base_classes_names_list -> base_class_name
{ 
			CurrentSemanticValue.stn = new named_type_reference_list(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 311: // base_classes_names_list -> base_classes_names_list, tkComma, base_class_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as named_type_reference_list).Add(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 312: // base_class_name -> simple_type_identifier
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 313: // base_class_name -> template_type
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 314: // base_class_name -> typeclass_restriction
{
			var names = new List<ident>();
			names.Add((ValueStack[ValueStack.Depth-1].id as typeclass_restriction).name);
			CurrentSemanticValue.stn = new typeclass_reference(null, names, (ValueStack[ValueStack.Depth-1].id as typeclass_restriction).restriction_args); }
        break;
      case 315: // template_arguments -> tkLower, ident_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 316: // optional_where_section -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 317: // optional_where_section -> where_part_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 318: // where_part_list -> where_part
{ 
			CurrentSemanticValue.stn = new where_definition_list(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 319: // where_part_list -> where_part_list, where_part
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as where_definition_list).Add(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 320: // where_part -> tkWhere, ident_list, tkColon, type_ref_and_secific_list, 
                //               tkSemiColon
{ 
			CurrentSemanticValue.stn = new where_definition(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-2].stn as where_type_specificator_list, CurrentLocationSpan); 
		}
        break;
      case 321: // where_part -> tkWhere, typeclass_restriction, tkSemiColon
{
			CurrentSemanticValue.stn = new where_typeclass_constraint(ValueStack[ValueStack.Depth-2].id as typeclass_restriction);
		}
        break;
      case 322: // type_ref_and_secific_list -> type_ref_or_secific
{ 
			CurrentSemanticValue.stn = new where_type_specificator_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 323: // type_ref_and_secific_list -> type_ref_and_secific_list, tkComma, 
                //                              type_ref_or_secific
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as where_type_specificator_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 324: // type_ref_or_secific -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 325: // type_ref_or_secific -> tkClass
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefClass, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 326: // type_ref_or_secific -> tkRecord
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 327: // type_ref_or_secific -> tkConstructor
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 328: // member_list_section -> member_list
{ 
			CurrentSemanticValue.stn = new class_body_list(ValueStack[ValueStack.Depth-1].stn as class_members, CurrentLocationSpan);
        }
        break;
      case 329: // member_list_section -> member_list_section, ot_visibility_specifier, 
                //                        member_list
{ 
		    (ValueStack[ValueStack.Depth-1].stn as class_members).access_mod = ValueStack[ValueStack.Depth-2].stn as access_modifer_node;
			(ValueStack[ValueStack.Depth-3].stn as class_body_list).Add(ValueStack[ValueStack.Depth-1].stn as class_members,CurrentLocationSpan);
			
			if ((ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks[0].Count == 0)
                (ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks.RemoveAt(0);
			
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 330: // ot_visibility_specifier -> tkInternal
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.internal_modifer, CurrentLocationSpan); }
        break;
      case 331: // ot_visibility_specifier -> tkPublic
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.public_modifer, CurrentLocationSpan); }
        break;
      case 332: // ot_visibility_specifier -> tkProtected
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.protected_modifer, CurrentLocationSpan); }
        break;
      case 333: // ot_visibility_specifier -> tkPrivate
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.private_modifer, CurrentLocationSpan); }
        break;
      case 334: // member_list -> /* empty */
{ CurrentSemanticValue.stn = new class_members(); }
        break;
      case 335: // member_list -> field_or_const_definition_list, optional_semicolon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 336: // member_list -> method_decl_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 337: // member_list -> field_or_const_definition_list, tkSemiColon, method_decl_list
{  
			(ValueStack[ValueStack.Depth-3].stn as class_members).members.AddRange((ValueStack[ValueStack.Depth-1].stn as class_members).members);
			(ValueStack[ValueStack.Depth-3].stn as class_members).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 338: // ident_list -> identifier
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 339: // ident_list -> ident_list, tkComma, identifier
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 340: // optional_semicolon -> /* empty */
{ CurrentSemanticValue.ob = null; }
        break;
      case 341: // optional_semicolon -> tkSemiColon
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 342: // field_or_const_definition_list -> field_or_const_definition
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 343: // field_or_const_definition_list -> field_or_const_definition_list, tkSemiColon, 
                //                                   field_or_const_definition
{   
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 344: // field_or_const_definition -> attribute_declarations, 
                //                              simple_field_or_const_definition
{  
		    (ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 345: // method_decl_list -> method_or_property_decl
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 346: // method_decl_list -> method_decl_list, method_or_property_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 347: // method_or_property_decl -> method_decl_withattr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 348: // method_or_property_decl -> property_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 349: // simple_field_or_const_definition -> tkConst, only_const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 350: // simple_field_or_const_definition -> field_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 351: // simple_field_or_const_definition -> class_or_static, field_definition
{ 
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).var_attr = definition_attribute.Static;
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 352: // class_or_static -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 353: // class_or_static -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 354: // field_definition -> var_decl_part
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 355: // field_definition -> tkEvent, ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, true, CurrentLocationSpan); 
        }
        break;
      case 356: // method_decl_withattr -> attribute_declarations, method_header
{  
			(ValueStack[ValueStack.Depth-1].td as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td;
        }
        break;
      case 357: // method_decl_withattr -> attribute_declarations, method_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
            if (ValueStack[ValueStack.Depth-1].stn is procedure_definition && (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
                (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
     }
        break;
      case 358: // method_decl -> inclass_proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 359: // method_decl -> inclass_constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 360: // method_header -> class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 361: // method_header -> method_procfunc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 362: // method_header -> constr_destr_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 363: // method_procfunc_header -> proc_func_header
{ 
			CurrentSemanticValue.td = NewProcfuncHeading(ValueStack[ValueStack.Depth-1].td as procedure_header);
		}
        break;
      case 364: // proc_func_header -> proc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 365: // proc_func_header -> func_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 366: // constr_destr_header -> tkConstructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 367: // constr_destr_header -> class_or_static, tkConstructor, optional_proc_name, 
                //                        fp_list, optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,true,null,null,CurrentLocationSpan);
        }
        break;
      case 368: // constr_destr_header -> tkDestructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new destructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name, false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 369: // optional_proc_name -> proc_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 370: // optional_proc_name -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 371: // qualified_identifier -> identifier
{ CurrentSemanticValue.stn = new method_name(null,null,ValueStack[ValueStack.Depth-1].id,null,CurrentLocationSpan); }
        break;
      case 372: // qualified_identifier -> visibility_specifier
{ CurrentSemanticValue.stn = new method_name(null,null,ValueStack[ValueStack.Depth-1].id,null,CurrentLocationSpan); }
        break;
      case 373: // qualified_identifier -> qualified_identifier, tkPoint, identifier
{
			CurrentSemanticValue.stn = NewQualifiedIdentifier(ValueStack[ValueStack.Depth-3].stn as method_name, ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
        }
        break;
      case 374: // qualified_identifier -> qualified_identifier, tkPoint, visibility_specifier
{
			CurrentSemanticValue.stn = NewQualifiedIdentifier(ValueStack[ValueStack.Depth-3].stn as method_name, ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
        }
        break;
      case 375: // property_definition -> attribute_declarations, simple_property_definition
{  
			CurrentSemanticValue.stn = NewPropertyDefinition(ValueStack[ValueStack.Depth-2].stn as attribute_list, ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 376: // simple_property_definition -> tkProperty, qualified_identifier, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        }
        break;
      case 377: // simple_property_definition -> tkProperty, qualified_identifier, 
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
      case 378: // simple_property_definition -> class_or_static, tkProperty, qualified_identifier, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        	(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
        }
        break;
      case 379: // simple_property_definition -> class_or_static, tkProperty, qualified_identifier, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, property_modificator, tkSemiColon, 
                //                               array_defaultproperty
{ 
			parsertools.AddErrorFromResource("STATIC_PROPERTIES_CANNOT_HAVE_ATTRBUTE_{0}",LocationStack[LocationStack.Depth-3],ValueStack[ValueStack.Depth-3].id.name);        	
        }
        break;
      case 380: // simple_property_definition -> tkAuto, tkProperty, qualified_identifier, 
                //                               property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 381: // simple_property_definition -> class_or_static, tkAuto, tkProperty, 
                //                               qualified_identifier, property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 382: // optional_property_initialization -> tkAssign, expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 383: // optional_property_initialization -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 384: // array_defaultproperty -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 385: // array_defaultproperty -> tkDefault, tkSemiColon
{ 
			CurrentSemanticValue.stn = new property_array_default();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 386: // property_interface -> property_parameter_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_interface(ValueStack[ValueStack.Depth-3].stn as property_parameter_list, ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan);
        }
        break;
      case 387: // property_parameter_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 388: // property_parameter_list -> tkSquareOpen, parameter_decl_list, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 389: // parameter_decl_list -> parameter_decl
{ 
			CurrentSemanticValue.stn = new property_parameter_list(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 390: // parameter_decl_list -> parameter_decl_list, tkSemiColon, parameter_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as property_parameter_list).Add(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 391: // parameter_decl -> ident_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_parameter(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 392: // optional_read_expr -> expr_with_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 393: // optional_read_expr -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 395: // property_specifiers -> tkRead, optional_read_expr, write_property_specifiers
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
      case 396: // property_specifiers -> tkWrite, unlabelled_stmt, read_property_specifiers
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
      case 398: // write_property_specifiers -> tkWrite, unlabelled_stmt
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
      case 400: // read_property_specifiers -> tkRead, optional_read_expr
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
      case 401: // var_decl -> var_decl_part, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 404: // var_decl_part -> ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, false, CurrentLocationSpan);
		}
        break;
      case 405: // var_decl_part -> ident_list, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, null, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan);		
		}
        break;
      case 406: // var_decl_part -> ident_list, tkColon, type_ref, tkAssignOrEqual, 
                //                  typed_var_init_expression
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan); 
		}
        break;
      case 407: // typed_var_init_expression -> typed_const_plus
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 408: // typed_var_init_expression -> expl_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 409: // typed_var_init_expression -> identifier, tkArrow, lambda_function_body
{  
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 410: // typed_var_init_expression -> tkRoundOpen, tkRoundClose, lambda_type_ref, 
                //                              tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 411: // typed_var_init_expression -> tkRoundOpen, typed_const_list, tkRoundClose, 
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
      case 412: // typed_const_plus -> typed_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 413: // typed_const_plus -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 414: // constr_destr_decl -> constr_destr_header, block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 415: // constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, tkAssign, 
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
      case 416: // constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
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
      case 417: // inclass_constr_destr_decl -> constr_destr_header, inclass_block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 418: // inclass_constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, 
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
      case 419: // inclass_constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
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
      case 420: // proc_func_decl -> proc_func_decl_noclass
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 421: // proc_func_decl -> class_or_static, proc_func_decl_noclass
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 422: // proc_func_decl_noclass -> proc_func_header, proc_func_external_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
        }
        break;
      case 423: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 424: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, expr_l1, 
                //                           tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].ex is dot_question_node)
				parsertools.AddErrorFromResource("DOT_QUECTION_IN_SHORT_FUN",LocationStack[LocationStack.Depth-2]);
	
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 425: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 426: // proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 427: // proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                           optional_method_modificators1, tkAssign, 
                //                           unlabelled_stmt, tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-2]);
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 428: // proc_func_decl_noclass -> proc_func_header, tkForward, tkSemiColon
{
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-3].td as procedure_header, null, CurrentLocationSpan);
            (CurrentSemanticValue.stn as procedure_definition).proc_header.proc_attributes.Add((procedure_attribute)ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-2].id.source_context);
		}
        break;
      case 429: // inclass_proc_func_decl -> inclass_proc_func_decl_noclass
{ 
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 430: // inclass_proc_func_decl -> class_or_static, inclass_proc_func_decl_noclass
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 431: // inclass_proc_func_decl_noclass -> proc_func_header, inclass_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
		}
        break;
      case 432: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, tkColon, 
                //                                   fptype, optional_method_modificators1, 
                //                                   tkAssign, expr_l1_func_decl_lambda, 
                //                                   tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 433: // inclass_proc_func_decl_noclass -> tkFunction, func_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   expr_l1_func_decl_lambda, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 434: // inclass_proc_func_decl_noclass -> tkProcedure, proc_name, fp_list, 
                //                                   optional_method_modificators1, tkAssign, 
                //                                   unlabelled_stmt, tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parsertools.build_tree_for_formatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 435: // proc_func_external_block -> block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 436: // proc_func_external_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 437: // proc_name -> func_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 438: // func_name -> func_meth_name_ident
{ 
			CurrentSemanticValue.stn = new method_name(null,null, ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan); 
		}
        break;
      case 439: // func_name -> func_class_name_ident_list, tkPoint, func_meth_name_ident
{ 
        	var ln = ValueStack[ValueStack.Depth-3].ob as List<ident>;
        	var cnt = ln.Count;
        	if (cnt == 1)
				CurrentSemanticValue.stn = new method_name(null, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
			else 	
				CurrentSemanticValue.stn = new method_name(ln, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
		}
        break;
      case 440: // func_class_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 441: // func_class_name_ident_list -> func_class_name_ident
{ 
			CurrentSemanticValue.ob = new List<ident>(); 
			(CurrentSemanticValue.ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
		}
        break;
      case 442: // func_class_name_ident_list -> func_class_name_ident_list, tkPoint, 
                //                               func_class_name_ident
{ 
			(ValueStack[ValueStack.Depth-3].ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob; 
		}
        break;
      case 443: // func_meth_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 444: // func_meth_name_ident -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 445: // func_meth_name_ident -> operator_name_ident, template_arguments
{ CurrentSemanticValue.id = new template_operator_name(null, ValueStack[ValueStack.Depth-1].stn as ident_list, ValueStack[ValueStack.Depth-2].ex as operator_name_ident, CurrentLocationSpan); }
        break;
      case 446: // func_name_with_template_args -> func_name_ident
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 447: // func_name_with_template_args -> func_name_ident, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 448: // func_name_ident -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 449: // proc_header -> tkProcedure, proc_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{ 
        	CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, CurrentLocationSpan); 
        }
        break;
      case 450: // func_header -> tkFunction, func_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, null, CurrentLocationSpan); 
		}
        break;
      case 451: // func_header -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                optional_method_modificators, optional_where_section
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, ValueStack[ValueStack.Depth-3].td as type_definition, CurrentLocationSpan); 
        }
        break;
      case 452: // external_block -> tkExternal, external_directive_ident, tkName, 
                //                   external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 453: // external_block -> tkExternal, external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-2].ex, null, CurrentLocationSpan); 
		}
        break;
      case 454: // external_block -> tkExternal, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(null, null, CurrentLocationSpan); 
		}
        break;
      case 455: // external_directive_ident -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 456: // external_directive_ident -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 457: // block -> decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 458: // inclass_block -> inclass_decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 459: // inclass_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 460: // fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 461: // fp_list -> tkRoundOpen, tkRoundClose
{ 
			CurrentSemanticValue.stn = null;
		}
        break;
      case 462: // fp_list -> tkRoundOpen, fp_sect_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			if (CurrentSemanticValue.stn != null)
				CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 463: // fp_sect_list -> fp_sect
{ 
			CurrentSemanticValue.stn = new formal_parameters(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
        }
        break;
      case 464: // fp_sect_list -> fp_sect_list, tkSemiColon, fp_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);   
        }
        break;
      case 465: // fp_sect -> attribute_declarations, simple_fp_sect
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as  attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 466: // simple_fp_sect -> param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan); 
		}
        break;
      case 467: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.var_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 468: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.const_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 469: // simple_fp_sect -> tkParams, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td,parametr_kind.params_parametr,null, CurrentLocationSpan);  
		}
        break;
      case 470: // simple_fp_sect -> param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.none, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 471: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.var_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 472: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.const_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 473: // param_name_list -> param_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 474: // param_name_list -> param_name_list, tkComma, param_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);  
		}
        break;
      case 475: // param_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 476: // fptype -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 477: // fptype_noproctype -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 478: // fptype_noproctype -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 479: // fptype_noproctype -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 480: // fptype_noproctype -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 481: // fptype_noproctype -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 482: // stmt -> unlabelled_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 483: // stmt -> label_name, tkColon, stmt
{ 
			CurrentSemanticValue.stn = new labeled_statement(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);  
		}
        break;
      case 484: // unlabelled_stmt -> /* empty */
{ 
			CurrentSemanticValue.stn = new empty_statement(); 
			CurrentSemanticValue.stn.source_context = null;
		}
        break;
      case 485: // unlabelled_stmt -> assignment
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 486: // unlabelled_stmt -> proc_call
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 487: // unlabelled_stmt -> goto_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 488: // unlabelled_stmt -> compound_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 489: // unlabelled_stmt -> if_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 490: // unlabelled_stmt -> case_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 491: // unlabelled_stmt -> repeat_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 492: // unlabelled_stmt -> while_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 493: // unlabelled_stmt -> for_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 494: // unlabelled_stmt -> with_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 495: // unlabelled_stmt -> inherited_message
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 496: // unlabelled_stmt -> try_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 497: // unlabelled_stmt -> raise_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 498: // unlabelled_stmt -> foreach_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 499: // unlabelled_stmt -> var_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 500: // unlabelled_stmt -> expr_as_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 501: // unlabelled_stmt -> lock_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 502: // unlabelled_stmt -> yield_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 503: // unlabelled_stmt -> yield_sequence_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 504: // unlabelled_stmt -> loop_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 505: // unlabelled_stmt -> match_with
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 506: // loop_stmt -> tkLoop, expr_l1, tkDo, unlabelled_stmt
{
			CurrentSemanticValue.stn = new loop_stmt(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].stn as statement,CurrentLocationSpan);
		}
        break;
      case 507: // yield_stmt -> tkYield, expr_l1
{
			CurrentSemanticValue.stn = new yield_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 508: // yield_sequence_stmt -> tkYield, tkSequence, expr_l1
{
			CurrentSemanticValue.stn = new yield_sequence_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 509: // var_stmt -> tkVar, var_decl_part
{ 
			CurrentSemanticValue.stn = new var_statement(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 510: // var_stmt -> tkRoundOpen, tkVar, identifier, tkComma, var_ident_list, 
                //             tkRoundClose, tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].ob as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].ob as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 511: // var_stmt -> tkVar, tkRoundOpen, identifier, tkComma, ident_list, tkRoundClose, 
                //             tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			ValueStack[ValueStack.Depth-4].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
	    }
        break;
      case 512: // assignment -> var_reference, assign_operator, expr_with_func_decl_lambda
{      
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
        }
        break;
      case 513: // assignment -> tkRoundOpen, variable, tkComma, variable_list, tkRoundClose, 
                //               assign_operator, expr
{
			if (ValueStack[ValueStack.Depth-2].op.type != Operators.Assignment)
			    parsertools.AddErrorFromResource("ONLY_BASE_ASSIGNMENT_FOR_TUPLE",LocationStack[LocationStack.Depth-2]);
			(ValueStack[ValueStack.Depth-4].ob as addressed_value_list).Insert(0,ValueStack[ValueStack.Depth-6].ex as addressed_value);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_tuple(ValueStack[ValueStack.Depth-4].ob as addressed_value_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 514: // variable_list -> variable
{
		CurrentSemanticValue.ob = new addressed_value_list(ValueStack[ValueStack.Depth-1].ex as addressed_value,LocationStack[LocationStack.Depth-1]);
	}
        break;
      case 515: // variable_list -> variable_list, tkComma, variable
{
		(ValueStack[ValueStack.Depth-3].ob as addressed_value_list).Add(ValueStack[ValueStack.Depth-1].ex as addressed_value);
		(ValueStack[ValueStack.Depth-3].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
	}
        break;
      case 516: // var_ident_list -> tkVar, identifier
{
		CurrentSemanticValue.ob = new ident_list(ValueStack[ValueStack.Depth-1].id,CurrentLocationSpan);
	}
        break;
      case 517: // var_ident_list -> var_ident_list, tkComma, tkVar, identifier
{
		(ValueStack[ValueStack.Depth-4].ob as ident_list).Add(ValueStack[ValueStack.Depth-1].id);
		(ValueStack[ValueStack.Depth-4].ob as ident_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-4].ob;
	}
        break;
      case 518: // proc_call -> var_reference
{ 
			CurrentSemanticValue.stn = new procedure_call(ValueStack[ValueStack.Depth-1].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex is ident, CurrentLocationSpan); 
		}
        break;
      case 519: // goto_stmt -> tkGoto, label_name
{ 
			CurrentSemanticValue.stn = new goto_statement(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 520: // compound_stmt -> tkBegin, stmt_list, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			(CurrentSemanticValue.stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(CurrentSemanticValue.stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 521: // stmt_list -> stmt
{ 
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 522: // stmt_list -> stmt_list, tkSemiColon, stmt
{  
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as statement_list).Add(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 523: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan); 
        }
        break;
      case 524: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt, tkElse, unlabelled_stmt
{
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as statement, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 525: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, else_case, tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 526: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, tkSemiColon, else_case, 
                //               tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 527: // pattern_cases -> pattern_case
{
            CurrentSemanticValue.stn = new pattern_cases(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 528: // pattern_cases -> pattern_cases, tkSemiColon, pattern_case
{
            CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as pattern_cases).Add(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 529: // pattern_case -> pattern_optional_var, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 530: // pattern_case -> deconstruction_or_const_pattern, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 531: // pattern_case -> collection_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 532: // pattern_case -> tuple_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 533: // case_stmt -> tkCase, expr_l1, tkOf, case_list, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 534: // case_stmt -> tkCase, expr_l1, tkOf, case_list, tkSemiColon, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 535: // case_stmt -> tkCase, expr_l1, tkOf, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-4].ex, NewCaseItem(new empty_statement(), null), ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 536: // case_list -> case_item
{
			if (ValueStack[ValueStack.Depth-1].stn is empty_statement) 
				CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, null);
			else CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 537: // case_list -> case_list, tkSemiColon, case_item
{ 
			CurrentSemanticValue.stn = AddCaseItem(ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 538: // case_item -> case_label_list, tkColon, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new case_variant(ValueStack[ValueStack.Depth-3].stn as expression_list, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 539: // case_label_list -> case_label
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 540: // case_label_list -> case_label_list, tkComma, case_label
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 541: // case_label -> const_elem
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 542: // else_case -> /* empty */
{ CurrentSemanticValue.stn = null;}
        break;
      case 543: // else_case -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 544: // repeat_stmt -> tkRepeat, stmt_list, tkUntil, expr
{ 
		    CurrentSemanticValue.stn = new repeat_node(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-3].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-4].ti;
			(ValueStack[ValueStack.Depth-3].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-2].ti;
			ValueStack[ValueStack.Depth-3].stn.source_context = LocationStack[LocationStack.Depth-4].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 545: // while_stmt -> tkWhile, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewWhileStmt(ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);    
        }
        break;
      case 546: // optional_tk_do -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 547: // optional_tk_do -> /* empty */
{ CurrentSemanticValue.ti = null; }
        break;
      case 548: // lock_stmt -> tkLock, expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new lock_stmt(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 549: // foreach_stmt -> tkForeach, identifier, foreach_stmt_ident_dype_opt, tkIn, 
                //                 expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-6].id, ValueStack[ValueStack.Depth-5].td, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
            if (ValueStack[ValueStack.Depth-5].td == null)
                parsertools.AddWarningFromResource("USING_UNLOCAL_FOREACH_VARIABLE", ValueStack[ValueStack.Depth-6].id.source_context);
        }
        break;
      case 550: // foreach_stmt -> tkForeach, tkVar, identifier, tkColon, type_ref, tkIn, expr_l1, 
                //                 tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-5].td, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 551: // foreach_stmt -> tkForeach, tkVar, identifier, tkIn, expr_l1, tkDo, 
                //                 unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-5].id, new no_type_foreach(), ValueStack[ValueStack.Depth-3].ex, (statement)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 552: // foreach_stmt_ident_dype_opt -> tkColon, type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 554: // for_stmt -> tkFor, optional_var, identifier, for_stmt_decl_or_assign, expr_l1, 
                //             for_cycle_type, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-8].ob, ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-5].ex, (for_cycle_type)ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 555: // optional_var -> tkVar
{ CurrentSemanticValue.ob = true; }
        break;
      case 556: // optional_var -> /* empty */
{ CurrentSemanticValue.ob = false; }
        break;
      case 558: // for_stmt_decl_or_assign -> tkColon, simple_type_identifier, tkAssign
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-2].td; }
        break;
      case 559: // for_cycle_type -> tkTo
{ CurrentSemanticValue.ob = for_cycle_type.to; }
        break;
      case 560: // for_cycle_type -> tkDownto
{ CurrentSemanticValue.ob = for_cycle_type.downto; }
        break;
      case 561: // with_stmt -> tkWith, expr_list, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new with_statement(ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 562: // inherited_message -> tkInherited
{ 
			CurrentSemanticValue.stn = new inherited_message();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 563: // try_stmt -> tkTry, stmt_list, try_handler
{ 
			CurrentSemanticValue.stn = new try_stmt(ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].stn as try_handler, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			ValueStack[ValueStack.Depth-2].stn.source_context = LocationStack[LocationStack.Depth-3].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 564: // try_handler -> tkFinally, stmt_list, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_finally(ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan);
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(ValueStack[ValueStack.Depth-2].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
		}
        break;
      case 565: // try_handler -> tkExcept, exception_block, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_except((exception_block)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan);  
			if ((ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list != null)
			{
				(ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list.source_context = CurrentLocationSpan;
				(ValueStack[ValueStack.Depth-2].stn as exception_block).source_context = CurrentLocationSpan;
			}
		}
        break;
      case 566: // exception_block -> exception_handler_list, exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-2].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 567: // exception_block -> exception_handler_list, tkSemiColon, 
                //                    exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-3].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 568: // exception_block -> stmt_list
{ 
			CurrentSemanticValue.stn = new exception_block(ValueStack[ValueStack.Depth-1].stn as statement_list, null, null, LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 569: // exception_handler_list -> exception_handler
{ 
			CurrentSemanticValue.stn = new exception_handler_list(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 570: // exception_handler_list -> exception_handler_list, tkSemiColon, 
                //                           exception_handler
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as exception_handler_list).Add(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 571: // exception_block_else_branch -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 572: // exception_block_else_branch -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 573: // exception_handler -> tkOn, exception_identifier, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new exception_handler((ValueStack[ValueStack.Depth-3].stn as exception_ident).variable, (ValueStack[ValueStack.Depth-3].stn as exception_ident).type_name, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 574: // exception_identifier -> exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(null, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 575: // exception_identifier -> exception_variable, tkColon, 
                //                         exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(ValueStack[ValueStack.Depth-3].id, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 576: // exception_class_type_identifier -> simple_type_identifier
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 577: // exception_variable -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 578: // raise_stmt -> tkRaise
{ 
			CurrentSemanticValue.stn = new raise_stmt(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 579: // raise_stmt -> tkRaise, expr
{ 
			CurrentSemanticValue.stn = new raise_stmt(ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);  
		}
        break;
      case 580: // expr_list -> expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 581: // expr_list -> expr_list, tkComma, expr_with_func_decl_lambda
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 582: // expr_as_stmt -> allowable_expr_as_stmt
{ 
			CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 583: // allowable_expr_as_stmt -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 584: // expr_with_func_decl_lambda -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 585: // expr_with_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 586: // expr -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 587: // expr -> format_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 588: // expr_l1 -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 589: // expr_l1 -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 590: // expr_l1_func_decl_lambda -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 591: // expr_l1_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 592: // expr_dq -> relop_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 593: // expr_dq -> expr_dq, tkDoubleQuestion, relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 594: // sizeof_expr -> tkSizeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new sizeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, null, CurrentLocationSpan);  
		}
        break;
      case 595: // typeof_expr -> tkTypeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 596: // typeof_expr -> tkTypeOf, tkRoundOpen, empty_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 597: // question_expr -> expr_l1, tkQuestion, expr_l1, tkColon, expr_l1
{ 
            if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
            	parsertools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 598: // empty_template_type_reference -> simple_type_identifier, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 599: // empty_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
        }
        break;
      case 600: // simple_or_template_type_reference -> simple_type_identifier
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 601: // simple_or_template_type_reference -> simple_type_identifier, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 602: // simple_or_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                      template_type_or_typeclass_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 603: // optional_array_initializer -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = new array_const((expression_list)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan); 
		}
        break;
      case 605: // new_expr -> tkNew, simple_or_template_type_reference, 
                //             optional_expr_list_with_bracket
{
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-2].td, ValueStack[ValueStack.Depth-1].stn as expression_list, false, null, CurrentLocationSpan);
        }
        break;
      case 606: // new_expr -> tkNew, simple_or_template_type_reference, tkSquareOpen, 
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
      case 607: // new_expr -> tkNew, tkClass, tkRoundOpen, list_fields_in_unnamed_object, 
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
      case 608: // field_in_unnamed_object -> identifier, tkAssign, relop_expr
{
		    if (ValueStack[ValueStack.Depth-1].ex is nil_const)
				parsertools.AddErrorFromResource("NIL_IN_UNNAMED_OBJECT",CurrentLocationSpan);		    
			CurrentSemanticValue.ob = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 609: // field_in_unnamed_object -> relop_expr
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
      case 610: // list_fields_in_unnamed_object -> field_in_unnamed_object
{
			var l = new name_assign_expr_list();
			CurrentSemanticValue.ob = l.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
		}
        break;
      case 611: // list_fields_in_unnamed_object -> list_fields_in_unnamed_object, tkComma, 
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
      case 612: // optional_expr_list_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 613: // optional_expr_list_with_bracket -> tkRoundOpen, optional_expr_list, 
                //                                    tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 614: // relop_expr -> simple_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 615: // relop_expr -> relop_expr, relop, simple_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 616: // relop_expr -> is_type_expr, tkRoundOpen, pattern_out_param_list, tkRoundClose
{
            var isTypeCheck = ValueStack[ValueStack.Depth-4].ex as typecast_node;
            var deconstructorPattern = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, isTypeCheck.type_def, null, CurrentLocationSpan); 
            CurrentSemanticValue.ex = new is_pattern_expr(isTypeCheck.expr, deconstructorPattern, CurrentLocationSpan);
        }
        break;
      case 617: // relop_expr -> term, tkIs, collection_pattern
{
            CurrentSemanticValue.ex = new is_pattern_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 618: // relop_expr -> term, tkIs, tuple_pattern
{
            CurrentSemanticValue.ex = new is_pattern_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 619: // pattern -> simple_or_template_type_reference, tkRoundOpen, 
                //            pattern_out_param_list, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 620: // pattern_optional_var -> simple_or_template_type_reference, tkRoundOpen, 
                //                         pattern_out_param_list_optional_var, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 621: // deconstruction_or_const_pattern -> simple_or_template_type_reference, 
                //                                    tkRoundOpen, 
                //                                    pattern_out_param_list_optional_var, 
                //                                    tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 622: // deconstruction_or_const_pattern -> const_pattern_expr_list
{
		    CurrentSemanticValue.stn = new const_pattern(ValueStack[ValueStack.Depth-1].ob as List<syntax_tree_node>, CurrentLocationSpan); 
		}
        break;
      case 623: // const_pattern_expr_list -> const_pattern_expression
{ 
			CurrentSemanticValue.ob = new List<syntax_tree_node>(); 
			(CurrentSemanticValue.ob as List<syntax_tree_node>).Add(ValueStack[ValueStack.Depth-1].stn);
		}
        break;
      case 624: // const_pattern_expr_list -> const_pattern_expr_list, tkComma, 
                //                            const_pattern_expression
{ 
			var list = ValueStack[ValueStack.Depth-3].ob as List<syntax_tree_node>;
            list.Add(ValueStack[ValueStack.Depth-1].stn);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 625: // const_pattern_expression -> literal_or_number
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 626: // const_pattern_expression -> simple_or_template_type_reference
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 627: // const_pattern_expression -> tkNil
{ 
			CurrentSemanticValue.stn = new nil_const();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 628: // const_pattern_expression -> sizeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 629: // const_pattern_expression -> typeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 630: // collection_pattern -> tkSquareOpen, collection_pattern_expr_list, tkSquareClose
{
			CurrentSemanticValue.stn = new collection_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 631: // collection_pattern_expr_list -> collection_pattern_list_item
{
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 632: // collection_pattern_expr_list -> collection_pattern_expr_list, tkComma, 
                //                                 collection_pattern_list_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 633: // collection_pattern_list_item -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 634: // collection_pattern_list_item -> collection_pattern_var_item
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 635: // collection_pattern_list_item -> tkUnderscore
{
			CurrentSemanticValue.stn = new collection_pattern_wild_card(CurrentLocationSpan);
		}
        break;
      case 636: // collection_pattern_list_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 637: // collection_pattern_list_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 638: // collection_pattern_list_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 639: // collection_pattern_list_item -> tkDotDot
{
			CurrentSemanticValue.stn = new collection_pattern_gap_parameter(CurrentLocationSpan);
		}
        break;
      case 640: // collection_pattern_var_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new collection_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 641: // tuple_pattern -> tkRoundOpen, tuple_pattern_item_list, tkRoundClose
{
			if ((ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>).Count>6) 
				parsertools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
			CurrentSemanticValue.stn = new tuple_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 642: // tuple_pattern_item -> tkUnderscore
{ 
			CurrentSemanticValue.stn = new tuple_pattern_wild_card(CurrentLocationSpan); 
		}
        break;
      case 643: // tuple_pattern_item -> literal_or_number
{ 
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 644: // tuple_pattern_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new tuple_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 645: // tuple_pattern_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 646: // tuple_pattern_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 647: // tuple_pattern_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 648: // tuple_pattern_item_list -> tuple_pattern_item
{ 
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 649: // tuple_pattern_item_list -> tuple_pattern_item_list, tkComma, tuple_pattern_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 650: // pattern_out_param_list_optional_var -> pattern_out_param_optional_var
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 651: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkSemiColon, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 652: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkComma, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 653: // pattern_out_param_list -> pattern_out_param
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 654: // pattern_out_param_list -> pattern_out_param_list, tkSemiColon, 
                //                           pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 655: // pattern_out_param_list -> pattern_out_param_list, tkComma, pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 656: // pattern_out_param -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 657: // pattern_out_param -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 658: // pattern_out_param -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 659: // pattern_out_param -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 660: // pattern_out_param -> pattern
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 661: // pattern_out_param -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 662: // pattern_out_param -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 663: // pattern_out_param_optional_var -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 664: // pattern_out_param_optional_var -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 665: // pattern_out_param_optional_var -> identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, false, CurrentLocationSpan);
        }
        break;
      case 666: // pattern_out_param_optional_var -> identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, false, CurrentLocationSpan);
        }
        break;
      case 667: // pattern_out_param_optional_var -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 668: // pattern_out_param_optional_var -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 669: // pattern_out_param_optional_var -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 670: // pattern_out_param_optional_var -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 671: // pattern_out_param_optional_var -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 672: // simple_expr_or_nothing -> simple_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 673: // simple_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 674: // const_expr_or_nothing -> const_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 675: // const_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 676: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 677: // format_expr -> tkColon, simple_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 678: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing, tkColon, 
                //                simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 679: // format_expr -> tkColon, simple_expr_or_nothing, tkColon, simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 680: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 681: // format_const_expr -> tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 682: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing, tkColon, 
                //                      const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 683: // format_const_expr -> tkColon, const_expr_or_nothing, tkColon, const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 684: // relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 685: // relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 686: // relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 687: // relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 688: // relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 689: // relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 690: // relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 691: // simple_expr -> term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 692: // simple_expr -> simple_expr, addop, term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 693: // addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 694: // addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 695: // addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 696: // addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 697: // addop -> tkCSharpStyleOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 698: // typecast_op -> tkAs
{ 
			CurrentSemanticValue.ob = op_typecast.as_op; 
		}
        break;
      case 699: // typecast_op -> tkIs
{ 
			CurrentSemanticValue.ob = op_typecast.is_op; 
		}
        break;
      case 700: // as_is_expr -> is_type_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 701: // as_is_expr -> as_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 702: // as_expr -> term, tkAs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 703: // is_type_expr -> term, tkIs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 704: // simple_term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 705: // power_expr -> simple_term, tkStarStar, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 706: // term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 707: // term -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 708: // term -> power_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 709: // term -> term, mulop, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 710: // term -> term, mulop, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 711: // term -> as_is_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 712: // mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 713: // mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 714: // mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 715: // mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 716: // mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 717: // mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 718: // mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 719: // default_expr -> tkDefault, tkRoundOpen, simple_or_template_type_reference, 
                //                 tkRoundClose
{ 
			CurrentSemanticValue.ex = new default_operator(ValueStack[ValueStack.Depth-2].td as named_type_reference, CurrentLocationSpan);  
		}
        break;
      case 720: // tuple -> tkRoundOpen, expr_l1, tkComma, expr_l1_list, lambda_type_ref, 
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
      case 721: // factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 722: // factor -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 723: // factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 724: // factor -> tkSquareOpen, elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new pascal_set_constant(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 725: // factor -> tkNot, factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 726: // factor -> sign, factor
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
      case 727: // factor -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 728: // factor -> tuple
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 729: // literal_or_number -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 730: // literal_or_number -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 731: // var_question_point -> variable, tkQuestionPoint, variable
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 732: // var_question_point -> variable, tkQuestionPoint, var_question_point
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 733: // var_reference -> var_address, variable
{
			CurrentSemanticValue.ex = NewVarReference(ValueStack[ValueStack.Depth-2].stn as get_address, ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);
		}
        break;
      case 734: // var_reference -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 735: // var_reference -> var_question_point
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 736: // var_address -> tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(CurrentLocationSpan);
		}
        break;
      case 737: // var_address -> var_address, tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(ValueStack[ValueStack.Depth-2].stn as get_address, CurrentLocationSpan);
		}
        break;
      case 738: // attribute_variable -> simple_type_identifier, optional_expr_list_with_bracket
{ 
			CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
		}
        break;
      case 739: // attribute_variable -> template_type, optional_expr_list_with_bracket
{
            CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 740: // dotted_identifier -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 741: // dotted_identifier -> dotted_identifier, tkPoint, identifier_or_keyword
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
		}
        break;
      case 742: // variable_as_type -> dotted_identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;}
        break;
      case 743: // variable_as_type -> dotted_identifier, template_type_params
{ CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-2].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);   }
        break;
      case 744: // variable_or_literal_or_number -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 745: // variable_or_literal_or_number -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 746: // variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 747: // variable -> operator_name_ident
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 748: // variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 749: // variable -> tkRoundOpen, expr, tkRoundClose
{
		    if (!parsertools.build_tree_for_formatter) 
            {
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
            } 
			else CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
        }
        break;
      case 750: // variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 751: // variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 752: // variable -> literal_or_number, tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 753: // variable -> variable_or_literal_or_number, tkSquareOpen, expr_list, 
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
      case 754: // variable -> variable, tkQuestionSquareOpen, format_expr, tkSquareClose
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
      case 755: // variable -> variable, tkRoundOpen, optional_expr_list, tkRoundClose
{
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 756: // variable -> variable, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 757: // variable -> tuple, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 758: // variable -> variable, tkDeref
{
			CurrentSemanticValue.ex = new roof_dereference(ValueStack[ValueStack.Depth-2].ex as addressed_value,CurrentLocationSpan);
        }
        break;
      case 759: // variable -> variable, tkAmpersend, template_type_or_typeclass_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 760: // optional_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 761: // optional_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 762: // elem_list -> elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 763: // elem_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 764: // elem_list1 -> elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 765: // elem_list1 -> elem_list1, tkComma, elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 766: // elem -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 767: // elem -> expr, tkDotDot, expr
{ CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 768: // one_literal -> tkStringLiteral
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 769: // one_literal -> tkAsciiChar
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 770: // literal -> literal_list
{ 
			CurrentSemanticValue.ex = NewLiteral(ValueStack[ValueStack.Depth-1].stn as literal_const_line);
        }
        break;
      case 771: // literal -> tkFormatStringLiteral
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
      case 772: // literal_list -> one_literal
{ 
			CurrentSemanticValue.stn = new literal_const_line(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 773: // literal_list -> literal_list, one_literal
{ 
        	var line = ValueStack[ValueStack.Depth-2].stn as literal_const_line;
            if (line.literals.Last() is string_const && ValueStack[ValueStack.Depth-1].ex is string_const)
            	parsertools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.stn = line.Add(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 774: // operator_name_ident -> tkOperator, overload_operator
{ 
			CurrentSemanticValue.ex = new operator_name_ident((ValueStack[ValueStack.Depth-1].op as op_type_node).text, (ValueStack[ValueStack.Depth-1].op as op_type_node).type, CurrentLocationSpan);
		}
        break;
      case 775: // optional_method_modificators -> tkSemiColon
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 776: // optional_method_modificators -> tkSemiColon, meth_modificators, tkSemiColon
{ 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 777: // optional_method_modificators1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 778: // optional_method_modificators1 -> tkSemiColon, meth_modificators
{ 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 779: // meth_modificators -> meth_modificator
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan); 
		}
        break;
      case 780: // meth_modificators -> meth_modificators, tkSemiColon, meth_modificator
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as procedure_attributes_list).Add(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan);  
		}
        break;
      case 781: // identifier -> tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 782: // identifier -> property_specifier_directives
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 783: // identifier -> non_reserved
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 784: // identifier_or_keyword -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 785: // identifier_or_keyword -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 786: // identifier_or_keyword -> reserved_keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 787: // identifier_keyword_operatorname -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 788: // identifier_keyword_operatorname -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 789: // identifier_keyword_operatorname -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 790: // meth_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 791: // meth_modificator -> tkOverload
{ 
            CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id;
            parsertools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", ValueStack[ValueStack.Depth-1].id.source_context);
        }
        break;
      case 792: // meth_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 793: // meth_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 794: // meth_modificator -> tkExtensionMethod
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 795: // meth_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 796: // property_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 797: // property_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 798: // property_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 799: // property_specifier_directives -> tkRead
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 800: // property_specifier_directives -> tkWrite
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 801: // non_reserved -> tkName
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 802: // non_reserved -> tkNew
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 803: // visibility_specifier -> tkInternal
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 804: // visibility_specifier -> tkPublic
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 805: // visibility_specifier -> tkProtected
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 806: // visibility_specifier -> tkPrivate
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 807: // keyword -> visibility_specifier
{ 
			CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  
		}
        break;
      case 808: // keyword -> tkSealed
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 809: // keyword -> tkTemplate
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 810: // keyword -> tkOr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 811: // keyword -> tkTypeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 812: // keyword -> tkSizeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 813: // keyword -> tkDefault
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 814: // keyword -> tkWhere
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 815: // keyword -> tkXor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 816: // keyword -> tkAnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 817: // keyword -> tkDiv
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 818: // keyword -> tkMod
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 819: // keyword -> tkShl
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 820: // keyword -> tkShr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 821: // keyword -> tkNot
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 822: // keyword -> tkAs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 823: // keyword -> tkIn
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 824: // keyword -> tkIs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 825: // keyword -> tkArray
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 826: // keyword -> tkSequence
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 827: // keyword -> tkBegin
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 828: // keyword -> tkCase
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 829: // keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 830: // keyword -> tkConst
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 831: // keyword -> tkConstructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 832: // keyword -> tkDestructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 833: // keyword -> tkDownto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 834: // keyword -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 835: // keyword -> tkElse
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 836: // keyword -> tkEnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 837: // keyword -> tkExcept
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 838: // keyword -> tkFile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 839: // keyword -> tkAuto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 840: // keyword -> tkFinalization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 841: // keyword -> tkFinally
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 842: // keyword -> tkFor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 843: // keyword -> tkForeach
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 844: // keyword -> tkFunction
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 845: // keyword -> tkIf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 846: // keyword -> tkImplementation
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 847: // keyword -> tkInherited
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 848: // keyword -> tkInitialization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 849: // keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 850: // keyword -> tkProcedure
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 851: // keyword -> tkProperty
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 852: // keyword -> tkRaise
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 853: // keyword -> tkRecord
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 854: // keyword -> tkRepeat
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 855: // keyword -> tkSet
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 856: // keyword -> tkTry
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 857: // keyword -> tkType
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 858: // keyword -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 859: // keyword -> tkThen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 860: // keyword -> tkTo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 861: // keyword -> tkUntil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 862: // keyword -> tkUses
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 863: // keyword -> tkVar
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 864: // keyword -> tkWhile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 865: // keyword -> tkWith
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 866: // keyword -> tkNil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 867: // keyword -> tkGoto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 868: // keyword -> tkOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 869: // keyword -> tkLabel
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 870: // keyword -> tkProgram
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 871: // keyword -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 872: // keyword -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 873: // keyword -> tkNamespace
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 874: // keyword -> tkExternal
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 875: // keyword -> tkParams
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 876: // keyword -> tkEvent
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 877: // keyword -> tkYield
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 878: // keyword -> tkMatch
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 879: // keyword -> tkWhen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 880: // keyword -> tkInstance
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 881: // reserved_keyword -> tkOperator
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 882: // overload_operator -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 883: // overload_operator -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 884: // overload_operator -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 885: // overload_operator -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 886: // overload_operator -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 887: // overload_operator -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 888: // overload_operator -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 889: // overload_operator -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 890: // overload_operator -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 891: // overload_operator -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 892: // overload_operator -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 893: // overload_operator -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 894: // overload_operator -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 895: // overload_operator -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 896: // overload_operator -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 897: // overload_operator -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 898: // overload_operator -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 899: // overload_operator -> tkNot
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 900: // overload_operator -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 901: // overload_operator -> tkImplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 902: // overload_operator -> tkExplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 903: // overload_operator -> assign_operator
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 904: // overload_operator -> tkStarStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 905: // assign_operator -> tkAssign
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 906: // assign_operator -> tkPlusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 907: // assign_operator -> tkMinusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 908: // assign_operator -> tkMultEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 909: // assign_operator -> tkDivEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 910: // func_decl_lambda -> identifier, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 911: // func_decl_lambda -> tkRoundOpen, tkRoundClose, lambda_type_ref_noproctype, 
                //                     tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 912: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkRoundClose, 
                //                     lambda_type_ref_noproctype, tkArrow, lambda_function_body
{
			var idList = new ident_list(ValueStack[ValueStack.Depth-7].id, LocationStack[LocationStack.Depth-7]); 
            var loc = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5]);
			var formalPars = new formal_parameters(new typed_parameters(idList, ValueStack[ValueStack.Depth-5].td, parametr_kind.none, null, loc), loc);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 913: // func_decl_lambda -> tkRoundOpen, identifier, tkSemiColon, full_lambda_fp_list, 
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
      case 914: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkSemiColon, 
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
      case 915: // func_decl_lambda -> tkRoundOpen, expr_l1, tkComma, expr_l1_list, 
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
      case 916: // func_decl_lambda -> expl_func_decl_lambda
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
		}
        break;
      case 917: // optional_full_lambda_fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 918: // optional_full_lambda_fp_list -> tkSemiColon, full_lambda_fp_list
{
		CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
	}
        break;
      case 919: // rem_lambda -> lambda_type_ref_noproctype, tkArrow, lambda_function_body
{ 
		    CurrentSemanticValue.ob = new pair_type_stlist(ValueStack[ValueStack.Depth-3].td,ValueStack[ValueStack.Depth-1].stn as statement_list);
		}
        break;
      case 920: // expl_func_decl_lambda -> tkFunction, lambda_type_ref, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 921: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, tkRoundClose, lambda_type_ref, 
                //                          tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 922: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, full_lambda_fp_list, 
                //                          tkRoundClose, lambda_type_ref, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 923: // expl_func_decl_lambda -> tkProcedure, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 924: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, tkRoundClose, tkArrow, 
                //                          lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 925: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, full_lambda_fp_list, 
                //                          tkRoundClose, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-4].stn as formal_parameters, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 926: // full_lambda_fp_list -> lambda_simple_fp_sect
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
      case 927: // full_lambda_fp_list -> full_lambda_fp_list, tkSemiColon, lambda_simple_fp_sect
{
			CurrentSemanticValue.stn =(ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
		}
        break;
      case 928: // lambda_simple_fp_sect -> ident_list, lambda_type_ref
{
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-2].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan);
		}
        break;
      case 929: // lambda_type_ref -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
		}
        break;
      case 930: // lambda_type_ref -> tkColon, fptype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 931: // lambda_type_ref_noproctype -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
		}
        break;
      case 932: // lambda_type_ref_noproctype -> tkColon, fptype_noproctype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 933: // common_lambda_body -> compound_stmt
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 934: // common_lambda_body -> if_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 935: // common_lambda_body -> while_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 936: // common_lambda_body -> repeat_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 937: // common_lambda_body -> for_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 938: // common_lambda_body -> foreach_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 939: // common_lambda_body -> loop_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 940: // common_lambda_body -> case_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 941: // common_lambda_body -> try_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 942: // common_lambda_body -> lock_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 943: // common_lambda_body -> raise_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 944: // common_lambda_body -> yield_stmt
{
			parsertools.AddErrorFromResource("YIELD_STATEMENT_CANNOT_BE_USED_IN_LAMBDA_BODY", CurrentLocationSpan);
		}
        break;
      case 945: // lambda_function_body -> expr_l1_func_decl_lambda
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
      case 946: // lambda_function_body -> common_lambda_body
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 947: // lambda_procedure_body -> proc_call
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 948: // lambda_procedure_body -> assignment
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 949: // lambda_procedure_body -> common_lambda_body
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
