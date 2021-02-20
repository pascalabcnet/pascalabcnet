// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace PascalABCCompiler.TreeRealization
{
	public class using_namespace
	{
		private string _namespace_name;

		public using_namespace(string namespace_name)
		{
			_namespace_name=namespace_name;
		}

		public string namespace_name
		{
			get
			{
				return _namespace_name;
			}
		}
	}
}