using System;
using com.calitha.goldparser.content;

namespace com.calitha.goldparser.lalr
{
	/// <summary>
	/// Factory class for creating Action objects..
	/// </summary>
	public sealed class ActionFactory
	{
		private ActionFactory()
		{
		}

		/// <summary>
		/// Creates a new action by specifying the needed information.
		/// </summary>
		/// <param name="record">A part of the LALR record from the file content.</param>
		/// <param name="states">The LALR states.</param>
		/// <param name="symbols">The symbols.</param>
		/// <param name="rules">The rules.</param>
		/// <returns>A new action object.</returns>
		public static Action CreateAction(ActionSubRecord record,
			                              StateCollection states,
			                              SymbolCollection symbols,
			                              RuleCollection rules)
		{
			Action action;
			switch (record.Action)
			{
				case 1: action = CreateShiftAction(record,symbols,states); break;
				case 2: action = CreateReduceAction(record,symbols,rules); break;
				case 3: action = CreateGotoAction(record,symbols,states); break;
				case 4: action = CreateAcceptAction(record,symbols); break;
				default: return null; //todo: make exception
			}
			return action;
		}

		private static ShiftAction CreateShiftAction(ActionSubRecord record,
													 SymbolCollection symbols,
			                                         StateCollection states
			                                         )
		{
			State state = states[record.Target];
			SymbolTerminal symbol = symbols[record.SymbolIndex] as SymbolTerminal;
			//todo: exception symbol type
			return new ShiftAction(symbol,state);
		}

		private static ReduceAction CreateReduceAction(ActionSubRecord record,
			                                           SymbolCollection symbols,
			                                           RuleCollection rules)
		{
			SymbolTerminal symbol = symbols[record.SymbolIndex] as SymbolTerminal;
			Rule rule = rules[record.Target];
			return new ReduceAction(symbol,rule);
		}

		private static GotoAction CreateGotoAction(ActionSubRecord record,
			                                       SymbolCollection symbols,
			                                       StateCollection states)
		{
			SymbolNonterminal symbol = symbols[record.SymbolIndex] as SymbolNonterminal;
			State state = states[record.Target];
			return new GotoAction(symbol,state);
		}

		private static AcceptAction CreateAcceptAction(ActionSubRecord record,
			                                           SymbolCollection symbols)
		{
			SymbolTerminal symbol = symbols[record.SymbolIndex] as SymbolTerminal;
			return new AcceptAction(symbol);
		}

	}
}
