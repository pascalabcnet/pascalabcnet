using System;
using System.Collections;

namespace SymbolTable
{
	/// <summary>
	/// 
	/// </summary>
	public class AreaArrayTable
	{
		private ArrayList data;
		
		public AreaArrayTable(int StartSize)
		{
			data=new ArrayList(StartSize);
		}
		public int Add(int[] table)
		{
			return data.Add(table);
		}
		public int[] this [int index]
		{
			get
			{
				return (data[index] as int[]);
			}
			set
			{
				data[index]=value;
			}
		}
	}
}
