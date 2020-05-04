// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ICSharpCode.SharpDevelop.Widgets.SideBar
{
    public static class ScreenScale
    {
        private static double scale = -1;
        public static double Calc()
        {
            if (scale > 0)
                return scale;
            var dpiXProperty = typeof(System.Windows.SystemParameters).GetProperty("DpiX", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var dpiX = (int)dpiXProperty.GetValue(null, null);
            scale = (double)dpiX / 96;
            return scale;
        }
    }
    public enum SideTabItemStatus {
		Normal,
		Selected,
		Chosen,
		Drag
	}
	
	public class SideTabItem
	{
		string name;
		object tag;
		SideTabItemStatus sideTabItemStatus;
		Bitmap icon;
		bool canBeRenamed = true;
		bool canBeDeleted = true;
		
		public Bitmap Icon {
			get {
				return icon;
			} //
			set {
				icon = value;
			}
		}
		
		public SideTabItemStatus SideTabItemStatus {
			get {
				return sideTabItemStatus;
			}
			set {
				sideTabItemStatus = value;
			}
		}
		
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		
		public object Tag {
			get {
				return tag;
			}
			set {
				tag = value;
			}
		}
		
		public bool CanBeRenamed {
			get {
				return canBeRenamed;
			}
			set {
				canBeRenamed = value;
			}
		}
		
		public bool CanBeDeleted {
			get {
				return canBeDeleted;
			}
			set {
				canBeDeleted = value;
			}
		}
		
		public SideTabItem(string name)
		{
			int idx = name.IndexOf("\n");
			if (idx > 0) {
				this.name = name.Substring(0, idx);
			} else {
				this.name = name;
			}			
		}
		
		public SideTabItem(string name, object tag) : this(name)
		{
			this.tag = tag;
		}
		
		public SideTabItem(string name, object tag, Bitmap icon) : this(name, tag)
		{
			this.icon = new Bitmap(icon);
		}
		
		public SideTabItem Clone()
		{
			return (SideTabItem)MemberwiseClone();
		}
		
		public virtual void DrawItem(Graphics g, Font f, Rectangle rectangle)
		{
            var sz = ScreenScale.Calc();
            var xleft = Convert.ToInt32(3 * sz);
            float width = 0;
            if (Icon != null)
                width = Icon.Width;
            var YDelta = Convert.ToInt32(Math.Max(0,(rectangle.Height - g.MeasureString("AgQ", f).Height)/2));
            var stringPoint = new PointF(rectangle.X + width * (float)sz + 1 + xleft, rectangle.Y + YDelta);
            var imageRect = new Rectangle(xleft, rectangle.Y + YDelta, Convert.ToInt32(16 * sz), Convert.ToInt32(16 * sz));
            switch (sideTabItemStatus) {
				case SideTabItemStatus.Normal:
					if (Icon != null) {
                        //g.DrawImage(Icon, 0, rectangle.Y);
                        g.DrawImage(Icon, imageRect);
					}
					g.DrawString(name, f, SystemBrushes.ControlText, stringPoint);
					break;
				case SideTabItemStatus.Drag:
					ControlPaint.DrawBorder3D(g, rectangle, Border3DStyle.RaisedInner);
					rectangle.X += 1;
					rectangle.Y += 1;
					rectangle.Width  -= 2;
					rectangle.Height -= 2;
					
					g.FillRectangle(SystemBrushes.ControlDarkDark, rectangle);
					if (Icon != null) {
                        //g.DrawImage(Icon, 0, rectangle.Y);
                        g.DrawImage(Icon, imageRect);
					}
					g.DrawString(name, f, SystemBrushes.HighlightText, stringPoint);
                    break;
				case SideTabItemStatus.Selected:
					ControlPaint.DrawBorder3D(g, rectangle, Border3DStyle.RaisedInner);
					if (Icon != null) {
                        //g.DrawImage(Icon, 0, rectangle.Y);
                        g.DrawImage(Icon, imageRect);
					}
					g.DrawString(name, f, SystemBrushes.ControlText, stringPoint);
					break;
				case SideTabItemStatus.Chosen:
					ControlPaint.DrawBorder3D(g, rectangle, Border3DStyle.Sunken);
					rectangle.X += 1;
					rectangle.Y += 1;
					rectangle.Width  -= 2;
					rectangle.Height -= 2;
					
					using (Brush brush = new SolidBrush(ControlPaint.Light(SystemColors.Control))) {
						g.FillRectangle(brush, rectangle);
					}
					
					if (Icon != null) {
                        //g.DrawImage(Icon, 1, rectangle.Y + 1);
                        imageRect.Offset(1, 1);
                        g.DrawImage(Icon, imageRect);
					}
                    stringPoint.X += 1;
                    stringPoint.Y += 1;
                    g.DrawString(name, f, SystemBrushes.ControlText, stringPoint);
					break;
			}
		}
	}
}
