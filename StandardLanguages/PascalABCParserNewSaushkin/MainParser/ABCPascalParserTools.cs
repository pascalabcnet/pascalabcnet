// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using System.Linq;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Parsers;
using PascalABCCompiler;

namespace Languages.Pascal.Frontend.Core
{
    // Класс глобальных описаний и статических методов
    // для использования различными подсистемами парсера и сканера
    public class PascalParserTools : BaseParserTools
    {
        public System.Collections.Stack NodesStack; // SSM: для каких-то вспомогательных целей в двух правилах

        private int lambdaNum = 0;
        public List<function_lambda_definition> pascalABCLambdaDefinitions;
        public List<var_def_statement> pascalABCVarStatements;
        public List<type_declaration> pascalABCTypeDeclarations;

        protected override BaseParser ParserCached 
        { 
            get
            {
                if (parserCached == null)
                    parserCached = Facade.LanguageProvider.Instance.SelectLanguageByName(StringConstants.pascalLanguageName).Parser;
                return parserCached;
            }
        }

        static PascalParserTools()
        {
            tokenNum = new Dictionary<string, string>();

            tokenNum["tkStatement"] = StringResources.Get("OPERATOR");
            tokenNum["tkExpression"] = StringResources.Get("EXPR");
            tokenNum["EOF"] = StringResources.Get("EOF1");
            tokenNum["tkIdentifier"] = StringResources.Get("TKIDENTIFIER");
            tokenNum["tkStringLiteral"] = StringResources.Get("TKSTRINGLITERAL");
            tokenNum["tkAmpersend"] = "'";
            tokenNum["tkColon"]="':'";
            tokenNum["tkDotDot"]="'..'";
            tokenNum["tkPoint"]="'.'";
            tokenNum["tkRoundOpen"]="'('";
            tokenNum["tkRoundClose"]="')'";
            tokenNum["tkSemiColon"]="';'";
            tokenNum["tkSquareOpen"]="'['";
            tokenNum["tkSquareClose"]="']'";
            tokenNum["tkQuestion"]="'?'";
            
            tokenNum["tkComma"]="','";
            tokenNum["tkAssign"]="':='";
            tokenNum["tkPlusEqual"]="'+='";
            tokenNum["tkMinusEqual"]="'-='";
            tokenNum["tkMultEqual"]="'*='";
            tokenNum["tkDivEqual"]="'/='";
            tokenNum["tkMinus"]="'-'";
            tokenNum["tkPlus"]="'+'";
            tokenNum["tkSlash"]="'//'";
            tokenNum["tkStar"]="'*'";
            tokenNum["tkEqual"]="'='";
            tokenNum["tkGreater"]="'>'";
            tokenNum["tkGreaterEqual"]="'>='";
            tokenNum["tkLower"]="'<'";
            tokenNum["tkLowerEqual"]="'<='";
            tokenNum["tkNotEqual"]="'<>'";
            tokenNum["tkArrow"]="'->'";
            tokenNum["tkAddressOf"]="'@'";
            tokenNum["tkDeref"]="'^'";
            tokenNum["tkStarStar"]="'**'";
        }

        public PascalParserTools()
        {
            NodesStack = new System.Collections.Stack();
            pascalABCLambdaDefinitions = new List<function_lambda_definition>();
            pascalABCVarStatements = new List<var_def_statement>();
            pascalABCTypeDeclarations = new List<type_declaration>();
        }

        /// <summary>
        /// Разбор директивы, согласно спецификации языка
        /// </summary>
        public void ParseDirective(string directive, QUT.Gppg.LexLocation location, out string directiveName, out List<string> directiveParams)
        {
            // текст без спецсимволов {$}
            string directiveText = directive.Substring(2, directive.Length - 3);
            directiveName = GetDirectiveName(directiveText);
            directiveParams = new List<string>();

            // пустая директива - ошибка
            if (directiveName == "")
            {
                AddErrorFromResource("EMPTY_DIRECTIVE", location);
                return;
            }

            // проверка имени директивы
            if (!ParserCached.ValidDirectives.ContainsKey(directiveName))
            {
                AddErrorFromResource("UNKNOWN_DIRECTIVE{0}", location, directiveName);
                return;
            }

            // подстрока с параметрами
            string paramsString = directiveText.Substring(directiveText.IndexOf(directiveName) + directiveName.Length);
            
            if (ParserCached.ValidDirectives[directiveName].quotesAreSpecialSymbols)
            {
                directiveParams = SplitDirectiveParamsWithQuotesAsSpecialSymbols(paramsString);
            }
            else
            {
                directiveParams = SplitDirectiveParamsOrdinary(paramsString);
            }

            CheckDirectiveParams(directiveName, directiveParams, location);
        }

        protected override string GetFromStringResources(string res)
        {
            return StringResources.Get(res);
        }

        public int TokenPriority(string tok)
        {
            switch (tok)
            {
                case "tkSemiColon":
                    return 120;
                case "tkStatement":
                    return 100;
                case "tkExpression":
                    return 100;
                case "tkEqual":
                    return 90;
                case "tkColon":
                    return 80;
                case "tkAssign":
                    return 70;
                case "tkIdentifier":
                    return 60;
                case "tkRoundClose":
                    return 50;
                case "tkBegin":
                    return 40;
                case "tkEnd":
                    return 30;
                case "tkDotDot":
                    return 20;
                case "tkSquareClose":
                    return 10;
                case "tkGreater":
                    return 5;
            }
            return 0;
        }

        public string CreateErrorString(string yytext, params object[] args)
        {
            string prefix = "";
            if (yytext != "")
                prefix = StringResources.Get("FOUND{0}");
            else
                prefix = StringResources.Get("FOUNDEOF");

            if (this.buildTreeForFormatterStrings && prefix == StringResources.Get("FOUNDEOF"))
            {
                yytext = "}";
                prefix = StringResources.Get("FOUND{0}");
            }
                
            // Преобразовали в список строк - хорошо
            List<string> tokens = new List<string>(args.Skip(1).Cast<string>());

            // Исключаем, т.к. в реальных программах никогда не встретятся
            tokens = tokens.Except((new string[] { "tkParseModeExpression", "tkParseModeStatement", "tkDirectiveName" })).ToList();

            // Это - временное решение, пока эти слова относятся к идентификаторам (что неправильно)
            if (tokens.Contains("tkIdentifier"))
                tokens = tokens.Except((new string[] { "tkAbstract", "tkOverload", "tkReintroduce", "tkOverride", "tkVirtual", "tkAt", "tkOn", "tkName", "tkForward", "tkRead", "tkWrite" })).ToList();

            // Добавляем фиктивный токен, что означает, что далее могут идти несколько токенов, начинающих выражение
            if (tokens.Contains("tkFor") && tokens.Contains("tkIf") && tokens.Contains("tkRepeat") && tokens.Contains("tkWhile"))
            {
                tokens.Clear();
                tokens.Add("tkStatement");
            }

            // Добавляем фиктивный токен, что означает, что далее могут идти несколько токенов, начинающих выражение
            if (tokens.Contains("tkIdentifier") && tokens.Contains("tkInteger") && tokens.Contains("tkFloat"))
            {
                tokens.Clear();
                tokens.Add("tkExpression");
            }

            tokens = tokens.OrderByDescending(s => TokenPriority(s)).ToList();

            /*if (args.Contains("EOF") && yytext!="")
                return "Текст за концом программы недопустим";
             if (args.Contains("tkIdentifier"))
                 return string.Format(prefix + "ожидался идентификатор", "'" + yytext + "'");*/
            if (tokens.Contains("tkProgram"))
                return string.Format(prefix + StringResources.Get("EXPECTEDBEGIN"), "'" + yytext + "'");

            var MaxTok = tokens.First();
            if (yytext != null && yytext.ToLower() == "record" && MaxTok == "tkSealed")
                return StringResources.Get("WRONG_ATTRIBUTE_FOR_RECORD");

            var ExpectedString = StringResources.Get("EXPECTED{1}");

            if (MaxTok.Equals("tkStatement") || MaxTok.Equals("tkIdentifier"))
                ExpectedString = StringResources.Get("EXPECTEDR{1}");
            else if (MaxTok.Equals("tkStringLiteral"))
                ExpectedString = StringResources.Get("EXPECTEDF{1}");
            if ((MaxTok == "EOF" || MaxTok == "EOF1" || MaxTok == "FOUNDEOF") && this.buildTreeForFormatterStrings)
                MaxTok = "}";
            var MaxTokHuman = ConvertToHumanName(MaxTok);

            // string w = string.Join(" или ", tokens.Select(s => ConvertToHumanName((string)s)));

            return string.Format(prefix + ExpectedString, "'" + yytext + "'", MaxTokHuman); 
        }

        public string directive_parameter(string s)
        {
            var ind = s.IndexOf(" ");
            if (ind < 0)
                return "";
            else
            {
                s = s.Substring(ind + 1);
                s = s.TrimEnd('}');
                return s;
            }
        }

        public ident create_directive_name(string text, SourceContext sc)
        {
            ident dn = new ident(new string(text.ToCharArray(1, text.Length - 1)));
            dn.source_context = sc;
            return dn;
        }

        public function_lambda_definition find_pascalABC_lambda_name(string name)
        {
            int i = 0;
            while (i < pascalABCLambdaDefinitions.Count && (pascalABCLambdaDefinitions[i] == null || ((function_lambda_definition)pascalABCLambdaDefinitions[i]).lambda_name != name))
                i++;
            if (i < pascalABCLambdaDefinitions.Count)
                return (function_lambda_definition)pascalABCLambdaDefinitions[i];
            else
                return null;
        }

        public var_def_statement find_var_statements(string name)
        {
            int i = pascalABCVarStatements.Count - 1;
            bool b = false;
            while (!b && i >= 0)
            {
                var_def_statement vds = (var_def_statement)pascalABCVarStatements[i];
                int j = 0;
                while (j < vds.vars.idents.Count &&
                    vds.vars.idents[j].name != name)
                    j++;
                if (j < vds.vars.idents.Count)
                    b = true;
                else
                    i--;
            }
            if (i >= 0 && i < pascalABCVarStatements.Count &&
                ((var_def_statement)pascalABCVarStatements[i]).vars_type != null)
                return (var_def_statement)pascalABCVarStatements[i];
            else
                return null;
        }

        public ident func_decl_lambda(object lr0, object lr2)
        {
            statement_list _statement_list = (statement_list)lr2;
            expression_list _expression_list = new expression_list();
            ident_list _i_l = new ident_list();
            formal_parameters _formal_parameters = new formal_parameters();
            if (lr0 != null)
            {
                List<object> ar = (List<object>)lr0;
                for (int i = 0; i < ar.Count; i++)
                    if (ar[i] is ident)
                        _i_l.idents.Add((ident)ar[i]);
                    else
                        _i_l.idents.Add(((var_def_statement)ar[i]).vars.idents[0]);

                for (int i = 0; i < _i_l.idents.Count; i++)
                    _expression_list.expressions.Add(_i_l.idents[i]);

                for (int i = 0; i < ar.Count; i++)
                {
                    ident_list _ident_list = new ident_list();
                    ident id = _i_l.idents[i];
                    _ident_list.idents.Add(id);
                    string name_param = id.name;
                    typed_parameters _typed_parameters = null;
                    int k = 0;
                    {
                        named_type_reference _named_type_reference = new named_type_reference();
                        type_definition t_d = new type_definition();
                        if (ar[i] is ident)
                        {
                            ident idtype = new ident("object");
                            _named_type_reference.names.Add(idtype);
                            t_d = (type_definition)_named_type_reference;
                        }
                        else
                        {
                            t_d = ((var_def_statement)ar[i]).vars_type;
                        }
                        _typed_parameters = new typed_parameters(_ident_list, t_d, parametr_kind.none, null);
                        //parsertools.create_source_context(_typed_parameters, _ident_list, t_d);

                    }
                    _formal_parameters.params_list.Add(_typed_parameters);
                }
            }
            //////////////////////////
            named_type_reference _named_type_reference1 = new named_type_reference();
            ident idtype1 = new ident("object");
            _named_type_reference1.source_context = idtype1.source_context;
            _named_type_reference1.names.Add(idtype1);
            /////////////////////////////
            lambdaNum++;
            function_lambda_definition _procedure_definition = new function_lambda_definition();
            _procedure_definition.formal_parameters = _formal_parameters;
            _procedure_definition.return_type = (type_definition)_named_type_reference1;
            _procedure_definition.ident_list = _i_l;
            _procedure_definition.proc_body = null;
            _procedure_definition.parameters = _expression_list;
            _procedure_definition.lambda_name = "__lambda__" + lambdaNum;
            //new function_lambda_definition(_formal_parameters, (type_definition)_named_type_reference1, _i_l, null, _expression_list, "lambda" + lambda_num);
            object rt = _i_l;
            _procedure_definition.proc_body = _statement_list;

            //////////////////////////////vnutrennie lambda
            if (_procedure_definition.defs == null)
                _procedure_definition.defs = new List<declaration>();
            while (pascalABCLambdaDefinitions.Count > 0 && pascalABCLambdaDefinitions[pascalABCLambdaDefinitions.Count - 1] != null)
            {
                _procedure_definition.defs.Add(lambda((function_lambda_definition)pascalABCLambdaDefinitions[pascalABCLambdaDefinitions.Count - 1]));
                pascalABCLambdaDefinitions.RemoveAt(pascalABCLambdaDefinitions.Count - 1);
            }
            if (pascalABCLambdaDefinitions.Count > 0 && pascalABCLambdaDefinitions[pascalABCLambdaDefinitions.Count - 1] == null)
                pascalABCLambdaDefinitions.RemoveAt(pascalABCLambdaDefinitions.Count - 1);
            pascalABCLambdaDefinitions.Add(_procedure_definition);
            ///////////////////////////////////////////////
            //parsertools.create_source_context(_procedure_definition, _expression_list, rt);
            ident _name = new ident(_procedure_definition.lambda_name);
            if (lr0 != null)
                _name.source_context = _i_l.idents[0].source_context;
            return _name;
        }

        public void add_lambda_to_program_block(block _block)
        {
            //tasha 16.04.2010 
            if (pascalABCLambdaDefinitions.Count > 0)
            {
                if (_block.defs == null)
                    _block.defs = new declarations();
                for (int i = 0; i < pascalABCLambdaDefinitions.Count; i++)
                    _block.defs.defs.Add(lambda((function_lambda_definition)pascalABCLambdaDefinitions[i]));
                pascalABCLambdaDefinitions.Clear();
            }
        }

        public void add_lambda(object lr1, procedure_definition _procedure_definition)
        {
            if (pascalABCLambdaDefinitions.Count > 0)//tasha 16.04.2010
            {
                block _block = (block)lr1;
                if (_block.defs == null)
                    _block.defs = new declarations();
                for (int i = 0; i < pascalABCLambdaDefinitions.Count; i++)
                    _block.defs.defs.Add(lambda((function_lambda_definition)pascalABCLambdaDefinitions[i]));
                pascalABCLambdaDefinitions.Clear();
                _procedure_definition.proc_body = (proc_block)_block;
            }
            else
                //////////////////////////tasha 16.04.2010
                _procedure_definition.proc_body = (proc_block)lr1;
        }

        public void for_assignment(object lr0, object lr2)
        {
            if (lr0 is ident && lr2 is ident && ((ident)lr2).name.Contains("__lambda__"))
            {
                string type_name = "";
                var_def_statement vds = find_var_statements(((ident)lr0).name);
                if (vds != null)
                    type_name = ((named_type_reference)vds.vars_type).names[0].name;
                if (type_name != "")
                {
                    int ii = 0;
                    while (ii < pascalABCTypeDeclarations.Count &&
                        ((type_declaration)pascalABCTypeDeclarations[ii]).type_name.name != type_name)
                        ii++;
                    if (ii < pascalABCTypeDeclarations.Count)
                    {
                        type_definition td = ((type_declaration)pascalABCTypeDeclarations[ii]).type_def;
                        function_lambda_definition fld = find_pascalABC_lambda_name(((ident)lr2).name);
                        fld.return_type = ((function_header)td).return_type;
                        for (int k = 0; k < fld.formal_parameters.params_list.Count && k < ((function_header)td).parameters.params_list.Count; k++)
                            fld.formal_parameters.params_list[k].vars_type = ((function_header)td).parameters.params_list[k].vars_type;
                    }
                    pascalABCVarStatements.Remove(vds);
                }
            }
        }

    }
}