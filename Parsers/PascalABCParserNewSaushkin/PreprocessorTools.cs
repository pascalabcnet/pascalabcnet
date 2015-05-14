using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using QUT.Gppg;

namespace GPPGPreprocessor3
{
    public class Directive
    {
        public string text;
        public QUT.Gppg.LexLocation loc;
        public static List<Directive> dirs = new List<Directive>();
        public Directive(string text, QUT.Gppg.LexLocation loc)
        {
            this.text = text;
            this.loc = loc;
            dirs.Add(this);
        }
        public override string ToString()
        {
            return String.Format("({0},{1})-({2},{3}) {4}",loc.StartLine,loc.StartColumn,loc.EndLine,loc.EndColumn,text);
        }
    }

    public static class PreprocessorTools
    {
        public static compiler_directive MakeDirective(string text, LexLocation loc)
        {
            string name = "";
            string directive = "";

            if (text != null)
            {
                var ind = text.IndexOf(' ');
                if (ind == -1)
                    name = text;
                else
                {
                    name = text.Substring(0, ind);
                    directive = text.Substring(ind+1);
                    if (directive.StartsWith("'"))
                        directive = directive.Remove(0,1);
                    if (directive.EndsWith("'"))
                        directive = directive.Substring(0, directive.Length - 1);
                }
            }

            return new compiler_directive(new token_info(name), new token_info(directive), loc);
        }
    }
}
