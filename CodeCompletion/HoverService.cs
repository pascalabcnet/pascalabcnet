// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.IO;
using Languages.Facade;

namespace CodeCompletion
{
    /// <summary>
    /// Editor-neutral hover description calculation.
    /// </summary>
    public static class HoverService
    {
        public static string GetDescription(
            ILanguage language,
            DomConverter domConverter,
            string fileName,
            string text,
            int offset,
            int line,
            int column)
        {
            if (language == null || domConverter == null)
                return null;

            PascalABCCompiler.Parsers.KeywordKind keyword;
            string expressionWithoutBrackets;
            var expressionText = language.LanguageIntellisenseSupport.FindExpressionFromAnyPosition(
                offset, text, line, column, out keyword, out expressionWithoutBrackets);

            if (expressionText == null)
                expressionText = expressionWithoutBrackets;

            if (expressionText == null)
                return null;

            var errors = new List<PascalABCCompiler.Errors.Error>();
            var warnings = new List<PascalABCCompiler.Errors.CompilerWarning>();
            var expression = language.Parser.GetExpression(
                "test" + Path.GetExtension(fileName), expressionText, errors, warnings);

            // Pascal permits a dereference marker before the expression.
            if (errors.Count > 0 && ReferenceEquals(language, LanguageProvider.Instance.MainLanguage))
            {
                var textWithoutLeadingSpaces = expressionText.TrimStart();
                if (textWithoutLeadingSpaces.Length > 0 && textWithoutLeadingSpaces[0] == '^')
                {
                    errors.Clear();
                    expressionWithoutBrackets = expressionWithoutBrackets.TrimStart().Substring(1);
                    expression = language.Parser.GetExpression(
                        "test" + Path.GetExtension(fileName),
                        textWithoutLeadingSpaces.Substring(1),
                        errors,
                        warnings);
                }
            }

            var errorsWithoutBrackets = new List<PascalABCCompiler.Errors.Error>();
            var expressionWithoutBracketsTree = language.Parser.GetExpression(
                "test" + Path.GetExtension(fileName),
                expressionWithoutBrackets,
                errorsWithoutBrackets,
                warnings);

            if (expressionWithoutBracketsTree == null || errorsWithoutBrackets.Count > 0)
                return null;

            var useHeader = false;
            if (expression == null || errors.Count > 0)
            {
                useHeader = true;
                expression = expressionWithoutBracketsTree;
            }

            return domConverter.GetDescription(
                expression, fileName, expressionWithoutBrackets, line, column, keyword, useHeader);
        }
    }
}
