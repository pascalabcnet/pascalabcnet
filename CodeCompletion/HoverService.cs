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
            if (language == null)
                return null;

            // Do not describe a routine or value as an expression when the parser
            // found its name in a type reference that did not resolve to a type.
            if (domConverter != null && domConverter.visitor.IsNonTypeReference(text, offset, line, column))
                return null;

            PascalABCCompiler.Contracts.Parsers.KeywordKind keyword;
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
            {
                // A qualified implementation header such as T<U>.P is not a Pascal expression.
                // Use the resolved method only when the cursor is on its name in that header.
                if (domConverter != null && domConverter.visitor.FindScopeByLocation(line + 1, column + 1) is ProcRealization method &&
                    method.IsInScope(method.methodNameLocation, line + 1, column + 1))
                {
                    string methodName = method.def_proc.si.name;
                    return domConverter.GetDescription(new PascalABCCompiler.SyntaxTree.ident(methodName),
                        fileName, methodName, line, column,
                        PascalABCCompiler.Contracts.Parsers.KeywordKind.Function, true);
                }

                return null;
            }

            var useHeader = false;
            if (expression == null || errors.Count > 0)
            {
                useHeader = true;
                expression = expressionWithoutBracketsTree;
            }

            string description = domConverter == null ? null : domConverter.GetDescription(
                expression, fileName, expressionWithoutBrackets, line, column, keyword, useHeader);
            if (!string.IsNullOrEmpty(description) ||
                !ReferenceEquals(language, LanguageProvider.Instance.MainLanguage) ||
                !(expressionWithoutBracketsTree is PascalABCCompiler.SyntaxTree.dot_node))
                return description;

            int calleeEnd = offset;
            while (calleeEnd < text.Length && (char.IsLetterOrDigit(text[calleeEnd]) || text[calleeEnd] == '_'))
                calleeEnd++;
            int next = calleeEnd;
            while (next < text.Length && char.IsWhiteSpace(text[next]))
                next++;
            if (next == text.Length || text[next] != '(')
                return description;

            var recovered = CodeCompletionController.CompileCallPrefix(domConverter, fileName, text, calleeEnd);
            return recovered == null ? description : recovered.GetDescription(
                expressionWithoutBracketsTree, fileName, expressionWithoutBrackets, line, column, keyword, true);
        }
    }
}
