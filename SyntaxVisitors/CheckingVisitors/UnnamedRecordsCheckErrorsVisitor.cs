// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxVisitors.CheckingVisitors
{
    public class CollectDefNamesFromMostUpperLocalContextVisitor : BaseEnterExitVisitor
    {
        public List<ident> idents = new List<ident>();
        public class_definition StartUnnamedRecord; // безымянная запись, до которой и для которой будем искать имена
        public static CollectDefNamesFromMostUpperLocalContextVisitor New(class_definition cd)
        {
            return new CollectDefNamesFromMostUpperLocalContextVisitor(cd);
        }
        public CollectDefNamesFromMostUpperLocalContextVisitor(class_definition cd)
        {
            StartUnnamedRecord = cd;
        }
        public void Add(ident id)
        {
            if (idents.FindIndex(x => string.Compare(x.name, id.name, true) == 0) == -1)
                idents.Add(id);
        }
        public void AddIdentList(ident_list idl)
        {
            foreach (var id in idl.idents)
                Add(id);
        }
        public void AddProcHeader(procedure_header h)
        {
            if (h.name != null)
                Add(h.name.meth_name);
            if (h.template_args != null)
                AddIdentList(h.template_args);
            if (h.parameters != null)
                foreach (var tp in h.parameters.params_list)
                {
                    AddIdentList(tp.idents);
                }
        }

        /*public override void visit(template_type_name ttn) // при определении типа
        {
            Add(ttn);
            AddIdentList(ttn.template_args);
        }*/
        public override void visit(var_def_statement vds)
        {
            AddIdentList(vds.vars);
            ProcessNode(vds.vars_type);
        }
        /*public override void visit(typed_parameters tp)
        {
            AddIdentList(tp.idents);
        }*/
        public override void visit(procedure_header h)
        {
            AddProcHeader(h);
        }
        public override void visit(function_header h)
        {
            AddProcHeader(h);
        }
        public override void visit(typed_const_definition cd)
        {
            Add(cd.const_name);
        }
        public override void visit(simple_const_definition cd)
        {
            Add(cd.const_name);
        }
        public override void visit(type_declaration td)
        {
            Add(td.type_name);
            if (td.type_name is template_type_name ttn)
            {
                AddIdentList(ttn.template_args);
            }
            ProcessNode(td.type_def);
        }

        // var_def_statement.idents +
        // typed_parameters (параметры процедуры-функции) +
        // method_name +
        // template_type_name + 
        // type_declaration.type_name
        // const_definition.constant_name +

        public override void visit(class_definition cd)
        {
            if (cd == StartUnnamedRecord) 
            {
                //cd = cd;
                // то внутри нее не искать - мы как раз с ней и проверяем пересечение имен
            }
            else if (StartUnnamedRecord.source_context.In(cd.source_context))
            {
                DefaultVisit(cd);
            }
            else
            {

            }
        }

    }

    public class OneUnnamedRecordCaptureNamesVisitor : BaseEnterExitVisitor
    {
        // Имена, захваченные записью. Их мы будем искать затем во внешних контекстах
        // Сюда не включаются имена во вложенных безымянных записях
        // Сюда не включаются имена, являющиеся определениями (в данном случае это только определения полей)

        // Есть еще одна проблема - в имена, захваченные записью, не должны включаться имена, определенные в самой записи (!!!) - #2214

        public List<ident> idents = new List<ident>();
        public List<ident> defIdents = new List<ident>();
        public class_definition StartUnnamedRecord;
        public static OneUnnamedRecordCaptureNamesVisitor New(class_definition cd)
        {
            return new OneUnnamedRecordCaptureNamesVisitor(cd);
        }
        public OneUnnamedRecordCaptureNamesVisitor(class_definition cd)
        {
            StartUnnamedRecord = cd;
        }
        // Накопленные имена типов надо будет сравнивать только с именами в секции type (в процедуре) и с именами в параметрах внешних обобщений (процедур и классов)
        public void CollectNames()
        {
            var defBlocks = StartUnnamedRecord.body.class_def_blocks;
            // Теперь известно, что остался ровно один class_def_block
            foreach (var member in defBlocks[0].members)
            {
                var q = member as var_def_statement;
                if (q == null)
                    continue;
                var id = q.vars.idents[0]; // это просто имя
                var type = q.vars_type;
                var initValue = q.inital_value;
                defIdents.Add(id); // Определения надо добавлять в другой список. Потом вычитать его из определений в неглобальных контекстах
                ProcessNode(type);
                if (initValue != null)
                    ProcessNode(initValue);
            }
        }
        public override void visit(ident id)
        {
            if (idents.FindIndex(x => string.Compare(x.name,id.name,true) == 0) == -1)
                idents.Add(id);
        }
        public override void visit(named_type_reference ntr)
        {
            visit(ntr.names[0]);
            if (ntr is template_type_reference ttr)
            {
                foreach (var tt in ttr.params_list.params_list)
                    ProcessNode(tt);
            }
        }
        public override void visit(class_definition cd)
        {
            if (cd.keyword == class_keyword.Record && cd.Parent is var_def_statement) // безымянная запись
            {
                // то пропустить её - она вложенная, мы её уже обошли
            }
            else base.visit(cd);
        }

    }
    public class UnnamedRecordsCheckVisitor : BaseEnterExitVisitor, IPipelineVisitor
    {
        public static UnnamedRecordsCheckVisitor New
        {
            get { return new UnnamedRecordsCheckVisitor(); }
        }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            ProcessNode(root);

            next();
        }

        public override void Exit(syntax_tree_node st)
        {
            var cd = st as class_definition;
            if (cd == null)
                return;
            if (cd.keyword == class_keyword.Record) 
            {
                // Проверить, что это неглобальная запись. Если глобальная, то можно всё - и выйти
                syntax_tree_node MostUpperNodeWithNotGlobalNames = null;

                syntax_tree_node sn = cd;
                while ((sn != null) && !(sn is program_module))
                {
                    sn = sn.Parent;
                    if (sn is procedure_definition)
                        MostUpperNodeWithNotGlobalNames = sn;
                    if (sn is class_definition)
                    {
                        MostUpperNodeWithNotGlobalNames = sn;
                        sn = sn.Parent;
                        if (sn is type_declaration || sn is var_def_statement)
                            MostUpperNodeWithNotGlobalNames = sn;
                    }
                }

                if (MostUpperNodeWithNotGlobalNames == null) // то есть, это - глобальный уроень, и захватывать можно всё
                    return;

                // если попали сюда, то запись вложенная

                //if (cd.Parent is var_def_statement) // безымянная запись
                {
                    if (cd.class_parents != null && cd.class_parents.Count > 0)
                        throw new SyntaxVisitorError("NESTED_RECORD_CANNOT_IMPLEMENT_INTERFACE", cd.class_parents[0].source_context);

                    var defBlocks = cd.body.class_def_blocks;

                    if (defBlocks[0].access_mod.access_level != access_modifer.public_modifer)
                    {
                        var f = cd.body.class_def_blocks.First();
                        var loc = f.source_context;
                        if (loc == null)
                            loc = cd.source_context;
                        throw new SyntaxVisitorError("NESTED_RECORD_CAN_CONTAIN_ONLY_ONE_PUBLIC_VISIBILITY_SECTION", loc);
                    }

                    if (defBlocks.Count > 1)
                    {
                        var el1 = cd.body.class_def_blocks.ElementAt(1);
                        var loc = el1.access_mod.source_context;
                        if (loc == null)
                            loc = cd.source_context;
                        throw new SyntaxVisitorError("NESTED_RECORD_CANNOT_CONTAIN_SEVERAL_VISIBILITY_SECTIONS", loc);
                    }

                    // здесь уже известно, что defBlocks.Count = 1 - пройтись по этому блоку и проверить, нет ли методов или свойств

                    foreach (var d in defBlocks[0].members)
                    {
                        var pd = d as procedure_definition;
                        if (pd != null)
                            throw new SyntaxVisitorError("NESTED_RECORD_CANNOT_CONTAIN_METHODS", pd.source_context);
                        var sp = d as simple_property;
                        if (sp != null)
                            throw new SyntaxVisitorError("NESTED_RECORD_CANNOT_CONTAIN_PROPERTIES", sp.source_context);
                    }
                }

                // после накопления имен подниматься по внешнему контексту, пропуская эту запись, и искать переменные и типы
                // Переменные - в var_def_statement. Типы - в type_definition, в типовых параметрах обобщенных процедур и в типовых параметрах имен типов

                // Подняться вверх до program_module и запомнить последнюю class_definition, procedure_definition 
                // или class_definition или type_declaration если это именованный тип
                // То есть у class_definition проверять Parent - если это type_declaration, то смотреть на имя типа, а если это var_def_statement, то смотреть на имена переменных

                var cdvis = OneUnnamedRecordCaptureNamesVisitor.New(cd);
                cdvis.CollectNames();

                var localsvis = CollectDefNamesFromMostUpperLocalContextVisitor.New(cd);
                if (MostUpperNodeWithNotGlobalNames != null)
                {
                    localsvis.ProcessNode(MostUpperNodeWithNotGlobalNames);
                }

                var lvi = localsvis.idents.Select(id => id.name).ToArray();
                var cvi = cdvis.idents;
                var cvdi = cdvis.defIdents;

                // Теперь проверим пересечение
                foreach (var id in cvi)
                {
                    var s = id.name;
                    foreach (var lname in lvi)
                    {
                        var ind = cvdi.FindIndex(idd => string.Compare(idd.name, lname, true) == 0);
                        bool b = ind != -1;
                        if (b)
                            b = cvdi[ind].source_context.Less(id.source_context);
                        if (string.Compare(s,lname,true)==0 && !b)
                        {
                            // то надо еще проверить cvdi чтобы там как раз не было этих имен
                            throw new SyntaxVisitorError("NESTED_RECORD_CANNOT_CATCH_NAMES_FROM_NONGLOBAL_CONTEXT", id);
                        }
                    } 
                }
            }
        }
    }
}
