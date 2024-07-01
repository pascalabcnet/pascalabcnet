﻿// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.IO;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using PascalABCCompiler;
using Languages.Pascal.Frontend.Core;
using PascalABCCompiler.ParserTools.Directives;
using static PascalABCCompiler.ParserTools.Directives.DirectiveHelper;

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

        public PascalABCNewLanguageParser()
        {
            InitializeValidDirectives();
        }
        
        public override void Reset()
        {
            CompilerDirectives = new List<compiler_directive>();
            Errors.Clear();
        }


        /// <summary>
        /// Здесь записываются все директивы, поддерживаемые языком, а также правила для проверки их параметров (ограничения, накладываемые со стороны языка)
        /// </summary>
        private void InitializeValidDirectives()
        {
            #region VALID DIRECTIVES
            ValidDirectives = new Dictionary<string, DirectiveInfo>(StringComparer.CurrentCultureIgnoreCase)
            {
                [StringConstants.compiler_directive_apptype] = new DirectiveInfo(SingleAnyOfCheck("console", "windows", "dll", "pcu")),
                [StringConstants.compiler_directive_reference] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_include_namespace] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_savepcu] = new DirectiveInfo(SingleAnyOfCheck("true", "false")),
                [StringConstants.compiler_directive_zerobasedstrings] = new DirectiveInfo(SingleAnyOfCheck("on", "off"), paramsNums: new int[2] { 0, 1 }),
                [StringConstants.compiler_directive_zerobasedstrings_ON] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_zerobasedstrings_OFF] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_nullbasedstrings_ON] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_nullbasedstrings_OFF] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_initstring_as_empty_ON] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_initstring_as_empty_OFF] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_resource] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_platformtarget] = new DirectiveInfo(SingleAnyOfCheck("x86", "x64", "anycpu", "dotnet5win", "dotnet5linux", "dotnet5macos", "native")),
                [StringConstants.compiler_directive_faststrings] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_gendoc] = new DirectiveInfo(SingleAnyOfCheck("true", "false")),
                [StringConstants.compiler_directive_region] = new DirectiveInfo(checkParamsNumNeeded: false),
                [StringConstants.compiler_directive_endregion] = new DirectiveInfo(checkParamsNumNeeded: false),
                [StringConstants.compiler_directive_ifdef] = new DirectiveInfo(SingleIsValidIdCheck()),
                [StringConstants.compiler_directive_endif] = new DirectiveInfo(SingleIsValidIdCheck(), paramsNums: new int[2] { 0, 1 }),
                [StringConstants.compiler_directive_ifndef] = new DirectiveInfo(SingleIsValidIdCheck()),
                [StringConstants.compiler_directive_else] = new DirectiveInfo(SingleIsValidIdCheck(), paramsNums: new int[2] { 0, 1 }),
                [StringConstants.compiler_directive_undef] = new DirectiveInfo(SingleIsValidIdCheck()),
                [StringConstants.compiler_directive_define] = new DirectiveInfo(SingleIsValidIdCheck()),
                [StringConstants.compiler_directive_include] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_targetframework] = new DirectiveInfo(),
                [StringConstants.compiler_directive_hidden_idents] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_version_string] = new DirectiveInfo(IsValidVersionCheck()),
                [StringConstants.compiler_directive_product_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_company_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_trademark_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_main_resource_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_title_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_description_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_omp] = new DirectiveInfo(SingleAnyOfCheck("critical", "parallel"), checkParamsNumNeeded: false)
            }; 
            #endregion
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

            PascalParserTools parserTools = new PascalParserTools(this); // контекст сканера и парсера
            parserTools.errors = Errors;
            parserTools.warnings = Warnings;
            parserTools.compilerDirectives = CompilerDirectives;
            parserTools.currentFileName = Path.GetFullPath(fileName);


            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            scanner.parserTools = parserTools; // передали parserTools в объект сканера
            if (definesList != null)
                scanner.Defines.AddRange(definesList);

            GPPGParser parser = new GPPGParser(scanner);
            parserTools.buildTreeForFormatter = buildTreeForFormatter;
            parser.parserTools = parserTools; // передали parserTools в объект парсера

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

        protected override syntax_tree_node BuildTreeInNormalMode(string FileName, string Text, List<string> DefinesList = null)
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

            if (root == null && origText != null && origText.Contains("<"))
            {
                Errors.Clear();
                root = Parse(String.Concat("<<expression>>", Environment.NewLine, origText.Replace("<", "&<")), FileName);
            }
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

        protected override syntax_tree_node BuildTreeInSpecialMode(string FileName, string Text)
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
