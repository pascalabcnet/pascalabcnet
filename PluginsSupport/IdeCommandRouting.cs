using System;

namespace VisualPascalABCPlugins
{
    // Optional extension: existing IWorkbench contracts and legacy commands stay intact.
    public interface IIdeCommandHandler
    {
        // True means accepted, not that an asynchronous compilation has completed.
        bool TryExecute(VisualEnvironmentCompilerAction action, object context);
    }

    public interface IWorkbenchCommandService
    {
        IDisposable RegisterCommandHandler(IIdeCommandHandler handler);
        bool LegacyServicesSuspended { get; set; }
    }

    public sealed class IdeRunCommand
    {
        public ICodeFileDocument Document { get; set; }
        public bool DebugRequested { get; set; }
        public bool RedirectConsoleIO { get; set; }
    }

    public sealed class IdeCommandRouter
    {
        private IIdeCommandHandler handler;

        public IDisposable Register(IIdeCommandHandler value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (handler != null) throw new InvalidOperationException("An IDE command handler is already registered.");
            handler = value;
            return new Registration(() => { if (ReferenceEquals(handler, value)) handler = null; });
        }

        public bool TryExecute(VisualEnvironmentCompilerAction action, object context = null)
        {
            if (action != VisualEnvironmentCompilerAction.Build &&
                action != VisualEnvironmentCompilerAction.Run &&
                action != VisualEnvironmentCompilerAction.Stop) return false;
            return handler?.TryExecute(action, context) == true;
        }

        private sealed class Registration : IDisposable
        {
            private Action release;
            public Registration(Action release) { this.release = release; }
            public void Dispose() { var action = release; release = null; action?.Invoke(); }
        }
    }
}
