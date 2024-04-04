// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace PascalABCCompiler.SemanticTree
{

	public abstract class AbstractVisitor : ISemanticVisitor
	{

		public virtual void visit(ISemanticNode value)
		{
		}

		public virtual void visit(IDefinitionNode value)
		{
		}

		public virtual void visit(ITypeNode value)
		{
		}

		public virtual void visit(IBasicTypeNode value)
		{
		}

		public virtual void visit(ICommonTypeNode value)
		{
		}

		public virtual void visit(ICompiledTypeNode value)
		{
		}

		public virtual void visit(IStatementNode value)
		{
		}

		public virtual void visit(IExpressionNode value)
		{
		}

		public virtual void visit(IFunctionCallNode value)
		{
		}

		public virtual void visit(IBasicFunctionCallNode value)
		{
		}

		public virtual void visit(ICommonNamespaceFunctionCallNode value)
		{
		}

		public virtual void visit(ICommonNestedInFunctionFunctionCallNode value)
		{
		}

		public virtual void visit(ICommonMethodCallNode value)
		{
		}

		public virtual void visit(ICommonStaticMethodCallNode value)
		{
		}

		public virtual void visit(ICompiledMethodCallNode value)
		{
		}

		public virtual void visit(ICompiledStaticMethodCallNode value)
		{
		}

		public virtual void visit(IFunctionNode value)
		{
		}

		public virtual void visit(IClassMemberNode value)
		{
		}

		public virtual void visit(ICompiledClassMemberNode value)
		{
		}

		public virtual void visit(ICommonClassMemberNode value)
		{
		}

		public virtual void visit(IFunctionMemberNode value)
		{
		}

		public virtual void visit(INamespaceMemberNode value)
		{
		}

		public virtual void visit(IBasicFunctionNode value)
		{
		}

		public virtual void visit(ICommonFunctionNode value)
		{
		}

		public virtual void visit(ICommonNamespaceFunctionNode value)
		{
		}

		public virtual void visit(ICommonNestedInFunctionFunctionNode value)
		{
		}

		public virtual void visit(ICommonMethodNode value)
		{
		}

		public virtual void visit(ICompiledMethodNode value)
		{
		}

		public virtual void visit(IIfNode value)
		{
		}

		public virtual void visit(IWhileNode value)
		{
		}

		public virtual void visit(IRepeatNode value)
		{
		}

		public virtual void visit(IForNode value)
		{
		}

		public virtual void visit(IStatementsListNode value)
		{
		}

		public virtual void visit(INamespaceNode value)
		{
		}

		public virtual void visit(ICommonNamespaceNode value)
		{
		}

		public virtual void visit(ICompiledNamespaceNode value)
		{
		}

		public virtual void visit(IDllNode value)
		{
		}

		public virtual void visit(IProgramNode value)
		{
		}

		public virtual void visit(IAddressedExpressionNode value)
		{
		}

		public virtual void visit(ILocalVariableReferenceNode value)
		{
		}

		public virtual void visit(INamespaceVariableReferenceNode value)
		{
		}

		public virtual void visit(ICommonClassFieldReferenceNode value)
		{
		}

		public virtual void visit(IStaticCommonClassFieldReferenceNode value)
		{
		}

		public virtual void visit(ICompiledFieldReferenceNode value)
		{
		}

		public virtual void visit(IStaticCompiledFieldReferenceNode value)
		{
		}

		public virtual void visit(ICommonParameterReferenceNode value)
		{
		}

		public virtual void visit(IConstantNode value)
		{
		}

		public virtual void visit(IBoolConstantNode value)
		{
		}

		public virtual void visit(IIntConstantNode value)
		{
		}

        public virtual void visit(ILongConstantNode value)
        {
        }

        public virtual void visit(IByteConstantNode value)
        {
        }

        public virtual void visit(IFloatConstantNode value)
        {
        }
        
        public virtual void visit(IDoubleConstantNode value)
		{
		}

		public virtual void visit(ICharConstantNode value)
		{
		}

		public virtual void visit(IStringConstantNode value)
		{
		}

		public virtual void visit(IVAriableDefinitionNode value)
		{
		}

		public virtual void visit(ILocalVariableNode value)
		{
		}

		public virtual void visit(ICommonNamespaceVariableNode value)
		{
		}

		public virtual void visit(ICommonClassFieldNode value)
		{
		}

		public virtual void visit(ICompiledClassFieldNode value)
		{
		}

		public virtual void visit(IParameterNode value)
		{
		}

		public virtual void visit(ICommonParameterNode value)
		{
		}

		public virtual void visit(IBasicParameterNode value)
		{
		}

		public virtual void visit(ICompiledParameterNode value)
		{
		}

		public virtual void visit(IConstantDefinitionNode value)
		{
		}

        public virtual void visit(IClassConstantDefinitionNode value)
        {
        }

        public virtual void visit(ICompiledClassConstantDefinitionNode value)
        {
        }

        public virtual void visit(INamespaceConstantDefinitionNode value)
        {
        }

        public virtual void visit(ICommonFunctionConstantDefinitionNode value)
        {
        }

		public virtual void visit(IPropertyNode value)
		{
			// TODO:  Add AbstractVisitor.SemanticTree.ISemanticVisitor.visit implementation
		}

		public virtual void visit(ICommonPropertyNode value)
		{
			// TODO:  Add AbstractVisitor.SemanticTree.ISemanticVisitor.visit implementation
		}

		public virtual void visit(IBasicPropertyNode value)
		{
			// TODO:  Add AbstractVisitor.SemanticTree.ISemanticVisitor.visit implementation
		}

		public virtual void visit(ICompiledPropertyNode value)
		{
			// TODO:  Add AbstractVisitor.SemanticTree.ISemanticVisitor.visit implementation
		}

		public virtual void visit(IThisNode value)
		{
			// TODO:  Add AbstractVisitor.SemanticTree.ISemanticVisitor.visit implementation
		}

		public virtual void visit(IReturnNode value)
		{
			// TODO:  Add AbstractVisitor.SemanticTree.ISemanticVisitor.visit implementation
		}

		public virtual void visit(ICommonConstructorCall value)
		{

		}

		public virtual void visit(ICompiledConstructorCall value)
		{

		}

		public virtual void visit(IWhileBreakNode value)
		{
			
		}

		public virtual void visit(IRepeatBreakNode value)
		{

		}

		public virtual void visit(IForBreakNode value)
		{

		}

		public virtual void visit(IWhileContinueNode value)
		{

		}

		public virtual void visit(IRepeatContinueNode value)
		{

		}

		public virtual void visit(IForContinueNode value)
		{

		}

		public virtual void visit(ICompiledConstructorNode value)
		{

		}

		public virtual void visit(ISimpleArrayNode value)
		{
			
		}

		public virtual void visit(ISimpleArrayIndexingNode value)
		{
			
		}
		

		public virtual void visit(IExternalStatementNode value)
		{
			
		}
		
		public virtual void visit(IRefTypeNode value)
		{
			
		}
		
		public virtual void visit(IGetAddrNode value)
		{
			
		}
		
		public virtual void visit(IDereferenceNode value)
		{
			
		}

        public virtual void visit(IThrowNode value)
        {

        }

        public virtual void visit(ISwitchNode value)
        {

        }


        public virtual void visit(ICaseVariantNode value)
        {

        }

        public virtual void visit(ICaseRangeNode value)
        {

        }

	    public virtual void visit(INullConstantNode value)
        {
            
        }

        public virtual void visit(IUnsizedArray value)
        {
        }

        public virtual void visit(IRuntimeManagedMethodBody value)
        {
        }

        public virtual void visit(ISByteConstantNode value)
        {
        }

        public virtual void visit(IShortConstantNode value)
        {
        }

        public virtual void visit(IUShortConstantNode value)
        {
        }

        public virtual void visit(IUIntConstantNode value)
        {
        }

        public virtual void visit(IULongConstantNode value)
        {
        }

        public virtual void visit(IIsNode value)
        {
        }

        public virtual void visit(IAsNode value)
        {
        }
        
        public virtual void visit(ISizeOfOperator value)
        {
        }

        public virtual void visit(ITypeOfOperator value)
        {
        }

        public virtual void visit(IExitProcedure value)
        {
        }

        public virtual void visit(ITryBlockNode value)
        {
        }

        public virtual void visit(IExceptionFilterBlockNode value)
        {
        }

        public virtual void visit(IArrayConstantNode value)
        {
        }

        public virtual void visit(IStatementsExpressionNode value)
        {
        }

        public virtual void visit(IQuestionColonExpressionNode value)
        {
        }
        
        public virtual void visit(IRecordConstantNode value)
        {
        	
        }

        public virtual void visit(ILabelNode value)
        {
        	
        }

        public virtual void visit(ILabeledStatementNode value)
        {
        	
        }

        public virtual void visit(IGotoStatementNode value)
        {
        	
        }

        public virtual void visit(ICompiledStaticMethodCallNodeAsConstant value)
        {
        	
        }

        public virtual void visit(ICommonNamespaceFunctionCallNodeAsConstant value)
        {
        	
        }

        public virtual void visit(IEnumConstNode value)
        {
        	
        }

        public virtual void visit(IForeachNode value)
        {
        	
        }
        
        public virtual void visit(ILockStatement value)
        {
        	
        }

        public virtual void visit(ILocalBlockVariableNode value)
        {
        }

        public virtual void visit(ILocalBlockVariableReferenceNode value)
        {
        }
        public virtual void visit(ICompiledConstructorCallAsConstant value)
        {
        }
        
        public virtual void visit(IRethrowStatement value)
        {
        	
        }
        
        public virtual void visit(IForeachBreakNode value)
        {
        	
        }
        
        public virtual void visit(IForeachContinueNode value)
        {
        	
        }
        
		public virtual void visit(INamespaceConstantReference value)
		{

		}
		
		public virtual void visit(IFunctionConstantReference value)
		{

		}
        
		public virtual void visit(ICommonConstructorCallAsConstant value)
		{
			
		}
		
		public virtual void visit(IArrayInitializer value)
		{
			
		}
		
		public virtual void visit(ICommonEventNode value)
		{
			
		}
		
		public virtual void visit(IEventNode value)
		{
			
		}
        
        public virtual void visit(ICompiledEventNode value)
        {
        	
        }
        
        public virtual void visit(IStaticEventReference value)
        {
        	
        }
        
        public virtual void visit(INonStaticEventReference value)
        {
        	
        }
        
        public virtual void visit(IRecordInitializer value)
        {
        	
        }

        public virtual void visit(IDefaultOperatorNode value)
        {

        }
        
        public virtual void visit(IAttributeNode value)
        {
        	
        }
        
        public virtual void visit(IPInvokeStatementNode value)
        {
        	
        }
        
        public virtual void visit(IBasicFunctionCallNodeAsConstant value)
        {
        	
        }

        public virtual void visit(ICompiledStaticFieldReferenceNodeAsConstant value)
        {
        }

        public virtual void visit(ILambdaFunctionNode value)
        {

        }
        public virtual void visit(ILambdaFunctionCallNode value)
        {

        }

        public virtual void visit(ICommonNamespaceEventNode value)
        {
        }

        public virtual void visit(IDefaultOperatorNodeAsConstant value)
        {

        }

        public virtual void visit(IDoubleQuestionColonExpressionNode value)
        {
		
        }

        public virtual void visit(ITypeOfOperatorAsConstant value)
        {
            
        }

		public virtual void visit(ICommonStaticMethodCallNodeAsConstant value)
        {

        }

		public virtual void visit(ISizeOfOperatorAsConstant value)
        {

        }

	}

}
