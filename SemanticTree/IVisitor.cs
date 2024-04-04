// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
namespace PascalABCCompiler.SemanticTree
{

	public interface ISemanticVisitor
	{

		void visit(ISemanticNode value);

		void visit(IDefinitionNode value);

		void visit(ITypeNode value);

		void visit(IBasicTypeNode value);

		void visit(ICommonTypeNode value);

		void visit(ICompiledTypeNode value);

		void visit(IStatementNode value);

		void visit(IExpressionNode value);

		void visit(IFunctionCallNode value);

		void visit(IBasicFunctionCallNode value);

		void visit(ICommonNamespaceFunctionCallNode value);

		void visit(ICommonNestedInFunctionFunctionCallNode value);

		void visit(ICommonMethodCallNode value);

		void visit(ICommonStaticMethodCallNode value);

		void visit(ICompiledMethodCallNode value);

		void visit(ICompiledStaticMethodCallNode value);

		void visit(IFunctionNode value);

		void visit(IClassMemberNode value);

		void visit(ICompiledClassMemberNode value);

		void visit(ICommonClassMemberNode value);

		void visit(IFunctionMemberNode value);

		void visit(INamespaceMemberNode value);

		void visit(IBasicFunctionNode value);

		void visit(ICommonFunctionNode value);

		void visit(ICommonNamespaceFunctionNode value);

		void visit(ICommonNestedInFunctionFunctionNode value);

		void visit(ICommonMethodNode value);

		void visit(ICompiledMethodNode value);

		void visit(IIfNode value);

		void visit(IWhileNode value);

		void visit(IRepeatNode value);

		void visit(IForNode value);

		void visit(IStatementsListNode value);

		void visit(INamespaceNode value);

		void visit(ICommonNamespaceNode value);

		void visit(ICompiledNamespaceNode value);

		void visit(IDllNode value);

		void visit(IProgramNode value);

		void visit(IAddressedExpressionNode value);

		void visit(ILocalVariableReferenceNode value);

		void visit(INamespaceVariableReferenceNode value);

		void visit(ICommonClassFieldReferenceNode value);

		void visit(IStaticCommonClassFieldReferenceNode value);

		void visit(ICompiledFieldReferenceNode value);

		void visit(IStaticCompiledFieldReferenceNode value);

		void visit(ICommonParameterReferenceNode value);

		void visit(IConstantNode value);

		void visit(IBoolConstantNode value);

		void visit(IByteConstantNode value);

		void visit(IIntConstantNode value);

		void visit(ISByteConstantNode value);

		void visit(IShortConstantNode value);

		void visit(IUShortConstantNode value);

		void visit(IUIntConstantNode value);

		void visit(IULongConstantNode value);

		void visit(ILongConstantNode value);

		void visit(IDoubleConstantNode value);

		void visit(IFloatConstantNode value);

		void visit(ICharConstantNode value);

		void visit(IStringConstantNode value);

		//void visit(IAssignNode value);

		void visit(IVAriableDefinitionNode value);

		void visit(ILocalVariableNode value);

		void visit(ICommonNamespaceVariableNode value);

		void visit(ICommonClassFieldNode value);

		void visit(ICompiledClassFieldNode value);

		void visit(IParameterNode value);

		void visit(ICommonParameterNode value);

		void visit(IBasicParameterNode value);

		void visit(ICompiledParameterNode value);

		void visit(IConstantDefinitionNode value);

		void visit(IClassConstantDefinitionNode value);

		void visit(ICompiledClassConstantDefinitionNode value);

		void visit(INamespaceConstantDefinitionNode value);

		void visit(ICommonFunctionConstantDefinitionNode value);

		void visit(IPropertyNode value);

		void visit(ICommonPropertyNode value);

		void visit(IBasicPropertyNode value);

		void visit(ICompiledPropertyNode value);

		void visit(IThisNode value);

		void visit(IReturnNode value);

		void visit(ICommonConstructorCall value);

		void visit(ICompiledConstructorCall value);

		void visit(IWhileBreakNode value);

		void visit(IRepeatBreakNode value);

		void visit(IForBreakNode value);

		void visit(IWhileContinueNode value);

		void visit(IRepeatContinueNode value);

		void visit(IForContinueNode value);

		void visit(ICompiledConstructorNode value);

		void visit(ISimpleArrayNode value);

		void visit(ISimpleArrayIndexingNode value);

		void visit(IExternalStatementNode value);

		void visit(IRefTypeNode value);

		void visit(IGetAddrNode value);

		void visit(IDereferenceNode value);

		void visit(IThrowNode value);

		void visit(ISwitchNode value);

		void visit(ICaseVariantNode value);

		void visit(ICaseRangeNode value);

		void visit(INullConstantNode value);

		void visit(IUnsizedArray value);

		void visit(IRuntimeManagedMethodBody value);

		void visit(IAsNode value);

		void visit(IIsNode value);

		void visit(ISizeOfOperator value);

		void visit(ITypeOfOperator value);

		void visit(IExitProcedure value);

		void visit(ITryBlockNode value);

		void visit(IExceptionFilterBlockNode value);

		void visit(IArrayConstantNode value);

		void visit(IStatementsExpressionNode value);

		void visit(IQuestionColonExpressionNode value);

		void visit(IRecordConstantNode value);

		void visit(ILabelNode value);

		void visit(ILabeledStatementNode value);

		void visit(IGotoStatementNode value);

		void visit(ICompiledStaticMethodCallNodeAsConstant value);

		void visit(ICompiledConstructorCallAsConstant value);

		void visit(ICommonNamespaceFunctionCallNodeAsConstant value);

		void visit(IEnumConstNode value);

		void visit(IForeachNode value);

		void visit(ILockStatement value);

		void visit(ILocalBlockVariableNode value);

		void visit(ILocalBlockVariableReferenceNode value);

		void visit(IRethrowStatement value);

		void visit(IForeachBreakNode value);

		void visit(IForeachContinueNode value);

		void visit(INamespaceConstantReference value);

		void visit(IFunctionConstantReference value);

		void visit(ICommonConstructorCallAsConstant value);

		void visit(IArrayInitializer value);

		void visit(ICommonEventNode value);

		void visit(IEventNode value);

		void visit(ICompiledEventNode value);

		void visit(IStaticEventReference value);

		void visit(INonStaticEventReference value);

		void visit(IRecordInitializer value);

		void visit(IDefaultOperatorNode value);

		void visit(IAttributeNode value);

		void visit(IPInvokeStatementNode value);

		void visit(IBasicFunctionCallNodeAsConstant value);

		void visit(ICompiledStaticFieldReferenceNodeAsConstant value);

		void visit(ILambdaFunctionNode value);

		void visit(ILambdaFunctionCallNode value);

		void visit(ICommonNamespaceEventNode value);

		void visit(IDefaultOperatorNodeAsConstant value);

		void visit(IDoubleQuestionColonExpressionNode value);

		void visit(ITypeOfOperatorAsConstant value);

		void visit(ICommonStaticMethodCallNodeAsConstant value);

		void visit(ISizeOfOperatorAsConstant value);
	}
}
