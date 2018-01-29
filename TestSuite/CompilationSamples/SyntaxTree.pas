unit SyntaxTree;

interface

uses System.Collections.Generic;

type
   file_position = class
   private
    function get_column_num: Integer;
    function get_line_num: Integer;
    
    // Methods
    public constructor Create(line_num: Integer; column_num: Integer); 

    // Properties
    public property column_num: Integer read get_column_num;
    public property line_num: Integer read get_line_num;

    // Fields
    private _column_num: Integer;
    private _line_num: Integer;

  end;

   SourceContext = class
   private
    function get_begin_position: file_position;
    function get_end_position: file_position;
    function get_FileName: string;
    procedure set_FileName(value: string);
    function get_LeftSourceContext: SourceContext;
    function get_Length: Integer;
    function get_Position: Integer;
    function get_RightSourceContext: SourceContext;

    // Methods
    public constructor Create(left: SourceContext; right: SourceContext); 
    public constructor Create(beg_line_num: Integer; beg_column_num: Integer; end_line_num: Integer; end_column_num: Integer); 
    public constructor Create(beg_line_num: Integer; beg_column_num: Integer; end_line_num: Integer; end_column_num: Integer; _begin_symbol_position: Integer; _end_symbol_position: Integer); 
    public function &In(sc: SourceContext): boolean; 
    public function ToString: string; override; 

    // Properties
    public property begin_position: file_position read get_begin_position;
    public property end_position: file_position read get_end_position;
    public property FileName: string read get_FileName write set_FileName;
    public property LeftSourceContext: SourceContext read get_LeftSourceContext;
    public property Length: Integer read get_Length;
    public property Position: Integer read get_Position;
    public property RightSourceContext: SourceContext read get_RightSourceContext;

    // Fields
    private _begin_position: file_position;
    private _begin_symbol_position: Integer;
    private _end_position: file_position;
    private _end_symbol_position: Integer;
    private _file_name: string;

  end;
  for_cycle_type = (fct_to,fct_downto);
  access_modifer = (public_modifer, protected_modifer, private_modifer, published_modifer, internal_modifer, none);
  c_scalar_sign = (ssNone, ssSigned, ssUnsigned);
  c_scalar_type_name = ( tn_char, tn_int, tn_short, tn_long, tn_short_int, tn_long_int, tn_float, tn_double, tn_void);
  class_keyword = ( ckClass, ckInterface, ckRecord, ckStruct, ckUnion, ckTemplateClass, ckTemplateRecord);
  class_attribute = ( caNone, caSealed);
  parametr_kind = (pk_none, pk_var_parametr, pk_const_parametr, pk_out_parametr, pk_params_parametr );
  type_definition_attribute = ( tdaNone, tdaStatic, tdaExtern, tdaRegister, tdaAuto, tdaVolatile, tdaConst, tdaInline);
  known_type = (string_type,none_type);
  proc_attribute = (attr_stdcall,attr_override,attr_forward,attr_far,attr_virtual,attr_dynamic,attr_cdecl,
		attr_overload,attr_pascal,attr_register,attr_reintroduce,attr_external,attr_export,attr_near,attr_safecall,
		attr_abstract,attr_static);
	
	Operators =
	(                           // Pascal  C
		opUndefined,
        
        //Arithmetic operators
        opPlus,                   //  +       +
        opMinus,                  //  -       -
        opMultiplication,         //  *       *   
        opDivision,               //  /       /
        opModulusRemainder,       //  mod     %
        opAssignmentAddition,     //  +=      +=
        opAssignmentSubtraction,  //  -=      -=
        opAssignmentMultiplication,// *=      *= 
        opAssignmentDivision,     //  /=      /=
        opAssignmentModulus,      //          %=
        opPrefixIncrement,        //          ++()
        opPostfixIncrement, 	    //          ()++
        opPrefixDecrement,        //          --()
        opPostfixDecrement, 	    //          ()--
        
        //Comparison operators
        opLogicalAND,             //  and     &&
        opLogicalOR,              //  or      ||
        opIntegerDivision,        //  div     
        opLess,                   //  <       < 
        opGreater,                //  >       >
        opLessEqual,              //  <=      <= 
        opGreaterEqual,           //  >=      >=
        opEqual,                  //  =       ==
        opNotEqual,               //  <>      !=
        opLogicalNOT,        //  not     !
        
        //Bitwise operators
        opBitwiseLeftShift,       //  shl     <<                 
        opBitwiseRightShift,      //  shr     >>
        opBitwiseAND,             //          &
        opBitwiseOR,              //          |
        opBitwiseXOR,             //  xor     ^
        opBitwiseNOT,             //          ~
        opAssignmentBitwiseLeftShift,//       <<=
        opAssignmentBitwiseRightShift,//      >>=
        opAssignmentBitwiseAND,   //          &=
        opAssignmentBitwiseOR,    //          |=
        opAssignmentBitwiseXOR,   //          ^=

        //Other operators
        opAssignment,             //  :=      =                 
        opIn,                     //  in
        opIs,                     //  is
        opAs,                     //  as
        opDereference,            //  ^       *
        opAddressOf,              //  @       &
        opMemberByPointer,        //          ->
        opMember                  //  .       .
    );
  SwitchPartType = ( sptSwitch, sptCase, sptDefault );
  JumpStmtType = ( jstReturn, jstBreak, jstContinue );
  UnitHeaderKeyword = ( uhkUnit, uhkLibrary);
  WhileCycleType = ( wctWhile, wctDoWhile );
  op_typecast = ( is_op, as_op, typecast );
  oberon_export_marker = (export,export_readonly);
  LanguageId = ( PascalABCNET, C );
  
  IVisitor = class;
  
  
  access_modifer_node = class;
  addressed_value = class;
  addressed_value_funcname = class;
  array_const = class;
  array_of_const_type_definition = class;
  array_of_named_type_definition = class;
  array_size = class;
  array_type = class;
  assign = class;
  bin_expr = class;
  block = class;
  bool_const = class;
  c_for_cycle = class;
  c_module = class;
  c_scalar_type = class;
  case_node = class;
  case_variant = class;
  case_variants = class;
  char_const = class;
  class_body = class;
  class_definition = class;
  class_members = class;
  class_predefinition = class;
  compilation_unit = class;
  compiler_directive = class;
  compiler_directive_if = class;
  compiler_directive_list = class;
  const_definition = class;
  const_node = class;
  &constructor = class;
  consts_definitions_list = class;
  //declaration = class;
  declarations = class;
  declarations_as_statement = class;
  default_indexer_property_node = class;
  dereference = class;
  &destructor = class;
  diap_expr = class;
  diapason = class;
  diapason_expr = class;
  documentation_comment_list = class;
  documentation_comment_section = class;
  documentation_comment_tag = class;
  documentation_comment_tag_param = class;
  dot_node = class;
  double_const = class;
  empty_statement = class;
  enum_type_definition = class;
  enumerator = class;
  enumerator_list = class;
  exception_block = class;
  exception_handler = class;
  exception_handler_list = class;
  exception_ident = class;
  //expression = class;
  expression_as_statement = class;
  expression_list = class;
  external_directive = class;
  file_type = class;
  file_type_definition = class;
  for_node = class;
  foreach_stmt = class;
  formal_parametres = class;
  format_expr = class;
  function_header = class;
  get_address = class;
  goto_statement = class;
  hex_constant = class;
  ident = class;
  ident_list = class;
  if_node = class;
  implementation_node = class;
  index_property = class;
  indexer = class;
  indexers_types = class;
  inherited_ident = class;
  inherited_message = class;
  inherited_method_call = class;
  initfinal_part = class;
  int32_const = class;
  int64_const = class;
  interface_node = class;
  jump_stmt = class;
  known_type_definition = class;
  known_type_ident = class;

  label_definitions = class;

  labeled_statement = class;
  literal = class;

  literal_const_line = class;

  lock_stmt = class;

  loop_stmt = class;

  method_call = class;

  method_name = class;

  named_type_reference = class;

  named_type_reference_list = class;

  new_expr = class;

  nil_const = class;

  oberon_exit_stmt = class;

  oberon_ident_with_export_marker = class;

  oberon_import_module = class;

  oberon_module = class;

  oberon_procedure_header = class;

  oberon_procedure_receiver = class;

  oberon_withstmt = class;

  oberon_withstmt_guardstat = class;

  oberon_withstmt_guardstat_list = class;

  on_exception = class;

  on_exception_list = class;

  op_type_node = class;

  operator_name_ident = class;

  pascal_set_constant = class;

  proc_block = class;

  procedure_attribute = class;

  procedure_attributes_list = class;

  procedure_call = class;

  procedure_definition = class;

  procedure_header = class;

  program_body = class;

  program_module = class;

  program_name = class;

  program_tree = class;

  property_accessors = class;

  property_array_default = class;

  property_interface = class;

  property_parameter = class;

  property_parameter_list = class;

  question_colon_expression = class;

  raise_statement = class;

  raise_stmt = class;

  read_accessor_name = class;

  record_const = class;

  record_const_definition = class;

  record_type = class;

  record_type_parts = class;

  ref_type = class;

  repeat_node = class;

  roof_dereference = class;

  set_type_definition = class;

  sharp_char_const = class;

  simple_const_definition = class;

  simple_property = class;

  sizeof_operator = class;

  //statement = class;

  statement_list = class;

  string_const = class;

  string_num_definition = class;

  subprogram_body = class;
  
  switch_stmt = class;
  
  template_param_list = class;

  template_type_reference = class;
  token_info = class;

  token_taginfo = class;

  try_except_statement = class;

  try_finally_statement = class;

  try_handler = class;

  try_handler_except = class;

  try_handler_finally = class;

  try_statement = class;

 try_stmt = class;

 type_declaration = class;

 type_declarations = class;

 //type_definition = class;

 type_definition_attr = class;

 type_definition_attr_list = class;

 type_definition_list = class;

 typecast_node = class;

 typed_const_definition = class;

 typed_parametres = class;

 typeof_operator = class;

 uint64_const = class;

 un_expr = class;

 unit_module = class;

 unit_name = class;

 unit_or_namespace = class;

 uses_list = class;

 uses_unit_in = class;

 using_list = class;

 var_def_list = class;

 var_def_statement = class;

 var_statement = class;

 variable_definitions = class;

 variant = class;

 variant_list = class;

 variant_record_type = class;

 variant_type = class;

 variant_types = class;

 where_definition = class;

 where_definition_list = class;

 while_node = class;

 with_statement = class;

 write_accessor_name = class;

syntax_tree_node = class
    private _source_context: SourceContext;
    // Methods
    public constructor Create; 
    public constructor Create(_source_context: SourceContext); 
    public procedure visit(visitor: IVisitor); virtual; 

    // Properties
    public property source_context: SourceContext read _source_context write _source_context;

    // Fields
    

  end;
  
  declaration = class(syntax_tree_node)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

  end;
  
  statement = class(declaration)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

  end;
  
  try_statement = class(statement)
    private _statements: statement_list;
    // Methods
    public constructor Create; 
    public constructor Create(_statements: statement_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property statements: statement_list read _statements write _statements;

    // Fields
  end;
  
  expression = class(statement)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

  end;
  
  unit_or_namespace = class(syntax_tree_node)
    private _name: ident_list;
    // Methods
    public constructor Create; 
    public constructor Create(_name: ident_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property name: ident_list read _name write _name;

    // Fields
  end;
  
  token_info = class(syntax_tree_node)
    private _text: string;
    // Methods
    public constructor Create; 
    public constructor Create(_text: string); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property text: string read _text write _text;

    // Fields
  end;
  
  type_definition = class(declaration)
    private _attr_list: type_definition_attr_list;
    // Methods
    public constructor Create; 
    public constructor Create(_attr_list: type_definition_attr_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property attr_list: type_definition_attr_list read _attr_list write _attr_list;

    // Fields
  end;
  
  proc_block = class(syntax_tree_node)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

  end;
  
  addressed_value = class(expression)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

  end;
  
  simple_property = class(declaration)
    private _accessors: property_accessors;
     private _array_default: property_array_default;
     private _index_expression: expression;
     private _parameter_list: property_parameter_list;
     private _property_name: ident;
     private _property_type: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_property_name: ident; _property_type: type_definition; _index_expression: expression; _accessors: property_accessors; _array_default: property_array_default; _parameter_list: property_parameter_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property accessors: property_accessors read _accessors write _accessors;
    public property array_default: property_array_default read _array_default write _array_default;
    public property index_expression: expression read _index_expression write _index_expression;
    public property parameter_list: property_parameter_list read _parameter_list write _parameter_list;
    public property property_name: ident read _property_name write _property_name;
    public property property_type: type_definition read _property_type write _property_type;

    // Fields
    end;

  const_node = class(addressed_value)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

  end;
  
  int64_const = class(const_node)
    private _val: Int64;
    // Methods
    public constructor Create; 
    public constructor Create(_val: Int64); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property val: Int64 read _val write _val;

    // Fields
  end;
  
  compilation_unit = class(syntax_tree_node)
    private _compiler_directives: List<compiler_directive>;
     private _file_name: string;
     private _Language: LanguageId;
    // Methods
    public constructor Create; 
    public constructor Create(_file_name: string; _compiler_directives: List<compiler_directive>; _Language: LanguageId); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property compiler_directives: List<compiler_directive> read _compiler_directives write _compiler_directives;
    public property file_name: string read _file_name write _file_name;
    public property Language: LanguageId read _Language write _Language;

    // Fields

  end;
  
  literal = class(const_node)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

  end;
  
  type_declaration = class(declaration)
    private _type_def: type_definition;
     private _type_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_type_name: ident; _type_def: type_definition); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property type_def: type_definition read _type_def write _type_def;
    public property type_name: ident read _type_name write _type_name;

    // Fields
  end;
  
  procedure_header = class(type_definition)
    private _class_keyword: boolean;
     private _name: method_name;
     private _of_object: boolean;
     private _parametres: formal_parametres;
     private _proc_attributes: procedure_attributes_list;
    // Methods
    public constructor Create; 
    public constructor Create(_parametres: formal_parametres; _proc_attributes: procedure_attributes_list; _name: method_name; _of_object: boolean; _class_keyword: boolean); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property class_keyword: boolean read _class_keyword write _class_keyword;
    public property name: method_name read _name write _name;
    public property of_object: boolean read _of_object write _of_object;
    public property parametres: formal_parametres read _parametres write _parametres;
    public property proc_attributes: procedure_attributes_list read _proc_attributes write _proc_attributes;

    // Fields
  end;
  
  IVisitor = class
    // Methods
    procedure visit(_access_modifer_node: access_modifer_node);virtual;
    procedure visit(_addressed_value: addressed_value);virtual;
    procedure visit(_addressed_value_funcname: addressed_value_funcname);virtual;
    procedure visit(_array_const: array_const);virtual;
    procedure visit(_array_of_const_type_definition: array_of_const_type_definition);virtual;
    procedure visit(_array_of_named_type_definition: array_of_named_type_definition);virtual;
    procedure visit(_array_size: array_size);virtual;
    procedure visit(_array_type: array_type);virtual;
    procedure visit(_assign: assign);virtual;
    procedure visit(_bin_expr: bin_expr);virtual;
    procedure visit(_block: block);virtual;
    procedure visit(_bool_const: bool_const);virtual;
    procedure visit(_c_for_cycle: c_for_cycle);virtual;
    procedure visit(_c_module: c_module);virtual;
    procedure visit(_c_scalar_type: c_scalar_type);virtual;
    procedure visit(_case_node: case_node);virtual;
    procedure visit(_case_variant: case_variant);virtual;
    procedure visit(_case_variants: case_variants);virtual;
    procedure visit(_char_const: char_const);virtual;
    procedure visit(_class_body: class_body);virtual;
    procedure visit(_class_definition: class_definition);virtual;
    procedure visit(_class_members: class_members);virtual;
    procedure visit(_class_predefinition: class_predefinition);virtual;
    procedure visit(_compilation_unit: compilation_unit);virtual;
    procedure visit(_compiler_directive: compiler_directive);virtual;
    procedure visit(_compiler_directive_if: compiler_directive_if);virtual;
    procedure visit(_compiler_directive_list: compiler_directive_list);virtual;
    procedure visit(_const_definition: const_definition);virtual;
    procedure visit(_const_node: const_node);virtual;
    procedure visit(_constructor: &constructor);virtual;
    procedure visit(_consts_definitions_list: consts_definitions_list);virtual;
    procedure visit(_declaration: declaration);virtual;
    procedure visit(_declarations: declarations);virtual;
    procedure visit(_declarations_as_statement: declarations_as_statement);virtual;
    procedure visit(_default_indexer_property_node: default_indexer_property_node);virtual;
    procedure visit(_dereference: dereference);virtual;
    procedure visit(_destructor: &destructor);virtual;
    procedure visit(_diap_expr: diap_expr);virtual;
    procedure visit(_diapason: diapason);virtual;
    procedure visit(_diapason_expr: diapason_expr);virtual;
    procedure visit(_documentation_comment_list: documentation_comment_list);virtual;
    procedure visit(_documentation_comment_section: documentation_comment_section);virtual;
    procedure visit(_documentation_comment_tag: documentation_comment_tag);virtual;
    procedure visit(_documentation_comment_tag_param: documentation_comment_tag_param);virtual;
    procedure visit(_dot_node: dot_node);virtual;
    procedure visit(_double_const: double_const);virtual;
    procedure visit(_empty_statement: empty_statement);virtual;
    procedure visit(_enum_type_definition: enum_type_definition);virtual;
    procedure visit(_enumerator: enumerator);virtual;
    procedure visit(_enumerator_list: enumerator_list);virtual;
    procedure visit(_exception_block: exception_block);virtual;
    procedure visit(_exception_handler: exception_handler);virtual;
    procedure visit(_exception_handler_list: exception_handler_list);virtual;
    procedure visit(_exception_ident: exception_ident);virtual;
    procedure visit(_expression: expression);virtual;
    procedure visit(_expression_as_statement: expression_as_statement);virtual;
    procedure visit(_expression_list: expression_list);virtual;
    procedure visit(_external_directive: external_directive);virtual;
    procedure visit(_file_type: file_type);virtual;
    procedure visit(_file_type_definition: file_type_definition);virtual;
    procedure visit(_for_node: for_node);virtual;
    procedure visit(_foreach_stmt: foreach_stmt);virtual;
    procedure visit(_formal_parametres: formal_parametres);virtual;
    procedure visit(_format_expr: format_expr);virtual;
    procedure visit(_function_header: function_header);virtual;
    procedure visit(_get_address: get_address);virtual;
    procedure visit(_goto_statement: goto_statement);virtual;
    procedure visit(_hex_constant: hex_constant);virtual;
    procedure visit(_ident: ident);virtual;
    procedure visit(_ident_list: ident_list);virtual;
    procedure visit(_if_node: if_node);virtual;
    procedure visit(_implementation_node: implementation_node);virtual;
    procedure visit(_index_property: index_property);virtual;
    procedure visit(_indexer: indexer);virtual;
    procedure visit(_indexers_types: indexers_types);virtual;
    procedure visit(_inherited_ident: inherited_ident);virtual;
    procedure visit(_inherited_message: inherited_message);virtual;
    procedure visit(_inherited_method_call: inherited_method_call);virtual;
    procedure visit(_initfinal_part: initfinal_part);virtual;
    procedure visit(_int32_const: int32_const);virtual;
    procedure visit(_int64_const: int64_const);virtual;
    procedure visit(_interface_node: interface_node);virtual;
    procedure visit(_jump_stmt: jump_stmt);virtual;
    procedure visit(_known_type_definition: known_type_definition);virtual;
    procedure visit(_known_type_ident: known_type_ident);virtual;
    procedure visit(_label_definitions: label_definitions);virtual;
    procedure visit(_labeled_statement: labeled_statement);virtual;
    procedure visit(_literal: literal);virtual;
    procedure visit(_literal_const_line: literal_const_line);virtual;
    procedure visit(_lock_stmt: lock_stmt);virtual;
    procedure visit(_loop_stmt: loop_stmt);virtual;
    procedure visit(_method_call: method_call);virtual;
    procedure visit(_method_name: method_name);virtual;
    procedure visit(_named_type_reference: named_type_reference);virtual;
    procedure visit(_named_type_reference_list: named_type_reference_list);virtual;
    procedure visit(_new_expr: new_expr);virtual;
    procedure visit(_nil_const: nil_const);virtual;
    procedure visit(_oberon_exit_stmt: oberon_exit_stmt);virtual;
    procedure visit(_oberon_ident_with_export_marker: oberon_ident_with_export_marker);virtual;
    procedure visit(_oberon_import_module: oberon_import_module);virtual;
    procedure visit(_oberon_module: oberon_module);virtual;
    procedure visit(_oberon_procedure_header: oberon_procedure_header);virtual;
    procedure visit(_oberon_procedure_receiver: oberon_procedure_receiver);virtual;
    procedure visit(_oberon_withstmt: oberon_withstmt);virtual;
    procedure visit(_oberon_withstmt_guardstat: oberon_withstmt_guardstat);virtual;
    procedure visit(_oberon_withstmt_guardstat_list: oberon_withstmt_guardstat_list);virtual;
    procedure visit(_on_exception: on_exception);virtual;
    procedure visit(_on_exception_list: on_exception_list);virtual;
    procedure visit(_op_type_node: op_type_node);virtual;
    procedure visit(_operator_name_ident: operator_name_ident);virtual;
    procedure visit(_pascal_set_constant: pascal_set_constant);virtual;
    procedure visit(_proc_block: proc_block);virtual;
    procedure visit(_procedure_attribute: procedure_attribute);virtual;
    procedure visit(_procedure_attributes_list: procedure_attributes_list);virtual;
    procedure visit(_procedure_call: procedure_call);virtual;
    procedure visit(_procedure_definition: procedure_definition);virtual;
    procedure visit(_procedure_header: procedure_header);virtual;
    procedure visit(_program_body: program_body);virtual;
    procedure visit(_program_module: program_module);virtual;
    procedure visit(_program_name: program_name);virtual;
    procedure visit(_program_tree: program_tree);virtual;
    procedure visit(_property_accessors: property_accessors);virtual;
    procedure visit(_property_array_default: property_array_default);virtual;
    procedure visit(_property_interface: property_interface);virtual;
    procedure visit(_property_parameter: property_parameter);virtual;
    procedure visit(_property_parameter_list: property_parameter_list);virtual;
    procedure visit(_question_colon_expression: question_colon_expression);virtual;
    procedure visit(_raise_statement: raise_statement);virtual;
    procedure visit(_raise_stmt: raise_stmt);virtual;
    procedure visit(_read_accessor_name: read_accessor_name);virtual;
    procedure visit(_record_const: record_const);virtual;
    procedure visit(_record_const_definition: record_const_definition);virtual;
    procedure visit(_record_type: record_type);virtual;
    procedure visit(_record_type_parts: record_type_parts);virtual;
    procedure visit(_ref_type: ref_type);virtual;
    procedure visit(_repeat_node: repeat_node);virtual;
    procedure visit(_roof_dereference: roof_dereference);virtual;
    procedure visit(_set_type_definition: set_type_definition);virtual;
    procedure visit(_sharp_char_const: sharp_char_const);virtual;
    procedure visit(_simple_const_definition: simple_const_definition);virtual;
    procedure visit(_simple_property: simple_property);virtual;
    procedure visit(_sizeof_operator: sizeof_operator);virtual;
    procedure visit(_statement: statement);virtual;
    procedure visit(_statement_list: statement_list);virtual;
    procedure visit(_string_const: string_const);virtual;
    procedure visit(_string_num_definition: string_num_definition);virtual;
    procedure visit(_subprogram_body: subprogram_body);virtual;
    procedure visit(_switch_stmt: switch_stmt);virtual;
    procedure visit(_syntax_tree_node: syntax_tree_node);virtual;
    procedure visit(_template_param_list: template_param_list);virtual;
    procedure visit(_template_type_reference: template_type_reference);virtual;
    procedure visit(_token_info: token_info);virtual;
    procedure visit(_token_taginfo: token_taginfo);virtual;
    procedure visit(_try_except_statement: try_except_statement);virtual;
    procedure visit(_try_finally_statement: try_finally_statement);virtual;
    procedure visit(_try_handler: try_handler);virtual;
    procedure visit(_try_handler_except: try_handler_except);virtual;
    procedure visit(_try_handler_finally: try_handler_finally);virtual;
    procedure visit(_try_statement: try_statement);virtual;
    procedure visit(_try_stmt: try_stmt);virtual;
    procedure visit(_type_declaration: type_declaration);virtual;
    procedure visit(_type_declarations: type_declarations);virtual;
    procedure visit(_type_definition: type_definition);virtual;
    procedure visit(_type_definition_attr: type_definition_attr);virtual;
    procedure visit(_type_definition_attr_list: type_definition_attr_list);virtual;
    procedure visit(_type_definition_list: type_definition_list);virtual;
    procedure visit(_typecast_node: typecast_node);virtual;
    procedure visit(_typed_const_definition: typed_const_definition);virtual;
    procedure visit(_typed_parametres: typed_parametres);virtual;
    procedure visit(_typeof_operator: typeof_operator);virtual;
    procedure visit(_uint64_const: uint64_const);virtual;
    procedure visit(_un_expr: un_expr);virtual;
    procedure visit(_unit_module: unit_module);virtual;
    procedure visit(_unit_name: unit_name);virtual;
    procedure visit(_unit_or_namespace: unit_or_namespace);virtual;
    procedure visit(_uses_list: uses_list);virtual;
    procedure visit(_uses_unit_in: uses_unit_in);virtual;
    procedure visit(_using_list: using_list);virtual;
    procedure visit(_var_def_list: var_def_list);virtual;
    procedure visit(_var_def_statement: var_def_statement);virtual;
    procedure visit(_var_statement: var_statement);virtual;
    procedure visit(_variable_definitions: variable_definitions);virtual;
    procedure visit(_variant: variant);virtual;
    procedure visit(_variant_list: variant_list);virtual;
    procedure visit(_variant_record_type: variant_record_type);virtual;
    procedure visit(_variant_type: variant_type);virtual;
    procedure visit(_variant_types: variant_types);virtual;
    procedure visit(_where_definition: where_definition);virtual;
    procedure visit(_where_definition_list: where_definition_list);virtual;
    procedure visit(_while_node: while_node);virtual;
    procedure visit(_with_statement: with_statement);virtual;
    procedure visit(_write_accessor_name: write_accessor_name);virtual;

end;
  
  
  
  OperatorServices = class
    // Methods
    public constructor Create; 
    public class function IsAssigmentOperator(&Operator: Operators): boolean; 
    public class function ToString(&Operator: Operators; Language: LanguageId): string;

  end;
  
  Utils = class
    // Methods
    public constructor Create; 
    public class function IdentListToString(list: List<ident>; separator: string): string;

  end;

  access_modifer_node = class(syntax_tree_node)
    private _access_level: access_modifer;
    // Methods
    public constructor Create; 
    public constructor Create(_access_level: access_modifer); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property access_level: access_modifer read _access_level write _access_level;

    // Fields


  end;
  
  
  addressed_value_funcname = class(addressed_value)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

  end;
  
  array_const = class(expression)
    private _elements: expression_list;
    // Methods
    public constructor Create; 
    public constructor Create(_elements: expression_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property elements: expression_list read _elements write _elements;

    // Fields
  end;
  
  array_of_const_type_definition = class(type_definition)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

  end;
  
  array_of_named_type_definition = class(type_definition)
    private _type_name: named_type_reference;
    // Methods
    public constructor Create; 
    public constructor Create(_type_name: named_type_reference); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property type_name: named_type_reference read _type_name write _type_name;

    // Fields


  end;
  
  array_size = class(type_definition)
    private _max_value: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_max_value: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property max_value: expression read _max_value write _max_value;

    // Fields


  end;
  
  array_type = class(type_definition)
    private _elemets_types: type_definition;
    private _indexers: indexers_types;
    // Methods
    public constructor Create; 
    public constructor Create(_indexers: indexers_types; _elemets_types: type_definition); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property elemets_types: type_definition read _elemets_types write _elemets_types;
    public property indexers: indexers_types read _indexers write _indexers;

    // Fields


  end;
  
  assign = class(expression)
     private _from: expression;
     private _operator_type: Operators;
     private _to: addressed_value;
    // Methods
    public constructor Create; 
    public constructor Create(_to: addressed_value; _from: expression; _operator_type: Operators); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property from: expression read _from write _from;
    public property operator_type: Operators read _operator_type write _operator_type;
    public property &to: addressed_value read _to write _to;

    // Fields


end;

  bin_expr = class(addressed_value)
     private _left: expression;
     private _operation_type: Operators;
     private _right: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_left: expression; _right: expression; _operation_type: Operators); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property left: expression read _left write _left;
    public property operation_type: Operators read _operation_type write _operation_type;
    public property right: expression read _right write _right;

    // Fields


end;

   block = class(proc_block)
    // Fields
    private _defs: declarations;
    private _program_code: statement_list;
    
    // Methods
    public constructor Create; 
    public constructor Create(_defs: declarations; _program_code: statement_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property defs: declarations read _defs write _defs;
    public property program_code: statement_list read _program_code write _program_code;

end;

   bool_const = class(const_node)
    // Fields
    private _val: boolean;
    // Methods
    public constructor Create; 
    public constructor Create(_val: boolean); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property val: boolean read _val write _val;

end;

   c_for_cycle = class(statement)
     private _expr1: statement;
     private _expr2: expression;
     private _expr3: expression;
     private _stmt: statement;
    // Methods
    public constructor Create; 
    public constructor Create(_expr1: statement; _expr2: expression; _expr3: expression; _stmt: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property expr1: statement read _expr1 write _expr1;
    public property expr2: expression read _expr2 write _expr2;
    public property expr3: expression read _expr3 write _expr3;
    public property stmt: statement read _stmt write _stmt;
     

end;

   c_module = class(compilation_unit)
    // Fields
     private _defs: declarations;
     private _used_units: uses_list;

    // Methods
    public constructor Create; 
    public constructor Create(_defs: declarations; _used_units: uses_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property defs: declarations read _defs write _defs;
    public property used_units: uses_list read _used_units write _used_units;

end;

c_scalar_type = class(type_definition)
    // Fields
     private _scalar_name: c_scalar_type_name;
     private _sign: c_scalar_sign;

    // Methods
    public constructor Create; 
    public constructor Create(_scalar_name: c_scalar_type_name; _sign: c_scalar_sign); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property scalar_name: c_scalar_type_name read _scalar_name write _scalar_name;
    public property sign: c_scalar_sign read _sign write _sign;

    
end;

case_node = class(statement)
        // Fields
     private _conditions: case_variants;
     private _else_statement: statement;
     private _param: expression;
     
    // Methods
    public constructor Create; 
    public constructor Create(_param: expression; _conditions: case_variants; _else_statement: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property conditions: case_variants read _conditions write _conditions;
    public property else_statement: statement read _else_statement write _else_statement;
    public property param: expression read _param write _param;

end;

case_variant = class(statement)
       // Fields
     private _conditions: expression_list;
     private _exec_if_true: statement;
     
    // Methods
    public constructor Create; 
    public constructor Create(_conditions: expression_list; _exec_if_true: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property conditions: expression_list read _conditions write _conditions;
    public property exec_if_true: statement read _exec_if_true write _exec_if_true;

 

end;

case_variants = class(syntax_tree_node)
     // Fields
     private _variants: List<case_variant>;
    // Methods
    public constructor Create; 
    public constructor Create(_variants: List<case_variant>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property variants: List<case_variant> read _variants write _variants;

end;

char_const = class(literal)
       // Fields
     private _cconst: Char;
    // Methods
    public constructor Create; 
    public constructor Create(_cconst: Char); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property cconst: Char read _cconst write _cconst;


end;

class_body = class(syntax_tree_node)
    private _class_def_blocks: List<class_members>;
    // Methods
    public constructor Create; 
    public constructor Create(_class_def_blocks: List<class_members>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property class_def_blocks: List<class_members> read _class_def_blocks write _class_def_blocks;

    // Fields
     

end;

class_definition = class(type_definition)
     private _attribute: class_attribute;
     private _body: class_body;
     private _class_parents: named_type_reference_list;
     private _keyword: class_keyword;
     private _template_args: ident_list;
     private _where_section: where_definition_list;
    // Methods
    public constructor Create; 
    public constructor Create(_class_parents: named_type_reference_list; _body: class_body; _keyword: class_keyword; _template_args: ident_list; _where_section: where_definition_list; _attribute: class_attribute); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property attribute: class_attribute read _attribute write _attribute;
    public property body: class_body read _body write _body;
    public property class_parents: named_type_reference_list read _class_parents write _class_parents;
    public property keyword: class_keyword read _keyword write _keyword;
    public property template_args: ident_list read _template_args write _template_args;
    public property where_section: where_definition_list read _where_section write _where_section;

    // Fields
end;

class_members = class(syntax_tree_node)
    private _access_mod: access_modifer_node;
    private _members: List<declaration>;
    // Methods
    public constructor Create; 
    public constructor Create(_members: List<declaration>; _access_mod: access_modifer_node); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property access_mod: access_modifer_node read _access_mod write _access_mod;
    public property members: List<declaration> read _members write _members;

end;

class_predefinition = class(type_declaration)
   private _class_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_class_name: ident); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property class_name: ident read _class_name write _class_name;

    // Fields
     

end;

 

 compiler_directive = class(syntax_tree_node)
    private _Directive: token_info;
     private _Name: token_info;
    // Methods
    public constructor Create; 
    public constructor Create(_Name: token_info; _Directive: token_info); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property Directive: token_info read _Directive write _Directive;
    public property Name: token_info read _Name write _Name;

    // Fields

end;

compiler_directive_if = class(compiler_directive)
   private _elseif_part: compiler_directive;
     private _if_part: compiler_directive;
    // Methods
    public constructor Create; 
    public constructor Create(_if_part: compiler_directive; _elseif_part: compiler_directive); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property elseif_part: compiler_directive read _elseif_part write _elseif_part;
    public property if_part: compiler_directive read _if_part write _if_part;

    // Fields
    

end;

 compiler_directive_list = class(compiler_directive)
    private _directives: List<compiler_directive>;
    // Methods
    public constructor Create; 
    public constructor Create(_directives: List<compiler_directive>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property directives: List<compiler_directive> read _directives write _directives;

    // Fields
     

end;

const_definition = class(declaration)
    private _const_name: ident;
     private _const_value: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_const_name: ident; _const_value: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property const_name: ident read _const_name write _const_name;
    public property const_value: expression read _const_value write _const_value;

    // Fields
     

end;


&constructor = class(procedure_header)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;

consts_definitions_list = class(declaration)
    private _const_defs: List<const_definition>;
    // Methods
    public constructor Create; 
    public constructor Create(_const_defs: List<const_definition>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property const_defs: List<const_definition> read _const_defs write _const_defs;

    // Fields
    

end;



 declarations = class(syntax_tree_node)
    private _defs: List<declaration>;
    // Methods
    public constructor Create; 
    public constructor Create(_defs: List<declaration>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property defs: List<declaration> read _defs write _defs;

    // Fields
     

end;

declarations_as_statement = class(statement)
    private _defs: declarations;
    // Methods
    public constructor Create; 
    public constructor Create(_defs: declarations); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property defs: declarations read _defs write _defs;

    // Fields
     

end;

default_indexer_property_node = class(syntax_tree_node)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;

dereference = class(addressed_value_funcname)
    private _dereferencing_value: addressed_value;
    // Methods
    public constructor Create; 
    public constructor Create(_dereferencing_value: addressed_value); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property dereferencing_value: addressed_value read _dereferencing_value write _dereferencing_value;

    // Fields
     

end;

&destructor = class(procedure_header)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;

diap_expr = class(expression)
    private _left: expression;
     private _right: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_left: expression; _right: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property left: expression read _left write _left;
    public property right: expression read _right write _right;

    // Fields
     

end;

 diapason = class(type_definition)
    private _left: expression;
     private _right: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_left: expression; _right: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property left: expression read _left write _left;
    public property right: expression read _right write _right;

    // Fields


end;

diapason_expr = class(expression)
    private _left: expression;
     private _right: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_left: expression; _right: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property left: expression read _left write _left;
    public property right: expression read _right write _right;

    // Fields
     

end;

documentation_comment_list = class(syntax_tree_node)
    private _sections: List<documentation_comment_section>;
    // Methods
    public constructor Create; 
    public constructor Create(_sections: List<documentation_comment_section>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property sections: List<documentation_comment_section> read _sections write _sections;

    // Fields
     

end;

 documentation_comment_section = class(syntax_tree_node)
    private _tags: List<documentation_comment_tag>;
     private _text: string;
    // Methods
    public constructor Create; 
    public constructor Create(_tags: List<documentation_comment_tag>; _text: string); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property tags: List<documentation_comment_tag> read _tags write _tags;
    public property text: string read _text write _text;

    // Fields
     

end;

documentation_comment_tag = class(syntax_tree_node)
    private _name: string;
     private _parameters: List<documentation_comment_tag_param>;
     private _text: string;
    // Methods
    public constructor Create; 
    public constructor Create(_name: string; _parameters: List<documentation_comment_tag_param>; _text: string); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property name: string read _name write _name;
    public property parameters: List<documentation_comment_tag_param> read _parameters write _parameters;
    public property text: string read _text write _text;

    // Fields
     

end;

documentation_comment_tag_param = class(syntax_tree_node)
    private _name: string;
     private _value: string;
    // Methods
    public constructor Create; 
    public constructor Create(_name: string; _value: string); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property name: string read _name write _name;
    public property value: string read _value write _value;

    // Fields
     

end;

dot_node = class(addressed_value_funcname)
    private _left: addressed_value;
     private _right: addressed_value;
    // Methods
    public constructor Create; 
    public constructor Create(_left: addressed_value; _right: addressed_value); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property left: addressed_value read _left write _left;
    public property right: addressed_value read _right write _right;

    // Fields
     

end;

double_const = class(const_node)
    // Fields
     private _val: Double;
    // Methods
    public constructor Create; 
    public constructor Create(_val: Double); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property val: Double read _val write _val;

    

end;

 empty_statement = class(statement)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;

enum_type_definition = class(type_definition)
    private _enumerators: enumerator_list;
    // Methods
    public constructor Create; 
    public constructor Create(_enumerators: enumerator_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property enumerators: enumerator_list read _enumerators write _enumerators;

    // Fields
     

end;

enumerator = class(syntax_tree_node)
    private _name: ident;
     private _value: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_name: ident; _value: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property name: ident read _name write _name;
    public property value: expression read _value write _value;

    // Fields
     

end;

enumerator_list = class(syntax_tree_node)
    private _enumerators: List<enumerator>;
    // Methods
    public constructor Create; 
    public constructor Create(_enumerators: List<enumerator>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property enumerators: List<enumerator> read _enumerators write _enumerators;

    // Fields
     

end;

exception_block = class(syntax_tree_node)
    private _else_stmt_list: statement_list;
     private _handlers: exception_handler_list;
     private _stmt_list: statement_list;
    // Methods
    public constructor Create; 
    public constructor Create(_stmt_list: statement_list; _handlers: exception_handler_list; _else_stmt_list: statement_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property else_stmt_list: statement_list read _else_stmt_list write _else_stmt_list;
    public property handlers: exception_handler_list read _handlers write _handlers;
    public property stmt_list: statement_list read _stmt_list write _stmt_list;

    // Fields
     

end;

exception_handler = class(syntax_tree_node)
    private _statements: statement;
     private _type_name: named_type_reference;
     private _variable: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_variable: ident; _type_name: named_type_reference; _statements: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property statements: statement read _statements write _statements;
    public property type_name: named_type_reference read _type_name write _type_name;
    public property variable: ident read _variable write _variable;

    // Fields
     

end;

exception_handler_list = class(syntax_tree_node)
    private _handlers: List<exception_handler>;
    // Methods
    public constructor Create; 
    public constructor Create(_handlers: List<exception_handler>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property handlers: List<exception_handler> read _handlers write _handlers;

    // Fields
     

end;

exception_ident = class(syntax_tree_node)
    private _type_name: named_type_reference;
     private _variable: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_variable: ident; _type_name: named_type_reference); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property type_name: named_type_reference read _type_name write _type_name;
    public property variable: ident read _variable write _variable;

    // Fields
     

end;


expression_as_statement = class(statement)
    private _expr: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_expr: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property expr: expression read _expr write _expr;

    // Fields
     

end;

expression_list = class(expression)
    private _expressions: List<expression>;
    // Methods
    public constructor Create; 
    public constructor Create(_expressions: List<expression>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property expressions: List<expression> read _expressions write _expressions;

    // Fields
     

end;

external_directive = class(proc_block)
    private _modulename: expression;
     private _name: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_modulename: expression; _name: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property modulename: expression read _modulename write _modulename;
    public property name: expression read _name write _name;

    // Fields
     

end;

 file_type = class(type_definition)
    private _file_of_type: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_file_of_type: type_definition); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property file_of_type: type_definition read _file_of_type write _file_of_type;

    // Fields
     

end;

file_type_definition = class(type_definition)
    private _elem_type: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_elem_type: type_definition); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property elem_type: type_definition read _elem_type write _elem_type;

    // Fields
     

end;

for_node = class(statement)
    private _create_loop_variable: boolean;
     private _cycle_type: for_cycle_type;
     private _finish_value: expression;
     private _increment_value: expression;
     private _initial_value: expression;
     private _loop_variable: ident;
     private _statements: statement;
     private _type_name: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_loop_variable: ident; _initial_value: expression; _finish_value: expression; _statements: statement; _cycle_type: for_cycle_type; _increment_value: expression; _type_name: type_definition; _create_loop_variable: boolean); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property create_loop_variable: boolean read _create_loop_variable write _create_loop_variable;
    public property cycle_type: for_cycle_type read _cycle_type write _cycle_type;
    public property finish_value: expression read _finish_value write _finish_value;
    public property increment_value: expression read _increment_value write _increment_value;
    public property initial_value: expression read _initial_value write _initial_value;
    public property loop_variable: ident read _loop_variable write _loop_variable;
    public property statements: statement read _statements write _statements;
    public property type_name: type_definition read _type_name write _type_name;

    // Fields
     

end;

foreach_stmt = class(statement)
    private _identifier: ident;
     private _in_what: expression;
     private _stmt: statement;
     private _type_name: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_identifier: ident; _type_name: type_definition; _in_what: expression; _stmt: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property identifier: ident read _identifier write _identifier;
    public property in_what: expression read _in_what write _in_what;
    public property stmt: statement read _stmt write _stmt;
    public property type_name: type_definition read _type_name write _type_name;

    // Fields
     

end;

 formal_parametres = class(syntax_tree_node)
    private _params_list: List<typed_parametres>;
    // Methods
    public constructor Create; 
    public constructor Create(_params_list: List<typed_parametres>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property params_list: List<typed_parametres> read _params_list write _params_list;

    // Fields
     

end;

format_expr = class(expression)
    private _expr: expression;
     private _format1: expression;
     private _format2: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_expr: expression; _format1: expression; _format2: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property expr: expression read _expr write _expr;
    public property format1: expression read _format1 write _format1;
    public property format2: expression read _format2 write _format2;

    // Fields
     

end;

 function_header = class(procedure_header)
    private _return_type: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_return_type: type_definition); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property return_type: type_definition read _return_type write _return_type;

    // Fields
     

end;

get_address = class(addressed_value_funcname)
    private _address_of: addressed_value;
    // Methods
    public constructor Create; 
    public constructor Create(_address_of: addressed_value); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property address_of: addressed_value read _address_of write _address_of;

    // Fields
     

end;

goto_statement = class(statement)
    private _label: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_label: ident); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property &label: ident read _label write _label;

    // Fields
     

end;

hex_constant = class(int64_const)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;

ident = class(addressed_value_funcname)
     private _name: string;
    // Methods
    public constructor Create; 
    public constructor Create(_name: string); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property name: string read _name write _name;

    // Fields
    

end;

ident_list = class(syntax_tree_node)
    private _idents: List<ident>;
    // Methods
    public constructor Create; 
    public constructor Create(_idents: List<ident>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property idents: List<ident> read _idents write _idents;

    // Fields
     

end;

if_node = class(statement)
    private _condition: expression;
    private _else_body: statement;
    private _then_body: statement;

    // Methods
    public constructor Create; 
    public constructor Create(_condition: expression; _then_body: statement; _else_body: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property condition: expression read _condition write _condition;
    public property else_body: statement read _else_body write _else_body;
    public property then_body: statement read _then_body write _then_body;

    // Fields
     

end;

implementation_node = class(syntax_tree_node)
     private _implementation_definitions: declarations;
     private _uses_modules: uses_list;
     private _using_namespaces: using_list;
    // Methods
    public constructor Create; 
    public constructor Create(_uses_modules: uses_list; _implementation_definitions: declarations; _using_namespaces: using_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property implementation_definitions: declarations read _implementation_definitions write _implementation_definitions;
    public property uses_modules: uses_list read _uses_modules write _uses_modules;
    public property using_namespaces: using_list read _using_namespaces write _using_namespaces;

    // Fields


end;

index_property = class(simple_property)
    private _is_default: default_indexer_property_node;
     private _property_parametres: formal_parametres;
    // Methods
    public constructor Create; 
    public constructor Create(_property_parametres: formal_parametres; _is_default: default_indexer_property_node); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property is_default: default_indexer_property_node read _is_default write _is_default;
    public property property_parametres: formal_parametres read _property_parametres write _property_parametres;

    // Fields
     

end;

indexer = class(dereference)
    private _indexes: expression_list;
    // Methods
    public constructor Create; 
    public constructor Create(_indexes: expression_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property indexes: expression_list read _indexes write _indexes;

    // Fields
     

end;

 indexers_types = class(type_definition)
    private _indexers: List<type_definition>;
    // Methods
    public constructor Create; 
    public constructor Create(_indexers: List<type_definition>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property indexers: List<type_definition> read _indexers write _indexers;

    // Fields
     

end;

inherited_ident = class(ident)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;

inherited_message = class(statement)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;

inherited_method_call = class(statement)
    private _exprs: expression_list;
     private _method_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_method_name: ident; _exprs: expression_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property exprs: expression_list read _exprs write _exprs;
    public property method_name: ident read _method_name write _method_name;

    // Fields
     

end;

initfinal_part = class(syntax_tree_node)
    private _finalization_sect: statement_list;
     private _initialization_sect: statement_list;
    // Methods
    public constructor Create; 
    public constructor Create(_initialization_sect: statement_list; _finalization_sect: statement_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property finalization_sect: statement_list read _finalization_sect write _finalization_sect;
    public property initialization_sect: statement_list read _initialization_sect write _initialization_sect;

    // Fields
     

end;

int32_const = class(const_node)
    private _val: Integer;
    // Methods
    public constructor Create; 
    public constructor Create(_val: Integer); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property val: Integer read _val write _val;

    // Fields
     
end;



interface_node = class(syntax_tree_node)
    private _interface_definitions: declarations;
     private _uses_modules: uses_list;
     private _using_namespaces: using_list;
    // Methods
    public constructor Create; 
    public constructor Create(_interface_definitions: declarations; _uses_modules: uses_list; _using_namespaces: using_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property interface_definitions: declarations read _interface_definitions write _interface_definitions;
    public property uses_modules: uses_list read _uses_modules write _uses_modules;
    public property using_namespaces: using_list read _using_namespaces write _using_namespaces;

    // Fields
     

end;

 jump_stmt = class(statement)
    private _expr: expression;
     private _JumpType: JumpStmtType;
    // Methods
    public constructor Create; 
    public constructor Create(_expr: expression; _JumpType: JumpStmtType); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property expr: expression read _expr write _expr;
    public property JumpType: JumpStmtType read _JumpType write _JumpType;

    // Fields
     

end;

known_type_definition = class(type_definition)
    private _tp: known_type;
     private _unit_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_tp: known_type; _unit_name: ident); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property tp: known_type read _tp write _tp;
    public property unit_name: ident read _unit_name write _unit_name;

    // Fields
     

end;

known_type_ident = class(ident)
    private _type: known_type;
    // Methods
    public constructor Create; 
    public constructor Create(_type: known_type); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property &type: known_type read _type write _type;

    // Fields
     

end;

 label_definitions = class(declaration)
    private _labels: ident_list;
    // Methods
    public constructor Create; 
    public constructor Create(_labels: ident_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property labels: ident_list read _labels write _labels;

    // Fields
     

end;

 labeled_statement = class(statement)
    private _label_name: ident;
     private _to_statement: statement;
    // Methods
    public constructor Create; 
    public constructor Create(_label_name: ident; _to_statement: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property label_name: ident read _label_name write _label_name;
    public property to_statement: statement read _to_statement write _to_statement;

    // Fields
     

end;

literal_const_line = class(literal)
    private _literals: List<literal>;
    // Methods
    public constructor Create; 
    public constructor Create(_literals: List<literal>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property literals: List<literal> read _literals write _literals;

    // Fields
     

end;

 lock_stmt = class(statement)
    private _lock_object: expression;
     private _stmt: statement;
    // Methods
    public constructor Create; 
    public constructor Create(_lock_object: expression; _stmt: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property lock_object: expression read _lock_object write _lock_object;
    public property stmt: statement read _stmt write _stmt;

    // Fields
     

end;

loop_stmt = class(statement)
    private _stmt: statement;
    // Methods
    public constructor Create; 
    public constructor Create(_stmt: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property stmt: statement read _stmt write _stmt;

    // Fields
     

end;

method_call = class(dereference)
    private _parametres: expression_list;
    // Methods
    public constructor Create; 
    public constructor Create(_parametres: expression_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property parametres: expression_list read _parametres write _parametres;

    // Fields
     

end;

method_name = class(syntax_tree_node)
    private _class_name: ident;
     private _meth_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_class_name: ident; _meth_name: ident); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property class_name: ident read _class_name write _class_name;
    public property meth_name: ident read _meth_name write _meth_name;

    // Fields
     

end;

named_type_reference = class(type_definition)
    private _names: List<ident>;
    // Methods
    public constructor Create; 
    public constructor Create(_names: List<ident>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property names: List<ident> read _names write _names;

    // Fields
     

end;

named_type_reference_list = class(syntax_tree_node)
    private _types: List<named_type_reference>;
    // Methods
    public constructor Create; 
    public constructor Create(_types: List<named_type_reference>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property types: List<named_type_reference> read _types write _types;

    // Fields
     

end;

new_expr = class(addressed_value)
    private _new_array: boolean;
     private _params_list: expression_list;
     private _type: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_type: type_definition; _params_list: expression_list; _new_array: boolean); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property new_array: boolean read _new_array write _new_array;
    public property params_list: expression_list read _params_list write _params_list;
    public property &type: type_definition read _type write _type;

    // Fields
     

end;

nil_const = class(const_node)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;

oberon_exit_stmt = class(statement)
    private _text: string;
    // Methods
    public constructor Create; 
    public constructor Create(_text: string); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property text: string read _text write _text;

    // Fields
     

end;

oberon_ident_with_export_marker = class(ident)
    private _marker: oberon_export_marker;
    // Methods
    public constructor Create; 
    public constructor Create(_marker: oberon_export_marker); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property marker: oberon_export_marker read _marker write _marker;

    // Fields
     

end;

oberon_import_module = class(unit_or_namespace)
    private _new_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_new_name: ident); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property new_name: ident read _new_name write _new_name;

    // Fields
     

end;

oberon_module = class(compilation_unit)
    private _definitions: declarations;
     private _first_name: ident;
     private _import_list: uses_list;
     private _module_code: statement_list;
     private _second_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_first_name: ident; _second_name: ident; _import_list: uses_list; _definitions: declarations; _module_code: statement_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property definitions: declarations read _definitions write _definitions;
    public property first_name: ident read _first_name write _first_name;
    public property import_list: uses_list read _import_list write _import_list;
    public property module_code: statement_list read _module_code write _module_code;
    public property second_name: ident read _second_name write _second_name;

    // Fields
     

end;

oberon_procedure_header = class(function_header)
    private _first_name: ident;
     private _receiver: oberon_procedure_receiver;
     private _second_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_receiver: oberon_procedure_receiver; _first_name: ident; _second_name: ident); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property first_name: ident read _first_name write _first_name;
    public property receiver: oberon_procedure_receiver read _receiver write _receiver;
    public property second_name: ident read _second_name write _second_name;

    // Fields
     

end;

oberon_procedure_receiver = class(syntax_tree_node)
    private _param_kind: parametr_kind;
     private _receiver_name: ident;
     private _receiver_typename: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_param_kind: parametr_kind; _receiver_name: ident; _receiver_typename: ident); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property param_kind: parametr_kind read _param_kind write _param_kind;
    public property receiver_name: ident read _receiver_name write _receiver_name;
    public property receiver_typename: ident read _receiver_typename write _receiver_typename;

    // Fields
     

end;

oberon_withstmt = class(statement)
    private _else_stmt: statement;
     private _quardstat_list: oberon_withstmt_guardstat_list;
    // Methods
    public constructor Create; 
    public constructor Create(_quardstat_list: oberon_withstmt_guardstat_list; _else_stmt: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property else_stmt: statement read _else_stmt write _else_stmt;
    public property quardstat_list: oberon_withstmt_guardstat_list read _quardstat_list write _quardstat_list;

    // Fields
     

end;

oberon_withstmt_guardstat = class(syntax_tree_node)
    private _name: addressed_value;
     private _stmt: statement;
     private _type_name: type_definition;

    // Methods
    public constructor Create; 
    public constructor Create(_name: addressed_value; _type_name: type_definition; _stmt: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property name: addressed_value read _name write _name;
    public property stmt: statement read _stmt write _stmt;
    public property type_name: type_definition read _type_name write _type_name;

    // Fields
     
end;

oberon_withstmt_guardstat_list = class(syntax_tree_node)
    private _guardstats: List<oberon_withstmt_guardstat>;
    // Methods
    public constructor Create; 
    public constructor Create(_guardstats: List<oberon_withstmt_guardstat>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property guardstats: List<oberon_withstmt_guardstat> read _guardstats write _guardstats;

    // Fields
     

end;

on_exception = class(syntax_tree_node)
    private _exception_type_name: ident;
     private _exception_var_name: ident;
     private _stat: statement;
    // Methods
    public constructor Create; 
    public constructor Create(_exception_var_name: ident; _exception_type_name: ident; _stat: statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property exception_type_name: ident read _exception_type_name write _exception_type_name;
    public property exception_var_name: ident read _exception_var_name write _exception_var_name;
    public property stat: statement read _stat write _stat;

    // Fields
     

end;

on_exception_list = class(syntax_tree_node)
    private _on_exceptions: List<on_exception>;
    // Methods
    public constructor Create; 
    public constructor Create(_on_exceptions: List<on_exception>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property on_exceptions: List<on_exception> read _on_exceptions write _on_exceptions;

    // Fields
     

end;

op_type_node = class(token_info)
    private _type: Operators;
    // Methods
    public constructor Create; 
    public constructor Create(_type: Operators); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property &type: Operators read _type write _type;

    // Fields
     

end;

operator_name_ident = class(ident)
    private _operator_type: Operators;
    // Methods
    public constructor Create; 
    public constructor Create(_operator_type: Operators); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property operator_type: Operators read _operator_type write _operator_type;

    // Fields
     

end;

pascal_set_constant = class(expression)
    private _values: expression_list;
    // Methods
    public constructor Create; 
    public constructor Create(_values: expression_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property values: expression_list read _values write _values;

    // Fields
     

end;



procedure_attribute = class(ident)
    private _attribute_type: proc_attribute;
    // Methods
    public constructor Create; 
    public constructor Create(_attribute_type: proc_attribute); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property attribute_type: proc_attribute read _attribute_type write _attribute_type;

    // Fields
     

end;

procedure_attributes_list = class(syntax_tree_node)
    private _proc_attributes: List<procedure_attribute>;
    // Methods
    public constructor Create; 
    public constructor Create(_proc_attributes: List<procedure_attribute>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property proc_attributes: List<procedure_attribute> read _proc_attributes write _proc_attributes;

    // Fields
     

end;

procedure_call = class(statement)
    private _func_name: addressed_value;
    // Methods
    public constructor Create; 
    public constructor Create(_func_name: addressed_value); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property func_name: addressed_value read _func_name write _func_name;

    // Fields
     

end;

procedure_definition = class(declaration)
    private _proc_body: proc_block;
     private _proc_header: procedure_header;
    // Methods
    public constructor Create; 
    public constructor Create(_proc_header: procedure_header; _proc_body: proc_block); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property proc_body: proc_block read _proc_body write _proc_body;
    public property proc_header: procedure_header read _proc_header write _proc_header;

    // Fields
     

end;



program_body = class(syntax_tree_node)
    private _program_code: statement_list;
     private _program_definitions: declarations;
     private _used_units: uses_list;
     private _using_list: using_list;
    // Methods
    public constructor Create; 
    public constructor Create(_used_units: uses_list; _program_definitions: declarations; _program_code: statement_list; _using_list: using_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property program_code: statement_list read _program_code write _program_code;
    public property program_definitions: declarations read _program_definitions write _program_definitions;
    public property used_units: uses_list read _used_units write _used_units;
    public property using_list: SyntaxTree.using_list read _using_list write _using_list;

    // Fields
     

end;

program_module = class(compilation_unit)
    private _program_block: block;
     private _program_name: program_name;
     private _used_units: uses_list;
     private _using_namespaces: using_list;
    // Methods
    public constructor Create; 
    public constructor Create(_program_name: program_name; _used_units: uses_list; _program_block: block; _using_namespaces: using_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property program_block: block read _program_block write _program_block;
    public property program_name: SyntaxTree.program_name read _program_name write _program_name;
    public property used_units: uses_list read _used_units write _used_units;
    public property using_namespaces: using_list read _using_namespaces write _using_namespaces;

    // Fields
     

end;

program_name = class(syntax_tree_node)
     private _prog_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_prog_name: ident); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property prog_name: ident read _prog_name write _prog_name;

    // Fields
    

end;

program_tree = class(syntax_tree_node)
    private _compilation_units: List<compilation_unit>;
    // Methods
    public constructor Create; 
    public constructor Create(_compilation_units: List<compilation_unit>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property compilation_units: List<compilation_unit> read _compilation_units write _compilation_units;

    // Fields
     

end;

property_accessors = class(syntax_tree_node)
    private _read_accessor: read_accessor_name;
    private _write_accessor: write_accessor_name;
    // Methods
    public constructor Create; 
    public constructor Create(_read_accessor: read_accessor_name; _write_accessor: write_accessor_name); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property read_accessor: read_accessor_name read _read_accessor write _read_accessor;
    public property write_accessor: write_accessor_name read _write_accessor write _write_accessor;

    // Fields
     

end;

property_array_default = class(syntax_tree_node)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;

 property_interface = class(syntax_tree_node)
    private _index_expression: expression;
     private _parameter_list: property_parameter_list;
     private _property_type: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_parameter_list: property_parameter_list; _property_type: type_definition; _index_expression: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property index_expression: expression read _index_expression write _index_expression;
    public property parameter_list: property_parameter_list read _parameter_list write _parameter_list;
    public property property_type: type_definition read _property_type write _property_type;

    // Fields
     

end;

property_parameter = class(syntax_tree_node)
    private _names: ident_list;
     private _type: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_names: ident_list; _type: type_definition); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property names: ident_list read _names write _names;
    public property &type: type_definition read _type write _type;

    // Fields
     

end;

property_parameter_list = class(syntax_tree_node)
    private _parameters: List<property_parameter>;
    // Methods
    public constructor Create; 
    public constructor Create(_parameters: List<property_parameter>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property parameters: List<property_parameter> read _parameters write _parameters;

    // Fields
     

end;

question_colon_expression = class(expression)
    private _condition: expression;
     private _ret_if_false: expression;
     private _ret_if_true: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_condition: expression; _ret_if_true: expression; _ret_if_false: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property condition: expression read _condition write _condition;
    public property ret_if_false: expression read _ret_if_false write _ret_if_false;
    public property ret_if_true: expression read _ret_if_true write _ret_if_true;

    // Fields
     

end;

 raise_statement = class(statement)
    private _excep: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_excep: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property excep: expression read _excep write _excep;

    // Fields
     

end;

raise_stmt = class(statement)
    private _address: expression;
     private _expr: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_expr: expression; _address: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property address: expression read _address write _address;
    public property expr: expression read _expr write _expr;

    // Fields
     

end;

read_accessor_name = class(syntax_tree_node)
    private _accessor_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_accessor_name: ident); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property accessor_name: ident read _accessor_name write _accessor_name;

    // Fields
     

end;

record_const = class(expression)
    private _rec_consts: List<record_const_definition>;
    // Methods
    public constructor Create; 
    public constructor Create(_rec_consts: List<record_const_definition>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property rec_consts: List<record_const_definition> read _rec_consts write _rec_consts;

    // Fields
     

end;

record_const_definition = class(statement)
    private _name: ident;
     private _val: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_name: ident; _val: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property name: ident read _name write _name;
    public property val: expression read _val write _val;

    // Fields
     

end;

record_type = class(type_definition)
    private _base_type: type_definition;
     private _parts: record_type_parts;
    // Methods
    public constructor Create; 
    public constructor Create(_parts: record_type_parts; _base_type: type_definition); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property base_type: type_definition read _base_type write _base_type;
    public property parts: record_type_parts read _parts write _parts;

    // Fields
     

end;

record_type_parts = class(syntax_tree_node)
    private _fixed_part: var_def_list;
     private _variant_part: variant_record_type;
    // Methods
    public constructor Create; 
    public constructor Create(_fixed_part: var_def_list; _variant_part: variant_record_type); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property fixed_part: var_def_list read _fixed_part write _fixed_part;
    public property variant_part: variant_record_type read _variant_part write _variant_part;

    // Fields
     

end;

ref_type = class(type_definition)
    private _pointed_to: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_pointed_to: type_definition); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property pointed_to: type_definition read _pointed_to write _pointed_to;

    // Fields
     

end;

repeat_node = class(statement)
    private _expr: expression;
     private _statements: statement;
    // Methods
    public constructor Create; 
    public constructor Create(_statements: statement; _expr: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property expr: expression read _expr write _expr;
    public property statements: statement read _statements write _statements;

    // Fields
     

end;

roof_dereference = class(dereference)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;

set_type_definition = class(type_definition)
    private _of_type: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_of_type: type_definition); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property of_type: type_definition read _of_type write _of_type;

    // Fields
     

end;

sharp_char_const = class(literal)
     private _char_num: Integer;
    // Methods
    public constructor Create; 
    public constructor Create(_char_num: Integer); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property char_num: Integer read _char_num write _char_num;

    // Fields
    

end;

simple_const_definition = class(const_definition)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;



sizeof_operator = class(addressed_value)
    private _expr: expression;
     private _type_def: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_type_def: type_definition; _expr: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property expr: expression read _expr write _expr;
    public property type_def: type_definition read _type_def write _type_def;

    // Fields
     

end;

 statement_list = class(statement)
    private _left_logical_bracket: syntax_tree_node;
     private _right_logical_bracket: syntax_tree_node;
     private _subnodes: List<statement>;
    // Methods
    public constructor Create; 
    public constructor Create(_subnodes: List<statement>; _left_logical_bracket: syntax_tree_node; _right_logical_bracket: syntax_tree_node); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property left_logical_bracket: syntax_tree_node read _left_logical_bracket write _left_logical_bracket;
    public property right_logical_bracket: syntax_tree_node read _right_logical_bracket write _right_logical_bracket;
    public property subnodes: List<statement> read _subnodes write _subnodes;

    // Fields
     

end;

string_const = class(literal)
    private _Value: string;
    // Methods
    public constructor Create; 
    public constructor Create(_Value: string); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property Value: string read _Value write _Value;

    // Fields
     

end;

string_num_definition = class(type_definition)
    private _name: ident;
     private _num_of_symbols: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_num_of_symbols: expression; _name: ident); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property name: ident read _name write _name;
    public property num_of_symbols: expression read _num_of_symbols write _num_of_symbols;

    // Fields
     

end;

subprogram_body = class(syntax_tree_node)
    private _subprogram_code: statement_list;
     private _subprogram_defs: declarations;
    // Methods
    public constructor Create; 
    public constructor Create(_subprogram_code: statement_list; _subprogram_defs: declarations); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property subprogram_code: statement_list read _subprogram_code write _subprogram_code;
    public property subprogram_defs: declarations read _subprogram_defs write _subprogram_defs;

    // Fields
     

end;

switch_stmt = class(statement)
    private _condition: expression;
     private _Part: SwitchPartType;
     private _stmt: statement;
    // Methods
    public constructor Create; 
    public constructor Create(_condition: expression; _stmt: statement; _Part: SwitchPartType); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property condition: expression read _condition write _condition;
    public property Part: SwitchPartType read _Part write _Part;
    public property stmt: statement read _stmt write _stmt;

    // Fields
     

end;

  
 
template_param_list = class(dereference)
    private _params_list: List<type_definition>;
    // Methods
    public constructor Create; 
    public constructor Create(_params_list: List<type_definition>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property params_list: List<type_definition> read _params_list write _params_list;

    // Fields
     

end;

template_type_reference = class(named_type_reference)
    private _name: named_type_reference;
     private _params_list: template_param_list;
    // Methods
    public constructor Create; 
    public constructor Create(_name: named_type_reference; _params_list: template_param_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property name: named_type_reference read _name write _name;
    public property params_list: template_param_list read _params_list write _params_list;

    // Fields
     

end;

token_taginfo = class(token_info)
    private _tag: object;
    // Methods
    public constructor Create; 
    public constructor Create(_tag: object); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property tag: object read _tag write _tag;

    // Fields
     

end;

try_except_statement = class(try_statement)
    private _else_statements: statement_list;
     private _on_except: on_exception_list;
    // Methods
    public constructor Create; 
    public constructor Create(_on_except: on_exception_list; _else_statements: statement_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property else_statements: statement_list read _else_statements write _else_statements;
    public property on_except: on_exception_list read _on_except write _on_except;

    // Fields
     

end;

try_finally_statement = class(try_statement)
    private _finally_statements: statement_list;
    // Methods
    public constructor Create; 
    public constructor Create(_finally_statements: statement_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property finally_statements: statement_list read _finally_statements write _finally_statements;

    // Fields
     

end;

try_handler = class(syntax_tree_node)
    // Methods
    public constructor Create; 
    public procedure visit(visitor: IVisitor); override; 

end;

try_handler_except = class(try_handler)
    private _except_block: exception_block;
    // Methods
    public constructor Create; 
    public constructor Create(_except_block: exception_block); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property except_block: exception_block read _except_block write _except_block;

    // Fields
     

end;

try_handler_finally = class(try_handler)
    private _stmt_list: statement_list;
    // Methods
    public constructor Create; 
    public constructor Create(_stmt_list: statement_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property stmt_list: statement_list read _stmt_list write _stmt_list;

    // Fields
     

end;

try_stmt = class(statement)
    private _handler: try_handler;
     private _stmt_list: statement_list;
    // Methods
    public constructor Create; 
    public constructor Create(_stmt_list: statement_list; _handler: try_handler); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property handler: try_handler read _handler write _handler;
    public property stmt_list: statement_list read _stmt_list write _stmt_list;

    // Fields
     

end;



type_declarations = class(declaration)
    private _types_decl: List<type_declaration>;
    // Methods
    public constructor Create; 
    public constructor Create(_types_decl: List<type_declaration>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property types_decl: List<type_declaration> read _types_decl write _types_decl;

    // Fields
     

end;



type_definition_attr = class(type_definition)
    private _attr: type_definition_attribute;
    // Methods
    public constructor Create; 
    public constructor Create(_attr: type_definition_attribute); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property attr: type_definition_attribute read _attr write _attr;

    // Fields
     

end;

type_definition_attr_list = class(syntax_tree_node)
    private _attributes: List<type_definition_attr>;
    // Methods
    public constructor Create; 
    public constructor Create(_attributes: List<type_definition_attr>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property attributes: List<type_definition_attr> read _attributes write _attributes;

    // Fields
     

end;

type_definition_list = class(syntax_tree_node)
    private _defs: List<type_definition>;
    // Methods
    public constructor Create; 
    public constructor Create(_defs: List<type_definition>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property defs: List<type_definition> read _defs write _defs;

    // Fields
     

end;

typecast_node = class(addressed_value)
     private _cast_op: op_typecast;
     private _expr: addressed_value;
     private _type_def: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_expr: addressed_value; _type_def: type_definition; _cast_op: op_typecast); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property cast_op: op_typecast read _cast_op write _cast_op;
    public property expr: addressed_value read _expr write _expr;
    public property type_def: type_definition read _type_def write _type_def;

    // Fields
    

end;

typed_const_definition = class(const_definition)
    private _const_type: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_const_type: type_definition); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property const_type: type_definition read _const_type write _const_type;

    // Fields
     

end;

typed_parametres = class(syntax_tree_node) 
    private _idents: ident_list;
     private _inital_value: expression;
     private _param_kind: parametr_kind;
     private _vars_type: type_definition;
     // Methods
    public constructor Create; 
    public constructor Create(_idents: ident_list; _vars_type: type_definition; _param_kind: parametr_kind; _inital_value: expression); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property idents: ident_list read _idents write _idents;
    public property inital_value: expression read _inital_value write _inital_value;
    public property param_kind: parametr_kind read _param_kind write _param_kind;
    public property vars_type: type_definition read _vars_type write _vars_type;

    // Fields
     

end;

typeof_operator = class(addressed_value)
    private _type_name: named_type_reference;
    // Methods
    public constructor Create; 
    public constructor Create(_type_name: named_type_reference); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property type_name: named_type_reference read _type_name write _type_name;

    // Fields
     

end;

uint64_const = class(const_node)
    private _val: UInt64;
    // Methods
    public constructor Create; 
    public constructor Create(_val: UInt64); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property val: UInt64 read _val write _val;

    // Fields
     

end;

un_expr = class(addressed_value)
    private _operation_type: Operators;
     private _subnode: expression;
    // Methods
    public constructor Create; 
    public constructor Create(_subnode: expression; _operation_type: Operators); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property operation_type: Operators read _operation_type write _operation_type;
    public property subnode: expression read _subnode write _subnode;

    // Fields
     

end;

unit_module = class(compilation_unit)
    private _finalization_part: statement_list;
     private _implementation_part: implementation_node;
     private _initialization_part: statement_list;
     private _interface_part: interface_node;
     private _unit_name: unit_name;
    // Methods
    public constructor Create; 
    public constructor Create(_unit_name: unit_name; _interface_part: interface_node; _implementation_part: implementation_node; _initialization_part: statement_list; _finalization_part: statement_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property finalization_part: statement_list read _finalization_part write _finalization_part;
    public property implementation_part: implementation_node read _implementation_part write _implementation_part;
    public property initialization_part: statement_list read _initialization_part write _initialization_part;
    public property interface_part: interface_node read _interface_part write _interface_part;
    public property unit_name: SyntaxTree.unit_name read _unit_name write _unit_name;

    // Fields
     

end;

unit_name = class(syntax_tree_node)
    private _HeaderKeyword: UnitHeaderKeyword;
     private _idunit_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_idunit_name: ident; _HeaderKeyword: UnitHeaderKeyword); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property HeaderKeyword: UnitHeaderKeyword read _HeaderKeyword write _HeaderKeyword;
    public property idunit_name: ident read _idunit_name write _idunit_name;

    // Fields
     

end;

uses_list = class(syntax_tree_node)
    private _units: List<unit_or_namespace>;
    // Methods
    public constructor Create; 
    public constructor Create(_units: List<unit_or_namespace>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property units: List<unit_or_namespace> read _units write _units;

    // Fields
     

end;

uses_unit_in = class(unit_or_namespace)
    private _in_file: string_const;
    // Methods
    public constructor Create; 
    public constructor Create(_in_file: string_const); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property in_file: string_const read _in_file write _in_file;

    // Fields
     

end;

using_list = class(syntax_tree_node)
    private _namespaces: List<unit_or_namespace>;
    // Methods
    public constructor Create; 
    public constructor Create(_namespaces: List<unit_or_namespace>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property namespaces: List<unit_or_namespace> read _namespaces write _namespaces;

    // Fields
     

end;

var_def_list = class(syntax_tree_node)
    private _vars: List<var_def_statement>;
    // Methods
    public constructor Create; 
    public constructor Create(_vars: List<var_def_statement>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property vars: List<var_def_statement> read _vars write _vars;

    // Fields
     

end;

var_def_statement = class(declaration)
    private _inital_value: expression;
     private _is_event: boolean;
     private _var_attr: type_definition_attribute;
     private _vars: ident_list;
     private _vars_type: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_vars: ident_list; _vars_type: type_definition; _inital_value: expression; _var_attr: type_definition_attribute; _is_event: boolean); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property inital_value: expression read _inital_value write _inital_value;
    public property is_event: boolean read _is_event write _is_event;
    public property var_attr: type_definition_attribute read _var_attr write _var_attr;
    public property vars: ident_list read _vars write _vars;
    public property vars_type: type_definition read _vars_type write _vars_type;

    // Fields
     

end;

var_statement = class(statement)
    private _var_def: var_def_statement;
    // Methods
    public constructor Create; 
    public constructor Create(_var_def: var_def_statement); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property var_def: var_def_statement read _var_def write _var_def;

    // Fields
     

end;

variable_definitions = class(declaration)
    private _var_definitions: List<var_def_statement>;
    // Methods
    public constructor Create; 
    public constructor Create(_var_definitions: List<var_def_statement>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property var_definitions: List<var_def_statement> read _var_definitions write _var_definitions;

    // Fields
     

end;

variant = class(syntax_tree_node)
    private _vars: ident_list;
     private _vars_type: type_definition;
    // Methods
    public constructor Create; 
    public constructor Create(_vars: ident_list; _vars_type: type_definition); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property vars: ident_list read _vars write _vars;
    public property vars_type: type_definition read _vars_type write _vars_type;

    // Fields
     

end;

variant_list = class(syntax_tree_node)
    private _vars: List<variant>;
    // Methods
    public constructor Create; 
    public constructor Create(_vars: List<variant>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property vars: List<variant> read _vars write _vars;

    // Fields
     

end;

variant_record_type = class(syntax_tree_node)
    private _var_name: ident;
     private _var_type: type_definition;
     private _vars: variant_types;
    // Methods
    public constructor Create; 
    public constructor Create(_var_name: ident; _var_type: type_definition; _vars: variant_types); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property var_name: ident read _var_name write _var_name;
    public property var_type: type_definition read _var_type write _var_type;
    public property vars: variant_types read _vars write _vars;

    // Fields
     

end;

variant_type = class(syntax_tree_node)
    private _case_exprs: expression_list;
     private _parts: record_type_parts;
    // Methods
    public constructor Create; 
    public constructor Create(_case_exprs: expression_list; _parts: record_type_parts); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property case_exprs: expression_list read _case_exprs write _case_exprs;
    public property parts: record_type_parts read _parts write _parts;

    // Fields
     

end;

variant_types = class(syntax_tree_node)
    private _vars: List<variant_type>;
    // Methods
    public constructor Create; 
    public constructor Create(_vars: List<variant_type>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property vars: List<variant_type> read _vars write _vars;

    // Fields
     

end;

where_definition = class(syntax_tree_node)
    private _constructor_specific_params: type_definition_list;
     private _names: ident_list;
     private _types: type_definition_list;
    // Methods
    public constructor Create; 
    public constructor Create(_names: ident_list; _types: type_definition_list; _constructor_specific_params: type_definition_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property constructor_specific_params: type_definition_list read _constructor_specific_params write _constructor_specific_params;
    public property names: ident_list read _names write _names;
    public property types: type_definition_list read _types write _types;

    // Fields
     

end;

where_definition_list = class(syntax_tree_node)
    private _defs: List<where_definition>;
    // Methods
    public constructor Create; 
    public constructor Create(_defs: List<where_definition>); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property defs: List<where_definition> read _defs write _defs;

    // Fields
     

end;

while_node = class(statement)
    private _CycleType: WhileCycleType;
     private _expr: expression;
     private _statements: statement;
    // Methods
    public constructor Create; 
    public constructor Create(_expr: expression; _statements: statement; _CycleType: WhileCycleType); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property CycleType: WhileCycleType read _CycleType write _CycleType;
    public property expr: expression read _expr write _expr;
    public property statements: statement read _statements write _statements;

    // Fields
     

end;

with_statement = class(statement)
    private _do_with: expression_list;
     private _what_do: statement;
    // Methods
    public constructor Create; 
    public constructor Create(_what_do: statement; _do_with: expression_list); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property do_with: expression_list read _do_with write _do_with;
    public property what_do: statement read _what_do write _what_do;

    // Fields
     

end;

write_accessor_name = class(syntax_tree_node)
    private _accessor_name: ident;
    // Methods
    public constructor Create; 
    public constructor Create(_accessor_name: ident); 
    public procedure visit(visitor: IVisitor); override; 

    // Properties
    public property accessor_name: ident read _accessor_name write _accessor_name;

    // Fields
     

end;

  
implementation

function file_position.get_column_num: Integer;
begin
  Result := self._column_num;
end;

function file_position.get_line_num: Integer;
begin
  Result := self._line_num;
end;

constructor file_position.Create(line_num: Integer; column_num: Integer);
begin
  self._line_num := line_num;
  self._column_num := column_num
end;


function SourceContext.get_begin_position: file_position;
begin
  Result := self._begin_position;
end;

function SourceContext.get_end_position: file_position;
begin
  Result := self._end_position;
end;

function SourceContext.get_FileName: string;
begin
  Result := self._file_name;
end;

procedure SourceContext.set_FileName(value: string);
begin
  self._file_name := value;
end;

function SourceContext.get_LeftSourceContext: SourceContext;
begin
  var sc := SourceContext.Create(self.begin_position.line_num, self.begin_position.column_num, self.begin_position.line_num, self.begin_position.column_num);
  sc.FileName := self.FileName;
  Result := sc
end;


function SourceContext.get_Length: Integer;
begin
  Result := self._end_symbol_position - self._begin_symbol_position;
end;

function SourceContext.get_Position: Integer;
begin
  Result := self._begin_symbol_position;
end;

function SourceContext.get_RightSourceContext: SourceContext;
begin
  var sc := SourceContext.Create(self.end_position.line_num, self.end_position.column_num, self.end_position.line_num, self.end_position.column_num);
  sc.FileName := self.FileName;
  Result := sc
end;

constructor SourceContext.Create(left: SourceContext; right: SourceContext);
begin
    self._file_name := nil;
    if (left <> nil) then
        self._begin_position := file_position.Create(left.begin_position.line_num, left.begin_position.column_num);
    if (right <> nil) then
        self._end_position := file_position.Create(right.end_position.line_num, right.end_position.column_num);
    if (left = nil) then
        self._begin_position := self._end_position;
    if (right = nil) then
        self._end_position := self._begin_position;
    self._begin_symbol_position := left._begin_symbol_position;
    self._end_symbol_position := right._end_symbol_position;
    self.FileName :=  (left.FileName = right.FileName)?left.FileName:nil;
end;

constructor SourceContext.Create(beg_line_num: Integer; beg_column_num: Integer; end_line_num: Integer; end_column_num: Integer);
begin
  self._file_name := nil;
  self._begin_position := file_position.Create(beg_line_num, beg_column_num);
  self._end_position := file_position.Create(end_line_num, end_column_num);
  self._begin_symbol_position := 0;
  self._end_symbol_position := 0
end;

constructor SourceContext.Create(beg_line_num: Integer; beg_column_num: Integer; end_line_num: Integer; end_column_num: Integer; _begin_symbol_position: Integer; _end_symbol_position: Integer);
begin
  self._file_name := nil;
  self._begin_position := file_position.Create(beg_line_num, beg_column_num);
  self._end_position := file_position.Create(end_line_num, end_column_num);
  self._begin_symbol_position := _begin_symbol_position;
  self._end_symbol_position := _end_symbol_position
end;

function SourceContext.&In(sc: SourceContext): boolean;
begin
  Result := ((((self.begin_position.line_num >= sc.begin_position.line_num) and (self.end_position.line_num <= sc.end_position.line_num)) and ((self.begin_position.line_num <> sc.begin_position.line_num) or (self.begin_position.column_num >= sc.begin_position.column_num))) and ((self.end_position.line_num <> sc.end_position.line_num) or (self.end_position.column_num <= sc.end_position.column_num)));
end;

function SourceContext.ToString: string;
begin
  //Result := string.Format('[({0},{1})-({2},{3})]', New(array[4] of TObject, ( ( self.begin_position.line_num, self.begin_position.column_num, self.end_position.line_num, self.end_position.column_num ) )))
end;

constructor syntax_tree_node.Create;
begin
  
end;

constructor syntax_tree_node.Create(_source_context: SourceContext);
begin
  self._source_context := _source_context;
end;

procedure syntax_tree_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor addressed_value.Create;
begin
  
end;

procedure addressed_value.visit(visitor: IVisitor);
begin
 visitor.visit(self); 
end;


constructor addressed_value_funcname.Create;
begin
  
end;

procedure addressed_value_funcname.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor array_const.Create;
begin
  
end;

constructor array_const.Create(_elements: expression_list);
begin
  self._elements := _elements;
end;

procedure array_const.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor array_of_const_type_definition.Create;
begin
  
end;

procedure array_of_const_type_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor array_of_named_type_definition.Create;
begin
  
end;

constructor array_of_named_type_definition.Create(_type_name: named_type_reference);
begin
  self._type_name := _type_name;
end;

procedure array_of_named_type_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor array_size.Create;
begin
  
end;

constructor array_size.Create(_max_value: expression);
begin
  self._max_value := _max_value;
end;

procedure array_size.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor array_type.Create;
begin
  
end;

constructor array_type.Create(_indexers: indexers_types; _elemets_types: type_definition);
begin
  self._indexers := _indexers;
  self._elemets_types := _elemets_types;
end;

procedure array_type.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor assign.Create;
begin
  
end;

constructor assign.Create(_to: addressed_value; _from: expression; _operator_type: Operators);
begin
  self._to := _to;
  self._from := _from;
  self._operator_type := _operator_type;
end;

procedure assign.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor bin_expr.Create;
begin
  
end;

constructor bin_expr.Create(_left: expression; _right: expression; _operation_type: Operators);
begin
  self._left := _left;
  self._right := _right;
end;

procedure bin_expr.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor block.Create;
begin
  
end;

constructor block.Create(_defs: declarations; _program_code: statement_list);
begin
  self._defs := _defs;
  self._program_code := _program_code;
end;

procedure block.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor bool_const.Create;
begin
  
end;

constructor bool_const.Create(_val: boolean);
begin
  self._val := _val;
end;

procedure bool_const.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor c_for_cycle.Create;
begin
  
end;

constructor c_for_cycle.Create(_expr1: statement; _expr2: expression; _expr3: expression; _stmt: statement);
begin
  self._expr1 := _expr1;
  self._expr2 := _expr2;
  self._expr3 := _expr3;
  self._stmt := _stmt;
end;

procedure c_for_cycle.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor c_module.Create;
begin
  
end;

constructor c_module.Create(_defs: declarations; _used_units: uses_list);
begin
  self._defs := _defs;
  self._used_units := _used_units;
end;

procedure c_module.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor c_scalar_type.Create;
begin
  
end;

constructor c_scalar_type.Create(_scalar_name: c_scalar_type_name; _sign: c_scalar_sign);
begin
  self._scalar_name := _scalar_name;
  self._sign := _sign
end;

procedure c_scalar_type.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor case_node.Create;
begin
  
end;

constructor case_node.Create(_param: expression; _conditions: case_variants; _else_statement: statement);
begin
  self._param := _param;
  self._conditions := _conditions;
  self._else_statement := _else_statement
end;

procedure case_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor case_variant.Create;
begin
  
end;

constructor case_variant.Create(_conditions: expression_list; _exec_if_true: statement);
begin
  self._conditions := _conditions;
  self._exec_if_true := _exec_if_true
end;

procedure case_variant.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor case_variants.Create;
begin
  self._variants := new List<case_variant>();
end;

constructor case_variants.Create(_variants: List<case_variant>);
begin
  self._variants := _variants
end;

procedure case_variants.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor char_const.Create;
begin
  
end;

constructor char_const.Create(_cconst: Char);
begin
  self._cconst := _cconst;
end;

procedure char_const.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor class_body.Create;
begin
  self._class_def_blocks := new List<class_members>();
end;

constructor class_body.Create(_class_def_blocks: List<class_members>);
begin
  self._class_def_blocks := _class_def_blocks
end;

procedure class_body.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor class_definition.Create;
begin
  
end;

constructor class_definition.Create(_class_parents: named_type_reference_list; _body: class_body; _keyword: class_keyword; _template_args: ident_list; _where_section: where_definition_list; _attribute: class_attribute);
begin
  self._class_parents := _class_parents;
  self._body := _body;
  self._keyword := _keyword;
  self._template_args := _template_args;
  self._where_section := _where_section;
  self._attribute := _attribute
end;

procedure class_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor class_members.Create;
begin
  self._members := new List<declaration>();
end;

constructor class_members.Create(_members: List<declaration>; _access_mod: access_modifer_node);
begin
  self._members := _members;
  self._access_mod := _access_mod
end;

procedure class_members.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor class_predefinition.Create;
begin
  
end;

constructor class_predefinition.Create(_class_name: ident);
begin
  self._class_name := _class_name;
end;

procedure class_predefinition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor compilation_unit.Create;
begin
  self._compiler_directives := new List<compiler_directive>();
end;

constructor compilation_unit.Create(_file_name: string; _compiler_directives: List<compiler_directive>; _Language: LanguageId);
begin
  self._file_name := _file_name;
  self._compiler_directives := _compiler_directives;
  self._Language := _Language
end;

procedure compilation_unit.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor compiler_directive.Create;
begin
  
end;

constructor compiler_directive.Create(_Name: token_info; _Directive: token_info);
begin
  self._Name := _Name;
  self._Directive := _Directive
end;

procedure compiler_directive.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor compiler_directive_if.Create;
begin
  
end;

constructor compiler_directive_if.Create(_if_part: compiler_directive; _elseif_part: compiler_directive);
begin
  self._if_part := _if_part;
  self._elseif_part := _elseif_part
end;

procedure compiler_directive_if.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor compiler_directive_list.Create;
begin
  
end;

constructor compiler_directive_list.Create(_directives: List<compiler_directive>);
begin
  self._directives := _directives;
end;

procedure compiler_directive_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor const_definition.Create;
begin
  
end;

constructor const_definition.Create(_const_name: ident; _const_value: expression);
begin
  self._const_name := _const_name;
  self._const_value := _const_value
end;

procedure const_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor const_node.Create;
begin
  
end;

procedure const_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor &constructor.Create;
begin
  
end;

procedure &constructor.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor consts_definitions_list.Create;
begin
  self._const_defs := new List<const_definition>();
end;

constructor consts_definitions_list.Create(_const_defs: List<const_definition>);
begin
  self._const_defs := _const_defs;
end;

procedure consts_definitions_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor declaration.Create;
begin
  
end;

procedure declaration.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor declarations.Create;
begin
  self._defs := new List<declaration>();
end;

constructor declarations.Create(_defs: List<declaration>);
begin
  self._defs := _defs;
end;

procedure declarations.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor declarations_as_statement.Create;
begin
  
end;

constructor declarations_as_statement.Create(_defs: declarations);
begin
  self._defs := _defs;
end;

procedure declarations_as_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor default_indexer_property_node.Create;
begin
  
end;

procedure default_indexer_property_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor dereference.Create;
begin
  
end;

constructor dereference.Create(_dereferencing_value: addressed_value);
begin
  self._dereferencing_value := _dereferencing_value;
end;

procedure dereference.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor &destructor.Create;
begin
  
end;

procedure &destructor.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor diap_expr.Create;
begin
  
end;

constructor diap_expr.Create(_left: expression; _right: expression);
begin
  self._left := _left;
  self._right := _right;
end;

procedure diap_expr.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor diapason.Create;
begin
  
end;

constructor diapason.Create(_left: expression; _right: expression);
begin
  self._left := _left;
  self._right := _right;
end;

procedure diapason.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor diapason_expr.Create;
begin
  
end;

constructor diapason_expr.Create(_left: expression; _right: expression);
begin
  self._left := _left;
  self._right := _right;  
end;

procedure diapason_expr.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor documentation_comment_list.Create;
begin
  self._sections := new List<documentation_comment_section>();
end;

constructor documentation_comment_list.Create(_sections: List<documentation_comment_section>);
begin
  self._sections := _sections;
end;

procedure documentation_comment_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor documentation_comment_section.Create;
begin
  self._tags := new List<documentation_comment_tag>();
end;

constructor documentation_comment_section.Create(_tags: List<documentation_comment_tag>; _text: string);
begin
  self._tags := _tags;
  self._text := _text;
end;

procedure documentation_comment_section.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor documentation_comment_tag.Create;
begin
  self._parameters := new List<documentation_comment_tag_param>()
end;

constructor documentation_comment_tag.Create(_name: string; _parameters: List<documentation_comment_tag_param>; _text: string);
begin
  self._name := _name;
  self._parameters := _parameters;
  self._text := _text
end;

procedure documentation_comment_tag.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor documentation_comment_tag_param.Create;
begin
  
end;

constructor documentation_comment_tag_param.Create(_name: string; _value: string);
begin
  self._name := _name;
  self._value := _value;
end;

procedure documentation_comment_tag_param.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor dot_node.Create;
begin
  
end;

constructor dot_node.Create(_left: addressed_value; _right: addressed_value);
begin
  self._left := _left;
  self._right := _right;
end;

procedure dot_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor double_const.Create;
begin
  
end;

constructor double_const.Create(_val: Double);
begin
  self._val := _val;
end;

procedure double_const.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor empty_statement.Create;
begin
  
end;

procedure empty_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor enum_type_definition.Create;
begin
  
end;

constructor enum_type_definition.Create(_enumerators: enumerator_list);
begin
  self._enumerators := _enumerators;
end;

procedure enum_type_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor enumerator.Create;
begin
  
end;

constructor enumerator.Create(_name: ident; _value: expression);
begin
  self._name := _name;
  self._value := _value;
end;

procedure enumerator.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor enumerator_list.Create;
begin
  self._enumerators := new List<enumerator>();
end;

constructor enumerator_list.Create(_enumerators: List<enumerator>);
begin
  self._enumerators := _enumerators;
end;

procedure enumerator_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor exception_block.Create;
begin
  
end;

constructor exception_block.Create(_stmt_list: statement_list; _handlers: exception_handler_list; _else_stmt_list: statement_list);
begin
  self._stmt_list := _stmt_list;
  self._handlers := _handlers;
  self._else_stmt_list := _else_stmt_list
end;

procedure exception_block.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor exception_handler.Create;
begin
  
end;

constructor exception_handler.Create(_variable: ident; _type_name: named_type_reference; _statements: statement);
begin
  self._variable := _variable;
  self._type_name := _type_name;
  self._statements := _statements
end;

procedure exception_handler.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor exception_handler_list.Create;
begin
  self._handlers := new List<exception_handler>();
end;

constructor exception_handler_list.Create(_handlers: List<exception_handler>);
begin
  self._handlers := _handlers;
end;

procedure exception_handler_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor exception_ident.Create;
begin
  
end;

constructor exception_ident.Create(_variable: ident; _type_name: named_type_reference);
begin
  self._variable := _variable;
  self._type_name := _type_name;
end;

procedure exception_ident.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor expression.Create;
begin
  
end;

procedure expression.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor expression_as_statement.Create;
begin
  
end;

constructor expression_as_statement.Create(_expr: expression);
begin
  self._expr := _expr;
end;

procedure expression_as_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor expression_list.Create;
begin
  self._expressions := new List<expression>();
end;

constructor expression_list.Create(_expressions: List<expression>);
begin
  self._expressions := _expressions;
end;

procedure expression_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor external_directive.Create;
begin
  
end;

constructor external_directive.Create(_modulename: expression; _name: expression);
begin
  self._modulename := _modulename;
  self._name := _name
end;

procedure external_directive.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor file_type.Create;
begin
  
end;

constructor file_type.Create(_file_of_type: type_definition);
begin
  self._file_of_type := _file_of_type
end;

procedure file_type.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor file_type_definition.Create;
begin
  
end;

constructor file_type_definition.Create(_elem_type: type_definition);
begin
  self._elem_type := _elem_type;
end;

procedure file_type_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor for_node.Create;
begin
  
end;

constructor for_node.Create(_loop_variable: ident; _initial_value: expression; _finish_value: expression; _statements: statement; _cycle_type: for_cycle_type; _increment_value: expression; _type_name: type_definition; _create_loop_variable: boolean);
begin
  self._loop_variable := _loop_variable;
  self._initial_value := _initial_value;
  self._finish_value := _finish_value;
  self._statements := _statements;
  self._cycle_type := _cycle_type;
  self._increment_value := _increment_value;
  self._type_name := _type_name;
  self._create_loop_variable := _create_loop_variable
end;

procedure for_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor foreach_stmt.Create;
begin
  
end;

constructor foreach_stmt.Create(_identifier: ident; _type_name: type_definition; _in_what: expression; _stmt: statement);
begin
  self._identifier := _identifier;
  self._type_name := _type_name;
  self._in_what := _in_what;
  self._stmt := _stmt
end;

procedure foreach_stmt.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor formal_parametres.Create;
begin
  self._params_list := new List<typed_parametres>();
end;

constructor formal_parametres.Create(_params_list: List<typed_parametres>);
begin
  self._params_list := _params_list;
end;

procedure formal_parametres.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor format_expr.Create;
begin
  
end;

constructor format_expr.Create(_expr: expression; _format1: expression; _format2: expression);
begin
  self._expr := _expr;
  self._format1 := _format1;
  self._format2 := _format2
end;

procedure format_expr.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor function_header.Create;
begin
  
end;

constructor function_header.Create(_return_type: type_definition);
begin
  self._return_type := _return_type
end;

procedure function_header.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor get_address.Create;
begin
  
end;

constructor get_address.Create(_address_of: addressed_value);
begin
  self._address_of := _address_of
end;

procedure get_address.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor goto_statement.Create;
begin
  
end;

constructor goto_statement.Create(_label: ident);
begin
  self._label := _label;
end;

procedure goto_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor hex_constant.Create;
begin
  
end;

procedure hex_constant.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor ident.Create;
begin
  
end;

constructor ident.Create(_name: string);
begin
  self._name := _name
end;

procedure ident.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor ident_list.Create;
begin
  self._idents := new List<ident>();
end;

constructor ident_list.Create(_idents: List<ident>);
begin
  self._idents := _idents;
end;

procedure ident_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor if_node.Create;
begin
  
end;

constructor if_node.Create(_condition: expression; _then_body: statement; _else_body: statement);
begin
  self._condition := _condition;
  self._then_body := _then_body;
  self._else_body := _else_body
end;

procedure if_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor implementation_node.Create;
begin
  
end;

constructor implementation_node.Create(_uses_modules: uses_list; _implementation_definitions: declarations; _using_namespaces: using_list);
begin
  self._uses_modules := _uses_modules;
  self._implementation_definitions := _implementation_definitions;
  self._using_namespaces := _using_namespaces
end;

procedure implementation_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor index_property.Create;
begin
  
end;

constructor index_property.Create(_property_parametres: formal_parametres; _is_default: default_indexer_property_node);
begin
  self._property_parametres := _property_parametres;
  self._is_default := _is_default
end;

procedure index_property.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor indexer.Create;
begin
  
end;

constructor indexer.Create(_indexes: expression_list);
begin
  self._indexes := _indexes
end;

procedure indexer.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor indexers_types.Create;
begin
  self._indexers := new List<type_definition>()
end;

constructor indexers_types.Create(_indexers: List<type_definition>);
begin
  self._indexers := _indexers
end;

procedure indexers_types.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor inherited_ident.Create;
begin
  
end;

procedure inherited_ident.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor inherited_message.Create;
begin
  
end;

procedure inherited_message.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor inherited_method_call.Create;
begin
  
end;

constructor inherited_method_call.Create(_method_name: ident; _exprs: expression_list);
begin
  self._method_name := _method_name;
  self._exprs := _exprs
end;

procedure inherited_method_call.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor initfinal_part.Create;
begin
  
end;

constructor initfinal_part.Create(_initialization_sect: statement_list; _finalization_sect: statement_list);
begin
  self._initialization_sect := _initialization_sect;
  self._finalization_sect := _finalization_sect
end;

procedure initfinal_part.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor int32_const.Create;
begin
  
end;

constructor int32_const.Create(_val: Integer);
begin
  self._val := _val;
end;

procedure int32_const.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor int64_const.Create;
begin
  
end;

constructor int64_const.Create(_val: Int64);
begin
  self._val := _val;
end;

procedure int64_const.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor interface_node.Create;
begin
  
end;

constructor interface_node.Create(_interface_definitions: declarations; _uses_modules: uses_list; _using_namespaces: using_list);
begin
  self._interface_definitions := _interface_definitions;
  self._uses_modules := _uses_modules;
  self._using_namespaces := _using_namespaces
end;

procedure interface_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor jump_stmt.Create;
begin
  
end;

constructor jump_stmt.Create(_expr: expression; _JumpType: JumpStmtType);
begin
  self._expr := _expr;
  self._JumpType := _JumpType
end;

procedure jump_stmt.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor known_type_definition.Create;
begin
  
end;

constructor known_type_definition.Create(_tp: known_type; _unit_name: ident);
begin
  self._tp := _tp;
  self._unit_name := _unit_name
end;

procedure known_type_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor known_type_ident.Create;
begin
  
end;

constructor known_type_ident.Create(_type: known_type);
begin
  self._type := _type
end;

procedure known_type_ident.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor label_definitions.Create;
begin
  
end;

constructor label_definitions.Create(_labels: ident_list);
begin
  self._labels := _labels;
end;

procedure label_definitions.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor labeled_statement.Create;
begin
  
end;

constructor labeled_statement.Create(_label_name: ident; _to_statement: statement);
begin
  self._label_name := _label_name;
  self._to_statement := _to_statement
end;

procedure labeled_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor literal.Create;
begin
  
end;

procedure literal.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor literal_const_line.Create;
begin
  self._literals := new List<literal>();
end;

constructor literal_const_line.Create(_literals: List<literal>);
begin
  self._literals := _literals;
end;

procedure literal_const_line.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor lock_stmt.Create;
begin
  
end;

constructor lock_stmt.Create(_lock_object: expression; _stmt: statement);
begin
  self._lock_object := _lock_object;
  self._stmt := _stmt
end;

procedure lock_stmt.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor loop_stmt.Create;
begin
  
end;

constructor loop_stmt.Create(_stmt: statement);
begin
  self._stmt := _stmt;
end;

procedure loop_stmt.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor method_call.Create;
begin
  
end;

constructor method_call.Create(_parametres: expression_list);
begin
  self._parametres := _parametres;
end;

procedure method_call.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor method_name.Create;
begin
  
end;

constructor method_name.Create(_class_name: ident; _meth_name: ident);
begin
  self._class_name := _class_name;
  self._meth_name := _meth_name
end;

procedure method_name.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor named_type_reference.Create;
begin
  self._names := new List<ident>();
end;

constructor named_type_reference.Create(_names: List<ident>);
begin
  self._names := _names;
end;

procedure named_type_reference.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor named_type_reference_list.Create;
begin
  self._types := new List<named_type_reference>();  
end;

constructor named_type_reference_list.Create(_types: List<named_type_reference>);
begin
  self._types := _types
end;

procedure named_type_reference_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor new_expr.Create;
begin
  
end;

constructor new_expr.Create(_type: type_definition; _params_list: expression_list; _new_array: boolean);
begin
  self._type := _type;
  self._params_list := _params_list;
  self._new_array := _new_array
end;

procedure new_expr.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor nil_const.Create;
begin
  
end;

procedure nil_const.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor oberon_exit_stmt.Create;
begin
  
end;

constructor oberon_exit_stmt.Create(_text: string);
begin
  self._text := _text;
end;

procedure oberon_exit_stmt.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor oberon_ident_with_export_marker.Create;
begin
  
end;

constructor oberon_ident_with_export_marker.Create(_marker: oberon_export_marker);
begin
  self._marker := _marker;
end;

procedure oberon_ident_with_export_marker.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor oberon_import_module.Create;
begin
  
end;

constructor oberon_import_module.Create(_new_name: ident);
begin
  self._new_name := _new_name;
end;

procedure oberon_import_module.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor oberon_module.Create;
begin
  
end;

constructor oberon_module.Create(_first_name: ident; _second_name: ident; _import_list: uses_list; _definitions: declarations; _module_code: statement_list);
begin
  self._first_name := _first_name;
  self._second_name := _second_name;
  self._import_list := _import_list;
  self._definitions := _definitions;
  self._module_code := _module_code
end;

procedure oberon_module.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor oberon_procedure_header.Create;
begin
  
end;

constructor oberon_procedure_header.Create(_receiver: oberon_procedure_receiver; _first_name: ident; _second_name: ident);
begin
  self._receiver := _receiver;
  self._first_name := _first_name;
  self._second_name := _second_name
end;

procedure oberon_procedure_header.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor oberon_procedure_receiver.Create;
begin
  
end;

constructor oberon_procedure_receiver.Create(_param_kind: parametr_kind; _receiver_name: ident; _receiver_typename: ident);
begin
  self._param_kind := _param_kind;
  self._receiver_name := _receiver_name;
  self._receiver_typename := _receiver_typename
end;

procedure oberon_procedure_receiver.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor oberon_withstmt.Create;
begin
  
end;

constructor oberon_withstmt.Create(_quardstat_list: oberon_withstmt_guardstat_list; _else_stmt: statement);
begin
  self._quardstat_list := _quardstat_list;
  self._else_stmt := _else_stmt
end;

procedure oberon_withstmt.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor oberon_withstmt_guardstat.Create;
begin
  
end;

constructor oberon_withstmt_guardstat.Create(_name: addressed_value; _type_name: type_definition; _stmt: statement);
begin
  self._name := _name;
  self._type_name := _type_name;
  self._stmt := _stmt
end;

procedure oberon_withstmt_guardstat.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor oberon_withstmt_guardstat_list.Create;
begin
  self._guardstats := new List<oberon_withstmt_guardstat>();  
end;

constructor oberon_withstmt_guardstat_list.Create(_guardstats: List<oberon_withstmt_guardstat>);
begin
  self._guardstats := _guardstats
end;

procedure oberon_withstmt_guardstat_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor on_exception.Create;
begin
  
end;

constructor on_exception.Create(_exception_var_name: ident; _exception_type_name: ident; _stat: statement);
begin
  self._exception_var_name := _exception_var_name;
  self._exception_type_name := _exception_type_name;
  self._stat := _stat
end;

procedure on_exception.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor on_exception_list.Create;
begin
  self._on_exceptions := new List<on_exception>();  
end;

constructor on_exception_list.Create(_on_exceptions: List<on_exception>);
begin
  self._on_exceptions := _on_exceptions
end;

procedure on_exception_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor op_type_node.Create;
begin
  
end;

constructor op_type_node.Create(_type: Operators);
begin
  self._type := _type;
end;

procedure op_type_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor operator_name_ident.Create;
begin
  
end;

constructor operator_name_ident.Create(_operator_type: Operators);
begin
  self._operator_type := _operator_type;
end;

procedure operator_name_ident.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor pascal_set_constant.Create;
begin
  
end;

constructor pascal_set_constant.Create(_values: expression_list);
begin
  self._values := _values;
end;

procedure pascal_set_constant.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor proc_block.Create;
begin
  
end;

procedure proc_block.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor procedure_attribute.Create;
begin
  
end;

constructor procedure_attribute.Create(_attribute_type: proc_attribute);
begin
  self._attribute_type := _attribute_type;
end;

procedure procedure_attribute.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor procedure_attributes_list.Create;
begin
  self._proc_attributes := new List<procedure_attribute>();  
end;

constructor procedure_attributes_list.Create(_proc_attributes: List<procedure_attribute>);
begin
  self._proc_attributes := _proc_attributes
end;

procedure procedure_attributes_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor procedure_call.Create;
begin
  
end;

constructor procedure_call.Create(_func_name: addressed_value);
begin
  self._func_name := _func_name;
end;

procedure procedure_call.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor procedure_definition.Create;
begin
  
end;

constructor procedure_definition.Create(_proc_header: procedure_header; _proc_body: proc_block);
begin
  self._proc_header := _proc_header;
  self._proc_body := _proc_body
end;

procedure procedure_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor procedure_header.Create;
begin
  
end;

constructor procedure_header.Create(_parametres: formal_parametres; _proc_attributes: procedure_attributes_list; _name: method_name; _of_object: boolean; _class_keyword: boolean);
begin
  self._parametres := _parametres;
  self._proc_attributes := _proc_attributes;
  self._name := _name;
  self._of_object := _of_object;
  self._class_keyword := _class_keyword
end;

procedure procedure_header.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor program_body.Create;
begin
  
end;

constructor program_body.Create(_used_units: uses_list; _program_definitions: declarations; _program_code: statement_list; _using_list: SyntaxTree.using_list);
begin
  self._used_units := _used_units;
  self._program_definitions := _program_definitions;
  self._program_code := _program_code;
  self._using_list := _using_list
end;

procedure program_body.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor program_module.Create;
begin
  
end;

constructor program_module.Create(_program_name: SyntaxTree.program_name; _used_units: uses_list; _program_block: block; _using_namespaces: using_list);
begin
  self._program_name := _program_name;
  self._used_units := _used_units;
  self._program_block := _program_block;
  self._using_namespaces := _using_namespaces
end;

procedure program_module.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor program_name.Create;
begin
  
end;

constructor program_name.Create(_prog_name: ident);
begin
  self._prog_name := _prog_name;
end;

procedure program_name.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor program_tree.Create;
begin
  self._compilation_units := new List<compilation_unit>();  
end;

constructor program_tree.Create(_compilation_units: List<compilation_unit>);
begin
  self._compilation_units := _compilation_units
end;

procedure program_tree.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor property_accessors.Create;
begin
  
end;

constructor property_accessors.Create(_read_accessor: read_accessor_name; _write_accessor: write_accessor_name);
begin
  self._read_accessor := _read_accessor;
  self._write_accessor := _write_accessor
end;

procedure property_accessors.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor property_array_default.Create;
begin
  
end;

procedure property_array_default.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor property_interface.Create;
begin
  
end;

constructor property_interface.Create(_parameter_list: property_parameter_list; _property_type: type_definition; _index_expression: expression);
begin
  self._parameter_list := _parameter_list;
  self._property_type := _property_type;
  self._index_expression := _index_expression
end;

procedure property_interface.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor property_parameter.Create;
begin
  
end;

constructor property_parameter.Create(_names: ident_list; _type: type_definition);
begin
  self._names := _names;
  self._type := _type
end;

procedure property_parameter.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor property_parameter_list.Create;
begin
  self._parameters := new List<property_parameter>()
end;

constructor property_parameter_list.Create(_parameters: List<property_parameter>);
begin
  self._parameters := _parameters;
end;

procedure property_parameter_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor question_colon_expression.Create;
begin
  
end;

constructor question_colon_expression.Create(_condition: expression; _ret_if_true: expression; _ret_if_false: expression);
begin
  self._condition := _condition;
  self._ret_if_true := _ret_if_true;
  self._ret_if_false := _ret_if_false
end;

procedure question_colon_expression.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor raise_statement.Create;
begin
  
end;

constructor raise_statement.Create(_excep: expression);
begin
  self._excep := _excep;
end;

procedure raise_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor raise_stmt.Create;
begin
  
end;

constructor raise_stmt.Create(_expr: expression; _address: expression);
begin
  self._expr := _expr;
  self._address := _address
end;

procedure raise_stmt.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor read_accessor_name.Create;
begin
  
end;

constructor read_accessor_name.Create(_accessor_name: ident);
begin
  self._accessor_name := _accessor_name;
end;

procedure read_accessor_name.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor record_const.Create;
begin
  self._rec_consts := new List<record_const_definition>();  
end;

constructor record_const.Create(_rec_consts: List<record_const_definition>);
begin
  self._rec_consts := _rec_consts
end;

procedure record_const.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor record_const_definition.Create;
begin
  
end;

constructor record_const_definition.Create(_name: ident; _val: expression);
begin
  self._name := _name;
  self._val := _val
end;

procedure record_const_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor record_type.Create;
begin
  
end;

constructor record_type.Create(_parts: record_type_parts; _base_type: type_definition);
begin
  self._parts := _parts;
  self._base_type := _base_type
end;

procedure record_type.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor record_type_parts.Create;
begin
  
end;

constructor record_type_parts.Create(_fixed_part: var_def_list; _variant_part: variant_record_type);
begin
  self._fixed_part := _fixed_part;
  self._variant_part := _variant_part
end;

procedure record_type_parts.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor ref_type.Create;
begin
  
end;

constructor ref_type.Create(_pointed_to: type_definition);
begin
  self._pointed_to := _pointed_to;
end;

procedure ref_type.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor repeat_node.Create;
begin
  
end;

constructor repeat_node.Create(_statements: statement; _expr: expression);
begin
  self._statements := _statements;
  self._expr := _expr
end;

procedure repeat_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor roof_dereference.Create;
begin
  
end;

procedure roof_dereference.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor set_type_definition.Create;
begin
  
end;

constructor set_type_definition.Create(_of_type: type_definition);
begin
  self._of_type := _of_type;
end;

procedure set_type_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor sharp_char_const.Create;
begin
  
end;

constructor sharp_char_const.Create(_char_num: Integer);
begin
  self._char_num := _char_num;
end;

procedure sharp_char_const.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor simple_const_definition.Create;
begin
  
end;

procedure simple_const_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor simple_property.Create;
begin
  
end;

constructor simple_property.Create(_property_name: ident; _property_type: type_definition; _index_expression: expression; _accessors: property_accessors; _array_default: property_array_default; _parameter_list: property_parameter_list);
begin
  self._property_name := _property_name;
  self._property_type := _property_type;
  self._index_expression := _index_expression;
  self._accessors := _accessors;
  self._array_default := _array_default;
  self._parameter_list := _parameter_list
end;

procedure simple_property.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor sizeof_operator.Create;
begin
  
end;

constructor sizeof_operator.Create(_type_def: type_definition; _expr: expression);
begin
  self._type_def := _type_def;
  self._expr := _expr
end;

procedure sizeof_operator.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor statement.Create;
begin
  
end;

procedure statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor statement_list.Create;
begin
  self._subnodes := new List<statement>();  
end;

constructor statement_list.Create(_subnodes: List<statement>; _left_logical_bracket: syntax_tree_node; _right_logical_bracket: syntax_tree_node);
begin
  self._subnodes := _subnodes;
  self._left_logical_bracket := _left_logical_bracket;
  self._right_logical_bracket := _right_logical_bracket
end;

procedure statement_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor string_const.Create;
begin
  
end;

constructor string_const.Create(_Value: string);
begin
  self._Value := _Value;
end;

procedure string_const.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor string_num_definition.Create;
begin
  
end;

constructor string_num_definition.Create(_num_of_symbols: expression; _name: ident);
begin
  self._num_of_symbols := _num_of_symbols;
  self._name := _name
end;

procedure string_num_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor subprogram_body.Create;
begin
  
end;

constructor subprogram_body.Create(_subprogram_code: statement_list; _subprogram_defs: declarations);
begin
  self._subprogram_code := _subprogram_code;
  self._subprogram_defs := _subprogram_defs
end;

procedure subprogram_body.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor switch_stmt.Create;
begin
  
end;

constructor switch_stmt.Create(_condition: expression; _stmt: statement; _Part: SwitchPartType);
begin
  self._condition := _condition;
  self._stmt := _stmt;
  self._Part := _Part
end;

procedure switch_stmt.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor template_param_list.Create;
begin
  self._params_list := new List<type_definition>();  
end;

constructor template_param_list.Create(_params_list: List<type_definition>);
begin
  self._params_list := _params_list
end;

procedure template_param_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor template_type_reference.Create;
begin
  
end;

constructor template_type_reference.Create(_name: named_type_reference; _params_list: template_param_list);
begin
  self._name := _name;
  self._params_list := _params_list
end;

procedure template_type_reference.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor token_info.Create;
begin
  
end;

constructor token_info.Create(_text: string);
begin
  self._text := _text;
end;

procedure token_info.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor token_taginfo.Create;
begin
  
end;

constructor token_taginfo.Create(_tag: object);
begin
  self._tag := _tag;
end;

procedure token_taginfo.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor try_except_statement.Create;
begin
  
end;

constructor try_except_statement.Create(_on_except: on_exception_list; _else_statements: statement_list);
begin
  self._on_except := _on_except;
  self._else_statements := _else_statements
end;

procedure try_except_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor try_finally_statement.Create;
begin
  
end;

constructor try_finally_statement.Create(_finally_statements: statement_list);
begin
  self._finally_statements := _finally_statements
end;

procedure try_finally_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor try_handler.Create;
begin
  
end;

procedure try_handler.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor try_handler_except.Create;
begin
  
end;

constructor try_handler_except.Create(_except_block: exception_block);
begin
  self._except_block := _except_block
end;

procedure try_handler_except.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor try_handler_finally.Create;
begin
  
end;

constructor try_handler_finally.Create(_stmt_list: statement_list);
begin
  self._stmt_list := _stmt_list;
end;

procedure try_handler_finally.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor try_statement.Create;
begin
  
end;

constructor try_statement.Create(_statements: statement_list);
begin
  self._statements := _statements;
end;

procedure try_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor try_stmt.Create;
begin
  
end;

constructor try_stmt.Create(_stmt_list: statement_list; _handler: try_handler);
begin
  self._stmt_list := _stmt_list;
  self._handler := _handler
end;

procedure try_stmt.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor type_declaration.Create;
begin
  
end;

constructor type_declaration.Create(_type_name: ident; _type_def: type_definition);
begin
  self._type_name := _type_name;
  self._type_def := _type_def
end;

procedure type_declaration.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor type_declarations.Create;
begin
  self._types_decl := new List<type_declaration>();  
end;

constructor type_declarations.Create(_types_decl: List<type_declaration>);
begin
  self._types_decl := _types_decl
end;

procedure type_declarations.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor type_definition.Create;
begin
  
end;

constructor type_definition.Create(_attr_list: type_definition_attr_list);
begin
  self._attr_list := _attr_list;
end;

procedure type_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor type_definition_attr.Create;
begin
  
end;

constructor type_definition_attr.Create(_attr: type_definition_attribute);
begin
  self._attr := _attr;
end;

procedure type_definition_attr.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor type_definition_attr_list.Create;
begin
  self._attributes := new List<type_definition_attr>();  
end;

constructor type_definition_attr_list.Create(_attributes: List<type_definition_attr>);
begin
  self._attributes := _attributes
end;

procedure type_definition_attr_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor type_definition_list.Create;
begin
  self._defs := new List<type_definition>();  
end;

constructor type_definition_list.Create(_defs: List<type_definition>);
begin
  self._defs := _defs
end;

procedure type_definition_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor typecast_node.Create;
begin
  
end;

constructor typecast_node.Create(_expr: addressed_value; _type_def: type_definition; _cast_op: op_typecast);
begin
  self._expr := _expr;
  self._type_def := _type_def;
  self._cast_op := _cast_op
end;

procedure typecast_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor typed_const_definition.Create;
begin
  
end;

constructor typed_const_definition.Create(_const_type: type_definition);
begin
  self._const_type := _const_type
end;

procedure typed_const_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor typed_parametres.Create;
begin
  
end;

constructor typed_parametres.Create(_idents: ident_list; _vars_type: type_definition; _param_kind: parametr_kind; _inital_value: expression);
begin
  self._idents := _idents;
  self._vars_type := _vars_type;
  self._param_kind := _param_kind;
  self._inital_value := _inital_value
end;

procedure typed_parametres.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor typeof_operator.Create;
begin
  
end;

constructor typeof_operator.Create(_type_name: named_type_reference);
begin
  self._type_name := _type_name;
end;

procedure typeof_operator.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor uint64_const.Create;
begin
  
end;

constructor uint64_const.Create(_val: UInt64);
begin
  self._val := _val;
end;

procedure uint64_const.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor un_expr.Create;
begin
  
end;

constructor un_expr.Create(_subnode: expression; _operation_type: Operators);
begin
  self._subnode := _subnode;
  self._operation_type := _operation_type
end;

procedure un_expr.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor unit_module.Create;
begin
  
end;

constructor unit_module.Create(_unit_name: SyntaxTree.unit_name; _interface_part: interface_node; _implementation_part: implementation_node; _initialization_part: statement_list; _finalization_part: statement_list);
begin
  self._unit_name := _unit_name;
  self._interface_part := _interface_part;
  self._implementation_part := _implementation_part;
  self._initialization_part := _initialization_part;
  self._finalization_part := _finalization_part
end;

procedure unit_module.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor unit_name.Create;
begin
  
end;

constructor unit_name.Create(_idunit_name: ident; _HeaderKeyword: UnitHeaderKeyword);
begin
  self._idunit_name := _idunit_name;
  self._HeaderKeyword := _HeaderKeyword
end;

procedure unit_name.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor unit_or_namespace.Create;
begin
  
end;

constructor unit_or_namespace.Create(_name: ident_list);
begin
  self._name := _name
end;

procedure unit_or_namespace.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor uses_list.Create;
begin
  self._units := new List<unit_or_namespace>();
end;

constructor uses_list.Create(_units: List<unit_or_namespace>);
begin
  self._units := _units;
end;

procedure uses_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor uses_unit_in.Create;
begin
  
end;

constructor uses_unit_in.Create(_in_file: string_const);
begin
  self._in_file := _in_file;
end;

procedure uses_unit_in.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor using_list.Create;
begin
  self._namespaces := new List<unit_or_namespace>();  
end;

constructor using_list.Create(_namespaces: List<unit_or_namespace>);
begin
  self._namespaces := _namespaces
end;

procedure using_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor var_def_list.Create;
begin
  self._vars := new List<var_def_statement>();  
end;

constructor var_def_list.Create(_vars: List<var_def_statement>);
begin
  self._vars := _vars
end;

procedure var_def_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor var_def_statement.Create;
begin
  
end;

constructor var_def_statement.Create(_vars: ident_list; _vars_type: type_definition; _inital_value: expression; _var_attr: type_definition_attribute; _is_event: boolean);
begin
  self._vars := _vars;
  self._vars_type := _vars_type;
  self._inital_value := _inital_value;
  self._var_attr := _var_attr;
  self._is_event := _is_event
end;

procedure var_def_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor var_statement.Create;
begin
  
end;

constructor var_statement.Create(_var_def: var_def_statement);
begin
  self._var_def := _var_def;
end;

procedure var_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor variable_definitions.Create;
begin
  self._var_definitions := new List<var_def_statement>();  
end;

constructor variable_definitions.Create(_var_definitions: List<var_def_statement>);
begin
  self._var_definitions := _var_definitions
end;

procedure variable_definitions.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor variant.Create;
begin
  
end;

constructor variant.Create(_vars: ident_list; _vars_type: type_definition);
begin
  self._vars := _vars;
  self._vars_type := _vars_type
end;

procedure variant.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor variant_list.Create;
begin
  self._vars := new List<variant>();  
end;

constructor variant_list.Create(_vars: List<variant>);
begin
  self._vars := _vars
end;

procedure variant_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor variant_record_type.Create;
begin
  
end;

constructor variant_record_type.Create(_var_name: ident; _var_type: type_definition; _vars: variant_types);
begin
  self._var_name := _var_name;
  self._var_type := _var_type;
  self._vars := _vars
end;

procedure variant_record_type.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor variant_type.Create;
begin
  
end;

constructor variant_type.Create(_case_exprs: expression_list; _parts: record_type_parts);
begin
  self._case_exprs := _case_exprs;
  self._parts := _parts
end;

procedure variant_type.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor variant_types.Create;
begin
   self._vars := new List<variant_type>();  
end;

constructor variant_types.Create(_vars: List<variant_type>);
begin
  self._vars := _vars
end;

procedure variant_types.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor where_definition.Create;
begin
  
end;

constructor where_definition.Create(_names: ident_list; _types: type_definition_list; _constructor_specific_params: type_definition_list);
begin
  self._names := _names;
  self._types := _types;
  self._constructor_specific_params := _constructor_specific_params
end;

procedure where_definition.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor where_definition_list.Create;
begin
  self._defs := new List<where_definition>();  
end;

constructor where_definition_list.Create(_defs: List<where_definition>);
begin
  self._defs := _defs
end;

procedure where_definition_list.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor while_node.Create;
begin
  
end;

constructor while_node.Create(_expr: expression; _statements: statement; _CycleType: WhileCycleType);
begin
  self._expr := _expr;
  self._statements := _statements;
  self._CycleType := _CycleType
end;

procedure while_node.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor with_statement.Create;
begin
  
end;

constructor with_statement.Create(_what_do: statement; _do_with: expression_list);
begin
  self._what_do := _what_do;
  self._do_with := _do_with
end;

procedure with_statement.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor write_accessor_name.Create;
begin
  
end;

constructor write_accessor_name.Create(_accessor_name: ident);
begin
  self._accessor_name := _accessor_name
end;

procedure write_accessor_name.visit(visitor: IVisitor);
begin
  visitor.visit(self);
end;


constructor OperatorServices.Create;
begin
  
end;

class function OperatorServices.IsAssigmentOperator(&Operator: Operators): boolean;
begin
  
end;

class function OperatorServices.ToString(&Operator: Operators; Language: LanguageId): string;
begin
  
end;


constructor Utils.Create;
begin
  
end;

class function Utils.IdentListToString(list: List<ident>; separator: string): string;
begin
  
end;


procedure IVisitor.visit(_access_modifer_node: access_modifer_node);
begin

end;

procedure IVisitor.visit(_addressed_value: addressed_value);
begin

end;

procedure IVisitor.visit(_addressed_value_funcname: addressed_value_funcname);
begin
  
end;

procedure IVisitor.visit(_array_const: array_const);
begin
  
end;

procedure IVisitor.visit(_array_of_const_type_definition: array_of_const_type_definition);
begin
  
end;

procedure IVisitor.visit(_array_of_named_type_definition: array_of_named_type_definition);
begin
  
end;

procedure IVisitor.visit(_array_size: array_size);
begin
  
end;

procedure IVisitor.visit(_array_type: array_type);
begin
  
end;

procedure IVisitor.visit(_assign: assign);
begin
  
end;

procedure IVisitor.visit(_bin_expr: bin_expr);
begin
  
end;

procedure IVisitor.visit(_block: block);
begin
  
end;

procedure IVisitor.visit(_bool_const: bool_const);
begin
  
end;

procedure IVisitor.visit(_c_for_cycle: c_for_cycle);
begin
  
end;

procedure IVisitor.visit(_c_module: c_module);
begin
  
end;

procedure IVisitor.visit(_c_scalar_type: c_scalar_type);
begin
  
end;

procedure IVisitor.visit(_case_node: case_node);
begin
  
end;

procedure IVisitor.visit(_case_variant: case_variant);
begin
  
end;

procedure IVisitor.visit(_case_variants: case_variants);
begin
  
end;

procedure IVisitor.visit(_char_const: char_const);
begin
  
end;

procedure IVisitor.visit(_class_body: class_body);
begin
  
end;

procedure IVisitor.visit(_class_definition: class_definition);
begin
  
end;

procedure IVisitor.visit(_class_members: class_members);
begin
  
end;

procedure IVisitor.visit(_class_predefinition: class_predefinition);
begin
  
end;

procedure IVisitor.visit(_compilation_unit: compilation_unit);
begin
  
end;

procedure IVisitor.visit(_compiler_directive: compiler_directive);
begin
  
end;

procedure IVisitor.visit(_compiler_directive_if: compiler_directive_if);
begin
  
end;

procedure IVisitor.visit(_compiler_directive_list: compiler_directive_list);
begin
  
end;

procedure IVisitor.visit(_const_definition: const_definition);
begin
  
end;

procedure IVisitor.visit(_const_node: const_node);
begin
  
end;

procedure IVisitor.visit(_constructor: &constructor);
begin
  
end;

procedure IVisitor.visit(_consts_definitions_list: consts_definitions_list);
begin
  
end;

procedure IVisitor.visit(_declaration: declaration);
begin
  
end;

procedure IVisitor.visit(_declarations: declarations);
begin
  
end;

procedure IVisitor.visit(_declarations_as_statement: declarations_as_statement);
begin
  
end;

procedure IVisitor.visit(_default_indexer_property_node: default_indexer_property_node);
begin
  
end;

procedure IVisitor.visit(_dereference: dereference);
begin
  
end;

procedure IVisitor.visit(_destructor: &destructor);
begin
  
end;

procedure IVisitor.visit(_diap_expr: diap_expr);
begin
  
end;

procedure IVisitor.visit(_diapason: diapason);
begin
  
end;

procedure IVisitor.visit(_diapason_expr: diapason_expr);
begin
  
end;

procedure IVisitor.visit(_documentation_comment_list: documentation_comment_list);
begin
  
end;

procedure IVisitor.visit(_documentation_comment_section: documentation_comment_section);
begin
  
end;

procedure IVisitor.visit(_documentation_comment_tag: documentation_comment_tag);
begin
  
end;

procedure IVisitor.visit(_documentation_comment_tag_param: documentation_comment_tag_param);
begin
  
end;

procedure IVisitor.visit(_dot_node: dot_node);
begin
  
end;

procedure IVisitor.visit(_double_const: double_const);
begin
  
end;

procedure IVisitor.visit(_empty_statement: empty_statement);
begin
  
end;

procedure IVisitor.visit(_enum_type_definition: enum_type_definition);
begin
  
end;

procedure IVisitor.visit(_enumerator: enumerator);
begin
  
end;

procedure IVisitor.visit(_enumerator_list: enumerator_list);
begin
  
end;

procedure IVisitor.visit(_exception_block: exception_block);
begin
  
end;

procedure IVisitor.visit(_exception_handler: exception_handler);
begin
  
end;

procedure IVisitor.visit(_exception_handler_list: exception_handler_list);
begin
  
end;

procedure IVisitor.visit(_exception_ident: exception_ident);
begin
  
end;

procedure IVisitor.visit(_expression: expression);
begin
  
end;

procedure IVisitor.visit(_expression_as_statement: expression_as_statement);
begin
  
end;

procedure IVisitor.visit(_expression_list: expression_list);
begin
  
end;

procedure IVisitor.visit(_external_directive: external_directive);
begin
  
end;

procedure IVisitor.visit(_file_type: file_type);
begin
  
end;

procedure IVisitor.visit(_file_type_definition: file_type_definition);
begin
  
end;

procedure IVisitor.visit(_for_node: for_node);
begin
  
end;

procedure IVisitor.visit(_foreach_stmt: foreach_stmt);
begin
  
end;

procedure IVisitor.visit(_formal_parametres: formal_parametres);
begin
  
end;

procedure IVisitor.visit(_format_expr: format_expr);
begin

end;

procedure IVisitor.visit(_function_header: function_header);
begin
  
end;

procedure IVisitor.visit(_get_address: get_address);
begin
  
end;

procedure IVisitor.visit(_goto_statement: goto_statement);
begin
  
end;

procedure IVisitor.visit(_hex_constant: hex_constant);
begin
  
end;

procedure IVisitor.visit(_ident: ident);
begin
  
end;

procedure IVisitor.visit(_ident_list: ident_list);
begin
  
end;

procedure IVisitor.visit(_if_node: if_node);
begin
  
end;

procedure IVisitor.visit(_implementation_node: implementation_node);
begin
  
end;

procedure IVisitor.visit(_index_property: index_property);
begin
  
end;

procedure IVisitor.visit(_indexer: indexer);
begin
  
end;

procedure IVisitor.visit(_indexers_types: indexers_types);
begin
  
end;

procedure IVisitor.visit(_inherited_ident: inherited_ident);
begin
  
end;

procedure IVisitor.visit(_inherited_message: inherited_message);
begin
  
end;

procedure IVisitor.visit(_inherited_method_call: inherited_method_call);
begin
  
end;

procedure IVisitor.visit(_initfinal_part: initfinal_part);
begin
  
end;

procedure IVisitor.visit(_int32_const: int32_const);
begin
  
end;

procedure IVisitor.visit(_int64_const: int64_const);
begin
  
end;

procedure IVisitor.visit(_interface_node: interface_node);
begin
  
end;

procedure IVisitor.visit(_jump_stmt: jump_stmt);
begin
  
end;

procedure IVisitor.visit(_known_type_definition: known_type_definition);
begin
  
end;

procedure IVisitor.visit(_known_type_ident: known_type_ident);
begin
  
end;

procedure IVisitor.visit(_label_definitions: label_definitions);
begin
  
end;

procedure IVisitor.visit(_labeled_statement: labeled_statement);
begin
  
end;

procedure IVisitor.visit(_literal: literal);
begin
  
end;

procedure IVisitor.visit(_literal_const_line: literal_const_line);
begin
  
end;

procedure IVisitor.visit(_lock_stmt: lock_stmt);
begin
  
end;

procedure IVisitor.visit(_loop_stmt: loop_stmt);
begin
  
end;

procedure IVisitor.visit(_method_call: method_call);
begin
  
end;

procedure IVisitor.visit(_method_name: method_name);
begin
  
end;

procedure IVisitor.visit(_named_type_reference: named_type_reference);
begin
  
end;

procedure IVisitor.visit(_named_type_reference_list: named_type_reference_list);
begin
  
end;

procedure IVisitor.visit(_new_expr: new_expr);
begin
  
end;

procedure IVisitor.visit(_nil_const: nil_const);
begin
  
end;

procedure IVisitor.visit(_oberon_exit_stmt: oberon_exit_stmt);
begin
  
end;

procedure IVisitor.visit(_oberon_ident_with_export_marker: oberon_ident_with_export_marker);
begin
  
end;

procedure IVisitor.visit(_oberon_import_module: oberon_import_module);
begin
  
end;

procedure IVisitor.visit(_oberon_module: oberon_module);
begin
  
end;

procedure IVisitor.visit(_oberon_procedure_header: oberon_procedure_header);
begin
  
end;

procedure IVisitor.visit(_oberon_procedure_receiver: oberon_procedure_receiver);
begin
  
end;

procedure IVisitor.visit(_oberon_withstmt: oberon_withstmt);
begin
  
end;

procedure IVisitor.visit(_oberon_withstmt_guardstat: oberon_withstmt_guardstat);
begin
  
end;

procedure IVisitor.visit(_oberon_withstmt_guardstat_list: oberon_withstmt_guardstat_list);
begin
  
end;

procedure IVisitor.visit(_on_exception: on_exception);
begin
  
end;

procedure IVisitor.visit(_on_exception_list: on_exception_list);
begin
  
end;

procedure IVisitor.visit(_op_type_node: op_type_node);
begin
  
end;

procedure IVisitor.visit(_operator_name_ident: operator_name_ident);
begin
  
end;

procedure IVisitor.visit(_pascal_set_constant: pascal_set_constant);
begin
  
end;

procedure IVisitor.visit(_proc_block: proc_block);
begin
  
end;

procedure IVisitor.visit(_procedure_attribute: procedure_attribute);
begin
  
end;

procedure IVisitor.visit(_procedure_attributes_list: procedure_attributes_list);
begin
  
end;

procedure IVisitor.visit(_procedure_call: procedure_call);
begin
  
end;

procedure IVisitor.visit(_procedure_definition: procedure_definition);
begin
  
end;

procedure IVisitor.visit(_procedure_header: procedure_header);
begin
  
end;

procedure IVisitor.visit(_program_body: program_body);
begin
  
end;

procedure IVisitor.visit(_program_module: program_module);
begin
  
end;

procedure IVisitor.visit(_program_name: program_name);
begin
  
end;

procedure IVisitor.visit(_program_tree: program_tree);
begin
  
end;

procedure IVisitor.visit(_property_accessors: property_accessors);
begin
  
end;

procedure IVisitor.visit(_property_array_default: property_array_default);
begin
  
end;

procedure IVisitor.visit(_property_interface: property_interface);
begin
  
end;

procedure IVisitor.visit(_property_parameter: property_parameter);
begin
  
end;

procedure IVisitor.visit(_property_parameter_list: property_parameter_list);
begin
  
end;

procedure IVisitor.visit(_question_colon_expression: question_colon_expression);
begin
  
end;

procedure IVisitor.visit(_raise_statement: raise_statement);
begin
  
end;

procedure IVisitor.visit(_raise_stmt: raise_stmt);
begin
  
end;

procedure IVisitor.visit(_read_accessor_name: read_accessor_name);
begin
  
end;

procedure IVisitor.visit(_record_const: record_const);
begin
  
end;

procedure IVisitor.visit(_record_const_definition: record_const_definition);
begin
  
end;

procedure IVisitor.visit(_record_type: record_type);
begin
  
end;

procedure IVisitor.visit(_record_type_parts: record_type_parts);
begin
  
end;

procedure IVisitor.visit(_ref_type: ref_type);
begin
  
end;

procedure IVisitor.visit(_repeat_node: repeat_node);
begin
  
end;

procedure IVisitor.visit(_roof_dereference: roof_dereference);
begin
  
end;

procedure IVisitor.visit(_set_type_definition: set_type_definition);
begin
  
end;

procedure IVisitor.visit(_sharp_char_const: sharp_char_const);
begin
  
end;

procedure IVisitor.visit(_simple_const_definition: simple_const_definition);
begin
  
end;

procedure IVisitor.visit(_simple_property: simple_property);
begin
  
end;

procedure IVisitor.visit(_sizeof_operator: sizeof_operator);
begin
  
end;

procedure IVisitor.visit(_statement: statement);
begin
  
end;

procedure IVisitor.visit(_statement_list: statement_list);
begin
  
end;

procedure IVisitor.visit(_string_const: string_const);
begin
  
end;

procedure IVisitor.visit(_string_num_definition: string_num_definition);
begin
  
end;

procedure IVisitor.visit(_subprogram_body: subprogram_body);
begin
  
end;

procedure IVisitor.visit(_switch_stmt: switch_stmt);
begin
  
end;

procedure IVisitor.visit(_syntax_tree_node: syntax_tree_node);
begin
  
end;

procedure IVisitor.visit(_template_param_list: template_param_list);
begin
  
end;

procedure IVisitor.visit(_template_type_reference: template_type_reference);
begin
  
end;

procedure IVisitor.visit(_token_info: token_info);
begin
  
end;

procedure IVisitor.visit(_token_taginfo: token_taginfo);
begin
  
end;

procedure IVisitor.visit(_try_except_statement: try_except_statement);
begin
  
end;

procedure IVisitor.visit(_try_finally_statement: try_finally_statement);
begin
  
end;

procedure IVisitor.visit(_try_handler: try_handler);
begin
  
end;

procedure IVisitor.visit(_try_handler_except: try_handler_except);
begin
  
end;

procedure IVisitor.visit(_try_handler_finally: try_handler_finally);
begin
  
end;

procedure IVisitor.visit(_try_statement: try_statement);
begin
  
end;

procedure IVisitor.visit(_try_stmt: try_stmt);
begin
  
end;

procedure IVisitor.visit(_type_declaration: type_declaration);
begin
  
end;

procedure IVisitor.visit(_type_declarations: type_declarations);
begin
  
end;

procedure IVisitor.visit(_type_definition: type_definition);
begin
  
end;

procedure IVisitor.visit(_type_definition_attr: type_definition_attr);
begin
  
end;

procedure IVisitor.visit(_type_definition_attr_list: type_definition_attr_list);
begin
  
end;

procedure IVisitor.visit(_type_definition_list: type_definition_list);
begin
  
end;

procedure IVisitor.visit(_typecast_node: typecast_node);
begin
  
end;

procedure IVisitor.visit(_typed_const_definition: typed_const_definition);
begin
  
end;

procedure IVisitor.visit(_typed_parametres: typed_parametres);
begin
  
end;

procedure IVisitor.visit(_typeof_operator: typeof_operator);
begin
  
end;

procedure IVisitor.visit(_uint64_const: uint64_const);
begin
  
end;

procedure IVisitor.visit(_un_expr: un_expr);
begin
  
end;

procedure IVisitor.visit(_unit_module: unit_module);
begin
  
end;

procedure IVisitor.visit(_unit_name: unit_name);
begin
  
end;

procedure IVisitor.visit(_unit_or_namespace: unit_or_namespace);
begin
  
end;

procedure IVisitor.visit(_uses_list: uses_list);
begin
  
end;

procedure IVisitor.visit(_uses_unit_in: uses_unit_in);
begin
  
end;

procedure IVisitor.visit(_using_list: using_list);
begin
  
end;

procedure IVisitor.visit(_var_def_list: var_def_list);
begin
  
end;

procedure IVisitor.visit(_var_def_statement: var_def_statement);
begin
  
end;

procedure IVisitor.visit(_var_statement: var_statement);
begin
  
end;

procedure IVisitor.visit(_variable_definitions: variable_definitions);
begin
  
end;

procedure IVisitor.visit(_variant: variant);
begin
  
end;

procedure IVisitor.visit(_variant_list: variant_list);
begin
  
end;

procedure IVisitor.visit(_variant_record_type: variant_record_type);
begin
  
end;

procedure IVisitor.visit(_variant_type: variant_type);
begin
  
end;

procedure IVisitor.visit(_variant_types: variant_types);
begin
  
end;

procedure IVisitor.visit(_where_definition: where_definition);
begin
  
end;

procedure IVisitor.visit(_where_definition_list: where_definition_list);
begin
  
end;

procedure IVisitor.visit(_while_node: while_node);
begin
  
end;

procedure IVisitor.visit(_with_statement: with_statement);
begin
  
end;

procedure IVisitor.visit(_write_accessor_name: write_accessor_name);
begin
  
end;


constructor access_modifer_node.Create;
begin
  
end;

constructor access_modifer_node.Create(_access_level: access_modifer);
begin
  
end;

procedure access_modifer_node.visit(visitor: IVisitor);
begin
  
end;

end.