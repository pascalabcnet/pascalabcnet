using System;
using System.Collections;
using System.Collections.Specialized;
using com.calitha.commons;

namespace com.calitha.goldparser.dfa
{
	/// <summary>
	/// A transition (edge) between DFA states. The source and target state can be the same.
	/// </summary>
	public class Transition
	{
		private State target;
		private ISet charset;

		/// <summary>
		/// Creates a new transition by specifying the target state and the criteria for
		/// taking a transition to another state. The source state does not need to be
		/// specified, because the state itself knows its transition.
		/// </summary>
		/// <param name="target">The target state.</param>
		/// <param name="characters">The character set criteria.</param>
		public Transition(State target, string characters)
		{
			this.target = target;
			if (characters.Length > 10)
				this.charset = new HashSet();
			else
				this.charset = new ArraySet();
			char[] ca = characters.ToCharArray();
			foreach (Char ch in ca)
			{
				this.charset.Add(ch);
			}
		}

		/// <summary>
		/// The target state.
		/// </summary>
		public State Target {get {return target;}}

		/// <summary>
		/// The criteria for going to the target state.
		/// </summary>
		public ISet CharSet {get {return charset;}}
	}
	
	/// <summary>
	/// A type-safe list of transitions.
	/// </summary>
	public class TransitionCollection : IEnumerable
	{	
		private IList list;
		private IDictionary map;

		public TransitionCollection()
		{
			list = new ArrayList();
			map = new HybridDictionary();
		}
		
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public int Add(Transition transition)
		{
			IEnumerator enumerator = transition.CharSet.GetEnumerator();
			while (enumerator.MoveNext())
			{
				char ch = (char)enumerator.Current;
				map.Add(ch, transition);
			}
			return list.Add(transition);
		}
	
		public Transition Find(char ch)
		{
			return map[ch] as Transition;
		}

	}
	
}
