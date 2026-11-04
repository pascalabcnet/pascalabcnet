using System;
using System.Collections.Generic;
namespace VisualPascalABC
{
    public partial class RunManager
    {
        private readonly Dictionary<string, Action> externalProcesses = new Dictionary<string, Action>(
            System.IO.Path.DirectorySeparatorChar == '\\' ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        public string PrepareExternalArguments(string args)
        {
            ChangeArgsBeforeRun?.Invoke(ref args);
            return args;
        }

        public void ExternalStarted(string fileName, Action stop)
        {
            lock (externalProcesses) externalProcesses.Add(fileName, stop);
            Starting?.Invoke(fileName);
        }

        public void ExternalExited(string fileName)
        {
            bool removed;
            lock (externalProcesses) removed = externalProcesses.Remove(fileName);
            if (removed) Exited?.Invoke(fileName);
        }

        public void ForgetExternalRun(string fileName) { lock (externalProcesses) externalProcesses.Remove(fileName); }

        public void ExternalOutput(string fileName, string text)
        {
            OutputStringReceived?.Invoke(fileName, StreamType.Output, text);
        }
    }
}
