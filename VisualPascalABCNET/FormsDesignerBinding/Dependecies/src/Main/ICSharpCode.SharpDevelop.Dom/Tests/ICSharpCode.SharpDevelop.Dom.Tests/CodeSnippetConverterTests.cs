﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using NUnit.Framework;

namespace ICSharpCode.SharpDevelop.Dom.Tests
{
	static class SharedProjectContentRegistryForTests
	{
		public static readonly ProjectContentRegistry Instance = new ProjectContentRegistry();
		
		static SharedProjectContentRegistryForTests() {} // delay-initialize
	}
	
	[TestFixture]
	public class CodeSnippetConverterTests
	{
		List<IProjectContent> referencedContents;
		CodeSnippetConverter converter;
		string errors;
		
		public CodeSnippetConverterTests()
		{
			referencedContents = new List<IProjectContent> {
				SharedProjectContentRegistryForTests.Instance.Mscorlib,
				SharedProjectContentRegistryForTests.Instance.GetProjectContentForReference("System", typeof(Uri).Module.FullyQualifiedName)
			};
		}
		
		[SetUp]
		public void SetUp()
		{
			converter = new CodeSnippetConverter {
				ReferencedContents = referencedContents
			};
		}
		
		[Test]
		public void FixExpressionCase()
		{
			Assert.AreEqual("AppDomain.CurrentDomain", converter.VBToCSharp("appdomain.currentdomain", out errors));
		}
		
		[Test]
		public void Statements()
		{
			Assert.AreEqual("a = Console.Title;\n" +
			                "b = Console.ReadLine();",
			                Normalize(converter.VBToCSharp("a = Console.Title\n" +
			                                               "b = Console.Readline", out errors)));
		}
		
		[Test]
		public void UnknownPropertyReference()
		{
			Assert.AreEqual("a = Me.Font",
			                Normalize(converter.CSharpToVB("a = this.Font;", out errors)));
		}
		
		[Test]
		public void UnknownPropertyReference2()
		{
			Assert.AreEqual("X(Me.Font)",
			                Normalize(converter.CSharpToVB("X(this.Font);", out errors)));
		}
		
		[Test]
		public void FixReferenceToOtherMethodInSameClass()
		{
			Assert.AreEqual("public void A()\n" +
			                "{\n" +
			                "  Test();\n" +
			                "}\n" +
			                "public void Test()\n" +
			                "{\n" +
			                "}",
			                Normalize(converter.VBToCSharp("Sub A()\n" +
			                                               " test\n" +
			                                               "End Sub\n" +
			                                               "Sub Test\n" +
			                                               "End Sub",
			                                               out errors)));
		}
		
		[Test]
		public void ConvertFloatingPointToInteger()
		{
			Assert.AreEqual("CInt(Math.Truncate(-3.5))", converter.CSharpToVB("(int)(-3.5)", out errors));
			Assert.AreEqual("CInt(Math.Truncate(-3.5F))", converter.CSharpToVB("(int)(-3.5f)", out errors));
			Assert.AreEqual("CInt(Math.Truncate(-3.5D))", converter.CSharpToVB("(int)(-3.5m)", out errors));
		}
		
		[Test]
		public void ConvertLongToInteger()
		{
			Assert.AreEqual("CInt(-35L)", converter.CSharpToVB("(int)(-35L)", out errors));
		}
		
		[Test]
		public void ConvertCharToInteger()
		{
			Assert.AreEqual("AscW(\"x\"C)", converter.CSharpToVB("(int)'x'", out errors));
		}
		
		[Test]
		public void ConvertCharToByte()
		{
			Assert.AreEqual("CByte(AscW(\"x\"C))", converter.CSharpToVB("(byte)'x'", out errors));
		}
		
		[Test]
		public void ConvertIntegerToChar()
		{
			Assert.AreEqual("ChrW(65)", converter.CSharpToVB("(char)65", out errors));
		}
		
		[Test]
		public void ConvertByteToChar()
		{
			Assert.AreEqual("ChrW(CByte(65))", converter.CSharpToVB("(char)(byte)65", out errors));
		}
		
		[Test]
		public void FixItemAccess()
		{
			Assert.AreEqual("public void A(System.Collections.Generic.List<string> l)\n" +
			                "{\n" +
			                "  l[0].ToString();\n" +
			                "}",
			                Normalize(converter.VBToCSharp("Sub A(l As Generic.List(Of String))\n" +
			                                               " l.Item(0).ToString()\n" +
			                                               "End Sub",
			                                               out errors)));
		}
		
		[Test]
		public void ListAccessWithinForNextLoop()
		{
			Assert.AreEqual("public void A(System.Collections.Generic.List<string> l)\n" +
			                "{\n" +
			                "  for (int i = 0; i <= l.Count - 1; i++) {\n" +
			                "    sum += l[i].Length;\n" +
			                "  }\n" +
			                "}",
			                Normalize(converter.VBToCSharp("Sub A(l As Generic.List(Of String))\n" +
			                                               " For i As Integer = 0 To l.Count - 1\n" +
			                                               "   sum += l(i).Length\n" +
			                                               " Next\n" +
			                                               "End Sub",
			                                               out errors)));
		}
		
		[Test]
		public void ListAccessOnField()
		{
			Assert.AreEqual("private System.Collections.Generic.List<System.Collections.Generic.List<string>> ParsedText = new System.Collections.Generic.List<System.Collections.Generic.List<string>>();\n" +
			                "public void A()\n" +
			                "{\n" +
			                "  ParsedText[0].ToString();\n" +
			                "}",
			                Normalize(converter.VBToCSharp("Private ParsedText As New Generic.List(Of Generic.List(Of String))\n" +
			                                               "Sub A()\n" +
			                                               " ParsedText(0).ToString()\n" +
			                                               "End Sub",
			                                               out errors)));
		}
		
		string Normalize(string text)
		{
			return text.Replace("\t", "  ").Replace("\r", "").Trim();
		}
	}
}
