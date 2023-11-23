using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUT.Gppg;

using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;

namespace VeryBasicParser
{
    public class VeryBasicParserTools
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

        public List<var_def_statement> pascalABC_var_statements;
        public List<type_declaration> pascalABC_type_declarations;
        public VeryBasicParserTools()
        {
            NodesStack = new System.Collections.Stack();
            pascalABC_var_statements = new List<var_def_statement>();
            pascalABC_type_declarations = new List<type_declaration>();
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
        private const_node create_int_const(string text, SourceContext sc, System.Globalization.NumberStyles NumberStyles)
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
        public const_node create_int_const(string text, SourceContext sc)
        {
            return create_int_const(text, sc, System.Globalization.NumberStyles.Integer);
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
        public ident create_ident(string text, SourceContext sc)
        {
            if (text[0] == '&')
                text = text.Substring(1);
            ident id = new ident(text);
            id.source_context = sc;
            return id;
        }
        public void AddError(string message, LexLocation loc)
        {
            errors.Add(new SyntaxError(message, CurrentFileName, loc, null));
        }

        public string CreateErrorString(string yytext, LexLocation yyloc, params object[] args)
        {
            string expected = String.Join(" ", args.Skip(1).Select(x => x.ToString()));
            string err = $"PARSER ERROR \"{yytext}\" AT LINE #{yyloc.StartLine}: EXPECTED  {expected}, FOUND {args[0]}";
            return err;
        }
    }
}
