// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

namespace PascalABCCompiler.TreeRealization
{
    public class compiler_directive
    {
        public string name;
        public string directive;
        public location location;
        public string source_file; // Файл из которого загрузили директиву. В отличии от location - может указывать на .pcu

        public compiler_directive(string name, string directive, location loc, string source_file)
        {
            this.name = name;
            this.directive = directive;
            this.location = loc;
            this.source_file = source_file;
        }
    }
}