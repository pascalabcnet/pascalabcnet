// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class RenameSameBlockLocalVarsVisitor : BaseChangeVisitor
    {
        // Надо хранить принадлежность имени конкретному ПИ на определенном уровне вложенности
        // И отображение этого имени в новое

        // Map :: CurrentLevel -> { BlockName -> NewBlockName }
        private List<Dictionary<string, string>> BlockNamesStack { get; set; }

        private Dictionary<string, int> BlockNamesCounter { get; set; }


        private int CurrentLevel = -1;

        public RenameSameBlockLocalVarsVisitor()
        {
            this.BlockNamesStack = new List<Dictionary<string, string>>();
            this.BlockNamesCounter = new Dictionary<string, int>();
        }

        public static RenameSameBlockLocalVarsVisitor New
        {
            get { return new RenameSameBlockLocalVarsVisitor(); }
        }

        public static void Accept(procedure_definition pd)
        {
            New.ProcessNode(pd);
        }

        public override void visit(declarations decls)
        {
            // Для обхода в правильном порядке сверху вниз
            var vd_s = decls.defs.OfType<variable_definitions>().ToArray();
            for (int i = 0; i < vd_s.Count(); ++i)
            {
                ProcessNode(vd_s[i]);
            }
        }

        public override void visit(procedure_header proc_header)
        {
            // DO THIS THING!
        }

        private bool first_time_visit_function_header = true;
        public override void visit(function_header fh)
        {
            if (first_time_visit_function_header)
            {
                //DefaultVisit(fh);
                first_time_visit_function_header = false;
            }
            // DO NOTHING
        }

        public override void visit(formal_parameters fp)
        {
            if (!first_time_visit_function_header) // чтобы в лямбдах не заходить в формальные параметры. Во вложенных тоже не зайдёт
                return;
            // SSM переименование формальных параметров (нужно если мы их изменяем внутри)
            ++CurrentLevel;
            BlockNamesStack.Add(new Dictionary<string, string>());
            if (fp != null)
            {
                var fpids = fp.params_list.SelectMany(tp => tp.idents.idents);
                foreach (var v in fpids)
                {
                    var low = v.name.ToLower();
                    //BlockNamesStack[CurrentLevel].Add(low, "$fp_"+ low);
                    BlockNamesStack[CurrentLevel][low] = "$fp_" + low;
                }
            }
            // DO NOTHING
        }

        public override void visit(statement_list stlist)
        {
            ++CurrentLevel;

            if (BlockNamesStack.Count <= CurrentLevel)
            {
                // Создаем отображение для имен текущего уровня вложенности мини-пространства имен
                BlockNamesStack.Add(new Dictionary<string, string>());
            }

            //base.visit(stlist);
            for (var i = 0; i < stlist.list.Count; ++i)
                ProcessNode(stlist.list[i]);

            BlockNamesStack.RemoveAt(BlockNamesStack.Count - 1);

            --CurrentLevel;
        }

        public override void visit(var_def_statement var_def)
        {
            if (var_def.vars.idents.Any(id => id.name.StartsWith("$")))
            {
                base.visit(var_def); // SSM 17/07/16 исправление ошибки - не обходилось выражение-инициализатор
                return;
            }

            var newLocalNames = var_def.vars.idents.Select(id => 
                {
                    var low = id.name.ToLower();

                    var newName = this.CreateNewVariableName(low);
                    //BlockNamesStack[CurrentLevel].Add(low, newName);
                    BlockNamesStack[CurrentLevel][low] = newName;
                    return new ident(newName, id.source_context);
                });

            var newVS = new var_def_statement(new ident_list(newLocalNames.ToArray()),
                var_def.vars_type,
                var_def.inital_value);

            Replace(var_def, newVS);
            listNodes[listNodes.Count - 1] = newVS; //SSM 8.11.18
            ProcessNode(newVS.inital_value); // SSM 10.06.2020 #2103
            //base.visit(newVS); // SSM 10.06.2020 - зачем обходить всё?
        }

        /*public override void visit(var_statement vs)
        {
            if (vs.var_def.vars.idents.Any(id => id.name.StartsWith("$")))
            {
                base.visit(vs); // SSM 17/07/16 исправление ошибки - не обходилось выражение-инициализатор
                return;
            }

            var newLocalNames = vs.var_def.vars.idents.Select(id =>
            {
                var low = id.name
                //.ToLower()
                ;

                var newName = this.CreateNewVariableName(low);
                //BlockNamesStack[CurrentLevel].Add(low, newName);
                BlockNamesStack[CurrentLevel][low] = newName;
                return new ident(newName, id.source_context);
            });

            var newVS = new var_statement(new var_def_statement(new ident_list(newLocalNames.ToArray()),
                vs.var_def.vars_type,
                vs.var_def.inital_value));

            Replace(vs, newVS);

            base.visit(newVS);
        }/* */

        public override void visit(variable_definitions vd)
        {
            if (vd.var_definitions.Any(vds => vds.vars.idents.Any(id => id.name.StartsWith("$"))))
                return;

            ++CurrentLevel;

            if (BlockNamesStack.Count <= CurrentLevel)
            {
                // Создаем отображение для имен текущего уровня вложенности мини-пространства имен
                BlockNamesStack.Add(new Dictionary<string, string>());
            }

            var newVD = new variable_definitions(
                vd.var_definitions.Select(vds =>
                {
                    var newLocalNames = vds.vars.idents.Select(id =>
                    {
                        var low = id.name.ToLower();

                        var newName = this.CreateNewVariableName(low);
                        //BlockNamesStack[CurrentLevel].Add(low, newName);
                        BlockNamesStack[CurrentLevel][low] = newName;
                        return new ident(newName, id.source_context);
                    });

                    return new var_def_statement(new ident_list(newLocalNames.ToArray()), vds.vars_type, vds.inital_value);
                }).ToList(),
                vd.source_context);

            Replace(vd, newVD);
            base.visit(newVD);
        }

        public override void visit(ident id)
        {
            // Это очень частный алгоритм переименования в мини-ПИ, но затрагивает он и лямбды. И механизм для этого - неадекватный
            // Надо исключать формальные параметры лямбд и одноимённые переменнные, определяемые у них внутри
            var newName = this.GetNewVariableName(id.name.ToLower());
            if (newName != null)
            {
                Replace(id, new ident(newName, id.source_context));
            }
        }

        public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);
            if (dn.right.GetType() != typeof(ident))
                ProcessNode(dn.right);
        }

        public override void visit(function_lambda_definition fld)
        {
            BlockNamesStack.Add(new Dictionary<string, string>());
            var last = BlockNamesStack.Count - 1;
            for (var i=0; i< fld.parameters.expressions.Count; i++)
            {
                var qname = (fld.parameters.expressions[i] as ident).name.ToLower();
                BlockNamesStack[last][qname] = "-fl"; // -fl - формальный параметр лямбды - это стоп в поиске в GetNewVariableName
            }
            base.visit(fld);
        }

        private string CreateNewVariableName(string name)
        {
            name = name.ToLower();
            if (BlockNamesCounter.ContainsKey(name))
            {
                ++BlockNamesCounter[name];
            }
            else
            {
                BlockNamesCounter.Add(name, 0);
            }
            return "$" + name + "__" + BlockNamesCounter[name]; 
        }

        private string GetNewVariableName(string name)
        {
            name = name.ToLower();
            for (int i = CurrentLevel; i >= 0; --i)
            {
                if (BlockNamesStack[i].ContainsKey(name))
                {
                    if (BlockNamesStack[i][name] == "-fl") // формальный параметр лямбды - это стоп в поиске (переименовывать не надо!)
                        return null;
                    return BlockNamesStack[i][name];
                }
            }
            return null;
        }
    }
}
