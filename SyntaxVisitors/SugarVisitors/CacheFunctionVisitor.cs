using System;
using System.Collections.Generic;
using System.Linq;

using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;

namespace SyntaxVisitors.SugarVisitors
{
    public class CacheFunctionVisitor : BaseChangeVisitor, IPipelineVisitor
    {
        private static int num = 0;
        public static string UniqueNumStr()
        {
            num++;
            return num.ToString();
        }
        public static CacheFunctionVisitor New
        {
            get { return new CacheFunctionVisitor(); }
        }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            ProcessNode(root);

            next();
        }

        public override void visit(simple_attribute_list al)
        {
            base.visit(al);
        }
        public override void visit(procedure_definition pd)
        {
            var attrs = pd.proc_header.attributes;
            // Проверить, что среди атрибутов есть [Cache]
            // Проверить, что [Cache] - без параметров
            // Можно [Cache 100] - ограничение на количество кешируемых значений
            // И тогда - какая стратегия отброса старых:

            //var qq1 = attrs?.attributes?.SelectMany(att => att?.attributes).ToArray();
            List<ident> qq = null;
            if (attrs != null)
                qq = attrs.attributes?.SelectMany(att => att?.attributes)?.Where(at => at?.type is named_type_reference)
                  .Select(at=>at.type.names).Where(ns=>ns.Count==1).Select(nm => nm[0])
                  .ToList();
            //if (attrs?.attributes?[0].attributes?[0]?.type?.names?[0]?.name?.ToLower()=="cache") // слишком быстро
            if (qq != null && qq.Any(nm=>nm.name.ToLower()=="cache"))
            {
                // проверить, что 
                // это функция +
                // это не метод расширения +
                // параметры есть (хоть один) +
                // параметров не более семи +
                // тип возвращаемого значения задаётся явно +
                // имя функции не составное +
                // отсутствие var и const в параметрах +
                // функция определена глобально

                if (pd.DescendantNodes().OfType<yield_node>().Count() > 0)
                    throw new SyntaxVisitorError("FUNCTIONS_WITH_CACHE_ATTRIBUTE_CANNOT_HAVE_YIELD", pd.proc_header);
                var fh = pd.proc_header as function_header;
                if (fh == null)
                    throw new SyntaxVisitorError("ONLY_FUNCTION_SHOULD_HAVE_CACHE_ATTRIBUTE",pd.proc_header);
                var isExtension = fh.proc_attributes.proc_attributes.Any(attr => attr.name == "extensionmethod");
                if (isExtension)
                    throw new SyntaxVisitorError("EXTENSION_FUNCTIONS_SHOULD_NOT_HAVE_CACHE_ATTRIBUTE", fh);
                if (fh.parameters == null || fh.parameters != null && fh.parameters.Count == 0)
                    throw new SyntaxVisitorError("FUNCTIONS_WITH_CACHE_ATTRIBUTE_SHOULD_HAVE_PARAMETERS", fh);
                if (fh.parameters != null && fh.parameters.params_list.Sum(tp => tp.idents.idents.Count) > 7)
                    throw new SyntaxVisitorError("FUNCTIONS_WITH_CACHE_ATTRIBUTE_SHOULD_HAVE_LESSTHEN8_PARAMETERS", fh);
                if (fh.return_type == null)
                    throw new SyntaxVisitorError("FUNCTIONS_WITH_CACHE_ATTRIBUTE_SHOULD_HAVE_EXPLICIT_RETURN_TYPE", fh);
                if (fh.name.class_name != null)
                    throw new SyntaxVisitorError("FUNCTIONS_WITH_CACHE_ATTRIBUTE_SHOULD_NOT_HAVE_COMPOUND_NAME", fh.name);
                var notByValue = pd.proc_header.parameters.params_list.Any(tp => tp.param_kind != parametr_kind.none);
                if (notByValue)
                    throw new SyntaxVisitorError("FUNCTIONS_WITH_CACHE_ATTRIBUTE_SHOULD_NOT_HAVE_VAR_CONST_PARAMS_MODIFIERS", fh);

                var pp = pd.proc_header.parameters.params_list.SelectMany(tp => tp.idents.idents.Select(id => Tuple.Create(id, tp.vars_type))).ToArray();

                // var @tpar := Tuple.Create(все параметры)
                var tupleIdent = new ident("@tpar" + UniqueNumStr());
                // SSM 08.05.22 - один параметр не надо оборачивать в кортеж

                // SSM 09.05.22 - для реализации кортежа используется ValueTuple если он определен
                var TupleName = "Tuple";
                if (System.Type.GetType("System.ValueTuple") != null)
                    TupleName = "ValueTuple";

                var_statement vs;
                if (pp.Length == 1)
                    vs = new var_statement(tupleIdent, pp[0].Item1 as expression);
                else vs = new var_statement(tupleIdent,
                  new method_call(
                      new dot_node(new dot_node("?System", TupleName),new ident(PascalABCCompiler.StringConstants.default_constructor_name)),
                      new expression_list(pp.Select(pair => pair.Item1 as expression).ToList())
                  )
                );
                var proc_code = (pd.proc_body as block).program_code;
                proc_code.list.Insert(0, vs);
                if (proc_code.list[proc_code.list.Count - 1] is empty_statement)
                    proc_code.list.RemoveAt(proc_code.list.Count - 1);

                // var @ИмяФункцииDict := new Dictionary<типTuple, тип возвр значения функции>
                // var d := new PABCSystem.Dictionary<System.Tuple<integer,integer,real>,integer>;
                // SSM 08.05.22 - один параметр не надо оборачивать в кортеж
                template_param_list ttp;
                if (pp.Length == 1)
                    ttp = new template_param_list(pp[0].Item2 as type_definition);
                else
                    ttp = new template_param_list(new List<type_definition>(pp.Select(pair => pair.Item2 as type_definition)));

                type_definition ttr_tuple;
                if (pp.Length == 1)
                    ttr_tuple = pp[0].Item2 as type_definition;
                else
                    ttr_tuple = new template_type_reference(new named_type_reference(new List<ident> { new ident("System"), new ident(TupleName) }),
                    ttp
                    );

                var ttr_dict = new template_type_reference(new named_type_reference(new List<ident> { new ident("PABCSystem"), new ident("Dictionary") }),
                new template_param_list(new List<type_definition> { ttr_tuple,(pd.proc_header as function_header).return_type }));

                var new_ex = new new_expr(ttr_dict, new expression_list());
                var sug_ex = new sugared_expression(ttp, new_ex, new_ex.source_context); // семантическая проверка выражения

                var dictIdent = new ident("@" + pd.proc_header.name.ToString() + "DictCache" + UniqueNumStr());
                var vsglobal = new var_statement(dictIdent, sug_ex);

                var declarations = pd.Parent as declarations;
                if (declarations == null)
                    throw new SyntaxVisitorError("FUNCTIONS_WITH_CACHE_ATTRIBUTE_SHOULD_BE_DEFINED_GLOBALLY", fh);

                var ind = declarations.list.FindIndex(decl => decl == pd);
                declarations.list.Insert(ind, vsglobal);
                // До: 
                //   if d.ContainsKey(tup) then
                //   begin
                //     Result := d[tup];
                //     exit;
                //   end;
                // а надо другой код

                /*var indr = new indexer(dictIdent, new expression_list(tupleIdent));
                var assRes = new assign("Result", indr);
                var exitst = new procedure_call(new ident("exit"));
                var stlst = new statement_list(assRes, exitst);
                var cond = new method_call(new dot_node(dictIdent, new ident("ContainsKey")),
                    new expression_list(tupleIdent));
                var ifstatement = new if_node(cond, stlst);*/

                // if d.TryGetValue(tup,Result) then exit;

                var indr = new indexer(dictIdent, new expression_list(tupleIdent));
                var exitst = new procedure_call(new ident("exit"));
                var cond = new method_call(new dot_node(dictIdent, new ident("TryGetValue")),
                    new expression_list(new List<expression> { tupleIdent, new ident("Result") }));
                var ifstatement = new if_node(cond, exitst);
                proc_code.list.Insert(1, ifstatement);

                // В конец: d[tup] := Result. И надо то же после каждого exit
                var assEnd = new assign(indr, new ident("Result"));
                // Вставить d[tup] := Result перед каждым exit начиная с конца
                var pcc = pd.DescendantNodes().OfType<procedure_call>().Where(pc => pc.func_name is ident id && id.name.ToLower() == "exit" && pc.source_context != null).ToArray();
                foreach (var p in pcc.Reverse())
                {
                    var ae = assEnd.Clone() as assign;
                    if (p.Parent is statement_list sl)
                        sl.ReplaceInList(p, new List<statement> { ae, p});
                    else p.Parent.ReplaceDescendant(p as statement, new statement_list(new List<statement> { ae, p }));
                }
                if (proc_code.list[proc_code.list.Count - 1] is procedure_call pcall && pcall.func_name is ident idd && idd.name.ToLower() == "exit")
                    ; // ничего не добавлять - т.к. перед exit всё добавлено
                else proc_code.list.Add(assEnd);

                // Добавить в начало функции семантические проверки:
                // отсутствие делегатов и pointer в параметрах
                // К сожалению, это описание 
                //   var d := new PABCSystem.Dictionary<System.Tuple<function(i: integer): integer,integer>,integer>;
                // даёт ошибку раньше, поскольку размещено в разделе описаний
                // Нужно включать семантическую проверку в выражение new
                var semchk1 = new semantic_check_sugared_statement_node(typeof(semantic_check_delegates_pointers_in_cached_function), new List<syntax_tree_node> { pd.proc_header.parameters }, pd.source_context);
                (pd.proc_body as block).program_code.list.Insert(0, semchk1);
                semchk1.Parent = (pd.proc_body as block).program_code;
            }
        }
    }
}
