// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using VisualPascalABCPlugins;
using System.Threading;
using Microsoft.VisualBasic;
using System.CodeDom.Compiler;

namespace VisualPascalABC
{
	/*public class VBNETCompiler
	{
		private static VBNETCompiler compiler;
		public event PascalABCCompiler.ChangeCompilerStateEventDelegate OnChangeCompilerState;
		
		public static VBNETCompiler Compiler
		{
			get
			{
				if (compiler == null)
				{
					compiler = new VBNETCompiler();
					
				}
				return compiler;
			}
		}
		
		public void Compile(PascalABCCompiler.CompilerOptions options)
		{
			VBCodeProvider vbcp = new VBCodeProvider();
			string[] sources = new string[1];
			sources[0] = options.SourceFileName;
			CompilerParameters comp_opt = new CompilerParameters();
			comp_opt.OutputAssembly = options.OutputFileName;
			comp_opt.IncludeDebugInformation = options.Debug;
			comp_opt.GenerateExecutable = true;
			CompilerResults res = vbcp.CompileAssemblyFromFile(comp_opt,sources);
		}
	}*/
}

