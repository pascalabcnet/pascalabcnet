using System;

using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{

    //TODO: Переделать с использованием коллекций.
	///<summary>
	///Collection of unit_node
	///</summary>
	[Serializable]
	public class unit_nodeArrayList : System.Collections.IEnumerable,System.Collections.ICollection,System.ICloneable
	{
	
		private System.Collections.ArrayList arr=new System.Collections.ArrayList();
	
		///<summary>
		///Adds an unit_node to the end of the unit_nodeArrayList.
		///</summary>
		///<param name="value">The unit_node to be added to the end of the ArrayList. The value can be a null reference.<param>
		///<returns>The unit_nodeArrayList index at which the value has been added.</returns>
		public int Add(unit_node value)
		{
			return arr.Add(value);
		}
		
		///<summary>
		///Adds the elements of an ICollection to the end of the unit_nodeArrayList
		///</summary>
		///<param name="c">The ICollection whose elements should be added to the end of the ArrayList. The collection itself cannot be a null reference (Nothing in Visual Basic), but it can contain elements that are a null reference.<param>
		///<returns>Return value is void</returns>
		private void AddRange(System.Collections.ICollection c)
		{
			arr.AddRange(c);
		}
		
		///<summary>
		///Adds the elements of an unit_node[] to the end of the unit_nodeArrayList
		///</summary>
		///<param name="array">The unit_node[] whose elements should be added to the end of the ArrayList. The unit_node[] itself cannot be a null reference, but it can contain elements that are a null reference.<param>
		///<returns>Return value is void</returns>
		public void AddRange(unit_node[] array)
		{
			arr.AddRange(array);
		}
		
		///<summary>
		///Adds the elements of an unit_nodeArrayList to the end of the unit_nodeArrayList
		///</summary>
		///<param name="array_list">The unit_nodeArrayList whose elements should be added to the end of the ArrayList. The unit_nodeArrayList itself cannot be a null reference, but it can contain elements that are a null reference.<param>
		///<returns>Return value is void</returns>
		public void AddRange(unit_nodeArrayList array_list)
		{
			arr.AddRange(array_list);
		}
		
		///<summary>
		///Uses a binary search algorithm to locate a specific element in the sorted unit_nodeArrayList or a portion of it.
		///</summary>
		///<param name="value">The Object to locate. The value can be a null reference.<param>
		///<returns>The zero-based index of value in the sorted unit_nodeArrayList, if value is found; otherwise, a negative number, which is the bitwise complement of the index of the next element that is larger than value or, if there is no larger element, the bitwise complement of Count.</returns>
		public int BinarySearch(unit_node value)
		{
			return arr.BinarySearch(value);
		}
		
		///<summary>
		///Searches the entire sorted ArrayList for an element using the specified comparer and returns the zero-based index of the element.
		///</summary>
		///<param name="value">The Object to locate. The value can be a null reference.<param>
		///<param name="comparer">The IComparer implementation to use when comparing elements. -or- A null reference (Nothing in Visual Basic) to use the default comparer that is the IComparable implementation of each element.<param>
		///<returns>The zero-based index of value in the sorted unit_nodeArrayList, if value is found; otherwise, a negative number, which is the bitwise complement of the index of the next element that is larger than value or, if there is no larger element, the bitwise complement of Count.</returns>
		public void BinarySearch(unit_node value,System.Collections.IComparer comparer)
		{
			arr.BinarySearch(value,comparer);
		}
		
		///<summary>
		///Searches the entire sorted ArrayList for an element using the specified comparer and returns the zero-based index of the element.
		///</summary>
		///<param name="index">The zero-based starting index of the range to search.<param>
		///<param name="count">The length of the range to search.<param>
		///<param name="value">The Object to locate. The value can be a null reference.<param>
		///<param name="comparer">The IComparer implementation to use when comparing elements. -or- A null reference (Nothing in Visual Basic) to use the default comparer that is the IComparable implementation of each element.<param>
		///<returns>The zero-based index of value in the sorted unit_nodeArrayList, if value is found; otherwise, a negative number, which is the bitwise complement of the index of the next element that is larger than value or, if there is no larger element, the bitwise complement of Count.</returns>
		public void BinarySearch(int index,int count,unit_node value,System.Collections.IComparer comparer)
		{
			arr.BinarySearch(value,comparer);
		}
		
		///<summary>
		///Creates a shallow copy of the ArrayList.
		///</summary>
		///<returns>A shallow copy of the unit_nodeArrayList.</returns>
		public virtual unit_nodeArrayList clone()
		{
			unit_nodeArrayList al=new unit_nodeArrayList();
			al.arr=(System.Collections.ArrayList)this.arr.Clone();
			return al;
		}
		
		///<summary>
		///Creates a shallow copy of the ArrayList.
		///</summary>
		///<returns>A shallow copy of the unit_nodeArrayList.</returns>
		public virtual object Clone()
		{
			return this.clone();
		}
		
		///<summary>
		///Determines whether an element is in the unit_nodeArrayList.
		///</summary>
		///<param name="item">The unit_node to locate in the ArrayList. The value can be a null reference.<param>
		///<returns>true if item is found in the unit_nodeArrayList; otherwise, false.</returns>
		public bool Contains(unit_node item)
		{
			return arr.Contains(item);
		}
		
		///<summary>
		///Searches for the specified unit_node and returns the zero-based index of the first occurrence within the entire unit_nodeArrayList.
		///</summary>
		///<param name="value">The unit_node to locate in the unit_nodeArrayList. The value can be a null reference.<param>
		///<returns>The zero-based index of the first occurrence of value within the entire unit_nodeArrayList, if found; otherwise, -1.</returns>
		public int IndexOf(unit_node value)
		{
			return arr.IndexOf(value);
		}
		
		///<summary>
		///Searches for the specified unit_node and returns the zero-based index of the first occurrence within the section of the unit_nodeArrayList that extends from the specified index to the last element.
		///</summary>
		///<param name="value">The unit_node to locate in the unit_nodeArrayList. The value can be a null reference.<param>
		///<param name="startIndex">The zero-based starting index of the search.<param>
		///<returns>The zero-based index of the first occurrence of value within the section of the unit_nodeArrayList that extends from startIndex to the last element, if found; otherwise, -1.</returns>
		public int IndexOf(unit_node value,int startIndex)
		{
			return arr.IndexOf(value,startIndex);
		}
		
		///<summary>
		///Searches for the specified unit_node and returns the zero-based index of the first occurrence within the section of the unit_nodeArrayList that starts at the specified index and contains the specified number of elements.
		///</summary>
		///<param name="value">The unit_node to locate in the unit_nodeArrayList. The value can be a null reference.<param>
		///<param name="startIndex">The zero-based starting index of the search.<param>
		///<param name="count">The number of elements in the section to search.<param>
		///<returns>The zero-based index of the first occurrence of value within the section of the ArrayList that starts at startIndex and contains count number of elements, if found; otherwise, -1.</returns>
		public int IndexOf(unit_node value,int startIndex,int count)
		{
			return arr.IndexOf(value,startIndex,count);
		}
		
		///<summary>
		///Inserts an element into the unit_nodeArrayList at the specified index.
		///</summary>
		///<param name="index">The zero-based index at which value should be inserted.<param>
		///<param name="value">The unit_node to insert. The value can be a null reference.<param>
		///<returns>Return value is void</returns>
		public void Insert(int index,unit_node value)
		{
			arr.Insert(index,value);
		}
		
		///<summary>
		///Inserts the elements of a unit_node into the unit_nodeArrayList at the specified index.
		///</summary>
		///<param name="index">The zero-based index at which the new elements should be inserted.<param>
		///<param name="arr">The unit_node[] whose elements should be inserted into the unit_nodeArrayList. The unit_node[] itself cannot be a null reference, but it can contain elements that are a null reference.<param>
		///<returns>Return value is void</returns>
		public void InsertRange(int index,unit_node[] unit_node_arr)
		{
			arr.InsertRange(index,arr);
		}
		
		///<summary>
		///Inserts the elements of a collection into the unit_nodeArrayList at the specified index.
		///</summary>
		///<param name="index">The zero-based index at which the new elements should be inserted.<param>
		///<param name="array_list">The unit_nodeArrayList whose elements should be inserted into the unit_nodeArrayList. The unit_nodeArrayList itself cannot be a null reference, but it can contain elements that are a null reference.<param>
		///<returns>Return value is void</returns>
		public void InsertRange(int index,unit_nodeArrayList array_list)
		{
			arr.InsertRange(index,array_list);
		}
		
		///<summary>
		///Inserts the elements of a collection into the unit_nodeArrayList at the specified index.
		///</summary>
		///<param name="index">The zero-based index at which the new elements should be inserted.<param>
		///<param name="value">The ICollection whose elements should be inserted into the unit_nodeArrayList. The collection itself cannot be a null reference, but it can contain elements that are a null reference.<param>
		///<returns>Return value is void</returns>
		private void InsertRange(int index,System.Collections.ICollection c)
		{
			arr.InsertRange(index,c);
		}
		
		///<summary>
		///Searches for the specified unit_node and returns the zero-based index of the last occurrence within the entire unit_nodeArrayList.
		///</summary>
		///<param name="value">The unit_node to locate in the unit_nodeArrayList. The value can be a null reference.<param>
		///<returns>The zero-based index of the last occurrence of value within the entire the ArrayList, if found; otherwise, -1.</returns>
		public int LastIndexOf(unit_node value)
		{
			return arr.LastIndexOf(value);
		}
		
		///<summary>
		///Searches for the specified unit_node and returns the zero-based index of the last occurrence within the section of the unit_nodeArrayList that extends from the first element to the specified index.
		///</summary>
		///<param name="value">The unit_node to locate in the unit_nodeArrayList. The value can be a null reference.<param>
		///<param name="startIndex">The zero-based starting index of the backward search.<param>
		///<returns>The zero-based index of the last occurrence of value within the section of the unit_nodeArrayList that extends from the first element to startIndex, if found; otherwise, -1.</returns>
		public int LastIndexOf(unit_node value,int startIndex)
		{
			return arr.LastIndexOf(value,startIndex);
		}
		
		///<summary>
		///Searches for the specified unit_node and returns the zero-based index of the last occurrence within the section of the unit_nodeArrayList that contains the specified number of elements and ends at the specified index.
		///</summary>
		///<param name="value">The unit_node to locate in the unit_nodeArrayList. The value can be a null reference.<param>
		///<param name="startIndex">The zero-based starting index of the backward search.<param>
		///<param name="count">The number of elements in the section to search.<param>
		///<returns>The zero-based index of the last occurrence of value within the section of the unit_nodeArrayList that extends from the first element to startIndex, if found; otherwise, -1.</returns>
		public int LastIndexOf(unit_node value,int startIndex,int count)
		{
			return arr.LastIndexOf(value,startIndex,count);
		}
		
		///<summary>
		///Gets or sets the element at the specified index.
		///</summary>
		///<param name="index">The zero-based index of the element to get or set.<param>
		public unit_node this [int index]
		{
			get
			{
				return ((unit_node)(arr[index]));
			}
			set
			{
				arr[index]=value;
			}
		}
		
		///<summary>
		///Removes the first occurrence of a specific object from the ArrayList.
		///</summary>
		///<param name="value">The unit_node to remove from the ArrayList. The value can be a null reference.<param>
		///<returns>Return value is void</returns>
		public void Remove(unit_node value)
		{
			arr.Remove(value);
		}
		
		///<summary>
		///Copies the elements of a collection over a range of elements in the unit_nodeArrayList.
		///</summary>
		///<param name="index">The zero-based unit_nodeArrayList index at which to start copying the elements of c.<param>
		///<param name="c">The ICollection whose elements to copy to the ArrayList. The collection itself cannot be a null reference (Nothing in Visual Basic), but it can contain elements that are a null reference.<param>
		///<returns>Return value is void</returns>
		private void SetRange(int index,System.Collections.ICollection c)
		{
			arr.SetRange(index,c);
		}
		
		///<summary>
		///Copies the elements of the unit_nodeArrayList to a new unit_node array.
		///</summary>
		///<returns>An unit_node array containing copies of the elements of the unit_nodeArrayList.</returns>
		public unit_node[] ToArray()
		{
			return ((unit_node[])(arr.ToArray(typeof(unit_node))));
		}
		
		///<summary>
		///Gets the number of elements actually contained in the ArrayList.
		///</summary>
		public int Count
		{
			get
			{
				return arr.Count;
			}
		}

		///<summary>
		///Copies the entire ArrayList to a compatible one-dimensional Array, starting at the specified index of the target array.
		///</summary>
		///<param name="array">The one-dimensional Array that is the destination of the elements copied from unit_nodeArrayList. The Array must have zero-based indexing.<param>
		///<param name="arrayIndex">The zero-based index in array at which copying begins.<param>
		///<returns>Return value is void</returns>
		public void CopyTo(System.Array array,int arrayIndex)
		{
			arr.CopyTo(array,arrayIndex);
		}
		
		///<summary>
		///Copies the entire ArrayList to a compatible one-dimensional unit_node[], starting at the specified index of the target array.
		///</summary>
		///<param name="array">The one-dimensional unit_node[] that is the destination of the elements copied from unit_nodeArrayList. The Array must have zero-based indexing.<param>
		///<param name="arrayIndex">The zero-based index in array at which copying begins.<param>
		///<returns>Return value is void</returns>
		public void CopyTo(unit_node[] array,int arrayIndex)
		{
			arr.CopyTo(array,arrayIndex);
		}

		///<summary>
		///Copies the entire unit_nodeArrayList to a compatible one-dimensional unit_node[], starting at the beginning of the target array.
		///</summary>
		///<param name="array">The one-dimensional unit_node[] that is the destination of the elements copied from unit_nodeArrayList. The Array must have zero-based indexing.<param>
		///<returns>Return value is void</returns>
		public void CopyTo(unit_node[] array)
		{
			arr.CopyTo(array);
		}

		///<summary>
		///Copies the entire ArrayList to a compatible one-dimensional unit_node[], starting at the specified index of the target array.
		///</summary>
		///<param name="index">The zero-based index in the source ArrayList at which copying begins.<param>
		///<param name="array">The one-dimensional unit_node[] that is the destination of the elements copied from unit_nodeArrayList. The Array must have zero-based indexing.<param>
		///<param name="arrayIndex">The zero-based index in array at which copying begins.<param>
		///<param name="count">The number of elements to copy.<param>
		///<returns>Return value is void</returns>
		public void CopyTo(int index,unit_node[] array,int arrayIndex,int count)
		{
			arr.CopyTo(index,array,arrayIndex,count);
		}

		///<summary>
		///Gets a value indicating whether access to the ArrayList is synchronized (thread-safe).
		///</summary>
		public bool IsSynchronized
		{
			get
			{
				return arr.IsSynchronized;
			}
		}

		///<summary>
		///Gets a value indicating whether the ArrayList has a fixed size.
		///</summary>
		public bool IsFixedSize
		{
			get
			{
				return arr.IsFixedSize;
			}
		}

		///<summary>
		///Gets a value indicating whether the ArrayList is read-only.
		///</summary>
		public bool IsReadOnly
		{
			get
			{
				return arr.IsReadOnly;
			}
		}

		///<summary>
		///Gets an object that can be used to synchronize access to the ArrayList.
		///</summary>
		public object SyncRoot
		{
			get
			{
				return arr.SyncRoot;
			}
		}
	
		///<summary>
		///Returns an enumerator for the entire ArrayList.
		///</summary>
		///<returns>An IEnumerator for the entire unit_nodeArrayList.</returns>
		public System.Collections.IEnumerator GetEnumerator()
		{
			return arr.GetEnumerator();
		}

		///<summary>
		///Removes all elements from the ArrayList.
		///</summary>
		///<returns>Return value is void</returns>
		public void Clear()
		{
			arr.Clear();
		}
		
		public int Capacity
		{
			get
			{
				return arr.Capacity;
			}
			set
			{
				arr.Capacity=value;
			}
		}
		
		public unit_nodeArrayList GetRange(int index,int count)
		{
			System.Collections.ArrayList al=arr.GetRange(index,count);
			unit_nodeArrayList tnal=new unit_nodeArrayList();
			tnal.arr=al;
			return tnal;
		}
		
		public void RemoveAt(int index)
		{
			arr.RemoveAt(index);
		}
		
		public void RemoveRange(int index,int count)
		{
			arr.RemoveRange(index,count);
		}
		
		public void Reverse()
		{
			arr.Reverse();
		}
		
		public void Reverse(int index,int count)
		{
			arr.Reverse(index,count);
		}
		
		public void SetRange(int index,unit_node[] tnarr)
		{
			SetRange(index,tnarr);
		}
		
		public void SetRange(int index,unit_nodeArrayList tnarl)
		{
			SetRange(index,tnarl);
		}
		
		public void TrimToSize()
		{
			arr.TrimToSize();
		}
	
	}

}