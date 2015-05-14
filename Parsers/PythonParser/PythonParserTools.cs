using System;
using System.Collections.Generic;
using System.Text;
using GPPGTools;

namespace PascalABCCompiler.PythonABCParser
{
    public static class StringResources
    {
        // for getting strings from \bin\Lng\<LNG>\PythonABCParser.dat

        private const string prefix = "PYTHONABCPARSER_";

        public static string Get(string Id)
        {
            string ret = PascalABCCompiler.StringResources.Get(prefix + Id);
            if (ret == prefix + Id)
                return Id;
            return ret;
        }
    }

    class PythonParserTools : GPPGParserTools { }
}
