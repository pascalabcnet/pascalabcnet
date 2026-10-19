using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VisualPascalABCPlugins
{
    internal sealed class Net10StatusDisplay : IDisposable
    {
        internal const long RestoreProgressDelayMilliseconds = 2000;
        private readonly Form form;
        private readonly ToolStripStatusLabel label;
        private string lastText;
        private bool owned;
        private bool writing;
        private bool disposed;
        private readonly Dictionary<string, RestoreStatus> restores = new Dictionary<string, RestoreStatus>();
        private readonly Func<string, string> resource;
        private readonly StatusStrip strip;
        private ToolStripProgressBar indicator;
        private ToolStripButton cancel;
        private string savedStatus, shownRestore;
        private bool compilationActive;
        private bool languageVisible = true;
        private string savedToolTip;
        private ToolStripButton restorePackages;
        private Func<bool> packagesRequired;
        private bool manualRestoreActive;
        private Action restoreAction;

        internal void SetPackageRestoreRequired(Func<bool> required, Action action)
        {
            packagesRequired = required;
            restoreAction = action;
            RenderRestore();
        }

        internal void SetManualRestoreActive(bool active)
        {
            manualRestoreActive = active;
            RenderRestore();
        }

        private void RenderRestoreButton(bool restoring)
        {
            // Compilation/manual restore may finish between timer ticks. Read the
            // current editor's state here, not a snapshot taken before its refresh.
            bool visible = languageVisible && !compilationActive && !restoring && !manualRestoreActive &&
                packagesRequired?.Invoke() == true;
            if (visible && restorePackages == null)
            {
                restorePackages = new ToolStripButton(resource("NUGET_RESTORE_PACKAGES")) {
                    Name = "Net10NuGetRestore", DisplayStyle = ToolStripItemDisplayStyle.Text,
                    ToolTipText = resource("NUGET_RESTORE_PACKAGES_HINT") };
                restorePackages.Click += (sender, e) => restoreAction?.Invoke();
                strip.Items.Insert(strip.Items.IndexOf(label) + 1, restorePackages);
            }
            if (restorePackages != null) restorePackages.Visible = visible;
        }

        private sealed class RestoreStatus
        {
            internal Net10RestoreProgress Progress;
            internal Action Cancel;
            internal bool Cancelling;
        }

        public Net10StatusDisplay(Form form, Func<string, string> resource = null)
        {
            this.form = form;
            this.resource = resource ?? new Func<string, string>(key => key);
            var strips = form.Controls.Find("statusStrip1", true);
            if (strips.Length == 1 && strips[0] is StatusStrip found)
            {
                strip = found;
                label = strip.Items["toolStripStatusLabel5"] as ToolStripStatusLabel;
            }
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
            BeginOperation();
            Update(text);
        }

        public void StartReady(bool modern, string readyText)
        {
            Start(modern ? ".NET 10: " + readyText : readyText);
        }

        // Claim the next compilation's messages without adding a transient status.
        public void BeginOperation() { owned = true; }

        // Called on the UI thread. The normal Run/Stop command is deliberately unrelated.
        internal void SetCompilationActive(bool active)
        {
            compilationActive = active;
            RenderRestore();
        }

        internal void SetLanguageRestoreVisible(bool visible)
        {
            languageVisible = visible;
            RenderRestore();
        }

        internal void ShowRestoreProgress(string source, Net10RestoreProgress value, Action cancelAction)
        {
            if (disposed || label == null || string.IsNullOrEmpty(value.operationId)) return;
            string key = source + ":" + value.operationId;
            if (value.stage == "end") restores.Remove(key);
            else if (value.stage == "begin" || restores.ContainsKey(key))
            {
                if (!restores.TryGetValue(key, out var state)) restores[key] = state = new RestoreStatus();
                state.Progress = value;
                state.Cancel = cancelAction;
            }
            RenderRestore();
        }

        internal void ClearRestores(string source)
        {
            foreach (string key in new List<string>(restores.Keys))
                if (key.StartsWith(source + ":", StringComparison.Ordinal)) restores.Remove(key);
            RenderRestore();
        }

        private void RenderRestore()
        {
            if (disposed || label == null || form.IsDisposed) return;
            RestoreStatus selected = null;
            foreach (var pair in restores)
                if (pair.Value.Progress.elapsedMilliseconds >= RestoreProgressDelayMilliseconds &&
                    (pair.Key.StartsWith("compiler:", StringComparison.Ordinal) || (!compilationActive && languageVisible)))
                { selected = pair.Value; break; }
            RenderRestoreButton(selected != null);
            if (selected == null)
            {
                if (shownRestore != null && label.Text == shownRestore) WriteLabel(savedStatus);
                if (shownRestore != null) label.ToolTipText = savedToolTip;
                shownRestore = null;
                if (indicator != null)
                {
                    strip.Items.Remove(indicator); indicator.Dispose(); indicator = null;
                    strip.Items.Remove(cancel); cancel.Dispose(); cancel = null;
                }
                return;
            }
            if (shownRestore == null) { savedStatus = label.Text; savedToolTip = label.ToolTipText; }
            if (indicator == null)
            {
                indicator = new ToolStripProgressBar { Name = "Net10NuGetProgress", Width = 65,
                    Style = ProgressBarStyle.Marquee, MarqueeAnimationSpeed = 30 };
                cancel = new ToolStripButton(resource("NUGET_CANCEL")) { Name = "Net10NuGetCancel",
                    DisplayStyle = ToolStripItemDisplayStyle.Text };
                cancel.Click += (sender, e) => CancelShownRestore();
                int position = strip.Items.IndexOf(label) + 1;
                strip.Items.Insert(position, indicator);
                strip.Items.Insert(position + 1, cancel);
            }
            cancel.Visible = selected.Progress.elapsedMilliseconds >= 5000;
            cancel.Enabled = !selected.Cancelling;
            string fullText = selected.Cancelling ? resource("NUGET_CANCELLING") : selected.Progress.Format(resource);
            // Keep the existing strip single-line even with very long package IDs.
            // Time/count precede the optional identity; the full value stays in a tooltip.
            strip.PerformLayout();
            shownRestore = FitSingleLine(fullText, label.Font, label.Width);
            label.ToolTipText = fullText;
            WriteLabel(shownRestore);
        }

        internal static string FitSingleLine(string text, System.Drawing.Font font, int width)
        {
            text = text.Replace('\r', ' ').Replace('\n', ' ');
            if (TextRenderer.MeasureText(text, font).Width <= width) return text;
            int left = 0, right = text.Length;
            while (left < right)
            {
                int middle = (left + right + 1) / 2;
                if (TextRenderer.MeasureText(text.Substring(0, middle) + "…", font).Width <= width) left = middle;
                else right = middle - 1;
            }
            return text.Substring(0, left) + "…";
        }

        private void CancelShownRestore()
        {
            foreach (var pair in restores)
                if ((pair.Key.StartsWith("compiler:", StringComparison.Ordinal) || (!compilationActive && languageVisible)) &&
                    pair.Value.Progress.elapsedMilliseconds >= 5000 && !pair.Value.Cancelling)
                {
                    pair.Value.Cancelling = true;
                    RenderRestore();
                    pair.Value.Cancel?.Invoke();
                    break;
                }
        }

        private void WriteLabel(string text)
        {
            writing = true;
            try { label.Text = text; }
            finally { writing = false; }
        }

        // Only errors are visible, in the shared left-hand status field.
        public void ShowLanguageStatus(string text)
        {
            if (disposed || form.IsDisposed) return;
            bool failed = text.StartsWith("LSP: ошибка — ", StringComparison.Ordinal) ||
                text.StartsWith("LSP: связь потеряна — ", StringComparison.Ordinal);
            if (failed) Start(text);
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
            if (shownRestore != null) { savedStatus = lastText = text; return; }
            writing = true;
            try { lastText = text; label.Text = text; }
            finally { writing = false; }
        }

        public void Dispose()
        {
            disposed = true;
            if (indicator != null) { strip.Items.Remove(indicator); indicator.Dispose(); }
            if (cancel != null) { strip.Items.Remove(cancel); cancel.Dispose(); }
            if (restorePackages != null) { strip.Items.Remove(restorePackages); restorePackages.Dispose(); }
            if (label != null) label.TextChanged -= TextChanged;
        }
    }
}
