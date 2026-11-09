// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  DESKTOP-L2INHKG
// DateTime: 09.11.2026 10:31:00
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
  private static Rule[] rules = new Rule[1037];
  private static State[] states = new State[1721];
  private static string[] nonTerms = new string[] {
      "parse_goal", "unit_reexport_clauses", "unit_reexport_list", "unit_reexport_item", 
      "unit_key_word", "class_or_static", "assignment", "optional_array_initializer", 
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
      "inclass_proc_func_decl_without_modifiers", "constr_destr_decl", "inclass_constr_destr_decl", 
      "method_decl", "proc_func_constr_destr_decl_with_attr", "proc_func_decl_without_modifiers", 
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
    states[0] = new State(new int[]{61,1608,105,1687,106,1688,109,1689,88,1694,90,1699,89,1706,75,1711,77,1717,3,-27,52,-27,91,-27,59,-27,29,-27,66,-27,50,-27,53,-27,62,-27,11,-27,44,-27,37,-27,28,-27,26,-27,19,-27,30,-27,31,-27},new int[]{-1,1,-237,3,-238,4,-310,1620,-312,1621,-5,1682,-178,1693});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{3,1604,52,-14,91,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-188,5,-189,1602,-187,1607});
    states[5] = new State(-41,new int[]{-306,6});
    states[6] = new State(new int[]{52,1590,59,-73,29,-73,66,-73,50,-73,53,-73,62,-73,11,-73,44,-73,37,-73,28,-73,26,-73,19,-73,30,-73,31,-73,91,-73},new int[]{-20,7,-308,14,-39,15,-43,1525,-44,1526});
    states[7] = new State(new int[]{7,9,10,10,5,11,100,12,6,13,2,-26},new int[]{-191,8});
    states[8] = new State(-20);
    states[9] = new State(-21);
    states[10] = new State(-22);
    states[11] = new State(-23);
    states[12] = new State(-24);
    states[13] = new State(-25);
    states[14] = new State(-42);
    states[15] = new State(new int[]{91,17},new int[]{-259,16});
    states[16] = new State(-34);
    states[17] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499},new int[]{-256,18,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[18] = new State(new int[]{92,19,10,20});
    states[19] = new State(-535);
    states[20] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499},new int[]{-265,21,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[21] = new State(-537);
    states[22] = new State(-497);
    states[23] = new State(-500);
    states[24] = new State(new int[]{110,430,111,431,112,432,113,433,114,434,92,-533,10,-533,98,-533,101,-533,33,-533,104,-533,2,-533,9,-533,12,-533,100,-533,99,-533,32,-533,85,-533,84,-533,83,-533,82,-533,81,-533,86,-533},new int[]{-197,25});
    states[25] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,834,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683},new int[]{-91,26,-90,27,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,835,-103,660,-327,678});
    states[26] = new State(-527);
    states[27] = new State(new int[]{70,28,92,-603,10,-603,98,-603,101,-603,33,-603,104,-603,2,-603,9,-603,12,-603,100,-603,99,-603,32,-603,85,-603,84,-603,83,-603,82,-603,81,-603,86,-603});
    states[28] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,29,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[29] = new State(-612);
    states[30] = new State(new int[]{16,31,70,-613,92,-613,10,-613,98,-613,101,-613,33,-613,104,-613,2,-613,9,-613,12,-613,100,-613,99,-613,32,-613,85,-613,84,-613,83,-613,82,-613,81,-613,86,-613,6,-613,76,-613,5,-613,51,-613,58,-613,141,-613,143,-613,161,-613,80,-613,78,-613,160,-613,87,-613,45,-613,42,-613,8,-613,21,-613,22,-613,144,-613,147,-613,145,-613,146,-613,155,-613,158,-613,157,-613,156,-613,11,-613,57,-613,91,-613,40,-613,25,-613,97,-613,54,-613,35,-613,55,-613,102,-613,47,-613,36,-613,53,-613,60,-613,74,-613,72,-613,38,-613,71,-613,13,-616});
    states[31] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369},new int[]{-101,32,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579});
    states[32] = new State(new int[]{120,158,125,159,123,160,121,161,124,162,122,163,137,164,135,165,16,-626,70,-626,92,-626,10,-626,98,-626,101,-626,33,-626,104,-626,2,-626,9,-626,12,-626,100,-626,99,-626,32,-626,85,-626,84,-626,83,-626,82,-626,81,-626,86,-626,13,-626,6,-626,76,-626,5,-626,51,-626,58,-626,141,-626,143,-626,161,-626,80,-626,78,-626,160,-626,87,-626,45,-626,42,-626,8,-626,21,-626,22,-626,144,-626,147,-626,145,-626,146,-626,155,-626,158,-626,157,-626,156,-626,11,-626,57,-626,91,-626,40,-626,25,-626,97,-626,54,-626,35,-626,55,-626,102,-626,47,-626,36,-626,53,-626,60,-626,74,-626,72,-626,38,-626,71,-626,116,-626,115,-626,128,-626,129,-626,126,-626,138,-626,136,-626,118,-626,117,-626,131,-626,132,-626,133,-626,134,-626,130,-626},new int[]{-199,33});
    states[33] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-108,34,-246,1524,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,603,-271,579});
    states[34] = new State(new int[]{6,35,120,-653,125,-653,123,-653,121,-653,124,-653,122,-653,137,-653,135,-653,16,-653,70,-653,92,-653,10,-653,98,-653,101,-653,33,-653,104,-653,2,-653,9,-653,12,-653,100,-653,99,-653,32,-653,85,-653,84,-653,83,-653,82,-653,81,-653,86,-653,13,-653,76,-653,5,-653,51,-653,58,-653,141,-653,143,-653,161,-653,80,-653,78,-653,160,-653,87,-653,45,-653,42,-653,8,-653,21,-653,22,-653,144,-653,147,-653,145,-653,146,-653,155,-653,158,-653,157,-653,156,-653,11,-653,57,-653,91,-653,40,-653,25,-653,97,-653,54,-653,35,-653,55,-653,102,-653,47,-653,36,-653,53,-653,60,-653,74,-653,72,-653,38,-653,71,-653,116,-653,115,-653,128,-653,129,-653,126,-653,138,-653,136,-653,118,-653,117,-653,131,-653,132,-653,133,-653,134,-653,130,-653});
    states[35] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369},new int[]{-86,36,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,603,-271,579});
    states[36] = new State(new int[]{116,173,115,174,128,175,129,176,126,177,6,-732,5,-732,120,-732,125,-732,123,-732,121,-732,124,-732,122,-732,137,-732,135,-732,16,-732,70,-732,92,-732,10,-732,98,-732,101,-732,33,-732,104,-732,2,-732,9,-732,12,-732,100,-732,99,-732,32,-732,85,-732,84,-732,83,-732,82,-732,81,-732,86,-732,13,-732,76,-732,51,-732,58,-732,141,-732,143,-732,161,-732,80,-732,78,-732,160,-732,87,-732,45,-732,42,-732,8,-732,21,-732,22,-732,144,-732,147,-732,145,-732,146,-732,155,-732,158,-732,157,-732,156,-732,11,-732,57,-732,91,-732,40,-732,25,-732,97,-732,54,-732,35,-732,55,-732,102,-732,47,-732,36,-732,53,-732,60,-732,74,-732,72,-732,38,-732,71,-732,138,-732,136,-732,118,-732,117,-732,131,-732,132,-732,133,-732,134,-732,130,-732},new int[]{-200,37});
    states[37] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-85,38,-246,1523,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,603,-271,579});
    states[38] = new State(new int[]{138,179,136,1513,118,1516,117,1517,131,1518,132,1519,133,1520,134,1521,130,1522,116,-734,115,-734,128,-734,129,-734,126,-734,6,-734,5,-734,120,-734,125,-734,123,-734,121,-734,124,-734,122,-734,137,-734,135,-734,16,-734,70,-734,92,-734,10,-734,98,-734,101,-734,33,-734,104,-734,2,-734,9,-734,12,-734,100,-734,99,-734,32,-734,85,-734,84,-734,83,-734,82,-734,81,-734,86,-734,13,-734,76,-734,51,-734,58,-734,141,-734,143,-734,161,-734,80,-734,78,-734,160,-734,87,-734,45,-734,42,-734,8,-734,21,-734,22,-734,144,-734,147,-734,145,-734,146,-734,155,-734,158,-734,157,-734,156,-734,11,-734,57,-734,91,-734,40,-734,25,-734,97,-734,54,-734,35,-734,55,-734,102,-734,47,-734,36,-734,53,-734,60,-734,74,-734,72,-734,38,-734,71,-734},new int[]{-201,39});
    states[39] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-99,40,-272,41,-246,42,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-100,462});
    states[40] = new State(-755);
    states[41] = new State(-756);
    states[42] = new State(-757);
    states[43] = new State(-770);
    states[44] = new State(new int[]{7,45,11,151,17,381,138,-771,136,-771,118,-771,117,-771,131,-771,132,-771,133,-771,134,-771,130,-771,116,-771,115,-771,128,-771,129,-771,126,-771,6,-771,5,-771,120,-771,125,-771,123,-771,121,-771,124,-771,122,-771,137,-771,135,-771,16,-771,70,-771,92,-771,10,-771,98,-771,101,-771,33,-771,104,-771,2,-771,9,-771,12,-771,100,-771,99,-771,32,-771,85,-771,84,-771,83,-771,82,-771,81,-771,86,-771,13,-771,76,-771,51,-771,58,-771,141,-771,143,-771,161,-771,80,-771,78,-771,160,-771,87,-771,45,-771,42,-771,8,-771,21,-771,22,-771,144,-771,147,-771,145,-771,146,-771,155,-771,158,-771,157,-771,156,-771,57,-771,91,-771,40,-771,25,-771,97,-771,54,-771,35,-771,55,-771,102,-771,47,-771,36,-771,53,-771,60,-771,74,-771,72,-771,38,-771,71,-771,119,-768});
    states[45] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,84,60,83,61,82,62,81,63,68,64,64,65,128,66,22,67,21,68,63,69,23,70,129,71,130,72,131,73,132,74,133,75,134,76,135,77,136,78,137,79,138,80,24,81,73,82,91,83,25,84,26,85,29,86,30,87,31,88,71,89,99,90,32,91,92,92,33,93,34,94,27,95,104,96,101,97,35,98,36,99,37,100,40,101,41,102,42,103,103,104,43,105,44,106,46,107,47,108,48,109,97,110,49,111,102,112,50,113,28,114,51,115,70,116,98,117,52,118,53,119,54,120,55,121,56,122,57,123,58,124,59,125,61,126,105,127,106,128,109,129,107,130,108,131,62,132,74,133,38,134,39,135,69,136,148,137,60,138,139,139,140,140,79,141,153,142,152,143,72,144,154,145,150,146,151,147,149,148,45,150},new int[]{-141,46,-150,47,-154,50,-155,53,-296,58,-153,59,-297,149});
    states[46] = new State(-805);
    states[47] = new State(-847);
    states[48] = new State(-841);
    states[49] = new State(-842);
    states[50] = new State(-843);
    states[51] = new State(-863);
    states[52] = new State(-864);
    states[53] = new State(-844);
    states[54] = new State(-865);
    states[55] = new State(-866);
    states[56] = new State(-845);
    states[57] = new State(-846);
    states[58] = new State(-848);
    states[59] = new State(-871);
    states[60] = new State(-867);
    states[61] = new State(-868);
    states[62] = new State(-869);
    states[63] = new State(-870);
    states[64] = new State(-872);
    states[65] = new State(-873);
    states[66] = new State(-874);
    states[67] = new State(-875);
    states[68] = new State(-876);
    states[69] = new State(-877);
    states[70] = new State(-878);
    states[71] = new State(-879);
    states[72] = new State(-880);
    states[73] = new State(-881);
    states[74] = new State(-882);
    states[75] = new State(-883);
    states[76] = new State(-884);
    states[77] = new State(-885);
    states[78] = new State(-886);
    states[79] = new State(-887);
    states[80] = new State(-888);
    states[81] = new State(-889);
    states[82] = new State(-890);
    states[83] = new State(-891);
    states[84] = new State(-892);
    states[85] = new State(-893);
    states[86] = new State(-894);
    states[87] = new State(-895);
    states[88] = new State(-896);
    states[89] = new State(-897);
    states[90] = new State(-898);
    states[91] = new State(-899);
    states[92] = new State(-900);
    states[93] = new State(-901);
    states[94] = new State(-902);
    states[95] = new State(-903);
    states[96] = new State(-904);
    states[97] = new State(-905);
    states[98] = new State(-906);
    states[99] = new State(-907);
    states[100] = new State(-908);
    states[101] = new State(-909);
    states[102] = new State(-910);
    states[103] = new State(-911);
    states[104] = new State(-912);
    states[105] = new State(-913);
    states[106] = new State(-914);
    states[107] = new State(-915);
    states[108] = new State(-916);
    states[109] = new State(-917);
    states[110] = new State(-918);
    states[111] = new State(-919);
    states[112] = new State(-920);
    states[113] = new State(-921);
    states[114] = new State(-922);
    states[115] = new State(-923);
    states[116] = new State(-924);
    states[117] = new State(-925);
    states[118] = new State(-926);
    states[119] = new State(-927);
    states[120] = new State(-928);
    states[121] = new State(-929);
    states[122] = new State(-930);
    states[123] = new State(-931);
    states[124] = new State(-932);
    states[125] = new State(-933);
    states[126] = new State(-934);
    states[127] = new State(-935);
    states[128] = new State(-936);
    states[129] = new State(-937);
    states[130] = new State(-938);
    states[131] = new State(-939);
    states[132] = new State(-940);
    states[133] = new State(-941);
    states[134] = new State(-942);
    states[135] = new State(-943);
    states[136] = new State(-944);
    states[137] = new State(-945);
    states[138] = new State(-946);
    states[139] = new State(-947);
    states[140] = new State(-948);
    states[141] = new State(-949);
    states[142] = new State(-950);
    states[143] = new State(-951);
    states[144] = new State(-952);
    states[145] = new State(-953);
    states[146] = new State(-954);
    states[147] = new State(-955);
    states[148] = new State(-956);
    states[149] = new State(-849);
    states[150] = new State(-957);
    states[151] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,834,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683},new int[]{-74,152,-91,397,-90,27,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,835,-103,660,-327,678});
    states[152] = new State(new int[]{12,153,100,154});
    states[153] = new State(-807);
    states[154] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,834,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683},new int[]{-91,155,-90,27,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,835,-103,660,-327,678});
    states[155] = new State(-598);
    states[156] = new State(-610);
    states[157] = new State(new int[]{120,158,125,159,123,160,121,161,124,162,122,163,137,164,135,165,16,-625,70,-625,92,-625,10,-625,98,-625,101,-625,33,-625,104,-625,2,-625,9,-625,12,-625,100,-625,99,-625,32,-625,85,-625,84,-625,83,-625,82,-625,81,-625,86,-625,13,-625,6,-625,76,-625,5,-625,51,-625,58,-625,141,-625,143,-625,161,-625,80,-625,78,-625,160,-625,87,-625,45,-625,42,-625,8,-625,21,-625,22,-625,144,-625,147,-625,145,-625,146,-625,155,-625,158,-625,157,-625,156,-625,11,-625,57,-625,91,-625,40,-625,25,-625,97,-625,54,-625,35,-625,55,-625,102,-625,47,-625,36,-625,53,-625,60,-625,74,-625,72,-625,38,-625,71,-625,116,-625,115,-625,128,-625,129,-625,126,-625,138,-625,136,-625,118,-625,117,-625,131,-625,132,-625,133,-625,134,-625,130,-625},new int[]{-199,33});
    states[158] = new State(-723);
    states[159] = new State(-724);
    states[160] = new State(-725);
    states[161] = new State(-726);
    states[162] = new State(-727);
    states[163] = new State(-728);
    states[164] = new State(-729);
    states[165] = new State(new int[]{137,166});
    states[166] = new State(-730);
    states[167] = new State(new int[]{6,35,5,168,120,-652,125,-652,123,-652,121,-652,124,-652,122,-652,137,-652,135,-652,16,-652,70,-652,92,-652,10,-652,98,-652,101,-652,33,-652,104,-652,2,-652,9,-652,12,-652,100,-652,99,-652,32,-652,85,-652,84,-652,83,-652,82,-652,81,-652,86,-652,13,-652,76,-652});
    states[168] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,5,-712,70,-712,92,-712,10,-712,98,-712,101,-712,33,-712,104,-712,2,-712,9,-712,12,-712,100,-712,99,-712,32,-712,84,-712,83,-712,82,-712,81,-712,6,-712},new int[]{-118,169,-108,604,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,603,-271,579});
    states[169] = new State(new int[]{5,170,70,-715,92,-715,10,-715,98,-715,101,-715,33,-715,104,-715,2,-715,9,-715,12,-715,100,-715,99,-715,32,-715,85,-715,84,-715,83,-715,82,-715,81,-715,86,-715,6,-715,76,-715});
    states[170] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369},new int[]{-108,171,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,603,-271,579});
    states[171] = new State(new int[]{6,35,70,-717,92,-717,10,-717,98,-717,101,-717,33,-717,104,-717,2,-717,9,-717,12,-717,100,-717,99,-717,32,-717,85,-717,84,-717,83,-717,82,-717,81,-717,86,-717,76,-717});
    states[172] = new State(new int[]{116,173,115,174,128,175,129,176,126,177,6,-731,5,-731,120,-731,125,-731,123,-731,121,-731,124,-731,122,-731,137,-731,135,-731,16,-731,70,-731,92,-731,10,-731,98,-731,101,-731,33,-731,104,-731,2,-731,9,-731,12,-731,100,-731,99,-731,32,-731,85,-731,84,-731,83,-731,82,-731,81,-731,86,-731,13,-731,76,-731,51,-731,58,-731,141,-731,143,-731,161,-731,80,-731,78,-731,160,-731,87,-731,45,-731,42,-731,8,-731,21,-731,22,-731,144,-731,147,-731,145,-731,146,-731,155,-731,158,-731,157,-731,156,-731,11,-731,57,-731,91,-731,40,-731,25,-731,97,-731,54,-731,35,-731,55,-731,102,-731,47,-731,36,-731,53,-731,60,-731,74,-731,72,-731,38,-731,71,-731,138,-731,136,-731,118,-731,117,-731,131,-731,132,-731,133,-731,134,-731,130,-731},new int[]{-200,37});
    states[173] = new State(-736);
    states[174] = new State(-737);
    states[175] = new State(-738);
    states[176] = new State(-739);
    states[177] = new State(-740);
    states[178] = new State(new int[]{138,179,136,1513,118,1516,117,1517,131,1518,132,1519,133,1520,134,1521,130,1522,116,-733,115,-733,128,-733,129,-733,126,-733,6,-733,5,-733,120,-733,125,-733,123,-733,121,-733,124,-733,122,-733,137,-733,135,-733,16,-733,70,-733,92,-733,10,-733,98,-733,101,-733,33,-733,104,-733,2,-733,9,-733,12,-733,100,-733,99,-733,32,-733,85,-733,84,-733,83,-733,82,-733,81,-733,86,-733,13,-733,76,-733,51,-733,58,-733,141,-733,143,-733,161,-733,80,-733,78,-733,160,-733,87,-733,45,-733,42,-733,8,-733,21,-733,22,-733,144,-733,147,-733,145,-733,146,-733,155,-733,158,-733,157,-733,156,-733,11,-733,57,-733,91,-733,40,-733,25,-733,97,-733,54,-733,35,-733,55,-733,102,-733,47,-733,36,-733,53,-733,60,-733,74,-733,72,-733,38,-733,71,-733},new int[]{-201,39});
    states[179] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,24,495},new int[]{-287,180,-282,181,-183,182,-150,219,-154,50,-155,53,-274,501});
    states[180] = new State(-747);
    states[181] = new State(-748);
    states[182] = new State(new int[]{7,183,4,186,123,188,8,-634,138,-634,136,-634,118,-634,117,-634,131,-634,132,-634,133,-634,134,-634,130,-634,116,-634,115,-634,128,-634,129,-634,126,-634,6,-634,5,-634,120,-634,125,-634,121,-634,124,-634,122,-634,137,-634,135,-634,16,-634,70,-634,92,-634,10,-634,98,-634,101,-634,33,-634,104,-634,2,-634,9,-634,12,-634,100,-634,99,-634,32,-634,85,-634,84,-634,83,-634,82,-634,81,-634,86,-634,13,-634,76,-634,51,-634,58,-634,141,-634,143,-634,161,-634,80,-634,78,-634,160,-634,87,-634,45,-634,42,-634,21,-634,22,-634,144,-634,147,-634,145,-634,146,-634,155,-634,158,-634,157,-634,156,-634,11,-634,57,-634,91,-634,40,-634,25,-634,97,-634,54,-634,35,-634,55,-634,102,-634,47,-634,36,-634,53,-634,60,-634,74,-634,72,-634,38,-634,71,-634},new int[]{-302,185});
    states[183] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,84,60,83,61,82,62,81,63,68,64,64,65,128,66,22,67,21,68,63,69,23,70,129,71,130,72,131,73,132,74,133,75,134,76,135,77,136,78,137,79,138,80,24,81,73,82,91,83,25,84,26,85,29,86,30,87,31,88,71,89,99,90,32,91,92,92,33,93,34,94,27,95,104,96,101,97,35,98,36,99,37,100,40,101,41,102,42,103,103,104,43,105,44,106,46,107,47,108,48,109,97,110,49,111,102,112,50,113,28,114,51,115,70,116,98,117,52,118,53,119,54,120,55,121,56,122,57,123,58,124,59,125,61,126,105,127,106,128,109,129,107,130,108,131,62,132,74,133,38,134,39,135,69,136,148,137,60,138,139,139,140,140,79,141,153,142,152,143,72,144,154,145,150,146,151,147,149,148,45,150},new int[]{-141,184,-150,47,-154,50,-155,53,-296,58,-153,59,-297,149});
    states[184] = new State(-268);
    states[185] = new State(-635);
    states[186] = new State(new int[]{123,188},new int[]{-302,187});
    states[187] = new State(-636);
    states[188] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-300,189,-283,314,-276,193,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-285,1471,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,1472,-227,562,-226,563,-304,1473});
    states[189] = new State(new int[]{121,190,100,191});
    states[190] = new State(-242);
    states[191] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-283,192,-276,193,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-285,1471,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,1472,-227,562,-226,563,-304,1473});
    states[192] = new State(-246);
    states[193] = new State(new int[]{13,194,121,-250,100,-250,120,-250,9,-250,10,-250,8,-250,138,-250,136,-250,118,-250,117,-250,131,-250,132,-250,133,-250,134,-250,130,-250,116,-250,115,-250,128,-250,129,-250,126,-250,6,-250,5,-250,125,-250,123,-250,124,-250,122,-250,137,-250,135,-250,16,-250,70,-250,92,-250,98,-250,101,-250,33,-250,104,-250,2,-250,12,-250,99,-250,32,-250,85,-250,84,-250,83,-250,82,-250,81,-250,86,-250,76,-250,51,-250,58,-250,141,-250,143,-250,161,-250,80,-250,78,-250,160,-250,87,-250,45,-250,42,-250,21,-250,22,-250,144,-250,147,-250,145,-250,146,-250,155,-250,158,-250,157,-250,156,-250,11,-250,57,-250,91,-250,40,-250,25,-250,97,-250,54,-250,35,-250,55,-250,102,-250,47,-250,36,-250,53,-250,60,-250,74,-250,72,-250,38,-250,71,-250,127,-250,110,-250});
    states[194] = new State(-251);
    states[195] = new State(new int[]{6,1511,116,248,115,249,128,250,129,251,13,-255,121,-255,100,-255,120,-255,9,-255,10,-255,8,-255,138,-255,136,-255,118,-255,117,-255,131,-255,132,-255,133,-255,134,-255,130,-255,126,-255,5,-255,125,-255,123,-255,124,-255,122,-255,137,-255,135,-255,16,-255,70,-255,92,-255,98,-255,101,-255,33,-255,104,-255,2,-255,12,-255,99,-255,32,-255,85,-255,84,-255,83,-255,82,-255,81,-255,86,-255,76,-255,51,-255,58,-255,141,-255,143,-255,161,-255,80,-255,78,-255,160,-255,87,-255,45,-255,42,-255,21,-255,22,-255,144,-255,147,-255,145,-255,146,-255,155,-255,158,-255,157,-255,156,-255,11,-255,57,-255,91,-255,40,-255,25,-255,97,-255,54,-255,35,-255,55,-255,102,-255,47,-255,36,-255,53,-255,60,-255,74,-255,72,-255,38,-255,71,-255,127,-255,110,-255},new int[]{-196,196});
    states[196] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286},new int[]{-109,197,-110,316,-183,353,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284});
    states[197] = new State(new int[]{118,255,117,256,131,257,132,258,133,259,134,260,130,261,6,-259,116,-259,115,-259,128,-259,129,-259,13,-259,121,-259,100,-259,120,-259,9,-259,10,-259,8,-259,138,-259,136,-259,126,-259,5,-259,125,-259,123,-259,124,-259,122,-259,137,-259,135,-259,16,-259,70,-259,92,-259,98,-259,101,-259,33,-259,104,-259,2,-259,12,-259,99,-259,32,-259,85,-259,84,-259,83,-259,82,-259,81,-259,86,-259,76,-259,51,-259,58,-259,141,-259,143,-259,161,-259,80,-259,78,-259,160,-259,87,-259,45,-259,42,-259,21,-259,22,-259,144,-259,147,-259,145,-259,146,-259,155,-259,158,-259,157,-259,156,-259,11,-259,57,-259,91,-259,40,-259,25,-259,97,-259,54,-259,35,-259,55,-259,102,-259,47,-259,36,-259,53,-259,60,-259,74,-259,72,-259,38,-259,71,-259,127,-259,110,-259},new int[]{-198,198});
    states[198] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286},new int[]{-110,199,-183,353,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284});
    states[199] = new State(new int[]{8,200,118,-261,117,-261,131,-261,132,-261,133,-261,134,-261,130,-261,6,-261,116,-261,115,-261,128,-261,129,-261,13,-261,121,-261,100,-261,120,-261,9,-261,10,-261,138,-261,136,-261,126,-261,5,-261,125,-261,123,-261,124,-261,122,-261,137,-261,135,-261,16,-261,70,-261,92,-261,98,-261,101,-261,33,-261,104,-261,2,-261,12,-261,99,-261,32,-261,85,-261,84,-261,83,-261,82,-261,81,-261,86,-261,76,-261,51,-261,58,-261,141,-261,143,-261,161,-261,80,-261,78,-261,160,-261,87,-261,45,-261,42,-261,21,-261,22,-261,144,-261,147,-261,145,-261,146,-261,155,-261,158,-261,157,-261,156,-261,11,-261,57,-261,91,-261,40,-261,25,-261,97,-261,54,-261,35,-261,55,-261,102,-261,47,-261,36,-261,53,-261,60,-261,74,-261,72,-261,38,-261,71,-261,127,-261,110,-261});
    states[200] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340,9,-192},new int[]{-78,201,-76,203,-97,1510,-93,206,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[201] = new State(new int[]{9,202});
    states[202] = new State(-266);
    states[203] = new State(new int[]{100,204,9,-191,12,-191});
    states[204] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-97,205,-93,206,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[205] = new State(-194);
    states[206] = new State(new int[]{13,207,16,211,6,1504,100,-195,9,-195,12,-195,5,-195});
    states[207] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-93,208,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[208] = new State(new int[]{5,209,13,207,16,211});
    states[209] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-93,210,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[210] = new State(new int[]{13,207,16,211,6,-129,100,-129,9,-129,12,-129,5,-129,92,-129,10,-129,98,-129,101,-129,33,-129,104,-129,2,-129,99,-129,32,-129,85,-129,84,-129,83,-129,82,-129,81,-129,86,-129});
    states[211] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-94,212,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850});
    states[212] = new State(new int[]{120,238,125,239,123,240,121,241,124,242,122,243,137,244,135,245,13,-128,16,-128,6,-128,100,-128,9,-128,12,-128,5,-128,92,-128,10,-128,98,-128,101,-128,33,-128,104,-128,2,-128,99,-128,32,-128,85,-128,84,-128,83,-128,82,-128,81,-128,86,-128},new int[]{-195,213});
    states[213] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-84,214,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850});
    states[214] = new State(new int[]{116,248,115,249,128,250,129,251,120,-125,125,-125,123,-125,121,-125,124,-125,122,-125,137,-125,135,-125,13,-125,16,-125,6,-125,100,-125,9,-125,12,-125,5,-125,92,-125,10,-125,98,-125,101,-125,33,-125,104,-125,2,-125,99,-125,32,-125,85,-125,84,-125,83,-125,82,-125,81,-125,86,-125},new int[]{-196,215});
    states[215] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-16,216,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850});
    states[216] = new State(new int[]{136,253,138,254,118,255,117,256,131,257,132,258,133,259,134,260,130,261,116,-139,115,-139,128,-139,129,-139,120,-139,125,-139,123,-139,121,-139,124,-139,122,-139,137,-139,135,-139,13,-139,16,-139,6,-139,100,-139,9,-139,12,-139,5,-139,92,-139,10,-139,98,-139,101,-139,33,-139,104,-139,2,-139,99,-139,32,-139,85,-139,84,-139,83,-139,82,-139,81,-139,86,-139},new int[]{-204,217,-198,220});
    states[217] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-287,218,-183,182,-150,219,-154,50,-155,53});
    states[218] = new State(-144);
    states[219] = new State(-267);
    states[220] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-13,221,-273,222,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-14,850});
    states[221] = new State(-151);
    states[222] = new State(-152);
    states[223] = new State(new int[]{4,225,11,227,7,827,142,829,8,830,136,-162,138,-162,118,-162,117,-162,131,-162,132,-162,133,-162,134,-162,130,-162,116,-162,115,-162,128,-162,129,-162,120,-162,125,-162,123,-162,121,-162,124,-162,122,-162,137,-162,135,-162,13,-162,16,-162,6,-162,100,-162,9,-162,12,-162,5,-162,92,-162,10,-162,98,-162,101,-162,33,-162,104,-162,2,-162,99,-162,32,-162,85,-162,84,-162,83,-162,82,-162,81,-162,86,-162,119,-160},new int[]{-15,224});
    states[224] = new State(-182);
    states[225] = new State(new int[]{123,188},new int[]{-302,226});
    states[226] = new State(-183);
    states[227] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340,5,1506,12,-192},new int[]{-124,228,-78,230,-93,232,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856,-76,203,-97,1510});
    states[228] = new State(new int[]{12,229});
    states[229] = new State(-184);
    states[230] = new State(new int[]{12,231});
    states[231] = new State(-188);
    states[232] = new State(new int[]{5,233,13,207,16,211,6,1504,100,-195,12,-195});
    states[233] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340,5,-714,12,-714},new int[]{-125,234,-93,1503,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[234] = new State(new int[]{5,235,12,-719});
    states[235] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-93,236,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[236] = new State(new int[]{13,207,16,211,12,-721});
    states[237] = new State(new int[]{120,238,125,239,123,240,121,241,124,242,122,243,137,244,135,245,13,-126,16,-126,6,-126,100,-126,9,-126,12,-126,5,-126,92,-126,10,-126,98,-126,101,-126,33,-126,104,-126,2,-126,99,-126,32,-126,85,-126,84,-126,83,-126,82,-126,81,-126,86,-126},new int[]{-195,213});
    states[238] = new State(-130);
    states[239] = new State(-131);
    states[240] = new State(-132);
    states[241] = new State(-133);
    states[242] = new State(-134);
    states[243] = new State(-135);
    states[244] = new State(-136);
    states[245] = new State(new int[]{137,246});
    states[246] = new State(-137);
    states[247] = new State(new int[]{116,248,115,249,128,250,129,251,120,-124,125,-124,123,-124,121,-124,124,-124,122,-124,137,-124,135,-124,13,-124,16,-124,6,-124,100,-124,9,-124,12,-124,5,-124,92,-124,10,-124,98,-124,101,-124,33,-124,104,-124,2,-124,99,-124,32,-124,85,-124,84,-124,83,-124,82,-124,81,-124,86,-124},new int[]{-196,215});
    states[248] = new State(-140);
    states[249] = new State(-141);
    states[250] = new State(-142);
    states[251] = new State(-143);
    states[252] = new State(new int[]{136,253,138,254,118,255,117,256,131,257,132,258,133,259,134,260,130,261,116,-138,115,-138,128,-138,129,-138,120,-138,125,-138,123,-138,121,-138,124,-138,122,-138,137,-138,135,-138,13,-138,16,-138,6,-138,100,-138,9,-138,12,-138,5,-138,92,-138,10,-138,98,-138,101,-138,33,-138,104,-138,2,-138,99,-138,32,-138,85,-138,84,-138,83,-138,82,-138,81,-138,86,-138},new int[]{-204,217,-198,220});
    states[253] = new State(-741);
    states[254] = new State(-742);
    states[255] = new State(-153);
    states[256] = new State(-154);
    states[257] = new State(-155);
    states[258] = new State(-156);
    states[259] = new State(-157);
    states[260] = new State(-158);
    states[261] = new State(-159);
    states[262] = new State(-148);
    states[263] = new State(-176);
    states[264] = new State(new int[]{26,1492,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,11,-866,17,-866,8,-866,7,-866,142,-866,4,-866,15,-866,110,-866,111,-866,112,-866,113,-866,114,-866,92,-866,10,-866,5,-866,98,-866,101,-866,33,-866,104,-866,2,-866,127,-866,138,-866,136,-866,118,-866,117,-866,131,-866,132,-866,133,-866,134,-866,130,-866,116,-866,115,-866,128,-866,129,-866,126,-866,6,-866,120,-866,125,-866,123,-866,121,-866,124,-866,122,-866,137,-866,135,-866,16,-866,70,-866,9,-866,12,-866,100,-866,99,-866,32,-866,84,-866,83,-866,82,-866,81,-866,13,-866,119,-866,76,-866,51,-866,58,-866,141,-866,45,-866,42,-866,21,-866,22,-866,144,-866,147,-866,145,-866,146,-866,155,-866,158,-866,157,-866,156,-866,57,-866,91,-866,40,-866,25,-866,97,-866,54,-866,35,-866,55,-866,102,-866,47,-866,36,-866,53,-866,60,-866,74,-866,72,-866,38,-866,71,-866},new int[]{-287,265,-183,182,-150,219,-154,50,-155,53});
    states[265] = new State(new int[]{11,267,8,1489,92,-650,10,-650,98,-650,101,-650,33,-650,104,-650,2,-650,138,-650,136,-650,118,-650,117,-650,131,-650,132,-650,133,-650,134,-650,130,-650,116,-650,115,-650,128,-650,129,-650,126,-650,6,-650,5,-650,120,-650,125,-650,123,-650,121,-650,124,-650,122,-650,137,-650,135,-650,16,-650,70,-650,9,-650,12,-650,100,-650,99,-650,32,-650,85,-650,84,-650,83,-650,82,-650,81,-650,86,-650,13,-650,76,-650,51,-650,58,-650,141,-650,143,-650,161,-650,80,-650,78,-650,160,-650,87,-650,45,-650,42,-650,21,-650,22,-650,144,-650,147,-650,145,-650,146,-650,155,-650,158,-650,157,-650,156,-650,57,-650,91,-650,40,-650,25,-650,97,-650,54,-650,35,-650,55,-650,102,-650,47,-650,36,-650,53,-650,60,-650,74,-650,72,-650,38,-650,71,-650},new int[]{-73,266});
    states[266] = new State(-641);
    states[267] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,834,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683,12,-818},new int[]{-69,268,-74,641,-91,397,-90,27,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,835,-103,660,-327,678});
    states[268] = new State(new int[]{12,269});
    states[269] = new State(new int[]{8,271,92,-640,10,-640,98,-640,101,-640,33,-640,104,-640,2,-640,138,-640,136,-640,118,-640,117,-640,131,-640,132,-640,133,-640,134,-640,130,-640,116,-640,115,-640,128,-640,129,-640,126,-640,6,-640,5,-640,120,-640,125,-640,123,-640,121,-640,124,-640,122,-640,137,-640,135,-640,16,-640,70,-640,9,-640,12,-640,100,-640,99,-640,32,-640,85,-640,84,-640,83,-640,82,-640,81,-640,86,-640,13,-640,76,-640,51,-640,58,-640,141,-640,143,-640,161,-640,80,-640,78,-640,160,-640,87,-640,45,-640,42,-640,21,-640,22,-640,144,-640,147,-640,145,-640,146,-640,155,-640,158,-640,157,-640,156,-640,11,-640,57,-640,91,-640,40,-640,25,-640,97,-640,54,-640,35,-640,55,-640,102,-640,47,-640,36,-640,53,-640,60,-640,74,-640,72,-640,38,-640,71,-640},new int[]{-8,270});
    states[270] = new State(-642);
    states[271] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,1001,135,842,116,354,115,355,63,340,9,-205},new int[]{-68,272,-67,274,-88,1004,-87,277,-93,278,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856,-98,1005,-247,1006});
    states[272] = new State(new int[]{9,273});
    states[273] = new State(-639);
    states[274] = new State(new int[]{100,275,9,-206});
    states[275] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,1001,135,842,116,354,115,355,63,340},new int[]{-88,276,-87,277,-93,278,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856,-98,1005,-247,1006});
    states[276] = new State(-208);
    states[277] = new State(-424);
    states[278] = new State(new int[]{13,207,16,211,100,-201,9,-201,92,-201,10,-201,98,-201,101,-201,33,-201,104,-201,2,-201,12,-201,99,-201,32,-201,85,-201,84,-201,83,-201,82,-201,81,-201,86,-201});
    states[279] = new State(-177);
    states[280] = new State(new int[]{144,282,147,283,7,-829,11,-829,17,-829,138,-829,136,-829,118,-829,117,-829,131,-829,132,-829,133,-829,134,-829,130,-829,116,-829,115,-829,128,-829,129,-829,126,-829,6,-829,5,-829,120,-829,125,-829,123,-829,121,-829,124,-829,122,-829,137,-829,135,-829,16,-829,70,-829,92,-829,10,-829,98,-829,101,-829,33,-829,104,-829,2,-829,9,-829,12,-829,100,-829,99,-829,32,-829,85,-829,84,-829,83,-829,82,-829,81,-829,86,-829,13,-829,119,-829,76,-829,51,-829,58,-829,141,-829,143,-829,161,-829,80,-829,78,-829,160,-829,87,-829,45,-829,42,-829,8,-829,21,-829,22,-829,145,-829,146,-829,155,-829,158,-829,157,-829,156,-829,57,-829,91,-829,40,-829,25,-829,97,-829,54,-829,35,-829,55,-829,102,-829,47,-829,36,-829,53,-829,60,-829,74,-829,72,-829,38,-829,71,-829,127,-829,110,-829,4,-829,142,-829},new int[]{-169,281});
    states[281] = new State(-833);
    states[282] = new State(-827);
    states[283] = new State(-828);
    states[284] = new State(-832);
    states[285] = new State(-830);
    states[286] = new State(-831);
    states[287] = new State(-178);
    states[288] = new State(-197);
    states[289] = new State(-198);
    states[290] = new State(-199);
    states[291] = new State(-200);
    states[292] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,293,-154,50,-155,53});
    states[293] = new State(-179);
    states[294] = new State(-180);
    states[295] = new State(new int[]{8,296});
    states[296] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-287,297,-183,182,-150,219,-154,50,-155,53});
    states[297] = new State(new int[]{9,298});
    states[298] = new State(-627);
    states[299] = new State(-181);
    states[300] = new State(new int[]{8,301});
    states[301] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-287,302,-286,304,-183,306,-150,219,-154,50,-155,53});
    states[302] = new State(new int[]{9,303});
    states[303] = new State(-628);
    states[304] = new State(new int[]{9,305});
    states[305] = new State(-629);
    states[306] = new State(new int[]{7,183,4,307,123,309,125,1487,9,-634},new int[]{-302,185,-303,1488});
    states[307] = new State(new int[]{123,309,125,1487},new int[]{-302,187,-303,308});
    states[308] = new State(-633);
    states[309] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,24,495,48,503,49,550,34,554,73,558,44,564,37,605,121,-249,100,-249},new int[]{-300,189,-301,310,-283,314,-276,193,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-285,1471,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,1472,-227,562,-226,563,-304,1473,-284,1486});
    states[310] = new State(new int[]{121,311,100,312});
    states[311] = new State(-244);
    states[312] = new State(-249,new int[]{-284,313});
    states[313] = new State(-248);
    states[314] = new State(-245);
    states[315] = new State(new int[]{118,255,117,256,131,257,132,258,133,259,134,260,130,261,6,-258,116,-258,115,-258,128,-258,129,-258,13,-258,121,-258,100,-258,120,-258,9,-258,10,-258,8,-258,138,-258,136,-258,126,-258,5,-258,125,-258,123,-258,124,-258,122,-258,137,-258,135,-258,16,-258,70,-258,92,-258,98,-258,101,-258,33,-258,104,-258,2,-258,12,-258,99,-258,32,-258,85,-258,84,-258,83,-258,82,-258,81,-258,86,-258,76,-258,51,-258,58,-258,141,-258,143,-258,161,-258,80,-258,78,-258,160,-258,87,-258,45,-258,42,-258,21,-258,22,-258,144,-258,147,-258,145,-258,146,-258,155,-258,158,-258,157,-258,156,-258,11,-258,57,-258,91,-258,40,-258,25,-258,97,-258,54,-258,35,-258,55,-258,102,-258,47,-258,36,-258,53,-258,60,-258,74,-258,72,-258,38,-258,71,-258,127,-258,110,-258},new int[]{-198,198});
    states[316] = new State(new int[]{8,200,118,-260,117,-260,131,-260,132,-260,133,-260,134,-260,130,-260,6,-260,116,-260,115,-260,128,-260,129,-260,13,-260,121,-260,100,-260,120,-260,9,-260,10,-260,138,-260,136,-260,126,-260,5,-260,125,-260,123,-260,124,-260,122,-260,137,-260,135,-260,16,-260,70,-260,92,-260,98,-260,101,-260,33,-260,104,-260,2,-260,12,-260,99,-260,32,-260,85,-260,84,-260,83,-260,82,-260,81,-260,86,-260,76,-260,51,-260,58,-260,141,-260,143,-260,161,-260,80,-260,78,-260,160,-260,87,-260,45,-260,42,-260,21,-260,22,-260,144,-260,147,-260,145,-260,146,-260,155,-260,158,-260,157,-260,156,-260,11,-260,57,-260,91,-260,40,-260,25,-260,97,-260,54,-260,35,-260,55,-260,102,-260,47,-260,36,-260,53,-260,60,-260,74,-260,72,-260,38,-260,71,-260,127,-260,110,-260});
    states[317] = new State(new int[]{7,183,127,318,123,188,8,-262,118,-262,117,-262,131,-262,132,-262,133,-262,134,-262,130,-262,6,-262,116,-262,115,-262,128,-262,129,-262,13,-262,121,-262,100,-262,120,-262,9,-262,10,-262,138,-262,136,-262,126,-262,5,-262,125,-262,124,-262,122,-262,137,-262,135,-262,16,-262,70,-262,92,-262,98,-262,101,-262,33,-262,104,-262,2,-262,12,-262,99,-262,32,-262,85,-262,84,-262,83,-262,82,-262,81,-262,86,-262,76,-262,51,-262,58,-262,141,-262,143,-262,161,-262,80,-262,78,-262,160,-262,87,-262,45,-262,42,-262,21,-262,22,-262,144,-262,147,-262,145,-262,146,-262,155,-262,158,-262,157,-262,156,-262,11,-262,57,-262,91,-262,40,-262,25,-262,97,-262,54,-262,35,-262,55,-262,102,-262,47,-262,36,-262,53,-262,60,-262,74,-262,72,-262,38,-262,71,-262,110,-262},new int[]{-302,637});
    states[318] = new State(new int[]{8,320,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-283,319,-276,193,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-285,1471,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,1472,-227,562,-226,563,-304,1473});
    states[319] = new State(-295);
    states[320] = new State(new int[]{9,321,143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-83,326,-81,332,-280,333,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[321] = new State(new int[]{127,322,121,-299,100,-299,120,-299,9,-299,10,-299,8,-299,138,-299,136,-299,118,-299,117,-299,131,-299,132,-299,133,-299,134,-299,130,-299,116,-299,115,-299,128,-299,129,-299,126,-299,6,-299,5,-299,125,-299,123,-299,124,-299,122,-299,137,-299,135,-299,16,-299,70,-299,92,-299,98,-299,101,-299,33,-299,104,-299,2,-299,12,-299,99,-299,32,-299,85,-299,84,-299,83,-299,82,-299,81,-299,86,-299,13,-299,76,-299,51,-299,58,-299,141,-299,143,-299,161,-299,80,-299,78,-299,160,-299,87,-299,45,-299,42,-299,21,-299,22,-299,144,-299,147,-299,145,-299,146,-299,155,-299,158,-299,157,-299,156,-299,11,-299,57,-299,91,-299,40,-299,25,-299,97,-299,54,-299,35,-299,55,-299,102,-299,47,-299,36,-299,53,-299,60,-299,74,-299,72,-299,38,-299,71,-299,110,-299});
    states[322] = new State(new int[]{8,324,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-283,323,-276,193,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-285,1471,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,1472,-227,562,-226,563,-304,1473});
    states[323] = new State(-297);
    states[324] = new State(new int[]{9,325,143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-83,326,-81,332,-280,333,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[325] = new State(new int[]{127,322,121,-301,100,-301,120,-301,9,-301,10,-301,8,-301,138,-301,136,-301,118,-301,117,-301,131,-301,132,-301,133,-301,134,-301,130,-301,116,-301,115,-301,128,-301,129,-301,126,-301,6,-301,5,-301,125,-301,123,-301,124,-301,122,-301,137,-301,135,-301,16,-301,70,-301,92,-301,98,-301,101,-301,33,-301,104,-301,2,-301,12,-301,99,-301,32,-301,85,-301,84,-301,83,-301,82,-301,81,-301,86,-301,13,-301,76,-301,51,-301,58,-301,141,-301,143,-301,161,-301,80,-301,78,-301,160,-301,87,-301,45,-301,42,-301,21,-301,22,-301,144,-301,147,-301,145,-301,146,-301,155,-301,158,-301,157,-301,156,-301,11,-301,57,-301,91,-301,40,-301,25,-301,97,-301,54,-301,35,-301,55,-301,102,-301,47,-301,36,-301,53,-301,60,-301,74,-301,72,-301,38,-301,71,-301,110,-301});
    states[326] = new State(new int[]{9,327,100,360});
    states[327] = new State(new int[]{127,328,13,-257,121,-257,100,-257,120,-257,9,-257,10,-257,8,-257,138,-257,136,-257,118,-257,117,-257,131,-257,132,-257,133,-257,134,-257,130,-257,116,-257,115,-257,128,-257,129,-257,126,-257,6,-257,5,-257,125,-257,123,-257,124,-257,122,-257,137,-257,135,-257,16,-257,70,-257,92,-257,98,-257,101,-257,33,-257,104,-257,2,-257,12,-257,99,-257,32,-257,85,-257,84,-257,83,-257,82,-257,81,-257,86,-257,76,-257,51,-257,58,-257,141,-257,143,-257,161,-257,80,-257,78,-257,160,-257,87,-257,45,-257,42,-257,21,-257,22,-257,144,-257,147,-257,145,-257,146,-257,155,-257,158,-257,157,-257,156,-257,11,-257,57,-257,91,-257,40,-257,25,-257,97,-257,54,-257,35,-257,55,-257,102,-257,47,-257,36,-257,53,-257,60,-257,74,-257,72,-257,38,-257,71,-257,110,-257});
    states[328] = new State(new int[]{8,330,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-283,329,-276,193,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-285,1471,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,1472,-227,562,-226,563,-304,1473});
    states[329] = new State(-298);
    states[330] = new State(new int[]{9,331,143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-83,326,-81,332,-280,333,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[331] = new State(new int[]{127,322,121,-302,100,-302,120,-302,9,-302,10,-302,8,-302,138,-302,136,-302,118,-302,117,-302,131,-302,132,-302,133,-302,134,-302,130,-302,116,-302,115,-302,128,-302,129,-302,126,-302,6,-302,5,-302,125,-302,123,-302,124,-302,122,-302,137,-302,135,-302,16,-302,70,-302,92,-302,98,-302,101,-302,33,-302,104,-302,2,-302,12,-302,99,-302,32,-302,85,-302,84,-302,83,-302,82,-302,81,-302,86,-302,13,-302,76,-302,51,-302,58,-302,141,-302,143,-302,161,-302,80,-302,78,-302,160,-302,87,-302,45,-302,42,-302,21,-302,22,-302,144,-302,147,-302,145,-302,146,-302,155,-302,158,-302,157,-302,156,-302,11,-302,57,-302,91,-302,40,-302,25,-302,97,-302,54,-302,35,-302,55,-302,102,-302,47,-302,36,-302,53,-302,60,-302,74,-302,72,-302,38,-302,71,-302,110,-302});
    states[332] = new State(-269);
    states[333] = new State(new int[]{120,334,9,-271,100,-271});
    states[334] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,335,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[335] = new State(new int[]{70,28,9,-272,100,-272});
    states[336] = new State(-752);
    states[337] = new State(-778);
    states[338] = new State(-779);
    states[339] = new State(-772);
    states[340] = new State(new int[]{8,341});
    states[341] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,357},new int[]{-288,342,-287,344,-183,345,-150,219,-154,50,-155,53,-277,347,-276,348,-96,195,-109,315,-110,316,-19,350,-202,351,-168,356,-170,280,-169,284,-304,1485});
    states[342] = new State(new int[]{9,343});
    states[343] = new State(-766);
    states[344] = new State(-637);
    states[345] = new State(new int[]{7,183,4,186,123,188,9,-634,8,-262,118,-262,117,-262,131,-262,132,-262,133,-262,134,-262,130,-262,6,-262,116,-262,115,-262,128,-262,129,-262,13,-262},new int[]{-302,346});
    states[346] = new State(new int[]{9,-635,13,-241});
    states[347] = new State(-638);
    states[348] = new State(new int[]{13,349});
    states[349] = new State(-232);
    states[350] = new State(-263);
    states[351] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286},new int[]{-110,352,-183,353,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284});
    states[352] = new State(new int[]{8,200,118,-264,117,-264,131,-264,132,-264,133,-264,134,-264,130,-264,6,-264,116,-264,115,-264,128,-264,129,-264,13,-264,121,-264,100,-264,120,-264,9,-264,10,-264,138,-264,136,-264,126,-264,5,-264,125,-264,123,-264,124,-264,122,-264,137,-264,135,-264,16,-264,70,-264,92,-264,98,-264,101,-264,33,-264,104,-264,2,-264,12,-264,99,-264,32,-264,85,-264,84,-264,83,-264,82,-264,81,-264,86,-264,76,-264,51,-264,58,-264,141,-264,143,-264,161,-264,80,-264,78,-264,160,-264,87,-264,45,-264,42,-264,21,-264,22,-264,144,-264,147,-264,145,-264,146,-264,155,-264,158,-264,157,-264,156,-264,11,-264,57,-264,91,-264,40,-264,25,-264,97,-264,54,-264,35,-264,55,-264,102,-264,47,-264,36,-264,53,-264,60,-264,74,-264,72,-264,38,-264,71,-264,127,-264,110,-264});
    states[353] = new State(new int[]{7,183,8,-262,118,-262,117,-262,131,-262,132,-262,133,-262,134,-262,130,-262,6,-262,116,-262,115,-262,128,-262,129,-262,13,-262,121,-262,100,-262,120,-262,9,-262,10,-262,138,-262,136,-262,126,-262,5,-262,125,-262,123,-262,124,-262,122,-262,137,-262,135,-262,16,-262,70,-262,92,-262,98,-262,101,-262,33,-262,104,-262,2,-262,12,-262,99,-262,32,-262,85,-262,84,-262,83,-262,82,-262,81,-262,86,-262,76,-262,51,-262,58,-262,141,-262,143,-262,161,-262,80,-262,78,-262,160,-262,87,-262,45,-262,42,-262,21,-262,22,-262,144,-262,147,-262,145,-262,146,-262,155,-262,158,-262,157,-262,156,-262,11,-262,57,-262,91,-262,40,-262,25,-262,97,-262,54,-262,35,-262,55,-262,102,-262,47,-262,36,-262,53,-262,60,-262,74,-262,72,-262,38,-262,71,-262,127,-262,110,-262});
    states[354] = new State(-174);
    states[355] = new State(-175);
    states[356] = new State(-265);
    states[357] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-83,358,-81,332,-280,333,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[358] = new State(new int[]{9,359,100,360});
    states[359] = new State(-257);
    states[360] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-81,361,-280,333,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[361] = new State(-270);
    states[362] = new State(new int[]{13,349,120,-234,9,-234,100,-234,10,-234,121,-234,8,-234,138,-234,136,-234,118,-234,117,-234,131,-234,132,-234,133,-234,134,-234,130,-234,116,-234,115,-234,128,-234,129,-234,126,-234,6,-234,5,-234,125,-234,123,-234,124,-234,122,-234,137,-234,135,-234,16,-234,70,-234,92,-234,98,-234,101,-234,33,-234,104,-234,2,-234,12,-234,99,-234,32,-234,85,-234,84,-234,83,-234,82,-234,81,-234,86,-234,76,-234,51,-234,58,-234,141,-234,143,-234,161,-234,80,-234,78,-234,160,-234,87,-234,45,-234,42,-234,21,-234,22,-234,144,-234,147,-234,145,-234,146,-234,155,-234,158,-234,157,-234,156,-234,11,-234,57,-234,91,-234,40,-234,25,-234,97,-234,54,-234,35,-234,55,-234,102,-234,47,-234,36,-234,53,-234,60,-234,74,-234,72,-234,38,-234,71,-234,127,-234,110,-234});
    states[363] = new State(new int[]{11,364,7,-841,127,-841,123,-841,8,-841,118,-841,117,-841,131,-841,132,-841,133,-841,134,-841,130,-841,6,-841,116,-841,115,-841,128,-841,129,-841,13,-841,120,-841,9,-841,100,-841,10,-841,121,-841,138,-841,136,-841,126,-841,5,-841,125,-841,124,-841,122,-841,137,-841,135,-841,16,-841,70,-841,92,-841,98,-841,101,-841,33,-841,104,-841,2,-841,12,-841,99,-841,32,-841,85,-841,84,-841,83,-841,82,-841,81,-841,86,-841,76,-841,51,-841,58,-841,141,-841,143,-841,161,-841,80,-841,78,-841,160,-841,87,-841,45,-841,42,-841,21,-841,22,-841,144,-841,147,-841,145,-841,146,-841,155,-841,158,-841,157,-841,156,-841,57,-841,91,-841,40,-841,25,-841,97,-841,54,-841,35,-841,55,-841,102,-841,47,-841,36,-841,53,-841,60,-841,74,-841,72,-841,38,-841,71,-841,110,-841});
    states[364] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-93,365,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[365] = new State(new int[]{12,366,13,207,16,211});
    states[366] = new State(-290);
    states[367] = new State(-163);
    states[368] = new State(-172);
    states[369] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599,12,-822},new int[]{-71,370,-80,372,-95,450,-90,375,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[370] = new State(new int[]{12,371});
    states[371] = new State(-171);
    states[372] = new State(new int[]{100,373,12,-821,76,-821});
    states[373] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-95,374,-90,375,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[374] = new State(-824);
    states[375] = new State(new int[]{70,28,6,376,100,-825,12,-825,76,-825});
    states[376] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,377,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[377] = new State(new int[]{70,28,100,-826,12,-826,76,-826});
    states[378] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369},new int[]{-99,379,-18,380,-168,337,-170,280,-169,284,-19,338,-59,339,-202,390,-116,391,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459});
    states[379] = new State(-773);
    states[380] = new State(new int[]{7,45,11,151,17,381,138,-771,136,-771,118,-771,117,-771,131,-771,132,-771,133,-771,134,-771,130,-771,116,-771,115,-771,128,-771,129,-771,126,-771,6,-771,5,-771,120,-771,125,-771,123,-771,121,-771,124,-771,122,-771,137,-771,135,-771,16,-771,70,-771,92,-771,10,-771,98,-771,101,-771,33,-771,104,-771,2,-771,9,-771,12,-771,100,-771,99,-771,32,-771,85,-771,84,-771,83,-771,82,-771,81,-771,86,-771,13,-771,76,-771,51,-771,58,-771,141,-771,143,-771,161,-771,80,-771,78,-771,160,-771,87,-771,45,-771,42,-771,8,-771,21,-771,22,-771,144,-771,147,-771,145,-771,146,-771,155,-771,158,-771,157,-771,156,-771,57,-771,91,-771,40,-771,25,-771,97,-771,54,-771,35,-771,55,-771,102,-771,47,-771,36,-771,53,-771,60,-771,74,-771,72,-771,38,-771,71,-771});
    states[381] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,5,599},new int[]{-123,382,-108,384,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,603,-271,579});
    states[382] = new State(new int[]{12,383});
    states[383] = new State(-809);
    states[384] = new State(new int[]{5,168,6,35});
    states[385] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369},new int[]{-99,386,-272,387,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-100,462});
    states[386] = new State(-774);
    states[387] = new State(-751);
    states[388] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369},new int[]{-99,389,-18,380,-168,337,-170,280,-169,284,-19,338,-59,339,-202,390,-116,391,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459});
    states[389] = new State(-775);
    states[390] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369},new int[]{-99,386,-18,380,-168,337,-170,280,-169,284,-19,338,-59,339,-202,390,-116,391,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459});
    states[391] = new State(-776);
    states[392] = new State(new int[]{141,1484,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,1114,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369},new int[]{-114,393,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729});
    states[393] = new State(new int[]{11,394,17,401,8,703,7,1109,142,1111,4,1112,110,-782,111,-782,112,-782,113,-782,114,-782,92,-782,10,-782,98,-782,101,-782,33,-782,104,-782,2,-782,138,-782,136,-782,118,-782,117,-782,131,-782,132,-782,133,-782,134,-782,130,-782,116,-782,115,-782,128,-782,129,-782,126,-782,6,-782,5,-782,120,-782,125,-782,123,-782,121,-782,124,-782,122,-782,137,-782,135,-782,16,-782,70,-782,9,-782,12,-782,100,-782,99,-782,32,-782,85,-782,84,-782,83,-782,82,-782,81,-782,86,-782,13,-782,119,-782,76,-782,51,-782,58,-782,141,-782,143,-782,161,-782,80,-782,78,-782,160,-782,87,-782,45,-782,42,-782,21,-782,22,-782,144,-782,147,-782,145,-782,146,-782,155,-782,158,-782,157,-782,156,-782,57,-782,91,-782,40,-782,25,-782,97,-782,54,-782,35,-782,55,-782,102,-782,47,-782,36,-782,53,-782,60,-782,74,-782,72,-782,38,-782,71,-782});
    states[394] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,834,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683},new int[]{-74,395,-91,397,-90,27,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,835,-103,660,-327,678});
    states[395] = new State(new int[]{12,396,100,154});
    states[396] = new State(-806);
    states[397] = new State(-597);
    states[398] = new State(new int[]{138,-776,136,-776,118,-776,117,-776,131,-776,132,-776,133,-776,134,-776,130,-776,116,-776,115,-776,128,-776,129,-776,126,-776,6,-776,5,-776,120,-776,125,-776,123,-776,121,-776,124,-776,122,-776,137,-776,135,-776,16,-776,70,-776,92,-776,10,-776,98,-776,101,-776,33,-776,104,-776,2,-776,9,-776,12,-776,100,-776,99,-776,32,-776,85,-776,84,-776,83,-776,82,-776,81,-776,86,-776,13,-776,76,-776,51,-776,58,-776,141,-776,143,-776,161,-776,80,-776,78,-776,160,-776,87,-776,45,-776,42,-776,8,-776,21,-776,22,-776,144,-776,147,-776,145,-776,146,-776,155,-776,158,-776,157,-776,156,-776,11,-776,57,-776,91,-776,40,-776,25,-776,97,-776,54,-776,35,-776,55,-776,102,-776,47,-776,36,-776,53,-776,60,-776,74,-776,72,-776,38,-776,71,-776,119,-769});
    states[399] = new State(-786);
    states[400] = new State(new int[]{11,394,17,401,8,703,7,1109,142,1111,4,1112,15,1119,110,-783,111,-783,112,-783,113,-783,114,-783,92,-783,10,-783,98,-783,101,-783,33,-783,104,-783,2,-783,138,-783,136,-783,118,-783,117,-783,131,-783,132,-783,133,-783,134,-783,130,-783,116,-783,115,-783,128,-783,129,-783,126,-783,6,-783,5,-783,120,-783,125,-783,123,-783,121,-783,124,-783,122,-783,137,-783,135,-783,16,-783,70,-783,9,-783,12,-783,100,-783,99,-783,32,-783,85,-783,84,-783,83,-783,82,-783,81,-783,86,-783,13,-783,119,-783,76,-783,51,-783,58,-783,141,-783,143,-783,161,-783,80,-783,78,-783,160,-783,87,-783,45,-783,42,-783,21,-783,22,-783,144,-783,147,-783,145,-783,146,-783,155,-783,158,-783,157,-783,156,-783,57,-783,91,-783,40,-783,25,-783,97,-783,54,-783,35,-783,55,-783,102,-783,47,-783,36,-783,53,-783,60,-783,74,-783,72,-783,38,-783,71,-783});
    states[401] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,5,599},new int[]{-123,402,-108,384,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,603,-271,579});
    states[402] = new State(new int[]{12,403});
    states[403] = new State(-808);
    states[404] = new State(-798);
    states[405] = new State(-799);
    states[406] = new State(new int[]{115,408,116,409,117,410,118,411,120,412,121,413,122,414,123,415,124,416,125,417,128,418,129,419,130,420,131,421,132,422,133,423,134,424,135,425,137,426,139,427,140,428,110,430,111,431,112,432,113,433,114,434,119,435},new int[]{-203,407,-197,429});
    states[407] = new State(-834);
    states[408] = new State(-958);
    states[409] = new State(-959);
    states[410] = new State(-960);
    states[411] = new State(-961);
    states[412] = new State(-962);
    states[413] = new State(-963);
    states[414] = new State(-964);
    states[415] = new State(-965);
    states[416] = new State(-966);
    states[417] = new State(-967);
    states[418] = new State(-968);
    states[419] = new State(-969);
    states[420] = new State(-970);
    states[421] = new State(-971);
    states[422] = new State(-972);
    states[423] = new State(-973);
    states[424] = new State(-974);
    states[425] = new State(-975);
    states[426] = new State(-976);
    states[427] = new State(-977);
    states[428] = new State(-978);
    states[429] = new State(-979);
    states[430] = new State(-981);
    states[431] = new State(-982);
    states[432] = new State(-983);
    states[433] = new State(-984);
    states[434] = new State(-985);
    states[435] = new State(-980);
    states[436] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,437,-154,50,-155,53});
    states[437] = new State(-800);
    states[438] = new State(new int[]{53,688,56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599,18,670},new int[]{-90,439,-363,441,-105,533,-104,1104,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-362,1105,-103,1106});
    states[439] = new State(new int[]{9,440,70,28});
    states[440] = new State(-801);
    states[441] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599,53,1115},new int[]{-90,442,-362,444,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[442] = new State(new int[]{9,443,70,28});
    states[443] = new State(-802);
    states[444] = new State(-796);
    states[445] = new State(-803);
    states[446] = new State(-804);
    states[447] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-71,448,-80,372,-95,450,-90,375,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[448] = new State(new int[]{76,449});
    states[449] = new State(-810);
    states[450] = new State(-823);
    states[451] = new State(-811);
    states[452] = new State(-812);
    states[453] = new State(new int[]{7,454,138,-777,136,-777,118,-777,117,-777,131,-777,132,-777,133,-777,134,-777,130,-777,116,-777,115,-777,128,-777,129,-777,126,-777,6,-777,5,-777,120,-777,125,-777,123,-777,121,-777,124,-777,122,-777,137,-777,135,-777,16,-777,70,-777,92,-777,10,-777,98,-777,101,-777,33,-777,104,-777,2,-777,9,-777,12,-777,100,-777,99,-777,32,-777,85,-777,84,-777,83,-777,82,-777,81,-777,86,-777,13,-777,76,-777,51,-777,58,-777,141,-777,143,-777,161,-777,80,-777,78,-777,160,-777,87,-777,45,-777,42,-777,8,-777,21,-777,22,-777,144,-777,147,-777,145,-777,146,-777,155,-777,158,-777,157,-777,156,-777,11,-777,57,-777,91,-777,40,-777,25,-777,97,-777,54,-777,35,-777,55,-777,102,-777,47,-777,36,-777,53,-777,60,-777,74,-777,72,-777,38,-777,71,-777});
    states[454] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,84,60,83,61,82,62,81,63,68,64,64,65,128,66,22,67,21,68,63,69,23,70,129,71,130,72,131,73,132,74,133,75,134,76,135,77,136,78,137,79,138,80,24,81,73,82,91,83,25,84,26,85,29,86,30,87,31,88,71,89,99,90,32,91,92,92,33,93,34,94,27,95,104,96,101,97,35,98,36,99,37,100,40,101,41,102,42,103,103,104,43,105,44,106,46,107,47,108,48,109,97,110,49,111,102,112,50,113,28,114,51,115,70,116,98,117,52,118,53,119,54,120,55,121,56,122,57,123,58,124,59,125,61,126,105,127,106,128,109,129,107,130,108,131,62,132,74,133,38,134,39,135,69,136,148,137,60,138,139,139,140,140,79,141,153,142,152,143,72,144,154,145,150,146,151,147,149,148,45,406},new int[]{-151,455,-150,456,-154,50,-155,53,-296,457,-153,59,-194,458});
    states[455] = new State(-814);
    states[456] = new State(-850);
    states[457] = new State(-851);
    states[458] = new State(-852);
    states[459] = new State(-784);
    states[460] = new State(-753);
    states[461] = new State(-754);
    states[462] = new State(new int[]{119,463});
    states[463] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369},new int[]{-99,464,-272,465,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-100,462});
    states[464] = new State(-749);
    states[465] = new State(-750);
    states[466] = new State(-758);
    states[467] = new State(new int[]{8,468,138,-743,136,-743,118,-743,117,-743,131,-743,132,-743,133,-743,134,-743,130,-743,116,-743,115,-743,128,-743,129,-743,126,-743,6,-743,5,-743,120,-743,125,-743,123,-743,121,-743,124,-743,122,-743,137,-743,135,-743,16,-743,70,-743,92,-743,10,-743,98,-743,101,-743,33,-743,104,-743,2,-743,9,-743,12,-743,100,-743,99,-743,32,-743,85,-743,84,-743,83,-743,82,-743,81,-743,86,-743,13,-743,76,-743,51,-743,58,-743,141,-743,143,-743,161,-743,80,-743,78,-743,160,-743,87,-743,45,-743,42,-743,21,-743,22,-743,144,-743,147,-743,145,-743,146,-743,155,-743,158,-743,157,-743,156,-743,11,-743,57,-743,91,-743,40,-743,25,-743,97,-743,54,-743,35,-743,55,-743,102,-743,47,-743,36,-743,53,-743,60,-743,74,-743,72,-743,38,-743,71,-743});
    states[468] = new State(new int[]{14,473,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,53,475,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,11,927,8,940},new int[]{-357,469,-355,1483,-18,474,-168,337,-170,280,-169,284,-19,338,-344,1474,-287,1475,-183,182,-150,219,-154,50,-155,53,-347,1481,-348,1482});
    states[469] = new State(new int[]{9,470,10,471,100,1479});
    states[470] = new State(-655);
    states[471] = new State(new int[]{14,473,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,53,475,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,11,927,8,940},new int[]{-355,472,-18,474,-168,337,-170,280,-169,284,-19,338,-344,1474,-287,1475,-183,182,-150,219,-154,50,-155,53,-347,1481,-348,1482});
    states[472] = new State(-692);
    states[473] = new State(-694);
    states[474] = new State(-695);
    states[475] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,476,-154,50,-155,53});
    states[476] = new State(new int[]{5,477,9,-697,10,-697,100,-697});
    states[477] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,478,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[478] = new State(-696);
    states[479] = new State(new int[]{9,480,143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-83,326,-81,332,-280,333,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[480] = new State(new int[]{127,322});
    states[481] = new State(-235);
    states[482] = new State(new int[]{13,483,127,484,120,-240,9,-240,100,-240,10,-240,121,-240,8,-240,138,-240,136,-240,118,-240,117,-240,131,-240,132,-240,133,-240,134,-240,130,-240,116,-240,115,-240,128,-240,129,-240,126,-240,6,-240,5,-240,125,-240,123,-240,124,-240,122,-240,137,-240,135,-240,16,-240,70,-240,92,-240,98,-240,101,-240,33,-240,104,-240,2,-240,12,-240,99,-240,32,-240,85,-240,84,-240,83,-240,82,-240,81,-240,86,-240,76,-240,51,-240,58,-240,141,-240,143,-240,161,-240,80,-240,78,-240,160,-240,87,-240,45,-240,42,-240,21,-240,22,-240,144,-240,147,-240,145,-240,146,-240,155,-240,158,-240,157,-240,156,-240,11,-240,57,-240,91,-240,40,-240,25,-240,97,-240,54,-240,35,-240,55,-240,102,-240,47,-240,36,-240,53,-240,60,-240,74,-240,72,-240,38,-240,71,-240,110,-240});
    states[483] = new State(-233);
    states[484] = new State(new int[]{8,486,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-283,485,-276,193,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-285,1471,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,1472,-227,562,-226,563,-304,1473});
    states[485] = new State(-296);
    states[486] = new State(new int[]{9,487,143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-83,326,-81,332,-280,333,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[487] = new State(new int[]{127,322,121,-300,100,-300,120,-300,9,-300,10,-300,8,-300,138,-300,136,-300,118,-300,117,-300,131,-300,132,-300,133,-300,134,-300,130,-300,116,-300,115,-300,128,-300,129,-300,126,-300,6,-300,5,-300,125,-300,123,-300,124,-300,122,-300,137,-300,135,-300,16,-300,70,-300,92,-300,98,-300,101,-300,33,-300,104,-300,2,-300,12,-300,99,-300,32,-300,85,-300,84,-300,83,-300,82,-300,81,-300,86,-300,13,-300,76,-300,51,-300,58,-300,141,-300,143,-300,161,-300,80,-300,78,-300,160,-300,87,-300,45,-300,42,-300,21,-300,22,-300,144,-300,147,-300,145,-300,146,-300,155,-300,158,-300,157,-300,156,-300,11,-300,57,-300,91,-300,40,-300,25,-300,97,-300,54,-300,35,-300,55,-300,102,-300,47,-300,36,-300,53,-300,60,-300,74,-300,72,-300,38,-300,71,-300,110,-300});
    states[488] = new State(-236);
    states[489] = new State(-237);
    states[490] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,491,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[491] = new State(-273);
    states[492] = new State(-491);
    states[493] = new State(-238);
    states[494] = new State(-274);
    states[495] = new State(new int[]{11,496,58,1469});
    states[496] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,357,12,-286,100,-286},new int[]{-167,497,-275,1468,-276,1467,-96,195,-109,315,-110,316,-183,353,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284});
    states[497] = new State(new int[]{12,498,100,1465});
    states[498] = new State(new int[]{58,499});
    states[499] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,500,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[500] = new State(-280);
    states[501] = new State(-281);
    states[502] = new State(-275);
    states[503] = new State(new int[]{8,1345,23,-322,11,-322,92,-322,84,-322,83,-322,82,-322,81,-322,29,-322,143,-322,161,-322,85,-322,86,-322,80,-322,78,-322,160,-322,87,-322,62,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322},new int[]{-186,504});
    states[504] = new State(new int[]{23,1336,11,-329,92,-329,84,-329,83,-329,82,-329,81,-329,29,-329,143,-329,161,-329,85,-329,86,-329,80,-329,78,-329,160,-329,87,-329,62,-329,28,-329,26,-329,44,-329,37,-329,19,-329,30,-329,31,-329,46,-329,27,-329},new int[]{-321,505,-320,1334,-319,1356});
    states[505] = new State(new int[]{11,629,92,-346,84,-346,83,-346,82,-346,81,-346,29,-219,143,-219,161,-219,85,-219,86,-219,80,-219,78,-219,160,-219,87,-219,62,-219,28,-219,26,-219,44,-219,37,-219,19,-219,30,-219,31,-219,46,-219,27,-219},new int[]{-27,506,-34,1257,-36,510,-47,1258,-9,1259,-254,1156,-35,1421,-56,1423,-55,516,-57,1422});
    states[506] = new State(new int[]{92,507,84,1253,83,1254,82,1255,81,1256},new int[]{-10,508});
    states[507] = new State(-304);
    states[508] = new State(new int[]{11,629,92,-346,84,-346,83,-346,82,-346,81,-346,29,-219,143,-219,161,-219,85,-219,86,-219,80,-219,78,-219,160,-219,87,-219,62,-219,28,-219,26,-219,44,-219,37,-219,19,-219,30,-219,31,-219,46,-219,27,-219},new int[]{-34,509,-36,510,-47,1258,-9,1259,-254,1156,-35,1421,-56,1423,-55,516,-57,1422});
    states[509] = new State(-341);
    states[510] = new State(new int[]{10,512,92,-352,84,-352,83,-352,82,-352,81,-352},new int[]{-193,511});
    states[511] = new State(-347);
    states[512] = new State(new int[]{11,629,92,-353,84,-353,83,-353,82,-353,81,-353,29,-219,143,-219,161,-219,85,-219,86,-219,80,-219,78,-219,160,-219,87,-219,62,-219,28,-219,26,-219,44,-219,37,-219,19,-219,30,-219,31,-219,46,-219,27,-219},new int[]{-47,513,-35,514,-9,1259,-254,1156,-56,1423,-55,516,-57,1422});
    states[513] = new State(-355);
    states[514] = new State(new int[]{11,629,92,-349,84,-349,83,-349,82,-349,81,-349,28,-219,26,-219,44,-219,37,-219,19,-219,30,-219,31,-219,46,-219,27,-219},new int[]{-56,515,-55,516,-9,517,-254,1156,-57,1422});
    states[515] = new State(-358);
    states[516] = new State(-359);
    states[517] = new State(new int[]{28,1381,26,1382,44,1329,37,1364,19,1384,30,1392,31,1399,11,629,46,1406,27,1415},new int[]{-225,518,-254,519,-222,520,-262,521,-6,522,-233,1383,-231,1317,-228,1328,-232,1363,-230,1390,-218,1403,-219,1404,-221,1405});
    states[518] = new State(-368);
    states[519] = new State(-218);
    states[520] = new State(-369);
    states[521] = new State(-385);
    states[522] = new State(new int[]{30,524,19,1199,46,1271,27,1309,44,1329,37,1364},new int[]{-233,523,-219,1198,-231,1317,-228,1328,-232,1363});
    states[523] = new State(-372);
    states[524] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,8,-384,110,-384,10,-384},new int[]{-175,525,-174,1181,-173,1182,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[525] = new State(new int[]{8,566,110,-475,10,-475},new int[]{-131,526});
    states[526] = new State(new int[]{110,528,10,1170},new int[]{-210,527});
    states[527] = new State(-381);
    states[528] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499},new int[]{-264,529,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[529] = new State(new int[]{10,530});
    states[530] = new State(-430);
    states[531] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,92,-579,10,-579,98,-579,101,-579,33,-579,104,-579,2,-579,9,-579,12,-579,100,-579,99,-579,32,-579,84,-579,83,-579,82,-579,81,-579},new int[]{-150,437,-154,50,-155,53});
    states[532] = new State(new int[]{53,1158,56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599,18,670},new int[]{-90,439,-363,441,-105,533,-114,695,-104,1104,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-362,1105,-103,1106});
    states[533] = new State(new int[]{100,534});
    states[534] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,18,670},new int[]{-82,535,-105,1149,-104,1148,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-103,1106});
    states[535] = new State(new int[]{100,1146,5,547,10,-1015,9,-1015},new int[]{-328,536});
    states[536] = new State(new int[]{10,539,9,-1003},new int[]{-335,537});
    states[537] = new State(new int[]{9,538});
    states[538] = new State(-767);
    states[539] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-330,540,-331,1097,-161,543,-150,810,-154,50,-155,53});
    states[540] = new State(new int[]{10,541,9,-1004});
    states[541] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-331,542,-161,543,-150,810,-154,50,-155,53});
    states[542] = new State(-1013);
    states[543] = new State(new int[]{100,545,5,547,10,-1015,9,-1015},new int[]{-328,544});
    states[544] = new State(-1014);
    states[545] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,546,-154,50,-155,53});
    states[546] = new State(-351);
    states[547] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,548,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[548] = new State(-1016);
    states[549] = new State(-276);
    states[550] = new State(new int[]{58,551});
    states[551] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,552,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[552] = new State(-287);
    states[553] = new State(-277);
    states[554] = new State(new int[]{58,555,121,-289,100,-289,120,-289,9,-289,10,-289,8,-289,138,-289,136,-289,118,-289,117,-289,131,-289,132,-289,133,-289,134,-289,130,-289,116,-289,115,-289,128,-289,129,-289,126,-289,6,-289,5,-289,125,-289,123,-289,124,-289,122,-289,137,-289,135,-289,16,-289,70,-289,92,-289,98,-289,101,-289,33,-289,104,-289,2,-289,12,-289,99,-289,32,-289,85,-289,84,-289,83,-289,82,-289,81,-289,86,-289,13,-289,76,-289,51,-289,141,-289,143,-289,161,-289,80,-289,78,-289,160,-289,87,-289,45,-289,42,-289,21,-289,22,-289,144,-289,147,-289,145,-289,146,-289,155,-289,158,-289,157,-289,156,-289,11,-289,57,-289,91,-289,40,-289,25,-289,97,-289,54,-289,35,-289,55,-289,102,-289,47,-289,36,-289,53,-289,60,-289,74,-289,72,-289,38,-289,71,-289,127,-289,110,-289});
    states[555] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,556,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[556] = new State(-288);
    states[557] = new State(-278);
    states[558] = new State(new int[]{58,559});
    states[559] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,560,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[560] = new State(-279);
    states[561] = new State(-239);
    states[562] = new State(-291);
    states[563] = new State(-292);
    states[564] = new State(new int[]{8,566,121,-475,100,-475,120,-475,9,-475,10,-475,138,-475,136,-475,118,-475,117,-475,131,-475,132,-475,133,-475,134,-475,130,-475,116,-475,115,-475,128,-475,129,-475,126,-475,6,-475,5,-475,125,-475,123,-475,124,-475,122,-475,137,-475,135,-475,16,-475,70,-475,92,-475,98,-475,101,-475,33,-475,104,-475,2,-475,12,-475,99,-475,32,-475,85,-475,84,-475,83,-475,82,-475,81,-475,86,-475,13,-475,76,-475,51,-475,58,-475,141,-475,143,-475,161,-475,80,-475,78,-475,160,-475,87,-475,45,-475,42,-475,21,-475,22,-475,144,-475,147,-475,145,-475,146,-475,155,-475,158,-475,157,-475,156,-475,11,-475,57,-475,91,-475,40,-475,25,-475,97,-475,54,-475,35,-475,55,-475,102,-475,47,-475,36,-475,53,-475,60,-475,74,-475,72,-475,38,-475,71,-475,127,-475,110,-475},new int[]{-131,565});
    states[565] = new State(-293);
    states[566] = new State(new int[]{9,567,11,629,143,-219,161,-219,85,-219,86,-219,80,-219,78,-219,160,-219,87,-219,53,-219,29,-219,108,-219},new int[]{-132,568,-58,1157,-9,572,-254,1156});
    states[567] = new State(-476);
    states[568] = new State(new int[]{9,569,10,570});
    states[569] = new State(-477);
    states[570] = new State(new int[]{11,629,143,-219,161,-219,85,-219,86,-219,80,-219,78,-219,160,-219,87,-219,53,-219,29,-219,108,-219},new int[]{-58,571,-9,572,-254,1156});
    states[571] = new State(-479);
    states[572] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,53,613,29,619,108,625,11,629},new int[]{-299,573,-254,519,-162,574,-138,612,-150,611,-154,50,-155,53});
    states[573] = new State(-480);
    states[574] = new State(new int[]{5,575,100,609});
    states[575] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,576,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[576] = new State(new int[]{110,577,9,-481,10,-481});
    states[577] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,578,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[578] = new State(new int[]{70,28,9,-485,10,-485});
    states[579] = new State(-744);
    states[580] = new State(new int[]{70,-614,92,-614,10,-614,98,-614,101,-614,33,-614,104,-614,2,-614,9,-614,12,-614,100,-614,99,-614,32,-614,85,-614,84,-614,83,-614,82,-614,81,-614,86,-614,6,-614,76,-614,5,-614,51,-614,58,-614,141,-614,143,-614,161,-614,80,-614,78,-614,160,-614,87,-614,45,-614,42,-614,8,-614,21,-614,22,-614,144,-614,147,-614,145,-614,146,-614,155,-614,158,-614,157,-614,156,-614,11,-614,57,-614,91,-614,40,-614,25,-614,97,-614,54,-614,35,-614,55,-614,102,-614,47,-614,36,-614,53,-614,60,-614,74,-614,72,-614,38,-614,71,-614,13,-617});
    states[581] = new State(new int[]{13,582});
    states[582] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369},new int[]{-120,583,-102,586,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,588});
    states[583] = new State(new int[]{5,584,13,582});
    states[584] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369},new int[]{-120,585,-102,586,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,588});
    states[585] = new State(new int[]{13,582,70,-630,92,-630,10,-630,98,-630,101,-630,33,-630,104,-630,2,-630,9,-630,12,-630,100,-630,99,-630,32,-630,85,-630,84,-630,83,-630,82,-630,81,-630,86,-630,6,-630,76,-630,5,-630,51,-630,58,-630,141,-630,143,-630,161,-630,80,-630,78,-630,160,-630,87,-630,45,-630,42,-630,8,-630,21,-630,22,-630,144,-630,147,-630,145,-630,146,-630,155,-630,158,-630,157,-630,156,-630,11,-630,57,-630,91,-630,40,-630,25,-630,97,-630,54,-630,35,-630,55,-630,102,-630,47,-630,36,-630,53,-630,60,-630,74,-630,72,-630,38,-630,71,-630});
    states[586] = new State(new int[]{16,31,5,-616,13,-616,70,-616,92,-616,10,-616,98,-616,101,-616,33,-616,104,-616,2,-616,9,-616,12,-616,100,-616,99,-616,32,-616,85,-616,84,-616,83,-616,82,-616,81,-616,86,-616,6,-616,76,-616,51,-616,58,-616,141,-616,143,-616,161,-616,80,-616,78,-616,160,-616,87,-616,45,-616,42,-616,8,-616,21,-616,22,-616,144,-616,147,-616,145,-616,146,-616,155,-616,158,-616,157,-616,156,-616,11,-616,57,-616,91,-616,40,-616,25,-616,97,-616,54,-616,35,-616,55,-616,102,-616,47,-616,36,-616,53,-616,60,-616,74,-616,72,-616,38,-616,71,-616});
    states[587] = new State(new int[]{6,35,120,-652,125,-652,123,-652,121,-652,124,-652,122,-652,137,-652,135,-652,16,-652,70,-652,92,-652,10,-652,98,-652,101,-652,33,-652,104,-652,2,-652,9,-652,12,-652,100,-652,99,-652,32,-652,85,-652,84,-652,83,-652,82,-652,81,-652,86,-652,76,-652,13,-652,5,-652,51,-652,58,-652,141,-652,143,-652,161,-652,80,-652,78,-652,160,-652,87,-652,45,-652,42,-652,8,-652,21,-652,22,-652,144,-652,147,-652,145,-652,146,-652,155,-652,158,-652,157,-652,156,-652,11,-652,57,-652,91,-652,40,-652,25,-652,97,-652,54,-652,35,-652,55,-652,102,-652,47,-652,36,-652,53,-652,60,-652,74,-652,72,-652,38,-652,71,-652,116,-652,115,-652,128,-652,129,-652,126,-652,138,-652,136,-652,118,-652,117,-652,131,-652,132,-652,133,-652,134,-652,130,-652});
    states[588] = new State(-617);
    states[589] = new State(-615);
    states[590] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-121,591,-102,596,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-246,597});
    states[591] = new State(new int[]{51,592});
    states[592] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-121,593,-102,596,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-246,597});
    states[593] = new State(new int[]{32,594});
    states[594] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-121,595,-102,596,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-246,597});
    states[595] = new State(-631);
    states[596] = new State(new int[]{16,31,51,-618,32,-618,120,-618,125,-618,123,-618,121,-618,124,-618,122,-618,137,-618,135,-618,70,-618,92,-618,10,-618,98,-618,101,-618,33,-618,104,-618,2,-618,9,-618,12,-618,100,-618,99,-618,85,-618,84,-618,83,-618,82,-618,81,-618,86,-618,13,-618,6,-618,76,-618,5,-618,58,-618,141,-618,143,-618,161,-618,80,-618,78,-618,160,-618,87,-618,45,-618,42,-618,8,-618,21,-618,22,-618,144,-618,147,-618,145,-618,146,-618,155,-618,158,-618,157,-618,156,-618,11,-618,57,-618,91,-618,40,-618,25,-618,97,-618,54,-618,35,-618,55,-618,102,-618,47,-618,36,-618,53,-618,60,-618,74,-618,72,-618,38,-618,71,-618,116,-618,115,-618,128,-618,129,-618,126,-618,138,-618,136,-618,118,-618,117,-618,131,-618,132,-618,133,-618,134,-618,130,-618});
    states[597] = new State(-619);
    states[598] = new State(-611);
    states[599] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,5,-712,70,-712,92,-712,10,-712,98,-712,101,-712,33,-712,104,-712,2,-712,9,-712,12,-712,100,-712,99,-712,32,-712,84,-712,83,-712,82,-712,81,-712,6,-712},new int[]{-118,600,-108,604,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,603,-271,579});
    states[600] = new State(new int[]{5,601,70,-716,92,-716,10,-716,98,-716,101,-716,33,-716,104,-716,2,-716,9,-716,12,-716,100,-716,99,-716,32,-716,85,-716,84,-716,83,-716,82,-716,81,-716,86,-716,6,-716,76,-716});
    states[601] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369},new int[]{-108,602,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,603,-271,579});
    states[602] = new State(new int[]{6,35,70,-718,92,-718,10,-718,98,-718,101,-718,33,-718,104,-718,2,-718,9,-718,12,-718,100,-718,99,-718,32,-718,85,-718,84,-718,83,-718,82,-718,81,-718,86,-718,76,-718});
    states[603] = new State(-743);
    states[604] = new State(new int[]{6,35,5,-711,70,-711,92,-711,10,-711,98,-711,101,-711,33,-711,104,-711,2,-711,9,-711,12,-711,100,-711,99,-711,32,-711,85,-711,84,-711,83,-711,82,-711,81,-711,86,-711,76,-711});
    states[605] = new State(new int[]{8,566,5,-475},new int[]{-131,606});
    states[606] = new State(new int[]{5,607});
    states[607] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,608,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[608] = new State(-294);
    states[609] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-138,610,-150,611,-154,50,-155,53});
    states[610] = new State(-489);
    states[611] = new State(-490);
    states[612] = new State(-488);
    states[613] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-162,614,-138,612,-150,611,-154,50,-155,53});
    states[614] = new State(new int[]{5,615,100,609});
    states[615] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,616,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[616] = new State(new int[]{110,617,9,-482,10,-482});
    states[617] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,618,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[618] = new State(new int[]{70,28,9,-486,10,-486});
    states[619] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-162,620,-138,612,-150,611,-154,50,-155,53});
    states[620] = new State(new int[]{5,621,100,609});
    states[621] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,622,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[622] = new State(new int[]{110,623,9,-483,10,-483});
    states[623] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,624,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[624] = new State(new int[]{70,28,9,-487,10,-487});
    states[625] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-162,626,-138,612,-150,611,-154,50,-155,53});
    states[626] = new State(new int[]{5,627,100,609});
    states[627] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,628,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[628] = new State(-484);
    states[629] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-255,630,-11,1155,-12,634,-183,635,-150,1150,-154,50,-155,53,-304,1153});
    states[630] = new State(new int[]{12,631,100,632});
    states[631] = new State(-220);
    states[632] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-11,633,-12,634,-183,635,-150,1150,-154,50,-155,53,-304,1153});
    states[633] = new State(-222);
    states[634] = new State(-223);
    states[635] = new State(new int[]{7,183,8,638,123,188,12,-648,100,-648},new int[]{-72,636,-302,637});
    states[636] = new State(-788);
    states[637] = new State(-241);
    states[638] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,834,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683,9,-818},new int[]{-69,639,-74,641,-91,397,-90,27,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,835,-103,660,-327,678});
    states[639] = new State(new int[]{9,640});
    states[640] = new State(-649);
    states[641] = new State(new int[]{100,154,12,-817,9,-817});
    states[642] = new State(new int[]{127,643,11,-798,17,-798,8,-798,7,-798,142,-798,4,-798,15,-798,138,-798,136,-798,118,-798,117,-798,131,-798,132,-798,133,-798,134,-798,130,-798,116,-798,115,-798,128,-798,129,-798,126,-798,6,-798,5,-798,120,-798,125,-798,123,-798,121,-798,124,-798,122,-798,137,-798,135,-798,16,-798,70,-798,92,-798,10,-798,98,-798,101,-798,33,-798,104,-798,2,-798,9,-798,12,-798,100,-798,99,-798,32,-798,85,-798,84,-798,83,-798,82,-798,81,-798,86,-798,13,-798,119,-798});
    states[643] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,644,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[644] = new State(-995);
    states[645] = new State(-1032);
    states[646] = new State(new int[]{16,31,92,-622,10,-622,98,-622,101,-622,33,-622,104,-622,2,-622,9,-622,12,-622,100,-622,99,-622,32,-622,85,-622,84,-622,83,-622,82,-622,81,-622,86,-622,13,-616});
    states[647] = new State(new int[]{53,688,9,715,56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,1125,21,295,22,300,76,447,11,369,40,590,5,599,18,670},new int[]{-90,439,-363,441,-105,648,-150,1084,-7,1122,-104,1104,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,1124,-135,392,-114,400,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-362,1105,-103,1106});
    states[648] = new State(new int[]{100,649});
    states[649] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,18,670},new int[]{-82,650,-105,1149,-104,1148,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-103,1106});
    states[650] = new State(new int[]{100,1146,5,547,10,-1015,9,-1015},new int[]{-328,651});
    states[651] = new State(new int[]{10,539,9,-1003},new int[]{-335,652});
    states[652] = new State(new int[]{9,653});
    states[653] = new State(new int[]{5,662,7,-767,138,-767,136,-767,118,-767,117,-767,131,-767,132,-767,133,-767,134,-767,130,-767,116,-767,115,-767,128,-767,129,-767,126,-767,6,-767,120,-767,125,-767,123,-767,121,-767,124,-767,122,-767,137,-767,135,-767,16,-767,70,-767,92,-767,10,-767,98,-767,101,-767,33,-767,104,-767,2,-767,9,-767,12,-767,100,-767,99,-767,32,-767,85,-767,84,-767,83,-767,82,-767,81,-767,86,-767,13,-767,127,-1017},new int[]{-339,654,-329,655});
    states[654] = new State(-1000);
    states[655] = new State(new int[]{127,656});
    states[656] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,657,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[657] = new State(-1005);
    states[658] = new State(new int[]{92,-623,10,-623,98,-623,101,-623,33,-623,104,-623,2,-623,9,-623,12,-623,100,-623,99,-623,32,-623,85,-623,84,-623,83,-623,82,-623,81,-623,86,-623,13,-617});
    states[659] = new State(-624);
    states[660] = new State(new int[]{5,662,127,-1017},new int[]{-339,661,-329,655});
    states[661] = new State(-1001);
    states[662] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,357,142,490,24,495,48,503,49,550,34,554,73,558},new int[]{-281,663,-276,664,-96,195,-109,315,-110,316,-183,665,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-260,666,-253,667,-285,668,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-304,669});
    states[663] = new State(-1018);
    states[664] = new State(-492);
    states[665] = new State(new int[]{7,183,123,188,8,-262,118,-262,117,-262,131,-262,132,-262,133,-262,134,-262,130,-262,6,-262,116,-262,115,-262,128,-262,129,-262,127,-262},new int[]{-302,637});
    states[666] = new State(-493);
    states[667] = new State(-494);
    states[668] = new State(-495);
    states[669] = new State(-496);
    states[670] = new State(new int[]{18,670,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-26,671,-25,677,-103,675,-150,676,-154,50,-155,53});
    states[671] = new State(new int[]{100,672});
    states[672] = new State(new int[]{18,670,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-25,673,-103,675,-150,676,-154,50,-155,53});
    states[673] = new State(new int[]{9,674,100,-990});
    states[674] = new State(-986);
    states[675] = new State(-987);
    states[676] = new State(-988);
    states[677] = new State(-989);
    states[678] = new State(-1002);
    states[679] = new State(new int[]{8,1136,5,662,127,-1017},new int[]{-329,680});
    states[680] = new State(new int[]{127,681});
    states[681] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,682,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[682] = new State(-1006);
    states[683] = new State(new int[]{127,684,8,1128});
    states[684] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,687,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-333,685,-215,686,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-7,1126,-334,1127,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[685] = new State(-1009);
    states[686] = new State(-1034);
    states[687] = new State(new int[]{53,688,56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,1125,21,295,22,300,76,447,11,369,40,590,5,599,18,670},new int[]{-90,439,-363,441,-105,533,-114,695,-7,1122,-104,1104,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,1124,-135,392,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-362,1105,-103,1106});
    states[688] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,689,-154,50,-155,53});
    states[689] = new State(new int[]{110,690});
    states[690] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-102,691,-90,693,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-104,156,-244,580,-120,581,-246,589,-123,598});
    states[691] = new State(new int[]{9,692,16,31,10,-613,70,-613,13,-616});
    states[692] = new State(-785);
    states[693] = new State(new int[]{10,694,70,28});
    states[694] = new State(-794);
    states[695] = new State(new int[]{100,696,11,394,17,401,8,703,7,1109,142,1111,4,1112,15,1119,138,-783,136,-783,118,-783,117,-783,131,-783,132,-783,133,-783,134,-783,130,-783,116,-783,115,-783,128,-783,129,-783,126,-783,6,-783,5,-783,120,-783,125,-783,123,-783,121,-783,124,-783,122,-783,137,-783,135,-783,16,-783,9,-783,70,-783,13,-783,119,-783,110,-783,111,-783,112,-783,113,-783,114,-783});
    states[696] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,1114,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369},new int[]{-340,697,-114,1118,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729});
    states[697] = new State(new int[]{9,698,100,701});
    states[698] = new State(new int[]{110,430,111,431,112,432,113,433,114,434},new int[]{-197,699});
    states[699] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,700,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[700] = new State(new int[]{70,28,92,-528,10,-528,98,-528,101,-528,33,-528,104,-528,2,-528,9,-528,12,-528,100,-528,99,-528,32,-528,85,-528,84,-528,83,-528,82,-528,81,-528,86,-528});
    states[701] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,1114,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369},new int[]{-114,702,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729});
    states[702] = new State(new int[]{11,394,17,401,8,703,7,1109,142,1111,4,1112,9,-530,100,-530});
    states[703] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,713,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683,9,-820},new int[]{-70,704,-75,706,-92,1108,-90,709,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,710,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,1107,-103,660,-327,678});
    states[704] = new State(new int[]{9,705});
    states[705] = new State(-797);
    states[706] = new State(new int[]{100,707,9,-819});
    states[707] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,713,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683},new int[]{-92,708,-90,709,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,710,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,1107,-103,660,-327,678});
    states[708] = new State(-600);
    states[709] = new State(new int[]{70,28,100,-606,9,-606});
    states[710] = new State(new int[]{110,711,127,643,11,-798,17,-798,8,-798,7,-798,142,-798,4,-798,15,-798,138,-798,136,-798,118,-798,117,-798,131,-798,132,-798,133,-798,134,-798,130,-798,116,-798,115,-798,128,-798,129,-798,126,-798,6,-798,5,-798,120,-798,125,-798,123,-798,121,-798,124,-798,122,-798,137,-798,135,-798,16,-798,70,-798,100,-798,9,-798,13,-798,119,-798});
    states[711] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,712,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[712] = new State(-607);
    states[713] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,100,-609,9,-609},new int[]{-150,437,-154,50,-155,53});
    states[714] = new State(new int[]{53,688,9,715,56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599,18,670},new int[]{-90,439,-363,441,-105,648,-150,1084,-104,1104,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-362,1105,-103,1106});
    states[715] = new State(new int[]{5,662,127,-1017},new int[]{-329,716});
    states[716] = new State(new int[]{127,717});
    states[717] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,718,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[718] = new State(-996);
    states[719] = new State(-1033);
    states[720] = new State(-1019);
    states[721] = new State(-1020);
    states[722] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,723,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[723] = new State(new int[]{51,724});
    states[724] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499,9,-499,12,-499,100,-499,99,-499,32,-499,84,-499,83,-499,82,-499,81,-499},new int[]{-264,725,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[725] = new State(new int[]{32,726,92,-538,10,-538,98,-538,101,-538,33,-538,104,-538,2,-538,9,-538,12,-538,100,-538,99,-538,85,-538,84,-538,83,-538,82,-538,81,-538,86,-538});
    states[726] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499,9,-499,12,-499,100,-499,99,-499,32,-499,84,-499,83,-499,82,-499,81,-499},new int[]{-264,727,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[727] = new State(-539);
    states[728] = new State(new int[]{7,45,11,151,17,381});
    states[729] = new State(new int[]{7,454});
    states[730] = new State(-501);
    states[731] = new State(-502);
    states[732] = new State(new int[]{155,734,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-146,733,-150,735,-154,50,-155,53});
    states[733] = new State(-534);
    states[734] = new State(-105);
    states[735] = new State(-106);
    states[736] = new State(-503);
    states[737] = new State(-504);
    states[738] = new State(-505);
    states[739] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,740,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[740] = new State(new int[]{58,741});
    states[741] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340,32,749,92,-558},new int[]{-38,742,-257,1081,-266,1083,-77,1074,-113,1080,-97,1079,-93,206,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[742] = new State(new int[]{10,745,32,749,92,-558},new int[]{-257,743});
    states[743] = new State(new int[]{92,744});
    states[744] = new State(-549);
    states[745] = new State(new int[]{32,749,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340,92,-558},new int[]{-257,746,-266,748,-77,1074,-113,1080,-97,1079,-93,206,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[746] = new State(new int[]{92,747});
    states[747] = new State(-550);
    states[748] = new State(-553);
    states[749] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499,92,-499},new int[]{-256,750,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[750] = new State(new int[]{10,20,92,-559});
    states[751] = new State(-536);
    states[752] = new State(new int[]{11,-798,17,-798,8,-798,7,-798,142,-798,4,-798,15,-798,110,-798,111,-798,112,-798,113,-798,114,-798,92,-798,10,-798,98,-798,101,-798,33,-798,104,-798,2,-798,5,-106});
    states[753] = new State(new int[]{7,-197,11,-197,17,-197,5,-105});
    states[754] = new State(-506);
    states[755] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,98,-499,10,-499},new int[]{-256,756,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[756] = new State(new int[]{98,757,10,20});
    states[757] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,758,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[758] = new State(new int[]{70,28,92,-560,10,-560,98,-560,101,-560,33,-560,104,-560,2,-560,9,-560,12,-560,100,-560,99,-560,32,-560,85,-560,84,-560,83,-560,82,-560,81,-560,86,-560});
    states[759] = new State(-507);
    states[760] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,761,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[761] = new State(new int[]{99,1070,141,-563,143,-563,161,-563,85,-563,86,-563,80,-563,78,-563,160,-563,87,-563,45,-563,42,-563,8,-563,21,-563,22,-563,144,-563,147,-563,145,-563,146,-563,155,-563,158,-563,157,-563,156,-563,76,-563,11,-563,57,-563,91,-563,40,-563,25,-563,97,-563,54,-563,35,-563,55,-563,102,-563,47,-563,36,-563,53,-563,60,-563,74,-563,72,-563,38,-563,92,-563,10,-563,98,-563,101,-563,33,-563,104,-563,2,-563,9,-563,12,-563,100,-563,32,-563,84,-563,83,-563,82,-563,81,-563},new int[]{-295,762});
    states[762] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499,9,-499,12,-499,100,-499,99,-499,32,-499,84,-499,83,-499,82,-499,81,-499},new int[]{-264,763,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[763] = new State(-561);
    states[764] = new State(-508);
    states[765] = new State(new int[]{53,1073,143,-570,161,-570,85,-570,86,-570,80,-570,78,-570,160,-570,87,-570},new int[]{-21,766});
    states[766] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,767,-154,50,-155,53});
    states[767] = new State(new int[]{5,1023,110,-568},new int[]{-278,768});
    states[768] = new State(new int[]{110,769});
    states[769] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,770,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[770] = new State(new int[]{70,1071,71,1072},new int[]{-122,771});
    states[771] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,772,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[772] = new State(new int[]{160,1066,99,1070,141,-563,143,-563,161,-563,85,-563,86,-563,80,-563,78,-563,87,-563,45,-563,42,-563,8,-563,21,-563,22,-563,144,-563,147,-563,145,-563,146,-563,155,-563,158,-563,157,-563,156,-563,76,-563,11,-563,57,-563,91,-563,40,-563,25,-563,97,-563,54,-563,35,-563,55,-563,102,-563,47,-563,36,-563,53,-563,60,-563,74,-563,72,-563,38,-563,92,-563,10,-563,98,-563,101,-563,33,-563,104,-563,2,-563,9,-563,12,-563,100,-563,32,-563,84,-563,83,-563,82,-563,81,-563},new int[]{-295,773});
    states[773] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499,9,-499,12,-499,100,-499,99,-499,32,-499,84,-499,83,-499,82,-499,81,-499},new int[]{-264,774,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[774] = new State(-576);
    states[775] = new State(-509);
    states[776] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,834,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683},new int[]{-74,777,-91,397,-90,27,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,835,-103,660,-327,678});
    states[777] = new State(new int[]{99,778,100,154});
    states[778] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499,9,-499,12,-499,100,-499,99,-499,32,-499,84,-499,83,-499,82,-499,81,-499},new int[]{-264,779,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[779] = new State(-578);
    states[780] = new State(-510);
    states[781] = new State(-511);
    states[782] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499,101,-499,33,-499},new int[]{-256,783,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[783] = new State(new int[]{10,20,101,785,33,1044},new int[]{-293,784});
    states[784] = new State(-580);
    states[785] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499},new int[]{-256,786,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[786] = new State(new int[]{92,787,10,20});
    states[787] = new State(-581);
    states[788] = new State(-512);
    states[789] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599,92,-595,10,-595,98,-595,101,-595,33,-595,104,-595,2,-595,9,-595,12,-595,100,-595,99,-595,32,-595,84,-595,83,-595,82,-595,81,-595},new int[]{-90,790,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[790] = new State(new int[]{70,28,92,-596,10,-596,98,-596,101,-596,33,-596,104,-596,2,-596,9,-596,12,-596,100,-596,99,-596,32,-596,85,-596,84,-596,83,-596,82,-596,81,-596,86,-596});
    states[791] = new State(-513);
    states[792] = new State(new int[]{53,1025,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,793,-154,50,-155,53});
    states[793] = new State(new int[]{5,1023,137,-568},new int[]{-278,794});
    states[794] = new State(new int[]{137,795});
    states[795] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,796,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[796] = new State(new int[]{87,1021,99,-566},new int[]{-364,797});
    states[797] = new State(new int[]{99,798});
    states[798] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499,9,-499,12,-499,100,-499,99,-499,32,-499,84,-499,83,-499,82,-499,81,-499},new int[]{-264,799,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[799] = new State(-573);
    states[800] = new State(-514);
    states[801] = new State(new int[]{8,803,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-315,802,-161,811,-150,810,-154,50,-155,53});
    states[802] = new State(-524);
    states[803] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,804,-154,50,-155,53});
    states[804] = new State(new int[]{100,805});
    states[805] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-161,806,-150,810,-154,50,-155,53});
    states[806] = new State(new int[]{9,807,100,545});
    states[807] = new State(new int[]{110,808});
    states[808] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,809,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[809] = new State(new int[]{70,28,92,-526,10,-526,98,-526,101,-526,33,-526,104,-526,2,-526,9,-526,12,-526,100,-526,99,-526,32,-526,85,-526,84,-526,83,-526,82,-526,81,-526,86,-526});
    states[810] = new State(-350);
    states[811] = new State(new int[]{5,812,100,545,110,1019});
    states[812] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,813,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[813] = new State(new int[]{110,1017,120,1018,92,-414,10,-414,98,-414,101,-414,33,-414,104,-414,2,-414,9,-414,12,-414,100,-414,99,-414,32,-414,85,-414,84,-414,83,-414,82,-414,81,-414,86,-414},new int[]{-342,814});
    states[814] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,988,135,842,116,354,115,355,63,340,37,679,44,683,40,590},new int[]{-89,815,-88,816,-87,277,-93,278,-94,237,-84,817,-16,252,-13,262,-17,223,-150,857,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856,-98,1005,-247,1006,-327,1015,-246,1016});
    states[815] = new State(-416);
    states[816] = new State(-417);
    states[817] = new State(new int[]{6,818,116,248,115,249,128,250,129,251,120,-124,125,-124,123,-124,121,-124,124,-124,122,-124,137,-124,135,-124,13,-124,16,-124,92,-124,10,-124,98,-124,101,-124,33,-124,104,-124,2,-124,9,-124,12,-124,100,-124,99,-124,32,-124,85,-124,84,-124,83,-124,82,-124,81,-124,86,-124},new int[]{-196,215});
    states[818] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-16,819,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850});
    states[819] = new State(new int[]{136,253,138,254,118,255,117,256,131,257,132,258,133,259,134,260,130,261,92,-418,10,-418,98,-418,101,-418,33,-418,104,-418,2,-418,9,-418,12,-418,100,-418,99,-418,32,-418,85,-418,84,-418,83,-418,82,-418,81,-418,86,-418},new int[]{-204,217,-198,220});
    states[820] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-71,821,-80,372,-95,450,-90,375,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[821] = new State(new int[]{76,822});
    states[822] = new State(-173);
    states[823] = new State(-164);
    states[824] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,836,135,842,116,354,115,355,63,340},new int[]{-13,825,-17,826,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,844,-176,846,-59,847});
    states[825] = new State(-165);
    states[826] = new State(new int[]{4,225,11,227,7,827,142,829,8,830,136,-162,138,-162,118,-162,117,-162,131,-162,132,-162,133,-162,134,-162,130,-162,116,-162,115,-162,128,-162,129,-162,120,-162,125,-162,123,-162,121,-162,124,-162,122,-162,137,-162,135,-162,13,-162,16,-162,6,-162,100,-162,9,-162,12,-162,5,-162,92,-162,10,-162,98,-162,101,-162,33,-162,104,-162,2,-162,99,-162,32,-162,85,-162,84,-162,83,-162,82,-162,81,-162,86,-162},new int[]{-15,224});
    states[827] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,84,60,83,61,82,62,81,63,68,64,64,65,128,66,22,67,21,68,63,69,23,70,129,71,130,72,131,73,132,74,133,75,134,76,135,77,136,78,137,79,138,80,24,81,73,82,91,83,25,84,26,85,29,86,30,87,31,88,71,89,99,90,32,91,92,92,33,93,34,94,27,95,104,96,101,97,35,98,36,99,37,100,40,101,41,102,42,103,103,104,43,105,44,106,46,107,47,108,48,109,97,110,49,111,102,112,50,113,28,114,51,115,70,116,98,117,52,118,53,119,54,120,55,121,56,122,57,123,58,124,59,125,61,126,105,127,106,128,109,129,107,130,108,131,62,132,74,133,38,134,39,135,69,136,148,137,60,138,139,139,140,140,79,141,153,142,152,143,72,144,154,145,150,146,151,147,149,148,45,150},new int[]{-141,828,-150,47,-154,50,-155,53,-296,58,-153,59,-297,149});
    states[828] = new State(-185);
    states[829] = new State(-186);
    states[830] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,834,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683,9,-190},new int[]{-79,831,-74,833,-91,397,-90,27,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,835,-103,660,-327,678});
    states[831] = new State(new int[]{9,832});
    states[832] = new State(-187);
    states[833] = new State(new int[]{100,154,9,-189});
    states[834] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,92,-605,10,-605,98,-605,101,-605,33,-605,104,-605,2,-605,9,-605,12,-605,100,-605,99,-605,32,-605,84,-605,83,-605,82,-605,81,-605},new int[]{-150,437,-154,50,-155,53});
    states[835] = new State(-604);
    states[836] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-93,837,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[837] = new State(new int[]{9,838,13,207,16,211});
    states[838] = new State(-166);
    states[839] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-93,840,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[840] = new State(new int[]{9,841,13,207,16,211});
    states[841] = new State(new int[]{136,-166,138,-166,118,-166,117,-166,131,-166,132,-166,133,-166,134,-166,130,-166,116,-166,115,-166,128,-166,129,-166,120,-166,125,-166,123,-166,121,-166,124,-166,122,-166,137,-166,135,-166,13,-166,16,-166,6,-166,100,-166,9,-166,12,-166,5,-166,92,-166,10,-166,98,-166,101,-166,33,-166,104,-166,2,-166,99,-166,32,-166,85,-166,84,-166,83,-166,82,-166,81,-166,86,-166,119,-161});
    states[842] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,836,135,842,116,354,115,355,63,340},new int[]{-13,843,-17,826,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,844,-176,846,-59,847});
    states[843] = new State(-167);
    states[844] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,836,135,842,116,354,115,355,63,340},new int[]{-13,845,-17,826,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,844,-176,846,-59,847});
    states[845] = new State(-168);
    states[846] = new State(-169);
    states[847] = new State(-170);
    states[848] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-13,845,-273,849,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-14,850});
    states[849] = new State(-147);
    states[850] = new State(new int[]{119,851});
    states[851] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-13,852,-273,853,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-14,850});
    states[852] = new State(-145);
    states[853] = new State(-146);
    states[854] = new State(-149);
    states[855] = new State(-150);
    states[856] = new State(-127);
    states[857] = new State(new int[]{127,858,4,-176,11,-176,7,-176,142,-176,8,-176,136,-176,138,-176,118,-176,117,-176,131,-176,132,-176,133,-176,134,-176,130,-176,6,-176,116,-176,115,-176,128,-176,129,-176,120,-176,125,-176,123,-176,121,-176,124,-176,122,-176,137,-176,135,-176,13,-176,16,-176,92,-176,10,-176,98,-176,101,-176,33,-176,104,-176,2,-176,9,-176,12,-176,100,-176,99,-176,32,-176,85,-176,84,-176,83,-176,82,-176,81,-176,86,-176,119,-176});
    states[858] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,859,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[859] = new State(-420);
    states[860] = new State(-1021);
    states[861] = new State(-1022);
    states[862] = new State(-1023);
    states[863] = new State(-1024);
    states[864] = new State(-1025);
    states[865] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,866,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[866] = new State(new int[]{99,867});
    states[867] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499,9,-499,12,-499,100,-499,99,-499,32,-499,84,-499,83,-499,82,-499,81,-499},new int[]{-264,868,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[868] = new State(-521);
    states[869] = new State(-515);
    states[870] = new State(-601);
    states[871] = new State(-602);
    states[872] = new State(-516);
    states[873] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,874,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[874] = new State(new int[]{99,875});
    states[875] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499,9,-499,12,-499,100,-499,99,-499,32,-499,84,-499,83,-499,82,-499,81,-499},new int[]{-264,876,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[876] = new State(-564);
    states[877] = new State(-517);
    states[878] = new State(new int[]{73,880,56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,714,21,295,22,300,76,447,11,369,40,590,18,670,37,679,44,683},new int[]{-106,879,-104,882,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-326,883,-103,660,-327,678});
    states[879] = new State(-522);
    states[880] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,714,21,295,22,300,76,447,11,369,40,590,18,670,37,679,44,683},new int[]{-106,881,-104,882,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-326,883,-103,660,-327,678});
    states[881] = new State(-523);
    states[882] = new State(-620);
    states[883] = new State(-621);
    states[884] = new State(-518);
    states[885] = new State(-519);
    states[886] = new State(-520);
    states[887] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,888,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[888] = new State(new int[]{55,889});
    states[889] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,56,967,21,295,22,300,11,927,8,940},new int[]{-354,890,-353,981,-346,897,-287,902,-183,182,-150,219,-154,50,-155,53,-345,959,-361,962,-343,970,-18,965,-168,337,-170,280,-169,284,-19,338,-261,968,-298,969,-347,971,-348,974});
    states[890] = new State(new int[]{10,893,32,749,92,-558},new int[]{-257,891});
    states[891] = new State(new int[]{92,892});
    states[892] = new State(-540);
    states[893] = new State(new int[]{32,749,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,56,967,21,295,22,300,11,927,8,940,92,-558},new int[]{-257,894,-353,896,-346,897,-287,902,-183,182,-150,219,-154,50,-155,53,-345,959,-361,962,-343,970,-18,965,-168,337,-170,280,-169,284,-19,338,-261,968,-298,969,-347,971,-348,974});
    states[894] = new State(new int[]{92,895});
    states[895] = new State(-541);
    states[896] = new State(-543);
    states[897] = new State(new int[]{39,898});
    states[898] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,899,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[899] = new State(new int[]{5,900});
    states[900] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499,32,-499,92,-499},new int[]{-264,901,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[901] = new State(-544);
    states[902] = new State(new int[]{8,903,100,-663,5,-663});
    states[903] = new State(new int[]{14,908,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,116,354,115,355,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,53,915,11,927,8,940},new int[]{-358,904,-356,958,-18,909,-168,337,-170,280,-169,284,-19,338,-202,910,-150,912,-154,50,-155,53,-346,919,-287,920,-183,182,-347,926,-348,957});
    states[904] = new State(new int[]{9,905,10,906,100,924});
    states[905] = new State(new int[]{39,-657,5,-658});
    states[906] = new State(new int[]{14,908,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,116,354,115,355,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,53,915,11,927,8,940},new int[]{-356,907,-18,909,-168,337,-170,280,-169,284,-19,338,-202,910,-150,912,-154,50,-155,53,-346,919,-287,920,-183,182,-347,926,-348,957});
    states[907] = new State(-689);
    states[908] = new State(-701);
    states[909] = new State(-702);
    states[910] = new State(new int[]{144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291},new int[]{-18,911,-168,337,-170,280,-169,284,-19,338});
    states[911] = new State(-703);
    states[912] = new State(new int[]{5,913,9,-705,10,-705,100,-705,7,-267,4,-267,123,-267,8,-267});
    states[913] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,914,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[914] = new State(-704);
    states[915] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,916,-154,50,-155,53});
    states[916] = new State(new int[]{5,917,9,-707,10,-707,100,-707});
    states[917] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,918,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[918] = new State(-706);
    states[919] = new State(-708);
    states[920] = new State(new int[]{8,921});
    states[921] = new State(new int[]{14,908,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,116,354,115,355,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,53,915,11,927,8,940},new int[]{-358,922,-356,958,-18,909,-168,337,-170,280,-169,284,-19,338,-202,910,-150,912,-154,50,-155,53,-346,919,-287,920,-183,182,-347,926,-348,957});
    states[922] = new State(new int[]{9,923,10,906,100,924});
    states[923] = new State(-657);
    states[924] = new State(new int[]{14,908,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,116,354,115,355,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,53,915,11,927,8,940},new int[]{-356,925,-18,909,-168,337,-170,280,-169,284,-19,338,-202,910,-150,912,-154,50,-155,53,-346,919,-287,920,-183,182,-347,926,-348,957});
    states[925] = new State(-690);
    states[926] = new State(-709);
    states[927] = new State(new int[]{144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,53,934,14,936,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,11,927,8,940,6,955},new int[]{-359,928,-349,956,-18,932,-168,337,-170,280,-169,284,-19,338,-351,933,-346,937,-287,920,-183,182,-150,219,-154,50,-155,53,-347,938,-348,939});
    states[928] = new State(new int[]{12,929,100,930});
    states[929] = new State(-667);
    states[930] = new State(new int[]{144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,53,934,14,936,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,11,927,8,940,6,955},new int[]{-349,931,-18,932,-168,337,-170,280,-169,284,-19,338,-351,933,-346,937,-287,920,-183,182,-150,219,-154,50,-155,53,-347,938,-348,939});
    states[931] = new State(-669);
    states[932] = new State(-670);
    states[933] = new State(-671);
    states[934] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,935,-154,50,-155,53});
    states[935] = new State(-677);
    states[936] = new State(-672);
    states[937] = new State(-673);
    states[938] = new State(-674);
    states[939] = new State(-675);
    states[940] = new State(new int[]{14,945,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,116,354,115,355,53,949,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,11,927,8,940},new int[]{-360,941,-350,954,-18,946,-168,337,-170,280,-169,284,-19,338,-202,947,-346,951,-287,920,-183,182,-150,219,-154,50,-155,53,-347,952,-348,953});
    states[941] = new State(new int[]{9,942,100,943});
    states[942] = new State(-678);
    states[943] = new State(new int[]{14,945,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,116,354,115,355,53,949,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,11,927,8,940},new int[]{-350,944,-18,946,-168,337,-170,280,-169,284,-19,338,-202,947,-346,951,-287,920,-183,182,-150,219,-154,50,-155,53,-347,952,-348,953});
    states[944] = new State(-687);
    states[945] = new State(-679);
    states[946] = new State(-680);
    states[947] = new State(new int[]{144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291},new int[]{-18,948,-168,337,-170,280,-169,284,-19,338});
    states[948] = new State(-681);
    states[949] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,950,-154,50,-155,53});
    states[950] = new State(-682);
    states[951] = new State(-683);
    states[952] = new State(-684);
    states[953] = new State(-685);
    states[954] = new State(-686);
    states[955] = new State(-676);
    states[956] = new State(-668);
    states[957] = new State(-710);
    states[958] = new State(-688);
    states[959] = new State(new int[]{5,960});
    states[960] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499,32,-499,92,-499},new int[]{-264,961,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[961] = new State(-545);
    states[962] = new State(new int[]{100,963,5,-659});
    states[963] = new State(new int[]{144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,56,967,21,295,22,300},new int[]{-343,964,-18,965,-168,337,-170,280,-169,284,-19,338,-287,966,-183,182,-150,219,-154,50,-155,53,-261,968,-298,969});
    states[964] = new State(-661);
    states[965] = new State(-662);
    states[966] = new State(-663);
    states[967] = new State(-664);
    states[968] = new State(-665);
    states[969] = new State(-666);
    states[970] = new State(-660);
    states[971] = new State(new int[]{5,972});
    states[972] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499,32,-499,92,-499},new int[]{-264,973,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[973] = new State(-546);
    states[974] = new State(new int[]{39,975,5,979});
    states[975] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,976,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[976] = new State(new int[]{5,977});
    states[977] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499,32,-499,92,-499},new int[]{-264,978,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[978] = new State(-547);
    states[979] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499,32,-499,92,-499},new int[]{-264,980,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[980] = new State(-548);
    states[981] = new State(-542);
    states[982] = new State(-1026);
    states[983] = new State(-1027);
    states[984] = new State(-1028);
    states[985] = new State(-1029);
    states[986] = new State(-1030);
    states[987] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,714,21,295,22,300,76,447,11,369,40,590,18,670,37,679,44,683},new int[]{-106,879,-104,882,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-326,883,-103,660,-327,678});
    states[988] = new State(new int[]{9,996,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,1001,135,842,116,354,115,355,63,340},new int[]{-93,989,-68,990,-249,994,-94,237,-84,247,-16,252,-13,262,-17,223,-150,1000,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856,-67,274,-88,1004,-87,277,-98,1005,-247,1006,-248,1007,-250,1014,-139,1010});
    states[989] = new State(new int[]{9,841,13,207,16,211,100,-201});
    states[990] = new State(new int[]{9,991});
    states[991] = new State(new int[]{127,992,92,-204,10,-204,98,-204,101,-204,33,-204,104,-204,2,-204,9,-204,12,-204,100,-204,99,-204,32,-204,85,-204,84,-204,83,-204,82,-204,81,-204,86,-204});
    states[992] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,993,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[993] = new State(-422);
    states[994] = new State(new int[]{9,995});
    states[995] = new State(-209);
    states[996] = new State(new int[]{5,547,127,-1015},new int[]{-328,997});
    states[997] = new State(new int[]{127,998});
    states[998] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,999,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[999] = new State(-421);
    states[1000] = new State(new int[]{4,-176,11,-176,7,-176,142,-176,8,-176,136,-176,138,-176,118,-176,117,-176,131,-176,132,-176,133,-176,134,-176,130,-176,116,-176,115,-176,128,-176,129,-176,120,-176,125,-176,123,-176,121,-176,124,-176,122,-176,137,-176,135,-176,9,-176,13,-176,16,-176,100,-176,119,-176,5,-215});
    states[1001] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,1001,135,842,116,354,115,355,63,340,9,-205},new int[]{-93,989,-68,1002,-249,994,-94,237,-84,247,-16,252,-13,262,-17,223,-150,1000,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856,-67,274,-88,1004,-87,277,-98,1005,-247,1006,-248,1007,-250,1014,-139,1010});
    states[1002] = new State(new int[]{9,1003});
    states[1003] = new State(-204);
    states[1004] = new State(-207);
    states[1005] = new State(-202);
    states[1006] = new State(-203);
    states[1007] = new State(new int[]{10,1008,9,-210});
    states[1008] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,9,-211},new int[]{-250,1009,-139,1010,-150,1013,-154,50,-155,53});
    states[1009] = new State(-213);
    states[1010] = new State(new int[]{5,1011});
    states[1011] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,1001,135,842,116,354,115,355,63,340},new int[]{-87,1012,-93,278,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856,-98,1005,-247,1006});
    states[1012] = new State(-214);
    states[1013] = new State(-215);
    states[1014] = new State(-212);
    states[1015] = new State(-419);
    states[1016] = new State(-423);
    states[1017] = new State(-412);
    states[1018] = new State(-413);
    states[1019] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,834,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683},new int[]{-91,1020,-90,27,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,835,-103,660,-327,678});
    states[1020] = new State(-415);
    states[1021] = new State(new int[]{143,1022});
    states[1022] = new State(-565);
    states[1023] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,1024,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1024] = new State(-567);
    states[1025] = new State(new int[]{8,1033,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,1026,-154,50,-155,53});
    states[1026] = new State(new int[]{5,1023,137,-568},new int[]{-278,1027});
    states[1027] = new State(new int[]{137,1028});
    states[1028] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,1029,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[1029] = new State(new int[]{87,1021,99,-566},new int[]{-364,1030});
    states[1030] = new State(new int[]{99,1031});
    states[1031] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499,9,-499,12,-499,100,-499,99,-499,32,-499,84,-499,83,-499,82,-499,81,-499},new int[]{-264,1032,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1032] = new State(-574);
    states[1033] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-161,1034,-150,810,-154,50,-155,53});
    states[1034] = new State(new int[]{9,1035,100,545});
    states[1035] = new State(new int[]{137,1036});
    states[1036] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,1037,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[1037] = new State(new int[]{87,1021,99,-566},new int[]{-364,1038});
    states[1038] = new State(new int[]{99,1039});
    states[1039] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499,9,-499,12,-499,100,-499,99,-499,32,-499,84,-499,83,-499,82,-499,81,-499},new int[]{-264,1040,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1040] = new State(-575);
    states[1041] = new State(new int[]{5,1042});
    states[1042] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499},new int[]{-265,1043,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[1043] = new State(-498);
    states[1044] = new State(new int[]{79,1052,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499,92,-499},new int[]{-62,1045,-65,1047,-64,1064,-256,1065,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[1045] = new State(new int[]{92,1046});
    states[1046] = new State(-582);
    states[1047] = new State(new int[]{10,1049,32,1062,92,-588},new int[]{-258,1048});
    states[1048] = new State(-583);
    states[1049] = new State(new int[]{79,1052,32,1062,92,-588},new int[]{-64,1050,-258,1051});
    states[1050] = new State(-587);
    states[1051] = new State(-584);
    states[1052] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-66,1053,-182,1056,-183,1057,-150,1058,-154,50,-155,53,-143,1059});
    states[1053] = new State(new int[]{99,1054});
    states[1054] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499,32,-499,92,-499},new int[]{-264,1055,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1055] = new State(-590);
    states[1056] = new State(-591);
    states[1057] = new State(new int[]{7,183,99,-593});
    states[1058] = new State(new int[]{7,-267,99,-267,5,-594});
    states[1059] = new State(new int[]{5,1060});
    states[1060] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-182,1061,-183,1057,-150,219,-154,50,-155,53});
    states[1061] = new State(-592);
    states[1062] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499,92,-499},new int[]{-256,1063,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[1063] = new State(new int[]{10,20,92,-589});
    states[1064] = new State(-586);
    states[1065] = new State(new int[]{10,20,92,-585});
    states[1066] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,1067,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[1067] = new State(new int[]{99,1068});
    states[1068] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499,98,-499,101,-499,33,-499,104,-499,2,-499,9,-499,12,-499,100,-499,99,-499,32,-499,84,-499,83,-499,82,-499,81,-499},new int[]{-264,1069,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1069] = new State(-577);
    states[1070] = new State(-562);
    states[1071] = new State(-571);
    states[1072] = new State(-572);
    states[1073] = new State(-569);
    states[1074] = new State(new int[]{5,1075,100,1077});
    states[1075] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499,32,-499,92,-499},new int[]{-264,1076,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1076] = new State(-554);
    states[1077] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-113,1078,-97,1079,-93,206,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[1078] = new State(-556);
    states[1079] = new State(-557);
    states[1080] = new State(-555);
    states[1081] = new State(new int[]{92,1082});
    states[1082] = new State(-551);
    states[1083] = new State(-552);
    states[1084] = new State(new int[]{5,1085,10,1098,11,-798,17,-798,8,-798,7,-798,142,-798,4,-798,15,-798,110,-798,111,-798,112,-798,113,-798,114,-798,138,-798,136,-798,118,-798,117,-798,131,-798,132,-798,133,-798,134,-798,130,-798,116,-798,115,-798,128,-798,129,-798,126,-798,6,-798,120,-798,125,-798,123,-798,121,-798,124,-798,122,-798,137,-798,135,-798,16,-798,9,-798,70,-798,100,-798,13,-798,119,-798});
    states[1085] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,1086,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1086] = new State(new int[]{9,1087,10,1091});
    states[1087] = new State(new int[]{5,662,127,-1017},new int[]{-329,1088});
    states[1088] = new State(new int[]{127,1089});
    states[1089] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,1090,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[1090] = new State(-997);
    states[1091] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-330,1092,-331,1097,-161,543,-150,810,-154,50,-155,53});
    states[1092] = new State(new int[]{9,1093,10,541});
    states[1093] = new State(new int[]{5,662,127,-1017},new int[]{-329,1094});
    states[1094] = new State(new int[]{127,1095});
    states[1095] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,1096,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[1096] = new State(-999);
    states[1097] = new State(-1012);
    states[1098] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-330,1099,-331,1097,-161,543,-150,810,-154,50,-155,53});
    states[1099] = new State(new int[]{9,1100,10,541});
    states[1100] = new State(new int[]{5,662,127,-1017},new int[]{-329,1101});
    states[1101] = new State(new int[]{127,1102});
    states[1102] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,1103,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[1103] = new State(-998);
    states[1104] = new State(new int[]{9,-610,70,-610,100,-991});
    states[1105] = new State(-795);
    states[1106] = new State(-992);
    states[1107] = new State(-608);
    states[1108] = new State(-599);
    states[1109] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,84,60,83,61,82,62,81,63,68,64,64,65,128,66,22,67,21,68,63,69,23,70,129,71,130,72,131,73,132,74,133,75,134,76,135,77,136,78,137,79,138,80,24,81,73,82,91,83,25,84,26,85,29,86,30,87,31,88,71,89,99,90,32,91,92,92,33,93,34,94,27,95,104,96,101,97,35,98,36,99,37,100,40,101,41,102,42,103,103,104,43,105,44,106,46,107,47,108,48,109,97,110,49,111,102,112,50,113,28,114,51,115,70,116,98,117,52,118,53,119,54,120,55,121,56,122,57,123,58,124,59,125,61,126,105,127,106,128,109,129,107,130,108,131,62,132,74,133,38,134,39,135,69,136,148,137,60,138,139,139,140,140,79,141,153,142,152,143,72,144,154,145,150,146,151,147,149,148,45,406},new int[]{-151,1110,-150,456,-154,50,-155,53,-296,457,-153,59,-194,458});
    states[1110] = new State(-813);
    states[1111] = new State(-815);
    states[1112] = new State(new int[]{123,188},new int[]{-302,1113});
    states[1113] = new State(-816);
    states[1114] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599,53,1115,18,670},new int[]{-90,439,-363,441,-105,533,-104,1104,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-362,1105,-103,1106});
    states[1115] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,1116,-154,50,-155,53});
    states[1116] = new State(new int[]{110,1117});
    states[1117] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,693,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[1118] = new State(new int[]{11,394,17,401,8,703,7,1109,142,1111,4,1112,9,-529,100,-529});
    states[1119] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,1114,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369},new int[]{-114,1120,-119,1121,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729});
    states[1120] = new State(new int[]{11,394,17,401,8,703,7,1109,142,1111,4,1112,15,1119,110,-780,111,-780,112,-780,113,-780,114,-780,92,-780,10,-780,98,-780,101,-780,33,-780,104,-780,2,-780,138,-780,136,-780,118,-780,117,-780,131,-780,132,-780,133,-780,134,-780,130,-780,116,-780,115,-780,128,-780,129,-780,126,-780,6,-780,5,-780,120,-780,125,-780,123,-780,121,-780,124,-780,122,-780,137,-780,135,-780,16,-780,70,-780,9,-780,12,-780,100,-780,99,-780,32,-780,85,-780,84,-780,83,-780,82,-780,81,-780,86,-780,13,-780,119,-780,76,-780,51,-780,58,-780,141,-780,143,-780,161,-780,80,-780,78,-780,160,-780,87,-780,45,-780,42,-780,21,-780,22,-780,144,-780,147,-780,145,-780,146,-780,155,-780,158,-780,157,-780,156,-780,57,-780,91,-780,40,-780,25,-780,97,-780,54,-780,35,-780,55,-780,102,-780,47,-780,36,-780,53,-780,60,-780,74,-780,72,-780,38,-780,71,-780});
    states[1121] = new State(-781);
    states[1122] = new State(new int[]{9,1123});
    states[1123] = new State(-1031);
    states[1124] = new State(new int[]{110,430,111,431,112,432,113,433,114,434,138,-776,136,-776,118,-776,117,-776,131,-776,132,-776,133,-776,134,-776,130,-776,116,-776,115,-776,128,-776,129,-776,126,-776,6,-776,5,-776,120,-776,125,-776,123,-776,121,-776,124,-776,122,-776,137,-776,135,-776,16,-776,9,-776,70,-776,100,-776,13,-776,2,-776,119,-769},new int[]{-197,25});
    states[1125] = new State(new int[]{53,688,56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599,18,670},new int[]{-90,439,-363,441,-105,533,-114,695,-104,1104,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-362,1105,-103,1106});
    states[1126] = new State(-1035);
    states[1127] = new State(-1036);
    states[1128] = new State(new int[]{9,1129,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-330,1132,-331,1097,-161,543,-150,810,-154,50,-155,53});
    states[1129] = new State(new int[]{127,1130});
    states[1130] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,687,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-333,1131,-215,686,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-7,1126,-334,1127,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[1131] = new State(-1010);
    states[1132] = new State(new int[]{9,1133,10,541});
    states[1133] = new State(new int[]{127,1134});
    states[1134] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,42,436,8,687,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-333,1135,-215,686,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-7,1126,-334,1127,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[1135] = new State(-1011);
    states[1136] = new State(new int[]{9,1137,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-330,1141,-331,1097,-161,543,-150,810,-154,50,-155,53});
    states[1137] = new State(new int[]{5,662,127,-1017},new int[]{-329,1138});
    states[1138] = new State(new int[]{127,1139});
    states[1139] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,1140,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[1140] = new State(-1007);
    states[1141] = new State(new int[]{9,1142,10,541});
    states[1142] = new State(new int[]{5,662,127,-1017},new int[]{-329,1143});
    states[1143] = new State(new int[]{127,1144});
    states[1144] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,647,21,295,22,300,76,447,11,369,18,670,37,679,44,683,91,17,40,722,54,760,97,755,35,765,36,792,72,865,25,739,102,782,60,873,47,789,74,987},new int[]{-332,1145,-107,645,-102,646,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,658,-120,581,-326,659,-103,660,-327,678,-334,719,-259,720,-156,721,-322,860,-251,861,-127,862,-126,863,-128,864,-37,982,-305,983,-172,984,-252,985,-129,986});
    states[1145] = new State(-1008);
    states[1146] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,18,670},new int[]{-105,1147,-104,1148,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-103,1106});
    states[1147] = new State(-994);
    states[1148] = new State(-991);
    states[1149] = new State(-993);
    states[1150] = new State(new int[]{5,1151,7,-267,8,-267,123,-267,12,-267,100,-267});
    states[1151] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-12,1152,-183,635,-150,219,-154,50,-155,53,-304,1153});
    states[1152] = new State(-224);
    states[1153] = new State(new int[]{8,638,12,-648,100,-648},new int[]{-72,1154});
    states[1154] = new State(-789);
    states[1155] = new State(-221);
    states[1156] = new State(-217);
    states[1157] = new State(-478);
    states[1158] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,1159,-154,50,-155,53});
    states[1159] = new State(new int[]{110,690,100,1160});
    states[1160] = new State(new int[]{53,1168},new int[]{-341,1161});
    states[1161] = new State(new int[]{9,1162,100,1165});
    states[1162] = new State(new int[]{110,1163});
    states[1163] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,1164,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[1164] = new State(new int[]{70,28,92,-525,10,-525,98,-525,101,-525,33,-525,104,-525,2,-525,9,-525,12,-525,100,-525,99,-525,32,-525,85,-525,84,-525,83,-525,82,-525,81,-525,86,-525});
    states[1165] = new State(new int[]{53,1166});
    states[1166] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,1167,-154,50,-155,53});
    states[1167] = new State(-532);
    states[1168] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,1169,-154,50,-155,53});
    states[1169] = new State(-531);
    states[1170] = new State(new int[]{148,1174,150,1175,151,1176,152,1177,154,1178,153,1179,107,-835,91,-835,59,-835,29,-835,66,-835,50,-835,53,-835,62,-835,11,-835,28,-835,26,-835,44,-835,37,-835,19,-835,30,-835,31,-835,46,-835,27,-835,92,-835,84,-835,83,-835,82,-835,81,-835,23,-835,149,-835,41,-835},new int[]{-209,1171,-212,1180});
    states[1171] = new State(new int[]{10,1172});
    states[1172] = new State(new int[]{148,1174,150,1175,151,1176,152,1177,154,1178,153,1179,107,-836,91,-836,59,-836,29,-836,66,-836,50,-836,53,-836,62,-836,11,-836,28,-836,26,-836,44,-836,37,-836,19,-836,30,-836,31,-836,46,-836,27,-836,92,-836,84,-836,83,-836,82,-836,81,-836,23,-836,149,-836,41,-836},new int[]{-212,1173});
    states[1173] = new State(-840);
    states[1174] = new State(-853);
    states[1175] = new State(-854);
    states[1176] = new State(-855);
    states[1177] = new State(-856);
    states[1178] = new State(-857);
    states[1179] = new State(-858);
    states[1180] = new State(-839);
    states[1181] = new State(-383);
    states[1182] = new State(-452);
    states[1183] = new State(-453);
    states[1184] = new State(new int[]{8,-458,110,-458,10,-458,11,-458,5,-458,7,-455});
    states[1185] = new State(new int[]{123,1187,8,-461,110,-461,10,-461,7,-461,11,-461,5,-461},new int[]{-158,1186});
    states[1186] = new State(-462);
    states[1187] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-161,1188,-150,810,-154,50,-155,53});
    states[1188] = new State(new int[]{121,1189,100,545});
    states[1189] = new State(-328);
    states[1190] = new State(-463);
    states[1191] = new State(new int[]{123,1187,8,-459,110,-459,10,-459,11,-459,5,-459},new int[]{-158,1192});
    states[1192] = new State(-460);
    states[1193] = new State(new int[]{7,1194});
    states[1194] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406},new int[]{-145,1195,-152,1196,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191});
    states[1195] = new State(-454);
    states[1196] = new State(-457);
    states[1197] = new State(-456);
    states[1198] = new State(-438);
    states[1199] = new State(new int[]{44,1329,37,1364},new int[]{-219,1200,-231,1201,-228,1328,-232,1363});
    states[1200] = new State(-440);
    states[1201] = new State(new int[]{107,1319,59,-76,29,-76,66,-76,50,-76,53,-76,62,-76,91,-76},new int[]{-179,1202,-45,1203,-41,1206,-63,1318});
    states[1202] = new State(-446);
    states[1203] = new State(new int[]{91,17},new int[]{-259,1204});
    states[1204] = new State(new int[]{10,1205});
    states[1205] = new State(-473);
    states[1206] = new State(new int[]{59,1209,29,1230,66,1234,50,1446,53,1461,62,1463,91,-75},new int[]{-48,1207,-171,1208,-31,1215,-54,1232,-292,1236,-313,1448});
    states[1207] = new State(-77);
    states[1208] = new State(-93);
    states[1209] = new State(new int[]{155,734,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-159,1210,-146,1214,-150,735,-154,50,-155,53});
    states[1210] = new State(new int[]{10,1211,100,1212});
    states[1211] = new State(-102);
    states[1212] = new State(new int[]{155,734,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-146,1213,-150,735,-154,50,-155,53});
    states[1213] = new State(-104);
    states[1214] = new State(-103);
    states[1215] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,59,-94,29,-94,66,-94,50,-94,53,-94,62,-94,91,-94},new int[]{-29,1216,-30,1217,-144,1219,-150,1229,-154,50,-155,53});
    states[1216] = new State(-108);
    states[1217] = new State(new int[]{10,1218});
    states[1218] = new State(-118);
    states[1219] = new State(new int[]{120,1220,5,1225});
    states[1220] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,1223,135,842,116,354,115,355,63,340},new int[]{-112,1221,-93,1222,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856,-98,1224});
    states[1221] = new State(-119);
    states[1222] = new State(new int[]{13,207,16,211,10,-121,92,-121,84,-121,83,-121,82,-121,81,-121});
    states[1223] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,1001,135,842,116,354,115,355,63,340,9,-205},new int[]{-93,989,-68,1002,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856,-67,274,-88,1004,-87,277,-98,1005,-247,1006});
    states[1224] = new State(-122);
    states[1225] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,1226,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1226] = new State(new int[]{120,1227});
    states[1227] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,1001,135,842,116,354,115,355,63,340},new int[]{-87,1228,-93,278,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856,-98,1005,-247,1006});
    states[1228] = new State(-120);
    states[1229] = new State(-123);
    states[1230] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-29,1231,-30,1217,-144,1219,-150,1229,-154,50,-155,53});
    states[1231] = new State(-107);
    states[1232] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,59,-95,29,-95,66,-95,50,-95,53,-95,62,-95,91,-95},new int[]{-29,1233,-30,1217,-144,1219,-150,1229,-154,50,-155,53});
    states[1233] = new State(-110);
    states[1234] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-29,1235,-30,1217,-144,1219,-150,1229,-154,50,-155,53});
    states[1235] = new State(-109);
    states[1236] = new State(new int[]{11,629,59,-96,29,-96,66,-96,50,-96,53,-96,62,-96,91,-96,143,-219,161,-219,85,-219,86,-219,80,-219,78,-219,160,-219,87,-219},new int[]{-51,1237,-9,1238,-254,1156});
    states[1237] = new State(-112);
    states[1238] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,11,629},new int[]{-52,1239,-254,519,-147,1240,-150,1438,-154,50,-155,53,-148,1443});
    states[1239] = new State(-216);
    states[1240] = new State(new int[]{120,1241});
    states[1241] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605,68,1432,69,1433,148,1434,27,1435,28,1436,26,-310,43,-310,64,-310},new int[]{-290,1242,-280,1244,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563,-32,1245,-23,1246,-24,1430,-22,1437});
    states[1242] = new State(new int[]{10,1243});
    states[1243] = new State(-225);
    states[1244] = new State(-230);
    states[1245] = new State(-231);
    states[1246] = new State(new int[]{26,1424,43,1425,64,1426},new int[]{-294,1247});
    states[1247] = new State(new int[]{8,1345,23,-322,11,-322,92,-322,84,-322,83,-322,82,-322,81,-322,29,-322,143,-322,161,-322,85,-322,86,-322,80,-322,78,-322,160,-322,87,-322,62,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322,10,-322},new int[]{-186,1248});
    states[1248] = new State(new int[]{23,1336,11,-329,92,-329,84,-329,83,-329,82,-329,81,-329,29,-329,143,-329,161,-329,85,-329,86,-329,80,-329,78,-329,160,-329,87,-329,62,-329,28,-329,26,-329,44,-329,37,-329,19,-329,30,-329,31,-329,46,-329,27,-329,10,-329},new int[]{-321,1249,-320,1334,-319,1356});
    states[1249] = new State(new int[]{11,629,10,-320,92,-346,84,-346,83,-346,82,-346,81,-346,29,-219,143,-219,161,-219,85,-219,86,-219,80,-219,78,-219,160,-219,87,-219,62,-219,28,-219,26,-219,44,-219,37,-219,19,-219,30,-219,31,-219,46,-219,27,-219},new int[]{-28,1250,-27,1251,-34,1257,-36,510,-47,1258,-9,1259,-254,1156,-35,1421,-56,1423,-55,516,-57,1422});
    states[1250] = new State(-303);
    states[1251] = new State(new int[]{92,1252,84,1253,83,1254,82,1255,81,1256},new int[]{-10,508});
    states[1252] = new State(-321);
    states[1253] = new State(-342);
    states[1254] = new State(-343);
    states[1255] = new State(-344);
    states[1256] = new State(-345);
    states[1257] = new State(-340);
    states[1258] = new State(-354);
    states[1259] = new State(new int[]{29,1261,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,62,1265,28,1381,26,1382,11,629,44,1329,37,1364,19,1384,30,1392,31,1399,46,1406,27,1415},new int[]{-53,1260,-254,519,-225,518,-222,520,-262,521,-316,1263,-315,1264,-161,811,-150,810,-154,50,-155,53,-6,1269,-233,1383,-231,1317,-228,1328,-232,1363,-230,1390,-218,1403,-219,1404,-221,1405});
    states[1260] = new State(-356);
    states[1261] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-30,1262,-144,1219,-150,1229,-154,50,-155,53});
    states[1262] = new State(-361);
    states[1263] = new State(-362);
    states[1264] = new State(-366);
    states[1265] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-161,1266,-150,810,-154,50,-155,53});
    states[1266] = new State(new int[]{5,1267,100,545});
    states[1267] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,1268,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1268] = new State(-367);
    states[1269] = new State(new int[]{30,524,19,1199,46,1271,27,1309,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,62,1265,44,1329,37,1364},new int[]{-316,1270,-233,523,-219,1198,-315,1264,-161,811,-150,810,-154,50,-155,53,-231,1317,-228,1328,-232,1363});
    states[1270] = new State(-363);
    states[1271] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406},new int[]{-173,1272,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1272] = new State(new int[]{11,1300,5,-397},new int[]{-236,1273,-241,1297});
    states[1273] = new State(new int[]{85,1286,86,1292,10,-404},new int[]{-205,1274});
    states[1274] = new State(new int[]{10,1275});
    states[1275] = new State(new int[]{63,1280,153,1282,152,1283,148,1284,151,1285,11,-394,28,-394,26,-394,44,-394,37,-394,19,-394,30,-394,31,-394,46,-394,27,-394,92,-394,84,-394,83,-394,82,-394,81,-394},new int[]{-208,1276,-213,1277});
    states[1276] = new State(-388);
    states[1277] = new State(new int[]{10,1278});
    states[1278] = new State(new int[]{63,1280,11,-394,28,-394,26,-394,44,-394,37,-394,19,-394,30,-394,31,-394,46,-394,27,-394,92,-394,84,-394,83,-394,82,-394,81,-394},new int[]{-208,1279});
    states[1279] = new State(-389);
    states[1280] = new State(new int[]{10,1281});
    states[1281] = new State(-395);
    states[1282] = new State(-859);
    states[1283] = new State(-860);
    states[1284] = new State(-861);
    states[1285] = new State(-862);
    states[1286] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,834,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683,10,-403},new int[]{-117,1287,-91,1291,-90,27,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,835,-103,660,-327,678});
    states[1287] = new State(new int[]{86,1289,10,-407},new int[]{-206,1288});
    states[1288] = new State(-405);
    states[1289] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499},new int[]{-264,1290,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1290] = new State(-408);
    states[1291] = new State(-402);
    states[1292] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499},new int[]{-264,1293,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1293] = new State(new int[]{85,1295,10,-409},new int[]{-207,1294});
    states[1294] = new State(-406);
    states[1295] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,834,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683,10,-403},new int[]{-117,1296,-91,1291,-90,27,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,835,-103,660,-327,678});
    states[1296] = new State(-410);
    states[1297] = new State(new int[]{5,1298});
    states[1298] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,1299,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1299] = new State(-396);
    states[1300] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-240,1301,-239,1308,-161,1305,-150,810,-154,50,-155,53});
    states[1301] = new State(new int[]{12,1302,10,1303});
    states[1302] = new State(-398);
    states[1303] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-239,1304,-161,1305,-150,810,-154,50,-155,53});
    states[1304] = new State(-400);
    states[1305] = new State(new int[]{5,1306,100,545});
    states[1306] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,1307,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1307] = new State(-401);
    states[1308] = new State(-399);
    states[1309] = new State(new int[]{46,1310});
    states[1310] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406},new int[]{-173,1311,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1311] = new State(new int[]{11,1300,5,-397},new int[]{-236,1312,-241,1297});
    states[1312] = new State(new int[]{110,1315,10,-393},new int[]{-214,1313});
    states[1313] = new State(new int[]{10,1314});
    states[1314] = new State(-391);
    states[1315] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,1316,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[1316] = new State(new int[]{70,28,10,-392});
    states[1317] = new State(new int[]{107,1319,11,-377,28,-377,26,-377,44,-377,37,-377,19,-377,30,-377,31,-377,46,-377,27,-377,92,-377,84,-377,83,-377,82,-377,81,-377,59,-76,29,-76,66,-76,50,-76,53,-76,62,-76,91,-76},new int[]{-179,1202,-45,1203,-41,1206,-63,1318});
    states[1318] = new State(-474);
    states[1319] = new State(new int[]{10,1327,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,144,282,147,283,145,285,146,286},new int[]{-111,1320,-150,1324,-154,50,-155,53,-168,1325,-170,280,-169,284});
    states[1320] = new State(new int[]{80,1321,10,1326});
    states[1321] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,144,282,147,283,145,285,146,286},new int[]{-111,1322,-150,1324,-154,50,-155,53,-168,1325,-170,280,-169,284});
    states[1322] = new State(new int[]{10,1323});
    states[1323] = new State(-467);
    states[1324] = new State(-470);
    states[1325] = new State(-471);
    states[1326] = new State(-468);
    states[1327] = new State(-469);
    states[1328] = new State(-378);
    states[1329] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406},new int[]{-174,1330,-173,1182,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1330] = new State(new int[]{8,566,10,-475,110,-475},new int[]{-131,1331});
    states[1331] = new State(new int[]{10,1361,110,-837},new int[]{-210,1332,-211,1357});
    states[1332] = new State(new int[]{23,1336,107,-329,91,-329,59,-329,29,-329,66,-329,50,-329,53,-329,62,-329,11,-329,28,-329,26,-329,44,-329,37,-329,19,-329,30,-329,31,-329,46,-329,27,-329,92,-329,84,-329,83,-329,82,-329,81,-329,149,-329,41,-329},new int[]{-321,1333,-320,1334,-319,1356});
    states[1333] = new State(-464);
    states[1334] = new State(new int[]{23,1336,11,-330,92,-330,84,-330,83,-330,82,-330,81,-330,29,-330,143,-330,161,-330,85,-330,86,-330,80,-330,78,-330,160,-330,87,-330,62,-330,28,-330,26,-330,44,-330,37,-330,19,-330,30,-330,31,-330,46,-330,27,-330,10,-330,107,-330,91,-330,59,-330,66,-330,50,-330,53,-330,149,-330,41,-330},new int[]{-319,1335});
    states[1335] = new State(-332);
    states[1336] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-161,1337,-150,810,-154,50,-155,53});
    states[1337] = new State(new int[]{5,1338,100,545});
    states[1338] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,1344,49,550,34,554,73,558,44,564,37,605,26,1353,30,1354},new int[]{-291,1339,-289,1355,-280,1343,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1339] = new State(new int[]{10,1340,100,1341});
    states[1340] = new State(-333);
    states[1341] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,1344,49,550,34,554,73,558,44,564,37,605,26,1353,30,1354},new int[]{-289,1342,-280,1343,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1342] = new State(-335);
    states[1343] = new State(-336);
    states[1344] = new State(new int[]{8,1345,10,-338,100,-338,23,-322,11,-322,92,-322,84,-322,83,-322,82,-322,81,-322,29,-322,143,-322,161,-322,85,-322,86,-322,80,-322,78,-322,160,-322,87,-322,62,-322,28,-322,26,-322,44,-322,37,-322,19,-322,30,-322,31,-322,46,-322,27,-322},new int[]{-186,504});
    states[1345] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-185,1346,-184,1352,-183,1350,-150,219,-154,50,-155,53,-304,1351});
    states[1346] = new State(new int[]{9,1347,100,1348});
    states[1347] = new State(-323);
    states[1348] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-184,1349,-183,1350,-150,219,-154,50,-155,53,-304,1351});
    states[1349] = new State(-325);
    states[1350] = new State(new int[]{7,183,123,188,9,-326,100,-326},new int[]{-302,637});
    states[1351] = new State(-327);
    states[1352] = new State(-324);
    states[1353] = new State(-337);
    states[1354] = new State(-339);
    states[1355] = new State(-334);
    states[1356] = new State(-331);
    states[1357] = new State(new int[]{110,1358});
    states[1358] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499},new int[]{-264,1359,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1359] = new State(new int[]{10,1360});
    states[1360] = new State(-449);
    states[1361] = new State(new int[]{148,1174,150,1175,151,1176,152,1177,154,1178,153,1179,23,-835,107,-835,91,-835,59,-835,29,-835,66,-835,50,-835,53,-835,62,-835,11,-835,28,-835,26,-835,44,-835,37,-835,19,-835,30,-835,31,-835,46,-835,27,-835,92,-835,84,-835,83,-835,82,-835,81,-835,149,-835},new int[]{-209,1362,-212,1180});
    states[1362] = new State(new int[]{10,1172,110,-838});
    states[1363] = new State(-379);
    states[1364] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406},new int[]{-173,1365,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1365] = new State(new int[]{8,566,5,-475,10,-475,110,-475},new int[]{-131,1366});
    states[1366] = new State(new int[]{5,1369,10,1361,110,-837},new int[]{-210,1367,-211,1377});
    states[1367] = new State(new int[]{23,1336,107,-329,91,-329,59,-329,29,-329,66,-329,50,-329,53,-329,62,-329,11,-329,28,-329,26,-329,44,-329,37,-329,19,-329,30,-329,31,-329,46,-329,27,-329,92,-329,84,-329,83,-329,82,-329,81,-329,149,-329,41,-329},new int[]{-321,1368,-320,1334,-319,1356});
    states[1368] = new State(-465);
    states[1369] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,1370,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1370] = new State(new int[]{10,1361,110,-837},new int[]{-210,1371,-211,1373});
    states[1371] = new State(new int[]{23,1336,107,-329,91,-329,59,-329,29,-329,66,-329,50,-329,53,-329,62,-329,11,-329,28,-329,26,-329,44,-329,37,-329,19,-329,30,-329,31,-329,46,-329,27,-329,92,-329,84,-329,83,-329,82,-329,81,-329,149,-329,41,-329},new int[]{-321,1372,-320,1334,-319,1356});
    states[1372] = new State(-466);
    states[1373] = new State(new int[]{110,1374});
    states[1374] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,714,21,295,22,300,76,447,11,369,40,590,18,670,37,679,44,683},new int[]{-106,1375,-104,882,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-326,883,-103,660,-327,678});
    states[1375] = new State(new int[]{10,1376});
    states[1376] = new State(-447);
    states[1377] = new State(new int[]{110,1378});
    states[1378] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,714,21,295,22,300,76,447,11,369,40,590,18,670,37,679,44,683},new int[]{-106,1379,-104,882,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-326,883,-103,660,-327,678});
    states[1379] = new State(new int[]{10,1380});
    states[1380] = new State(-448);
    states[1381] = new State(-364);
    states[1382] = new State(-365);
    states[1383] = new State(-373);
    states[1384] = new State(new int[]{28,1381,26,1382,44,1329,37,1364},new int[]{-6,1385,-233,1388,-219,1389,-231,1317,-228,1328,-232,1363});
    states[1385] = new State(new int[]{44,1329,37,1364},new int[]{-233,1386,-219,1387,-231,1317,-228,1328,-232,1363});
    states[1386] = new State(-374);
    states[1387] = new State(-439);
    states[1388] = new State(-375);
    states[1389] = new State(-437);
    states[1390] = new State(new int[]{107,1319,11,-376,28,-376,26,-376,44,-376,37,-376,19,-376,30,-376,31,-376,46,-376,27,-376,92,-376,84,-376,83,-376,82,-376,81,-376,59,-76,29,-76,66,-76,50,-76,53,-76,62,-76,91,-76},new int[]{-179,1391,-45,1203,-41,1206,-63,1318});
    states[1391] = new State(-428);
    states[1392] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,8,-384,110,-384,10,-384},new int[]{-175,1393,-174,1181,-173,1182,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1393] = new State(new int[]{8,566,110,-475,10,-475},new int[]{-131,1394});
    states[1394] = new State(new int[]{110,1396,10,1170},new int[]{-210,1395});
    states[1395] = new State(-380);
    states[1396] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499},new int[]{-264,1397,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1397] = new State(new int[]{10,1398});
    states[1398] = new State(-429);
    states[1399] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,8,-384,10,-384},new int[]{-175,1400,-174,1181,-173,1182,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1400] = new State(new int[]{8,566,10,-475},new int[]{-131,1401});
    states[1401] = new State(new int[]{10,1170},new int[]{-210,1402});
    states[1402] = new State(-382);
    states[1403] = new State(-370);
    states[1404] = new State(-436);
    states[1405] = new State(-371);
    states[1406] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406},new int[]{-173,1407,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1407] = new State(new int[]{11,1300,5,-397},new int[]{-236,1408,-241,1297});
    states[1408] = new State(new int[]{85,1286,86,1292,10,-404},new int[]{-205,1409});
    states[1409] = new State(new int[]{10,1410});
    states[1410] = new State(new int[]{63,1280,153,1282,152,1283,148,1284,151,1285,11,-394,28,-394,26,-394,44,-394,37,-394,19,-394,30,-394,31,-394,46,-394,27,-394,92,-394,84,-394,83,-394,82,-394,81,-394},new int[]{-208,1411,-213,1412});
    states[1411] = new State(-386);
    states[1412] = new State(new int[]{10,1413});
    states[1413] = new State(new int[]{63,1280,11,-394,28,-394,26,-394,44,-394,37,-394,19,-394,30,-394,31,-394,46,-394,27,-394,92,-394,84,-394,83,-394,82,-394,81,-394},new int[]{-208,1414});
    states[1414] = new State(-387);
    states[1415] = new State(new int[]{46,1416});
    states[1416] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406},new int[]{-173,1417,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1417] = new State(new int[]{11,1300,5,-397},new int[]{-236,1418,-241,1297});
    states[1418] = new State(new int[]{110,1315,10,-393},new int[]{-214,1419});
    states[1419] = new State(new int[]{10,1420});
    states[1420] = new State(-390);
    states[1421] = new State(new int[]{11,629,92,-348,84,-348,83,-348,82,-348,81,-348,28,-219,26,-219,44,-219,37,-219,19,-219,30,-219,31,-219,46,-219,27,-219},new int[]{-56,515,-55,516,-9,517,-254,1156,-57,1422});
    states[1422] = new State(-360);
    states[1423] = new State(-357);
    states[1424] = new State(-314);
    states[1425] = new State(-315);
    states[1426] = new State(new int[]{26,1427,48,1428,43,1429,8,-316,23,-316,11,-316,92,-316,84,-316,83,-316,82,-316,81,-316,29,-316,143,-316,161,-316,85,-316,86,-316,80,-316,78,-316,160,-316,87,-316,62,-316,28,-316,44,-316,37,-316,19,-316,30,-316,31,-316,46,-316,27,-316,10,-316});
    states[1427] = new State(-317);
    states[1428] = new State(-318);
    states[1429] = new State(-319);
    states[1430] = new State(new int[]{68,1432,69,1433,148,1434,27,1435,28,1436,26,-311,43,-311,64,-311},new int[]{-22,1431});
    states[1431] = new State(-313);
    states[1432] = new State(-305);
    states[1433] = new State(-306);
    states[1434] = new State(-307);
    states[1435] = new State(-308);
    states[1436] = new State(-309);
    states[1437] = new State(-312);
    states[1438] = new State(new int[]{123,1440,120,-227},new int[]{-158,1439});
    states[1439] = new State(-228);
    states[1440] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-161,1441,-150,810,-154,50,-155,53});
    states[1441] = new State(new int[]{122,1442,121,1189,100,545});
    states[1442] = new State(-229);
    states[1443] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605,68,1432,69,1433,148,1434,27,1435,28,1436,26,-310,43,-310,64,-310},new int[]{-290,1444,-280,1244,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563,-32,1245,-23,1246,-24,1430,-22,1437});
    states[1444] = new State(new int[]{10,1445});
    states[1445] = new State(-226);
    states[1446] = new State(new int[]{11,629,143,-219,161,-219,85,-219,86,-219,80,-219,78,-219,160,-219,87,-219},new int[]{-51,1447,-9,1238,-254,1156});
    states[1447] = new State(-111);
    states[1448] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,8,1453,59,-97,29,-97,66,-97,50,-97,53,-97,62,-97,91,-97},new int[]{-317,1449,-314,1450,-315,1451,-161,811,-150,810,-154,50,-155,53});
    states[1449] = new State(-117);
    states[1450] = new State(-113);
    states[1451] = new State(new int[]{10,1452});
    states[1452] = new State(-411);
    states[1453] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,1454,-154,50,-155,53});
    states[1454] = new State(new int[]{100,1455});
    states[1455] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-161,1456,-150,810,-154,50,-155,53});
    states[1456] = new State(new int[]{9,1457,100,545});
    states[1457] = new State(new int[]{110,1458});
    states[1458] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,1459,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[1459] = new State(new int[]{10,1460});
    states[1460] = new State(-114);
    states[1461] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,8,1453},new int[]{-317,1462,-314,1450,-315,1451,-161,811,-150,810,-154,50,-155,53});
    states[1462] = new State(-115);
    states[1463] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,8,1453},new int[]{-317,1464,-314,1450,-315,1451,-161,811,-150,810,-154,50,-155,53});
    states[1464] = new State(-116);
    states[1465] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,357,12,-286,100,-286},new int[]{-275,1466,-276,1467,-96,195,-109,315,-110,316,-183,353,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284});
    states[1466] = new State(-284);
    states[1467] = new State(-285);
    states[1468] = new State(-283);
    states[1469] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-280,1470,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1470] = new State(-282);
    states[1471] = new State(-252);
    states[1472] = new State(-253);
    states[1473] = new State(new int[]{127,484,121,-254,100,-254,120,-254,9,-254,10,-254,8,-254,138,-254,136,-254,118,-254,117,-254,131,-254,132,-254,133,-254,134,-254,130,-254,116,-254,115,-254,128,-254,129,-254,126,-254,6,-254,5,-254,125,-254,123,-254,124,-254,122,-254,137,-254,135,-254,16,-254,70,-254,92,-254,98,-254,101,-254,33,-254,104,-254,2,-254,12,-254,99,-254,32,-254,85,-254,84,-254,83,-254,82,-254,81,-254,86,-254,13,-254,76,-254,51,-254,58,-254,141,-254,143,-254,161,-254,80,-254,78,-254,160,-254,87,-254,45,-254,42,-254,21,-254,22,-254,144,-254,147,-254,145,-254,146,-254,155,-254,158,-254,157,-254,156,-254,11,-254,57,-254,91,-254,40,-254,25,-254,97,-254,54,-254,35,-254,55,-254,102,-254,47,-254,36,-254,53,-254,60,-254,74,-254,72,-254,38,-254,71,-254,110,-254});
    states[1474] = new State(-698);
    states[1475] = new State(new int[]{8,1476});
    states[1476] = new State(new int[]{14,473,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,53,475,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,11,927,8,940},new int[]{-357,1477,-355,1483,-18,474,-168,337,-170,280,-169,284,-19,338,-344,1474,-287,1475,-183,182,-150,219,-154,50,-155,53,-347,1481,-348,1482});
    states[1477] = new State(new int[]{9,1478,10,471,100,1479});
    states[1478] = new State(-656);
    states[1479] = new State(new int[]{14,473,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,53,475,143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,11,927,8,940},new int[]{-355,1480,-18,474,-168,337,-170,280,-169,284,-19,338,-344,1474,-287,1475,-183,182,-150,219,-154,50,-155,53,-347,1481,-348,1482});
    states[1480] = new State(-693);
    states[1481] = new State(-699);
    states[1482] = new State(-700);
    states[1483] = new State(-691);
    states[1484] = new State(-787);
    states[1485] = new State(new int[]{13,483});
    states[1486] = new State(-247);
    states[1487] = new State(-243);
    states[1488] = new State(-632);
    states[1489] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,713,8,714,21,295,22,300,76,447,11,369,40,590,5,599,18,670,37,679,44,683,9,-820},new int[]{-70,1490,-75,706,-92,1108,-90,709,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,710,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-326,1107,-103,660,-327,678});
    states[1490] = new State(new int[]{9,1491});
    states[1491] = new State(-651);
    states[1492] = new State(new int[]{8,1493});
    states[1493] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-337,1494,-336,1502,-150,1498,-154,50,-155,53,-104,1501,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[1494] = new State(new int[]{9,1495,100,1496});
    states[1495] = new State(-643);
    states[1496] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-336,1497,-150,1498,-154,50,-155,53,-104,1501,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[1497] = new State(-647);
    states[1498] = new State(new int[]{110,1499,11,-798,17,-798,8,-798,7,-798,142,-798,4,-798,15,-798,138,-798,136,-798,118,-798,117,-798,131,-798,132,-798,133,-798,134,-798,130,-798,116,-798,115,-798,128,-798,129,-798,126,-798,6,-798,120,-798,125,-798,123,-798,121,-798,124,-798,122,-798,137,-798,135,-798,16,-798,9,-798,100,-798,13,-798,119,-798});
    states[1499] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590},new int[]{-104,1500,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589});
    states[1500] = new State(-644);
    states[1501] = new State(-645);
    states[1502] = new State(-646);
    states[1503] = new State(new int[]{13,207,16,211,5,-713,12,-713});
    states[1504] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-93,1505,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[1505] = new State(new int[]{13,207,16,211,100,-196,9,-196,12,-196,5,-196});
    states[1506] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340,5,-714,12,-714},new int[]{-125,1507,-93,1503,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[1507] = new State(new int[]{5,1508,12,-720});
    states[1508] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,42,292,21,295,22,300,11,369,76,820,56,823,141,824,8,839,135,842,116,354,115,355,63,340},new int[]{-93,1509,-94,237,-84,247,-16,252,-13,262,-17,223,-150,263,-154,50,-155,53,-168,279,-170,280,-169,284,-19,287,-261,294,-298,299,-242,367,-243,368,-202,848,-176,846,-59,847,-269,854,-273,855,-14,850,-245,856});
    states[1509] = new State(new int[]{13,207,16,211,12,-722});
    states[1510] = new State(-193);
    states[1511] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286},new int[]{-96,1512,-109,315,-110,316,-183,353,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284});
    states[1512] = new State(new int[]{116,248,115,249,128,250,129,251,13,-256,121,-256,100,-256,120,-256,9,-256,10,-256,8,-256,138,-256,136,-256,118,-256,117,-256,131,-256,132,-256,133,-256,134,-256,130,-256,126,-256,6,-256,5,-256,125,-256,123,-256,124,-256,122,-256,137,-256,135,-256,16,-256,70,-256,92,-256,98,-256,101,-256,33,-256,104,-256,2,-256,12,-256,99,-256,32,-256,85,-256,84,-256,83,-256,82,-256,81,-256,86,-256,76,-256,51,-256,58,-256,141,-256,143,-256,161,-256,80,-256,78,-256,160,-256,87,-256,45,-256,42,-256,21,-256,22,-256,144,-256,147,-256,145,-256,146,-256,155,-256,158,-256,157,-256,156,-256,11,-256,57,-256,91,-256,40,-256,25,-256,97,-256,54,-256,35,-256,55,-256,102,-256,47,-256,36,-256,53,-256,60,-256,74,-256,72,-256,38,-256,71,-256,127,-256,110,-256},new int[]{-196,196});
    states[1513] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,24,495},new int[]{-287,1514,-282,1515,-183,182,-150,219,-154,50,-155,53,-274,501});
    states[1514] = new State(-745);
    states[1515] = new State(-746);
    states[1516] = new State(-759);
    states[1517] = new State(-760);
    states[1518] = new State(-761);
    states[1519] = new State(-762);
    states[1520] = new State(-763);
    states[1521] = new State(-764);
    states[1522] = new State(-765);
    states[1523] = new State(-735);
    states[1524] = new State(-654);
    states[1525] = new State(-35);
    states[1526] = new State(new int[]{59,1209,29,1230,66,1234,50,1446,53,1461,62,1463,11,629,91,-70,92,-70,103,-70,44,-219,37,-219,28,-219,26,-219,19,-219,30,-219,31,-219},new int[]{-49,1527,-171,1528,-31,1529,-54,1530,-292,1531,-313,1532,-223,1533,-9,1534,-254,1156});
    states[1527] = new State(-74);
    states[1528] = new State(-84);
    states[1529] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,59,-85,29,-85,66,-85,50,-85,53,-85,62,-85,11,-85,44,-85,37,-85,28,-85,26,-85,19,-85,30,-85,31,-85,91,-85,92,-85,103,-85},new int[]{-29,1216,-30,1217,-144,1219,-150,1229,-154,50,-155,53});
    states[1530] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,59,-86,29,-86,66,-86,50,-86,53,-86,62,-86,11,-86,44,-86,37,-86,28,-86,26,-86,19,-86,30,-86,31,-86,91,-86,92,-86,103,-86},new int[]{-29,1233,-30,1217,-144,1219,-150,1229,-154,50,-155,53});
    states[1531] = new State(new int[]{11,629,59,-87,29,-87,66,-87,50,-87,53,-87,62,-87,44,-87,37,-87,28,-87,26,-87,19,-87,30,-87,31,-87,91,-87,92,-87,103,-87,143,-219,161,-219,85,-219,86,-219,80,-219,78,-219,160,-219,87,-219},new int[]{-51,1237,-9,1238,-254,1156});
    states[1532] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,8,1453,59,-88,29,-88,66,-88,50,-88,53,-88,62,-88,11,-88,44,-88,37,-88,28,-88,26,-88,19,-88,30,-88,31,-88,91,-88,92,-88,103,-88},new int[]{-317,1449,-314,1450,-315,1451,-161,811,-150,810,-154,50,-155,53});
    states[1533] = new State(-89);
    states[1534] = new State(new int[]{44,1547,37,1554,28,1381,26,1382,19,1577,30,1584,31,1399,11,629},new int[]{-216,1535,-254,519,-217,1536,-224,1537,-231,1538,-228,1328,-232,1363,-6,1567,-220,1581,-230,1582});
    states[1535] = new State(-92);
    states[1536] = new State(-90);
    states[1537] = new State(-431);
    states[1538] = new State(new int[]{149,1540,107,1319,59,-73,29,-73,66,-73,50,-73,53,-73,62,-73,11,-73,44,-73,37,-73,28,-73,26,-73,19,-73,30,-73,31,-73,91,-73},new int[]{-181,1539,-180,1542,-43,1543,-44,1526,-63,1546});
    states[1539] = new State(-441);
    states[1540] = new State(new int[]{10,1541});
    states[1541] = new State(-442);
    states[1542] = new State(-450);
    states[1543] = new State(new int[]{91,17},new int[]{-259,1544});
    states[1544] = new State(new int[]{10,1545});
    states[1545] = new State(-472);
    states[1546] = new State(-451);
    states[1547] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406},new int[]{-174,1548,-173,1182,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1548] = new State(new int[]{8,566,10,-475,110,-475},new int[]{-131,1549});
    states[1549] = new State(new int[]{10,1361,110,-837},new int[]{-210,1332,-211,1550});
    states[1550] = new State(new int[]{110,1551});
    states[1551] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499},new int[]{-264,1552,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1552] = new State(new int[]{10,1553});
    states[1553] = new State(-445);
    states[1554] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406},new int[]{-173,1555,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1555] = new State(new int[]{8,566,5,-475,10,-475,110,-475},new int[]{-131,1556});
    states[1556] = new State(new int[]{5,1557,10,1361,110,-837},new int[]{-210,1367,-211,1563});
    states[1557] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,1558,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1558] = new State(new int[]{10,1361,110,-837},new int[]{-210,1371,-211,1559});
    states[1559] = new State(new int[]{110,1560});
    states[1560] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,714,21,295,22,300,76,447,11,369,40,590,18,670,37,679,44,683},new int[]{-106,1561,-104,882,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-326,883,-103,660,-327,678});
    states[1561] = new State(new int[]{10,1562});
    states[1562] = new State(-443);
    states[1563] = new State(new int[]{110,1564});
    states[1564] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,714,21,295,22,300,76,447,11,369,40,590,18,670,37,679,44,683},new int[]{-106,1565,-104,882,-102,30,-101,157,-108,587,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,642,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-326,883,-103,660,-327,678});
    states[1565] = new State(new int[]{10,1566});
    states[1566] = new State(-444);
    states[1567] = new State(new int[]{19,1569,30,1571,44,1547,37,1554},new int[]{-224,1568,-231,1538,-228,1328,-232,1363});
    states[1568] = new State(-432);
    states[1569] = new State(new int[]{44,1547,37,1554},new int[]{-224,1570,-231,1538,-228,1328,-232,1363});
    states[1570] = new State(-435);
    states[1571] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,8,-384,110,-384,10,-384},new int[]{-175,1572,-174,1181,-173,1182,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1572] = new State(new int[]{8,566,110,-475,10,-475},new int[]{-131,1573});
    states[1573] = new State(new int[]{110,1574,10,1170},new int[]{-210,527});
    states[1574] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499},new int[]{-264,1575,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1575] = new State(new int[]{10,1576});
    states[1576] = new State(-427);
    states[1577] = new State(new int[]{44,1547,37,1554,28,1381,26,1382},new int[]{-224,1578,-6,1579,-231,1538,-228,1328,-232,1363});
    states[1578] = new State(-433);
    states[1579] = new State(new int[]{44,1547,37,1554},new int[]{-224,1580,-231,1538,-228,1328,-232,1363});
    states[1580] = new State(-434);
    states[1581] = new State(-91);
    states[1582] = new State(-73,new int[]{-180,1583,-43,1543,-44,1526});
    states[1583] = new State(-425);
    states[1584] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406,8,-384,110,-384,10,-384},new int[]{-175,1585,-174,1181,-173,1182,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1585] = new State(new int[]{8,566,110,-475,10,-475},new int[]{-131,1586});
    states[1586] = new State(new int[]{110,1587,10,1170},new int[]{-210,1395});
    states[1587] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,10,-499},new int[]{-264,1588,-7,23,-116,24,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886});
    states[1588] = new State(new int[]{10,1589});
    states[1589] = new State(-426);
    states[1590] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,84,60,83,61,82,62,81,63,68,64,64,65,128,66,22,67,21,68,63,69,23,70,129,71,130,72,131,73,132,74,133,75,134,76,135,77,136,78,137,79,138,80,24,81,73,82,91,83,25,84,26,85,29,86,30,87,31,88,71,89,99,90,32,91,92,92,33,93,34,94,27,95,104,96,101,97,35,98,36,99,37,100,40,101,41,102,42,103,103,104,43,105,44,106,46,107,47,108,48,109,97,110,49,111,102,112,50,113,28,114,51,115,70,116,98,117,52,118,53,119,54,120,55,121,56,122,57,123,58,124,59,125,61,126,105,127,106,128,109,129,107,130,108,131,62,132,74,133,38,134,39,135,69,136,148,137,60,138,139,139,140,140,79,141,153,142,152,143,72,144,154,145,150,146,151,147,149,148,45,150},new int[]{-307,1591,-311,1601,-160,1595,-141,1600,-150,47,-154,50,-155,53,-296,58,-153,59,-297,149});
    states[1591] = new State(new int[]{10,1592,100,1593});
    states[1592] = new State(-38);
    states[1593] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,84,60,83,61,82,62,81,63,68,64,64,65,128,66,22,67,21,68,63,69,23,70,129,71,130,72,131,73,132,74,133,75,134,76,135,77,136,78,137,79,138,80,24,81,73,82,91,83,25,84,26,85,29,86,30,87,31,88,71,89,99,90,32,91,92,92,33,93,34,94,27,95,104,96,101,97,35,98,36,99,37,100,40,101,41,102,42,103,103,104,43,105,44,106,46,107,47,108,48,109,97,110,49,111,102,112,50,113,28,114,51,115,70,116,98,117,52,118,53,119,54,120,55,121,56,122,57,123,58,124,59,125,61,126,105,127,106,128,109,129,107,130,108,131,62,132,74,133,38,134,39,135,69,136,148,137,60,138,139,139,140,140,79,141,153,142,152,143,72,144,154,145,150,146,151,147,149,148,45,150},new int[]{-311,1594,-160,1595,-141,1600,-150,47,-154,50,-155,53,-296,58,-153,59,-297,149});
    states[1594] = new State(-44);
    states[1595] = new State(new int[]{7,1596,137,1598,10,-45,100,-45});
    states[1596] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,84,60,83,61,82,62,81,63,68,64,64,65,128,66,22,67,21,68,63,69,23,70,129,71,130,72,131,73,132,74,133,75,134,76,135,77,136,78,137,79,138,80,24,81,73,82,91,83,25,84,26,85,29,86,30,87,31,88,71,89,99,90,32,91,92,92,33,93,34,94,27,95,104,96,101,97,35,98,36,99,37,100,40,101,41,102,42,103,103,104,43,105,44,106,46,107,47,108,48,109,97,110,49,111,102,112,50,113,28,114,51,115,70,116,98,117,52,118,53,119,54,120,55,121,56,122,57,123,58,124,59,125,61,126,105,127,106,128,109,129,107,130,108,131,62,132,74,133,38,134,39,135,69,136,148,137,60,138,139,139,140,140,79,141,153,142,152,143,72,144,154,145,150,146,151,147,149,148,45,150},new int[]{-141,1597,-150,47,-154,50,-155,53,-296,58,-153,59,-297,149});
    states[1597] = new State(-37);
    states[1598] = new State(new int[]{144,1599});
    states[1599] = new State(-46);
    states[1600] = new State(-36);
    states[1601] = new State(-43);
    states[1602] = new State(new int[]{3,1604,52,-15,91,-15,59,-15,29,-15,66,-15,50,-15,53,-15,62,-15,11,-15,44,-15,37,-15,28,-15,26,-15,19,-15,30,-15,31,-15,43,-15,92,-15,103,-15},new int[]{-187,1603});
    states[1603] = new State(-17);
    states[1604] = new State(new int[]{143,1605,144,1606});
    states[1605] = new State(-18);
    states[1606] = new State(-19);
    states[1607] = new State(-16);
    states[1608] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,1609,-154,50,-155,53});
    states[1609] = new State(new int[]{10,1611,8,1612},new int[]{-190,1610});
    states[1610] = new State(-28);
    states[1611] = new State(-29);
    states[1612] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-192,1613,-149,1619,-150,1618,-154,50,-155,53});
    states[1613] = new State(new int[]{9,1614,100,1616});
    states[1614] = new State(new int[]{10,1615});
    states[1615] = new State(-30);
    states[1616] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-149,1617,-150,1618,-154,50,-155,53});
    states[1617] = new State(-32);
    states[1618] = new State(-33);
    states[1619] = new State(-31);
    states[1620] = new State(-3);
    states[1621] = new State(new int[]{43,1642,52,-41,59,-41,29,-41,66,-41,50,-41,53,-41,62,-41,11,-41,44,-41,37,-41,28,-41,26,-41,19,-41,30,-41,31,-41,92,-41,103,-41,91,-41},new int[]{-165,1622,-166,1639,-306,1680});
    states[1622] = new State(new int[]{41,1636},new int[]{-164,1623});
    states[1623] = new State(new int[]{92,1626,103,1627,91,1633},new int[]{-157,1624});
    states[1624] = new State(new int[]{7,1625});
    states[1625] = new State(-47);
    states[1626] = new State(-63);
    states[1627] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,104,-499,10,-499},new int[]{-256,1628,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[1628] = new State(new int[]{92,1629,104,1630,10,20});
    states[1629] = new State(-64);
    states[1630] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499},new int[]{-256,1631,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[1631] = new State(new int[]{92,1632,10,20});
    states[1632] = new State(-65);
    states[1633] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,92,-499,10,-499},new int[]{-256,1634,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041});
    states[1634] = new State(new int[]{92,1635,10,20});
    states[1635] = new State(-66);
    states[1636] = new State(-41,new int[]{-306,1637});
    states[1637] = new State(new int[]{52,1590,59,-73,29,-73,66,-73,50,-73,53,-73,62,-73,11,-73,44,-73,37,-73,28,-73,26,-73,19,-73,30,-73,31,-73,92,-73,103,-73,91,-73},new int[]{-43,1638,-308,14,-44,1526});
    states[1638] = new State(-61);
    states[1639] = new State(new int[]{92,1626,103,1627,91,1633},new int[]{-157,1640});
    states[1640] = new State(new int[]{7,1641});
    states[1641] = new State(-48);
    states[1642] = new State(-41,new int[]{-306,1643});
    states[1643] = new State(new int[]{52,1590,161,-55,29,-55,66,-55,50,-55,53,-55,62,-55,11,-55,44,-55,37,-55,41,-55},new int[]{-2,1644,-308,14});
    states[1644] = new State(new int[]{161,1646,29,-68,66,-68,50,-68,53,-68,62,-68,11,-68,44,-68,37,-68,41,-68},new int[]{-42,1645,-40,1657});
    states[1645] = new State(-54);
    states[1646] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-3,1647,-4,1656,-150,1651,-154,50,-155,53});
    states[1647] = new State(new int[]{10,1648,100,1649});
    states[1648] = new State(-56);
    states[1649] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-4,1650,-150,1651,-154,50,-155,53});
    states[1650] = new State(-58);
    states[1651] = new State(new int[]{7,1652});
    states[1652] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,1653,-154,50,-155,53});
    states[1653] = new State(new int[]{136,1654,10,-59,100,-59});
    states[1654] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-150,1655,-154,50,-155,53});
    states[1655] = new State(-60);
    states[1656] = new State(-57);
    states[1657] = new State(new int[]{29,1230,66,1234,50,1446,53,1461,62,1463,11,629,41,-67,44,-219,37,-219},new int[]{-50,1658,-31,1659,-54,1660,-292,1661,-313,1662,-235,1663,-9,1664,-254,1156,-234,1679});
    states[1658] = new State(-69);
    states[1659] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,29,-78,66,-78,50,-78,53,-78,62,-78,11,-78,44,-78,37,-78,41,-78},new int[]{-29,1216,-30,1217,-144,1219,-150,1229,-154,50,-155,53});
    states[1660] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,29,-79,66,-79,50,-79,53,-79,62,-79,11,-79,44,-79,37,-79,41,-79},new int[]{-29,1233,-30,1217,-144,1219,-150,1229,-154,50,-155,53});
    states[1661] = new State(new int[]{11,629,29,-80,66,-80,50,-80,53,-80,62,-80,44,-80,37,-80,41,-80,143,-219,161,-219,85,-219,86,-219,80,-219,78,-219,160,-219,87,-219},new int[]{-51,1237,-9,1238,-254,1156});
    states[1662] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,8,1453,29,-81,66,-81,50,-81,53,-81,62,-81,11,-81,44,-81,37,-81,41,-81},new int[]{-317,1449,-314,1450,-315,1451,-161,811,-150,810,-154,50,-155,53});
    states[1663] = new State(-82);
    states[1664] = new State(new int[]{44,1671,11,629,37,1674},new int[]{-228,1665,-254,519,-232,1668});
    states[1665] = new State(new int[]{149,1666,29,-98,66,-98,50,-98,53,-98,62,-98,11,-98,44,-98,37,-98,41,-98});
    states[1666] = new State(new int[]{10,1667});
    states[1667] = new State(-99);
    states[1668] = new State(new int[]{149,1669,29,-100,66,-100,50,-100,53,-100,62,-100,11,-100,44,-100,37,-100,41,-100});
    states[1669] = new State(new int[]{10,1670});
    states[1670] = new State(-101);
    states[1671] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406},new int[]{-174,1672,-173,1182,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1672] = new State(new int[]{8,566,10,-475},new int[]{-131,1673});
    states[1673] = new State(new int[]{10,1170},new int[]{-210,1332});
    states[1674] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,45,406},new int[]{-173,1675,-145,1183,-140,1184,-137,1185,-150,1190,-154,50,-155,53,-194,1191,-338,1193,-152,1197});
    states[1675] = new State(new int[]{8,566,5,-475,10,-475},new int[]{-131,1676});
    states[1676] = new State(new int[]{5,1677,10,1170},new int[]{-210,1367});
    states[1677] = new State(new int[]{143,363,161,49,85,51,86,52,80,54,78,55,160,56,87,57,155,288,158,289,157,290,156,291,116,354,115,355,144,282,147,283,145,285,146,286,8,479,142,490,24,495,48,503,49,550,34,554,73,558,44,564,37,605},new int[]{-279,1678,-280,492,-276,362,-96,195,-109,315,-110,316,-183,317,-150,219,-154,50,-155,53,-19,350,-202,351,-168,356,-170,280,-169,284,-277,481,-304,482,-260,488,-253,489,-285,493,-282,494,-274,501,-33,502,-267,549,-133,553,-134,557,-229,561,-227,562,-226,563});
    states[1678] = new State(new int[]{10,1170},new int[]{-210,1371});
    states[1679] = new State(-83);
    states[1680] = new State(new int[]{52,1590,59,-73,29,-73,66,-73,50,-73,53,-73,62,-73,11,-73,44,-73,37,-73,28,-73,26,-73,19,-73,30,-73,31,-73,92,-73,103,-73,91,-73},new int[]{-43,1681,-308,14,-44,1526});
    states[1681] = new State(-62);
    states[1682] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-142,1683,-150,1686,-154,50,-155,53});
    states[1683] = new State(new int[]{10,1684});
    states[1684] = new State(new int[]{3,1604,43,-14,92,-14,103,-14,91,-14,52,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-188,1685,-189,1602,-187,1607});
    states[1685] = new State(-49);
    states[1686] = new State(-53);
    states[1687] = new State(-51);
    states[1688] = new State(-52);
    states[1689] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,84,60,83,61,82,62,81,63,68,64,64,65,128,66,22,67,21,68,63,69,23,70,129,71,130,72,131,73,132,74,133,75,134,76,135,77,136,78,137,79,138,80,24,81,73,82,91,83,25,84,26,85,29,86,30,87,31,88,71,89,99,90,32,91,92,92,33,93,34,94,27,95,104,96,101,97,35,98,36,99,37,100,40,101,41,102,42,103,103,104,43,105,44,106,46,107,47,108,48,109,97,110,49,111,102,112,50,113,28,114,51,115,70,116,98,117,52,118,53,119,54,120,55,121,56,122,57,123,58,124,59,125,61,126,105,127,106,128,109,129,107,130,108,131,62,132,74,133,38,134,39,135,69,136,148,137,60,138,139,139,140,140,79,141,153,142,152,143,72,144,154,145,150,146,151,147,149,148,45,150},new int[]{-160,1690,-141,1600,-150,47,-154,50,-155,53,-296,58,-153,59,-297,149});
    states[1690] = new State(new int[]{10,1691,7,1596});
    states[1691] = new State(new int[]{3,1604,43,-14,92,-14,103,-14,91,-14,52,-14,59,-14,29,-14,66,-14,50,-14,53,-14,62,-14,11,-14,44,-14,37,-14,28,-14,26,-14,19,-14,30,-14,31,-14},new int[]{-188,1692,-189,1602,-187,1607});
    states[1692] = new State(-50);
    states[1693] = new State(-4);
    states[1694] = new State(new int[]{50,1696,56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,438,21,295,22,300,76,447,11,369,40,590,5,599},new int[]{-90,1695,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,398,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598});
    states[1695] = new State(new int[]{70,28,2,-7});
    states[1696] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-147,1697,-150,1698,-154,50,-155,53});
    states[1697] = new State(-8);
    states[1698] = new State(new int[]{123,1187,2,-227},new int[]{-158,1439});
    states[1699] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57},new int[]{-324,1700,-325,1701,-150,1705,-154,50,-155,53});
    states[1700] = new State(-9);
    states[1701] = new State(new int[]{7,1702,123,188,2,-792},new int[]{-302,1704});
    states[1702] = new State(new int[]{143,48,161,49,85,51,86,52,80,54,78,55,160,56,87,57,84,60,83,61,82,62,81,63,68,64,64,65,128,66,22,67,21,68,63,69,23,70,129,71,130,72,131,73,132,74,133,75,134,76,135,77,136,78,137,79,138,80,24,81,73,82,91,83,25,84,26,85,29,86,30,87,31,88,71,89,99,90,32,91,92,92,33,93,34,94,27,95,104,96,101,97,35,98,36,99,37,100,40,101,41,102,42,103,103,104,43,105,44,106,46,107,47,108,48,109,97,110,49,111,102,112,50,113,28,114,51,115,70,116,98,117,52,118,53,119,54,120,55,121,56,122,57,123,58,124,59,125,61,126,105,127,106,128,109,129,107,130,108,131,62,132,74,133,38,134,39,135,69,136,148,137,60,138,139,139,140,140,79,141,153,142,152,143,72,144,154,145,150,146,151,147,149,148,45,150},new int[]{-141,1703,-150,47,-154,50,-155,53,-296,58,-153,59,-297,149});
    states[1703] = new State(-791);
    states[1704] = new State(-793);
    states[1705] = new State(-790);
    states[1706] = new State(new int[]{56,43,144,282,147,283,145,285,146,286,155,288,158,289,157,290,156,291,63,340,135,378,116,354,115,355,142,388,141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,436,8,532,21,295,22,300,76,447,11,369,40,590,5,599,53,801},new int[]{-263,1707,-90,1708,-104,156,-102,30,-101,157,-108,167,-86,172,-85,178,-99,336,-18,44,-168,337,-170,280,-169,284,-19,338,-59,339,-202,385,-116,1124,-135,392,-114,400,-150,404,-154,50,-155,53,-194,405,-261,445,-298,446,-243,451,-115,452,-60,453,-119,459,-176,460,-272,461,-100,462,-268,466,-270,467,-271,579,-244,580,-120,581,-246,589,-123,598,-7,1709,-318,1710});
    states[1707] = new State(-10);
    states[1708] = new State(new int[]{70,28,2,-11});
    states[1709] = new State(-12);
    states[1710] = new State(-13);
    states[1711] = new State(new int[]{52,1590,141,-39,143,-39,161,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,11,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,44,-39,37,-39,10,-39,2,-39},new int[]{-309,1712,-308,1716});
    states[1712] = new State(-71,new int[]{-46,1713});
    states[1713] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,44,1547,37,1554,10,-499,2,-499},new int[]{-256,1714,-224,1715,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041,-231,1538,-228,1328,-232,1363});
    states[1714] = new State(new int[]{10,20,2,-5});
    states[1715] = new State(-72);
    states[1716] = new State(-40);
    states[1717] = new State(new int[]{52,1590,141,-39,143,-39,161,-39,85,-39,86,-39,80,-39,78,-39,160,-39,87,-39,45,-39,42,-39,8,-39,21,-39,22,-39,144,-39,147,-39,145,-39,146,-39,155,-39,158,-39,157,-39,156,-39,76,-39,11,-39,57,-39,91,-39,40,-39,25,-39,97,-39,54,-39,35,-39,55,-39,102,-39,47,-39,36,-39,53,-39,60,-39,74,-39,72,-39,38,-39,44,-39,37,-39,10,-39,2,-39},new int[]{-309,1718,-308,1716});
    states[1718] = new State(-71,new int[]{-46,1719});
    states[1719] = new State(new int[]{141,399,143,48,161,49,85,51,86,52,80,54,78,264,160,56,87,57,45,406,42,531,8,532,21,295,22,300,144,282,147,283,145,285,146,286,155,753,158,289,157,290,156,291,76,447,11,369,57,732,91,17,40,722,25,739,97,755,54,760,35,765,55,776,102,782,47,789,36,792,53,801,60,873,74,878,72,865,38,887,44,1547,37,1554,10,-499,2,-499},new int[]{-256,1720,-224,1715,-265,751,-264,22,-7,23,-116,24,-135,392,-114,400,-150,752,-154,50,-155,53,-194,405,-261,445,-298,446,-18,728,-168,337,-170,280,-169,284,-19,338,-243,451,-115,452,-60,729,-119,459,-215,730,-136,731,-259,736,-156,737,-37,738,-251,754,-322,759,-127,764,-323,775,-163,780,-305,781,-252,788,-126,791,-318,800,-61,869,-177,870,-176,871,-172,872,-129,877,-130,884,-128,885,-352,886,-146,1041,-231,1538,-228,1328,-232,1363});
    states[1720] = new State(new int[]{10,20,2,-6});

    rules[1] = new Rule(-365, new int[]{-1,2});
    rules[2] = new Rule(-1, new int[]{-237});
    rules[3] = new Rule(-1, new int[]{-310});
    rules[4] = new Rule(-1, new int[]{-178});
    rules[5] = new Rule(-1, new int[]{75,-309,-46,-256});
    rules[6] = new Rule(-1, new int[]{77,-309,-46,-256});
    rules[7] = new Rule(-178, new int[]{88,-90});
    rules[8] = new Rule(-178, new int[]{88,50,-147});
    rules[9] = new Rule(-178, new int[]{90,-324});
    rules[10] = new Rule(-178, new int[]{89,-263});
    rules[11] = new Rule(-263, new int[]{-90});
    rules[12] = new Rule(-263, new int[]{-7});
    rules[13] = new Rule(-263, new int[]{-318});
    rules[14] = new Rule(-188, new int[]{});
    rules[15] = new Rule(-188, new int[]{-189});
    rules[16] = new Rule(-189, new int[]{-187});
    rules[17] = new Rule(-189, new int[]{-189,-187});
    rules[18] = new Rule(-187, new int[]{3,143});
    rules[19] = new Rule(-187, new int[]{3,144});
    rules[20] = new Rule(-237, new int[]{-238,-188,-306,-20,-191});
    rules[21] = new Rule(-191, new int[]{7});
    rules[22] = new Rule(-191, new int[]{10});
    rules[23] = new Rule(-191, new int[]{5});
    rules[24] = new Rule(-191, new int[]{100});
    rules[25] = new Rule(-191, new int[]{6});
    rules[26] = new Rule(-191, new int[]{});
    rules[27] = new Rule(-238, new int[]{});
    rules[28] = new Rule(-238, new int[]{61,-150,-190});
    rules[29] = new Rule(-190, new int[]{10});
    rules[30] = new Rule(-190, new int[]{8,-192,9,10});
    rules[31] = new Rule(-192, new int[]{-149});
    rules[32] = new Rule(-192, new int[]{-192,100,-149});
    rules[33] = new Rule(-149, new int[]{-150});
    rules[34] = new Rule(-20, new int[]{-39,-259});
    rules[35] = new Rule(-39, new int[]{-43});
    rules[36] = new Rule(-160, new int[]{-141});
    rules[37] = new Rule(-160, new int[]{-160,7,-141});
    rules[38] = new Rule(-308, new int[]{52,-307,10});
    rules[39] = new Rule(-309, new int[]{});
    rules[40] = new Rule(-309, new int[]{-308});
    rules[41] = new Rule(-306, new int[]{});
    rules[42] = new Rule(-306, new int[]{-306,-308});
    rules[43] = new Rule(-307, new int[]{-311});
    rules[44] = new Rule(-307, new int[]{-307,100,-311});
    rules[45] = new Rule(-311, new int[]{-160});
    rules[46] = new Rule(-311, new int[]{-160,137,144});
    rules[47] = new Rule(-310, new int[]{-312,-165,-164,-157,7});
    rules[48] = new Rule(-310, new int[]{-312,-166,-157,7});
    rules[49] = new Rule(-312, new int[]{-5,-142,10,-188});
    rules[50] = new Rule(-312, new int[]{109,-160,10,-188});
    rules[51] = new Rule(-5, new int[]{105});
    rules[52] = new Rule(-5, new int[]{106});
    rules[53] = new Rule(-142, new int[]{-150});
    rules[54] = new Rule(-165, new int[]{43,-306,-2,-42});
    rules[55] = new Rule(-2, new int[]{});
    rules[56] = new Rule(-2, new int[]{-2,161,-3,10});
    rules[57] = new Rule(-3, new int[]{-4});
    rules[58] = new Rule(-3, new int[]{-3,100,-4});
    rules[59] = new Rule(-4, new int[]{-150,7,-150});
    rules[60] = new Rule(-4, new int[]{-150,7,-150,136,-150});
    rules[61] = new Rule(-164, new int[]{41,-306,-43});
    rules[62] = new Rule(-166, new int[]{-306,-43});
    rules[63] = new Rule(-157, new int[]{92});
    rules[64] = new Rule(-157, new int[]{103,-256,92});
    rules[65] = new Rule(-157, new int[]{103,-256,104,-256,92});
    rules[66] = new Rule(-157, new int[]{91,-256,92});
    rules[67] = new Rule(-42, new int[]{-40});
    rules[68] = new Rule(-40, new int[]{});
    rules[69] = new Rule(-40, new int[]{-40,-50});
    rules[70] = new Rule(-43, new int[]{-44});
    rules[71] = new Rule(-46, new int[]{});
    rules[72] = new Rule(-46, new int[]{-46,-224});
    rules[73] = new Rule(-44, new int[]{});
    rules[74] = new Rule(-44, new int[]{-44,-49});
    rules[75] = new Rule(-45, new int[]{-41});
    rules[76] = new Rule(-41, new int[]{});
    rules[77] = new Rule(-41, new int[]{-41,-48});
    rules[78] = new Rule(-50, new int[]{-31});
    rules[79] = new Rule(-50, new int[]{-54});
    rules[80] = new Rule(-50, new int[]{-292});
    rules[81] = new Rule(-50, new int[]{-313});
    rules[82] = new Rule(-50, new int[]{-235});
    rules[83] = new Rule(-50, new int[]{-234});
    rules[84] = new Rule(-49, new int[]{-171});
    rules[85] = new Rule(-49, new int[]{-31});
    rules[86] = new Rule(-49, new int[]{-54});
    rules[87] = new Rule(-49, new int[]{-292});
    rules[88] = new Rule(-49, new int[]{-313});
    rules[89] = new Rule(-49, new int[]{-223});
    rules[90] = new Rule(-216, new int[]{-217});
    rules[91] = new Rule(-216, new int[]{-220});
    rules[92] = new Rule(-223, new int[]{-9,-216});
    rules[93] = new Rule(-48, new int[]{-171});
    rules[94] = new Rule(-48, new int[]{-31});
    rules[95] = new Rule(-48, new int[]{-54});
    rules[96] = new Rule(-48, new int[]{-292});
    rules[97] = new Rule(-48, new int[]{-313});
    rules[98] = new Rule(-235, new int[]{-9,-228});
    rules[99] = new Rule(-235, new int[]{-9,-228,149,10});
    rules[100] = new Rule(-234, new int[]{-9,-232});
    rules[101] = new Rule(-234, new int[]{-9,-232,149,10});
    rules[102] = new Rule(-171, new int[]{59,-159,10});
    rules[103] = new Rule(-159, new int[]{-146});
    rules[104] = new Rule(-159, new int[]{-159,100,-146});
    rules[105] = new Rule(-146, new int[]{155});
    rules[106] = new Rule(-146, new int[]{-150});
    rules[107] = new Rule(-31, new int[]{29,-29});
    rules[108] = new Rule(-31, new int[]{-31,-29});
    rules[109] = new Rule(-54, new int[]{66,-29});
    rules[110] = new Rule(-54, new int[]{-54,-29});
    rules[111] = new Rule(-292, new int[]{50,-51});
    rules[112] = new Rule(-292, new int[]{-292,-51});
    rules[113] = new Rule(-317, new int[]{-314});
    rules[114] = new Rule(-317, new int[]{8,-150,100,-161,9,110,-104,10});
    rules[115] = new Rule(-313, new int[]{53,-317});
    rules[116] = new Rule(-313, new int[]{62,-317});
    rules[117] = new Rule(-313, new int[]{-313,-317});
    rules[118] = new Rule(-29, new int[]{-30,10});
    rules[119] = new Rule(-30, new int[]{-144,120,-112});
    rules[120] = new Rule(-30, new int[]{-144,5,-280,120,-87});
    rules[121] = new Rule(-112, new int[]{-93});
    rules[122] = new Rule(-112, new int[]{-98});
    rules[123] = new Rule(-144, new int[]{-150});
    rules[124] = new Rule(-94, new int[]{-84});
    rules[125] = new Rule(-94, new int[]{-94,-195,-84});
    rules[126] = new Rule(-93, new int[]{-94});
    rules[127] = new Rule(-93, new int[]{-245});
    rules[128] = new Rule(-93, new int[]{-93,16,-94});
    rules[129] = new Rule(-245, new int[]{-93,13,-93,5,-93});
    rules[130] = new Rule(-195, new int[]{120});
    rules[131] = new Rule(-195, new int[]{125});
    rules[132] = new Rule(-195, new int[]{123});
    rules[133] = new Rule(-195, new int[]{121});
    rules[134] = new Rule(-195, new int[]{124});
    rules[135] = new Rule(-195, new int[]{122});
    rules[136] = new Rule(-195, new int[]{137});
    rules[137] = new Rule(-195, new int[]{135,137});
    rules[138] = new Rule(-84, new int[]{-16});
    rules[139] = new Rule(-84, new int[]{-84,-196,-16});
    rules[140] = new Rule(-196, new int[]{116});
    rules[141] = new Rule(-196, new int[]{115});
    rules[142] = new Rule(-196, new int[]{128});
    rules[143] = new Rule(-196, new int[]{129});
    rules[144] = new Rule(-269, new int[]{-16,-204,-287});
    rules[145] = new Rule(-273, new int[]{-14,119,-13});
    rules[146] = new Rule(-273, new int[]{-14,119,-273});
    rules[147] = new Rule(-273, new int[]{-202,-273});
    rules[148] = new Rule(-16, new int[]{-13});
    rules[149] = new Rule(-16, new int[]{-269});
    rules[150] = new Rule(-16, new int[]{-273});
    rules[151] = new Rule(-16, new int[]{-16,-198,-13});
    rules[152] = new Rule(-16, new int[]{-16,-198,-273});
    rules[153] = new Rule(-198, new int[]{118});
    rules[154] = new Rule(-198, new int[]{117});
    rules[155] = new Rule(-198, new int[]{131});
    rules[156] = new Rule(-198, new int[]{132});
    rules[157] = new Rule(-198, new int[]{133});
    rules[158] = new Rule(-198, new int[]{134});
    rules[159] = new Rule(-198, new int[]{130});
    rules[160] = new Rule(-14, new int[]{-17});
    rules[161] = new Rule(-14, new int[]{8,-93,9});
    rules[162] = new Rule(-13, new int[]{-17});
    rules[163] = new Rule(-13, new int[]{-242});
    rules[164] = new Rule(-13, new int[]{56});
    rules[165] = new Rule(-13, new int[]{141,-13});
    rules[166] = new Rule(-13, new int[]{8,-93,9});
    rules[167] = new Rule(-13, new int[]{135,-13});
    rules[168] = new Rule(-13, new int[]{-202,-13});
    rules[169] = new Rule(-13, new int[]{-176});
    rules[170] = new Rule(-13, new int[]{-59});
    rules[171] = new Rule(-243, new int[]{11,-71,12});
    rules[172] = new Rule(-242, new int[]{-243});
    rules[173] = new Rule(-242, new int[]{76,-71,76});
    rules[174] = new Rule(-202, new int[]{116});
    rules[175] = new Rule(-202, new int[]{115});
    rules[176] = new Rule(-17, new int[]{-150});
    rules[177] = new Rule(-17, new int[]{-168});
    rules[178] = new Rule(-17, new int[]{-19});
    rules[179] = new Rule(-17, new int[]{42,-150});
    rules[180] = new Rule(-17, new int[]{-261});
    rules[181] = new Rule(-17, new int[]{-298});
    rules[182] = new Rule(-17, new int[]{-17,-15});
    rules[183] = new Rule(-17, new int[]{-17,4,-302});
    rules[184] = new Rule(-17, new int[]{-17,11,-124,12});
    rules[185] = new Rule(-15, new int[]{7,-141});
    rules[186] = new Rule(-15, new int[]{142});
    rules[187] = new Rule(-15, new int[]{8,-79,9});
    rules[188] = new Rule(-15, new int[]{11,-78,12});
    rules[189] = new Rule(-79, new int[]{-74});
    rules[190] = new Rule(-79, new int[]{});
    rules[191] = new Rule(-78, new int[]{-76});
    rules[192] = new Rule(-78, new int[]{});
    rules[193] = new Rule(-76, new int[]{-97});
    rules[194] = new Rule(-76, new int[]{-76,100,-97});
    rules[195] = new Rule(-97, new int[]{-93});
    rules[196] = new Rule(-97, new int[]{-93,6,-93});
    rules[197] = new Rule(-19, new int[]{155});
    rules[198] = new Rule(-19, new int[]{158});
    rules[199] = new Rule(-19, new int[]{157});
    rules[200] = new Rule(-19, new int[]{156});
    rules[201] = new Rule(-87, new int[]{-93});
    rules[202] = new Rule(-87, new int[]{-98});
    rules[203] = new Rule(-87, new int[]{-247});
    rules[204] = new Rule(-98, new int[]{8,-68,9});
    rules[205] = new Rule(-68, new int[]{});
    rules[206] = new Rule(-68, new int[]{-67});
    rules[207] = new Rule(-67, new int[]{-88});
    rules[208] = new Rule(-67, new int[]{-67,100,-88});
    rules[209] = new Rule(-247, new int[]{8,-249,9});
    rules[210] = new Rule(-249, new int[]{-248});
    rules[211] = new Rule(-249, new int[]{-248,10});
    rules[212] = new Rule(-248, new int[]{-250});
    rules[213] = new Rule(-248, new int[]{-248,10,-250});
    rules[214] = new Rule(-250, new int[]{-139,5,-87});
    rules[215] = new Rule(-139, new int[]{-150});
    rules[216] = new Rule(-51, new int[]{-9,-52});
    rules[217] = new Rule(-9, new int[]{-254});
    rules[218] = new Rule(-9, new int[]{-9,-254});
    rules[219] = new Rule(-9, new int[]{});
    rules[220] = new Rule(-254, new int[]{11,-255,12});
    rules[221] = new Rule(-255, new int[]{-11});
    rules[222] = new Rule(-255, new int[]{-255,100,-11});
    rules[223] = new Rule(-11, new int[]{-12});
    rules[224] = new Rule(-11, new int[]{-150,5,-12});
    rules[225] = new Rule(-52, new int[]{-147,120,-290,10});
    rules[226] = new Rule(-52, new int[]{-148,-290,10});
    rules[227] = new Rule(-147, new int[]{-150});
    rules[228] = new Rule(-147, new int[]{-150,-158});
    rules[229] = new Rule(-148, new int[]{-150,123,-161,122});
    rules[230] = new Rule(-290, new int[]{-280});
    rules[231] = new Rule(-290, new int[]{-32});
    rules[232] = new Rule(-277, new int[]{-276,13});
    rules[233] = new Rule(-277, new int[]{-304,13});
    rules[234] = new Rule(-280, new int[]{-276});
    rules[235] = new Rule(-280, new int[]{-277});
    rules[236] = new Rule(-280, new int[]{-260});
    rules[237] = new Rule(-280, new int[]{-253});
    rules[238] = new Rule(-280, new int[]{-285});
    rules[239] = new Rule(-280, new int[]{-229});
    rules[240] = new Rule(-280, new int[]{-304});
    rules[241] = new Rule(-304, new int[]{-183,-302});
    rules[242] = new Rule(-302, new int[]{123,-300,121});
    rules[243] = new Rule(-303, new int[]{125});
    rules[244] = new Rule(-303, new int[]{123,-301,121});
    rules[245] = new Rule(-300, new int[]{-283});
    rules[246] = new Rule(-300, new int[]{-300,100,-283});
    rules[247] = new Rule(-301, new int[]{-284});
    rules[248] = new Rule(-301, new int[]{-301,100,-284});
    rules[249] = new Rule(-284, new int[]{});
    rules[250] = new Rule(-283, new int[]{-276});
    rules[251] = new Rule(-283, new int[]{-276,13});
    rules[252] = new Rule(-283, new int[]{-285});
    rules[253] = new Rule(-283, new int[]{-229});
    rules[254] = new Rule(-283, new int[]{-304});
    rules[255] = new Rule(-276, new int[]{-96});
    rules[256] = new Rule(-276, new int[]{-96,6,-96});
    rules[257] = new Rule(-276, new int[]{8,-83,9});
    rules[258] = new Rule(-96, new int[]{-109});
    rules[259] = new Rule(-96, new int[]{-96,-196,-109});
    rules[260] = new Rule(-109, new int[]{-110});
    rules[261] = new Rule(-109, new int[]{-109,-198,-110});
    rules[262] = new Rule(-110, new int[]{-183});
    rules[263] = new Rule(-110, new int[]{-19});
    rules[264] = new Rule(-110, new int[]{-202,-110});
    rules[265] = new Rule(-110, new int[]{-168});
    rules[266] = new Rule(-110, new int[]{-110,8,-78,9});
    rules[267] = new Rule(-183, new int[]{-150});
    rules[268] = new Rule(-183, new int[]{-183,7,-141});
    rules[269] = new Rule(-83, new int[]{-81});
    rules[270] = new Rule(-83, new int[]{-83,100,-81});
    rules[271] = new Rule(-81, new int[]{-280});
    rules[272] = new Rule(-81, new int[]{-280,120,-90});
    rules[273] = new Rule(-253, new int[]{142,-279});
    rules[274] = new Rule(-285, new int[]{-282});
    rules[275] = new Rule(-285, new int[]{-33});
    rules[276] = new Rule(-285, new int[]{-267});
    rules[277] = new Rule(-285, new int[]{-133});
    rules[278] = new Rule(-285, new int[]{-134});
    rules[279] = new Rule(-134, new int[]{73,58,-280});
    rules[280] = new Rule(-282, new int[]{24,11,-167,12,58,-280});
    rules[281] = new Rule(-282, new int[]{-274});
    rules[282] = new Rule(-274, new int[]{24,58,-280});
    rules[283] = new Rule(-167, new int[]{-275});
    rules[284] = new Rule(-167, new int[]{-167,100,-275});
    rules[285] = new Rule(-275, new int[]{-276});
    rules[286] = new Rule(-275, new int[]{});
    rules[287] = new Rule(-267, new int[]{49,58,-280});
    rules[288] = new Rule(-133, new int[]{34,58,-280});
    rules[289] = new Rule(-133, new int[]{34});
    rules[290] = new Rule(-260, new int[]{143,11,-93,12});
    rules[291] = new Rule(-229, new int[]{-227});
    rules[292] = new Rule(-227, new int[]{-226});
    rules[293] = new Rule(-226, new int[]{44,-131});
    rules[294] = new Rule(-226, new int[]{37,-131,5,-279});
    rules[295] = new Rule(-226, new int[]{-183,127,-283});
    rules[296] = new Rule(-226, new int[]{-304,127,-283});
    rules[297] = new Rule(-226, new int[]{8,9,127,-283});
    rules[298] = new Rule(-226, new int[]{8,-83,9,127,-283});
    rules[299] = new Rule(-226, new int[]{-183,127,8,9});
    rules[300] = new Rule(-226, new int[]{-304,127,8,9});
    rules[301] = new Rule(-226, new int[]{8,9,127,8,9});
    rules[302] = new Rule(-226, new int[]{8,-83,9,127,8,9});
    rules[303] = new Rule(-32, new int[]{-23,-294,-186,-321,-28});
    rules[304] = new Rule(-33, new int[]{48,-186,-321,-27,92});
    rules[305] = new Rule(-22, new int[]{68});
    rules[306] = new Rule(-22, new int[]{69});
    rules[307] = new Rule(-22, new int[]{148});
    rules[308] = new Rule(-22, new int[]{27});
    rules[309] = new Rule(-22, new int[]{28});
    rules[310] = new Rule(-23, new int[]{});
    rules[311] = new Rule(-23, new int[]{-24});
    rules[312] = new Rule(-24, new int[]{-22});
    rules[313] = new Rule(-24, new int[]{-24,-22});
    rules[314] = new Rule(-294, new int[]{26});
    rules[315] = new Rule(-294, new int[]{43});
    rules[316] = new Rule(-294, new int[]{64});
    rules[317] = new Rule(-294, new int[]{64,26});
    rules[318] = new Rule(-294, new int[]{64,48});
    rules[319] = new Rule(-294, new int[]{64,43});
    rules[320] = new Rule(-28, new int[]{});
    rules[321] = new Rule(-28, new int[]{-27,92});
    rules[322] = new Rule(-186, new int[]{});
    rules[323] = new Rule(-186, new int[]{8,-185,9});
    rules[324] = new Rule(-185, new int[]{-184});
    rules[325] = new Rule(-185, new int[]{-185,100,-184});
    rules[326] = new Rule(-184, new int[]{-183});
    rules[327] = new Rule(-184, new int[]{-304});
    rules[328] = new Rule(-158, new int[]{123,-161,121});
    rules[329] = new Rule(-321, new int[]{});
    rules[330] = new Rule(-321, new int[]{-320});
    rules[331] = new Rule(-320, new int[]{-319});
    rules[332] = new Rule(-320, new int[]{-320,-319});
    rules[333] = new Rule(-319, new int[]{23,-161,5,-291,10});
    rules[334] = new Rule(-291, new int[]{-289});
    rules[335] = new Rule(-291, new int[]{-291,100,-289});
    rules[336] = new Rule(-289, new int[]{-280});
    rules[337] = new Rule(-289, new int[]{26});
    rules[338] = new Rule(-289, new int[]{48});
    rules[339] = new Rule(-289, new int[]{30});
    rules[340] = new Rule(-27, new int[]{-34});
    rules[341] = new Rule(-27, new int[]{-27,-10,-34});
    rules[342] = new Rule(-10, new int[]{84});
    rules[343] = new Rule(-10, new int[]{83});
    rules[344] = new Rule(-10, new int[]{82});
    rules[345] = new Rule(-10, new int[]{81});
    rules[346] = new Rule(-34, new int[]{});
    rules[347] = new Rule(-34, new int[]{-36,-193});
    rules[348] = new Rule(-34, new int[]{-35});
    rules[349] = new Rule(-34, new int[]{-36,10,-35});
    rules[350] = new Rule(-161, new int[]{-150});
    rules[351] = new Rule(-161, new int[]{-161,100,-150});
    rules[352] = new Rule(-193, new int[]{});
    rules[353] = new Rule(-193, new int[]{10});
    rules[354] = new Rule(-36, new int[]{-47});
    rules[355] = new Rule(-36, new int[]{-36,10,-47});
    rules[356] = new Rule(-47, new int[]{-9,-53});
    rules[357] = new Rule(-35, new int[]{-56});
    rules[358] = new Rule(-35, new int[]{-35,-56});
    rules[359] = new Rule(-56, new int[]{-55});
    rules[360] = new Rule(-56, new int[]{-57});
    rules[361] = new Rule(-53, new int[]{29,-30});
    rules[362] = new Rule(-53, new int[]{-316});
    rules[363] = new Rule(-53, new int[]{-6,-316});
    rules[364] = new Rule(-6, new int[]{28});
    rules[365] = new Rule(-6, new int[]{26});
    rules[366] = new Rule(-316, new int[]{-315});
    rules[367] = new Rule(-316, new int[]{62,-161,5,-280});
    rules[368] = new Rule(-55, new int[]{-9,-225});
    rules[369] = new Rule(-55, new int[]{-9,-222});
    rules[370] = new Rule(-222, new int[]{-218});
    rules[371] = new Rule(-222, new int[]{-221});
    rules[372] = new Rule(-225, new int[]{-6,-233});
    rules[373] = new Rule(-225, new int[]{-233});
    rules[374] = new Rule(-225, new int[]{19,-6,-233});
    rules[375] = new Rule(-225, new int[]{19,-233});
    rules[376] = new Rule(-225, new int[]{-230});
    rules[377] = new Rule(-233, new int[]{-231});
    rules[378] = new Rule(-231, new int[]{-228});
    rules[379] = new Rule(-231, new int[]{-232});
    rules[380] = new Rule(-230, new int[]{30,-175,-131,-210});
    rules[381] = new Rule(-230, new int[]{-6,30,-175,-131,-210});
    rules[382] = new Rule(-230, new int[]{31,-175,-131,-210});
    rules[383] = new Rule(-175, new int[]{-174});
    rules[384] = new Rule(-175, new int[]{});
    rules[385] = new Rule(-57, new int[]{-9,-262});
    rules[386] = new Rule(-262, new int[]{46,-173,-236,-205,10,-208});
    rules[387] = new Rule(-262, new int[]{46,-173,-236,-205,10,-213,10,-208});
    rules[388] = new Rule(-262, new int[]{-6,46,-173,-236,-205,10,-208});
    rules[389] = new Rule(-262, new int[]{-6,46,-173,-236,-205,10,-213,10,-208});
    rules[390] = new Rule(-262, new int[]{27,46,-173,-236,-214,10});
    rules[391] = new Rule(-262, new int[]{-6,27,46,-173,-236,-214,10});
    rules[392] = new Rule(-214, new int[]{110,-90});
    rules[393] = new Rule(-214, new int[]{});
    rules[394] = new Rule(-208, new int[]{});
    rules[395] = new Rule(-208, new int[]{63,10});
    rules[396] = new Rule(-236, new int[]{-241,5,-279});
    rules[397] = new Rule(-241, new int[]{});
    rules[398] = new Rule(-241, new int[]{11,-240,12});
    rules[399] = new Rule(-240, new int[]{-239});
    rules[400] = new Rule(-240, new int[]{-240,10,-239});
    rules[401] = new Rule(-239, new int[]{-161,5,-279});
    rules[402] = new Rule(-117, new int[]{-91});
    rules[403] = new Rule(-117, new int[]{});
    rules[404] = new Rule(-205, new int[]{});
    rules[405] = new Rule(-205, new int[]{85,-117,-206});
    rules[406] = new Rule(-205, new int[]{86,-264,-207});
    rules[407] = new Rule(-206, new int[]{});
    rules[408] = new Rule(-206, new int[]{86,-264});
    rules[409] = new Rule(-207, new int[]{});
    rules[410] = new Rule(-207, new int[]{85,-117});
    rules[411] = new Rule(-314, new int[]{-315,10});
    rules[412] = new Rule(-342, new int[]{110});
    rules[413] = new Rule(-342, new int[]{120});
    rules[414] = new Rule(-315, new int[]{-161,5,-280});
    rules[415] = new Rule(-315, new int[]{-161,110,-91});
    rules[416] = new Rule(-315, new int[]{-161,5,-280,-342,-89});
    rules[417] = new Rule(-89, new int[]{-88});
    rules[418] = new Rule(-89, new int[]{-84,6,-16});
    rules[419] = new Rule(-89, new int[]{-327});
    rules[420] = new Rule(-89, new int[]{-150,127,-332});
    rules[421] = new Rule(-89, new int[]{8,9,-328,127,-332});
    rules[422] = new Rule(-89, new int[]{8,-68,9,127,-332});
    rules[423] = new Rule(-89, new int[]{-246});
    rules[424] = new Rule(-88, new int[]{-87});
    rules[425] = new Rule(-220, new int[]{-230,-180});
    rules[426] = new Rule(-220, new int[]{30,-175,-131,110,-264,10});
    rules[427] = new Rule(-220, new int[]{-6,30,-175,-131,110,-264,10});
    rules[428] = new Rule(-221, new int[]{-230,-179});
    rules[429] = new Rule(-221, new int[]{30,-175,-131,110,-264,10});
    rules[430] = new Rule(-221, new int[]{-6,30,-175,-131,110,-264,10});
    rules[431] = new Rule(-217, new int[]{-224});
    rules[432] = new Rule(-217, new int[]{-6,-224});
    rules[433] = new Rule(-217, new int[]{19,-224});
    rules[434] = new Rule(-217, new int[]{19,-6,-224});
    rules[435] = new Rule(-217, new int[]{-6,19,-224});
    rules[436] = new Rule(-218, new int[]{-219});
    rules[437] = new Rule(-218, new int[]{19,-219});
    rules[438] = new Rule(-218, new int[]{-6,-219});
    rules[439] = new Rule(-218, new int[]{19,-6,-219});
    rules[440] = new Rule(-218, new int[]{-6,19,-219});
    rules[441] = new Rule(-224, new int[]{-231,-181});
    rules[442] = new Rule(-224, new int[]{-231,149,10});
    rules[443] = new Rule(-224, new int[]{37,-173,-131,5,-279,-211,110,-106,10});
    rules[444] = new Rule(-224, new int[]{37,-173,-131,-211,110,-106,10});
    rules[445] = new Rule(-224, new int[]{44,-174,-131,-211,110,-264,10});
    rules[446] = new Rule(-219, new int[]{-231,-179});
    rules[447] = new Rule(-219, new int[]{37,-173,-131,5,-279,-211,110,-106,10});
    rules[448] = new Rule(-219, new int[]{37,-173,-131,-211,110,-106,10});
    rules[449] = new Rule(-219, new int[]{44,-174,-131,-211,110,-264,10});
    rules[450] = new Rule(-181, new int[]{-180});
    rules[451] = new Rule(-181, new int[]{-63});
    rules[452] = new Rule(-174, new int[]{-173});
    rules[453] = new Rule(-173, new int[]{-145});
    rules[454] = new Rule(-173, new int[]{-338,7,-145});
    rules[455] = new Rule(-152, new int[]{-140});
    rules[456] = new Rule(-338, new int[]{-152});
    rules[457] = new Rule(-338, new int[]{-338,7,-152});
    rules[458] = new Rule(-145, new int[]{-140});
    rules[459] = new Rule(-145, new int[]{-194});
    rules[460] = new Rule(-145, new int[]{-194,-158});
    rules[461] = new Rule(-140, new int[]{-137});
    rules[462] = new Rule(-140, new int[]{-137,-158});
    rules[463] = new Rule(-137, new int[]{-150});
    rules[464] = new Rule(-228, new int[]{44,-174,-131,-210,-321});
    rules[465] = new Rule(-232, new int[]{37,-173,-131,-210,-321});
    rules[466] = new Rule(-232, new int[]{37,-173,-131,5,-279,-210,-321});
    rules[467] = new Rule(-63, new int[]{107,-111,80,-111,10});
    rules[468] = new Rule(-63, new int[]{107,-111,10});
    rules[469] = new Rule(-63, new int[]{107,10});
    rules[470] = new Rule(-111, new int[]{-150});
    rules[471] = new Rule(-111, new int[]{-168});
    rules[472] = new Rule(-180, new int[]{-43,-259,10});
    rules[473] = new Rule(-179, new int[]{-45,-259,10});
    rules[474] = new Rule(-179, new int[]{-63});
    rules[475] = new Rule(-131, new int[]{});
    rules[476] = new Rule(-131, new int[]{8,9});
    rules[477] = new Rule(-131, new int[]{8,-132,9});
    rules[478] = new Rule(-132, new int[]{-58});
    rules[479] = new Rule(-132, new int[]{-132,10,-58});
    rules[480] = new Rule(-58, new int[]{-9,-299});
    rules[481] = new Rule(-299, new int[]{-162,5,-279});
    rules[482] = new Rule(-299, new int[]{53,-162,5,-279});
    rules[483] = new Rule(-299, new int[]{29,-162,5,-279});
    rules[484] = new Rule(-299, new int[]{108,-162,5,-279});
    rules[485] = new Rule(-299, new int[]{-162,5,-279,110,-90});
    rules[486] = new Rule(-299, new int[]{53,-162,5,-279,110,-90});
    rules[487] = new Rule(-299, new int[]{29,-162,5,-279,110,-90});
    rules[488] = new Rule(-162, new int[]{-138});
    rules[489] = new Rule(-162, new int[]{-162,100,-138});
    rules[490] = new Rule(-138, new int[]{-150});
    rules[491] = new Rule(-279, new int[]{-280});
    rules[492] = new Rule(-281, new int[]{-276});
    rules[493] = new Rule(-281, new int[]{-260});
    rules[494] = new Rule(-281, new int[]{-253});
    rules[495] = new Rule(-281, new int[]{-285});
    rules[496] = new Rule(-281, new int[]{-304});
    rules[497] = new Rule(-265, new int[]{-264});
    rules[498] = new Rule(-265, new int[]{-146,5,-265});
    rules[499] = new Rule(-264, new int[]{});
    rules[500] = new Rule(-264, new int[]{-7});
    rules[501] = new Rule(-264, new int[]{-215});
    rules[502] = new Rule(-264, new int[]{-136});
    rules[503] = new Rule(-264, new int[]{-259});
    rules[504] = new Rule(-264, new int[]{-156});
    rules[505] = new Rule(-264, new int[]{-37});
    rules[506] = new Rule(-264, new int[]{-251});
    rules[507] = new Rule(-264, new int[]{-322});
    rules[508] = new Rule(-264, new int[]{-127});
    rules[509] = new Rule(-264, new int[]{-323});
    rules[510] = new Rule(-264, new int[]{-163});
    rules[511] = new Rule(-264, new int[]{-305});
    rules[512] = new Rule(-264, new int[]{-252});
    rules[513] = new Rule(-264, new int[]{-126});
    rules[514] = new Rule(-264, new int[]{-318});
    rules[515] = new Rule(-264, new int[]{-61});
    rules[516] = new Rule(-264, new int[]{-172});
    rules[517] = new Rule(-264, new int[]{-129});
    rules[518] = new Rule(-264, new int[]{-130});
    rules[519] = new Rule(-264, new int[]{-128});
    rules[520] = new Rule(-264, new int[]{-352});
    rules[521] = new Rule(-128, new int[]{72,-104,99,-264});
    rules[522] = new Rule(-129, new int[]{74,-106});
    rules[523] = new Rule(-130, new int[]{74,73,-106});
    rules[524] = new Rule(-318, new int[]{53,-315});
    rules[525] = new Rule(-318, new int[]{8,53,-150,100,-341,9,110,-90});
    rules[526] = new Rule(-318, new int[]{53,8,-150,100,-161,9,110,-90});
    rules[527] = new Rule(-7, new int[]{-116,-197,-91});
    rules[528] = new Rule(-7, new int[]{8,-114,100,-340,9,-197,-90});
    rules[529] = new Rule(-340, new int[]{-114});
    rules[530] = new Rule(-340, new int[]{-340,100,-114});
    rules[531] = new Rule(-341, new int[]{53,-150});
    rules[532] = new Rule(-341, new int[]{-341,100,53,-150});
    rules[533] = new Rule(-215, new int[]{-116});
    rules[534] = new Rule(-136, new int[]{57,-146});
    rules[535] = new Rule(-259, new int[]{91,-256,92});
    rules[536] = new Rule(-256, new int[]{-265});
    rules[537] = new Rule(-256, new int[]{-256,10,-265});
    rules[538] = new Rule(-156, new int[]{40,-104,51,-264});
    rules[539] = new Rule(-156, new int[]{40,-104,51,-264,32,-264});
    rules[540] = new Rule(-352, new int[]{38,-104,55,-354,-257,92});
    rules[541] = new Rule(-352, new int[]{38,-104,55,-354,10,-257,92});
    rules[542] = new Rule(-354, new int[]{-353});
    rules[543] = new Rule(-354, new int[]{-354,10,-353});
    rules[544] = new Rule(-353, new int[]{-346,39,-104,5,-264});
    rules[545] = new Rule(-353, new int[]{-345,5,-264});
    rules[546] = new Rule(-353, new int[]{-347,5,-264});
    rules[547] = new Rule(-353, new int[]{-348,39,-104,5,-264});
    rules[548] = new Rule(-353, new int[]{-348,5,-264});
    rules[549] = new Rule(-37, new int[]{25,-104,58,-38,-257,92});
    rules[550] = new Rule(-37, new int[]{25,-104,58,-38,10,-257,92});
    rules[551] = new Rule(-37, new int[]{25,-104,58,-257,92});
    rules[552] = new Rule(-38, new int[]{-266});
    rules[553] = new Rule(-38, new int[]{-38,10,-266});
    rules[554] = new Rule(-266, new int[]{-77,5,-264});
    rules[555] = new Rule(-77, new int[]{-113});
    rules[556] = new Rule(-77, new int[]{-77,100,-113});
    rules[557] = new Rule(-113, new int[]{-97});
    rules[558] = new Rule(-257, new int[]{});
    rules[559] = new Rule(-257, new int[]{32,-256});
    rules[560] = new Rule(-251, new int[]{97,-256,98,-90});
    rules[561] = new Rule(-322, new int[]{54,-104,-295,-264});
    rules[562] = new Rule(-295, new int[]{99});
    rules[563] = new Rule(-295, new int[]{});
    rules[564] = new Rule(-172, new int[]{60,-104,99,-264});
    rules[565] = new Rule(-364, new int[]{87,143});
    rules[566] = new Rule(-364, new int[]{});
    rules[567] = new Rule(-278, new int[]{5,-280});
    rules[568] = new Rule(-278, new int[]{});
    rules[569] = new Rule(-21, new int[]{53});
    rules[570] = new Rule(-21, new int[]{});
    rules[571] = new Rule(-122, new int[]{70});
    rules[572] = new Rule(-122, new int[]{71});
    rules[573] = new Rule(-126, new int[]{36,-150,-278,137,-104,-364,99,-264});
    rules[574] = new Rule(-126, new int[]{36,53,-150,-278,137,-104,-364,99,-264});
    rules[575] = new Rule(-126, new int[]{36,53,8,-161,9,137,-104,-364,99,-264});
    rules[576] = new Rule(-127, new int[]{35,-21,-150,-278,110,-104,-122,-104,-295,-264});
    rules[577] = new Rule(-127, new int[]{35,-21,-150,-278,110,-104,-122,-104,160,-104,99,-264});
    rules[578] = new Rule(-323, new int[]{55,-74,99,-264});
    rules[579] = new Rule(-163, new int[]{42});
    rules[580] = new Rule(-305, new int[]{102,-256,-293});
    rules[581] = new Rule(-293, new int[]{101,-256,92});
    rules[582] = new Rule(-293, new int[]{33,-62,92});
    rules[583] = new Rule(-62, new int[]{-65,-258});
    rules[584] = new Rule(-62, new int[]{-65,10,-258});
    rules[585] = new Rule(-62, new int[]{-256});
    rules[586] = new Rule(-65, new int[]{-64});
    rules[587] = new Rule(-65, new int[]{-65,10,-64});
    rules[588] = new Rule(-258, new int[]{});
    rules[589] = new Rule(-258, new int[]{32,-256});
    rules[590] = new Rule(-64, new int[]{79,-66,99,-264});
    rules[591] = new Rule(-66, new int[]{-182});
    rules[592] = new Rule(-66, new int[]{-143,5,-182});
    rules[593] = new Rule(-182, new int[]{-183});
    rules[594] = new Rule(-143, new int[]{-150});
    rules[595] = new Rule(-252, new int[]{47});
    rules[596] = new Rule(-252, new int[]{47,-90});
    rules[597] = new Rule(-74, new int[]{-91});
    rules[598] = new Rule(-74, new int[]{-74,100,-91});
    rules[599] = new Rule(-75, new int[]{-92});
    rules[600] = new Rule(-75, new int[]{-75,100,-92});
    rules[601] = new Rule(-61, new int[]{-177});
    rules[602] = new Rule(-177, new int[]{-176});
    rules[603] = new Rule(-91, new int[]{-90});
    rules[604] = new Rule(-91, new int[]{-326});
    rules[605] = new Rule(-91, new int[]{42});
    rules[606] = new Rule(-92, new int[]{-90});
    rules[607] = new Rule(-92, new int[]{-150,110,-104});
    rules[608] = new Rule(-92, new int[]{-326});
    rules[609] = new Rule(-92, new int[]{42});
    rules[610] = new Rule(-90, new int[]{-104});
    rules[611] = new Rule(-90, new int[]{-123});
    rules[612] = new Rule(-90, new int[]{-90,70,-104});
    rules[613] = new Rule(-104, new int[]{-102});
    rules[614] = new Rule(-104, new int[]{-244});
    rules[615] = new Rule(-104, new int[]{-246});
    rules[616] = new Rule(-120, new int[]{-102});
    rules[617] = new Rule(-120, new int[]{-244});
    rules[618] = new Rule(-121, new int[]{-102});
    rules[619] = new Rule(-121, new int[]{-246});
    rules[620] = new Rule(-106, new int[]{-104});
    rules[621] = new Rule(-106, new int[]{-326});
    rules[622] = new Rule(-107, new int[]{-102});
    rules[623] = new Rule(-107, new int[]{-244});
    rules[624] = new Rule(-107, new int[]{-326});
    rules[625] = new Rule(-102, new int[]{-101});
    rules[626] = new Rule(-102, new int[]{-102,16,-101});
    rules[627] = new Rule(-261, new int[]{21,8,-287,9});
    rules[628] = new Rule(-298, new int[]{22,8,-287,9});
    rules[629] = new Rule(-298, new int[]{22,8,-286,9});
    rules[630] = new Rule(-244, new int[]{-120,13,-120,5,-120});
    rules[631] = new Rule(-246, new int[]{40,-121,51,-121,32,-121});
    rules[632] = new Rule(-286, new int[]{-183,-303});
    rules[633] = new Rule(-286, new int[]{-183,4,-303});
    rules[634] = new Rule(-287, new int[]{-183});
    rules[635] = new Rule(-287, new int[]{-183,-302});
    rules[636] = new Rule(-287, new int[]{-183,4,-302});
    rules[637] = new Rule(-288, new int[]{-287});
    rules[638] = new Rule(-288, new int[]{-277});
    rules[639] = new Rule(-8, new int[]{8,-68,9});
    rules[640] = new Rule(-8, new int[]{});
    rules[641] = new Rule(-176, new int[]{78,-287,-73});
    rules[642] = new Rule(-176, new int[]{78,-287,11,-69,12,-8});
    rules[643] = new Rule(-176, new int[]{78,26,8,-337,9});
    rules[644] = new Rule(-336, new int[]{-150,110,-104});
    rules[645] = new Rule(-336, new int[]{-104});
    rules[646] = new Rule(-337, new int[]{-336});
    rules[647] = new Rule(-337, new int[]{-337,100,-336});
    rules[648] = new Rule(-72, new int[]{});
    rules[649] = new Rule(-72, new int[]{8,-69,9});
    rules[650] = new Rule(-73, new int[]{});
    rules[651] = new Rule(-73, new int[]{8,-70,9});
    rules[652] = new Rule(-101, new int[]{-108});
    rules[653] = new Rule(-101, new int[]{-101,-199,-108});
    rules[654] = new Rule(-101, new int[]{-101,-199,-246});
    rules[655] = new Rule(-101, new int[]{-270,8,-357,9});
    rules[656] = new Rule(-344, new int[]{-287,8,-357,9});
    rules[657] = new Rule(-346, new int[]{-287,8,-358,9});
    rules[658] = new Rule(-345, new int[]{-287,8,-358,9});
    rules[659] = new Rule(-345, new int[]{-361});
    rules[660] = new Rule(-361, new int[]{-343});
    rules[661] = new Rule(-361, new int[]{-361,100,-343});
    rules[662] = new Rule(-343, new int[]{-18});
    rules[663] = new Rule(-343, new int[]{-287});
    rules[664] = new Rule(-343, new int[]{56});
    rules[665] = new Rule(-343, new int[]{-261});
    rules[666] = new Rule(-343, new int[]{-298});
    rules[667] = new Rule(-347, new int[]{11,-359,12});
    rules[668] = new Rule(-359, new int[]{-349});
    rules[669] = new Rule(-359, new int[]{-359,100,-349});
    rules[670] = new Rule(-349, new int[]{-18});
    rules[671] = new Rule(-349, new int[]{-351});
    rules[672] = new Rule(-349, new int[]{14});
    rules[673] = new Rule(-349, new int[]{-346});
    rules[674] = new Rule(-349, new int[]{-347});
    rules[675] = new Rule(-349, new int[]{-348});
    rules[676] = new Rule(-349, new int[]{6});
    rules[677] = new Rule(-351, new int[]{53,-150});
    rules[678] = new Rule(-348, new int[]{8,-360,9});
    rules[679] = new Rule(-350, new int[]{14});
    rules[680] = new Rule(-350, new int[]{-18});
    rules[681] = new Rule(-350, new int[]{-202,-18});
    rules[682] = new Rule(-350, new int[]{53,-150});
    rules[683] = new Rule(-350, new int[]{-346});
    rules[684] = new Rule(-350, new int[]{-347});
    rules[685] = new Rule(-350, new int[]{-348});
    rules[686] = new Rule(-360, new int[]{-350});
    rules[687] = new Rule(-360, new int[]{-360,100,-350});
    rules[688] = new Rule(-358, new int[]{-356});
    rules[689] = new Rule(-358, new int[]{-358,10,-356});
    rules[690] = new Rule(-358, new int[]{-358,100,-356});
    rules[691] = new Rule(-357, new int[]{-355});
    rules[692] = new Rule(-357, new int[]{-357,10,-355});
    rules[693] = new Rule(-357, new int[]{-357,100,-355});
    rules[694] = new Rule(-355, new int[]{14});
    rules[695] = new Rule(-355, new int[]{-18});
    rules[696] = new Rule(-355, new int[]{53,-150,5,-280});
    rules[697] = new Rule(-355, new int[]{53,-150});
    rules[698] = new Rule(-355, new int[]{-344});
    rules[699] = new Rule(-355, new int[]{-347});
    rules[700] = new Rule(-355, new int[]{-348});
    rules[701] = new Rule(-356, new int[]{14});
    rules[702] = new Rule(-356, new int[]{-18});
    rules[703] = new Rule(-356, new int[]{-202,-18});
    rules[704] = new Rule(-356, new int[]{-150,5,-280});
    rules[705] = new Rule(-356, new int[]{-150});
    rules[706] = new Rule(-356, new int[]{53,-150,5,-280});
    rules[707] = new Rule(-356, new int[]{53,-150});
    rules[708] = new Rule(-356, new int[]{-346});
    rules[709] = new Rule(-356, new int[]{-347});
    rules[710] = new Rule(-356, new int[]{-348});
    rules[711] = new Rule(-118, new int[]{-108});
    rules[712] = new Rule(-118, new int[]{});
    rules[713] = new Rule(-125, new int[]{-93});
    rules[714] = new Rule(-125, new int[]{});
    rules[715] = new Rule(-123, new int[]{-108,5,-118});
    rules[716] = new Rule(-123, new int[]{5,-118});
    rules[717] = new Rule(-123, new int[]{-108,5,-118,5,-108});
    rules[718] = new Rule(-123, new int[]{5,-118,5,-108});
    rules[719] = new Rule(-124, new int[]{-93,5,-125});
    rules[720] = new Rule(-124, new int[]{5,-125});
    rules[721] = new Rule(-124, new int[]{-93,5,-125,5,-93});
    rules[722] = new Rule(-124, new int[]{5,-125,5,-93});
    rules[723] = new Rule(-199, new int[]{120});
    rules[724] = new Rule(-199, new int[]{125});
    rules[725] = new Rule(-199, new int[]{123});
    rules[726] = new Rule(-199, new int[]{121});
    rules[727] = new Rule(-199, new int[]{124});
    rules[728] = new Rule(-199, new int[]{122});
    rules[729] = new Rule(-199, new int[]{137});
    rules[730] = new Rule(-199, new int[]{135,137});
    rules[731] = new Rule(-108, new int[]{-86});
    rules[732] = new Rule(-108, new int[]{-108,6,-86});
    rules[733] = new Rule(-86, new int[]{-85});
    rules[734] = new Rule(-86, new int[]{-86,-200,-85});
    rules[735] = new Rule(-86, new int[]{-86,-200,-246});
    rules[736] = new Rule(-200, new int[]{116});
    rules[737] = new Rule(-200, new int[]{115});
    rules[738] = new Rule(-200, new int[]{128});
    rules[739] = new Rule(-200, new int[]{129});
    rules[740] = new Rule(-200, new int[]{126});
    rules[741] = new Rule(-204, new int[]{136});
    rules[742] = new Rule(-204, new int[]{138});
    rules[743] = new Rule(-268, new int[]{-270});
    rules[744] = new Rule(-268, new int[]{-271});
    rules[745] = new Rule(-271, new int[]{-85,136,-287});
    rules[746] = new Rule(-271, new int[]{-85,136,-282});
    rules[747] = new Rule(-270, new int[]{-85,138,-287});
    rules[748] = new Rule(-270, new int[]{-85,138,-282});
    rules[749] = new Rule(-272, new int[]{-100,119,-99});
    rules[750] = new Rule(-272, new int[]{-100,119,-272});
    rules[751] = new Rule(-272, new int[]{-202,-272});
    rules[752] = new Rule(-85, new int[]{-99});
    rules[753] = new Rule(-85, new int[]{-176});
    rules[754] = new Rule(-85, new int[]{-272});
    rules[755] = new Rule(-85, new int[]{-85,-201,-99});
    rules[756] = new Rule(-85, new int[]{-85,-201,-272});
    rules[757] = new Rule(-85, new int[]{-85,-201,-246});
    rules[758] = new Rule(-85, new int[]{-268});
    rules[759] = new Rule(-201, new int[]{118});
    rules[760] = new Rule(-201, new int[]{117});
    rules[761] = new Rule(-201, new int[]{131});
    rules[762] = new Rule(-201, new int[]{132});
    rules[763] = new Rule(-201, new int[]{133});
    rules[764] = new Rule(-201, new int[]{134});
    rules[765] = new Rule(-201, new int[]{130});
    rules[766] = new Rule(-59, new int[]{63,8,-288,9});
    rules[767] = new Rule(-60, new int[]{8,-105,100,-82,-328,-335,9});
    rules[768] = new Rule(-100, new int[]{-18});
    rules[769] = new Rule(-100, new int[]{-116});
    rules[770] = new Rule(-99, new int[]{56});
    rules[771] = new Rule(-99, new int[]{-18});
    rules[772] = new Rule(-99, new int[]{-59});
    rules[773] = new Rule(-99, new int[]{135,-99});
    rules[774] = new Rule(-99, new int[]{-202,-99});
    rules[775] = new Rule(-99, new int[]{142,-99});
    rules[776] = new Rule(-99, new int[]{-116});
    rules[777] = new Rule(-99, new int[]{-60});
    rules[778] = new Rule(-18, new int[]{-168});
    rules[779] = new Rule(-18, new int[]{-19});
    rules[780] = new Rule(-119, new int[]{-114,15,-114});
    rules[781] = new Rule(-119, new int[]{-114,15,-119});
    rules[782] = new Rule(-116, new int[]{-135,-114});
    rules[783] = new Rule(-116, new int[]{-114});
    rules[784] = new Rule(-116, new int[]{-119});
    rules[785] = new Rule(-116, new int[]{8,53,-150,110,-102,9});
    rules[786] = new Rule(-135, new int[]{141});
    rules[787] = new Rule(-135, new int[]{-135,141});
    rules[788] = new Rule(-12, new int[]{-183,-72});
    rules[789] = new Rule(-12, new int[]{-304,-72});
    rules[790] = new Rule(-325, new int[]{-150});
    rules[791] = new Rule(-325, new int[]{-325,7,-141});
    rules[792] = new Rule(-324, new int[]{-325});
    rules[793] = new Rule(-324, new int[]{-325,-302});
    rules[794] = new Rule(-362, new int[]{53,-150,110,-90,10});
    rules[795] = new Rule(-363, new int[]{-362});
    rules[796] = new Rule(-363, new int[]{-363,-362});
    rules[797] = new Rule(-115, new int[]{-114,8,-70,9});
    rules[798] = new Rule(-114, new int[]{-150});
    rules[799] = new Rule(-114, new int[]{-194});
    rules[800] = new Rule(-114, new int[]{42,-150});
    rules[801] = new Rule(-114, new int[]{8,-90,9});
    rules[802] = new Rule(-114, new int[]{8,-363,-90,9});
    rules[803] = new Rule(-114, new int[]{-261});
    rules[804] = new Rule(-114, new int[]{-298});
    rules[805] = new Rule(-114, new int[]{-18,7,-141});
    rules[806] = new Rule(-114, new int[]{-114,11,-74,12});
    rules[807] = new Rule(-114, new int[]{-18,11,-74,12});
    rules[808] = new Rule(-114, new int[]{-114,17,-123,12});
    rules[809] = new Rule(-114, new int[]{-18,17,-123,12});
    rules[810] = new Rule(-114, new int[]{76,-71,76});
    rules[811] = new Rule(-114, new int[]{-243});
    rules[812] = new Rule(-114, new int[]{-115});
    rules[813] = new Rule(-114, new int[]{-114,7,-151});
    rules[814] = new Rule(-114, new int[]{-60,7,-151});
    rules[815] = new Rule(-114, new int[]{-114,142});
    rules[816] = new Rule(-114, new int[]{-114,4,-302});
    rules[817] = new Rule(-69, new int[]{-74});
    rules[818] = new Rule(-69, new int[]{});
    rules[819] = new Rule(-70, new int[]{-75});
    rules[820] = new Rule(-70, new int[]{});
    rules[821] = new Rule(-71, new int[]{-80});
    rules[822] = new Rule(-71, new int[]{});
    rules[823] = new Rule(-80, new int[]{-95});
    rules[824] = new Rule(-80, new int[]{-80,100,-95});
    rules[825] = new Rule(-95, new int[]{-90});
    rules[826] = new Rule(-95, new int[]{-90,6,-90});
    rules[827] = new Rule(-169, new int[]{144});
    rules[828] = new Rule(-169, new int[]{147});
    rules[829] = new Rule(-168, new int[]{-170});
    rules[830] = new Rule(-168, new int[]{145});
    rules[831] = new Rule(-168, new int[]{146});
    rules[832] = new Rule(-170, new int[]{-169});
    rules[833] = new Rule(-170, new int[]{-170,-169});
    rules[834] = new Rule(-194, new int[]{45,-203});
    rules[835] = new Rule(-210, new int[]{10});
    rules[836] = new Rule(-210, new int[]{10,-209,10});
    rules[837] = new Rule(-211, new int[]{});
    rules[838] = new Rule(-211, new int[]{10,-209});
    rules[839] = new Rule(-209, new int[]{-212});
    rules[840] = new Rule(-209, new int[]{-209,10,-212});
    rules[841] = new Rule(-150, new int[]{143});
    rules[842] = new Rule(-150, new int[]{161});
    rules[843] = new Rule(-150, new int[]{-154});
    rules[844] = new Rule(-150, new int[]{-155});
    rules[845] = new Rule(-150, new int[]{160});
    rules[846] = new Rule(-150, new int[]{87});
    rules[847] = new Rule(-141, new int[]{-150});
    rules[848] = new Rule(-141, new int[]{-296});
    rules[849] = new Rule(-141, new int[]{-297});
    rules[850] = new Rule(-151, new int[]{-150});
    rules[851] = new Rule(-151, new int[]{-296});
    rules[852] = new Rule(-151, new int[]{-194});
    rules[853] = new Rule(-212, new int[]{148});
    rules[854] = new Rule(-212, new int[]{150});
    rules[855] = new Rule(-212, new int[]{151});
    rules[856] = new Rule(-212, new int[]{152});
    rules[857] = new Rule(-212, new int[]{154});
    rules[858] = new Rule(-212, new int[]{153});
    rules[859] = new Rule(-213, new int[]{153});
    rules[860] = new Rule(-213, new int[]{152});
    rules[861] = new Rule(-213, new int[]{148});
    rules[862] = new Rule(-213, new int[]{151});
    rules[863] = new Rule(-154, new int[]{85});
    rules[864] = new Rule(-154, new int[]{86});
    rules[865] = new Rule(-155, new int[]{80});
    rules[866] = new Rule(-155, new int[]{78});
    rules[867] = new Rule(-153, new int[]{84});
    rules[868] = new Rule(-153, new int[]{83});
    rules[869] = new Rule(-153, new int[]{82});
    rules[870] = new Rule(-153, new int[]{81});
    rules[871] = new Rule(-296, new int[]{-153});
    rules[872] = new Rule(-296, new int[]{68});
    rules[873] = new Rule(-296, new int[]{64});
    rules[874] = new Rule(-296, new int[]{128});
    rules[875] = new Rule(-296, new int[]{22});
    rules[876] = new Rule(-296, new int[]{21});
    rules[877] = new Rule(-296, new int[]{63});
    rules[878] = new Rule(-296, new int[]{23});
    rules[879] = new Rule(-296, new int[]{129});
    rules[880] = new Rule(-296, new int[]{130});
    rules[881] = new Rule(-296, new int[]{131});
    rules[882] = new Rule(-296, new int[]{132});
    rules[883] = new Rule(-296, new int[]{133});
    rules[884] = new Rule(-296, new int[]{134});
    rules[885] = new Rule(-296, new int[]{135});
    rules[886] = new Rule(-296, new int[]{136});
    rules[887] = new Rule(-296, new int[]{137});
    rules[888] = new Rule(-296, new int[]{138});
    rules[889] = new Rule(-296, new int[]{24});
    rules[890] = new Rule(-296, new int[]{73});
    rules[891] = new Rule(-296, new int[]{91});
    rules[892] = new Rule(-296, new int[]{25});
    rules[893] = new Rule(-296, new int[]{26});
    rules[894] = new Rule(-296, new int[]{29});
    rules[895] = new Rule(-296, new int[]{30});
    rules[896] = new Rule(-296, new int[]{31});
    rules[897] = new Rule(-296, new int[]{71});
    rules[898] = new Rule(-296, new int[]{99});
    rules[899] = new Rule(-296, new int[]{32});
    rules[900] = new Rule(-296, new int[]{92});
    rules[901] = new Rule(-296, new int[]{33});
    rules[902] = new Rule(-296, new int[]{34});
    rules[903] = new Rule(-296, new int[]{27});
    rules[904] = new Rule(-296, new int[]{104});
    rules[905] = new Rule(-296, new int[]{101});
    rules[906] = new Rule(-296, new int[]{35});
    rules[907] = new Rule(-296, new int[]{36});
    rules[908] = new Rule(-296, new int[]{37});
    rules[909] = new Rule(-296, new int[]{40});
    rules[910] = new Rule(-296, new int[]{41});
    rules[911] = new Rule(-296, new int[]{42});
    rules[912] = new Rule(-296, new int[]{103});
    rules[913] = new Rule(-296, new int[]{43});
    rules[914] = new Rule(-296, new int[]{44});
    rules[915] = new Rule(-296, new int[]{46});
    rules[916] = new Rule(-296, new int[]{47});
    rules[917] = new Rule(-296, new int[]{48});
    rules[918] = new Rule(-296, new int[]{97});
    rules[919] = new Rule(-296, new int[]{49});
    rules[920] = new Rule(-296, new int[]{102});
    rules[921] = new Rule(-296, new int[]{50});
    rules[922] = new Rule(-296, new int[]{28});
    rules[923] = new Rule(-296, new int[]{51});
    rules[924] = new Rule(-296, new int[]{70});
    rules[925] = new Rule(-296, new int[]{98});
    rules[926] = new Rule(-296, new int[]{52});
    rules[927] = new Rule(-296, new int[]{53});
    rules[928] = new Rule(-296, new int[]{54});
    rules[929] = new Rule(-296, new int[]{55});
    rules[930] = new Rule(-296, new int[]{56});
    rules[931] = new Rule(-296, new int[]{57});
    rules[932] = new Rule(-296, new int[]{58});
    rules[933] = new Rule(-296, new int[]{59});
    rules[934] = new Rule(-296, new int[]{61});
    rules[935] = new Rule(-296, new int[]{105});
    rules[936] = new Rule(-296, new int[]{106});
    rules[937] = new Rule(-296, new int[]{109});
    rules[938] = new Rule(-296, new int[]{107});
    rules[939] = new Rule(-296, new int[]{108});
    rules[940] = new Rule(-296, new int[]{62});
    rules[941] = new Rule(-296, new int[]{74});
    rules[942] = new Rule(-296, new int[]{38});
    rules[943] = new Rule(-296, new int[]{39});
    rules[944] = new Rule(-296, new int[]{69});
    rules[945] = new Rule(-296, new int[]{148});
    rules[946] = new Rule(-296, new int[]{60});
    rules[947] = new Rule(-296, new int[]{139});
    rules[948] = new Rule(-296, new int[]{140});
    rules[949] = new Rule(-296, new int[]{79});
    rules[950] = new Rule(-296, new int[]{153});
    rules[951] = new Rule(-296, new int[]{152});
    rules[952] = new Rule(-296, new int[]{72});
    rules[953] = new Rule(-296, new int[]{154});
    rules[954] = new Rule(-296, new int[]{150});
    rules[955] = new Rule(-296, new int[]{151});
    rules[956] = new Rule(-296, new int[]{149});
    rules[957] = new Rule(-297, new int[]{45});
    rules[958] = new Rule(-203, new int[]{115});
    rules[959] = new Rule(-203, new int[]{116});
    rules[960] = new Rule(-203, new int[]{117});
    rules[961] = new Rule(-203, new int[]{118});
    rules[962] = new Rule(-203, new int[]{120});
    rules[963] = new Rule(-203, new int[]{121});
    rules[964] = new Rule(-203, new int[]{122});
    rules[965] = new Rule(-203, new int[]{123});
    rules[966] = new Rule(-203, new int[]{124});
    rules[967] = new Rule(-203, new int[]{125});
    rules[968] = new Rule(-203, new int[]{128});
    rules[969] = new Rule(-203, new int[]{129});
    rules[970] = new Rule(-203, new int[]{130});
    rules[971] = new Rule(-203, new int[]{131});
    rules[972] = new Rule(-203, new int[]{132});
    rules[973] = new Rule(-203, new int[]{133});
    rules[974] = new Rule(-203, new int[]{134});
    rules[975] = new Rule(-203, new int[]{135});
    rules[976] = new Rule(-203, new int[]{137});
    rules[977] = new Rule(-203, new int[]{139});
    rules[978] = new Rule(-203, new int[]{140});
    rules[979] = new Rule(-203, new int[]{-197});
    rules[980] = new Rule(-203, new int[]{119});
    rules[981] = new Rule(-197, new int[]{110});
    rules[982] = new Rule(-197, new int[]{111});
    rules[983] = new Rule(-197, new int[]{112});
    rules[984] = new Rule(-197, new int[]{113});
    rules[985] = new Rule(-197, new int[]{114});
    rules[986] = new Rule(-103, new int[]{18,-26,100,-25,9});
    rules[987] = new Rule(-25, new int[]{-103});
    rules[988] = new Rule(-25, new int[]{-150});
    rules[989] = new Rule(-26, new int[]{-25});
    rules[990] = new Rule(-26, new int[]{-26,100,-25});
    rules[991] = new Rule(-105, new int[]{-104});
    rules[992] = new Rule(-105, new int[]{-103});
    rules[993] = new Rule(-82, new int[]{-105});
    rules[994] = new Rule(-82, new int[]{-82,100,-105});
    rules[995] = new Rule(-326, new int[]{-150,127,-332});
    rules[996] = new Rule(-326, new int[]{8,9,-329,127,-332});
    rules[997] = new Rule(-326, new int[]{8,-150,5,-279,9,-329,127,-332});
    rules[998] = new Rule(-326, new int[]{8,-150,10,-330,9,-329,127,-332});
    rules[999] = new Rule(-326, new int[]{8,-150,5,-279,10,-330,9,-329,127,-332});
    rules[1000] = new Rule(-326, new int[]{8,-105,100,-82,-328,-335,9,-339});
    rules[1001] = new Rule(-326, new int[]{-103,-339});
    rules[1002] = new Rule(-326, new int[]{-327});
    rules[1003] = new Rule(-335, new int[]{});
    rules[1004] = new Rule(-335, new int[]{10,-330});
    rules[1005] = new Rule(-339, new int[]{-329,127,-332});
    rules[1006] = new Rule(-327, new int[]{37,-329,127,-332});
    rules[1007] = new Rule(-327, new int[]{37,8,9,-329,127,-332});
    rules[1008] = new Rule(-327, new int[]{37,8,-330,9,-329,127,-332});
    rules[1009] = new Rule(-327, new int[]{44,127,-333});
    rules[1010] = new Rule(-327, new int[]{44,8,9,127,-333});
    rules[1011] = new Rule(-327, new int[]{44,8,-330,9,127,-333});
    rules[1012] = new Rule(-330, new int[]{-331});
    rules[1013] = new Rule(-330, new int[]{-330,10,-331});
    rules[1014] = new Rule(-331, new int[]{-161,-328});
    rules[1015] = new Rule(-328, new int[]{});
    rules[1016] = new Rule(-328, new int[]{5,-279});
    rules[1017] = new Rule(-329, new int[]{});
    rules[1018] = new Rule(-329, new int[]{5,-281});
    rules[1019] = new Rule(-334, new int[]{-259});
    rules[1020] = new Rule(-334, new int[]{-156});
    rules[1021] = new Rule(-334, new int[]{-322});
    rules[1022] = new Rule(-334, new int[]{-251});
    rules[1023] = new Rule(-334, new int[]{-127});
    rules[1024] = new Rule(-334, new int[]{-126});
    rules[1025] = new Rule(-334, new int[]{-128});
    rules[1026] = new Rule(-334, new int[]{-37});
    rules[1027] = new Rule(-334, new int[]{-305});
    rules[1028] = new Rule(-334, new int[]{-172});
    rules[1029] = new Rule(-334, new int[]{-252});
    rules[1030] = new Rule(-334, new int[]{-129});
    rules[1031] = new Rule(-334, new int[]{8,-7,9});
    rules[1032] = new Rule(-332, new int[]{-107});
    rules[1033] = new Rule(-332, new int[]{-334});
    rules[1034] = new Rule(-333, new int[]{-215});
    rules[1035] = new Rule(-333, new int[]{-7});
    rules[1036] = new Rule(-333, new int[]{-334});
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
      case 54: // interface_part -> tkInterface, uses_clause, unit_reexport_clauses, 
               //                   interface_decl_sect_list
{ 
            var defs = (declarations)ValueStack[ValueStack.Depth-1].stn;
            defs.defs.InsertRange(0, ((declarations)ValueStack[ValueStack.Depth-2].stn).defs);
            CurrentSemanticValue.stn = new interface_node(defs, ValueStack[ValueStack.Depth-3].stn as uses_list, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]));
        }
        break;
      case 55: // unit_reexport_clauses -> /* empty */
{ CurrentSemanticValue.stn = new declarations(); }
        break;
      case 56: // unit_reexport_clauses -> unit_reexport_clauses, tkExport, unit_reexport_list, 
               //                          tkSemiColon
{
            var defs = (declarations)ValueStack[ValueStack.Depth-4].stn;
            defs.defs.AddRange(((declarations)ValueStack[ValueStack.Depth-2].stn).defs);
            CurrentSemanticValue.stn = defs;
        }
        break;
      case 57: // unit_reexport_list -> unit_reexport_item
{ CurrentSemanticValue.stn = new declarations().Add((declaration)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); }
        break;
      case 58: // unit_reexport_list -> unit_reexport_list, tkComma, unit_reexport_item
{ CurrentSemanticValue.stn = ((declarations)ValueStack[ValueStack.Depth-3].stn).Add((declaration)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); }
        break;
      case 59: // unit_reexport_item -> identifier, tkPoint, identifier
{ CurrentSemanticValue.stn = new unit_reexport(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan); }
        break;
      case 60: // unit_reexport_item -> identifier, tkPoint, identifier, tkAs, identifier
{ CurrentSemanticValue.stn = new unit_reexport(ValueStack[ValueStack.Depth-5].id, ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); }
        break;
      case 61: // implementation_part -> tkImplementation, uses_clause, decl_sect_list
{ 
			CurrentSemanticValue.stn = new implementation_node(ValueStack[ValueStack.Depth-2].stn as uses_list, ValueStack[ValueStack.Depth-1].stn as declarations, null, LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1])); 
        }
        break;
      case 62: // abc_interface_part -> uses_clause, decl_sect_list
{ 
			CurrentSemanticValue.stn = new interface_node(ValueStack[ValueStack.Depth-1].stn as declarations, ValueStack[ValueStack.Depth-2].stn as uses_list, null, null); 
        }
        break;
      case 63: // initialization_part -> tkEnd
{ 
			CurrentSemanticValue.stn = new initfinal_part(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 64: // initialization_part -> tkInitialization, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 65: // initialization_part -> tkInitialization, stmt_list, tkFinalization, stmt_list, 
               //                        tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-5].ti, ValueStack[ValueStack.Depth-4].stn as statement_list, ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, CurrentLocationSpan);
        }
        break;
      case 66: // initialization_part -> tkBegin, stmt_list, tkEnd
{ 
		  CurrentSemanticValue.stn = new initfinal_part(ValueStack[ValueStack.Depth-3].ti, ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].ti, null, null, CurrentLocationSpan);
        }
        break;
      case 67: // interface_decl_sect_list -> int_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 68: // int_decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations();  
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 69: // int_decl_sect_list1 -> int_decl_sect_list1, int_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 70: // decl_sect_list -> decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 71: // decl_sect_list_proc_func_only -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 72: // decl_sect_list_proc_func_only -> decl_sect_list_proc_func_only, 
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
      case 73: // decl_sect_list1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = CurrentSemanticValue.stn as declarations;
		}
        break;
      case 74: // decl_sect_list1 -> decl_sect_list1, decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 75: // inclass_decl_sect_list -> inclass_decl_sect_list1
{
			if ((ValueStack[ValueStack.Depth-1].stn as declarations).Count > 0) 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
			else 
				CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 76: // inclass_decl_sect_list1 -> /* empty */
{ 
        	CurrentSemanticValue.stn = new declarations(); 
        }
        break;
      case 77: // inclass_decl_sect_list1 -> inclass_decl_sect_list1, abc_decl_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as declarations).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 78: // int_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 79: // int_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 80: // int_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 81: // int_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 82: // int_decl_sect -> int_proc_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 83: // int_decl_sect -> int_func_header
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 84: // decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 85: // decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 86: // decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 87: // decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 88: // decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 89: // decl_sect -> proc_func_constr_destr_decl_with_attr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 90: // proc_func_constr_destr_decl -> proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 91: // proc_func_constr_destr_decl -> constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 92: // proc_func_constr_destr_decl_with_attr -> attribute_declarations, 
               //                                          proc_func_constr_destr_decl
{
		    (ValueStack[ValueStack.Depth-1].stn as procedure_definition).AssignAttrList(ValueStack[ValueStack.Depth-2].stn as attribute_list);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 93: // abc_decl_sect -> label_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 94: // abc_decl_sect -> const_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 95: // abc_decl_sect -> res_str_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 96: // abc_decl_sect -> type_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 97: // abc_decl_sect -> var_decl_sect
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 98: // int_proc_header -> attribute_declarations, proc_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 99: // int_proc_header -> attribute_declarations, proc_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 100: // int_func_header -> attribute_declarations, func_header
{  
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
			(CurrentSemanticValue.td as procedure_header).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
        }
        break;
      case 101: // int_func_header -> attribute_declarations, func_header, tkForward, tkSemiColon
{  
			CurrentSemanticValue.td = NewProcedureHeader(ValueStack[ValueStack.Depth-4].stn as attribute_list, ValueStack[ValueStack.Depth-3].td as procedure_header, ValueStack[ValueStack.Depth-2].id as procedure_attribute, CurrentLocationSpan);
		}
        break;
      case 102: // label_decl_sect -> tkLabel, label_list, tkSemiColon
{ 
			CurrentSemanticValue.stn = new label_definitions(ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
		}
        break;
      case 103: // label_list -> label_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 104: // label_list -> label_list, tkComma, label_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 105: // label_name -> tkInteger
{ 
			CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ex.ToString(), CurrentLocationSpan);
		}
        break;
      case 106: // label_name -> identifier
{ 
			CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; 
		}
        break;
      case 107: // const_decl_sect -> tkConst, const_decl
{ 
			CurrentSemanticValue.stn = new consts_definitions_list(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 108: // const_decl_sect -> const_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as consts_definitions_list).Add(ValueStack[ValueStack.Depth-1].stn as const_definition, CurrentLocationSpan);
		}
        break;
      case 109: // res_str_decl_sect -> tkResourceString, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 110: // res_str_decl_sect -> res_str_decl_sect, const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 111: // type_decl_sect -> tkType, type_decl
{ 
            CurrentSemanticValue.stn = new type_declarations(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 112: // type_decl_sect -> type_decl_sect, type_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as type_declarations).Add(ValueStack[ValueStack.Depth-1].stn as type_declaration, CurrentLocationSpan);
		}
        break;
      case 113: // var_decl_with_assign_var_tuple -> var_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 114: // var_decl_with_assign_var_tuple -> tkRoundOpen, identifier, tkComma, ident_list, 
                //                                   tkRoundClose, tkAssign, expr_l1, 
                //                                   tkSemiColon
{
			(ValueStack[ValueStack.Depth-5].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-7].id);
			ValueStack[ValueStack.Depth-5].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4]);
			CurrentSemanticValue.stn = new var_tuple_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
		}
        break;
      case 115: // var_decl_sect -> tkVar, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 116: // var_decl_sect -> tkEvent, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = new variable_definitions(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);                        
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).is_event = true;
        }
        break;
      case 117: // var_decl_sect -> var_decl_sect, var_decl_with_assign_var_tuple
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as variable_definitions).Add(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 118: // const_decl -> only_const_decl, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 119: // only_const_decl -> const_name, tkEqual, init_const_expr
{ 
			CurrentSemanticValue.stn = new simple_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 120: // only_const_decl -> const_name, tkColon, type_ref, tkEqual, typed_const
{ 
			CurrentSemanticValue.stn = new typed_const_definition(ValueStack[ValueStack.Depth-5].id, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-3].td, CurrentLocationSpan);
		}
        break;
      case 121: // init_const_expr -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 122: // init_const_expr -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 123: // const_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 124: // const_relop_expr -> const_simple_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 125: // const_relop_expr -> const_relop_expr, const_relop, const_simple_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 126: // const_expr -> const_relop_expr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 127: // const_expr -> question_constexpr
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 128: // const_expr -> const_expr, tkDoubleQuestion, const_relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 129: // question_constexpr -> const_expr, tkQuestion, const_expr, tkColon, const_expr
{ CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 130: // const_relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 131: // const_relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 132: // const_relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 133: // const_relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 134: // const_relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 135: // const_relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 136: // const_relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 137: // const_relop -> tkNot, tkIn
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
      case 138: // const_simple_expr -> const_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 139: // const_simple_expr -> const_simple_expr, const_addop, const_term
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 140: // const_addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 141: // const_addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 142: // const_addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 143: // const_addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 144: // as_is_constexpr -> const_term, typecast_op, simple_or_template_type_reference
{ 
			CurrentSemanticValue.ex = NewAsIsConstexpr(ValueStack[ValueStack.Depth-3].ex, (op_typecast)ValueStack[ValueStack.Depth-2].ob, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);                                
		}
        break;
      case 145: // power_constexpr -> const_factor_without_unary_op, tkStarStar, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 146: // power_constexpr -> const_factor_without_unary_op, tkStarStar, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 147: // power_constexpr -> sign, power_constexpr
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 148: // const_term -> const_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 149: // const_term -> as_is_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 150: // const_term -> power_constexpr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 151: // const_term -> const_term, const_mulop, const_factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 152: // const_term -> const_term, const_mulop, power_constexpr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 153: // const_mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 154: // const_mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 155: // const_mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 156: // const_mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 157: // const_mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 158: // const_mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 159: // const_mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 160: // const_factor_without_unary_op -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 161: // const_factor_without_unary_op -> tkRoundOpen, const_expr, tkRoundClose
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 162: // const_factor -> const_variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 163: // const_factor -> const_set
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 164: // const_factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 165: // const_factor -> tkAddressOf, const_factor
{ 
			CurrentSemanticValue.ex = new get_address(ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);  
		}
        break;
      case 166: // const_factor -> tkRoundOpen, const_expr, tkRoundClose
{ 
	 	    CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 167: // const_factor -> tkNot, const_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 168: // const_factor -> sign, const_factor
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
      case 169: // const_factor -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 170: // const_factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 171: // pascal_set_const -> tkSquareOpen, elem_list, tkSquareClose
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
      case 172: // const_set -> pascal_set_const
{
            CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;				
		}
        break;
      case 173: // const_set -> tkVertParen, elem_list, tkVertParen
{ 
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, '|', CurrentLocationSpan);  
		}
        break;
      case 174: // sign -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 175: // sign -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 176: // const_variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 177: // const_variable -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 178: // const_variable -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 179: // const_variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 180: // const_variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 181: // const_variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 182: // const_variable -> const_variable, const_variable_2
{
			CurrentSemanticValue.ex = NewConstVariable(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 183: // const_variable -> const_variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 184: // const_variable -> const_variable, tkSquareOpen, format_const_expr, 
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
      case 185: // const_variable_2 -> tkPoint, identifier_or_keyword
{ 
			CurrentSemanticValue.ex = new dot_node(null, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 186: // const_variable_2 -> tkDeref
{ 
			CurrentSemanticValue.ex = new roof_dereference();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 187: // const_variable_2 -> tkRoundOpen, optional_const_func_expr_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 188: // const_variable_2 -> tkSquareOpen, const_elem_list, tkSquareClose
{ 
			CurrentSemanticValue.ex = new indexer(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);  
		}
        break;
      case 189: // optional_const_func_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 190: // optional_const_func_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 191: // const_elem_list -> const_elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 193: // const_elem_list1 -> const_elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 194: // const_elem_list1 -> const_elem_list1, tkComma, const_elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 195: // const_elem -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 196: // const_elem -> const_expr, tkDotDot, const_expr
{ 
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 197: // unsigned_number -> tkInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 198: // unsigned_number -> tkHex
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 199: // unsigned_number -> tkFloat
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 200: // unsigned_number -> tkBigInteger
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 201: // typed_const -> const_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 202: // typed_const -> array_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 203: // typed_const -> record_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 204: // array_const -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new array_const(ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 206: // typed_const_list -> typed_const_list1
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 207: // typed_const_list1 -> typed_const_plus
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
        }
        break;
      case 208: // typed_const_list1 -> typed_const_list1, tkComma, typed_const_plus
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 209: // record_const -> tkRoundOpen, const_field_list, tkRoundClose
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 210: // const_field_list -> const_field_list_1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 211: // const_field_list -> const_field_list_1, tkSemiColon
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex; }
        break;
      case 212: // const_field_list_1 -> const_field
{ 
			CurrentSemanticValue.ex = new record_const(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 213: // const_field_list_1 -> const_field_list_1, tkSemiColon, const_field
{ 
			CurrentSemanticValue.ex = (ValueStack[ValueStack.Depth-3].ex as record_const).Add(ValueStack[ValueStack.Depth-1].stn as record_const_definition, CurrentLocationSpan);
		}
        break;
      case 214: // const_field -> const_field_name, tkColon, typed_const
{ 
			CurrentSemanticValue.stn = new record_const_definition(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 215: // const_field_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 216: // type_decl -> attribute_declarations, simple_type_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = LocationStack[LocationStack.Depth-1];
        }
        break;
      case 217: // attribute_declarations -> attribute_declaration
{ 
			CurrentSemanticValue.stn = new attribute_list(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
    }
        break;
      case 218: // attribute_declarations -> attribute_declarations, attribute_declaration
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as simple_attribute_list, CurrentLocationSpan);
		}
        break;
      case 219: // attribute_declarations -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 220: // attribute_declaration -> tkSquareOpen, one_or_some_attribute, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 221: // one_or_some_attribute -> one_attribute
{ 
			CurrentSemanticValue.stn = new simple_attribute_list(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 222: // one_or_some_attribute -> one_or_some_attribute, tkComma, one_attribute
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as simple_attribute_list).Add(ValueStack[ValueStack.Depth-1].stn as attribute, CurrentLocationSpan);
		}
        break;
      case 223: // one_attribute -> attribute_variable
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 224: // one_attribute -> identifier, tkColon, attribute_variable
{  
			(ValueStack[ValueStack.Depth-1].stn as attribute).qualifier = ValueStack[ValueStack.Depth-3].id;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 225: // simple_type_decl -> type_decl_identifier, tkEqual, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-4].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 226: // simple_type_decl -> template_identifier_with_equal, type_decl_type, tkSemiColon
{ 
			CurrentSemanticValue.stn = new type_declaration(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan); 
		}
        break;
      case 227: // type_decl_identifier -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 228: // type_decl_identifier -> identifier, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 229: // template_identifier_with_equal -> identifier, tkLower, ident_list, 
                //                                   tkGreaterEqual
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-4].id.name, ValueStack[ValueStack.Depth-2].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 230: // type_decl_type -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 231: // type_decl_type -> object_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 232: // simple_type_question -> simple_type, tkQuestion
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
      case 233: // simple_type_question -> template_type, tkQuestion
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
      case 234: // type_ref -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 235: // type_ref -> simple_type_question
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 236: // type_ref -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 237: // type_ref -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 238: // type_ref -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 239: // type_ref -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 240: // type_ref -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 241: // template_type -> simple_type_identifier, template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference(ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan); 
		}
        break;
      case 242: // template_type_params -> tkLower, template_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 243: // template_type_empty_params -> tkNotEqual
{
            var ntr = new named_type_reference(new ident(""), CurrentLocationSpan);
            
			CurrentSemanticValue.stn = new template_param_list(ntr, CurrentLocationSpan);
            ntr.source_context = new SourceContext(CurrentSemanticValue.stn.source_context.end_position.line_num, CurrentSemanticValue.stn.source_context.end_position.column_num, CurrentSemanticValue.stn.source_context.begin_position.line_num, CurrentSemanticValue.stn.source_context.begin_position.column_num);
		}
        break;
      case 244: // template_type_empty_params -> tkLower, template_empty_param_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 245: // template_param_list -> template_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 246: // template_param_list -> template_param_list, tkComma, template_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 247: // template_empty_param_list -> template_empty_param
{ 
			CurrentSemanticValue.stn = new template_param_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 248: // template_empty_param_list -> template_empty_param_list, tkComma, 
                //                              template_empty_param
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as template_param_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 249: // template_empty_param -> /* empty */
{ 
            CurrentSemanticValue.td = new named_type_reference(new ident(""), CurrentLocationSpan);
        }
        break;
      case 250: // template_param -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 251: // template_param -> simple_type, tkQuestion
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
      case 252: // template_param -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 253: // template_param -> procedural_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 254: // template_param -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 255: // simple_type -> range_expr
{
	    	CurrentSemanticValue.td = parserTools.ConvertDotNodeOrIdentToNamedTypeReference(ValueStack[ValueStack.Depth-1].ex); 
	    }
        break;
      case 256: // simple_type -> range_expr, tkDotDot, range_expr
{ 
			CurrentSemanticValue.td = new diapason(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 257: // simple_type -> tkRoundOpen, enumeration_id_list, tkRoundClose
{ 
			CurrentSemanticValue.td = new enum_type_definition(ValueStack[ValueStack.Depth-2].stn as enumerator_list, CurrentLocationSpan);  
		}
        break;
      case 258: // range_expr -> range_term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 259: // range_expr -> range_expr, const_addop, range_term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 260: // range_term -> range_factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 261: // range_term -> range_term, const_mulop, range_factor
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 262: // range_factor -> simple_type_identifier
{ 
			CurrentSemanticValue.ex = parserTools.ConvertNamedTypeReferenceToDotNodeOrIdent(ValueStack[ValueStack.Depth-1].td as named_type_reference);
        }
        break;
      case 263: // range_factor -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 264: // range_factor -> sign, range_factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 265: // range_factor -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 266: // range_factor -> range_factor, tkRoundOpen, const_elem_list, tkRoundClose
{ 
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value, ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 267: // simple_type_identifier -> identifier
{ 
			CurrentSemanticValue.td = new named_type_reference(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 268: // simple_type_identifier -> simple_type_identifier, tkPoint, 
                //                           identifier_or_keyword
{ 
			CurrentSemanticValue.td = (ValueStack[ValueStack.Depth-3].td as named_type_reference).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 269: // enumeration_id_list -> enumeration_id
{ 
			CurrentSemanticValue.stn = new enumerator_list(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 270: // enumeration_id_list -> enumeration_id_list, tkComma, enumeration_id
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as enumerator_list).Add(ValueStack[ValueStack.Depth-1].stn as enumerator, CurrentLocationSpan);
        }
        break;
      case 271: // enumeration_id -> type_ref
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan); 
		}
        break;
      case 272: // enumeration_id -> type_ref, tkEqual, expr
{ 
			CurrentSemanticValue.stn = new enumerator(ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 273: // pointer_type -> tkDeref, fptype
{ 
			CurrentSemanticValue.td = new ref_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 274: // structured_type -> array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 275: // structured_type -> record_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 276: // structured_type -> set_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 277: // structured_type -> file_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 278: // structured_type -> sequence_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 279: // sequence_type -> tkSequence, tkOf, type_ref
{
			CurrentSemanticValue.td = new sequence_type(ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
		}
        break;
      case 280: // array_type -> tkArray, tkSquareOpen, simple_type_list, tkSquareClose, tkOf, 
                //               type_ref
{ 
			CurrentSemanticValue.td = new array_type(ValueStack[ValueStack.Depth-4].stn as indexers_types, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 281: // array_type -> unsized_array_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 282: // unsized_array_type -> tkArray, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new array_type(null, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
        }
        break;
      case 283: // simple_type_list -> simple_type_or_
{ 
			CurrentSemanticValue.stn = new indexers_types(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 284: // simple_type_list -> simple_type_list, tkComma, simple_type_or_
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as indexers_types).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 285: // simple_type_or_ -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 286: // simple_type_or_ -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 287: // set_type -> tkSet, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new set_type_definition(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 288: // file_type -> tkFile, tkOf, type_ref
{ 
			CurrentSemanticValue.td = new file_type(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 289: // file_type -> tkFile
{ 
			CurrentSemanticValue.td = new file_type();  
			CurrentSemanticValue.td.source_context = CurrentLocationSpan;
		}
        break;
      case 290: // string_type -> tkIdentifier, tkSquareOpen, const_expr, tkSquareClose
{ 
			CurrentSemanticValue.td = new string_num_definition(ValueStack[ValueStack.Depth-2].ex, ValueStack[ValueStack.Depth-4].id, CurrentLocationSpan);
		}
        break;
      case 291: // procedural_type -> procedural_type_kind
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 292: // procedural_type_kind -> proc_type_decl
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 293: // proc_type_decl -> tkProcedure, fp_list
{ 
			CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-1].stn as formal_parameters,null,null,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 294: // proc_type_decl -> tkFunction, fp_list, tkColon, fptype
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, null, null, null, ValueStack[ValueStack.Depth-1].td as type_definition, CurrentLocationSpan);
        }
        break;
      case 295: // proc_type_decl -> simple_type_identifier, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 296: // proc_type_decl -> template_type, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-3].td,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);            
    	}
        break;
      case 297: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 298: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   template_param
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-4].stn as enumerator_list,ValueStack[ValueStack.Depth-1].td,CurrentLocationSpan);
    	}
        break;
      case 299: // proc_type_decl -> simple_type_identifier, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 300: // proc_type_decl -> template_type, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(ValueStack[ValueStack.Depth-4].td,null,null,CurrentLocationSpan);
    	}
        break;
      case 301: // proc_type_decl -> tkRoundOpen, tkRoundClose, tkArrow, tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,null,null,CurrentLocationSpan);
    	}
        break;
      case 302: // proc_type_decl -> tkRoundOpen, enumeration_id_list, tkRoundClose, tkArrow, 
                //                   tkRoundOpen, tkRoundClose
{
    		CurrentSemanticValue.td = new modern_proc_type(null,ValueStack[ValueStack.Depth-5].stn as enumerator_list,null,CurrentLocationSpan);
    	}
        break;
      case 303: // object_type -> class_attributes, class_or_interface_keyword, 
                //                optional_base_classes, optional_where_section, 
                //                optional_component_list_seq_end
{ 
            var cd = NewObjectType((class_attribute)ValueStack[ValueStack.Depth-5].ob, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].stn as named_type_reference_list, ValueStack[ValueStack.Depth-2].stn as where_definition_list, ValueStack[ValueStack.Depth-1].stn as class_body_list, CurrentLocationSpan); 
			CurrentSemanticValue.td = cd;
		}
        break;
      case 304: // record_type -> tkRecord, optional_base_classes, optional_where_section, 
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
      case 305: // class_attribute -> tkSealed
{ CurrentSemanticValue.ob = class_attribute.Sealed; }
        break;
      case 306: // class_attribute -> tkPartial
{ CurrentSemanticValue.ob = class_attribute.Partial; }
        break;
      case 307: // class_attribute -> tkAbstract
{ CurrentSemanticValue.ob = class_attribute.Abstract; }
        break;
      case 308: // class_attribute -> tkAuto
{ CurrentSemanticValue.ob = class_attribute.Auto; }
        break;
      case 309: // class_attribute -> tkStatic
{ CurrentSemanticValue.ob = class_attribute.Static; }
        break;
      case 310: // class_attributes -> /* empty */
{ 
			CurrentSemanticValue.ob = class_attribute.None; 
		}
        break;
      case 311: // class_attributes -> class_attributes1
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 312: // class_attributes1 -> class_attribute
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ob;
		}
        break;
      case 313: // class_attributes1 -> class_attributes1, class_attribute
{
            if (((class_attribute)ValueStack[ValueStack.Depth-2].ob & (class_attribute)ValueStack[ValueStack.Depth-1].ob) == (class_attribute)ValueStack[ValueStack.Depth-1].ob)
                parserTools.AddErrorFromResource("ATTRIBUTE_REDECLARED",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.ob  = ((class_attribute)ValueStack[ValueStack.Depth-2].ob) | ((class_attribute)ValueStack[ValueStack.Depth-1].ob);
			//$$ = $1;
		}
        break;
      case 314: // class_or_interface_keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 315: // class_or_interface_keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 316: // class_or_interface_keyword -> tkTemplate
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-1].ti);
		}
        break;
      case 317: // class_or_interface_keyword -> tkTemplate, tkClass
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "c", CurrentLocationSpan);
		}
        break;
      case 318: // class_or_interface_keyword -> tkTemplate, tkRecord
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "r", CurrentLocationSpan);
		}
        break;
      case 319: // class_or_interface_keyword -> tkTemplate, tkInterface
{ 
			CurrentSemanticValue.ti = NewClassOrInterfaceKeyword(ValueStack[ValueStack.Depth-2].ti, "i", CurrentLocationSpan);
		}
        break;
      case 320: // optional_component_list_seq_end -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 321: // optional_component_list_seq_end -> member_list_section, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 323: // optional_base_classes -> tkRoundOpen, base_classes_names_list, tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 324: // base_classes_names_list -> base_class_name
{ 
			CurrentSemanticValue.stn = new named_type_reference_list(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 325: // base_classes_names_list -> base_classes_names_list, tkComma, base_class_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as named_type_reference_list).Add(ValueStack[ValueStack.Depth-1].stn as named_type_reference, CurrentLocationSpan);
		}
        break;
      case 326: // base_class_name -> simple_type_identifier
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 327: // base_class_name -> template_type
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 328: // template_arguments -> tkLower, ident_list, tkGreater
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 329: // optional_where_section -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 330: // optional_where_section -> where_part_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 331: // where_part_list -> where_part
{ 
			CurrentSemanticValue.stn = new where_definition_list(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 332: // where_part_list -> where_part_list, where_part
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as where_definition_list).Add(ValueStack[ValueStack.Depth-1].stn as where_definition, CurrentLocationSpan);
		}
        break;
      case 333: // where_part -> tkWhere, ident_list, tkColon, type_ref_and_secific_list, 
                //               tkSemiColon
{ 
			CurrentSemanticValue.stn = new where_definition(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-2].stn as where_type_specificator_list, CurrentLocationSpan); 
		}
        break;
      case 334: // type_ref_and_secific_list -> type_ref_or_secific
{ 
			CurrentSemanticValue.stn = new where_type_specificator_list(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 335: // type_ref_and_secific_list -> type_ref_and_secific_list, tkComma, 
                //                              type_ref_or_secific
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as where_type_specificator_list).Add(ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
		}
        break;
      case 336: // type_ref_or_secific -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 337: // type_ref_or_secific -> tkClass
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefClass, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 338: // type_ref_or_secific -> tkRecord
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 339: // type_ref_or_secific -> tkConstructor
{ 
			CurrentSemanticValue.td = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); 
		}
        break;
      case 340: // member_list_section -> member_list
{ 
			CurrentSemanticValue.stn = new class_body_list(ValueStack[ValueStack.Depth-1].stn as class_members, CurrentLocationSpan);
        }
        break;
      case 341: // member_list_section -> member_list_section, ot_visibility_specifier, 
                //                        member_list
{ 
		    (ValueStack[ValueStack.Depth-1].stn as class_members).access_mod = ValueStack[ValueStack.Depth-2].stn as access_modifer_node;
			(ValueStack[ValueStack.Depth-3].stn as class_body_list).Add(ValueStack[ValueStack.Depth-1].stn as class_members,CurrentLocationSpan);
			
			if ((ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks[0].Count == 0)
                (ValueStack[ValueStack.Depth-3].stn as class_body_list).class_def_blocks.RemoveAt(0);
			
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 342: // ot_visibility_specifier -> tkInternal
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.internal_modifer, CurrentLocationSpan); }
        break;
      case 343: // ot_visibility_specifier -> tkPublic
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.public_modifer, CurrentLocationSpan); }
        break;
      case 344: // ot_visibility_specifier -> tkProtected
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.protected_modifer, CurrentLocationSpan); }
        break;
      case 345: // ot_visibility_specifier -> tkPrivate
{ CurrentSemanticValue.stn = new access_modifer_node(access_modifer.private_modifer, CurrentLocationSpan); }
        break;
      case 346: // member_list -> /* empty */
{ CurrentSemanticValue.stn = new class_members(); }
        break;
      case 347: // member_list -> field_or_const_definition_list, optional_semicolon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 348: // member_list -> method_decl_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 349: // member_list -> field_or_const_definition_list, tkSemiColon, method_decl_list
{  
			(ValueStack[ValueStack.Depth-3].stn as class_members).members.AddRange((ValueStack[ValueStack.Depth-1].stn as class_members).members);
			(ValueStack[ValueStack.Depth-3].stn as class_members).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-3].stn;
        }
        break;
      case 350: // ident_list -> identifier
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 351: // ident_list -> ident_list, tkComma, identifier
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);
		}
        break;
      case 352: // optional_semicolon -> /* empty */
{ CurrentSemanticValue.ob = null; }
        break;
      case 353: // optional_semicolon -> tkSemiColon
{ CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 354: // field_or_const_definition_list -> field_or_const_definition
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 355: // field_or_const_definition_list -> field_or_const_definition_list, tkSemiColon, 
                //                                   field_or_const_definition
{   
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 356: // field_or_const_definition -> attribute_declarations, 
                //                              simple_field_or_const_definition
{  
		    (ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 357: // method_decl_list -> method_or_property_decl
{ 
			CurrentSemanticValue.stn = new class_members(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 358: // method_decl_list -> method_decl_list, method_or_property_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-2].stn as class_members).Add(ValueStack[ValueStack.Depth-1].stn as declaration, CurrentLocationSpan);
        }
        break;
      case 359: // method_or_property_decl -> method_decl_withattr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 360: // method_or_property_decl -> property_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 361: // simple_field_or_const_definition -> tkConst, only_const_decl
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 362: // simple_field_or_const_definition -> field_definition
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 363: // simple_field_or_const_definition -> class_or_static, field_definition
{ 
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).var_attr = definition_attribute.Static;
			(ValueStack[ValueStack.Depth-1].stn as var_def_statement).source_context = CurrentLocationSpan;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 364: // class_or_static -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 365: // class_or_static -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 366: // field_definition -> var_decl_part
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 367: // field_definition -> tkEvent, ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, true, CurrentLocationSpan); 
        }
        break;
      case 368: // method_decl_withattr -> attribute_declarations, method_header
{  
			(ValueStack[ValueStack.Depth-1].td as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td;
        }
        break;
      case 369: // method_decl_withattr -> attribute_declarations, method_decl
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
            if (ValueStack[ValueStack.Depth-1].stn is procedure_definition && (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
                (ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.attributes = ValueStack[ValueStack.Depth-2].stn as attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
     }
        break;
      case 370: // method_decl -> inclass_proc_func_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 371: // method_decl -> inclass_constr_destr_decl
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 372: // method_header -> class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 373: // method_header -> method_procfunc_header
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
		}
        break;
      case 374: // method_header -> tkAsync, class_or_static, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).class_keyword = true;
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 375: // method_header -> tkAsync, method_procfunc_header
{ 
			(ValueStack[ValueStack.Depth-1].td as procedure_header).IsAsync = true;
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; 
		}
        break;
      case 376: // method_header -> constr_destr_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 377: // method_procfunc_header -> proc_func_header
{ 
			CurrentSemanticValue.td = NewProcfuncHeading(ValueStack[ValueStack.Depth-1].td as procedure_header);
		}
        break;
      case 378: // proc_func_header -> proc_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 379: // proc_func_header -> func_header
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 380: // constr_destr_header -> tkConstructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 381: // constr_destr_header -> class_or_static, tkConstructor, optional_proc_name, 
                //                        fp_list, optional_method_modificators
{ 
			CurrentSemanticValue.td = new constructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name,false,true,null,null,CurrentLocationSpan);
        }
        break;
      case 382: // constr_destr_header -> tkDestructor, optional_proc_name, fp_list, 
                //                        optional_method_modificators
{ 
			CurrentSemanticValue.td = new destructor(null,ValueStack[ValueStack.Depth-2].stn as formal_parameters,ValueStack[ValueStack.Depth-1].stn as procedure_attributes_list,ValueStack[ValueStack.Depth-3].stn as method_name, false,false,null,null,CurrentLocationSpan);
        }
        break;
      case 383: // optional_proc_name -> proc_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 384: // optional_proc_name -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 385: // property_definition -> attribute_declarations, simple_property_definition
{  
			CurrentSemanticValue.stn = NewPropertyDefinition(ValueStack[ValueStack.Depth-2].stn as attribute_list, ValueStack[ValueStack.Depth-1].stn as declaration, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 386: // simple_property_definition -> tkProperty, func_name, property_interface, 
                //                               property_specifiers, tkSemiColon, 
                //                               array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        }
        break;
      case 387: // simple_property_definition -> tkProperty, func_name, property_interface, 
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
      case 388: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, array_defaultproperty
{ 
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-5].stn as method_name, ValueStack[ValueStack.Depth-4].stn as property_interface, ValueStack[ValueStack.Depth-3].stn as property_accessors, proc_attribute.attr_none, ValueStack[ValueStack.Depth-1].stn as property_array_default, CurrentLocationSpan);
        	(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
        }
        break;
      case 389: // simple_property_definition -> class_or_static, tkProperty, func_name, 
                //                               property_interface, property_specifiers, 
                //                               tkSemiColon, property_modificator, tkSemiColon, 
                //                               array_defaultproperty
{ 
			parserTools.AddErrorFromResource("STATIC_PROPERTIES_CANNOT_HAVE_ATTRBUTE_{0}",LocationStack[LocationStack.Depth-3],ValueStack[ValueStack.Depth-3].id.name);        	
        }
        break;
      case 390: // simple_property_definition -> tkAuto, tkProperty, func_name, property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 391: // simple_property_definition -> class_or_static, tkAuto, tkProperty, func_name, 
                //                               property_interface, 
                //                               optional_property_initialization, tkSemiColon
{
			CurrentSemanticValue.stn = NewSimplePropertyDefinition(ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-3].stn as property_interface, null, proc_attribute.attr_none, null, CurrentLocationSpan);
			(CurrentSemanticValue.stn as simple_property).is_auto = true;
			(CurrentSemanticValue.stn as simple_property).attr = definition_attribute.Static;
			(CurrentSemanticValue.stn as simple_property).initial_value = ValueStack[ValueStack.Depth-2].ex;
		}
        break;
      case 392: // optional_property_initialization -> tkAssign, expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 393: // optional_property_initialization -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 394: // array_defaultproperty -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 395: // array_defaultproperty -> tkDefault, tkSemiColon
{ 
			CurrentSemanticValue.stn = new property_array_default();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 396: // property_interface -> property_parameter_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_interface(ValueStack[ValueStack.Depth-3].stn as property_parameter_list, ValueStack[ValueStack.Depth-1].td, null, CurrentLocationSpan);
        }
        break;
      case 397: // property_parameter_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 398: // property_parameter_list -> tkSquareOpen, parameter_decl_list, tkSquareClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 399: // parameter_decl_list -> parameter_decl
{ 
			CurrentSemanticValue.stn = new property_parameter_list(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 400: // parameter_decl_list -> parameter_decl_list, tkSemiColon, parameter_decl
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as property_parameter_list).Add(ValueStack[ValueStack.Depth-1].stn as property_parameter, CurrentLocationSpan);
		}
        break;
      case 401: // parameter_decl -> ident_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new property_parameter(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 402: // optional_read_expr -> expr_with_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 403: // optional_read_expr -> /* empty */
{ CurrentSemanticValue.ex = null; }
        break;
      case 405: // property_specifiers -> tkRead, optional_read_expr, write_property_specifiers
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
      case 406: // property_specifiers -> tkWrite, unlabelled_stmt, read_property_specifiers
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
      case 408: // write_property_specifiers -> tkWrite, unlabelled_stmt
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
      case 410: // read_property_specifiers -> tkRead, optional_read_expr
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
      case 411: // var_decl -> var_decl_part, tkSemiColon
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 414: // var_decl_part -> ident_list, tkColon, type_ref
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, null, definition_attribute.None, false, CurrentLocationSpan);
		}
        break;
      case 415: // var_decl_part -> ident_list, tkAssign, expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-3].stn as ident_list, null, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan);		
		}
        break;
      case 416: // var_decl_part -> ident_list, tkColon, type_ref, tkAssignOrEqual, 
                //                  typed_var_init_expression
{ 
			CurrentSemanticValue.stn = new var_def_statement(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].ex, definition_attribute.None, false, CurrentLocationSpan); 
		}
        break;
      case 417: // typed_var_init_expression -> typed_const_plus
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 418: // typed_var_init_expression -> const_simple_expr, tkDotDot, const_term
{ 
		if (parserTools.buildTreeForFormatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
		}
        break;
      case 419: // typed_var_init_expression -> expl_func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 420: // typed_var_init_expression -> identifier, tkArrow, lambda_function_body
{  
			var idList = new ident_list(ValueStack[ValueStack.Depth-3].id, LocationStack[LocationStack.Depth-3]); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), parametr_kind.none, null, LocationStack[LocationStack.Depth-3]), LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new lambda_any_type_node_syntax(), LocationStack[LocationStack.Depth-3]), ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 421: // typed_var_init_expression -> tkRoundOpen, tkRoundClose, lambda_type_ref, 
                //                              tkArrow, lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, CurrentLocationSpan);
		}
        break;
      case 422: // typed_var_init_expression -> tkRoundOpen, typed_const_list, tkRoundClose, 
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
      case 423: // typed_var_init_expression -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 424: // typed_const_plus -> typed_const
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 425: // constr_destr_decl -> constr_destr_header, block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 426: // constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, tkAssign, 
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
      case 427: // constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
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
      case 428: // inclass_constr_destr_decl -> constr_destr_header, inclass_block
{ 
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as block, CurrentLocationSpan);
        }
        break;
      case 429: // inclass_constr_destr_decl -> tkConstructor, optional_proc_name, fp_list, 
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
      case 430: // inclass_constr_destr_decl -> class_or_static, tkConstructor, optional_proc_name, 
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
      case 431: // proc_func_decl -> proc_func_decl_without_modifiers
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 432: // proc_func_decl -> class_or_static, proc_func_decl_without_modifiers
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 433: // proc_func_decl -> tkAsync, proc_func_decl_without_modifiers
{
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;		
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 434: // proc_func_decl -> tkAsync, class_or_static, proc_func_decl_without_modifiers
{ 
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 435: // proc_func_decl -> class_or_static, tkAsync, proc_func_decl_without_modifiers
{ 
        	(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 436: // inclass_proc_func_decl -> inclass_proc_func_decl_without_modifiers
{ 
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 437: // inclass_proc_func_decl -> tkAsync, inclass_proc_func_decl_without_modifiers
{ 
			(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.IsAsync = true;
            CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
        }
        break;
      case 438: // inclass_proc_func_decl -> class_or_static, 
                //                           inclass_proc_func_decl_without_modifiers
{ 
		    if ((ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header != null)
		    {
				(ValueStack[ValueStack.Depth-1].stn as procedure_definition).proc_header.class_keyword = true;
			}
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 439: // inclass_proc_func_decl -> tkAsync, class_or_static, 
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
      case 440: // inclass_proc_func_decl -> class_or_static, tkAsync, 
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
      case 441: // proc_func_decl_without_modifiers -> proc_func_header, proc_func_external_block
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
      case 442: // proc_func_decl_without_modifiers -> proc_func_header, tkForward, tkSemiColon
{
			CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-3].td as procedure_header, null, CurrentLocationSpan);
            (CurrentSemanticValue.stn as procedure_definition).proc_header.proc_attributes.Add((procedure_attribute)ValueStack[ValueStack.Depth-2].id, ValueStack[ValueStack.Depth-2].id.source_context);
		}
        break;
      case 443: // proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, tkColon, 
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
      case 444: // proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, 
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
      case 445: // proc_func_decl_without_modifiers -> tkProcedure, proc_name, fp_list, 
                //                                     optional_method_modificators1, tkAssign, 
                //                                     unlabelled_stmt, tkSemiColon
{
			if (ValueStack[ValueStack.Depth-2].stn is empty_statement)
				parserTools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",LocationStack[LocationStack.Depth-2]);
			CurrentSemanticValue.stn = SyntaxTreeBuilder.BuildShortProcDefinition(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-4].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-7].Merge(LocationStack[LocationStack.Depth-4]));
		}
        break;
      case 446: // inclass_proc_func_decl_without_modifiers -> proc_func_header, inclass_block
{
            CurrentSemanticValue.stn = new procedure_definition(ValueStack[ValueStack.Depth-2].td as procedure_header, ValueStack[ValueStack.Depth-1].stn as proc_block, CurrentLocationSpan);
		}
        break;
      case 447: // inclass_proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, 
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
      case 448: // inclass_proc_func_decl_without_modifiers -> tkFunction, func_name, fp_list, 
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
      case 449: // inclass_proc_func_decl_without_modifiers -> tkProcedure, proc_name, fp_list, 
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
      case 450: // proc_func_external_block -> block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 451: // proc_func_external_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 452: // proc_name -> func_name
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 453: // func_name -> func_meth_name_ident
{ 
			CurrentSemanticValue.stn = new method_name(null,null, ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan); 
		}
        break;
      case 454: // func_name -> func_class_name_ident_list, tkPoint, func_meth_name_ident
{ 
        	var ln = ValueStack[ValueStack.Depth-3].ob as List<ident>;
        	var cnt = ln.Count;
        	if (cnt == 1)
				CurrentSemanticValue.stn = new method_name(null, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
			else 	
				CurrentSemanticValue.stn = new method_name(ln, ln[cnt-1], ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
		}
        break;
      case 455: // func_class_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 456: // func_class_name_ident_list -> func_class_name_ident
{ 
			CurrentSemanticValue.ob = new List<ident>(); 
			(CurrentSemanticValue.ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
		}
        break;
      case 457: // func_class_name_ident_list -> func_class_name_ident_list, tkPoint, 
                //                               func_class_name_ident
{ 
			(ValueStack[ValueStack.Depth-3].ob as List<ident>).Add(ValueStack[ValueStack.Depth-1].id);
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob; 
		}
        break;
      case 458: // func_meth_name_ident -> func_name_with_template_args
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 459: // func_meth_name_ident -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 460: // func_meth_name_ident -> operator_name_ident, template_arguments
{ CurrentSemanticValue.id = new template_operator_name(null, ValueStack[ValueStack.Depth-1].stn as ident_list, ValueStack[ValueStack.Depth-2].ex as operator_name_ident, CurrentLocationSpan); }
        break;
      case 461: // func_name_with_template_args -> func_name_ident
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 462: // func_name_with_template_args -> func_name_ident, template_arguments
{ 
			CurrentSemanticValue.id = new template_type_name(ValueStack[ValueStack.Depth-2].id.name, ValueStack[ValueStack.Depth-1].stn as ident_list, CurrentLocationSpan); 
        }
        break;
      case 463: // func_name_ident -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 464: // proc_header -> tkProcedure, proc_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{ 
        	CurrentSemanticValue.td = new procedure_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, CurrentLocationSpan); 
        }
        break;
      case 465: // func_header -> tkFunction, func_name, fp_list, optional_method_modificators, 
                //                optional_where_section
{
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-3].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-4].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, null, CurrentLocationSpan); 
		}
        break;
      case 466: // func_header -> tkFunction, func_name, fp_list, tkColon, fptype, 
                //                optional_method_modificators, optional_where_section
{ 
			CurrentSemanticValue.td = new function_header(ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-2].stn as procedure_attributes_list, ValueStack[ValueStack.Depth-6].stn as method_name, ValueStack[ValueStack.Depth-1].stn as where_definition_list, ValueStack[ValueStack.Depth-3].td as type_definition, CurrentLocationSpan); 
        }
        break;
      case 467: // external_block -> tkExternal, external_directive_ident, tkName, 
                //                   external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan); 
		}
        break;
      case 468: // external_block -> tkExternal, external_directive_ident, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(ValueStack[ValueStack.Depth-2].ex, null, CurrentLocationSpan); 
		}
        break;
      case 469: // external_block -> tkExternal, tkSemiColon
{ 
			CurrentSemanticValue.stn = new external_directive(null, null, CurrentLocationSpan); 
		}
        break;
      case 470: // external_directive_ident -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 471: // external_directive_ident -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 472: // block -> decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 473: // inclass_block -> inclass_decl_sect_list, compound_stmt, tkSemiColon
{ 
			CurrentSemanticValue.stn = new block(ValueStack[ValueStack.Depth-3].stn as declarations, ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan); 
		}
        break;
      case 474: // inclass_block -> external_block
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 475: // fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 476: // fp_list -> tkRoundOpen, tkRoundClose
{ 
			CurrentSemanticValue.stn = null;
		}
        break;
      case 477: // fp_list -> tkRoundOpen, fp_sect_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			if (CurrentSemanticValue.stn != null)
				CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 478: // fp_sect_list -> fp_sect
{ 
			CurrentSemanticValue.stn = new formal_parameters(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
        }
        break;
      case 479: // fp_sect_list -> fp_sect_list, tkSemiColon, fp_sect
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);   
        }
        break;
      case 480: // fp_sect -> attribute_declarations, simple_fp_sect
{  
			(ValueStack[ValueStack.Depth-1].stn as declaration).attributes = ValueStack[ValueStack.Depth-2].stn as  attribute_list;
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
        }
        break;
      case 481: // simple_fp_sect -> param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan); 
		}
        break;
      case 482: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.var_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 483: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.const_parametr, null, CurrentLocationSpan);  
		}
        break;
      case 484: // simple_fp_sect -> tkParams, param_name_list, tkColon, fptype
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-3].stn as ident_list, ValueStack[ValueStack.Depth-1].td,parametr_kind.params_parametr,null, CurrentLocationSpan);  
		}
        break;
      case 485: // simple_fp_sect -> param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.none, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 486: // simple_fp_sect -> tkVar, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.var_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 487: // simple_fp_sect -> tkConst, param_name_list, tkColon, fptype, tkAssign, expr
{ 
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-5].stn as ident_list, ValueStack[ValueStack.Depth-3].td, parametr_kind.const_parametr, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 488: // param_name_list -> param_name
{ 
			CurrentSemanticValue.stn = new ident_list(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 489: // param_name_list -> param_name_list, tkComma, param_name
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as ident_list).Add(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan);  
		}
        break;
      case 490: // param_name -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 491: // fptype -> type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 492: // fptype_noproctype -> simple_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 493: // fptype_noproctype -> string_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 494: // fptype_noproctype -> pointer_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 495: // fptype_noproctype -> structured_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 496: // fptype_noproctype -> template_type
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 497: // stmt -> unlabelled_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 498: // stmt -> label_name, tkColon, stmt
{ 
			CurrentSemanticValue.stn = new labeled_statement(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);  
		}
        break;
      case 499: // unlabelled_stmt -> /* empty */
{ 
			CurrentSemanticValue.stn = new empty_statement(); 
			CurrentSemanticValue.stn.source_context = null;
		}
        break;
      case 500: // unlabelled_stmt -> assignment
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 501: // unlabelled_stmt -> proc_call
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 502: // unlabelled_stmt -> goto_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 503: // unlabelled_stmt -> compound_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 504: // unlabelled_stmt -> if_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 505: // unlabelled_stmt -> case_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 506: // unlabelled_stmt -> repeat_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 507: // unlabelled_stmt -> while_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 508: // unlabelled_stmt -> for_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 509: // unlabelled_stmt -> with_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 510: // unlabelled_stmt -> inherited_message
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 511: // unlabelled_stmt -> try_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 512: // unlabelled_stmt -> raise_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 513: // unlabelled_stmt -> foreach_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 514: // unlabelled_stmt -> var_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 515: // unlabelled_stmt -> expr_as_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 516: // unlabelled_stmt -> lock_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 517: // unlabelled_stmt -> yield_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 518: // unlabelled_stmt -> yield_sequence_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 519: // unlabelled_stmt -> loop_stmt
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 520: // unlabelled_stmt -> match_with
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 521: // loop_stmt -> tkLoop, expr_l1, tkDo, unlabelled_stmt
{
			CurrentSemanticValue.stn = new loop_stmt(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].stn as statement,CurrentLocationSpan);
		}
        break;
      case 522: // yield_stmt -> tkYield, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 523: // yield_sequence_stmt -> tkYield, tkSequence, expr_l1_func_decl_lambda
{
			CurrentSemanticValue.stn = new yield_sequence_node(ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 524: // var_stmt -> tkVar, var_decl_part
{ 
			CurrentSemanticValue.stn = new var_statement(ValueStack[ValueStack.Depth-1].stn as var_def_statement, CurrentLocationSpan);
		}
        break;
      case 525: // var_stmt -> tkRoundOpen, tkVar, identifier, tkComma, var_ident_list, 
                //             tkRoundClose, tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].ob as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].ob as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 526: // var_stmt -> tkVar, tkRoundOpen, identifier, tkComma, ident_list, tkRoundClose, 
                //             tkAssign, expr
{
			(ValueStack[ValueStack.Depth-4].stn as ident_list).Insert(0,ValueStack[ValueStack.Depth-6].id);
			ValueStack[ValueStack.Depth-4].stn.source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-8],LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_var_tuple(ValueStack[ValueStack.Depth-4].stn as ident_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
	    }
        break;
      case 527: // assignment -> var_reference, assign_operator, expr_with_func_decl_lambda
{      
        	if (!(ValueStack[ValueStack.Depth-3].ex is addressed_value))
        		parserTools.AddErrorFromResource("LEFT_SIDE_CANNOT_BE_ASSIGNED_TO",CurrentLocationSpan);
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan);
        }
        break;
      case 528: // assignment -> tkRoundOpen, variable, tkComma, variable_list, tkRoundClose, 
                //               assign_operator, expr
{
			if (ValueStack[ValueStack.Depth-2].op.type != Operators.Assignment)
			    parserTools.AddErrorFromResource("ONLY_BASE_ASSIGNMENT_FOR_TUPLE",LocationStack[LocationStack.Depth-2]);
			(ValueStack[ValueStack.Depth-4].ob as addressed_value_list).Insert(0,ValueStack[ValueStack.Depth-6].ex as addressed_value);
			(ValueStack[ValueStack.Depth-4].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-7],LocationStack[LocationStack.Depth-6],LocationStack[LocationStack.Depth-5],LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.stn = new assign_tuple(ValueStack[ValueStack.Depth-4].ob as addressed_value_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 529: // variable_list -> variable
{
		CurrentSemanticValue.ob = new addressed_value_list(ValueStack[ValueStack.Depth-1].ex as addressed_value,LocationStack[LocationStack.Depth-1]);
	}
        break;
      case 530: // variable_list -> variable_list, tkComma, variable
{
		(ValueStack[ValueStack.Depth-3].ob as addressed_value_list).Add(ValueStack[ValueStack.Depth-1].ex as addressed_value);
		(ValueStack[ValueStack.Depth-3].ob as syntax_tree_node).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
	}
        break;
      case 531: // var_ident_list -> tkVar, identifier
{
		CurrentSemanticValue.ob = new ident_list(ValueStack[ValueStack.Depth-1].id,CurrentLocationSpan);
	}
        break;
      case 532: // var_ident_list -> var_ident_list, tkComma, tkVar, identifier
{
		(ValueStack[ValueStack.Depth-4].ob as ident_list).Add(ValueStack[ValueStack.Depth-1].id);
		(ValueStack[ValueStack.Depth-4].ob as ident_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-4],LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-2],LocationStack[LocationStack.Depth-1]);
		CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-4].ob;
	}
        break;
      case 533: // proc_call -> var_reference
{ 
			CurrentSemanticValue.stn = new procedure_call(ValueStack[ValueStack.Depth-1].ex as addressed_value, ValueStack[ValueStack.Depth-1].ex is ident, CurrentLocationSpan); 
		}
        break;
      case 534: // goto_stmt -> tkGoto, label_name
{ 
			CurrentSemanticValue.stn = new goto_statement(ValueStack[ValueStack.Depth-1].id, CurrentLocationSpan); 
		}
        break;
      case 535: // compound_stmt -> tkBegin, stmt_list, tkEnd
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
			(CurrentSemanticValue.stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(CurrentSemanticValue.stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
        }
        break;
      case 536: // stmt_list -> stmt
{ 
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, LocationStack[LocationStack.Depth-1]);
        }
        break;
      case 537: // stmt_list -> stmt_list, tkSemiColon, stmt
{  
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as statement_list).Add(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
        }
        break;
      case 538: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan); 
        }
        break;
      case 539: // if_stmt -> tkIf, expr_l1, tkThen, unlabelled_stmt, tkElse, unlabelled_stmt
{
			CurrentSemanticValue.stn = new if_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as statement, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 540: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, else_case, tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 541: // match_with -> tkMatch, expr_l1, tkWith, pattern_cases, tkSemiColon, else_case, 
                //               tkEnd
{ 
            CurrentSemanticValue.stn = new match_with(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as pattern_cases, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan);
        }
        break;
      case 542: // pattern_cases -> pattern_case
{
            CurrentSemanticValue.stn = new pattern_cases(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 543: // pattern_cases -> pattern_cases, tkSemiColon, pattern_case
{
            CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as pattern_cases).Add(ValueStack[ValueStack.Depth-1].stn as pattern_case);
        }
        break;
      case 544: // pattern_case -> pattern_optional_var, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 545: // pattern_case -> deconstruction_or_const_pattern, tkColon, unlabelled_stmt
{
            CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 546: // pattern_case -> collection_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 547: // pattern_case -> tuple_pattern, tkWhen, expr_l1, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-5].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
		}
        break;
      case 548: // pattern_case -> tuple_pattern, tkColon, unlabelled_stmt
{
			CurrentSemanticValue.stn = new pattern_case(ValueStack[ValueStack.Depth-3].stn as pattern_node, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
		}
        break;
      case 549: // case_stmt -> tkCase, expr_l1, tkOf, case_list, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 550: // case_stmt -> tkCase, expr_l1, tkOf, case_list, tkSemiColon, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-6].ex, ValueStack[ValueStack.Depth-4].stn as case_variants, ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 551: // case_stmt -> tkCase, expr_l1, tkOf, else_case, tkEnd
{ 
			CurrentSemanticValue.stn = new case_node(ValueStack[ValueStack.Depth-4].ex, NewCaseItem(new empty_statement(), null), ValueStack[ValueStack.Depth-2].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 552: // case_list -> case_item
{
			if (ValueStack[ValueStack.Depth-1].stn is empty_statement) 
				CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, null);
			else CurrentSemanticValue.stn = NewCaseItem(ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 553: // case_list -> case_list, tkSemiColon, case_item
{ 
			CurrentSemanticValue.stn = AddCaseItem(ValueStack[ValueStack.Depth-3].stn as case_variants, ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
		}
        break;
      case 554: // case_item -> case_label_list, tkColon, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new case_variant(ValueStack[ValueStack.Depth-3].stn as expression_list, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
		}
        break;
      case 555: // case_label_list -> case_label
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 556: // case_label_list -> case_label_list, tkComma, case_label
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 557: // case_label -> const_elem
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 558: // else_case -> /* empty */
{ CurrentSemanticValue.stn = null;}
        break;
      case 559: // else_case -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 560: // repeat_stmt -> tkRepeat, stmt_list, tkUntil, expr
{ 
		    CurrentSemanticValue.stn = new repeat_node(ValueStack[ValueStack.Depth-3].stn as statement_list, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-3].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-4].ti;
			(ValueStack[ValueStack.Depth-3].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-2].ti;
			ValueStack[ValueStack.Depth-3].stn.source_context = LocationStack[LocationStack.Depth-4].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 561: // while_stmt -> tkWhile, expr_l1, optional_tk_do, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewWhileStmt(ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);    
        }
        break;
      case 562: // optional_tk_do -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 563: // optional_tk_do -> /* empty */
{ CurrentSemanticValue.ti = null; }
        break;
      case 564: // lock_stmt -> tkLock, expr_l1, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new lock_stmt(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan); 
        }
        break;
      case 565: // index_or_nothing -> tkIndex, tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 566: // index_or_nothing -> /* empty */
{ CurrentSemanticValue.id = null; }
        break;
      case 567: // optional_type_specification -> tkColon, type_ref
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 568: // optional_type_specification -> /* empty */
{ CurrentSemanticValue.td = null; }
        break;
      case 569: // optional_var -> tkVar
{ CurrentSemanticValue.ob = true; }
        break;
      case 570: // optional_var -> /* empty */
{ CurrentSemanticValue.ob = false; }
        break;
      case 571: // for_cycle_type -> tkTo
{ CurrentSemanticValue.ob = for_cycle_type.to; }
        break;
      case 572: // for_cycle_type -> tkDownto
{ CurrentSemanticValue.ob = for_cycle_type.downto; }
        break;
      case 573: // foreach_stmt -> tkForeach, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
            if (ValueStack[ValueStack.Depth-6].td == null)
                parserTools.AddWarningFromResource("USING_UNLOCAL_FOREACH_VARIABLE", ValueStack[ValueStack.Depth-7].id.source_context);
        }
        break;
      case 574: // foreach_stmt -> tkForeach, tkVar, identifier, optional_type_specification, tkIn, 
                //                 expr_l1, index_or_nothing, tkDo, unlabelled_stmt
{ 
        	if (ValueStack[ValueStack.Depth-6].td == null)
				CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, new no_type_foreach(), ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
			else CurrentSemanticValue.stn = new foreach_stmt(ValueStack[ValueStack.Depth-7].id, ValueStack[ValueStack.Depth-6].td, ValueStack[ValueStack.Depth-4].ex, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].id, CurrentLocationSpan);
        }
        break;
      case 575: // foreach_stmt -> tkForeach, tkVar, tkRoundOpen, ident_list, tkRoundClose, tkIn, 
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
      case 576: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, optional_tk_do, 
                //             unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-9].ob, ValueStack[ValueStack.Depth-8].id, ValueStack[ValueStack.Depth-7].td, ValueStack[ValueStack.Depth-5].ex, (for_cycle_type)ValueStack[ValueStack.Depth-4].ob, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-2].ti, ValueStack[ValueStack.Depth-1].stn as statement, null, CurrentLocationSpan);
        }
        break;
      case 577: // for_stmt -> tkFor, optional_var, identifier, optional_type_specification, 
                //             tkAssign, expr_l1, for_cycle_type, expr_l1, tkStep, expr_l1, tkDo, 
                //             unlabelled_stmt
{ 
			CurrentSemanticValue.stn = NewForStmt((bool)ValueStack[ValueStack.Depth-11].ob, ValueStack[ValueStack.Depth-10].id, ValueStack[ValueStack.Depth-9].td, ValueStack[ValueStack.Depth-7].ex, (for_cycle_type)ValueStack[ValueStack.Depth-6].ob, ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-4].ti, ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].ex, CurrentLocationSpan);
        }
        break;
      case 578: // with_stmt -> tkWith, expr_list, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new with_statement(ValueStack[ValueStack.Depth-1].stn as statement, ValueStack[ValueStack.Depth-3].stn as expression_list, CurrentLocationSpan); 
		}
        break;
      case 579: // inherited_message -> tkInherited
{ 
			CurrentSemanticValue.stn = new inherited_message();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 580: // try_stmt -> tkTry, stmt_list, try_handler
{ 
			CurrentSemanticValue.stn = new try_stmt(ValueStack[ValueStack.Depth-2].stn as statement_list, ValueStack[ValueStack.Depth-1].stn as try_handler, CurrentLocationSpan); 
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			ValueStack[ValueStack.Depth-2].stn.source_context = LocationStack[LocationStack.Depth-3].Merge(LocationStack[LocationStack.Depth-2]);
        }
        break;
      case 581: // try_handler -> tkFinally, stmt_list, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_finally(ValueStack[ValueStack.Depth-2].stn as statement_list, CurrentLocationSpan);
			(ValueStack[ValueStack.Depth-2].stn as statement_list).left_logical_bracket = ValueStack[ValueStack.Depth-3].ti;
			(ValueStack[ValueStack.Depth-2].stn as statement_list).right_logical_bracket = ValueStack[ValueStack.Depth-1].ti;
		}
        break;
      case 582: // try_handler -> tkExcept, exception_block, tkEnd
{ 
			CurrentSemanticValue.stn = new try_handler_except((exception_block)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan);  
			if ((ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list != null)
			{
				(ValueStack[ValueStack.Depth-2].stn as exception_block).stmt_list.source_context = CurrentLocationSpan;
				(ValueStack[ValueStack.Depth-2].stn as exception_block).source_context = CurrentLocationSpan;
			}
		}
        break;
      case 583: // exception_block -> exception_handler_list, exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-2].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 584: // exception_block -> exception_handler_list, tkSemiColon, 
                //                    exception_block_else_branch
{ 
			CurrentSemanticValue.stn = new exception_block(null, (exception_handler_list)ValueStack[ValueStack.Depth-3].stn, (statement_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
		}
        break;
      case 585: // exception_block -> stmt_list
{ 
			CurrentSemanticValue.stn = new exception_block(ValueStack[ValueStack.Depth-1].stn as statement_list, null, null, LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 586: // exception_handler_list -> exception_handler
{ 
			CurrentSemanticValue.stn = new exception_handler_list(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 587: // exception_handler_list -> exception_handler_list, tkSemiColon, 
                //                           exception_handler
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as exception_handler_list).Add(ValueStack[ValueStack.Depth-1].stn as exception_handler, CurrentLocationSpan); 
		}
        break;
      case 588: // exception_block_else_branch -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 589: // exception_block_else_branch -> tkElse, stmt_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 590: // exception_handler -> tkOn, exception_identifier, tkDo, unlabelled_stmt
{ 
			CurrentSemanticValue.stn = new exception_handler((ValueStack[ValueStack.Depth-3].stn as exception_ident).variable, (ValueStack[ValueStack.Depth-3].stn as exception_ident).type_name, ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 591: // exception_identifier -> exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(null, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 592: // exception_identifier -> exception_variable, tkColon, 
                //                         exception_class_type_identifier
{ 
			CurrentSemanticValue.stn = new exception_ident(ValueStack[ValueStack.Depth-3].id, (named_type_reference)ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan); 
		}
        break;
      case 593: // exception_class_type_identifier -> simple_type_identifier
{ CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 594: // exception_variable -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 595: // raise_stmt -> tkRaise
{ 
			CurrentSemanticValue.stn = new raise_stmt(); 
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 596: // raise_stmt -> tkRaise, expr
{ 
			CurrentSemanticValue.stn = new raise_stmt(ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan);  
		}
        break;
      case 597: // expr_list -> expr_with_func_decl_lambda
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 598: // expr_list -> expr_list, tkComma, expr_with_func_decl_lambda
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 599: // expr_list_func_param -> expr_with_func_decl_lambda_ass
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 600: // expr_list_func_param -> expr_list_func_param, tkComma, 
                //                         expr_with_func_decl_lambda_ass
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 601: // expr_as_stmt -> allowable_expr_as_stmt
{ 
			CurrentSemanticValue.stn = new expression_as_statement(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 602: // allowable_expr_as_stmt -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 603: // expr_with_func_decl_lambda -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 604: // expr_with_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 605: // expr_with_func_decl_lambda -> tkInherited
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 606: // expr_with_func_decl_lambda_ass -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 607: // expr_with_func_decl_lambda_ass -> identifier, tkAssign, expr_l1
{ CurrentSemanticValue.ex = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); }
        break;
      case 608: // expr_with_func_decl_lambda_ass -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 609: // expr_with_func_decl_lambda_ass -> tkInherited
{ CurrentSemanticValue.ex = new inherited_ident("", CurrentLocationSpan); }
        break;
      case 610: // expr -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 611: // expr -> format_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 612: // expr -> expr, tkTo, expr_l1
{ CurrentSemanticValue.ex = new to_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 613: // expr_l1 -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 614: // expr_l1 -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 615: // expr_l1 -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 616: // expr_l1_for_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 617: // expr_l1_for_question_expr -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 618: // expr_l1_for_new_question_expr -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 619: // expr_l1_for_new_question_expr -> new_question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 620: // expr_l1_func_decl_lambda -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 621: // expr_l1_func_decl_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 622: // expr_l1_for_lambda -> expr_dq
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 623: // expr_l1_for_lambda -> question_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 624: // expr_l1_for_lambda -> func_decl_lambda
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 625: // expr_dq -> relop_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 626: // expr_dq -> expr_dq, tkDoubleQuestion, relop_expr
{ CurrentSemanticValue.ex = new double_question_node(ValueStack[ValueStack.Depth-3].ex as expression, ValueStack[ValueStack.Depth-1].ex as expression, CurrentLocationSpan);}
        break;
      case 627: // sizeof_expr -> tkSizeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new sizeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, null, CurrentLocationSpan);  
		}
        break;
      case 628: // typeof_expr -> tkTypeOf, tkRoundOpen, simple_or_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 629: // typeof_expr -> tkTypeOf, tkRoundOpen, empty_template_type_reference, 
                //                tkRoundClose
{ 
			CurrentSemanticValue.ex = new typeof_operator((named_type_reference)ValueStack[ValueStack.Depth-2].td, CurrentLocationSpan);  
		}
        break;
      case 630: // question_expr -> expr_l1_for_question_expr, tkQuestion, 
                //                  expr_l1_for_question_expr, tkColon, 
                //                  expr_l1_for_question_expr
{ 
            if (ValueStack[ValueStack.Depth-3].ex is nil_const && ValueStack[ValueStack.Depth-1].ex is nil_const)
            	parserTools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",LocationStack[LocationStack.Depth-3]);
			CurrentSemanticValue.ex = new question_colon_expression(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);  
		}
        break;
      case 631: // new_question_expr -> tkIf, expr_l1_for_new_question_expr, tkThen, 
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
      case 632: // empty_template_type_reference -> simple_type_identifier, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 633: // empty_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                  template_type_empty_params
{
            CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan);
        }
        break;
      case 634: // simple_or_template_type_reference -> simple_type_identifier
{ 
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 635: // simple_or_template_type_reference -> simple_type_identifier, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-2].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 636: // simple_or_template_type_reference -> simple_type_identifier, tkAmpersend, 
                //                                      template_type_params
{ 
			CurrentSemanticValue.td = new template_type_reference((named_type_reference)ValueStack[ValueStack.Depth-3].td, (template_param_list)ValueStack[ValueStack.Depth-1].stn, CurrentLocationSpan); 
        }
        break;
      case 637: // simple_or_template_or_question_type_reference -> 
                //                                                  simple_or_template_type_reference
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 638: // simple_or_template_or_question_type_reference -> simple_type_question
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 639: // optional_array_initializer -> tkRoundOpen, typed_const_list, tkRoundClose
{ 
			CurrentSemanticValue.stn = new array_const((expression_list)ValueStack[ValueStack.Depth-2].stn, CurrentLocationSpan); 
		}
        break;
      case 641: // new_expr -> tkNew, simple_or_template_type_reference, 
                //             optional_expr_list_func_param_with_bracket
{
			CurrentSemanticValue.ex = new new_expr(ValueStack[ValueStack.Depth-2].td, ValueStack[ValueStack.Depth-1].stn as expression_list, false, null, CurrentLocationSpan);
        }
        break;
      case 642: // new_expr -> tkNew, simple_or_template_type_reference, tkSquareOpen, 
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
      case 643: // new_expr -> tkNew, tkClass, tkRoundOpen, list_fields_in_unnamed_object, 
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
      case 644: // field_in_unnamed_object -> identifier, tkAssign, expr_l1
{
		    if (ValueStack[ValueStack.Depth-1].ex is nil_const)
				parserTools.AddErrorFromResource("NIL_IN_UNNAMED_OBJECT",CurrentLocationSpan);		    
			CurrentSemanticValue.ob = new name_assign_expr(ValueStack[ValueStack.Depth-3].id,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		}
        break;
      case 645: // field_in_unnamed_object -> expr_l1
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
      case 646: // list_fields_in_unnamed_object -> field_in_unnamed_object
{
			var l = new name_assign_expr_list();
			CurrentSemanticValue.ob = l.Add(ValueStack[ValueStack.Depth-1].ob as name_assign_expr);
		}
        break;
      case 647: // list_fields_in_unnamed_object -> list_fields_in_unnamed_object, tkComma, 
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
      case 648: // optional_expr_list_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 649: // optional_expr_list_with_bracket -> tkRoundOpen, optional_expr_list, 
                //                                    tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 650: // optional_expr_list_func_param_with_bracket -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 651: // optional_expr_list_func_param_with_bracket -> tkRoundOpen, 
                //                                               optional_expr_list_func_param, 
                //                                               tkRoundClose
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; }
        break;
      case 652: // relop_expr -> simple_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 653: // relop_expr -> relop_expr, relop, simple_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 654: // relop_expr -> relop_expr, relop, new_question_expr
{ 
        	if (ValueStack[ValueStack.Depth-2].op.type == Operators.NotIn)
        		CurrentSemanticValue.ex = new un_expr(new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, Operators.In, CurrentLocationSpan),Operators.LogicalNOT,CurrentLocationSpan);
        	else	
				CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 655: // relop_expr -> is_type_expr, tkRoundOpen, pattern_out_param_list, tkRoundClose
{
            var isTypeCheck = ValueStack[ValueStack.Depth-4].ex as typecast_node;
            var deconstructorPattern = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, isTypeCheck.type_def, null, CurrentLocationSpan); 
            CurrentSemanticValue.ex = new is_pattern_expr(isTypeCheck.expr, deconstructorPattern, CurrentLocationSpan);
        }
        break;
      case 656: // pattern -> simple_or_template_type_reference, tkRoundOpen, 
                //            pattern_out_param_list, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 657: // pattern_optional_var -> simple_or_template_type_reference, tkRoundOpen, 
                //                         pattern_out_param_list_optional_var, tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 658: // deconstruction_or_const_pattern -> simple_or_template_type_reference, 
                //                                    tkRoundOpen, 
                //                                    pattern_out_param_list_optional_var, 
                //                                    tkRoundClose
{ 
            CurrentSemanticValue.stn = new deconstructor_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, ValueStack[ValueStack.Depth-4].td, null, CurrentLocationSpan); 
        }
        break;
      case 659: // deconstruction_or_const_pattern -> const_pattern_expr_list
{
		    CurrentSemanticValue.stn = new const_pattern(ValueStack[ValueStack.Depth-1].ob as List<syntax_tree_node>, CurrentLocationSpan); 
		}
        break;
      case 660: // const_pattern_expr_list -> const_pattern_expression
{ 
			CurrentSemanticValue.ob = new List<syntax_tree_node>(); 
			(CurrentSemanticValue.ob as List<syntax_tree_node>).Add(ValueStack[ValueStack.Depth-1].stn);
		}
        break;
      case 661: // const_pattern_expr_list -> const_pattern_expr_list, tkComma, 
                //                            const_pattern_expression
{ 
			var list = ValueStack[ValueStack.Depth-3].ob as List<syntax_tree_node>;
            list.Add(ValueStack[ValueStack.Depth-1].stn);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 662: // const_pattern_expression -> literal_or_number
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 663: // const_pattern_expression -> simple_or_template_type_reference
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].td; }
        break;
      case 664: // const_pattern_expression -> tkNil
{ 
			CurrentSemanticValue.stn = new nil_const();  
			CurrentSemanticValue.stn.source_context = CurrentLocationSpan;
		}
        break;
      case 665: // const_pattern_expression -> sizeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 666: // const_pattern_expression -> typeof_expr
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 667: // collection_pattern -> tkSquareOpen, collection_pattern_expr_list, tkSquareClose
{
			CurrentSemanticValue.stn = new collection_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 668: // collection_pattern_expr_list -> collection_pattern_list_item
{
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 669: // collection_pattern_expr_list -> collection_pattern_expr_list, tkComma, 
                //                                 collection_pattern_list_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 670: // collection_pattern_list_item -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 671: // collection_pattern_list_item -> collection_pattern_var_item
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 672: // collection_pattern_list_item -> tkUnderscore
{
			CurrentSemanticValue.stn = new collection_pattern_wild_card(CurrentLocationSpan);
		}
        break;
      case 673: // collection_pattern_list_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 674: // collection_pattern_list_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 675: // collection_pattern_list_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 676: // collection_pattern_list_item -> tkDotDot
{
			CurrentSemanticValue.stn = new collection_pattern_gap_parameter(CurrentLocationSpan);
		}
        break;
      case 677: // collection_pattern_var_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new collection_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 678: // tuple_pattern -> tkRoundOpen, tuple_pattern_item_list, tkRoundClose
{
			if ((ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>).Count>6) 
				parserTools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",CurrentLocationSpan);
			CurrentSemanticValue.stn = new tuple_pattern(ValueStack[ValueStack.Depth-2].ob as List<pattern_parameter>, CurrentLocationSpan);
		}
        break;
      case 679: // tuple_pattern_item -> tkUnderscore
{ 
			CurrentSemanticValue.stn = new tuple_pattern_wild_card(CurrentLocationSpan); 
		}
        break;
      case 680: // tuple_pattern_item -> literal_or_number
{ 
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 681: // tuple_pattern_item -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 682: // tuple_pattern_item -> tkVar, identifier
{
            CurrentSemanticValue.stn = new tuple_pattern_var_parameter(ValueStack[ValueStack.Depth-1].id, null, CurrentLocationSpan);
        }
        break;
      case 683: // tuple_pattern_item -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 684: // tuple_pattern_item -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 685: // tuple_pattern_item -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 686: // tuple_pattern_item_list -> tuple_pattern_item
{ 
			CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
		}
        break;
      case 687: // tuple_pattern_item_list -> tuple_pattern_item_list, tkComma, tuple_pattern_item
{
			var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
		}
        break;
      case 688: // pattern_out_param_list_optional_var -> pattern_out_param_optional_var
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 689: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkSemiColon, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 690: // pattern_out_param_list_optional_var -> pattern_out_param_list_optional_var, 
                //                                        tkComma, 
                //                                        pattern_out_param_optional_var
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 691: // pattern_out_param_list -> pattern_out_param
{
            CurrentSemanticValue.ob = new List<pattern_parameter>();
            (CurrentSemanticValue.ob as List<pattern_parameter>).Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
        }
        break;
      case 692: // pattern_out_param_list -> pattern_out_param_list, tkSemiColon, 
                //                           pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 693: // pattern_out_param_list -> pattern_out_param_list, tkComma, pattern_out_param
{
            var list = ValueStack[ValueStack.Depth-3].ob as List<pattern_parameter>;
            list.Add(ValueStack[ValueStack.Depth-1].stn as pattern_parameter);
            CurrentSemanticValue.ob = list;
        }
        break;
      case 694: // pattern_out_param -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 695: // pattern_out_param -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 696: // pattern_out_param -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 697: // pattern_out_param -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 698: // pattern_out_param -> pattern
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 699: // pattern_out_param -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 700: // pattern_out_param -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 701: // pattern_out_param_optional_var -> tkUnderscore
{
			CurrentSemanticValue.stn = new wild_card_deconstructor_parameter(CurrentLocationSpan);
		}
        break;
      case 702: // pattern_out_param_optional_var -> literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 703: // pattern_out_param_optional_var -> sign, literal_or_number
{
			CurrentSemanticValue.stn = new const_pattern_parameter(new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan), CurrentLocationSpan);
		}
        break;
      case 704: // pattern_out_param_optional_var -> identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, false, CurrentLocationSpan);
        }
        break;
      case 705: // pattern_out_param_optional_var -> identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, false, CurrentLocationSpan);
        }
        break;
      case 706: // pattern_out_param_optional_var -> tkVar, identifier, tkColon, type_ref
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-3].id, ValueStack[ValueStack.Depth-1].td, true, CurrentLocationSpan);
        }
        break;
      case 707: // pattern_out_param_optional_var -> tkVar, identifier
{
            CurrentSemanticValue.stn = new var_deconstructor_parameter(ValueStack[ValueStack.Depth-1].id, null, true, CurrentLocationSpan);
        }
        break;
      case 708: // pattern_out_param_optional_var -> pattern_optional_var
{
            CurrentSemanticValue.stn = new recursive_deconstructor_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
        }
        break;
      case 709: // pattern_out_param_optional_var -> collection_pattern
{
			CurrentSemanticValue.stn = new recursive_collection_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 710: // pattern_out_param_optional_var -> tuple_pattern
{
			CurrentSemanticValue.stn = new recursive_tuple_parameter(ValueStack[ValueStack.Depth-1].stn as pattern_node, CurrentLocationSpan);
		}
        break;
      case 711: // simple_expr_or_nothing -> simple_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 712: // simple_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 713: // const_expr_or_nothing -> const_expr
{
		CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
	}
        break;
      case 714: // const_expr_or_nothing -> /* empty */
{
		CurrentSemanticValue.ex = null;
	}
        break;
      case 715: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing
{
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 716: // format_expr -> tkColon, simple_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 717: // format_expr -> simple_expr, tkColon, simple_expr_or_nothing, tkColon, 
                //                simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 718: // format_expr -> tkColon, simple_expr_or_nothing, tkColon, simple_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 719: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 720: // format_const_expr -> tkColon, const_expr_or_nothing
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-1].ex, null, CurrentLocationSpan); 
		}
        break;
      case 721: // format_const_expr -> const_expr, tkColon, const_expr_or_nothing, tkColon, 
                //                      const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(ValueStack[ValueStack.Depth-5].ex, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 722: // format_const_expr -> tkColon, const_expr_or_nothing, tkColon, const_expr
{ 
			CurrentSemanticValue.ex = new format_expr(null, ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 723: // relop -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 724: // relop -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 725: // relop -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 726: // relop -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 727: // relop -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 728: // relop -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 729: // relop -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 730: // relop -> tkNot, tkIn
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
      case 731: // simple_expr -> term1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 732: // simple_expr -> simple_expr, tkDotDot, term1
{ 
		if (parserTools.buildTreeForFormatter)
			CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan);
		else 
			CurrentSemanticValue.ex = new diapason_expr_new(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,CurrentLocationSpan); 
	}
        break;
      case 733: // term1 -> term
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 734: // term1 -> term1, addop, term
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 735: // term1 -> term1, addop, new_question_expr
{ 
			CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 736: // addop -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 737: // addop -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 738: // addop -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 739: // addop -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 740: // addop -> tkCSharpStyleOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 741: // typecast_op -> tkAs
{ 
			CurrentSemanticValue.ob = op_typecast.as_op; 
		}
        break;
      case 742: // typecast_op -> tkIs
{ 
			CurrentSemanticValue.ob = op_typecast.is_op; 
		}
        break;
      case 743: // as_is_expr -> is_type_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 744: // as_is_expr -> as_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 745: // as_expr -> term, tkAs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 746: // as_expr -> term, tkAs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.as_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 747: // is_type_expr -> term, tkIs, simple_or_template_type_reference
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
        }
        break;
      case 748: // is_type_expr -> term, tkIs, array_type
{
            CurrentSemanticValue.ex = NewAsIsExpr(ValueStack[ValueStack.Depth-3].ex, op_typecast.is_op, ValueStack[ValueStack.Depth-1].td, CurrentLocationSpan);
	    }
        break;
      case 749: // power_expr -> factor_without_unary_op, tkStarStar, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 750: // power_expr -> factor_without_unary_op, tkStarStar, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 751: // power_expr -> sign, power_expr
{ CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); }
        break;
      case 752: // term -> factor
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 753: // term -> new_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 754: // term -> power_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 755: // term -> term, mulop, factor
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 756: // term -> term, mulop, power_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 757: // term -> term, mulop, new_question_expr
{ CurrentSemanticValue.ex = new bin_expr(ValueStack[ValueStack.Depth-3].ex,ValueStack[ValueStack.Depth-1].ex,(ValueStack[ValueStack.Depth-2].op).type, CurrentLocationSpan); }
        break;
      case 758: // term -> as_is_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 759: // mulop -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 760: // mulop -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 761: // mulop -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 762: // mulop -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 763: // mulop -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 764: // mulop -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 765: // mulop -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 766: // default_expr -> tkDefault, tkRoundOpen, 
                //                 simple_or_template_or_question_type_reference, tkRoundClose
{ 
			CurrentSemanticValue.ex = new default_operator(ValueStack[ValueStack.Depth-2].td as named_type_reference, CurrentLocationSpan);  
		}
        break;
      case 767: // tuple -> tkRoundOpen, expr_l1_or_unpacked, tkComma, expr_l1_or_unpacked_list, 
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
      case 768: // factor_without_unary_op -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 769: // factor_without_unary_op -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 770: // factor -> tkNil
{ 
			CurrentSemanticValue.ex = new nil_const();  
			CurrentSemanticValue.ex.source_context = CurrentLocationSpan;
		}
        break;
      case 771: // factor -> literal_or_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 772: // factor -> default_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 773: // factor -> tkNot, factor
{ 
			CurrentSemanticValue.ex = new un_expr(ValueStack[ValueStack.Depth-1].ex, ValueStack[ValueStack.Depth-2].op.type, CurrentLocationSpan); 
		}
        break;
      case 774: // factor -> sign, factor
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
      case 775: // factor -> tkDeref, factor
{
            CurrentSemanticValue.ex = new index(ValueStack[ValueStack.Depth-1].ex, true, CurrentLocationSpan);
        }
        break;
      case 776: // factor -> var_reference
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 777: // factor -> tuple
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 778: // literal_or_number -> literal
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 779: // literal_or_number -> unsigned_number
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 780: // var_question_point -> variable, tkQuestionPoint, variable
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 781: // var_question_point -> variable, tkQuestionPoint, var_question_point
{
		CurrentSemanticValue.ex = new dot_question_node(ValueStack[ValueStack.Depth-3].ex as addressed_value,ValueStack[ValueStack.Depth-1].ex as addressed_value,CurrentLocationSpan);
	}
        break;
      case 782: // var_reference -> var_address, variable
{
			CurrentSemanticValue.ex = NewVarReference(ValueStack[ValueStack.Depth-2].stn as get_address, ValueStack[ValueStack.Depth-1].ex as addressed_value, CurrentLocationSpan);
		}
        break;
      case 783: // var_reference -> variable
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 784: // var_reference -> var_question_point
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 785: // var_reference -> tkRoundOpen, tkVar, identifier, tkAssign, expr_dq, 
                //                  tkRoundClose
{ CurrentSemanticValue.ex = new let_var_expr(ValueStack[ValueStack.Depth-4].id,ValueStack[ValueStack.Depth-2].ex,CurrentLocationSpan); }
        break;
      case 786: // var_address -> tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(CurrentLocationSpan);
		}
        break;
      case 787: // var_address -> var_address, tkAddressOf
{ 
			CurrentSemanticValue.stn = NewVarAddress(ValueStack[ValueStack.Depth-2].stn as get_address, CurrentLocationSpan);
		}
        break;
      case 788: // attribute_variable -> simple_type_identifier, optional_expr_list_with_bracket
{ 
			CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
		}
        break;
      case 789: // attribute_variable -> template_type, optional_expr_list_with_bracket
{
            CurrentSemanticValue.stn = new attribute(null, ValueStack[ValueStack.Depth-2].td as named_type_reference, ValueStack[ValueStack.Depth-1].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 790: // dotted_identifier -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 791: // dotted_identifier -> dotted_identifier, tkPoint, identifier_or_keyword
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
		}
        break;
      case 792: // variable_as_type -> dotted_identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;}
        break;
      case 793: // variable_as_type -> dotted_identifier, template_type_params
{ CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-2].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);   }
        break;
      case 794: // var_with_init_for_expr_with_let -> tkVar, identifier, tkAssign, expr, 
                //                                    tkSemiColon
{
			CurrentSemanticValue.stn = new assign(ValueStack[ValueStack.Depth-4].id as addressed_value, ValueStack[ValueStack.Depth-2].ex, Operators.Assignment, CurrentLocationSpan);
		}
        break;
      case 795: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 796: // var_with_init_for_expr_with_let_list -> var_with_init_for_expr_with_let_list, 
                //                                         var_with_init_for_expr_with_let
{
			ValueStack[ValueStack.Depth-2].stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn;
		}
        break;
      case 797: // proc_func_call -> variable, tkRoundOpen, optional_expr_list_func_param, 
                //                   tkRoundClose
{
			if (ValueStack[ValueStack.Depth-4].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-4], "^");
			CurrentSemanticValue.ex = new method_call(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list, CurrentLocationSpan);
        }
        break;
      case 798: // variable -> identifier
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 799: // variable -> operator_name_ident
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 800: // variable -> tkInherited, identifier
{ 
			CurrentSemanticValue.ex = new inherited_ident(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);
		}
        break;
      case 801: // variable -> tkRoundOpen, expr, tkRoundClose
{
		    if (!parserTools.buildTreeForFormatter) 
            {
                ValueStack[ValueStack.Depth-2].ex.source_context = CurrentLocationSpan;
                CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-2].ex;
            } 
			else CurrentSemanticValue.ex = new bracket_expr(ValueStack[ValueStack.Depth-2].ex, CurrentLocationSpan);
        }
        break;
      case 802: // variable -> tkRoundOpen, var_with_init_for_expr_with_let_list, expr, 
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
      case 803: // variable -> sizeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 804: // variable -> typeof_expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 805: // variable -> literal_or_number, tkPoint, identifier_or_keyword
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");		
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan); 
		}
        break;
      case 806: // variable -> variable, tkSquareOpen, expr_list, tkSquareClose
{
        	CurrentSemanticValue.ex = NewIndexerOrSlice(ValueStack[ValueStack.Depth-4].ex as addressed_value,ValueStack[ValueStack.Depth-2].stn as expression_list,CurrentLocationSpan);
        }
        break;
      case 807: // variable -> literal_or_number, tkSquareOpen, expr_list, tkSquareClose
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
      case 808: // variable -> variable, tkQuestionSquareOpen, format_expr, tkSquareClose
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
      case 809: // variable -> literal_or_number, tkQuestionSquareOpen, format_expr, tkSquareClose
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
      case 810: // variable -> tkVertParen, elem_list, tkVertParen
{ 
			CurrentSemanticValue.ex = new array_const_new(ValueStack[ValueStack.Depth-2].stn as expression_list, '|', CurrentLocationSpan);  
		}
        break;
      case 811: // variable -> pascal_set_const
{ 
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex;
		}
        break;
      case 812: // variable -> proc_func_call
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 813: // variable -> variable, tkPoint, identifier_keyword_operatorname
{
			if (ValueStack[ValueStack.Depth-3].ex is index)
				parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}", LocationStack[LocationStack.Depth-3], "^");
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 814: // variable -> tuple, tkPoint, identifier_keyword_operatorname
{
			CurrentSemanticValue.ex = new dot_node(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].id as addressed_value, CurrentLocationSpan);
        }
        break;
      case 815: // variable -> variable, tkDeref
{
			CurrentSemanticValue.ex = new roof_dereference(ValueStack[ValueStack.Depth-2].ex as addressed_value,CurrentLocationSpan);
        }
        break;
      case 816: // variable -> variable, tkAmpersend, template_type_params
{
			CurrentSemanticValue.ex = new ident_with_templateparams(ValueStack[ValueStack.Depth-3].ex as addressed_value, ValueStack[ValueStack.Depth-1].stn as template_param_list, CurrentLocationSpan);
        }
        break;
      case 817: // optional_expr_list -> expr_list
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 818: // optional_expr_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 819: // optional_expr_list_func_param -> expr_list_func_param
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 820: // optional_expr_list_func_param -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 821: // elem_list -> elem_list1
{ CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; }
        break;
      case 822: // elem_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 823: // elem_list1 -> elem
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 824: // elem_list1 -> elem_list1, tkComma, elem
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan);
		}
        break;
      case 825: // elem -> expr
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 826: // elem -> expr, tkDotDot, expr
{ CurrentSemanticValue.ex = new diapason_expr(ValueStack[ValueStack.Depth-3].ex, ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); }
        break;
      case 827: // one_literal -> tkStringLiteral
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 828: // one_literal -> tkAsciiChar
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].stn as literal; }
        break;
      case 829: // literal -> literal_list
{ 
			CurrentSemanticValue.ex = NewLiteral(ValueStack[ValueStack.Depth-1].stn as literal_const_line);
        }
        break;
      case 830: // literal -> tkFormatStringLiteral
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
      case 831: // literal -> tkMultilineStringLiteral
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
      case 832: // literal_list -> one_literal
{ 
			CurrentSemanticValue.stn = new literal_const_line(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 833: // literal_list -> literal_list, one_literal
{ 
        	var line = ValueStack[ValueStack.Depth-2].stn as literal_const_line;
            if (line.literals.Last() is string_const && ValueStack[ValueStack.Depth-1].ex is string_const)
            	parserTools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",LocationStack[LocationStack.Depth-1]);
			CurrentSemanticValue.stn = line.Add(ValueStack[ValueStack.Depth-1].ex as literal, CurrentLocationSpan);
        }
        break;
      case 834: // operator_name_ident -> tkOperator, overload_operator
{ 
			CurrentSemanticValue.ex = new operator_name_ident((ValueStack[ValueStack.Depth-1].op as op_type_node).text, (ValueStack[ValueStack.Depth-1].op as op_type_node).type, CurrentLocationSpan);
		}
        break;
      case 835: // optional_method_modificators -> tkSemiColon
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 836: // optional_method_modificators -> tkSemiColon, meth_modificators, tkSemiColon
{ 
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-2].stn; 
		}
        break;
      case 837: // optional_method_modificators1 -> /* empty */
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(new List<procedure_attribute>(),CurrentLocationSpan); 
		}
        break;
      case 838: // optional_method_modificators1 -> tkSemiColon, meth_modificators
{ 
			//parserTools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
		}
        break;
      case 839: // meth_modificators -> meth_modificator
{ 
			CurrentSemanticValue.stn = new procedure_attributes_list(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan); 
		}
        break;
      case 840: // meth_modificators -> meth_modificators, tkSemiColon, meth_modificator
{ 
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as procedure_attributes_list).Add(ValueStack[ValueStack.Depth-1].id as procedure_attribute, CurrentLocationSpan);  
		}
        break;
      case 841: // identifier -> tkIdentifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 842: // identifier -> tkExport
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 843: // identifier -> property_specifier_directives
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 844: // identifier -> non_reserved
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 845: // identifier -> tkStep
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 846: // identifier -> tkIndex
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 847: // identifier_or_keyword -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 848: // identifier_or_keyword -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 849: // identifier_or_keyword -> reserved_keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 850: // identifier_keyword_operatorname -> identifier
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 851: // identifier_keyword_operatorname -> keyword
{ CurrentSemanticValue.id = new ident(ValueStack[ValueStack.Depth-1].ti.text, CurrentLocationSpan); }
        break;
      case 852: // identifier_keyword_operatorname -> operator_name_ident
{ CurrentSemanticValue.id = (ident)ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 853: // meth_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 854: // meth_modificator -> tkOverload
{ 
            CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id;
            parserTools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", ValueStack[ValueStack.Depth-1].id.source_context);
        }
        break;
      case 855: // meth_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 856: // meth_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 857: // meth_modificator -> tkExtensionMethod
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 858: // meth_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 859: // property_modificator -> tkVirtual
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 860: // property_modificator -> tkOverride
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 861: // property_modificator -> tkAbstract
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 862: // property_modificator -> tkReintroduce
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 863: // property_specifier_directives -> tkRead
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 864: // property_specifier_directives -> tkWrite
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 865: // non_reserved -> tkName
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 866: // non_reserved -> tkNew
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 867: // visibility_specifier -> tkInternal
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 868: // visibility_specifier -> tkPublic
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 869: // visibility_specifier -> tkProtected
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 870: // visibility_specifier -> tkPrivate
{ CurrentSemanticValue.id = ValueStack[ValueStack.Depth-1].id; }
        break;
      case 871: // keyword -> visibility_specifier
{ 
			CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  
		}
        break;
      case 872: // keyword -> tkSealed
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 873: // keyword -> tkTemplate
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 874: // keyword -> tkOr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 875: // keyword -> tkTypeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 876: // keyword -> tkSizeOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 877: // keyword -> tkDefault
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 878: // keyword -> tkWhere
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 879: // keyword -> tkXor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 880: // keyword -> tkAnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 881: // keyword -> tkDiv
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 882: // keyword -> tkMod
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 883: // keyword -> tkShl
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 884: // keyword -> tkShr
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 885: // keyword -> tkNot
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 886: // keyword -> tkAs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 887: // keyword -> tkIn
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 888: // keyword -> tkIs
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 889: // keyword -> tkArray
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 890: // keyword -> tkSequence
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 891: // keyword -> tkBegin
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 892: // keyword -> tkCase
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 893: // keyword -> tkClass
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 894: // keyword -> tkConst
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 895: // keyword -> tkConstructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 896: // keyword -> tkDestructor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 897: // keyword -> tkDownto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 898: // keyword -> tkDo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 899: // keyword -> tkElse
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 900: // keyword -> tkEnd
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 901: // keyword -> tkExcept
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 902: // keyword -> tkFile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 903: // keyword -> tkAuto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 904: // keyword -> tkFinalization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 905: // keyword -> tkFinally
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 906: // keyword -> tkFor
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 907: // keyword -> tkForeach
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 908: // keyword -> tkFunction
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 909: // keyword -> tkIf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 910: // keyword -> tkImplementation
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 911: // keyword -> tkInherited
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 912: // keyword -> tkInitialization
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 913: // keyword -> tkInterface
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 914: // keyword -> tkProcedure
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 915: // keyword -> tkProperty
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 916: // keyword -> tkRaise
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 917: // keyword -> tkRecord
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 918: // keyword -> tkRepeat
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 919: // keyword -> tkSet
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 920: // keyword -> tkTry
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 921: // keyword -> tkType
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 922: // keyword -> tkStatic
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 923: // keyword -> tkThen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 924: // keyword -> tkTo
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 925: // keyword -> tkUntil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 926: // keyword -> tkUses
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 927: // keyword -> tkVar
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 928: // keyword -> tkWhile
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 929: // keyword -> tkWith
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 930: // keyword -> tkNil
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 931: // keyword -> tkGoto
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 932: // keyword -> tkOf
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 933: // keyword -> tkLabel
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 934: // keyword -> tkProgram
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 935: // keyword -> tkUnit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 936: // keyword -> tkLibrary
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 937: // keyword -> tkNamespace
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 938: // keyword -> tkExternal
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 939: // keyword -> tkParams
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 940: // keyword -> tkEvent
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 941: // keyword -> tkYield
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 942: // keyword -> tkMatch
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 943: // keyword -> tkWhen
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 944: // keyword -> tkPartial
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 945: // keyword -> tkAbstract
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 946: // keyword -> tkLock
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 947: // keyword -> tkImplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 948: // keyword -> tkExplicit
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 949: // keyword -> tkOn
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 950: // keyword -> tkVirtual
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 951: // keyword -> tkOverride
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 952: // keyword -> tkLoop
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 953: // keyword -> tkExtensionMethod
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 954: // keyword -> tkOverload
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 955: // keyword -> tkReintroduce
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 956: // keyword -> tkForward
{ CurrentSemanticValue.ti = new token_info(ValueStack[ValueStack.Depth-1].id.name, CurrentLocationSpan);  }
        break;
      case 957: // reserved_keyword -> tkOperator
{ CurrentSemanticValue.ti = ValueStack[ValueStack.Depth-1].ti; }
        break;
      case 958: // overload_operator -> tkMinus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 959: // overload_operator -> tkPlus
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 960: // overload_operator -> tkSlash
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 961: // overload_operator -> tkStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 962: // overload_operator -> tkEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 963: // overload_operator -> tkGreater
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 964: // overload_operator -> tkGreaterEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 965: // overload_operator -> tkLower
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 966: // overload_operator -> tkLowerEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 967: // overload_operator -> tkNotEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 968: // overload_operator -> tkOr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 969: // overload_operator -> tkXor
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 970: // overload_operator -> tkAnd
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 971: // overload_operator -> tkDiv
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 972: // overload_operator -> tkMod
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 973: // overload_operator -> tkShl
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 974: // overload_operator -> tkShr
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 975: // overload_operator -> tkNot
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 976: // overload_operator -> tkIn
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 977: // overload_operator -> tkImplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 978: // overload_operator -> tkExplicit
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 979: // overload_operator -> assign_operator
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 980: // overload_operator -> tkStarStar
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 981: // assign_operator -> tkAssign
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 982: // assign_operator -> tkPlusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 983: // assign_operator -> tkMinusEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 984: // assign_operator -> tkMultEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 985: // assign_operator -> tkDivEqual
{ CurrentSemanticValue.op = ValueStack[ValueStack.Depth-1].op; }
        break;
      case 986: // lambda_unpacked_params -> tkBackSlashRoundOpen, 
                //                           lambda_list_of_unpacked_params_or_id, tkComma, 
                //                           lambda_unpacked_params_or_id, tkRoundClose
{
			// �?ез�?л�?�?а�? надо п�?исвои�?�? каком�? �?о са�?а�?ном�? пол�? в function_lambda_definition
			(ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-2].ob as ident_or_list);
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-4].ob as unpacked_list_of_ident_or_list;
		}
        break;
      case 987: // lambda_unpacked_params_or_id -> lambda_unpacked_params
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].ex as unpacked_list_of_ident_or_list);
		}
        break;
      case 988: // lambda_unpacked_params_or_id -> identifier
{
			CurrentSemanticValue.ob = new ident_or_list(ValueStack[ValueStack.Depth-1].id as ident);
		}
        break;
      case 989: // lambda_list_of_unpacked_params_or_id -> lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = new unpacked_list_of_ident_or_list();
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LocationStack[LocationStack.Depth-1];
		}
        break;
      case 990: // lambda_list_of_unpacked_params_or_id -> lambda_list_of_unpacked_params_or_id, 
                //                                         tkComma, lambda_unpacked_params_or_id
{
			CurrentSemanticValue.ob = ValueStack[ValueStack.Depth-3].ob;
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).Add(ValueStack[ValueStack.Depth-1].ob as ident_or_list);
			(CurrentSemanticValue.ob as unpacked_list_of_ident_or_list).source_context = LexLocation.MergeAll(LocationStack[LocationStack.Depth-3],LocationStack[LocationStack.Depth-1]);
		}
        break;
      case 991: // expr_l1_or_unpacked -> expr_l1
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 992: // expr_l1_or_unpacked -> lambda_unpacked_params
{ CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; }
        break;
      case 993: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked
{ 
			CurrentSemanticValue.stn = new expression_list(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 994: // expr_l1_or_unpacked_list -> expr_l1_or_unpacked_list, tkComma, 
                //                             expr_l1_or_unpacked
{
			CurrentSemanticValue.stn = (ValueStack[ValueStack.Depth-3].stn as expression_list).Add(ValueStack[ValueStack.Depth-1].ex, CurrentLocationSpan); 
		}
        break;
      case 995: // func_decl_lambda -> identifier, tkArrow, lambda_function_body
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
      case 996: // func_decl_lambda -> tkRoundOpen, tkRoundClose, lambda_type_ref_noproctype, 
                //                     tkArrow, lambda_function_body
{
		    // �?дес�? надо анализи�?ова�?�? по �?ел�? и либо ос�?авля�?�? lambda_inferred_type, либо дела�?�? его null!
		    var sl = ValueStack[ValueStack.Depth-1].stn as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �?о надо в�?води�?�?
				CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, sl, CurrentLocationSpan);
			else CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, sl, CurrentLocationSpan);	
		}
        break;
      case 997: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkRoundClose, 
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
      case 998: // func_decl_lambda -> tkRoundOpen, identifier, tkSemiColon, full_lambda_fp_list, 
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
      case 999: // func_decl_lambda -> tkRoundOpen, identifier, tkColon, fptype, tkSemiColon, 
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
      case 1000: // func_decl_lambda -> tkRoundOpen, expr_l1_or_unpacked, tkComma, 
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
      case 1001: // func_decl_lambda -> lambda_unpacked_params, rem_lambda
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
      case 1002: // func_decl_lambda -> expl_func_decl_lambda
{
			CurrentSemanticValue.ex = ValueStack[ValueStack.Depth-1].ex; 
		}
        break;
      case 1003: // optional_full_lambda_fp_list -> /* empty */
{ CurrentSemanticValue.stn = null; }
        break;
      case 1004: // optional_full_lambda_fp_list -> tkSemiColon, full_lambda_fp_list
{
		CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn; 
	}
        break;
      case 1005: // rem_lambda -> lambda_type_ref_noproctype, tkArrow, lambda_function_body
{ 
		    CurrentSemanticValue.ob = new pair_type_stlist(ValueStack[ValueStack.Depth-3].td,ValueStack[ValueStack.Depth-1].stn as statement_list);
		}
        break;
      case 1006: // expl_func_decl_lambda -> tkFunction, lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1007: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, tkRoundClose, 
                 //                          lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1008: // expl_func_decl_lambda -> tkFunction, tkRoundOpen, full_lambda_fp_list, 
                 //                          tkRoundClose, lambda_type_ref_noproctype, tkArrow, 
                 //                          lambda_function_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-5].stn as formal_parameters, ValueStack[ValueStack.Depth-3].td, ValueStack[ValueStack.Depth-1].stn as statement_list, 1, CurrentLocationSpan);
		}
        break;
      case 1009: // expl_func_decl_lambda -> tkProcedure, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1010: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, tkRoundClose, tkArrow, 
                 //                          lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1011: // expl_func_decl_lambda -> tkProcedure, tkRoundOpen, full_lambda_fp_list, 
                 //                          tkRoundClose, tkArrow, lambda_procedure_body
{
			CurrentSemanticValue.ex = new function_lambda_definition(lambdaHelper.CreateLambdaName(), ValueStack[ValueStack.Depth-4].stn as formal_parameters, null, ValueStack[ValueStack.Depth-1].stn as statement_list, 2, CurrentLocationSpan);
		}
        break;
      case 1012: // full_lambda_fp_list -> lambda_simple_fp_sect
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
      case 1013: // full_lambda_fp_list -> full_lambda_fp_list, tkSemiColon, lambda_simple_fp_sect
{
			CurrentSemanticValue.stn =(ValueStack[ValueStack.Depth-3].stn as formal_parameters).Add(ValueStack[ValueStack.Depth-1].stn as typed_parameters, CurrentLocationSpan);
		}
        break;
      case 1014: // lambda_simple_fp_sect -> ident_list, lambda_type_ref
{
			CurrentSemanticValue.stn = new typed_parameters(ValueStack[ValueStack.Depth-2].stn as ident_list, ValueStack[ValueStack.Depth-1].td, parametr_kind.none, null, CurrentLocationSpan);
		}
        break;
      case 1015: // lambda_type_ref -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
		}
        break;
      case 1016: // lambda_type_ref -> tkColon, fptype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 1017: // lambda_type_ref_noproctype -> /* empty */
{
			CurrentSemanticValue.td = new lambda_inferred_type(new lambda_any_type_node_syntax(), null);
		}
        break;
      case 1018: // lambda_type_ref_noproctype -> tkColon, fptype_noproctype
{
			CurrentSemanticValue.td = ValueStack[ValueStack.Depth-1].td;
		}
        break;
      case 1019: // common_lambda_body -> compound_stmt
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 1020: // common_lambda_body -> if_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1021: // common_lambda_body -> while_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1022: // common_lambda_body -> repeat_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1023: // common_lambda_body -> for_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1024: // common_lambda_body -> foreach_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1025: // common_lambda_body -> loop_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1026: // common_lambda_body -> case_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1027: // common_lambda_body -> try_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1028: // common_lambda_body -> lock_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1029: // common_lambda_body -> raise_stmt
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1030: // common_lambda_body -> yield_stmt
{
			parserTools.AddErrorFromResource("YIELD_STATEMENT_CANNOT_BE_USED_IN_LAMBDA_BODY", CurrentLocationSpan);
		}
        break;
      case 1031: // common_lambda_body -> tkRoundOpen, assignment, tkRoundClose
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-2].stn as statement, LocationStack[LocationStack.Depth-2]);
		}
        break;
      case 1032: // lambda_function_body -> expr_l1_for_lambda
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
      case 1033: // lambda_function_body -> common_lambda_body
{
			CurrentSemanticValue.stn = ValueStack[ValueStack.Depth-1].stn;
		}
        break;
      case 1034: // lambda_procedure_body -> proc_call
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1035: // lambda_procedure_body -> assignment
{
			CurrentSemanticValue.stn = new statement_list(ValueStack[ValueStack.Depth-1].stn as statement, CurrentLocationSpan);
		}
        break;
      case 1036: // lambda_procedure_body -> common_lambda_body
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
