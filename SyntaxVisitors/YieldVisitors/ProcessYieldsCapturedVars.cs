// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Errors;

using PascalABCCompiler.YieldHelpers;

namespace SyntaxVisitors
{

    public static class CapturedNamesHelper
    {
        public static int CurrentLocalVariableNum = 0;

        public static void Reset()
        {
            CurrentLocalVariableNum = 0;
        }

        public static string MakeCapturedFormalParameterName(string formalParamName)
        {
            return string.Format("<>{0}__{1}", YieldConsts.ReservedNum.MethodFormalParam, formalParamName);
        }

        public static string MakeCapturedLocalName(string localName)
        {
            return string.Format("<{0}>{1}__{2}", localName, YieldConsts.ReservedNum.MethodLocalVariable, ++CurrentLocalVariableNum);
        }
    }

    public class ProcessYieldCapturedVarsVisitor : BaseChangeVisitor
    {
        int clnum = 0;

        public string NewYieldClassName()
        {
            clnum++;
            return "clyield#" + clnum.ToString();
        }

        public FindMainIdentsVisitor mids; // захваченные переменные процедуры по всем её yield 

        public int countNodesVisited;

        public bool hasYields = false;

        public static ProcessYieldCapturedVarsVisitor New
        {
            get { return new ProcessYieldCapturedVarsVisitor(); }
        }

        public ProcessYieldCapturedVarsVisitor()
        {
            //PrintInfo = false; 
        }

        /*public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);
            countNodesVisited++;

            // сокращение обходимых узлов. Как сделать фильтр по тем узлам, которые необходимо обходить? Например, все операторы (без выражений и описаний), все описания (без операторов)
            if (st.GetType()==typeof(assign) || st.GetType()==typeof(var_def_statement) || st is procedure_call || st is procedure_header || st is expression)
            {
                visitNode = false; // фильтр - куда не заходить 
            }
        }*/

        /*public override void visit(class_members cm)
        {
            foreach (var decl in cm.members)
            {
                if (decl is procedure_header || decl is procedure_definition)
                    decl.visit(this);
            }
            base.visit(cm);
        }*/

        type_declarations GenClassesForYield(procedure_definition pd,
            IEnumerable<var_def_statement> fields, // локальные переменные
            IDictionary<string, string> localsMap, // отображение для захваченных имен локальных переменных
            IDictionary<string, string> formalParamsMap//, // отображение для захваченных имен формальных параметров
            //IDictionary<var_def_statement, var_def_statement> localsCloneMap // отображение для оберток локальных переменных 
            ) 
        {
            var fh = (pd.proc_header as function_header);
            if (fh == null)
                throw new SyntaxError("Only functions can contain yields", "", pd.proc_header.source_context, pd.proc_header);
            var seqt = fh.return_type as sequence_type;
            /*if (seqt == null)
                throw new SyntaxError("Functions with yields must return sequences", "", fh.return_type.source_context, fh.return_type);*/

            // Теперь на месте функции генерируем класс

            // Захваченные локальные переменные
            var cm = class_members.Public;
            var capturedFields = fields.Select(vds =>
                                    {
                                        ident_list ids = new ident_list(vds.vars.idents.Select(id => new ident(localsMap[id.name.ToLower()])).ToArray());
                                        if (vds.vars_type == null) //&& vds.inital_value != null)
                                        {
                                            if (vds.inital_value != null)
                                            {
                                                //return new var_def_statement(ids, new yield_unknown_expression_type(localsCloneMap[vds], varsTypeDetectorHelper), null);
                                                return new var_def_statement(ids, new yield_unknown_expression_type(vds), null); // SSM - убрал localsCloneMap[vds] - заменил на vds - не знаю, зачем вообще это отображение делалось - всё равно оно было тождественным!!!
                                            }
                                            else
                                            {
                                                throw new SyntaxVisitorError("Variable defenition without type and value!",vds.source_context); // SSM - быть такого не может - грамматика не пропустит
                                            }
                                        }
                                        else
                                        {
                                            return new var_def_statement(ids, vds.vars_type, null);
                                        }
                                        
                                        //return new var_def_statement(ids, vds.vars_type, vds.inital_value);
                                    });

            foreach (var m in capturedFields)
                cm.Add(m);

            // Параметры функции
            List<ident> lid = new List<ident>();
            var pars = fh.parameters;
            if (pars != null)
                foreach (var ps in pars.params_list)
                {
                    if (ps.param_kind != parametr_kind.none && ps.param_kind != parametr_kind.params_parametr)
                        throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_VAR_CONST_PARAMS_MODIFIERS", pars.source_context);
                    if (ps.inital_value != null)
                        throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_DEFAULT_PARAMETERS", pars.source_context);
                    //var_def_statement vds = new var_def_statement(ps.idents, ps.vars_type);
                    ident_list ids = new ident_list(ps.idents.list.Select(id => new ident(formalParamsMap[id.name.ToLower()])).ToArray());
                    var_def_statement vds = new var_def_statement(ids, ps.vars_type);
                    cm.Add(vds); // все параметры функции делаем полями класса
                    //lid.AddRange(vds.vars.idents);
                    lid.AddRange(ps.idents.list);
                }

            var stels = seqt.elements_type;

            var iteratorClassName = GetClassName(pd);

            /*var staticClass = false;

            var classDef = GetMethodClassDefinition(pd);

            if (classDef != null && (classDef.attribute & class_attribute.Static) != 0)
                staticClass = true;*/

            // frninja 08/18/15 - Для захвата self
            if (iteratorClassName != null /*&& !staticClass*/)
            {
                // frninja 20/04/16 - поддержка шаблонных классов
                var iteratorClassRef = CreateClassReference(iteratorClassName);

                cm.Add(new var_def_statement(YieldConsts.Self, iteratorClassRef));
            }

            var GetEnumeratorBody = new statement_list();
            var ResetBody = new statement_list();
            var nsex = new raise_stmt(new new_expr("System.NotSupportedException"), null);
            ResetBody.Add(nsex);

            // Системные поля и методы для реализации интерфейса IEnumerable
            cm.Add(new var_def_statement(YieldConsts.State, "integer"),
                new var_def_statement(YieldConsts.Current, stels),
                procedure_definition.EmptyDefaultConstructor,
                new procedure_definition("Reset", ResetBody),
                new procedure_definition("MoveNext", "boolean", pd.proc_body),
                new procedure_definition("System.Collections.IEnumerator.get_Current", "object", new assign("Result", YieldConsts.Current)),
                //new procedure_definition("System.Collections.IEnumerable.GetEnumerator", "System.Collections.IEnumerator", new assign("Result", "Self"))
                new procedure_definition("System.Collections.IEnumerable.GetEnumerator", "System.Collections.IEnumerator", GetEnumeratorBody)
                );

            // frninja 20/04/16 - поддержка шаблонных классов
            var yieldClassName = NewYieldClassName();
            var yieldClassHelperName = yieldClassName + "Helper";

            var className = this.CreateHelperClassName(yieldClassName, iteratorClassName, pd);
            var classNameHelper = this.CreateHelperClassName(yieldClassHelperName, iteratorClassName, pd);
            

            var interfaces = new named_type_reference_list("System.Collections.IEnumerator", "System.Collections.IEnumerable");

            // frninja 24/04/16 - поддержка шаблонных классов
            //var td = new type_declaration(classNameHelper, this.CreateHelperClassDefinition(classNameHelper, pd, interfaces, cm));
                //SyntaxTreeBuilder.BuildClassDefinition(interfaces, cm));

            // Изменение тела процедуры
            

            // frninja 20/04/16 - поддержка шаблонных классов
            var stl = new statement_list(new var_statement("$res", new new_expr(this.CreateClassReference(className), new expression_list())));
            

            //stl.AddMany(lid.Select(id => new assign(new dot_node("$res", id), id)));
            stl.AddMany(lid.Select(id => new assign(new dot_node("$res", new ident(formalParamsMap[id.name.ToLower()])), id)));

            // frninja 08/12/15 - захват self
            if (iteratorClassName != null && !pd.proc_header.class_keyword)
            {
                stl.Add(new assign(new dot_node("$res", YieldConsts.Self), new ident("self")));
            }

            stl.Add(new assign("Result", "$res"));

            // New body
            pd.proc_body = new block(stl);

            if (iteratorClassName != null)
            {
                var cd = UpperTo<class_definition>();
                if (cd != null)
                {
                    // Если метод описан в классе 
                    // frninja 10/12/15 - заменить на function_header и перенести описание тела в declarations
                    Replace(pd, fh);
                    var decls = UpperTo<declarations>();
                    if (decls != null)
                    {
                        // frninja 12/05/16 - забыли копировать return
                        //function_header nfh = ObjectCopier.Clone(fh);

                        function_header nfh = fh.TypedClone();
                        nfh.proc_attributes.proc_attributes.RemoveAll(pat => pat.attribute_type == proc_attribute.attr_override);

                        //function_header nfh = new function_header();
                        //nfh.name = new method_name(fh.name.meth_name.name);

                        // Set className
                        nfh.name.class_name = iteratorClassName;
                        //nfh.parameters = fh.parameters;
                        //nfh.proc_attributes = fh.proc_attributes;
                        //nfh.return_type = fh.return_type;

                        procedure_definition npd = new procedure_definition(nfh, new block(stl));

                        // Update header
                        //pd.proc_header.className.class_name = GetClassName(pd);
                        // Add to decls
                        decls.Add(npd);
                    }
                }
            }

            // Второй класс

            var tpl = new template_param_list(stels);

            var IEnumeratorT = new template_type_reference("System.Collections.Generic.IEnumerator", tpl);

            var cm1 = cm.Add( //class_members.Public.Add(
                //procedure_definition.EmptyDefaultConstructor,
                new procedure_definition(new function_header("get_Current", stels), new assign("Result", YieldConsts.Current)),
                new procedure_definition(new function_header("GetEnumerator", IEnumeratorT), GetEnumeratorBody),
                new procedure_definition("Dispose")
            );


            // frninja 20/04/16 - поддержка шаблонных классов
            var interfaces1 = new named_type_reference_list(/*this.CreateClassReference(classNameHelper) as named_type_reference*/);
            var IEnumerableT = new template_type_reference("System.Collections.Generic.IEnumerable", tpl);

            interfaces1.Add(IEnumerableT).Add(IEnumeratorT);

            // frninja 24/04/16 - поддержка шаблонных классов

            // frninja 05/06/16 - фикс. Поддержка where секции
            var helperClassDefinition = this.CreateHelperClassDefinition(className, pd, interfaces1, cm1);
            helperClassDefinition.where_section = this.GetMethodWhereSection(pd);

            var td1 = new type_declaration(className, helperClassDefinition);
            //SyntaxTreeBuilder.BuildClassDefinition(interfaces1, cm1));


            var stl1 = new statement_list(new var_statement("$res", new new_expr(this.CreateClassReference(className), new expression_list())));
            

            stl1.AddMany(lid.Select(id => new assign(new dot_node("$res", new ident(formalParamsMap[id.name.ToLower()])), new ident(formalParamsMap[id.name.ToLower()]))));

            // Переприсваивание self 
            if (iteratorClassName != null && !pd.proc_header.class_keyword)
            {
                stl1.Add(new assign(new dot_node("$res", YieldConsts.Self), new ident(YieldConsts.Self)));
            }

            stl1.Add(new assign("Result", "$res"));


            /*GetEnumeratorBody.Add(new if_node(new bin_expr(new ident(YieldConsts.State), new int32_const(0), Operators.Equal),
                new assign("Result", "Self"),
                stl1));*/ // SSM 06.12.20 - исправление неработающего Batch. Нужно по хорошему вводить доп. состояние - как в C#: -2. Так медленнее, но правильно
            GetEnumeratorBody.Add(stl1);

            var cct = new type_declarations(/*td*/);
            cct.Add(td1);

            return cct;
        }

        private void CollectFormalParams(procedure_definition pd, ISet<var_def_statement> collectedFormalParams)
        {
            if (pd.proc_header.parameters != null)
                collectedFormalParams.UnionWith(pd.proc_header.parameters.params_list.Select(tp => new var_def_statement(tp.idents, tp.vars_type)));
        }

        private void CollectFormalParamsNames(procedure_definition pd, ISet<string> collectedFormalParamsNames)
        {
            if (pd.proc_header.parameters != null)
                collectedFormalParamsNames.UnionWith(pd.proc_header.parameters.params_list.SelectMany(tp => tp.idents.idents).Select(id => id.name.ToLower()));
        }


        /// <summary>
        /// Создает обращение к имени класса по имени класса
        /// </summary>
        /// <param name="className">Имя класса</param>
        /// <returns></returns>
        private type_definition CreateClassReference(ident className)
        {
            if (className is template_type_name)
            {
                return new template_type_reference(
                    new named_type_reference(className),
                    new template_param_list(string.Join(",", (className as template_type_name).template_args.idents.Select(id => id.name)))
                    );
            }
            return new named_type_reference(className);
        }


        /// <summary>
        /// Создает имя вспомогательного класса
        /// </summary>
        /// <param name="helperName">Имя вспомогательного класса</param>
        /// <param name="className">Имя класса</param>
        /// <returns></returns>
        private ident CreateHelperClassName(string helperName, ident className, procedure_definition pd)
        {
            if (className is template_type_name)
            {
                if (pd.proc_header.template_args != null)
                {
                    var template_args = (className as template_type_name).template_args.Clone() as ident_list;
                    template_args.AddMany(pd.proc_header.template_args.idents.ToArray());
                    return new template_type_name(helperName, template_args);
                }
                else
                    return new template_type_name(helperName, (className as template_type_name).template_args);
            }
            else if (pd.proc_header.template_args != null)
            {
                return new template_type_name(helperName, pd.proc_header.template_args);
            }
            return new ident(helperName);
        }

        private class_definition CreateHelperClassDefinition(ident className, procedure_definition pd, named_type_reference_list parents, params class_members[] cms)
        {
            if (className is template_type_name)
            {
                return SyntaxTreeBuilder.BuildClassDefinition(parents, (className as template_type_name).template_args , cms);
            }
            else if (pd.proc_header.template_args != null)
            {
                return SyntaxTreeBuilder.BuildClassDefinition(parents, pd.proc_header.template_args, cms);
            }
            return SyntaxTreeBuilder.BuildClassDefinition(parents, cms);
        }


        /// <summary>
        /// Получает имя класса, в котором описан метод-итератор
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        private ident GetClassName(procedure_definition pd)
        {
            if (pd.proc_header.name.class_name != null)
            {
                // Объявление вне класса его метода
                return pd.proc_header.name.class_name;
            }
            else
            {
                // Объявление функции в классе?
                var classDef = UpperNode(3) as class_definition;
                if (classDef != null)
                {
                    var td = UpperNode(4) as type_declaration;
                    if (td != null)
                    {
                        return td.type_name;
                    }
                }
            }

            return null;
        }

        private class_definition GetMethodClassDefinition(procedure_definition pd)
        {
            if (!IsClassMethod(pd))
            {
                return null;
            }

            var cd = UpperTo<class_definition>();
            if (cd != null)
            {
                // Метод класса описан в классе
                return cd;
            }
            else
            {
                // Метод класса описан вне класса

                cd = UpperTo<declarations>().list
                    .OfType<type_declarations>()
                    .SelectMany(tdecls => tdecls.types_decl)
                    .Where(td => td.type_name.name.ToLower() == GetClassName(pd).name.ToLower())
                    .Select(td => td.type_def as class_definition)
                    .Where(_cd => _cd != null)
                    .DefaultIfEmpty()
                    .First();
                if (cd == null)
                {
                    implementation_node impl = UpperTo<implementation_node>();
                    if (impl != null)
                    {
                        cd = (impl.Parent as unit_module).interface_part.interface_definitions.list
                            .OfType<type_declarations>()
                            .SelectMany(tdecls => tdecls.types_decl)
                            .Where(td => td.type_name.name.ToLower() == GetClassName(pd).name.ToLower())
                            .Select(td => td.type_def as class_definition)
                            .Where(_cd => _cd != null)
                            .DefaultIfEmpty()
                            .First();
                    }
                }
                
                return cd;
            }
        }

        private where_definition_list GetMethodWhereSection(procedure_definition pd)
        {

            where_definition_list res = null;
            if (pd.proc_header.where_defs != null)
            {
                res = ObjectCopier.Clone(pd.proc_header.where_defs);
                //return pd.proc_header.where_defs.TypedClone();
            }
            else
            {
                var pdPredefs = UpperTo<declarations>().defs
                    .OfType<procedure_definition>()
                    .Where(lpd => lpd.proc_body == null
                            && lpd.proc_header.name.meth_name.name.ToLower() == pd.proc_header.name.meth_name.name.ToLower()
                            && lpd.proc_header.proc_attributes.proc_attributes.FindIndex(attr => attr.attribute_type == proc_attribute.attr_forward) != -1);
                if (pdPredefs.Count() > 0)
                {
                    res = ObjectCopier.Clone(pdPredefs.First().proc_header.where_defs);
                    //return pdPredefs.First().proc_header.where_defs.TypedClone();
                }
            }

            if (GetMethodClassDefinition(pd) is class_definition cd && cd.where_section != null)
            {
                if (res==null)
                    res = ObjectCopier.Clone(cd.where_section);
                else
                    res.defs.AddRange(cd.where_section.defs.Select(ObjectCopier.Clone));
            }

            return res;
        }

        /// <summary>
        /// Определяет описан ли метод-итератор в некотором классе
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        private bool IsClassMethod(procedure_definition pd)
        {
            return GetClassName(pd) != null;
        }

        private void CollectClassFieldsNames(procedure_definition pd, ISet<string> collectedFields)
        {
            ident className = GetClassName(pd);

            if (className != null)
            {
                CollectClassFieldsVisitor fieldsVis = new CollectClassFieldsVisitor(className);
                var cu = UpperTo<compilation_unit>();
                if (cu != null)
                {
                    cu.visit(fieldsVis);
                    // Collect
                    collectedFields.UnionWith(fieldsVis.CollectedFields.Select(id => id.name.ToLower()));
                }
            }
        }

        private void CollectClassMethodsNames(procedure_definition pd, ISet<string> collectedMethods)
        {
            ident className = GetClassName(pd);

            if (className != null)
            {
                CollectClassMethodsVisitor methodsVis = new CollectClassMethodsVisitor(className);
                var cu = UpperTo<compilation_unit>();
                if (cu != null)
                {
                    cu.visit(methodsVis);
                    // Collect
                    collectedMethods.UnionWith(methodsVis.CollectedMethods.Select(id => id.name != null ? id.name.ToLower() : null));
                }
            }
        }

        private void CollectClassPropertiesNames(procedure_definition pd, ISet<string> collectedProperties)
        {
            ident className = GetClassName(pd);

            if (className != null)
            {
                CollectClassPropertiesVisitor propertiesVis = new CollectClassPropertiesVisitor(className);
                var cu = UpperTo<compilation_unit>();
                if (cu != null)
                {
                    cu.visit(propertiesVis);
                    // Collect
                    collectedProperties.UnionWith(propertiesVis.CollectedProperties.Select(id => id.name.ToLower()));
                }
            }
        }

        private void CollectUnitGlobalsNames(procedure_definition pd, ISet<string> collectedUnitGlobalsName)
        {
            var cu = UpperTo<compilation_unit>();
            if (cu != null)
            {
                var ugVis = new CollectUnitGlobalsVisitor();
                cu.visit(ugVis);
                // Collect
                collectedUnitGlobalsName.UnionWith(ugVis.CollectedGlobals.Select(id => id.name.ToLower()));
            }
        }

        private void CreateCapturedLocalsNamesMap(ISet<string> localsNames, IDictionary<string, string> capturedLocalsNamesMap)
        {
            foreach (var localName in localsNames)
            {
                capturedLocalsNamesMap.Add(localName, CapturedNamesHelper.MakeCapturedLocalName(localName));
            }
        }

        private void CreateCapturedFormalParamsNamesMap(ISet<string> formalParamsNames, IDictionary<string, string> captueedFormalParamsNamesMap)
        {
            foreach (var formalParamName in formalParamsNames)
            {
                captueedFormalParamsNamesMap.Add(formalParamName, CapturedNamesHelper.MakeCapturedFormalParameterName(formalParamName));
            }
        }

        // frninja 31/05/16
        /// <summary>
        /// Вставляет метод-хелпер
        /// </summary>
        /// <param name="pd">Метод-итератор</param>
        /// <param name="helper">Метод-хелпер</param>
        private void InsertHelperMethod(procedure_definition pd, procedure_definition helper)
        {
            helper.proc_header.is_yield_helper = true;

            if (IsClassMethod(pd))
            {
                var cd = UpperTo<class_definition>();
                if (cd != null)
                {
                    // Метод класса описан в классе
                    var classMembers = UpperTo<class_members>();
                    classMembers.Add(helper);
                }
                else
                {
                    // Метод класса описан вне класса

                    var decls = UpperTo<declarations>();
                    
                    var classMembers = decls.list
                        .Select(decl => decl as type_declarations)
                        .Where(tdecls => tdecls != null)
                        .SelectMany(tdecls => tdecls.types_decl)
                        .Where(td => td.type_name.name.ToLower() == GetClassName(pd).name.ToLower())
                        .Select(td => td.type_def as class_definition)
                        .Where(_cd => _cd != null && _cd.body != null)
                        .SelectMany(_cd => _cd.body.class_def_blocks);
                    implementation_node impl = UpperTo<implementation_node>();
                    if (impl != null)
                    {
                        classMembers = classMembers.Union((impl.Parent as unit_module).interface_part.interface_definitions.list
                        .Select(decl => decl as type_declarations)
                        .Where(tdecls => tdecls != null)
                        .SelectMany(tdecls => tdecls.types_decl)
                        .Where(td => td.type_name.name.ToLower() == GetClassName(pd).name.ToLower())
                        .Select(td => td.type_def as class_definition)
                        .Where(_cd => _cd != null && _cd.body != null)
                        .SelectMany(_cd => _cd.body.class_def_blocks));
                    }

                    // Вставляем предописание метода-хелпера 
                    var helperPredefHeader = ObjectCopier.Clone(helper.proc_header);
                    helperPredefHeader.name.class_name = null;
                    //classMembers.First().members.Insert(0, helperPredefHeader);
                    classMembers.First().members.Add(helperPredefHeader); // SSM bug fix #1474

                    // Вставляем тело метода-хелпера
                    UpperTo<declarations>().InsertBefore(pd, helper);
                }
            }
            else
            {
                UpperTo<declarations>().InsertBefore(pd, helper);
            }
        }

        // frninja 31/05/16
        /// <summary>
        /// Вставляет клон метода-итератора для того, чтобы проверить ошибки существующим бэкендом (пересечение диапазонов case, повторные объявления переменных)
        /// </summary>
        /// <param name="pd"></param>
        private void CreateErrorCheckerHelper(procedure_definition pd)
        {
            // Клонируем исходный метод для проверок ошибок бэкендом
            //var pdCloned = ObjectCopier.Clone(pd);
            var pdCloned = (procedure_definition)pd.Clone();

            pdCloned.proc_header.proc_attributes.proc_attributes.RemoveAll(pat => pat.attribute_type == proc_attribute.attr_override);

            pdCloned.has_yield = false;


            // Добавляем в класс метод с обертками для локальных переменных
            pdCloned.proc_header.name.meth_name = new ident(YieldConsts.YieldHelperMethodPrefix + "_error_checkerr>" + pd.proc_header.name.meth_name.name,
                // frninja 05/06/16 - фиксим source_context
                pd.proc_header.name.meth_name.source_context); // = new method_name("<yield_helper_locals_type_detector>" + pd.proc_header.className.meth_name.className);
            //pdCloned.is_yield_helper = true;

            InsertHelperMethod(pd, pdCloned);
        }

        /// <summary>
        /// Обработка локальных переменных метода и их типов для корректного захвата
        /// </summary>
        /// <param className="pd">Объявление метода</param>
        /// <returns>Коллекция посещенных локальных переменных</returns>
        // SSM - Эта функция уже не нужна - я иногда использую ее чтобы посмотреть как выглядит функция после Loweringа
        private void CreateLocalVariablesTypeProxies(procedure_definition pd, out IEnumerable<var_def_statement> localsClonesCollection)
        {
            // Выполняем определение типов локальных переменных с автовыводом типов

            // Клонируем исходный метод для вставки оберток-хелперов для локальных переменных и дальнейшей обработки на семантике
            var pdCloned = ObjectCopier.Clone(pd);
            //pd.
            //var pdCloned = (procedure_definition)pd.Clone();

            pdCloned.has_yield = false;

            // Заменяем локальные переменные с неизвестным типом на обертки-хелперы (откладываем до семантики)
            var localsTypeDetectorHelperVisitor = new LocalVariablesTypeDetectorHelperVisior();
            pdCloned.visit(localsTypeDetectorHelperVisitor);

            // frninja 16/03/16 - строим список локальных переменных в правильном порядке
            localsTypeDetectorHelperVisitor.LocalDeletedDefs.AddRange(localsTypeDetectorHelperVisitor.LocalDeletedVS);
            localsTypeDetectorHelperVisitor.LocalDeletedVS.Clear();

            localsClonesCollection = localsTypeDetectorHelperVisitor.LocalDeletedDefs.ToArray();

            // Добавляем в класс метод с обертками для локальных переменных
            pdCloned.proc_header.name.meth_name = new ident(YieldConsts.YieldHelperMethodPrefix+ "_locals_type_detector>" + pd.proc_header.name.meth_name.name,
                // frninja 05/06/16 - фиксим source_context
                pd.proc_header.name.meth_name.source_context); // = new method_name("<yield_helper_locals_type_detector>" + pd.proc_header.className.meth_name.className);

            //pdCloned.proc_header.proc_attributes.proc_attributes.RemoveAll(pat => pat.attribute_type == proc_attribute.attr_override);

            InsertHelperMethod(pd, pdCloned); // SSM 13.07.16 - вызов этого метода можно не добавлять
        }

        /// <summary>
        /// Отображение локальных в клонированные локальные
        /// </summary>
        /// <param className="from">Откуда</param>
        /// <param className="to">Куда</param>
        /// <returns>Отображение</returns>
        /*private Dictionary<var_def_statement, var_def_statement> CreateLocalsClonesMap(IEnumerable<var_def_statement> from, IEnumerable<var_def_statement> to)
        {
            // Нужно тк клонировали метод для создания хелпера-определителя типов локальных переменных - Eq не будет работать

            // Строим отображение из локальных переменных клона оригинального метода в локальные переменные основного метода
            Dictionary<var_def_statement, var_def_statement> localsClonesMap = new Dictionary<var_def_statement, var_def_statement>();

            var localsArr = from.ToArray();
            var localsClonesArr = to.ToArray();

            // Create map :: locals -> cloned locals
            for (int i = 0; i < localsArr.Length; ++i)
            {
                localsClonesMap.Add(localsArr[i], localsClonesArr[i]);
            }

            return localsClonesMap;
        }*/

        

        /// <summary>
        /// Вставляем описание классов-хелперов для yield перед методом-итератором в зависимости от его описания
        /// </summary>
        /// <param className="pd">Метод-итератор</param>
        /// <param className="cct">Описание классов-хелперов для yield</param>
        private void InsertYieldHelpers(procedure_definition pd, type_declarations cct)
        {
            if (IsClassMethod(pd))
            {
                var cd = UpperTo<class_definition>();
                if (cd != null)
                {
                    // Если метод класса описан в классе
                    var td = UpperTo<type_declarations>();

                    // frninja 20/04/16 - выпилено 

                    //foreach (var helperName in cct.types_decl.Select(ttd => ttd.type_name))
                    //{
                    //    var helperPredef = new type_declaration(helperName, new class_definition());
                        //td.types_decl.Insert(0, helperPredef);
                    //}

                    // Insert class predefenition!
                    //var iteratorClassPredef = new type_declaration(GetClassName(pd), new class_definition(null));
                    //td.types_decl.Insert(0, iteratorClassPredef);

                    foreach (var helper in cct.types_decl)
                    {
                        td.types_decl.Add(helper);
                    }

                }
                else
                {
                    // Метод класса описан вне класса
                    UpperTo<declarations>().InsertBefore(pd, cct);
                }
            }
            else
            {
                UpperTo<declarations>().InsertBefore(pd, cct);
            }
        }


        /// <summary>
        /// Захватываем имена в методе
        /// </summary>
        /// <param className="pd">Метод-итератор</param>
        /// <param className="deletedLocals">Коллекция удаленных локальных переменных</param>
        /// <param className="capturedLocalsNamesMap">Построенное отображение имен локальных переменных в захваченные имена</param>
        /// <param className="capturedFormalParamsNamesMap">Построенное отображение имен формальных параметров в захваченные имена</param>
        private void ReplaceCapturedVariables(procedure_definition pd,
            IEnumerable<var_def_statement> deletedLocals,
            out IDictionary<string, string> capturedLocalsNamesMap,
            out IDictionary<string, string> capturedFormalParamsNamesMap)
        {
            // Структуры данных под классификацию имен в методе

            // Classification
            ISet<string> CollectedLocalsNames = new HashSet<string>();
            ISet<string> CollectedFormalParamsNames = new HashSet<string>();
            ISet<string> CollectedClassFieldsNames = new HashSet<string>();
            ISet<string> CollectedClassMethodsNames = new HashSet<string>();
            ISet<string> CollectedClassPropertiesNames = new HashSet<string>();
            ISet<string> CollectedUnitGlobalsNames = new HashSet<string>();

            ISet<var_def_statement> CollectedLocals = new HashSet<var_def_statement>();
            ISet<var_def_statement> CollectedFormalParams = new HashSet<var_def_statement>();

            // Map from ident idName -> captured ident idName
            capturedLocalsNamesMap = new Dictionary<string, string>();
            capturedFormalParamsNamesMap = new Dictionary<string, string>();

            // Собираем инфу о именах

            // Collect locals
            CollectedLocals.UnionWith(deletedLocals);
            CollectedLocalsNames.UnionWith(deletedLocals.SelectMany(vds => vds.vars.idents).Select(id => id.name.ToLower()));
            // Collect formal params
            CollectFormalParams(pd, CollectedFormalParams);
            CollectFormalParamsNames(pd, CollectedFormalParamsNames);

            // Collect class fields
            CollectClassFieldsNames(pd, CollectedClassFieldsNames);
            // Collect class methods
            CollectClassMethodsNames(pd, CollectedClassMethodsNames);
            // Collect class properties
            CollectClassPropertiesNames(pd, CollectedClassPropertiesNames);
            // Collect unit globals
            CollectUnitGlobalsNames(pd, CollectedUnitGlobalsNames);

            // Строим отображения для имён захваченных локальных переменных и формальных параметров

            // Create maps :: idName -> captureName
            CreateCapturedLocalsNamesMap(CollectedLocalsNames, capturedLocalsNamesMap);
            CreateCapturedFormalParamsNamesMap(CollectedFormalParamsNames, capturedFormalParamsNamesMap);

            // Выполняем замену захват имён в теле метода
            // AHAHA test!
            ReplaceCapturedVariablesVisitor rcapVis = new ReplaceCapturedVariablesVisitor(
                CollectedLocalsNames,
                CollectedFormalParamsNames,
                CollectedClassFieldsNames,
                CollectedClassMethodsNames,
                CollectedClassPropertiesNames,
                CollectedUnitGlobalsNames,
                capturedLocalsNamesMap,
                capturedFormalParamsNamesMap,
                IsClassMethod(pd),
                pd.proc_header.class_keyword,
                GetClassName(pd)
                );
            // Replace
            (pd.proc_body as block).program_code.visit(rcapVis);
        }

        private bool IsExtensionMethod(procedure_definition pd)
        {
            var tdecls = UpperTo<declarations>().defs.OfType<type_declarations>().SelectMany(tds => tds.types_decl);
            implementation_node impl = UpperTo<implementation_node>();
            if (impl != null)
            {
                tdecls = tdecls.Union((impl.Parent as unit_module).interface_part.interface_definitions.defs.OfType<type_declarations>().SelectMany(tds => tds.types_decl));
            }
            
            var isExtension = pd.proc_header.proc_attributes.proc_attributes.Any(attr => attr.name == "extensionmethod");

            if (isExtension)
            {
                // Метод объявлен как extensionmethod
                
                // !!!!!!!! TODO: Проверить что имя класса не находится в этом модуле.

                // Убираем за ненадобностью имя класса ибо оно указано как тип обязательного параметра self
                
                pd.proc_header.name.class_name = null;
                return true;
            }
            else
            {
                // Если не похоже на метод-расширение или объявление вне класса
                if (pd.proc_header.name.class_name == null)
                    return false;

                
                // Разрешаем только имена типов из этого модуля (не расширения)
                if (!tdecls.Any(td => td.type_name.name.ToLower() == pd.proc_header.name.class_name.name.ToLower()))
                {
                    // Имя в модуле не найдено -> метод расширение описанный без extensionmethod. Ругаемся!!!
                    throw new SyntaxVisitorError("Possible extension-method definintion without extensionmethod keyword. Please use extensionmethod syntax",
                        pd.proc_header.source_context);
                }
                
            }
            
            return false;
        }

        /// <summary>
        /// Вставляет предописание метода-итератора для рекурсивных вызовов, если метод описан вне класса
        /// </summary>
        private bool InsertGlobalIteratorMethodPredefinition(procedure_definition pd)
        {
            if (IsExtensionMethod(pd))  // SSM 17.07.16 - нельзя генерировать предописания для extension-методов!
            {
                return false;
            }

            if (IsClassMethod(pd))
            {
                return false;
            }

            // SSM 20/08/16 и еще нельзя генерировать если это функция, описанная в interface и implementation модуля
            var un = UpperNode(2) as implementation_node;
            if (un != null)
            {
                var cu = UpperNode(3) as unit_module;
                if (cu != null) // а это всегда так
                {
                    var fhh = cu.interface_part.interface_definitions.defs.OfType<function_header>().Where(fh => fh.name.meth_name.name.ToLower()==pd.proc_header.name.meth_name.name.ToLower());
                    if (fhh.Any()) // если еcть в разделе интерфейса такие же имена, то не генерировать предописание 
                        return false;
                    // вообще-то надо сравнивать заголовки - не только имена - но даже так устраняется основная масса ошибок
                }
            }

            var pdPredefs = UpperTo<declarations>().defs
                .OfType<procedure_definition>()
                .Where(lpd => lpd.proc_body == null
                        && lpd.proc_header.name.meth_name.name.ToLower() == pd.proc_header.name.meth_name.name.ToLower()
                        && lpd.proc_header.proc_attributes.proc_attributes.FindIndex(attr => attr.attribute_type == proc_attribute.attr_forward) != -1);


            bool isPredefined = pdPredefs.Count() > 0;

            if (!isPredefined)
            {

                var fh = ObjectCopier.Clone(pd.proc_header as function_header);
                fh.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_forward));

                procedure_definition predef = new procedure_definition() { proc_header = fh };
                // frninja 05/06/16 - для шаблонов с where
                predef.proc_header.where_defs = ObjectCopier.Clone(pd.proc_header.where_defs);

                UpperTo<declarations>().InsertBefore(pd, predef);

                return true;
            }

            return false;
        }

        private void CheckInnerMethodsWithYield(procedure_definition pd)
        {
            var pdWithYields = pd.DescendantNodes(includeSelf: true)
                .OfType<procedure_definition>()
                .Where(npd => npd.has_yield);

            var pddds = pdWithYields.Count();
        }

        public override void visit(procedure_definition pd)
        {
            // frninja 14/06/16 - проверяем наличие yield у вложенных методов (и запрещаем)
            CheckInnerMethodsWithYield(pd);

            if (!pd.has_yield)
                return;

            // frninja 05/06/16 - вставляем предописание если метод-итератор описан не в классе (обычная функция) чтоб работали рекурсивные вызовы
            // Нужен также для верной работы <yield_error> функции, проверяющей разные ошибки на этапе семантики
            // Как только уберу <yield_error>-функцию, от этого тоже можно избавиться
            bool methodPredefCreated = InsertGlobalIteratorMethodPredefinition(pd);

            // frninja 24/05/16 - оборачиваем одиночные операторы в begin..end
            AddBeginEndsVisitor.Accept(pd);

            /*
            // Проверяем проблемы имен для for
            CheckVariablesRedefenitionVisitor checkVarRedefVisitor = new CheckVariablesRedefenitionVisitor(
                new HashSet<string>(
                    pd.proc_header.parameters != null
                    ?
                    pd.proc_header.parameters.params_list.SelectMany(fp => fp.idents.idents.Select(id => id.name))
                    :
                    Enumerable.Empty<string>()));
            pd.visit(checkVarRedefVisitor);
            */

            
            // SSM 21/06 - Выносим yield x -> x
            CapturedLambdaInYieldVisitor.Accept(pd);

            // Выносим выражение из yield в отдельную переменную
            ReplaceYieldExprByVarVisitor.Accept(pd);

            // Раскрываем операторы yield sequence. На семантике они не существуют
            LoweringYieldSequenceVisitor.Accept(pd);

            // frninja 31/05/16 - добавляем метод-хелпер, возьмет на себя проверку разных ошибок уже существующим бэкендом
            CreateErrorCheckerHelper(pd); // SSM 14/07/16 - переставил до переименования переменных чтобы отлавливались ошибки одинаковых имен в разных пространствах имен
            // SSM - можно сделать спец визитор, который бы отлавливал дубли имен - тогда этого не надо

            // Переименовываем одинаковые имена в мини-ПИ: begin var a := 1 end; begin var a := 1 end; 
            RenameSameBlockLocalVarsVisitor.Accept(pd);

            // SSM 01/08/16 - надо захватить и переименовать еще все формальные параметры. 
            // Это решит проблему их изменения в pd при следующем вызове запроса.
            var bb = pd.proc_body as block;
            if (pd.proc_header.parameters != null)
            {
                var fpids = pd.proc_header.parameters.params_list.SelectMany(tp => tp.idents.idents);
                foreach (var v in fpids)
                {
                    var vds = new var_statement(new ident("$fp_"+v.name.ToLower(), v.source_context), v);
                    bb.program_code.AddFirst(vds);
                }
            }

            // Теперь lowering
            LoweringVisitor.Accept(pd);

            // frninja 13/04/16 - убираем лишние begin..end
            DeleteRedundantBeginEnds.Accept(pd);

            // Обработка метода для корректного захвата локальных переменных и их типов
            // - это уже не надо - иногда можно включать чтобы посмотреть, что собой представляет функция после Loweringа
            //IEnumerable<var_def_statement> localsClonesCollection;
            //CreateLocalVariablesTypeProxies(pd, out localsClonesCollection);         

            // frninja 16/11/15: перенес ниже чтобы работал захват для lowered for

            var dld = MoveAllLocalDefsToLists.Accept(pd); // Удалить в локальных и блочных описаниях этой процедуры все переменные и вынести их в отдельный список var_def_statement

            // Строим отображение из локальных переменных клона оригинального метода в локальные переменные основного метода
            //Dictionary<var_def_statement, var_def_statement> localsCloneMap = CreateLocalsClonesMap(dld.LocalDeletedDefs, localsClonesCollection);

            // frninja 08/12/15

            // Выполняем захват имён
            IDictionary<string, string> CapturedLocalsNamesMap;
            IDictionary<string, string> CapturedFormalParamsNamesMap;
            ReplaceCapturedVariables(pd, dld.LocalDeletedDefs, out CapturedLocalsNamesMap, out CapturedFormalParamsNamesMap);

            //mids.vars.Except(dld.LocalDeletedDefsNames); // параметры остались. Их тоже надо исключать - они и так будут обработаны
            // В результате работы в mids.vars что-то осталось. Это не локальные переменные и с ними непонятно что делать

            // Обработать параметры! 
            // Как? Ищем в mids formal_parametrs, но надо выделить именно обращение к параметрам - не полям класса, не глобальным переменным

            var cfa = new ConstructFiniteAutomata(pd.proc_body as block);
            cfa.Transform();

            (pd.proc_body as block).program_code = cfa.res;

            // Конструируем определение класса
            var cct = GenClassesForYield(pd, dld.LocalDeletedDefs, CapturedLocalsNamesMap, CapturedFormalParamsNamesMap/*, localsCloneMap*/); // все удаленные описания переменных делаем описанием класса

            // Вставляем классы-хелперы
            InsertYieldHelpers(pd, cct);

            // frninja 20/05/16 - фикс для повторного обхода
            pd.has_yield = false;

            // frninja 05/06/16 - убираем where-секцию у описания метода. Они уже скопированы куда надо
            /*if (methodPredefCreated)
            {
                pd.proc_header.where_defs = null;
            }*/

            //mids = null; // вдруг мы выйдем из процедуры, не зайдем в другую, а там - оператор! Такого конечно не может быть
        }
    }

    /*class ConstructFiniteAutomata1
    {
        public statement_list res = new statement_list();
        statement_list stl;
        int curState = 0;

        statement_list curStatList;
        statement_list StatListAfterCase = new statement_list();

        case_node cas; // формируемый case

        //private Dictionary<labeled_statement, List<int>> dispatches = new Dictionary<labeled_statement, List<int>>();

        private labeled_statement OuterLabeledStatement;
        private Dictionary<int, labeled_statement> Dispatches = new Dictionary<int,labeled_statement>();

        public ConstructFiniteAutomata1(block bl)
        {
            this.stl = bl.program_code;
        }

        private void AddState(out int stateNumber, out ident resumeLabel)
        {
            stateNumber = curState++;
            resumeLabel = null;
        }

        public void Process(statement st)
        {
            if (!(st is yield_node || st is labeled_statement))
            {
                curStatList.Add(st);
            }
            if (st is yield_node)
            {

                var yn = st as yield_node;
                curState += 1;
                curStatList.AddMany(
                    new assign(YieldConsts.Current, yn.ex),
                    new assign(YieldConsts.State, curState),
                    new assign("Result", true),
                    new procedure_call("exit")
                );

                curStatList = new statement_list();
                
                case_variant cv = new case_variant(new expression_list(new int32_const(curState)), curStatList);
                cas.conditions.variants.Add(cv);
            }
            if (st is labeled_statement)
            {
                var ls = st as labeled_statement;

                // frninja 13/04/16 - диспетчерезация к следующему состоянию
                curStatList.Add(new goto_statement(ls.label_name));

                curStatList = StatListAfterCase;
                curStatList.Add(new labeled_statement(ls.label_name));
                Process(ls.to_statement);

            }
        }

        public void Transform()
        {
            cas = new case_node(new ident(YieldConsts.State));

            curStatList = new statement_list();
            case_variant cv = new case_variant(new expression_list(new int32_const(curState)), curStatList);
            cas.conditions.variants.Add(cv);

            foreach (var st in stl.subnodes)
                Process(st);

            

            // frninja 13/04/16 - фикс для зависающего в последнем состоянии
            var lastStateCV = cas.conditions.variants.Last().exec_if_true as statement_list;
            if (lastStateCV != null)
            {
                lastStateCV.Add(new procedure_call("exit"));
            }

            stl.subnodes = BaseChangeVisitor.SeqStatements(cas, StatListAfterCase).ToList();
            //statement_list res = new statement_list(cas);
            res = stl;
        }
    }*/

    // SSM 07/07/16 - изменен алгоритм генерации конечного автомата на более простой. Вместо case генерируется последовательность if ... goto состояние.
    class ConstructFiniteAutomata
    {
        public statement_list res = new statement_list(); // сюда писать результат
        statement_list stl; // это исходные операторы тела функции
        declarations defs;  // это декларации функции - для добавления меток

        int curState = 0;

        public ConstructFiniteAutomata(block bl)
        {
            this.stl = bl.program_code;
            defs = bl.defs;
        }

        public void Process(statement st)
        {
            if (st is yield_node)
            {
                var yn = st as yield_node;
                curState += 1;
                res.AddMany(
                    new assign(YieldConsts.Current, yn.ex, yn.source_context),
                    new assign(YieldConsts.State, curState),
                    new assign("Result", true),
                    new procedure_call("exit"),
                    new labeled_statement(YieldConsts.LabelStatePrefix+curState.ToString())
                );
            }
            else if (st is labeled_statement)
            {
                var ls = st as labeled_statement;
                res.Add(new labeled_statement(ls.label_name));
                Process(ls.to_statement);
            }
            else
            {
                res.Add(st);
            }
        }

        public void Transform()
        {
            res.Add(new labeled_statement(YieldConsts.LabelStatePrefix+curState.ToString()));

            foreach (var st in stl.subnodes)
                Process(st);

            // добавим метки состояний
            var idseq = Enumerable.Range(0, curState + 1).Select(i => new ident("lbstate#" + i.ToString()));
            var idl = new ident_list(idseq.ToList());
            defs.Add(new label_definitions(idl));

            statement ifgoto = new goto_statement(YieldConsts.LabelStatePrefix + curState.ToString());
            for (var i = curState - 1; i >= 0; i--)
                ifgoto = new if_node(new bin_expr(new ident(YieldConsts.State), new int32_const(i), Operators.Equal),
                    new goto_statement(YieldConsts.LabelStatePrefix + i.ToString()),
                    ifgoto
                    );
            res.AddFirst(ifgoto);
            res.AddFirst(new assign("Result", false));
        }
    }
}
