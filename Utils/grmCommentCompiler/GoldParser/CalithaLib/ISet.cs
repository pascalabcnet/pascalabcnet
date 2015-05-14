using System;
using System.Collections;

namespace com.calitha.commons
{
	/// <summary>
	/// ISet is the interface used for all modifiable collections of object,
	/// in which the objects has no order.
	/// </summary>
	public interface ISet : ICollection
	{
		/// <summary>
		/// Adds an object to the set.
		/// </summary>
		/// <param name="obj">Object added</param>
		void Add(Object obj);

		/// <summary>
		/// Clears the entire set.
		/// </summary>
		void Clear();

		/// <summary>
		/// Determines if the set contains and object.
		/// </summary>
		/// <param name="obj">The object to search for.</param>
		/// <returns>True of the set contains it, otherwise false.</returns>
		bool Contains(Object obj);

		/// <summary>
		/// Removes an object from the set.
		/// </summary>
		/// <param name="obj">Object being removed.</param>
		void Remove(Object obj);


	}
}
