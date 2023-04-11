// ObjectValueKind.cs
//
// Author:
//   Lluis Sanchez Gual <lluis@novell.com>
//
// Copyright (c) 2008 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

using System;

namespace Mono.Debugging.Client
{
	[Flags]
	public enum ObjectValueFlags: uint {
		None = 0,
		Object = 1,             // The value is an object
		Array = 1 << 1,         // The value is an array
		Primitive = 1 << 2,     // The value is a primitive value
		Unknown = 1 << 3,       // The evaluated identifier is unknown
		Error = 1 << 4,         // The expression evaluation returned an error
		NotSupported = 1 << 5,  // The expression is valid but its evaluation is not supported
		Evaluating = 1 << 6,    // The expression is being evaluated. The value will be updated when done.
		ImplicitNotSupported = 1 << 7,    // The expression is valid but it can't be performed implicitly.
		KindMask = 0x000000ff,
		
		Field = 1 << 8,
		Property = 1 << 9,
		Parameter = 1 << 10,
		Variable = 1 << 11,
		ArrayElement = 1 << 12,
		Method = 1 << 13,
		Literal = 1 << 14,
		Type = 1 << 15,
		Namespace = 1 << 16,
		Group = 1 << 17,
		OriginMask = 0x0003ff00,
		
		Global = 1 << 18,	// For fields, it means static
		ReadOnly = 1 << 19,
		NoRefresh = 1 << 20, // When set, this value can't be refreshed
		EvaluatingGroup = 1 << 21, // When set, this value represents a set of values being evaluated
		                           // When evaluation ends, the value is updated, and the children are the
		                           // values represented by this group
		IEnumerable = 1 << 22,
		
		// For field and property
		Public = 1 << 24,
		Protected = 1 << 25,
		Internal = 1 << 26,
		Private = 1 << 27,
		InternalProtected = Internal | Protected,
		AccessMask = Public | Protected | Internal | Private
	}
}
