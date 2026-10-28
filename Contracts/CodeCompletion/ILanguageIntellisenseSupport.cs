using System;

namespace PascalABCCompiler.Contracts
{
    /// <summary>
	/// Интерфейс, предоставляющий информацию интеллисенсу
	/// </summary>
	public interface ILanguageIntellisenseSupport
    {
        ILanguageInformation LanguageInformation { get; set; }

        /// <summary>
        /// Получить полное описание элемента (в желтой подсказке)
        /// </summary>
        string GetDescription(IBaseScope scope);
        /// <summary>
        /// Получить краткое описание элемента (без ключевых слов)
        /// </summary>
        string GetSimpleDescription(IBaseScope scope);
        /// <summary>
        /// Получить короткое имя откомпилированного типа
        /// </summary>
        string GetShortName(ICompiledTypeScope scope);
        /// <summary>
        /// Получить короткое имя откомпилированного метода
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
    	string GetShortName(ICompiledMethodScope scope);
        /// <summary>
        /// Получить короткое имя откомпилированного конструктора
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
    	string GetShortName(ICompiledConstructorScope scope);
        string GetShortName(IProcScope scope);
        string GetShortTypeName(Type t, bool noalias = true);
        /// <summary>
        /// Получить представление массива рамерности rank
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
    	string GetArrayDescription(string elementType, int rank);
        /// <summary>
        /// Получить длинное имя откомпилированного типа
        /// </summary>
        string GetFullTypeName(ICompiledTypeScope scope, bool no_alias = true);
        string GetSynonimDescription(ITypeScope scope);
        string GetSynonimDescription(ITypeSynonimScope scope);
        string GetSynonimDescription(IProcScope scope);
        string GetKeyword(SymbolKind kind);
        string ConstructOverridedMethodHeader(IProcScope scope, out int off);
        string GetSimpleDescriptionWithoutNamespace(ITypeScope scope);
        string GetClassKeyword(SyntaxTree.class_keyword keyw);

        string[] GetIndexerString(IBaseScope scope);
        /// <summary>
        /// Получить строковое представление символа
        /// </summary>
        string GetStringForChar(char c);
        /// <summary>
        /// Получить строковое представление литерального символа
        /// </summary>
        string GetStringForSharpChar(int num);
        /// <summary>
        /// Получить строковое представление строки
        /// </summary>    	
        string GetStringForString(string s);
        /// <summary>
        /// Сконструировать реализацию метода
        /// </summary>
        string ConstructHeader(string meth, IProcScope scope, int tabCount);
        string ConstructHeader(IProcRealizationScope scope, int tabCount);
        /// <summary>
        /// Получить выражение в строке Text, до смещения off (используется при нажатии точки)
        /// </summary>
        string FindExpression(int off, string Text, int line, int col, out KeywordKind keyw);
        /// <summary>
        /// Получить выражение в строке Text, до смещения off и возможно после смещения до конца идентификатора (используется при наведении мыши)
        /// </summary>
        string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out KeywordKind keyw, out string expr_without_brackets);
        /// <summary>
    	/// Получить выражение в строке Text, до смещения off и возможно после смещения до конца идентификатора (используется при наведении мыши)
    	/// </summary>
    	string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out string expr_without_brackets);
        /// <summary>
        /// Получить выражение в строке Text, до смещения off (используется при нажатии (, [, запятой)
        /// </summary>
        string FindExpressionForMethod(int off, string Text, int line, int col, char pressed_key, ref int num_param);
        /// <summary>
        /// Получить идентификатор
        /// </summary>
        string FindOnlyIdentifier(int off, string Text, int line, int col, ref string name);
        string FindPattern(int off, string Text, out bool is_pattern);
        string GetDocumentTemplate(string lineText, string Text, int line, int col, int pos);

        /// <summary>
        /// Получить минимальный шаблон для модуля с именем unitName
        /// </summary>
        string GetUnitTemplate(string unitName);

        /// <summary>
        /// Проверить на ключевое слово
        /// </summary>
        KeywordKind TestForKeyword(string Text, int i);
        KeywordKind GetKeywordKind(string name);
        /*/// <summary>
    	/// Получить все ключевые слова
    	/// </summary>
    	string[] GetKeywords();
    	/// <summary>
    	/// Получить все ключевые слова такого рода (var, type,...), т.е. при вводе идентификатора после них не выдавать всплывающую подсказку 
    	/// </summary>
    	string[] GetTypeKeywords();*/
        bool IsDefinitionIdentifierAfterKeyword(KeywordKind keyw);
        /// <summary>
        /// Является ли символ разделителем при вызове функции
        /// </summary>
        bool IsMethodCallParameterSeparator(char key);
        /// <summary>
        /// Является ли символ открывающей скобкой при вызове функции
        /// </summary>
        bool IsOpenBracketForMethodCall(char key);
        bool IsOpenBracketForIndex(char key);
        bool IsTypeAfterKeyword(KeywordKind keyw);
        bool IsNamespaceAfterKeyword(KeywordKind keyw);
        string GetStandardTypeByKeyword(KeywordKind keyw);
        /*string GetSystemUnitName();
    	string GetBodyStartBracket();
    	string GetBodyEndBracket();*/
        string SkipNew(int off, string Text, ref KeywordKind keyw);
        //char GetParameterDelimiter();
        string GetCompiledTypeRepresentation(Type t, System.Reflection.MemberInfo mi, ref int line, ref int col);
        bool IsKeyword(string value);

        void RenameOrExcludeSpecialNames(SymInfo[] symInfos);

        bool IsParams(string paramDescription);

        int FindClosingParenthesis(string descriptionAfterOpeningParenthesis, char parenthesis);

        int FindParamDelim(string descriptionAfterOpeningParenthesis, int number);

        int FindParamDelimForIndexer(string descriptionAfterOpeningParenthesis, int number);

        /// <summary>
        /// Вызывать ли преобразователей синтаксического дерева в работе Intellisense
        /// </summary>
        bool ApplySyntaxTreeConvertersForIntellisense { get; }

        string BodyStartBracket
        {
            get;
        }
        string BodyEndBracket
        {
            get;
        }
        string ParameterDelimiter
        {
            get;
        }
        string DelimiterInIndexer
        {
            get;
        }
        string ResultVariableName
        {
            get;
        }
        bool IncludeDotNetEntities
        {
            get;
        }
        bool AddStandardUnitNamesToUserScope
        {
            get;
        }

        /// <summary>
        /// Добавить пространства имен .NET, подключаемые по умолчанию. Полной поддержки false нет (см. комментарий)
        /// </summary>
        // Если поставить false, то CorrectTreeWithSemantic из DomSyntaxTreeVisitor будет поправлять типы неправильно
        // из-за того, что полное имя типа integer, например, System.Int32,
        // а имя System не будет добавлено в Scope и значит выведется Int32 без преобразования в кастомное название типа для языка
        bool AddStandardNetNamespacesToUserScope
        {
            get;
        }

        /// <summary>
        /// Может ли source context функций быть вложен в source context других функций
        /// </summary>
        bool UsesFunctionsOverlappingSourceContext
        {
            get;
        }
    }
}
