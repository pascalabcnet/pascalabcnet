using System;
using System.Collections;
using com.calitha.goldparser.structure;

namespace com.calitha.goldparser.content
{
	/// <summary>
	/// CharacterSetTable contains a list of CharacterSet records.
	/// </summary>
	public class CharacterSetTable : IEnumerable
	{
		private IList list;

		public CharacterSetTable(CGTStructure structure, int start, int count)
		{
			list = new ArrayList();
			for (int i=start;i<start+count;i++)
			{
				CharacterSetRecord charSet = new CharacterSetRecord(structure.Records[i]);
				list.Add(charSet);
			}	
		}
		
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public CharacterSetRecord Get(int index)
		{
			return list[index] as CharacterSetRecord;
		}
		
		public CharacterSetRecord this[int index]
		{
			get
			{
				return Get(index);
			}
		}

		public int Count {get{return list.Count;}}

	}
}
