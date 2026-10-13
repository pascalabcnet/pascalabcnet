using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VisualPascalABCPlugins
{
    // Optional contract: the IDE owns lifecycle/events; the plugin owns the process.
    public interface IExternalRunService
    {
        IDisposable PrepareExternalCompile(string sourceFileName);
        string PrepareExternalArguments(string arguments);
        IExternalRunSession RegisterExternalRun(ICodeFileDocument document, string outputFile, Action stop);
        string GetExternalOutputFile(ICodeFileDocument document);
    }

    public interface IExternalRunSession : IDisposable
    {
        void Started();
        void WriteOutput(string text);
        void ReportException(RuntimeExceptionInfo error);
    }

    public sealed class ExternalRunSession : IExternalRunSession
    {
        private readonly Action started, finished;
        private readonly Action<RuntimeExceptionInfo> exception;
        private readonly Action<string> output;
        private bool running, disposed;
        public ExternalRunSession(Action started, Action finished, Action<RuntimeExceptionInfo> exception,
            Action<string> output = null)
        {
            this.started = started; this.finished = finished; this.exception = exception;
            this.output = output;
        }
        public void Started()
        {
            if (disposed || running) return;
            running = true;
            started();
        }
        public void ReportException(RuntimeExceptionInfo error)
        {
            if (!disposed && running) exception(error);
        }
        public void WriteOutput(string text) { if (!disposed && running) output?.Invoke(text); }
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            if (running) { running = false; finished(); }
        }
    }

    public sealed class RuntimeStackFrame
    {
        public string FunctionName { get; set; }
        public string FileName { get; set; }
        public int Line { get; set; }
    }

    public sealed class RuntimeExceptionInfo
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string Stack { get; set; }
        public List<RuntimeStackFrame> Frames { get; } = new List<RuntimeStackFrame>();
        public static RuntimeExceptionInfo Parse(string payload)
        {
            int message = payload.IndexOf("[MESSAGE]", StringComparison.Ordinal);
            int stack = payload.IndexOf("[STACK]", StringComparison.Ordinal);
            if (message < 0 || stack < message) throw new FormatException("Invalid runtime exception message.");
            var result = new RuntimeExceptionInfo { Type = payload.Substring(0, message),
                Message = payload.Substring(message + 9, stack - message - 9), Stack = payload.Substring(stack + 7) };
            foreach (string line in result.Stack.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var match = Regex.Match(line, @"\s+(?:in|в)\s+(.+):(?:line|строка)\s+(\d+)\s*$");
                int number;
                bool hasSource = match.Success && int.TryParse(match.Groups[2].Value, out number) && number < 16777214;
                result.Frames.Add(new RuntimeStackFrame { FunctionName = match.Success ? line.Substring(0, match.Index).Trim() : line.Trim(),
                    FileName = hasSource ? match.Groups[1].Value : null,
                    Line = hasSource ? int.Parse(match.Groups[2].Value) : 0 });
            }
            return result;
        }
    }
}
