// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Text;
using System.Collections.Generic;
//using com.calitha.goldparser;
using System.Runtime.Serialization;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using System.IO;
using GoldParser;

namespace PascalABCCompiler.ParserTools
{
    public class GPBParser // Странный класс, не являющийся парсером, а служащий для передачи информации. Состоит из двух частей: ссылок на парсер и грамматику и информации для передачи
    {
        public Parser LRParser;
        public Grammar LanguageGrammar;
	    public List<Error> errors;
        public string current_file_name;
        public int max_errors = 10;
        public object prev_node = null;
        public parser_tools parsertools;
        public List<compiler_directive> CompilerDirectives;
        
        public GPBParser(Stream stream)
        {
            LRParser = null;
            LanguageGrammar = new GoldParser.Grammar(new BinaryReader(stream));
        }
        
        public GPBParser(GoldParser.Grammar grammar)
        {
        	LRParser = null;
        	LanguageGrammar = grammar;
        }
        
        public GPBParser()
        {
            LRParser = null;
            LanguageGrammar = null;
        }
    }

	[Serializable()]
	public class SymbolException : System.Exception
	{
		public SymbolException(string message) : base(message)
		{
		}

		public SymbolException(string message,
			Exception inner) : base(message, inner)
		{
		}

		protected SymbolException(SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}

	}

	[Serializable()]
	public class RuleException : System.Exception
	{

		public RuleException(string message) : base(message)
		{
		}

		public RuleException(string message,
			Exception inner) : base(message, inner)
		{
		}

		protected RuleException(SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}

	}
	public class parser_tools
	{
		private const int max_char_const=0xFFFF;

        public int LineCorrection = 0;
        public GoldParser.Parser parser; // Ужас - два разных парсера! SSM 19.01.12

        public virtual void check_comment_text(GPBParser parser)
        {

        }
        
        public virtual SourceContext GetTokenSourceContext()
        {
            int LineNum = parser.TokenLineNumber + LineCorrection;
            return new SourceContext(LineNum, parser.TokenLinePosition, LineNum, parser.TokenLinePosition + parser.TokenLength - 1, parser.TokenCharPosition, parser.TokenCharPosition + parser.TokenLength);
        }

        public virtual SourceContext GetTokenSourceContext(GoldParser.Parser parser)
        {
            int LineNum = parser.TokenLineNumber + LineCorrection;
            return new SourceContext(LineNum, parser.TokenLinePosition, LineNum, parser.TokenLinePosition + parser.TokenLength - 1, parser.TokenCharPosition, parser.TokenCharPosition + parser.TokenLength);
        }
        
        /*public virtual string terminal_token_to_string(TerminalToken token)
		{
			string name=symbol_to_string(token.Symbol);
			if (name!=null) return name;
			if (token.UserObject!=null)
			{
				if (token.UserObject is token_info) return ((token_info)token.UserObject).text;
				return token.UserObject.ToString();
			}

			return token.Text;
		} */

        public virtual List<Symbol> GetPrioritySymbols(Symbol[] Symbols)
        {
            List<Symbol> res = new List<Symbol>();
            int maxp = -1, p;
            foreach (Symbol symbol in Symbols)
            {
                p = symbol_priority(symbol);
                if (p > maxp) maxp = p;
            }
            foreach (Symbol symbol in Symbols)
                if (symbol_priority(symbol) == maxp)
                    res.Add(symbol);
            return res;
        }

        public string[] SymbolsToStrings(Symbol[] Symbols)
        {
        	string[] res = new string[Symbols.Length>0?1:0];
            for (int i = 0; i < res.Length; i++)
                res[i] = symbol_to_string(Symbols[i]);
            return res;
        }

        public virtual string symbol_collection_to_string(Symbol[] Symbols)
		{
            return PascalABCCompiler.FormatTools.ObjectsToString(SymbolsToStrings(GetPrioritySymbols(Symbols).ToArray()), ",");
		}

		public virtual string symbol_to_string(Symbol symbol)
		{
			return null;
		}

		public virtual int symbol_priority(Symbol symbol)
        {
            return 0;
		}

        public virtual ident create_directive_name(GPBParser parser)
        {
            return create_directive_name(parser.LRParser.TokenString, GetTokenSourceContext(parser.LRParser));
        }

        public virtual ident create_directive_name(string text, SourceContext sc)
        {
            ident dn = new ident(new string(text.ToCharArray(1, text.Length - 1)));
            dn.source_context = sc;
            return dn;
        }

        public virtual string ReplaceSpecialSymbols(string text)
        {
            text = text.Replace("''", "'");
            return text;
        }

        public virtual char_const create_char_const(GPBParser parser)
        {
            return create_char_const(parser.LRParser.TokenString,GetTokenSourceContext(parser.LRParser));
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

        public virtual sharp_char_const create_sharp_char_const(GPBParser parser)
        {
            string text = parser.LRParser.TokenString;
            string int_text = new string(text.ToCharArray(1, text.Length - 1));
            SourceContext sc = GetTokenSourceContext(parser.LRParser);//create_source_context(token);
            sharp_char_const scc = null;
            int val = 0;
            if (int.TryParse(int_text, out val))
            {
                if (val > max_char_const)
                {
                    scc = new sharp_char_const(0);
                    parser.errors.Add(new TooBigCharNumberInSharpCharConstant(parser.current_file_name, sc, scc));
                }
                else
                    scc = new sharp_char_const(val);
                scc.source_context = sc;
            }
            else
            {
                parser.errors.Add(new TooBigCharNumberInSharpCharConstant(parser.current_file_name, sc, scc));
            }
            return scc;
        }

        public virtual const_node create_double_const(GPBParser parser)
		{
			const_node cn=null;
            SourceContext sc = GetTokenSourceContext(parser.LRParser);
			try
			{
				System.Globalization.NumberFormatInfo sgnfi=new System.Globalization.NumberFormatInfo();
				sgnfi.NumberDecimalSeparator=".";
				double val=double.Parse(parser.LRParser.TokenString,sgnfi);
				cn=new double_const(val);
				cn.source_context=sc;
			}
			catch(Exception)
			{
				parser.errors.Add(new BadFloat(parser.current_file_name,sc,(syntax_tree_node)parser.prev_node));
			}
			return cn;
		}

        public virtual const_node create_int_const(GPBParser parser)
        {
            return create_int_const(parser, System.Globalization.NumberStyles.Integer);
        }

        public virtual const_node create_hex_const(GPBParser parser)
        {
            return create_int_const(parser, System.Globalization.NumberStyles.HexNumber);
        }

        public virtual const_node create_int_const(GPBParser parser, System.Globalization.NumberStyles NumberStyles)
		{
            //таблица целых констант на уровне синтаксиса
            //      не может быть - 0 +
            // 32--------16----8----|----8----16--------32----------------64(bits)
            // [  int64  )[       int32       ](  int64 ](      uint64     ]
            string text = parser.LRParser.TokenString;
            if (NumberStyles == System.Globalization.NumberStyles.HexNumber)
                text = text.Substring(1);
            const_node cn=new int32_const();
            SourceContext sc = GetTokenSourceContext(parser.LRParser);//create_source_context(token);
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
                    if(NumberStyles == System.Globalization.NumberStyles.HexNumber)
                        parser.errors.Add(new BadHex(parser.current_file_name, sc, (syntax_tree_node)parser.prev_node));
                    else
                        parser.errors.Add(new BadInt(parser.current_file_name, sc, (syntax_tree_node)parser.prev_node));
                }
            }
            cn.source_context = sc;
			return cn;
		}
        
        public virtual ident create_ident(GPBParser parser)
        {
            string text = parser.LRParser.TokenString;
            if (text.IndexOf('&') == 0)
                text = text.Substring(1);
            ident id = new ident(text);
            id.source_context = GetTokenSourceContext(parser.LRParser);
            return id;
        }

        public procedure_attributes_list AddModifier(procedure_attributes_list proc_list, SyntaxTree.proc_attribute modif)
        {
            if (proc_list == null)
                proc_list = new procedure_attributes_list();
            foreach (procedure_attribute attr in proc_list.proc_attributes)
                if (attr.attribute_type == modif)
                    return proc_list;
            proc_list.proc_attributes.Add(new procedure_attribute(modif));
            return proc_list;
        }

		public virtual literal create_string_const(GPBParser parser)
		{
			literal lt;
            string text = parser.LRParser.TokenString;
			if (text.Length==3 && text[0]=='\'' && text[2]=='\'')
			{
				lt=new char_const(text[1]);
                lt.source_context = GetTokenSourceContext(parser.LRParser);//create_source_context(token);
				return lt;
			}
            text = ReplaceSpecialSymbols(text.Substring(1, text.Length - 2));
			lt=new string_const(text);
            lt.source_context = GetTokenSourceContext(parser.LRParser);//create_source_context(token);
			return lt;
			
		}

		/*public SourceContext create_source_context(com.calitha.goldparser.TerminalToken token)
		{
			return new SourceContext(token.Location.LineNr+1,token.Location.ColumnNr+1,token.Location.LineNr+1,token.Location.ColumnNr+token.Text.Length);
		}
		public file_position create_file_position(com.calitha.goldparser.TerminalToken token)
		{
			return new file_position(token.Location.LineNr+1,token.Location.ColumnNr+1);
		}*/

        public void create_source_context(object to,object left,object right)
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
            file_position fp=((syntax_tree_node)left).source_context.begin_position;
            ((syntax_tree_node)to).source_context = new SourceContext(fp.line_num,fp.column_num,fp.line_num,fp.column_num,0,0);
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

        public object sc_not_null(object o1, object o2,object o3)
        {
            if (o1 != null)
                if (((syntax_tree_node)o1).source_context != null) return o1;
            if (o2 != null)
                if (((syntax_tree_node)o2).source_context != null) return o2;
            return o3;
        }

        public object sc_not_null(params object[] arr)
		{
			foreach(object o in arr)
				if (o!=null) 
					if (((syntax_tree_node)o).source_context!=null) return o;
			return null;
		}

		public void assign_source_context(object to,object from)
		{
			//debug
			//if (((tree_node)from).source_context==null) Console.WriteLine("\n\rerror: from sc is null(assign_source_context)!\n\r");
			((syntax_tree_node)to).source_context=((syntax_tree_node)from).source_context;
		}

		/*public SourceContext create_source_context(Token[] tokens)
		{
			object right=null,left=null;
			for(int i=0;i<tokens.Length;i++)
				if (tokens[i].UserObject!=null)
					if(((tree_node)tokens[i].UserObject).source_context!=null)
					{
						if (left==null) 
							left=tokens[i].UserObject;
						right=tokens[i].UserObject;
					}
			if (left==null) return null;
			return new SourceContext(((tree_node)left).source_context,((tree_node)right).source_context);
		}*/
	}

	
	



}
