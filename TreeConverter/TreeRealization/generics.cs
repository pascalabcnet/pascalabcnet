// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Здесь описана реализация generic-типов
//Файлом владеет ssyy.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConverter;

namespace PascalABCCompiler.TreeRealization
{
    //Вспомогательный класс для реализации generic-типов
    public class generic_type_instance_info
    {
        public List<type_node> param_types;
        public generic_instance_type_node pseudo_instance;

        public generic_type_instance_info(List<type_node> _param_types, generic_instance_type_node _pseudo_instance)
        {
            param_types = _param_types;
            pseudo_instance = _pseudo_instance;
        }
    }

    public class generic_function_instance_info
    {
        public List<type_node> param_types;
        public function_node pseudo_instance;

        public generic_function_instance_info(List<type_node> _param_types, function_node _pseudo_instance)
        {
            param_types = _param_types;
            pseudo_instance = _pseudo_instance;
        }
    }

    public class generic_parameter_eliminations
    {
        public bool is_class = false;
        public bool is_value = false;
        public bool has_default_ctor = false;
        public type_node base_class = null;
        public List<type_node> implementing_interfaces = null;

        public static void add_default_ctor(common_type_node param)
        {
            common_method_node cnode = new common_method_node(
                compiler_string_consts.default_constructor_name, param, null,
                param, SemanticTree.polymorphic_state.ps_common,
                SemanticTree.field_access_level.fal_public, null);
            cnode.is_constructor = true;
            param.methods.AddElement(cnode);
            param.add_name(compiler_string_consts.default_constructor_name, new SymbolInfo(cnode));
            param.has_default_constructor = true;
        }

        public static List<generic_parameter_eliminations> make_eliminations_common(List<SemanticTree.ICommonTypeNode> generic_params)
        {
            List<generic_parameter_eliminations> _parameters_eliminations = new List<generic_parameter_eliminations>();
            foreach (type_node t in generic_params)
            {
                generic_parameter_eliminations gpe = new generic_parameter_eliminations();
                gpe.has_default_ctor = generic_convertions.type_has_default_ctor(t, false);
                gpe.is_class = t.is_class;
                gpe.is_value = t.is_value;
                gpe.base_class = t.base_type;
                gpe.implementing_interfaces = new List<type_node>(t.ImplementingInterfaces.Count);
                foreach (type_node interf in t.ImplementingInterfaces)
                {
                    gpe.implementing_interfaces.Add(interf);
                }
                _parameters_eliminations.Add(gpe);
            }
            return _parameters_eliminations;
        }

        public static List<generic_parameter_eliminations> make_eliminations_compiled(Type[] pars)
        {
            List<generic_parameter_eliminations> _parameters_eliminations = new List<generic_parameter_eliminations>();
            foreach (Type t in pars)
            {
                generic_parameter_eliminations gpe = new generic_parameter_eliminations();
                gpe.has_default_ctor =
                    ((t.GenericParameterAttributes &
                    GenericParameterAttributes.DefaultConstructorConstraint) != 0);
                gpe.is_class =
                    ((t.GenericParameterAttributes &
                    GenericParameterAttributes.ReferenceTypeConstraint) != 0);
                gpe.is_value =
                    ((t.GenericParameterAttributes &
                    GenericParameterAttributes.NotNullableValueTypeConstraint) != 0);
                gpe.base_class = compiled_type_node.get_type_node(t.BaseType);
                Type[] net_interf = t.GetInterfaces();
                gpe.implementing_interfaces = new List<type_node>(net_interf.Length);
                foreach (Type net_t in net_interf)
                {
                    gpe.implementing_interfaces.Add(compiled_type_node.get_type_node(net_t));
                }
                _parameters_eliminations.Add(gpe);
            }
            return _parameters_eliminations;
        }

        public static CompilationErrorWithLocation check_type_generic_useful(type_node tn, location loc)
        {
            if (tn == null)
            {
                return new SimpleSemanticError(loc, "TYPE_NAME_EXPECTED");
            }
            if (tn.IsPointer)
            {
                return new SimpleSemanticError(loc, "CANNOT_USE_POINTER_AS_GENERIC_ARGUMENT");
            }
            switch (tn.type_special_kind)
            {
                case PascalABCCompiler.SemanticTree.type_special_kind.diap_type:
                    return new SimpleSemanticError(loc, "CANNOT_USE_DIAPASON_AS_GENERIC_ARGUMENT");
                case PascalABCCompiler.SemanticTree.type_special_kind.typed_file:
                    return new SimpleSemanticError(loc, "CANNOT_USE_TYPED_FILE_AS_GENERIC_ARGUMENT");
                case PascalABCCompiler.SemanticTree.type_special_kind.short_string:
                    return new SimpleSemanticError(loc, "CANNOT_USE_SHORT_STRING_AS_GENERIC_ARGUMENT");
            }
            /*if (tn == SystemLibrary.SystemLibrary.void_type)
            {
                return new VoidNotValid(loc);
            }*/
            SystemLibrary.SystemLibrary.syn_visitor.check_for_type_allowed(tn,loc);
            internal_interface ii = tn.get_internal_interface(internal_interface_kind.bounded_array_interface);
            if (ii != null)
            {
                return new SimpleSemanticError(loc, "CANNOT_USE_BOUNDED_ARRAY_AS_GENERIC_ARGUMENT");
            }
            return null;
        }

        public static CompilationErrorWithLocation check_type_list(List<type_node> tparams, List<generic_parameter_eliminations> gpe_list, bool method_param_types, out int i)
        {
            int count = tparams.Count;
            for (i = 0; i < count; i++)
            {
                generic_parameter_eliminations gpe = gpe_list[i];
                type_node tn = tparams[i];
                if (gpe.is_class && !tn.is_class)
                {
                    return new SimpleSemanticError(null, "PARAMETER_{0}_MUST_BE_REFERENCE_TYPE", tn.PrintableName);
                }
                if (gpe.is_value && !tn.is_value && !tn.is_generic_parameter)
                {
                    return new SimpleSemanticError(null, "PARAMETER_{0}_MUST_BE_VALUE_TYPE", tn.PrintableName);
                }
                if (gpe.base_class != null && gpe.base_class != SystemLibrary.SystemLibrary.object_type)
                {
                    type_node base_type = generic_convertions.determine_type(gpe.base_class, tparams, method_param_types);
                    if (base_type != tn && !type_table.is_derived(base_type, tn) && !tn.is_generic_parameter)
                    {
                        return new SimpleSemanticError(null, "PARAMETER_{0}_MUST_BE_DERIVED_FROM_{1}", tn.PrintableName, base_type.name);
                    }
                }
                foreach (type_node interf in gpe.implementing_interfaces)
                {
                    type_node di = generic_convertions.determine_type(interf, tparams, method_param_types);
                    if (di != tn &&
                        (tn.ImplementingInterfaces == null ||
                        !type_table.is_derived(di, tn)))
                    {
                        return new SimpleSemanticError(null, "PARAMETER_{0}_MUST_IMPLEMENT_INTERFACE_{1}", tn.PrintableName, di.name);
                    }
                }
                if (gpe.has_default_ctor)
                {
                    if (tn.IsAbstract || !tn.is_value && !generic_convertions.type_has_default_ctor(tn, false))
                    {
                        return new SimpleSemanticError(null, "PARAMETER_{0}_MUST_HAVE_DEFAULT_CONSTRUCTOR", tn.PrintableName);
                    }
                }
            }
            return null;
        }

    }

    public class type_instance_and_location
    {
        public generic_instance_type_node instance;
        public location loc;

        public type_instance_and_location(generic_instance_type_node _instance, location _loc)
        {
            instance = _instance;
            loc = _loc;
        }
    }

    //Вспомогательный класс для создания псевдо-инстанций generic-типов.
    //TODO: sdelat singletonom. staticheskie klassy eto gadost
    public static class generic_convertions
    {
        //Список, хранящий все псевдо-инстанции generic-типов, нужен для NetGenerator.
        public static List<SemanticTree.IGenericTypeInstance> all_type_instances =
            new List<SemanticTree.IGenericTypeInstance>();

        public static List<SemanticTree.IGenericFunctionInstance> all_function_instances =
            new List<SemanticTree.IGenericFunctionInstance>();

        public static Hashtable generic_instances = new Hashtable();

        public static syntax_tree_visitor visitor;

        public static List<generic_type_instance_info> get_type_instances(type_node original_generic_type)
        {
            List<generic_type_instance_info> instances = generic_instances[original_generic_type] as List<generic_type_instance_info>;
            if (instances == null)
            {
                instances = new List<generic_type_instance_info>();
                generic_instances.Add(original_generic_type, instances);
            }
            return instances;
        }

        public static List<generic_function_instance_info> get_function_instances(function_node original_generic_function)
        {
            List<generic_function_instance_info> instances = generic_instances[original_generic_function] as List<generic_function_instance_info>;
            if (instances == null)
            {
                instances = new List<generic_function_instance_info>();
                generic_instances.Add(original_generic_function, instances);
            }
            return instances;
        }

        public static type_node find_type_instance(List<generic_type_instance_info> _generic_instances, List<type_node> param_types)
        {
            int count = param_types.Count;
            foreach (generic_type_instance_info gii in _generic_instances)
            {
                bool equals = true;
                for (int i = 0; equals && i < count; ++i)
                {
                    equals = (param_types[i] == gii.param_types[i]);
                }
                if (equals)
                {
                    //Такая инстанция уже есть
                    return gii.pseudo_instance;
                }
            }
            return null;
        }

        public static void check_instances_are_correct(List<type_instance_and_location> insts)
        {
            int num;
            foreach (type_instance_and_location tia in insts)
            {
                List<generic_parameter_eliminations> gpes = tia.instance.original_generic.parameters_eliminations;
                CompilationErrorWithLocation err = generic_parameter_eliminations.check_type_list(tia.instance.instance_params, gpes, false, out num);
                if (err != null)
                {
                    err.loc = tia.loc;
                    throw err;
                }
            }
        }

        public static function_node find_function_instance(List<generic_function_instance_info> _generic_instances, List<type_node> param_types)
        {
            int count = param_types.Count;
            foreach (generic_function_instance_info gii in _generic_instances)
            {
                bool equals = true;
                for (int i = 0; equals && i < count; ++i)
                {
                    equals = (param_types[i] == gii.param_types[i]);
                }
                if (equals)
                {
                    //Такая инстанция уже есть
                    return gii.pseudo_instance;
                }
            }
            return null;
        }

        public static void init_generic_instance(type_node original, generic_instance_type_node instance, /*SymbolTable.ClassScope instance_scope,*/ List<type_node> param_types)
        {
            instance.IsInterface = original.IsInterface;
            instance.is_class = original.is_class;
            instance.internal_is_value = original.is_value;
            instance.SetIsSealed(original.IsSealed);
            instance.IsDelegate = original.IsDelegate;
            instance.type_special_kind = original.type_special_kind;

            //Определяем базовый тип
            type_node btype = determine_type(
                original.base_type, param_types, false);
            instance.SetBaseTypeIgnoringScope(btype);
            
            //instance._scope = new SymbolTable.GenericTypeInstanceScope(instance, instance.original_generic.Scope, btype.Scope);

            foreach (type_node interf in original.ImplementingInterfaces)
            {
                instance.ImplementingInterfaces.Add(
                    determine_type(interf, param_types, false)
                    );
            }

            SystemLibrary.SystemLibrary.init_reference_type(instance);
            instance.conform_basic_functions();
            //(ssyy) Нужно, чтобы добавились конструкторы
            //ctnode.find_in_type(compiler_string_consts.default_constructor_name);
            instance.instance_params = param_types;

            property_node orig_pn = original.default_property_node;
            if (orig_pn != null)
            {
                if (orig_pn.comprehensive_type == original)
                {
                    //Свойство по умолчанию описано в оригинальном коде generic-a;
                    //конвертируем его
                    instance.default_property = instance.ConvertMember(orig_pn) as common_property_node;
                }
                else
                {
                    //Свойство по умолчанию описано в каком-то предке оригинального generic-a
                    if (orig_pn.comprehensive_type.is_generic_type_definition)
                    {
                        instance.default_property = instance.find_instance_type_from(orig_pn.comprehensive_type).default_property;
                    }
                }
            }

            var shouldAddToAllTypeInstances = true;
            if (LambdaHelper.processingLambdaParametersForTypeInference != 0)
            {
                foreach (var par in instance.generic_parameters)
                {
                    if (par is lambda_any_type_node)
                    {
                        shouldAddToAllTypeInstances = false;
                        break;
                    }
                }
            }

            if (shouldAddToAllTypeInstances) //lroman// Если зашли сюда при выведении типов параметров лямбды, то тип инстанцироваться может с типом lambda_any_type_node. Поэтому, если выводим типы. То данную инстанцию не добавляем
            {
                if (instance.instance_params[0] is ienumerable_auto_type) // SSM 10.07.16 (yields) в эту таблицу не включаются типы IEnumerable<ienumerable_auto_type>, т.к. потом они всё равно автовыводятся
                {
                    //instance = instance;
                }
                else
                {
                    generic_convertions.all_type_instances.Add(instance);
                }
            }

            internal_interface ii = original.get_internal_interface(internal_interface_kind.delegate_interface);
            if (ii != null)
            {
                delegate_internal_interface dii = ii as delegate_internal_interface;
                common_method_node inv = instance.ConvertMember(dii.invoke_method) as common_method_node;
                common_method_node constr = instance.ConvertMember(dii.constructor) as common_method_node;
                constr.function_code = new runtime_statement(SemanticTree.runtime_statement_type.ctor_delegate, null);
                delegate_internal_interface converted_dii = new delegate_internal_interface(inv.return_value_type,
                    inv, constr);
                converted_dii.parameters.AddRange(inv.parameters);
                instance.add_internal_interface(converted_dii);
            }
        }

        public static void reset_generics()
        {
            generic_instances.Clear();
            all_type_instances.Clear();
            all_function_instances.Clear();
        }

        //Определяет, как должен выглядеть тип в семантическом дереве.
        //Возвращает этот тип.
        public static type_node determine_type(Type t, List<type_node> param_types, bool method_param_types)
        {
            if (t == null) return null;
            if (t.IsGenericParameter)
            {
                //Если мы определяем тип-параметры метода, нет необходимости рассматривать
                //тип-параметры типа, и наоборот. Это для поддержки generic-методов в
                //generic-типе.
                if (method_param_types == (t.DeclaringMethod != null))
                {
                    return param_types[t.GenericParameterPosition];
                }
                else
                {
                    return compiled_type_node.get_type_node(t);
                }
            }
            if (t.IsGenericType)
            {
                //Формируем список типов-параметров
                Type[] args = t.GetGenericArguments();
                List<type_node> semantic_args = new List<type_node>();
                foreach (Type arg in args)
                {
                    semantic_args.Add(determine_type(arg, param_types, method_param_types));
                }

                //Получаем описание generic-класса
                Type def = t.GetGenericTypeDefinition();
                compiled_type_node ct_def = compiled_type_node.get_type_node(def);

                //Создаём псевдо-инстанцию
                return ct_def.get_instance(semantic_args);
            }

            //Для типов, не имеющих отношения к generic-типам.
            return compiled_type_node.get_type_node(t);
        }

        public static type_node determine_type(type_node tn, List<type_node> param_types, bool method_param_types)
        {
            if (tn == null) return null;
            ref_type_node rtn = tn as ref_type_node;
            if (rtn != null)
            {
                type_node ptype = generic_convertions.determine_type(rtn.pointed_type, param_types, method_param_types);
                if (ptype == rtn.pointed_type) return tn;
                ref_type_node rez_ref = ptype.ref_type;
                rez_ref.loc = rtn.loc;
                return rez_ref;
            }
            array_internal_interface ii = tn.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
            if (ii != null)
            {
                type_node elem_tp = determine_type(ii.element_type, param_types, method_param_types);
                if (elem_tp != ii.element_type)
                {
                    return SystemLibrary.SystemLibrary.syn_visitor.convertion_data_and_alghoritms.type_constructor.create_unsized_array(elem_tp, null, ii.rank, null);
                }
                return tn;
            }
            common_type_node comm_type = tn as common_type_node;
            if (comm_type != null)
            {
                if (comm_type.is_generic_parameter)
                {
                    if (method_param_types && comm_type.generic_function_container != null)
                    {
                        return param_types[comm_type.generic_param_index];
                    }
                    else if (!method_param_types && comm_type.generic_type_container != null)
                    {
                        return param_types[comm_type.generic_param_index];
                    }
                    return tn;
                }
                generic_instance_type_node gitn = tn as generic_instance_type_node;
                if (gitn != null)
                {
                    List<type_node> semantic_args = new List<type_node>();
                    List<type_node> gitn_inst_parameters = gitn.instance_params;
                    foreach (type_node arg in gitn_inst_parameters)
                    {
                        semantic_args.Add(determine_type(arg, param_types, method_param_types));
                    }
                    return gitn.original_generic.get_instance(semantic_args);
                }
                if (comm_type.is_generic_type_definition)
                {
                    return comm_type.get_instance(param_types);
                }
                if (comm_type.type_special_kind == SemanticTree.type_special_kind.array_kind)
                {
                    type_node elem_tp = determine_type(comm_type.element_type, param_types, method_param_types);
                    if (elem_tp != comm_type.element_type)
                    {
                        return SystemLibrary.SystemLibrary.syn_visitor.convertion_data_and_alghoritms.type_constructor.create_unsized_array(elem_tp, null, 1, comm_type.loc);
                    }
                    return tn;
                }
                if (comm_type.type_special_kind == SemanticTree.type_special_kind.set_type)
                {
                    type_node elem_tp = determine_type(comm_type.element_type, param_types, method_param_types);
                    if (elem_tp != comm_type.element_type)
                    {
                        return SystemLibrary.SystemLibrary.syn_visitor.context.create_set_type(elem_tp, comm_type.loc);
                    }
                    return tn;
                }
                if (comm_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.typed_file)
                {
                    type_node elem_tp = determine_type(comm_type.element_type, param_types, method_param_types);
                    if (elem_tp != comm_type.element_type)
                    {
                        return SystemLibrary.SystemLibrary.syn_visitor.context.create_typed_file_type(elem_tp, comm_type.loc);
                    }
                    return tn;
                }
                return tn;
            }
            compiled_type_node ctn = tn as compiled_type_node;
            if (ctn == null || (!ctn.compiled_type.IsGenericType && !ctn.compiled_type.IsGenericParameter))
            {
                return tn;
            }
            return determine_type(ctn.compiled_type, param_types, method_param_types);
        }

        private static bool CheckIfTypeDependsOnUndeducedGenericParameters(type_node formalType, type_node[] deduced) //lroman
        {
            if (formalType.generic_function_container != null)
            {
                var par_num = formalType.generic_param_index;
                return deduced[par_num] == null;
            }

            var formalRef = formalType as ref_type_node;
            if (formalRef != null)
            {
                return CheckIfTypeDependsOnUndeducedGenericParameters(formalRef.pointed_type, deduced);
            }

            var formalIi = formalType.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
            if (formalIi != null)
            {
                return CheckIfTypeDependsOnUndeducedGenericParameters(formalIi.element_type, deduced);
            }

            if (formalType.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.set_type)
            {
                return CheckIfTypeDependsOnUndeducedGenericParameters(formalType.element_type, deduced);
            }

            if (formalType.IsDelegate)
            {
                var dii = formalType.get_internal_interface(internal_interface_kind.delegate_interface) as delegate_internal_interface;
                var paramCount = dii.parameters.Count;
                for (var i = 0; i < paramCount; i++)
                {
                    if (!CheckIfTypeDependsOnUndeducedGenericParameters(dii.parameters[i].type, deduced))
                    {
                        return false;
                    }
                }
                return CheckIfTypeDependsOnUndeducedGenericParameters(dii.return_value_type, deduced);
            }

            if (formalType.is_generic_type_instance)
            {
                var pcount = formalType.instance_params.Count;
                for (var k = 0; k < pcount; ++k)
                {
                    if (!CheckIfTypeDependsOnUndeducedGenericParameters(formalType.instance_params[k], deduced))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        //Сохранить типы параметров и тип возвращаемого значения лямбд перед попыткой вычисления реального типа возвращаемого знаяения лямбд
        private static Dictionary<string, Tuple<List<type_node>, type_node>>  SaveLambdasStates(IEnumerable<function_lambda_definition> lambdaParametersList)
        {
            var savedLambdasStates = new Dictionary<string, Tuple<List<type_node>, type_node>>();

            foreach (var lambda in lambdaParametersList)
            {
                var parsTypes = new List<type_node>();
                if (lambda.formal_parameters != null && lambda.formal_parameters.params_list != null &&
                    lambda.formal_parameters.params_list.Count > 0)
                {
                    parsTypes.AddRange(lambda
                                           .formal_parameters
                                           .params_list
                                           .Select(t =>
                                               {
                                                   var lambdaNode = t.vars_type as lambda_inferred_type;
                                                   if (lambdaNode == null)
                                                   {
                                                       return null;
                                                   }
                                                   return lambdaNode.real_type is lambda_any_type_node
                                                              ? (lambda_any_type_node) lambdaNode.real_type
                                                              : null;
                                               }));
                }

                type_node retType;
                var lambdaResNode = lambda.return_type as lambda_inferred_type;
                if (lambdaResNode == null)
                {
                    retType = null;
                }
                else
                {
                    retType = lambdaResNode.real_type is lambda_any_type_node
                                ? (lambda_any_type_node)lambdaResNode.real_type
                                : null;
                }

                savedLambdasStates.Add(lambda.lambda_name, 
                    new Tuple<List<type_node>, type_node>(parsTypes, retType));
            }

            return savedLambdasStates;
        }
        
        //Восстановление типов в исходное состояние на случай подстановки других типов
        private static void RestoreLambdasStates(List<function_lambda_definition> lambdaParametersList,
                                                 Dictionary<string, Tuple<List<type_node>, type_node>> savedLambdasStates)
        {
            for (var i = 0; i < lambdaParametersList.Count; i++)
            {
                var state = savedLambdasStates[lambdaParametersList[i].lambda_name];
                var lambdaPar = lambdaParametersList[i];
                for (var k = 0; k < state.Item1.Count; k++)
                {
                    var lambdaNode = lambdaPar.formal_parameters.params_list[k].vars_type as lambda_inferred_type;
                    if (lambdaNode != null
                        && state.Item1[k] != null)
                    {
                        lambdaNode.real_type = state.Item1[k];
                    }
                }

                var resLambdaNode = lambdaPar.return_type as lambda_inferred_type;
                if (resLambdaNode != null
                    && state.Item2 != null)
                {
                    resLambdaNode.real_type = state.Item2;
                }
            }
        }

        //Попытка вычислить типы после подстановки типов формальных параметров
        public static bool TryToDeduceTypesInLambda(function_lambda_definition lambda_syntax_node,
                                                    delegate_internal_interface formal_delegate, 
                                                    type_node[] deduced, List<int> nils,
                                                    out Exception exception_on_body_compilation)
        {
            var there_are_undeduced_params = false;
            var param_counter = 0;
            var visitor = SystemLibrary.SystemLibrary.syn_visitor;
            var result = true;
            exception_on_body_compilation = null;

            /*if (lambda_syntax_node.formal_parameters == null
                || lambda_syntax_node.formal_parameters.params_list == null
                || lambda_syntax_node.formal_parameters.params_list.Count == 0)
            {
                return false;
            }*/
            if (lambda_syntax_node.formal_parameters != null)
            foreach (var t in lambda_syntax_node.formal_parameters.params_list)
            {
                var lambdaInfType = t.vars_type as lambda_inferred_type;
                if (lambdaInfType != null
                    && lambdaInfType.real_type is lambda_any_type_node)
                {
                    if (formal_delegate == null) // SSM 5.12.15
                    {
                        return false;
                    }
                    if (!CheckIfTypeDependsOnUndeducedGenericParameters(formal_delegate.parameters[param_counter].type, deduced)) //Если тип параметра не зависит от невыведенных дженерик-параметров, то можем вычислить этот тип явно
                    {
                        lambdaInfType.real_type = generic_convertions.determine_type(formal_delegate.parameters[param_counter].type,
                                                                           deduced.ToList(),
                                                                           true); //инстанцируем и записываем вычесленный тип
                    }
                    else
                    {
                        there_are_undeduced_params = true; //иначе мы не сможем вывести тип возвращаемого значения
                        break;
                    }
                }
                param_counter += t.idents.idents.Count;
            }

            if (!there_are_undeduced_params
                && lambda_syntax_node.return_type is lambda_inferred_type
                && ((lambda_inferred_type)lambda_syntax_node.return_type).real_type is lambda_any_type_node)
            {
                var lambdaName = lambda_syntax_node.lambda_name;
                var fl = lambda_syntax_node.lambda_visit_mode;
                lambda_syntax_node.lambda_visit_mode = LambdaVisitMode.VisitForAdvancedMethodCallProcessing;
                var aux_name = LambdaHelper.GetAuxiliaryLambdaName(lambda_syntax_node.lambda_name);
                lambda_syntax_node.lambda_name = aux_name;

                try
                {
                    visitor.visit(lambda_syntax_node); //пробуем скомпилировать тело лямбды, вычислим тип возвращаемого значения
                }
                catch (Exception exc)
                {
                    exception_on_body_compilation = exc; // Если произошло исключение то запишем его в выходной параметр, оно потом будет обработано вызывающим методом
                    result = false;
                }
                finally
                {
                    var context = visitor.context;

                    //Далее надо удалить ненужный узел из списка функций
                    LambdaHelper.RemoveLambdaInfoFromCompilationContext(context, lambda_syntax_node);
                    
                    lambda_syntax_node.lambda_name = lambdaName;
                    lambda_syntax_node.lambda_visit_mode = fl;

                    if (result)
                    {
                        if (formal_delegate == null) // SSM 5.12.15
                        {
                            result = false;
                        }
                        else
                        { 
                            if (formal_delegate.return_value_type==null) // SSM 19/04/16 - эта проверка в связи с падением при передаче функции вместо процедуры в качестве функционального параметра: a.Foreach(x->1)
                            {
                                result = false;
                            }
                            else if (!DeduceInstanceTypes(formal_delegate.return_value_type,
                                                     (type_node)((lambda_inferred_type)lambda_syntax_node.return_type).real_type,
                                                     deduced, nils)) //Выводим дженерик-параметры после того как вычислили тип возвращаемого значения
                            {
                                result = false;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static function_node DeduceFunction(function_node func, expressions_list fact, bool alone, compilation_context context, location loc, List<SyntaxTree.expression> syntax_nodes_parameters = null)
        {
            parameter_list formal = func.parameters;
            int formal_count = formal.Count;
            int fact_count = fact.Count;
            int generic_type_params_count = func.generic_parameters_count;
            type_node[] deduced = new type_node[generic_type_params_count];
            List<int> nils = new List<int>();
            int count_params_to_see = fact_count;

            var lambda_syntax_nodes = new Dictionary<string, function_lambda_definition>(); //lroman Получим список фактических параметров-лямбд текущей вызываемой подпрограммы
            if (syntax_nodes_parameters != null
                && syntax_nodes_parameters.Count > 0) //lroman
            {
                lambda_syntax_nodes = syntax_nodes_parameters
                    .OfType<function_lambda_definition>()
                    .ToDictionary(f => f.lambda_name, f => f);
            }

            var lambda_in_parameters = lambda_syntax_nodes.Count > 0; 
            var saved_lambdas_states = SaveLambdasStates(lambda_syntax_nodes.Select(ld => ld.Value)); // Сохраним типы лямбды перед вычислениями

            if (fact_count < formal_count)
            {
                //Сравниваем количества параметров
                parameter par = formal[fact_count];
                if (par.default_value == null && !par.is_params)
                {
                    if (alone)
                        throw new NoFunctionWithSameParametresNum(loc, alone, func);
                    return null;
                }
            }
            else
            {
                type_node last_params_type = null;
                bool last_is_params = false;
                parameter par = null;
                if (formal_count > 0)
                {
                    par = formal[formal_count - 1];
                    last_is_params = par.is_params;
                }
                if (last_is_params)
                {
                    array_internal_interface aii = par.type.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
                    last_params_type = aii.element_type;
                }
                if (fact_count > formal_count)
                {
                    //Фактических больше, чем формальных. Последний формальный должен быть params...
                    if (last_is_params)
                    {
                        for (int i = formal_count - 1; i < fact_count; ++i)
                        {
                            //Проверяем фактические, попадающие под params...
                            if (!DeduceInstanceTypes(last_params_type, fact[i].type, deduced, nils))
                            {
                                if (alone)
                                    throw new SimpleSemanticError(loc, "GENERIC_FUNCTION_{0}_CAN_NOT_BE_CALLED_WITH_THESE_PARAMETERS", func.name);
                                return null;
                            }
                        }
                        count_params_to_see = formal_count - 1;
                    }
                    else
                    {
                        if (alone)
                            throw new NoFunctionWithSameParametresNum(loc, alone, func);
                        return null;
                    }
                }
                else if (last_is_params)
                {
                    for (int i = formal_count - 1; i < fact_count; ++i)
                    {
                        //Проверяем фактические, попадающие под params...
                        type_node tn = fact[i].type;
                        if (tn.element_type != null && tn.type_special_kind != SemanticTree.type_special_kind.array_wrapper)
                            tn = tn.element_type;
                        if (!DeduceInstanceTypes(last_params_type, tn, deduced, nils))
                        {
                            if (alone)
                                throw new SimpleSemanticError(loc, "GENERIC_FUNCTION_{0}_CAN_NOT_BE_CALLED_WITH_THESE_PARAMETERS", func.name);
                            return null;
                        }
                    }
                    count_params_to_see = formal_count - 1;
                }
            }
            bool need_params_work = (count_params_to_see > 0 && formal[count_params_to_see - 1].is_params);
            if (need_params_work)
            {
                count_params_to_see -= 1;
            }

            var continue_trying_to_infer_types = true; 
            Dictionary<string, delegate_internal_interface> formal_delegates = null;

            var testIsTypeclassRestricted = func.Attributes?.Any(x => x.AttributeType.name == "__TypeclassRestrictedFunctionAttribute");
            var isTypeclassRestricted = testIsTypeclassRestricted.HasValue && testIsTypeclassRestricted.Value;
            var typeclasses = func.get_generic_params_list().Where(t => t.Attributes != null && t.Attributes.Any(a => a.AttributeType != null && a.AttributeType.name == "__TypeclassGenericParameterAttribute"));
            while (continue_trying_to_infer_types) //Продолжаем пытаться вычислить типы до тех пор пока состояние о выведенных типах не будет отличаться от состояния на предыдущей итерации
            {
                var previous_deduce_state = deduced // Текущее состояние выведенных на данный момент типов. Простой список индексов с уже выведенными типами из массива deduced
                        .Select((t, i) => new { Type = t, Index = i })
                        .Where(t => t.Type != null)
                        .Select(t => t.Index)
                        .ToArray();

                for (int i = 0; i < count_params_to_see; ++i)
                {
                    if (alone && fact[i].type is delegated_methods && (fact[i].type as delegated_methods).empty_param_method != null && DeduceInstanceTypes(formal[i].type, (fact[i].type as delegated_methods).empty_param_method.type, deduced, nils))
                        continue;
                    else
                    if (!DeduceInstanceTypes(formal[i].type, fact[i].type, deduced, nils))
                    {
                        if (alone && fact[i].type is delegated_methods && (fact[i].type as delegated_methods).empty_param_method != null)
                        {
                            if (DeduceInstanceTypes(formal[i].type, (fact[i].type as delegated_methods).empty_param_method.type, deduced, nils))
                                continue;
                        }
                        if (alone)
                            throw new SimpleSemanticError(loc, "GENERIC_FUNCTION_{0}_CAN_NOT_BE_CALLED_WITH_THESE_PARAMETERS", func.name);
                        RestoreLambdasStates(lambda_syntax_nodes.Values.ToList(), saved_lambdas_states);
                        return null;
                    }

                }

                if (lambda_in_parameters)
                {
                    if (formal_delegates == null)
                    {
                        formal_delegates = new Dictionary<string, delegate_internal_interface>();

                        for (int i = 0; i < count_params_to_see; ++i)
                            //Выделим из формальных параметров те, которые соотвтествуют фактическим параметрам-лямбдам
                        {
                            var lambda_func = fact[i].type as delegated_methods;
                            if (lambda_func != null
                                && lambda_func.proper_methods.Count == 1
                                && LambdaHelper.IsLambdaName(lambda_func.proper_methods[0].simple_function_node.name))
                            {
                                formal_delegates.Add(LambdaHelper.GetLambdaNamePartWithoutGenerics(lambda_func.proper_methods[0].simple_function_node.name),
                                                     formal[i].type.get_internal_interface(
                                                         internal_interface_kind.delegate_interface) as
                                                     delegate_internal_interface);
                            }
                        }
                    }

                    foreach (var formal_delegate in formal_delegates)
                    // Перебираем все полученные формальные параметры, соответствующие фактическим лямбдам
                    {
                        var lambda_syntax_node = lambda_syntax_nodes[formal_delegate.Key];
                        Exception on_lambda_body_compile_exception;
                        // Исключение которое может возникнуть в результате компиляции тела лямбды если мы выберем неправильные типы параметров
                        var b = TryToDeduceTypesInLambda(lambda_syntax_node, formal_delegate.Value, deduced, nils,
                                                      out on_lambda_body_compile_exception);
                        if (!b)
                            // Пробуем вычислить типы из лямбд
                        {
                            RestoreLambdasStates(lambda_syntax_nodes.Values.ToList(), saved_lambdas_states);

                            if (on_lambda_body_compile_exception != null)
                            {
                                if (alone)
                                {
                                    throw on_lambda_body_compile_exception;
                                }

                                throw new FailedWhileTryingToCompileLambdaBodyWithGivenParametersException(
                                    on_lambda_body_compile_exception);
                            }

                            if (alone)
                            {
                                throw new SimpleSemanticError(loc, "GENERIC_FUNCTION_{0}_CAN_NOT_BE_CALLED_WITH_THESE_PARAMETERS", func.name);
                            }
                            return null;
                        }
                    }
                }

                if (isTypeclassRestricted)
                {
                    //func.wh
                    foreach (var tc in typeclasses)
                    {
                        var instances = context.typeclassInstances.Where(ti =>
                            (ti.ImplementingInterfaces[0] as common_generic_instance_type_node).original_generic ==
                            (tc.ImplementingInterfaces[0] as common_generic_instance_type_node).original_generic);
                        
                        var appropriateInstances = instances.Where(ti =>
                            (ti.ImplementingInterfaces[0] as common_generic_instance_type_node).instance_params.SequenceEqual(
                                (tc.ImplementingInterfaces[0] as common_generic_instance_type_node).instance_params.Select(ip => deduced[ip.generic_param_index])));

                        if (appropriateInstances.Count() == 1)
                        {
                            var foundInstance = appropriateInstances.First() as common_type_node;
                            if (foundInstance.generic_params.Count > 0)
                            {
                                throw new NotImplementedException("Can't deduce type for typeclass inheritance");
                            }
                            deduced[tc.generic_param_index] = appropriateInstances.First();
                        }
                    }
                    //context.typeclassInstances.Where(ti => ti.ImplementingInterfaces[0].)
                }

                var current_deduce_state = deduced               //текущее состояние выведенных типов
                    .Select((t, ii) => new {Type = t, Index = ii})
                    .Where(t => t.Type != null)
                    .Select(t => t.Index)
                    .ToArray();

                if (previous_deduce_state.SequenceEqual(current_deduce_state)) // Если ничего с прошлой итерации не изменилось, то дальше нет смысла пробовать выводить. Выходим из цикла
                {
                    continue_trying_to_infer_types = false;
                }
            }

            RestoreLambdasStates(lambda_syntax_nodes.Values.ToList(), saved_lambdas_states);

            if (need_params_work)
            {
                type_node[] tmp_deduced = (type_node[])deduced.Clone();
                List<int> tmp_nils = new List<int>();
                tmp_nils.AddRange(nils);
                if (!DeduceInstanceTypes(formal[count_params_to_see].type, fact[count_params_to_see].type, deduced, nils))
                {
                    //Второй шанс. Учитываем слово params.
                    deduced = tmp_deduced;
                    nils = tmp_nils;
                    if (!DeduceInstanceTypes(formal[count_params_to_see].type.element_type, fact[count_params_to_see].type, deduced, nils))
                    {
                        if (alone)
                            throw new SimpleSemanticError(loc, "GENERIC_FUNCTION_{0}_CAN_NOT_BE_CALLED_WITH_THESE_PARAMETERS", func.name);
                        return null;
                    }
                }
            }
            //Вывели всё, что могли. Теперь проверяем.
            for (int i = 0; i < generic_type_params_count; ++i)
            {
                if (deduced[i] == null)
                {
                    if (alone)
                        throw new SimpleSemanticError(loc, "CAN_NOT_DEDUCE_TYPE_PARAMS_FROM_CALL_{0}", func.name);
                    return null;
                }
            }
            foreach (int num in nils)
            {
                if (!type_table.is_with_nil_allowed(deduced[num]))
                {
                    if (alone)
                        throw new SimpleSemanticError(loc, "GENERIC_FUNCTION_{0}_CAN_NOT_BE_CALLED_WITH_THESE_PARAMETERS", func.name);
                    return null;
                }
            }
            foreach (type_node tt in deduced)
            {
                CompilationErrorWithLocation check_err = generic_parameter_eliminations.check_type_generic_useful(tt, loc);
                if (check_err != null)
                {
                    if (alone)
                        throw check_err;
                    return null;
                }
            }
            //Итак, вывели все параметры. Теперь инстанцируем.
            List<type_node> deduced_list = new List<type_node>(generic_type_params_count);
            deduced_list.AddRange(deduced);
            return func.get_instance(deduced_list, alone, loc);
        }

        //Выведение типов
        public static bool DeduceInstanceTypes(type_node formal_type, type_node fact_type, type_node[] deduced, List<int> nils)
        {
            if (fact_type == null)//issue #347
                return false;
            if (formal_type.generic_function_container == null && fact_type.generic_function_container != null)
            {
                //swap
                type_node tmp = formal_type;
                formal_type = fact_type;
                fact_type = tmp;
            }
            //Формальный тип - generic-параметр функции. Выводим.
            if (formal_type.generic_function_container != null)
            {
                int par_num = formal_type.generic_param_index;
                if (fact_type.semantic_node_type == semantic_node_type.null_type_node)
                {
                    nils.Add(par_num);
                    return true;
                }
                if (deduced[par_num] == null)
                {
                    //Этот тип-параметр ещё не был выведен.
                    if (fact_type is delegated_methods && (fact_type as delegated_methods).empty_param_method != null && (fact_type as delegated_methods).empty_param_method.ret_type != null)
                        fact_type = (fact_type as delegated_methods).empty_param_method.ret_type;
                    //if (fact_type is delegated_methods)
                    //    fact_type = visitor.CreateDelegate((fact_type as delegated_methods).proper_methods[0].simple_function_node);
                    deduced[par_num] = fact_type;
                    return true;
                }
                //Этот тип-параметр уже был выведен. Сравниваем с выведенным.
                if (!convertion_data_and_alghoritms.eq_type_nodes(fact_type, deduced[par_num], true))
                {
                    if (fact_type is delegated_methods && deduced[par_num].IsDelegate)
                    {
                        delegate_internal_interface d1 = deduced[par_num].get_internal_interface(internal_interface_kind.delegate_interface) as delegate_internal_interface;
                        return convertion_data_and_alghoritms.function_eq_params_and_result((fact_type as delegated_methods).proper_methods[0].simple_function_node, d1.invoke_method);
                    }
                    else if (fact_type is delegated_methods && deduced[par_num] is delegated_methods)
                    {
                        return convertion_data_and_alghoritms.function_eq_params_and_result((fact_type as delegated_methods).proper_methods[0].simple_function_node, (deduced[par_num] as delegated_methods).proper_methods[0].simple_function_node);
                    }
                    return false;
                }
                return true;
            }
            //Указатели
            ref_type_node formal_ref = formal_type as ref_type_node;
            if (formal_ref != null)
            {
                ref_type_node fact_ref = fact_type as ref_type_node;
                if (fact_ref == null)
                {
                    goto eq_cmp;
                }
                return DeduceInstanceTypes(formal_ref.pointed_type, fact_ref.pointed_type, deduced, nils);
            }
            //безразмерные массивы
            array_internal_interface formal_ii = formal_type.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
            if (formal_ii != null)
            {
                array_internal_interface fact_ii = fact_type.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
                if (fact_ii == null)
                {
                    goto eq_cmp;
                }
                return DeduceInstanceTypes(formal_ii.element_type, fact_ii.element_type, deduced, nils);
            }
            //множества
            if (formal_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.set_type)
            {
                if (fact_type.type_special_kind != PascalABCCompiler.SemanticTree.type_special_kind.set_type)
                {
                    goto eq_cmp;
                }
                return DeduceInstanceTypes(formal_type.element_type, fact_type.element_type, deduced, nils);
            }
            //множества
            if (formal_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.typed_file)
            {
                if (fact_type.type_special_kind != PascalABCCompiler.SemanticTree.type_special_kind.typed_file)
                {
                    goto eq_cmp;
                }
                return DeduceInstanceTypes(formal_type.element_type, fact_type.element_type, deduced, nils);
            }
            //Делегаты
            if (formal_type.IsDelegate)
            {
                //Если текущий параметр - лямбда, то просто выводим дженерик-параметры из типов, которые уже известны. Не трогаем lambda_any_type_node. Остальное выведется в цикле выше 
                var lambda_func = fact_type as delegated_methods;
                if (lambda_func != null 
                    && lambda_func.proper_methods.Count == 1 
                    && LambdaHelper.IsLambdaName(lambda_func.proper_methods[0].simple_function_node.name))
                {
                    var dii = formal_type.get_internal_interface(internal_interface_kind.delegate_interface) as delegate_internal_interface;
                    var fact_func = lambda_func.proper_methods[0].simple_function_node;

                    if (fact_func.parameters.Count != dii.parameters.Count)
                    {
                        goto eq_cmp;
                    }

                    var param_count = fact_func.parameters.Count;

                    for (var i = 0; i < param_count; i++)
                    {
                        if (fact_func.parameters[i].parameter_type != dii.parameters[i].parameter_type ||
                            fact_func.parameters[i].is_params != dii.parameters[i].is_params)
                            goto eq_cmp;
                    }
                    for (var i = 0; i < param_count; i++)
                    {
                        if (fact_func.parameters[i].type is lambda_any_type_node)
                        {
                            continue;
                        }
                        // 07.04.15 - SSM поменял местами первые 2 параметра - видимо, была ошибка
                        if (!DeduceInstanceTypes(dii.parameters[i].type, fact_func.parameters[i].type, deduced, nils))
                        //if (!DeduceInstanceTypes(fact_func.parameters[i].type, dii.parameters[i].type, deduced, nils))
                        {
                            goto eq_cmp;
                        }
                    }
                    if (fact_func.return_value_type == null && dii.return_value_type == null)
                    {
                        //ok
                    }
                    else if (fact_func.return_value_type is lambda_any_type_node) //lroman//
                    {
                        if (dii.return_value_type == null)
                        {
                            goto eq_cmp;
                        }
                        return true;
                    }
                    // 07.04.15 - SSM поменял местами первые 2 параметра - видимо, была ошибка
                    else if (fact_func.return_value_type == null || !DeduceInstanceTypes(dii.return_value_type, fact_func.return_value_type, deduced, nils)) // SSM 29.05.14 - не выводится если IEnumerable<TResult>
//                    else if (fact_func.return_value_type == null || !DeduceInstanceTypes(fact_func.return_value_type, dii.return_value_type, deduced, nils)) // SSM 29.05.14 - не выводится если IEnumerable<TResult>
                    {
                        goto eq_cmp;
                    }
                    return true;
                }

                if (!convertion_data_and_alghoritms.eq_type_nodes(formal_type, fact_type, true))
                {
                    delegate_internal_interface dii = formal_type.get_internal_interface(internal_interface_kind.delegate_interface) as delegate_internal_interface;
                    delegated_methods dm = fact_type as delegated_methods;
                    if (dm != null && dm.proper_methods.Count == 1)
                    {
                        function_node fact_func = dm.proper_methods[0].simple_function_node;
                        if (fact_func.parameters.Count != dii.parameters.Count)
                        {
                            goto eq_cmp;
                        }
                        int param_count = fact_func.parameters.Count;
                        for (int i = 0; i < param_count; i++)
                        {
                            if (fact_func.parameters[i].parameter_type != dii.parameters[i].parameter_type ||
                                fact_func.parameters[i].is_params != dii.parameters[i].is_params)
                                goto eq_cmp;
                        }
                        for (int i = 0; i < param_count; i++)
                        {
                            if (!DeduceInstanceTypes(dii.parameters[i].type, fact_func.parameters[i].type, deduced, nils))      // 07.04.15 - SSM поменял местами первые 2 параметра - видимо, была ошибка
                            //if (!DeduceInstanceTypes(fact_func.parameters[i].type, dii.parameters[i].type, deduced, nils))
                            {
                                goto eq_cmp;
                            }
                        }
                        if (fact_func.return_value_type == null && dii.return_value_type == null)
                        {
                            //ok
                        }
                        else if (fact_func.return_value_type is lambda_any_type_node && dii.return_value_type == null) //lroman//
                        {
                            goto eq_cmp;
                        }
                        // 07.04.15 - SSM поменял местами первые 2 параметра - видимо, была ошибка
                        else if (fact_func.return_value_type == null || !DeduceInstanceTypes(dii.return_value_type, fact_func.return_value_type, deduced, nils)) // SSM 29.05.14 - не выводится если IEnumerable<TResult>
//                        else if (fact_func.return_value_type == null || !DeduceInstanceTypes(fact_func.return_value_type, dii.return_value_type, deduced, nils)) // SSM 29.05.14 - не выводится если IEnumerable<TResult>
                        {
                            goto eq_cmp;
                        }
                        return true;
                    }
                }
                if (fact_type.IsDelegate && fact_type.semantic_node_type == semantic_node_type.delegated_method)
                    return true;
                //goto eq_cmp;
            }
            //Инстанции дженериков
            if (formal_type.is_generic_type_instance)
            {
                //if () goto eq_cmp;
                type_node fact_type_converted; //Сюда будет записан фактический тип или интерфейс, к которому он приводится
                if (!fact_type.is_generic_type_instance || formal_type.original_generic != fact_type.original_generic)
                {
                    //if (!formal_type.IsInterface) goto eq_cmp;
                    fact_type_converted = null;
                    type_node base_type = fact_type.base_type;
                    while (base_type != null)
                    {
                        if (base_type.original_generic == formal_type.original_generic)
                        {
                            fact_type_converted = base_type;
                            break;
                        }
                        base_type = base_type.base_type;
                    }
                    if (fact_type.ImplementingInterfaces != null && fact_type_converted == null)
                    foreach (type_node ti in fact_type.ImplementingInterfaces)
                    {
                        if (ti.original_generic == formal_type.original_generic)
                        {
                            fact_type_converted = ti;
                            break;
                        }
                    }
                    if (fact_type_converted == null) goto eq_cmp;
                }
                else
                {
                    fact_type_converted = fact_type;
                }
                int pcount = formal_type.instance_params.Count;
                for (int k = 0; k < pcount; ++k)
                {
                    if (!DeduceInstanceTypes(formal_type.instance_params[k], fact_type_converted.instance_params[k], deduced, nils))
                    {
                        goto eq_cmp;
                    }
                }
                return true;
            }
            //Если совпадают - всё хорошо.
            eq_cmp:
            if (convertion_data_and_alghoritms.eq_type_nodes(formal_type, fact_type, true))
            {
                return true;
            }
            possible_type_convertions ptc = type_table.get_convertions(fact_type, formal_type);
            if (ptc.first != null)
            {
                return true;
            }
            return false;
        }

        public static string MakePseudoInstanceName(string name, List<type_node> param_types, bool type_name)
        {
            string rez;
            if (type_name)
            {
                int last = name.LastIndexOf(compiler_string_consts.generic_params_infix);
                if (last < 0)
                {
                    rez = name;
                }
                else
                {
                    rez = name.Substring(0, last);
                }
            }
            else
            {
                rez = name;
            }
            bool first = true;
            foreach (type_node tnode in param_types)
            {
                rez += ((first) ? "<" : ",") + tnode.PrintableName;
                first = false;
            }
            rez += ">";
            return rez;
        }

        public static bool type_has_default_ctor(type_node tn, bool find_protected_ctors)
        {
            List<SymbolInfo> sil = tn.find_in_type(compiler_string_consts.default_constructor_name, tn.Scope);
            if (sil != null)
            {
                foreach (SymbolInfo si in sil)
                {
                    function_node fn = si.sym_info as function_node;
                    if (find_protected_ctors ||
                        fn.field_access_level == PascalABCCompiler.SemanticTree.field_access_level.fal_public)
                    {
                        compiled_constructor_node pconstr = fn as compiled_constructor_node;
                        common_method_node mconstr = fn as common_method_node;
                        if ((pconstr != null ||
                            mconstr != null && mconstr.is_constructor) &&
                            (fn.parameters.Count == 0 || fn.parameters[0].default_value != null)
                            )
                        {
                            //Нашли конструктор по умолчанию у предка
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static function_node get_function_instance(function_node orig, List<type_node> param_types)
        {
            List<generic_function_instance_info> _generic_instances = get_function_instances(orig);
            function_node founded_inst = find_function_instance(_generic_instances, param_types);
            if (founded_inst != null) return founded_inst;
            for (int i=0; i<param_types.Count; i++)
            {
                if (param_types[i] is delegated_methods)
                {
                    base_function_call bfc = (param_types[i] as delegated_methods).proper_methods[0];
                    var context = SystemLibrary.SystemLibrary.syn_visitor.context;
                    common_type_node del =
                        type_constructor.instance.create_delegate(context.get_delegate_type_name(), bfc.simple_function_node.return_value_type, bfc.simple_function_node.parameters, context.converted_namespace, null);
                    context.converted_namespace.types.AddElement(del);
                    param_types[i] = del;
                }
            }
            //Создаём новую псевдо-инстанцию
            common_function_node new_func = null;
            SemanticTree.IGenericFunctionInstance new_inst = null;
            if (orig.semantic_node_type == semantic_node_type.common_namespace_function_node)
            {
                generic_namespace_function_instance_node nnode = new generic_namespace_function_instance_node(orig as common_namespace_function_node, param_types);
                nnode.ConnectedToType = (orig as common_namespace_function_node).ConnectedToType;
                new_func = nnode;
                new_inst = nnode;
            }
            else
            {
                generic_method_instance_node ctnode = new generic_method_instance_node(orig, param_types);
                new_func = ctnode;
                new_inst = ctnode;
            }
            common_function_node common_orig = orig as common_function_node;
            if (common_orig != null)
            {
                new_func.num_of_default_variables = common_orig.num_of_default_variables;
            }

            var shouldAddToAllTypeInstances = true;
            if (LambdaHelper.processingLambdaParametersForTypeInference != 0)
            {
                foreach (var par in param_types)
                {
                    if (par is lambda_any_type_node)
                    {
                        shouldAddToAllTypeInstances = false;
                        break;
                    }
                }
            }

            if (shouldAddToAllTypeInstances) //lroman// Если зашли сюда при выведении типов параметров лямбды, то функция инстанцироваться может с типом lambda_any_type_node. Поэтому, если выводим типы. То данную инстанцию не добавляем 
            {
                _generic_instances.Add(new generic_function_instance_info(param_types, new_func));
                all_function_instances.Add(new_inst);
            }
            return new_func;
        }

    }

    public class generic_instance_type_node : common_type_node, SemanticTree.IGenericTypeInstance
    {
        protected Hashtable _members = new Hashtable();
        protected Hashtable _member_definitions = new Hashtable();

        protected type_node _original_generic;

        public override SymbolTable.Scope Scope
        {
            get
            {
                return _original_generic.Scope;
            }
        }

        SemanticTree.ITypeNode SemanticTree.IGenericTypeInstance.original_generic
        {
            get { return _original_generic; }
        }

        public override type_node original_generic
        {
            get { return _original_generic; }
        }

        public Hashtable used_members
        {
            get
            {
                return _members;
            }
        }

        protected List<SemanticTree.ITypeNode> _generic_parameters = null;
        protected List<type_node> _instance_params;

        public generic_instance_type_node(type_node original_generic_type,
            List<type_node> param_types,
            type_node base_type, string name, SemanticTree.type_access_level type_access_level,
            common_namespace_node comprehensive_namespace, location loc)
            :
            base(base_type, name, type_access_level, comprehensive_namespace,
            null, loc)
        {
            _original_generic = original_generic_type;
            _instance_params = param_types;
        }

        public List<SemanticTree.ITypeNode> generic_parameters
        {
            get
            {
                if (_generic_parameters == null)
                {
                    _generic_parameters = new List<SemanticTree.ITypeNode>(_instance_params.Count);
                    foreach (type_node tn in _instance_params)
                    {
                        _generic_parameters.Add(tn);
                    }
                }
                return _generic_parameters;
            }
        }

        private List<SymbolInfo> temp_names = new List<SymbolInfo>(3);

        public override void add_name(string name, SymbolInfo si)
        {
            temp_names.Add(si);
        }

        protected void AddMember(object original, object converted)
        {
            _members.Add(original, converted);
            _member_definitions.Add(converted, original);
        }

        protected parameter_list make_parameters(parameter_list orig_pl, common_function_node fn)
        {
            parameter_list pl = new parameter_list();
            //TODO: разобраться с concrete_parameter_type
            foreach (parameter p in orig_pl)
            {
                common_parameter cp = new common_parameter(
                    p.name, generic_convertions.determine_type(p.type, _instance_params, false),
                    p.parameter_type,
                    fn, concrete_parameter_type.cpt_none, p.inital_value, null);
                cp.intrenal_is_params = p.is_params;
                cp.is_special_name = p.is_special_name;
                cp.is_ret_value = p.is_ret_value;
                cp.default_value = p.default_value;
                pl.AddElement(cp);
            }
            return pl;
        }

        //(ssyy) Создаёт метод псевдо-инстанции generic-типа.
        protected common_method_node make_method(function_node orig_fn, location loc)
        {
            if (orig_fn == null)
            {
                return null;
            }
            List<type_node> meth_inst_pars = null;
            SemanticTree.IClassMemberNode orig_member = orig_fn as SemanticTree.IClassMemberNode;
            common_method_node cmn = new common_method_node(
                orig_fn.name,
                //generic_convertions.determine_type(orig_fn.return_value_type, _instance_params, false),
                null,
                loc, this, orig_member.polymorphic_state, orig_member.field_access_level,
                null);
            if (orig_fn.is_generic_function)
            {
                List<type_node> orig_tpars = orig_fn.get_generic_params_list();
                int type_count = orig_tpars.Count;
                cmn.generic_params = new List<PascalABCCompiler.SemanticTree.ICommonTypeNode>(orig_tpars.Count);
                foreach (type_node t in orig_tpars)
                {
                    common_type_node par = new common_type_node(t.name, PascalABCCompiler.SemanticTree.type_access_level.tal_public,
                        null, SystemLibrary.SystemLibrary.syn_visitor.convertion_data_and_alghoritms.symbol_table.CreateInterfaceScope(null, SystemLibrary.SystemLibrary.object_type.Scope, null), null);
                    SystemLibrary.SystemLibrary.init_reference_type(par);
                    par.SetBaseType(SystemLibrary.SystemLibrary.object_type);
                    cmn.generic_params.Add(par);
                    par.generic_function_container = cmn;
                }
                meth_inst_pars = cmn.get_generic_params_list();
                List<generic_parameter_eliminations> gpes = orig_fn.parameters_eliminations;
                for (int i = 0; i < type_count; ++i)
                {
                    common_type_node p = (common_type_node)(meth_inst_pars[i]);
                    generic_parameter_eliminations gpe = gpes[i];
                    p.SetBaseType(generic_convertions.determine_type(
                        generic_convertions.determine_type(gpe.base_class, _instance_params, false),
                        meth_inst_pars, true));
                    p.is_class = gpe.is_class;
                    p.internal_is_value = gpe.is_value;
                    foreach (type_node interf in gpe.implementing_interfaces)
                    {
                        type_table.AddInterface(p, generic_convertions.determine_type(
                            generic_convertions.determine_type(interf, _instance_params, false),
                            meth_inst_pars, true), null);
                    }
                    if (gpe.has_default_ctor)
                    {
                        generic_parameter_eliminations.add_default_ctor(p);
                    }
                }
            }
            cmn.parameters.AddRange(make_parameters(orig_fn.parameters, cmn));
            if (orig_fn.is_generic_function)
            {
                foreach (common_parameter cp in cmn.parameters)
                {
                    cp.type = generic_convertions.determine_type(cp.type, meth_inst_pars, true);
                }
            }
            common_method_node common_orig = orig_fn as common_method_node;
            if (common_orig != null)
            {
                cmn.num_of_default_variables = common_orig.num_of_default_variables;
            }
            compiled_constructor_node compiled_orig = orig_fn as compiled_constructor_node;
            cmn.is_constructor = (compiled_orig != null ||
                (common_orig != null && common_orig.is_constructor));
            cmn.return_value_type = generic_convertions.determine_type(orig_fn.return_value_type, _instance_params, false);
            if (orig_fn.is_generic_function)
            {
                cmn.return_value_type = generic_convertions.determine_type(cmn.return_value_type, meth_inst_pars, true);
            }
            if (orig_fn is common_function_node)
            {
                cmn.return_variable = (orig_fn as common_function_node)?.return_variable;
            }
            return cmn;
        }

        public override List<type_node> instance_params
        {
            get
            {
                return _instance_params;
            }
            set
            {
                _instance_params = value;
            }
        }

        public override bool is_generic_type_instance
        {
            get
            {
                return true;
            }
        }

        protected void AddPropertyAccessors(property_node pn)
        {
            if (pn.get_function != null)
            {
                if (_members[pn.get_function] == null)
                {
                    ConvertMember(pn.get_function);
                }
            }
            if (pn.set_function != null)
            {
                if (_members[pn.set_function] == null)
                {
                    ConvertMember(pn.set_function);
                }
            }
        }

        protected common_event make_event(event_node orig_event, location loc)
        {
            common_event cme = orig_event as common_event;
            compiled_event ce = orig_event as compiled_event;
            if (_members[orig_event.add_method] == null)
                ConvertMember(orig_event.add_method);
            if (_members[orig_event.remove_method] == null)
                ConvertMember(orig_event.remove_method);
            if (orig_event.raise_method != null && _members[orig_event.raise_method] == null)
                ConvertMember(orig_event.raise_method);
            common_event evnt = new common_event(orig_event.name, generic_convertions.determine_type(
                orig_event.delegate_type, _instance_params, false), this,
                _members[orig_event.add_method] as common_method_node,
                _members[orig_event.remove_method] as common_method_node,
                orig_event.raise_method != null ? _members[orig_event.raise_method] as common_method_node : null,
                cme != null ? cme.field_access_level : SemanticTree.field_access_level.fal_public,
                orig_event.is_static ? SemanticTree.polymorphic_state.ps_static : SemanticTree.polymorphic_state.ps_common,
                loc
                );
            return evnt;
        }

        protected common_property_node make_property(property_node orig_pn, location loc)
        {
            AddPropertyAccessors(orig_pn);
            common_property_node cpn = new common_property_node(
                orig_pn.name, this, generic_convertions.determine_type(
                orig_pn.property_type, _instance_params, false),
                (orig_pn.get_function == null) ? null : _members[orig_pn.get_function] as common_method_node,
                (orig_pn.set_function == null) ? null : _members[orig_pn.set_function] as common_method_node,
                loc, orig_pn.field_access_level, orig_pn.polymorphic_state);
            cpn.parameters.AddRange(make_parameters(orig_pn.parameters, null));
            return cpn;
        }

        public definition_node get_member_definition(definition_node instance_member)
        {
            return _member_definitions[instance_member] as definition_node;
        }

        public generic_instance_type_node find_instance_type_from(type_node orig_type)
        {
            generic_instance_type_node bt = null;//this;
            /*while (bt != null)
            {
                if (bt._original_generic == orig_type)
                {
                    return bt;
                }
                bt = bt.base_type as generic_instance_type_node;
            }*/
            type_node curt = this;
            while (curt != null)
            {
                bt = curt as generic_instance_type_node;
                if (bt != null && bt.original_generic == orig_type)
                {
                    return bt;
                }
                curt = curt.base_type;
            }
            foreach (type_node tn in ImplementingInterfaces)
            {
                bt = tn as generic_instance_type_node;
                if (bt != null && bt._original_generic == orig_type)
                {
                    return bt;
                }
            }
            return null;
        }

        public definition_node ConvertMember(definition_node orig_node)
        {
            definition_node rez_node = _members[orig_node] as definition_node;
            if (rez_node == null)
            {
                //Преобразуем найденный член класса.
                SemanticTree.IClassMemberNode orig_member = orig_node as SemanticTree.IClassMemberNode;
                if (orig_member == null)
                {
                    //Для basic_function
                    generic_instance_type_node gitn = base_type as generic_instance_type_node;
                    if (gitn == null)
                        return orig_node;
                    else
                        return gitn.ConvertMember(orig_node);
                }
                type_node tn = orig_member.comperehensive_type as type_node;
                if (orig_member.comperehensive_type.is_generic_type_instance)
                    tn = tn.original_generic;
                if (tn != _original_generic)
                {
                    if (orig_member.comperehensive_type.is_generic_type_definition)
                    {
                        generic_instance_type_node compr_type = find_instance_type_from(orig_member.comperehensive_type as type_node);
                        if (compr_type == null)
                        {
                            compiled_function_node cfn = orig_node as compiled_function_node;
                            if (cfn == null)
                            {
                                return orig_node;
                            }
                            compiled_type_node cct = orig_member.comperehensive_type as compiled_type_node;
                            type_node inst_type = this;
                            do
                            {
                                inst_type = inst_type.base_type;
                            }
                            while (inst_type.semantic_node_type != semantic_node_type.compiled_type_node ||
                                (inst_type != cct && inst_type.original_generic != cct));


                            /*MethodInfo[] meths = cct._compiled_type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                BindingFlags.Static | BindingFlags.Instance);
                            int num = System.Array.IndexOf(meths, cfn.method_info);

                            //!!! прикольно, но индексы в meths и instmeths не совпадают!!!

                            MethodInfo[] instmeths = ((compiled_type_node)inst_type)._compiled_type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                BindingFlags.Static | BindingFlags.Instance);

                            //var mt1 = meths.Select(m => m.MetadataToken).OrderBy(x => x);
                            //var mt2 = instmeths.Select(m => m.MetadataToken).OrderBy(x => x);


                            MethodInfo mi = instmeths[num];*/

                            // SSM 2018.05.05 bug fix #664 
                            var mdtok = cfn.method_info.MetadataToken;
                            MethodInfo[] instmeths = ((compiled_type_node)inst_type)._compiled_type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                BindingFlags.Static | BindingFlags.Instance);

                            MethodInfo mi = System.Array.Find(instmeths, m => m.MetadataToken == mdtok);
                            // SSM 2018.05.05 end bug fix #664 

                            return compiled_function_node.get_compiled_method(mi);
                        }
                        else
                            return compr_type.ConvertMember(orig_node);
                    }
                    else if (orig_member.comperehensive_type.is_generic_type_instance)
                    {
                        /*
                        // SSM 29/04/18 - проба - как достать IndexOf из MyList<integer> - наследника List<integer> 
                        // Это поздно - надо как-то раньше
                          if (tn is compiled_type_node)
                          tn = generic_convertions.determine_type((tn as compiled_type_node).compiled_type, instance_params,false);

                        var tn1 = tn as compiled_type_node;
                        var ff = tn1.find_in_type("IndexOf");*/

                        generic_instance_type_node compr_type = find_instance_type_from(tn);
                        if (compr_type == null)
                        {
                            compiled_function_node cfn = orig_node as compiled_function_node;
                            if (cfn == null)
                            {
                                return orig_node;
                            }
                            compiled_type_node cct = tn as compiled_type_node;
                            type_node inst_type = this;
                            do
                            {
                                inst_type = inst_type.base_type;
                            }
                            while (inst_type.semantic_node_type != semantic_node_type.compiled_type_node ||
                                (inst_type != cct && inst_type.original_generic != cct));


                            MethodInfo[] meths = cct._compiled_type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                BindingFlags.Static | BindingFlags.Instance);
                            int num = System.Array.IndexOf(meths, cfn.method_info);

                            MethodInfo mi = ((compiled_type_node)inst_type)._compiled_type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                BindingFlags.Static | BindingFlags.Instance)[num];
                            return compiled_function_node.get_compiled_method(mi);
                        }
                        else
                            return compr_type.ConvertMember(orig_node);
                        //
                    }
                    else
                        return orig_node;
                }
                SemanticTree.ILocated orig_loc = orig_node as SemanticTree.ILocated;
                location loc = (orig_loc == null) ? null : (orig_loc.Location as location);
                switch (orig_node.general_node_type)
                {
                    case general_node_type.constant_definition:
                        constant_definition_node orig_cdn = orig_node as constant_definition_node;
                        rez_node = new class_constant_definition(
                            orig_cdn.name, orig_cdn.const_value,
                            loc, this, orig_member.field_access_level);
                        break;
                    case general_node_type.variable_node:
                        var_definition_node orig_cf = (var_definition_node)(orig_node);
                        rez_node = new class_field(orig_cf.name,
                            generic_convertions.determine_type(orig_cf.type, _instance_params, false),
                            this, orig_member.polymorphic_state,
                            orig_member.field_access_level, loc);
                        break;
                    case general_node_type.property_node:
                        property_node orig_pn = (property_node)(orig_node);
                        rez_node = make_property(orig_pn, loc);
                        break;
                    case general_node_type.function_node:
                        function_node orig_fn = (function_node)(orig_node);
                        rez_node = make_method(orig_fn, loc);
                        break;
                    case general_node_type.event_node:
                        //(ssyy) Не знаю, что тут делать
                        event_node orig_event = (event_node)orig_node;
                        rez_node = make_event(orig_event, loc);
                        break;
                    default:
                        throw new CompilerInternalError("Unexpected definition_node.");
                }
                AddMember(orig_node, rez_node);
            }
            return rez_node;
        }

        public List<SymbolInfo> ConvertSymbolInfo(List<SymbolInfo> start)
        {
            List<SymbolInfo> rez_start = null;
            SymbolInfo rez_si = null;
            if (start != null)
            {
                foreach (SymbolInfo si in start)
                {
                    definition_node dnode = ConvertMember(si.sym_info);
                    rez_si = new SymbolInfo(dnode, si.access_level, si.symbol_kind);
                    //Дополняем список SymbolInfo преобразованным значением
                    if (rez_start == null)
                    {
                        rez_start = new List<SymbolInfo>();
                        rez_start.Add(rez_si);
                    }
                    else
                    {
                        rez_start.Add(rez_si);
                    }
                }
            }
            return rez_start;
        }

        public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
        {
            List<SymbolInfo> si = _original_generic.find(name);
            return ConvertSymbolInfo(si);//delete
        }

        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            List<SymbolInfo> sil = _original_generic.find_in_type(name);
            sil = ConvertSymbolInfo(sil);
            return sil;
        }

        public override List<SymbolInfo> find_in_type(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            //var or = generic_convertions.determine_type(_original_generic,this.instance_params,false); // циклится
            List<SymbolInfo> sil = null;
            /*var ctn = base_type as compiled_type_node;
            if (ctn != null && ctn.is_generic_type_instance)
            {
                sil = ctn.find_in_type(name, CurrentScope, no_search_in_extension_methods);
                var sil1 = _original_generic.find_in_type(name, CurrentScope);
                sil1 = ConvertSymbolInfo(sil1);
                if (sil!=null)
                    sil1.InsertRange(0,sil);
                return sil1;
            }*/
            sil = _original_generic.find_in_type(name, CurrentScope);
            sil = ConvertSymbolInfo(sil);
            return sil;
        }

        private void conform_basic_function(string name, int base_func_num)
        {
            SymbolInfo si1 = _original_generic.find_first_in_type(name, true);
            AddMember(si1.sym_info, temp_names[base_func_num].sym_info);
        }

        public void conform_basic_functions()
        {
            conform_basic_function(compiler_string_consts.assign_name, 0);
            conform_basic_function(compiler_string_consts.eq_name, 1);
            conform_basic_function(compiler_string_consts.noteq_name, 2);
            temp_names = null;
        }

        public virtual List<function_node> all_methods
        {
            get
            {
                return null;
            }
        }

        public virtual List<type_node> all_field_types
        {
            get
            {
                return null;
            }
        }

        public override generic_instance_type_node base_generic_instance
        {
            get
            {
                return this;
            }
        }

        public override bool depended_from_indefinite
        {
            get
            {
                if (_instance_params == null)
                {
                    return false;
                }
                foreach (type_node tn in _instance_params)
                {
                    if (tn.depended_from_indefinite)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

    }

    //Класс, характеризующий одну псевдоинстанцию generic-типа
    public class compiled_generic_instance_type_node : generic_instance_type_node, SemanticTree.ICompiledGenericTypeInstance
    {
        public compiled_type_node compiled_original_generic
        {
            get
            {
                return _original_generic as compiled_type_node;
            }
        }

        public override string BaseFullName
        {
            get
            {
                return compiled_original_generic.BaseFullName;
            }
        }

        public compiled_generic_instance_type_node(compiled_type_node generic_definition,
            List<type_node> param_types,
            type_node base_type, string name,
            SemanticTree.type_access_level type_access_level,
            common_namespace_node comprehensive_namespace,
            location loc)
            :
            base(generic_definition, param_types, base_type, name, type_access_level, comprehensive_namespace,
            loc)
        {
        }

        private List<function_node> _all_methods = null;

        public override List<function_node> all_methods
        {
            get
            {
                if (_all_methods != null) return _all_methods;
                _all_methods = new List<function_node>();
                System.Reflection.MemberInfo[] orig_members = compiled_original_generic.compiled_type.GetMembers();
                foreach (System.Reflection.MemberInfo mi in orig_members)
                {
                    if (mi.MemberType == System.Reflection.MemberTypes.Method)
                    {
                        compiled_function_node cmeth = compiled_function_node.get_compiled_method((System.Reflection.MethodInfo)mi);
                        _all_methods.Add(ConvertMember(cmeth) as function_node);
                    }
                }
                return _all_methods;
            }
        }

        private List<type_node> _all_field_types = null;
        public override List<type_node> all_field_types
        {
            get
            {
                if (_all_field_types != null) return _all_field_types;
                System.Reflection.FieldInfo[] fields = compiled_original_generic.compiled_type.GetFields();
                _all_field_types = new List<type_node>();
                foreach (System.Reflection.FieldInfo fi in fields)
                {
                    if (!fi.IsStatic)
                    {
                        _all_field_types.Add(generic_convertions.determine_type(compiled_type_node.get_type_node(fi.FieldType), instance_params, false));
                    }
                }
                return _all_field_types;
            }
        }
    }

    //Класс, характеризующий одну псевдоинстанцию generic-типа
    public class common_generic_instance_type_node : generic_instance_type_node, SemanticTree.ICommonGenericTypeInstance
    {
        public common_type_node common_original_generic
        {
            get
            {
                return _original_generic as common_type_node;
            }
        }

        public common_generic_instance_type_node(common_type_node generic_definition,
            List<type_node> param_types,
            type_node base_type, string name,
            SemanticTree.type_access_level type_access_level,
            common_namespace_node comprehensive_namespace,
            location loc)
            :
            base(generic_definition, param_types, base_type, name, type_access_level, comprehensive_namespace,
            loc)
        {
        }

        private List<function_node> _all_methods = null;
        public override List<function_node> all_methods
        {
            get
            {
                if (_all_methods != null) return _all_methods;
                common_method_node_list orig_meths = common_original_generic.methods;
                _all_methods = new List<function_node>(orig_meths.Count);
                foreach (common_method_node cnode in orig_meths)
                {
                    _all_methods.Add(ConvertMember(cnode) as function_node);
                }
                return _all_methods;
            }
        }

        private List<type_node> _all_field_types = null;
        public override List<type_node> all_field_types
        {
            get
            {
                if (_all_field_types != null) return _all_field_types;
                class_field_list orig_fields = common_original_generic.fields;
                _all_field_types = new List<type_node>(orig_fields.Count);
                foreach (class_field cf in orig_fields)
                {
                    _all_field_types.Add(generic_convertions.determine_type(cf.type, this.instance_params, false));
                }
                return _all_field_types;
            }
        }
    }

    //Класс для запрещения создания в одной области видимости generic-классов с одинаковым именем.
    public class generic_indicator : definition_node, SemanticTree.ILocated
    {
        private common_type_node _generic;

        public common_type_node generic
        {
            get
            {
                return _generic;
            }
        }

        public generic_indicator(common_type_node generic_type)
        {
            _generic = generic_type;
        }

        public SemanticTree.ILocation Location
        {
            get
            {
                return _generic.Location;
            }
        }

        public location loc
        {
            get
            {
                return _generic.loc;
            }
        }

        public override general_node_type general_node_type
        {
            get
            {
                return TreeRealization.general_node_type.generic_indicator;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.generic_indicator;
            }
        }
    }

    public class GenericParameterAbilities
    {
        public bool useful_for_pointers = false;
        public bool useful_for_binary_files = false;
        public bool useful_for_typed_files = false;
    }

    public class generic_method_instance_node : common_method_node, SemanticTree.IGenericFunctionInstance
    {
        protected List<type_node> _instance_params;

        public List<type_node> instance_params
        {
            get
            {
                return _instance_params;
            }
        }

        protected List<SemanticTree.ITypeNode> _generic_parameters = null;

        public List<SemanticTree.ITypeNode> generic_parameters
        {
            get
            {
                if (_generic_parameters == null)
                {
                    _generic_parameters = new List<SemanticTree.ITypeNode>();
                    foreach (type_node t in _instance_params)
                    {
                        _generic_parameters.Add(t);
                    }
                }
                return _generic_parameters;
            }
        }

        protected function_node _original_function;

        SemanticTree.IFunctionNode SemanticTree.IGenericFunctionInstance.original_function
        {
            get
            {
                return _original_function;
            }
        }

        public override function_node original_function
        {
            get
            {
                return _original_function;
            }
        }

        public override bool is_generic_function_instance
        {
            get
            {
                return true;
            }
        }

        public override SemanticTree.ITypeNode comperehensive_type
        {
            get
            {
                return (_original_function as SemanticTree.IClassMemberNode).comperehensive_type;
            }
        }

        public generic_method_instance_node(function_node original_generic_function, List<type_node> instance_parameters)
            : base(
            generic_convertions.MakePseudoInstanceName(original_generic_function.name, instance_parameters, false),
            null, null)
        {
            _original_function = original_generic_function;
            _instance_params = instance_parameters;

            this.field_access_level = original_generic_function.field_access_level;
            this.is_final = original_generic_function.is_final;
            this.is_overload = true;
            this.polymorphic_state = original_generic_function.polymorphic_state;

            this.return_value_type = generic_convertions.determine_type(original_generic_function.return_value_type, instance_parameters, true);

            foreach (parameter par in original_generic_function.parameters)
            {
                common_parameter cpar = new common_parameter(par.name,
                    generic_convertions.determine_type(par.type, _instance_params, true),
                    par.parameter_type, this,
                    (par.parameter_type == SemanticTree.parameter_type.var) ? concrete_parameter_type.cpt_var : concrete_parameter_type.cpt_none,
                    par.default_value, null);
                cpar.inital_value = par.inital_value;
                cpar.default_value = par.default_value;
                cpar.intrenal_is_params = par.is_params;
                cpar.is_ret_value = par.is_ret_value;
                cpar.is_special_name = par.is_special_name;
                parameters.AddElement(cpar);
            }
        }

        public override List<type_node> get_generic_params_list()
        {
            return instance_params;
        }
    }

    public class generic_namespace_function_instance_node: common_namespace_function_node, SemanticTree.IGenericFunctionInstance
    {
        protected List<type_node> _instance_params;

        public List<type_node> instance_params
        {
            get
            {
                return _instance_params;
            }
        }

        protected List<SemanticTree.ITypeNode> _generic_parameters = null;

        public List<SemanticTree.ITypeNode> generic_parameters
        {
            get
            {
                if (_generic_parameters == null)
                {
                    _generic_parameters = new List<SemanticTree.ITypeNode>();
                    foreach (type_node t in _instance_params)
                    {
                        _generic_parameters.Add(t);
                    }
                }
                return _generic_parameters;
            }
        }

        protected common_namespace_function_node _original_function;

        SemanticTree.IFunctionNode SemanticTree.IGenericFunctionInstance.original_function
        {
            get
            {
                return _original_function;
            }
        }

        public override function_node original_function
        {
            get
            {
                return _original_function;
            }
        }

        public override bool is_generic_function_instance
        {
            get
            {
                return true;
            }
        }

        public generic_namespace_function_instance_node(common_namespace_function_node original_generic_function, List<type_node> instance_parameters)
            : base(
            generic_convertions.MakePseudoInstanceName(original_generic_function.name, instance_parameters, false),
            null, original_generic_function.namespace_node, null)
        {
            _original_function = original_generic_function;
            _instance_params = instance_parameters;

            this.field_access_level = original_generic_function.field_access_level;
            this.is_final = original_generic_function.is_final;
            this.is_overload = true;
            this.polymorphic_state = original_generic_function.polymorphic_state;

            this.return_value_type = generic_convertions.determine_type(original_generic_function.return_value_type, instance_parameters, true);

            foreach (parameter par in original_generic_function.parameters)
            {
                common_parameter cpar = new common_parameter(par.name,
                    generic_convertions.determine_type(par.type, _instance_params, true),
                    par.parameter_type, this,
                    (par.parameter_type == SemanticTree.parameter_type.var) ? concrete_parameter_type.cpt_var : concrete_parameter_type.cpt_none,
                    par.default_value, null);
                cpar.inital_value = par.inital_value;
                cpar.default_value = par.default_value;
                cpar.intrenal_is_params = par.is_params;
                cpar.is_ret_value = par.is_ret_value;
                cpar.is_special_name = par.is_special_name;
                parameters.AddElement(cpar);
            }
        }

        public override List<type_node> get_generic_params_list()
        {
            return instance_params;
        }
    }

    public class default_operator_node : expression_node, SemanticTree.IDefaultOperatorNode
    {
        public default_operator_node(type_node tn, location loc)
            : base(tn, loc)
        {
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.default_operator;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }

    }
}
