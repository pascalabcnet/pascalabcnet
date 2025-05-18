// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
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
            PascalABCCompiler.StringResourcesLanguage.LoadDefaultConfig();

            Languages.Integration.LanguageIntegrator.LoadAllLanguages();

            Console.Error.WriteLine("Language server started ...");

            var loggerFactory = new LoggerFactory();

            var server = new LanguageServer(
                    Console.OpenStandardInput(),
                    Console.OpenStandardOutput(),
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
