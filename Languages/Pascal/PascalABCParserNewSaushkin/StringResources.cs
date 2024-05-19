// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.ParserTools;

namespace Languages.Pascal.Frontend
{
    /// <summary>
    /// Строковые ресурсы парсера PascalABC.NET
    /// </summary>
    internal static class StringResources
    {
        private const string prefix = "PASCALABCPARSER_";
        public static string Get(string id)
        {
            return BaseStringResources.Get(id, prefix);
        }
    }
}
