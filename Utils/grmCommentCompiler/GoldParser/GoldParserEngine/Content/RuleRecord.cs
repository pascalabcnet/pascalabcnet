using System;
using com.calitha.commons;
using com.calitha.goldparser.structure;

namespace com.calitha.goldparser.content
{
	/// <summary>
	/// The RuleRecord is a record the defines a rule to reduce tokens.
	/// </summary>
	public class RuleRecord
	{
		private int index;
		private int nonterminal;
		private IntegerList symbols;

		public RuleRecord(Record record)
		{
			if (record.Entries.Count < 4)
				throw new CGTContentException("Invalid number of entries for rule");
			byte header = record.Entries[0].ToByteValue();
			if (header != 82) //'R'
				throw new CGTContentException("Invalid rule header");
			this.index = record.Entries[1].ToIntValue();
			this.nonterminal = record.Entries[2].ToIntValue();		
			//skip reserved empty entry
			this.symbols = new IntegerList();
			for (int i=4;i<record.Entries.Count;i++)
			{
				int symbol = record.Entries[i].ToIntValue();
				symbols.Add(symbol);
			}
		}
		
		public int Index{get{return index;}}
		public int Nonterminal{get{return nonterminal;}}
		public IntegerList Symbols{get{return symbols;}}
	}
}
