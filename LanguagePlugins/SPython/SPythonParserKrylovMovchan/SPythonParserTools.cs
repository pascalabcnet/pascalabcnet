using System;
using System.Collections.Generic;
using System.Linq;

using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Parsers;
using System.Text;
using PascalABCCompiler.Errors;
using PascalABCCompiler.ParserTools.Directives;

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
            //TokenNum["tkStringLiteral"] = StringResources.Get("TKSTRINGLITERAL");
            TokenNum["ASSIGN"] = "=";
            TokenNum["COLON"] = "':'";
            TokenNum["DOT"] = "'.'";
            TokenNum["LPAR"] = "'('";
            TokenNum["RPAR"] = "')'";
            TokenNum[";"] = StringResources.Get("EOL");
            TokenNum["SEMICOLON"] = "':'";
            TokenNum["#{"] = StringResources.Get("#{");
            TokenNum["INDENT"] = StringResources.Get("#{");
            TokenNum["#}"] = StringResources.Get("#}");
            TokenNum["UNINDENT"] = StringResources.Get("#}");
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
                case "SEMICOLON":
                    return 120;
                case "STATEMENT":
                    return 100;
                case "EXPRESSION":
                    return 100;
                case "EQUAL":
                    return 90;
                case "COLON":
                    return 80;
                case "ASSIGN":
                    return 70;
                case "ID":
                    return 60;
                case "LPAR":
                    return 100;
                case "RPAR":
                    return 100;
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

        public string CreateErrorString(string yytext, params object[] args)
        {
            string prefix = "";
            if (yytext != "")
                prefix = StringResources.Get("FOUND{0}");
            else
                prefix = StringResources.Get("FOUNDEOF");

            /*if (this.build_tree_for_format_strings && prefix == StringResources.Get("FOUNDEOF"))
            {
                yytext = "}";
                prefix = StringResources.Get("FOUND{0}");
            }*/

            // Преобразовали в список строк - хорошо
            List<string> tokens = new List<string>(args.Skip(1).Cast<string>());

            // Исключаем, т.к. в реальных программах никогда не встретятся
            //tokens = tokens.Except((new string[] { "tkParseModeExpression", "tkParseModeStatement", "tkDirectiveName" })).ToList();

            // Это - временное решение, пока эти слова относятся к идентификаторам (что неправильно)
            //if (tokens.Contains("ID"))
            //    tokens = tokens.Except((new string[] { "tkAbstract", "tkOverload", "tkReintroduce", "tkOverride", "tkVirtual", "tkAt", "tkOn", "tkName", "tkForward", "tkRead", "tkWrite" })).ToList();

            // Добавляем фиктивный токен, что означает, что далее могут идти несколько токенов, начинающих выражение
            if (tokens.Contains("FOR") && tokens.Contains("IF") && tokens.Contains("WHILE") && tokens.Contains("DEF"))
            {
                tokens.Clear();
                tokens.Add("STATEMENT");
            }

            // Добавляем фиктивный токен, что означает, что далее могут идти несколько токенов, начинающих выражение
            if (tokens.Contains("ID") && tokens.Contains("INTNUM") && tokens.Contains("REALNUM") && tokens.Contains("STRINGNUM"))
            {
                tokens.Clear();
                tokens.Add("EXPRESSION");
            }

            tokens = tokens.OrderByDescending(s => TokenPriority(s)).ToList();

            /*if (args.Contains("EOF") && yytext!="")
                return "Текст за концом программы недопустим";
             if (args.Contains("tkIdentifier"))
                 return string.Format(prefix + "ожидался идентификатор", "'" + yytext + "'");*/
            /*if (tokens.Contains("tkProgram"))
                return string.Format(prefix + StringResources.Get("EXPECTEDBEGIN"), "'" + yytext + "'");
            */
            var MaxTok = tokens.First();
            //if (yytext != null && yytext.ToLower() == "record" && MaxTok == "tkSealed")
            //    return StringResources.Get("WRONG_ATTRIBUTE_FOR_RECORD");

            var ExpectedString = StringResources.Get("EXPECTED{1}");

            if (MaxTok.Equals("STATEMENT") || MaxTok.Equals("ID") || MaxTok.Equals("INDENT") || MaxTok.Equals("UNINDENT"))
                ExpectedString = StringResources.Get("EXPECTEDR{1}");
            else if (MaxTok.Equals("STRINGNUM"))
                ExpectedString = StringResources.Get("EXPECTEDF{1}");
            if ((MaxTok == "EOF" || MaxTok == "EOF1" || MaxTok == "FOUNDEOF") && this.build_tree_for_format_strings)
                MaxTok = "}";
            var MaxTokHuman = ConvertToHumanName(MaxTok);

            if (yytext == ";" || yytext == "#{" || yytext == "#}")
            {
                prefix = StringResources.Get("FOUNDM{0}");
                yytext = ConvertToHumanName(yytext);
                return string.Format(prefix + ExpectedString, yytext, MaxTokHuman);
            }

            // string w = string.Join(" или ", tokens.Select(s => ConvertToHumanName((string)s)));
       
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
