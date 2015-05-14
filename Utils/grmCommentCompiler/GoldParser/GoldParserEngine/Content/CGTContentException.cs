using System;
using System.Runtime.Serialization;

namespace com.calitha.goldparser.content
{

	/// <summary>
	/// This exception is thrown if something is wrong with the cgt content.
	/// For example if there are not enough entries in a record.
	/// </summary>
	[Serializable()]
	public class CGTContentException : System.IO.IOException
	{
		public CGTContentException(string message) : base(message)
		{
		}

		public CGTContentException(string message,
			Exception inner) : base(message, inner)
		{
		}

		protected CGTContentException(SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}

	}
}
