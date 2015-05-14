using System;
using System.Text;
using System.Collections;

namespace com.calitha.goldparser.structure
{

	/// <summary>
	/// RecordCollection is a type-safe list for Record items.
	/// </summary>
	public class RecordCollection : IEnumerable
	{
		private IList list;

		public RecordCollection()
		{
			list = new ArrayList();
		}
		
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
		
		public int Add(Record record)
		{
			return list.Add(record);
		}
		
		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			str.Append("Records:\n");
			foreach (Record record in this)
			{
				str.Append("***START RECORD***\n");
				str.Append(record.ToString());                
				str.Append("***END RECORD***\n");
			}
			return str.ToString();
		}			

		public Record Get(int index)
		{
			if (index < 0 || index >= list.Count)
				return null;
			else
				return list[index] as Record;
		}
			

	
		public Record this[int index]
		{
			get
			{
				return Get(index);
			}
		}
		
		public int Count { get{return list.Count;} }
	}

	/// <summary>
	/// The Record is part of the compiled grammar table that contains one or more entries.
	/// </summary>
	public class Record
	{
		private EntryCollection entries;

		public Record()
		{
			this.entries = new EntryCollection();
		}
		
		public override string ToString()
		{
			return entries.ToString();
		}
		
		public EntryCollection Entries{ get{return entries;} }
	}
}
