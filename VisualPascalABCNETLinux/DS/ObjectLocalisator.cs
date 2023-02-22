// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;

namespace VisualPascalABC
{
    class ObjectLocalisator
    {
        private object vaule;
        private string str;
        public ObjectLocalisator(object val, string str)
        {
            vaule = val;
            this.str = PascalABCCompiler.StringResources.Get(str);
        }
        public object Value
        {
            get { return vaule; }
        }
        public override string ToString()
        {
            return str;
        }
    }
}
