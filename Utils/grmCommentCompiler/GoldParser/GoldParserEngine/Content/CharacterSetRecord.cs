using System;
using com.calitha.goldparser.structure;

namespace com.calitha.goldparser.content
{
	/// <summary>
	/// The CharacterSetRecord is a record that defines a character set.
	/// </summary>
	public class CharacterSetRecord
	{
		private int index;
		private string characters;

		public CharacterSetRecord(Record record)
		{
			if (record.Entries.Count != 3)
				throw new CGTContentException("Invalid number of entries for character set");
			byte header = record.Entries[0].ToByteValue();
			if (header != 67) //'C'
				throw new CGTContentException("Invalid character set header");
			this.index = record.Entries[1].ToIntValue();
			this.characters = record.Entries[2].ToStringValue();
		}
		
		public int Index{get{return index;}}
		public string Characters{get{return characters;}}
	}
}
