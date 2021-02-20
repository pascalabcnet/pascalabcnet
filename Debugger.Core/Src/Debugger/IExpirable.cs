// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbeck�" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 1634 $</version>
// </file>

using System;

namespace Debugger
{
	public interface IExpirable
	{
		event EventHandler Expired;
		
		bool HasExpired { get ; }
	}
}
