using System;
using com.calitha.goldparser.content;

namespace com.calitha.goldparser
{
	/// <summary>
	/// The SymbolFactory is for creating a symbol identified by a record in
	/// the fil content.
	/// </summary>
	public sealed class SymbolFactory
	{
		private SymbolFactory()
		{
		}

		/// <summary>
		/// Creates a new symbol or gives a reference to a symbol that is
		/// determined by the type of the symbol record in the file content.
		/// </summary>
		/// <param name="symbolRecord"></param>
		/// <returns></returns>
		static public Symbol CreateSymbol(SymbolRecord symbolRecord)
		{
			Symbol symbol;
			switch (symbolRecord.Kind)
			{
				case 0:
					symbol = new SymbolNonterminal(symbolRecord.Index,symbolRecord.Name);
					break;
				case 1:
					symbol = new SymbolTerminal(symbolRecord.Index,symbolRecord.Name);
					break;
				case 2:
					symbol = new SymbolWhiteSpace(symbolRecord.Index);
					break;
				case 3:
					symbol = SymbolCollection.EOF;
					break;
				case 4:
					symbol = new SymbolCommentStart(symbolRecord.Index);
					break;
				case 5:
					symbol = new SymbolCommentEnd(symbolRecord.Index);
					break;
				case 6:
					symbol = new SymbolCommentLine(symbolRecord.Index);
					break;
				case 7:
					symbol = SymbolCollection.ERROR;
					break;
				default:
					// this sort of symbol should never be here
					symbol = new SymbolError(-1);
					break;
			}
			return symbol;
		}
	}


}
