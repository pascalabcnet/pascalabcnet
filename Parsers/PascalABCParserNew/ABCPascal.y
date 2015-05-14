%{
// Эти объявления добавляются в класс GPPGParser, представляющий собой парсер, генерируемый системой gppg
    public syntax_tree_node root; // Корневой узел синтаксического дерева 

    public List<Error> errors;
    public string current_file_name;
    public int max_errors = 10;
    public PT parsertools;
    public List<compiler_directive> CompilerDirectives;

    public GPPGParser(AbstractScanner<PascalABCNewParser.Union, LexLocation> scanner) : base(scanner) { }
%}

%output=ABCPascalYacc.cs 

%parsertype GPPGParser

%using PascalABCCompiler.SyntaxTree;
%using PascalABCNewParser;
%using PascalABCCompiler.ParserTools;
%using PascalABCCompiler.Errors;
%using PascalABCCompiler.PascalABCParser.Errors;

%namespace GPPGParserScanner

%YYSTYPE PascalABCNewParser.Union

%start parse_goal

%token <ti> tkDirectiveName tkAmpersend tkColon tkDotDot tkPoint tkRoundOpen tkRoundClose tkSemiColon tkSquareOpen tkSquareClose tkQuestion 
%token <ti> tkSizeOf tkTypeOf tkWhere tkArray tkCase tkClass tkConst tkConstructor tkDestructor tkElse  tkExcept tkFile tkFinalization tkFor tkForeach tkFunction 
%token <ti> tkIf tkImplementation tkInherited tkInterface tkProcedure tkOperator tkProperty tkRaise tkRecord tkSet tkType tkThen tkUses tkUsing tkVar tkWhile tkWith tkNil 
%token <ti> tkGoto tkOf tkLabel tkLock tkProgram tkEvent tkDefault tkTemplate tkPacked tkInline tkExports tkResourceString tkThreadvar tkFinal tkTo tkDownto
%token <id> tkShortInt tkSmallInt tkOrdInteger tkByte tkLongInt tkInt64 tkWord tkBoolean tkChar tkWideChar tkLongWord tkPChar tkCardinal tkVariant tkOleVariant tkParams 
%token <id> tkAt tkOn tkContains tkOut tkPackage tkRequires
%token <id> tkAbsolute tkAssembler tkAutomated tkDispid tkExternal tkImplements tkIndex tkMessage tkName tkNodefault tkPrivate tkProtected tkPublic tkInternal tkRead tkResident tkStored tkWrite tkReadOnly tkWriteOnly 
%token <ti> tkParseModeExpression tkParseModeStatement tkBegin tkEnd 
%token <ti> tkAsmBody tkILCode tkError tkSquareClos INVISIBLE
%token <ti> tkRepeat tkUntil tkDo tkComma tkFinally tkTry
%token <ti> tkInitialization tkFinalization  
%token <op> tkAssign tkPlusEqual tkMinusEqual tkMultEqual tkDivEqual tkMinus tkPlus tkSlash tkStar tkEqual tkGreater tkGreaterEqual tkLower tkLowerEqual 
%token <op> tkNotEqual tkCSharpStyleOr tkArrow tkOr tkXor tkAnd tkDiv tkMod tkShl tkShr tkNot tkAs tkIn tkIs tkImplicit tkExplicit tkAddressOf tkDeref
%token <id> tkDirectiveName tkIdentifier tkUnit tkLibrary tkReal tkSingle tkDouble tkExtended tkComp
%token <con> tkStringLiteral tkAsciiChar
%token <pat> tkStatic tkAbstract tkForward tkOverload tkReintroduce tkOverride tkVirtual 
%token <con> tkInteger tkFloat tkHex 

%type <a> assignment  
%type <ac> opt_array_initializer  
%type <al> opt_attribute_declarations  attribute_declarations  
%type <amn> ot_visibility_specifier  
%type <at> one_attribute attribute_variable 
%type <av> const_factor const_variable_2 const_term const_variable var_specifiers literal_or_number unsigned_number
%type <b> program_block  
%type <bo> opt_var  
%type <cb> not_component_list_seq opt_not_component_list_seq_end  
%type <cd> const_decl only_const_decl  
%type <cdl> const_decl_sect  
%type <cld> new_object_type not_object_type object_type new_record_type  
%type <cm> not_component_list not_component_list_2 not_component_list_1  
%type <cn> case_stmt    
%type <cvs> case_list  
%type <d> program_decl_sect_list int_decl_sect_list1 abc_decl_sect_list1 int_decl_sect_list impl_decl_sect_list impl_decl_sect_list1 abc_decl_sect_list 
%type <d1> filed_or_const_definition abc_decl_sect impl_decl_sect int_decl_sect type_decl simple_type_decl simple_filed_or_const_definition res_str_decl_sect 
%type <d1> not_method_definition not_property_definition fp_sect 
%type <dop> default_expr  
%type <eas> expr_as_stmt  
%type <eb> exception_block  
%type <ed> abc_external_directr external_directr  
%type <eh> exception_handler  
%type <ehl> exception_handler_list  
%type <ei> exception_identifier  
%type <el> typed_const_list  opt_expr_list elem_list opt_expr_list_with_bracket expr_list const_elem_list1 const_func_expr_list case_label_list const_elem_list opt_const_func_expr_list elem_list1  
%type <en> enumeration_id  
%type <enl> enumeration_id_list  
%type <ex> const_simple_expr term typed_const typed_const_or_new expr const_expr elem range_expr const_elem array_const factor relop_expr expr_l1 simple_expr range_term range_factor 
%type <ex> external_directr_ident init_const_expr case_label not_property_interface_index var_init_value var_init_value_typed variable var_reference
%type <fct> for_cycle_type  
%type <fe> format_expr  
%type <fes> foreach_stmt  
%type <fn> for_stmt  
%type <fp> fp_list fp_sect_list  
%type <ft> file_type  
%type <ga> var_address  
%type <gs> goto_stmt  
%type <id> func_name_ident param_name const_field_name func_name_with_template_args identifier_or_keyword unit_name exception_variable const_name func_meth_name_ident label_name type_decl_identifier template_identifier_with_equal 
%type <id> program_param program_name identifier unit_key_word identifier_keyword_operatorname  func_class_name_ident opt_identifier var_name visibility_specifier func_decl_lambda
%type <id> real_type_name ord_type_name variant_type_name property_specifier_directives non_reserved other
%type <ifn> if_stmt if_then_else_branch  
%type <ifp> initialization_part  
%type <il> template_arguments label_list var_name_list ident_or_keyword_pointseparator_list ident_list  param_name_list not_parameter_name_list 
%type <im> inherited_message  
%type <ind> implementation_part  
%type <inn> interface_part abc_interface_part  
%type <it> simple_type_list  
%type <lit> literal one_literal
%type <lcn> literal_list  
%type <lds> label_decl_sect  
%type <lo> ident_list1 ident_list2
%type <ls> lock_stmt  
%type <mn> func_name proc_name opt_proc_name qualified_identifier 
%type <nex> new_expr allowable_expr_as_stmt  
%type <node> parse_goal parts var_or_identifier abc_proc_block not_constructor_block_decl abc_block proc_block proc_block_decl func_block block
%type <ntr> exception_class_type_identifier simple_type_identifier  
%type <ntr> base_class_name  
%type <ntrl> base_classes_names_list opt_base_classes
%type <ob> one_compiler_directive filed_or_const_definition_or_am opt_head_compiler_directives head_compiler_directives program_heading_2 opt_tk_point program_param_list not_guid opt_semicolon
%type <oni> operator_name_ident  
%type <op> const_relop const_addop assign_operator const_mulop relop addop mulop sign overload_operator
%type <opt> typecast_op  
%type <pa> not_property_specifiers
%type <pad> not_array_defaultproperty 
%type <pal> meth_modificators opt_meth_modificators  
%type <pat> meth_modificator field_access_modifier
%type <pc> proc_call  
%type <pd> abc_proc_decl func_decl proc_decl abc_constructor_decl constructor_decl func_decl_with_attr constructor_decl
%type <pd> abc_destructor_decl abc_method_decl proc_decl_with_attr constructor_decl_with_attr abc_func_decl destructor_decl destructor_decl_with_attr abc_func_decl_noclass  proc_decl_noclass abc_proc_decl_noclass func_decl_noclass
%type <ph> not_method_heading procedural_type_decl procedural_type_kind not_destructor_heading proc_heading procedural_type not_constructor_heading_object not_constructor_heading not_procfunc_heading_variants 
%type <ph> func_heading not_procfunc_heading int_func_heading int_proc_heading
%type <pint> not_property_interface
%type <pm> program_file  
%type <pn> program_heading  
%type <pp> not_parameter_decl
%type <ppl> not_parameter_decl_list not_property_parameter_list
%type <psc> const_set  
%type <qce> question_expr question_constexpr  
%type <rc> record_const const_field_list_1 const_field_list  
%type <rcd> const_field  
%type <rn> repeat_stmt  
%type <rs> raise_stmt  
%type <rt> pointer_type  
%type <sal> attribute_declaration one_or_some_attribute  
%type <se> maybe_error
%type <sl> stmt_list else_case exception_block_else_branch  compound_stmt  
%type <snd> string_type  
%type <soo> sizeof_expr  
%type <sp> simple_not_property_definition not_simple_property_definition
%type <st> stmt_or_expression then_branch else_branch  unlabelled_stmt lambda_body stmt case_item
%type <std> set_type  
%type <tcn> as_is_expr as_is_constexpr  
%type <td> unsized_array_type simple_type_or_ simple_type array_name_for_new_expr foreach_stmt_ident_dype_opt fptype_new fptype type_ref array_type 
%type <td> template_param structured_type unpacked_structured_type simple_or_template_type_reference type_ref_or_secific for_stmt_decl_or_assign type_decl_type
%type <tdl> type_ref_and_secific_list  
%type <tds> type_decl_sect  
%type <th> try_handler  
%type <ti> record_keyword class_or_interface_keyword opt_tk_do keyword reserved_keyword class_attributes 
%type <too> typeof_expr  
%type <tpars> simple_fp_sect   
%type <tpl> template_param_list template_type_params  
%type <ttr> template_type
%type <ts> try_stmt  
%type <ugl> using_clause using_list  
%type <ul> main_uses_clause uses_clause main_used_units_list  
%type <um> unit_file  
%type <uon> using_one main_used_unit_name
%type <un> unit_heading  
%type <vd> var_decl_sect  
%type <vds> var_decl var_decl_part not_field_definition var_decl_part_in_stmt var_decl_part_assign var_decl_part_normal
%type <vs> var_stmt  
%type <wd> where_part  
%type <wdl> where_part_list opt_where_section  
%type <wn> while_stmt  
%type <ws> with_stmt  

%%

parse_goal                
    : program_file {root = $1;}
    | unit_file {root = $1;}
    | parts {root = $1;}
    ;

parts
    : tkParseModeExpression expr
        {$$=$2;}
    | tkParseModeStatement stmt_or_expression
        {$$=$2;}
    ;

stmt_or_expression
    : expr 
        { $$ = new expression_as_statement($1,@$);}
    | assignment
        { $$ = $1; }
    | var_stmt
        { $$ = $1; }
    ;
    
opt_head_compiler_directives
    :
        { $$=null; }
    |  head_compiler_directives
        { $$=null; }
    ;

head_compiler_directives
    : one_compiler_directive             
        {$$=null;}
    |  head_compiler_directives one_compiler_directive
        {$$=null;}
    ;

one_compiler_directive
    : tkDirectiveName tkIdentifier                 
        {
			token_info t1 = new token_info($1.name,@1);
			token_info t2 = new token_info($2.name,@2);
			compiler_directive cd=new compiler_directive(t1,t2,@$); 
			CompilerDirectives.Add(cd); 
			$$=null;
        }
    |  tkDirectiveName tkStringLiteral             
        {
			token_info t1 = new token_info($1.name,@1);
			token_info t2 = new token_info(((string_const)$2).Value,@2);
			compiler_directive cd=new compiler_directive(t1,t2,@$); 
			CompilerDirectives.Add(cd); 
			$$=null;
        }
    ;

program_file
    : program_heading opt_head_compiler_directives main_uses_clause using_clause program_block opt_tk_point
        { 
			$$ = new program_module($1 as program_name,(uses_list)$3,(block)$5,$4 as using_list,@1.Merge(@5)); 
			$$.Language = LanguageId.PascalABCNET;
			if ($6 == null && $5 != null)
			{
			   file_position fp = ($5 as syntax_tree_node).source_context.end_position;
			   syntax_tree_node err_stn = (syntax_tree_node)$5;
			   if (($5 is block) && ($5 as block).program_code != null && ($5 as block).program_code.subnodes != null && ($5 as block).program_code.subnodes.Count > 0)
			   err_stn = ($5 as block).program_code.subnodes[($5 as block).program_code.subnodes.Count-1];
			   parsertools.errors.Add(new PABCNETUnexpectedToken(current_file_name, StringResources.Get("TKPOINT"),new SourceContext(fp.line_num, fp.column_num+1, fp.line_num, fp.column_num+1, 0, 0),err_stn));
			}
        }
		;
		
opt_tk_point
    : tkPoint
        {$$=$1;}
    | tkSemiColon
        {$$=null;}
    | tkColon
        {$$=null;}
    | tkComma
        {$$=null;}
    | tkDotDot
        {$$=null;}
    |
    ;
        
program_heading           
    :  
		{ $$ = null; } 
    |  tkProgram program_name program_heading_2    
        { $$ = new program_name((ident)$2,@$); }
    ;

program_heading_2
    : tkSemiColon
        {$$=null;}
    | tkRoundOpen program_param_list tkRoundClose tkSemiColon
        {$$=null;}
    ;

program_name
    : identifier
        {$$=$1;}
    ;

program_param_list
    : program_param 
        {$$=null;}
    |  program_param_list tkComma program_param
        {$$=null;}
    ;

program_param
    : identifier
        {$$=$1;}
    ;

program_block
    : program_decl_sect_list compound_stmt     
        { 
		  $$ = new block(null,$2 as statement_list,@$); 
          if ($1!=null) 
		  {
            $$.defs=$1 as declarations;
            //parsertools.create_source_context($$,$1,$2);
          }
		  //else   
            //parsertools.create_source_context($$,$2,$2);
          // tasha 16.04.2010 
          parsertools.add_lambda_to_program_block($$);
        }
    ;

program_decl_sect_list
    : impl_decl_sect_list
        {$$=$1;}
    ;

uses_clause
    : main_uses_clause
        {$$=$1;}
    ;

using_clause
    :
        {$$=null;}
    |  using_list
        {$$=$1;}
    ;

using_list
    : using_one                                 
        { 
			$$ = new using_list(); 
			$$.namespaces.Add((unit_or_namespace)$1);
			$$.source_context = @$;
		}
    |  using_list using_one                           
        { 
			$$ = (using_list)$1;
			$$.namespaces.Add((unit_or_namespace)$2);
			$$.source_context = @1.Merge(@2);
		} 
    ;

using_one
    :tkUsing ident_or_keyword_pointseparator_list tkSemiColon
        { 
			$$ = new unit_or_namespace((ident_list)$2,@$);  
		}
    ;

ident_or_keyword_pointseparator_list
    : identifier_or_keyword                                   
        { 
			$$ = new ident_list();  
			$$.idents.Add($1);
			$$.source_context = @1;
		}
    | ident_or_keyword_pointseparator_list tkPoint identifier_or_keyword 
        { 
			$$ = $1; 
			$$.idents.Add($3); 
			$$.source_context = @1.Merge(@3);
		}
    ;

main_uses_clause
    :
    |  tkUses main_used_units_list tkSemiColon           
        { 
			$$=$2;
			$$.source_context = @$;
		}
    ;

main_used_units_list
    : main_used_units_list tkComma main_used_unit_name
        { 
		  $$ = (uses_list)$1;
          $$.units.Add((unit_or_namespace)$3);
        } 
    |  main_used_unit_name                              
        { 
		  $$ = new uses_list(); 
          $$.units.Add((unit_or_namespace)$1);
        }
    ;

main_used_unit_name
    : ident_or_keyword_pointseparator_list      
        { $$ = new unit_or_namespace((ident_list)$1); }
    |  ident_or_keyword_pointseparator_list tkIn tkStringLiteral
        { $$ = new uses_unit_in(); 
        $$.name=(ident_list)$1;
        ($$ as uses_unit_in).in_file=(string_const)$3;
        }
    ;

unit_file
    : unit_heading interface_part implementation_part initialization_part tkPoint
        { 
			$$ = new unit_module((unit_name)$1,(interface_node)$2,(implementation_node)$3,((initfinal_part)$4).initialization_sect,((initfinal_part)$4).finalization_sect, @$); 
			$$.Language = LanguageId.PascalABCNET;                               
        }
    | unit_heading abc_interface_part initialization_part tkPoint
        { 
			$$ = new unit_module((unit_name)$1,(interface_node)$2,null,((initfinal_part)$3).initialization_sect,((initfinal_part)$3).finalization_sect, @$); 
			$$.Language = LanguageId.PascalABCNET;
        }
    ;

unit_heading
    : unit_key_word unit_name tkSemiColon opt_head_compiler_directives
        { 
			$$ = new unit_name((ident)$2,UnitHeaderKeyword.Unit,@$); 
			if(((ident)$1).name.ToLower()=="library")
				$$.HeaderKeyword=UnitHeaderKeyword.Library;
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
    : tkInterface uses_clause using_clause int_decl_sect_list 
        { 
			$$ = new interface_node($4 as declarations,$2 as uses_list,$3 as using_list,@$); 
        }
    ;

implementation_part
    : tkImplementation uses_clause using_clause impl_decl_sect_list
        { 
			$$ = new implementation_node($2 as uses_list,$4 as declarations,$3 as using_list,@$); 
        }
	;
		
abc_interface_part
    : uses_clause using_clause impl_decl_sect_list    
        { 
			$$ = new interface_node($3 as declarations,$1 as uses_list,$2 as using_list,@$); 
        }
    ;

initialization_part                                           
    : tkEnd                                              
        { 
			$$ = new initfinal_part(); 
		}
    | tkInitialization stmt_list tkEnd                 
        { 
		  $$ = new initfinal_part($2,null,@$); 
          $2.left_logical_bracket=$1;
          $2.right_logical_bracket=$3;
        }
    | tkInitialization stmt_list tkFinalization stmt_list tkEnd
        { 
		  $$ = new initfinal_part($2,$4,@$); 
          $2.left_logical_bracket=$1;
          $4.left_logical_bracket=$3;
          $4.right_logical_bracket=$5;
        }
    | tkBegin stmt_list tkEnd                          
        { 
		  $$ = new initfinal_part($2,null,@$); 
          $2.left_logical_bracket=$1;
          $2.right_logical_bracket=$3;
        }
    ;

int_decl_sect_list
    : int_decl_sect_list1         
        {
			if (((declarations)$1).defs.Count>0) 
				$$ = $1; 
			else $$ =  null;
		}
	;
	
int_decl_sect_list1
    :                                      
        { $$ = new declarations(); }
    | int_decl_sect_list1 int_decl_sect      
        { 
			$$ = (declarations)$1;
			$$.defs.Add((declaration)$2);
			$$.source_context = @$;
        } 
    ;

impl_decl_sect_list
    : impl_decl_sect_list1                      
        {
			if (((declarations)$1).defs.Count>0) 
				$$ = $1; 
			else $$ = null;
		}
	;
	
impl_decl_sect_list1
    :                        
        { $$ = new declarations(); }
    | impl_decl_sect_list1 impl_decl_sect     
        { 
			$$ = (declarations)$1;
			$$.defs.Add((declaration)$2);
			$$.source_context = @$;
        } 
	;
	
abc_decl_sect_list
    :abc_decl_sect_list1                  
        {
			if (((declarations)$1).defs.Count>0) 
				$$ = $1; 
			else $$ = null;
		}
	;
	
abc_decl_sect_list1
    :                        
        { $$ = new declarations(); }
    |  abc_decl_sect_list1 abc_decl_sect   
        { 
			$$ = (declarations)$1;
			$$.defs.Add((declaration)$2);
			$$.source_context = @$;
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
    | int_proc_heading
		{ $$ = $1; }
    | int_func_heading
		{ $$ = $1; }
    ;

impl_decl_sect
    : label_decl_sect
		{ $$ = $1; }
    |  const_decl_sect
		{ $$ = $1; }
    |  res_str_decl_sect
		{ $$ = $1; }
    |  type_decl_sect
		{ $$ = $1; }
    |  var_decl_sect
		{ $$ = $1; }
    |  proc_decl_with_attr
		{ $$ = $1; }
    |  func_decl_with_attr
		{ $$ = $1; }
    |  constructor_decl_with_attr
		{ $$ = $1; }
    |  destructor_decl_with_attr
		{ $$ = $1; }
    ;

proc_decl_with_attr
    : opt_attribute_declarations proc_decl
        {  
			$$=($2 as procedure_definition);
			if ($$.proc_header != null)
				$$.proc_header.attributes = $1 as attribute_list;
        }
    ;
            
func_decl_with_attr
    : opt_attribute_declarations func_decl
        {  
			$$=($2 as procedure_definition);
			if ($$.proc_header != null)
				$$.proc_header.attributes = $1 as attribute_list;
        }
    ;
            
constructor_decl_with_attr
    : opt_attribute_declarations constructor_decl
        {  
			$$=($2 as procedure_definition);
			if ($$.proc_header != null)
				$$.proc_header.attributes = $1 as attribute_list;
        }
    ;

destructor_decl_with_attr
    : opt_attribute_declarations destructor_decl
        {  
			$$=($2 as procedure_definition);
			if ($$.proc_header != null)
				$$.proc_header.attributes = $1 as attribute_list;
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

int_proc_heading
    : opt_attribute_declarations proc_heading
        {  
			$$=($2 as procedure_header);
			$$.attributes = $1 as attribute_list;
        }
    | opt_attribute_declarations proc_heading tkForward tkSemiColon    
        {  
			$$=($2 as procedure_header);
			if ($$.proc_attributes==null) 
				$$.proc_attributes=new procedure_attributes_list();
			$$.proc_attributes.proc_attributes.Add((procedure_attribute)$3);
			$$.attributes = $1 as attribute_list;
			$$.source_context = @$;
			$$.proc_attributes.source_context = @3;
		}
    ;

int_func_heading
    : opt_attribute_declarations func_heading
        {  
			$$=($2 as procedure_header);
			$$.attributes = $1 as attribute_list;
        }
    |  opt_attribute_declarations func_heading tkForward tkSemiColon       
        {  
			$$=($2 as procedure_header);
			if ($$.proc_attributes==null) 
				$$.proc_attributes=new procedure_attributes_list();
			$$.proc_attributes.proc_attributes.Add((procedure_attribute)$3);
			$$.attributes = $1 as attribute_list;
			$$.source_context = @$;
			$$.proc_attributes.source_context = @3;
		}
    ;

label_decl_sect
    : tkLabel label_list tkSemiColon           
        { 
			$$ = new label_definitions((ident_list)$2,@$); 
		}
    ;

label_list
    : label_name                               
        { 
			$$ = new ident_list();  
			$$.idents.Add($1); 
			$$.source_context = @$;
		}
    |  label_list tkComma label_name          
        { 
			$$ = $1; 
			$$.idents.Add($3); 
			$$.source_context = @$;
		}
    ;

label_name
    : tkInteger                           
        { 
			$$ = new ident(); 
			if ($1 is int32_const)
				$$.name = ((int32_const)$1).val.ToString();
			else if ($1 is int64_const)
				$$.name = ((int64_const)$1).val.ToString();
			else
				$$.name = ((uint64_const)$1).val.ToString();
			$$.source_context = @$;
		}
    |  tkFloat                              
        { 
			$$ = new ident(((double_const)$1).val.ToString());  
			$$.source_context = @$;
		}
    |  identifier
		{ $$ = $1; }
    ;

const_decl_sect
    : tkConst const_decl                       
        { 
			$$ = new consts_definitions_list(); 
			$$.const_defs.Add((const_definition)$2);
			$$.source_context = @$;
		}
    |  const_decl_sect const_decl          
        { 
			$$ = (consts_definitions_list)$1;
			$$.const_defs.Add((const_definition)$2);
			$$.source_context = @$;
		} 
    ;

res_str_decl_sect
    : tkResourceString const_decl
		{ $$ = $2; }
    |  res_str_decl_sect const_decl
		{ $$ = $1; }
    ;

type_decl_sect
    : tkType type_decl                         
        { 
			$$ = new type_declarations(); 
			///////////////tasha 28.04.2010
			parsertools.pascalABC_type_declarations.Add((type_declaration)$2);
			///////////////////////////////
			$$.types_decl.Add((type_declaration)$2);
			$$.source_context = @$;
		}
    |  type_decl_sect type_decl        
        { 
			$$ = (type_declarations)$1;
			$$.types_decl.Add((type_declaration)$2);
			$$.source_context = @$;
		} 
    ;

var_decl_sect
    : tkVar var_decl                     
        { 
			$$ = new variable_definitions(); 
			///////////////tasha 28.04.2010
			parsertools.pascalABC_var_statements.Add((var_def_statement)$2);
			///////////////////////////////
			$$.var_definitions.Add((var_def_statement)$2);
			$$.source_context = @$;
		}
    | tkEvent var_decl
        { 
			$$ = new variable_definitions(); 
			$$.var_definitions.Add((var_def_statement)$2);
			($2 as var_def_statement).is_event = true;
			$$.source_context = @$;
        }
    |  var_decl_sect var_decl              
        { 
			$$ = (variable_definitions)$1;
			$$.var_definitions.Add((var_def_statement)$2);
			$$.source_context = @$;
		} 
    ;

const_decl
    : only_const_decl tkSemiColon        
        { $$=$1; }
    ;

only_const_decl
    : const_name tkEqual init_const_expr       
        { 
			$$ = new simple_const_definition((ident)$1,(expression)$3,@$); 
		}
    |  const_name tkColon type_ref tkEqual typed_const
        { 
			$$ = new typed_const_definition((type_definition)$3,@$); 
			$$.const_name=(ident)$1;
			$$.const_value=(expression)$5;
		} 
    ;

init_const_expr
    : const_expr
		{ $$ = $1; }
    |  array_const
		{ $$ = $1; }
    ;

const_name
    : identifier
		{ $$ = $1; }
    ;

const_expr
    : const_simple_expr
		{ $$ = $1; }
    |  const_simple_expr const_relop const_simple_expr   
        { $$ = new bin_expr((expression)$1,(expression)$3,((op_type_node)$2).type,@$); }
    |  question_constexpr
		{ $$ = null; }
    ;

question_constexpr
    : const_expr tkQuestion const_expr tkColon const_expr    
        { $$ = new question_colon_expression((expression)$1,(expression)$3,(expression)$5,@$); }
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
    |  const_simple_expr const_addop const_term      
        { $$ = new bin_expr((expression)$1,(expression)$3,((op_type_node)$2).type,@$); }
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
			$$ = new typecast_node((addressed_value)$1,(type_definition)$3,(op_typecast)$2); 
			if (!($1 is addressed_value)) 
				parsertools.errors.Add(new bad_operand_type(current_file_name,((syntax_tree_node)$1).source_context,$$));                                
		}
    ;

const_term
    : const_factor
		{ $$ = $1; }
    | as_is_constexpr
		{ $$ = $1; }
    | const_term const_mulop const_factor                  
        { $$ = new bin_expr((expression)$1,(expression)$3,((op_type_node)$2).type,@$); }
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

const_factor
    : const_variable
		{ $$ = $1; }
    | const_set
		{ $$ = $1; }
    | unsigned_number
		{ $$ = $1; }
    | literal
		{ $$ = $1; }
    | tkNil                        
        { 
			$$ = new nil_const();  
			$$.source_context = @$;
		}
    | tkAddressOf const_factor                         
        { 
			$$ = new get_address((addressed_value)$2,@$);  
		}
    | tkRoundOpen const_expr tkRoundClose              
        { 
			$$ = new bracket_expr((expression)$2,@$); 
		}
    | tkNot const_factor                
        { 
			$$ = new un_expr((expression)$2,((op_type_node)$1).type,@$); 
		}
    | sign const_factor                              
        { 
			$$ = new un_expr((expression)$2,((op_type_node)$1).type,@$); 
		}
    | tkDeref const_factor              
        { 
			$$ = new roof_dereference(); 
			($$ as roof_dereference).dereferencing_value=(addressed_value)$2;
			$$.source_context = @$;
		}
    ;

const_set
    : tkSquareOpen const_elem_list tkSquareClose 
        { 
			$$ = new pascal_set_constant($2 as expression_list,@$); 
		}
    ;

sign
    : tkPlus
		{$$ = $1;}
    | tkMinus
		{$$ = $1;}
    ;

const_variable
    : identifier
		{ $$ = $1; }
    |  sizeof_expr
		{ $$ = $1; }
    |  typeof_expr
		{ $$ = $1; }
    |  const_variable const_variable_2        
        {
			if ($2 is dereference) 
				((dereference)$2).dereferencing_value=(addressed_value)$1;
			if ($2 is dot_node) 
				((dot_node)$2).left=(addressed_value)$1;
			$$ = $2;
			$$.source_context = @$;
        }
    ;

const_variable_2
    : tkPoint identifier_or_keyword                               
        { $$ = new dot_node(null,(addressed_value)$2,@$); }
    |  tkDeref                                            
        { 
			$$ = new roof_dereference();  
			$$.source_context = @$;
		}
    |  tkRoundOpen opt_const_func_expr_list tkRoundClose 
        { 
			$$ = new method_call((expression_list)$2,@$);  
		}
    |  tkSquareOpen const_elem_list tkSquareClose     
        { 
			$$ = new indexer((expression_list)$2,@$);  
		}
    ;

opt_const_func_expr_list
    : const_func_expr_list
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;
        
const_func_expr_list
    : const_expr                               
        { 	
			$$ = new expression_list();  
			$$.expressions.Add($1); 
			$$.source_context = @$;
		}
    |  const_func_expr_list tkComma const_expr 
        { 
			$$ = $1; 
			$$.expressions.Add($3); 
			$$.source_context = @$;
		}
    ;

const_elem_list
    : const_elem_list1
		{ $$ = $1; }
    |
    ;

const_elem_list1
    : const_elem                              
        { 
			$$ = new expression_list();  
			$$.expressions.Add($1); 
			$$.source_context = @$;
		}
    |  const_elem_list1 tkComma const_elem 
        { 
			$$ = $1; 
			$$.expressions.Add($3); 
			$$.source_context = @$;
		}
    ;

const_elem
    : const_expr
		{ $$ = $1; }
    |  const_expr tkDotDot const_expr      
        { $$ = new diapason_expr((expression)$1,(expression)$3,@$); }
    ;

unsigned_number
    : tkInteger
		{ $$ = $1; }
    | tkHex
		{ $$ = $1; }
    | tkFloat
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
			$$ = new array_const((expression_list)$2); 
			$$.source_context = @$;
		}
    |  tkRoundOpen record_const tkRoundClose    
        { $$ = $2; }
    |  tkRoundOpen array_const tkRoundClose     
        { $$ = $2; }
    ;

typed_const_list
    :
    |  typed_const_or_new  
        { 
			$$ = new expression_list(); 
			$$.expressions.Add((expression)$1);
			$$.source_context = @$;
        }
    |  typed_const_list tkComma typed_const_or_new   
        { 
			$$ = (expression_list)$1;
			if ($$ == null)
			{
				$$ = new expression_list();
				parsertools.errors.Add(new PABCNETUnexpectedToken(current_file_name, StringResources.Get("TKIDENTIFIER"),((syntax_tree_node)$2).source_context,$$));
			}
			else
			{
				$$.expressions.Add((expression)$3);
				$$.source_context = @$;
			}
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
    |  const_field_list_1 tkSemiColon           
        { $$ = $1; }
	;
	
const_field_list_1
    : const_field                                 
        { 
			$$ = new record_const();  
			$$.rec_consts.Add($1); 
			$$.source_context = @$;
		}
    |  const_field_list_1 tkSemiColon const_field  
        { 
			$$ = $1; 
			$$.rec_consts.Add($3); 
			$$.source_context = @$;
		}
    ;

const_field
    : const_field_name tkColon typed_const     
        { 
			$$ = new record_const_definition((ident)$1,(expression)$3,@$); 
		}
    ;

const_field_name
    : identifier
		{ $$ = $1; }
    ;

type_decl 
    : opt_attribute_declarations simple_type_decl
        {  
			$$ = $2 as declaration;
			$$.attributes = $1 as attribute_list;
			$$.source_context = @$;
        }
    ;

opt_attribute_declarations
    : attribute_declarations 
        { $$=$1; }
    | 
		{ $$ = null; }
    ;
    
attribute_declarations
    : attribute_declaration 
        { 
			$$ = new attribute_list(); 
			$$.attributes.Add((simple_attribute_list)$1);
			$$.source_context = @$;
    }
    | attribute_declarations attribute_declaration
        { 
			$$ = (attribute_list)$1;
			$$.attributes.Add((simple_attribute_list)$2);
			$$.source_context = @$;
		}
    ;
    
attribute_declaration
    : tkSquareOpen one_or_some_attribute tkSquareClose
        { $$ = $2; }
    ;

one_or_some_attribute
    : one_attribute 
        { 
			$$ = new simple_attribute_list();  
			$$.attributes.Add($1); 
			$$.source_context = @$;
		}
    | one_or_some_attribute tkComma one_attribute
        { 
			$$ = $1; 
			$$.attributes.Add($3); 
			$$.source_context = @$;
		}
    ;
        
one_attribute
    : attribute_variable
		{ $$ = $1; }
    | identifier tkColon attribute_variable
        {  
			$$=$3 as attribute;
			$$.qualifier = $1 as ident;
			$$.source_context = @$;
        }
    ;
        
simple_type_decl
    : type_decl_identifier tkEqual type_decl_type tkSemiColon 
        { 
			$$ = new type_declaration((ident)$1,(type_definition)$3,@$); 
		}
    |  template_identifier_with_equal type_decl_type tkSemiColon 
        { 
			$$ = new type_declaration((ident)$1,(type_definition)$2,@$); 
		}
    ;

type_decl_identifier
    : identifier
		{ $$ = $1; }
    |  identifier template_arguments           
        { 
			$$ = new template_type_name((ident_list)$2,@$); 
			$$.name=((ident)$1).name;
        }
	;
	
template_identifier_with_equal
    : identifier tkLower ident_list tkGreaterEqual   
        { 
			$$ = new template_type_name((ident_list)$3,@$); 
			$$.name=((ident)$1).name;
        }
    ;

type_decl_type
    : type_ref
		{ $$ = $1; }
    |  tkType type_ref                                 
        {
			$$=$2;
			$$.source_context = @$;
		}
    |  object_type
		{ $$ = $1; }
    ;

type_ref
    : simple_type
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
        { $$ = new template_type_reference((named_type_reference)$1,(template_param_list)$2,@$); }
    ;

template_type_params
    : tkLower template_param_list tkGreater            
        {
			$$=$2;
			$$.source_context = @$;
		}
    ;

template_param_list
    : template_param                              
        { 
			$$ = new template_param_list();  
			$$.params_list.Add($1); 
			$$.source_context = @$;
		}
    |  template_param_list tkComma template_param  
        { 
			$$ = $1; 
			$$.params_list.Add($3); 
			$$.source_context = @$;
		}
    ;

template_param
    : simple_type_identifier
		{ $$ = $1; }
    |  template_type
		{ $$ = $1; }
    ;

simple_type
    : simple_type_identifier
	    { $$ = $1; }
    |  range_expr tkDotDot range_expr  
        { $$ = new diapason((expression)$1,(expression)$3,@$); }
    |  tkRoundOpen enumeration_id_list tkRoundClose
        { 
			$$ = new enum_type_definition((enumerator_list)$2,@$);  
		}
    ;

range_expr
    : range_term
		{ $$ = $1; }
    |  range_expr const_addop range_term    
        { 
			$$ = new bin_expr($1 as expression,$3 as expression,((op_type_node)$2).type,@$); 
		}
    ;

range_term
    : range_factor
		{ $$ = $1; }
    |  range_term const_mulop range_factor  
        { 
			$$ = new bin_expr($1 as expression,$3 as expression,((op_type_node)$2).type,@$); 
		}
    ;

range_factor
    : simple_type_identifier                   
        { 
			if(((named_type_reference)$1).names.Count>0)
				$$=((named_type_reference)$1).names[0];
			else
				$$=null;
        }
    |  unsigned_number
		{ $$ = $1; }
    |  sign range_factor           
        { $$ = new un_expr((expression)$2,((op_type_node)$1).type,@$); }
    |  literal
		{ $$ = $1; }
    |  range_factor tkRoundOpen const_elem_list tkRoundClose
        { 
			$$ = new method_call((expression_list)$3,@$); 
			($$ as method_call).dereferencing_value=(addressed_value)$1;
        }
    |  tkRoundOpen const_expr tkRoundClose      
        { $$=$2;}
    ;

simple_type_identifier        
    : identifier                              
        { 
			$$ = new named_type_reference();  
			$$.names.Add($1); 
			$$.source_context = @$;
		}
    | simple_type_identifier tkPoint identifier_or_keyword
        { 
			$$ = $1; 
			$$.names.Add($3); 
			$$.source_context = @$;
		}
    ;

enumeration_id_list
    : enumeration_id tkComma enumeration_id  
        { 
			$$ = new enumerator_list(); 
			$$.enumerators.Add((enumerator)$1);
			$$.enumerators.Add((enumerator)$3);
			$$.source_context = @$;
        }      
    |  enumeration_id_list tkComma enumeration_id
        { 
			$$ = (enumerator_list)$1;
			$$.enumerators.Add((enumerator)$3);
			$$.source_context = @$;
        }  
	;
	
enumeration_id
    : identifier          
        { $$ = new enumerator($1 as ident,null,@$); }
    | identifier tkEqual expr
        { $$ = new enumerator($1 as ident,$3 as expression,@$); }
    ;
        
pointer_type
    : tkDeref fptype             
        { 
			$$ = new ref_type((type_definition)$2,@$);
		}
    ;
                            
structured_type
    : unpacked_structured_type
		{ $$ = $1; }
    |  tkPacked unpacked_structured_type
		{ $$ = $2; }
    ;

unpacked_structured_type
    : array_type
		{ $$ = $1; }
    | new_record_type
		{ $$ = $1; }
    | set_type
		{ $$ = $1; }
    | file_type
		{ $$ = $1; }
    ;

array_type
    : tkArray tkSquareOpen simple_type_list tkSquareClose tkOf type_ref
        { 
			$$ = new array_type((indexers_types)$3,(type_definition)$6,@$); 
        }
    | unsized_array_type
		{ $$ = $1; }
    ;

unsized_array_type
    : tkArray tkOf type_ref                                              
        { 
			$$ = new array_type(null,(type_definition)$3,@$); 
        }
    ;
    
simple_type_list
    : simple_type_or_                                
        { 
			$$ = new indexers_types(); 
			$$.indexers.Add((type_definition)$1);
			$$.source_context = @$;
        }
    |  simple_type_list tkComma simple_type_or_   
        { 
			$$ = (indexers_types)$1;
			$$.indexers.Add((type_definition)$3);
			$$.source_context = @$;
        } 
    ;

simple_type_or_
    : simple_type
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;
        
set_type
    : tkSet tkOf simple_type         
        { 
			$$ = new set_type_definition((type_definition)$3,@$); 
		}
    ;

file_type
    : tkFile tkOf type_ref           
        { 
			$$ = new file_type((type_definition)$3,@$);
		}
    |  tkFile                               
        { 
			$$ = new file_type();  
			$$.source_context = @$;
		}
    ;

string_type
    : tkIdentifier tkSquareOpen const_expr tkSquareClose     
        { 
			$$ = new string_num_definition((expression)$3,(ident)$1,@$);
		}
    ;

procedural_type
    : procedural_type_kind
		{ $$ = $1; }
    ;

procedural_type_kind
    : procedural_type_decl
		{ $$ = $1; }
    ;

procedural_type_decl
    : tkProcedure fp_list maybe_error              
        { 
			$$ = new procedure_header((formal_parameters)$2,null,null,false,false,null,null,@$); 
			if($3!=null)
				($3 as SyntaxError).bad_node=$$;
        }
    |  tkFunction fp_list tkColon fptype       
        { 
			$$ = new function_header(); 
			if ($2!=null) 
				$$.parameters=(formal_parameters)$2;
			if ($4!=null) 
				($$ as function_header).return_type=(type_definition)$4;
			$$.of_object = false;
			$$.class_keyword = false;
			$$.source_context = @$;
        }
    ;

maybe_error
    : tkColon fptype                 
        { 
			$$ = new unexpected_return_value(current_file_name,((syntax_tree_node)$2).source_context,null); 
			parsertools.errors.Add($$);
		}
    |
		{ $$ = null; }
    ;

object_type
    : new_object_type
		{ $$ = $1; }
    ;

new_object_type
    : not_object_type
		{ $$ = $1; }
    ;

not_object_type
    : class_attributes class_or_interface_keyword opt_base_classes opt_where_section opt_not_component_list_seq_end
        { 
			$$ = new class_definition($3 as named_type_reference_list,$5 as class_body,class_keyword.Class,null,$4 as where_definition_list, class_attribute.None,@$); 
			string kw=($2 as token_info).text.ToLower();
			if($1!=null)
				$$.attribute=(class_attribute)(($1 as token_taginfo).tag);
			if (kw=="record") 
				$$.keyword=class_keyword.Record;
			else
			if (kw=="interface") 
				$$.keyword=class_keyword.Interface;
			else
			if (kw=="i") 
				$$.keyword=class_keyword.TemplateInterface;
			else
			if (kw=="r") 
				$$.keyword=class_keyword.TemplateRecord;
			else
			if (kw=="c") 
				$$.keyword=class_keyword.TemplateClass;
			if ($$.body!=null && $$.body.class_def_blocks!=null && 
				$$.body.class_def_blocks.Count>0 && $$.body.class_def_blocks[0].access_mod==null)
			{
				if($$.keyword==class_keyword.Class)
					$$.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.internal_modifer);
				else
					$$.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.none);
			}   
		}
    ;

new_record_type 
    : record_keyword opt_base_classes opt_where_section not_component_list_seq tkEnd   
        { 
			$$ = new class_definition($2 as named_type_reference_list,$4 as class_body,class_keyword.Record,null,$3 as where_definition_list, class_attribute.None); 
			if ($$.body!=null && $$.body.class_def_blocks!=null && 
				$$.body.class_def_blocks.Count>0 && $$.body.class_def_blocks[0].access_mod==null)
			{
                $$.body.class_def_blocks[0].access_mod = new access_modifer_node(access_modifer.public_modifer);
			}   
			$$.source_context = @$;
		}
    ;

class_attributes
    : tkFinal
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;

class_or_interface_keyword
    : tkClass
		{ $$ = $1; }
    | tkInterface
		{ $$ = $1; }
    | tkTemplate                                  
        { 
			$$ = (token_info)$1;
			$$.text="c";
		} 
    |  tkTemplate tkClass                                 
        { 
			$$ = (token_info)$1;
			$$.text="c";
			$$.source_context = @$;
		} 
    |  tkTemplate tkRecord                 
        { 
			$$ = (token_info)$1;
			$$.text="r";
			$$.source_context = @$;
		} 
    |  tkTemplate tkInterface                              
        { 
			$$ = (token_info)$1;
			$$.text="i";
			$$.source_context = @$;
		} 
    ;

record_keyword
    : tkRecord
		{ $$ = $1; }
    ;

opt_not_component_list_seq_end
    :
		{ $$ = null; }
    |  not_component_list_seq tkEnd          
        {
			$$=$1;
			$$.source_context = @$;
		}
    ;

opt_base_classes
    :
    |  tkRoundOpen base_classes_names_list tkRoundClose  
        { $$ = $2; }
    ;

base_classes_names_list
    : base_class_name                     
        { 
			$$ = new named_type_reference_list();  
			$$.types.Add($1); 
			$$.source_context = @$;
		}
    |  base_classes_names_list tkComma base_class_name
        { 
			$$ = $1; 
			$$.types.Add($3); 
			$$.source_context = @$;
		}
    ;

base_class_name
    : simple_type_identifier
		{ $$ = $1; }
    |  template_type
		{ $$ = $1; }
    ;

template_arguments
    : tkLower ident_list tkGreater           
        {
			$$ = $2;
			$$.source_context = @$;
		}
    ;

opt_where_section
    :
		{ $$ = null; }
    |  where_part_list
		{ $$ = $1; }
    ;

where_part_list
    : where_part               
        { 
			$$ = new where_definition_list(); 
			$$.defs.Add((where_definition)$1);
			$$.source_context = @$;
		}
    |  where_part_list where_part    
        { 
			$$ = (where_definition_list)$1;
			$$.defs.Add((where_definition)$2);
			$$.source_context = @$;
		} 
    ;

where_part
    : tkWhere ident_list tkColon type_ref_and_secific_list tkSemiColon
        { 
			$$ = new where_definition((ident_list)$2,(type_definition_list)$4,@$); 
		}
    ;

type_ref_and_secific_list
    : type_ref_or_secific             
        { 
			$$ = new type_definition_list();  
			$$.defs.Add($1); 
			$$.source_context = @$;
		}
    |  type_ref_and_secific_list tkComma type_ref_or_secific  
        { 
			$$ = $1; 
			$$.defs.Add($3); 
			$$.source_context = @$;
		}
    ;

type_ref_or_secific
    : type_ref
		{ $$ = $1; }
    |  tkClass         
        { 
			$$ = new declaration_specificator(DeclarationSpecificator.WhereDefClass, ($1 as token_info).text,@$); 
		}
    |  tkRecord                   
        { 
			$$ = new declaration_specificator(DeclarationSpecificator.WhereDefValueType, ($1 as token_info).text,@$); 
		}
    |  tkConstructor              
        { 
			$$ = new declaration_specificator(DeclarationSpecificator.WhereDefConstructor, ($1 as token_info).text,@$); 
		}
    ;

not_component_list_seq
    : not_component_list      
        { 
			$$ = new class_body(); 
			if ($1!=null) 
				$$.class_def_blocks.Add((class_members)$1);
			$$.source_context = @$;
        }
    |  not_component_list_seq ot_visibility_specifier not_component_list
        { 
			$$ = (class_body)$1;
			class_members cl = (class_members)$3;
			if (cl==null) 
			{   
				cl = new class_members();
				cl.source_context = @2;
			}
			cl.access_mod = (access_modifer_node)$2;
			$$.class_def_blocks.Add(cl);
			$$.source_context = @$;
        } 
    ;

ot_visibility_specifier
    : tkInternal           
        { $$ = new access_modifer_node(access_modifer.internal_modifer,@$); }
    |  tkPublic                   
        { $$ = new access_modifer_node(access_modifer.public_modifer,@$); }
    |  tkProtected                
        { $$ = new access_modifer_node(access_modifer.protected_modifer,@$); }
    |  tkPrivate                  
        { $$ = new access_modifer_node(access_modifer.private_modifer,@$); }
    ;

ident_list
    : identifier                               
        { 
			$$ = new ident_list();  
			$$.idents.Add($1); 
			$$.source_context = @$;
		}
    | ident_list tkComma identifier       
        { 
			$$ = $1; 
			$$.idents.Add($3); 
			$$.source_context = @$;
		}
    ;

not_component_list
    : not_guid
		{ $$ = null; }
    |  not_guid not_component_list_1 opt_semicolon         
        { $$ = $2; }
    |  not_guid not_component_list_2          
        { $$ = $2; }
    |  not_guid not_component_list_1 tkSemiColon not_component_list_2
        {  
			$$=(class_members)$2;
			for (int i=0;i<((class_members)$4).members.Count;i++)
				$$.members.Add(((class_members)$4).members[i]);
			$$.source_context = @$;
        }
	;
	
opt_semicolon
    :
		{ $$ = null; }
    |  tkSemiColon
		{ $$ = $1; }
    ;

not_guid
    :
		{ $$ = null; }
    ;

not_component_list_1
    : filed_or_const_definition                     
        { 
			$$ = new class_members(); 
			$$.members.Add((declaration)$1);
			$$.source_context = @$;
        }
    |  not_component_list_1 tkSemiColon filed_or_const_definition_or_am
        { 
			$$ = (class_members)$1;
			if($3 is declaration)
				$$.members.Add((declaration)$3);
			else
				($$.members[$$.members.Count-1] as var_def_statement).var_attr=definition_attribute.Static;
			$$.source_context = @$;
        }  
    ;

not_component_list_2
    : not_method_definition                    
        { 
			$$ = new class_members(); 
			$$.members.Add((declaration)$1);
			$$.source_context = @$;
        }
    |  not_property_definition                  
        { 
			$$ = new class_members(); 
			$$.members.Add((declaration)$1);
			$$.source_context = @$;
        }
    |  not_component_list_2 not_method_definition
        { 
			$$ = (class_members)$1;
			$$.members.Add((declaration)$2);
			$$.source_context = @$;
        }  
    |  not_component_list_2 not_property_definition
        { 
			$$ = (class_members)$1;
			$$.members.Add((declaration)$2);
			$$.source_context = @$;
        }  
    ;

filed_or_const_definition
    : opt_attribute_declarations simple_filed_or_const_definition
        {  
			$$=$2 as declaration;
			$$.attributes = $1 as attribute_list;
        }
    ;
    
simple_filed_or_const_definition
    : tkConst only_const_decl            
        { 
			$$=$2;
			$$.source_context = @$;
		}
    | not_field_definition
		{ $$ = $1; }
    | tkClass not_field_definition       
        { 
			$$ = (var_def_statement)$2;
			($$ as var_def_statement).var_attr = definition_attribute.Static;
			$$.source_context = @$;
        } 
    ;

filed_or_const_definition_or_am
    : filed_or_const_definition
		{ $$ = $1; }
    | field_access_modifier
		{ $$ = $1; }
    ;

not_field_definition
    : var_decl_part
		{ $$ = $1; }
    |  tkEvent var_name_list tkColon type_ref
        { 
			$$ = new var_def_statement((ident_list)$2,(type_definition)$4,null,definition_attribute.None,true,@$); 
        } 
    ;

field_access_modifier
    : tkStatic
		{ $$ = $1; }
    ;

not_method_definition
    : opt_attribute_declarations not_method_heading
        {  
			$$=($2 as declaration);
			if ($$ != null)
				$$.attributes = $1 as attribute_list; 
        }
    |  opt_attribute_declarations abc_method_decl
        {  
			$$=($2 as procedure_definition);
			if ($$ != null && ($$ as procedure_definition).proc_header != null)
				($$ as procedure_definition).proc_header.attributes = $1 as attribute_list; 
     }
    ;

abc_method_decl
    : abc_proc_decl
		{ $$ = $1; }
    | abc_func_decl
		{ $$ = $1; }
    | abc_constructor_decl
		{ $$ = $1; }
    | abc_destructor_decl
		{ $$ = $1; }
    ;

not_method_heading
    : tkClass not_procfunc_heading
        { 
			((procedure_header)$2).class_keyword=true;
			$$ = $2;
		}
    | not_procfunc_heading
		{ $$ = $1; }
    | not_constructor_heading
		{ $$ = $1; }
    | not_destructor_heading
		{ $$ = $1; }
    ;

not_procfunc_heading
    : not_procfunc_heading_variants 
        { 
			((procedure_header)$1).name.explicit_interface_name=((procedure_header)$1).name.class_name;
			((procedure_header)$1).name.class_name=null;
			$$=$1;
		}
    ;

not_procfunc_heading_variants
    : proc_heading
		{ $$ = $1; }
    | func_heading
		{ $$ = $1; }
    ;

not_constructor_heading
    : not_constructor_heading_object
		{ $$ = $1; }
    | tkClass not_constructor_heading_object
        { 
			((procedure_header)$2).class_keyword=true;
			$$=$2;
		}
    ;

opt_proc_name
    : proc_name
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;

not_constructor_heading_object
    : tkConstructor opt_proc_name fp_list opt_meth_modificators
        { 
			$$ = new constructor(); 
			object rt=$2;
			$$.name=$2 as method_name;
			if ($3!=null) 
			{
			  rt=$3;
			  $$.parameters=(formal_parameters)$3;
			}
			if ($4!=null) 
			{
			  rt=$4;
			  if (((procedure_attributes_list)$4).proc_attributes.Count > 0) 
				$$.proc_attributes=(procedure_attributes_list)$4;
			}
			parsertools.create_source_context($$,$1,rt);
        }
    ;

not_destructor_heading
    : tkDestructor opt_proc_name fp_list opt_meth_modificators
        { 
			$$ = new destructor(); 
			$$.name = $2 as method_name;
			if ($3!=null) 
			{
				$$.parameters=(formal_parameters)$3;
			}
			if ($4!=null) 
			{
				if (((procedure_attributes_list)$4).proc_attributes.Count>0) 
					$$.proc_attributes=(procedure_attributes_list)$4;
			}
			$$.source_context = @$;
        }
    ;

qualified_identifier
    : identifier                                       
        { $$ = new method_name(null,(ident)$1,null,@$); }
    | visibility_specifier                             
        { $$ = new method_name(null,(ident)$1,null,@$); }
    | qualified_identifier tkPoint identifier                   
        {
			$$ = (method_name)$1;
			$$.class_name=$$.meth_name;
			$$.meth_name=(ident)$3;
			$$.source_context = @$;
        }
    | qualified_identifier tkPoint visibility_specifier         
        {
			$$ = (method_name)$1;
			$$.class_name=$$.meth_name;
			$$.meth_name=(ident)$3;
			$$.source_context = @$;
        }
    ;

not_property_definition
    : opt_attribute_declarations simple_not_property_definition
        {  
			$$ = $2 as declaration;
			$$.attributes = $1 as attribute_list;
        }
    ;
    
simple_not_property_definition
    : not_simple_property_definition
		{ $$ = $1; }
    |  tkClass not_simple_property_definition    
        { 
			$$ = (simple_property)$2;
			$$.attr=definition_attribute.Static;
			$$.source_context = @$;
        } 
	;
	
not_simple_property_definition
    : tkProperty qualified_identifier not_property_interface not_property_specifiers tkSemiColon not_array_defaultproperty
        { 
			$$ = new simple_property(); 
			$$.property_name=((method_name)$2).meth_name;
			if ($3!=null)
			{
				$$.parameter_list=((property_interface)$3).parameter_list;
				$$.property_type=((property_interface)$3).property_type;
				$$.index_expression=((property_interface)$3).index_expression;
			}
			if ($4!=null) $$.accessors=(property_accessors)$4;
			if ($6!=null) $$.array_default=(property_array_default)$6;
			$$.source_context = @$;
        }
    ;

not_array_defaultproperty
    :
    |  tkDefault tkSemiColon                   
        { 
			$$ = new property_array_default();  
			$$.source_context = @$;
		}
    ;

not_property_interface
    :
    |  not_property_parameter_list tkColon fptype not_property_interface_index
        { 
			$$ = new property_interface(); 
			$$.parameter_list=(property_parameter_list)$1;
			$$.property_type=(type_definition)$3;
			$$.index_expression=(expression)$4;
			$$.source_context = @$;
        }
    ;

not_property_interface_index
    :
		{ $$ = null; }
    |  tkIndex expr                  
        { $$ = $2; }
    ;

not_property_parameter_list
    :
		{ $$ = null; }
    |  tkSquareOpen not_parameter_decl_list tkSquareClos
        { $$ = $2; }
    ;

not_parameter_decl_list
    : not_parameter_decl                                  
        { 
			$$ = new property_parameter_list();  
			$$.parameters.Add($1); 
			$$.source_context = @$;
		}
    |  not_parameter_decl_list tkSemiColon not_parameter_decl  
        { 
			$$ = $1; 
			$$.parameters.Add($3); 
			$$.source_context = @$;
		}
    ;

not_parameter_decl                                            
    : not_parameter_name_list tkColon fptype         
        { 
			$$ = new property_parameter((ident_list)$1,(type_definition)$3,@$); 
		}
    | tkConst not_parameter_name_list tkColon fptype
    | tkVar not_parameter_name_list tkColon fptype
    | tkOut not_parameter_name_list tkColon fptype
    ;

not_parameter_name_list
    : ident_list
		{ $$ = $1; }
    ;

opt_identifier
    : identifier
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;

not_property_specifiers
    :
    | tkReadOnly not_property_specifiers
    | tkWriteOnly not_property_specifiers
    | tkDefault const_expr not_property_specifiers
    | tkRead opt_identifier not_property_specifiers   
        { 
			$$ = $3 as property_accessors;
			if ($$==null) 
			{
				$$=new property_accessors();
			}
			if($2!=null && ((ident)$2).name.ToLower()=="write")
			{
				$$.read_accessor=new read_accessor_name(null);
				$$.write_accessor=new write_accessor_name(null);
				$$.read_accessor.source_context = @1;
				$$.write_accessor.source_context = @2;
			}
			else
			{
				$$.read_accessor = new read_accessor_name((ident)$2);                             
				$$.read_accessor.source_context = @1.Merge(@2);
			}
			$$.source_context = @$;
        }
    | tkWrite opt_identifier not_property_specifiers     
        { 
			$$ = $3 as property_accessors;
			if ($$==null) 
			{
				$$=new property_accessors();
			}
			$$.write_accessor = new write_accessor_name((ident)$2);
			$$.write_accessor.source_context = @1.Merge(@2);;
			$$.source_context = @$;
        }
    ;

var_decl
    : var_decl_part tkSemiColon              
        {$$=$1;}
    ;

var_decl_part
    : var_decl_part_normal
        { $$ = $1;}
    | var_decl_part_assign
        { $$ = $1;}
    | var_name_list tkColon type_ref tkAssign var_init_value_typed
        { $$ = new var_def_statement((ident_list)$1,(type_definition)$3,(expression)$5,definition_attribute.None,false,@$); }
    ;

var_decl_part_in_stmt
    : var_decl_part
        { $$ = $1;}
    ;

var_decl_part_assign
    : var_name_list tkAssign var_init_value
        { $$ = new var_def_statement((ident_list)$1,null,(expression)$3,definition_attribute.None,false,@$); }
    ;

var_decl_part_normal
    : var_name_list tkColon type_ref
        { $$ = new var_def_statement((ident_list)$1,(type_definition)$3,null,definition_attribute.None,false,@$); }
    ;

var_init_value
    : expr
        { $$ = $1; }
    ;

var_init_value_typed
    : typed_const_or_new
        { $$ = $1; }
    ;

typed_const_or_new
    : typed_const
		{ $$ = $1; }
    | new_expr
		{ $$ = $1; }
    | default_expr
		{ $$ = $1; }
    ;

var_name_list
    : var_name                                    
        { 
			$$ = new ident_list(); 
			$$.idents.Add((ident)$1);
			$$.source_context = @$;
		}
    | var_name_list tkComma var_name                 
        { 
			$$ = (ident_list)$1;
			$$.idents.Add((ident)$3);
			$$.source_context = @$;
		} 
    ;

var_name
    : identifier
		{ $$ = $1; }
    ;

constructor_decl
    : not_constructor_heading not_constructor_block_decl   
        { 
			$$ = new procedure_definition((procedure_header)$1,null,@$);
			if ($2!=null) 
			{
				if ($2 is proc_block) 
					$$.proc_body = (proc_block)$2;
				if ($2 is procedure_attribute) 
				{
					procedure_header ph = $$.proc_header;
					if (ph.proc_attributes==null) 
					{
						ph.proc_attributes = new procedure_attributes_list();
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)$2);
					ph.proc_attributes.source_context = @2;
				}
			}	
        }
    ;

abc_constructor_decl
    : not_constructor_heading abc_block        
        { 
			$$ = new procedure_definition((procedure_header)$1,null,@$);
			if ($2!=null) 
			{
				if ($2 is proc_block) 
					$$.proc_body = (proc_block)$2;
				if ($2 is procedure_attribute) 
				{
					procedure_header ph = $$.proc_header;
					if(ph.proc_attributes==null) 
					{
						ph.proc_attributes=new procedure_attributes_list();
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)$2);
					ph.proc_attributes.source_context = @2;
				}
			}	
        }
    ;

destructor_decl
    : not_destructor_heading not_constructor_block_decl
        { 
			$$ = new procedure_definition((procedure_header)$1,null,@$);
			if ($2!=null) 
			{
				if ($2 is proc_block) 
					$$.proc_body = (proc_block)$2;
				if ($2 is procedure_attribute) 
				{
					procedure_header ph = $$.proc_header;
					if(ph.proc_attributes==null) 
					{
						ph.proc_attributes=new procedure_attributes_list();
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)$2);
					ph.proc_attributes.source_context = @2;
				}
			}	
        }
    ;

abc_destructor_decl
    : not_destructor_heading abc_block         
        { 
			$$ = new procedure_definition((procedure_header)$1,null,@$);
			if ($2!=null) 
			{
				if ($2 is proc_block) 
					$$.proc_body = (proc_block)$2;
				if ($2 is procedure_attribute) 
				{
					procedure_header ph = $$.proc_header;
					if(ph.proc_attributes==null) 
					{
						ph.proc_attributes=new procedure_attributes_list();
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)$2);
					ph.proc_attributes.source_context = @2;
				}
			}	
        }
    ;

not_constructor_block_decl
    : block
		{ $$ = $1; }
    | external_directr
		{ $$ = $1; }
    ;

proc_decl
    : proc_decl_noclass
		{ $$ = $1; }
    | tkClass proc_decl_noclass             
        { 
			(($2 as procedure_definition).proc_header as procedure_header).class_keyword=true;
			$$=$2;
		}
    ;

proc_decl_noclass
    : proc_heading proc_block                      
        {
			$$ = new procedure_definition((procedure_header)$1,null,@$);
			if($2 != null) 
			{
				if ($2 is proc_block)
				{
					parsertools.add_lambda($2, $$);//tasha 16.04.2010
				}
				if ($2 is procedure_attribute) 
				{
					procedure_header ph = $$.proc_header;
					if (ph.proc_attributes==null) 
					{
						ph.proc_attributes = new procedure_attributes_list();
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)$2);
					ph.proc_attributes.source_context = @2;
				}
			}   
        }
    ;

abc_proc_decl
    : abc_proc_decl_noclass
		{ $$ = $1; }
    | tkClass abc_proc_decl_noclass         
        { 
			(($2 as procedure_definition).proc_header as procedure_header).class_keyword = true;
			$$ = $2;
		}
    ;

abc_proc_decl_noclass
    : proc_heading abc_proc_block                      
        {
			$$ = new procedure_definition((procedure_header)$1,null,@$);
			if ($2!=null) 
			{
				if ($2 is proc_block)
				{
					parsertools.add_lambda($2, $$); // tasha 16.04.2010
				}
				if ($2 is procedure_attribute) 
				{
					procedure_header ph = $$.proc_header;
					if (ph.proc_attributes==null) 
					{
						ph.proc_attributes = new procedure_attributes_list();
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)$2);
					ph.proc_attributes.source_context = @2;
				}
			}   
		}
    ;

func_decl
    : func_decl_noclass
		{ $$ = $1; }
    |  tkClass func_decl_noclass             
        { 
			(($2 as procedure_definition).proc_header as procedure_header).class_keyword=true;
			$$=$2;
		}
    ;

func_decl_noclass
    : func_heading func_block                        
        {
			$$ = new procedure_definition((function_header)$1,null,@$);
			if ($2!=null) 
			{
				if ($2 is proc_block)
				{
					parsertools.add_lambda($2, $$); // tasha 16.04.2010
				}
				if($2 is procedure_attribute) 
				{
					procedure_header ph = $$.proc_header;
					if (ph.proc_attributes==null) 
					{
						ph.proc_attributes = new procedure_attributes_list();
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)$2);
					ph.proc_attributes.source_context = @2;
				}
			}   
		}
    ;

abc_func_decl
    : abc_func_decl_noclass
		{ $$ = $1; }
    |  tkClass abc_func_decl_noclass         
        { 
			(($2 as procedure_definition).proc_header as procedure_header).class_keyword=true;
			$$=$2;
		}
    ;

abc_func_decl_noclass
    : func_heading abc_proc_block                    
        {
			$$ = new procedure_definition((function_header)$1,null,@$);
			if ($2!=null) 
			{
				if ($2 is proc_block)
				{
					parsertools.add_lambda($2, $$); // tasha 16.04.2010
				}
				if ($2 is procedure_attribute) 
				{
					procedure_header ph = $$.proc_header;
					if (ph.proc_attributes==null) 
					{
						ph.proc_attributes=new procedure_attributes_list();
					}
					ph.proc_attributes.proc_attributes.Add((procedure_attribute)$2);
					ph.proc_attributes.source_context = @2;
				}
			}   
        }
    ;

proc_heading
    : tkProcedure proc_name fp_list maybe_error opt_meth_modificators opt_where_section 
        { 
			$$ = new procedure_header(null,null,(method_name)$2,false,false,null,null,@$); 
			if ($$.name.meth_name is template_type_name)
			{
				$$.template_args = ($$.name.meth_name as template_type_name).template_args;
				ident id = new ident($$.name.meth_name.name,$$.name.meth_name.source_context);
				$$.name.meth_name = id;
			}
			if ($3!=null) 
			{
			  $$.parameters = (formal_parameters)$3;
			}
			if($4!=null)
				($4 as SyntaxError).bad_node = $$;
			if ($5!=null) 
			{
				if (((procedure_attributes_list)$5).proc_attributes.Count>0) 
					$$.proc_attributes = (procedure_attributes_list)$5;
			}
			if ($6!=null) 
			{
			  $$.where_defs = (where_definition_list)$6;
			}
        }
    ;

proc_name
    : func_name
		{ $$ = $1; }
    ;

func_name
    : func_meth_name_ident                        
        { $$ = new method_name(null,(ident)$1,null,@$); }
    | func_class_name_ident tkPoint func_meth_name_ident  
        { $$ = new method_name((ident)$1,(ident)$3,null,@$); }
    | func_class_name_ident tkPoint func_class_name_ident tkPoint func_meth_name_ident  
        { $$ = new method_name((ident)$1,(ident)$5,(ident)$3,@$); }
    ;

func_class_name_ident
    : func_name_with_template_args
		{ $$ = $1; }
    ;

func_meth_name_ident
    : func_name_with_template_args
		{ $$ = $1; }
    | operator_name_ident
		{ $$ = $1; }
    ;

func_name_with_template_args
    : func_name_ident
		{ $$ = $1; }
    | func_name_ident template_arguments  
        { 
			$$ = new template_type_name((ident_list)$2,@$); 
			$$.name=((ident)$1).name;
        }
	;
	
func_name_ident
    : identifier
		{ $$ = $1; }
    | visibility_specifier
		{ $$ = $1; }
    ;

func_heading
    : tkFunction func_name fp_list tkColon fptype opt_meth_modificators opt_where_section
        { 
			$$ = new function_header(); 
			$$.name = (method_name)$2;
			if ($$.name.meth_name is template_type_name)
			{
				$$.template_args=($$.name.meth_name as template_type_name).template_args;
				ident id = new ident($$.name.meth_name.name,$$.name.meth_name.source_context);
				$$.name.meth_name=id;
			}
			//$$.template_args=(ident_list)$3;
			if ($3!=null) 
			{
				$$.parameters = (formal_parameters)$3;
			}
			if ($5!=null) 
			{
				($$ as function_header).return_type=(type_definition)$5;
			}
			if ($6!=null) 
			{
				if (((procedure_attributes_list)$6).proc_attributes.Count>0) 
					$$.proc_attributes = (procedure_attributes_list)$6;
			}
			if ($7!=null) 
			{
				$$.where_defs = (where_definition_list)$7;
			}
			$$.of_object = false;
			$$.class_keyword = false;
			$$.source_context = @$;
        }
    |  tkFunction func_name opt_meth_modificators             
        { 
			$$ = new function_header(); 
			$$.name=(method_name)$2;
			if ($3!=null) 
			{
				if (((procedure_attributes_list)$3).proc_attributes.Count>0) 
					$$.proc_attributes = (procedure_attributes_list)$3;
			}
			$$.of_object = false;
			$$.class_keyword = false;
			$$.source_context = @$;
        }
    ;

proc_block
    : proc_block_decl
		{ $$ = $1; }
    ;

func_block
    : proc_block_decl
		{ $$ = $1; }
    ;

proc_block_decl
    : block
		{ $$ = $1; }
    | external_directr
		{ $$ = $1; }
    | tkForward tkSemiColon                       
        { $$ = $1; }
    ;

abc_proc_block
    : abc_block
		{ $$ = $1; }
    | external_directr
		{ $$ = $1; }
    ;

external_directr
    : abc_external_directr
		{ $$ = $1; }
    | abc_external_directr tkSemiColon          
        { $$ = $1; }
    | tkExternal tkSemiColon
        { 
			$$ = new external_directive(null,null,@$); 
		}
    ;

external_directr_ident
    : identifier
		{ $$ = $1; }
    | literal
		{ $$ = $1; }
    ;

abc_external_directr
    : tkExternal external_directr_ident tkName external_directr_ident
        { 
			$$ = new external_directive((expression)$2,(expression)$4,@$); 
		}
    | tkExternal external_directr_ident
        { 
			$$ = new external_directive((expression)$2,null,@$); 
		}
    ;

block
    : impl_decl_sect_list compound_stmt tkSemiColon    
        { 
			$$ = new block((declarations)$1,(statement_list)$2,@$); 
		}
    ;

abc_block
    : abc_decl_sect_list compound_stmt tkSemiColon 
        { 
			$$ = new block((declarations)$1,(statement_list)$2,@$); 
		}
    ;

fp_list
    :
		{ $$ = null; }
    |  tkRoundOpen fp_sect_list tkRoundClose         
        { 
			if($2!=null) 
				$2.source_context = @$;
			$$=$2;
		}
    ;
                                                        
fp_sect_list
    :
		{ $$ = null; }
    | fp_sect                                          
        { 
			$$ = new formal_parameters(); 
			$$.params_list.Add((typed_parameters)$1);
			$$.source_context = @$;
        }
    | fp_sect_list tkSemiColon fp_sect               
        { 
			$$ = (formal_parameters)$1;
			$$.params_list.Add((typed_parameters)$3);
			$$.source_context = @$;
        } 
    ;

fp_sect
    : opt_attribute_declarations simple_fp_sect
        {  
		    $$=$2 as declaration;
            $$.attributes = $1 as attribute_list;
        }
    ;
    
simple_fp_sect
    : param_name_list tkColon fptype_new             
        { $$ = new typed_parameters((ident_list)$1,(type_definition)$3,parametr_kind.none,null,@$); }
    | tkVar param_name_list tkColon fptype_new       
        { 
			$$ = new typed_parameters((ident_list)$2,(type_definition)$4,parametr_kind.var_parametr,null,@$);  
		}
    | tkOut param_name_list tkColon fptype_new       
        { $$ = new typed_parameters((ident_list)$2,(type_definition)$4,parametr_kind.out_parametr,null,@$); }
    | tkConst param_name_list tkColon fptype_new     
        { $$ = new typed_parameters((ident_list)$2,(type_definition)$4,parametr_kind.const_parametr,null,@$); }
    | tkParams param_name_list tkColon fptype_new    
        { $$ = new typed_parameters((ident_list)$2,(type_definition)$4,parametr_kind.params_parametr,null,@$); }
    | param_name_list tkColon fptype tkAssign const_expr        
        { $$ = new typed_parameters((ident_list)$1,(type_definition)$3,parametr_kind.none,(expression)$5,@$); }
    | tkVar param_name_list tkColon fptype tkAssign const_expr  
        { $$ = new typed_parameters((ident_list)$2,(type_definition)$4,parametr_kind.var_parametr,(expression)$6,@$); }
    | tkOut param_name_list tkColon fptype tkAssign const_expr      
        { $$ = new typed_parameters((ident_list)$2,(type_definition)$4,parametr_kind.out_parametr,(expression)$6,@$); }
    | tkConst param_name_list tkColon fptype tkAssign const_expr    
        { $$ = new typed_parameters((ident_list)$2,(type_definition)$4,parametr_kind.const_parametr,(expression)$6,@$); }
    ;

param_name_list
    : param_name                                       
        { 
			$$ = new ident_list();  
			$$.idents.Add($1); 
			$$.source_context = @$;
		}
    |  param_name_list tkComma param_name      
        { 
			$$ = $1; 
			$$.idents.Add($3); 
			$$.source_context = @$;
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

fptype_new
    : type_ref
		{ $$ = $1; }
    | tkArray tkOf tkConst                               
        { 
			$$ = new array_of_const_type_definition();  
			$$.source_context = @$;
		}
    ;

stmt
    : unlabelled_stmt  
		{ $$ = $1;}
    | label_name tkColon stmt         
        { 
			$$ = new labeled_statement((ident)$1,(statement)$3,@$);  
		}
    ;

unlabelled_stmt
    :                                        
        { $$ = new empty_statement(); }
    | assignment 
		{ $$ = $1;}
    | proc_call
		{ $$ = $1;}
    | goto_stmt
		{ $$ = $1;}
    | compound_stmt
		{ $$ = $1;}
    | if_stmt
		{ $$ = $1;}
    | case_stmt
		{ $$ = $1;}
    | repeat_stmt
		{ $$ = $1;}
    | while_stmt
		{ $$ = $1;}
    | for_stmt
		{ $$ = $1;}
    | with_stmt
		{ $$ = $1;}
    | inherited_message
		{ $$ = $1;}
    | try_stmt
		{ $$ = $1;}
    | raise_stmt
		{ $$ = $1;}
    | foreach_stmt
		{ $$ = $1;}
    | var_stmt
		{ $$ = $1;}
    | expr_as_stmt
		{ $$ = $1;}
    | lock_stmt
		{ $$ = $1;}
    ;
var_stmt
    : tkVar var_decl_part_in_stmt        
        { 
			$$ = new var_statement($2 as var_def_statement,@$); 
			///////////////tasha 28.04.2010
			parsertools.pascalABC_var_statements.Add((var_def_statement)$2);
			///////////////////////////////
		}
    ;

assignment
    : var_reference assign_operator expr           
        {
			///////////////tasha 28.04.2010
			parsertools.for_assignment($1, $3);
			///////////////////////////////
			$$ = new assign($1 as addressed_value,$3 as expression,((op_type_node)$2).type,@$);
        }
    ;
        
proc_call
    : var_reference                                    
        { $$ = new procedure_call($1 as addressed_value,@$); }
    ;

goto_stmt
    : tkGoto label_name              
        { 
			$$ = new goto_statement((ident)$2,@$); 
		}
    ;

compound_stmt                                             
    : tkBegin stmt_list tkEnd                
        {
			$2.source_context = @$;
			((statement_list)$2).left_logical_bracket = (syntax_tree_node)$1;
			((statement_list)$2).right_logical_bracket = (syntax_tree_node)$3;
			$$ = $2;
        }
    ;

stmt_list
    : stmt                                             
        { 
			$$ = new statement_list(); 
			$$.subnodes.Add((statement)$1);
			$$.source_context = @$;
        }
    |  stmt_list tkSemiColon stmt                     
        {  
			$$ = (statement_list)$1;
			if($$!=$3) 
				$$.subnodes.Add((statement)$3);
			$$.source_context = @$;
        }
    ;

if_stmt
    : tkIf expr if_then_else_branch                  
        {
		    ((if_node)$3).condition = (expression)$2;
			$$ = $3;
			$$.source_context = @$;
        }
    ;

if_then_else_branch
    : tkThen then_branch                             
        { 
			$$ = new if_node(null,(statement)$2,null,@$); 
        }
    |  tkThen then_branch tkElse else_branch          
        { 
			$$ = new if_node(null,(statement)$2,(statement)$4,@$); 
        }
    ;

then_branch
    : stmt
		{ $$ = $1;}
    ;

else_branch
    : stmt
		{ $$ = $1;}
    ;

case_stmt
    : tkCase expr tkOf case_list else_case tkEnd 
        { 
			$$ = new case_node((expression)$2,$4 as case_variants,$5 as statement,@$); 
		}
    ;

case_list
    : case_item                                
        { 
			$$ = new case_variants(); 
			if ($1 is case_variant)
			{
				$$.variants.Add((case_variant)$1);
			}
			$$.source_context = @$;
		}
    |  case_list tkSemiColon case_item         
        { 
			$$ = (case_variants)$1;
			if ($3 is case_variant) 
				$$.variants.Add((case_variant)$3);
			$$.source_context = @$;
		} 
    ;

case_item
    :                                     
        { $$ = new empty_statement(); }
    |  case_label_list tkColon stmt            
        { $$ = new case_variant((expression_list)$1,(statement)$3,@$); }
    ;

case_label_list
    : case_label                               
        { 
			$$ = new expression_list();  
			$$.expressions.Add($1); 
			$$.source_context = @$;
		}
    |  case_label_list tkComma case_label      
        { 
			$$ = $1; 
			$$.expressions.Add($3); 
			$$.source_context = @$;
		}
    ;

case_label
    : const_elem
		{ $$ = $1;}
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
			$$ = new repeat_node((statement)$2,(expression)$4); 
			((statement_list)$2).left_logical_bracket=(syntax_tree_node)$1;
			((statement_list)$2).right_logical_bracket=(syntax_tree_node)$3;
			$2.source_context = @1.Merge(@3);
			$$.source_context = @$;
        }
	;
	
while_stmt
    : tkWhile expr opt_tk_do stmt                        
        { 
			$$ = new while_node((expression)$2,(statement)$4,WhileCycleType.While,@$); 
			if ($3 == null)
			{
				file_position fp = ($2 as syntax_tree_node).source_context.end_position;
				syntax_tree_node err_stn = (syntax_tree_node)$4;
				if (err_stn == null)
					err_stn = (syntax_tree_node)$2;
				parsertools.errors.Add(new PABCNETUnexpectedToken(current_file_name, StringResources.Get("TKDO"),new SourceContext(fp.line_num, fp.column_num+1, fp.line_num, fp.column_num+1, 0, 0),err_stn));
			}
        }
    ;

opt_tk_do
    : tkDo
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;
        
lock_stmt
    : tkLock expr tkDo stmt            
        { $$ = new lock_stmt((expression)$2,(statement)$4,@$); }
	;
	
foreach_stmt
    : tkForeach identifier foreach_stmt_ident_dype_opt tkIn expr tkDo stmt
        { $$ = new foreach_stmt((ident)$2,(type_definition)$3,(expression)$5,(statement)$7,@$); }
    | tkForeach tkVar identifier tkColon type_ref tkIn expr tkDo stmt
        { $$ = new foreach_stmt((ident)$3,(type_definition)$5,(expression)$7,(statement)$9,@$); }
    ;

foreach_stmt_ident_dype_opt
    : tkColon type_ref                   
        { $$ = $2; }
    |
    ;
           
for_stmt
    : tkFor opt_var identifier for_stmt_decl_or_assign expr for_cycle_type expr opt_tk_do stmt
        { 
			$$ = new for_node((ident)$3,(expression)$5,(expression)$7,(statement)$9,(for_cycle_type)$6,null,$4 as type_definition, $2!=false,@$); 
			if ($8 == null)
			{
				file_position fp = ($7 as syntax_tree_node).source_context.end_position;
				syntax_tree_node err_stn = (syntax_tree_node)$9;
				if (err_stn == null)
					err_stn = (syntax_tree_node)$7;
				parsertools.errors.Add(new PABCNETUnexpectedToken(current_file_name, StringResources.Get("TKDO"),new SourceContext(fp.line_num, fp.column_num+1, fp.line_num, fp.column_num+1, 0, 0),err_stn));
			}
        }
	;
	
opt_var
    : tkVar
        { $$ = true; }
    |	{ $$ = false; }
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
    : tkWith expr_list tkDo stmt               
        { $$ = new with_statement((statement)$4,(expression_list)$2,@$); }  
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
			$$ = new try_stmt(((statement_list)$2),(try_handler)$3,@$); 
			((statement_list)$2).left_logical_bracket=(syntax_tree_node)$1;
			$2.source_context = @1.Merge(@2);
        } 
    ;

try_handler
    : tkFinally stmt_list tkEnd          
        { 
			$$ = new try_handler_finally((statement_list)$2,@$); 
			((statement_list)$2).left_logical_bracket = (syntax_tree_node)$1;
			((statement_list)$2).right_logical_bracket = (syntax_tree_node)$3;
		}
    | tkExcept exception_block tkEnd    
        { 
			$$ = new try_handler_except((exception_block)$2,@$);  
		}
    ;

exception_block
    : exception_handler_list exception_block_else_branch       
        { 
			$$ = new exception_block(null,(exception_handler_list)$1,(statement_list)$2,@$); 
		}
    |  exception_handler_list tkSemiColon exception_block_else_branch 
        { 
			$$ = new exception_block(null,(exception_handler_list)$1,(statement_list)$3,@$); 
		}
    |  stmt_list                          
        { 
			$$ = new exception_block((statement_list)$1,null,null,@$); 
		}
    ;

exception_handler_list
    : exception_handler                                   
        { 
			$$ = new exception_handler_list();  
			$$.handlers.Add($1); 
			$$.source_context = @$;
		}
    | exception_handler_list tkSemiColon exception_handler   
        { 
			$$ = $1; 
			$$.handlers.Add($3); 
			$$.source_context = @$;
		}
    ;

exception_block_else_branch
    :
        { $$ = null; }
    |  tkElse stmt_list                      
        { $$ = $2; }
    ;

exception_handler
    : tkOn exception_identifier tkDo stmt            
        { 
			$$ = new exception_handler(((exception_ident)$2).variable,((exception_ident)$2).type_name,(statement)$4,@$); 
		}
    ;

exception_identifier
    : exception_class_type_identifier                         
        { $$ = new exception_ident(null,(named_type_reference)$1,@$); }
    |  exception_variable tkColon exception_class_type_identifier  
        { $$ = new exception_ident((ident)$1,(named_type_reference)$3,@$); }
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
    |  tkRaise expr                             
        { $$ = new raise_stmt((expression)$2,null,@$); }
    |  tkRaise expr tkAt expr                 
        { $$ = new raise_stmt((expression)$2,(expression)$4,@$); }
    ;

expr_list
    : expr                                
        { 
			$$ = new expression_list();  
			$$.expressions.Add($1); 
			$$.source_context = @$;
		}
    |  expr_list tkComma expr              
        { 
			$$ = $1; 
			$$.expressions.Add($3); 
			$$.source_context = @$;
		}
    ;

expr_as_stmt
    : allowable_expr_as_stmt      
        { $$ = new expression_as_statement((expression)$1,@$); }
    ;

allowable_expr_as_stmt
    : new_expr
		{ $$ = $1; }
    ;

expr
    : expr_l1
		{ $$ = $1; }
    | format_expr
		{ $$ = $1; }
    | func_decl_lambda
        { $$ = $1; }
    ;

expr_l1
    : relop_expr
		{ $$ = $1; }
    | question_expr
		{ $$ = $1; }
    ;

sizeof_expr
    : tkSizeOf tkRoundOpen simple_or_template_type_reference tkRoundClose
        { $$ = new sizeof_operator((named_type_reference)$3,null,@$); }
    ;

typeof_expr
    : tkTypeOf tkRoundOpen simple_or_template_type_reference tkRoundClose
        { $$ = new typeof_operator((named_type_reference)$3,@$); }
    ;

question_expr
    : expr_l1 tkQuestion expr_l1 tkColon expr_l1 
        { $$ = new question_colon_expression((expression)$1,(expression)$3,(expression)$5,@$); }
    ;

simple_or_template_type_reference
    : simple_type_identifier
		{ $$ = $1;}
    |  simple_type_identifier template_type_params
        { $$ = new template_type_reference((named_type_reference)$1,(template_param_list)$2,@$); }
    |  simple_type_identifier tkAmpersend template_type_params
        { $$ = new template_type_reference((named_type_reference)$1,(template_param_list)$3,@$); }
    ;

opt_array_initializer
    : tkRoundOpen typed_const_list tkRoundClose
        { $$ = new array_const((expression_list)$2,@$); }
    |
    ;
            
new_expr
    : identifier simple_or_template_type_reference opt_expr_list_with_bracket
        {
			$$ = new new_expr((named_type_reference)$2,$3 as expression_list,false,null,@$);
			if (($1 as ident).name.ToLower()!="new")
				parsertools.errors.Add(new PABCNETUnexpectedToken(current_file_name,";",((syntax_tree_node)$1).source_context,$$));
        }
    | identifier array_name_for_new_expr tkSquareOpen expr_list tkSquareClose opt_array_initializer
        {
			$$ = new new_expr((type_definition)$2,$4 as expression_list,true,$6 as array_const,@$);
			if (($1 as ident).name.ToLower()!="new")
				parsertools.errors.Add(new PABCNETUnexpectedToken(current_file_name,";",((syntax_tree_node)$1).source_context,$$));
        }
    ;

array_name_for_new_expr
    : simple_type_identifier 
		{ $$ = $1; }
    | unsized_array_type 
		{ $$ = $1; }
    ;

opt_expr_list_with_bracket
    :
		{ $$ = null; }
    |  tkRoundOpen opt_expr_list tkRoundClose    
        { $$ = $2; }
    ;

relop_expr
    : simple_expr
		{ $$ = $1; }
    | simple_expr relop relop_expr              
        { $$ = new bin_expr((expression)$1,(expression)$3,((op_type_node)$2).type,@$); }
    ;

format_expr 
    : simple_expr tkColon simple_expr                        
        { $$ = new format_expr((expression)$1,(expression)$3,null,@$); }
    | simple_expr tkColon simple_expr tkColon simple_expr   
        { $$ = new format_expr((expression)$1,(expression)$3,(expression)$5,@$); }
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
    : term
		{ $$ = $1; }
    | simple_expr addop term                        
        { $$ = new bin_expr($1 as expression,$3 as expression,((op_type_node)$2).type,@$); }
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
        { $$ = op_typecast.as_op; }
    |  tkIs                                        
        { $$ = op_typecast.is_op; }
    ;

as_is_expr
    : term typecast_op simple_or_template_type_reference     
        { 
			$$ = new typecast_node((addressed_value)$1,(type_definition)$3,(op_typecast)$2,@$); 
			if (!($1 is addressed_value)) 
				parsertools.errors.Add(new bad_operand_type(current_file_name,((syntax_tree_node)$1).source_context,$$));
        }
	;

term
    : factor
		{ $$ = $1; }
    | new_expr
		{ $$ = $1; }
    | term mulop factor                             
        { $$ = new bin_expr($1 as expression,$3 as expression,((op_type_node)$2).type,@$); }
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
        { $$ = new default_operator($3 as named_type_reference,@$); }
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
        { $$ = new pascal_set_constant($2 as expression_list,@$); }
    | tkNot factor              
        { $$ = new un_expr($2 as expression,((op_type_node)$1).type,@$); }
    | sign factor             
        { $$ = new un_expr($2 as expression,((op_type_node)$1).type,@$); }
    | tkDeref factor                
        { 
			$$ = new roof_dereference(); 
			($$ as roof_dereference).dereferencing_value=(addressed_value)$2;
			$$.source_context = @$; 
		}
    | var_reference
		{ $$ = $1; }
    | tkRoundOpen func_decl_lambda tkRoundClose tkRoundOpen expr_list tkRoundClose
        { 
			function_lambda_definition fld = parsertools.find_pascalABC_lambda_name(((ident)$2).name);
			$$ = new function_lambda_call(fld, (expression_list)$5,@$);
		}
    ;
      
literal_or_number
    : literal
		{ $$ = $1; }
    | unsigned_number
		{ $$ = $1; }
    ;

var_reference
    : var_address variable                   
        {
			((get_address)$1).address_of = (addressed_value)$2;
			$$ = (addressed_value)parsertools.NodesStack.Pop();
			$$.source_context = @$; 
		}
    |  variable {$$ = $1;}
    ;
 
var_address
    : tkAddressOf                                
        { 
			$$ = new get_address(); 
			parsertools.NodesStack.Push($$);
			$$.source_context = @$; 
		}
    |  var_address tkAddressOf                  
        { 
			$$ = new get_address(); 
			((get_address)$1).address_of = (addressed_value)$$;
			$$.source_context = @$; 
		}
    ;

attribute_variable
    : simple_type_identifier opt_expr_list_with_bracket
        { 
			$$ = new attribute(null,(named_type_reference)$1,(expression_list)$2,@$); 
		}
    ;
    
variable
    : identifier 
		{ $$ = $1; }
    | operator_name_ident
		{ $$ = $1; }
    | tkInherited identifier            
        { 
			$$ = new inherited_ident(); 
			($$ as inherited_ident).name=((ident)$2).name; 
			$$.source_context = @$; 
		}
    | tkRoundOpen expr tkRoundClose         
        {
			if (!parsertools.build_tree_for_brackets) 
			{ 
				$2.source_context = @$; 
				$$ = $2;
			} 
			else
			{
				$$ = new bracket_expr($2 as expression,@$);
			}
        }
    | sizeof_expr
		{ $$ = $1; }
    | typeof_expr
		{ $$ = $1; }
    | tkRoundOpen tkRoundClose
    | literal_or_number tkPoint identifier_or_keyword
        { $$ = new dot_node((addressed_value)$1,(addressed_value)$3,@$); }
    | variable var_specifiers                
        {
			if ($2 is dot_node) 
			{
				((dot_node)$2).left = (addressed_value)$1;
			}
			else
			if ($2 is template_param_list) 
			{
				((dot_node)(((template_param_list)$2).dereferencing_value)).left = (addressed_value)$1;
				((template_param_list)$2).dereferencing_value.source_context = @$;
			}
			else
			if ($2 is dereference) 
			{
				((dereference)$2).dereferencing_value = (addressed_value)$1;
			}
			else
			if ($2 is ident_with_templateparams) 
			{
				((ident_with_templateparams)$2).name = (addressed_value_funcname)$1;
			}
				$2.source_context = @$; 
			$$ = $2;
        }
    ;

opt_expr_list
    : expr_list
		{ $$ = $1; }
    |
		{ $$ = null; }
    ;

var_specifiers
    : tkSquareOpen expr_list tkSquareClose     
        { $$ = new indexer((expression_list)$2,@$); }
    | tkSquareOpen tkSquareClose
    | tkRoundOpen opt_expr_list tkRoundClose
        { $$ = new method_call($2 as expression_list,@$); }
    | tkPoint identifier_keyword_operatorname   
        { $$ = new dot_node(null,(addressed_value)$2,@$); }
    | tkDeref                             
        { 
			$$ = new roof_dereference();  
			$$.source_context = @$;
		}
    | tkAmpersend template_type_params         
        { $$ = new ident_with_templateparams(null,(template_param_list)$2,@$); }
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
			$$ = new expression_list();  
			$$.expressions.Add($1); 
			$$.source_context = @$;
		}
    | elem_list1 tkComma elem     
        { 
			$$ = $1; 
			$$.expressions.Add($3); 
			$$.source_context = @$;
		}
    ;

elem
    : expr
		{ $$ = $1; }
    | expr tkDotDot expr          
        { $$ = new diapason_expr((expression)$1,(expression)$3,@$); }
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
			literal_const_line lcl=(literal_const_line)$1;
			if (lcl.literals.Count==1) 
				$$=lcl.literals[0];
			$$.source_context = @$;
        }
	;
	
literal_list
    : one_literal                 
        { 
			$$ = new literal_const_line(); 
			$$.literals.Add((literal)$1);
			$$.source_context = @$;
        }
    |  literal_list one_literal    
        { 
			$$ = (literal_const_line)$1;
			$$.literals.Add((literal)$2);
			$$.source_context = @$;
        } 
    ;

operator_name_ident
    : tkOperator overload_operator           
        { 
			$$ = new operator_name_ident(((op_type_node)$2).type,@$); 
			$$.name=((op_type_node)$2).text;
		}
	;
	
opt_meth_modificators
    : tkSemiColon                                        
        { 
			$$ = new procedure_attributes_list();  
			parsertools.AddModifier($$,proc_attribute.attr_overload);  
			$$.source_context = @$;
		}
    |  tkSemiColon meth_modificators tkSemiColon     
        { 
			parsertools.AddModifier((procedure_attributes_list)$2,proc_attribute.attr_overload); 
			$$ = $2; 
		}
    ;

meth_modificators
    : meth_modificator                
        { 
			$$ = new procedure_attributes_list();  
			$$.proc_attributes.Add($1); 
			$$.source_context = @$;
		}
    |  meth_modificators tkSemiColon meth_modificator  
        { 
			$$ = $1; 
			$$.proc_attributes.Add($3); 
			$$.source_context = @$;
		}
    ;

identifier
    : tkIdentifier 
		{$$ = $1;}
    | real_type_name
		{ $$ = $1; }
    | ord_type_name
		{ $$ = $1; }
    | variant_type_name
		{ $$ = $1; }
    | meth_modificator
		{ $$ = $1; }
    | property_specifier_directives
		{ $$ = $1; }
    | non_reserved
		{ $$ = $1; }
    | other
		{ $$ = $1; }
    ;

identifier_or_keyword
    : identifier
		{ $$ = $1; }
    | keyword                    
        { $$ = new ident(($1 as token_info).text,@$); }
    | reserved_keyword                   
        { $$ = new ident(($1 as token_info).text,@$); }
    ;

identifier_keyword_operatorname
    : identifier
		{ $$ = $1; }
    | keyword                    
        { $$ = new ident(($1 as token_info).text,@$); }
    | operator_name_ident
		{ $$ = $1; }
    ;

real_type_name
    : tkReal
		{ $$ = $1; }
    | tkSingle
		{ $$ = $1; }
    | tkDouble
		{ $$ = $1; }
    | tkExtended
		{ $$ = $1; }
    | tkComp
		{ $$ = $1; }
    ;

ord_type_name
    : tkShortInt
		{ $$ = $1; }
    | tkSmallInt
		{ $$ = $1; }
    | tkOrdInteger
		{ $$ = $1; }
    | tkByte
		{ $$ = $1; }
    | tkLongInt
		{ $$ = $1; }
    | tkInt64
		{ $$ = $1; }
    | tkWord
		{ $$ = $1; }
    | tkBoolean
		{ $$ = $1; }
    | tkChar
		{ $$ = $1; }
    | tkWideChar
		{ $$ = $1; }
    | tkLongWord
		{ $$ = $1; }
    | tkPChar
		{ $$ = $1; }
    | tkCardinal
		{ $$ = $1; }
    ;

variant_type_name
    : tkVariant
		{ $$ = $1; }
    | tkOleVariant
		{ $$ = $1; }
    ;

meth_modificator
    : tkAbstract
		{ $$ = $1; }
    | tkOverload
		{ $$ = $1; }
    | tkReintroduce
		{ $$ = $1; }
    | tkOverride
		{ $$ = $1; }
    | tkVirtual
		{ $$ = $1; }
    | tkStatic
		{ $$ = $1; }
    ;

property_specifier_directives
    : tkRead
		{ $$ = $1; }
    | tkWrite
		{ $$ = $1; }
    | tkStored
		{ $$ = $1; }
    | tkNodefault
		{ $$ = $1; }
    | tkImplements
		{ $$ = $1; }
    | tkWriteOnly
		{ $$ = $1; }
    | tkReadOnly
		{ $$ = $1; }
    | tkDispid
		{ $$ = $1; }
    ;

non_reserved
    : tkAt
		{ $$ = $1; }
    | tkAbsolute
		{ $$ = $1; }
    | tkOn
		{ $$ = $1; }
    | tkName
		{ $$ = $1; }
    | tkIndex
		{ $$ = $1; }
    | tkMessage
		{ $$ = $1; }
    | tkContains
		{ $$ = $1; }
    | tkRequires
		{ $$ = $1; }
    | tkForward
		{ $$ = $1; }
    | tkOut
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

other
    : tkPackage
		{ $$ = $1; }
    | tkUnit
		{ $$ = $1; }
    | tkLibrary
		{ $$ = $1; }
    | tkExternal
		{ $$ = $1; }
    | tkParams
		{ $$ = $1; }
    ;

keyword
    : visibility_specifier 
        { $$ = new token_info(($1 as ident).name,@$); }
    |  tkFinal
		{ $$ = $1; }
    |  tkTemplate
		{ $$ = $1; }
    |  tkOr
		{ $$ = $1; }
    |  tkTypeOf
		{ $$ = $1; }
    |  tkSizeOf
		{ $$ = $1; }
    |  tkDefault
		{ $$ = $1; }
    |  tkWhere
		{ $$ = $1; }
    |  tkXor
		{ $$ = $1; }
    |  tkAnd
		{ $$ = $1; }
    |  tkDiv
		{ $$ = $1; }
    |  tkMod
		{ $$ = $1; }
    |  tkShl
		{ $$ = $1; }
    |  tkShr
		{ $$ = $1; }
    |  tkNot
		{ $$ = $1; }
    |  tkAs
		{ $$ = $1; }
    |  tkIn
		{ $$ = $1; }
    |  tkIs
		{ $$ = $1; }
    |  tkArray
		{ $$ = $1; }
    |  tkBegin
		{ $$ = $1; }
    |  tkCase
		{ $$ = $1; }
    |  tkClass
		{ $$ = $1; }
    |  tkConst
		{ $$ = $1; }
    |  tkConstructor
		{ $$ = $1; }
    |  tkDestructor
		{ $$ = $1; }
    |  tkDownto
		{ $$ = $1; }
    |  tkDo
		{ $$ = $1; }
    |  tkElse
		{ $$ = $1; }
    |  tkExcept
		{ $$ = $1; }
    |  tkFile
		{ $$ = $1; }
    |  tkFinalization
		{ $$ = $1; }
    |  tkFinally
		{ $$ = $1; }
    |  tkFor
		{ $$ = $1; }
    |  tkForeach
		{ $$ = $1; }
    |  tkFunction
		{ $$ = $1; }
    |  tkIf
		{ $$ = $1; }
    |  tkImplementation
		{ $$ = $1; }
    |  tkInherited
		{ $$ = $1; }
    |  tkInitialization
		{ $$ = $1; }
    |  tkInterface
		{ $$ = $1; }
    |  tkProcedure
		{ $$ = $1; }
    |  tkProperty
		{ $$ = $1; }
    |  tkRaise
		{ $$ = $1; }
    |  tkRecord
		{ $$ = $1; }
    |  tkRepeat
		{ $$ = $1; }
    |  tkSet
		{ $$ = $1; }
    |  tkTry
		{ $$ = $1; }
    |  tkType
		{ $$ = $1; }
    |  tkThen
		{ $$ = $1; }
    |  tkTo
		{ $$ = $1; }
    |  tkUntil
		{ $$ = $1; }
    |  tkUses
		{ $$ = $1; }
    |  tkUsing
		{ $$ = $1; }
    |  tkVar
		{ $$ = $1; }
    |  tkWhile
		{ $$ = $1; }
    |  tkWith
		{ $$ = $1; }
    |  tkNil
		{ $$ = $1; }
    |  tkGoto
		{ $$ = $1; }
    |  tkOf
		{ $$ = $1; }
    |  tkLabel
		{ $$ = $1; }
    |  tkProgram
		{ $$ = $1; }
    ;

reserved_keyword
    : tkOperator
		{ $$ = $1; }
    | tkEnd
		{ $$ = $1; }
    ;

overload_operator
    : tkMinus
		{ $$ = $1; }
    | tkPlus
		{ $$ = $1; }
    | tkSquareOpen tkSquareClose
    | tkRoundOpen tkRoundClose
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
    | tkAddressOf
		{ $$ = $1; }
    | tkDeref
		{ $$ = $1; }
    | tkImplicit
		{ $$ = $1; }
    | tkExplicit
		{ $$ = $1; }
    | assign_operator
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

func_decl_lambda
    : ident_list1 tkArrow lambda_body
        { $$ = parsertools.func_decl_lambda($1, $3); }
    | tkArrow lambda_body               
        { $$ = parsertools.func_decl_lambda(null, $2); }
    | tkRoundOpen tkRoundClose tkArrow lambda_body 
        { $$ = parsertools.func_decl_lambda(null, $4); }
    ;

ident_list1
    : tkRoundOpen identifier tkComma ident_list2 tkRoundClose
        { $$ = parsertools.ident_list11($2, $4); }
    | identifier  
        { $$ = parsertools.ident_list12($1); }
    | tkRoundOpen identifier tkColon fptype tkComma ident_list2 tkRoundClose
        { $$ = parsertools.ident_list13($2, $4, $6); }
    | tkRoundOpen identifier tkColon fptype tkRoundClose 
        { $$ = parsertools.ident_list14($2, $4); }
    ;

ident_list2
    : ident_list2 tkComma var_or_identifier 
        { $$ = parsertools.ident_list21($1, $3); }
    | var_or_identifier                
        { $$ = parsertools.ident_list12($1); }
    ;

var_or_identifier
    : identifier                   
        { $$ = $1; }
    | identifier tkColon fptype  
        {
			var vds = new var_def_statement();
			vds.vars = new ident_list();
			vds.vars.idents.Add((ident)$1);
			vds.vars_type = (named_type_reference)$3;
			$$ = vds;
			$$.source_context = @$;
        }
    ;

lambda_body
    : expr_l1 
        {
			$$ = new statement_list();
			ident id = new ident("result");
			op_type_node _op_type_node = new op_type_node(Operators.Assignment);
			assign _assign = new assign((addressed_value)id, $1 as expression, _op_type_node.type);
			parsertools.create_source_context(_assign, id, $1);
			($$ as statement_list).subnodes.Add((statement)_assign);
			$$.source_context = @$;
			//block _block = new block(null, _statement_list);
		}
    | compound_stmt 
        { $$ = $1; }
    ;

%%


