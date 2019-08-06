// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
namespace PascalABCCompiler
{

	public class FileNotFound : TreeConverter.CompilationErrorWithLocation
	{
		private string _file_name;

		public FileNotFound(string file_name, TreeRealization.location loc):base(loc)
		{
			_file_name=file_name;
		}

		public string file_name
		{
			get
			{
				return _file_name;
			}
		}

		public override string ToString()
		{
			return string.Format(StringResources.Get("COMPILATIONERROR_FILE_{0}_NOT_FOUND"), _file_name);
        }

	}

	public class DLLReadingError : TreeConverter.CompilationError
	{
		private string _dll_name;

		public DLLReadingError(string dll_name)
		{
			_dll_name=dll_name;
		}

		public string dll_name
		{
			get
			{
				return _dll_name;
			}
		}

		public override string ToString()
		{
			//return ("Dll: "+dll_name+" not found");
			return string.Format(StringResources.Get("COMPILATIONERROR_ASSEMBLY_{0}_READING_ERROR"), dll_name);
		}

	}

	public class InvalidUnit : TreeConverter.CompilationError
	{
		private string _unit_name;

		public InvalidUnit(string unit_name)
		{
			_unit_name=unit_name;
		}

		public string unit_name
		{
			get
			{
				return _unit_name;
			}
		}

		public override string ToString()
		{
			return ("Invalid unit: "+_unit_name);
		}

	}

	public class UnitCompilationError : TreeConverter.CompilationError
	{
		private SyntaxTree.unit_or_namespace _SyntaxUsesUnit;
		private string _file_name;

		public UnitCompilationError(string fileName,SyntaxTree.unit_or_namespace syntaxUsesUnit)
		{
			_SyntaxUsesUnit=syntaxUsesUnit;
			_file_name=fileName;
		}

		public SyntaxTree.unit_or_namespace SyntaxUsesUnit
		{
			get
			{
				return _SyntaxUsesUnit;
			}
		}

		public string file_name
		{
			get
			{
				return _file_name;
			}
		}
	}

}
