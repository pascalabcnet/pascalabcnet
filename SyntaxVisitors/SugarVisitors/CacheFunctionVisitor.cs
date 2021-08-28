using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class CacheFunctionVisitor : BaseChangeVisitor
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

                var fh = pd.proc_header as function_header;
                if (fh == null)
                    throw new SyntaxVisitorError("ONLY_FUNCTION_SHOULD_HAVE_CACHE_ATTRIBUTE",pd.proc_header);
                var isExtension = fh.proc_attributes.proc_attributes.Any(attr => attr.name == "extensionmethod");
                if (isExtension)
                    throw new SyntaxVisitorError("EXTENSION_FUNCTIONS_SHOULD_NOT_HAVE_CACHE_ATTRIBUTE", fh);
                if (fh.parameters != null && fh.parameters.Count == 0)
                    throw new SyntaxVisitorError("FUNCTIONS_WITH_CACHE_ATTRIBUTE_SHOULD_HAVE_PARAMETERS", fh);
                if (fh.parameters != null && fh.parameters.Count > 7)
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
                var vs = new var_statement(tupleIdent,
                  new method_call(
                      new dot_node(new dot_node("?System", "Tuple"),new ident("Create")),
                      new expression_list(pp.Select(pair => pair.Item1 as expression).ToList())
                  )
                );
                (pd.proc_body as block).program_code.list.Insert(0, vs);

                // var @ИмяФункцииDict := new Dictionary<типTuple, тип возвр значения функции>
                // var d := new PABCSystem.Dictionary<System.Tuple<integer,integer,real>,integer>;
                var ttp = new template_param_list(new List<type_definition>(pp.Select(pair => pair.Item2 as type_definition)));
                var ttr_tuple = new template_type_reference(new named_type_reference(new List<ident> { new ident("System"), new ident("Tuple") }),
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
                var indr = new indexer(dictIdent, new expression_list(tupleIdent));
                var assRes = new assign("Result", indr);
                var exitst = new procedure_call(new ident("exit"));
                var stlst = new statement_list(assRes, exitst);
                var cond = new method_call(new dot_node(dictIdent, new ident("ContainsKey")),
                    new expression_list(tupleIdent));
                var ifstatement = new if_node(cond, stlst);
                (pd.proc_body as block).program_code.list.Insert(1, ifstatement);
                // В конец: d[tup] := Result
                var assEnd = new assign(indr, new ident("Result"));
                (pd.proc_body as block).program_code.list.Add(assEnd);

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
