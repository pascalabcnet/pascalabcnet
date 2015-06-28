using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace ParsePABC1
{
    class DeleteLocalDefs : BaseChangeVisitor
    {
        public List<var_def_statement> LocalDeletedIds = new List<var_def_statement>();
        public List<var_def_statement> BlockDeletedIds = new List<var_def_statement>();
        public HashSet<string> idsToDelete;
        public HashSet<string> deletedIdsToDeleteInLocalScope = new HashSet<string>(); // одно множество и на Local и на Block

        public DeleteLocalDefs(HashSet<string> ids) // надо запускать этот визитор начиная с корня подпрограммы
        {
            idsToDelete = ids;
        }
        public override void visit(var_statement vs)
        {
            var idents = vs.var_def.vars.idents;

            var IdentsToDeleteInVarDef = idents.FindAll(id => idsToDelete.Contains(id.name)); // найти в операторе все идентификаторы для удаления
            if (IdentsToDeleteInVarDef.Count != 0)
            {
                deletedIdsToDeleteInLocalScope.UnionWith(IdentsToDeleteInVarDef.Select(id => id.name)); // добавить те идентификаторы, которые мы удаляем из данного описания

                LocalDeletedIds.Add(new var_def_statement(new ident_list(IdentsToDeleteInVarDef), vs.var_def.vars_type, vs.var_def.inital_value)); // добавить описание из удаленных в данном разделе описаний идентификаторов
                idents.RemoveAll(id => idsToDelete.Contains(id.name)); // удалить в операторе все идентификаторы для удаления

                idsToDelete.ExceptWith(deletedIdsToDeleteInLocalScope);

                if (idents.Count == 0) // то и весь var_statement надо убить
                    this.DeleteInStatementList(vs);
            }
            // Здесь мы не обрабатываем вложенный var_def_statement, поэтому когда мы будем обрабатывать его в другом visit, то это будут переменные до beginа подпрограммы или основной программы
        }
        public override void visit(var_def_statement vd)
        {
            var idents = vd.vars.idents;

            var IdentsToDeleteInVarDef = idents.FindAll(id => idsToDelete.Contains(id.name)); // найти в операторе все идентификаторы для удаления
            if (IdentsToDeleteInVarDef.Count != 0)
            {
                deletedIdsToDeleteInLocalScope.UnionWith(IdentsToDeleteInVarDef.Select(id => id.name)); // добавить те идентификаторы, которые мы удаляем из данного описания

                BlockDeletedIds.Add(new var_def_statement(new ident_list(IdentsToDeleteInVarDef), vd.vars_type, vd.inital_value)); // добавить описание из удаленных в данном разделе описаний идентификаторов
                idents.RemoveAll(id => idsToDelete.Contains(id.name)); // удалить в операторе все идентификаторы для удаления

                if (idents.Count == 0)
                {
                    // Выше  - variable_definitions, еще выше - declarations
                    var uvdsl = UpperNode() as variable_definitions;
                    uvdsl.var_definitions.Remove(vd);  // Проблема - мы удаляем первую var_def_statement и вторая становится первой. А в цикле обхода - индексы что естественно
                    if (uvdsl.var_definitions.Count == 0)
                    {
                        var d = UpperNode(2) as declarations;
                        d.defs.Remove(uvdsl);
                    }
                }
                    
            }
        }
        public override void visit(variable_definitions vd) 
        {
            if (vd.var_definitions != null)
                for (int i = vd.var_definitions.Count - 1; i >= 0; --i) // в обратном порядке - тогда удаление текущего элемента работает корректно
                    ProcessNode(vd.var_definitions[i]);
        }

        public override void visit(declarations d)
        {
            if (d.defs != null)
                for (int i = d.defs.Count - 1; i >= 0; --i) // в обратном порядке - тогда удаление текущего элемента работает корректно
                    ProcessNode(d.defs[i]);
        }

        public void AfterProcTraverse()
        {
            idsToDelete.ExceptWith(deletedIdsToDeleteInLocalScope); // исключаем из множества удаляемых идентификаторов те, которые мы нашли и удалили в секции var_statement. Это надо делать не для каждого описания, а в конце подпрограммы после удаления из всех секций var_statement
        }
    }
}
