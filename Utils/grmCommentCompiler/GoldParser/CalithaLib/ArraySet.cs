using System;
using System.Collections;

namespace com.calitha.commons
{
	/// <summary>
	/// Implements a set by using an ArrayList internally.
	/// It is thefore only wise to use it for small collections.
	/// </summary>
	public class ArraySet : AbstractSet
	{
		private IList list;

		public ArraySet() : base(new ArrayList())
		{
			list = (ArrayList)collection;
		}

		public override void Add(Object obj)
		{
			list.Add(obj);
		}

		public override void Clear()
		{
			list.Clear();
		}

		public override bool Contains(Object obj)
		{
			return list.Contains(obj);
		}

		public override void Remove(Object obj)
		{
			list.Remove(obj);
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
		public override IEnumerator GetEnumerator()
		{
			return collection.GetEnumerator();
		}


	}

}
