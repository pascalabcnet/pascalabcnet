using System.Collections.Generic;

namespace PascalABCCompiler.CoreUtils
{
    /// <summary>
    /// Хранилище для сгенерированных компилятором переменных
    /// </summary>
    public static class GeneratedNamesManager
    {
        private static readonly Dictionary<string, int> counters = new Dictionary<string, int>();

        public static void Reset() => counters.Clear();

        /// <summary>
        /// Возвращает уникальное имя для переданного префикса (используется автоинкремент)
        /// </summary>
        public static string GenerateName(string prefix, string suffix = "")
        {
            counters.TryGetValue(prefix, out int current);
            counters[prefix] = current + 1;
            return $"{prefix}{current}{suffix}";
        }
    }
}
