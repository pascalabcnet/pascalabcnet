using System;
using System.Text;
using System.Collections;

namespace com.calitha.commons
{
	/// <summary>
	/// A type-safe list of integers.
	/// </summary>
	public class IntegerList
	{
		private ArrayList list;

		/// <summary>
		/// Creates a new integer list.
		/// </summary>
		public IntegerList()
		{
			list = new ArrayList();
		}
		
		/// <summary>
		/// Adds an integer to a list.
		/// </summary>
		/// <param name="value">The value that will be added.</param>
		/// <returns>Index at which the value has been added.</returns>
		public int Add(int value)
		{
			return list.Add(value);
		}
		
		/// <summary>
		/// Removes all elements from the list.
		/// </summary>
		public void Clear()
		{
			list.Clear();
		}

		/// <summary>
		/// A shallow copy of the list.
		/// </summary>
		/// <returns></returns>
		public virtual object Clone()
		{
			IntegerList result = new IntegerList();
			result.list = (ArrayList)list.Clone();
			return result;
		}

		public bool Contains(int value)
		{
			for (int i=0; i< list.Count; i++)
			{
				if (this[i] == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Copies the entire list to a compatible one-dimensional array,
		/// starting at the beginning of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional Array that is the destination
		/// of the elements copied from the list.
		/// The Array must have zero-based indexing.</param>
		public void CopyTo(Array array)
		{
			list.CopyTo(array);
		}

		/// <summary>
		/// Copies the entire list to a compatible one-dimensional Array, 
		/// starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional Array that is the destination
		/// of the elements copied from ArrayList.
		/// The Array must have zero-based indexing. </param>
		/// <param name="arrayIndex">The zero-based index in array at which
		/// copying begins.</param>
		public void CopyTo(Array array, int arrayIndex)
		{
			list.CopyTo(array,arrayIndex);
		}

		/// <summary>
		/// Copies a range of elements from the list to a compatible one-dimensional
		/// Array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="index">The zero-based index in the source list at which copying begins.</param>
		/// <param name="array">The one-dimensional Array that is the destination of
		/// the elements copied from the list. The Array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <param name="count">The number of elements to copy.</param>
		public void CopyTo(int index, Array array, int arrayIndex, int count)
		{
			list.CopyTo(index,array,arrayIndex,count);
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the current Object.
		/// </summary>
		/// <param name="obj">The Object to compare with the current Object.</param>
		/// <returns>true if the specified Object is equal to the current Object;
		/// otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (! (obj is IntegerList))
				return false;
			IntegerList otherList = (IntegerList)obj;
			if (otherList.Count != this.Count)
				return false;
			for (int i=0; i < this.list.Count; i++)
			{
				if (this[i] != otherList[i])
					return false;
			}
			return true;
		}

		/// <summary>
		/// Determines whether the specified Object instances are considered equal.
		/// </summary>
		/// <param name="objA">The first Object to compare.</param>
		/// <param name="objB">The second Object to compare.</param>
		/// <returns>true if objA is the same instance as objB or if both are null
		/// references or if objA.Equals(objB) returns true; otherwise, false.
		/// </returns>
		public new static bool Equals(object objA, object objB)
		{
			return objA.Equals(objB);
		}

		/// <summary>
		/// Returns an enumerator for the entire list.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator for a section of the ArrayList.
		/// </summary>
		/// <param name="index">The zero-based starting index of the ArrayList section
		/// that the enumerator should refer to.</param>
		/// <param name="count">The number of elements in the ArrayList section that
		/// the enumerator should refer to</param>
		/// <returns>The enumerator.</returns>
		public IEnumerator GetEnumerator(int index, int count)
		{
			return list.GetEnumerator(index,count);
		}

		/// <summary>
		/// Serves as a hash function.
		/// </summary>
		/// <returns>A hash code.</returns>
		public override int GetHashCode()
		{
			return (list.Count * (this[0] + this[list.Count-1]).GetHashCode());
		}

		/// <summary>
		/// Returns an integer list which represents a subset of the elements in
		/// the source list.
		/// </summary>
		/// <param name="index">The zero-based list index at which the range starts.</param>
		/// <param name="count">The number of elements in the range.</param>
		/// <returns>An integer list which represents a subset of the elements
		/// in the source list.</returns>
		public IntegerList GetRange(int index, int count)
		{
			IntegerList result = new IntegerList();
			result.list = this.list.GetRange(index,count);
			return result;
		}

		/// <summary>
		/// Searches for the specified Object and returns the zero-based index of
		/// the first occurrence within the entire list.
		/// </summary>
		/// <param name="value">The value to locate in the list.</param>
		/// <returns>The zero-based index of the first occurrence of value within the
		/// entire list, if found; otherwise, -1.</returns>
		public int IndexOf(int value)
		{
			return IndexOf(value,0,list.Count);
		}

		/// <summary>
		/// Searches for the specified Object and returns the zero-based index of the
		/// first occurrence within the section of the ArrayList that extends from the
		/// specified index to the last element.
		/// </summary>
		/// <param name="value">The value to locate in the list.</param>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <returns>The zero-based index of the first occurrence of value within the section
		/// of the ArrayList that extends from startIndex to the last element, if found;
		/// otherwise, -1.</returns>
		public int IndexOf(int value, int startIndex)
		{
			return IndexOf(value,startIndex,list.Count-startIndex);
		}

		/// <summary>
		/// Searches for the specified Object and returns the zero-based index of the
		/// first occurrence within the section of the list that starts at the specified
		/// index and contains the specified number of elements.
		/// </summary>
		/// <param name="value">The value to locate in the list.</param>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <returns>The zero-based index of the first occurrence of value within the
		/// section of the ArrayList that starts at startIndex and contains count number
		/// of elements, if found; otherwise, -1.</returns>
		public int IndexOf(int value, int startIndex, int count)
		{
			if ((startIndex < 0) || (startIndex >= list.Count))
				throw new ArgumentOutOfRangeException(); //TODO: arg
			if (count < 0)
				throw new ArgumentOutOfRangeException(); //TODO: arg
			if (startIndex + count > list.Count)
				throw new ArgumentOutOfRangeException(); //TODO: arg
			for (int i=startIndex; i < startIndex + count; i++)
			{
				if (this[i] == value)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Inserts an element into the list at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="value">The value to insert.</param>
		public void Insert(int index, int value)
		{
			list.Insert(index,value);
		}

		/// <summary>
		/// Inserts the elements of the other integer list into the list at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which the new elements should be inserted.</param>
		/// <param name="other">The other list whose elements should be inserted into the list.</param>
		public void InsertRange(int index, IntegerList other)
		{
			for (int i = 0; i < other.Count; i++)
			{
				list[index+i] = other[i];
			}
		}

		/// <summary>
		/// Searches for the specified value and returns the zero-based index of the last
		/// occurrence within the entire list.
		/// </summary>
		/// <param name="value">The value to locate in the list.</param>
		/// <returns>The zero-based index of the last occurrence of value within the entire
		/// list, if found; otherwise, -1.</returns>
		public int LastIndexOf(int value)
		{
			return LastIndexOf(value,list.Count-1,list.Count);
		}

		/// <summary>
		/// Searches for the specified value and returns the zero-based index of the last
		/// occurrence within the section of the list that extends from the first element
		/// to the specified index
		/// </summary>
		/// <param name="value">The value to search for.</param>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <returns>The zero-based index of the last occurrence of value within the section
		/// of the list that extends from the first element to startIndex, if found;
		/// otherwise, -1.</returns>
		public int LastIndexOf(int value, int startIndex)
		{
			return LastIndexOf(value,startIndex,startIndex+1);
		}

		/// <summary>
		/// Searches for the specified value and returns the zero-based index of the
		/// last occurrence within the section of the list that contains the specified
		/// number of elements and ends at the specified index.
		/// </summary>
		/// <param name="value">The value to locate in the list.</param>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <returns>The zero-based index of the last occurrence of value within the section
		/// of the list that extends from the first element to startIndex, if found;
		/// otherwise, -1.</returns>
		public int LastIndexOf(int value, int startIndex, int count)
		{
			if ((startIndex < 0) || (startIndex >= list.Count))
				throw new ArgumentOutOfRangeException(); //TODO: arg
			if (count < 0)
				throw new ArgumentOutOfRangeException(); //TODO: arg
			if (startIndex - count < -1)
				throw new ArgumentOutOfRangeException(); //TODO: arg
			for (int i=startIndex; i < startIndex - count; i--)
			{
				if (this[i] == value)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Removes the first occurrence of a specific value from the list.
		/// </summary>
		/// <param name="value">The value to remove from the list.</param>
		public void Remove(int value)
		{
			for (int i=0; i < list.Count; i++)
			{
				if (this[i] == value)
				{
					list.RemoveAt(i);
					return;
				}
			}
		}

		/// <summary>
		/// Removes the element at the specified index of the list.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		public void RemoveAt(int index)
		{
			list.RemoveAt(index);
		}

		/// <summary>
		/// Removes a range of elements from the list.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range of elements to remove.</param>
		/// <param name="count">The number of elements to remove.</param>
		public void RemoveRange(int index, int count)
		{
			list.RemoveRange(index,count);
		}

		/// <summary>
		/// Returns an IntegerList whose elements are copies of the specified value.
		/// </summary>
		/// <param name="value">The value to copy multiple times in the list.</param>
		/// <param name="count">The number of times value should be copied.</param>
		/// <returns>An IntegerList with count number of elements, all of which are copies of value.</returns>
		public static IntegerList Repeat(int value, int count)
		{
			IntegerList result = new IntegerList();
			for (int i=0; i < count; i++)
			{
				result.Add(value);
			}
			return result;
		}
			
		/// <summary>
		/// Reverses the order of the elements in the entire list.
		/// </summary>
		public void Reverse()
		{
			list.Reverse();
		}

		/// <summary>
		/// Reverses the order of the elements in the specified range.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to reverse.</param>
		/// <param name="count">The number of elements in the range to reverse.</param>
		public void Reverse(int index, int count)
		{
			list.Reverse(index,count);
		}

		/// <summary>
		/// Copies the elements of a collection over a range of elements in the list.
		/// </summary>
		/// <param name="index">The zero-based index at which to start copying the elements
		/// of otherList</param>
		/// <param name="otherList">The other list whose elements to copy to the list.</param>
		public void SetRange(int index, IntegerList otherList)
		{
			list.SetRange(index,otherList.list);
		}

		/// <summary>
		/// Sorts the elements in the entire list using the IComparable implementation
		/// of the Integer.
		/// </summary>
		public void Sort()
		{
			list.Sort();
		}

		/// <summary>
		/// Sorts the elements in the entire ArrayList using the specified comparer.
		/// </summary>
		/// <param name="comparer">The IComparer implementation to use when comparing elements.</param>
		public void Sort(IComparer comparer)
		{
			list.Sort(comparer);
		}

		/// <summary>
		/// Sorts the elements in a section of the list using the specified comparer.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to sort.</param>
		/// <param name="count">The length of the range to sort.</param>
		/// <param name="comparer">The IComparer implementation to use when comparing elements.</param>
		public void Sort(int index, int count, IComparer comparer)
		{
			list.Sort(index,count,comparer);
		}

		/// <summary>
		/// Returns an IntegerList wrapper that is synchronized.
		/// </summary>
		/// <param name="list">The list to synchronize.</param>
		/// <returns>An IntegerList wrapper that is synchronized (thread-safe).</returns>
		public static IntegerList Synchronized(IntegerList list)
		{
			IntegerList result = new IntegerList();
			result.list = ArrayList.Synchronized(list.list);
			return result;
		}

		/// <summary>
		/// Returns a String that represents the list.
		/// </summary>
		/// <returns>A String that represents the current list.</returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("[");
			for (int i=0; i < list.Count; i++)
			{
				builder.Append(this[i]);
				if (i<list.Count-1)
					builder.Append(", ");
			}
			builder.Append("]");
			return builder.ToString();
		}

		/// <summary>
		/// Sets the capacity to the actual number of elements in the list.
		/// </summary>
		public void TrimToSize()
		{
			list.TrimToSize();
		}

		/// <summary>
		/// The number of elements that the list can contain.
		/// </summary>
		public int Capacity {get{return list.Capacity;}}

		/// <summary>
		/// The number of elements actually contained in the list.
		/// </summary>
		public int Count {get{return list.Count;}}

		/// <summary>
		/// True if the list has a fixed size; otherwise false.
		/// </summary>
		public bool IsFixedSize {get{return list.IsFixedSize;}}

		/// <summary>
		/// True if the list is read-only, otherwise false.
		/// </summary>
		public bool IsReadOnly {get{return list.IsReadOnly;}}

		/// <summary>
		/// True if the list is synchronized, otherwise false.
		/// </summary>
		public bool IsSynchronized {get{return list.IsSynchronized;}}

		/// <summary>
		/// Gets a value from the list.
		/// </summary>
		public int this[int index] {get{return (Int32)list[index];}}

		/// <summary>
		/// An object that can be used to synchronize access to the list.
		/// </summary>
		public object SyncRoot {get{return list.SyncRoot;}}


	}
}
