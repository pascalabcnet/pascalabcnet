// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

namespace PascalABCCompiler
{
	public static class CompilerStringConstants
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

		#region COMPILER DIRECTIVES
		public const string version_string = "version";
		public const string product_string = "product";
		public const string company_string = "company";
		public const string copyright_string = "copyright";
		public const string trademark_string = "trademark";
		public const string main_resource_string = "mainresource";
		public const string title_string = "title";
		public const string description_string = "description"; 
		#endregion
	}
}