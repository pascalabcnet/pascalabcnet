// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System.Text;
using Languages.Facade;

namespace Languages.Pascal
{
    /// <summary>
    /// Explicit PascalABC.NET language registration for headless applications.
    /// </summary>
    public static class PascalLanguageRegistration
    {
        /// <summary>
        /// Registers PascalABC.NET without scanning for *LanguageInfo.dll files.
        /// Repeated calls return the same language instance.
        /// </summary>
        public static PascalABCLanguage RegisterPascalLanguage()
        {
#if PABCNET_MODERN
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            var languages = LanguageProvider.Instance.Languages;

            lock (languages)
            {
                for (var i = 0; i < languages.Count; i++)
                {
                    var pascalLanguage = languages[i] as PascalABCLanguage;
                    if (pascalLanguage == null)
                        continue;

                    if (i != 0)
                    {
                        languages.RemoveAt(i);
                        languages.Insert(0, pascalLanguage);
                    }

                    return pascalLanguage;
                }

                var newPascalLanguage = new PascalABCLanguage();
                languages.Insert(0, newPascalLanguage);
                return newPascalLanguage;
            }
        }
    }
}
