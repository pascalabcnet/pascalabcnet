using System;
using System.IO;
using System.Text;
using System.Reflection;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.Errors;
using System.Collections.Generic;
using GPPGParserScanner;

namespace PascalABCCompiler.KuMir00Parser
{
    public class KuMir00LanguageParser : BaseParser
    {
        public KuMir00LanguageParser()
            : base("KuMir00", "0.1", "(c) ZM, 2010", true, new string[] { ".alg00" })
        {
        }

        public override syntax_tree_node BuildTreeInNormalMode(string FileName, string Text)
        {            
            PT.Errors = Errors;
            PT.CurrentFileName = FileName;
            
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            
            GPPGParser parser = new GPPGParser(scanner);

            parser._units.Add(new GPPGParser.unit_data());
            parser.unit_number++;
                       
            if (!parser.Parse())
                if (Errors.Count == 0)
                    PT.AddError("Неопознанная синтаксическая ошибка!", null);

            return parser.root;
        }

        public override syntax_tree_node BuildTreeInExprMode(string FileName, string Text)
        {
            PT.Errors = Errors;
            PT.CurrentFileName = FileName;

            Scanner scanner = new Scanner();

            Text = String.Concat((char)1, Text);
            scanner.SetSource(Text, 0);

            GPPGParser parser = new GPPGParser(scanner);

            parser._units.Add(new GPPGParser.unit_data());
            parser.unit_number++;

            if (!parser.Parse())
                if (Errors.Count == 0)
                    PT.AddError("Не разобрали выражение", null);

            return parser.root;
        }
    }
}
