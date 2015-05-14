using System;
using System.Collections;

namespace com.calitha.goldparser
{

	/// <summary>
	/// Type-safe list of Rule objects.
	/// </summary>
	public class RuleCollection : IEnumerable
	{
		private IList list;

		public RuleCollection()
		{
			list = new ArrayList();
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public void Add(Rule rule)
		{
			list.Add(rule);
		}

		public Rule Get(int index)
		{
			return list[index] as Rule;
		}

		public Rule this[int index]
		{
			get {return Get(index);}
		}
	}


	/// <summary>
	/// The Rule consists of the symbols that can be reduced to another symbol.
	/// </summary>
	public class Rule
	{
		private int id;
		private SymbolNonterminal lhs;
		private Symbol[] rhs;

		/// <summary>
		/// Creates a new rule.
		/// </summary>
		/// <param name="id">Id of this rule.</param>
		/// <param name="lhs">Left hand side. The other symbols can be reduced to
		/// this symbol.</param>
		/// <param name="rhs">The right hand side. The symbols that can be reduced.</param>
		public Rule(int id, SymbolNonterminal lhs, Symbol[] rhs)
		{
			this.id = id;
			this.lhs = lhs;
			this.rhs = rhs;
		}

		/// <summary>
		/// String representation of the rule.
		/// </summary>
		/// <returns>The string.</returns>
		public override String ToString()
		{
			String str = lhs+" ::= ";
			for (int i=0; i < rhs.Length; i++)
			{
				str += rhs[i]+" ";
			}
			return str.Substring(0,str.Length-1);
		}

		/// <summary>
		/// Id of this rule.
		/// </summary>
		public int Id {get{return id;}}

		/// <summary>
		/// Left hand side. The other symbols can be reduced to
		/// this symbol.
		/// </summary>
		public SymbolNonterminal Lhs {get{return lhs;}}

		/// <summary>
		/// Right hand side. The symbols that can be reduced.
		/// </summary>
		public Symbol[] Rhs {get{return rhs;}}
	}
}
