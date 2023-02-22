// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="none" email=""/>
//     <version>$Revision: 915 $</version>
// </file>

using System;
using System.Diagnostics;
using System.Drawing;

namespace ICSharpCode.TextEditor.Util
{
	class TipSplitter: TipSection
	{
		bool         isHorizontal;
		float     [] offsets;
		TipSection[] tipSections;
		public bool is_desc;
		
		public TipSplitter(Graphics graphics, bool horizontal, params TipSection[] sections): base(graphics)
		{
			Debug.Assert(sections != null);
			
			isHorizontal = horizontal;
			offsets = new float[sections.Length];
			tipSections = (TipSection[])sections.Clone();	
		}
		
		public override void Draw(PointF location)
		{
			/*if (is_desc)
			{
				tipSections[0].Draw
						(new PointF(location.X + offsets[0], location.Y));
				string s1 = (tipSections[0] as TipText).tipText;
				string s2 = (tipSections[1] as TipText).tipText;
				string s3 = (tipSections[2] as TipText).tipText;
				SizeF sz1 = Graphics.MeasureString(s1,(tipSections[0] as TipText).tipFont,new SizeF(tipSections[0].AllocatedSize.Width,tipSections[0].AllocatedSize.Height),(tipSections[0] as TipText).GetInternalStringFormat());
				SizeF sz2 = Graphics.MeasureString(s2,(tipSections[1] as TipText).tipFont,new SizeF(tipSections[1].AllocatedSize.Width,tipSections[1].AllocatedSize.Height),(tipSections[1] as TipText).GetInternalStringFormat());
				SizeF sz3 = Graphics.MeasureString(s3,(tipSections[2] as TipText).tipFont,new SizeF(tipSections[2].AllocatedSize.Width,tipSections[2].AllocatedSize.Height),(tipSections[2] as TipText).GetInternalStringFormat());
				//if (tipSections[1].AllocatedSize.Width + tipSections[0].AllocatedSize.Width <= this.AllocatedSize.Width)
				if (sz2.Height <= sz1.Height + 3)
				{
					tipSections[1].Draw(new PointF(location.X + offsets[1], location.Y));
					//if (tipSections[2].AllocatedSize.Width + tipSections[0].AllocatedSize.Width + tipSections[1].AllocatedSize.Width <= this.AllocatedSize.Width)
					if (sz3.Height <= sz1.Height + 3)
						tipSections[2].Draw(new PointF(location.X + offsets[2], location.Y));
					else
						tipSections[2].Draw(new PointF(location.X, location.Y+sz1.Height));
				}
				else
				{
					tipSections[1].SetAllocatedSize(new SizeF(AllocatedSize.Width,30));
					tipSections[1].SetRequiredSize(new SizeF(AllocatedSize.Width,30));
					tipSections[1].Draw
						(new PointF(location.X, location.Y+sz1.Height));
					tipSections[2].SetAllocatedSize(new SizeF(AllocatedSize.Width,30));
					tipSections[2].SetRequiredSize(new SizeF(AllocatedSize.Width,30));
					tipSections[2].Draw
						(new PointF(location.X, location.Y+sz1.Height+sz1.Height));
				}
				
				return;
			}*/
			
			if (isHorizontal) {
				for (int i = 0; i < tipSections.Length; i ++) {
					tipSections[i].Draw
						(new PointF(location.X + offsets[i], location.Y));
				}
			} else {
				for (int i = 0; i < tipSections.Length; i ++) {
					tipSections[i].Draw
						(new PointF(location.X, location.Y + offsets[i]));
				}
			}
		}
		
		protected override void OnMaximumSizeChanged()
		{
			base.OnMaximumSizeChanged();
			
			float currentDim = 0;
			float otherDim = 0;
			SizeF availableArea = MaximumSize;
			
			for (int i = 0; i < tipSections.Length; i ++) {
				TipSection section = (TipSection)tipSections[i];
				
				section.GlobalMaxX = this.GlobalMaxX;
				section.SetMaximumSize(availableArea);
				this.LeftOffset = Math.Max(section.LeftOffset,this.LeftOffset);
				SizeF requiredArea = section.GetRequiredSize();
				offsets[i] = currentDim;

				// It's best to start on pixel borders, so this will
				// round up to the nearest pixel. Otherwise there are
				// weird cutoff artifacts.
				float pixelsUsed;
				
				if (isHorizontal) {
					pixelsUsed  = (float)Math.Ceiling(requiredArea.Width);
					currentDim += pixelsUsed;
					
					availableArea.Width = Math.Max
						(0, availableArea.Width - pixelsUsed);
					
					otherDim = Math.Max(otherDim, requiredArea.Height);
				} else {
					pixelsUsed  = (float)Math.Ceiling(requiredArea.Height);
					currentDim += pixelsUsed;
					
					availableArea.Height = Math.Max
						(0, availableArea.Height - pixelsUsed);
					
					otherDim = Math.Max(otherDim, requiredArea.Width+8);
				}
			}
			
			foreach (TipSection section in tipSections) {
				if (isHorizontal) {
					section.SetAllocatedSize(new SizeF(section.GetRequiredSize().Width, otherDim));
				} else {
					section.SetAllocatedSize(new SizeF(otherDim, section.GetRequiredSize().Height));
				}
			}

			if (isHorizontal) {
				SetRequiredSize(new SizeF(currentDim, otherDim));
			} else {
				SetRequiredSize(new SizeF(otherDim, currentDim));
			}
		}
	}
}
