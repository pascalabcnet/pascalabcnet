using System;
using System.Collections.Generic;
using System.Text;

namespace Converter
{
    public class SourceTextBilder
    {
        public ListSyntRules ListSyntRules;
        public TextFormatter TextFormatter;
        private StringBuilder sourceString = new StringBuilder("");
        private StringBuilder sourceStringNode = new StringBuilder("");
        private StringBuilder sourceStringNodeBlock = new StringBuilder("");
        private Stack<string> nodeStringStack = new Stack<string>();
        public Dictionary<string, string> namespacesStr = new Dictionary<string,string>();

        public SourceTextBilder()
        {
            TextFormatter = new TextFormatter();
            // todo
            ListSyntRules = new ListSyntRules(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName) + "\\Rules_CS.txt");

            TextFormatter.GetBodyBlock += getBodyBlock;
        }

        public SourceTextBilder(ListSyntRules _listSyntRules, TextFormatter _textFormatter)
        {
            ListSyntRules = _listSyntRules;
            TextFormatter = _textFormatter;

            TextFormatter.GetBodyBlock += getBodyBlock;
        }

        private bool isEmptyString(string res)
        {           
            // состоит ли строка только из пробелов            
            for (int i = 0; i < res.Length; i++)
            {
                if (!(res[i] == ' ' || res[i] == '\r' || res[i] == '\n'))
                    return false;                
            }
            return true;
        }

        private string getBodyBlock()
        {
            string res = "";
            while (isEmptyString(res) && (nodeStringStack.Count != 0))
            //if ((nodeStringStack.Count != 0))
            {
                res = nodeStringStack.Pop();                
            }
            if (res == "%empty%")
                res = "";
            return res;
        }

        private void addNodeInToString(StringBuilder _string, string _nodeName, object _node)
        {
            _string.Append(TextFormatter.Indents.BlockBody);
            _string.Append(TextFormatter.FormatRuleNode(ListSyntRules.GetRule(_nodeName), _node));
            _string.Append(Environment.NewLine);
        }

        public void AddNodeInToStack(string _node)
        {
            nodeStringStack.Push(_node);
        }

        public string GetNodeFromStack()
        {
            if (nodeStringStack.Count != 0)
                return nodeStringStack.Pop();
            else
                return "";
             
        }        

        public string ConvertNode (string _nodeName, object _node)
        {
            //addNodeInToString(sourceString, _nodeName, _node);               
            return TextFormatter.FormatRuleNode(ListSyntRules.GetRule(_nodeName), _node);
        }

        public void AddInToSourceText(string _text)
        {
            sourceString.Append(_text);
        }
    }
}
