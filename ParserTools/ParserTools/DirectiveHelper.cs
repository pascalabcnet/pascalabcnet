using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using PascalABCCompiler.Errors;

namespace PascalABCCompiler.ParserTools.Directives
{
    /// <summary>
    /// Класс для хранения информации о директиве компилятора (количество параметров, проверки для параметров и др.)
    /// </summary>
    public class DirectiveInfo
    {
        private readonly ParamChecksCollection checks;

        public int[] paramsNums = new int[1] { 1 }; // по умолчанию один параметр
        
        public bool checkParamsNumNeeded; // false, если параметров неограниченное кол-во
        
        public bool quotesAreSpecialSymbols; // true, если нужно ставить кавычки для объединения нескольких слов в одно (как в пути к файлу с пробелами)

        // по умолчанию никаких проверок параметров, но включена проверка их кол-ва
        public DirectiveInfo(ParamChecksCollection paramChecks = null, bool quotesAreSpecialSymbols = false, bool checkParamsNumNeeded = true, int[] paramsNums = null)
        {
            this.checks = paramChecks;
            this.quotesAreSpecialSymbols = quotesAreSpecialSymbols;
            if (paramsNums != null)
                this.paramsNums = paramsNums;
            this.checkParamsNumNeeded = checkParamsNumNeeded;
        }

        /// <summary>
        /// Функция для проверки всех параметров директивы
        /// </summary>
        public bool ParamsValid(List<string> directiveParams, out int indexOfMismatch, out string errorMessage)
        {
            // если проверки не нужны
            if (checks == null)
            {
                indexOfMismatch = -1;
                errorMessage = null;
                return true;
            }
            return checks.CheckParams(directiveParams, out indexOfMismatch, out errorMessage);
        }
    }

    /// <summary>
    /// Базовый класс для всех проверок параметров директив
    /// </summary>
    public abstract class ParamCheckBase
    {
        public int paramsToCheckNum; // кол-во параметров, проверяемых одной проверкой, по умолчанию - 1

        public string errorMessage = "";

        public ParamCheckBase(int paramsToCheckNum = 1)
        {
            this.paramsToCheckNum = paramsToCheckNum;
        }

        /// <summary>
        /// Проверка одного параметра
        /// </summary>
        public abstract bool CheckParam(string param);

        /// <summary>
        /// Проверка нужного кол-ва параметров
        /// </summary>
        public bool CheckParams(string[] directiveParams, out int indexOfMismatch)
        {
            for (var i = 0; i < directiveParams.Length; i++)
            {
                if (!CheckParam(directiveParams[i]))
                {
                    indexOfMismatch = i;
                    return false;
                }
            }

            indexOfMismatch = -1;
            return true;
        }
    }

    /// <summary>
    /// Класс для пользовательской проверки параметров (можно передать лямбду с проверкой)
    /// </summary>
    public sealed class FuncCheck : ParamCheckBase
    {
        private readonly Predicate<string> check;

        public FuncCheck(Predicate<string> pred, string errorMessage = "", int paramsToCheckNum = 1) : base(paramsToCheckNum)
        {
            this.errorMessage = errorMessage;
            check = pred;
        }

        public override bool CheckParam(string s) => check(s);
    }

    /// <summary>
    /// Проверка, что параметр имеет формат идентификатора
    /// </summary>
    public class IsValidIdentifierCheck : ParamCheckBase
    {

        public IsValidIdentifierCheck(int paramsToCheckNum = 1) : base(paramsToCheckNum)
        {
            errorMessage = ParserErrorsStringResources.Get("EXPECTED_IDENTIFIER");
        
        }

        public override bool CheckParam(string param)
        {
            return Regex.IsMatch(param, @"^[\p{L}_][\p{L}0-9_]*$", RegexOptions.Compiled);
        }
    }

    /// <summary>
    /// Проверка принадлежности параметра списку возможных вариантов
    /// </summary>
    public class IsAnyOfCheck : ParamCheckBase
    {
        private readonly string[] paramVariants;

        public IsAnyOfCheck(int paramsToCheckNum = 1, params string[] paramVariants) : base(paramsToCheckNum)
        {
            this.paramVariants = paramVariants;
            
            if (paramVariants.Length <= 5)
            {
                string firstPartOfError = ParserErrorsStringResources.Get("AVAILABLE_PARAM_VARIANTS{0}");
                errorMessage = string.Format(firstPartOfError, string.Join(", ", paramVariants));
            }
            else
            {
                errorMessage = ParserErrorsStringResources.Get("SEE_THE_HELP_FOR_VARIANTS");
            }
        }

        public override bool CheckParam(string param)
        {
            return paramVariants.Contains(param.ToLower());
        }
    }

    /// <summary>
    /// Проверка, что расширение параметра входит в список возможных расширений
    /// </summary>
    public class IsAnyExtensionsOfCheck : ParamCheckBase
    {
        private readonly string[] extVariants;

        public IsAnyExtensionsOfCheck(int paramsToCheckNum = 1, params string[] extVariants) : base(paramsToCheckNum)
        {
            this.extVariants = extVariants;

            string firstPartOfError = ParserErrorsStringResources.Get("AVAILABLE_EXT_VARIANTS{0}");
            errorMessage = string.Format(firstPartOfError, string.Join(", ", extVariants));
        }

        public override bool CheckParam(string param)
        {
            return extVariants.Contains(Path.GetExtension(param.ToLower()));
        }
    }

    /// <summary>
    /// Класс для хранения всех проверок одной директивы 
    /// </summary>
    public class ParamChecksCollection
    {
        private readonly ParamCheckBase[] checks;

        public ParamChecksCollection(params ParamCheckBase[] checks)
        {
            this.checks = checks;
        }

        public bool CheckParams(List<string> directiveParams, out int indexOfMismatch, out string errorMessage)
        {
            int directivesChecked = 0;
            for (var i = 0; i < checks.Length; i++)
            {
                // каждая проверка работает с кол-вом параметров, которое в ней указано
                if (!checks[i].CheckParams(directiveParams.Skip(directivesChecked).Take(checks[i].paramsToCheckNum).ToArray(), out int index))
                {
                    indexOfMismatch = index;
                    errorMessage = checks[i].errorMessage;
                    return false;
                }
                directivesChecked += checks[i].paramsToCheckNum;
            }

            indexOfMismatch = -1;
            errorMessage = null;
            return true;
        }
    }

    /// <summary>
    /// Класс вспомогательных методов для работы с директивами
    /// </summary>
    public static class DirectiveHelper
    {
        #region SINGLE CHECKS WRAPPERS
        public static ParamChecksCollection SingleAnyOfCheck(params string[] paramVariants)
        {
            return new ParamChecksCollection(new IsAnyOfCheck(1, paramVariants));
        }

        public static ParamChecksCollection SingleAnyExtOfCheck(params string[] extVariants)
        {
            return new ParamChecksCollection(new IsAnyExtensionsOfCheck(1, extVariants));
        }

        public static ParamChecksCollection SingleIsValidIdCheck() 
        {
            return new ParamChecksCollection(new IsValidIdentifierCheck(1));
        }

        public static ParamChecksCollection IsValidVersionCheck()
        {
            return new ParamChecksCollection(new FuncCheck(s => Regex.IsMatch(s, @"^\d+\.\d+\.\d+$"), ParserErrorsStringResources.Get("INVALID_VERSION")));
        }
        #endregion

        public static DirectiveInfo NoParamsDirectiveInfo()
        {
            return new DirectiveInfo(paramsNums: new int[1] { 0 });
        }
    }
}

