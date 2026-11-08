using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace WeifenLuo.WinFormsUI.Docking
{
    public abstract class InertButtonBase : Control
    {
        private static readonly bool showHoverBorder = ShouldDrawHoverBorder(
            Type.GetType("Mono.Runtime") != null, Environment.OSVersion.Platform);

        internal static bool ShouldDrawHoverBorder(bool isMono, PlatformID platform)
        {
            return !(isMono && platform == PlatformID.Unix);
        }

        protected InertButtonBase()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        public abstract Bitmap HoverImage { get; }

        public abstract Bitmap PressImage { get; }

        public abstract Bitmap Image { get; }

        protected virtual Color ImageBackgroundColor { get { return Color.Transparent; } }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Color color = ImageBackgroundColor;
            if (color.A == 255)
            {
                using (var brush = new SolidBrush(color))
                    e.Graphics.FillRectangle(brush, ClientRectangle);
            }
            else
                base.OnPaintBackground(e);
        }

        private bool m_isMouseOver = false;
        protected bool IsMouseOver
        {
            get { return m_isMouseOver; }
            private set
            {
                if (m_isMouseOver == value)
                    return;

                m_isMouseOver = value;
                Invalidate();
            }
        }

        private bool m_isMouseDown = false;
        protected bool IsMouseDown
        {
            get { return m_isMouseDown; }
            private set
            {
                if (m_isMouseDown == value)
                    return;

                m_isMouseDown = value;
                Invalidate();
            }
        }

        protected override Size DefaultSize
        {
            get { return new Size(16, 15); }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool over = ClientRectangle.Contains(e.X, e.Y);
            if (IsMouseOver != over)
                IsMouseOver = over;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!IsMouseOver)
                IsMouseOver = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsMouseOver)
                IsMouseOver = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!IsMouseDown)
                IsMouseDown = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsMouseDown)
                IsMouseDown = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (HoverImage != null)
            {
                if (IsMouseOver && Enabled)
                {
                    e.Graphics.DrawImage(
                       IsMouseDown ? PressImage : HoverImage,
                       PatchController.EnableHighDpi == true
                           ? ClientRectangle
                           : new Rectangle(0, 0, Image.Width, Image.Height));
                }
                else
                {
                    e.Graphics.DrawImage(
                       Image,
                       PatchController.EnableHighDpi == true
                           ? ClientRectangle
                           : new Rectangle(0, 0, Image.Width, Image.Height));
                }

                base.OnPaint(e);
                return;
            }

            if (IsMouseOver && Enabled && showHoverBorder)
            {
                using (Pen pen = new Pen(ForeColor))
                {
                    e.Graphics.DrawRectangle(pen, Rectangle.Inflate(ClientRectangle, -1, -1));
                }
            }

            using (ImageAttributes imageAttributes = new ImageAttributes())
            {
                ColorMap[] colorMap = new ColorMap[2];
                colorMap[0] = new ColorMap();
                colorMap[0].OldColor = Color.FromArgb(0, 0, 0);
                colorMap[0].NewColor = ForeColor;
                colorMap[1] = new ColorMap();
                colorMap[1].OldColor = Image.GetPixel(0, 0);
                colorMap[1].NewColor = ImageBackgroundColor;

                imageAttributes.SetRemapTable(colorMap);

                e.Graphics.DrawImage(
                   Image,
                   new Rectangle(0, 0, Image.Width, Image.Height),
                   0, 0,
                   Image.Width,
                   Image.Height,
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }

            base.OnPaint(e);
        }

        public void RefreshChanges()
        {
            if (IsDisposed)
                return;

            bool mouseOver = ClientRectangle.Contains(PointToClient(Control.MousePosition));
            if (mouseOver != IsMouseOver)
                IsMouseOver = mouseOver;

            OnRefreshChanges();
        }

        protected virtual void OnRefreshChanges()
        {
        }
    }
}
