// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SyntaxTree;
//using ICSharpCode.SharpDevelop.Dom;
using System.Reflection;
using PascalABCCompiler;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;
//using SymbolTable;

namespace CodeCompletion
{
	public class ExpressionEvaluator
    {
    	public Stack<RetValue> eval_stack = new Stack<RetValue>();
    	
    	public void EvalPlus()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val + (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val + (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (int)left.prim_val + (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val + (long)right.prim_val; break;
								case TypeCode.String : res.string_val = ((int)left.prim_val).ToString() + right.string_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val + (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val + (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val + (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val + (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (int)left.prim_val + (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Double:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (double)left.prim_val + (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (double)left.prim_val + (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (double)left.prim_val + (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (double)left.prim_val + (long)right.prim_val; break;
								case TypeCode.String : res.string_val = ((double)left.prim_val).ToString() + right.string_val; break;
								case TypeCode.Int16 : res.prim_val = (double)left.prim_val + (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (double)left.prim_val + (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (double)left.prim_val + (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (double)left.prim_val + (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (double)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (double)left.prim_val + (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val + (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val + (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (byte)left.prim_val + (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val + (long)right.prim_val; break;
								case TypeCode.String : res.string_val = ((byte)left.prim_val).ToString() + right.string_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val + (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val + (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val + (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val + (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (byte)left.prim_val + (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val + (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val + (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val + (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val + (long)right.prim_val; break;
								case TypeCode.String : res.string_val = ((System.Int16)left.prim_val).ToString() + right.string_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val + (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val + (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val + (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val + (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.Int16)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val + (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val + (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val + (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val + (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val + (long)right.prim_val; break;
								case TypeCode.String : res.string_val = ((System.Int64)left.prim_val).ToString() + right.string_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val + (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val + (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val + (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val + (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val + (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.String:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.string_val = left.string_val + right.prim_val.ToString(); break;
								case TypeCode.Int32 : res.string_val = left.string_val + right.prim_val.ToString(); break;
								case TypeCode.Double : res.string_val = left.string_val + right.prim_val.ToString(); break;
								case TypeCode.Int64 : res.string_val = left.string_val + right.prim_val.ToString(); break;
								case TypeCode.String : res.string_val = left.string_val + right.string_val; break;
								case TypeCode.Int16 : res.string_val = left.string_val + right.prim_val.ToString(); break;
								case TypeCode.SByte : res.string_val = left.string_val + right.prim_val.ToString(); break;
								case TypeCode.UInt16 : res.string_val = left.string_val + right.prim_val.ToString(); break;
								case TypeCode.UInt32 : res.string_val = left.string_val + right.prim_val.ToString(); break;
								case TypeCode.UInt64 : res.string_val = left.string_val + right.prim_val.ToString(); break;
								case TypeCode.Single : res.string_val = left.string_val + right.prim_val.ToString(); break;
								case TypeCode.Char : res.string_val = left.string_val + right.prim_val.ToString(); break;
							}
						}
						break;
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val + (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val + (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val + (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val + (long)right.prim_val; break;
								case TypeCode.String : res.string_val = ((System.SByte)left.prim_val).ToString() + right.string_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val + (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val + (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val + (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val + (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.SByte)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val + (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val + (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val + (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val + (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val + (long)right.prim_val; break;
								case TypeCode.String : res.string_val = ((System.UInt16)left.prim_val).ToString() + right.string_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val + (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val + (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val + (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val + (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val + (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val + (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val + (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val + (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val + (long)right.prim_val; break;
								case TypeCode.String : res.string_val = ((System.UInt32)left.prim_val).ToString() + right.string_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val + (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val + (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val + (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val + (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val + (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val + (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val + (System.UInt64)((int)right.prim_val); break;
								case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val + (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt64)left.prim_val + (System.UInt64)((long)right.prim_val); break;
								case TypeCode.String : res.string_val = ((System.UInt64)left.prim_val).ToString() + right.string_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val + (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val + (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val + (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val + (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val + (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Single:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Single)left.prim_val + (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Single)left.prim_val + (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Single)left.prim_val + (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Single)left.prim_val + (long)right.prim_val; break;
								case TypeCode.String : res.string_val = ((System.Single)left.prim_val).ToString() + right.string_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Single)left.prim_val + (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Single)left.prim_val + (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Single)left.prim_val + (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Single)left.prim_val + (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Single)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Single)left.prim_val + (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Char:
						{
							switch(rcode)
							{
								case TypeCode.Char : res.string_val = left.string_val + right.string_val; break;
								case TypeCode.String : res.string_val = left.prim_val.ToString() + right.string_val; break;
							}
						}
						break;
					
				}
				
			}
			
			eval_stack.Push(res);
		}
		
		public void EvalMinus()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val - (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val - (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (int)left.prim_val - (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val - (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val - (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val - (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val - (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val - (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) - (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (int)left.prim_val - (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Double:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (double)left.prim_val - (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (double)left.prim_val - (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (double)left.prim_val - (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (double)left.prim_val - (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (double)left.prim_val - (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (double)left.prim_val - (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (double)left.prim_val - (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (double)left.prim_val - (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (double)left.prim_val - (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (double)left.prim_val - (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val - (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val - (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (byte)left.prim_val - (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val - (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val - (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val - (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val - (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val - (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val - (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (byte)left.prim_val - (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val - (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val - (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val - (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val - (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val - (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val - (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val - (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val - (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) - (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val - (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val - (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val - (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val - (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val - (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val - (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val - (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val - (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val - (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val - (System.Int64)((System.UInt64)right.prim_val); break;
								case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val - (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val - (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val - (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val - (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val - (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val - (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val - (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val - (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val - (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) - (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val - (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val - (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val - (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val - (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val - (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val - (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val - (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val - (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val - (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val - (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val - (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val - (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val - (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val - (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val - (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val - (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val - (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val - (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val - (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val - (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val - (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val - (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val - (System.UInt64)((int)right.prim_val); break;
								case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val - (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (long)((System.UInt64)left.prim_val) - (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val - (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val - (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val - (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val - (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val - (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val - (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Single:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Single)left.prim_val - (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Single)left.prim_val - (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Single)left.prim_val - (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Single)left.prim_val - (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Single)left.prim_val - (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Single)left.prim_val - (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Single)left.prim_val - (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Single)left.prim_val - (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Single)left.prim_val - (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Single)left.prim_val - (System.Single)right.prim_val; break;
							}
						}
						break;
					
				}
				
			}
			
			eval_stack.Push(res);
		}

        public void EvalMult()
        {
            RetValue right = eval_stack.Pop();
            RetValue left = eval_stack.Pop();
            RetValue res = new RetValue();
            if (left.prim_val != null && right.prim_val != null)
            {
                TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
                TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
                switch (lcode)
                {
                    case TypeCode.Int32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (int)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (int)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (int)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (int)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (int)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (int)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (int)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (int)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((int)left.prim_val) * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (int)left.prim_val * (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (double)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (double)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (double)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (double)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (double)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (double)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (double)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (double)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (double)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (double)left.prim_val * (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (byte)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (byte)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (byte)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (byte)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (byte)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (byte)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (byte)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (byte)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (byte)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (byte)left.prim_val * (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int16)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int16)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int16)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int16)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int16)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int16)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int16)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int16)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.Int16)left.prim_val) * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Int16)left.prim_val * (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Int64)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Int64)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Int64)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Int64)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Int64)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Int64)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Int64)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Int64)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Int64)left.prim_val * (System.Int64)((System.UInt64)right.prim_val); break;
                                case TypeCode.Single: res.prim_val = (System.Int64)left.prim_val * (System.Single)right.prim_val; break;
                            }
                        }
                        break;

                    case TypeCode.SByte:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.SByte)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.SByte)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.SByte)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.SByte)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.SByte)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.SByte)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.SByte)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.SByte)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)((System.SByte)left.prim_val) * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.SByte)left.prim_val * (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt16)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt16)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt16)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt16)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt16)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt16)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt16)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt16)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt16)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt16)left.prim_val * (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt32)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt32)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.UInt32)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.UInt32)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt32)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.UInt32)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt32)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt32)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt32)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt32)left.prim_val * (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.UInt64)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.UInt64)left.prim_val * (System.UInt64)((int)right.prim_val); break;
                                case TypeCode.Double: res.prim_val = (System.UInt64)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (long)((System.UInt64)left.prim_val) * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.UInt64)left.prim_val * (System.UInt64)((System.Int16)right.prim_val); break;
                                case TypeCode.SByte: res.prim_val = (System.UInt64)left.prim_val * (System.UInt64)((sbyte)right.prim_val); break;
                                case TypeCode.UInt16: res.prim_val = (System.UInt64)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.UInt64)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.UInt64)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.UInt64)left.prim_val * (System.Single)right.prim_val; break;
                            }
                        }
                        break;
                    case TypeCode.Single:
                        {
                            switch (rcode)
                            {
                                case TypeCode.Byte: res.prim_val = (System.Single)left.prim_val * (byte)right.prim_val; break;
                                case TypeCode.Int32: res.prim_val = (System.Single)left.prim_val * (int)right.prim_val; break;
                                case TypeCode.Double: res.prim_val = (System.Single)left.prim_val * (double)right.prim_val; break;
                                case TypeCode.Int64: res.prim_val = (System.Single)left.prim_val * (long)right.prim_val; break;
                                //case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
                                case TypeCode.Int16: res.prim_val = (System.Single)left.prim_val * (System.Int16)right.prim_val; break;
                                case TypeCode.SByte: res.prim_val = (System.Single)left.prim_val * (sbyte)right.prim_val; break;
                                case TypeCode.UInt16: res.prim_val = (System.Single)left.prim_val * (System.UInt16)right.prim_val; break;
                                case TypeCode.UInt32: res.prim_val = (System.Single)left.prim_val * (System.UInt32)right.prim_val; break;
                                case TypeCode.UInt64: res.prim_val = (System.Single)left.prim_val * (System.UInt64)right.prim_val; break;
                                case TypeCode.Single: res.prim_val = (System.Single)left.prim_val * (System.Single)right.prim_val; break;
                            }
                        }
                        break;

                }

            }

            eval_stack.Push(res);
        }
		
		public void EvalDiv()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
                res.prim_val = Convert.ToDouble(left.prim_val) / Convert.ToDouble(right.prim_val);
				eval_stack.Push(res);
			}
		}
		
		public void EvalIDiv()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val / (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val / (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val / (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val / (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val / (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val / (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val / (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) / (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Double:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (double)left.prim_val / (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (double)left.prim_val / (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (double)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (double)left.prim_val / (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (double)left.prim_val / (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (double)left.prim_val / (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (double)left.prim_val / (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (double)left.prim_val / (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (double)left.prim_val / (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (double)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val / (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val / (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val / (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val / (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val / (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val / (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val / (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val / (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val / (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val / (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val / (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val / (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val / (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val / (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val / (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) / (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val / (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val / (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val / (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val / (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val / (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val / (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val / (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val / (System.Int64)((System.UInt64)right.prim_val); break;
								case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val / (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val / (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val / (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val / (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val / (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val / (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val / (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) / (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val / (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val / (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val / (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val / (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val / (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val / (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val / (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val / (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val / (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val / (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val / (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val / (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val / (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val / (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val / (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val / (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val / (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)((int)right.prim_val); break;
								case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (long)((System.UInt64)left.prim_val) / (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val / (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val / (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val / (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Single:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Single)left.prim_val / (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Single)left.prim_val / (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Single)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Single)left.prim_val / (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Single)left.prim_val / (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Single)left.prim_val / (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Single)left.prim_val / (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Single)left.prim_val / (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Single)left.prim_val / (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Single)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalAnd()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val & (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val & (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val & (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val & (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val & (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val & (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val & (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) & (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val & (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val & (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val & (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val & (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val & (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val & (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val & (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val & (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val & (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val & (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val & (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val & (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val & (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val & (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val & (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) & (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val & (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val & (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val & (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val & (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val & (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val & (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val & (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val & (System.Int64)((System.UInt64)right.prim_val); break;
								//case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val & (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val & (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val & (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val & (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val & (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val & (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val & (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) & (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val & (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val & (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val & (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val & (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val & (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val & (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val & (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val & (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val & (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val & (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val & (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val & (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val & (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val & (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val & (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val & (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val & (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val & (System.UInt64)((int)right.prim_val); break;
								//case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (long)((System.UInt64)left.prim_val) & (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val & (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val & (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val & (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val & (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val & (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Boolean:
						{
							switch(rcode)
							{
								case TypeCode.Boolean : res.prim_val = (bool)left.prim_val && (bool)right.prim_val; break;	
							}
						}
						break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalOr()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
			switch (lcode)
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val | (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val | (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (long)((int)left.prim_val) | (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val | (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val | (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val | (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val | (int)((System.UInt32)right.prim_val); break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) | (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val | (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val | (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val | (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val | (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val | (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val | (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val | (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val | (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val | (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val | (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val | (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val | (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val | (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val | (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val | (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) | (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val | (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val | (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val | (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val | (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val | (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val | (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val | (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val | (System.Int64)((System.UInt64)right.prim_val); break;
								//case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val | (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val | (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val | (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val | (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val | (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val | (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val | (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) | (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val | (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val | (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val | (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val | (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val | (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val | (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val | (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val | (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val | (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val | (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val | (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val | (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val | (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val | (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val | (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val | (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val | (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val | (System.UInt64)((int)right.prim_val); break;
								//case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (long)((System.UInt64)left.prim_val) | (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val | (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val | (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val | (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val | (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val | (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Boolean:
						{
							switch(rcode)
							{
								case TypeCode.Boolean : res.prim_val = (bool)left.prim_val || (bool)right.prim_val; break;	
							}
						}
						break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalXor()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
			switch (lcode)
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val ^ (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val ^ (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val ^ (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val ^ (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val ^ (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val ^ (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val ^ (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) ^ (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val ^ (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val ^ (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val ^ (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val ^ (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val ^ (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val ^ (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val ^ (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val ^ (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val ^ (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val ^ (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val ^ (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val ^ (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val ^ (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val ^ (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val ^ (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) ^ (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val ^ (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val ^ (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val ^ (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val ^ (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val ^ (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val ^ (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val ^ (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val ^ (System.Int64)((System.UInt64)right.prim_val); break;
								//case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val ^ (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val ^ (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val ^ (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val ^ (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val ^ (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val ^ (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val ^ (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) ^ (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val ^ (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val ^ (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val ^ (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val ^ (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val ^ (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val ^ (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val ^ (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val ^ (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val ^ (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val ^ (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val ^ (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val ^ (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val ^ (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val ^ (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val ^ (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val ^ (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val ^ (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt64)((int)right.prim_val); break;
								//case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (long)((System.UInt64)left.prim_val) ^ (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val ^ (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalBitwiseLeft()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode)
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val << (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val << (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (long)((int)left.prim_val) << (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val << (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val << (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val << (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val << (int)((System.UInt32)right.prim_val); break;
								//case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) << (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val << (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val << (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (long)((byte)left.prim_val) << (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val << (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val << (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val << (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.UInt32)((byte)left.prim_val) << (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.UInt64)((byte)left.prim_val) << (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val << (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val << (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (long)((System.Int16)left.prim_val) << (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val << (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val << (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val << (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.UInt32)((System.Int16)left.prim_val) << (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) << (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val << (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val << (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (long)((System.Int64)left.prim_val) << (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val << (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val << (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val << (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val << (System.Int64)((System.UInt32)right.prim_val); break;
								//case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val << (System.Int64)((System.UInt64)right.prim_val); break;
								//case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val << (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val << (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val << (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val << (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val << (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val << (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.UInt32)((System.SByte)left.prim_val) << (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) << (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val << (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val << (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val << (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val << (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val << (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val << (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val << (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val << (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val << (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val << (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val << (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val << (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val << (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val << (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val << (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val << (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val << (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val << (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (System.UInt64)left.prim_val << (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val << (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val << (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val << (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val << (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val << (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalBitwiseRight()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode)
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val >> (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val >> (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (int)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (int)left.prim_val >> (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val >> (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val >> (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val >> (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (int)left.prim_val >> (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (int)left.prim_val >> (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (int)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val >> (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val >> (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (byte)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (byte)left.prim_val >> (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val >> (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val >> (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val >> (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val >> (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val >> (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (byte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val >> (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val >> (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val >> (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val >> (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val >> (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val >> (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val >> (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.Int16)left.prim_val >> (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val >> (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val >> (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val >> (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val >> (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val >> (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val >> (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val >> (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val >> (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val >> (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val >> (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val >> (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val >> (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val >> (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val >> (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val >> (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.SByte)left.prim_val >> (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val >> (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val >> (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val >> (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val >> (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val >> (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val >> (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val >> (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val >> (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val >> (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val >> (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val >> (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val >> (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val >> (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val >> (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val >> (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val >> (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val >> (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val >> (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val / (double)right.prim_val; break;
								//case TypeCode.Int64 : res.prim_val = (System.UInt64)left.prim_val >> (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val >> (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val >> (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val >> (System.UInt16)right.prim_val; break;
								//case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val >> (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val >> (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val / (System.Single)right.prim_val; break;
							}
						}
						break;

				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalEqual()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val == (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val == (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (int)left.prim_val == (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val == (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() == (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val == (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val == (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val == (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val == (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) == (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (int)left.prim_val == (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Double:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (double)left.prim_val == (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (double)left.prim_val == (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (double)left.prim_val == (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (double)left.prim_val == (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() == (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (double)left.prim_val == (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (double)left.prim_val == (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (double)left.prim_val == (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (double)left.prim_val == (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (double)left.prim_val == (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (double)left.prim_val == (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val == (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val == (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (byte)left.prim_val == (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val == (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() == (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val == (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val == (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val == (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val == (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val == (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (byte)left.prim_val == (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val == (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val == (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val == (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val == (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() == (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val == (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val == (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val == (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val == (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.Int16)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val == (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val == (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val == (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val == (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val == (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() == (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val == (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val == (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val == (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val == (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val == (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.String:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
								case TypeCode.Int32 : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
								case TypeCode.Double : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
								case TypeCode.Int64 : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
								case TypeCode.String : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
								case TypeCode.Int16 : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
								case TypeCode.SByte : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
								case TypeCode.UInt16 : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
								case TypeCode.UInt32 : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
								case TypeCode.UInt64 : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
								case TypeCode.Single : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
								case TypeCode.Char : res.prim_val = (string)left.prim_val == right.prim_val.ToString(); break;
							}
						}
						break;
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val == (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val == (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val == (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val == (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() == (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val == (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val == (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val == (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val == (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.SByte)left.prim_val + (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val == (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val == (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val == (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val == (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val == (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() == (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val == (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val == (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val == (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val == (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val == (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val == (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val == (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val == (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val == (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val == (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() == (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val == (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val == (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val == (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val == (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val == (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val == (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val == (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val == (System.UInt64)((int)right.prim_val); break;
								case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val == (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt64)left.prim_val == (System.UInt64)((long)right.prim_val); break;
								case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() == (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val == (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val == (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val == (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val == (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val == (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val == (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Single:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Single)left.prim_val == (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Single)left.prim_val == (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Single)left.prim_val == (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Single)left.prim_val == (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() == (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Single)left.prim_val == (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Single)left.prim_val == (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Single)left.prim_val == (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Single)left.prim_val == (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Single)left.prim_val == (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Single)left.prim_val == (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Boolean:
						{
							switch(rcode)
							{
								case TypeCode.Boolean : res.prim_val = (bool)left.prim_val == (bool)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Char:
						{
							switch(rcode)
							{
								case TypeCode.Char: res.prim_val = (char)left.prim_val == (char)right.prim_val; break;
								case TypeCode.String: res.prim_val = left.prim_val.ToString() == right.prim_val.ToString(); break;
							}
						}
						break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalNotEqual()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val != (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val != (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (int)left.prim_val != (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val != (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() != (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val != (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val != (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val != (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val != (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) != (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (int)left.prim_val != (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Double:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (double)left.prim_val != (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (double)left.prim_val != (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (double)left.prim_val != (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (double)left.prim_val != (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() != (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (double)left.prim_val != (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (double)left.prim_val != (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (double)left.prim_val != (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (double)left.prim_val != (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (double)left.prim_val != (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (double)left.prim_val != (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val != (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val != (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (byte)left.prim_val != (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val != (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() != (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val != (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val != (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val != (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val != (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val != (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (byte)left.prim_val != (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val != (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val != (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val != (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val != (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() != (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val != (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val != (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val != (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val != (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.Int16)left.prim_val != (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val != (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val != (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val != (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val != (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val != (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() != (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val != (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val != (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val != (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val != (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val != (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val != (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.String:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
								case TypeCode.Int32 : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
								case TypeCode.Double : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
								case TypeCode.Int64 : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
								case TypeCode.String : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
								case TypeCode.Int16 : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
								case TypeCode.SByte : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
								case TypeCode.UInt16 : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
								case TypeCode.UInt32 : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
								case TypeCode.UInt64 : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
								case TypeCode.Single : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
								case TypeCode.Char : res.prim_val = (string)left.prim_val != right.prim_val.ToString(); break;
							}
						}
						break;
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val != (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val != (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val != (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val != (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() != (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val != (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val != (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val != (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val != (System.UInt32)right.prim_val; break;
								//case TypeCode.UInt64 : res.prim_val = (System.SByte)left.prim_val != (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val != (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val != (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val != (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val != (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val != (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() != (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val != (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val != (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val != (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val != (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val != (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val != (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val != (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val != (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val != (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val != (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() != (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val != (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val != (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val != (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val != (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val != (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val != (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val != (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val != (System.UInt64)((int)right.prim_val); break;
								case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val != (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt64)left.prim_val != (System.UInt64)((long)right.prim_val); break;
								case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() != (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val != (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val != (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val != (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val != (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val != (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val != (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Single:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Single)left.prim_val != (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Single)left.prim_val != (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Single)left.prim_val != (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Single)left.prim_val != (long)right.prim_val; break;
								case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() != (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Single)left.prim_val != (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Single)left.prim_val != (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Single)left.prim_val != (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Single)left.prim_val != (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Single)left.prim_val != (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Single)left.prim_val != (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Boolean:
						{
							switch(rcode)
							{
								case TypeCode.Boolean : res.prim_val = (bool)left.prim_val != (bool)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Char:
						{
							switch(rcode)
							{
								case TypeCode.Char: res.prim_val = (char)left.prim_val != (char)right.prim_val; break;
							}
						}
						break;
					
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalLess()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val < (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val < (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (int)left.prim_val < (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val < (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val < (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val < (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val < (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val < (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) < (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (int)left.prim_val < (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Double:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (double)left.prim_val < (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (double)left.prim_val < (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (double)left.prim_val < (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (double)left.prim_val < (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (double)left.prim_val < (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (double)left.prim_val < (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (double)left.prim_val < (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (double)left.prim_val < (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (double)left.prim_val < (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (double)left.prim_val < (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val < (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val < (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (byte)left.prim_val < (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val < (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val < (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val < (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val < (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val < (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val < (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (byte)left.prim_val < (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val < (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val < (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val < (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val < (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val < (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val < (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val < (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val < (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) < (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val < (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val < (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val < (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val < (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val < (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val < (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val < (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val < (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val < (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val < (System.Int64)((System.UInt64)right.prim_val); break;
								case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val < (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val < (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val < (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val < (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val < (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val < (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val < (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val < (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val < (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) < (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val < (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val < (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val < (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val < (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val < (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val < (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val < (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val < (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val < (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val < (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val < (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val < (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val < (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val < (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val < (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val < (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val < (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val < (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val < (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val < (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val < (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val < (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val < (System.UInt64)((int)right.prim_val); break;
								case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val < (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (long)((System.UInt64)left.prim_val) < (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val < (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val < (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val < (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val < (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val < (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val < (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Single:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Single)left.prim_val < (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Single)left.prim_val < (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Single)left.prim_val < (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Single)left.prim_val < (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Single)left.prim_val < (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Single)left.prim_val < (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Single)left.prim_val < (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Single)left.prim_val < (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Single)left.prim_val < (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Single)left.prim_val < (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Boolean:
						{
							switch(rcode)
							{
									case TypeCode.Boolean : res.prim_val = (int)left.prim_val < (int)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Char:
						{
							switch(rcode)
							{
								case TypeCode.Char: res.prim_val = (char)left.prim_val < (char)right.prim_val; break;
							}
						}
						break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalLessEqual()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val <= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val <= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (int)left.prim_val <= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val <= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val <= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val <= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val <= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val <= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) <= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (int)left.prim_val <= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Double:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (double)left.prim_val <= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (double)left.prim_val <= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (double)left.prim_val <= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (double)left.prim_val <= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (double)left.prim_val <= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (double)left.prim_val <= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (double)left.prim_val <= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (double)left.prim_val <= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (double)left.prim_val <= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (double)left.prim_val <= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val <= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val <= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (byte)left.prim_val <= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val <= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val <= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val <= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val <= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val <= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val <= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (byte)left.prim_val <= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val <= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val <= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val <= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val <= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val <= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val <= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val <= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val <= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) <= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val <= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val <= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val <= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val <= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val <= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val <= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val <= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val <= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val <= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val <= (System.Int64)((System.UInt64)right.prim_val); break;
								case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val <= (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val <= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val <= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val <= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val <= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val <= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val <= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val <= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val <= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) <= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val <= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val <= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val <= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val <= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val <= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val <= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val <= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val <= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val <= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val <= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val <= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val <= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val <= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val <= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val <= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val <= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val <= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val <= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val <= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val <= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val <= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val <= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val <= (System.UInt64)((int)right.prim_val); break;
								case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val <= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (long)((System.UInt64)left.prim_val) <= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val <= (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val <= (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val <= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val <= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val <= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val <= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Single:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Single)left.prim_val <= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Single)left.prim_val <= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Single)left.prim_val <= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Single)left.prim_val <= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Single)left.prim_val <= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Single)left.prim_val <= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Single)left.prim_val <= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Single)left.prim_val <= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Single)left.prim_val <= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Single)left.prim_val <= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Boolean:
						{
							switch(rcode)
							{
									case TypeCode.Boolean : res.prim_val = (int)left.prim_val <= (int)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Char:
						{
							switch(rcode)
							{
								case TypeCode.Char: res.prim_val = (char)left.prim_val <= (char)right.prim_val; break;
							}
						}
						break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalGreater()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val > (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val > (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (int)left.prim_val > (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val > (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val > (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val > (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val > (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val > (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) > (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (int)left.prim_val > (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Double:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (double)left.prim_val > (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (double)left.prim_val > (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (double)left.prim_val > (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (double)left.prim_val > (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (double)left.prim_val > (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (double)left.prim_val > (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (double)left.prim_val > (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (double)left.prim_val > (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (double)left.prim_val > (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (double)left.prim_val > (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val > (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val > (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (byte)left.prim_val > (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val > (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val > (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val > (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val > (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val > (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val > (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (byte)left.prim_val > (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val > (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val > (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val > (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val > (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val > (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val > (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val > (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val > (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) > (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val > (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val > (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val > (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val > (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val > (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val > (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val > (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val > (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val > (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val > (System.Int64)((System.UInt64)right.prim_val); break;
								case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val > (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val > (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val > (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val > (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val > (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val > (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val > (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val > (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val > (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) > (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val > (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val > (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val > (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val > (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val > (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val > (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val > (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val > (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val > (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val > (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val > (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val > (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val > (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val > (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val > (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val > (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val > (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val > (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val > (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val > (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val > (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val > (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val > (System.UInt64)((int)right.prim_val); break;
								case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val > (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (long)((System.UInt64)left.prim_val) > (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val > (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val > (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val > (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val > (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val > (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val > (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Single:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Single)left.prim_val > (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Single)left.prim_val > (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Single)left.prim_val > (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Single)left.prim_val > (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Single)left.prim_val > (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Single)left.prim_val > (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Single)left.prim_val > (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Single)left.prim_val > (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Single)left.prim_val > (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Single)left.prim_val > (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Boolean:
						{
							switch(rcode)
							{
									case TypeCode.Boolean : res.prim_val = (int)left.prim_val > (int)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Char:
						{
							switch(rcode)
							{
								case TypeCode.Char: res.prim_val = (char)left.prim_val > (char)right.prim_val; break;
							}
						}
						break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalGreaterEqual()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val >= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val >= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (int)left.prim_val >= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val >= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val >= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val >= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val >= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val >= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) >= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (int)left.prim_val >= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Double:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (double)left.prim_val >= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (double)left.prim_val >= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (double)left.prim_val >= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (double)left.prim_val >= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((double)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (double)left.prim_val >= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (double)left.prim_val >= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (double)left.prim_val >= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (double)left.prim_val >= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (double)left.prim_val >= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (double)left.prim_val >= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val >= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val >= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (byte)left.prim_val >= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val >= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val >= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val >= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val >= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val >= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val >= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (byte)left.prim_val >= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val >= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val >= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val >= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val >= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val >= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val >= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val >= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val >= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) >= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val >= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val >= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val >= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val >= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val >= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val >= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val >= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val >= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val >= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val >= (System.Int64)((System.UInt64)right.prim_val); break;
								case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val >= (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val >= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val >= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val >= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val >= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val >= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val >= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val >= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val >= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) >= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val >= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val >= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val >= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val >= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val >= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val >= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val >= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val >= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val >= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val >= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val >= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val >= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val >= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val >= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val >= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val >= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val >= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val >= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val >= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val >= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val >= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val >= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val >= (System.UInt64)((int)right.prim_val); break;
								case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val >= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (long)((System.UInt64)left.prim_val) >= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val >= (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val >= (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val >= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val >= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val >= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val >= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Single:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Single)left.prim_val >= (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Single)left.prim_val >= (int)right.prim_val; break;
								case TypeCode.Double : res.prim_val = (System.Single)left.prim_val >= (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Single)left.prim_val >= (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Single)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Single)left.prim_val >= (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Single)left.prim_val >= (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Single)left.prim_val >= (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Single)left.prim_val >= (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Single)left.prim_val >= (System.UInt64)right.prim_val; break;
								case TypeCode.Single : res.prim_val = (System.Single)left.prim_val >= (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Boolean:
						{
							switch(rcode)
							{
								case TypeCode.Boolean : res.prim_val = (int)left.prim_val >= (int)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Char:
						{
							switch(rcode)
							{
								case TypeCode.Char: res.prim_val = (char)left.prim_val >= (char)right.prim_val; break;
							}
						}
						break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalRem()
		{
			RetValue right = eval_stack.Pop();
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null && right.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				TypeCode rcode = Type.GetTypeCode(right.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Int32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (int)left.prim_val % (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (int)left.prim_val % (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (int)left.prim_val % (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (int)left.prim_val % (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((int)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (int)left.prim_val % (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (int)left.prim_val % (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (int)left.prim_val % (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (int)left.prim_val % (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((int)left.prim_val) % (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (int)left.prim_val % (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Byte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (byte)left.prim_val % (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (byte)left.prim_val % (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (byte)left.prim_val % (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (byte)left.prim_val % (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((byte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (byte)left.prim_val % (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (byte)left.prim_val % (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (byte)left.prim_val % (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (byte)left.prim_val % (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (byte)left.prim_val % (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (byte)left.prim_val % (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int16)left.prim_val % (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int16)left.prim_val % (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int16)left.prim_val % (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int16)left.prim_val % (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int16)left.prim_val % (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int16)left.prim_val % (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int16)left.prim_val % (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int16)left.prim_val % (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.Int16)left.prim_val) % (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.Int16)left.prim_val % (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.Int64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.Int64)left.prim_val % (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.Int64)left.prim_val % (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.Int64)left.prim_val % (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.Int64)left.prim_val % (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.Int64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.Int64)left.prim_val % (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.Int64)left.prim_val % (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.Int64)left.prim_val % (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.Int64)left.prim_val % (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.Int64)left.prim_val % (System.Int64)((System.UInt64)right.prim_val); break;
								//case TypeCode.Single : res.prim_val = (System.Int64)left.prim_val % (System.Single)right.prim_val; break;
							}
						}
						break;
					
					case TypeCode.SByte:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.SByte)left.prim_val % (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.SByte)left.prim_val % (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.SByte)left.prim_val % (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.SByte)left.prim_val % (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.SByte)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.SByte)left.prim_val % (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.SByte)left.prim_val % (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.SByte)left.prim_val % (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.SByte)left.prim_val % (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)((System.SByte)left.prim_val) % (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.SByte)left.prim_val % (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt16:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt16)left.prim_val % (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt16)left.prim_val % (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt16)left.prim_val % (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt16)left.prim_val % (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt16)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt16)left.prim_val % (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt16)left.prim_val % (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt16)left.prim_val % (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt16)left.prim_val % (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt16)left.prim_val % (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt16)left.prim_val % (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt32:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt32)left.prim_val % (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt32)left.prim_val % (int)right.prim_val; break;
								//case TypeCode.Double : res.prim_val = (System.UInt32)left.prim_val % (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (System.UInt32)left.prim_val % (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt32)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt32)left.prim_val % (System.Int16)right.prim_val; break;
								case TypeCode.SByte : res.prim_val = (System.UInt32)left.prim_val % (sbyte)right.prim_val; break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt32)left.prim_val % (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt32)left.prim_val % (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt32)left.prim_val % (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt32)left.prim_val % (System.Single)right.prim_val; break;
							}
						}
						break;
					case TypeCode.UInt64:
						{
							switch(rcode)
							{
								case TypeCode.Byte : res.prim_val = (System.UInt64)left.prim_val % (byte)right.prim_val; break;
								case TypeCode.Int32 : res.prim_val = (System.UInt64)left.prim_val % (System.UInt64)((int)right.prim_val); break;
								//case TypeCode.Double : res.prim_val = (System.UInt64)left.prim_val % (double)right.prim_val; break;
								case TypeCode.Int64 : res.prim_val = (long)((System.UInt64)left.prim_val) % (long)right.prim_val; break;
								//case TypeCode.String : res.prim_val = ((System.UInt64)left.prim_val).ToString() + (string)right.prim_val; break;
								case TypeCode.Int16 : res.prim_val = (System.UInt64)left.prim_val % (System.UInt64)((System.Int16)right.prim_val); break;
								case TypeCode.SByte : res.prim_val = (System.UInt64)left.prim_val % (System.UInt64)((sbyte)right.prim_val); break;
								case TypeCode.UInt16 : res.prim_val = (System.UInt64)left.prim_val % (System.UInt16)right.prim_val; break;
								case TypeCode.UInt32 : res.prim_val = (System.UInt64)left.prim_val % (System.UInt32)right.prim_val; break;
								case TypeCode.UInt64 : res.prim_val = (System.UInt64)left.prim_val % (System.UInt64)right.prim_val; break;
								//case TypeCode.Single : res.prim_val = (System.UInt64)left.prim_val % (System.Single)right.prim_val; break;
							}
						}
						break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalNot()
		{
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Boolean : res.prim_val = !(bool)left.prim_val; break;
					case TypeCode.Byte: res.prim_val = ~(byte)left.prim_val; break;
					case TypeCode.Int16: res.prim_val = ~(System.Int16)left.prim_val; break;
					case TypeCode.Int32: res.prim_val = ~(System.Int32)left.prim_val; break;
					case TypeCode.Int64: res.prim_val = ~(System.Int64)left.prim_val; break;
					case TypeCode.SByte: res.prim_val = ~(sbyte)left.prim_val; break;
					case TypeCode.UInt16: res.prim_val = ~(System.UInt16)left.prim_val; break;
					case TypeCode.UInt32: res.prim_val = ~(System.UInt32)left.prim_val; break;
					case TypeCode.UInt64: res.prim_val = ~(System.UInt64)left.prim_val; break;
				}
				eval_stack.Push(res);
			}
		}
		
		public void EvalUnmin()
		{
			RetValue left = eval_stack.Pop();
			RetValue res = new RetValue();
			if (left.prim_val != null)
			{
				TypeCode lcode = Type.GetTypeCode(left.prim_val.GetType());
				switch (lcode) 
				{
					case TypeCode.Byte: res.prim_val = -(byte)left.prim_val; break;
					case TypeCode.Int16: res.prim_val = -(System.Int16)left.prim_val; break;
					case TypeCode.Int32: res.prim_val = -(System.Int32)left.prim_val; break;
					case TypeCode.Int64: res.prim_val = -(System.Int64)left.prim_val; break;
					case TypeCode.SByte: res.prim_val = -(sbyte)left.prim_val; break;
					case TypeCode.UInt16: res.prim_val = -(System.UInt16)left.prim_val; break;
					case TypeCode.UInt32: res.prim_val = -(System.UInt32)left.prim_val; break;
					//case TypeCode.UInt64: res.prim_val = -(System.UInt64)left.prim_val; break;
					case TypeCode.Double: res.prim_val = -(double)left.prim_val; break;
					case TypeCode.Single: res.prim_val = -(System.Single)left.prim_val; break;
				}
				eval_stack.Push(res);
			}
		}
        public void visit(token_taginfo node)
        {
            
        }
        public void visit(declaration_specificator node)
        {

        }


    }
}

