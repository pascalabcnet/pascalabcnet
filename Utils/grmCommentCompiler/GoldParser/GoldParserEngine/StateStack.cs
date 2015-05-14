using System;
using System.Collections;
using com.calitha.goldparser.lalr;


namespace com.calitha.goldparser
{
	
	
	/// <summary>
	/// The stack of LALR states for the parser. It is used to store the current and
	/// previous states, which is usefull if a reduction occurs.
	/// </summary>
	public class StateStack
	{
		protected Stack stack;
		
		/// <summary>
		/// Creates a new stack.
		/// </summary>
		public StateStack()
		{
			stack = new Stack();
		}
		
		/// <summary>
		/// Clears the entire stack.
		/// </summary>
		public virtual void Clear()
		{
			stack.Clear();
		}
		
		/// <summary>
		/// Peeks to see the state on the top of the stack.
		/// </summary>
		/// <returns></returns>
		public virtual State Peek()
		{
			return (State)stack.Peek();
		}
		
		/// <summary>
		/// Pops the state on the top from the stack.
		/// </summary>
		/// <returns></returns>
		public virtual State Pop()
		{
			return (State)stack.Pop();
		}
			
		/// <summary>
		/// Pushes a state on top of the stack.
		/// </summary>
		/// <param name="state"></param>
		public virtual void Push(State state)
		{
			stack.Push(state);
		}
		
		/// <summary>
		/// The number of states on the stack.
		/// </summary>
		public virtual int Count {get {return stack.Count;}}

			
	}
}
