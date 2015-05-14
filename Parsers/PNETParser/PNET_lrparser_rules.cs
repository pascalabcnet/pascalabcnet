
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//������ ���� ��������� �������, �� ��������� �������������!!!
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.PNETParser.Errors;
using PascalABCCompiler.ParserTools;
using GoldParser;

namespace  PascalABCCompiler.PNETParser
{
public partial class GPBParser_PNET : GPBParser
{







///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//SymbolConstants
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

public enum SymbolConstants : int
{
	SYMBOL_EOF                                  =   0, // (EOF)
	SYMBOL_ERROR                                =   1, // (Error)
	SYMBOL_WHITESPACE                           =   2, // (Whitespace)
	SYMBOL_TKABSOLUTE                           =   3, // tkAbsolute
	SYMBOL_TKABSTRACT                           =   4, // tkAbstract
	SYMBOL_TKADDRESSOF                          =   5, // tkAddressOf
	SYMBOL_TKAMPERSEND                          =   6, // tkAmpersend
	SYMBOL_TKAND                                =   7, // tkAnd
	SYMBOL_TKARRAY                              =   8, // tkArray
	SYMBOL_TKAS                                 =   9, // tkAs
	SYMBOL_TKASCIICHAR                          =  10, // tkAsciiChar
	SYMBOL_TKASMBODY                            =  11, // tkAsmBody
	SYMBOL_TKASSEMBLER                          =  12, // tkAssembler
	SYMBOL_TKASSIGN                             =  13, // tkAssign
	SYMBOL_TKAT                                 =  14, // tkAt
	SYMBOL_TKAUTOMATED                          =  15, // tkAutomated
	SYMBOL_TKBEGIN                              =  16, // tkBegin
	SYMBOL_TKBF                                 =  17, // tkBF
	SYMBOL_TKBOOLEAN                            =  18, // tkBoolean
	SYMBOL_TKBYTE                               =  19, // tkByte
	SYMBOL_TKCARDINAL                           =  20, // tkCardinal
	SYMBOL_TKCASE                               =  21, // tkCase
	SYMBOL_TKCHAR                               =  22, // tkChar
	SYMBOL_TKCLASS                              =  23, // tkClass
	SYMBOL_TKCOLON                              =  24, // tkColon
	SYMBOL_TKCOMMA                              =  25, // tkComma
	SYMBOL_TKCOMP                               =  26, // tkComp
	SYMBOL_TKCONST                              =  27, // tkConst
	SYMBOL_TKCONSTRUCTOR                        =  28, // tkConstructor
	SYMBOL_TKCONTAINS                           =  29, // tkContains
	SYMBOL_TKDEFAULT                            =  30, // tkDefault
	SYMBOL_TKDEREF                              =  31, // tkDeref
	SYMBOL_TKDESTRUCTOR                         =  32, // tkDestructor
	SYMBOL_TKDIRECTIVENAME                      =  33, // tkDirectiveName
	SYMBOL_TKDISPID                             =  34, // tkDispid
	SYMBOL_TKDIV                                =  35, // tkDiv
	SYMBOL_TKDIVEQUAL                           =  36, // tkDivEqual
	SYMBOL_TKDO                                 =  37, // tkDo
	SYMBOL_TKDOTDOT                             =  38, // tkDotDot
	SYMBOL_TKDOUBLE                             =  39, // tkDouble
	SYMBOL_TKDOWNTO                             =  40, // tkDownto
	SYMBOL_TKELSE                               =  41, // tkElse
	SYMBOL_TKEND                                =  42, // tkEnd
	SYMBOL_TKEQUAL                              =  43, // tkEqual
	SYMBOL_TKERROR                              =  44, // tkError
	SYMBOL_TKEVENT                              =  45, // tkEvent
	SYMBOL_TKEXCEPT                             =  46, // tkExcept
	SYMBOL_TKEXPORTS                            =  47, // tkExports
	SYMBOL_TKEXTENDED                           =  48, // tkExtended
	SYMBOL_TKEXTERNAL                           =  49, // tkExternal
	SYMBOL_TKFILE                               =  50, // tkFile
	SYMBOL_TKFINAL                              =  51, // tkFinal
	SYMBOL_TKFINALIZATION                       =  52, // tkFinalization
	SYMBOL_TKFINALLY                            =  53, // tkFinally
	SYMBOL_TKFLOAT                              =  54, // tkFloat
	SYMBOL_TKFOR                                =  55, // tkFor
	SYMBOL_TKFOREACH                            =  56, // tkForeach
	SYMBOL_TKFORWARD                            =  57, // tkForward
	SYMBOL_TKFUNCTION                           =  58, // tkFunction
	SYMBOL_TKGOTO                               =  59, // tkGoto
	SYMBOL_TKGREATER                            =  60, // tkGreater
	SYMBOL_TKGREATEREQUAL                       =  61, // tkGreaterEqual
	SYMBOL_TKHEX                                =  62, // tkHex
	SYMBOL_TKIDENTIFIER                         =  63, // tkIdentifier
	SYMBOL_TKIF                                 =  64, // tkIf
	SYMBOL_TKILCODE                             =  65, // tkILCode
	SYMBOL_TKIMPLEMENTATION                     =  66, // tkImplementation
	SYMBOL_TKIMPLEMENTS                         =  67, // tkImplements
	SYMBOL_TKIN                                 =  68, // tkIn
	SYMBOL_TKINDEX                              =  69, // tkIndex
	SYMBOL_TKINHERITED                          =  70, // tkInherited
	SYMBOL_TKINITIALIZATION                     =  71, // tkInitialization
	SYMBOL_TKINLINE                             =  72, // tkInline
	SYMBOL_TKINT64                              =  73, // tkInt64
	SYMBOL_TKINTEGER                            =  74, // tkInteger
	SYMBOL_TKINTERFACE                          =  75, // tkInterface
	SYMBOL_TKINTERNAL                           =  76, // tkInternal
	SYMBOL_TKIS                                 =  77, // tkIs
	SYMBOL_TKLABEL                              =  78, // tkLabel
	SYMBOL_TKLIBRARY                            =  79, // tkLibrary
	SYMBOL_TKLOCK                               =  80, // tkLock
	SYMBOL_TKLONGINT                            =  81, // tkLongInt
	SYMBOL_TKLONGWORD                           =  82, // tkLongWord
	SYMBOL_TKLOWER                              =  83, // tkLower
	SYMBOL_TKLOWEREQUAL                         =  84, // tkLowerEqual
	SYMBOL_TKMESSAGE                            =  85, // tkMessage
	SYMBOL_TKMINUS                              =  86, // tkMinus
	SYMBOL_TKMINUSEQUAL                         =  87, // tkMinusEqual
	SYMBOL_TKMOD                                =  88, // tkMod
	SYMBOL_TKMULTEQUAL                          =  89, // tkMultEqual
	SYMBOL_TKNAME                               =  90, // tkName
	SYMBOL_TKNIL                                =  91, // tkNil
	SYMBOL_TKNODEFAULT                          =  92, // tkNodefault
	SYMBOL_TKNOT                                =  93, // tkNot
	SYMBOL_TKNOTEQUAL                           =  94, // tkNotEqual
	SYMBOL_TKOBJECT                             =  95, // tkObject
	SYMBOL_TKOF                                 =  96, // tkOf
	SYMBOL_TKOLEVARIANT                         =  97, // tkOleVariant
	SYMBOL_TKON                                 =  98, // tkOn
	SYMBOL_TKOPERATOR                           =  99, // tkOperator
	SYMBOL_TKOR                                 = 100, // tkOr
	SYMBOL_TKORDINTEGER                         = 101, // tkOrdInteger
	SYMBOL_TKOUT                                = 102, // tkOut
	SYMBOL_TKOVERLOAD                           = 103, // tkOverload
	SYMBOL_TKOVERRIDE                           = 104, // tkOverride
	SYMBOL_TKPACKAGE                            = 105, // tkPackage
	SYMBOL_TKPACKED                             = 106, // tkPacked
	SYMBOL_TKPARAMS                             = 107, // tkParams
	SYMBOL_TKPARSEMODEEXPRESSION                = 108, // tkParseModeExpression
	SYMBOL_TKPCHAR                              = 109, // tkPChar
	SYMBOL_TKPLUS                               = 110, // tkPlus
	SYMBOL_TKPLUSEQUAL                          = 111, // tkPlusEqual
	SYMBOL_TKPOINT                              = 112, // tkPoint
	SYMBOL_TKPRIVATE                            = 113, // tkPrivate
	SYMBOL_TKPROCEDURE                          = 114, // tkProcedure
	SYMBOL_TKPROGRAM                            = 115, // tkProgram
	SYMBOL_TKPROPERTY                           = 116, // tkProperty
	SYMBOL_TKPROTECTED                          = 117, // tkProtected
	SYMBOL_TKPUBLIC                             = 118, // tkPublic
	SYMBOL_TKQUESTION                           = 119, // tkQuestion
	SYMBOL_TKRAISE                              = 120, // tkRaise
	SYMBOL_TKREAD                               = 121, // tkRead
	SYMBOL_TKREADONLY                           = 122, // tkReadOnly
	SYMBOL_TKREAL                               = 123, // tkReal
	SYMBOL_TKRECORD                             = 124, // tkRecord
	SYMBOL_TKREINTRODUCE                        = 125, // tkReintroduce
	SYMBOL_TKREPEAT                             = 126, // tkRepeat
	SYMBOL_TKREQUIRES                           = 127, // tkRequires
	SYMBOL_TKRESIDENT                           = 128, // tkResident
	SYMBOL_TKRESOURCESTRING                     = 129, // tkResourceString
	SYMBOL_TKROUNDCLOSE                         = 130, // tkRoundClose
	SYMBOL_TKROUNDOPEN                          = 131, // tkRoundOpen
	SYMBOL_TKSEMICOLON                          = 132, // tkSemiColon
	SYMBOL_TKSET                                = 133, // tkSet
	SYMBOL_TKSHL                                = 134, // tkShl
	SYMBOL_TKSHORTINT                           = 135, // tkShortInt
	SYMBOL_TKSHR                                = 136, // tkShr
	SYMBOL_TKSINGLE                             = 137, // tkSingle
	SYMBOL_TKSIZEOF                             = 138, // tkSizeOf
	SYMBOL_TKSLASH                              = 139, // tkSlash
	SYMBOL_TKSMALLINT                           = 140, // tkSmallInt
	SYMBOL_TKSQUARECLOSE                        = 141, // tkSquareClose
	SYMBOL_TKSQUAREOPEN                         = 142, // tkSquareOpen
	SYMBOL_TKSTAR                               = 143, // tkStar
	SYMBOL_TKSTATIC                             = 144, // tkStatic
	SYMBOL_TKSTORED                             = 145, // tkStored
	SYMBOL_TKSTRINGLITERAL                      = 146, // tkStringLiteral
	SYMBOL_TKTEMPLATE                           = 147, // tkTemplate
	SYMBOL_TKTHEN                               = 148, // tkThen
	SYMBOL_TKTHREADVAR                          = 149, // tkThreadvar
	SYMBOL_TKTO                                 = 150, // tkTo
	SYMBOL_TKTRY                                = 151, // tkTry
	SYMBOL_TKTYPE                               = 152, // tkType
	SYMBOL_TKTYPEOF                             = 153, // tkTypeOf
	SYMBOL_TKUNIT                               = 154, // tkUnit
	SYMBOL_TKUNTIL                              = 155, // tkUntil
	SYMBOL_TKUSES                               = 156, // tkUses
	SYMBOL_TKUSING                              = 157, // tkUsing
	SYMBOL_TKVAR                                = 158, // tkVar
	SYMBOL_TKVARIANT                            = 159, // tkVariant
	SYMBOL_TKVIRTUAL                            = 160, // tkVirtual
	SYMBOL_TKWHERE                              = 161, // tkWhere
	SYMBOL_TKWHILE                              = 162, // tkWhile
	SYMBOL_TKWIDECHAR                           = 163, // tkWideChar
	SYMBOL_TKWITH                               = 164, // tkWith
	SYMBOL_TKWORD                               = 165, // tkWord
	SYMBOL_TKWRITE                              = 166, // tkWrite
	SYMBOL_TKWRITEONLY                          = 167, // tkWriteOnly
	SYMBOL_TKXOR                                = 168, // tkXor
	SYMBOL_ABC_BLOCK                            = 169, // <abc_block>
	SYMBOL_ABC_CONSTRUCTOR_DECL                 = 170, // <abc_constructor_decl>
	SYMBOL_ABC_DECL_SECT                        = 171, // <abc_decl_sect>
	SYMBOL_ABC_DECL_SECT_LIST                   = 172, // <abc_decl_sect_list>
	SYMBOL_ABC_DECL_SECT_LIST1                  = 173, // <abc_decl_sect_list1>
	SYMBOL_ABC_DESTRUCTOR_DECL                  = 174, // <abc_destructor_decl>
	SYMBOL_ABC_EXTERNAL_DIRECTR                 = 175, // <abc_external_directr>
	SYMBOL_ABC_FUNC_DECL                        = 176, // <abc_func_decl>
	SYMBOL_ABC_FUNC_DECL_NOCLASS                = 177, // <abc_func_decl_noclass>
	SYMBOL_ABC_INTERFACE_PART                   = 178, // <abc_interface_part>
	SYMBOL_ABC_METHOD_DECL                      = 179, // <abc_method_decl>
	SYMBOL_ABC_PROC_BLOCK                       = 180, // <abc_proc_block>
	SYMBOL_ABC_PROC_DECL                        = 181, // <abc_proc_decl>
	SYMBOL_ABC_PROC_DECL_NOCLASS                = 182, // <abc_proc_decl_noclass>
	SYMBOL_ADDOP                                = 183, // <addop>
	SYMBOL_ALLOWABLE_EXPR_AS_STMT               = 184, // <allowable_expr_as_stmt>
	SYMBOL_ARRAY_CONST                          = 185, // <array_const>
	SYMBOL_ARRAY_NAME_FOR_NEW_EXPR              = 186, // <array_name_for_new_expr>
	SYMBOL_ARRAY_TYPE                           = 187, // <array_type>
	SYMBOL_ASM_BLOCK                            = 188, // <asm_block>
	SYMBOL_ASM_STMT                             = 189, // <asm_stmt>
	SYMBOL_ASSIGN_OPERATOR                      = 190, // <assign_operator>
	SYMBOL_ASSIGNMENT                           = 191, // <assignment>
	SYMBOL_BASE_CLASS_NAME                      = 192, // <base_class_name>
	SYMBOL_BASE_CLASSES_NAMES_LIST              = 193, // <base_classes_names_list>
	SYMBOL_BF_BLOCK                             = 194, // <bf_block>
	SYMBOL_BF_EMPTY_INSTRUCTION                 = 195, // <bf_empty_instruction>
	SYMBOL_BF_INSTRUCTION                       = 196, // <bf_instruction>
	SYMBOL_BF_INSTRUCTIONS                      = 197, // <bf_instructions>
	SYMBOL_BF_INSTRUCTIONS_LIST                 = 198, // <bf_instructions_list>
	SYMBOL_BLOCK                                = 199, // <block>
	SYMBOL_CASE_ITEM                            = 200, // <case_item>
	SYMBOL_CASE_LABEL                           = 201, // <case_label>
	SYMBOL_CASE_LABEL_LIST                      = 202, // <case_label_list>
	SYMBOL_CASE_LIST                            = 203, // <case_list>
	SYMBOL_CASE_STMT                            = 204, // <case_stmt>
	SYMBOL_CASE_TAG_LIST                        = 205, // <case_tag_list>
	SYMBOL_CLASS_ATTRIBUTES                     = 206, // <class_attributes>
	SYMBOL_CLASS_OR_INTERFACE_KEYWORD           = 207, // <class_or_interface_keyword>
	SYMBOL_COMPOUND_STMT                        = 208, // <compound_stmt>
	SYMBOL_CONST_ADDOP                          = 209, // <const_addop>
	SYMBOL_CONST_DECL                           = 210, // <const_decl>
	SYMBOL_CONST_DECL_SECT                      = 211, // <const_decl_sect>
	SYMBOL_CONST_ELEM                           = 212, // <const_elem>
	SYMBOL_CONST_ELEM_LIST                      = 213, // <const_elem_list>
	SYMBOL_CONST_ELEM_LIST1                     = 214, // <const_elem_list1>
	SYMBOL_CONST_EXPR                           = 215, // <const_expr>
	SYMBOL_CONST_EXPR_LIST                      = 216, // <const_expr_list>
	SYMBOL_CONST_FACTOR                         = 217, // <const_factor>
	SYMBOL_CONST_FIELD                          = 218, // <const_field>
	SYMBOL_CONST_FIELD_LIST                     = 219, // <const_field_list>
	SYMBOL_CONST_FIELD_LIST_1                   = 220, // <const_field_list_1>
	SYMBOL_CONST_FIELD_NAME                     = 221, // <const_field_name>
	SYMBOL_CONST_FUNC_EXPR_LIST                 = 222, // <const_func_expr_list>
	SYMBOL_CONST_MULOP                          = 223, // <const_mulop>
	SYMBOL_CONST_NAME                           = 224, // <const_name>
	SYMBOL_CONST_RELOP                          = 225, // <const_relop>
	SYMBOL_CONST_SET                            = 226, // <const_set>
	SYMBOL_CONST_SIMPLE_EXPR                    = 227, // <const_simple_expr>
	SYMBOL_CONST_TERM                           = 228, // <const_term>
	SYMBOL_CONST_VARIABLE                       = 229, // <const_variable>
	SYMBOL_CONST_VARIABLE_2                     = 230, // <const_variable_2>
	SYMBOL_CONSTRUCTOR_DECL                     = 231, // <constructor_decl>
	SYMBOL_CONTAINS_CLAUSE                      = 232, // <contains_clause>
	SYMBOL_DECLARED_VAR_NAME                    = 233, // <declared_var_name>
	SYMBOL_DESTRUCTOR_DECL                      = 234, // <destructor_decl>
	SYMBOL_ELEM                                 = 235, // <elem>
	SYMBOL_ELEM_LIST                            = 236, // <elem_list>
	SYMBOL_ELEM_LIST1                           = 237, // <elem_list1>
	SYMBOL_ELSE_BRANCH                          = 238, // <else_branch>
	SYMBOL_ELSE_CASE                            = 239, // <else_case>
	SYMBOL_EMPTY                                = 240, // <empty>
	SYMBOL_ENUMERATION_ID                       = 241, // <enumeration_id>
	SYMBOL_ENUMERATION_ID_LIST                  = 242, // <enumeration_id_list>
	SYMBOL_ERROR2                               = 243, // <error>
	SYMBOL_EXCEPTION_BLOCK                      = 244, // <exception_block>
	SYMBOL_EXCEPTION_BLOCK_ELSE_BRANCH          = 245, // <exception_block_else_branch>
	SYMBOL_EXCEPTION_CLASS_TYPE_IDENTIFIER      = 246, // <exception_class_type_identifier>
	SYMBOL_EXCEPTION_HANDLER                    = 247, // <exception_handler>
	SYMBOL_EXCEPTION_HANDLER_LIST               = 248, // <exception_handler_list>
	SYMBOL_EXCEPTION_IDENTIFIER                 = 249, // <exception_identifier>
	SYMBOL_EXCEPTION_VARIABLE                   = 250, // <exception_variable>
	SYMBOL_EXPORT_CLAUSE                        = 251, // <export_clause>
	SYMBOL_EXPORTS_ENTRY                        = 252, // <exports_entry>
	SYMBOL_EXPORTS_INDEX                        = 253, // <exports_index>
	SYMBOL_EXPORTS_LIST                         = 254, // <exports_list>
	SYMBOL_EXPORTS_NAME                         = 255, // <exports_name>
	SYMBOL_EXPORTS_RESIDENT                     = 256, // <exports_resident>
	SYMBOL_EXPR                                 = 257, // <expr>
	SYMBOL_EXPR_AS_STMT                         = 258, // <expr_as_stmt>
	SYMBOL_EXPR_L1                              = 259, // <expr_l1>
	SYMBOL_EXPR_LIST                            = 260, // <expr_list>
	SYMBOL_EXTERNAL_DIRECTR                     = 261, // <external_directr>
	SYMBOL_EXTERNAL_DIRECTR_IDENT               = 262, // <external_directr_ident>
	SYMBOL_FACTOR                               = 263, // <factor>
	SYMBOL_FIELD_ACCESS_MODIFIER                = 264, // <field_access_modifier>
	SYMBOL_FIELD_LIST                           = 265, // <field_list>
	SYMBOL_FILE_TYPE                            = 266, // <file_type>
	SYMBOL_FILED_OR_CONST_DEFINITION            = 267, // <filed_or_const_definition>
	SYMBOL_FILED_OR_CONST_DEFINITION_OR_AM      = 268, // <filed_or_const_definition_or_am>
	SYMBOL_FIXED_PART                           = 269, // <fixed_part>
	SYMBOL_FIXED_PART_2                         = 270, // <fixed_part_2>
	SYMBOL_FOR_CYCLE_TYPE                       = 271, // <for_cycle_type>
	SYMBOL_FOR_STMT                             = 272, // <for_stmt>
	SYMBOL_FOR_STMT_DECL_OR_ASSIGN              = 273, // <for_stmt_decl_or_assign>
	SYMBOL_FOREACH_STMT                         = 274, // <foreach_stmt>
	SYMBOL_FOREACH_STMT_IDENT_DYPE_OPT          = 275, // <foreach_stmt_ident_dype_opt>
	SYMBOL_FORMAT_EXPR                          = 276, // <format_expr>
	SYMBOL_FP_LIST                              = 277, // <fp_list>
	SYMBOL_FP_SECT                              = 278, // <fp_sect>
	SYMBOL_FP_SECT_LIST                         = 279, // <fp_sect_list>
	SYMBOL_FPTYPE                               = 280, // <fptype>
	SYMBOL_FPTYPE_NEW                           = 281, // <fptype_new>
	SYMBOL_FUNC_BLOCK                           = 282, // <func_block>
	SYMBOL_FUNC_CLASS_NAME_IDENT                = 283, // <func_class_name_ident>
	SYMBOL_FUNC_DECL                            = 284, // <func_decl>
	SYMBOL_FUNC_DECL_NOCLASS                    = 285, // <func_decl_noclass>
	SYMBOL_FUNC_HEADING                         = 286, // <func_heading>
	SYMBOL_FUNC_METH_NAME_IDENT                 = 287, // <func_meth_name_ident>
	SYMBOL_FUNC_NAME                            = 288, // <func_name>
	SYMBOL_GOTO_STMT                            = 289, // <goto_stmt>
	SYMBOL_HEAD_COMPILER_DIRECTIVES             = 290, // <head_compiler_directives>
	SYMBOL_IDENT_LIST                           = 291, // <ident_list>
	SYMBOL_IDENT_OR_KEYWORD_POINTSEPARATOR_LIST = 292, // <ident_or_keyword_pointseparator_list>
	SYMBOL_IDENTIFIER                           = 293, // <identifier>
	SYMBOL_IDENTIFIER_KEYWORD_OPERATORNAME      = 294, // <identifier_keyword_operatorname>
	SYMBOL_IDENTIFIER_OR_KEYWORD                = 295, // <identifier_or_keyword>
	SYMBOL_IF_STMT                              = 296, // <if_stmt>
	SYMBOL_IF_THEN_ELSE_BRANCH                  = 297, // <if_then_else_branch>
	SYMBOL_IMPL_DECL_SECT                       = 298, // <impl_decl_sect>
	SYMBOL_IMPL_DECL_SECT_LIST                  = 299, // <impl_decl_sect_list>
	SYMBOL_IMPL_DECL_SECT_LIST1                 = 300, // <impl_decl_sect_list1>
	SYMBOL_IMPLEMENTATION_PART                  = 301, // <implementation_part>
	SYMBOL_INHERITED_MESSAGE                    = 302, // <inherited_message>
	SYMBOL_INIT_CONST_EXPR                      = 303, // <init_const_expr>
	SYMBOL_INITIALIZATION_PART                  = 304, // <initialization_part>
	SYMBOL_INT_DECL_SECT                        = 305, // <int_decl_sect>
	SYMBOL_INT_DECL_SECT_LIST                   = 306, // <int_decl_sect_list>
	SYMBOL_INT_DECL_SECT_LIST1                  = 307, // <int_decl_sect_list1>
	SYMBOL_INT_FUNC_HEADING                     = 308, // <int_func_heading>
	SYMBOL_INT_PROC_HEADING                     = 309, // <int_proc_heading>
	SYMBOL_INTEGER_CONST                        = 310, // <integer_const>
	SYMBOL_INTERFACE_PART                       = 311, // <interface_part>
	SYMBOL_KEYWORD                              = 312, // <keyword>
	SYMBOL_LABEL_DECL_SECT                      = 313, // <label_decl_sect>
	SYMBOL_LABEL_LIST                           = 314, // <label_list>
	SYMBOL_LABEL_NAME                           = 315, // <label_name>
	SYMBOL_LIBRARY_BLOCK                        = 316, // <library_block>
	SYMBOL_LIBRARY_FILE                         = 317, // <library_file>
	SYMBOL_LIBRARY_HEADING                      = 318, // <library_heading>
	SYMBOL_LIBRARY_IMPL_DECL_SECT               = 319, // <library_impl_decl_sect>
	SYMBOL_LIBRARY_IMPL_DECL_SECT_LIST          = 320, // <library_impl_decl_sect_list>
	SYMBOL_LITERAL                              = 321, // <literal>
	SYMBOL_LITERAL_LIST                         = 322, // <literal_list>
	SYMBOL_LITERAL_OR_NUMBER                    = 323, // <literal_or_number>
	SYMBOL_LOCK_STMT                            = 324, // <lock_stmt>
	SYMBOL_MAIN_USED_UNIT_NAME                  = 325, // <main_used_unit_name>
	SYMBOL_MAIN_USED_UNITS_LIST                 = 326, // <main_used_units_list>
	SYMBOL_MAIN_USES_CLAUSE                     = 327, // <main_uses_clause>
	SYMBOL_MAYBE_ERROR                          = 328, // <maybe_error>
	SYMBOL_METH_MODIFICATOR                     = 329, // <meth_modificator>
	SYMBOL_METH_MODIFICATORS                    = 330, // <meth_modificators>
	SYMBOL_MULOP                                = 331, // <mulop>
	SYMBOL_NEW_EXPR                             = 332, // <new_expr>
	SYMBOL_NEW_OBJECT_TYPE                      = 333, // <new_object_type>
	SYMBOL_NEW_RECORD_TYPE                      = 334, // <new_record_type>
	SYMBOL_NON_RESERVED                         = 335, // <non_reserved>
	SYMBOL_NOT_ARRAY_DEFAULTPROPERTY            = 336, // <not_array_defaultproperty>
	SYMBOL_NOT_COMPONENT_LIST                   = 337, // <not_component_list>
	SYMBOL_NOT_COMPONENT_LIST_1                 = 338, // <not_component_list_1>
	SYMBOL_NOT_COMPONENT_LIST_2                 = 339, // <not_component_list_2>
	SYMBOL_NOT_COMPONENT_LIST_SEQ               = 340, // <not_component_list_seq>
	SYMBOL_NOT_CONSTRUCTOR_BLOCK_DECL           = 341, // <not_constructor_block_decl>
	SYMBOL_NOT_CONSTRUCTOR_HEADING              = 342, // <not_constructor_heading>
	SYMBOL_NOT_DESTRUCTOR_HEADING               = 343, // <not_destructor_heading>
	SYMBOL_NOT_FIELD_DEFINITION                 = 344, // <not_field_definition>
	SYMBOL_NOT_GUID                             = 345, // <not_guid>
	SYMBOL_NOT_METHOD_DEFINITION                = 346, // <not_method_definition>
	SYMBOL_NOT_METHOD_HEADING                   = 347, // <not_method_heading>
	SYMBOL_NOT_OBJECT_TYPE                      = 348, // <not_object_type>
	SYMBOL_NOT_OBJECT_TYPE_IDENTIFIER_LIST      = 349, // <not_object_type_identifier_list>
	SYMBOL_NOT_PARAMETER_DECL                   = 350, // <not_parameter_decl>
	SYMBOL_NOT_PARAMETER_DECL_LIST              = 351, // <not_parameter_decl_list>
	SYMBOL_NOT_PARAMETER_NAME_LIST              = 352, // <not_parameter_name_list>
	SYMBOL_NOT_PROPERTY_DEFINITION              = 353, // <not_property_definition>
	SYMBOL_NOT_PROPERTY_INTERFACE               = 354, // <not_property_interface>
	SYMBOL_NOT_PROPERTY_INTERFACE_INDEX         = 355, // <not_property_interface_index>
	SYMBOL_NOT_PROPERTY_PARAMETER_LIST          = 356, // <not_property_parameter_list>
	SYMBOL_NOT_PROPERTY_SPECIFIERS              = 357, // <not_property_specifiers>
	SYMBOL_OBJECT_TYPE                          = 358, // <object_type>
	SYMBOL_ONE_COMPILER_DIRECTIVE               = 359, // <one_compiler_directive>
	SYMBOL_ONE_LITERAL                          = 360, // <one_literal>
	SYMBOL_ONLY_CONST_DECL                      = 361, // <only_const_decl>
	SYMBOL_OOT_COMPONENT_LIST                   = 362, // <oot_component_list>
	SYMBOL_OOT_CONSTRUCTOR_HEAD                 = 363, // <oot_constructor_head>
	SYMBOL_OOT_DESTRUCTOR_HEAD                  = 364, // <oot_destructor_head>
	SYMBOL_OOT_FIELD                            = 365, // <oot_field>
	SYMBOL_OOT_FIELD_IDENTIFIER                 = 366, // <oot_field_identifier>
	SYMBOL_OOT_FIELD_LIST                       = 367, // <oot_field_list>
	SYMBOL_OOT_ID_LIST                          = 368, // <oot_id_list>
	SYMBOL_OOT_METHOD                           = 369, // <oot_method>
	SYMBOL_OOT_METHOD_HEAD                      = 370, // <oot_method_head>
	SYMBOL_OOT_METHOD_LIST                      = 371, // <oot_method_list>
	SYMBOL_OOT_PRIVAT_LIST                      = 372, // <oot_privat_list>
	SYMBOL_OOT_SUCCESSOR                        = 373, // <oot_successor>
	SYMBOL_OOT_TYPEIDENTIFIER                   = 374, // <oot_typeidentifier>
	SYMBOL_OPERATOR_NAME_IDENT                  = 375, // <operator_name_ident>
	SYMBOL_OPT_BASE_CLASSES                     = 376, // <opt_base_classes>
	SYMBOL_OPT_EXPR_LIST                        = 377, // <opt_expr_list>
	SYMBOL_OPT_EXPR_LIST_WITH_BRACKET           = 378, // <opt_expr_list_with_bracket>
	SYMBOL_OPT_HEAD_COMPILER_DIRECTIVES         = 379, // <opt_head_compiler_directives>
	SYMBOL_OPT_IDENTIFIER                       = 380, // <opt_identifier>
	SYMBOL_OPT_METH_MODIFICATORS                = 381, // <opt_meth_modificators>
	SYMBOL_OPT_NOT_COMPONENT_LIST_SEQ_END       = 382, // <opt_not_component_list_seq_end>
	SYMBOL_OPT_SEMICOLON                        = 383, // <opt_semicolon>
	SYMBOL_OPT_TEMPLATE_ARGUMENTS               = 384, // <opt_template_arguments>
	SYMBOL_OPT_TEMPLATE_TYPE_PARAMS             = 385, // <opt_template_type_params>
	SYMBOL_OPT_VAR                              = 386, // <opt_var>
	SYMBOL_OPT_WHERE_SECTION                    = 387, // <opt_where_section>
	SYMBOL_OPTIONAL_QUALIFIED_IDENTIFIER        = 388, // <optional_qualified_identifier>
	SYMBOL_ORD_TYPE_NAME                        = 389, // <ord_type_name>
	SYMBOL_OT_VISIBILITY_SPECIFIER              = 390, // <ot_visibility_specifier>
	SYMBOL_OTHER                                = 391, // <other>
	SYMBOL_OVERLOAD_OPERATOR                    = 392, // <overload_operator>
	SYMBOL_PACKAGE_FILE                         = 393, // <package_file>
	SYMBOL_PACKAGE_NAME                         = 394, // <package_name>
	SYMBOL_PARAM_NAME                           = 395, // <param_name>
	SYMBOL_PARAM_NAME_LIST                      = 396, // <param_name_list>
	SYMBOL_PARSE_GOAL                           = 397, // <parse_goal>
	SYMBOL_PARTS                                = 398, // <parts>
	SYMBOL_POINTER_TYPE                         = 399, // <pointer_type>
	SYMBOL_PROC_BLOCK                           = 400, // <proc_block>
	SYMBOL_PROC_BLOCK_DECL                      = 401, // <proc_block_decl>
	SYMBOL_PROC_CALL                            = 402, // <proc_call>
	SYMBOL_PROC_DECL                            = 403, // <proc_decl>
	SYMBOL_PROC_DECL_NOCLASS                    = 404, // <proc_decl_noclass>
	SYMBOL_PROC_HEADING                         = 405, // <proc_heading>
	SYMBOL_PROC_NAME                            = 406, // <proc_name>
	SYMBOL_PROCEDURAL_TYPE                      = 407, // <procedural_type>
	SYMBOL_PROCEDURAL_TYPE_DECL                 = 408, // <procedural_type_decl>
	SYMBOL_PROCEDURAL_TYPE_KIND                 = 409, // <procedural_type_kind>
	SYMBOL_PROGRAM_BLOCK                        = 410, // <program_block>
	SYMBOL_PROGRAM_DECL_SECT_LIST               = 411, // <program_decl_sect_list>
	SYMBOL_PROGRAM_FILE                         = 412, // <program_file>
	SYMBOL_PROGRAM_HEADING                      = 413, // <program_heading>
	SYMBOL_PROGRAM_HEADING_2                    = 414, // <program_heading_2>
	SYMBOL_PROGRAM_NAME                         = 415, // <program_name>
	SYMBOL_PROGRAM_PARAM                        = 416, // <program_param>
	SYMBOL_PROGRAM_PARAM_LIST                   = 417, // <program_param_list>
	SYMBOL_PROPERTY_SPECIFIER_DIRECTIVES        = 418, // <property_specifier_directives>
	SYMBOL_QUALIFIED_IDENTIFIER                 = 419, // <qualified_identifier>
	SYMBOL_QUESTION_EXPR                        = 420, // <question_expr>
	SYMBOL_RAISE_STMT                           = 421, // <raise_stmt>
	SYMBOL_RANGE_EXPR                           = 422, // <range_expr>
	SYMBOL_RANGE_FACTOR                         = 423, // <range_factor>
	SYMBOL_RANGE_METHODNAME                     = 424, // <range_methodname>
	SYMBOL_RANGE_TERM                           = 425, // <range_term>
	SYMBOL_REAL_TYPE_NAME                       = 426, // <real_type_name>
	SYMBOL_RECORD_COMPONENT_LIST                = 427, // <record_component_list>
	SYMBOL_RECORD_CONST                         = 428, // <record_const>
	SYMBOL_RECORD_KEYWORD                       = 429, // <record_keyword>
	SYMBOL_RECORD_SECTION                       = 430, // <record_section>
	SYMBOL_RECORD_SECTION_ID                    = 431, // <record_section_id>
	SYMBOL_RECORD_SECTION_ID_LIST               = 432, // <record_section_id_list>
	SYMBOL_RECORD_TYPE                          = 433, // <record_type>
	SYMBOL_RELOP                                = 434, // <relop>
	SYMBOL_RELOP_EXPR                           = 435, // <relop_expr>
	SYMBOL_REPEAT_STMT                          = 436, // <repeat_stmt>
	SYMBOL_REQUIRES_CLAUSE                      = 437, // <requires_clause>
	SYMBOL_RES_STR_DECL_SECT                    = 438, // <res_str_decl_sect>
	SYMBOL_RESERVED_KEYWORD                     = 439, // <reserved_keyword>
	SYMBOL_SET_TYPE                             = 440, // <set_type>
	SYMBOL_SIGN                                 = 441, // <sign>
	SYMBOL_SIMPLE_EXPR                          = 442, // <simple_expr>
	SYMBOL_SIMPLE_TYPE                          = 443, // <simple_type>
	SYMBOL_SIMPLE_TYPE_IDENTIFIER               = 444, // <simple_type_identifier>
	SYMBOL_SIMPLE_TYPE_LIST                     = 445, // <simple_type_list>
	SYMBOL_SIZEOF_EXPR                          = 446, // <sizeof_expr>
	SYMBOL_STMT                                 = 447, // <stmt>
	SYMBOL_STMT_LIST                            = 448, // <stmt_list>
	SYMBOL_STRING_TYPE                          = 449, // <string_type>
	SYMBOL_STRUCTURED_TYPE                      = 450, // <structured_type>
	SYMBOL_TAG_FIELD                            = 451, // <tag_field>
	SYMBOL_TAG_FIELD_NAME                       = 452, // <tag_field_name>
	SYMBOL_TAG_FIELD_TYPENAME                   = 453, // <tag_field_typename>
	SYMBOL_TEMPLATE_PARAM                       = 454, // <template_param>
	SYMBOL_TEMPLATE_PARAM_LIST                  = 455, // <template_param_list>
	SYMBOL_TEMPLATE_TYPE                        = 456, // <template_type>
	SYMBOL_TEMPLATE_TYPE_BACK_VARSPECIFIERS     = 457, // <template_type_back_varspecifiers>
	SYMBOL_TEMPLATE_TYPE_PARAMS                 = 458, // <template_type_params>
	SYMBOL_TERM                                 = 459, // <term>
	SYMBOL_THEN_BRANCH                          = 460, // <then_branch>
	SYMBOL_TRY_HANDLER                          = 461, // <try_handler>
	SYMBOL_TRY_STMT                             = 462, // <try_stmt>
	SYMBOL_TYPE_DECL                            = 463, // <type_decl>
	SYMBOL_TYPE_DECL_SECT                       = 464, // <type_decl_sect>
	SYMBOL_TYPE_DECL_TYPE                       = 465, // <type_decl_type>
	SYMBOL_TYPE_REF                             = 466, // <type_ref>
	SYMBOL_TYPE_REF_AND_SECIFIC_LIST            = 467, // <type_ref_and_secific_list>
	SYMBOL_TYPE_REF_OR_SECIFIC                  = 468, // <type_ref_or_secific>
	SYMBOL_TYPECAST_OP                          = 469, // <typecast_op>
	SYMBOL_TYPED_CONST                          = 470, // <typed_const>
	SYMBOL_TYPED_CONST_LIST                     = 471, // <typed_const_list>
	SYMBOL_TYPEOF_EXPR                          = 472, // <typeof_expr>
	SYMBOL_UNIT_FILE                            = 473, // <unit_file>
	SYMBOL_UNIT_HEADING                         = 474, // <unit_heading>
	SYMBOL_UNIT_KEY_WORD                        = 475, // <unit_key_word>
	SYMBOL_UNIT_NAME                            = 476, // <unit_name>
	SYMBOL_UNLABELLED_STMT                      = 477, // <unlabelled_stmt>
	SYMBOL_UNPACKED_STRUCTURED_TYPE             = 478, // <unpacked_structured_type>
	SYMBOL_UNSIGNED_NUMBER                      = 479, // <unsigned_number>
	SYMBOL_UNSIZED_ARRAY_TYPE                   = 480, // <unsized_array_type>
	SYMBOL_USES_CLAUSE                          = 481, // <uses_clause>
	SYMBOL_USING_CLAUSE                         = 482, // <using_clause>
	SYMBOL_USING_LIST                           = 483, // <using_list>
	SYMBOL_USING_ONE                            = 484, // <using_one>
	SYMBOL_VAR_ADDRESS                          = 485, // <var_address>
	SYMBOL_VAR_DECL                             = 486, // <var_decl>
	SYMBOL_VAR_DECL_PART                        = 487, // <var_decl_part>
	SYMBOL_VAR_DECL_PART_ASSIGN                 = 488, // <var_decl_part_assign>
	SYMBOL_VAR_DECL_PART_IN_STMT                = 489, // <var_decl_part_in_stmt>
	SYMBOL_VAR_DECL_PART_NORMAL                 = 490, // <var_decl_part_normal>
	SYMBOL_VAR_DECL_SECT                        = 491, // <var_decl_sect>
	SYMBOL_VAR_INIT_VALUE                       = 492, // <var_init_value>
	SYMBOL_VAR_INIT_VALUE_TYPED                 = 493, // <var_init_value_typed>
	SYMBOL_VAR_NAME                             = 494, // <var_name>
	SYMBOL_VAR_NAME_LIST                        = 495, // <var_name_list>
	SYMBOL_VAR_REFERENCE                        = 496, // <var_reference>
	SYMBOL_VAR_SPECIFIERS                       = 497, // <var_specifiers>
	SYMBOL_VAR_STMT                             = 498, // <var_stmt>
	SYMBOL_VARIABLE                             = 499, // <variable>
	SYMBOL_VARIANT                              = 500, // <variant>
	SYMBOL_VARIANT_FIELD_LIST                   = 501, // <variant_field_list>
	SYMBOL_VARIANT_LIST                         = 502, // <variant_list>
	SYMBOL_VARIANT_LIST_2                       = 503, // <variant_list_2>
	SYMBOL_VARIANT_PART                         = 504, // <variant_part>
	SYMBOL_VARIANT_TYPE_NAME                    = 505, // <variant_type_name>
	SYMBOL_VISIBILITY_SPECIFIER                 = 506, // <visibility_specifier>
	SYMBOL_WHERE_PART                           = 507, // <where_part>
	SYMBOL_WHERE_PART_LIST                      = 508, // <where_part_list>
	SYMBOL_WHILE_STMT                           = 509, // <while_stmt>
	SYMBOL_WITH_STMT                            = 510  // <with_stmt>
};














///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//RuleConstants
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

public enum RuleConstants : int
{
	RULE_PARSE_GOAL                                                 =   0, // <parse_goal> ::= <program_file>
	RULE_PARSE_GOAL2                                                =   1, // <parse_goal> ::= <unit_file>
	RULE_PARSE_GOAL3                                                =   2, // <parse_goal> ::= <parts>
	RULE_PARTS_TKPARSEMODEEXPRESSION                                =   3, // <parts> ::= tkParseModeExpression <expr>
	RULE_OPT_HEAD_COMPILER_DIRECTIVES                               =   4, // <opt_head_compiler_directives> ::= 
	RULE_OPT_HEAD_COMPILER_DIRECTIVES2                              =   5, // <opt_head_compiler_directives> ::= <head_compiler_directives>
	RULE_HEAD_COMPILER_DIRECTIVES                                   =   6, // <head_compiler_directives> ::= <one_compiler_directive>
	RULE_HEAD_COMPILER_DIRECTIVES2                                  =   7, // <head_compiler_directives> ::= <head_compiler_directives> <one_compiler_directive>
	RULE_ONE_COMPILER_DIRECTIVE_TKDIRECTIVENAME_TKIDENTIFIER        =   8, // <one_compiler_directive> ::= tkDirectiveName tkIdentifier
	RULE_ONE_COMPILER_DIRECTIVE_TKDIRECTIVENAME_TKSTRINGLITERAL     =   9, // <one_compiler_directive> ::= tkDirectiveName tkStringLiteral
	RULE_PROGRAM_FILE_TKPOINT                                       =  10, // <program_file> ::= <program_heading> <opt_head_compiler_directives> <main_uses_clause> <using_clause> <program_block> tkPoint
	RULE_PROGRAM_HEADING                                            =  11, // <program_heading> ::= 
	RULE_PROGRAM_HEADING_TKPROGRAM                                  =  12, // <program_heading> ::= tkProgram <program_name> <program_heading_2>
	RULE_PROGRAM_HEADING_2_TKSEMICOLON                              =  13, // <program_heading_2> ::= tkSemiColon
	RULE_PROGRAM_HEADING_2_TKROUNDOPEN_TKROUNDCLOSE_TKSEMICOLON     =  14, // <program_heading_2> ::= tkRoundOpen <program_param_list> tkRoundClose tkSemiColon
	RULE_PROGRAM_NAME_TKIDENTIFIER                                  =  15, // <program_name> ::= tkIdentifier
	RULE_PROGRAM_PARAM_LIST                                         =  16, // <program_param_list> ::= <program_param>
	RULE_PROGRAM_PARAM_LIST_TKCOMMA                                 =  17, // <program_param_list> ::= <program_param_list> tkComma <program_param>
	RULE_PROGRAM_PARAM_TKIDENTIFIER                                 =  18, // <program_param> ::= tkIdentifier
	RULE_PROGRAM_BLOCK                                              =  19, // <program_block> ::= <program_decl_sect_list> <compound_stmt>
	RULE_PROGRAM_DECL_SECT_LIST                                     =  20, // <program_decl_sect_list> ::= <impl_decl_sect_list>
	RULE_USES_CLAUSE                                                =  21, // <uses_clause> ::= <main_uses_clause>
	RULE_USING_CLAUSE                                               =  22, // <using_clause> ::= 
	RULE_USING_CLAUSE2                                              =  23, // <using_clause> ::= <using_list>
	RULE_USING_LIST                                                 =  24, // <using_list> ::= <using_one> <empty>
	RULE_USING_LIST2                                                =  25, // <using_list> ::= <using_list> <using_one>
	RULE_USING_ONE_TKUSING_TKSEMICOLON                              =  26, // <using_one> ::= tkUsing <ident_or_keyword_pointseparator_list> tkSemiColon
	RULE_IDENT_OR_KEYWORD_POINTSEPARATOR_LIST                       =  27, // <ident_or_keyword_pointseparator_list> ::= <identifier_or_keyword> <empty>
	RULE_IDENT_OR_KEYWORD_POINTSEPARATOR_LIST_TKPOINT               =  28, // <ident_or_keyword_pointseparator_list> ::= <ident_or_keyword_pointseparator_list> tkPoint <identifier_or_keyword>
	RULE_MAIN_USES_CLAUSE                                           =  29, // <main_uses_clause> ::= 
	RULE_MAIN_USES_CLAUSE_TKUSES_TKSEMICOLON                        =  30, // <main_uses_clause> ::= tkUses <main_used_units_list> tkSemiColon
	RULE_MAIN_USED_UNITS_LIST_TKCOMMA                               =  31, // <main_used_units_list> ::= <main_used_units_list> tkComma <main_used_unit_name>
	RULE_MAIN_USED_UNITS_LIST                                       =  32, // <main_used_units_list> ::= <main_used_unit_name> <empty>
	RULE_MAIN_USED_UNIT_NAME                                        =  33, // <main_used_unit_name> ::= <ident_or_keyword_pointseparator_list> <empty>
	RULE_MAIN_USED_UNIT_NAME_TKIN_TKSTRINGLITERAL                   =  34, // <main_used_unit_name> ::= <ident_or_keyword_pointseparator_list> tkIn tkStringLiteral
	RULE_LIBRARY_FILE_TKPOINT                                       =  35, // <library_file> ::= <library_heading> <main_uses_clause> <library_block> tkPoint
	RULE_LIBRARY_HEADING_TKLIBRARY_TKIDENTIFIER_TKSEMICOLON         =  36, // <library_heading> ::= tkLibrary tkIdentifier tkSemiColon
	RULE_LIBRARY_BLOCK                                              =  37, // <library_block> ::= <library_impl_decl_sect_list> <compound_stmt>
	RULE_LIBRARY_IMPL_DECL_SECT_LIST                                =  38, // <library_impl_decl_sect_list> ::= 
	RULE_LIBRARY_IMPL_DECL_SECT_LIST2                               =  39, // <library_impl_decl_sect_list> ::= <library_impl_decl_sect_list> <library_impl_decl_sect>
	RULE_LIBRARY_IMPL_DECL_SECT                                     =  40, // <library_impl_decl_sect> ::= <label_decl_sect>
	RULE_LIBRARY_IMPL_DECL_SECT2                                    =  41, // <library_impl_decl_sect> ::= <const_decl_sect>
	RULE_LIBRARY_IMPL_DECL_SECT3                                    =  42, // <library_impl_decl_sect> ::= <res_str_decl_sect>
	RULE_LIBRARY_IMPL_DECL_SECT4                                    =  43, // <library_impl_decl_sect> ::= <type_decl_sect>
	RULE_LIBRARY_IMPL_DECL_SECT5                                    =  44, // <library_impl_decl_sect> ::= <var_decl_sect>
	RULE_LIBRARY_IMPL_DECL_SECT6                                    =  45, // <library_impl_decl_sect> ::= <proc_decl>
	RULE_LIBRARY_IMPL_DECL_SECT7                                    =  46, // <library_impl_decl_sect> ::= <func_decl>
	RULE_LIBRARY_IMPL_DECL_SECT8                                    =  47, // <library_impl_decl_sect> ::= <constructor_decl>
	RULE_LIBRARY_IMPL_DECL_SECT9                                    =  48, // <library_impl_decl_sect> ::= <destructor_decl>
	RULE_LIBRARY_IMPL_DECL_SECT10                                   =  49, // <library_impl_decl_sect> ::= <export_clause>
	RULE_EXPORT_CLAUSE_TKEXPORTS_TKSEMICOLON                        =  50, // <export_clause> ::= tkExports <exports_list> tkSemiColon
	RULE_EXPORTS_LIST                                               =  51, // <exports_list> ::= <exports_entry>
	RULE_EXPORTS_LIST_TKCOMMA                                       =  52, // <exports_list> ::= <exports_list> tkComma <exports_entry>
	RULE_EXPORTS_ENTRY                                              =  53, // <exports_entry> ::= <identifier> <exports_index> <exports_name> <exports_resident>
	RULE_EXPORTS_INDEX                                              =  54, // <exports_index> ::= 
	RULE_EXPORTS_INDEX_TKINDEX                                      =  55, // <exports_index> ::= tkIndex <integer_const>
	RULE_EXPORTS_NAME                                               =  56, // <exports_name> ::= 
	RULE_EXPORTS_NAME_TKNAME                                        =  57, // <exports_name> ::= tkName <identifier>
	RULE_EXPORTS_NAME_TKNAME2                                       =  58, // <exports_name> ::= tkName <literal>
	RULE_EXPORTS_RESIDENT                                           =  59, // <exports_resident> ::= 
	RULE_EXPORTS_RESIDENT_TKRESIDENT                                =  60, // <exports_resident> ::= tkResident
	RULE_UNIT_FILE_TKPOINT                                          =  61, // <unit_file> ::= <unit_heading> <interface_part> <implementation_part> <initialization_part> tkPoint
	RULE_UNIT_FILE_TKPOINT2                                         =  62, // <unit_file> ::= <unit_heading> <abc_interface_part> <initialization_part> tkPoint
	RULE_UNIT_HEADING_TKSEMICOLON                                   =  63, // <unit_heading> ::= <unit_key_word> <unit_name> tkSemiColon <opt_head_compiler_directives>
	RULE_UNIT_KEY_WORD_TKUNIT                                       =  64, // <unit_key_word> ::= tkUnit
	RULE_UNIT_KEY_WORD_TKLIBRARY                                    =  65, // <unit_key_word> ::= tkLibrary
	RULE_UNIT_NAME_TKIDENTIFIER                                     =  66, // <unit_name> ::= tkIdentifier
	RULE_INTERFACE_PART_TKINTERFACE                                 =  67, // <interface_part> ::= tkInterface <uses_clause> <using_clause> <int_decl_sect_list>
	RULE_IMPLEMENTATION_PART_TKIMPLEMENTATION                       =  68, // <implementation_part> ::= tkImplementation <uses_clause> <using_clause> <impl_decl_sect_list>
	RULE_ABC_INTERFACE_PART                                         =  69, // <abc_interface_part> ::= <uses_clause> <using_clause> <impl_decl_sect_list>
	RULE_INITIALIZATION_PART_TKEND                                  =  70, // <initialization_part> ::= tkEnd
	RULE_INITIALIZATION_PART_TKINITIALIZATION_TKEND                 =  71, // <initialization_part> ::= tkInitialization <stmt_list> tkEnd
	RULE_INITIALIZATION_PART_TKINITIALIZATION_TKFINALIZATION_TKEND  =  72, // <initialization_part> ::= tkInitialization <stmt_list> tkFinalization <stmt_list> tkEnd
	RULE_INITIALIZATION_PART_TKBEGIN_TKEND                          =  73, // <initialization_part> ::= tkBegin <stmt_list> tkEnd
	RULE_PACKAGE_FILE_TKPACKAGE_TKSEMICOLON_TKEND_TKPOINT           =  74, // <package_file> ::= tkPackage <package_name> tkSemiColon <requires_clause> <contains_clause> tkEnd tkPoint
	RULE_PACKAGE_NAME                                               =  75, // <package_name> ::= <identifier>
	RULE_REQUIRES_CLAUSE                                            =  76, // <requires_clause> ::= 
	RULE_REQUIRES_CLAUSE_TKREQUIRES                                 =  77, // <requires_clause> ::= tkRequires
	RULE_REQUIRES_CLAUSE_TKREQUIRES_TKSEMICOLON                     =  78, // <requires_clause> ::= tkRequires <main_used_units_list> tkSemiColon
	RULE_CONTAINS_CLAUSE                                            =  79, // <contains_clause> ::= 
	RULE_CONTAINS_CLAUSE_TKCONTAINS                                 =  80, // <contains_clause> ::= tkContains
	RULE_CONTAINS_CLAUSE_TKCONTAINS_TKSEMICOLON                     =  81, // <contains_clause> ::= tkContains <main_used_units_list> tkSemiColon
	RULE_INT_DECL_SECT_LIST                                         =  82, // <int_decl_sect_list> ::= <int_decl_sect_list1> <empty>
	RULE_INT_DECL_SECT_LIST1                                        =  83, // <int_decl_sect_list1> ::= <empty> <empty>
	RULE_INT_DECL_SECT_LIST12                                       =  84, // <int_decl_sect_list1> ::= <int_decl_sect_list1> <int_decl_sect>
	RULE_IMPL_DECL_SECT_LIST                                        =  85, // <impl_decl_sect_list> ::= <impl_decl_sect_list1> <empty>
	RULE_IMPL_DECL_SECT_LIST1                                       =  86, // <impl_decl_sect_list1> ::= <empty> <empty>
	RULE_IMPL_DECL_SECT_LIST12                                      =  87, // <impl_decl_sect_list1> ::= <impl_decl_sect_list1> <impl_decl_sect>
	RULE_ABC_DECL_SECT_LIST                                         =  88, // <abc_decl_sect_list> ::= <abc_decl_sect_list1> <empty>
	RULE_ABC_DECL_SECT_LIST1                                        =  89, // <abc_decl_sect_list1> ::= <empty> <empty>
	RULE_ABC_DECL_SECT_LIST12                                       =  90, // <abc_decl_sect_list1> ::= <abc_decl_sect_list1> <abc_decl_sect>
	RULE_INT_DECL_SECT                                              =  91, // <int_decl_sect> ::= <const_decl_sect>
	RULE_INT_DECL_SECT2                                             =  92, // <int_decl_sect> ::= <res_str_decl_sect>
	RULE_INT_DECL_SECT3                                             =  93, // <int_decl_sect> ::= <type_decl_sect>
	RULE_INT_DECL_SECT4                                             =  94, // <int_decl_sect> ::= <var_decl_sect>
	RULE_INT_DECL_SECT5                                             =  95, // <int_decl_sect> ::= <int_proc_heading>
	RULE_INT_DECL_SECT6                                             =  96, // <int_decl_sect> ::= <int_func_heading>
	RULE_IMPL_DECL_SECT                                             =  97, // <impl_decl_sect> ::= <label_decl_sect>
	RULE_IMPL_DECL_SECT2                                            =  98, // <impl_decl_sect> ::= <const_decl_sect>
	RULE_IMPL_DECL_SECT3                                            =  99, // <impl_decl_sect> ::= <res_str_decl_sect>
	RULE_IMPL_DECL_SECT4                                            = 100, // <impl_decl_sect> ::= <type_decl_sect>
	RULE_IMPL_DECL_SECT5                                            = 101, // <impl_decl_sect> ::= <var_decl_sect>
	RULE_IMPL_DECL_SECT6                                            = 102, // <impl_decl_sect> ::= <proc_decl>
	RULE_IMPL_DECL_SECT7                                            = 103, // <impl_decl_sect> ::= <func_decl>
	RULE_IMPL_DECL_SECT8                                            = 104, // <impl_decl_sect> ::= <constructor_decl>
	RULE_IMPL_DECL_SECT9                                            = 105, // <impl_decl_sect> ::= <destructor_decl>
	RULE_ABC_DECL_SECT                                              = 106, // <abc_decl_sect> ::= <label_decl_sect>
	RULE_ABC_DECL_SECT2                                             = 107, // <abc_decl_sect> ::= <const_decl_sect>
	RULE_ABC_DECL_SECT3                                             = 108, // <abc_decl_sect> ::= <res_str_decl_sect>
	RULE_ABC_DECL_SECT4                                             = 109, // <abc_decl_sect> ::= <type_decl_sect>
	RULE_ABC_DECL_SECT5                                             = 110, // <abc_decl_sect> ::= <var_decl_sect>
	RULE_INT_PROC_HEADING                                           = 111, // <int_proc_heading> ::= <proc_heading>
	RULE_INT_PROC_HEADING_TKFORWARD_TKSEMICOLON                     = 112, // <int_proc_heading> ::= <proc_heading> tkForward tkSemiColon
	RULE_INT_FUNC_HEADING                                           = 113, // <int_func_heading> ::= <func_heading>
	RULE_INT_FUNC_HEADING_TKFORWARD_TKSEMICOLON                     = 114, // <int_func_heading> ::= <func_heading> tkForward tkSemiColon
	RULE_LABEL_DECL_SECT_TKLABEL_TKSEMICOLON                        = 115, // <label_decl_sect> ::= tkLabel <label_list> tkSemiColon
	RULE_LABEL_LIST                                                 = 116, // <label_list> ::= <label_name> <empty>
	RULE_LABEL_LIST_TKCOMMA                                         = 117, // <label_list> ::= <label_list> tkComma <label_name>
	RULE_LABEL_NAME_TKINTEGER                                       = 118, // <label_name> ::= tkInteger <empty>
	RULE_LABEL_NAME_TKFLOAT                                         = 119, // <label_name> ::= tkFloat <empty>
	RULE_LABEL_NAME                                                 = 120, // <label_name> ::= <identifier>
	RULE_CONST_DECL_SECT_TKCONST                                    = 121, // <const_decl_sect> ::= tkConst <const_decl>
	RULE_CONST_DECL_SECT                                            = 122, // <const_decl_sect> ::= <const_decl_sect> <const_decl>
	RULE_RES_STR_DECL_SECT_TKRESOURCESTRING                         = 123, // <res_str_decl_sect> ::= tkResourceString <const_decl>
	RULE_RES_STR_DECL_SECT                                          = 124, // <res_str_decl_sect> ::= <res_str_decl_sect> <const_decl>
	RULE_TYPE_DECL_SECT_TKTYPE                                      = 125, // <type_decl_sect> ::= tkType <type_decl>
	RULE_TYPE_DECL_SECT                                             = 126, // <type_decl_sect> ::= <type_decl_sect> <type_decl>
	RULE_VAR_DECL_SECT_TKVAR                                        = 127, // <var_decl_sect> ::= tkVar <var_decl>
	RULE_VAR_DECL_SECT_TKTHREADVAR                                  = 128, // <var_decl_sect> ::= tkThreadvar <var_decl>
	RULE_VAR_DECL_SECT                                              = 129, // <var_decl_sect> ::= <var_decl_sect> <var_decl>
	RULE_CONST_DECL_TKSEMICOLON                                     = 130, // <const_decl> ::= <only_const_decl> tkSemiColon
	RULE_ONLY_CONST_DECL_TKEQUAL                                    = 131, // <only_const_decl> ::= <const_name> tkEqual <init_const_expr>
	RULE_ONLY_CONST_DECL_TKCOLON_TKEQUAL                            = 132, // <only_const_decl> ::= <const_name> tkColon <type_ref> tkEqual <typed_const>
	RULE_INIT_CONST_EXPR                                            = 133, // <init_const_expr> ::= <const_expr>
	RULE_INIT_CONST_EXPR2                                           = 134, // <init_const_expr> ::= <array_const>
	RULE_CONST_NAME                                                 = 135, // <const_name> ::= <identifier>
	RULE_CONST_EXPR                                                 = 136, // <const_expr> ::= <const_simple_expr>
	RULE_CONST_EXPR2                                                = 137, // <const_expr> ::= <const_simple_expr> <const_relop> <const_simple_expr>
	RULE_CONST_RELOP_TKEQUAL                                        = 138, // <const_relop> ::= tkEqual
	RULE_CONST_RELOP_TKNOTEQUAL                                     = 139, // <const_relop> ::= tkNotEqual
	RULE_CONST_RELOP_TKLOWER                                        = 140, // <const_relop> ::= tkLower
	RULE_CONST_RELOP_TKGREATER                                      = 141, // <const_relop> ::= tkGreater
	RULE_CONST_RELOP_TKLOWEREQUAL                                   = 142, // <const_relop> ::= tkLowerEqual
	RULE_CONST_RELOP_TKGREATEREQUAL                                 = 143, // <const_relop> ::= tkGreaterEqual
	RULE_CONST_RELOP_TKIN                                           = 144, // <const_relop> ::= tkIn
	RULE_CONST_SIMPLE_EXPR                                          = 145, // <const_simple_expr> ::= <const_term>
	RULE_CONST_SIMPLE_EXPR2                                         = 146, // <const_simple_expr> ::= <const_simple_expr> <const_addop> <const_term>
	RULE_CONST_ADDOP_TKPLUS                                         = 147, // <const_addop> ::= tkPlus
	RULE_CONST_ADDOP_TKMINUS                                        = 148, // <const_addop> ::= tkMinus
	RULE_CONST_ADDOP_TKOR                                           = 149, // <const_addop> ::= tkOr
	RULE_CONST_ADDOP_TKXOR                                          = 150, // <const_addop> ::= tkXor
	RULE_CONST_TERM                                                 = 151, // <const_term> ::= <const_factor>
	RULE_CONST_TERM2                                                = 152, // <const_term> ::= <const_term> <const_mulop> <const_factor>
	RULE_CONST_MULOP_TKSTAR                                         = 153, // <const_mulop> ::= tkStar
	RULE_CONST_MULOP_TKSLASH                                        = 154, // <const_mulop> ::= tkSlash
	RULE_CONST_MULOP_TKDIV                                          = 155, // <const_mulop> ::= tkDiv
	RULE_CONST_MULOP_TKMOD                                          = 156, // <const_mulop> ::= tkMod
	RULE_CONST_MULOP_TKSHL                                          = 157, // <const_mulop> ::= tkShl
	RULE_CONST_MULOP_TKSHR                                          = 158, // <const_mulop> ::= tkShr
	RULE_CONST_MULOP_TKAND                                          = 159, // <const_mulop> ::= tkAnd
	RULE_CONST_FACTOR                                               = 160, // <const_factor> ::= <const_variable>
	RULE_CONST_FACTOR2                                              = 161, // <const_factor> ::= <const_set>
	RULE_CONST_FACTOR3                                              = 162, // <const_factor> ::= <unsigned_number>
	RULE_CONST_FACTOR4                                              = 163, // <const_factor> ::= <literal>
	RULE_CONST_FACTOR_TKNIL                                         = 164, // <const_factor> ::= tkNil <empty>
	RULE_CONST_FACTOR_TKADDRESSOF                                   = 165, // <const_factor> ::= tkAddressOf <const_factor>
	RULE_CONST_FACTOR_TKROUNDOPEN_TKROUNDCLOSE                      = 166, // <const_factor> ::= tkRoundOpen <const_expr> tkRoundClose
	RULE_CONST_FACTOR_TKNOT                                         = 167, // <const_factor> ::= tkNot <const_factor>
	RULE_CONST_FACTOR5                                              = 168, // <const_factor> ::= <sign> <const_factor>
	RULE_CONST_FACTOR_TKDEREF                                       = 169, // <const_factor> ::= tkDeref <const_factor>
	RULE_CONST_SET_TKSQUAREOPEN_TKSQUARECLOSE                       = 170, // <const_set> ::= tkSquareOpen <const_elem_list> tkSquareClose
	RULE_SIGN_TKPLUS                                                = 171, // <sign> ::= tkPlus
	RULE_SIGN_TKMINUS                                               = 172, // <sign> ::= tkMinus
	RULE_CONST_VARIABLE                                             = 173, // <const_variable> ::= <identifier>
	RULE_CONST_VARIABLE2                                            = 174, // <const_variable> ::= <const_variable> <const_variable_2>
	RULE_CONST_VARIABLE_2_TKPOINT                                   = 175, // <const_variable_2> ::= tkPoint <identifier_or_keyword>
	RULE_CONST_VARIABLE_2_TKDEREF                                   = 176, // <const_variable_2> ::= tkDeref <empty>
	RULE_CONST_VARIABLE_2_TKROUNDOPEN_TKROUNDCLOSE                  = 177, // <const_variable_2> ::= tkRoundOpen <const_func_expr_list> tkRoundClose
	RULE_CONST_VARIABLE_2_TKSQUAREOPEN_TKSQUARECLOSE                = 178, // <const_variable_2> ::= tkSquareOpen <const_elem_list> tkSquareClose
	RULE_CONST_FUNC_EXPR_LIST                                       = 179, // <const_func_expr_list> ::= <const_expr> <empty>
	RULE_CONST_FUNC_EXPR_LIST_TKCOMMA                               = 180, // <const_func_expr_list> ::= <const_func_expr_list> tkComma <const_expr>
	RULE_CONST_ELEM_LIST                                            = 181, // <const_elem_list> ::= <const_elem_list1>
	RULE_CONST_ELEM_LIST2                                           = 182, // <const_elem_list> ::= 
	RULE_CONST_ELEM_LIST1                                           = 183, // <const_elem_list1> ::= <const_elem> <empty>
	RULE_CONST_ELEM_LIST1_TKCOMMA                                   = 184, // <const_elem_list1> ::= <const_elem_list1> tkComma <const_elem>
	RULE_CONST_ELEM                                                 = 185, // <const_elem> ::= <const_expr>
	RULE_CONST_ELEM_TKDOTDOT                                        = 186, // <const_elem> ::= <const_expr> tkDotDot <const_expr>
	RULE_UNSIGNED_NUMBER_TKINTEGER                                  = 187, // <unsigned_number> ::= tkInteger
	RULE_UNSIGNED_NUMBER_TKHEX                                      = 188, // <unsigned_number> ::= tkHex
	RULE_UNSIGNED_NUMBER_TKFLOAT                                    = 189, // <unsigned_number> ::= tkFloat
	RULE_TYPED_CONST                                                = 190, // <typed_const> ::= <const_expr>
	RULE_TYPED_CONST2                                               = 191, // <typed_const> ::= <array_const>
	RULE_TYPED_CONST3                                               = 192, // <typed_const> ::= <record_const>
	RULE_ARRAY_CONST_TKROUNDOPEN_TKROUNDCLOSE                       = 193, // <array_const> ::= tkRoundOpen <typed_const_list> tkRoundClose
	RULE_ARRAY_CONST_TKROUNDOPEN_TKROUNDCLOSE2                      = 194, // <array_const> ::= tkRoundOpen <record_const> tkRoundClose
	RULE_ARRAY_CONST_TKROUNDOPEN_TKROUNDCLOSE3                      = 195, // <array_const> ::= tkRoundOpen <array_const> tkRoundClose
	RULE_TYPED_CONST_LIST                                           = 196, // <typed_const_list> ::= 
	RULE_TYPED_CONST_LIST_TKCOMMA                                   = 197, // <typed_const_list> ::= <typed_const> tkComma <typed_const>
	RULE_TYPED_CONST_LIST_TKCOMMA2                                  = 198, // <typed_const_list> ::= <typed_const_list> tkComma <typed_const>
	RULE_RECORD_CONST_TKROUNDOPEN_TKROUNDCLOSE                      = 199, // <record_const> ::= tkRoundOpen <const_field_list> tkRoundClose
	RULE_CONST_FIELD_LIST                                           = 200, // <const_field_list> ::= <const_field_list_1>
	RULE_CONST_FIELD_LIST_TKSEMICOLON                               = 201, // <const_field_list> ::= <const_field_list_1> tkSemiColon
	RULE_CONST_FIELD_LIST_1                                         = 202, // <const_field_list_1> ::= <const_field> <empty>
	RULE_CONST_FIELD_LIST_1_TKSEMICOLON                             = 203, // <const_field_list_1> ::= <const_field_list_1> tkSemiColon <const_field>
	RULE_CONST_FIELD_TKCOLON                                        = 204, // <const_field> ::= <const_field_name> tkColon <typed_const>
	RULE_CONST_FIELD_NAME                                           = 205, // <const_field_name> ::= <identifier>
	RULE_TYPE_DECL_TKEQUAL_TKSEMICOLON                              = 206, // <type_decl> ::= <identifier> tkEqual <type_decl_type> tkSemiColon
	RULE_TYPE_DECL_TYPE                                             = 207, // <type_decl_type> ::= <type_ref>
	RULE_TYPE_DECL_TYPE_TKTYPE                                      = 208, // <type_decl_type> ::= tkType <type_ref>
	RULE_TYPE_DECL_TYPE2                                            = 209, // <type_decl_type> ::= <object_type>
	RULE_TYPE_REF                                                   = 210, // <type_ref> ::= <simple_type>
	RULE_TYPE_REF2                                                  = 211, // <type_ref> ::= <string_type>
	RULE_TYPE_REF3                                                  = 212, // <type_ref> ::= <pointer_type>
	RULE_TYPE_REF4                                                  = 213, // <type_ref> ::= <structured_type>
	RULE_TYPE_REF5                                                  = 214, // <type_ref> ::= <procedural_type>
	RULE_TYPE_REF6                                                  = 215, // <type_ref> ::= <template_type>
	RULE_TEMPLATE_TYPE                                              = 216, // <template_type> ::= <simple_type_identifier> <template_type_params>
	RULE_TEMPLATE_TYPE_PARAMS_TKLOWER_TKGREATER                     = 217, // <template_type_params> ::= tkLower <template_param_list> tkGreater
	RULE_TEMPLATE_PARAM_LIST                                        = 218, // <template_param_list> ::= <template_param> <empty>
	RULE_TEMPLATE_PARAM_LIST_TKCOMMA                                = 219, // <template_param_list> ::= <template_param_list> tkComma <template_param>
	RULE_TEMPLATE_PARAM                                             = 220, // <template_param> ::= <simple_type_identifier>
	RULE_TEMPLATE_PARAM2                                            = 221, // <template_param> ::= <template_type>
	RULE_SIMPLE_TYPE                                                = 222, // <simple_type> ::= <simple_type_identifier>
	RULE_SIMPLE_TYPE_TKDOTDOT                                       = 223, // <simple_type> ::= <range_expr> tkDotDot <range_expr>
	RULE_SIMPLE_TYPE_TKROUNDOPEN_TKROUNDCLOSE                       = 224, // <simple_type> ::= tkRoundOpen <enumeration_id_list> tkRoundClose
	RULE_RANGE_EXPR                                                 = 225, // <range_expr> ::= <range_term>
	RULE_RANGE_EXPR2                                                = 226, // <range_expr> ::= <range_expr> <const_addop> <range_term>
	RULE_RANGE_TERM                                                 = 227, // <range_term> ::= <range_factor>
	RULE_RANGE_TERM2                                                = 228, // <range_term> ::= <range_term> <const_mulop> <range_factor>
	RULE_RANGE_FACTOR                                               = 229, // <range_factor> ::= <simple_type_identifier> <empty>
	RULE_RANGE_FACTOR2                                              = 230, // <range_factor> ::= <unsigned_number>
	RULE_RANGE_FACTOR3                                              = 231, // <range_factor> ::= <sign> <range_factor>
	RULE_RANGE_FACTOR4                                              = 232, // <range_factor> ::= <literal>
	RULE_RANGE_FACTOR_TKROUNDOPEN_TKROUNDCLOSE                      = 233, // <range_factor> ::= <range_factor> tkRoundOpen <const_elem_list> tkRoundClose
	RULE_RANGE_FACTOR_TKROUNDOPEN_TKROUNDCLOSE2                     = 234, // <range_factor> ::= tkRoundOpen <const_expr> tkRoundClose
	RULE_RANGE_METHODNAME                                           = 235, // <range_methodname> ::= <identifier>
	RULE_RANGE_METHODNAME_TKPOINT                                   = 236, // <range_methodname> ::= <identifier> tkPoint <identifier_or_keyword>
	RULE_SIMPLE_TYPE_IDENTIFIER                                     = 237, // <simple_type_identifier> ::= <identifier> <empty>
	RULE_SIMPLE_TYPE_IDENTIFIER_TKPOINT                             = 238, // <simple_type_identifier> ::= <simple_type_identifier> tkPoint <identifier_or_keyword>
	RULE_ENUMERATION_ID_LIST_TKCOMMA                                = 239, // <enumeration_id_list> ::= <enumeration_id> tkComma <enumeration_id>
	RULE_ENUMERATION_ID_LIST_TKCOMMA2                               = 240, // <enumeration_id_list> ::= <enumeration_id_list> tkComma <enumeration_id>
	RULE_ENUMERATION_ID                                             = 241, // <enumeration_id> ::= <identifier> <empty>
	RULE_POINTER_TYPE_TKDEREF                                       = 242, // <pointer_type> ::= tkDeref <fptype>
	RULE_STRUCTURED_TYPE                                            = 243, // <structured_type> ::= <unpacked_structured_type>
	RULE_STRUCTURED_TYPE_TKPACKED                                   = 244, // <structured_type> ::= tkPacked <unpacked_structured_type>
	RULE_UNPACKED_STRUCTURED_TYPE                                   = 245, // <unpacked_structured_type> ::= <array_type>
	RULE_UNPACKED_STRUCTURED_TYPE2                                  = 246, // <unpacked_structured_type> ::= <new_record_type>
	RULE_UNPACKED_STRUCTURED_TYPE3                                  = 247, // <unpacked_structured_type> ::= <set_type>
	RULE_UNPACKED_STRUCTURED_TYPE4                                  = 248, // <unpacked_structured_type> ::= <file_type>
	RULE_ARRAY_TYPE_TKARRAY_TKSQUAREOPEN_TKSQUARECLOSE_TKOF         = 249, // <array_type> ::= tkArray tkSquareOpen <simple_type_list> tkSquareClose tkOf <type_ref>
	RULE_ARRAY_TYPE                                                 = 250, // <array_type> ::= <unsized_array_type>
	RULE_UNSIZED_ARRAY_TYPE_TKARRAY_TKOF                            = 251, // <unsized_array_type> ::= tkArray tkOf <type_ref>
	RULE_SIMPLE_TYPE_LIST                                           = 252, // <simple_type_list> ::= <simple_type> <empty>
	RULE_SIMPLE_TYPE_LIST_TKCOMMA                                   = 253, // <simple_type_list> ::= <simple_type_list> tkComma <simple_type>
	RULE_RECORD_TYPE_TKRECORD_TKEND                                 = 254, // <record_type> ::= tkRecord <field_list> tkEnd
	RULE_RECORD_TYPE_TKRECORD_TKEND2                                = 255, // <record_type> ::= tkRecord tkEnd
	RULE_FIELD_LIST                                                 = 256, // <field_list> ::= <fixed_part> <empty>
	RULE_FIELD_LIST2                                                = 257, // <field_list> ::= <variant_part> <empty>
	RULE_FIELD_LIST_TKSEMICOLON                                     = 258, // <field_list> ::= <fixed_part_2> tkSemiColon <variant_part>
	RULE_FIXED_PART                                                 = 259, // <fixed_part> ::= <fixed_part_2>
	RULE_FIXED_PART_TKSEMICOLON                                     = 260, // <fixed_part> ::= <fixed_part_2> tkSemiColon
	RULE_FIXED_PART_2                                               = 261, // <fixed_part_2> ::= <record_section> <empty>
	RULE_FIXED_PART_2_TKSEMICOLON                                   = 262, // <fixed_part_2> ::= <fixed_part_2> tkSemiColon <record_section>
	RULE_RECORD_SECTION_TKCOLON                                     = 263, // <record_section> ::= <record_section_id_list> tkColon <type_ref>
	RULE_RECORD_SECTION_ID_LIST                                     = 264, // <record_section_id_list> ::= <record_section_id> <empty>
	RULE_RECORD_SECTION_ID_LIST_TKCOMMA                             = 265, // <record_section_id_list> ::= <record_section_id_list> tkComma <record_section_id>
	RULE_RECORD_SECTION_ID                                          = 266, // <record_section_id> ::= <identifier>
	RULE_VARIANT_PART_TKCASE_TKOF                                   = 267, // <variant_part> ::= tkCase <tag_field> tkOf <variant_list>
	RULE_TAG_FIELD                                                  = 268, // <tag_field> ::= <tag_field_name> <empty>
	RULE_TAG_FIELD_TKCOLON                                          = 269, // <tag_field> ::= <tag_field_name> tkColon <tag_field_typename>
	RULE_TAG_FIELD_NAME                                             = 270, // <tag_field_name> ::= <identifier>
	RULE_TAG_FIELD_TYPENAME                                         = 271, // <tag_field_typename> ::= <fptype>
	RULE_VARIANT_LIST                                               = 272, // <variant_list> ::= <variant_list_2>
	RULE_VARIANT_LIST_TKSEMICOLON                                   = 273, // <variant_list> ::= <variant_list_2> tkSemiColon
	RULE_VARIANT_LIST_2                                             = 274, // <variant_list_2> ::= <variant> <empty>
	RULE_VARIANT_LIST_2_TKSEMICOLON                                 = 275, // <variant_list_2> ::= <variant_list_2> tkSemiColon <variant>
	RULE_VARIANT_TKCOLON_TKROUNDOPEN_TKROUNDCLOSE                   = 276, // <variant> ::= <case_tag_list> tkColon tkRoundOpen <variant_field_list> tkRoundClose
	RULE_VARIANT_FIELD_LIST                                         = 277, // <variant_field_list> ::= 
	RULE_VARIANT_FIELD_LIST2                                        = 278, // <variant_field_list> ::= <field_list>
	RULE_CASE_TAG_LIST                                              = 279, // <case_tag_list> ::= <const_expr_list>
	RULE_CONST_EXPR_LIST                                            = 280, // <const_expr_list> ::= <const_expr> <empty>
	RULE_CONST_EXPR_LIST_TKCOMMA                                    = 281, // <const_expr_list> ::= <const_expr_list> tkComma <const_expr>
	RULE_SET_TYPE_TKSET_TKOF                                        = 282, // <set_type> ::= tkSet tkOf <simple_type>
	RULE_FILE_TYPE_TKFILE_TKOF                                      = 283, // <file_type> ::= tkFile tkOf <type_ref>
	RULE_FILE_TYPE_TKFILE                                           = 284, // <file_type> ::= tkFile <empty>
	RULE_STRING_TYPE_TKIDENTIFIER_TKSQUAREOPEN_TKSQUARECLOSE        = 285, // <string_type> ::= tkIdentifier tkSquareOpen <const_expr> tkSquareClose
	RULE_PROCEDURAL_TYPE                                            = 286, // <procedural_type> ::= <procedural_type_kind>
	RULE_PROCEDURAL_TYPE_KIND                                       = 287, // <procedural_type_kind> ::= <procedural_type_decl>
	RULE_PROCEDURAL_TYPE_KIND_TKOF                                  = 288, // <procedural_type_kind> ::= <procedural_type_decl> tkOf <identifier>
	RULE_PROCEDURAL_TYPE_DECL_TKPROCEDURE                           = 289, // <procedural_type_decl> ::= tkProcedure <fp_list> <maybe_error>
	RULE_PROCEDURAL_TYPE_DECL_TKFUNCTION_TKCOLON                    = 290, // <procedural_type_decl> ::= tkFunction <fp_list> tkColon <fptype>
	RULE_MAYBE_ERROR_TKCOLON                                        = 291, // <maybe_error> ::= tkColon <fptype>
	RULE_MAYBE_ERROR                                                = 292, // <maybe_error> ::= 
	RULE_OBJECT_TYPE                                                = 293, // <object_type> ::= <new_object_type>
	RULE_OOT_PRIVAT_LIST                                            = 294, // <oot_privat_list> ::= 
	RULE_OOT_PRIVAT_LIST_TKPRIVATE                                  = 295, // <oot_privat_list> ::= tkPrivate <oot_component_list>
	RULE_OOT_COMPONENT_LIST                                         = 296, // <oot_component_list> ::= 
	RULE_OOT_COMPONENT_LIST2                                        = 297, // <oot_component_list> ::= <oot_field_list>
	RULE_OOT_COMPONENT_LIST3                                        = 298, // <oot_component_list> ::= <oot_field_list> <oot_method_list>
	RULE_OOT_COMPONENT_LIST4                                        = 299, // <oot_component_list> ::= <oot_method_list>
	RULE_OOT_SUCCESSOR_TKROUNDOPEN_TKROUNDCLOSE                     = 300, // <oot_successor> ::= tkRoundOpen <oot_typeidentifier> tkRoundClose
	RULE_OOT_TYPEIDENTIFIER                                         = 301, // <oot_typeidentifier> ::= <identifier>
	RULE_OOT_FIELD_LIST                                             = 302, // <oot_field_list> ::= <oot_field>
	RULE_OOT_FIELD_LIST2                                            = 303, // <oot_field_list> ::= <oot_field_list> <oot_field>
	RULE_OOT_FIELD_TKCOLON_TKSEMICOLON                              = 304, // <oot_field> ::= <oot_id_list> tkColon <type_ref> tkSemiColon
	RULE_OOT_ID_LIST                                                = 305, // <oot_id_list> ::= <oot_field_identifier>
	RULE_OOT_ID_LIST_TKCOMMA                                        = 306, // <oot_id_list> ::= <oot_id_list> tkComma <oot_field_identifier>
	RULE_OOT_FIELD_IDENTIFIER                                       = 307, // <oot_field_identifier> ::= <identifier>
	RULE_OOT_METHOD_LIST                                            = 308, // <oot_method_list> ::= <oot_method>
	RULE_OOT_METHOD_LIST2                                           = 309, // <oot_method_list> ::= <oot_method_list> <oot_method>
	RULE_OOT_METHOD                                                 = 310, // <oot_method> ::= <oot_method_head>
	RULE_OOT_METHOD_HEAD                                            = 311, // <oot_method_head> ::= <proc_heading>
	RULE_OOT_METHOD_HEAD2                                           = 312, // <oot_method_head> ::= <func_heading>
	RULE_OOT_METHOD_HEAD3                                           = 313, // <oot_method_head> ::= <oot_constructor_head>
	RULE_OOT_METHOD_HEAD4                                           = 314, // <oot_method_head> ::= <oot_destructor_head>
	RULE_OOT_CONSTRUCTOR_HEAD_TKCONSTRUCTOR                         = 315, // <oot_constructor_head> ::= tkConstructor <proc_name> <fp_list> <opt_meth_modificators>
	RULE_OOT_DESTRUCTOR_HEAD_TKDESTRUCTOR                           = 316, // <oot_destructor_head> ::= tkDestructor <proc_name> <fp_list> <opt_meth_modificators>
	RULE_NEW_OBJECT_TYPE                                            = 317, // <new_object_type> ::= <not_object_type>
	RULE_NOT_OBJECT_TYPE                                            = 318, // <not_object_type> ::= <class_attributes> <class_or_interface_keyword> <opt_template_arguments> <opt_base_classes> <opt_where_section> <opt_not_component_list_seq_end>
	RULE_NEW_RECORD_TYPE_TKEND                                      = 319, // <new_record_type> ::= <record_keyword> <opt_template_arguments> <opt_base_classes> <opt_where_section> <not_component_list_seq> tkEnd
	RULE_CLASS_ATTRIBUTES_TKFINAL                                   = 320, // <class_attributes> ::= tkFinal
	RULE_CLASS_ATTRIBUTES                                           = 321, // <class_attributes> ::= 
	RULE_CLASS_OR_INTERFACE_KEYWORD_TKCLASS                         = 322, // <class_or_interface_keyword> ::= tkClass
	RULE_CLASS_OR_INTERFACE_KEYWORD_TKINTERFACE                     = 323, // <class_or_interface_keyword> ::= tkInterface
	RULE_CLASS_OR_INTERFACE_KEYWORD_TKTEMPLATE                      = 324, // <class_or_interface_keyword> ::= tkTemplate
	RULE_CLASS_OR_INTERFACE_KEYWORD_TKTEMPLATE_TKCLASS              = 325, // <class_or_interface_keyword> ::= tkTemplate tkClass
	RULE_CLASS_OR_INTERFACE_KEYWORD_TKTEMPLATE_TKRECORD             = 326, // <class_or_interface_keyword> ::= tkTemplate tkRecord
	RULE_CLASS_OR_INTERFACE_KEYWORD_TKTEMPLATE_TKINTERFACE          = 327, // <class_or_interface_keyword> ::= tkTemplate tkInterface
	RULE_RECORD_KEYWORD_TKRECORD                                    = 328, // <record_keyword> ::= tkRecord
	RULE_OPT_NOT_COMPONENT_LIST_SEQ_END                             = 329, // <opt_not_component_list_seq_end> ::= 
	RULE_OPT_NOT_COMPONENT_LIST_SEQ_END_TKEND                       = 330, // <opt_not_component_list_seq_end> ::= <not_component_list_seq> tkEnd
	RULE_OPT_BASE_CLASSES                                           = 331, // <opt_base_classes> ::= 
	RULE_OPT_BASE_CLASSES_TKROUNDOPEN_TKROUNDCLOSE                  = 332, // <opt_base_classes> ::= tkRoundOpen <base_classes_names_list> tkRoundClose
	RULE_BASE_CLASSES_NAMES_LIST                                    = 333, // <base_classes_names_list> ::= <base_class_name> <empty>
	RULE_BASE_CLASSES_NAMES_LIST_TKCOMMA                            = 334, // <base_classes_names_list> ::= <base_classes_names_list> tkComma <base_class_name>
	RULE_BASE_CLASS_NAME                                            = 335, // <base_class_name> ::= <simple_type_identifier>
	RULE_BASE_CLASS_NAME2                                           = 336, // <base_class_name> ::= <template_type>
	RULE_OPT_TEMPLATE_ARGUMENTS                                     = 337, // <opt_template_arguments> ::= 
	RULE_OPT_TEMPLATE_ARGUMENTS_TKLOWER_TKGREATER                   = 338, // <opt_template_arguments> ::= tkLower <ident_list> tkGreater
	RULE_OPT_WHERE_SECTION                                          = 339, // <opt_where_section> ::= 
	RULE_OPT_WHERE_SECTION2                                         = 340, // <opt_where_section> ::= <where_part_list>
	RULE_WHERE_PART_LIST                                            = 341, // <where_part_list> ::= <where_part> <empty>
	RULE_WHERE_PART_LIST2                                           = 342, // <where_part_list> ::= <where_part_list> <where_part>
	RULE_WHERE_PART_TKWHERE_TKCOLON_TKSEMICOLON                     = 343, // <where_part> ::= tkWhere <ident_list> tkColon <type_ref_and_secific_list> tkSemiColon
	RULE_TYPE_REF_AND_SECIFIC_LIST                                  = 344, // <type_ref_and_secific_list> ::= <type_ref_or_secific> <empty>
	RULE_TYPE_REF_AND_SECIFIC_LIST_TKCOMMA                          = 345, // <type_ref_and_secific_list> ::= <type_ref_and_secific_list> tkComma <type_ref_or_secific>
	RULE_TYPE_REF_OR_SECIFIC                                        = 346, // <type_ref_or_secific> ::= <type_ref>
	RULE_TYPE_REF_OR_SECIFIC_TKCLASS                                = 347, // <type_ref_or_secific> ::= tkClass
	RULE_TYPE_REF_OR_SECIFIC_TKRECORD                               = 348, // <type_ref_or_secific> ::= tkRecord
	RULE_TYPE_REF_OR_SECIFIC_TKCONSTRUCTOR                          = 349, // <type_ref_or_secific> ::= tkConstructor
	RULE_RECORD_COMPONENT_LIST                                      = 350, // <record_component_list> ::= <not_component_list> <empty>
	RULE_NOT_COMPONENT_LIST_SEQ                                     = 351, // <not_component_list_seq> ::= <not_component_list> <empty>
	RULE_NOT_COMPONENT_LIST_SEQ2                                    = 352, // <not_component_list_seq> ::= <not_component_list_seq> <ot_visibility_specifier> <not_component_list>
	RULE_OT_VISIBILITY_SPECIFIER_TKINTERNAL                         = 353, // <ot_visibility_specifier> ::= tkInternal
	RULE_OT_VISIBILITY_SPECIFIER_TKPUBLIC                           = 354, // <ot_visibility_specifier> ::= tkPublic
	RULE_OT_VISIBILITY_SPECIFIER_TKPROTECTED                        = 355, // <ot_visibility_specifier> ::= tkProtected
	RULE_OT_VISIBILITY_SPECIFIER_TKPRIVATE                          = 356, // <ot_visibility_specifier> ::= tkPrivate
	RULE_NOT_OBJECT_TYPE_IDENTIFIER_LIST                            = 357, // <not_object_type_identifier_list> ::= <simple_type_identifier> <empty>
	RULE_NOT_OBJECT_TYPE_IDENTIFIER_LIST_TKCOMMA                    = 358, // <not_object_type_identifier_list> ::= <not_object_type_identifier_list> tkComma <simple_type_identifier>
	RULE_IDENT_LIST                                                 = 359, // <ident_list> ::= <identifier> <empty>
	RULE_IDENT_LIST_TKCOMMA                                         = 360, // <ident_list> ::= <ident_list> tkComma <identifier>
	RULE_NOT_COMPONENT_LIST                                         = 361, // <not_component_list> ::= <not_guid>
	RULE_NOT_COMPONENT_LIST2                                        = 362, // <not_component_list> ::= <not_guid> <not_component_list_1> <opt_semicolon>
	RULE_NOT_COMPONENT_LIST3                                        = 363, // <not_component_list> ::= <not_guid> <not_component_list_2>
	RULE_NOT_COMPONENT_LIST_TKSEMICOLON                             = 364, // <not_component_list> ::= <not_guid> <not_component_list_1> tkSemiColon <not_component_list_2>
	RULE_OPT_SEMICOLON                                              = 365, // <opt_semicolon> ::= 
	RULE_OPT_SEMICOLON_TKSEMICOLON                                  = 366, // <opt_semicolon> ::= tkSemiColon
	RULE_NOT_GUID                                                   = 367, // <not_guid> ::= 
	RULE_NOT_COMPONENT_LIST_1                                       = 368, // <not_component_list_1> ::= <filed_or_const_definition> <empty>
	RULE_NOT_COMPONENT_LIST_1_TKSEMICOLON                           = 369, // <not_component_list_1> ::= <not_component_list_1> tkSemiColon <filed_or_const_definition_or_am>
	RULE_NOT_COMPONENT_LIST_2                                       = 370, // <not_component_list_2> ::= <not_method_definition> <empty>
	RULE_NOT_COMPONENT_LIST_22                                      = 371, // <not_component_list_2> ::= <not_property_definition> <empty>
	RULE_NOT_COMPONENT_LIST_23                                      = 372, // <not_component_list_2> ::= <not_component_list_2> <not_method_definition>
	RULE_NOT_COMPONENT_LIST_24                                      = 373, // <not_component_list_2> ::= <not_component_list_2> <not_property_definition>
	RULE_FILED_OR_CONST_DEFINITION_TKCONST                          = 374, // <filed_or_const_definition> ::= tkConst <only_const_decl>
	RULE_FILED_OR_CONST_DEFINITION                                  = 375, // <filed_or_const_definition> ::= <not_field_definition>
	RULE_FILED_OR_CONST_DEFINITION_OR_AM                            = 376, // <filed_or_const_definition_or_am> ::= <filed_or_const_definition>
	RULE_FILED_OR_CONST_DEFINITION_OR_AM2                           = 377, // <filed_or_const_definition_or_am> ::= <field_access_modifier>
	RULE_NOT_FIELD_DEFINITION                                       = 378, // <not_field_definition> ::= <var_decl_part>
	RULE_NOT_FIELD_DEFINITION_TKEVENT_TKCOLON                       = 379, // <not_field_definition> ::= tkEvent <var_name_list> tkColon <type_ref>
	RULE_FIELD_ACCESS_MODIFIER_TKSTATIC                             = 380, // <field_access_modifier> ::= tkStatic
	RULE_NOT_METHOD_DEFINITION                                      = 381, // <not_method_definition> ::= <not_method_heading>
	RULE_NOT_METHOD_DEFINITION2                                     = 382, // <not_method_definition> ::= <abc_method_decl>
	RULE_ABC_METHOD_DECL                                            = 383, // <abc_method_decl> ::= <abc_proc_decl>
	RULE_ABC_METHOD_DECL2                                           = 384, // <abc_method_decl> ::= <abc_func_decl>
	RULE_ABC_METHOD_DECL3                                           = 385, // <abc_method_decl> ::= <abc_constructor_decl>
	RULE_ABC_METHOD_DECL4                                           = 386, // <abc_method_decl> ::= <abc_destructor_decl>
	RULE_NOT_METHOD_HEADING_TKCLASS                                 = 387, // <not_method_heading> ::= tkClass <proc_heading>
	RULE_NOT_METHOD_HEADING_TKCLASS2                                = 388, // <not_method_heading> ::= tkClass <func_heading>
	RULE_NOT_METHOD_HEADING                                         = 389, // <not_method_heading> ::= <func_heading>
	RULE_NOT_METHOD_HEADING2                                        = 390, // <not_method_heading> ::= <proc_heading>
	RULE_NOT_METHOD_HEADING3                                        = 391, // <not_method_heading> ::= <not_constructor_heading>
	RULE_NOT_METHOD_HEADING4                                        = 392, // <not_method_heading> ::= <not_destructor_heading>
	RULE_OPTIONAL_QUALIFIED_IDENTIFIER                              = 393, // <optional_qualified_identifier> ::= <qualified_identifier>
	RULE_OPTIONAL_QUALIFIED_IDENTIFIER2                             = 394, // <optional_qualified_identifier> ::= 
	RULE_NOT_CONSTRUCTOR_HEADING_TKCONSTRUCTOR                      = 395, // <not_constructor_heading> ::= tkConstructor <optional_qualified_identifier> <fp_list> <opt_meth_modificators>
	RULE_NOT_DESTRUCTOR_HEADING_TKDESTRUCTOR                        = 396, // <not_destructor_heading> ::= tkDestructor <optional_qualified_identifier> <fp_list> <opt_meth_modificators>
	RULE_QUALIFIED_IDENTIFIER                                       = 397, // <qualified_identifier> ::= <identifier> <empty>
	RULE_QUALIFIED_IDENTIFIER2                                      = 398, // <qualified_identifier> ::= <visibility_specifier> <empty>
	RULE_QUALIFIED_IDENTIFIER_TKPOINT                               = 399, // <qualified_identifier> ::= <qualified_identifier> tkPoint <identifier>
	RULE_QUALIFIED_IDENTIFIER_TKPOINT2                              = 400, // <qualified_identifier> ::= <qualified_identifier> tkPoint <visibility_specifier>
	RULE_NOT_PROPERTY_DEFINITION_TKPROPERTY_TKSEMICOLON             = 401, // <not_property_definition> ::= tkProperty <qualified_identifier> <not_property_interface> <not_property_specifiers> tkSemiColon <not_array_defaultproperty>
	RULE_NOT_ARRAY_DEFAULTPROPERTY                                  = 402, // <not_array_defaultproperty> ::= 
	RULE_NOT_ARRAY_DEFAULTPROPERTY_TKDEFAULT_TKSEMICOLON            = 403, // <not_array_defaultproperty> ::= tkDefault tkSemiColon
	RULE_NOT_PROPERTY_INTERFACE                                     = 404, // <not_property_interface> ::= 
	RULE_NOT_PROPERTY_INTERFACE_TKCOLON                             = 405, // <not_property_interface> ::= <not_property_parameter_list> tkColon <fptype> <not_property_interface_index>
	RULE_NOT_PROPERTY_INTERFACE_INDEX                               = 406, // <not_property_interface_index> ::= 
	RULE_NOT_PROPERTY_INTERFACE_INDEX_TKINDEX                       = 407, // <not_property_interface_index> ::= tkIndex <expr>
	RULE_NOT_PROPERTY_PARAMETER_LIST                                = 408, // <not_property_parameter_list> ::= 
	RULE_NOT_PROPERTY_PARAMETER_LIST_TKSQUAREOPEN_TKSQUARECLOSE     = 409, // <not_property_parameter_list> ::= tkSquareOpen <not_parameter_decl_list> tkSquareClose
	RULE_NOT_PARAMETER_DECL_LIST                                    = 410, // <not_parameter_decl_list> ::= <not_parameter_decl> <empty>
	RULE_NOT_PARAMETER_DECL_LIST_TKSEMICOLON                        = 411, // <not_parameter_decl_list> ::= <not_parameter_decl_list> tkSemiColon <not_parameter_decl>
	RULE_NOT_PARAMETER_DECL_TKCOLON                                 = 412, // <not_parameter_decl> ::= <not_parameter_name_list> tkColon <fptype>
	RULE_NOT_PARAMETER_DECL_TKCONST_TKCOLON                         = 413, // <not_parameter_decl> ::= tkConst <not_parameter_name_list> tkColon <fptype>
	RULE_NOT_PARAMETER_DECL_TKVAR_TKCOLON                           = 414, // <not_parameter_decl> ::= tkVar <not_parameter_name_list> tkColon <fptype>
	RULE_NOT_PARAMETER_DECL_TKOUT_TKCOLON                           = 415, // <not_parameter_decl> ::= tkOut <not_parameter_name_list> tkColon <fptype>
	RULE_NOT_PARAMETER_NAME_LIST                                    = 416, // <not_parameter_name_list> ::= <ident_list>
	RULE_OPT_IDENTIFIER                                             = 417, // <opt_identifier> ::= <identifier>
	RULE_OPT_IDENTIFIER2                                            = 418, // <opt_identifier> ::= 
	RULE_NOT_PROPERTY_SPECIFIERS                                    = 419, // <not_property_specifiers> ::= 
	RULE_NOT_PROPERTY_SPECIFIERS_TKREADONLY                         = 420, // <not_property_specifiers> ::= tkReadOnly <not_property_specifiers>
	RULE_NOT_PROPERTY_SPECIFIERS_TKWRITEONLY                        = 421, // <not_property_specifiers> ::= tkWriteOnly <not_property_specifiers>
	RULE_NOT_PROPERTY_SPECIFIERS_TKDEFAULT                          = 422, // <not_property_specifiers> ::= tkDefault <const_expr> <not_property_specifiers>
	RULE_NOT_PROPERTY_SPECIFIERS_TKREAD                             = 423, // <not_property_specifiers> ::= tkRead <opt_identifier> <not_property_specifiers>
	RULE_NOT_PROPERTY_SPECIFIERS_TKWRITE                            = 424, // <not_property_specifiers> ::= tkWrite <opt_identifier> <not_property_specifiers>
	RULE_VAR_DECL_TKSEMICOLON                                       = 425, // <var_decl> ::= <var_decl_part> tkSemiColon
	RULE_VAR_DECL_PART                                              = 426, // <var_decl_part> ::= <var_decl_part_normal>
	RULE_VAR_DECL_PART2                                             = 427, // <var_decl_part> ::= <var_decl_part_assign>
	RULE_VAR_DECL_PART_TKCOLON_TKASSIGN                             = 428, // <var_decl_part> ::= <var_name_list> tkColon <type_ref> tkAssign <var_init_value_typed>
	RULE_VAR_DECL_PART_IN_STMT                                      = 429, // <var_decl_part_in_stmt> ::= <var_decl_part>
	RULE_VAR_DECL_PART_ASSIGN_TKASSIGN                              = 430, // <var_decl_part_assign> ::= <var_name_list> tkAssign <var_init_value>
	RULE_VAR_DECL_PART_NORMAL_TKCOLON                               = 431, // <var_decl_part_normal> ::= <var_name_list> tkColon <type_ref>
	RULE_VAR_INIT_VALUE                                             = 432, // <var_init_value> ::= <expr>
	RULE_VAR_INIT_VALUE_TYPED                                       = 433, // <var_init_value_typed> ::= <typed_const>
	RULE_VAR_INIT_VALUE_TYPED2                                      = 434, // <var_init_value_typed> ::= <new_expr>
	RULE_VAR_NAME_LIST                                              = 435, // <var_name_list> ::= <var_name> <empty>
	RULE_VAR_NAME_LIST_TKCOMMA                                      = 436, // <var_name_list> ::= <var_name_list> tkComma <var_name>
	RULE_VAR_NAME                                                   = 437, // <var_name> ::= <identifier>
	RULE_DECLARED_VAR_NAME                                          = 438, // <declared_var_name> ::= <identifier>
	RULE_CONSTRUCTOR_DECL                                           = 439, // <constructor_decl> ::= <not_constructor_heading> <not_constructor_block_decl>
	RULE_ABC_CONSTRUCTOR_DECL                                       = 440, // <abc_constructor_decl> ::= <not_constructor_heading> <abc_block>
	RULE_DESTRUCTOR_DECL                                            = 441, // <destructor_decl> ::= <not_destructor_heading> <not_constructor_block_decl>
	RULE_ABC_DESTRUCTOR_DECL                                        = 442, // <abc_destructor_decl> ::= <not_destructor_heading> <abc_block>
	RULE_NOT_CONSTRUCTOR_BLOCK_DECL                                 = 443, // <not_constructor_block_decl> ::= <block>
	RULE_NOT_CONSTRUCTOR_BLOCK_DECL2                                = 444, // <not_constructor_block_decl> ::= <external_directr>
	RULE_NOT_CONSTRUCTOR_BLOCK_DECL3                                = 445, // <not_constructor_block_decl> ::= <asm_block>
	RULE_PROC_DECL                                                  = 446, // <proc_decl> ::= <proc_decl_noclass>
	RULE_PROC_DECL_TKCLASS                                          = 447, // <proc_decl> ::= tkClass <proc_decl_noclass>
	RULE_PROC_DECL_NOCLASS                                          = 448, // <proc_decl_noclass> ::= <proc_heading> <proc_block>
	RULE_ABC_PROC_DECL                                              = 449, // <abc_proc_decl> ::= <abc_proc_decl_noclass>
	RULE_ABC_PROC_DECL_TKCLASS                                      = 450, // <abc_proc_decl> ::= tkClass <abc_proc_decl_noclass>
	RULE_ABC_PROC_DECL_NOCLASS                                      = 451, // <abc_proc_decl_noclass> ::= <proc_heading> <abc_proc_block>
	RULE_FUNC_DECL                                                  = 452, // <func_decl> ::= <func_decl_noclass>
	RULE_FUNC_DECL_TKCLASS                                          = 453, // <func_decl> ::= tkClass <func_decl_noclass>
	RULE_FUNC_DECL_NOCLASS                                          = 454, // <func_decl_noclass> ::= <func_heading> <func_block>
	RULE_ABC_FUNC_DECL                                              = 455, // <abc_func_decl> ::= <abc_func_decl_noclass>
	RULE_ABC_FUNC_DECL_TKCLASS                                      = 456, // <abc_func_decl> ::= tkClass <abc_func_decl_noclass>
	RULE_ABC_FUNC_DECL_NOCLASS                                      = 457, // <abc_func_decl_noclass> ::= <func_heading> <abc_proc_block>
	RULE_PROC_HEADING_TKPROCEDURE                                   = 458, // <proc_heading> ::= tkProcedure <proc_name> <opt_template_arguments> <fp_list> <maybe_error> <opt_meth_modificators> <opt_where_section>
	RULE_PROC_NAME                                                  = 459, // <proc_name> ::= <func_name>
	RULE_FUNC_NAME                                                  = 460, // <func_name> ::= <func_meth_name_ident> <empty>
	RULE_FUNC_NAME_TKPOINT                                          = 461, // <func_name> ::= <func_class_name_ident> tkPoint <func_meth_name_ident>
	RULE_FUNC_METH_NAME_IDENT                                       = 462, // <func_meth_name_ident> ::= <identifier>
	RULE_FUNC_METH_NAME_IDENT2                                      = 463, // <func_meth_name_ident> ::= <visibility_specifier>
	RULE_FUNC_METH_NAME_IDENT3                                      = 464, // <func_meth_name_ident> ::= <operator_name_ident>
	RULE_FUNC_CLASS_NAME_IDENT                                      = 465, // <func_class_name_ident> ::= <identifier>
	RULE_FUNC_CLASS_NAME_IDENT2                                     = 466, // <func_class_name_ident> ::= <visibility_specifier>
	RULE_FUNC_HEADING_TKFUNCTION_TKCOLON                            = 467, // <func_heading> ::= tkFunction <func_name> <opt_template_arguments> <fp_list> tkColon <fptype> <opt_meth_modificators> <opt_where_section>
	RULE_FUNC_HEADING_TKFUNCTION                                    = 468, // <func_heading> ::= tkFunction <func_name> <opt_meth_modificators>
	RULE_PROC_BLOCK                                                 = 469, // <proc_block> ::= <proc_block_decl>
	RULE_FUNC_BLOCK                                                 = 470, // <func_block> ::= <proc_block_decl>
	RULE_PROC_BLOCK_DECL                                            = 471, // <proc_block_decl> ::= <block>
	RULE_PROC_BLOCK_DECL2                                           = 472, // <proc_block_decl> ::= <external_directr>
	RULE_PROC_BLOCK_DECL3                                           = 473, // <proc_block_decl> ::= <asm_block>
	RULE_PROC_BLOCK_DECL_TKFORWARD_TKSEMICOLON                      = 474, // <proc_block_decl> ::= tkForward tkSemiColon
	RULE_ABC_PROC_BLOCK                                             = 475, // <abc_proc_block> ::= <abc_block>
	RULE_ABC_PROC_BLOCK2                                            = 476, // <abc_proc_block> ::= <external_directr>
	RULE_EXTERNAL_DIRECTR                                           = 477, // <external_directr> ::= <abc_external_directr>
	RULE_EXTERNAL_DIRECTR_TKSEMICOLON                               = 478, // <external_directr> ::= <abc_external_directr> tkSemiColon
	RULE_EXTERNAL_DIRECTR_IDENT                                     = 479, // <external_directr_ident> ::= <identifier>
	RULE_EXTERNAL_DIRECTR_IDENT2                                    = 480, // <external_directr_ident> ::= <literal>
	RULE_ABC_EXTERNAL_DIRECTR_TKEXTERNAL_TKNAME                     = 481, // <abc_external_directr> ::= tkExternal <external_directr_ident> tkName <external_directr_ident>
	RULE_ASM_BLOCK_TKASMBODY_TKSEMICOLON                            = 482, // <asm_block> ::= <impl_decl_sect_list> tkAsmBody tkSemiColon
	RULE_BF_BLOCK_TKBF_TKEND_TKSEMICOLON                            = 483, // <bf_block> ::= tkBF <bf_instructions> tkEnd tkSemiColon
	RULE_BF_EMPTY_INSTRUCTION                                       = 484, // <bf_empty_instruction> ::= 
	RULE_BF_INSTRUCTIONS                                            = 485, // <bf_instructions> ::= <bf_instructions_list>
	RULE_BF_INSTRUCTIONS2                                           = 486, // <bf_instructions> ::= <bf_empty_instruction> <empty>
	RULE_BF_INSTRUCTIONS_LIST                                       = 487, // <bf_instructions_list> ::= <bf_instruction> <empty>
	RULE_BF_INSTRUCTIONS_LIST2                                      = 488, // <bf_instructions_list> ::= <bf_instructions_list> <bf_instruction>
	RULE_BF_INSTRUCTION_TKGREATER                                   = 489, // <bf_instruction> ::= tkGreater <empty>
	RULE_BF_INSTRUCTION_TKLOWER                                     = 490, // <bf_instruction> ::= tkLower <empty>
	RULE_BF_INSTRUCTION_TKPLUS                                      = 491, // <bf_instruction> ::= tkPlus <empty>
	RULE_BF_INSTRUCTION_TKMINUS                                     = 492, // <bf_instruction> ::= tkMinus <empty>
	RULE_BF_INSTRUCTION_TKPOINT                                     = 493, // <bf_instruction> ::= tkPoint <empty>
	RULE_BF_INSTRUCTION_TKCOMMA                                     = 494, // <bf_instruction> ::= tkComma <empty>
	RULE_BF_INSTRUCTION_TKSQUAREOPEN_TKSQUARECLOSE                  = 495, // <bf_instruction> ::= tkSquareOpen <bf_instructions> tkSquareClose
	RULE_BF_INSTRUCTION_TKDOTDOT                                    = 496, // <bf_instruction> ::= tkDotDot <empty>
	RULE_BLOCK_TKSEMICOLON                                          = 497, // <block> ::= <impl_decl_sect_list> <compound_stmt> tkSemiColon
	RULE_ABC_BLOCK_TKSEMICOLON                                      = 498, // <abc_block> ::= <abc_decl_sect_list> <compound_stmt> tkSemiColon
	RULE_FP_LIST                                                    = 499, // <fp_list> ::= 
	RULE_FP_LIST_TKROUNDOPEN_TKROUNDCLOSE                           = 500, // <fp_list> ::= tkRoundOpen <fp_sect_list> tkRoundClose
	RULE_FP_SECT_LIST                                               = 501, // <fp_sect_list> ::= 
	RULE_FP_SECT_LIST2                                              = 502, // <fp_sect_list> ::= <fp_sect> <empty>
	RULE_FP_SECT_LIST_TKSEMICOLON                                   = 503, // <fp_sect_list> ::= <fp_sect_list> tkSemiColon <fp_sect>
	RULE_FP_SECT_TKCOLON                                            = 504, // <fp_sect> ::= <param_name_list> tkColon <fptype_new>
	RULE_FP_SECT                                                    = 505, // <fp_sect> ::= <param_name_list> <empty>
	RULE_FP_SECT_TKVAR_TKCOLON                                      = 506, // <fp_sect> ::= tkVar <param_name_list> tkColon <fptype_new>
	RULE_FP_SECT_TKVAR                                              = 507, // <fp_sect> ::= tkVar <param_name_list> <empty>
	RULE_FP_SECT_TKOUT_TKCOLON                                      = 508, // <fp_sect> ::= tkOut <param_name_list> tkColon <fptype_new>
	RULE_FP_SECT_TKOUT                                              = 509, // <fp_sect> ::= tkOut <param_name_list> <empty>
	RULE_FP_SECT_TKCONST_TKCOLON                                    = 510, // <fp_sect> ::= tkConst <param_name_list> tkColon <fptype_new>
	RULE_FP_SECT_TKCONST                                            = 511, // <fp_sect> ::= tkConst <param_name_list> <empty>
	RULE_FP_SECT_TKPARAMS_TKCOLON                                   = 512, // <fp_sect> ::= tkParams <param_name_list> tkColon <fptype_new>
	RULE_FP_SECT_TKPARAMS                                           = 513, // <fp_sect> ::= tkParams <param_name_list> <empty>
	RULE_FP_SECT_TKCOLON_TKASSIGN                                   = 514, // <fp_sect> ::= <param_name_list> tkColon <fptype> tkAssign <const_expr>
	RULE_FP_SECT_TKVAR_TKCOLON_TKASSIGN                             = 515, // <fp_sect> ::= tkVar <param_name_list> tkColon <fptype> tkAssign <const_expr>
	RULE_FP_SECT_TKOUT_TKCOLON_TKASSIGN                             = 516, // <fp_sect> ::= tkOut <param_name_list> tkColon <fptype> tkAssign <const_expr>
	RULE_FP_SECT_TKCONST_TKCOLON_TKASSIGN                           = 517, // <fp_sect> ::= tkConst <param_name_list> tkColon <fptype> tkAssign <const_expr>
	RULE_PARAM_NAME_LIST                                            = 518, // <param_name_list> ::= <param_name> <empty>
	RULE_PARAM_NAME_LIST_TKCOMMA                                    = 519, // <param_name_list> ::= <param_name_list> tkComma <param_name>
	RULE_PARAM_NAME                                                 = 520, // <param_name> ::= <identifier>
	RULE_FPTYPE                                                     = 521, // <fptype> ::= <type_ref>
	RULE_FPTYPE_NEW                                                 = 522, // <fptype_new> ::= <type_ref>
	RULE_FPTYPE_NEW_TKARRAY_TKOF_TKCONST                            = 523, // <fptype_new> ::= tkArray tkOf tkConst
	RULE_STMT                                                       = 524, // <stmt> ::= <unlabelled_stmt>
	RULE_STMT_TKCOLON                                               = 525, // <stmt> ::= <label_name> tkColon <unlabelled_stmt>
	RULE_UNLABELLED_STMT                                            = 526, // <unlabelled_stmt> ::= <empty> <empty>
	RULE_UNLABELLED_STMT2                                           = 527, // <unlabelled_stmt> ::= <assignment>
	RULE_UNLABELLED_STMT3                                           = 528, // <unlabelled_stmt> ::= <proc_call>
	RULE_UNLABELLED_STMT4                                           = 529, // <unlabelled_stmt> ::= <goto_stmt>
	RULE_UNLABELLED_STMT5                                           = 530, // <unlabelled_stmt> ::= <compound_stmt>
	RULE_UNLABELLED_STMT6                                           = 531, // <unlabelled_stmt> ::= <if_stmt>
	RULE_UNLABELLED_STMT7                                           = 532, // <unlabelled_stmt> ::= <case_stmt>
	RULE_UNLABELLED_STMT8                                           = 533, // <unlabelled_stmt> ::= <repeat_stmt>
	RULE_UNLABELLED_STMT9                                           = 534, // <unlabelled_stmt> ::= <while_stmt>
	RULE_UNLABELLED_STMT10                                          = 535, // <unlabelled_stmt> ::= <for_stmt>
	RULE_UNLABELLED_STMT11                                          = 536, // <unlabelled_stmt> ::= <with_stmt>
	RULE_UNLABELLED_STMT12                                          = 537, // <unlabelled_stmt> ::= <asm_stmt>
	RULE_UNLABELLED_STMT13                                          = 538, // <unlabelled_stmt> ::= <inherited_message>
	RULE_UNLABELLED_STMT14                                          = 539, // <unlabelled_stmt> ::= <try_stmt>
	RULE_UNLABELLED_STMT15                                          = 540, // <unlabelled_stmt> ::= <raise_stmt>
	RULE_UNLABELLED_STMT16                                          = 541, // <unlabelled_stmt> ::= <foreach_stmt>
	RULE_UNLABELLED_STMT17                                          = 542, // <unlabelled_stmt> ::= <var_stmt>
	RULE_UNLABELLED_STMT18                                          = 543, // <unlabelled_stmt> ::= <expr_as_stmt>
	RULE_UNLABELLED_STMT19                                          = 544, // <unlabelled_stmt> ::= <lock_stmt>
	RULE_VAR_STMT_TKVAR                                             = 545, // <var_stmt> ::= tkVar <var_decl_part_in_stmt>
	RULE_ASSIGNMENT                                                 = 546, // <assignment> ::= <var_reference> <assign_operator> <expr>
	RULE_PROC_CALL                                                  = 547, // <proc_call> ::= <var_reference> <empty>
	RULE_GOTO_STMT_TKGOTO                                           = 548, // <goto_stmt> ::= tkGoto <label_name>
	RULE_COMPOUND_STMT_TKBEGIN_TKEND                                = 549, // <compound_stmt> ::= tkBegin <stmt_list> tkEnd
	RULE_COMPOUND_STMT_TKILCODE                                     = 550, // <compound_stmt> ::= tkILCode
	RULE_STMT_LIST                                                  = 551, // <stmt_list> ::= <stmt> <empty>
	RULE_STMT_LIST_TKSEMICOLON                                      = 552, // <stmt_list> ::= <stmt_list> tkSemiColon <stmt>
	RULE_IF_STMT_TKIF                                               = 553, // <if_stmt> ::= tkIf <expr> <if_then_else_branch>
	RULE_IF_THEN_ELSE_BRANCH_TKTHEN                                 = 554, // <if_then_else_branch> ::= tkThen <then_branch>
	RULE_IF_THEN_ELSE_BRANCH_TKTHEN_TKELSE                          = 555, // <if_then_else_branch> ::= tkThen <then_branch> tkElse <else_branch>
	RULE_THEN_BRANCH                                                = 556, // <then_branch> ::= <stmt>
	RULE_ELSE_BRANCH                                                = 557, // <else_branch> ::= <stmt>
	RULE_CASE_STMT_TKCASE_TKOF_TKEND                                = 558, // <case_stmt> ::= tkCase <expr> tkOf <case_list> <else_case> tkEnd
	RULE_CASE_LIST                                                  = 559, // <case_list> ::= <case_item> <empty>
	RULE_CASE_LIST_TKSEMICOLON                                      = 560, // <case_list> ::= <case_list> tkSemiColon <case_item>
	RULE_CASE_ITEM                                                  = 561, // <case_item> ::= <empty> <empty>
	RULE_CASE_ITEM_TKCOLON                                          = 562, // <case_item> ::= <case_label_list> tkColon <stmt>
	RULE_CASE_LABEL_LIST                                            = 563, // <case_label_list> ::= <case_label> <empty>
	RULE_CASE_LABEL_LIST_TKCOMMA                                    = 564, // <case_label_list> ::= <case_label_list> tkComma <case_label>
	RULE_CASE_LABEL                                                 = 565, // <case_label> ::= <const_elem>
	RULE_ELSE_CASE                                                  = 566, // <else_case> ::= 
	RULE_ELSE_CASE_TKELSE                                           = 567, // <else_case> ::= tkElse <stmt_list>
	RULE_REPEAT_STMT_TKREPEAT_TKUNTIL                               = 568, // <repeat_stmt> ::= tkRepeat <stmt_list> tkUntil <expr>
	RULE_WHILE_STMT_TKWHILE_TKDO                                    = 569, // <while_stmt> ::= tkWhile <expr> tkDo <stmt>
	RULE_LOCK_STMT_TKLOCK_TKDO                                      = 570, // <lock_stmt> ::= tkLock <expr> tkDo <stmt>
	RULE_FOREACH_STMT_TKFOREACH_TKIN_TKDO                           = 571, // <foreach_stmt> ::= tkForeach <identifier> <foreach_stmt_ident_dype_opt> tkIn <expr> tkDo <stmt>
	RULE_FOREACH_STMT_IDENT_DYPE_OPT_TKCOLON                        = 572, // <foreach_stmt_ident_dype_opt> ::= tkColon <type_ref>
	RULE_FOREACH_STMT_IDENT_DYPE_OPT                                = 573, // <foreach_stmt_ident_dype_opt> ::= 
	RULE_FOR_STMT_TKFOR_TKDO                                        = 574, // <for_stmt> ::= tkFor <opt_var> <identifier> <for_stmt_decl_or_assign> <expr> <for_cycle_type> <expr> tkDo <stmt>
	RULE_OPT_VAR_TKVAR                                              = 575, // <opt_var> ::= tkVar
	RULE_OPT_VAR                                                    = 576, // <opt_var> ::= 
	RULE_FOR_STMT_DECL_OR_ASSIGN_TKASSIGN                           = 577, // <for_stmt_decl_or_assign> ::= tkAssign
	RULE_FOR_STMT_DECL_OR_ASSIGN_TKCOLON_TKASSIGN                   = 578, // <for_stmt_decl_or_assign> ::= tkColon <simple_type_identifier> tkAssign
	RULE_FOR_CYCLE_TYPE_TKTO                                        = 579, // <for_cycle_type> ::= tkTo
	RULE_FOR_CYCLE_TYPE_TKDOWNTO                                    = 580, // <for_cycle_type> ::= tkDownto
	RULE_WITH_STMT_TKWITH_TKDO                                      = 581, // <with_stmt> ::= tkWith <expr_list> tkDo <stmt>
	RULE_INHERITED_MESSAGE_TKINHERITED                              = 582, // <inherited_message> ::= tkInherited <empty>
	RULE_TRY_STMT_TKTRY                                             = 583, // <try_stmt> ::= tkTry <stmt_list> <try_handler>
	RULE_TRY_HANDLER_TKFINALLY_TKEND                                = 584, // <try_handler> ::= tkFinally <stmt_list> tkEnd
	RULE_TRY_HANDLER_TKEXCEPT_TKEND                                 = 585, // <try_handler> ::= tkExcept <exception_block> tkEnd
	RULE_EXCEPTION_BLOCK                                            = 586, // <exception_block> ::= <exception_handler_list> <exception_block_else_branch>
	RULE_EXCEPTION_BLOCK_TKSEMICOLON                                = 587, // <exception_block> ::= <exception_handler_list> tkSemiColon <exception_block_else_branch>
	RULE_EXCEPTION_BLOCK2                                           = 588, // <exception_block> ::= <stmt_list> <empty>
	RULE_EXCEPTION_HANDLER_LIST                                     = 589, // <exception_handler_list> ::= <exception_handler> <empty>
	RULE_EXCEPTION_HANDLER_LIST_TKSEMICOLON                         = 590, // <exception_handler_list> ::= <exception_handler_list> tkSemiColon <exception_handler>
	RULE_EXCEPTION_BLOCK_ELSE_BRANCH                                = 591, // <exception_block_else_branch> ::= 
	RULE_EXCEPTION_BLOCK_ELSE_BRANCH_TKELSE                         = 592, // <exception_block_else_branch> ::= tkElse <stmt_list>
	RULE_EXCEPTION_HANDLER_TKON_TKDO                                = 593, // <exception_handler> ::= tkOn <exception_identifier> tkDo <stmt>
	RULE_EXCEPTION_IDENTIFIER                                       = 594, // <exception_identifier> ::= <exception_class_type_identifier> <empty>
	RULE_EXCEPTION_IDENTIFIER_TKCOLON                               = 595, // <exception_identifier> ::= <exception_variable> tkColon <exception_class_type_identifier>
	RULE_EXCEPTION_CLASS_TYPE_IDENTIFIER                            = 596, // <exception_class_type_identifier> ::= <simple_type_identifier>
	RULE_EXCEPTION_VARIABLE                                         = 597, // <exception_variable> ::= <identifier>
	RULE_RAISE_STMT_TKRAISE                                         = 598, // <raise_stmt> ::= tkRaise <empty>
	RULE_RAISE_STMT_TKRAISE2                                        = 599, // <raise_stmt> ::= tkRaise <expr>
	RULE_RAISE_STMT_TKRAISE_TKAT                                    = 600, // <raise_stmt> ::= tkRaise <expr> tkAt <expr>
	RULE_ASM_STMT_TKASMBODY                                         = 601, // <asm_stmt> ::= tkAsmBody
	RULE_EXPR_LIST                                                  = 602, // <expr_list> ::= <expr> <empty>
	RULE_EXPR_LIST_TKCOMMA                                          = 603, // <expr_list> ::= <expr_list> tkComma <expr>
	RULE_EXPR_AS_STMT                                               = 604, // <expr_as_stmt> ::= <allowable_expr_as_stmt> <empty>
	RULE_ALLOWABLE_EXPR_AS_STMT                                     = 605, // <allowable_expr_as_stmt> ::= <new_expr>
	RULE_EXPR                                                       = 606, // <expr> ::= <expr_l1>
	RULE_EXPR2                                                      = 607, // <expr> ::= <format_expr>
	RULE_EXPR_L1                                                    = 608, // <expr_l1> ::= <relop_expr>
	RULE_EXPR_L12                                                   = 609, // <expr_l1> ::= <question_expr>
	RULE_EXPR_L13                                                   = 610, // <expr_l1> ::= <new_expr>
	RULE_SIZEOF_EXPR_TKSIZEOF_TKROUNDOPEN_TKROUNDCLOSE              = 611, // <sizeof_expr> ::= tkSizeOf tkRoundOpen <simple_type_identifier> tkRoundClose
	RULE_TYPEOF_EXPR_TKTYPEOF_TKROUNDOPEN_TKROUNDCLOSE              = 612, // <typeof_expr> ::= tkTypeOf tkRoundOpen <simple_type_identifier> tkRoundClose
	RULE_QUESTION_EXPR_TKQUESTION_TKCOLON                           = 613, // <question_expr> ::= <expr_l1> tkQuestion <expr_l1> tkColon <expr_l1>
	RULE_NEW_EXPR                                                   = 614, // <new_expr> ::= <identifier> <simple_type_identifier> <opt_template_type_params> <opt_expr_list_with_bracket>
	RULE_NEW_EXPR_TKSQUAREOPEN_TKSQUARECLOSE                        = 615, // <new_expr> ::= <identifier> <array_name_for_new_expr> tkSquareOpen <expr_list> tkSquareClose
	RULE_ARRAY_NAME_FOR_NEW_EXPR                                    = 616, // <array_name_for_new_expr> ::= <simple_type_identifier>
	RULE_ARRAY_NAME_FOR_NEW_EXPR2                                   = 617, // <array_name_for_new_expr> ::= <unsized_array_type>
	RULE_OPT_TEMPLATE_TYPE_PARAMS                                   = 618, // <opt_template_type_params> ::= 
	RULE_OPT_TEMPLATE_TYPE_PARAMS2                                  = 619, // <opt_template_type_params> ::= <template_type_params>
	RULE_OPT_EXPR_LIST_WITH_BRACKET                                 = 620, // <opt_expr_list_with_bracket> ::= 
	RULE_OPT_EXPR_LIST_WITH_BRACKET_TKROUNDOPEN_TKROUNDCLOSE        = 621, // <opt_expr_list_with_bracket> ::= tkRoundOpen <opt_expr_list> tkRoundClose
	RULE_RELOP_EXPR                                                 = 622, // <relop_expr> ::= <simple_expr>
	RULE_RELOP_EXPR2                                                = 623, // <relop_expr> ::= <simple_expr> <relop> <relop_expr>
	RULE_FORMAT_EXPR_TKCOLON                                        = 624, // <format_expr> ::= <simple_expr> tkColon <simple_expr>
	RULE_FORMAT_EXPR_TKCOLON_TKCOLON                                = 625, // <format_expr> ::= <simple_expr> tkColon <simple_expr> tkColon <simple_expr>
	RULE_RELOP_TKEQUAL                                              = 626, // <relop> ::= tkEqual
	RULE_RELOP_TKNOTEQUAL                                           = 627, // <relop> ::= tkNotEqual
	RULE_RELOP_TKLOWER                                              = 628, // <relop> ::= tkLower
	RULE_RELOP_TKGREATER                                            = 629, // <relop> ::= tkGreater
	RULE_RELOP_TKLOWEREQUAL                                         = 630, // <relop> ::= tkLowerEqual
	RULE_RELOP_TKGREATEREQUAL                                       = 631, // <relop> ::= tkGreaterEqual
	RULE_RELOP_TKIN                                                 = 632, // <relop> ::= tkIn
	RULE_SIMPLE_EXPR                                                = 633, // <simple_expr> ::= <term>
	RULE_SIMPLE_EXPR2                                               = 634, // <simple_expr> ::= <simple_expr> <addop> <term>
	RULE_ADDOP_TKPLUS                                               = 635, // <addop> ::= tkPlus
	RULE_ADDOP_TKMINUS                                              = 636, // <addop> ::= tkMinus
	RULE_ADDOP_TKOR                                                 = 637, // <addop> ::= tkOr
	RULE_ADDOP_TKXOR                                                = 638, // <addop> ::= tkXor
	RULE_TYPECAST_OP_TKAS                                           = 639, // <typecast_op> ::= tkAs <empty>
	RULE_TYPECAST_OP_TKIS                                           = 640, // <typecast_op> ::= tkIs <empty>
	RULE_TERM                                                       = 641, // <term> ::= <factor>
	RULE_TERM2                                                      = 642, // <term> ::= <term> <mulop> <factor>
	RULE_TERM3                                                      = 643, // <term> ::= <term> <typecast_op> <simple_type_identifier>
	RULE_MULOP_TKSTAR                                               = 644, // <mulop> ::= tkStar
	RULE_MULOP_TKSLASH                                              = 645, // <mulop> ::= tkSlash
	RULE_MULOP_TKDIV                                                = 646, // <mulop> ::= tkDiv
	RULE_MULOP_TKMOD                                                = 647, // <mulop> ::= tkMod
	RULE_MULOP_TKSHL                                                = 648, // <mulop> ::= tkShl
	RULE_MULOP_TKSHR                                                = 649, // <mulop> ::= tkShr
	RULE_MULOP_TKAND                                                = 650, // <mulop> ::= tkAnd
	RULE_FACTOR_TKNIL                                               = 651, // <factor> ::= tkNil <empty>
	RULE_FACTOR                                                     = 652, // <factor> ::= <literal_or_number>
	RULE_FACTOR_TKSQUAREOPEN_TKSQUARECLOSE                          = 653, // <factor> ::= tkSquareOpen <elem_list> tkSquareClose
	RULE_FACTOR_TKNOT                                               = 654, // <factor> ::= tkNot <factor>
	RULE_FACTOR2                                                    = 655, // <factor> ::= <sign> <factor>
	RULE_FACTOR_TKDEREF                                             = 656, // <factor> ::= tkDeref <factor>
	RULE_FACTOR3                                                    = 657, // <factor> ::= <var_reference>
	RULE_LITERAL_OR_NUMBER                                          = 658, // <literal_or_number> ::= <literal>
	RULE_LITERAL_OR_NUMBER2                                         = 659, // <literal_or_number> ::= <unsigned_number>
	RULE_VAR_REFERENCE                                              = 660, // <var_reference> ::= <var_address> <variable>
	RULE_VAR_REFERENCE2                                             = 661, // <var_reference> ::= <variable>
	RULE_VAR_ADDRESS_TKADDRESSOF                                    = 662, // <var_address> ::= tkAddressOf <empty>
	RULE_VAR_ADDRESS_TKADDRESSOF2                                   = 663, // <var_address> ::= <var_address> tkAddressOf
	RULE_VARIABLE                                                   = 664, // <variable> ::= <identifier>
	RULE_VARIABLE2                                                  = 665, // <variable> ::= <operator_name_ident>
	RULE_VARIABLE_TKINHERITED                                       = 666, // <variable> ::= tkInherited <identifier>
	RULE_VARIABLE_TKROUNDOPEN_TKROUNDCLOSE                          = 667, // <variable> ::= tkRoundOpen <expr> tkRoundClose
	RULE_VARIABLE3                                                  = 668, // <variable> ::= <sizeof_expr>
	RULE_VARIABLE4                                                  = 669, // <variable> ::= <typeof_expr>
	RULE_VARIABLE_TKROUNDOPEN_TKROUNDCLOSE2                         = 670, // <variable> ::= tkRoundOpen tkRoundClose
	RULE_VARIABLE_TKPOINT                                           = 671, // <variable> ::= <literal_or_number> tkPoint <identifier_or_keyword>
	RULE_VARIABLE5                                                  = 672, // <variable> ::= <variable> <var_specifiers>
	RULE_OPT_EXPR_LIST                                              = 673, // <opt_expr_list> ::= <expr_list>
	RULE_OPT_EXPR_LIST2                                             = 674, // <opt_expr_list> ::= 
	RULE_VAR_SPECIFIERS_TKSQUAREOPEN_TKSQUARECLOSE                  = 675, // <var_specifiers> ::= tkSquareOpen <expr_list> tkSquareClose
	RULE_VAR_SPECIFIERS_TKSQUAREOPEN_TKSQUARECLOSE2                 = 676, // <var_specifiers> ::= tkSquareOpen tkSquareClose
	RULE_VAR_SPECIFIERS_TKROUNDOPEN_TKROUNDCLOSE                    = 677, // <var_specifiers> ::= tkRoundOpen <opt_expr_list> tkRoundClose
	RULE_VAR_SPECIFIERS_TKPOINT                                     = 678, // <var_specifiers> ::= tkPoint <identifier_keyword_operatorname>
	RULE_VAR_SPECIFIERS_TKDEREF                                     = 679, // <var_specifiers> ::= tkDeref <empty>
	RULE_VAR_SPECIFIERS_TKAMPERSEND                                 = 680, // <var_specifiers> ::= tkAmpersend <template_type_params>
	RULE_TEMPLATE_TYPE_BACK_VARSPECIFIERS_TKROUNDOPEN_TKROUNDCLOSE  = 681, // <template_type_back_varspecifiers> ::= tkRoundOpen <expr_list> tkRoundClose
	RULE_TEMPLATE_TYPE_BACK_VARSPECIFIERS_TKROUNDOPEN_TKROUNDCLOSE2 = 682, // <template_type_back_varspecifiers> ::= tkRoundOpen tkRoundClose
	RULE_ELEM_LIST                                                  = 683, // <elem_list> ::= <elem_list1>
	RULE_ELEM_LIST2                                                 = 684, // <elem_list> ::= 
	RULE_ELEM_LIST1                                                 = 685, // <elem_list1> ::= <elem> <empty>
	RULE_ELEM_LIST1_TKCOMMA                                         = 686, // <elem_list1> ::= <elem_list1> tkComma <elem>
	RULE_ELEM                                                       = 687, // <elem> ::= <expr>
	RULE_ELEM_TKDOTDOT                                              = 688, // <elem> ::= <expr> tkDotDot <expr>
	RULE_ONE_LITERAL_TKSTRINGLITERAL                                = 689, // <one_literal> ::= tkStringLiteral
	RULE_ONE_LITERAL_TKASCIICHAR                                    = 690, // <one_literal> ::= tkAsciiChar
	RULE_LITERAL                                                    = 691, // <literal> ::= <literal_list> <empty>
	RULE_LITERAL_LIST                                               = 692, // <literal_list> ::= <one_literal> <empty>
	RULE_LITERAL_LIST2                                              = 693, // <literal_list> ::= <literal_list> <one_literal>
	RULE_OPERATOR_NAME_IDENT_TKOPERATOR                             = 694, // <operator_name_ident> ::= tkOperator <overload_operator>
	RULE_OPT_METH_MODIFICATORS_TKSEMICOLON                          = 695, // <opt_meth_modificators> ::= tkSemiColon
	RULE_OPT_METH_MODIFICATORS_TKSEMICOLON_TKSEMICOLON              = 696, // <opt_meth_modificators> ::= tkSemiColon <meth_modificators> tkSemiColon
	RULE_METH_MODIFICATORS                                          = 697, // <meth_modificators> ::= <meth_modificator> <empty>
	RULE_METH_MODIFICATORS_TKSEMICOLON                              = 698, // <meth_modificators> ::= <meth_modificators> tkSemiColon <meth_modificator>
	RULE_INTEGER_CONST_TKINTEGER                                    = 699, // <integer_const> ::= <sign> tkInteger
	RULE_INTEGER_CONST_TKINTEGER2                                   = 700, // <integer_const> ::= tkInteger
	RULE_INTEGER_CONST_TKHEX                                        = 701, // <integer_const> ::= <sign> tkHex
	RULE_INTEGER_CONST_TKHEX2                                       = 702, // <integer_const> ::= tkHex
	RULE_INTEGER_CONST                                              = 703, // <integer_const> ::= <identifier>
	RULE_INTEGER_CONST2                                             = 704, // <integer_const> ::= <sign> <identifier>
	RULE_IDENTIFIER_TKIDENTIFIER                                    = 705, // <identifier> ::= tkIdentifier
	RULE_IDENTIFIER                                                 = 706, // <identifier> ::= <real_type_name>
	RULE_IDENTIFIER2                                                = 707, // <identifier> ::= <ord_type_name>
	RULE_IDENTIFIER3                                                = 708, // <identifier> ::= <variant_type_name>
	RULE_IDENTIFIER4                                                = 709, // <identifier> ::= <meth_modificator>
	RULE_IDENTIFIER5                                                = 710, // <identifier> ::= <property_specifier_directives>
	RULE_IDENTIFIER6                                                = 711, // <identifier> ::= <non_reserved>
	RULE_IDENTIFIER7                                                = 712, // <identifier> ::= <other>
	RULE_IDENTIFIER_OR_KEYWORD                                      = 713, // <identifier_or_keyword> ::= <identifier>
	RULE_IDENTIFIER_OR_KEYWORD2                                     = 714, // <identifier_or_keyword> ::= <keyword> <empty>
	RULE_IDENTIFIER_OR_KEYWORD3                                     = 715, // <identifier_or_keyword> ::= <reserved_keyword> <empty>
	RULE_IDENTIFIER_KEYWORD_OPERATORNAME                            = 716, // <identifier_keyword_operatorname> ::= <identifier>
	RULE_IDENTIFIER_KEYWORD_OPERATORNAME2                           = 717, // <identifier_keyword_operatorname> ::= <keyword> <empty>
	RULE_IDENTIFIER_KEYWORD_OPERATORNAME3                           = 718, // <identifier_keyword_operatorname> ::= <operator_name_ident>
	RULE_REAL_TYPE_NAME_TKREAL                                      = 719, // <real_type_name> ::= tkReal
	RULE_REAL_TYPE_NAME_TKSINGLE                                    = 720, // <real_type_name> ::= tkSingle
	RULE_REAL_TYPE_NAME_TKDOUBLE                                    = 721, // <real_type_name> ::= tkDouble
	RULE_REAL_TYPE_NAME_TKEXTENDED                                  = 722, // <real_type_name> ::= tkExtended
	RULE_REAL_TYPE_NAME_TKCOMP                                      = 723, // <real_type_name> ::= tkComp
	RULE_ORD_TYPE_NAME_TKSHORTINT                                   = 724, // <ord_type_name> ::= tkShortInt
	RULE_ORD_TYPE_NAME_TKSMALLINT                                   = 725, // <ord_type_name> ::= tkSmallInt
	RULE_ORD_TYPE_NAME_TKORDINTEGER                                 = 726, // <ord_type_name> ::= tkOrdInteger
	RULE_ORD_TYPE_NAME_TKBYTE                                       = 727, // <ord_type_name> ::= tkByte
	RULE_ORD_TYPE_NAME_TKLONGINT                                    = 728, // <ord_type_name> ::= tkLongInt
	RULE_ORD_TYPE_NAME_TKINT64                                      = 729, // <ord_type_name> ::= tkInt64
	RULE_ORD_TYPE_NAME_TKWORD                                       = 730, // <ord_type_name> ::= tkWord
	RULE_ORD_TYPE_NAME_TKBOOLEAN                                    = 731, // <ord_type_name> ::= tkBoolean
	RULE_ORD_TYPE_NAME_TKCHAR                                       = 732, // <ord_type_name> ::= tkChar
	RULE_ORD_TYPE_NAME_TKWIDECHAR                                   = 733, // <ord_type_name> ::= tkWideChar
	RULE_ORD_TYPE_NAME_TKLONGWORD                                   = 734, // <ord_type_name> ::= tkLongWord
	RULE_ORD_TYPE_NAME_TKPCHAR                                      = 735, // <ord_type_name> ::= tkPChar
	RULE_ORD_TYPE_NAME_TKCARDINAL                                   = 736, // <ord_type_name> ::= tkCardinal
	RULE_VARIANT_TYPE_NAME_TKVARIANT                                = 737, // <variant_type_name> ::= tkVariant
	RULE_VARIANT_TYPE_NAME_TKOLEVARIANT                             = 738, // <variant_type_name> ::= tkOleVariant
	RULE_METH_MODIFICATOR_TKABSTRACT                                = 739, // <meth_modificator> ::= tkAbstract
	RULE_METH_MODIFICATOR_TKOVERLOAD                                = 740, // <meth_modificator> ::= tkOverload
	RULE_METH_MODIFICATOR_TKREINTRODUCE                             = 741, // <meth_modificator> ::= tkReintroduce
	RULE_METH_MODIFICATOR_TKOVERRIDE                                = 742, // <meth_modificator> ::= tkOverride
	RULE_METH_MODIFICATOR_TKVIRTUAL                                 = 743, // <meth_modificator> ::= tkVirtual
	RULE_METH_MODIFICATOR_TKSTATIC                                  = 744, // <meth_modificator> ::= tkStatic
	RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKDEFAULT                    = 745, // <property_specifier_directives> ::= tkDefault
	RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKREAD                       = 746, // <property_specifier_directives> ::= tkRead
	RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKWRITE                      = 747, // <property_specifier_directives> ::= tkWrite
	RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKSTORED                     = 748, // <property_specifier_directives> ::= tkStored
	RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKNODEFAULT                  = 749, // <property_specifier_directives> ::= tkNodefault
	RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKIMPLEMENTS                 = 750, // <property_specifier_directives> ::= tkImplements
	RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKWRITEONLY                  = 751, // <property_specifier_directives> ::= tkWriteOnly
	RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKREADONLY                   = 752, // <property_specifier_directives> ::= tkReadOnly
	RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKDISPID                     = 753, // <property_specifier_directives> ::= tkDispid
	RULE_NON_RESERVED_TKAT                                          = 754, // <non_reserved> ::= tkAt
	RULE_NON_RESERVED_TKABSOLUTE                                    = 755, // <non_reserved> ::= tkAbsolute
	RULE_NON_RESERVED_TKON                                          = 756, // <non_reserved> ::= tkOn
	RULE_NON_RESERVED_TKNAME                                        = 757, // <non_reserved> ::= tkName
	RULE_NON_RESERVED_TKINDEX                                       = 758, // <non_reserved> ::= tkIndex
	RULE_NON_RESERVED_TKMESSAGE                                     = 759, // <non_reserved> ::= tkMessage
	RULE_NON_RESERVED_TKCONTAINS                                    = 760, // <non_reserved> ::= tkContains
	RULE_NON_RESERVED_TKREQUIRES                                    = 761, // <non_reserved> ::= tkRequires
	RULE_NON_RESERVED_TKFORWARD                                     = 762, // <non_reserved> ::= tkForward
	RULE_NON_RESERVED_TKOUT                                         = 763, // <non_reserved> ::= tkOut
	RULE_NON_RESERVED_TKOBJECT                                      = 764, // <non_reserved> ::= tkObject
	RULE_VISIBILITY_SPECIFIER_TKINTERNAL                            = 765, // <visibility_specifier> ::= tkInternal
	RULE_VISIBILITY_SPECIFIER_TKPUBLIC                              = 766, // <visibility_specifier> ::= tkPublic
	RULE_VISIBILITY_SPECIFIER_TKPROTECTED                           = 767, // <visibility_specifier> ::= tkProtected
	RULE_VISIBILITY_SPECIFIER_TKPRIVATE                             = 768, // <visibility_specifier> ::= tkPrivate
	RULE_OTHER_TKPACKAGE                                            = 769, // <other> ::= tkPackage
	RULE_OTHER_TKUNIT                                               = 770, // <other> ::= tkUnit
	RULE_OTHER_TKLIBRARY                                            = 771, // <other> ::= tkLibrary
	RULE_OTHER_TKEXTERNAL                                           = 772, // <other> ::= tkExternal
	RULE_OTHER_TKBF                                                 = 773, // <other> ::= tkBF
	RULE_OTHER_TKPARAMS                                             = 774, // <other> ::= tkParams
	RULE_KEYWORD                                                    = 775, // <keyword> ::= <visibility_specifier> <empty>
	RULE_KEYWORD_TKFINAL                                            = 776, // <keyword> ::= tkFinal
	RULE_KEYWORD_TKTEMPLATE                                         = 777, // <keyword> ::= tkTemplate
	RULE_KEYWORD_TKOR                                               = 778, // <keyword> ::= tkOr
	RULE_KEYWORD_TKTYPEOF                                           = 779, // <keyword> ::= tkTypeOf
	RULE_KEYWORD_TKSIZEOF                                           = 780, // <keyword> ::= tkSizeOf
	RULE_KEYWORD_TKWHERE                                            = 781, // <keyword> ::= tkWhere
	RULE_KEYWORD_TKXOR                                              = 782, // <keyword> ::= tkXor
	RULE_KEYWORD_TKAND                                              = 783, // <keyword> ::= tkAnd
	RULE_KEYWORD_TKDIV                                              = 784, // <keyword> ::= tkDiv
	RULE_KEYWORD_TKMOD                                              = 785, // <keyword> ::= tkMod
	RULE_KEYWORD_TKSHL                                              = 786, // <keyword> ::= tkShl
	RULE_KEYWORD_TKSHR                                              = 787, // <keyword> ::= tkShr
	RULE_KEYWORD_TKNOT                                              = 788, // <keyword> ::= tkNot
	RULE_KEYWORD_TKAS                                               = 789, // <keyword> ::= tkAs
	RULE_KEYWORD_TKIN                                               = 790, // <keyword> ::= tkIn
	RULE_KEYWORD_TKIS                                               = 791, // <keyword> ::= tkIs
	RULE_KEYWORD_TKARRAY                                            = 792, // <keyword> ::= tkArray
	RULE_KEYWORD_TKBEGIN                                            = 793, // <keyword> ::= tkBegin
	RULE_KEYWORD_TKCASE                                             = 794, // <keyword> ::= tkCase
	RULE_KEYWORD_TKCLASS                                            = 795, // <keyword> ::= tkClass
	RULE_KEYWORD_TKCONST                                            = 796, // <keyword> ::= tkConst
	RULE_KEYWORD_TKCONSTRUCTOR                                      = 797, // <keyword> ::= tkConstructor
	RULE_KEYWORD_TKDESTRUCTOR                                       = 798, // <keyword> ::= tkDestructor
	RULE_KEYWORD_TKDOWNTO                                           = 799, // <keyword> ::= tkDownto
	RULE_KEYWORD_TKDO                                               = 800, // <keyword> ::= tkDo
	RULE_KEYWORD_TKELSE                                             = 801, // <keyword> ::= tkElse
	RULE_KEYWORD_TKEND                                              = 802, // <keyword> ::= tkEnd
	RULE_KEYWORD_TKEXCEPT                                           = 803, // <keyword> ::= tkExcept
	RULE_KEYWORD_TKFILE                                             = 804, // <keyword> ::= tkFile
	RULE_KEYWORD_TKFINALIZATION                                     = 805, // <keyword> ::= tkFinalization
	RULE_KEYWORD_TKFINALLY                                          = 806, // <keyword> ::= tkFinally
	RULE_KEYWORD_TKFOR                                              = 807, // <keyword> ::= tkFor
	RULE_KEYWORD_TKFUNCTION                                         = 808, // <keyword> ::= tkFunction
	RULE_KEYWORD_TKIF                                               = 809, // <keyword> ::= tkIf
	RULE_KEYWORD_TKIMPLEMENTATION                                   = 810, // <keyword> ::= tkImplementation
	RULE_KEYWORD_TKINHERITED                                        = 811, // <keyword> ::= tkInherited
	RULE_KEYWORD_TKINITIALIZATION                                   = 812, // <keyword> ::= tkInitialization
	RULE_KEYWORD_TKINTERFACE                                        = 813, // <keyword> ::= tkInterface
	RULE_KEYWORD_TKPROCEDURE                                        = 814, // <keyword> ::= tkProcedure
	RULE_KEYWORD_TKPROPERTY                                         = 815, // <keyword> ::= tkProperty
	RULE_KEYWORD_TKRAISE                                            = 816, // <keyword> ::= tkRaise
	RULE_KEYWORD_TKRECORD                                           = 817, // <keyword> ::= tkRecord
	RULE_KEYWORD_TKREPEAT                                           = 818, // <keyword> ::= tkRepeat
	RULE_KEYWORD_TKSET                                              = 819, // <keyword> ::= tkSet
	RULE_KEYWORD_TKTRY                                              = 820, // <keyword> ::= tkTry
	RULE_KEYWORD_TKTYPE                                             = 821, // <keyword> ::= tkType
	RULE_KEYWORD_TKTHEN                                             = 822, // <keyword> ::= tkThen
	RULE_KEYWORD_TKTO                                               = 823, // <keyword> ::= tkTo
	RULE_KEYWORD_TKUNTIL                                            = 824, // <keyword> ::= tkUntil
	RULE_KEYWORD_TKUSES                                             = 825, // <keyword> ::= tkUses
	RULE_KEYWORD_TKUSING                                            = 826, // <keyword> ::= tkUsing
	RULE_KEYWORD_TKVAR                                              = 827, // <keyword> ::= tkVar
	RULE_KEYWORD_TKWHILE                                            = 828, // <keyword> ::= tkWhile
	RULE_KEYWORD_TKWITH                                             = 829, // <keyword> ::= tkWith
	RULE_KEYWORD_TKNIL                                              = 830, // <keyword> ::= tkNil
	RULE_KEYWORD_TKGOTO                                             = 831, // <keyword> ::= tkGoto
	RULE_KEYWORD_TKOF                                               = 832, // <keyword> ::= tkOf
	RULE_KEYWORD_TKLABEL                                            = 833, // <keyword> ::= tkLabel
	RULE_KEYWORD_TKPROGRAM                                          = 834, // <keyword> ::= tkProgram
	RULE_RESERVED_KEYWORD_TKOPERATOR                                = 835, // <reserved_keyword> ::= tkOperator
	RULE_OVERLOAD_OPERATOR_TKMINUS                                  = 836, // <overload_operator> ::= tkMinus
	RULE_OVERLOAD_OPERATOR_TKPLUS                                   = 837, // <overload_operator> ::= tkPlus
	RULE_OVERLOAD_OPERATOR_TKSQUAREOPEN_TKSQUARECLOSE               = 838, // <overload_operator> ::= tkSquareOpen tkSquareClose
	RULE_OVERLOAD_OPERATOR_TKROUNDOPEN_TKROUNDCLOSE                 = 839, // <overload_operator> ::= tkRoundOpen tkRoundClose
	RULE_OVERLOAD_OPERATOR_TKSLASH                                  = 840, // <overload_operator> ::= tkSlash
	RULE_OVERLOAD_OPERATOR_TKSTAR                                   = 841, // <overload_operator> ::= tkStar
	RULE_OVERLOAD_OPERATOR_TKEQUAL                                  = 842, // <overload_operator> ::= tkEqual
	RULE_OVERLOAD_OPERATOR_TKGREATER                                = 843, // <overload_operator> ::= tkGreater
	RULE_OVERLOAD_OPERATOR_TKGREATEREQUAL                           = 844, // <overload_operator> ::= tkGreaterEqual
	RULE_OVERLOAD_OPERATOR_TKLOWER                                  = 845, // <overload_operator> ::= tkLower
	RULE_OVERLOAD_OPERATOR_TKLOWEREQUAL                             = 846, // <overload_operator> ::= tkLowerEqual
	RULE_OVERLOAD_OPERATOR_TKNOTEQUAL                               = 847, // <overload_operator> ::= tkNotEqual
	RULE_OVERLOAD_OPERATOR_TKOR                                     = 848, // <overload_operator> ::= tkOr
	RULE_OVERLOAD_OPERATOR_TKXOR                                    = 849, // <overload_operator> ::= tkXor
	RULE_OVERLOAD_OPERATOR_TKAND                                    = 850, // <overload_operator> ::= tkAnd
	RULE_OVERLOAD_OPERATOR_TKDIV                                    = 851, // <overload_operator> ::= tkDiv
	RULE_OVERLOAD_OPERATOR_TKMOD                                    = 852, // <overload_operator> ::= tkMod
	RULE_OVERLOAD_OPERATOR_TKSHL                                    = 853, // <overload_operator> ::= tkShl
	RULE_OVERLOAD_OPERATOR_TKSHR                                    = 854, // <overload_operator> ::= tkShr
	RULE_OVERLOAD_OPERATOR_TKNOT                                    = 855, // <overload_operator> ::= tkNot
	RULE_OVERLOAD_OPERATOR_TKIN                                     = 856, // <overload_operator> ::= tkIn
	RULE_OVERLOAD_OPERATOR_TKADDRESSOF                              = 857, // <overload_operator> ::= tkAddressOf
	RULE_OVERLOAD_OPERATOR_TKDEREF                                  = 858, // <overload_operator> ::= tkDeref
	RULE_OVERLOAD_OPERATOR                                          = 859, // <overload_operator> ::= <assign_operator>
	RULE_ASSIGN_OPERATOR_TKASSIGN                                   = 860, // <assign_operator> ::= tkAssign
	RULE_ASSIGN_OPERATOR_TKPLUSEQUAL                                = 861, // <assign_operator> ::= tkPlusEqual
	RULE_ASSIGN_OPERATOR_TKMINUSEQUAL                               = 862, // <assign_operator> ::= tkMinusEqual
	RULE_ASSIGN_OPERATOR_TKMULTEQUAL                                = 863, // <assign_operator> ::= tkMultEqual
	RULE_ASSIGN_OPERATOR_TKDIVEQUAL                                 = 864, // <assign_operator> ::= tkDivEqual
	RULE_EMPTY                                                      = 865, // <empty> ::= 
	RULE_ERROR_TKERROR                                              = 866  // <error> ::= tkError
};













///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//CreateTerminalObject
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

private Object CreateTerminalObject(int TokenSymbolIndex)
{
switch (TokenSymbolIndex)
{
	case (int)SymbolConstants.SYMBOL_EOF :
    	//(EOF)
	//TERMINAL:EOF
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ERROR :
    	//(Error)
	//TERMINAL:Error
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_WHITESPACE :
    	//(Whitespace)
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'Whitespace' return NULL! \n\r");*/return null;
	case (int)SymbolConstants.SYMBOL_TKABSOLUTE :
    	//tkAbsolute

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKABSTRACT :
    	//tkAbstract

		{
			procedure_attribute _procedure_attribute=new procedure_attribute(proc_attribute.attr_abstract);
                        
			//_procedure_attribute.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_procedure_attribute.source_context=parsertools.GetTokenSourceContext();
			 	_procedure_attribute.name=LRParser.TokenText;
			return _procedure_attribute;
		}

	case (int)SymbolConstants.SYMBOL_TKADDRESSOF :
    	//tkAddressOf

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKAMPERSEND :
    	//tkAmpersend

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKAND :
    	//tkAnd

		{
			op_type_node _op_type_node=new op_type_node(Operators.LogicalAND);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			  _op_type_node.text=LRParser.TokenText;
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKARRAY :
    	//tkArray

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKAS :
    	//tkAs

		{
			op_type_node _op_type_node=new op_type_node(Operators.As);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			   _op_type_node.text=LRParser.TokenText;
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKASCIICHAR :
    	//tkAsciiChar
return parsertools.create_sharp_char_const(this);
	case (int)SymbolConstants.SYMBOL_TKASMBODY :
    	//tkAsmBody
	//TERMINAL:tkAsmBody
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TKASSEMBLER :
    	//tkAssembler

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKASSIGN :
    	//tkAssign

		{
			op_type_node _op_type_node=new op_type_node(Operators.Assignment);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKAT :
    	//tkAt

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKAUTOMATED :
    	//tkAutomated

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKBEGIN :
    	//tkBegin

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKBF :
    	//tkBF

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKBOOLEAN :
    	//tkBoolean

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKBYTE :
    	//tkByte

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKCARDINAL :
    	//tkCardinal

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKCASE :
    	//tkCase

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKCHAR :
    	//tkChar

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKCLASS :
    	//tkClass

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKCOLON :
    	//tkColon

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKCOMMA :
    	//tkComma

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKCOMP :
    	//tkComp

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKCONST :
    	//tkConst

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKCONSTRUCTOR :
    	//tkConstructor

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKCONTAINS :
    	//tkContains

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKDEFAULT :
    	//tkDefault

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKDEREF :
    	//tkDeref

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKDESTRUCTOR :
    	//tkDestructor

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKDIRECTIVENAME :
    	//tkDirectiveName
return parsertools.create_directive_name(this);
	case (int)SymbolConstants.SYMBOL_TKDISPID :
    	//tkDispid

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKDIV :
    	//tkDiv

		{
			op_type_node _op_type_node=new op_type_node(Operators.IntegerDivision);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			 _op_type_node.text=LRParser.TokenText;
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKDIVEQUAL :
    	//tkDivEqual

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentDivision);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKDO :
    	//tkDo

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKDOTDOT :
    	//tkDotDot

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKDOUBLE :
    	//tkDouble

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKDOWNTO :
    	//tkDownto

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKELSE :
    	//tkElse

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKEND :
    	//tkEnd

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKEQUAL :
    	//tkEqual

		{
			op_type_node _op_type_node=new op_type_node(Operators.Equal);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKERROR :
    	//tkError
	//TERMINAL:tkError
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TKEVENT :
    	//tkEvent

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKEXCEPT :
    	//tkExcept

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKEXPORTS :
    	//tkExports
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'tkExports' return NULL! \n\r");*/return null;
	case (int)SymbolConstants.SYMBOL_TKEXTENDED :
    	//tkExtended

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKEXTERNAL :
    	//tkExternal

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKFILE :
    	//tkFile

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKFINAL :
    	//tkFinal

		{
			token_taginfo _token_taginfo=new token_taginfo(class_attribute.Sealed);
                        
			//_token_taginfo.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_taginfo.source_context=parsertools.GetTokenSourceContext();
			
			return _token_taginfo;
		}

	case (int)SymbolConstants.SYMBOL_TKFINALIZATION :
    	//tkFinalization

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKFINALLY :
    	//tkFinally

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKFLOAT :
    	//tkFloat
return parsertools.create_double_const(this);
	case (int)SymbolConstants.SYMBOL_TKFOR :
    	//tkFor

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKFOREACH :
    	//tkForeach

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKFORWARD :
    	//tkForward

		{
			procedure_attribute _procedure_attribute=new procedure_attribute(proc_attribute.attr_forward);
                        
			//_procedure_attribute.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_procedure_attribute.source_context=parsertools.GetTokenSourceContext();
			 	_procedure_attribute.name=LRParser.TokenText;
			return _procedure_attribute;
		}

	case (int)SymbolConstants.SYMBOL_TKFUNCTION :
    	//tkFunction

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKGOTO :
    	//tkGoto

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKGREATER :
    	//tkGreater

		{
			op_type_node _op_type_node=new op_type_node(Operators.Greater);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKGREATEREQUAL :
    	//tkGreaterEqual

		{
			op_type_node _op_type_node=new op_type_node(Operators.GreaterEqual);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKHEX :
    	//tkHex
return parsertools.create_hex_const(this);
	case (int)SymbolConstants.SYMBOL_TKIDENTIFIER :
    	//tkIdentifier
return parsertools.create_ident(this);
	case (int)SymbolConstants.SYMBOL_TKIF :
    	//tkIf

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKILCODE :
    	//tkILCode

	case (int)SymbolConstants.SYMBOL_TKIMPLEMENTATION :
    	//tkImplementation

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKIMPLEMENTS :
    	//tkImplements

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKIN :
    	//tkIn

		{
			op_type_node _op_type_node=new op_type_node(Operators.In);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			   _op_type_node.text=LRParser.TokenText;
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKINDEX :
    	//tkIndex

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKINHERITED :
    	//tkInherited

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKINITIALIZATION :
    	//tkInitialization

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKINLINE :
    	//tkInline
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'tkInline' return NULL! \n\r");*/return null;
	case (int)SymbolConstants.SYMBOL_TKINT64 :
    	//tkInt64

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKINTEGER :
    	//tkInteger
return parsertools.create_int_const(this);
	case (int)SymbolConstants.SYMBOL_TKINTERFACE :
    	//tkInterface

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKINTERNAL :
    	//tkInternal

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKIS :
    	//tkIs

		{
			op_type_node _op_type_node=new op_type_node(Operators.Is);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			   _op_type_node.text=LRParser.TokenText;
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKLABEL :
    	//tkLabel

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKLIBRARY :
    	//tkLibrary

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKLOCK :
    	//tkLock

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKLONGINT :
    	//tkLongInt

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKLONGWORD :
    	//tkLongWord

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKLOWER :
    	//tkLower

		{
			op_type_node _op_type_node=new op_type_node(Operators.Less);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKLOWEREQUAL :
    	//tkLowerEqual

		{
			op_type_node _op_type_node=new op_type_node(Operators.LessEqual);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKMESSAGE :
    	//tkMessage

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKMINUS :
    	//tkMinus

		{
			op_type_node _op_type_node=new op_type_node(Operators.Minus);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKMINUSEQUAL :
    	//tkMinusEqual

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentSubtraction);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKMOD :
    	//tkMod

		{
			op_type_node _op_type_node=new op_type_node(Operators.ModulusRemainder);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			  _op_type_node.text=LRParser.TokenText;
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKMULTEQUAL :
    	//tkMultEqual

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentMultiplication);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKNAME :
    	//tkName

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKNIL :
    	//tkNil

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKNODEFAULT :
    	//tkNodefault

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKNOT :
    	//tkNot

		{
			op_type_node _op_type_node=new op_type_node(Operators.LogicalNOT);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			  _op_type_node.text=LRParser.TokenText;
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKNOTEQUAL :
    	//tkNotEqual

		{
			op_type_node _op_type_node=new op_type_node(Operators.NotEqual);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKOBJECT :
    	//tkObject

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKOF :
    	//tkOf

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKOLEVARIANT :
    	//tkOleVariant

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKON :
    	//tkOn

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKOPERATOR :
    	//tkOperator

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKOR :
    	//tkOr

		{
			op_type_node _op_type_node=new op_type_node(Operators.LogicalOR);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			   _op_type_node.text=LRParser.TokenText;
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKORDINTEGER :
    	//tkOrdInteger

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKOUT :
    	//tkOut

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKOVERLOAD :
    	//tkOverload

		{
			procedure_attribute _procedure_attribute=new procedure_attribute(proc_attribute.attr_overload);
                        
			//_procedure_attribute.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_procedure_attribute.source_context=parsertools.GetTokenSourceContext();
			 	_procedure_attribute.name=LRParser.TokenText;
			return _procedure_attribute;
		}

	case (int)SymbolConstants.SYMBOL_TKOVERRIDE :
    	//tkOverride

		{
			procedure_attribute _procedure_attribute=new procedure_attribute(proc_attribute.attr_override);
                        
			//_procedure_attribute.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_procedure_attribute.source_context=parsertools.GetTokenSourceContext();
			 	_procedure_attribute.name=LRParser.TokenText;
			return _procedure_attribute;
		}

	case (int)SymbolConstants.SYMBOL_TKPACKAGE :
    	//tkPackage

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKPACKED :
    	//tkPacked
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'tkPacked' return NULL! \n\r");*/return null;
	case (int)SymbolConstants.SYMBOL_TKPARAMS :
    	//tkParams

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKPARSEMODEEXPRESSION :
    	//tkParseModeExpression
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'tkParseModeExpression' return NULL! \n\r");*/return null;
	case (int)SymbolConstants.SYMBOL_TKPCHAR :
    	//tkPChar

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKPLUS :
    	//tkPlus

		{
			op_type_node _op_type_node=new op_type_node(Operators.Plus);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKPLUSEQUAL :
    	//tkPlusEqual

		{
			op_type_node _op_type_node=new op_type_node(Operators.AssignmentAddition);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKPOINT :
    	//tkPoint

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKPRIVATE :
    	//tkPrivate

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKPROCEDURE :
    	//tkProcedure

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKPROGRAM :
    	//tkProgram

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKPROPERTY :
    	//tkProperty

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKPROTECTED :
    	//tkProtected

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKPUBLIC :
    	//tkPublic

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKQUESTION :
    	//tkQuestion

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKRAISE :
    	//tkRaise

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKREAD :
    	//tkRead

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKREADONLY :
    	//tkReadOnly

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKREAL :
    	//tkReal

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKRECORD :
    	//tkRecord

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKREINTRODUCE :
    	//tkReintroduce

		{
			procedure_attribute _procedure_attribute=new procedure_attribute(proc_attribute.attr_reintroduce);
                        
			//_procedure_attribute.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_procedure_attribute.source_context=parsertools.GetTokenSourceContext();
			_procedure_attribute.name=LRParser.TokenText;
			return _procedure_attribute;
		}

	case (int)SymbolConstants.SYMBOL_TKREPEAT :
    	//tkRepeat

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKREQUIRES :
    	//tkRequires

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKRESIDENT :
    	//tkResident

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKRESOURCESTRING :
    	//tkResourceString
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'tkResourceString' return NULL! \n\r");*/return null;
	case (int)SymbolConstants.SYMBOL_TKROUNDCLOSE :
    	//tkRoundClose

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKROUNDOPEN :
    	//tkRoundOpen

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKSEMICOLON :
    	//tkSemiColon

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKSET :
    	//tkSet

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKSHL :
    	//tkShl

		{
			op_type_node _op_type_node=new op_type_node(Operators.BitwiseLeftShift);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			  _op_type_node.text=LRParser.TokenText;
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKSHORTINT :
    	//tkShortInt

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKSHR :
    	//tkShr

		{
			op_type_node _op_type_node=new op_type_node(Operators.BitwiseRightShift);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			  _op_type_node.text=LRParser.TokenText;
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKSINGLE :
    	//tkSingle

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKSIZEOF :
    	//tkSizeOf

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKSLASH :
    	//tkSlash

		{
			op_type_node _op_type_node=new op_type_node(Operators.Division);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKSMALLINT :
    	//tkSmallInt

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKSQUARECLOSE :
    	//tkSquareClose

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKSQUAREOPEN :
    	//tkSquareOpen

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKSTAR :
    	//tkStar

		{
			op_type_node _op_type_node=new op_type_node(Operators.Multiplication);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_TKSTATIC :
    	//tkStatic

		{
			procedure_attribute _procedure_attribute=new procedure_attribute(proc_attribute.attr_static);
                        
			//_procedure_attribute.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_procedure_attribute.source_context=parsertools.GetTokenSourceContext();
			 	_procedure_attribute.name=LRParser.TokenText;
			return _procedure_attribute;
		}

	case (int)SymbolConstants.SYMBOL_TKSTORED :
    	//tkStored

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKSTRINGLITERAL :
    	//tkStringLiteral
return parsertools.create_string_const(this);
	case (int)SymbolConstants.SYMBOL_TKTEMPLATE :
    	//tkTemplate

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKTHEN :
    	//tkThen

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKTHREADVAR :
    	//tkThreadvar
 /*Console.Error.WriteLine("\n\rwarning: TerminalToken 'tkThreadvar' return NULL! \n\r");*/return null;
	case (int)SymbolConstants.SYMBOL_TKTO :
    	//tkTo

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKTRY :
    	//tkTry

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKTYPE :
    	//tkType

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKTYPEOF :
    	//tkTypeOf

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKUNIT :
    	//tkUnit

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKUNTIL :
    	//tkUntil

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKUSES :
    	//tkUses

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKUSING :
    	//tkUsing

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKVAR :
    	//tkVar

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKVARIANT :
    	//tkVariant

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKVIRTUAL :
    	//tkVirtual

		{
			procedure_attribute _procedure_attribute=new procedure_attribute(proc_attribute.attr_virtual);
                        
			//_procedure_attribute.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_procedure_attribute.source_context=parsertools.GetTokenSourceContext();
			 	_procedure_attribute.name=LRParser.TokenText;
			return _procedure_attribute;
		}

	case (int)SymbolConstants.SYMBOL_TKWHERE :
    	//tkWhere

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKWHILE :
    	//tkWhile

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKWIDECHAR :
    	//tkWideChar

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKWITH :
    	//tkWith

		{
			token_info _token_info=new token_info(LRParser.TokenText);
                        
			//_token_info.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_token_info.source_context=parsertools.GetTokenSourceContext();
			
			return _token_info;
		}

	case (int)SymbolConstants.SYMBOL_TKWORD :
    	//tkWord

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKWRITE :
    	//tkWrite

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKWRITEONLY :
    	//tkWriteOnly

		{
			ident _ident=new ident(LRParser.TokenText);
                        
			//_ident.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_ident.source_context=parsertools.GetTokenSourceContext();
			
			return _ident;
		}

	case (int)SymbolConstants.SYMBOL_TKXOR :
    	//tkXor

		{
			op_type_node _op_type_node=new op_type_node(Operators.BitwiseXOR);
                        
			//_op_type_node.source_context=new SourceContext(LRParser.TokenLineNumber,LRParser.TokenLinePosition,LRParser.TokenLineNumber,LRParser.TokenLinePosition+LRParser.TokenLength-1);
			_op_type_node.source_context=parsertools.GetTokenSourceContext();
			  _op_type_node.text=LRParser.TokenText;
			return _op_type_node;
		}

	case (int)SymbolConstants.SYMBOL_ABC_BLOCK :
    	//<abc_block>
	//TERMINAL:abc_block
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_CONSTRUCTOR_DECL :
    	//<abc_constructor_decl>
	//TERMINAL:abc_constructor_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_DECL_SECT :
    	//<abc_decl_sect>
	//TERMINAL:abc_decl_sect
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_DECL_SECT_LIST :
    	//<abc_decl_sect_list>
	//TERMINAL:abc_decl_sect_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_DECL_SECT_LIST1 :
    	//<abc_decl_sect_list1>
	//TERMINAL:abc_decl_sect_list1
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_DESTRUCTOR_DECL :
    	//<abc_destructor_decl>
	//TERMINAL:abc_destructor_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_EXTERNAL_DIRECTR :
    	//<abc_external_directr>
	//TERMINAL:abc_external_directr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_FUNC_DECL :
    	//<abc_func_decl>
	//TERMINAL:abc_func_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_FUNC_DECL_NOCLASS :
    	//<abc_func_decl_noclass>
	//TERMINAL:abc_func_decl_noclass
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_INTERFACE_PART :
    	//<abc_interface_part>
	//TERMINAL:abc_interface_part
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_METHOD_DECL :
    	//<abc_method_decl>
	//TERMINAL:abc_method_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_PROC_BLOCK :
    	//<abc_proc_block>
	//TERMINAL:abc_proc_block
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_PROC_DECL :
    	//<abc_proc_decl>
	//TERMINAL:abc_proc_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ABC_PROC_DECL_NOCLASS :
    	//<abc_proc_decl_noclass>
	//TERMINAL:abc_proc_decl_noclass
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ADDOP :
    	//<addop>
	//TERMINAL:addop
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ALLOWABLE_EXPR_AS_STMT :
    	//<allowable_expr_as_stmt>
	//TERMINAL:allowable_expr_as_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ARRAY_CONST :
    	//<array_const>
	//TERMINAL:array_const
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ARRAY_NAME_FOR_NEW_EXPR :
    	//<array_name_for_new_expr>
	//TERMINAL:array_name_for_new_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ARRAY_TYPE :
    	//<array_type>
	//TERMINAL:array_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ASM_BLOCK :
    	//<asm_block>
	//TERMINAL:asm_block
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ASM_STMT :
    	//<asm_stmt>
	//TERMINAL:asm_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ASSIGN_OPERATOR :
    	//<assign_operator>
	//TERMINAL:assign_operator
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ASSIGNMENT :
    	//<assignment>
	//TERMINAL:assignment
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_BASE_CLASS_NAME :
    	//<base_class_name>
	//TERMINAL:base_class_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_BASE_CLASSES_NAMES_LIST :
    	//<base_classes_names_list>
	//TERMINAL:base_classes_names_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_BF_BLOCK :
    	//<bf_block>
	//TERMINAL:bf_block
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_BF_EMPTY_INSTRUCTION :
    	//<bf_empty_instruction>
	//TERMINAL:bf_empty_instruction
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_BF_INSTRUCTION :
    	//<bf_instruction>
	//TERMINAL:bf_instruction
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_BF_INSTRUCTIONS :
    	//<bf_instructions>
	//TERMINAL:bf_instructions
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_BF_INSTRUCTIONS_LIST :
    	//<bf_instructions_list>
	//TERMINAL:bf_instructions_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_BLOCK :
    	//<block>
	//TERMINAL:block
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CASE_ITEM :
    	//<case_item>
	//TERMINAL:case_item
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CASE_LABEL :
    	//<case_label>
	//TERMINAL:case_label
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CASE_LABEL_LIST :
    	//<case_label_list>
	//TERMINAL:case_label_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CASE_LIST :
    	//<case_list>
	//TERMINAL:case_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CASE_STMT :
    	//<case_stmt>
	//TERMINAL:case_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CASE_TAG_LIST :
    	//<case_tag_list>
	//TERMINAL:case_tag_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CLASS_ATTRIBUTES :
    	//<class_attributes>
	//TERMINAL:class_attributes
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CLASS_OR_INTERFACE_KEYWORD :
    	//<class_or_interface_keyword>
	//TERMINAL:class_or_interface_keyword
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_COMPOUND_STMT :
    	//<compound_stmt>
	//TERMINAL:compound_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_ADDOP :
    	//<const_addop>
	//TERMINAL:const_addop
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_DECL :
    	//<const_decl>
	//TERMINAL:const_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_DECL_SECT :
    	//<const_decl_sect>
	//TERMINAL:const_decl_sect
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_ELEM :
    	//<const_elem>
	//TERMINAL:const_elem
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_ELEM_LIST :
    	//<const_elem_list>
	//TERMINAL:const_elem_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_ELEM_LIST1 :
    	//<const_elem_list1>
	//TERMINAL:const_elem_list1
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_EXPR :
    	//<const_expr>
	//TERMINAL:const_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_EXPR_LIST :
    	//<const_expr_list>
	//TERMINAL:const_expr_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_FACTOR :
    	//<const_factor>
	//TERMINAL:const_factor
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_FIELD :
    	//<const_field>
	//TERMINAL:const_field
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_FIELD_LIST :
    	//<const_field_list>
	//TERMINAL:const_field_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_FIELD_LIST_1 :
    	//<const_field_list_1>
	//TERMINAL:const_field_list_1
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_FIELD_NAME :
    	//<const_field_name>
	//TERMINAL:const_field_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_FUNC_EXPR_LIST :
    	//<const_func_expr_list>
	//TERMINAL:const_func_expr_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_MULOP :
    	//<const_mulop>
	//TERMINAL:const_mulop
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_NAME :
    	//<const_name>
	//TERMINAL:const_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_RELOP :
    	//<const_relop>
	//TERMINAL:const_relop
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_SET :
    	//<const_set>
	//TERMINAL:const_set
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_SIMPLE_EXPR :
    	//<const_simple_expr>
	//TERMINAL:const_simple_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_TERM :
    	//<const_term>
	//TERMINAL:const_term
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_VARIABLE :
    	//<const_variable>
	//TERMINAL:const_variable
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONST_VARIABLE_2 :
    	//<const_variable_2>
	//TERMINAL:const_variable_2
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONSTRUCTOR_DECL :
    	//<constructor_decl>
	//TERMINAL:constructor_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_CONTAINS_CLAUSE :
    	//<contains_clause>
	//TERMINAL:contains_clause
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_DECLARED_VAR_NAME :
    	//<declared_var_name>
	//TERMINAL:declared_var_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_DESTRUCTOR_DECL :
    	//<destructor_decl>
	//TERMINAL:destructor_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ELEM :
    	//<elem>
	//TERMINAL:elem
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ELEM_LIST :
    	//<elem_list>
	//TERMINAL:elem_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ELEM_LIST1 :
    	//<elem_list1>
	//TERMINAL:elem_list1
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ELSE_BRANCH :
    	//<else_branch>
	//TERMINAL:else_branch
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ELSE_CASE :
    	//<else_case>
	//TERMINAL:else_case
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EMPTY :
    	//<empty>
	//TERMINAL:empty
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ENUMERATION_ID :
    	//<enumeration_id>
	//TERMINAL:enumeration_id
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ENUMERATION_ID_LIST :
    	//<enumeration_id_list>
	//TERMINAL:enumeration_id_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ERROR2 :
    	//<error>
	//TERMINAL:error
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXCEPTION_BLOCK :
    	//<exception_block>
	//TERMINAL:exception_block
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXCEPTION_BLOCK_ELSE_BRANCH :
    	//<exception_block_else_branch>
	//TERMINAL:exception_block_else_branch
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXCEPTION_CLASS_TYPE_IDENTIFIER :
    	//<exception_class_type_identifier>
	//TERMINAL:exception_class_type_identifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXCEPTION_HANDLER :
    	//<exception_handler>
	//TERMINAL:exception_handler
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXCEPTION_HANDLER_LIST :
    	//<exception_handler_list>
	//TERMINAL:exception_handler_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXCEPTION_IDENTIFIER :
    	//<exception_identifier>
	//TERMINAL:exception_identifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXCEPTION_VARIABLE :
    	//<exception_variable>
	//TERMINAL:exception_variable
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPORT_CLAUSE :
    	//<export_clause>
	//TERMINAL:export_clause
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPORTS_ENTRY :
    	//<exports_entry>
	//TERMINAL:exports_entry
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPORTS_INDEX :
    	//<exports_index>
	//TERMINAL:exports_index
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPORTS_LIST :
    	//<exports_list>
	//TERMINAL:exports_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPORTS_NAME :
    	//<exports_name>
	//TERMINAL:exports_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPORTS_RESIDENT :
    	//<exports_resident>
	//TERMINAL:exports_resident
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPR :
    	//<expr>
	//TERMINAL:expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPR_AS_STMT :
    	//<expr_as_stmt>
	//TERMINAL:expr_as_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPR_L1 :
    	//<expr_l1>
	//TERMINAL:expr_l1
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXPR_LIST :
    	//<expr_list>
	//TERMINAL:expr_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXTERNAL_DIRECTR :
    	//<external_directr>
	//TERMINAL:external_directr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_EXTERNAL_DIRECTR_IDENT :
    	//<external_directr_ident>
	//TERMINAL:external_directr_ident
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FACTOR :
    	//<factor>
	//TERMINAL:factor
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FIELD_ACCESS_MODIFIER :
    	//<field_access_modifier>
	//TERMINAL:field_access_modifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FIELD_LIST :
    	//<field_list>
	//TERMINAL:field_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FILE_TYPE :
    	//<file_type>
	//TERMINAL:file_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FILED_OR_CONST_DEFINITION :
    	//<filed_or_const_definition>
	//TERMINAL:filed_or_const_definition
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FILED_OR_CONST_DEFINITION_OR_AM :
    	//<filed_or_const_definition_or_am>
	//TERMINAL:filed_or_const_definition_or_am
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FIXED_PART :
    	//<fixed_part>
	//TERMINAL:fixed_part
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FIXED_PART_2 :
    	//<fixed_part_2>
	//TERMINAL:fixed_part_2
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FOR_CYCLE_TYPE :
    	//<for_cycle_type>
	//TERMINAL:for_cycle_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FOR_STMT :
    	//<for_stmt>
	//TERMINAL:for_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FOR_STMT_DECL_OR_ASSIGN :
    	//<for_stmt_decl_or_assign>
	//TERMINAL:for_stmt_decl_or_assign
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FOREACH_STMT :
    	//<foreach_stmt>
	//TERMINAL:foreach_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FOREACH_STMT_IDENT_DYPE_OPT :
    	//<foreach_stmt_ident_dype_opt>
	//TERMINAL:foreach_stmt_ident_dype_opt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FORMAT_EXPR :
    	//<format_expr>
	//TERMINAL:format_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FP_LIST :
    	//<fp_list>
	//TERMINAL:fp_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FP_SECT :
    	//<fp_sect>
	//TERMINAL:fp_sect
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FP_SECT_LIST :
    	//<fp_sect_list>
	//TERMINAL:fp_sect_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FPTYPE :
    	//<fptype>
	//TERMINAL:fptype
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FPTYPE_NEW :
    	//<fptype_new>
	//TERMINAL:fptype_new
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FUNC_BLOCK :
    	//<func_block>
	//TERMINAL:func_block
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FUNC_CLASS_NAME_IDENT :
    	//<func_class_name_ident>
	//TERMINAL:func_class_name_ident
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FUNC_DECL :
    	//<func_decl>
	//TERMINAL:func_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FUNC_DECL_NOCLASS :
    	//<func_decl_noclass>
	//TERMINAL:func_decl_noclass
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FUNC_HEADING :
    	//<func_heading>
	//TERMINAL:func_heading
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FUNC_METH_NAME_IDENT :
    	//<func_meth_name_ident>
	//TERMINAL:func_meth_name_ident
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_FUNC_NAME :
    	//<func_name>
	//TERMINAL:func_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_GOTO_STMT :
    	//<goto_stmt>
	//TERMINAL:goto_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_HEAD_COMPILER_DIRECTIVES :
    	//<head_compiler_directives>
	//TERMINAL:head_compiler_directives
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IDENT_LIST :
    	//<ident_list>
	//TERMINAL:ident_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IDENT_OR_KEYWORD_POINTSEPARATOR_LIST :
    	//<ident_or_keyword_pointseparator_list>
	//TERMINAL:ident_or_keyword_pointseparator_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IDENTIFIER :
    	//<identifier>
	//TERMINAL:identifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IDENTIFIER_KEYWORD_OPERATORNAME :
    	//<identifier_keyword_operatorname>
	//TERMINAL:identifier_keyword_operatorname
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IDENTIFIER_OR_KEYWORD :
    	//<identifier_or_keyword>
	//TERMINAL:identifier_or_keyword
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IF_STMT :
    	//<if_stmt>
	//TERMINAL:if_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IF_THEN_ELSE_BRANCH :
    	//<if_then_else_branch>
	//TERMINAL:if_then_else_branch
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IMPL_DECL_SECT :
    	//<impl_decl_sect>
	//TERMINAL:impl_decl_sect
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IMPL_DECL_SECT_LIST :
    	//<impl_decl_sect_list>
	//TERMINAL:impl_decl_sect_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IMPL_DECL_SECT_LIST1 :
    	//<impl_decl_sect_list1>
	//TERMINAL:impl_decl_sect_list1
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_IMPLEMENTATION_PART :
    	//<implementation_part>
	//TERMINAL:implementation_part
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INHERITED_MESSAGE :
    	//<inherited_message>
	//TERMINAL:inherited_message
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INIT_CONST_EXPR :
    	//<init_const_expr>
	//TERMINAL:init_const_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INITIALIZATION_PART :
    	//<initialization_part>
	//TERMINAL:initialization_part
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INT_DECL_SECT :
    	//<int_decl_sect>
	//TERMINAL:int_decl_sect
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INT_DECL_SECT_LIST :
    	//<int_decl_sect_list>
	//TERMINAL:int_decl_sect_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INT_DECL_SECT_LIST1 :
    	//<int_decl_sect_list1>
	//TERMINAL:int_decl_sect_list1
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INT_FUNC_HEADING :
    	//<int_func_heading>
	//TERMINAL:int_func_heading
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INT_PROC_HEADING :
    	//<int_proc_heading>
	//TERMINAL:int_proc_heading
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INTEGER_CONST :
    	//<integer_const>
	//TERMINAL:integer_const
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_INTERFACE_PART :
    	//<interface_part>
	//TERMINAL:interface_part
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_KEYWORD :
    	//<keyword>
	//TERMINAL:keyword
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LABEL_DECL_SECT :
    	//<label_decl_sect>
	//TERMINAL:label_decl_sect
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LABEL_LIST :
    	//<label_list>
	//TERMINAL:label_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LABEL_NAME :
    	//<label_name>
	//TERMINAL:label_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LIBRARY_BLOCK :
    	//<library_block>
	//TERMINAL:library_block
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LIBRARY_FILE :
    	//<library_file>
	//TERMINAL:library_file
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LIBRARY_HEADING :
    	//<library_heading>
	//TERMINAL:library_heading
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LIBRARY_IMPL_DECL_SECT :
    	//<library_impl_decl_sect>
	//TERMINAL:library_impl_decl_sect
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LIBRARY_IMPL_DECL_SECT_LIST :
    	//<library_impl_decl_sect_list>
	//TERMINAL:library_impl_decl_sect_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LITERAL :
    	//<literal>
	//TERMINAL:literal
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LITERAL_LIST :
    	//<literal_list>
	//TERMINAL:literal_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LITERAL_OR_NUMBER :
    	//<literal_or_number>
	//TERMINAL:literal_or_number
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_LOCK_STMT :
    	//<lock_stmt>
	//TERMINAL:lock_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_MAIN_USED_UNIT_NAME :
    	//<main_used_unit_name>
	//TERMINAL:main_used_unit_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_MAIN_USED_UNITS_LIST :
    	//<main_used_units_list>
	//TERMINAL:main_used_units_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_MAIN_USES_CLAUSE :
    	//<main_uses_clause>
	//TERMINAL:main_uses_clause
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_MAYBE_ERROR :
    	//<maybe_error>
	//TERMINAL:maybe_error
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_METH_MODIFICATOR :
    	//<meth_modificator>
	//TERMINAL:meth_modificator
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_METH_MODIFICATORS :
    	//<meth_modificators>
	//TERMINAL:meth_modificators
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_MULOP :
    	//<mulop>
	//TERMINAL:mulop
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NEW_EXPR :
    	//<new_expr>
	//TERMINAL:new_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NEW_OBJECT_TYPE :
    	//<new_object_type>
	//TERMINAL:new_object_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NEW_RECORD_TYPE :
    	//<new_record_type>
	//TERMINAL:new_record_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NON_RESERVED :
    	//<non_reserved>
	//TERMINAL:non_reserved
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_ARRAY_DEFAULTPROPERTY :
    	//<not_array_defaultproperty>
	//TERMINAL:not_array_defaultproperty
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_COMPONENT_LIST :
    	//<not_component_list>
	//TERMINAL:not_component_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_COMPONENT_LIST_1 :
    	//<not_component_list_1>
	//TERMINAL:not_component_list_1
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_COMPONENT_LIST_2 :
    	//<not_component_list_2>
	//TERMINAL:not_component_list_2
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_COMPONENT_LIST_SEQ :
    	//<not_component_list_seq>
	//TERMINAL:not_component_list_seq
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_CONSTRUCTOR_BLOCK_DECL :
    	//<not_constructor_block_decl>
	//TERMINAL:not_constructor_block_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_CONSTRUCTOR_HEADING :
    	//<not_constructor_heading>
	//TERMINAL:not_constructor_heading
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_DESTRUCTOR_HEADING :
    	//<not_destructor_heading>
	//TERMINAL:not_destructor_heading
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_FIELD_DEFINITION :
    	//<not_field_definition>
	//TERMINAL:not_field_definition
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_GUID :
    	//<not_guid>
	//TERMINAL:not_guid
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_METHOD_DEFINITION :
    	//<not_method_definition>
	//TERMINAL:not_method_definition
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_METHOD_HEADING :
    	//<not_method_heading>
	//TERMINAL:not_method_heading
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_OBJECT_TYPE :
    	//<not_object_type>
	//TERMINAL:not_object_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_OBJECT_TYPE_IDENTIFIER_LIST :
    	//<not_object_type_identifier_list>
	//TERMINAL:not_object_type_identifier_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_PARAMETER_DECL :
    	//<not_parameter_decl>
	//TERMINAL:not_parameter_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_PARAMETER_DECL_LIST :
    	//<not_parameter_decl_list>
	//TERMINAL:not_parameter_decl_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_PARAMETER_NAME_LIST :
    	//<not_parameter_name_list>
	//TERMINAL:not_parameter_name_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_PROPERTY_DEFINITION :
    	//<not_property_definition>
	//TERMINAL:not_property_definition
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_PROPERTY_INTERFACE :
    	//<not_property_interface>
	//TERMINAL:not_property_interface
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_PROPERTY_INTERFACE_INDEX :
    	//<not_property_interface_index>
	//TERMINAL:not_property_interface_index
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_PROPERTY_PARAMETER_LIST :
    	//<not_property_parameter_list>
	//TERMINAL:not_property_parameter_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_NOT_PROPERTY_SPECIFIERS :
    	//<not_property_specifiers>
	//TERMINAL:not_property_specifiers
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OBJECT_TYPE :
    	//<object_type>
	//TERMINAL:object_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ONE_COMPILER_DIRECTIVE :
    	//<one_compiler_directive>
	//TERMINAL:one_compiler_directive
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ONE_LITERAL :
    	//<one_literal>
	//TERMINAL:one_literal
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ONLY_CONST_DECL :
    	//<only_const_decl>
	//TERMINAL:only_const_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_COMPONENT_LIST :
    	//<oot_component_list>
	//TERMINAL:oot_component_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_CONSTRUCTOR_HEAD :
    	//<oot_constructor_head>
	//TERMINAL:oot_constructor_head
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_DESTRUCTOR_HEAD :
    	//<oot_destructor_head>
	//TERMINAL:oot_destructor_head
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_FIELD :
    	//<oot_field>
	//TERMINAL:oot_field
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_FIELD_IDENTIFIER :
    	//<oot_field_identifier>
	//TERMINAL:oot_field_identifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_FIELD_LIST :
    	//<oot_field_list>
	//TERMINAL:oot_field_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_ID_LIST :
    	//<oot_id_list>
	//TERMINAL:oot_id_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_METHOD :
    	//<oot_method>
	//TERMINAL:oot_method
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_METHOD_HEAD :
    	//<oot_method_head>
	//TERMINAL:oot_method_head
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_METHOD_LIST :
    	//<oot_method_list>
	//TERMINAL:oot_method_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_PRIVAT_LIST :
    	//<oot_privat_list>
	//TERMINAL:oot_privat_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_SUCCESSOR :
    	//<oot_successor>
	//TERMINAL:oot_successor
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OOT_TYPEIDENTIFIER :
    	//<oot_typeidentifier>
	//TERMINAL:oot_typeidentifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPERATOR_NAME_IDENT :
    	//<operator_name_ident>
	//TERMINAL:operator_name_ident
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_BASE_CLASSES :
    	//<opt_base_classes>
	//TERMINAL:opt_base_classes
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_EXPR_LIST :
    	//<opt_expr_list>
	//TERMINAL:opt_expr_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_EXPR_LIST_WITH_BRACKET :
    	//<opt_expr_list_with_bracket>
	//TERMINAL:opt_expr_list_with_bracket
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_HEAD_COMPILER_DIRECTIVES :
    	//<opt_head_compiler_directives>
	//TERMINAL:opt_head_compiler_directives
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_IDENTIFIER :
    	//<opt_identifier>
	//TERMINAL:opt_identifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_METH_MODIFICATORS :
    	//<opt_meth_modificators>
	//TERMINAL:opt_meth_modificators
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_NOT_COMPONENT_LIST_SEQ_END :
    	//<opt_not_component_list_seq_end>
	//TERMINAL:opt_not_component_list_seq_end
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_SEMICOLON :
    	//<opt_semicolon>
	//TERMINAL:opt_semicolon
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_TEMPLATE_ARGUMENTS :
    	//<opt_template_arguments>
	//TERMINAL:opt_template_arguments
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_TEMPLATE_TYPE_PARAMS :
    	//<opt_template_type_params>
	//TERMINAL:opt_template_type_params
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_VAR :
    	//<opt_var>
	//TERMINAL:opt_var
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPT_WHERE_SECTION :
    	//<opt_where_section>
	//TERMINAL:opt_where_section
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OPTIONAL_QUALIFIED_IDENTIFIER :
    	//<optional_qualified_identifier>
	//TERMINAL:optional_qualified_identifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_ORD_TYPE_NAME :
    	//<ord_type_name>
	//TERMINAL:ord_type_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OT_VISIBILITY_SPECIFIER :
    	//<ot_visibility_specifier>
	//TERMINAL:ot_visibility_specifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OTHER :
    	//<other>
	//TERMINAL:other
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_OVERLOAD_OPERATOR :
    	//<overload_operator>
	//TERMINAL:overload_operator
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PACKAGE_FILE :
    	//<package_file>
	//TERMINAL:package_file
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PACKAGE_NAME :
    	//<package_name>
	//TERMINAL:package_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PARAM_NAME :
    	//<param_name>
	//TERMINAL:param_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PARAM_NAME_LIST :
    	//<param_name_list>
	//TERMINAL:param_name_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PARSE_GOAL :
    	//<parse_goal>
	//TERMINAL:parse_goal
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PARTS :
    	//<parts>
	//TERMINAL:parts
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_POINTER_TYPE :
    	//<pointer_type>
	//TERMINAL:pointer_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROC_BLOCK :
    	//<proc_block>
	//TERMINAL:proc_block
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROC_BLOCK_DECL :
    	//<proc_block_decl>
	//TERMINAL:proc_block_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROC_CALL :
    	//<proc_call>
	//TERMINAL:proc_call
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROC_DECL :
    	//<proc_decl>
	//TERMINAL:proc_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROC_DECL_NOCLASS :
    	//<proc_decl_noclass>
	//TERMINAL:proc_decl_noclass
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROC_HEADING :
    	//<proc_heading>
	//TERMINAL:proc_heading
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROC_NAME :
    	//<proc_name>
	//TERMINAL:proc_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROCEDURAL_TYPE :
    	//<procedural_type>
	//TERMINAL:procedural_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROCEDURAL_TYPE_DECL :
    	//<procedural_type_decl>
	//TERMINAL:procedural_type_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROCEDURAL_TYPE_KIND :
    	//<procedural_type_kind>
	//TERMINAL:procedural_type_kind
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROGRAM_BLOCK :
    	//<program_block>
	//TERMINAL:program_block
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROGRAM_DECL_SECT_LIST :
    	//<program_decl_sect_list>
	//TERMINAL:program_decl_sect_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROGRAM_FILE :
    	//<program_file>
	//TERMINAL:program_file
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROGRAM_HEADING :
    	//<program_heading>
	//TERMINAL:program_heading
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROGRAM_HEADING_2 :
    	//<program_heading_2>
	//TERMINAL:program_heading_2
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROGRAM_NAME :
    	//<program_name>
	//TERMINAL:program_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROGRAM_PARAM :
    	//<program_param>
	//TERMINAL:program_param
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROGRAM_PARAM_LIST :
    	//<program_param_list>
	//TERMINAL:program_param_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_PROPERTY_SPECIFIER_DIRECTIVES :
    	//<property_specifier_directives>
	//TERMINAL:property_specifier_directives
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_QUALIFIED_IDENTIFIER :
    	//<qualified_identifier>
	//TERMINAL:qualified_identifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_QUESTION_EXPR :
    	//<question_expr>
	//TERMINAL:question_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RAISE_STMT :
    	//<raise_stmt>
	//TERMINAL:raise_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RANGE_EXPR :
    	//<range_expr>
	//TERMINAL:range_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RANGE_FACTOR :
    	//<range_factor>
	//TERMINAL:range_factor
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RANGE_METHODNAME :
    	//<range_methodname>
	//TERMINAL:range_methodname
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RANGE_TERM :
    	//<range_term>
	//TERMINAL:range_term
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_REAL_TYPE_NAME :
    	//<real_type_name>
	//TERMINAL:real_type_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RECORD_COMPONENT_LIST :
    	//<record_component_list>
	//TERMINAL:record_component_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RECORD_CONST :
    	//<record_const>
	//TERMINAL:record_const
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RECORD_KEYWORD :
    	//<record_keyword>
	//TERMINAL:record_keyword
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RECORD_SECTION :
    	//<record_section>
	//TERMINAL:record_section
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RECORD_SECTION_ID :
    	//<record_section_id>
	//TERMINAL:record_section_id
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RECORD_SECTION_ID_LIST :
    	//<record_section_id_list>
	//TERMINAL:record_section_id_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RECORD_TYPE :
    	//<record_type>
	//TERMINAL:record_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RELOP :
    	//<relop>
	//TERMINAL:relop
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RELOP_EXPR :
    	//<relop_expr>
	//TERMINAL:relop_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_REPEAT_STMT :
    	//<repeat_stmt>
	//TERMINAL:repeat_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_REQUIRES_CLAUSE :
    	//<requires_clause>
	//TERMINAL:requires_clause
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RES_STR_DECL_SECT :
    	//<res_str_decl_sect>
	//TERMINAL:res_str_decl_sect
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_RESERVED_KEYWORD :
    	//<reserved_keyword>
	//TERMINAL:reserved_keyword
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_SET_TYPE :
    	//<set_type>
	//TERMINAL:set_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_SIGN :
    	//<sign>
	//TERMINAL:sign
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_SIMPLE_EXPR :
    	//<simple_expr>
	//TERMINAL:simple_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_SIMPLE_TYPE :
    	//<simple_type>
	//TERMINAL:simple_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_SIMPLE_TYPE_IDENTIFIER :
    	//<simple_type_identifier>
	//TERMINAL:simple_type_identifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_SIMPLE_TYPE_LIST :
    	//<simple_type_list>
	//TERMINAL:simple_type_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_SIZEOF_EXPR :
    	//<sizeof_expr>
	//TERMINAL:sizeof_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_STMT :
    	//<stmt>
	//TERMINAL:stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_STMT_LIST :
    	//<stmt_list>
	//TERMINAL:stmt_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_STRING_TYPE :
    	//<string_type>
	//TERMINAL:string_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_STRUCTURED_TYPE :
    	//<structured_type>
	//TERMINAL:structured_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TAG_FIELD :
    	//<tag_field>
	//TERMINAL:tag_field
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TAG_FIELD_NAME :
    	//<tag_field_name>
	//TERMINAL:tag_field_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TAG_FIELD_TYPENAME :
    	//<tag_field_typename>
	//TERMINAL:tag_field_typename
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TEMPLATE_PARAM :
    	//<template_param>
	//TERMINAL:template_param
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TEMPLATE_PARAM_LIST :
    	//<template_param_list>
	//TERMINAL:template_param_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TEMPLATE_TYPE :
    	//<template_type>
	//TERMINAL:template_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TEMPLATE_TYPE_BACK_VARSPECIFIERS :
    	//<template_type_back_varspecifiers>
	//TERMINAL:template_type_back_varspecifiers
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TEMPLATE_TYPE_PARAMS :
    	//<template_type_params>
	//TERMINAL:template_type_params
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TERM :
    	//<term>
	//TERMINAL:term
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_THEN_BRANCH :
    	//<then_branch>
	//TERMINAL:then_branch
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TRY_HANDLER :
    	//<try_handler>
	//TERMINAL:try_handler
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TRY_STMT :
    	//<try_stmt>
	//TERMINAL:try_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TYPE_DECL :
    	//<type_decl>
	//TERMINAL:type_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TYPE_DECL_SECT :
    	//<type_decl_sect>
	//TERMINAL:type_decl_sect
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TYPE_DECL_TYPE :
    	//<type_decl_type>
	//TERMINAL:type_decl_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TYPE_REF :
    	//<type_ref>
	//TERMINAL:type_ref
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TYPE_REF_AND_SECIFIC_LIST :
    	//<type_ref_and_secific_list>
	//TERMINAL:type_ref_and_secific_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TYPE_REF_OR_SECIFIC :
    	//<type_ref_or_secific>
	//TERMINAL:type_ref_or_secific
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TYPECAST_OP :
    	//<typecast_op>
	//TERMINAL:typecast_op
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TYPED_CONST :
    	//<typed_const>
	//TERMINAL:typed_const
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TYPED_CONST_LIST :
    	//<typed_const_list>
	//TERMINAL:typed_const_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_TYPEOF_EXPR :
    	//<typeof_expr>
	//TERMINAL:typeof_expr
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_UNIT_FILE :
    	//<unit_file>
	//TERMINAL:unit_file
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_UNIT_HEADING :
    	//<unit_heading>
	//TERMINAL:unit_heading
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_UNIT_KEY_WORD :
    	//<unit_key_word>
	//TERMINAL:unit_key_word
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_UNIT_NAME :
    	//<unit_name>
	//TERMINAL:unit_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_UNLABELLED_STMT :
    	//<unlabelled_stmt>
	//TERMINAL:unlabelled_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_UNPACKED_STRUCTURED_TYPE :
    	//<unpacked_structured_type>
	//TERMINAL:unpacked_structured_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_UNSIGNED_NUMBER :
    	//<unsigned_number>
	//TERMINAL:unsigned_number
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_UNSIZED_ARRAY_TYPE :
    	//<unsized_array_type>
	//TERMINAL:unsized_array_type
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_USES_CLAUSE :
    	//<uses_clause>
	//TERMINAL:uses_clause
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_USING_CLAUSE :
    	//<using_clause>
	//TERMINAL:using_clause
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_USING_LIST :
    	//<using_list>
	//TERMINAL:using_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_USING_ONE :
    	//<using_one>
	//TERMINAL:using_one
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_ADDRESS :
    	//<var_address>
	//TERMINAL:var_address
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_DECL :
    	//<var_decl>
	//TERMINAL:var_decl
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_DECL_PART :
    	//<var_decl_part>
	//TERMINAL:var_decl_part
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_DECL_PART_ASSIGN :
    	//<var_decl_part_assign>
	//TERMINAL:var_decl_part_assign
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_DECL_PART_IN_STMT :
    	//<var_decl_part_in_stmt>
	//TERMINAL:var_decl_part_in_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_DECL_PART_NORMAL :
    	//<var_decl_part_normal>
	//TERMINAL:var_decl_part_normal
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_DECL_SECT :
    	//<var_decl_sect>
	//TERMINAL:var_decl_sect
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_INIT_VALUE :
    	//<var_init_value>
	//TERMINAL:var_init_value
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_INIT_VALUE_TYPED :
    	//<var_init_value_typed>
	//TERMINAL:var_init_value_typed
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_NAME :
    	//<var_name>
	//TERMINAL:var_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_NAME_LIST :
    	//<var_name_list>
	//TERMINAL:var_name_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_REFERENCE :
    	//<var_reference>
	//TERMINAL:var_reference
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_SPECIFIERS :
    	//<var_specifiers>
	//TERMINAL:var_specifiers
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VAR_STMT :
    	//<var_stmt>
	//TERMINAL:var_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIABLE :
    	//<variable>
	//TERMINAL:variable
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIANT :
    	//<variant>
	//TERMINAL:variant
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIANT_FIELD_LIST :
    	//<variant_field_list>
	//TERMINAL:variant_field_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIANT_LIST :
    	//<variant_list>
	//TERMINAL:variant_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIANT_LIST_2 :
    	//<variant_list_2>
	//TERMINAL:variant_list_2
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIANT_PART :
    	//<variant_part>
	//TERMINAL:variant_part
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VARIANT_TYPE_NAME :
    	//<variant_type_name>
	//TERMINAL:variant_type_name
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_VISIBILITY_SPECIFIER :
    	//<visibility_specifier>
	//TERMINAL:visibility_specifier
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_WHERE_PART :
    	//<where_part>
	//TERMINAL:where_part
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_WHERE_PART_LIST :
    	//<where_part_list>
	//TERMINAL:where_part_list
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_WHILE_STMT :
    	//<while_stmt>
	//TERMINAL:while_stmt
	return null;
	//ENDTERMINAL
	case (int)SymbolConstants.SYMBOL_WITH_STMT :
    	//<with_stmt>
	//TERMINAL:with_stmt
	return null;
	//ENDTERMINAL
}
throw new SymbolException("Unknown symbol");
}















///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
//CreateNonTerminalObject
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

public Object CreateNonTerminalObject(int ReductionRuleIndex)
{
switch (ReductionRuleIndex)
{
	case (int)RuleConstants.RULE_PARSE_GOAL :
	//<parse_goal> ::= <program_file>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PARSE_GOAL2 :
	//<parse_goal> ::= <unit_file>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PARSE_GOAL3 :
	//<parse_goal> ::= <parts>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PARTS_TKPARSEMODEEXPRESSION :
	//<parts> ::= tkParseModeExpression <expr>
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_OPT_HEAD_COMPILER_DIRECTIVES :
	//<opt_head_compiler_directives> ::= 
	//NONTERMINAL:<opt_head_compiler_directives> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_OPT_HEAD_COMPILER_DIRECTIVES2 :
	//<opt_head_compiler_directives> ::= <head_compiler_directives>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_HEAD_COMPILER_DIRECTIVES :
	//<head_compiler_directives> ::= <one_compiler_directive>
return null;
	case (int)RuleConstants.RULE_HEAD_COMPILER_DIRECTIVES2 :
	//<head_compiler_directives> ::= <head_compiler_directives> <one_compiler_directive>
return null;
	case (int)RuleConstants.RULE_ONE_COMPILER_DIRECTIVE_TKDIRECTIVENAME_TKIDENTIFIER :
	//<one_compiler_directive> ::= tkDirectiveName tkIdentifier
 {
									token_info t1 = new token_info();
									t1.text=((ident)LRParser.GetReductionSyntaxNode(0)).name;
					                                t1.source_context = ((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
									token_info t2 = new token_info();
									t2.text=((ident)LRParser.GetReductionSyntaxNode(1)).name;
					                                t2.source_context = ((ident)LRParser.GetReductionSyntaxNode(1)).source_context;
									compiler_directive cd=new compiler_directive(t1,t2); 
									parsertools.create_source_context(cd,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)); 
									CompilerDirectives.Add(cd); return null;
								}
	case (int)RuleConstants.RULE_ONE_COMPILER_DIRECTIVE_TKDIRECTIVENAME_TKSTRINGLITERAL :
	//<one_compiler_directive> ::= tkDirectiveName tkStringLiteral
 {
									token_info t1 = new token_info();
									t1.text=((ident)LRParser.GetReductionSyntaxNode(0)).name;
					                                t1.source_context = ((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
									token_info t2 = new token_info();
									t2.text=((string_const)LRParser.GetReductionSyntaxNode(1)).Value;
					                                t2.source_context = ((string_const)LRParser.GetReductionSyntaxNode(1)).source_context;
									compiler_directive cd=new compiler_directive(t1,t2); 
									parsertools.create_source_context(cd,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)); 
									CompilerDirectives.Add(cd); return null;
								}
	case (int)RuleConstants.RULE_PROGRAM_FILE_TKPOINT :
	//<program_file> ::= <program_heading> <opt_head_compiler_directives> <main_uses_clause> <using_clause> <program_block> tkPoint
         
		{
			program_module _program_module=new program_module(LRParser.GetReductionSyntaxNode(0) as program_name,(uses_list)LRParser.GetReductionSyntaxNode(2),(block)LRParser.GetReductionSyntaxNode(4),LRParser.GetReductionSyntaxNode(3) as using_list);
			
									 _program_module.Language = LanguageId.PascalABCNET;
									 parsertools.create_source_context(_program_module,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(4)),LRParser.GetReductionSyntaxNode(4));	
									 
			return _program_module;
		}

	case (int)RuleConstants.RULE_PROGRAM_HEADING :
	//<program_heading> ::= 
	//NONTERMINAL:<program_heading> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_PROGRAM_HEADING_TKPROGRAM :
	//<program_heading> ::= tkProgram <program_name> <program_heading_2>
         
		{
			program_name _program_name=new program_name((ident)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_program_name,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _program_name;
		}

	case (int)RuleConstants.RULE_PROGRAM_HEADING_2_TKSEMICOLON :
	//<program_heading_2> ::= tkSemiColon
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROGRAM_HEADING_2_TKROUNDOPEN_TKROUNDCLOSE_TKSEMICOLON :
	//<program_heading_2> ::= tkRoundOpen <program_param_list> tkRoundClose tkSemiColon
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<program_heading_2> ::= tkRoundOpen <program_param_list> tkRoundClose tkSemiColon"));}return null;
	case (int)RuleConstants.RULE_PROGRAM_NAME_TKIDENTIFIER :
	//<program_name> ::= tkIdentifier
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROGRAM_PARAM_LIST :
	//<program_param_list> ::= <program_param>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROGRAM_PARAM_LIST_TKCOMMA :
	//<program_param_list> ::= <program_param_list> tkComma <program_param>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<program_param_list> ::= <program_param_list> tkComma <program_param>"));}return null;
	case (int)RuleConstants.RULE_PROGRAM_PARAM_TKIDENTIFIER :
	//<program_param> ::= tkIdentifier
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROGRAM_BLOCK :
	//<program_block> ::= <program_decl_sect_list> <compound_stmt>
         
		{
			block _block=new block(null,LRParser.GetReductionSyntaxNode(1) as statement_list);
			
							if (LRParser.GetReductionSyntaxNode(0)!=null) {
								_block.defs=LRParser.GetReductionSyntaxNode(0) as declarations;
								parsertools.create_source_context(_block,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
							}else	
								parsertools.create_source_context(_block,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1));
							
			return _block;
		}

	case (int)RuleConstants.RULE_PROGRAM_DECL_SECT_LIST :
	//<program_decl_sect_list> ::= <impl_decl_sect_list>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_USES_CLAUSE :
	//<uses_clause> ::= <main_uses_clause>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_USING_CLAUSE :
	//<using_clause> ::= 
	//NONTERMINAL:<using_clause> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_USING_CLAUSE2 :
	//<using_clause> ::= <using_list>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_USING_LIST :
	//<using_list> ::= <using_one> <empty>
         
		{
			using_list _using_list=new using_list();
			
								parsertools.create_source_context(_using_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
								_using_list.namespaces.Add((unit_or_namespace)LRParser.GetReductionSyntaxNode(0));
			return _using_list;
		}

	case (int)RuleConstants.RULE_USING_LIST2 :
	//<using_list> ::= <using_list> <using_one>
         
		{
			using_list _using_list=(using_list)LRParser.GetReductionSyntaxNode(0);
								parsertools.create_source_context(_using_list,_using_list,LRParser.GetReductionSyntaxNode(1));
								_using_list.namespaces.Add((unit_or_namespace)LRParser.GetReductionSyntaxNode(1));
			return _using_list;
		}

	case (int)RuleConstants.RULE_USING_ONE_TKUSING_TKSEMICOLON :
	//<using_one> ::= tkUsing <ident_or_keyword_pointseparator_list> tkSemiColon
         
		{
			unit_or_namespace _unit_or_namespace=new unit_or_namespace((ident_list)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_unit_or_namespace,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _unit_or_namespace;
		}

	case (int)RuleConstants.RULE_IDENT_OR_KEYWORD_POINTSEPARATOR_LIST :
	//<ident_or_keyword_pointseparator_list> ::= <identifier_or_keyword> <empty>
         
		//TemplateList for ident_list (create)
		{
			ident_list _ident_list=new ident_list();
			_ident_list.source_context=((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
			_ident_list.idents.Add((ident)LRParser.GetReductionSyntaxNode(0));
			return _ident_list;
		}

	case (int)RuleConstants.RULE_IDENT_OR_KEYWORD_POINTSEPARATOR_LIST_TKPOINT :
	//<ident_or_keyword_pointseparator_list> ::= <ident_or_keyword_pointseparator_list> tkPoint <identifier_or_keyword>

		//TemplateList for ident_list (add)         
		{
			ident_list _ident_list=(ident_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_ident_list,_ident_list,LRParser.GetReductionSyntaxNode(2));
			_ident_list.idents.Add(LRParser.GetReductionSyntaxNode(2) as ident);
			return _ident_list;
		}

	case (int)RuleConstants.RULE_MAIN_USES_CLAUSE :
	//<main_uses_clause> ::= 
	//NONTERMINAL:<main_uses_clause> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_MAIN_USES_CLAUSE_TKUSES_TKSEMICOLON :
	//<main_uses_clause> ::= tkUses <main_used_units_list> tkSemiColon
 parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_MAIN_USED_UNITS_LIST_TKCOMMA :
	//<main_used_units_list> ::= <main_used_units_list> tkComma <main_used_unit_name>
         
		{
			uses_list _uses_list=(uses_list)LRParser.GetReductionSyntaxNode(0);
								_uses_list.units.Add((unit_or_namespace)LRParser.GetReductionSyntaxNode(2));
								
			return _uses_list;
		}

	case (int)RuleConstants.RULE_MAIN_USED_UNITS_LIST :
	//<main_used_units_list> ::= <main_used_unit_name> <empty>
         
		{
			uses_list _uses_list=new uses_list();
			
								_uses_list.units.Add((unit_or_namespace)LRParser.GetReductionSyntaxNode(0));
								
			return _uses_list;
		}

	case (int)RuleConstants.RULE_MAIN_USED_UNIT_NAME :
	//<main_used_unit_name> ::= <ident_or_keyword_pointseparator_list> <empty>
         
		{
			unit_or_namespace _unit_or_namespace=new unit_or_namespace((ident_list)LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(_unit_or_namespace,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
			return _unit_or_namespace;
		}

	case (int)RuleConstants.RULE_MAIN_USED_UNIT_NAME_TKIN_TKSTRINGLITERAL :
	//<main_used_unit_name> ::= <ident_or_keyword_pointseparator_list> tkIn tkStringLiteral
         
		{
			uses_unit_in _uses_unit_in=new uses_unit_in();
			parsertools.create_source_context(_uses_unit_in,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
								_uses_unit_in.name=(ident_list)LRParser.GetReductionSyntaxNode(0);
								_uses_unit_in.in_file=(string_const)LRParser.GetReductionSyntaxNode(2);
								
			return _uses_unit_in;
		}

	case (int)RuleConstants.RULE_LIBRARY_FILE_TKPOINT :
	//<library_file> ::= <library_heading> <main_uses_clause> <library_block> tkPoint
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<library_file> ::= <library_heading> <main_uses_clause> <library_block> tkPoint"));}return null;
	case (int)RuleConstants.RULE_LIBRARY_HEADING_TKLIBRARY_TKIDENTIFIER_TKSEMICOLON :
	//<library_heading> ::= tkLibrary tkIdentifier tkSemiColon
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<library_heading> ::= tkLibrary tkIdentifier tkSemiColon"));}return null;
	case (int)RuleConstants.RULE_LIBRARY_BLOCK :
	//<library_block> ::= <library_impl_decl_sect_list> <compound_stmt>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<library_block> ::= <library_impl_decl_sect_list> <compound_stmt>"));}return null;
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT_LIST :
	//<library_impl_decl_sect_list> ::= 
	//NONTERMINAL:<library_impl_decl_sect_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT_LIST2 :
	//<library_impl_decl_sect_list> ::= <library_impl_decl_sect_list> <library_impl_decl_sect>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<library_impl_decl_sect_list> ::= <library_impl_decl_sect_list> <library_impl_decl_sect>"));}return null;
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT :
	//<library_impl_decl_sect> ::= <label_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT2 :
	//<library_impl_decl_sect> ::= <const_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT3 :
	//<library_impl_decl_sect> ::= <res_str_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT4 :
	//<library_impl_decl_sect> ::= <type_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT5 :
	//<library_impl_decl_sect> ::= <var_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT6 :
	//<library_impl_decl_sect> ::= <proc_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT7 :
	//<library_impl_decl_sect> ::= <func_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT8 :
	//<library_impl_decl_sect> ::= <constructor_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT9 :
	//<library_impl_decl_sect> ::= <destructor_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LIBRARY_IMPL_DECL_SECT10 :
	//<library_impl_decl_sect> ::= <export_clause>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXPORT_CLAUSE_TKEXPORTS_TKSEMICOLON :
	//<export_clause> ::= tkExports <exports_list> tkSemiColon
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<export_clause> ::= tkExports <exports_list> tkSemiColon"));}return null;
	case (int)RuleConstants.RULE_EXPORTS_LIST :
	//<exports_list> ::= <exports_entry>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXPORTS_LIST_TKCOMMA :
	//<exports_list> ::= <exports_list> tkComma <exports_entry>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<exports_list> ::= <exports_list> tkComma <exports_entry>"));}return null;
	case (int)RuleConstants.RULE_EXPORTS_ENTRY :
	//<exports_entry> ::= <identifier> <exports_index> <exports_name> <exports_resident>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<exports_entry> ::= <identifier> <exports_index> <exports_name> <exports_resident>"));}return null;
	case (int)RuleConstants.RULE_EXPORTS_INDEX :
	//<exports_index> ::= 
	//NONTERMINAL:<exports_index> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_EXPORTS_INDEX_TKINDEX :
	//<exports_index> ::= tkIndex <integer_const>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<exports_index> ::= tkIndex <integer_const>"));}return null;
	case (int)RuleConstants.RULE_EXPORTS_NAME :
	//<exports_name> ::= 
	//NONTERMINAL:<exports_name> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_EXPORTS_NAME_TKNAME :
	//<exports_name> ::= tkName <identifier>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<exports_name> ::= tkName <identifier>"));}return null;
	case (int)RuleConstants.RULE_EXPORTS_NAME_TKNAME2 :
	//<exports_name> ::= tkName <literal>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<exports_name> ::= tkName <literal>"));}return null;
	case (int)RuleConstants.RULE_EXPORTS_RESIDENT :
	//<exports_resident> ::= 
	//NONTERMINAL:<exports_resident> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_EXPORTS_RESIDENT_TKRESIDENT :
	//<exports_resident> ::= tkResident
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNIT_FILE_TKPOINT :
	//<unit_file> ::= <unit_heading> <interface_part> <implementation_part> <initialization_part> tkPoint
         
		{
			unit_module _unit_module=new unit_module((unit_name)LRParser.GetReductionSyntaxNode(0),(interface_node)LRParser.GetReductionSyntaxNode(1),(implementation_node)LRParser.GetReductionSyntaxNode(2),((initfinal_part)LRParser.GetReductionSyntaxNode(3)).initialization_sect,((initfinal_part)LRParser.GetReductionSyntaxNode(3)).finalization_sect);
			
								_unit_module.Language = LanguageId.PascalABCNET;
                                                                parsertools.create_source_context(_unit_module,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
								
			return _unit_module;
		}

	case (int)RuleConstants.RULE_UNIT_FILE_TKPOINT2 :
	//<unit_file> ::= <unit_heading> <abc_interface_part> <initialization_part> tkPoint
         
		{
			unit_module _unit_module=new unit_module((unit_name)LRParser.GetReductionSyntaxNode(0),(interface_node)LRParser.GetReductionSyntaxNode(1),null,((initfinal_part)LRParser.GetReductionSyntaxNode(2)).initialization_sect,((initfinal_part)LRParser.GetReductionSyntaxNode(2)).finalization_sect);
			
								_unit_module.Language = LanguageId.PascalABCNET;
                                                                parsertools.create_source_context(_unit_module,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
								
			return _unit_module;
		}

	case (int)RuleConstants.RULE_UNIT_HEADING_TKSEMICOLON :
	//<unit_heading> ::= <unit_key_word> <unit_name> tkSemiColon <opt_head_compiler_directives>
         
		{
			unit_name _unit_name=new unit_name((ident)LRParser.GetReductionSyntaxNode(1),UnitHeaderKeyword.Unit);
			 
								if(((ident)LRParser.GetReductionSyntaxNode(0)).name.ToLower()=="library")
									_unit_name.HeaderKeyword=UnitHeaderKeyword.Library;
								parsertools.create_source_context(_unit_name,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _unit_name;
		}

	case (int)RuleConstants.RULE_UNIT_KEY_WORD_TKUNIT :
	//<unit_key_word> ::= tkUnit
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNIT_KEY_WORD_TKLIBRARY :
	//<unit_key_word> ::= tkLibrary
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNIT_NAME_TKIDENTIFIER :
	//<unit_name> ::= tkIdentifier
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INTERFACE_PART_TKINTERFACE :
	//<interface_part> ::= tkInterface <uses_clause> <using_clause> <int_decl_sect_list>
         
		{
			interface_node _interface_node=new interface_node();
			 
								_interface_node.uses_modules=LRParser.GetReductionSyntaxNode(1) as uses_list;
								_interface_node.using_namespaces=LRParser.GetReductionSyntaxNode(2) as using_list;
								_interface_node.interface_definitions=LRParser.GetReductionSyntaxNode(3) as declarations;
								parsertools.create_source_context(_interface_node,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));
								
			return _interface_node;
		}

	case (int)RuleConstants.RULE_IMPLEMENTATION_PART_TKIMPLEMENTATION :
	//<implementation_part> ::= tkImplementation <uses_clause> <using_clause> <impl_decl_sect_list>
         
		{
			implementation_node _implementation_node=new implementation_node();
			
								_implementation_node.uses_modules=LRParser.GetReductionSyntaxNode(1) as uses_list;
								_implementation_node.using_namespaces=LRParser.GetReductionSyntaxNode(2) as using_list;
								_implementation_node.implementation_definitions=LRParser.GetReductionSyntaxNode(3) as declarations;
								parsertools.create_source_context(_implementation_node,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));
								
			return _implementation_node;
		}

	case (int)RuleConstants.RULE_ABC_INTERFACE_PART :
	//<abc_interface_part> ::= <uses_clause> <using_clause> <impl_decl_sect_list>
         
		{
			interface_node _interface_node=new interface_node();
			
								_interface_node.uses_modules=LRParser.GetReductionSyntaxNode(0) as uses_list;
								_interface_node.using_namespaces=LRParser.GetReductionSyntaxNode(1) as using_list;
								_interface_node.interface_definitions=LRParser.GetReductionSyntaxNode(2) as declarations;
								object lt=parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(2));							
								object rt=parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0));							
								if (lt!=null)parsertools.create_source_context(_interface_node,lt,rt);
								
			return _interface_node;
		}

	case (int)RuleConstants.RULE_INITIALIZATION_PART_TKEND :
	//<initialization_part> ::= tkEnd
         
		{
			initfinal_part _initfinal_part=new initfinal_part();
			
			return _initfinal_part;
		}

	case (int)RuleConstants.RULE_INITIALIZATION_PART_TKINITIALIZATION_TKEND :
	//<initialization_part> ::= tkInitialization <stmt_list> tkEnd
         
		{
			initfinal_part _initfinal_part=new initfinal_part((statement_list)LRParser.GetReductionSyntaxNode(1),null);
			
								((statement_list)LRParser.GetReductionSyntaxNode(1)).left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(0);
								((statement_list)LRParser.GetReductionSyntaxNode(1)).right_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(2);
								
			return _initfinal_part;
		}

	case (int)RuleConstants.RULE_INITIALIZATION_PART_TKINITIALIZATION_TKFINALIZATION_TKEND :
	//<initialization_part> ::= tkInitialization <stmt_list> tkFinalization <stmt_list> tkEnd
         
		{
			initfinal_part _initfinal_part=new initfinal_part((statement_list)LRParser.GetReductionSyntaxNode(1),(statement_list)LRParser.GetReductionSyntaxNode(3));
			
								((statement_list)LRParser.GetReductionSyntaxNode(1)).left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(0);
								((statement_list)LRParser.GetReductionSyntaxNode(3)).left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(2);
								((statement_list)LRParser.GetReductionSyntaxNode(3)).right_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(4);
								
			return _initfinal_part;
		}

	case (int)RuleConstants.RULE_INITIALIZATION_PART_TKBEGIN_TKEND :
	//<initialization_part> ::= tkBegin <stmt_list> tkEnd
         
		{
			initfinal_part _initfinal_part=new initfinal_part((statement_list)LRParser.GetReductionSyntaxNode(1),null);
			
								((statement_list)LRParser.GetReductionSyntaxNode(1)).left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(0);
								((statement_list)LRParser.GetReductionSyntaxNode(1)).right_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(2);
								
			return _initfinal_part;
		}

	case (int)RuleConstants.RULE_PACKAGE_FILE_TKPACKAGE_TKSEMICOLON_TKEND_TKPOINT :
	//<package_file> ::= tkPackage <package_name> tkSemiColon <requires_clause> <contains_clause> tkEnd tkPoint
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<package_file> ::= tkPackage <package_name> tkSemiColon <requires_clause> <contains_clause> tkEnd tkPoint"));}return null;
	case (int)RuleConstants.RULE_PACKAGE_NAME :
	//<package_name> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_REQUIRES_CLAUSE :
	//<requires_clause> ::= 
	//NONTERMINAL:<requires_clause> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_REQUIRES_CLAUSE_TKREQUIRES :
	//<requires_clause> ::= tkRequires
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_REQUIRES_CLAUSE_TKREQUIRES_TKSEMICOLON :
	//<requires_clause> ::= tkRequires <main_used_units_list> tkSemiColon
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<requires_clause> ::= tkRequires <main_used_units_list> tkSemiColon"));}return null;
	case (int)RuleConstants.RULE_CONTAINS_CLAUSE :
	//<contains_clause> ::= 
	//NONTERMINAL:<contains_clause> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_CONTAINS_CLAUSE_TKCONTAINS :
	//<contains_clause> ::= tkContains
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONTAINS_CLAUSE_TKCONTAINS_TKSEMICOLON :
	//<contains_clause> ::= tkContains <main_used_units_list> tkSemiColon
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<contains_clause> ::= tkContains <main_used_units_list> tkSemiColon"));}return null;
	case (int)RuleConstants.RULE_INT_DECL_SECT_LIST :
	//<int_decl_sect_list> ::= <int_decl_sect_list1> <empty>
if (((declarations)LRParser.GetReductionSyntaxNode(0)).defs.Count>0) return LRParser.GetReductionSyntaxNode(0); return null;
	case (int)RuleConstants.RULE_INT_DECL_SECT_LIST1 :
	//<int_decl_sect_list1> ::= <empty> <empty>
         
		{
			declarations _declarations=new declarations();
			
			return _declarations;
		}

	case (int)RuleConstants.RULE_INT_DECL_SECT_LIST12 :
	//<int_decl_sect_list1> ::= <int_decl_sect_list1> <int_decl_sect>
         
		{
			declarations _declarations=(declarations)LRParser.GetReductionSyntaxNode(0);
							_declarations.defs.Add((declaration)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_declarations,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)),LRParser.GetReductionSyntaxNode(1));
							
			return _declarations;
		}

	case (int)RuleConstants.RULE_IMPL_DECL_SECT_LIST :
	//<impl_decl_sect_list> ::= <impl_decl_sect_list1> <empty>
if (((declarations)LRParser.GetReductionSyntaxNode(0)).defs.Count>0) return LRParser.GetReductionSyntaxNode(0); return null;
	case (int)RuleConstants.RULE_IMPL_DECL_SECT_LIST1 :
	//<impl_decl_sect_list1> ::= <empty> <empty>
         
		{
			declarations _declarations=new declarations();
			
			return _declarations;
		}

	case (int)RuleConstants.RULE_IMPL_DECL_SECT_LIST12 :
	//<impl_decl_sect_list1> ::= <impl_decl_sect_list1> <impl_decl_sect>
         
		{
			declarations _declarations=(declarations)LRParser.GetReductionSyntaxNode(0);
							_declarations.defs.Add((declaration)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_declarations,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)),LRParser.GetReductionSyntaxNode(1));
							
			return _declarations;
		}

	case (int)RuleConstants.RULE_ABC_DECL_SECT_LIST :
	//<abc_decl_sect_list> ::= <abc_decl_sect_list1> <empty>
if (((declarations)LRParser.GetReductionSyntaxNode(0)).defs.Count>0) return LRParser.GetReductionSyntaxNode(0); return null;
	case (int)RuleConstants.RULE_ABC_DECL_SECT_LIST1 :
	//<abc_decl_sect_list1> ::= <empty> <empty>
         
		{
			declarations _declarations=new declarations();
			
			return _declarations;
		}

	case (int)RuleConstants.RULE_ABC_DECL_SECT_LIST12 :
	//<abc_decl_sect_list1> ::= <abc_decl_sect_list1> <abc_decl_sect>
         
		{
			declarations _declarations=(declarations)LRParser.GetReductionSyntaxNode(0);
							_declarations.defs.Add((declaration)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_declarations,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)),LRParser.GetReductionSyntaxNode(1));
							
			return _declarations;
		}

	case (int)RuleConstants.RULE_INT_DECL_SECT :
	//<int_decl_sect> ::= <const_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INT_DECL_SECT2 :
	//<int_decl_sect> ::= <res_str_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INT_DECL_SECT3 :
	//<int_decl_sect> ::= <type_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INT_DECL_SECT4 :
	//<int_decl_sect> ::= <var_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INT_DECL_SECT5 :
	//<int_decl_sect> ::= <int_proc_heading>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INT_DECL_SECT6 :
	//<int_decl_sect> ::= <int_func_heading>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IMPL_DECL_SECT :
	//<impl_decl_sect> ::= <label_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IMPL_DECL_SECT2 :
	//<impl_decl_sect> ::= <const_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IMPL_DECL_SECT3 :
	//<impl_decl_sect> ::= <res_str_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IMPL_DECL_SECT4 :
	//<impl_decl_sect> ::= <type_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IMPL_DECL_SECT5 :
	//<impl_decl_sect> ::= <var_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IMPL_DECL_SECT6 :
	//<impl_decl_sect> ::= <proc_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IMPL_DECL_SECT7 :
	//<impl_decl_sect> ::= <func_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IMPL_DECL_SECT8 :
	//<impl_decl_sect> ::= <constructor_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IMPL_DECL_SECT9 :
	//<impl_decl_sect> ::= <destructor_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_DECL_SECT :
	//<abc_decl_sect> ::= <label_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_DECL_SECT2 :
	//<abc_decl_sect> ::= <const_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_DECL_SECT3 :
	//<abc_decl_sect> ::= <res_str_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_DECL_SECT4 :
	//<abc_decl_sect> ::= <type_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_DECL_SECT5 :
	//<abc_decl_sect> ::= <var_decl_sect>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INT_PROC_HEADING :
	//<int_proc_heading> ::= <proc_heading>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INT_PROC_HEADING_TKFORWARD_TKSEMICOLON :
	//<int_proc_heading> ::= <proc_heading> tkForward tkSemiColon
         
		{
			procedure_header _procedure_header;
			 _procedure_header=(LRParser.GetReductionSyntaxNode(0) as procedure_header);
							if (_procedure_header.proc_attributes==null) _procedure_header.proc_attributes=new procedure_attributes_list();
							_procedure_header.proc_attributes.proc_attributes.Add((procedure_attribute)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_procedure_header.proc_attributes,parsertools.sc_not_null(_procedure_header.proc_attributes,LRParser.GetReductionSyntaxNode(1)),LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_procedure_header,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _procedure_header;
		}

	case (int)RuleConstants.RULE_INT_FUNC_HEADING :
	//<int_func_heading> ::= <func_heading>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INT_FUNC_HEADING_TKFORWARD_TKSEMICOLON :
	//<int_func_heading> ::= <func_heading> tkForward tkSemiColon
         
		{
			procedure_header _procedure_header;
			 _procedure_header=(LRParser.GetReductionSyntaxNode(0) as procedure_header);
							if (_procedure_header.proc_attributes==null) _procedure_header.proc_attributes=new procedure_attributes_list();
							_procedure_header.proc_attributes.proc_attributes.Add((procedure_attribute)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_procedure_header.proc_attributes,parsertools.sc_not_null(_procedure_header.proc_attributes,LRParser.GetReductionSyntaxNode(1)),LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_procedure_header,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _procedure_header;
		}

	case (int)RuleConstants.RULE_LABEL_DECL_SECT_TKLABEL_TKSEMICOLON :
	//<label_decl_sect> ::= tkLabel <label_list> tkSemiColon
         
		{
			label_definitions _label_definitions=new label_definitions((ident_list)LRParser.GetReductionSyntaxNode(1));
			
							parsertools.create_source_context(_label_definitions,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _label_definitions;
		}

	case (int)RuleConstants.RULE_LABEL_LIST :
	//<label_list> ::= <label_name> <empty>
         
		//TemplateList for ident_list (create)
		{
			ident_list _ident_list=new ident_list();
			_ident_list.source_context=((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
			_ident_list.idents.Add((ident)LRParser.GetReductionSyntaxNode(0));
			return _ident_list;
		}

	case (int)RuleConstants.RULE_LABEL_LIST_TKCOMMA :
	//<label_list> ::= <label_list> tkComma <label_name>

		//TemplateList for ident_list (add)         
		{
			ident_list _ident_list=(ident_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_ident_list,_ident_list,LRParser.GetReductionSyntaxNode(2));
			_ident_list.idents.Add(LRParser.GetReductionSyntaxNode(2) as ident);
			return _ident_list;
		}

	case (int)RuleConstants.RULE_LABEL_NAME_TKINTEGER :
	//<label_name> ::= tkInteger <empty>
         
		{
			ident _ident=new ident();
			
								if(LRParser.GetReductionSyntaxNode(0) is int32_const)
									_ident.name = ((int32_const)LRParser.GetReductionSyntaxNode(0)).val.ToString();
								else
								if(LRParser.GetReductionSyntaxNode(0) is int64_const)
									_ident.name = ((int64_const)LRParser.GetReductionSyntaxNode(0)).val.ToString();
								else
									_ident.name = ((uint64_const)LRParser.GetReductionSyntaxNode(0)).val.ToString();
								parsertools.assign_source_context(_ident,LRParser.GetReductionSyntaxNode(0));
			return _ident;
		}

	case (int)RuleConstants.RULE_LABEL_NAME_TKFLOAT :
	//<label_name> ::= tkFloat <empty>
         
		{
			ident _ident=new ident(((double_const)LRParser.GetReductionSyntaxNode(0)).val.ToString());
			 parsertools.assign_source_context(_ident,LRParser.GetReductionSyntaxNode(0));
			return _ident;
		}

	case (int)RuleConstants.RULE_LABEL_NAME :
	//<label_name> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_DECL_SECT_TKCONST :
	//<const_decl_sect> ::= tkConst <const_decl>
         
		{
			consts_definitions_list _consts_definitions_list=new consts_definitions_list();
			
							_consts_definitions_list.const_defs.Add((const_definition)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_consts_definitions_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _consts_definitions_list;
		}

	case (int)RuleConstants.RULE_CONST_DECL_SECT :
	//<const_decl_sect> ::= <const_decl_sect> <const_decl>
         
		{
			consts_definitions_list _consts_definitions_list=(consts_definitions_list)LRParser.GetReductionSyntaxNode(0);
							_consts_definitions_list.const_defs.Add((const_definition)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_consts_definitions_list,_consts_definitions_list,LRParser.GetReductionSyntaxNode(1));
			return _consts_definitions_list;
		}

	case (int)RuleConstants.RULE_RES_STR_DECL_SECT_TKRESOURCESTRING :
	//<res_str_decl_sect> ::= tkResourceString <const_decl>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<res_str_decl_sect> ::= tkResourceString <const_decl>"));}return null;
	case (int)RuleConstants.RULE_RES_STR_DECL_SECT :
	//<res_str_decl_sect> ::= <res_str_decl_sect> <const_decl>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<res_str_decl_sect> ::= <res_str_decl_sect> <const_decl>"));}return null;
	case (int)RuleConstants.RULE_TYPE_DECL_SECT_TKTYPE :
	//<type_decl_sect> ::= tkType <type_decl>
         
		{
			type_declarations _type_declarations=new type_declarations();
			
							_type_declarations.types_decl.Add((type_declaration)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_type_declarations,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _type_declarations;
		}

	case (int)RuleConstants.RULE_TYPE_DECL_SECT :
	//<type_decl_sect> ::= <type_decl_sect> <type_decl>
         
		{
			type_declarations _type_declarations=(type_declarations)LRParser.GetReductionSyntaxNode(0);
							_type_declarations.types_decl.Add((type_declaration)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_type_declarations,_type_declarations,LRParser.GetReductionSyntaxNode(1));
			return _type_declarations;
		}

	case (int)RuleConstants.RULE_VAR_DECL_SECT_TKVAR :
	//<var_decl_sect> ::= tkVar <var_decl>
         
		{
			variable_definitions _variable_definitions=new variable_definitions();
			
							_variable_definitions.var_definitions.Add((var_def_statement)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_variable_definitions,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _variable_definitions;
		}

	case (int)RuleConstants.RULE_VAR_DECL_SECT_TKTHREADVAR :
	//<var_decl_sect> ::= tkThreadvar <var_decl>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<var_decl_sect> ::= tkThreadvar <var_decl>"));}return null;
	case (int)RuleConstants.RULE_VAR_DECL_SECT :
	//<var_decl_sect> ::= <var_decl_sect> <var_decl>
         
		{
			variable_definitions _variable_definitions=(variable_definitions)LRParser.GetReductionSyntaxNode(0);
							_variable_definitions.var_definitions.Add((var_def_statement)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_variable_definitions,_variable_definitions,LRParser.GetReductionSyntaxNode(1));
			return _variable_definitions;
		}

	case (int)RuleConstants.RULE_CONST_DECL_TKSEMICOLON :
	//<const_decl> ::= <only_const_decl> tkSemiColon
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ONLY_CONST_DECL_TKEQUAL :
	//<only_const_decl> ::= <const_name> tkEqual <init_const_expr>
         
		{
			simple_const_definition _simple_const_definition=new simple_const_definition();
			
								_simple_const_definition.const_name=(ident)LRParser.GetReductionSyntaxNode(0);
								_simple_const_definition.const_value=(expression)LRParser.GetReductionSyntaxNode(2);
								parsertools.create_source_context(_simple_const_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _simple_const_definition;
		}

	case (int)RuleConstants.RULE_ONLY_CONST_DECL_TKCOLON_TKEQUAL :
	//<only_const_decl> ::= <const_name> tkColon <type_ref> tkEqual <typed_const>
         
		{
			typed_const_definition _typed_const_definition=new typed_const_definition();
			
								_typed_const_definition.const_name=(ident)LRParser.GetReductionSyntaxNode(0);
								_typed_const_definition.const_type=(type_definition)LRParser.GetReductionSyntaxNode(2);
								_typed_const_definition.const_value=(expression)LRParser.GetReductionSyntaxNode(4);
								parsertools.create_source_context(_typed_const_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
			return _typed_const_definition;
		}

	case (int)RuleConstants.RULE_INIT_CONST_EXPR :
	//<init_const_expr> ::= <const_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INIT_CONST_EXPR2 :
	//<init_const_expr> ::= <array_const>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_NAME :
	//<const_name> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_EXPR :
	//<const_expr> ::= <const_simple_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_EXPR2 :
	//<const_expr> ::= <const_simple_expr> <const_relop> <const_simple_expr>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_CONST_RELOP_TKEQUAL :
	//<const_relop> ::= tkEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_RELOP_TKNOTEQUAL :
	//<const_relop> ::= tkNotEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_RELOP_TKLOWER :
	//<const_relop> ::= tkLower
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_RELOP_TKGREATER :
	//<const_relop> ::= tkGreater
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_RELOP_TKLOWEREQUAL :
	//<const_relop> ::= tkLowerEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_RELOP_TKGREATEREQUAL :
	//<const_relop> ::= tkGreaterEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_RELOP_TKIN :
	//<const_relop> ::= tkIn
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_SIMPLE_EXPR :
	//<const_simple_expr> ::= <const_term>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_SIMPLE_EXPR2 :
	//<const_simple_expr> ::= <const_simple_expr> <const_addop> <const_term>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_CONST_ADDOP_TKPLUS :
	//<const_addop> ::= tkPlus
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_ADDOP_TKMINUS :
	//<const_addop> ::= tkMinus
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_ADDOP_TKOR :
	//<const_addop> ::= tkOr
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_ADDOP_TKXOR :
	//<const_addop> ::= tkXor
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_TERM :
	//<const_term> ::= <const_factor>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_TERM2 :
	//<const_term> ::= <const_term> <const_mulop> <const_factor>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_CONST_MULOP_TKSTAR :
	//<const_mulop> ::= tkStar
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_MULOP_TKSLASH :
	//<const_mulop> ::= tkSlash
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_MULOP_TKDIV :
	//<const_mulop> ::= tkDiv
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_MULOP_TKMOD :
	//<const_mulop> ::= tkMod
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_MULOP_TKSHL :
	//<const_mulop> ::= tkShl
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_MULOP_TKSHR :
	//<const_mulop> ::= tkShr
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_MULOP_TKAND :
	//<const_mulop> ::= tkAnd
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_FACTOR :
	//<const_factor> ::= <const_variable>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_FACTOR2 :
	//<const_factor> ::= <const_set>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_FACTOR3 :
	//<const_factor> ::= <unsigned_number>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_FACTOR4 :
	//<const_factor> ::= <literal>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_FACTOR_TKNIL :
	//<const_factor> ::= tkNil <empty>
         
		{
			nil_const _nil_const=new nil_const();
			 parsertools.create_source_context(_nil_const,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _nil_const;
		}

	case (int)RuleConstants.RULE_CONST_FACTOR_TKADDRESSOF :
	//<const_factor> ::= tkAddressOf <const_factor>
         
		{
			get_address _get_address=new get_address((addressed_value)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_get_address,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _get_address;
		}

	case (int)RuleConstants.RULE_CONST_FACTOR_TKROUNDOPEN_TKROUNDCLOSE :
	//<const_factor> ::= tkRoundOpen <const_expr> tkRoundClose
 return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_CONST_FACTOR_TKNOT :
	//<const_factor> ::= tkNot <const_factor>
         
		{
			un_expr _un_expr=new un_expr((expression)LRParser.GetReductionSyntaxNode(1),((op_type_node)LRParser.GetReductionSyntaxNode(0)).type);
			parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			
			return _un_expr;
		}

	case (int)RuleConstants.RULE_CONST_FACTOR5 :
	//<const_factor> ::= <sign> <const_factor>
         
		{
			un_expr _un_expr=new un_expr((expression)LRParser.GetReductionSyntaxNode(1),((op_type_node)LRParser.GetReductionSyntaxNode(0)).type);
			parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			
			return _un_expr;
		}

	case (int)RuleConstants.RULE_CONST_FACTOR_TKDEREF :
	//<const_factor> ::= tkDeref <const_factor>
         
		{
			roof_dereference _roof_dereference=new roof_dereference();
			 
								_roof_dereference.dereferencing_value=(addressed_value)LRParser.GetReductionSyntaxNode(1);
								parsertools.create_source_context(_roof_dereference,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _roof_dereference;
		}

	case (int)RuleConstants.RULE_CONST_SET_TKSQUAREOPEN_TKSQUARECLOSE :
	//<const_set> ::= tkSquareOpen <const_elem_list> tkSquareClose
         
		{
			pascal_set_constant _pascal_set_constant=new pascal_set_constant(LRParser.GetReductionSyntaxNode(1) as expression_list);
			
								parsertools.create_source_context(_pascal_set_constant,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _pascal_set_constant;
		}

	case (int)RuleConstants.RULE_SIGN_TKPLUS :
	//<sign> ::= tkPlus
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIGN_TKMINUS :
	//<sign> ::= tkMinus
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_VARIABLE :
	//<const_variable> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_VARIABLE2 :
	//<const_variable> ::= <const_variable> <const_variable_2>
if (LRParser.GetReductionSyntaxNode(1) is dereference) {
							  ((dereference)LRParser.GetReductionSyntaxNode(1)).dereferencing_value=(addressed_value)LRParser.GetReductionSyntaxNode(0);
							  parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
							}
							if (LRParser.GetReductionSyntaxNode(1) is dot_node) {
							  ((dot_node)LRParser.GetReductionSyntaxNode(1)).left=(addressed_value)LRParser.GetReductionSyntaxNode(0);
							  parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),((dot_node)LRParser.GetReductionSyntaxNode(1)).right);
							}
							return LRParser.GetReductionSyntaxNode(1);
							
	case (int)RuleConstants.RULE_CONST_VARIABLE_2_TKPOINT :
	//<const_variable_2> ::= tkPoint <identifier_or_keyword>
         
		{
			dot_node _dot_node=new dot_node(null,(addressed_value)LRParser.GetReductionSyntaxNode(1));
			
			return _dot_node;
		}

	case (int)RuleConstants.RULE_CONST_VARIABLE_2_TKDEREF :
	//<const_variable_2> ::= tkDeref <empty>
         
		{
			roof_dereference _roof_dereference=new roof_dereference();
			 parsertools.assign_source_context(_roof_dereference,LRParser.GetReductionSyntaxNode(0));
			return _roof_dereference;
		}

	case (int)RuleConstants.RULE_CONST_VARIABLE_2_TKROUNDOPEN_TKROUNDCLOSE :
	//<const_variable_2> ::= tkRoundOpen <const_func_expr_list> tkRoundClose
         
		{
			method_call _method_call=new method_call((expression_list)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_method_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _method_call;
		}

	case (int)RuleConstants.RULE_CONST_VARIABLE_2_TKSQUAREOPEN_TKSQUARECLOSE :
	//<const_variable_2> ::= tkSquareOpen <const_elem_list> tkSquareClose
         
		{
			indexer _indexer=new indexer((expression_list)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_indexer,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _indexer;
		}

	case (int)RuleConstants.RULE_CONST_FUNC_EXPR_LIST :
	//<const_func_expr_list> ::= <const_expr> <empty>
         
		//TemplateList for expression_list (create)
		{
			expression_list _expression_list=new expression_list();
			_expression_list.source_context=((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
			_expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
			return _expression_list;
		}

	case (int)RuleConstants.RULE_CONST_FUNC_EXPR_LIST_TKCOMMA :
	//<const_func_expr_list> ::= <const_func_expr_list> tkComma <const_expr>

		//TemplateList for expression_list (add)         
		{
			expression_list _expression_list=(expression_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_expression_list,_expression_list,LRParser.GetReductionSyntaxNode(2));
			_expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);
			return _expression_list;
		}

	case (int)RuleConstants.RULE_CONST_ELEM_LIST :
	//<const_elem_list> ::= <const_elem_list1>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_ELEM_LIST2 :
	//<const_elem_list> ::= 
	//NONTERMINAL:<const_elem_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_CONST_ELEM_LIST1 :
	//<const_elem_list1> ::= <const_elem> <empty>
         
		//TemplateList for expression_list (create)
		{
			expression_list _expression_list=new expression_list();
			_expression_list.source_context=((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
			_expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
			return _expression_list;
		}

	case (int)RuleConstants.RULE_CONST_ELEM_LIST1_TKCOMMA :
	//<const_elem_list1> ::= <const_elem_list1> tkComma <const_elem>

		//TemplateList for expression_list (add)         
		{
			expression_list _expression_list=(expression_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_expression_list,_expression_list,LRParser.GetReductionSyntaxNode(2));
			_expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);
			return _expression_list;
		}

	case (int)RuleConstants.RULE_CONST_ELEM :
	//<const_elem> ::= <const_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_ELEM_TKDOTDOT :
	//<const_elem> ::= <const_expr> tkDotDot <const_expr>
         
		{
			diapason_expr _diapason_expr=new diapason_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2));
			parsertools.create_source_context(_diapason_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _diapason_expr;
		}

	case (int)RuleConstants.RULE_UNSIGNED_NUMBER_TKINTEGER :
	//<unsigned_number> ::= tkInteger
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNSIGNED_NUMBER_TKHEX :
	//<unsigned_number> ::= tkHex
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNSIGNED_NUMBER_TKFLOAT :
	//<unsigned_number> ::= tkFloat
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPED_CONST :
	//<typed_const> ::= <const_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPED_CONST2 :
	//<typed_const> ::= <array_const>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPED_CONST3 :
	//<typed_const> ::= <record_const>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ARRAY_CONST_TKROUNDOPEN_TKROUNDCLOSE :
	//<array_const> ::= tkRoundOpen <typed_const_list> tkRoundClose
         
		{
			array_const _array_const=new array_const((expression_list)LRParser.GetReductionSyntaxNode(1));
			
							parsertools.create_source_context(_array_const,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _array_const;
		}

	case (int)RuleConstants.RULE_ARRAY_CONST_TKROUNDOPEN_TKROUNDCLOSE2 :
	//<array_const> ::= tkRoundOpen <record_const> tkRoundClose
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_ARRAY_CONST_TKROUNDOPEN_TKROUNDCLOSE3 :
	//<array_const> ::= tkRoundOpen <array_const> tkRoundClose
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_TYPED_CONST_LIST :
	//<typed_const_list> ::= 
	//NONTERMINAL:<typed_const_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_TYPED_CONST_LIST_TKCOMMA :
	//<typed_const_list> ::= <typed_const> tkComma <typed_const>
         
		{
			expression_list _expression_list=new expression_list();
			
							_expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
							_expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(2));
							parsertools.create_source_context(_expression_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _expression_list;
		}

	case (int)RuleConstants.RULE_TYPED_CONST_LIST_TKCOMMA2 :
	//<typed_const_list> ::= <typed_const_list> tkComma <typed_const>
         
		{
			expression_list _expression_list=(expression_list)LRParser.GetReductionSyntaxNode(0);
							parsertools.create_source_context(_expression_list,_expression_list,LRParser.GetReductionSyntaxNode(2));
							_expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(2));
			return _expression_list;
		}

	case (int)RuleConstants.RULE_RECORD_CONST_TKROUNDOPEN_TKROUNDCLOSE :
	//<record_const> ::= tkRoundOpen <const_field_list> tkRoundClose
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_CONST_FIELD_LIST :
	//<const_field_list> ::= <const_field_list_1>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_FIELD_LIST_TKSEMICOLON :
	//<const_field_list> ::= <const_field_list_1> tkSemiColon
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_FIELD_LIST_1 :
	//<const_field_list_1> ::= <const_field> <empty>
         
		//TemplateList for record_const (create)
		{
			record_const _record_const=new record_const();
			_record_const.source_context=((record_const_definition)LRParser.GetReductionSyntaxNode(0)).source_context;
			_record_const.rec_consts.Add((record_const_definition)LRParser.GetReductionSyntaxNode(0));
			return _record_const;
		}

	case (int)RuleConstants.RULE_CONST_FIELD_LIST_1_TKSEMICOLON :
	//<const_field_list_1> ::= <const_field_list_1> tkSemiColon <const_field>

		//TemplateList for record_const (add)         
		{
			record_const _record_const=(record_const)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_record_const,_record_const,LRParser.GetReductionSyntaxNode(2));
			_record_const.rec_consts.Add(LRParser.GetReductionSyntaxNode(2) as record_const_definition);
			return _record_const;
		}

	case (int)RuleConstants.RULE_CONST_FIELD_TKCOLON :
	//<const_field> ::= <const_field_name> tkColon <typed_const>
         
		{
			record_const_definition _record_const_definition=new record_const_definition((ident)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2));
			parsertools.create_source_context(_record_const_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _record_const_definition;
		}

	case (int)RuleConstants.RULE_CONST_FIELD_NAME :
	//<const_field_name> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPE_DECL_TKEQUAL_TKSEMICOLON :
	//<type_decl> ::= <identifier> tkEqual <type_decl_type> tkSemiColon
         
		{
			type_declaration _type_declaration=new type_declaration((ident)LRParser.GetReductionSyntaxNode(0),(type_definition)LRParser.GetReductionSyntaxNode(2));
			parsertools.create_source_context(_type_declaration,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _type_declaration;
		}

	case (int)RuleConstants.RULE_TYPE_DECL_TYPE :
	//<type_decl_type> ::= <type_ref>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPE_DECL_TYPE_TKTYPE :
	//<type_decl_type> ::= tkType <type_ref>
parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_TYPE_DECL_TYPE2 :
	//<type_decl_type> ::= <object_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPE_REF :
	//<type_ref> ::= <simple_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPE_REF2 :
	//<type_ref> ::= <string_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPE_REF3 :
	//<type_ref> ::= <pointer_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPE_REF4 :
	//<type_ref> ::= <structured_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPE_REF5 :
	//<type_ref> ::= <procedural_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPE_REF6 :
	//<type_ref> ::= <template_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TEMPLATE_TYPE :
	//<template_type> ::= <simple_type_identifier> <template_type_params>
         
		{
			template_type_reference _template_type_reference=new template_type_reference((named_type_reference)LRParser.GetReductionSyntaxNode(0),(template_param_list)LRParser.GetReductionSyntaxNode(1));
			parsertools.create_source_context(_template_type_reference,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			
			return _template_type_reference;
		}

	case (int)RuleConstants.RULE_TEMPLATE_TYPE_PARAMS_TKLOWER_TKGREATER :
	//<template_type_params> ::= tkLower <template_param_list> tkGreater
parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_TEMPLATE_PARAM_LIST :
	//<template_param_list> ::= <template_param> <empty>
         
		//TemplateList for template_param_list (create)
		{
			template_param_list _template_param_list=new template_param_list();
			_template_param_list.source_context=((type_definition)LRParser.GetReductionSyntaxNode(0)).source_context;
			_template_param_list.params_list.Add((type_definition)LRParser.GetReductionSyntaxNode(0));
			return _template_param_list;
		}

	case (int)RuleConstants.RULE_TEMPLATE_PARAM_LIST_TKCOMMA :
	//<template_param_list> ::= <template_param_list> tkComma <template_param>

		//TemplateList for template_param_list (add)         
		{
			template_param_list _template_param_list=(template_param_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_template_param_list,_template_param_list,LRParser.GetReductionSyntaxNode(2));
			_template_param_list.params_list.Add(LRParser.GetReductionSyntaxNode(2) as type_definition);
			return _template_param_list;
		}

	case (int)RuleConstants.RULE_TEMPLATE_PARAM :
	//<template_param> ::= <simple_type_identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TEMPLATE_PARAM2 :
	//<template_param> ::= <template_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_TYPE :
	//<simple_type> ::= <simple_type_identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_TYPE_TKDOTDOT :
	//<simple_type> ::= <range_expr> tkDotDot <range_expr>
         
		{
			diapason _diapason=new diapason((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2));
			parsertools.create_source_context(_diapason,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _diapason;
		}

	case (int)RuleConstants.RULE_SIMPLE_TYPE_TKROUNDOPEN_TKROUNDCLOSE :
	//<simple_type> ::= tkRoundOpen <enumeration_id_list> tkRoundClose
         
		{
			enum_type_definition _enum_type_definition=new enum_type_definition((enumerator_list)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_enum_type_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _enum_type_definition;
		}

	case (int)RuleConstants.RULE_RANGE_EXPR :
	//<range_expr> ::= <range_term>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RANGE_EXPR2 :
	//<range_expr> ::= <range_expr> <const_addop> <range_term>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_RANGE_TERM :
	//<range_term> ::= <range_factor>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RANGE_TERM2 :
	//<range_term> ::= <range_term> <const_mulop> <range_factor>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_RANGE_FACTOR :
	//<range_factor> ::= <simple_type_identifier> <empty>
 if(((named_type_reference)LRParser.GetReductionSyntaxNode(0)).names.Count>0)
								return ((named_type_reference)LRParser.GetReductionSyntaxNode(0)).names[0];
						 	   else
							        return null;
							
	case (int)RuleConstants.RULE_RANGE_FACTOR2 :
	//<range_factor> ::= <unsigned_number>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RANGE_FACTOR3 :
	//<range_factor> ::= <sign> <range_factor>
         
		{
			un_expr _un_expr=new un_expr((expression)LRParser.GetReductionSyntaxNode(1),((op_type_node)LRParser.GetReductionSyntaxNode(0)).type);
			parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			
			return _un_expr;
		}

	case (int)RuleConstants.RULE_RANGE_FACTOR4 :
	//<range_factor> ::= <literal>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RANGE_FACTOR_TKROUNDOPEN_TKROUNDCLOSE :
	//<range_factor> ::= <range_factor> tkRoundOpen <const_elem_list> tkRoundClose
         
		{
			method_call _method_call=new method_call((expression_list)LRParser.GetReductionSyntaxNode(2));
			
							_method_call.dereferencing_value=(addressed_value)LRParser.GetReductionSyntaxNode(0);
							parsertools.create_source_context(_method_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
							
			return _method_call;
		}

	case (int)RuleConstants.RULE_RANGE_FACTOR_TKROUNDOPEN_TKROUNDCLOSE2 :
	//<range_factor> ::= tkRoundOpen <const_expr> tkRoundClose
 return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_RANGE_METHODNAME :
	//<range_methodname> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RANGE_METHODNAME_TKPOINT :
	//<range_methodname> ::= <identifier> tkPoint <identifier_or_keyword>
         
		{
			dot_node _dot_node=new dot_node((ident)LRParser.GetReductionSyntaxNode(0),(ident)LRParser.GetReductionSyntaxNode(2));
			parsertools.create_source_context(_dot_node,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _dot_node;
		}

	case (int)RuleConstants.RULE_SIMPLE_TYPE_IDENTIFIER :
	//<simple_type_identifier> ::= <identifier> <empty>
         
		//TemplateList for named_type_reference (create)
		{
			named_type_reference _named_type_reference=new named_type_reference();
			_named_type_reference.source_context=((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
			_named_type_reference.names.Add((ident)LRParser.GetReductionSyntaxNode(0));
			return _named_type_reference;
		}

	case (int)RuleConstants.RULE_SIMPLE_TYPE_IDENTIFIER_TKPOINT :
	//<simple_type_identifier> ::= <simple_type_identifier> tkPoint <identifier_or_keyword>

		//TemplateList for named_type_reference (add)         
		{
			named_type_reference _named_type_reference=(named_type_reference)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_named_type_reference,_named_type_reference,LRParser.GetReductionSyntaxNode(2));
			_named_type_reference.names.Add(LRParser.GetReductionSyntaxNode(2) as ident);
			return _named_type_reference;
		}

	case (int)RuleConstants.RULE_ENUMERATION_ID_LIST_TKCOMMA :
	//<enumeration_id_list> ::= <enumeration_id> tkComma <enumeration_id>
         
		{
			enumerator_list _enumerator_list=new enumerator_list();
			
                                                        _enumerator_list.enumerators.Add((enumerator)LRParser.GetReductionSyntaxNode(0));
                                                        _enumerator_list.enumerators.Add((enumerator)LRParser.GetReductionSyntaxNode(2));
							parsertools.create_source_context(_enumerator_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
							
			return _enumerator_list;
		}

	case (int)RuleConstants.RULE_ENUMERATION_ID_LIST_TKCOMMA2 :
	//<enumeration_id_list> ::= <enumeration_id_list> tkComma <enumeration_id>
         
		{
			enumerator_list _enumerator_list=(enumerator_list)LRParser.GetReductionSyntaxNode(0);
                                                        _enumerator_list.enumerators.Add((enumerator)LRParser.GetReductionSyntaxNode(2));
							parsertools.create_source_context(_enumerator_list,_enumerator_list,LRParser.GetReductionSyntaxNode(2));
							
			return _enumerator_list;
		}

	case (int)RuleConstants.RULE_ENUMERATION_ID :
	//<enumeration_id> ::= <identifier> <empty>
         
		{
			enumerator _enumerator=new enumerator(LRParser.GetReductionSyntaxNode(0) as ident,null);
			parsertools.create_source_context(_enumerator,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
			return _enumerator;
		}

	case (int)RuleConstants.RULE_POINTER_TYPE_TKDEREF :
	//<pointer_type> ::= tkDeref <fptype>
         
		{
			ref_type _ref_type=new ref_type((type_definition)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_ref_type,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _ref_type;
		}

	case (int)RuleConstants.RULE_STRUCTURED_TYPE :
	//<structured_type> ::= <unpacked_structured_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_STRUCTURED_TYPE_TKPACKED :
	//<structured_type> ::= tkPacked <unpacked_structured_type>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<structured_type> ::= tkPacked <unpacked_structured_type>"));}return null;
	case (int)RuleConstants.RULE_UNPACKED_STRUCTURED_TYPE :
	//<unpacked_structured_type> ::= <array_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNPACKED_STRUCTURED_TYPE2 :
	//<unpacked_structured_type> ::= <new_record_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNPACKED_STRUCTURED_TYPE3 :
	//<unpacked_structured_type> ::= <set_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNPACKED_STRUCTURED_TYPE4 :
	//<unpacked_structured_type> ::= <file_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ARRAY_TYPE_TKARRAY_TKSQUAREOPEN_TKSQUARECLOSE_TKOF :
	//<array_type> ::= tkArray tkSquareOpen <simple_type_list> tkSquareClose tkOf <type_ref>
         
		{
			array_type _array_type=new array_type((indexers_types)LRParser.GetReductionSyntaxNode(2),(type_definition)LRParser.GetReductionSyntaxNode(5));
			
										parsertools.create_source_context(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(3));
										parsertools.create_source_context(_array_type,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(5));
										
			return _array_type;
		}

	case (int)RuleConstants.RULE_ARRAY_TYPE :
	//<array_type> ::= <unsized_array_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNSIZED_ARRAY_TYPE_TKARRAY_TKOF :
	//<unsized_array_type> ::= tkArray tkOf <type_ref>
         
		{
			array_type _array_type=new array_type(null,(type_definition)LRParser.GetReductionSyntaxNode(2));
			
										parsertools.create_source_context(_array_type,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
										
			return _array_type;
		}

	case (int)RuleConstants.RULE_SIMPLE_TYPE_LIST :
	//<simple_type_list> ::= <simple_type> <empty>
         
		{
			indexers_types _indexers_types=new indexers_types();
			
							_indexers_types.indexers.Add((type_definition)LRParser.GetReductionSyntaxNode(0));
							
			return _indexers_types;
		}

	case (int)RuleConstants.RULE_SIMPLE_TYPE_LIST_TKCOMMA :
	//<simple_type_list> ::= <simple_type_list> tkComma <simple_type>
         
		{
			indexers_types _indexers_types=(indexers_types)LRParser.GetReductionSyntaxNode(0);
							_indexers_types.indexers.Add((type_definition)LRParser.GetReductionSyntaxNode(2));
							
			return _indexers_types;
		}

	case (int)RuleConstants.RULE_RECORD_TYPE_TKRECORD_TKEND :
	//<record_type> ::= tkRecord <field_list> tkEnd
         
		{
			record_type _record_type=new record_type((record_type_parts)LRParser.GetReductionSyntaxNode(1),null);
			 parsertools.create_source_context(_record_type,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _record_type;
		}

	case (int)RuleConstants.RULE_RECORD_TYPE_TKRECORD_TKEND2 :
	//<record_type> ::= tkRecord tkEnd
         
		{
			record_type _record_type=new record_type(null,null);
			 parsertools.create_source_context(_record_type,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _record_type;
		}

	case (int)RuleConstants.RULE_FIELD_LIST :
	//<field_list> ::= <fixed_part> <empty>
         
		{
			record_type_parts _record_type_parts=new record_type_parts((var_def_list)LRParser.GetReductionSyntaxNode(0),null);
			 parsertools.create_source_context(_record_type_parts,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _record_type_parts;
		}

	case (int)RuleConstants.RULE_FIELD_LIST2 :
	//<field_list> ::= <variant_part> <empty>
         
		{
			record_type_parts _record_type_parts=new record_type_parts(null,(variant_record_type)LRParser.GetReductionSyntaxNode(0));
			 parsertools.create_source_context(_record_type_parts,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _record_type_parts;
		}

	case (int)RuleConstants.RULE_FIELD_LIST_TKSEMICOLON :
	//<field_list> ::= <fixed_part_2> tkSemiColon <variant_part>
         
		{
			record_type_parts _record_type_parts=new record_type_parts((var_def_list)LRParser.GetReductionSyntaxNode(0),(variant_record_type)LRParser.GetReductionSyntaxNode(2));
			parsertools.create_source_context(_record_type_parts,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _record_type_parts;
		}

	case (int)RuleConstants.RULE_FIXED_PART :
	//<fixed_part> ::= <fixed_part_2>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FIXED_PART_TKSEMICOLON :
	//<fixed_part> ::= <fixed_part_2> tkSemiColon
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FIXED_PART_2 :
	//<fixed_part_2> ::= <record_section> <empty>
         
		//TemplateList for var_def_list (create)
		{
			var_def_list _var_def_list=new var_def_list();
			_var_def_list.source_context=((var_def_statement)LRParser.GetReductionSyntaxNode(0)).source_context;
			_var_def_list.vars.Add((var_def_statement)LRParser.GetReductionSyntaxNode(0));
			return _var_def_list;
		}

	case (int)RuleConstants.RULE_FIXED_PART_2_TKSEMICOLON :
	//<fixed_part_2> ::= <fixed_part_2> tkSemiColon <record_section>

		//TemplateList for var_def_list (add)         
		{
			var_def_list _var_def_list=(var_def_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_var_def_list,_var_def_list,LRParser.GetReductionSyntaxNode(2));
			_var_def_list.vars.Add(LRParser.GetReductionSyntaxNode(2) as var_def_statement);
			return _var_def_list;
		}

	case (int)RuleConstants.RULE_RECORD_SECTION_TKCOLON :
	//<record_section> ::= <record_section_id_list> tkColon <type_ref>
         
		{
			var_def_statement _var_def_statement=new var_def_statement((ident_list)LRParser.GetReductionSyntaxNode(0),(type_definition)LRParser.GetReductionSyntaxNode(2),null,definition_attribute.None,false);
			 
							parsertools.create_source_context(_var_def_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
							
			return _var_def_statement;
		}

	case (int)RuleConstants.RULE_RECORD_SECTION_ID_LIST :
	//<record_section_id_list> ::= <record_section_id> <empty>
         
		//TemplateList for ident_list (create)
		{
			ident_list _ident_list=new ident_list();
			_ident_list.source_context=((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
			_ident_list.idents.Add((ident)LRParser.GetReductionSyntaxNode(0));
			return _ident_list;
		}

	case (int)RuleConstants.RULE_RECORD_SECTION_ID_LIST_TKCOMMA :
	//<record_section_id_list> ::= <record_section_id_list> tkComma <record_section_id>

		//TemplateList for ident_list (add)         
		{
			ident_list _ident_list=(ident_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_ident_list,_ident_list,LRParser.GetReductionSyntaxNode(2));
			_ident_list.idents.Add(LRParser.GetReductionSyntaxNode(2) as ident);
			return _ident_list;
		}

	case (int)RuleConstants.RULE_RECORD_SECTION_ID :
	//<record_section_id> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIANT_PART_TKCASE_TKOF :
	//<variant_part> ::= tkCase <tag_field> tkOf <variant_list>
         
		{
			variant_record_type _variant_record_type;
			 _variant_record_type=(variant_record_type)LRParser.GetReductionSyntaxNode(1);
							_variant_record_type.vars=LRParser.GetReductionSyntaxNode(3) as variant_types;
							parsertools.create_source_context(_variant_record_type,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2)));	
							
			return _variant_record_type;
		}

	case (int)RuleConstants.RULE_TAG_FIELD :
	//<tag_field> ::= <tag_field_name> <empty>
         
		{
			variant_record_type _variant_record_type=new variant_record_type((ident)LRParser.GetReductionSyntaxNode(0),null,null);
			
			return _variant_record_type;
		}

	case (int)RuleConstants.RULE_TAG_FIELD_TKCOLON :
	//<tag_field> ::= <tag_field_name> tkColon <tag_field_typename>
         
		{
			variant_record_type _variant_record_type=new variant_record_type((ident)LRParser.GetReductionSyntaxNode(0),(type_definition)LRParser.GetReductionSyntaxNode(2),null);
			
			return _variant_record_type;
		}

	case (int)RuleConstants.RULE_TAG_FIELD_NAME :
	//<tag_field_name> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TAG_FIELD_TYPENAME :
	//<tag_field_typename> ::= <fptype>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIANT_LIST :
	//<variant_list> ::= <variant_list_2>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIANT_LIST_TKSEMICOLON :
	//<variant_list> ::= <variant_list_2> tkSemiColon
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIANT_LIST_2 :
	//<variant_list_2> ::= <variant> <empty>
         
		//TemplateList for variant_types (create)
		{
			variant_types _variant_types=new variant_types();
			_variant_types.source_context=((variant_type)LRParser.GetReductionSyntaxNode(0)).source_context;
			_variant_types.vars.Add((variant_type)LRParser.GetReductionSyntaxNode(0));
			return _variant_types;
		}

	case (int)RuleConstants.RULE_VARIANT_LIST_2_TKSEMICOLON :
	//<variant_list_2> ::= <variant_list_2> tkSemiColon <variant>

		//TemplateList for variant_types (add)         
		{
			variant_types _variant_types=(variant_types)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_variant_types,_variant_types,LRParser.GetReductionSyntaxNode(2));
			_variant_types.vars.Add(LRParser.GetReductionSyntaxNode(2) as variant_type);
			return _variant_types;
		}

	case (int)RuleConstants.RULE_VARIANT_TKCOLON_TKROUNDOPEN_TKROUNDCLOSE :
	//<variant> ::= <case_tag_list> tkColon tkRoundOpen <variant_field_list> tkRoundClose
         
		{
			variant_type _variant_type=new variant_type((expression_list)LRParser.GetReductionSyntaxNode(0),(record_type_parts)LRParser.GetReductionSyntaxNode(3));
			
							parsertools.create_source_context(_variant_type,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));								
							
			return _variant_type;
		}

	case (int)RuleConstants.RULE_VARIANT_FIELD_LIST :
	//<variant_field_list> ::= 
	//NONTERMINAL:<variant_field_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_VARIANT_FIELD_LIST2 :
	//<variant_field_list> ::= <field_list>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CASE_TAG_LIST :
	//<case_tag_list> ::= <const_expr_list>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONST_EXPR_LIST :
	//<const_expr_list> ::= <const_expr> <empty>
         
		//TemplateList for expression_list (create)
		{
			expression_list _expression_list=new expression_list();
			_expression_list.source_context=((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
			_expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
			return _expression_list;
		}

	case (int)RuleConstants.RULE_CONST_EXPR_LIST_TKCOMMA :
	//<const_expr_list> ::= <const_expr_list> tkComma <const_expr>

		//TemplateList for expression_list (add)         
		{
			expression_list _expression_list=(expression_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_expression_list,_expression_list,LRParser.GetReductionSyntaxNode(2));
			_expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);
			return _expression_list;
		}

	case (int)RuleConstants.RULE_SET_TYPE_TKSET_TKOF :
	//<set_type> ::= tkSet tkOf <simple_type>
         
		{
			set_type_definition _set_type_definition=new set_type_definition((type_definition)LRParser.GetReductionSyntaxNode(2));
			
							parsertools.create_source_context(_set_type_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _set_type_definition;
		}

	case (int)RuleConstants.RULE_FILE_TYPE_TKFILE_TKOF :
	//<file_type> ::= tkFile tkOf <type_ref>
         
		{
			file_type _file_type=new file_type((type_definition)LRParser.GetReductionSyntaxNode(2));
			 parsertools.create_source_context(_file_type,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _file_type;
		}

	case (int)RuleConstants.RULE_FILE_TYPE_TKFILE :
	//<file_type> ::= tkFile <empty>
         
		{
			file_type _file_type=new file_type();
			 parsertools.assign_source_context(_file_type,LRParser.GetReductionSyntaxNode(0));
			return _file_type;
		}

	case (int)RuleConstants.RULE_STRING_TYPE_TKIDENTIFIER_TKSQUAREOPEN_TKSQUARECLOSE :
	//<string_type> ::= tkIdentifier tkSquareOpen <const_expr> tkSquareClose
         
		{
			string_num_definition _string_num_definition=new string_num_definition((expression)LRParser.GetReductionSyntaxNode(2),(ident)LRParser.GetReductionSyntaxNode(0));
			
								parsertools.create_source_context(_string_num_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _string_num_definition;
		}

	case (int)RuleConstants.RULE_PROCEDURAL_TYPE :
	//<procedural_type> ::= <procedural_type_kind>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROCEDURAL_TYPE_KIND :
	//<procedural_type_kind> ::= <procedural_type_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROCEDURAL_TYPE_KIND_TKOF :
	//<procedural_type_kind> ::= <procedural_type_decl> tkOf <identifier>
 if ((LRParser.GetReductionSyntaxNode(2) as ident).name.ToLower()=="object")
									((procedure_header)LRParser.GetReductionSyntaxNode(0)).of_object=true;
								else
									errors.Add(new Errors.unexpected_ident(current_file_name,(ident)LRParser.GetReductionSyntaxNode(2),"object",((syntax_tree_node)LRParser.GetReductionSyntaxNode(2)).source_context,(syntax_tree_node)LRParser.GetReductionSyntaxNode(0)));
								return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROCEDURAL_TYPE_DECL_TKPROCEDURE :
	//<procedural_type_decl> ::= tkProcedure <fp_list> <maybe_error>
         
		{
			procedure_header _procedure_header=new procedure_header((formal_parametres)LRParser.GetReductionSyntaxNode(1),null,null,false,false,null,null);
			 
								object rt=LRParser.GetReductionSyntaxNode(0);
								if (LRParser.GetReductionSyntaxNode(1)!=null) rt=LRParser.GetReductionSyntaxNode(1);
								parsertools.create_source_context(_procedure_header,LRParser.GetReductionSyntaxNode(0),rt);
								if(LRParser.GetReductionSyntaxNode(2)!=null)
									(LRParser.GetReductionSyntaxNode(2) as SyntaxError).bad_node=_procedure_header;
								
			return _procedure_header;
		}

	case (int)RuleConstants.RULE_PROCEDURAL_TYPE_DECL_TKFUNCTION_TKCOLON :
	//<procedural_type_decl> ::= tkFunction <fp_list> tkColon <fptype>
         
		{
			function_header _function_header=new function_header();
			 
								if (LRParser.GetReductionSyntaxNode(1)!=null) 
		                                                  _function_header.parametres=(formal_parametres)LRParser.GetReductionSyntaxNode(1);
								if (LRParser.GetReductionSyntaxNode(3)!=null) 
								  _function_header.return_type=(type_definition)LRParser.GetReductionSyntaxNode(3);
								_function_header.of_object=false;
								_function_header.class_keyword=false;
								parsertools.create_source_context(_function_header,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
								
			return _function_header;
		}

	case (int)RuleConstants.RULE_MAYBE_ERROR_TKCOLON :
	//<maybe_error> ::= tkColon <fptype>
 Errors.unexpected_return_value er=new Errors.unexpected_return_value(current_file_name,((syntax_tree_node)LRParser.GetReductionSyntaxNode(1)).source_context,null); errors.Add(er);return er;
	case (int)RuleConstants.RULE_MAYBE_ERROR :
	//<maybe_error> ::= 
	//NONTERMINAL:<maybe_error> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_OBJECT_TYPE :
	//<object_type> ::= <new_object_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_PRIVAT_LIST :
	//<oot_privat_list> ::= 
	//NONTERMINAL:<oot_privat_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_OOT_PRIVAT_LIST_TKPRIVATE :
	//<oot_privat_list> ::= tkPrivate <oot_component_list>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<oot_privat_list> ::= tkPrivate <oot_component_list>"));}return null;
	case (int)RuleConstants.RULE_OOT_COMPONENT_LIST :
	//<oot_component_list> ::= 
	//NONTERMINAL:<oot_component_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_OOT_COMPONENT_LIST2 :
	//<oot_component_list> ::= <oot_field_list>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_COMPONENT_LIST3 :
	//<oot_component_list> ::= <oot_field_list> <oot_method_list>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<oot_component_list> ::= <oot_field_list> <oot_method_list>"));}return null;
	case (int)RuleConstants.RULE_OOT_COMPONENT_LIST4 :
	//<oot_component_list> ::= <oot_method_list>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_SUCCESSOR_TKROUNDOPEN_TKROUNDCLOSE :
	//<oot_successor> ::= tkRoundOpen <oot_typeidentifier> tkRoundClose
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<oot_successor> ::= tkRoundOpen <oot_typeidentifier> tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_OOT_TYPEIDENTIFIER :
	//<oot_typeidentifier> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_FIELD_LIST :
	//<oot_field_list> ::= <oot_field>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_FIELD_LIST2 :
	//<oot_field_list> ::= <oot_field_list> <oot_field>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<oot_field_list> ::= <oot_field_list> <oot_field>"));}return null;
	case (int)RuleConstants.RULE_OOT_FIELD_TKCOLON_TKSEMICOLON :
	//<oot_field> ::= <oot_id_list> tkColon <type_ref> tkSemiColon
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<oot_field> ::= <oot_id_list> tkColon <type_ref> tkSemiColon"));}return null;
	case (int)RuleConstants.RULE_OOT_ID_LIST :
	//<oot_id_list> ::= <oot_field_identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_ID_LIST_TKCOMMA :
	//<oot_id_list> ::= <oot_id_list> tkComma <oot_field_identifier>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<oot_id_list> ::= <oot_id_list> tkComma <oot_field_identifier>"));}return null;
	case (int)RuleConstants.RULE_OOT_FIELD_IDENTIFIER :
	//<oot_field_identifier> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_METHOD_LIST :
	//<oot_method_list> ::= <oot_method>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_METHOD_LIST2 :
	//<oot_method_list> ::= <oot_method_list> <oot_method>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<oot_method_list> ::= <oot_method_list> <oot_method>"));}return null;
	case (int)RuleConstants.RULE_OOT_METHOD :
	//<oot_method> ::= <oot_method_head>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_METHOD_HEAD :
	//<oot_method_head> ::= <proc_heading>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_METHOD_HEAD2 :
	//<oot_method_head> ::= <func_heading>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_METHOD_HEAD3 :
	//<oot_method_head> ::= <oot_constructor_head>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_METHOD_HEAD4 :
	//<oot_method_head> ::= <oot_destructor_head>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OOT_CONSTRUCTOR_HEAD_TKCONSTRUCTOR :
	//<oot_constructor_head> ::= tkConstructor <proc_name> <fp_list> <opt_meth_modificators>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<oot_constructor_head> ::= tkConstructor <proc_name> <fp_list> <opt_meth_modificators>"));}return null;
	case (int)RuleConstants.RULE_OOT_DESTRUCTOR_HEAD_TKDESTRUCTOR :
	//<oot_destructor_head> ::= tkDestructor <proc_name> <fp_list> <opt_meth_modificators>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<oot_destructor_head> ::= tkDestructor <proc_name> <fp_list> <opt_meth_modificators>"));}return null;
	case (int)RuleConstants.RULE_NEW_OBJECT_TYPE :
	//<new_object_type> ::= <not_object_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_OBJECT_TYPE :
	//<not_object_type> ::= <class_attributes> <class_or_interface_keyword> <opt_template_arguments> <opt_base_classes> <opt_where_section> <opt_not_component_list_seq_end>
         
		{
			class_definition _class_definition=new class_definition(LRParser.GetReductionSyntaxNode(3) as named_type_reference_list,LRParser.GetReductionSyntaxNode(5) as class_body,class_keyword.Class,LRParser.GetReductionSyntaxNode(2) as ident_list,LRParser.GetReductionSyntaxNode(4) as where_definition_list, class_attribute.None);
			 
									string kw=(LRParser.GetReductionSyntaxNode(1) as token_info).text.ToLower();
									if(LRParser.GetReductionSyntaxNode(0)!=null)
										_class_definition.attribute=(class_attribute)((LRParser.GetReductionSyntaxNode(0) as token_taginfo).tag);
									if (kw=="record") 
										_class_definition.keyword=class_keyword.Record;
									else
									if (kw=="interface") 
										_class_definition.keyword=class_keyword.Interface;
									else
									if (kw=="i<>") 
										_class_definition.keyword=class_keyword.TemplateInterface;
									else
									if (kw=="r<>") 
										_class_definition.keyword=class_keyword.TemplateRecord;
									else
									if (kw=="c<>") 
										_class_definition.keyword=class_keyword.TemplateClass;
									if (_class_definition.body!=null && _class_definition.body.class_def_blocks!=null && 
										_class_definition.body.class_def_blocks.Count>0 && _class_definition.body.class_def_blocks[0].access_mod==null)
									{
										if(_class_definition.keyword==class_keyword.Class)
		                        						_class_definition.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.internal_modifer);
										else
											_class_definition.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.none);
									}	
									parsertools.create_source_context(_class_definition,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(5),LRParser.GetReductionSyntaxNode(4),LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));
			return _class_definition;
		}

	case (int)RuleConstants.RULE_NEW_RECORD_TYPE_TKEND :
	//<new_record_type> ::= <record_keyword> <opt_template_arguments> <opt_base_classes> <opt_where_section> <not_component_list_seq> tkEnd
         
		{
			class_definition _class_definition=new class_definition(LRParser.GetReductionSyntaxNode(2) as named_type_reference_list,LRParser.GetReductionSyntaxNode(4) as class_body,class_keyword.Record,LRParser.GetReductionSyntaxNode(1) as ident_list,LRParser.GetReductionSyntaxNode(3) as where_definition_list, class_attribute.None);
			 
									if (_class_definition.body!=null && _class_definition.body.class_def_blocks!=null && 
										_class_definition.body.class_def_blocks.Count>0 && _class_definition.body.class_def_blocks[0].access_mod==null)
									{
		                        					_class_definition.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.public_modifer);
									}	
									parsertools.create_source_context(_class_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(5));
			return _class_definition;
		}

	case (int)RuleConstants.RULE_CLASS_ATTRIBUTES_TKFINAL :
	//<class_attributes> ::= tkFinal
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CLASS_ATTRIBUTES :
	//<class_attributes> ::= 
	//NONTERMINAL:<class_attributes> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_CLASS_OR_INTERFACE_KEYWORD_TKCLASS :
	//<class_or_interface_keyword> ::= tkClass
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CLASS_OR_INTERFACE_KEYWORD_TKINTERFACE :
	//<class_or_interface_keyword> ::= tkInterface
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CLASS_OR_INTERFACE_KEYWORD_TKTEMPLATE :
	//<class_or_interface_keyword> ::= tkTemplate
         
		{
			token_info _token_info=(token_info)LRParser.GetReductionSyntaxNode(0);_token_info.text="c<>";
			return _token_info;
		}

	case (int)RuleConstants.RULE_CLASS_OR_INTERFACE_KEYWORD_TKTEMPLATE_TKCLASS :
	//<class_or_interface_keyword> ::= tkTemplate tkClass
         
		{
			token_info _token_info=(token_info)LRParser.GetReductionSyntaxNode(0);_token_info.text="c<>";parsertools.create_source_context(_token_info,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _token_info;
		}

	case (int)RuleConstants.RULE_CLASS_OR_INTERFACE_KEYWORD_TKTEMPLATE_TKRECORD :
	//<class_or_interface_keyword> ::= tkTemplate tkRecord
         
		{
			token_info _token_info=(token_info)LRParser.GetReductionSyntaxNode(0);_token_info.text="r<>";parsertools.create_source_context(_token_info,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _token_info;
		}

	case (int)RuleConstants.RULE_CLASS_OR_INTERFACE_KEYWORD_TKTEMPLATE_TKINTERFACE :
	//<class_or_interface_keyword> ::= tkTemplate tkInterface
         
		{
			token_info _token_info=(token_info)LRParser.GetReductionSyntaxNode(0);_token_info.text="i<>";parsertools.create_source_context(_token_info,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _token_info;
		}

	case (int)RuleConstants.RULE_RECORD_KEYWORD_TKRECORD :
	//<record_keyword> ::= tkRecord
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OPT_NOT_COMPONENT_LIST_SEQ_END :
	//<opt_not_component_list_seq_end> ::= 
	//NONTERMINAL:<opt_not_component_list_seq_end> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_OPT_NOT_COMPONENT_LIST_SEQ_END_TKEND :
	//<opt_not_component_list_seq_end> ::= <not_component_list_seq> tkEnd
parsertools.create_source_context(LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)),LRParser.GetReductionSyntaxNode(1));return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OPT_BASE_CLASSES :
	//<opt_base_classes> ::= 
	//NONTERMINAL:<opt_base_classes> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_OPT_BASE_CLASSES_TKROUNDOPEN_TKROUNDCLOSE :
	//<opt_base_classes> ::= tkRoundOpen <base_classes_names_list> tkRoundClose
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_BASE_CLASSES_NAMES_LIST :
	//<base_classes_names_list> ::= <base_class_name> <empty>
         
		//TemplateList for named_type_reference_list (create)
		{
			named_type_reference_list _named_type_reference_list=new named_type_reference_list();
			_named_type_reference_list.source_context=((named_type_reference)LRParser.GetReductionSyntaxNode(0)).source_context;
			_named_type_reference_list.types.Add((named_type_reference)LRParser.GetReductionSyntaxNode(0));
			return _named_type_reference_list;
		}

	case (int)RuleConstants.RULE_BASE_CLASSES_NAMES_LIST_TKCOMMA :
	//<base_classes_names_list> ::= <base_classes_names_list> tkComma <base_class_name>

		//TemplateList for named_type_reference_list (add)         
		{
			named_type_reference_list _named_type_reference_list=(named_type_reference_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_named_type_reference_list,_named_type_reference_list,LRParser.GetReductionSyntaxNode(2));
			_named_type_reference_list.types.Add(LRParser.GetReductionSyntaxNode(2) as named_type_reference);
			return _named_type_reference_list;
		}

	case (int)RuleConstants.RULE_BASE_CLASS_NAME :
	//<base_class_name> ::= <simple_type_identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_BASE_CLASS_NAME2 :
	//<base_class_name> ::= <template_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OPT_TEMPLATE_ARGUMENTS :
	//<opt_template_arguments> ::= 
	//NONTERMINAL:<opt_template_arguments> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_OPT_TEMPLATE_ARGUMENTS_TKLOWER_TKGREATER :
	//<opt_template_arguments> ::= tkLower <ident_list> tkGreater
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_OPT_WHERE_SECTION :
	//<opt_where_section> ::= 
	//NONTERMINAL:<opt_where_section> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_OPT_WHERE_SECTION2 :
	//<opt_where_section> ::= <where_part_list>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_WHERE_PART_LIST :
	//<where_part_list> ::= <where_part> <empty>
         
		{
			where_definition_list _where_definition_list=new where_definition_list();
			
				    		parsertools.create_source_context(_where_definition_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
						_where_definition_list.defs.Add((where_definition)LRParser.GetReductionSyntaxNode(0));
			return _where_definition_list;
		}

	case (int)RuleConstants.RULE_WHERE_PART_LIST2 :
	//<where_part_list> ::= <where_part_list> <where_part>
         
		{
			where_definition_list _where_definition_list=(where_definition_list)LRParser.GetReductionSyntaxNode(0);
						parsertools.create_source_context(_where_definition_list,_where_definition_list,LRParser.GetReductionSyntaxNode(1));
						_where_definition_list.defs.Add((where_definition)LRParser.GetReductionSyntaxNode(1));
			return _where_definition_list;
		}

	case (int)RuleConstants.RULE_WHERE_PART_TKWHERE_TKCOLON_TKSEMICOLON :
	//<where_part> ::= tkWhere <ident_list> tkColon <type_ref_and_secific_list> tkSemiColon
         
		{
			where_definition _where_definition=new where_definition((ident_list)LRParser.GetReductionSyntaxNode(1),(type_definition_list)LRParser.GetReductionSyntaxNode(3));
			
											parsertools.create_source_context(_where_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
			return _where_definition;
		}

	case (int)RuleConstants.RULE_TYPE_REF_AND_SECIFIC_LIST :
	//<type_ref_and_secific_list> ::= <type_ref_or_secific> <empty>
         
		//TemplateList for type_definition_list (create)
		{
			type_definition_list _type_definition_list=new type_definition_list();
			_type_definition_list.source_context=((type_definition)LRParser.GetReductionSyntaxNode(0)).source_context;
			_type_definition_list.defs.Add((type_definition)LRParser.GetReductionSyntaxNode(0));
			return _type_definition_list;
		}

	case (int)RuleConstants.RULE_TYPE_REF_AND_SECIFIC_LIST_TKCOMMA :
	//<type_ref_and_secific_list> ::= <type_ref_and_secific_list> tkComma <type_ref_or_secific>

		//TemplateList for type_definition_list (add)         
		{
			type_definition_list _type_definition_list=(type_definition_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_type_definition_list,_type_definition_list,LRParser.GetReductionSyntaxNode(2));
			_type_definition_list.defs.Add(LRParser.GetReductionSyntaxNode(2) as type_definition);
			return _type_definition_list;
		}

	case (int)RuleConstants.RULE_TYPE_REF_OR_SECIFIC :
	//<type_ref_or_secific> ::= <type_ref>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPE_REF_OR_SECIFIC_TKCLASS :
	//<type_ref_or_secific> ::= tkClass
         
		{
			declaration_specificator _declaration_specificator=new declaration_specificator(DeclarationSpecificator.WhereDefClass, (LRParser.GetReductionSyntaxNode(0) as token_info).text);
			
					parsertools.assign_source_context(_declaration_specificator,LRParser.GetReductionSyntaxNode(0));
			return _declaration_specificator;
		}

	case (int)RuleConstants.RULE_TYPE_REF_OR_SECIFIC_TKRECORD :
	//<type_ref_or_secific> ::= tkRecord
         
		{
			declaration_specificator _declaration_specificator=new declaration_specificator(DeclarationSpecificator.WhereDefValueType, (LRParser.GetReductionSyntaxNode(0) as token_info).text);
			
					parsertools.assign_source_context(_declaration_specificator,LRParser.GetReductionSyntaxNode(0));
			return _declaration_specificator;
		}

	case (int)RuleConstants.RULE_TYPE_REF_OR_SECIFIC_TKCONSTRUCTOR :
	//<type_ref_or_secific> ::= tkConstructor
         
		{
			declaration_specificator _declaration_specificator=new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, (LRParser.GetReductionSyntaxNode(0) as token_info).text);
			
					parsertools.assign_source_context(_declaration_specificator,LRParser.GetReductionSyntaxNode(0));
			return _declaration_specificator;
		}

	case (int)RuleConstants.RULE_RECORD_COMPONENT_LIST :
	//<record_component_list> ::= <not_component_list> <empty>
         
		{
			class_body _class_body=new class_body();
			
					if (LRParser.GetReductionSyntaxNode(0)!=null) {
		                        access_modifer_node acn=new access_modifer_node(access_modifer.public_modifer);
					((class_members)LRParser.GetReductionSyntaxNode(0)).access_mod = acn;
					_class_body.class_def_blocks.Add((class_members)LRParser.GetReductionSyntaxNode(0));
					parsertools.assign_source_context(_class_body,LRParser.GetReductionSyntaxNode(0));							
					}
					
			return _class_body;
		}

	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST_SEQ :
	//<not_component_list_seq> ::= <not_component_list> <empty>
         
		{
			class_body _class_body=new class_body();
			
					if (LRParser.GetReductionSyntaxNode(0)!=null) {
					_class_body.class_def_blocks.Add((class_members)LRParser.GetReductionSyntaxNode(0));
					parsertools.assign_source_context(_class_body,LRParser.GetReductionSyntaxNode(0));							
					}
					
			return _class_body;
		}

	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST_SEQ2 :
	//<not_component_list_seq> ::= <not_component_list_seq> <ot_visibility_specifier> <not_component_list>
         
		{
			class_body _class_body=(class_body)LRParser.GetReductionSyntaxNode(0);
					class_members cl=(class_members)LRParser.GetReductionSyntaxNode(2);
					if (cl==null) 
					{	
						cl=new class_members();
						parsertools.create_source_context(cl,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1));
					}
					cl.access_mod=(access_modifer_node)LRParser.GetReductionSyntaxNode(1);
					_class_body.class_def_blocks.Add(cl);
					parsertools.create_source_context(_class_body,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)),parsertools.sc_not_null(cl,LRParser.GetReductionSyntaxNode(1)));							
					
			return _class_body;
		}

	case (int)RuleConstants.RULE_OT_VISIBILITY_SPECIFIER_TKINTERNAL :
	//<ot_visibility_specifier> ::= tkInternal
         
		{
			access_modifer_node _access_modifer_node=new access_modifer_node(access_modifer.internal_modifer);
			 parsertools.assign_source_context(_access_modifer_node,LRParser.GetReductionSyntaxNode(0));
			return _access_modifer_node;
		}

	case (int)RuleConstants.RULE_OT_VISIBILITY_SPECIFIER_TKPUBLIC :
	//<ot_visibility_specifier> ::= tkPublic
         
		{
			access_modifer_node _access_modifer_node=new access_modifer_node(access_modifer.public_modifer);
			 parsertools.assign_source_context(_access_modifer_node,LRParser.GetReductionSyntaxNode(0));
			return _access_modifer_node;
		}

	case (int)RuleConstants.RULE_OT_VISIBILITY_SPECIFIER_TKPROTECTED :
	//<ot_visibility_specifier> ::= tkProtected
         
		{
			access_modifer_node _access_modifer_node=new access_modifer_node(access_modifer.protected_modifer);
			 parsertools.assign_source_context(_access_modifer_node,LRParser.GetReductionSyntaxNode(0));
			return _access_modifer_node;
		}

	case (int)RuleConstants.RULE_OT_VISIBILITY_SPECIFIER_TKPRIVATE :
	//<ot_visibility_specifier> ::= tkPrivate
         
		{
			access_modifer_node _access_modifer_node=new access_modifer_node(access_modifer.private_modifer);
			 parsertools.assign_source_context(_access_modifer_node,LRParser.GetReductionSyntaxNode(0));
			return _access_modifer_node;
		}

	case (int)RuleConstants.RULE_NOT_OBJECT_TYPE_IDENTIFIER_LIST :
	//<not_object_type_identifier_list> ::= <simple_type_identifier> <empty>
         
		//TemplateList for named_type_reference_list (create)
		{
			named_type_reference_list _named_type_reference_list=new named_type_reference_list();
			_named_type_reference_list.source_context=((named_type_reference)LRParser.GetReductionSyntaxNode(0)).source_context;
			_named_type_reference_list.types.Add((named_type_reference)LRParser.GetReductionSyntaxNode(0));
			return _named_type_reference_list;
		}

	case (int)RuleConstants.RULE_NOT_OBJECT_TYPE_IDENTIFIER_LIST_TKCOMMA :
	//<not_object_type_identifier_list> ::= <not_object_type_identifier_list> tkComma <simple_type_identifier>

		//TemplateList for named_type_reference_list (add)         
		{
			named_type_reference_list _named_type_reference_list=(named_type_reference_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_named_type_reference_list,_named_type_reference_list,LRParser.GetReductionSyntaxNode(2));
			_named_type_reference_list.types.Add(LRParser.GetReductionSyntaxNode(2) as named_type_reference);
			return _named_type_reference_list;
		}

	case (int)RuleConstants.RULE_IDENT_LIST :
	//<ident_list> ::= <identifier> <empty>
         
		//TemplateList for ident_list (create)
		{
			ident_list _ident_list=new ident_list();
			_ident_list.source_context=((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
			_ident_list.idents.Add((ident)LRParser.GetReductionSyntaxNode(0));
			return _ident_list;
		}

	case (int)RuleConstants.RULE_IDENT_LIST_TKCOMMA :
	//<ident_list> ::= <ident_list> tkComma <identifier>

		//TemplateList for ident_list (add)         
		{
			ident_list _ident_list=(ident_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_ident_list,_ident_list,LRParser.GetReductionSyntaxNode(2));
			_ident_list.idents.Add(LRParser.GetReductionSyntaxNode(2) as ident);
			return _ident_list;
		}

	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST :
	//<not_component_list> ::= <not_guid>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST2 :
	//<not_component_list> ::= <not_guid> <not_component_list_1> <opt_semicolon>
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST3 :
	//<not_component_list> ::= <not_guid> <not_component_list_2>
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST_TKSEMICOLON :
	//<not_component_list> ::= <not_guid> <not_component_list_1> tkSemiColon <not_component_list_2>
         
		{
			class_members _class_members;
			 _class_members=(class_members)LRParser.GetReductionSyntaxNode(1);
							for (int i=0;i<((class_members)LRParser.GetReductionSyntaxNode(3)).members.Count;i++)
								_class_members.members.Add(((class_members)LRParser.GetReductionSyntaxNode(3)).members[i]);
							parsertools.create_source_context(_class_members,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(3));							
							
			return _class_members;
		}

	case (int)RuleConstants.RULE_OPT_SEMICOLON :
	//<opt_semicolon> ::= 
	//NONTERMINAL:<opt_semicolon> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_OPT_SEMICOLON_TKSEMICOLON :
	//<opt_semicolon> ::= tkSemiColon
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_GUID :
	//<not_guid> ::= 
	//NONTERMINAL:<not_guid> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST_1 :
	//<not_component_list_1> ::= <filed_or_const_definition> <empty>
         
		{
			class_members _class_members=new class_members();
			
							_class_members.members.Add((declaration)LRParser.GetReductionSyntaxNode(0));
							parsertools.assign_source_context(_class_members,LRParser.GetReductionSyntaxNode(0));							
							
			return _class_members;
		}

	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST_1_TKSEMICOLON :
	//<not_component_list_1> ::= <not_component_list_1> tkSemiColon <filed_or_const_definition_or_am>
         
		{
			class_members _class_members=(class_members)LRParser.GetReductionSyntaxNode(0);
							if(LRParser.GetReductionSyntaxNode(2) is declaration)
								_class_members.members.Add((declaration)LRParser.GetReductionSyntaxNode(2));
							else
								(_class_members.members[_class_members.members.Count-1] as var_def_statement).var_attr=definition_attribute.Static;
							parsertools.create_source_context(_class_members,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));							
							
			return _class_members;
		}

	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST_2 :
	//<not_component_list_2> ::= <not_method_definition> <empty>
         
		{
			class_members _class_members=new class_members();
			
							_class_members.members.Add((declaration)LRParser.GetReductionSyntaxNode(0));
							parsertools.assign_source_context(_class_members,LRParser.GetReductionSyntaxNode(0));							
							
			return _class_members;
		}

	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST_22 :
	//<not_component_list_2> ::= <not_property_definition> <empty>
         
		{
			class_members _class_members=new class_members();
			
							_class_members.members.Add((declaration)LRParser.GetReductionSyntaxNode(0));
							parsertools.assign_source_context(_class_members,LRParser.GetReductionSyntaxNode(0));							
							
			return _class_members;
		}

	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST_23 :
	//<not_component_list_2> ::= <not_component_list_2> <not_method_definition>
         
		{
			class_members _class_members=(class_members)LRParser.GetReductionSyntaxNode(0);
							_class_members.members.Add((declaration)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_class_members,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));							
							
			return _class_members;
		}

	case (int)RuleConstants.RULE_NOT_COMPONENT_LIST_24 :
	//<not_component_list_2> ::= <not_component_list_2> <not_property_definition>
         
		{
			class_members _class_members=(class_members)LRParser.GetReductionSyntaxNode(0);
							_class_members.members.Add((declaration)LRParser.GetReductionSyntaxNode(1));
							parsertools.create_source_context(_class_members,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));							
							
			return _class_members;
		}

	case (int)RuleConstants.RULE_FILED_OR_CONST_DEFINITION_TKCONST :
	//<filed_or_const_definition> ::= tkConst <only_const_decl>
 parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)); return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_FILED_OR_CONST_DEFINITION :
	//<filed_or_const_definition> ::= <not_field_definition>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FILED_OR_CONST_DEFINITION_OR_AM :
	//<filed_or_const_definition_or_am> ::= <filed_or_const_definition>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FILED_OR_CONST_DEFINITION_OR_AM2 :
	//<filed_or_const_definition_or_am> ::= <field_access_modifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_FIELD_DEFINITION :
	//<not_field_definition> ::= <var_decl_part>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_FIELD_DEFINITION_TKEVENT_TKCOLON :
	//<not_field_definition> ::= tkEvent <var_name_list> tkColon <type_ref>
         
		{
			var_def_statement _var_def_statement=new var_def_statement((ident_list)LRParser.GetReductionSyntaxNode(1),(type_definition)LRParser.GetReductionSyntaxNode(3),null,definition_attribute.None,true);
			
							parsertools.create_source_context(_var_def_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
							
			return _var_def_statement;
		}

	case (int)RuleConstants.RULE_FIELD_ACCESS_MODIFIER_TKSTATIC :
	//<field_access_modifier> ::= tkStatic
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_METHOD_DEFINITION :
	//<not_method_definition> ::= <not_method_heading>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_METHOD_DEFINITION2 :
	//<not_method_definition> ::= <abc_method_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_METHOD_DECL :
	//<abc_method_decl> ::= <abc_proc_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_METHOD_DECL2 :
	//<abc_method_decl> ::= <abc_func_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_METHOD_DECL3 :
	//<abc_method_decl> ::= <abc_constructor_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_METHOD_DECL4 :
	//<abc_method_decl> ::= <abc_destructor_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_METHOD_HEADING_TKCLASS :
	//<not_method_heading> ::= tkClass <proc_heading>
 ((procedure_header)LRParser.GetReductionSyntaxNode(1)).class_keyword=true;return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_NOT_METHOD_HEADING_TKCLASS2 :
	//<not_method_heading> ::= tkClass <func_heading>
 ((procedure_header)LRParser.GetReductionSyntaxNode(1)).class_keyword=true;return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_NOT_METHOD_HEADING :
	//<not_method_heading> ::= <func_heading>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_METHOD_HEADING2 :
	//<not_method_heading> ::= <proc_heading>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_METHOD_HEADING3 :
	//<not_method_heading> ::= <not_constructor_heading>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_METHOD_HEADING4 :
	//<not_method_heading> ::= <not_destructor_heading>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OPTIONAL_QUALIFIED_IDENTIFIER :
	//<optional_qualified_identifier> ::= <qualified_identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OPTIONAL_QUALIFIED_IDENTIFIER2 :
	//<optional_qualified_identifier> ::= 
	//NONTERMINAL:<optional_qualified_identifier> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_NOT_CONSTRUCTOR_HEADING_TKCONSTRUCTOR :
	//<not_constructor_heading> ::= tkConstructor <optional_qualified_identifier> <fp_list> <opt_meth_modificators>
         
		{
			constructor _constructor=new constructor();
			 
								object rt=LRParser.GetReductionSyntaxNode(1);
								_constructor.name=LRParser.GetReductionSyntaxNode(1) as method_name;
								if (LRParser.GetReductionSyntaxNode(2)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(2);
		                                                  _constructor.parametres=(formal_parametres)LRParser.GetReductionSyntaxNode(2);
								}
								if (LRParser.GetReductionSyntaxNode(3)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(3);
								  if (((procedure_attributes_list)LRParser.GetReductionSyntaxNode(3)).proc_attributes.Count>0) 
									_constructor.proc_attributes=(procedure_attributes_list)LRParser.GetReductionSyntaxNode(3);
								}
								parsertools.create_source_context(_constructor,LRParser.GetReductionSyntaxNode(0),rt);
								
			return _constructor;
		}

	case (int)RuleConstants.RULE_NOT_DESTRUCTOR_HEADING_TKDESTRUCTOR :
	//<not_destructor_heading> ::= tkDestructor <optional_qualified_identifier> <fp_list> <opt_meth_modificators>
         
		{
			destructor _destructor=new destructor();
			 
								object rt=LRParser.GetReductionSyntaxNode(1);
								_destructor.name=LRParser.GetReductionSyntaxNode(1) as method_name;
								if (LRParser.GetReductionSyntaxNode(2)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(2);
		                                                  _destructor.parametres=(formal_parametres)LRParser.GetReductionSyntaxNode(2);
								}
								if (LRParser.GetReductionSyntaxNode(3)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(3);
								  if (((procedure_attributes_list)LRParser.GetReductionSyntaxNode(3)).proc_attributes.Count>0) 
									_destructor.proc_attributes=(procedure_attributes_list)LRParser.GetReductionSyntaxNode(3);
								}
								parsertools.create_source_context(_destructor,LRParser.GetReductionSyntaxNode(0),rt);
								
			return _destructor;
		}

	case (int)RuleConstants.RULE_QUALIFIED_IDENTIFIER :
	//<qualified_identifier> ::= <identifier> <empty>
         
		{
			method_name _method_name=new method_name(null,(ident)LRParser.GetReductionSyntaxNode(0),null);
			parsertools.create_source_context(_method_name,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
			return _method_name;
		}

	case (int)RuleConstants.RULE_QUALIFIED_IDENTIFIER2 :
	//<qualified_identifier> ::= <visibility_specifier> <empty>
         
		{
			method_name _method_name=new method_name(null,(ident)LRParser.GetReductionSyntaxNode(0),null);
			parsertools.create_source_context(_method_name,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
			return _method_name;
		}

	case (int)RuleConstants.RULE_QUALIFIED_IDENTIFIER_TKPOINT :
	//<qualified_identifier> ::= <qualified_identifier> tkPoint <identifier>
{
								method_name mn=(method_name)LRParser.GetReductionSyntaxNode(0);
								mn.class_name=mn.meth_name;
								mn.meth_name=(ident)LRParser.GetReductionSyntaxNode(2);
								parsertools.create_source_context(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
								return LRParser.GetReductionSyntaxNode(0);
								}
	case (int)RuleConstants.RULE_QUALIFIED_IDENTIFIER_TKPOINT2 :
	//<qualified_identifier> ::= <qualified_identifier> tkPoint <visibility_specifier>
{
								method_name mn=(method_name)LRParser.GetReductionSyntaxNode(0);
								mn.class_name=mn.meth_name;
								mn.meth_name=(ident)LRParser.GetReductionSyntaxNode(2);
								parsertools.create_source_context(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
								return LRParser.GetReductionSyntaxNode(0);
								}
	case (int)RuleConstants.RULE_NOT_PROPERTY_DEFINITION_TKPROPERTY_TKSEMICOLON :
	//<not_property_definition> ::= tkProperty <qualified_identifier> <not_property_interface> <not_property_specifiers> tkSemiColon <not_array_defaultproperty>
         
		{
			simple_property _simple_property=new simple_property();
			
								_simple_property.property_name=((method_name)LRParser.GetReductionSyntaxNode(1)).meth_name;
								if (LRParser.GetReductionSyntaxNode(2)!=null){
									_simple_property.parameter_list=((property_interface)LRParser.GetReductionSyntaxNode(2)).parameter_list;
									_simple_property.property_type=((property_interface)LRParser.GetReductionSyntaxNode(2)).property_type;
									_simple_property.index_expression=((property_interface)LRParser.GetReductionSyntaxNode(2)).index_expression;
								}
								if (LRParser.GetReductionSyntaxNode(3)!=null) _simple_property.accessors=(property_accessors)LRParser.GetReductionSyntaxNode(3);
								if (LRParser.GetReductionSyntaxNode(5)!=null) _simple_property.array_default=(property_array_default)LRParser.GetReductionSyntaxNode(5);
								parsertools.create_source_context(_simple_property,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(5),LRParser.GetReductionSyntaxNode(4),LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));
								
			return _simple_property;
		}

	case (int)RuleConstants.RULE_NOT_ARRAY_DEFAULTPROPERTY :
	//<not_array_defaultproperty> ::= 
	//NONTERMINAL:<not_array_defaultproperty> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_NOT_ARRAY_DEFAULTPROPERTY_TKDEFAULT_TKSEMICOLON :
	//<not_array_defaultproperty> ::= tkDefault tkSemiColon
         
		{
			property_array_default _property_array_default=new property_array_default();
			 parsertools.create_source_context(_property_array_default,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _property_array_default;
		}

	case (int)RuleConstants.RULE_NOT_PROPERTY_INTERFACE :
	//<not_property_interface> ::= 
	//NONTERMINAL:<not_property_interface> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_NOT_PROPERTY_INTERFACE_TKCOLON :
	//<not_property_interface> ::= <not_property_parameter_list> tkColon <fptype> <not_property_interface_index>
         
		{
			property_interface _property_interface=new property_interface();
			
								_property_interface.parameter_list=(property_parameter_list)LRParser.GetReductionSyntaxNode(0);
								_property_interface.property_type=(type_definition)LRParser.GetReductionSyntaxNode(2);
								_property_interface.index_expression=(expression)LRParser.GetReductionSyntaxNode(3);
								parsertools.create_source_context(_property_interface,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(3)),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));
								
			return _property_interface;
		}

	case (int)RuleConstants.RULE_NOT_PROPERTY_INTERFACE_INDEX :
	//<not_property_interface_index> ::= 
	//NONTERMINAL:<not_property_interface_index> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_NOT_PROPERTY_INTERFACE_INDEX_TKINDEX :
	//<not_property_interface_index> ::= tkIndex <expr>
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_NOT_PROPERTY_PARAMETER_LIST :
	//<not_property_parameter_list> ::= 
	//NONTERMINAL:<not_property_parameter_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_NOT_PROPERTY_PARAMETER_LIST_TKSQUAREOPEN_TKSQUARECLOSE :
	//<not_property_parameter_list> ::= tkSquareOpen <not_parameter_decl_list> tkSquareClose
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_NOT_PARAMETER_DECL_LIST :
	//<not_parameter_decl_list> ::= <not_parameter_decl> <empty>
         
		//TemplateList for property_parameter_list (create)
		{
			property_parameter_list _property_parameter_list=new property_parameter_list();
			_property_parameter_list.source_context=((property_parameter)LRParser.GetReductionSyntaxNode(0)).source_context;
			_property_parameter_list.parameters.Add((property_parameter)LRParser.GetReductionSyntaxNode(0));
			return _property_parameter_list;
		}

	case (int)RuleConstants.RULE_NOT_PARAMETER_DECL_LIST_TKSEMICOLON :
	//<not_parameter_decl_list> ::= <not_parameter_decl_list> tkSemiColon <not_parameter_decl>

		//TemplateList for property_parameter_list (add)         
		{
			property_parameter_list _property_parameter_list=(property_parameter_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_property_parameter_list,_property_parameter_list,LRParser.GetReductionSyntaxNode(2));
			_property_parameter_list.parameters.Add(LRParser.GetReductionSyntaxNode(2) as property_parameter);
			return _property_parameter_list;
		}

	case (int)RuleConstants.RULE_NOT_PARAMETER_DECL_TKCOLON :
	//<not_parameter_decl> ::= <not_parameter_name_list> tkColon <fptype>
         
		{
			property_parameter _property_parameter=new property_parameter((ident_list)LRParser.GetReductionSyntaxNode(0),(type_definition)LRParser.GetReductionSyntaxNode(2));
			parsertools.create_source_context(_property_parameter,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _property_parameter;
		}

	case (int)RuleConstants.RULE_NOT_PARAMETER_DECL_TKCONST_TKCOLON :
	//<not_parameter_decl> ::= tkConst <not_parameter_name_list> tkColon <fptype>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<not_parameter_decl> ::= tkConst <not_parameter_name_list> tkColon <fptype>"));}return null;
	case (int)RuleConstants.RULE_NOT_PARAMETER_DECL_TKVAR_TKCOLON :
	//<not_parameter_decl> ::= tkVar <not_parameter_name_list> tkColon <fptype>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<not_parameter_decl> ::= tkVar <not_parameter_name_list> tkColon <fptype>"));}return null;
	case (int)RuleConstants.RULE_NOT_PARAMETER_DECL_TKOUT_TKCOLON :
	//<not_parameter_decl> ::= tkOut <not_parameter_name_list> tkColon <fptype>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<not_parameter_decl> ::= tkOut <not_parameter_name_list> tkColon <fptype>"));}return null;
	case (int)RuleConstants.RULE_NOT_PARAMETER_NAME_LIST :
	//<not_parameter_name_list> ::= <ident_list>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OPT_IDENTIFIER :
	//<opt_identifier> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OPT_IDENTIFIER2 :
	//<opt_identifier> ::= 
	//NONTERMINAL:<opt_identifier> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_NOT_PROPERTY_SPECIFIERS :
	//<not_property_specifiers> ::= 
	//NONTERMINAL:<not_property_specifiers> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_NOT_PROPERTY_SPECIFIERS_TKREADONLY :
	//<not_property_specifiers> ::= tkReadOnly <not_property_specifiers>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<not_property_specifiers> ::= tkReadOnly <not_property_specifiers>"));}return null;
	case (int)RuleConstants.RULE_NOT_PROPERTY_SPECIFIERS_TKWRITEONLY :
	//<not_property_specifiers> ::= tkWriteOnly <not_property_specifiers>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<not_property_specifiers> ::= tkWriteOnly <not_property_specifiers>"));}return null;
	case (int)RuleConstants.RULE_NOT_PROPERTY_SPECIFIERS_TKDEFAULT :
	//<not_property_specifiers> ::= tkDefault <const_expr> <not_property_specifiers>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<not_property_specifiers> ::= tkDefault <const_expr> <not_property_specifiers>"));}return null;
	case (int)RuleConstants.RULE_NOT_PROPERTY_SPECIFIERS_TKREAD :
	//<not_property_specifiers> ::= tkRead <opt_identifier> <not_property_specifiers>
         
		{
			property_accessors _property_accessors;
			 
								property_accessors _pa=LRParser.GetReductionSyntaxNode(2) as property_accessors;
							        if (_pa==null) {
								_pa=new property_accessors();parsertools.create_source_context(_pa,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));
								}
								_property_accessors=_pa;
								if(LRParser.GetReductionSyntaxNode(1)!=null && ((ident)LRParser.GetReductionSyntaxNode(1)).name.ToLower()=="write")
								{
								_property_accessors.read_accessor=new read_accessor_name(null);
								_property_accessors.write_accessor=new write_accessor_name(null);
								parsertools.create_source_context(_property_accessors.read_accessor,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
								parsertools.create_source_context(_property_accessors.write_accessor,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1));
								parsertools.create_source_context(_property_accessors,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
								}
								else
								{
								_property_accessors.read_accessor=new read_accessor_name((ident)LRParser.GetReductionSyntaxNode(1));								
								parsertools.create_source_context(_property_accessors.read_accessor,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));
								parsertools.create_source_context(_property_accessors,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));
								}
								
			return _property_accessors;
		}

	case (int)RuleConstants.RULE_NOT_PROPERTY_SPECIFIERS_TKWRITE :
	//<not_property_specifiers> ::= tkWrite <opt_identifier> <not_property_specifiers>
         
		{
			property_accessors _property_accessors;
			 
								property_accessors _pa=LRParser.GetReductionSyntaxNode(2) as property_accessors;
							        if (_pa==null) {
								_pa=new property_accessors();parsertools.create_source_context(_pa,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));
								}
								_property_accessors=_pa;
								_property_accessors.write_accessor=new write_accessor_name((ident)LRParser.GetReductionSyntaxNode(1));
								parsertools.create_source_context(_property_accessors.write_accessor,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));
								parsertools.create_source_context(_property_accessors,LRParser.GetReductionSyntaxNode(0),_pa);
								
			return _property_accessors;
		}

	case (int)RuleConstants.RULE_VAR_DECL_TKSEMICOLON :
	//<var_decl> ::= <var_decl_part> tkSemiColon
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VAR_DECL_PART :
	//<var_decl_part> ::= <var_decl_part_normal>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VAR_DECL_PART2 :
	//<var_decl_part> ::= <var_decl_part_assign>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VAR_DECL_PART_TKCOLON_TKASSIGN :
	//<var_decl_part> ::= <var_name_list> tkColon <type_ref> tkAssign <var_init_value_typed>
         
		{
			var_def_statement _var_def_statement=new var_def_statement((ident_list)LRParser.GetReductionSyntaxNode(0),(type_definition)LRParser.GetReductionSyntaxNode(2),(expression)LRParser.GetReductionSyntaxNode(4),definition_attribute.None,false);
			parsertools.create_source_context(_var_def_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
			
			return _var_def_statement;
		}

	case (int)RuleConstants.RULE_VAR_DECL_PART_IN_STMT :
	//<var_decl_part_in_stmt> ::= <var_decl_part>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VAR_DECL_PART_ASSIGN_TKASSIGN :
	//<var_decl_part_assign> ::= <var_name_list> tkAssign <var_init_value>
         
		{
			var_def_statement _var_def_statement=new var_def_statement((ident_list)LRParser.GetReductionSyntaxNode(0),null,(expression)LRParser.GetReductionSyntaxNode(2),definition_attribute.None,false);
			parsertools.create_source_context(_var_def_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _var_def_statement;
		}

	case (int)RuleConstants.RULE_VAR_DECL_PART_NORMAL_TKCOLON :
	//<var_decl_part_normal> ::= <var_name_list> tkColon <type_ref>
         
		{
			var_def_statement _var_def_statement=new var_def_statement((ident_list)LRParser.GetReductionSyntaxNode(0),(type_definition)LRParser.GetReductionSyntaxNode(2),null,definition_attribute.None,false);
			parsertools.create_source_context(_var_def_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _var_def_statement;
		}

	case (int)RuleConstants.RULE_VAR_INIT_VALUE :
	//<var_init_value> ::= <expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VAR_INIT_VALUE_TYPED :
	//<var_init_value_typed> ::= <typed_const>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VAR_INIT_VALUE_TYPED2 :
	//<var_init_value_typed> ::= <new_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VAR_NAME_LIST :
	//<var_name_list> ::= <var_name> <empty>
         
		{
			ident_list _ident_list=new ident_list();
			parsertools.create_source_context(_ident_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
								_ident_list.idents.Add((ident)LRParser.GetReductionSyntaxNode(0));
			return _ident_list;
		}

	case (int)RuleConstants.RULE_VAR_NAME_LIST_TKCOMMA :
	//<var_name_list> ::= <var_name_list> tkComma <var_name>
         
		{
			ident_list _ident_list=(ident_list)LRParser.GetReductionSyntaxNode(0);
								_ident_list.idents.Add((ident)LRParser.GetReductionSyntaxNode(2));
								parsertools.create_source_context(_ident_list,_ident_list,LRParser.GetReductionSyntaxNode(2));
			return _ident_list;
		}

	case (int)RuleConstants.RULE_VAR_NAME :
	//<var_name> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_DECLARED_VAR_NAME :
	//<declared_var_name> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CONSTRUCTOR_DECL :
	//<constructor_decl> ::= <not_constructor_heading> <not_constructor_block_decl>

                //procedure_definition create
		{
			procedure_definition _procedure_definition=new procedure_definition((procedure_header)LRParser.GetReductionSyntaxNode(0),null);
			object rt=LRParser.GetReductionSyntaxNode(0);
			if(LRParser.GetReductionSyntaxNode(1)!=null) {
				rt=LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is proc_block) _procedure_definition.proc_body=(proc_block)LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is procedure_attribute) {
					procedure_header ph=_procedure_definition.proc_header;
					if(ph.proc_attributes==null) {
						ph.proc_attributes=new procedure_attributes_list();
						parsertools.assign_source_context(ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)LRParser.GetReductionSyntaxNode(1));
					parsertools.create_source_context(ph.proc_attributes,ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
				}
			}	
			parsertools.create_source_context(_procedure_definition,LRParser.GetReductionSyntaxNode(0),rt);
			
			return _procedure_definition;
		}

	case (int)RuleConstants.RULE_ABC_CONSTRUCTOR_DECL :
	//<abc_constructor_decl> ::= <not_constructor_heading> <abc_block>

                //procedure_definition create
		{
			procedure_definition _procedure_definition=new procedure_definition((procedure_header)LRParser.GetReductionSyntaxNode(0),null);
			object rt=LRParser.GetReductionSyntaxNode(0);
			if(LRParser.GetReductionSyntaxNode(1)!=null) {
				rt=LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is proc_block) _procedure_definition.proc_body=(proc_block)LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is procedure_attribute) {
					procedure_header ph=_procedure_definition.proc_header;
					if(ph.proc_attributes==null) {
						ph.proc_attributes=new procedure_attributes_list();
						parsertools.assign_source_context(ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)LRParser.GetReductionSyntaxNode(1));
					parsertools.create_source_context(ph.proc_attributes,ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
				}
			}	
			parsertools.create_source_context(_procedure_definition,LRParser.GetReductionSyntaxNode(0),rt);
			
			return _procedure_definition;
		}

	case (int)RuleConstants.RULE_DESTRUCTOR_DECL :
	//<destructor_decl> ::= <not_destructor_heading> <not_constructor_block_decl>

                //procedure_definition create
		{
			procedure_definition _procedure_definition=new procedure_definition((procedure_header)LRParser.GetReductionSyntaxNode(0),null);
			object rt=LRParser.GetReductionSyntaxNode(0);
			if(LRParser.GetReductionSyntaxNode(1)!=null) {
				rt=LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is proc_block) _procedure_definition.proc_body=(proc_block)LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is procedure_attribute) {
					procedure_header ph=_procedure_definition.proc_header;
					if(ph.proc_attributes==null) {
						ph.proc_attributes=new procedure_attributes_list();
						parsertools.assign_source_context(ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)LRParser.GetReductionSyntaxNode(1));
					parsertools.create_source_context(ph.proc_attributes,ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
				}
			}	
			parsertools.create_source_context(_procedure_definition,LRParser.GetReductionSyntaxNode(0),rt);
			
			return _procedure_definition;
		}

	case (int)RuleConstants.RULE_ABC_DESTRUCTOR_DECL :
	//<abc_destructor_decl> ::= <not_destructor_heading> <abc_block>

                //procedure_definition create
		{
			procedure_definition _procedure_definition=new procedure_definition((procedure_header)LRParser.GetReductionSyntaxNode(0),null);
			object rt=LRParser.GetReductionSyntaxNode(0);
			if(LRParser.GetReductionSyntaxNode(1)!=null) {
				rt=LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is proc_block) _procedure_definition.proc_body=(proc_block)LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is procedure_attribute) {
					procedure_header ph=_procedure_definition.proc_header;
					if(ph.proc_attributes==null) {
						ph.proc_attributes=new procedure_attributes_list();
						parsertools.assign_source_context(ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)LRParser.GetReductionSyntaxNode(1));
					parsertools.create_source_context(ph.proc_attributes,ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
				}
			}	
			parsertools.create_source_context(_procedure_definition,LRParser.GetReductionSyntaxNode(0),rt);
			
			return _procedure_definition;
		}

	case (int)RuleConstants.RULE_NOT_CONSTRUCTOR_BLOCK_DECL :
	//<not_constructor_block_decl> ::= <block>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_CONSTRUCTOR_BLOCK_DECL2 :
	//<not_constructor_block_decl> ::= <external_directr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NOT_CONSTRUCTOR_BLOCK_DECL3 :
	//<not_constructor_block_decl> ::= <asm_block>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROC_DECL :
	//<proc_decl> ::= <proc_decl_noclass>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROC_DECL_TKCLASS :
	//<proc_decl> ::= tkClass <proc_decl_noclass>
 ((LRParser.GetReductionSyntaxNode(1) as procedure_definition).proc_header as procedure_header).class_keyword=true;return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_PROC_DECL_NOCLASS :
	//<proc_decl_noclass> ::= <proc_heading> <proc_block>

                //procedure_definition create
		{
			procedure_definition _procedure_definition=new procedure_definition((procedure_header)LRParser.GetReductionSyntaxNode(0),null);
			object rt=LRParser.GetReductionSyntaxNode(0);
			if(LRParser.GetReductionSyntaxNode(1)!=null) {
				rt=LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is proc_block) _procedure_definition.proc_body=(proc_block)LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is procedure_attribute) {
					procedure_header ph=_procedure_definition.proc_header;
					if(ph.proc_attributes==null) {
						ph.proc_attributes=new procedure_attributes_list();
						parsertools.assign_source_context(ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)LRParser.GetReductionSyntaxNode(1));
					parsertools.create_source_context(ph.proc_attributes,ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
				}
			}	
			parsertools.create_source_context(_procedure_definition,LRParser.GetReductionSyntaxNode(0),rt);
			
			return _procedure_definition;
		}

	case (int)RuleConstants.RULE_ABC_PROC_DECL :
	//<abc_proc_decl> ::= <abc_proc_decl_noclass>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_PROC_DECL_TKCLASS :
	//<abc_proc_decl> ::= tkClass <abc_proc_decl_noclass>
 ((LRParser.GetReductionSyntaxNode(1) as procedure_definition).proc_header as procedure_header).class_keyword=true;return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_ABC_PROC_DECL_NOCLASS :
	//<abc_proc_decl_noclass> ::= <proc_heading> <abc_proc_block>

                //procedure_definition create
		{
			procedure_definition _procedure_definition=new procedure_definition((procedure_header)LRParser.GetReductionSyntaxNode(0),null);
			object rt=LRParser.GetReductionSyntaxNode(0);
			if(LRParser.GetReductionSyntaxNode(1)!=null) {
				rt=LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is proc_block) _procedure_definition.proc_body=(proc_block)LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is procedure_attribute) {
					procedure_header ph=_procedure_definition.proc_header;
					if(ph.proc_attributes==null) {
						ph.proc_attributes=new procedure_attributes_list();
						parsertools.assign_source_context(ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)LRParser.GetReductionSyntaxNode(1));
					parsertools.create_source_context(ph.proc_attributes,ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
				}
			}	
			parsertools.create_source_context(_procedure_definition,LRParser.GetReductionSyntaxNode(0),rt);
			
			return _procedure_definition;
		}

	case (int)RuleConstants.RULE_FUNC_DECL :
	//<func_decl> ::= <func_decl_noclass>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FUNC_DECL_TKCLASS :
	//<func_decl> ::= tkClass <func_decl_noclass>
 ((LRParser.GetReductionSyntaxNode(1) as procedure_definition).proc_header as procedure_header).class_keyword=true;return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_FUNC_DECL_NOCLASS :
	//<func_decl_noclass> ::= <func_heading> <func_block>

                //procedure_definition create
		{
			procedure_definition _procedure_definition=new procedure_definition((function_header)LRParser.GetReductionSyntaxNode(0),null);
			object rt=LRParser.GetReductionSyntaxNode(0);
			if(LRParser.GetReductionSyntaxNode(1)!=null) {
				rt=LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is proc_block) _procedure_definition.proc_body=(proc_block)LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is procedure_attribute) {
					procedure_header ph=_procedure_definition.proc_header;
					if(ph.proc_attributes==null) {
						ph.proc_attributes=new procedure_attributes_list();
						parsertools.assign_source_context(ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)LRParser.GetReductionSyntaxNode(1));
					parsertools.create_source_context(ph.proc_attributes,ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
				}
			}	
			parsertools.create_source_context(_procedure_definition,LRParser.GetReductionSyntaxNode(0),rt);
			
			return _procedure_definition;
		}

	case (int)RuleConstants.RULE_ABC_FUNC_DECL :
	//<abc_func_decl> ::= <abc_func_decl_noclass>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_FUNC_DECL_TKCLASS :
	//<abc_func_decl> ::= tkClass <abc_func_decl_noclass>
 ((LRParser.GetReductionSyntaxNode(1) as procedure_definition).proc_header as procedure_header).class_keyword=true;return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_ABC_FUNC_DECL_NOCLASS :
	//<abc_func_decl_noclass> ::= <func_heading> <abc_proc_block>

                //procedure_definition create
		{
			procedure_definition _procedure_definition=new procedure_definition((function_header)LRParser.GetReductionSyntaxNode(0),null);
			object rt=LRParser.GetReductionSyntaxNode(0);
			if(LRParser.GetReductionSyntaxNode(1)!=null) {
				rt=LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is proc_block) _procedure_definition.proc_body=(proc_block)LRParser.GetReductionSyntaxNode(1);
				if(LRParser.GetReductionSyntaxNode(1) is procedure_attribute) {
					procedure_header ph=_procedure_definition.proc_header;
					if(ph.proc_attributes==null) {
						ph.proc_attributes=new procedure_attributes_list();
						parsertools.assign_source_context(ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)LRParser.GetReductionSyntaxNode(1));
					parsertools.create_source_context(ph.proc_attributes,ph.proc_attributes,LRParser.GetReductionSyntaxNode(1));
				}
			}	
			parsertools.create_source_context(_procedure_definition,LRParser.GetReductionSyntaxNode(0),rt);
			
			return _procedure_definition;
		}

	case (int)RuleConstants.RULE_PROC_HEADING_TKPROCEDURE :
	//<proc_heading> ::= tkProcedure <proc_name> <opt_template_arguments> <fp_list> <maybe_error> <opt_meth_modificators> <opt_where_section>
         
		{
			procedure_header _procedure_header=new procedure_header(null,null,(method_name)LRParser.GetReductionSyntaxNode(1),false,false,(ident_list)LRParser.GetReductionSyntaxNode(2),null);
			 
								object rt=LRParser.GetReductionSyntaxNode(1);
								if (LRParser.GetReductionSyntaxNode(3)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(3);
		                                                  _procedure_header.parametres=(formal_parametres)LRParser.GetReductionSyntaxNode(3);
								}
								if(LRParser.GetReductionSyntaxNode(4)!=null)
									(LRParser.GetReductionSyntaxNode(4) as SyntaxError).bad_node=_procedure_header;
								if (LRParser.GetReductionSyntaxNode(5)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(5);
								  if (((procedure_attributes_list)LRParser.GetReductionSyntaxNode(5)).proc_attributes.Count>0) 
									_procedure_header.proc_attributes=(procedure_attributes_list)LRParser.GetReductionSyntaxNode(5);
								}
								if (LRParser.GetReductionSyntaxNode(6)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(6);
								  _procedure_header.where_defs = (where_definition_list)LRParser.GetReductionSyntaxNode(6);
								}
								parsertools.create_source_context(_procedure_header,LRParser.GetReductionSyntaxNode(0),rt);
								
			return _procedure_header;
		}

	case (int)RuleConstants.RULE_PROC_NAME :
	//<proc_name> ::= <func_name>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FUNC_NAME :
	//<func_name> ::= <func_meth_name_ident> <empty>
         
		{
			method_name _method_name=new method_name(null,(ident)LRParser.GetReductionSyntaxNode(0),null);
			parsertools.create_source_context(_method_name,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
			return _method_name;
		}

	case (int)RuleConstants.RULE_FUNC_NAME_TKPOINT :
	//<func_name> ::= <func_class_name_ident> tkPoint <func_meth_name_ident>
         
		{
			method_name _method_name=new method_name((ident)LRParser.GetReductionSyntaxNode(0),(ident)LRParser.GetReductionSyntaxNode(2),null);
			parsertools.create_source_context(_method_name,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _method_name;
		}

	case (int)RuleConstants.RULE_FUNC_METH_NAME_IDENT :
	//<func_meth_name_ident> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FUNC_METH_NAME_IDENT2 :
	//<func_meth_name_ident> ::= <visibility_specifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FUNC_METH_NAME_IDENT3 :
	//<func_meth_name_ident> ::= <operator_name_ident>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FUNC_CLASS_NAME_IDENT :
	//<func_class_name_ident> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FUNC_CLASS_NAME_IDENT2 :
	//<func_class_name_ident> ::= <visibility_specifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FUNC_HEADING_TKFUNCTION_TKCOLON :
	//<func_heading> ::= tkFunction <func_name> <opt_template_arguments> <fp_list> tkColon <fptype> <opt_meth_modificators> <opt_where_section>
         
		{
			function_header _function_header=new function_header();
			 
								object rt=LRParser.GetReductionSyntaxNode(1);
								_function_header.name=(method_name)LRParser.GetReductionSyntaxNode(1);
								_function_header.template_args=(ident_list)LRParser.GetReductionSyntaxNode(2);
								if (LRParser.GetReductionSyntaxNode(3)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(3);
		                                                  _function_header.parametres=(formal_parametres)LRParser.GetReductionSyntaxNode(3);
								}
								if (LRParser.GetReductionSyntaxNode(5)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(5);
								  _function_header.return_type=(type_definition)LRParser.GetReductionSyntaxNode(5);
								}
								if (LRParser.GetReductionSyntaxNode(6)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(6);
								  if (((procedure_attributes_list)LRParser.GetReductionSyntaxNode(6)).proc_attributes.Count>0) 
									_function_header.proc_attributes=(procedure_attributes_list)LRParser.GetReductionSyntaxNode(6);
								}
								if (LRParser.GetReductionSyntaxNode(7)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(7);
								  _function_header.where_defs = (where_definition_list)LRParser.GetReductionSyntaxNode(7);
								}
								_function_header.of_object=false;
 								_function_header.class_keyword=false;
								parsertools.create_source_context(_function_header,LRParser.GetReductionSyntaxNode(0),rt);
								
			return _function_header;
		}

	case (int)RuleConstants.RULE_FUNC_HEADING_TKFUNCTION :
	//<func_heading> ::= tkFunction <func_name> <opt_meth_modificators>
         
		{
			function_header _function_header=new function_header();
			 
								object rt=LRParser.GetReductionSyntaxNode(1);
								_function_header.name=(method_name)LRParser.GetReductionSyntaxNode(1);
								if (LRParser.GetReductionSyntaxNode(2)!=null) {
								  rt=LRParser.GetReductionSyntaxNode(2);
								  if (((procedure_attributes_list)LRParser.GetReductionSyntaxNode(2)).proc_attributes.Count>0) 
									_function_header.proc_attributes=(procedure_attributes_list)LRParser.GetReductionSyntaxNode(2);
								}
								_function_header.of_object=false;
 								_function_header.class_keyword=false;
								parsertools.create_source_context(_function_header,LRParser.GetReductionSyntaxNode(0),rt);
								
			return _function_header;
		}

	case (int)RuleConstants.RULE_PROC_BLOCK :
	//<proc_block> ::= <proc_block_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FUNC_BLOCK :
	//<func_block> ::= <proc_block_decl>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROC_BLOCK_DECL :
	//<proc_block_decl> ::= <block>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROC_BLOCK_DECL2 :
	//<proc_block_decl> ::= <external_directr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROC_BLOCK_DECL3 :
	//<proc_block_decl> ::= <asm_block>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROC_BLOCK_DECL_TKFORWARD_TKSEMICOLON :
	//<proc_block_decl> ::= tkForward tkSemiColon
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_PROC_BLOCK :
	//<abc_proc_block> ::= <abc_block>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_PROC_BLOCK2 :
	//<abc_proc_block> ::= <external_directr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXTERNAL_DIRECTR :
	//<external_directr> ::= <abc_external_directr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXTERNAL_DIRECTR_TKSEMICOLON :
	//<external_directr> ::= <abc_external_directr> tkSemiColon
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXTERNAL_DIRECTR_IDENT :
	//<external_directr_ident> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXTERNAL_DIRECTR_IDENT2 :
	//<external_directr_ident> ::= <literal>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ABC_EXTERNAL_DIRECTR_TKEXTERNAL_TKNAME :
	//<abc_external_directr> ::= tkExternal <external_directr_ident> tkName <external_directr_ident>
         
		{
			external_directive _external_directive=new external_directive((expression)LRParser.GetReductionSyntaxNode(1),(expression)LRParser.GetReductionSyntaxNode(3));
			 
										parsertools.create_source_context(_external_directive,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _external_directive;
		}

	case (int)RuleConstants.RULE_ASM_BLOCK_TKASMBODY_TKSEMICOLON :
	//<asm_block> ::= <impl_decl_sect_list> tkAsmBody tkSemiColon
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<asm_block> ::= <impl_decl_sect_list> tkAsmBody tkSemiColon"));}return null;
	case (int)RuleConstants.RULE_BF_BLOCK_TKBF_TKEND_TKSEMICOLON :
	//<bf_block> ::= tkBF <bf_instructions> tkEnd tkSemiColon
         
		{
			block _block=new block(new declarations(),(statement_list)LRParser.GetReductionSyntaxNode(1));
			
								parsertools.create_source_context(_block,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
								parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
								parsertools.create_source_context(_block.defs,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
								//_block.defs.defs.Add()
								
			return _block;
		}

	case (int)RuleConstants.RULE_BF_EMPTY_INSTRUCTION :
	//<bf_empty_instruction> ::= 
	//NONTERMINAL:<bf_empty_instruction> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_BF_INSTRUCTIONS :
	//<bf_instructions> ::= <bf_instructions_list>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_BF_INSTRUCTIONS2 :
	//<bf_instructions> ::= <bf_empty_instruction> <empty>
         
		{
			statement_list _statement_list=new statement_list();
			 _statement_list.subnodes.Add(new empty_statement());
			return _statement_list;
		}

	case (int)RuleConstants.RULE_BF_INSTRUCTIONS_LIST :
	//<bf_instructions_list> ::= <bf_instruction> <empty>
         
		{
			statement_list _statement_list=new statement_list();
			
								_statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(0));
								
			return _statement_list;
		}

	case (int)RuleConstants.RULE_BF_INSTRUCTIONS_LIST2 :
	//<bf_instructions_list> ::= <bf_instructions_list> <bf_instruction>
         
		{
			statement_list _statement_list=(statement_list)LRParser.GetReductionSyntaxNode(0);
								_statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(1));
								
			return _statement_list;
		}

	case (int)RuleConstants.RULE_BF_INSTRUCTION_TKGREATER :
	//<bf_instruction> ::= tkGreater <empty>
         
		{
			ident _ident=new ident("IncCaret");
			procedure_call _procedure_call=new procedure_call(_ident);
			parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(_procedure_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
                        return _procedure_call;
		}

	case (int)RuleConstants.RULE_BF_INSTRUCTION_TKLOWER :
	//<bf_instruction> ::= tkLower <empty>
         
		{
			ident _ident=new ident("DecCaret");
			procedure_call _procedure_call=new procedure_call(_ident);
			parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(_procedure_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
                        return _procedure_call;
		}

	case (int)RuleConstants.RULE_BF_INSTRUCTION_TKPLUS :
	//<bf_instruction> ::= tkPlus <empty>
         
		{
			ident _ident=new ident("IncCaretValue");
			procedure_call _procedure_call=new procedure_call(_ident);
			parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(_procedure_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
                        return _procedure_call;
		}

	case (int)RuleConstants.RULE_BF_INSTRUCTION_TKMINUS :
	//<bf_instruction> ::= tkMinus <empty>
         
		{
			ident _ident=new ident("DecCaretValue");
			procedure_call _procedure_call=new procedure_call(_ident);
			parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(_procedure_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
                        return _procedure_call;
		}

	case (int)RuleConstants.RULE_BF_INSTRUCTION_TKPOINT :
	//<bf_instruction> ::= tkPoint <empty>
         
		{
			ident _ident=new ident("WriteCaretValue");
			procedure_call _procedure_call=new procedure_call(_ident);
			parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(_procedure_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
                        return _procedure_call;
		}

	case (int)RuleConstants.RULE_BF_INSTRUCTION_TKCOMMA :
	//<bf_instruction> ::= tkComma <empty>
         
		{
			ident _ident=new ident("ReadCaretValue");
			procedure_call _procedure_call=new procedure_call(_ident);
			parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(_procedure_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
                        return _procedure_call;
		}

	case (int)RuleConstants.RULE_BF_INSTRUCTION_TKSQUAREOPEN_TKSQUARECLOSE :
	//<bf_instruction> ::= tkSquareOpen <bf_instructions> tkSquareClose
         
		{
			method_call m=new method_call();
			ident id=new ident("CaretValueNotNull");
			m.dereferencing_value=id;
			parsertools.create_source_context(id,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(m,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			while_node wn=new while_node();
                        wn.expr=m;
			wn.statements=(statement_list)LRParser.GetReductionSyntaxNode(1);
                        return wn;
		}

	case (int)RuleConstants.RULE_BF_INSTRUCTION_TKDOTDOT :
	//<bf_instruction> ::= tkDotDot <empty>
         
		{
			ident _ident=new ident("WriteCaretValue2");
			procedure_call _procedure_call=new procedure_call(_ident);
			parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(_procedure_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
                        return _procedure_call;
		}

	case (int)RuleConstants.RULE_BLOCK_TKSEMICOLON :
	//<block> ::= <impl_decl_sect_list> <compound_stmt> tkSemiColon
         
		{
			block _block=new block((declarations)LRParser.GetReductionSyntaxNode(0),(statement_list)LRParser.GetReductionSyntaxNode(1));
			
								parsertools.create_source_context(_block,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)),LRParser.GetReductionSyntaxNode(2));
			return _block;
		}

	case (int)RuleConstants.RULE_ABC_BLOCK_TKSEMICOLON :
	//<abc_block> ::= <abc_decl_sect_list> <compound_stmt> tkSemiColon
         
		{
			block _block=new block((declarations)LRParser.GetReductionSyntaxNode(0),(statement_list)LRParser.GetReductionSyntaxNode(1));
			
								parsertools.create_source_context(_block,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)),LRParser.GetReductionSyntaxNode(2));
			return _block;
		}

	case (int)RuleConstants.RULE_FP_LIST :
	//<fp_list> ::= 
	//NONTERMINAL:<fp_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_FP_LIST_TKROUNDOPEN_TKROUNDCLOSE :
	//<fp_list> ::= tkRoundOpen <fp_sect_list> tkRoundClose
 if(LRParser.GetReductionSyntaxNode(1)!=null) parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
								return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_FP_SECT_LIST :
	//<fp_sect_list> ::= 
	//NONTERMINAL:<fp_sect_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_FP_SECT_LIST2 :
	//<fp_sect_list> ::= <fp_sect> <empty>
         
		{
			formal_parametres _formal_parametres=new formal_parametres();
			
								_formal_parametres.params_list.Add((typed_parametres)LRParser.GetReductionSyntaxNode(0));
								
			return _formal_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_LIST_TKSEMICOLON :
	//<fp_sect_list> ::= <fp_sect_list> tkSemiColon <fp_sect>
         
		{
			formal_parametres _formal_parametres=(formal_parametres)LRParser.GetReductionSyntaxNode(0);
								_formal_parametres.params_list.Add((typed_parametres)LRParser.GetReductionSyntaxNode(2));	
								
			return _formal_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKCOLON :
	//<fp_sect> ::= <param_name_list> tkColon <fptype_new>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(0),(type_definition)LRParser.GetReductionSyntaxNode(2),parametr_kind.none,null);
			parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT :
	//<fp_sect> ::= <param_name_list> <empty>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(0),null,parametr_kind.none,null);
			parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKVAR_TKCOLON :
	//<fp_sect> ::= tkVar <param_name_list> tkColon <fptype_new>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(1),(type_definition)LRParser.GetReductionSyntaxNode(3),parametr_kind.var_parametr,null);
			 parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKVAR :
	//<fp_sect> ::= tkVar <param_name_list> <empty>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(1),null,parametr_kind.var_parametr,null);
			 parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKOUT_TKCOLON :
	//<fp_sect> ::= tkOut <param_name_list> tkColon <fptype_new>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(1),(type_definition)LRParser.GetReductionSyntaxNode(3),parametr_kind.out_parametr,null);
			 parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKOUT :
	//<fp_sect> ::= tkOut <param_name_list> <empty>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(1),null,parametr_kind.out_parametr,null);
			 parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKCONST_TKCOLON :
	//<fp_sect> ::= tkConst <param_name_list> tkColon <fptype_new>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(1),(type_definition)LRParser.GetReductionSyntaxNode(3),parametr_kind.const_parametr,null);
			 parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKCONST :
	//<fp_sect> ::= tkConst <param_name_list> <empty>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(1),null,parametr_kind.const_parametr,null);
			 parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKPARAMS_TKCOLON :
	//<fp_sect> ::= tkParams <param_name_list> tkColon <fptype_new>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(1),(type_definition)LRParser.GetReductionSyntaxNode(3),parametr_kind.params_parametr,null);
			 parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKPARAMS :
	//<fp_sect> ::= tkParams <param_name_list> <empty>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(1),null,parametr_kind.params_parametr,null);
			 parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKCOLON_TKASSIGN :
	//<fp_sect> ::= <param_name_list> tkColon <fptype> tkAssign <const_expr>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(0),(type_definition)LRParser.GetReductionSyntaxNode(2),parametr_kind.none,(expression)LRParser.GetReductionSyntaxNode(4));
			parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
			
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKVAR_TKCOLON_TKASSIGN :
	//<fp_sect> ::= tkVar <param_name_list> tkColon <fptype> tkAssign <const_expr>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(1),(type_definition)LRParser.GetReductionSyntaxNode(3),parametr_kind.var_parametr,(expression)LRParser.GetReductionSyntaxNode(5));
			 parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKOUT_TKCOLON_TKASSIGN :
	//<fp_sect> ::= tkOut <param_name_list> tkColon <fptype> tkAssign <const_expr>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(1),(type_definition)LRParser.GetReductionSyntaxNode(3),parametr_kind.out_parametr,(expression)LRParser.GetReductionSyntaxNode(5));
			 parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_FP_SECT_TKCONST_TKCOLON_TKASSIGN :
	//<fp_sect> ::= tkConst <param_name_list> tkColon <fptype> tkAssign <const_expr>
         
		{
			typed_parametres _typed_parametres=new typed_parametres((ident_list)LRParser.GetReductionSyntaxNode(1),(type_definition)LRParser.GetReductionSyntaxNode(3),parametr_kind.const_parametr,(expression)LRParser.GetReductionSyntaxNode(5));
			 parsertools.create_source_context(_typed_parametres,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _typed_parametres;
		}

	case (int)RuleConstants.RULE_PARAM_NAME_LIST :
	//<param_name_list> ::= <param_name> <empty>
         
		//TemplateList for ident_list (create)
		{
			ident_list _ident_list=new ident_list();
			_ident_list.source_context=((ident)LRParser.GetReductionSyntaxNode(0)).source_context;
			_ident_list.idents.Add((ident)LRParser.GetReductionSyntaxNode(0));
			return _ident_list;
		}

	case (int)RuleConstants.RULE_PARAM_NAME_LIST_TKCOMMA :
	//<param_name_list> ::= <param_name_list> tkComma <param_name>

		//TemplateList for ident_list (add)         
		{
			ident_list _ident_list=(ident_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_ident_list,_ident_list,LRParser.GetReductionSyntaxNode(2));
			_ident_list.idents.Add(LRParser.GetReductionSyntaxNode(2) as ident);
			return _ident_list;
		}

	case (int)RuleConstants.RULE_PARAM_NAME :
	//<param_name> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FPTYPE :
	//<fptype> ::= <type_ref>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FPTYPE_NEW :
	//<fptype_new> ::= <type_ref>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FPTYPE_NEW_TKARRAY_TKOF_TKCONST :
	//<fptype_new> ::= tkArray tkOf tkConst
         
		{
			array_of_const_type_definition _array_of_const_type_definition=new array_of_const_type_definition();
			 parsertools.create_source_context(_array_of_const_type_definition,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _array_of_const_type_definition;
		}

	case (int)RuleConstants.RULE_STMT :
	//<stmt> ::= <unlabelled_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_STMT_TKCOLON :
	//<stmt> ::= <label_name> tkColon <unlabelled_stmt>
         
		{
			labeled_statement _labeled_statement=new labeled_statement((ident)LRParser.GetReductionSyntaxNode(0),(statement)LRParser.GetReductionSyntaxNode(2));
			 parsertools.create_source_context(_labeled_statement,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1)));
			return _labeled_statement;
		}

	case (int)RuleConstants.RULE_UNLABELLED_STMT :
	//<unlabelled_stmt> ::= <empty> <empty>
         
		{
			empty_statement _empty_statement=new empty_statement();
			
			return _empty_statement;
		}

	case (int)RuleConstants.RULE_UNLABELLED_STMT2 :
	//<unlabelled_stmt> ::= <assignment>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT3 :
	//<unlabelled_stmt> ::= <proc_call>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT4 :
	//<unlabelled_stmt> ::= <goto_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT5 :
	//<unlabelled_stmt> ::= <compound_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT6 :
	//<unlabelled_stmt> ::= <if_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT7 :
	//<unlabelled_stmt> ::= <case_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT8 :
	//<unlabelled_stmt> ::= <repeat_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT9 :
	//<unlabelled_stmt> ::= <while_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT10 :
	//<unlabelled_stmt> ::= <for_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT11 :
	//<unlabelled_stmt> ::= <with_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT12 :
	//<unlabelled_stmt> ::= <asm_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT13 :
	//<unlabelled_stmt> ::= <inherited_message>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT14 :
	//<unlabelled_stmt> ::= <try_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT15 :
	//<unlabelled_stmt> ::= <raise_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT16 :
	//<unlabelled_stmt> ::= <foreach_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT17 :
	//<unlabelled_stmt> ::= <var_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT18 :
	//<unlabelled_stmt> ::= <expr_as_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_UNLABELLED_STMT19 :
	//<unlabelled_stmt> ::= <lock_stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VAR_STMT_TKVAR :
	//<var_stmt> ::= tkVar <var_decl_part_in_stmt>
         
		{
			var_statement _var_statement=new var_statement(LRParser.GetReductionSyntaxNode(1) as var_def_statement);
			 parsertools.create_source_context(_var_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _var_statement;
		}

	case (int)RuleConstants.RULE_ASSIGNMENT :
	//<assignment> ::= <var_reference> <assign_operator> <expr>
         
		{
			assign _assign=new assign(LRParser.GetReductionSyntaxNode(0) as addressed_value,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_assign,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _assign;
		}

	case (int)RuleConstants.RULE_PROC_CALL :
	//<proc_call> ::= <var_reference> <empty>
         
		{
			procedure_call _procedure_call=new procedure_call(LRParser.GetReductionSyntaxNode(0) as addressed_value);
			parsertools.create_source_context(_procedure_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
			return _procedure_call;
		}

	case (int)RuleConstants.RULE_GOTO_STMT_TKGOTO :
	//<goto_stmt> ::= tkGoto <label_name>
         
		{
			goto_statement _goto_statement=new goto_statement((ident)LRParser.GetReductionSyntaxNode(1));
			parsertools.create_source_context(_goto_statement,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1));
			
			return _goto_statement;
		}

	case (int)RuleConstants.RULE_COMPOUND_STMT_TKBEGIN_TKEND :
	//<compound_stmt> ::= tkBegin <stmt_list> tkEnd

	 							parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
								((statement_list)LRParser.GetReductionSyntaxNode(1)).left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(0);
								((statement_list)LRParser.GetReductionSyntaxNode(1)).right_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(2);
								return LRParser.GetReductionSyntaxNode(1);
								
	case (int)RuleConstants.RULE_COMPOUND_STMT_TKILCODE :
	//<compound_stmt> ::= tkILCode
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_STMT_LIST :
	//<stmt_list> ::= <stmt> <empty>
         
		{
			statement_list _statement_list=new statement_list();
			
								_statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(0));
								parsertools.assign_source_context(_statement_list,LRParser.GetReductionSyntaxNode(0)); 
								
			return _statement_list;
		}

	case (int)RuleConstants.RULE_STMT_LIST_TKSEMICOLON :
	//<stmt_list> ::= <stmt_list> tkSemiColon <stmt>
         
		{
			statement_list _statement_list;
			 _statement_list=(statement_list)LRParser.GetReductionSyntaxNode(0);
								_statement_list.subnodes.Add((statement)LRParser.GetReductionSyntaxNode(2));
								parsertools.create_source_context(_statement_list,_statement_list,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1))); 
								
			return _statement_list;
		}

	case (int)RuleConstants.RULE_IF_STMT_TKIF :
	//<if_stmt> ::= tkIf <expr> <if_then_else_branch>
((if_node)LRParser.GetReductionSyntaxNode(2)).condition=(expression)LRParser.GetReductionSyntaxNode(1);
								parsertools.create_source_context(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
								return LRParser.GetReductionSyntaxNode(2);
								
	case (int)RuleConstants.RULE_IF_THEN_ELSE_BRANCH_TKTHEN :
	//<if_then_else_branch> ::= tkThen <then_branch>
         
		{
			if_node _if_node=new if_node(null,(statement)LRParser.GetReductionSyntaxNode(1),null);
			
								parsertools.create_source_context(_if_node,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));	
								
			return _if_node;
		}

	case (int)RuleConstants.RULE_IF_THEN_ELSE_BRANCH_TKTHEN_TKELSE :
	//<if_then_else_branch> ::= tkThen <then_branch> tkElse <else_branch>
         
		{
			if_node _if_node=new if_node(null,(statement)LRParser.GetReductionSyntaxNode(1),(statement)LRParser.GetReductionSyntaxNode(3));
			
								parsertools.create_source_context(_if_node,LRParser.GetReductionSyntaxNode(2),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2)));	
								
			return _if_node;
		}

	case (int)RuleConstants.RULE_THEN_BRANCH :
	//<then_branch> ::= <stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ELSE_BRANCH :
	//<else_branch> ::= <stmt>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_CASE_STMT_TKCASE_TKOF_TKEND :
	//<case_stmt> ::= tkCase <expr> tkOf <case_list> <else_case> tkEnd
         
		{
			case_node _case_node=new case_node((expression)LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(3) as case_variants,LRParser.GetReductionSyntaxNode(4) as statement);
			
								parsertools.create_source_context(_case_node,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(5));
			return _case_node;
		}

	case (int)RuleConstants.RULE_CASE_LIST :
	//<case_list> ::= <case_item> <empty>
         
		{
			case_variants _case_variants=new case_variants();
			 
								if (LRParser.GetReductionSyntaxNode(0) is case_variant)
								{
									_case_variants.variants.Add((case_variant)LRParser.GetReductionSyntaxNode(0));
									parsertools.create_source_context(_case_variants,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
								}
			return _case_variants;
		}

	case (int)RuleConstants.RULE_CASE_LIST_TKSEMICOLON :
	//<case_list> ::= <case_list> tkSemiColon <case_item>
         
		{
			case_variants _case_variants=(case_variants)LRParser.GetReductionSyntaxNode(0);
								parsertools.create_source_context(_case_variants,parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1)),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1))); 							
								if (LRParser.GetReductionSyntaxNode(2) is case_variant) _case_variants.variants.Add((case_variant)LRParser.GetReductionSyntaxNode(2));
			return _case_variants;
		}

	case (int)RuleConstants.RULE_CASE_ITEM :
	//<case_item> ::= <empty> <empty>
         
		{
			empty_statement _empty_statement=new empty_statement();
			
			return _empty_statement;
		}

	case (int)RuleConstants.RULE_CASE_ITEM_TKCOLON :
	//<case_item> ::= <case_label_list> tkColon <stmt>
         
		{
			case_variant _case_variant=new case_variant((expression_list)LRParser.GetReductionSyntaxNode(0),(statement)LRParser.GetReductionSyntaxNode(2));
			 
								parsertools.create_source_context(_case_variant,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1)));
			return _case_variant;
		}

	case (int)RuleConstants.RULE_CASE_LABEL_LIST :
	//<case_label_list> ::= <case_label> <empty>
         
		//TemplateList for expression_list (create)
		{
			expression_list _expression_list=new expression_list();
			_expression_list.source_context=((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
			_expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
			return _expression_list;
		}

	case (int)RuleConstants.RULE_CASE_LABEL_LIST_TKCOMMA :
	//<case_label_list> ::= <case_label_list> tkComma <case_label>

		//TemplateList for expression_list (add)         
		{
			expression_list _expression_list=(expression_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_expression_list,_expression_list,LRParser.GetReductionSyntaxNode(2));
			_expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);
			return _expression_list;
		}

	case (int)RuleConstants.RULE_CASE_LABEL :
	//<case_label> ::= <const_elem>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ELSE_CASE :
	//<else_case> ::= 
	//NONTERMINAL:<else_case> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_ELSE_CASE_TKELSE :
	//<else_case> ::= tkElse <stmt_list>
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_REPEAT_STMT_TKREPEAT_TKUNTIL :
	//<repeat_stmt> ::= tkRepeat <stmt_list> tkUntil <expr>
         
		{
			repeat_node _repeat_node=new repeat_node((statement)LRParser.GetReductionSyntaxNode(1),(expression)LRParser.GetReductionSyntaxNode(3));
			
								((statement_list)LRParser.GetReductionSyntaxNode(1)).left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(0);
								((statement_list)LRParser.GetReductionSyntaxNode(1)).right_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(2);
                                                                parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
								parsertools.create_source_context(_repeat_node,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));	
								
			return _repeat_node;
		}

	case (int)RuleConstants.RULE_WHILE_STMT_TKWHILE_TKDO :
	//<while_stmt> ::= tkWhile <expr> tkDo <stmt>
         
		{
			while_node _while_node=new while_node((expression)LRParser.GetReductionSyntaxNode(1),(statement)LRParser.GetReductionSyntaxNode(3),WhileCycleType.While);
			
								parsertools.create_source_context(_while_node,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2)));	
								
			return _while_node;
		}

	case (int)RuleConstants.RULE_LOCK_STMT_TKLOCK_TKDO :
	//<lock_stmt> ::= tkLock <expr> tkDo <stmt>
         
		{
			lock_stmt _lock_stmt=new lock_stmt((expression)LRParser.GetReductionSyntaxNode(1),(statement)LRParser.GetReductionSyntaxNode(3));
			
								parsertools.create_source_context(_lock_stmt,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2)));	
								
			return _lock_stmt;
		}

	case (int)RuleConstants.RULE_FOREACH_STMT_TKFOREACH_TKIN_TKDO :
	//<foreach_stmt> ::= tkForeach <identifier> <foreach_stmt_ident_dype_opt> tkIn <expr> tkDo <stmt>
         
		{
			foreach_stmt _foreach_stmt=new foreach_stmt((ident)LRParser.GetReductionSyntaxNode(1),(type_definition)LRParser.GetReductionSyntaxNode(2),(expression)LRParser.GetReductionSyntaxNode(4),(statement)LRParser.GetReductionSyntaxNode(6));
			
								parsertools.create_source_context(_foreach_stmt,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(6),LRParser.GetReductionSyntaxNode(5)));	
								
			return _foreach_stmt;
		}

	case (int)RuleConstants.RULE_FOREACH_STMT_IDENT_DYPE_OPT_TKCOLON :
	//<foreach_stmt_ident_dype_opt> ::= tkColon <type_ref>
 return LRParser.GetReductionSyntaxNode(1); 
	case (int)RuleConstants.RULE_FOREACH_STMT_IDENT_DYPE_OPT :
	//<foreach_stmt_ident_dype_opt> ::= 
	//NONTERMINAL:<foreach_stmt_ident_dype_opt> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_FOR_STMT_TKFOR_TKDO :
	//<for_stmt> ::= tkFor <opt_var> <identifier> <for_stmt_decl_or_assign> <expr> <for_cycle_type> <expr> tkDo <stmt>
         
		{
			for_node _for_node=new for_node((ident)LRParser.GetReductionSyntaxNode(2),(expression)LRParser.GetReductionSyntaxNode(4),(expression)LRParser.GetReductionSyntaxNode(6),(statement)LRParser.GetReductionSyntaxNode(8),(for_cycle_type)LRParser.GetReductionSyntaxNode(5),null,LRParser.GetReductionSyntaxNode(3) as type_definition, LRParser.GetReductionSyntaxNode(1)!=null);
			
								parsertools.create_source_context(_for_node,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(8),LRParser.GetReductionSyntaxNode(7)));	
								
			return _for_node;
		}

	case (int)RuleConstants.RULE_OPT_VAR_TKVAR :
	//<opt_var> ::= tkVar
return true;
	case (int)RuleConstants.RULE_OPT_VAR :
	//<opt_var> ::= 
	//NONTERMINAL:<opt_var> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_FOR_STMT_DECL_OR_ASSIGN_TKASSIGN :
	//<for_stmt_decl_or_assign> ::= tkAssign
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FOR_STMT_DECL_OR_ASSIGN_TKCOLON_TKASSIGN :
	//<for_stmt_decl_or_assign> ::= tkColon <simple_type_identifier> tkAssign
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_FOR_CYCLE_TYPE_TKTO :
	//<for_cycle_type> ::= tkTo
 return for_cycle_type.to; 
	case (int)RuleConstants.RULE_FOR_CYCLE_TYPE_TKDOWNTO :
	//<for_cycle_type> ::= tkDownto
 return for_cycle_type.downto; 
	case (int)RuleConstants.RULE_WITH_STMT_TKWITH_TKDO :
	//<with_stmt> ::= tkWith <expr_list> tkDo <stmt>
         
		{
			with_statement _with_statement=new with_statement((statement)LRParser.GetReductionSyntaxNode(3),(expression_list)LRParser.GetReductionSyntaxNode(1));
			
								parsertools.create_source_context(_with_statement,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2)));
			return _with_statement;
		}

	case (int)RuleConstants.RULE_INHERITED_MESSAGE_TKINHERITED :
	//<inherited_message> ::= tkInherited <empty>
         
		{
			inherited_message _inherited_message=new inherited_message();
			 parsertools.assign_source_context(_inherited_message,LRParser.GetReductionSyntaxNode(0));
			return _inherited_message;
		}

	case (int)RuleConstants.RULE_TRY_STMT_TKTRY :
	//<try_stmt> ::= tkTry <stmt_list> <try_handler>
         
		{
			try_stmt _try_stmt=new try_stmt(((statement_list)LRParser.GetReductionSyntaxNode(1)),(try_handler)LRParser.GetReductionSyntaxNode(2));
			 
							((statement_list)LRParser.GetReductionSyntaxNode(1)).left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(0);
							parsertools.create_source_context(_try_stmt,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
							
			return _try_stmt;
		}

	case (int)RuleConstants.RULE_TRY_HANDLER_TKFINALLY_TKEND :
	//<try_handler> ::= tkFinally <stmt_list> tkEnd
         
		{
			try_handler_finally _try_handler_finally=new try_handler_finally((statement_list)LRParser.GetReductionSyntaxNode(1));
			 
							((statement_list)LRParser.GetReductionSyntaxNode(1)).left_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(0);
							((statement_list)LRParser.GetReductionSyntaxNode(1)).right_logical_bracket=(syntax_tree_node)LRParser.GetReductionSyntaxNode(2);
							parsertools.create_source_context(_try_handler_finally,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _try_handler_finally;
		}

	case (int)RuleConstants.RULE_TRY_HANDLER_TKEXCEPT_TKEND :
	//<try_handler> ::= tkExcept <exception_block> tkEnd
         
		{
			try_handler_except _try_handler_except=new try_handler_except((exception_block)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_try_handler_except,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _try_handler_except;
		}

	case (int)RuleConstants.RULE_EXCEPTION_BLOCK :
	//<exception_block> ::= <exception_handler_list> <exception_block_else_branch>
         
		{
			exception_block _exception_block=new exception_block(null,(exception_handler_list)LRParser.GetReductionSyntaxNode(0),(statement_list)LRParser.GetReductionSyntaxNode(1));
			
										parsertools.create_source_context(_exception_block,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0)));
			return _exception_block;
		}

	case (int)RuleConstants.RULE_EXCEPTION_BLOCK_TKSEMICOLON :
	//<exception_block> ::= <exception_handler_list> tkSemiColon <exception_block_else_branch>
         
		{
			exception_block _exception_block=new exception_block(null,(exception_handler_list)LRParser.GetReductionSyntaxNode(0),(statement_list)LRParser.GetReductionSyntaxNode(2));
			
										parsertools.create_source_context(_exception_block,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1)));
			return _exception_block;
		}

	case (int)RuleConstants.RULE_EXCEPTION_BLOCK2 :
	//<exception_block> ::= <stmt_list> <empty>
         
		{
			exception_block _exception_block=new exception_block((statement_list)LRParser.GetReductionSyntaxNode(0),null,null);
			
										if (((syntax_tree_node)LRParser.GetReductionSyntaxNode(0)).source_context!=null) parsertools.assign_source_context(_exception_block,LRParser.GetReductionSyntaxNode(0));
			return _exception_block;
		}

	case (int)RuleConstants.RULE_EXCEPTION_HANDLER_LIST :
	//<exception_handler_list> ::= <exception_handler> <empty>
         
		//TemplateList for exception_handler_list (create)
		{
			exception_handler_list _exception_handler_list=new exception_handler_list();
			_exception_handler_list.source_context=((exception_handler)LRParser.GetReductionSyntaxNode(0)).source_context;
			_exception_handler_list.handlers.Add((exception_handler)LRParser.GetReductionSyntaxNode(0));
			return _exception_handler_list;
		}

	case (int)RuleConstants.RULE_EXCEPTION_HANDLER_LIST_TKSEMICOLON :
	//<exception_handler_list> ::= <exception_handler_list> tkSemiColon <exception_handler>

		//TemplateList for exception_handler_list (add)         
		{
			exception_handler_list _exception_handler_list=(exception_handler_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_exception_handler_list,_exception_handler_list,LRParser.GetReductionSyntaxNode(2));
			_exception_handler_list.handlers.Add(LRParser.GetReductionSyntaxNode(2) as exception_handler);
			return _exception_handler_list;
		}

	case (int)RuleConstants.RULE_EXCEPTION_BLOCK_ELSE_BRANCH :
	//<exception_block_else_branch> ::= 
	//NONTERMINAL:<exception_block_else_branch> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_EXCEPTION_BLOCK_ELSE_BRANCH_TKELSE :
	//<exception_block_else_branch> ::= tkElse <stmt_list>
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_EXCEPTION_HANDLER_TKON_TKDO :
	//<exception_handler> ::= tkOn <exception_identifier> tkDo <stmt>
         
		{
			exception_handler _exception_handler=new exception_handler(((exception_ident)LRParser.GetReductionSyntaxNode(1)).variable,((exception_ident)LRParser.GetReductionSyntaxNode(1)).type_name,(statement)LRParser.GetReductionSyntaxNode(3));
			
								parsertools.create_source_context(_exception_handler,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(3)));
			return _exception_handler;
		}

	case (int)RuleConstants.RULE_EXCEPTION_IDENTIFIER :
	//<exception_identifier> ::= <exception_class_type_identifier> <empty>
         
		{
			exception_ident _exception_ident=new exception_ident(null,(named_type_reference)LRParser.GetReductionSyntaxNode(0));
			parsertools.create_source_context(_exception_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
			return _exception_ident;
		}

	case (int)RuleConstants.RULE_EXCEPTION_IDENTIFIER_TKCOLON :
	//<exception_identifier> ::= <exception_variable> tkColon <exception_class_type_identifier>
         
		{
			exception_ident _exception_ident=new exception_ident((ident)LRParser.GetReductionSyntaxNode(0),(named_type_reference)LRParser.GetReductionSyntaxNode(2));
			parsertools.create_source_context(_exception_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _exception_ident;
		}

	case (int)RuleConstants.RULE_EXCEPTION_CLASS_TYPE_IDENTIFIER :
	//<exception_class_type_identifier> ::= <simple_type_identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXCEPTION_VARIABLE :
	//<exception_variable> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RAISE_STMT_TKRAISE :
	//<raise_stmt> ::= tkRaise <empty>
         
		{
			raise_stmt _raise_stmt=new raise_stmt();
			 parsertools.assign_source_context(_raise_stmt,LRParser.GetReductionSyntaxNode(0));
			return _raise_stmt;
		}

	case (int)RuleConstants.RULE_RAISE_STMT_TKRAISE2 :
	//<raise_stmt> ::= tkRaise <expr>
         
		{
			raise_stmt _raise_stmt=new raise_stmt((expression)LRParser.GetReductionSyntaxNode(1),null);
			 parsertools.create_source_context(_raise_stmt,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _raise_stmt;
		}

	case (int)RuleConstants.RULE_RAISE_STMT_TKRAISE_TKAT :
	//<raise_stmt> ::= tkRaise <expr> tkAt <expr>
         
		{
			raise_stmt _raise_stmt=new raise_stmt((expression)LRParser.GetReductionSyntaxNode(1),(expression)LRParser.GetReductionSyntaxNode(3));
			 parsertools.create_source_context(_raise_stmt,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _raise_stmt;
		}

	case (int)RuleConstants.RULE_ASM_STMT_TKASMBODY :
	//<asm_stmt> ::= tkAsmBody
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXPR_LIST :
	//<expr_list> ::= <expr> <empty>
         
		//TemplateList for expression_list (create)
		{
			expression_list _expression_list=new expression_list();
			_expression_list.source_context=((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
			_expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
			return _expression_list;
		}

	case (int)RuleConstants.RULE_EXPR_LIST_TKCOMMA :
	//<expr_list> ::= <expr_list> tkComma <expr>

		//TemplateList for expression_list (add)         
		{
			expression_list _expression_list=(expression_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_expression_list,_expression_list,LRParser.GetReductionSyntaxNode(2));
			_expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);
			return _expression_list;
		}

	case (int)RuleConstants.RULE_EXPR_AS_STMT :
	//<expr_as_stmt> ::= <allowable_expr_as_stmt> <empty>
         
		{
			expression_as_statement _expression_as_statement=new expression_as_statement((expression)LRParser.GetReductionSyntaxNode(0));
			 parsertools.create_source_context(_expression_as_statement,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _expression_as_statement;
		}

	case (int)RuleConstants.RULE_ALLOWABLE_EXPR_AS_STMT :
	//<allowable_expr_as_stmt> ::= <new_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXPR :
	//<expr> ::= <expr_l1>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXPR2 :
	//<expr> ::= <format_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXPR_L1 :
	//<expr_l1> ::= <relop_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXPR_L12 :
	//<expr_l1> ::= <question_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EXPR_L13 :
	//<expr_l1> ::= <new_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIZEOF_EXPR_TKSIZEOF_TKROUNDOPEN_TKROUNDCLOSE :
	//<sizeof_expr> ::= tkSizeOf tkRoundOpen <simple_type_identifier> tkRoundClose
         
		{
			sizeof_operator _sizeof_operator=new sizeof_operator((named_type_reference)LRParser.GetReductionSyntaxNode(2),null);
			 parsertools.create_source_context(_sizeof_operator,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _sizeof_operator;
		}

	case (int)RuleConstants.RULE_TYPEOF_EXPR_TKTYPEOF_TKROUNDOPEN_TKROUNDCLOSE :
	//<typeof_expr> ::= tkTypeOf tkRoundOpen <simple_type_identifier> tkRoundClose
         
		{
			typeof_operator _typeof_operator=new typeof_operator((named_type_reference)LRParser.GetReductionSyntaxNode(2));
			 parsertools.create_source_context(_typeof_operator,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(3));
			return _typeof_operator;
		}

	case (int)RuleConstants.RULE_QUESTION_EXPR_TKQUESTION_TKCOLON :
	//<question_expr> ::= <expr_l1> tkQuestion <expr_l1> tkColon <expr_l1>
         
		{
			question_colon_expression _question_colon_expression=new question_colon_expression((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),(expression)LRParser.GetReductionSyntaxNode(4));
			 parsertools.create_source_context(_question_colon_expression,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
			return _question_colon_expression;
		}

	case (int)RuleConstants.RULE_NEW_EXPR :
	//<new_expr> ::= <identifier> <simple_type_identifier> <opt_template_type_params> <opt_expr_list_with_bracket>

							{
							named_type_reference ntr;
							if(LRParser.GetReductionSyntaxNode(2)!=null)
							{
							  ntr=new template_type_reference((named_type_reference)LRParser.GetReductionSyntaxNode(1),(template_param_list)LRParser.GetReductionSyntaxNode(2));
							  parsertools.create_source_context(ntr,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(2));
							}
							else
							  ntr=(named_type_reference)LRParser.GetReductionSyntaxNode(1);
							new_expr newexpr=new new_expr(ntr,LRParser.GetReductionSyntaxNode(3) as expression_list,false,null);
							parsertools.create_source_context(newexpr,LRParser.GetReductionSyntaxNode(0),parsertools.sc_not_null(LRParser.GetReductionSyntaxNode(3),LRParser.GetReductionSyntaxNode(2),LRParser.GetReductionSyntaxNode(1)));
							if ((LRParser.GetReductionSyntaxNode(0) as ident).name.ToLower()!="new")
								errors.Add(new Errors.PABCNETUnexpectedToken(current_file_name,";",((syntax_tree_node)LRParser.GetReductionSyntaxNode(0)).source_context,newexpr));
							return newexpr;
							}
							
	case (int)RuleConstants.RULE_NEW_EXPR_TKSQUAREOPEN_TKSQUARECLOSE :
	//<new_expr> ::= <identifier> <array_name_for_new_expr> tkSquareOpen <expr_list> tkSquareClose

							{
							new_expr newexpr=new new_expr((type_definition)LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(3) as expression_list,true,null);
							parsertools.create_source_context(newexpr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
							if ((LRParser.GetReductionSyntaxNode(0) as ident).name.ToLower()!="new")
								errors.Add(new Errors.PABCNETUnexpectedToken(current_file_name,";",((syntax_tree_node)LRParser.GetReductionSyntaxNode(0)).source_context,newexpr));
							return newexpr;
							}
							
	case (int)RuleConstants.RULE_ARRAY_NAME_FOR_NEW_EXPR :
	//<array_name_for_new_expr> ::= <simple_type_identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ARRAY_NAME_FOR_NEW_EXPR2 :
	//<array_name_for_new_expr> ::= <unsized_array_type>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OPT_TEMPLATE_TYPE_PARAMS :
	//<opt_template_type_params> ::= 
	//NONTERMINAL:<opt_template_type_params> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_OPT_TEMPLATE_TYPE_PARAMS2 :
	//<opt_template_type_params> ::= <template_type_params>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OPT_EXPR_LIST_WITH_BRACKET :
	//<opt_expr_list_with_bracket> ::= 
	//NONTERMINAL:<opt_expr_list_with_bracket> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_OPT_EXPR_LIST_WITH_BRACKET_TKROUNDOPEN_TKROUNDCLOSE :
	//<opt_expr_list_with_bracket> ::= tkRoundOpen <opt_expr_list> tkRoundClose
return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_RELOP_EXPR :
	//<relop_expr> ::= <simple_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_EXPR2 :
	//<relop_expr> ::= <simple_expr> <relop> <relop_expr>
         
		{
			bin_expr _bin_expr=new bin_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_FORMAT_EXPR_TKCOLON :
	//<format_expr> ::= <simple_expr> tkColon <simple_expr>
         
		{
			format_expr _format_expr=new format_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),null);
			parsertools.create_source_context(_format_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _format_expr;
		}

	case (int)RuleConstants.RULE_FORMAT_EXPR_TKCOLON_TKCOLON :
	//<format_expr> ::= <simple_expr> tkColon <simple_expr> tkColon <simple_expr>
         
		{
			format_expr _format_expr=new format_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2),(expression)LRParser.GetReductionSyntaxNode(4));
			parsertools.create_source_context(_format_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(4));
			
			return _format_expr;
		}

	case (int)RuleConstants.RULE_RELOP_TKEQUAL :
	//<relop> ::= tkEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_TKNOTEQUAL :
	//<relop> ::= tkNotEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_TKLOWER :
	//<relop> ::= tkLower
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_TKGREATER :
	//<relop> ::= tkGreater
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_TKLOWEREQUAL :
	//<relop> ::= tkLowerEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_TKGREATEREQUAL :
	//<relop> ::= tkGreaterEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RELOP_TKIN :
	//<relop> ::= tkIn
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_EXPR :
	//<simple_expr> ::= <term>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_SIMPLE_EXPR2 :
	//<simple_expr> ::= <simple_expr> <addop> <term>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_ADDOP_TKPLUS :
	//<addop> ::= tkPlus
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ADDOP_TKMINUS :
	//<addop> ::= tkMinus
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ADDOP_TKOR :
	//<addop> ::= tkOr
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ADDOP_TKXOR :
	//<addop> ::= tkXor
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TYPECAST_OP_TKAS :
	//<typecast_op> ::= tkAs <empty>
 return op_typecast.as_op; 
	case (int)RuleConstants.RULE_TYPECAST_OP_TKIS :
	//<typecast_op> ::= tkIs <empty>
 return op_typecast.is_op; 
	case (int)RuleConstants.RULE_TERM :
	//<term> ::= <factor>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_TERM2 :
	//<term> ::= <term> <mulop> <factor>
         
		{
			bin_expr _bin_expr=new bin_expr(LRParser.GetReductionSyntaxNode(0) as expression,LRParser.GetReductionSyntaxNode(2) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			parsertools.create_source_context(_bin_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _bin_expr;
		}

	case (int)RuleConstants.RULE_TERM3 :
	//<term> ::= <term> <typecast_op> <simple_type_identifier>
         
		{
			typecast_node _typecast_node=new typecast_node((addressed_value)LRParser.GetReductionSyntaxNode(0),(type_definition)LRParser.GetReductionSyntaxNode(2),(op_typecast)LRParser.GetReductionSyntaxNode(1));
			parsertools.create_source_context(_typecast_node,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
								if (!(LRParser.GetReductionSyntaxNode(0) is addressed_value)) 
									errors.Add(new Errors.bad_operand_type(current_file_name,((syntax_tree_node)LRParser.GetReductionSyntaxNode(0)).source_context,_typecast_node));
								
			return _typecast_node;
		}

	case (int)RuleConstants.RULE_MULOP_TKSTAR :
	//<mulop> ::= tkStar
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_MULOP_TKSLASH :
	//<mulop> ::= tkSlash
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_MULOP_TKDIV :
	//<mulop> ::= tkDiv
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_MULOP_TKMOD :
	//<mulop> ::= tkMod
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_MULOP_TKSHL :
	//<mulop> ::= tkShl
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_MULOP_TKSHR :
	//<mulop> ::= tkShr
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_MULOP_TKAND :
	//<mulop> ::= tkAnd
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FACTOR_TKNIL :
	//<factor> ::= tkNil <empty>
         
		{
			nil_const _nil_const=new nil_const();
			 parsertools.create_source_context(_nil_const,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _nil_const;
		}

	case (int)RuleConstants.RULE_FACTOR :
	//<factor> ::= <literal_or_number>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_FACTOR_TKSQUAREOPEN_TKSQUARECLOSE :
	//<factor> ::= tkSquareOpen <elem_list> tkSquareClose
         
		{
			pascal_set_constant _pascal_set_constant=new pascal_set_constant(LRParser.GetReductionSyntaxNode(1) as expression_list);
			 parsertools.create_source_context(_pascal_set_constant,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _pascal_set_constant;
		}

	case (int)RuleConstants.RULE_FACTOR_TKNOT :
	//<factor> ::= tkNot <factor>
         
		{
			un_expr _un_expr=new un_expr(LRParser.GetReductionSyntaxNode(1) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(0)).type);
			parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			
			return _un_expr;
		}

	case (int)RuleConstants.RULE_FACTOR2 :
	//<factor> ::= <sign> <factor>
         
		{
			un_expr _un_expr=new un_expr(LRParser.GetReductionSyntaxNode(1) as expression,((op_type_node)LRParser.GetReductionSyntaxNode(0)).type);
			parsertools.create_source_context(_un_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			
			return _un_expr;
		}

	case (int)RuleConstants.RULE_FACTOR_TKDEREF :
	//<factor> ::= tkDeref <factor>
         
		{
			roof_dereference _roof_dereference=new roof_dereference();
			 
							_roof_dereference.dereferencing_value=(addressed_value)LRParser.GetReductionSyntaxNode(1);
							parsertools.create_source_context(_roof_dereference,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _roof_dereference;
		}

	case (int)RuleConstants.RULE_FACTOR3 :
	//<factor> ::= <var_reference>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LITERAL_OR_NUMBER :
	//<literal_or_number> ::= <literal>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LITERAL_OR_NUMBER2 :
	//<literal_or_number> ::= <unsigned_number>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VAR_REFERENCE :
	//<var_reference> ::= <var_address> <variable>
((get_address)LRParser.GetReductionSyntaxNode(0)).address_of=(addressed_value)LRParser.GetReductionSyntaxNode(1);parsertools.create_source_context(NodesStack.Peek(),NodesStack.Peek(),LRParser.GetReductionSyntaxNode(1));return NodesStack.Pop();
	case (int)RuleConstants.RULE_VAR_REFERENCE2 :
	//<var_reference> ::= <variable>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VAR_ADDRESS_TKADDRESSOF :
	//<var_address> ::= tkAddressOf <empty>
         
		{
			get_address _get_address=new get_address();
			 parsertools.assign_source_context(_get_address,LRParser.GetReductionSyntaxNode(0)); NodesStack.Push(_get_address);
			return _get_address;
		}

	case (int)RuleConstants.RULE_VAR_ADDRESS_TKADDRESSOF2 :
	//<var_address> ::= <var_address> tkAddressOf
         
		{
			get_address _get_address=new get_address();
			 ((get_address)LRParser.GetReductionSyntaxNode(0)).address_of=(addressed_value)_get_address;parsertools.create_source_context(_get_address,LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(1));
			return _get_address;
		}

	case (int)RuleConstants.RULE_VARIABLE :
	//<variable> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIABLE2 :
	//<variable> ::= <operator_name_ident>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIABLE_TKINHERITED :
	//<variable> ::= tkInherited <identifier>
         
		{
			inherited_ident _inherited_ident=new inherited_ident();
			 _inherited_ident.name=((ident)LRParser.GetReductionSyntaxNode(1)).name; parsertools.create_source_context(_inherited_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _inherited_ident;
		}

	case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKROUNDCLOSE :
	//<variable> ::= tkRoundOpen <expr> tkRoundClose
parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));return LRParser.GetReductionSyntaxNode(1);
	case (int)RuleConstants.RULE_VARIABLE3 :
	//<variable> ::= <sizeof_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIABLE4 :
	//<variable> ::= <typeof_expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIABLE_TKROUNDOPEN_TKROUNDCLOSE2 :
	//<variable> ::= tkRoundOpen tkRoundClose
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<variable> ::= tkRoundOpen tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_VARIABLE_TKPOINT :
	//<variable> ::= <literal_or_number> tkPoint <identifier_or_keyword>
         
		{
			dot_node _dot_node=new dot_node((addressed_value)LRParser.GetReductionSyntaxNode(0),(addressed_value)LRParser.GetReductionSyntaxNode(2));
			parsertools.create_source_context(_dot_node,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _dot_node;
		}

	case (int)RuleConstants.RULE_VARIABLE5 :
	//<variable> ::= <variable> <var_specifiers>

							if (LRParser.GetReductionSyntaxNode(1) is dot_node) 
							{
							  ((dot_node)LRParser.GetReductionSyntaxNode(1)).left=(addressed_value)LRParser.GetReductionSyntaxNode(0);
							  parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),((dot_node)LRParser.GetReductionSyntaxNode(1)).right);
							}
							else
							if (LRParser.GetReductionSyntaxNode(1) is template_param_list) 
							{
							  ((dot_node)(((template_param_list)LRParser.GetReductionSyntaxNode(1)).dereferencing_value)).left=(addressed_value)LRParser.GetReductionSyntaxNode(0);
                                                          parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
							  parsertools.create_source_context(((template_param_list)LRParser.GetReductionSyntaxNode(1)).dereferencing_value,LRParser.GetReductionSyntaxNode(0),((template_param_list)LRParser.GetReductionSyntaxNode(1)).dereferencing_value);
							}
							else
							if (LRParser.GetReductionSyntaxNode(1) is dereference) 
							{
							  ((dereference)LRParser.GetReductionSyntaxNode(1)).dereferencing_value=(addressed_value)LRParser.GetReductionSyntaxNode(0);
							  parsertools.create_source_context(LRParser.GetReductionSyntaxNode(1),LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
							}
							else
							if (LRParser.GetReductionSyntaxNode(1) is ident_with_templateparams) 
							{
								((ident_with_templateparams)LRParser.GetReductionSyntaxNode(1)).name=(addressed_value_funcname)LRParser.GetReductionSyntaxNode(0);	
							}
							return LRParser.GetReductionSyntaxNode(1);
							
	case (int)RuleConstants.RULE_OPT_EXPR_LIST :
	//<opt_expr_list> ::= <expr_list>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OPT_EXPR_LIST2 :
	//<opt_expr_list> ::= 
	//NONTERMINAL:<opt_expr_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_VAR_SPECIFIERS_TKSQUAREOPEN_TKSQUARECLOSE :
	//<var_specifiers> ::= tkSquareOpen <expr_list> tkSquareClose
         
		{
			indexer _indexer=new indexer((expression_list)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_indexer,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _indexer;
		}

	case (int)RuleConstants.RULE_VAR_SPECIFIERS_TKSQUAREOPEN_TKSQUARECLOSE2 :
	//<var_specifiers> ::= tkSquareOpen tkSquareClose
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<var_specifiers> ::= tkSquareOpen tkSquareClose"));}return null;
	case (int)RuleConstants.RULE_VAR_SPECIFIERS_TKROUNDOPEN_TKROUNDCLOSE :
	//<var_specifiers> ::= tkRoundOpen <opt_expr_list> tkRoundClose
         
		{
			method_call _method_call=new method_call(LRParser.GetReductionSyntaxNode(1) as expression_list);
			 parsertools.create_source_context(_method_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _method_call;
		}

	case (int)RuleConstants.RULE_VAR_SPECIFIERS_TKPOINT :
	//<var_specifiers> ::= tkPoint <identifier_keyword_operatorname>
         
		{
			dot_node _dot_node=new dot_node(null,(addressed_value)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_dot_node,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _dot_node;
		}

	case (int)RuleConstants.RULE_VAR_SPECIFIERS_TKDEREF :
	//<var_specifiers> ::= tkDeref <empty>
         
		{
			roof_dereference _roof_dereference=new roof_dereference();
			 parsertools.assign_source_context(_roof_dereference,LRParser.GetReductionSyntaxNode(0));
			return _roof_dereference;
		}

	case (int)RuleConstants.RULE_VAR_SPECIFIERS_TKAMPERSEND :
	//<var_specifiers> ::= tkAmpersend <template_type_params>
         
		{
			ident_with_templateparams _ident_with_templateparams=new ident_with_templateparams(null,(template_param_list)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_ident_with_templateparams,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _ident_with_templateparams;
		}

	case (int)RuleConstants.RULE_TEMPLATE_TYPE_BACK_VARSPECIFIERS_TKROUNDOPEN_TKROUNDCLOSE :
	//<template_type_back_varspecifiers> ::= tkRoundOpen <expr_list> tkRoundClose
         
		{
			method_call _method_call=new method_call((expression_list)LRParser.GetReductionSyntaxNode(1));
			 parsertools.create_source_context(_method_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			return _method_call;
		}

	case (int)RuleConstants.RULE_TEMPLATE_TYPE_BACK_VARSPECIFIERS_TKROUNDOPEN_TKROUNDCLOSE2 :
	//<template_type_back_varspecifiers> ::= tkRoundOpen tkRoundClose
         
		{
			method_call _method_call=new method_call();
			 parsertools.create_source_context(_method_call,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _method_call;
		}

	case (int)RuleConstants.RULE_ELEM_LIST :
	//<elem_list> ::= <elem_list1>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ELEM_LIST2 :
	//<elem_list> ::= 
	//NONTERMINAL:<elem_list> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_ELEM_LIST1 :
	//<elem_list1> ::= <elem> <empty>
         
		//TemplateList for expression_list (create)
		{
			expression_list _expression_list=new expression_list();
			_expression_list.source_context=((expression)LRParser.GetReductionSyntaxNode(0)).source_context;
			_expression_list.expressions.Add((expression)LRParser.GetReductionSyntaxNode(0));
			return _expression_list;
		}

	case (int)RuleConstants.RULE_ELEM_LIST1_TKCOMMA :
	//<elem_list1> ::= <elem_list1> tkComma <elem>

		//TemplateList for expression_list (add)         
		{
			expression_list _expression_list=(expression_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_expression_list,_expression_list,LRParser.GetReductionSyntaxNode(2));
			_expression_list.expressions.Add(LRParser.GetReductionSyntaxNode(2) as expression);
			return _expression_list;
		}

	case (int)RuleConstants.RULE_ELEM :
	//<elem> ::= <expr>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ELEM_TKDOTDOT :
	//<elem> ::= <expr> tkDotDot <expr>
         
		{
			diapason_expr _diapason_expr=new diapason_expr((expression)LRParser.GetReductionSyntaxNode(0),(expression)LRParser.GetReductionSyntaxNode(2));
			parsertools.create_source_context(_diapason_expr,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(2));
			
			return _diapason_expr;
		}

	case (int)RuleConstants.RULE_ONE_LITERAL_TKSTRINGLITERAL :
	//<one_literal> ::= tkStringLiteral
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ONE_LITERAL_TKASCIICHAR :
	//<one_literal> ::= tkAsciiChar
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_LITERAL :
	//<literal> ::= <literal_list> <empty>
 literal_const_line lcl=(literal_const_line)LRParser.GetReductionSyntaxNode(0);
	                                        if (lcl.literals.Count==1) return lcl.literals[0];
						return lcl;
						
	case (int)RuleConstants.RULE_LITERAL_LIST :
	//<literal_list> ::= <one_literal> <empty>
         
		{
			literal_const_line _literal_const_line=new literal_const_line();
			parsertools.create_source_context(_literal_const_line,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			
						_literal_const_line.literals.Add((literal)LRParser.GetReductionSyntaxNode(0));
						
			return _literal_const_line;
		}

	case (int)RuleConstants.RULE_LITERAL_LIST2 :
	//<literal_list> ::= <literal_list> <one_literal>
         
		{
			literal_const_line _literal_const_line=(literal_const_line)LRParser.GetReductionSyntaxNode(0);
						_literal_const_line.literals.Add((literal)LRParser.GetReductionSyntaxNode(1));
						parsertools.create_source_context(_literal_const_line,_literal_const_line,LRParser.GetReductionSyntaxNode(1));
						
			return _literal_const_line;
		}

	case (int)RuleConstants.RULE_OPERATOR_NAME_IDENT_TKOPERATOR :
	//<operator_name_ident> ::= tkOperator <overload_operator>
         
		{
			operator_name_ident _operator_name_ident=new operator_name_ident(((op_type_node)LRParser.GetReductionSyntaxNode(1)).type);
			
								_operator_name_ident.name=((op_type_node)LRParser.GetReductionSyntaxNode(1)).text;
								parsertools.create_source_context(_operator_name_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(1));
			return _operator_name_ident;
		}

	case (int)RuleConstants.RULE_OPT_METH_MODIFICATORS_TKSEMICOLON :
	//<opt_meth_modificators> ::= tkSemiColon
         
		{
			procedure_attributes_list _procedure_attributes_list=new procedure_attributes_list();
			 parsertools.AddModifier(_procedure_attributes_list,proc_attribute.attr_overload);  parsertools.create_source_context(_procedure_attributes_list,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0)); 
			return _procedure_attributes_list;
		}

	case (int)RuleConstants.RULE_OPT_METH_MODIFICATORS_TKSEMICOLON_TKSEMICOLON :
	//<opt_meth_modificators> ::= tkSemiColon <meth_modificators> tkSemiColon
 parsertools.AddModifier((procedure_attributes_list)LRParser.GetReductionSyntaxNode(1),proc_attribute.attr_overload); return LRParser.GetReductionSyntaxNode(1); 
	case (int)RuleConstants.RULE_METH_MODIFICATORS :
	//<meth_modificators> ::= <meth_modificator> <empty>
         
		//TemplateList for procedure_attributes_list (create)
		{
			procedure_attributes_list _procedure_attributes_list=new procedure_attributes_list();
			_procedure_attributes_list.source_context=((procedure_attribute)LRParser.GetReductionSyntaxNode(0)).source_context;
			_procedure_attributes_list.proc_attributes.Add((procedure_attribute)LRParser.GetReductionSyntaxNode(0));
			return _procedure_attributes_list;
		}

	case (int)RuleConstants.RULE_METH_MODIFICATORS_TKSEMICOLON :
	//<meth_modificators> ::= <meth_modificators> tkSemiColon <meth_modificator>

		//TemplateList for procedure_attributes_list (add)         
		{
			procedure_attributes_list _procedure_attributes_list=(procedure_attributes_list)LRParser.GetReductionSyntaxNode(0);
			parsertools.create_source_context(_procedure_attributes_list,_procedure_attributes_list,LRParser.GetReductionSyntaxNode(2));
			_procedure_attributes_list.proc_attributes.Add(LRParser.GetReductionSyntaxNode(2) as procedure_attribute);
			return _procedure_attributes_list;
		}

	case (int)RuleConstants.RULE_INTEGER_CONST_TKINTEGER :
	//<integer_const> ::= <sign> tkInteger
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<integer_const> ::= <sign> tkInteger"));}return null;
	case (int)RuleConstants.RULE_INTEGER_CONST_TKINTEGER2 :
	//<integer_const> ::= tkInteger
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INTEGER_CONST_TKHEX :
	//<integer_const> ::= <sign> tkHex
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<integer_const> ::= <sign> tkHex"));}return null;
	case (int)RuleConstants.RULE_INTEGER_CONST_TKHEX2 :
	//<integer_const> ::= tkHex
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INTEGER_CONST :
	//<integer_const> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_INTEGER_CONST2 :
	//<integer_const> ::= <sign> <identifier>
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<integer_const> ::= <sign> <identifier>"));}return null;
	case (int)RuleConstants.RULE_IDENTIFIER_TKIDENTIFIER :
	//<identifier> ::= tkIdentifier
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IDENTIFIER :
	//<identifier> ::= <real_type_name>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IDENTIFIER2 :
	//<identifier> ::= <ord_type_name>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IDENTIFIER3 :
	//<identifier> ::= <variant_type_name>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IDENTIFIER4 :
	//<identifier> ::= <meth_modificator>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IDENTIFIER5 :
	//<identifier> ::= <property_specifier_directives>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IDENTIFIER6 :
	//<identifier> ::= <non_reserved>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IDENTIFIER7 :
	//<identifier> ::= <other>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IDENTIFIER_OR_KEYWORD :
	//<identifier_or_keyword> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IDENTIFIER_OR_KEYWORD2 :
	//<identifier_or_keyword> ::= <keyword> <empty>
         
		{
			ident _ident=new ident((LRParser.GetReductionSyntaxNode(0) as token_info).text);
			 parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _ident;
		}

	case (int)RuleConstants.RULE_IDENTIFIER_OR_KEYWORD3 :
	//<identifier_or_keyword> ::= <reserved_keyword> <empty>
         
		{
			ident _ident=new ident((LRParser.GetReductionSyntaxNode(0) as token_info).text);
			 parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _ident;
		}

	case (int)RuleConstants.RULE_IDENTIFIER_KEYWORD_OPERATORNAME :
	//<identifier_keyword_operatorname> ::= <identifier>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_IDENTIFIER_KEYWORD_OPERATORNAME2 :
	//<identifier_keyword_operatorname> ::= <keyword> <empty>
         
		{
			ident _ident=new ident((LRParser.GetReductionSyntaxNode(0) as token_info).text);
			 parsertools.create_source_context(_ident,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _ident;
		}

	case (int)RuleConstants.RULE_IDENTIFIER_KEYWORD_OPERATORNAME3 :
	//<identifier_keyword_operatorname> ::= <operator_name_ident>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_REAL_TYPE_NAME_TKREAL :
	//<real_type_name> ::= tkReal
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_REAL_TYPE_NAME_TKSINGLE :
	//<real_type_name> ::= tkSingle
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_REAL_TYPE_NAME_TKDOUBLE :
	//<real_type_name> ::= tkDouble
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_REAL_TYPE_NAME_TKEXTENDED :
	//<real_type_name> ::= tkExtended
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_REAL_TYPE_NAME_TKCOMP :
	//<real_type_name> ::= tkComp
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKSHORTINT :
	//<ord_type_name> ::= tkShortInt
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKSMALLINT :
	//<ord_type_name> ::= tkSmallInt
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKORDINTEGER :
	//<ord_type_name> ::= tkOrdInteger
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKBYTE :
	//<ord_type_name> ::= tkByte
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKLONGINT :
	//<ord_type_name> ::= tkLongInt
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKINT64 :
	//<ord_type_name> ::= tkInt64
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKWORD :
	//<ord_type_name> ::= tkWord
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKBOOLEAN :
	//<ord_type_name> ::= tkBoolean
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKCHAR :
	//<ord_type_name> ::= tkChar
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKWIDECHAR :
	//<ord_type_name> ::= tkWideChar
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKLONGWORD :
	//<ord_type_name> ::= tkLongWord
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKPCHAR :
	//<ord_type_name> ::= tkPChar
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ORD_TYPE_NAME_TKCARDINAL :
	//<ord_type_name> ::= tkCardinal
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIANT_TYPE_NAME_TKVARIANT :
	//<variant_type_name> ::= tkVariant
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VARIANT_TYPE_NAME_TKOLEVARIANT :
	//<variant_type_name> ::= tkOleVariant
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_METH_MODIFICATOR_TKABSTRACT :
	//<meth_modificator> ::= tkAbstract
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_METH_MODIFICATOR_TKOVERLOAD :
	//<meth_modificator> ::= tkOverload
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_METH_MODIFICATOR_TKREINTRODUCE :
	//<meth_modificator> ::= tkReintroduce
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_METH_MODIFICATOR_TKOVERRIDE :
	//<meth_modificator> ::= tkOverride
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_METH_MODIFICATOR_TKVIRTUAL :
	//<meth_modificator> ::= tkVirtual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_METH_MODIFICATOR_TKSTATIC :
	//<meth_modificator> ::= tkStatic
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKDEFAULT :
	//<property_specifier_directives> ::= tkDefault
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKREAD :
	//<property_specifier_directives> ::= tkRead
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKWRITE :
	//<property_specifier_directives> ::= tkWrite
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKSTORED :
	//<property_specifier_directives> ::= tkStored
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKNODEFAULT :
	//<property_specifier_directives> ::= tkNodefault
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKIMPLEMENTS :
	//<property_specifier_directives> ::= tkImplements
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKWRITEONLY :
	//<property_specifier_directives> ::= tkWriteOnly
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKREADONLY :
	//<property_specifier_directives> ::= tkReadOnly
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_PROPERTY_SPECIFIER_DIRECTIVES_TKDISPID :
	//<property_specifier_directives> ::= tkDispid
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NON_RESERVED_TKAT :
	//<non_reserved> ::= tkAt
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NON_RESERVED_TKABSOLUTE :
	//<non_reserved> ::= tkAbsolute
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NON_RESERVED_TKON :
	//<non_reserved> ::= tkOn
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NON_RESERVED_TKNAME :
	//<non_reserved> ::= tkName
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NON_RESERVED_TKINDEX :
	//<non_reserved> ::= tkIndex
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NON_RESERVED_TKMESSAGE :
	//<non_reserved> ::= tkMessage
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NON_RESERVED_TKCONTAINS :
	//<non_reserved> ::= tkContains
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NON_RESERVED_TKREQUIRES :
	//<non_reserved> ::= tkRequires
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NON_RESERVED_TKFORWARD :
	//<non_reserved> ::= tkForward
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NON_RESERVED_TKOUT :
	//<non_reserved> ::= tkOut
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_NON_RESERVED_TKOBJECT :
	//<non_reserved> ::= tkObject
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VISIBILITY_SPECIFIER_TKINTERNAL :
	//<visibility_specifier> ::= tkInternal
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VISIBILITY_SPECIFIER_TKPUBLIC :
	//<visibility_specifier> ::= tkPublic
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VISIBILITY_SPECIFIER_TKPROTECTED :
	//<visibility_specifier> ::= tkProtected
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_VISIBILITY_SPECIFIER_TKPRIVATE :
	//<visibility_specifier> ::= tkPrivate
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OTHER_TKPACKAGE :
	//<other> ::= tkPackage
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OTHER_TKUNIT :
	//<other> ::= tkUnit
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OTHER_TKLIBRARY :
	//<other> ::= tkLibrary
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OTHER_TKEXTERNAL :
	//<other> ::= tkExternal
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OTHER_TKBF :
	//<other> ::= tkBF
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OTHER_TKPARAMS :
	//<other> ::= tkParams
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD :
	//<keyword> ::= <visibility_specifier> <empty>
         
		{
			token_info _token_info=new token_info((LRParser.GetReductionSyntaxNode(0) as ident).name);
			 parsertools.create_source_context(_token_info,LRParser.GetReductionSyntaxNode(0),LRParser.GetReductionSyntaxNode(0));
			return _token_info;
		}

	case (int)RuleConstants.RULE_KEYWORD_TKFINAL :
	//<keyword> ::= tkFinal
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKTEMPLATE :
	//<keyword> ::= tkTemplate
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKOR :
	//<keyword> ::= tkOr
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKTYPEOF :
	//<keyword> ::= tkTypeOf
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKSIZEOF :
	//<keyword> ::= tkSizeOf
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKWHERE :
	//<keyword> ::= tkWhere
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKXOR :
	//<keyword> ::= tkXor
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKAND :
	//<keyword> ::= tkAnd
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKDIV :
	//<keyword> ::= tkDiv
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKMOD :
	//<keyword> ::= tkMod
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKSHL :
	//<keyword> ::= tkShl
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKSHR :
	//<keyword> ::= tkShr
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKNOT :
	//<keyword> ::= tkNot
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKAS :
	//<keyword> ::= tkAs
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKIN :
	//<keyword> ::= tkIn
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKIS :
	//<keyword> ::= tkIs
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKARRAY :
	//<keyword> ::= tkArray
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKBEGIN :
	//<keyword> ::= tkBegin
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKCASE :
	//<keyword> ::= tkCase
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKCLASS :
	//<keyword> ::= tkClass
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKCONST :
	//<keyword> ::= tkConst
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKCONSTRUCTOR :
	//<keyword> ::= tkConstructor
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKDESTRUCTOR :
	//<keyword> ::= tkDestructor
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKDOWNTO :
	//<keyword> ::= tkDownto
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKDO :
	//<keyword> ::= tkDo
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKELSE :
	//<keyword> ::= tkElse
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKEND :
	//<keyword> ::= tkEnd
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKEXCEPT :
	//<keyword> ::= tkExcept
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKFILE :
	//<keyword> ::= tkFile
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKFINALIZATION :
	//<keyword> ::= tkFinalization
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKFINALLY :
	//<keyword> ::= tkFinally
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKFOR :
	//<keyword> ::= tkFor
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKFUNCTION :
	//<keyword> ::= tkFunction
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKIF :
	//<keyword> ::= tkIf
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKIMPLEMENTATION :
	//<keyword> ::= tkImplementation
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKINHERITED :
	//<keyword> ::= tkInherited
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKINITIALIZATION :
	//<keyword> ::= tkInitialization
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKINTERFACE :
	//<keyword> ::= tkInterface
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKPROCEDURE :
	//<keyword> ::= tkProcedure
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKPROPERTY :
	//<keyword> ::= tkProperty
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKRAISE :
	//<keyword> ::= tkRaise
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKRECORD :
	//<keyword> ::= tkRecord
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKREPEAT :
	//<keyword> ::= tkRepeat
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKSET :
	//<keyword> ::= tkSet
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKTRY :
	//<keyword> ::= tkTry
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKTYPE :
	//<keyword> ::= tkType
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKTHEN :
	//<keyword> ::= tkThen
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKTO :
	//<keyword> ::= tkTo
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKUNTIL :
	//<keyword> ::= tkUntil
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKUSES :
	//<keyword> ::= tkUses
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKUSING :
	//<keyword> ::= tkUsing
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKVAR :
	//<keyword> ::= tkVar
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKWHILE :
	//<keyword> ::= tkWhile
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKWITH :
	//<keyword> ::= tkWith
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKNIL :
	//<keyword> ::= tkNil
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKGOTO :
	//<keyword> ::= tkGoto
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKOF :
	//<keyword> ::= tkOf
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKLABEL :
	//<keyword> ::= tkLabel
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_KEYWORD_TKPROGRAM :
	//<keyword> ::= tkProgram
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_RESERVED_KEYWORD_TKOPERATOR :
	//<reserved_keyword> ::= tkOperator
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKMINUS :
	//<overload_operator> ::= tkMinus
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKPLUS :
	//<overload_operator> ::= tkPlus
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKSQUAREOPEN_TKSQUARECLOSE :
	//<overload_operator> ::= tkSquareOpen tkSquareClose
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<overload_operator> ::= tkSquareOpen tkSquareClose"));}return null;
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKROUNDOPEN_TKROUNDCLOSE :
	//<overload_operator> ::= tkRoundOpen tkRoundClose
 {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,"<overload_operator> ::= tkRoundOpen tkRoundClose"));}return null;
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKSLASH :
	//<overload_operator> ::= tkSlash
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKSTAR :
	//<overload_operator> ::= tkStar
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKEQUAL :
	//<overload_operator> ::= tkEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKGREATER :
	//<overload_operator> ::= tkGreater
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKGREATEREQUAL :
	//<overload_operator> ::= tkGreaterEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKLOWER :
	//<overload_operator> ::= tkLower
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKLOWEREQUAL :
	//<overload_operator> ::= tkLowerEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKNOTEQUAL :
	//<overload_operator> ::= tkNotEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKOR :
	//<overload_operator> ::= tkOr
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKXOR :
	//<overload_operator> ::= tkXor
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKAND :
	//<overload_operator> ::= tkAnd
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKDIV :
	//<overload_operator> ::= tkDiv
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKMOD :
	//<overload_operator> ::= tkMod
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKSHL :
	//<overload_operator> ::= tkShl
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKSHR :
	//<overload_operator> ::= tkShr
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKNOT :
	//<overload_operator> ::= tkNot
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKIN :
	//<overload_operator> ::= tkIn
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKADDRESSOF :
	//<overload_operator> ::= tkAddressOf
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR_TKDEREF :
	//<overload_operator> ::= tkDeref
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_OVERLOAD_OPERATOR :
	//<overload_operator> ::= <assign_operator>
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ASSIGN_OPERATOR_TKASSIGN :
	//<assign_operator> ::= tkAssign
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ASSIGN_OPERATOR_TKPLUSEQUAL :
	//<assign_operator> ::= tkPlusEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ASSIGN_OPERATOR_TKMINUSEQUAL :
	//<assign_operator> ::= tkMinusEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ASSIGN_OPERATOR_TKMULTEQUAL :
	//<assign_operator> ::= tkMultEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_ASSIGN_OPERATOR_TKDIVEQUAL :
	//<assign_operator> ::= tkDivEqual
return LRParser.GetReductionSyntaxNode(0);
	case (int)RuleConstants.RULE_EMPTY :
	//<empty> ::= 
	//NONTERMINAL:<empty> ::= 
	return null;
	//ENDNONTERMINAL
	case (int)RuleConstants.RULE_ERROR_TKERROR :
	//<error> ::= tkError
return LRParser.GetReductionSyntaxNode(0);
}
throw new RuleException("Unknown rule");
}  






} 
}
