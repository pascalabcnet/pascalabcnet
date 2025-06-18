using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Converter
{
    public class ListSyntRules
    {
        // хранит соответствие между узлами семантического дерева РАВСNET и соответствующими синтаксическими правилами другого языка
        private Dictionary<string, string> Rules;
        private string fileName;

        private const string nodeSeparator = "@", ruleSeparator = "::";

        public ListSyntRules(string _fileName)
        {
            fileName = _fileName;

            initialisation();
        }

        public string GetRule(string _nodeName)
        {
            string result;            
            if (Rules.TryGetValue(_nodeName, out result))
            {
                return result;
            }
            else
                return "";
        }

        public string[] GetWordsFromRule(string _node)
        {
            return GetRule(_node).Split(' ');
        }

        private void initialisation()
        {
            Rules = new Dictionary<string, string>();
            initRules(readRulesTXTInToStr());
        }

        private string readRulesTXTInToStr()
        {
            TextReader readerTXT = File.OpenText(fileName);
            return readerTXT.ReadToEnd();
        }

        private void initRules(string _rulesTXT)
        {
            string[] nameNodeAndRule;
            string[] nodes;
            string[] nodeSeparators = new string[1], ruleSeparators = new string[1];
            nodeSeparators[0] = nodeSeparator;
            ruleSeparators[0] = ruleSeparator;
            nodes = _rulesTXT.Split(nodeSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string node in nodes)
            {
                nameNodeAndRule = node.Split(ruleSeparators, StringSplitOptions.RemoveEmptyEntries);
                nameNodeAndRule[0] = nameNodeAndRule[0].Replace(" ", "");
                Rules.Add(nameNodeAndRule[0], nameNodeAndRule[1]);
            }
        }

    }
}
