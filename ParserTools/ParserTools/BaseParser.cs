// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;
using System;

namespace PascalABCCompiler.Parsers
{
    public abstract class BaseParser : IParser
    {
        public List<Error> Errors { get; protected set; } = new List<Error>();

        public List<CompilerWarning> Warnings { get; protected set; } = new List<CompilerWarning>();

        public List<compiler_directive> CompilerDirectives { get; protected set; } = new List<compiler_directive>();

        public Func<bool> CheckIfParsingUnit { get; set; }

        public Dictionary<string, ParserTools.Directives.DirectiveInfo> ValidDirectives { get; protected set; }

        private ILanguageInformation languageInformation;
        public virtual ILanguageInformation LanguageInformation
        {
            get
            {
                if (languageInformation == null)
                    languageInformation = new DefaultLanguageInformation(this);
                return languageInformation;
            }
        }

        protected BaseKeywords Keywords { get; set; }

        /// <summary>
        /// Возвращеает синтаксическое дерево модуля
        /// </summary>
        public SyntaxTree.compilation_unit GetCompilationUnit(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings, ParseMode parseMode, List<string> DefinesList = null)
        {
            return GetSyntaxTreeChecked<SyntaxTree.compilation_unit>(FileName, Text, Errors, Warnings, parseMode, DefinesList);
        }

        public SyntaxTree.compilation_unit GetCompilationUnitForFormatter(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings)
        {
            return GetSyntaxTreeChecked<SyntaxTree.compilation_unit>(FileName, Text, Errors, Warnings, ParseMode.ForFormatter);
        }

        public SyntaxTree.expression GetExpression(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings)
        {
            try // SSM 06.09.18
            {
                return GetSyntaxTreeChecked<SyntaxTree.expression>(FileName, Text, Errors, Warnings, ParseMode.Expression);
            }
            catch
            {
                return null;
            }
        }

        public SyntaxTree.expression GetTypeAsExpression(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings)
        {
            return GetSyntaxTreeChecked<SyntaxTree.expression>(FileName, Text, Errors, Warnings, ParseMode.TypeAsExpression);
        }

        public SyntaxTree.statement GetStatement(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings)
        {
            return GetSyntaxTreeChecked<SyntaxTree.statement>(FileName, Text, Errors, Warnings, ParseMode.Statement);
        }

        /// <summary>
        /// Обобщенная функция для получения различных синтаксических узлов c проверкой, что узел того типа, который ожидался
        /// </summary>
        private T GetSyntaxTreeChecked<T>(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings, ParseMode parseMode, List<string> DefinesList = null) where T : SyntaxTree.syntax_tree_node
        {
            syntax_tree_node unitNode = BuildTree(FileName, Text, Errors, Warnings, parseMode, DefinesList);

            if (unitNode == null)
                return null;

            if (unitNode is T)
                return unitNode as T;

            Errors.Add(new UnexpectedNodeType(FileName, unitNode.source_context, null));

            return null;
        }

        private void InitializeBeforeParsing(List<Error> Errors, List<CompilerWarning> Warnings)
        {
            this.Errors = Errors;
            this.Warnings = Warnings;
        }

        protected virtual syntax_tree_node BuildTree(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings, ParseMode ParseMode, List<string> DefinesList = null)
        {
            InitializeBeforeParsing(Errors, Warnings);
            
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

        protected virtual void PreBuildTree(string FileName) { }

        protected abstract syntax_tree_node BuildTreeInNormalMode(string FileName, string Text, List<string> DefinesList = null);

        protected abstract syntax_tree_node BuildTreeInTypeExprMode(string FileName, string Text);

        protected abstract syntax_tree_node BuildTreeInExprMode(string FileName, string Text);

        protected abstract syntax_tree_node BuildTreeInSpecialMode(string FileName, string Text);

        protected abstract syntax_tree_node BuildTreeInFormatterMode(string FileName, string Text);

        protected abstract syntax_tree_node BuildTreeInStatementMode(string FileName, string Text);

        public virtual void Reset()
        {
            // если нужно - переопределяйте
        }        

    }
}
