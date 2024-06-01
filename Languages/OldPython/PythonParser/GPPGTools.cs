// Move this file to ParserTools project, when more then one parser come with gppg

using System;
using System.Collections.Generic;
using System.Text;
using gppg;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;


namespace GPPGTools
{
    public static class GPPGParser
    {
        // HACK: used static, 'cause need in GPPGParserTools, yacc, lex

        public static compilation_unit CompilationUnit; // ! syntax tree

        public static string current_file_name;
        public static List<Error> errors;
        public static int max_errors = 10;
        public static List<compiler_directive> CompilerDirectives;
    }

    public class GPPGParserTools
    {
        public virtual SourceContext GetTokenSourceContext(LexLocation loc)
        {
            // HACK: loc.sCol + 1 -> special magic with source context :(
            return new SourceContext(loc.sLin,loc.sCol + 1,loc.eLin,loc.eCol);
        }

        public virtual void AssignSourceContext(object to, LexLocation loc)
        {
            if (to != null)
                ((syntax_tree_node) to).source_context = GetTokenSourceContext(loc);
        }

        public virtual statement_list GetStatements(object _object)
        {
            statement_list _statement_list;
            if (_object is statement_list)
                _statement_list = _object as statement_list;
            else
            {
                _statement_list = new statement_list();
                if (_object is statement)
                    _statement_list.subnodes.Add(_object as statement);
                else
                    _statement_list.subnodes.Add(new empty_statement());
            }
            return _statement_list;
        }


        public virtual const_node create_int_const(string text,LexLocation loc)
        {
            return create_int_const(text,loc,System.Globalization.NumberStyles.Integer);
        }

        public virtual const_node create_hex_const(string text,LexLocation loc)
        {
            return create_int_const(text,loc,System.Globalization.NumberStyles.HexNumber);
        }

        public virtual const_node create_int_const(string text,LexLocation loc,System.Globalization.NumberStyles NumberStyles)
        {
            if (NumberStyles == System.Globalization.NumberStyles.HexNumber)
                text = text.Substring(1);
            const_node cn = new int32_const();
            if (text.Length < 8)
                (cn as int32_const).val = Int32.Parse(text,System.Globalization.NumberStyles.Integer);
            else
            {
                try
                {
                    UInt64 uint64 = UInt64.Parse(text,System.Globalization.NumberStyles.Integer);
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
                        GPPGParser.errors.Add(new BadHex(GPPGParser.current_file_name,GetTokenSourceContext(loc),new syntax_tree_node()));
                    else
                        GPPGParser.errors.Add(new BadInt(GPPGParser.current_file_name,GetTokenSourceContext(loc),new syntax_tree_node()));
                }
            }
            cn.source_context = GetTokenSourceContext(loc);
            return cn;
        }

        public virtual const_node create_double_const(string text,LexLocation loc)
        {
            const_node cn = null;
            try
            {
                System.Globalization.NumberFormatInfo sgnfi = new System.Globalization.NumberFormatInfo();
                sgnfi.NumberDecimalSeparator = ".";
                double val = double.Parse(text,sgnfi);
                cn = new double_const(val);
                cn.source_context = GetTokenSourceContext(loc);
            }
            catch (Exception)
            {
                PascalABCCompiler.PythonABCParser.Errors.UnexpectedToken ut = new PascalABCCompiler.PythonABCParser.Errors.UnexpectedToken(GPPGParser.current_file_name,GetTokenSourceContext(loc),new syntax_tree_node());
                GPPGParser.errors.Add(ut);
            }
            return cn;
        }

        public virtual char_const create_char_const(string text,LexLocation loc)
        {
            string char_text = new string(text.ToCharArray(1,text.Length - 2));
            SourceContext sc = GetTokenSourceContext(loc);
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

        public virtual literal create_string_const(string text,LexLocation loc)
        {
            literal lt;
            if (text.Length == 3 && text[0] == '\'' && text[2] == '\'')
            {
                lt = new char_const(text[1]);
                lt.source_context = GetTokenSourceContext(loc);
                return lt;
            }
            string text1 = ReplaceSpecialSymbols(text.Substring(1, text.Length - 2));
            lt = new string_const(text1);
            lt.source_context = GetTokenSourceContext(loc);
            return lt;

        }

        public virtual bool_const create_bool_const(bool val,LexLocation loc)
        {
            bool_const bc = new bool_const(val);
            bc.source_context = GetTokenSourceContext(loc);
            return bc;
        }

        public virtual string ReplaceSpecialSymbols(string text)
        {
            text = text.Replace("''","'");
            return text;
        }

        public virtual ident create_directive_name(string name,LexLocation loc)
        {
            ident i = new ident(name);
            i.source_context = GetTokenSourceContext(loc);
            return i;
        }

        public virtual token_info create_token_info(string text,LexLocation loc)
        {
            token_info ti = new token_info(text);
            ti.source_context = GetTokenSourceContext(loc);
            return ti;
        }

        public virtual op_type_node create_op_type_node(Operators op,LexLocation loc)
        {
            op_type_node otn = new op_type_node(op);
            otn.source_context = GetTokenSourceContext(loc);
            return otn;
        }
    }
}
