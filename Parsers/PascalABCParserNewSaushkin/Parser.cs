// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.IO;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using Languages.Pascal.Frontend.Core;

namespace Languages.Pascal.Frontend.Wrapping
{
    // SSM: класс, являющийся обёрткой над GPPG парсером
    /*public class GPPGParserHelper
    {
        private List<Errors.Error> Errs;
        private List<Errors.CompilerWarning> Warnings;
        private string FileName;
        public bool build_tree_for_formatter = false;
        public List<string> DefinesList = null;
        public List<compiler_directive> compilerDirectives;

        public GPPGParserHelper(List<Errors.Error> Errs, List<Errors.CompilerWarning> Warnings, string FileName)
        {
            this.Errs = Errs;
            this.Warnings = Warnings;
            this.FileName = FileName;
        }

        public syntax_tree_node Parse(string Text, List<compiler_directive> compilerDirectives = null)
        {
#if DEBUG
#if _ERR
            FileInfo f = new FileInfo(FileName);
            var sv = Path.ChangeExtension(FileName,".grmtrack1");
            var sw = new StreamWriter(sv);
            Console.SetError(sw);
#endif
#endif
            this.compilerDirectives = compilerDirectives;

            PT parsertools = new PT(); // контекст сканера и парсера
            parsertools.errors = Errs;
            parsertools.warnings = Warnings;
            parsertools.compilerDirectives = compilerDirectives;
            parsertools.CurrentFileName = Path.GetFullPath(FileName);


            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            scanner.parsertools = parsertools;// передали parsertools в объект сканера
            if (DefinesList != null)
                scanner.Defines.AddRange(DefinesList);
            GPPGParser parser = new GPPGParser(scanner);
            parsertools.build_tree_for_formatter = build_tree_for_formatter;
            parser.parsertools = parsertools; // передали parsertools в объект парсера
            
            if (!parser.Parse())
                if (Errs.Count == 0)
                    parsertools.AddError("Неопознанная синтаксическая ошибка!", null);
#if DEBUG
#if _ERR
            sw.Close();
#endif
#endif
            return parser.root;
        }
    }*/

    /// <summary>
    /// Основной парсер языка PascalABC.NET. Реализован Саушкиным Романом.
    /// </summary>
    public class PascalABCNewLanguageParser : BaseParser
    {
        
        public override void Reset()
        {
            CompilerDirectives = new List<compiler_directive>();
            Errors.Clear();
        }

        protected override void PreBuildTree(string FileName)
        {
            CompilerDirectives = new List<compiler_directive>();
        }

        private syntax_tree_node Parse(string Text, string fileName, bool buildTreeForFormatter = false, List<string> definesList = null)
        {
#if DEBUG
#if _ERR
            FileInfo f = new FileInfo(FileName);
            var sv = Path.ChangeExtension(FileName,".grmtrack1");
            var sw = new StreamWriter(sv);
            Console.SetError(sw);
#endif
#endif

            PascalParserTools parserTools = new PascalParserTools(Errors, Warnings, LanguageInformation.ValidDirectives, 
                buildTreeForFormatter, false,
                Path.GetFullPath(fileName), CompilerDirectives); // контекст сканера и парсера

            Scanner scanner = new Scanner(Text, parserTools, LanguageInformation.KeywordsStorage, definesList);

            GPPGParser parser = new GPPGParser(scanner, parserTools);

            if (!parser.Parse())
                if (Errors.Count == 0)
                    parserTools.AddError("Неопознанная синтаксическая ошибка!", null);
#if DEBUG
#if _ERR
            sw.Close();
#endif
#endif
            return parser.root;
        }

        protected override syntax_tree_node BuildTreeInNormalMode(string FileName, string Text, bool compilingNotMainProgram, List<string> DefinesList = null)
        {
            Errors.Clear();
            Warnings.Clear();
            syntax_tree_node root = Parse(Text, FileName, false, DefinesList);

            if (Errors.Count > 0)
                return null;

            return root;
        }

        protected override syntax_tree_node BuildTreeInExprMode(string FileName, string Text)
        {
            if (Text == string.Empty)
                return null;
            // LineCorrection = -1 не забыть
            string origText = Text;
            Text = String.Concat("<<expression>>", Environment.NewLine, Text);

            syntax_tree_node root = Parse(Text, FileName);

            // убрали эту вторую попытку пока, т.к. Intellisense не должен выдавать подсказку в случае, когда компилятор выдает ошибку в такой ситуации  EVA 10.10.2024
            /*if (root == null && origText != null && origText.Contains("<"))
            {
                Errors.Clear();
                root = Parse(String.Concat("<<expression>>", Environment.NewLine, origText.Replace("<", "&<")), FileName);
            }*/
            return root as expression;
        }

        protected override syntax_tree_node BuildTreeInTypeExprMode(string FileName, string Text)
        {
            // LineCorrection = -1 не забыть
            Text = String.Concat("<<type>>", Environment.NewLine, Text);

            syntax_tree_node root = Parse(Text, FileName);
            return root as expression;
        }

        protected override syntax_tree_node BuildTreeInStatementMode(string FileName, string Text)
        {
            Text = String.Concat("<<statement>>", Environment.NewLine, Text);

            syntax_tree_node root = Parse(Text, FileName);
            return root as statement;
        }

        protected override syntax_tree_node BuildTreeInSpecialMode(string FileName, string Text, bool compilingNotMainProgram)
        {
            Errors.Clear();
            syntax_tree_node root = Parse(Text, FileName);
            return root;
        }

        protected override syntax_tree_node BuildTreeInFormatterMode(string FileName, string Text)
        {
            Errors.Clear();
            syntax_tree_node root = Parse(Text, FileName, true);
            return root;
        }

    }
}
