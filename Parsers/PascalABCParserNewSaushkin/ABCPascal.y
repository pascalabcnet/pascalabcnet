// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
// SSM 21/11/16 ������ ��������� �������� �� ������� ������� (�.�. ������������ � ���������)
%{
// ��� ���������� ����������� � ����� GPPGParser, �������������� ����� ������, ������������ �������� gppg
    public syntax_tree_node root; // �������� ���� ��������������� ������ 

    public List<Error> errors;
    public string current_file_name;
    public int max_errors = 10;
    public PT parsertools;
    public List<compiler_directive> CompilerDirectives;
	public ParserLambdaHelper lambdaHelper = new ParserLambdaHelper();
	
    public GPPGParser(AbstractScanner<PascalABCSavParser.Union, LexLocation> scanner) : base(scanner) { }
%} 

%output=ABCPascalYacc.cs 
%partial
%parsertype GPPGParser

%using PascalABCCompiler.SyntaxTree;
%using PascalABCSavParser;
%using PascalABCCompiler.ParserTools;
%using PascalABCCompiler.Errors;
%using System.Linq;
%using SyntaxVisitors;

%namespace GPPGParserScanner

%YYSTYPE PascalABCSavParser.Union

%start parse_goal

%token <ti> tkDirectiveName tkAmpersend tkColon tkDotDot tkPoint tkRoundOpen tkRoundClose tkSemiColon tkSquareOpen tkSquareClose tkQuestion tkUnderscore tkQuestionPoint tkDoubleQuestion tkQuestionSquareOpen
%token <ti> tkBackSlashRoundOpen
%token <ti> tkSizeOf tkTypeOf tkWhere tkArray tkCase tkClass tkAuto tkStatic tkConst tkConstructor tkDestructor tkElse  tkExcept tkFile tkFor tkForeach tkFunction tkMatch tkWhen
%token <ti> tkIf tkImplementation tkInherited tkInterface tkProcedure tkOperator tkProperty tkRaise tkRecord tkSet tkType tkThen tkUses tkVar tkWhile tkWith tkNil 
%token <ti> tkGoto tkOf tkLabel tkLock tkProgram tkEvent tkDefault tkTemplate tkPacked tkExports tkResourceString tkThreadvar tkSealed tkPartial tkTo tkDownto
%token <ti> tkLoop 
%token <ti> tkSequence tkYield tkShortProgram tkVertParen tkShortSFProgram
%token <id> tkNew
%token <id> tkOn 
%token <id> tkName tkPrivate tkProtected tkPublic tkInternal tkRead tkWrite  
%token <ti> tkParseModeExpression tkParseModeStatement tkParseModeType tkBegin tkEnd 
%token <ti> tkAsmBody tkILCode tkError INVISIBLE
%token <ti> tkRepeat tkUntil tkDo tkComma tkFinally tkTry
%token <ti> tkInitialization tkFinalization tkUnit tkLibrary tkExternal tkParams tkNamespace
%token <op> tkAssign tkPlusEqual tkMinusEqual tkMultEqual tkDivEqual tkMinus tkPlus tkSlash tkStar tkStarStar tkEqual tkGreater tkGreaterEqual tkLower tkLowerEqual 
%token <op> tkNotEqual tkCSharpStyleOr tkArrow tkOr tkXor tkAnd tkDiv tkMod tkShl tkShr tkNot tkAs tkIn tkIs tkImplicit tkExplicit tkAddressOf tkDeref
%token <id> tkDirectiveName tkIdentifier 
%token <stn> tkStringLiteral tkFormatStringLiteral tkAsciiChar
%token <id> tkAbstract tkForward tkOverload tkReintroduce tkOverride tkVirtual tkExtensionMethod 
%token <ex> tkInteger tkBigInteger tkFloat tkHex
%token <id> tkUnknown 

%type <ti> unit_key_word class_or_static
%type <stn> assignment 
%type <stn> optional_array_initializer  
%type <stn> attribute_declarations  
%type <stn> ot_visibility_specifier  
%type <stn> one_attribute attribute_variable 
%type <ex> const_factor const_factor_without_unary_op const_variable_2 const_term const_variable literal_or_number unsigned_number variable_or_literal_or_number 
%type <stn> program_block  
%type <ob> optional_var class_attribute class_attributes class_attributes1 
%type <ob>  lambda_unpacked_params_or_id lambda_list_of_unpacked_params_or_id
%type <stn> member_list_section optional_component_list_seq_end  
%type <stn> const_decl only_const_decl  
%type <stn> const_decl_sect  
%type <td> object_type record_type  
%type <stn> member_list method_decl_list field_or_const_definition_list  
%type <stn> case_stmt    
%type <stn> case_list  
%type <stn> program_decl_sect_list int_decl_sect_list1 inclass_decl_sect_list1 interface_decl_sect_list decl_sect_list decl_sect_list1 inclass_decl_sect_list 
%type <stn> decl_sect_list_proc_func_only
%type <stn> field_or_const_definition abc_decl_sect decl_sect int_decl_sect type_decl simple_type_decl simple_field_or_const_definition res_str_decl_sect 
%type <stn> method_decl_withattr method_or_property_decl property_definition fp_sect 
%type <ex> default_expr tuple 
%type <stn> expr_as_stmt  
%type <stn> exception_block  
%type <stn> external_block  
%type <stn> exception_handler  
%type <stn> exception_handler_list  
%type <stn> exception_identifier  
%type <stn> typed_const_list1 typed_const_list optional_expr_list elem_list optional_expr_list_with_bracket expr_list const_elem_list1 /*const_expr_list*/ case_label_list const_elem_list optional_const_func_expr_list elem_list1  
%type <stn> enumeration_id expr_l1_or_unpacked_list
// %type <stn> expr_l1_list
%type <stn> enumeration_id_list  
%type <ex> const_simple_expr term term1 typed_const typed_const_plus typed_var_init_expression expr expr_with_func_decl_lambda const_expr const_relop_expr elem range_expr const_elem array_const factor factor_without_unary_op relop_expr expr_dq 
%type <ex> lambda_unpacked_params expr_l1 expr_l1_or_unpacked expr_l1_func_decl_lambda expr_l1_for_lambda simple_expr range_term range_factor 
%type <ex> external_directive_ident init_const_expr case_label variable var_reference /*optional_write_expr*/ optional_read_expr simple_expr_or_nothing var_question_point expr_l1_for_question_expr expr_l1_for_new_question_expr
%type <ob> for_cycle_type  
%type <ex> format_expr format_const_expr const_expr_or_nothing /* simple_expr_with_deref_or_nothing simple_expr_with_deref expr_l1_for_indexer*/
%type <stn> foreach_stmt  
%type <stn> for_stmt loop_stmt yield_stmt yield_sequence_stmt
%type <stn> fp_list fp_sect_list  
%type <td> file_type sequence_type 
%type <stn> var_address  
%type <stn> goto_stmt 
%type <id> func_name_ident param_name const_field_name func_name_with_template_args identifier_or_keyword unit_name exception_variable const_name func_meth_name_ident label_name type_decl_identifier template_identifier_with_equal 
%type <id> program_param identifier identifier_keyword_operatorname func_class_name_ident /*optional_identifier*/ visibility_specifier 
%type <id> property_specifier_directives non_reserved 
%type <stn> if_stmt   
%type <stn> initialization_part  
%type <stn> template_arguments label_list ident_or_keyword_pointseparator_list ident_list  param_name_list  
%type <stn> inherited_message  
%type <stn> implementation_part  
%type <stn> interface_part abc_interface_part  
%type <stn> simple_type_list  
%type <ex> literal one_literal
%type <stn> literal_list  
%type <stn> label_decl_sect  
%type <stn> lock_stmt  
%type <stn> func_name proc_name optional_proc_name
%type <ex> new_expr allowable_expr_as_stmt  
%type <stn> parse_goal parts inclass_block block proc_func_external_block
%type <td> exception_class_type_identifier simple_type_identifier //idp  
%type <stn> base_class_name  
%type <stn> base_classes_names_list optional_base_classes
%type <ob> one_compiler_directive optional_head_compiler_directives head_compiler_directives program_heading_2 optional_tk_point program_param_list optional_semicolon
%type <ex> operator_name_ident  
%type <op> const_relop const_addop assign_operator const_mulop relop addop mulop sign overload_operator
%type <ob> typecast_op  
%type <stn> property_specifiers
%type <stn> write_property_specifiers
%type <stn> read_property_specifiers
%type <stn> array_defaultproperty 
%type <stn> meth_modificators optional_method_modificators optional_method_modificators1  
%type <id> meth_modificator property_modificator 
%type <ex> optional_property_initialization
%type <stn> proc_call  
%type <stn> proc_func_constr_destr_decl proc_func_decl inclass_proc_func_decl inclass_proc_func_decl_noclass constr_destr_decl inclass_constr_destr_decl
%type <stn> method_decl proc_func_constr_destr_decl_with_attr proc_func_decl_noclass  
%type <td> method_header proc_type_decl procedural_type_kind proc_header procedural_type constr_destr_header proc_func_header 
%type <td> func_header method_procfunc_header int_func_header int_proc_header
%type <stn> property_interface
%type <stn> program_file  
%type <stn> program_header  
%type <stn> parameter_decl
%type <stn> parameter_decl_list property_parameter_list
%type <ex> const_set  
%type <ex> question_expr question_constexpr new_question_expr  
%type <ex> record_const const_field_list_1 const_field_list  
%type <stn> const_field  
%type <stn> repeat_stmt  
%type <stn> raise_stmt  
%type <td> pointer_type  
%type <stn> attribute_declaration one_or_some_attribute  
%type <stn> stmt_list else_case exception_block_else_branch  compound_stmt  
%type <td> string_type  
%type <ex> sizeof_expr  
%type <stn> simple_property_definition
%type <stn> stmt_or_expression unlabelled_stmt stmt case_item
%type <td> set_type  
%type <ex> as_is_expr as_is_constexpr is_type_expr as_expr power_expr power_constexpr
%type <td> unsized_array_type simple_type_or_ simple_type simple_type_question/*array_name_for_new_expr*/ foreach_stmt_ident_dype_opt fptype type_ref fptype_noproctype array_type 
%type <td> template_param template_empty_param structured_type unpacked_structured_type empty_template_type_reference simple_or_template_type_reference type_ref_or_secific for_stmt_decl_or_assign type_decl_type
%type <stn> type_ref_and_secific_list  
%type <stn> type_decl_sect
%type <stn> try_handler  
%type <ti> class_or_interface_keyword optional_tk_do keyword reserved_keyword  
%type <ex> typeof_expr  
%type <stn> simple_fp_sect   
%type <stn> template_param_list template_empty_param_list template_type_params template_type_empty_params
%type <td> template_type
%type <stn> try_stmt  
%type <stn> uses_clause used_units_list uses_clause_one uses_clause_one_or_empty 
%type <stn> unit_file  
%type <stn> used_unit_name
%type <stn> unit_header  
%type <stn> var_decl_sect  
%type <stn> var_decl var_decl_part /*var_decl_internal*/ field_definition var_decl_with_assign_var_tuple 
%type <stn> var_stmt  
%type <stn> where_part  
%type <stn> where_part_list optional_where_section  
%type <stn> while_stmt  
%type <stn> with_stmt  
%type <ex> variable_as_type dotted_identifier
%type <ex> func_decl_lambda expl_func_decl_lambda
%type <td> lambda_type_ref lambda_type_ref_noproctype
%type <stn> full_lambda_fp_list lambda_simple_fp_sect lambda_function_body lambda_procedure_body common_lambda_body optional_full_lambda_fp_list
%type <ob> field_in_unnamed_object list_fields_in_unnamed_object func_class_name_ident_list rem_lambda variable_list var_ident_list
%type <ti> tkAssignOrEqual
%type <stn> const_pattern_expression pattern deconstruction_or_const_pattern pattern_optional_var collection_pattern tuple_pattern collection_pattern_list_item tuple_pattern_item collection_pattern_var_item match_with pattern_case pattern_cases pattern_out_param pattern_out_param_optional_var 
%type <ob> pattern_out_param_list pattern_out_param_list_optional_var collection_pattern_expr_list tuple_pattern_item_list const_pattern_expr_list

%%

parse_goal                
    : program_file 
		{ root = $1; }
    | unit_file 
		{ root = $1; }
    | parts 
		{ root = $1; }
	| tkShortProgram uses_clause_one_or_empty decl_sect_list_proc_func_only stmt_list	
		{ 
			var stl = $4 as statement_list;
			stl.left_logical_bracket = new token_info("");
			stl.right_logical_bracket = new token_info("");
			var ul = $2 as uses_list;
			root = $$ = NewProgramModule(null, null, ul, new block($3 as declarations, stl, @4), new token_info(""), LexLocation.MergeAll(@2,@3,@4)); 
		}
	| tkShortSFProgram uses_clause_one_or_empty decl_sect_list_proc_func_only stmt_list	
		{
			var stl = $4 as statement_list;
			stl.left_logical_bracket = new token_info("");
			stl.right_logical_bracket = new token_info("");
			var un = new unit_or_namespace(new ident_list("SF"),null);
			var ul = $2 as uses_list;
			if (ul == null)
			//var un1 = new unit_or_namespace(new ident_list("School"),null);
				ul = new uses_list(un,null);
			else ul.Insert(0,un);
			//ul.Add(un1);
			root = $$ = NewProgramModule(null, null, ul, new block($3 as declarations, stl, @$), new token_info(""), LexLocation.MergeAll(@2,@3,@4)); 
		}
    ;

parts
    : tkParseModeExpression expr
        { $$ = $2; }
    | tkParseModeExpression tkType type_decl_identifier
        { $$ = $3; }
    | tkParseModeType variable_as_type
		{ $$ = $2; }
	| tkParseModeStatement stmt_or_expression
        { $$ = $2; }
		
    ;

stmt_or_expression
    : expr 
        { $$ = new expression_as_statement($1,@$);}
    | assignment
        { $$ = $1; }
    | var_stmt
        { $$ = $1; }
    ;
    
optional_head_compiler_directives
    :
        { $$ = null; }
    |  head_compiler_directives
        { $$ = null; }
    ;

head_compiler_directives
    : one_compiler_directive             
        { $$ = null; }
    |  head_compiler_directives one_compiler_directive
        { $$ = null; }
    ;

one_compiler_directive
    : tkDirectiveName tkIdentifier                 
        {
			parsertools.AddErrorFromResource("UNSUPPORTED_OLD_DIRECTIVES",@$);
			$$ = null;
        }
    | tkDirectiveName tkStringLiteral             
        {
			parsertools.AddErrorFromResource("UNSUPPORTED_OLD_DIRECTIVES",@$);
			$$ = null;
        }
    ;

program_file
    : program_header optional_head_compiler_directives uses_clause program_block optional_tk_point
        { 
			$$ = NewProgramModule($1 as program_name, $2, $3 as uses_list, $4, $5, @$);
        }
	;

/* ��� ����� ��� intellisens� ����� ��������� ������ ��� ���������� ����� � ����� */
optional_tk_point 
    : tkPoint
        { $$ = $1; }
    | tkSemiColon
        { $$ = null; }
    | tkColon
        { $$ = null; }
    | tkComma
        { $$ = null; }
    | tkDotDot
        { $$ = null; }
    |
    ;
        
program_header           
    :  
		{ $$ = null; } 
    | tkProgram identifier program_heading_2    
        { $$ = new program_name($2,@$); }
    ;

program_heading_2
    : tkSemiColon
        { $$ = null; }
    | tkRoundOpen program_param_list tkRoundClose tkSemiColon
        { $$ = null; }
    ;

program_param_list
    : program_param 
        { $$ = null; }
    | program_param_list tkComma program_param
        { $$ = null; }
    ;

program_param
    : identifier
        { $$ = $1; }
    ;

program_block
    : program_decl_sect_list compound_stmt     
        { 
			$$ = new block($1 as declarations, $2 as statement_list, @$);
        }
    ;

program_decl_sect_list
    : decl_sect_list
        { $$ = $1; }
    ;

ident_or_keyword_pointseparator_list
    : identifier_or_keyword                                   
        { 
			$$ = new ident_list($1, @$);
		}
    | ident_or_keyword_pointseparator_list tkPoint identifier_or_keyword 
        { 
			$$ = ($1 as ident_list).Add($3, @$);
		}
    ;


uses_clause_one
	: tkUses used_units_list tkSemiColon
		{
			$$ = $2;
			$$.source_context = @$;
		}
	;
	
uses_clause_one_or_empty
	: 
		{ 
			$$ = null; 
		}
	| uses_clause_one
		{
			if (parsertools.build_tree_for_formatter)
				$$ = new uses_closure($1 as uses_list,@$);
			$$ = $1;
		}
	;

uses_clause
    :
		{ 
			$$ = null; 
		}
    | uses_clause uses_clause_one            
        { 
   			if (parsertools.build_tree_for_formatter)
   			{
	        	if ($1 == null)
                {
	        		$$ = new uses_closure($2 as uses_list,@$);
                }
	        	else {
                    ($1 as uses_closure).AddUsesList($2 as uses_list,@$);
                    $$ = $1;
                }
   			}
   			else 
   			{
	        	if ($1 == null)
                {
                    $$ = $2;
                    $$.source_context = @$;
                }
	        	else 
                {
                    ($1 as uses_list).AddUsesList($2 as uses_list,@$);
                    $$ = $1;
                    $$.source_context = @$;
                }
			}
		}
    ;

used_units_list
    : used_unit_name                              
        { 
		  $$ = new uses_list($1 as unit_or_namespace,@$);
        }
    | used_units_list tkComma used_unit_name
        { 
		  $$ = ($1 as uses_list).Add($3 as unit_or_namespace, @$);
        } 
    ;

used_unit_name
    : ident_or_keyword_pointseparator_list      
        { 
			$$ = new unit_or_namespace($1 as ident_list,@$); 
		}
    | ident_or_keyword_pointseparator_list tkIn tkStringLiteral
        { 
        	if ($3 is char_const _cc)
        		$3 = new string_const(_cc.cconst.ToString());
			$$ = new uses_unit_in($1 as ident_list, $3 as string_const, @$);
        }
    ;

unit_file
    : attribute_declarations unit_header interface_part implementation_part initialization_part tkPoint
        { 
			$$ = new unit_module($2 as unit_name, $3 as interface_node, $4 as implementation_node, ($5 as initfinal_part).initialization_sect, ($5 as initfinal_part).finalization_sect, $1 as attribute_list, @$);                    
		}
    | attribute_declarations unit_header abc_interface_part initialization_part tkPoint
        { 
			$$ = new unit_module($2 as unit_name, $3 as interface_node, null, ($4 as initfinal_part).initialization_sect, ($4 as initfinal_part).finalization_sect, $1 as attribute_list, @$);
        }
    ;

unit_header
    : unit_key_word unit_name tkSemiColon optional_head_compiler_directives
        { 
			$$ = NewUnitHeading(new ident($1.text, @1), $2, @$); 
		}
    | tkNamespace ident_or_keyword_pointseparator_list tkSemiColon optional_head_compiler_directives
        {
            $$ = NewNamespaceHeading(new ident($1.text, @1), $2 as ident_list, @$);
        }
    ;

unit_key_word
    : tkUnit
		{ $$ = $1; }
    | tkLibrary
		{ $$ = $1; }
    ;

unit_name
    : identifier
		{ $$ = $1; }
    ;

interface_part
    : tkInterface uses_clause interface_decl_sect_list 
        { 
			$$ = new interface_node($3 as declarations, $2 as uses_list, null, LexLocation.MergeAll(@1,@2,@3)); 
        }
    ;

implementation_part
    : tkImplementation uses_clause decl_sect_list
        { 
			$$ = new implementation_node($2 as uses_list, $3 as declarations, null, LexLocation.MergeAll(@1,@2,@3)); 
        }
	;
		
abc_interface_part
    : uses_clause decl_sect_list    
        { 
			$$ = new interface_node($2 as declarations, $1 as uses_list, null, null); 
        }
    ;

initialization_part                                          
    : tkEnd                                              
        { 
			$$ = new initfinal_part(); 
			$$.source_context = @$;
		}
    | tkInitialization stmt_list tkEnd                 
        { 
		  $$ = new initfinal_part($1, $2 as statement_list, $3, null, null, @$);
        }
    | tkInitialization stmt_list tkFinalization stmt_list tkEnd
        { 
		  $$ = new initfinal_part($1, $2 as statement_list, $3, $4 as statement_list, $5, @$);
        }
    | tkBegin stmt_list tkEnd                          
        { 
		  $$ = new initfinal_part($1, $2 as statement_list, $3, null, null, @$);
        }
    ;

interface_decl_sect_list
    : int_decl_sect_list1         
        {
			if (($1 as declarations).Count > 0) 
				$$ = $1; 
			else 
				$$ = $1;
		}
	;
	
int_decl_sect_list1
    :                                      
        { 
			$$ = new declarations();  
			if (GlobalDecls==null) 
				GlobalDecls = $$ as declarations;
		}
    | int_decl_sect_list1 int_decl_sect      
        { 
			$$ = ($1 as declarations).Add($2 as declaration, @$);
        } 
    ;

decl_sect_list
    : decl_sect_list1                      
        {
			if (($1 as declarations).Count > 0) 
				$$ = $1; 
			else 
				$$ = $1;
		}
	;
	
decl_sect_list_proc_func_only
	:
        { 
			$$ = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = $$ as declarations;
		}
	| decl_sect_list_proc_func_only proc_func_decl_noclass
		{
			var dcl = $1 as declarations;
			if (dcl.Count == 0)			
				$$ = dcl.Add($2 as declaration, @2);
			else
			{
				var sc = dcl.source_context;
				sc = sc.Merge($2.source_context);
				$$ = dcl.Add($2 as declaration, @2);
				$$.source_context = sc;			
			}
		}		
	;
	
decl_sect_list1
    :                        
        { 
			$$ = new declarations(); 
			if (GlobalDecls==null) 
				GlobalDecls = $$ as declarations;
		}
    | decl_sect_list1 decl_sect     
        { 
			$$ = ($1 as declarations).Add($2 as declaration, @$);
        } 
	;
	
inclass_decl_sect_list
    : inclass_decl_sect_list1                  
        {
			if (($1 as declarations).Count > 0) 
				$$ = $1; 
			else 
				$$ = $1;
		}
	;
	
inclass_decl_sect_list1
    :                        
        { 
        	$$ = new declarations(); 
        }
    | inclass_decl_sect_list1 abc_decl_sect   
        { 
			$$ = ($1 as declarations).Add($2 as declaration, @$);
        } 
	;
	
int_decl_sect
    : const_decl_sect
		{ $$ = $1; }
    | res_str_decl_sect
		{ $$ = $1; }
    | type_decl_sect
		{ $$ = $1; }
    | var_decl_sect
		{ $$ = $1; }
    | int_proc_header
		{ $$ = $1; }
    | int_func_header
		{ $$ = $1; }
    ;

decl_sect
    : label_decl_sect
		{ $$ = $1; }
    | const_decl_sect
		{ $$ = $1; }
    | res_str_decl_sect
		{ $$ = $1; }
    | type_decl_sect
		{ $$ = $1; }
    | var_decl_sect
		{ $$ = $1; }
    | proc_func_constr_destr_decl_with_attr
		{ $$ = $1; }
    ;

/* SSM 2.1.13 ��������� ���������� */
proc_func_constr_destr_decl 
	: proc_func_decl              
		{ $$ = $1; }
	| constr_destr_decl 
		{ $$ = $1; }
	;

proc_func_constr_destr_decl_with_attr
	: attribute_declarations proc_func_constr_destr_decl
		{
		    ($2 as procedure_definition).AssignAttrList($1 as attribute_list);
			$$ = $2;
		}
	;
	
abc_decl_sect
    : label_decl_sect
		{ $$ = $1; }
    | const_decl_sect
		{ $$ = $1; }
    | res_str_decl_sect
		{ $$ = $1; }
    | type_decl_sect
		{ $$ = $1; }
    | var_decl_sect
		{ $$ = $1; }
    ;

int_proc_header
    : attribute_declarations proc_header
        {  
			$$ = $2;
			($$ as procedure_header).attributes = $1 as attribute_list;
        }
    | attribute_declarations proc_header tkForward tkSemiColon    
        {  
			$$ = NewProcedureHeader($1 as attribute_list, $2 as procedure_header, $3 as procedure_attribute, @$);
		}
    ;

int_func_header                           
    : attribute_declarations func_header
        {  
			$$ = $2;
			($$ as procedure_header).attributes = $1 as attribute_list;
        }
    | attribute_declarations func_header tkForward tkSemiColon       
        {  
			$$ = NewProcedureHeader($1 as attribute_list, $2 as procedure_header, $3 as procedure_attribute, @$);
		}
    ;

label_decl_sect
    : tkLabel label_list tkSemiColon           
        { 
			$$ = new label_definitions($2 as ident_list, @$); 
		}
    ;

label_list
    : label_name                               
        { 
			$$ = new ident_list($1, @$);
		}
    | label_list tkComma label_name          
        { 
			$$ = ($1 as ident_list).Add($3, @$);
		}
    ;

label_name
    : tkInteger                           
        { 
			$$ = new ident($1.ToString(), @$);
		}
    | identifier
		{ 
			$$ = $1; 
		}
    ;

const_decl_sect
    : tkConst const_decl                       
        { 
			$$ = new consts_definitions_list($2 as const_definition, @$);
		}
    | const_decl_sect const_decl          
        { 
			$$ = ($1 as consts_definitions_list).Add($2 as const_definition, @$);
		} 
    ;

res_str_decl_sect
    : tkResourceString const_decl
		{ 
			$$ = $2; 
		}
    | res_str_decl_sect const_decl
		{ 
			$$ = $1; 
		}
    ;

type_decl_sect
    : tkType type_decl                         
        { 
            $$ = new type_declarations($2 as type_declaration, @$);
		}
    | type_decl_sect type_decl        
        { 
			$$ = ($1 as type_declarations).Add($2 as type_declaration, @$);
		} 
    ;

var_decl_with_assign_var_tuple
	: var_decl 
		{ 
			$$ = $1; 
		}
	| tkRoundOpen identifier tkComma ident_list tkRoundClose tkAssign expr_l1 tkSemiColon
		{
			($4 as ident_list).Insert(0,$2);
			$4.source_context = LexLocation.MergeAll(@1,@2,@3,@4,@5);
			$$ = new var_tuple_def_statement($4 as ident_list, $7, @$);
		}
	;

var_decl_sect
    : tkVar var_decl_with_assign_var_tuple                     
        { 
			$$ = new variable_definitions($2 as var_def_statement, @$);
		}
    | tkEvent var_decl_with_assign_var_tuple
        { 
			$$ = new variable_definitions($2 as var_def_statement, @$);                        
			($2 as var_def_statement).is_event = true;
        }
    | var_decl_sect var_decl_with_assign_var_tuple              
        { 
			$$ = ($1 as variable_definitions).Add($2 as var_def_statement, @$);
		} 
    /*| tkVar tkRoundOpen identifier tkComma ident_list tkRoundClose tkAssign expr_l1 tkSemiColon
	    {
			($5 as ident_list).Insert(0,$3);
			$5.source_context = LexLocation.MergeAll(@1,@2,@3,@4,@5,@6);
			$$ = new assign_var_tuple($5 as ident_list, $8, @$);
	    }*/
    ;

const_decl
    : only_const_decl tkSemiColon        
        { $$ = $1; }
    ;

only_const_decl
    : const_name tkEqual init_const_expr       
        { 
			$$ = new simple_const_definition($1, $3, @$); 
		}
    | const_name tkColon type_ref tkEqual typed_const
        { 
			$$ = new typed_const_definition($1, $5, $3, @$);
		} 
    ;

init_const_expr
    : const_expr
		{ $$ = $1; }
    | array_const
		{ $$ = $1; }
    ;

const_name
    : identifier
		{ $$ = $1; }
    ;

//expr_l1_list
//    : expr_l1                                
//        { 
//			$$ = new expression_list($1, @$); 
//		}
//    | expr_l1_list tkComma expr_l1               
//		{
//			$$ = ($1 as expression_list).Add($3, @$); 
//		}
//    ;

const_relop_expr
    : const_simple_expr
		{ 
			$$ = $1; 
		}
    | const_relop_expr const_relop const_simple_expr   
        { 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
	;

const_expr
    : const_relop_expr
		{ 
			$$ = $1; 
		}
    | question_constexpr
		{ 
			$$ = $1; 
		}
	| const_expr tkDoubleQuestion const_relop_expr
		{ $$ = new double_question_node($1 as expression, $3 as expression, @$);}
    ;

question_constexpr
    : const_expr tkQuestion const_expr tkColon const_expr    
        { $$ = new question_colon_expression($1, $3, $5, @$); }
    ;

const_relop
    : tkEqual
		{ $$ = $1; }
    | tkNotEqual
		{ $$ = $1; }
    | tkLower
		{ $$ = $1; }
    | tkGreater
		{ $$ = $1; }
    | tkLowerEqual
		{ $$ = $1; }
    | tkGreaterEqual
		{ $$ = $1; }
    | tkIn
		{ $$ = $1; }
    ;

const_simple_expr
    : const_term
		{ $$ = $1; }
    | const_simple_expr const_addop const_term      
        { $$ = new bin_expr($1, $3, $2.type, @$); }
    ;

const_addop
    : tkPlus
		{ $$ = $1; }
    | tkMinus
		{ $$ = $1; }
    | tkOr
		{ $$ = $1; }
    | tkXor
		{ $$ = $1; }
    ;

as_is_constexpr
    : const_term typecast_op simple_or_template_type_reference       
        { 
			$$ = NewAsIsConstexpr($1, (op_typecast)$2, $3, @$);                                
		}
    ;

power_constexpr
    : const_factor_without_unary_op tkStarStar const_factor 
    	{ $$ = new bin_expr($1, $3, $2.type, @$); }
    | const_factor_without_unary_op tkStarStar power_constexpr
        { $$ = new bin_expr($1,$3,($2).type, @$); }
    | sign power_constexpr
    	{ $$ = new un_expr($2, $1.type, @$); }
    ;
    
const_term
    : const_factor
		{ $$ = $1; }
    | as_is_constexpr
		{ $$ = $1; }
    | power_constexpr
		{ $$ = $1; }
    | const_term const_mulop const_factor                  
        { $$ = new bin_expr($1, $3, $2.type, @$); }
    | const_term const_mulop power_constexpr                             
        { $$ = new bin_expr($1,$3,($2).type, @$); }
    ;

const_mulop
    : tkStar
		{ $$ = $1; }
    | tkSlash
		{ $$ = $1; }
    | tkDiv
		{ $$ = $1; }
    | tkMod
		{ $$ = $1; }
    | tkShl
		{ $$ = $1; }
    | tkShr
		{ $$ = $1; }
    | tkAnd
		{ $$ = $1; }
    ;

const_factor_without_unary_op
	: const_variable
		{ $$ = $1; }
	| tkRoundOpen const_expr tkRoundClose
		{ $$ = $2; }
	;

const_factor
    : const_variable
		{ $$ = $1; }
    | const_set
		{ $$ = $1; }
    | tkNil                        
        { 
			$$ = new nil_const();  
			$$.source_context = @$;
		}
    | tkAddressOf const_factor                         
        { 
			$$ = new get_address($2 as addressed_value, @$);  
		}
    | tkRoundOpen const_expr tkRoundClose              
        { 
	 	    $$ = new bracket_expr($2, @$); 
		}
    | tkNot const_factor                
        { 
			$$ = new un_expr($2, $1.type, @$); 
		}
    | sign const_factor                              
        { 
		    // ������ ��������� ����� ��������
			if ($1.type == Operators.Minus)
			{
			    var i64 = $2 as int64_const;
				if (i64 != null && i64.val == (Int64)Int32.MaxValue + 1)
				{
					$$ = new int32_const(Int32.MinValue,@$);
					break;
				}
				var ui64 = $2 as uint64_const;
				if (ui64 != null && ui64.val == (UInt64)Int64.MaxValue + 1)
				{
					$$ = new int64_const(Int64.MinValue,@$);
					break;
				}
				if (ui64 != null && ui64.val > (UInt64)Int64.MaxValue + 1)
				{
					parsertools.AddErrorFromResource("BAD_INT2",@$);
					break;
				}
			    // ����� ������� ���������� ��������� � �������������� �������
			}
			$$ = new un_expr($2, $1.type, @$); 
		}
    | new_expr
		{ $$ = $1; }
	 | default_expr
		{ $$ = $1; }
//    | tkDeref const_factor              
//        { 
//			$$ = new roof_dereference($2 as addressed_value, @$);
//		}
    ;

const_set
    : tkSquareOpen elem_list tkSquareClose 
        { 
			$$ = new pascal_set_constant($2 as expression_list, @$); 
		}
    | tkVertParen elem_list tkVertParen     
        { 
			$$ = new array_const_new($2 as expression_list, @$);  
		}
    ;

sign
    : tkPlus
		{ $$ = $1; }
    | tkMinus
		{ $$ = $1; }
    ;

const_variable
    : identifier
		{ $$ = $1; }
    | literal // SSM 02.10.18 ��� '123'.Length ��� ������������� ��������
		{ $$ = $1; }
    | unsigned_number
		{ $$ = $1; }
    | tkInherited identifier            
        { 
			$$ = new inherited_ident($2.name, @$);
		}
    | sizeof_expr
		{ $$ = $1; }
    | typeof_expr
		{ $$ = $1; }
/*    | tkRoundOpen const_expr tkRoundClose 
        { 
            if (!parsertools.build_tree_for_formatter) 
            {
                $2.source_context = @$;
                $$ = $2;
            } 
			else $$ = new bracket_expr($2, @$); 
        }*/
    | const_variable const_variable_2        
        {
			$$ = NewConstVariable($1, $2, @$);
        }
    | const_variable tkAmpersend template_type_params                
        {
			$$ = new ident_with_templateparams($1 as addressed_value, $3 as template_param_list, @$);
        }
    | const_variable tkSquareOpen format_const_expr tkSquareClose
        { 
    		var fe = $3 as format_expr;
            if (!parsertools.build_tree_for_formatter)
            {
                if (fe.expr == null)
                    fe.expr = new int32_const(int.MaxValue,@3);
                if (fe.format1 == null)
                    fe.format1 = new int32_const(int.MaxValue,@3);
            }
    		$$ = new slice_expr($1 as addressed_value,fe.expr,fe.format1,fe.format2,@$);
		}
    ;

const_variable_2
    : tkPoint identifier_or_keyword                               
        { 
			$$ = new dot_node(null, $2 as addressed_value, @$); 
		}
    | tkDeref                                            
        { 
			$$ = new roof_dereference();  
			$$.source_context = @$;
		}
    | tkRoundOpen optional_const_func_expr_list tkRoundClose 
        { 
			$$ = new method_call($2 as expression_list, @$);  
		}
    | tkSquareOpen const_elem_list tkSquareClose     
        { 
			$$ = new indexer($2 as expression_list, @$);  
		}
    ;

optional_const_func_expr_list
    : expr_list
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;
        
/*const_expr_list
    : const_expr                               
        { 	
			$$ = new expression_list($1, @$);
		}
    | const_expr_list tkComma const_expr 
        { 
			$$ = ($1 as expression_list).Add($3, @$);
		}
    ;*/

const_elem_list
    : const_elem_list1
		{ $$ = $1; }
    |
    ;

const_elem_list1
    : const_elem                              
        { 
			$$ = new expression_list($1, @$);
		}
    | const_elem_list1 tkComma const_elem 
        { 
			$$ = ($1 as expression_list).Add($3, @$);
		}
    ;

const_elem
    : const_expr
		{ $$ = $1; }
    | const_expr tkDotDot const_expr      
        { 
			$$ = new diapason_expr($1, $3, @$); 
		}
    ;

unsigned_number
    : tkInteger
		{ $$ = $1; }
    | tkHex
		{ $$ = $1; }
    | tkFloat
		{ $$ = $1; }
    | tkBigInteger
		{ $$ = $1; }
    ;

typed_const
    : const_expr
		{ $$ = $1; }
    | array_const
		{ $$ = $1; }
    | record_const
		{ $$ = $1; }
    ;

array_const
    : tkRoundOpen typed_const_list tkRoundClose
        { 
			$$ = new array_const($2 as expression_list, @$); 
		}
//    | tkRoundOpen record_const tkRoundClose    
//        { $$ = $2; }
//    | tkRoundOpen array_const tkRoundClose     
//        { $$ = $2; }
    ;

typed_const_list
	: 
	| typed_const_list1
		{
			$$ = $1;
		}
	;
	
typed_const_list1
    : typed_const_plus  
        { 
			$$ = new expression_list($1, @$);
        }
    | typed_const_list1 tkComma typed_const_plus   
        { 
			$$ = ($1 as expression_list).Add($3, @$);
		} 
	;
	
record_const
    : tkRoundOpen const_field_list tkRoundClose 
        {
			$$ = $2;
			$$.source_context = @$;
		}
    ;

const_field_list
    : const_field_list_1
		{ $$ = $1; }
    | const_field_list_1 tkSemiColon           
        { $$ = $1; }
	;
	
const_field_list_1
    : const_field                                 
        { 
			$$ = new record_const($1 as record_const_definition, @$);
		}
    | const_field_list_1 tkSemiColon const_field  
        { 
			$$ = ($1 as record_const).Add($3 as record_const_definition, @$);
		}
    ;

const_field
    : const_field_name tkColon typed_const     
        { 
			$$ = new record_const_definition($1, $3, @$); 
		}
    ;

const_field_name
    : identifier
		{ $$ = $1; }
    ;

type_decl 
    : attribute_declarations simple_type_decl
        {  
			($2 as declaration).attributes = $1 as attribute_list;
			$$ = $2;
			$$.source_context = @2;
        }
    ;

attribute_declarations
    : attribute_declaration 
    { 
			$$ = new attribute_list($1 as simple_attribute_list, @$);
    }
    | attribute_declarations attribute_declaration
        { 
			$$ = ($1 as attribute_list).Add($2 as simple_attribute_list, @$);
		}
    | 
		{ $$ = null; }
    ;
    
attribute_declaration
    : tkSquareOpen one_or_some_attribute tkSquareClose
        { $$ = $2; }
    ;

one_or_some_attribute
    : one_attribute 
        { 
			$$ = new simple_attribute_list($1 as attribute, @$);
		}
    | one_or_some_attribute tkComma one_attribute
        { 
			$$ = ($1 as simple_attribute_list).Add($3 as attribute, @$);
		}
    ;
        
one_attribute
    : attribute_variable
		{ $$ = $1; }
    | identifier tkColon attribute_variable
        {  
			($3 as attribute).qualifier = $1;
			$$ = $3;
			$$.source_context = @$;
        }
    ;
        
simple_type_decl
    : type_decl_identifier tkEqual type_decl_type tkSemiColon 
        { 
			$$ = new type_declaration($1, $3, @$); 
		}
    | template_identifier_with_equal type_decl_type tkSemiColon 
        { 
			$$ = new type_declaration($1, $2, @$); 
		}
    ;

type_decl_identifier
    : identifier
		{ $$ = $1; }
    | identifier template_arguments           
        { 
			$$ = new template_type_name($1.name, $2 as ident_list, @$); 
        }
	;
	
template_identifier_with_equal
    : identifier tkLower ident_list tkGreaterEqual   
        { 
			$$ = new template_type_name($1.name, $3 as ident_list, @$); 
        }
    ;

type_decl_type
    : type_ref
		{ $$ = $1; }
    | object_type
		{ $$ = $1; }
    ;

simple_type_question
	: simple_type tkQuestion
		{
            if (parsertools.build_tree_for_formatter)
   			{
                $$ = $1;
            }
            else
            {
                var l = new List<ident>();
                l.Add(new ident("System"));
                l.Add(new ident("Nullable"));
                $$ = new template_type_reference(new named_type_reference(l), new template_param_list($1), @$);
            }
		}
	| template_type tkQuestion
		{
            if (parsertools.build_tree_for_formatter)
   			{
                $$ = $1;
            }
            else
            {
                var l = new List<ident>();
                l.Add(new ident("System"));
                l.Add(new ident("Nullable"));
                $$ = new template_type_reference(new named_type_reference(l), new template_param_list($1), @$);
            }
		}
	;	

type_ref
    : simple_type
		{ $$ = $1; }
	| simple_type_question
		{ $$ = $1; }
    | string_type
		{ $$ = $1; }
    | pointer_type
		{ $$ = $1; }
    | structured_type
		{ $$ = $1; }
    | procedural_type
		{ $$ = $1; }
    | template_type
		{ $$ = $1; }
	    ;

template_type
    : simple_type_identifier template_type_params    
        { 
			$$ = new template_type_reference($1 as named_type_reference, $2 as template_param_list, @$); 
		}
    ;

template_type_params
    : tkLower template_param_list tkGreater            
        {
			$$ = $2;
			$$.source_context = @$;
		}
    ;

template_type_empty_params
    :   tkNotEqual            
        {
            var ntr = new named_type_reference(new ident(""), @$);
            
			$$ = new template_param_list(ntr, @$);
            ntr.source_context = new SourceContext($$.source_context.end_position.line_num, $$.source_context.end_position.column_num, $$.source_context.begin_position.line_num, $$.source_context.begin_position.column_num);
		}
    |   tkLower template_empty_param_list tkGreater            
        {
			$$ = $2;
			$$.source_context = @$;
		}
    ;
    
template_param_list
    : template_param                              
        { 
			$$ = new template_param_list($1, @$);
		}
    | template_param_list tkComma template_param  
        { 
			$$ = ($1 as template_param_list).Add($3, @$);
		}
    ;

template_empty_param_list
    : template_empty_param                              
        { 
			$$ = new template_param_list($1, @$);
		}
    | template_empty_param_list tkComma template_empty_param  
        { 
			$$ = ($1 as template_param_list).Add($3, @$);
		}
    ;

template_empty_param
    : 
        { 
            $$ = new named_type_reference(new ident(""), @$);
        }
    ;
    
template_param
    : simple_type
		{ $$ = $1; }
    | simple_type tkQuestion
		{
            if (parsertools.build_tree_for_formatter)
   			{
                $$ = $1;
            }
            else
            {
                var l = new List<ident>();
                l.Add(new ident("System"));
                l.Add(new ident("Nullable"));
                $$ = new template_type_reference(new named_type_reference(l), new template_param_list($1), @$);
            }
		}
    | structured_type
		{ $$ = $1; }
    | procedural_type
		{ $$ = $1; }
    | template_type
		{ $$ = $1; }
    ;

simple_type
    : range_expr
	    {
	    	$$ = parsertools.ConvertDotNodeOrIdentToNamedTypeReference($1); 
	    }
    | range_expr tkDotDot range_expr  
        { 
			$$ = new diapason($1, $3, @$); 
		}
    | tkRoundOpen enumeration_id_list tkRoundClose
        { 
			$$ = new enum_type_definition($2 as enumerator_list, @$);  
		}
    ;

range_expr
    : range_term
		{ $$ = $1; }
    | range_expr const_addop range_term    
        { 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
    ;

range_term
    : range_factor
		{ $$ = $1; }
    | range_term const_mulop range_factor  
        { 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
    ;

range_factor
    : simple_type_identifier                   
        { 
			$$ = parsertools.ConvertNamedTypeReferenceToDotNodeOrIdent($1 as named_type_reference);
        }
    | unsigned_number
		{ $$ = $1; }
    | sign range_factor           
        { 
			$$ = new un_expr($2, $1.type, @$); 
		}
    | literal
		{ $$ = $1; }
    | range_factor tkRoundOpen const_elem_list tkRoundClose
        { 
			$$ = new method_call($1 as addressed_value, $3 as expression_list, @$);
        }
    ;

simple_type_identifier        
    : identifier                              
        { 
			$$ = new named_type_reference($1, @$);
		}
    | simple_type_identifier tkPoint identifier_or_keyword
        { 
			$$ = ($1 as named_type_reference).Add($3, @$);
		}
    ;

enumeration_id_list
    : enumeration_id tkComma enumeration_id  
        { 
			$$ = new enumerator_list($1 as enumerator, @$);
			($$ as enumerator_list).Add($3 as enumerator, @$);
        }      
    | enumeration_id_list tkComma enumeration_id
        { 
			$$ = ($1 as enumerator_list).Add($3 as enumerator, @$);
        }  
	;
	
enumeration_id
    : type_ref          
        { 
			$$ = new enumerator($1, null, @$); 
		}
    | type_ref tkEqual expr
        { 
			$$ = new enumerator($1, $3, @$); 
		}
    ;
        
pointer_type
    : tkDeref fptype             
        { 
			$$ = new ref_type($2,@$);
		}
    ;
                            
structured_type
    : unpacked_structured_type
		{ $$ = $1; }
    | tkPacked unpacked_structured_type
		{ $$ = $2; }
    ;

unpacked_structured_type
    : array_type
		{ $$ = $1; }
    | record_type
		{ $$ = $1; }
    | set_type
		{ $$ = $1; }
    | file_type
		{ $$ = $1; }
    | sequence_type
		{ $$ = $1; }
    ;
    
sequence_type
	: tkSequence tkOf type_ref
		{
			$$ = new sequence_type($3,@$);
		}
	;

array_type
    : tkArray tkSquareOpen simple_type_list tkSquareClose tkOf type_ref
        { 
			$$ = new array_type($3 as indexers_types, $6, @$); 
        }
    | unsized_array_type
		{ $$ = $1; }
    ;

unsized_array_type
    : tkArray tkOf type_ref                                              
        { 
			$$ = new array_type(null, $3, @$); 
        }
    ;
    
simple_type_list
    : simple_type_or_                                
        { 
			$$ = new indexers_types($1, @$);
        }
    | simple_type_list tkComma simple_type_or_   
        { 
			$$ = ($1 as indexers_types).Add($3, @$);
        } 
    ;

simple_type_or_
    : simple_type
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;
        
set_type
    : tkSet tkOf type_ref         
        { 
			$$ = new set_type_definition($3, @$); 
		}
    ;

file_type
    : tkFile tkOf type_ref           
        { 
			$$ = new file_type($3, @$);
		}
    | tkFile                               
        { 
			$$ = new file_type();  
			$$.source_context = @$;
		}
    ;

string_type
    : tkIdentifier tkSquareOpen const_expr tkSquareClose     
        { 
			$$ = new string_num_definition($3, $1, @$);
		}
    ;

procedural_type
    : procedural_type_kind
		{ $$ = $1; }
    ;

procedural_type_kind
    : proc_type_decl
		{ $$ = $1; }
    ;

proc_type_decl
    : tkProcedure fp_list 
        { 
			$$ = new procedure_header($2 as formal_parameters,null,null,false,false,null,null,@$);
        }
/*	| tkFunction fp_list
		{
			$$ = new function_header($2 as formal_parameters, null, null, null, null, @$);
		}*/
    | tkFunction fp_list tkColon fptype       
        { 
			$$ = new function_header($2 as formal_parameters, null, null, null, $4 as type_definition, @$);
        }
	| simple_type_identifier tkArrow template_param // ��� 2 ������� ������ ���������� � ���� template_param - ����� ��������
    	{
    		$$ = new modern_proc_type($1,null,$3,@$);            
    	}
	| template_type tkArrow template_param
    	{
    		$$ = new modern_proc_type($1,null,$3,@$);            
    	}
    | tkRoundOpen tkRoundClose tkArrow template_param
    	{
    		$$ = new modern_proc_type(null,null,$4,@$);
    	}
    | tkRoundOpen enumeration_id_list tkRoundClose tkArrow template_param
    	{
    		$$ = new modern_proc_type(null,$2 as enumerator_list,$5,@$);
    	}
    | simple_type_identifier tkArrow tkRoundOpen tkRoundClose // ��� 2 ������� ������ ���������� � ���� template_param - ����� ��������
    	{
    		$$ = new modern_proc_type($1,null,null,@$);
    	}
    | template_type tkArrow tkRoundOpen tkRoundClose
    	{
    		$$ = new modern_proc_type($1,null,null,@$);
    	}
    | tkRoundOpen tkRoundClose tkArrow tkRoundOpen tkRoundClose
    	{
    		$$ = new modern_proc_type(null,null,null,@$);
    	}
    | tkRoundOpen enumeration_id_list tkRoundClose tkArrow tkRoundOpen tkRoundClose
    	{
    		$$ = new modern_proc_type(null,$2 as enumerator_list,null,@$);
    	}    
    ;

object_type
    : class_attributes class_or_interface_keyword optional_base_classes optional_where_section optional_component_list_seq_end
        { 
            var cd = NewObjectType((class_attribute)$1, $2, $3 as named_type_reference_list, $4 as where_definition_list, $5 as class_body_list, @$); 
			$$ = cd;
		}
    ;

record_type 
    : tkRecord optional_base_classes optional_where_section member_list_section tkEnd   
        { 
			var nnrt = new class_definition($2 as named_type_reference_list, $4 as class_body_list, class_keyword.Record, null, $3 as where_definition_list, class_attribute.None, false, @$); 
			if (/*nnrt.body!=null && nnrt.body.class_def_blocks!=null && 
				nnrt.body.class_def_blocks.Count>0 &&*/ 
				nnrt.body.class_def_blocks[0].access_mod==null)
			{
                nnrt.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.public_modifer);
			}        
			$$ = nnrt;
		}
    ;

class_attribute
    : tkSealed
		{ $$ = class_attribute.Sealed; }
    | tkPartial
		{ $$ = class_attribute.Partial; }
    | tkAbstract
		{ $$ = class_attribute.Abstract; }
    | tkAuto
		{ $$ = class_attribute.Auto; }
    | tkStatic
		{ $$ = class_attribute.Static; }
    ;
	
class_attributes 
	: 
		{ 
			$$ = class_attribute.None; 
		}
	| class_attributes1
		{
			$$ = $1;
		}
	;

class_attributes1		
	: class_attribute
		{
			$$ = $1;
		}
	| class_attributes1 class_attribute
		{
            if (((class_attribute)$1 & (class_attribute)$2) == (class_attribute)$2)
                parsertools.AddErrorFromResource("ATTRIBUTE_REDECLARED",@2);
			$$  = ((class_attribute)$1) | ((class_attribute)$2);
			//$$ = $1;
		}
	;
		
class_or_interface_keyword						
    : tkClass
		{ $$ = $1; }
    | tkInterface
		{ $$ = $1; }
    | tkTemplate                                  
        { 
			$$ = NewClassOrInterfaceKeyword($1);
		} 
    | tkTemplate tkClass                                 
        { 
			$$ = NewClassOrInterfaceKeyword($1, "c", @$);
		} 
    | tkTemplate tkRecord                 
        { 
			$$ = NewClassOrInterfaceKeyword($1, "r", @$);
		} 
    | tkTemplate tkInterface                              
        { 
			$$ = NewClassOrInterfaceKeyword($1, "i", @$);
		} 
    ;

optional_component_list_seq_end
    :
		{ $$ = null; }
    | member_list_section tkEnd          
        {
			$$ = $1;
			$$.source_context = @$;
		}
    ;

optional_base_classes
    :
    | tkRoundOpen base_classes_names_list tkRoundClose  
        { $$ = $2; }
    ;

base_classes_names_list
    : base_class_name                     
        { 
			$$ = new named_type_reference_list($1 as named_type_reference, @$);
		}
    | base_classes_names_list tkComma base_class_name
        { 
			$$ = ($1 as named_type_reference_list).Add($3 as named_type_reference, @$);
		}
    ;

base_class_name
    : simple_type_identifier
		{ $$ = $1; }
    | template_type
		{ $$ = $1; }
    ;

template_arguments
    : tkLower ident_list tkGreater           
        {
			$$ = $2;
			$$.source_context = @$;
		}
    ;

optional_where_section
    :
		{ $$ = null; }
    | where_part_list
		{ $$ = $1; }
    ;

where_part_list
    : where_part               
        { 
			$$ = new where_definition_list($1 as where_definition, @$);
		}
    | where_part_list where_part    
        { 
			$$ = ($1 as where_definition_list).Add($2 as where_definition, @$);
		} 
    ;

where_part
    : tkWhere ident_list tkColon type_ref_and_secific_list tkSemiColon
        { 
			$$ = new where_definition($2 as ident_list, $4 as where_type_specificator_list, @$); 
		}
    ;

type_ref_and_secific_list
    : type_ref_or_secific             
        { 
			$$ = new where_type_specificator_list($1, @$);
		}
    | type_ref_and_secific_list tkComma type_ref_or_secific  
        { 
			$$ = ($1 as where_type_specificator_list).Add($3, @$);
		}
    ;

type_ref_or_secific
    : type_ref
		{ $$ = $1; }
    | tkClass         
        { 
			$$ = new declaration_specificator(DeclarationSpecificator.WhereDefClass, $1.text, @$); 
		}
    | tkRecord                   
        { 
			$$ = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, $1.text, @$); 
		}
    | tkConstructor              
        { 
			$$ = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, $1.text, @$); 
		}
    ;

member_list_section
    : member_list      
        { 
			$$ = new class_body_list($1 as class_members, @$);
        }
    | member_list_section ot_visibility_specifier member_list
        { 
		    ($3 as class_members).access_mod = $2 as access_modifer_node;
			($1 as class_body_list).Add($3 as class_members,@$);
			
			if (($1 as class_body_list).class_def_blocks[0].Count == 0)
                ($1 as class_body_list).class_def_blocks.RemoveAt(0);
			
			$$ = $1;
        } 
    ;

ot_visibility_specifier
    : tkInternal           
        { $$ = new access_modifer_node(access_modifer.internal_modifer, @$); }
    | tkPublic                   
        { $$ = new access_modifer_node(access_modifer.public_modifer, @$); }
    | tkProtected                
        { $$ = new access_modifer_node(access_modifer.protected_modifer, @$); }
    | tkPrivate                  
        { $$ = new access_modifer_node(access_modifer.private_modifer, @$); }
    ;

member_list
    : 
		{ $$ = new class_members(); }
    | field_or_const_definition_list optional_semicolon         
        { $$ = $1; } 
    | method_decl_list          
        { $$ = $1; }
    | field_or_const_definition_list tkSemiColon method_decl_list
        {  
			($1 as class_members).members.AddRange(($3 as class_members).members);
			($1 as class_members).source_context = @$;
			$$ = $1;
        }
	;
	
ident_list
    : identifier                               
        { 
			$$ = new ident_list($1, @$);
		}
    | ident_list tkComma identifier       
        { 
			$$ = ($1 as ident_list).Add($3, @$);
		}
    ;

optional_semicolon
    :
		{ $$ = null; }
    | tkSemiColon
		{ $$ = $1; }
    ;

field_or_const_definition_list
    : field_or_const_definition                     
        { 
			$$ = new class_members($1 as declaration, @$);
        }
    | field_or_const_definition_list tkSemiColon field_or_const_definition
        {   
			$$ = ($1 as class_members).Add($3 as declaration, @$);
        }  
    ;

field_or_const_definition
    : attribute_declarations simple_field_or_const_definition
        {  
		    ($2 as declaration).attributes = $1 as attribute_list;
			$$ = $2;
        }
    ;
    
method_decl_list
    : method_or_property_decl
        { 
			$$ = new class_members($1 as declaration, @$);
        }
    | method_decl_list method_or_property_decl
        { 
			$$ = ($1 as class_members).Add($2 as declaration, @$);
        }  
    ;

method_or_property_decl
	: method_decl_withattr
		{ $$ = $1; }
	| property_definition
		{ $$ = $1; }
	;
	
simple_field_or_const_definition
    : tkConst only_const_decl            
        { 
			$$ = $2;
			$$.source_context = @$;
		}
    | field_definition
		{ $$ = $1; }
    | class_or_static field_definition       
        { 
			($2 as var_def_statement).var_attr = definition_attribute.Static;
			($2 as var_def_statement).source_context = @$;
			$$ = $2;
        }        
    ;

class_or_static
    : tkStatic 
        { $$ = $1; }
    | tkClass 
        { $$ = $1; }
    ;
    
field_definition
    : var_decl_part
		{ $$ = $1; }
    | tkEvent ident_list tkColon type_ref
        { 
			$$ = new var_def_statement($2 as ident_list, $4, null, definition_attribute.None, true, @$); 
        } 
    ;

method_decl_withattr
    : attribute_declarations method_header
        {  
			($2 as declaration).attributes = $1 as attribute_list;
			$$ = $2;
        }
    | attribute_declarations method_decl
        {  
			($2 as declaration).attributes = $1 as attribute_list;
            if ($2 is procedure_definition && ($2 as procedure_definition).proc_header != null)
                ($2 as procedure_definition).proc_header.attributes = $1 as attribute_list;
			$$ = $2;
     }
    ;

method_decl
    : inclass_proc_func_decl
		{ $$ = $1; }
    | inclass_constr_destr_decl
		{ $$ = $1; }
    ;

method_header
    : class_or_static method_procfunc_header
        { 
			($2 as procedure_header).class_keyword = true;
			$$ = $2;
		}
    | method_procfunc_header
		{ $$ = $1; }
    | constr_destr_header
		{ $$ = $1; }
    ;

method_procfunc_header
    : proc_func_header 
        { 
			$$ = NewProcfuncHeading($1 as procedure_header);
		}
    ;

proc_func_header
    : proc_header
		{ $$ = $1; }
    | func_header
		{ $$ = $1; }
    ;

constr_destr_header	
    : tkConstructor optional_proc_name fp_list optional_method_modificators 
        { 
			$$ = new constructor(null,$3 as formal_parameters,$4 as procedure_attributes_list,$2 as method_name,false,false,null,null,@$);
        }
    | class_or_static tkConstructor optional_proc_name fp_list optional_method_modificators 
        { 
			$$ = new constructor(null,$4 as formal_parameters,$5 as procedure_attributes_list,$3 as method_name,false,true,null,null,@$);
        }
    | tkDestructor optional_proc_name fp_list optional_method_modificators 
        { 
			$$ = new destructor(null,$3 as formal_parameters,$4 as procedure_attributes_list,$2 as method_name, false,false,null,null,@$);
        }
    ;
	
optional_proc_name
    : proc_name
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;

property_definition
    : attribute_declarations simple_property_definition
        {  
			$$ = NewPropertyDefinition($1 as attribute_list, $2 as declaration, @2);
        }
    ;
    
simple_property_definition
    : tkProperty func_name property_interface property_specifiers tkSemiColon array_defaultproperty
        { 
			$$ = NewSimplePropertyDefinition($2 as method_name, $3 as property_interface, $4 as property_accessors, proc_attribute.attr_none, $6 as property_array_default, @$);
        }
    | tkProperty func_name property_interface property_specifiers tkSemiColon property_modificator tkSemiColon array_defaultproperty
        { 
            proc_attribute pa = proc_attribute.attr_none;
            if ($6.name.ToLower() == "virtual")
               	pa = proc_attribute.attr_virtual;
 			else if ($6.name.ToLower() == "override") 
 			    pa = proc_attribute.attr_override;
            else if ($6.name.ToLower() == "abstract") 
 			    pa = proc_attribute.attr_abstract;
			$$ = NewSimplePropertyDefinition($2 as method_name, $3 as property_interface, $4 as property_accessors, pa, $8 as property_array_default, @$);
        }
    | class_or_static tkProperty func_name property_interface property_specifiers tkSemiColon array_defaultproperty
        { 
			$$ = NewSimplePropertyDefinition($3 as method_name, $4 as property_interface, $5 as property_accessors, proc_attribute.attr_none, $7 as property_array_default, @$);
        	($$ as simple_property).attr = definition_attribute.Static;
        }
    | class_or_static tkProperty func_name property_interface property_specifiers tkSemiColon property_modificator tkSemiColon array_defaultproperty
        { 
			parsertools.AddErrorFromResource("STATIC_PROPERTIES_CANNOT_HAVE_ATTRBUTE_{0}",@7,$7.name);        	
        }
	| tkAuto tkProperty func_name property_interface optional_property_initialization tkSemiColon
		{
			$$ = NewSimplePropertyDefinition($3 as method_name, $4 as property_interface, null, proc_attribute.attr_none, null, @$);
			($$ as simple_property).is_auto = true;
			($$ as simple_property).initial_value = $5;
		}
	| class_or_static tkAuto tkProperty func_name property_interface optional_property_initialization tkSemiColon
		{
			$$ = NewSimplePropertyDefinition($4 as method_name, $5 as property_interface, null, proc_attribute.attr_none, null, @$);
			($$ as simple_property).is_auto = true;
			($$ as simple_property).attr = definition_attribute.Static;
			($$ as simple_property).initial_value = $6;
		}
    ;

optional_property_initialization
	: tkAssign expr { $$ = $2; }
	| { $$ = null; }
	;

array_defaultproperty
    :  
		{ $$ = null; }
    | tkDefault tkSemiColon               
        { 
			$$ = new property_array_default();  
			$$.source_context = @$;
		}
    ;

property_interface
    :/*  
		{ $$ = null; }
    |*/ property_parameter_list tkColon fptype 
        { 
			$$ = new property_interface($1 as property_parameter_list, $3, null, @$);
        }
    ;

property_parameter_list
    :
		{ $$ = null; }
    | tkSquareOpen parameter_decl_list tkSquareClose
        { $$ = $2; }
    ;

parameter_decl_list
    : parameter_decl                                  
        { 
			$$ = new property_parameter_list($1 as property_parameter, @$);
		}
    | parameter_decl_list tkSemiColon parameter_decl  
        { 
			$$ = ($1 as property_parameter_list).Add($3 as property_parameter, @$);
		}
    ;

parameter_decl                                            
    : ident_list tkColon fptype         
        { 
			$$ = new property_parameter($1 as ident_list, $3, @$); 
		}
    ;

/*optional_identifier
    : identifier
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;
    
optional_write_expr    
    : var_reference
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;*/
    
optional_read_expr    
    : expr_with_func_decl_lambda
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;

property_specifiers
    :
    | tkRead optional_read_expr write_property_specifiers   
        { 
        	if ($2 == null || $2 is ident) // ����������� ��������
        	{
        		$$ = NewPropertySpecifiersRead($1, $2 as ident, null, null, $3 as property_accessors, @$);
        	}
        	else // ����������� ��������
        	{
				var id = NewId("#GetGen", @2);
                procedure_definition pr = null;
                if (!parsertools.build_tree_for_formatter)
                    pr = CreateAndAddToClassReadFunc($2, id, @2);
				$$ = NewPropertySpecifiersRead($1, id, pr, $2, $3 as property_accessors, @$); // $2 ��������� ��� �������������� 
			}
        }
    | tkWrite unlabelled_stmt read_property_specifiers     
        { 
        	if ($2 is empty_statement)
        	{
        	
        		$$ = NewPropertySpecifiersWrite($1, null, null, null, $3 as property_accessors, @$);
        	}
        	else if ($2 is procedure_call && ($2 as procedure_call).is_ident) // ����������� ��������
        	{
        	
        		$$ = NewPropertySpecifiersWrite($1, ($2 as procedure_call).func_name as ident, null, null, $3 as property_accessors, @$);  // ������ �������� - � ���������������
        	}
        	else // ����������� ��������
        	{
				var id = NewId("#SetGen", @2);
                procedure_definition pr = null;
                if (!parsertools.build_tree_for_formatter)
                    pr = CreateAndAddToClassWriteProc($2 as statement,id,@2);
                if (parsertools.build_tree_for_formatter)
					$$ = NewPropertySpecifiersWrite($1, id, pr, $2 as statement, $3 as property_accessors, @$); // $2 ��������� ��� ��������������
				else $$ = NewPropertySpecifiersWrite($1, id, pr, null, $3 as property_accessors, @$); 	
			}
        }
    ;
write_property_specifiers
    :
    |  tkWrite unlabelled_stmt     
       { 
        	if ($2 is empty_statement)
        	{
        	
        		$$ = NewPropertySpecifiersWrite($1, null, null, null, null, @$);
        	}
        	else if ($2 is procedure_call && ($2 as procedure_call).is_ident)
        	{
        		$$ = NewPropertySpecifiersWrite($1, ($2 as procedure_call).func_name as ident, null, null, null, @$); // ������ �������� - � ���������������
        	}
        	else 
        	{
				var id = NewId("#SetGen", @2);
                procedure_definition pr = null;
                if (!parsertools.build_tree_for_formatter)
                    pr = CreateAndAddToClassWriteProc($2 as statement,id,@2);
                if (parsertools.build_tree_for_formatter)
					$$ = NewPropertySpecifiersWrite($1, id, pr, $2 as statement, null, @$);
				else $$ = NewPropertySpecifiersWrite($1, id, pr, null, null, @$);	
			}
       }
    ;
    
read_property_specifiers
    :
    |  tkRead optional_read_expr
       { 
        	if ($2 == null || $2 is ident)
        	{
        		$$ = NewPropertySpecifiersRead($1, $2 as ident, null, null, null, @$);
        	}
        	else 
        	{
				var id = NewId("#GetGen", @2);
                procedure_definition pr = null;
                if (!parsertools.build_tree_for_formatter)
                    pr = CreateAndAddToClassReadFunc($2,id,@2);
				$$ = NewPropertySpecifiersRead($1, id, pr, $2, null, @$);
			}
       }      
    ;
    
var_decl
    : var_decl_part tkSemiColon              
        { $$ = $1; }
    ;
    
/*var_decl_internal
    : ident_list tkColon type_ref
        { 
			$$ = new var_def_statement($1 as ident_list, $3, null, definition_attribute.None, false, @$);
		}
    | ident_list tkAssign expr
        { 
			$$ = new var_def_statement($1 as ident_list, null, $3, definition_attribute.None, false, @$);		
		}
    | ident_list tkColon type_ref tkAssign expr
        { 
			$$ = new var_def_statement($1 as ident_list, $3, $5, definition_attribute.None, false, @$); 
		}
	;*/

tkAssignOrEqual
	: tkAssign
	| tkEqual
	;

var_decl_part
    : ident_list tkColon type_ref
        { 
			$$ = new var_def_statement($1 as ident_list, $3, null, definition_attribute.None, false, @$);
		}
    | ident_list tkAssign expr_with_func_decl_lambda
        { 
			$$ = new var_def_statement($1 as ident_list, null, $3, definition_attribute.None, false, @$);		
		}
    /*| ident_list tkAssign expl_func_decl_lambda
        { 
			$$ = new var_def_statement($1 as ident_list, null, $3, definition_attribute.None, false, @$);		
		}*/
    | ident_list tkColon type_ref tkAssignOrEqual typed_var_init_expression // typed_const_plus ��� ����� �� ��������� :) �� ���� �� ������ Tuples, ��������� ��� ����������� � ��������� ������� ���������������� �������� 
        { 
			$$ = new var_def_statement($1 as ident_list, $3, $5, definition_attribute.None, false, @$); 
		}
    ;

typed_var_init_expression
	: typed_const_plus 
		{ $$ = $1; }
	| const_simple_expr tkDotDot const_term // SSM 18/01/20
		{ 
		if (parsertools.build_tree_for_formatter)
			$$ = new diapason_expr($1,$3,@$);
		else 
			$$ = new diapason_expr_new($1,$3,@$); 
		}
	| expl_func_decl_lambda
		{ $$ = $1; }
    | identifier tkArrow lambda_function_body
		{  
			var idList = new ident_list($1, @1); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), @1), parametr_kind.none, null, @1), @1);
			$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), @1), $3 as statement_list, @$);
		}
    | tkRoundOpen tkRoundClose lambda_type_ref tkArrow lambda_function_body
		{
			$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, $3, $5 as statement_list, @$);
		}
    | tkRoundOpen typed_const_list tkRoundClose tkArrow lambda_function_body
		{  
		    var el = $2 as expression_list;
		    var cnt = el.Count;
		    
			var idList = new ident_list();
			idList.source_context = @2;
			
			for (int j = 0; j < cnt; j++)
			{
				if (!(el.expressions[j] is ident))
					parsertools.AddErrorFromResource("ONE_TKIDENTIFIER",el.expressions[j].source_context);
				idList.idents.Add(el.expressions[j] as ident);
			}	
				
			var any = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), @2);	
				
			var formalPars = new formal_parameters(new typed_parameters(idList, any, parametr_kind.none, null, @2), @2);
			$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, any, $5 as statement_list, @$);
		}
	;

typed_const_plus
    : typed_const
		{ $$ = $1; }
    /*| new_expr
		{ $$ = $1; }*/
    ;

constr_destr_decl
    : constr_destr_header block   
        { 
			$$ = new procedure_definition($1 as procedure_header, $2 as block, @$);
        }
    | tkConstructor optional_proc_name fp_list tkAssign unlabelled_stmt tkSemiColon         
        { 
   			if ($5 is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",@6);
            var tmp = new constructor(null,$3 as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),@$),$2 as method_name,false,false,null,null,LexLocation.MergeAll(@1,@2,@3));
            $$ = new procedure_definition(tmp as procedure_header, new block(null,new statement_list($5 as statement,@5),@5), @1.Merge(@5));
            if (parsertools.build_tree_for_formatter)
				$$ = new short_func_definition($$ as procedure_definition);
        }
    | class_or_static tkConstructor optional_proc_name fp_list tkAssign unlabelled_stmt tkSemiColon         
        { 
   			if ($6 is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",@7);
            var tmp = new constructor(null,$4 as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),@$),$3 as method_name,false,true,null,null,LexLocation.MergeAll(@1,@2,@3,@4));
            $$ = new procedure_definition(tmp as procedure_header, new block(null,new statement_list($6 as statement,@6),@6), @1.Merge(@6));
            if (parsertools.build_tree_for_formatter)
				$$ = new short_func_definition($$ as procedure_definition);
        }
    ;

inclass_constr_destr_decl
    : constr_destr_header inclass_block        
        { 
			$$ = new procedure_definition($1 as procedure_header, $2 as block, @$);
        }
    | tkConstructor optional_proc_name fp_list tkAssign unlabelled_stmt tkSemiColon         
        { 
   			if ($5 is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",@6);
            var tmp = new constructor(null,$3 as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),@$),$2 as method_name,false,false,null,null,LexLocation.MergeAll(@1,@2,@3));
            $$ = new procedure_definition(tmp as procedure_header, new block(null,new statement_list($5 as statement,@5),@5), @1.Merge(@5));
            if (parsertools.build_tree_for_formatter)
				$$ = new short_func_definition($$ as procedure_definition);
        }
    | class_or_static tkConstructor optional_proc_name fp_list tkAssign unlabelled_stmt tkSemiColon         
        { 
   			if ($6 is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",@7);
            var tmp = new constructor(null,$4 as formal_parameters,new procedure_attributes_list(new List<procedure_attribute>(),@$),$3 as method_name,false,true,null,null,LexLocation.MergeAll(@1,@2,@3,@4));
            $$ = new procedure_definition(tmp as procedure_header, new block(null,new statement_list($6 as statement,@6),@6), @1.Merge(@6));
            if (parsertools.build_tree_for_formatter)
				$$ = new short_func_definition($$ as procedure_definition);
        }
    ;
	
proc_func_decl
    : proc_func_decl_noclass
		{ $$ = $1; }
    | class_or_static proc_func_decl_noclass             
        { 
			($2 as procedure_definition).proc_header.class_keyword = true;
			$$ = $2;
		}
    ;

proc_func_decl_noclass
    : proc_func_header proc_func_external_block                      
        {
            $$ = new procedure_definition($1 as procedure_header, $2 as proc_block, @$);
        }
	| tkFunction func_name fp_list tkColon fptype optional_method_modificators1 tkAssign expr_l1 tkSemiColon
		{
			$$ = SyntaxTreeBuilder.BuildShortFuncDefinition($3 as formal_parameters, $6 as procedure_attributes_list, $2 as method_name, $5 as type_definition, $8, @1.Merge(@6));
		}
	| tkFunction func_name fp_list optional_method_modificators1 tkAssign expr_l1 tkSemiColon
		{
			if ($6 is dot_question_node)
				parsertools.AddErrorFromResource("DOT_QUECTION_IN_SHORT_FUN",@6);
	
			$$ = SyntaxTreeBuilder.BuildShortFuncDefinition($3 as formal_parameters, $4 as procedure_attributes_list, $2 as method_name, null, $6, @1.Merge(@4));
		}
	| tkFunction func_name fp_list tkColon fptype optional_method_modificators1 tkAssign func_decl_lambda tkSemiColon
		{
			$$ = SyntaxTreeBuilder.BuildShortFuncDefinition($3 as formal_parameters, $6 as procedure_attributes_list, $2 as method_name, $5 as type_definition, $8, @1.Merge(@6));
		}
	| tkFunction func_name fp_list optional_method_modificators1 tkAssign func_decl_lambda tkSemiColon
		{
			$$ = SyntaxTreeBuilder.BuildShortFuncDefinition($3 as formal_parameters, $4 as procedure_attributes_list, $2 as method_name, null, $6, @1.Merge(@4));
		}
	| tkProcedure proc_name fp_list optional_method_modificators1 tkAssign unlabelled_stmt tkSemiColon
		{
			if ($6 is empty_statement)
				parsertools.AddErrorFromResource("EMPTY_STATEMENT_IN_SHORT_PROC_DEFINITION",@6);
			$$ = SyntaxTreeBuilder.BuildShortProcDefinition($3 as formal_parameters, $4 as procedure_attributes_list, $2 as method_name, $6 as statement, @1.Merge(@4));
		}
	| proc_func_header tkForward tkSemiColon
		{
			$$ = new procedure_definition($1 as procedure_header, null, @$);
            ($$ as procedure_definition).proc_header.proc_attributes.Add((procedure_attribute)$2, $2.source_context);
		}
    ;

inclass_proc_func_decl
    : inclass_proc_func_decl_noclass
		{ 
            $$ = $1; 
        }
    | class_or_static inclass_proc_func_decl_noclass         
        { 
		    if (($2 as procedure_definition).proc_header != null)
				($2 as procedure_definition).proc_header.class_keyword = true;
			$$ = $2;
		}
    ;

inclass_proc_func_decl_noclass
    : proc_func_header inclass_block                      
        {
            $$ = new procedure_definition($1 as procedure_header, $2 as proc_block, @$);
		}
	| tkFunction func_name fp_list tkColon fptype optional_method_modificators1 tkAssign expr_l1_func_decl_lambda tkSemiColon
		{
			$$ = SyntaxTreeBuilder.BuildShortFuncDefinition($3 as formal_parameters, $6 as procedure_attributes_list, $2 as method_name, $5 as type_definition, $8, @1.Merge(@6));
			if (parsertools.build_tree_for_formatter)
				$$ = new short_func_definition($$ as procedure_definition);
		}
	| tkFunction func_name fp_list optional_method_modificators1 tkAssign expr_l1_func_decl_lambda tkSemiColon
		{
			$$ = SyntaxTreeBuilder.BuildShortFuncDefinition($3 as formal_parameters, $4 as procedure_attributes_list, $2 as method_name, null, $6, @1.Merge(@4));
			if (parsertools.build_tree_for_formatter)
				$$ = new short_func_definition($$ as procedure_definition);
		}
	| tkProcedure proc_name fp_list optional_method_modificators1 tkAssign unlabelled_stmt tkSemiColon
		{
			$$ = SyntaxTreeBuilder.BuildShortProcDefinition($3 as formal_parameters, $4 as procedure_attributes_list, $2 as method_name, $6 as statement, @1.Merge(@4));
			if (parsertools.build_tree_for_formatter)
				$$ = new short_func_definition($$ as procedure_definition);
		}
    ;

proc_func_external_block
    : block
		{ $$ = $1; }
    | external_block
		{ $$ = $1; }
    ;

proc_name
    : func_name
		{ $$ = $1; }
    ;

func_name
    : func_meth_name_ident                        
        { 
			$$ = new method_name(null,null, $1, null, @$); 
		}
    | func_class_name_ident_list tkPoint func_meth_name_ident  
        { 
        	var ln = $1 as List<ident>;
        	var cnt = ln.Count;
        	if (cnt == 1)
				$$ = new method_name(null, ln[cnt-1], $3, null, @$);
			else 	
				$$ = new method_name(ln, ln[cnt-1], $3, null, @$);
		}
    ;

func_class_name_ident
    : func_name_with_template_args
		{ $$ = $1; }
    ;
    
func_class_name_ident_list
	: func_class_name_ident 
		{ 
			$$ = new List<ident>(); 
			($$ as List<ident>).Add($1);
		}
	| func_class_name_ident_list tkPoint func_class_name_ident 
		{ 
			($1 as List<ident>).Add($3);
			$$ = $1; 
		}
	;

func_meth_name_ident
    : func_name_with_template_args
		{ $$ = $1; }
    | operator_name_ident
		{ $$ = (ident)$1; }
    | operator_name_ident template_arguments
		{ $$ = new template_operator_name(null, $2 as ident_list, $1 as operator_name_ident, @$); }
    ;

func_name_with_template_args
    : func_name_ident
		{ $$ = $1; }
    | func_name_ident template_arguments  
        { 
			$$ = new template_type_name($1.name, $2 as ident_list, @$); 
        }
	;
	
func_name_ident
    : identifier
		{ $$ = $1; }
//    | func_name_ident tkPoint identifier
//		{ $$ = $3; }
    ;

proc_header
    : tkProcedure proc_name fp_list optional_method_modificators optional_where_section 
        { 
        	$$ = new procedure_header($3 as formal_parameters, $4 as procedure_attributes_list, $2 as method_name, $5 as where_definition_list, @$); 
        }
    ;

func_header
    : tkFunction func_name fp_list optional_method_modificators optional_where_section 
		{
			$$ = new function_header($3 as formal_parameters, $4 as procedure_attributes_list, $2 as method_name, $5 as where_definition_list, null, @$); 
		}
	| tkFunction func_name fp_list tkColon fptype optional_method_modificators optional_where_section
        { 
			$$ = new function_header($3 as formal_parameters, $6 as procedure_attributes_list, $2 as method_name, $7 as where_definition_list, $5 as type_definition, @$); 
        }
    ;

external_block
    : tkExternal external_directive_ident tkName external_directive_ident tkSemiColon
        { 
			$$ = new external_directive($2, $4, @$); 
		}
    | tkExternal external_directive_ident tkSemiColon
        { 
			$$ = new external_directive($2, null, @$); 
		}
    | tkExternal tkSemiColon
        { 
			$$ = new external_directive(null, null, @$); 
		}
    ;

external_directive_ident
    : identifier
		{ $$ = $1; }
    | literal
		{ $$ = $1; }
    ;

block
    : decl_sect_list compound_stmt tkSemiColon    
        { 
			$$ = new block($1 as declarations, $2 as statement_list, @$); 
		}
    ;

inclass_block
    : inclass_decl_sect_list compound_stmt tkSemiColon 
        { 
			$$ = new block($1 as declarations, $2 as statement_list, @$); 
		}
    | external_block
		{ $$ = $1; }
    ;

fp_list
    :
		{ $$ = null; }
    | tkRoundOpen tkRoundClose         
        { 
			$$ = null;
		}
    | tkRoundOpen fp_sect_list tkRoundClose         
        { 
			$$ = $2;
			if ($$ != null)
				$$.source_context = @$;
		}
    ;
                                                        
fp_sect_list
    : fp_sect                                          
        { 
			$$ = new formal_parameters($1 as typed_parameters, @$);
        }
    | fp_sect_list tkSemiColon fp_sect               
        { 
			$$ = ($1 as formal_parameters).Add($3 as typed_parameters, @$);   
        } 
    ;

fp_sect
    : attribute_declarations simple_fp_sect
        {  
			($2 as declaration).attributes = $1 as  attribute_list;
			$$ = $2;
        }
    ;
    
simple_fp_sect
    : param_name_list tkColon fptype             
        { 
			$$ = new typed_parameters($1 as ident_list, $3, parametr_kind.none, null, @$); 
		}
    | tkVar param_name_list tkColon fptype
        { 
			$$ = new typed_parameters($2 as ident_list, $4, parametr_kind.var_parametr, null, @$);  
		}
    | tkConst param_name_list tkColon fptype
        { 
			$$ = new typed_parameters($2 as ident_list, $4, parametr_kind.const_parametr, null, @$);  
		}
    | tkParams param_name_list tkColon fptype    
        { 
			$$ = new typed_parameters($2 as ident_list, $4,parametr_kind.params_parametr,null, @$);  
		}
    | param_name_list tkColon fptype tkAssign expr        
        { 
			$$ = new typed_parameters($1 as ident_list, $3, parametr_kind.none, $5, @$); 
		}
    | tkVar param_name_list tkColon fptype tkAssign expr  
        { 
			$$ = new typed_parameters($2 as ident_list, $4, parametr_kind.var_parametr, $6, @$);  
		}
    | tkConst param_name_list tkColon fptype tkAssign expr    
        { 
			$$ = new typed_parameters($2 as ident_list, $4, parametr_kind.const_parametr, $6, @$);  
		}
    ;

param_name_list
    : param_name                                       
        { 
			$$ = new ident_list($1, @$); 
		}
    | param_name_list tkComma param_name      
        { 
			$$ = ($1 as ident_list).Add($3, @$);  
		}
    ;

param_name
    : identifier
		{ $$ = $1; }
    ;

fptype                                                        
    : type_ref
		{ $$ = $1; }
    ;
    
fptype_noproctype
    : simple_type
		{ $$ = $1; }
    | string_type
		{ $$ = $1; }
    | pointer_type
		{ $$ = $1; }
    | structured_type
		{ $$ = $1; }
    | template_type
		{ $$ = $1; }
	    ;

stmt
    : unlabelled_stmt  
		{ $$ = $1; }
    | label_name tkColon stmt         
        { 
			$$ = new labeled_statement($1, $3 as statement, @$);  
		}
    ;

unlabelled_stmt
    :                                        
        { 
			$$ = new empty_statement(); 
			$$.source_context = null;
		}
    | assignment 
		{ $$ = $1; }
    | proc_call
		{ $$ = $1; }
    | goto_stmt
		{ $$ = $1; }
    | compound_stmt
		{ $$ = $1; }
    | if_stmt
		{ $$ = $1; }
    | case_stmt
		{ $$ = $1; }
    | repeat_stmt
		{ $$ = $1; }
    | while_stmt
		{ $$ = $1; }
    | for_stmt
		{ $$ = $1; }
    | with_stmt
		{ $$ = $1; }
    | inherited_message
		{ $$ = $1; }
    | try_stmt
		{ $$ = $1; }
    | raise_stmt
		{ $$ = $1; }
    | foreach_stmt
		{ $$ = $1; }
    | var_stmt
		{ $$ = $1; }
    | expr_as_stmt
		{ $$ = $1; }
    | lock_stmt
		{ $$ = $1; }
	| yield_stmt	
		{ $$ = $1; }
	| yield_sequence_stmt	
		{ $$ = $1; }
	| loop_stmt	
		{ $$ = $1; }
    | match_with
        { $$ = $1; }
    ;
    
loop_stmt
	: tkLoop expr_l1 tkDo unlabelled_stmt 
		{
			$$ = new loop_stmt($2,$4 as statement,@$);
		}
	;
	
yield_stmt
	: tkYield expr_l1_func_decl_lambda
		{
			$$ = new yield_node($2,@$);
		}
	;
	
yield_sequence_stmt
	: tkYield tkSequence expr_l1_func_decl_lambda
		{
			$$ = new yield_sequence_node($3,@$);
		}
	;

var_stmt
    : tkVar var_decl_part
        { 
			$$ = new var_statement($2 as var_def_statement, @$);
		}
    | tkRoundOpen tkVar identifier tkComma var_ident_list tkRoundClose tkAssign expr
		{
			($5 as ident_list).Insert(0,$3);
			($5 as syntax_tree_node).source_context = LexLocation.MergeAll(@1,@2,@3,@4,@5,@6);
			$$ = new assign_var_tuple($5 as ident_list, $8, @$);
		}		
    | tkVar tkRoundOpen identifier tkComma ident_list tkRoundClose tkAssign expr
	    {
			($5 as ident_list).Insert(0,$3);
			$5.source_context = LexLocation.MergeAll(@1,@2,@3,@4,@5,@6);
			$$ = new assign_var_tuple($5 as ident_list, $8, @$);
	    }
    ;

assignment
    : var_reference assign_operator expr_with_func_decl_lambda           
        {      
			$$ = new assign($1 as addressed_value, $3, $2.type, @$);
        }
    | tkRoundOpen variable tkComma variable_list tkRoundClose assign_operator expr
		{
			if ($6.type != Operators.Assignment)
			    parsertools.AddErrorFromResource("ONLY_BASE_ASSIGNMENT_FOR_TUPLE",@6);
			($4 as addressed_value_list).Insert(0,$2 as addressed_value);
			($4 as syntax_tree_node).source_context = LexLocation.MergeAll(@1,@2,@3,@4,@5);
			$$ = new assign_tuple($4 as addressed_value_list, $7, @$);
		}	
    | variable tkQuestionSquareOpen format_expr tkSquareClose assign_operator expr
		{
			var fe = $3 as format_expr;
            if (!parsertools.build_tree_for_formatter)
            {
                if (fe.expr == null)
                    fe.expr = new int32_const(int.MaxValue,@3);
                if (fe.format1 == null)
                    fe.format1 = new int32_const(int.MaxValue,@3);
            }
      		var left = new slice_expr_question($1 as addressed_value,fe.expr,fe.format1,fe.format2,@$);
            $$ = new assign(left, $6, $5.type, @$);
		}
    ;
    
variable_list
	: variable
	{
		$$ = new addressed_value_list($1 as addressed_value,@1);
	}
	| variable_list tkComma variable
	{
		($1 as addressed_value_list).Add($3 as addressed_value);
		($1 as syntax_tree_node).source_context = LexLocation.MergeAll(@1,@2,@3);
		$$ = $1;
	}
	;

var_ident_list
	: tkVar identifier
	{
		$$ = new ident_list($2,@$);
	}
	| var_ident_list tkComma tkVar identifier
	{
		($1 as ident_list).Add($4);
		($1 as ident_list).source_context = LexLocation.MergeAll(@1,@2,@3,@4);
		$$ = $1;
	}
	;

proc_call
    : var_reference                                    
        { 
			$$ = new procedure_call($1 as addressed_value, $1 is ident, @$); 
		}
    ;

goto_stmt
    : tkGoto label_name              
        { 
			$$ = new goto_statement($2, @$); 
		}
    ;

compound_stmt                                             
    : tkBegin stmt_list tkEnd                
        {
			$$ = $2;
			($$ as statement_list).left_logical_bracket = $1;
			($$ as statement_list).right_logical_bracket = $3;
			$$.source_context = @$;
        }
    ;

stmt_list
    : stmt                                             
        { 
			$$ = new statement_list($1 as statement, @1);
        }
    | stmt_list tkSemiColon stmt                     
        {  
			$$ = ($1 as statement_list).Add($3 as statement, @$);
        }
    ;

if_stmt
	: tkIf expr_l1 tkThen unlabelled_stmt
        { 
			$$ = new if_node($2, $4 as statement, null, @$); 
        }
	| tkIf expr_l1 tkThen unlabelled_stmt tkElse unlabelled_stmt
        {
			$$ = new if_node($2, $4 as statement, $6 as statement, @$); 
        }
    ;

match_with
    : tkMatch expr_l1 tkWith pattern_cases else_case tkEnd
        { 
            $$ = new match_with($2, $4 as pattern_cases, $5 as statement, @$);
        }
    | tkMatch expr_l1 tkWith pattern_cases tkSemiColon else_case tkEnd
        { 
            $$ = new match_with($2, $4 as pattern_cases, $6 as statement, @$);
        }
    ;
    
pattern_cases
    : pattern_case
        {
            $$ = new pattern_cases($1 as pattern_case);
        }
    | pattern_cases tkSemiColon pattern_case
        {
            $$ = ($1 as pattern_cases).Add($3 as pattern_case);
        }
    ;
    
pattern_case
    : pattern_optional_var tkWhen expr_l1 tkColon unlabelled_stmt
        {
            $$ = new pattern_case($1 as pattern_node, $5 as statement, $3, @$);
        }
    | deconstruction_or_const_pattern tkColon unlabelled_stmt
        {
            $$ = new pattern_case($1 as pattern_node, $3 as statement, null, @$);
        }
	| collection_pattern tkColon unlabelled_stmt
		{
			$$ = new pattern_case($1 as pattern_node, $3 as statement, null, @$);
		}
	| tuple_pattern tkWhen expr_l1 tkColon unlabelled_stmt
		{
			$$ = new pattern_case($1 as pattern_node, $5 as statement, $3, @$);
		}
	| tuple_pattern tkColon unlabelled_stmt
		{
			$$ = new pattern_case($1 as pattern_node, $3 as statement, null, @$);
		}
    ;
    
case_stmt
    : tkCase expr_l1 tkOf case_list else_case tkEnd 
        { 
			$$ = new case_node($2, $4 as case_variants, $5 as statement, @$); 
		}  
	| tkCase expr_l1 tkOf case_list tkSemiColon else_case tkEnd 
        { 
			$$ = new case_node($2, $4 as case_variants, $6 as statement, @$); 
		}
	| tkCase expr_l1 tkOf else_case tkEnd 
        { 
			$$ = new case_node($2, NewCaseItem(new empty_statement(), null), $4 as statement, @$); 
		}		
    ;

case_list
    : case_item
        {
			if ($1 is empty_statement) 
				$$ = NewCaseItem($1, null);
			else $$ = NewCaseItem($1, @$);
		}
    | case_list tkSemiColon case_item         
        { 
			$$ = AddCaseItem($1 as case_variants, $3, @$);
		} 
    ;

case_item
    : case_label_list tkColon unlabelled_stmt            
        { 
			$$ = new case_variant($1 as expression_list, $3 as statement, @$); 
		}
    ;

case_label_list
    : case_label                               
        { 
			$$ = new expression_list($1, @$);
		}
    |  case_label_list tkComma case_label      
        { 
			$$ = ($1 as expression_list).Add($3, @$);
		}
    ;

case_label
    : const_elem
		{ $$ = $1; }
    ;

else_case
    :
		{ $$ = null;}
    |  tkElse stmt_list                  
        { $$ = $2; }
    ;

repeat_stmt
    : tkRepeat stmt_list tkUntil expr                  
        { 
		    $$ = new repeat_node($2 as statement_list, $4, @$); 
			($2 as statement_list).left_logical_bracket = $1;
			($2 as statement_list).right_logical_bracket = $3;
			$2.source_context = @1.Merge(@3);
        }
	;
	
while_stmt
    : tkWhile expr_l1 optional_tk_do unlabelled_stmt                        
        { 
			$$ = NewWhileStmt($1, $2, $3, $4 as statement, @$);    
        }
    ;

optional_tk_do
    : tkDo
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;
        
lock_stmt
    : tkLock expr_l1 tkDo unlabelled_stmt            
        { 
			$$ = new lock_stmt($2, $4 as statement, @$); 
        }
	;
	
foreach_stmt
    : tkForeach identifier foreach_stmt_ident_dype_opt tkIn expr_l1 tkDo unlabelled_stmt
        { 
			$$ = new foreach_stmt($2, $3, $5, $7 as statement, @$);
            if ($3 == null)
                parsertools.AddWarningFromResource("USING_UNLOCAL_FOREACH_VARIABLE", $2.source_context);
        }
    | tkForeach tkVar identifier tkColon type_ref tkIn expr_l1 tkDo unlabelled_stmt
        { 
			$$ = new foreach_stmt($3, $5, $7, $9 as statement, @$); 
        }
    | tkForeach tkVar identifier tkIn expr_l1 tkDo unlabelled_stmt
        { 
			$$ = new foreach_stmt($3, new no_type_foreach(), $5, (statement)$7, @$); 
        }
    | tkForeach tkVar tkRoundOpen ident_list tkRoundClose tkIn expr_l1 tkDo unlabelled_stmt // �������� �������
        { 
        	if (parsertools.build_tree_for_formatter)
        	{
        		var il = $4 as ident_list;
        		il.source_context = LexLocation.MergeAll(@4,@5); // ����� ��� ��������������
        		$$ = new foreach_stmt_formatting(il,$7,$9 as statement,@$);
        	}
        	else
        	{
        		// ���� �������� - ���������, ��� ����� ������� ������������ ���� ��� ��������
        		// ��������� ����� � � foreach, �� ���-�� ������ ���� ������, ��� ��� �������� ����
        		// ��������, ������������� #fe - �� ��� ������ ����
                var id = NewId("#fe",@4);
                var tttt = new assign_var_tuple($4 as ident_list, id, @$);
                statement_list nine = $9 is statement_list ? $9 as statement_list : new statement_list($9 as statement,@9);
                nine.Insert(0,tttt);
			    var fe = new foreach_stmt(id, new no_type_foreach(), $7, nine, @$);
			    fe.ext = $4 as ident_list;
			    $$ = fe;
			}
        }
    ;

foreach_stmt_ident_dype_opt
    : tkColon type_ref                   
        { $$ = $2; }
    |
    ;
           
for_stmt
    : tkFor optional_var identifier for_stmt_decl_or_assign expr_l1 for_cycle_type expr_l1 optional_tk_do unlabelled_stmt
        { 
			$$ = NewForStmt((bool)$2, $3, $4, $5, (for_cycle_type)$6, $7, $8, $9 as statement, @$);
        }
	;
	
optional_var
    : tkVar
        { $$ = true; }
    |	
		{ $$ = false; }
    ;

for_stmt_decl_or_assign
    : tkAssign
    | tkColon simple_type_identifier tkAssign       
        { $$ = $2; }
    ;

for_cycle_type
    : tkTo                             
        { $$ = for_cycle_type.to; }
    | tkDownto                           
        { $$ = for_cycle_type.downto; }
    ;

with_stmt
    : tkWith expr_list tkDo unlabelled_stmt               
        { 
			$$ = new with_statement($4 as statement, $2 as expression_list, @$); 
		}  
    ;

inherited_message
    : tkInherited                   
        { 
			$$ = new inherited_message();  
			$$.source_context = @$;
		}
    ;

try_stmt
    : tkTry stmt_list try_handler      
        { 
			$$ = new try_stmt($2 as statement_list, $3 as try_handler, @$); 
			($2 as statement_list).left_logical_bracket = $1;
			$2.source_context = @1.Merge(@2);
        } 
    ;

try_handler
    : tkFinally stmt_list tkEnd          
        { 
			$$ = new try_handler_finally($2 as statement_list, @$);
			($2 as statement_list).left_logical_bracket = $1;
			($2 as statement_list).right_logical_bracket = $3;
		}
    | tkExcept exception_block tkEnd    
        { 
			$$ = new try_handler_except((exception_block)$2, @$);  
			if (($2 as exception_block).stmt_list != null)
			{
				($2 as exception_block).stmt_list.source_context = @$;
				($2 as exception_block).source_context = @$;
			}
		}
    ;

exception_block
    : exception_handler_list exception_block_else_branch       
        { 
			$$ = new exception_block(null, (exception_handler_list)$1, (statement_list)$2, @$); 
		}
    |  exception_handler_list tkSemiColon exception_block_else_branch 
        { 
			$$ = new exception_block(null, (exception_handler_list)$1, (statement_list)$3, @$); 
		}
    |  stmt_list                          
        { 
			$$ = new exception_block($1 as statement_list, null, null, @1);
		}
    ;

exception_handler_list
    : exception_handler                                   
        { 
			$$ = new exception_handler_list($1 as exception_handler, @$); 
		}
    | exception_handler_list tkSemiColon exception_handler   
        { 
			$$ = ($1 as exception_handler_list).Add($3 as exception_handler, @$); 
		}
    ;

exception_block_else_branch
    :
        { $$ = null; }
    |  tkElse stmt_list                      
        { $$ = $2; }
    ;

exception_handler
    : tkOn exception_identifier tkDo unlabelled_stmt            
        { 
			$$ = new exception_handler(($2 as exception_ident).variable, ($2 as exception_ident).type_name, $4 as statement, @$);
		}
    ;

exception_identifier
    : exception_class_type_identifier                         
        { 
			$$ = new exception_ident(null, (named_type_reference)$1, @$); 
		}
    | exception_variable tkColon exception_class_type_identifier  
        { 
			$$ = new exception_ident($1, (named_type_reference)$3, @$); 
		}
    ;

exception_class_type_identifier
    : simple_type_identifier 
		{ $$ = $1; }
    ;

exception_variable
    : identifier 
		{ $$ = $1; }
    ;

raise_stmt                                            
    : tkRaise                                    
        { 
			$$ = new raise_stmt(); 
			$$.source_context = @$;
		}
    | tkRaise expr                             
        { 
			$$ = new raise_stmt($2, null, @$);  
		}
    ;

expr_list
    : expr_with_func_decl_lambda                                
        { 
			$$ = new expression_list($1, @$); 
		}
    | expr_list tkComma expr_with_func_decl_lambda              
		{
			$$ = ($1 as expression_list).Add($3, @$); 
		}
    ;

expr_as_stmt
    : allowable_expr_as_stmt      
        { 
			$$ = new expression_as_statement($1, @$);  
		}
    ;

allowable_expr_as_stmt
    : new_expr
		{ $$ = $1; }
    ;

expr_with_func_decl_lambda
	: expr
		{ $$ = $1; }
    | func_decl_lambda
        { $$ = $1; }
	| tkInherited
		{ $$ = new inherited_ident("", @$); }
    ;

expr
    : expr_l1
		{ $$ = $1; }
    | format_expr
		{ $$ = $1; }
    ;
    /*
expr_l1_for_indexer
    : expr_l1 
        { $$ = $1; }
    | format_expr
		{ $$ = $1; }
    ;*/

expr_l1
    : expr_dq
		{ $$ = $1; }
    | question_expr
		{ $$ = $1; }
    | new_question_expr
		{ $$ = $1; }
    ;
    
expr_l1_for_question_expr
    : expr_dq
		{ $$ = $1; }
    | question_expr
		{ $$ = $1; }
    ;
    
expr_l1_for_new_question_expr
    : expr_dq
		{ $$ = $1; }
    | new_question_expr
		{ $$ = $1; }
    ;

expr_l1_func_decl_lambda
	: expr_l1
		{ $$ = $1; }
    | func_decl_lambda
        { $$ = $1; }
    ;
    
expr_l1_for_lambda
    : expr_dq
		{ $$ = $1; }
    | question_expr
		{ $$ = $1; }
    | func_decl_lambda
        { $$ = $1; }
    ;
	
expr_dq
	: relop_expr
		{ $$ = $1; }
	| expr_dq tkDoubleQuestion relop_expr
		{ $$ = new double_question_node($1 as expression, $3 as expression, @$);}
	;
    
sizeof_expr
    : tkSizeOf tkRoundOpen simple_or_template_type_reference tkRoundClose
        { 
			$$ = new sizeof_operator((named_type_reference)$3, null, @$);  
		}
    ;

typeof_expr
    : tkTypeOf tkRoundOpen simple_or_template_type_reference tkRoundClose
        { 
			$$ = new typeof_operator((named_type_reference)$3, @$);  
		}
    |
      tkTypeOf tkRoundOpen empty_template_type_reference tkRoundClose
        { 
			$$ = new typeof_operator((named_type_reference)$3, @$);  
		}
    ;

question_expr
    : expr_l1_for_question_expr tkQuestion expr_l1_for_question_expr tkColon expr_l1_for_question_expr 
        { 
            if ($3 is nil_const && $5 is nil_const)
            	parsertools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",@3);
			$$ = new question_colon_expression($1, $3, $5, @$);  
		}
    ;
    
new_question_expr
	: tkIf expr_l1_for_new_question_expr tkThen expr_l1_for_new_question_expr tkElse expr_l1_for_new_question_expr 
        { 
        	if (parsertools.build_tree_for_formatter)
        	{
        		$$ = new if_expr_new($2, $4, $6, @$);
        	}
        	else
        	{
            	if ($4 is nil_const && $6 is nil_const)
            		parsertools.AddErrorFromResource("TWO_NILS_IN_QUESTION_EXPR",@4);
				$$ = new question_colon_expression($2, $4, $6, @$);
			}			
		}
	;
    

empty_template_type_reference
    : simple_type_identifier template_type_empty_params
        {
            $$ = new template_type_reference((named_type_reference)$1, (template_param_list)$2, @$); 
        }
    | simple_type_identifier tkAmpersend template_type_empty_params
        {
            $$ = new template_type_reference((named_type_reference)$1, (template_param_list)$3, @$);
        }
    ;
    
simple_or_template_type_reference
    : simple_type_identifier
		{ 
			$$ = $1;
		}
    | simple_type_identifier template_type_params
        { 
			$$ = new template_type_reference((named_type_reference)$1, (template_param_list)$2, @$); 
        }
    | simple_type_identifier tkAmpersend template_type_params
        { 
			$$ = new template_type_reference((named_type_reference)$1, (template_param_list)$3, @$); 
        }
    ;

optional_array_initializer
    : tkRoundOpen typed_const_list tkRoundClose
        { 
			$$ = new array_const((expression_list)$2, @$); 
		}
    |
    ;

new_expr
    : tkNew simple_or_template_type_reference optional_expr_list_with_bracket
        {
			$$ = new new_expr($2, $3 as expression_list, false, null, @$);
        }      
    | tkNew simple_or_template_type_reference tkSquareOpen optional_expr_list tkSquareClose optional_array_initializer
        {
        	var el = $4 as expression_list;
        	if (el == null)
        	{
        		var cnt = 0;
        		var ac = $6 as array_const;
        		if (ac != null && ac.elements != null)
	        	    cnt = ac.elements.Count;
	        	else parsertools.AddErrorFromResource("WITHOUT_INIT_AND_SIZE",@5);
        		el = new expression_list(new int32_const(cnt),@1);
        	}	
			$$ = new new_expr($2, el, true, $6 as array_const, @$);
        }      
    | tkNew tkClass tkRoundOpen list_fields_in_unnamed_object tkRoundClose
        {
        // sugared node	
        	var l = $4 as name_assign_expr_list;
        	var exprs = l.name_expr.Select(x=>x.expr).ToList();
        	var typename = "AnonymousType#"+Guid();
        	var type = new named_type_reference(typename,@1);
        	
			// node new_expr - for code generation of new node
			var ne = new new_expr(type, new expression_list(exprs), @$);
			// node unnamed_type_object - for formatting and code generation (new node and Anonymous class)
			$$ = new unnamed_type_object(l, true, ne, @$);
        }      
    ;
    
field_in_unnamed_object
	: identifier tkAssign relop_expr
		{
		    if ($3 is nil_const)
				parsertools.AddErrorFromResource("NIL_IN_UNNAMED_OBJECT",@$);		    
			$$ = new name_assign_expr($1,$3,@$);
		}
	| relop_expr
		{
			ident name = null;
			var id = $1 as ident;
			dot_node dot;
			if (id != null)
				name = id;
			else 
            {
            	dot = $1 as dot_node;
            	if (dot != null)
            	{
            		name = dot.right as ident;
            	}            	
            } 
			if (name == null)
				parsertools.errors.Add(new bad_anon_type(parsertools.CurrentFileName, @1, null));	
			$$ = new name_assign_expr(name,$1,@$);
		}
	;
	
list_fields_in_unnamed_object
	: field_in_unnamed_object
		{
			var l = new name_assign_expr_list();
			$$ = l.Add($1 as name_assign_expr);
		}
	| list_fields_in_unnamed_object tkComma field_in_unnamed_object
		{
			var nel = $1 as name_assign_expr_list;
			var ss = nel.name_expr.Select(ne=>ne.name.name).FirstOrDefault(x=>string.Compare(x,($3 as name_assign_expr).name.name,true)==0);
            if (ss != null)
            	parsertools.errors.Add(new anon_type_duplicate_name(parsertools.CurrentFileName, @3, null));
			nel.Add($3 as name_assign_expr);
			$$ = $1;
		}
	;

/*array_name_for_new_expr
    : simple_type_identifier 
		{ $$ = $1; }
//    | unsized_array_type - � ���� ����� ����������
//		{ $$ = $1; }
    ;*/

optional_expr_list_with_bracket
    :
		{ $$ = null; }
    |  tkRoundOpen optional_expr_list tkRoundClose    
        { $$ = $2; }
    ;

relop_expr
    : simple_expr
		{ $$ = $1; }
    | relop_expr relop simple_expr
        { 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
    | relop_expr relop new_question_expr
        { 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
    | is_type_expr tkRoundOpen pattern_out_param_list tkRoundClose
        {
            var isTypeCheck = $1 as typecast_node;
            var deconstructorPattern = new deconstructor_pattern($3 as List<pattern_parameter>, isTypeCheck.type_def, null, @$); 
            $$ = new is_pattern_expr(isTypeCheck.expr, deconstructorPattern, @$);
        }
	/*
	| term tkIs collection_pattern
        {
            $$ = new is_pattern_expr($1, $3 as pattern_node, @$);
        }
	| term tkIs tuple_pattern
        {
            $$ = new is_pattern_expr($1, $3 as pattern_node, @$);
        }
    */
    ;
    
pattern
    : simple_or_template_type_reference tkRoundOpen pattern_out_param_list tkRoundClose
        { 
            $$ = new deconstructor_pattern($3 as List<pattern_parameter>, $1, null, @$); 
        }
    ;

pattern_optional_var
    : simple_or_template_type_reference tkRoundOpen pattern_out_param_list_optional_var tkRoundClose
        { 
            $$ = new deconstructor_pattern($3 as List<pattern_parameter>, $1, null, @$); 
        }
    ;  

deconstruction_or_const_pattern
    : simple_or_template_type_reference tkRoundOpen pattern_out_param_list_optional_var tkRoundClose
        { 
            $$ = new deconstructor_pattern($3 as List<pattern_parameter>, $1, null, @$); 
        }
	| const_pattern_expr_list
		{
		    $$ = new const_pattern($1 as List<syntax_tree_node>, @$); 
		}
    ;  

const_pattern_expr_list
	: const_pattern_expression
		{ 
			$$ = new List<syntax_tree_node>(); 
			($$ as List<syntax_tree_node>).Add($1);
		}
	| const_pattern_expr_list tkComma const_pattern_expression 
		{ 
			var list = $1 as List<syntax_tree_node>;
            list.Add($3);
            $$ = list;
		}
	;

const_pattern_expression
	: literal_or_number  
		{ $$ = $1; }
	| simple_or_template_type_reference 
		{ $$ = $1; }
	| tkNil 
		{ 
			$$ = new nil_const();  
			$$.source_context = @$;
		}
	| sizeof_expr
		{ $$ = $1; }
    | typeof_expr
		{ $$ = $1; }
	;

collection_pattern
	: tkSquareOpen collection_pattern_expr_list tkSquareClose
		{
			$$ = new collection_pattern($2 as List<pattern_parameter>, @$);
		}
	;

collection_pattern_expr_list
	: collection_pattern_list_item
		{
			$$ = new List<pattern_parameter>();
            ($$ as List<pattern_parameter>).Add($1 as pattern_parameter);
		}
	| collection_pattern_expr_list tkComma collection_pattern_list_item
		{
			var list = $1 as List<pattern_parameter>;
            list.Add($3 as pattern_parameter);
            $$ = list;
		}
	;

collection_pattern_list_item
	: literal_or_number
		{
			$$ = new const_pattern_parameter($1, @$);
		}
	| collection_pattern_var_item
		{
			$$ = $1;
		}
	| tkUnderscore
		{
			$$ = new collection_pattern_wild_card(@$);
		}
	/*| pattern 
        {
            $$ = new recursive_deconstructor_parameter($1 as pattern_node, @$);
        }*/
	| pattern_optional_var
        {
            $$ = new recursive_deconstructor_parameter($1 as pattern_node, @$);
        }
	| collection_pattern
		{
			$$ = new recursive_collection_parameter($1 as pattern_node, @$);
		}
	| tuple_pattern
		{
			$$ = new recursive_tuple_parameter($1 as pattern_node, @$);
		}
	| tkDotDot
		{
			$$ = new collection_pattern_gap_parameter(@$);
		}
	;

collection_pattern_var_item
    : tkVar identifier
        {
            $$ = new collection_pattern_var_parameter($2, null, @$);
        }
    ;

tuple_pattern
	: tkRoundOpen tuple_pattern_item_list tkRoundClose
		{
			if (($2 as List<pattern_parameter>).Count>6) 
				parsertools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",@$);
			$$ = new tuple_pattern($2 as List<pattern_parameter>, @$);
		}	
    ; 

tuple_pattern_item
	: tkUnderscore 
		{ 
			$$ = new tuple_pattern_wild_card(@$); 
		} 
	| literal_or_number 
		{ 
			$$ = new const_pattern_parameter($1, @$);
		}
	| sign literal_or_number
		{
			$$ = new const_pattern_parameter(new un_expr($2, $1.type, @$), @$);
		}
    | tkVar identifier
        {
            $$ = new tuple_pattern_var_parameter($2, null, @$);
        }
	| pattern_optional_var
        {
            $$ = new recursive_deconstructor_parameter($1 as pattern_node, @$);
        }
	| collection_pattern
		{
			$$ = new recursive_collection_parameter($1 as pattern_node, @$);
		}
	| tuple_pattern
		{
			$$ = new recursive_tuple_parameter($1 as pattern_node, @$);
		}
	;

tuple_pattern_item_list
	: tuple_pattern_item
		{ 
			$$ = new List<pattern_parameter>();
            ($$ as List<pattern_parameter>).Add($1 as pattern_parameter);
		}
	| tuple_pattern_item_list tkComma tuple_pattern_item
		{
			var list = $1 as List<pattern_parameter>;
            list.Add($3 as pattern_parameter);
            $$ = list;
		}
	;

pattern_out_param_list_optional_var
    : pattern_out_param_optional_var
        {
            $$ = new List<pattern_parameter>();
            ($$ as List<pattern_parameter>).Add($1 as pattern_parameter);
        }
    | pattern_out_param_list_optional_var tkSemiColon pattern_out_param_optional_var
        {
            var list = $1 as List<pattern_parameter>;
            list.Add($3 as pattern_parameter);
            $$ = list;
        }
    | pattern_out_param_list_optional_var tkComma pattern_out_param_optional_var
        {
            var list = $1 as List<pattern_parameter>;
            list.Add($3 as pattern_parameter);
            $$ = list;
        }
    ;

pattern_out_param_list
    : pattern_out_param
        {
            $$ = new List<pattern_parameter>();
            ($$ as List<pattern_parameter>).Add($1 as pattern_parameter);
        }
    | pattern_out_param_list tkSemiColon pattern_out_param
        {
            var list = $1 as List<pattern_parameter>;
            list.Add($3 as pattern_parameter);
            $$ = list;
        }
    | pattern_out_param_list tkComma pattern_out_param
        {
            var list = $1 as List<pattern_parameter>;
            list.Add($3 as pattern_parameter);
            $$ = list;
        }
    ;    
    
pattern_out_param
    : tkUnderscore
		{
			$$ = new wild_card_deconstructor_parameter(@$);
		}
	| literal_or_number
		{
			$$ = new const_pattern_parameter($1, @$);
		}
	| tkVar identifier tkColon type_ref
        {
            $$ = new var_deconstructor_parameter($2, $4, true, @$);
        }
    | tkVar identifier
        {
            $$ = new var_deconstructor_parameter($2, null, true, @$);
        }
    | pattern 
        {
            $$ = new recursive_deconstructor_parameter($1 as pattern_node, @$);
        }
	| collection_pattern
		{
			$$ = new recursive_collection_parameter($1 as pattern_node, @$);
		}
	| tuple_pattern
		{
			$$ = new recursive_tuple_parameter($1 as pattern_node, @$);
		}
    ;    
    
pattern_out_param_optional_var
	: tkUnderscore
		{
			$$ = new wild_card_deconstructor_parameter(@$);
		}
	| literal_or_number
		{
			$$ = new const_pattern_parameter($1, @$);
		}
	| sign literal_or_number
		{
			$$ = new const_pattern_parameter(new un_expr($2, $1.type, @$), @$);
		}
    | identifier tkColon type_ref
        {
            $$ = new var_deconstructor_parameter($1, $3, false, @$);
        }
    | identifier
        {
            $$ = new var_deconstructor_parameter($1, null, false, @$);
        }
    | tkVar identifier tkColon type_ref
        {
            $$ = new var_deconstructor_parameter($2, $4, true, @$);
        }
    | tkVar identifier
        {
            $$ = new var_deconstructor_parameter($2, null, true, @$);
        }
    | pattern_optional_var
        {
            $$ = new recursive_deconstructor_parameter($1 as pattern_node, @$);
        }
	| collection_pattern
		{
			$$ = new recursive_collection_parameter($1 as pattern_node, @$);
		}
	| tuple_pattern
		{
			$$ = new recursive_tuple_parameter($1 as pattern_node, @$);
		}
    ;
    
simple_expr_or_nothing
	: simple_expr 
	{
		$$ = $1;
	}
	|
	{
		$$ = null;
	}
	;

const_expr_or_nothing
	: const_expr 
	{
		$$ = $1;
	}
	|
	{
		$$ = null;
	}
	;
/*
simple_expr_with_deref_or_nothing
    : tkDeref simple_expr
    {
        $$ = new simple_expr_with_deref($2, true);
    }
	| simple_expr 
	{
		$$ = new simple_expr_with_deref($1, false);
	}
	|
	{
		$$ = null;
	}
	;

simple_expr_with_deref
    : simple_expr 
    { 
        $$ = new simple_expr_with_deref($1, false); 
    }
    | tkDeref simple_expr
    {
        $$ = new simple_expr_with_deref($2, true);
    }
    ;
*/
format_expr 
    : simple_expr tkColon simple_expr_or_nothing                        
        {
			$$ = new format_expr($1, $3, null, @$); 
		}
    | tkColon simple_expr_or_nothing                        
        { 
			$$ = new format_expr(null, $2, null, @$); 
		}
    | simple_expr tkColon simple_expr_or_nothing tkColon simple_expr  
        { 
			$$ = new format_expr($1, $3, $5, @$); 
		}
    | tkColon simple_expr_or_nothing tkColon simple_expr
        { 
			$$ = new format_expr(null, $2, $4, @$); 
		}
    ;

format_const_expr 
    : const_expr tkColon const_expr_or_nothing                        
        { 
			$$ = new format_expr($1, $3, null, @$); 
		}
    | tkColon const_expr_or_nothing                        
        { 
			$$ = new format_expr(null, $2, null, @$); 
		}
    | const_expr tkColon const_expr_or_nothing tkColon const_expr   
        { 
			$$ = new format_expr($1, $3, $5, @$); 
		}
    | tkColon const_expr_or_nothing tkColon const_expr   
        { 
			$$ = new format_expr(null, $2, $4, @$); 
		}
    ;


relop
    : tkEqual
		{ $$ = $1; }
    | tkNotEqual
		{ $$ = $1; }
    | tkLower
		{ $$ = $1; }
    | tkGreater
		{ $$ = $1; }
    | tkLowerEqual
		{ $$ = $1; }
    | tkGreaterEqual
		{ $$ = $1; }
    | tkIn
		{ $$ = $1; }
    ;

simple_expr                                                    
    : term1
		{ $$ = $1; }
    | simple_expr tkDotDot term1 
	{ 
		if (parsertools.build_tree_for_formatter)
			$$ = new diapason_expr($1,$3,@$);
		else 
			$$ = new diapason_expr_new($1,$3,@$); 
	}
    ;

term1                                                    
    : term
		{ $$ = $1; }
    | term1 addop term                        
        { 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
    | term1 addop new_question_expr                        
        { 
			$$ = new bin_expr($1, $3, $2.type, @$); 
		}
    ;

addop
    : tkPlus
		{ $$ = $1; }
    | tkMinus
		{ $$ = $1; }
    | tkOr
		{ $$ = $1; }
    | tkXor
		{ $$ = $1; }
    | tkCSharpStyleOr
		{ $$ = $1; }
    ;

typecast_op
    : tkAs                      
        { 
			$$ = op_typecast.as_op; 
		}
    | tkIs                                        
        { 
			$$ = op_typecast.is_op; 
		}
    ;

as_is_expr
    : is_type_expr   
        { $$ = $1; }
    | as_expr
        { $$ = $1; }
	;

as_expr
    : term tkAs simple_or_template_type_reference
        {
            $$ = NewAsIsExpr($1, op_typecast.as_op, $3, @$);
        }
    | term tkAs array_type
	    {
            $$ = NewAsIsExpr($1, op_typecast.as_op, $3, @$);
	    }
    ;
    
is_type_expr
    : term tkIs simple_or_template_type_reference
        {
            $$ = NewAsIsExpr($1, op_typecast.is_op, $3, @$);
        }
    | term tkIs array_type
	    {
            $$ = NewAsIsExpr($1, op_typecast.as_op, $3, @$);
	    }
    ;
    
power_expr
    : factor_without_unary_op tkStarStar factor
        { $$ = new bin_expr($1,$3,($2).type, @$); }
    | factor_without_unary_op tkStarStar power_expr
        { $$ = new bin_expr($1,$3,($2).type, @$); }
    | sign power_expr
    	{ $$ = new un_expr($2, $1.type, @$); }
    ;
    
term
    : factor
		{ $$ = $1; }
    | new_expr
		{ $$ = $1; }
    | power_expr
        { $$ = $1; }
    | term mulop factor                             
        { $$ = new bin_expr($1,$3,($2).type, @$); }
    | term mulop power_expr                             
        { $$ = new bin_expr($1,$3,($2).type, @$); }
    | term mulop new_question_expr                             
        { $$ = new bin_expr($1,$3,($2).type, @$); }
    | as_is_expr
		{ $$ = $1; }
    ;

mulop
    : tkStar
		{ $$ = $1; }
    | tkSlash
		{ $$ = $1; }
    | tkDiv
		{ $$ = $1; }
    | tkMod
		{ $$ = $1; }
    | tkShl
		{ $$ = $1; }
    | tkShr
		{ $$ = $1; }
    | tkAnd
		{ $$ = $1; }
    ;

default_expr
    :  tkDefault tkRoundOpen simple_or_template_type_reference tkRoundClose
        { 
			$$ = new default_operator($3 as named_type_reference, @$);  
		}
    ;

tuple
	 : tkRoundOpen expr_l1_or_unpacked tkComma expr_l1_or_unpacked_list lambda_type_ref optional_full_lambda_fp_list tkRoundClose // lambda_type_ref optional_full_lambda_fp_list ����� �������� ����� �� ���� ���������� � ����������� ����� 
		{
			if ($2 is unpacked_list_of_ident_or_list) 
				parsertools.AddErrorFromResource("EXPRESSION_EXPECTED",@2);
			foreach (var ex in ($4 as expression_list).expressions)
				if (ex is unpacked_list_of_ident_or_list)
					parsertools.AddErrorFromResource("EXPRESSION_EXPECTED",ex.source_context);
			/*if ($5 != null) 
				parsertools.AddErrorFromResource("BAD_TUPLE",@5);
			if ($6 != null) 
				parsertools.AddErrorFromResource("BAD_TUPLE",@6);*/

			if (($4 as expression_list).Count>6) 
				parsertools.AddErrorFromResource("TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7",@$);
            ($4 as expression_list).Insert(0,$2);
			$$ = new tuple_node($4 as expression_list,@$);
		}	
    ; 

factor_without_unary_op
	: literal_or_number
		{ $$ = $1; }
	| var_reference
		{ $$ = $1; }
	;

factor        
    : tkNil                     
        { 
			$$ = new nil_const();  
			$$.source_context = @$;
		}
    | literal_or_number
		{ $$ = $1; }
    | default_expr
		{ $$ = $1; }
    | tkSquareOpen elem_list tkSquareClose     
        { 
			$$ = new pascal_set_constant($2 as expression_list, @$);  
		}
    | tkNot factor              
        { 
			$$ = new un_expr($2, $1.type, @$); 
		}
    | sign factor             
        {
			if ($1.type == Operators.Minus)
			{
			    var i64 = $2 as int64_const;
				if (i64 != null && i64.val == (Int64)Int32.MaxValue + 1)
				{
					$$ = new int32_const(Int32.MinValue,@$);
					break;
				}
				var ui64 = $2 as uint64_const;
				if (ui64 != null && ui64.val == (UInt64)Int64.MaxValue + 1)
				{
					$$ = new int64_const(Int64.MinValue,@$);
					break;
				}
				if (ui64 != null && ui64.val > (UInt64)Int64.MaxValue + 1)
				{
					parsertools.AddErrorFromResource("BAD_INT2",@$);
					break;
				}
			    // ����� ������� ���������� ��������� � �������������� �������
			}
			$$ = new un_expr($2, $1.type, @$);
		}
    | tkDeref factor
        {
            $$ = new index($2, true, @$);
        }
    | var_reference
		{ $$ = $1; }
	| tuple 
		{ $$ = $1; }
	;
      
literal_or_number
    : literal
		{ $$ = $1; }
    | unsigned_number
		{ $$ = $1; }
/*	| sign unsigned_number           
        { 
			$$ = new un_expr($2, $1.type, @$); 
		}*/
	;


var_question_point
	: variable tkQuestionPoint variable
	{
		$$ = new dot_question_node($1 as addressed_value,$3 as addressed_value,@$);
	}
	| variable tkQuestionPoint var_question_point 
	{
		$$ = new dot_question_node($1 as addressed_value,$3 as addressed_value,@$);
	}
	;

var_reference
    : var_address variable                   
        {
			$$ = NewVarReference($1 as get_address, $2 as addressed_value, @$);
		}
    | variable 
		{ $$ = $1; }
    | var_question_point 
		{ $$ = $1; }
    ;
 
var_address
    : tkAddressOf                                
        { 
			$$ = NewVarAddress(@$);
		}
    | var_address tkAddressOf                  
        { 
			$$ = NewVarAddress($1 as get_address, @$);
		}
    ;

attribute_variable
    : simple_type_identifier optional_expr_list_with_bracket
        { 
			$$ = new attribute(null, $1 as named_type_reference, $2 as expression_list, @$);
		}
    | template_type optional_expr_list_with_bracket
        {
            $$ = new attribute(null, $1 as named_type_reference, $2 as expression_list, @$);
        }
    ;

dotted_identifier
	: identifier { $$ = $1; }
	| dotted_identifier tkPoint identifier_or_keyword
		{
			$$ = new dot_node($1 as addressed_value, $3 as addressed_value, @$);
		}
	;
	
variable_as_type
	: dotted_identifier { $$ = $1;}
	| dotted_identifier template_type_params 
		{ $$ = new ident_with_templateparams($1 as addressed_value, $2 as template_param_list, @$);   }
	;
	
variable_or_literal_or_number
	: variable 
		{ $$ = $1; }
	| literal_or_number
		{ $$ = $1; }
	;
	
variable
    : identifier 
		{ $$ = $1; }
    | operator_name_ident
		{ $$ = $1; }
    | tkInherited identifier            
        { 
			$$ = new inherited_ident($2.name, @$);
		}
	
    | tkRoundOpen expr tkRoundClose         
        {
		    if (!parsertools.build_tree_for_formatter) 
            {
                $2.source_context = @$;
                $$ = $2;
            } 
			else $$ = new bracket_expr($2, @$);
        }
    | sizeof_expr
		{ $$ = $1; }
    | typeof_expr
		{ $$ = $1; }
    | literal_or_number tkPoint identifier_or_keyword
        { 
			$$ = new dot_node($1 as addressed_value, $3 as addressed_value, @$); 
		}
    | variable_or_literal_or_number tkSquareOpen expr_list tkSquareClose                
        {
        	var el = $3 as expression_list; // SSM 10/03/16
        	if (el.Count==1 && el.expressions[0] is format_expr) 
        	{
        		var fe = el.expressions[0] as format_expr;
                if (!parsertools.build_tree_for_formatter)
                {
                    if (fe.expr == null)
                        fe.expr = new int32_const(int.MaxValue,@3);
                    if (fe.format1 == null)
                        fe.format1 = new int32_const(int.MaxValue,@3);
                }
        		$$ = new slice_expr($1 as addressed_value,fe.expr,fe.format1,fe.format2,@$);
			}   
			// ����������� �����
            else if (el.expressions.Any(e => e is format_expr))
            {
            	if (el.expressions.Count > 4)
            		parsertools.AddErrorFromResource("SLICES_OF MULTIDIMENSIONAL_ARRAYS_ALLOW_ONLY_FOR_RANK_LT_5",@$); // ����� ����������� �������� ��������� ������ ��� �������� ����������� < 5  
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
				var sle = new slice_expr($1 as addressed_value,null,null,null,@$);
				sle.slices = ll;
				$$ = sle;
            }
			else $$ = new indexer($1 as addressed_value, el, @$);
        }
    | variable_or_literal_or_number tkQuestionSquareOpen format_expr tkSquareClose                
        {
        	var fe = $3 as format_expr; // SSM 9/01/17
            if (!parsertools.build_tree_for_formatter)
            {
                if (fe.expr == null)
                    fe.expr = new int32_const(int.MaxValue,@3);
                if (fe.format1 == null)
                    fe.format1 = new int32_const(int.MaxValue,@3);
            }
      		$$ = new slice_expr_question($1 as addressed_value,fe.expr,fe.format1,fe.format2,@$);
        }
    | tkVertParen elem_list tkVertParen     
        { 
			$$ = new array_const_new($2 as expression_list, @$);  
		}
    | variable tkRoundOpen optional_expr_list tkRoundClose                
        {
			$$ = new method_call($1 as addressed_value,$3 as expression_list, @$);
        }
    | variable tkPoint identifier_keyword_operatorname
        {
			$$ = new dot_node($1 as addressed_value, $3 as addressed_value, @$);
        }
    | tuple tkPoint identifier_keyword_operatorname
        {
			$$ = new dot_node($1 as addressed_value, $3 as addressed_value, @$);
        }
    /*| variable tkQuestionPoint identifier_keyword_operatorname                
        {
			$$ = new dot_question_node($1 as addressed_value, $3 as addressed_value, @$);
        }*/
    | variable tkDeref              
        {
			$$ = new roof_dereference($1 as addressed_value,@$);
        }
    | variable tkAmpersend template_type_params                
        {
			$$ = new ident_with_templateparams($1 as addressed_value, $3 as template_param_list, @$);
        }
    ;
    
optional_expr_list
    : expr_list
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;

elem_list
    : elem_list1
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;

elem_list1
    : elem                                
        { 
			$$ = new expression_list($1, @$); 
		}
    | elem_list1 tkComma elem     
        { 
			$$ = ($1 as expression_list).Add($3, @$);
		}
    ;

elem
    : expr
		{ $$ = $1; }
    | expr tkDotDot expr          
        { $$ = new diapason_expr($1, $3, @$); }
    ;

one_literal
    : tkStringLiteral
		{ $$ = $1 as literal; }
    | tkAsciiChar
		{ $$ = $1 as literal; }
    ;

literal
    :literal_list                      
        { 
			$$ = NewLiteral($1 as literal_const_line);
        }
    | tkFormatStringLiteral
        {
            if (parsertools.build_tree_for_formatter)
   			{
                $$ = $1 as string_const;
            }
            else
            {
                $$ = NewFormatString($1 as string_const);
            }
        }
	;
	
literal_list
    : one_literal                 
        { 
			$$ = new literal_const_line($1 as literal, @$);
        }
    | literal_list one_literal    
        { 
        	var line = $1 as literal_const_line;
            if (line.literals.Last() is string_const && $2 is string_const)
            	parsertools.AddErrorFromResource("TWO_STRING_LITERALS_IN_SUCCESSION",@2);
			$$ = line.Add($2 as literal, @$);
        } 
    ;

operator_name_ident
    : tkOperator overload_operator           
        { 
			$$ = new operator_name_ident(($2 as op_type_node).text, ($2 as op_type_node).type, @$);
		}
	;
	
optional_method_modificators
    : tkSemiColon                                        
        { 
			$$ = new procedure_attributes_list(new List<procedure_attribute>(),@$); 
		}
    | tkSemiColon meth_modificators tkSemiColon     
        { 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			$$ = $2; 
		}
    ;
    
optional_method_modificators1
    : 
        { 
			$$ = new procedure_attributes_list(new List<procedure_attribute>(),@$); 
		}
    | tkSemiColon meth_modificators 
        { 
			//parsertools.AddModifier((procedure_attributes_list)$2, proc_attribute.attr_overload); 
			$$ = $2; 
		}
    ;
    
meth_modificators
    : meth_modificator                
        { 
			$$ = new procedure_attributes_list($1 as procedure_attribute, @$); 
		}
    | meth_modificators tkSemiColon meth_modificator  
        { 
			$$ = ($1 as procedure_attributes_list).Add($3 as procedure_attribute, @$);  
		}
    ;

identifier
    : tkIdentifier 
		{ $$ = $1; }
    | property_specifier_directives
		{ $$ = $1; }
    | non_reserved
		{ $$ = $1; }
    ;

identifier_or_keyword
    : identifier
		{ $$ = $1; }
    | keyword                    
        { $$ = new ident($1.text, @$); }
    | reserved_keyword                   
        { $$ = new ident($1.text, @$); }
    ;

identifier_keyword_operatorname
    : identifier
		{ $$ = $1; }
    | keyword                    
        { $$ = new ident($1.text, @$); }
    | operator_name_ident
		{ $$ = (ident)$1; }
    ;

meth_modificator
    : tkAbstract
		{ $$ = $1; }
    | tkOverload
		{ 
            $$ = $1;
            parsertools.AddWarningFromResource("OVERLOAD_IS_NOT_USED", $1.source_context);
        }
    | tkReintroduce
		{ $$ = $1; }
    | tkOverride
		{ $$ = $1; }
    | tkExtensionMethod
		{ $$ = $1; }
    | tkVirtual
		{ $$ = $1; }
    ;
    
property_modificator
	: tkVirtual
		{ $$ = $1; }
	| tkOverride
		{ $$ = $1; }
    | tkAbstract
		{ $$ = $1; }
    | tkReintroduce
		{ $$ = $1; }
	;
    
property_specifier_directives
    : tkRead
		{ $$ = $1; }
    | tkWrite
		{ $$ = $1; }
    ;

non_reserved
    : tkName
		{ $$ = $1; }
    | tkNew
		{ $$ = $1; }
    ;

visibility_specifier
    : tkInternal
		{ $$ = $1; }
    | tkPublic
		{ $$ = $1; }
    | tkProtected
		{ $$ = $1; }
    | tkPrivate
		{ $$ = $1; }
    ;

keyword
    : visibility_specifier 
        { 
			$$ = new token_info($1.name, @$);  
		}
    | tkSealed
		{ $$ = $1; }
    | tkTemplate
		{ $$ = $1; }
    | tkOr
		{ $$ = $1; }
    | tkTypeOf
		{ $$ = $1; }
    | tkSizeOf
		{ $$ = $1; }
    | tkDefault
		{ $$ = $1; }
    | tkWhere
		{ $$ = $1; }
    | tkXor
		{ $$ = $1; }
    | tkAnd
		{ $$ = $1; }
    | tkDiv
		{ $$ = $1; }
    | tkMod
		{ $$ = $1; }
    | tkShl
		{ $$ = $1; }
    | tkShr
		{ $$ = $1; }
    | tkNot
		{ $$ = $1; }
    | tkAs
		{ $$ = $1; }
    | tkIn
		{ $$ = $1; }
    | tkIs
		{ $$ = $1; }
    | tkArray
		{ $$ = $1; }
    | tkSequence
		{ $$ = $1; }
    | tkBegin
		{ $$ = $1; }
    | tkCase
		{ $$ = $1; }
    | tkClass
		{ $$ = $1; }
    | tkConst
		{ $$ = $1; }
    | tkConstructor
		{ $$ = $1; }
    | tkDestructor
		{ $$ = $1; }
    | tkDownto
		{ $$ = $1; }
    | tkDo
		{ $$ = $1; }
    | tkElse
		{ $$ = $1; }
    | tkEnd
		{ $$ = $1; }
    | tkExcept
		{ $$ = $1; }
    | tkFile
		{ $$ = $1; }
	| tkAuto		
		{ $$ = $1; }
    | tkFinalization
		{ $$ = $1; }
    | tkFinally
		{ $$ = $1; }
    | tkFor
		{ $$ = $1; }
    | tkForeach
		{ $$ = $1; }
    | tkFunction
		{ $$ = $1; }
    | tkIf
		{ $$ = $1; }
    | tkImplementation
		{ $$ = $1; }
    | tkInherited
		{ $$ = $1; }
    | tkInitialization
		{ $$ = $1; }
    | tkInterface
		{ $$ = $1; }
    | tkProcedure
		{ $$ = $1; }
    | tkProperty
		{ $$ = $1; }
    | tkRaise
		{ $$ = $1; }
    | tkRecord
		{ $$ = $1; }
    | tkRepeat
		{ $$ = $1; }
    | tkSet
		{ $$ = $1; }
    | tkTry
		{ $$ = $1; }
    | tkType
		{ $$ = $1; }
    | tkStatic
		{ $$ = $1; }
    | tkThen
		{ $$ = $1; }
    | tkTo
		{ $$ = $1; }
    | tkUntil
		{ $$ = $1; }
    | tkUses
		{ $$ = $1; }
    | tkVar
		{ $$ = $1; }
    | tkWhile
		{ $$ = $1; }
    | tkWith
		{ $$ = $1; }
    | tkNil
		{ $$ = $1; }
    | tkGoto
		{ $$ = $1; }
    | tkOf
		{ $$ = $1; }
    | tkLabel
		{ $$ = $1; }
    | tkProgram
		{ $$ = $1; }
    | tkUnit
		{ $$ = $1; }
    | tkLibrary
		{ $$ = $1; }
    | tkNamespace
		{ $$ = $1; }
    | tkExternal
		{ $$ = $1; }
    | tkParams
		{ $$ = $1; }
	| tkEvent	
		{ $$ = $1; }
	| tkYield	
		{ $$ = $1; }
    | tkMatch
        { $$ = $1; }
    | tkWhen
        { $$ = $1; }
    | tkPartial
        { $$ = $1; }
    | tkAbstract
        { $$ = new token_info($1.name, @$);  }
    | tkLock
        { $$ = $1; }
    | tkImplicit
        { $$ = $1; }
    | tkExplicit
        { $$ = $1; }
    | tkOn
        { $$ = new token_info($1.name, @$);  }
    | tkVirtual
        { $$ = new token_info($1.name, @$);  }
    | tkOverride
        { $$ = new token_info($1.name, @$);  }
    | tkLoop
        { $$ = $1; }
    | tkExtensionMethod
        { $$ = new token_info($1.name, @$);  }
    | tkOverload
        { $$ = new token_info($1.name, @$);  }
    | tkReintroduce
        { $$ = new token_info($1.name, @$);  }
    | tkForward
        { $$ = new token_info($1.name, @$);  }
    ;

reserved_keyword
    : tkOperator
		{ $$ = $1; }
    ;

overload_operator
    : tkMinus
		{ $$ = $1; }
    | tkPlus
		{ $$ = $1; }
    | tkSlash
		{ $$ = $1; }
    | tkStar
		{ $$ = $1; }
    | tkEqual
		{ $$ = $1; }
    | tkGreater
		{ $$ = $1; }
    | tkGreaterEqual
		{ $$ = $1; }
    | tkLower
		{ $$ = $1; }
    | tkLowerEqual
		{ $$ = $1; }
    | tkNotEqual
		{ $$ = $1; }
    | tkOr
		{ $$ = $1; }
    | tkXor
		{ $$ = $1; }
    | tkAnd
		{ $$ = $1; }
    | tkDiv
		{ $$ = $1; }
    | tkMod
		{ $$ = $1; }
    | tkShl
		{ $$ = $1; }
    | tkShr
		{ $$ = $1; }
    | tkNot
		{ $$ = $1; }
    | tkIn
		{ $$ = $1; }
    | tkImplicit
		{ $$ = $1; }
    | tkExplicit
		{ $$ = $1; }
    | assign_operator
		{ $$ = $1; }
    | tkStarStar
		{ $$ = $1; }
    ;

assign_operator
    : tkAssign
		{ $$ = $1; }
    | tkPlusEqual
		{ $$ = $1; }
    | tkMinusEqual
		{ $$ = $1; }
    | tkMultEqual
		{ $$ = $1; }
    | tkDivEqual
		{ $$ = $1; }
    ;

lambda_unpacked_params
	: tkBackSlashRoundOpen lambda_list_of_unpacked_params_or_id tkComma lambda_unpacked_params_or_id tkRoundClose
		{
			// ��������� ���� ��������� ������ �� ��������� ���� � function_lambda_definition
			($2 as unpacked_list_of_ident_or_list).Add($4 as ident_or_list);
			$$ = $2 as unpacked_list_of_ident_or_list;
		}
	;
	
lambda_unpacked_params_or_id
	: lambda_unpacked_params // ident_or_list
		{
			$$ = new ident_or_list($1 as unpacked_list_of_ident_or_list);
		}
	| identifier // ident_or_list
		{
			$$ = new ident_or_list($1 as ident);
		}
	;
	
lambda_list_of_unpacked_params_or_id
	: lambda_unpacked_params_or_id
		{
			$$ = new unpacked_list_of_ident_or_list();
			($$ as unpacked_list_of_ident_or_list).Add($1 as ident_or_list);
			($$ as unpacked_list_of_ident_or_list).source_context = @1;
		}
	| lambda_list_of_unpacked_params_or_id tkComma lambda_unpacked_params_or_id
		{
			$$ = $1;
			($$ as unpacked_list_of_ident_or_list).Add($3 as ident_or_list);
			($$ as unpacked_list_of_ident_or_list).source_context = LexLocation.MergeAll(@1,@3);
		}
	;

	
expr_l1_or_unpacked
	: expr_l1 
		{ $$ = $1; }
	| lambda_unpacked_params 
		{ $$ = $1; }
	;
	

expr_l1_or_unpacked_list
	: expr_l1_or_unpacked
        { 
			$$ = new expression_list($1, @$); 
		}
	| expr_l1_or_unpacked_list tkComma expr_l1_or_unpacked
		{
			$$ = ($1 as expression_list).Add($3, @$); 
		}
	;	
	
func_decl_lambda
	: identifier tkArrow lambda_function_body
		{
			var idList = new ident_list($1, @1); 
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), @1), parametr_kind.none, null, @1), @1);
			//var sl = $3 as statement_list;
			//if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName($3, "Result") != null) // ���� ��� ���� ��������� ��� ���� ���������� Result, �� ��������� ���� 
			    $$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), @1), $3 as statement_list, @$);
			//else 
			//$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, $3 as statement_list, @$);  
		}
    | tkRoundOpen tkRoundClose lambda_type_ref_noproctype tkArrow lambda_function_body
		{
		    // ����� ���� ������������� �� ���� � ���� ��������� lambda_inferred_type, ���� ������ ��� null!
		    var sl = $5 as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
				$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, $3, sl, @$);
			else $$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, sl, @$);	
		}
	| tkRoundOpen identifier tkColon fptype tkRoundClose lambda_type_ref_noproctype tkArrow lambda_function_body
		{
			var idList = new ident_list($2, @2); 
            var loc = LexLocation.MergeAll(@2,@3,@4);
			var formalPars = new formal_parameters(new typed_parameters(idList, $4, parametr_kind.none, null, loc), loc);
		    var sl = $8 as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
				$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, $6, sl, @$);
			else $$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, @$);	
		}
	| tkRoundOpen identifier tkSemiColon full_lambda_fp_list tkRoundClose lambda_type_ref_noproctype tkArrow lambda_function_body
		{
			var idList = new ident_list($2, @2);
			var formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null), parametr_kind.none, null, @2), LexLocation.MergeAll(@2,@3,@4));
			for (int i = 0; i < ($4 as formal_parameters).Count; i++)
				formalPars.Add(($4 as formal_parameters).params_list[i]);
		    var sl = $8 as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
				$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, $6, sl, @$);
			else $$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, @$);	
		}
	| tkRoundOpen identifier tkColon fptype tkSemiColon full_lambda_fp_list tkRoundClose lambda_type_ref_noproctype tkArrow lambda_function_body
		{
			var idList = new ident_list($2, @2);
            var loc = LexLocation.MergeAll(@2,@3,@4);
			var formalPars = new formal_parameters(new typed_parameters(idList, $4, parametr_kind.none, null, loc), LexLocation.MergeAll(@2,@3,@4,@5,@6));
			for (int i = 0; i < ($6 as formal_parameters).Count; i++)
				formalPars.Add(($6 as formal_parameters).params_list[i]);
		    var sl = $10 as statement_list;
		    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
				$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, $8, sl, @$);
			else $$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, sl, @$);
		}
    | tkRoundOpen expr_l1_or_unpacked tkComma expr_l1_or_unpacked_list lambda_type_ref optional_full_lambda_fp_list tkRoundClose rem_lambda // optional_full_lambda_fp_list - ��� ������� ��-�� ���������� � ���������
		{ 
			var pair = $8 as pair_type_stlist;
			
			if ($5 is lambda_inferred_type)
			{
				// ������� ���� \(x,y)
				// �������� �� ���� expr_list1. ���� ���� �� ���� - ���� ident_or_list �� ����� �� ���� ����� � �����
				// ���������, ��� $6 = null
				// ������������ List<expression> ��� unpacked_params � ���������
				var has_unpacked = false;
				if ($2 is unpacked_list_of_ident_or_list)
					has_unpacked = true;
				if (!has_unpacked)
					foreach (var x in ($4 as expression_list).expressions)
					{
						if (x is unpacked_list_of_ident_or_list)
						{
							has_unpacked = true;
							break;
						}
					}
				if (has_unpacked) // ��� ����� �����
				{
					if ($6 != null)
					{
						parsertools.AddErrorFromResource("SEMICOLON_IN_PARAMS",@6);
					}
				
					var lst_ex = new List<expression>();
					lst_ex.Add($2 as expression);
					foreach (var x in ($4 as expression_list).expressions)
						lst_ex.Add(x);
					
					function_lambda_definition fld = null; //= new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, 
    					//new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), @2), pair.exprs, @$);

					var sl1 = pair.exprs;
			    	if (sl1.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl1, "result") != null) // �� ���� ��������
						fld = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, pair.tn, pair.exprs, @$);
					else fld = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, pair.exprs, @$);	

					fld.unpacked_params = lst_ex;
					$$ = fld;					
					return;
				}
				
				var formal_pars = new formal_parameters();
				var idd = $2 as ident;
				if (idd==null)
					parsertools.AddErrorFromResource("ONE_TKIDENTIFIER",@2);
				var lambda_inf_type = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
				var new_typed_pars = new typed_parameters(new ident_list(idd, idd.source_context), lambda_inf_type, parametr_kind.none, null, idd.source_context);
				formal_pars.Add(new_typed_pars);
				foreach (var id in ($4 as expression_list).expressions)
				{
					var idd1 = id as ident;
					if (idd1==null)
						parsertools.AddErrorFromResource("ONE_TKIDENTIFIER",id.source_context);
					
					lambda_inf_type = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
					new_typed_pars = new typed_parameters(new ident_list(idd1, idd1.source_context), lambda_inf_type, parametr_kind.none, null, idd1.source_context);
					formal_pars.Add(new_typed_pars);
				}
				
				if ($6 != null)
					for (int i = 0; i < ($6 as formal_parameters).Count; i++)
						formal_pars.Add(($6 as formal_parameters).params_list[i]);		
					
				formal_pars.source_context = LexLocation.MergeAll(@2,@3,@4,@5);
			    
			    var sl = pair.exprs;
			    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
					$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formal_pars, pair.tn, pair.exprs, @$);
				else $$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formal_pars, null, pair.exprs, @$);	
			}
			else
			{			
				var loc = LexLocation.MergeAll(@2,@3,@4);
				var idd = $2 as ident;
				if (idd==null)
					parsertools.AddErrorFromResource("ONE_TKIDENTIFIER",@2);
				
				var idList = new ident_list(idd, loc);
				
				var iddlist = ($4 as expression_list).expressions;
				
				for (int j = 0; j < iddlist.Count; j++)
				{
					var idd2 = iddlist[j] as ident;
					if (idd2==null)
						parsertools.AddErrorFromResource("ONE_TKIDENTIFIER",idd2.source_context);
					idList.Add(idd2);
				}	
				var parsType = $5;
				var formalPars = new formal_parameters(new typed_parameters(idList, parsType, parametr_kind.none, null, loc), LexLocation.MergeAll(@2,@3,@4,@5,@6));
				
				if ($6 != null)
					for (int i = 0; i < ($6 as formal_parameters).Count; i++)
						formalPars.Add(($6 as formal_parameters).params_list[i]);

				var sl = pair.exprs;
			    if (sl.expr_lambda_body || SyntaxVisitors.HasNameVisitor.HasName(sl, "result") != null) // �� ���� ��������
					$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, pair.tn, pair.exprs, @$);
				else $$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), formalPars, null, pair.exprs, @$);
			}
		}
    | lambda_unpacked_params rem_lambda // ������ � �����������
    	{
    		var pair = $2 as pair_type_stlist;
    		// ���� ���������� ��������� - null. �������� �� �������� ���������
    		$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, 
    			new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), @1), pair.exprs, @$);
    		// unpacked_params - ��� ��� ������ ���������. ��� ���������� - ���� ������ ���������. ��������, ������ �������
    		var lst_ex = new List<expression>();
    		lst_ex.Add($1 as unpacked_list_of_ident_or_list);
    		($$ as function_lambda_definition).unpacked_params = lst_ex;  
    	}
	| expl_func_decl_lambda
		{
			$$ = $1; 
		}
	;
	
optional_full_lambda_fp_list
	: { $$ = null; }
	| tkSemiColon full_lambda_fp_list 
	{
		$$ = $2; 
	}
	;
	
rem_lambda
	: lambda_type_ref_noproctype tkArrow lambda_function_body
		{ 
		    $$ = new pair_type_stlist($1,$3 as statement_list);
		}
	;
	
expl_func_decl_lambda
	: tkFunction lambda_type_ref_noproctype tkArrow lambda_function_body // SSM 11.08.20 ������� _noproctype � 3 ����������� 
		{
			$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, $2, $4 as statement_list, 1, @$);
		}
	| tkFunction tkRoundOpen tkRoundClose lambda_type_ref_noproctype tkArrow lambda_function_body
		{
			$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, $4, $6 as statement_list, 1, @$);
		}
	| tkFunction tkRoundOpen full_lambda_fp_list tkRoundClose lambda_type_ref_noproctype tkArrow lambda_function_body
		{
			$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), $3 as formal_parameters, $5, $7 as statement_list, 1, @$);
		}
	| tkProcedure tkArrow lambda_procedure_body
		{
			$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, $3 as statement_list, 2, @$);
		}
	| tkProcedure tkRoundOpen tkRoundClose tkArrow lambda_procedure_body
		{
			$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), null, null, $5 as statement_list, 2, @$);
		}
	| tkProcedure tkRoundOpen full_lambda_fp_list tkRoundClose tkArrow lambda_procedure_body
		{
			$$ = new function_lambda_definition(lambdaHelper.CreateLambdaName(), $3 as formal_parameters, null, $6 as statement_list, 2, @$);
		}
	;
	
full_lambda_fp_list
	: lambda_simple_fp_sect
		{
			var typed_pars = $1 as typed_parameters;
			if (typed_pars.vars_type is lambda_inferred_type)
			{
				$$ = new formal_parameters();
				foreach (var id in typed_pars.idents.idents)
				{
					var lambda_inf_type = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
					var new_typed_pars = new typed_parameters(new ident_list(id, id.source_context), lambda_inf_type, parametr_kind.none, null, id.source_context);
					($$ as formal_parameters).Add(new_typed_pars);
				}
				$$.source_context = @$;
			}
			else
			{
				$$ = new formal_parameters(typed_pars, @$);
			}
		}
	| full_lambda_fp_list tkSemiColon lambda_simple_fp_sect
		{
			$$ =($1 as formal_parameters).Add($3 as typed_parameters, @$);
		}
	;
	
lambda_simple_fp_sect
    : ident_list lambda_type_ref
		{
			$$ = new typed_parameters($1 as ident_list, $2, parametr_kind.none, null, @$);
		}
    ;

lambda_type_ref
	:
		{
			$$ = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
		}
	| tkColon fptype
		{
			$$ = $2;
		}
	;
		
lambda_type_ref_noproctype
	:
		{
			$$ = new lambda_inferred_type(new PascalABCCompiler.TreeRealization.lambda_any_type_node(), null);
		}
	| tkColon fptype_noproctype
		{
			$$ = $2;
		}
	;

common_lambda_body
	: compound_stmt
		{
			$$ = $1;
		}
    | if_stmt
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| while_stmt
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| repeat_stmt
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| for_stmt
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| foreach_stmt
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| loop_stmt
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| case_stmt
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| try_stmt
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| lock_stmt
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| raise_stmt
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| yield_stmt
		{
			parsertools.AddErrorFromResource("YIELD_STATEMENT_CANNOT_BE_USED_IN_LAMBDA_BODY", @$);
		}
	;


lambda_function_body
	: expr_l1_for_lambda 
		{
		    var id = SyntaxVisitors.HasNameVisitor.HasName($1, "Result"); 
            if (id != null)
            {
                 parsertools.AddErrorFromResource("RESULT_IDENT_NOT_EXPECTED_IN_THIS_CONTEXT", id.source_context);
            }
			var sl = new statement_list(new assign("result",$1,@$),@$); // ���� �������� ��� � assign ��� ������������������� ��� ������ - ����� ��������� ����� Result
			sl.expr_lambda_body = true;
			$$ = sl;
		}
	| common_lambda_body
		{
			$$ = $1;
		}
	;	

lambda_procedure_body
	: proc_call
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| assignment
		{
			$$ = new statement_list($1 as statement, @$);
		}
	| common_lambda_body
		{
			$$ = $1;
		}
	;

%%