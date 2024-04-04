// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="none" email=""/>
//     <version>$Revision: 2561 $</version>
// </file>

using System;
using System.Drawing;

namespace ICSharpCode.TextEditor.Util
{
	class CountTipText: TipText
	{
		float triHeight = 10;
		float triWidth  = 10;
		
		public CountTipText(Graphics graphics, Font font, string text) : base(graphics, font, text)
		{
		}
		
		void DrawTriangle(float x, float y, bool flipped)
		{
			Brush brush = BrushRegistry.GetBrush(Color.FromArgb(192, 192, 192));
			base.Graphics.FillRectangle(brush, new RectangleF(x, y, triHeight, triHeight));
			float triHeight2 = triHeight / 2;
			float triHeight4 = triHeight / 4;
			brush = Brushes.Black;
			if (flipped) {
				base.Graphics.FillPolygon(brush, new PointF[] {
				                          	new PointF(x,                y + triHeight2 - triHeight4),
				                          	new PointF(x + triWidth / 2, y + triHeight2 + triHeight4),
				                          	new PointF(x + triWidth,     y + triHeight2 - triHeight4),
				                          });
				
			} else {
				base.Graphics.FillPolygon(brush, new PointF[] {
				                          	new PointF(x,                y +  triHeight2 + triHeight4),
				                          	new PointF(x + triWidth / 2, y +  triHeight2 - triHeight4),
				                          	new PointF(x + triWidth,     y +  triHeight2 + triHeight4),
				                          });
			}
		}
		
		public Rectangle DrawingRectangle1;
		public Rectangle DrawingRectangle2;
		
		public override void Draw(PointF location)
		{
			if (tipText != null && tipText.Length > 0) {
				base.Draw(new PointF(location.X + triWidth + 4, location.Y));
				DrawingRectangle1 = new Rectangle((int)location.X + 2,
				                                  (int)location.Y + 2,
				                                  (int)(triWidth),
				                                  (int)(triHeight));
				DrawingRectangle2 = new Rectangle((int)(location.X + base.AllocatedSize.Width - triWidth  - 2),
				                                  (int)location.Y + 2,
				                                  (int)(triWidth),
				                                  (int)(triHeight));
				DrawTriangle(location.X + 2, location.Y + 2, false);
				DrawTriangle(location.X + base.AllocatedSize.Width - triWidth  - 2, location.Y + 2, true);
			}
		}
		
		protected override void OnMaximumSizeChanged()
		{
			if (IsTextVisible()) {
				SizeF tipSize = Graphics.MeasureString
					(tipText, tipFont, MaximumSize,
					 GetInternalStringFormat());
				tipSize.Width += triWidth * 2 + 8;
				SetRequiredSize(tipSize);
			} else {
				SetRequiredSize(SizeF.Empty);
			}
		}
		
	}
	
	class TipText: TipSection
	{
		protected StringAlignment horzAlign;
		protected StringAlignment vertAlign;
		protected Color           tipColor;
		public Font            tipFont;
		protected StringFormat    tipFormat;
		protected StringFormat tipHdrFormat;
		public string          tipText;
		public int beg_bold;
		public int len_bold;
		public bool insight_wnd=false;
		public TipText desc1;
		public TipText desc2;
		public TipText desc3;
		public bool is_doc;
		public bool param_name;
		public bool param_desc;
		public bool is_head;
		public bool need_tab;
		
		public TipText(Graphics graphics, Font font, string text):
			base(graphics)
		{
			tipFont = font; tipText = text;
			if (text != null && text.Length > short.MaxValue)
				throw new ArgumentException("TipText: text too long (max. is " + short.MaxValue + " characters)", "text");
			
			Color               = SystemColors.ControlText;
			HorizontalAlignment = StringAlignment.Near;
			VerticalAlignment   = StringAlignment.Near;
		}
		
		public override void Draw(PointF location)
		{
			if (IsTextVisible()) {
				RectangleF drawRectangle = new RectangleF(location, AllocatedSize);
				Font fnt = new Font(tipFont, FontStyle.Bold);
				/*Graphics.DrawString(tipText.Substring(0,beg_bold), tipFont,
				                    BrushRegistry.GetBrush(Color),
				                    drawRectangle,
				                    GetInternalStringFormat());
				Graphics.DrawString(tipText.Substring(beg_bold,len_bold), fnt,
				                    BrushRegistry.GetBrush(Color),
				                    drawRectangle,
				                    GetInternalStringFormat());
				Graphics.DrawString(tipText.Substring(beg_bold+len_bold), tipFont,
				                    BrushRegistry.GetBrush(Color),
				                    drawRectangle,
				                    GetInternalStringFormat());*/
				if (this is CountTipText || is_doc)
				{
					Graphics.DrawString(tipText,tipFont,
				                    BrushRegistry.GetBrush(Color),
				                    drawRectangle,
				                    GetInternalStringFormat());
				}
				else if (is_head)
				{
					Graphics.DrawString(tipText,fnt,
				                    BrushRegistry.GetBrush(Color),
				                    drawRectangle,
				                    GetInternalStringFormatForHeader());
				}
				else if (param_name)
				{
					drawRectangle.X += 10;
					Graphics.DrawString(tipText,fnt,
				                    BrushRegistry.GetBrush(System.Drawing.Color.Brown),
				                    drawRectangle,
				                    GetInternalStringFormatForHeader());
				}
				else if (need_tab)
				{
					if (param_desc)
						drawRectangle.X += 20;
					else
						drawRectangle.X += 10;
					Graphics.DrawString(tipText,tipFont,
				                    BrushRegistry.GetBrush(Color),
				                    drawRectangle,
				                    GetInternalStringFormat());
				}
				else
				{
					SizeF szfull = Graphics.MeasureString(tipText,tipFont, new SizeF(drawRectangle.Width,drawRectangle.Height),GetInternalStringFormat());
					
					SizeF sz1 = Graphics.MeasureString(tipText.Substring(0,beg_bold),tipFont, new SizeF(drawRectangle.Width,drawRectangle.Height),GetInternalStringFormat());
					if (szfull.Height > sz1.Height+3)
					{
						Graphics.DrawString(tipText,tipFont,
				                    BrushRegistry.GetBrush(Color),
				                    drawRectangle,
				                    GetInternalStringFormat());
						return;
					}
					Graphics.DrawString(tipText.Substring(0,beg_bold), tipFont,
				                    BrushRegistry.GetBrush(Color),
				                    drawRectangle,
				                    GetInternalStringFormat());
					SizeF sz2 = Graphics.MeasureString(tipText.Substring(beg_bold,len_bold),fnt, new SizeF(drawRectangle.Width,drawRectangle.Height),GetInternalStringFormat());
					SizeF sz2_simp = Graphics.MeasureString(tipText.Substring(beg_bold,len_bold),tipFont, new SizeF(drawRectangle.Width,drawRectangle.Height),GetInternalStringFormat());
					float x_coord = drawRectangle.X;
					float y_coord = drawRectangle.Y;
					if (sz1.Width + sz2.Width > drawRectangle.Width)
					{
						if (drawRectangle.Height > sz1.Height)
							y_coord += sz1.Height;
					}
					else
					{
						x_coord += sz1.Width;
					}
					Graphics.DrawString(tipText.Substring(beg_bold,len_bold), fnt,
				                    BrushRegistry.GetBrush(Color),
				                    x_coord,
				                    y_coord,
				                    GetInternalStringFormat());
					SizeF sz3 = Graphics.MeasureString(tipText.Substring(beg_bold+len_bold),tipFont, new SizeF(drawRectangle.Width,drawRectangle.Height),GetInternalStringFormat());
					if (sz1.Width + sz2.Width + sz3.Width > drawRectangle.Width)
					{
						if (drawRectangle.Height > sz1.Height)
						{
							x_coord = drawRectangle.X;
							y_coord += sz2.Height;
						}
					}
					else 
					{
						x_coord += sz2.Width;
					}
					Graphics.DrawString(tipText.Substring(beg_bold+len_bold), tipFont,
				                    BrushRegistry.GetBrush(Color),
				                    x_coord,
				                    y_coord,
				                    GetInternalStringFormat());
				
				}
				
			}
			
		}
		
		public StringFormat GetInternalStringFormat()
		{
			if (tipFormat == null) {
				tipFormat = CreateTipStringFormat(horzAlign, vertAlign);
			}
			
			return tipFormat;
		}
		
		public StringFormat GetInternalStringFormatForParameter()
		{
			if (tipFormat == null) {
				tipFormat = CreateTipStringFormatForParameter(horzAlign, vertAlign);
			}
			
			return tipFormat;
		}
		
		public StringFormat GetInternalStringFormatForHeader()
		{
			if (tipHdrFormat == null) {
				tipHdrFormat = CreateTipStringFormatForHeader(horzAlign, vertAlign);
			}
			
			return tipHdrFormat;
		}
		
		private void SetRequiredSizeInternal(SizeF drawRectangle)
		{
			Font fnt = new Font(tipFont, FontStyle.Bold);
			SizeF sz1 = Graphics.MeasureString(tipText.Substring(0,beg_bold),tipFont, new SizeF(drawRectangle.Width,drawRectangle.Height),GetInternalStringFormat());
			SizeF sz2 = Graphics.MeasureString(tipText.Substring(beg_bold,len_bold),fnt, new SizeF(drawRectangle.Width,drawRectangle.Height),GetInternalStringFormat());
			SizeF sz2_simp = Graphics.MeasureString(tipText.Substring(beg_bold,len_bold),tipFont, new SizeF(drawRectangle.Width,drawRectangle.Height),GetInternalStringFormat());
			if (sz1.Width + sz2.Width >drawRectangle.Width/*-sz2_simp.Width*/)
			{
				if (drawRectangle.Height <= sz1.Height)
				{
					drawRectangle.Width = drawRectangle.Width + sz1.Width+sz2.Width-drawRectangle.Width;
				}
			}
			else
			{
				//x_coord += sz1.Width;
			}
			SizeF sz3 = Graphics.MeasureString(tipText.Substring(beg_bold+len_bold),tipFont, new SizeF(drawRectangle.Width,drawRectangle.Height),GetInternalStringFormat());
			if (sz1.Width + sz2.Width + sz3.Width > drawRectangle.Width/*-sz2_simp.Width*/)
			{
				if (drawRectangle.Height <= sz1.Height)
				{
					drawRectangle.Width = drawRectangle.Width + sz1.Width+sz2.Width+sz3.Width-drawRectangle.Width;
				}
			}
			SetRequiredSize(drawRectangle);
		}
		
		protected override void OnMaximumSizeChanged()
		{
			base.OnMaximumSizeChanged();
			
			if (IsTextVisible()) {
				SizeF tmp = Graphics.MeasureString
					("a", new Font(tipFont, FontStyle.Bold), MaximumSize,
					 GetInternalStringFormat());
				SizeF tipSize; 
				if (this.len_bold > 0 /*|| need_tab*/) 
				tipSize = Graphics.MeasureString
					(tipText, new Font(tipFont, FontStyle.Bold), MaximumSize,
					 GetInternalStringFormat());
				else
				if (need_tab)
				{
					tipSize = Graphics.MeasureString
						((param_desc?"    ":"   ")+tipText, tipFont, MaximumSize,
					 GetInternalStringFormat());
                    
				}
				else
				tipSize = Graphics.MeasureString
					(tipText, tipFont, MaximumSize,
					 GetInternalStringFormat());
				/*if (!(this is CountTipText) && !is_doc)
				{
					float max = MaximumSize.Width;
				
					while (tipSize.Height > tmp.Height+3 && max < this.GlobalMaxX-100)
					{
						max += 20;
						tipSize = Graphics.MeasureString(tipText, new Font(tipFont, FontStyle.Bold), new SizeF(max,tipSize.Height),
					 	GetInternalStringFormat());
					}
					//if (max < this.GlobalMaxX-10)
					{
						LeftOffset = Convert.ToInt32(max - MaximumSize.Width);
						MaximumSize = new SizeF(max,tipSize.Height);
					}
				}*/
				//SetRequiredSize(tipSize);
				SetRequiredSizeInternal(tipSize);
			} else {
				SetRequiredSize(SizeF.Empty);
			}
		}
		
		StringFormat CreateTipStringFormatForHeader(StringAlignment horizontalAlignment, StringAlignment verticalAlignment)
		{
			StringFormat format = (StringFormat)StringFormat.GenericDefault.Clone();
			format.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.MeasureTrailingSpaces;
			// note: Align Near, Line Center seemed to do something before
			
			format.Alignment     = horizontalAlignment;
			format.LineAlignment = verticalAlignment;
			
			return format;
		}
		
		StringFormat CreateTipStringFormatForParameter(StringAlignment horizontalAlignment, StringAlignment verticalAlignment)
		{
			StringFormat format = (StringFormat)StringFormat.GenericTypographic.Clone();
			format.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.MeasureTrailingSpaces;
			// note: Align Near, Line Center seemed to do something before
			
			format.Alignment     = horizontalAlignment;
			format.LineAlignment = verticalAlignment;
			
			return format;
		}
		
		StringFormat CreateTipStringFormat(StringAlignment horizontalAlignment, StringAlignment verticalAlignment)
		{
			StringFormat format = null;
			if (!insight_wnd)
				format = (StringFormat)StringFormat.GenericDefault.Clone();
			else 
				format = (StringFormat)StringFormat.GenericTypographic.Clone();
			format.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.MeasureTrailingSpaces;
			// note: Align Near, Line Center seemed to do something before
			
			format.Alignment     = horizontalAlignment;
			format.LineAlignment = verticalAlignment;
			
			return format;
		}
		
		protected bool IsTextVisible()
		{
			return tipText != null && tipText.Length > 0;
		}
		
		public Color Color {
			get {
				return tipColor;
			}
			set {
				tipColor = value;
			}
		}
		
		public StringAlignment HorizontalAlignment {
			get {
				return horzAlign;
			}
			set {
				horzAlign = value;
				tipFormat = null;
			}
		}
		
		public StringAlignment VerticalAlignment {
			get {
				return vertAlign;
			}
			set {
				vertAlign = value;
				tipFormat = null;
			}
		}
	}
}
