using System;
using System.Collections;
using com.calitha.goldparser.structure;

namespace com.calitha.goldparser.content
{
	/// <summary>
	/// The SymbolTable contains a list of symbol records.
	/// </summary>
	public class SymbolTable : IEnumerable
	{
		private IList list;

		public SymbolTable(CGTStructure cgtStructure, int start, int count)
		{
			list = new ArrayList();
			for (int i=start;i<start+count;i++)
			{
				SymbolRecord symbol = new SymbolRecord(cgtStructure.Records[i]);
				list.Add(symbol);
			}
		}
		
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
	
		public SymbolRecord Get(int index)
		{
			return list[index] as SymbolRecord;
		}

		public SymbolRecord this[int index]
		{
			get
			{
				return Get(index);
			}
		}

		public int Count {get{return list.Count;}}
		
	}
	
}
