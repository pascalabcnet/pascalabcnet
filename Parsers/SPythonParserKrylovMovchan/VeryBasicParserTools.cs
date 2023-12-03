using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUT.Gppg;

using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using UniversalParserHelper;

namespace VeryBasicParser
{
    public static class StringResources
    {
        private static string prefix = "PASCALABCPARSER_";
        public static string Get(string Id)
        {
            string ret = PascalABCCompiler.StringResources.Get(prefix + Id);
            if (ret == prefix + Id)
                return Id;
            else
                return ret;
        }
    }
    public class VeryBasicParserTools: UniversalParserHelper.UniversalParserHelper
    {
    }
}
