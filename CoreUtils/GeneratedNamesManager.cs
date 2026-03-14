// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

// Данный проект не зависит от других проектов
using System.Collections.Generic;

namespace PascalABCCompiler.CoreUtils
{
    /// <summary>
    /// Хранилище для сгенерированных компилятором переменных
    /// </summary>
    public class GeneratedNamesManager
    {
        private readonly Dictionary<string, int> counters = new Dictionary<string, int>();

        /// <summary>
        /// Возвращает уникальное имя для переданного префикса (используется автоинкремент)
        /// </summary>
        public string GenerateName(string prefix, string suffix = "")
        {
            counters.TryGetValue(prefix, out int current);
            counters[prefix] = current + 1;
            return $"{prefix}{current}{suffix}";
        }
    }
}
