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

namespace PascalABCCompiler.Oberon00Parser
{
    public class Oberon00LanguageParser : BaseParser
    {        

        public Oberon00LanguageParser()
            : base("Oberon00", "0.1", "(c) Stanislav Mikhalkovich, 2010", true, new string[] { ".obr" })
        {
        }

        public override syntax_tree_node BuildTreeInNormalMode(string FileName, string Text)
        {            
            PT.Errors = Errors;
            PT.CurrentFileName = FileName;
            
            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            
            GPPGParser parser = new GPPGParser(scanner);
                                   
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

            // Добавление в начало текста символа с кодом 1. Используется для парсинга выражений с последующим использованием в Intellisense 
            Text = String.Concat((char)1, Text);
            scanner.SetSource(Text, 0);

            GPPGParser parser = new GPPGParser(scanner);

            if (!parser.Parse())
                if (Errors.Count == 0)
                    PT.AddError("Не разобрали выражение", null);

            return parser.root;
        }
    }
}
