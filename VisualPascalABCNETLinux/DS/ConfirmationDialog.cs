using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABC
{
    internal static class ConfirmationDialog
    {
        internal static DialogResult Show(string text, string caption, MessageBoxButtons buttons,
            MessageBoxIcon icon = MessageBoxIcon.None)
        {
            Form owner = Form.ActiveForm;
            if (Type.GetType("Mono.Runtime") == null || Environment.OSVersion.Platform != PlatformID.Unix ||
                PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO != "ru" ||
                (buttons != MessageBoxButtons.YesNo && buttons != MessageBoxButtons.YesNoCancel))
                return MessageBox.Show(owner, text, caption, buttons, icon);

            using (Form dialog = Create(text, caption, buttons, icon))
                return dialog.ShowDialog(owner);
        }

        internal static Form Create(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            bool canCancel = buttons == MessageBoxButtons.YesNoCancel;
            var dialog = new Form {
                Text = caption, Font = SystemFonts.MessageBoxFont, FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent, ShowInTaskbar = false,
                MinimizeBox = false, MaximizeBox = false, ControlBox = canCancel,
                AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(12)
            };
            var layout = new TableLayoutPanel { AutoSize = true, ColumnCount = 2, RowCount = 2, Dock = DockStyle.Fill };
            var message = new Label {
                Text = text, AutoSize = true, UseMnemonic = false,
                MaximumSize = new Size(Math.Max(200, Math.Min(600, Screen.PrimaryScreen.WorkingArea.Width - 100)), 0),
                Margin = new Padding(12, 4, 0, 12)
            };
            Icon symbol = icon == MessageBoxIcon.Question ? SystemIcons.Question :
                icon == MessageBoxIcon.Warning ? SystemIcons.Warning :
                icon == MessageBoxIcon.Error ? SystemIcons.Error :
                icon == MessageBoxIcon.Information ? SystemIcons.Information : null;
            if (symbol != null)
            {
                var image = symbol.ToBitmap();
                var picture = new PictureBox { Image = image, SizeMode = PictureBoxSizeMode.AutoSize, Margin = new Padding(0, 4, 0, 0) };
                picture.Disposed += (sender, e) => image.Dispose();
                layout.Controls.Add(picture, 0, 0);
            }
            layout.Controls.Add(message, 1, 0);
            var choices = new FlowLayoutPanel {
                AutoSize = true, WrapContents = false, Anchor = AnchorStyles.Right,
                FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(0, 8, 0, 0)
            };
            foreach (DialogResult result in canCancel ?
                new[] { DialogResult.Yes, DialogResult.No, DialogResult.Cancel } :
                new[] { DialogResult.Yes, DialogResult.No })
            {
                var button = new Button {
                    Text = result == DialogResult.Yes ? "Да" : result == DialogResult.No ? "Нет" : "Отмена",
                    DialogResult = result, AutoSize = true, Padding = new Padding(12, 3, 12, 3),
                    MinimumSize = new Size(dialog.Font.Height * 5, dialog.Font.Height * 2), Margin = new Padding(4, 0, 0, 0)
                };
                choices.Controls.Add(button);
                if (result == DialogResult.Yes) dialog.AcceptButton = button;
                if (result == DialogResult.Cancel) dialog.CancelButton = button;
            }
            layout.Controls.Add(choices, 0, 1);
            layout.SetColumnSpan(choices, 2);
            dialog.Controls.Add(layout);
            dialog.ActiveControl = (Control)dialog.AcceptButton;
            dialog.FormClosing += (sender, e) => {
                if (dialog.DialogResult != DialogResult.None) return;
                if (canCancel) dialog.DialogResult = DialogResult.Cancel;
                else e.Cancel = true;
            };
            return dialog;
        }
    }
}
