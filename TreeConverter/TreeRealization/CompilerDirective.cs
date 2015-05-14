using System;

using PascalABCCompiler.Collections;

namespace PascalABCCompiler.TreeRealization
{
    public class compiler_directive
    {
        public string name;
        public string directive;
        public location location;
        public compiler_directive(string name,string directive,location loc)
        {
            this.name = name;
            this.directive = directive;
            this.location = loc;
        }
    }
}