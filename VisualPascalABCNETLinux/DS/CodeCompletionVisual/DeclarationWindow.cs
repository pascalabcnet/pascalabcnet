using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Util;

namespace ICSharpCode.TextEditor.Gui.CompletionWindow
{
    // Form.Show takes focus in Mono despite ShowWithoutActivation. ToolTip uses
    // Mono's dedicated nonactivating window instead. Never show a Form here.
    public sealed class DeclarationWindow : Component, IDeclarationViewWindow
    {
        private readonly Form owner;
        private readonly ToolTip toolTip;
        private readonly Control layout;
        private string description;
        private bool visible;
        public bool in_completion_list;
        public event EventHandler VisibleChanged;

        public DeclarationWindow(Form parent)
        {
            owner = parent ?? throw new ArgumentNullException(nameof(parent));
            // The painter needs a control for layout/FindForm, but must never
            // resize the editor or the native tooltip from its Draw callback.
            layout = new Control { Visible = false, TabStop = false, Font = VisualPascalABC.Constants.CompletionFont };
            owner.Controls.Add(layout);
            toolTip = new ToolTip {
                OwnerDraw = true, ShowAlways = true,
                UseAnimation = false, UseFading = false
            };
            toolTip.Popup += MeasureTip;
            toolTip.Draw += DrawTip;
            owner.Deactivate += OwnerUnavailable;
            owner.Resize += OwnerUnavailable;
            owner.FormClosed += OwnerUnavailable;
            owner.Disposed += OwnerDisposed;
        }

        public Font Font { get { return layout.Font; } set { layout.Font = value; } }
        public Point Location { get { return layout.Location; } set { layout.Location = value; } }
        public int Width { get { return layout.Width; } }
        public Size ClientSize { get { return layout.ClientSize; } }
        public Rectangle Bounds { get { return new Rectangle(Location, ClientSize); } }
        public bool Visible { get { return visible; } }
        public bool IsDisposed { get; private set; }

        public string Description
        {
            get { return description; }
            set {
                Hide();
                description = TrimDescription(value);
                if (!string.IsNullOrEmpty(description)) ShowDeclarationViewWindow();
            }
        }

        private string TrimDescription(string value)
        {
            if (!in_completion_list || value == null) return value;
            int end = value.Length;
            foreach (string tag in new[] { "<returns>", "<params>" }) {
                int index = value.IndexOf(tag, StringComparison.Ordinal);
                if (index >= 0) end = Math.Min(end, index);
            }
            return value.Substring(0, end).TrimEnd();
        }

        public void ShowDeclarationViewWindow()
        {
            if (IsDisposed || owner.IsDisposed || !owner.Visible || string.IsNullOrEmpty(description)) return;
            // Hide before each Show: Mono otherwise hooks owner events again.
            Hide();
            using (Graphics graphics = owner.CreateGraphics())
                TipPainterTools.GetDrawingSizeHelpTipFromCombinedDescription(
                    layout, graphics, Font, null, description, -1, -1, false);
            Rectangle area = Screen.GetWorkingArea(Location);
            Location = new Point(Math.Max(area.Left, Math.Min(Location.X, area.Right - Width)),
                Math.Max(area.Top, Math.Min(Location.Y, area.Bottom - ClientSize.Height)));
            toolTip.Show(description, owner, owner.PointToClient(Location));
            SetVisible(true);
        }

        public void Hide()
        {
            if (IsDisposed) return;
            // In particular, do not ask for owner.Handle from its Disposed event.
            if (!owner.IsDisposed && owner.IsHandleCreated) toolTip.Hide(owner);
            SetVisible(false);
        }

        private void SetVisible(bool value)
        {
            if (visible == value) return;
            visible = value;
            VisibleChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MeasureTip(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = layout.ClientSize;
        }

        private void DrawTip(object sender, DrawToolTipEventArgs e)
        {
            e.Graphics.Clear(SystemColors.Info);
            TipPainterTools.DrawHelpTipFromCombinedDescription(
                layout, e.Graphics, Font, null, description, -1, -1, false);
        }

        private void OwnerUnavailable(object sender, EventArgs e) { Hide(); }
        private void OwnerDisposed(object sender, EventArgs e) { Dispose(); }
        public void CloseDeclarationViewWindow() { Dispose(); }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed) {
                Hide();
                IsDisposed = true;
                owner.Deactivate -= OwnerUnavailable;
                owner.Resize -= OwnerUnavailable;
                owner.FormClosed -= OwnerUnavailable;
                owner.Disposed -= OwnerDisposed;
                toolTip.Dispose();
                layout.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

