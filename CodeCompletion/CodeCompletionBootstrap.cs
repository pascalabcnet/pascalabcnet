// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Globalization;

namespace CodeCompletion
{
    /// <summary>
    /// Initializes the process-wide state required by headless code completion clients.
    /// </summary>
    public static class CodeCompletionBootstrap
    {
        private static readonly object initializationLock = new object();

        /// <summary>
        /// Initializes code completion using the current UI culture.
        /// </summary>
        public static void Initialize()
        {
            Initialize(null);
        }

        /// <summary>
        /// Initializes code completion using the requested documentation language.
        /// Values such as "ru", "ru-RU", "en" and "en-US" are accepted.
        /// </summary>
        public static void Initialize(string languageIso)
        {
            var resolvedLanguageIso = ResolveLanguageIso(languageIso);

            lock (initializationLock)
            {
                if (CodeCompletionController.comp == null)
                    CodeCompletionController.comp = new PascalABCCompiler.Compiler();

                CodeCompletionController.currentLanguageISO = resolvedLanguageIso;
                PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO = resolvedLanguageIso;
            }
        }

        private static string ResolveLanguageIso(string languageIso)
        {
            try
            {
                var culture = string.IsNullOrWhiteSpace(languageIso)
                    ? CultureInfo.CurrentUICulture
                    : CultureInfo.GetCultureInfo(languageIso);
                var resolvedLanguageIso = culture.TwoLetterISOLanguageName;

                if (!string.IsNullOrWhiteSpace(resolvedLanguageIso))
                {
                    resolvedLanguageIso = resolvedLanguageIso.ToLowerInvariant();
                    if (resolvedLanguageIso.Length == 2 &&
                        resolvedLanguageIso != "iv" &&
                        char.IsLetter(resolvedLanguageIso[0]) &&
                        char.IsLetter(resolvedLanguageIso[1]))
                    {
                        return resolvedLanguageIso;
                    }
                }
            }
            catch (CultureNotFoundException)
            {
            }

            return "en";
        }
    }
}
