using System;
using System.Collections;

namespace com.calitha.goldparser
{		 
	/// <summary>
	/// Stack of tokens and reductions.
	/// </summary>
	public class TokenStack
	{
		protected Stack stack;
		
		/// <summary>
		/// Creates a new empty stack.
		/// </summary>
		public TokenStack()
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
		/// Peeks to see the top of the stack.
		/// </summary>
		/// <returns>The top of the stack.</returns>
		public virtual Token Peek()
		{
			return (Token)stack.Peek();
		}
		
		/// <summary>
		/// Pops the top item from the stack.
		/// </summary>
		/// <returns>The top of the stack.</returns>
		public virtual Token Pop()
		{
			return (Token)stack.Pop();
		}
			
		/// <summary>
		/// Pushes an item on the stack.
		/// </summary>
		/// <param name="token">The items that will be pushed.</param>
		public virtual void Push(Token token)
		{
			stack.Push(token);
		}
		
		/// <summary>
		/// The number of items on the stack.
		/// </summary>
		public virtual int Count {get {return stack.Count;}}

			
	}	 
}
