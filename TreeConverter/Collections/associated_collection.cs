// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace PascalABCCompiler.Collections
{
    /*
    public class associated_collection<KeyType,ElementType>
    {
        private System.Collections.Generic.Dictionary<KeyType, ElementType> int_collection =
            new System.Collections.Generic.Dictionary<KeyType, ElementType>();

        public void AssociateElement(KeyType key, ElementType element)
        {
#if DEBUG
            if (int_collection[key]!=null)
            {
                throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Duplicate key in associated collection.");
            }
#endif
            int_collection[key] = element;
        }

        public ElementType this[KeyType key]
        {
            get
            {
                return int_collection[key];
            }
        }
    }
    */
}