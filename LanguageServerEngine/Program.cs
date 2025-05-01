using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageServerEngine
{
    internal class Program
    {
        static async Task Main()
        {
            Languages.Integration.LanguageIntegrator.LoadAllLanguages();

            await Console.Error.WriteLineAsync("Language server started ...");

            var server = await LanguageServer.From(options =>
                options
                    .WithInput(Console.OpenStandardInput())
                    .WithOutput(Console.OpenStandardOutput())
                    //.WithLoggerFactory(new LoggerFactory())
                    //.ConfigureLogging(
                    //    logging =>
                    //    {
                    //        logging.SetMinimumLevel(LogLevel.Trace);
                    //        // logging.AddLanguageProtocolLogging();
                    //    }
                    //)
                    //.WithServices(x => x.AddLogging(b => b.SetMinimumLevel(LogLevel.Trace)))
                    .WithServices(ConfigureServices)
                    .WithHandler<TextDocumentSyncHandler>()
                    // .WithHandler<CompletionHandler>()
             );

            await server.WaitForExit;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DocumentStorage>();
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            // services.AddSingleton<TextDocumentSyncHandler>();
        }
    }


    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
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
    }
}
