// 
// FieldReferenceBatch.cs
//  
// Authors: David Karlaš <david.karlas@xamarin.com>
// 
// Copyright (c) 2014 Xamarin Inc. (http://www.xamarin.com)
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

using Mono.Debugger.Soft;
using System.Collections.Generic;

namespace Mono.Debugging.Soft
{
	public class FieldReferenceBatch
	{
		readonly object locker = new object ();
		readonly object obj;
		List<FieldInfoMirror> fields = new List<FieldInfoMirror> ();
		object[] results;

		public FieldReferenceBatch (object obj)
		{
			this.obj = obj;
		}

		public void Add (FieldInfoMirror fieldVal)
		{
			lock (locker) {
				fields.Add (fieldVal);
			}
		}

		public object GetValue (FieldInfoMirror field)
		{
			lock (locker) {
				if (results == null || fields.Count != results.Length)
					results = ((ObjectMirror)obj).GetValues (fields);
				return results [fields.IndexOf (field)];
			}
		}

		public void Invalidate ()
		{
			lock (locker) {
				results = null;
			}
		}
	}
}
