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
