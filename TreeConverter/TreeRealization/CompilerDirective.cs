// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
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