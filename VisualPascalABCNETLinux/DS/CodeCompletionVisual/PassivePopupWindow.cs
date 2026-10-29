using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ICSharpCode.TextEditor.Gui.CompletionWindow
{
    // ToolStripDropDown has ActivateOnShow=false in Mono. Use the passive
    // surface, without Show() entering ToolStrip's keyboard/menu tracker.
    public class PassivePopupWindow : ToolStripDropDown
    {
        protected int ClosedNotifications { get; private set; }
        protected PassivePopupWindow()
        {
            Font = VisualPascalABC.Constants.CompletionFont;
            AutoClose = false;
            AutoSize = false;
            Padding = Padding.Empty;
            Margin = Padding.Empty;
            TabStop = false;
            ShowItemToolTips = false;
            SetStyle(ControlStyles.Selectable, false);
        }

        protected void ShowPassiveWindow() { base.SetVisibleCore(true); }

        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            ClosedNotifications++;
            base.OnClosed(e);
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            base.OnOpening(e);
            // Content is painted or hosted as controls; there are no menu items.
            e.Cancel = false;
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == 0x0021) { // WM_MOUSEACTIVATE
                message.Result = new IntPtr(3); // MA_NOACTIVATE: still deliver the click.
                return;
            }
            base.WndProc(ref message);
        }

        // Clicking empty space must not invoke ToolStrip's menu-selection logic.
        protected override void OnMouseDown(MouseEventArgs e) { }
        protected override void OnMouseUp(MouseEventArgs e) { Capture = false; }
    }

    internal sealed class PassiveCompletionScrollBar : VScrollBar
    {
        internal PassiveCompletionScrollBar()
        {
            TabStop = false;
            SetStyle(ControlStyles.Selectable, false);
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == 0x0021) {
                message.Result = new IntPtr(3);
                return;
            }
            base.WndProc(ref message);
        }
    }
}
