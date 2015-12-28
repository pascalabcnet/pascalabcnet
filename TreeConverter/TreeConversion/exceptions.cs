// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Класс, представляющий внутреннюю ошибку компилятора.
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