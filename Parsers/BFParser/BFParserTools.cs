using System;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.ParserTools;
using GoldParser;


namespace PascalABCCompiler.BFParser
{
	public class BF_parsertools:parser_tools
	{
		public override string symbol_to_string(Symbol symbol)
		{
			return null;
		}
		public override int symbol_priority(Symbol symbol)
		{
			return 0;
		}
	}
}
