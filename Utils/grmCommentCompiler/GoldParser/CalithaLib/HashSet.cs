using System;
using System.Collections;

namespace com.calitha.commons
{
	/// <summary>
	/// Implements a set using a hastable. It is therefore fast even
	/// when having large collections.
	/// </summary>
	public class HashSet : AbstractSet
	{
		private IDictionary map;

		public HashSet() : base(new Hashtable())
		{
			map = (Hashtable)collection;
		}

		public override void Add(Object obj)
		{
			
			map.Add(obj,null);
		}

		public override void Clear()
		{
			map.Clear();
		}

		public override bool Contains(Object obj)
		{
			return map.Contains(obj);
		}

		public override void Remove(Object obj)
		{
			map.Remove(obj);
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
		public override IEnumerator GetEnumerator()
		{
			return map.Keys.GetEnumerator();
		}


	}
}
