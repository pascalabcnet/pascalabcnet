using System;
using System.Runtime.Serialization;

namespace com.calitha.goldparser
{

	/// <summary>
	/// ParserException is throws when an unexpected situation occurs while parsing.
	/// For example if the LALR parser is in a state where no actions can be taken.
	/// </summary>
	[Serializable()]
	public class ParserException : System.ApplicationException
	{
		public ParserException(string message) : base(message)
		{
		}

		public ParserException(string message,
			                   Exception inner) : base(message, inner)
		{
		}

		protected ParserException(SerializationInfo info,
			                      StreamingContext context) : base(info, context)
		{
		}

	}

}
