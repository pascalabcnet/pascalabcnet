// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;

namespace PascalABCCompiler
{
	public static class StringConstants
	{
		#region PASCAL LANGUAGE 
		public const string pascalLanguageName = "PascalABCNET";
		public const string pascalSourceFileExtension = ".pas";
		public const string pascalCompiledUnitExtension = ".pcu";
		public const string PABCSystemName = "PABCSystem";
		public const string PABCExtensionsName = "PABCExtensions";
		public static readonly string[] pascalDefaultStandardModules = new string[]
		{
			PABCSystemName,
			PABCExtensionsName
		};
		#endregion

	}
}