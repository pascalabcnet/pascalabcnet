// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using PascalABCCompiler.TreeRealization;
namespace PascalABCCompiler.TreeConverter
{
    public enum VariableInitializationParams { ConstantOnly, ConstructorCall, Expression }
    public enum PoinerRealization { VoidStar, IntPtr}
    public static class SemanticRulesConstants
    {
        /// <summary>
        /// Определять методы в константных записях
        /// </summary>
        public static bool DefineMethodsInConstantRecord = false;
        
        /// <summary>
        /// Наследование константных записей
        /// </summary>
        public static bool InheritanceConstantRecord = false;
        
        /// <summary>
        /// Инициализировать строку пустой срокой
        /// </summary>
        public static bool InitStringAsEmptyString = true;

        /// <summary>
        /// Делать строки с быстрым доступом к символам на запись, но со ссылочной семантикой
        /// </summary>
        public static bool FastStrings = false;

        /// <summary>
        /// Индексировать строки с 0
        /// </summary>
        public static bool ZeroBasedStrings = false;
        
        /// <summary>
        /// Строгая проверка типа указателя
        /// </summary>
        public static bool StrongPointersTypeCheckForDotNet = true;
        
        /// <summary>
        /// Использовать is as для указателей
        /// </summary>
        public static bool IsAsForPointers = false;
        
        /// <summary>
        /// Чуствительность к регистру таблицы символов
        /// </summary>
        public static bool SymbolTableCaseSensitive = false;
        
        /// <summary>
        /// Разрешить использование операторов += -= /= *= для приметивных типов
        /// </summary>
        public static bool UseExtendedAssignmentOperatorsForPrimitiveTypes = true;
        
        /// <summary>
        /// Разрешить оператор /= для целых
        /// </summary>
        public static bool UseDivisionAssignmentOperatorsForIntegerTypes = false;
        
        /// <summary>
        /// Разрешить инициализацию списка переменных одним инициализатором
        /// </summary>
        public static bool ManyVariablesOneInitializator = false;
        
        /// <summary>
        /// Разрешить указатели на аргументы шаблона
        /// </summary>
        public static bool AllowPointersForGenericParameters = true;

        /// <summary>
        /// Разрешить запись аргументов шаблона в файл
        /// </summary>
        public static bool AllowGenericParametersForFiles = true;

        /// <summary>
        /// Отключить накопление ошибок
        /// </summary>
        public static bool ThrowErrorWithoutSave = true;

        /// <summary>
        /// Добавлять переменную result в функции
        /// </summary>
        public static bool AddResultVariable = true;

        //!PoinerRealization! in NetGenerator\Helpers.cs
        public static PoinerRealization PoinerRealization = PoinerRealization.VoidStar;
        public static VariableInitializationParams VariableInitializationParams = VariableInitializationParams.Expression;

        /// <summary>
        /// Запертить объявление переменной в блоке если ее имя совпадает с переменной объявленой в объемлющем блоке
        /// </summary>
        public static bool DisabledDefinitionBlockVariablesWithSameNameThatInAboveScope = true;

        /// <summary>
        /// Разрешить неазависимость от порядка объявления методов
        /// </summary>
        public static bool OrderIndependedMethodNames = true;

        /// <summary>
        /// Разрешить неазависимость от порядка объявления функций
        /// </summary>
        public static bool OrderIndependedFunctionNames = false;
        
        /// <summary>
        /// Разрешить неазависимость от порядка объявления типов
        /// </summary>
        public static bool OrderIndependedTypeNames = false;

        /// <summary>
        /// Разрешить процедуру exit
        /// </summary>
        public static bool EnableExitProcedure = true;

        /// <summary>
        /// Инициализировать переменные типа аргументов управляемых шаблонов
        /// </summary>
        public static bool RuntimeInitVariablesOfGenericParameters = true;

        /// <summary>
        /// Разрешить неявное преобразование нетипизированного указателя к типизированному
        /// </summary>
        public static bool ImplicitConversionFormPointerToTypedPointer = true;


        public static type_node ClassBaseType = null;
        public static type_node StringType = null;
        public static type_node StructBaseType = null;
        
        public static bool AllowUseFormatExprAnywhere = false;
		public static bool AllowChangeLoopVariable = true;

		public static bool AllowGlobalVisibilityForPABCDll = true; // SSM не знаю, зачем понадобилось по умолчанию присваивать false. Сделал по умолчанию True. Исправляется тем самым баг  #1575
        // !!! Не присваивать false! Иначе ложится компиляция библиотек, использующих модули, и наоборот!!!

        public static bool AddressOfOperatorNonOverloaded=true;

        public static bool GenerateNativeCode = false;
    }
}
