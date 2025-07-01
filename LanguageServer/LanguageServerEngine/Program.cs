// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Linq;
using System.Text;
using System.IO.Pipes;

namespace LanguageServerEngine
{
    internal class Program
    {
        static async Task Main()
        {
            PascalABCCompiler.StringResourcesLanguage.LoadDefaultConfig();

            Languages.Integration.LanguageIntegrator.LoadAllLanguages();

            CodeCompletion.DomSyntaxTreeVisitor.use_semantic_for_intellisense = true;

            var pipeName = "language-pipe";
            var inputPipe = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            Console.Error.WriteLine("Waiting for client...");
            await inputPipe.WaitForConnectionAsync();

            var loggerFactory = new LoggerFactory();

            var server = new LanguageServer(
                    inputPipe,
                    inputPipe,
                    loggerFactory,
                    true
             );

            var documentStorage = new DocumentStorage();

            server.AddHandler(new TextDocumentSyncHandler(documentStorage));

            server.AddHandler(new CompletionHandler(documentStorage));

            server.AddHandler(new HoverHandler(documentStorage));

            server.AddHandler(new SignatureHelpHandler(documentStorage));

            await server.Initialize();

            await server.WaitForExit;
        }
    }
}
