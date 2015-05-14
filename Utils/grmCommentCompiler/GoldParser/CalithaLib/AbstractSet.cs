using System;
using System.Collections;

namespace com.calitha.commons
{
	/// <summary>
	/// AbstractSet can be extended to implement a set that uses
	/// a collection instance to achieve the set functionality
	/// </summary>
	public abstract class AbstractSet : ISet
	{

		protected ICollection collection;

		/// <summary>
		/// This class itself is abstract, but this constructor can be called
		/// by extending classes to achieve common functionality for sets
		/// using a collection aggregate instance.
		/// </summary>
		/// <param name="collection"></param>
		public AbstractSet(ICollection collection)
		{
			this.collection = collection;
		}

		/// <summary>
		/// Number of objects currently in the collection.
		/// </summary>
		public virtual int Count 
		{
			get {return collection.Count;}
		}

		/// <summary>
		/// When implemented by a class, gets a value indicating whether
		/// access to the ICollection is synchronized (thread-safe).
		/// </summary>
		/// 
		public bool IsSynchronized 
		{
			get {return collection.IsSynchronized;}
		}
		
		/// <summary>
		/// When implemented by a class, gets an object that can be used
		/// to synchronize access to the ICollection.
		/// </summary>
		public object SyncRoot 
		{
			get {return SyncRoot;}
		}

		/// <summary>
		/// When implemented by a class, copies the elements of the ICollection 
		/// to an Array, starting at a particular Array index.
		/// </summary>
		/// <param name="array">The one-dimensional Array that is the destination 
		/// of the elements copied from ICollection. The Array must have 
		/// zero-based indexing.</param>
		/// <param name="index">The zero-based index in array at which 
		/// copying begins.</param>
		public void CopyTo(
			Array array,
			int index
			)
		{
			collection.CopyTo(array,index);
		}

		public abstract void Add(Object obj);
		public abstract void Clear();
		public abstract bool Contains(Object obj);
		public abstract void Remove(Object obj);

		abstract public IEnumerator GetEnumerator();


	}
}
