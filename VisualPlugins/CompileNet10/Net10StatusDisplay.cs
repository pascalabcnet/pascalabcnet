using System;
using System.Windows.Forms;

namespace VisualPascalABCPlugins
{
    internal sealed class Net10StatusDisplay : IDisposable
    {
        private readonly Form form;
        private readonly ToolStripStatusLabel label;
        private string lastText;
        private bool owned;
        private bool writing;
        private bool disposed;

        public Net10StatusDisplay(Form form)
        {
            this.form = form;
            var strips = form.Controls.Find("statusStrip1", true);
            if (strips.Length == 1 && strips[0] is StatusStrip strip)
                label = strip.Items["toolStripStatusLabel5"] as ToolStripStatusLabel;
            if (label != null) label.TextChanged += TextChanged;
        }

        private void TextChanged(object sender, EventArgs e)
        {
            // Normal compilation/run may take over the shared status line.
            // Do not replace its newer messages with a late net10 completion.
            if (!writing && label.Text != lastText) owned = false;
        }

        public void Start(string text)
        {
            owned = true;
            Update(text);
        }

        public void Update(string text)
        {
            if (disposed || form.IsDisposed || label == null) return;
            if (form.InvokeRequired)
            {
                try { form.BeginInvoke(new Action(() => Update(text))); }
                catch (InvalidOperationException) { }
                return;
            }
            if (!owned) return;
            writing = true;
            try { lastText = text; label.Text = text; }
            finally { writing = false; }
        }

        public void Dispose()
        {
            disposed = true;
            if (label != null) label.TextChanged -= TextChanged;
        }
    }
}
