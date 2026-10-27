using System.Collections.Generic;

namespace PascalABCCompiler.Contracts
{
    /// <summary>
    /// Информация о языке
    /// </summary>
    public interface ILanguageInformation
    {
        /// <summary>
        /// Название языка
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Версия языка
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Авторское право
        /// </summary>
        string Copyright { get; }

        /// <summary>
        /// Чувствительность к регистру
        /// </summary>
        bool CaseSensitive { get; }

        /// <summary>
        /// Расширения файлов, относящиеся к языку
        /// </summary>
        string[] FilesExtensions { get; }

        /// <summary>
        /// Названия системных модулей (стандартной библиотеки и др.)
        /// </summary>
        string[] SystemUnitNames { get; }

        /// <summary>
        /// Нужно ли вызывать конверторы синтаксического дерева, срабатывающие после компиляции зависимостей
        /// </summary>
        bool SyntaxTreeIsConvertedAfterUsedModulesCompilation { get; }

        /// <summary>
        /// Класс с информацией о ключевых словах языка
        /// </summary>
        BaseKeywords KeywordsStorage
        {
            get;
        }

        /// <summary>
        /// Данные о всех поддерживаемых директивах компилятора
        /// </summary>
        Dictionary<string, IDirectiveInfo> ValidDirectives { get; }

        /// <summary>
        /// Обозначение однострочного комментария (используется в TestRunner)
        /// </summary>
        string CommentSymbol { get; }

        /// <summary>
        /// Имена стандартных модулей, которые не совпадают с именами их файлов.
        /// Можно задавать null.
        /// </summary>
        Dictionary<string, string> SpecialModulesAliases { get; }
    }
}
