using System;
using com.calitha.goldparser.structure;

namespace com.calitha.goldparser.content
{
	/// <summary>
	/// The SymbolRecord is a record that defines a symbol.
	/// </summary>
	public class SymbolRecord
	{
		private int index;
		private string name;
		private int kind;

		public SymbolRecord(Record record)
		{
			if (record.Entries.Count != 4)
				throw new CGTContentException("Invalid number of entries for symbol");
			byte header = record.Entries[0].ToByteValue();
			if (header != 83) //'S'
				throw new CGTContentException("Invalid symbol header");
			this.index  = record.Entries[1].ToIntValue();
			this.name   = record.Entries[2].ToStringValue();
			this.kind   = record.Entries[3].ToIntValue();
		}
		
		public int Index{get{return index;}}
		public string Name{get{return name;}}
		public int Kind{get{return kind;}}
	}
}
