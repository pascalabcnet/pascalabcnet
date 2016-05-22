using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class CheckVariablesRedefenitionVisitor : WalkingVisitorNew
    {
        private ISet<string> UpperBlockNames { get; set; }

        // Map :: CurrentLevel -> { BlockNames }
        private List<ISet<string>> BlockNamesStack { get;  set; }

        private int CurrentLevel = -1;

        public CheckVariablesRedefenitionVisitor(ISet<string> upperBlockNames)
        {
            //this.UpperBlockNames = upperBlockNames;
            this.UpperBlockNames = new HashSet<string>();
            this.BlockNamesStack = new List<ISet<string>>(10);
        }


        public override void visit(formal_parameters fp)
        {
            foreach (var id in fp.params_list.SelectMany(tp => tp.idents.idents))
            {
                CheckVariableAlreadyDefined(id);
                UpperBlockNames.Add(id.name);
            }
        }

        public override void visit(variable_definitions vd)
        {
            foreach (var id in vd.var_definitions.SelectMany(vds => vds.vars.idents))
            {
                CheckVariableAlreadyDefined(id);
                UpperBlockNames.Add(id.name);
            }

            base.visit(vd);
        }

        /*
         * // Scope Level 0
         * begin 
         *   var x = value; // Name in Scope Level 0
         *   // Scope Level 1
         *   begin
         *     var x = value; // Name in Scope Level 1 <-- REDEFENITION ERROR, x defined at Level 0
         *     
         *     var y = value;
         *     var y = value2; // Name in Scope Level 1 <-- REDEFINITION ERROR, y defined at Level 1
         *     
         *     // Scope Level 2
         *     begin
         *       var z = y; // Name in Scope Level 2
         *     end
         *     // Scope Level 2
         *     begin
         *       var z = value; // Name in Scope Level 2
         *     end
         *   end
         * 
         * end
        */

        public override void visit(statement_list stlist)
        {
            ++CurrentLevel;

            if (BlockNamesStack.Count <= CurrentLevel)
            {
                // Создаем множество имен для текущего уровня вложенности мини-пространства имен
                BlockNamesStack.Add(new HashSet<string>());
            }


            base.visit(stlist);
            //for (var i = 0; i < stlist.list.Count; ++i)
            //    ProcessNode(stlist.list[i]);

            BlockNamesStack.RemoveAt(BlockNamesStack.Count - 1);

            --CurrentLevel;
        }

        public override void visit(var_statement vs)
        {
            foreach (var id in vs.var_def.vars.idents)
            {
                // Проверяем есть ли такое имя выше?
                CheckVariableAlreadyDefined(id);

                BlockNamesStack[CurrentLevel].Add(id.name);
            }

            base.visit(vs);
        }

        

        public override void visit(for_node fn)
        {
            if (fn.create_loop_variable)
            {
                CheckVariableAlreadyDefined(fn.loop_variable);
            }
            base.visit(fn);
        }

        private bool IsVariableAlreadyDefined(string name)
        {
            if (UpperBlockNames.Contains(name))
                return true;
            for (int i = 0; i <= CurrentLevel; ++i)
            {
                if (BlockNamesStack[i].Contains(name))
                    return true;
            }
            return false;
        }

        private void CheckVariableAlreadyDefined(ident id)
        {
            string name = id.name;

            if (IsVariableAlreadyDefined(name))
            {
                throw new PascalABCCompiler.Errors.SyntaxError(string.Format("Var {0} is already defined", name), "", id.source_context, id);
            }
        }
        
    }
}
