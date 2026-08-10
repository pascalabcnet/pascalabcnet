// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Linq;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;

namespace CodeCompletion
{
    /// <summary>
    /// Symbols selected for an editor-neutral completion request.
    /// </summary>
    public sealed class CompletionSymbolResult
    {
        internal CompletionSymbolResult(
            SymInfo[] symbols,
            SymInfo selectedSymbol,
            SymScope dotScope,
            bool shouldAbortCompletion)
        {
            Symbols = symbols;
            SelectedSymbol = selectedSymbol;
            DotScope = dotScope;
            ShouldAbortCompletion = shouldAbortCompletion;
        }

        public SymInfo[] Symbols { get; private set; }
        public SymInfo SelectedSymbol { get; private set; }
        public SymScope DotScope { get; private set; }
        public bool ShouldAbortCompletion { get; private set; }
    }

    /// <summary>
    /// Editor-neutral symbol selection for completion clients.
    /// </summary>
    public static class CompletionSymbolService
    {
        public static SymInfo[] GetSymbolsByFirstCharacter(
            DomConverter domConverter,
            int line,
            int column,
            bool typeExpected,
            bool namespaceExpected,
            string pattern,
            bool smartIntellisense,
            int namespaceVisibleRange)
        {
            if (domConverter == null)
                return namespaceExpected ? DomConverter.standard_units : null;

            if (typeExpected)
                return domConverter.GetTypeByPattern(
                    pattern, line, column, true, namespaceVisibleRange);

            if (namespaceExpected)
                return smartIntellisense
                    ? domConverter.GetNamespaces()
                    : DomConverter.standard_units;

            return domConverter.GetNameByPattern(
                null, line, column, true, namespaceVisibleRange);
        }

        public static CompletionSymbolResult GetSymbols(
            DomConverter domConverter,
            int line,
            int column,
            CompletionTriggerContext context,
            bool languageCaseSensitive,
            string expressionText,
            bool ctrlOrShiftSpaceAfterDot,
            bool insidePatternWithDots,
            string pattern,
            expression expression,
            PascalABCCompiler.Parsers.KeywordKind keyword,
            bool smartIntellisense,
            int namespaceVisibleRange)
        {
            SymInfo[] symbols = null;
            SymInfo selectedSymbol = null;
            SymScope dotScope = null;
            var shouldAbortCompletion = false;

            if (domConverter == null)
            {
                if (!context.SpaceAfterUses && !context.CtrlSpace)
                    shouldAbortCompletion = true;

                if (context.SpaceAfterUses)
                    symbols = DomConverter.standard_units;

                return new CompletionSymbolResult(
                    symbols, selectedSymbol, dotScope, shouldAbortCompletion);
            }

            if (context.SpaceAfterNew)
            {
                symbols = domConverter.GetTypes(expression, line, column, out selectedSymbol);
            }
            else if (context.SpaceAfterUses)
            {
                symbols = smartIntellisense
                    ? domConverter.GetNamespaces()
                    : DomConverter.standard_units;
            }
            else if (context.DotPressed || ctrlOrShiftSpaceAfterDot)
            {
                symbols = domConverter.GetName(
                    expression, expressionText, line, column, keyword, ref dotScope);
            }
            else if (context.CtrlSpace || context.ShiftSpace)
            {
                if (insidePatternWithDots)
                {
                    symbols = domConverter.GetName(
                            expression, expressionText, line, column, keyword, ref dotScope)
                        .Where(symbol => symbol.name.StartsWith(
                            pattern,
                            languageCaseSensitive
                                ? StringComparison.Ordinal
                                : StringComparison.OrdinalIgnoreCase))
                        .ToArray();
                }
                else
                {
                    symbols = domConverter.GetNameByPattern(
                        pattern,
                        line,
                        column,
                        context.CtrlSpace,
                        namespaceVisibleRange);
                }
            }

            return new CompletionSymbolResult(
                symbols, selectedSymbol, dotScope, shouldAbortCompletion);
        }
    }
}
