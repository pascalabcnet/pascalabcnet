using System;
using com.calitha.goldparser.structure;

namespace com.calitha.goldparser.content
{
	/// <summary>
	/// The LALRStateRecord is a record the defines a LALR state.
	/// </summary>
	public class LALRStateRecord
	{
		private int index;
		private ActionSubRecordCollection actionSubRecords;

		public LALRStateRecord(Record record)
		{
			if (record.Entries.Count < 3)
				throw new CGTContentException("Invalid number of entries for LALR state");
			byte header = record.Entries[0].ToByteValue();
			if (header != 76) //'L'
				throw new CGTContentException("Invalid LALR state header");
			this.index = record.Entries[1].ToIntValue();
			//skip empty reserved entry
			actionSubRecords = new ActionSubRecordCollection(record,3);
		}

		public int Index{get{return index;}}
		public ActionSubRecordCollection ActionSubRecords{get{return actionSubRecords;}}
	}
}
