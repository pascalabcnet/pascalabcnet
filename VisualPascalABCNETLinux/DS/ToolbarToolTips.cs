using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABC
{
    // Mono's standard ToolStrip tooltip measures and draws with different padding.
    // Own the tooltip so both operations use the same text rectangle.
    internal sealed class ToolbarToolTips : Component
    {
        private readonly ToolStrip toolbar;
        private readonly Form owner;
        private readonly ToolTip tooltip = new ToolTip { OwnerDraw = true };
        private readonly Timer delay = new Timer { Interval = 500 };
        private ToolStripItem currentItem;
        private string shownText;
        private readonly bool originalShowItemToolTips;

        public ToolbarToolTips(IContainer container, ToolStrip toolbar, Form owner)
        {
            container.Add(this);
            this.toolbar = toolbar;
            this.owner = owner;
            originalShowItemToolTips = toolbar.ShowItemToolTips;
            toolbar.ShowItemToolTips = false;
            foreach (ToolStripItem item in toolbar.Items) Attach(item);
            toolbar.ItemAdded += ItemAdded;
            toolbar.ItemRemoved += ItemRemoved;
            toolbar.MouseLeave += Hide;
            toolbar.MouseDown += Hide;
            toolbar.VisibleChanged += Hide;
            owner.Deactivate += Hide;
            delay.Tick += Show;
            tooltip.Popup += Popup;
            tooltip.Draw += Draw;
        }

        private void Attach(ToolStripItem item)
        {
            item.MouseEnter += Enter;
            item.MouseLeave += Hide;
            item.MouseDown += Hide;
        }

        private void Detach(ToolStripItem item)
        {
            item.MouseEnter -= Enter;
            item.MouseLeave -= Hide;
            item.MouseDown -= Hide;
        }

        private void ItemAdded(object sender, ToolStripItemEventArgs e) { Attach(e.Item); }
        private void ItemRemoved(object sender, ToolStripItemEventArgs e)
        {
            Detach(e.Item);
            if (e.Item == currentItem) Hide(sender, e);
        }

        private void Enter(object sender, EventArgs e)
        {
            Hide(sender, e);
            if (Control.MouseButtons != MouseButtons.None || sender is ToolStripSeparator) return;
            currentItem = (ToolStripItem)sender;
            delay.Start();
        }

        private void Hide(object sender, EventArgs e)
        {
            delay.Stop();
            tooltip.Hide(toolbar);
            currentItem = null;
        }

        private void Show(object sender, EventArgs e)
        {
            delay.Stop();
            if (currentItem == null || currentItem.Owner != toolbar || !toolbar.Visible ||
                toolbar.GetItemAt(toolbar.PointToClient(Cursor.Position)) != currentItem) return;
            string text = GetText(currentItem);
            if (string.IsNullOrEmpty(text)) return;
            shownText = text;
            tooltip.Show(text, toolbar, currentItem.Bounds.Left, currentItem.Bounds.Bottom + 2, 5000);
        }

        internal static string GetText(ToolStripItem item)
        {
            string text = item.ToolTipText;
            if (!string.IsNullOrEmpty(text) || !item.AutoToolTip) return text;
            // Match ToolStrip's automatic captions, including mnemonic markers.
            var caption = new System.Text.StringBuilder();
            text = item.Text ?? string.Empty;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != '&') caption.Append(text[i]);
                else if (i + 1 < text.Length && text[i + 1] == '&')
                {
                    caption.Append('&');
                    i++;
                }
            }
            return caption.ToString();
        }

        internal static Size Measure(Graphics graphics, Font font, string text)
        {
            using (var format = new StringFormat(StringFormat.GenericTypographic))
            {
                Size size = Size.Ceiling(graphics.MeasureString(text, font, int.MaxValue, format));
                return new Size(size.Width + 12, size.Height + 6);
            }
        }

        internal static void Paint(Graphics graphics, Rectangle bounds, Font font, string text)
        {
            graphics.FillRectangle(SystemBrushes.Info, bounds);
            ControlPaint.DrawBorder(graphics, bounds, SystemColors.WindowFrame, ButtonBorderStyle.Solid);
            Rectangle content = Rectangle.Inflate(bounds, -6, -3);
            using (var format = new StringFormat(StringFormat.GenericTypographic))
                graphics.DrawString(text, font, SystemBrushes.InfoText, content, format);
        }

        private void Popup(object sender, PopupEventArgs e)
        {
            using (Graphics graphics = toolbar.CreateGraphics())
                e.ToolTipSize = Measure(graphics, SystemFonts.DefaultFont, shownText ?? string.Empty);
        }

        private void Draw(object sender, DrawToolTipEventArgs e)
        {
            Paint(e.Graphics, e.Bounds, SystemFonts.DefaultFont, e.ToolTipText);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                delay.Stop();
                toolbar.ItemAdded -= ItemAdded;
                toolbar.ItemRemoved -= ItemRemoved;
                foreach (ToolStripItem item in toolbar.Items) Detach(item);
                toolbar.MouseLeave -= Hide;
                toolbar.MouseDown -= Hide;
                toolbar.VisibleChanged -= Hide;
                owner.Deactivate -= Hide;
                toolbar.ShowItemToolTips = originalShowItemToolTips;
                delay.Dispose();
                tooltip.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
