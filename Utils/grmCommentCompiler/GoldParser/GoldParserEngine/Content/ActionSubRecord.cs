using System;
using System.Collections;
using com.calitha.goldparser.content;
using com.calitha.goldparser.structure;

namespace com.calitha.goldparser.content
{

	/// <summary>
	/// ActionSubRecordCollection contains parts a record that define the actions in a LALR state.
	/// </summary>
	public class ActionSubRecordCollection : IEnumerable
	{
		private IList list;

		public ActionSubRecordCollection(Record record, int start)
		{
			list = new ArrayList();
			if ((record.Entries.Count-start) % 4 != 0)
				throw new CGTContentException("Invalid number of entries for actions in LALR state");
			for (int i=start;i<record.Entries.Count;i=i+4)
			{
				ActionSubRecord actionRecord = new ActionSubRecord(record.Entries[i],
																   record.Entries[i+1],
																   record.Entries[i+2]);
				list.Add(actionRecord);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

	}

	/// <summary>
	/// The ActionSubRecord is a part of a record that define action in a LALR state.
	/// </summary>
	public class ActionSubRecord
	{
		public ActionSubRecord(Entry symbolEntry, Entry actionEntry, Entry targetEntry)
		{
			this.symbolIndex = symbolEntry.ToIntValue();
			this.action = actionEntry.ToIntValue();
			this.target = targetEntry.ToIntValue();
		}

		protected int symbolIndex;
		protected int action;
		protected int target;
		public int SymbolIndex{get{return symbolIndex;}}
		public int Action{get{return action;}}
		public int Target{get{return target;}}
	}
}
