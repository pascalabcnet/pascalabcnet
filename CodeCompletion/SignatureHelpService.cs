// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.IO;
using Languages.Facade;

namespace CodeCompletion
{
    /// <summary>
    /// Editor-neutral signature help result.
    /// </summary>
    public sealed class SignatureHelpResult
    {
        internal SignatureHelpResult(
            string[] signatures,
            int defaultIndex,
            int currentParameter,
            int parameterCount)
        {
            Signatures = signatures;
            DefaultIndex = defaultIndex;
            CurrentParameter = currentParameter;
            ParameterCount = parameterCount;
        }

        public string[] Signatures { get; private set; }
        public int DefaultIndex { get; private set; }
        public int CurrentParameter { get; private set; }
        public int ParameterCount { get; private set; }
    }

    /// <summary>
    /// Editor-neutral signature help calculation and lifetime checks.
    /// </summary>
    public static class SignatureHelpService
    {
        public static SignatureHelpResult GetSignatureHelp(
            ILanguage language,
            DomConverter domConverter,
            string fileName,
            string textBeforeCaret,
            int caretOffset,
            int line,
            int column,
            char triggerCharacter,
            int currentParameter,
            int currentParameterForSelection)
        {
            if (language == null || domConverter == null)
                return null;

            var expressionText = language.LanguageIntellisenseSupport.FindExpressionForMethod(
                caretOffset,
                textBeforeCaret,
                line,
                column,
                triggerCharacter,
                ref currentParameter);

            var errors = new List<PascalABCCompiler.Errors.Error>();
            var warnings = new List<PascalABCCompiler.Errors.CompilerWarning>();
            var expression = language.Parser.GetExpression(
                "test" + Path.GetExtension(fileName), expressionText, errors, warnings);

            if (expression == null || errors.Count > 0)
                return null;

            string[] signatures = null;
            var defaultIndex = 0;
            var parameterCount = 0;

            if (language.LanguageIntellisenseSupport.IsOpenBracketForMethodCall(triggerCharacter) ||
                language.LanguageIntellisenseSupport.IsMethodCallParameterSeparator(triggerCharacter))
            {
                signatures = domConverter.GetNameOfMethod(
                    expression,
                    expressionText,
                    line,
                    column,
                    currentParameter,
                    ref defaultIndex,
                    currentParameterForSelection,
                    out parameterCount);
            }
            else if (language.LanguageIntellisenseSupport.IsOpenBracketForIndex(triggerCharacter))
            {
                signatures = domConverter.GetIndex(expression, line, column);
            }

            return new SignatureHelpResult(
                signatures, defaultIndex, currentParameter, parameterCount);
        }

        public static bool ShouldClose(string text, int initialOffset, int caretOffset)
        {
            if (caretOffset <= initialOffset)
                return true;

            var brackets = 0;
            var insideString = false;
            var endOffset = Math.Min(caretOffset, text.Length);

            for (var offset = initialOffset; offset < endOffset; offset++)
            {
                var character = text[offset];
                switch (character)
                {
                    case '\'':
                        insideString = !insideString;
                        break;
                    case '[':
                    case '(':
                        if (!insideString)
                            brackets++;
                        break;
                    case ']':
                    case ')':
                        if (!insideString)
                            brackets--;
                        if (brackets <= 0)
                            return true;
                        break;
                    case ';':
                        if (!insideString)
                            return true;
                        break;
                }
            }

            return false;
        }
    }
}
