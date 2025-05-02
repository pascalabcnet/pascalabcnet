using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;
using System;
using System.Threading.Tasks;

namespace LanguageServerEngine
{
    internal class Program
    {
        static async Task Main()
        {
            Languages.Integration.LanguageIntegrator.LoadAllLanguages();

            Console.Error.WriteLine("Language server started ...");

            var loggerFactory = new LoggerFactory();

            var server = new LanguageServer(
                    Console.OpenStandardInput(),
                    Console.OpenStandardOutput(),
                    loggerFactory,
                    true
             );

            /*var server = new LanguageServer(
                new LoggingStream(Console.OpenStandardInput(), loggerFactory.CreateLogger("LSP-IN")),
                new LoggingStream(Console.OpenStandardOutput(), loggerFactory.CreateLogger("LSP-OUT")),
                loggerFactory
            );*/

            var documentStorage = new DocumentStorage();

            server.AddHandler(new TextDocumentSyncHandler(documentStorage));

            server.AddHandler(new CompletionHandler(documentStorage));

            await server.Initialize();

            await server.WaitForExit;
        }

        /*private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DocumentStorage>();
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }*/
    }


    /*public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken ct, RequestHandlerDelegate<TResponse> next)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await next(); // Выполняем обработчик

                // Логируем успешный ответ
                await Console.Error.WriteLineAsync(
                    string.Format(
                        "[SERVER] Response: {0} => {1}\nTime: {2}ms\nData: {3}",
                        typeof(TRequest).Name,
                        typeof(TResponse).Name,
                        stopwatch.ElapsedMilliseconds,
                        JsonSerializer.Serialize(response)
                    )
                );

                return response;
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                await Console.Error.WriteLineAsync(
                    string.Format(
                        "{0}\n[ERROR] {1} failed after {2}ms",
                        ex,
                        typeof(TRequest).Name,
                        stopwatch.ElapsedMilliseconds
                    )
                );

                throw; // Пробрасываем исключение дальше
            }

        }
    }*/

    /*public class LoggingStream : Stream
    {
        private readonly Stream _inner;
        private readonly ILogger _logger;

        public LoggingStream(Stream inner, ILogger logger)
        {
            _inner = inner;
            _logger = logger;
        }

        // Реализуем только необходимые методы
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken ct)
        {
            int read = await _inner.ReadAsync(buffer, offset, count, ct);

            // Логируем сырые данные
            await Console.Error.WriteLineAsync(string.Format("<<< IN [{0} bytes]:\n{1}",
                read,
                BitConverter.ToString(buffer, offset, read)));

            return read;
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken ct)
        {
            // Логируем перед отправкой
            await Console.Error.WriteLineAsync(string.Format(">>> OUT [{0} bytes]:\n{1}",
                count,
                BitConverter.ToString(buffer, offset, count)));

            await _inner.WriteAsync(buffer, offset, count, ct);
        }

        // Обязательные абстрактные методы (минимальная реализация)
        public override bool CanRead => _inner.CanRead;
        public override bool CanSeek => _inner.CanSeek;
        public override bool CanWrite => _inner.CanWrite;
        public override long Length => _inner.Length;
        public override long Position { get => _inner.Position; set => _inner.Position = value; }
        public override void Flush() => _inner.Flush();
        public override int Read(byte[] buffer, int offset, int count) { Console.Error.WriteLine("Sdf"); return _inner.Read(buffer, offset, count); }
        public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
        public override void SetLength(long value) => _inner.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) { Console.Error.WriteLine("Sdf"); _inner.Write(buffer, offset, count); }
    }*/
}
