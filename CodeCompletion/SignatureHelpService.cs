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
        public static int FindParameterListStart(string signature)
        {
            int open = signature.IndexOf('(');
            if (open != 0)
                return open;

            // The signature of an extension starts with a parenthesized receiver description.
            int depth = 1;
            for (int i = 1; i < signature.Length; i++)
            {
                if (signature[i] == '(')
                    depth++;
                else if (signature[i] == ')' && --depth == 0)
                    return signature.IndexOf('(', i + 1);
            }
            return -1;
        }

        /// <summary>
        /// Calculates signature help using the legacy editor contract:
        /// <paramref name="caretOffset"/> points to the trigger character, while line and
        /// column are zero-based caret coordinates after that character.
        /// </summary>
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
            if (language == null)
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
                if (domConverter != null)
                    signatures = domConverter.GetNameOfMethod(expression, expressionText, line, column,
                        currentParameter, ref defaultIndex, currentParameterForSelection, out parameterCount);

                if ((signatures == null || signatures.Length == 0) && triggerCharacter == '(' &&
                    ReferenceEquals(language, LanguageProvider.Instance.MainLanguage) &&
                    expression is PascalABCCompiler.SyntaxTree.dot_node)
                {
                    var recovered = CodeCompletionController.CompileCallPrefix(domConverter, fileName, textBeforeCaret, caretOffset);
                    if (recovered != null)
                        signatures = recovered.GetNameOfMethod(expression, expressionText, line, column,
                            currentParameter, ref defaultIndex, currentParameterForSelection, out parameterCount);
                }
            }
            else if (language.LanguageIntellisenseSupport.IsOpenBracketForIndex(triggerCharacter))
            {
                if (domConverter != null)
                    signatures = domConverter.GetIndex(expression, line, column);
            }

            return new SignatureHelpResult(
                signatures, defaultIndex, currentParameter, parameterCount);
        }

        /// <summary>
        /// Calculates signature help using a zero-based caret offset after the trigger.
        /// This is the preferred overload for headless and LSP-style clients.
        /// </summary>
        public static SignatureHelpResult GetSignatureHelpAtCaret(
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
            if (textBeforeCaret == null)
                throw new ArgumentNullException(nameof(textBeforeCaret));
            if (caretOffset <= 0 || caretOffset > textBeforeCaret.Length)
                throw new ArgumentOutOfRangeException(nameof(caretOffset));
            if (textBeforeCaret[caretOffset - 1] != triggerCharacter)
                throw new ArgumentException(
                    "The character immediately before the caret must match the trigger character.",
                    nameof(triggerCharacter));

            return GetSignatureHelp(
                language,
                domConverter,
                fileName,
                textBeforeCaret,
                caretOffset - 1,
                line,
                column,
                triggerCharacter,
                currentParameter,
                currentParameterForSelection);
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
