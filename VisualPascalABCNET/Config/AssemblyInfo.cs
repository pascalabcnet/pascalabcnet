// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

[assembly: AssemblyTitle("PascalABC.NET")]
[assembly: AssemblyDescription("PascalABC.NET Compiler GUI")]
[assembly: AssemblyConfiguration("")]

// IMPORTANT! IDE собирается под net462
// Атрибут подменяется только для поддержки High DPI
[assembly: TargetFramework(".NETFramework,Version=v4.7.1")]