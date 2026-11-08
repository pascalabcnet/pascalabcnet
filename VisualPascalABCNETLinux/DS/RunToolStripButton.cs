using System;
using System.Windows.Forms;

namespace VisualPascalABC
{
    internal sealed class RunToolStripButton : ToolStripButton
    {
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            ToolStrip parent = GetCurrentParent();
            // Mono ignores MouseLeave while the button is disabled during execution.
            // On re-enabling, clear the old hover/pressed state unless it is still real.
            if (Enabled && parent != null && !parent.ContainsFocus &&
                !Bounds.Contains(parent.PointToClient(Cursor.Position)))
            {
                base.OnMouseLeave(EventArgs.Empty);
                Invalidate();
            }
        }
    }
}
