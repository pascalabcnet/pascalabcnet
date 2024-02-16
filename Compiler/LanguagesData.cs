// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System.Collections.Generic;

namespace PascalABCCompiler
{
    /// <summary>
    /// Класс для хранения данных о сторонних языках, подключаемых через систему плагинов
    /// </summary>
    public static class LanguagesData
    {
        
        // соответствие языков их расширениям
        public static Dictionary<string, string> languagesByExtensions = new Dictionary<string, string>()
        {
            [TreeConverter.compiler_string_consts.pascalSourceFileExtension] = TreeConverter.compiler_string_consts.pascalLanguageName,

            // ...
        };
    }
}
