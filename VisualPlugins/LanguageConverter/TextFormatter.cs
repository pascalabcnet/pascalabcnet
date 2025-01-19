using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using PascalABCCompiler.SemanticTree;
using System.Text.RegularExpressions;

namespace Converter
{
    public class TextFormatter
    {
        private Dictionary<string, getFieldValueFromSemanticNode> FieldValuesFromSemanticNode = new Dictionary<string, getFieldValueFromSemanticNode>();
        private List<string> SpecSymbols = new List<string>();

        private delegate string getFieldValueFromSemanticNode(object node);
        public delegate string convertKeyWord(string _nameKeyWord, string _factVaulekeyWord);
        public delegate string getBodyBlock();

        public convertKeyWord ConvertKeyWord;
        public getBodyBlock GetBodyBlock;

        public IndentValues Indents = new IndentValues();

        public struct IndentValues
        {
            private Dictionary<string, string> indents;
            private string blockBodyInSourceText;
            private string blockBody;
            public void init()
            {
                indents = new Dictionary<string, string>();
                indents.Add(IndentNames.BlockBegin, Environment.NewLine);
                indents.Add(IndentNames.BlockEnd, Environment.NewLine);
                indents.Add(IndentNames.BlockBody, "    ");
                indents.Add(IndentNames.BetweenWords, " ");

                blockBody = indents[IndentNames.BlockBody];
                blockBodyInSourceText = indents[IndentNames.BlockBody];
            }
            public string BlockBodyIncrement()
            {               
                return indents[IndentNames.BlockBody] += blockBody;
            }
            public string BlockBodyDecrement()
            {
                return indents[IndentNames.BlockBody] = indents[IndentNames.BlockBody].Remove(indents[IndentNames.BlockBody].Length - blockBody.Length);
            }
            public string BlockBodyInSourceText
            {
                get
                {
                    return blockBodyInSourceText;
                }
            }
            public string BlockBegin
            {
                set
                {
                    indents[IndentNames.BlockBegin] = value;

                }
                get
                {
                    return indents[IndentNames.BlockBegin];
                }
            }
            public string BlockEnd
            {
                set
                {
                    indents[IndentNames.BlockEnd] = value;
                }
                get
                {
                    return indents[IndentNames.BlockEnd];
                }
            }
            public string BlockBody
            {
                set
                {
                    blockBody = value;
                    indents[IndentNames.BlockBody] = value;
                }
                get
                {
                    return indents[IndentNames.BlockBody];
                }
            }
            public string BetweenWords
            {
                set
                {
                    indents[IndentNames.BetweenWords] = value;
                }
                get
                {
                    return indents[IndentNames.BetweenWords];
                }
            }
            public string GetIndent(string _indentName)
            {
                if (indents.ContainsKey(_indentName))
                    return indents[_indentName];
                return "";
            }
        }

        public struct IndentNames
        {
            public static string BlockBegin = "BlockBegin";
            public static string BlockEnd = "BlockEnd";
            public static string BlockBody = "BlockBody";
            public static string BetweenWords = "BW";
        }

        public struct SpecSymbValues
        {
            public static string KeyWord = "$";
            public static string Indent = "*";
        }

        public struct SemanticNodeFields
        {
            public static string Body = "body";
            public static string Expr = "expr";
            public static string ObjName = "obj_name";
            public static string Parameters = "parameters";            
            public static string Name = "name";
            public static string Type = "type";
            public static string Value = "value";
            public static string AccessModifierField = "access_modifier_field";
            public static string AccessModifierType = "access_modifier_type";
        }



        public TextFormatter()
        {
            init();
        }

        private void init()
        {
            Indents.init();
            specSymbolsInit();
            semanticNodeFieldsMethodsInit();
        }
        private void specSymbolsInit()
        {
            SpecSymbols.Add(SpecSymbValues.KeyWord);
            SpecSymbols.Add(SpecSymbValues.Indent);
        }

        private void semanticNodeFieldsMethodsInit()
        {               
            FieldValuesFromSemanticNode.Add(SemanticNodeFields.Name, getFieldValueFromSemanticNodeName);
            FieldValuesFromSemanticNode.Add(SemanticNodeFields.Type, getFieldValueFromSemanticNodeType);
            FieldValuesFromSemanticNode.Add(SemanticNodeFields.AccessModifierField, getFieldValueFromSemanticNodeAccessModifierField);
            FieldValuesFromSemanticNode.Add(SemanticNodeFields.AccessModifierType, getFieldValueFromSemanticNodeAccessModifierType);            
        }

        // методы, возвращающие фактическое значение соответствующего поля для данного узла 
        #region
        // вот они:           
        private string getFieldValueFromSemanticNodeName(object _node)
        {
            if (_node is ICommonNamespaceNode)
            {
                string name = (_node as ICommonNamespaceNode).namespace_name;
                if (name == "")
                    // Задавать пользователем дефолтное имя главного неймспейса!!!
                    name = "Program";
                return name;
            }

            if (_node is ILocalVariableReferenceNode)
                return (_node as ILocalVariableReferenceNode).Variable.name;

            if (_node is INamespaceVariableReferenceNode)
                return (_node as INamespaceVariableReferenceNode).Variable.name;

            if (_node is ILocalBlockVariableReferenceNode)
                return (_node as ILocalBlockVariableReferenceNode).Variable.name;

            if (_node is ICompiledStaticMethodCallNode)
                return (_node as ICompiledStaticMethodCallNode).compiled_type.compiled_type.ToString() + "." + (_node as ICompiledStaticMethodCallNode).static_method.name;

            if (_node is ICompiledMethodCallNode)
                return (_node as ICompiledMethodCallNode).compiled_method.name;

            if (_node is ITypeNode)
                return (_node as ITypeNode).name;

            if (_node is ITypeSynonym)
                return (_node as ITypeSynonym).name;

            if (_node is IFunctionNode)
                return (_node as IFunctionNode).name;

            if (_node is ICommonNamespaceFunctionCallNode)
                return (_node as ICommonNamespaceFunctionCallNode).function.name;

            if (_node is IExternalStatementNode)
                return (_node as IExternalStatementNode).name;

            if (_node is IVAriableDefinitionNode)
                return (_node as IVAriableDefinitionNode).name;

            if (_node is IConstantDefinitionNode)
                return (_node as IConstantDefinitionNode).name;

            if (_node is IPropertyNode)
                return (_node as IPropertyNode).name;

            if (_node is ILabelNode)
                return (_node as ILabelNode).name;

            return "";
        }

        private string getType(object _node)
        {
            if (_node is ICompiledTypeNode)
                return (_node as ICompiledTypeNode).compiled_type.ToString();
            if (_node is ICommonTypeNode)
                if ((_node as ICommonTypeNode).comprehensive_namespace.namespace_name != "")
                    return (_node as ICommonTypeNode).comprehensive_namespace.namespace_name + "." + (_node as ICommonTypeNode).name;
                else 
                    return (_node as ICommonTypeNode).name;
            return "";
        }

        private string getFieldValueFromSemanticNodeType(object _node)
        {
            if (_node is ICommonNamespaceFunctionNode)
            {// TODO: сделать определения для разных языков нормальное
                if ((_node as ICommonNamespaceFunctionNode).return_value_type == null)
                    return "void";
                else
                    return getType((_node as ICommonNamespaceFunctionNode).return_value_type);
            }

            if (_node is ITypeOfOperator)
                return getType((_node as ITypeOfOperator).oftype); 

            if (_node is ICommonConstructorCall)
                return getType((_node as ICommonConstructorCall).common_type);

            if (_node is ICompiledTypeNode)
                return (_node as ICompiledTypeNode).compiled_type.ToString();

            if (_node is IExpressionNode)
                return getType((_node as IExpressionNode).type);

            if (_node is IVAriableDefinitionNode)
                return getType((_node as IVAriableDefinitionNode).type);

            if (_node is IConstantDefinitionNode)
                return getType((_node as IConstantDefinitionNode).type);

            return "";
        }

        private string getFieldValueFromSemanticNodeAccessModifierField(object _node)
        {
            //fal_private, fal_internal, fal_protected, fal_public
            if (_node is IClassMemberNode)
            {
                //поместить в класс семант. правил структуру с их строковыми значениями
                IClassMemberNode node = _node as IClassMemberNode;

                if ((node).field_access_level == field_access_level.fal_internal)
                    return "internal" + Indents.BetweenWords;

                if ((node).field_access_level == field_access_level.fal_private)
                    return "private" + Indents.BetweenWords;

                if ((node).field_access_level == field_access_level.fal_protected)
                    return "protected" + Indents.BetweenWords;

                if ((node).field_access_level == field_access_level.fal_public)
                    return "public" + Indents.BetweenWords;
            }

            return "";
        }

        private string getFieldValueFromSemanticNodeAccessModifierType(object _node)
        {
            // структуру в абстр. класс для хранения соотв. значений в Сишарпе (и других языках)
            string fieldValue = "";
            if (_node is ICommonTypeNode)
            {
                ICommonTypeNode node = _node as ICommonTypeNode;

                if ((node).type_access_level == type_access_level.tal_internal)
                    return "internal" + Indents.BetweenWords;

                if ((node).type_access_level == type_access_level.tal_public)
                    return "public" + Indents.BetweenWords;
            }
            return "";
        }

        #endregion

        private string getWordValue(string _word, string _specSymb)
        {
            if (_specSymb == SpecSymbValues.KeyWord)
                return _word;

            if (_specSymb == SpecSymbValues.Indent)
                return Indents.GetIndent(_word);

            return "";
        }

        public string FormatWord(string _word, object _node)
        {
            // cltkfnm yjhvfkmyj
            if (_word != "")
            {
                _word = _word.Replace(" ", "");
                _word = _word.Replace("\r", "");
                _word = _word.Replace("\n", "");
            }
            else return _word;

            foreach (string specSymbol in this.SpecSymbols)
            {
                if (_word.Contains(specSymbol))
                {
                    _word = _word.Replace(specSymbol, "");
                    return getWordValue(_word, specSymbol);
                }
            }
            if (_word == SemanticNodeFields.Body || _word == SemanticNodeFields.Expr || _word == SemanticNodeFields.Parameters || _word == SemanticNodeFields.ObjName)
            {
                if (GetBodyBlock != null)
                    return GetBodyBlock();
            }
            if (FieldValuesFromSemanticNode.ContainsKey(_word))
            {
                string word = FieldValuesFromSemanticNode[_word](_node);                
                return replSymb(word);                
            }
            return _word;
        }

        private string replSymb(string _word)
        {
            _word = _word.Replace("$", "S");          
            return _word;            
        }

        private string deleteSymbolsFromWord(string _word)
        {
            // переделать этот ужас!!!
            _word = _word.Replace("$", "");
            _word = _word.Replace("#", "");
            return _word;
        }


        // если привязан спец. обработчик для ключ. слов - вызывается, иначе возвр. точное значение, как в PABCNET
        public string FormatWordWithKeyWord(string _word, object _node)
        {
            if (_word != "")
            {
                _word = _word.Replace(" ", "");
                _word = _word.Replace("\r", "");
                _word = _word.Replace("\n", "");
            }
            else return _word;

            foreach (string specSymbol in this.SpecSymbols)
            {
                if (_word.Contains(specSymbol))
                {
                    _word = _word.Replace(specSymbol, "");
                    return getWordValue(_word, specSymbol);
                }
                else
                {
                    if (ConvertKeyWord != null)
                        return ConvertKeyWord(_word, FieldValuesFromSemanticNode[_word](_node));
                    else
                        return FieldValuesFromSemanticNode[_word](_node);
                }
            }

            return _word;
        }

        // значения, соответствующие ключевым словам в PABCNET, преобразуются методом, привязанным к делегату
        public string FormatRuleNodeWithKeyWord(string _ruleNode, object _node)
        {
            StringBuilder nodeString = new StringBuilder("");
            string[] ruleNode = _ruleNode.Split(' ');

            foreach (string nodeWord in ruleNode)
                nodeString.Append(this.FormatWordWithKeyWord(nodeWord, _node));

            return nodeString.ToString();
        }

        // для языков, у которых значения спецификаторов(?) строго совпадают с соответствующими в PascalABCNET
        public string FormatRuleNode(string _ruleNode, object _node)
        {
            StringBuilder nodeString = new StringBuilder("");
            string[] ruleNode = _ruleNode.Split(' ');

            foreach (string nodeWord in ruleNode)
                nodeString.Append(this.FormatWord(nodeWord, _node));            

            return nodeString.ToString();
        }
    }
}
