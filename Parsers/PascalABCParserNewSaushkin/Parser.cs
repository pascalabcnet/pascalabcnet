// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.IO;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCSavParser;
using PascalABCCompiler.Parsers;
using GPPGParserScanner;

namespace PascalABCCompiler.PascalABCNewParser
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

    // SSM: Наш основной парсер (реализует НАШ интерфейс IParser)
    public class PascalABCNewLanguageParser : BaseParser, IParser
    {
        //private GPPGParserHelper localparserhelper;

        public PascalABCNewLanguageParser()
            : base("PascalABC.NET", "1.2", "Copyright © 2005-2024 by Ivan Bondarev, Stanislav Mikhalkovich", new string[] { "PABCSystem", "PABCExtensions" }, false, new string[] { ".pas" })
        {
        }

        public override void Reset()
        {
            CompilerDirectives = new List<compiler_directive>();
            Errors.Clear();
        }

        public override SourceFilesProviderDelegate SourceFilesProvider
        {
            get
            {
                return sourceFilesProvider;
            }
            set
            {
                sourceFilesProvider = value;
            }
        }

        public override void PreBuildTree(string FileName)
        {
            CompilerDirectives = new List<compiler_directive>();
        }

        public override syntax_tree_node BuildTree(string FileName, string Text, ParseMode ParseMode, List<string> DefinesList = null)
        {
            syntax_tree_node root = null;

            PreBuildTree(FileName);
            switch (ParseMode)
            {
                case ParseMode.Normal:
                    root = BuildTreeInNormalMode(FileName, Text, DefinesList);
                    break;
                case ParseMode.Expression:
                    root = BuildTreeInExprMode(FileName, Text);
                    break;
                case ParseMode.TypeAsExpression:
                    root = BuildTreeInTypeExprMode(FileName, Text);
                    break;
                case ParseMode.Special:
                    root = BuildTreeInSpecialMode(FileName, Text);
                    break;
                case ParseMode.ForFormatter:
                    root = BuildTreeInFormatterMode(FileName, Text);
                    break;
                case ParseMode.Statement:
                    root = BuildTreeInStatementMode(FileName, Text);
                    break;
                default:
                    break;
            }

            if (root != null && root is compilation_unit compilationUnit)
            {
                compilationUnit.file_name = FileName;
                compilationUnit.compiler_directives = CompilerDirectives;

                if (root is unit_module unitModule)
                    if (unitModule.unit_name.HeaderKeyword == UnitHeaderKeyword.Library)
                        unitModule.compiler_directives.Add(new compiler_directive(new token_info("apptype"), new token_info("dll")));
            }

            return root;
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

            PT parsertools = new PT(); // контекст сканера и парсера
            parsertools.errors = Errors;
            parsertools.warnings = Warnings;
            parsertools.compilerDirectives = CompilerDirectives;
            parsertools.CurrentFileName = Path.GetFullPath(fileName);


            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            scanner.parsertools = parsertools; // передали parsertools в объект сканера
            if (definesList != null)
                scanner.Defines.AddRange(definesList);

            GPPGParser parser = new GPPGParser(scanner);
            parsertools.build_tree_for_formatter = buildTreeForFormatter;
            parser.parsertools = parsertools; // передали parsertools в объект парсера

            if (!parser.Parse())
                if (Errors.Count == 0)
                    parsertools.AddError("Неопознанная синтаксическая ошибка!", null);
#if DEBUG
#if _ERR
            sw.Close();
#endif
#endif
            return parser.root;
        }

        public override syntax_tree_node BuildTreeInNormalMode(string FileName, string Text, List<string> DefinesList = null)
        {
            Errors.Clear();
            Warnings.Clear();
            syntax_tree_node root = Parse(Text, FileName, false, DefinesList);

            if (Errors.Count > 0)
                return null;

            return root;
        }

        public override syntax_tree_node BuildTreeInExprMode(string FileName, string Text)
        {
            if (Text == string.Empty)
                return null;
            // LineCorrection = -1 не забыть
            string origText = Text;
            Text = String.Concat("<<expression>>", Environment.NewLine, Text);

            syntax_tree_node root = Parse(Text, FileName);

            if (root == null && origText != null && origText.Contains("<"))
            {
                Errors.Clear();
                root = Parse(String.Concat("<<expression>>", Environment.NewLine, origText.Replace("<", "&<")), FileName);
            }
            return root as expression;
        }

        public override syntax_tree_node BuildTreeInTypeExprMode(string FileName, string Text)
        {
            // LineCorrection = -1 не забыть
            Text = String.Concat("<<type>>", Environment.NewLine, Text);
            // localparser.parsertools.LineCorrection = -1;

            syntax_tree_node root = Parse(Text, FileName);
            return root as expression;
        }

        public override syntax_tree_node BuildTreeInStatementMode(string FileName, string Text)
        {
            Text = String.Concat("<<statement>>", Environment.NewLine, Text);
            // localparser.parsertools.LineCorrection = -1;

            syntax_tree_node root = Parse(Text, FileName);
            return root as statement;
        }

        public override syntax_tree_node BuildTreeInSpecialMode(string FileName, string Text)
        {
            Errors.Clear();
            syntax_tree_node root = Parse(Text, FileName);
            return root;
        }

        public override syntax_tree_node BuildTreeInFormatterMode(string FileName, string Text)
        {
            Errors.Clear();
            syntax_tree_node root = Parse(Text, FileName);
            return root;
        }

    }
}
