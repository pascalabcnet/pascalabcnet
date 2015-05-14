using System;
using System.Text;
using System.Collections;
using com.calitha.commons;

namespace com.calitha.goldparser
{
	/// <summary>
	/// Abstract symbol implementation.
	/// </summary>
	public abstract class Symbol
	{
		private int id;
		private string name;

		protected Symbol(int id, string name)
		{
			this.id = id;
			this.name = name;
		}

		public override bool Equals(Object obj)
		{
			TripleState result = Util.EqualsNoState(this, obj);
			if (result == TripleState.TRUE)
				return true;
			if (result == TripleState.FALSE)
				return false;
			else
			{
				Symbol other = (Symbol)obj;
				return (this.id == other.id);
			}
		}

		public override int GetHashCode()
		{
			return id;
		}
		
		public override String ToString()
		{
			return name;
		}

		public int Id {get {return id;}}
		public string Name {get {return name;}}
	}
	
	/// <summary>
	/// SymbolNonterminal is for symbols that are not directly linked to one token.
	/// </summary>
	public class SymbolNonterminal : Symbol
	{
		public SymbolNonterminal(int id, string name) : base(id,name)
		{
		}

		public override String ToString()
		{
			return "<"+base.ToString()+">";
		}
	}
	
	/// <summary>
	/// SymbolTerminal is a symbol that is linked to a token.
	/// </summary>
	public class SymbolTerminal : Symbol
	{
		public SymbolTerminal(int id, string name) : base(id,name)
		{
		}
	}
	
	/// <summary>
	/// SymbolWhiteSpace is the symbol of white-space tokens.
	/// </summary>
	public class SymbolWhiteSpace : SymbolTerminal
	{
		public SymbolWhiteSpace(int id) : base(id,"(Whitespace)")
		{
		}
	}
	
	/// <summary>
	/// SymbolEnd is the symbol for the end-of-file token.
	/// </summary>
	public class SymbolEnd : SymbolTerminal
	{
		public SymbolEnd(int id) : base(id,"(EOF)")
		{}
	}

	/// <summary>
	/// SymbolCommentStart is the symbol for the comment start token.
	/// </summary>
	public class SymbolCommentStart : SymbolTerminal
	{
		public SymbolCommentStart(int id) : base(id,"(Comment Start)")
		{}
	}

	/// <summary>
	/// SymbolCommentEnd is the symbol for the comment end token.
	/// </summary>
	public class SymbolCommentEnd : SymbolTerminal
	{
		public SymbolCommentEnd(int id) : base(id,"(Comment End)")
		{}
	}

	/// <summary>
	/// SymbolCommentLine is the symbol for the comment line token.
	/// </summary>
	public class SymbolCommentLine : SymbolTerminal
	{
		public SymbolCommentLine(int id) : base(id,"(Comment Line)")
		{}
	}

	/// <summary>
	/// SymbolError is the symbol for the error token.
	/// </summary>
	public class SymbolError : SymbolTerminal
	{
		public SymbolError(int id) : base(id,"(ERROR)")
		{
		}
	}

	/// <summary>
	/// Type-safe list for Symbol objects.
	/// The class contains constant symbol objects for the pre-defined terminal symbols.
	/// </summary>
	public class SymbolCollection : IEnumerable
	{
		static public SymbolEnd EOF = new SymbolEnd(0);
		static public SymbolError ERROR = new SymbolError(1);

		protected IList list;
	
		public SymbolCollection()
		{
			list = new ArrayList();
		}
	
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public void Add(Symbol symbol)
		{
			list.Add(symbol);
		}	

		public Symbol Get(int index)
		{
			return list[index] as Symbol;
		}

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			foreach(Symbol symbol in this)
			{
				str.Append(symbol.ToString());
				str.Append(" ");
			}
			if (str.Length > 0)
				str.Remove(str.Length-1,1);
			return str.ToString();
		}


		public Symbol this[int index]
		{
			get {return Get(index);}
		}

	}

}
