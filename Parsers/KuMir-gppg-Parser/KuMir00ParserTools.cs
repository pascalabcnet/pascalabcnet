using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using QUT.Gppg;

namespace PascalABCCompiler.KuMir00Parser
{
    // Класс глобальных описаний и статических методов
    // для использования различными подсистемами парсера
    public static class PT // PT - parser tools
    {
        public static List<Error> Errors;
        //public static int max_errors;
        public static string CurrentFileName;
        public static SourceContext ToSourceContext(LexLocation loc)
        {
            if (loc != null)
                return new SourceContext(loc.StartLine, loc.StartColumn + 1, loc.EndLine, loc.EndColumn);
            return null;
        }
        public static void AddError(string message, LexLocation loc)
        {
            Errors.Add(new SyntaxError(message, CurrentFileName, ToSourceContext(loc), null));
        }
    }
}