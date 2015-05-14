using System;
using System.Text;
using System.Collections;
using com.calitha.commons;
using com.calitha.goldparser.content;

namespace com.calitha.goldparser.structure
{
	/// <summary>
	/// EntryCollection is a type-safe list of Entry items.
	/// </summary>
	public class EntryCollection : IEnumerable
	{
		private IList list;

		public EntryCollection()
		{
			list = new ArrayList();
		}
		
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
		
		public int Add(Entry entry)
		{
			return list.Add(entry);
		}

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			foreach (Entry entry in this)
			{
				str.Append(entry.ToString());
				str.Append("\n");
			}
			return str.ToString();
		}
		
		public Entry Get(int index)
		{
			if (index < 0 || index >= list.Count)
				return null;
			else
				return list[index] as Entry;
		}

		public Entry this[int index]
		{
			get
			{
				return Get(index);
			}
		}
		
		public int Count { get{return list.Count;} }
	}

	/// <summary>
	/// Each record in the file structure contains one or more of these Entry objects.
	/// 
	/// </summary>
	abstract public class Entry
	{
		public byte ToByteValue()
		{
			ByteEntry entry = this as ByteEntry;
			if (entry == null)
				throw new CGTContentException("Entry is not a byte");
			return entry.Value;
		}

		public bool ToBoolValue()
		{
			BooleanEntry entry = this as BooleanEntry;
			if (entry == null)
				throw new CGTContentException("Entry is not a boolean");
			return entry.Value;
		}

		public int ToIntValue()
		{
			IntegerEntry entry = this as IntegerEntry;
			if (entry == null)
				throw new CGTContentException("Entry is not an integer");
			return entry.Value;
		}
	
		public string ToStringValue()
		{
			StringEntry entry = this as StringEntry;
			if (entry == null)
				throw new CGTContentException("Entry is not a string");
			return entry.Value;
		}
	}
	
	public class EmptyEntry : Entry
	{
	    public EmptyEntry()
	    {
	    }
    
        public override string ToString()
        {
            return "Empty";
         }
    }
	
	public class ByteEntry : Entry
	{
	    private byte value;
	    
	    public ByteEntry(CalithaBinReader reader)
	    {
	        value = reader.ReadByte();
	    }

        public override string ToString()
        {
            return (String.Format("{0}: {1}", this.GetType(), value));
        }   

        public byte Value {get {return value;}}
	    
	}
	
	public class BooleanEntry : Entry
	{
	    private bool value;
	    
	    public BooleanEntry(CalithaBinReader reader)
	    {
	        value = reader.ReadBoolean();
	    }
	    
        public override string ToString()
        {
            return (String.Format("{0}: {1}", this.GetType(), value));
        }   
        
        public bool Value {get {return value;}}
	}
	
	public class IntegerEntry : Entry
	{
	    private short value;
	    
	    public IntegerEntry(CalithaBinReader reader)
	    {
	        value = reader.ReadInt16();
	    }
	    
        public override string ToString()
        {
            return (String.Format("{0}: {1}", this.GetType(), value));
        }   
        
        public short Value {get {return value;}}
	}
	
	public class StringEntry : Entry
	{
	    private string value;
	    
	    public StringEntry(CalithaBinReader reader)
	    {
	        value = reader.ReadUnicodeString();
	    }
	    
        public override string ToString()
        {
            return (String.Format("{0}: {1}", this.GetType(), value));
        }   
        
        public string Value {get {return value;}}
	}
}
