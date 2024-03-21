// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using QUT.Gppg;
using GPPGParserScanner;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PascalABCSavParser
{
    public class Union
    {   
	    public expression ex;
	    public ident id;                
        public Object ob;  
        public op_type_node op;  
        public syntax_tree_node stn;
        public token_info ti;
        public type_definition td;
    }

    public static class StringResources
    {
        private static string prefix = "PASCALABCPARSER_";
        public static string Get(string Id)
        {
            string ret = PascalABCCompiler.StringResources.Get(prefix + Id);
            if (ret == prefix + Id)
                return Id;
            else
                return ret;
        }
    }

    // Класс глобальных описаний и статических методов
    // для использования различными подсистемами парсера и сканера
    public class PT // PT - parser tools
    {
        private const int max_char_const = 0xFFFF;
        // SSM: Errors инициализируется в другом месте - сюда только передается!
        public List<Error> errors;
        public List<CompilerWarning> warnings;
        public System.Collections.Stack NodesStack; // SSM: для каких-то вспомогательных целей в двух правилах
        public bool build_tree_for_formatter = false;
        public bool build_tree_for_format_strings = false;
        public string CurrentFileName;
        public List<compiler_directive> compilerDirectives;

        int lambda_num = 0;
        public List<function_lambda_definition> pascalABC_lambda_definitions;
        public List<var_def_statement> pascalABC_var_statements;
        public List<type_declaration> pascalABC_type_declarations;
        public static Dictionary<string, string> tokenNum; // строки, соответствующие терминалам, для вывода ошибок - SSM

        // TODO: CompilerDirectives по идее сюда еще надо

        // Преобразование GPPG LexLocation в наш SourceContext

        static PT()
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

        public PT()
        {
            NodesStack = new System.Collections.Stack();
            pascalABC_lambda_definitions = new List<function_lambda_definition>();
            pascalABC_var_statements = new List<var_def_statement>();
            pascalABC_type_declarations = new List<type_declaration>();
        }

        public void DivideDirectiveOn(string yytext, out string directivename, out string directiveparam)
        {
            var ind = yytext.IndexOfAny(new char[]{' ','}'});
            directivename = yytext.Substring(2,ind-2).ToUpper().Trim();
            directiveparam = yytext.Substring(ind + 1).TrimEnd('}').Trim();
        }

        public void CheckDirectiveParams(string directivename, string directiveparam)
        {

        }

        public SourceContext ToSourceContext(LexLocation loc)
        {
            if (loc != null)
                return new SourceContext(loc.StartLine, loc.StartColumn + 1, loc.EndLine, loc.EndColumn);
            return null;
        }

        public string ConvertToHumanName(string s)
        {
            var v = s;
            if (tokenNum.ContainsKey(v))
                v = tokenNum[v];            
            else if (v.StartsWith("tk"))
                v = v.Remove(0,2).ToLower();

            return v;
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

        public string MaxToken(string[] args)
        {
            return args.Max(); 
        }

        public string CreateErrorString(string yytext, params object[] args)
        {
            string prefix = "";
            if (yytext != "")
                prefix = StringResources.Get("FOUND{0}");
            else
                prefix = StringResources.Get("FOUNDEOF");

            if (this.build_tree_for_format_strings && prefix == StringResources.Get("FOUNDEOF"))
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
            if ((MaxTok == "EOF" || MaxTok == "EOF1" || MaxTok == "FOUNDEOF") && this.build_tree_for_format_strings)
                MaxTok = "}";
            var MaxTokHuman = ConvertToHumanName(MaxTok);

            // string w = string.Join(" или ", tokens.Select(s => ConvertToHumanName((string)s)));

            return string.Format(prefix + ExpectedString, "'" + yytext + "'", MaxTokHuman); 
        }

        public void AddError(string message, LexLocation loc)
        {
           errors.Add(new SyntaxError(message, CurrentFileName, loc, null));
        }

        public void AddErrorFromResource(string res, PascalABCCompiler.SyntaxTree.SourceContext loc, params string[] pars)
        {
            res = StringResources.Get(res);
            if (pars != null && pars.Length > 0)
                res = string.Format(res, pars);
            errors.Add(new SyntaxError(res, CurrentFileName, loc, null));
        }

        public void AddWarningFromResource(string res, PascalABCCompiler.SyntaxTree.SourceContext loc, params string[] pars)
        {
            res = StringResources.Get(res);
            if (pars != null && pars.Length > 0)
                res = string.Format(res, pars);
            warnings.Add(new CommonWarning(res, CurrentFileName, loc.begin_position.line_num, loc.begin_position.column_num));
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

        public string ReplaceSpecialSymbols(string text)
        {
            text = text.Replace("''", "'");
            return text;
        }

        public char_const create_char_const(string text, SourceContext sc)
        {
            string char_text = new string(text.ToCharArray(1, text.Length - 2));
            char_text = ReplaceSpecialSymbols(char_text);
            char_const ct = new char_const();
            ct.source_context = sc;
            if (char_text.Length == 1)
            {
                ct.cconst = char_text[0];
                return ct;
            }
            return null;
        }

        public sharp_char_const create_sharp_char_const(string text, SourceContext sc)
        {
            string int_text = new string(text.ToCharArray(1, text.Length - 1));
            sharp_char_const scc = null;
            int val = 0;
            if (int.TryParse(int_text, out val))
            {
                if (val > max_char_const)
                {
                    scc = new sharp_char_const(0);
                    errors.Add(new TooBigCharNumberInSharpCharConstant(CurrentFileName, sc, scc));
                }
                else
                    scc = new sharp_char_const(val);
                scc.source_context = sc;
            }
            else
            {
                errors.Add(new TooBigCharNumberInSharpCharConstant(CurrentFileName, sc, scc));
            }
            return scc;
        }

        public const_node create_double_const(string text, SourceContext sc)
        {
            const_node cn = null;
            try
            {
                System.Globalization.NumberFormatInfo sgnfi = new System.Globalization.NumberFormatInfo();
                sgnfi.NumberDecimalSeparator = ".";

                text = RemoveThousandsDelimiter(text, sc);

                double val = double.Parse(text, sgnfi);
                cn = new double_const(val);
                cn.source_context = sc;
            }
            catch (Exception)
            {
                errors.Add(new BadFloat(CurrentFileName, sc, null));
            }
            return cn;
        }

        public const_node create_hex_const(string text, SourceContext sc)
        {
            return create_int_const(text, sc, System.Globalization.NumberStyles.HexNumber);
        }

        public const_node create_int_const(string text, SourceContext sc)
        {
            return create_int_const(text, sc, System.Globalization.NumberStyles.Integer);
        }

        public const_node create_bigint_const(string text, SourceContext sc)
        {
            text = RemoveThousandsDelimiter(text, sc);

            var txt = text.Substring(0, text.Length - 2);
            var cn = new bigint_const();
            try
            {
                cn.val = System.UInt64.Parse(txt);
            }
            catch (Exception)
            {
                errors.Add(new BadInt(CurrentFileName, sc, null));
            }
            cn.source_context = sc;
            return cn;
        }

        public string RemoveThousandsDelimiter(string s, SourceContext sc)
        {
            if (s.EndsWith("_") || s.Contains("__"))
            {
                var errstr = ParserErrorsStringResources.Get("BAD_FORMED_NUM_CONST");
                errors.Add(new SyntaxError(errstr, CurrentFileName, sc, null));
            }

            return s.Replace("_", "");
        }

        public const_node create_int_const(string text, SourceContext sc, System.Globalization.NumberStyles NumberStyles)
        {
            //таблица целых констант на уровне синтаксиса
            //      не может быть - 0 +
            // 32--------16----8----|----8----16--------32----------------64(bits)
            // [  int64  )[       int32       ](  int64 ](      uint64     ]
            text = RemoveThousandsDelimiter(text, sc);
            if (NumberStyles == System.Globalization.NumberStyles.HexNumber)
                text = text.Substring(1);
            const_node cn = new int32_const();
            if (text.Length < 8)
                (cn as int32_const).val = Int32.Parse(text, NumberStyles);
            else
            {
                try
                {
                    UInt64 uint64 = UInt64.Parse(text, NumberStyles);
                    if (uint64 <= Int32.MaxValue)
                        (cn as int32_const).val = (Int32)uint64;
                    else
                        if (uint64 <= Int64.MaxValue)
                            cn = new int64_const((Int64)uint64);
                        else
                            cn = new uint64_const(uint64);
                }
                catch (Exception)
                {
                    if (NumberStyles == System.Globalization.NumberStyles.HexNumber)
                        errors.Add(new BadHex(CurrentFileName, sc, null));
                    else
                        errors.Add(new BadInt(CurrentFileName, sc, null));
                }
            }
            cn.source_context = sc;
            return cn;
        }

        public ident create_ident(string text, SourceContext sc)
        {
            if (text[0] == '&')
                text = text.Substring(1);
            ident id = new ident(text);
            id.source_context = sc;
            return id;
        }

        public procedure_attributes_list AddModifier(procedure_attributes_list proc_list, proc_attribute modif)
        {
            if (proc_list == null)
                proc_list = new procedure_attributes_list();
            foreach (procedure_attribute attr in proc_list.proc_attributes)
                if (attr.attribute_type == modif)
                    return proc_list;
            proc_list.proc_attributes.Add(new procedure_attribute(modif));
            return proc_list;
        }

        public literal create_string_const(string text, SourceContext sc)
        {
            literal lt;
            if (text.Length == 3 && text[0] == '\'' && text[2] == '\'')
            {
                lt = new char_const(text[1]);
                lt.source_context = sc;
                return lt;
            }
            text = ReplaceSpecialSymbols(text.Substring(1, text.Length - 2));
            lt = new string_const(text);
            lt.source_context = sc;
            return lt;
        }

        public literal create_multiline_string_const(string text, SourceContext sc)
        {
            if (build_tree_for_formatter)
            {
                literal lt1 = new string_const(text);
                lt1.source_context = sc;
                return lt1;
            }

            var NewLine = System.Environment.NewLine;
            // Многострочная строка должна содержать как минимум два перехода на новую строку
            // разобьём её по символам перехода на новую строку
            //var ss = text.Split('\n');
            text = text.Substring(3, text.Length - 6);
            var ss = Regex.Split(text, System.Environment.NewLine);
            // Если одна строка, то ошибка - MULTILINE_STRING_SHOULD_BE_PLACED_ON_SEVERAL_LINES
            if (ss.Length == 1)
                //this.AddErrorFromResource("MULTILINE_STRING_SHOULD_BE_PLACED_ON_SEVERAL_LINES", sc);
                return create_string_const(text, sc);

            // Проверим первую строку
            ss[0] = ss[0].Trim();
            if (ss[0].Length > 0)
                // мы сюда точно не должны попасть. Это должна быть обычная строка!
                this.AddErrorFromResource("IMPOSSIBLE_MULTILINE_ERROR", sc);

            // Количество лидирующих пробелов в последней строке
            var numspaces = ss[ss.Length-1].TakeWhile(Char.IsWhiteSpace).Count();
            // Во всех строках от первой до предпоследней (если они есть) количество лидирующих пробелов должно быть не меньше чем в последней
            // мультистрочная строка содержит несовместимые отступы
            // MULTILINE_STRING_CONTAINS_INCONSISTENT_INDENTS
            for (var i = 1; i < ss.Length - 1; i++)
                if (ss[i].TakeWhile(Char.IsWhiteSpace).Count() < numspaces)
                {
                    this.AddErrorFromResource("MULTILINE_STRING_CONTAINS_INCONSISTENT_INDENTS", sc);
                    break;
                }
            // Теперь удалить ровно такое количество пробелов в каждой строке с 1 по предпоследнюю
            for (var i = 1; i < ss.Length - 1; i++)
                ss[i] = ss[i].Remove(0, numspaces);
            // Наконец, соберем text из всех строк с первой по предпоследнюю
            var ll = ss.ToList();
            ll.RemoveAt(0);
            ll.RemoveAt(ll.Count - 1);
            text = string.Join(NewLine, ll.ToArray());
            literal lt = new string_const(text);
            lt.source_context = sc;
            return lt;
        }

        public literal create_format_string_const(string text, SourceContext sc)
        {
            literal lt;
            text = ReplaceSpecialSymbols(text.Substring(2, text.Length - 3));
            lt = new string_const(text);
            (lt as string_const).IsInterpolated = true;
            lt.source_context = sc;
            return lt;
        }

        public procedure_definition lambda(function_lambda_definition _function_lambda_definition)
        {
            procedure_definition _func_def = new procedure_definition();
            method_name _method_name1 = new method_name(null,null, new ident(_function_lambda_definition.lambda_name), null);
            //parsertools.create_source_context(_method_name1, _method_name1.meth_name, _method_name1.meth_name);
            function_header _function_header1 = new function_header();

            object rt1 = new object();
            _function_header1.name = _method_name1;
            if (_function_header1.name.meth_name is template_type_name)
            {
                _function_header1.template_args = (_function_header1.name.meth_name as template_type_name).template_args;
                ident id = new ident(_function_header1.name.meth_name.name);
                //parsertools.create_source_context(id, _function_header1.name.meth_name, _function_header1.name.meth_name);
                _function_header1.name.meth_name = id;
            }

            formal_parameters fps = new PascalABCCompiler.SyntaxTree.formal_parameters();
            _function_header1.parameters = _function_lambda_definition.formal_parameters;//fps;

            /*SyntaxTree.named_type_reference _named_type_reference = new SyntaxTree.named_type_reference();
            SyntaxTree.ident idtype = new SyntaxTree.ident("object");
            _named_type_reference.source_context = idtype.source_context;
            _named_type_reference.names.Add(idtype);
            rt1 = _named_type_reference;
            _function_header1.return_type = (SyntaxTree.type_definition)_named_type_reference;*/
            _function_header1.return_type = _function_lambda_definition.return_type;

            _function_header1.of_object = false;
            _function_header1.class_keyword = false;
            token_info _token_info = new token_info("function");
            //_token_info.source_context = parsertools.GetTokenSourceContext();
            //parsertools.create_source_context(_function_header1, _token_info, _token_info);

            block _block1 = new block(null, null);
            statement_list sl1 = new statement_list();
            sl1.subnodes.Add(_function_lambda_definition.proc_body);
            _block1.program_code = sl1;
            _func_def.proc_header = _function_header1;
            _func_def.proc_body = (proc_block)_block1;
            if (_function_lambda_definition.defs != null)
            {
                if (((block)_func_def.proc_body).defs == null)
                    ((block)_func_def.proc_body).defs = new declarations();
                for (int l = 0; l < _function_lambda_definition.defs.Count; l++)
                    ((block)_func_def.proc_body).defs.defs.Add(_function_lambda_definition.defs[l] as procedure_definition);
            }
            _function_lambda_definition.proc_definition = _func_def;
            //parsertools.create_source_context(_func_def, _function_header1, _function_header1);
            return _func_def;
        }

        public function_lambda_definition find_pascalABC_lambda_name(string name)
        {
            int i = 0;
            while (i < pascalABC_lambda_definitions.Count && (pascalABC_lambda_definitions[i] == null || ((function_lambda_definition)pascalABC_lambda_definitions[i]).lambda_name != name))
                i++;
            if (i < pascalABC_lambda_definitions.Count)
                return (function_lambda_definition)pascalABC_lambda_definitions[i];
            else
                return null;
        }

        public var_def_statement find_var_statements(string name)
        {
            int i = pascalABC_var_statements.Count - 1;
            bool b = false;
            while (!b && i >= 0)
            {
                var_def_statement vds = (var_def_statement)pascalABC_var_statements[i];
                int j = 0;
                while (j < vds.vars.idents.Count &&
                    vds.vars.idents[j].name != name)
                    j++;
                if (j < vds.vars.idents.Count)
                    b = true;
                else
                    i--;
            }
            if (i >= 0 && i < pascalABC_var_statements.Count &&
                ((var_def_statement)pascalABC_var_statements[i]).vars_type != null)
                return (var_def_statement)pascalABC_var_statements[i];
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
            lambda_num++;
            function_lambda_definition _procedure_definition = new function_lambda_definition();
            _procedure_definition.formal_parameters = _formal_parameters;
            _procedure_definition.return_type = (type_definition)_named_type_reference1;
            _procedure_definition.ident_list = _i_l;
            _procedure_definition.proc_body = null;
            _procedure_definition.parameters = _expression_list;
            _procedure_definition.lambda_name = "__lambda__" + lambda_num;
            //new function_lambda_definition(_formal_parameters, (type_definition)_named_type_reference1, _i_l, null, _expression_list, "lambda" + lambda_num);
            object rt = _i_l;
            _procedure_definition.proc_body = _statement_list;

            //////////////////////////////vnutrennie lambda
            if (_procedure_definition.defs == null)
                _procedure_definition.defs = new List<declaration>();
            while (pascalABC_lambda_definitions.Count > 0 && pascalABC_lambda_definitions[pascalABC_lambda_definitions.Count - 1] != null)
            {
                _procedure_definition.defs.Add(lambda((function_lambda_definition)pascalABC_lambda_definitions[pascalABC_lambda_definitions.Count - 1]));
                pascalABC_lambda_definitions.RemoveAt(pascalABC_lambda_definitions.Count - 1);
            }
            if (pascalABC_lambda_definitions.Count > 0 && pascalABC_lambda_definitions[pascalABC_lambda_definitions.Count - 1] == null)
                pascalABC_lambda_definitions.RemoveAt(pascalABC_lambda_definitions.Count - 1);
            pascalABC_lambda_definitions.Add(_procedure_definition);
            ///////////////////////////////////////////////
            //parsertools.create_source_context(_procedure_definition, _expression_list, rt);
            ident _name = new ident(_procedure_definition.lambda_name);
            if (lr0 != null)
                _name.source_context = _i_l.idents[0].source_context;
            return _name;
        }

        public List<object> ident_list11(object lr1, object lr3)
        {
            List<object> ar = (List<object>)lr3;
            ar.Insert(0, lr1);
            return ar;
        }

        public List<object> ident_list12(object lr0)
        {
            List<object> ar = new List<object>();
            ar.Add(lr0);
            return ar;
        }

        public List<object> ident_list21(object lr0, object lr2)
        {
            List<object> ar = (List<object>)lr0;
            ar.Add(lr2);
            return ar;
        }

        public List<object> ident_list13(object lr1, object lr3, object lr5)
        {
            List<object> ar = (List<object>)lr5;
            //named_type_reference n_t_r = (named_type_reference)lr3;
            var_def_statement vds = new var_def_statement();
            vds.vars = new ident_list();
            vds.vars.idents.Add((ident)lr1);
            vds.vars_type = (type_definition)lr3;//n_t_r;
            ar.Insert(0, vds);
            return ar;
        }

        public List<object> ident_list14(object lr1, object lr3)
        {
            List<object> ar = new List<object>();
            //named_type_reference n_t_r = (named_type_reference)lr3;
            var_def_statement vds = new var_def_statement();
            vds.vars = new ident_list();
            vds.vars.idents.Add((ident)lr1);
            vds.vars_type = (type_definition)lr3;
            ar.Add(vds);
            return ar;
        }

        public void add_lambda_to_program_block(block _block)
        {
            //tasha 16.04.2010 
            if (pascalABC_lambda_definitions.Count > 0)
            {
                if (_block.defs == null)
                    _block.defs = new declarations();
                for (int i = 0; i < pascalABC_lambda_definitions.Count; i++)
                    _block.defs.defs.Add(lambda((function_lambda_definition)pascalABC_lambda_definitions[i]));
                pascalABC_lambda_definitions.Clear();
            }
        }

        public void add_lambda(object lr1, procedure_definition _procedure_definition)
        {
            if (pascalABC_lambda_definitions.Count > 0)//tasha 16.04.2010
            {
                block _block = (block)lr1;
                if (_block.defs == null)
                    _block.defs = new declarations();
                for (int i = 0; i < pascalABC_lambda_definitions.Count; i++)
                    _block.defs.defs.Add(lambda((function_lambda_definition)pascalABC_lambda_definitions[i]));
                pascalABC_lambda_definitions.Clear();
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
                    while (ii < pascalABC_type_declarations.Count &&
                        ((type_declaration)pascalABC_type_declarations[ii]).type_name.name != type_name)
                        ii++;
                    if (ii < pascalABC_type_declarations.Count)
                    {
                        type_definition td = ((type_declaration)pascalABC_type_declarations[ii]).type_def;
                        function_lambda_definition fld = find_pascalABC_lambda_name(((ident)lr2).name);
                        fld.return_type = ((function_header)td).return_type;
                        for (int k = 0; k < fld.formal_parameters.params_list.Count && k < ((function_header)td).parameters.params_list.Count; k++)
                            fld.formal_parameters.params_list[k].vars_type = ((function_header)td).parameters.params_list[k].vars_type;
                    }
                    pascalABC_var_statements.Remove(vds);
                }
            }
        }

        public void create_source_context(object to, object left, object right)
        {
            if (to != null)
                ((syntax_tree_node)to).source_context = get_source_context(left, right);
        }

        public SourceContext get_source_context(object left, object right)
        {
            //debug
            /*if (left == null && right!=null)
            {
                Console.WriteLine("\n\rerror: left is null(create_source_context)!\n\r");
                Console.WriteLine(((syntax_tree_node)right).source_context.ToString());
            }
            if (right == null && left!=null)
            {
                Console.WriteLine("\n\rerror: right is null(create_source_context)!\n\r");
                Console.WriteLine(((syntax_tree_node)left).source_context.ToString());
            }
            if (((syntax_tree_node)left).source_context == null)
            {
                Console.WriteLine("\n\rerror: source_context is null!(left)\n\r");
                return null;
            }
            if (((syntax_tree_node)right).source_context == null)
            {
                Console.WriteLine("\n\rerror: source_context is null!(right)\n\r");
                return null;
            }
            */
            if ((left == null) || (right == null) || (((syntax_tree_node)left).source_context == null) || (((syntax_tree_node)right).source_context == null))
                return null;
            return new SourceContext(((syntax_tree_node)left).source_context, ((syntax_tree_node)right).source_context);
        }

        public void create_source_context_left(object to, object left)
        {
            file_position fp = ((syntax_tree_node)left).source_context.begin_position;
            ((syntax_tree_node)to).source_context = new SourceContext(fp.line_num, fp.column_num, fp.line_num, fp.column_num, 0, 0);
        }

        public void create_source_context_right(object to, object right)
        {
            file_position fp = ((syntax_tree_node)right).source_context.end_position;
            ((syntax_tree_node)to).source_context = new SourceContext(fp.line_num, fp.column_num, fp.line_num, fp.column_num, 0, 0);
        }

        public object sc_not_null(object o1, object o2)
        {
            if (o1 != null)
                if (((syntax_tree_node)o1).source_context != null) return o1;
            return o2;
        }

        public object sc_not_null(object o1, object o2, object o3)
        {
            if (o1 != null)
                if (((syntax_tree_node)o1).source_context != null) return o1;
            if (o2 != null)
                if (((syntax_tree_node)o2).source_context != null) return o2;
            return o3;
        }

        public object sc_not_null(params object[] arr)
        {
            foreach (object o in arr)
                if (o != null)
                    if (((syntax_tree_node)o).source_context != null) return o;
            return null;
        }

        public void assign_source_context(object to, object from)
        {
            //debug
            //if (((tree_node)from).source_context==null) Console.WriteLine("\n\rerror: from sc is null(assign_source_context)!\n\r");
            if (to!=null && from!=null)
            ((syntax_tree_node)to).source_context = ((syntax_tree_node)from).source_context;
        }

        public statement MyStmt(expression ex, statement st)
        {
            // Проверить, что в ex - целый тип
            // Сделать специальный узел для проверки new semantic_check("Тип проверки",params syntax_node[] ob)
            // Включать этот узел первым для "сахарных" узлов синтаксического дерева
            var sc = new semantic_check("ExprIsInteger", ex);

            var id = new ident("#my");
            var idlist = new ident_list(id);
            var typ = new named_type_reference("integer");
            var one = new int32_const(1);
            var vdef = new var_def_statement(idlist, typ, one, definition_attribute.None, false, null);
            var vstat = new var_statement(vdef, null);

            var ass = new assign(new ident("#my"), one, Operators.AssignmentAddition);
            var stlistwhile = new statement_list(st);
            stlistwhile.Add(ass);

            var bin = new bin_expr(id, ex, Operators.LessEqual);

            var wh = new while_node(bin, stlistwhile, WhileCycleType.While);

            var stlist = new statement_list(sc);
            stlist.Add(vstat);
            stlist.Add(wh);
            return stlist;
        }
        public expression ConvertNamedTypeReferenceToDotNodeOrIdent(named_type_reference ntr) // либо ident либо dot_node
        {
            if (ntr.names.Count == 1)
                return ntr.names[0];
            else
            {
                var dn = new dot_node(ntr.names[0], ntr.names[1], ntr.names[0].source_context.Merge(ntr.names[1].source_context));
                for (var i = 2; i < ntr.names.Count; i++)
                    dn = new dot_node(dn, ntr.names[i],dn.source_context.Merge(ntr.names[i].source_context));
                dn.source_context = ntr.source_context;
                return dn;
            }
        }
        public named_type_reference ConvertDotNodeOrIdentToNamedTypeReference(expression en)
        {
            if (en is ident)
                return new named_type_reference(en as ident, en.source_context);
            if (en is dot_node)
            {
                var dn = en as dot_node;
                var sc = dn.source_context;
                var ids = new List<ident>();
                ids.Add(dn.right as ident);
                while (dn.left is dot_node)
                {
                    dn = dn.left as dot_node;
                    ids.Add(dn.right as ident);
                }
                ids.Add(dn.left as ident);
                ids.Reverse();
                return new named_type_reference(ids, sc);
            }
            this.AddErrorFromResource("TYPE_NAME_EXPECTED",  en.source_context);
            return null;
        }
    }
}