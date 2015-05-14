using System;

namespace com.calitha.goldparser.dfa
{
	/// <summary>
	/// The interface for the Deterministic Finite Automata
	/// </summary>
	public interface IDFA
	{
		/// <summary>
		/// Sets the DFA back to the starting state, so it can be used to get a new token.
		/// </summary>
		void Reset();

		/// <summary>
		/// Goto the next state depending on an input character.
		/// </summary>
		/// <param name="ch">The character that determines what state to go to next.</param>
		/// <returns>The new current state.</returns>
		State GotoNext(char ch);

		/// <summary>
		/// The current state in the DFA.
		/// </summary>
		State CurrentState {get;}
	}
	
	/// <summary>
	/// Implementation of a Deterministic Finite Automata.
	/// </summary>
	public class DFA : IDFA
	{
		private StateCollection states;
		private State startState;
		private State currentState;

		/// <summary>
		/// Creates a new DFA.
		/// </summary>
		/// <param name="states">The states that are part of the DFA.</param>
		/// <param name="startState">The starting state</param>
		public DFA(StateCollection states, State startState)
		{
			this.states       = states;
			this.startState   = startState;
			this.currentState = startState;
		}
		
		/// <summary>
		/// Sets the DFA back to the starting state, so it can be used to get a new token.
		/// </summary>
		public void Reset()
		{
			this.currentState = startState;
		}
		
		/// <summary>
		/// Goto the next state depending on an input character.
		/// </summary>
		/// <param name="ch">The character that determines what state to go to next.</param>
		/// <returns>The new current state.</returns>
		public State GotoNext(char ch)
		{
			Transition transition = currentState.Transitions.Find(ch);
			if (transition != null)
			{
				currentState = transition.Target;
				return currentState;
			}
			else
				return null;
		}
			
		/// <summary>
		/// The current state in the DFA.
		/// </summary>
		public State CurrentState {get {return currentState;}}
	}
}
