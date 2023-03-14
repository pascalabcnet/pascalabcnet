// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace VisualPascalABC
{

    public class Form1StringResources
    {
        public static readonly string Prefix = "VP_MF_";
        public static string Get(string key)
        {
            return PascalABCCompiler.StringResources.Get(Prefix + key);
        }
        public static void SetTextForAllControls(Control c)
        {
            PascalABCCompiler.StringResources.SetTextForAllObjects(c, Prefix);
        }
    }
    public class RuntimeExceptionsStringResources
    {
        public static readonly string Prefix = "RUNTIME_EXCEPTION";
        public static string Get(string key)
        {
            string res=PascalABCCompiler.StringResources.Get(Prefix + key);
            if (res != Prefix + key)
                return res;
            return key;
        }
    }



}
