// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;
using System;
using System.Threading.Tasks;
using System.IO.Pipes;

namespace LanguageServerEngine
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            PascalABCCompiler.StringResourcesLanguage.LoadDefaultConfig();

            Languages.Integration.LanguageIntegrator.LoadAllLanguages();

            CodeCompletion.DomSyntaxTreeVisitor.use_semantic_for_intellisense = true;

            LanguageServer server;

            if (args.Length > 1)
            {
                // Если клиент общается через stdin/stdout (например, VS Code)
                if (args[0] == "--transport" && args[1] == "stdio")
                {

                    server = new LanguageServer(
                        Console.OpenStandardInput(),
                        Console.OpenStandardOutput(),
                        new LoggerFactory(),
                        true
                    );

                    AddHandlers(server);

                    await server.Initialize();

                    await server.WaitForExit;
                }
                else
                {
                    Console.Error.WriteLine($"Error: Arguments '{args[1]} {args[2]}' are not supported.");
                    Console.Error.WriteLine($"Shutting down...");

                    return;
                }
            }
            else if (args.Length == 1)
            {
                Console.Error.WriteLine($"Error: Wrong number of arguments passed.");
                Console.Error.WriteLine($"Shutting down...");

                return;
            }
            // Иначе сервер создает pipe
            else
            {
                var pipeName = "language-pipe";
                using (var inputPipe = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
                {
                    Console.Error.WriteLine("Waiting for client...");

                    await inputPipe.WaitForConnectionAsync();

                    server = new LanguageServer(
                        inputPipe,
                        inputPipe,
                        new LoggerFactory(),
                        true
                    );

                    AddHandlers(server);

                    await server.Initialize();

                    await server.WaitForExit;
                }
            }
        }

        private static void AddHandlers(LanguageServer server)
        {
            var documentStorage = new DocumentStorage();

            server.AddHandler(new TextDocumentSyncHandler(documentStorage));

            server.AddHandler(new CompletionHandler(documentStorage));

            server.AddHandler(new HoverHandler(documentStorage));

            server.AddHandler(new SignatureHelpHandler(documentStorage));
        }
    }
}
