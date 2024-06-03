using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxVisitors.Async
{
    public class VarsHelper
    {
        public HashSet<var_statement> VarsList = new HashSet<var_statement>();

        public HashSet<var_statement> TypedVarsList = new HashSet<var_statement>();

        public HashSet<var_statement> NoneTypedVarsList = new HashSet<var_statement>();

		public HashSet<string> Parametrs = new HashSet<string>();

		public HashSet<string> ExprHash = new HashSet<string>();

        public HashSet<var_statement> TasksHash = new HashSet<var_statement>();

        public Dictionary<string,string> RepVarsDict = new Dictionary<string,string>();

		public HashSet<string> ClassIdentSet = new HashSet<string>();
		public HashSet<string> ClassStaticSet = new HashSet<string>();

		public Dictionary<string, string> TaskIdents = new Dictionary<string, string>();

		int k = 0;
        public VarsHelper()
        {
            
        }

        public string newLabelName(string old)
        {
            k++;
            return "@awvar@_" + k.ToString() +"_" + old;
        }

        public void AddNewRep(var_statement var_Statement)
        {
            foreach (var v in var_Statement.var_def.vars.list)
                if (!RepVarsDict.ContainsKey(v.name))
                {
                    if (v.name.StartsWith("@"))
                    {
                        //RepVarsDict.Add(v.name, v.name);
                        continue;
                    }
                    var nv = newLabelName(v.name);
 
                    RepVarsDict.Add(v.name, nv);
                    v.name = nv;
                }
        }

        public void MarkVars()
        {
            foreach (var var in VarsList) 
            {
                var v = var.var_def;
                if (v.vars_type != null)
                    TypedVarsList.Add(var);
                else
                    NoneTypedVarsList.Add(var);
                var vd = var.var_def;
                if (ExprHash.Contains(vd.vars.list[0].name))
                {
                    TasksHash.Add(var);
                }
            }
        }

    }
}
