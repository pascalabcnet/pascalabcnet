using System;
namespace Converter
{
    public interface ISemanticNodeConverter
    {
        string ConvertPABCNETNodeAdd(string _nodeName, object _node);
        string ConvertPABCNETNodeAnd(string _nodeName, object _node);
        string ConvertPABCNETNodeAssign(string _nodeName, object _node);
        string ConvertPABCNETNodeBlock(string _nodeName, object _node);
        string ConvertPABCNETNodeBoolConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeByteConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeCharConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeClass(string _nodeName, object _node);
        string ConvertPABCNETNodeDec(string _nodeName, object _node);
        string ConvertPABCNETNodeDiv(string _nodeName, object _node);
        string ConvertPABCNETNodeDoubleConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeEq(string _nodeName, object _node);
        string ConvertPABCNETNodeFloatConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeFor(string _nodeName, object _node);
        string ConvertPABCNETNodeFunction(string _nodeName, object _node);
        string ConvertPABCNETNodeFunctionCall(string _nodeName, object _node);
        string ConvertPABCNETNodeGr(string _nodeName, object _node);
        string ConvertPABCNETNodeGrEq(string _nodeName, object _node);
        string ConvertPABCNETNodeIf(string _nodeName, object _node);
        string ConvertPABCNETNodeInc(string _nodeName, object _node);
        string ConvertPABCNETNodeIntConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeLocalBlockVariable(string _nodeName, object _node);
        string ConvertPABCNETNodeLocVariableReference(string _nodeName, object _node);
        string ConvertPABCNETNodeLongConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeMod(string _nodeName, object _node);
        string ConvertPABCNETNodeMult(string _nodeName, object _node);
        string ConvertPABCNETNodeNamespace(string _nodeName, object _node);
        string ConvertPABCNETNodeNamespaceConst(string _nodeName, object _node);
        string ConvertPABCNETNodeNamespaceVariable(string _nodeName, object _node);
        string ConvertPABCNETNodeBinNot(string _nodeName, object _node);
        string ConvertPABCNETNodeUnNot(string _nodeName, object _node);
        string ConvertPABCNETNodeNotEq(string _nodeName, object _node);
        string ConvertPABCNETNodeNullConst(string _nodeName, object _node);
        string ConvertPABCNETNodeOr(string _nodeName, object _node);
        string ConvertPABCNETNodeParameter(string _nodeName, object _node);
        string ConvertPABCNETNodeParameterReference(string _nodeName, object _node);
        string ConvertPABCNETNodeProgram(string _nodeName, object _node);
        string ConvertPABCNETNodeRepeat(string _nodeName, object _node);
        string ConvertPABCNETNodeReturn(string _nodeName, object _node);
        string ConvertPABCNETNodeSByteConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeShl(string _nodeName, object _node);
        string ConvertPABCNETNodeShortConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeShr(string _nodeName, object _node);
        string ConvertPABCNETNodeSimpleArrInd(string _nodeName, object _node);
        string ConvertPABCNETNodeSm(string _nodeName, object _node);
        string ConvertPABCNETNodeSmEq(string _nodeName, object _node);
        string ConvertPABCNETNodeStatements(string _nodeName, object _node);
        string ConvertPABCNETNodeStatementsNested(string _nodeName, object _node);
        string ConvertPABCNETNodeStaticMethodCall(string _nodeName, object _node);
        string ConvertPABCNETNodeStringConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeSubtract(string _nodeName, object _node);
        string ConvertPABCNETNodeType(string _nodeName, object _node);
        string ConvertPABCNETNodeTypeOf(string _nodeName, object _node);
        string ConvertPABCNETNodeUIntConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeULongConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeUnMin(string _nodeName, object _node);
        string ConvertPABCNETNodeUShortConstant(string _nodeName, object _node);
        string ConvertPABCNETNodeVariable(string _nodeName, object _node);
        string ConvertPABCNETNodeVariableReference(string _nodeName, object _node);
        string ConvertPABCNETNodeWhile(string _nodeName, object _node);
        string ConvertPABCNETNodeXor(string _nodeName, object _node);
        string GetConvertedText();
        SourceTextBilder SourceTextBuilder { get; set; }

        string ConvertPABCNETNodeConstructorCall(string _nodeName, object _node);
        string ConvertPABCNETNodeCompMethodCall(string _nodeName, object _node);
        string ConvertPABCNETNodeIfElse(string _nodeName, object _node);
        string ConvertPABCNETNodeUnInc(string _nodeName, object _node);
        string ConvertPABCNETNodeUnDec(string _nodeName, object _node);

    }
}
