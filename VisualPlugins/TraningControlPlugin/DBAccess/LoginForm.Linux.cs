#if PABC_LINUX_PLUGIN
using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABCPlugins.DBAccess
{
    public partial class LoginForm
    {
        private static void MakePluginImageTransparent(PictureBox picture)
        {
            if (picture.Image == null) return;
            // Do not use MakeTransparent: Mono implements it through libgdiplus color keys.
            // Store actual transparent pixels before ToolStrip scales or clones the bitmap.
            var image = new Bitmap(picture.Image.Width, picture.Image.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var source = new Bitmap(picture.Image))
            {
                int key = picture.BackColor.ToArgb() & 0x00ffffff;
                for (int y = 0; y < source.Height; y++)
                    for (int x = 0; x < source.Width; x++)
                    {
                        Color pixel = source.GetPixel(x, y);
                        image.SetPixel(x, y, pixel.A == 0 || (pixel.ToArgb() & 0x00ffffff) == key
                            ? Color.FromArgb(0, 0, 0, 0) : pixel);
                    }
            }
            picture.Image = image;
            picture.Disposed += (sender, args) => image.Dispose();
        }

        // Mono's LinkLabel can clip centered link text. Keep the same action,
        // but let a normal Label paint the complete rating in the Linux build.
        private void InitializeLinuxRating()
        {
            MakePluginImageTransparent(PluginImage);
            MakePluginImageTransparent(PluginImageAuthorized);
            var rating = new Label
            {
                Name = "linuxRating",
                Bounds = labelRating.Bounds,
                Font = labelRating.Font,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = labelRating.LinkColor,
                Cursor = Cursors.Hand,
                TabStop = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            rating.Click += (sender, args) => labelRating_LinkClicked(labelRating, new LinkLabelLinkClickedEventArgs(null));
            rating.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.Enter || args.KeyCode == Keys.Space)
                {
                    args.Handled = true;
                    args.SuppressKeyPress = true;
                    labelRating_LinkClicked(labelRating, new LinkLabelLinkClickedEventArgs(null));
                }
            };
            labelRating.TextChanged += (sender, args) => rating.Text = labelRating.Text;
            panelAuthorized.Controls.Remove(labelRating);
            labelRating.Visible = false;
            panelAuthorized.Controls.Add(rating);
            Disposed += (sender, args) => labelRating.Dispose();
        }
    }
}
#endif
