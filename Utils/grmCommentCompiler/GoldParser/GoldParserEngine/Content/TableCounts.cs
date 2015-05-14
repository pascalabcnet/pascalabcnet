using System;
using com.calitha.goldparser.structure;

namespace com.calitha.goldparser.content
{
	/// <summary>
	/// The TableCounts contain how many record there are for symbols, character sets,
	/// rules, DFA states and LALR states.
	/// </summary>
	public class TableCounts
	{
		private int symbolTable;
		private int characterSetTable;
		private int ruleTable;
		private int dfaTable;
		private int lalrTable;

		public TableCounts(Record record)
		{
			if (record.Entries.Count != 6)
				throw new CGTContentException("Invalid number of entries for table counts");
			byte header = record.Entries[0].ToByteValue();
			if (header != 84) //'T'
				throw new CGTContentException("Invalid table counts header");							
			this.symbolTable        = record.Entries[1].ToIntValue();
			this.characterSetTable  = record.Entries[2].ToIntValue();
			this.ruleTable          = record.Entries[3].ToIntValue();
			this.dfaTable           = record.Entries[4].ToIntValue();
			this.lalrTable          = record.Entries[5].ToIntValue();
		}
		
		public int SymbolTable{get{return symbolTable;}}
		public int CharacterSetTable{get{return characterSetTable;}}
		public int RuleTable{get{return ruleTable;}}
		public int DFATable{get{return dfaTable;}}
		public int LALRTable{get{return lalrTable;}}
	}
}
