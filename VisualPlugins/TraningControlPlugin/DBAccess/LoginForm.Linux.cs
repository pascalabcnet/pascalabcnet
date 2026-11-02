#if PABC_LINUX_PLUGIN
using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABCPlugins.DBAccess
{
    public partial class LoginForm
    {
        // Mono's LinkLabel can clip centered link text. Keep the same action,
        // but let a normal Label paint the complete rating in the Linux build.
        private void InitializeLinuxRating()
        {
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
