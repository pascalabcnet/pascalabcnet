// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System.Collections.Generic;
using System.IO;
using Languages.Facade;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;

namespace CodeCompletion
{
    /// <summary>
    /// Editor-neutral description of the action that requested completion.
    /// </summary>
    public struct CompletionTriggerContext
    {
        public bool DotPressed { get; set; }
        public bool CtrlSpace { get; set; }
        public bool ShiftSpace { get; set; }
        public bool SpaceAfterNew { get; set; }
        public bool SpaceAfterUses { get; set; }
    }

    /// <summary>
    /// Textual expression and pattern found immediately before a completion request.
    /// </summary>
    public sealed class CompletionExpressionInfo
    {
        internal CompletionExpressionInfo()
        {
        }

        public string ExpressionText { get; internal set; }
        public string Pattern { get; internal set; }
        public bool InsidePatternWithDots { get; internal set; }
        public bool CtrlOrShiftSpaceAfterDot { get; internal set; }
        public PascalABCCompiler.Parsers.KeywordKind Keyword { get; internal set; }
    }

    /// <summary>
    /// Result of parsing an expression prepared for completion.
    /// </summary>
    public sealed class CompletionExpressionParseResult
    {
        internal CompletionExpressionParseResult(expression expression, bool shouldAbortCompletion)
        {
            Expression = expression;
            ShouldAbortCompletion = shouldAbortCompletion;
        }

        public expression Expression { get; private set; }
        public bool ShouldAbortCompletion { get; private set; }
    }

    /// <summary>
    /// Editor-neutral preparation and parsing of expressions used by completion clients.
    /// </summary>
    public static class CompletionExpressionService
    {
        public static CompletionExpressionInfo Analyze(
            int offset,
            string text,
            int line,
            int column,
            ILanguageIntellisenseSupport intellisenseSupport,
            CompletionTriggerContext context,
            PascalABCCompiler.Parsers.KeywordKind keyword)
        {
            var result = new CompletionExpressionInfo
            {
                Keyword = keyword
            };

            if (context.CtrlSpace || context.ShiftSpace)
            {
                bool isPattern;
                result.Pattern = intellisenseSupport.FindPattern(offset, text, out isPattern);

                if (!isPattern && text[offset - 1] == '.')
                    result.CtrlOrShiftSpaceAfterDot = true;

                if (isPattern && text[offset - result.Pattern.Length - 1] == '.' || result.CtrlOrShiftSpaceAfterDot)
                {
                    result.InsidePatternWithDots = true;
                    result.ExpressionText = intellisenseSupport.FindExpression(
                        offset - (result.Pattern == null ? 0 : result.Pattern.Length) - 1,
                        text,
                        line,
                        column,
                        out keyword);
                    result.Keyword = keyword;
                }
            }
            else if (context.SpaceAfterNew)
            {
                result.ExpressionText = intellisenseSupport.SkipNew(offset - 1, text, ref keyword);
                result.Keyword = keyword;
            }
            else if (context.DotPressed)
            {
                result.ExpressionText = intellisenseSupport.FindExpression(offset, text, line, column, out keyword);
                result.Keyword = keyword;
            }

            return result;
        }

        public static CompletionExpressionParseResult Parse(
            IParser parser,
            string fileName,
            string expressionText,
            CompletionTriggerContext context)
        {
            var errors = new List<PascalABCCompiler.Errors.Error>();
            var warnings = new List<PascalABCCompiler.Errors.CompilerWarning>();
            var expression = parser.GetTypeAsExpression(
                "test" + Path.GetExtension(fileName), expressionText, errors, warnings);

            if (expression == null)
            {
                errors.Clear();
                expression = parser.GetExpression(
                    "test" + Path.GetExtension(fileName), expressionText, errors, warnings);
            }

            var shouldAbortCompletion =
                (expression == null || errors.Count > 0) && !context.SpaceAfterNew;

            return new CompletionExpressionParseResult(expression, shouldAbortCompletion);
        }
    }
}
