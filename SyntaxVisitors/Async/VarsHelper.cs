using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxVisitors.Async
{
    // Класс для работы с переменными
    public class VarsHelper
    {
        // Множество всех переменных, находящихся в данном асинхронном методе
        public HashSet<var_statement> VarsList = new HashSet<var_statement>();

        // Множество  всех явно типизированных переменных 
        public HashSet<var_statement> TypedVarsList = new HashSet<var_statement>();

        // Множество  всех неявно типизированных переменных 
        public HashSet<var_statement> NoneTypedVarsList = new HashSet<var_statement>();

        // Множество  всех параметров асинхронного метода 
        public HashSet<string> Parametrs = new HashSet<string>();

        // Множество  переменных, находящихся внутри await
        public HashSet<string> ExprHash = new HashSet<string>();

        // Множество переменных, являющихся Task<T>
        public HashSet<var_statement> TasksHash = new HashSet<var_statement>();

        // Словарик переменных, которые нужно переименовать
        public Dictionary<string,string> RepVarsDict = new Dictionary<string,string>();

        // Список переменных класса
		public HashSet<string> ClassIdentSet = new HashSet<string>();

        // Список статических переменных класса
        public HashSet<string> ClassStaticSet = new HashSet<string>();

        // Словарик переменных, являющихся Task<T>
		public Dictionary<string, string> TaskIdents = new Dictionary<string, string>();

		int LabelNameCounter = 0;
        public string newLabelName(string old)
        {
            LabelNameCounter++;
            return "@awvar@_" + LabelNameCounter.ToString() +"_" + old;
        }
        // Добавляем новую переменную, которую нужно будет переименовать
        public void AddNewRep(var_statement var_Statement)
        {
            foreach (var v in var_Statement.var_def.vars.list)
                if (!RepVarsDict.ContainsKey(v.name))
                {
                    if (v.name.StartsWith("@"))
                    {
                        continue;
                    }
                    var nv = newLabelName(v.name);
 
                    RepVarsDict.Add(v.name, nv);
                    v.name = nv;
                }
        }
        // Помечаем какими являются переменные, чтобы знать 
        // что делать с ними дальше
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
