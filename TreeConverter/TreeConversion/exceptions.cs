// ласс, представл€ющий внутреннюю ошибку компил€тора.
using System;

namespace PascalABCCompiler.TreeConverter
{

	public class CompilerInternalError : System.Exception
	{

		private string error_message="";

		public CompilerInternalError()
		{
            
		}

		public CompilerInternalError(string message)
		{
			error_message=message;
		}

		public override string ToString()
		{
			return ("Compile internal error: "+error_message);
		}
	}

}