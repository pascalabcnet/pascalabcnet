#if PABC_LINUX_PLUGIN
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace VisualPascalABCPlugins
{
    // Linux uses the common async completion trigger and the host-owned passive popup.
    internal sealed class Net10LinuxLanguageSession : IDisposable
    {
        private readonly Form form;
        private readonly Func<bool> enabled;
        private readonly Timer gate = new Timer { Interval = 500 };
        private readonly Net10LanguageDocuments documents;
        private readonly Net10Completion completion;
        private readonly Net10Signature signature;
        private readonly Net10Hover hover;
        private readonly Net10Navigation navigation;
        private readonly Net10DocumentSymbols symbols;
        private readonly Func<bool> modernMode, symbolsEnabled;
        private readonly Func<bool> hoverEnabled;
        private readonly Func<bool> signatureEnabled;
        private readonly IExternalIntelliSenseSession ui;
        private readonly Func<bool> completionEnabled;
        private readonly ToolStripStatusLabel label;
        private readonly EventHandler applicationExit;
        private bool disposed;
        private volatile string lastStatus;
        internal Net10LanguageDocuments Documents => documents;

        internal Net10LinuxLanguageSession(Form form, IWorkbenchDocumentEvents events, Func<bool> enabled,
            Func<INet10LanguageClient> createClient, IExternalIntelliSenseSession ui = null, Func<bool> completionEnabled = null, Func<bool> signatureEnabled = null, Func<bool> hoverEnabled = null, Action<List<Net10NavigationTarget>> navigate = null,
            IWorkbenchDocumentService documentService = null, IWorkbenchDocumentSymbolsUI symbolsUI = null,
            Func<bool> modernMode = null, Func<bool> symbolsEnabled = null)
        {
            this.form = form; this.enabled = enabled;
            this.modernMode = modernMode ?? enabled;
            this.symbolsEnabled = symbolsEnabled ?? (() => true);
            var strip = FindStatusStrip(form);
            if (strip != null)
            {
                label = new ToolStripStatusLabel {
                    Name = "LinuxLanguageServerStatus", Visible = false,
                    Margin = new Padding(8, 0, 2, 0), Alignment = ToolStripItemAlignment.Right
                };
                strip.Items.Add(label);
            }
            documents = new Net10LanguageDocuments(events, createClient, Report);
            this.ui = ui;
            this.completionEnabled = completionEnabled ?? (() => true);
            this.signatureEnabled = signatureEnabled ?? (() => true);
            this.hoverEnabled = hoverEnabled ?? (() => true);
            if (ui != null)
            {
                completion = new Net10Completion(form, events, documents, () => enabled() && this.completionEnabled(), ui);
                signature = new Net10Signature(form, events, documents, () => enabled() && this.signatureEnabled(), ui);
                completion.Opening += signature.CloseHints;
                hover = new Net10Hover(form, events, documents, () => enabled() && this.hoverEnabled(), ui);
            }
            if (navigate != null)
            {
                navigation = new Net10Navigation(events, documents, enabled, targets => {
                    signature?.CloseHints();
                    ui?.Hide(IntelliSenseHint.Hover);
                    navigate(targets);
                });
                if (ui is IExternalNavigationSession navigationUI)
                    navigationUI.NavigationRequested += navigation.NavigateAtCaret;
            }
            if (documentService != null && symbolsUI != null)
                symbols = new Net10DocumentSymbols(events, documentService, documents, symbolsUI.GetDocumentSymbolsView);
            form.Shown += FormShown;
            applicationExit = (sender, e) => documents.AbortOnApplicationExit();
            Application.ApplicationExit += applicationExit;
            gate.Tick += (sender, e) => UpdateActivity();
            gate.Start();
        }
        internal void UpdateActivity()
        {
            if (disposed || form.IsDisposed) return;
            bool active = enabled();
            documents.SetActive(active);
            completion?.SetActive(active && completionEnabled());
            signature?.SetActive(active && signatureEnabled());
            hover?.SetActive(active && hoverEnabled());
            navigation?.SetActive(active);
            if (navigation != null && ui is IExternalNavigationSession navigationUI)
                navigationUI.NavigationEnabled = active;
            symbols?.SetActive(modernMode(), active && symbolsEnabled());
            if (label != null) label.Visible = active;
        }
        private void FormShown(object sender, EventArgs e) { if (lastStatus != null) Report(lastStatus); }
        private void Report(string text)
        {
            lastStatus = text;
            if (disposed || form.IsDisposed || form.Disposing || !form.IsHandleCreated) return;
            try { form.BeginInvoke(new Action(() => {
                if (disposed || form.IsDisposed || !enabled() || label == null) return;
                label.Text = text.StartsWith("LSP: ошибка", StringComparison.Ordinal) ||
                    text.StartsWith("LSP: связь потеряна", StringComparison.Ordinal) ? "LSP: ошибка" : text;
                label.ToolTipText = text;
            })); }
            catch (InvalidOperationException) { }
        }
        private static StatusStrip FindStatusStrip(Control root)
        {
            if (root is StatusStrip strip) return strip;
            foreach (Control child in root.Controls) { var found = FindStatusStrip(child); if (found != null) return found; }
            return null;
        }
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            gate.Dispose();
            form.Shown -= FormShown;
            completion?.Dispose();
            signature?.Dispose();
            hover?.Dispose();
            navigation?.Dispose();
            symbols?.Dispose();
            ui?.Dispose();
            documents.Dispose();
            // Do not leave a server behind if Mono exits before asynchronous shutdown finishes.
            documents.AbortOnApplicationExit();
            Application.ApplicationExit -= applicationExit;
            label?.Dispose();
        }
    }
}
#endif
