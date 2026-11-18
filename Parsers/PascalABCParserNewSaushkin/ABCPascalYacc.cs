// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  DESKTOP-L2INHKG
// DateTime: 18.11.2026 19:10:16
// UserName: user
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
    tkFloat=157,tkHex=158,tkUnknown=159,tkStep=160,tkExport=161};

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
  private static Rule[] rules = new Rule[1052];
  private static State[] states = new State[1743];
  private static string[] nonTerms = new string[] {
      "parse_goal", "unit_reexport_nonempty", "unit_reexport_clauses", "unit_reexport_list", 
      "unit_reexport_item", "unit_reexport_after_declarations", "unit_key_word", 
      "class_or_static", "assignment", "optional_array_initializer", "attribute_declarations", 
      "ot_visibility_specifier", "one_attribute", "attribute_variable", "const_factor", 
      "const_factor_without_unary_op", "const_variable_2", "const_term", "const_variable", 
      "literal_or_number", "unsigned_number", "program_block", "optional_var", 
      "class_attribute", "class_attributes", "class_attributes1", "lambda_unpacked_params_or_id", 
      "lambda_list_of_unpacked_params_or_id", "member_list_section", "optional_component_list_seq_end", 
      "const_decl", "only_const_decl", "const_decl_sect", "object_type", "record_type", 
      "member_list", "method_decl_list", "field_or_const_definition_list", "case_stmt", 
      "case_list", "program_decl_sect_list", "int_decl_sect_list1", "inclass_decl_sect_list1", 
      "interface_decl_sect_list", "decl_sect_list", "decl_sect_list1", "inclass_decl_sect_list", 
      "decl_sect_list_proc_func_only", "field_or_const_definition", "abc_decl_sect", 
      "decl_sect", "int_decl_sect", "type_decl", "simple_type_decl", "simple_field_or_const_definition", 
      "res_str_decl_sect", "method_decl_withattr", "method_or_property_decl", 
      "property_definition", "fp_sect", "default_expr", "tuple", "expr_as_stmt", 
      "exception_block", "external_block", "exception_handler", "exception_handler_list", 
      "exception_identifier", "typed_const_list1", "typed_const_list", "optional_expr_list", 
      "optional_expr_list_func_param", "elem_list", "optional_expr_list_with_bracket", 
      "optional_expr_list_func_param_with_bracket", "expr_list", "expr_list_func_param", 
      "const_elem_list1", "case_label_list", "const_elem_list", "optional_const_func_expr_list", 
      "elem_list1", "enumeration_id", "expr_l1_or_unpacked_list", "enumeration_id_list", 
      "const_simple_expr", "term", "term1", "typed_const", "typed_const_plus", 
      "typed_var_init_expression", "expr", "expr_with_func_decl_lambda", "expr_with_func_decl_lambda_ass", 
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
      "interface_declarations", "simple_type_list", "literal", "one_literal", 
      "literal_list", "label_decl_sect", "lock_stmt", "func_name", "proc_name", 
      "optional_proc_name", "new_expr", "allowable_expr_as_stmt", "parts", "inclass_block", 
      "block", "proc_func_external_block", "exception_class_type_identifier", 
      "simple_type_identifier", "base_class_name", "base_classes_names_list", 
      "optional_base_classes", "one_compiler_directive", "optional_head_compiler_directives", 
      "head_compiler_directives", "program_heading_2", "optional_tk_point", "program_param_list", 
      "optional_semicolon", "operator_name_ident", "const_relop", "const_addop", 
      "assign_operator", "const_mulop", "relop", "addop", "mulop", "sign", "overload_operator", 
      "typecast_op", "property_specifiers", "write_property_specifiers", "read_property_specifiers", 
      "array_defaultproperty", "meth_modificators", "optional_method_modificators", 
      "optional_method_modificators1", "meth_modificator", "property_modificator", 
      "optional_property_initialization", "proc_call", "proc_func_constr_destr_decl", 
      "proc_func_decl", "inclass_proc_func_decl", "inclass_proc_func_decl_without_modifiers", 
      "constr_destr_decl", "inclass_constr_destr_decl", "method_decl", "proc_func_constr_destr_decl_with_attr", 
      "proc_func_decl_without_modifiers", "method_header", "proc_type_decl", 
      "procedural_type_kind", "proc_header", "procedural_type", "constr_destr_header", 
      "proc_func_header", "func_header", "method_procfunc_header", "int_func_header", 
      "int_proc_header", "property_interface", "program_file", "program_header", 
      "parameter_decl", "parameter_decl_list", "property_parameter_list", "const_set", 
      "pascal_set_const", "question_expr", "question_constexpr", "new_question_expr", 
      "record_const", "const_field_list_1", "const_field_list", "const_field", 
      "repeat_stmt", "raise_stmt", "pointer_type", "attribute_declaration", "one_or_some_attribute", 
      "stmt_list", "else_case", "exception_block_else_branch", "compound_stmt", 
      "string_type", "sizeof_expr", "simple_property_definition", "stmt_or_expression", 
      "unlabelled_stmt", "stmt", "case_item", "set_type", "as_is_expr", "as_is_constexpr", 
      "is_type_expr", "as_expr", "power_expr", "power_constexpr", "unsized_array_type", 
      "simple_type_or_", "simple_type", "simple_type_question", "optional_type_specification", 
      "fptype", "type_ref", "fptype_noproctype", "array_type", "template_param", 
      "template_empty_param", "structured_type", "empty_template_type_reference", 
      "simple_or_template_type_reference", "simple_or_template_or_question_type_reference", 
      "type_ref_or_secific", "type_decl_type", "type_ref_and_secific_list", "type_decl_sect", 
      "try_handler", "class_or_interface_keyword", "optional_tk_do", "keyword", 
      "reserved_keyword", "typeof_expr", "simple_fp_sect", "template_param_list", 
      "template_empty_param_list", "template_type_params", "template_type_empty_params", 
      "template_type", "try_stmt", "uses_clause", "used_units_list", "uses_clause_one", 
      "uses_clause_one_or_empty", "uses_clause_nonempty", "unit_file", "used_unit_name", 
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
      "tuple_pattern_item_list", "const_pattern_expr_list", "var_with_init_for_expr_with_let", 
      "var_with_init_for_expr_with_let_list", "index_or_nothing", "$accept", 
      };

  static GPPGParser() {
    states[0] = new State(new int[]{61,1610,105,1709,106,1710,109,1711,88,1716,90,1721,89,1728,75,1733,77,1739,3,-27,52,-27,91,-27,59,-27,29,-27,66,-27,50,-27,53,-27,62,-27,11,-27,44,-27,37,-27,28,-27,26,-27,19,-27,30,-27,31,-27},new int[]{-1,1,-240,3,-241,4,-314,1622,-316,1623,-7,1704,-181,1715});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,1606,52,-14,91,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-191,5,-192,1604,-190,1609});
    states[5] = new State(new int[]{52,1591,91,-41,59,-41,29,-41,66,-41,50,-41,53,-41,62,-41,11,-41,44,-41,37,-41,28,-41,26,-41,19,-41,30,-41,31,-41},new int[]{-309,6,-313,1589,-311,1603});
    states[6] = new State(-82,new int[]{-22,7,-41,14,-45,1524,-46,1525});
    states[7] = new State(new int[]{7,9,10,10,5,11,100,12,6,13,2,-26},new int[]{-194,8});
    states[8] = new State(-20);
    states[9] = new State(-21);
    states[10] = new State(-22);
    states[11] = new State(-23);
    states[12] = new State(-24);
    states[13] = new State(-25);
    states[14] = new State(new int[]{91,16},new int[]{-262,15});
    states[15] = new State(-34);
    states[16] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514},new int[]{-259,17,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[17] = new State(new int[]{92,18,10,19});
    states[18] = new State(-550);
    states[19] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514},new int[]{-268,20,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[20] = new State(-552);
    states[21] = new State(-512);
    states[22] = new State(-515);
    states[23] = new State(new int[]{110,429,111,430,112,431,113,432,114,433,92,-548,10,-548,98,-548,101,-548,33,-548,104,-548,2,-548,9,-548,12,-548,100,-548,99,-548,32,-548,85,-548,84,-548,83,-548,82,-548,81,-548,86,-548},new int[]{-200,24});
    states[24] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-93,25,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,834,-105,659,-331,677});
    states[25] = new State(-542);
    states[26] = new State(new int[]{70,27,92,-618,10,-618,98,-618,101,-618,33,-618,104,-618,2,-618,9,-618,12,-618,100,-618,99,-618,32,-618,85,-618,84,-618,83,-618,82,-618,81,-618,86,-618});
    states[27] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,28,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[28] = new State(-627);
    states[29] = new State(new int[]{16,30,70,-628,92,-628,10,-628,98,-628,101,-628,33,-628,104,-628,2,-628,9,-628,12,-628,100,-628,99,-628,32,-628,85,-628,84,-628,83,-628,82,-628,81,-628,86,-628,6,-628,76,-628,5,-628,51,-628,58,-628,141,-628,143,-628,161,-628,80,-628,78,-628,160,-628,87,-628,45,-628,42,-628,8,-628,21,-628,22,-628,144,-628,147,-628,145,-628,146,-628,155,-628,158,-628,157,-628,156,-628,11,-628,57,-628,91,-628,40,-628,25,-628,97,-628,54,-628,35,-628,55,-628,102,-628,47,-628,36,-628,53,-628,60,-628,74,-628,72,-628,38,-628,71,-628,13,-631});
    states[30] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-103,31,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578});
    states[31] = new State(new int[]{120,157,125,158,123,159,121,160,124,161,122,162,137,163,135,164,16,-641,70,-641,92,-641,10,-641,98,-641,101,-641,33,-641,104,-641,2,-641,9,-641,12,-641,100,-641,99,-641,32,-641,85,-641,84,-641,83,-641,82,-641,81,-641,86,-641,13,-641,6,-641,76,-641,5,-641,51,-641,58,-641,141,-641,143,-641,161,-641,80,-641,78,-641,160,-641,87,-641,45,-641,42,-641,8,-641,21,-641,22,-641,144,-641,147,-641,145,-641,146,-641,155,-641,158,-641,157,-641,156,-641,11,-641,57,-641,91,-641,40,-641,25,-641,97,-641,54,-641,35,-641,55,-641,102,-641,47,-641,36,-641,53,-641,60,-641,74,-641,72,-641,38,-641,71,-641,116,-641,115,-641,128,-641,129,-641,126,-641,138,-641,136,-641,118,-641,117,-641,131,-641,132,-641,133,-641,134,-641,130,-641},new int[]{-202,32});
    states[32] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-110,33,-249,1523,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,602,-274,578});
    states[33] = new State(new int[]{6,34,120,-668,125,-668,123,-668,121,-668,124,-668,122,-668,137,-668,135,-668,16,-668,70,-668,92,-668,10,-668,98,-668,101,-668,33,-668,104,-668,2,-668,9,-668,12,-668,100,-668,99,-668,32,-668,85,-668,84,-668,83,-668,82,-668,81,-668,86,-668,13,-668,76,-668,5,-668,51,-668,58,-668,141,-668,143,-668,161,-668,80,-668,78,-668,160,-668,87,-668,45,-668,42,-668,8,-668,21,-668,22,-668,144,-668,147,-668,145,-668,146,-668,155,-668,158,-668,157,-668,156,-668,11,-668,57,-668,91,-668,40,-668,25,-668,97,-668,54,-668,35,-668,55,-668,102,-668,47,-668,36,-668,53,-668,60,-668,74,-668,72,-668,38,-668,71,-668,116,-668,115,-668,128,-668,129,-668,126,-668,138,-668,136,-668,118,-668,117,-668,131,-668,132,-668,133,-668,134,-668,130,-668});
    states[34] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-88,35,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,602,-274,578});
    states[35] = new State(new int[]{116,172,115,173,128,174,129,175,126,176,6,-747,5,-747,120,-747,125,-747,123,-747,121,-747,124,-747,122,-747,137,-747,135,-747,16,-747,70,-747,92,-747,10,-747,98,-747,101,-747,33,-747,104,-747,2,-747,9,-747,12,-747,100,-747,99,-747,32,-747,85,-747,84,-747,83,-747,82,-747,81,-747,86,-747,13,-747,76,-747,51,-747,58,-747,141,-747,143,-747,161,-747,80,-747,78,-747,160,-747,87,-747,45,-747,42,-747,8,-747,21,-747,22,-747,144,-747,147,-747,145,-747,146,-747,155,-747,158,-747,157,-747,156,-747,11,-747,57,-747,91,-747,40,-747,25,-747,97,-747,54,-747,35,-747,55,-747,102,-747,47,-747,36,-747,53,-747,60,-747,74,-747,72,-747,38,-747,71,-747,138,-747,136,-747,118,-747,117,-747,131,-747,132,-747,133,-747,134,-747,130,-747},new int[]{-203,36});
    states[36] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-87,37,-249,1522,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,602,-274,578});
    states[37] = new State(new int[]{138,178,136,1512,118,1515,117,1516,131,1517,132,1518,133,1519,134,1520,130,1521,116,-749,115,-749,128,-749,129,-749,126,-749,6,-749,5,-749,120,-749,125,-749,123,-749,121,-749,124,-749,122,-749,137,-749,135,-749,16,-749,70,-749,92,-749,10,-749,98,-749,101,-749,33,-749,104,-749,2,-749,9,-749,12,-749,100,-749,99,-749,32,-749,85,-749,84,-749,83,-749,82,-749,81,-749,86,-749,13,-749,76,-749,51,-749,58,-749,141,-749,143,-749,161,-749,80,-749,78,-749,160,-749,87,-749,45,-749,42,-749,8,-749,21,-749,22,-749,144,-749,147,-749,145,-749,146,-749,155,-749,158,-749,157,-749,156,-749,11,-749,57,-749,91,-749,40,-749,25,-749,97,-749,54,-749,35,-749,55,-749,102,-749,47,-749,36,-749,53,-749,60,-749,74,-749,72,-749,38,-749,71,-749},new int[]{-204,38});
    states[38] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-101,39,-275,40,-249,41,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-102,461});
    states[39] = new State(-770);
    states[40] = new State(-771);
    states[41] = new State(-772);
    states[42] = new State(-785);
    states[43] = new State(new int[]{7,44,11,150,17,380,138,-786,136,-786,118,-786,117,-786,131,-786,132,-786,133,-786,134,-786,130,-786,116,-786,115,-786,128,-786,129,-786,126,-786,6,-786,5,-786,120,-786,125,-786,123,-786,121,-786,124,-786,122,-786,137,-786,135,-786,16,-786,70,-786,92,-786,10,-786,98,-786,101,-786,33,-786,104,-786,2,-786,9,-786,12,-786,100,-786,99,-786,32,-786,85,-786,84,-786,83,-786,82,-786,81,-786,86,-786,13,-786,76,-786,51,-786,58,-786,141,-786,143,-786,161,-786,80,-786,78,-786,160,-786,87,-786,45,-786,42,-786,8,-786,21,-786,22,-786,144,-786,147,-786,145,-786,146,-786,155,-786,158,-786,157,-786,156,-786,57,-786,91,-786,40,-786,25,-786,97,-786,54,-786,35,-786,55,-786,102,-786,47,-786,36,-786,53,-786,60,-786,74,-786,72,-786,38,-786,71,-786,119,-783});
    states[44] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-143,45,-152,46,-156,49,-157,52,-299,57,-155,58,-300,148});
    states[45] = new State(-820);
    states[46] = new State(-862);
    states[47] = new State(-856);
    states[48] = new State(-857);
    states[49] = new State(-858);
    states[50] = new State(-878);
    states[51] = new State(-879);
    states[52] = new State(-859);
    states[53] = new State(-880);
    states[54] = new State(-881);
    states[55] = new State(-860);
    states[56] = new State(-861);
    states[57] = new State(-863);
    states[58] = new State(-886);
    states[59] = new State(-882);
    states[60] = new State(-883);
    states[61] = new State(-884);
    states[62] = new State(-885);
    states[63] = new State(-887);
    states[64] = new State(-888);
    states[65] = new State(-889);
    states[66] = new State(-890);
    states[67] = new State(-891);
    states[68] = new State(-892);
    states[69] = new State(-893);
    states[70] = new State(-894);
    states[71] = new State(-895);
    states[72] = new State(-896);
    states[73] = new State(-897);
    states[74] = new State(-898);
    states[75] = new State(-899);
    states[76] = new State(-900);
    states[77] = new State(-901);
    states[78] = new State(-902);
    states[79] = new State(-903);
    states[80] = new State(-904);
    states[81] = new State(-905);
    states[82] = new State(-906);
    states[83] = new State(-907);
    states[84] = new State(-908);
    states[85] = new State(-909);
    states[86] = new State(-910);
    states[87] = new State(-911);
    states[88] = new State(-912);
    states[89] = new State(-913);
    states[90] = new State(-914);
    states[91] = new State(-915);
    states[92] = new State(-916);
    states[93] = new State(-917);
    states[94] = new State(-918);
    states[95] = new State(-919);
    states[96] = new State(-920);
    states[97] = new State(-921);
    states[98] = new State(-922);
    states[99] = new State(-923);
    states[100] = new State(-924);
    states[101] = new State(-925);
    states[102] = new State(-926);
    states[103] = new State(-927);
    states[104] = new State(-928);
    states[105] = new State(-929);
    states[106] = new State(-930);
    states[107] = new State(-931);
    states[108] = new State(-932);
    states[109] = new State(-933);
    states[110] = new State(-934);
    states[111] = new State(-935);
    states[112] = new State(-936);
    states[113] = new State(-937);
    states[114] = new State(-938);
    states[115] = new State(-939);
    states[116] = new State(-940);
    states[117] = new State(-941);
    states[118] = new State(-942);
    states[119] = new State(-943);
    states[120] = new State(-944);
    states[121] = new State(-945);
    states[122] = new State(-946);
    states[123] = new State(-947);
    states[124] = new State(-948);
    states[125] = new State(-949);
    states[126] = new State(-950);
    states[127] = new State(-951);
    states[128] = new State(-952);
    states[129] = new State(-953);
    states[130] = new State(-954);
    states[131] = new State(-955);
    states[132] = new State(-956);
    states[133] = new State(-957);
    states[134] = new State(-958);
    states[135] = new State(-959);
    states[136] = new State(-960);
    states[137] = new State(-961);
    states[138] = new State(-962);
    states[139] = new State(-963);
    states[140] = new State(-964);
    states[141] = new State(-965);
    states[142] = new State(-966);
    states[143] = new State(-967);
    states[144] = new State(-968);
    states[145] = new State(-969);
    states[146] = new State(-970);
    states[147] = new State(-971);
    states[148] = new State(-864);
    states[149] = new State(-972);
    states[150] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-76,151,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,834,-105,659,-331,677});
    states[151] = new State(new int[]{12,152,100,153});
    states[152] = new State(-822);
    states[153] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-93,154,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,834,-105,659,-331,677});
    states[154] = new State(-613);
    states[155] = new State(-625);
    states[156] = new State(new int[]{120,157,125,158,123,159,121,160,124,161,122,162,137,163,135,164,16,-640,70,-640,92,-640,10,-640,98,-640,101,-640,33,-640,104,-640,2,-640,9,-640,12,-640,100,-640,99,-640,32,-640,85,-640,84,-640,83,-640,82,-640,81,-640,86,-640,13,-640,6,-640,76,-640,5,-640,51,-640,58,-640,141,-640,143,-640,161,-640,80,-640,78,-640,160,-640,87,-640,45,-640,42,-640,8,-640,21,-640,22,-640,144,-640,147,-640,145,-640,146,-640,155,-640,158,-640,157,-640,156,-640,11,-640,57,-640,91,-640,40,-640,25,-640,97,-640,54,-640,35,-640,55,-640,102,-640,47,-640,36,-640,53,-640,60,-640,74,-640,72,-640,38,-640,71,-640,116,-640,115,-640,128,-640,129,-640,126,-640,138,-640,136,-640,118,-640,117,-640,131,-640,132,-640,133,-640,134,-640,130,-640},new int[]{-202,32});
    states[157] = new State(-738);
    states[158] = new State(-739);
    states[159] = new State(-740);
    states[160] = new State(-741);
    states[161] = new State(-742);
    states[162] = new State(-743);
    states[163] = new State(-744);
    states[164] = new State(new int[]{137,165});
    states[165] = new State(-745);
    states[166] = new State(new int[]{6,34,5,167,120,-667,125,-667,123,-667,121,-667,124,-667,122,-667,137,-667,135,-667,16,-667,70,-667,92,-667,10,-667,98,-667,101,-667,33,-667,104,-667,2,-667,9,-667,12,-667,100,-667,99,-667,32,-667,85,-667,84,-667,83,-667,82,-667,81,-667,86,-667,13,-667,76,-667});
    states[167] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,5,-727,70,-727,92,-727,10,-727,98,-727,101,-727,33,-727,104,-727,2,-727,9,-727,12,-727,100,-727,99,-727,32,-727,84,-727,83,-727,82,-727,81,-727,6,-727},new int[]{-120,168,-110,603,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,602,-274,578});
    states[168] = new State(new int[]{5,169,70,-730,92,-730,10,-730,98,-730,101,-730,33,-730,104,-730,2,-730,9,-730,12,-730,100,-730,99,-730,32,-730,85,-730,84,-730,83,-730,82,-730,81,-730,86,-730,6,-730,76,-730});
    states[169] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-110,170,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,602,-274,578});
    states[170] = new State(new int[]{6,34,70,-732,92,-732,10,-732,98,-732,101,-732,33,-732,104,-732,2,-732,9,-732,12,-732,100,-732,99,-732,32,-732,85,-732,84,-732,83,-732,82,-732,81,-732,86,-732,76,-732});
    states[171] = new State(new int[]{116,172,115,173,128,174,129,175,126,176,6,-746,5,-746,120,-746,125,-746,123,-746,121,-746,124,-746,122,-746,137,-746,135,-746,16,-746,70,-746,92,-746,10,-746,98,-746,101,-746,33,-746,104,-746,2,-746,9,-746,12,-746,100,-746,99,-746,32,-746,85,-746,84,-746,83,-746,82,-746,81,-746,86,-746,13,-746,76,-746,51,-746,58,-746,141,-746,143,-746,161,-746,80,-746,78,-746,160,-746,87,-746,45,-746,42,-746,8,-746,21,-746,22,-746,144,-746,147,-746,145,-746,146,-746,155,-746,158,-746,157,-746,156,-746,11,-746,57,-746,91,-746,40,-746,25,-746,97,-746,54,-746,35,-746,55,-746,102,-746,47,-746,36,-746,53,-746,60,-746,74,-746,72,-746,38,-746,71,-746,138,-746,136,-746,118,-746,117,-746,131,-746,132,-746,133,-746,134,-746,130,-746},new int[]{-203,36});
    states[172] = new State(-751);
    states[173] = new State(-752);
    states[174] = new State(-753);
    states[175] = new State(-754);
    states[176] = new State(-755);
    states[177] = new State(new int[]{138,178,136,1512,118,1515,117,1516,131,1517,132,1518,133,1519,134,1520,130,1521,116,-748,115,-748,128,-748,129,-748,126,-748,6,-748,5,-748,120,-748,125,-748,123,-748,121,-748,124,-748,122,-748,137,-748,135,-748,16,-748,70,-748,92,-748,10,-748,98,-748,101,-748,33,-748,104,-748,2,-748,9,-748,12,-748,100,-748,99,-748,32,-748,85,-748,84,-748,83,-748,82,-748,81,-748,86,-748,13,-748,76,-748,51,-748,58,-748,141,-748,143,-748,161,-748,80,-748,78,-748,160,-748,87,-748,45,-748,42,-748,8,-748,21,-748,22,-748,144,-748,147,-748,145,-748,146,-748,155,-748,158,-748,157,-748,156,-748,11,-748,57,-748,91,-748,40,-748,25,-748,97,-748,54,-748,35,-748,55,-748,102,-748,47,-748,36,-748,53,-748,60,-748,74,-748,72,-748,38,-748,71,-748},new int[]{-204,38});
    states[178] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,24,494},new int[]{-290,179,-285,180,-186,181,-152,218,-156,49,-157,52,-277,500});
    states[179] = new State(-762);
    states[180] = new State(-763);
    states[181] = new State(new int[]{7,182,4,185,123,187,8,-649,138,-649,136,-649,118,-649,117,-649,131,-649,132,-649,133,-649,134,-649,130,-649,116,-649,115,-649,128,-649,129,-649,126,-649,6,-649,5,-649,120,-649,125,-649,121,-649,124,-649,122,-649,137,-649,135,-649,16,-649,70,-649,92,-649,10,-649,98,-649,101,-649,33,-649,104,-649,2,-649,9,-649,12,-649,100,-649,99,-649,32,-649,85,-649,84,-649,83,-649,82,-649,81,-649,86,-649,13,-649,76,-649,51,-649,58,-649,141,-649,143,-649,161,-649,80,-649,78,-649,160,-649,87,-649,45,-649,42,-649,21,-649,22,-649,144,-649,147,-649,145,-649,146,-649,155,-649,158,-649,157,-649,156,-649,11,-649,57,-649,91,-649,40,-649,25,-649,97,-649,54,-649,35,-649,55,-649,102,-649,47,-649,36,-649,53,-649,60,-649,74,-649,72,-649,38,-649,71,-649},new int[]{-305,184});
    states[182] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-143,183,-152,46,-156,49,-157,52,-299,57,-155,58,-300,148});
    states[183] = new State(-283);
    states[184] = new State(-650);
    states[185] = new State(new int[]{123,187},new int[]{-305,186});
    states[186] = new State(-651);
    states[187] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-303,188,-286,313,-279,192,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-288,1470,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,1471,-230,561,-229,562,-307,1472});
    states[188] = new State(new int[]{121,189,100,190});
    states[189] = new State(-257);
    states[190] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,191,-279,192,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-288,1470,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,1471,-230,561,-229,562,-307,1472});
    states[191] = new State(-261);
    states[192] = new State(new int[]{13,193,121,-265,100,-265,120,-265,9,-265,10,-265,8,-265,138,-265,136,-265,118,-265,117,-265,131,-265,132,-265,133,-265,134,-265,130,-265,116,-265,115,-265,128,-265,129,-265,126,-265,6,-265,5,-265,125,-265,123,-265,124,-265,122,-265,137,-265,135,-265,16,-265,70,-265,92,-265,98,-265,101,-265,33,-265,104,-265,2,-265,12,-265,99,-265,32,-265,85,-265,84,-265,83,-265,82,-265,81,-265,86,-265,76,-265,51,-265,58,-265,141,-265,143,-265,161,-265,80,-265,78,-265,160,-265,87,-265,45,-265,42,-265,21,-265,22,-265,144,-265,147,-265,145,-265,146,-265,155,-265,158,-265,157,-265,156,-265,11,-265,57,-265,91,-265,40,-265,25,-265,97,-265,54,-265,35,-265,55,-265,102,-265,47,-265,36,-265,53,-265,60,-265,74,-265,72,-265,38,-265,71,-265,127,-265,110,-265});
    states[193] = new State(-266);
    states[194] = new State(new int[]{6,1510,116,247,115,248,128,249,129,250,13,-270,121,-270,100,-270,120,-270,9,-270,10,-270,8,-270,138,-270,136,-270,118,-270,117,-270,131,-270,132,-270,133,-270,134,-270,130,-270,126,-270,5,-270,125,-270,123,-270,124,-270,122,-270,137,-270,135,-270,16,-270,70,-270,92,-270,98,-270,101,-270,33,-270,104,-270,2,-270,12,-270,99,-270,32,-270,85,-270,84,-270,83,-270,82,-270,81,-270,86,-270,76,-270,51,-270,58,-270,141,-270,143,-270,161,-270,80,-270,78,-270,160,-270,87,-270,45,-270,42,-270,21,-270,22,-270,144,-270,147,-270,145,-270,146,-270,155,-270,158,-270,157,-270,156,-270,11,-270,57,-270,91,-270,40,-270,25,-270,97,-270,54,-270,35,-270,55,-270,102,-270,47,-270,36,-270,53,-270,60,-270,74,-270,72,-270,38,-270,71,-270,127,-270,110,-270},new int[]{-199,195});
    states[195] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285},new int[]{-111,196,-112,315,-186,352,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283});
    states[196] = new State(new int[]{118,254,117,255,131,256,132,257,133,258,134,259,130,260,6,-274,116,-274,115,-274,128,-274,129,-274,13,-274,121,-274,100,-274,120,-274,9,-274,10,-274,8,-274,138,-274,136,-274,126,-274,5,-274,125,-274,123,-274,124,-274,122,-274,137,-274,135,-274,16,-274,70,-274,92,-274,98,-274,101,-274,33,-274,104,-274,2,-274,12,-274,99,-274,32,-274,85,-274,84,-274,83,-274,82,-274,81,-274,86,-274,76,-274,51,-274,58,-274,141,-274,143,-274,161,-274,80,-274,78,-274,160,-274,87,-274,45,-274,42,-274,21,-274,22,-274,144,-274,147,-274,145,-274,146,-274,155,-274,158,-274,157,-274,156,-274,11,-274,57,-274,91,-274,40,-274,25,-274,97,-274,54,-274,35,-274,55,-274,102,-274,47,-274,36,-274,53,-274,60,-274,74,-274,72,-274,38,-274,71,-274,127,-274,110,-274},new int[]{-201,197});
    states[197] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285},new int[]{-112,198,-186,352,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283});
    states[198] = new State(new int[]{8,199,118,-276,117,-276,131,-276,132,-276,133,-276,134,-276,130,-276,6,-276,116,-276,115,-276,128,-276,129,-276,13,-276,121,-276,100,-276,120,-276,9,-276,10,-276,138,-276,136,-276,126,-276,5,-276,125,-276,123,-276,124,-276,122,-276,137,-276,135,-276,16,-276,70,-276,92,-276,98,-276,101,-276,33,-276,104,-276,2,-276,12,-276,99,-276,32,-276,85,-276,84,-276,83,-276,82,-276,81,-276,86,-276,76,-276,51,-276,58,-276,141,-276,143,-276,161,-276,80,-276,78,-276,160,-276,87,-276,45,-276,42,-276,21,-276,22,-276,144,-276,147,-276,145,-276,146,-276,155,-276,158,-276,157,-276,156,-276,11,-276,57,-276,91,-276,40,-276,25,-276,97,-276,54,-276,35,-276,55,-276,102,-276,47,-276,36,-276,53,-276,60,-276,74,-276,72,-276,38,-276,71,-276,127,-276,110,-276});
    states[199] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,9,-207},new int[]{-80,200,-78,202,-99,1509,-95,205,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[200] = new State(new int[]{9,201});
    states[201] = new State(-281);
    states[202] = new State(new int[]{100,203,9,-206,12,-206});
    states[203] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-99,204,-95,205,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[204] = new State(-209);
    states[205] = new State(new int[]{13,206,16,210,6,1503,100,-210,9,-210,12,-210,5,-210});
    states[206] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,207,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[207] = new State(new int[]{5,208,13,206,16,210});
    states[208] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,209,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[209] = new State(new int[]{13,206,16,210,6,-144,100,-144,9,-144,12,-144,5,-144,92,-144,10,-144,98,-144,101,-144,33,-144,104,-144,2,-144,99,-144,32,-144,85,-144,84,-144,83,-144,82,-144,81,-144,86,-144});
    states[210] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-96,211,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849});
    states[211] = new State(new int[]{120,237,125,238,123,239,121,240,124,241,122,242,137,243,135,244,13,-143,16,-143,6,-143,100,-143,9,-143,12,-143,5,-143,92,-143,10,-143,98,-143,101,-143,33,-143,104,-143,2,-143,99,-143,32,-143,85,-143,84,-143,83,-143,82,-143,81,-143,86,-143},new int[]{-198,212});
    states[212] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-86,213,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849});
    states[213] = new State(new int[]{116,247,115,248,128,249,129,250,120,-140,125,-140,123,-140,121,-140,124,-140,122,-140,137,-140,135,-140,13,-140,16,-140,6,-140,100,-140,9,-140,12,-140,5,-140,92,-140,10,-140,98,-140,101,-140,33,-140,104,-140,2,-140,99,-140,32,-140,85,-140,84,-140,83,-140,82,-140,81,-140,86,-140},new int[]{-199,214});
    states[214] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-18,215,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849});
    states[215] = new State(new int[]{136,252,138,253,118,254,117,255,131,256,132,257,133,258,134,259,130,260,116,-154,115,-154,128,-154,129,-154,120,-154,125,-154,123,-154,121,-154,124,-154,122,-154,137,-154,135,-154,13,-154,16,-154,6,-154,100,-154,9,-154,12,-154,5,-154,92,-154,10,-154,98,-154,101,-154,33,-154,104,-154,2,-154,99,-154,32,-154,85,-154,84,-154,83,-154,82,-154,81,-154,86,-154},new int[]{-207,216,-201,219});
    states[216] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-290,217,-186,181,-152,218,-156,49,-157,52});
    states[217] = new State(-159);
    states[218] = new State(-282);
    states[219] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-15,220,-276,221,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-16,849});
    states[220] = new State(-166);
    states[221] = new State(-167);
    states[222] = new State(new int[]{4,224,11,226,7,826,142,828,8,829,136,-177,138,-177,118,-177,117,-177,131,-177,132,-177,133,-177,134,-177,130,-177,116,-177,115,-177,128,-177,129,-177,120,-177,125,-177,123,-177,121,-177,124,-177,122,-177,137,-177,135,-177,13,-177,16,-177,6,-177,100,-177,9,-177,12,-177,5,-177,92,-177,10,-177,98,-177,101,-177,33,-177,104,-177,2,-177,99,-177,32,-177,85,-177,84,-177,83,-177,82,-177,81,-177,86,-177,119,-175},new int[]{-17,223});
    states[223] = new State(-197);
    states[224] = new State(new int[]{123,187},new int[]{-305,225});
    states[225] = new State(-198);
    states[226] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,5,1505,12,-207},new int[]{-126,227,-80,229,-95,231,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855,-78,202,-99,1509});
    states[227] = new State(new int[]{12,228});
    states[228] = new State(-199);
    states[229] = new State(new int[]{12,230});
    states[230] = new State(-203);
    states[231] = new State(new int[]{5,232,13,206,16,210,6,1503,100,-210,12,-210});
    states[232] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,5,-729,12,-729},new int[]{-127,233,-95,1502,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[233] = new State(new int[]{5,234,12,-734});
    states[234] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,235,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[235] = new State(new int[]{13,206,16,210,12,-736});
    states[236] = new State(new int[]{120,237,125,238,123,239,121,240,124,241,122,242,137,243,135,244,13,-141,16,-141,6,-141,100,-141,9,-141,12,-141,5,-141,92,-141,10,-141,98,-141,101,-141,33,-141,104,-141,2,-141,99,-141,32,-141,85,-141,84,-141,83,-141,82,-141,81,-141,86,-141},new int[]{-198,212});
    states[237] = new State(-145);
    states[238] = new State(-146);
    states[239] = new State(-147);
    states[240] = new State(-148);
    states[241] = new State(-149);
    states[242] = new State(-150);
    states[243] = new State(-151);
    states[244] = new State(new int[]{137,245});
    states[245] = new State(-152);
    states[246] = new State(new int[]{116,247,115,248,128,249,129,250,120,-139,125,-139,123,-139,121,-139,124,-139,122,-139,137,-139,135,-139,13,-139,16,-139,6,-139,100,-139,9,-139,12,-139,5,-139,92,-139,10,-139,98,-139,101,-139,33,-139,104,-139,2,-139,99,-139,32,-139,85,-139,84,-139,83,-139,82,-139,81,-139,86,-139},new int[]{-199,214});
    states[247] = new State(-155);
    states[248] = new State(-156);
    states[249] = new State(-157);
    states[250] = new State(-158);
    states[251] = new State(new int[]{136,252,138,253,118,254,117,255,131,256,132,257,133,258,134,259,130,260,116,-153,115,-153,128,-153,129,-153,120,-153,125,-153,123,-153,121,-153,124,-153,122,-153,137,-153,135,-153,13,-153,16,-153,6,-153,100,-153,9,-153,12,-153,5,-153,92,-153,10,-153,98,-153,101,-153,33,-153,104,-153,2,-153,99,-153,32,-153,85,-153,84,-153,83,-153,82,-153,81,-153,86,-153},new int[]{-207,216,-201,219});
    states[252] = new State(-756);
    states[253] = new State(-757);
    states[254] = new State(-168);
    states[255] = new State(-169);
    states[256] = new State(-170);
    states[257] = new State(-171);
    states[258] = new State(-172);
    states[259] = new State(-173);
    states[260] = new State(-174);
    states[261] = new State(-163);
    states[262] = new State(-191);
    states[263] = new State(new int[]{26,1491,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,-881,17,-881,8,-881,7,-881,142,-881,4,-881,15,-881,110,-881,111,-881,112,-881,113,-881,114,-881,92,-881,10,-881,5,-881,98,-881,101,-881,33,-881,104,-881,2,-881,127,-881,138,-881,136,-881,118,-881,117,-881,131,-881,132,-881,133,-881,134,-881,130,-881,116,-881,115,-881,128,-881,129,-881,126,-881,6,-881,120,-881,125,-881,123,-881,121,-881,124,-881,122,-881,137,-881,135,-881,16,-881,70,-881,9,-881,12,-881,100,-881,99,-881,32,-881,84,-881,83,-881,82,-881,81,-881,13,-881,119,-881,76,-881,51,-881,58,-881,141,-881,45,-881,42,-881,21,-881,22,-881,144,-881,147,-881,145,-881,146,-881,155,-881,158,-881,157,-881,156,-881,57,-881,91,-881,40,-881,25,-881,97,-881,54,-881,35,-881,55,-881,102,-881,47,-881,36,-881,53,-881,60,-881,74,-881,72,-881,38,-881,71,-881},new int[]{-290,264,-186,181,-152,218,-156,49,-157,52});
    states[264] = new State(new int[]{11,266,8,1488,92,-665,10,-665,98,-665,101,-665,33,-665,104,-665,2,-665,138,-665,136,-665,118,-665,117,-665,131,-665,132,-665,133,-665,134,-665,130,-665,116,-665,115,-665,128,-665,129,-665,126,-665,6,-665,5,-665,120,-665,125,-665,123,-665,121,-665,124,-665,122,-665,137,-665,135,-665,16,-665,70,-665,9,-665,12,-665,100,-665,99,-665,32,-665,85,-665,84,-665,83,-665,82,-665,81,-665,86,-665,13,-665,76,-665,51,-665,58,-665,141,-665,143,-665,161,-665,80,-665,78,-665,160,-665,87,-665,45,-665,42,-665,21,-665,22,-665,144,-665,147,-665,145,-665,146,-665,155,-665,158,-665,157,-665,156,-665,57,-665,91,-665,40,-665,25,-665,97,-665,54,-665,35,-665,55,-665,102,-665,47,-665,36,-665,53,-665,60,-665,74,-665,72,-665,38,-665,71,-665},new int[]{-75,265});
    states[265] = new State(-656);
    states[266] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,12,-833},new int[]{-71,267,-76,640,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,834,-105,659,-331,677});
    states[267] = new State(new int[]{12,268});
    states[268] = new State(new int[]{8,270,92,-655,10,-655,98,-655,101,-655,33,-655,104,-655,2,-655,138,-655,136,-655,118,-655,117,-655,131,-655,132,-655,133,-655,134,-655,130,-655,116,-655,115,-655,128,-655,129,-655,126,-655,6,-655,5,-655,120,-655,125,-655,123,-655,121,-655,124,-655,122,-655,137,-655,135,-655,16,-655,70,-655,9,-655,12,-655,100,-655,99,-655,32,-655,85,-655,84,-655,83,-655,82,-655,81,-655,86,-655,13,-655,76,-655,51,-655,58,-655,141,-655,143,-655,161,-655,80,-655,78,-655,160,-655,87,-655,45,-655,42,-655,21,-655,22,-655,144,-655,147,-655,145,-655,146,-655,155,-655,158,-655,157,-655,156,-655,11,-655,57,-655,91,-655,40,-655,25,-655,97,-655,54,-655,35,-655,55,-655,102,-655,47,-655,36,-655,53,-655,60,-655,74,-655,72,-655,38,-655,71,-655},new int[]{-10,269});
    states[269] = new State(-657);
    states[270] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339,9,-220},new int[]{-70,271,-69,273,-90,1003,-89,276,-95,277,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855,-100,1004,-250,1005});
    states[271] = new State(new int[]{9,272});
    states[272] = new State(-654);
    states[273] = new State(new int[]{100,274,9,-221});
    states[274] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339},new int[]{-90,275,-89,276,-95,277,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855,-100,1004,-250,1005});
    states[275] = new State(-223);
    states[276] = new State(-439);
    states[277] = new State(new int[]{13,206,16,210,100,-216,9,-216,92,-216,10,-216,98,-216,101,-216,33,-216,104,-216,2,-216,12,-216,99,-216,32,-216,85,-216,84,-216,83,-216,82,-216,81,-216,86,-216});
    states[278] = new State(-192);
    states[279] = new State(new int[]{144,281,147,282,7,-844,11,-844,17,-844,138,-844,136,-844,118,-844,117,-844,131,-844,132,-844,133,-844,134,-844,130,-844,116,-844,115,-844,128,-844,129,-844,126,-844,6,-844,5,-844,120,-844,125,-844,123,-844,121,-844,124,-844,122,-844,137,-844,135,-844,16,-844,70,-844,92,-844,10,-844,98,-844,101,-844,33,-844,104,-844,2,-844,9,-844,12,-844,100,-844,99,-844,32,-844,85,-844,84,-844,83,-844,82,-844,81,-844,86,-844,13,-844,119,-844,76,-844,51,-844,58,-844,141,-844,143,-844,161,-844,80,-844,78,-844,160,-844,87,-844,45,-844,42,-844,8,-844,21,-844,22,-844,145,-844,146,-844,155,-844,158,-844,157,-844,156,-844,57,-844,91,-844,40,-844,25,-844,97,-844,54,-844,35,-844,55,-844,102,-844,47,-844,36,-844,53,-844,60,-844,74,-844,72,-844,38,-844,71,-844,127,-844,110,-844,4,-844,142,-844},new int[]{-172,280});
    states[280] = new State(-848);
    states[281] = new State(-842);
    states[282] = new State(-843);
    states[283] = new State(-847);
    states[284] = new State(-845);
    states[285] = new State(-846);
    states[286] = new State(-193);
    states[287] = new State(-212);
    states[288] = new State(-213);
    states[289] = new State(-214);
    states[290] = new State(-215);
    states[291] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,292,-156,49,-157,52});
    states[292] = new State(-194);
    states[293] = new State(-195);
    states[294] = new State(new int[]{8,295});
    states[295] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-290,296,-186,181,-152,218,-156,49,-157,52});
    states[296] = new State(new int[]{9,297});
    states[297] = new State(-642);
    states[298] = new State(-196);
    states[299] = new State(new int[]{8,300});
    states[300] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-290,301,-289,303,-186,305,-152,218,-156,49,-157,52});
    states[301] = new State(new int[]{9,302});
    states[302] = new State(-643);
    states[303] = new State(new int[]{9,304});
    states[304] = new State(-644);
    states[305] = new State(new int[]{7,182,4,306,123,308,125,1486,9,-649},new int[]{-305,184,-306,1487});
    states[306] = new State(new int[]{123,308,125,1486},new int[]{-305,186,-306,307});
    states[307] = new State(-648);
    states[308] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,24,494,48,502,49,549,34,553,73,557,44,563,37,604,121,-264,100,-264},new int[]{-303,188,-304,309,-286,313,-279,192,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-288,1470,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,1471,-230,561,-229,562,-307,1472,-287,1485});
    states[309] = new State(new int[]{121,310,100,311});
    states[310] = new State(-259);
    states[311] = new State(-264,new int[]{-287,312});
    states[312] = new State(-263);
    states[313] = new State(-260);
    states[314] = new State(new int[]{118,254,117,255,131,256,132,257,133,258,134,259,130,260,6,-273,116,-273,115,-273,128,-273,129,-273,13,-273,121,-273,100,-273,120,-273,9,-273,10,-273,8,-273,138,-273,136,-273,126,-273,5,-273,125,-273,123,-273,124,-273,122,-273,137,-273,135,-273,16,-273,70,-273,92,-273,98,-273,101,-273,33,-273,104,-273,2,-273,12,-273,99,-273,32,-273,85,-273,84,-273,83,-273,82,-273,81,-273,86,-273,76,-273,51,-273,58,-273,141,-273,143,-273,161,-273,80,-273,78,-273,160,-273,87,-273,45,-273,42,-273,21,-273,22,-273,144,-273,147,-273,145,-273,146,-273,155,-273,158,-273,157,-273,156,-273,11,-273,57,-273,91,-273,40,-273,25,-273,97,-273,54,-273,35,-273,55,-273,102,-273,47,-273,36,-273,53,-273,60,-273,74,-273,72,-273,38,-273,71,-273,127,-273,110,-273},new int[]{-201,197});
    states[315] = new State(new int[]{8,199,118,-275,117,-275,131,-275,132,-275,133,-275,134,-275,130,-275,6,-275,116,-275,115,-275,128,-275,129,-275,13,-275,121,-275,100,-275,120,-275,9,-275,10,-275,138,-275,136,-275,126,-275,5,-275,125,-275,123,-275,124,-275,122,-275,137,-275,135,-275,16,-275,70,-275,92,-275,98,-275,101,-275,33,-275,104,-275,2,-275,12,-275,99,-275,32,-275,85,-275,84,-275,83,-275,82,-275,81,-275,86,-275,76,-275,51,-275,58,-275,141,-275,143,-275,161,-275,80,-275,78,-275,160,-275,87,-275,45,-275,42,-275,21,-275,22,-275,144,-275,147,-275,145,-275,146,-275,155,-275,158,-275,157,-275,156,-275,11,-275,57,-275,91,-275,40,-275,25,-275,97,-275,54,-275,35,-275,55,-275,102,-275,47,-275,36,-275,53,-275,60,-275,74,-275,72,-275,38,-275,71,-275,127,-275,110,-275});
    states[316] = new State(new int[]{7,182,127,317,123,187,8,-277,118,-277,117,-277,131,-277,132,-277,133,-277,134,-277,130,-277,6,-277,116,-277,115,-277,128,-277,129,-277,13,-277,121,-277,100,-277,120,-277,9,-277,10,-277,138,-277,136,-277,126,-277,5,-277,125,-277,124,-277,122,-277,137,-277,135,-277,16,-277,70,-277,92,-277,98,-277,101,-277,33,-277,104,-277,2,-277,12,-277,99,-277,32,-277,85,-277,84,-277,83,-277,82,-277,81,-277,86,-277,76,-277,51,-277,58,-277,141,-277,143,-277,161,-277,80,-277,78,-277,160,-277,87,-277,45,-277,42,-277,21,-277,22,-277,144,-277,147,-277,145,-277,146,-277,155,-277,158,-277,157,-277,156,-277,11,-277,57,-277,91,-277,40,-277,25,-277,97,-277,54,-277,35,-277,55,-277,102,-277,47,-277,36,-277,53,-277,60,-277,74,-277,72,-277,38,-277,71,-277,110,-277},new int[]{-305,636});
    states[317] = new State(new int[]{8,319,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,318,-279,192,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-288,1470,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,1471,-230,561,-229,562,-307,1472});
    states[318] = new State(-310);
    states[319] = new State(new int[]{9,320,143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,325,-83,331,-283,332,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[320] = new State(new int[]{127,321,121,-314,100,-314,120,-314,9,-314,10,-314,8,-314,138,-314,136,-314,118,-314,117,-314,131,-314,132,-314,133,-314,134,-314,130,-314,116,-314,115,-314,128,-314,129,-314,126,-314,6,-314,5,-314,125,-314,123,-314,124,-314,122,-314,137,-314,135,-314,16,-314,70,-314,92,-314,98,-314,101,-314,33,-314,104,-314,2,-314,12,-314,99,-314,32,-314,85,-314,84,-314,83,-314,82,-314,81,-314,86,-314,13,-314,76,-314,51,-314,58,-314,141,-314,143,-314,161,-314,80,-314,78,-314,160,-314,87,-314,45,-314,42,-314,21,-314,22,-314,144,-314,147,-314,145,-314,146,-314,155,-314,158,-314,157,-314,156,-314,11,-314,57,-314,91,-314,40,-314,25,-314,97,-314,54,-314,35,-314,55,-314,102,-314,47,-314,36,-314,53,-314,60,-314,74,-314,72,-314,38,-314,71,-314,110,-314});
    states[321] = new State(new int[]{8,323,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,322,-279,192,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-288,1470,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,1471,-230,561,-229,562,-307,1472});
    states[322] = new State(-312);
    states[323] = new State(new int[]{9,324,143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,325,-83,331,-283,332,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[324] = new State(new int[]{127,321,121,-316,100,-316,120,-316,9,-316,10,-316,8,-316,138,-316,136,-316,118,-316,117,-316,131,-316,132,-316,133,-316,134,-316,130,-316,116,-316,115,-316,128,-316,129,-316,126,-316,6,-316,5,-316,125,-316,123,-316,124,-316,122,-316,137,-316,135,-316,16,-316,70,-316,92,-316,98,-316,101,-316,33,-316,104,-316,2,-316,12,-316,99,-316,32,-316,85,-316,84,-316,83,-316,82,-316,81,-316,86,-316,13,-316,76,-316,51,-316,58,-316,141,-316,143,-316,161,-316,80,-316,78,-316,160,-316,87,-316,45,-316,42,-316,21,-316,22,-316,144,-316,147,-316,145,-316,146,-316,155,-316,158,-316,157,-316,156,-316,11,-316,57,-316,91,-316,40,-316,25,-316,97,-316,54,-316,35,-316,55,-316,102,-316,47,-316,36,-316,53,-316,60,-316,74,-316,72,-316,38,-316,71,-316,110,-316});
    states[325] = new State(new int[]{9,326,100,359});
    states[326] = new State(new int[]{127,327,13,-272,121,-272,100,-272,120,-272,9,-272,10,-272,8,-272,138,-272,136,-272,118,-272,117,-272,131,-272,132,-272,133,-272,134,-272,130,-272,116,-272,115,-272,128,-272,129,-272,126,-272,6,-272,5,-272,125,-272,123,-272,124,-272,122,-272,137,-272,135,-272,16,-272,70,-272,92,-272,98,-272,101,-272,33,-272,104,-272,2,-272,12,-272,99,-272,32,-272,85,-272,84,-272,83,-272,82,-272,81,-272,86,-272,76,-272,51,-272,58,-272,141,-272,143,-272,161,-272,80,-272,78,-272,160,-272,87,-272,45,-272,42,-272,21,-272,22,-272,144,-272,147,-272,145,-272,146,-272,155,-272,158,-272,157,-272,156,-272,11,-272,57,-272,91,-272,40,-272,25,-272,97,-272,54,-272,35,-272,55,-272,102,-272,47,-272,36,-272,53,-272,60,-272,74,-272,72,-272,38,-272,71,-272,110,-272});
    states[327] = new State(new int[]{8,329,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,328,-279,192,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-288,1470,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,1471,-230,561,-229,562,-307,1472});
    states[328] = new State(-313);
    states[329] = new State(new int[]{9,330,143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,325,-83,331,-283,332,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[330] = new State(new int[]{127,321,121,-317,100,-317,120,-317,9,-317,10,-317,8,-317,138,-317,136,-317,118,-317,117,-317,131,-317,132,-317,133,-317,134,-317,130,-317,116,-317,115,-317,128,-317,129,-317,126,-317,6,-317,5,-317,125,-317,123,-317,124,-317,122,-317,137,-317,135,-317,16,-317,70,-317,92,-317,98,-317,101,-317,33,-317,104,-317,2,-317,12,-317,99,-317,32,-317,85,-317,84,-317,83,-317,82,-317,81,-317,86,-317,13,-317,76,-317,51,-317,58,-317,141,-317,143,-317,161,-317,80,-317,78,-317,160,-317,87,-317,45,-317,42,-317,21,-317,22,-317,144,-317,147,-317,145,-317,146,-317,155,-317,158,-317,157,-317,156,-317,11,-317,57,-317,91,-317,40,-317,25,-317,97,-317,54,-317,35,-317,55,-317,102,-317,47,-317,36,-317,53,-317,60,-317,74,-317,72,-317,38,-317,71,-317,110,-317});
    states[331] = new State(-284);
    states[332] = new State(new int[]{120,333,9,-286,100,-286});
    states[333] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,334,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[334] = new State(new int[]{70,27,9,-287,100,-287});
    states[335] = new State(-767);
    states[336] = new State(-793);
    states[337] = new State(-794);
    states[338] = new State(-787);
    states[339] = new State(new int[]{8,340});
    states[340] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,356},new int[]{-291,341,-290,343,-186,344,-152,218,-156,49,-157,52,-280,346,-279,347,-98,194,-111,314,-112,315,-21,349,-205,350,-171,355,-173,279,-172,283,-307,1484});
    states[341] = new State(new int[]{9,342});
    states[342] = new State(-781);
    states[343] = new State(-652);
    states[344] = new State(new int[]{7,182,4,185,123,187,9,-649,8,-277,118,-277,117,-277,131,-277,132,-277,133,-277,134,-277,130,-277,6,-277,116,-277,115,-277,128,-277,129,-277,13,-277},new int[]{-305,345});
    states[345] = new State(new int[]{9,-650,13,-256});
    states[346] = new State(-653);
    states[347] = new State(new int[]{13,348});
    states[348] = new State(-247);
    states[349] = new State(-278);
    states[350] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285},new int[]{-112,351,-186,352,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283});
    states[351] = new State(new int[]{8,199,118,-279,117,-279,131,-279,132,-279,133,-279,134,-279,130,-279,6,-279,116,-279,115,-279,128,-279,129,-279,13,-279,121,-279,100,-279,120,-279,9,-279,10,-279,138,-279,136,-279,126,-279,5,-279,125,-279,123,-279,124,-279,122,-279,137,-279,135,-279,16,-279,70,-279,92,-279,98,-279,101,-279,33,-279,104,-279,2,-279,12,-279,99,-279,32,-279,85,-279,84,-279,83,-279,82,-279,81,-279,86,-279,76,-279,51,-279,58,-279,141,-279,143,-279,161,-279,80,-279,78,-279,160,-279,87,-279,45,-279,42,-279,21,-279,22,-279,144,-279,147,-279,145,-279,146,-279,155,-279,158,-279,157,-279,156,-279,11,-279,57,-279,91,-279,40,-279,25,-279,97,-279,54,-279,35,-279,55,-279,102,-279,47,-279,36,-279,53,-279,60,-279,74,-279,72,-279,38,-279,71,-279,127,-279,110,-279});
    states[352] = new State(new int[]{7,182,8,-277,118,-277,117,-277,131,-277,132,-277,133,-277,134,-277,130,-277,6,-277,116,-277,115,-277,128,-277,129,-277,13,-277,121,-277,100,-277,120,-277,9,-277,10,-277,138,-277,136,-277,126,-277,5,-277,125,-277,123,-277,124,-277,122,-277,137,-277,135,-277,16,-277,70,-277,92,-277,98,-277,101,-277,33,-277,104,-277,2,-277,12,-277,99,-277,32,-277,85,-277,84,-277,83,-277,82,-277,81,-277,86,-277,76,-277,51,-277,58,-277,141,-277,143,-277,161,-277,80,-277,78,-277,160,-277,87,-277,45,-277,42,-277,21,-277,22,-277,144,-277,147,-277,145,-277,146,-277,155,-277,158,-277,157,-277,156,-277,11,-277,57,-277,91,-277,40,-277,25,-277,97,-277,54,-277,35,-277,55,-277,102,-277,47,-277,36,-277,53,-277,60,-277,74,-277,72,-277,38,-277,71,-277,127,-277,110,-277});
    states[353] = new State(-189);
    states[354] = new State(-190);
    states[355] = new State(-280);
    states[356] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,357,-83,331,-283,332,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[357] = new State(new int[]{9,358,100,359});
    states[358] = new State(-272);
    states[359] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-83,360,-283,332,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[360] = new State(-285);
    states[361] = new State(new int[]{13,348,120,-249,9,-249,100,-249,10,-249,121,-249,8,-249,138,-249,136,-249,118,-249,117,-249,131,-249,132,-249,133,-249,134,-249,130,-249,116,-249,115,-249,128,-249,129,-249,126,-249,6,-249,5,-249,125,-249,123,-249,124,-249,122,-249,137,-249,135,-249,16,-249,70,-249,92,-249,98,-249,101,-249,33,-249,104,-249,2,-249,12,-249,99,-249,32,-249,85,-249,84,-249,83,-249,82,-249,81,-249,86,-249,76,-249,51,-249,58,-249,141,-249,143,-249,161,-249,80,-249,78,-249,160,-249,87,-249,45,-249,42,-249,21,-249,22,-249,144,-249,147,-249,145,-249,146,-249,155,-249,158,-249,157,-249,156,-249,11,-249,57,-249,91,-249,40,-249,25,-249,97,-249,54,-249,35,-249,55,-249,102,-249,47,-249,36,-249,53,-249,60,-249,74,-249,72,-249,38,-249,71,-249,127,-249,110,-249});
    states[362] = new State(new int[]{11,363,7,-856,127,-856,123,-856,8,-856,118,-856,117,-856,131,-856,132,-856,133,-856,134,-856,130,-856,6,-856,116,-856,115,-856,128,-856,129,-856,13,-856,120,-856,9,-856,100,-856,10,-856,121,-856,138,-856,136,-856,126,-856,5,-856,125,-856,124,-856,122,-856,137,-856,135,-856,16,-856,70,-856,92,-856,98,-856,101,-856,33,-856,104,-856,2,-856,12,-856,99,-856,32,-856,85,-856,84,-856,83,-856,82,-856,81,-856,86,-856,76,-856,51,-856,58,-856,141,-856,143,-856,161,-856,80,-856,78,-856,160,-856,87,-856,45,-856,42,-856,21,-856,22,-856,144,-856,147,-856,145,-856,146,-856,155,-856,158,-856,157,-856,156,-856,57,-856,91,-856,40,-856,25,-856,97,-856,54,-856,35,-856,55,-856,102,-856,47,-856,36,-856,53,-856,60,-856,74,-856,72,-856,38,-856,71,-856,110,-856});
    states[363] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,364,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[364] = new State(new int[]{12,365,13,206,16,210});
    states[365] = new State(-305);
    states[366] = new State(-178);
    states[367] = new State(-187);
    states[368] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,12,-837},new int[]{-73,369,-82,371,-97,449,-92,374,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[369] = new State(new int[]{12,370});
    states[370] = new State(-186);
    states[371] = new State(new int[]{100,372,12,-836,76,-836});
    states[372] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-97,373,-92,374,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[373] = new State(-839);
    states[374] = new State(new int[]{70,27,6,375,100,-840,12,-840,76,-840});
    states[375] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,376,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[376] = new State(new int[]{70,27,100,-841,12,-841,76,-841});
    states[377] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-101,378,-20,379,-171,336,-173,279,-172,283,-21,337,-61,338,-205,389,-118,390,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458});
    states[378] = new State(-788);
    states[379] = new State(new int[]{7,44,11,150,17,380,138,-786,136,-786,118,-786,117,-786,131,-786,132,-786,133,-786,134,-786,130,-786,116,-786,115,-786,128,-786,129,-786,126,-786,6,-786,5,-786,120,-786,125,-786,123,-786,121,-786,124,-786,122,-786,137,-786,135,-786,16,-786,70,-786,92,-786,10,-786,98,-786,101,-786,33,-786,104,-786,2,-786,9,-786,12,-786,100,-786,99,-786,32,-786,85,-786,84,-786,83,-786,82,-786,81,-786,86,-786,13,-786,76,-786,51,-786,58,-786,141,-786,143,-786,161,-786,80,-786,78,-786,160,-786,87,-786,45,-786,42,-786,8,-786,21,-786,22,-786,144,-786,147,-786,145,-786,146,-786,155,-786,158,-786,157,-786,156,-786,57,-786,91,-786,40,-786,25,-786,97,-786,54,-786,35,-786,55,-786,102,-786,47,-786,36,-786,53,-786,60,-786,74,-786,72,-786,38,-786,71,-786});
    states[380] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,5,598},new int[]{-125,381,-110,383,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,602,-274,578});
    states[381] = new State(new int[]{12,382});
    states[382] = new State(-824);
    states[383] = new State(new int[]{5,167,6,34});
    states[384] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-101,385,-275,386,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-102,461});
    states[385] = new State(-789);
    states[386] = new State(-766);
    states[387] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-101,388,-20,379,-171,336,-173,279,-172,283,-21,337,-61,338,-205,389,-118,390,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458});
    states[388] = new State(-790);
    states[389] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-101,385,-20,379,-171,336,-173,279,-172,283,-21,337,-61,338,-205,389,-118,390,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458});
    states[390] = new State(-791);
    states[391] = new State(new int[]{141,1483,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,1113,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368},new int[]{-116,392,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728});
    states[392] = new State(new int[]{11,393,17,400,8,702,7,1108,142,1110,4,1111,110,-797,111,-797,112,-797,113,-797,114,-797,92,-797,10,-797,98,-797,101,-797,33,-797,104,-797,2,-797,138,-797,136,-797,118,-797,117,-797,131,-797,132,-797,133,-797,134,-797,130,-797,116,-797,115,-797,128,-797,129,-797,126,-797,6,-797,5,-797,120,-797,125,-797,123,-797,121,-797,124,-797,122,-797,137,-797,135,-797,16,-797,70,-797,9,-797,12,-797,100,-797,99,-797,32,-797,85,-797,84,-797,83,-797,82,-797,81,-797,86,-797,13,-797,119,-797,76,-797,51,-797,58,-797,141,-797,143,-797,161,-797,80,-797,78,-797,160,-797,87,-797,45,-797,42,-797,21,-797,22,-797,144,-797,147,-797,145,-797,146,-797,155,-797,158,-797,157,-797,156,-797,57,-797,91,-797,40,-797,25,-797,97,-797,54,-797,35,-797,55,-797,102,-797,47,-797,36,-797,53,-797,60,-797,74,-797,72,-797,38,-797,71,-797});
    states[393] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-76,394,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,834,-105,659,-331,677});
    states[394] = new State(new int[]{12,395,100,153});
    states[395] = new State(-821);
    states[396] = new State(-612);
    states[397] = new State(new int[]{138,-791,136,-791,118,-791,117,-791,131,-791,132,-791,133,-791,134,-791,130,-791,116,-791,115,-791,128,-791,129,-791,126,-791,6,-791,5,-791,120,-791,125,-791,123,-791,121,-791,124,-791,122,-791,137,-791,135,-791,16,-791,70,-791,92,-791,10,-791,98,-791,101,-791,33,-791,104,-791,2,-791,9,-791,12,-791,100,-791,99,-791,32,-791,85,-791,84,-791,83,-791,82,-791,81,-791,86,-791,13,-791,76,-791,51,-791,58,-791,141,-791,143,-791,161,-791,80,-791,78,-791,160,-791,87,-791,45,-791,42,-791,8,-791,21,-791,22,-791,144,-791,147,-791,145,-791,146,-791,155,-791,158,-791,157,-791,156,-791,11,-791,57,-791,91,-791,40,-791,25,-791,97,-791,54,-791,35,-791,55,-791,102,-791,47,-791,36,-791,53,-791,60,-791,74,-791,72,-791,38,-791,71,-791,119,-784});
    states[398] = new State(-801);
    states[399] = new State(new int[]{11,393,17,400,8,702,7,1108,142,1110,4,1111,15,1118,110,-798,111,-798,112,-798,113,-798,114,-798,92,-798,10,-798,98,-798,101,-798,33,-798,104,-798,2,-798,138,-798,136,-798,118,-798,117,-798,131,-798,132,-798,133,-798,134,-798,130,-798,116,-798,115,-798,128,-798,129,-798,126,-798,6,-798,5,-798,120,-798,125,-798,123,-798,121,-798,124,-798,122,-798,137,-798,135,-798,16,-798,70,-798,9,-798,12,-798,100,-798,99,-798,32,-798,85,-798,84,-798,83,-798,82,-798,81,-798,86,-798,13,-798,119,-798,76,-798,51,-798,58,-798,141,-798,143,-798,161,-798,80,-798,78,-798,160,-798,87,-798,45,-798,42,-798,21,-798,22,-798,144,-798,147,-798,145,-798,146,-798,155,-798,158,-798,157,-798,156,-798,57,-798,91,-798,40,-798,25,-798,97,-798,54,-798,35,-798,55,-798,102,-798,47,-798,36,-798,53,-798,60,-798,74,-798,72,-798,38,-798,71,-798});
    states[400] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,5,598},new int[]{-125,401,-110,383,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,602,-274,578});
    states[401] = new State(new int[]{12,402});
    states[402] = new State(-823);
    states[403] = new State(-813);
    states[404] = new State(-814);
    states[405] = new State(new int[]{115,407,116,408,117,409,118,410,120,411,121,412,122,413,123,414,124,415,125,416,128,417,129,418,130,419,131,420,132,421,133,422,134,423,135,424,137,425,139,426,140,427,110,429,111,430,112,431,113,432,114,433,119,434},new int[]{-206,406,-200,428});
    states[406] = new State(-849);
    states[407] = new State(-973);
    states[408] = new State(-974);
    states[409] = new State(-975);
    states[410] = new State(-976);
    states[411] = new State(-977);
    states[412] = new State(-978);
    states[413] = new State(-979);
    states[414] = new State(-980);
    states[415] = new State(-981);
    states[416] = new State(-982);
    states[417] = new State(-983);
    states[418] = new State(-984);
    states[419] = new State(-985);
    states[420] = new State(-986);
    states[421] = new State(-987);
    states[422] = new State(-988);
    states[423] = new State(-989);
    states[424] = new State(-990);
    states[425] = new State(-991);
    states[426] = new State(-992);
    states[427] = new State(-993);
    states[428] = new State(-994);
    states[429] = new State(-996);
    states[430] = new State(-997);
    states[431] = new State(-998);
    states[432] = new State(-999);
    states[433] = new State(-1000);
    states[434] = new State(-995);
    states[435] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,436,-156,49,-157,52});
    states[436] = new State(-815);
    states[437] = new State(new int[]{53,687,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-367,440,-107,532,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-366,1104,-105,1105});
    states[438] = new State(new int[]{9,439,70,27});
    states[439] = new State(-816);
    states[440] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,53,1114},new int[]{-92,441,-366,443,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[441] = new State(new int[]{9,442,70,27});
    states[442] = new State(-817);
    states[443] = new State(-811);
    states[444] = new State(-818);
    states[445] = new State(-819);
    states[446] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-73,447,-82,371,-97,449,-92,374,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[447] = new State(new int[]{76,448});
    states[448] = new State(-825);
    states[449] = new State(-838);
    states[450] = new State(-826);
    states[451] = new State(-827);
    states[452] = new State(new int[]{7,453,138,-792,136,-792,118,-792,117,-792,131,-792,132,-792,133,-792,134,-792,130,-792,116,-792,115,-792,128,-792,129,-792,126,-792,6,-792,5,-792,120,-792,125,-792,123,-792,121,-792,124,-792,122,-792,137,-792,135,-792,16,-792,70,-792,92,-792,10,-792,98,-792,101,-792,33,-792,104,-792,2,-792,9,-792,12,-792,100,-792,99,-792,32,-792,85,-792,84,-792,83,-792,82,-792,81,-792,86,-792,13,-792,76,-792,51,-792,58,-792,141,-792,143,-792,161,-792,80,-792,78,-792,160,-792,87,-792,45,-792,42,-792,8,-792,21,-792,22,-792,144,-792,147,-792,145,-792,146,-792,155,-792,158,-792,157,-792,156,-792,11,-792,57,-792,91,-792,40,-792,25,-792,97,-792,54,-792,35,-792,55,-792,102,-792,47,-792,36,-792,53,-792,60,-792,74,-792,72,-792,38,-792,71,-792});
    states[453] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,405},new int[]{-153,454,-152,455,-156,49,-157,52,-299,456,-155,58,-197,457});
    states[454] = new State(-829);
    states[455] = new State(-865);
    states[456] = new State(-866);
    states[457] = new State(-867);
    states[458] = new State(-799);
    states[459] = new State(-768);
    states[460] = new State(-769);
    states[461] = new State(new int[]{119,462});
    states[462] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-101,463,-275,464,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-102,461});
    states[463] = new State(-764);
    states[464] = new State(-765);
    states[465] = new State(-773);
    states[466] = new State(new int[]{8,467,138,-758,136,-758,118,-758,117,-758,131,-758,132,-758,133,-758,134,-758,130,-758,116,-758,115,-758,128,-758,129,-758,126,-758,6,-758,5,-758,120,-758,125,-758,123,-758,121,-758,124,-758,122,-758,137,-758,135,-758,16,-758,70,-758,92,-758,10,-758,98,-758,101,-758,33,-758,104,-758,2,-758,9,-758,12,-758,100,-758,99,-758,32,-758,85,-758,84,-758,83,-758,82,-758,81,-758,86,-758,13,-758,76,-758,51,-758,58,-758,141,-758,143,-758,161,-758,80,-758,78,-758,160,-758,87,-758,45,-758,42,-758,21,-758,22,-758,144,-758,147,-758,145,-758,146,-758,155,-758,158,-758,157,-758,156,-758,11,-758,57,-758,91,-758,40,-758,25,-758,97,-758,54,-758,35,-758,55,-758,102,-758,47,-758,36,-758,53,-758,60,-758,74,-758,72,-758,38,-758,71,-758});
    states[467] = new State(new int[]{14,472,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,474,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-361,468,-359,1482,-20,473,-171,336,-173,279,-172,283,-21,337,-348,1473,-290,1474,-186,181,-152,218,-156,49,-157,52,-351,1480,-352,1481});
    states[468] = new State(new int[]{9,469,10,470,100,1478});
    states[469] = new State(-670);
    states[470] = new State(new int[]{14,472,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,474,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-359,471,-20,473,-171,336,-173,279,-172,283,-21,337,-348,1473,-290,1474,-186,181,-152,218,-156,49,-157,52,-351,1480,-352,1481});
    states[471] = new State(-707);
    states[472] = new State(-709);
    states[473] = new State(-710);
    states[474] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,475,-156,49,-157,52});
    states[475] = new State(new int[]{5,476,9,-712,10,-712,100,-712});
    states[476] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,477,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[477] = new State(-711);
    states[478] = new State(new int[]{9,479,143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,325,-83,331,-283,332,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[479] = new State(new int[]{127,321});
    states[480] = new State(-250);
    states[481] = new State(new int[]{13,482,127,483,120,-255,9,-255,100,-255,10,-255,121,-255,8,-255,138,-255,136,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,116,-255,115,-255,128,-255,129,-255,126,-255,6,-255,5,-255,125,-255,123,-255,124,-255,122,-255,137,-255,135,-255,16,-255,70,-255,92,-255,98,-255,101,-255,33,-255,104,-255,2,-255,12,-255,99,-255,32,-255,85,-255,84,-255,83,-255,82,-255,81,-255,86,-255,76,-255,51,-255,58,-255,141,-255,143,-255,161,-255,80,-255,78,-255,160,-255,87,-255,45,-255,42,-255,21,-255,22,-255,144,-255,147,-255,145,-255,146,-255,155,-255,158,-255,157,-255,156,-255,11,-255,57,-255,91,-255,40,-255,25,-255,97,-255,54,-255,35,-255,55,-255,102,-255,47,-255,36,-255,53,-255,60,-255,74,-255,72,-255,38,-255,71,-255,110,-255});
    states[482] = new State(-248);
    states[483] = new State(new int[]{8,485,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,484,-279,192,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-288,1470,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,1471,-230,561,-229,562,-307,1472});
    states[484] = new State(-311);
    states[485] = new State(new int[]{9,486,143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,325,-83,331,-283,332,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[486] = new State(new int[]{127,321,121,-315,100,-315,120,-315,9,-315,10,-315,8,-315,138,-315,136,-315,118,-315,117,-315,131,-315,132,-315,133,-315,134,-315,130,-315,116,-315,115,-315,128,-315,129,-315,126,-315,6,-315,5,-315,125,-315,123,-315,124,-315,122,-315,137,-315,135,-315,16,-315,70,-315,92,-315,98,-315,101,-315,33,-315,104,-315,2,-315,12,-315,99,-315,32,-315,85,-315,84,-315,83,-315,82,-315,81,-315,86,-315,13,-315,76,-315,51,-315,58,-315,141,-315,143,-315,161,-315,80,-315,78,-315,160,-315,87,-315,45,-315,42,-315,21,-315,22,-315,144,-315,147,-315,145,-315,146,-315,155,-315,158,-315,157,-315,156,-315,11,-315,57,-315,91,-315,40,-315,25,-315,97,-315,54,-315,35,-315,55,-315,102,-315,47,-315,36,-315,53,-315,60,-315,74,-315,72,-315,38,-315,71,-315,110,-315});
    states[487] = new State(-251);
    states[488] = new State(-252);
    states[489] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,490,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[490] = new State(-288);
    states[491] = new State(-506);
    states[492] = new State(-253);
    states[493] = new State(-289);
    states[494] = new State(new int[]{11,495,58,1468});
    states[495] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,356,12,-301,100,-301},new int[]{-170,496,-278,1467,-279,1466,-98,194,-111,314,-112,315,-186,352,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283});
    states[496] = new State(new int[]{12,497,100,1464});
    states[497] = new State(new int[]{58,498});
    states[498] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,499,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[499] = new State(-295);
    states[500] = new State(-296);
    states[501] = new State(-290);
    states[502] = new State(new int[]{8,1344,23,-337,11,-337,92,-337,84,-337,83,-337,82,-337,81,-337,29,-337,143,-337,161,-337,85,-337,86,-337,80,-337,78,-337,160,-337,87,-337,62,-337,28,-337,26,-337,44,-337,37,-337,19,-337,30,-337,31,-337,46,-337,27,-337},new int[]{-189,503});
    states[503] = new State(new int[]{23,1335,11,-344,92,-344,84,-344,83,-344,82,-344,81,-344,29,-344,143,-344,161,-344,85,-344,86,-344,80,-344,78,-344,160,-344,87,-344,62,-344,28,-344,26,-344,44,-344,37,-344,19,-344,30,-344,31,-344,46,-344,27,-344},new int[]{-325,504,-324,1333,-323,1355});
    states[504] = new State(new int[]{11,628,92,-361,84,-361,83,-361,82,-361,81,-361,29,-234,143,-234,161,-234,85,-234,86,-234,80,-234,78,-234,160,-234,87,-234,62,-234,28,-234,26,-234,44,-234,37,-234,19,-234,30,-234,31,-234,46,-234,27,-234},new int[]{-29,505,-36,1256,-38,509,-49,1257,-11,1258,-257,1155,-37,1420,-58,1422,-57,515,-59,1421});
    states[505] = new State(new int[]{92,506,84,1252,83,1253,82,1254,81,1255},new int[]{-12,507});
    states[506] = new State(-319);
    states[507] = new State(new int[]{11,628,92,-361,84,-361,83,-361,82,-361,81,-361,29,-234,143,-234,161,-234,85,-234,86,-234,80,-234,78,-234,160,-234,87,-234,62,-234,28,-234,26,-234,44,-234,37,-234,19,-234,30,-234,31,-234,46,-234,27,-234},new int[]{-36,508,-38,509,-49,1257,-11,1258,-257,1155,-37,1420,-58,1422,-57,515,-59,1421});
    states[508] = new State(-356);
    states[509] = new State(new int[]{10,511,92,-367,84,-367,83,-367,82,-367,81,-367},new int[]{-196,510});
    states[510] = new State(-362);
    states[511] = new State(new int[]{11,628,92,-368,84,-368,83,-368,82,-368,81,-368,29,-234,143,-234,161,-234,85,-234,86,-234,80,-234,78,-234,160,-234,87,-234,62,-234,28,-234,26,-234,44,-234,37,-234,19,-234,30,-234,31,-234,46,-234,27,-234},new int[]{-49,512,-37,513,-11,1258,-257,1155,-58,1422,-57,515,-59,1421});
    states[512] = new State(-370);
    states[513] = new State(new int[]{11,628,92,-364,84,-364,83,-364,82,-364,81,-364,28,-234,26,-234,44,-234,37,-234,19,-234,30,-234,31,-234,46,-234,27,-234},new int[]{-58,514,-57,515,-11,516,-257,1155,-59,1421});
    states[514] = new State(-373);
    states[515] = new State(-374);
    states[516] = new State(new int[]{28,1380,26,1381,44,1328,37,1363,19,1383,30,1391,31,1398,11,628,46,1405,27,1414},new int[]{-228,517,-257,518,-225,519,-265,520,-8,521,-236,1382,-234,1316,-231,1327,-235,1362,-233,1389,-221,1402,-222,1403,-224,1404});
    states[517] = new State(-383);
    states[518] = new State(-233);
    states[519] = new State(-384);
    states[520] = new State(-400);
    states[521] = new State(new int[]{30,523,19,1198,46,1270,27,1308,44,1328,37,1363},new int[]{-236,522,-222,1197,-234,1316,-231,1327,-235,1362});
    states[522] = new State(-387);
    states[523] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,8,-399,110,-399,10,-399},new int[]{-178,524,-177,1180,-176,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[524] = new State(new int[]{8,565,110,-490,10,-490},new int[]{-133,525});
    states[525] = new State(new int[]{110,527,10,1169},new int[]{-213,526});
    states[526] = new State(-396);
    states[527] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514},new int[]{-267,528,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[528] = new State(new int[]{10,529});
    states[529] = new State(-445);
    states[530] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,92,-594,10,-594,98,-594,101,-594,33,-594,104,-594,2,-594,9,-594,12,-594,100,-594,99,-594,32,-594,84,-594,83,-594,82,-594,81,-594},new int[]{-152,436,-156,49,-157,52});
    states[531] = new State(new int[]{53,1157,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-367,440,-107,532,-116,694,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-366,1104,-105,1105});
    states[532] = new State(new int[]{100,533});
    states[533] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,18,669},new int[]{-84,534,-107,1148,-106,1147,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-105,1105});
    states[534] = new State(new int[]{100,1145,5,546,10,-1030,9,-1030},new int[]{-332,535});
    states[535] = new State(new int[]{10,538,9,-1018},new int[]{-339,536});
    states[536] = new State(new int[]{9,537});
    states[537] = new State(-782);
    states[538] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-334,539,-335,1096,-163,542,-152,809,-156,49,-157,52});
    states[539] = new State(new int[]{10,540,9,-1019});
    states[540] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-335,541,-163,542,-152,809,-156,49,-157,52});
    states[541] = new State(-1028);
    states[542] = new State(new int[]{100,544,5,546,10,-1030,9,-1030},new int[]{-332,543});
    states[543] = new State(-1029);
    states[544] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,545,-156,49,-157,52});
    states[545] = new State(-366);
    states[546] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,547,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[547] = new State(-1031);
    states[548] = new State(-291);
    states[549] = new State(new int[]{58,550});
    states[550] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,551,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[551] = new State(-302);
    states[552] = new State(-292);
    states[553] = new State(new int[]{58,554,121,-304,100,-304,120,-304,9,-304,10,-304,8,-304,138,-304,136,-304,118,-304,117,-304,131,-304,132,-304,133,-304,134,-304,130,-304,116,-304,115,-304,128,-304,129,-304,126,-304,6,-304,5,-304,125,-304,123,-304,124,-304,122,-304,137,-304,135,-304,16,-304,70,-304,92,-304,98,-304,101,-304,33,-304,104,-304,2,-304,12,-304,99,-304,32,-304,85,-304,84,-304,83,-304,82,-304,81,-304,86,-304,13,-304,76,-304,51,-304,141,-304,143,-304,161,-304,80,-304,78,-304,160,-304,87,-304,45,-304,42,-304,21,-304,22,-304,144,-304,147,-304,145,-304,146,-304,155,-304,158,-304,157,-304,156,-304,11,-304,57,-304,91,-304,40,-304,25,-304,97,-304,54,-304,35,-304,55,-304,102,-304,47,-304,36,-304,53,-304,60,-304,74,-304,72,-304,38,-304,71,-304,127,-304,110,-304});
    states[554] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,555,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[555] = new State(-303);
    states[556] = new State(-293);
    states[557] = new State(new int[]{58,558});
    states[558] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,559,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[559] = new State(-294);
    states[560] = new State(-254);
    states[561] = new State(-306);
    states[562] = new State(-307);
    states[563] = new State(new int[]{8,565,121,-490,100,-490,120,-490,9,-490,10,-490,138,-490,136,-490,118,-490,117,-490,131,-490,132,-490,133,-490,134,-490,130,-490,116,-490,115,-490,128,-490,129,-490,126,-490,6,-490,5,-490,125,-490,123,-490,124,-490,122,-490,137,-490,135,-490,16,-490,70,-490,92,-490,98,-490,101,-490,33,-490,104,-490,2,-490,12,-490,99,-490,32,-490,85,-490,84,-490,83,-490,82,-490,81,-490,86,-490,13,-490,76,-490,51,-490,58,-490,141,-490,143,-490,161,-490,80,-490,78,-490,160,-490,87,-490,45,-490,42,-490,21,-490,22,-490,144,-490,147,-490,145,-490,146,-490,155,-490,158,-490,157,-490,156,-490,11,-490,57,-490,91,-490,40,-490,25,-490,97,-490,54,-490,35,-490,55,-490,102,-490,47,-490,36,-490,53,-490,60,-490,74,-490,72,-490,38,-490,71,-490,127,-490,110,-490},new int[]{-133,564});
    states[564] = new State(-308);
    states[565] = new State(new int[]{9,566,11,628,143,-234,161,-234,85,-234,86,-234,80,-234,78,-234,160,-234,87,-234,53,-234,29,-234,108,-234},new int[]{-134,567,-60,1156,-11,571,-257,1155});
    states[566] = new State(-491);
    states[567] = new State(new int[]{9,568,10,569});
    states[568] = new State(-492);
    states[569] = new State(new int[]{11,628,143,-234,161,-234,85,-234,86,-234,80,-234,78,-234,160,-234,87,-234,53,-234,29,-234,108,-234},new int[]{-60,570,-11,571,-257,1155});
    states[570] = new State(-494);
    states[571] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,53,612,29,618,108,624,11,628},new int[]{-302,572,-257,518,-164,573,-140,611,-152,610,-156,49,-157,52});
    states[572] = new State(-495);
    states[573] = new State(new int[]{5,574,100,608});
    states[574] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,575,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[575] = new State(new int[]{110,576,9,-496,10,-496});
    states[576] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,577,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[577] = new State(new int[]{70,27,9,-500,10,-500});
    states[578] = new State(-759);
    states[579] = new State(new int[]{70,-629,92,-629,10,-629,98,-629,101,-629,33,-629,104,-629,2,-629,9,-629,12,-629,100,-629,99,-629,32,-629,85,-629,84,-629,83,-629,82,-629,81,-629,86,-629,6,-629,76,-629,5,-629,51,-629,58,-629,141,-629,143,-629,161,-629,80,-629,78,-629,160,-629,87,-629,45,-629,42,-629,8,-629,21,-629,22,-629,144,-629,147,-629,145,-629,146,-629,155,-629,158,-629,157,-629,156,-629,11,-629,57,-629,91,-629,40,-629,25,-629,97,-629,54,-629,35,-629,55,-629,102,-629,47,-629,36,-629,53,-629,60,-629,74,-629,72,-629,38,-629,71,-629,13,-632});
    states[580] = new State(new int[]{13,581});
    states[581] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-122,582,-104,585,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,587});
    states[582] = new State(new int[]{5,583,13,581});
    states[583] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-122,584,-104,585,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,587});
    states[584] = new State(new int[]{13,581,70,-645,92,-645,10,-645,98,-645,101,-645,33,-645,104,-645,2,-645,9,-645,12,-645,100,-645,99,-645,32,-645,85,-645,84,-645,83,-645,82,-645,81,-645,86,-645,6,-645,76,-645,5,-645,51,-645,58,-645,141,-645,143,-645,161,-645,80,-645,78,-645,160,-645,87,-645,45,-645,42,-645,8,-645,21,-645,22,-645,144,-645,147,-645,145,-645,146,-645,155,-645,158,-645,157,-645,156,-645,11,-645,57,-645,91,-645,40,-645,25,-645,97,-645,54,-645,35,-645,55,-645,102,-645,47,-645,36,-645,53,-645,60,-645,74,-645,72,-645,38,-645,71,-645});
    states[585] = new State(new int[]{16,30,5,-631,13,-631,70,-631,92,-631,10,-631,98,-631,101,-631,33,-631,104,-631,2,-631,9,-631,12,-631,100,-631,99,-631,32,-631,85,-631,84,-631,83,-631,82,-631,81,-631,86,-631,6,-631,76,-631,51,-631,58,-631,141,-631,143,-631,161,-631,80,-631,78,-631,160,-631,87,-631,45,-631,42,-631,8,-631,21,-631,22,-631,144,-631,147,-631,145,-631,146,-631,155,-631,158,-631,157,-631,156,-631,11,-631,57,-631,91,-631,40,-631,25,-631,97,-631,54,-631,35,-631,55,-631,102,-631,47,-631,36,-631,53,-631,60,-631,74,-631,72,-631,38,-631,71,-631});
    states[586] = new State(new int[]{6,34,120,-667,125,-667,123,-667,121,-667,124,-667,122,-667,137,-667,135,-667,16,-667,70,-667,92,-667,10,-667,98,-667,101,-667,33,-667,104,-667,2,-667,9,-667,12,-667,100,-667,99,-667,32,-667,85,-667,84,-667,83,-667,82,-667,81,-667,86,-667,76,-667,13,-667,5,-667,51,-667,58,-667,141,-667,143,-667,161,-667,80,-667,78,-667,160,-667,87,-667,45,-667,42,-667,8,-667,21,-667,22,-667,144,-667,147,-667,145,-667,146,-667,155,-667,158,-667,157,-667,156,-667,11,-667,57,-667,91,-667,40,-667,25,-667,97,-667,54,-667,35,-667,55,-667,102,-667,47,-667,36,-667,53,-667,60,-667,74,-667,72,-667,38,-667,71,-667,116,-667,115,-667,128,-667,129,-667,126,-667,138,-667,136,-667,118,-667,117,-667,131,-667,132,-667,133,-667,134,-667,130,-667});
    states[587] = new State(-632);
    states[588] = new State(-630);
    states[589] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-123,590,-104,595,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-249,596});
    states[590] = new State(new int[]{51,591});
    states[591] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-123,592,-104,595,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-249,596});
    states[592] = new State(new int[]{32,593});
    states[593] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-123,594,-104,595,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-249,596});
    states[594] = new State(-646);
    states[595] = new State(new int[]{16,30,51,-633,32,-633,120,-633,125,-633,123,-633,121,-633,124,-633,122,-633,137,-633,135,-633,70,-633,92,-633,10,-633,98,-633,101,-633,33,-633,104,-633,2,-633,9,-633,12,-633,100,-633,99,-633,85,-633,84,-633,83,-633,82,-633,81,-633,86,-633,13,-633,6,-633,76,-633,5,-633,58,-633,141,-633,143,-633,161,-633,80,-633,78,-633,160,-633,87,-633,45,-633,42,-633,8,-633,21,-633,22,-633,144,-633,147,-633,145,-633,146,-633,155,-633,158,-633,157,-633,156,-633,11,-633,57,-633,91,-633,40,-633,25,-633,97,-633,54,-633,35,-633,55,-633,102,-633,47,-633,36,-633,53,-633,60,-633,74,-633,72,-633,38,-633,71,-633,116,-633,115,-633,128,-633,129,-633,126,-633,138,-633,136,-633,118,-633,117,-633,131,-633,132,-633,133,-633,134,-633,130,-633});
    states[596] = new State(-634);
    states[597] = new State(-626);
    states[598] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,5,-727,70,-727,92,-727,10,-727,98,-727,101,-727,33,-727,104,-727,2,-727,9,-727,12,-727,100,-727,99,-727,32,-727,84,-727,83,-727,82,-727,81,-727,6,-727},new int[]{-120,599,-110,603,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,602,-274,578});
    states[599] = new State(new int[]{5,600,70,-731,92,-731,10,-731,98,-731,101,-731,33,-731,104,-731,2,-731,9,-731,12,-731,100,-731,99,-731,32,-731,85,-731,84,-731,83,-731,82,-731,81,-731,86,-731,6,-731,76,-731});
    states[600] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-110,601,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,602,-274,578});
    states[601] = new State(new int[]{6,34,70,-733,92,-733,10,-733,98,-733,101,-733,33,-733,104,-733,2,-733,9,-733,12,-733,100,-733,99,-733,32,-733,85,-733,84,-733,83,-733,82,-733,81,-733,86,-733,76,-733});
    states[602] = new State(-758);
    states[603] = new State(new int[]{6,34,5,-726,70,-726,92,-726,10,-726,98,-726,101,-726,33,-726,104,-726,2,-726,9,-726,12,-726,100,-726,99,-726,32,-726,85,-726,84,-726,83,-726,82,-726,81,-726,86,-726,76,-726});
    states[604] = new State(new int[]{8,565,5,-490},new int[]{-133,605});
    states[605] = new State(new int[]{5,606});
    states[606] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,607,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[607] = new State(-309);
    states[608] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-140,609,-152,610,-156,49,-157,52});
    states[609] = new State(-504);
    states[610] = new State(-505);
    states[611] = new State(-503);
    states[612] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-164,613,-140,611,-152,610,-156,49,-157,52});
    states[613] = new State(new int[]{5,614,100,608});
    states[614] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,615,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[615] = new State(new int[]{110,616,9,-497,10,-497});
    states[616] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,617,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[617] = new State(new int[]{70,27,9,-501,10,-501});
    states[618] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-164,619,-140,611,-152,610,-156,49,-157,52});
    states[619] = new State(new int[]{5,620,100,608});
    states[620] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,621,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[621] = new State(new int[]{110,622,9,-498,10,-498});
    states[622] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,623,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[623] = new State(new int[]{70,27,9,-502,10,-502});
    states[624] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-164,625,-140,611,-152,610,-156,49,-157,52});
    states[625] = new State(new int[]{5,626,100,608});
    states[626] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,627,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[627] = new State(-499);
    states[628] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-258,629,-13,1154,-14,633,-186,634,-152,1149,-156,49,-157,52,-307,1152});
    states[629] = new State(new int[]{12,630,100,631});
    states[630] = new State(-235);
    states[631] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-13,632,-14,633,-186,634,-152,1149,-156,49,-157,52,-307,1152});
    states[632] = new State(-237);
    states[633] = new State(-238);
    states[634] = new State(new int[]{7,182,8,637,123,187,12,-663,100,-663},new int[]{-74,635,-305,636});
    states[635] = new State(-803);
    states[636] = new State(-256);
    states[637] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,9,-833},new int[]{-71,638,-76,640,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,834,-105,659,-331,677});
    states[638] = new State(new int[]{9,639});
    states[639] = new State(-664);
    states[640] = new State(new int[]{100,153,12,-832,9,-832});
    states[641] = new State(new int[]{127,642,11,-813,17,-813,8,-813,7,-813,142,-813,4,-813,15,-813,138,-813,136,-813,118,-813,117,-813,131,-813,132,-813,133,-813,134,-813,130,-813,116,-813,115,-813,128,-813,129,-813,126,-813,6,-813,5,-813,120,-813,125,-813,123,-813,121,-813,124,-813,122,-813,137,-813,135,-813,16,-813,70,-813,92,-813,10,-813,98,-813,101,-813,33,-813,104,-813,2,-813,9,-813,12,-813,100,-813,99,-813,32,-813,85,-813,84,-813,83,-813,82,-813,81,-813,86,-813,13,-813,119,-813});
    states[642] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,643,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[643] = new State(-1010);
    states[644] = new State(-1047);
    states[645] = new State(new int[]{16,30,92,-637,10,-637,98,-637,101,-637,33,-637,104,-637,2,-637,9,-637,12,-637,100,-637,99,-637,32,-637,85,-637,84,-637,83,-637,82,-637,81,-637,86,-637,13,-631});
    states[646] = new State(new int[]{53,687,9,714,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,1124,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-367,440,-107,647,-152,1083,-9,1121,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,1123,-137,391,-116,399,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-366,1104,-105,1105});
    states[647] = new State(new int[]{100,648});
    states[648] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,18,669},new int[]{-84,649,-107,1148,-106,1147,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-105,1105});
    states[649] = new State(new int[]{100,1145,5,546,10,-1030,9,-1030},new int[]{-332,650});
    states[650] = new State(new int[]{10,538,9,-1018},new int[]{-339,651});
    states[651] = new State(new int[]{9,652});
    states[652] = new State(new int[]{5,661,7,-782,138,-782,136,-782,118,-782,117,-782,131,-782,132,-782,133,-782,134,-782,130,-782,116,-782,115,-782,128,-782,129,-782,126,-782,6,-782,120,-782,125,-782,123,-782,121,-782,124,-782,122,-782,137,-782,135,-782,16,-782,70,-782,92,-782,10,-782,98,-782,101,-782,33,-782,104,-782,2,-782,9,-782,12,-782,100,-782,99,-782,32,-782,85,-782,84,-782,83,-782,82,-782,81,-782,86,-782,13,-782,127,-1032},new int[]{-343,653,-333,654});
    states[653] = new State(-1015);
    states[654] = new State(new int[]{127,655});
    states[655] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,656,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[656] = new State(-1020);
    states[657] = new State(new int[]{92,-638,10,-638,98,-638,101,-638,33,-638,104,-638,2,-638,9,-638,12,-638,100,-638,99,-638,32,-638,85,-638,84,-638,83,-638,82,-638,81,-638,86,-638,13,-632});
    states[658] = new State(-639);
    states[659] = new State(new int[]{5,661,127,-1032},new int[]{-343,660,-333,654});
    states[660] = new State(-1016);
    states[661] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,356,142,489,24,494,48,502,49,549,34,553,73,557},new int[]{-284,662,-279,663,-98,194,-111,314,-112,315,-186,664,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-263,665,-256,666,-288,667,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-307,668});
    states[662] = new State(-1033);
    states[663] = new State(-507);
    states[664] = new State(new int[]{7,182,123,187,8,-277,118,-277,117,-277,131,-277,132,-277,133,-277,134,-277,130,-277,6,-277,116,-277,115,-277,128,-277,129,-277,127,-277},new int[]{-305,636});
    states[665] = new State(-508);
    states[666] = new State(-509);
    states[667] = new State(-510);
    states[668] = new State(-511);
    states[669] = new State(new int[]{18,669,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-28,670,-27,676,-105,674,-152,675,-156,49,-157,52});
    states[670] = new State(new int[]{100,671});
    states[671] = new State(new int[]{18,669,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-27,672,-105,674,-152,675,-156,49,-157,52});
    states[672] = new State(new int[]{9,673,100,-1005});
    states[673] = new State(-1001);
    states[674] = new State(-1002);
    states[675] = new State(-1003);
    states[676] = new State(-1004);
    states[677] = new State(-1017);
    states[678] = new State(new int[]{8,1135,5,661,127,-1032},new int[]{-333,679});
    states[679] = new State(new int[]{127,680});
    states[680] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,681,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[681] = new State(-1021);
    states[682] = new State(new int[]{127,683,8,1127});
    states[683] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,686,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-337,684,-218,685,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-9,1125,-338,1126,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[684] = new State(-1024);
    states[685] = new State(-1049);
    states[686] = new State(new int[]{53,687,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,1124,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-367,440,-107,532,-116,694,-9,1121,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,1123,-137,391,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-366,1104,-105,1105});
    states[687] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,688,-156,49,-157,52});
    states[688] = new State(new int[]{110,689});
    states[689] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-104,690,-92,692,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-106,155,-247,579,-122,580,-249,588,-125,597});
    states[690] = new State(new int[]{9,691,16,30,10,-628,70,-628,13,-631});
    states[691] = new State(-800);
    states[692] = new State(new int[]{10,693,70,27});
    states[693] = new State(-809);
    states[694] = new State(new int[]{100,695,11,393,17,400,8,702,7,1108,142,1110,4,1111,15,1118,138,-798,136,-798,118,-798,117,-798,131,-798,132,-798,133,-798,134,-798,130,-798,116,-798,115,-798,128,-798,129,-798,126,-798,6,-798,5,-798,120,-798,125,-798,123,-798,121,-798,124,-798,122,-798,137,-798,135,-798,16,-798,9,-798,70,-798,13,-798,119,-798,110,-798,111,-798,112,-798,113,-798,114,-798});
    states[695] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,1113,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368},new int[]{-344,696,-116,1117,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728});
    states[696] = new State(new int[]{9,697,100,700});
    states[697] = new State(new int[]{110,429,111,430,112,431,113,432,114,433},new int[]{-200,698});
    states[698] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,699,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[699] = new State(new int[]{70,27,92,-543,10,-543,98,-543,101,-543,33,-543,104,-543,2,-543,9,-543,12,-543,100,-543,99,-543,32,-543,85,-543,84,-543,83,-543,82,-543,81,-543,86,-543});
    states[700] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,1113,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368},new int[]{-116,701,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728});
    states[701] = new State(new int[]{11,393,17,400,8,702,7,1108,142,1110,4,1111,9,-545,100,-545});
    states[702] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,712,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,9,-835},new int[]{-72,703,-77,705,-94,1107,-92,708,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,709,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,1106,-105,659,-331,677});
    states[703] = new State(new int[]{9,704});
    states[704] = new State(-812);
    states[705] = new State(new int[]{100,706,9,-834});
    states[706] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,712,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-94,707,-92,708,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,709,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,1106,-105,659,-331,677});
    states[707] = new State(-615);
    states[708] = new State(new int[]{70,27,100,-621,9,-621});
    states[709] = new State(new int[]{110,710,127,642,11,-813,17,-813,8,-813,7,-813,142,-813,4,-813,15,-813,138,-813,136,-813,118,-813,117,-813,131,-813,132,-813,133,-813,134,-813,130,-813,116,-813,115,-813,128,-813,129,-813,126,-813,6,-813,5,-813,120,-813,125,-813,123,-813,121,-813,124,-813,122,-813,137,-813,135,-813,16,-813,70,-813,100,-813,9,-813,13,-813,119,-813});
    states[710] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,711,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[711] = new State(-622);
    states[712] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,100,-624,9,-624},new int[]{-152,436,-156,49,-157,52});
    states[713] = new State(new int[]{53,687,9,714,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-367,440,-107,647,-152,1083,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-366,1104,-105,1105});
    states[714] = new State(new int[]{5,661,127,-1032},new int[]{-333,715});
    states[715] = new State(new int[]{127,716});
    states[716] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,717,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[717] = new State(-1011);
    states[718] = new State(-1048);
    states[719] = new State(-1034);
    states[720] = new State(-1035);
    states[721] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,722,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[722] = new State(new int[]{51,723});
    states[723] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514,9,-514,12,-514,100,-514,99,-514,32,-514,84,-514,83,-514,82,-514,81,-514},new int[]{-267,724,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[724] = new State(new int[]{32,725,92,-553,10,-553,98,-553,101,-553,33,-553,104,-553,2,-553,9,-553,12,-553,100,-553,99,-553,85,-553,84,-553,83,-553,82,-553,81,-553,86,-553});
    states[725] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514,9,-514,12,-514,100,-514,99,-514,32,-514,84,-514,83,-514,82,-514,81,-514},new int[]{-267,726,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[726] = new State(-554);
    states[727] = new State(new int[]{7,44,11,150,17,380});
    states[728] = new State(new int[]{7,453});
    states[729] = new State(-516);
    states[730] = new State(-517);
    states[731] = new State(new int[]{155,733,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-148,732,-152,734,-156,49,-157,52});
    states[732] = new State(-549);
    states[733] = new State(-120);
    states[734] = new State(-121);
    states[735] = new State(-518);
    states[736] = new State(-519);
    states[737] = new State(-520);
    states[738] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,739,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[739] = new State(new int[]{58,740});
    states[740] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,32,748,92,-573},new int[]{-40,741,-260,1080,-269,1082,-79,1073,-115,1079,-99,1078,-95,205,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[741] = new State(new int[]{10,744,32,748,92,-573},new int[]{-260,742});
    states[742] = new State(new int[]{92,743});
    states[743] = new State(-564);
    states[744] = new State(new int[]{32,748,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,92,-573},new int[]{-260,745,-269,747,-79,1073,-115,1079,-99,1078,-95,205,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[745] = new State(new int[]{92,746});
    states[746] = new State(-565);
    states[747] = new State(-568);
    states[748] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514,92,-514},new int[]{-259,749,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[749] = new State(new int[]{10,19,92,-574});
    states[750] = new State(-551);
    states[751] = new State(new int[]{11,-813,17,-813,8,-813,7,-813,142,-813,4,-813,15,-813,110,-813,111,-813,112,-813,113,-813,114,-813,92,-813,10,-813,98,-813,101,-813,33,-813,104,-813,2,-813,5,-121});
    states[752] = new State(new int[]{7,-212,11,-212,17,-212,5,-120});
    states[753] = new State(-521);
    states[754] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,98,-514,10,-514},new int[]{-259,755,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[755] = new State(new int[]{98,756,10,19});
    states[756] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,757,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[757] = new State(new int[]{70,27,92,-575,10,-575,98,-575,101,-575,33,-575,104,-575,2,-575,9,-575,12,-575,100,-575,99,-575,32,-575,85,-575,84,-575,83,-575,82,-575,81,-575,86,-575});
    states[758] = new State(-522);
    states[759] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,760,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[760] = new State(new int[]{99,1069,141,-578,143,-578,161,-578,85,-578,86,-578,80,-578,78,-578,160,-578,87,-578,45,-578,42,-578,8,-578,21,-578,22,-578,144,-578,147,-578,145,-578,146,-578,155,-578,158,-578,157,-578,156,-578,76,-578,11,-578,57,-578,91,-578,40,-578,25,-578,97,-578,54,-578,35,-578,55,-578,102,-578,47,-578,36,-578,53,-578,60,-578,74,-578,72,-578,38,-578,92,-578,10,-578,98,-578,101,-578,33,-578,104,-578,2,-578,9,-578,12,-578,100,-578,32,-578,84,-578,83,-578,82,-578,81,-578},new int[]{-298,761});
    states[761] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514,9,-514,12,-514,100,-514,99,-514,32,-514,84,-514,83,-514,82,-514,81,-514},new int[]{-267,762,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[762] = new State(-576);
    states[763] = new State(-523);
    states[764] = new State(new int[]{53,1072,143,-585,161,-585,85,-585,86,-585,80,-585,78,-585,160,-585,87,-585},new int[]{-23,765});
    states[765] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,766,-156,49,-157,52});
    states[766] = new State(new int[]{5,1022,110,-583},new int[]{-281,767});
    states[767] = new State(new int[]{110,768});
    states[768] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,769,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[769] = new State(new int[]{70,1070,71,1071},new int[]{-124,770});
    states[770] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,771,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[771] = new State(new int[]{160,1065,99,1069,141,-578,143,-578,161,-578,85,-578,86,-578,80,-578,78,-578,87,-578,45,-578,42,-578,8,-578,21,-578,22,-578,144,-578,147,-578,145,-578,146,-578,155,-578,158,-578,157,-578,156,-578,76,-578,11,-578,57,-578,91,-578,40,-578,25,-578,97,-578,54,-578,35,-578,55,-578,102,-578,47,-578,36,-578,53,-578,60,-578,74,-578,72,-578,38,-578,92,-578,10,-578,98,-578,101,-578,33,-578,104,-578,2,-578,9,-578,12,-578,100,-578,32,-578,84,-578,83,-578,82,-578,81,-578},new int[]{-298,772});
    states[772] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514,9,-514,12,-514,100,-514,99,-514,32,-514,84,-514,83,-514,82,-514,81,-514},new int[]{-267,773,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[773] = new State(-591);
    states[774] = new State(-524);
    states[775] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-76,776,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,834,-105,659,-331,677});
    states[776] = new State(new int[]{99,777,100,153});
    states[777] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514,9,-514,12,-514,100,-514,99,-514,32,-514,84,-514,83,-514,82,-514,81,-514},new int[]{-267,778,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[778] = new State(-593);
    states[779] = new State(-525);
    states[780] = new State(-526);
    states[781] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514,101,-514,33,-514},new int[]{-259,782,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[782] = new State(new int[]{10,19,101,784,33,1043},new int[]{-296,783});
    states[783] = new State(-595);
    states[784] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514},new int[]{-259,785,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[785] = new State(new int[]{92,786,10,19});
    states[786] = new State(-596);
    states[787] = new State(-527);
    states[788] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,92,-610,10,-610,98,-610,101,-610,33,-610,104,-610,2,-610,9,-610,12,-610,100,-610,99,-610,32,-610,84,-610,83,-610,82,-610,81,-610},new int[]{-92,789,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[789] = new State(new int[]{70,27,92,-611,10,-611,98,-611,101,-611,33,-611,104,-611,2,-611,9,-611,12,-611,100,-611,99,-611,32,-611,85,-611,84,-611,83,-611,82,-611,81,-611,86,-611});
    states[790] = new State(-528);
    states[791] = new State(new int[]{53,1024,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,792,-156,49,-157,52});
    states[792] = new State(new int[]{5,1022,137,-583},new int[]{-281,793});
    states[793] = new State(new int[]{137,794});
    states[794] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,795,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[795] = new State(new int[]{87,1020,99,-581},new int[]{-368,796});
    states[796] = new State(new int[]{99,797});
    states[797] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514,9,-514,12,-514,100,-514,99,-514,32,-514,84,-514,83,-514,82,-514,81,-514},new int[]{-267,798,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[798] = new State(-588);
    states[799] = new State(-529);
    states[800] = new State(new int[]{8,802,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-319,801,-163,810,-152,809,-156,49,-157,52});
    states[801] = new State(-539);
    states[802] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,803,-156,49,-157,52});
    states[803] = new State(new int[]{100,804});
    states[804] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,805,-152,809,-156,49,-157,52});
    states[805] = new State(new int[]{9,806,100,544});
    states[806] = new State(new int[]{110,807});
    states[807] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,808,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[808] = new State(new int[]{70,27,92,-541,10,-541,98,-541,101,-541,33,-541,104,-541,2,-541,9,-541,12,-541,100,-541,99,-541,32,-541,85,-541,84,-541,83,-541,82,-541,81,-541,86,-541});
    states[809] = new State(-365);
    states[810] = new State(new int[]{5,811,100,544,110,1018});
    states[811] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,812,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[812] = new State(new int[]{110,1016,120,1017,92,-429,10,-429,98,-429,101,-429,33,-429,104,-429,2,-429,9,-429,12,-429,100,-429,99,-429,32,-429,85,-429,84,-429,83,-429,82,-429,81,-429,86,-429},new int[]{-346,813});
    states[813] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,987,135,841,116,353,115,354,63,339,37,678,44,682,40,589},new int[]{-91,814,-90,815,-89,276,-95,277,-96,236,-86,816,-18,251,-15,261,-19,222,-152,856,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855,-100,1004,-250,1005,-331,1014,-249,1015});
    states[814] = new State(-431);
    states[815] = new State(-432);
    states[816] = new State(new int[]{6,817,116,247,115,248,128,249,129,250,120,-139,125,-139,123,-139,121,-139,124,-139,122,-139,137,-139,135,-139,13,-139,16,-139,92,-139,10,-139,98,-139,101,-139,33,-139,104,-139,2,-139,9,-139,12,-139,100,-139,99,-139,32,-139,85,-139,84,-139,83,-139,82,-139,81,-139,86,-139},new int[]{-199,214});
    states[817] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-18,818,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849});
    states[818] = new State(new int[]{136,252,138,253,118,254,117,255,131,256,132,257,133,258,134,259,130,260,92,-433,10,-433,98,-433,101,-433,33,-433,104,-433,2,-433,9,-433,12,-433,100,-433,99,-433,32,-433,85,-433,84,-433,83,-433,82,-433,81,-433,86,-433},new int[]{-207,216,-201,219});
    states[819] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-73,820,-82,371,-97,449,-92,374,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[820] = new State(new int[]{76,821});
    states[821] = new State(-188);
    states[822] = new State(-179);
    states[823] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,835,135,841,116,353,115,354,63,339},new int[]{-15,824,-19,825,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,843,-179,845,-61,846});
    states[824] = new State(-180);
    states[825] = new State(new int[]{4,224,11,226,7,826,142,828,8,829,136,-177,138,-177,118,-177,117,-177,131,-177,132,-177,133,-177,134,-177,130,-177,116,-177,115,-177,128,-177,129,-177,120,-177,125,-177,123,-177,121,-177,124,-177,122,-177,137,-177,135,-177,13,-177,16,-177,6,-177,100,-177,9,-177,12,-177,5,-177,92,-177,10,-177,98,-177,101,-177,33,-177,104,-177,2,-177,99,-177,32,-177,85,-177,84,-177,83,-177,82,-177,81,-177,86,-177},new int[]{-17,223});
    states[826] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-143,827,-152,46,-156,49,-157,52,-299,57,-155,58,-300,148});
    states[827] = new State(-200);
    states[828] = new State(-201);
    states[829] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,9,-205},new int[]{-81,830,-76,832,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,834,-105,659,-331,677});
    states[830] = new State(new int[]{9,831});
    states[831] = new State(-202);
    states[832] = new State(new int[]{100,153,9,-204});
    states[833] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,92,-620,10,-620,98,-620,101,-620,33,-620,104,-620,2,-620,9,-620,12,-620,100,-620,99,-620,32,-620,84,-620,83,-620,82,-620,81,-620},new int[]{-152,436,-156,49,-157,52});
    states[834] = new State(-619);
    states[835] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,836,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[836] = new State(new int[]{9,837,13,206,16,210});
    states[837] = new State(-181);
    states[838] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,839,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[839] = new State(new int[]{9,840,13,206,16,210});
    states[840] = new State(new int[]{136,-181,138,-181,118,-181,117,-181,131,-181,132,-181,133,-181,134,-181,130,-181,116,-181,115,-181,128,-181,129,-181,120,-181,125,-181,123,-181,121,-181,124,-181,122,-181,137,-181,135,-181,13,-181,16,-181,6,-181,100,-181,9,-181,12,-181,5,-181,92,-181,10,-181,98,-181,101,-181,33,-181,104,-181,2,-181,99,-181,32,-181,85,-181,84,-181,83,-181,82,-181,81,-181,86,-181,119,-176});
    states[841] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,835,135,841,116,353,115,354,63,339},new int[]{-15,842,-19,825,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,843,-179,845,-61,846});
    states[842] = new State(-182);
    states[843] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,835,135,841,116,353,115,354,63,339},new int[]{-15,844,-19,825,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,843,-179,845,-61,846});
    states[844] = new State(-183);
    states[845] = new State(-184);
    states[846] = new State(-185);
    states[847] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-15,844,-276,848,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-16,849});
    states[848] = new State(-162);
    states[849] = new State(new int[]{119,850});
    states[850] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-15,851,-276,852,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-16,849});
    states[851] = new State(-160);
    states[852] = new State(-161);
    states[853] = new State(-164);
    states[854] = new State(-165);
    states[855] = new State(-142);
    states[856] = new State(new int[]{127,857,4,-191,11,-191,7,-191,142,-191,8,-191,136,-191,138,-191,118,-191,117,-191,131,-191,132,-191,133,-191,134,-191,130,-191,6,-191,116,-191,115,-191,128,-191,129,-191,120,-191,125,-191,123,-191,121,-191,124,-191,122,-191,137,-191,135,-191,13,-191,16,-191,92,-191,10,-191,98,-191,101,-191,33,-191,104,-191,2,-191,9,-191,12,-191,100,-191,99,-191,32,-191,85,-191,84,-191,83,-191,82,-191,81,-191,86,-191,119,-191});
    states[857] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,858,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[858] = new State(-435);
    states[859] = new State(-1036);
    states[860] = new State(-1037);
    states[861] = new State(-1038);
    states[862] = new State(-1039);
    states[863] = new State(-1040);
    states[864] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,865,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[865] = new State(new int[]{99,866});
    states[866] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514,9,-514,12,-514,100,-514,99,-514,32,-514,84,-514,83,-514,82,-514,81,-514},new int[]{-267,867,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[867] = new State(-536);
    states[868] = new State(-530);
    states[869] = new State(-616);
    states[870] = new State(-617);
    states[871] = new State(-531);
    states[872] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,873,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[873] = new State(new int[]{99,874});
    states[874] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514,9,-514,12,-514,100,-514,99,-514,32,-514,84,-514,83,-514,82,-514,81,-514},new int[]{-267,875,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[875] = new State(-579);
    states[876] = new State(-532);
    states[877] = new State(new int[]{73,879,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,878,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-330,882,-105,659,-331,677});
    states[878] = new State(-537);
    states[879] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,880,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-330,882,-105,659,-331,677});
    states[880] = new State(-538);
    states[881] = new State(-635);
    states[882] = new State(-636);
    states[883] = new State(-533);
    states[884] = new State(-534);
    states[885] = new State(-535);
    states[886] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,887,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[887] = new State(new int[]{55,888});
    states[888] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,56,966,21,294,22,299,11,926,8,939},new int[]{-358,889,-357,980,-350,896,-290,901,-186,181,-152,218,-156,49,-157,52,-349,958,-365,961,-347,969,-20,964,-171,336,-173,279,-172,283,-21,337,-264,967,-301,968,-351,970,-352,973});
    states[889] = new State(new int[]{10,892,32,748,92,-573},new int[]{-260,890});
    states[890] = new State(new int[]{92,891});
    states[891] = new State(-555);
    states[892] = new State(new int[]{32,748,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,56,966,21,294,22,299,11,926,8,939,92,-573},new int[]{-260,893,-357,895,-350,896,-290,901,-186,181,-152,218,-156,49,-157,52,-349,958,-365,961,-347,969,-20,964,-171,336,-173,279,-172,283,-21,337,-264,967,-301,968,-351,970,-352,973});
    states[893] = new State(new int[]{92,894});
    states[894] = new State(-556);
    states[895] = new State(-558);
    states[896] = new State(new int[]{39,897});
    states[897] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,898,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[898] = new State(new int[]{5,899});
    states[899] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514,32,-514,92,-514},new int[]{-267,900,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[900] = new State(-559);
    states[901] = new State(new int[]{8,902,100,-678,5,-678});
    states[902] = new State(new int[]{14,907,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,53,914,11,926,8,939},new int[]{-362,903,-360,957,-20,908,-171,336,-173,279,-172,283,-21,337,-205,909,-152,911,-156,49,-157,52,-350,918,-290,919,-186,181,-351,925,-352,956});
    states[903] = new State(new int[]{9,904,10,905,100,923});
    states[904] = new State(new int[]{39,-672,5,-673});
    states[905] = new State(new int[]{14,907,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,53,914,11,926,8,939},new int[]{-360,906,-20,908,-171,336,-173,279,-172,283,-21,337,-205,909,-152,911,-156,49,-157,52,-350,918,-290,919,-186,181,-351,925,-352,956});
    states[906] = new State(-704);
    states[907] = new State(-716);
    states[908] = new State(-717);
    states[909] = new State(new int[]{144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290},new int[]{-20,910,-171,336,-173,279,-172,283,-21,337});
    states[910] = new State(-718);
    states[911] = new State(new int[]{5,912,9,-720,10,-720,100,-720,7,-282,4,-282,123,-282,8,-282});
    states[912] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,913,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[913] = new State(-719);
    states[914] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,915,-156,49,-157,52});
    states[915] = new State(new int[]{5,916,9,-722,10,-722,100,-722});
    states[916] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,917,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[917] = new State(-721);
    states[918] = new State(-723);
    states[919] = new State(new int[]{8,920});
    states[920] = new State(new int[]{14,907,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,53,914,11,926,8,939},new int[]{-362,921,-360,957,-20,908,-171,336,-173,279,-172,283,-21,337,-205,909,-152,911,-156,49,-157,52,-350,918,-290,919,-186,181,-351,925,-352,956});
    states[921] = new State(new int[]{9,922,10,905,100,923});
    states[922] = new State(-672);
    states[923] = new State(new int[]{14,907,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,53,914,11,926,8,939},new int[]{-360,924,-20,908,-171,336,-173,279,-172,283,-21,337,-205,909,-152,911,-156,49,-157,52,-350,918,-290,919,-186,181,-351,925,-352,956});
    states[924] = new State(-705);
    states[925] = new State(-724);
    states[926] = new State(new int[]{144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,933,14,935,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939,6,954},new int[]{-363,927,-353,955,-20,931,-171,336,-173,279,-172,283,-21,337,-355,932,-350,936,-290,919,-186,181,-152,218,-156,49,-157,52,-351,937,-352,938});
    states[927] = new State(new int[]{12,928,100,929});
    states[928] = new State(-682);
    states[929] = new State(new int[]{144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,933,14,935,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939,6,954},new int[]{-353,930,-20,931,-171,336,-173,279,-172,283,-21,337,-355,932,-350,936,-290,919,-186,181,-152,218,-156,49,-157,52,-351,937,-352,938});
    states[930] = new State(-684);
    states[931] = new State(-685);
    states[932] = new State(-686);
    states[933] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,934,-156,49,-157,52});
    states[934] = new State(-692);
    states[935] = new State(-687);
    states[936] = new State(-688);
    states[937] = new State(-689);
    states[938] = new State(-690);
    states[939] = new State(new int[]{14,944,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,53,948,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-364,940,-354,953,-20,945,-171,336,-173,279,-172,283,-21,337,-205,946,-350,950,-290,919,-186,181,-152,218,-156,49,-157,52,-351,951,-352,952});
    states[940] = new State(new int[]{9,941,100,942});
    states[941] = new State(-693);
    states[942] = new State(new int[]{14,944,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,53,948,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-354,943,-20,945,-171,336,-173,279,-172,283,-21,337,-205,946,-350,950,-290,919,-186,181,-152,218,-156,49,-157,52,-351,951,-352,952});
    states[943] = new State(-702);
    states[944] = new State(-694);
    states[945] = new State(-695);
    states[946] = new State(new int[]{144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290},new int[]{-20,947,-171,336,-173,279,-172,283,-21,337});
    states[947] = new State(-696);
    states[948] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,949,-156,49,-157,52});
    states[949] = new State(-697);
    states[950] = new State(-698);
    states[951] = new State(-699);
    states[952] = new State(-700);
    states[953] = new State(-701);
    states[954] = new State(-691);
    states[955] = new State(-683);
    states[956] = new State(-725);
    states[957] = new State(-703);
    states[958] = new State(new int[]{5,959});
    states[959] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514,32,-514,92,-514},new int[]{-267,960,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[960] = new State(-560);
    states[961] = new State(new int[]{100,962,5,-674});
    states[962] = new State(new int[]{144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,56,966,21,294,22,299},new int[]{-347,963,-20,964,-171,336,-173,279,-172,283,-21,337,-290,965,-186,181,-152,218,-156,49,-157,52,-264,967,-301,968});
    states[963] = new State(-676);
    states[964] = new State(-677);
    states[965] = new State(-678);
    states[966] = new State(-679);
    states[967] = new State(-680);
    states[968] = new State(-681);
    states[969] = new State(-675);
    states[970] = new State(new int[]{5,971});
    states[971] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514,32,-514,92,-514},new int[]{-267,972,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[972] = new State(-561);
    states[973] = new State(new int[]{39,974,5,978});
    states[974] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,975,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[975] = new State(new int[]{5,976});
    states[976] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514,32,-514,92,-514},new int[]{-267,977,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[977] = new State(-562);
    states[978] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514,32,-514,92,-514},new int[]{-267,979,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[979] = new State(-563);
    states[980] = new State(-557);
    states[981] = new State(-1041);
    states[982] = new State(-1042);
    states[983] = new State(-1043);
    states[984] = new State(-1044);
    states[985] = new State(-1045);
    states[986] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,878,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-330,882,-105,659,-331,677});
    states[987] = new State(new int[]{9,995,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339},new int[]{-95,988,-70,989,-252,993,-96,236,-86,246,-18,251,-15,261,-19,222,-152,999,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855,-69,273,-90,1003,-89,276,-100,1004,-250,1005,-251,1006,-253,1013,-141,1009});
    states[988] = new State(new int[]{9,840,13,206,16,210,100,-216});
    states[989] = new State(new int[]{9,990});
    states[990] = new State(new int[]{127,991,92,-219,10,-219,98,-219,101,-219,33,-219,104,-219,2,-219,9,-219,12,-219,100,-219,99,-219,32,-219,85,-219,84,-219,83,-219,82,-219,81,-219,86,-219});
    states[991] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,992,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[992] = new State(-437);
    states[993] = new State(new int[]{9,994});
    states[994] = new State(-224);
    states[995] = new State(new int[]{5,546,127,-1030},new int[]{-332,996});
    states[996] = new State(new int[]{127,997});
    states[997] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,998,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[998] = new State(-436);
    states[999] = new State(new int[]{4,-191,11,-191,7,-191,142,-191,8,-191,136,-191,138,-191,118,-191,117,-191,131,-191,132,-191,133,-191,134,-191,130,-191,116,-191,115,-191,128,-191,129,-191,120,-191,125,-191,123,-191,121,-191,124,-191,122,-191,137,-191,135,-191,9,-191,13,-191,16,-191,100,-191,119,-191,5,-230});
    states[1000] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339,9,-220},new int[]{-95,988,-70,1001,-252,993,-96,236,-86,246,-18,251,-15,261,-19,222,-152,999,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855,-69,273,-90,1003,-89,276,-100,1004,-250,1005,-251,1006,-253,1013,-141,1009});
    states[1001] = new State(new int[]{9,1002});
    states[1002] = new State(-219);
    states[1003] = new State(-222);
    states[1004] = new State(-217);
    states[1005] = new State(-218);
    states[1006] = new State(new int[]{10,1007,9,-225});
    states[1007] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,9,-226},new int[]{-253,1008,-141,1009,-152,1012,-156,49,-157,52});
    states[1008] = new State(-228);
    states[1009] = new State(new int[]{5,1010});
    states[1010] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339},new int[]{-89,1011,-95,277,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855,-100,1004,-250,1005});
    states[1011] = new State(-229);
    states[1012] = new State(-230);
    states[1013] = new State(-227);
    states[1014] = new State(-434);
    states[1015] = new State(-438);
    states[1016] = new State(-427);
    states[1017] = new State(-428);
    states[1018] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-93,1019,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,834,-105,659,-331,677});
    states[1019] = new State(-430);
    states[1020] = new State(new int[]{143,1021});
    states[1021] = new State(-580);
    states[1022] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,1023,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1023] = new State(-582);
    states[1024] = new State(new int[]{8,1032,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1025,-156,49,-157,52});
    states[1025] = new State(new int[]{5,1022,137,-583},new int[]{-281,1026});
    states[1026] = new State(new int[]{137,1027});
    states[1027] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,1028,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[1028] = new State(new int[]{87,1020,99,-581},new int[]{-368,1029});
    states[1029] = new State(new int[]{99,1030});
    states[1030] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514,9,-514,12,-514,100,-514,99,-514,32,-514,84,-514,83,-514,82,-514,81,-514},new int[]{-267,1031,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1031] = new State(-589);
    states[1032] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1033,-152,809,-156,49,-157,52});
    states[1033] = new State(new int[]{9,1034,100,544});
    states[1034] = new State(new int[]{137,1035});
    states[1035] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,1036,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[1036] = new State(new int[]{87,1020,99,-581},new int[]{-368,1037});
    states[1037] = new State(new int[]{99,1038});
    states[1038] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514,9,-514,12,-514,100,-514,99,-514,32,-514,84,-514,83,-514,82,-514,81,-514},new int[]{-267,1039,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1039] = new State(-590);
    states[1040] = new State(new int[]{5,1041});
    states[1041] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514},new int[]{-268,1042,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[1042] = new State(-513);
    states[1043] = new State(new int[]{79,1051,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514,92,-514},new int[]{-64,1044,-67,1046,-66,1063,-259,1064,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[1044] = new State(new int[]{92,1045});
    states[1045] = new State(-597);
    states[1046] = new State(new int[]{10,1048,32,1061,92,-603},new int[]{-261,1047});
    states[1047] = new State(-598);
    states[1048] = new State(new int[]{79,1051,32,1061,92,-603},new int[]{-66,1049,-261,1050});
    states[1049] = new State(-602);
    states[1050] = new State(-599);
    states[1051] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-68,1052,-185,1055,-186,1056,-152,1057,-156,49,-157,52,-145,1058});
    states[1052] = new State(new int[]{99,1053});
    states[1053] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514,32,-514,92,-514},new int[]{-267,1054,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1054] = new State(-605);
    states[1055] = new State(-606);
    states[1056] = new State(new int[]{7,182,99,-608});
    states[1057] = new State(new int[]{7,-282,99,-282,5,-609});
    states[1058] = new State(new int[]{5,1059});
    states[1059] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-185,1060,-186,1056,-152,218,-156,49,-157,52});
    states[1060] = new State(-607);
    states[1061] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514,92,-514},new int[]{-259,1062,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[1062] = new State(new int[]{10,19,92,-604});
    states[1063] = new State(-601);
    states[1064] = new State(new int[]{10,19,92,-600});
    states[1065] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,1066,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[1066] = new State(new int[]{99,1067});
    states[1067] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514,98,-514,101,-514,33,-514,104,-514,2,-514,9,-514,12,-514,100,-514,99,-514,32,-514,84,-514,83,-514,82,-514,81,-514},new int[]{-267,1068,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1068] = new State(-592);
    states[1069] = new State(-577);
    states[1070] = new State(-586);
    states[1071] = new State(-587);
    states[1072] = new State(-584);
    states[1073] = new State(new int[]{5,1074,100,1076});
    states[1074] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514,32,-514,92,-514},new int[]{-267,1075,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1075] = new State(-569);
    states[1076] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-115,1077,-99,1078,-95,205,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[1077] = new State(-571);
    states[1078] = new State(-572);
    states[1079] = new State(-570);
    states[1080] = new State(new int[]{92,1081});
    states[1081] = new State(-566);
    states[1082] = new State(-567);
    states[1083] = new State(new int[]{5,1084,10,1097,11,-813,17,-813,8,-813,7,-813,142,-813,4,-813,15,-813,110,-813,111,-813,112,-813,113,-813,114,-813,138,-813,136,-813,118,-813,117,-813,131,-813,132,-813,133,-813,134,-813,130,-813,116,-813,115,-813,128,-813,129,-813,126,-813,6,-813,120,-813,125,-813,123,-813,121,-813,124,-813,122,-813,137,-813,135,-813,16,-813,9,-813,70,-813,100,-813,13,-813,119,-813});
    states[1084] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,1085,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1085] = new State(new int[]{9,1086,10,1090});
    states[1086] = new State(new int[]{5,661,127,-1032},new int[]{-333,1087});
    states[1087] = new State(new int[]{127,1088});
    states[1088] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,1089,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[1089] = new State(-1012);
    states[1090] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-334,1091,-335,1096,-163,542,-152,809,-156,49,-157,52});
    states[1091] = new State(new int[]{9,1092,10,540});
    states[1092] = new State(new int[]{5,661,127,-1032},new int[]{-333,1093});
    states[1093] = new State(new int[]{127,1094});
    states[1094] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,1095,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[1095] = new State(-1014);
    states[1096] = new State(-1027);
    states[1097] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-334,1098,-335,1096,-163,542,-152,809,-156,49,-157,52});
    states[1098] = new State(new int[]{9,1099,10,540});
    states[1099] = new State(new int[]{5,661,127,-1032},new int[]{-333,1100});
    states[1100] = new State(new int[]{127,1101});
    states[1101] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,1102,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[1102] = new State(-1013);
    states[1103] = new State(new int[]{9,-625,70,-625,100,-1006});
    states[1104] = new State(-810);
    states[1105] = new State(-1007);
    states[1106] = new State(-623);
    states[1107] = new State(-614);
    states[1108] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,405},new int[]{-153,1109,-152,455,-156,49,-157,52,-299,456,-155,58,-197,457});
    states[1109] = new State(-828);
    states[1110] = new State(-830);
    states[1111] = new State(new int[]{123,187},new int[]{-305,1112});
    states[1112] = new State(-831);
    states[1113] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,53,1114,18,669},new int[]{-92,438,-367,440,-107,532,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-366,1104,-105,1105});
    states[1114] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1115,-156,49,-157,52});
    states[1115] = new State(new int[]{110,1116});
    states[1116] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,692,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[1117] = new State(new int[]{11,393,17,400,8,702,7,1108,142,1110,4,1111,9,-544,100,-544});
    states[1118] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,1113,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368},new int[]{-116,1119,-121,1120,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728});
    states[1119] = new State(new int[]{11,393,17,400,8,702,7,1108,142,1110,4,1111,15,1118,110,-795,111,-795,112,-795,113,-795,114,-795,92,-795,10,-795,98,-795,101,-795,33,-795,104,-795,2,-795,138,-795,136,-795,118,-795,117,-795,131,-795,132,-795,133,-795,134,-795,130,-795,116,-795,115,-795,128,-795,129,-795,126,-795,6,-795,5,-795,120,-795,125,-795,123,-795,121,-795,124,-795,122,-795,137,-795,135,-795,16,-795,70,-795,9,-795,12,-795,100,-795,99,-795,32,-795,85,-795,84,-795,83,-795,82,-795,81,-795,86,-795,13,-795,119,-795,76,-795,51,-795,58,-795,141,-795,143,-795,161,-795,80,-795,78,-795,160,-795,87,-795,45,-795,42,-795,21,-795,22,-795,144,-795,147,-795,145,-795,146,-795,155,-795,158,-795,157,-795,156,-795,57,-795,91,-795,40,-795,25,-795,97,-795,54,-795,35,-795,55,-795,102,-795,47,-795,36,-795,53,-795,60,-795,74,-795,72,-795,38,-795,71,-795});
    states[1120] = new State(-796);
    states[1121] = new State(new int[]{9,1122});
    states[1122] = new State(-1046);
    states[1123] = new State(new int[]{110,429,111,430,112,431,113,432,114,433,138,-791,136,-791,118,-791,117,-791,131,-791,132,-791,133,-791,134,-791,130,-791,116,-791,115,-791,128,-791,129,-791,126,-791,6,-791,5,-791,120,-791,125,-791,123,-791,121,-791,124,-791,122,-791,137,-791,135,-791,16,-791,9,-791,70,-791,100,-791,13,-791,2,-791,119,-784},new int[]{-200,24});
    states[1124] = new State(new int[]{53,687,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-367,440,-107,532,-116,694,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-366,1104,-105,1105});
    states[1125] = new State(-1050);
    states[1126] = new State(-1051);
    states[1127] = new State(new int[]{9,1128,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-334,1131,-335,1096,-163,542,-152,809,-156,49,-157,52});
    states[1128] = new State(new int[]{127,1129});
    states[1129] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,686,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-337,1130,-218,685,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-9,1125,-338,1126,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[1130] = new State(-1025);
    states[1131] = new State(new int[]{9,1132,10,540});
    states[1132] = new State(new int[]{127,1133});
    states[1133] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,686,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-337,1134,-218,685,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-9,1125,-338,1126,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[1134] = new State(-1026);
    states[1135] = new State(new int[]{9,1136,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-334,1140,-335,1096,-163,542,-152,809,-156,49,-157,52});
    states[1136] = new State(new int[]{5,661,127,-1032},new int[]{-333,1137});
    states[1137] = new State(new int[]{127,1138});
    states[1138] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,1139,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[1139] = new State(-1022);
    states[1140] = new State(new int[]{9,1141,10,540});
    states[1141] = new State(new int[]{5,661,127,-1032},new int[]{-333,1142});
    states[1142] = new State(new int[]{127,1143});
    states[1143] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-336,1144,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,657,-122,580,-330,658,-105,659,-331,677,-338,718,-262,719,-158,720,-326,859,-254,860,-129,861,-128,862,-130,863,-39,981,-308,982,-175,983,-255,984,-131,985});
    states[1144] = new State(-1023);
    states[1145] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,18,669},new int[]{-107,1146,-106,1147,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-105,1105});
    states[1146] = new State(-1009);
    states[1147] = new State(-1006);
    states[1148] = new State(-1008);
    states[1149] = new State(new int[]{5,1150,7,-282,8,-282,123,-282,12,-282,100,-282});
    states[1150] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-14,1151,-186,634,-152,218,-156,49,-157,52,-307,1152});
    states[1151] = new State(-239);
    states[1152] = new State(new int[]{8,637,12,-663,100,-663},new int[]{-74,1153});
    states[1153] = new State(-804);
    states[1154] = new State(-236);
    states[1155] = new State(-232);
    states[1156] = new State(-493);
    states[1157] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1158,-156,49,-157,52});
    states[1158] = new State(new int[]{110,689,100,1159});
    states[1159] = new State(new int[]{53,1167},new int[]{-345,1160});
    states[1160] = new State(new int[]{9,1161,100,1164});
    states[1161] = new State(new int[]{110,1162});
    states[1162] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,1163,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[1163] = new State(new int[]{70,27,92,-540,10,-540,98,-540,101,-540,33,-540,104,-540,2,-540,9,-540,12,-540,100,-540,99,-540,32,-540,85,-540,84,-540,83,-540,82,-540,81,-540,86,-540});
    states[1164] = new State(new int[]{53,1165});
    states[1165] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1166,-156,49,-157,52});
    states[1166] = new State(-547);
    states[1167] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1168,-156,49,-157,52});
    states[1168] = new State(-546);
    states[1169] = new State(new int[]{148,1173,150,1174,151,1175,152,1176,154,1177,153,1178,107,-850,91,-850,59,-850,29,-850,66,-850,50,-850,53,-850,62,-850,11,-850,28,-850,26,-850,44,-850,37,-850,19,-850,30,-850,31,-850,46,-850,27,-850,92,-850,84,-850,83,-850,82,-850,81,-850,23,-850,149,-850,161,-850,41,-850},new int[]{-212,1170,-215,1179});
    states[1170] = new State(new int[]{10,1171});
    states[1171] = new State(new int[]{148,1173,150,1174,151,1175,152,1176,154,1177,153,1178,107,-851,91,-851,59,-851,29,-851,66,-851,50,-851,53,-851,62,-851,11,-851,28,-851,26,-851,44,-851,37,-851,19,-851,30,-851,31,-851,46,-851,27,-851,92,-851,84,-851,83,-851,82,-851,81,-851,23,-851,149,-851,161,-851,41,-851},new int[]{-215,1172});
    states[1172] = new State(-855);
    states[1173] = new State(-868);
    states[1174] = new State(-869);
    states[1175] = new State(-870);
    states[1176] = new State(-871);
    states[1177] = new State(-872);
    states[1178] = new State(-873);
    states[1179] = new State(-854);
    states[1180] = new State(-398);
    states[1181] = new State(-467);
    states[1182] = new State(-468);
    states[1183] = new State(new int[]{8,-473,110,-473,10,-473,11,-473,5,-473,7,-470});
    states[1184] = new State(new int[]{123,1186,8,-476,110,-476,10,-476,7,-476,11,-476,5,-476},new int[]{-160,1185});
    states[1185] = new State(-477);
    states[1186] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1187,-152,809,-156,49,-157,52});
    states[1187] = new State(new int[]{121,1188,100,544});
    states[1188] = new State(-343);
    states[1189] = new State(-478);
    states[1190] = new State(new int[]{123,1186,8,-474,110,-474,10,-474,11,-474,5,-474},new int[]{-160,1191});
    states[1191] = new State(-475);
    states[1192] = new State(new int[]{7,1193});
    states[1193] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-147,1194,-154,1195,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190});
    states[1194] = new State(-469);
    states[1195] = new State(-472);
    states[1196] = new State(-471);
    states[1197] = new State(-453);
    states[1198] = new State(new int[]{44,1328,37,1363},new int[]{-222,1199,-234,1200,-231,1327,-235,1362});
    states[1199] = new State(-455);
    states[1200] = new State(new int[]{107,1318,59,-85,29,-85,66,-85,50,-85,53,-85,62,-85,91,-85},new int[]{-182,1201,-47,1202,-43,1205,-65,1317});
    states[1201] = new State(-461);
    states[1202] = new State(new int[]{91,16},new int[]{-262,1203});
    states[1203] = new State(new int[]{10,1204});
    states[1204] = new State(-488);
    states[1205] = new State(new int[]{59,1208,29,1229,66,1233,50,1445,53,1460,62,1462,91,-84},new int[]{-50,1206,-174,1207,-33,1214,-56,1231,-295,1235,-317,1447});
    states[1206] = new State(-86);
    states[1207] = new State(-108);
    states[1208] = new State(new int[]{155,733,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-161,1209,-148,1213,-152,734,-156,49,-157,52});
    states[1209] = new State(new int[]{10,1210,100,1211});
    states[1210] = new State(-117);
    states[1211] = new State(new int[]{155,733,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-148,1212,-152,734,-156,49,-157,52});
    states[1212] = new State(-119);
    states[1213] = new State(-118);
    states[1214] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-109,29,-109,66,-109,50,-109,53,-109,62,-109,91,-109},new int[]{-31,1215,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1215] = new State(-123);
    states[1216] = new State(new int[]{10,1217});
    states[1217] = new State(-133);
    states[1218] = new State(new int[]{120,1219,5,1224});
    states[1219] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1222,135,841,116,353,115,354,63,339},new int[]{-114,1220,-95,1221,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855,-100,1223});
    states[1220] = new State(-134);
    states[1221] = new State(new int[]{13,206,16,210,10,-136,92,-136,84,-136,83,-136,82,-136,81,-136});
    states[1222] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339,9,-220},new int[]{-95,988,-70,1001,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855,-69,273,-90,1003,-89,276,-100,1004,-250,1005});
    states[1223] = new State(-137);
    states[1224] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,1225,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1225] = new State(new int[]{120,1226});
    states[1226] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339},new int[]{-89,1227,-95,277,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855,-100,1004,-250,1005});
    states[1227] = new State(-135);
    states[1228] = new State(-138);
    states[1229] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-31,1230,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1230] = new State(-122);
    states[1231] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-110,29,-110,66,-110,50,-110,53,-110,62,-110,91,-110},new int[]{-31,1232,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1232] = new State(-125);
    states[1233] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-31,1234,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1234] = new State(-124);
    states[1235] = new State(new int[]{11,628,59,-111,29,-111,66,-111,50,-111,53,-111,62,-111,91,-111,143,-234,161,-234,85,-234,86,-234,80,-234,78,-234,160,-234,87,-234},new int[]{-53,1236,-11,1237,-257,1155});
    states[1236] = new State(-127);
    states[1237] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,628},new int[]{-54,1238,-257,518,-149,1239,-152,1437,-156,49,-157,52,-150,1442});
    states[1238] = new State(-231);
    states[1239] = new State(new int[]{120,1240});
    states[1240] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604,68,1431,69,1432,148,1433,27,1434,28,1435,26,-325,43,-325,64,-325},new int[]{-293,1241,-283,1243,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562,-34,1244,-25,1245,-26,1429,-24,1436});
    states[1241] = new State(new int[]{10,1242});
    states[1242] = new State(-240);
    states[1243] = new State(-245);
    states[1244] = new State(-246);
    states[1245] = new State(new int[]{26,1423,43,1424,64,1425},new int[]{-297,1246});
    states[1246] = new State(new int[]{8,1344,23,-337,11,-337,92,-337,84,-337,83,-337,82,-337,81,-337,29,-337,143,-337,161,-337,85,-337,86,-337,80,-337,78,-337,160,-337,87,-337,62,-337,28,-337,26,-337,44,-337,37,-337,19,-337,30,-337,31,-337,46,-337,27,-337,10,-337},new int[]{-189,1247});
    states[1247] = new State(new int[]{23,1335,11,-344,92,-344,84,-344,83,-344,82,-344,81,-344,29,-344,143,-344,161,-344,85,-344,86,-344,80,-344,78,-344,160,-344,87,-344,62,-344,28,-344,26,-344,44,-344,37,-344,19,-344,30,-344,31,-344,46,-344,27,-344,10,-344},new int[]{-325,1248,-324,1333,-323,1355});
    states[1248] = new State(new int[]{11,628,10,-335,92,-361,84,-361,83,-361,82,-361,81,-361,29,-234,143,-234,161,-234,85,-234,86,-234,80,-234,78,-234,160,-234,87,-234,62,-234,28,-234,26,-234,44,-234,37,-234,19,-234,30,-234,31,-234,46,-234,27,-234},new int[]{-30,1249,-29,1250,-36,1256,-38,509,-49,1257,-11,1258,-257,1155,-37,1420,-58,1422,-57,515,-59,1421});
    states[1249] = new State(-318);
    states[1250] = new State(new int[]{92,1251,84,1252,83,1253,82,1254,81,1255},new int[]{-12,507});
    states[1251] = new State(-336);
    states[1252] = new State(-357);
    states[1253] = new State(-358);
    states[1254] = new State(-359);
    states[1255] = new State(-360);
    states[1256] = new State(-355);
    states[1257] = new State(-369);
    states[1258] = new State(new int[]{29,1260,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,62,1264,28,1380,26,1381,11,628,44,1328,37,1363,19,1383,30,1391,31,1398,46,1405,27,1414},new int[]{-55,1259,-257,518,-228,517,-225,519,-265,520,-320,1262,-319,1263,-163,810,-152,809,-156,49,-157,52,-8,1268,-236,1382,-234,1316,-231,1327,-235,1362,-233,1389,-221,1402,-222,1403,-224,1404});
    states[1259] = new State(-371);
    states[1260] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-32,1261,-146,1218,-152,1228,-156,49,-157,52});
    states[1261] = new State(-376);
    states[1262] = new State(-377);
    states[1263] = new State(-381);
    states[1264] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1265,-152,809,-156,49,-157,52});
    states[1265] = new State(new int[]{5,1266,100,544});
    states[1266] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,1267,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1267] = new State(-382);
    states[1268] = new State(new int[]{30,523,19,1198,46,1270,27,1308,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,62,1264,44,1328,37,1363},new int[]{-320,1269,-236,522,-222,1197,-319,1263,-163,810,-152,809,-156,49,-157,52,-234,1316,-231,1327,-235,1362});
    states[1269] = new State(-378);
    states[1270] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-176,1271,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1271] = new State(new int[]{11,1299,5,-412},new int[]{-239,1272,-244,1296});
    states[1272] = new State(new int[]{85,1285,86,1291,10,-419},new int[]{-208,1273});
    states[1273] = new State(new int[]{10,1274});
    states[1274] = new State(new int[]{63,1279,153,1281,152,1282,148,1283,151,1284,11,-409,28,-409,26,-409,44,-409,37,-409,19,-409,30,-409,31,-409,46,-409,27,-409,92,-409,84,-409,83,-409,82,-409,81,-409},new int[]{-211,1275,-216,1276});
    states[1275] = new State(-403);
    states[1276] = new State(new int[]{10,1277});
    states[1277] = new State(new int[]{63,1279,11,-409,28,-409,26,-409,44,-409,37,-409,19,-409,30,-409,31,-409,46,-409,27,-409,92,-409,84,-409,83,-409,82,-409,81,-409},new int[]{-211,1278});
    states[1278] = new State(-404);
    states[1279] = new State(new int[]{10,1280});
    states[1280] = new State(-410);
    states[1281] = new State(-874);
    states[1282] = new State(-875);
    states[1283] = new State(-876);
    states[1284] = new State(-877);
    states[1285] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,10,-418},new int[]{-119,1286,-93,1290,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,834,-105,659,-331,677});
    states[1286] = new State(new int[]{86,1288,10,-422},new int[]{-209,1287});
    states[1287] = new State(-420);
    states[1288] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514},new int[]{-267,1289,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1289] = new State(-423);
    states[1290] = new State(-417);
    states[1291] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514},new int[]{-267,1292,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1292] = new State(new int[]{85,1294,10,-424},new int[]{-210,1293});
    states[1293] = new State(-421);
    states[1294] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,10,-418},new int[]{-119,1295,-93,1290,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,834,-105,659,-331,677});
    states[1295] = new State(-425);
    states[1296] = new State(new int[]{5,1297});
    states[1297] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,1298,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1298] = new State(-411);
    states[1299] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-243,1300,-242,1307,-163,1304,-152,809,-156,49,-157,52});
    states[1300] = new State(new int[]{12,1301,10,1302});
    states[1301] = new State(-413);
    states[1302] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-242,1303,-163,1304,-152,809,-156,49,-157,52});
    states[1303] = new State(-415);
    states[1304] = new State(new int[]{5,1305,100,544});
    states[1305] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,1306,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1306] = new State(-416);
    states[1307] = new State(-414);
    states[1308] = new State(new int[]{46,1309});
    states[1309] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-176,1310,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1310] = new State(new int[]{11,1299,5,-412},new int[]{-239,1311,-244,1296});
    states[1311] = new State(new int[]{110,1314,10,-408},new int[]{-217,1312});
    states[1312] = new State(new int[]{10,1313});
    states[1313] = new State(-406);
    states[1314] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,1315,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[1315] = new State(new int[]{70,27,10,-407});
    states[1316] = new State(new int[]{107,1318,11,-392,28,-392,26,-392,44,-392,37,-392,19,-392,30,-392,31,-392,46,-392,27,-392,92,-392,84,-392,83,-392,82,-392,81,-392,59,-85,29,-85,66,-85,50,-85,53,-85,62,-85,91,-85},new int[]{-182,1201,-47,1202,-43,1205,-65,1317});
    states[1317] = new State(-489);
    states[1318] = new State(new int[]{10,1326,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,144,281,147,282,145,284,146,285},new int[]{-113,1319,-152,1323,-156,49,-157,52,-171,1324,-173,279,-172,283});
    states[1319] = new State(new int[]{80,1320,10,1325});
    states[1320] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,144,281,147,282,145,284,146,285},new int[]{-113,1321,-152,1323,-156,49,-157,52,-171,1324,-173,279,-172,283});
    states[1321] = new State(new int[]{10,1322});
    states[1322] = new State(-482);
    states[1323] = new State(-485);
    states[1324] = new State(-486);
    states[1325] = new State(-483);
    states[1326] = new State(-484);
    states[1327] = new State(-393);
    states[1328] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-177,1329,-176,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1329] = new State(new int[]{8,565,10,-490,110,-490},new int[]{-133,1330});
    states[1330] = new State(new int[]{10,1360,110,-852},new int[]{-213,1331,-214,1356});
    states[1331] = new State(new int[]{23,1335,107,-344,91,-344,59,-344,29,-344,66,-344,50,-344,53,-344,62,-344,11,-344,28,-344,26,-344,44,-344,37,-344,19,-344,30,-344,31,-344,46,-344,27,-344,92,-344,84,-344,83,-344,82,-344,81,-344,149,-344,161,-344,41,-344},new int[]{-325,1332,-324,1333,-323,1355});
    states[1332] = new State(-479);
    states[1333] = new State(new int[]{23,1335,11,-345,92,-345,84,-345,83,-345,82,-345,81,-345,29,-345,143,-345,161,-345,85,-345,86,-345,80,-345,78,-345,160,-345,87,-345,62,-345,28,-345,26,-345,44,-345,37,-345,19,-345,30,-345,31,-345,46,-345,27,-345,10,-345,107,-345,91,-345,59,-345,66,-345,50,-345,53,-345,149,-345,41,-345},new int[]{-323,1334});
    states[1334] = new State(-347);
    states[1335] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1336,-152,809,-156,49,-157,52});
    states[1336] = new State(new int[]{5,1337,100,544});
    states[1337] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,1343,49,549,34,553,73,557,44,563,37,604,26,1352,30,1353},new int[]{-294,1338,-292,1354,-283,1342,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1338] = new State(new int[]{10,1339,100,1340});
    states[1339] = new State(-348);
    states[1340] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,1343,49,549,34,553,73,557,44,563,37,604,26,1352,30,1353},new int[]{-292,1341,-283,1342,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1341] = new State(-350);
    states[1342] = new State(-351);
    states[1343] = new State(new int[]{8,1344,10,-353,100,-353,23,-337,11,-337,92,-337,84,-337,83,-337,82,-337,81,-337,29,-337,143,-337,161,-337,85,-337,86,-337,80,-337,78,-337,160,-337,87,-337,62,-337,28,-337,26,-337,44,-337,37,-337,19,-337,30,-337,31,-337,46,-337,27,-337},new int[]{-189,503});
    states[1344] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-188,1345,-187,1351,-186,1349,-152,218,-156,49,-157,52,-307,1350});
    states[1345] = new State(new int[]{9,1346,100,1347});
    states[1346] = new State(-338);
    states[1347] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-187,1348,-186,1349,-152,218,-156,49,-157,52,-307,1350});
    states[1348] = new State(-340);
    states[1349] = new State(new int[]{7,182,123,187,9,-341,100,-341},new int[]{-305,636});
    states[1350] = new State(-342);
    states[1351] = new State(-339);
    states[1352] = new State(-352);
    states[1353] = new State(-354);
    states[1354] = new State(-349);
    states[1355] = new State(-346);
    states[1356] = new State(new int[]{110,1357});
    states[1357] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514},new int[]{-267,1358,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1358] = new State(new int[]{10,1359});
    states[1359] = new State(-464);
    states[1360] = new State(new int[]{148,1173,150,1174,151,1175,152,1176,154,1177,153,1178,23,-850,107,-850,91,-850,59,-850,29,-850,66,-850,50,-850,53,-850,62,-850,11,-850,28,-850,26,-850,44,-850,37,-850,19,-850,30,-850,31,-850,46,-850,27,-850,92,-850,84,-850,83,-850,82,-850,81,-850,149,-850},new int[]{-212,1361,-215,1179});
    states[1361] = new State(new int[]{10,1171,110,-853});
    states[1362] = new State(-394);
    states[1363] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-176,1364,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1364] = new State(new int[]{8,565,5,-490,10,-490,110,-490},new int[]{-133,1365});
    states[1365] = new State(new int[]{5,1368,10,1360,110,-852},new int[]{-213,1366,-214,1376});
    states[1366] = new State(new int[]{23,1335,107,-344,91,-344,59,-344,29,-344,66,-344,50,-344,53,-344,62,-344,11,-344,28,-344,26,-344,44,-344,37,-344,19,-344,30,-344,31,-344,46,-344,27,-344,92,-344,84,-344,83,-344,82,-344,81,-344,149,-344,161,-344,41,-344},new int[]{-325,1367,-324,1333,-323,1355});
    states[1367] = new State(-480);
    states[1368] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,1369,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1369] = new State(new int[]{10,1360,110,-852},new int[]{-213,1370,-214,1372});
    states[1370] = new State(new int[]{23,1335,107,-344,91,-344,59,-344,29,-344,66,-344,50,-344,53,-344,62,-344,11,-344,28,-344,26,-344,44,-344,37,-344,19,-344,30,-344,31,-344,46,-344,27,-344,92,-344,84,-344,83,-344,82,-344,81,-344,149,-344,161,-344,41,-344},new int[]{-325,1371,-324,1333,-323,1355});
    states[1371] = new State(-481);
    states[1372] = new State(new int[]{110,1373});
    states[1373] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,1374,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-330,882,-105,659,-331,677});
    states[1374] = new State(new int[]{10,1375});
    states[1375] = new State(-462);
    states[1376] = new State(new int[]{110,1377});
    states[1377] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,1378,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-330,882,-105,659,-331,677});
    states[1378] = new State(new int[]{10,1379});
    states[1379] = new State(-463);
    states[1380] = new State(-379);
    states[1381] = new State(-380);
    states[1382] = new State(-388);
    states[1383] = new State(new int[]{28,1380,26,1381,44,1328,37,1363},new int[]{-8,1384,-236,1387,-222,1388,-234,1316,-231,1327,-235,1362});
    states[1384] = new State(new int[]{44,1328,37,1363},new int[]{-236,1385,-222,1386,-234,1316,-231,1327,-235,1362});
    states[1385] = new State(-389);
    states[1386] = new State(-454);
    states[1387] = new State(-390);
    states[1388] = new State(-452);
    states[1389] = new State(new int[]{107,1318,11,-391,28,-391,26,-391,44,-391,37,-391,19,-391,30,-391,31,-391,46,-391,27,-391,92,-391,84,-391,83,-391,82,-391,81,-391,59,-85,29,-85,66,-85,50,-85,53,-85,62,-85,91,-85},new int[]{-182,1390,-47,1202,-43,1205,-65,1317});
    states[1390] = new State(-443);
    states[1391] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,8,-399,110,-399,10,-399},new int[]{-178,1392,-177,1180,-176,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1392] = new State(new int[]{8,565,110,-490,10,-490},new int[]{-133,1393});
    states[1393] = new State(new int[]{110,1395,10,1169},new int[]{-213,1394});
    states[1394] = new State(-395);
    states[1395] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514},new int[]{-267,1396,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1396] = new State(new int[]{10,1397});
    states[1397] = new State(-444);
    states[1398] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,8,-399,10,-399},new int[]{-178,1399,-177,1180,-176,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1399] = new State(new int[]{8,565,10,-490},new int[]{-133,1400});
    states[1400] = new State(new int[]{10,1169},new int[]{-213,1401});
    states[1401] = new State(-397);
    states[1402] = new State(-385);
    states[1403] = new State(-451);
    states[1404] = new State(-386);
    states[1405] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-176,1406,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1406] = new State(new int[]{11,1299,5,-412},new int[]{-239,1407,-244,1296});
    states[1407] = new State(new int[]{85,1285,86,1291,10,-419},new int[]{-208,1408});
    states[1408] = new State(new int[]{10,1409});
    states[1409] = new State(new int[]{63,1279,153,1281,152,1282,148,1283,151,1284,11,-409,28,-409,26,-409,44,-409,37,-409,19,-409,30,-409,31,-409,46,-409,27,-409,92,-409,84,-409,83,-409,82,-409,81,-409},new int[]{-211,1410,-216,1411});
    states[1410] = new State(-401);
    states[1411] = new State(new int[]{10,1412});
    states[1412] = new State(new int[]{63,1279,11,-409,28,-409,26,-409,44,-409,37,-409,19,-409,30,-409,31,-409,46,-409,27,-409,92,-409,84,-409,83,-409,82,-409,81,-409},new int[]{-211,1413});
    states[1413] = new State(-402);
    states[1414] = new State(new int[]{46,1415});
    states[1415] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-176,1416,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1416] = new State(new int[]{11,1299,5,-412},new int[]{-239,1417,-244,1296});
    states[1417] = new State(new int[]{110,1314,10,-408},new int[]{-217,1418});
    states[1418] = new State(new int[]{10,1419});
    states[1419] = new State(-405);
    states[1420] = new State(new int[]{11,628,92,-363,84,-363,83,-363,82,-363,81,-363,28,-234,26,-234,44,-234,37,-234,19,-234,30,-234,31,-234,46,-234,27,-234},new int[]{-58,514,-57,515,-11,516,-257,1155,-59,1421});
    states[1421] = new State(-375);
    states[1422] = new State(-372);
    states[1423] = new State(-329);
    states[1424] = new State(-330);
    states[1425] = new State(new int[]{26,1426,48,1427,43,1428,8,-331,23,-331,11,-331,92,-331,84,-331,83,-331,82,-331,81,-331,29,-331,143,-331,161,-331,85,-331,86,-331,80,-331,78,-331,160,-331,87,-331,62,-331,28,-331,44,-331,37,-331,19,-331,30,-331,31,-331,46,-331,27,-331,10,-331});
    states[1426] = new State(-332);
    states[1427] = new State(-333);
    states[1428] = new State(-334);
    states[1429] = new State(new int[]{68,1431,69,1432,148,1433,27,1434,28,1435,26,-326,43,-326,64,-326},new int[]{-24,1430});
    states[1430] = new State(-328);
    states[1431] = new State(-320);
    states[1432] = new State(-321);
    states[1433] = new State(-322);
    states[1434] = new State(-323);
    states[1435] = new State(-324);
    states[1436] = new State(-327);
    states[1437] = new State(new int[]{123,1439,120,-242},new int[]{-160,1438});
    states[1438] = new State(-243);
    states[1439] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1440,-152,809,-156,49,-157,52});
    states[1440] = new State(new int[]{122,1441,121,1188,100,544});
    states[1441] = new State(-244);
    states[1442] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604,68,1431,69,1432,148,1433,27,1434,28,1435,26,-325,43,-325,64,-325},new int[]{-293,1443,-283,1243,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562,-34,1244,-25,1245,-26,1429,-24,1436});
    states[1443] = new State(new int[]{10,1444});
    states[1444] = new State(-241);
    states[1445] = new State(new int[]{11,628,143,-234,161,-234,85,-234,86,-234,80,-234,78,-234,160,-234,87,-234},new int[]{-53,1446,-11,1237,-257,1155});
    states[1446] = new State(-126);
    states[1447] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1452,59,-112,29,-112,66,-112,50,-112,53,-112,62,-112,91,-112},new int[]{-321,1448,-318,1449,-319,1450,-163,810,-152,809,-156,49,-157,52});
    states[1448] = new State(-132);
    states[1449] = new State(-128);
    states[1450] = new State(new int[]{10,1451});
    states[1451] = new State(-426);
    states[1452] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1453,-156,49,-157,52});
    states[1453] = new State(new int[]{100,1454});
    states[1454] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1455,-152,809,-156,49,-157,52});
    states[1455] = new State(new int[]{9,1456,100,544});
    states[1456] = new State(new int[]{110,1457});
    states[1457] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,1458,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[1458] = new State(new int[]{10,1459});
    states[1459] = new State(-129);
    states[1460] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1452},new int[]{-321,1461,-318,1449,-319,1450,-163,810,-152,809,-156,49,-157,52});
    states[1461] = new State(-130);
    states[1462] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1452},new int[]{-321,1463,-318,1449,-319,1450,-163,810,-152,809,-156,49,-157,52});
    states[1463] = new State(-131);
    states[1464] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,356,12,-301,100,-301},new int[]{-278,1465,-279,1466,-98,194,-111,314,-112,315,-186,352,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283});
    states[1465] = new State(-299);
    states[1466] = new State(-300);
    states[1467] = new State(-298);
    states[1468] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-283,1469,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1469] = new State(-297);
    states[1470] = new State(-267);
    states[1471] = new State(-268);
    states[1472] = new State(new int[]{127,483,121,-269,100,-269,120,-269,9,-269,10,-269,8,-269,138,-269,136,-269,118,-269,117,-269,131,-269,132,-269,133,-269,134,-269,130,-269,116,-269,115,-269,128,-269,129,-269,126,-269,6,-269,5,-269,125,-269,123,-269,124,-269,122,-269,137,-269,135,-269,16,-269,70,-269,92,-269,98,-269,101,-269,33,-269,104,-269,2,-269,12,-269,99,-269,32,-269,85,-269,84,-269,83,-269,82,-269,81,-269,86,-269,13,-269,76,-269,51,-269,58,-269,141,-269,143,-269,161,-269,80,-269,78,-269,160,-269,87,-269,45,-269,42,-269,21,-269,22,-269,144,-269,147,-269,145,-269,146,-269,155,-269,158,-269,157,-269,156,-269,11,-269,57,-269,91,-269,40,-269,25,-269,97,-269,54,-269,35,-269,55,-269,102,-269,47,-269,36,-269,53,-269,60,-269,74,-269,72,-269,38,-269,71,-269,110,-269});
    states[1473] = new State(-713);
    states[1474] = new State(new int[]{8,1475});
    states[1475] = new State(new int[]{14,472,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,474,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-361,1476,-359,1482,-20,473,-171,336,-173,279,-172,283,-21,337,-348,1473,-290,1474,-186,181,-152,218,-156,49,-157,52,-351,1480,-352,1481});
    states[1476] = new State(new int[]{9,1477,10,470,100,1478});
    states[1477] = new State(-671);
    states[1478] = new State(new int[]{14,472,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,474,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-359,1479,-20,473,-171,336,-173,279,-172,283,-21,337,-348,1473,-290,1474,-186,181,-152,218,-156,49,-157,52,-351,1480,-352,1481});
    states[1479] = new State(-708);
    states[1480] = new State(-714);
    states[1481] = new State(-715);
    states[1482] = new State(-706);
    states[1483] = new State(-802);
    states[1484] = new State(new int[]{13,482});
    states[1485] = new State(-262);
    states[1486] = new State(-258);
    states[1487] = new State(-647);
    states[1488] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,712,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,9,-835},new int[]{-72,1489,-77,705,-94,1107,-92,708,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,709,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-330,1106,-105,659,-331,677});
    states[1489] = new State(new int[]{9,1490});
    states[1490] = new State(-666);
    states[1491] = new State(new int[]{8,1492});
    states[1492] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-341,1493,-340,1501,-152,1497,-156,49,-157,52,-106,1500,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[1493] = new State(new int[]{9,1494,100,1495});
    states[1494] = new State(-658);
    states[1495] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-340,1496,-152,1497,-156,49,-157,52,-106,1500,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[1496] = new State(-662);
    states[1497] = new State(new int[]{110,1498,11,-813,17,-813,8,-813,7,-813,142,-813,4,-813,15,-813,138,-813,136,-813,118,-813,117,-813,131,-813,132,-813,133,-813,134,-813,130,-813,116,-813,115,-813,128,-813,129,-813,126,-813,6,-813,120,-813,125,-813,123,-813,121,-813,124,-813,122,-813,137,-813,135,-813,16,-813,9,-813,100,-813,13,-813,119,-813});
    states[1498] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,1499,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588});
    states[1499] = new State(-659);
    states[1500] = new State(-660);
    states[1501] = new State(-661);
    states[1502] = new State(new int[]{13,206,16,210,5,-728,12,-728});
    states[1503] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,1504,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[1504] = new State(new int[]{13,206,16,210,100,-211,9,-211,12,-211,5,-211});
    states[1505] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,5,-729,12,-729},new int[]{-127,1506,-95,1502,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[1506] = new State(new int[]{5,1507,12,-735});
    states[1507] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,1508,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-171,278,-173,279,-172,283,-21,286,-264,293,-301,298,-245,366,-246,367,-205,847,-179,845,-61,846,-272,853,-276,854,-16,849,-248,855});
    states[1508] = new State(new int[]{13,206,16,210,12,-737});
    states[1509] = new State(-208);
    states[1510] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285},new int[]{-98,1511,-111,314,-112,315,-186,352,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283});
    states[1511] = new State(new int[]{116,247,115,248,128,249,129,250,13,-271,121,-271,100,-271,120,-271,9,-271,10,-271,8,-271,138,-271,136,-271,118,-271,117,-271,131,-271,132,-271,133,-271,134,-271,130,-271,126,-271,6,-271,5,-271,125,-271,123,-271,124,-271,122,-271,137,-271,135,-271,16,-271,70,-271,92,-271,98,-271,101,-271,33,-271,104,-271,2,-271,12,-271,99,-271,32,-271,85,-271,84,-271,83,-271,82,-271,81,-271,86,-271,76,-271,51,-271,58,-271,141,-271,143,-271,161,-271,80,-271,78,-271,160,-271,87,-271,45,-271,42,-271,21,-271,22,-271,144,-271,147,-271,145,-271,146,-271,155,-271,158,-271,157,-271,156,-271,11,-271,57,-271,91,-271,40,-271,25,-271,97,-271,54,-271,35,-271,55,-271,102,-271,47,-271,36,-271,53,-271,60,-271,74,-271,72,-271,38,-271,71,-271,127,-271,110,-271},new int[]{-199,195});
    states[1512] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,24,494},new int[]{-290,1513,-285,1514,-186,181,-152,218,-156,49,-157,52,-277,500});
    states[1513] = new State(-760);
    states[1514] = new State(-761);
    states[1515] = new State(-774);
    states[1516] = new State(-775);
    states[1517] = new State(-776);
    states[1518] = new State(-777);
    states[1519] = new State(-778);
    states[1520] = new State(-779);
    states[1521] = new State(-780);
    states[1522] = new State(-750);
    states[1523] = new State(-669);
    states[1524] = new State(-35);
    states[1525] = new State(new int[]{59,1208,29,1229,66,1233,50,1445,53,1460,62,1462,11,628,91,-79,92,-79,103,-79,44,-234,37,-234,28,-234,26,-234,19,-234,30,-234,31,-234},new int[]{-51,1526,-174,1527,-33,1528,-56,1529,-295,1530,-317,1531,-226,1532,-11,1533,-257,1155});
    states[1526] = new State(-83);
    states[1527] = new State(-99);
    states[1528] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-100,29,-100,66,-100,50,-100,53,-100,62,-100,11,-100,44,-100,37,-100,28,-100,26,-100,19,-100,30,-100,31,-100,91,-100,92,-100,103,-100},new int[]{-31,1215,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1529] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-101,29,-101,66,-101,50,-101,53,-101,62,-101,11,-101,44,-101,37,-101,28,-101,26,-101,19,-101,30,-101,31,-101,91,-101,92,-101,103,-101},new int[]{-31,1232,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1530] = new State(new int[]{11,628,59,-102,29,-102,66,-102,50,-102,53,-102,62,-102,44,-102,37,-102,28,-102,26,-102,19,-102,30,-102,31,-102,91,-102,92,-102,103,-102,143,-234,161,-234,85,-234,86,-234,80,-234,78,-234,160,-234,87,-234},new int[]{-53,1236,-11,1237,-257,1155});
    states[1531] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1452,59,-103,29,-103,66,-103,50,-103,53,-103,62,-103,11,-103,44,-103,37,-103,28,-103,26,-103,19,-103,30,-103,31,-103,91,-103,92,-103,103,-103},new int[]{-321,1448,-318,1449,-319,1450,-163,810,-152,809,-156,49,-157,52});
    states[1532] = new State(-104);
    states[1533] = new State(new int[]{44,1546,37,1553,28,1380,26,1381,19,1576,30,1583,31,1398,11,628},new int[]{-219,1534,-257,518,-220,1535,-227,1536,-234,1537,-231,1327,-235,1362,-8,1566,-223,1580,-233,1581});
    states[1534] = new State(-107);
    states[1535] = new State(-105);
    states[1536] = new State(-446);
    states[1537] = new State(new int[]{149,1539,107,1318,59,-82,29,-82,66,-82,50,-82,53,-82,62,-82,11,-82,44,-82,37,-82,28,-82,26,-82,19,-82,30,-82,31,-82,91,-82},new int[]{-184,1538,-183,1541,-45,1542,-46,1525,-65,1545});
    states[1538] = new State(-456);
    states[1539] = new State(new int[]{10,1540});
    states[1540] = new State(-457);
    states[1541] = new State(-465);
    states[1542] = new State(new int[]{91,16},new int[]{-262,1543});
    states[1543] = new State(new int[]{10,1544});
    states[1544] = new State(-487);
    states[1545] = new State(-466);
    states[1546] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-177,1547,-176,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1547] = new State(new int[]{8,565,10,-490,110,-490},new int[]{-133,1548});
    states[1548] = new State(new int[]{10,1360,110,-852},new int[]{-213,1331,-214,1549});
    states[1549] = new State(new int[]{110,1550});
    states[1550] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514},new int[]{-267,1551,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1551] = new State(new int[]{10,1552});
    states[1552] = new State(-460);
    states[1553] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-176,1554,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1554] = new State(new int[]{8,565,5,-490,10,-490,110,-490},new int[]{-133,1555});
    states[1555] = new State(new int[]{5,1556,10,1360,110,-852},new int[]{-213,1366,-214,1562});
    states[1556] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,1557,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1557] = new State(new int[]{10,1360,110,-852},new int[]{-213,1370,-214,1558});
    states[1558] = new State(new int[]{110,1559});
    states[1559] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,1560,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-330,882,-105,659,-331,677});
    states[1560] = new State(new int[]{10,1561});
    states[1561] = new State(-458);
    states[1562] = new State(new int[]{110,1563});
    states[1563] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,1564,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-330,882,-105,659,-331,677});
    states[1564] = new State(new int[]{10,1565});
    states[1565] = new State(-459);
    states[1566] = new State(new int[]{19,1568,30,1570,44,1546,37,1553},new int[]{-227,1567,-234,1537,-231,1327,-235,1362});
    states[1567] = new State(-447);
    states[1568] = new State(new int[]{44,1546,37,1553},new int[]{-227,1569,-234,1537,-231,1327,-235,1362});
    states[1569] = new State(-450);
    states[1570] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,8,-399,110,-399,10,-399},new int[]{-178,1571,-177,1180,-176,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1571] = new State(new int[]{8,565,110,-490,10,-490},new int[]{-133,1572});
    states[1572] = new State(new int[]{110,1573,10,1169},new int[]{-213,526});
    states[1573] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514},new int[]{-267,1574,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1574] = new State(new int[]{10,1575});
    states[1575] = new State(-442);
    states[1576] = new State(new int[]{44,1546,37,1553,28,1380,26,1381},new int[]{-227,1577,-8,1578,-234,1537,-231,1327,-235,1362});
    states[1577] = new State(-448);
    states[1578] = new State(new int[]{44,1546,37,1553},new int[]{-227,1579,-234,1537,-231,1327,-235,1362});
    states[1579] = new State(-449);
    states[1580] = new State(-106);
    states[1581] = new State(-82,new int[]{-183,1582,-45,1542,-46,1525});
    states[1582] = new State(-440);
    states[1583] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,8,-399,110,-399,10,-399},new int[]{-178,1584,-177,1180,-176,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1584] = new State(new int[]{8,565,110,-490,10,-490},new int[]{-133,1585});
    states[1585] = new State(new int[]{110,1586,10,1169},new int[]{-213,1394});
    states[1586] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-514},new int[]{-267,1587,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885});
    states[1587] = new State(new int[]{10,1588});
    states[1588] = new State(-441);
    states[1589] = new State(new int[]{52,1591,91,-42,59,-42,29,-42,66,-42,50,-42,53,-42,62,-42,11,-42,44,-42,37,-42,28,-42,26,-42,19,-42,30,-42,31,-42,92,-42,103,-42},new int[]{-311,1590});
    states[1590] = new State(-44);
    states[1591] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-310,1592,-315,1602,-162,1596,-143,1601,-152,46,-156,49,-157,52,-299,57,-155,58,-300,148});
    states[1592] = new State(new int[]{10,1593,100,1594});
    states[1593] = new State(-38);
    states[1594] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-315,1595,-162,1596,-143,1601,-152,46,-156,49,-157,52,-299,57,-155,58,-300,148});
    states[1595] = new State(-46);
    states[1596] = new State(new int[]{7,1597,137,1599,10,-47,100,-47});
    states[1597] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-143,1598,-152,46,-156,49,-157,52,-299,57,-155,58,-300,148});
    states[1598] = new State(-37);
    states[1599] = new State(new int[]{144,1600});
    states[1600] = new State(-48);
    states[1601] = new State(-36);
    states[1602] = new State(-45);
    states[1603] = new State(-43);
    states[1604] = new State(new int[]{3,1606,52,-15,91,-15,59,-15,29,-15,66,-15,50,-15,53,-15,62,-15,11,-15,44,-15,37,-15,28,-15,26,-15,19,-15,30,-15,31,-15,43,-15,92,-15,103,-15},new int[]{-190,1605});
    states[1605] = new State(-17);
    states[1606] = new State(new int[]{143,1607,144,1608});
    states[1607] = new State(-18);
    states[1608] = new State(-19);
    states[1609] = new State(-16);
    states[1610] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1611,-156,49,-157,52});
    states[1611] = new State(new int[]{10,1613,8,1614},new int[]{-193,1612});
    states[1612] = new State(-28);
    states[1613] = new State(-29);
    states[1614] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-195,1615,-151,1621,-152,1620,-156,49,-157,52});
    states[1615] = new State(new int[]{9,1616,100,1618});
    states[1616] = new State(new int[]{10,1617});
    states[1617] = new State(-30);
    states[1618] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-151,1619,-152,1620,-156,49,-157,52});
    states[1619] = new State(-32);
    states[1620] = new State(-33);
    states[1621] = new State(-31);
    states[1622] = new State(-3);
    states[1623] = new State(new int[]{43,1644,52,1591,59,-41,29,-41,66,-41,50,-41,53,-41,62,-41,11,-41,44,-41,37,-41,28,-41,26,-41,19,-41,30,-41,31,-41,92,-41,103,-41,91,-41},new int[]{-167,1624,-168,1641,-309,1702,-313,1589,-311,1603});
    states[1624] = new State(new int[]{41,1638},new int[]{-166,1625});
    states[1625] = new State(new int[]{92,1628,103,1629,91,1635},new int[]{-159,1626});
    states[1626] = new State(new int[]{7,1627});
    states[1627] = new State(-49);
    states[1628] = new State(-72);
    states[1629] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,104,-514,10,-514},new int[]{-259,1630,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[1630] = new State(new int[]{92,1631,104,1632,10,19});
    states[1631] = new State(-73);
    states[1632] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514},new int[]{-259,1633,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[1633] = new State(new int[]{92,1634,10,19});
    states[1634] = new State(-74);
    states[1635] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-514,10,-514},new int[]{-259,1636,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040});
    states[1636] = new State(new int[]{92,1637,10,19});
    states[1637] = new State(-75);
    states[1638] = new State(new int[]{52,1591,59,-41,29,-41,66,-41,50,-41,53,-41,62,-41,11,-41,44,-41,37,-41,28,-41,26,-41,19,-41,30,-41,31,-41,92,-41,103,-41,91,-41},new int[]{-309,1639,-313,1589,-311,1603});
    states[1639] = new State(-82,new int[]{-45,1640,-46,1525});
    states[1640] = new State(-70);
    states[1641] = new State(new int[]{92,1628,103,1629,91,1635},new int[]{-159,1642});
    states[1642] = new State(new int[]{7,1643});
    states[1643] = new State(-50);
    states[1644] = new State(new int[]{52,1591,161,1678,29,-60,66,-60,50,-60,53,-60,62,-60,11,-60,44,-60,37,-60,41,-60},new int[]{-313,1645,-169,1697,-2,1698,-311,1603,-3,1647});
    states[1645] = new State(new int[]{161,1678,52,1591,29,-60,66,-60,50,-60,53,-60,62,-60,11,-60,44,-60,37,-60,41,-60},new int[]{-169,1646,-311,1590,-3,1647,-2,1696});
    states[1646] = new State(-56);
    states[1647] = new State(-77,new int[]{-44,1648,-42,1649});
    states[1648] = new State(-59);
    states[1649] = new State(new int[]{29,1229,66,1233,50,1445,53,1460,62,1462,11,628,41,-76,44,-234,37,-234},new int[]{-52,1650,-33,1651,-56,1670,-295,1671,-317,1674,-238,1676,-11,1679,-257,1155,-237,1694});
    states[1650] = new State(-78);
    states[1651] = new State(new int[]{143,47,161,1653,85,50,86,51,80,53,78,54,160,55,87,56,29,-87,66,-87,50,-87,53,-87,62,-87,11,-87,44,-87,37,-87,41,-87},new int[]{-31,1215,-6,1652,-32,1216,-146,1218,-152,1228,-156,49,-157,52,-2,1666});
    states[1652] = new State(-88);
    states[1653] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,120,-857,5,-857,123,-857,100,-857,110,-857},new int[]{-4,1654,-5,1665,-152,1658,-156,49,-157,52});
    states[1654] = new State(new int[]{10,1655,100,1656});
    states[1655] = new State(-62);
    states[1656] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-5,1657,-152,1658,-156,49,-157,52});
    states[1657] = new State(-65);
    states[1658] = new State(new int[]{7,1659,136,1663,10,-68,100,-68});
    states[1659] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1660,-156,49,-157,52});
    states[1660] = new State(new int[]{136,1661,10,-66,100,-66});
    states[1661] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1662,-156,49,-157,52});
    states[1662] = new State(-67);
    states[1663] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1664,-156,49,-157,52});
    states[1664] = new State(-69);
    states[1665] = new State(-64);
    states[1666] = new State(new int[]{161,1667,29,-98,66,-98,50,-98,53,-98,62,-98,11,-98,44,-98,37,-98,41,-98});
    states[1667] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-4,1668,-5,1665,-152,1658,-156,49,-157,52});
    states[1668] = new State(new int[]{10,1669,100,1656});
    states[1669] = new State(-63);
    states[1670] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,29,-89,66,-89,50,-89,53,-89,62,-89,11,-89,44,-89,37,-89,41,-89},new int[]{-31,1232,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1671] = new State(new int[]{11,628,29,-90,66,-90,50,-90,53,-90,62,-90,44,-90,37,-90,41,-90,161,-234,143,-234,85,-234,86,-234,80,-234,78,-234,160,-234,87,-234},new int[]{-53,1236,-11,1672,-257,1155});
    states[1672] = new State(new int[]{161,1653,143,47,85,50,86,51,80,53,78,54,160,55,87,56,11,628},new int[]{-6,1673,-54,1238,-257,518,-2,1666,-149,1239,-152,1437,-156,49,-157,52,-150,1442});
    states[1673] = new State(-91);
    states[1674] = new State(new int[]{143,47,161,1653,85,50,86,51,80,53,78,54,160,55,87,56,8,1452,29,-92,66,-92,50,-92,53,-92,62,-92,11,-92,44,-92,37,-92,41,-92},new int[]{-321,1448,-6,1675,-318,1449,-319,1450,-163,810,-152,809,-156,49,-157,52,-2,1666});
    states[1675] = new State(-93);
    states[1676] = new State(new int[]{161,1678,29,-94,66,-94,50,-94,53,-94,62,-94,11,-94,44,-94,37,-94,41,-94},new int[]{-6,1677,-2,1666});
    states[1677] = new State(-95);
    states[1678] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-4,1654,-5,1665,-152,1658,-156,49,-157,52});
    states[1679] = new State(new int[]{44,1686,11,628,37,1689},new int[]{-231,1680,-257,518,-235,1683});
    states[1680] = new State(new int[]{149,1681,161,-113,29,-113,66,-113,50,-113,53,-113,62,-113,11,-113,44,-113,37,-113,41,-113});
    states[1681] = new State(new int[]{10,1682});
    states[1682] = new State(-114);
    states[1683] = new State(new int[]{149,1684,161,-115,29,-115,66,-115,50,-115,53,-115,62,-115,11,-115,44,-115,37,-115,41,-115});
    states[1684] = new State(new int[]{10,1685});
    states[1685] = new State(-116);
    states[1686] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-177,1687,-176,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1687] = new State(new int[]{8,565,10,-490},new int[]{-133,1688});
    states[1688] = new State(new int[]{10,1169},new int[]{-213,1331});
    states[1689] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-176,1690,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-197,1190,-342,1192,-154,1196});
    states[1690] = new State(new int[]{8,565,5,-490,10,-490},new int[]{-133,1691});
    states[1691] = new State(new int[]{5,1692,10,1169},new int[]{-213,1366});
    states[1692] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-282,1693,-283,491,-279,361,-98,194,-111,314,-112,315,-186,316,-152,218,-156,49,-157,52,-21,349,-205,350,-171,355,-173,279,-172,283,-280,480,-307,481,-263,487,-256,488,-288,492,-285,493,-277,500,-35,501,-270,548,-135,552,-136,556,-232,560,-230,561,-229,562});
    states[1693] = new State(new int[]{10,1169},new int[]{-213,1370});
    states[1694] = new State(new int[]{161,1678,29,-96,66,-96,50,-96,53,-96,62,-96,11,-96,44,-96,37,-96,41,-96},new int[]{-6,1695,-2,1666});
    states[1695] = new State(-97);
    states[1696] = new State(new int[]{161,1667,29,-61,66,-61,50,-61,53,-61,62,-61,11,-61,44,-61,37,-61,41,-61});
    states[1697] = new State(-57);
    states[1698] = new State(new int[]{161,1667,52,1591,29,-61,66,-61,50,-61,53,-61,62,-61,11,-61,44,-61,37,-61,41,-61},new int[]{-313,1699,-311,1603});
    states[1699] = new State(new int[]{161,1678,52,1591,29,-60,66,-60,50,-60,53,-60,62,-60,11,-60,44,-60,37,-60,41,-60},new int[]{-3,1700,-311,1590,-2,1696});
    states[1700] = new State(-77,new int[]{-44,1701,-42,1649});
    states[1701] = new State(-58);
    states[1702] = new State(-82,new int[]{-45,1703,-46,1525});
    states[1703] = new State(-71);
    states[1704] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-144,1705,-152,1708,-156,49,-157,52});
    states[1705] = new State(new int[]{10,1706});
    states[1706] = new State(new int[]{3,1606,43,-14,52,-14,92,-14,103,-14,91,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-191,1707,-192,1604,-190,1609});
    states[1707] = new State(-51);
    states[1708] = new State(-55);
    states[1709] = new State(-53);
    states[1710] = new State(-54);
    states[1711] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-162,1712,-143,1601,-152,46,-156,49,-157,52,-299,57,-155,58,-300,148});
    states[1712] = new State(new int[]{10,1713,7,1597});
    states[1713] = new State(new int[]{3,1606,43,-14,52,-14,92,-14,103,-14,91,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-191,1714,-192,1604,-190,1609});
    states[1714] = new State(-52);
    states[1715] = new State(-4);
    states[1716] = new State(new int[]{50,1718,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,1717,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597});
    states[1717] = new State(new int[]{70,27,2,-7});
    states[1718] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-149,1719,-152,1720,-156,49,-157,52});
    states[1719] = new State(-8);
    states[1720] = new State(new int[]{123,1186,2,-242},new int[]{-160,1438});
    states[1721] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-328,1722,-329,1723,-152,1727,-156,49,-157,52});
    states[1722] = new State(-9);
    states[1723] = new State(new int[]{7,1724,123,187,2,-807},new int[]{-305,1726});
    states[1724] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-143,1725,-152,46,-156,49,-157,52,-299,57,-155,58,-300,148});
    states[1725] = new State(-806);
    states[1726] = new State(-808);
    states[1727] = new State(-805);
    states[1728] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,531,21,294,22,299,76,446,11,368,40,589,5,598,53,800},new int[]{-266,1729,-92,1730,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-171,336,-173,279,-172,283,-21,337,-61,338,-205,384,-118,1123,-137,391,-116,399,-152,403,-156,49,-157,52,-197,404,-264,444,-301,445,-246,450,-117,451,-62,452,-121,458,-179,459,-275,460,-102,461,-271,465,-273,466,-274,578,-247,579,-122,580,-249,588,-125,597,-9,1731,-322,1732});
    states[1729] = new State(-10);
    states[1730] = new State(new int[]{70,27,2,-11});
    states[1731] = new State(-12);
    states[1732] = new State(-13);
    states[1733] = new State(new int[]{52,1591,141,-39,143,-39,161,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,11,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,44,-39,37,-39,10,-39,2,-39},new int[]{-312,1734,-311,1738});
    states[1734] = new State(-80,new int[]{-48,1735});
    states[1735] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,44,1546,37,1553,10,-514,2,-514},new int[]{-259,1736,-227,1737,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040,-234,1537,-231,1327,-235,1362});
    states[1736] = new State(new int[]{10,19,2,-5});
    states[1737] = new State(-81);
    states[1738] = new State(-40);
    states[1739] = new State(new int[]{52,1591,141,-39,143,-39,161,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,11,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,44,-39,37,-39,10,-39,2,-39},new int[]{-312,1740,-311,1738});
    states[1740] = new State(-80,new int[]{-48,1741});
    states[1741] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,44,1546,37,1553,10,-514,2,-514},new int[]{-259,1742,-227,1737,-268,750,-267,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-197,404,-264,444,-301,445,-20,727,-171,336,-173,279,-172,283,-21,337,-246,450,-117,451,-62,728,-121,458,-218,729,-138,730,-262,735,-158,736,-39,737,-254,753,-326,758,-129,763,-327,774,-165,779,-308,780,-255,787,-128,790,-322,799,-63,868,-180,869,-179,870,-175,871,-131,876,-132,883,-130,884,-356,885,-148,1040,-234,1537,-231,1327,-235,1362});
    states[1742] = new State(new int[]{10,19,2,-6});

    rules[1] = new Rule(-369, new int[]{-1,2});
    rules[2] = new Rule(-1, new int[]{-240});
    rules[3] = new Rule(-1, new int[]{-314});
    rules[4] = new Rule(-1, new int[]{-181});
    rules[5] = new Rule(-1, new int[]{75,-312,-48,-259});
    rules[6] = new Rule(-1, new int[]{77,-312,-48,-259});
    rules[7] = new Rule(-181, new int[]{88,-92});
    rules[8] = new Rule(-181, new int[]{88,50,-149});
    rules[9] = new Rule(-181, new int[]{90,-328});
    rules[10] = new Rule(-181, new int[]{89,-266});
    rules[11] = new Rule(-266, new int[]{-92});
    rules[12] = new Rule(-266, new int[]{-9});
    rules[13] = new Rule(-266, new int[]{-322});
    rules[14] = new Rule(-191, new int[]{});
    rules[15] = new Rule(-191, new int[]{-192});
    rules[16] = new Rule(-192, new int[]{-190});
    rules[17] = new Rule(-192, new int[]{-192,-190});
    rules[18] = new Rule(-190, new int[]{3,143});
    rules[19] = new Rule(-190, new int[]{3,144});
    rules[20] = new Rule(-240, new int[]{-241,-191,-309,-22,-194});
    rules[21] = new Rule(-194, new int[]{7});
    rules[22] = new Rule(-194, new int[]{10});
    rules[23] = new Rule(-194, new int[]{5});
    rules[24] = new Rule(-194, new int[]{100});
    rules[25] = new Rule(-194, new int[]{6});
    rules[26] = new Rule(-194, new int[]{});
    rules[27] = new Rule(-241, new int[]{});
    rules[28] = new Rule(-241, new int[]{61,-152,-193});
    rules[29] = new Rule(-193, new int[]{10});
    rules[30] = new Rule(-193, new int[]{8,-195,9,10});
    rules[31] = new Rule(-195, new int[]{-151});
    rules[32] = new Rule(-195, new int[]{-195,100,-151});
    rules[33] = new Rule(-151, new int[]{-152});
    rules[34] = new Rule(-22, new int[]{-41,-262});
    rules[35] = new Rule(-41, new int[]{-45});
    rules[36] = new Rule(-162, new int[]{-143});
    rules[37] = new Rule(-162, new int[]{-162,7,-143});
    rules[38] = new Rule(-311, new int[]{52,-310,10});
    rules[39] = new Rule(-312, new int[]{});
    rules[40] = new Rule(-312, new int[]{-311});
    rules[41] = new Rule(-309, new int[]{});
    rules[42] = new Rule(-309, new int[]{-313});
    rules[43] = new Rule(-313, new int[]{-311});
    rules[44] = new Rule(-313, new int[]{-313,-311});
    rules[45] = new Rule(-310, new int[]{-315});
    rules[46] = new Rule(-310, new int[]{-310,100,-315});
    rules[47] = new Rule(-315, new int[]{-162});
    rules[48] = new Rule(-315, new int[]{-162,137,144});
    rules[49] = new Rule(-314, new int[]{-316,-167,-166,-159,7});
    rules[50] = new Rule(-314, new int[]{-316,-168,-159,7});
    rules[51] = new Rule(-316, new int[]{-7,-144,10,-191});
    rules[52] = new Rule(-316, new int[]{109,-162,10,-191});
    rules[53] = new Rule(-7, new int[]{105});
    rules[54] = new Rule(-7, new int[]{106});
    rules[55] = new Rule(-144, new int[]{-152});
    rules[56] = new Rule(-167, new int[]{43,-313,-169});
    rules[57] = new Rule(-167, new int[]{43,-169});
    rules[58] = new Rule(-167, new int[]{43,-2,-313,-3,-44});
    rules[59] = new Rule(-169, new int[]{-3,-44});
    rules[60] = new Rule(-3, new int[]{});
    rules[61] = new Rule(-3, new int[]{-2});
    rules[62] = new Rule(-2, new int[]{161,-4,10});
    rules[63] = new Rule(-2, new int[]{-2,161,-4,10});
    rules[64] = new Rule(-4, new int[]{-5});
    rules[65] = new Rule(-4, new int[]{-4,100,-5});
    rules[66] = new Rule(-5, new int[]{-152,7,-152});
    rules[67] = new Rule(-5, new int[]{-152,7,-152,136,-152});
    rules[68] = new Rule(-5, new int[]{-152});
    rules[69] = new Rule(-5, new int[]{-152,136,-152});
    rules[70] = new Rule(-166, new int[]{41,-309,-45});
    rules[71] = new Rule(-168, new int[]{-309,-45});
    rules[72] = new Rule(-159, new int[]{92});
    rules[73] = new Rule(-159, new int[]{103,-259,92});
    rules[74] = new Rule(-159, new int[]{103,-259,104,-259,92});
    rules[75] = new Rule(-159, new int[]{91,-259,92});
    rules[76] = new Rule(-44, new int[]{-42});
    rules[77] = new Rule(-42, new int[]{});
    rules[78] = new Rule(-42, new int[]{-42,-52});
    rules[79] = new Rule(-45, new int[]{-46});
    rules[80] = new Rule(-48, new int[]{});
    rules[81] = new Rule(-48, new int[]{-48,-227});
    rules[82] = new Rule(-46, new int[]{});
    rules[83] = new Rule(-46, new int[]{-46,-51});
    rules[84] = new Rule(-47, new int[]{-43});
    rules[85] = new Rule(-43, new int[]{});
    rules[86] = new Rule(-43, new int[]{-43,-50});
    rules[87] = new Rule(-52, new int[]{-33});
    rules[88] = new Rule(-52, new int[]{-33,-6});
    rules[89] = new Rule(-52, new int[]{-56});
    rules[90] = new Rule(-52, new int[]{-295});
    rules[91] = new Rule(-52, new int[]{-295,-11,-6});
    rules[92] = new Rule(-52, new int[]{-317});
    rules[93] = new Rule(-52, new int[]{-317,-6});
    rules[94] = new Rule(-52, new int[]{-238});
    rules[95] = new Rule(-52, new int[]{-238,-6});
    rules[96] = new Rule(-52, new int[]{-237});
    rules[97] = new Rule(-52, new int[]{-237,-6});
    rules[98] = new Rule(-6, new int[]{-2});
    rules[99] = new Rule(-51, new int[]{-174});
    rules[100] = new Rule(-51, new int[]{-33});
    rules[101] = new Rule(-51, new int[]{-56});
    rules[102] = new Rule(-51, new int[]{-295});
    rules[103] = new Rule(-51, new int[]{-317});
    rules[104] = new Rule(-51, new int[]{-226});
    rules[105] = new Rule(-219, new int[]{-220});
    rules[106] = new Rule(-219, new int[]{-223});
    rules[107] = new Rule(-226, new int[]{-11,-219});
    rules[108] = new Rule(-50, new int[]{-174});
    rules[109] = new Rule(-50, new int[]{-33});
    rules[110] = new Rule(-50, new int[]{-56});
    rules[111] = new Rule(-50, new int[]{-295});
    rules[112] = new Rule(-50, new int[]{-317});
    rules[113] = new Rule(-238, new int[]{-11,-231});
    rules[114] = new Rule(-238, new int[]{-11,-231,149,10});
    rules[115] = new Rule(-237, new int[]{-11,-235});
    rules[116] = new Rule(-237, new int[]{-11,-235,149,10});
    rules[117] = new Rule(-174, new int[]{59,-161,10});
    rules[118] = new Rule(-161, new int[]{-148});
    rules[119] = new Rule(-161, new int[]{-161,100,-148});
    rules[120] = new Rule(-148, new int[]{155});
    rules[121] = new Rule(-148, new int[]{-152});
    rules[122] = new Rule(-33, new int[]{29,-31});
    rules[123] = new Rule(-33, new int[]{-33,-31});
    rules[124] = new Rule(-56, new int[]{66,-31});
    rules[125] = new Rule(-56, new int[]{-56,-31});
    rules[126] = new Rule(-295, new int[]{50,-53});
    rules[127] = new Rule(-295, new int[]{-295,-53});
    rules[128] = new Rule(-321, new int[]{-318});
    rules[129] = new Rule(-321, new int[]{8,-152,100,-163,9,110,-106,10});
    rules[130] = new Rule(-317, new int[]{53,-321});
    rules[131] = new Rule(-317, new int[]{62,-321});
    rules[132] = new Rule(-317, new int[]{-317,-321});
    rules[133] = new Rule(-31, new int[]{-32,10});
    rules[134] = new Rule(-32, new int[]{-146,120,-114});
    rules[135] = new Rule(-32, new int[]{-146,5,-283,120,-89});
    rules[136] = new Rule(-114, new int[]{-95});
    rules[137] = new Rule(-114, new int[]{-100});
    rules[138] = new Rule(-146, new int[]{-152});
    rules[139] = new Rule(-96, new int[]{-86});
    rules[140] = new Rule(-96, new int[]{-96,-198,-86});
    rules[141] = new Rule(-95, new int[]{-96});
    rules[142] = new Rule(-95, new int[]{-248});
    rules[143] = new Rule(-95, new int[]{-95,16,-96});
    rules[144] = new Rule(-248, new int[]{-95,13,-95,5,-95});
    rules[145] = new Rule(-198, new int[]{120});
    rules[146] = new Rule(-198, new int[]{125});
    rules[147] = new Rule(-198, new int[]{123});
    rules[148] = new Rule(-198, new int[]{121});
    rules[149] = new Rule(-198, new int[]{124});
    rules[150] = new Rule(-198, new int[]{122});
    rules[151] = new Rule(-198, new int[]{137});
    rules[152] = new Rule(-198, new int[]{135,137});
    rules[153] = new Rule(-86, new int[]{-18});
    rules[154] = new Rule(-86, new int[]{-86,-199,-18});
    rules[155] = new Rule(-199, new int[]{116});
    rules[156] = new Rule(-199, new int[]{115});
    rules[157] = new Rule(-199, new int[]{128});
    rules[158] = new Rule(-199, new int[]{129});
    rules[159] = new Rule(-272, new int[]{-18,-207,-290});
    rules[160] = new Rule(-276, new int[]{-16,119,-15});
    rules[161] = new Rule(-276, new int[]{-16,119,-276});
    rules[162] = new Rule(-276, new int[]{-205,-276});
    rules[163] = new Rule(-18, new int[]{-15});
    rules[164] = new Rule(-18, new int[]{-272});
    rules[165] = new Rule(-18, new int[]{-276});
    rules[166] = new Rule(-18, new int[]{-18,-201,-15});
    rules[167] = new Rule(-18, new int[]{-18,-201,-276});
    rules[168] = new Rule(-201, new int[]{118});
    rules[169] = new Rule(-201, new int[]{117});
    rules[170] = new Rule(-201, new int[]{131});
    rules[171] = new Rule(-201, new int[]{132});
    rules[172] = new Rule(-201, new int[]{133});
    rules[173] = new Rule(-201, new int[]{134});
    rules[174] = new Rule(-201, new int[]{130});
    rules[175] = new Rule(-16, new int[]{-19});
    rules[176] = new Rule(-16, new int[]{8,-95,9});
    rules[177] = new Rule(-15, new int[]{-19});
    rules[178] = new Rule(-15, new int[]{-245});
    rules[179] = new Rule(-15, new int[]{56});
    rules[180] = new Rule(-15, new int[]{141,-15});
    rules[181] = new Rule(-15, new int[]{8,-95,9});
    rules[182] = new Rule(-15, new int[]{135,-15});
    rules[183] = new Rule(-15, new int[]{-205,-15});
    rules[184] = new Rule(-15, new int[]{-179});
    rules[185] = new Rule(-15, new int[]{-61});
    rules[186] = new Rule(-246, new int[]{11,-73,12});
    rules[187] = new Rule(-245, new int[]{-246});
    rules[188] = new Rule(-245, new int[]{76,-73,76});
    rules[189] = new Rule(-205, new int[]{116});
    rules[190] = new Rule(-205, new int[]{115});
    rules[191] = new Rule(-19, new int[]{-152});
    rules[192] = new Rule(-19, new int[]{-171});
    rules[193] = new Rule(-19, new int[]{-21});
    rules[194] = new Rule(-19, new int[]{42,-152});
    rules[195] = new Rule(-19, new int[]{-264});
    rules[196] = new Rule(-19, new int[]{-301});
    rules[197] = new Rule(-19, new int[]{-19,-17});
    rules[198] = new Rule(-19, new int[]{-19,4,-305});
    rules[199] = new Rule(-19, new int[]{-19,11,-126,12});
    rules[200] = new Rule(-17, new int[]{7,-143});
    rules[201] = new Rule(-17, new int[]{142});
    rules[202] = new Rule(-17, new int[]{8,-81,9});
    rules[203] = new Rule(-17, new int[]{11,-80,12});
    rules[204] = new Rule(-81, new int[]{-76});
    rules[205] = new Rule(-81, new int[]{});
    rules[206] = new Rule(-80, new int[]{-78});
    rules[207] = new Rule(-80, new int[]{});
    rules[208] = new Rule(-78, new int[]{-99});
    rules[209] = new Rule(-78, new int[]{-78,100,-99});
    rules[210] = new Rule(-99, new int[]{-95});
    rules[211] = new Rule(-99, new int[]{-95,6,-95});
    rules[212] = new Rule(-21, new int[]{155});
    rules[213] = new Rule(-21, new int[]{158});
    rules[214] = new Rule(-21, new int[]{157});
    rules[215] = new Rule(-21, new int[]{156});
    rules[216] = new Rule(-89, new int[]{-95});
    rules[217] = new Rule(-89, new int[]{-100});
    rules[218] = new Rule(-89, new int[]{-250});
    rules[219] = new Rule(-100, new int[]{8,-70,9});
    rules[220] = new Rule(-70, new int[]{});
    rules[221] = new Rule(-70, new int[]{-69});
    rules[222] = new Rule(-69, new int[]{-90});
    rules[223] = new Rule(-69, new int[]{-69,100,-90});
    rules[224] = new Rule(-250, new int[]{8,-252,9});
    rules[225] = new Rule(-252, new int[]{-251});
    rules[226] = new Rule(-252, new int[]{-251,10});
    rules[227] = new Rule(-251, new int[]{-253});
    rules[228] = new Rule(-251, new int[]{-251,10,-253});
    rules[229] = new Rule(-253, new int[]{-141,5,-89});
    rules[230] = new Rule(-141, new int[]{-152});
    rules[231] = new Rule(-53, new int[]{-11,-54});
    rules[232] = new Rule(-11, new int[]{-257});
    rules[233] = new Rule(-11, new int[]{-11,-257});
    rules[234] = new Rule(-11, new int[]{});
    rules[235] = new Rule(-257, new int[]{11,-258,12});
    rules[236] = new Rule(-258, new int[]{-13});
    rules[237] = new Rule(-258, new int[]{-258,100,-13});
    rules[238] = new Rule(-13, new int[]{-14});
    rules[239] = new Rule(-13, new int[]{-152,5,-14});
    rules[240] = new Rule(-54, new int[]{-149,120,-293,10});
    rules[241] = new Rule(-54, new int[]{-150,-293,10});
    rules[242] = new Rule(-149, new int[]{-152});
    rules[243] = new Rule(-149, new int[]{-152,-160});
    rules[244] = new Rule(-150, new int[]{-152,123,-163,122});
    rules[245] = new Rule(-293, new int[]{-283});
    rules[246] = new Rule(-293, new int[]{-34});
    rules[247] = new Rule(-280, new int[]{-279,13});
    rules[248] = new Rule(-280, new int[]{-307,13});
    rules[249] = new Rule(-283, new int[]{-279});
    rules[250] = new Rule(-283, new int[]{-280});
    rules[251] = new Rule(-283, new int[]{-263});
    rules[252] = new Rule(-283, new int[]{-256});
    rules[253] = new Rule(-283, new int[]{-288});
    rules[254] = new Rule(-283, new int[]{-232});
    rules[255] = new Rule(-283, new int[]{-307});
    rules[256] = new Rule(-307, new int[]{-186,-305});
    rules[257] = new Rule(-305, new int[]{123,-303,121});
    rules[258] = new Rule(-306, new int[]{125});
    rules[259] = new Rule(-306, new int[]{123,-304,121});
    rules[260] = new Rule(-303, new int[]{-286});
    rules[261] = new Rule(-303, new int[]{-303,100,-286});
    rules[262] = new Rule(-304, new int[]{-287});
    rules[263] = new Rule(-304, new int[]{-304,100,-287});
    rules[264] = new Rule(-287, new int[]{});
    rules[265] = new Rule(-286, new int[]{-279});
    rules[266] = new Rule(-286, new int[]{-279,13});
    rules[267] = new Rule(-286, new int[]{-288});
    rules[268] = new Rule(-286, new int[]{-232});
    rules[269] = new Rule(-286, new int[]{-307});
    rules[270] = new Rule(-279, new int[]{-98});
    rules[271] = new Rule(-279, new int[]{-98,6,-98});
    rules[272] = new Rule(-279, new int[]{8,-85,9});
    rules[273] = new Rule(-98, new int[]{-111});
    rules[274] = new Rule(-98, new int[]{-98,-199,-111});
    rules[275] = new Rule(-111, new int[]{-112});
    rules[276] = new Rule(-111, new int[]{-111,-201,-112});
    rules[277] = new Rule(-112, new int[]{-186});
    rules[278] = new Rule(-112, new int[]{-21});
    rules[279] = new Rule(-112, new int[]{-205,-112});
    rules[280] = new Rule(-112, new int[]{-171});
    rules[281] = new Rule(-112, new int[]{-112,8,-80,9});
    rules[282] = new Rule(-186, new int[]{-152});
    rules[283] = new Rule(-186, new int[]{-186,7,-143});
    rules[284] = new Rule(-85, new int[]{-83});
    rules[285] = new Rule(-85, new int[]{-85,100,-83});
    rules[286] = new Rule(-83, new int[]{-283});
    rules[287] = new Rule(-83, new int[]{-283,120,-92});
    rules[288] = new Rule(-256, new int[]{142,-282});
    rules[289] = new Rule(-288, new int[]{-285});
    rules[290] = new Rule(-288, new int[]{-35});
    rules[291] = new Rule(-288, new int[]{-270});
    rules[292] = new Rule(-288, new int[]{-135});
    rules[293] = new Rule(-288, new int[]{-136});
    rules[294] = new Rule(-136, new int[]{73,58,-283});
    rules[295] = new Rule(-285, new int[]{24,11,-170,12,58,-283});
    rules[296] = new Rule(-285, new int[]{-277});
    rules[297] = new Rule(-277, new int[]{24,58,-283});
    rules[298] = new Rule(-170, new int[]{-278});
    rules[299] = new Rule(-170, new int[]{-170,100,-278});
    rules[300] = new Rule(-278, new int[]{-279});
    rules[301] = new Rule(-278, new int[]{});
    rules[302] = new Rule(-270, new int[]{49,58,-283});
    rules[303] = new Rule(-135, new int[]{34,58,-283});
    rules[304] = new Rule(-135, new int[]{34});
    rules[305] = new Rule(-263, new int[]{143,11,-95,12});
    rules[306] = new Rule(-232, new int[]{-230});
    rules[307] = new Rule(-230, new int[]{-229});
    rules[308] = new Rule(-229, new int[]{44,-133});
    rules[309] = new Rule(-229, new int[]{37,-133,5,-282});
    rules[310] = new Rule(-229, new int[]{-186,127,-286});
    rules[311] = new Rule(-229, new int[]{-307,127,-286});
    rules[312] = new Rule(-229, new int[]{8,9,127,-286});
    rules[313] = new Rule(-229, new int[]{8,-85,9,127,-286});
    rules[314] = new Rule(-229, new int[]{-186,127,8,9});
    rules[315] = new Rule(-229, new int[]{-307,127,8,9});
    rules[316] = new Rule(-229, new int[]{8,9,127,8,9});
    rules[317] = new Rule(-229, new int[]{8,-85,9,127,8,9});
    rules[318] = new Rule(-34, new int[]{-25,-297,-189,-325,-30});
    rules[319] = new Rule(-35, new int[]{48,-189,-325,-29,92});
    rules[320] = new Rule(-24, new int[]{68});
    rules[321] = new Rule(-24, new int[]{69});
    rules[322] = new Rule(-24, new int[]{148});
    rules[323] = new Rule(-24, new int[]{27});
    rules[324] = new Rule(-24, new int[]{28});
    rules[325] = new Rule(-25, new int[]{});
    rules[326] = new Rule(-25, new int[]{-26});
    rules[327] = new Rule(-26, new int[]{-24});
    rules[328] = new Rule(-26, new int[]{-26,-24});
    rules[329] = new Rule(-297, new int[]{26});
    rules[330] = new Rule(-297, new int[]{43});
    rules[331] = new Rule(-297, new int[]{64});
    rules[332] = new Rule(-297, new int[]{64,26});
    rules[333] = new Rule(-297, new int[]{64,48});
    rules[334] = new Rule(-297, new int[]{64,43});
    rules[335] = new Rule(-30, new int[]{});
    rules[336] = new Rule(-30, new int[]{-29,92});
    rules[337] = new Rule(-189, new int[]{});
    rules[338] = new Rule(-189, new int[]{8,-188,9});
    rules[339] = new Rule(-188, new int[]{-187});
    rules[340] = new Rule(-188, new int[]{-188,100,-187});
    rules[341] = new Rule(-187, new int[]{-186});
    rules[342] = new Rule(-187, new int[]{-307});
    rules[343] = new Rule(-160, new int[]{123,-163,121});
    rules[344] = new Rule(-325, new int[]{});
    rules[345] = new Rule(-325, new int[]{-324});
    rules[346] = new Rule(-324, new int[]{-323});
    rules[347] = new Rule(-324, new int[]{-324,-323});
    rules[348] = new Rule(-323, new int[]{23,-163,5,-294,10});
    rules[349] = new Rule(-294, new int[]{-292});
    rules[350] = new Rule(-294, new int[]{-294,100,-292});
    rules[351] = new Rule(-292, new int[]{-283});
    rules[352] = new Rule(-292, new int[]{26});
    rules[353] = new Rule(-292, new int[]{48});
    rules[354] = new Rule(-292, new int[]{30});
    rules[355] = new Rule(-29, new int[]{-36});
    rules[356] = new Rule(-29, new int[]{-29,-12,-36});
    rules[357] = new Rule(-12, new int[]{84});
    rules[358] = new Rule(-12, new int[]{83});
    rules[359] = new Rule(-12, new int[]{82});
    rules[360] = new Rule(-12, new int[]{81});
    rules[361] = new Rule(-36, new int[]{});
    rules[362] = new Rule(-36, new int[]{-38,-196});
    rules[363] = new Rule(-36, new int[]{-37});
    rules[364] = new Rule(-36, new int[]{-38,10,-37});
    rules[365] = new Rule(-163, new int[]{-152});
    rules[366] = new Rule(-163, new int[]{-163,100,-152});
    rules[367] = new Rule(-196, new int[]{});
    rules[368] = new Rule(-196, new int[]{10});
    rules[369] = new Rule(-38, new int[]{-49});
    rules[370] = new Rule(-38, new int[]{-38,10,-49});
    rules[371] = new Rule(-49, new int[]{-11,-55});
    rules[372] = new Rule(-37, new int[]{-58});
    rules[373] = new Rule(-37, new int[]{-37,-58});
    rules[374] = new Rule(-58, new int[]{-57});
    rules[375] = new Rule(-58, new int[]{-59});
    rules[376] = new Rule(-55, new int[]{29,-32});
    rules[377] = new Rule(-55, new int[]{-320});
    rules[378] = new Rule(-55, new int[]{-8,-320});
    rules[379] = new Rule(-8, new int[]{28});
    rules[380] = new Rule(-8, new int[]{26});
    rules[381] = new Rule(-320, new int[]{-319});
    rules[382] = new Rule(-320, new int[]{62,-163,5,-283});
    rules[383] = new Rule(-57, new int[]{-11,-228});
    rules[384] = new Rule(-57, new int[]{-11,-225});
    rules[385] = new Rule(-225, new int[]{-221});
    rules[386] = new Rule(-225, new int[]{-224});
    rules[387] = new Rule(-228, new int[]{-8,-236});
    rules[388] = new Rule(-228, new int[]{-236});
    rules[389] = new Rule(-228, new int[]{19,-8,-236});
    rules[390] = new Rule(-228, new int[]{19,-236});
    rules[391] = new Rule(-228, new int[]{-233});
    rules[392] = new Rule(-236, new int[]{-234});
    rules[393] = new Rule(-234, new int[]{-231});
    rules[394] = new Rule(-234, new int[]{-235});
    rules[395] = new Rule(-233, new int[]{30,-178,-133,-213});
    rules[396] = new Rule(-233, new int[]{-8,30,-178,-133,-213});
    rules[397] = new Rule(-233, new int[]{31,-178,-133,-213});
    rules[398] = new Rule(-178, new int[]{-177});
    rules[399] = new Rule(-178, new int[]{});
    rules[400] = new Rule(-59, new int[]{-11,-265});
    rules[401] = new Rule(-265, new int[]{46,-176,-239,-208,10,-211});
    rules[402] = new Rule(-265, new int[]{46,-176,-239,-208,10,-216,10,-211});
    rules[403] = new Rule(-265, new int[]{-8,46,-176,-239,-208,10,-211});
    rules[404] = new Rule(-265, new int[]{-8,46,-176,-239,-208,10,-216,10,-211});
    rules[405] = new Rule(-265, new int[]{27,46,-176,-239,-217,10});
    rules[406] = new Rule(-265, new int[]{-8,27,46,-176,-239,-217,10});
    rules[407] = new Rule(-217, new int[]{110,-92});
    rules[408] = new Rule(-217, new int[]{});
    rules[409] = new Rule(-211, new int[]{});
    rules[410] = new Rule(-211, new int[]{63,10});
    rules[411] = new Rule(-239, new int[]{-244,5,-282});
    rules[412] = new Rule(-244, new int[]{});
    rules[413] = new Rule(-244, new int[]{11,-243,12});
    rules[414] = new Rule(-243, new int[]{-242});
    rules[415] = new Rule(-243, new int[]{-243,10,-242});
    rules[416] = new Rule(-242, new int[]{-163,5,-282});
    rules[417] = new Rule(-119, new int[]{-93});
    rules[418] = new Rule(-119, new int[]{});
    rules[419] = new Rule(-208, new int[]{});
    rules[420] = new Rule(-208, new int[]{85,-119,-209});
    rules[421] = new Rule(-208, new int[]{86,-267,-210});
    rules[422] = new Rule(-209, new int[]{});
    rules[423] = new Rule(-209, new int[]{86,-267});
    rules[424] = new Rule(-210, new int[]{});
    rules[425] = new Rule(-210, new int[]{85,-119});
    rules[426] = new Rule(-318, new int[]{-319,10});
    rules[427] = new Rule(-346, new int[]{110});
    rules[428] = new Rule(-346, new int[]{120});
    rules[429] = new Rule(-319, new int[]{-163,5,-283});
    rules[430] = new Rule(-319, new int[]{-163,110,-93});
    rules[431] = new Rule(-319, new int[]{-163,5,-283,-346,-91});
    rules[432] = new Rule(-91, new int[]{-90});
    rules[433] = new Rule(-91, new int[]{-86,6,-18});
    rules[434] = new Rule(-91, new int[]{-331});
    rules[435] = new Rule(-91, new int[]{-152,127,-336});
    rules[436] = new Rule(-91, new int[]{8,9,-332,127,-336});
    rules[437] = new Rule(-91, new int[]{8,-70,9,127,-336});
    rules[438] = new Rule(-91, new int[]{-249});
    rules[439] = new Rule(-90, new int[]{-89});
    rules[440] = new Rule(-223, new int[]{-233,-183});
    rules[441] = new Rule(-223, new int[]{30,-178,-133,110,-267,10});
    rules[442] = new Rule(-223, new int[]{-8,30,-178,-133,110,-267,10});
    rules[443] = new Rule(-224, new int[]{-233,-182});
    rules[444] = new Rule(-224, new int[]{30,-178,-133,110,-267,10});
    rules[445] = new Rule(-224, new int[]{-8,30,-178,-133,110,-267,10});
    rules[446] = new Rule(-220, new int[]{-227});
    rules[447] = new Rule(-220, new int[]{-8,-227});
    rules[448] = new Rule(-220, new int[]{19,-227});
    rules[449] = new Rule(-220, new int[]{19,-8,-227});
    rules[450] = new Rule(-220, new int[]{-8,19,-227});
    rules[451] = new Rule(-221, new int[]{-222});
    rules[452] = new Rule(-221, new int[]{19,-222});
    rules[453] = new Rule(-221, new int[]{-8,-222});
    rules[454] = new Rule(-221, new int[]{19,-8,-222});
    rules[455] = new Rule(-221, new int[]{-8,19,-222});
    rules[456] = new Rule(-227, new int[]{-234,-184});
    rules[457] = new Rule(-227, new int[]{-234,149,10});
    rules[458] = new Rule(-227, new int[]{37,-176,-133,5,-282,-214,110,-108,10});
    rules[459] = new Rule(-227, new int[]{37,-176,-133,-214,110,-108,10});
    rules[460] = new Rule(-227, new int[]{44,-177,-133,-214,110,-267,10});
    rules[461] = new Rule(-222, new int[]{-234,-182});
    rules[462] = new Rule(-222, new int[]{37,-176,-133,5,-282,-214,110,-108,10});
    rules[463] = new Rule(-222, new int[]{37,-176,-133,-214,110,-108,10});
    rules[464] = new Rule(-222, new int[]{44,-177,-133,-214,110,-267,10});
    rules[465] = new Rule(-184, new int[]{-183});
    rules[466] = new Rule(-184, new int[]{-65});
    rules[467] = new Rule(-177, new int[]{-176});
    rules[468] = new Rule(-176, new int[]{-147});
    rules[469] = new Rule(-176, new int[]{-342,7,-147});
    rules[470] = new Rule(-154, new int[]{-142});
    rules[471] = new Rule(-342, new int[]{-154});
    rules[472] = new Rule(-342, new int[]{-342,7,-154});
    rules[473] = new Rule(-147, new int[]{-142});
    rules[474] = new Rule(-147, new int[]{-197});
    rules[475] = new Rule(-147, new int[]{-197,-160});
    rules[476] = new Rule(-142, new int[]{-139});
    rules[477] = new Rule(-142, new int[]{-139,-160});
    rules[478] = new Rule(-139, new int[]{-152});
    rules[479] = new Rule(-231, new int[]{44,-177,-133,-213,-325});
    rules[480] = new Rule(-235, new int[]{37,-176,-133,-213,-325});
    rules[481] = new Rule(-235, new int[]{37,-176,-133,5,-282,-213,-325});
    rules[482] = new Rule(-65, new int[]{107,-113,80,-113,10});
    rules[483] = new Rule(-65, new int[]{107,-113,10});
    rules[484] = new Rule(-65, new int[]{107,10});
    rules[485] = new Rule(-113, new int[]{-152});
    rules[486] = new Rule(-113, new int[]{-171});
    rules[487] = new Rule(-183, new int[]{-45,-262,10});
    rules[488] = new Rule(-182, new int[]{-47,-262,10});
    rules[489] = new Rule(-182, new int[]{-65});
    rules[490] = new Rule(-133, new int[]{});
    rules[491] = new Rule(-133, new int[]{8,9});
    rules[492] = new Rule(-133, new int[]{8,-134,9});
    rules[493] = new Rule(-134, new int[]{-60});
    rules[494] = new Rule(-134, new int[]{-134,10,-60});
    rules[495] = new Rule(-60, new int[]{-11,-302});
    rules[496] = new Rule(-302, new int[]{-164,5,-282});
    rules[497] = new Rule(-302, new int[]{53,-164,5,-282});
    rules[498] = new Rule(-302, new int[]{29,-164,5,-282});
    rules[499] = new Rule(-302, new int[]{108,-164,5,-282});
    rules[500] = new Rule(-302, new int[]{-164,5,-282,110,-92});
    rules[501] = new Rule(-302, new int[]{53,-164,5,-282,110,-92});
    rules[502] = new Rule(-302, new int[]{29,-164,5,-282,110,-92});
    rules[503] = new Rule(-164, new int[]{-140});
    rules[504] = new Rule(-164, new int[]{-164,100,-140});
    rules[505] = new Rule(-140, new int[]{-152});
    rules[506] = new Rule(-282, new int[]{-283});
    rules[507] = new Rule(-284, new int[]{-279});
    rules[508] = new Rule(-284, new int[]{-263});
    rules[509] = new Rule(-284, new int[]{-256});
    rules[510] = new Rule(-284, new int[]{-288});
    rules[511] = new Rule(-284, new int[]{-307});
    rules[512] = new Rule(-268, new int[]{-267});
    rules[513] = new Rule(-268, new int[]{-148,5,-268});
    rules[514] = new Rule(-267, new int[]{});
    rules[515] = new Rule(-267, new int[]{-9});
    rules[516] = new Rule(-267, new int[]{-218});
    rules[517] = new Rule(-267, new int[]{-138});
    rules[518] = new Rule(-267, new int[]{-262});
    rules[519] = new Rule(-267, new int[]{-158});
    rules[520] = new Rule(-267, new int[]{-39});
    rules[521] = new Rule(-267, new int[]{-254});
    rules[522] = new Rule(-267, new int[]{-326});
    rules[523] = new Rule(-267, new int[]{-129});
    rules[524] = new Rule(-267, new int[]{-327});
    rules[525] = new Rule(-267, new int[]{-165});
    rules[526] = new Rule(-267, new int[]{-308});
    rules[527] = new Rule(-267, new int[]{-255});
    rules[528] = new Rule(-267, new int[]{-128});
    rules[529] = new Rule(-267, new int[]{-322});
    rules[530] = new Rule(-267, new int[]{-63});
    rules[531] = new Rule(-267, new int[]{-175});
    rules[532] = new Rule(-267, new int[]{-131});
    rules[533] = new Rule(-267, new int[]{-132});
    rules[534] = new Rule(-267, new int[]{-130});
    rules[535] = new Rule(-267, new int[]{-356});
    rules[536] = new Rule(-130, new int[]{72,-106,99,-267});
    rules[537] = new Rule(-131, new int[]{74,-108});
    rules[538] = new Rule(-132, new int[]{74,73,-108});
    rules[539] = new Rule(-322, new int[]{53,-319});
    rules[540] = new Rule(-322, new int[]{8,53,-152,100,-345,9,110,-92});
    rules[541] = new Rule(-322, new int[]{53,8,-152,100,-163,9,110,-92});
    rules[542] = new Rule(-9, new int[]{-118,-200,-93});
    rules[543] = new Rule(-9, new int[]{8,-116,100,-344,9,-200,-92});
    rules[544] = new Rule(-344, new int[]{-116});
    rules[545] = new Rule(-344, new int[]{-344,100,-116});
    rules[546] = new Rule(-345, new int[]{53,-152});
    rules[547] = new Rule(-345, new int[]{-345,100,53,-152});
    rules[548] = new Rule(-218, new int[]{-118});
    rules[549] = new Rule(-138, new int[]{57,-148});
    rules[550] = new Rule(-262, new int[]{91,-259,92});
    rules[551] = new Rule(-259, new int[]{-268});
    rules[552] = new Rule(-259, new int[]{-259,10,-268});
    rules[553] = new Rule(-158, new int[]{40,-106,51,-267});
    rules[554] = new Rule(-158, new int[]{40,-106,51,-267,32,-267});
    rules[555] = new Rule(-356, new int[]{38,-106,55,-358,-260,92});
    rules[556] = new Rule(-356, new int[]{38,-106,55,-358,10,-260,92});
    rules[557] = new Rule(-358, new int[]{-357});
    rules[558] = new Rule(-358, new int[]{-358,10,-357});
    rules[559] = new Rule(-357, new int[]{-350,39,-106,5,-267});
    rules[560] = new Rule(-357, new int[]{-349,5,-267});
    rules[561] = new Rule(-357, new int[]{-351,5,-267});
    rules[562] = new Rule(-357, new int[]{-352,39,-106,5,-267});
    rules[563] = new Rule(-357, new int[]{-352,5,-267});
    rules[564] = new Rule(-39, new int[]{25,-106,58,-40,-260,92});
    rules[565] = new Rule(-39, new int[]{25,-106,58,-40,10,-260,92});
    rules[566] = new Rule(-39, new int[]{25,-106,58,-260,92});
    rules[567] = new Rule(-40, new int[]{-269});
    rules[568] = new Rule(-40, new int[]{-40,10,-269});
    rules[569] = new Rule(-269, new int[]{-79,5,-267});
    rules[570] = new Rule(-79, new int[]{-115});
    rules[571] = new Rule(-79, new int[]{-79,100,-115});
    rules[572] = new Rule(-115, new int[]{-99});
    rules[573] = new Rule(-260, new int[]{});
    rules[574] = new Rule(-260, new int[]{32,-259});
    rules[575] = new Rule(-254, new int[]{97,-259,98,-92});
    rules[576] = new Rule(-326, new int[]{54,-106,-298,-267});
    rules[577] = new Rule(-298, new int[]{99});
    rules[578] = new Rule(-298, new int[]{});
    rules[579] = new Rule(-175, new int[]{60,-106,99,-267});
    rules[580] = new Rule(-368, new int[]{87,143});
    rules[581] = new Rule(-368, new int[]{});
    rules[582] = new Rule(-281, new int[]{5,-283});
    rules[583] = new Rule(-281, new int[]{});
    rules[584] = new Rule(-23, new int[]{53});
    rules[585] = new Rule(-23, new int[]{});
    rules[586] = new Rule(-124, new int[]{70});
    rules[587] = new Rule(-124, new int[]{71});
    rules[588] = new Rule(-128, new int[]{36,-152,-281,137,-106,-368,99,-267});
    rules[589] = new Rule(-128, new int[]{36,53,-152,-281,137,-106,-368,99,-267});
    rules[590] = new Rule(-128, new int[]{36,53,8,-163,9,137,-106,-368,99,-267});
    rules[591] = new Rule(-129, new int[]{35,-23,-152,-281,110,-106,-124,-106,-298,-267});
    rules[592] = new Rule(-129, new int[]{35,-23,-152,-281,110,-106,-124,-106,160,-106,99,-267});
    rules[593] = new Rule(-327, new int[]{55,-76,99,-267});
    rules[594] = new Rule(-165, new int[]{42});
    rules[595] = new Rule(-308, new int[]{102,-259,-296});
    rules[596] = new Rule(-296, new int[]{101,-259,92});
    rules[597] = new Rule(-296, new int[]{33,-64,92});
    rules[598] = new Rule(-64, new int[]{-67,-261});
    rules[599] = new Rule(-64, new int[]{-67,10,-261});
    rules[600] = new Rule(-64, new int[]{-259});
    rules[601] = new Rule(-67, new int[]{-66});
    rules[602] = new Rule(-67, new int[]{-67,10,-66});
    rules[603] = new Rule(-261, new int[]{});
    rules[604] = new Rule(-261, new int[]{32,-259});
    rules[605] = new Rule(-66, new int[]{79,-68,99,-267});
    rules[606] = new Rule(-68, new int[]{-185});
    rules[607] = new Rule(-68, new int[]{-145,5,-185});
    rules[608] = new Rule(-185, new int[]{-186});
    rules[609] = new Rule(-145, new int[]{-152});
    rules[610] = new Rule(-255, new int[]{47});
    rules[611] = new Rule(-255, new int[]{47,-92});
    rules[612] = new Rule(-76, new int[]{-93});
    rules[613] = new Rule(-76, new int[]{-76,100,-93});
    rules[614] = new Rule(-77, new int[]{-94});
    rules[615] = new Rule(-77, new int[]{-77,100,-94});
    rules[616] = new Rule(-63, new int[]{-180});
    rules[617] = new Rule(-180, new int[]{-179});
    rules[618] = new Rule(-93, new int[]{-92});
    rules[619] = new Rule(-93, new int[]{-330});
    rules[620] = new Rule(-93, new int[]{42});
    rules[621] = new Rule(-94, new int[]{-92});
    rules[622] = new Rule(-94, new int[]{-152,110,-106});
    rules[623] = new Rule(-94, new int[]{-330});
    rules[624] = new Rule(-94, new int[]{42});
    rules[625] = new Rule(-92, new int[]{-106});
    rules[626] = new Rule(-92, new int[]{-125});
    rules[627] = new Rule(-92, new int[]{-92,70,-106});
    rules[628] = new Rule(-106, new int[]{-104});
    rules[629] = new Rule(-106, new int[]{-247});
    rules[630] = new Rule(-106, new int[]{-249});
    rules[631] = new Rule(-122, new int[]{-104});
    rules[632] = new Rule(-122, new int[]{-247});
    rules[633] = new Rule(-123, new int[]{-104});
    rules[634] = new Rule(-123, new int[]{-249});
    rules[635] = new Rule(-108, new int[]{-106});
    rules[636] = new Rule(-108, new int[]{-330});
    rules[637] = new Rule(-109, new int[]{-104});
    rules[638] = new Rule(-109, new int[]{-247});
    rules[639] = new Rule(-109, new int[]{-330});
    rules[640] = new Rule(-104, new int[]{-103});
    rules[641] = new Rule(-104, new int[]{-104,16,-103});
    rules[642] = new Rule(-264, new int[]{21,8,-290,9});
    rules[643] = new Rule(-301, new int[]{22,8,-290,9});
    rules[644] = new Rule(-301, new int[]{22,8,-289,9});
    rules[645] = new Rule(-247, new int[]{-122,13,-122,5,-122});
    rules[646] = new Rule(-249, new int[]{40,-123,51,-123,32,-123});
    rules[647] = new Rule(-289, new int[]{-186,-306});
    rules[648] = new Rule(-289, new int[]{-186,4,-306});
    rules[649] = new Rule(-290, new int[]{-186});
    rules[650] = new Rule(-290, new int[]{-186,-305});
    rules[651] = new Rule(-290, new int[]{-186,4,-305});
    rules[652] = new Rule(-291, new int[]{-290});
    rules[653] = new Rule(-291, new int[]{-280});
    rules[654] = new Rule(-10, new int[]{8,-70,9});
    rules[655] = new Rule(-10, new int[]{});
    rules[656] = new Rule(-179, new int[]{78,-290,-75});
    rules[657] = new Rule(-179, new int[]{78,-290,11,-71,12,-10});
    rules[658] = new Rule(-179, new int[]{78,26,8,-341,9});
    rules[659] = new Rule(-340, new int[]{-152,110,-106});
    rules[660] = new Rule(-340, new int[]{-106});
    rules[661] = new Rule(-341, new int[]{-340});
    rules[662] = new Rule(-341, new int[]{-341,100,-340});
    rules[663] = new Rule(-74, new int[]{});
    rules[664] = new Rule(-74, new int[]{8,-71,9});
    rules[665] = new Rule(-75, new int[]{});
    rules[666] = new Rule(-75, new int[]{8,-72,9});
    rules[667] = new Rule(-103, new int[]{-110});
    rules[668] = new Rule(-103, new int[]{-103,-202,-110});
    rules[669] = new Rule(-103, new int[]{-103,-202,-249});
    rules[670] = new Rule(-103, new int[]{-273,8,-361,9});
    rules[671] = new Rule(-348, new int[]{-290,8,-361,9});
    rules[672] = new Rule(-350, new int[]{-290,8,-362,9});
    rules[673] = new Rule(-349, new int[]{-290,8,-362,9});
    rules[674] = new Rule(-349, new int[]{-365});
    rules[675] = new Rule(-365, new int[]{-347});
    rules[676] = new Rule(-365, new int[]{-365,100,-347});
    rules[677] = new Rule(-347, new int[]{-20});
    rules[678] = new Rule(-347, new int[]{-290});
    rules[679] = new Rule(-347, new int[]{56});
    rules[680] = new Rule(-347, new int[]{-264});
    rules[681] = new Rule(-347, new int[]{-301});
    rules[682] = new Rule(-351, new int[]{11,-363,12});
    rules[683] = new Rule(-363, new int[]{-353});
    rules[684] = new Rule(-363, new int[]{-363,100,-353});
    rules[685] = new Rule(-353, new int[]{-20});
    rules[686] = new Rule(-353, new int[]{-355});
    rules[687] = new Rule(-353, new int[]{14});
    rules[688] = new Rule(-353, new int[]{-350});
    rules[689] = new Rule(-353, new int[]{-351});
    rules[690] = new Rule(-353, new int[]{-352});
    rules[691] = new Rule(-353, new int[]{6});
    rules[692] = new Rule(-355, new int[]{53,-152});
    rules[693] = new Rule(-352, new int[]{8,-364,9});
    rules[694] = new Rule(-354, new int[]{14});
    rules[695] = new Rule(-354, new int[]{-20});
    rules[696] = new Rule(-354, new int[]{-205,-20});
    rules[697] = new Rule(-354, new int[]{53,-152});
    rules[698] = new Rule(-354, new int[]{-350});
    rules[699] = new Rule(-354, new int[]{-351});
    rules[700] = new Rule(-354, new int[]{-352});
    rules[701] = new Rule(-364, new int[]{-354});
    rules[702] = new Rule(-364, new int[]{-364,100,-354});
    rules[703] = new Rule(-362, new int[]{-360});
    rules[704] = new Rule(-362, new int[]{-362,10,-360});
    rules[705] = new Rule(-362, new int[]{-362,100,-360});
    rules[706] = new Rule(-361, new int[]{-359});
    rules[707] = new Rule(-361, new int[]{-361,10,-359});
    rules[708] = new Rule(-361, new int[]{-361,100,-359});
    rules[709] = new Rule(-359, new int[]{14});
    rules[710] = new Rule(-359, new int[]{-20});
    rules[711] = new Rule(-359, new int[]{53,-152,5,-283});
    rules[712] = new Rule(-359, new int[]{53,-152});
    rules[713] = new Rule(-359, new int[]{-348});
    rules[714] = new Rule(-359, new int[]{-351});
    rules[715] = new Rule(-359, new int[]{-352});
    rules[716] = new Rule(-360, new int[]{14});
    rules[717] = new Rule(-360, new int[]{-20});
    rules[718] = new Rule(-360, new int[]{-205,-20});
    rules[719] = new Rule(-360, new int[]{-152,5,-283});
    rules[720] = new Rule(-360, new int[]{-152});
    rules[721] = new Rule(-360, new int[]{53,-152,5,-283});
    rules[722] = new Rule(-360, new int[]{53,-152});
    rules[723] = new Rule(-360, new int[]{-350});
    rules[724] = new Rule(-360, new int[]{-351});
    rules[725] = new Rule(-360, new int[]{-352});
    rules[726] = new Rule(-120, new int[]{-110});
    rules[727] = new Rule(-120, new int[]{});
    rules[728] = new Rule(-127, new int[]{-95});
    rules[729] = new Rule(-127, new int[]{});
    rules[730] = new Rule(-125, new int[]{-110,5,-120});
    rules[731] = new Rule(-125, new int[]{5,-120});
    rules[732] = new Rule(-125, new int[]{-110,5,-120,5,-110});
    rules[733] = new Rule(-125, new int[]{5,-120,5,-110});
    rules[734] = new Rule(-126, new int[]{-95,5,-127});
    rules[735] = new Rule(-126, new int[]{5,-127});
    rules[736] = new Rule(-126, new int[]{-95,5,-127,5,-95});
    rules[737] = new Rule(-126, new int[]{5,-127,5,-95});
    rules[738] = new Rule(-202, new int[]{120});
    rules[739] = new Rule(-202, new int[]{125});
    rules[740] = new Rule(-202, new int[]{123});
    rules[741] = new Rule(-202, new int[]{121});
    rules[742] = new Rule(-202, new int[]{124});
    rules[743] = new Rule(-202, new int[]{122});
    rules[744] = new Rule(-202, new int[]{137});
    rules[745] = new Rule(-202, new int[]{135,137});
    rules[746] = new Rule(-110, new int[]{-88});
    rules[747] = new Rule(-110, new int[]{-110,6,-88});
    rules[748] = new Rule(-88, new int[]{-87});
    rules[749] = new Rule(-88, new int[]{-88,-203,-87});
    rules[750] = new Rule(-88, new int[]{-88,-203,-249});
    rules[751] = new Rule(-203, new int[]{116});
    rules[752] = new Rule(-203, new int[]{115});
    rules[753] = new Rule(-203, new int[]{128});
    rules[754] = new Rule(-203, new int[]{129});
    rules[755] = new Rule(-203, new int[]{126});
    rules[756] = new Rule(-207, new int[]{136});
    rules[757] = new Rule(-207, new int[]{138});
    rules[758] = new Rule(-271, new int[]{-273});
    rules[759] = new Rule(-271, new int[]{-274});
    rules[760] = new Rule(-274, new int[]{-87,136,-290});
    rules[761] = new Rule(-274, new int[]{-87,136,-285});
    rules[762] = new Rule(-273, new int[]{-87,138,-290});
    rules[763] = new Rule(-273, new int[]{-87,138,-285});
    rules[764] = new Rule(-275, new int[]{-102,119,-101});
    rules[765] = new Rule(-275, new int[]{-102,119,-275});
    rules[766] = new Rule(-275, new int[]{-205,-275});
    rules[767] = new Rule(-87, new int[]{-101});
    rules[768] = new Rule(-87, new int[]{-179});
    rules[769] = new Rule(-87, new int[]{-275});
    rules[770] = new Rule(-87, new int[]{-87,-204,-101});
    rules[771] = new Rule(-87, new int[]{-87,-204,-275});
    rules[772] = new Rule(-87, new int[]{-87,-204,-249});
    rules[773] = new Rule(-87, new int[]{-271});
    rules[774] = new Rule(-204, new int[]{118});
    rules[775] = new Rule(-204, new int[]{117});
    rules[776] = new Rule(-204, new int[]{131});
    rules[777] = new Rule(-204, new int[]{132});
    rules[778] = new Rule(-204, new int[]{133});
    rules[779] = new Rule(-204, new int[]{134});
    rules[780] = new Rule(-204, new int[]{130});
    rules[781] = new Rule(-61, new int[]{63,8,-291,9});
    rules[782] = new Rule(-62, new int[]{8,-107,100,-84,-332,-339,9});
    rules[783] = new Rule(-102, new int[]{-20});
    rules[784] = new Rule(-102, new int[]{-118});
    rules[785] = new Rule(-101, new int[]{56});
    rules[786] = new Rule(-101, new int[]{-20});
    rules[787] = new Rule(-101, new int[]{-61});
    rules[788] = new Rule(-101, new int[]{135,-101});
    rules[789] = new Rule(-101, new int[]{-205,-101});
    rules[790] = new Rule(-101, new int[]{142,-101});
    rules[791] = new Rule(-101, new int[]{-118});
    rules[792] = new Rule(-101, new int[]{-62});
    rules[793] = new Rule(-20, new int[]{-171});
    rules[794] = new Rule(-20, new int[]{-21});
    rules[795] = new Rule(-121, new int[]{-116,15,-116});
    rules[796] = new Rule(-121, new int[]{-116,15,-121});
    rules[797] = new Rule(-118, new int[]{-137,-116});
    rules[798] = new Rule(-118, new int[]{-116});
    rules[799] = new Rule(-118, new int[]{-121});
    rules[800] = new Rule(-118, new int[]{8,53,-152,110,-104,9});
    rules[801] = new Rule(-137, new int[]{141});
    rules[802] = new Rule(-137, new int[]{-137,141});
    rules[803] = new Rule(-14, new int[]{-186,-74});
    rules[804] = new Rule(-14, new int[]{-307,-74});
    rules[805] = new Rule(-329, new int[]{-152});
    rules[806] = new Rule(-329, new int[]{-329,7,-143});
    rules[807] = new Rule(-328, new int[]{-329});
    rules[808] = new Rule(-328, new int[]{-329,-305});
    rules[809] = new Rule(-366, new int[]{53,-152,110,-92,10});
    rules[810] = new Rule(-367, new int[]{-366});
    rules[811] = new Rule(-367, new int[]{-367,-366});
    rules[812] = new Rule(-117, new int[]{-116,8,-72,9});
    rules[813] = new Rule(-116, new int[]{-152});
    rules[814] = new Rule(-116, new int[]{-197});
    rules[815] = new Rule(-116, new int[]{42,-152});
    rules[816] = new Rule(-116, new int[]{8,-92,9});
    rules[817] = new Rule(-116, new int[]{8,-367,-92,9});
    rules[818] = new Rule(-116, new int[]{-264});
    rules[819] = new Rule(-116, new int[]{-301});
    rules[820] = new Rule(-116, new int[]{-20,7,-143});
    rules[821] = new Rule(-116, new int[]{-116,11,-76,12});
    rules[822] = new Rule(-116, new int[]{-20,11,-76,12});
    rules[823] = new Rule(-116, new int[]{-116,17,-125,12});
    rules[824] = new Rule(-116, new int[]{-20,17,-125,12});
    rules[825] = new Rule(-116, new int[]{76,-73,76});
    rules[826] = new Rule(-116, new int[]{-246});
    rules[827] = new Rule(-116, new int[]{-117});
    rules[828] = new Rule(-116, new int[]{-116,7,-153});
    rules[829] = new Rule(-116, new int[]{-62,7,-153});
    rules[830] = new Rule(-116, new int[]{-116,142});
    rules[831] = new Rule(-116, new int[]{-116,4,-305});
    rules[832] = new Rule(-71, new int[]{-76});
    rules[833] = new Rule(-71, new int[]{});
    rules[834] = new Rule(-72, new int[]{-77});
    rules[835] = new Rule(-72, new int[]{});
    rules[836] = new Rule(-73, new int[]{-82});
    rules[837] = new Rule(-73, new int[]{});
    rules[838] = new Rule(-82, new int[]{-97});
    rules[839] = new Rule(-82, new int[]{-82,100,-97});
    rules[840] = new Rule(-97, new int[]{-92});
    rules[841] = new Rule(-97, new int[]{-92,6,-92});
    rules[842] = new Rule(-172, new int[]{144});
    rules[843] = new Rule(-172, new int[]{147});
    rules[844] = new Rule(-171, new int[]{-173});
    rules[845] = new Rule(-171, new int[]{145});
    rules[846] = new Rule(-171, new int[]{146});
    rules[847] = new Rule(-173, new int[]{-172});
    rules[848] = new Rule(-173, new int[]{-173,-172});
    rules[849] = new Rule(-197, new int[]{45,-206});
    rules[850] = new Rule(-213, new int[]{10});
    rules[851] = new Rule(-213, new int[]{10,-212,10});
    rules[852] = new Rule(-214, new int[]{});
    rules[853] = new Rule(-214, new int[]{10,-212});
    rules[854] = new Rule(-212, new int[]{-215});
    rules[855] = new Rule(-212, new int[]{-212,10,-215});
    rules[856] = new Rule(-152, new int[]{143});
    rules[857] = new Rule(-152, new int[]{161});
    rules[858] = new Rule(-152, new int[]{-156});
    rules[859] = new Rule(-152, new int[]{-157});
    rules[860] = new Rule(-152, new int[]{160});
    rules[861] = new Rule(-152, new int[]{87});
    rules[862] = new Rule(-143, new int[]{-152});
    rules[863] = new Rule(-143, new int[]{-299});
    rules[864] = new Rule(-143, new int[]{-300});
    rules[865] = new Rule(-153, new int[]{-152});
    rules[866] = new Rule(-153, new int[]{-299});
    rules[867] = new Rule(-153, new int[]{-197});
    rules[868] = new Rule(-215, new int[]{148});
    rules[869] = new Rule(-215, new int[]{150});
    rules[870] = new Rule(-215, new int[]{151});
    rules[871] = new Rule(-215, new int[]{152});
    rules[872] = new Rule(-215, new int[]{154});
    rules[873] = new Rule(-215, new int[]{153});
    rules[874] = new Rule(-216, new int[]{153});
    rules[875] = new Rule(-216, new int[]{152});
    rules[876] = new Rule(-216, new int[]{148});
    rules[877] = new Rule(-216, new int[]{151});
    rules[878] = new Rule(-156, new int[]{85});
    rules[879] = new Rule(-156, new int[]{86});
    rules[880] = new Rule(-157, new int[]{80});
    rules[881] = new Rule(-157, new int[]{78});
    rules[882] = new Rule(-155, new int[]{84});
    rules[883] = new Rule(-155, new int[]{83});
    rules[884] = new Rule(-155, new int[]{82});
    rules[885] = new Rule(-155, new int[]{81});
    rules[886] = new Rule(-299, new int[]{-155});
    rules[887] = new Rule(-299, new int[]{68});
    rules[888] = new Rule(-299, new int[]{64});
    rules[889] = new Rule(-299, new int[]{128});
    rules[890] = new Rule(-299, new int[]{22});
    rules[891] = new Rule(-299, new int[]{21});
    rules[892] = new Rule(-299, new int[]{63});
    rules[893] = new Rule(-299, new int[]{23});
    rules[894] = new Rule(-299, new int[]{129});
    rules[895] = new Rule(-299, new int[]{130});
    rules[896] = new Rule(-299, new int[]{131});
    rules[897] = new Rule(-299, new int[]{132});
    rules[898] = new Rule(-299, new int[]{133});
    rules[899] = new Rule(-299, new int[]{134});
    rules[900] = new Rule(-299, new int[]{135});
    rules[901] = new Rule(-299, new int[]{136});
    rules[902] = new Rule(-299, new int[]{137});
    rules[903] = new Rule(-299, new int[]{138});
    rules[904] = new Rule(-299, new int[]{24});
    rules[905] = new Rule(-299, new int[]{73});
    rules[906] = new Rule(-299, new int[]{91});
    rules[907] = new Rule(-299, new int[]{25});
    rules[908] = new Rule(-299, new int[]{26});
    rules[909] = new Rule(-299, new int[]{29});
    rules[910] = new Rule(-299, new int[]{30});
    rules[911] = new Rule(-299, new int[]{31});
    rules[912] = new Rule(-299, new int[]{71});
    rules[913] = new Rule(-299, new int[]{99});
    rules[914] = new Rule(-299, new int[]{32});
    rules[915] = new Rule(-299, new int[]{92});
    rules[916] = new Rule(-299, new int[]{33});
    rules[917] = new Rule(-299, new int[]{34});
    rules[918] = new Rule(-299, new int[]{27});
    rules[919] = new Rule(-299, new int[]{104});
    rules[920] = new Rule(-299, new int[]{101});
    rules[921] = new Rule(-299, new int[]{35});
    rules[922] = new Rule(-299, new int[]{36});
    rules[923] = new Rule(-299, new int[]{37});
    rules[924] = new Rule(-299, new int[]{40});
    rules[925] = new Rule(-299, new int[]{41});
    rules[926] = new Rule(-299, new int[]{42});
    rules[927] = new Rule(-299, new int[]{103});
    rules[928] = new Rule(-299, new int[]{43});
    rules[929] = new Rule(-299, new int[]{44});
    rules[930] = new Rule(-299, new int[]{46});
    rules[931] = new Rule(-299, new int[]{47});
    rules[932] = new Rule(-299, new int[]{48});
    rules[933] = new Rule(-299, new int[]{97});
    rules[934] = new Rule(-299, new int[]{49});
    rules[935] = new Rule(-299, new int[]{102});
    rules[936] = new Rule(-299, new int[]{50});
    rules[937] = new Rule(-299, new int[]{28});
    rules[938] = new Rule(-299, new int[]{51});
    rules[939] = new Rule(-299, new int[]{70});
    rules[940] = new Rule(-299, new int[]{98});
    rules[941] = new Rule(-299, new int[]{52});
    rules[942] = new Rule(-299, new int[]{53});
    rules[943] = new Rule(-299, new int[]{54});
    rules[944] = new Rule(-299, new int[]{55});
    rules[945] = new Rule(-299, new int[]{56});
    rules[946] = new Rule(-299, new int[]{57});
    rules[947] = new Rule(-299, new int[]{58});
    rules[948] = new Rule(-299, new int[]{59});
    rules[949] = new Rule(-299, new int[]{61});
    rules[950] = new Rule(-299, new int[]{105});
    rules[951] = new Rule(-299, new int[]{106});
    rules[952] = new Rule(-299, new int[]{109});
    rules[953] = new Rule(-299, new int[]{107});
    rules[954] = new Rule(-299, new int[]{108});
    rules[955] = new Rule(-299, new int[]{62});
    rules[956] = new Rule(-299, new int[]{74});
    rules[957] = new Rule(-299, new int[]{38});
    rules[958] = new Rule(-299, new int[]{39});
    rules[959] = new Rule(-299, new int[]{69});
    rules[960] = new Rule(-299, new int[]{148});
    rules[961] = new Rule(-299, new int[]{60});
    rules[962] = new Rule(-299, new int[]{139});
    rules[963] = new Rule(-299, new int[]{140});
    rules[964] = new Rule(-299, new int[]{79});
    rules[965] = new Rule(-299, new int[]{153});
    rules[966] = new Rule(-299, new int[]{152});
    rules[967] = new Rule(-299, new int[]{72});
    rules[968] = new Rule(-299, new int[]{154});
    rules[969] = new Rule(-299, new int[]{150});
    rules[970] = new Rule(-299, new int[]{151});
    rules[971] = new Rule(-299, new int[]{149});
    rules[972] = new Rule(-300, new int[]{45});
    rules[973] = new Rule(-206, new int[]{115});
    rules[974] = new Rule(-206, new int[]{116});
    rules[975] = new Rule(-206, new int[]{117});
    rules[976] = new Rule(-206, new int[]{118});
    rules[977] = new Rule(-206, new int[]{120});
    rules[978] = new Rule(-206, new int[]{121});
    rules[979] = new Rule(-206, new int[]{122});
    rules[980] = new Rule(-206, new int[]{123});
    rules[981] = new Rule(-206, new int[]{124});
    rules[982] = new Rule(-206, new int[]{125});
    rules[983] = new Rule(-206, new int[]{128});
    rules[984] = new Rule(-206, new int[]{129});
    rules[985] = new Rule(-206, new int[]{130});
    rules[986] = new Rule(-206, new int[]{131});
    rules[987] = new Rule(-206, new int[]{132});
    rules[988] = new Rule(-206, new int[]{133});
    rules[989] = new Rule(-206, new int[]{134});
    rules[990] = new Rule(-206, new int[]{135});
    rules[991] = new Rule(-206, new int[]{137});
    rules[992] = new Rule(-206, new int[]{139});
    rules[993] = new Rule(-206, new int[]{140});
    rules[994] = new Rule(-206, new int[]{-200});
    rules[995] = new Rule(-206, new int[]{119});
    rules[996] = new Rule(-200, new int[]{110});
    rules[997] = new Rule(-200, new int[]{111});
    rules[998] = new Rule(-200, new int[]{112});
    rules[999] = new Rule(-200, new int[]{113});
    rules[1000] = new Rule(-200, new int[]{114});
    rules[1001] = new Rule(-105, new int[]{18,-28,100,-27,9});
    rules[1002] = new Rule(-27, new int[]{-105});
    rules[1003] = new Rule(-27, new int[]{-152});
    rules[1004] = new Rule(-28, new int[]{-27});
    rules[1005] = new Rule(-28, new int[]{-28,100,-27});
    rules[1006] = new Rule(-107, new int[]{-106});
    rules[1007] = new Rule(-107, new int[]{-105});
    rules[1008] = new Rule(-84, new int[]{-107});
    rules[1009] = new Rule(-84, new int[]{-84,100,-107});
    rules[1010] = new Rule(-330, new int[]{-152,127,-336});
    rules[1011] = new Rule(-330, new int[]{8,9,-333,127,-336});
    rules[1012] = new Rule(-330, new int[]{8,-152,5,-282,9,-333,127,-336});
    rules[1013] = new Rule(-330, new int[]{8,-152,10,-334,9,-333,127,-336});
    rules[1014] = new Rule(-330, new int[]{8,-152,5,-282,10,-334,9,-333,127,-336});
    rules[1015] = new Rule(-330, new int[]{8,-107,100,-84,-332,-339,9,-343});
    rules[1016] = new Rule(-330, new int[]{-105,-343});
    rules[1017] = new Rule(-330, new int[]{-331});
    rules[1018] = new Rule(-339, new int[]{});
    rules[1019] = new Rule(-339, new int[]{10,-334});
    rules[1020] = new Rule(-343, new int[]{-333,127,-336});
    rules[1021] = new Rule(-331, new int[]{37,-333,127,-336});
    rules[1022] = new Rule(-331, new int[]{37,8,9,-333,127,-336});
    rules[1023] = new Rule(-331, new int[]{37,8,-334,9,-333,127,-336});
    rules[1024] = new Rule(-331, new int[]{44,127,-337});
    rules[1025] = new Rule(-331, new int[]{44,8,9,127,-337});
    rules[1026] = new Rule(-331, new int[]{44,8,-334,9,127,-337});
    rules[1027] = new Rule(-334, new int[]{-335});
    rules[1028] = new Rule(-334, new int[]{-334,10,-335});
    rules[1029] = new Rule(-335, new int[]{-163,-332});
    rules[1030] = new Rule(-332, new int[]{});
    rules[1031] = new Rule(-332, new int[]{5,-282});
    rules[1032] = new Rule(-333, new int[]{});
    rules[1033] = new Rule(-333, new int[]{5,-284});
    rules[1034] = new Rule(-338, new int[]{-262});
    rules[1035] = new Rule(-338, new int[]{-158});
    rules[1036] = new Rule(-338, new int[]{-326});
    rules[1037] = new Rule(-338, new int[]{-254});
    rules[1038] = new Rule(-338, new int[]{-129});
    rules[1039] = new Rule(-338, new int[]{-128});
    rules[1040] = new Rule(-338, new int[]{-130});
    rules[1041] = new Rule(-338, new int[]{-39});
    rules[1042] = new Rule(-338, new int[]{-308});
    rules[1043] = new Rule(-338, new int[]{-175});
    rules[1044] = new Rule(-338, new int[]{-255});
    rules[1045] = new Rule(-338, new int[]{-131});
    rules[1046] = new Rule(-338, new int[]{8,-9,9});
    rules[1047] = new Rule(-336, new int[]{-109});
    rules[1048] = new Rule(-336, new int[]{-338});
    rules[1049] = new Rule(-337, new int[]{-218});
    rules[1050] = new Rule(-337, new int[]{-9});
    rules[1051] = new Rule(-337, new int[]{-338});
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
{ CurrentSemanticValue.stn = null; }
        break;
      case 42: // uses_clause -> uses_clause_nonempty
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 43: // uses_clause_nonempty -> uses_clause_one
{
            CurrentSemanticValue.stn = parserTools.buildTreeForFormatter ? new uses_closure((uses_list)ValueStack[ValueStack.Depth-1].stn, LocationStack[LocationStack.Depth-1]) : ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 44: // uses_clause_nonempty -> uses_clause_nonempty, uses_clause_one
{
            if (parserTools.buildTreeForFormatter)
                ((uses_closure)ValueStack[ValueStack.Depth-2].stn).AddUsesList((uses_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
            else
                ((uses_list)ValueStack[ValueStack.Depth-2].stn).AddUsesList((uses_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
            CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 45: // used_units_list -> used_unit_name
{ 
		  CurrentSemanticValue.stn = new uses_list(ValueStack[ValueStack.Depth-1].stn as unit_or_namespace,CurrentLocationSpan);
        }
        break;
      case 46: // used_units_list -> used_units_list, tkComma, used_unit_name
{ 
		  CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as uses_list).Add(ValueStack[ValueStack.Depth-1].stn as unit_or_namespace, CurrentLocationSpan);
        }
        break;
      case 47: // used_unit_name -> ident_or_keyword_pointseparator_list
{ 
			CurrentSemanticValue.stn = new unit_or_namespace(ValueStack[ValueStack.Depth-1].stn as ident_list,CurrentLocationSpan); 
		}
        break;
      case 48: // used_unit_name -> ident_or_keyword_pointseparator_list, tkIn, tkStringLiteral
{ 
        	if (ValueStack[ValueStack.Depth-1].stn is char_const _cc)
        		ValueStack[ValueStack.Depth-1].stn = new string_const(_cc.cconst.ToString());
			CurrentSemanticValue.stn = new uses_unit_in(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].stn as string_const, CurrentLocationSpan);
        }
        break;
      case 49: // unit_file -> unit_header, interface_part, implementation_part, 
               //              initialization_part, tkPoint
{ 
			CurrentSemanticValue.stn = new unit_module(ValueStack[ValueStack.Depth-5].stn as unit_name, ValueStack[ValueStack.Depth-4].stn as interface_node, ValueStack[ValueStack.Depth-3].stn as implementation_node, 
			  (ValueStack[ValueStack.Depth-2].stn as initfinal_part).initialization_sect, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).finalization_sect, /*$1 as attribute_list*/ null, CurrentLocationSpan);             
		}
        break;
      case 50: // unit_file -> unit_header, abc_interface_part, initialization_part, tkPoint
{ 
			CurrentSemanticValue.stn = new unit_module(ValueStack[ValueStack.Depth-4].stn as unit_name, ValueStack[ValueStack.Depth-3].stn as interface_node, null, 
			  (ValueStack[ValueStack.Depth-2].stn as initfinal_part).initialization_sect, (ValueStack[ValueStack.Depth-2].stn as initfinal_part).finalization_sect, /*$1 as attribute_list*/ null, CurrentLocationSpan); 
        }
        break;
      case 51: // unit_header -> unit_key_word, unit_name, tkSemiColon, 
               //                optional_head_compiler_directives
{ 
			CurrentSemanticValue.stn = NewUnitHeading(new ident(ValueStack[ValueStack.Depth-4].ti.text, LocationStack[LocationStack.Depth-4]), ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan); 
		}
        break;
      case 52: // unit_header -> tkNamespace, ident_or_keyword_pointseparator_list, tkSemiColon, 
               //                optional_head_compiler_directives
{
            CurrentSemanticValue.stn = NewNamespaceHeading(new ident(ValueStack[ValueStack.Depth-4].ti.text, LocationStack[LocationStack.Depth-4]), ValueStack[ValueStack.Depth-3].stn as ident_list, CurrentLocationSpan);
        }
        break;
      case 53: // unit_key_word -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 54: // unit_key_word -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 55: // unit_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 56: // interface_part -> tkInterface, uses_clause_nonempty, interface_declarations
{
            CurrentSemanticValue.stn = new interface_node((declarations)ValueStack[ValueStack.Depth-1].stn, (uses_list)ValueStack[ValueStack.Depth-2].stn, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]));
        }
        break;
      case 57: // interface_part -> tkInterface, interface_declarations
{
            CurrentSemanticValue.stn = new interface_node((declarations)ValueStack[ValueStack.Depth-1].stn, null, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]));
        }
        break;
      case 58: // interface_part -> tkInterface, unit_reexport_nonempty, uses_clause_nonempty, 
               //                   unit_reexport_clauses, interface_decl_sect_list
{
            parserTools.AddErrorFromResource("REEXPORT_BEFORE_USES", LocationStack[LocationStack.Depth-4]);
            CurrentSemanticValue.stn = new interface_node((declarations)ValueStack[ValueStack.Depth-1].stn, (uses_list)ValueStack[ValueStack.Depth-3].stn, null, CurrentLocationSpan);
        }
        break;
      case 59: // interface_declarations -> unit_reexport_clauses, interface_decl_sect_list
{
            var defs = (declarations)ValueStack[ValueStack.Depth-1].stn;
            defs.defs.InsertRange(0, ((declarations)ValueStack[ValueStack.Depth-2].stn).defs);
            CurrentSemanticValue.stn = defs;
        }
        break;
      case 60: // unit_reexport_clauses -> /* empty */
{ CurrentSemanticValue.stn = new declarations(); }
        break;
      case 61: // unit_reexport_clauses -> unit_reexport_nonempty
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 62: // unit_reexport_nonempty -> tkExport, unit_reexport_list, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 63: // unit_reexport_nonempty -> unit_reexport_nonempty, tkExport, unit_reexport_list, 
               //                           tkSemiColon
{
            var defs = (declarations)ValueStack[ValueStack.Depth-4].stn;
            defs.defs.AddRange(((declarations)ValueStack[ValueStack.Depth-2].stn).defs);
            CurrentSemanticValue.stn = defs;
        }
        break;
      case 64: // unit_reexport_list -> unit_reexport_item
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn == null ? new declarations() : new declarations().Add((declaration)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); }
        break;
      case 65: // unit_reexport_list -> unit_reexport_list, tkComma, unit_reexport_item
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn == null ? ValueStack[ValueStack.Depth-3].stn : ((declarations)ValueStack[ValueStack.Depth-3].stn).Add((declaration)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); }
        break;
      case 66: // unit_reexport_item -> identifier, tkPoint, identifier
{ CurrentSemanticValue.stn = new unit_reexport(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan); }
        break;
      case 67: // unit_reexport_item -> identifier, tkPoint, identifier, tkAs, identifier
{ CurrentSemanticValue.stn = new unit_reexport(ValueStack[ValueStack.Depth-5].id, ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); }
        break;
      case 68: // unit_reexport_item -> identifier
{ parserTools.AddErrorFromResource("REEXPORT_REQUIRES_MODULE_NAME{0}", LocationStack[LocationStack.Depth-1], ValueStack[ValueStack.Depth-1].id.name); CurrentSemanticValue.stn = null; }
        break;
      case 69: // unit_reexport_item -> identifier, tkAs, identifier
{ parserTools.AddErrorFromResource("REEXPORT_REQUIRES_MODULE_NAME{0}", LocationStack[LocationStack.Depth-3], ValueStack[ValueStack.Depth-3].id.name); CurrentSemanticValue.stn = null; }
        break;
      case 70: // implementation_part -> tkImplementation, uses_clause, decl_sect_list
{ 
			CurrentSemanticValue.stn = new implementation_node(ValueStack[ValueStack.Depth-2].stn as uses_list, ValueStack[ValueStack.Depth-1].stn as declarations, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
        }
        break;
      case 71: // abc_interface_part -> uses_clause, decl_sect_list
{ 
			CurrentSemanticValue.stn = new interface_node(ValueStack[ValueStack.Depth-1].stn as declarations, ValueStack[ValueStack.Depth-2].stn as uses_list, null, null); 
        }
        break;
      case 72: // initialization_part -> tkEnd
{ 
			CurrentSemanticValue.stn = new initfinal_part(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 73: // initialization_part -> tkInitialization, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 74: // initialization_part -> tkInitialization, stmt_list, tkFinalization, stmt_list, 
               //                        tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-5].ti, ValueStack[ValueStack.Depth-4].stn as statement_list, ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, CurrentLocationSpan);
        }
        break;
      case 75: // initialization_part -> tkBegin, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 76: // interface_decl_sect_list -> int_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 77: // int_decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations();  
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 78: // int_decl_sect_list1 -> int_decl_sect_list1, int_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 79: // decl_sect_list -> decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 80: // decl_sect_list_proc_func_only -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 81: // decl_sect_list_proc_func_only -> decl_sect_list_proc_func_only, 
               //                                  proc_func_decl_without_modifiers
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
      case 82: // decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 83: // decl_sect_list1 -> decl_sect_list1, decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 84: // inclass_decl_sect_list -> inclass_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 85: // inclass_decl_sect_list1 -> /* empty */
{ 
        	CurrentSemanticValue.stn = new declarations(); 
        }
        break;
      case 86: // inclass_decl_sect_list1 -> inclass_decl_sect_list1, abc_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 87: // int_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 88: // int_decl_sect -> const_decl_sect, unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 89: // int_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 90: // int_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 91: // int_decl_sect -> type_decl_sect, attribute_declarations, 
               //                  unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn; }
        break;
      case 92: // int_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 93: // int_decl_sect -> var_decl_sect, unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 94: // int_decl_sect -> int_proc_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 95: // int_decl_sect -> int_proc_header, unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].td; }
        break;
      case 96: // int_decl_sect -> int_func_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 97: // int_decl_sect -> int_func_header, unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].td; }
        break;
      case 98: // unit_reexport_after_declarations -> unit_reexport_nonempty
{ parserTools.AddErrorFromResource("REEXPORT_AFTER_DECLARATIONS", LocationStack[LocationStack.Depth-1]); CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 99: // decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 100: // decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 101: // decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 102: // decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 103: // decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 104: // decl_sect -> proc_func_constr_destr_decl_with_attr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 105: // proc_func_constr_destr_decl -> proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 106: // proc_func_constr_destr_decl -> constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 107: // proc_func_constr_destr_decl_with_attr -> attribute_declarations, 
                //                                          proc_func_constr_destr_decl
{
		    (ValueStack[ValueStack.Depth-1].stn as procedure_definition).AssignAttrList(ValueStack[ValueStack.Depth-2].stn as attribute_list);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 108: // abc_decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 109: // abc_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 110: // abc_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 111: // abc_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 112: // abc_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 113: // int_proc_header -> attribute_declarations, proc_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 114: // int_proc_header -> attribute_declarations, proc_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 115: // int_func_header -> attribute_declarations, func_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 116: // int_func_header -> attribute_declarations, func_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 117: // label_decl_sect -> tkLabel, label_list, tkSemiColon
{ 
			CurrentSemanticValue.stn = new label_definitions(ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
		}
        break;
      case 118: // label_list -> label_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 119: // label_list -> label_list, tkComma, label_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 120: // label_name -> tkInteger
{ 
			CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ex.ToString(), CurrentLocationSpan);
		}
        break;
      case 121: // label_name -> identifier
{ 
			CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; 
		}
        break;
      case 122: // const_decl_sect -> tkConst, const_decl
{ 
			CurrentSemanticValue.stn = new consts_definitions_list(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 123: // const_decl_sect -> const_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as consts_definitions_list).Add(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 124: // res_str_decl_sect -> tkResourceString, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 125: // res_str_decl_sect -> res_str_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 126: // type_decl_sect -> tkType, type_decl
{ 
            CurrentSemanticValue.stn = new type_declarations(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 127: // type_decl_sect -> type_decl_sect, type_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as type_declarations).Add(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 128: // var_decl_with_assign_var_tuple -> var_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 129: // var_decl_with_assign_var_tuple -> tkRoundOpen, identifier, tkComma, ident_list, 
                //                                   tkRoundClose, tkAssign, expr_l1, 
                //                                   tkSemiColon
{
			(ValueStack[ValueStack.Depth-5].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-7].id);
			ValueStack[ValueStack.Depth-5].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]);
			CurrentSemanticValue.stn = new var_tuple_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
		}
        break;
      case 130: // var_decl_sect -> tkVar, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 131: // var_decl_sect -> tkEvent, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);                        
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).is_event = true;
        }
        break;
      case 132: // var_decl_sect -> var_decl_sect, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as variable_definitions).Add(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 133: // const_decl -> only_const_decl, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 134: // only_const_decl -> const_name, tkEqual, init_const_expr
{ 
			CurrentSemanticValue.stn = new simple_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 135: // only_const_decl -> const_name, tkColon, type_ref, tkEqual, typed_const
{ 
			CurrentSemanticValue.stn = new typed_const_definition(ValueStack[ValueStack.Depth-5].id, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-3].td, CurrentLocationSpan);
		}
        break;
      case 136: // init_const_expr -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 137: // init_const_expr -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 138: // const_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 139: // const_relop_expr -> const_simple_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 140: // const_relop_expr -> const_relop_expr, const_relop, const_simple_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 141: // const_expr -> const_relop_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 142: // const_expr -> question_constexpr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 143: // const_expr -> const_expr, tkDoubleQuestion, const_relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 144: // question_constexpr -> const_expr, tkQuestion, const_expr, tkColon, const_expr
{ CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 145: // const_relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 146: // const_relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 147: // const_relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 148: // const_relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 149: // const_relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 150: // const_relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 151: // const_relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 152: // const_relop -> tkNot, tkIn
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
      case 153: // const_simple_expr -> const_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 154: // const_simple_expr -> const_simple_expr, const_addop, const_term
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 155: // const_addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 156: // const_addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 157: // const_addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 158: // const_addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 159: // as_is_constexpr -> const_term, typecast_op, simple_or_template_type_reference
{ 
			CurrentSemanticValue.ex = NewAsIsConstexpr(ValueStack[ValueStack.Depth-3].ex, (op_typecast)ValueStack[ValueStack.Depth-2].ob, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);                                
		}
        break;
      case 160: // power_constexpr -> const_factor_without_unary_op, tkStarStar, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 161: // power_constexpr -> const_factor_without_unary_op, tkStarStar, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 162: // power_constexpr -> sign, power_constexpr
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 163: // const_term -> const_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 164: // const_term -> as_is_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 165: // const_term -> power_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 166: // const_term -> const_term, const_mulop, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 167: // const_term -> const_term, const_mulop, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 168: // const_mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 169: // const_mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 170: // const_mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 171: // const_mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 172: // const_mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 173: // const_mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 174: // const_mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 175: // const_factor_without_unary_op -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 176: // const_factor_without_unary_op -> tkRoundOpen, const_expr, tkRoundClose
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 177: // const_factor -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 178: // const_factor -> const_set
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 179: // const_factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 180: // const_factor -> tkAddressOf, const_factor
{ 
			CurrentSemanticValue.ex = new get_address(ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);  
		}
        break;
      case 181: // const_factor -> tkRoundOpen, const_expr, tkRoundClose
{ 
	 	    CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 182: // const_factor -> tkNot, const_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 183: // const_factor -> sign, const_factor
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
      case 184: // const_factor -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 185: // const_factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 186: // pascal_set_const -> tkSquareOpen, elem_list, tkSquareClose
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
			else CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, '[', CurrentLocationSpan); 				
		}
        break;
      case 187: // const_set -> pascal_set_const
{
            CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;				
		}
        break;
      case 188: // const_set -> tkVertParen, elem_list, tkVertParen
{ 
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, '|', CurrentLocationSpan);  
		}
        break;
      case 189: // sign -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 190: // sign -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 191: // const_variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 192: // const_variable -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 193: // const_variable -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 194: // const_variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 195: // const_variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 196: // const_variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 197: // const_variable -> const_variable, const_variable_2
{
			CurrentSemanticValue.ex = NewConstVariable(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 198: // const_variable -> const_variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 199: // const_variable -> const_variable, tkSquareOpen, format_const_expr, 
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
      case 200: // const_variable_2 -> tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.ex = new dot_node(null, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 201: // const_variable_2 -> tkDeref
{ 
			CurrentSemanticValue.ex = new roof_dereference();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 202: // const_variable_2 -> tkRoundOpen, optional_const_func_expr_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 203: // const_variable_2 -> tkSquareOpen, const_elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 204: // optional_const_func_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 205: // optional_const_func_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 206: // const_elem_list -> const_elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 208: // const_elem_list1 -> const_elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 209: // const_elem_list1 -> const_elem_list1, tkComma, const_elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 210: // const_elem -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 211: // const_elem -> const_expr, tkDotDot, const_expr
{ 
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 212: // unsigned_number -> tkInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 213: // unsigned_number -> tkHex
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 214: // unsigned_number -> tkFloat
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 215: // unsigned_number -> tkBigInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 216: // typed_const -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 217: // typed_const -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 218: // typed_const -> record_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 219: // array_const -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new array_const(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 221: // typed_const_list -> typed_const_list1
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 222: // typed_const_list1 -> typed_const_plus
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 223: // typed_const_list1 -> typed_const_list1, tkComma, typed_const_plus
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 224: // record_const -> tkRoundOpen, const_field_list, tkRoundClose
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 225: // const_field_list -> const_field_list_1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 226: // const_field_list -> const_field_list_1, tkSemiColon
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 227: // const_field_list_1 -> const_field
{ 
			CurrentSemanticValue.ex = new record_const(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 228: // const_field_list_1 -> const_field_list_1, tkSemiColon, const_field
{ 
			CurrentSemanticValue.ex = (ValueStack[ValueStack.Depth-3].ex as record_const).Add(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 229: // const_field -> const_field_name, tkColon, typed_const
{ 
			CurrentSemanticValue.stn = new record_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 230: // const_field_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 231: // type_decl -> attribute_declarations, simple_type_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = LocationStack[LocationStack.Depth-1];
        }
        break;
      case 232: // attribute_declarations -> attribute_declaration
{ 
			CurrentSemanticValue.stn = new attribute_list(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
    }
        break;
      case 233: // attribute_declarations -> attribute_declarations, attribute_declaration
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
		}
        break;
      case 234: // attribute_declarations -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 235: // attribute_declaration -> tkSquareOpen, one_or_some_attribute, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 236: // one_or_some_attribute -> one_attribute
{ 
			CurrentSemanticValue.stn = new simple_attribute_list(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 237: // one_or_some_attribute -> one_or_some_attribute, tkComma, one_attribute
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as simple_attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 238: // one_attribute -> attribute_variable
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 239: // one_attribute -> identifier, tkColon, attribute_variable
{  
			(ValueStack[ValueStack.Depth-1].stn as attribute).qualifier = ValueStack[ValueStack.Depth-3].id;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 240: // simple_type_decl -> type_decl_identifier, tkEqual, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-4].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 241: // simple_type_decl -> template_identifier_with_equal, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 242: // type_decl_identifier -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 243: // type_decl_identifier -> identifier, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 244: // template_identifier_with_equal -> identifier, tkLower, ident_list, 
                //                                   tkGreaterEqual
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-4].id.name, ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 245: // type_decl_type -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 246: // type_decl_type -> object_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 247: // simple_type_question -> simple_type, tkQuestion
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
      case 248: // simple_type_question -> template_type, tkQuestion
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
      case 249: // type_ref -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 250: // type_ref -> simple_type_question
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 251: // type_ref -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 252: // type_ref -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 253: // type_ref -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 254: // type_ref -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 255: // type_ref -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 256: // template_type -> simple_type_identifier, template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference(ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan); 
		}
        break;
      case 257: // template_type_params -> tkLower, template_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 258: // template_type_empty_params -> tkNotEqual
{
            var ntr = new named_type_reference(new ident(""), CurrentLocationSpan);
            
			CurrentSemanticValue.stn = new template_param_list(ntr, CurrentLocationSpan);
            ntr.source_context = new SourceContext(CurrentSemanticValue.stn.source_context.end_position.line_num, CurrentSemanticValue.stn.source_context.end_position.column_num, CurrentSemanticValue.stn.source_context.begin_position.line_num, CurrentSemanticValue.stn.source_context.begin_position.column_num);
		}
        break;
      case 259: // template_type_empty_params -> tkLower, template_empty_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 260: // template_param_list -> template_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 261: // template_param_list -> template_param_list, tkComma, template_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 262: // template_empty_param_list -> template_empty_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 263: // template_empty_param_list -> template_empty_param_list, tkComma, 
                //                              template_empty_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 264: // template_empty_param -> /* empty */
{ 
            CurrentSemanticValue.td = new named_type_reference(new ident(""), CurrentLocationSpan);
        }
        break;
      case 265: // template_param -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 266: // template_param -> simple_type, tkQuestion
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
      case 267: // template_param -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 268: // template_param -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 269: // template_param -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 270: // simple_type -> range_expr
{
	    	CurrentSemanticValue.td = parserTools.ConvertDotNodeOrIdentToNamedTypeReference(ValueStack[ValueStack.Depth-1].ex); 
	    }
        break;
      case 271: // simple_type -> range_expr, tkDotDot, range_expr
{ 
			CurrentSemanticValue.td = new diapason(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 272: // simple_type -> tkRoundOpen, enumeration_id_list, tkRoundClose
{ 
			CurrentSemanticValue.td = new enum_type_definition(ValueStack[ValueStack.Depth-2].stn as enumerator_list, CurrentLocationSpan);  
		}
        break;
      case 273: // range_expr -> range_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 274: // range_expr -> range_expr, const_addop, range_term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 275: // range_term -> range_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 276: // range_term -> range_term, const_mulop, range_factor
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 277: // range_factor -> simple_type_identifier
{ 
			CurrentSemanticValue.ex = parserTools.ConvertNamedTypeReferenceToDotNodeOrIdent(ValueStack[ValueStack.Depth-1].td as named_type_reference);
        }
        break;
      case 278: // range_factor -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 279: // range_factor -> sign, range_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 280: // range_factor -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 281: // range_factor -> range_factor, tkRoundOpen, const_elem_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value, ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 282: // simple_type_identifier -> identifier
{ 
			CurrentSemanticValue.td = new named_type_reference(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 283: // simple_type_identifier -> simple_type_identifier, tkPoint, 
                //                           identifier_or_keyword
{ 
			CurrentSemanticValue.td = (ValueStack[ValueStack.Depth-3].td as named_type_reference).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 284: // enumeration_id_list -> enumeration_id
{ 
			CurrentSemanticValue.stn = new enumerator_list(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 285: // enumeration_id_list -> enumeration_id_list, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 286: // enumeration_id -> type_ref
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan); 
		}
        break;
      case 287: // enumeration_id -> type_ref, tkEqual, expr
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 288: // pointer_type -> tkDeref, fptype
{ 
			CurrentSemanticValue.td = new ref_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 289: // structured_type -> array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 290: // structured_type -> record_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 291: // structured_type -> set_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 292: // structured_type -> file_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 293: // structured_type -> sequence_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 294: // sequence_type -> tkSequence, tkOf, type_ref
{
			CurrentSemanticValue.td = new sequence_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 295: // array_type -> tkArray, tkSquareOpen, simple_type_list, tkSquareClose, tkOf, 
                //               type_ref
{ 
			CurrentSemanticValue.td = new array_type(ValueStack[ValueStack.Depth-4].stn as indexers_types, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 296: // array_type -> unsized_array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 297: // unsized_array_type -> tkArray, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new array_type(null, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 298: // simple_type_list -> simple_type_or_
{ 
			CurrentSemanticValue.stn = new indexers_types(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 299: // simple_type_list -> simple_type_list, tkComma, simple_type_or_
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as indexers_types).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 300: // simple_type_or_ -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 301: // simple_type_or_ -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 302: // set_type -> tkSet, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new set_type_definition(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 303: // file_type -> tkFile, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new file_type(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 304: // file_type -> tkFile
{ 
			CurrentSemanticValue.td = new file_type();  
			CurrentSemanticValue.td.source_context = CurrentLocationSpan;
		}
        break;
      case 305: // string_type -> tkIdentifier, tkSquareOpen, const_expr, tkSquareClose
{ 
			CurrentSemanticValue.td = new string_num_definition(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-4].id, CurrentLocationSpan);
		}
        break;
      case 306: // procedural_type -> procedural_type_kind
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 307: // procedural_type_kind -> proc_type_decl
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 308: // proc_type_decl -> tkProcedure, fp_list
{ 
			CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-1].stn as formal_parameters,null,null,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 309: // proc_type_decl -> tkFunction, fp_list, tkColon, fptype
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, null, null, null, ValueStack[ValueStack.Depth-1].td as type_definition, CurrentLocationSpan);
        }
        break;
      case 310: // proc_type_decl -> simple_type_identifier, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 311: // proc_type_decl -> template_type, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 312: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 313: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-4].stn as enumerator_list,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 314: // proc_type_decl -> simple_type_identifier, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 315: // proc_type_decl -> template_type, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 316: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,null,CurrentLocationSpan);
    	}
        break;
      case 317: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-5].stn as enumerator_list,null,CurrentLocationSpan);
    	}
        break;
      case 318: // object_type -> class_attributes, class_or_interface_keyword, 
                //                optional_base_classes, optional_where_section, 
                //                optional_component_list_seq_end
{ 
            var cd = NewObjectType((class_attribute)ValueStack[ValueStack.Depth-5].ob, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as where_definition_list, ValueStack[ValueStack.Depth-1].stn as class_body_list, CurrentLocationSpan); 
			CurrentSemanticValue.td = cd;
		}
        break;
      case 319: // record_type -> tkRecord, optional_base_classes, optional_where_section, 
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
      case 320: // class_attribute -> tkSealed
{ CurrentSemanticValue.ob = class_attribute.Sealed; }
        break;
      case 321: // class_attribute -> tkPartial
{ CurrentSemanticValue.ob = class_attribute.Partial; }
        break;
      case 322: // class_attribute -> tkAbstract
{ CurrentSemanticValue.ob = class_attribute.Abstract; }
        break;
      case 323: // class_attribute -> tkAuto
{ CurrentSemanticValue.ob = class_attribute.Auto; }
        break;
      case 324: // class_attribute -> tkStatic
{ CurrentSemanticValue.ob = class_attribute.Static; }
        break;
      case 325: // class_attributes -> /* empty */
{ 
			CurrentSemanticValue.ob = class_attribute.None; 
		}
        break;
      case 326: // class_attributes -> class_attributes1
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 327: // class_attributes1 -> class_attribute
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 328: // class_attributes1 -> class_attributes1, class_attribute
{
            if (((class_attribute)ValueStack[ValueStack.Depth-2].ob & (class_attribute)ValueStack[ValueStack.Depth-1].ob) == (class_attribute)ValueStack[ValueStack.Depth-1].ob)
                parserTools.AddErrorFromResource("ATTRIBUTE_REDECLARED",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.ob  = ((class_attribute)ValueStack[ValueStack.Depth-2].ob) | ((class_attribute)ValueStack[ValueStack.Depth-1].ob);
			//$$ = $1;
		}
        break;
      case 329: // class_or_interface_keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 330: // class_or_interface_keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 331: // class_or_interface_keyword -> tkTemplate
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-1].ti);
		}
        break;
      case 332: // class_or_interface_keyword -> tkTemplate, tkClass
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "c", CurrentLocationSpan);
		}
        break;
      case 333: // class_or_interface_keyword -> tkTemplate, tkRecord
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "r", CurrentLocationSpan);
		}
        break;
      case 334: // class_or_interface_keyword -> tkTemplate, tkInterface
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "i", CurrentLocationSpan);
		}
        break;
      case 335: // optional_component_list_seq_end -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 336: // optional_component_list_seq_end -> member_list_section, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 338: // optional_base_classes -> tkRoundOpen, base_classes_names_list, tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 339: // base_classes_names_list -> base_class_name
{ 
			CurrentSemanticValue.stn = new named_type_reference_list(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 340: // base_classes_names_list -> base_classes_names_list, tkComma, base_class_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as named_type_reference_list).Add(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 341: // base_class_name -> simple_type_identifier
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 342: // base_class_name -> template_type
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 343: // template_arguments -> tkLower, ident_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 344: // optional_where_section -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 345: // optional_where_section -> where_part_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 346: // where_part_list -> where_part
{ 
			CurrentSemanticValue.stn = new where_definition_list(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 347: // where_part_list -> where_part_list, where_part
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as where_definition_list).Add(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 348: // where_part -> tkWhere, ident_list, tkColon, type_ref_and_secific_list, 
                //               tkSemiColon
{ 
			CurrentSemanticValue.stn = new where_definition(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-2].stn as where_type_specificator_list, CurrentLocationSpan); 
		}
        break;
      case 349: // type_ref_and_secific_list -> type_ref_or_secific
{ 
			CurrentSemanticValue.stn = new where_type_specificator_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 350: // type_ref_and_secific_list -> type_ref_and_secific_list, tkComma, 
                //                              type_ref_or_secific
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as where_type_specificator_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 351: // type_ref_or_secific -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 352: // type_ref_or_secific -> tkClass
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefClass, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 353: // type_ref_or_secific -> tkRecord
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 354: // type_ref_or_secific -> tkConstructor
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 355: // member_list_section -> member_list
{ 
			CurrentSemanticValue.stn = new class_body_list(ValueStack[ValueStack.Depth-1].stn as class_members, CurrentLocationSpan);
        }
        break;
      case 356: // member_list_section -> member_list_section, ot_visibility_specifier, 
                //                        member_list
{ 
		    (ValueStack[ValueStack.Depth-1].stn as class_members).access_mod = ValueStack[ValueStack.Depth-2].stn as access_modifer_node;
			(ValueStack[ValueStack.Depth-3].stn as class_body_list).Add(ValueStack[ValueStack.Depth-1].stn as class_members,CurrentLocationSpan);
			
			if ((ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks[0].Count == 0)
                (ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks.RemoveAt(0);
			
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 357: // ot_visibility_specifier -> tkInternal
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.internal_modifer, CurrentLocationSpan); }
        break;
      case 358: // ot_visibility_specifier -> tkPublic
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.public_modifer, CurrentLocationSpan); }
        break;
      case 359: // ot_visibility_specifier -> tkProtected
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.protected_modifer, CurrentLocationSpan); }
        break;
      case 360: // ot_visibility_specifier -> tkPrivate
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.private_modifer, CurrentLocationSpan); }
        break;
      case 361: // member_list -> /* empty */
{ CurrentSemanticValue.stn = new class_members(); }
        break;
      case 362: // member_list -> field_or_const_definition_list, optional_semicolon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 363: // member_list -> method_decl_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 364: // member_list -> field_or_const_definition_list, tkSemiColon, method_decl_list
{  
			(ValueStack[ValueStack.Depth-3].stn as class_members).members.AddRange((ValueStack[ValueStack.Depth-1].stn as class_members).members);
			(ValueStack[ValueStack.Depth-3].stn as class_members).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 365: // ident_list -> identifier
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 366: // ident_list -> ident_list, tkComma, identifier
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 367: // optional_semicolon -> /* empty */
{ CurrentSemanticValue.ob = null; }
        break;
      case 368: // optional_semicolon -> tkSemiColon
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 369: // field_or_const_definition_list -> field_or_const_definition
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 370: // field_or_const_definition_list -> field_or_const_definition_list, tkSemiColon, 
                //                                   field_or_const_definition
{   
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 371: // field_or_const_definition -> attribute_declarations, 
                //                              simple_field_or_const_definition
{  
		    (ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 372: // method_decl_list -> method_or_property_decl
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 373: // method_decl_list -> method_decl_list, method_or_property_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 374: // method_or_property_decl -> method_decl_withattr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 375: // method_or_property_decl -> property_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 376: // simple_field_or_const_definition -> tkConst, only_const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 377: // simple_field_or_const_definition -> field_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 378: // simple_field_or_const_definition -> class_or_static, field_definition
{ 
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).var_attr = definition_attribute.Static;
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 379: // class_or_static -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 380: // class_or_static -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 381: // field_definition -> var_decl_part
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 382: // field_definition -> tkEvent, ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, true, CurrentLocationSpan); 
        }
        break;
      case 383: // method_decl_withattr -> attribute_declarations, method_header
{  
			(ValueStack[ValueStack.Depth-1].td as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td;
        }
        break;
      case 384: // method_decl_withattr -> attribute_declarations, method_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
            if (ValueStack[ValueStack.Depth-1].stn is procedure_definition && (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
                (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
     }
        break;
      case 385: // method_decl -> inclass_proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 386: // method_decl -> inclass_constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 387: // method_header -> class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 388: // method_header -> method_procfunc_header
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
		}
        break;
      case 389: // method_header -> tkAsync, class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 390: // method_header -> tkAsync, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
		}
        break;
      case 391: // method_header -> constr_destr_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 392: // method_procfunc_header -> proc_func_header
{ 
			CurrentSemanticValue.td = NewProcfuncHeading(ValueStack[ValueStack.Depth-1].td as procedure_header);
		}
        break;
      case 393: // proc_func_header -> proc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 394: // proc_func_header -> func_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 395: // constr_destr_header -> tkConstructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 396: // constr_destr_header -> class_or_static, tkConstructor, optional_proc_name, 
                //                        fp_list, optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,true,null,null,CurrentLocationSpan);
        }
        break;
      case 397: // constr_destr_header -> tkDestructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new destructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name, false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 398: // optional_proc_name -> proc_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 399: // optional_proc_name -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 400: // property_definition -> attribute_declarations, simple_property_definition
{  
			CurrentSemanticValue.stn = NewPropertyDefinition(ValueStack[ValueStack.Depth-2].stn as attribute_list, ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 401: // simple_property_definition -> tkProperty, func_name, property_interface, 
                //                               property_specifiers, tkSemiColon, 
                //                               array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        }
        break;
      case 402: // simple_property_definition -> tkProperty, func_name, property_interface, 
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
      case 403: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        	(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
        }
        break;
      case 404: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, property_modificator, tkSemiColon, 
                //                               array_defaultproperty
{ 
			parserTools.AddErrorFromResource("STATIC_PROPERTIES_CANNOT_HAVE_ATTRBUTE_{0}",LocationStack[LocationStack.Depth-3],ValueStack[ValueStack.Depth-3].id.name);        	
        }
        break;
      case 405: // simple_property_definition -> tkAuto, tkProperty, func_name, property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 406: // simple_property_definition -> class_or_static, tkAuto, tkProperty, func_name, 
                //                               property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 407: // optional_property_initialization -> tkAssign, expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 408: // optional_property_initialization -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 409: // array_defaultproperty -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 410: // array_defaultproperty -> tkDefault, tkSemiColon
{ 
			CurrentSemanticValue.stn = new property_array_default();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 411: // property_interface -> property_parameter_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_interface(ValueStack[ValueStack.Depth-3].stn as property_parameter_list, ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan);
        }
        break;
      case 412: // property_parameter_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 413: // property_parameter_list -> tkSquareOpen, parameter_decl_list, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 414: // parameter_decl_list -> parameter_decl
{ 
			CurrentSemanticValue.stn = new property_parameter_list(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 415: // parameter_decl_list -> parameter_decl_list, tkSemiColon, parameter_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as property_parameter_list).Add(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 416: // parameter_decl -> ident_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_parameter(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 417: // optional_read_expr -> expr_with_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 418: // optional_read_expr -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 420: // property_specifiers -> tkRead, optional_read_expr, write_property_specifiers
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
      case 421: // property_specifiers -> tkWrite, unlabelled_stmt, read_property_specifiers
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
      case 423: // write_property_specifiers -> tkWrite, unlabelled_stmt
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
      case 425: // read_property_specifiers -> tkRead, optional_read_expr
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
      case 426: // var_decl -> var_decl_part, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 429: // var_decl_part -> ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, false, CurrentLocationSpan);
		}
        break;
      case 430: // var_decl_part -> ident_list, tkAssign, expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, null, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan);		
		}
        break;
      case 431: // var_decl_part -> ident_list, tkColon, type_ref, tkAssignOrEqual, 
                //                  typed_var_init_expression
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan); 
		}
        break;
      case 432: // typed_var_init_expression -> typed_const_plus
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 433: // typed_var_init_expression -> const_simple_expr, tkDotDot, const_term
{ 
		if (parserTools.buildTreeForFormatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
		}
        break;
      case 434: // typed_var_init_expression -> expl_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 435: // typed_var_init_expression -> identifier, tkArrow, lambda_function_body
{  
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 436: // typed_var_init_expression -> tkRoundOpen, tkRoundClose, lambda_type_ref, 
                //                              tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 437: // typed_var_init_expression -> tkRoundOpen, typed_const_list, tkRoundClose, 
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
      case 438: // typed_var_init_expression -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 439: // typed_const_plus -> typed_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 440: // constr_destr_decl -> constr_destr_header, block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 441: // constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, tkAssign, 
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
      case 442: // constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
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
      case 443: // inclass_constr_destr_decl -> constr_destr_header, inclass_block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 444: // inclass_constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, 
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
      case 445: // inclass_constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
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
      case 446: // proc_func_decl -> proc_func_decl_without_modifiers
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 447: // proc_func_decl -> class_or_static, proc_func_decl_without_modifiers
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 448: // proc_func_decl -> tkAsync, proc_func_decl_without_modifiers
{
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;		
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 449: // proc_func_decl -> tkAsync, class_or_static, proc_func_decl_without_modifiers
{ 
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 450: // proc_func_decl -> class_or_static, tkAsync, proc_func_decl_without_modifiers
{ 
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 451: // inclass_proc_func_decl -> inclass_proc_func_decl_without_modifiers
{ 
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 452: // inclass_proc_func_decl -> tkAsync, inclass_proc_func_decl_without_modifiers
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 453: // inclass_proc_func_decl -> class_or_static, 
                //                           inclass_proc_func_decl_without_modifiers
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
		    {
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			}
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 454: // inclass_proc_func_decl -> tkAsync, class_or_static, 
                //                           inclass_proc_func_decl_without_modifiers
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
		    {
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			}
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 455: // inclass_proc_func_decl -> class_or_static, tkAsync, 
                //                           inclass_proc_func_decl_without_modifiers
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
		    {
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			}
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 456: // proc_func_decl_without_modifiers -> proc_func_header, proc_func_external_block
{
            var ph = ValueStack[ValueStack.Depth-2].td as procedure_header;
            var pal = ph.proc_attributes;
			var mn = ph.name;
			if (mn.meth_name is operator_name_ident && mn.class_name == null && 
                !pal.proc_attributes.Select(p => p.attribute_type).Contains(proc_attribute.attr_extension))
		    {
                pal.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_extension));
            }
            CurrentSemanticValue.stn = new procedure_definition(ph, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
        }
        break;
      case 457: // proc_func_decl_without_modifiers -> proc_func_header, tkForward, tkSemiColon
{
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-3].td as procedure_header, null, CurrentLocationSpan);
            (CurrentSemanticValue.stn as procedure_definition).proc_header.proc_attributes.Add((procedure_attribute)ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-2].id.source_context);
		}
        break;
      case 458: // proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, tkColon, 
                //                                     fptype, optional_method_modificators1, 
                //                                     tkAssign, expr_l1_func_decl_lambda, 
                //                                     tkSemiColon
{
		    var pal = ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list;
		    var mn = ValueStack[ValueStack.Depth-8].stn as method_name;
		    if (mn.meth_name is operator_name_ident && mn.class_name == null &&
                !pal.proc_attributes.Select(p => p.attribute_type).Contains(proc_attribute.attr_extension))
		    {
                pal.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_extension));
            }
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, pal, mn, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 459: // proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, 
                //                                     optional_method_modificators1, tkAssign, 
                //                                     expr_l1_func_decl_lambda, tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].ex is dot_question_node)
				parserTools.AddErrorFromResource("DOT_QUECTION_IN_SHORT_FUN",LocationStack[LocationStack.Depth-2]);

		    var pal = ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list;
		    var mn = ValueStack[ValueStack.Depth-6].stn as method_name;
		    if (mn.meth_name is operator_name_ident && mn.class_name == null &&
                !pal.proc_attributes.Select(p => p.attribute_type).Contains(proc_attribute.attr_extension))
		    {
                pal.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_extension));
            }

			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, pal, mn, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 460: // proc_func_decl_without_modifiers -> tkProcedure, proc_name, fp_list, 
                //                                     optional_method_modificators1, tkAssign, 
                //                                     unlabelled_stmt, tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-2]);
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 461: // inclass_proc_func_decl_without_modifiers -> proc_func_header, inclass_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
		}
        break;
      case 462: // inclass_proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, 
                //                                             tkColon, fptype, 
                //                                             optional_method_modificators1, 
                //                                             tkAssign, 
                //                                             expr_l1_func_decl_lambda, 
                //                                             tkSemiColon
{
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-7].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-8].stn as method_name, ValueStack[ValueStack.Depth-5].td as type_definition, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-9].Merge(LocationStack[LocationStack.Depth-4]));
			if (parserTools.buildTreeForFormatter) // э�?о �?ол�?ко в inclass надо писа�?�? - в об�?�?ном proc_func_decl_without_modifiers - не надо! �?о�?ем�? - �?же �?�?�?а�?ено
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 463: // inclass_proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, 
                //                                             optional_method_modificators1, 
                //                                             tkAssign, 
                //                                             expr_l1_func_decl_lambda, 
                //                                             tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].ex is dot_question_node)
				parserTools.AddErrorFromResource("DOT_QUECTION_IN_SHORT_FUN",LocationStack[LocationStack.Depth-2]);
	
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortFuncDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, null, ValueStack[ValueStack.Depth-2].ex, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parserTools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 464: // inclass_proc_func_decl_without_modifiers -> tkProcedure, proc_name, fp_list, 
                //                                             optional_method_modificators1, 
                //                                             tkAssign, unlabelled_stmt, 
                //                                             tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-2]);

            CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
			if (parserTools.buildTreeForFormatter)
				CurrentSemanticValue.stn = new short_func_definition(CurrentSemanticValue.stn as procedure_definition);
		}
        break;
      case 465: // proc_func_external_block -> block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 466: // proc_func_external_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 467: // proc_name -> func_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 468: // func_name -> func_meth_name_ident
{ 
			CurrentSemanticValue.stn = new method_name(null,null, ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan); 
		}
        break;
      case 469: // func_name -> func_class_name_ident_list, tkPoint, func_meth_name_ident
{ 
        	var ln = ValueStack[ValueStack.Depth-3].ob as List<ident>;
        	var cnt = ln.Count;
        	if (cnt == 1)
				CurrentSemanticValue.stn = new method_name(null, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
			else 	
				CurrentSemanticValue.stn = new method_name(ln, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
		}
        break;
      case 470: // func_class_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 471: // func_class_name_ident_list -> func_class_name_ident
{ 
			CurrentSemanticValue.ob = new List<ident>(); 
			(CurrentSemanticValue.ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
		}
        break;
      case 472: // func_class_name_ident_list -> func_class_name_ident_list, tkPoint, 
                //                               func_class_name_ident
{ 
			(ValueStack[ValueStack.Depth-3].ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob; 
		}
        break;
      case 473: // func_meth_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 474: // func_meth_name_ident -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 475: // func_meth_name_ident -> operator_name_ident, template_arguments
{ CurrentSemanticValue.id = new template_operator_name(null, ValueStack[ValueStack.Depth-1].stn as ident_list, ValueStack[ValueStack.Depth-2].ex as operator_name_ident, CurrentLocationSpan); }
        break;
      case 476: // func_name_with_template_args -> func_name_ident
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 477: // func_name_with_template_args -> func_name_ident, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 478: // func_name_ident -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 479: // proc_header -> tkProcedure, proc_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{ 
        	CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, CurrentLocationSpan); 
        }
        break;
      case 480: // func_header -> tkFunction, func_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, null, CurrentLocationSpan); 
		}
        break;
      case 481: // func_header -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                optional_method_modificators, optional_where_section
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, ValueStack[ValueStack.Depth-3].td as type_definition, CurrentLocationSpan); 
        }
        break;
      case 482: // external_block -> tkExternal, external_directive_ident, tkName, 
                //                   external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 483: // external_block -> tkExternal, external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-2].ex, null, CurrentLocationSpan); 
		}
        break;
      case 484: // external_block -> tkExternal, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(null, null, CurrentLocationSpan); 
		}
        break;
      case 485: // external_directive_ident -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 486: // external_directive_ident -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 487: // block -> decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 488: // inclass_block -> inclass_decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 489: // inclass_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 490: // fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 491: // fp_list -> tkRoundOpen, tkRoundClose
{ 
			CurrentSemanticValue.stn = null;
		}
        break;
      case 492: // fp_list -> tkRoundOpen, fp_sect_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			if (CurrentSemanticValue.stn != null)
				CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 493: // fp_sect_list -> fp_sect
{ 
			CurrentSemanticValue.stn = new formal_parameters(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
        }
        break;
      case 494: // fp_sect_list -> fp_sect_list, tkSemiColon, fp_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);   
        }
        break;
      case 495: // fp_sect -> attribute_declarations, simple_fp_sect
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as  attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 496: // simple_fp_sect -> param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan); 
		}
        break;
      case 497: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.var_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 498: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.const_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 499: // simple_fp_sect -> tkParams, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td,parametr_kind.params_parametr,null, CurrentLocationSpan);  
		}
        break;
      case 500: // simple_fp_sect -> param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.none, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 501: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.var_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 502: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.const_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 503: // param_name_list -> param_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 504: // param_name_list -> param_name_list, tkComma, param_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);  
		}
        break;
      case 505: // param_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 506: // fptype -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 507: // fptype_noproctype -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 508: // fptype_noproctype -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 509: // fptype_noproctype -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 510: // fptype_noproctype -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 511: // fptype_noproctype -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 512: // stmt -> unlabelled_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 513: // stmt -> label_name, tkColon, stmt
{ 
			CurrentSemanticValue.stn = new labeled_statement(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);  
		}
        break;
      case 514: // unlabelled_stmt -> /* empty */
{ 
			CurrentSemanticValue.stn = new empty_statement(); 
			CurrentSemanticValue.stn.source_context = null;
		}
        break;
      case 515: // unlabelled_stmt -> assignment
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 516: // unlabelled_stmt -> proc_call
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 517: // unlabelled_stmt -> goto_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 518: // unlabelled_stmt -> compound_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 519: // unlabelled_stmt -> if_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 520: // unlabelled_stmt -> case_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 521: // unlabelled_stmt -> repeat_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 522: // unlabelled_stmt -> while_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 523: // unlabelled_stmt -> for_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 524: // unlabelled_stmt -> with_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 525: // unlabelled_stmt -> inherited_message
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 526: // unlabelled_stmt -> try_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 527: // unlabelled_stmt -> raise_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 528: // unlabelled_stmt -> foreach_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 529: // unlabelled_stmt -> var_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 530: // unlabelled_stmt -> expr_as_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 531: // unlabelled_stmt -> lock_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 532: // unlabelled_stmt -> yield_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 533: // unlabelled_stmt -> yield_sequence_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 534: // unlabelled_stmt -> loop_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 535: // unlabelled_stmt -> match_with
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 536: // loop_stmt -> tkLoop, expr_l1, tkDo, unlabelled_stmt
{
			CurrentSemanticValue.stn = new loop_stmt(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].stn as statement,CurrentLocationSpan);
		}
        break;
      case 537: // yield_stmt -> tkYield, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 538: // yield_sequence_stmt -> tkYield, tkSequence, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_sequence_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 539: // var_stmt -> tkVar, var_decl_part
{ 
			CurrentSemanticValue.stn = new var_statement(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 540: // var_stmt -> tkRoundOpen, tkVar, identifier, tkComma, var_ident_list, 
                //             tkRoundClose, tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].ob as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].ob as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 541: // var_stmt -> tkVar, tkRoundOpen, identifier, tkComma, ident_list, tkRoundClose, 
                //             tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			ValueStack[ValueStack.Depth-4].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
	    }
        break;
      case 542: // assignment -> var_reference, assign_operator, expr_with_func_decl_lambda
{      
        	if (!(ValueStack[ValueStack.Depth-3].ex is addressed_value))
        		parserTools.AddErrorFromResource("LEFT_SIDE_CANNOT_BE_ASSIGNED_TO",CurrentLocationSpan);
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
        }
        break;
      case 543: // assignment -> tkRoundOpen, variable, tkComma, variable_list, tkRoundClose, 
                //               assign_operator, expr
{
			if (ValueStack[ValueStack.Depth-2].op.type != Operators.Assignment)
			    parserTools.AddErrorFromResource("ONLY_BASE_ASSIGNMENT_FOR_TUPLE",LocationStack[LocationStack.Depth-2]);
			(ValueStack[ValueStack.Depth-4].ob as addressed_value_list).Insert(0,ValueStack[ValueStack.Depth-6].ex as addressed_value);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_tuple(ValueStack[ValueStack.Depth-4].ob as addressed_value_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 544: // variable_list -> variable
{
		CurrentSemanticValue.ob = new addressed_value_list(ValueStack[ValueStack.Depth-1].ex as addressed_value,LocationStack[LocationStack.Depth-1]);
	}
        break;
      case 545: // variable_list -> variable_list, tkComma, variable
{
		(ValueStack[ValueStack.Depth-3].ob as addressed_value_list).Add(ValueStack[ValueStack.Depth-1].ex as addressed_value);
		(ValueStack[ValueStack.Depth-3].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
	}
        break;
      case 546: // var_ident_list -> tkVar, identifier
{
		CurrentSemanticValue.ob = new ident_list(ValueStack[ValueStack.Depth-1].id,CurrentLocationSpan);
	}
        break;
      case 547: // var_ident_list -> var_ident_list, tkComma, tkVar, identifier
{
		(ValueStack[ValueStack.Depth-4].ob as ident_list).Add(ValueStack[ValueStack.Depth-1].id);
		(ValueStack[ValueStack.Depth-4].ob as ident_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-4].ob;
	}
        break;
      case 548: // proc_call -> var_reference
{ 
			CurrentSemanticValue.stn = new procedure_call(ValueStack[ValueStack.Depth-1].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex is ident, CurrentLocationSpan); 
		}
        break;
      case 549: // goto_stmt -> tkGoto, label_name
{ 
			CurrentSemanticValue.stn = new goto_statement(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 550: // compound_stmt -> tkBegin, stmt_list, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			(CurrentSemanticValue.stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(CurrentSemanticValue.stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 551: // stmt_list -> stmt
{ 
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 552: // stmt_list -> stmt_list, tkSemiColon, stmt
{  
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as statement_list).Add(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 553: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan); 
        }
        break;
      case 554: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt, tkElse, unlabelled_stmt
{
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as statement, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 555: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, else_case, tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 556: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, tkSemiColon, else_case, 
                //               tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 557: // pattern_cases -> pattern_case
{
            CurrentSemanticValue.stn = new pattern_cases(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 558: // pattern_cases -> pattern_cases, tkSemiColon, pattern_case
{
            CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as pattern_cases).Add(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 559: // pattern_case -> pattern_optional_var, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 560: // pattern_case -> deconstruction_or_const_pattern, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 561: // pattern_case -> collection_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 562: // pattern_case -> tuple_pattern, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
		}
        break;
      case 563: // pattern_case -> tuple_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 564: // case_stmt -> tkCase, expr_l1, tkOf, case_list, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 565: // case_stmt -> tkCase, expr_l1, tkOf, case_list, tkSemiColon, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 566: // case_stmt -> tkCase, expr_l1, tkOf, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-4].ex, NewCaseItem(new empty_statement(), null), ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 567: // case_list -> case_item
{
			if (ValueStack[ValueStack.Depth-1].stn is empty_statement) 
				CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, null);
			else CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 568: // case_list -> case_list, tkSemiColon, case_item
{ 
			CurrentSemanticValue.stn = AddCaseItem(ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 569: // case_item -> case_label_list, tkColon, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new case_variant(ValueStack[ValueStack.Depth-3].stn as expression_list, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 570: // case_label_list -> case_label
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 571: // case_label_list -> case_label_list, tkComma, case_label
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 572: // case_label -> const_elem
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 573: // else_case -> /* empty */
{ CurrentSemanticValue.stn = null;}
        break;
      case 574: // else_case -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 575: // repeat_stmt -> tkRepeat, stmt_list, tkUntil, expr
{ 
		    CurrentSemanticValue.stn = new repeat_node(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-3].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-4].ti;
			(ValueStack[ValueStack.Depth-3].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-2].ti;
			ValueStack[ValueStack.Depth-3].stn.source_context = LocationStack[LocationStack.Depth-4].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 576: // while_stmt -> tkWhile, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewWhileStmt(ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);    
        }
        break;
      case 577: // optional_tk_do -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 578: // optional_tk_do -> /* empty */
{ CurrentSemanticValue.ti = null; }
        break;
      case 579: // lock_stmt -> tkLock, expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new lock_stmt(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 580: // index_or_nothing -> tkIndex, tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 581: // index_or_nothing -> /* empty */
{ CurrentSemanticValue.id = null; }
        break;
      case 582: // optional_type_specification -> tkColon, type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 583: // optional_type_specification -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 584: // optional_var -> tkVar
{ CurrentSemanticValue.ob = true; }
        break;
      case 585: // optional_var -> /* empty */
{ CurrentSemanticValue.ob = false; }
        break;
      case 586: // for_cycle_type -> tkTo
{ CurrentSemanticValue.ob = for_cycle_type.to; }
        break;
      case 587: // for_cycle_type -> tkDownto
{ CurrentSemanticValue.ob = for_cycle_type.downto; }
        break;
      case 588: // foreach_stmt -> tkForeach, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
            if (ValueStack[ValueStack.Depth-6].td == null)
                parserTools.AddWarningFromResource("USING_UNLOCAL_FOREACH_VARIABLE", ValueStack[ValueStack.Depth-7].id.source_context);
        }
        break;
      case 589: // foreach_stmt -> tkForeach, tkVar, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
        	if (ValueStack[ValueStack.Depth-6].td == null)
				CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, new no_type_foreach(), ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
			else CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
        }
        break;
      case 590: // foreach_stmt -> tkForeach, tkVar, tkRoundOpen, ident_list, tkRoundClose, tkIn, 
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
      case 591: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, optional_tk_do, 
                //             unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-9].ob, ValueStack[ValueStack.Depth-8].id, ValueStack[ValueStack.Depth-7].td, ValueStack[ValueStack.Depth-5].ex, (for_cycle_type)ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 592: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, tkStep, expr_l1, tkDo, 
                //             unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-11].ob, ValueStack[ValueStack.Depth-10].id, ValueStack[ValueStack.Depth-9].td, ValueStack[ValueStack.Depth-7].ex, (for_cycle_type)ValueStack[ValueStack.Depth-6].ob, ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 593: // with_stmt -> tkWith, expr_list, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new with_statement(ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 594: // inherited_message -> tkInherited
{ 
			CurrentSemanticValue.stn = new inherited_message();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 595: // try_stmt -> tkTry, stmt_list, try_handler
{ 
			CurrentSemanticValue.stn = new try_stmt(ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].stn as try_handler, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			ValueStack[ValueStack.Depth-2].stn.source_context = LocationStack[LocationStack.Depth-3].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 596: // try_handler -> tkFinally, stmt_list, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_finally(ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan);
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(ValueStack[ValueStack.Depth-2].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
		}
        break;
      case 597: // try_handler -> tkExcept, exception_block, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_except((exception_block)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan);  
			if ((ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list != null)
			{
				(ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list.source_context = CurrentLocationSpan;
				(ValueStack[ValueStack.Depth-2].stn as exception_block).source_context = CurrentLocationSpan;
			}
		}
        break;
      case 598: // exception_block -> exception_handler_list, exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-2].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 599: // exception_block -> exception_handler_list, tkSemiColon, 
                //                    exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-3].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 600: // exception_block -> stmt_list
{ 
			CurrentSemanticValue.stn = new exception_block(ValueStack[ValueStack.Depth-1].stn as statement_list, null, null, LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 601: // exception_handler_list -> exception_handler
{ 
			CurrentSemanticValue.stn = new exception_handler_list(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 602: // exception_handler_list -> exception_handler_list, tkSemiColon, 
                //                           exception_handler
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as exception_handler_list).Add(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 603: // exception_block_else_branch -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 604: // exception_block_else_branch -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 605: // exception_handler -> tkOn, exception_identifier, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new exception_handler((ValueStack[ValueStack.Depth-3].stn as exception_ident).variable, (ValueStack[ValueStack.Depth-3].stn as exception_ident).type_name, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 606: // exception_identifier -> exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(null, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 607: // exception_identifier -> exception_variable, tkColon, 
                //                         exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(ValueStack[ValueStack.Depth-3].id, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 608: // exception_class_type_identifier -> simple_type_identifier
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 609: // exception_variable -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 610: // raise_stmt -> tkRaise
{ 
			CurrentSemanticValue.stn = new raise_stmt(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 611: // raise_stmt -> tkRaise, expr
{ 
			CurrentSemanticValue.stn = new raise_stmt(ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);  
		}
        break;
      case 612: // expr_list -> expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 613: // expr_list -> expr_list, tkComma, expr_with_func_decl_lambda
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 614: // expr_list_func_param -> expr_with_func_decl_lambda_ass
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 615: // expr_list_func_param -> expr_list_func_param, tkComma, 
                //                         expr_with_func_decl_lambda_ass
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 616: // expr_as_stmt -> allowable_expr_as_stmt
{ 
			CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 617: // allowable_expr_as_stmt -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 618: // expr_with_func_decl_lambda -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 619: // expr_with_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 620: // expr_with_func_decl_lambda -> tkInherited
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 621: // expr_with_func_decl_lambda_ass -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 622: // expr_with_func_decl_lambda_ass -> identifier, tkAssign, expr_l1
{ CurrentSemanticValue.ex = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); }
        break;
      case 623: // expr_with_func_decl_lambda_ass -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 624: // expr_with_func_decl_lambda_ass -> tkInherited
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 625: // expr -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 626: // expr -> format_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 627: // expr -> expr, tkTo, expr_l1
{ CurrentSemanticValue.ex = new to_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 628: // expr_l1 -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 629: // expr_l1 -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 630: // expr_l1 -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 631: // expr_l1_for_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 632: // expr_l1_for_question_expr -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 633: // expr_l1_for_new_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 634: // expr_l1_for_new_question_expr -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 635: // expr_l1_func_decl_lambda -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 636: // expr_l1_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 637: // expr_l1_for_lambda -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 638: // expr_l1_for_lambda -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 639: // expr_l1_for_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 640: // expr_dq -> relop_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 641: // expr_dq -> expr_dq, tkDoubleQuestion, relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 642: // sizeof_expr -> tkSizeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new sizeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, null, CurrentLocationSpan);  
		}
        break;
      case 643: // typeof_expr -> tkTypeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 644: // typeof_expr -> tkTypeOf, tkRoundOpen, empty_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 645: // question_expr -> expr_l1_for_question_expr, tkQuestion, 
                //                  expr_l1_for_question_expr, tkColon, 
                //                  expr_l1_for_question_expr
{ 
            if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
            	parserTools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 646: // new_question_expr -> tkIf, expr_l1_for_new_question_expr, tkThen, 
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
      case 647: // empty_template_type_reference -> simple_type_identifier, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 648: // empty_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
        }
        break;
      case 649: // simple_or_template_type_reference -> simple_type_identifier
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 650: // simple_or_template_type_reference -> simple_type_identifier, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 651: // simple_or_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 652: // simple_or_template_or_question_type_reference -> 
                //                                                  simple_or_template_type_reference
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 653: // simple_or_template_or_question_type_reference -> simple_type_question
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 654: // optional_array_initializer -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = new array_const((expression_list)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan); 
		}
        break;
      case 656: // new_expr -> tkNew, simple_or_template_type_reference, 
                //             optional_expr_list_func_param_with_bracket
{
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-2].td, ValueStack[ValueStack.Depth-1].stn as expression_list, false, null, CurrentLocationSpan);
        }
        break;
      case 657: // new_expr -> tkNew, simple_or_template_type_reference, tkSquareOpen, 
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
      case 658: // new_expr -> tkNew, tkClass, tkRoundOpen, list_fields_in_unnamed_object, 
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
      case 659: // field_in_unnamed_object -> identifier, tkAssign, expr_l1
{
		    if (ValueStack[ValueStack.Depth-1].ex is nil_const)
				parserTools.AddErrorFromResource("NIL_IN_UNNAMED_OBJECT",CurrentLocationSpan);		    
			CurrentSemanticValue.ob = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 660: // field_in_unnamed_object -> expr_l1
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
      case 661: // list_fields_in_unnamed_object -> field_in_unnamed_object
{
			var l = new name_assign_expr_list();
			CurrentSemanticValue.ob = l.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
		}
        break;
      case 662: // list_fields_in_unnamed_object -> list_fields_in_unnamed_object, tkComma, 
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
      case 663: // optional_expr_list_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 664: // optional_expr_list_with_bracket -> tkRoundOpen, optional_expr_list, 
                //                                    tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 665: // optional_expr_list_func_param_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 666: // optional_expr_list_func_param_with_bracket -> tkRoundOpen, 
                //                                               optional_expr_list_func_param, 
                //                                               tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 667: // relop_expr -> simple_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 668: // relop_expr -> relop_expr, relop, simple_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 669: // relop_expr -> relop_expr, relop, new_question_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 670: // relop_expr -> is_type_expr, tkRoundOpen, pattern_out_param_list, tkRoundClose
{
            var isTypeCheck = ValueStack[ValueStack.Depth-4].ex as typecast_node;
            var deconstructorPattern = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, isTypeCheck.type_def, null, CurrentLocationSpan); 
            CurrentSemanticValue.ex = new is_pattern_expr(isTypeCheck.expr, deconstructorPattern, CurrentLocationSpan);
        }
        break;
      case 671: // pattern -> simple_or_template_type_reference, tkRoundOpen, 
                //            pattern_out_param_list, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 672: // pattern_optional_var -> simple_or_template_type_reference, tkRoundOpen, 
                //                         pattern_out_param_list_optional_var, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 673: // deconstruction_or_const_pattern -> simple_or_template_type_reference, 
                //                                    tkRoundOpen, 
                //                                    pattern_out_param_list_optional_var, 
                //                                    tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 674: // deconstruction_or_const_pattern -> const_pattern_expr_list
{
		    CurrentSemanticValue.stn = new const_pattern(ValueStack[ValueStack.Depth-1].ob as List<syntax_tree_node>, CurrentLocationSpan); 
		}
        break;
      case 675: // const_pattern_expr_list -> const_pattern_expression
{ 
			CurrentSemanticValue.ob = new List<syntax_tree_node>(); 
			(CurrentSemanticValue.ob as List<syntax_tree_node>).Add(ValueStack[ValueStack.Depth-1].stn);
		}
        break;
      case 676: // const_pattern_expr_list -> const_pattern_expr_list, tkComma, 
                //                            const_pattern_expression
{ 
			var list = ValueStack[ValueStack.Depth-3].ob as List<syntax_tree_node>;
            list.Add(ValueStack[ValueStack.Depth-1].stn);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 677: // const_pattern_expression -> literal_or_number
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 678: // const_pattern_expression -> simple_or_template_type_reference
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 679: // const_pattern_expression -> tkNil
{ 
			CurrentSemanticValue.stn = new nil_const();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 680: // const_pattern_expression -> sizeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 681: // const_pattern_expression -> typeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 682: // collection_pattern -> tkSquareOpen, collection_pattern_expr_list, tkSquareClose
{
			CurrentSemanticValue.stn = new collection_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 683: // collection_pattern_expr_list -> collection_pattern_list_item
{
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 684: // collection_pattern_expr_list -> collection_pattern_expr_list, tkComma, 
                //                                 collection_pattern_list_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 685: // collection_pattern_list_item -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 686: // collection_pattern_list_item -> collection_pattern_var_item
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 687: // collection_pattern_list_item -> tkUnderscore
{
			CurrentSemanticValue.stn = new collection_pattern_wild_card(CurrentLocationSpan);
		}
        break;
      case 688: // collection_pattern_list_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 689: // collection_pattern_list_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 690: // collection_pattern_list_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 691: // collection_pattern_list_item -> tkDotDot
{
			CurrentSemanticValue.stn = new collection_pattern_gap_parameter(CurrentLocationSpan);
		}
        break;
      case 692: // collection_pattern_var_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new collection_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 693: // tuple_pattern -> tkRoundOpen, tuple_pattern_item_list, tkRoundClose
{
			if ((ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>).Count>6) 
				parserTools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
			CurrentSemanticValue.stn = new tuple_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 694: // tuple_pattern_item -> tkUnderscore
{ 
			CurrentSemanticValue.stn = new tuple_pattern_wild_card(CurrentLocationSpan); 
		}
        break;
      case 695: // tuple_pattern_item -> literal_or_number
{ 
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 696: // tuple_pattern_item -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 697: // tuple_pattern_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new tuple_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 698: // tuple_pattern_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 699: // tuple_pattern_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 700: // tuple_pattern_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 701: // tuple_pattern_item_list -> tuple_pattern_item
{ 
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 702: // tuple_pattern_item_list -> tuple_pattern_item_list, tkComma, tuple_pattern_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 703: // pattern_out_param_list_optional_var -> pattern_out_param_optional_var
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 704: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkSemiColon, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 705: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkComma, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 706: // pattern_out_param_list -> pattern_out_param
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 707: // pattern_out_param_list -> pattern_out_param_list, tkSemiColon, 
                //                           pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 708: // pattern_out_param_list -> pattern_out_param_list, tkComma, pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 709: // pattern_out_param -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 710: // pattern_out_param -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 711: // pattern_out_param -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 712: // pattern_out_param -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 713: // pattern_out_param -> pattern
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 714: // pattern_out_param -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 715: // pattern_out_param -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 716: // pattern_out_param_optional_var -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 717: // pattern_out_param_optional_var -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 718: // pattern_out_param_optional_var -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 719: // pattern_out_param_optional_var -> identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, false, CurrentLocationSpan);
        }
        break;
      case 720: // pattern_out_param_optional_var -> identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, false, CurrentLocationSpan);
        }
        break;
      case 721: // pattern_out_param_optional_var -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 722: // pattern_out_param_optional_var -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 723: // pattern_out_param_optional_var -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 724: // pattern_out_param_optional_var -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 725: // pattern_out_param_optional_var -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 726: // simple_expr_or_nothing -> simple_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 727: // simple_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 728: // const_expr_or_nothing -> const_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 729: // const_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 730: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing
{
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 731: // format_expr -> tkColon, simple_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 732: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing, tkColon, 
                //                simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 733: // format_expr -> tkColon, simple_expr_or_nothing, tkColon, simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 734: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 735: // format_const_expr -> tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 736: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing, tkColon, 
                //                      const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 737: // format_const_expr -> tkColon, const_expr_or_nothing, tkColon, const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 738: // relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 739: // relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 740: // relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 741: // relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 742: // relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 743: // relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 744: // relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 745: // relop -> tkNot, tkIn
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
      case 746: // simple_expr -> term1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 747: // simple_expr -> simple_expr, tkDotDot, term1
{ 
		if (parserTools.buildTreeForFormatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
	}
        break;
      case 748: // term1 -> term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 749: // term1 -> term1, addop, term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 750: // term1 -> term1, addop, new_question_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 751: // addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 752: // addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 753: // addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 754: // addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 755: // addop -> tkCSharpStyleOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 756: // typecast_op -> tkAs
{ 
			CurrentSemanticValue.ob = op_typecast.as_op; 
		}
        break;
      case 757: // typecast_op -> tkIs
{ 
			CurrentSemanticValue.ob = op_typecast.is_op; 
		}
        break;
      case 758: // as_is_expr -> is_type_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 759: // as_is_expr -> as_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 760: // as_expr -> term, tkAs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 761: // as_expr -> term, tkAs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 762: // is_type_expr -> term, tkIs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 763: // is_type_expr -> term, tkIs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 764: // power_expr -> factor_without_unary_op, tkStarStar, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 765: // power_expr -> factor_without_unary_op, tkStarStar, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 766: // power_expr -> sign, power_expr
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 767: // term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 768: // term -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 769: // term -> power_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 770: // term -> term, mulop, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 771: // term -> term, mulop, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 772: // term -> term, mulop, new_question_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 773: // term -> as_is_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 774: // mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 775: // mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 776: // mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 777: // mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 778: // mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 779: // mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 780: // mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 781: // default_expr -> tkDefault, tkRoundOpen, 
                //                 simple_or_template_or_question_type_reference, tkRoundClose
{ 
			CurrentSemanticValue.ex = new default_operator(ValueStack[ValueStack.Depth-2].td as named_type_reference, CurrentLocationSpan);  
		}
        break;
      case 782: // tuple -> tkRoundOpen, expr_l1_or_unpacked, tkComma, expr_l1_or_unpacked_list, 
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
      case 783: // factor_without_unary_op -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 784: // factor_without_unary_op -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 785: // factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 786: // factor -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 787: // factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 788: // factor -> tkNot, factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 789: // factor -> sign, factor
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
      case 790: // factor -> tkDeref, factor
{
            CurrentSemanticValue.ex = new index(ValueStack[ValueStack.Depth-1].ex, true, CurrentLocationSpan);
        }
        break;
      case 791: // factor -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 792: // factor -> tuple
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 793: // literal_or_number -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 794: // literal_or_number -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 795: // var_question_point -> variable, tkQuestionPoint, variable
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 796: // var_question_point -> variable, tkQuestionPoint, var_question_point
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 797: // var_reference -> var_address, variable
{
			CurrentSemanticValue.ex = NewVarReference(ValueStack[ValueStack.Depth-2].stn as get_address, ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);
		}
        break;
      case 798: // var_reference -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 799: // var_reference -> var_question_point
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 800: // var_reference -> tkRoundOpen, tkVar, identifier, tkAssign, expr_dq, 
                //                  tkRoundClose
{ CurrentSemanticValue.ex = new let_var_expr(ValueStack[ValueStack.Depth-4].id,ValueStack[ValueStack.Depth-2].ex,CurrentLocationSpan); }
        break;
      case 801: // var_address -> tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(CurrentLocationSpan);
		}
        break;
      case 802: // var_address -> var_address, tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(ValueStack[ValueStack.Depth-2].stn as get_address, CurrentLocationSpan);
		}
        break;
      case 803: // attribute_variable -> simple_type_identifier, optional_expr_list_with_bracket
{ 
			CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
		}
        break;
      case 804: // attribute_variable -> template_type, optional_expr_list_with_bracket
{
            CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 805: // dotted_identifier -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 806: // dotted_identifier -> dotted_identifier, tkPoint, identifier_or_keyword
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
		}
        break;
      case 807: // variable_as_type -> dotted_identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;}
        break;
      case 808: // variable_as_type -> dotted_identifier, template_type_params
{ CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-2].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);   }
        break;
      case 809: // var_with_init_for_expr_with_let -> tkVar, identifier, tkAssign, expr, 
                //                                    tkSemiColon
{
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-4].id as addressed_value, ValueStack[ValueStack.Depth-2].ex, Operators.Assignment, CurrentLocationSpan);
		}
        break;
      case 810: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 811: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let_list, 
                //                                         var_with_init_for_expr_with_let
{
			ValueStack[ValueStack.Depth-2].stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
		}
        break;
      case 812: // proc_func_call -> variable, tkRoundOpen, optional_expr_list_func_param, 
                //                   tkRoundClose
{
			if (ValueStack[ValueStack.Depth-4].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-4], "^");
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 813: // variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 814: // variable -> operator_name_ident
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 815: // variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 816: // variable -> tkRoundOpen, expr, tkRoundClose
{
		    if (!parserTools.buildTreeForFormatter) 
            {
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
            } 
			else CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
        }
        break;
      case 817: // variable -> tkRoundOpen, var_with_init_for_expr_with_let_list, expr, 
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
      case 818: // variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 819: // variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 820: // variable -> literal_or_number, tkPoint, identifier_or_keyword
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");		
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 821: // variable -> variable, tkSquareOpen, expr_list, tkSquareClose
{
        	CurrentSemanticValue.ex = NewIndexerOrSlice(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list,CurrentLocationSpan);
        }
        break;
      case 822: // variable -> literal_or_number, tkSquareOpen, expr_list, tkSquareClose
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
      case 823: // variable -> variable, tkQuestionSquareOpen, format_expr, tkSquareClose
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
      case 824: // variable -> literal_or_number, tkQuestionSquareOpen, format_expr, tkSquareClose
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
      case 825: // variable -> tkVertParen, elem_list, tkVertParen
{ 
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, '|', CurrentLocationSpan);  
		}
        break;
      case 826: // variable -> pascal_set_const
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
		}
        break;
      case 827: // variable -> proc_func_call
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 828: // variable -> variable, tkPoint, identifier_keyword_operatorname
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 829: // variable -> tuple, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 830: // variable -> variable, tkDeref
{
			CurrentSemanticValue.ex = new roof_dereference(ValueStack[ValueStack.Depth-2].ex as addressed_value,CurrentLocationSpan);
        }
        break;
      case 831: // variable -> variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 832: // optional_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 833: // optional_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 834: // optional_expr_list_func_param -> expr_list_func_param
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 835: // optional_expr_list_func_param -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 836: // elem_list -> elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 837: // elem_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 838: // elem_list1 -> elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 839: // elem_list1 -> elem_list1, tkComma, elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 840: // elem -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 841: // elem -> expr, tkDotDot, expr
{ CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 842: // one_literal -> tkStringLiteral
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 843: // one_literal -> tkAsciiChar
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 844: // literal -> literal_list
{ 
			CurrentSemanticValue.ex = NewLiteral(ValueStack[ValueStack.Depth-1].stn as literal_const_line);
        }
        break;
      case 845: // literal -> tkFormatStringLiteral
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
      case 846: // literal -> tkMultilineStringLiteral
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
      case 847: // literal_list -> one_literal
{ 
			CurrentSemanticValue.stn = new literal_const_line(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 848: // literal_list -> literal_list, one_literal
{ 
        	var line = ValueStack[ValueStack.Depth-2].stn as literal_const_line;
            if (line.literals.Last() is string_const && ValueStack[ValueStack.Depth-1].ex is string_const)
            	parserTools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.stn = line.Add(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 849: // operator_name_ident -> tkOperator, overload_operator
{ 
			CurrentSemanticValue.ex = new operator_name_ident((ValueStack[ValueStack.Depth-1].op as op_type_node).text, (ValueStack[ValueStack.Depth-1].op as op_type_node).type, CurrentLocationSpan);
		}
        break;
      case 850: // optional_method_modificators -> tkSemiColon
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 851: // optional_method_modificators -> tkSemiColon, meth_modificators, tkSemiColon
{ 
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 852: // optional_method_modificators1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 853: // optional_method_modificators1 -> tkSemiColon, meth_modificators
{ 
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 854: // meth_modificators -> meth_modificator
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan); 
		}
        break;
      case 855: // meth_modificators -> meth_modificators, tkSemiColon, meth_modificator
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as procedure_attributes_list).Add(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan);  
		}
        break;
      case 856: // identifier -> tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 857: // identifier -> tkExport
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 858: // identifier -> property_specifier_directives
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 859: // identifier -> non_reserved
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 860: // identifier -> tkStep
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 861: // identifier -> tkIndex
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 862: // identifier_or_keyword -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 863: // identifier_or_keyword -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 864: // identifier_or_keyword -> reserved_keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 865: // identifier_keyword_operatorname -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 866: // identifier_keyword_operatorname -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 867: // identifier_keyword_operatorname -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 868: // meth_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 869: // meth_modificator -> tkOverload
{ 
            CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id;
            parserTools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", ValueStack[ValueStack.Depth-1].id.source_context);
        }
        break;
      case 870: // meth_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 871: // meth_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 872: // meth_modificator -> tkExtensionMethod
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 873: // meth_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 874: // property_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 875: // property_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 876: // property_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 877: // property_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 878: // property_specifier_directives -> tkRead
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 879: // property_specifier_directives -> tkWrite
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 880: // non_reserved -> tkName
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 881: // non_reserved -> tkNew
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 882: // visibility_specifier -> tkInternal
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 883: // visibility_specifier -> tkPublic
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 884: // visibility_specifier -> tkProtected
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 885: // visibility_specifier -> tkPrivate
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 886: // keyword -> visibility_specifier
{ 
			CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  
		}
        break;
      case 887: // keyword -> tkSealed
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 888: // keyword -> tkTemplate
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 889: // keyword -> tkOr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 890: // keyword -> tkTypeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 891: // keyword -> tkSizeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 892: // keyword -> tkDefault
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 893: // keyword -> tkWhere
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 894: // keyword -> tkXor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 895: // keyword -> tkAnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 896: // keyword -> tkDiv
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 897: // keyword -> tkMod
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 898: // keyword -> tkShl
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 899: // keyword -> tkShr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 900: // keyword -> tkNot
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 901: // keyword -> tkAs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 902: // keyword -> tkIn
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 903: // keyword -> tkIs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 904: // keyword -> tkArray
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 905: // keyword -> tkSequence
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 906: // keyword -> tkBegin
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 907: // keyword -> tkCase
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 908: // keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 909: // keyword -> tkConst
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 910: // keyword -> tkConstructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 911: // keyword -> tkDestructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 912: // keyword -> tkDownto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 913: // keyword -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 914: // keyword -> tkElse
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 915: // keyword -> tkEnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 916: // keyword -> tkExcept
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 917: // keyword -> tkFile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 918: // keyword -> tkAuto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 919: // keyword -> tkFinalization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 920: // keyword -> tkFinally
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 921: // keyword -> tkFor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 922: // keyword -> tkForeach
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 923: // keyword -> tkFunction
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 924: // keyword -> tkIf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 925: // keyword -> tkImplementation
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 926: // keyword -> tkInherited
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 927: // keyword -> tkInitialization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 928: // keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 929: // keyword -> tkProcedure
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 930: // keyword -> tkProperty
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 931: // keyword -> tkRaise
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 932: // keyword -> tkRecord
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 933: // keyword -> tkRepeat
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 934: // keyword -> tkSet
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 935: // keyword -> tkTry
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 936: // keyword -> tkType
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 937: // keyword -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 938: // keyword -> tkThen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 939: // keyword -> tkTo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 940: // keyword -> tkUntil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 941: // keyword -> tkUses
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 942: // keyword -> tkVar
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 943: // keyword -> tkWhile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 944: // keyword -> tkWith
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 945: // keyword -> tkNil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 946: // keyword -> tkGoto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 947: // keyword -> tkOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 948: // keyword -> tkLabel
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 949: // keyword -> tkProgram
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 950: // keyword -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 951: // keyword -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 952: // keyword -> tkNamespace
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 953: // keyword -> tkExternal
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 954: // keyword -> tkParams
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 955: // keyword -> tkEvent
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 956: // keyword -> tkYield
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 957: // keyword -> tkMatch
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 958: // keyword -> tkWhen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 959: // keyword -> tkPartial
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 960: // keyword -> tkAbstract
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 961: // keyword -> tkLock
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 962: // keyword -> tkImplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 963: // keyword -> tkExplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 964: // keyword -> tkOn
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 965: // keyword -> tkVirtual
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 966: // keyword -> tkOverride
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 967: // keyword -> tkLoop
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 968: // keyword -> tkExtensionMethod
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 969: // keyword -> tkOverload
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 970: // keyword -> tkReintroduce
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 971: // keyword -> tkForward
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 972: // reserved_keyword -> tkOperator
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 973: // overload_operator -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 974: // overload_operator -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 975: // overload_operator -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 976: // overload_operator -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 977: // overload_operator -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 978: // overload_operator -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 979: // overload_operator -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 980: // overload_operator -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 981: // overload_operator -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 982: // overload_operator -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 983: // overload_operator -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 984: // overload_operator -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 985: // overload_operator -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 986: // overload_operator -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 987: // overload_operator -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 988: // overload_operator -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 989: // overload_operator -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 990: // overload_operator -> tkNot
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 991: // overload_operator -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 992: // overload_operator -> tkImplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 993: // overload_operator -> tkExplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 994: // overload_operator -> assign_operator
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 995: // overload_operator -> tkStarStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 996: // assign_operator -> tkAssign
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 997: // assign_operator -> tkPlusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 998: // assign_operator -> tkMinusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 999: // assign_operator -> tkMultEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1000: // assign_operator -> tkDivEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1001: // lambda_unpacked_params -> tkBackSlashRoundOpen, 
                 //                           lambda_list_of_unpacked_params_or_id, tkComma, 
                 //                           lambda_unpacked_params_or_id, tkRoundClose
{
			// �?ез�?л�?�?а�? надо п�?исвои�?�? каком�? �?о са�?а�?ном�? пол�? в function_lambda_definition
			(ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-2].ob as ident_or_list);
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list;
		}
        break;
      case 1002: // lambda_unpacked_params_or_id -> lambda_unpacked_params
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].ex as unpacked_list_of_ident_or_list);
		}
        break;
      case 1003: // lambda_unpacked_params_or_id -> identifier
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].id as ident);
		}
        break;
      case 1004: // lambda_list_of_unpacked_params_or_id -> lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = new unpacked_list_of_ident_or_list();
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LocationStack[LocationStack.Depth-1];
		}
        break;
      case 1005: // lambda_list_of_unpacked_params_or_id -> lambda_list_of_unpacked_params_or_id, 
                 //                                         tkComma, lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 1006: // expr_l1_or_unpacked -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 1007: // expr_l1_or_unpacked -> lambda_unpacked_params
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 1008: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 1009: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked_list, tkComma, 
                 //                             expr_l1_or_unpacked
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 1010: // func_decl_lambda -> identifier, tkArrow, lambda_function_body
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
      case 1011: // func_decl_lambda -> tkRoundOpen, tkRoundClose, lambda_type_ref_noproctype, 
                 //                     tkArrow, lambda_function_body
{
		    // �?дес�? надо анализи�?ова�?�? по �?ел�? и либо ос�?авля�?�? lambda_inferred_type, либо дела�?�? его null!
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, sl, CurrentLocationSpan);	
		}
        break;
      case 1012: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkRoundClose, 
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
      case 1013: // func_decl_lambda -> tkRoundOpen, identifier, tkSemiColon, full_lambda_fp_list, 
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
      case 1014: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkSemiColon, 
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
      case 1015: // func_decl_lambda -> tkRoundOpen, expr_l1_or_unpacked, tkComma, 
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
      case 1016: // func_decl_lambda -> lambda_unpacked_params, rem_lambda
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
      case 1017: // func_decl_lambda -> expl_func_decl_lambda
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 1018: // optional_full_lambda_fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 1019: // optional_full_lambda_fp_list -> tkSemiColon, full_lambda_fp_list
{
		CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
	}
        break;
      case 1020: // rem_lambda -> lambda_type_ref_noproctype, tkArrow, lambda_function_body
{ 
		    CurrentSemanticValue.ob = new pair_type_stlist(ValueStack[ValueStack.Depth-3].td,ValueStack[ValueStack.Depth-1].stn as statement_list);
		}
        break;
      case 1021: // expl_func_decl_lambda -> tkFunction, lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1022: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, tkRoundClose, 
                 //                          lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1023: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, full_lambda_fp_list, 
                 //                          tkRoundClose, lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1024: // expl_func_decl_lambda -> tkProcedure, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1025: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, tkRoundClose, tkArrow, 
                 //                          lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1026: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, full_lambda_fp_list, 
                 //                          tkRoundClose, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-4].stn as formal_parameters, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1027: // full_lambda_fp_list -> lambda_simple_fp_sect
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
      case 1028: // full_lambda_fp_list -> full_lambda_fp_list, tkSemiColon, lambda_simple_fp_sect
{
			CurrentSemanticValue.stn =(ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
		}
        break;
      case 1029: // lambda_simple_fp_sect -> ident_list, lambda_type_ref
{
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-2].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan);
		}
        break;
      case 1030: // lambda_type_ref -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
		}
        break;
      case 1031: // lambda_type_ref -> tkColon, fptype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 1032: // lambda_type_ref_noproctype -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
		}
        break;
      case 1033: // lambda_type_ref_noproctype -> tkColon, fptype_noproctype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 1034: // common_lambda_body -> compound_stmt
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 1035: // common_lambda_body -> if_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1036: // common_lambda_body -> while_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1037: // common_lambda_body -> repeat_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1038: // common_lambda_body -> for_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1039: // common_lambda_body -> foreach_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1040: // common_lambda_body -> loop_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1041: // common_lambda_body -> case_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1042: // common_lambda_body -> try_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1043: // common_lambda_body -> lock_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1044: // common_lambda_body -> raise_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1045: // common_lambda_body -> yield_stmt
{
			parserTools.AddErrorFromResource("YIELD_STATEMENT_CANNOT_BE_USED_IN_LAMBDA_BODY", CurrentLocationSpan);
		}
        break;
      case 1046: // common_lambda_body -> tkRoundOpen, assignment, tkRoundClose
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-2]);
		}
        break;
      case 1047: // lambda_function_body -> expr_l1_for_lambda
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
      case 1048: // lambda_function_body -> common_lambda_body
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 1049: // lambda_procedure_body -> proc_call
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1050: // lambda_procedure_body -> assignment
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1051: // lambda_procedure_body -> common_lambda_body
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
