// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;
using PascalABCCompiler.TreeRealization;
using System.Collections;

namespace PascalABCCompiler
{
    public class VarInfo
    {
        public int cur_ass=0;
        public int cur_use=0;
        public int num_use = 0;
        public int act_num_use = 0;
        public int num_ass = 0;
        public location last_ass_loc=null;
    }

    public class FldInfo
    {
        public int cur_ass = 0;
        public int cur_use = 0;
        public int num_use = 0;
    }

    public class ParamInfo
    {
        public int cur_ass = 0;
        public int cur_use = 0;
        public int num_use = 0;
        public int act_num_use = 0;
        public int num_ass = 0;
    }

    public class OptimizerHelper
    {
        private Hashtable ht = new Hashtable();
        private Dictionary<var_definition_node, List<CompilerWarningWithLocation>> warns = new Dictionary<var_definition_node, List<CompilerWarningWithLocation>>();
        private HashSet<common_function_node> ext_funcs = new HashSet<common_function_node>();

        public void AddVariable(var_definition_node vdn)
        {
            VarInfo vi = new VarInfo();
            if (ht[vdn] == null)
                ht[vdn] = vi;
        }

        public VarInfo GetVariable(var_definition_node vdn)
        {
            return (VarInfo)ht[vdn];
        }

        public void AddParameter(common_parameter vdn)
        {
            ParamInfo vi = new ParamInfo();
            ht[vdn] = vi;
        }

        public ParamInfo GetParameter(common_parameter vdn)
        {
            return (ParamInfo)ht[vdn];
        }

        public void AddField(class_field fld)
        {
            FldInfo vi = new FldInfo();
            ht[fld] = vi;
        }

        public FldInfo GetField(class_field fld)
        {
            return (FldInfo)ht[fld];
        }

        public void AddTempWarning(var_definition_node vdn, CompilerWarningWithLocation cw)
        {
            if ( !warns.TryGetValue(vdn, out var lst) )
            {
                lst = new List<CompilerWarningWithLocation>();
                warns[vdn] = lst;
            }
            lst.Add(cw);
        }

        public void AddRealWarning(var_definition_node vdn, List<CompilerWarningWithLocation> cw)
        {
            List<CompilerWarningWithLocation> lst = warns[vdn];
            cw.AddRange(lst);
        }

        public void MarkAsExternal(common_function_node cfn)
        {
            ext_funcs.Add(cfn);
        }

        public bool IsExternal(common_function_node cfn)
        {
            return ext_funcs.Contains(cfn);
        }
    }
}