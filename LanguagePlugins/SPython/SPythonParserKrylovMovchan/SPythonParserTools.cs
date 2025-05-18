using System;
using System.Collections.Generic;
using System.Linq;

using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Parsers;
using System.Text;
using PascalABCCompiler.Errors;
using PascalABCCompiler.ParserTools.Directives;
using QUT.Gppg;
using System.Text.RegularExpressions;

namespace SPythonParser
{
    public static class StringResources
    {
        private static string prefix = "SPYTHONPARSER_";
        public static string Get(string Id)
        {
            string ret = PascalABCCompiler.StringResources.Get(prefix + Id);
            if (ret == prefix + Id)
                return Id;
            else
                return ret;
        }
    }

    public class SymbolTable
    {
        private SymbolTable outerScope;
        private SortedSet<string> symbols = new SortedSet<string>();
        public SymbolTable(SymbolTable outerScope = null)
        {
            this.outerScope = outerScope;
        }

        public bool Contains(string ident)
        {
            SymbolTable curr = this;

            while (curr != null)
            {
                if (curr.symbols.Contains(ident))
                    return true;
                curr = curr.outerScope;
            }

            return false;
        }

        public void Add(string ident)
        {
            symbols.Add(ident);
        }

        public SymbolTable OuterScope { get { return outerScope; } }
    }
    public class SPythonParserTools : BaseParserTools
    {
        public bool build_tree_for_formatter = false;
        public bool build_tree_for_format_strings = false;

        public override Dictionary<string, string> TokenNum { get; protected set; }
        
        private void InitializeTokenNum()
        {
            TokenNum = new Dictionary<string, string>();
            TokenNum["STATEMENT"] = StringResources.Get("STMT");
            TokenNum["EXPRESSION"] = StringResources.Get("EXPR");
            TokenNum["EOF"] = StringResources.Get("EOF1");
            TokenNum["ID"] = StringResources.Get("IDENTIFIER");
            TokenNum["INTNUM"] = StringResources.Get("INTNUM");
            TokenNum["REALNUM"] = StringResources.Get("REALNUM");
            TokenNum["STRINGNUM"] = StringResources.Get("STRINGNUM");
            TokenNum["END_OF_FILE"] = StringResources.Get("#$");
            TokenNum["#$"] = StringResources.Get("#$");
            TokenNum["INDENT"] = StringResources.Get("#{");
            TokenNum["#{"] = StringResources.Get("#{");
            TokenNum["UNINDENT"] = StringResources.Get("#}");
            TokenNum["#}"] = StringResources.Get("#}");
            TokenNum["END_OF_LINE"] = StringResources.Get("#;");
            TokenNum["#;"] = StringResources.Get("#;");
            //TokenNum["tkStringLiteral"] = StringResources.Get("TKSTRINGLITERAL");
            TokenNum["IN"] = "'in'";
            TokenNum["IMPORT"] = "'import'";
            TokenNum["SEMICOLON"] = "';'";
            TokenNum["ASSIGN"] = "'='";
            TokenNum["COLON"] = "':'";
            TokenNum["DOT"] = "'.'";
            TokenNum["LPAR"] = "'('";
            TokenNum["RPAR"] = "')'";
            TokenNum["LBRACKET"] = "'['";
            TokenNum["RBRACKET"] = "']'";
            TokenNum["tkQuestion"] = "'?'";
            TokenNum["COMMA"] = "','";
            TokenNum["PERCENTAGE"] = "'%'";
            TokenNum["DIVIDE"] = "'/'";
            TokenNum["MINUS"] = "'-'";
            TokenNum["PLUS"] = "'+'";
            TokenNum["SLASHSLASH"] = "'//'";
            TokenNum["STAR"] = "'*'";
            TokenNum["EQUAL"] = "'=='";
            TokenNum["NOTEQUAL"] = "'!='";
            TokenNum["GREATER"] = "'>'";
            TokenNum["GREATEREQUAL"] = "'>='";
            TokenNum["LESS"] = "'<'";
            TokenNum["LESSEQUAL"] = "'<='";
            TokenNum["ARROW"] = "'->'";
        }

        public SPythonParserTools(List<Error> errors, List<CompilerWarning> warnings,
            Dictionary<string, DirectiveInfo> validDirectives, bool buildTreeForFormatter = false, bool buildTreeForFormatterStrings = false,
            string currentFileName = null, List<compiler_directive> compilerDirectives = null) 
            : base(errors, warnings, validDirectives, buildTreeForFormatter, buildTreeForFormatterStrings, currentFileName, compilerDirectives)
        {

            InitializeTokenNum();
        }

        public int TokenPriority(string tok)
        {
            switch (tok)
            {
                case "END_OF_LINE":
                    return 300;
                case "STATEMENT":
                    return 100;
                case "EXPRESSION":
                    return 100;
                case "EQUAL":
                    return 90;
                case "COLON":
                    return 200;
                case "ASSIGN":
                    return 70;
                case "ID":
                    return 60;
                case "LPAR":
                    return 100;
                case "RPAR":
                    return 130;
                case "DOT":
                    return 20;
                case "RBRACKET":
                    return 10;
                case "GREATER":
                    return 5;
            }
            return 0;
        }

        protected override string ReplaceSpecialSymbols(string text)
        {
            StringBuilder new_text = new StringBuilder(text);
            int curr = 0;
            for (int i = 0; i < text.Length; ++i)
            {
                if (text[i] != '\\') new_text[curr++] = text[i];
                else switch (text[i + 1])
                    {
                        case 'a': new_text[curr++] = '\a'; ++i; break;
                        case '0': new_text[curr++] = '\0'; ++i; break;
                        case 'b': new_text[curr++] = '\b'; ++i; break;
                        case 't': new_text[curr++] = '\t'; ++i; break;
                        case 'n': new_text[curr++] = '\n'; ++i; break;
                        case 'v': new_text[curr++] = '\v'; ++i; break;
                        case 'f': new_text[curr++] = '\f'; ++i; break;
                        case 'r': new_text[curr++] = '\r'; ++i; break;

                        case '\'': new_text[curr++] = '\''; ++i; break;
                        case '"': new_text[curr++] = '"'; ++i; break;
                        case '\\': new_text[curr++] = '\\'; ++i; break;

                        default: new_text[curr++] = '\\'; break;
                    }
            }
            return new_text.ToString().Substring(0, curr);
        }

        public override literal create_string_const(string text, SourceContext sc)
        {
            literal lt;
            text = ReplaceSpecialSymbols(text.Substring(1, text.Length - 2));
            lt = new string_const(text);
            lt.source_context = sc;
            return lt;
        }

        static private expression RemoveBracesAndContent(string input, SourceContext sc)
    {
        var regex = new Regex(@"\{([^}]+)\}");
        var parts = new List<expression>();

        int lastIndex = 0;
        foreach (Match m in regex.Matches(input))
        {
            if (m.Index > lastIndex)
            {
                var text = input.Substring(lastIndex, m.Index - lastIndex);
                if (text != "")
                    parts.Add(new string_const(text, sc));
            }

            SourceContext sc1 = new SourceContext(sc.LeftSourceContext, sc.RightSourceContext);
            sc1.begin_position.column_num += m.Index + 3;
            parts.Add(new ident(m.Groups[1].Value, sc1));
            lastIndex = m.Index + m.Length;
        }

        if (lastIndex < input.Length)
        {
            var text = input.Substring(lastIndex);
            if (text != "")
                parts.Add(new string_const(text, sc));
        }

        expression result = parts[parts.Count - 1];
        for (int i = parts.Count - 2; i >= 0; i--)
        {
            result = new bin_expr(parts[i], result, Operators.Plus, sc);
        }

        return result;
    }

        public expression create_fstring(string text, SourceContext sc)
        {
            text = ReplaceSpecialSymbols(text.Substring(2, text.Length - 3));
            expression res = RemoveBracesAndContent(text, sc);
            res.source_context = sc;
            return res;
        }

        public LexLocation GetLexLocation(string found, string expected, LexLocation prev_loc, LexLocation curr_loc)
        {
            // жертвы, чтобы достать имя файла
            SourceContext sc = curr_loc;
            if (found == "#{" && expected == "END_OF_LINE" ||
                found == "#;" && expected == "INDENT")
                return new LexLocation(curr_loc.StartLine + 1, 0, curr_loc.StartLine + 1, 0, sc.FileName);
            if (found == "#{" || found == "#;")
                return new LexLocation(prev_loc.EndLine, prev_loc.EndColumn, prev_loc.EndLine, prev_loc.EndColumn + 1, sc.FileName);
            return curr_loc;
        }

        public string ExpectedToken(bool line_needs_colon, params object[] args) {
            string found = args[0].ToString();
            List<string> tokens = new List<string>(args.Skip(1).Cast<string>());

            if (tokens.Count != 1) {
                tokens = tokens.Except((new string[] { found })).ToList();
            }

            // Это - временное решение, пока эти слова относятся к идентификаторам (что неправильно)
            //if (tokens.Contains("ID"))
            //    tokens = tokens.Except((new string[] { "tkAbstract", "tkOverload", "tkReintroduce", "tkOverride", "tkVirtual", "tkAt", "tkOn", "tkName", "tkForward", "tkRead", "tkWrite" })).ToList();

            if (tokens.Contains("FOR") && tokens.Contains("IF") && tokens.Contains("WHILE") && tokens.Contains("DEF"))
                return "STATEMENT";

            if (tokens.Contains("ID") && tokens.Contains("INTNUM") && tokens.Contains("REALNUM") && tokens.Contains("STRINGNUM"))
                return "EXPRESSION";

            if (line_needs_colon && tokens.Contains("COLON"))
                return "COLON";

            tokens = tokens.OrderByDescending(s => TokenPriority(s)).ToList();
            
            return tokens.First();
        }

        public string CreateErrorString(string yytext, string exp_token) {
            if (yytext.Equals("#;") && exp_token.Equals("INDENT")) {
                return StringResources.Get("LINE_WITHOUT_INDENT");
            }
            
            if (yytext.Equals("#{") && !exp_token.Equals("END_OF_LINE")) {
                yytext = "#;";
            }
            if (exp_token.Equals("INDENT")) {
                exp_token = "END_OF_LINE";
            }

            string prefix = "";
            if (yytext != "")
                prefix = StringResources.Get("FOUND{0}");
            else
                prefix = StringResources.Get("FOUNDEOF");

            var ExpectedString = StringResources.Get("EXPECTED{1}");

            if (exp_token.Equals("STATEMENT") || 
                exp_token.Equals("ID") || 
                exp_token.Equals("INDENT") || 
                exp_token.Equals("UNINDENT") ||
                exp_token.Equals("UNINDENT") ||
                exp_token.Equals("END_OF_LINE") ||
                exp_token.Equals("END_OF_FILE"))
                ExpectedString = StringResources.Get("EXPECTEDR{1}");
            else if (exp_token.Equals("STRINGNUM"))
                ExpectedString = StringResources.Get("EXPECTEDF{1}");
            var MaxTokHuman = ConvertToHumanName(exp_token);

            if (yytext == "#{" || yytext == "#}" || yytext == "#;" || yytext == "#$")
            {
                prefix = StringResources.Get("FOUNDM{0}");
                yytext = ConvertToHumanName(yytext);
                return string.Format(prefix + ExpectedString, yytext, MaxTokHuman);
            }
            
            return string.Format(prefix + ExpectedString, "'" + yytext + "'", MaxTokHuman);
        }



        
        /* public string CreateErrorString(string yytext, LexLocation yyloc, params object[] args)
         {
             string expected = String.Join(", ", args.Skip(1).Select(x => x.ToString()));
             string err = $"PARSER ERROR \"{yytext}\" AT LINE #{yyloc.StartLine}: EXPECTED  {expected}, FOUND {args[0]}";
             return err;
         }*/

        protected override string GetFromStringResources(string id)
        {
            return StringResources.Get(id);
        }

        protected override string ExtractDirectiveTextWithoutSpecialSymbols(string directive)
        {
            throw new NotImplementedException();
        }

        public typecast_node NewAsIsExpr(syntax_tree_node term, op_typecast typecast_op, type_definition simple_or_template_type_reference, LexLocation loc)
        {
            var naie = new typecast_node((addressed_value)term, simple_or_template_type_reference, typecast_op, loc); 
            
            return naie;
        }
    }

    // temp
    public class ParserLambdaHelper
    {
        private int lambda_num = 0;
        public List<function_lambda_definition> lambdaDefinitions;
        public static string lambdaPrefix = "<>lambda";

        public ParserLambdaHelper()
        {
            lambdaDefinitions = new List<function_lambda_definition>();
        }

        public string CreateLambdaName()
        {
            lambda_num++;
            return lambdaPrefix + lambda_num.ToString();
        }

        public bool IsLambdaName(ident id)
        {
            return id.name.StartsWith(lambdaPrefix);
        }
    }
}
