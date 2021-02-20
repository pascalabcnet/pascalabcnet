// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbeck�" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 1965 $</version>
// </file>

using System;

namespace Debugger
{
	/// <summary>
	/// Identifies frame on thread callstack
	/// </summary>
	struct FrameID {
		uint chainIndex;
		uint frameIndex;
		
		public uint ChainIndex {
			get {
				return chainIndex;
			}
		}
		
		public uint FrameIndex {
			get {
				return frameIndex;
			}
		}
		
		public FrameID(uint chainIndex, uint frameIndex)
		{
			this.chainIndex = chainIndex;
			this.frameIndex = frameIndex;
		}
		
		public override int GetHashCode()
		{
			return chainIndex.GetHashCode() ^ frameIndex.GetHashCode();
		}
		
		public override bool Equals(object obj)
		{
			if (!(obj is FrameID)) return false; 
			FrameID myFrameID = (FrameID)obj;
			return this.chainIndex == myFrameID.chainIndex && this.frameIndex == myFrameID.frameIndex;
		}
		
		public override string ToString()
		{
			return string.Format("{0},{1}", this.chainIndex, this.frameIndex);
		}
	}
}
