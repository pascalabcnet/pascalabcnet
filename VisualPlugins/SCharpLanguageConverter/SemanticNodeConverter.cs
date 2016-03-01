using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SemanticTree;

namespace Converter
{
    public class CSharpSemanticNodeConverter : ISemanticNodeConverter
    {
        private SourceTextBilder sourceTextBuilder;
        public SourceTextBilder SourceTextBuilder
        {
            set
            {
                sourceTextBuilder = value;
            }
            get
            {
                return sourceTextBuilder;
            }
        }

        public CSharpSemanticNodeConverter()
        {
            SourceTextBuilder = new SourceTextBilder();
        }

        public CSharpSemanticNodeConverter(ListSyntRules _listSyntRules, TextFormatter _textFormatter)
        {
            SourceTextBuilder = new SourceTextBilder(_listSyntRules, _textFormatter);
        }

        public string GetConvertedText()
        {
            return SourceTextBuilder.GetNodeFromStack();
        }

        public virtual string ConvertPABCNETNodeProgram(string _nodeName, object _node)
        {
            // IProgramNode - основной узел, содержит весь текст программы, 
            // обрабатывается визитором последним.
            return SourceTextBuilder.GetNodeFromStack();
        }

        public virtual string ConvertPABCNETNodeClass(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeNamespace(string _nodeName, object _node)
        {
            //SourceTextBuilder.TextFormatter.Indents.BlockBodyIncrement();
            string nodeContext = SourceTextBuilder.ConvertNode(_nodeName, _node);
            //SourceTextBuilder.TextFormatter.Indents.BlockBodyDecrement();
            return nodeContext;
        }

        public virtual string ConvertPABCNETNodeBlock(string _nodeName, object _node)
        {
            return "";
        }

        public virtual string ConvertPABCNETNodeType(string _nodeName, object _node)
        {
            if ((_node as ICommonTypeNode).is_value_type)
                return SourceTextBuilder.ConvertNode("struct", _node);
            else // сделать для шаблонов, параметризованных щаблонов, классов.
                return SourceTextBuilder.ConvertNode("class", _node);

        }

        public virtual string ConvertPABCNETNodeFunction(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeVariable(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeStatementsNested(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeStatements(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeAssign(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeVariableReference(string _nodeName, object _node)
        {
            //  возвращаем имя переменной
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeParameterReference(string _nodeName, object _node)
        {
            //  возвращаем имя переменной
            return (_node as ICommonParameterReferenceNode).parameter.name;
        }

        public virtual string ConvertPABCNETNodeBoolConstant(string _nodeName, object _node)
        {
            //  возвращаем имя переменной
            return (_node as IBoolConstantNode).constant_value.ToString();
        }

        public virtual string ConvertPABCNETNodeFunctionCall(string _nodeName, object _node)
        {
            //  возвращаем имя переменной
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeParameter(string _nodeName, object _node)
        {
            string type = "";
            //  возвращаем имя  и тип переменной
            if ((_node as ICommonParameterNode).type is ICompiledTypeNode)
                type = ((_node as ICommonParameterNode).type as ICompiledTypeNode).compiled_type.ToString();
            if ((_node as ICommonParameterNode).type is ICommonTypeNode)
                type = ((_node as ICommonParameterNode).type as ICommonTypeNode).name;
            return type + " " + (_node as ICommonParameterNode).name;
        }

        public virtual string ConvertPABCNETNodeNamespaceVariable(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeNamespaceConst(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeIf(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeLocalBlockVariable(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeStaticMethodCall(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeTypeOf(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeIntConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return (_node as IIntConstantNode).constant_value.ToString();
        }

        public virtual string ConvertPABCNETNodeByteConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return (_node as IByteConstantNode).constant_value.ToString();
        }
        public virtual string ConvertPABCNETNodeSByteConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return (_node as ISByteConstantNode).constant_value.ToString();
        }
        public virtual string ConvertPABCNETNodeShortConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return (_node as IShortConstantNode).constant_value.ToString();
        }
        public virtual string ConvertPABCNETNodeUShortConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return (_node as IUShortConstantNode).constant_value.ToString();
        }
        public virtual string ConvertPABCNETNodeUIntConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return (_node as IUIntConstantNode).constant_value.ToString();
        }
        public virtual string ConvertPABCNETNodeULongConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return (_node as IULongConstantNode).constant_value.ToString();
        }

        public virtual string ConvertPABCNETNodeLongConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return (_node as ILongConstantNode).constant_value.ToString();
        }

        public virtual string ConvertPABCNETNodeDoubleConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return (_node as IDoubleConstantNode).constant_value.ToString();
        }

        public virtual string ConvertPABCNETNodeFloatConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return (_node as IFloatConstantNode).constant_value.ToString();
        }

        public virtual string ConvertPABCNETNodeCharConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return "'" + (_node as ICharConstantNode).constant_value.ToString() + "'";
        }

        public virtual string ConvertPABCNETNodeStringConstant(string _nodeName, object _node)
        {
            //  возвращаем число integer
            return "\"" + (_node as IStringConstantNode).constant_value.ToString() + "\"";
        }

        public virtual string ConvertPABCNETNodeWhile(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeRepeat(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeFor(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeSimpleArrInd(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeAdd(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeSubtract(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeMult(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeDiv(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeMod(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeInc(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeDec(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeBinNot(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeUnNot(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeShr(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeShl(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeReturn(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeEq(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeNotEq(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeGr(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeGrEq(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeSm(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeSmEq(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeAnd(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeOr(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeXor(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeUnMin(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeLocVariableReference(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeNullConst(string _nodeName, object _node)
        {
            return "null";
        }

        public virtual string ConvertPABCNETNodeConstructorCall(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeCompMethodCall(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeIfElse(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeUnInc(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }

        public virtual string ConvertPABCNETNodeUnDec(string _nodeName, object _node)
        {
            return SourceTextBuilder.ConvertNode(_nodeName, _node);
        }
        public override string ToString()
        {
            return "C#";
        }
    }
}
