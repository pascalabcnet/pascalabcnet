// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  DESKTOP-L2INHKG
// DateTime: 19.11.2026 10:03:51
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
  private static Rule[] rules = new Rule[1062];
  private static State[] states = new State[1761];
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
      "interface_declarations", "simple_unit_declarations", "simple_unit_decl_list", 
      "simple_unit_decl", "simple_type_list", "literal", "one_literal", "literal_list", 
      "label_decl_sect", "lock_stmt", "func_name", "proc_name", "optional_proc_name", 
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
    states[0] = new State(new int[]{61,1610,105,1727,106,1728,109,1729,88,1734,90,1739,89,1746,75,1751,77,1757,3,-27,52,-27,91,-27,59,-27,29,-27,66,-27,50,-27,53,-27,62,-27,11,-27,44,-27,37,-27,28,-27,26,-27,19,-27,30,-27,31,-27},new int[]{-1,1,-243,3,-244,4,-317,1622,-319,1623,-7,1722,-184,1733});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,1606,52,-14,91,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-194,5,-195,1604,-193,1609});
    states[5] = new State(new int[]{52,1591,91,-41,59,-41,29,-41,66,-41,50,-41,53,-41,62,-41,11,-41,44,-41,37,-41,28,-41,26,-41,19,-41,30,-41,31,-41},new int[]{-312,6,-316,1589,-314,1603});
    states[6] = new State(-92,new int[]{-22,7,-41,14,-45,1524,-46,1525});
    states[7] = new State(new int[]{7,9,10,10,5,11,100,12,6,13,2,-26},new int[]{-197,8});
    states[8] = new State(-20);
    states[9] = new State(-21);
    states[10] = new State(-22);
    states[11] = new State(-23);
    states[12] = new State(-24);
    states[13] = new State(-25);
    states[14] = new State(new int[]{91,16},new int[]{-265,15});
    states[15] = new State(-34);
    states[16] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524},new int[]{-262,17,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[17] = new State(new int[]{92,18,10,19});
    states[18] = new State(-560);
    states[19] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524},new int[]{-271,20,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[20] = new State(-562);
    states[21] = new State(-522);
    states[22] = new State(-525);
    states[23] = new State(new int[]{110,429,111,430,112,431,113,432,114,433,92,-558,10,-558,98,-558,101,-558,33,-558,104,-558,2,-558,9,-558,12,-558,100,-558,99,-558,32,-558,85,-558,84,-558,83,-558,82,-558,81,-558,86,-558},new int[]{-203,24});
    states[24] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-93,25,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,834,-105,659,-334,677});
    states[25] = new State(-552);
    states[26] = new State(new int[]{70,27,92,-628,10,-628,98,-628,101,-628,33,-628,104,-628,2,-628,9,-628,12,-628,100,-628,99,-628,32,-628,85,-628,84,-628,83,-628,82,-628,81,-628,86,-628});
    states[27] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,28,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[28] = new State(-637);
    states[29] = new State(new int[]{16,30,70,-638,92,-638,10,-638,98,-638,101,-638,33,-638,104,-638,2,-638,9,-638,12,-638,100,-638,99,-638,32,-638,85,-638,84,-638,83,-638,82,-638,81,-638,86,-638,6,-638,76,-638,5,-638,51,-638,58,-638,141,-638,143,-638,161,-638,80,-638,78,-638,160,-638,87,-638,45,-638,42,-638,8,-638,21,-638,22,-638,144,-638,147,-638,145,-638,146,-638,155,-638,158,-638,157,-638,156,-638,11,-638,57,-638,91,-638,40,-638,25,-638,97,-638,54,-638,35,-638,55,-638,102,-638,47,-638,36,-638,53,-638,60,-638,74,-638,72,-638,38,-638,71,-638,13,-641});
    states[30] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-103,31,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578});
    states[31] = new State(new int[]{120,157,125,158,123,159,121,160,124,161,122,162,137,163,135,164,16,-651,70,-651,92,-651,10,-651,98,-651,101,-651,33,-651,104,-651,2,-651,9,-651,12,-651,100,-651,99,-651,32,-651,85,-651,84,-651,83,-651,82,-651,81,-651,86,-651,13,-651,6,-651,76,-651,5,-651,51,-651,58,-651,141,-651,143,-651,161,-651,80,-651,78,-651,160,-651,87,-651,45,-651,42,-651,8,-651,21,-651,22,-651,144,-651,147,-651,145,-651,146,-651,155,-651,158,-651,157,-651,156,-651,11,-651,57,-651,91,-651,40,-651,25,-651,97,-651,54,-651,35,-651,55,-651,102,-651,47,-651,36,-651,53,-651,60,-651,74,-651,72,-651,38,-651,71,-651,116,-651,115,-651,128,-651,129,-651,126,-651,138,-651,136,-651,118,-651,117,-651,131,-651,132,-651,133,-651,134,-651,130,-651},new int[]{-205,32});
    states[32] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-110,33,-252,1523,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,602,-277,578});
    states[33] = new State(new int[]{6,34,120,-678,125,-678,123,-678,121,-678,124,-678,122,-678,137,-678,135,-678,16,-678,70,-678,92,-678,10,-678,98,-678,101,-678,33,-678,104,-678,2,-678,9,-678,12,-678,100,-678,99,-678,32,-678,85,-678,84,-678,83,-678,82,-678,81,-678,86,-678,13,-678,76,-678,5,-678,51,-678,58,-678,141,-678,143,-678,161,-678,80,-678,78,-678,160,-678,87,-678,45,-678,42,-678,8,-678,21,-678,22,-678,144,-678,147,-678,145,-678,146,-678,155,-678,158,-678,157,-678,156,-678,11,-678,57,-678,91,-678,40,-678,25,-678,97,-678,54,-678,35,-678,55,-678,102,-678,47,-678,36,-678,53,-678,60,-678,74,-678,72,-678,38,-678,71,-678,116,-678,115,-678,128,-678,129,-678,126,-678,138,-678,136,-678,118,-678,117,-678,131,-678,132,-678,133,-678,134,-678,130,-678});
    states[34] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-88,35,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,602,-277,578});
    states[35] = new State(new int[]{116,172,115,173,128,174,129,175,126,176,6,-757,5,-757,120,-757,125,-757,123,-757,121,-757,124,-757,122,-757,137,-757,135,-757,16,-757,70,-757,92,-757,10,-757,98,-757,101,-757,33,-757,104,-757,2,-757,9,-757,12,-757,100,-757,99,-757,32,-757,85,-757,84,-757,83,-757,82,-757,81,-757,86,-757,13,-757,76,-757,51,-757,58,-757,141,-757,143,-757,161,-757,80,-757,78,-757,160,-757,87,-757,45,-757,42,-757,8,-757,21,-757,22,-757,144,-757,147,-757,145,-757,146,-757,155,-757,158,-757,157,-757,156,-757,11,-757,57,-757,91,-757,40,-757,25,-757,97,-757,54,-757,35,-757,55,-757,102,-757,47,-757,36,-757,53,-757,60,-757,74,-757,72,-757,38,-757,71,-757,138,-757,136,-757,118,-757,117,-757,131,-757,132,-757,133,-757,134,-757,130,-757},new int[]{-206,36});
    states[36] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-87,37,-252,1522,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,602,-277,578});
    states[37] = new State(new int[]{138,178,136,1512,118,1515,117,1516,131,1517,132,1518,133,1519,134,1520,130,1521,116,-759,115,-759,128,-759,129,-759,126,-759,6,-759,5,-759,120,-759,125,-759,123,-759,121,-759,124,-759,122,-759,137,-759,135,-759,16,-759,70,-759,92,-759,10,-759,98,-759,101,-759,33,-759,104,-759,2,-759,9,-759,12,-759,100,-759,99,-759,32,-759,85,-759,84,-759,83,-759,82,-759,81,-759,86,-759,13,-759,76,-759,51,-759,58,-759,141,-759,143,-759,161,-759,80,-759,78,-759,160,-759,87,-759,45,-759,42,-759,8,-759,21,-759,22,-759,144,-759,147,-759,145,-759,146,-759,155,-759,158,-759,157,-759,156,-759,11,-759,57,-759,91,-759,40,-759,25,-759,97,-759,54,-759,35,-759,55,-759,102,-759,47,-759,36,-759,53,-759,60,-759,74,-759,72,-759,38,-759,71,-759},new int[]{-207,38});
    states[38] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-101,39,-278,40,-252,41,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-102,461});
    states[39] = new State(-780);
    states[40] = new State(-781);
    states[41] = new State(-782);
    states[42] = new State(-795);
    states[43] = new State(new int[]{7,44,11,150,17,380,138,-796,136,-796,118,-796,117,-796,131,-796,132,-796,133,-796,134,-796,130,-796,116,-796,115,-796,128,-796,129,-796,126,-796,6,-796,5,-796,120,-796,125,-796,123,-796,121,-796,124,-796,122,-796,137,-796,135,-796,16,-796,70,-796,92,-796,10,-796,98,-796,101,-796,33,-796,104,-796,2,-796,9,-796,12,-796,100,-796,99,-796,32,-796,85,-796,84,-796,83,-796,82,-796,81,-796,86,-796,13,-796,76,-796,51,-796,58,-796,141,-796,143,-796,161,-796,80,-796,78,-796,160,-796,87,-796,45,-796,42,-796,8,-796,21,-796,22,-796,144,-796,147,-796,145,-796,146,-796,155,-796,158,-796,157,-796,156,-796,57,-796,91,-796,40,-796,25,-796,97,-796,54,-796,35,-796,55,-796,102,-796,47,-796,36,-796,53,-796,60,-796,74,-796,72,-796,38,-796,71,-796,119,-793});
    states[44] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-143,45,-152,46,-156,49,-157,52,-302,57,-155,58,-303,148});
    states[45] = new State(-830);
    states[46] = new State(-872);
    states[47] = new State(-866);
    states[48] = new State(-867);
    states[49] = new State(-868);
    states[50] = new State(-888);
    states[51] = new State(-889);
    states[52] = new State(-869);
    states[53] = new State(-890);
    states[54] = new State(-891);
    states[55] = new State(-870);
    states[56] = new State(-871);
    states[57] = new State(-873);
    states[58] = new State(-896);
    states[59] = new State(-892);
    states[60] = new State(-893);
    states[61] = new State(-894);
    states[62] = new State(-895);
    states[63] = new State(-897);
    states[64] = new State(-898);
    states[65] = new State(-899);
    states[66] = new State(-900);
    states[67] = new State(-901);
    states[68] = new State(-902);
    states[69] = new State(-903);
    states[70] = new State(-904);
    states[71] = new State(-905);
    states[72] = new State(-906);
    states[73] = new State(-907);
    states[74] = new State(-908);
    states[75] = new State(-909);
    states[76] = new State(-910);
    states[77] = new State(-911);
    states[78] = new State(-912);
    states[79] = new State(-913);
    states[80] = new State(-914);
    states[81] = new State(-915);
    states[82] = new State(-916);
    states[83] = new State(-917);
    states[84] = new State(-918);
    states[85] = new State(-919);
    states[86] = new State(-920);
    states[87] = new State(-921);
    states[88] = new State(-922);
    states[89] = new State(-923);
    states[90] = new State(-924);
    states[91] = new State(-925);
    states[92] = new State(-926);
    states[93] = new State(-927);
    states[94] = new State(-928);
    states[95] = new State(-929);
    states[96] = new State(-930);
    states[97] = new State(-931);
    states[98] = new State(-932);
    states[99] = new State(-933);
    states[100] = new State(-934);
    states[101] = new State(-935);
    states[102] = new State(-936);
    states[103] = new State(-937);
    states[104] = new State(-938);
    states[105] = new State(-939);
    states[106] = new State(-940);
    states[107] = new State(-941);
    states[108] = new State(-942);
    states[109] = new State(-943);
    states[110] = new State(-944);
    states[111] = new State(-945);
    states[112] = new State(-946);
    states[113] = new State(-947);
    states[114] = new State(-948);
    states[115] = new State(-949);
    states[116] = new State(-950);
    states[117] = new State(-951);
    states[118] = new State(-952);
    states[119] = new State(-953);
    states[120] = new State(-954);
    states[121] = new State(-955);
    states[122] = new State(-956);
    states[123] = new State(-957);
    states[124] = new State(-958);
    states[125] = new State(-959);
    states[126] = new State(-960);
    states[127] = new State(-961);
    states[128] = new State(-962);
    states[129] = new State(-963);
    states[130] = new State(-964);
    states[131] = new State(-965);
    states[132] = new State(-966);
    states[133] = new State(-967);
    states[134] = new State(-968);
    states[135] = new State(-969);
    states[136] = new State(-970);
    states[137] = new State(-971);
    states[138] = new State(-972);
    states[139] = new State(-973);
    states[140] = new State(-974);
    states[141] = new State(-975);
    states[142] = new State(-976);
    states[143] = new State(-977);
    states[144] = new State(-978);
    states[145] = new State(-979);
    states[146] = new State(-980);
    states[147] = new State(-981);
    states[148] = new State(-874);
    states[149] = new State(-982);
    states[150] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-76,151,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,834,-105,659,-334,677});
    states[151] = new State(new int[]{12,152,100,153});
    states[152] = new State(-832);
    states[153] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-93,154,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,834,-105,659,-334,677});
    states[154] = new State(-623);
    states[155] = new State(-635);
    states[156] = new State(new int[]{120,157,125,158,123,159,121,160,124,161,122,162,137,163,135,164,16,-650,70,-650,92,-650,10,-650,98,-650,101,-650,33,-650,104,-650,2,-650,9,-650,12,-650,100,-650,99,-650,32,-650,85,-650,84,-650,83,-650,82,-650,81,-650,86,-650,13,-650,6,-650,76,-650,5,-650,51,-650,58,-650,141,-650,143,-650,161,-650,80,-650,78,-650,160,-650,87,-650,45,-650,42,-650,8,-650,21,-650,22,-650,144,-650,147,-650,145,-650,146,-650,155,-650,158,-650,157,-650,156,-650,11,-650,57,-650,91,-650,40,-650,25,-650,97,-650,54,-650,35,-650,55,-650,102,-650,47,-650,36,-650,53,-650,60,-650,74,-650,72,-650,38,-650,71,-650,116,-650,115,-650,128,-650,129,-650,126,-650,138,-650,136,-650,118,-650,117,-650,131,-650,132,-650,133,-650,134,-650,130,-650},new int[]{-205,32});
    states[157] = new State(-748);
    states[158] = new State(-749);
    states[159] = new State(-750);
    states[160] = new State(-751);
    states[161] = new State(-752);
    states[162] = new State(-753);
    states[163] = new State(-754);
    states[164] = new State(new int[]{137,165});
    states[165] = new State(-755);
    states[166] = new State(new int[]{6,34,5,167,120,-677,125,-677,123,-677,121,-677,124,-677,122,-677,137,-677,135,-677,16,-677,70,-677,92,-677,10,-677,98,-677,101,-677,33,-677,104,-677,2,-677,9,-677,12,-677,100,-677,99,-677,32,-677,85,-677,84,-677,83,-677,82,-677,81,-677,86,-677,13,-677,76,-677});
    states[167] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,5,-737,70,-737,92,-737,10,-737,98,-737,101,-737,33,-737,104,-737,2,-737,9,-737,12,-737,100,-737,99,-737,32,-737,84,-737,83,-737,82,-737,81,-737,6,-737},new int[]{-120,168,-110,603,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,602,-277,578});
    states[168] = new State(new int[]{5,169,70,-740,92,-740,10,-740,98,-740,101,-740,33,-740,104,-740,2,-740,9,-740,12,-740,100,-740,99,-740,32,-740,85,-740,84,-740,83,-740,82,-740,81,-740,86,-740,6,-740,76,-740});
    states[169] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-110,170,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,602,-277,578});
    states[170] = new State(new int[]{6,34,70,-742,92,-742,10,-742,98,-742,101,-742,33,-742,104,-742,2,-742,9,-742,12,-742,100,-742,99,-742,32,-742,85,-742,84,-742,83,-742,82,-742,81,-742,86,-742,76,-742});
    states[171] = new State(new int[]{116,172,115,173,128,174,129,175,126,176,6,-756,5,-756,120,-756,125,-756,123,-756,121,-756,124,-756,122,-756,137,-756,135,-756,16,-756,70,-756,92,-756,10,-756,98,-756,101,-756,33,-756,104,-756,2,-756,9,-756,12,-756,100,-756,99,-756,32,-756,85,-756,84,-756,83,-756,82,-756,81,-756,86,-756,13,-756,76,-756,51,-756,58,-756,141,-756,143,-756,161,-756,80,-756,78,-756,160,-756,87,-756,45,-756,42,-756,8,-756,21,-756,22,-756,144,-756,147,-756,145,-756,146,-756,155,-756,158,-756,157,-756,156,-756,11,-756,57,-756,91,-756,40,-756,25,-756,97,-756,54,-756,35,-756,55,-756,102,-756,47,-756,36,-756,53,-756,60,-756,74,-756,72,-756,38,-756,71,-756,138,-756,136,-756,118,-756,117,-756,131,-756,132,-756,133,-756,134,-756,130,-756},new int[]{-206,36});
    states[172] = new State(-761);
    states[173] = new State(-762);
    states[174] = new State(-763);
    states[175] = new State(-764);
    states[176] = new State(-765);
    states[177] = new State(new int[]{138,178,136,1512,118,1515,117,1516,131,1517,132,1518,133,1519,134,1520,130,1521,116,-758,115,-758,128,-758,129,-758,126,-758,6,-758,5,-758,120,-758,125,-758,123,-758,121,-758,124,-758,122,-758,137,-758,135,-758,16,-758,70,-758,92,-758,10,-758,98,-758,101,-758,33,-758,104,-758,2,-758,9,-758,12,-758,100,-758,99,-758,32,-758,85,-758,84,-758,83,-758,82,-758,81,-758,86,-758,13,-758,76,-758,51,-758,58,-758,141,-758,143,-758,161,-758,80,-758,78,-758,160,-758,87,-758,45,-758,42,-758,8,-758,21,-758,22,-758,144,-758,147,-758,145,-758,146,-758,155,-758,158,-758,157,-758,156,-758,11,-758,57,-758,91,-758,40,-758,25,-758,97,-758,54,-758,35,-758,55,-758,102,-758,47,-758,36,-758,53,-758,60,-758,74,-758,72,-758,38,-758,71,-758},new int[]{-207,38});
    states[178] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,24,494},new int[]{-293,179,-288,180,-189,181,-152,218,-156,49,-157,52,-280,500});
    states[179] = new State(-772);
    states[180] = new State(-773);
    states[181] = new State(new int[]{7,182,4,185,123,187,8,-659,138,-659,136,-659,118,-659,117,-659,131,-659,132,-659,133,-659,134,-659,130,-659,116,-659,115,-659,128,-659,129,-659,126,-659,6,-659,5,-659,120,-659,125,-659,121,-659,124,-659,122,-659,137,-659,135,-659,16,-659,70,-659,92,-659,10,-659,98,-659,101,-659,33,-659,104,-659,2,-659,9,-659,12,-659,100,-659,99,-659,32,-659,85,-659,84,-659,83,-659,82,-659,81,-659,86,-659,13,-659,76,-659,51,-659,58,-659,141,-659,143,-659,161,-659,80,-659,78,-659,160,-659,87,-659,45,-659,42,-659,21,-659,22,-659,144,-659,147,-659,145,-659,146,-659,155,-659,158,-659,157,-659,156,-659,11,-659,57,-659,91,-659,40,-659,25,-659,97,-659,54,-659,35,-659,55,-659,102,-659,47,-659,36,-659,53,-659,60,-659,74,-659,72,-659,38,-659,71,-659},new int[]{-308,184});
    states[182] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-143,183,-152,46,-156,49,-157,52,-302,57,-155,58,-303,148});
    states[183] = new State(-293);
    states[184] = new State(-660);
    states[185] = new State(new int[]{123,187},new int[]{-308,186});
    states[186] = new State(-661);
    states[187] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-306,188,-289,313,-282,192,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-291,1470,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,1471,-233,561,-232,562,-310,1472});
    states[188] = new State(new int[]{121,189,100,190});
    states[189] = new State(-267);
    states[190] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-289,191,-282,192,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-291,1470,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,1471,-233,561,-232,562,-310,1472});
    states[191] = new State(-271);
    states[192] = new State(new int[]{13,193,121,-275,100,-275,120,-275,9,-275,10,-275,8,-275,138,-275,136,-275,118,-275,117,-275,131,-275,132,-275,133,-275,134,-275,130,-275,116,-275,115,-275,128,-275,129,-275,126,-275,6,-275,5,-275,125,-275,123,-275,124,-275,122,-275,137,-275,135,-275,16,-275,70,-275,92,-275,98,-275,101,-275,33,-275,104,-275,2,-275,12,-275,99,-275,32,-275,85,-275,84,-275,83,-275,82,-275,81,-275,86,-275,76,-275,51,-275,58,-275,141,-275,143,-275,161,-275,80,-275,78,-275,160,-275,87,-275,45,-275,42,-275,21,-275,22,-275,144,-275,147,-275,145,-275,146,-275,155,-275,158,-275,157,-275,156,-275,11,-275,57,-275,91,-275,40,-275,25,-275,97,-275,54,-275,35,-275,55,-275,102,-275,47,-275,36,-275,53,-275,60,-275,74,-275,72,-275,38,-275,71,-275,127,-275,110,-275});
    states[193] = new State(-276);
    states[194] = new State(new int[]{6,1510,116,247,115,248,128,249,129,250,13,-280,121,-280,100,-280,120,-280,9,-280,10,-280,8,-280,138,-280,136,-280,118,-280,117,-280,131,-280,132,-280,133,-280,134,-280,130,-280,126,-280,5,-280,125,-280,123,-280,124,-280,122,-280,137,-280,135,-280,16,-280,70,-280,92,-280,98,-280,101,-280,33,-280,104,-280,2,-280,12,-280,99,-280,32,-280,85,-280,84,-280,83,-280,82,-280,81,-280,86,-280,76,-280,51,-280,58,-280,141,-280,143,-280,161,-280,80,-280,78,-280,160,-280,87,-280,45,-280,42,-280,21,-280,22,-280,144,-280,147,-280,145,-280,146,-280,155,-280,158,-280,157,-280,156,-280,11,-280,57,-280,91,-280,40,-280,25,-280,97,-280,54,-280,35,-280,55,-280,102,-280,47,-280,36,-280,53,-280,60,-280,74,-280,72,-280,38,-280,71,-280,127,-280,110,-280},new int[]{-202,195});
    states[195] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285},new int[]{-111,196,-112,315,-189,352,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283});
    states[196] = new State(new int[]{118,254,117,255,131,256,132,257,133,258,134,259,130,260,6,-284,116,-284,115,-284,128,-284,129,-284,13,-284,121,-284,100,-284,120,-284,9,-284,10,-284,8,-284,138,-284,136,-284,126,-284,5,-284,125,-284,123,-284,124,-284,122,-284,137,-284,135,-284,16,-284,70,-284,92,-284,98,-284,101,-284,33,-284,104,-284,2,-284,12,-284,99,-284,32,-284,85,-284,84,-284,83,-284,82,-284,81,-284,86,-284,76,-284,51,-284,58,-284,141,-284,143,-284,161,-284,80,-284,78,-284,160,-284,87,-284,45,-284,42,-284,21,-284,22,-284,144,-284,147,-284,145,-284,146,-284,155,-284,158,-284,157,-284,156,-284,11,-284,57,-284,91,-284,40,-284,25,-284,97,-284,54,-284,35,-284,55,-284,102,-284,47,-284,36,-284,53,-284,60,-284,74,-284,72,-284,38,-284,71,-284,127,-284,110,-284},new int[]{-204,197});
    states[197] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285},new int[]{-112,198,-189,352,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283});
    states[198] = new State(new int[]{8,199,118,-286,117,-286,131,-286,132,-286,133,-286,134,-286,130,-286,6,-286,116,-286,115,-286,128,-286,129,-286,13,-286,121,-286,100,-286,120,-286,9,-286,10,-286,138,-286,136,-286,126,-286,5,-286,125,-286,123,-286,124,-286,122,-286,137,-286,135,-286,16,-286,70,-286,92,-286,98,-286,101,-286,33,-286,104,-286,2,-286,12,-286,99,-286,32,-286,85,-286,84,-286,83,-286,82,-286,81,-286,86,-286,76,-286,51,-286,58,-286,141,-286,143,-286,161,-286,80,-286,78,-286,160,-286,87,-286,45,-286,42,-286,21,-286,22,-286,144,-286,147,-286,145,-286,146,-286,155,-286,158,-286,157,-286,156,-286,11,-286,57,-286,91,-286,40,-286,25,-286,97,-286,54,-286,35,-286,55,-286,102,-286,47,-286,36,-286,53,-286,60,-286,74,-286,72,-286,38,-286,71,-286,127,-286,110,-286});
    states[199] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,9,-217},new int[]{-80,200,-78,202,-99,1509,-95,205,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[200] = new State(new int[]{9,201});
    states[201] = new State(-291);
    states[202] = new State(new int[]{100,203,9,-216,12,-216});
    states[203] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-99,204,-95,205,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[204] = new State(-219);
    states[205] = new State(new int[]{13,206,16,210,6,1503,100,-220,9,-220,12,-220,5,-220});
    states[206] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,207,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[207] = new State(new int[]{5,208,13,206,16,210});
    states[208] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,209,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[209] = new State(new int[]{13,206,16,210,6,-154,100,-154,9,-154,12,-154,5,-154,92,-154,10,-154,98,-154,101,-154,33,-154,104,-154,2,-154,99,-154,32,-154,85,-154,84,-154,83,-154,82,-154,81,-154,86,-154});
    states[210] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-96,211,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849});
    states[211] = new State(new int[]{120,237,125,238,123,239,121,240,124,241,122,242,137,243,135,244,13,-153,16,-153,6,-153,100,-153,9,-153,12,-153,5,-153,92,-153,10,-153,98,-153,101,-153,33,-153,104,-153,2,-153,99,-153,32,-153,85,-153,84,-153,83,-153,82,-153,81,-153,86,-153},new int[]{-201,212});
    states[212] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-86,213,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849});
    states[213] = new State(new int[]{116,247,115,248,128,249,129,250,120,-150,125,-150,123,-150,121,-150,124,-150,122,-150,137,-150,135,-150,13,-150,16,-150,6,-150,100,-150,9,-150,12,-150,5,-150,92,-150,10,-150,98,-150,101,-150,33,-150,104,-150,2,-150,99,-150,32,-150,85,-150,84,-150,83,-150,82,-150,81,-150,86,-150},new int[]{-202,214});
    states[214] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-18,215,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849});
    states[215] = new State(new int[]{136,252,138,253,118,254,117,255,131,256,132,257,133,258,134,259,130,260,116,-164,115,-164,128,-164,129,-164,120,-164,125,-164,123,-164,121,-164,124,-164,122,-164,137,-164,135,-164,13,-164,16,-164,6,-164,100,-164,9,-164,12,-164,5,-164,92,-164,10,-164,98,-164,101,-164,33,-164,104,-164,2,-164,99,-164,32,-164,85,-164,84,-164,83,-164,82,-164,81,-164,86,-164},new int[]{-210,216,-204,219});
    states[216] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-293,217,-189,181,-152,218,-156,49,-157,52});
    states[217] = new State(-169);
    states[218] = new State(-292);
    states[219] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-15,220,-279,221,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-16,849});
    states[220] = new State(-176);
    states[221] = new State(-177);
    states[222] = new State(new int[]{4,224,11,226,7,826,142,828,8,829,136,-187,138,-187,118,-187,117,-187,131,-187,132,-187,133,-187,134,-187,130,-187,116,-187,115,-187,128,-187,129,-187,120,-187,125,-187,123,-187,121,-187,124,-187,122,-187,137,-187,135,-187,13,-187,16,-187,6,-187,100,-187,9,-187,12,-187,5,-187,92,-187,10,-187,98,-187,101,-187,33,-187,104,-187,2,-187,99,-187,32,-187,85,-187,84,-187,83,-187,82,-187,81,-187,86,-187,119,-185},new int[]{-17,223});
    states[223] = new State(-207);
    states[224] = new State(new int[]{123,187},new int[]{-308,225});
    states[225] = new State(-208);
    states[226] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,5,1505,12,-217},new int[]{-126,227,-80,229,-95,231,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855,-78,202,-99,1509});
    states[227] = new State(new int[]{12,228});
    states[228] = new State(-209);
    states[229] = new State(new int[]{12,230});
    states[230] = new State(-213);
    states[231] = new State(new int[]{5,232,13,206,16,210,6,1503,100,-220,12,-220});
    states[232] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,5,-739,12,-739},new int[]{-127,233,-95,1502,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[233] = new State(new int[]{5,234,12,-744});
    states[234] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,235,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[235] = new State(new int[]{13,206,16,210,12,-746});
    states[236] = new State(new int[]{120,237,125,238,123,239,121,240,124,241,122,242,137,243,135,244,13,-151,16,-151,6,-151,100,-151,9,-151,12,-151,5,-151,92,-151,10,-151,98,-151,101,-151,33,-151,104,-151,2,-151,99,-151,32,-151,85,-151,84,-151,83,-151,82,-151,81,-151,86,-151},new int[]{-201,212});
    states[237] = new State(-155);
    states[238] = new State(-156);
    states[239] = new State(-157);
    states[240] = new State(-158);
    states[241] = new State(-159);
    states[242] = new State(-160);
    states[243] = new State(-161);
    states[244] = new State(new int[]{137,245});
    states[245] = new State(-162);
    states[246] = new State(new int[]{116,247,115,248,128,249,129,250,120,-149,125,-149,123,-149,121,-149,124,-149,122,-149,137,-149,135,-149,13,-149,16,-149,6,-149,100,-149,9,-149,12,-149,5,-149,92,-149,10,-149,98,-149,101,-149,33,-149,104,-149,2,-149,99,-149,32,-149,85,-149,84,-149,83,-149,82,-149,81,-149,86,-149},new int[]{-202,214});
    states[247] = new State(-165);
    states[248] = new State(-166);
    states[249] = new State(-167);
    states[250] = new State(-168);
    states[251] = new State(new int[]{136,252,138,253,118,254,117,255,131,256,132,257,133,258,134,259,130,260,116,-163,115,-163,128,-163,129,-163,120,-163,125,-163,123,-163,121,-163,124,-163,122,-163,137,-163,135,-163,13,-163,16,-163,6,-163,100,-163,9,-163,12,-163,5,-163,92,-163,10,-163,98,-163,101,-163,33,-163,104,-163,2,-163,99,-163,32,-163,85,-163,84,-163,83,-163,82,-163,81,-163,86,-163},new int[]{-210,216,-204,219});
    states[252] = new State(-766);
    states[253] = new State(-767);
    states[254] = new State(-178);
    states[255] = new State(-179);
    states[256] = new State(-180);
    states[257] = new State(-181);
    states[258] = new State(-182);
    states[259] = new State(-183);
    states[260] = new State(-184);
    states[261] = new State(-173);
    states[262] = new State(-201);
    states[263] = new State(new int[]{26,1491,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,-891,17,-891,8,-891,7,-891,142,-891,4,-891,15,-891,110,-891,111,-891,112,-891,113,-891,114,-891,92,-891,10,-891,5,-891,98,-891,101,-891,33,-891,104,-891,2,-891,127,-891,138,-891,136,-891,118,-891,117,-891,131,-891,132,-891,133,-891,134,-891,130,-891,116,-891,115,-891,128,-891,129,-891,126,-891,6,-891,120,-891,125,-891,123,-891,121,-891,124,-891,122,-891,137,-891,135,-891,16,-891,70,-891,9,-891,12,-891,100,-891,99,-891,32,-891,84,-891,83,-891,82,-891,81,-891,13,-891,119,-891,76,-891,51,-891,58,-891,141,-891,45,-891,42,-891,21,-891,22,-891,144,-891,147,-891,145,-891,146,-891,155,-891,158,-891,157,-891,156,-891,57,-891,91,-891,40,-891,25,-891,97,-891,54,-891,35,-891,55,-891,102,-891,47,-891,36,-891,53,-891,60,-891,74,-891,72,-891,38,-891,71,-891},new int[]{-293,264,-189,181,-152,218,-156,49,-157,52});
    states[264] = new State(new int[]{11,266,8,1488,92,-675,10,-675,98,-675,101,-675,33,-675,104,-675,2,-675,138,-675,136,-675,118,-675,117,-675,131,-675,132,-675,133,-675,134,-675,130,-675,116,-675,115,-675,128,-675,129,-675,126,-675,6,-675,5,-675,120,-675,125,-675,123,-675,121,-675,124,-675,122,-675,137,-675,135,-675,16,-675,70,-675,9,-675,12,-675,100,-675,99,-675,32,-675,85,-675,84,-675,83,-675,82,-675,81,-675,86,-675,13,-675,76,-675,51,-675,58,-675,141,-675,143,-675,161,-675,80,-675,78,-675,160,-675,87,-675,45,-675,42,-675,21,-675,22,-675,144,-675,147,-675,145,-675,146,-675,155,-675,158,-675,157,-675,156,-675,57,-675,91,-675,40,-675,25,-675,97,-675,54,-675,35,-675,55,-675,102,-675,47,-675,36,-675,53,-675,60,-675,74,-675,72,-675,38,-675,71,-675},new int[]{-75,265});
    states[265] = new State(-666);
    states[266] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,12,-843},new int[]{-71,267,-76,640,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,834,-105,659,-334,677});
    states[267] = new State(new int[]{12,268});
    states[268] = new State(new int[]{8,270,92,-665,10,-665,98,-665,101,-665,33,-665,104,-665,2,-665,138,-665,136,-665,118,-665,117,-665,131,-665,132,-665,133,-665,134,-665,130,-665,116,-665,115,-665,128,-665,129,-665,126,-665,6,-665,5,-665,120,-665,125,-665,123,-665,121,-665,124,-665,122,-665,137,-665,135,-665,16,-665,70,-665,9,-665,12,-665,100,-665,99,-665,32,-665,85,-665,84,-665,83,-665,82,-665,81,-665,86,-665,13,-665,76,-665,51,-665,58,-665,141,-665,143,-665,161,-665,80,-665,78,-665,160,-665,87,-665,45,-665,42,-665,21,-665,22,-665,144,-665,147,-665,145,-665,146,-665,155,-665,158,-665,157,-665,156,-665,11,-665,57,-665,91,-665,40,-665,25,-665,97,-665,54,-665,35,-665,55,-665,102,-665,47,-665,36,-665,53,-665,60,-665,74,-665,72,-665,38,-665,71,-665},new int[]{-10,269});
    states[269] = new State(-667);
    states[270] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339,9,-230},new int[]{-70,271,-69,273,-90,1003,-89,276,-95,277,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855,-100,1004,-253,1005});
    states[271] = new State(new int[]{9,272});
    states[272] = new State(-664);
    states[273] = new State(new int[]{100,274,9,-231});
    states[274] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339},new int[]{-90,275,-89,276,-95,277,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855,-100,1004,-253,1005});
    states[275] = new State(-233);
    states[276] = new State(-449);
    states[277] = new State(new int[]{13,206,16,210,100,-226,9,-226,92,-226,10,-226,98,-226,101,-226,33,-226,104,-226,2,-226,12,-226,99,-226,32,-226,85,-226,84,-226,83,-226,82,-226,81,-226,86,-226});
    states[278] = new State(-202);
    states[279] = new State(new int[]{144,281,147,282,7,-854,11,-854,17,-854,138,-854,136,-854,118,-854,117,-854,131,-854,132,-854,133,-854,134,-854,130,-854,116,-854,115,-854,128,-854,129,-854,126,-854,6,-854,5,-854,120,-854,125,-854,123,-854,121,-854,124,-854,122,-854,137,-854,135,-854,16,-854,70,-854,92,-854,10,-854,98,-854,101,-854,33,-854,104,-854,2,-854,9,-854,12,-854,100,-854,99,-854,32,-854,85,-854,84,-854,83,-854,82,-854,81,-854,86,-854,13,-854,119,-854,76,-854,51,-854,58,-854,141,-854,143,-854,161,-854,80,-854,78,-854,160,-854,87,-854,45,-854,42,-854,8,-854,21,-854,22,-854,145,-854,146,-854,155,-854,158,-854,157,-854,156,-854,57,-854,91,-854,40,-854,25,-854,97,-854,54,-854,35,-854,55,-854,102,-854,47,-854,36,-854,53,-854,60,-854,74,-854,72,-854,38,-854,71,-854,127,-854,110,-854,4,-854,142,-854},new int[]{-175,280});
    states[280] = new State(-858);
    states[281] = new State(-852);
    states[282] = new State(-853);
    states[283] = new State(-857);
    states[284] = new State(-855);
    states[285] = new State(-856);
    states[286] = new State(-203);
    states[287] = new State(-222);
    states[288] = new State(-223);
    states[289] = new State(-224);
    states[290] = new State(-225);
    states[291] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,292,-156,49,-157,52});
    states[292] = new State(-204);
    states[293] = new State(-205);
    states[294] = new State(new int[]{8,295});
    states[295] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-293,296,-189,181,-152,218,-156,49,-157,52});
    states[296] = new State(new int[]{9,297});
    states[297] = new State(-652);
    states[298] = new State(-206);
    states[299] = new State(new int[]{8,300});
    states[300] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-293,301,-292,303,-189,305,-152,218,-156,49,-157,52});
    states[301] = new State(new int[]{9,302});
    states[302] = new State(-653);
    states[303] = new State(new int[]{9,304});
    states[304] = new State(-654);
    states[305] = new State(new int[]{7,182,4,306,123,308,125,1486,9,-659},new int[]{-308,184,-309,1487});
    states[306] = new State(new int[]{123,308,125,1486},new int[]{-308,186,-309,307});
    states[307] = new State(-658);
    states[308] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,24,494,48,502,49,549,34,553,73,557,44,563,37,604,121,-274,100,-274},new int[]{-306,188,-307,309,-289,313,-282,192,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-291,1470,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,1471,-233,561,-232,562,-310,1472,-290,1485});
    states[309] = new State(new int[]{121,310,100,311});
    states[310] = new State(-269);
    states[311] = new State(-274,new int[]{-290,312});
    states[312] = new State(-273);
    states[313] = new State(-270);
    states[314] = new State(new int[]{118,254,117,255,131,256,132,257,133,258,134,259,130,260,6,-283,116,-283,115,-283,128,-283,129,-283,13,-283,121,-283,100,-283,120,-283,9,-283,10,-283,8,-283,138,-283,136,-283,126,-283,5,-283,125,-283,123,-283,124,-283,122,-283,137,-283,135,-283,16,-283,70,-283,92,-283,98,-283,101,-283,33,-283,104,-283,2,-283,12,-283,99,-283,32,-283,85,-283,84,-283,83,-283,82,-283,81,-283,86,-283,76,-283,51,-283,58,-283,141,-283,143,-283,161,-283,80,-283,78,-283,160,-283,87,-283,45,-283,42,-283,21,-283,22,-283,144,-283,147,-283,145,-283,146,-283,155,-283,158,-283,157,-283,156,-283,11,-283,57,-283,91,-283,40,-283,25,-283,97,-283,54,-283,35,-283,55,-283,102,-283,47,-283,36,-283,53,-283,60,-283,74,-283,72,-283,38,-283,71,-283,127,-283,110,-283},new int[]{-204,197});
    states[315] = new State(new int[]{8,199,118,-285,117,-285,131,-285,132,-285,133,-285,134,-285,130,-285,6,-285,116,-285,115,-285,128,-285,129,-285,13,-285,121,-285,100,-285,120,-285,9,-285,10,-285,138,-285,136,-285,126,-285,5,-285,125,-285,123,-285,124,-285,122,-285,137,-285,135,-285,16,-285,70,-285,92,-285,98,-285,101,-285,33,-285,104,-285,2,-285,12,-285,99,-285,32,-285,85,-285,84,-285,83,-285,82,-285,81,-285,86,-285,76,-285,51,-285,58,-285,141,-285,143,-285,161,-285,80,-285,78,-285,160,-285,87,-285,45,-285,42,-285,21,-285,22,-285,144,-285,147,-285,145,-285,146,-285,155,-285,158,-285,157,-285,156,-285,11,-285,57,-285,91,-285,40,-285,25,-285,97,-285,54,-285,35,-285,55,-285,102,-285,47,-285,36,-285,53,-285,60,-285,74,-285,72,-285,38,-285,71,-285,127,-285,110,-285});
    states[316] = new State(new int[]{7,182,127,317,123,187,8,-287,118,-287,117,-287,131,-287,132,-287,133,-287,134,-287,130,-287,6,-287,116,-287,115,-287,128,-287,129,-287,13,-287,121,-287,100,-287,120,-287,9,-287,10,-287,138,-287,136,-287,126,-287,5,-287,125,-287,124,-287,122,-287,137,-287,135,-287,16,-287,70,-287,92,-287,98,-287,101,-287,33,-287,104,-287,2,-287,12,-287,99,-287,32,-287,85,-287,84,-287,83,-287,82,-287,81,-287,86,-287,76,-287,51,-287,58,-287,141,-287,143,-287,161,-287,80,-287,78,-287,160,-287,87,-287,45,-287,42,-287,21,-287,22,-287,144,-287,147,-287,145,-287,146,-287,155,-287,158,-287,157,-287,156,-287,11,-287,57,-287,91,-287,40,-287,25,-287,97,-287,54,-287,35,-287,55,-287,102,-287,47,-287,36,-287,53,-287,60,-287,74,-287,72,-287,38,-287,71,-287,110,-287},new int[]{-308,636});
    states[317] = new State(new int[]{8,319,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-289,318,-282,192,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-291,1470,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,1471,-233,561,-232,562,-310,1472});
    states[318] = new State(-320);
    states[319] = new State(new int[]{9,320,143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,325,-83,331,-286,332,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[320] = new State(new int[]{127,321,121,-324,100,-324,120,-324,9,-324,10,-324,8,-324,138,-324,136,-324,118,-324,117,-324,131,-324,132,-324,133,-324,134,-324,130,-324,116,-324,115,-324,128,-324,129,-324,126,-324,6,-324,5,-324,125,-324,123,-324,124,-324,122,-324,137,-324,135,-324,16,-324,70,-324,92,-324,98,-324,101,-324,33,-324,104,-324,2,-324,12,-324,99,-324,32,-324,85,-324,84,-324,83,-324,82,-324,81,-324,86,-324,13,-324,76,-324,51,-324,58,-324,141,-324,143,-324,161,-324,80,-324,78,-324,160,-324,87,-324,45,-324,42,-324,21,-324,22,-324,144,-324,147,-324,145,-324,146,-324,155,-324,158,-324,157,-324,156,-324,11,-324,57,-324,91,-324,40,-324,25,-324,97,-324,54,-324,35,-324,55,-324,102,-324,47,-324,36,-324,53,-324,60,-324,74,-324,72,-324,38,-324,71,-324,110,-324});
    states[321] = new State(new int[]{8,323,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-289,322,-282,192,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-291,1470,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,1471,-233,561,-232,562,-310,1472});
    states[322] = new State(-322);
    states[323] = new State(new int[]{9,324,143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,325,-83,331,-286,332,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[324] = new State(new int[]{127,321,121,-326,100,-326,120,-326,9,-326,10,-326,8,-326,138,-326,136,-326,118,-326,117,-326,131,-326,132,-326,133,-326,134,-326,130,-326,116,-326,115,-326,128,-326,129,-326,126,-326,6,-326,5,-326,125,-326,123,-326,124,-326,122,-326,137,-326,135,-326,16,-326,70,-326,92,-326,98,-326,101,-326,33,-326,104,-326,2,-326,12,-326,99,-326,32,-326,85,-326,84,-326,83,-326,82,-326,81,-326,86,-326,13,-326,76,-326,51,-326,58,-326,141,-326,143,-326,161,-326,80,-326,78,-326,160,-326,87,-326,45,-326,42,-326,21,-326,22,-326,144,-326,147,-326,145,-326,146,-326,155,-326,158,-326,157,-326,156,-326,11,-326,57,-326,91,-326,40,-326,25,-326,97,-326,54,-326,35,-326,55,-326,102,-326,47,-326,36,-326,53,-326,60,-326,74,-326,72,-326,38,-326,71,-326,110,-326});
    states[325] = new State(new int[]{9,326,100,359});
    states[326] = new State(new int[]{127,327,13,-282,121,-282,100,-282,120,-282,9,-282,10,-282,8,-282,138,-282,136,-282,118,-282,117,-282,131,-282,132,-282,133,-282,134,-282,130,-282,116,-282,115,-282,128,-282,129,-282,126,-282,6,-282,5,-282,125,-282,123,-282,124,-282,122,-282,137,-282,135,-282,16,-282,70,-282,92,-282,98,-282,101,-282,33,-282,104,-282,2,-282,12,-282,99,-282,32,-282,85,-282,84,-282,83,-282,82,-282,81,-282,86,-282,76,-282,51,-282,58,-282,141,-282,143,-282,161,-282,80,-282,78,-282,160,-282,87,-282,45,-282,42,-282,21,-282,22,-282,144,-282,147,-282,145,-282,146,-282,155,-282,158,-282,157,-282,156,-282,11,-282,57,-282,91,-282,40,-282,25,-282,97,-282,54,-282,35,-282,55,-282,102,-282,47,-282,36,-282,53,-282,60,-282,74,-282,72,-282,38,-282,71,-282,110,-282});
    states[327] = new State(new int[]{8,329,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-289,328,-282,192,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-291,1470,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,1471,-233,561,-232,562,-310,1472});
    states[328] = new State(-323);
    states[329] = new State(new int[]{9,330,143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,325,-83,331,-286,332,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[330] = new State(new int[]{127,321,121,-327,100,-327,120,-327,9,-327,10,-327,8,-327,138,-327,136,-327,118,-327,117,-327,131,-327,132,-327,133,-327,134,-327,130,-327,116,-327,115,-327,128,-327,129,-327,126,-327,6,-327,5,-327,125,-327,123,-327,124,-327,122,-327,137,-327,135,-327,16,-327,70,-327,92,-327,98,-327,101,-327,33,-327,104,-327,2,-327,12,-327,99,-327,32,-327,85,-327,84,-327,83,-327,82,-327,81,-327,86,-327,13,-327,76,-327,51,-327,58,-327,141,-327,143,-327,161,-327,80,-327,78,-327,160,-327,87,-327,45,-327,42,-327,21,-327,22,-327,144,-327,147,-327,145,-327,146,-327,155,-327,158,-327,157,-327,156,-327,11,-327,57,-327,91,-327,40,-327,25,-327,97,-327,54,-327,35,-327,55,-327,102,-327,47,-327,36,-327,53,-327,60,-327,74,-327,72,-327,38,-327,71,-327,110,-327});
    states[331] = new State(-294);
    states[332] = new State(new int[]{120,333,9,-296,100,-296});
    states[333] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,334,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[334] = new State(new int[]{70,27,9,-297,100,-297});
    states[335] = new State(-777);
    states[336] = new State(-803);
    states[337] = new State(-804);
    states[338] = new State(-797);
    states[339] = new State(new int[]{8,340});
    states[340] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,356},new int[]{-294,341,-293,343,-189,344,-152,218,-156,49,-157,52,-283,346,-282,347,-98,194,-111,314,-112,315,-21,349,-208,350,-174,355,-176,279,-175,283,-310,1484});
    states[341] = new State(new int[]{9,342});
    states[342] = new State(-791);
    states[343] = new State(-662);
    states[344] = new State(new int[]{7,182,4,185,123,187,9,-659,8,-287,118,-287,117,-287,131,-287,132,-287,133,-287,134,-287,130,-287,6,-287,116,-287,115,-287,128,-287,129,-287,13,-287},new int[]{-308,345});
    states[345] = new State(new int[]{9,-660,13,-266});
    states[346] = new State(-663);
    states[347] = new State(new int[]{13,348});
    states[348] = new State(-257);
    states[349] = new State(-288);
    states[350] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285},new int[]{-112,351,-189,352,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283});
    states[351] = new State(new int[]{8,199,118,-289,117,-289,131,-289,132,-289,133,-289,134,-289,130,-289,6,-289,116,-289,115,-289,128,-289,129,-289,13,-289,121,-289,100,-289,120,-289,9,-289,10,-289,138,-289,136,-289,126,-289,5,-289,125,-289,123,-289,124,-289,122,-289,137,-289,135,-289,16,-289,70,-289,92,-289,98,-289,101,-289,33,-289,104,-289,2,-289,12,-289,99,-289,32,-289,85,-289,84,-289,83,-289,82,-289,81,-289,86,-289,76,-289,51,-289,58,-289,141,-289,143,-289,161,-289,80,-289,78,-289,160,-289,87,-289,45,-289,42,-289,21,-289,22,-289,144,-289,147,-289,145,-289,146,-289,155,-289,158,-289,157,-289,156,-289,11,-289,57,-289,91,-289,40,-289,25,-289,97,-289,54,-289,35,-289,55,-289,102,-289,47,-289,36,-289,53,-289,60,-289,74,-289,72,-289,38,-289,71,-289,127,-289,110,-289});
    states[352] = new State(new int[]{7,182,8,-287,118,-287,117,-287,131,-287,132,-287,133,-287,134,-287,130,-287,6,-287,116,-287,115,-287,128,-287,129,-287,13,-287,121,-287,100,-287,120,-287,9,-287,10,-287,138,-287,136,-287,126,-287,5,-287,125,-287,123,-287,124,-287,122,-287,137,-287,135,-287,16,-287,70,-287,92,-287,98,-287,101,-287,33,-287,104,-287,2,-287,12,-287,99,-287,32,-287,85,-287,84,-287,83,-287,82,-287,81,-287,86,-287,76,-287,51,-287,58,-287,141,-287,143,-287,161,-287,80,-287,78,-287,160,-287,87,-287,45,-287,42,-287,21,-287,22,-287,144,-287,147,-287,145,-287,146,-287,155,-287,158,-287,157,-287,156,-287,11,-287,57,-287,91,-287,40,-287,25,-287,97,-287,54,-287,35,-287,55,-287,102,-287,47,-287,36,-287,53,-287,60,-287,74,-287,72,-287,38,-287,71,-287,127,-287,110,-287});
    states[353] = new State(-199);
    states[354] = new State(-200);
    states[355] = new State(-290);
    states[356] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,357,-83,331,-286,332,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[357] = new State(new int[]{9,358,100,359});
    states[358] = new State(-282);
    states[359] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-83,360,-286,332,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[360] = new State(-295);
    states[361] = new State(new int[]{13,348,120,-259,9,-259,100,-259,10,-259,121,-259,8,-259,138,-259,136,-259,118,-259,117,-259,131,-259,132,-259,133,-259,134,-259,130,-259,116,-259,115,-259,128,-259,129,-259,126,-259,6,-259,5,-259,125,-259,123,-259,124,-259,122,-259,137,-259,135,-259,16,-259,70,-259,92,-259,98,-259,101,-259,33,-259,104,-259,2,-259,12,-259,99,-259,32,-259,85,-259,84,-259,83,-259,82,-259,81,-259,86,-259,76,-259,51,-259,58,-259,141,-259,143,-259,161,-259,80,-259,78,-259,160,-259,87,-259,45,-259,42,-259,21,-259,22,-259,144,-259,147,-259,145,-259,146,-259,155,-259,158,-259,157,-259,156,-259,11,-259,57,-259,91,-259,40,-259,25,-259,97,-259,54,-259,35,-259,55,-259,102,-259,47,-259,36,-259,53,-259,60,-259,74,-259,72,-259,38,-259,71,-259,127,-259,110,-259});
    states[362] = new State(new int[]{11,363,7,-866,127,-866,123,-866,8,-866,118,-866,117,-866,131,-866,132,-866,133,-866,134,-866,130,-866,6,-866,116,-866,115,-866,128,-866,129,-866,13,-866,120,-866,9,-866,100,-866,10,-866,121,-866,138,-866,136,-866,126,-866,5,-866,125,-866,124,-866,122,-866,137,-866,135,-866,16,-866,70,-866,92,-866,98,-866,101,-866,33,-866,104,-866,2,-866,12,-866,99,-866,32,-866,85,-866,84,-866,83,-866,82,-866,81,-866,86,-866,76,-866,51,-866,58,-866,141,-866,143,-866,161,-866,80,-866,78,-866,160,-866,87,-866,45,-866,42,-866,21,-866,22,-866,144,-866,147,-866,145,-866,146,-866,155,-866,158,-866,157,-866,156,-866,57,-866,91,-866,40,-866,25,-866,97,-866,54,-866,35,-866,55,-866,102,-866,47,-866,36,-866,53,-866,60,-866,74,-866,72,-866,38,-866,71,-866,110,-866});
    states[363] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,364,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[364] = new State(new int[]{12,365,13,206,16,210});
    states[365] = new State(-315);
    states[366] = new State(-188);
    states[367] = new State(-197);
    states[368] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,12,-847},new int[]{-73,369,-82,371,-97,449,-92,374,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[369] = new State(new int[]{12,370});
    states[370] = new State(-196);
    states[371] = new State(new int[]{100,372,12,-846,76,-846});
    states[372] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-97,373,-92,374,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[373] = new State(-849);
    states[374] = new State(new int[]{70,27,6,375,100,-850,12,-850,76,-850});
    states[375] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,376,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[376] = new State(new int[]{70,27,100,-851,12,-851,76,-851});
    states[377] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-101,378,-20,379,-174,336,-176,279,-175,283,-21,337,-61,338,-208,389,-118,390,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458});
    states[378] = new State(-798);
    states[379] = new State(new int[]{7,44,11,150,17,380,138,-796,136,-796,118,-796,117,-796,131,-796,132,-796,133,-796,134,-796,130,-796,116,-796,115,-796,128,-796,129,-796,126,-796,6,-796,5,-796,120,-796,125,-796,123,-796,121,-796,124,-796,122,-796,137,-796,135,-796,16,-796,70,-796,92,-796,10,-796,98,-796,101,-796,33,-796,104,-796,2,-796,9,-796,12,-796,100,-796,99,-796,32,-796,85,-796,84,-796,83,-796,82,-796,81,-796,86,-796,13,-796,76,-796,51,-796,58,-796,141,-796,143,-796,161,-796,80,-796,78,-796,160,-796,87,-796,45,-796,42,-796,8,-796,21,-796,22,-796,144,-796,147,-796,145,-796,146,-796,155,-796,158,-796,157,-796,156,-796,57,-796,91,-796,40,-796,25,-796,97,-796,54,-796,35,-796,55,-796,102,-796,47,-796,36,-796,53,-796,60,-796,74,-796,72,-796,38,-796,71,-796});
    states[380] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,5,598},new int[]{-125,381,-110,383,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,602,-277,578});
    states[381] = new State(new int[]{12,382});
    states[382] = new State(-834);
    states[383] = new State(new int[]{5,167,6,34});
    states[384] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-101,385,-278,386,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-102,461});
    states[385] = new State(-799);
    states[386] = new State(-776);
    states[387] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-101,388,-20,379,-174,336,-176,279,-175,283,-21,337,-61,338,-208,389,-118,390,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458});
    states[388] = new State(-800);
    states[389] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-101,385,-20,379,-174,336,-176,279,-175,283,-21,337,-61,338,-208,389,-118,390,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458});
    states[390] = new State(-801);
    states[391] = new State(new int[]{141,1483,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,1113,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368},new int[]{-116,392,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728});
    states[392] = new State(new int[]{11,393,17,400,8,702,7,1108,142,1110,4,1111,110,-807,111,-807,112,-807,113,-807,114,-807,92,-807,10,-807,98,-807,101,-807,33,-807,104,-807,2,-807,138,-807,136,-807,118,-807,117,-807,131,-807,132,-807,133,-807,134,-807,130,-807,116,-807,115,-807,128,-807,129,-807,126,-807,6,-807,5,-807,120,-807,125,-807,123,-807,121,-807,124,-807,122,-807,137,-807,135,-807,16,-807,70,-807,9,-807,12,-807,100,-807,99,-807,32,-807,85,-807,84,-807,83,-807,82,-807,81,-807,86,-807,13,-807,119,-807,76,-807,51,-807,58,-807,141,-807,143,-807,161,-807,80,-807,78,-807,160,-807,87,-807,45,-807,42,-807,21,-807,22,-807,144,-807,147,-807,145,-807,146,-807,155,-807,158,-807,157,-807,156,-807,57,-807,91,-807,40,-807,25,-807,97,-807,54,-807,35,-807,55,-807,102,-807,47,-807,36,-807,53,-807,60,-807,74,-807,72,-807,38,-807,71,-807});
    states[393] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-76,394,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,834,-105,659,-334,677});
    states[394] = new State(new int[]{12,395,100,153});
    states[395] = new State(-831);
    states[396] = new State(-622);
    states[397] = new State(new int[]{138,-801,136,-801,118,-801,117,-801,131,-801,132,-801,133,-801,134,-801,130,-801,116,-801,115,-801,128,-801,129,-801,126,-801,6,-801,5,-801,120,-801,125,-801,123,-801,121,-801,124,-801,122,-801,137,-801,135,-801,16,-801,70,-801,92,-801,10,-801,98,-801,101,-801,33,-801,104,-801,2,-801,9,-801,12,-801,100,-801,99,-801,32,-801,85,-801,84,-801,83,-801,82,-801,81,-801,86,-801,13,-801,76,-801,51,-801,58,-801,141,-801,143,-801,161,-801,80,-801,78,-801,160,-801,87,-801,45,-801,42,-801,8,-801,21,-801,22,-801,144,-801,147,-801,145,-801,146,-801,155,-801,158,-801,157,-801,156,-801,11,-801,57,-801,91,-801,40,-801,25,-801,97,-801,54,-801,35,-801,55,-801,102,-801,47,-801,36,-801,53,-801,60,-801,74,-801,72,-801,38,-801,71,-801,119,-794});
    states[398] = new State(-811);
    states[399] = new State(new int[]{11,393,17,400,8,702,7,1108,142,1110,4,1111,15,1118,110,-808,111,-808,112,-808,113,-808,114,-808,92,-808,10,-808,98,-808,101,-808,33,-808,104,-808,2,-808,138,-808,136,-808,118,-808,117,-808,131,-808,132,-808,133,-808,134,-808,130,-808,116,-808,115,-808,128,-808,129,-808,126,-808,6,-808,5,-808,120,-808,125,-808,123,-808,121,-808,124,-808,122,-808,137,-808,135,-808,16,-808,70,-808,9,-808,12,-808,100,-808,99,-808,32,-808,85,-808,84,-808,83,-808,82,-808,81,-808,86,-808,13,-808,119,-808,76,-808,51,-808,58,-808,141,-808,143,-808,161,-808,80,-808,78,-808,160,-808,87,-808,45,-808,42,-808,21,-808,22,-808,144,-808,147,-808,145,-808,146,-808,155,-808,158,-808,157,-808,156,-808,57,-808,91,-808,40,-808,25,-808,97,-808,54,-808,35,-808,55,-808,102,-808,47,-808,36,-808,53,-808,60,-808,74,-808,72,-808,38,-808,71,-808});
    states[400] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,5,598},new int[]{-125,401,-110,383,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,602,-277,578});
    states[401] = new State(new int[]{12,402});
    states[402] = new State(-833);
    states[403] = new State(-823);
    states[404] = new State(-824);
    states[405] = new State(new int[]{115,407,116,408,117,409,118,410,120,411,121,412,122,413,123,414,124,415,125,416,128,417,129,418,130,419,131,420,132,421,133,422,134,423,135,424,137,425,139,426,140,427,110,429,111,430,112,431,113,432,114,433,119,434},new int[]{-209,406,-203,428});
    states[406] = new State(-859);
    states[407] = new State(-983);
    states[408] = new State(-984);
    states[409] = new State(-985);
    states[410] = new State(-986);
    states[411] = new State(-987);
    states[412] = new State(-988);
    states[413] = new State(-989);
    states[414] = new State(-990);
    states[415] = new State(-991);
    states[416] = new State(-992);
    states[417] = new State(-993);
    states[418] = new State(-994);
    states[419] = new State(-995);
    states[420] = new State(-996);
    states[421] = new State(-997);
    states[422] = new State(-998);
    states[423] = new State(-999);
    states[424] = new State(-1000);
    states[425] = new State(-1001);
    states[426] = new State(-1002);
    states[427] = new State(-1003);
    states[428] = new State(-1004);
    states[429] = new State(-1006);
    states[430] = new State(-1007);
    states[431] = new State(-1008);
    states[432] = new State(-1009);
    states[433] = new State(-1010);
    states[434] = new State(-1005);
    states[435] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,436,-156,49,-157,52});
    states[436] = new State(-825);
    states[437] = new State(new int[]{53,687,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-370,440,-107,532,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-369,1104,-105,1105});
    states[438] = new State(new int[]{9,439,70,27});
    states[439] = new State(-826);
    states[440] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,53,1114},new int[]{-92,441,-369,443,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[441] = new State(new int[]{9,442,70,27});
    states[442] = new State(-827);
    states[443] = new State(-821);
    states[444] = new State(-828);
    states[445] = new State(-829);
    states[446] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-73,447,-82,371,-97,449,-92,374,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[447] = new State(new int[]{76,448});
    states[448] = new State(-835);
    states[449] = new State(-848);
    states[450] = new State(-836);
    states[451] = new State(-837);
    states[452] = new State(new int[]{7,453,138,-802,136,-802,118,-802,117,-802,131,-802,132,-802,133,-802,134,-802,130,-802,116,-802,115,-802,128,-802,129,-802,126,-802,6,-802,5,-802,120,-802,125,-802,123,-802,121,-802,124,-802,122,-802,137,-802,135,-802,16,-802,70,-802,92,-802,10,-802,98,-802,101,-802,33,-802,104,-802,2,-802,9,-802,12,-802,100,-802,99,-802,32,-802,85,-802,84,-802,83,-802,82,-802,81,-802,86,-802,13,-802,76,-802,51,-802,58,-802,141,-802,143,-802,161,-802,80,-802,78,-802,160,-802,87,-802,45,-802,42,-802,8,-802,21,-802,22,-802,144,-802,147,-802,145,-802,146,-802,155,-802,158,-802,157,-802,156,-802,11,-802,57,-802,91,-802,40,-802,25,-802,97,-802,54,-802,35,-802,55,-802,102,-802,47,-802,36,-802,53,-802,60,-802,74,-802,72,-802,38,-802,71,-802});
    states[453] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,405},new int[]{-153,454,-152,455,-156,49,-157,52,-302,456,-155,58,-200,457});
    states[454] = new State(-839);
    states[455] = new State(-875);
    states[456] = new State(-876);
    states[457] = new State(-877);
    states[458] = new State(-809);
    states[459] = new State(-778);
    states[460] = new State(-779);
    states[461] = new State(new int[]{119,462});
    states[462] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-101,463,-278,464,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-102,461});
    states[463] = new State(-774);
    states[464] = new State(-775);
    states[465] = new State(-783);
    states[466] = new State(new int[]{8,467,138,-768,136,-768,118,-768,117,-768,131,-768,132,-768,133,-768,134,-768,130,-768,116,-768,115,-768,128,-768,129,-768,126,-768,6,-768,5,-768,120,-768,125,-768,123,-768,121,-768,124,-768,122,-768,137,-768,135,-768,16,-768,70,-768,92,-768,10,-768,98,-768,101,-768,33,-768,104,-768,2,-768,9,-768,12,-768,100,-768,99,-768,32,-768,85,-768,84,-768,83,-768,82,-768,81,-768,86,-768,13,-768,76,-768,51,-768,58,-768,141,-768,143,-768,161,-768,80,-768,78,-768,160,-768,87,-768,45,-768,42,-768,21,-768,22,-768,144,-768,147,-768,145,-768,146,-768,155,-768,158,-768,157,-768,156,-768,11,-768,57,-768,91,-768,40,-768,25,-768,97,-768,54,-768,35,-768,55,-768,102,-768,47,-768,36,-768,53,-768,60,-768,74,-768,72,-768,38,-768,71,-768});
    states[467] = new State(new int[]{14,472,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,474,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-364,468,-362,1482,-20,473,-174,336,-176,279,-175,283,-21,337,-351,1473,-293,1474,-189,181,-152,218,-156,49,-157,52,-354,1480,-355,1481});
    states[468] = new State(new int[]{9,469,10,470,100,1478});
    states[469] = new State(-680);
    states[470] = new State(new int[]{14,472,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,474,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-362,471,-20,473,-174,336,-176,279,-175,283,-21,337,-351,1473,-293,1474,-189,181,-152,218,-156,49,-157,52,-354,1480,-355,1481});
    states[471] = new State(-717);
    states[472] = new State(-719);
    states[473] = new State(-720);
    states[474] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,475,-156,49,-157,52});
    states[475] = new State(new int[]{5,476,9,-722,10,-722,100,-722});
    states[476] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,477,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[477] = new State(-721);
    states[478] = new State(new int[]{9,479,143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,325,-83,331,-286,332,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[479] = new State(new int[]{127,321});
    states[480] = new State(-260);
    states[481] = new State(new int[]{13,482,127,483,120,-265,9,-265,100,-265,10,-265,121,-265,8,-265,138,-265,136,-265,118,-265,117,-265,131,-265,132,-265,133,-265,134,-265,130,-265,116,-265,115,-265,128,-265,129,-265,126,-265,6,-265,5,-265,125,-265,123,-265,124,-265,122,-265,137,-265,135,-265,16,-265,70,-265,92,-265,98,-265,101,-265,33,-265,104,-265,2,-265,12,-265,99,-265,32,-265,85,-265,84,-265,83,-265,82,-265,81,-265,86,-265,76,-265,51,-265,58,-265,141,-265,143,-265,161,-265,80,-265,78,-265,160,-265,87,-265,45,-265,42,-265,21,-265,22,-265,144,-265,147,-265,145,-265,146,-265,155,-265,158,-265,157,-265,156,-265,11,-265,57,-265,91,-265,40,-265,25,-265,97,-265,54,-265,35,-265,55,-265,102,-265,47,-265,36,-265,53,-265,60,-265,74,-265,72,-265,38,-265,71,-265,110,-265});
    states[482] = new State(-258);
    states[483] = new State(new int[]{8,485,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-289,484,-282,192,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-291,1470,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,1471,-233,561,-232,562,-310,1472});
    states[484] = new State(-321);
    states[485] = new State(new int[]{9,486,143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-85,325,-83,331,-286,332,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[486] = new State(new int[]{127,321,121,-325,100,-325,120,-325,9,-325,10,-325,8,-325,138,-325,136,-325,118,-325,117,-325,131,-325,132,-325,133,-325,134,-325,130,-325,116,-325,115,-325,128,-325,129,-325,126,-325,6,-325,5,-325,125,-325,123,-325,124,-325,122,-325,137,-325,135,-325,16,-325,70,-325,92,-325,98,-325,101,-325,33,-325,104,-325,2,-325,12,-325,99,-325,32,-325,85,-325,84,-325,83,-325,82,-325,81,-325,86,-325,13,-325,76,-325,51,-325,58,-325,141,-325,143,-325,161,-325,80,-325,78,-325,160,-325,87,-325,45,-325,42,-325,21,-325,22,-325,144,-325,147,-325,145,-325,146,-325,155,-325,158,-325,157,-325,156,-325,11,-325,57,-325,91,-325,40,-325,25,-325,97,-325,54,-325,35,-325,55,-325,102,-325,47,-325,36,-325,53,-325,60,-325,74,-325,72,-325,38,-325,71,-325,110,-325});
    states[487] = new State(-261);
    states[488] = new State(-262);
    states[489] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,490,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[490] = new State(-298);
    states[491] = new State(-516);
    states[492] = new State(-263);
    states[493] = new State(-299);
    states[494] = new State(new int[]{11,495,58,1468});
    states[495] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,356,12,-311,100,-311},new int[]{-173,496,-281,1467,-282,1466,-98,194,-111,314,-112,315,-189,352,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283});
    states[496] = new State(new int[]{12,497,100,1464});
    states[497] = new State(new int[]{58,498});
    states[498] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,499,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[499] = new State(-305);
    states[500] = new State(-306);
    states[501] = new State(-300);
    states[502] = new State(new int[]{8,1344,23,-347,11,-347,92,-347,84,-347,83,-347,82,-347,81,-347,29,-347,143,-347,161,-347,85,-347,86,-347,80,-347,78,-347,160,-347,87,-347,62,-347,28,-347,26,-347,44,-347,37,-347,19,-347,30,-347,31,-347,46,-347,27,-347},new int[]{-192,503});
    states[503] = new State(new int[]{23,1335,11,-354,92,-354,84,-354,83,-354,82,-354,81,-354,29,-354,143,-354,161,-354,85,-354,86,-354,80,-354,78,-354,160,-354,87,-354,62,-354,28,-354,26,-354,44,-354,37,-354,19,-354,30,-354,31,-354,46,-354,27,-354},new int[]{-328,504,-327,1333,-326,1355});
    states[504] = new State(new int[]{11,628,92,-371,84,-371,83,-371,82,-371,81,-371,29,-244,143,-244,161,-244,85,-244,86,-244,80,-244,78,-244,160,-244,87,-244,62,-244,28,-244,26,-244,44,-244,37,-244,19,-244,30,-244,31,-244,46,-244,27,-244},new int[]{-29,505,-36,1256,-38,509,-49,1257,-11,1258,-260,1155,-37,1420,-58,1422,-57,515,-59,1421});
    states[505] = new State(new int[]{92,506,84,1252,83,1253,82,1254,81,1255},new int[]{-12,507});
    states[506] = new State(-329);
    states[507] = new State(new int[]{11,628,92,-371,84,-371,83,-371,82,-371,81,-371,29,-244,143,-244,161,-244,85,-244,86,-244,80,-244,78,-244,160,-244,87,-244,62,-244,28,-244,26,-244,44,-244,37,-244,19,-244,30,-244,31,-244,46,-244,27,-244},new int[]{-36,508,-38,509,-49,1257,-11,1258,-260,1155,-37,1420,-58,1422,-57,515,-59,1421});
    states[508] = new State(-366);
    states[509] = new State(new int[]{10,511,92,-377,84,-377,83,-377,82,-377,81,-377},new int[]{-199,510});
    states[510] = new State(-372);
    states[511] = new State(new int[]{11,628,92,-378,84,-378,83,-378,82,-378,81,-378,29,-244,143,-244,161,-244,85,-244,86,-244,80,-244,78,-244,160,-244,87,-244,62,-244,28,-244,26,-244,44,-244,37,-244,19,-244,30,-244,31,-244,46,-244,27,-244},new int[]{-49,512,-37,513,-11,1258,-260,1155,-58,1422,-57,515,-59,1421});
    states[512] = new State(-380);
    states[513] = new State(new int[]{11,628,92,-374,84,-374,83,-374,82,-374,81,-374,28,-244,26,-244,44,-244,37,-244,19,-244,30,-244,31,-244,46,-244,27,-244},new int[]{-58,514,-57,515,-11,516,-260,1155,-59,1421});
    states[514] = new State(-383);
    states[515] = new State(-384);
    states[516] = new State(new int[]{28,1380,26,1381,44,1328,37,1363,19,1383,30,1391,31,1398,11,628,46,1405,27,1414},new int[]{-231,517,-260,518,-228,519,-268,520,-8,521,-239,1382,-237,1316,-234,1327,-238,1362,-236,1389,-224,1402,-225,1403,-227,1404});
    states[517] = new State(-393);
    states[518] = new State(-243);
    states[519] = new State(-394);
    states[520] = new State(-410);
    states[521] = new State(new int[]{30,523,19,1198,46,1270,27,1308,44,1328,37,1363},new int[]{-239,522,-225,1197,-237,1316,-234,1327,-238,1362});
    states[522] = new State(-397);
    states[523] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,8,-409,110,-409,10,-409},new int[]{-181,524,-180,1180,-179,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[524] = new State(new int[]{8,565,110,-500,10,-500},new int[]{-133,525});
    states[525] = new State(new int[]{110,527,10,1169},new int[]{-216,526});
    states[526] = new State(-406);
    states[527] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524},new int[]{-270,528,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[528] = new State(new int[]{10,529});
    states[529] = new State(-455);
    states[530] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,92,-604,10,-604,98,-604,101,-604,33,-604,104,-604,2,-604,9,-604,12,-604,100,-604,99,-604,32,-604,84,-604,83,-604,82,-604,81,-604},new int[]{-152,436,-156,49,-157,52});
    states[531] = new State(new int[]{53,1157,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-370,440,-107,532,-116,694,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-369,1104,-105,1105});
    states[532] = new State(new int[]{100,533});
    states[533] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,18,669},new int[]{-84,534,-107,1148,-106,1147,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-105,1105});
    states[534] = new State(new int[]{100,1145,5,546,10,-1040,9,-1040},new int[]{-335,535});
    states[535] = new State(new int[]{10,538,9,-1028},new int[]{-342,536});
    states[536] = new State(new int[]{9,537});
    states[537] = new State(-792);
    states[538] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-337,539,-338,1096,-163,542,-152,809,-156,49,-157,52});
    states[539] = new State(new int[]{10,540,9,-1029});
    states[540] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-338,541,-163,542,-152,809,-156,49,-157,52});
    states[541] = new State(-1038);
    states[542] = new State(new int[]{100,544,5,546,10,-1040,9,-1040},new int[]{-335,543});
    states[543] = new State(-1039);
    states[544] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,545,-156,49,-157,52});
    states[545] = new State(-376);
    states[546] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,547,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[547] = new State(-1041);
    states[548] = new State(-301);
    states[549] = new State(new int[]{58,550});
    states[550] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,551,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[551] = new State(-312);
    states[552] = new State(-302);
    states[553] = new State(new int[]{58,554,121,-314,100,-314,120,-314,9,-314,10,-314,8,-314,138,-314,136,-314,118,-314,117,-314,131,-314,132,-314,133,-314,134,-314,130,-314,116,-314,115,-314,128,-314,129,-314,126,-314,6,-314,5,-314,125,-314,123,-314,124,-314,122,-314,137,-314,135,-314,16,-314,70,-314,92,-314,98,-314,101,-314,33,-314,104,-314,2,-314,12,-314,99,-314,32,-314,85,-314,84,-314,83,-314,82,-314,81,-314,86,-314,13,-314,76,-314,51,-314,141,-314,143,-314,161,-314,80,-314,78,-314,160,-314,87,-314,45,-314,42,-314,21,-314,22,-314,144,-314,147,-314,145,-314,146,-314,155,-314,158,-314,157,-314,156,-314,11,-314,57,-314,91,-314,40,-314,25,-314,97,-314,54,-314,35,-314,55,-314,102,-314,47,-314,36,-314,53,-314,60,-314,74,-314,72,-314,38,-314,71,-314,127,-314,110,-314});
    states[554] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,555,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[555] = new State(-313);
    states[556] = new State(-303);
    states[557] = new State(new int[]{58,558});
    states[558] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,559,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[559] = new State(-304);
    states[560] = new State(-264);
    states[561] = new State(-316);
    states[562] = new State(-317);
    states[563] = new State(new int[]{8,565,121,-500,100,-500,120,-500,9,-500,10,-500,138,-500,136,-500,118,-500,117,-500,131,-500,132,-500,133,-500,134,-500,130,-500,116,-500,115,-500,128,-500,129,-500,126,-500,6,-500,5,-500,125,-500,123,-500,124,-500,122,-500,137,-500,135,-500,16,-500,70,-500,92,-500,98,-500,101,-500,33,-500,104,-500,2,-500,12,-500,99,-500,32,-500,85,-500,84,-500,83,-500,82,-500,81,-500,86,-500,13,-500,76,-500,51,-500,58,-500,141,-500,143,-500,161,-500,80,-500,78,-500,160,-500,87,-500,45,-500,42,-500,21,-500,22,-500,144,-500,147,-500,145,-500,146,-500,155,-500,158,-500,157,-500,156,-500,11,-500,57,-500,91,-500,40,-500,25,-500,97,-500,54,-500,35,-500,55,-500,102,-500,47,-500,36,-500,53,-500,60,-500,74,-500,72,-500,38,-500,71,-500,127,-500,110,-500},new int[]{-133,564});
    states[564] = new State(-318);
    states[565] = new State(new int[]{9,566,11,628,143,-244,161,-244,85,-244,86,-244,80,-244,78,-244,160,-244,87,-244,53,-244,29,-244,108,-244},new int[]{-134,567,-60,1156,-11,571,-260,1155});
    states[566] = new State(-501);
    states[567] = new State(new int[]{9,568,10,569});
    states[568] = new State(-502);
    states[569] = new State(new int[]{11,628,143,-244,161,-244,85,-244,86,-244,80,-244,78,-244,160,-244,87,-244,53,-244,29,-244,108,-244},new int[]{-60,570,-11,571,-260,1155});
    states[570] = new State(-504);
    states[571] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,53,612,29,618,108,624,11,628},new int[]{-305,572,-260,518,-164,573,-140,611,-152,610,-156,49,-157,52});
    states[572] = new State(-505);
    states[573] = new State(new int[]{5,574,100,608});
    states[574] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,575,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[575] = new State(new int[]{110,576,9,-506,10,-506});
    states[576] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,577,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[577] = new State(new int[]{70,27,9,-510,10,-510});
    states[578] = new State(-769);
    states[579] = new State(new int[]{70,-639,92,-639,10,-639,98,-639,101,-639,33,-639,104,-639,2,-639,9,-639,12,-639,100,-639,99,-639,32,-639,85,-639,84,-639,83,-639,82,-639,81,-639,86,-639,6,-639,76,-639,5,-639,51,-639,58,-639,141,-639,143,-639,161,-639,80,-639,78,-639,160,-639,87,-639,45,-639,42,-639,8,-639,21,-639,22,-639,144,-639,147,-639,145,-639,146,-639,155,-639,158,-639,157,-639,156,-639,11,-639,57,-639,91,-639,40,-639,25,-639,97,-639,54,-639,35,-639,55,-639,102,-639,47,-639,36,-639,53,-639,60,-639,74,-639,72,-639,38,-639,71,-639,13,-642});
    states[580] = new State(new int[]{13,581});
    states[581] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-122,582,-104,585,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,587});
    states[582] = new State(new int[]{5,583,13,581});
    states[583] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-122,584,-104,585,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,587});
    states[584] = new State(new int[]{13,581,70,-655,92,-655,10,-655,98,-655,101,-655,33,-655,104,-655,2,-655,9,-655,12,-655,100,-655,99,-655,32,-655,85,-655,84,-655,83,-655,82,-655,81,-655,86,-655,6,-655,76,-655,5,-655,51,-655,58,-655,141,-655,143,-655,161,-655,80,-655,78,-655,160,-655,87,-655,45,-655,42,-655,8,-655,21,-655,22,-655,144,-655,147,-655,145,-655,146,-655,155,-655,158,-655,157,-655,156,-655,11,-655,57,-655,91,-655,40,-655,25,-655,97,-655,54,-655,35,-655,55,-655,102,-655,47,-655,36,-655,53,-655,60,-655,74,-655,72,-655,38,-655,71,-655});
    states[585] = new State(new int[]{16,30,5,-641,13,-641,70,-641,92,-641,10,-641,98,-641,101,-641,33,-641,104,-641,2,-641,9,-641,12,-641,100,-641,99,-641,32,-641,85,-641,84,-641,83,-641,82,-641,81,-641,86,-641,6,-641,76,-641,51,-641,58,-641,141,-641,143,-641,161,-641,80,-641,78,-641,160,-641,87,-641,45,-641,42,-641,8,-641,21,-641,22,-641,144,-641,147,-641,145,-641,146,-641,155,-641,158,-641,157,-641,156,-641,11,-641,57,-641,91,-641,40,-641,25,-641,97,-641,54,-641,35,-641,55,-641,102,-641,47,-641,36,-641,53,-641,60,-641,74,-641,72,-641,38,-641,71,-641});
    states[586] = new State(new int[]{6,34,120,-677,125,-677,123,-677,121,-677,124,-677,122,-677,137,-677,135,-677,16,-677,70,-677,92,-677,10,-677,98,-677,101,-677,33,-677,104,-677,2,-677,9,-677,12,-677,100,-677,99,-677,32,-677,85,-677,84,-677,83,-677,82,-677,81,-677,86,-677,76,-677,13,-677,5,-677,51,-677,58,-677,141,-677,143,-677,161,-677,80,-677,78,-677,160,-677,87,-677,45,-677,42,-677,8,-677,21,-677,22,-677,144,-677,147,-677,145,-677,146,-677,155,-677,158,-677,157,-677,156,-677,11,-677,57,-677,91,-677,40,-677,25,-677,97,-677,54,-677,35,-677,55,-677,102,-677,47,-677,36,-677,53,-677,60,-677,74,-677,72,-677,38,-677,71,-677,116,-677,115,-677,128,-677,129,-677,126,-677,138,-677,136,-677,118,-677,117,-677,131,-677,132,-677,133,-677,134,-677,130,-677});
    states[587] = new State(-642);
    states[588] = new State(-640);
    states[589] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-123,590,-104,595,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-252,596});
    states[590] = new State(new int[]{51,591});
    states[591] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-123,592,-104,595,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-252,596});
    states[592] = new State(new int[]{32,593});
    states[593] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-123,594,-104,595,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-252,596});
    states[594] = new State(-656);
    states[595] = new State(new int[]{16,30,51,-643,32,-643,120,-643,125,-643,123,-643,121,-643,124,-643,122,-643,137,-643,135,-643,70,-643,92,-643,10,-643,98,-643,101,-643,33,-643,104,-643,2,-643,9,-643,12,-643,100,-643,99,-643,85,-643,84,-643,83,-643,82,-643,81,-643,86,-643,13,-643,6,-643,76,-643,5,-643,58,-643,141,-643,143,-643,161,-643,80,-643,78,-643,160,-643,87,-643,45,-643,42,-643,8,-643,21,-643,22,-643,144,-643,147,-643,145,-643,146,-643,155,-643,158,-643,157,-643,156,-643,11,-643,57,-643,91,-643,40,-643,25,-643,97,-643,54,-643,35,-643,55,-643,102,-643,47,-643,36,-643,53,-643,60,-643,74,-643,72,-643,38,-643,71,-643,116,-643,115,-643,128,-643,129,-643,126,-643,138,-643,136,-643,118,-643,117,-643,131,-643,132,-643,133,-643,134,-643,130,-643});
    states[596] = new State(-644);
    states[597] = new State(-636);
    states[598] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,5,-737,70,-737,92,-737,10,-737,98,-737,101,-737,33,-737,104,-737,2,-737,9,-737,12,-737,100,-737,99,-737,32,-737,84,-737,83,-737,82,-737,81,-737,6,-737},new int[]{-120,599,-110,603,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,602,-277,578});
    states[599] = new State(new int[]{5,600,70,-741,92,-741,10,-741,98,-741,101,-741,33,-741,104,-741,2,-741,9,-741,12,-741,100,-741,99,-741,32,-741,85,-741,84,-741,83,-741,82,-741,81,-741,86,-741,6,-741,76,-741});
    states[600] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368},new int[]{-110,601,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,602,-277,578});
    states[601] = new State(new int[]{6,34,70,-743,92,-743,10,-743,98,-743,101,-743,33,-743,104,-743,2,-743,9,-743,12,-743,100,-743,99,-743,32,-743,85,-743,84,-743,83,-743,82,-743,81,-743,86,-743,76,-743});
    states[602] = new State(-768);
    states[603] = new State(new int[]{6,34,5,-736,70,-736,92,-736,10,-736,98,-736,101,-736,33,-736,104,-736,2,-736,9,-736,12,-736,100,-736,99,-736,32,-736,85,-736,84,-736,83,-736,82,-736,81,-736,86,-736,76,-736});
    states[604] = new State(new int[]{8,565,5,-500},new int[]{-133,605});
    states[605] = new State(new int[]{5,606});
    states[606] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,607,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[607] = new State(-319);
    states[608] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-140,609,-152,610,-156,49,-157,52});
    states[609] = new State(-514);
    states[610] = new State(-515);
    states[611] = new State(-513);
    states[612] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-164,613,-140,611,-152,610,-156,49,-157,52});
    states[613] = new State(new int[]{5,614,100,608});
    states[614] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,615,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[615] = new State(new int[]{110,616,9,-507,10,-507});
    states[616] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,617,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[617] = new State(new int[]{70,27,9,-511,10,-511});
    states[618] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-164,619,-140,611,-152,610,-156,49,-157,52});
    states[619] = new State(new int[]{5,620,100,608});
    states[620] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,621,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[621] = new State(new int[]{110,622,9,-508,10,-508});
    states[622] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,623,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[623] = new State(new int[]{70,27,9,-512,10,-512});
    states[624] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-164,625,-140,611,-152,610,-156,49,-157,52});
    states[625] = new State(new int[]{5,626,100,608});
    states[626] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,627,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[627] = new State(-509);
    states[628] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-261,629,-13,1154,-14,633,-189,634,-152,1149,-156,49,-157,52,-310,1152});
    states[629] = new State(new int[]{12,630,100,631});
    states[630] = new State(-245);
    states[631] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-13,632,-14,633,-189,634,-152,1149,-156,49,-157,52,-310,1152});
    states[632] = new State(-247);
    states[633] = new State(-248);
    states[634] = new State(new int[]{7,182,8,637,123,187,12,-673,100,-673},new int[]{-74,635,-308,636});
    states[635] = new State(-813);
    states[636] = new State(-266);
    states[637] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,9,-843},new int[]{-71,638,-76,640,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,834,-105,659,-334,677});
    states[638] = new State(new int[]{9,639});
    states[639] = new State(-674);
    states[640] = new State(new int[]{100,153,12,-842,9,-842});
    states[641] = new State(new int[]{127,642,11,-823,17,-823,8,-823,7,-823,142,-823,4,-823,15,-823,138,-823,136,-823,118,-823,117,-823,131,-823,132,-823,133,-823,134,-823,130,-823,116,-823,115,-823,128,-823,129,-823,126,-823,6,-823,5,-823,120,-823,125,-823,123,-823,121,-823,124,-823,122,-823,137,-823,135,-823,16,-823,70,-823,92,-823,10,-823,98,-823,101,-823,33,-823,104,-823,2,-823,9,-823,12,-823,100,-823,99,-823,32,-823,85,-823,84,-823,83,-823,82,-823,81,-823,86,-823,13,-823,119,-823});
    states[642] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,643,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[643] = new State(-1020);
    states[644] = new State(-1057);
    states[645] = new State(new int[]{16,30,92,-647,10,-647,98,-647,101,-647,33,-647,104,-647,2,-647,9,-647,12,-647,100,-647,99,-647,32,-647,85,-647,84,-647,83,-647,82,-647,81,-647,86,-647,13,-641});
    states[646] = new State(new int[]{53,687,9,714,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,1124,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-370,440,-107,647,-152,1083,-9,1121,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,1123,-137,391,-116,399,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-369,1104,-105,1105});
    states[647] = new State(new int[]{100,648});
    states[648] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,18,669},new int[]{-84,649,-107,1148,-106,1147,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-105,1105});
    states[649] = new State(new int[]{100,1145,5,546,10,-1040,9,-1040},new int[]{-335,650});
    states[650] = new State(new int[]{10,538,9,-1028},new int[]{-342,651});
    states[651] = new State(new int[]{9,652});
    states[652] = new State(new int[]{5,661,7,-792,138,-792,136,-792,118,-792,117,-792,131,-792,132,-792,133,-792,134,-792,130,-792,116,-792,115,-792,128,-792,129,-792,126,-792,6,-792,120,-792,125,-792,123,-792,121,-792,124,-792,122,-792,137,-792,135,-792,16,-792,70,-792,92,-792,10,-792,98,-792,101,-792,33,-792,104,-792,2,-792,9,-792,12,-792,100,-792,99,-792,32,-792,85,-792,84,-792,83,-792,82,-792,81,-792,86,-792,13,-792,127,-1042},new int[]{-346,653,-336,654});
    states[653] = new State(-1025);
    states[654] = new State(new int[]{127,655});
    states[655] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,656,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[656] = new State(-1030);
    states[657] = new State(new int[]{92,-648,10,-648,98,-648,101,-648,33,-648,104,-648,2,-648,9,-648,12,-648,100,-648,99,-648,32,-648,85,-648,84,-648,83,-648,82,-648,81,-648,86,-648,13,-642});
    states[658] = new State(-649);
    states[659] = new State(new int[]{5,661,127,-1042},new int[]{-346,660,-336,654});
    states[660] = new State(-1026);
    states[661] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,356,142,489,24,494,48,502,49,549,34,553,73,557},new int[]{-287,662,-282,663,-98,194,-111,314,-112,315,-189,664,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-266,665,-259,666,-291,667,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-310,668});
    states[662] = new State(-1043);
    states[663] = new State(-517);
    states[664] = new State(new int[]{7,182,123,187,8,-287,118,-287,117,-287,131,-287,132,-287,133,-287,134,-287,130,-287,6,-287,116,-287,115,-287,128,-287,129,-287,127,-287},new int[]{-308,636});
    states[665] = new State(-518);
    states[666] = new State(-519);
    states[667] = new State(-520);
    states[668] = new State(-521);
    states[669] = new State(new int[]{18,669,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-28,670,-27,676,-105,674,-152,675,-156,49,-157,52});
    states[670] = new State(new int[]{100,671});
    states[671] = new State(new int[]{18,669,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-27,672,-105,674,-152,675,-156,49,-157,52});
    states[672] = new State(new int[]{9,673,100,-1015});
    states[673] = new State(-1011);
    states[674] = new State(-1012);
    states[675] = new State(-1013);
    states[676] = new State(-1014);
    states[677] = new State(-1027);
    states[678] = new State(new int[]{8,1135,5,661,127,-1042},new int[]{-336,679});
    states[679] = new State(new int[]{127,680});
    states[680] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,681,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[681] = new State(-1031);
    states[682] = new State(new int[]{127,683,8,1127});
    states[683] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,686,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-340,684,-221,685,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-9,1125,-341,1126,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[684] = new State(-1034);
    states[685] = new State(-1059);
    states[686] = new State(new int[]{53,687,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,1124,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-370,440,-107,532,-116,694,-9,1121,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,1123,-137,391,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-369,1104,-105,1105});
    states[687] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,688,-156,49,-157,52});
    states[688] = new State(new int[]{110,689});
    states[689] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-104,690,-92,692,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-106,155,-250,579,-122,580,-252,588,-125,597});
    states[690] = new State(new int[]{9,691,16,30,10,-638,70,-638,13,-641});
    states[691] = new State(-810);
    states[692] = new State(new int[]{10,693,70,27});
    states[693] = new State(-819);
    states[694] = new State(new int[]{100,695,11,393,17,400,8,702,7,1108,142,1110,4,1111,15,1118,138,-808,136,-808,118,-808,117,-808,131,-808,132,-808,133,-808,134,-808,130,-808,116,-808,115,-808,128,-808,129,-808,126,-808,6,-808,5,-808,120,-808,125,-808,123,-808,121,-808,124,-808,122,-808,137,-808,135,-808,16,-808,9,-808,70,-808,13,-808,119,-808,110,-808,111,-808,112,-808,113,-808,114,-808});
    states[695] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,1113,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368},new int[]{-347,696,-116,1117,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728});
    states[696] = new State(new int[]{9,697,100,700});
    states[697] = new State(new int[]{110,429,111,430,112,431,113,432,114,433},new int[]{-203,698});
    states[698] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,699,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[699] = new State(new int[]{70,27,92,-553,10,-553,98,-553,101,-553,33,-553,104,-553,2,-553,9,-553,12,-553,100,-553,99,-553,32,-553,85,-553,84,-553,83,-553,82,-553,81,-553,86,-553});
    states[700] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,1113,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368},new int[]{-116,701,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728});
    states[701] = new State(new int[]{11,393,17,400,8,702,7,1108,142,1110,4,1111,9,-555,100,-555});
    states[702] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,712,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,9,-845},new int[]{-72,703,-77,705,-94,1107,-92,708,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,709,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,1106,-105,659,-334,677});
    states[703] = new State(new int[]{9,704});
    states[704] = new State(-822);
    states[705] = new State(new int[]{100,706,9,-844});
    states[706] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,712,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-94,707,-92,708,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,709,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,1106,-105,659,-334,677});
    states[707] = new State(-625);
    states[708] = new State(new int[]{70,27,100,-631,9,-631});
    states[709] = new State(new int[]{110,710,127,642,11,-823,17,-823,8,-823,7,-823,142,-823,4,-823,15,-823,138,-823,136,-823,118,-823,117,-823,131,-823,132,-823,133,-823,134,-823,130,-823,116,-823,115,-823,128,-823,129,-823,126,-823,6,-823,5,-823,120,-823,125,-823,123,-823,121,-823,124,-823,122,-823,137,-823,135,-823,16,-823,70,-823,100,-823,9,-823,13,-823,119,-823});
    states[710] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,711,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[711] = new State(-632);
    states[712] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,100,-634,9,-634},new int[]{-152,436,-156,49,-157,52});
    states[713] = new State(new int[]{53,687,9,714,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-370,440,-107,647,-152,1083,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-369,1104,-105,1105});
    states[714] = new State(new int[]{5,661,127,-1042},new int[]{-336,715});
    states[715] = new State(new int[]{127,716});
    states[716] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,717,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[717] = new State(-1021);
    states[718] = new State(-1058);
    states[719] = new State(-1044);
    states[720] = new State(-1045);
    states[721] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,722,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[722] = new State(new int[]{51,723});
    states[723] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524,9,-524,12,-524,100,-524,99,-524,32,-524,84,-524,83,-524,82,-524,81,-524},new int[]{-270,724,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[724] = new State(new int[]{32,725,92,-563,10,-563,98,-563,101,-563,33,-563,104,-563,2,-563,9,-563,12,-563,100,-563,99,-563,85,-563,84,-563,83,-563,82,-563,81,-563,86,-563});
    states[725] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524,9,-524,12,-524,100,-524,99,-524,32,-524,84,-524,83,-524,82,-524,81,-524},new int[]{-270,726,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[726] = new State(-564);
    states[727] = new State(new int[]{7,44,11,150,17,380});
    states[728] = new State(new int[]{7,453});
    states[729] = new State(-526);
    states[730] = new State(-527);
    states[731] = new State(new int[]{155,733,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-148,732,-152,734,-156,49,-157,52});
    states[732] = new State(-559);
    states[733] = new State(-130);
    states[734] = new State(-131);
    states[735] = new State(-528);
    states[736] = new State(-529);
    states[737] = new State(-530);
    states[738] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,739,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[739] = new State(new int[]{58,740});
    states[740] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,32,748,92,-583},new int[]{-40,741,-263,1080,-272,1082,-79,1073,-115,1079,-99,1078,-95,205,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[741] = new State(new int[]{10,744,32,748,92,-583},new int[]{-263,742});
    states[742] = new State(new int[]{92,743});
    states[743] = new State(-574);
    states[744] = new State(new int[]{32,748,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,92,-583},new int[]{-263,745,-272,747,-79,1073,-115,1079,-99,1078,-95,205,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[745] = new State(new int[]{92,746});
    states[746] = new State(-575);
    states[747] = new State(-578);
    states[748] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524,92,-524},new int[]{-262,749,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[749] = new State(new int[]{10,19,92,-584});
    states[750] = new State(-561);
    states[751] = new State(new int[]{11,-823,17,-823,8,-823,7,-823,142,-823,4,-823,15,-823,110,-823,111,-823,112,-823,113,-823,114,-823,92,-823,10,-823,98,-823,101,-823,33,-823,104,-823,2,-823,5,-131});
    states[752] = new State(new int[]{7,-222,11,-222,17,-222,5,-130});
    states[753] = new State(-531);
    states[754] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,98,-524,10,-524},new int[]{-262,755,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[755] = new State(new int[]{98,756,10,19});
    states[756] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,757,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[757] = new State(new int[]{70,27,92,-585,10,-585,98,-585,101,-585,33,-585,104,-585,2,-585,9,-585,12,-585,100,-585,99,-585,32,-585,85,-585,84,-585,83,-585,82,-585,81,-585,86,-585});
    states[758] = new State(-532);
    states[759] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,760,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[760] = new State(new int[]{99,1069,141,-588,143,-588,161,-588,85,-588,86,-588,80,-588,78,-588,160,-588,87,-588,45,-588,42,-588,8,-588,21,-588,22,-588,144,-588,147,-588,145,-588,146,-588,155,-588,158,-588,157,-588,156,-588,76,-588,11,-588,57,-588,91,-588,40,-588,25,-588,97,-588,54,-588,35,-588,55,-588,102,-588,47,-588,36,-588,53,-588,60,-588,74,-588,72,-588,38,-588,92,-588,10,-588,98,-588,101,-588,33,-588,104,-588,2,-588,9,-588,12,-588,100,-588,32,-588,84,-588,83,-588,82,-588,81,-588},new int[]{-301,761});
    states[761] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524,9,-524,12,-524,100,-524,99,-524,32,-524,84,-524,83,-524,82,-524,81,-524},new int[]{-270,762,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[762] = new State(-586);
    states[763] = new State(-533);
    states[764] = new State(new int[]{53,1072,143,-595,161,-595,85,-595,86,-595,80,-595,78,-595,160,-595,87,-595},new int[]{-23,765});
    states[765] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,766,-156,49,-157,52});
    states[766] = new State(new int[]{5,1022,110,-593},new int[]{-284,767});
    states[767] = new State(new int[]{110,768});
    states[768] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,769,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[769] = new State(new int[]{70,1070,71,1071},new int[]{-124,770});
    states[770] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,771,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[771] = new State(new int[]{160,1065,99,1069,141,-588,143,-588,161,-588,85,-588,86,-588,80,-588,78,-588,87,-588,45,-588,42,-588,8,-588,21,-588,22,-588,144,-588,147,-588,145,-588,146,-588,155,-588,158,-588,157,-588,156,-588,76,-588,11,-588,57,-588,91,-588,40,-588,25,-588,97,-588,54,-588,35,-588,55,-588,102,-588,47,-588,36,-588,53,-588,60,-588,74,-588,72,-588,38,-588,92,-588,10,-588,98,-588,101,-588,33,-588,104,-588,2,-588,9,-588,12,-588,100,-588,32,-588,84,-588,83,-588,82,-588,81,-588},new int[]{-301,772});
    states[772] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524,9,-524,12,-524,100,-524,99,-524,32,-524,84,-524,83,-524,82,-524,81,-524},new int[]{-270,773,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[773] = new State(-601);
    states[774] = new State(-534);
    states[775] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-76,776,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,834,-105,659,-334,677});
    states[776] = new State(new int[]{99,777,100,153});
    states[777] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524,9,-524,12,-524,100,-524,99,-524,32,-524,84,-524,83,-524,82,-524,81,-524},new int[]{-270,778,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[778] = new State(-603);
    states[779] = new State(-535);
    states[780] = new State(-536);
    states[781] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524,101,-524,33,-524},new int[]{-262,782,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[782] = new State(new int[]{10,19,101,784,33,1043},new int[]{-299,783});
    states[783] = new State(-605);
    states[784] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524},new int[]{-262,785,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[785] = new State(new int[]{92,786,10,19});
    states[786] = new State(-606);
    states[787] = new State(-537);
    states[788] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,92,-620,10,-620,98,-620,101,-620,33,-620,104,-620,2,-620,9,-620,12,-620,100,-620,99,-620,32,-620,84,-620,83,-620,82,-620,81,-620},new int[]{-92,789,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[789] = new State(new int[]{70,27,92,-621,10,-621,98,-621,101,-621,33,-621,104,-621,2,-621,9,-621,12,-621,100,-621,99,-621,32,-621,85,-621,84,-621,83,-621,82,-621,81,-621,86,-621});
    states[790] = new State(-538);
    states[791] = new State(new int[]{53,1024,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,792,-156,49,-157,52});
    states[792] = new State(new int[]{5,1022,137,-593},new int[]{-284,793});
    states[793] = new State(new int[]{137,794});
    states[794] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,795,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[795] = new State(new int[]{87,1020,99,-591},new int[]{-371,796});
    states[796] = new State(new int[]{99,797});
    states[797] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524,9,-524,12,-524,100,-524,99,-524,32,-524,84,-524,83,-524,82,-524,81,-524},new int[]{-270,798,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[798] = new State(-598);
    states[799] = new State(-539);
    states[800] = new State(new int[]{8,802,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-322,801,-163,810,-152,809,-156,49,-157,52});
    states[801] = new State(-549);
    states[802] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,803,-156,49,-157,52});
    states[803] = new State(new int[]{100,804});
    states[804] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,805,-152,809,-156,49,-157,52});
    states[805] = new State(new int[]{9,806,100,544});
    states[806] = new State(new int[]{110,807});
    states[807] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,808,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[808] = new State(new int[]{70,27,92,-551,10,-551,98,-551,101,-551,33,-551,104,-551,2,-551,9,-551,12,-551,100,-551,99,-551,32,-551,85,-551,84,-551,83,-551,82,-551,81,-551,86,-551});
    states[809] = new State(-375);
    states[810] = new State(new int[]{5,811,100,544,110,1018});
    states[811] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,812,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[812] = new State(new int[]{110,1016,120,1017,92,-439,10,-439,98,-439,101,-439,33,-439,104,-439,2,-439,9,-439,12,-439,100,-439,99,-439,32,-439,85,-439,84,-439,83,-439,82,-439,81,-439,86,-439},new int[]{-349,813});
    states[813] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,987,135,841,116,353,115,354,63,339,37,678,44,682,40,589},new int[]{-91,814,-90,815,-89,276,-95,277,-96,236,-86,816,-18,251,-15,261,-19,222,-152,856,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855,-100,1004,-253,1005,-334,1014,-252,1015});
    states[814] = new State(-441);
    states[815] = new State(-442);
    states[816] = new State(new int[]{6,817,116,247,115,248,128,249,129,250,120,-149,125,-149,123,-149,121,-149,124,-149,122,-149,137,-149,135,-149,13,-149,16,-149,92,-149,10,-149,98,-149,101,-149,33,-149,104,-149,2,-149,9,-149,12,-149,100,-149,99,-149,32,-149,85,-149,84,-149,83,-149,82,-149,81,-149,86,-149},new int[]{-202,214});
    states[817] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-18,818,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849});
    states[818] = new State(new int[]{136,252,138,253,118,254,117,255,131,256,132,257,133,258,134,259,130,260,92,-443,10,-443,98,-443,101,-443,33,-443,104,-443,2,-443,9,-443,12,-443,100,-443,99,-443,32,-443,85,-443,84,-443,83,-443,82,-443,81,-443,86,-443},new int[]{-210,216,-204,219});
    states[819] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-73,820,-82,371,-97,449,-92,374,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[820] = new State(new int[]{76,821});
    states[821] = new State(-198);
    states[822] = new State(-189);
    states[823] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,835,135,841,116,353,115,354,63,339},new int[]{-15,824,-19,825,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,843,-182,845,-61,846});
    states[824] = new State(-190);
    states[825] = new State(new int[]{4,224,11,226,7,826,142,828,8,829,136,-187,138,-187,118,-187,117,-187,131,-187,132,-187,133,-187,134,-187,130,-187,116,-187,115,-187,128,-187,129,-187,120,-187,125,-187,123,-187,121,-187,124,-187,122,-187,137,-187,135,-187,13,-187,16,-187,6,-187,100,-187,9,-187,12,-187,5,-187,92,-187,10,-187,98,-187,101,-187,33,-187,104,-187,2,-187,99,-187,32,-187,85,-187,84,-187,83,-187,82,-187,81,-187,86,-187},new int[]{-17,223});
    states[826] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-143,827,-152,46,-156,49,-157,52,-302,57,-155,58,-303,148});
    states[827] = new State(-210);
    states[828] = new State(-211);
    states[829] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,9,-215},new int[]{-81,830,-76,832,-93,396,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,834,-105,659,-334,677});
    states[830] = new State(new int[]{9,831});
    states[831] = new State(-212);
    states[832] = new State(new int[]{100,153,9,-214});
    states[833] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,92,-630,10,-630,98,-630,101,-630,33,-630,104,-630,2,-630,9,-630,12,-630,100,-630,99,-630,32,-630,84,-630,83,-630,82,-630,81,-630},new int[]{-152,436,-156,49,-157,52});
    states[834] = new State(-629);
    states[835] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,836,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[836] = new State(new int[]{9,837,13,206,16,210});
    states[837] = new State(-191);
    states[838] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,839,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[839] = new State(new int[]{9,840,13,206,16,210});
    states[840] = new State(new int[]{136,-191,138,-191,118,-191,117,-191,131,-191,132,-191,133,-191,134,-191,130,-191,116,-191,115,-191,128,-191,129,-191,120,-191,125,-191,123,-191,121,-191,124,-191,122,-191,137,-191,135,-191,13,-191,16,-191,6,-191,100,-191,9,-191,12,-191,5,-191,92,-191,10,-191,98,-191,101,-191,33,-191,104,-191,2,-191,99,-191,32,-191,85,-191,84,-191,83,-191,82,-191,81,-191,86,-191,119,-186});
    states[841] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,835,135,841,116,353,115,354,63,339},new int[]{-15,842,-19,825,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,843,-182,845,-61,846});
    states[842] = new State(-192);
    states[843] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,835,135,841,116,353,115,354,63,339},new int[]{-15,844,-19,825,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,843,-182,845,-61,846});
    states[844] = new State(-193);
    states[845] = new State(-194);
    states[846] = new State(-195);
    states[847] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-15,844,-279,848,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-16,849});
    states[848] = new State(-172);
    states[849] = new State(new int[]{119,850});
    states[850] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-15,851,-279,852,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-16,849});
    states[851] = new State(-170);
    states[852] = new State(-171);
    states[853] = new State(-174);
    states[854] = new State(-175);
    states[855] = new State(-152);
    states[856] = new State(new int[]{127,857,4,-201,11,-201,7,-201,142,-201,8,-201,136,-201,138,-201,118,-201,117,-201,131,-201,132,-201,133,-201,134,-201,130,-201,6,-201,116,-201,115,-201,128,-201,129,-201,120,-201,125,-201,123,-201,121,-201,124,-201,122,-201,137,-201,135,-201,13,-201,16,-201,92,-201,10,-201,98,-201,101,-201,33,-201,104,-201,2,-201,9,-201,12,-201,100,-201,99,-201,32,-201,85,-201,84,-201,83,-201,82,-201,81,-201,86,-201,119,-201});
    states[857] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,858,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[858] = new State(-445);
    states[859] = new State(-1046);
    states[860] = new State(-1047);
    states[861] = new State(-1048);
    states[862] = new State(-1049);
    states[863] = new State(-1050);
    states[864] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,865,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[865] = new State(new int[]{99,866});
    states[866] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524,9,-524,12,-524,100,-524,99,-524,32,-524,84,-524,83,-524,82,-524,81,-524},new int[]{-270,867,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[867] = new State(-546);
    states[868] = new State(-540);
    states[869] = new State(-626);
    states[870] = new State(-627);
    states[871] = new State(-541);
    states[872] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,873,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[873] = new State(new int[]{99,874});
    states[874] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524,9,-524,12,-524,100,-524,99,-524,32,-524,84,-524,83,-524,82,-524,81,-524},new int[]{-270,875,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[875] = new State(-589);
    states[876] = new State(-542);
    states[877] = new State(new int[]{73,879,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,878,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-333,882,-105,659,-334,677});
    states[878] = new State(-547);
    states[879] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,880,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-333,882,-105,659,-334,677});
    states[880] = new State(-548);
    states[881] = new State(-645);
    states[882] = new State(-646);
    states[883] = new State(-543);
    states[884] = new State(-544);
    states[885] = new State(-545);
    states[886] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,887,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[887] = new State(new int[]{55,888});
    states[888] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,56,966,21,294,22,299,11,926,8,939},new int[]{-361,889,-360,980,-353,896,-293,901,-189,181,-152,218,-156,49,-157,52,-352,958,-368,961,-350,969,-20,964,-174,336,-176,279,-175,283,-21,337,-267,967,-304,968,-354,970,-355,973});
    states[889] = new State(new int[]{10,892,32,748,92,-583},new int[]{-263,890});
    states[890] = new State(new int[]{92,891});
    states[891] = new State(-565);
    states[892] = new State(new int[]{32,748,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,56,966,21,294,22,299,11,926,8,939,92,-583},new int[]{-263,893,-360,895,-353,896,-293,901,-189,181,-152,218,-156,49,-157,52,-352,958,-368,961,-350,969,-20,964,-174,336,-176,279,-175,283,-21,337,-267,967,-304,968,-354,970,-355,973});
    states[893] = new State(new int[]{92,894});
    states[894] = new State(-566);
    states[895] = new State(-568);
    states[896] = new State(new int[]{39,897});
    states[897] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,898,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[898] = new State(new int[]{5,899});
    states[899] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524,32,-524,92,-524},new int[]{-270,900,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[900] = new State(-569);
    states[901] = new State(new int[]{8,902,100,-688,5,-688});
    states[902] = new State(new int[]{14,907,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,53,914,11,926,8,939},new int[]{-365,903,-363,957,-20,908,-174,336,-176,279,-175,283,-21,337,-208,909,-152,911,-156,49,-157,52,-353,918,-293,919,-189,181,-354,925,-355,956});
    states[903] = new State(new int[]{9,904,10,905,100,923});
    states[904] = new State(new int[]{39,-682,5,-683});
    states[905] = new State(new int[]{14,907,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,53,914,11,926,8,939},new int[]{-363,906,-20,908,-174,336,-176,279,-175,283,-21,337,-208,909,-152,911,-156,49,-157,52,-353,918,-293,919,-189,181,-354,925,-355,956});
    states[906] = new State(-714);
    states[907] = new State(-726);
    states[908] = new State(-727);
    states[909] = new State(new int[]{144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290},new int[]{-20,910,-174,336,-176,279,-175,283,-21,337});
    states[910] = new State(-728);
    states[911] = new State(new int[]{5,912,9,-730,10,-730,100,-730,7,-292,4,-292,123,-292,8,-292});
    states[912] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,913,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[913] = new State(-729);
    states[914] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,915,-156,49,-157,52});
    states[915] = new State(new int[]{5,916,9,-732,10,-732,100,-732});
    states[916] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,917,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[917] = new State(-731);
    states[918] = new State(-733);
    states[919] = new State(new int[]{8,920});
    states[920] = new State(new int[]{14,907,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,53,914,11,926,8,939},new int[]{-365,921,-363,957,-20,908,-174,336,-176,279,-175,283,-21,337,-208,909,-152,911,-156,49,-157,52,-353,918,-293,919,-189,181,-354,925,-355,956});
    states[921] = new State(new int[]{9,922,10,905,100,923});
    states[922] = new State(-682);
    states[923] = new State(new int[]{14,907,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,53,914,11,926,8,939},new int[]{-363,924,-20,908,-174,336,-176,279,-175,283,-21,337,-208,909,-152,911,-156,49,-157,52,-353,918,-293,919,-189,181,-354,925,-355,956});
    states[924] = new State(-715);
    states[925] = new State(-734);
    states[926] = new State(new int[]{144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,933,14,935,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939,6,954},new int[]{-366,927,-356,955,-20,931,-174,336,-176,279,-175,283,-21,337,-358,932,-353,936,-293,919,-189,181,-152,218,-156,49,-157,52,-354,937,-355,938});
    states[927] = new State(new int[]{12,928,100,929});
    states[928] = new State(-692);
    states[929] = new State(new int[]{144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,933,14,935,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939,6,954},new int[]{-356,930,-20,931,-174,336,-176,279,-175,283,-21,337,-358,932,-353,936,-293,919,-189,181,-152,218,-156,49,-157,52,-354,937,-355,938});
    states[930] = new State(-694);
    states[931] = new State(-695);
    states[932] = new State(-696);
    states[933] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,934,-156,49,-157,52});
    states[934] = new State(-702);
    states[935] = new State(-697);
    states[936] = new State(-698);
    states[937] = new State(-699);
    states[938] = new State(-700);
    states[939] = new State(new int[]{14,944,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,53,948,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-367,940,-357,953,-20,945,-174,336,-176,279,-175,283,-21,337,-208,946,-353,950,-293,919,-189,181,-152,218,-156,49,-157,52,-354,951,-355,952});
    states[940] = new State(new int[]{9,941,100,942});
    states[941] = new State(-703);
    states[942] = new State(new int[]{14,944,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,116,353,115,354,53,948,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-357,943,-20,945,-174,336,-176,279,-175,283,-21,337,-208,946,-353,950,-293,919,-189,181,-152,218,-156,49,-157,52,-354,951,-355,952});
    states[943] = new State(-712);
    states[944] = new State(-704);
    states[945] = new State(-705);
    states[946] = new State(new int[]{144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290},new int[]{-20,947,-174,336,-176,279,-175,283,-21,337});
    states[947] = new State(-706);
    states[948] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,949,-156,49,-157,52});
    states[949] = new State(-707);
    states[950] = new State(-708);
    states[951] = new State(-709);
    states[952] = new State(-710);
    states[953] = new State(-711);
    states[954] = new State(-701);
    states[955] = new State(-693);
    states[956] = new State(-735);
    states[957] = new State(-713);
    states[958] = new State(new int[]{5,959});
    states[959] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524,32,-524,92,-524},new int[]{-270,960,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[960] = new State(-570);
    states[961] = new State(new int[]{100,962,5,-684});
    states[962] = new State(new int[]{144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,56,966,21,294,22,299},new int[]{-350,963,-20,964,-174,336,-176,279,-175,283,-21,337,-293,965,-189,181,-152,218,-156,49,-157,52,-267,967,-304,968});
    states[963] = new State(-686);
    states[964] = new State(-687);
    states[965] = new State(-688);
    states[966] = new State(-689);
    states[967] = new State(-690);
    states[968] = new State(-691);
    states[969] = new State(-685);
    states[970] = new State(new int[]{5,971});
    states[971] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524,32,-524,92,-524},new int[]{-270,972,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[972] = new State(-571);
    states[973] = new State(new int[]{39,974,5,978});
    states[974] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,975,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[975] = new State(new int[]{5,976});
    states[976] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524,32,-524,92,-524},new int[]{-270,977,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[977] = new State(-572);
    states[978] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524,32,-524,92,-524},new int[]{-270,979,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[979] = new State(-573);
    states[980] = new State(-567);
    states[981] = new State(-1051);
    states[982] = new State(-1052);
    states[983] = new State(-1053);
    states[984] = new State(-1054);
    states[985] = new State(-1055);
    states[986] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,878,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-333,882,-105,659,-334,677});
    states[987] = new State(new int[]{9,995,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339},new int[]{-95,988,-70,989,-255,993,-96,236,-86,246,-18,251,-15,261,-19,222,-152,999,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855,-69,273,-90,1003,-89,276,-100,1004,-253,1005,-254,1006,-256,1013,-141,1009});
    states[988] = new State(new int[]{9,840,13,206,16,210,100,-226});
    states[989] = new State(new int[]{9,990});
    states[990] = new State(new int[]{127,991,92,-229,10,-229,98,-229,101,-229,33,-229,104,-229,2,-229,9,-229,12,-229,100,-229,99,-229,32,-229,85,-229,84,-229,83,-229,82,-229,81,-229,86,-229});
    states[991] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,992,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[992] = new State(-447);
    states[993] = new State(new int[]{9,994});
    states[994] = new State(-234);
    states[995] = new State(new int[]{5,546,127,-1040},new int[]{-335,996});
    states[996] = new State(new int[]{127,997});
    states[997] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,998,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[998] = new State(-446);
    states[999] = new State(new int[]{4,-201,11,-201,7,-201,142,-201,8,-201,136,-201,138,-201,118,-201,117,-201,131,-201,132,-201,133,-201,134,-201,130,-201,116,-201,115,-201,128,-201,129,-201,120,-201,125,-201,123,-201,121,-201,124,-201,122,-201,137,-201,135,-201,9,-201,13,-201,16,-201,100,-201,119,-201,5,-240});
    states[1000] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339,9,-230},new int[]{-95,988,-70,1001,-255,993,-96,236,-86,246,-18,251,-15,261,-19,222,-152,999,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855,-69,273,-90,1003,-89,276,-100,1004,-253,1005,-254,1006,-256,1013,-141,1009});
    states[1001] = new State(new int[]{9,1002});
    states[1002] = new State(-229);
    states[1003] = new State(-232);
    states[1004] = new State(-227);
    states[1005] = new State(-228);
    states[1006] = new State(new int[]{10,1007,9,-235});
    states[1007] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,9,-236},new int[]{-256,1008,-141,1009,-152,1012,-156,49,-157,52});
    states[1008] = new State(-238);
    states[1009] = new State(new int[]{5,1010});
    states[1010] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339},new int[]{-89,1011,-95,277,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855,-100,1004,-253,1005});
    states[1011] = new State(-239);
    states[1012] = new State(-240);
    states[1013] = new State(-237);
    states[1014] = new State(-444);
    states[1015] = new State(-448);
    states[1016] = new State(-437);
    states[1017] = new State(-438);
    states[1018] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682},new int[]{-93,1019,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,834,-105,659,-334,677});
    states[1019] = new State(-440);
    states[1020] = new State(new int[]{143,1021});
    states[1021] = new State(-590);
    states[1022] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,1023,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1023] = new State(-592);
    states[1024] = new State(new int[]{8,1032,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1025,-156,49,-157,52});
    states[1025] = new State(new int[]{5,1022,137,-593},new int[]{-284,1026});
    states[1026] = new State(new int[]{137,1027});
    states[1027] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,1028,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[1028] = new State(new int[]{87,1020,99,-591},new int[]{-371,1029});
    states[1029] = new State(new int[]{99,1030});
    states[1030] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524,9,-524,12,-524,100,-524,99,-524,32,-524,84,-524,83,-524,82,-524,81,-524},new int[]{-270,1031,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1031] = new State(-599);
    states[1032] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1033,-152,809,-156,49,-157,52});
    states[1033] = new State(new int[]{9,1034,100,544});
    states[1034] = new State(new int[]{137,1035});
    states[1035] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,1036,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[1036] = new State(new int[]{87,1020,99,-591},new int[]{-371,1037});
    states[1037] = new State(new int[]{99,1038});
    states[1038] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524,9,-524,12,-524,100,-524,99,-524,32,-524,84,-524,83,-524,82,-524,81,-524},new int[]{-270,1039,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1039] = new State(-600);
    states[1040] = new State(new int[]{5,1041});
    states[1041] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524},new int[]{-271,1042,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[1042] = new State(-523);
    states[1043] = new State(new int[]{79,1051,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524,92,-524},new int[]{-64,1044,-67,1046,-66,1063,-262,1064,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[1044] = new State(new int[]{92,1045});
    states[1045] = new State(-607);
    states[1046] = new State(new int[]{10,1048,32,1061,92,-613},new int[]{-264,1047});
    states[1047] = new State(-608);
    states[1048] = new State(new int[]{79,1051,32,1061,92,-613},new int[]{-66,1049,-264,1050});
    states[1049] = new State(-612);
    states[1050] = new State(-609);
    states[1051] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-68,1052,-188,1055,-189,1056,-152,1057,-156,49,-157,52,-145,1058});
    states[1052] = new State(new int[]{99,1053});
    states[1053] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524,32,-524,92,-524},new int[]{-270,1054,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1054] = new State(-615);
    states[1055] = new State(-616);
    states[1056] = new State(new int[]{7,182,99,-618});
    states[1057] = new State(new int[]{7,-292,99,-292,5,-619});
    states[1058] = new State(new int[]{5,1059});
    states[1059] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-188,1060,-189,1056,-152,218,-156,49,-157,52});
    states[1060] = new State(-617);
    states[1061] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524,92,-524},new int[]{-262,1062,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[1062] = new State(new int[]{10,19,92,-614});
    states[1063] = new State(-611);
    states[1064] = new State(new int[]{10,19,92,-610});
    states[1065] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,1066,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[1066] = new State(new int[]{99,1067});
    states[1067] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524,98,-524,101,-524,33,-524,104,-524,2,-524,9,-524,12,-524,100,-524,99,-524,32,-524,84,-524,83,-524,82,-524,81,-524},new int[]{-270,1068,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1068] = new State(-602);
    states[1069] = new State(-587);
    states[1070] = new State(-596);
    states[1071] = new State(-597);
    states[1072] = new State(-594);
    states[1073] = new State(new int[]{5,1074,100,1076});
    states[1074] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524,32,-524,92,-524},new int[]{-270,1075,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1075] = new State(-579);
    states[1076] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-115,1077,-99,1078,-95,205,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[1077] = new State(-581);
    states[1078] = new State(-582);
    states[1079] = new State(-580);
    states[1080] = new State(new int[]{92,1081});
    states[1081] = new State(-576);
    states[1082] = new State(-577);
    states[1083] = new State(new int[]{5,1084,10,1097,11,-823,17,-823,8,-823,7,-823,142,-823,4,-823,15,-823,110,-823,111,-823,112,-823,113,-823,114,-823,138,-823,136,-823,118,-823,117,-823,131,-823,132,-823,133,-823,134,-823,130,-823,116,-823,115,-823,128,-823,129,-823,126,-823,6,-823,120,-823,125,-823,123,-823,121,-823,124,-823,122,-823,137,-823,135,-823,16,-823,9,-823,70,-823,100,-823,13,-823,119,-823});
    states[1084] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,1085,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1085] = new State(new int[]{9,1086,10,1090});
    states[1086] = new State(new int[]{5,661,127,-1042},new int[]{-336,1087});
    states[1087] = new State(new int[]{127,1088});
    states[1088] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,1089,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[1089] = new State(-1022);
    states[1090] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-337,1091,-338,1096,-163,542,-152,809,-156,49,-157,52});
    states[1091] = new State(new int[]{9,1092,10,540});
    states[1092] = new State(new int[]{5,661,127,-1042},new int[]{-336,1093});
    states[1093] = new State(new int[]{127,1094});
    states[1094] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,1095,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[1095] = new State(-1024);
    states[1096] = new State(-1037);
    states[1097] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-337,1098,-338,1096,-163,542,-152,809,-156,49,-157,52});
    states[1098] = new State(new int[]{9,1099,10,540});
    states[1099] = new State(new int[]{5,661,127,-1042},new int[]{-336,1100});
    states[1100] = new State(new int[]{127,1101});
    states[1101] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,1102,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[1102] = new State(-1023);
    states[1103] = new State(new int[]{9,-635,70,-635,100,-1016});
    states[1104] = new State(-820);
    states[1105] = new State(-1017);
    states[1106] = new State(-633);
    states[1107] = new State(-624);
    states[1108] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,405},new int[]{-153,1109,-152,455,-156,49,-157,52,-302,456,-155,58,-200,457});
    states[1109] = new State(-838);
    states[1110] = new State(-840);
    states[1111] = new State(new int[]{123,187},new int[]{-308,1112});
    states[1112] = new State(-841);
    states[1113] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,53,1114,18,669},new int[]{-92,438,-370,440,-107,532,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-369,1104,-105,1105});
    states[1114] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1115,-156,49,-157,52});
    states[1115] = new State(new int[]{110,1116});
    states[1116] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,692,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[1117] = new State(new int[]{11,393,17,400,8,702,7,1108,142,1110,4,1111,9,-554,100,-554});
    states[1118] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,1113,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368},new int[]{-116,1119,-121,1120,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728});
    states[1119] = new State(new int[]{11,393,17,400,8,702,7,1108,142,1110,4,1111,15,1118,110,-805,111,-805,112,-805,113,-805,114,-805,92,-805,10,-805,98,-805,101,-805,33,-805,104,-805,2,-805,138,-805,136,-805,118,-805,117,-805,131,-805,132,-805,133,-805,134,-805,130,-805,116,-805,115,-805,128,-805,129,-805,126,-805,6,-805,5,-805,120,-805,125,-805,123,-805,121,-805,124,-805,122,-805,137,-805,135,-805,16,-805,70,-805,9,-805,12,-805,100,-805,99,-805,32,-805,85,-805,84,-805,83,-805,82,-805,81,-805,86,-805,13,-805,119,-805,76,-805,51,-805,58,-805,141,-805,143,-805,161,-805,80,-805,78,-805,160,-805,87,-805,45,-805,42,-805,21,-805,22,-805,144,-805,147,-805,145,-805,146,-805,155,-805,158,-805,157,-805,156,-805,57,-805,91,-805,40,-805,25,-805,97,-805,54,-805,35,-805,55,-805,102,-805,47,-805,36,-805,53,-805,60,-805,74,-805,72,-805,38,-805,71,-805});
    states[1120] = new State(-806);
    states[1121] = new State(new int[]{9,1122});
    states[1122] = new State(-1056);
    states[1123] = new State(new int[]{110,429,111,430,112,431,113,432,114,433,138,-801,136,-801,118,-801,117,-801,131,-801,132,-801,133,-801,134,-801,130,-801,116,-801,115,-801,128,-801,129,-801,126,-801,6,-801,5,-801,120,-801,125,-801,123,-801,121,-801,124,-801,122,-801,137,-801,135,-801,16,-801,9,-801,70,-801,100,-801,13,-801,2,-801,119,-794},new int[]{-203,24});
    states[1124] = new State(new int[]{53,687,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598,18,669},new int[]{-92,438,-370,440,-107,532,-116,694,-106,1103,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-369,1104,-105,1105});
    states[1125] = new State(-1060);
    states[1126] = new State(-1061);
    states[1127] = new State(new int[]{9,1128,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-337,1131,-338,1096,-163,542,-152,809,-156,49,-157,52});
    states[1128] = new State(new int[]{127,1129});
    states[1129] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,686,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-340,1130,-221,685,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-9,1125,-341,1126,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[1130] = new State(-1035);
    states[1131] = new State(new int[]{9,1132,10,540});
    states[1132] = new State(new int[]{127,1133});
    states[1133] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,42,435,8,686,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-340,1134,-221,685,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-9,1125,-341,1126,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[1134] = new State(-1036);
    states[1135] = new State(new int[]{9,1136,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-337,1140,-338,1096,-163,542,-152,809,-156,49,-157,52});
    states[1136] = new State(new int[]{5,661,127,-1042},new int[]{-336,1137});
    states[1137] = new State(new int[]{127,1138});
    states[1138] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,1139,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[1139] = new State(-1032);
    states[1140] = new State(new int[]{9,1141,10,540});
    states[1141] = new State(new int[]{5,661,127,-1042},new int[]{-336,1142});
    states[1142] = new State(new int[]{127,1143});
    states[1143] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,646,21,294,22,299,76,446,11,368,18,669,37,678,44,682,91,16,40,721,54,759,97,754,35,764,36,791,72,864,25,738,102,781,60,872,47,788,74,986},new int[]{-339,1144,-109,644,-104,645,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,657,-122,580,-333,658,-105,659,-334,677,-341,718,-265,719,-158,720,-329,859,-257,860,-129,861,-128,862,-130,863,-39,981,-311,982,-178,983,-258,984,-131,985});
    states[1144] = new State(-1033);
    states[1145] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,18,669},new int[]{-107,1146,-106,1147,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-105,1105});
    states[1146] = new State(-1019);
    states[1147] = new State(-1016);
    states[1148] = new State(-1018);
    states[1149] = new State(new int[]{5,1150,7,-292,8,-292,123,-292,12,-292,100,-292});
    states[1150] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-14,1151,-189,634,-152,218,-156,49,-157,52,-310,1152});
    states[1151] = new State(-249);
    states[1152] = new State(new int[]{8,637,12,-673,100,-673},new int[]{-74,1153});
    states[1153] = new State(-814);
    states[1154] = new State(-246);
    states[1155] = new State(-242);
    states[1156] = new State(-503);
    states[1157] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1158,-156,49,-157,52});
    states[1158] = new State(new int[]{110,689,100,1159});
    states[1159] = new State(new int[]{53,1167},new int[]{-348,1160});
    states[1160] = new State(new int[]{9,1161,100,1164});
    states[1161] = new State(new int[]{110,1162});
    states[1162] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,1163,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[1163] = new State(new int[]{70,27,92,-550,10,-550,98,-550,101,-550,33,-550,104,-550,2,-550,9,-550,12,-550,100,-550,99,-550,32,-550,85,-550,84,-550,83,-550,82,-550,81,-550,86,-550});
    states[1164] = new State(new int[]{53,1165});
    states[1165] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1166,-156,49,-157,52});
    states[1166] = new State(-557);
    states[1167] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1168,-156,49,-157,52});
    states[1168] = new State(-556);
    states[1169] = new State(new int[]{148,1173,150,1174,151,1175,152,1176,154,1177,153,1178,107,-860,91,-860,59,-860,29,-860,66,-860,50,-860,53,-860,62,-860,11,-860,28,-860,26,-860,44,-860,37,-860,19,-860,30,-860,31,-860,46,-860,27,-860,92,-860,84,-860,83,-860,82,-860,81,-860,23,-860,149,-860,161,-860,41,-860},new int[]{-215,1170,-218,1179});
    states[1170] = new State(new int[]{10,1171});
    states[1171] = new State(new int[]{148,1173,150,1174,151,1175,152,1176,154,1177,153,1178,107,-861,91,-861,59,-861,29,-861,66,-861,50,-861,53,-861,62,-861,11,-861,28,-861,26,-861,44,-861,37,-861,19,-861,30,-861,31,-861,46,-861,27,-861,92,-861,84,-861,83,-861,82,-861,81,-861,23,-861,149,-861,161,-861,41,-861},new int[]{-218,1172});
    states[1172] = new State(-865);
    states[1173] = new State(-878);
    states[1174] = new State(-879);
    states[1175] = new State(-880);
    states[1176] = new State(-881);
    states[1177] = new State(-882);
    states[1178] = new State(-883);
    states[1179] = new State(-864);
    states[1180] = new State(-408);
    states[1181] = new State(-477);
    states[1182] = new State(-478);
    states[1183] = new State(new int[]{8,-483,110,-483,10,-483,11,-483,5,-483,7,-480});
    states[1184] = new State(new int[]{123,1186,8,-486,110,-486,10,-486,7,-486,11,-486,5,-486},new int[]{-160,1185});
    states[1185] = new State(-487);
    states[1186] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1187,-152,809,-156,49,-157,52});
    states[1187] = new State(new int[]{121,1188,100,544});
    states[1188] = new State(-353);
    states[1189] = new State(-488);
    states[1190] = new State(new int[]{123,1186,8,-484,110,-484,10,-484,11,-484,5,-484},new int[]{-160,1191});
    states[1191] = new State(-485);
    states[1192] = new State(new int[]{7,1193});
    states[1193] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-147,1194,-154,1195,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190});
    states[1194] = new State(-479);
    states[1195] = new State(-482);
    states[1196] = new State(-481);
    states[1197] = new State(-463);
    states[1198] = new State(new int[]{44,1328,37,1363},new int[]{-225,1199,-237,1200,-234,1327,-238,1362});
    states[1199] = new State(-465);
    states[1200] = new State(new int[]{107,1318,59,-95,29,-95,66,-95,50,-95,53,-95,62,-95,91,-95},new int[]{-185,1201,-47,1202,-43,1205,-65,1317});
    states[1201] = new State(-471);
    states[1202] = new State(new int[]{91,16},new int[]{-265,1203});
    states[1203] = new State(new int[]{10,1204});
    states[1204] = new State(-498);
    states[1205] = new State(new int[]{59,1208,29,1229,66,1233,50,1445,53,1460,62,1462,91,-94},new int[]{-50,1206,-177,1207,-33,1214,-56,1231,-298,1235,-320,1447});
    states[1206] = new State(-96);
    states[1207] = new State(-118);
    states[1208] = new State(new int[]{155,733,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-161,1209,-148,1213,-152,734,-156,49,-157,52});
    states[1209] = new State(new int[]{10,1210,100,1211});
    states[1210] = new State(-127);
    states[1211] = new State(new int[]{155,733,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-148,1212,-152,734,-156,49,-157,52});
    states[1212] = new State(-129);
    states[1213] = new State(-128);
    states[1214] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-119,29,-119,66,-119,50,-119,53,-119,62,-119,91,-119},new int[]{-31,1215,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1215] = new State(-133);
    states[1216] = new State(new int[]{10,1217});
    states[1217] = new State(-143);
    states[1218] = new State(new int[]{120,1219,5,1224});
    states[1219] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1222,135,841,116,353,115,354,63,339},new int[]{-114,1220,-95,1221,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855,-100,1223});
    states[1220] = new State(-144);
    states[1221] = new State(new int[]{13,206,16,210,10,-146,92,-146,84,-146,83,-146,82,-146,81,-146});
    states[1222] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339,9,-230},new int[]{-95,988,-70,1001,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855,-69,273,-90,1003,-89,276,-100,1004,-253,1005});
    states[1223] = new State(-147);
    states[1224] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,1225,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1225] = new State(new int[]{120,1226});
    states[1226] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,1000,135,841,116,353,115,354,63,339},new int[]{-89,1227,-95,277,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855,-100,1004,-253,1005});
    states[1227] = new State(-145);
    states[1228] = new State(-148);
    states[1229] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-31,1230,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1230] = new State(-132);
    states[1231] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-120,29,-120,66,-120,50,-120,53,-120,62,-120,91,-120},new int[]{-31,1232,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1232] = new State(-135);
    states[1233] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-31,1234,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1234] = new State(-134);
    states[1235] = new State(new int[]{11,628,59,-121,29,-121,66,-121,50,-121,53,-121,62,-121,91,-121,143,-244,161,-244,85,-244,86,-244,80,-244,78,-244,160,-244,87,-244},new int[]{-53,1236,-11,1237,-260,1155});
    states[1236] = new State(-137);
    states[1237] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,628},new int[]{-54,1238,-260,518,-149,1239,-152,1437,-156,49,-157,52,-150,1442});
    states[1238] = new State(-241);
    states[1239] = new State(new int[]{120,1240});
    states[1240] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604,68,1431,69,1432,148,1433,27,1434,28,1435,26,-335,43,-335,64,-335},new int[]{-296,1241,-286,1243,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562,-34,1244,-25,1245,-26,1429,-24,1436});
    states[1241] = new State(new int[]{10,1242});
    states[1242] = new State(-250);
    states[1243] = new State(-255);
    states[1244] = new State(-256);
    states[1245] = new State(new int[]{26,1423,43,1424,64,1425},new int[]{-300,1246});
    states[1246] = new State(new int[]{8,1344,23,-347,11,-347,92,-347,84,-347,83,-347,82,-347,81,-347,29,-347,143,-347,161,-347,85,-347,86,-347,80,-347,78,-347,160,-347,87,-347,62,-347,28,-347,26,-347,44,-347,37,-347,19,-347,30,-347,31,-347,46,-347,27,-347,10,-347},new int[]{-192,1247});
    states[1247] = new State(new int[]{23,1335,11,-354,92,-354,84,-354,83,-354,82,-354,81,-354,29,-354,143,-354,161,-354,85,-354,86,-354,80,-354,78,-354,160,-354,87,-354,62,-354,28,-354,26,-354,44,-354,37,-354,19,-354,30,-354,31,-354,46,-354,27,-354,10,-354},new int[]{-328,1248,-327,1333,-326,1355});
    states[1248] = new State(new int[]{11,628,10,-345,92,-371,84,-371,83,-371,82,-371,81,-371,29,-244,143,-244,161,-244,85,-244,86,-244,80,-244,78,-244,160,-244,87,-244,62,-244,28,-244,26,-244,44,-244,37,-244,19,-244,30,-244,31,-244,46,-244,27,-244},new int[]{-30,1249,-29,1250,-36,1256,-38,509,-49,1257,-11,1258,-260,1155,-37,1420,-58,1422,-57,515,-59,1421});
    states[1249] = new State(-328);
    states[1250] = new State(new int[]{92,1251,84,1252,83,1253,82,1254,81,1255},new int[]{-12,507});
    states[1251] = new State(-346);
    states[1252] = new State(-367);
    states[1253] = new State(-368);
    states[1254] = new State(-369);
    states[1255] = new State(-370);
    states[1256] = new State(-365);
    states[1257] = new State(-379);
    states[1258] = new State(new int[]{29,1260,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,62,1264,28,1380,26,1381,11,628,44,1328,37,1363,19,1383,30,1391,31,1398,46,1405,27,1414},new int[]{-55,1259,-260,518,-231,517,-228,519,-268,520,-323,1262,-322,1263,-163,810,-152,809,-156,49,-157,52,-8,1268,-239,1382,-237,1316,-234,1327,-238,1362,-236,1389,-224,1402,-225,1403,-227,1404});
    states[1259] = new State(-381);
    states[1260] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-32,1261,-146,1218,-152,1228,-156,49,-157,52});
    states[1261] = new State(-386);
    states[1262] = new State(-387);
    states[1263] = new State(-391);
    states[1264] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1265,-152,809,-156,49,-157,52});
    states[1265] = new State(new int[]{5,1266,100,544});
    states[1266] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,1267,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1267] = new State(-392);
    states[1268] = new State(new int[]{30,523,19,1198,46,1270,27,1308,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,62,1264,44,1328,37,1363},new int[]{-323,1269,-239,522,-225,1197,-322,1263,-163,810,-152,809,-156,49,-157,52,-237,1316,-234,1327,-238,1362});
    states[1269] = new State(-388);
    states[1270] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-179,1271,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1271] = new State(new int[]{11,1299,5,-422},new int[]{-242,1272,-247,1296});
    states[1272] = new State(new int[]{85,1285,86,1291,10,-429},new int[]{-211,1273});
    states[1273] = new State(new int[]{10,1274});
    states[1274] = new State(new int[]{63,1279,153,1281,152,1282,148,1283,151,1284,11,-419,28,-419,26,-419,44,-419,37,-419,19,-419,30,-419,31,-419,46,-419,27,-419,92,-419,84,-419,83,-419,82,-419,81,-419},new int[]{-214,1275,-219,1276});
    states[1275] = new State(-413);
    states[1276] = new State(new int[]{10,1277});
    states[1277] = new State(new int[]{63,1279,11,-419,28,-419,26,-419,44,-419,37,-419,19,-419,30,-419,31,-419,46,-419,27,-419,92,-419,84,-419,83,-419,82,-419,81,-419},new int[]{-214,1278});
    states[1278] = new State(-414);
    states[1279] = new State(new int[]{10,1280});
    states[1280] = new State(-420);
    states[1281] = new State(-884);
    states[1282] = new State(-885);
    states[1283] = new State(-886);
    states[1284] = new State(-887);
    states[1285] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,10,-428},new int[]{-119,1286,-93,1290,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,834,-105,659,-334,677});
    states[1286] = new State(new int[]{86,1288,10,-432},new int[]{-212,1287});
    states[1287] = new State(-430);
    states[1288] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524},new int[]{-270,1289,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1289] = new State(-433);
    states[1290] = new State(-427);
    states[1291] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524},new int[]{-270,1292,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1292] = new State(new int[]{85,1294,10,-434},new int[]{-213,1293});
    states[1293] = new State(-431);
    states[1294] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,833,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,10,-428},new int[]{-119,1295,-93,1290,-92,26,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,834,-105,659,-334,677});
    states[1295] = new State(-435);
    states[1296] = new State(new int[]{5,1297});
    states[1297] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,1298,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1298] = new State(-421);
    states[1299] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-246,1300,-245,1307,-163,1304,-152,809,-156,49,-157,52});
    states[1300] = new State(new int[]{12,1301,10,1302});
    states[1301] = new State(-423);
    states[1302] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-245,1303,-163,1304,-152,809,-156,49,-157,52});
    states[1303] = new State(-425);
    states[1304] = new State(new int[]{5,1305,100,544});
    states[1305] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,1306,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1306] = new State(-426);
    states[1307] = new State(-424);
    states[1308] = new State(new int[]{46,1309});
    states[1309] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-179,1310,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1310] = new State(new int[]{11,1299,5,-422},new int[]{-242,1311,-247,1296});
    states[1311] = new State(new int[]{110,1314,10,-418},new int[]{-220,1312});
    states[1312] = new State(new int[]{10,1313});
    states[1313] = new State(-416);
    states[1314] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,1315,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[1315] = new State(new int[]{70,27,10,-417});
    states[1316] = new State(new int[]{107,1318,11,-402,28,-402,26,-402,44,-402,37,-402,19,-402,30,-402,31,-402,46,-402,27,-402,92,-402,84,-402,83,-402,82,-402,81,-402,59,-95,29,-95,66,-95,50,-95,53,-95,62,-95,91,-95},new int[]{-185,1201,-47,1202,-43,1205,-65,1317});
    states[1317] = new State(-499);
    states[1318] = new State(new int[]{10,1326,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,144,281,147,282,145,284,146,285},new int[]{-113,1319,-152,1323,-156,49,-157,52,-174,1324,-176,279,-175,283});
    states[1319] = new State(new int[]{80,1320,10,1325});
    states[1320] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,144,281,147,282,145,284,146,285},new int[]{-113,1321,-152,1323,-156,49,-157,52,-174,1324,-176,279,-175,283});
    states[1321] = new State(new int[]{10,1322});
    states[1322] = new State(-492);
    states[1323] = new State(-495);
    states[1324] = new State(-496);
    states[1325] = new State(-493);
    states[1326] = new State(-494);
    states[1327] = new State(-403);
    states[1328] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-180,1329,-179,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1329] = new State(new int[]{8,565,10,-500,110,-500},new int[]{-133,1330});
    states[1330] = new State(new int[]{10,1360,110,-862},new int[]{-216,1331,-217,1356});
    states[1331] = new State(new int[]{23,1335,107,-354,91,-354,59,-354,29,-354,66,-354,50,-354,53,-354,62,-354,11,-354,28,-354,26,-354,44,-354,37,-354,19,-354,30,-354,31,-354,46,-354,27,-354,92,-354,84,-354,83,-354,82,-354,81,-354,149,-354,161,-354,41,-354},new int[]{-328,1332,-327,1333,-326,1355});
    states[1332] = new State(-489);
    states[1333] = new State(new int[]{23,1335,11,-355,92,-355,84,-355,83,-355,82,-355,81,-355,29,-355,143,-355,161,-355,85,-355,86,-355,80,-355,78,-355,160,-355,87,-355,62,-355,28,-355,26,-355,44,-355,37,-355,19,-355,30,-355,31,-355,46,-355,27,-355,10,-355,107,-355,91,-355,59,-355,66,-355,50,-355,53,-355,149,-355,41,-355},new int[]{-326,1334});
    states[1334] = new State(-357);
    states[1335] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1336,-152,809,-156,49,-157,52});
    states[1336] = new State(new int[]{5,1337,100,544});
    states[1337] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,1343,49,549,34,553,73,557,44,563,37,604,26,1352,30,1353},new int[]{-297,1338,-295,1354,-286,1342,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1338] = new State(new int[]{10,1339,100,1340});
    states[1339] = new State(-358);
    states[1340] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,1343,49,549,34,553,73,557,44,563,37,604,26,1352,30,1353},new int[]{-295,1341,-286,1342,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1341] = new State(-360);
    states[1342] = new State(-361);
    states[1343] = new State(new int[]{8,1344,10,-363,100,-363,23,-347,11,-347,92,-347,84,-347,83,-347,82,-347,81,-347,29,-347,143,-347,161,-347,85,-347,86,-347,80,-347,78,-347,160,-347,87,-347,62,-347,28,-347,26,-347,44,-347,37,-347,19,-347,30,-347,31,-347,46,-347,27,-347},new int[]{-192,503});
    states[1344] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-191,1345,-190,1351,-189,1349,-152,218,-156,49,-157,52,-310,1350});
    states[1345] = new State(new int[]{9,1346,100,1347});
    states[1346] = new State(-348);
    states[1347] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-190,1348,-189,1349,-152,218,-156,49,-157,52,-310,1350});
    states[1348] = new State(-350);
    states[1349] = new State(new int[]{7,182,123,187,9,-351,100,-351},new int[]{-308,636});
    states[1350] = new State(-352);
    states[1351] = new State(-349);
    states[1352] = new State(-362);
    states[1353] = new State(-364);
    states[1354] = new State(-359);
    states[1355] = new State(-356);
    states[1356] = new State(new int[]{110,1357});
    states[1357] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524},new int[]{-270,1358,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1358] = new State(new int[]{10,1359});
    states[1359] = new State(-474);
    states[1360] = new State(new int[]{148,1173,150,1174,151,1175,152,1176,154,1177,153,1178,23,-860,107,-860,91,-860,59,-860,29,-860,66,-860,50,-860,53,-860,62,-860,11,-860,28,-860,26,-860,44,-860,37,-860,19,-860,30,-860,31,-860,46,-860,27,-860,92,-860,84,-860,83,-860,82,-860,81,-860,149,-860},new int[]{-215,1361,-218,1179});
    states[1361] = new State(new int[]{10,1171,110,-863});
    states[1362] = new State(-404);
    states[1363] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-179,1364,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1364] = new State(new int[]{8,565,5,-500,10,-500,110,-500},new int[]{-133,1365});
    states[1365] = new State(new int[]{5,1368,10,1360,110,-862},new int[]{-216,1366,-217,1376});
    states[1366] = new State(new int[]{23,1335,107,-354,91,-354,59,-354,29,-354,66,-354,50,-354,53,-354,62,-354,11,-354,28,-354,26,-354,44,-354,37,-354,19,-354,30,-354,31,-354,46,-354,27,-354,92,-354,84,-354,83,-354,82,-354,81,-354,149,-354,161,-354,41,-354},new int[]{-328,1367,-327,1333,-326,1355});
    states[1367] = new State(-490);
    states[1368] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,1369,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1369] = new State(new int[]{10,1360,110,-862},new int[]{-216,1370,-217,1372});
    states[1370] = new State(new int[]{23,1335,107,-354,91,-354,59,-354,29,-354,66,-354,50,-354,53,-354,62,-354,11,-354,28,-354,26,-354,44,-354,37,-354,19,-354,30,-354,31,-354,46,-354,27,-354,92,-354,84,-354,83,-354,82,-354,81,-354,149,-354,161,-354,41,-354},new int[]{-328,1371,-327,1333,-326,1355});
    states[1371] = new State(-491);
    states[1372] = new State(new int[]{110,1373});
    states[1373] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,1374,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-333,882,-105,659,-334,677});
    states[1374] = new State(new int[]{10,1375});
    states[1375] = new State(-472);
    states[1376] = new State(new int[]{110,1377});
    states[1377] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,1378,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-333,882,-105,659,-334,677});
    states[1378] = new State(new int[]{10,1379});
    states[1379] = new State(-473);
    states[1380] = new State(-389);
    states[1381] = new State(-390);
    states[1382] = new State(-398);
    states[1383] = new State(new int[]{28,1380,26,1381,44,1328,37,1363},new int[]{-8,1384,-239,1387,-225,1388,-237,1316,-234,1327,-238,1362});
    states[1384] = new State(new int[]{44,1328,37,1363},new int[]{-239,1385,-225,1386,-237,1316,-234,1327,-238,1362});
    states[1385] = new State(-399);
    states[1386] = new State(-464);
    states[1387] = new State(-400);
    states[1388] = new State(-462);
    states[1389] = new State(new int[]{107,1318,11,-401,28,-401,26,-401,44,-401,37,-401,19,-401,30,-401,31,-401,46,-401,27,-401,92,-401,84,-401,83,-401,82,-401,81,-401,59,-95,29,-95,66,-95,50,-95,53,-95,62,-95,91,-95},new int[]{-185,1390,-47,1202,-43,1205,-65,1317});
    states[1390] = new State(-453);
    states[1391] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,8,-409,110,-409,10,-409},new int[]{-181,1392,-180,1180,-179,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1392] = new State(new int[]{8,565,110,-500,10,-500},new int[]{-133,1393});
    states[1393] = new State(new int[]{110,1395,10,1169},new int[]{-216,1394});
    states[1394] = new State(-405);
    states[1395] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524},new int[]{-270,1396,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1396] = new State(new int[]{10,1397});
    states[1397] = new State(-454);
    states[1398] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,8,-409,10,-409},new int[]{-181,1399,-180,1180,-179,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1399] = new State(new int[]{8,565,10,-500},new int[]{-133,1400});
    states[1400] = new State(new int[]{10,1169},new int[]{-216,1401});
    states[1401] = new State(-407);
    states[1402] = new State(-395);
    states[1403] = new State(-461);
    states[1404] = new State(-396);
    states[1405] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-179,1406,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1406] = new State(new int[]{11,1299,5,-422},new int[]{-242,1407,-247,1296});
    states[1407] = new State(new int[]{85,1285,86,1291,10,-429},new int[]{-211,1408});
    states[1408] = new State(new int[]{10,1409});
    states[1409] = new State(new int[]{63,1279,153,1281,152,1282,148,1283,151,1284,11,-419,28,-419,26,-419,44,-419,37,-419,19,-419,30,-419,31,-419,46,-419,27,-419,92,-419,84,-419,83,-419,82,-419,81,-419},new int[]{-214,1410,-219,1411});
    states[1410] = new State(-411);
    states[1411] = new State(new int[]{10,1412});
    states[1412] = new State(new int[]{63,1279,11,-419,28,-419,26,-419,44,-419,37,-419,19,-419,30,-419,31,-419,46,-419,27,-419,92,-419,84,-419,83,-419,82,-419,81,-419},new int[]{-214,1413});
    states[1413] = new State(-412);
    states[1414] = new State(new int[]{46,1415});
    states[1415] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-179,1416,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1416] = new State(new int[]{11,1299,5,-422},new int[]{-242,1417,-247,1296});
    states[1417] = new State(new int[]{110,1314,10,-418},new int[]{-220,1418});
    states[1418] = new State(new int[]{10,1419});
    states[1419] = new State(-415);
    states[1420] = new State(new int[]{11,628,92,-373,84,-373,83,-373,82,-373,81,-373,28,-244,26,-244,44,-244,37,-244,19,-244,30,-244,31,-244,46,-244,27,-244},new int[]{-58,514,-57,515,-11,516,-260,1155,-59,1421});
    states[1421] = new State(-385);
    states[1422] = new State(-382);
    states[1423] = new State(-339);
    states[1424] = new State(-340);
    states[1425] = new State(new int[]{26,1426,48,1427,43,1428,8,-341,23,-341,11,-341,92,-341,84,-341,83,-341,82,-341,81,-341,29,-341,143,-341,161,-341,85,-341,86,-341,80,-341,78,-341,160,-341,87,-341,62,-341,28,-341,44,-341,37,-341,19,-341,30,-341,31,-341,46,-341,27,-341,10,-341});
    states[1426] = new State(-342);
    states[1427] = new State(-343);
    states[1428] = new State(-344);
    states[1429] = new State(new int[]{68,1431,69,1432,148,1433,27,1434,28,1435,26,-336,43,-336,64,-336},new int[]{-24,1430});
    states[1430] = new State(-338);
    states[1431] = new State(-330);
    states[1432] = new State(-331);
    states[1433] = new State(-332);
    states[1434] = new State(-333);
    states[1435] = new State(-334);
    states[1436] = new State(-337);
    states[1437] = new State(new int[]{123,1439,120,-252},new int[]{-160,1438});
    states[1438] = new State(-253);
    states[1439] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1440,-152,809,-156,49,-157,52});
    states[1440] = new State(new int[]{122,1441,121,1188,100,544});
    states[1441] = new State(-254);
    states[1442] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604,68,1431,69,1432,148,1433,27,1434,28,1435,26,-335,43,-335,64,-335},new int[]{-296,1443,-286,1243,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562,-34,1244,-25,1245,-26,1429,-24,1436});
    states[1443] = new State(new int[]{10,1444});
    states[1444] = new State(-251);
    states[1445] = new State(new int[]{11,628,143,-244,161,-244,85,-244,86,-244,80,-244,78,-244,160,-244,87,-244},new int[]{-53,1446,-11,1237,-260,1155});
    states[1446] = new State(-136);
    states[1447] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1452,59,-122,29,-122,66,-122,50,-122,53,-122,62,-122,91,-122},new int[]{-324,1448,-321,1449,-322,1450,-163,810,-152,809,-156,49,-157,52});
    states[1448] = new State(-142);
    states[1449] = new State(-138);
    states[1450] = new State(new int[]{10,1451});
    states[1451] = new State(-436);
    states[1452] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1453,-156,49,-157,52});
    states[1453] = new State(new int[]{100,1454});
    states[1454] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-163,1455,-152,809,-156,49,-157,52});
    states[1455] = new State(new int[]{9,1456,100,544});
    states[1456] = new State(new int[]{110,1457});
    states[1457] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,1458,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[1458] = new State(new int[]{10,1459});
    states[1459] = new State(-139);
    states[1460] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1452},new int[]{-324,1461,-321,1449,-322,1450,-163,810,-152,809,-156,49,-157,52});
    states[1461] = new State(-140);
    states[1462] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1452},new int[]{-324,1463,-321,1449,-322,1450,-163,810,-152,809,-156,49,-157,52});
    states[1463] = new State(-141);
    states[1464] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,356,12,-311,100,-311},new int[]{-281,1465,-282,1466,-98,194,-111,314,-112,315,-189,352,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283});
    states[1465] = new State(-309);
    states[1466] = new State(-310);
    states[1467] = new State(-308);
    states[1468] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-286,1469,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1469] = new State(-307);
    states[1470] = new State(-277);
    states[1471] = new State(-278);
    states[1472] = new State(new int[]{127,483,121,-279,100,-279,120,-279,9,-279,10,-279,8,-279,138,-279,136,-279,118,-279,117,-279,131,-279,132,-279,133,-279,134,-279,130,-279,116,-279,115,-279,128,-279,129,-279,126,-279,6,-279,5,-279,125,-279,123,-279,124,-279,122,-279,137,-279,135,-279,16,-279,70,-279,92,-279,98,-279,101,-279,33,-279,104,-279,2,-279,12,-279,99,-279,32,-279,85,-279,84,-279,83,-279,82,-279,81,-279,86,-279,13,-279,76,-279,51,-279,58,-279,141,-279,143,-279,161,-279,80,-279,78,-279,160,-279,87,-279,45,-279,42,-279,21,-279,22,-279,144,-279,147,-279,145,-279,146,-279,155,-279,158,-279,157,-279,156,-279,11,-279,57,-279,91,-279,40,-279,25,-279,97,-279,54,-279,35,-279,55,-279,102,-279,47,-279,36,-279,53,-279,60,-279,74,-279,72,-279,38,-279,71,-279,110,-279});
    states[1473] = new State(-723);
    states[1474] = new State(new int[]{8,1475});
    states[1475] = new State(new int[]{14,472,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,474,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-364,1476,-362,1482,-20,473,-174,336,-176,279,-175,283,-21,337,-351,1473,-293,1474,-189,181,-152,218,-156,49,-157,52,-354,1480,-355,1481});
    states[1476] = new State(new int[]{9,1477,10,470,100,1478});
    states[1477] = new State(-681);
    states[1478] = new State(new int[]{14,472,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,53,474,143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,11,926,8,939},new int[]{-362,1479,-20,473,-174,336,-176,279,-175,283,-21,337,-351,1473,-293,1474,-189,181,-152,218,-156,49,-157,52,-354,1480,-355,1481});
    states[1479] = new State(-718);
    states[1480] = new State(-724);
    states[1481] = new State(-725);
    states[1482] = new State(-716);
    states[1483] = new State(-812);
    states[1484] = new State(new int[]{13,482});
    states[1485] = new State(-272);
    states[1486] = new State(-268);
    states[1487] = new State(-657);
    states[1488] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,712,8,713,21,294,22,299,76,446,11,368,40,589,5,598,18,669,37,678,44,682,9,-845},new int[]{-72,1489,-77,705,-94,1107,-92,708,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,709,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-333,1106,-105,659,-334,677});
    states[1489] = new State(new int[]{9,1490});
    states[1490] = new State(-676);
    states[1491] = new State(new int[]{8,1492});
    states[1492] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-344,1493,-343,1501,-152,1497,-156,49,-157,52,-106,1500,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[1493] = new State(new int[]{9,1494,100,1495});
    states[1494] = new State(-668);
    states[1495] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-343,1496,-152,1497,-156,49,-157,52,-106,1500,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[1496] = new State(-672);
    states[1497] = new State(new int[]{110,1498,11,-823,17,-823,8,-823,7,-823,142,-823,4,-823,15,-823,138,-823,136,-823,118,-823,117,-823,131,-823,132,-823,133,-823,134,-823,130,-823,116,-823,115,-823,128,-823,129,-823,126,-823,6,-823,120,-823,125,-823,123,-823,121,-823,124,-823,122,-823,137,-823,135,-823,16,-823,9,-823,100,-823,13,-823,119,-823});
    states[1498] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589},new int[]{-106,1499,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588});
    states[1499] = new State(-669);
    states[1500] = new State(-670);
    states[1501] = new State(-671);
    states[1502] = new State(new int[]{13,206,16,210,5,-738,12,-738});
    states[1503] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,1504,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[1504] = new State(new int[]{13,206,16,210,100,-221,9,-221,12,-221,5,-221});
    states[1505] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339,5,-739,12,-739},new int[]{-127,1506,-95,1502,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[1506] = new State(new int[]{5,1507,12,-745});
    states[1507] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,42,291,21,294,22,299,11,368,76,819,56,822,141,823,8,838,135,841,116,353,115,354,63,339},new int[]{-95,1508,-96,236,-86,246,-18,251,-15,261,-19,222,-152,262,-156,49,-157,52,-174,278,-176,279,-175,283,-21,286,-267,293,-304,298,-248,366,-249,367,-208,847,-182,845,-61,846,-275,853,-279,854,-16,849,-251,855});
    states[1508] = new State(new int[]{13,206,16,210,12,-747});
    states[1509] = new State(-218);
    states[1510] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285},new int[]{-98,1511,-111,314,-112,315,-189,352,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283});
    states[1511] = new State(new int[]{116,247,115,248,128,249,129,250,13,-281,121,-281,100,-281,120,-281,9,-281,10,-281,8,-281,138,-281,136,-281,118,-281,117,-281,131,-281,132,-281,133,-281,134,-281,130,-281,126,-281,6,-281,5,-281,125,-281,123,-281,124,-281,122,-281,137,-281,135,-281,16,-281,70,-281,92,-281,98,-281,101,-281,33,-281,104,-281,2,-281,12,-281,99,-281,32,-281,85,-281,84,-281,83,-281,82,-281,81,-281,86,-281,76,-281,51,-281,58,-281,141,-281,143,-281,161,-281,80,-281,78,-281,160,-281,87,-281,45,-281,42,-281,21,-281,22,-281,144,-281,147,-281,145,-281,146,-281,155,-281,158,-281,157,-281,156,-281,11,-281,57,-281,91,-281,40,-281,25,-281,97,-281,54,-281,35,-281,55,-281,102,-281,47,-281,36,-281,53,-281,60,-281,74,-281,72,-281,38,-281,71,-281,127,-281,110,-281},new int[]{-202,195});
    states[1512] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,24,494},new int[]{-293,1513,-288,1514,-189,181,-152,218,-156,49,-157,52,-280,500});
    states[1513] = new State(-770);
    states[1514] = new State(-771);
    states[1515] = new State(-784);
    states[1516] = new State(-785);
    states[1517] = new State(-786);
    states[1518] = new State(-787);
    states[1519] = new State(-788);
    states[1520] = new State(-789);
    states[1521] = new State(-790);
    states[1522] = new State(-760);
    states[1523] = new State(-679);
    states[1524] = new State(-35);
    states[1525] = new State(new int[]{59,1208,29,1229,66,1233,50,1445,53,1460,62,1462,11,628,91,-89,92,-89,103,-89,44,-244,37,-244,28,-244,26,-244,19,-244,30,-244,31,-244},new int[]{-51,1526,-177,1527,-33,1528,-56,1529,-298,1530,-320,1531,-229,1532,-11,1533,-260,1155});
    states[1526] = new State(-93);
    states[1527] = new State(-109);
    states[1528] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-110,29,-110,66,-110,50,-110,53,-110,62,-110,11,-110,44,-110,37,-110,28,-110,26,-110,19,-110,30,-110,31,-110,91,-110,92,-110,103,-110},new int[]{-31,1215,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1529] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,59,-111,29,-111,66,-111,50,-111,53,-111,62,-111,11,-111,44,-111,37,-111,28,-111,26,-111,19,-111,30,-111,31,-111,91,-111,92,-111,103,-111},new int[]{-31,1232,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1530] = new State(new int[]{11,628,59,-112,29,-112,66,-112,50,-112,53,-112,62,-112,44,-112,37,-112,28,-112,26,-112,19,-112,30,-112,31,-112,91,-112,92,-112,103,-112,143,-244,161,-244,85,-244,86,-244,80,-244,78,-244,160,-244,87,-244},new int[]{-53,1236,-11,1237,-260,1155});
    states[1531] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,8,1452,59,-113,29,-113,66,-113,50,-113,53,-113,62,-113,11,-113,44,-113,37,-113,28,-113,26,-113,19,-113,30,-113,31,-113,91,-113,92,-113,103,-113},new int[]{-324,1448,-321,1449,-322,1450,-163,810,-152,809,-156,49,-157,52});
    states[1532] = new State(-114);
    states[1533] = new State(new int[]{44,1546,37,1553,28,1380,26,1381,19,1576,30,1583,31,1398,11,628},new int[]{-222,1534,-260,518,-223,1535,-230,1536,-237,1537,-234,1327,-238,1362,-8,1566,-226,1580,-236,1581});
    states[1534] = new State(-117);
    states[1535] = new State(-115);
    states[1536] = new State(-456);
    states[1537] = new State(new int[]{149,1539,107,1318,59,-92,29,-92,66,-92,50,-92,53,-92,62,-92,11,-92,44,-92,37,-92,28,-92,26,-92,19,-92,30,-92,31,-92,91,-92},new int[]{-187,1538,-186,1541,-45,1542,-46,1525,-65,1545});
    states[1538] = new State(-466);
    states[1539] = new State(new int[]{10,1540});
    states[1540] = new State(-467);
    states[1541] = new State(-475);
    states[1542] = new State(new int[]{91,16},new int[]{-265,1543});
    states[1543] = new State(new int[]{10,1544});
    states[1544] = new State(-497);
    states[1545] = new State(-476);
    states[1546] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-180,1547,-179,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1547] = new State(new int[]{8,565,10,-500,110,-500},new int[]{-133,1548});
    states[1548] = new State(new int[]{10,1360,110,-862},new int[]{-216,1331,-217,1549});
    states[1549] = new State(new int[]{110,1550});
    states[1550] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524},new int[]{-270,1551,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1551] = new State(new int[]{10,1552});
    states[1552] = new State(-470);
    states[1553] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-179,1554,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1554] = new State(new int[]{8,565,5,-500,10,-500,110,-500},new int[]{-133,1555});
    states[1555] = new State(new int[]{5,1556,10,1360,110,-862},new int[]{-216,1366,-217,1562});
    states[1556] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,1557,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1557] = new State(new int[]{10,1360,110,-862},new int[]{-216,1370,-217,1558});
    states[1558] = new State(new int[]{110,1559});
    states[1559] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,1560,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-333,882,-105,659,-334,677});
    states[1560] = new State(new int[]{10,1561});
    states[1561] = new State(-468);
    states[1562] = new State(new int[]{110,1563});
    states[1563] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,713,21,294,22,299,76,446,11,368,40,589,18,669,37,678,44,682},new int[]{-108,1564,-106,881,-104,29,-103,156,-110,586,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,641,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-333,882,-105,659,-334,677});
    states[1564] = new State(new int[]{10,1565});
    states[1565] = new State(-469);
    states[1566] = new State(new int[]{19,1568,30,1570,44,1546,37,1553},new int[]{-230,1567,-237,1537,-234,1327,-238,1362});
    states[1567] = new State(-457);
    states[1568] = new State(new int[]{44,1546,37,1553},new int[]{-230,1569,-237,1537,-234,1327,-238,1362});
    states[1569] = new State(-460);
    states[1570] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,8,-409,110,-409,10,-409},new int[]{-181,1571,-180,1180,-179,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1571] = new State(new int[]{8,565,110,-500,10,-500},new int[]{-133,1572});
    states[1572] = new State(new int[]{110,1573,10,1169},new int[]{-216,526});
    states[1573] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524},new int[]{-270,1574,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1574] = new State(new int[]{10,1575});
    states[1575] = new State(-452);
    states[1576] = new State(new int[]{44,1546,37,1553,28,1380,26,1381},new int[]{-230,1577,-8,1578,-237,1537,-234,1327,-238,1362});
    states[1577] = new State(-458);
    states[1578] = new State(new int[]{44,1546,37,1553},new int[]{-230,1579,-237,1537,-234,1327,-238,1362});
    states[1579] = new State(-459);
    states[1580] = new State(-116);
    states[1581] = new State(-92,new int[]{-186,1582,-45,1542,-46,1525});
    states[1582] = new State(-450);
    states[1583] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405,8,-409,110,-409,10,-409},new int[]{-181,1584,-180,1180,-179,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1584] = new State(new int[]{8,565,110,-500,10,-500},new int[]{-133,1585});
    states[1585] = new State(new int[]{110,1586,10,1169},new int[]{-216,1394});
    states[1586] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,10,-524},new int[]{-270,1587,-9,22,-118,23,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885});
    states[1587] = new State(new int[]{10,1588});
    states[1588] = new State(-451);
    states[1589] = new State(new int[]{52,1591,91,-42,59,-42,29,-42,66,-42,50,-42,53,-42,62,-42,11,-42,44,-42,37,-42,28,-42,26,-42,19,-42,30,-42,31,-42,92,-42,103,-42},new int[]{-314,1590});
    states[1590] = new State(-44);
    states[1591] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-313,1592,-318,1602,-162,1596,-143,1601,-152,46,-156,49,-157,52,-302,57,-155,58,-303,148});
    states[1592] = new State(new int[]{10,1593,100,1594});
    states[1593] = new State(-38);
    states[1594] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-318,1595,-162,1596,-143,1601,-152,46,-156,49,-157,52,-302,57,-155,58,-303,148});
    states[1595] = new State(-46);
    states[1596] = new State(new int[]{7,1597,137,1599,10,-47,100,-47});
    states[1597] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-143,1598,-152,46,-156,49,-157,52,-302,57,-155,58,-303,148});
    states[1598] = new State(-37);
    states[1599] = new State(new int[]{144,1600});
    states[1600] = new State(-48);
    states[1601] = new State(-36);
    states[1602] = new State(-45);
    states[1603] = new State(-43);
    states[1604] = new State(new int[]{3,1606,52,-15,91,-15,59,-15,29,-15,66,-15,50,-15,53,-15,62,-15,11,-15,44,-15,37,-15,28,-15,26,-15,19,-15,30,-15,31,-15,43,-15,161,-15,92,-15,103,-15},new int[]{-193,1605});
    states[1605] = new State(-17);
    states[1606] = new State(new int[]{143,1607,144,1608});
    states[1607] = new State(-18);
    states[1608] = new State(-19);
    states[1609] = new State(-16);
    states[1610] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-152,1611,-156,49,-157,52});
    states[1611] = new State(new int[]{10,1613,8,1614},new int[]{-196,1612});
    states[1612] = new State(-28);
    states[1613] = new State(-29);
    states[1614] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-198,1615,-151,1621,-152,1620,-156,49,-157,52});
    states[1615] = new State(new int[]{9,1616,100,1618});
    states[1616] = new State(new int[]{10,1617});
    states[1617] = new State(-30);
    states[1618] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-151,1619,-152,1620,-156,49,-157,52});
    states[1619] = new State(-32);
    states[1620] = new State(-33);
    states[1621] = new State(-31);
    states[1622] = new State(-3);
    states[1623] = new State(new int[]{43,1644,52,1591,161,1678,59,-60,29,-60,66,-60,50,-60,53,-60,62,-60,11,-60,44,-60,37,-60,28,-60,26,-60,19,-60,30,-60,31,-60,92,-60,103,-60,91,-60},new int[]{-167,1624,-168,1641,-316,1702,-314,1603,-170,1717,-3,1704,-2,1718});
    states[1624] = new State(new int[]{41,1638},new int[]{-166,1625});
    states[1625] = new State(new int[]{92,1628,103,1629,91,1635},new int[]{-159,1626});
    states[1626] = new State(new int[]{7,1627});
    states[1627] = new State(-49);
    states[1628] = new State(-82);
    states[1629] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,104,-524,10,-524},new int[]{-262,1630,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[1630] = new State(new int[]{92,1631,104,1632,10,19});
    states[1631] = new State(-83);
    states[1632] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524},new int[]{-262,1633,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[1633] = new State(new int[]{92,1634,10,19});
    states[1634] = new State(-84);
    states[1635] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,92,-524,10,-524},new int[]{-262,1636,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040});
    states[1636] = new State(new int[]{92,1637,10,19});
    states[1637] = new State(-85);
    states[1638] = new State(new int[]{52,1591,59,-41,29,-41,66,-41,50,-41,53,-41,62,-41,11,-41,44,-41,37,-41,28,-41,26,-41,19,-41,30,-41,31,-41,92,-41,103,-41,91,-41},new int[]{-312,1639,-316,1589,-314,1603});
    states[1639] = new State(-92,new int[]{-45,1640,-46,1525});
    states[1640] = new State(-70);
    states[1641] = new State(new int[]{92,1628,103,1629,91,1635},new int[]{-159,1642});
    states[1642] = new State(new int[]{7,1643});
    states[1643] = new State(-50);
    states[1644] = new State(new int[]{52,1591,161,1678,29,-60,66,-60,50,-60,53,-60,62,-60,11,-60,44,-60,37,-60,41,-60},new int[]{-316,1645,-169,1697,-2,1698,-314,1603,-3,1647});
    states[1645] = new State(new int[]{161,1678,52,1591,29,-60,66,-60,50,-60,53,-60,62,-60,11,-60,44,-60,37,-60,41,-60},new int[]{-169,1646,-314,1590,-3,1647,-2,1696});
    states[1646] = new State(-56);
    states[1647] = new State(-87,new int[]{-44,1648,-42,1649});
    states[1648] = new State(-59);
    states[1649] = new State(new int[]{29,1229,66,1233,50,1445,53,1460,62,1462,11,628,41,-86,44,-244,37,-244},new int[]{-52,1650,-33,1651,-56,1670,-298,1671,-320,1674,-241,1676,-11,1679,-260,1155,-240,1694});
    states[1650] = new State(-88);
    states[1651] = new State(new int[]{143,47,161,1653,85,50,86,51,80,53,78,54,160,55,87,56,29,-97,66,-97,50,-97,53,-97,62,-97,11,-97,44,-97,37,-97,41,-97},new int[]{-31,1215,-6,1652,-32,1216,-146,1218,-152,1228,-156,49,-157,52,-2,1666});
    states[1652] = new State(-98);
    states[1653] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,120,-867,5,-867,123,-867,100,-867,110,-867},new int[]{-4,1654,-5,1665,-152,1658,-156,49,-157,52});
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
    states[1666] = new State(new int[]{161,1667,29,-108,66,-108,50,-108,53,-108,62,-108,11,-108,44,-108,37,-108,41,-108,59,-108,28,-108,26,-108,19,-108,30,-108,31,-108,92,-108,103,-108,91,-108});
    states[1667] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-4,1668,-5,1665,-152,1658,-156,49,-157,52});
    states[1668] = new State(new int[]{10,1669,100,1656});
    states[1669] = new State(-63);
    states[1670] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,29,-99,66,-99,50,-99,53,-99,62,-99,11,-99,44,-99,37,-99,41,-99},new int[]{-31,1232,-32,1216,-146,1218,-152,1228,-156,49,-157,52});
    states[1671] = new State(new int[]{11,628,29,-100,66,-100,50,-100,53,-100,62,-100,44,-100,37,-100,41,-100,161,-244,143,-244,85,-244,86,-244,80,-244,78,-244,160,-244,87,-244},new int[]{-53,1236,-11,1672,-260,1155});
    states[1672] = new State(new int[]{161,1653,143,47,85,50,86,51,80,53,78,54,160,55,87,56,11,628},new int[]{-6,1673,-54,1238,-260,518,-2,1666,-149,1239,-152,1437,-156,49,-157,52,-150,1442});
    states[1673] = new State(-101);
    states[1674] = new State(new int[]{143,47,161,1653,85,50,86,51,80,53,78,54,160,55,87,56,8,1452,29,-102,66,-102,50,-102,53,-102,62,-102,11,-102,44,-102,37,-102,41,-102},new int[]{-324,1448,-6,1675,-321,1449,-322,1450,-163,810,-152,809,-156,49,-157,52,-2,1666});
    states[1675] = new State(-103);
    states[1676] = new State(new int[]{161,1678,29,-104,66,-104,50,-104,53,-104,62,-104,11,-104,44,-104,37,-104,41,-104},new int[]{-6,1677,-2,1666});
    states[1677] = new State(-105);
    states[1678] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-4,1654,-5,1665,-152,1658,-156,49,-157,52});
    states[1679] = new State(new int[]{44,1686,11,628,37,1689},new int[]{-234,1680,-260,518,-238,1683});
    states[1680] = new State(new int[]{149,1681,161,-123,29,-123,66,-123,50,-123,53,-123,62,-123,11,-123,44,-123,37,-123,41,-123});
    states[1681] = new State(new int[]{10,1682});
    states[1682] = new State(-124);
    states[1683] = new State(new int[]{149,1684,161,-125,29,-125,66,-125,50,-125,53,-125,62,-125,11,-125,44,-125,37,-125,41,-125});
    states[1684] = new State(new int[]{10,1685});
    states[1685] = new State(-126);
    states[1686] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-180,1687,-179,1181,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1687] = new State(new int[]{8,565,10,-500},new int[]{-133,1688});
    states[1688] = new State(new int[]{10,1169},new int[]{-216,1331});
    states[1689] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,45,405},new int[]{-179,1690,-147,1182,-142,1183,-139,1184,-152,1189,-156,49,-157,52,-200,1190,-345,1192,-154,1196});
    states[1690] = new State(new int[]{8,565,5,-500,10,-500},new int[]{-133,1691});
    states[1691] = new State(new int[]{5,1692,10,1169},new int[]{-216,1366});
    states[1692] = new State(new int[]{143,362,161,48,85,50,86,51,80,53,78,54,160,55,87,56,155,287,158,288,157,289,156,290,116,353,115,354,144,281,147,282,145,284,146,285,8,478,142,489,24,494,48,502,49,549,34,553,73,557,44,563,37,604},new int[]{-285,1693,-286,491,-282,361,-98,194,-111,314,-112,315,-189,316,-152,218,-156,49,-157,52,-21,349,-208,350,-174,355,-176,279,-175,283,-283,480,-310,481,-266,487,-259,488,-291,492,-288,493,-280,500,-35,501,-273,548,-135,552,-136,556,-235,560,-233,561,-232,562});
    states[1693] = new State(new int[]{10,1169},new int[]{-216,1370});
    states[1694] = new State(new int[]{161,1678,29,-106,66,-106,50,-106,53,-106,62,-106,11,-106,44,-106,37,-106,41,-106},new int[]{-6,1695,-2,1666});
    states[1695] = new State(-107);
    states[1696] = new State(new int[]{161,1667,29,-61,66,-61,50,-61,53,-61,62,-61,11,-61,44,-61,37,-61,41,-61,59,-61,28,-61,26,-61,19,-61,30,-61,31,-61,92,-61,103,-61,91,-61});
    states[1697] = new State(-57);
    states[1698] = new State(new int[]{161,1667,52,1591,29,-61,66,-61,50,-61,53,-61,62,-61,11,-61,44,-61,37,-61,41,-61},new int[]{-316,1699,-314,1603});
    states[1699] = new State(new int[]{161,1678,52,1591,29,-60,66,-60,50,-60,53,-60,62,-60,11,-60,44,-60,37,-60,41,-60},new int[]{-3,1700,-314,1590,-2,1696});
    states[1700] = new State(-87,new int[]{-44,1701,-42,1649});
    states[1701] = new State(-58);
    states[1702] = new State(new int[]{161,1678,52,1591,59,-60,29,-60,66,-60,50,-60,53,-60,62,-60,11,-60,44,-60,37,-60,28,-60,26,-60,19,-60,30,-60,31,-60,92,-60,103,-60,91,-60},new int[]{-170,1703,-314,1590,-3,1704,-2,1696});
    states[1703] = new State(-71);
    states[1704] = new State(-75,new int[]{-171,1705});
    states[1705] = new State(new int[]{59,1208,29,1229,66,1233,50,1445,53,1460,62,1462,11,628,92,-74,103,-74,91,-74,44,-244,37,-244,28,-244,26,-244,19,-244,30,-244,31,-244},new int[]{-172,1706,-51,1707,-177,1527,-33,1708,-56,1529,-298,1710,-320,1713,-229,1715,-11,1533,-260,1155});
    states[1706] = new State(-76);
    states[1707] = new State(-77);
    states[1708] = new State(new int[]{143,47,161,1653,85,50,86,51,80,53,78,54,160,55,87,56,59,-110,29,-110,66,-110,50,-110,53,-110,62,-110,11,-110,44,-110,37,-110,28,-110,26,-110,19,-110,30,-110,31,-110,92,-110,103,-110,91,-110},new int[]{-31,1215,-6,1709,-32,1216,-146,1218,-152,1228,-156,49,-157,52,-2,1666});
    states[1709] = new State(-78);
    states[1710] = new State(new int[]{11,628,59,-112,29,-112,66,-112,50,-112,53,-112,62,-112,44,-112,37,-112,28,-112,26,-112,19,-112,30,-112,31,-112,92,-112,103,-112,91,-112,161,-244,143,-244,85,-244,86,-244,80,-244,78,-244,160,-244,87,-244},new int[]{-53,1236,-11,1711,-260,1155});
    states[1711] = new State(new int[]{161,1653,143,47,85,50,86,51,80,53,78,54,160,55,87,56,11,628},new int[]{-6,1712,-54,1238,-260,518,-2,1666,-149,1239,-152,1437,-156,49,-157,52,-150,1442});
    states[1712] = new State(-80);
    states[1713] = new State(new int[]{143,47,161,1653,85,50,86,51,80,53,78,54,160,55,87,56,8,1452,59,-113,29,-113,66,-113,50,-113,53,-113,62,-113,11,-113,44,-113,37,-113,28,-113,26,-113,19,-113,30,-113,31,-113,92,-113,103,-113,91,-113},new int[]{-324,1448,-6,1714,-321,1449,-322,1450,-163,810,-152,809,-156,49,-157,52,-2,1666});
    states[1714] = new State(-79);
    states[1715] = new State(new int[]{161,1678,59,-114,29,-114,66,-114,50,-114,53,-114,62,-114,11,-114,44,-114,37,-114,28,-114,26,-114,19,-114,30,-114,31,-114,92,-114,103,-114,91,-114},new int[]{-6,1716,-2,1666});
    states[1716] = new State(-81);
    states[1717] = new State(-72);
    states[1718] = new State(new int[]{161,1667,52,1591,59,-61,29,-61,66,-61,50,-61,53,-61,62,-61,11,-61,44,-61,37,-61,28,-61,26,-61,19,-61,30,-61,31,-61,92,-61,103,-61,91,-61},new int[]{-316,1719,-314,1603});
    states[1719] = new State(new int[]{161,1678,52,1591,59,-60,29,-60,66,-60,50,-60,53,-60,62,-60,11,-60,44,-60,37,-60,28,-60,26,-60,19,-60,30,-60,31,-60,92,-60,103,-60,91,-60},new int[]{-3,1720,-314,1590,-2,1696});
    states[1720] = new State(-75,new int[]{-171,1721});
    states[1721] = new State(new int[]{59,1208,29,1229,66,1233,50,1445,53,1460,62,1462,11,628,92,-73,103,-73,91,-73,44,-244,37,-244,28,-244,26,-244,19,-244,30,-244,31,-244},new int[]{-172,1706,-51,1707,-177,1527,-33,1708,-56,1529,-298,1710,-320,1713,-229,1715,-11,1533,-260,1155});
    states[1722] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-144,1723,-152,1726,-156,49,-157,52});
    states[1723] = new State(new int[]{10,1724});
    states[1724] = new State(new int[]{3,1606,43,-14,52,-14,161,-14,92,-14,103,-14,91,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-194,1725,-195,1604,-193,1609});
    states[1725] = new State(-51);
    states[1726] = new State(-55);
    states[1727] = new State(-53);
    states[1728] = new State(-54);
    states[1729] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-162,1730,-143,1601,-152,46,-156,49,-157,52,-302,57,-155,58,-303,148});
    states[1730] = new State(new int[]{10,1731,7,1597});
    states[1731] = new State(new int[]{3,1606,43,-14,52,-14,161,-14,92,-14,103,-14,91,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-194,1732,-195,1604,-193,1609});
    states[1732] = new State(-52);
    states[1733] = new State(-4);
    states[1734] = new State(new int[]{50,1736,56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,437,21,294,22,299,76,446,11,368,40,589,5,598},new int[]{-92,1735,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,397,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597});
    states[1735] = new State(new int[]{70,27,2,-7});
    states[1736] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-149,1737,-152,1738,-156,49,-157,52});
    states[1737] = new State(-8);
    states[1738] = new State(new int[]{123,1186,2,-252},new int[]{-160,1438});
    states[1739] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56},new int[]{-331,1740,-332,1741,-152,1745,-156,49,-157,52});
    states[1740] = new State(-9);
    states[1741] = new State(new int[]{7,1742,123,187,2,-817},new int[]{-308,1744});
    states[1742] = new State(new int[]{143,47,161,48,85,50,86,51,80,53,78,54,160,55,87,56,84,59,83,60,82,61,81,62,68,63,64,64,128,65,22,66,21,67,63,68,23,69,129,70,130,71,131,72,132,73,133,74,134,75,135,76,136,77,137,78,138,79,24,80,73,81,91,82,25,83,26,84,29,85,30,86,31,87,71,88,99,89,32,90,92,91,33,92,34,93,27,94,104,95,101,96,35,97,36,98,37,99,40,100,41,101,42,102,103,103,43,104,44,105,46,106,47,107,48,108,97,109,49,110,102,111,50,112,28,113,51,114,70,115,98,116,52,117,53,118,54,119,55,120,56,121,57,122,58,123,59,124,61,125,105,126,106,127,109,128,107,129,108,130,62,131,74,132,38,133,39,134,69,135,148,136,60,137,139,138,140,139,79,140,153,141,152,142,72,143,154,144,150,145,151,146,149,147,45,149},new int[]{-143,1743,-152,46,-156,49,-157,52,-302,57,-155,58,-303,148});
    states[1743] = new State(-816);
    states[1744] = new State(-818);
    states[1745] = new State(-815);
    states[1746] = new State(new int[]{56,42,144,281,147,282,145,284,146,285,155,287,158,288,157,289,156,290,63,339,135,377,116,353,115,354,142,387,141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,435,8,531,21,294,22,299,76,446,11,368,40,589,5,598,53,800},new int[]{-269,1747,-92,1748,-106,155,-104,29,-103,156,-110,166,-88,171,-87,177,-101,335,-20,43,-174,336,-176,279,-175,283,-21,337,-61,338,-208,384,-118,1123,-137,391,-116,399,-152,403,-156,49,-157,52,-200,404,-267,444,-304,445,-249,450,-117,451,-62,452,-121,458,-182,459,-278,460,-102,461,-274,465,-276,466,-277,578,-250,579,-122,580,-252,588,-125,597,-9,1749,-325,1750});
    states[1747] = new State(-10);
    states[1748] = new State(new int[]{70,27,2,-11});
    states[1749] = new State(-12);
    states[1750] = new State(-13);
    states[1751] = new State(new int[]{52,1591,141,-39,143,-39,161,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,11,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,44,-39,37,-39,10,-39,2,-39},new int[]{-315,1752,-314,1756});
    states[1752] = new State(-90,new int[]{-48,1753});
    states[1753] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,44,1546,37,1553,10,-524,2,-524},new int[]{-262,1754,-230,1755,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040,-237,1537,-234,1327,-238,1362});
    states[1754] = new State(new int[]{10,19,2,-5});
    states[1755] = new State(-91);
    states[1756] = new State(-40);
    states[1757] = new State(new int[]{52,1591,141,-39,143,-39,161,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,11,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,44,-39,37,-39,10,-39,2,-39},new int[]{-315,1758,-314,1756});
    states[1758] = new State(-90,new int[]{-48,1759});
    states[1759] = new State(new int[]{141,398,143,47,161,48,85,50,86,51,80,53,78,263,160,55,87,56,45,405,42,530,8,531,21,294,22,299,144,281,147,282,145,284,146,285,155,752,158,288,157,289,156,290,76,446,11,368,57,731,91,16,40,721,25,738,97,754,54,759,35,764,55,775,102,781,47,788,36,791,53,800,60,872,74,877,72,864,38,886,44,1546,37,1553,10,-524,2,-524},new int[]{-262,1760,-230,1755,-271,750,-270,21,-9,22,-118,23,-137,391,-116,399,-152,751,-156,49,-157,52,-200,404,-267,444,-304,445,-20,727,-174,336,-176,279,-175,283,-21,337,-249,450,-117,451,-62,728,-121,458,-221,729,-138,730,-265,735,-158,736,-39,737,-257,753,-329,758,-129,763,-330,774,-165,779,-311,780,-258,787,-128,790,-325,799,-63,868,-183,869,-182,870,-178,871,-131,876,-132,883,-130,884,-359,885,-148,1040,-237,1537,-234,1327,-238,1362});
    states[1760] = new State(new int[]{10,19,2,-6});

    rules[1] = new Rule(-372, new int[]{-1,2});
    rules[2] = new Rule(-1, new int[]{-243});
    rules[3] = new Rule(-1, new int[]{-317});
    rules[4] = new Rule(-1, new int[]{-184});
    rules[5] = new Rule(-1, new int[]{75,-315,-48,-262});
    rules[6] = new Rule(-1, new int[]{77,-315,-48,-262});
    rules[7] = new Rule(-184, new int[]{88,-92});
    rules[8] = new Rule(-184, new int[]{88,50,-149});
    rules[9] = new Rule(-184, new int[]{90,-331});
    rules[10] = new Rule(-184, new int[]{89,-269});
    rules[11] = new Rule(-269, new int[]{-92});
    rules[12] = new Rule(-269, new int[]{-9});
    rules[13] = new Rule(-269, new int[]{-325});
    rules[14] = new Rule(-194, new int[]{});
    rules[15] = new Rule(-194, new int[]{-195});
    rules[16] = new Rule(-195, new int[]{-193});
    rules[17] = new Rule(-195, new int[]{-195,-193});
    rules[18] = new Rule(-193, new int[]{3,143});
    rules[19] = new Rule(-193, new int[]{3,144});
    rules[20] = new Rule(-243, new int[]{-244,-194,-312,-22,-197});
    rules[21] = new Rule(-197, new int[]{7});
    rules[22] = new Rule(-197, new int[]{10});
    rules[23] = new Rule(-197, new int[]{5});
    rules[24] = new Rule(-197, new int[]{100});
    rules[25] = new Rule(-197, new int[]{6});
    rules[26] = new Rule(-197, new int[]{});
    rules[27] = new Rule(-244, new int[]{});
    rules[28] = new Rule(-244, new int[]{61,-152,-196});
    rules[29] = new Rule(-196, new int[]{10});
    rules[30] = new Rule(-196, new int[]{8,-198,9,10});
    rules[31] = new Rule(-198, new int[]{-151});
    rules[32] = new Rule(-198, new int[]{-198,100,-151});
    rules[33] = new Rule(-151, new int[]{-152});
    rules[34] = new Rule(-22, new int[]{-41,-265});
    rules[35] = new Rule(-41, new int[]{-45});
    rules[36] = new Rule(-162, new int[]{-143});
    rules[37] = new Rule(-162, new int[]{-162,7,-143});
    rules[38] = new Rule(-314, new int[]{52,-313,10});
    rules[39] = new Rule(-315, new int[]{});
    rules[40] = new Rule(-315, new int[]{-314});
    rules[41] = new Rule(-312, new int[]{});
    rules[42] = new Rule(-312, new int[]{-316});
    rules[43] = new Rule(-316, new int[]{-314});
    rules[44] = new Rule(-316, new int[]{-316,-314});
    rules[45] = new Rule(-313, new int[]{-318});
    rules[46] = new Rule(-313, new int[]{-313,100,-318});
    rules[47] = new Rule(-318, new int[]{-162});
    rules[48] = new Rule(-318, new int[]{-162,137,144});
    rules[49] = new Rule(-317, new int[]{-319,-167,-166,-159,7});
    rules[50] = new Rule(-317, new int[]{-319,-168,-159,7});
    rules[51] = new Rule(-319, new int[]{-7,-144,10,-194});
    rules[52] = new Rule(-319, new int[]{109,-162,10,-194});
    rules[53] = new Rule(-7, new int[]{105});
    rules[54] = new Rule(-7, new int[]{106});
    rules[55] = new Rule(-144, new int[]{-152});
    rules[56] = new Rule(-167, new int[]{43,-316,-169});
    rules[57] = new Rule(-167, new int[]{43,-169});
    rules[58] = new Rule(-167, new int[]{43,-2,-316,-3,-44});
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
    rules[70] = new Rule(-166, new int[]{41,-312,-45});
    rules[71] = new Rule(-168, new int[]{-316,-170});
    rules[72] = new Rule(-168, new int[]{-170});
    rules[73] = new Rule(-168, new int[]{-2,-316,-3,-171});
    rules[74] = new Rule(-170, new int[]{-3,-171});
    rules[75] = new Rule(-171, new int[]{});
    rules[76] = new Rule(-171, new int[]{-171,-172});
    rules[77] = new Rule(-172, new int[]{-51});
    rules[78] = new Rule(-172, new int[]{-33,-6});
    rules[79] = new Rule(-172, new int[]{-320,-6});
    rules[80] = new Rule(-172, new int[]{-298,-11,-6});
    rules[81] = new Rule(-172, new int[]{-229,-6});
    rules[82] = new Rule(-159, new int[]{92});
    rules[83] = new Rule(-159, new int[]{103,-262,92});
    rules[84] = new Rule(-159, new int[]{103,-262,104,-262,92});
    rules[85] = new Rule(-159, new int[]{91,-262,92});
    rules[86] = new Rule(-44, new int[]{-42});
    rules[87] = new Rule(-42, new int[]{});
    rules[88] = new Rule(-42, new int[]{-42,-52});
    rules[89] = new Rule(-45, new int[]{-46});
    rules[90] = new Rule(-48, new int[]{});
    rules[91] = new Rule(-48, new int[]{-48,-230});
    rules[92] = new Rule(-46, new int[]{});
    rules[93] = new Rule(-46, new int[]{-46,-51});
    rules[94] = new Rule(-47, new int[]{-43});
    rules[95] = new Rule(-43, new int[]{});
    rules[96] = new Rule(-43, new int[]{-43,-50});
    rules[97] = new Rule(-52, new int[]{-33});
    rules[98] = new Rule(-52, new int[]{-33,-6});
    rules[99] = new Rule(-52, new int[]{-56});
    rules[100] = new Rule(-52, new int[]{-298});
    rules[101] = new Rule(-52, new int[]{-298,-11,-6});
    rules[102] = new Rule(-52, new int[]{-320});
    rules[103] = new Rule(-52, new int[]{-320,-6});
    rules[104] = new Rule(-52, new int[]{-241});
    rules[105] = new Rule(-52, new int[]{-241,-6});
    rules[106] = new Rule(-52, new int[]{-240});
    rules[107] = new Rule(-52, new int[]{-240,-6});
    rules[108] = new Rule(-6, new int[]{-2});
    rules[109] = new Rule(-51, new int[]{-177});
    rules[110] = new Rule(-51, new int[]{-33});
    rules[111] = new Rule(-51, new int[]{-56});
    rules[112] = new Rule(-51, new int[]{-298});
    rules[113] = new Rule(-51, new int[]{-320});
    rules[114] = new Rule(-51, new int[]{-229});
    rules[115] = new Rule(-222, new int[]{-223});
    rules[116] = new Rule(-222, new int[]{-226});
    rules[117] = new Rule(-229, new int[]{-11,-222});
    rules[118] = new Rule(-50, new int[]{-177});
    rules[119] = new Rule(-50, new int[]{-33});
    rules[120] = new Rule(-50, new int[]{-56});
    rules[121] = new Rule(-50, new int[]{-298});
    rules[122] = new Rule(-50, new int[]{-320});
    rules[123] = new Rule(-241, new int[]{-11,-234});
    rules[124] = new Rule(-241, new int[]{-11,-234,149,10});
    rules[125] = new Rule(-240, new int[]{-11,-238});
    rules[126] = new Rule(-240, new int[]{-11,-238,149,10});
    rules[127] = new Rule(-177, new int[]{59,-161,10});
    rules[128] = new Rule(-161, new int[]{-148});
    rules[129] = new Rule(-161, new int[]{-161,100,-148});
    rules[130] = new Rule(-148, new int[]{155});
    rules[131] = new Rule(-148, new int[]{-152});
    rules[132] = new Rule(-33, new int[]{29,-31});
    rules[133] = new Rule(-33, new int[]{-33,-31});
    rules[134] = new Rule(-56, new int[]{66,-31});
    rules[135] = new Rule(-56, new int[]{-56,-31});
    rules[136] = new Rule(-298, new int[]{50,-53});
    rules[137] = new Rule(-298, new int[]{-298,-53});
    rules[138] = new Rule(-324, new int[]{-321});
    rules[139] = new Rule(-324, new int[]{8,-152,100,-163,9,110,-106,10});
    rules[140] = new Rule(-320, new int[]{53,-324});
    rules[141] = new Rule(-320, new int[]{62,-324});
    rules[142] = new Rule(-320, new int[]{-320,-324});
    rules[143] = new Rule(-31, new int[]{-32,10});
    rules[144] = new Rule(-32, new int[]{-146,120,-114});
    rules[145] = new Rule(-32, new int[]{-146,5,-286,120,-89});
    rules[146] = new Rule(-114, new int[]{-95});
    rules[147] = new Rule(-114, new int[]{-100});
    rules[148] = new Rule(-146, new int[]{-152});
    rules[149] = new Rule(-96, new int[]{-86});
    rules[150] = new Rule(-96, new int[]{-96,-201,-86});
    rules[151] = new Rule(-95, new int[]{-96});
    rules[152] = new Rule(-95, new int[]{-251});
    rules[153] = new Rule(-95, new int[]{-95,16,-96});
    rules[154] = new Rule(-251, new int[]{-95,13,-95,5,-95});
    rules[155] = new Rule(-201, new int[]{120});
    rules[156] = new Rule(-201, new int[]{125});
    rules[157] = new Rule(-201, new int[]{123});
    rules[158] = new Rule(-201, new int[]{121});
    rules[159] = new Rule(-201, new int[]{124});
    rules[160] = new Rule(-201, new int[]{122});
    rules[161] = new Rule(-201, new int[]{137});
    rules[162] = new Rule(-201, new int[]{135,137});
    rules[163] = new Rule(-86, new int[]{-18});
    rules[164] = new Rule(-86, new int[]{-86,-202,-18});
    rules[165] = new Rule(-202, new int[]{116});
    rules[166] = new Rule(-202, new int[]{115});
    rules[167] = new Rule(-202, new int[]{128});
    rules[168] = new Rule(-202, new int[]{129});
    rules[169] = new Rule(-275, new int[]{-18,-210,-293});
    rules[170] = new Rule(-279, new int[]{-16,119,-15});
    rules[171] = new Rule(-279, new int[]{-16,119,-279});
    rules[172] = new Rule(-279, new int[]{-208,-279});
    rules[173] = new Rule(-18, new int[]{-15});
    rules[174] = new Rule(-18, new int[]{-275});
    rules[175] = new Rule(-18, new int[]{-279});
    rules[176] = new Rule(-18, new int[]{-18,-204,-15});
    rules[177] = new Rule(-18, new int[]{-18,-204,-279});
    rules[178] = new Rule(-204, new int[]{118});
    rules[179] = new Rule(-204, new int[]{117});
    rules[180] = new Rule(-204, new int[]{131});
    rules[181] = new Rule(-204, new int[]{132});
    rules[182] = new Rule(-204, new int[]{133});
    rules[183] = new Rule(-204, new int[]{134});
    rules[184] = new Rule(-204, new int[]{130});
    rules[185] = new Rule(-16, new int[]{-19});
    rules[186] = new Rule(-16, new int[]{8,-95,9});
    rules[187] = new Rule(-15, new int[]{-19});
    rules[188] = new Rule(-15, new int[]{-248});
    rules[189] = new Rule(-15, new int[]{56});
    rules[190] = new Rule(-15, new int[]{141,-15});
    rules[191] = new Rule(-15, new int[]{8,-95,9});
    rules[192] = new Rule(-15, new int[]{135,-15});
    rules[193] = new Rule(-15, new int[]{-208,-15});
    rules[194] = new Rule(-15, new int[]{-182});
    rules[195] = new Rule(-15, new int[]{-61});
    rules[196] = new Rule(-249, new int[]{11,-73,12});
    rules[197] = new Rule(-248, new int[]{-249});
    rules[198] = new Rule(-248, new int[]{76,-73,76});
    rules[199] = new Rule(-208, new int[]{116});
    rules[200] = new Rule(-208, new int[]{115});
    rules[201] = new Rule(-19, new int[]{-152});
    rules[202] = new Rule(-19, new int[]{-174});
    rules[203] = new Rule(-19, new int[]{-21});
    rules[204] = new Rule(-19, new int[]{42,-152});
    rules[205] = new Rule(-19, new int[]{-267});
    rules[206] = new Rule(-19, new int[]{-304});
    rules[207] = new Rule(-19, new int[]{-19,-17});
    rules[208] = new Rule(-19, new int[]{-19,4,-308});
    rules[209] = new Rule(-19, new int[]{-19,11,-126,12});
    rules[210] = new Rule(-17, new int[]{7,-143});
    rules[211] = new Rule(-17, new int[]{142});
    rules[212] = new Rule(-17, new int[]{8,-81,9});
    rules[213] = new Rule(-17, new int[]{11,-80,12});
    rules[214] = new Rule(-81, new int[]{-76});
    rules[215] = new Rule(-81, new int[]{});
    rules[216] = new Rule(-80, new int[]{-78});
    rules[217] = new Rule(-80, new int[]{});
    rules[218] = new Rule(-78, new int[]{-99});
    rules[219] = new Rule(-78, new int[]{-78,100,-99});
    rules[220] = new Rule(-99, new int[]{-95});
    rules[221] = new Rule(-99, new int[]{-95,6,-95});
    rules[222] = new Rule(-21, new int[]{155});
    rules[223] = new Rule(-21, new int[]{158});
    rules[224] = new Rule(-21, new int[]{157});
    rules[225] = new Rule(-21, new int[]{156});
    rules[226] = new Rule(-89, new int[]{-95});
    rules[227] = new Rule(-89, new int[]{-100});
    rules[228] = new Rule(-89, new int[]{-253});
    rules[229] = new Rule(-100, new int[]{8,-70,9});
    rules[230] = new Rule(-70, new int[]{});
    rules[231] = new Rule(-70, new int[]{-69});
    rules[232] = new Rule(-69, new int[]{-90});
    rules[233] = new Rule(-69, new int[]{-69,100,-90});
    rules[234] = new Rule(-253, new int[]{8,-255,9});
    rules[235] = new Rule(-255, new int[]{-254});
    rules[236] = new Rule(-255, new int[]{-254,10});
    rules[237] = new Rule(-254, new int[]{-256});
    rules[238] = new Rule(-254, new int[]{-254,10,-256});
    rules[239] = new Rule(-256, new int[]{-141,5,-89});
    rules[240] = new Rule(-141, new int[]{-152});
    rules[241] = new Rule(-53, new int[]{-11,-54});
    rules[242] = new Rule(-11, new int[]{-260});
    rules[243] = new Rule(-11, new int[]{-11,-260});
    rules[244] = new Rule(-11, new int[]{});
    rules[245] = new Rule(-260, new int[]{11,-261,12});
    rules[246] = new Rule(-261, new int[]{-13});
    rules[247] = new Rule(-261, new int[]{-261,100,-13});
    rules[248] = new Rule(-13, new int[]{-14});
    rules[249] = new Rule(-13, new int[]{-152,5,-14});
    rules[250] = new Rule(-54, new int[]{-149,120,-296,10});
    rules[251] = new Rule(-54, new int[]{-150,-296,10});
    rules[252] = new Rule(-149, new int[]{-152});
    rules[253] = new Rule(-149, new int[]{-152,-160});
    rules[254] = new Rule(-150, new int[]{-152,123,-163,122});
    rules[255] = new Rule(-296, new int[]{-286});
    rules[256] = new Rule(-296, new int[]{-34});
    rules[257] = new Rule(-283, new int[]{-282,13});
    rules[258] = new Rule(-283, new int[]{-310,13});
    rules[259] = new Rule(-286, new int[]{-282});
    rules[260] = new Rule(-286, new int[]{-283});
    rules[261] = new Rule(-286, new int[]{-266});
    rules[262] = new Rule(-286, new int[]{-259});
    rules[263] = new Rule(-286, new int[]{-291});
    rules[264] = new Rule(-286, new int[]{-235});
    rules[265] = new Rule(-286, new int[]{-310});
    rules[266] = new Rule(-310, new int[]{-189,-308});
    rules[267] = new Rule(-308, new int[]{123,-306,121});
    rules[268] = new Rule(-309, new int[]{125});
    rules[269] = new Rule(-309, new int[]{123,-307,121});
    rules[270] = new Rule(-306, new int[]{-289});
    rules[271] = new Rule(-306, new int[]{-306,100,-289});
    rules[272] = new Rule(-307, new int[]{-290});
    rules[273] = new Rule(-307, new int[]{-307,100,-290});
    rules[274] = new Rule(-290, new int[]{});
    rules[275] = new Rule(-289, new int[]{-282});
    rules[276] = new Rule(-289, new int[]{-282,13});
    rules[277] = new Rule(-289, new int[]{-291});
    rules[278] = new Rule(-289, new int[]{-235});
    rules[279] = new Rule(-289, new int[]{-310});
    rules[280] = new Rule(-282, new int[]{-98});
    rules[281] = new Rule(-282, new int[]{-98,6,-98});
    rules[282] = new Rule(-282, new int[]{8,-85,9});
    rules[283] = new Rule(-98, new int[]{-111});
    rules[284] = new Rule(-98, new int[]{-98,-202,-111});
    rules[285] = new Rule(-111, new int[]{-112});
    rules[286] = new Rule(-111, new int[]{-111,-204,-112});
    rules[287] = new Rule(-112, new int[]{-189});
    rules[288] = new Rule(-112, new int[]{-21});
    rules[289] = new Rule(-112, new int[]{-208,-112});
    rules[290] = new Rule(-112, new int[]{-174});
    rules[291] = new Rule(-112, new int[]{-112,8,-80,9});
    rules[292] = new Rule(-189, new int[]{-152});
    rules[293] = new Rule(-189, new int[]{-189,7,-143});
    rules[294] = new Rule(-85, new int[]{-83});
    rules[295] = new Rule(-85, new int[]{-85,100,-83});
    rules[296] = new Rule(-83, new int[]{-286});
    rules[297] = new Rule(-83, new int[]{-286,120,-92});
    rules[298] = new Rule(-259, new int[]{142,-285});
    rules[299] = new Rule(-291, new int[]{-288});
    rules[300] = new Rule(-291, new int[]{-35});
    rules[301] = new Rule(-291, new int[]{-273});
    rules[302] = new Rule(-291, new int[]{-135});
    rules[303] = new Rule(-291, new int[]{-136});
    rules[304] = new Rule(-136, new int[]{73,58,-286});
    rules[305] = new Rule(-288, new int[]{24,11,-173,12,58,-286});
    rules[306] = new Rule(-288, new int[]{-280});
    rules[307] = new Rule(-280, new int[]{24,58,-286});
    rules[308] = new Rule(-173, new int[]{-281});
    rules[309] = new Rule(-173, new int[]{-173,100,-281});
    rules[310] = new Rule(-281, new int[]{-282});
    rules[311] = new Rule(-281, new int[]{});
    rules[312] = new Rule(-273, new int[]{49,58,-286});
    rules[313] = new Rule(-135, new int[]{34,58,-286});
    rules[314] = new Rule(-135, new int[]{34});
    rules[315] = new Rule(-266, new int[]{143,11,-95,12});
    rules[316] = new Rule(-235, new int[]{-233});
    rules[317] = new Rule(-233, new int[]{-232});
    rules[318] = new Rule(-232, new int[]{44,-133});
    rules[319] = new Rule(-232, new int[]{37,-133,5,-285});
    rules[320] = new Rule(-232, new int[]{-189,127,-289});
    rules[321] = new Rule(-232, new int[]{-310,127,-289});
    rules[322] = new Rule(-232, new int[]{8,9,127,-289});
    rules[323] = new Rule(-232, new int[]{8,-85,9,127,-289});
    rules[324] = new Rule(-232, new int[]{-189,127,8,9});
    rules[325] = new Rule(-232, new int[]{-310,127,8,9});
    rules[326] = new Rule(-232, new int[]{8,9,127,8,9});
    rules[327] = new Rule(-232, new int[]{8,-85,9,127,8,9});
    rules[328] = new Rule(-34, new int[]{-25,-300,-192,-328,-30});
    rules[329] = new Rule(-35, new int[]{48,-192,-328,-29,92});
    rules[330] = new Rule(-24, new int[]{68});
    rules[331] = new Rule(-24, new int[]{69});
    rules[332] = new Rule(-24, new int[]{148});
    rules[333] = new Rule(-24, new int[]{27});
    rules[334] = new Rule(-24, new int[]{28});
    rules[335] = new Rule(-25, new int[]{});
    rules[336] = new Rule(-25, new int[]{-26});
    rules[337] = new Rule(-26, new int[]{-24});
    rules[338] = new Rule(-26, new int[]{-26,-24});
    rules[339] = new Rule(-300, new int[]{26});
    rules[340] = new Rule(-300, new int[]{43});
    rules[341] = new Rule(-300, new int[]{64});
    rules[342] = new Rule(-300, new int[]{64,26});
    rules[343] = new Rule(-300, new int[]{64,48});
    rules[344] = new Rule(-300, new int[]{64,43});
    rules[345] = new Rule(-30, new int[]{});
    rules[346] = new Rule(-30, new int[]{-29,92});
    rules[347] = new Rule(-192, new int[]{});
    rules[348] = new Rule(-192, new int[]{8,-191,9});
    rules[349] = new Rule(-191, new int[]{-190});
    rules[350] = new Rule(-191, new int[]{-191,100,-190});
    rules[351] = new Rule(-190, new int[]{-189});
    rules[352] = new Rule(-190, new int[]{-310});
    rules[353] = new Rule(-160, new int[]{123,-163,121});
    rules[354] = new Rule(-328, new int[]{});
    rules[355] = new Rule(-328, new int[]{-327});
    rules[356] = new Rule(-327, new int[]{-326});
    rules[357] = new Rule(-327, new int[]{-327,-326});
    rules[358] = new Rule(-326, new int[]{23,-163,5,-297,10});
    rules[359] = new Rule(-297, new int[]{-295});
    rules[360] = new Rule(-297, new int[]{-297,100,-295});
    rules[361] = new Rule(-295, new int[]{-286});
    rules[362] = new Rule(-295, new int[]{26});
    rules[363] = new Rule(-295, new int[]{48});
    rules[364] = new Rule(-295, new int[]{30});
    rules[365] = new Rule(-29, new int[]{-36});
    rules[366] = new Rule(-29, new int[]{-29,-12,-36});
    rules[367] = new Rule(-12, new int[]{84});
    rules[368] = new Rule(-12, new int[]{83});
    rules[369] = new Rule(-12, new int[]{82});
    rules[370] = new Rule(-12, new int[]{81});
    rules[371] = new Rule(-36, new int[]{});
    rules[372] = new Rule(-36, new int[]{-38,-199});
    rules[373] = new Rule(-36, new int[]{-37});
    rules[374] = new Rule(-36, new int[]{-38,10,-37});
    rules[375] = new Rule(-163, new int[]{-152});
    rules[376] = new Rule(-163, new int[]{-163,100,-152});
    rules[377] = new Rule(-199, new int[]{});
    rules[378] = new Rule(-199, new int[]{10});
    rules[379] = new Rule(-38, new int[]{-49});
    rules[380] = new Rule(-38, new int[]{-38,10,-49});
    rules[381] = new Rule(-49, new int[]{-11,-55});
    rules[382] = new Rule(-37, new int[]{-58});
    rules[383] = new Rule(-37, new int[]{-37,-58});
    rules[384] = new Rule(-58, new int[]{-57});
    rules[385] = new Rule(-58, new int[]{-59});
    rules[386] = new Rule(-55, new int[]{29,-32});
    rules[387] = new Rule(-55, new int[]{-323});
    rules[388] = new Rule(-55, new int[]{-8,-323});
    rules[389] = new Rule(-8, new int[]{28});
    rules[390] = new Rule(-8, new int[]{26});
    rules[391] = new Rule(-323, new int[]{-322});
    rules[392] = new Rule(-323, new int[]{62,-163,5,-286});
    rules[393] = new Rule(-57, new int[]{-11,-231});
    rules[394] = new Rule(-57, new int[]{-11,-228});
    rules[395] = new Rule(-228, new int[]{-224});
    rules[396] = new Rule(-228, new int[]{-227});
    rules[397] = new Rule(-231, new int[]{-8,-239});
    rules[398] = new Rule(-231, new int[]{-239});
    rules[399] = new Rule(-231, new int[]{19,-8,-239});
    rules[400] = new Rule(-231, new int[]{19,-239});
    rules[401] = new Rule(-231, new int[]{-236});
    rules[402] = new Rule(-239, new int[]{-237});
    rules[403] = new Rule(-237, new int[]{-234});
    rules[404] = new Rule(-237, new int[]{-238});
    rules[405] = new Rule(-236, new int[]{30,-181,-133,-216});
    rules[406] = new Rule(-236, new int[]{-8,30,-181,-133,-216});
    rules[407] = new Rule(-236, new int[]{31,-181,-133,-216});
    rules[408] = new Rule(-181, new int[]{-180});
    rules[409] = new Rule(-181, new int[]{});
    rules[410] = new Rule(-59, new int[]{-11,-268});
    rules[411] = new Rule(-268, new int[]{46,-179,-242,-211,10,-214});
    rules[412] = new Rule(-268, new int[]{46,-179,-242,-211,10,-219,10,-214});
    rules[413] = new Rule(-268, new int[]{-8,46,-179,-242,-211,10,-214});
    rules[414] = new Rule(-268, new int[]{-8,46,-179,-242,-211,10,-219,10,-214});
    rules[415] = new Rule(-268, new int[]{27,46,-179,-242,-220,10});
    rules[416] = new Rule(-268, new int[]{-8,27,46,-179,-242,-220,10});
    rules[417] = new Rule(-220, new int[]{110,-92});
    rules[418] = new Rule(-220, new int[]{});
    rules[419] = new Rule(-214, new int[]{});
    rules[420] = new Rule(-214, new int[]{63,10});
    rules[421] = new Rule(-242, new int[]{-247,5,-285});
    rules[422] = new Rule(-247, new int[]{});
    rules[423] = new Rule(-247, new int[]{11,-246,12});
    rules[424] = new Rule(-246, new int[]{-245});
    rules[425] = new Rule(-246, new int[]{-246,10,-245});
    rules[426] = new Rule(-245, new int[]{-163,5,-285});
    rules[427] = new Rule(-119, new int[]{-93});
    rules[428] = new Rule(-119, new int[]{});
    rules[429] = new Rule(-211, new int[]{});
    rules[430] = new Rule(-211, new int[]{85,-119,-212});
    rules[431] = new Rule(-211, new int[]{86,-270,-213});
    rules[432] = new Rule(-212, new int[]{});
    rules[433] = new Rule(-212, new int[]{86,-270});
    rules[434] = new Rule(-213, new int[]{});
    rules[435] = new Rule(-213, new int[]{85,-119});
    rules[436] = new Rule(-321, new int[]{-322,10});
    rules[437] = new Rule(-349, new int[]{110});
    rules[438] = new Rule(-349, new int[]{120});
    rules[439] = new Rule(-322, new int[]{-163,5,-286});
    rules[440] = new Rule(-322, new int[]{-163,110,-93});
    rules[441] = new Rule(-322, new int[]{-163,5,-286,-349,-91});
    rules[442] = new Rule(-91, new int[]{-90});
    rules[443] = new Rule(-91, new int[]{-86,6,-18});
    rules[444] = new Rule(-91, new int[]{-334});
    rules[445] = new Rule(-91, new int[]{-152,127,-339});
    rules[446] = new Rule(-91, new int[]{8,9,-335,127,-339});
    rules[447] = new Rule(-91, new int[]{8,-70,9,127,-339});
    rules[448] = new Rule(-91, new int[]{-252});
    rules[449] = new Rule(-90, new int[]{-89});
    rules[450] = new Rule(-226, new int[]{-236,-186});
    rules[451] = new Rule(-226, new int[]{30,-181,-133,110,-270,10});
    rules[452] = new Rule(-226, new int[]{-8,30,-181,-133,110,-270,10});
    rules[453] = new Rule(-227, new int[]{-236,-185});
    rules[454] = new Rule(-227, new int[]{30,-181,-133,110,-270,10});
    rules[455] = new Rule(-227, new int[]{-8,30,-181,-133,110,-270,10});
    rules[456] = new Rule(-223, new int[]{-230});
    rules[457] = new Rule(-223, new int[]{-8,-230});
    rules[458] = new Rule(-223, new int[]{19,-230});
    rules[459] = new Rule(-223, new int[]{19,-8,-230});
    rules[460] = new Rule(-223, new int[]{-8,19,-230});
    rules[461] = new Rule(-224, new int[]{-225});
    rules[462] = new Rule(-224, new int[]{19,-225});
    rules[463] = new Rule(-224, new int[]{-8,-225});
    rules[464] = new Rule(-224, new int[]{19,-8,-225});
    rules[465] = new Rule(-224, new int[]{-8,19,-225});
    rules[466] = new Rule(-230, new int[]{-237,-187});
    rules[467] = new Rule(-230, new int[]{-237,149,10});
    rules[468] = new Rule(-230, new int[]{37,-179,-133,5,-285,-217,110,-108,10});
    rules[469] = new Rule(-230, new int[]{37,-179,-133,-217,110,-108,10});
    rules[470] = new Rule(-230, new int[]{44,-180,-133,-217,110,-270,10});
    rules[471] = new Rule(-225, new int[]{-237,-185});
    rules[472] = new Rule(-225, new int[]{37,-179,-133,5,-285,-217,110,-108,10});
    rules[473] = new Rule(-225, new int[]{37,-179,-133,-217,110,-108,10});
    rules[474] = new Rule(-225, new int[]{44,-180,-133,-217,110,-270,10});
    rules[475] = new Rule(-187, new int[]{-186});
    rules[476] = new Rule(-187, new int[]{-65});
    rules[477] = new Rule(-180, new int[]{-179});
    rules[478] = new Rule(-179, new int[]{-147});
    rules[479] = new Rule(-179, new int[]{-345,7,-147});
    rules[480] = new Rule(-154, new int[]{-142});
    rules[481] = new Rule(-345, new int[]{-154});
    rules[482] = new Rule(-345, new int[]{-345,7,-154});
    rules[483] = new Rule(-147, new int[]{-142});
    rules[484] = new Rule(-147, new int[]{-200});
    rules[485] = new Rule(-147, new int[]{-200,-160});
    rules[486] = new Rule(-142, new int[]{-139});
    rules[487] = new Rule(-142, new int[]{-139,-160});
    rules[488] = new Rule(-139, new int[]{-152});
    rules[489] = new Rule(-234, new int[]{44,-180,-133,-216,-328});
    rules[490] = new Rule(-238, new int[]{37,-179,-133,-216,-328});
    rules[491] = new Rule(-238, new int[]{37,-179,-133,5,-285,-216,-328});
    rules[492] = new Rule(-65, new int[]{107,-113,80,-113,10});
    rules[493] = new Rule(-65, new int[]{107,-113,10});
    rules[494] = new Rule(-65, new int[]{107,10});
    rules[495] = new Rule(-113, new int[]{-152});
    rules[496] = new Rule(-113, new int[]{-174});
    rules[497] = new Rule(-186, new int[]{-45,-265,10});
    rules[498] = new Rule(-185, new int[]{-47,-265,10});
    rules[499] = new Rule(-185, new int[]{-65});
    rules[500] = new Rule(-133, new int[]{});
    rules[501] = new Rule(-133, new int[]{8,9});
    rules[502] = new Rule(-133, new int[]{8,-134,9});
    rules[503] = new Rule(-134, new int[]{-60});
    rules[504] = new Rule(-134, new int[]{-134,10,-60});
    rules[505] = new Rule(-60, new int[]{-11,-305});
    rules[506] = new Rule(-305, new int[]{-164,5,-285});
    rules[507] = new Rule(-305, new int[]{53,-164,5,-285});
    rules[508] = new Rule(-305, new int[]{29,-164,5,-285});
    rules[509] = new Rule(-305, new int[]{108,-164,5,-285});
    rules[510] = new Rule(-305, new int[]{-164,5,-285,110,-92});
    rules[511] = new Rule(-305, new int[]{53,-164,5,-285,110,-92});
    rules[512] = new Rule(-305, new int[]{29,-164,5,-285,110,-92});
    rules[513] = new Rule(-164, new int[]{-140});
    rules[514] = new Rule(-164, new int[]{-164,100,-140});
    rules[515] = new Rule(-140, new int[]{-152});
    rules[516] = new Rule(-285, new int[]{-286});
    rules[517] = new Rule(-287, new int[]{-282});
    rules[518] = new Rule(-287, new int[]{-266});
    rules[519] = new Rule(-287, new int[]{-259});
    rules[520] = new Rule(-287, new int[]{-291});
    rules[521] = new Rule(-287, new int[]{-310});
    rules[522] = new Rule(-271, new int[]{-270});
    rules[523] = new Rule(-271, new int[]{-148,5,-271});
    rules[524] = new Rule(-270, new int[]{});
    rules[525] = new Rule(-270, new int[]{-9});
    rules[526] = new Rule(-270, new int[]{-221});
    rules[527] = new Rule(-270, new int[]{-138});
    rules[528] = new Rule(-270, new int[]{-265});
    rules[529] = new Rule(-270, new int[]{-158});
    rules[530] = new Rule(-270, new int[]{-39});
    rules[531] = new Rule(-270, new int[]{-257});
    rules[532] = new Rule(-270, new int[]{-329});
    rules[533] = new Rule(-270, new int[]{-129});
    rules[534] = new Rule(-270, new int[]{-330});
    rules[535] = new Rule(-270, new int[]{-165});
    rules[536] = new Rule(-270, new int[]{-311});
    rules[537] = new Rule(-270, new int[]{-258});
    rules[538] = new Rule(-270, new int[]{-128});
    rules[539] = new Rule(-270, new int[]{-325});
    rules[540] = new Rule(-270, new int[]{-63});
    rules[541] = new Rule(-270, new int[]{-178});
    rules[542] = new Rule(-270, new int[]{-131});
    rules[543] = new Rule(-270, new int[]{-132});
    rules[544] = new Rule(-270, new int[]{-130});
    rules[545] = new Rule(-270, new int[]{-359});
    rules[546] = new Rule(-130, new int[]{72,-106,99,-270});
    rules[547] = new Rule(-131, new int[]{74,-108});
    rules[548] = new Rule(-132, new int[]{74,73,-108});
    rules[549] = new Rule(-325, new int[]{53,-322});
    rules[550] = new Rule(-325, new int[]{8,53,-152,100,-348,9,110,-92});
    rules[551] = new Rule(-325, new int[]{53,8,-152,100,-163,9,110,-92});
    rules[552] = new Rule(-9, new int[]{-118,-203,-93});
    rules[553] = new Rule(-9, new int[]{8,-116,100,-347,9,-203,-92});
    rules[554] = new Rule(-347, new int[]{-116});
    rules[555] = new Rule(-347, new int[]{-347,100,-116});
    rules[556] = new Rule(-348, new int[]{53,-152});
    rules[557] = new Rule(-348, new int[]{-348,100,53,-152});
    rules[558] = new Rule(-221, new int[]{-118});
    rules[559] = new Rule(-138, new int[]{57,-148});
    rules[560] = new Rule(-265, new int[]{91,-262,92});
    rules[561] = new Rule(-262, new int[]{-271});
    rules[562] = new Rule(-262, new int[]{-262,10,-271});
    rules[563] = new Rule(-158, new int[]{40,-106,51,-270});
    rules[564] = new Rule(-158, new int[]{40,-106,51,-270,32,-270});
    rules[565] = new Rule(-359, new int[]{38,-106,55,-361,-263,92});
    rules[566] = new Rule(-359, new int[]{38,-106,55,-361,10,-263,92});
    rules[567] = new Rule(-361, new int[]{-360});
    rules[568] = new Rule(-361, new int[]{-361,10,-360});
    rules[569] = new Rule(-360, new int[]{-353,39,-106,5,-270});
    rules[570] = new Rule(-360, new int[]{-352,5,-270});
    rules[571] = new Rule(-360, new int[]{-354,5,-270});
    rules[572] = new Rule(-360, new int[]{-355,39,-106,5,-270});
    rules[573] = new Rule(-360, new int[]{-355,5,-270});
    rules[574] = new Rule(-39, new int[]{25,-106,58,-40,-263,92});
    rules[575] = new Rule(-39, new int[]{25,-106,58,-40,10,-263,92});
    rules[576] = new Rule(-39, new int[]{25,-106,58,-263,92});
    rules[577] = new Rule(-40, new int[]{-272});
    rules[578] = new Rule(-40, new int[]{-40,10,-272});
    rules[579] = new Rule(-272, new int[]{-79,5,-270});
    rules[580] = new Rule(-79, new int[]{-115});
    rules[581] = new Rule(-79, new int[]{-79,100,-115});
    rules[582] = new Rule(-115, new int[]{-99});
    rules[583] = new Rule(-263, new int[]{});
    rules[584] = new Rule(-263, new int[]{32,-262});
    rules[585] = new Rule(-257, new int[]{97,-262,98,-92});
    rules[586] = new Rule(-329, new int[]{54,-106,-301,-270});
    rules[587] = new Rule(-301, new int[]{99});
    rules[588] = new Rule(-301, new int[]{});
    rules[589] = new Rule(-178, new int[]{60,-106,99,-270});
    rules[590] = new Rule(-371, new int[]{87,143});
    rules[591] = new Rule(-371, new int[]{});
    rules[592] = new Rule(-284, new int[]{5,-286});
    rules[593] = new Rule(-284, new int[]{});
    rules[594] = new Rule(-23, new int[]{53});
    rules[595] = new Rule(-23, new int[]{});
    rules[596] = new Rule(-124, new int[]{70});
    rules[597] = new Rule(-124, new int[]{71});
    rules[598] = new Rule(-128, new int[]{36,-152,-284,137,-106,-371,99,-270});
    rules[599] = new Rule(-128, new int[]{36,53,-152,-284,137,-106,-371,99,-270});
    rules[600] = new Rule(-128, new int[]{36,53,8,-163,9,137,-106,-371,99,-270});
    rules[601] = new Rule(-129, new int[]{35,-23,-152,-284,110,-106,-124,-106,-301,-270});
    rules[602] = new Rule(-129, new int[]{35,-23,-152,-284,110,-106,-124,-106,160,-106,99,-270});
    rules[603] = new Rule(-330, new int[]{55,-76,99,-270});
    rules[604] = new Rule(-165, new int[]{42});
    rules[605] = new Rule(-311, new int[]{102,-262,-299});
    rules[606] = new Rule(-299, new int[]{101,-262,92});
    rules[607] = new Rule(-299, new int[]{33,-64,92});
    rules[608] = new Rule(-64, new int[]{-67,-264});
    rules[609] = new Rule(-64, new int[]{-67,10,-264});
    rules[610] = new Rule(-64, new int[]{-262});
    rules[611] = new Rule(-67, new int[]{-66});
    rules[612] = new Rule(-67, new int[]{-67,10,-66});
    rules[613] = new Rule(-264, new int[]{});
    rules[614] = new Rule(-264, new int[]{32,-262});
    rules[615] = new Rule(-66, new int[]{79,-68,99,-270});
    rules[616] = new Rule(-68, new int[]{-188});
    rules[617] = new Rule(-68, new int[]{-145,5,-188});
    rules[618] = new Rule(-188, new int[]{-189});
    rules[619] = new Rule(-145, new int[]{-152});
    rules[620] = new Rule(-258, new int[]{47});
    rules[621] = new Rule(-258, new int[]{47,-92});
    rules[622] = new Rule(-76, new int[]{-93});
    rules[623] = new Rule(-76, new int[]{-76,100,-93});
    rules[624] = new Rule(-77, new int[]{-94});
    rules[625] = new Rule(-77, new int[]{-77,100,-94});
    rules[626] = new Rule(-63, new int[]{-183});
    rules[627] = new Rule(-183, new int[]{-182});
    rules[628] = new Rule(-93, new int[]{-92});
    rules[629] = new Rule(-93, new int[]{-333});
    rules[630] = new Rule(-93, new int[]{42});
    rules[631] = new Rule(-94, new int[]{-92});
    rules[632] = new Rule(-94, new int[]{-152,110,-106});
    rules[633] = new Rule(-94, new int[]{-333});
    rules[634] = new Rule(-94, new int[]{42});
    rules[635] = new Rule(-92, new int[]{-106});
    rules[636] = new Rule(-92, new int[]{-125});
    rules[637] = new Rule(-92, new int[]{-92,70,-106});
    rules[638] = new Rule(-106, new int[]{-104});
    rules[639] = new Rule(-106, new int[]{-250});
    rules[640] = new Rule(-106, new int[]{-252});
    rules[641] = new Rule(-122, new int[]{-104});
    rules[642] = new Rule(-122, new int[]{-250});
    rules[643] = new Rule(-123, new int[]{-104});
    rules[644] = new Rule(-123, new int[]{-252});
    rules[645] = new Rule(-108, new int[]{-106});
    rules[646] = new Rule(-108, new int[]{-333});
    rules[647] = new Rule(-109, new int[]{-104});
    rules[648] = new Rule(-109, new int[]{-250});
    rules[649] = new Rule(-109, new int[]{-333});
    rules[650] = new Rule(-104, new int[]{-103});
    rules[651] = new Rule(-104, new int[]{-104,16,-103});
    rules[652] = new Rule(-267, new int[]{21,8,-293,9});
    rules[653] = new Rule(-304, new int[]{22,8,-293,9});
    rules[654] = new Rule(-304, new int[]{22,8,-292,9});
    rules[655] = new Rule(-250, new int[]{-122,13,-122,5,-122});
    rules[656] = new Rule(-252, new int[]{40,-123,51,-123,32,-123});
    rules[657] = new Rule(-292, new int[]{-189,-309});
    rules[658] = new Rule(-292, new int[]{-189,4,-309});
    rules[659] = new Rule(-293, new int[]{-189});
    rules[660] = new Rule(-293, new int[]{-189,-308});
    rules[661] = new Rule(-293, new int[]{-189,4,-308});
    rules[662] = new Rule(-294, new int[]{-293});
    rules[663] = new Rule(-294, new int[]{-283});
    rules[664] = new Rule(-10, new int[]{8,-70,9});
    rules[665] = new Rule(-10, new int[]{});
    rules[666] = new Rule(-182, new int[]{78,-293,-75});
    rules[667] = new Rule(-182, new int[]{78,-293,11,-71,12,-10});
    rules[668] = new Rule(-182, new int[]{78,26,8,-344,9});
    rules[669] = new Rule(-343, new int[]{-152,110,-106});
    rules[670] = new Rule(-343, new int[]{-106});
    rules[671] = new Rule(-344, new int[]{-343});
    rules[672] = new Rule(-344, new int[]{-344,100,-343});
    rules[673] = new Rule(-74, new int[]{});
    rules[674] = new Rule(-74, new int[]{8,-71,9});
    rules[675] = new Rule(-75, new int[]{});
    rules[676] = new Rule(-75, new int[]{8,-72,9});
    rules[677] = new Rule(-103, new int[]{-110});
    rules[678] = new Rule(-103, new int[]{-103,-205,-110});
    rules[679] = new Rule(-103, new int[]{-103,-205,-252});
    rules[680] = new Rule(-103, new int[]{-276,8,-364,9});
    rules[681] = new Rule(-351, new int[]{-293,8,-364,9});
    rules[682] = new Rule(-353, new int[]{-293,8,-365,9});
    rules[683] = new Rule(-352, new int[]{-293,8,-365,9});
    rules[684] = new Rule(-352, new int[]{-368});
    rules[685] = new Rule(-368, new int[]{-350});
    rules[686] = new Rule(-368, new int[]{-368,100,-350});
    rules[687] = new Rule(-350, new int[]{-20});
    rules[688] = new Rule(-350, new int[]{-293});
    rules[689] = new Rule(-350, new int[]{56});
    rules[690] = new Rule(-350, new int[]{-267});
    rules[691] = new Rule(-350, new int[]{-304});
    rules[692] = new Rule(-354, new int[]{11,-366,12});
    rules[693] = new Rule(-366, new int[]{-356});
    rules[694] = new Rule(-366, new int[]{-366,100,-356});
    rules[695] = new Rule(-356, new int[]{-20});
    rules[696] = new Rule(-356, new int[]{-358});
    rules[697] = new Rule(-356, new int[]{14});
    rules[698] = new Rule(-356, new int[]{-353});
    rules[699] = new Rule(-356, new int[]{-354});
    rules[700] = new Rule(-356, new int[]{-355});
    rules[701] = new Rule(-356, new int[]{6});
    rules[702] = new Rule(-358, new int[]{53,-152});
    rules[703] = new Rule(-355, new int[]{8,-367,9});
    rules[704] = new Rule(-357, new int[]{14});
    rules[705] = new Rule(-357, new int[]{-20});
    rules[706] = new Rule(-357, new int[]{-208,-20});
    rules[707] = new Rule(-357, new int[]{53,-152});
    rules[708] = new Rule(-357, new int[]{-353});
    rules[709] = new Rule(-357, new int[]{-354});
    rules[710] = new Rule(-357, new int[]{-355});
    rules[711] = new Rule(-367, new int[]{-357});
    rules[712] = new Rule(-367, new int[]{-367,100,-357});
    rules[713] = new Rule(-365, new int[]{-363});
    rules[714] = new Rule(-365, new int[]{-365,10,-363});
    rules[715] = new Rule(-365, new int[]{-365,100,-363});
    rules[716] = new Rule(-364, new int[]{-362});
    rules[717] = new Rule(-364, new int[]{-364,10,-362});
    rules[718] = new Rule(-364, new int[]{-364,100,-362});
    rules[719] = new Rule(-362, new int[]{14});
    rules[720] = new Rule(-362, new int[]{-20});
    rules[721] = new Rule(-362, new int[]{53,-152,5,-286});
    rules[722] = new Rule(-362, new int[]{53,-152});
    rules[723] = new Rule(-362, new int[]{-351});
    rules[724] = new Rule(-362, new int[]{-354});
    rules[725] = new Rule(-362, new int[]{-355});
    rules[726] = new Rule(-363, new int[]{14});
    rules[727] = new Rule(-363, new int[]{-20});
    rules[728] = new Rule(-363, new int[]{-208,-20});
    rules[729] = new Rule(-363, new int[]{-152,5,-286});
    rules[730] = new Rule(-363, new int[]{-152});
    rules[731] = new Rule(-363, new int[]{53,-152,5,-286});
    rules[732] = new Rule(-363, new int[]{53,-152});
    rules[733] = new Rule(-363, new int[]{-353});
    rules[734] = new Rule(-363, new int[]{-354});
    rules[735] = new Rule(-363, new int[]{-355});
    rules[736] = new Rule(-120, new int[]{-110});
    rules[737] = new Rule(-120, new int[]{});
    rules[738] = new Rule(-127, new int[]{-95});
    rules[739] = new Rule(-127, new int[]{});
    rules[740] = new Rule(-125, new int[]{-110,5,-120});
    rules[741] = new Rule(-125, new int[]{5,-120});
    rules[742] = new Rule(-125, new int[]{-110,5,-120,5,-110});
    rules[743] = new Rule(-125, new int[]{5,-120,5,-110});
    rules[744] = new Rule(-126, new int[]{-95,5,-127});
    rules[745] = new Rule(-126, new int[]{5,-127});
    rules[746] = new Rule(-126, new int[]{-95,5,-127,5,-95});
    rules[747] = new Rule(-126, new int[]{5,-127,5,-95});
    rules[748] = new Rule(-205, new int[]{120});
    rules[749] = new Rule(-205, new int[]{125});
    rules[750] = new Rule(-205, new int[]{123});
    rules[751] = new Rule(-205, new int[]{121});
    rules[752] = new Rule(-205, new int[]{124});
    rules[753] = new Rule(-205, new int[]{122});
    rules[754] = new Rule(-205, new int[]{137});
    rules[755] = new Rule(-205, new int[]{135,137});
    rules[756] = new Rule(-110, new int[]{-88});
    rules[757] = new Rule(-110, new int[]{-110,6,-88});
    rules[758] = new Rule(-88, new int[]{-87});
    rules[759] = new Rule(-88, new int[]{-88,-206,-87});
    rules[760] = new Rule(-88, new int[]{-88,-206,-252});
    rules[761] = new Rule(-206, new int[]{116});
    rules[762] = new Rule(-206, new int[]{115});
    rules[763] = new Rule(-206, new int[]{128});
    rules[764] = new Rule(-206, new int[]{129});
    rules[765] = new Rule(-206, new int[]{126});
    rules[766] = new Rule(-210, new int[]{136});
    rules[767] = new Rule(-210, new int[]{138});
    rules[768] = new Rule(-274, new int[]{-276});
    rules[769] = new Rule(-274, new int[]{-277});
    rules[770] = new Rule(-277, new int[]{-87,136,-293});
    rules[771] = new Rule(-277, new int[]{-87,136,-288});
    rules[772] = new Rule(-276, new int[]{-87,138,-293});
    rules[773] = new Rule(-276, new int[]{-87,138,-288});
    rules[774] = new Rule(-278, new int[]{-102,119,-101});
    rules[775] = new Rule(-278, new int[]{-102,119,-278});
    rules[776] = new Rule(-278, new int[]{-208,-278});
    rules[777] = new Rule(-87, new int[]{-101});
    rules[778] = new Rule(-87, new int[]{-182});
    rules[779] = new Rule(-87, new int[]{-278});
    rules[780] = new Rule(-87, new int[]{-87,-207,-101});
    rules[781] = new Rule(-87, new int[]{-87,-207,-278});
    rules[782] = new Rule(-87, new int[]{-87,-207,-252});
    rules[783] = new Rule(-87, new int[]{-274});
    rules[784] = new Rule(-207, new int[]{118});
    rules[785] = new Rule(-207, new int[]{117});
    rules[786] = new Rule(-207, new int[]{131});
    rules[787] = new Rule(-207, new int[]{132});
    rules[788] = new Rule(-207, new int[]{133});
    rules[789] = new Rule(-207, new int[]{134});
    rules[790] = new Rule(-207, new int[]{130});
    rules[791] = new Rule(-61, new int[]{63,8,-294,9});
    rules[792] = new Rule(-62, new int[]{8,-107,100,-84,-335,-342,9});
    rules[793] = new Rule(-102, new int[]{-20});
    rules[794] = new Rule(-102, new int[]{-118});
    rules[795] = new Rule(-101, new int[]{56});
    rules[796] = new Rule(-101, new int[]{-20});
    rules[797] = new Rule(-101, new int[]{-61});
    rules[798] = new Rule(-101, new int[]{135,-101});
    rules[799] = new Rule(-101, new int[]{-208,-101});
    rules[800] = new Rule(-101, new int[]{142,-101});
    rules[801] = new Rule(-101, new int[]{-118});
    rules[802] = new Rule(-101, new int[]{-62});
    rules[803] = new Rule(-20, new int[]{-174});
    rules[804] = new Rule(-20, new int[]{-21});
    rules[805] = new Rule(-121, new int[]{-116,15,-116});
    rules[806] = new Rule(-121, new int[]{-116,15,-121});
    rules[807] = new Rule(-118, new int[]{-137,-116});
    rules[808] = new Rule(-118, new int[]{-116});
    rules[809] = new Rule(-118, new int[]{-121});
    rules[810] = new Rule(-118, new int[]{8,53,-152,110,-104,9});
    rules[811] = new Rule(-137, new int[]{141});
    rules[812] = new Rule(-137, new int[]{-137,141});
    rules[813] = new Rule(-14, new int[]{-189,-74});
    rules[814] = new Rule(-14, new int[]{-310,-74});
    rules[815] = new Rule(-332, new int[]{-152});
    rules[816] = new Rule(-332, new int[]{-332,7,-143});
    rules[817] = new Rule(-331, new int[]{-332});
    rules[818] = new Rule(-331, new int[]{-332,-308});
    rules[819] = new Rule(-369, new int[]{53,-152,110,-92,10});
    rules[820] = new Rule(-370, new int[]{-369});
    rules[821] = new Rule(-370, new int[]{-370,-369});
    rules[822] = new Rule(-117, new int[]{-116,8,-72,9});
    rules[823] = new Rule(-116, new int[]{-152});
    rules[824] = new Rule(-116, new int[]{-200});
    rules[825] = new Rule(-116, new int[]{42,-152});
    rules[826] = new Rule(-116, new int[]{8,-92,9});
    rules[827] = new Rule(-116, new int[]{8,-370,-92,9});
    rules[828] = new Rule(-116, new int[]{-267});
    rules[829] = new Rule(-116, new int[]{-304});
    rules[830] = new Rule(-116, new int[]{-20,7,-143});
    rules[831] = new Rule(-116, new int[]{-116,11,-76,12});
    rules[832] = new Rule(-116, new int[]{-20,11,-76,12});
    rules[833] = new Rule(-116, new int[]{-116,17,-125,12});
    rules[834] = new Rule(-116, new int[]{-20,17,-125,12});
    rules[835] = new Rule(-116, new int[]{76,-73,76});
    rules[836] = new Rule(-116, new int[]{-249});
    rules[837] = new Rule(-116, new int[]{-117});
    rules[838] = new Rule(-116, new int[]{-116,7,-153});
    rules[839] = new Rule(-116, new int[]{-62,7,-153});
    rules[840] = new Rule(-116, new int[]{-116,142});
    rules[841] = new Rule(-116, new int[]{-116,4,-308});
    rules[842] = new Rule(-71, new int[]{-76});
    rules[843] = new Rule(-71, new int[]{});
    rules[844] = new Rule(-72, new int[]{-77});
    rules[845] = new Rule(-72, new int[]{});
    rules[846] = new Rule(-73, new int[]{-82});
    rules[847] = new Rule(-73, new int[]{});
    rules[848] = new Rule(-82, new int[]{-97});
    rules[849] = new Rule(-82, new int[]{-82,100,-97});
    rules[850] = new Rule(-97, new int[]{-92});
    rules[851] = new Rule(-97, new int[]{-92,6,-92});
    rules[852] = new Rule(-175, new int[]{144});
    rules[853] = new Rule(-175, new int[]{147});
    rules[854] = new Rule(-174, new int[]{-176});
    rules[855] = new Rule(-174, new int[]{145});
    rules[856] = new Rule(-174, new int[]{146});
    rules[857] = new Rule(-176, new int[]{-175});
    rules[858] = new Rule(-176, new int[]{-176,-175});
    rules[859] = new Rule(-200, new int[]{45,-209});
    rules[860] = new Rule(-216, new int[]{10});
    rules[861] = new Rule(-216, new int[]{10,-215,10});
    rules[862] = new Rule(-217, new int[]{});
    rules[863] = new Rule(-217, new int[]{10,-215});
    rules[864] = new Rule(-215, new int[]{-218});
    rules[865] = new Rule(-215, new int[]{-215,10,-218});
    rules[866] = new Rule(-152, new int[]{143});
    rules[867] = new Rule(-152, new int[]{161});
    rules[868] = new Rule(-152, new int[]{-156});
    rules[869] = new Rule(-152, new int[]{-157});
    rules[870] = new Rule(-152, new int[]{160});
    rules[871] = new Rule(-152, new int[]{87});
    rules[872] = new Rule(-143, new int[]{-152});
    rules[873] = new Rule(-143, new int[]{-302});
    rules[874] = new Rule(-143, new int[]{-303});
    rules[875] = new Rule(-153, new int[]{-152});
    rules[876] = new Rule(-153, new int[]{-302});
    rules[877] = new Rule(-153, new int[]{-200});
    rules[878] = new Rule(-218, new int[]{148});
    rules[879] = new Rule(-218, new int[]{150});
    rules[880] = new Rule(-218, new int[]{151});
    rules[881] = new Rule(-218, new int[]{152});
    rules[882] = new Rule(-218, new int[]{154});
    rules[883] = new Rule(-218, new int[]{153});
    rules[884] = new Rule(-219, new int[]{153});
    rules[885] = new Rule(-219, new int[]{152});
    rules[886] = new Rule(-219, new int[]{148});
    rules[887] = new Rule(-219, new int[]{151});
    rules[888] = new Rule(-156, new int[]{85});
    rules[889] = new Rule(-156, new int[]{86});
    rules[890] = new Rule(-157, new int[]{80});
    rules[891] = new Rule(-157, new int[]{78});
    rules[892] = new Rule(-155, new int[]{84});
    rules[893] = new Rule(-155, new int[]{83});
    rules[894] = new Rule(-155, new int[]{82});
    rules[895] = new Rule(-155, new int[]{81});
    rules[896] = new Rule(-302, new int[]{-155});
    rules[897] = new Rule(-302, new int[]{68});
    rules[898] = new Rule(-302, new int[]{64});
    rules[899] = new Rule(-302, new int[]{128});
    rules[900] = new Rule(-302, new int[]{22});
    rules[901] = new Rule(-302, new int[]{21});
    rules[902] = new Rule(-302, new int[]{63});
    rules[903] = new Rule(-302, new int[]{23});
    rules[904] = new Rule(-302, new int[]{129});
    rules[905] = new Rule(-302, new int[]{130});
    rules[906] = new Rule(-302, new int[]{131});
    rules[907] = new Rule(-302, new int[]{132});
    rules[908] = new Rule(-302, new int[]{133});
    rules[909] = new Rule(-302, new int[]{134});
    rules[910] = new Rule(-302, new int[]{135});
    rules[911] = new Rule(-302, new int[]{136});
    rules[912] = new Rule(-302, new int[]{137});
    rules[913] = new Rule(-302, new int[]{138});
    rules[914] = new Rule(-302, new int[]{24});
    rules[915] = new Rule(-302, new int[]{73});
    rules[916] = new Rule(-302, new int[]{91});
    rules[917] = new Rule(-302, new int[]{25});
    rules[918] = new Rule(-302, new int[]{26});
    rules[919] = new Rule(-302, new int[]{29});
    rules[920] = new Rule(-302, new int[]{30});
    rules[921] = new Rule(-302, new int[]{31});
    rules[922] = new Rule(-302, new int[]{71});
    rules[923] = new Rule(-302, new int[]{99});
    rules[924] = new Rule(-302, new int[]{32});
    rules[925] = new Rule(-302, new int[]{92});
    rules[926] = new Rule(-302, new int[]{33});
    rules[927] = new Rule(-302, new int[]{34});
    rules[928] = new Rule(-302, new int[]{27});
    rules[929] = new Rule(-302, new int[]{104});
    rules[930] = new Rule(-302, new int[]{101});
    rules[931] = new Rule(-302, new int[]{35});
    rules[932] = new Rule(-302, new int[]{36});
    rules[933] = new Rule(-302, new int[]{37});
    rules[934] = new Rule(-302, new int[]{40});
    rules[935] = new Rule(-302, new int[]{41});
    rules[936] = new Rule(-302, new int[]{42});
    rules[937] = new Rule(-302, new int[]{103});
    rules[938] = new Rule(-302, new int[]{43});
    rules[939] = new Rule(-302, new int[]{44});
    rules[940] = new Rule(-302, new int[]{46});
    rules[941] = new Rule(-302, new int[]{47});
    rules[942] = new Rule(-302, new int[]{48});
    rules[943] = new Rule(-302, new int[]{97});
    rules[944] = new Rule(-302, new int[]{49});
    rules[945] = new Rule(-302, new int[]{102});
    rules[946] = new Rule(-302, new int[]{50});
    rules[947] = new Rule(-302, new int[]{28});
    rules[948] = new Rule(-302, new int[]{51});
    rules[949] = new Rule(-302, new int[]{70});
    rules[950] = new Rule(-302, new int[]{98});
    rules[951] = new Rule(-302, new int[]{52});
    rules[952] = new Rule(-302, new int[]{53});
    rules[953] = new Rule(-302, new int[]{54});
    rules[954] = new Rule(-302, new int[]{55});
    rules[955] = new Rule(-302, new int[]{56});
    rules[956] = new Rule(-302, new int[]{57});
    rules[957] = new Rule(-302, new int[]{58});
    rules[958] = new Rule(-302, new int[]{59});
    rules[959] = new Rule(-302, new int[]{61});
    rules[960] = new Rule(-302, new int[]{105});
    rules[961] = new Rule(-302, new int[]{106});
    rules[962] = new Rule(-302, new int[]{109});
    rules[963] = new Rule(-302, new int[]{107});
    rules[964] = new Rule(-302, new int[]{108});
    rules[965] = new Rule(-302, new int[]{62});
    rules[966] = new Rule(-302, new int[]{74});
    rules[967] = new Rule(-302, new int[]{38});
    rules[968] = new Rule(-302, new int[]{39});
    rules[969] = new Rule(-302, new int[]{69});
    rules[970] = new Rule(-302, new int[]{148});
    rules[971] = new Rule(-302, new int[]{60});
    rules[972] = new Rule(-302, new int[]{139});
    rules[973] = new Rule(-302, new int[]{140});
    rules[974] = new Rule(-302, new int[]{79});
    rules[975] = new Rule(-302, new int[]{153});
    rules[976] = new Rule(-302, new int[]{152});
    rules[977] = new Rule(-302, new int[]{72});
    rules[978] = new Rule(-302, new int[]{154});
    rules[979] = new Rule(-302, new int[]{150});
    rules[980] = new Rule(-302, new int[]{151});
    rules[981] = new Rule(-302, new int[]{149});
    rules[982] = new Rule(-303, new int[]{45});
    rules[983] = new Rule(-209, new int[]{115});
    rules[984] = new Rule(-209, new int[]{116});
    rules[985] = new Rule(-209, new int[]{117});
    rules[986] = new Rule(-209, new int[]{118});
    rules[987] = new Rule(-209, new int[]{120});
    rules[988] = new Rule(-209, new int[]{121});
    rules[989] = new Rule(-209, new int[]{122});
    rules[990] = new Rule(-209, new int[]{123});
    rules[991] = new Rule(-209, new int[]{124});
    rules[992] = new Rule(-209, new int[]{125});
    rules[993] = new Rule(-209, new int[]{128});
    rules[994] = new Rule(-209, new int[]{129});
    rules[995] = new Rule(-209, new int[]{130});
    rules[996] = new Rule(-209, new int[]{131});
    rules[997] = new Rule(-209, new int[]{132});
    rules[998] = new Rule(-209, new int[]{133});
    rules[999] = new Rule(-209, new int[]{134});
    rules[1000] = new Rule(-209, new int[]{135});
    rules[1001] = new Rule(-209, new int[]{137});
    rules[1002] = new Rule(-209, new int[]{139});
    rules[1003] = new Rule(-209, new int[]{140});
    rules[1004] = new Rule(-209, new int[]{-203});
    rules[1005] = new Rule(-209, new int[]{119});
    rules[1006] = new Rule(-203, new int[]{110});
    rules[1007] = new Rule(-203, new int[]{111});
    rules[1008] = new Rule(-203, new int[]{112});
    rules[1009] = new Rule(-203, new int[]{113});
    rules[1010] = new Rule(-203, new int[]{114});
    rules[1011] = new Rule(-105, new int[]{18,-28,100,-27,9});
    rules[1012] = new Rule(-27, new int[]{-105});
    rules[1013] = new Rule(-27, new int[]{-152});
    rules[1014] = new Rule(-28, new int[]{-27});
    rules[1015] = new Rule(-28, new int[]{-28,100,-27});
    rules[1016] = new Rule(-107, new int[]{-106});
    rules[1017] = new Rule(-107, new int[]{-105});
    rules[1018] = new Rule(-84, new int[]{-107});
    rules[1019] = new Rule(-84, new int[]{-84,100,-107});
    rules[1020] = new Rule(-333, new int[]{-152,127,-339});
    rules[1021] = new Rule(-333, new int[]{8,9,-336,127,-339});
    rules[1022] = new Rule(-333, new int[]{8,-152,5,-285,9,-336,127,-339});
    rules[1023] = new Rule(-333, new int[]{8,-152,10,-337,9,-336,127,-339});
    rules[1024] = new Rule(-333, new int[]{8,-152,5,-285,10,-337,9,-336,127,-339});
    rules[1025] = new Rule(-333, new int[]{8,-107,100,-84,-335,-342,9,-346});
    rules[1026] = new Rule(-333, new int[]{-105,-346});
    rules[1027] = new Rule(-333, new int[]{-334});
    rules[1028] = new Rule(-342, new int[]{});
    rules[1029] = new Rule(-342, new int[]{10,-337});
    rules[1030] = new Rule(-346, new int[]{-336,127,-339});
    rules[1031] = new Rule(-334, new int[]{37,-336,127,-339});
    rules[1032] = new Rule(-334, new int[]{37,8,9,-336,127,-339});
    rules[1033] = new Rule(-334, new int[]{37,8,-337,9,-336,127,-339});
    rules[1034] = new Rule(-334, new int[]{44,127,-340});
    rules[1035] = new Rule(-334, new int[]{44,8,9,127,-340});
    rules[1036] = new Rule(-334, new int[]{44,8,-337,9,127,-340});
    rules[1037] = new Rule(-337, new int[]{-338});
    rules[1038] = new Rule(-337, new int[]{-337,10,-338});
    rules[1039] = new Rule(-338, new int[]{-163,-335});
    rules[1040] = new Rule(-335, new int[]{});
    rules[1041] = new Rule(-335, new int[]{5,-285});
    rules[1042] = new Rule(-336, new int[]{});
    rules[1043] = new Rule(-336, new int[]{5,-287});
    rules[1044] = new Rule(-341, new int[]{-265});
    rules[1045] = new Rule(-341, new int[]{-158});
    rules[1046] = new Rule(-341, new int[]{-329});
    rules[1047] = new Rule(-341, new int[]{-257});
    rules[1048] = new Rule(-341, new int[]{-129});
    rules[1049] = new Rule(-341, new int[]{-128});
    rules[1050] = new Rule(-341, new int[]{-130});
    rules[1051] = new Rule(-341, new int[]{-39});
    rules[1052] = new Rule(-341, new int[]{-311});
    rules[1053] = new Rule(-341, new int[]{-178});
    rules[1054] = new Rule(-341, new int[]{-258});
    rules[1055] = new Rule(-341, new int[]{-131});
    rules[1056] = new Rule(-341, new int[]{8,-9,9});
    rules[1057] = new Rule(-339, new int[]{-109});
    rules[1058] = new Rule(-339, new int[]{-341});
    rules[1059] = new Rule(-340, new int[]{-221});
    rules[1060] = new Rule(-340, new int[]{-9});
    rules[1061] = new Rule(-340, new int[]{-341});
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
      case 71: // abc_interface_part -> uses_clause_nonempty, simple_unit_declarations
{
            CurrentSemanticValue.stn = new interface_node((declarations)ValueStack[ValueStack.Depth-1].stn, (uses_list)ValueStack[ValueStack.Depth-2].stn, null, null);
        }
        break;
      case 72: // abc_interface_part -> simple_unit_declarations
{
            CurrentSemanticValue.stn = new interface_node((declarations)ValueStack[ValueStack.Depth-1].stn, null, null, null);
        }
        break;
      case 73: // abc_interface_part -> unit_reexport_nonempty, uses_clause_nonempty, 
               //                       unit_reexport_clauses, simple_unit_decl_list
{
            parserTools.AddErrorFromResource("REEXPORT_BEFORE_USES", LocationStack[LocationStack.Depth-4]);
            CurrentSemanticValue.stn = new interface_node((declarations)ValueStack[ValueStack.Depth-1].stn, (uses_list)ValueStack[ValueStack.Depth-3].stn, null, null);
        }
        break;
      case 74: // simple_unit_declarations -> unit_reexport_clauses, simple_unit_decl_list
{
            var defs = (declarations)ValueStack[ValueStack.Depth-1].stn;
            defs.defs.InsertRange(0, ((declarations)ValueStack[ValueStack.Depth-2].stn).defs);
            CurrentSemanticValue.stn = defs;
        }
        break;
      case 75: // simple_unit_decl_list -> /* empty */
{
            CurrentSemanticValue.stn = new declarations();
            if (GlobalDecls == null)
                GlobalDecls = (declarations)CurrentSemanticValue.stn;
        }
        break;
      case 76: // simple_unit_decl_list -> simple_unit_decl_list, simple_unit_decl
{ CurrentSemanticValue.stn = ((declarations)ValueStack[ValueStack.Depth-2].stn).Add((declaration)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); }
        break;
      case 77: // simple_unit_decl -> decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 78: // simple_unit_decl -> const_decl_sect, unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 79: // simple_unit_decl -> var_decl_sect, unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 80: // simple_unit_decl -> type_decl_sect, attribute_declarations, 
               //                     unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn; }
        break;
      case 81: // simple_unit_decl -> proc_func_constr_destr_decl_with_attr, 
               //                     unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 82: // initialization_part -> tkEnd
{ 
			CurrentSemanticValue.stn = new initfinal_part(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 83: // initialization_part -> tkInitialization, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 84: // initialization_part -> tkInitialization, stmt_list, tkFinalization, stmt_list, 
               //                        tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-5].ti, ValueStack[ValueStack.Depth-4].stn as statement_list, ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, CurrentLocationSpan);
        }
        break;
      case 85: // initialization_part -> tkBegin, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 86: // interface_decl_sect_list -> int_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 87: // int_decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations();  
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 88: // int_decl_sect_list1 -> int_decl_sect_list1, int_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 89: // decl_sect_list -> decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 90: // decl_sect_list_proc_func_only -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 91: // decl_sect_list_proc_func_only -> decl_sect_list_proc_func_only, 
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
      case 92: // decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 93: // decl_sect_list1 -> decl_sect_list1, decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 94: // inclass_decl_sect_list -> inclass_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 95: // inclass_decl_sect_list1 -> /* empty */
{ 
        	CurrentSemanticValue.stn = new declarations(); 
        }
        break;
      case 96: // inclass_decl_sect_list1 -> inclass_decl_sect_list1, abc_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 97: // int_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 98: // int_decl_sect -> const_decl_sect, unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 99: // int_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 100: // int_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 101: // int_decl_sect -> type_decl_sect, attribute_declarations, 
                //                  unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn; }
        break;
      case 102: // int_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 103: // int_decl_sect -> var_decl_sect, unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 104: // int_decl_sect -> int_proc_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 105: // int_decl_sect -> int_proc_header, unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].td; }
        break;
      case 106: // int_decl_sect -> int_func_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 107: // int_decl_sect -> int_func_header, unit_reexport_after_declarations
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].td; }
        break;
      case 108: // unit_reexport_after_declarations -> unit_reexport_nonempty
{ parserTools.AddErrorFromResource("REEXPORT_AFTER_DECLARATIONS", LocationStack[LocationStack.Depth-1]); CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 109: // decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 110: // decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 111: // decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 112: // decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 113: // decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 114: // decl_sect -> proc_func_constr_destr_decl_with_attr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 115: // proc_func_constr_destr_decl -> proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 116: // proc_func_constr_destr_decl -> constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 117: // proc_func_constr_destr_decl_with_attr -> attribute_declarations, 
                //                                          proc_func_constr_destr_decl
{
		    (ValueStack[ValueStack.Depth-1].stn as procedure_definition).AssignAttrList(ValueStack[ValueStack.Depth-2].stn as attribute_list);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 118: // abc_decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 119: // abc_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 120: // abc_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 121: // abc_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 122: // abc_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 123: // int_proc_header -> attribute_declarations, proc_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 124: // int_proc_header -> attribute_declarations, proc_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 125: // int_func_header -> attribute_declarations, func_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 126: // int_func_header -> attribute_declarations, func_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 127: // label_decl_sect -> tkLabel, label_list, tkSemiColon
{ 
			CurrentSemanticValue.stn = new label_definitions(ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
		}
        break;
      case 128: // label_list -> label_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 129: // label_list -> label_list, tkComma, label_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 130: // label_name -> tkInteger
{ 
			CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ex.ToString(), CurrentLocationSpan);
		}
        break;
      case 131: // label_name -> identifier
{ 
			CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; 
		}
        break;
      case 132: // const_decl_sect -> tkConst, const_decl
{ 
			CurrentSemanticValue.stn = new consts_definitions_list(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 133: // const_decl_sect -> const_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as consts_definitions_list).Add(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 134: // res_str_decl_sect -> tkResourceString, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 135: // res_str_decl_sect -> res_str_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 136: // type_decl_sect -> tkType, type_decl
{ 
            CurrentSemanticValue.stn = new type_declarations(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 137: // type_decl_sect -> type_decl_sect, type_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as type_declarations).Add(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 138: // var_decl_with_assign_var_tuple -> var_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 139: // var_decl_with_assign_var_tuple -> tkRoundOpen, identifier, tkComma, ident_list, 
                //                                   tkRoundClose, tkAssign, expr_l1, 
                //                                   tkSemiColon
{
			(ValueStack[ValueStack.Depth-5].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-7].id);
			ValueStack[ValueStack.Depth-5].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]);
			CurrentSemanticValue.stn = new var_tuple_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
		}
        break;
      case 140: // var_decl_sect -> tkVar, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 141: // var_decl_sect -> tkEvent, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);                        
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).is_event = true;
        }
        break;
      case 142: // var_decl_sect -> var_decl_sect, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as variable_definitions).Add(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 143: // const_decl -> only_const_decl, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 144: // only_const_decl -> const_name, tkEqual, init_const_expr
{ 
			CurrentSemanticValue.stn = new simple_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 145: // only_const_decl -> const_name, tkColon, type_ref, tkEqual, typed_const
{ 
			CurrentSemanticValue.stn = new typed_const_definition(ValueStack[ValueStack.Depth-5].id, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-3].td, CurrentLocationSpan);
		}
        break;
      case 146: // init_const_expr -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 147: // init_const_expr -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 148: // const_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 149: // const_relop_expr -> const_simple_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 150: // const_relop_expr -> const_relop_expr, const_relop, const_simple_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 151: // const_expr -> const_relop_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 152: // const_expr -> question_constexpr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 153: // const_expr -> const_expr, tkDoubleQuestion, const_relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 154: // question_constexpr -> const_expr, tkQuestion, const_expr, tkColon, const_expr
{ CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 155: // const_relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 156: // const_relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 157: // const_relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 158: // const_relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 159: // const_relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 160: // const_relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 161: // const_relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 162: // const_relop -> tkNot, tkIn
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
      case 163: // const_simple_expr -> const_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 164: // const_simple_expr -> const_simple_expr, const_addop, const_term
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 165: // const_addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 166: // const_addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 167: // const_addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 168: // const_addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 169: // as_is_constexpr -> const_term, typecast_op, simple_or_template_type_reference
{ 
			CurrentSemanticValue.ex = NewAsIsConstexpr(ValueStack[ValueStack.Depth-3].ex, (op_typecast)ValueStack[ValueStack.Depth-2].ob, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);                                
		}
        break;
      case 170: // power_constexpr -> const_factor_without_unary_op, tkStarStar, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 171: // power_constexpr -> const_factor_without_unary_op, tkStarStar, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 172: // power_constexpr -> sign, power_constexpr
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 173: // const_term -> const_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 174: // const_term -> as_is_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 175: // const_term -> power_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 176: // const_term -> const_term, const_mulop, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 177: // const_term -> const_term, const_mulop, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 178: // const_mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 179: // const_mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 180: // const_mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 181: // const_mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 182: // const_mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 183: // const_mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 184: // const_mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 185: // const_factor_without_unary_op -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 186: // const_factor_without_unary_op -> tkRoundOpen, const_expr, tkRoundClose
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 187: // const_factor -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 188: // const_factor -> const_set
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 189: // const_factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 190: // const_factor -> tkAddressOf, const_factor
{ 
			CurrentSemanticValue.ex = new get_address(ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);  
		}
        break;
      case 191: // const_factor -> tkRoundOpen, const_expr, tkRoundClose
{ 
	 	    CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 192: // const_factor -> tkNot, const_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 193: // const_factor -> sign, const_factor
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
      case 194: // const_factor -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 195: // const_factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 196: // pascal_set_const -> tkSquareOpen, elem_list, tkSquareClose
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
      case 197: // const_set -> pascal_set_const
{
            CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;				
		}
        break;
      case 198: // const_set -> tkVertParen, elem_list, tkVertParen
{ 
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, '|', CurrentLocationSpan);  
		}
        break;
      case 199: // sign -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 200: // sign -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 201: // const_variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 202: // const_variable -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 203: // const_variable -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 204: // const_variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 205: // const_variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 206: // const_variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 207: // const_variable -> const_variable, const_variable_2
{
			CurrentSemanticValue.ex = NewConstVariable(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 208: // const_variable -> const_variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 209: // const_variable -> const_variable, tkSquareOpen, format_const_expr, 
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
      case 210: // const_variable_2 -> tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.ex = new dot_node(null, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 211: // const_variable_2 -> tkDeref
{ 
			CurrentSemanticValue.ex = new roof_dereference();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 212: // const_variable_2 -> tkRoundOpen, optional_const_func_expr_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 213: // const_variable_2 -> tkSquareOpen, const_elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 214: // optional_const_func_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 215: // optional_const_func_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 216: // const_elem_list -> const_elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 218: // const_elem_list1 -> const_elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 219: // const_elem_list1 -> const_elem_list1, tkComma, const_elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 220: // const_elem -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 221: // const_elem -> const_expr, tkDotDot, const_expr
{ 
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 222: // unsigned_number -> tkInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 223: // unsigned_number -> tkHex
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 224: // unsigned_number -> tkFloat
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 225: // unsigned_number -> tkBigInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 226: // typed_const -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 227: // typed_const -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 228: // typed_const -> record_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 229: // array_const -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new array_const(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 231: // typed_const_list -> typed_const_list1
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 232: // typed_const_list1 -> typed_const_plus
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 233: // typed_const_list1 -> typed_const_list1, tkComma, typed_const_plus
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 234: // record_const -> tkRoundOpen, const_field_list, tkRoundClose
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 235: // const_field_list -> const_field_list_1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 236: // const_field_list -> const_field_list_1, tkSemiColon
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 237: // const_field_list_1 -> const_field
{ 
			CurrentSemanticValue.ex = new record_const(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 238: // const_field_list_1 -> const_field_list_1, tkSemiColon, const_field
{ 
			CurrentSemanticValue.ex = (ValueStack[ValueStack.Depth-3].ex as record_const).Add(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 239: // const_field -> const_field_name, tkColon, typed_const
{ 
			CurrentSemanticValue.stn = new record_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 240: // const_field_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 241: // type_decl -> attribute_declarations, simple_type_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = LocationStack[LocationStack.Depth-1];
        }
        break;
      case 242: // attribute_declarations -> attribute_declaration
{ 
			CurrentSemanticValue.stn = new attribute_list(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
    }
        break;
      case 243: // attribute_declarations -> attribute_declarations, attribute_declaration
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
		}
        break;
      case 244: // attribute_declarations -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 245: // attribute_declaration -> tkSquareOpen, one_or_some_attribute, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 246: // one_or_some_attribute -> one_attribute
{ 
			CurrentSemanticValue.stn = new simple_attribute_list(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 247: // one_or_some_attribute -> one_or_some_attribute, tkComma, one_attribute
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as simple_attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 248: // one_attribute -> attribute_variable
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 249: // one_attribute -> identifier, tkColon, attribute_variable
{  
			(ValueStack[ValueStack.Depth-1].stn as attribute).qualifier = ValueStack[ValueStack.Depth-3].id;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 250: // simple_type_decl -> type_decl_identifier, tkEqual, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-4].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 251: // simple_type_decl -> template_identifier_with_equal, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 252: // type_decl_identifier -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 253: // type_decl_identifier -> identifier, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 254: // template_identifier_with_equal -> identifier, tkLower, ident_list, 
                //                                   tkGreaterEqual
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-4].id.name, ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 255: // type_decl_type -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 256: // type_decl_type -> object_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 257: // simple_type_question -> simple_type, tkQuestion
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
      case 258: // simple_type_question -> template_type, tkQuestion
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
      case 259: // type_ref -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 260: // type_ref -> simple_type_question
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 261: // type_ref -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 262: // type_ref -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 263: // type_ref -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 264: // type_ref -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 265: // type_ref -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 266: // template_type -> simple_type_identifier, template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference(ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan); 
		}
        break;
      case 267: // template_type_params -> tkLower, template_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 268: // template_type_empty_params -> tkNotEqual
{
            var ntr = new named_type_reference(new ident(""), CurrentLocationSpan);
            
			CurrentSemanticValue.stn = new template_param_list(ntr, CurrentLocationSpan);
            ntr.source_context = new SourceContext(CurrentSemanticValue.stn.source_context.end_position.line_num, CurrentSemanticValue.stn.source_context.end_position.column_num, CurrentSemanticValue.stn.source_context.begin_position.line_num, CurrentSemanticValue.stn.source_context.begin_position.column_num);
		}
        break;
      case 269: // template_type_empty_params -> tkLower, template_empty_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 270: // template_param_list -> template_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 271: // template_param_list -> template_param_list, tkComma, template_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 272: // template_empty_param_list -> template_empty_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 273: // template_empty_param_list -> template_empty_param_list, tkComma, 
                //                              template_empty_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 274: // template_empty_param -> /* empty */
{ 
            CurrentSemanticValue.td = new named_type_reference(new ident(""), CurrentLocationSpan);
        }
        break;
      case 275: // template_param -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 276: // template_param -> simple_type, tkQuestion
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
      case 277: // template_param -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 278: // template_param -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 279: // template_param -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 280: // simple_type -> range_expr
{
	    	CurrentSemanticValue.td = parserTools.ConvertDotNodeOrIdentToNamedTypeReference(ValueStack[ValueStack.Depth-1].ex); 
	    }
        break;
      case 281: // simple_type -> range_expr, tkDotDot, range_expr
{ 
			CurrentSemanticValue.td = new diapason(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 282: // simple_type -> tkRoundOpen, enumeration_id_list, tkRoundClose
{ 
			CurrentSemanticValue.td = new enum_type_definition(ValueStack[ValueStack.Depth-2].stn as enumerator_list, CurrentLocationSpan);  
		}
        break;
      case 283: // range_expr -> range_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 284: // range_expr -> range_expr, const_addop, range_term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 285: // range_term -> range_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 286: // range_term -> range_term, const_mulop, range_factor
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 287: // range_factor -> simple_type_identifier
{ 
			CurrentSemanticValue.ex = parserTools.ConvertNamedTypeReferenceToDotNodeOrIdent(ValueStack[ValueStack.Depth-1].td as named_type_reference);
        }
        break;
      case 288: // range_factor -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 289: // range_factor -> sign, range_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 290: // range_factor -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 291: // range_factor -> range_factor, tkRoundOpen, const_elem_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value, ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 292: // simple_type_identifier -> identifier
{ 
			CurrentSemanticValue.td = new named_type_reference(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 293: // simple_type_identifier -> simple_type_identifier, tkPoint, 
                //                           identifier_or_keyword
{ 
			CurrentSemanticValue.td = (ValueStack[ValueStack.Depth-3].td as named_type_reference).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 294: // enumeration_id_list -> enumeration_id
{ 
			CurrentSemanticValue.stn = new enumerator_list(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 295: // enumeration_id_list -> enumeration_id_list, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 296: // enumeration_id -> type_ref
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan); 
		}
        break;
      case 297: // enumeration_id -> type_ref, tkEqual, expr
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 298: // pointer_type -> tkDeref, fptype
{ 
			CurrentSemanticValue.td = new ref_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 299: // structured_type -> array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 300: // structured_type -> record_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 301: // structured_type -> set_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 302: // structured_type -> file_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 303: // structured_type -> sequence_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 304: // sequence_type -> tkSequence, tkOf, type_ref
{
			CurrentSemanticValue.td = new sequence_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 305: // array_type -> tkArray, tkSquareOpen, simple_type_list, tkSquareClose, tkOf, 
                //               type_ref
{ 
			CurrentSemanticValue.td = new array_type(ValueStack[ValueStack.Depth-4].stn as indexers_types, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 306: // array_type -> unsized_array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 307: // unsized_array_type -> tkArray, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new array_type(null, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 308: // simple_type_list -> simple_type_or_
{ 
			CurrentSemanticValue.stn = new indexers_types(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 309: // simple_type_list -> simple_type_list, tkComma, simple_type_or_
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as indexers_types).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 310: // simple_type_or_ -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 311: // simple_type_or_ -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 312: // set_type -> tkSet, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new set_type_definition(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 313: // file_type -> tkFile, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new file_type(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 314: // file_type -> tkFile
{ 
			CurrentSemanticValue.td = new file_type();  
			CurrentSemanticValue.td.source_context = CurrentLocationSpan;
		}
        break;
      case 315: // string_type -> tkIdentifier, tkSquareOpen, const_expr, tkSquareClose
{ 
			CurrentSemanticValue.td = new string_num_definition(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-4].id, CurrentLocationSpan);
		}
        break;
      case 316: // procedural_type -> procedural_type_kind
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 317: // procedural_type_kind -> proc_type_decl
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 318: // proc_type_decl -> tkProcedure, fp_list
{ 
			CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-1].stn as formal_parameters,null,null,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 319: // proc_type_decl -> tkFunction, fp_list, tkColon, fptype
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, null, null, null, ValueStack[ValueStack.Depth-1].td as type_definition, CurrentLocationSpan);
        }
        break;
      case 320: // proc_type_decl -> simple_type_identifier, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 321: // proc_type_decl -> template_type, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 322: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 323: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-4].stn as enumerator_list,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 324: // proc_type_decl -> simple_type_identifier, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 325: // proc_type_decl -> template_type, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 326: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,null,CurrentLocationSpan);
    	}
        break;
      case 327: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-5].stn as enumerator_list,null,CurrentLocationSpan);
    	}
        break;
      case 328: // object_type -> class_attributes, class_or_interface_keyword, 
                //                optional_base_classes, optional_where_section, 
                //                optional_component_list_seq_end
{ 
            var cd = NewObjectType((class_attribute)ValueStack[ValueStack.Depth-5].ob, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as where_definition_list, ValueStack[ValueStack.Depth-1].stn as class_body_list, CurrentLocationSpan); 
			CurrentSemanticValue.td = cd;
		}
        break;
      case 329: // record_type -> tkRecord, optional_base_classes, optional_where_section, 
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
      case 330: // class_attribute -> tkSealed
{ CurrentSemanticValue.ob = class_attribute.Sealed; }
        break;
      case 331: // class_attribute -> tkPartial
{ CurrentSemanticValue.ob = class_attribute.Partial; }
        break;
      case 332: // class_attribute -> tkAbstract
{ CurrentSemanticValue.ob = class_attribute.Abstract; }
        break;
      case 333: // class_attribute -> tkAuto
{ CurrentSemanticValue.ob = class_attribute.Auto; }
        break;
      case 334: // class_attribute -> tkStatic
{ CurrentSemanticValue.ob = class_attribute.Static; }
        break;
      case 335: // class_attributes -> /* empty */
{ 
			CurrentSemanticValue.ob = class_attribute.None; 
		}
        break;
      case 336: // class_attributes -> class_attributes1
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 337: // class_attributes1 -> class_attribute
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 338: // class_attributes1 -> class_attributes1, class_attribute
{
            if (((class_attribute)ValueStack[ValueStack.Depth-2].ob & (class_attribute)ValueStack[ValueStack.Depth-1].ob) == (class_attribute)ValueStack[ValueStack.Depth-1].ob)
                parserTools.AddErrorFromResource("ATTRIBUTE_REDECLARED",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.ob  = ((class_attribute)ValueStack[ValueStack.Depth-2].ob) | ((class_attribute)ValueStack[ValueStack.Depth-1].ob);
			//$$ = $1;
		}
        break;
      case 339: // class_or_interface_keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 340: // class_or_interface_keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 341: // class_or_interface_keyword -> tkTemplate
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-1].ti);
		}
        break;
      case 342: // class_or_interface_keyword -> tkTemplate, tkClass
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "c", CurrentLocationSpan);
		}
        break;
      case 343: // class_or_interface_keyword -> tkTemplate, tkRecord
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "r", CurrentLocationSpan);
		}
        break;
      case 344: // class_or_interface_keyword -> tkTemplate, tkInterface
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "i", CurrentLocationSpan);
		}
        break;
      case 345: // optional_component_list_seq_end -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 346: // optional_component_list_seq_end -> member_list_section, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 348: // optional_base_classes -> tkRoundOpen, base_classes_names_list, tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 349: // base_classes_names_list -> base_class_name
{ 
			CurrentSemanticValue.stn = new named_type_reference_list(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 350: // base_classes_names_list -> base_classes_names_list, tkComma, base_class_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as named_type_reference_list).Add(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 351: // base_class_name -> simple_type_identifier
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 352: // base_class_name -> template_type
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 353: // template_arguments -> tkLower, ident_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 354: // optional_where_section -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 355: // optional_where_section -> where_part_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 356: // where_part_list -> where_part
{ 
			CurrentSemanticValue.stn = new where_definition_list(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 357: // where_part_list -> where_part_list, where_part
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as where_definition_list).Add(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 358: // where_part -> tkWhere, ident_list, tkColon, type_ref_and_secific_list, 
                //               tkSemiColon
{ 
			CurrentSemanticValue.stn = new where_definition(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-2].stn as where_type_specificator_list, CurrentLocationSpan); 
		}
        break;
      case 359: // type_ref_and_secific_list -> type_ref_or_secific
{ 
			CurrentSemanticValue.stn = new where_type_specificator_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 360: // type_ref_and_secific_list -> type_ref_and_secific_list, tkComma, 
                //                              type_ref_or_secific
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as where_type_specificator_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 361: // type_ref_or_secific -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 362: // type_ref_or_secific -> tkClass
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefClass, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 363: // type_ref_or_secific -> tkRecord
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 364: // type_ref_or_secific -> tkConstructor
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 365: // member_list_section -> member_list
{ 
			CurrentSemanticValue.stn = new class_body_list(ValueStack[ValueStack.Depth-1].stn as class_members, CurrentLocationSpan);
        }
        break;
      case 366: // member_list_section -> member_list_section, ot_visibility_specifier, 
                //                        member_list
{ 
		    (ValueStack[ValueStack.Depth-1].stn as class_members).access_mod = ValueStack[ValueStack.Depth-2].stn as access_modifer_node;
			(ValueStack[ValueStack.Depth-3].stn as class_body_list).Add(ValueStack[ValueStack.Depth-1].stn as class_members,CurrentLocationSpan);
			
			if ((ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks[0].Count == 0)
                (ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks.RemoveAt(0);
			
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 367: // ot_visibility_specifier -> tkInternal
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.internal_modifer, CurrentLocationSpan); }
        break;
      case 368: // ot_visibility_specifier -> tkPublic
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.public_modifer, CurrentLocationSpan); }
        break;
      case 369: // ot_visibility_specifier -> tkProtected
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.protected_modifer, CurrentLocationSpan); }
        break;
      case 370: // ot_visibility_specifier -> tkPrivate
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.private_modifer, CurrentLocationSpan); }
        break;
      case 371: // member_list -> /* empty */
{ CurrentSemanticValue.stn = new class_members(); }
        break;
      case 372: // member_list -> field_or_const_definition_list, optional_semicolon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 373: // member_list -> method_decl_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 374: // member_list -> field_or_const_definition_list, tkSemiColon, method_decl_list
{  
			(ValueStack[ValueStack.Depth-3].stn as class_members).members.AddRange((ValueStack[ValueStack.Depth-1].stn as class_members).members);
			(ValueStack[ValueStack.Depth-3].stn as class_members).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 375: // ident_list -> identifier
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 376: // ident_list -> ident_list, tkComma, identifier
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 377: // optional_semicolon -> /* empty */
{ CurrentSemanticValue.ob = null; }
        break;
      case 378: // optional_semicolon -> tkSemiColon
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 379: // field_or_const_definition_list -> field_or_const_definition
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 380: // field_or_const_definition_list -> field_or_const_definition_list, tkSemiColon, 
                //                                   field_or_const_definition
{   
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 381: // field_or_const_definition -> attribute_declarations, 
                //                              simple_field_or_const_definition
{  
		    (ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 382: // method_decl_list -> method_or_property_decl
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 383: // method_decl_list -> method_decl_list, method_or_property_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 384: // method_or_property_decl -> method_decl_withattr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 385: // method_or_property_decl -> property_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 386: // simple_field_or_const_definition -> tkConst, only_const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 387: // simple_field_or_const_definition -> field_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 388: // simple_field_or_const_definition -> class_or_static, field_definition
{ 
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).var_attr = definition_attribute.Static;
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 389: // class_or_static -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 390: // class_or_static -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 391: // field_definition -> var_decl_part
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 392: // field_definition -> tkEvent, ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, true, CurrentLocationSpan); 
        }
        break;
      case 393: // method_decl_withattr -> attribute_declarations, method_header
{  
			(ValueStack[ValueStack.Depth-1].td as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td;
        }
        break;
      case 394: // method_decl_withattr -> attribute_declarations, method_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
            if (ValueStack[ValueStack.Depth-1].stn is procedure_definition && (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
                (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
     }
        break;
      case 395: // method_decl -> inclass_proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 396: // method_decl -> inclass_constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 397: // method_header -> class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 398: // method_header -> method_procfunc_header
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
		}
        break;
      case 399: // method_header -> tkAsync, class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 400: // method_header -> tkAsync, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
		}
        break;
      case 401: // method_header -> constr_destr_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 402: // method_procfunc_header -> proc_func_header
{ 
			CurrentSemanticValue.td = NewProcfuncHeading(ValueStack[ValueStack.Depth-1].td as procedure_header);
		}
        break;
      case 403: // proc_func_header -> proc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 404: // proc_func_header -> func_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 405: // constr_destr_header -> tkConstructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 406: // constr_destr_header -> class_or_static, tkConstructor, optional_proc_name, 
                //                        fp_list, optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,true,null,null,CurrentLocationSpan);
        }
        break;
      case 407: // constr_destr_header -> tkDestructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new destructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name, false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 408: // optional_proc_name -> proc_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 409: // optional_proc_name -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 410: // property_definition -> attribute_declarations, simple_property_definition
{  
			CurrentSemanticValue.stn = NewPropertyDefinition(ValueStack[ValueStack.Depth-2].stn as attribute_list, ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 411: // simple_property_definition -> tkProperty, func_name, property_interface, 
                //                               property_specifiers, tkSemiColon, 
                //                               array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        }
        break;
      case 412: // simple_property_definition -> tkProperty, func_name, property_interface, 
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
      case 413: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        	(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
        }
        break;
      case 414: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, property_modificator, tkSemiColon, 
                //                               array_defaultproperty
{ 
			parserTools.AddErrorFromResource("STATIC_PROPERTIES_CANNOT_HAVE_ATTRBUTE_{0}",LocationStack[LocationStack.Depth-3],ValueStack[ValueStack.Depth-3].id.name);        	
        }
        break;
      case 415: // simple_property_definition -> tkAuto, tkProperty, func_name, property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 416: // simple_property_definition -> class_or_static, tkAuto, tkProperty, func_name, 
                //                               property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 417: // optional_property_initialization -> tkAssign, expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 418: // optional_property_initialization -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 419: // array_defaultproperty -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 420: // array_defaultproperty -> tkDefault, tkSemiColon
{ 
			CurrentSemanticValue.stn = new property_array_default();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 421: // property_interface -> property_parameter_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_interface(ValueStack[ValueStack.Depth-3].stn as property_parameter_list, ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan);
        }
        break;
      case 422: // property_parameter_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 423: // property_parameter_list -> tkSquareOpen, parameter_decl_list, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 424: // parameter_decl_list -> parameter_decl
{ 
			CurrentSemanticValue.stn = new property_parameter_list(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 425: // parameter_decl_list -> parameter_decl_list, tkSemiColon, parameter_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as property_parameter_list).Add(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 426: // parameter_decl -> ident_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_parameter(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 427: // optional_read_expr -> expr_with_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 428: // optional_read_expr -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 430: // property_specifiers -> tkRead, optional_read_expr, write_property_specifiers
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
      case 431: // property_specifiers -> tkWrite, unlabelled_stmt, read_property_specifiers
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
      case 433: // write_property_specifiers -> tkWrite, unlabelled_stmt
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
      case 435: // read_property_specifiers -> tkRead, optional_read_expr
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
      case 436: // var_decl -> var_decl_part, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 439: // var_decl_part -> ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, false, CurrentLocationSpan);
		}
        break;
      case 440: // var_decl_part -> ident_list, tkAssign, expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, null, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan);		
		}
        break;
      case 441: // var_decl_part -> ident_list, tkColon, type_ref, tkAssignOrEqual, 
                //                  typed_var_init_expression
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan); 
		}
        break;
      case 442: // typed_var_init_expression -> typed_const_plus
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 443: // typed_var_init_expression -> const_simple_expr, tkDotDot, const_term
{ 
		if (parserTools.buildTreeForFormatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
		}
        break;
      case 444: // typed_var_init_expression -> expl_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 445: // typed_var_init_expression -> identifier, tkArrow, lambda_function_body
{  
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 446: // typed_var_init_expression -> tkRoundOpen, tkRoundClose, lambda_type_ref, 
                //                              tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 447: // typed_var_init_expression -> tkRoundOpen, typed_const_list, tkRoundClose, 
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
      case 448: // typed_var_init_expression -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 449: // typed_const_plus -> typed_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 450: // constr_destr_decl -> constr_destr_header, block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 451: // constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, tkAssign, 
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
      case 452: // constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
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
      case 453: // inclass_constr_destr_decl -> constr_destr_header, inclass_block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 454: // inclass_constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, 
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
      case 455: // inclass_constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
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
      case 456: // proc_func_decl -> proc_func_decl_without_modifiers
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 457: // proc_func_decl -> class_or_static, proc_func_decl_without_modifiers
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 458: // proc_func_decl -> tkAsync, proc_func_decl_without_modifiers
{
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;		
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 459: // proc_func_decl -> tkAsync, class_or_static, proc_func_decl_without_modifiers
{ 
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 460: // proc_func_decl -> class_or_static, tkAsync, proc_func_decl_without_modifiers
{ 
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 461: // inclass_proc_func_decl -> inclass_proc_func_decl_without_modifiers
{ 
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 462: // inclass_proc_func_decl -> tkAsync, inclass_proc_func_decl_without_modifiers
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 463: // inclass_proc_func_decl -> class_or_static, 
                //                           inclass_proc_func_decl_without_modifiers
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
		    {
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			}
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 464: // inclass_proc_func_decl -> tkAsync, class_or_static, 
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
      case 465: // inclass_proc_func_decl -> class_or_static, tkAsync, 
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
      case 466: // proc_func_decl_without_modifiers -> proc_func_header, proc_func_external_block
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
      case 467: // proc_func_decl_without_modifiers -> proc_func_header, tkForward, tkSemiColon
{
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-3].td as procedure_header, null, CurrentLocationSpan);
            (CurrentSemanticValue.stn as procedure_definition).proc_header.proc_attributes.Add((procedure_attribute)ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-2].id.source_context);
		}
        break;
      case 468: // proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, tkColon, 
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
      case 469: // proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, 
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
      case 470: // proc_func_decl_without_modifiers -> tkProcedure, proc_name, fp_list, 
                //                                     optional_method_modificators1, tkAssign, 
                //                                     unlabelled_stmt, tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-2]);
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 471: // inclass_proc_func_decl_without_modifiers -> proc_func_header, inclass_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
		}
        break;
      case 472: // inclass_proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, 
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
      case 473: // inclass_proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, 
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
      case 474: // inclass_proc_func_decl_without_modifiers -> tkProcedure, proc_name, fp_list, 
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
      case 475: // proc_func_external_block -> block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 476: // proc_func_external_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 477: // proc_name -> func_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 478: // func_name -> func_meth_name_ident
{ 
			CurrentSemanticValue.stn = new method_name(null,null, ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan); 
		}
        break;
      case 479: // func_name -> func_class_name_ident_list, tkPoint, func_meth_name_ident
{ 
        	var ln = ValueStack[ValueStack.Depth-3].ob as List<ident>;
        	var cnt = ln.Count;
        	if (cnt == 1)
				CurrentSemanticValue.stn = new method_name(null, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
			else 	
				CurrentSemanticValue.stn = new method_name(ln, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
		}
        break;
      case 480: // func_class_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 481: // func_class_name_ident_list -> func_class_name_ident
{ 
			CurrentSemanticValue.ob = new List<ident>(); 
			(CurrentSemanticValue.ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
		}
        break;
      case 482: // func_class_name_ident_list -> func_class_name_ident_list, tkPoint, 
                //                               func_class_name_ident
{ 
			(ValueStack[ValueStack.Depth-3].ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob; 
		}
        break;
      case 483: // func_meth_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 484: // func_meth_name_ident -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 485: // func_meth_name_ident -> operator_name_ident, template_arguments
{ CurrentSemanticValue.id = new template_operator_name(null, ValueStack[ValueStack.Depth-1].stn as ident_list, ValueStack[ValueStack.Depth-2].ex as operator_name_ident, CurrentLocationSpan); }
        break;
      case 486: // func_name_with_template_args -> func_name_ident
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 487: // func_name_with_template_args -> func_name_ident, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 488: // func_name_ident -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 489: // proc_header -> tkProcedure, proc_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{ 
        	CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, CurrentLocationSpan); 
        }
        break;
      case 490: // func_header -> tkFunction, func_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, null, CurrentLocationSpan); 
		}
        break;
      case 491: // func_header -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                optional_method_modificators, optional_where_section
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, ValueStack[ValueStack.Depth-3].td as type_definition, CurrentLocationSpan); 
        }
        break;
      case 492: // external_block -> tkExternal, external_directive_ident, tkName, 
                //                   external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 493: // external_block -> tkExternal, external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-2].ex, null, CurrentLocationSpan); 
		}
        break;
      case 494: // external_block -> tkExternal, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(null, null, CurrentLocationSpan); 
		}
        break;
      case 495: // external_directive_ident -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 496: // external_directive_ident -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 497: // block -> decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 498: // inclass_block -> inclass_decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 499: // inclass_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 500: // fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 501: // fp_list -> tkRoundOpen, tkRoundClose
{ 
			CurrentSemanticValue.stn = null;
		}
        break;
      case 502: // fp_list -> tkRoundOpen, fp_sect_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			if (CurrentSemanticValue.stn != null)
				CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 503: // fp_sect_list -> fp_sect
{ 
			CurrentSemanticValue.stn = new formal_parameters(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
        }
        break;
      case 504: // fp_sect_list -> fp_sect_list, tkSemiColon, fp_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);   
        }
        break;
      case 505: // fp_sect -> attribute_declarations, simple_fp_sect
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as  attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 506: // simple_fp_sect -> param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan); 
		}
        break;
      case 507: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.var_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 508: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.const_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 509: // simple_fp_sect -> tkParams, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td,parametr_kind.params_parametr,null, CurrentLocationSpan);  
		}
        break;
      case 510: // simple_fp_sect -> param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.none, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 511: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.var_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 512: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.const_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 513: // param_name_list -> param_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 514: // param_name_list -> param_name_list, tkComma, param_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);  
		}
        break;
      case 515: // param_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 516: // fptype -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 517: // fptype_noproctype -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 518: // fptype_noproctype -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 519: // fptype_noproctype -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 520: // fptype_noproctype -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 521: // fptype_noproctype -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 522: // stmt -> unlabelled_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 523: // stmt -> label_name, tkColon, stmt
{ 
			CurrentSemanticValue.stn = new labeled_statement(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);  
		}
        break;
      case 524: // unlabelled_stmt -> /* empty */
{ 
			CurrentSemanticValue.stn = new empty_statement(); 
			CurrentSemanticValue.stn.source_context = null;
		}
        break;
      case 525: // unlabelled_stmt -> assignment
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 526: // unlabelled_stmt -> proc_call
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 527: // unlabelled_stmt -> goto_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 528: // unlabelled_stmt -> compound_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 529: // unlabelled_stmt -> if_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 530: // unlabelled_stmt -> case_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 531: // unlabelled_stmt -> repeat_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 532: // unlabelled_stmt -> while_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 533: // unlabelled_stmt -> for_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 534: // unlabelled_stmt -> with_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 535: // unlabelled_stmt -> inherited_message
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 536: // unlabelled_stmt -> try_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 537: // unlabelled_stmt -> raise_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 538: // unlabelled_stmt -> foreach_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 539: // unlabelled_stmt -> var_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 540: // unlabelled_stmt -> expr_as_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 541: // unlabelled_stmt -> lock_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 542: // unlabelled_stmt -> yield_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 543: // unlabelled_stmt -> yield_sequence_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 544: // unlabelled_stmt -> loop_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 545: // unlabelled_stmt -> match_with
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 546: // loop_stmt -> tkLoop, expr_l1, tkDo, unlabelled_stmt
{
			CurrentSemanticValue.stn = new loop_stmt(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].stn as statement,CurrentLocationSpan);
		}
        break;
      case 547: // yield_stmt -> tkYield, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 548: // yield_sequence_stmt -> tkYield, tkSequence, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_sequence_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 549: // var_stmt -> tkVar, var_decl_part
{ 
			CurrentSemanticValue.stn = new var_statement(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 550: // var_stmt -> tkRoundOpen, tkVar, identifier, tkComma, var_ident_list, 
                //             tkRoundClose, tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].ob as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].ob as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 551: // var_stmt -> tkVar, tkRoundOpen, identifier, tkComma, ident_list, tkRoundClose, 
                //             tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			ValueStack[ValueStack.Depth-4].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
	    }
        break;
      case 552: // assignment -> var_reference, assign_operator, expr_with_func_decl_lambda
{      
        	if (!(ValueStack[ValueStack.Depth-3].ex is addressed_value))
        		parserTools.AddErrorFromResource("LEFT_SIDE_CANNOT_BE_ASSIGNED_TO",CurrentLocationSpan);
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
        }
        break;
      case 553: // assignment -> tkRoundOpen, variable, tkComma, variable_list, tkRoundClose, 
                //               assign_operator, expr
{
			if (ValueStack[ValueStack.Depth-2].op.type != Operators.Assignment)
			    parserTools.AddErrorFromResource("ONLY_BASE_ASSIGNMENT_FOR_TUPLE",LocationStack[LocationStack.Depth-2]);
			(ValueStack[ValueStack.Depth-4].ob as addressed_value_list).Insert(0,ValueStack[ValueStack.Depth-6].ex as addressed_value);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_tuple(ValueStack[ValueStack.Depth-4].ob as addressed_value_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 554: // variable_list -> variable
{
		CurrentSemanticValue.ob = new addressed_value_list(ValueStack[ValueStack.Depth-1].ex as addressed_value,LocationStack[LocationStack.Depth-1]);
	}
        break;
      case 555: // variable_list -> variable_list, tkComma, variable
{
		(ValueStack[ValueStack.Depth-3].ob as addressed_value_list).Add(ValueStack[ValueStack.Depth-1].ex as addressed_value);
		(ValueStack[ValueStack.Depth-3].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
	}
        break;
      case 556: // var_ident_list -> tkVar, identifier
{
		CurrentSemanticValue.ob = new ident_list(ValueStack[ValueStack.Depth-1].id,CurrentLocationSpan);
	}
        break;
      case 557: // var_ident_list -> var_ident_list, tkComma, tkVar, identifier
{
		(ValueStack[ValueStack.Depth-4].ob as ident_list).Add(ValueStack[ValueStack.Depth-1].id);
		(ValueStack[ValueStack.Depth-4].ob as ident_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-4].ob;
	}
        break;
      case 558: // proc_call -> var_reference
{ 
			CurrentSemanticValue.stn = new procedure_call(ValueStack[ValueStack.Depth-1].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex is ident, CurrentLocationSpan); 
		}
        break;
      case 559: // goto_stmt -> tkGoto, label_name
{ 
			CurrentSemanticValue.stn = new goto_statement(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 560: // compound_stmt -> tkBegin, stmt_list, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			(CurrentSemanticValue.stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(CurrentSemanticValue.stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 561: // stmt_list -> stmt
{ 
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 562: // stmt_list -> stmt_list, tkSemiColon, stmt
{  
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as statement_list).Add(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 563: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan); 
        }
        break;
      case 564: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt, tkElse, unlabelled_stmt
{
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as statement, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 565: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, else_case, tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 566: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, tkSemiColon, else_case, 
                //               tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 567: // pattern_cases -> pattern_case
{
            CurrentSemanticValue.stn = new pattern_cases(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 568: // pattern_cases -> pattern_cases, tkSemiColon, pattern_case
{
            CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as pattern_cases).Add(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 569: // pattern_case -> pattern_optional_var, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 570: // pattern_case -> deconstruction_or_const_pattern, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 571: // pattern_case -> collection_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 572: // pattern_case -> tuple_pattern, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
		}
        break;
      case 573: // pattern_case -> tuple_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 574: // case_stmt -> tkCase, expr_l1, tkOf, case_list, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 575: // case_stmt -> tkCase, expr_l1, tkOf, case_list, tkSemiColon, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 576: // case_stmt -> tkCase, expr_l1, tkOf, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-4].ex, NewCaseItem(new empty_statement(), null), ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 577: // case_list -> case_item
{
			if (ValueStack[ValueStack.Depth-1].stn is empty_statement) 
				CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, null);
			else CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 578: // case_list -> case_list, tkSemiColon, case_item
{ 
			CurrentSemanticValue.stn = AddCaseItem(ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 579: // case_item -> case_label_list, tkColon, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new case_variant(ValueStack[ValueStack.Depth-3].stn as expression_list, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 580: // case_label_list -> case_label
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 581: // case_label_list -> case_label_list, tkComma, case_label
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 582: // case_label -> const_elem
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 583: // else_case -> /* empty */
{ CurrentSemanticValue.stn = null;}
        break;
      case 584: // else_case -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 585: // repeat_stmt -> tkRepeat, stmt_list, tkUntil, expr
{ 
		    CurrentSemanticValue.stn = new repeat_node(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-3].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-4].ti;
			(ValueStack[ValueStack.Depth-3].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-2].ti;
			ValueStack[ValueStack.Depth-3].stn.source_context = LocationStack[LocationStack.Depth-4].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 586: // while_stmt -> tkWhile, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewWhileStmt(ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);    
        }
        break;
      case 587: // optional_tk_do -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 588: // optional_tk_do -> /* empty */
{ CurrentSemanticValue.ti = null; }
        break;
      case 589: // lock_stmt -> tkLock, expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new lock_stmt(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 590: // index_or_nothing -> tkIndex, tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 591: // index_or_nothing -> /* empty */
{ CurrentSemanticValue.id = null; }
        break;
      case 592: // optional_type_specification -> tkColon, type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 593: // optional_type_specification -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 594: // optional_var -> tkVar
{ CurrentSemanticValue.ob = true; }
        break;
      case 595: // optional_var -> /* empty */
{ CurrentSemanticValue.ob = false; }
        break;
      case 596: // for_cycle_type -> tkTo
{ CurrentSemanticValue.ob = for_cycle_type.to; }
        break;
      case 597: // for_cycle_type -> tkDownto
{ CurrentSemanticValue.ob = for_cycle_type.downto; }
        break;
      case 598: // foreach_stmt -> tkForeach, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
            if (ValueStack[ValueStack.Depth-6].td == null)
                parserTools.AddWarningFromResource("USING_UNLOCAL_FOREACH_VARIABLE", ValueStack[ValueStack.Depth-7].id.source_context);
        }
        break;
      case 599: // foreach_stmt -> tkForeach, tkVar, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
        	if (ValueStack[ValueStack.Depth-6].td == null)
				CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, new no_type_foreach(), ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
			else CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
        }
        break;
      case 600: // foreach_stmt -> tkForeach, tkVar, tkRoundOpen, ident_list, tkRoundClose, tkIn, 
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
      case 601: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, optional_tk_do, 
                //             unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-9].ob, ValueStack[ValueStack.Depth-8].id, ValueStack[ValueStack.Depth-7].td, ValueStack[ValueStack.Depth-5].ex, (for_cycle_type)ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 602: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, tkStep, expr_l1, tkDo, 
                //             unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-11].ob, ValueStack[ValueStack.Depth-10].id, ValueStack[ValueStack.Depth-9].td, ValueStack[ValueStack.Depth-7].ex, (for_cycle_type)ValueStack[ValueStack.Depth-6].ob, ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 603: // with_stmt -> tkWith, expr_list, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new with_statement(ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 604: // inherited_message -> tkInherited
{ 
			CurrentSemanticValue.stn = new inherited_message();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 605: // try_stmt -> tkTry, stmt_list, try_handler
{ 
			CurrentSemanticValue.stn = new try_stmt(ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].stn as try_handler, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			ValueStack[ValueStack.Depth-2].stn.source_context = LocationStack[LocationStack.Depth-3].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 606: // try_handler -> tkFinally, stmt_list, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_finally(ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan);
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(ValueStack[ValueStack.Depth-2].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
		}
        break;
      case 607: // try_handler -> tkExcept, exception_block, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_except((exception_block)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan);  
			if ((ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list != null)
			{
				(ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list.source_context = CurrentLocationSpan;
				(ValueStack[ValueStack.Depth-2].stn as exception_block).source_context = CurrentLocationSpan;
			}
		}
        break;
      case 608: // exception_block -> exception_handler_list, exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-2].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 609: // exception_block -> exception_handler_list, tkSemiColon, 
                //                    exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-3].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 610: // exception_block -> stmt_list
{ 
			CurrentSemanticValue.stn = new exception_block(ValueStack[ValueStack.Depth-1].stn as statement_list, null, null, LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 611: // exception_handler_list -> exception_handler
{ 
			CurrentSemanticValue.stn = new exception_handler_list(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 612: // exception_handler_list -> exception_handler_list, tkSemiColon, 
                //                           exception_handler
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as exception_handler_list).Add(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 613: // exception_block_else_branch -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 614: // exception_block_else_branch -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 615: // exception_handler -> tkOn, exception_identifier, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new exception_handler((ValueStack[ValueStack.Depth-3].stn as exception_ident).variable, (ValueStack[ValueStack.Depth-3].stn as exception_ident).type_name, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 616: // exception_identifier -> exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(null, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 617: // exception_identifier -> exception_variable, tkColon, 
                //                         exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(ValueStack[ValueStack.Depth-3].id, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 618: // exception_class_type_identifier -> simple_type_identifier
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 619: // exception_variable -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 620: // raise_stmt -> tkRaise
{ 
			CurrentSemanticValue.stn = new raise_stmt(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 621: // raise_stmt -> tkRaise, expr
{ 
			CurrentSemanticValue.stn = new raise_stmt(ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);  
		}
        break;
      case 622: // expr_list -> expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 623: // expr_list -> expr_list, tkComma, expr_with_func_decl_lambda
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 624: // expr_list_func_param -> expr_with_func_decl_lambda_ass
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 625: // expr_list_func_param -> expr_list_func_param, tkComma, 
                //                         expr_with_func_decl_lambda_ass
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 626: // expr_as_stmt -> allowable_expr_as_stmt
{ 
			CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 627: // allowable_expr_as_stmt -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 628: // expr_with_func_decl_lambda -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 629: // expr_with_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 630: // expr_with_func_decl_lambda -> tkInherited
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 631: // expr_with_func_decl_lambda_ass -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 632: // expr_with_func_decl_lambda_ass -> identifier, tkAssign, expr_l1
{ CurrentSemanticValue.ex = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); }
        break;
      case 633: // expr_with_func_decl_lambda_ass -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 634: // expr_with_func_decl_lambda_ass -> tkInherited
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 635: // expr -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 636: // expr -> format_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 637: // expr -> expr, tkTo, expr_l1
{ CurrentSemanticValue.ex = new to_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 638: // expr_l1 -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 639: // expr_l1 -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 640: // expr_l1 -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 641: // expr_l1_for_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 642: // expr_l1_for_question_expr -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 643: // expr_l1_for_new_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 644: // expr_l1_for_new_question_expr -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 645: // expr_l1_func_decl_lambda -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 646: // expr_l1_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 647: // expr_l1_for_lambda -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 648: // expr_l1_for_lambda -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 649: // expr_l1_for_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 650: // expr_dq -> relop_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 651: // expr_dq -> expr_dq, tkDoubleQuestion, relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 652: // sizeof_expr -> tkSizeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new sizeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, null, CurrentLocationSpan);  
		}
        break;
      case 653: // typeof_expr -> tkTypeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 654: // typeof_expr -> tkTypeOf, tkRoundOpen, empty_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 655: // question_expr -> expr_l1_for_question_expr, tkQuestion, 
                //                  expr_l1_for_question_expr, tkColon, 
                //                  expr_l1_for_question_expr
{ 
            if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
            	parserTools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 656: // new_question_expr -> tkIf, expr_l1_for_new_question_expr, tkThen, 
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
      case 657: // empty_template_type_reference -> simple_type_identifier, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 658: // empty_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
        }
        break;
      case 659: // simple_or_template_type_reference -> simple_type_identifier
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 660: // simple_or_template_type_reference -> simple_type_identifier, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 661: // simple_or_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 662: // simple_or_template_or_question_type_reference -> 
                //                                                  simple_or_template_type_reference
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 663: // simple_or_template_or_question_type_reference -> simple_type_question
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 664: // optional_array_initializer -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = new array_const((expression_list)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan); 
		}
        break;
      case 666: // new_expr -> tkNew, simple_or_template_type_reference, 
                //             optional_expr_list_func_param_with_bracket
{
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-2].td, ValueStack[ValueStack.Depth-1].stn as expression_list, false, null, CurrentLocationSpan);
        }
        break;
      case 667: // new_expr -> tkNew, simple_or_template_type_reference, tkSquareOpen, 
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
      case 668: // new_expr -> tkNew, tkClass, tkRoundOpen, list_fields_in_unnamed_object, 
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
      case 669: // field_in_unnamed_object -> identifier, tkAssign, expr_l1
{
		    if (ValueStack[ValueStack.Depth-1].ex is nil_const)
				parserTools.AddErrorFromResource("NIL_IN_UNNAMED_OBJECT",CurrentLocationSpan);		    
			CurrentSemanticValue.ob = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 670: // field_in_unnamed_object -> expr_l1
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
      case 671: // list_fields_in_unnamed_object -> field_in_unnamed_object
{
			var l = new name_assign_expr_list();
			CurrentSemanticValue.ob = l.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
		}
        break;
      case 672: // list_fields_in_unnamed_object -> list_fields_in_unnamed_object, tkComma, 
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
      case 673: // optional_expr_list_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 674: // optional_expr_list_with_bracket -> tkRoundOpen, optional_expr_list, 
                //                                    tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 675: // optional_expr_list_func_param_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 676: // optional_expr_list_func_param_with_bracket -> tkRoundOpen, 
                //                                               optional_expr_list_func_param, 
                //                                               tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 677: // relop_expr -> simple_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 678: // relop_expr -> relop_expr, relop, simple_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 679: // relop_expr -> relop_expr, relop, new_question_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 680: // relop_expr -> is_type_expr, tkRoundOpen, pattern_out_param_list, tkRoundClose
{
            var isTypeCheck = ValueStack[ValueStack.Depth-4].ex as typecast_node;
            var deconstructorPattern = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, isTypeCheck.type_def, null, CurrentLocationSpan); 
            CurrentSemanticValue.ex = new is_pattern_expr(isTypeCheck.expr, deconstructorPattern, CurrentLocationSpan);
        }
        break;
      case 681: // pattern -> simple_or_template_type_reference, tkRoundOpen, 
                //            pattern_out_param_list, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 682: // pattern_optional_var -> simple_or_template_type_reference, tkRoundOpen, 
                //                         pattern_out_param_list_optional_var, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 683: // deconstruction_or_const_pattern -> simple_or_template_type_reference, 
                //                                    tkRoundOpen, 
                //                                    pattern_out_param_list_optional_var, 
                //                                    tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 684: // deconstruction_or_const_pattern -> const_pattern_expr_list
{
		    CurrentSemanticValue.stn = new const_pattern(ValueStack[ValueStack.Depth-1].ob as List<syntax_tree_node>, CurrentLocationSpan); 
		}
        break;
      case 685: // const_pattern_expr_list -> const_pattern_expression
{ 
			CurrentSemanticValue.ob = new List<syntax_tree_node>(); 
			(CurrentSemanticValue.ob as List<syntax_tree_node>).Add(ValueStack[ValueStack.Depth-1].stn);
		}
        break;
      case 686: // const_pattern_expr_list -> const_pattern_expr_list, tkComma, 
                //                            const_pattern_expression
{ 
			var list = ValueStack[ValueStack.Depth-3].ob as List<syntax_tree_node>;
            list.Add(ValueStack[ValueStack.Depth-1].stn);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 687: // const_pattern_expression -> literal_or_number
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 688: // const_pattern_expression -> simple_or_template_type_reference
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 689: // const_pattern_expression -> tkNil
{ 
			CurrentSemanticValue.stn = new nil_const();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 690: // const_pattern_expression -> sizeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 691: // const_pattern_expression -> typeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 692: // collection_pattern -> tkSquareOpen, collection_pattern_expr_list, tkSquareClose
{
			CurrentSemanticValue.stn = new collection_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 693: // collection_pattern_expr_list -> collection_pattern_list_item
{
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 694: // collection_pattern_expr_list -> collection_pattern_expr_list, tkComma, 
                //                                 collection_pattern_list_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 695: // collection_pattern_list_item -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 696: // collection_pattern_list_item -> collection_pattern_var_item
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 697: // collection_pattern_list_item -> tkUnderscore
{
			CurrentSemanticValue.stn = new collection_pattern_wild_card(CurrentLocationSpan);
		}
        break;
      case 698: // collection_pattern_list_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 699: // collection_pattern_list_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 700: // collection_pattern_list_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 701: // collection_pattern_list_item -> tkDotDot
{
			CurrentSemanticValue.stn = new collection_pattern_gap_parameter(CurrentLocationSpan);
		}
        break;
      case 702: // collection_pattern_var_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new collection_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 703: // tuple_pattern -> tkRoundOpen, tuple_pattern_item_list, tkRoundClose
{
			if ((ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>).Count>6) 
				parserTools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
			CurrentSemanticValue.stn = new tuple_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 704: // tuple_pattern_item -> tkUnderscore
{ 
			CurrentSemanticValue.stn = new tuple_pattern_wild_card(CurrentLocationSpan); 
		}
        break;
      case 705: // tuple_pattern_item -> literal_or_number
{ 
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 706: // tuple_pattern_item -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 707: // tuple_pattern_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new tuple_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 708: // tuple_pattern_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 709: // tuple_pattern_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 710: // tuple_pattern_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 711: // tuple_pattern_item_list -> tuple_pattern_item
{ 
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 712: // tuple_pattern_item_list -> tuple_pattern_item_list, tkComma, tuple_pattern_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 713: // pattern_out_param_list_optional_var -> pattern_out_param_optional_var
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 714: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkSemiColon, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 715: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkComma, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 716: // pattern_out_param_list -> pattern_out_param
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 717: // pattern_out_param_list -> pattern_out_param_list, tkSemiColon, 
                //                           pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 718: // pattern_out_param_list -> pattern_out_param_list, tkComma, pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 719: // pattern_out_param -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 720: // pattern_out_param -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 721: // pattern_out_param -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 722: // pattern_out_param -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 723: // pattern_out_param -> pattern
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 724: // pattern_out_param -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 725: // pattern_out_param -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 726: // pattern_out_param_optional_var -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 727: // pattern_out_param_optional_var -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 728: // pattern_out_param_optional_var -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 729: // pattern_out_param_optional_var -> identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, false, CurrentLocationSpan);
        }
        break;
      case 730: // pattern_out_param_optional_var -> identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, false, CurrentLocationSpan);
        }
        break;
      case 731: // pattern_out_param_optional_var -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 732: // pattern_out_param_optional_var -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 733: // pattern_out_param_optional_var -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 734: // pattern_out_param_optional_var -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 735: // pattern_out_param_optional_var -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 736: // simple_expr_or_nothing -> simple_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 737: // simple_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 738: // const_expr_or_nothing -> const_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 739: // const_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 740: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing
{
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 741: // format_expr -> tkColon, simple_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 742: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing, tkColon, 
                //                simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 743: // format_expr -> tkColon, simple_expr_or_nothing, tkColon, simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 744: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 745: // format_const_expr -> tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 746: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing, tkColon, 
                //                      const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 747: // format_const_expr -> tkColon, const_expr_or_nothing, tkColon, const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 748: // relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 749: // relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 750: // relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 751: // relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 752: // relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 753: // relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 754: // relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 755: // relop -> tkNot, tkIn
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
      case 756: // simple_expr -> term1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 757: // simple_expr -> simple_expr, tkDotDot, term1
{ 
		if (parserTools.buildTreeForFormatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
	}
        break;
      case 758: // term1 -> term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 759: // term1 -> term1, addop, term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 760: // term1 -> term1, addop, new_question_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 761: // addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 762: // addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 763: // addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 764: // addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 765: // addop -> tkCSharpStyleOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 766: // typecast_op -> tkAs
{ 
			CurrentSemanticValue.ob = op_typecast.as_op; 
		}
        break;
      case 767: // typecast_op -> tkIs
{ 
			CurrentSemanticValue.ob = op_typecast.is_op; 
		}
        break;
      case 768: // as_is_expr -> is_type_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 769: // as_is_expr -> as_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 770: // as_expr -> term, tkAs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 771: // as_expr -> term, tkAs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 772: // is_type_expr -> term, tkIs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 773: // is_type_expr -> term, tkIs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 774: // power_expr -> factor_without_unary_op, tkStarStar, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 775: // power_expr -> factor_without_unary_op, tkStarStar, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 776: // power_expr -> sign, power_expr
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 777: // term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 778: // term -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 779: // term -> power_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 780: // term -> term, mulop, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 781: // term -> term, mulop, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 782: // term -> term, mulop, new_question_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 783: // term -> as_is_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 784: // mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 785: // mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 786: // mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 787: // mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 788: // mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 789: // mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 790: // mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 791: // default_expr -> tkDefault, tkRoundOpen, 
                //                 simple_or_template_or_question_type_reference, tkRoundClose
{ 
			CurrentSemanticValue.ex = new default_operator(ValueStack[ValueStack.Depth-2].td as named_type_reference, CurrentLocationSpan);  
		}
        break;
      case 792: // tuple -> tkRoundOpen, expr_l1_or_unpacked, tkComma, expr_l1_or_unpacked_list, 
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
      case 793: // factor_without_unary_op -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 794: // factor_without_unary_op -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 795: // factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 796: // factor -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 797: // factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 798: // factor -> tkNot, factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 799: // factor -> sign, factor
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
      case 800: // factor -> tkDeref, factor
{
            CurrentSemanticValue.ex = new index(ValueStack[ValueStack.Depth-1].ex, true, CurrentLocationSpan);
        }
        break;
      case 801: // factor -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 802: // factor -> tuple
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 803: // literal_or_number -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 804: // literal_or_number -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 805: // var_question_point -> variable, tkQuestionPoint, variable
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 806: // var_question_point -> variable, tkQuestionPoint, var_question_point
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 807: // var_reference -> var_address, variable
{
			CurrentSemanticValue.ex = NewVarReference(ValueStack[ValueStack.Depth-2].stn as get_address, ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);
		}
        break;
      case 808: // var_reference -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 809: // var_reference -> var_question_point
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 810: // var_reference -> tkRoundOpen, tkVar, identifier, tkAssign, expr_dq, 
                //                  tkRoundClose
{ CurrentSemanticValue.ex = new let_var_expr(ValueStack[ValueStack.Depth-4].id,ValueStack[ValueStack.Depth-2].ex,CurrentLocationSpan); }
        break;
      case 811: // var_address -> tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(CurrentLocationSpan);
		}
        break;
      case 812: // var_address -> var_address, tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(ValueStack[ValueStack.Depth-2].stn as get_address, CurrentLocationSpan);
		}
        break;
      case 813: // attribute_variable -> simple_type_identifier, optional_expr_list_with_bracket
{ 
			CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
		}
        break;
      case 814: // attribute_variable -> template_type, optional_expr_list_with_bracket
{
            CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 815: // dotted_identifier -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 816: // dotted_identifier -> dotted_identifier, tkPoint, identifier_or_keyword
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
		}
        break;
      case 817: // variable_as_type -> dotted_identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;}
        break;
      case 818: // variable_as_type -> dotted_identifier, template_type_params
{ CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-2].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);   }
        break;
      case 819: // var_with_init_for_expr_with_let -> tkVar, identifier, tkAssign, expr, 
                //                                    tkSemiColon
{
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-4].id as addressed_value, ValueStack[ValueStack.Depth-2].ex, Operators.Assignment, CurrentLocationSpan);
		}
        break;
      case 820: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 821: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let_list, 
                //                                         var_with_init_for_expr_with_let
{
			ValueStack[ValueStack.Depth-2].stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
		}
        break;
      case 822: // proc_func_call -> variable, tkRoundOpen, optional_expr_list_func_param, 
                //                   tkRoundClose
{
			if (ValueStack[ValueStack.Depth-4].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-4], "^");
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 823: // variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 824: // variable -> operator_name_ident
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 825: // variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 826: // variable -> tkRoundOpen, expr, tkRoundClose
{
		    if (!parserTools.buildTreeForFormatter) 
            {
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
            } 
			else CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
        }
        break;
      case 827: // variable -> tkRoundOpen, var_with_init_for_expr_with_let_list, expr, 
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
      case 828: // variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 829: // variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 830: // variable -> literal_or_number, tkPoint, identifier_or_keyword
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");		
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 831: // variable -> variable, tkSquareOpen, expr_list, tkSquareClose
{
        	CurrentSemanticValue.ex = NewIndexerOrSlice(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list,CurrentLocationSpan);
        }
        break;
      case 832: // variable -> literal_or_number, tkSquareOpen, expr_list, tkSquareClose
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
      case 833: // variable -> variable, tkQuestionSquareOpen, format_expr, tkSquareClose
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
      case 834: // variable -> literal_or_number, tkQuestionSquareOpen, format_expr, tkSquareClose
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
      case 835: // variable -> tkVertParen, elem_list, tkVertParen
{ 
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, '|', CurrentLocationSpan);  
		}
        break;
      case 836: // variable -> pascal_set_const
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
		}
        break;
      case 837: // variable -> proc_func_call
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 838: // variable -> variable, tkPoint, identifier_keyword_operatorname
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 839: // variable -> tuple, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 840: // variable -> variable, tkDeref
{
			CurrentSemanticValue.ex = new roof_dereference(ValueStack[ValueStack.Depth-2].ex as addressed_value,CurrentLocationSpan);
        }
        break;
      case 841: // variable -> variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 842: // optional_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 843: // optional_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 844: // optional_expr_list_func_param -> expr_list_func_param
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 845: // optional_expr_list_func_param -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 846: // elem_list -> elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 847: // elem_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 848: // elem_list1 -> elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 849: // elem_list1 -> elem_list1, tkComma, elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 850: // elem -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 851: // elem -> expr, tkDotDot, expr
{ CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 852: // one_literal -> tkStringLiteral
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 853: // one_literal -> tkAsciiChar
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 854: // literal -> literal_list
{ 
			CurrentSemanticValue.ex = NewLiteral(ValueStack[ValueStack.Depth-1].stn as literal_const_line);
        }
        break;
      case 855: // literal -> tkFormatStringLiteral
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
      case 856: // literal -> tkMultilineStringLiteral
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
      case 857: // literal_list -> one_literal
{ 
			CurrentSemanticValue.stn = new literal_const_line(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 858: // literal_list -> literal_list, one_literal
{ 
        	var line = ValueStack[ValueStack.Depth-2].stn as literal_const_line;
            if (line.literals.Last() is string_const && ValueStack[ValueStack.Depth-1].ex is string_const)
            	parserTools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.stn = line.Add(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 859: // operator_name_ident -> tkOperator, overload_operator
{ 
			CurrentSemanticValue.ex = new operator_name_ident((ValueStack[ValueStack.Depth-1].op as op_type_node).text, (ValueStack[ValueStack.Depth-1].op as op_type_node).type, CurrentLocationSpan);
		}
        break;
      case 860: // optional_method_modificators -> tkSemiColon
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 861: // optional_method_modificators -> tkSemiColon, meth_modificators, tkSemiColon
{ 
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 862: // optional_method_modificators1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 863: // optional_method_modificators1 -> tkSemiColon, meth_modificators
{ 
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 864: // meth_modificators -> meth_modificator
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan); 
		}
        break;
      case 865: // meth_modificators -> meth_modificators, tkSemiColon, meth_modificator
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as procedure_attributes_list).Add(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan);  
		}
        break;
      case 866: // identifier -> tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 867: // identifier -> tkExport
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 868: // identifier -> property_specifier_directives
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 869: // identifier -> non_reserved
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 870: // identifier -> tkStep
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 871: // identifier -> tkIndex
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 872: // identifier_or_keyword -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 873: // identifier_or_keyword -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 874: // identifier_or_keyword -> reserved_keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 875: // identifier_keyword_operatorname -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 876: // identifier_keyword_operatorname -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 877: // identifier_keyword_operatorname -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 878: // meth_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 879: // meth_modificator -> tkOverload
{ 
            CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id;
            parserTools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", ValueStack[ValueStack.Depth-1].id.source_context);
        }
        break;
      case 880: // meth_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 881: // meth_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 882: // meth_modificator -> tkExtensionMethod
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 883: // meth_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 884: // property_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 885: // property_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 886: // property_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 887: // property_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 888: // property_specifier_directives -> tkRead
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 889: // property_specifier_directives -> tkWrite
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 890: // non_reserved -> tkName
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 891: // non_reserved -> tkNew
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 892: // visibility_specifier -> tkInternal
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 893: // visibility_specifier -> tkPublic
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 894: // visibility_specifier -> tkProtected
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 895: // visibility_specifier -> tkPrivate
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 896: // keyword -> visibility_specifier
{ 
			CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  
		}
        break;
      case 897: // keyword -> tkSealed
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 898: // keyword -> tkTemplate
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 899: // keyword -> tkOr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 900: // keyword -> tkTypeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 901: // keyword -> tkSizeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 902: // keyword -> tkDefault
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 903: // keyword -> tkWhere
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 904: // keyword -> tkXor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 905: // keyword -> tkAnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 906: // keyword -> tkDiv
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 907: // keyword -> tkMod
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 908: // keyword -> tkShl
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 909: // keyword -> tkShr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 910: // keyword -> tkNot
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 911: // keyword -> tkAs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 912: // keyword -> tkIn
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 913: // keyword -> tkIs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 914: // keyword -> tkArray
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 915: // keyword -> tkSequence
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 916: // keyword -> tkBegin
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 917: // keyword -> tkCase
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 918: // keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 919: // keyword -> tkConst
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 920: // keyword -> tkConstructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 921: // keyword -> tkDestructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 922: // keyword -> tkDownto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 923: // keyword -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 924: // keyword -> tkElse
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 925: // keyword -> tkEnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 926: // keyword -> tkExcept
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 927: // keyword -> tkFile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 928: // keyword -> tkAuto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 929: // keyword -> tkFinalization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 930: // keyword -> tkFinally
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 931: // keyword -> tkFor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 932: // keyword -> tkForeach
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 933: // keyword -> tkFunction
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 934: // keyword -> tkIf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 935: // keyword -> tkImplementation
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 936: // keyword -> tkInherited
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 937: // keyword -> tkInitialization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 938: // keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 939: // keyword -> tkProcedure
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 940: // keyword -> tkProperty
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 941: // keyword -> tkRaise
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 942: // keyword -> tkRecord
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 943: // keyword -> tkRepeat
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 944: // keyword -> tkSet
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 945: // keyword -> tkTry
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 946: // keyword -> tkType
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 947: // keyword -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 948: // keyword -> tkThen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 949: // keyword -> tkTo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 950: // keyword -> tkUntil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 951: // keyword -> tkUses
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 952: // keyword -> tkVar
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 953: // keyword -> tkWhile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 954: // keyword -> tkWith
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 955: // keyword -> tkNil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 956: // keyword -> tkGoto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 957: // keyword -> tkOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 958: // keyword -> tkLabel
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 959: // keyword -> tkProgram
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 960: // keyword -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 961: // keyword -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 962: // keyword -> tkNamespace
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 963: // keyword -> tkExternal
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 964: // keyword -> tkParams
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 965: // keyword -> tkEvent
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 966: // keyword -> tkYield
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 967: // keyword -> tkMatch
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 968: // keyword -> tkWhen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 969: // keyword -> tkPartial
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 970: // keyword -> tkAbstract
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 971: // keyword -> tkLock
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 972: // keyword -> tkImplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 973: // keyword -> tkExplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 974: // keyword -> tkOn
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 975: // keyword -> tkVirtual
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 976: // keyword -> tkOverride
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 977: // keyword -> tkLoop
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 978: // keyword -> tkExtensionMethod
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 979: // keyword -> tkOverload
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 980: // keyword -> tkReintroduce
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 981: // keyword -> tkForward
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 982: // reserved_keyword -> tkOperator
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 983: // overload_operator -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 984: // overload_operator -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 985: // overload_operator -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 986: // overload_operator -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 987: // overload_operator -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 988: // overload_operator -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 989: // overload_operator -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 990: // overload_operator -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 991: // overload_operator -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 992: // overload_operator -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 993: // overload_operator -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 994: // overload_operator -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 995: // overload_operator -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 996: // overload_operator -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 997: // overload_operator -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 998: // overload_operator -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 999: // overload_operator -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1000: // overload_operator -> tkNot
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1001: // overload_operator -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1002: // overload_operator -> tkImplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1003: // overload_operator -> tkExplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1004: // overload_operator -> assign_operator
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1005: // overload_operator -> tkStarStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1006: // assign_operator -> tkAssign
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1007: // assign_operator -> tkPlusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1008: // assign_operator -> tkMinusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1009: // assign_operator -> tkMultEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1010: // assign_operator -> tkDivEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 1011: // lambda_unpacked_params -> tkBackSlashRoundOpen, 
                 //                           lambda_list_of_unpacked_params_or_id, tkComma, 
                 //                           lambda_unpacked_params_or_id, tkRoundClose
{
			// �?ез�?л�?�?а�? надо п�?исвои�?�? каком�? �?о са�?а�?ном�? пол�? в function_lambda_definition
			(ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-2].ob as ident_or_list);
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list;
		}
        break;
      case 1012: // lambda_unpacked_params_or_id -> lambda_unpacked_params
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].ex as unpacked_list_of_ident_or_list);
		}
        break;
      case 1013: // lambda_unpacked_params_or_id -> identifier
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].id as ident);
		}
        break;
      case 1014: // lambda_list_of_unpacked_params_or_id -> lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = new unpacked_list_of_ident_or_list();
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LocationStack[LocationStack.Depth-1];
		}
        break;
      case 1015: // lambda_list_of_unpacked_params_or_id -> lambda_list_of_unpacked_params_or_id, 
                 //                                         tkComma, lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 1016: // expr_l1_or_unpacked -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 1017: // expr_l1_or_unpacked -> lambda_unpacked_params
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 1018: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 1019: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked_list, tkComma, 
                 //                             expr_l1_or_unpacked
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 1020: // func_decl_lambda -> identifier, tkArrow, lambda_function_body
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
      case 1021: // func_decl_lambda -> tkRoundOpen, tkRoundClose, lambda_type_ref_noproctype, 
                 //                     tkArrow, lambda_function_body
{
		    // �?дес�? надо анализи�?ова�?�? по �?ел�? и либо ос�?авля�?�? lambda_inferred_type, либо дела�?�? его null!
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, sl, CurrentLocationSpan);	
		}
        break;
      case 1022: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkRoundClose, 
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
      case 1023: // func_decl_lambda -> tkRoundOpen, identifier, tkSemiColon, full_lambda_fp_list, 
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
      case 1024: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkSemiColon, 
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
      case 1025: // func_decl_lambda -> tkRoundOpen, expr_l1_or_unpacked, tkComma, 
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
      case 1026: // func_decl_lambda -> lambda_unpacked_params, rem_lambda
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
      case 1027: // func_decl_lambda -> expl_func_decl_lambda
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 1028: // optional_full_lambda_fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 1029: // optional_full_lambda_fp_list -> tkSemiColon, full_lambda_fp_list
{
		CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
	}
        break;
      case 1030: // rem_lambda -> lambda_type_ref_noproctype, tkArrow, lambda_function_body
{ 
		    CurrentSemanticValue.ob = new pair_type_stlist(ValueStack[ValueStack.Depth-3].td,ValueStack[ValueStack.Depth-1].stn as statement_list);
		}
        break;
      case 1031: // expl_func_decl_lambda -> tkFunction, lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1032: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, tkRoundClose, 
                 //                          lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1033: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, full_lambda_fp_list, 
                 //                          tkRoundClose, lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1034: // expl_func_decl_lambda -> tkProcedure, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1035: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, tkRoundClose, tkArrow, 
                 //                          lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1036: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, full_lambda_fp_list, 
                 //                          tkRoundClose, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-4].stn as formal_parameters, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1037: // full_lambda_fp_list -> lambda_simple_fp_sect
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
      case 1038: // full_lambda_fp_list -> full_lambda_fp_list, tkSemiColon, lambda_simple_fp_sect
{
			CurrentSemanticValue.stn =(ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
		}
        break;
      case 1039: // lambda_simple_fp_sect -> ident_list, lambda_type_ref
{
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-2].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan);
		}
        break;
      case 1040: // lambda_type_ref -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
		}
        break;
      case 1041: // lambda_type_ref -> tkColon, fptype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 1042: // lambda_type_ref_noproctype -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
		}
        break;
      case 1043: // lambda_type_ref_noproctype -> tkColon, fptype_noproctype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 1044: // common_lambda_body -> compound_stmt
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 1045: // common_lambda_body -> if_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1046: // common_lambda_body -> while_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1047: // common_lambda_body -> repeat_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1048: // common_lambda_body -> for_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1049: // common_lambda_body -> foreach_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1050: // common_lambda_body -> loop_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1051: // common_lambda_body -> case_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1052: // common_lambda_body -> try_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1053: // common_lambda_body -> lock_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1054: // common_lambda_body -> raise_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1055: // common_lambda_body -> yield_stmt
{
			parserTools.AddErrorFromResource("YIELD_STATEMENT_CANNOT_BE_USED_IN_LAMBDA_BODY", CurrentLocationSpan);
		}
        break;
      case 1056: // common_lambda_body -> tkRoundOpen, assignment, tkRoundClose
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-2]);
		}
        break;
      case 1057: // lambda_function_body -> expr_l1_for_lambda
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
      case 1058: // lambda_function_body -> common_lambda_body
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 1059: // lambda_procedure_body -> proc_call
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1060: // lambda_procedure_body -> assignment
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1061: // lambda_procedure_body -> common_lambda_body
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
