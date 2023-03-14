//------------------------------------------------------------------------------
/// <copyright from='1997' to='2002' company='Microsoft Corporation'>
///    Copyright (c) Microsoft Corporation. All Rights Reserved.
///
///    This source code is intended only as a supplement to Microsoft
///    Development Tools and/or on-line documentation.  See these other
///    materials for detailed information regarding Microsoft code samples.
///
/// </copyright>
//------------------------------------------------------------------------------
namespace SampleDesignerHost
{
    using System;
    using System.Collections;

	/// This is just a special stack to handle the transaction descriptions.
	/// It functions like a normal stack, except it looks for the first
	/// non-null (and non "") string.
    internal class StringStack : Stack {
         internal StringStack() {
         }

         internal string GetNonNull() {
             int items = this.Count;
             object item;
             object[] itemArr = this.ToArray();
             for (int i = items - 1; i >=0; i--) {
                 item = itemArr[i];
                 if (item != null && item is string && ((string)item).Length > 0) {
                     return (string)item;
                 }
             }
             return "";
         }
     }
}
