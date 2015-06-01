// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PascalABCCompiler.TreeRealization  //lroman//
{
    public class lambda_node
    {
        private SymbolTable.Scope _scope;

        public SymbolTable.Scope scope
        {
            get
            {
                return _scope;
            }
            set
            {
                _scope = value;
            }
        }

        public lambda_node(SymbolTable.Scope _scope)
        {
            this._scope = _scope;
        }
    }
}
