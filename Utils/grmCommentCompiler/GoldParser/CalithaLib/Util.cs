using System;

namespace com.calitha.commons
{
	public enum TripleState {FALSE, TRUE, UNKNOWN};

	public sealed class Util
	{
		private Util()
		{
		}

		public static TripleState EqualsNoState(Object first, Object second)
		{
			if (first == second) return TripleState.TRUE;
			if (first == null) return TripleState.FALSE;
			if (second == null) return TripleState.FALSE;
			if (first.GetType().Equals(second.GetType()))
				return TripleState.UNKNOWN;
			else
				return TripleState.FALSE;
		}

	}
}
