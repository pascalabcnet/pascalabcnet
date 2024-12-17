// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  LAPTOP-TE3HP881
// DateTime: 15.12.2024 8:40:16
// UserName: miks
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

    // private int maxErrors = 10;
    private PascalParserTools parserTools;
	private ParserLambdaHelper lambdaHelper = new ParserLambdaHelper();
	
    public GPPGParser(AbstractScanner<PascalABCCompiler.ParserTools.Union, LexLocation> scanner, PascalParserTools parserTools) : base(scanner) 
	{ 
		this.parserTools = parserTools;
	}
  // End verbatim content from ABCPascal.y

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[1029];
  private static State[] states = new State[1707];
  private static string[] nonTerms = new string[] {
      "parse_goal", "unit_key_word", "class_or_static", "assignment", "optional_array_initializer", 
      "attribute_declarations", "ot_visibility_specifier", "one_attribute", "attribute_variable", 
      "const_factor", "const_factor_without_unary_op", "const_variable_2", "const_term", 
      "const_variable", "literal_or_number", "unsigned_number", "program_block", 
      "optional_var", "class_attribute", "class_attributes", "class_attributes1", 
      "lambda_unpacked_params_or_id", "lambda_list_of_unpacked_params_or_id", 
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
    states[0] = new State(new int[]{61,1606,105,1673,106,1674,109,1675,88,1680,90,1685,89,1692,75,1697,77,1703,3,-27,52,-27,91,-27,59,-27,29,-27,66,-27,50,-27,53,-27,62,-27,11,-27,44,-27,37,-27,28,-27,26,-27,19,-27,30,-27,31,-27},new int[]{-1,1,-233,3,-234,4,-306,1618,-308,1619,-2,1668,-174,1679});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,1602,52,-14,91,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-184,5,-185,1600,-183,1605});
    states[5] = new State(-41,new int[]{-302,6});
    states[6] = new State(new int[]{52,1588,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,91,-67},new int[]{-17,7,-304,14,-36,15,-40,1519,-41,1520});
    states[7] = new State(new int[]{7,9,10,10,5,11,100,12,6,13,2,-26},new int[]{-187,8});
    states[8] = new State(-20);
    states[9] = new State(-21);
    states[10] = new State(-22);
    states[11] = new State(-23);
    states[12] = new State(-24);
    states[13] = new State(-25);
    states[14] = new State(-42);
    states[15] = new State(new int[]{91,17},new int[]{-255,16});
    states[16] = new State(-34);
    states[17] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494},new int[]{-252,18,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[18] = new State(new int[]{92,19,10,20});
    states[19] = new State(-530);
    states[20] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494},new int[]{-261,21,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[21] = new State(-532);
    states[22] = new State(-492);
    states[23] = new State(-495);
    states[24] = new State(new int[]{110,427,111,428,112,429,113,430,114,431,92,-528,10,-528,98,-528,101,-528,33,-528,104,-528,2,-528,9,-528,12,-528,100,-528,99,-528,32,-528,85,-528,84,-528,83,-528,82,-528,81,-528,86,-528},new int[]{-193,25});
    states[25] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,831,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680},new int[]{-87,26,-86,27,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,832,-99,657,-323,675});
    states[26] = new State(-522);
    states[27] = new State(new int[]{70,28,92,-598,10,-598,98,-598,101,-598,33,-598,104,-598,2,-598,9,-598,12,-598,100,-598,99,-598,32,-598,85,-598,84,-598,83,-598,82,-598,81,-598,86,-598});
    states[28] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,29,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[29] = new State(-607);
    states[30] = new State(new int[]{16,31,70,-608,92,-608,10,-608,98,-608,101,-608,33,-608,104,-608,2,-608,9,-608,12,-608,100,-608,99,-608,32,-608,85,-608,84,-608,83,-608,82,-608,81,-608,86,-608,6,-608,76,-608,5,-608,51,-608,58,-608,141,-608,143,-608,80,-608,78,-608,160,-608,87,-608,45,-608,42,-608,8,-608,21,-608,22,-608,144,-608,147,-608,145,-608,146,-608,155,-608,158,-608,157,-608,156,-608,11,-608,57,-608,91,-608,40,-608,25,-608,97,-608,54,-608,35,-608,55,-608,102,-608,47,-608,36,-608,53,-608,60,-608,74,-608,72,-608,38,-608,71,-608,13,-611});
    states[31] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366},new int[]{-97,32,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576});
    states[32] = new State(new int[]{120,157,125,158,123,159,121,160,124,161,122,162,137,163,135,164,16,-621,70,-621,92,-621,10,-621,98,-621,101,-621,33,-621,104,-621,2,-621,9,-621,12,-621,100,-621,99,-621,32,-621,85,-621,84,-621,83,-621,82,-621,81,-621,86,-621,13,-621,6,-621,76,-621,5,-621,51,-621,58,-621,141,-621,143,-621,80,-621,78,-621,160,-621,87,-621,45,-621,42,-621,8,-621,21,-621,22,-621,144,-621,147,-621,145,-621,146,-621,155,-621,158,-621,157,-621,156,-621,11,-621,57,-621,91,-621,40,-621,25,-621,97,-621,54,-621,35,-621,55,-621,102,-621,47,-621,36,-621,53,-621,60,-621,74,-621,72,-621,38,-621,71,-621,116,-621,115,-621,128,-621,129,-621,126,-621,138,-621,136,-621,118,-621,117,-621,131,-621,132,-621,133,-621,134,-621,130,-621},new int[]{-195,33});
    states[33] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-104,34,-242,1518,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,600,-267,576});
    states[34] = new State(new int[]{6,35,120,-646,125,-646,123,-646,121,-646,124,-646,122,-646,137,-646,135,-646,16,-646,70,-646,92,-646,10,-646,98,-646,101,-646,33,-646,104,-646,2,-646,9,-646,12,-646,100,-646,99,-646,32,-646,85,-646,84,-646,83,-646,82,-646,81,-646,86,-646,13,-646,76,-646,5,-646,51,-646,58,-646,141,-646,143,-646,80,-646,78,-646,160,-646,87,-646,45,-646,42,-646,8,-646,21,-646,22,-646,144,-646,147,-646,145,-646,146,-646,155,-646,158,-646,157,-646,156,-646,11,-646,57,-646,91,-646,40,-646,25,-646,97,-646,54,-646,35,-646,55,-646,102,-646,47,-646,36,-646,53,-646,60,-646,74,-646,72,-646,38,-646,71,-646,116,-646,115,-646,128,-646,129,-646,126,-646,138,-646,136,-646,118,-646,117,-646,131,-646,132,-646,133,-646,134,-646,130,-646});
    states[35] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366},new int[]{-82,36,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,600,-267,576});
    states[36] = new State(new int[]{116,172,115,173,128,174,129,175,126,176,6,-725,5,-725,120,-725,125,-725,123,-725,121,-725,124,-725,122,-725,137,-725,135,-725,16,-725,70,-725,92,-725,10,-725,98,-725,101,-725,33,-725,104,-725,2,-725,9,-725,12,-725,100,-725,99,-725,32,-725,85,-725,84,-725,83,-725,82,-725,81,-725,86,-725,13,-725,76,-725,51,-725,58,-725,141,-725,143,-725,80,-725,78,-725,160,-725,87,-725,45,-725,42,-725,8,-725,21,-725,22,-725,144,-725,147,-725,145,-725,146,-725,155,-725,158,-725,157,-725,156,-725,11,-725,57,-725,91,-725,40,-725,25,-725,97,-725,54,-725,35,-725,55,-725,102,-725,47,-725,36,-725,53,-725,60,-725,74,-725,72,-725,38,-725,71,-725,138,-725,136,-725,118,-725,117,-725,131,-725,132,-725,133,-725,134,-725,130,-725},new int[]{-196,37});
    states[37] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-81,38,-242,1517,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,600,-267,576});
    states[38] = new State(new int[]{138,178,136,1507,118,1510,117,1511,131,1512,132,1513,133,1514,134,1515,130,1516,116,-727,115,-727,128,-727,129,-727,126,-727,6,-727,5,-727,120,-727,125,-727,123,-727,121,-727,124,-727,122,-727,137,-727,135,-727,16,-727,70,-727,92,-727,10,-727,98,-727,101,-727,33,-727,104,-727,2,-727,9,-727,12,-727,100,-727,99,-727,32,-727,85,-727,84,-727,83,-727,82,-727,81,-727,86,-727,13,-727,76,-727,51,-727,58,-727,141,-727,143,-727,80,-727,78,-727,160,-727,87,-727,45,-727,42,-727,8,-727,21,-727,22,-727,144,-727,147,-727,145,-727,146,-727,155,-727,158,-727,157,-727,156,-727,11,-727,57,-727,91,-727,40,-727,25,-727,97,-727,54,-727,35,-727,55,-727,102,-727,47,-727,36,-727,53,-727,60,-727,74,-727,72,-727,38,-727,71,-727},new int[]{-197,39});
    states[39] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-95,40,-268,41,-242,42,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-96,459});
    states[40] = new State(-748);
    states[41] = new State(-749);
    states[42] = new State(-750);
    states[43] = new State(-763);
    states[44] = new State(new int[]{7,45,11,150,17,378,138,-764,136,-764,118,-764,117,-764,131,-764,132,-764,133,-764,134,-764,130,-764,116,-764,115,-764,128,-764,129,-764,126,-764,6,-764,5,-764,120,-764,125,-764,123,-764,121,-764,124,-764,122,-764,137,-764,135,-764,16,-764,70,-764,92,-764,10,-764,98,-764,101,-764,33,-764,104,-764,2,-764,9,-764,12,-764,100,-764,99,-764,32,-764,85,-764,84,-764,83,-764,82,-764,81,-764,86,-764,13,-764,76,-764,51,-764,58,-764,141,-764,143,-764,80,-764,78,-764,160,-764,87,-764,45,-764,42,-764,8,-764,21,-764,22,-764,144,-764,147,-764,145,-764,146,-764,155,-764,158,-764,157,-764,156,-764,57,-764,91,-764,40,-764,25,-764,97,-764,54,-764,35,-764,55,-764,102,-764,47,-764,36,-764,53,-764,60,-764,74,-764,72,-764,38,-764,71,-764,119,-761});
    states[45] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-137,46,-146,47,-150,49,-151,52,-292,57,-149,58,-293,148});
    states[46] = new State(-798);
    states[47] = new State(-839);
    states[48] = new State(-834);
    states[49] = new State(-835);
    states[50] = new State(-855);
    states[51] = new State(-856);
    states[52] = new State(-836);
    states[53] = new State(-857);
    states[54] = new State(-858);
    states[55] = new State(-837);
    states[56] = new State(-838);
    states[57] = new State(-840);
    states[58] = new State(-863);
    states[59] = new State(-859);
    states[60] = new State(-860);
    states[61] = new State(-861);
    states[62] = new State(-862);
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
    states[147] = new State(-948);
    states[148] = new State(-841);
    states[149] = new State(-949);
    states[150] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,831,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680},new int[]{-70,151,-87,394,-86,27,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,832,-99,657,-323,675});
    states[151] = new State(new int[]{12,152,100,153});
    states[152] = new State(-800);
    states[153] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,831,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680},new int[]{-87,154,-86,27,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,832,-99,657,-323,675});
    states[154] = new State(-593);
    states[155] = new State(-605);
    states[156] = new State(new int[]{120,157,125,158,123,159,121,160,124,161,122,162,137,163,135,164,16,-620,70,-620,92,-620,10,-620,98,-620,101,-620,33,-620,104,-620,2,-620,9,-620,12,-620,100,-620,99,-620,32,-620,85,-620,84,-620,83,-620,82,-620,81,-620,86,-620,13,-620,6,-620,76,-620,5,-620,51,-620,58,-620,141,-620,143,-620,80,-620,78,-620,160,-620,87,-620,45,-620,42,-620,8,-620,21,-620,22,-620,144,-620,147,-620,145,-620,146,-620,155,-620,158,-620,157,-620,156,-620,11,-620,57,-620,91,-620,40,-620,25,-620,97,-620,54,-620,35,-620,55,-620,102,-620,47,-620,36,-620,53,-620,60,-620,74,-620,72,-620,38,-620,71,-620,116,-620,115,-620,128,-620,129,-620,126,-620,138,-620,136,-620,118,-620,117,-620,131,-620,132,-620,133,-620,134,-620,130,-620},new int[]{-195,33});
    states[157] = new State(-716);
    states[158] = new State(-717);
    states[159] = new State(-718);
    states[160] = new State(-719);
    states[161] = new State(-720);
    states[162] = new State(-721);
    states[163] = new State(-722);
    states[164] = new State(new int[]{137,165});
    states[165] = new State(-723);
    states[166] = new State(new int[]{6,35,5,167,120,-645,125,-645,123,-645,121,-645,124,-645,122,-645,137,-645,135,-645,16,-645,70,-645,92,-645,10,-645,98,-645,101,-645,33,-645,104,-645,2,-645,9,-645,12,-645,100,-645,99,-645,32,-645,85,-645,84,-645,83,-645,82,-645,81,-645,86,-645,13,-645,76,-645});
    states[167] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,5,-705,70,-705,92,-705,10,-705,98,-705,101,-705,33,-705,104,-705,2,-705,9,-705,12,-705,100,-705,99,-705,32,-705,84,-705,83,-705,82,-705,81,-705,6,-705},new int[]{-114,168,-104,601,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,600,-267,576});
    states[168] = new State(new int[]{5,169,70,-708,92,-708,10,-708,98,-708,101,-708,33,-708,104,-708,2,-708,9,-708,12,-708,100,-708,99,-708,32,-708,85,-708,84,-708,83,-708,82,-708,81,-708,86,-708,6,-708,76,-708});
    states[169] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366},new int[]{-104,170,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,600,-267,576});
    states[170] = new State(new int[]{6,35,70,-710,92,-710,10,-710,98,-710,101,-710,33,-710,104,-710,2,-710,9,-710,12,-710,100,-710,99,-710,32,-710,85,-710,84,-710,83,-710,82,-710,81,-710,86,-710,76,-710});
    states[171] = new State(new int[]{116,172,115,173,128,174,129,175,126,176,6,-724,5,-724,120,-724,125,-724,123,-724,121,-724,124,-724,122,-724,137,-724,135,-724,16,-724,70,-724,92,-724,10,-724,98,-724,101,-724,33,-724,104,-724,2,-724,9,-724,12,-724,100,-724,99,-724,32,-724,85,-724,84,-724,83,-724,82,-724,81,-724,86,-724,13,-724,76,-724,51,-724,58,-724,141,-724,143,-724,80,-724,78,-724,160,-724,87,-724,45,-724,42,-724,8,-724,21,-724,22,-724,144,-724,147,-724,145,-724,146,-724,155,-724,158,-724,157,-724,156,-724,11,-724,57,-724,91,-724,40,-724,25,-724,97,-724,54,-724,35,-724,55,-724,102,-724,47,-724,36,-724,53,-724,60,-724,74,-724,72,-724,38,-724,71,-724,138,-724,136,-724,118,-724,117,-724,131,-724,132,-724,133,-724,134,-724,130,-724},new int[]{-196,37});
    states[172] = new State(-729);
    states[173] = new State(-730);
    states[174] = new State(-731);
    states[175] = new State(-732);
    states[176] = new State(-733);
    states[177] = new State(new int[]{138,178,136,1507,118,1510,117,1511,131,1512,132,1513,133,1514,134,1515,130,1516,116,-726,115,-726,128,-726,129,-726,126,-726,6,-726,5,-726,120,-726,125,-726,123,-726,121,-726,124,-726,122,-726,137,-726,135,-726,16,-726,70,-726,92,-726,10,-726,98,-726,101,-726,33,-726,104,-726,2,-726,9,-726,12,-726,100,-726,99,-726,32,-726,85,-726,84,-726,83,-726,82,-726,81,-726,86,-726,13,-726,76,-726,51,-726,58,-726,141,-726,143,-726,80,-726,78,-726,160,-726,87,-726,45,-726,42,-726,8,-726,21,-726,22,-726,144,-726,147,-726,145,-726,146,-726,155,-726,158,-726,157,-726,156,-726,11,-726,57,-726,91,-726,40,-726,25,-726,97,-726,54,-726,35,-726,55,-726,102,-726,47,-726,36,-726,53,-726,60,-726,74,-726,72,-726,38,-726,71,-726},new int[]{-197,39});
    states[178] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,24,492},new int[]{-283,179,-278,180,-179,181,-146,218,-150,49,-151,52,-270,498});
    states[179] = new State(-740);
    states[180] = new State(-741);
    states[181] = new State(new int[]{7,182,4,185,123,187,8,-629,138,-629,136,-629,118,-629,117,-629,131,-629,132,-629,133,-629,134,-629,130,-629,116,-629,115,-629,128,-629,129,-629,126,-629,6,-629,5,-629,120,-629,125,-629,121,-629,124,-629,122,-629,137,-629,135,-629,16,-629,70,-629,92,-629,10,-629,98,-629,101,-629,33,-629,104,-629,2,-629,9,-629,12,-629,100,-629,99,-629,32,-629,85,-629,84,-629,83,-629,82,-629,81,-629,86,-629,13,-629,76,-629,51,-629,58,-629,141,-629,143,-629,80,-629,78,-629,160,-629,87,-629,45,-629,42,-629,21,-629,22,-629,144,-629,147,-629,145,-629,146,-629,155,-629,158,-629,157,-629,156,-629,11,-629,57,-629,91,-629,40,-629,25,-629,97,-629,54,-629,35,-629,55,-629,102,-629,47,-629,36,-629,53,-629,60,-629,74,-629,72,-629,38,-629,71,-629},new int[]{-298,184});
    states[182] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-137,183,-146,47,-150,49,-151,52,-292,57,-149,58,-293,148});
    states[183] = new State(-261);
    states[184] = new State(-630);
    states[185] = new State(new int[]{123,187},new int[]{-298,186});
    states[186] = new State(-631);
    states[187] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-296,188,-279,311,-272,192,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-281,1468,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,1469,-223,559,-222,560,-300,1470});
    states[188] = new State(new int[]{121,189,100,190});
    states[189] = new State(-235);
    states[190] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-279,191,-272,192,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-281,1468,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,1469,-223,559,-222,560,-300,1470});
    states[191] = new State(-239);
    states[192] = new State(new int[]{13,193,121,-243,100,-243,120,-243,9,-243,10,-243,8,-243,138,-243,136,-243,118,-243,117,-243,131,-243,132,-243,133,-243,134,-243,130,-243,116,-243,115,-243,128,-243,129,-243,126,-243,6,-243,5,-243,125,-243,123,-243,124,-243,122,-243,137,-243,135,-243,16,-243,70,-243,92,-243,98,-243,101,-243,33,-243,104,-243,2,-243,12,-243,99,-243,32,-243,85,-243,84,-243,83,-243,82,-243,81,-243,86,-243,76,-243,51,-243,58,-243,141,-243,143,-243,80,-243,78,-243,160,-243,87,-243,45,-243,42,-243,21,-243,22,-243,144,-243,147,-243,145,-243,146,-243,155,-243,158,-243,157,-243,156,-243,11,-243,57,-243,91,-243,40,-243,25,-243,97,-243,54,-243,35,-243,55,-243,102,-243,47,-243,36,-243,53,-243,60,-243,74,-243,72,-243,38,-243,71,-243,127,-243,110,-243});
    states[193] = new State(-244);
    states[194] = new State(new int[]{6,1505,116,245,115,246,128,247,129,248,13,-248,121,-248,100,-248,120,-248,9,-248,10,-248,8,-248,138,-248,136,-248,118,-248,117,-248,131,-248,132,-248,133,-248,134,-248,130,-248,126,-248,5,-248,125,-248,123,-248,124,-248,122,-248,137,-248,135,-248,16,-248,70,-248,92,-248,98,-248,101,-248,33,-248,104,-248,2,-248,12,-248,99,-248,32,-248,85,-248,84,-248,83,-248,82,-248,81,-248,86,-248,76,-248,51,-248,58,-248,141,-248,143,-248,80,-248,78,-248,160,-248,87,-248,45,-248,42,-248,21,-248,22,-248,144,-248,147,-248,145,-248,146,-248,155,-248,158,-248,157,-248,156,-248,11,-248,57,-248,91,-248,40,-248,25,-248,97,-248,54,-248,35,-248,55,-248,102,-248,47,-248,36,-248,53,-248,60,-248,74,-248,72,-248,38,-248,71,-248,127,-248,110,-248},new int[]{-192,195});
    states[195] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283},new int[]{-105,196,-106,313,-179,350,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281});
    states[196] = new State(new int[]{118,252,117,253,131,254,132,255,133,256,134,257,130,258,6,-252,116,-252,115,-252,128,-252,129,-252,13,-252,121,-252,100,-252,120,-252,9,-252,10,-252,8,-252,138,-252,136,-252,126,-252,5,-252,125,-252,123,-252,124,-252,122,-252,137,-252,135,-252,16,-252,70,-252,92,-252,98,-252,101,-252,33,-252,104,-252,2,-252,12,-252,99,-252,32,-252,85,-252,84,-252,83,-252,82,-252,81,-252,86,-252,76,-252,51,-252,58,-252,141,-252,143,-252,80,-252,78,-252,160,-252,87,-252,45,-252,42,-252,21,-252,22,-252,144,-252,147,-252,145,-252,146,-252,155,-252,158,-252,157,-252,156,-252,11,-252,57,-252,91,-252,40,-252,25,-252,97,-252,54,-252,35,-252,55,-252,102,-252,47,-252,36,-252,53,-252,60,-252,74,-252,72,-252,38,-252,71,-252,127,-252,110,-252},new int[]{-194,197});
    states[197] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283},new int[]{-106,198,-179,350,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281});
    states[198] = new State(new int[]{8,199,118,-254,117,-254,131,-254,132,-254,133,-254,134,-254,130,-254,6,-254,116,-254,115,-254,128,-254,129,-254,13,-254,121,-254,100,-254,120,-254,9,-254,10,-254,138,-254,136,-254,126,-254,5,-254,125,-254,123,-254,124,-254,122,-254,137,-254,135,-254,16,-254,70,-254,92,-254,98,-254,101,-254,33,-254,104,-254,2,-254,12,-254,99,-254,32,-254,85,-254,84,-254,83,-254,82,-254,81,-254,86,-254,76,-254,51,-254,58,-254,141,-254,143,-254,80,-254,78,-254,160,-254,87,-254,45,-254,42,-254,21,-254,22,-254,144,-254,147,-254,145,-254,146,-254,155,-254,158,-254,157,-254,156,-254,11,-254,57,-254,91,-254,40,-254,25,-254,97,-254,54,-254,35,-254,55,-254,102,-254,47,-254,36,-254,53,-254,60,-254,74,-254,72,-254,38,-254,71,-254,127,-254,110,-254});
    states[199] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337,9,-185},new int[]{-74,200,-72,202,-93,1504,-89,205,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[200] = new State(new int[]{9,201});
    states[201] = new State(-259);
    states[202] = new State(new int[]{100,203,9,-184,12,-184});
    states[203] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-93,204,-89,205,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[204] = new State(-187);
    states[205] = new State(new int[]{13,206,16,210,6,1498,100,-188,9,-188,12,-188,5,-188});
    states[206] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-89,207,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[207] = new State(new int[]{5,208,13,206,16,210});
    states[208] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-89,209,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[209] = new State(new int[]{13,206,16,210,6,-123,100,-123,9,-123,12,-123,5,-123,92,-123,10,-123,98,-123,101,-123,33,-123,104,-123,2,-123,99,-123,32,-123,85,-123,84,-123,83,-123,82,-123,81,-123,86,-123});
    states[210] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-90,211,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847});
    states[211] = new State(new int[]{120,237,125,238,123,239,121,240,124,241,122,242,137,243,13,-122,16,-122,6,-122,100,-122,9,-122,12,-122,5,-122,92,-122,10,-122,98,-122,101,-122,33,-122,104,-122,2,-122,99,-122,32,-122,85,-122,84,-122,83,-122,82,-122,81,-122,86,-122},new int[]{-191,212});
    states[212] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-80,213,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847});
    states[213] = new State(new int[]{116,245,115,246,128,247,129,248,120,-119,125,-119,123,-119,121,-119,124,-119,122,-119,137,-119,13,-119,16,-119,6,-119,100,-119,9,-119,12,-119,5,-119,92,-119,10,-119,98,-119,101,-119,33,-119,104,-119,2,-119,99,-119,32,-119,85,-119,84,-119,83,-119,82,-119,81,-119,86,-119},new int[]{-192,214});
    states[214] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-13,215,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847});
    states[215] = new State(new int[]{136,250,138,251,118,252,117,253,131,254,132,255,133,256,134,257,130,258,116,-132,115,-132,128,-132,129,-132,120,-132,125,-132,123,-132,121,-132,124,-132,122,-132,137,-132,13,-132,16,-132,6,-132,100,-132,9,-132,12,-132,5,-132,92,-132,10,-132,98,-132,101,-132,33,-132,104,-132,2,-132,99,-132,32,-132,85,-132,84,-132,83,-132,82,-132,81,-132,86,-132},new int[]{-200,216,-194,219});
    states[216] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-283,217,-179,181,-146,218,-150,49,-151,52});
    states[217] = new State(-137);
    states[218] = new State(-260);
    states[219] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-10,220,-269,221,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-11,847});
    states[220] = new State(-144);
    states[221] = new State(-145);
    states[222] = new State(new int[]{4,224,11,226,7,824,142,826,8,827,136,-155,138,-155,118,-155,117,-155,131,-155,132,-155,133,-155,134,-155,130,-155,116,-155,115,-155,128,-155,129,-155,120,-155,125,-155,123,-155,121,-155,124,-155,122,-155,137,-155,13,-155,16,-155,6,-155,100,-155,9,-155,12,-155,5,-155,92,-155,10,-155,98,-155,101,-155,33,-155,104,-155,2,-155,99,-155,32,-155,85,-155,84,-155,83,-155,82,-155,81,-155,86,-155,119,-153},new int[]{-12,223});
    states[223] = new State(-175);
    states[224] = new State(new int[]{123,187},new int[]{-298,225});
    states[225] = new State(-176);
    states[226] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337,5,1500,12,-185},new int[]{-120,227,-74,229,-89,231,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853,-72,202,-93,1504});
    states[227] = new State(new int[]{12,228});
    states[228] = new State(-177);
    states[229] = new State(new int[]{12,230});
    states[230] = new State(-181);
    states[231] = new State(new int[]{5,232,13,206,16,210,6,1498,100,-188,12,-188});
    states[232] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337,5,-707,12,-707},new int[]{-121,233,-89,1497,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[233] = new State(new int[]{5,234,12,-712});
    states[234] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-89,235,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[235] = new State(new int[]{13,206,16,210,12,-714});
    states[236] = new State(new int[]{120,237,125,238,123,239,121,240,124,241,122,242,137,243,13,-120,16,-120,6,-120,100,-120,9,-120,12,-120,5,-120,92,-120,10,-120,98,-120,101,-120,33,-120,104,-120,2,-120,99,-120,32,-120,85,-120,84,-120,83,-120,82,-120,81,-120,86,-120},new int[]{-191,212});
    states[237] = new State(-124);
    states[238] = new State(-125);
    states[239] = new State(-126);
    states[240] = new State(-127);
    states[241] = new State(-128);
    states[242] = new State(-129);
    states[243] = new State(-130);
    states[244] = new State(new int[]{116,245,115,246,128,247,129,248,120,-118,125,-118,123,-118,121,-118,124,-118,122,-118,137,-118,13,-118,16,-118,6,-118,100,-118,9,-118,12,-118,5,-118,92,-118,10,-118,98,-118,101,-118,33,-118,104,-118,2,-118,99,-118,32,-118,85,-118,84,-118,83,-118,82,-118,81,-118,86,-118},new int[]{-192,214});
    states[245] = new State(-133);
    states[246] = new State(-134);
    states[247] = new State(-135);
    states[248] = new State(-136);
    states[249] = new State(new int[]{136,250,138,251,118,252,117,253,131,254,132,255,133,256,134,257,130,258,116,-131,115,-131,128,-131,129,-131,120,-131,125,-131,123,-131,121,-131,124,-131,122,-131,137,-131,13,-131,16,-131,6,-131,100,-131,9,-131,12,-131,5,-131,92,-131,10,-131,98,-131,101,-131,33,-131,104,-131,2,-131,99,-131,32,-131,85,-131,84,-131,83,-131,82,-131,81,-131,86,-131},new int[]{-200,216,-194,219});
    states[250] = new State(-734);
    states[251] = new State(-735);
    states[252] = new State(-146);
    states[253] = new State(-147);
    states[254] = new State(-148);
    states[255] = new State(-149);
    states[256] = new State(-150);
    states[257] = new State(-151);
    states[258] = new State(-152);
    states[259] = new State(-141);
    states[260] = new State(-169);
    states[261] = new State(new int[]{26,1486,143,48,85,50,86,51,80,53,78,54,160,55,87,56,11,-858,17,-858,8,-858,7,-858,142,-858,4,-858,15,-858,110,-858,111,-858,112,-858,113,-858,114,-858,92,-858,10,-858,5,-858,98,-858,101,-858,33,-858,104,-858,2,-858,127,-858,138,-858,136,-858,118,-858,117,-858,131,-858,132,-858,133,-858,134,-858,130,-858,116,-858,115,-858,128,-858,129,-858,126,-858,6,-858,120,-858,125,-858,123,-858,121,-858,124,-858,122,-858,137,-858,135,-858,16,-858,70,-858,9,-858,12,-858,100,-858,99,-858,32,-858,84,-858,83,-858,82,-858,81,-858,13,-858,119,-858,76,-858,51,-858,58,-858,141,-858,45,-858,42,-858,21,-858,22,-858,144,-858,147,-858,145,-858,146,-858,155,-858,158,-858,157,-858,156,-858,57,-858,91,-858,40,-858,25,-858,97,-858,54,-858,35,-858,55,-858,102,-858,47,-858,36,-858,53,-858,60,-858,74,-858,72,-858,38,-858,71,-858},new int[]{-283,262,-179,181,-146,218,-150,49,-151,52});
    states[262] = new State(new int[]{11,264,8,635,92,-643,10,-643,98,-643,101,-643,33,-643,104,-643,2,-643,138,-643,136,-643,118,-643,117,-643,131,-643,132,-643,133,-643,134,-643,130,-643,116,-643,115,-643,128,-643,129,-643,126,-643,6,-643,5,-643,120,-643,125,-643,123,-643,121,-643,124,-643,122,-643,137,-643,135,-643,16,-643,70,-643,9,-643,12,-643,100,-643,99,-643,32,-643,85,-643,84,-643,83,-643,82,-643,81,-643,86,-643,13,-643,76,-643,51,-643,58,-643,141,-643,143,-643,80,-643,78,-643,160,-643,87,-643,45,-643,42,-643,21,-643,22,-643,144,-643,147,-643,145,-643,146,-643,155,-643,158,-643,157,-643,156,-643,57,-643,91,-643,40,-643,25,-643,97,-643,54,-643,35,-643,55,-643,102,-643,47,-643,36,-643,53,-643,60,-643,74,-643,72,-643,38,-643,71,-643},new int[]{-69,263});
    states[263] = new State(-636);
    states[264] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,831,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680,12,-811},new int[]{-66,265,-70,638,-87,394,-86,27,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,832,-99,657,-323,675});
    states[265] = new State(new int[]{12,266});
    states[266] = new State(new int[]{8,268,92,-635,10,-635,98,-635,101,-635,33,-635,104,-635,2,-635,138,-635,136,-635,118,-635,117,-635,131,-635,132,-635,133,-635,134,-635,130,-635,116,-635,115,-635,128,-635,129,-635,126,-635,6,-635,5,-635,120,-635,125,-635,123,-635,121,-635,124,-635,122,-635,137,-635,135,-635,16,-635,70,-635,9,-635,12,-635,100,-635,99,-635,32,-635,85,-635,84,-635,83,-635,82,-635,81,-635,86,-635,13,-635,76,-635,51,-635,58,-635,141,-635,143,-635,80,-635,78,-635,160,-635,87,-635,45,-635,42,-635,21,-635,22,-635,144,-635,147,-635,145,-635,146,-635,155,-635,158,-635,157,-635,156,-635,11,-635,57,-635,91,-635,40,-635,25,-635,97,-635,54,-635,35,-635,55,-635,102,-635,47,-635,36,-635,53,-635,60,-635,74,-635,72,-635,38,-635,71,-635},new int[]{-5,267});
    states[267] = new State(-637);
    states[268] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,998,135,839,116,351,115,352,63,337,9,-198},new int[]{-65,269,-64,271,-84,1001,-83,274,-89,275,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853,-94,1002,-243,1003});
    states[269] = new State(new int[]{9,270});
    states[270] = new State(-634);
    states[271] = new State(new int[]{100,272,9,-199});
    states[272] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,998,135,839,116,351,115,352,63,337},new int[]{-84,273,-83,274,-89,275,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853,-94,1002,-243,1003});
    states[273] = new State(-201);
    states[274] = new State(-417);
    states[275] = new State(new int[]{13,206,16,210,100,-194,9,-194,92,-194,10,-194,98,-194,101,-194,33,-194,104,-194,2,-194,12,-194,99,-194,32,-194,85,-194,84,-194,83,-194,82,-194,81,-194,86,-194});
    states[276] = new State(-170);
    states[277] = new State(new int[]{144,279,147,280,7,-822,11,-822,17,-822,138,-822,136,-822,118,-822,117,-822,131,-822,132,-822,133,-822,134,-822,130,-822,116,-822,115,-822,128,-822,129,-822,126,-822,6,-822,5,-822,120,-822,125,-822,123,-822,121,-822,124,-822,122,-822,137,-822,135,-822,16,-822,70,-822,92,-822,10,-822,98,-822,101,-822,33,-822,104,-822,2,-822,9,-822,12,-822,100,-822,99,-822,32,-822,85,-822,84,-822,83,-822,82,-822,81,-822,86,-822,13,-822,119,-822,76,-822,51,-822,58,-822,141,-822,143,-822,80,-822,78,-822,160,-822,87,-822,45,-822,42,-822,8,-822,21,-822,22,-822,145,-822,146,-822,155,-822,158,-822,157,-822,156,-822,57,-822,91,-822,40,-822,25,-822,97,-822,54,-822,35,-822,55,-822,102,-822,47,-822,36,-822,53,-822,60,-822,74,-822,72,-822,38,-822,71,-822,127,-822,110,-822,4,-822,142,-822},new int[]{-165,278});
    states[278] = new State(-826);
    states[279] = new State(-820);
    states[280] = new State(-821);
    states[281] = new State(-825);
    states[282] = new State(-823);
    states[283] = new State(-824);
    states[284] = new State(-171);
    states[285] = new State(-190);
    states[286] = new State(-191);
    states[287] = new State(-192);
    states[288] = new State(-193);
    states[289] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,290,-150,49,-151,52});
    states[290] = new State(-172);
    states[291] = new State(-173);
    states[292] = new State(new int[]{8,293});
    states[293] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-283,294,-179,181,-146,218,-150,49,-151,52});
    states[294] = new State(new int[]{9,295});
    states[295] = new State(-622);
    states[296] = new State(-174);
    states[297] = new State(new int[]{8,298});
    states[298] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-283,299,-282,301,-179,303,-146,218,-150,49,-151,52});
    states[299] = new State(new int[]{9,300});
    states[300] = new State(-623);
    states[301] = new State(new int[]{9,302});
    states[302] = new State(-624);
    states[303] = new State(new int[]{7,182,4,304,123,306,125,1484,9,-629},new int[]{-298,184,-299,1485});
    states[304] = new State(new int[]{123,306,125,1484},new int[]{-298,186,-299,305});
    states[305] = new State(-628);
    states[306] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,24,492,48,500,49,547,34,551,73,555,44,561,37,602,121,-242,100,-242},new int[]{-296,188,-297,307,-279,311,-272,192,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-281,1468,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,1469,-223,559,-222,560,-300,1470,-280,1483});
    states[307] = new State(new int[]{121,308,100,309});
    states[308] = new State(-237);
    states[309] = new State(-242,new int[]{-280,310});
    states[310] = new State(-241);
    states[311] = new State(-238);
    states[312] = new State(new int[]{118,252,117,253,131,254,132,255,133,256,134,257,130,258,6,-251,116,-251,115,-251,128,-251,129,-251,13,-251,121,-251,100,-251,120,-251,9,-251,10,-251,8,-251,138,-251,136,-251,126,-251,5,-251,125,-251,123,-251,124,-251,122,-251,137,-251,135,-251,16,-251,70,-251,92,-251,98,-251,101,-251,33,-251,104,-251,2,-251,12,-251,99,-251,32,-251,85,-251,84,-251,83,-251,82,-251,81,-251,86,-251,76,-251,51,-251,58,-251,141,-251,143,-251,80,-251,78,-251,160,-251,87,-251,45,-251,42,-251,21,-251,22,-251,144,-251,147,-251,145,-251,146,-251,155,-251,158,-251,157,-251,156,-251,11,-251,57,-251,91,-251,40,-251,25,-251,97,-251,54,-251,35,-251,55,-251,102,-251,47,-251,36,-251,53,-251,60,-251,74,-251,72,-251,38,-251,71,-251,127,-251,110,-251},new int[]{-194,197});
    states[313] = new State(new int[]{8,199,118,-253,117,-253,131,-253,132,-253,133,-253,134,-253,130,-253,6,-253,116,-253,115,-253,128,-253,129,-253,13,-253,121,-253,100,-253,120,-253,9,-253,10,-253,138,-253,136,-253,126,-253,5,-253,125,-253,123,-253,124,-253,122,-253,137,-253,135,-253,16,-253,70,-253,92,-253,98,-253,101,-253,33,-253,104,-253,2,-253,12,-253,99,-253,32,-253,85,-253,84,-253,83,-253,82,-253,81,-253,86,-253,76,-253,51,-253,58,-253,141,-253,143,-253,80,-253,78,-253,160,-253,87,-253,45,-253,42,-253,21,-253,22,-253,144,-253,147,-253,145,-253,146,-253,155,-253,158,-253,157,-253,156,-253,11,-253,57,-253,91,-253,40,-253,25,-253,97,-253,54,-253,35,-253,55,-253,102,-253,47,-253,36,-253,53,-253,60,-253,74,-253,72,-253,38,-253,71,-253,127,-253,110,-253});
    states[314] = new State(new int[]{7,182,127,315,123,187,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,13,-255,121,-255,100,-255,120,-255,9,-255,10,-255,138,-255,136,-255,126,-255,5,-255,125,-255,124,-255,122,-255,137,-255,135,-255,16,-255,70,-255,92,-255,98,-255,101,-255,33,-255,104,-255,2,-255,12,-255,99,-255,32,-255,85,-255,84,-255,83,-255,82,-255,81,-255,86,-255,76,-255,51,-255,58,-255,141,-255,143,-255,80,-255,78,-255,160,-255,87,-255,45,-255,42,-255,21,-255,22,-255,144,-255,147,-255,145,-255,146,-255,155,-255,158,-255,157,-255,156,-255,11,-255,57,-255,91,-255,40,-255,25,-255,97,-255,54,-255,35,-255,55,-255,102,-255,47,-255,36,-255,53,-255,60,-255,74,-255,72,-255,38,-255,71,-255,110,-255},new int[]{-298,634});
    states[315] = new State(new int[]{8,317,143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-279,316,-272,192,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-281,1468,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,1469,-223,559,-222,560,-300,1470});
    states[316] = new State(-288);
    states[317] = new State(new int[]{9,318,143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-79,323,-77,329,-276,330,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[318] = new State(new int[]{127,319,121,-292,100,-292,120,-292,9,-292,10,-292,8,-292,138,-292,136,-292,118,-292,117,-292,131,-292,132,-292,133,-292,134,-292,130,-292,116,-292,115,-292,128,-292,129,-292,126,-292,6,-292,5,-292,125,-292,123,-292,124,-292,122,-292,137,-292,135,-292,16,-292,70,-292,92,-292,98,-292,101,-292,33,-292,104,-292,2,-292,12,-292,99,-292,32,-292,85,-292,84,-292,83,-292,82,-292,81,-292,86,-292,13,-292,76,-292,51,-292,58,-292,141,-292,143,-292,80,-292,78,-292,160,-292,87,-292,45,-292,42,-292,21,-292,22,-292,144,-292,147,-292,145,-292,146,-292,155,-292,158,-292,157,-292,156,-292,11,-292,57,-292,91,-292,40,-292,25,-292,97,-292,54,-292,35,-292,55,-292,102,-292,47,-292,36,-292,53,-292,60,-292,74,-292,72,-292,38,-292,71,-292,110,-292});
    states[319] = new State(new int[]{8,321,143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-279,320,-272,192,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-281,1468,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,1469,-223,559,-222,560,-300,1470});
    states[320] = new State(-290);
    states[321] = new State(new int[]{9,322,143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-79,323,-77,329,-276,330,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[322] = new State(new int[]{127,319,121,-294,100,-294,120,-294,9,-294,10,-294,8,-294,138,-294,136,-294,118,-294,117,-294,131,-294,132,-294,133,-294,134,-294,130,-294,116,-294,115,-294,128,-294,129,-294,126,-294,6,-294,5,-294,125,-294,123,-294,124,-294,122,-294,137,-294,135,-294,16,-294,70,-294,92,-294,98,-294,101,-294,33,-294,104,-294,2,-294,12,-294,99,-294,32,-294,85,-294,84,-294,83,-294,82,-294,81,-294,86,-294,13,-294,76,-294,51,-294,58,-294,141,-294,143,-294,80,-294,78,-294,160,-294,87,-294,45,-294,42,-294,21,-294,22,-294,144,-294,147,-294,145,-294,146,-294,155,-294,158,-294,157,-294,156,-294,11,-294,57,-294,91,-294,40,-294,25,-294,97,-294,54,-294,35,-294,55,-294,102,-294,47,-294,36,-294,53,-294,60,-294,74,-294,72,-294,38,-294,71,-294,110,-294});
    states[323] = new State(new int[]{9,324,100,357});
    states[324] = new State(new int[]{127,325,13,-250,121,-250,100,-250,120,-250,9,-250,10,-250,8,-250,138,-250,136,-250,118,-250,117,-250,131,-250,132,-250,133,-250,134,-250,130,-250,116,-250,115,-250,128,-250,129,-250,126,-250,6,-250,5,-250,125,-250,123,-250,124,-250,122,-250,137,-250,135,-250,16,-250,70,-250,92,-250,98,-250,101,-250,33,-250,104,-250,2,-250,12,-250,99,-250,32,-250,85,-250,84,-250,83,-250,82,-250,81,-250,86,-250,76,-250,51,-250,58,-250,141,-250,143,-250,80,-250,78,-250,160,-250,87,-250,45,-250,42,-250,21,-250,22,-250,144,-250,147,-250,145,-250,146,-250,155,-250,158,-250,157,-250,156,-250,11,-250,57,-250,91,-250,40,-250,25,-250,97,-250,54,-250,35,-250,55,-250,102,-250,47,-250,36,-250,53,-250,60,-250,74,-250,72,-250,38,-250,71,-250,110,-250});
    states[325] = new State(new int[]{8,327,143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-279,326,-272,192,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-281,1468,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,1469,-223,559,-222,560,-300,1470});
    states[326] = new State(-291);
    states[327] = new State(new int[]{9,328,143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-79,323,-77,329,-276,330,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[328] = new State(new int[]{127,319,121,-295,100,-295,120,-295,9,-295,10,-295,8,-295,138,-295,136,-295,118,-295,117,-295,131,-295,132,-295,133,-295,134,-295,130,-295,116,-295,115,-295,128,-295,129,-295,126,-295,6,-295,5,-295,125,-295,123,-295,124,-295,122,-295,137,-295,135,-295,16,-295,70,-295,92,-295,98,-295,101,-295,33,-295,104,-295,2,-295,12,-295,99,-295,32,-295,85,-295,84,-295,83,-295,82,-295,81,-295,86,-295,13,-295,76,-295,51,-295,58,-295,141,-295,143,-295,80,-295,78,-295,160,-295,87,-295,45,-295,42,-295,21,-295,22,-295,144,-295,147,-295,145,-295,146,-295,155,-295,158,-295,157,-295,156,-295,11,-295,57,-295,91,-295,40,-295,25,-295,97,-295,54,-295,35,-295,55,-295,102,-295,47,-295,36,-295,53,-295,60,-295,74,-295,72,-295,38,-295,71,-295,110,-295});
    states[329] = new State(-262);
    states[330] = new State(new int[]{120,331,9,-264,100,-264});
    states[331] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,332,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[332] = new State(new int[]{70,28,9,-265,100,-265});
    states[333] = new State(-745);
    states[334] = new State(-771);
    states[335] = new State(-772);
    states[336] = new State(-765);
    states[337] = new State(new int[]{8,338});
    states[338] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,354},new int[]{-284,339,-283,341,-179,342,-146,218,-150,49,-151,52,-273,344,-272,345,-92,194,-105,312,-106,313,-16,347,-198,348,-164,353,-166,277,-165,281,-300,1482});
    states[339] = new State(new int[]{9,340});
    states[340] = new State(-759);
    states[341] = new State(-632);
    states[342] = new State(new int[]{7,182,4,185,123,187,9,-629,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,13,-255},new int[]{-298,343});
    states[343] = new State(new int[]{9,-630,13,-234});
    states[344] = new State(-633);
    states[345] = new State(new int[]{13,346});
    states[346] = new State(-225);
    states[347] = new State(-256);
    states[348] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283},new int[]{-106,349,-179,350,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281});
    states[349] = new State(new int[]{8,199,118,-257,117,-257,131,-257,132,-257,133,-257,134,-257,130,-257,6,-257,116,-257,115,-257,128,-257,129,-257,13,-257,121,-257,100,-257,120,-257,9,-257,10,-257,138,-257,136,-257,126,-257,5,-257,125,-257,123,-257,124,-257,122,-257,137,-257,135,-257,16,-257,70,-257,92,-257,98,-257,101,-257,33,-257,104,-257,2,-257,12,-257,99,-257,32,-257,85,-257,84,-257,83,-257,82,-257,81,-257,86,-257,76,-257,51,-257,58,-257,141,-257,143,-257,80,-257,78,-257,160,-257,87,-257,45,-257,42,-257,21,-257,22,-257,144,-257,147,-257,145,-257,146,-257,155,-257,158,-257,157,-257,156,-257,11,-257,57,-257,91,-257,40,-257,25,-257,97,-257,54,-257,35,-257,55,-257,102,-257,47,-257,36,-257,53,-257,60,-257,74,-257,72,-257,38,-257,71,-257,127,-257,110,-257});
    states[350] = new State(new int[]{7,182,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,13,-255,121,-255,100,-255,120,-255,9,-255,10,-255,138,-255,136,-255,126,-255,5,-255,125,-255,123,-255,124,-255,122,-255,137,-255,135,-255,16,-255,70,-255,92,-255,98,-255,101,-255,33,-255,104,-255,2,-255,12,-255,99,-255,32,-255,85,-255,84,-255,83,-255,82,-255,81,-255,86,-255,76,-255,51,-255,58,-255,141,-255,143,-255,80,-255,78,-255,160,-255,87,-255,45,-255,42,-255,21,-255,22,-255,144,-255,147,-255,145,-255,146,-255,155,-255,158,-255,157,-255,156,-255,11,-255,57,-255,91,-255,40,-255,25,-255,97,-255,54,-255,35,-255,55,-255,102,-255,47,-255,36,-255,53,-255,60,-255,74,-255,72,-255,38,-255,71,-255,127,-255,110,-255});
    states[351] = new State(-167);
    states[352] = new State(-168);
    states[353] = new State(-258);
    states[354] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-79,355,-77,329,-276,330,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[355] = new State(new int[]{9,356,100,357});
    states[356] = new State(-250);
    states[357] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-77,358,-276,330,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[358] = new State(-263);
    states[359] = new State(new int[]{13,346,120,-227,9,-227,100,-227,10,-227,121,-227,8,-227,138,-227,136,-227,118,-227,117,-227,131,-227,132,-227,133,-227,134,-227,130,-227,116,-227,115,-227,128,-227,129,-227,126,-227,6,-227,5,-227,125,-227,123,-227,124,-227,122,-227,137,-227,135,-227,16,-227,70,-227,92,-227,98,-227,101,-227,33,-227,104,-227,2,-227,12,-227,99,-227,32,-227,85,-227,84,-227,83,-227,82,-227,81,-227,86,-227,76,-227,51,-227,58,-227,141,-227,143,-227,80,-227,78,-227,160,-227,87,-227,45,-227,42,-227,21,-227,22,-227,144,-227,147,-227,145,-227,146,-227,155,-227,158,-227,157,-227,156,-227,11,-227,57,-227,91,-227,40,-227,25,-227,97,-227,54,-227,35,-227,55,-227,102,-227,47,-227,36,-227,53,-227,60,-227,74,-227,72,-227,38,-227,71,-227,127,-227,110,-227});
    states[360] = new State(new int[]{11,361,7,-834,127,-834,123,-834,8,-834,118,-834,117,-834,131,-834,132,-834,133,-834,134,-834,130,-834,6,-834,116,-834,115,-834,128,-834,129,-834,13,-834,120,-834,9,-834,100,-834,10,-834,121,-834,138,-834,136,-834,126,-834,5,-834,125,-834,124,-834,122,-834,137,-834,135,-834,16,-834,70,-834,92,-834,98,-834,101,-834,33,-834,104,-834,2,-834,12,-834,99,-834,32,-834,85,-834,84,-834,83,-834,82,-834,81,-834,86,-834,76,-834,51,-834,58,-834,141,-834,143,-834,80,-834,78,-834,160,-834,87,-834,45,-834,42,-834,21,-834,22,-834,144,-834,147,-834,145,-834,146,-834,155,-834,158,-834,157,-834,156,-834,57,-834,91,-834,40,-834,25,-834,97,-834,54,-834,35,-834,55,-834,102,-834,47,-834,36,-834,53,-834,60,-834,74,-834,72,-834,38,-834,71,-834,110,-834});
    states[361] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-89,362,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[362] = new State(new int[]{12,363,13,206,16,210});
    states[363] = new State(-283);
    states[364] = new State(-156);
    states[365] = new State(-165);
    states[366] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596,12,-815},new int[]{-68,367,-76,369,-91,447,-86,372,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[367] = new State(new int[]{12,368});
    states[368] = new State(-164);
    states[369] = new State(new int[]{100,370,12,-814,76,-814});
    states[370] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-91,371,-86,372,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[371] = new State(-817);
    states[372] = new State(new int[]{70,28,6,373,100,-818,12,-818,76,-818});
    states[373] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,374,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[374] = new State(new int[]{70,28,100,-819,12,-819,76,-819});
    states[375] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366},new int[]{-95,376,-15,377,-164,334,-166,277,-165,281,-16,335,-56,336,-198,387,-112,388,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456});
    states[376] = new State(-766);
    states[377] = new State(new int[]{7,45,11,150,17,378,138,-764,136,-764,118,-764,117,-764,131,-764,132,-764,133,-764,134,-764,130,-764,116,-764,115,-764,128,-764,129,-764,126,-764,6,-764,5,-764,120,-764,125,-764,123,-764,121,-764,124,-764,122,-764,137,-764,135,-764,16,-764,70,-764,92,-764,10,-764,98,-764,101,-764,33,-764,104,-764,2,-764,9,-764,12,-764,100,-764,99,-764,32,-764,85,-764,84,-764,83,-764,82,-764,81,-764,86,-764,13,-764,76,-764,51,-764,58,-764,141,-764,143,-764,80,-764,78,-764,160,-764,87,-764,45,-764,42,-764,8,-764,21,-764,22,-764,144,-764,147,-764,145,-764,146,-764,155,-764,158,-764,157,-764,156,-764,57,-764,91,-764,40,-764,25,-764,97,-764,54,-764,35,-764,55,-764,102,-764,47,-764,36,-764,53,-764,60,-764,74,-764,72,-764,38,-764,71,-764});
    states[378] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,5,596},new int[]{-119,379,-104,381,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,600,-267,576});
    states[379] = new State(new int[]{12,380});
    states[380] = new State(-802);
    states[381] = new State(new int[]{5,167,6,35});
    states[382] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366},new int[]{-95,383,-268,384,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-96,459});
    states[383] = new State(-767);
    states[384] = new State(-744);
    states[385] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366},new int[]{-95,386,-15,377,-164,334,-166,277,-165,281,-16,335,-56,336,-198,387,-112,388,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456});
    states[386] = new State(-768);
    states[387] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366},new int[]{-95,383,-15,377,-164,334,-166,277,-165,281,-16,335,-56,336,-198,387,-112,388,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456});
    states[388] = new State(-769);
    states[389] = new State(new int[]{141,1481,143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,1111,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366},new int[]{-110,390,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726});
    states[390] = new State(new int[]{11,391,17,398,8,700,7,1106,142,1108,4,1109,110,-775,111,-775,112,-775,113,-775,114,-775,92,-775,10,-775,98,-775,101,-775,33,-775,104,-775,2,-775,138,-775,136,-775,118,-775,117,-775,131,-775,132,-775,133,-775,134,-775,130,-775,116,-775,115,-775,128,-775,129,-775,126,-775,6,-775,5,-775,120,-775,125,-775,123,-775,121,-775,124,-775,122,-775,137,-775,135,-775,16,-775,70,-775,9,-775,12,-775,100,-775,99,-775,32,-775,85,-775,84,-775,83,-775,82,-775,81,-775,86,-775,13,-775,119,-775,76,-775,51,-775,58,-775,141,-775,143,-775,80,-775,78,-775,160,-775,87,-775,45,-775,42,-775,21,-775,22,-775,144,-775,147,-775,145,-775,146,-775,155,-775,158,-775,157,-775,156,-775,57,-775,91,-775,40,-775,25,-775,97,-775,54,-775,35,-775,55,-775,102,-775,47,-775,36,-775,53,-775,60,-775,74,-775,72,-775,38,-775,71,-775});
    states[391] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,831,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680},new int[]{-70,392,-87,394,-86,27,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,832,-99,657,-323,675});
    states[392] = new State(new int[]{12,393,100,153});
    states[393] = new State(-799);
    states[394] = new State(-592);
    states[395] = new State(new int[]{138,-769,136,-769,118,-769,117,-769,131,-769,132,-769,133,-769,134,-769,130,-769,116,-769,115,-769,128,-769,129,-769,126,-769,6,-769,5,-769,120,-769,125,-769,123,-769,121,-769,124,-769,122,-769,137,-769,135,-769,16,-769,70,-769,92,-769,10,-769,98,-769,101,-769,33,-769,104,-769,2,-769,9,-769,12,-769,100,-769,99,-769,32,-769,85,-769,84,-769,83,-769,82,-769,81,-769,86,-769,13,-769,76,-769,51,-769,58,-769,141,-769,143,-769,80,-769,78,-769,160,-769,87,-769,45,-769,42,-769,8,-769,21,-769,22,-769,144,-769,147,-769,145,-769,146,-769,155,-769,158,-769,157,-769,156,-769,11,-769,57,-769,91,-769,40,-769,25,-769,97,-769,54,-769,35,-769,55,-769,102,-769,47,-769,36,-769,53,-769,60,-769,74,-769,72,-769,38,-769,71,-769,119,-762});
    states[396] = new State(-779);
    states[397] = new State(new int[]{11,391,17,398,8,700,7,1106,142,1108,4,1109,15,1116,110,-776,111,-776,112,-776,113,-776,114,-776,92,-776,10,-776,98,-776,101,-776,33,-776,104,-776,2,-776,138,-776,136,-776,118,-776,117,-776,131,-776,132,-776,133,-776,134,-776,130,-776,116,-776,115,-776,128,-776,129,-776,126,-776,6,-776,5,-776,120,-776,125,-776,123,-776,121,-776,124,-776,122,-776,137,-776,135,-776,16,-776,70,-776,9,-776,12,-776,100,-776,99,-776,32,-776,85,-776,84,-776,83,-776,82,-776,81,-776,86,-776,13,-776,119,-776,76,-776,51,-776,58,-776,141,-776,143,-776,80,-776,78,-776,160,-776,87,-776,45,-776,42,-776,21,-776,22,-776,144,-776,147,-776,145,-776,146,-776,155,-776,158,-776,157,-776,156,-776,57,-776,91,-776,40,-776,25,-776,97,-776,54,-776,35,-776,55,-776,102,-776,47,-776,36,-776,53,-776,60,-776,74,-776,72,-776,38,-776,71,-776});
    states[398] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,5,596},new int[]{-119,399,-104,381,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,600,-267,576});
    states[399] = new State(new int[]{12,400});
    states[400] = new State(-801);
    states[401] = new State(-791);
    states[402] = new State(-792);
    states[403] = new State(new int[]{115,405,116,406,117,407,118,408,120,409,121,410,122,411,123,412,124,413,125,414,128,415,129,416,130,417,131,418,132,419,133,420,134,421,135,422,137,423,139,424,140,425,110,427,111,428,112,429,113,430,114,431,119,432},new int[]{-199,404,-193,426});
    states[404] = new State(-827);
    states[405] = new State(-950);
    states[406] = new State(-951);
    states[407] = new State(-952);
    states[408] = new State(-953);
    states[409] = new State(-954);
    states[410] = new State(-955);
    states[411] = new State(-956);
    states[412] = new State(-957);
    states[413] = new State(-958);
    states[414] = new State(-959);
    states[415] = new State(-960);
    states[416] = new State(-961);
    states[417] = new State(-962);
    states[418] = new State(-963);
    states[419] = new State(-964);
    states[420] = new State(-965);
    states[421] = new State(-966);
    states[422] = new State(-967);
    states[423] = new State(-968);
    states[424] = new State(-969);
    states[425] = new State(-970);
    states[426] = new State(-971);
    states[427] = new State(-973);
    states[428] = new State(-974);
    states[429] = new State(-975);
    states[430] = new State(-976);
    states[431] = new State(-977);
    states[432] = new State(-972);
    states[433] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,434,-150,49,-151,52});
    states[434] = new State(-793);
    states[435] = new State(new int[]{53,685,56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596,18,667},new int[]{-86,436,-359,438,-101,530,-100,1101,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-358,1102,-99,1103});
    states[436] = new State(new int[]{9,437,70,28});
    states[437] = new State(-794);
    states[438] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596,53,1112},new int[]{-86,439,-358,441,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[439] = new State(new int[]{9,440,70,28});
    states[440] = new State(-795);
    states[441] = new State(-789);
    states[442] = new State(-796);
    states[443] = new State(-797);
    states[444] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-68,445,-76,369,-91,447,-86,372,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[445] = new State(new int[]{76,446});
    states[446] = new State(-803);
    states[447] = new State(-816);
    states[448] = new State(-804);
    states[449] = new State(-805);
    states[450] = new State(new int[]{7,451,138,-770,136,-770,118,-770,117,-770,131,-770,132,-770,133,-770,134,-770,130,-770,116,-770,115,-770,128,-770,129,-770,126,-770,6,-770,5,-770,120,-770,125,-770,123,-770,121,-770,124,-770,122,-770,137,-770,135,-770,16,-770,70,-770,92,-770,10,-770,98,-770,101,-770,33,-770,104,-770,2,-770,9,-770,12,-770,100,-770,99,-770,32,-770,85,-770,84,-770,83,-770,82,-770,81,-770,86,-770,13,-770,76,-770,51,-770,58,-770,141,-770,143,-770,80,-770,78,-770,160,-770,87,-770,45,-770,42,-770,8,-770,21,-770,22,-770,144,-770,147,-770,145,-770,146,-770,155,-770,158,-770,157,-770,156,-770,11,-770,57,-770,91,-770,40,-770,25,-770,97,-770,54,-770,35,-770,55,-770,102,-770,47,-770,36,-770,53,-770,60,-770,74,-770,72,-770,38,-770,71,-770});
    states[451] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,403},new int[]{-147,452,-146,453,-150,49,-151,52,-292,454,-149,58,-190,455});
    states[452] = new State(-807);
    states[453] = new State(-842);
    states[454] = new State(-843);
    states[455] = new State(-844);
    states[456] = new State(-777);
    states[457] = new State(-746);
    states[458] = new State(-747);
    states[459] = new State(new int[]{119,460});
    states[460] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366},new int[]{-95,461,-268,462,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-96,459});
    states[461] = new State(-742);
    states[462] = new State(-743);
    states[463] = new State(-751);
    states[464] = new State(new int[]{8,465,138,-736,136,-736,118,-736,117,-736,131,-736,132,-736,133,-736,134,-736,130,-736,116,-736,115,-736,128,-736,129,-736,126,-736,6,-736,5,-736,120,-736,125,-736,123,-736,121,-736,124,-736,122,-736,137,-736,135,-736,16,-736,70,-736,92,-736,10,-736,98,-736,101,-736,33,-736,104,-736,2,-736,9,-736,12,-736,100,-736,99,-736,32,-736,85,-736,84,-736,83,-736,82,-736,81,-736,86,-736,13,-736,76,-736,51,-736,58,-736,141,-736,143,-736,80,-736,78,-736,160,-736,87,-736,45,-736,42,-736,21,-736,22,-736,144,-736,147,-736,145,-736,146,-736,155,-736,158,-736,157,-736,156,-736,11,-736,57,-736,91,-736,40,-736,25,-736,97,-736,54,-736,35,-736,55,-736,102,-736,47,-736,36,-736,53,-736,60,-736,74,-736,72,-736,38,-736,71,-736});
    states[465] = new State(new int[]{14,470,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,53,472,143,48,85,50,86,51,80,53,78,54,160,55,87,56,11,924,8,937},new int[]{-353,466,-351,1480,-15,471,-164,334,-166,277,-165,281,-16,335,-340,1471,-283,1472,-179,181,-146,218,-150,49,-151,52,-343,1478,-344,1479});
    states[466] = new State(new int[]{9,467,10,468,100,1476});
    states[467] = new State(-648);
    states[468] = new State(new int[]{14,470,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,53,472,143,48,85,50,86,51,80,53,78,54,160,55,87,56,11,924,8,937},new int[]{-351,469,-15,471,-164,334,-166,277,-165,281,-16,335,-340,1471,-283,1472,-179,181,-146,218,-150,49,-151,52,-343,1478,-344,1479});
    states[469] = new State(-685);
    states[470] = new State(-687);
    states[471] = new State(-688);
    states[472] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,473,-150,49,-151,52});
    states[473] = new State(new int[]{5,474,9,-690,10,-690,100,-690});
    states[474] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,475,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[475] = new State(-689);
    states[476] = new State(new int[]{9,477,143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-79,323,-77,329,-276,330,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[477] = new State(new int[]{127,319});
    states[478] = new State(-228);
    states[479] = new State(new int[]{13,480,127,481,120,-233,9,-233,100,-233,10,-233,121,-233,8,-233,138,-233,136,-233,118,-233,117,-233,131,-233,132,-233,133,-233,134,-233,130,-233,116,-233,115,-233,128,-233,129,-233,126,-233,6,-233,5,-233,125,-233,123,-233,124,-233,122,-233,137,-233,135,-233,16,-233,70,-233,92,-233,98,-233,101,-233,33,-233,104,-233,2,-233,12,-233,99,-233,32,-233,85,-233,84,-233,83,-233,82,-233,81,-233,86,-233,76,-233,51,-233,58,-233,141,-233,143,-233,80,-233,78,-233,160,-233,87,-233,45,-233,42,-233,21,-233,22,-233,144,-233,147,-233,145,-233,146,-233,155,-233,158,-233,157,-233,156,-233,11,-233,57,-233,91,-233,40,-233,25,-233,97,-233,54,-233,35,-233,55,-233,102,-233,47,-233,36,-233,53,-233,60,-233,74,-233,72,-233,38,-233,71,-233,110,-233});
    states[480] = new State(-226);
    states[481] = new State(new int[]{8,483,143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-279,482,-272,192,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-281,1468,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,1469,-223,559,-222,560,-300,1470});
    states[482] = new State(-289);
    states[483] = new State(new int[]{9,484,143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-79,323,-77,329,-276,330,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[484] = new State(new int[]{127,319,121,-293,100,-293,120,-293,9,-293,10,-293,8,-293,138,-293,136,-293,118,-293,117,-293,131,-293,132,-293,133,-293,134,-293,130,-293,116,-293,115,-293,128,-293,129,-293,126,-293,6,-293,5,-293,125,-293,123,-293,124,-293,122,-293,137,-293,135,-293,16,-293,70,-293,92,-293,98,-293,101,-293,33,-293,104,-293,2,-293,12,-293,99,-293,32,-293,85,-293,84,-293,83,-293,82,-293,81,-293,86,-293,13,-293,76,-293,51,-293,58,-293,141,-293,143,-293,80,-293,78,-293,160,-293,87,-293,45,-293,42,-293,21,-293,22,-293,144,-293,147,-293,145,-293,146,-293,155,-293,158,-293,157,-293,156,-293,11,-293,57,-293,91,-293,40,-293,25,-293,97,-293,54,-293,35,-293,55,-293,102,-293,47,-293,36,-293,53,-293,60,-293,74,-293,72,-293,38,-293,71,-293,110,-293});
    states[485] = new State(-229);
    states[486] = new State(-230);
    states[487] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,488,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[488] = new State(-266);
    states[489] = new State(-486);
    states[490] = new State(-231);
    states[491] = new State(-267);
    states[492] = new State(new int[]{11,493,58,1466});
    states[493] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,354,12,-279,100,-279},new int[]{-163,494,-271,1465,-272,1464,-92,194,-105,312,-106,313,-179,350,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281});
    states[494] = new State(new int[]{12,495,100,1462});
    states[495] = new State(new int[]{58,496});
    states[496] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,497,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[497] = new State(-273);
    states[498] = new State(-274);
    states[499] = new State(-268);
    states[500] = new State(new int[]{8,1342,23,-315,11,-315,92,-315,84,-315,83,-315,82,-315,81,-315,29,-315,143,-315,85,-315,86,-315,80,-315,78,-315,160,-315,87,-315,62,-315,28,-315,26,-315,44,-315,37,-315,19,-315,30,-315,31,-315,46,-315,27,-315},new int[]{-182,501});
    states[501] = new State(new int[]{23,1333,11,-322,92,-322,84,-322,83,-322,82,-322,81,-322,29,-322,143,-322,85,-322,86,-322,80,-322,78,-322,160,-322,87,-322,62,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322},new int[]{-317,502,-316,1331,-315,1353});
    states[502] = new State(new int[]{11,626,92,-339,84,-339,83,-339,82,-339,81,-339,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-24,503,-31,1254,-33,507,-44,1255,-6,1256,-250,1153,-32,1418,-53,1420,-52,513,-54,1419});
    states[503] = new State(new int[]{92,504,84,1250,83,1251,82,1252,81,1253},new int[]{-7,505});
    states[504] = new State(-297);
    states[505] = new State(new int[]{11,626,92,-339,84,-339,83,-339,82,-339,81,-339,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-31,506,-33,507,-44,1255,-6,1256,-250,1153,-32,1418,-53,1420,-52,513,-54,1419});
    states[506] = new State(-334);
    states[507] = new State(new int[]{10,509,92,-345,84,-345,83,-345,82,-345,81,-345},new int[]{-189,508});
    states[508] = new State(-340);
    states[509] = new State(new int[]{11,626,92,-346,84,-346,83,-346,82,-346,81,-346,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-44,510,-32,511,-6,1256,-250,1153,-53,1420,-52,513,-54,1419});
    states[510] = new State(-348);
    states[511] = new State(new int[]{11,626,92,-342,84,-342,83,-342,82,-342,81,-342,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-53,512,-52,513,-6,514,-250,1153,-54,1419});
    states[512] = new State(-351);
    states[513] = new State(-352);
    states[514] = new State(new int[]{28,1378,26,1379,44,1326,37,1361,19,1381,30,1389,31,1396,11,626,46,1403,27,1412},new int[]{-221,515,-250,516,-218,517,-258,518,-3,519,-229,1380,-227,1314,-224,1325,-228,1360,-226,1387,-214,1400,-215,1401,-217,1402});
    states[515] = new State(-361);
    states[516] = new State(-211);
    states[517] = new State(-362);
    states[518] = new State(-378);
    states[519] = new State(new int[]{30,521,19,1196,46,1268,27,1306,44,1326,37,1361},new int[]{-229,520,-215,1195,-227,1314,-224,1325,-228,1360});
    states[520] = new State(-365);
    states[521] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,8,-377,110,-377,10,-377},new int[]{-171,522,-170,1178,-169,1179,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[522] = new State(new int[]{8,563,110,-470,10,-470},new int[]{-127,523});
    states[523] = new State(new int[]{110,525,10,1167},new int[]{-206,524});
    states[524] = new State(-374);
    states[525] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494},new int[]{-260,526,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[526] = new State(new int[]{10,527});
    states[527] = new State(-423);
    states[528] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,92,-574,10,-574,98,-574,101,-574,33,-574,104,-574,2,-574,9,-574,12,-574,100,-574,99,-574,32,-574,84,-574,83,-574,82,-574,81,-574},new int[]{-146,434,-150,49,-151,52});
    states[529] = new State(new int[]{53,1155,56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596,18,667},new int[]{-86,436,-359,438,-101,530,-110,692,-100,1101,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-358,1102,-99,1103});
    states[530] = new State(new int[]{100,531});
    states[531] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,18,667},new int[]{-78,532,-101,1146,-100,1145,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-99,1103});
    states[532] = new State(new int[]{100,1143,5,544,10,-1007,9,-1007},new int[]{-324,533});
    states[533] = new State(new int[]{10,536,9,-995},new int[]{-331,534});
    states[534] = new State(new int[]{9,535});
    states[535] = new State(-760);
    states[536] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-326,537,-327,1094,-157,540,-146,807,-150,49,-151,52});
    states[537] = new State(new int[]{10,538,9,-996});
    states[538] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-327,539,-157,540,-146,807,-150,49,-151,52});
    states[539] = new State(-1005);
    states[540] = new State(new int[]{100,542,5,544,10,-1007,9,-1007},new int[]{-324,541});
    states[541] = new State(-1006);
    states[542] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,543,-150,49,-151,52});
    states[543] = new State(-344);
    states[544] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,545,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[545] = new State(-1008);
    states[546] = new State(-269);
    states[547] = new State(new int[]{58,548});
    states[548] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,549,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[549] = new State(-280);
    states[550] = new State(-270);
    states[551] = new State(new int[]{58,552,121,-282,100,-282,120,-282,9,-282,10,-282,8,-282,138,-282,136,-282,118,-282,117,-282,131,-282,132,-282,133,-282,134,-282,130,-282,116,-282,115,-282,128,-282,129,-282,126,-282,6,-282,5,-282,125,-282,123,-282,124,-282,122,-282,137,-282,135,-282,16,-282,70,-282,92,-282,98,-282,101,-282,33,-282,104,-282,2,-282,12,-282,99,-282,32,-282,85,-282,84,-282,83,-282,82,-282,81,-282,86,-282,13,-282,76,-282,51,-282,141,-282,143,-282,80,-282,78,-282,160,-282,87,-282,45,-282,42,-282,21,-282,22,-282,144,-282,147,-282,145,-282,146,-282,155,-282,158,-282,157,-282,156,-282,11,-282,57,-282,91,-282,40,-282,25,-282,97,-282,54,-282,35,-282,55,-282,102,-282,47,-282,36,-282,53,-282,60,-282,74,-282,72,-282,38,-282,71,-282,127,-282,110,-282});
    states[552] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,553,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[553] = new State(-281);
    states[554] = new State(-271);
    states[555] = new State(new int[]{58,556});
    states[556] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,557,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[557] = new State(-272);
    states[558] = new State(-232);
    states[559] = new State(-284);
    states[560] = new State(-285);
    states[561] = new State(new int[]{8,563,121,-470,100,-470,120,-470,9,-470,10,-470,138,-470,136,-470,118,-470,117,-470,131,-470,132,-470,133,-470,134,-470,130,-470,116,-470,115,-470,128,-470,129,-470,126,-470,6,-470,5,-470,125,-470,123,-470,124,-470,122,-470,137,-470,135,-470,16,-470,70,-470,92,-470,98,-470,101,-470,33,-470,104,-470,2,-470,12,-470,99,-470,32,-470,85,-470,84,-470,83,-470,82,-470,81,-470,86,-470,13,-470,76,-470,51,-470,58,-470,141,-470,143,-470,80,-470,78,-470,160,-470,87,-470,45,-470,42,-470,21,-470,22,-470,144,-470,147,-470,145,-470,146,-470,155,-470,158,-470,157,-470,156,-470,11,-470,57,-470,91,-470,40,-470,25,-470,97,-470,54,-470,35,-470,55,-470,102,-470,47,-470,36,-470,53,-470,60,-470,74,-470,72,-470,38,-470,71,-470,127,-470,110,-470},new int[]{-127,562});
    states[562] = new State(-286);
    states[563] = new State(new int[]{9,564,11,626,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,53,-212,29,-212,108,-212},new int[]{-128,565,-55,1154,-6,569,-250,1153});
    states[564] = new State(-471);
    states[565] = new State(new int[]{9,566,10,567});
    states[566] = new State(-472);
    states[567] = new State(new int[]{11,626,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,53,-212,29,-212,108,-212},new int[]{-55,568,-6,569,-250,1153});
    states[568] = new State(-474);
    states[569] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,53,610,29,616,108,622,11,626},new int[]{-295,570,-250,516,-158,571,-134,609,-146,608,-150,49,-151,52});
    states[570] = new State(-475);
    states[571] = new State(new int[]{5,572,100,606});
    states[572] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,573,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[573] = new State(new int[]{110,574,9,-476,10,-476});
    states[574] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,575,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[575] = new State(new int[]{70,28,9,-480,10,-480});
    states[576] = new State(-737);
    states[577] = new State(new int[]{70,-609,92,-609,10,-609,98,-609,101,-609,33,-609,104,-609,2,-609,9,-609,12,-609,100,-609,99,-609,32,-609,85,-609,84,-609,83,-609,82,-609,81,-609,86,-609,6,-609,76,-609,5,-609,51,-609,58,-609,141,-609,143,-609,80,-609,78,-609,160,-609,87,-609,45,-609,42,-609,8,-609,21,-609,22,-609,144,-609,147,-609,145,-609,146,-609,155,-609,158,-609,157,-609,156,-609,11,-609,57,-609,91,-609,40,-609,25,-609,97,-609,54,-609,35,-609,55,-609,102,-609,47,-609,36,-609,53,-609,60,-609,74,-609,72,-609,38,-609,71,-609,13,-612});
    states[578] = new State(new int[]{13,579});
    states[579] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366},new int[]{-116,580,-98,583,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,585});
    states[580] = new State(new int[]{5,581,13,579});
    states[581] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366},new int[]{-116,582,-98,583,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,585});
    states[582] = new State(new int[]{13,579,70,-625,92,-625,10,-625,98,-625,101,-625,33,-625,104,-625,2,-625,9,-625,12,-625,100,-625,99,-625,32,-625,85,-625,84,-625,83,-625,82,-625,81,-625,86,-625,6,-625,76,-625,5,-625,51,-625,58,-625,141,-625,143,-625,80,-625,78,-625,160,-625,87,-625,45,-625,42,-625,8,-625,21,-625,22,-625,144,-625,147,-625,145,-625,146,-625,155,-625,158,-625,157,-625,156,-625,11,-625,57,-625,91,-625,40,-625,25,-625,97,-625,54,-625,35,-625,55,-625,102,-625,47,-625,36,-625,53,-625,60,-625,74,-625,72,-625,38,-625,71,-625});
    states[583] = new State(new int[]{16,31,5,-611,13,-611,70,-611,92,-611,10,-611,98,-611,101,-611,33,-611,104,-611,2,-611,9,-611,12,-611,100,-611,99,-611,32,-611,85,-611,84,-611,83,-611,82,-611,81,-611,86,-611,6,-611,76,-611,51,-611,58,-611,141,-611,143,-611,80,-611,78,-611,160,-611,87,-611,45,-611,42,-611,8,-611,21,-611,22,-611,144,-611,147,-611,145,-611,146,-611,155,-611,158,-611,157,-611,156,-611,11,-611,57,-611,91,-611,40,-611,25,-611,97,-611,54,-611,35,-611,55,-611,102,-611,47,-611,36,-611,53,-611,60,-611,74,-611,72,-611,38,-611,71,-611});
    states[584] = new State(new int[]{6,35,120,-645,125,-645,123,-645,121,-645,124,-645,122,-645,137,-645,135,-645,16,-645,70,-645,92,-645,10,-645,98,-645,101,-645,33,-645,104,-645,2,-645,9,-645,12,-645,100,-645,99,-645,32,-645,85,-645,84,-645,83,-645,82,-645,81,-645,86,-645,76,-645,13,-645,5,-645,51,-645,58,-645,141,-645,143,-645,80,-645,78,-645,160,-645,87,-645,45,-645,42,-645,8,-645,21,-645,22,-645,144,-645,147,-645,145,-645,146,-645,155,-645,158,-645,157,-645,156,-645,11,-645,57,-645,91,-645,40,-645,25,-645,97,-645,54,-645,35,-645,55,-645,102,-645,47,-645,36,-645,53,-645,60,-645,74,-645,72,-645,38,-645,71,-645,116,-645,115,-645,128,-645,129,-645,126,-645,138,-645,136,-645,118,-645,117,-645,131,-645,132,-645,133,-645,134,-645,130,-645});
    states[585] = new State(-612);
    states[586] = new State(-610);
    states[587] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-117,588,-98,593,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-242,594});
    states[588] = new State(new int[]{51,589});
    states[589] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-117,590,-98,593,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-242,594});
    states[590] = new State(new int[]{32,591});
    states[591] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-117,592,-98,593,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-242,594});
    states[592] = new State(-626);
    states[593] = new State(new int[]{16,31,51,-613,32,-613,120,-613,125,-613,123,-613,121,-613,124,-613,122,-613,137,-613,135,-613,70,-613,92,-613,10,-613,98,-613,101,-613,33,-613,104,-613,2,-613,9,-613,12,-613,100,-613,99,-613,85,-613,84,-613,83,-613,82,-613,81,-613,86,-613,13,-613,6,-613,76,-613,5,-613,58,-613,141,-613,143,-613,80,-613,78,-613,160,-613,87,-613,45,-613,42,-613,8,-613,21,-613,22,-613,144,-613,147,-613,145,-613,146,-613,155,-613,158,-613,157,-613,156,-613,11,-613,57,-613,91,-613,40,-613,25,-613,97,-613,54,-613,35,-613,55,-613,102,-613,47,-613,36,-613,53,-613,60,-613,74,-613,72,-613,38,-613,71,-613,116,-613,115,-613,128,-613,129,-613,126,-613,138,-613,136,-613,118,-613,117,-613,131,-613,132,-613,133,-613,134,-613,130,-613});
    states[594] = new State(-614);
    states[595] = new State(-606);
    states[596] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,5,-705,70,-705,92,-705,10,-705,98,-705,101,-705,33,-705,104,-705,2,-705,9,-705,12,-705,100,-705,99,-705,32,-705,84,-705,83,-705,82,-705,81,-705,6,-705},new int[]{-114,597,-104,601,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,600,-267,576});
    states[597] = new State(new int[]{5,598,70,-709,92,-709,10,-709,98,-709,101,-709,33,-709,104,-709,2,-709,9,-709,12,-709,100,-709,99,-709,32,-709,85,-709,84,-709,83,-709,82,-709,81,-709,86,-709,6,-709,76,-709});
    states[598] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366},new int[]{-104,599,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,600,-267,576});
    states[599] = new State(new int[]{6,35,70,-711,92,-711,10,-711,98,-711,101,-711,33,-711,104,-711,2,-711,9,-711,12,-711,100,-711,99,-711,32,-711,85,-711,84,-711,83,-711,82,-711,81,-711,86,-711,76,-711});
    states[600] = new State(-736);
    states[601] = new State(new int[]{6,35,5,-704,70,-704,92,-704,10,-704,98,-704,101,-704,33,-704,104,-704,2,-704,9,-704,12,-704,100,-704,99,-704,32,-704,85,-704,84,-704,83,-704,82,-704,81,-704,86,-704,76,-704});
    states[602] = new State(new int[]{8,563,5,-470},new int[]{-127,603});
    states[603] = new State(new int[]{5,604});
    states[604] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,605,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[605] = new State(-287);
    states[606] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-134,607,-146,608,-150,49,-151,52});
    states[607] = new State(-484);
    states[608] = new State(-485);
    states[609] = new State(-483);
    states[610] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-158,611,-134,609,-146,608,-150,49,-151,52});
    states[611] = new State(new int[]{5,612,100,606});
    states[612] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,613,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[613] = new State(new int[]{110,614,9,-477,10,-477});
    states[614] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,615,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[615] = new State(new int[]{70,28,9,-481,10,-481});
    states[616] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-158,617,-134,609,-146,608,-150,49,-151,52});
    states[617] = new State(new int[]{5,618,100,606});
    states[618] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,619,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[619] = new State(new int[]{110,620,9,-478,10,-478});
    states[620] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,621,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[621] = new State(new int[]{70,28,9,-482,10,-482});
    states[622] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-158,623,-134,609,-146,608,-150,49,-151,52});
    states[623] = new State(new int[]{5,624,100,606});
    states[624] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,625,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[625] = new State(-479);
    states[626] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-251,627,-8,1152,-9,631,-179,632,-146,1147,-150,49,-151,52,-300,1150});
    states[627] = new State(new int[]{12,628,100,629});
    states[628] = new State(-213);
    states[629] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-8,630,-9,631,-179,632,-146,1147,-150,49,-151,52,-300,1150});
    states[630] = new State(-215);
    states[631] = new State(-216);
    states[632] = new State(new int[]{7,182,8,635,123,187,12,-643,100,-643},new int[]{-69,633,-298,634});
    states[633] = new State(-781);
    states[634] = new State(-234);
    states[635] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,831,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680,9,-811},new int[]{-66,636,-70,638,-87,394,-86,27,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,832,-99,657,-323,675});
    states[636] = new State(new int[]{9,637});
    states[637] = new State(-644);
    states[638] = new State(new int[]{100,153,12,-810,9,-810});
    states[639] = new State(new int[]{127,640,11,-791,17,-791,8,-791,7,-791,142,-791,4,-791,15,-791,138,-791,136,-791,118,-791,117,-791,131,-791,132,-791,133,-791,134,-791,130,-791,116,-791,115,-791,128,-791,129,-791,126,-791,6,-791,5,-791,120,-791,125,-791,123,-791,121,-791,124,-791,122,-791,137,-791,135,-791,16,-791,70,-791,92,-791,10,-791,98,-791,101,-791,33,-791,104,-791,2,-791,9,-791,12,-791,100,-791,99,-791,32,-791,85,-791,84,-791,83,-791,82,-791,81,-791,86,-791,13,-791,119,-791});
    states[640] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,641,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[641] = new State(-987);
    states[642] = new State(-1024);
    states[643] = new State(new int[]{16,31,92,-617,10,-617,98,-617,101,-617,33,-617,104,-617,2,-617,9,-617,12,-617,100,-617,99,-617,32,-617,85,-617,84,-617,83,-617,82,-617,81,-617,86,-617,13,-611});
    states[644] = new State(new int[]{53,685,9,712,56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,1122,21,292,22,297,76,444,11,366,40,587,5,596,18,667},new int[]{-86,436,-359,438,-101,645,-146,1081,-4,1119,-100,1101,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,1121,-131,389,-110,397,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-358,1102,-99,1103});
    states[645] = new State(new int[]{100,646});
    states[646] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,18,667},new int[]{-78,647,-101,1146,-100,1145,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-99,1103});
    states[647] = new State(new int[]{100,1143,5,544,10,-1007,9,-1007},new int[]{-324,648});
    states[648] = new State(new int[]{10,536,9,-995},new int[]{-331,649});
    states[649] = new State(new int[]{9,650});
    states[650] = new State(new int[]{5,659,7,-760,138,-760,136,-760,118,-760,117,-760,131,-760,132,-760,133,-760,134,-760,130,-760,116,-760,115,-760,128,-760,129,-760,126,-760,6,-760,120,-760,125,-760,123,-760,121,-760,124,-760,122,-760,137,-760,135,-760,16,-760,70,-760,92,-760,10,-760,98,-760,101,-760,33,-760,104,-760,2,-760,9,-760,12,-760,100,-760,99,-760,32,-760,85,-760,84,-760,83,-760,82,-760,81,-760,86,-760,13,-760,127,-1009},new int[]{-335,651,-325,652});
    states[651] = new State(-992);
    states[652] = new State(new int[]{127,653});
    states[653] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,654,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[654] = new State(-997);
    states[655] = new State(new int[]{92,-618,10,-618,98,-618,101,-618,33,-618,104,-618,2,-618,9,-618,12,-618,100,-618,99,-618,32,-618,85,-618,84,-618,83,-618,82,-618,81,-618,86,-618,13,-612});
    states[656] = new State(-619);
    states[657] = new State(new int[]{5,659,127,-1009},new int[]{-335,658,-325,652});
    states[658] = new State(-993);
    states[659] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,354,142,487,24,492,48,500,49,547,34,551,73,555},new int[]{-277,660,-272,661,-92,194,-105,312,-106,313,-179,662,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-256,663,-249,664,-281,665,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-300,666});
    states[660] = new State(-1010);
    states[661] = new State(-487);
    states[662] = new State(new int[]{7,182,123,187,8,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,6,-255,116,-255,115,-255,128,-255,129,-255,127,-255},new int[]{-298,634});
    states[663] = new State(-488);
    states[664] = new State(-489);
    states[665] = new State(-490);
    states[666] = new State(-491);
    states[667] = new State(new int[]{18,667,143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-23,668,-22,674,-99,672,-146,673,-150,49,-151,52});
    states[668] = new State(new int[]{100,669});
    states[669] = new State(new int[]{18,667,143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-22,670,-99,672,-146,673,-150,49,-151,52});
    states[670] = new State(new int[]{9,671,100,-982});
    states[671] = new State(-978);
    states[672] = new State(-979);
    states[673] = new State(-980);
    states[674] = new State(-981);
    states[675] = new State(-994);
    states[676] = new State(new int[]{8,1133,5,659,127,-1009},new int[]{-325,677});
    states[677] = new State(new int[]{127,678});
    states[678] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,679,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[679] = new State(-998);
    states[680] = new State(new int[]{127,681,8,1125});
    states[681] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,684,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-329,682,-211,683,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-4,1123,-330,1124,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[682] = new State(-1001);
    states[683] = new State(-1026);
    states[684] = new State(new int[]{53,685,56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,1122,21,292,22,297,76,444,11,366,40,587,5,596,18,667},new int[]{-86,436,-359,438,-101,530,-110,692,-4,1119,-100,1101,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,1121,-131,389,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-358,1102,-99,1103});
    states[685] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,686,-150,49,-151,52});
    states[686] = new State(new int[]{110,687});
    states[687] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-98,688,-86,690,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-100,155,-240,577,-116,578,-242,586,-119,595});
    states[688] = new State(new int[]{9,689,16,31,10,-608,70,-608,13,-611});
    states[689] = new State(-778);
    states[690] = new State(new int[]{10,691,70,28});
    states[691] = new State(-787);
    states[692] = new State(new int[]{100,693,11,391,17,398,8,700,7,1106,142,1108,4,1109,15,1116,138,-776,136,-776,118,-776,117,-776,131,-776,132,-776,133,-776,134,-776,130,-776,116,-776,115,-776,128,-776,129,-776,126,-776,6,-776,5,-776,120,-776,125,-776,123,-776,121,-776,124,-776,122,-776,137,-776,135,-776,16,-776,9,-776,70,-776,13,-776,119,-776,110,-776,111,-776,112,-776,113,-776,114,-776});
    states[693] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,1111,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366},new int[]{-336,694,-110,1115,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726});
    states[694] = new State(new int[]{9,695,100,698});
    states[695] = new State(new int[]{110,427,111,428,112,429,113,430,114,431},new int[]{-193,696});
    states[696] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,697,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[697] = new State(new int[]{70,28,92,-523,10,-523,98,-523,101,-523,33,-523,104,-523,2,-523,9,-523,12,-523,100,-523,99,-523,32,-523,85,-523,84,-523,83,-523,82,-523,81,-523,86,-523});
    states[698] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,1111,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366},new int[]{-110,699,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726});
    states[699] = new State(new int[]{11,391,17,398,8,700,7,1106,142,1108,4,1109,9,-525,100,-525});
    states[700] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,710,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680,9,-813},new int[]{-67,701,-71,703,-88,1105,-86,706,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,707,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,1104,-99,657,-323,675});
    states[701] = new State(new int[]{9,702});
    states[702] = new State(-790);
    states[703] = new State(new int[]{100,704,9,-812});
    states[704] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,710,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680},new int[]{-88,705,-86,706,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,707,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,1104,-99,657,-323,675});
    states[705] = new State(-595);
    states[706] = new State(new int[]{70,28,100,-601,9,-601});
    states[707] = new State(new int[]{110,708,127,640,11,-791,17,-791,8,-791,7,-791,142,-791,4,-791,15,-791,138,-791,136,-791,118,-791,117,-791,131,-791,132,-791,133,-791,134,-791,130,-791,116,-791,115,-791,128,-791,129,-791,126,-791,6,-791,5,-791,120,-791,125,-791,123,-791,121,-791,124,-791,122,-791,137,-791,135,-791,16,-791,70,-791,100,-791,9,-791,13,-791,119,-791});
    states[708] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,709,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[709] = new State(-602);
    states[710] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,100,-604,9,-604},new int[]{-146,434,-150,49,-151,52});
    states[711] = new State(new int[]{53,685,9,712,56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596,18,667},new int[]{-86,436,-359,438,-101,645,-146,1081,-100,1101,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-358,1102,-99,1103});
    states[712] = new State(new int[]{5,659,127,-1009},new int[]{-325,713});
    states[713] = new State(new int[]{127,714});
    states[714] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,715,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[715] = new State(-988);
    states[716] = new State(-1025);
    states[717] = new State(-1011);
    states[718] = new State(-1012);
    states[719] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,720,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[720] = new State(new int[]{51,721});
    states[721] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,12,-494,100,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,722,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[722] = new State(new int[]{32,723,92,-533,10,-533,98,-533,101,-533,33,-533,104,-533,2,-533,9,-533,12,-533,100,-533,99,-533,85,-533,84,-533,83,-533,82,-533,81,-533,86,-533});
    states[723] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,12,-494,100,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,724,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[724] = new State(-534);
    states[725] = new State(new int[]{7,45,11,150,17,378});
    states[726] = new State(new int[]{7,451});
    states[727] = new State(-496);
    states[728] = new State(-497);
    states[729] = new State(new int[]{155,731,143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-142,730,-146,732,-150,49,-151,52});
    states[730] = new State(-529);
    states[731] = new State(-99);
    states[732] = new State(-100);
    states[733] = new State(-498);
    states[734] = new State(-499);
    states[735] = new State(-500);
    states[736] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,737,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[737] = new State(new int[]{58,738});
    states[738] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337,32,746,92,-553},new int[]{-35,739,-253,1078,-262,1080,-73,1071,-109,1077,-93,1076,-89,205,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[739] = new State(new int[]{10,742,32,746,92,-553},new int[]{-253,740});
    states[740] = new State(new int[]{92,741});
    states[741] = new State(-544);
    states[742] = new State(new int[]{32,746,143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337,92,-553},new int[]{-253,743,-262,745,-73,1071,-109,1077,-93,1076,-89,205,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[743] = new State(new int[]{92,744});
    states[744] = new State(-545);
    states[745] = new State(-548);
    states[746] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494,92,-494},new int[]{-252,747,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[747] = new State(new int[]{10,20,92,-554});
    states[748] = new State(-531);
    states[749] = new State(new int[]{11,-791,17,-791,8,-791,7,-791,142,-791,4,-791,15,-791,110,-791,111,-791,112,-791,113,-791,114,-791,92,-791,10,-791,98,-791,101,-791,33,-791,104,-791,2,-791,5,-100});
    states[750] = new State(new int[]{7,-190,11,-190,17,-190,5,-99});
    states[751] = new State(-501);
    states[752] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,98,-494,10,-494},new int[]{-252,753,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[753] = new State(new int[]{98,754,10,20});
    states[754] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,755,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[755] = new State(new int[]{70,28,92,-555,10,-555,98,-555,101,-555,33,-555,104,-555,2,-555,9,-555,12,-555,100,-555,99,-555,32,-555,85,-555,84,-555,83,-555,82,-555,81,-555,86,-555});
    states[756] = new State(-502);
    states[757] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,758,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[758] = new State(new int[]{99,1067,141,-558,143,-558,85,-558,86,-558,80,-558,78,-558,160,-558,87,-558,45,-558,42,-558,8,-558,21,-558,22,-558,144,-558,147,-558,145,-558,146,-558,155,-558,158,-558,157,-558,156,-558,76,-558,11,-558,57,-558,91,-558,40,-558,25,-558,97,-558,54,-558,35,-558,55,-558,102,-558,47,-558,36,-558,53,-558,60,-558,74,-558,72,-558,38,-558,92,-558,10,-558,98,-558,101,-558,33,-558,104,-558,2,-558,9,-558,12,-558,100,-558,32,-558,84,-558,83,-558,82,-558,81,-558},new int[]{-291,759});
    states[759] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,12,-494,100,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,760,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[760] = new State(-556);
    states[761] = new State(-503);
    states[762] = new State(new int[]{53,1070,143,-565,85,-565,86,-565,80,-565,78,-565,160,-565,87,-565},new int[]{-18,763});
    states[763] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,764,-150,49,-151,52});
    states[764] = new State(new int[]{5,1020,110,-563},new int[]{-274,765});
    states[765] = new State(new int[]{110,766});
    states[766] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,767,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[767] = new State(new int[]{70,1068,71,1069},new int[]{-118,768});
    states[768] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,769,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[769] = new State(new int[]{160,1063,99,1067,141,-558,143,-558,85,-558,86,-558,80,-558,78,-558,87,-558,45,-558,42,-558,8,-558,21,-558,22,-558,144,-558,147,-558,145,-558,146,-558,155,-558,158,-558,157,-558,156,-558,76,-558,11,-558,57,-558,91,-558,40,-558,25,-558,97,-558,54,-558,35,-558,55,-558,102,-558,47,-558,36,-558,53,-558,60,-558,74,-558,72,-558,38,-558,92,-558,10,-558,98,-558,101,-558,33,-558,104,-558,2,-558,9,-558,12,-558,100,-558,32,-558,84,-558,83,-558,82,-558,81,-558},new int[]{-291,770});
    states[770] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,12,-494,100,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,771,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[771] = new State(-571);
    states[772] = new State(-504);
    states[773] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,831,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680},new int[]{-70,774,-87,394,-86,27,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,832,-99,657,-323,675});
    states[774] = new State(new int[]{99,775,100,153});
    states[775] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,12,-494,100,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,776,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[776] = new State(-573);
    states[777] = new State(-505);
    states[778] = new State(-506);
    states[779] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494,101,-494,33,-494},new int[]{-252,780,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[780] = new State(new int[]{10,20,101,782,33,1041},new int[]{-289,781});
    states[781] = new State(-575);
    states[782] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494},new int[]{-252,783,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[783] = new State(new int[]{92,784,10,20});
    states[784] = new State(-576);
    states[785] = new State(-507);
    states[786] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596,92,-590,10,-590,98,-590,101,-590,33,-590,104,-590,2,-590,9,-590,12,-590,100,-590,99,-590,32,-590,84,-590,83,-590,82,-590,81,-590},new int[]{-86,787,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[787] = new State(new int[]{70,28,92,-591,10,-591,98,-591,101,-591,33,-591,104,-591,2,-591,9,-591,12,-591,100,-591,99,-591,32,-591,85,-591,84,-591,83,-591,82,-591,81,-591,86,-591});
    states[788] = new State(-508);
    states[789] = new State(new int[]{53,1022,143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,790,-150,49,-151,52});
    states[790] = new State(new int[]{5,1020,137,-563},new int[]{-274,791});
    states[791] = new State(new int[]{137,792});
    states[792] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,793,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[793] = new State(new int[]{87,1018,99,-561},new int[]{-360,794});
    states[794] = new State(new int[]{99,795});
    states[795] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,12,-494,100,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,796,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[796] = new State(-568);
    states[797] = new State(-509);
    states[798] = new State(new int[]{8,800,143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-311,799,-157,808,-146,807,-150,49,-151,52});
    states[799] = new State(-519);
    states[800] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,801,-150,49,-151,52});
    states[801] = new State(new int[]{100,802});
    states[802] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-157,803,-146,807,-150,49,-151,52});
    states[803] = new State(new int[]{9,804,100,542});
    states[804] = new State(new int[]{110,805});
    states[805] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,806,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[806] = new State(new int[]{70,28,92,-521,10,-521,98,-521,101,-521,33,-521,104,-521,2,-521,9,-521,12,-521,100,-521,99,-521,32,-521,85,-521,84,-521,83,-521,82,-521,81,-521,86,-521});
    states[807] = new State(-343);
    states[808] = new State(new int[]{5,809,100,542,110,1016});
    states[809] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,810,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[810] = new State(new int[]{110,1014,120,1015,92,-407,10,-407,98,-407,101,-407,33,-407,104,-407,2,-407,9,-407,12,-407,100,-407,99,-407,32,-407,85,-407,84,-407,83,-407,82,-407,81,-407,86,-407},new int[]{-338,811});
    states[811] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,985,135,839,116,351,115,352,63,337,37,676,44,680,40,587},new int[]{-85,812,-84,813,-83,274,-89,275,-90,236,-80,814,-13,249,-10,259,-14,222,-146,854,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853,-94,1002,-243,1003,-323,1012,-242,1013});
    states[812] = new State(-409);
    states[813] = new State(-410);
    states[814] = new State(new int[]{6,815,116,245,115,246,128,247,129,248,120,-118,125,-118,123,-118,121,-118,124,-118,122,-118,137,-118,13,-118,16,-118,92,-118,10,-118,98,-118,101,-118,33,-118,104,-118,2,-118,9,-118,12,-118,100,-118,99,-118,32,-118,85,-118,84,-118,83,-118,82,-118,81,-118,86,-118},new int[]{-192,214});
    states[815] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-13,816,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847});
    states[816] = new State(new int[]{136,250,138,251,118,252,117,253,131,254,132,255,133,256,134,257,130,258,92,-411,10,-411,98,-411,101,-411,33,-411,104,-411,2,-411,9,-411,12,-411,100,-411,99,-411,32,-411,85,-411,84,-411,83,-411,82,-411,81,-411,86,-411},new int[]{-200,216,-194,219});
    states[817] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-68,818,-76,369,-91,447,-86,372,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[818] = new State(new int[]{76,819});
    states[819] = new State(-166);
    states[820] = new State(-157);
    states[821] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,833,135,839,116,351,115,352,63,337},new int[]{-10,822,-14,823,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,841,-172,843,-56,844});
    states[822] = new State(-158);
    states[823] = new State(new int[]{4,224,11,226,7,824,142,826,8,827,136,-155,138,-155,118,-155,117,-155,131,-155,132,-155,133,-155,134,-155,130,-155,116,-155,115,-155,128,-155,129,-155,120,-155,125,-155,123,-155,121,-155,124,-155,122,-155,137,-155,13,-155,16,-155,6,-155,100,-155,9,-155,12,-155,5,-155,92,-155,10,-155,98,-155,101,-155,33,-155,104,-155,2,-155,99,-155,32,-155,85,-155,84,-155,83,-155,82,-155,81,-155,86,-155},new int[]{-12,223});
    states[824] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-137,825,-146,47,-150,49,-151,52,-292,57,-149,58,-293,148});
    states[825] = new State(-178);
    states[826] = new State(-179);
    states[827] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,831,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680,9,-183},new int[]{-75,828,-70,830,-87,394,-86,27,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,832,-99,657,-323,675});
    states[828] = new State(new int[]{9,829});
    states[829] = new State(-180);
    states[830] = new State(new int[]{100,153,9,-182});
    states[831] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,92,-600,10,-600,98,-600,101,-600,33,-600,104,-600,2,-600,9,-600,12,-600,100,-600,99,-600,32,-600,84,-600,83,-600,82,-600,81,-600},new int[]{-146,434,-150,49,-151,52});
    states[832] = new State(-599);
    states[833] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-89,834,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[834] = new State(new int[]{9,835,13,206,16,210});
    states[835] = new State(-159);
    states[836] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-89,837,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[837] = new State(new int[]{9,838,13,206,16,210});
    states[838] = new State(new int[]{136,-159,138,-159,118,-159,117,-159,131,-159,132,-159,133,-159,134,-159,130,-159,116,-159,115,-159,128,-159,129,-159,120,-159,125,-159,123,-159,121,-159,124,-159,122,-159,137,-159,13,-159,16,-159,6,-159,100,-159,9,-159,12,-159,5,-159,92,-159,10,-159,98,-159,101,-159,33,-159,104,-159,2,-159,99,-159,32,-159,85,-159,84,-159,83,-159,82,-159,81,-159,86,-159,119,-154});
    states[839] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,833,135,839,116,351,115,352,63,337},new int[]{-10,840,-14,823,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,841,-172,843,-56,844});
    states[840] = new State(-160);
    states[841] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,833,135,839,116,351,115,352,63,337},new int[]{-10,842,-14,823,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,841,-172,843,-56,844});
    states[842] = new State(-161);
    states[843] = new State(-162);
    states[844] = new State(-163);
    states[845] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-10,842,-269,846,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-11,847});
    states[846] = new State(-140);
    states[847] = new State(new int[]{119,848});
    states[848] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-10,849,-269,850,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-11,847});
    states[849] = new State(-138);
    states[850] = new State(-139);
    states[851] = new State(-142);
    states[852] = new State(-143);
    states[853] = new State(-121);
    states[854] = new State(new int[]{127,855,4,-169,11,-169,7,-169,142,-169,8,-169,136,-169,138,-169,118,-169,117,-169,131,-169,132,-169,133,-169,134,-169,130,-169,6,-169,116,-169,115,-169,128,-169,129,-169,120,-169,125,-169,123,-169,121,-169,124,-169,122,-169,137,-169,13,-169,16,-169,92,-169,10,-169,98,-169,101,-169,33,-169,104,-169,2,-169,9,-169,12,-169,100,-169,99,-169,32,-169,85,-169,84,-169,83,-169,82,-169,81,-169,86,-169,119,-169});
    states[855] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,856,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[856] = new State(-413);
    states[857] = new State(-1013);
    states[858] = new State(-1014);
    states[859] = new State(-1015);
    states[860] = new State(-1016);
    states[861] = new State(-1017);
    states[862] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,863,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[863] = new State(new int[]{99,864});
    states[864] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,12,-494,100,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,865,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[865] = new State(-516);
    states[866] = new State(-510);
    states[867] = new State(-596);
    states[868] = new State(-597);
    states[869] = new State(-511);
    states[870] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,871,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[871] = new State(new int[]{99,872});
    states[872] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,12,-494,100,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,873,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[873] = new State(-559);
    states[874] = new State(-512);
    states[875] = new State(new int[]{73,877,56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,711,21,292,22,297,76,444,11,366,40,587,18,667,37,676,44,680},new int[]{-102,876,-100,879,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-322,880,-99,657,-323,675});
    states[876] = new State(-517);
    states[877] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,711,21,292,22,297,76,444,11,366,40,587,18,667,37,676,44,680},new int[]{-102,878,-100,879,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-322,880,-99,657,-323,675});
    states[878] = new State(-518);
    states[879] = new State(-615);
    states[880] = new State(-616);
    states[881] = new State(-513);
    states[882] = new State(-514);
    states[883] = new State(-515);
    states[884] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,885,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[885] = new State(new int[]{55,886});
    states[886] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,56,964,21,292,22,297,11,924,8,937},new int[]{-350,887,-349,978,-342,894,-283,899,-179,181,-146,218,-150,49,-151,52,-341,956,-357,959,-339,967,-15,962,-164,334,-166,277,-165,281,-16,335,-257,965,-294,966,-343,968,-344,971});
    states[887] = new State(new int[]{10,890,32,746,92,-553},new int[]{-253,888});
    states[888] = new State(new int[]{92,889});
    states[889] = new State(-535);
    states[890] = new State(new int[]{32,746,143,48,85,50,86,51,80,53,78,54,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,56,964,21,292,22,297,11,924,8,937,92,-553},new int[]{-253,891,-349,893,-342,894,-283,899,-179,181,-146,218,-150,49,-151,52,-341,956,-357,959,-339,967,-15,962,-164,334,-166,277,-165,281,-16,335,-257,965,-294,966,-343,968,-344,971});
    states[891] = new State(new int[]{92,892});
    states[892] = new State(-536);
    states[893] = new State(-538);
    states[894] = new State(new int[]{39,895});
    states[895] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,896,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[896] = new State(new int[]{5,897});
    states[897] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494,32,-494,92,-494},new int[]{-260,898,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[898] = new State(-539);
    states[899] = new State(new int[]{8,900,100,-656,5,-656});
    states[900] = new State(new int[]{14,905,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,116,351,115,352,143,48,85,50,86,51,80,53,78,54,160,55,87,56,53,912,11,924,8,937},new int[]{-354,901,-352,955,-15,906,-164,334,-166,277,-165,281,-16,335,-198,907,-146,909,-150,49,-151,52,-342,916,-283,917,-179,181,-343,923,-344,954});
    states[901] = new State(new int[]{9,902,10,903,100,921});
    states[902] = new State(new int[]{39,-650,5,-651});
    states[903] = new State(new int[]{14,905,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,116,351,115,352,143,48,85,50,86,51,80,53,78,54,160,55,87,56,53,912,11,924,8,937},new int[]{-352,904,-15,906,-164,334,-166,277,-165,281,-16,335,-198,907,-146,909,-150,49,-151,52,-342,916,-283,917,-179,181,-343,923,-344,954});
    states[904] = new State(-682);
    states[905] = new State(-694);
    states[906] = new State(-695);
    states[907] = new State(new int[]{144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288},new int[]{-15,908,-164,334,-166,277,-165,281,-16,335});
    states[908] = new State(-696);
    states[909] = new State(new int[]{5,910,9,-698,10,-698,100,-698,7,-260,4,-260,123,-260,8,-260});
    states[910] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,911,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[911] = new State(-697);
    states[912] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,913,-150,49,-151,52});
    states[913] = new State(new int[]{5,914,9,-700,10,-700,100,-700});
    states[914] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,915,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[915] = new State(-699);
    states[916] = new State(-701);
    states[917] = new State(new int[]{8,918});
    states[918] = new State(new int[]{14,905,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,116,351,115,352,143,48,85,50,86,51,80,53,78,54,160,55,87,56,53,912,11,924,8,937},new int[]{-354,919,-352,955,-15,906,-164,334,-166,277,-165,281,-16,335,-198,907,-146,909,-150,49,-151,52,-342,916,-283,917,-179,181,-343,923,-344,954});
    states[919] = new State(new int[]{9,920,10,903,100,921});
    states[920] = new State(-650);
    states[921] = new State(new int[]{14,905,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,116,351,115,352,143,48,85,50,86,51,80,53,78,54,160,55,87,56,53,912,11,924,8,937},new int[]{-352,922,-15,906,-164,334,-166,277,-165,281,-16,335,-198,907,-146,909,-150,49,-151,52,-342,916,-283,917,-179,181,-343,923,-344,954});
    states[922] = new State(-683);
    states[923] = new State(-702);
    states[924] = new State(new int[]{144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,53,931,14,933,143,48,85,50,86,51,80,53,78,54,160,55,87,56,11,924,8,937,6,952},new int[]{-355,925,-345,953,-15,929,-164,334,-166,277,-165,281,-16,335,-347,930,-342,934,-283,917,-179,181,-146,218,-150,49,-151,52,-343,935,-344,936});
    states[925] = new State(new int[]{12,926,100,927});
    states[926] = new State(-660);
    states[927] = new State(new int[]{144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,53,931,14,933,143,48,85,50,86,51,80,53,78,54,160,55,87,56,11,924,8,937,6,952},new int[]{-345,928,-15,929,-164,334,-166,277,-165,281,-16,335,-347,930,-342,934,-283,917,-179,181,-146,218,-150,49,-151,52,-343,935,-344,936});
    states[928] = new State(-662);
    states[929] = new State(-663);
    states[930] = new State(-664);
    states[931] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,932,-150,49,-151,52});
    states[932] = new State(-670);
    states[933] = new State(-665);
    states[934] = new State(-666);
    states[935] = new State(-667);
    states[936] = new State(-668);
    states[937] = new State(new int[]{14,942,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,116,351,115,352,53,946,143,48,85,50,86,51,80,53,78,54,160,55,87,56,11,924,8,937},new int[]{-356,938,-346,951,-15,943,-164,334,-166,277,-165,281,-16,335,-198,944,-342,948,-283,917,-179,181,-146,218,-150,49,-151,52,-343,949,-344,950});
    states[938] = new State(new int[]{9,939,100,940});
    states[939] = new State(-671);
    states[940] = new State(new int[]{14,942,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,116,351,115,352,53,946,143,48,85,50,86,51,80,53,78,54,160,55,87,56,11,924,8,937},new int[]{-346,941,-15,943,-164,334,-166,277,-165,281,-16,335,-198,944,-342,948,-283,917,-179,181,-146,218,-150,49,-151,52,-343,949,-344,950});
    states[941] = new State(-680);
    states[942] = new State(-672);
    states[943] = new State(-673);
    states[944] = new State(new int[]{144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288},new int[]{-15,945,-164,334,-166,277,-165,281,-16,335});
    states[945] = new State(-674);
    states[946] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,947,-150,49,-151,52});
    states[947] = new State(-675);
    states[948] = new State(-676);
    states[949] = new State(-677);
    states[950] = new State(-678);
    states[951] = new State(-679);
    states[952] = new State(-669);
    states[953] = new State(-661);
    states[954] = new State(-703);
    states[955] = new State(-681);
    states[956] = new State(new int[]{5,957});
    states[957] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494,32,-494,92,-494},new int[]{-260,958,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[958] = new State(-540);
    states[959] = new State(new int[]{100,960,5,-652});
    states[960] = new State(new int[]{144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,143,48,85,50,86,51,80,53,78,54,160,55,87,56,56,964,21,292,22,297},new int[]{-339,961,-15,962,-164,334,-166,277,-165,281,-16,335,-283,963,-179,181,-146,218,-150,49,-151,52,-257,965,-294,966});
    states[961] = new State(-654);
    states[962] = new State(-655);
    states[963] = new State(-656);
    states[964] = new State(-657);
    states[965] = new State(-658);
    states[966] = new State(-659);
    states[967] = new State(-653);
    states[968] = new State(new int[]{5,969});
    states[969] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494,32,-494,92,-494},new int[]{-260,970,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[970] = new State(-541);
    states[971] = new State(new int[]{39,972,5,976});
    states[972] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,973,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[973] = new State(new int[]{5,974});
    states[974] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494,32,-494,92,-494},new int[]{-260,975,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[975] = new State(-542);
    states[976] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494,32,-494,92,-494},new int[]{-260,977,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[977] = new State(-543);
    states[978] = new State(-537);
    states[979] = new State(-1018);
    states[980] = new State(-1019);
    states[981] = new State(-1020);
    states[982] = new State(-1021);
    states[983] = new State(-1022);
    states[984] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,711,21,292,22,297,76,444,11,366,40,587,18,667,37,676,44,680},new int[]{-102,876,-100,879,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-322,880,-99,657,-323,675});
    states[985] = new State(new int[]{9,993,143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,998,135,839,116,351,115,352,63,337},new int[]{-89,986,-65,987,-245,991,-90,236,-80,244,-13,249,-10,259,-14,222,-146,997,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853,-64,271,-84,1001,-83,274,-94,1002,-243,1003,-244,1004,-246,1011,-135,1007});
    states[986] = new State(new int[]{9,838,13,206,16,210,100,-194});
    states[987] = new State(new int[]{9,988});
    states[988] = new State(new int[]{127,989,92,-197,10,-197,98,-197,101,-197,33,-197,104,-197,2,-197,9,-197,12,-197,100,-197,99,-197,32,-197,85,-197,84,-197,83,-197,82,-197,81,-197,86,-197});
    states[989] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,990,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[990] = new State(-415);
    states[991] = new State(new int[]{9,992});
    states[992] = new State(-202);
    states[993] = new State(new int[]{5,544,127,-1007},new int[]{-324,994});
    states[994] = new State(new int[]{127,995});
    states[995] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,996,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[996] = new State(-414);
    states[997] = new State(new int[]{4,-169,11,-169,7,-169,142,-169,8,-169,136,-169,138,-169,118,-169,117,-169,131,-169,132,-169,133,-169,134,-169,130,-169,116,-169,115,-169,128,-169,129,-169,120,-169,125,-169,123,-169,121,-169,124,-169,122,-169,137,-169,9,-169,13,-169,16,-169,100,-169,119,-169,5,-208});
    states[998] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,998,135,839,116,351,115,352,63,337,9,-198},new int[]{-89,986,-65,999,-245,991,-90,236,-80,244,-13,249,-10,259,-14,222,-146,997,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853,-64,271,-84,1001,-83,274,-94,1002,-243,1003,-244,1004,-246,1011,-135,1007});
    states[999] = new State(new int[]{9,1000});
    states[1000] = new State(-197);
    states[1001] = new State(-200);
    states[1002] = new State(-195);
    states[1003] = new State(-196);
    states[1004] = new State(new int[]{10,1005,9,-203});
    states[1005] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,9,-204},new int[]{-246,1006,-135,1007,-146,1010,-150,49,-151,52});
    states[1006] = new State(-206);
    states[1007] = new State(new int[]{5,1008});
    states[1008] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,998,135,839,116,351,115,352,63,337},new int[]{-83,1009,-89,275,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853,-94,1002,-243,1003});
    states[1009] = new State(-207);
    states[1010] = new State(-208);
    states[1011] = new State(-205);
    states[1012] = new State(-412);
    states[1013] = new State(-416);
    states[1014] = new State(-405);
    states[1015] = new State(-406);
    states[1016] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,831,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680},new int[]{-87,1017,-86,27,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,832,-99,657,-323,675});
    states[1017] = new State(-408);
    states[1018] = new State(new int[]{143,1019});
    states[1019] = new State(-560);
    states[1020] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,1021,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1021] = new State(-562);
    states[1022] = new State(new int[]{8,1030,143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,1023,-150,49,-151,52});
    states[1023] = new State(new int[]{5,1020,137,-563},new int[]{-274,1024});
    states[1024] = new State(new int[]{137,1025});
    states[1025] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,1026,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[1026] = new State(new int[]{87,1018,99,-561},new int[]{-360,1027});
    states[1027] = new State(new int[]{99,1028});
    states[1028] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,12,-494,100,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,1029,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1029] = new State(-569);
    states[1030] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-157,1031,-146,807,-150,49,-151,52});
    states[1031] = new State(new int[]{9,1032,100,542});
    states[1032] = new State(new int[]{137,1033});
    states[1033] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,1034,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[1034] = new State(new int[]{87,1018,99,-561},new int[]{-360,1035});
    states[1035] = new State(new int[]{99,1036});
    states[1036] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,12,-494,100,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,1037,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1037] = new State(-570);
    states[1038] = new State(new int[]{5,1039});
    states[1039] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494},new int[]{-261,1040,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[1040] = new State(-493);
    states[1041] = new State(new int[]{79,1049,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494,92,-494},new int[]{-59,1042,-62,1044,-61,1061,-252,1062,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[1042] = new State(new int[]{92,1043});
    states[1043] = new State(-577);
    states[1044] = new State(new int[]{10,1046,32,1059,92,-583},new int[]{-254,1045});
    states[1045] = new State(-578);
    states[1046] = new State(new int[]{79,1049,32,1059,92,-583},new int[]{-61,1047,-254,1048});
    states[1047] = new State(-582);
    states[1048] = new State(-579);
    states[1049] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-63,1050,-178,1053,-179,1054,-146,1055,-150,49,-151,52,-139,1056});
    states[1050] = new State(new int[]{99,1051});
    states[1051] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494,32,-494,92,-494},new int[]{-260,1052,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1052] = new State(-585);
    states[1053] = new State(-586);
    states[1054] = new State(new int[]{7,182,99,-588});
    states[1055] = new State(new int[]{7,-260,99,-260,5,-589});
    states[1056] = new State(new int[]{5,1057});
    states[1057] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-178,1058,-179,1054,-146,218,-150,49,-151,52});
    states[1058] = new State(-587);
    states[1059] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494,92,-494},new int[]{-252,1060,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[1060] = new State(new int[]{10,20,92,-584});
    states[1061] = new State(-581);
    states[1062] = new State(new int[]{10,20,92,-580});
    states[1063] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,1064,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[1064] = new State(new int[]{99,1065});
    states[1065] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494,98,-494,101,-494,33,-494,104,-494,2,-494,9,-494,12,-494,100,-494,99,-494,32,-494,84,-494,83,-494,82,-494,81,-494},new int[]{-260,1066,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1066] = new State(-572);
    states[1067] = new State(-557);
    states[1068] = new State(-566);
    states[1069] = new State(-567);
    states[1070] = new State(-564);
    states[1071] = new State(new int[]{5,1072,100,1074});
    states[1072] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494,32,-494,92,-494},new int[]{-260,1073,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1073] = new State(-549);
    states[1074] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-109,1075,-93,1076,-89,205,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[1075] = new State(-551);
    states[1076] = new State(-552);
    states[1077] = new State(-550);
    states[1078] = new State(new int[]{92,1079});
    states[1079] = new State(-546);
    states[1080] = new State(-547);
    states[1081] = new State(new int[]{5,1082,10,1095,11,-791,17,-791,8,-791,7,-791,142,-791,4,-791,15,-791,110,-791,111,-791,112,-791,113,-791,114,-791,138,-791,136,-791,118,-791,117,-791,131,-791,132,-791,133,-791,134,-791,130,-791,116,-791,115,-791,128,-791,129,-791,126,-791,6,-791,120,-791,125,-791,123,-791,121,-791,124,-791,122,-791,137,-791,135,-791,16,-791,9,-791,70,-791,100,-791,13,-791,119,-791});
    states[1082] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,1083,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1083] = new State(new int[]{9,1084,10,1088});
    states[1084] = new State(new int[]{5,659,127,-1009},new int[]{-325,1085});
    states[1085] = new State(new int[]{127,1086});
    states[1086] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,1087,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[1087] = new State(-989);
    states[1088] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-326,1089,-327,1094,-157,540,-146,807,-150,49,-151,52});
    states[1089] = new State(new int[]{9,1090,10,538});
    states[1090] = new State(new int[]{5,659,127,-1009},new int[]{-325,1091});
    states[1091] = new State(new int[]{127,1092});
    states[1092] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,1093,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[1093] = new State(-991);
    states[1094] = new State(-1004);
    states[1095] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-326,1096,-327,1094,-157,540,-146,807,-150,49,-151,52});
    states[1096] = new State(new int[]{9,1097,10,538});
    states[1097] = new State(new int[]{5,659,127,-1009},new int[]{-325,1098});
    states[1098] = new State(new int[]{127,1099});
    states[1099] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,1100,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[1100] = new State(-990);
    states[1101] = new State(new int[]{9,-605,70,-605,100,-983});
    states[1102] = new State(-788);
    states[1103] = new State(-984);
    states[1104] = new State(-603);
    states[1105] = new State(-594);
    states[1106] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,403},new int[]{-147,1107,-146,453,-150,49,-151,52,-292,454,-149,58,-190,455});
    states[1107] = new State(-806);
    states[1108] = new State(-808);
    states[1109] = new State(new int[]{123,187},new int[]{-298,1110});
    states[1110] = new State(-809);
    states[1111] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596,53,1112,18,667},new int[]{-86,436,-359,438,-101,530,-100,1101,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-358,1102,-99,1103});
    states[1112] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,1113,-150,49,-151,52});
    states[1113] = new State(new int[]{110,1114});
    states[1114] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,690,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[1115] = new State(new int[]{11,391,17,398,8,700,7,1106,142,1108,4,1109,9,-524,100,-524});
    states[1116] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,1111,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366},new int[]{-110,1117,-115,1118,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726});
    states[1117] = new State(new int[]{11,391,17,398,8,700,7,1106,142,1108,4,1109,15,1116,110,-773,111,-773,112,-773,113,-773,114,-773,92,-773,10,-773,98,-773,101,-773,33,-773,104,-773,2,-773,138,-773,136,-773,118,-773,117,-773,131,-773,132,-773,133,-773,134,-773,130,-773,116,-773,115,-773,128,-773,129,-773,126,-773,6,-773,5,-773,120,-773,125,-773,123,-773,121,-773,124,-773,122,-773,137,-773,135,-773,16,-773,70,-773,9,-773,12,-773,100,-773,99,-773,32,-773,85,-773,84,-773,83,-773,82,-773,81,-773,86,-773,13,-773,119,-773,76,-773,51,-773,58,-773,141,-773,143,-773,80,-773,78,-773,160,-773,87,-773,45,-773,42,-773,21,-773,22,-773,144,-773,147,-773,145,-773,146,-773,155,-773,158,-773,157,-773,156,-773,57,-773,91,-773,40,-773,25,-773,97,-773,54,-773,35,-773,55,-773,102,-773,47,-773,36,-773,53,-773,60,-773,74,-773,72,-773,38,-773,71,-773});
    states[1118] = new State(-774);
    states[1119] = new State(new int[]{9,1120});
    states[1120] = new State(-1023);
    states[1121] = new State(new int[]{110,427,111,428,112,429,113,430,114,431,138,-769,136,-769,118,-769,117,-769,131,-769,132,-769,133,-769,134,-769,130,-769,116,-769,115,-769,128,-769,129,-769,126,-769,6,-769,5,-769,120,-769,125,-769,123,-769,121,-769,124,-769,122,-769,137,-769,135,-769,16,-769,9,-769,70,-769,100,-769,13,-769,2,-769,119,-762},new int[]{-193,25});
    states[1122] = new State(new int[]{53,685,56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596,18,667},new int[]{-86,436,-359,438,-101,530,-110,692,-100,1101,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-358,1102,-99,1103});
    states[1123] = new State(-1027);
    states[1124] = new State(-1028);
    states[1125] = new State(new int[]{9,1126,143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-326,1129,-327,1094,-157,540,-146,807,-150,49,-151,52});
    states[1126] = new State(new int[]{127,1127});
    states[1127] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,684,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-329,1128,-211,683,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-4,1123,-330,1124,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[1128] = new State(-1002);
    states[1129] = new State(new int[]{9,1130,10,538});
    states[1130] = new State(new int[]{127,1131});
    states[1131] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,42,433,8,684,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-329,1132,-211,683,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-4,1123,-330,1124,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[1132] = new State(-1003);
    states[1133] = new State(new int[]{9,1134,143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-326,1138,-327,1094,-157,540,-146,807,-150,49,-151,52});
    states[1134] = new State(new int[]{5,659,127,-1009},new int[]{-325,1135});
    states[1135] = new State(new int[]{127,1136});
    states[1136] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,1137,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[1137] = new State(-999);
    states[1138] = new State(new int[]{9,1139,10,538});
    states[1139] = new State(new int[]{5,659,127,-1009},new int[]{-325,1140});
    states[1140] = new State(new int[]{127,1141});
    states[1141] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,644,21,292,22,297,76,444,11,366,18,667,37,676,44,680,91,17,40,719,54,757,97,752,35,762,36,789,72,862,25,736,102,779,60,870,47,786,74,984},new int[]{-328,1142,-103,642,-98,643,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,655,-116,578,-322,656,-99,657,-323,675,-330,716,-255,717,-152,718,-318,857,-247,858,-123,859,-122,860,-124,861,-34,979,-301,980,-168,981,-248,982,-125,983});
    states[1142] = new State(-1000);
    states[1143] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,18,667},new int[]{-101,1144,-100,1145,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-99,1103});
    states[1144] = new State(-986);
    states[1145] = new State(-983);
    states[1146] = new State(-985);
    states[1147] = new State(new int[]{5,1148,7,-260,8,-260,123,-260,12,-260,100,-260});
    states[1148] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-9,1149,-179,632,-146,218,-150,49,-151,52,-300,1150});
    states[1149] = new State(-217);
    states[1150] = new State(new int[]{8,635,12,-643,100,-643},new int[]{-69,1151});
    states[1151] = new State(-782);
    states[1152] = new State(-214);
    states[1153] = new State(-210);
    states[1154] = new State(-473);
    states[1155] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,1156,-150,49,-151,52});
    states[1156] = new State(new int[]{110,687,100,1157});
    states[1157] = new State(new int[]{53,1165},new int[]{-337,1158});
    states[1158] = new State(new int[]{9,1159,100,1162});
    states[1159] = new State(new int[]{110,1160});
    states[1160] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,1161,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[1161] = new State(new int[]{70,28,92,-520,10,-520,98,-520,101,-520,33,-520,104,-520,2,-520,9,-520,12,-520,100,-520,99,-520,32,-520,85,-520,84,-520,83,-520,82,-520,81,-520,86,-520});
    states[1162] = new State(new int[]{53,1163});
    states[1163] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,1164,-150,49,-151,52});
    states[1164] = new State(-527);
    states[1165] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,1166,-150,49,-151,52});
    states[1166] = new State(-526);
    states[1167] = new State(new int[]{148,1171,150,1172,151,1173,152,1174,154,1175,153,1176,107,-828,91,-828,59,-828,29,-828,66,-828,50,-828,53,-828,62,-828,11,-828,28,-828,26,-828,44,-828,37,-828,19,-828,30,-828,31,-828,46,-828,27,-828,92,-828,84,-828,83,-828,82,-828,81,-828,23,-828,149,-828,41,-828},new int[]{-205,1168,-208,1177});
    states[1168] = new State(new int[]{10,1169});
    states[1169] = new State(new int[]{148,1171,150,1172,151,1173,152,1174,154,1175,153,1176,107,-829,91,-829,59,-829,29,-829,66,-829,50,-829,53,-829,62,-829,11,-829,28,-829,26,-829,44,-829,37,-829,19,-829,30,-829,31,-829,46,-829,27,-829,92,-829,84,-829,83,-829,82,-829,81,-829,23,-829,149,-829,41,-829},new int[]{-208,1170});
    states[1170] = new State(-833);
    states[1171] = new State(-845);
    states[1172] = new State(-846);
    states[1173] = new State(-847);
    states[1174] = new State(-848);
    states[1175] = new State(-849);
    states[1176] = new State(-850);
    states[1177] = new State(-832);
    states[1178] = new State(-376);
    states[1179] = new State(-447);
    states[1180] = new State(-448);
    states[1181] = new State(new int[]{8,-453,110,-453,10,-453,11,-453,5,-453,7,-450});
    states[1182] = new State(new int[]{123,1184,8,-456,110,-456,10,-456,7,-456,11,-456,5,-456},new int[]{-154,1183});
    states[1183] = new State(-457);
    states[1184] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-157,1185,-146,807,-150,49,-151,52});
    states[1185] = new State(new int[]{121,1186,100,542});
    states[1186] = new State(-321);
    states[1187] = new State(-458);
    states[1188] = new State(new int[]{123,1184,8,-454,110,-454,10,-454,11,-454,5,-454},new int[]{-154,1189});
    states[1189] = new State(-455);
    states[1190] = new State(new int[]{7,1191});
    states[1191] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403},new int[]{-141,1192,-148,1193,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188});
    states[1192] = new State(-449);
    states[1193] = new State(-452);
    states[1194] = new State(-451);
    states[1195] = new State(-438);
    states[1196] = new State(new int[]{44,1326,37,1361},new int[]{-215,1197,-227,1198,-224,1325,-228,1360});
    states[1197] = new State(-440);
    states[1198] = new State(new int[]{107,1316,59,-70,29,-70,66,-70,50,-70,53,-70,62,-70,91,-70},new int[]{-175,1199,-42,1200,-38,1203,-60,1315});
    states[1199] = new State(-441);
    states[1200] = new State(new int[]{91,17},new int[]{-255,1201});
    states[1201] = new State(new int[]{10,1202});
    states[1202] = new State(-468);
    states[1203] = new State(new int[]{59,1206,29,1227,66,1231,50,1443,53,1458,62,1460,91,-69},new int[]{-45,1204,-167,1205,-28,1212,-51,1229,-288,1233,-309,1445});
    states[1204] = new State(-71);
    states[1205] = new State(-87);
    states[1206] = new State(new int[]{155,731,143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-155,1207,-142,1211,-146,732,-150,49,-151,52});
    states[1207] = new State(new int[]{10,1208,100,1209});
    states[1208] = new State(-96);
    states[1209] = new State(new int[]{155,731,143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-142,1210,-146,732,-150,49,-151,52});
    states[1210] = new State(-98);
    states[1211] = new State(-97);
    states[1212] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-88,29,-88,66,-88,50,-88,53,-88,62,-88,91,-88},new int[]{-26,1213,-27,1214,-140,1216,-146,1226,-150,49,-151,52});
    states[1213] = new State(-102);
    states[1214] = new State(new int[]{10,1215});
    states[1215] = new State(-112);
    states[1216] = new State(new int[]{120,1217,5,1222});
    states[1217] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,1220,135,839,116,351,115,352,63,337},new int[]{-108,1218,-89,1219,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853,-94,1221});
    states[1218] = new State(-113);
    states[1219] = new State(new int[]{13,206,16,210,10,-115,92,-115,84,-115,83,-115,82,-115,81,-115});
    states[1220] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,998,135,839,116,351,115,352,63,337,9,-198},new int[]{-89,986,-65,999,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853,-64,271,-84,1001,-83,274,-94,1002,-243,1003});
    states[1221] = new State(-116);
    states[1222] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,1223,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1223] = new State(new int[]{120,1224});
    states[1224] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,998,135,839,116,351,115,352,63,337},new int[]{-83,1225,-89,275,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853,-94,1002,-243,1003});
    states[1225] = new State(-114);
    states[1226] = new State(-117);
    states[1227] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-26,1228,-27,1214,-140,1216,-146,1226,-150,49,-151,52});
    states[1228] = new State(-101);
    states[1229] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-89,29,-89,66,-89,50,-89,53,-89,62,-89,91,-89},new int[]{-26,1230,-27,1214,-140,1216,-146,1226,-150,49,-151,52});
    states[1230] = new State(-104);
    states[1231] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-26,1232,-27,1214,-140,1216,-146,1226,-150,49,-151,52});
    states[1232] = new State(-103);
    states[1233] = new State(new int[]{11,626,59,-90,29,-90,66,-90,50,-90,53,-90,62,-90,91,-90,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-48,1234,-6,1235,-250,1153});
    states[1234] = new State(-106);
    states[1235] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,11,626},new int[]{-49,1236,-250,516,-143,1237,-146,1435,-150,49,-151,52,-144,1440});
    states[1236] = new State(-209);
    states[1237] = new State(new int[]{120,1238});
    states[1238] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602,68,1429,69,1430,148,1431,27,1432,28,1433,26,-303,43,-303,64,-303},new int[]{-286,1239,-276,1241,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560,-29,1242,-20,1243,-21,1427,-19,1434});
    states[1239] = new State(new int[]{10,1240});
    states[1240] = new State(-218);
    states[1241] = new State(-223);
    states[1242] = new State(-224);
    states[1243] = new State(new int[]{26,1421,43,1422,64,1423},new int[]{-290,1244});
    states[1244] = new State(new int[]{8,1342,23,-315,11,-315,92,-315,84,-315,83,-315,82,-315,81,-315,29,-315,143,-315,85,-315,86,-315,80,-315,78,-315,160,-315,87,-315,62,-315,28,-315,26,-315,44,-315,37,-315,19,-315,30,-315,31,-315,46,-315,27,-315,10,-315},new int[]{-182,1245});
    states[1245] = new State(new int[]{23,1333,11,-322,92,-322,84,-322,83,-322,82,-322,81,-322,29,-322,143,-322,85,-322,86,-322,80,-322,78,-322,160,-322,87,-322,62,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,10,-322},new int[]{-317,1246,-316,1331,-315,1353});
    states[1246] = new State(new int[]{11,626,10,-313,92,-339,84,-339,83,-339,82,-339,81,-339,29,-212,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212,62,-212,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-25,1247,-24,1248,-31,1254,-33,507,-44,1255,-6,1256,-250,1153,-32,1418,-53,1420,-52,513,-54,1419});
    states[1247] = new State(-296);
    states[1248] = new State(new int[]{92,1249,84,1250,83,1251,82,1252,81,1253},new int[]{-7,505});
    states[1249] = new State(-314);
    states[1250] = new State(-335);
    states[1251] = new State(-336);
    states[1252] = new State(-337);
    states[1253] = new State(-338);
    states[1254] = new State(-333);
    states[1255] = new State(-347);
    states[1256] = new State(new int[]{29,1258,143,48,85,50,86,51,80,53,78,54,160,55,87,56,62,1262,28,1378,26,1379,11,626,44,1326,37,1361,19,1381,30,1389,31,1396,46,1403,27,1412},new int[]{-50,1257,-250,516,-221,515,-218,517,-258,518,-312,1260,-311,1261,-157,808,-146,807,-150,49,-151,52,-3,1266,-229,1380,-227,1314,-224,1325,-228,1360,-226,1387,-214,1400,-215,1401,-217,1402});
    states[1257] = new State(-349);
    states[1258] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-27,1259,-140,1216,-146,1226,-150,49,-151,52});
    states[1259] = new State(-354);
    states[1260] = new State(-355);
    states[1261] = new State(-359);
    states[1262] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-157,1263,-146,807,-150,49,-151,52});
    states[1263] = new State(new int[]{5,1264,100,542});
    states[1264] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,1265,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1265] = new State(-360);
    states[1266] = new State(new int[]{30,521,19,1196,46,1268,27,1306,143,48,85,50,86,51,80,53,78,54,160,55,87,56,62,1262,44,1326,37,1361},new int[]{-312,1267,-229,520,-215,1195,-311,1261,-157,808,-146,807,-150,49,-151,52,-227,1314,-224,1325,-228,1360});
    states[1267] = new State(-356);
    states[1268] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403},new int[]{-169,1269,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1269] = new State(new int[]{11,1297,5,-390},new int[]{-232,1270,-237,1294});
    states[1270] = new State(new int[]{85,1283,86,1289,10,-397},new int[]{-201,1271});
    states[1271] = new State(new int[]{10,1272});
    states[1272] = new State(new int[]{63,1277,153,1279,152,1280,148,1281,151,1282,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-204,1273,-209,1274});
    states[1273] = new State(-381);
    states[1274] = new State(new int[]{10,1275});
    states[1275] = new State(new int[]{63,1277,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-204,1276});
    states[1276] = new State(-382);
    states[1277] = new State(new int[]{10,1278});
    states[1278] = new State(-388);
    states[1279] = new State(-851);
    states[1280] = new State(-852);
    states[1281] = new State(-853);
    states[1282] = new State(-854);
    states[1283] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,831,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680,10,-396},new int[]{-113,1284,-87,1288,-86,27,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,832,-99,657,-323,675});
    states[1284] = new State(new int[]{86,1286,10,-400},new int[]{-202,1285});
    states[1285] = new State(-398);
    states[1286] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494},new int[]{-260,1287,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1287] = new State(-401);
    states[1288] = new State(-395);
    states[1289] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494},new int[]{-260,1290,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1290] = new State(new int[]{85,1292,10,-402},new int[]{-203,1291});
    states[1291] = new State(-399);
    states[1292] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,831,8,711,21,292,22,297,76,444,11,366,40,587,5,596,18,667,37,676,44,680,10,-396},new int[]{-113,1293,-87,1288,-86,27,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-322,832,-99,657,-323,675});
    states[1293] = new State(-403);
    states[1294] = new State(new int[]{5,1295});
    states[1295] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,1296,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1296] = new State(-389);
    states[1297] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-236,1298,-235,1305,-157,1302,-146,807,-150,49,-151,52});
    states[1298] = new State(new int[]{12,1299,10,1300});
    states[1299] = new State(-391);
    states[1300] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-235,1301,-157,1302,-146,807,-150,49,-151,52});
    states[1301] = new State(-393);
    states[1302] = new State(new int[]{5,1303,100,542});
    states[1303] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,1304,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1304] = new State(-394);
    states[1305] = new State(-392);
    states[1306] = new State(new int[]{46,1307});
    states[1307] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403},new int[]{-169,1308,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1308] = new State(new int[]{11,1297,5,-390},new int[]{-232,1309,-237,1294});
    states[1309] = new State(new int[]{110,1312,10,-386},new int[]{-210,1310});
    states[1310] = new State(new int[]{10,1311});
    states[1311] = new State(-384);
    states[1312] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,1313,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[1313] = new State(new int[]{70,28,10,-385});
    states[1314] = new State(new int[]{107,1316,11,-370,28,-370,26,-370,44,-370,37,-370,19,-370,30,-370,31,-370,46,-370,27,-370,92,-370,84,-370,83,-370,82,-370,81,-370,59,-70,29,-70,66,-70,50,-70,53,-70,62,-70,91,-70},new int[]{-175,1199,-42,1200,-38,1203,-60,1315});
    states[1315] = new State(-469);
    states[1316] = new State(new int[]{10,1324,143,48,85,50,86,51,80,53,78,54,160,55,87,56,144,279,147,280,145,282,146,283},new int[]{-107,1317,-146,1321,-150,49,-151,52,-164,1322,-166,277,-165,281});
    states[1317] = new State(new int[]{80,1318,10,1323});
    states[1318] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,144,279,147,280,145,282,146,283},new int[]{-107,1319,-146,1321,-150,49,-151,52,-164,1322,-166,277,-165,281});
    states[1319] = new State(new int[]{10,1320});
    states[1320] = new State(-462);
    states[1321] = new State(-465);
    states[1322] = new State(-466);
    states[1323] = new State(-463);
    states[1324] = new State(-464);
    states[1325] = new State(-371);
    states[1326] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403},new int[]{-170,1327,-169,1179,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1327] = new State(new int[]{8,563,10,-470,110,-470},new int[]{-127,1328});
    states[1328] = new State(new int[]{10,1358,110,-830},new int[]{-206,1329,-207,1354});
    states[1329] = new State(new int[]{23,1333,107,-322,91,-322,59,-322,29,-322,66,-322,50,-322,53,-322,62,-322,11,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,92,-322,84,-322,83,-322,82,-322,81,-322,149,-322,41,-322},new int[]{-317,1330,-316,1331,-315,1353});
    states[1330] = new State(-459);
    states[1331] = new State(new int[]{23,1333,11,-323,92,-323,84,-323,83,-323,82,-323,81,-323,29,-323,143,-323,85,-323,86,-323,80,-323,78,-323,160,-323,87,-323,62,-323,28,-323,26,-323,44,-323,37,-323,19,-323,30,-323,31,-323,46,-323,27,-323,10,-323,107,-323,91,-323,59,-323,66,-323,50,-323,53,-323,149,-323,41,-323},new int[]{-315,1332});
    states[1332] = new State(-325);
    states[1333] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-157,1334,-146,807,-150,49,-151,52});
    states[1334] = new State(new int[]{5,1335,100,542});
    states[1335] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,1341,49,547,34,551,73,555,44,561,37,602,26,1350,30,1351},new int[]{-287,1336,-285,1352,-276,1340,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1336] = new State(new int[]{10,1337,100,1338});
    states[1337] = new State(-326);
    states[1338] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,1341,49,547,34,551,73,555,44,561,37,602,26,1350,30,1351},new int[]{-285,1339,-276,1340,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1339] = new State(-328);
    states[1340] = new State(-329);
    states[1341] = new State(new int[]{8,1342,10,-331,100,-331,23,-315,11,-315,92,-315,84,-315,83,-315,82,-315,81,-315,29,-315,143,-315,85,-315,86,-315,80,-315,78,-315,160,-315,87,-315,62,-315,28,-315,26,-315,44,-315,37,-315,19,-315,30,-315,31,-315,46,-315,27,-315},new int[]{-182,501});
    states[1342] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-181,1343,-180,1349,-179,1347,-146,218,-150,49,-151,52,-300,1348});
    states[1343] = new State(new int[]{9,1344,100,1345});
    states[1344] = new State(-316);
    states[1345] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-180,1346,-179,1347,-146,218,-150,49,-151,52,-300,1348});
    states[1346] = new State(-318);
    states[1347] = new State(new int[]{7,182,123,187,9,-319,100,-319},new int[]{-298,634});
    states[1348] = new State(-320);
    states[1349] = new State(-317);
    states[1350] = new State(-330);
    states[1351] = new State(-332);
    states[1352] = new State(-327);
    states[1353] = new State(-324);
    states[1354] = new State(new int[]{110,1355});
    states[1355] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494},new int[]{-260,1356,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1356] = new State(new int[]{10,1357});
    states[1357] = new State(-444);
    states[1358] = new State(new int[]{148,1171,150,1172,151,1173,152,1174,154,1175,153,1176,23,-828,107,-828,91,-828,59,-828,29,-828,66,-828,50,-828,53,-828,62,-828,11,-828,28,-828,26,-828,44,-828,37,-828,19,-828,30,-828,31,-828,46,-828,27,-828,92,-828,84,-828,83,-828,82,-828,81,-828,149,-828},new int[]{-205,1359,-208,1177});
    states[1359] = new State(new int[]{10,1169,110,-831});
    states[1360] = new State(-372);
    states[1361] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403},new int[]{-169,1362,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1362] = new State(new int[]{8,563,5,-470,10,-470,110,-470},new int[]{-127,1363});
    states[1363] = new State(new int[]{5,1366,10,1358,110,-830},new int[]{-206,1364,-207,1374});
    states[1364] = new State(new int[]{23,1333,107,-322,91,-322,59,-322,29,-322,66,-322,50,-322,53,-322,62,-322,11,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,92,-322,84,-322,83,-322,82,-322,81,-322,149,-322,41,-322},new int[]{-317,1365,-316,1331,-315,1353});
    states[1365] = new State(-460);
    states[1366] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,1367,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1367] = new State(new int[]{10,1358,110,-830},new int[]{-206,1368,-207,1370});
    states[1368] = new State(new int[]{23,1333,107,-322,91,-322,59,-322,29,-322,66,-322,50,-322,53,-322,62,-322,11,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,92,-322,84,-322,83,-322,82,-322,81,-322,149,-322,41,-322},new int[]{-317,1369,-316,1331,-315,1353});
    states[1369] = new State(-461);
    states[1370] = new State(new int[]{110,1371});
    states[1371] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,711,21,292,22,297,76,444,11,366,40,587,18,667,37,676,44,680},new int[]{-102,1372,-100,879,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-322,880,-99,657,-323,675});
    states[1372] = new State(new int[]{10,1373});
    states[1373] = new State(-442);
    states[1374] = new State(new int[]{110,1375});
    states[1375] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,711,21,292,22,297,76,444,11,366,40,587,18,667,37,676,44,680},new int[]{-102,1376,-100,879,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-322,880,-99,657,-323,675});
    states[1376] = new State(new int[]{10,1377});
    states[1377] = new State(-443);
    states[1378] = new State(-357);
    states[1379] = new State(-358);
    states[1380] = new State(-366);
    states[1381] = new State(new int[]{28,1378,26,1379,44,1326,37,1361},new int[]{-3,1382,-229,1385,-215,1386,-227,1314,-224,1325,-228,1360});
    states[1382] = new State(new int[]{44,1326,37,1361},new int[]{-229,1383,-215,1384,-227,1314,-224,1325,-228,1360});
    states[1383] = new State(-367);
    states[1384] = new State(-439);
    states[1385] = new State(-368);
    states[1386] = new State(-437);
    states[1387] = new State(new int[]{107,1316,11,-369,28,-369,26,-369,44,-369,37,-369,19,-369,30,-369,31,-369,46,-369,27,-369,92,-369,84,-369,83,-369,82,-369,81,-369,59,-70,29,-70,66,-70,50,-70,53,-70,62,-70,91,-70},new int[]{-175,1388,-42,1200,-38,1203,-60,1315});
    states[1388] = new State(-421);
    states[1389] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,8,-377,110,-377,10,-377},new int[]{-171,1390,-170,1178,-169,1179,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1390] = new State(new int[]{8,563,110,-470,10,-470},new int[]{-127,1391});
    states[1391] = new State(new int[]{110,1393,10,1167},new int[]{-206,1392});
    states[1392] = new State(-373);
    states[1393] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494},new int[]{-260,1394,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1394] = new State(new int[]{10,1395});
    states[1395] = new State(-422);
    states[1396] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,8,-377,10,-377},new int[]{-171,1397,-170,1178,-169,1179,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1397] = new State(new int[]{8,563,10,-470},new int[]{-127,1398});
    states[1398] = new State(new int[]{10,1167},new int[]{-206,1399});
    states[1399] = new State(-375);
    states[1400] = new State(-363);
    states[1401] = new State(-436);
    states[1402] = new State(-364);
    states[1403] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403},new int[]{-169,1404,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1404] = new State(new int[]{11,1297,5,-390},new int[]{-232,1405,-237,1294});
    states[1405] = new State(new int[]{85,1283,86,1289,10,-397},new int[]{-201,1406});
    states[1406] = new State(new int[]{10,1407});
    states[1407] = new State(new int[]{63,1277,153,1279,152,1280,148,1281,151,1282,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-204,1408,-209,1409});
    states[1408] = new State(-379);
    states[1409] = new State(new int[]{10,1410});
    states[1410] = new State(new int[]{63,1277,11,-387,28,-387,26,-387,44,-387,37,-387,19,-387,30,-387,31,-387,46,-387,27,-387,92,-387,84,-387,83,-387,82,-387,81,-387},new int[]{-204,1411});
    states[1411] = new State(-380);
    states[1412] = new State(new int[]{46,1413});
    states[1413] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403},new int[]{-169,1414,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1414] = new State(new int[]{11,1297,5,-390},new int[]{-232,1415,-237,1294});
    states[1415] = new State(new int[]{110,1312,10,-386},new int[]{-210,1416});
    states[1416] = new State(new int[]{10,1417});
    states[1417] = new State(-383);
    states[1418] = new State(new int[]{11,626,92,-341,84,-341,83,-341,82,-341,81,-341,28,-212,26,-212,44,-212,37,-212,19,-212,30,-212,31,-212,46,-212,27,-212},new int[]{-53,512,-52,513,-6,514,-250,1153,-54,1419});
    states[1419] = new State(-353);
    states[1420] = new State(-350);
    states[1421] = new State(-307);
    states[1422] = new State(-308);
    states[1423] = new State(new int[]{26,1424,48,1425,43,1426,8,-309,23,-309,11,-309,92,-309,84,-309,83,-309,82,-309,81,-309,29,-309,143,-309,85,-309,86,-309,80,-309,78,-309,160,-309,87,-309,62,-309,28,-309,44,-309,37,-309,19,-309,30,-309,31,-309,46,-309,27,-309,10,-309});
    states[1424] = new State(-310);
    states[1425] = new State(-311);
    states[1426] = new State(-312);
    states[1427] = new State(new int[]{68,1429,69,1430,148,1431,27,1432,28,1433,26,-304,43,-304,64,-304},new int[]{-19,1428});
    states[1428] = new State(-306);
    states[1429] = new State(-298);
    states[1430] = new State(-299);
    states[1431] = new State(-300);
    states[1432] = new State(-301);
    states[1433] = new State(-302);
    states[1434] = new State(-305);
    states[1435] = new State(new int[]{123,1437,120,-220},new int[]{-154,1436});
    states[1436] = new State(-221);
    states[1437] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-157,1438,-146,807,-150,49,-151,52});
    states[1438] = new State(new int[]{122,1439,121,1186,100,542});
    states[1439] = new State(-222);
    states[1440] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602,68,1429,69,1430,148,1431,27,1432,28,1433,26,-303,43,-303,64,-303},new int[]{-286,1441,-276,1241,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560,-29,1242,-20,1243,-21,1427,-19,1434});
    states[1441] = new State(new int[]{10,1442});
    states[1442] = new State(-219);
    states[1443] = new State(new int[]{11,626,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-48,1444,-6,1235,-250,1153});
    states[1444] = new State(-105);
    states[1445] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1450,59,-91,29,-91,66,-91,50,-91,53,-91,62,-91,91,-91},new int[]{-313,1446,-310,1447,-311,1448,-157,808,-146,807,-150,49,-151,52});
    states[1446] = new State(-111);
    states[1447] = new State(-107);
    states[1448] = new State(new int[]{10,1449});
    states[1449] = new State(-404);
    states[1450] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,1451,-150,49,-151,52});
    states[1451] = new State(new int[]{100,1452});
    states[1452] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-157,1453,-146,807,-150,49,-151,52});
    states[1453] = new State(new int[]{9,1454,100,542});
    states[1454] = new State(new int[]{110,1455});
    states[1455] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,1456,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[1456] = new State(new int[]{10,1457});
    states[1457] = new State(-108);
    states[1458] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1450},new int[]{-313,1459,-310,1447,-311,1448,-157,808,-146,807,-150,49,-151,52});
    states[1459] = new State(-109);
    states[1460] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1450},new int[]{-313,1461,-310,1447,-311,1448,-157,808,-146,807,-150,49,-151,52});
    states[1461] = new State(-110);
    states[1462] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,354,12,-279,100,-279},new int[]{-271,1463,-272,1464,-92,194,-105,312,-106,313,-179,350,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281});
    states[1463] = new State(-277);
    states[1464] = new State(-278);
    states[1465] = new State(-276);
    states[1466] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-276,1467,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1467] = new State(-275);
    states[1468] = new State(-245);
    states[1469] = new State(-246);
    states[1470] = new State(new int[]{127,481,121,-247,100,-247,120,-247,9,-247,10,-247,8,-247,138,-247,136,-247,118,-247,117,-247,131,-247,132,-247,133,-247,134,-247,130,-247,116,-247,115,-247,128,-247,129,-247,126,-247,6,-247,5,-247,125,-247,123,-247,124,-247,122,-247,137,-247,135,-247,16,-247,70,-247,92,-247,98,-247,101,-247,33,-247,104,-247,2,-247,12,-247,99,-247,32,-247,85,-247,84,-247,83,-247,82,-247,81,-247,86,-247,13,-247,76,-247,51,-247,58,-247,141,-247,143,-247,80,-247,78,-247,160,-247,87,-247,45,-247,42,-247,21,-247,22,-247,144,-247,147,-247,145,-247,146,-247,155,-247,158,-247,157,-247,156,-247,11,-247,57,-247,91,-247,40,-247,25,-247,97,-247,54,-247,35,-247,55,-247,102,-247,47,-247,36,-247,53,-247,60,-247,74,-247,72,-247,38,-247,71,-247,110,-247});
    states[1471] = new State(-691);
    states[1472] = new State(new int[]{8,1473});
    states[1473] = new State(new int[]{14,470,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,53,472,143,48,85,50,86,51,80,53,78,54,160,55,87,56,11,924,8,937},new int[]{-353,1474,-351,1480,-15,471,-164,334,-166,277,-165,281,-16,335,-340,1471,-283,1472,-179,181,-146,218,-150,49,-151,52,-343,1478,-344,1479});
    states[1474] = new State(new int[]{9,1475,10,468,100,1476});
    states[1475] = new State(-649);
    states[1476] = new State(new int[]{14,470,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,53,472,143,48,85,50,86,51,80,53,78,54,160,55,87,56,11,924,8,937},new int[]{-351,1477,-15,471,-164,334,-166,277,-165,281,-16,335,-340,1471,-283,1472,-179,181,-146,218,-150,49,-151,52,-343,1478,-344,1479});
    states[1477] = new State(-686);
    states[1478] = new State(-692);
    states[1479] = new State(-693);
    states[1480] = new State(-684);
    states[1481] = new State(-780);
    states[1482] = new State(new int[]{13,480});
    states[1483] = new State(-240);
    states[1484] = new State(-236);
    states[1485] = new State(-627);
    states[1486] = new State(new int[]{8,1487});
    states[1487] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-333,1488,-332,1496,-146,1492,-150,49,-151,52,-100,1495,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[1488] = new State(new int[]{9,1489,100,1490});
    states[1489] = new State(-638);
    states[1490] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-332,1491,-146,1492,-150,49,-151,52,-100,1495,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[1491] = new State(-642);
    states[1492] = new State(new int[]{110,1493,11,-791,17,-791,8,-791,7,-791,142,-791,4,-791,15,-791,138,-791,136,-791,118,-791,117,-791,131,-791,132,-791,133,-791,134,-791,130,-791,116,-791,115,-791,128,-791,129,-791,126,-791,6,-791,120,-791,125,-791,123,-791,121,-791,124,-791,122,-791,137,-791,135,-791,16,-791,9,-791,100,-791,13,-791,119,-791});
    states[1493] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587},new int[]{-100,1494,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586});
    states[1494] = new State(-639);
    states[1495] = new State(-640);
    states[1496] = new State(-641);
    states[1497] = new State(new int[]{13,206,16,210,5,-706,12,-706});
    states[1498] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-89,1499,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[1499] = new State(new int[]{13,206,16,210,100,-189,9,-189,12,-189,5,-189});
    states[1500] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337,5,-707,12,-707},new int[]{-121,1501,-89,1497,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[1501] = new State(new int[]{5,1502,12,-713});
    states[1502] = new State(new int[]{143,48,85,50,86,51,80,53,78,261,160,55,87,56,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,42,289,21,292,22,297,11,366,76,817,56,820,141,821,8,836,135,839,116,351,115,352,63,337},new int[]{-89,1503,-90,236,-80,244,-13,249,-10,259,-14,222,-146,260,-150,49,-151,52,-164,276,-166,277,-165,281,-16,284,-257,291,-294,296,-238,364,-239,365,-198,845,-172,843,-56,844,-265,851,-269,852,-11,847,-241,853});
    states[1503] = new State(new int[]{13,206,16,210,12,-715});
    states[1504] = new State(-186);
    states[1505] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283},new int[]{-92,1506,-105,312,-106,313,-179,350,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281});
    states[1506] = new State(new int[]{116,245,115,246,128,247,129,248,13,-249,121,-249,100,-249,120,-249,9,-249,10,-249,8,-249,138,-249,136,-249,118,-249,117,-249,131,-249,132,-249,133,-249,134,-249,130,-249,126,-249,6,-249,5,-249,125,-249,123,-249,124,-249,122,-249,137,-249,135,-249,16,-249,70,-249,92,-249,98,-249,101,-249,33,-249,104,-249,2,-249,12,-249,99,-249,32,-249,85,-249,84,-249,83,-249,82,-249,81,-249,86,-249,76,-249,51,-249,58,-249,141,-249,143,-249,80,-249,78,-249,160,-249,87,-249,45,-249,42,-249,21,-249,22,-249,144,-249,147,-249,145,-249,146,-249,155,-249,158,-249,157,-249,156,-249,11,-249,57,-249,91,-249,40,-249,25,-249,97,-249,54,-249,35,-249,55,-249,102,-249,47,-249,36,-249,53,-249,60,-249,74,-249,72,-249,38,-249,71,-249,127,-249,110,-249},new int[]{-192,195});
    states[1507] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,24,492},new int[]{-283,1508,-278,1509,-179,181,-146,218,-150,49,-151,52,-270,498});
    states[1508] = new State(-738);
    states[1509] = new State(-739);
    states[1510] = new State(-752);
    states[1511] = new State(-753);
    states[1512] = new State(-754);
    states[1513] = new State(-755);
    states[1514] = new State(-756);
    states[1515] = new State(-757);
    states[1516] = new State(-758);
    states[1517] = new State(-728);
    states[1518] = new State(-647);
    states[1519] = new State(-35);
    states[1520] = new State(new int[]{59,1206,29,1227,66,1231,50,1443,53,1458,62,1460,11,626,91,-64,92,-64,103,-64,44,-212,37,-212,28,-212,26,-212,19,-212,30,-212,31,-212},new int[]{-46,1521,-167,1522,-28,1523,-51,1524,-288,1525,-309,1526,-219,1527,-6,1528,-250,1153});
    states[1521] = new State(-68);
    states[1522] = new State(-78);
    states[1523] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-79,29,-79,66,-79,50,-79,53,-79,62,-79,11,-79,44,-79,37,-79,28,-79,26,-79,19,-79,30,-79,31,-79,91,-79,92,-79,103,-79},new int[]{-26,1213,-27,1214,-140,1216,-146,1226,-150,49,-151,52});
    states[1524] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-80,29,-80,66,-80,50,-80,53,-80,62,-80,11,-80,44,-80,37,-80,28,-80,26,-80,19,-80,30,-80,31,-80,91,-80,92,-80,103,-80},new int[]{-26,1230,-27,1214,-140,1216,-146,1226,-150,49,-151,52});
    states[1525] = new State(new int[]{11,626,59,-81,29,-81,66,-81,50,-81,53,-81,62,-81,44,-81,37,-81,28,-81,26,-81,19,-81,30,-81,31,-81,91,-81,92,-81,103,-81,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-48,1234,-6,1235,-250,1153});
    states[1526] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1450,59,-82,29,-82,66,-82,50,-82,53,-82,62,-82,11,-82,44,-82,37,-82,28,-82,26,-82,19,-82,30,-82,31,-82,91,-82,92,-82,103,-82},new int[]{-313,1446,-310,1447,-311,1448,-157,808,-146,807,-150,49,-151,52});
    states[1527] = new State(-83);
    states[1528] = new State(new int[]{44,1541,37,1548,28,1378,26,1379,19,1575,30,1582,31,1396,11,626},new int[]{-212,1529,-250,516,-213,1530,-220,1531,-227,1532,-224,1325,-228,1360,-3,1565,-216,1579,-226,1580});
    states[1529] = new State(-86);
    states[1530] = new State(-84);
    states[1531] = new State(-424);
    states[1532] = new State(new int[]{149,1534,107,1316,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,91,-67},new int[]{-177,1533,-176,1536,-40,1537,-41,1520,-60,1540});
    states[1533] = new State(-429);
    states[1534] = new State(new int[]{10,1535});
    states[1535] = new State(-435);
    states[1536] = new State(-445);
    states[1537] = new State(new int[]{91,17},new int[]{-255,1538});
    states[1538] = new State(new int[]{10,1539});
    states[1539] = new State(-467);
    states[1540] = new State(-446);
    states[1541] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403},new int[]{-170,1542,-169,1179,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1542] = new State(new int[]{8,563,10,-470,110,-470},new int[]{-127,1543});
    states[1543] = new State(new int[]{10,1358,110,-830},new int[]{-206,1329,-207,1544});
    states[1544] = new State(new int[]{110,1545});
    states[1545] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494},new int[]{-260,1546,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1546] = new State(new int[]{10,1547});
    states[1547] = new State(-434);
    states[1548] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403},new int[]{-169,1549,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1549] = new State(new int[]{8,563,5,-470,10,-470,110,-470},new int[]{-127,1550});
    states[1550] = new State(new int[]{5,1551,10,1358,110,-830},new int[]{-206,1364,-207,1559});
    states[1551] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,1552,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1552] = new State(new int[]{10,1358,110,-830},new int[]{-206,1368,-207,1553});
    states[1553] = new State(new int[]{110,1554});
    states[1554] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,711,21,292,22,297,76,444,11,366,40,587,18,667,37,676,44,680},new int[]{-100,1555,-322,1557,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-99,657,-323,675});
    states[1555] = new State(new int[]{10,1556});
    states[1556] = new State(-430);
    states[1557] = new State(new int[]{10,1558});
    states[1558] = new State(-432);
    states[1559] = new State(new int[]{110,1560});
    states[1560] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,711,21,292,22,297,76,444,11,366,40,587,18,667,37,676,44,680},new int[]{-100,1561,-322,1563,-98,30,-97,156,-104,584,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,639,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-99,657,-323,675});
    states[1561] = new State(new int[]{10,1562});
    states[1562] = new State(-431);
    states[1563] = new State(new int[]{10,1564});
    states[1564] = new State(-433);
    states[1565] = new State(new int[]{19,1567,30,1569,44,1541,37,1548},new int[]{-220,1566,-227,1532,-224,1325,-228,1360});
    states[1566] = new State(-425);
    states[1567] = new State(new int[]{44,1541,37,1548},new int[]{-220,1568,-227,1532,-224,1325,-228,1360});
    states[1568] = new State(-428);
    states[1569] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,8,-377,110,-377,10,-377},new int[]{-171,1570,-170,1178,-169,1179,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1570] = new State(new int[]{8,563,110,-470,10,-470},new int[]{-127,1571});
    states[1571] = new State(new int[]{110,1572,10,1167},new int[]{-206,524});
    states[1572] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494},new int[]{-260,1573,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1573] = new State(new int[]{10,1574});
    states[1574] = new State(-420);
    states[1575] = new State(new int[]{44,1541,37,1548,28,1378,26,1379},new int[]{-220,1576,-3,1577,-227,1532,-224,1325,-228,1360});
    states[1576] = new State(-426);
    states[1577] = new State(new int[]{44,1541,37,1548},new int[]{-220,1578,-227,1532,-224,1325,-228,1360});
    states[1578] = new State(-427);
    states[1579] = new State(-85);
    states[1580] = new State(-67,new int[]{-176,1581,-40,1537,-41,1520});
    states[1581] = new State(-418);
    states[1582] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403,8,-377,110,-377,10,-377},new int[]{-171,1583,-170,1178,-169,1179,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1583] = new State(new int[]{8,563,110,-470,10,-470},new int[]{-127,1584});
    states[1584] = new State(new int[]{110,1585,10,1167},new int[]{-206,1392});
    states[1585] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,10,-494},new int[]{-260,1586,-4,23,-112,24,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883});
    states[1586] = new State(new int[]{10,1587});
    states[1587] = new State(-419);
    states[1588] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-303,1589,-307,1599,-156,1593,-137,1598,-146,47,-150,49,-151,52,-292,57,-149,58,-293,148});
    states[1589] = new State(new int[]{10,1590,100,1591});
    states[1590] = new State(-38);
    states[1591] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-307,1592,-156,1593,-137,1598,-146,47,-150,49,-151,52,-292,57,-149,58,-293,148});
    states[1592] = new State(-44);
    states[1593] = new State(new int[]{7,1594,137,1596,10,-45,100,-45});
    states[1594] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-137,1595,-146,47,-150,49,-151,52,-292,57,-149,58,-293,148});
    states[1595] = new State(-37);
    states[1596] = new State(new int[]{144,1597});
    states[1597] = new State(-46);
    states[1598] = new State(-36);
    states[1599] = new State(-43);
    states[1600] = new State(new int[]{3,1602,52,-15,91,-15,59,-15,29,-15,66,-15,50,-15,53,-15,62,-15,11,-15,44,-15,37,-15,28,-15,26,-15,19,-15,30,-15,31,-15,43,-15,92,-15,103,-15},new int[]{-183,1601});
    states[1601] = new State(-17);
    states[1602] = new State(new int[]{143,1603,144,1604});
    states[1603] = new State(-18);
    states[1604] = new State(-19);
    states[1605] = new State(-16);
    states[1606] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-146,1607,-150,49,-151,52});
    states[1607] = new State(new int[]{10,1609,8,1610},new int[]{-186,1608});
    states[1608] = new State(-28);
    states[1609] = new State(-29);
    states[1610] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-188,1611,-145,1617,-146,1616,-150,49,-151,52});
    states[1611] = new State(new int[]{9,1612,100,1614});
    states[1612] = new State(new int[]{10,1613});
    states[1613] = new State(-30);
    states[1614] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-145,1615,-146,1616,-150,49,-151,52});
    states[1615] = new State(-32);
    states[1616] = new State(-33);
    states[1617] = new State(-31);
    states[1618] = new State(-3);
    states[1619] = new State(new int[]{43,1640,52,-41,59,-41,29,-41,66,-41,50,-41,53,-41,62,-41,11,-41,44,-41,37,-41,28,-41,26,-41,19,-41,30,-41,31,-41,92,-41,103,-41,91,-41},new int[]{-161,1620,-162,1637,-302,1666});
    states[1620] = new State(new int[]{41,1634},new int[]{-160,1621});
    states[1621] = new State(new int[]{92,1624,103,1625,91,1631},new int[]{-153,1622});
    states[1622] = new State(new int[]{7,1623});
    states[1623] = new State(-47);
    states[1624] = new State(-57);
    states[1625] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,104,-494,10,-494},new int[]{-252,1626,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[1626] = new State(new int[]{92,1627,104,1628,10,20});
    states[1627] = new State(-58);
    states[1628] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494},new int[]{-252,1629,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[1629] = new State(new int[]{92,1630,10,20});
    states[1630] = new State(-59);
    states[1631] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,92,-494,10,-494},new int[]{-252,1632,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038});
    states[1632] = new State(new int[]{92,1633,10,20});
    states[1633] = new State(-60);
    states[1634] = new State(-41,new int[]{-302,1635});
    states[1635] = new State(new int[]{52,1588,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,92,-67,103,-67,91,-67},new int[]{-40,1636,-304,14,-41,1520});
    states[1636] = new State(-55);
    states[1637] = new State(new int[]{92,1624,103,1625,91,1631},new int[]{-153,1638});
    states[1638] = new State(new int[]{7,1639});
    states[1639] = new State(-48);
    states[1640] = new State(-41,new int[]{-302,1641});
    states[1641] = new State(new int[]{52,1588,29,-62,66,-62,50,-62,53,-62,62,-62,11,-62,44,-62,37,-62,41,-62},new int[]{-39,1642,-304,14,-37,1643});
    states[1642] = new State(-54);
    states[1643] = new State(new int[]{29,1227,66,1231,50,1443,53,1458,62,1460,11,626,41,-61,44,-212,37,-212},new int[]{-47,1644,-28,1645,-51,1646,-288,1647,-309,1648,-231,1649,-6,1650,-250,1153,-230,1665});
    states[1644] = new State(-63);
    states[1645] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,29,-72,66,-72,50,-72,53,-72,62,-72,11,-72,44,-72,37,-72,41,-72},new int[]{-26,1213,-27,1214,-140,1216,-146,1226,-150,49,-151,52});
    states[1646] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,29,-73,66,-73,50,-73,53,-73,62,-73,11,-73,44,-73,37,-73,41,-73},new int[]{-26,1230,-27,1214,-140,1216,-146,1226,-150,49,-151,52});
    states[1647] = new State(new int[]{11,626,29,-74,66,-74,50,-74,53,-74,62,-74,44,-74,37,-74,41,-74,143,-212,85,-212,86,-212,80,-212,78,-212,160,-212,87,-212},new int[]{-48,1234,-6,1235,-250,1153});
    states[1648] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1450,29,-75,66,-75,50,-75,53,-75,62,-75,11,-75,44,-75,37,-75,41,-75},new int[]{-313,1446,-310,1447,-311,1448,-157,808,-146,807,-150,49,-151,52});
    states[1649] = new State(-76);
    states[1650] = new State(new int[]{44,1657,11,626,37,1660},new int[]{-224,1651,-250,516,-228,1654});
    states[1651] = new State(new int[]{149,1652,29,-92,66,-92,50,-92,53,-92,62,-92,11,-92,44,-92,37,-92,41,-92});
    states[1652] = new State(new int[]{10,1653});
    states[1653] = new State(-93);
    states[1654] = new State(new int[]{149,1655,29,-94,66,-94,50,-94,53,-94,62,-94,11,-94,44,-94,37,-94,41,-94});
    states[1655] = new State(new int[]{10,1656});
    states[1656] = new State(-95);
    states[1657] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403},new int[]{-170,1658,-169,1179,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1658] = new State(new int[]{8,563,10,-470},new int[]{-127,1659});
    states[1659] = new State(new int[]{10,1167},new int[]{-206,1329});
    states[1660] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,45,403},new int[]{-169,1661,-141,1180,-136,1181,-133,1182,-146,1187,-150,49,-151,52,-190,1188,-334,1190,-148,1194});
    states[1661] = new State(new int[]{8,563,5,-470,10,-470},new int[]{-127,1662});
    states[1662] = new State(new int[]{5,1663,10,1167},new int[]{-206,1364});
    states[1663] = new State(new int[]{143,360,85,50,86,51,80,53,78,54,160,55,87,56,155,285,158,286,157,287,156,288,116,351,115,352,144,279,147,280,145,282,146,283,8,476,142,487,24,492,48,500,49,547,34,551,73,555,44,561,37,602},new int[]{-275,1664,-276,489,-272,359,-92,194,-105,312,-106,313,-179,314,-146,218,-150,49,-151,52,-16,347,-198,348,-164,353,-166,277,-165,281,-273,478,-300,479,-256,485,-249,486,-281,490,-278,491,-270,498,-30,499,-263,546,-129,550,-130,554,-225,558,-223,559,-222,560});
    states[1664] = new State(new int[]{10,1167},new int[]{-206,1368});
    states[1665] = new State(-77);
    states[1666] = new State(new int[]{52,1588,59,-67,29,-67,66,-67,50,-67,53,-67,62,-67,11,-67,44,-67,37,-67,28,-67,26,-67,19,-67,30,-67,31,-67,92,-67,103,-67,91,-67},new int[]{-40,1667,-304,14,-41,1520});
    states[1667] = new State(-56);
    states[1668] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-138,1669,-146,1672,-150,49,-151,52});
    states[1669] = new State(new int[]{10,1670});
    states[1670] = new State(new int[]{3,1602,43,-14,92,-14,103,-14,91,-14,52,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-184,1671,-185,1600,-183,1605});
    states[1671] = new State(-49);
    states[1672] = new State(-53);
    states[1673] = new State(-51);
    states[1674] = new State(-52);
    states[1675] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-156,1676,-137,1598,-146,47,-150,49,-151,52,-292,57,-149,58,-293,148});
    states[1676] = new State(new int[]{10,1677,7,1594});
    states[1677] = new State(new int[]{3,1602,43,-14,92,-14,103,-14,91,-14,52,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-184,1678,-185,1600,-183,1605});
    states[1678] = new State(-50);
    states[1679] = new State(-4);
    states[1680] = new State(new int[]{50,1682,56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,435,21,292,22,297,76,444,11,366,40,587,5,596},new int[]{-86,1681,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,395,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595});
    states[1681] = new State(new int[]{70,28,2,-7});
    states[1682] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-143,1683,-146,1684,-150,49,-151,52});
    states[1683] = new State(-8);
    states[1684] = new State(new int[]{123,1184,2,-220},new int[]{-154,1436});
    states[1685] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-320,1686,-321,1687,-146,1691,-150,49,-151,52});
    states[1686] = new State(-9);
    states[1687] = new State(new int[]{7,1688,123,187,2,-785},new int[]{-298,1690});
    states[1688] = new State(new int[]{143,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-137,1689,-146,47,-150,49,-151,52,-292,57,-149,58,-293,148});
    states[1689] = new State(-784);
    states[1690] = new State(-786);
    states[1691] = new State(-783);
    states[1692] = new State(new int[]{56,43,144,279,147,280,145,282,146,283,155,285,158,286,157,287,156,288,63,337,135,375,116,351,115,352,142,385,141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,433,8,529,21,292,22,297,76,444,11,366,40,587,5,596,53,798},new int[]{-259,1693,-86,1694,-100,155,-98,30,-97,156,-104,166,-82,171,-81,177,-95,333,-15,44,-164,334,-166,277,-165,281,-16,335,-56,336,-198,382,-112,1121,-131,389,-110,397,-146,401,-150,49,-151,52,-190,402,-257,442,-294,443,-239,448,-111,449,-57,450,-115,456,-172,457,-268,458,-96,459,-264,463,-266,464,-267,576,-240,577,-116,578,-242,586,-119,595,-4,1695,-314,1696});
    states[1693] = new State(-10);
    states[1694] = new State(new int[]{70,28,2,-11});
    states[1695] = new State(-12);
    states[1696] = new State(-13);
    states[1697] = new State(new int[]{52,1588,141,-39,143,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,11,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,44,-39,37,-39,10,-39,2,-39},new int[]{-305,1698,-304,1702});
    states[1698] = new State(-65,new int[]{-43,1699});
    states[1699] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,44,1541,37,1548,10,-494,2,-494},new int[]{-252,1700,-220,1701,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038,-227,1532,-224,1325,-228,1360});
    states[1700] = new State(new int[]{10,20,2,-5});
    states[1701] = new State(-66);
    states[1702] = new State(-40);
    states[1703] = new State(new int[]{52,1588,141,-39,143,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,11,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,44,-39,37,-39,10,-39,2,-39},new int[]{-305,1704,-304,1702});
    states[1704] = new State(-65,new int[]{-43,1705});
    states[1705] = new State(new int[]{141,396,143,48,85,50,86,51,80,53,78,261,160,55,87,56,45,403,42,528,8,529,21,292,22,297,144,279,147,280,145,282,146,283,155,750,158,286,157,287,156,288,76,444,11,366,57,729,91,17,40,719,25,736,97,752,54,757,35,762,55,773,102,779,47,786,36,789,53,798,60,870,74,875,72,862,38,884,44,1541,37,1548,10,-494,2,-494},new int[]{-252,1706,-220,1701,-261,748,-260,22,-4,23,-112,24,-131,389,-110,397,-146,749,-150,49,-151,52,-190,402,-257,442,-294,443,-15,725,-164,334,-166,277,-165,281,-16,335,-239,448,-111,449,-57,726,-115,456,-211,727,-132,728,-255,733,-152,734,-34,735,-247,751,-318,756,-123,761,-319,772,-159,777,-301,778,-248,785,-122,788,-314,797,-58,866,-173,867,-172,868,-168,869,-125,874,-126,881,-124,882,-348,883,-142,1038,-227,1532,-224,1325,-228,1360});
    states[1706] = new State(new int[]{10,20,2,-6});

    rules[1] = new Rule(-361, new int[]{-1,2});
    rules[2] = new Rule(-1, new int[]{-233});
    rules[3] = new Rule(-1, new int[]{-306});
    rules[4] = new Rule(-1, new int[]{-174});
    rules[5] = new Rule(-1, new int[]{75,-305,-43,-252});
    rules[6] = new Rule(-1, new int[]{77,-305,-43,-252});
    rules[7] = new Rule(-174, new int[]{88,-86});
    rules[8] = new Rule(-174, new int[]{88,50,-143});
    rules[9] = new Rule(-174, new int[]{90,-320});
    rules[10] = new Rule(-174, new int[]{89,-259});
    rules[11] = new Rule(-259, new int[]{-86});
    rules[12] = new Rule(-259, new int[]{-4});
    rules[13] = new Rule(-259, new int[]{-314});
    rules[14] = new Rule(-184, new int[]{});
    rules[15] = new Rule(-184, new int[]{-185});
    rules[16] = new Rule(-185, new int[]{-183});
    rules[17] = new Rule(-185, new int[]{-185,-183});
    rules[18] = new Rule(-183, new int[]{3,143});
    rules[19] = new Rule(-183, new int[]{3,144});
    rules[20] = new Rule(-233, new int[]{-234,-184,-302,-17,-187});
    rules[21] = new Rule(-187, new int[]{7});
    rules[22] = new Rule(-187, new int[]{10});
    rules[23] = new Rule(-187, new int[]{5});
    rules[24] = new Rule(-187, new int[]{100});
    rules[25] = new Rule(-187, new int[]{6});
    rules[26] = new Rule(-187, new int[]{});
    rules[27] = new Rule(-234, new int[]{});
    rules[28] = new Rule(-234, new int[]{61,-146,-186});
    rules[29] = new Rule(-186, new int[]{10});
    rules[30] = new Rule(-186, new int[]{8,-188,9,10});
    rules[31] = new Rule(-188, new int[]{-145});
    rules[32] = new Rule(-188, new int[]{-188,100,-145});
    rules[33] = new Rule(-145, new int[]{-146});
    rules[34] = new Rule(-17, new int[]{-36,-255});
    rules[35] = new Rule(-36, new int[]{-40});
    rules[36] = new Rule(-156, new int[]{-137});
    rules[37] = new Rule(-156, new int[]{-156,7,-137});
    rules[38] = new Rule(-304, new int[]{52,-303,10});
    rules[39] = new Rule(-305, new int[]{});
    rules[40] = new Rule(-305, new int[]{-304});
    rules[41] = new Rule(-302, new int[]{});
    rules[42] = new Rule(-302, new int[]{-302,-304});
    rules[43] = new Rule(-303, new int[]{-307});
    rules[44] = new Rule(-303, new int[]{-303,100,-307});
    rules[45] = new Rule(-307, new int[]{-156});
    rules[46] = new Rule(-307, new int[]{-156,137,144});
    rules[47] = new Rule(-306, new int[]{-308,-161,-160,-153,7});
    rules[48] = new Rule(-306, new int[]{-308,-162,-153,7});
    rules[49] = new Rule(-308, new int[]{-2,-138,10,-184});
    rules[50] = new Rule(-308, new int[]{109,-156,10,-184});
    rules[51] = new Rule(-2, new int[]{105});
    rules[52] = new Rule(-2, new int[]{106});
    rules[53] = new Rule(-138, new int[]{-146});
    rules[54] = new Rule(-161, new int[]{43,-302,-39});
    rules[55] = new Rule(-160, new int[]{41,-302,-40});
    rules[56] = new Rule(-162, new int[]{-302,-40});
    rules[57] = new Rule(-153, new int[]{92});
    rules[58] = new Rule(-153, new int[]{103,-252,92});
    rules[59] = new Rule(-153, new int[]{103,-252,104,-252,92});
    rules[60] = new Rule(-153, new int[]{91,-252,92});
    rules[61] = new Rule(-39, new int[]{-37});
    rules[62] = new Rule(-37, new int[]{});
    rules[63] = new Rule(-37, new int[]{-37,-47});
    rules[64] = new Rule(-40, new int[]{-41});
    rules[65] = new Rule(-43, new int[]{});
    rules[66] = new Rule(-43, new int[]{-43,-220});
    rules[67] = new Rule(-41, new int[]{});
    rules[68] = new Rule(-41, new int[]{-41,-46});
    rules[69] = new Rule(-42, new int[]{-38});
    rules[70] = new Rule(-38, new int[]{});
    rules[71] = new Rule(-38, new int[]{-38,-45});
    rules[72] = new Rule(-47, new int[]{-28});
    rules[73] = new Rule(-47, new int[]{-51});
    rules[74] = new Rule(-47, new int[]{-288});
    rules[75] = new Rule(-47, new int[]{-309});
    rules[76] = new Rule(-47, new int[]{-231});
    rules[77] = new Rule(-47, new int[]{-230});
    rules[78] = new Rule(-46, new int[]{-167});
    rules[79] = new Rule(-46, new int[]{-28});
    rules[80] = new Rule(-46, new int[]{-51});
    rules[81] = new Rule(-46, new int[]{-288});
    rules[82] = new Rule(-46, new int[]{-309});
    rules[83] = new Rule(-46, new int[]{-219});
    rules[84] = new Rule(-212, new int[]{-213});
    rules[85] = new Rule(-212, new int[]{-216});
    rules[86] = new Rule(-219, new int[]{-6,-212});
    rules[87] = new Rule(-45, new int[]{-167});
    rules[88] = new Rule(-45, new int[]{-28});
    rules[89] = new Rule(-45, new int[]{-51});
    rules[90] = new Rule(-45, new int[]{-288});
    rules[91] = new Rule(-45, new int[]{-309});
    rules[92] = new Rule(-231, new int[]{-6,-224});
    rules[93] = new Rule(-231, new int[]{-6,-224,149,10});
    rules[94] = new Rule(-230, new int[]{-6,-228});
    rules[95] = new Rule(-230, new int[]{-6,-228,149,10});
    rules[96] = new Rule(-167, new int[]{59,-155,10});
    rules[97] = new Rule(-155, new int[]{-142});
    rules[98] = new Rule(-155, new int[]{-155,100,-142});
    rules[99] = new Rule(-142, new int[]{155});
    rules[100] = new Rule(-142, new int[]{-146});
    rules[101] = new Rule(-28, new int[]{29,-26});
    rules[102] = new Rule(-28, new int[]{-28,-26});
    rules[103] = new Rule(-51, new int[]{66,-26});
    rules[104] = new Rule(-51, new int[]{-51,-26});
    rules[105] = new Rule(-288, new int[]{50,-48});
    rules[106] = new Rule(-288, new int[]{-288,-48});
    rules[107] = new Rule(-313, new int[]{-310});
    rules[108] = new Rule(-313, new int[]{8,-146,100,-157,9,110,-100,10});
    rules[109] = new Rule(-309, new int[]{53,-313});
    rules[110] = new Rule(-309, new int[]{62,-313});
    rules[111] = new Rule(-309, new int[]{-309,-313});
    rules[112] = new Rule(-26, new int[]{-27,10});
    rules[113] = new Rule(-27, new int[]{-140,120,-108});
    rules[114] = new Rule(-27, new int[]{-140,5,-276,120,-83});
    rules[115] = new Rule(-108, new int[]{-89});
    rules[116] = new Rule(-108, new int[]{-94});
    rules[117] = new Rule(-140, new int[]{-146});
    rules[118] = new Rule(-90, new int[]{-80});
    rules[119] = new Rule(-90, new int[]{-90,-191,-80});
    rules[120] = new Rule(-89, new int[]{-90});
    rules[121] = new Rule(-89, new int[]{-241});
    rules[122] = new Rule(-89, new int[]{-89,16,-90});
    rules[123] = new Rule(-241, new int[]{-89,13,-89,5,-89});
    rules[124] = new Rule(-191, new int[]{120});
    rules[125] = new Rule(-191, new int[]{125});
    rules[126] = new Rule(-191, new int[]{123});
    rules[127] = new Rule(-191, new int[]{121});
    rules[128] = new Rule(-191, new int[]{124});
    rules[129] = new Rule(-191, new int[]{122});
    rules[130] = new Rule(-191, new int[]{137});
    rules[131] = new Rule(-80, new int[]{-13});
    rules[132] = new Rule(-80, new int[]{-80,-192,-13});
    rules[133] = new Rule(-192, new int[]{116});
    rules[134] = new Rule(-192, new int[]{115});
    rules[135] = new Rule(-192, new int[]{128});
    rules[136] = new Rule(-192, new int[]{129});
    rules[137] = new Rule(-265, new int[]{-13,-200,-283});
    rules[138] = new Rule(-269, new int[]{-11,119,-10});
    rules[139] = new Rule(-269, new int[]{-11,119,-269});
    rules[140] = new Rule(-269, new int[]{-198,-269});
    rules[141] = new Rule(-13, new int[]{-10});
    rules[142] = new Rule(-13, new int[]{-265});
    rules[143] = new Rule(-13, new int[]{-269});
    rules[144] = new Rule(-13, new int[]{-13,-194,-10});
    rules[145] = new Rule(-13, new int[]{-13,-194,-269});
    rules[146] = new Rule(-194, new int[]{118});
    rules[147] = new Rule(-194, new int[]{117});
    rules[148] = new Rule(-194, new int[]{131});
    rules[149] = new Rule(-194, new int[]{132});
    rules[150] = new Rule(-194, new int[]{133});
    rules[151] = new Rule(-194, new int[]{134});
    rules[152] = new Rule(-194, new int[]{130});
    rules[153] = new Rule(-11, new int[]{-14});
    rules[154] = new Rule(-11, new int[]{8,-89,9});
    rules[155] = new Rule(-10, new int[]{-14});
    rules[156] = new Rule(-10, new int[]{-238});
    rules[157] = new Rule(-10, new int[]{56});
    rules[158] = new Rule(-10, new int[]{141,-10});
    rules[159] = new Rule(-10, new int[]{8,-89,9});
    rules[160] = new Rule(-10, new int[]{135,-10});
    rules[161] = new Rule(-10, new int[]{-198,-10});
    rules[162] = new Rule(-10, new int[]{-172});
    rules[163] = new Rule(-10, new int[]{-56});
    rules[164] = new Rule(-239, new int[]{11,-68,12});
    rules[165] = new Rule(-238, new int[]{-239});
    rules[166] = new Rule(-238, new int[]{76,-68,76});
    rules[167] = new Rule(-198, new int[]{116});
    rules[168] = new Rule(-198, new int[]{115});
    rules[169] = new Rule(-14, new int[]{-146});
    rules[170] = new Rule(-14, new int[]{-164});
    rules[171] = new Rule(-14, new int[]{-16});
    rules[172] = new Rule(-14, new int[]{42,-146});
    rules[173] = new Rule(-14, new int[]{-257});
    rules[174] = new Rule(-14, new int[]{-294});
    rules[175] = new Rule(-14, new int[]{-14,-12});
    rules[176] = new Rule(-14, new int[]{-14,4,-298});
    rules[177] = new Rule(-14, new int[]{-14,11,-120,12});
    rules[178] = new Rule(-12, new int[]{7,-137});
    rules[179] = new Rule(-12, new int[]{142});
    rules[180] = new Rule(-12, new int[]{8,-75,9});
    rules[181] = new Rule(-12, new int[]{11,-74,12});
    rules[182] = new Rule(-75, new int[]{-70});
    rules[183] = new Rule(-75, new int[]{});
    rules[184] = new Rule(-74, new int[]{-72});
    rules[185] = new Rule(-74, new int[]{});
    rules[186] = new Rule(-72, new int[]{-93});
    rules[187] = new Rule(-72, new int[]{-72,100,-93});
    rules[188] = new Rule(-93, new int[]{-89});
    rules[189] = new Rule(-93, new int[]{-89,6,-89});
    rules[190] = new Rule(-16, new int[]{155});
    rules[191] = new Rule(-16, new int[]{158});
    rules[192] = new Rule(-16, new int[]{157});
    rules[193] = new Rule(-16, new int[]{156});
    rules[194] = new Rule(-83, new int[]{-89});
    rules[195] = new Rule(-83, new int[]{-94});
    rules[196] = new Rule(-83, new int[]{-243});
    rules[197] = new Rule(-94, new int[]{8,-65,9});
    rules[198] = new Rule(-65, new int[]{});
    rules[199] = new Rule(-65, new int[]{-64});
    rules[200] = new Rule(-64, new int[]{-84});
    rules[201] = new Rule(-64, new int[]{-64,100,-84});
    rules[202] = new Rule(-243, new int[]{8,-245,9});
    rules[203] = new Rule(-245, new int[]{-244});
    rules[204] = new Rule(-245, new int[]{-244,10});
    rules[205] = new Rule(-244, new int[]{-246});
    rules[206] = new Rule(-244, new int[]{-244,10,-246});
    rules[207] = new Rule(-246, new int[]{-135,5,-83});
    rules[208] = new Rule(-135, new int[]{-146});
    rules[209] = new Rule(-48, new int[]{-6,-49});
    rules[210] = new Rule(-6, new int[]{-250});
    rules[211] = new Rule(-6, new int[]{-6,-250});
    rules[212] = new Rule(-6, new int[]{});
    rules[213] = new Rule(-250, new int[]{11,-251,12});
    rules[214] = new Rule(-251, new int[]{-8});
    rules[215] = new Rule(-251, new int[]{-251,100,-8});
    rules[216] = new Rule(-8, new int[]{-9});
    rules[217] = new Rule(-8, new int[]{-146,5,-9});
    rules[218] = new Rule(-49, new int[]{-143,120,-286,10});
    rules[219] = new Rule(-49, new int[]{-144,-286,10});
    rules[220] = new Rule(-143, new int[]{-146});
    rules[221] = new Rule(-143, new int[]{-146,-154});
    rules[222] = new Rule(-144, new int[]{-146,123,-157,122});
    rules[223] = new Rule(-286, new int[]{-276});
    rules[224] = new Rule(-286, new int[]{-29});
    rules[225] = new Rule(-273, new int[]{-272,13});
    rules[226] = new Rule(-273, new int[]{-300,13});
    rules[227] = new Rule(-276, new int[]{-272});
    rules[228] = new Rule(-276, new int[]{-273});
    rules[229] = new Rule(-276, new int[]{-256});
    rules[230] = new Rule(-276, new int[]{-249});
    rules[231] = new Rule(-276, new int[]{-281});
    rules[232] = new Rule(-276, new int[]{-225});
    rules[233] = new Rule(-276, new int[]{-300});
    rules[234] = new Rule(-300, new int[]{-179,-298});
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
    rules[246] = new Rule(-279, new int[]{-225});
    rules[247] = new Rule(-279, new int[]{-300});
    rules[248] = new Rule(-272, new int[]{-92});
    rules[249] = new Rule(-272, new int[]{-92,6,-92});
    rules[250] = new Rule(-272, new int[]{8,-79,9});
    rules[251] = new Rule(-92, new int[]{-105});
    rules[252] = new Rule(-92, new int[]{-92,-192,-105});
    rules[253] = new Rule(-105, new int[]{-106});
    rules[254] = new Rule(-105, new int[]{-105,-194,-106});
    rules[255] = new Rule(-106, new int[]{-179});
    rules[256] = new Rule(-106, new int[]{-16});
    rules[257] = new Rule(-106, new int[]{-198,-106});
    rules[258] = new Rule(-106, new int[]{-164});
    rules[259] = new Rule(-106, new int[]{-106,8,-74,9});
    rules[260] = new Rule(-179, new int[]{-146});
    rules[261] = new Rule(-179, new int[]{-179,7,-137});
    rules[262] = new Rule(-79, new int[]{-77});
    rules[263] = new Rule(-79, new int[]{-79,100,-77});
    rules[264] = new Rule(-77, new int[]{-276});
    rules[265] = new Rule(-77, new int[]{-276,120,-86});
    rules[266] = new Rule(-249, new int[]{142,-275});
    rules[267] = new Rule(-281, new int[]{-278});
    rules[268] = new Rule(-281, new int[]{-30});
    rules[269] = new Rule(-281, new int[]{-263});
    rules[270] = new Rule(-281, new int[]{-129});
    rules[271] = new Rule(-281, new int[]{-130});
    rules[272] = new Rule(-130, new int[]{73,58,-276});
    rules[273] = new Rule(-278, new int[]{24,11,-163,12,58,-276});
    rules[274] = new Rule(-278, new int[]{-270});
    rules[275] = new Rule(-270, new int[]{24,58,-276});
    rules[276] = new Rule(-163, new int[]{-271});
    rules[277] = new Rule(-163, new int[]{-163,100,-271});
    rules[278] = new Rule(-271, new int[]{-272});
    rules[279] = new Rule(-271, new int[]{});
    rules[280] = new Rule(-263, new int[]{49,58,-276});
    rules[281] = new Rule(-129, new int[]{34,58,-276});
    rules[282] = new Rule(-129, new int[]{34});
    rules[283] = new Rule(-256, new int[]{143,11,-89,12});
    rules[284] = new Rule(-225, new int[]{-223});
    rules[285] = new Rule(-223, new int[]{-222});
    rules[286] = new Rule(-222, new int[]{44,-127});
    rules[287] = new Rule(-222, new int[]{37,-127,5,-275});
    rules[288] = new Rule(-222, new int[]{-179,127,-279});
    rules[289] = new Rule(-222, new int[]{-300,127,-279});
    rules[290] = new Rule(-222, new int[]{8,9,127,-279});
    rules[291] = new Rule(-222, new int[]{8,-79,9,127,-279});
    rules[292] = new Rule(-222, new int[]{-179,127,8,9});
    rules[293] = new Rule(-222, new int[]{-300,127,8,9});
    rules[294] = new Rule(-222, new int[]{8,9,127,8,9});
    rules[295] = new Rule(-222, new int[]{8,-79,9,127,8,9});
    rules[296] = new Rule(-29, new int[]{-20,-290,-182,-317,-25});
    rules[297] = new Rule(-30, new int[]{48,-182,-317,-24,92});
    rules[298] = new Rule(-19, new int[]{68});
    rules[299] = new Rule(-19, new int[]{69});
    rules[300] = new Rule(-19, new int[]{148});
    rules[301] = new Rule(-19, new int[]{27});
    rules[302] = new Rule(-19, new int[]{28});
    rules[303] = new Rule(-20, new int[]{});
    rules[304] = new Rule(-20, new int[]{-21});
    rules[305] = new Rule(-21, new int[]{-19});
    rules[306] = new Rule(-21, new int[]{-21,-19});
    rules[307] = new Rule(-290, new int[]{26});
    rules[308] = new Rule(-290, new int[]{43});
    rules[309] = new Rule(-290, new int[]{64});
    rules[310] = new Rule(-290, new int[]{64,26});
    rules[311] = new Rule(-290, new int[]{64,48});
    rules[312] = new Rule(-290, new int[]{64,43});
    rules[313] = new Rule(-25, new int[]{});
    rules[314] = new Rule(-25, new int[]{-24,92});
    rules[315] = new Rule(-182, new int[]{});
    rules[316] = new Rule(-182, new int[]{8,-181,9});
    rules[317] = new Rule(-181, new int[]{-180});
    rules[318] = new Rule(-181, new int[]{-181,100,-180});
    rules[319] = new Rule(-180, new int[]{-179});
    rules[320] = new Rule(-180, new int[]{-300});
    rules[321] = new Rule(-154, new int[]{123,-157,121});
    rules[322] = new Rule(-317, new int[]{});
    rules[323] = new Rule(-317, new int[]{-316});
    rules[324] = new Rule(-316, new int[]{-315});
    rules[325] = new Rule(-316, new int[]{-316,-315});
    rules[326] = new Rule(-315, new int[]{23,-157,5,-287,10});
    rules[327] = new Rule(-287, new int[]{-285});
    rules[328] = new Rule(-287, new int[]{-287,100,-285});
    rules[329] = new Rule(-285, new int[]{-276});
    rules[330] = new Rule(-285, new int[]{26});
    rules[331] = new Rule(-285, new int[]{48});
    rules[332] = new Rule(-285, new int[]{30});
    rules[333] = new Rule(-24, new int[]{-31});
    rules[334] = new Rule(-24, new int[]{-24,-7,-31});
    rules[335] = new Rule(-7, new int[]{84});
    rules[336] = new Rule(-7, new int[]{83});
    rules[337] = new Rule(-7, new int[]{82});
    rules[338] = new Rule(-7, new int[]{81});
    rules[339] = new Rule(-31, new int[]{});
    rules[340] = new Rule(-31, new int[]{-33,-189});
    rules[341] = new Rule(-31, new int[]{-32});
    rules[342] = new Rule(-31, new int[]{-33,10,-32});
    rules[343] = new Rule(-157, new int[]{-146});
    rules[344] = new Rule(-157, new int[]{-157,100,-146});
    rules[345] = new Rule(-189, new int[]{});
    rules[346] = new Rule(-189, new int[]{10});
    rules[347] = new Rule(-33, new int[]{-44});
    rules[348] = new Rule(-33, new int[]{-33,10,-44});
    rules[349] = new Rule(-44, new int[]{-6,-50});
    rules[350] = new Rule(-32, new int[]{-53});
    rules[351] = new Rule(-32, new int[]{-32,-53});
    rules[352] = new Rule(-53, new int[]{-52});
    rules[353] = new Rule(-53, new int[]{-54});
    rules[354] = new Rule(-50, new int[]{29,-27});
    rules[355] = new Rule(-50, new int[]{-312});
    rules[356] = new Rule(-50, new int[]{-3,-312});
    rules[357] = new Rule(-3, new int[]{28});
    rules[358] = new Rule(-3, new int[]{26});
    rules[359] = new Rule(-312, new int[]{-311});
    rules[360] = new Rule(-312, new int[]{62,-157,5,-276});
    rules[361] = new Rule(-52, new int[]{-6,-221});
    rules[362] = new Rule(-52, new int[]{-6,-218});
    rules[363] = new Rule(-218, new int[]{-214});
    rules[364] = new Rule(-218, new int[]{-217});
    rules[365] = new Rule(-221, new int[]{-3,-229});
    rules[366] = new Rule(-221, new int[]{-229});
    rules[367] = new Rule(-221, new int[]{19,-3,-229});
    rules[368] = new Rule(-221, new int[]{19,-229});
    rules[369] = new Rule(-221, new int[]{-226});
    rules[370] = new Rule(-229, new int[]{-227});
    rules[371] = new Rule(-227, new int[]{-224});
    rules[372] = new Rule(-227, new int[]{-228});
    rules[373] = new Rule(-226, new int[]{30,-171,-127,-206});
    rules[374] = new Rule(-226, new int[]{-3,30,-171,-127,-206});
    rules[375] = new Rule(-226, new int[]{31,-171,-127,-206});
    rules[376] = new Rule(-171, new int[]{-170});
    rules[377] = new Rule(-171, new int[]{});
    rules[378] = new Rule(-54, new int[]{-6,-258});
    rules[379] = new Rule(-258, new int[]{46,-169,-232,-201,10,-204});
    rules[380] = new Rule(-258, new int[]{46,-169,-232,-201,10,-209,10,-204});
    rules[381] = new Rule(-258, new int[]{-3,46,-169,-232,-201,10,-204});
    rules[382] = new Rule(-258, new int[]{-3,46,-169,-232,-201,10,-209,10,-204});
    rules[383] = new Rule(-258, new int[]{27,46,-169,-232,-210,10});
    rules[384] = new Rule(-258, new int[]{-3,27,46,-169,-232,-210,10});
    rules[385] = new Rule(-210, new int[]{110,-86});
    rules[386] = new Rule(-210, new int[]{});
    rules[387] = new Rule(-204, new int[]{});
    rules[388] = new Rule(-204, new int[]{63,10});
    rules[389] = new Rule(-232, new int[]{-237,5,-275});
    rules[390] = new Rule(-237, new int[]{});
    rules[391] = new Rule(-237, new int[]{11,-236,12});
    rules[392] = new Rule(-236, new int[]{-235});
    rules[393] = new Rule(-236, new int[]{-236,10,-235});
    rules[394] = new Rule(-235, new int[]{-157,5,-275});
    rules[395] = new Rule(-113, new int[]{-87});
    rules[396] = new Rule(-113, new int[]{});
    rules[397] = new Rule(-201, new int[]{});
    rules[398] = new Rule(-201, new int[]{85,-113,-202});
    rules[399] = new Rule(-201, new int[]{86,-260,-203});
    rules[400] = new Rule(-202, new int[]{});
    rules[401] = new Rule(-202, new int[]{86,-260});
    rules[402] = new Rule(-203, new int[]{});
    rules[403] = new Rule(-203, new int[]{85,-113});
    rules[404] = new Rule(-310, new int[]{-311,10});
    rules[405] = new Rule(-338, new int[]{110});
    rules[406] = new Rule(-338, new int[]{120});
    rules[407] = new Rule(-311, new int[]{-157,5,-276});
    rules[408] = new Rule(-311, new int[]{-157,110,-87});
    rules[409] = new Rule(-311, new int[]{-157,5,-276,-338,-85});
    rules[410] = new Rule(-85, new int[]{-84});
    rules[411] = new Rule(-85, new int[]{-80,6,-13});
    rules[412] = new Rule(-85, new int[]{-323});
    rules[413] = new Rule(-85, new int[]{-146,127,-328});
    rules[414] = new Rule(-85, new int[]{8,9,-324,127,-328});
    rules[415] = new Rule(-85, new int[]{8,-65,9,127,-328});
    rules[416] = new Rule(-85, new int[]{-242});
    rules[417] = new Rule(-84, new int[]{-83});
    rules[418] = new Rule(-216, new int[]{-226,-176});
    rules[419] = new Rule(-216, new int[]{30,-171,-127,110,-260,10});
    rules[420] = new Rule(-216, new int[]{-3,30,-171,-127,110,-260,10});
    rules[421] = new Rule(-217, new int[]{-226,-175});
    rules[422] = new Rule(-217, new int[]{30,-171,-127,110,-260,10});
    rules[423] = new Rule(-217, new int[]{-3,30,-171,-127,110,-260,10});
    rules[424] = new Rule(-213, new int[]{-220});
    rules[425] = new Rule(-213, new int[]{-3,-220});
    rules[426] = new Rule(-213, new int[]{19,-220});
    rules[427] = new Rule(-213, new int[]{19,-3,-220});
    rules[428] = new Rule(-213, new int[]{-3,19,-220});
    rules[429] = new Rule(-220, new int[]{-227,-177});
    rules[430] = new Rule(-220, new int[]{37,-169,-127,5,-275,-207,110,-100,10});
    rules[431] = new Rule(-220, new int[]{37,-169,-127,-207,110,-100,10});
    rules[432] = new Rule(-220, new int[]{37,-169,-127,5,-275,-207,110,-322,10});
    rules[433] = new Rule(-220, new int[]{37,-169,-127,-207,110,-322,10});
    rules[434] = new Rule(-220, new int[]{44,-170,-127,-207,110,-260,10});
    rules[435] = new Rule(-220, new int[]{-227,149,10});
    rules[436] = new Rule(-214, new int[]{-215});
    rules[437] = new Rule(-214, new int[]{19,-215});
    rules[438] = new Rule(-214, new int[]{-3,-215});
    rules[439] = new Rule(-214, new int[]{19,-3,-215});
    rules[440] = new Rule(-214, new int[]{-3,19,-215});
    rules[441] = new Rule(-215, new int[]{-227,-175});
    rules[442] = new Rule(-215, new int[]{37,-169,-127,5,-275,-207,110,-102,10});
    rules[443] = new Rule(-215, new int[]{37,-169,-127,-207,110,-102,10});
    rules[444] = new Rule(-215, new int[]{44,-170,-127,-207,110,-260,10});
    rules[445] = new Rule(-177, new int[]{-176});
    rules[446] = new Rule(-177, new int[]{-60});
    rules[447] = new Rule(-170, new int[]{-169});
    rules[448] = new Rule(-169, new int[]{-141});
    rules[449] = new Rule(-169, new int[]{-334,7,-141});
    rules[450] = new Rule(-148, new int[]{-136});
    rules[451] = new Rule(-334, new int[]{-148});
    rules[452] = new Rule(-334, new int[]{-334,7,-148});
    rules[453] = new Rule(-141, new int[]{-136});
    rules[454] = new Rule(-141, new int[]{-190});
    rules[455] = new Rule(-141, new int[]{-190,-154});
    rules[456] = new Rule(-136, new int[]{-133});
    rules[457] = new Rule(-136, new int[]{-133,-154});
    rules[458] = new Rule(-133, new int[]{-146});
    rules[459] = new Rule(-224, new int[]{44,-170,-127,-206,-317});
    rules[460] = new Rule(-228, new int[]{37,-169,-127,-206,-317});
    rules[461] = new Rule(-228, new int[]{37,-169,-127,5,-275,-206,-317});
    rules[462] = new Rule(-60, new int[]{107,-107,80,-107,10});
    rules[463] = new Rule(-60, new int[]{107,-107,10});
    rules[464] = new Rule(-60, new int[]{107,10});
    rules[465] = new Rule(-107, new int[]{-146});
    rules[466] = new Rule(-107, new int[]{-164});
    rules[467] = new Rule(-176, new int[]{-40,-255,10});
    rules[468] = new Rule(-175, new int[]{-42,-255,10});
    rules[469] = new Rule(-175, new int[]{-60});
    rules[470] = new Rule(-127, new int[]{});
    rules[471] = new Rule(-127, new int[]{8,9});
    rules[472] = new Rule(-127, new int[]{8,-128,9});
    rules[473] = new Rule(-128, new int[]{-55});
    rules[474] = new Rule(-128, new int[]{-128,10,-55});
    rules[475] = new Rule(-55, new int[]{-6,-295});
    rules[476] = new Rule(-295, new int[]{-158,5,-275});
    rules[477] = new Rule(-295, new int[]{53,-158,5,-275});
    rules[478] = new Rule(-295, new int[]{29,-158,5,-275});
    rules[479] = new Rule(-295, new int[]{108,-158,5,-275});
    rules[480] = new Rule(-295, new int[]{-158,5,-275,110,-86});
    rules[481] = new Rule(-295, new int[]{53,-158,5,-275,110,-86});
    rules[482] = new Rule(-295, new int[]{29,-158,5,-275,110,-86});
    rules[483] = new Rule(-158, new int[]{-134});
    rules[484] = new Rule(-158, new int[]{-158,100,-134});
    rules[485] = new Rule(-134, new int[]{-146});
    rules[486] = new Rule(-275, new int[]{-276});
    rules[487] = new Rule(-277, new int[]{-272});
    rules[488] = new Rule(-277, new int[]{-256});
    rules[489] = new Rule(-277, new int[]{-249});
    rules[490] = new Rule(-277, new int[]{-281});
    rules[491] = new Rule(-277, new int[]{-300});
    rules[492] = new Rule(-261, new int[]{-260});
    rules[493] = new Rule(-261, new int[]{-142,5,-261});
    rules[494] = new Rule(-260, new int[]{});
    rules[495] = new Rule(-260, new int[]{-4});
    rules[496] = new Rule(-260, new int[]{-211});
    rules[497] = new Rule(-260, new int[]{-132});
    rules[498] = new Rule(-260, new int[]{-255});
    rules[499] = new Rule(-260, new int[]{-152});
    rules[500] = new Rule(-260, new int[]{-34});
    rules[501] = new Rule(-260, new int[]{-247});
    rules[502] = new Rule(-260, new int[]{-318});
    rules[503] = new Rule(-260, new int[]{-123});
    rules[504] = new Rule(-260, new int[]{-319});
    rules[505] = new Rule(-260, new int[]{-159});
    rules[506] = new Rule(-260, new int[]{-301});
    rules[507] = new Rule(-260, new int[]{-248});
    rules[508] = new Rule(-260, new int[]{-122});
    rules[509] = new Rule(-260, new int[]{-314});
    rules[510] = new Rule(-260, new int[]{-58});
    rules[511] = new Rule(-260, new int[]{-168});
    rules[512] = new Rule(-260, new int[]{-125});
    rules[513] = new Rule(-260, new int[]{-126});
    rules[514] = new Rule(-260, new int[]{-124});
    rules[515] = new Rule(-260, new int[]{-348});
    rules[516] = new Rule(-124, new int[]{72,-100,99,-260});
    rules[517] = new Rule(-125, new int[]{74,-102});
    rules[518] = new Rule(-126, new int[]{74,73,-102});
    rules[519] = new Rule(-314, new int[]{53,-311});
    rules[520] = new Rule(-314, new int[]{8,53,-146,100,-337,9,110,-86});
    rules[521] = new Rule(-314, new int[]{53,8,-146,100,-157,9,110,-86});
    rules[522] = new Rule(-4, new int[]{-112,-193,-87});
    rules[523] = new Rule(-4, new int[]{8,-110,100,-336,9,-193,-86});
    rules[524] = new Rule(-336, new int[]{-110});
    rules[525] = new Rule(-336, new int[]{-336,100,-110});
    rules[526] = new Rule(-337, new int[]{53,-146});
    rules[527] = new Rule(-337, new int[]{-337,100,53,-146});
    rules[528] = new Rule(-211, new int[]{-112});
    rules[529] = new Rule(-132, new int[]{57,-142});
    rules[530] = new Rule(-255, new int[]{91,-252,92});
    rules[531] = new Rule(-252, new int[]{-261});
    rules[532] = new Rule(-252, new int[]{-252,10,-261});
    rules[533] = new Rule(-152, new int[]{40,-100,51,-260});
    rules[534] = new Rule(-152, new int[]{40,-100,51,-260,32,-260});
    rules[535] = new Rule(-348, new int[]{38,-100,55,-350,-253,92});
    rules[536] = new Rule(-348, new int[]{38,-100,55,-350,10,-253,92});
    rules[537] = new Rule(-350, new int[]{-349});
    rules[538] = new Rule(-350, new int[]{-350,10,-349});
    rules[539] = new Rule(-349, new int[]{-342,39,-100,5,-260});
    rules[540] = new Rule(-349, new int[]{-341,5,-260});
    rules[541] = new Rule(-349, new int[]{-343,5,-260});
    rules[542] = new Rule(-349, new int[]{-344,39,-100,5,-260});
    rules[543] = new Rule(-349, new int[]{-344,5,-260});
    rules[544] = new Rule(-34, new int[]{25,-100,58,-35,-253,92});
    rules[545] = new Rule(-34, new int[]{25,-100,58,-35,10,-253,92});
    rules[546] = new Rule(-34, new int[]{25,-100,58,-253,92});
    rules[547] = new Rule(-35, new int[]{-262});
    rules[548] = new Rule(-35, new int[]{-35,10,-262});
    rules[549] = new Rule(-262, new int[]{-73,5,-260});
    rules[550] = new Rule(-73, new int[]{-109});
    rules[551] = new Rule(-73, new int[]{-73,100,-109});
    rules[552] = new Rule(-109, new int[]{-93});
    rules[553] = new Rule(-253, new int[]{});
    rules[554] = new Rule(-253, new int[]{32,-252});
    rules[555] = new Rule(-247, new int[]{97,-252,98,-86});
    rules[556] = new Rule(-318, new int[]{54,-100,-291,-260});
    rules[557] = new Rule(-291, new int[]{99});
    rules[558] = new Rule(-291, new int[]{});
    rules[559] = new Rule(-168, new int[]{60,-100,99,-260});
    rules[560] = new Rule(-360, new int[]{87,143});
    rules[561] = new Rule(-360, new int[]{});
    rules[562] = new Rule(-274, new int[]{5,-276});
    rules[563] = new Rule(-274, new int[]{});
    rules[564] = new Rule(-18, new int[]{53});
    rules[565] = new Rule(-18, new int[]{});
    rules[566] = new Rule(-118, new int[]{70});
    rules[567] = new Rule(-118, new int[]{71});
    rules[568] = new Rule(-122, new int[]{36,-146,-274,137,-100,-360,99,-260});
    rules[569] = new Rule(-122, new int[]{36,53,-146,-274,137,-100,-360,99,-260});
    rules[570] = new Rule(-122, new int[]{36,53,8,-157,9,137,-100,-360,99,-260});
    rules[571] = new Rule(-123, new int[]{35,-18,-146,-274,110,-100,-118,-100,-291,-260});
    rules[572] = new Rule(-123, new int[]{35,-18,-146,-274,110,-100,-118,-100,160,-100,99,-260});
    rules[573] = new Rule(-319, new int[]{55,-70,99,-260});
    rules[574] = new Rule(-159, new int[]{42});
    rules[575] = new Rule(-301, new int[]{102,-252,-289});
    rules[576] = new Rule(-289, new int[]{101,-252,92});
    rules[577] = new Rule(-289, new int[]{33,-59,92});
    rules[578] = new Rule(-59, new int[]{-62,-254});
    rules[579] = new Rule(-59, new int[]{-62,10,-254});
    rules[580] = new Rule(-59, new int[]{-252});
    rules[581] = new Rule(-62, new int[]{-61});
    rules[582] = new Rule(-62, new int[]{-62,10,-61});
    rules[583] = new Rule(-254, new int[]{});
    rules[584] = new Rule(-254, new int[]{32,-252});
    rules[585] = new Rule(-61, new int[]{79,-63,99,-260});
    rules[586] = new Rule(-63, new int[]{-178});
    rules[587] = new Rule(-63, new int[]{-139,5,-178});
    rules[588] = new Rule(-178, new int[]{-179});
    rules[589] = new Rule(-139, new int[]{-146});
    rules[590] = new Rule(-248, new int[]{47});
    rules[591] = new Rule(-248, new int[]{47,-86});
    rules[592] = new Rule(-70, new int[]{-87});
    rules[593] = new Rule(-70, new int[]{-70,100,-87});
    rules[594] = new Rule(-71, new int[]{-88});
    rules[595] = new Rule(-71, new int[]{-71,100,-88});
    rules[596] = new Rule(-58, new int[]{-173});
    rules[597] = new Rule(-173, new int[]{-172});
    rules[598] = new Rule(-87, new int[]{-86});
    rules[599] = new Rule(-87, new int[]{-322});
    rules[600] = new Rule(-87, new int[]{42});
    rules[601] = new Rule(-88, new int[]{-86});
    rules[602] = new Rule(-88, new int[]{-146,110,-100});
    rules[603] = new Rule(-88, new int[]{-322});
    rules[604] = new Rule(-88, new int[]{42});
    rules[605] = new Rule(-86, new int[]{-100});
    rules[606] = new Rule(-86, new int[]{-119});
    rules[607] = new Rule(-86, new int[]{-86,70,-100});
    rules[608] = new Rule(-100, new int[]{-98});
    rules[609] = new Rule(-100, new int[]{-240});
    rules[610] = new Rule(-100, new int[]{-242});
    rules[611] = new Rule(-116, new int[]{-98});
    rules[612] = new Rule(-116, new int[]{-240});
    rules[613] = new Rule(-117, new int[]{-98});
    rules[614] = new Rule(-117, new int[]{-242});
    rules[615] = new Rule(-102, new int[]{-100});
    rules[616] = new Rule(-102, new int[]{-322});
    rules[617] = new Rule(-103, new int[]{-98});
    rules[618] = new Rule(-103, new int[]{-240});
    rules[619] = new Rule(-103, new int[]{-322});
    rules[620] = new Rule(-98, new int[]{-97});
    rules[621] = new Rule(-98, new int[]{-98,16,-97});
    rules[622] = new Rule(-257, new int[]{21,8,-283,9});
    rules[623] = new Rule(-294, new int[]{22,8,-283,9});
    rules[624] = new Rule(-294, new int[]{22,8,-282,9});
    rules[625] = new Rule(-240, new int[]{-116,13,-116,5,-116});
    rules[626] = new Rule(-242, new int[]{40,-117,51,-117,32,-117});
    rules[627] = new Rule(-282, new int[]{-179,-299});
    rules[628] = new Rule(-282, new int[]{-179,4,-299});
    rules[629] = new Rule(-283, new int[]{-179});
    rules[630] = new Rule(-283, new int[]{-179,-298});
    rules[631] = new Rule(-283, new int[]{-179,4,-298});
    rules[632] = new Rule(-284, new int[]{-283});
    rules[633] = new Rule(-284, new int[]{-273});
    rules[634] = new Rule(-5, new int[]{8,-65,9});
    rules[635] = new Rule(-5, new int[]{});
    rules[636] = new Rule(-172, new int[]{78,-283,-69});
    rules[637] = new Rule(-172, new int[]{78,-283,11,-66,12,-5});
    rules[638] = new Rule(-172, new int[]{78,26,8,-333,9});
    rules[639] = new Rule(-332, new int[]{-146,110,-100});
    rules[640] = new Rule(-332, new int[]{-100});
    rules[641] = new Rule(-333, new int[]{-332});
    rules[642] = new Rule(-333, new int[]{-333,100,-332});
    rules[643] = new Rule(-69, new int[]{});
    rules[644] = new Rule(-69, new int[]{8,-66,9});
    rules[645] = new Rule(-97, new int[]{-104});
    rules[646] = new Rule(-97, new int[]{-97,-195,-104});
    rules[647] = new Rule(-97, new int[]{-97,-195,-242});
    rules[648] = new Rule(-97, new int[]{-266,8,-353,9});
    rules[649] = new Rule(-340, new int[]{-283,8,-353,9});
    rules[650] = new Rule(-342, new int[]{-283,8,-354,9});
    rules[651] = new Rule(-341, new int[]{-283,8,-354,9});
    rules[652] = new Rule(-341, new int[]{-357});
    rules[653] = new Rule(-357, new int[]{-339});
    rules[654] = new Rule(-357, new int[]{-357,100,-339});
    rules[655] = new Rule(-339, new int[]{-15});
    rules[656] = new Rule(-339, new int[]{-283});
    rules[657] = new Rule(-339, new int[]{56});
    rules[658] = new Rule(-339, new int[]{-257});
    rules[659] = new Rule(-339, new int[]{-294});
    rules[660] = new Rule(-343, new int[]{11,-355,12});
    rules[661] = new Rule(-355, new int[]{-345});
    rules[662] = new Rule(-355, new int[]{-355,100,-345});
    rules[663] = new Rule(-345, new int[]{-15});
    rules[664] = new Rule(-345, new int[]{-347});
    rules[665] = new Rule(-345, new int[]{14});
    rules[666] = new Rule(-345, new int[]{-342});
    rules[667] = new Rule(-345, new int[]{-343});
    rules[668] = new Rule(-345, new int[]{-344});
    rules[669] = new Rule(-345, new int[]{6});
    rules[670] = new Rule(-347, new int[]{53,-146});
    rules[671] = new Rule(-344, new int[]{8,-356,9});
    rules[672] = new Rule(-346, new int[]{14});
    rules[673] = new Rule(-346, new int[]{-15});
    rules[674] = new Rule(-346, new int[]{-198,-15});
    rules[675] = new Rule(-346, new int[]{53,-146});
    rules[676] = new Rule(-346, new int[]{-342});
    rules[677] = new Rule(-346, new int[]{-343});
    rules[678] = new Rule(-346, new int[]{-344});
    rules[679] = new Rule(-356, new int[]{-346});
    rules[680] = new Rule(-356, new int[]{-356,100,-346});
    rules[681] = new Rule(-354, new int[]{-352});
    rules[682] = new Rule(-354, new int[]{-354,10,-352});
    rules[683] = new Rule(-354, new int[]{-354,100,-352});
    rules[684] = new Rule(-353, new int[]{-351});
    rules[685] = new Rule(-353, new int[]{-353,10,-351});
    rules[686] = new Rule(-353, new int[]{-353,100,-351});
    rules[687] = new Rule(-351, new int[]{14});
    rules[688] = new Rule(-351, new int[]{-15});
    rules[689] = new Rule(-351, new int[]{53,-146,5,-276});
    rules[690] = new Rule(-351, new int[]{53,-146});
    rules[691] = new Rule(-351, new int[]{-340});
    rules[692] = new Rule(-351, new int[]{-343});
    rules[693] = new Rule(-351, new int[]{-344});
    rules[694] = new Rule(-352, new int[]{14});
    rules[695] = new Rule(-352, new int[]{-15});
    rules[696] = new Rule(-352, new int[]{-198,-15});
    rules[697] = new Rule(-352, new int[]{-146,5,-276});
    rules[698] = new Rule(-352, new int[]{-146});
    rules[699] = new Rule(-352, new int[]{53,-146,5,-276});
    rules[700] = new Rule(-352, new int[]{53,-146});
    rules[701] = new Rule(-352, new int[]{-342});
    rules[702] = new Rule(-352, new int[]{-343});
    rules[703] = new Rule(-352, new int[]{-344});
    rules[704] = new Rule(-114, new int[]{-104});
    rules[705] = new Rule(-114, new int[]{});
    rules[706] = new Rule(-121, new int[]{-89});
    rules[707] = new Rule(-121, new int[]{});
    rules[708] = new Rule(-119, new int[]{-104,5,-114});
    rules[709] = new Rule(-119, new int[]{5,-114});
    rules[710] = new Rule(-119, new int[]{-104,5,-114,5,-104});
    rules[711] = new Rule(-119, new int[]{5,-114,5,-104});
    rules[712] = new Rule(-120, new int[]{-89,5,-121});
    rules[713] = new Rule(-120, new int[]{5,-121});
    rules[714] = new Rule(-120, new int[]{-89,5,-121,5,-89});
    rules[715] = new Rule(-120, new int[]{5,-121,5,-89});
    rules[716] = new Rule(-195, new int[]{120});
    rules[717] = new Rule(-195, new int[]{125});
    rules[718] = new Rule(-195, new int[]{123});
    rules[719] = new Rule(-195, new int[]{121});
    rules[720] = new Rule(-195, new int[]{124});
    rules[721] = new Rule(-195, new int[]{122});
    rules[722] = new Rule(-195, new int[]{137});
    rules[723] = new Rule(-195, new int[]{135,137});
    rules[724] = new Rule(-104, new int[]{-82});
    rules[725] = new Rule(-104, new int[]{-104,6,-82});
    rules[726] = new Rule(-82, new int[]{-81});
    rules[727] = new Rule(-82, new int[]{-82,-196,-81});
    rules[728] = new Rule(-82, new int[]{-82,-196,-242});
    rules[729] = new Rule(-196, new int[]{116});
    rules[730] = new Rule(-196, new int[]{115});
    rules[731] = new Rule(-196, new int[]{128});
    rules[732] = new Rule(-196, new int[]{129});
    rules[733] = new Rule(-196, new int[]{126});
    rules[734] = new Rule(-200, new int[]{136});
    rules[735] = new Rule(-200, new int[]{138});
    rules[736] = new Rule(-264, new int[]{-266});
    rules[737] = new Rule(-264, new int[]{-267});
    rules[738] = new Rule(-267, new int[]{-81,136,-283});
    rules[739] = new Rule(-267, new int[]{-81,136,-278});
    rules[740] = new Rule(-266, new int[]{-81,138,-283});
    rules[741] = new Rule(-266, new int[]{-81,138,-278});
    rules[742] = new Rule(-268, new int[]{-96,119,-95});
    rules[743] = new Rule(-268, new int[]{-96,119,-268});
    rules[744] = new Rule(-268, new int[]{-198,-268});
    rules[745] = new Rule(-81, new int[]{-95});
    rules[746] = new Rule(-81, new int[]{-172});
    rules[747] = new Rule(-81, new int[]{-268});
    rules[748] = new Rule(-81, new int[]{-81,-197,-95});
    rules[749] = new Rule(-81, new int[]{-81,-197,-268});
    rules[750] = new Rule(-81, new int[]{-81,-197,-242});
    rules[751] = new Rule(-81, new int[]{-264});
    rules[752] = new Rule(-197, new int[]{118});
    rules[753] = new Rule(-197, new int[]{117});
    rules[754] = new Rule(-197, new int[]{131});
    rules[755] = new Rule(-197, new int[]{132});
    rules[756] = new Rule(-197, new int[]{133});
    rules[757] = new Rule(-197, new int[]{134});
    rules[758] = new Rule(-197, new int[]{130});
    rules[759] = new Rule(-56, new int[]{63,8,-284,9});
    rules[760] = new Rule(-57, new int[]{8,-101,100,-78,-324,-331,9});
    rules[761] = new Rule(-96, new int[]{-15});
    rules[762] = new Rule(-96, new int[]{-112});
    rules[763] = new Rule(-95, new int[]{56});
    rules[764] = new Rule(-95, new int[]{-15});
    rules[765] = new Rule(-95, new int[]{-56});
    rules[766] = new Rule(-95, new int[]{135,-95});
    rules[767] = new Rule(-95, new int[]{-198,-95});
    rules[768] = new Rule(-95, new int[]{142,-95});
    rules[769] = new Rule(-95, new int[]{-112});
    rules[770] = new Rule(-95, new int[]{-57});
    rules[771] = new Rule(-15, new int[]{-164});
    rules[772] = new Rule(-15, new int[]{-16});
    rules[773] = new Rule(-115, new int[]{-110,15,-110});
    rules[774] = new Rule(-115, new int[]{-110,15,-115});
    rules[775] = new Rule(-112, new int[]{-131,-110});
    rules[776] = new Rule(-112, new int[]{-110});
    rules[777] = new Rule(-112, new int[]{-115});
    rules[778] = new Rule(-112, new int[]{8,53,-146,110,-98,9});
    rules[779] = new Rule(-131, new int[]{141});
    rules[780] = new Rule(-131, new int[]{-131,141});
    rules[781] = new Rule(-9, new int[]{-179,-69});
    rules[782] = new Rule(-9, new int[]{-300,-69});
    rules[783] = new Rule(-321, new int[]{-146});
    rules[784] = new Rule(-321, new int[]{-321,7,-137});
    rules[785] = new Rule(-320, new int[]{-321});
    rules[786] = new Rule(-320, new int[]{-321,-298});
    rules[787] = new Rule(-358, new int[]{53,-146,110,-86,10});
    rules[788] = new Rule(-359, new int[]{-358});
    rules[789] = new Rule(-359, new int[]{-359,-358});
    rules[790] = new Rule(-111, new int[]{-110,8,-67,9});
    rules[791] = new Rule(-110, new int[]{-146});
    rules[792] = new Rule(-110, new int[]{-190});
    rules[793] = new Rule(-110, new int[]{42,-146});
    rules[794] = new Rule(-110, new int[]{8,-86,9});
    rules[795] = new Rule(-110, new int[]{8,-359,-86,9});
    rules[796] = new Rule(-110, new int[]{-257});
    rules[797] = new Rule(-110, new int[]{-294});
    rules[798] = new Rule(-110, new int[]{-15,7,-137});
    rules[799] = new Rule(-110, new int[]{-110,11,-70,12});
    rules[800] = new Rule(-110, new int[]{-15,11,-70,12});
    rules[801] = new Rule(-110, new int[]{-110,17,-119,12});
    rules[802] = new Rule(-110, new int[]{-15,17,-119,12});
    rules[803] = new Rule(-110, new int[]{76,-68,76});
    rules[804] = new Rule(-110, new int[]{-239});
    rules[805] = new Rule(-110, new int[]{-111});
    rules[806] = new Rule(-110, new int[]{-110,7,-147});
    rules[807] = new Rule(-110, new int[]{-57,7,-147});
    rules[808] = new Rule(-110, new int[]{-110,142});
    rules[809] = new Rule(-110, new int[]{-110,4,-298});
    rules[810] = new Rule(-66, new int[]{-70});
    rules[811] = new Rule(-66, new int[]{});
    rules[812] = new Rule(-67, new int[]{-71});
    rules[813] = new Rule(-67, new int[]{});
    rules[814] = new Rule(-68, new int[]{-76});
    rules[815] = new Rule(-68, new int[]{});
    rules[816] = new Rule(-76, new int[]{-91});
    rules[817] = new Rule(-76, new int[]{-76,100,-91});
    rules[818] = new Rule(-91, new int[]{-86});
    rules[819] = new Rule(-91, new int[]{-86,6,-86});
    rules[820] = new Rule(-165, new int[]{144});
    rules[821] = new Rule(-165, new int[]{147});
    rules[822] = new Rule(-164, new int[]{-166});
    rules[823] = new Rule(-164, new int[]{145});
    rules[824] = new Rule(-164, new int[]{146});
    rules[825] = new Rule(-166, new int[]{-165});
    rules[826] = new Rule(-166, new int[]{-166,-165});
    rules[827] = new Rule(-190, new int[]{45,-199});
    rules[828] = new Rule(-206, new int[]{10});
    rules[829] = new Rule(-206, new int[]{10,-205,10});
    rules[830] = new Rule(-207, new int[]{});
    rules[831] = new Rule(-207, new int[]{10,-205});
    rules[832] = new Rule(-205, new int[]{-208});
    rules[833] = new Rule(-205, new int[]{-205,10,-208});
    rules[834] = new Rule(-146, new int[]{143});
    rules[835] = new Rule(-146, new int[]{-150});
    rules[836] = new Rule(-146, new int[]{-151});
    rules[837] = new Rule(-146, new int[]{160});
    rules[838] = new Rule(-146, new int[]{87});
    rules[839] = new Rule(-137, new int[]{-146});
    rules[840] = new Rule(-137, new int[]{-292});
    rules[841] = new Rule(-137, new int[]{-293});
    rules[842] = new Rule(-147, new int[]{-146});
    rules[843] = new Rule(-147, new int[]{-292});
    rules[844] = new Rule(-147, new int[]{-190});
    rules[845] = new Rule(-208, new int[]{148});
    rules[846] = new Rule(-208, new int[]{150});
    rules[847] = new Rule(-208, new int[]{151});
    rules[848] = new Rule(-208, new int[]{152});
    rules[849] = new Rule(-208, new int[]{154});
    rules[850] = new Rule(-208, new int[]{153});
    rules[851] = new Rule(-209, new int[]{153});
    rules[852] = new Rule(-209, new int[]{152});
    rules[853] = new Rule(-209, new int[]{148});
    rules[854] = new Rule(-209, new int[]{151});
    rules[855] = new Rule(-150, new int[]{85});
    rules[856] = new Rule(-150, new int[]{86});
    rules[857] = new Rule(-151, new int[]{80});
    rules[858] = new Rule(-151, new int[]{78});
    rules[859] = new Rule(-149, new int[]{84});
    rules[860] = new Rule(-149, new int[]{83});
    rules[861] = new Rule(-149, new int[]{82});
    rules[862] = new Rule(-149, new int[]{81});
    rules[863] = new Rule(-292, new int[]{-149});
    rules[864] = new Rule(-292, new int[]{68});
    rules[865] = new Rule(-292, new int[]{64});
    rules[866] = new Rule(-292, new int[]{128});
    rules[867] = new Rule(-292, new int[]{22});
    rules[868] = new Rule(-292, new int[]{21});
    rules[869] = new Rule(-292, new int[]{63});
    rules[870] = new Rule(-292, new int[]{23});
    rules[871] = new Rule(-292, new int[]{129});
    rules[872] = new Rule(-292, new int[]{130});
    rules[873] = new Rule(-292, new int[]{131});
    rules[874] = new Rule(-292, new int[]{132});
    rules[875] = new Rule(-292, new int[]{133});
    rules[876] = new Rule(-292, new int[]{134});
    rules[877] = new Rule(-292, new int[]{135});
    rules[878] = new Rule(-292, new int[]{136});
    rules[879] = new Rule(-292, new int[]{137});
    rules[880] = new Rule(-292, new int[]{138});
    rules[881] = new Rule(-292, new int[]{24});
    rules[882] = new Rule(-292, new int[]{73});
    rules[883] = new Rule(-292, new int[]{91});
    rules[884] = new Rule(-292, new int[]{25});
    rules[885] = new Rule(-292, new int[]{26});
    rules[886] = new Rule(-292, new int[]{29});
    rules[887] = new Rule(-292, new int[]{30});
    rules[888] = new Rule(-292, new int[]{31});
    rules[889] = new Rule(-292, new int[]{71});
    rules[890] = new Rule(-292, new int[]{99});
    rules[891] = new Rule(-292, new int[]{32});
    rules[892] = new Rule(-292, new int[]{92});
    rules[893] = new Rule(-292, new int[]{33});
    rules[894] = new Rule(-292, new int[]{34});
    rules[895] = new Rule(-292, new int[]{27});
    rules[896] = new Rule(-292, new int[]{104});
    rules[897] = new Rule(-292, new int[]{101});
    rules[898] = new Rule(-292, new int[]{35});
    rules[899] = new Rule(-292, new int[]{36});
    rules[900] = new Rule(-292, new int[]{37});
    rules[901] = new Rule(-292, new int[]{40});
    rules[902] = new Rule(-292, new int[]{41});
    rules[903] = new Rule(-292, new int[]{42});
    rules[904] = new Rule(-292, new int[]{103});
    rules[905] = new Rule(-292, new int[]{43});
    rules[906] = new Rule(-292, new int[]{44});
    rules[907] = new Rule(-292, new int[]{46});
    rules[908] = new Rule(-292, new int[]{47});
    rules[909] = new Rule(-292, new int[]{48});
    rules[910] = new Rule(-292, new int[]{97});
    rules[911] = new Rule(-292, new int[]{49});
    rules[912] = new Rule(-292, new int[]{102});
    rules[913] = new Rule(-292, new int[]{50});
    rules[914] = new Rule(-292, new int[]{28});
    rules[915] = new Rule(-292, new int[]{51});
    rules[916] = new Rule(-292, new int[]{70});
    rules[917] = new Rule(-292, new int[]{98});
    rules[918] = new Rule(-292, new int[]{52});
    rules[919] = new Rule(-292, new int[]{53});
    rules[920] = new Rule(-292, new int[]{54});
    rules[921] = new Rule(-292, new int[]{55});
    rules[922] = new Rule(-292, new int[]{56});
    rules[923] = new Rule(-292, new int[]{57});
    rules[924] = new Rule(-292, new int[]{58});
    rules[925] = new Rule(-292, new int[]{59});
    rules[926] = new Rule(-292, new int[]{61});
    rules[927] = new Rule(-292, new int[]{105});
    rules[928] = new Rule(-292, new int[]{106});
    rules[929] = new Rule(-292, new int[]{109});
    rules[930] = new Rule(-292, new int[]{107});
    rules[931] = new Rule(-292, new int[]{108});
    rules[932] = new Rule(-292, new int[]{62});
    rules[933] = new Rule(-292, new int[]{74});
    rules[934] = new Rule(-292, new int[]{38});
    rules[935] = new Rule(-292, new int[]{39});
    rules[936] = new Rule(-292, new int[]{69});
    rules[937] = new Rule(-292, new int[]{148});
    rules[938] = new Rule(-292, new int[]{60});
    rules[939] = new Rule(-292, new int[]{139});
    rules[940] = new Rule(-292, new int[]{140});
    rules[941] = new Rule(-292, new int[]{79});
    rules[942] = new Rule(-292, new int[]{153});
    rules[943] = new Rule(-292, new int[]{152});
    rules[944] = new Rule(-292, new int[]{72});
    rules[945] = new Rule(-292, new int[]{154});
    rules[946] = new Rule(-292, new int[]{150});
    rules[947] = new Rule(-292, new int[]{151});
    rules[948] = new Rule(-292, new int[]{149});
    rules[949] = new Rule(-293, new int[]{45});
    rules[950] = new Rule(-199, new int[]{115});
    rules[951] = new Rule(-199, new int[]{116});
    rules[952] = new Rule(-199, new int[]{117});
    rules[953] = new Rule(-199, new int[]{118});
    rules[954] = new Rule(-199, new int[]{120});
    rules[955] = new Rule(-199, new int[]{121});
    rules[956] = new Rule(-199, new int[]{122});
    rules[957] = new Rule(-199, new int[]{123});
    rules[958] = new Rule(-199, new int[]{124});
    rules[959] = new Rule(-199, new int[]{125});
    rules[960] = new Rule(-199, new int[]{128});
    rules[961] = new Rule(-199, new int[]{129});
    rules[962] = new Rule(-199, new int[]{130});
    rules[963] = new Rule(-199, new int[]{131});
    rules[964] = new Rule(-199, new int[]{132});
    rules[965] = new Rule(-199, new int[]{133});
    rules[966] = new Rule(-199, new int[]{134});
    rules[967] = new Rule(-199, new int[]{135});
    rules[968] = new Rule(-199, new int[]{137});
    rules[969] = new Rule(-199, new int[]{139});
    rules[970] = new Rule(-199, new int[]{140});
    rules[971] = new Rule(-199, new int[]{-193});
    rules[972] = new Rule(-199, new int[]{119});
    rules[973] = new Rule(-193, new int[]{110});
    rules[974] = new Rule(-193, new int[]{111});
    rules[975] = new Rule(-193, new int[]{112});
    rules[976] = new Rule(-193, new int[]{113});
    rules[977] = new Rule(-193, new int[]{114});
    rules[978] = new Rule(-99, new int[]{18,-23,100,-22,9});
    rules[979] = new Rule(-22, new int[]{-99});
    rules[980] = new Rule(-22, new int[]{-146});
    rules[981] = new Rule(-23, new int[]{-22});
    rules[982] = new Rule(-23, new int[]{-23,100,-22});
    rules[983] = new Rule(-101, new int[]{-100});
    rules[984] = new Rule(-101, new int[]{-99});
    rules[985] = new Rule(-78, new int[]{-101});
    rules[986] = new Rule(-78, new int[]{-78,100,-101});
    rules[987] = new Rule(-322, new int[]{-146,127,-328});
    rules[988] = new Rule(-322, new int[]{8,9,-325,127,-328});
    rules[989] = new Rule(-322, new int[]{8,-146,5,-275,9,-325,127,-328});
    rules[990] = new Rule(-322, new int[]{8,-146,10,-326,9,-325,127,-328});
    rules[991] = new Rule(-322, new int[]{8,-146,5,-275,10,-326,9,-325,127,-328});
    rules[992] = new Rule(-322, new int[]{8,-101,100,-78,-324,-331,9,-335});
    rules[993] = new Rule(-322, new int[]{-99,-335});
    rules[994] = new Rule(-322, new int[]{-323});
    rules[995] = new Rule(-331, new int[]{});
    rules[996] = new Rule(-331, new int[]{10,-326});
    rules[997] = new Rule(-335, new int[]{-325,127,-328});
    rules[998] = new Rule(-323, new int[]{37,-325,127,-328});
    rules[999] = new Rule(-323, new int[]{37,8,9,-325,127,-328});
    rules[1000] = new Rule(-323, new int[]{37,8,-326,9,-325,127,-328});
    rules[1001] = new Rule(-323, new int[]{44,127,-329});
    rules[1002] = new Rule(-323, new int[]{44,8,9,127,-329});
    rules[1003] = new Rule(-323, new int[]{44,8,-326,9,127,-329});
    rules[1004] = new Rule(-326, new int[]{-327});
    rules[1005] = new Rule(-326, new int[]{-326,10,-327});
    rules[1006] = new Rule(-327, new int[]{-157,-324});
    rules[1007] = new Rule(-324, new int[]{});
    rules[1008] = new Rule(-324, new int[]{5,-275});
    rules[1009] = new Rule(-325, new int[]{});
    rules[1010] = new Rule(-325, new int[]{5,-277});
    rules[1011] = new Rule(-330, new int[]{-255});
    rules[1012] = new Rule(-330, new int[]{-152});
    rules[1013] = new Rule(-330, new int[]{-318});
    rules[1014] = new Rule(-330, new int[]{-247});
    rules[1015] = new Rule(-330, new int[]{-123});
    rules[1016] = new Rule(-330, new int[]{-122});
    rules[1017] = new Rule(-330, new int[]{-124});
    rules[1018] = new Rule(-330, new int[]{-34});
    rules[1019] = new Rule(-330, new int[]{-301});
    rules[1020] = new Rule(-330, new int[]{-168});
    rules[1021] = new Rule(-330, new int[]{-248});
    rules[1022] = new Rule(-330, new int[]{-125});
    rules[1023] = new Rule(-330, new int[]{8,-4,9});
    rules[1024] = new Rule(-328, new int[]{-103});
    rules[1025] = new Rule(-328, new int[]{-330});
    rules[1026] = new Rule(-329, new int[]{-211});
    rules[1027] = new Rule(-329, new int[]{-4});
    rules[1028] = new Rule(-329, new int[]{-330});
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
			(CurrentSemanticValue.stn as compilation_unit).Language = PascalABCCompiler.StringConstants.pascalLanguageName;                
		}
        break;
      case 48: // unit_file -> unit_header, abc_interface_part, initialization_part, tkPoint
{ 
			CurrentSemanticValue.stn = new unit_module(ValueStack[ValueStack.Depth-4].stn as unit_name, ValueStack[ValueStack.Depth-3].stn as interface_node, null, 
			  (ValueStack[ValueStack.Depth-2].stn as initfinal_part).initialization_sect, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).finalization_sect, /*$1 as attribute_list*/ null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as compilation_unit).Language = PascalABCCompiler.StringConstants.pascalLanguageName;   
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
               //                                  proc_func_decl_noclass
{
			var dcl = ValueStack[ValueStack.Depth-2].stn as declarations;
			//($2 as procedure_definition).AssignAttrList($2 as attribute_list);
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
					parserTools.AddErrorFromResource("BAD_INT2",CurrentLocationSpan);
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
      case 164: // pascal_set_const -> tkSquareOpen, elem_list, tkSquareClose
{
            // �?сли elem_list п�?с�? или соде�?жи�? диапазон, �?о э�?о множес�?во, ина�?е массив. С PascalABC.NET 3.10  
            var is_set = false;
            var el = ValueStack[ValueStack.Depth-2].stn as expression_list;
            if (el == null || el.Count == 0)
              is_set = true;
            else if (el.expressions.Count(x => x is diapason_expr_new) > 0)
                is_set = true;
            if (is_set)   
				CurrentSemanticValue.ex = new pascal_set_constant(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 				
		}
        break;
      case 165: // const_set -> pascal_set_const
{
            CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;				
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
      case 607: // expr -> expr, tkTo, expr_l1
{ CurrentSemanticValue.ex = new to_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 608: // expr_l1 -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 609: // expr_l1 -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 610: // expr_l1 -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 611: // expr_l1_for_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 612: // expr_l1_for_question_expr -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 613: // expr_l1_for_new_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 614: // expr_l1_for_new_question_expr -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 615: // expr_l1_func_decl_lambda -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 616: // expr_l1_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 617: // expr_l1_for_lambda -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 618: // expr_l1_for_lambda -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 619: // expr_l1_for_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 620: // expr_dq -> relop_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 621: // expr_dq -> expr_dq, tkDoubleQuestion, relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 622: // sizeof_expr -> tkSizeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new sizeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, null, CurrentLocationSpan);  
		}
        break;
      case 623: // typeof_expr -> tkTypeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 624: // typeof_expr -> tkTypeOf, tkRoundOpen, empty_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 625: // question_expr -> expr_l1_for_question_expr, tkQuestion, 
                //                  expr_l1_for_question_expr, tkColon, 
                //                  expr_l1_for_question_expr
{ 
            if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
            	parserTools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 626: // new_question_expr -> tkIf, expr_l1_for_new_question_expr, tkThen, 
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
      case 627: // empty_template_type_reference -> simple_type_identifier, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 628: // empty_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
        }
        break;
      case 629: // simple_or_template_type_reference -> simple_type_identifier
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 630: // simple_or_template_type_reference -> simple_type_identifier, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 631: // simple_or_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 632: // simple_or_template_or_question_type_reference -> 
                //                                                  simple_or_template_type_reference
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 633: // simple_or_template_or_question_type_reference -> simple_type_question
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 634: // optional_array_initializer -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = new array_const((expression_list)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan); 
		}
        break;
      case 636: // new_expr -> tkNew, simple_or_template_type_reference, 
                //             optional_expr_list_with_bracket
{
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-2].td, ValueStack[ValueStack.Depth-1].stn as expression_list, false, null, CurrentLocationSpan);
        }
        break;
      case 637: // new_expr -> tkNew, simple_or_template_type_reference, tkSquareOpen, 
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
      case 638: // new_expr -> tkNew, tkClass, tkRoundOpen, list_fields_in_unnamed_object, 
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
      case 639: // field_in_unnamed_object -> identifier, tkAssign, expr_l1
{
		    if (ValueStack[ValueStack.Depth-1].ex is nil_const)
				parserTools.AddErrorFromResource("NIL_IN_UNNAMED_OBJECT",CurrentLocationSpan);		    
			CurrentSemanticValue.ob = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 640: // field_in_unnamed_object -> expr_l1
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
      case 641: // list_fields_in_unnamed_object -> field_in_unnamed_object
{
			var l = new name_assign_expr_list();
			CurrentSemanticValue.ob = l.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
		}
        break;
      case 642: // list_fields_in_unnamed_object -> list_fields_in_unnamed_object, tkComma, 
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
      case 643: // optional_expr_list_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 644: // optional_expr_list_with_bracket -> tkRoundOpen, optional_expr_list, 
                //                                    tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 645: // relop_expr -> simple_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 646: // relop_expr -> relop_expr, relop, simple_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 647: // relop_expr -> relop_expr, relop, new_question_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 648: // relop_expr -> is_type_expr, tkRoundOpen, pattern_out_param_list, tkRoundClose
{
            var isTypeCheck = ValueStack[ValueStack.Depth-4].ex as typecast_node;
            var deconstructorPattern = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, isTypeCheck.type_def, null, CurrentLocationSpan); 
            CurrentSemanticValue.ex = new is_pattern_expr(isTypeCheck.expr, deconstructorPattern, CurrentLocationSpan);
        }
        break;
      case 649: // pattern -> simple_or_template_type_reference, tkRoundOpen, 
                //            pattern_out_param_list, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 650: // pattern_optional_var -> simple_or_template_type_reference, tkRoundOpen, 
                //                         pattern_out_param_list_optional_var, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 651: // deconstruction_or_const_pattern -> simple_or_template_type_reference, 
                //                                    tkRoundOpen, 
                //                                    pattern_out_param_list_optional_var, 
                //                                    tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 652: // deconstruction_or_const_pattern -> const_pattern_expr_list
{
		    CurrentSemanticValue.stn = new const_pattern(ValueStack[ValueStack.Depth-1].ob as List<syntax_tree_node>, CurrentLocationSpan); 
		}
        break;
      case 653: // const_pattern_expr_list -> const_pattern_expression
{ 
			CurrentSemanticValue.ob = new List<syntax_tree_node>(); 
			(CurrentSemanticValue.ob as List<syntax_tree_node>).Add(ValueStack[ValueStack.Depth-1].stn);
		}
        break;
      case 654: // const_pattern_expr_list -> const_pattern_expr_list, tkComma, 
                //                            const_pattern_expression
{ 
			var list = ValueStack[ValueStack.Depth-3].ob as List<syntax_tree_node>;
            list.Add(ValueStack[ValueStack.Depth-1].stn);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 655: // const_pattern_expression -> literal_or_number
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 656: // const_pattern_expression -> simple_or_template_type_reference
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 657: // const_pattern_expression -> tkNil
{ 
			CurrentSemanticValue.stn = new nil_const();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 658: // const_pattern_expression -> sizeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 659: // const_pattern_expression -> typeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 660: // collection_pattern -> tkSquareOpen, collection_pattern_expr_list, tkSquareClose
{
			CurrentSemanticValue.stn = new collection_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 661: // collection_pattern_expr_list -> collection_pattern_list_item
{
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 662: // collection_pattern_expr_list -> collection_pattern_expr_list, tkComma, 
                //                                 collection_pattern_list_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 663: // collection_pattern_list_item -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 664: // collection_pattern_list_item -> collection_pattern_var_item
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 665: // collection_pattern_list_item -> tkUnderscore
{
			CurrentSemanticValue.stn = new collection_pattern_wild_card(CurrentLocationSpan);
		}
        break;
      case 666: // collection_pattern_list_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 667: // collection_pattern_list_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 668: // collection_pattern_list_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 669: // collection_pattern_list_item -> tkDotDot
{
			CurrentSemanticValue.stn = new collection_pattern_gap_parameter(CurrentLocationSpan);
		}
        break;
      case 670: // collection_pattern_var_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new collection_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 671: // tuple_pattern -> tkRoundOpen, tuple_pattern_item_list, tkRoundClose
{
			if ((ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>).Count>6) 
				parserTools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
			CurrentSemanticValue.stn = new tuple_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 672: // tuple_pattern_item -> tkUnderscore
{ 
			CurrentSemanticValue.stn = new tuple_pattern_wild_card(CurrentLocationSpan); 
		}
        break;
      case 673: // tuple_pattern_item -> literal_or_number
{ 
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 674: // tuple_pattern_item -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 675: // tuple_pattern_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new tuple_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 676: // tuple_pattern_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 677: // tuple_pattern_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 678: // tuple_pattern_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 679: // tuple_pattern_item_list -> tuple_pattern_item
{ 
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 680: // tuple_pattern_item_list -> tuple_pattern_item_list, tkComma, tuple_pattern_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 681: // pattern_out_param_list_optional_var -> pattern_out_param_optional_var
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 682: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkSemiColon, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 683: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkComma, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 684: // pattern_out_param_list -> pattern_out_param
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 685: // pattern_out_param_list -> pattern_out_param_list, tkSemiColon, 
                //                           pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 686: // pattern_out_param_list -> pattern_out_param_list, tkComma, pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 687: // pattern_out_param -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 688: // pattern_out_param -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 689: // pattern_out_param -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 690: // pattern_out_param -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 691: // pattern_out_param -> pattern
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 692: // pattern_out_param -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 693: // pattern_out_param -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 694: // pattern_out_param_optional_var -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 695: // pattern_out_param_optional_var -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 696: // pattern_out_param_optional_var -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 697: // pattern_out_param_optional_var -> identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, false, CurrentLocationSpan);
        }
        break;
      case 698: // pattern_out_param_optional_var -> identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, false, CurrentLocationSpan);
        }
        break;
      case 699: // pattern_out_param_optional_var -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 700: // pattern_out_param_optional_var -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 701: // pattern_out_param_optional_var -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 702: // pattern_out_param_optional_var -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 703: // pattern_out_param_optional_var -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 704: // simple_expr_or_nothing -> simple_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 705: // simple_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 706: // const_expr_or_nothing -> const_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 707: // const_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 708: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing
{
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 709: // format_expr -> tkColon, simple_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 710: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing, tkColon, 
                //                simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 711: // format_expr -> tkColon, simple_expr_or_nothing, tkColon, simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 712: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 713: // format_const_expr -> tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 714: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing, tkColon, 
                //                      const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 715: // format_const_expr -> tkColon, const_expr_or_nothing, tkColon, const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 716: // relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 717: // relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 718: // relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 719: // relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 720: // relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 721: // relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 722: // relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 723: // relop -> tkNot, tkIn
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
      case 724: // simple_expr -> term1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 725: // simple_expr -> simple_expr, tkDotDot, term1
{ 
		if (parserTools.buildTreeForFormatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
	}
        break;
      case 726: // term1 -> term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 727: // term1 -> term1, addop, term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 728: // term1 -> term1, addop, new_question_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 729: // addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 730: // addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 731: // addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 732: // addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 733: // addop -> tkCSharpStyleOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 734: // typecast_op -> tkAs
{ 
			CurrentSemanticValue.ob = op_typecast.as_op; 
		}
        break;
      case 735: // typecast_op -> tkIs
{ 
			CurrentSemanticValue.ob = op_typecast.is_op; 
		}
        break;
      case 736: // as_is_expr -> is_type_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 737: // as_is_expr -> as_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 738: // as_expr -> term, tkAs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 739: // as_expr -> term, tkAs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 740: // is_type_expr -> term, tkIs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 741: // is_type_expr -> term, tkIs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 742: // power_expr -> factor_without_unary_op, tkStarStar, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 743: // power_expr -> factor_without_unary_op, tkStarStar, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 744: // power_expr -> sign, power_expr
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 745: // term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 746: // term -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 747: // term -> power_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 748: // term -> term, mulop, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 749: // term -> term, mulop, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 750: // term -> term, mulop, new_question_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 751: // term -> as_is_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 752: // mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 753: // mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 754: // mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 755: // mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 756: // mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 757: // mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 758: // mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 759: // default_expr -> tkDefault, tkRoundOpen, 
                //                 simple_or_template_or_question_type_reference, tkRoundClose
{ 
			CurrentSemanticValue.ex = new default_operator(ValueStack[ValueStack.Depth-2].td as named_type_reference, CurrentLocationSpan);  
		}
        break;
      case 760: // tuple -> tkRoundOpen, expr_l1_or_unpacked, tkComma, expr_l1_or_unpacked_list, 
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
      case 761: // factor_without_unary_op -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 762: // factor_without_unary_op -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 763: // factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 764: // factor -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 765: // factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
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
      case 787: // var_with_init_for_expr_with_let -> tkVar, identifier, tkAssign, expr, 
                //                                    tkSemiColon
{
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-4].id as addressed_value, ValueStack[ValueStack.Depth-2].ex, Operators.Assignment, CurrentLocationSpan);
		}
        break;
      case 788: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 789: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let_list, 
                //                                         var_with_init_for_expr_with_let
{
			ValueStack[ValueStack.Depth-2].stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
		}
        break;
      case 790: // proc_func_call -> variable, tkRoundOpen, optional_expr_list_func_param, 
                //                   tkRoundClose
{
			if (ValueStack[ValueStack.Depth-4].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-4], "^");
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 791: // variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 792: // variable -> operator_name_ident
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 793: // variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 794: // variable -> tkRoundOpen, expr, tkRoundClose
{
		    if (!parserTools.buildTreeForFormatter) 
            {
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
            } 
			else CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
        }
        break;
      case 795: // variable -> tkRoundOpen, var_with_init_for_expr_with_let_list, expr, 
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
      case 796: // variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 797: // variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 798: // variable -> literal_or_number, tkPoint, identifier_or_keyword
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");		
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 799: // variable -> variable, tkSquareOpen, expr_list, tkSquareClose
{
        	CurrentSemanticValue.ex = NewIndexerOrSlice(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list,CurrentLocationSpan);
        }
        break;
      case 800: // variable -> literal_or_number, tkSquareOpen, expr_list, tkSquareClose
{
        	CurrentSemanticValue.ex = NewIndexerOrSlice(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list,CurrentLocationSpan);
        	/*var el = $3 as expression_list; // SSM 10/03/16
        	if (el.Count==1 && el.expressions[0] is format_expr) 
        	{
        		var fe = el.expressions[0] as format_expr;
                if (!parserTools.buildTreeForFormatter)
                {
                    if (fe.expr == null)
                        fe.expr = new int32_const(int.MaxValue,@3);
                    if (fe.format1 == null)
                        fe.format1 = new int32_const(int.MaxValue,@3);
                }
        		$$ = new slice_expr($1 as addressed_value,fe.expr,fe.format1,fe.format2,@$);
			}   
			// многоме�?н�?е с�?ез�?
            else if (el.expressions.Any(e => e is format_expr))
            {
            	if (el.expressions.Count > 4)
            		parserTools.AddErrorFromResource("SLICES_OF MULTIDIMENSIONAL_ARRAYS_ALLOW_ONLY_FOR_RANK_LT_5",@$); // С�?ез�? многоме�?н�?�? массивов �?аз�?е�?ен�? �?ол�?ко для массивов �?азме�?нос�?и < 5  
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
				var sle = new slice_expr($1 as addressed_value,null,null,null,@$);
				sle.slices = ll;
				$$ = sle;
            }
			else $$ = new indexer($1 as addressed_value, el, @$);*/
        }
        break;
      case 801: // variable -> variable, tkQuestionSquareOpen, format_expr, tkSquareClose
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
      case 802: // variable -> literal_or_number, tkQuestionSquareOpen, format_expr, tkSquareClose
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
      case 804: // variable -> pascal_set_const
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
		}
        break;
      case 805: // variable -> proc_func_call
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 806: // variable -> variable, tkPoint, identifier_keyword_operatorname
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 807: // variable -> tuple, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 808: // variable -> variable, tkDeref
{
			CurrentSemanticValue.ex = new roof_dereference(ValueStack[ValueStack.Depth-2].ex as addressed_value,CurrentLocationSpan);
        }
        break;
      case 809: // variable -> variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 810: // optional_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 811: // optional_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 812: // optional_expr_list_func_param -> expr_list_func_param
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 813: // optional_expr_list_func_param -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 814: // elem_list -> elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 815: // elem_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 816: // elem_list1 -> elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 817: // elem_list1 -> elem_list1, tkComma, elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 818: // elem -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 819: // elem -> expr, tkDotDot, expr
{ CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 820: // one_literal -> tkStringLiteral
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 821: // one_literal -> tkAsciiChar
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 822: // literal -> literal_list
{ 
			CurrentSemanticValue.ex = NewLiteral(ValueStack[ValueStack.Depth-1].stn as literal_const_line);
        }
        break;
      case 823: // literal -> tkFormatStringLiteral
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
      case 824: // literal -> tkMultilineStringLiteral
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
      case 825: // literal_list -> one_literal
{ 
			CurrentSemanticValue.stn = new literal_const_line(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 826: // literal_list -> literal_list, one_literal
{ 
        	var line = ValueStack[ValueStack.Depth-2].stn as literal_const_line;
            if (line.literals.Last() is string_const && ValueStack[ValueStack.Depth-1].ex is string_const)
            	parserTools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.stn = line.Add(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 827: // operator_name_ident -> tkOperator, overload_operator
{ 
			CurrentSemanticValue.ex = new operator_name_ident((ValueStack[ValueStack.Depth-1].op as op_type_node).text, (ValueStack[ValueStack.Depth-1].op as op_type_node).type, CurrentLocationSpan);
		}
        break;
      case 828: // optional_method_modificators -> tkSemiColon
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 829: // optional_method_modificators -> tkSemiColon, meth_modificators, tkSemiColon
{ 
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 830: // optional_method_modificators1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 831: // optional_method_modificators1 -> tkSemiColon, meth_modificators
{ 
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 832: // meth_modificators -> meth_modificator
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan); 
		}
        break;
      case 833: // meth_modificators -> meth_modificators, tkSemiColon, meth_modificator
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as procedure_attributes_list).Add(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan);  
		}
        break;
      case 834: // identifier -> tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 835: // identifier -> property_specifier_directives
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 836: // identifier -> non_reserved
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 837: // identifier -> tkStep
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 838: // identifier -> tkIndex
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 839: // identifier_or_keyword -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 840: // identifier_or_keyword -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 841: // identifier_or_keyword -> reserved_keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 842: // identifier_keyword_operatorname -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 843: // identifier_keyword_operatorname -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 844: // identifier_keyword_operatorname -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 845: // meth_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 846: // meth_modificator -> tkOverload
{ 
            CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id;
            parserTools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", ValueStack[ValueStack.Depth-1].id.source_context);
        }
        break;
      case 847: // meth_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 848: // meth_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 849: // meth_modificator -> tkExtensionMethod
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 850: // meth_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 851: // property_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 852: // property_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 853: // property_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 854: // property_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 855: // property_specifier_directives -> tkRead
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 856: // property_specifier_directives -> tkWrite
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 857: // non_reserved -> tkName
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 858: // non_reserved -> tkNew
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 859: // visibility_specifier -> tkInternal
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 860: // visibility_specifier -> tkPublic
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 861: // visibility_specifier -> tkProtected
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 862: // visibility_specifier -> tkPrivate
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 863: // keyword -> visibility_specifier
{ 
			CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  
		}
        break;
      case 864: // keyword -> tkSealed
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 865: // keyword -> tkTemplate
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 866: // keyword -> tkOr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 867: // keyword -> tkTypeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 868: // keyword -> tkSizeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 869: // keyword -> tkDefault
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 870: // keyword -> tkWhere
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 871: // keyword -> tkXor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 872: // keyword -> tkAnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 873: // keyword -> tkDiv
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 874: // keyword -> tkMod
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 875: // keyword -> tkShl
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 876: // keyword -> tkShr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 877: // keyword -> tkNot
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 878: // keyword -> tkAs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 879: // keyword -> tkIn
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 880: // keyword -> tkIs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 881: // keyword -> tkArray
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 882: // keyword -> tkSequence
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 883: // keyword -> tkBegin
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 884: // keyword -> tkCase
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 885: // keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 886: // keyword -> tkConst
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 887: // keyword -> tkConstructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 888: // keyword -> tkDestructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 889: // keyword -> tkDownto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 890: // keyword -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 891: // keyword -> tkElse
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 892: // keyword -> tkEnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 893: // keyword -> tkExcept
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 894: // keyword -> tkFile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 895: // keyword -> tkAuto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 896: // keyword -> tkFinalization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 897: // keyword -> tkFinally
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 898: // keyword -> tkFor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 899: // keyword -> tkForeach
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 900: // keyword -> tkFunction
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 901: // keyword -> tkIf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 902: // keyword -> tkImplementation
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 903: // keyword -> tkInherited
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 904: // keyword -> tkInitialization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 905: // keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 906: // keyword -> tkProcedure
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 907: // keyword -> tkProperty
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 908: // keyword -> tkRaise
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 909: // keyword -> tkRecord
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 910: // keyword -> tkRepeat
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 911: // keyword -> tkSet
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 912: // keyword -> tkTry
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 913: // keyword -> tkType
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 914: // keyword -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 915: // keyword -> tkThen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 916: // keyword -> tkTo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 917: // keyword -> tkUntil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 918: // keyword -> tkUses
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 919: // keyword -> tkVar
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 920: // keyword -> tkWhile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 921: // keyword -> tkWith
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 922: // keyword -> tkNil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 923: // keyword -> tkGoto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 924: // keyword -> tkOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 925: // keyword -> tkLabel
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 926: // keyword -> tkProgram
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 927: // keyword -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 928: // keyword -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 929: // keyword -> tkNamespace
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 930: // keyword -> tkExternal
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 931: // keyword -> tkParams
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 932: // keyword -> tkEvent
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 933: // keyword -> tkYield
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 934: // keyword -> tkMatch
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 935: // keyword -> tkWhen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 936: // keyword -> tkPartial
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 937: // keyword -> tkAbstract
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 938: // keyword -> tkLock
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 939: // keyword -> tkImplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 940: // keyword -> tkExplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 941: // keyword -> tkOn
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 942: // keyword -> tkVirtual
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 943: // keyword -> tkOverride
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 944: // keyword -> tkLoop
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 945: // keyword -> tkExtensionMethod
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 946: // keyword -> tkOverload
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 947: // keyword -> tkReintroduce
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 948: // keyword -> tkForward
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 949: // reserved_keyword -> tkOperator
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 950: // overload_operator -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 951: // overload_operator -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 952: // overload_operator -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 953: // overload_operator -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 954: // overload_operator -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 955: // overload_operator -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 956: // overload_operator -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 957: // overload_operator -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 958: // overload_operator -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 959: // overload_operator -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 960: // overload_operator -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 961: // overload_operator -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 962: // overload_operator -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 963: // overload_operator -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 964: // overload_operator -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 965: // overload_operator -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 966: // overload_operator -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 967: // overload_operator -> tkNot
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 968: // overload_operator -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 969: // overload_operator -> tkImplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 970: // overload_operator -> tkExplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 971: // overload_operator -> assign_operator
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 972: // overload_operator -> tkStarStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 973: // assign_operator -> tkAssign
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 974: // assign_operator -> tkPlusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 975: // assign_operator -> tkMinusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 976: // assign_operator -> tkMultEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 977: // assign_operator -> tkDivEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 978: // lambda_unpacked_params -> tkBackSlashRoundOpen, 
                //                           lambda_list_of_unpacked_params_or_id, tkComma, 
                //                           lambda_unpacked_params_or_id, tkRoundClose
{
			// �?ез�?л�?�?а�? надо п�?исвои�?�? каком�? �?о са�?а�?ном�? пол�? в function_lambda_definition
			(ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-2].ob as ident_or_list);
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list;
		}
        break;
      case 979: // lambda_unpacked_params_or_id -> lambda_unpacked_params
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].ex as unpacked_list_of_ident_or_list);
		}
        break;
      case 980: // lambda_unpacked_params_or_id -> identifier
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].id as ident);
		}
        break;
      case 981: // lambda_list_of_unpacked_params_or_id -> lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = new unpacked_list_of_ident_or_list();
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LocationStack[LocationStack.Depth-1];
		}
        break;
      case 982: // lambda_list_of_unpacked_params_or_id -> lambda_list_of_unpacked_params_or_id, 
                //                                         tkComma, lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 983: // expr_l1_or_unpacked -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 984: // expr_l1_or_unpacked -> lambda_unpacked_params
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 985: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 986: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked_list, tkComma, 
                //                             expr_l1_or_unpacked
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 987: // func_decl_lambda -> identifier, tkArrow, lambda_function_body
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
      case 988: // func_decl_lambda -> tkRoundOpen, tkRoundClose, lambda_type_ref_noproctype, 
                //                     tkArrow, lambda_function_body
{
		    // �?дес�? надо анализи�?ова�?�? по �?ел�? и либо ос�?авля�?�? lambda_inferred_type, либо дела�?�? его null!
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, sl, CurrentLocationSpan);	
		}
        break;
      case 989: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkRoundClose, 
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
      case 990: // func_decl_lambda -> tkRoundOpen, identifier, tkSemiColon, full_lambda_fp_list, 
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
      case 991: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkSemiColon, 
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
      case 992: // func_decl_lambda -> tkRoundOpen, expr_l1_or_unpacked, tkComma, 
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
      case 993: // func_decl_lambda -> lambda_unpacked_params, rem_lambda
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
      case 994: // func_decl_lambda -> expl_func_decl_lambda
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 995: // optional_full_lambda_fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 996: // optional_full_lambda_fp_list -> tkSemiColon, full_lambda_fp_list
{
		CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
	}
        break;
      case 997: // rem_lambda -> lambda_type_ref_noproctype, tkArrow, lambda_function_body
{ 
		    CurrentSemanticValue.ob = new pair_type_stlist(ValueStack[ValueStack.Depth-3].td,ValueStack[ValueStack.Depth-1].stn as statement_list);
		}
        break;
      case 998: // expl_func_decl_lambda -> tkFunction, lambda_type_ref_noproctype, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 999: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, tkRoundClose, 
                //                          lambda_type_ref_noproctype, tkArrow, 
                //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1000: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, full_lambda_fp_list, 
                 //                          tkRoundClose, lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1001: // expl_func_decl_lambda -> tkProcedure, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1002: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, tkRoundClose, tkArrow, 
                 //                          lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1003: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, full_lambda_fp_list, 
                 //                          tkRoundClose, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-4].stn as formal_parameters, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1004: // full_lambda_fp_list -> lambda_simple_fp_sect
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
      case 1005: // full_lambda_fp_list -> full_lambda_fp_list, tkSemiColon, lambda_simple_fp_sect
{
			CurrentSemanticValue.stn =(ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
		}
        break;
      case 1006: // lambda_simple_fp_sect -> ident_list, lambda_type_ref
{
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-2].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan);
		}
        break;
      case 1007: // lambda_type_ref -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
		}
        break;
      case 1008: // lambda_type_ref -> tkColon, fptype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 1009: // lambda_type_ref_noproctype -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
		}
        break;
      case 1010: // lambda_type_ref_noproctype -> tkColon, fptype_noproctype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 1011: // common_lambda_body -> compound_stmt
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 1012: // common_lambda_body -> if_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1013: // common_lambda_body -> while_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1014: // common_lambda_body -> repeat_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1015: // common_lambda_body -> for_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1016: // common_lambda_body -> foreach_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1017: // common_lambda_body -> loop_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1018: // common_lambda_body -> case_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1019: // common_lambda_body -> try_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1020: // common_lambda_body -> lock_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1021: // common_lambda_body -> raise_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1022: // common_lambda_body -> yield_stmt
{
			parserTools.AddErrorFromResource("YIELD_STATEMENT_CANNOT_BE_USED_IN_LAMBDA_BODY", CurrentLocationSpan);
		}
        break;
      case 1023: // common_lambda_body -> tkRoundOpen, assignment, tkRoundClose
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-2]);
		}
        break;
      case 1024: // lambda_function_body -> expr_l1_for_lambda
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
      case 1025: // lambda_function_body -> common_lambda_body
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 1026: // lambda_procedure_body -> proc_call
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1027: // lambda_procedure_body -> assignment
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1028: // lambda_procedure_body -> common_lambda_body
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
