using System;
using com.calitha.goldparser.structure;

namespace com.calitha.goldparser.content
{
	/// <summary>
	/// The DFAStateRecord is a record that defines a DFA state.
	/// </summary>
	public class DFAStateRecord
	{
		private int index;
		private bool acceptState;
		private int acceptIndex;
		private EdgeSubRecordCollection edgeSubRecords;

		public DFAStateRecord(Record record)
		{
			if (record.Entries.Count < 5)
				throw new CGTContentException("Invalid number of entries for DFA state");
			byte header = record.Entries[0].ToByteValue();
			if (header != 68) //'D'
				throw new CGTContentException("Invalid DFA state header");
			this.index = record.Entries[1].ToIntValue();
			this.acceptState = record.Entries[2].ToBoolValue();
			this.acceptIndex = record.Entries[3].ToIntValue();
			//skip empty reserved entry
			edgeSubRecords = new EdgeSubRecordCollection(record,5);
		}

		public int Index{get{return index;}}
		public bool AcceptState{get{return acceptState;}}
		public int AcceptIndex{get{return acceptIndex;}}
		public EdgeSubRecordCollection EdgeSubRecords{get{return edgeSubRecords;}}
	}
	
}
