// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
/***************************************************************************
 *   
 *   Интерфейс доступа к кодогенераторам для Compiler   
 *   Зависит от SemanticTree
 *
 ***************************************************************************/

using System;
using System.Collections;
using PascalABCCompiler.NetHelper;

namespace PascalABCCompiler.CodeGenerators
{
	/*public class CodeGeneratorOptions
	{
		public NETGenerator.TargetType TargetType;
	}*/

	public class Controller
	{
		private NETGenerator.ILConverter il_converter;//=new NETGenerator.ILConverter();

		public void GenerateILCodeAndSaveAssembly(SemanticTree.IProgramNode ProgramTree,string TargetFileName,string SourceFileName ,
            NETGenerator.CompilerOptions options, Hashtable StandartDirectories, string[] ResourceFiles)
		{
            il_converter = new NETGenerator.ILConverter(StandartDirectories);
			il_converter.ConvertFromTree(ProgramTree, TargetFileName, SourceFileName, options, ResourceFiles);
		}

		public void EmitAssemblyRedirects(AssemblyResolveScope resolveScope, string outputFileName) =>
			il_converter.EmitAssemblyRedirects(resolveScope, outputFileName);

		public void Reset()
		{
            il_converter = null;
        }
	}
}
