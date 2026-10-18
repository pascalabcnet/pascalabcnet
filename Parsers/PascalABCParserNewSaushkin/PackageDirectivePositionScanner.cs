#if PABCNET_MODERN
using System;
using PascalABCCompiler.Errors;
using PascalABCCompiler.ParserTools;
using QUT.Gppg;

namespace Languages.Pascal.Frontend.Core
{
    /// <summary>
    /// Checks package placement using real lexer output, without scanning text or
    /// changing the generated scanner and the recursive unit compilation algorithm.
    /// </summary>
    internal sealed class PackageDirectivePositionScanner : AbstractScanner<Union, LexLocation>
    {
        private readonly Scanner scanner;
        private readonly PascalParserTools parserTools;
        private int inspectedDirectives;
        private bool hasLanguageCode;

        internal Scanner InnerScanner { get { return scanner; } }

        internal PackageDirectivePositionScanner(Scanner scanner, PascalParserTools parserTools)
        {
            this.scanner = scanner;
            this.parserTools = parserTools;
        }

        public override LexLocation yylloc
        {
            get { return scanner.yylloc; }
            set { scanner.yylloc = value; }
        }

        public override int yylex()
        {
            int token = scanner.yylex();
            var directives = parserTools.compilerDirectives;
            while (inspectedDirectives < directives.Count)
            {
                var directive = directives[inspectedDirectives++];
                if (hasLanguageCode && string.Equals(directive.Name.text, "package", StringComparison.OrdinalIgnoreCase))
                {
                    // TEST-NET10-PACKAGE-PLACEMENT: inactive branches and strings are
                    // already excluded by the real lexer; preserve include locations.
                    parserTools.errors.Add(new SyntaxError(
                        Languages.Pascal.Frontend.StringResources.Get("PACKAGE_DIRECTIVE_MUST_PRECEDE_CODE"),
                        directive.source_context?.FileName ?? parserTools.currentFileName,
                        directive.source_context, null));
                }
            }
            if (token != (int)Tokens.EOF)
                hasLanguageCode = true;
            yylval = scanner.yylval;
            return token;
        }

        public override void yyerror(string format, params object[] args)
        {
            scanner.yyerror(format, args);
        }
    }
}
#endif
