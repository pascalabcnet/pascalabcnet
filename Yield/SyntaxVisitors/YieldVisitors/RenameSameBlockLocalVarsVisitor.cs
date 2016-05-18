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

        public override void visit(declarations decls)
        {
            // Для обхода в правильном порядке сверху вниз
            var vd_s = decls.defs.OfType<variable_definitions>().ToArray();
            for (int i = 0; i < vd_s.Count(); ++i)
            {
                ProcessNode(vd_s[i]);
            }
        }

        public override void visit(procedure_header pd)
        {
            // DO NOTHING
        }

        public override void visit(function_header fh)
        {
            // DO NOTHING
        }

        public override void visit(formal_parameters fp)
        {
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

        public override void visit(var_statement vs)
        {
            if (vs.var_def.vars.idents.Any(id => id.name.StartsWith("$")))
                return;

            var newLocalNames = vs.var_def.vars.idents.Select(id => 
                {
                    var newName = this.CreateNewVariableName(id.name);
                    BlockNamesStack[CurrentLevel].Add(id.name, newName);
                    return new ident(newName, id.source_context);
                });

            var newVS = new var_statement(new var_def_statement(new ident_list(newLocalNames.ToArray()),
                vs.var_def.vars_type,
                vs.var_def.inital_value));

            Replace(vs, newVS);

            base.visit(newVS);
        }

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
                        var newName = this.CreateNewVariableName(id.name);
                        BlockNamesStack[CurrentLevel].Add(id.name, newName);
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
            var newName = this.GetNewVariableName(id.name);
            if ((object)newName != null)
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

        private string CreateNewVariableName(string name)
        {
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
            for (int i = CurrentLevel; i >= 0; --i)
            {
                if (BlockNamesStack[i].ContainsKey(name))
                {
                    return BlockNamesStack[i][name];
                }
            }
            return null;
        }
    }
}
