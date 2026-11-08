using System.Collections.Generic;

namespace PascalABCCompiler.Contracts.Parsers
{
    public interface IKeywords
    {
        /// <summary>
        /// Типы ключевых слов
        /// </summary>
        IDictionary<string, KeywordKind> KeywordKinds { get; }

        /// <summary>
        /// Ключевые слова в контексте Intellisense (множество)
        /// </summary>
        ISet<string> KeywordsForIntellisenseSet { get; }

        /// <summary>
        /// Ключевые слова в контексте Intellisense (список)
        /// </summary>
        IList<string> KeywordsForIntellisenseList { get; }

        /// <summary>
        /// Ключевые слова, соответствующие типам (array, set), либо классу, функции
        /// </summary>
        IList<string> TypeKeywords { get; }

        /// <summary>
        /// Ключевые слова, которые должны восприниматься как функции при парсинге выражений
        /// </summary>
        ISet<string> KeywordsTreatedAsFunctions { get; }

        /// <summary>
        /// Возвращает токен соответствующий ключевому слову, либо токен идентификатора, если такого ключевого слова нет
        /// </summary>
        int KeywordOrIDToken(string keyword);
    }
}
