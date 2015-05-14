unit SemanticTree;

interface

uses System, System.Collections.Generic, System.Reflection;

type node_kind = (basic, common, compiled);
node_location_kind = (in_function_location,in_class_location,in_namespace_location,in_block_location);
parameter_type = (value, &var);
polymorphic_state = (ps_static,ps_common, ps_virtual, ps_virtual_abstract);
runtime_statement_type = (invoke_delegate,ctor_delegate,begin_invoke_delegate,end_invoke_delegate);
SpecialFunctionKind = (spkNone,&New,Dispose,NewArray);
type_access_level = (tal_public,tal_internal);
type_special_kind =
(
  none_kind,
  array_kind,
  enum_kind,
  typed_file,
  binary_file,
  short_string,
  array_wrapper,
  &record,
  set_type,
  base_set_type,
  diap_type,
  text_file
);

field_access_level =
(
  fal_private,
  fal_internal,
  fal_protected,
  fal_public
);

generic_parameter_kind =
(
  gpk_none,
  gpk_class,
  gpk_value
);

basic_function_type=
(
  none,
  iadd,
  isub,
  imul,
  idiv,
  imod,
  igr,
  ism,
  igreq,
  ismeq,
  ieq,
  inoteq,
  ishl,
  ishr,
  ior,
  inot,
  ixor,
  iand,
  iunmin,
  iinc,
  idec,
  isinc,
  isdec,
  iassign,
  uiadd,
  uisub,
  uimul,
  uidiv,
  uimod,
  uigr,
  uism,
  uigreq,
  uismeq,
  uieq,
  uinoteq,
  uishl,
  uishr,
  uior,
  uinot,
  uixor,
  uiand,
  uiunmin,
  uiinc,
  uidec,
  uisinc,
  uisdec,
  uiassign,
  badd,
  bsub,
  bmul,
  bdiv,
  bmod,
  bgr,
  bsm,
  bgreq,
  bsmeq,
  beq,
  bnoteq,
  bshl,
  bshr,
  bor,
  bnot,
  bxor,
  band,
  bunmin,
  binc,
  bdec,
  bsinc,
  bsdec,
  bassign,
  sbadd,
  sbsub,
  sbmul,
  sbdiv,
  sbmod,
  sbgr,
  sbsm,
  sbgreq,
  sbsmeq,
  sbeq,
  sbnoteq,
  sbshl,
  sbshr,
  sbor,
  sbnot,
  sbxor,
  sband,
  sbunmin,
  sbinc,
  sbdec,
  sbsinc,
  sbsdec,
  sbassign,
  sadd,
  ssub,
  smul,
  sdiv,
  smod,
  sgr,
  ssm,
  sgreq,
  ssmeq,
  seq,
  snoteq,
  sshl,
  sshr,
  sor,
  snot,
  sxor,
  sand,
  sunmin,
  sinc,
  sdec,
  ssinc,
  ssdec,
  sassign,
  usadd,
  ussub,
  usmul,
  usdiv,
  usmod,
  usgr,
  ussm,
  usgreq,
  ussmeq,
  useq,
  usnoteq,
  usshl,
  usshr,
  usor,
  usnot,
  usxor,
  usand,
  usunmin,
  usinc,
  usdec,
  ussinc,
  ussdec,
  usassign,
  ladd,
  lsub,
  lmul,
  ldiv,
  lmod,
  lgr,
  lsm,
  lgreq,
  lsmeq,
  leq,
  lnoteq,
  lshl,
  lshr,
  lor,
  lnot,
  lxor,
  land,
  lunmin,
  linc,
  ldec,
  lsinc,
  lsdec,
  lassign,
  uladd,
  ulsub,
  ulmul,
  uldiv,
  ulmod,
  ulgr,
  ulsm,
  ulgreq,
  ulsmeq,
  uleq,
  ulnoteq,
  ulshl,
  ulshr,
  ulor,
  ulnot,
  ulxor,
  uland,
  ulunmin,
  ulinc,
  uldec,
  ulsinc,
  ulsdec,
  ulassign,
  fadd,
  fsub,
  fmul,
  fdiv,
  fgr,
  fsm,
  fgreq,
  fsmeq,
  feq,
  fnoteq,
  funmin,
  fassign,
  dadd,
  dsub,
  dmul,
  ddiv,
  dgr,
  dsm,
  dgreq,
  dsmeq,
  deq,
  dnoteq,
  dunmin,
  dassign,
  boolgr,
  boolsm,
  boolgreq,
  boolsmeq,
  booleq,
  boolnoteq,
  boolor,
  boolnot,
  boolxor,
  booland,
  boolassign,
  chargr,
  charsm,
  chargreq,
  charsmeq,
  chareq,
  charnoteq,
  cinc,
  cdec,
  csinc,
  csdec,
  charassign,
  chartous,
  chartoi,
  chartoui,
  chartol,
  chartoul,
  chartof,
  chartod,
  chartob,
  chartosb,
  chartos,
  btos,
  btous,
  btoi,
  btoui,
  btol,
  btoul,
  btof,
  btod,
  btosb,
  btochar,
  sbtos,
  sbtoi,
  sbtol,
  sbtof,
  sbtod,
  sbtob,
  sbtous,
  sbtoui,
  sbtoul,
  sbtochar,
  stoi,
  stol,
  stof,
  stod,
  stob,
  stosb,
  stous,
  stoui,
  stoul,
  stochar,
  ustoi,
  ustoui,
  ustol,
  ustoul,
  ustof,
  ustod,
  ustob,
  ustosb,
  ustos,
  ustochar,
  itol,
  itof,
  itod,
  itob,
  itosb,
  itos,
  itous,
  itoui,
  itoul,
  itochar,
  uitol,
  uitoul,
  uitob,
  uitosb,
  uitos,
  uitous,
  uitoi,
  uitof,
  uitod,
  uitochar,
  ltof,
  ltod,
  ltob,
  ltosb,
  ltos,
  ltous,
  ltoi,
  ltoui,
  ltoul,
  ltochar,
  ultob,
  ultosb,
  ultos,
  ultous,
  ultoi,
  ultoui,
  ultol,
  ultochar,
  ultof,
  ultod,
  ftod,
  ftob,
  ftosb,
  ftos,
  ftous,
  ftoi,
  ftoui,
  ftol,
  ftoul,
  ftochar,
  dtob,
  dtosb,
  dtos,
  dtous,
  dtoi,
  dtoui,
  dtol,
  dtoul,
  dtof,
  dtochar,
  objassign,
  objeq,
  objnoteq,
  objtoobj,
  boolinc,
  booldec,
  boolsinc,
  boolsdec,
  booltoi,
  enumgr,
  enumgreq,
  enumsm,
  enumsmeq
);

type ISemanticVisitor = interface;

IDocument = interface

  { Properties }
  property file_name: string read;
end;

ILocation = interface

  { Properties }
  property begin_column_num: integer read;
  property begin_line_num: integer read;
  property document: IDocument read;
  property end_column_num: integer read;
  property end_line_num: integer read;
end;

ISemanticNode = interface

  { Methods }
  procedure visit(visitor: ISemanticVisitor);
end;

IDefinitionNode = interface (ISemanticNode)

  { Properties }
  property Documentation: String read;
end;

ILocated = interface

  { Properties }
  property Location: ILocation read;
end;

IStatementNode = interface (ISemanticNode, ILocated)

end;

ICommonFunctionNode = interface;
ICommonTypeNode = interface;

ITypeNode = interface (IDefinitionNode, ISemanticNode)

  { Properties }
  property base_type: ITypeNode read;
  property common_generic_function_container: ICommonFunctionNode read;
  property element_type: ITypeNode read;
  property generic_type_container: ICommonTypeNode read;
  property ImplementingInterfaces: List<ITypeNode> read;
  property is_class: Boolean read;
  property is_generic_parameter: Boolean read;
  property is_generic_type_definition: Boolean read;
  property is_value_type: Boolean read;
  property IsAbstract: Boolean read;
  property IsInterface: Boolean read;
  property name: String read;
  property NodeKind: node_kind read;
  property TypeSpecialKind: type_special_kind read;
end;

IParameterNode = interface;

IFunctionNode = interface (IDefinitionNode, ISemanticNode)

  { Properties }
  property generic_parameters_count: Int32 read;
  property is_generic_function: Boolean read;
  property name: String read;
  property NodeKind: node_kind read;
  property NodeLocationKind: node_location_kind read;
  property parameters: array of IParameterNode read;
  property return_value_type: ITypeNode read;
end;

ICommonFunctionConstantDefinitionNode = interface;
ICommonNestedInFunctionFunctionNode = interface;
ILocalVariableNode = interface;

ICommonFunctionNode = interface (IFunctionNode, IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property constants: array of ICommonFunctionConstantDefinitionNode read;
  property function_code: IStatementNode read;
  property functions_nodes: array of ICommonNestedInFunctionFunctionNode read;
  property generic_params: List<ICommonTypeNode> read;
  property return_variable: ILocalVariableNode read;
  property _SpecialFunctionKind: SpecialFunctionKind read;
  property var_definition_nodes: array of ILocalVariableNode read;
end;

IClassMemberNode = interface

  { Properties }
  property comperehensive_type: ITypeNode read;
  property _field_access_level: field_access_level read;
  property _polymorphic_state: polymorphic_state read;
end;

ICommonClassMemberNode = interface (IClassMemberNode)

  { Properties }
  property common_comprehensive_type: ICommonTypeNode read;
end;

ICommonMethodNode = interface (ICommonFunctionNode, IFunctionNode, IDefinitionNode, ISemanticNode, ILocated, ICommonClassMemberNode, IClassMemberNode)

  { Properties }
  property is_constructor: Boolean read;
  property is_final: Boolean read write;
  property newslot_awaited: Boolean read write;
  property overrided_method: IFunctionNode read;
end;

IExpressionNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property &type: ITypeNode read;
end;

IVAriableDefinitionNode = interface (IDefinitionNode, ISemanticNode)

  { Properties }
  property inital_value: IExpressionNode read;
  property name: String read;
  property NodeLocationKind: node_location_kind read;
  property &type: ITypeNode read;
end;

IParameterNode = interface (IVAriableDefinitionNode, IDefinitionNode, ISemanticNode)

  { Properties }
  property &function: IFunctionNode read;
  property is_params: Boolean read;
  property _parameter_type: parameter_type read;
end;

ILabelNode = interface (IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property name: String read;
end;

IGenericInstance = interface

  { Properties }
  property generic_parameters: List<ITypeNode> read;
end;

IAddressedExpressionNode = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

end;

IConstantNode = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property value: Object read;
end;
 
IArrayConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property ElementType: ITypeNode read;
  property ElementValues: array of IConstantNode read;
end;

IArrayInitializer = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property ElementType: ITypeNode read;
  property ElementValues: array of IExpressionNode read;
end;

IAsNode = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property left: IExpressionNode read;
  property right: ITypeNode read;
end;

IBasicFunctionNode = interface (IFunctionNode, IDefinitionNode, ISemanticNode)

  { Properties }
  property _basic_function_type: basic_function_type read;
end;

IFunctionCallNode = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property &function: IFunctionNode read;
  property last_result_function_call: Boolean read write;
  property real_parameters: array of IExpressionNode read;
end;

IBasicFunctionCallNode = interface (IFunctionCallNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property basic_function: IBasicFunctionNode read;
end;

IBasicParameterNode = interface (IParameterNode, IVAriableDefinitionNode, IDefinitionNode, ISemanticNode)

end;

IPropertyNode = interface (IDefinitionNode, ISemanticNode)

  { Properties }
  property comprehensive_type: ITypeNode read;
  property get_function: IFunctionNode read;
  property name: String read;
  property _node_kind: node_kind read;
  property parametres: array of IParameterNode read;
  property property_type: ITypeNode read;
  property set_function: IFunctionNode read;
end;



IBasicPropertyNode = interface (IPropertyNode, IDefinitionNode, ISemanticNode)

end;

IBasicTypeNode = interface (ITypeNode, IDefinitionNode, ISemanticNode)

end;

IBoolConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: Boolean read;
end;

IByteConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: Byte read;
end;
 
IIntConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: Int32 read;
end;

ICaseRangeNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property high_bound: IIntConstantNode read;
  property lower_bound: IIntConstantNode read;
end;

ICaseVariantNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property elements: array of IIntConstantNode read;
  property ranges: array of ICaseRangeNode read;
  property statement_to_execute: IStatementNode read;
end;

ICharConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: Char read;
end;

IConstantDefinitionNode = interface (IDefinitionNode, ISemanticNode)

  { Properties }
  property constant_value: IConstantNode read;
  property name: String read;
  property &type: ITypeNode read;
end;

IClassConstantDefinitionNode = interface (IConstantDefinitionNode, IDefinitionNode, ISemanticNode, IClassMemberNode, ILocated)

end;

ICommonClassFieldNode = interface (IVAriableDefinitionNode, IDefinitionNode, ISemanticNode, ICommonClassMemberNode, IClassMemberNode, ILocated)

end;

IReferenceNode = interface (IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property Variable: IVAriableDefinitionNode read;
end;

ICommonClassFieldReferenceNode = interface (IReferenceNode, IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property field: ICommonClassFieldNode read;
  property obj: IExpressionNode read;
end;

IEventNode = interface (IDefinitionNode, ISemanticNode)

end;

ICommonEventNode = interface (IEventNode, IDefinitionNode, ISemanticNode, ICommonClassMemberNode, IClassMemberNode, ILocated)

  { Properties }
  property AddMethod: ICommonMethodNode read;
  property DelegateType: ITypeNode read;
  property IsStatic: Boolean read;
  property Name: String read;
  property RaiseMethod: ICommonMethodNode read;
  property RemoveMethod: ICommonMethodNode read;
end;
 
ICommonFunctionConstantDefinitionNode = interface (IConstantDefinitionNode, IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property comprehensive_function: ICommonFunctionNode read;
end;

ICommonStaticMethodCallNode = interface (IFunctionCallNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property common_type: ICommonTypeNode read;
  property static_method: ICommonMethodNode read;
end;

ICommonConstructorCall = interface (ICommonStaticMethodCallNode, IFunctionCallNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Methods }
  function new_obj_awaited: Boolean;
end;

ICommonConstructorCallAsConstant = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property ConstructorCall: ICommonConstructorCall read;
end;

ICommonNamespaceNode = interface;
ICommonPropertyNode = interface;

INamespaceMemberNode = interface

  { Properties }
  property comprehensive_namespace: ICommonNamespaceNode read;
end;

ICommonTypeNode = interface (ITypeNode, IDefinitionNode, ISemanticNode, INamespaceMemberNode, ILocated)

  { Properties }
  property constants: array of IClassConstantDefinitionNode read;
  property default_property: IPropertyNode read;
  property events: array of ICommonEventNode read;
  property fields: array of ICommonClassFieldNode read;
  property generic_params: List<ICommonTypeNode> read;
  property has_static_constructor: Boolean read;
  property is_generic_type_definition: Boolean read;
  property IsSealed: Boolean read;
  property lower_value: IConstantNode read;
  property methods: array of ICommonMethodNode read;
  property properties: array of ICommonPropertyNode read;
  property runtime_initialization_marker: ICommonClassFieldNode read;
  property static_constructor: ICommonMethodNode read;
  property _type_access_level: type_access_level read;
  property upper_value: IConstantNode read;
end;

IGenericTypeInstance = interface (IGenericInstance, ICommonTypeNode, ITypeNode, IDefinitionNode, ISemanticNode, INamespaceMemberNode, ILocated)

  { Properties }
  property original_generic: ITypeNode read;
  property used_members: System.Collections.Hashtable read;
end;

ICommonGenericTypeInstance = interface (IGenericTypeInstance, IGenericInstance, ICommonTypeNode, ITypeNode, IDefinitionNode, ISemanticNode, INamespaceMemberNode, ILocated)

end;

INonStaticMethodCallNode = interface (IFunctionCallNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property virtual_call: Boolean read write;
end;

ICommonMethodCallNode = interface (INonStaticMethodCallNode, IFunctionCallNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property &method: ICommonMethodNode read;
  property obj: IExpressionNode read;
end;

ICommonNamespaceFunctionNode = interface;

ICommonNamespaceFunctionCallNode = interface (IFunctionCallNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property namespace_function: ICommonNamespaceFunctionNode read;
end;

ICommonNamespaceFunctionCallNodeAsConstant = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property MethodCall: ICommonNamespaceFunctionCallNode read;
end;

ICommonNamespaceFunctionNode = interface (ICommonFunctionNode, IFunctionNode, IDefinitionNode, ISemanticNode, ILocated, INamespaceMemberNode)

  { Properties }
  property namespace_node: ICommonNamespaceNode read;
end;

INamespaceNode = interface (IDefinitionNode, ISemanticNode)

  { Properties }
  property namespace_name: String read;
end;

INamespaceConstantDefinitionNode = interface;
ICommonNamespaceVariableNode = interface;

ICommonNamespaceNode = interface (INamespaceNode, IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property comprehensive_namespace: INamespaceNode read;
  property constants: array of INamespaceConstantDefinitionNode read;
  property functions: array of ICommonNamespaceFunctionNode read;
  property IsMain: Boolean read;
  property nested_namespaces: array of ICommonNamespaceNode read;
  property types: array of ICommonTypeNode read;
  property variables: array of ICommonNamespaceVariableNode read;
end;
 
ICommonNamespaceVariableNode = interface (IVAriableDefinitionNode, IDefinitionNode, ISemanticNode, INamespaceMemberNode, ILocated)

end;

ICommonNestedInFunctionFunctionCallNode = interface (IFunctionCallNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property common_function: ICommonNestedInFunctionFunctionNode read;
  property static_depth: Int32 read;
end;
 
IFunctionMemberNode = interface

  { Properties }
  property &function: ICommonFunctionNode read;
end;

ICommonNestedInFunctionFunctionNode = interface (ICommonFunctionNode, IFunctionNode, IDefinitionNode, ISemanticNode, ILocated, IFunctionMemberNode)

end;

ICommonParameterNode = interface (IParameterNode, IVAriableDefinitionNode, IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property common_function: ICommonFunctionNode read;
  property is_used_as_unlocal: Boolean read;
end;

ICommonParameterReferenceNode = interface (IReferenceNode, IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property parameter: ICommonParameterNode read;
  property static_depth: Int32 read;
end;

ICommonPropertyNode = interface (IPropertyNode, IDefinitionNode, ISemanticNode, ICommonClassMemberNode, IClassMemberNode, ILocated)

end;

ICompiledTypeNode = interface (ITypeNode, IDefinitionNode, ISemanticNode)

  { Properties }
  property compiled_type: &Type read;
end;

ICompiledClassConstantDefinitionNode = interface (IConstantDefinitionNode, IDefinitionNode, ISemanticNode, IClassMemberNode)

  { Properties }
  property comprehensive_type: ICompiledTypeNode read;
end;

ICompiledClassMemberNode = interface (IClassMemberNode)

  { Properties }
  property comprehensive_type: ICompiledTypeNode read;
end;

ICompiledClassFieldNode = interface (IVAriableDefinitionNode, IDefinitionNode, ISemanticNode, ICompiledClassMemberNode, IClassMemberNode)

  { Properties }
  property compiled_field: FieldInfo read;
end;

ICompiledConstantNode = interface (IConstantDefinitionNode, IDefinitionNode, ISemanticNode)

  { Properties }
  property comprehensive_type: ICompiledTypeNode read;
end;

ICompiledConstructorNode = interface (IFunctionNode, IDefinitionNode, ISemanticNode, ICompiledClassMemberNode, IClassMemberNode)

  { Properties }
  property constructor_info: ConstructorInfo read;
end;

ICompiledConstructorCall = interface (IFunctionCallNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Methods }
  function new_obj_awaited: Boolean;

  { Properties }
  property compiled_type: ICompiledTypeNode read;
  property &constructor: ICompiledConstructorNode read;
end;

ICompiledConstructorCallAsConstant = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property MethodCall: ICompiledConstructorCall read;
end;

ICompiledEventNode = interface (IEventNode, IDefinitionNode, ISemanticNode)

end;

ICompiledFieldReferenceNode = interface (IReferenceNode, IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property field: ICompiledClassFieldNode read;
  property obj: IExpressionNode read;
end;

ICompiledGenericMethodInstance = interface;

ICompiledGenericTypeInstance = interface (IGenericTypeInstance, IGenericInstance, ICommonTypeNode, ITypeNode, IDefinitionNode, ISemanticNode, INamespaceMemberNode, ILocated)

end;

ICompiledMethodNode = interface (IFunctionNode, IDefinitionNode, ISemanticNode, ICompiledClassMemberNode, IClassMemberNode)

  { Properties }
  property method_info: MethodInfo read;
end;

ICompiledMethodCallNode = interface (INonStaticMethodCallNode, IFunctionCallNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property compiled_method: ICompiledMethodNode read;
  property obj: IExpressionNode read;
end;

ICompiledNamespaceNode = interface (INamespaceNode, IDefinitionNode, ISemanticNode)

end;

ICompiledParameterNode = interface (IParameterNode, IVAriableDefinitionNode, IDefinitionNode, ISemanticNode)

  { Properties }
  property compiled_function: ICompiledMethodNode read;
end;

ICompiledPropertyNode = interface (IPropertyNode, IDefinitionNode, ISemanticNode, ICompiledClassMemberNode, IClassMemberNode)

  { Properties }
  property compiled_comprehensive_type: ICompiledTypeNode read;
  property compiled_get_method: ICompiledMethodNode read;
  property compiled_set_method: ICompiledMethodNode read;
  property property_info: PropertyInfo read;
end;

ICompiledStaticMethodCallNode = interface (IFunctionCallNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property compiled_type: ICompiledTypeNode read;
  property static_method: ICompiledMethodNode read;
  property template_parametres: array of ITypeNode read;
end;

ICompiledStaticMethodCallNodeAsConstant = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property MethodCall: ICompiledStaticMethodCallNode read;
end;

IDefaultOperatorNode = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

end;

IDereferenceNode = interface (IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property derefered_expr: IExpressionNode read;
end;

IProgramBase = interface (IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property namespaces: array of ICommonNamespaceNode read;
end;

IDllNode = interface (IProgramBase, IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property finalization_function: ICommonNamespaceFunctionNode read;
  property initialization_function: ICommonNamespaceFunctionNode read;
end;

IDoubleConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: real read;
end;

IEnumConstNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: Int32 read;
end;

ILocalBlockVariableReferenceNode = interface;

IExceptionFilterBlockNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property ExceptionHandler: IStatementNode read;
  property ExceptionInstance: ILocalBlockVariableReferenceNode read;
  property ExceptionType: ITypeNode read;
end;

IExitProcedure = interface (IStatementNode, ISemanticNode, ILocated)

end;

IExternalStatementNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property module_name: String read;
  property name: String read;
end;

IFloatConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: Single read;
end;

IForNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property body: IStatementNode read;
  property increment_statement: IStatementNode read;
  property init_while_expr: IExpressionNode read;
  property initialization_statement: IStatementNode read;
  property IsBoolCycle: Boolean read;
  property while_expr: IExpressionNode read;
end;

IForBreakNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property for_node: IForNode read;
end;

IForContinueNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property for_node: IForNode read;
end;

IForeachNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property Body: IStatementNode read;
  property InWhatExpr: IExpressionNode read;
  property VarIdent: IVAriableDefinitionNode read;
end;

IForeachBreakNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property foreach_node: IForeachNode read;
end;

IForeachContinueNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property foreach_node: IForeachNode read;
end;

IFunctionConstantDefinitionNode = interface (IConstantDefinitionNode, IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property &function: ICommonFunctionNode read;
end;

IFunctionConstantReference = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property Constant: ICommonFunctionConstantDefinitionNode read;
end;

IGenericFunctionInstance = interface (IGenericInstance, ICommonFunctionNode, IFunctionNode, IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property original_function: IFunctionNode read;
end;

IGetAddrNode = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property addr_of_expr: IExpressionNode read;
end;

IGotoStatementNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property &label: ILabelNode read;
end;

IIfNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property condition: IExpressionNode read;
  property else_body: IStatementNode read;
  property then_body: IStatementNode read;
end;


IIsNode = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property left: IExpressionNode read;
  property right: ITypeNode read;
end;

ILabeledStatementNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property &label: ILabelNode read;
  property statement: IStatementNode read;
end;

ILocalBlockVariableNode = interface;

IStatementsListNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property LeftLogicalBracketLocation: ILocation read;
  property LocalVariables: array of ILocalBlockVariableNode read;
  property RightLogicalBracketLocation: ILocation read;
  property statements: array of IStatementNode read;
end;

ILocalBlockVariableNode = interface (IVAriableDefinitionNode, IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property Block: IStatementsListNode read;
end;

ILocalBlockVariableReferenceNode = interface (IReferenceNode, IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property Variable: ILocalBlockVariableNode read;
end;

ILocalVariableNode = interface (IVAriableDefinitionNode, IDefinitionNode, ISemanticNode, IFunctionMemberNode, ILocated)

  { Properties }
  property is_used_as_unlocal: Boolean read;
end;

ILocalVariableReferenceNode = interface (IReferenceNode, IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property static_depth: integer read;
  property variable: ILocalVariableNode read;
end;

ILockStatement = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property Body: IStatementNode read;
  property LockObject: IExpressionNode read;
end;

ILongConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: Int64 read;
end;

INamespaceConstantDefinitionNode = interface (IConstantDefinitionNode, IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property comprehensive_namespace: ICommonNamespaceNode read;
end;

INamespaceConstantReference = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property Constant: INamespaceConstantDefinitionNode read;
end;

INamespaceVariableReferenceNode = interface (IReferenceNode, IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property variable: ICommonNamespaceVariableNode read;
end;

IStaticEventReference = interface (IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property &Event: IEventNode read;
end;

INonStaticEventReference = interface (IStaticEventReference, IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property obj: IExpressionNode read;
end;

INullConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

end;

IProgramNode = interface (IProgramBase, IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property generic_function_instances: List<IGenericFunctionInstance> read;
  property generic_type_instances: List<IGenericTypeInstance> read;
  property InitializationCode: IStatementNode read;
  property main_function: ICommonNamespaceFunctionNode read;
end;

IQuestionColonExpressionNode = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property condition: IExpressionNode read;
  property ret_if_false: IExpressionNode read;
  property ret_if_true: IExpressionNode read;
end;

IRecordConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property FieldValues: array of IConstantNode read;
end;

IRecordInitializer = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property FieldValues: array of IExpressionNode read;
end;

IRefTypeNode = interface (ITypeNode, IDefinitionNode, ISemanticNode)

  { Properties }
  property pointed_type: ITypeNode read;
end;

IRepeatNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property body: IStatementNode read;
  property condition: IExpressionNode read;
end;

IRepeatBreakNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property repeat_node: IRepeatNode read;
end;

IRepeatContinueNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property repeat_node: IRepeatNode read;
end;

IRethrowStatement = interface (IStatementNode, ISemanticNode, ILocated)

end;

IReturnNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property return_value: IExpressionNode read;
end;

IRuntimeManagedMethodBody = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property _runtime_statement_type: runtime_statement_type read;
end;

ISByteConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: SByte read;
end;

 
IShortConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: Int16 read;
end;

ISimpleArrayIndexingNode = interface (IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property &array: IExpressionNode read;
  property index: IExpressionNode read;
end;

ISimpleArrayNode = interface (ITypeNode, IDefinitionNode, ISemanticNode)

  { Properties }
  property element_type: ITypeNode read;
  property length: Int32 read;
end;

ISizeOfOperator = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property oftype: ITypeNode read;
end;

IStatementsExpressionNode = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property expresion: IExpressionNode read;
  property statements: array of IStatementNode read;
end;

ICompiledGenericMethodInstance = interface (IGenericFunctionInstance, IGenericInstance, ICommonMethodNode, ICommonFunctionNode, IFunctionNode, IDefinitionNode, ISemanticNode, ILocated, ICommonClassMemberNode, IClassMemberNode)

end;

IStaticCommonClassFieldReferenceNode = interface (IReferenceNode, IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property class_type: ICommonTypeNode read;
  property static_field: ICommonClassFieldNode read;
end;

IStaticCompiledFieldReferenceNode = interface (IReferenceNode, IAddressedExpressionNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property class_type: ICompiledTypeNode read;
  property static_field: ICompiledClassFieldNode read;
end;

IStringConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: String read;
end;

ISwitchNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property case_expression: IExpressionNode read;
  property case_variants: array of ICaseVariantNode read;
  property default_statement: IStatementNode read;
end;

IThisNode = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

end;

IThrowNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property exception_expresion: IExpressionNode read;
end;

ITryBlockNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property ExceptionFilters: array of IExceptionFilterBlockNode read;
  property FinallyStatements: IStatementNode read;
  property TryStatements: IStatementNode read;
end;

ITypeOfOperator = interface (IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property oftype: ITypeNode read;
end;

ITypeSynonym = interface (IDefinitionNode, ISemanticNode, ILocated)

  { Properties }
  property name: String read;
  property original_type: ITypeNode read;
end;

IUIntConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: UInt32 read;
end;

IULongConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: UInt64 read;
end;

IUnsizedArray = interface (ITypeNode, IDefinitionNode, ISemanticNode)

  { Properties }
  property element_type: ITypeNode read;
end;

IUShortConstantNode = interface (IConstantNode, IExpressionNode, IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property constant_value: UInt16 read;
end;

IWhileNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property body: IStatementNode read;
  property condition: IExpressionNode read;
end;

IWhileBreakNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property while_node: IWhileNode read;
end;

IWhileContinueNode = interface (IStatementNode, ISemanticNode, ILocated)

  { Properties }
  property while_node: IWhileNode read;
end;

ISemanticVisitor = interface
procedure visit(value: IAddressedExpressionNode);
  procedure visit(value: IArrayConstantNode);
  procedure visit(value: IArrayInitializer);
  procedure visit(value: IAsNode);
  procedure visit(value: IBasicFunctionCallNode);
  procedure visit(value: IBasicFunctionNode);
  procedure visit(value: IBasicParameterNode);
  procedure visit(value: IBasicPropertyNode);
  procedure visit(value: IBasicTypeNode);
  procedure visit(value: IBoolConstantNode);
  procedure visit(value: IByteConstantNode);
  procedure visit(value: ICaseRangeNode);
  procedure visit(value: ICaseVariantNode);
  procedure visit(value: ICharConstantNode);
  procedure visit(value: IClassConstantDefinitionNode);
  procedure visit(value: IClassMemberNode);
  procedure visit(value: ICommonClassFieldNode);
  procedure visit(value: ICommonClassFieldReferenceNode);
  procedure visit(value: ICommonClassMemberNode);
  procedure visit(value: ICommonConstructorCall);
  procedure visit(value: ICommonConstructorCallAsConstant);
  procedure visit(value: ICommonEventNode);
  procedure visit(value: ICommonFunctionConstantDefinitionNode);
  procedure visit(value: ICommonFunctionNode);
  procedure visit(value: ICommonMethodCallNode);
  procedure visit(value: ICommonMethodNode);
  procedure visit(value: ICommonNamespaceFunctionCallNode);
  procedure visit(value: ICommonNamespaceFunctionCallNodeAsConstant);
  procedure visit(value: ICommonNamespaceFunctionNode);
  procedure visit(value: ICommonNamespaceNode);
  procedure visit(value: ICommonNamespaceVariableNode);
  procedure visit(value: ICommonNestedInFunctionFunctionCallNode);
  procedure visit(value: ICommonNestedInFunctionFunctionNode);
  procedure visit(value: ICommonParameterNode);
  procedure visit(value: ICommonParameterReferenceNode);
  procedure visit(value: ICommonPropertyNode);
  procedure visit(value: ICommonStaticMethodCallNode);
  procedure visit(value: ICommonTypeNode);
  procedure visit(value: ICompiledClassConstantDefinitionNode);
  procedure visit(value: ICompiledClassFieldNode);
  procedure visit(value: ICompiledClassMemberNode);
  procedure visit(value: ICompiledConstructorCall);
  procedure visit(value: ICompiledConstructorCallAsConstant);
  procedure visit(value: ICompiledConstructorNode);
  procedure visit(value: ICompiledEventNode);
  procedure visit(value: ICompiledFieldReferenceNode);
  procedure visit(value: ICompiledMethodCallNode);
  procedure visit(value: ICompiledMethodNode);
  procedure visit(value: ICompiledNamespaceNode);
  procedure visit(value: ICompiledParameterNode);
  procedure visit(value: ICompiledPropertyNode);
  procedure visit(value: ICompiledStaticMethodCallNode);
  procedure visit(value: ICompiledStaticMethodCallNodeAsConstant);
  procedure visit(value: ICompiledTypeNode);
  procedure visit(value: IConstantDefinitionNode);
  procedure visit(value: IConstantNode);
  procedure visit(value: IDefaultOperatorNode);
  procedure visit(value: IDefinitionNode);
  procedure visit(value: IDereferenceNode);
  procedure visit(value: IDllNode);
  procedure visit(value: IDoubleConstantNode);
  procedure visit(value: IEnumConstNode);
  procedure visit(value: IEventNode);
  procedure visit(value: IExceptionFilterBlockNode);
  procedure visit(value: IExitProcedure);
  procedure visit(value: IExpressionNode);
  procedure visit(value: IExternalStatementNode);
  procedure visit(value: IFloatConstantNode);
  procedure visit(value: IForBreakNode);
  procedure visit(value: IForContinueNode);
  procedure visit(value: IForeachBreakNode);
  procedure visit(value: IForeachContinueNode);
  procedure visit(value: IForeachNode);
  procedure visit(value: IForNode);
  procedure visit(value: IFunctionCallNode);
  procedure visit(value: IFunctionConstantReference);
  procedure visit(value: IFunctionMemberNode);
  procedure visit(value: IFunctionNode);
  procedure visit(value: IGetAddrNode);
  procedure visit(value: IGotoStatementNode);
  procedure visit(value: IIfNode);
  procedure visit(value: IIntConstantNode);
  procedure visit(value: IIsNode);
  procedure visit(value: ILabeledStatementNode);
  procedure visit(value: ILabelNode);
  procedure visit(value: ILocalBlockVariableNode);
  procedure visit(value: ILocalBlockVariableReferenceNode);
  procedure visit(value: ILocalVariableNode);
  procedure visit(value: ILocalVariableReferenceNode);
  procedure visit(value: ILockStatement);
  procedure visit(value: ILongConstantNode);
  procedure visit(value: INamespaceConstantDefinitionNode);
  procedure visit(value: INamespaceConstantReference);
  procedure visit(value: INamespaceMemberNode);
  procedure visit(value: INamespaceNode);
  procedure visit(value: INamespaceVariableReferenceNode);
  procedure visit(value: INonStaticEventReference);
  procedure visit(value: INullConstantNode);
  procedure visit(value: IParameterNode);
  procedure visit(value: IProgramNode);
  procedure visit(value: IPropertyNode);
  procedure visit(value: IQuestionColonExpressionNode);
  procedure visit(value: IRecordConstantNode);
  procedure visit(value: IRecordInitializer);
  procedure visit(value: IRefTypeNode);
  procedure visit(value: IRepeatBreakNode);
  procedure visit(value: IRepeatContinueNode);
  procedure visit(value: IRepeatNode);
  procedure visit(value: IRethrowStatement);
  procedure visit(value: IReturnNode);
  procedure visit(value: IRuntimeManagedMethodBody);
  procedure visit(value: ISByteConstantNode);
  procedure visit(value: ISemanticNode);
  procedure visit(value: IShortConstantNode);
  procedure visit(value: ISimpleArrayIndexingNode);
  procedure visit(value: ISimpleArrayNode);
  procedure visit(value: ISizeOfOperator);
  procedure visit(value: IStatementNode);
  procedure visit(value: IStatementsExpressionNode);
  procedure visit(value: IStatementsListNode);
  procedure visit(value: IStaticCommonClassFieldReferenceNode);
  procedure visit(value: IStaticCompiledFieldReferenceNode);
  procedure visit(value: IStaticEventReference);
  procedure visit(value: IStringConstantNode);
  procedure visit(value: ISwitchNode);
  procedure visit(value: IThisNode);
  procedure visit(value: IThrowNode);
  procedure visit(value: ITryBlockNode);
  procedure visit(value: ITypeNode);
  procedure visit(value: ITypeOfOperator);
  procedure visit(value: IUIntConstantNode);
  procedure visit(value: IULongConstantNode);
  procedure visit(value: IUnsizedArray);
  procedure visit(value: IUShortConstantNode);
  procedure visit(value: IVAriableDefinitionNode);
  procedure visit(value: IWhileBreakNode);
  procedure visit(value: IWhileContinueNode);
  procedure visit(value: IWhileNode);
end;
 
type AbstractVisitor = class(ISemanticVisitor)
protected constructor Create;
public procedure visit(value: IAddressedExpressionNode); virtual; 
  public procedure visit(value: IArrayConstantNode); virtual; 
  public procedure visit(value: IArrayInitializer); virtual; 
  public procedure visit(value: IAsNode); virtual; 
  public procedure visit(value: IBasicFunctionCallNode); virtual; 
  public procedure visit(value: IBasicFunctionNode); virtual; 
  public procedure visit(value: IBasicParameterNode); virtual; 
  public procedure visit(value: IBasicPropertyNode); virtual; 
  public procedure visit(value: IBasicTypeNode); virtual; 
  public procedure visit(value: IBoolConstantNode); virtual; 
  public procedure visit(value: IByteConstantNode); virtual; 
  public procedure visit(value: ICaseRangeNode); virtual; 
  public procedure visit(value: ICaseVariantNode); virtual; 
  public procedure visit(value: ICharConstantNode); virtual; 
  public procedure visit(value: IClassConstantDefinitionNode); virtual; 
  public procedure visit(value: IClassMemberNode); virtual; 
  public procedure visit(value: ICommonClassFieldNode); virtual; 
  public procedure visit(value: ICommonClassFieldReferenceNode); virtual; 
  public procedure visit(value: ICommonClassMemberNode); virtual; 
  public procedure visit(value: ICommonConstructorCall); virtual; 
  public procedure visit(value: ICommonConstructorCallAsConstant); virtual; 
  public procedure visit(value: ICommonEventNode); virtual; 
  public procedure visit(value: ICommonFunctionConstantDefinitionNode); virtual; 
  public procedure visit(value: ICommonFunctionNode); virtual; 
  public procedure visit(value: ICommonMethodCallNode); virtual; 
  public procedure visit(value: ICommonMethodNode); virtual; 
  public procedure visit(value: ICommonNamespaceFunctionCallNode); virtual; 
  public procedure visit(value: ICommonNamespaceFunctionCallNodeAsConstant); virtual; 
  public procedure visit(value: ICommonNamespaceFunctionNode); virtual; 
  public procedure visit(value: ICommonNamespaceNode); virtual; 
  public procedure visit(value: ICommonNamespaceVariableNode); virtual; 
  public procedure visit(value: ICommonNestedInFunctionFunctionCallNode); virtual; 
  public procedure visit(value: ICommonNestedInFunctionFunctionNode); virtual; 
  public procedure visit(value: ICommonParameterNode); virtual; 
  public procedure visit(value: ICommonParameterReferenceNode); virtual; 
  public procedure visit(value: ICommonPropertyNode); virtual; 
  public procedure visit(value: ICommonStaticMethodCallNode); virtual; 
  public procedure visit(value: ICommonTypeNode); virtual; 
  public procedure visit(value: ICompiledClassConstantDefinitionNode); virtual; 
  public procedure visit(value: ICompiledClassFieldNode); virtual; 
  public procedure visit(value: ICompiledClassMemberNode); virtual; 
  public procedure visit(value: ICompiledConstructorCall); virtual; 
  public procedure visit(value: ICompiledConstructorCallAsConstant); virtual; 
  public procedure visit(value: ICompiledConstructorNode); virtual; 
  public procedure visit(value: ICompiledEventNode); virtual; 
  public procedure visit(value: ICompiledFieldReferenceNode); virtual; 
  public procedure visit(value: ICompiledMethodCallNode); virtual; 
  public procedure visit(value: ICompiledMethodNode); virtual; 
  public procedure visit(value: ICompiledNamespaceNode); virtual; 
  public procedure visit(value: ICompiledParameterNode); virtual; 
  public procedure visit(value: ICompiledPropertyNode); virtual; 
  public procedure visit(value: ICompiledStaticMethodCallNode); virtual; 
  public procedure visit(value: ICompiledStaticMethodCallNodeAsConstant); virtual; 
  public procedure visit(value: ICompiledTypeNode); virtual; 
  public procedure visit(value: IConstantDefinitionNode); virtual; 
  public procedure visit(value: IConstantNode); virtual; 
  public procedure visit(value: IDefaultOperatorNode); virtual; 
  public procedure visit(value: IDefinitionNode); virtual; 
  public procedure visit(value: IDereferenceNode); virtual; 
  public procedure visit(value: IDllNode); virtual; 
  public procedure visit(value: IDoubleConstantNode); virtual; 
  public procedure visit(value: IEnumConstNode); virtual; 
  public procedure visit(value: IEventNode); virtual; 
  public procedure visit(value: IExceptionFilterBlockNode); virtual; 
  public procedure visit(value: IExitProcedure); virtual; 
  public procedure visit(value: IExpressionNode); virtual; 
  public procedure visit(value: IExternalStatementNode); virtual; 
  public procedure visit(value: IFloatConstantNode); virtual; 
  public procedure visit(value: IForBreakNode); virtual; 
  public procedure visit(value: IForContinueNode); virtual; 
  public procedure visit(value: IForeachBreakNode); virtual; 
  public procedure visit(value: IForeachContinueNode); virtual; 
  public procedure visit(value: IForeachNode); virtual; 
  public procedure visit(value: IForNode); virtual; 
  public procedure visit(value: IFunctionCallNode); virtual; 
  public procedure visit(value: IFunctionConstantReference); virtual; 
  public procedure visit(value: IFunctionMemberNode); virtual; 
  public procedure visit(value: IFunctionNode); virtual; 
  public procedure visit(value: IGetAddrNode); virtual; 
  public procedure visit(value: IGotoStatementNode); virtual; 
  public procedure visit(value: IIfNode); virtual; 
  public procedure visit(value: IIntConstantNode); virtual; 
  public procedure visit(value: IIsNode); virtual; 
  public procedure visit(value: ILabeledStatementNode); virtual; 
  public procedure visit(value: ILabelNode); virtual; 
  public procedure visit(value: ILocalBlockVariableNode); virtual; 
  public procedure visit(value: ILocalBlockVariableReferenceNode); virtual; 
  public procedure visit(value: ILocalVariableNode); virtual; 
  public procedure visit(value: ILocalVariableReferenceNode); virtual; 
  public procedure visit(value: ILockStatement); virtual; 
  public procedure visit(value: ILongConstantNode); virtual; 
  public procedure visit(value: INamespaceConstantDefinitionNode); virtual; 
  public procedure visit(value: INamespaceConstantReference); virtual; 
  public procedure visit(value: INamespaceMemberNode); virtual; 
  public procedure visit(value: INamespaceNode); virtual; 
  public procedure visit(value: INamespaceVariableReferenceNode); virtual; 
  public procedure visit(value: INonStaticEventReference); virtual; 
  public procedure visit(value: INullConstantNode); virtual; 
  public procedure visit(value: IParameterNode); virtual; 
  public procedure visit(value: IProgramNode); virtual; 
  public procedure visit(value: IPropertyNode); virtual; 
  public procedure visit(value: IQuestionColonExpressionNode); virtual; 
  public procedure visit(value: IRecordConstantNode); virtual; 
  public procedure visit(value: IRecordInitializer); virtual; 
  public procedure visit(value: IRefTypeNode); virtual; 
  public procedure visit(value: IRepeatBreakNode); virtual; 
  public procedure visit(value: IRepeatContinueNode); virtual; 
  public procedure visit(value: IRepeatNode); virtual; 
  public procedure visit(value: IRethrowStatement); virtual; 
  public procedure visit(value: IReturnNode); virtual; 
  public procedure visit(value: IRuntimeManagedMethodBody); virtual; 
  public procedure visit(value: ISByteConstantNode); virtual; 
  public procedure visit(value: ISemanticNode); virtual; 
  public procedure visit(value: IShortConstantNode); virtual; 
  public procedure visit(value: ISimpleArrayIndexingNode); virtual; 
  public procedure visit(value: ISimpleArrayNode); virtual; 
  public procedure visit(value: ISizeOfOperator); virtual; 
  public procedure visit(value: IStatementNode); virtual; 
  public procedure visit(value: IStatementsExpressionNode); virtual; 
  public procedure visit(value: IStatementsListNode); virtual; 
  public procedure visit(value: IStaticCommonClassFieldReferenceNode); virtual; 
  public procedure visit(value: IStaticCompiledFieldReferenceNode); virtual; 
  public procedure visit(value: IStaticEventReference); virtual; 
  public procedure visit(value: IStringConstantNode); virtual; 
  public procedure visit(value: ISwitchNode); virtual; 
  public procedure visit(value: IThisNode); virtual; 
  public procedure visit(value: IThrowNode); virtual; 
  public procedure visit(value: ITryBlockNode); virtual; 
  public procedure visit(value: ITypeNode); virtual; 
  public procedure visit(value: ITypeOfOperator); virtual; 
  public procedure visit(value: IUIntConstantNode); virtual; 
  public procedure visit(value: IULongConstantNode); virtual; 
  public procedure visit(value: IUnsizedArray); virtual; 
  public procedure visit(value: IUShortConstantNode); virtual; 
  public procedure visit(value: IVAriableDefinitionNode); virtual; 
  public procedure visit(value: IWhileBreakNode); virtual; 
  public procedure visit(value: IWhileContinueNode); virtual; 
  public procedure visit(value: IWhileNode); virtual; 


end;

implementation

constructor AbstractVisitor.Create;
begin
  
end;

procedure AbstractVisitor.visit(value: IAddressedExpressionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IArrayConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IArrayInitializer);
begin
  
end;

procedure AbstractVisitor.visit(value: IAsNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IBasicFunctionCallNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IBasicFunctionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IBasicParameterNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IBasicPropertyNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IBasicTypeNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IBoolConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IByteConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICaseRangeNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICaseVariantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICharConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IClassConstantDefinitionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IClassMemberNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonClassFieldNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonClassFieldReferenceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonClassMemberNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonConstructorCall);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonConstructorCallAsConstant);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonEventNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonFunctionConstantDefinitionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonFunctionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonMethodCallNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonMethodNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonNamespaceFunctionCallNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonNamespaceFunctionCallNodeAsConstant);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonNamespaceFunctionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonNamespaceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonNamespaceVariableNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonNestedInFunctionFunctionCallNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonNestedInFunctionFunctionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonParameterNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonParameterReferenceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonPropertyNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonStaticMethodCallNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICommonTypeNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledClassConstantDefinitionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledClassFieldNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledClassMemberNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledConstructorCall);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledConstructorCallAsConstant);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledConstructorNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledEventNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledFieldReferenceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledMethodCallNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledMethodNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledNamespaceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledParameterNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledPropertyNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledStaticMethodCallNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledStaticMethodCallNodeAsConstant);
begin
  
end;

procedure AbstractVisitor.visit(value: ICompiledTypeNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IConstantDefinitionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IDefaultOperatorNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IDefinitionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IDereferenceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IDllNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IDoubleConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IEnumConstNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IEventNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IExceptionFilterBlockNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IExitProcedure);
begin
  
end;

procedure AbstractVisitor.visit(value: IExpressionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IExternalStatementNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IFloatConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IForBreakNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IForContinueNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IForeachBreakNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IForeachContinueNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IForeachNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IForNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IFunctionCallNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IFunctionConstantReference);
begin
  
end;

procedure AbstractVisitor.visit(value: IFunctionMemberNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IFunctionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IGetAddrNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IGotoStatementNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IIfNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IIntConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IIsNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ILabeledStatementNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ILabelNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ILocalBlockVariableNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ILocalBlockVariableReferenceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ILocalVariableNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ILocalVariableReferenceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ILockStatement);
begin
  
end;

procedure AbstractVisitor.visit(value: ILongConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: INamespaceConstantDefinitionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: INamespaceConstantReference);
begin
  
end;

procedure AbstractVisitor.visit(value: INamespaceMemberNode);
begin
  
end;

procedure AbstractVisitor.visit(value: INamespaceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: INamespaceVariableReferenceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: INonStaticEventReference);
begin
  
end;

procedure AbstractVisitor.visit(value: INullConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IParameterNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IProgramNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IPropertyNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IQuestionColonExpressionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IRecordConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IRecordInitializer);
begin
  
end;

procedure AbstractVisitor.visit(value: IRefTypeNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IRepeatBreakNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IRepeatContinueNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IRepeatNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IRethrowStatement);
begin
  
end;

procedure AbstractVisitor.visit(value: IReturnNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IRuntimeManagedMethodBody);
begin
  
end;

procedure AbstractVisitor.visit(value: ISByteConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ISemanticNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IShortConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ISimpleArrayIndexingNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ISimpleArrayNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ISizeOfOperator);
begin
  
end;

procedure AbstractVisitor.visit(value: IStatementNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IStatementsExpressionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IStatementsListNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IStaticCommonClassFieldReferenceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IStaticCompiledFieldReferenceNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IStaticEventReference);
begin
  
end;

procedure AbstractVisitor.visit(value: IStringConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ISwitchNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IThisNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IThrowNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ITryBlockNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ITypeNode);
begin
  
end;

procedure AbstractVisitor.visit(value: ITypeOfOperator);
begin
  
end;

procedure AbstractVisitor.visit(value: IUIntConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IULongConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IUnsizedArray);
begin
  
end;

procedure AbstractVisitor.visit(value: IUShortConstantNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IVAriableDefinitionNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IWhileBreakNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IWhileContinueNode);
begin
  
end;

procedure AbstractVisitor.visit(value: IWhileNode);
begin
  
end;

end.