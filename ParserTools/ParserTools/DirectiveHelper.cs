using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using PascalABCCompiler.Errors;

namespace PascalABCCompiler.ParserTools.Directives
{
    public class DirectiveInfo
    {
        private ParamChecksCollection checks;

        public int[] paramsNums = new int[1] { 1 };
        public bool checkParamsNumNeeded;

        public DirectiveInfo(ParamChecksCollection paramChecks = null, bool checkParamsNumNeeded = true, int[] paramsNums = null)
        {
            this.checks = paramChecks;
            if (paramsNums != null)
                this.paramsNums = paramsNums;
            this.checkParamsNumNeeded = checkParamsNumNeeded;
        }

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

    public abstract class ParamCheckBase
    {
        public int paramsToCheckNum;

        public string errorMessage = "";

        public ParamCheckBase(int paramsToCheckNum = 1)
        {
            this.paramsToCheckNum = paramsToCheckNum;
        }

        public abstract bool CheckParam(string param);

        public bool CheckParams(List<string> directiveParams, out int indexOfMismatch)
        {
            for (var i = 0; i < directiveParams.Count; i++)
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

        public static ParamCheckBase operator +(ParamCheckBase left, ParamCheckBase right)
        {
            return new FuncCheck(s => left.CheckParam(s) || right.CheckParam(s));
        }

        public static ParamCheckBase operator *(ParamCheckBase left, ParamCheckBase right)
        {
            return new FuncCheck(s => left.CheckParam(s) && right.CheckParam(s));
        }
    }

    public sealed class FuncCheck : ParamCheckBase
    {
        Predicate<string> check;

        public FuncCheck(Predicate<string> pred, string errorMessage = "", int paramsToCheckNum = 1) : base(paramsToCheckNum)
        {
            this.errorMessage = errorMessage;
            check = pred;
        }

        public override bool CheckParam(string s) => check(s);
    }

    public class IsValidIdentifierCheck : ParamCheckBase
    {

        public IsValidIdentifierCheck(int paramsToCheckNum = 1) : base(paramsToCheckNum)
        {
            errorMessage = ParserErrorsStringResources.Get("EXPECTED_IDENTIFIER");
        
        }

        public override bool CheckParam(string param)
        {
            return Regex.IsMatch(param, @"^[\p{L}_][\p{L}0-9_]*$");
        }
    }

    public class IsAnyOfCheck : ParamCheckBase
    {
        private string[] paramVariants;

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

    public class IsAnyExtensionsOfCheck : ParamCheckBase
    {
        private string[] extVariants;

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


    public class IsWordCheck : ParamCheckBase
    {
        public IsWordCheck(int paramsToCheckNum = 1) : base(paramsToCheckNum) 
        { 
            errorMessage = ParserErrorsStringResources.Get("EXPECTED_WORD");
        }

        public override bool CheckParam(string param)
        {
            return Regex.IsMatch(param, @"^\w+$");
        }
    }

    public class ParamChecksCollection
    {
        private readonly List<ParamCheckBase> checks = new List<ParamCheckBase>();

        public ParamChecksCollection(params ParamCheckBase[] checks)
        {
            this.checks = checks.ToList();
        }

        public bool CheckParams(List<string> directiveParams, out int indexOfMismatch, out string errorMessage)
        {
            for (var i = 0; i < checks.Count; i++)
            {
                if (!checks[i].CheckParams(directiveParams.Take(checks[i].paramsToCheckNum).ToList(), out int index))
                {
                    indexOfMismatch = index;
                    errorMessage = checks[i].errorMessage;
                    return false;
                }
            }

            indexOfMismatch = -1;
            errorMessage = null;
            return true;
        }
    }


    public static class DirectiveHelper
    {
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
    }
}

