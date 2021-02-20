using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;


// SSM 13/07/20
// Нигде не используется поскольку не решает проблему конфликта is_var и yield.
// Хотя видимо все другие проблемы может решать
namespace SyntaxVisitors.PatternsVisitors
{
    class CollectNamesInIsVarVisitor : WalkingVisitorNew
    {
        public HashSet<string> NamesInIsVars = new HashSet<string>();
        public static CollectNamesInIsVarVisitor New => new CollectNamesInIsVarVisitor();

        public override void visit(is_pattern_expr isPatternExpr)
        {
            var pp = isPatternExpr.right?.parameters;
            if (pp != null)
            {
                foreach (var p in pp)
                {
                    if (p is var_deconstructor_parameter vdp)
                    {
                        NamesInIsVars.Add(vdp.identifier.name.ToLower());
                    }
                }
            }
        }

        public static HashSet<string> NamesToRename(procedure_definition pd)
        {
            var v = new CollectNamesInIsVarVisitor();
            v.ProcessNode(pd);
            return v.NamesInIsVars;
        }

    }

    struct PairNames
    {
        public ident Name;
        public string NewName;
        public PairNames(ident Name, string NewName)
        {
            this.Name = Name;
            this.NewName = NewName;
        }
    }

    // Этот визитор запускать только с procedure_definition: ReplaceNamesInIsVarVisitor.New.ProcessNode(pd)
    class ReplaceNamesInIsVarVisitor : WalkingVisitorNew
    {
        HashSet<string> NamesToRename = null;
        // Список (стек) пространств имен
        List<syntax_tree_node> nsp = new List<syntax_tree_node>();
        // Словарь имён
        Dictionary<syntax_tree_node, List<PairNames>> dict = new Dictionary<syntax_tree_node, List<PairNames>>();
        private int num = 0;
        public string GenerateNewName(string name)
        {
            num += 1;
            return "#GenIsVar" + num + "#" + name;
        }
        public static ReplaceNamesInIsVarVisitor New => new ReplaceNamesInIsVarVisitor();

        public syntax_tree_node CurrentNamespace() => nsp[nsp.Count - 1];

        public void PushNamespace(syntax_tree_node stn)
        {
            dict[stn] = new List<PairNames>();
            nsp.Add(stn);
        }
        public void PopNamespace()
        {
            // удалить соответствующий элемент словаря - если он есть
            dict.Remove(CurrentNamespace());
            nsp.RemoveAt(nsp.Count - 1);
        }

        public void AddNameNewName(ident name, string newname)
        {
            if (!NamesToRename.Contains(name.name.ToLower())) // Если имя - не из нашего внимания, то не добавляем
                return;
            // Ошибка возникает когда это имя есть в каком-то namespace выше и оно - не через границу function_lambda_definition
            // Начинаем проверять с текущего и выше до f_l_d включительно или до p_d включительно. Если есть совпадение, то ошибка
            var i = nsp.Count - 1;
            while (true)
            {
                var curns = nsp[i];
                // проверить, есть ли в dict[curns] такое имя
                if (dict[curns].Select(n=>n.Name.name.ToLower()).Contains(name.name.ToLower()))
                    throw new SyntaxVisitorError("Повторное объявление", name.source_context);
                if (curns is procedure_definition || curns is function_lambda_definition)
                    break;
                i--;
            }

            dict[CurrentNamespace()].Add(new PairNames(name, newname));
        }
        public override void visit(procedure_definition pd) // три вида ПИ, которые надо учитывать
        {
            NamesToRename = CollectNamesInIsVarVisitor.NamesToRename(pd); // Это - акцент внимания
            if (NamesToRename.Count == 0)
                return; // nothing top do

            PushNamespace(pd);

            if (pd.proc_header.parameters != null)
            {
                var allnames = pd.proc_header.parameters.params_list.SelectMany(tp => tp.idents.idents);
                foreach (var name in allnames)
                    AddNameNewName(name, null); // null означает, что переименовывать не надо
            }

            base.visit(pd);
            PopNamespace();
            NamesToRename = null;
        }
        public override void visit(function_lambda_definition fd)
        {
            PushNamespace(fd);
            if (fd.formal_parameters != null)
            {
                var allnames = fd.formal_parameters.params_list.SelectMany(tp => tp.idents.idents);
                foreach (var name in allnames)
                    AddNameNewName(name, null); // null означает, что переименовывать не надо
            }

            base.visit(fd);
            PopNamespace();
        }
        public override void visit(statement_list stl)
        {
            PushNamespace(stl);
            base.visit(stl);
            PopNamespace();
        }

        public override void visit(is_pattern_expr isPatternExpr)
        {
            var pp = isPatternExpr.right?.parameters;
            if (pp != null)
            {
                foreach (var p in pp)
                {
                    if (p is var_deconstructor_parameter vdp)
                    {
                        var newname = GenerateNewName(vdp.identifier.name);
                        AddNameNewName(vdp.identifier.TypedClone(), newname);
                        vdp.identifier.name = newname;
                    }
                }
            }
        }

        // попытаться переименовать
        public void TryToRename(ident id)
        {
            if (!NamesToRename.Contains(id.name.ToLower()))
                return;
            // Начинаем искать по ПИ это имя с именем для переименования
            for (var i = nsp.Count - 1; i >= 0; i--)
            {
                var curns = nsp[i];
                // проверить, есть ли в dict[curns] такое имя
                var ind = dict[curns].FindIndex(pn => pn.Name.name.ToLower() == id.name.ToLower());
                if (ind != -1)
                {
                    var newname = dict[curns][ind].NewName;
                    if (newname != null)
                    {
                        id.name = newname;
                        return; //переименовали - миссия выполнена!
                    }
                }

                if (curns is procedure_definition || curns is function_lambda_definition)
                    break;
            }
        }

        // Собственно переименование
        public override void visit(ident id)
        {
            TryToRename(id);
        }

        public override void visit(dot_node dn)
        {
            if (dn.left is ident id)
                TryToRename(id); 
            else ProcessNode(dn.left);
            ProcessNode(dn.right);
        }

    }
}
