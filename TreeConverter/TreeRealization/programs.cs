// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

namespace PascalABCCompiler.TreeRealization
{
    /// <summary>
    /// Базовый класс для программ и dll.
    /// </summary>
    [Serializable]
    public abstract class program_base : definition_node, SemanticTree.IProgramBase
    {
        /// <summary>
        /// Расположение конструкции program имя_программы, или главного модуля dll.
        /// </summary>
        private location _loc;

        /// <summary>
        /// Список модулей, используемых в программе.
        /// </summary>
        private readonly common_unit_node_list _units = new common_unit_node_list();

        /// <summary>
        /// Список пространств имен в программе.
        /// </summary>
        private common_namespace_node[] _namespaces = null;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="loc">Расположение конструкции program имя_программы, или главного модуля dll.</param>
        public program_base(location loc)
        {
            _loc = loc;
        }

        /// <summary>
        /// Расположение конструкции program имя_программы, или главного модуля dll.
        /// </summary>
        public location loc
        {
            get
            {
                return _loc;
            }
        }

        /// <summary>
        /// Список модулей, используемых в программе или dll.
        /// </summary>
        public common_unit_node_list units
        {
            get
            {
                return _units;
            }
        }

        /// <summary>
        /// Обобщенный тип узла.
        /// </summary>
        public override general_node_type general_node_type
        {
            get
            {
                return general_node_type.program_node;
            }
        }

        //TODO: Исправить этот метод, так чтобы он работал только с корневыми или со всеми пространствами имен.
        /// <summary>
        /// Просматривает список модулей, и объединяет одни и те-же пространства имен разных модулей.
        /// </summary>
        /// <param name="units">Список модулей.</param>
        /// <returns>Список пространств имен.</returns>
        private static common_namespace_node[] get_units_namespaces(common_unit_node_list units)
        {
            /*System.Collections.Generic.Dictionary<string, common_namespace_node> namespaces =
                new System.Collections.Generic.Dictionary<string, common_namespace_node>(
                SystemLibrary.SystemLibrary.string_comparer);

            foreach (common_unit_node un in units)
            {
                foreach (common_namespace_node cnn in un.namespaces)
                {
                    string full_name = cnn.namespace_full_name;
                    common_namespace_node new_namespace;
                    if (!(namespaces.TryGetValue(full_name,out new_namespace)))
                    {
                        new_namespace = cnn.clone();
                        namespaces[full_name] = new_namespace;
                    }
                    else
                    {
                        new_namespace.merge_with_namespace(cnn);
                    }
                }
            }
            common_namespace_node[] ret = new common_namespace_node[namespaces.Count];
            namespaces.Values.CopyTo(ret, 0);
            return ret;*/
            System.Collections.Hashtable ht = new System.Collections.Hashtable();
            System.Collections.ArrayList al = new System.Collections.ArrayList();
            common_namespace_node main_cnn = null;
            foreach (common_unit_node un in units)
            {
            	foreach (common_namespace_node cnn in un.namespaces)
                {
                    //if (cnn.namespace_full_name != "")
                    //{
                        if (ht[cnn.namespace_name] == null)
                        {
                            al.Add(cnn); ht[cnn.namespace_name] = cnn;
                        }
                        //else
                        //    throw new Errors.CompilerInternalError("MergeNamespaces", new Errors.LocatedError("Dupblicate nammespace name: "+cnn.namespace_name));
                    //}
                    //else main_cnn = cnn;
                }
            }
            int num = 0; if (main_cnn != null) num++;
            common_namespace_node[] namespaces_to_ret = new common_namespace_node[al.Count + num];
            if (main_cnn != null) namespaces_to_ret[0] = main_cnn;
            al.CopyTo(namespaces_to_ret, num);
            return namespaces_to_ret;
            /*System.Collections.Hashtable ht = new System.Collections.Hashtable();
            System.Collections.ArrayList al = new System.Collections.ArrayList();
            foreach (common_unit_node un in units)
            {
                foreach (common_namespace_node cnn in un.namespaces)
                    if (ht[cnn.namespace_name] == null)
                    {
                        al.Add(cnn); 
                        ht[cnn.namespace_name] = cnn;
                    }
            }
            common_namespace_node[] namespaces_to_ret = new common_namespace_node[al.Count];
            al.CopyTo(namespaces_to_ret);
            return namespaces_to_ret;*/
        }
		
        private List<string> _used_namespaces = new List<string>();
        
        public string[] UsedNamespaces
        {
        	get
        	{
        		return _used_namespaces.ToArray();
        	}
        }
        
        /// <summary>
        /// Массив пространств имен, используемых в программе.
        /// </summary>
        public common_namespace_node[] common_namespaces
        {
            get
            {
                if (_namespaces == null)
                {
                    _namespaces = get_units_namespaces(_units);
                    System.Collections.Hashtable ns_ht = new System.Collections.Hashtable();
                    foreach (common_unit_node un in units)
            		{
            			foreach (string s in un.used_namespaces)
            			{
            				if (!ns_ht.ContainsKey(s))
            				{
            					this._used_namespaces.Add(s);
            					ns_ht.Add(s,s);
            				}
            			}
                    }
                }
                return _namespaces;
            }
        }

        /// <summary>
        /// Массив пространств имен, используемых в программе.
        /// Используется при обходе дерева посетителем.
        /// </summary>
        public SemanticTree.ICommonNamespaceNode[] namespaces
        {
            get
            {
                return common_namespaces;
            }
        }

        /// <summary>
        /// Расположение конструкции program имя_программы, или главного модуля dll.
        /// Используется при обходе дерева посетителем.
        /// </summary>
        public SemanticTree.ILocation Location
        {
            get
            {
                return _loc;
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

    /// <summary>
    /// Класс, представляющий всю программу.
    /// </summary>
	[Serializable]
	public class program_node : program_base, SemanticTree.IProgramNode
	{
        /// <summary>
        /// Главная процедура без параметров. Включает в себя вызовы процедур инициализации модулей (и финализации тоже).
        /// Она определена только здесь. Отсюда ее и надо компилировать в выходной код.
        /// </summary>
		private common_namespace_function_node _main_function;

        private bool _internal_dll = false;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="main_function">Главная функция программы.</param>
        /// <param name="loc">Расположение программы.</param>
		public program_node(common_namespace_function_node main_function,location loc) : base(loc)
		{
			_main_function=main_function;
		}

        public bool internal_dll
        {
            get
            {
                return _internal_dll;
            }
            set
            {
                _internal_dll = value;
            }
        }

        static HashSet<string> graph_modules = new HashSet<string>(new string[] { "GraphABC", "GraphWPF", "Graph3D", "PlotWPF", "Robot", "RobotField", "RobotZadan",
                                                                                   "RobotTaskMaker", "Drawman", "DrawManField", "ABCObjects", "ABCButtons", "ABCHouse", "ABCSprites",
                                                                                    "WPFObjects", "TurtleWPF", "Turtle", "Мозаика", "МозаикаABC", "FormsABC", "Чертежник", "Робот"});

        public void create_main_function(string[] used_stand_modules, Dictionary<string, object> config)
        {
        	add_needed_cctors();
        	common_namespace_function_node temp = _main_function;
            _main_function = new common_namespace_function_node("Main", null, null, (common_namespace_node)_main_function.comprehensive_namespace, null);
            location loc = temp.loc;
            statements_list sl = new statements_list(loc);
            _main_function.function_code = sl;
//            if (units[0].MainArgsParameter!=null)
//            {
//                _main_function.parameters.AddElement(units[0].MainArgsParameter);
//                sl.statements.AddElement(units[0].MainArgsAssignExpr);
//            }
//            if (units[0].IsConsoleApplicationVariableAssignExpr!=null)
//            {
//                sl.statements.AddElement(units[0].IsConsoleApplicationVariableAssignExpr);
//            }
            statements_list sl2 = sl;
            sl = new statements_list(null);
            common_namespace_function_node init_func = new common_namespace_function_node("$_Init_",null,null,(common_namespace_node)_main_function.comprehensive_namespace,null);
            ((common_namespace_node)_main_function.comprehensive_namespace).functions.AddElement(init_func);
            namespace_variable init_var = new namespace_variable("$is_init",SystemLibrary.SystemLibrary.bool_type,(common_namespace_node)_main_function.comprehensive_namespace,null);
            ((common_namespace_node)_main_function.comprehensive_namespace).variables.AddElement(init_var);
            if (SystemLibrary.SystemLibInitializer.ConfigVariable != null && SystemLibrary.SystemLibInitializer.ConfigVariable.Found)
            {
                namespace_variable conf_nv = null;
                compiled_variable_definition conf_cf = null;
                if (SystemLibrary.SystemLibInitializer.ConfigVariable.sym_info is namespace_variable)
                    conf_nv = SystemLibrary.SystemLibInitializer.ConfigVariable.sym_info as namespace_variable;
                else
                    conf_cf = SystemLibrary.SystemLibInitializer.ConfigVariable.sym_info as compiled_variable_definition;
                foreach (string config_var in config.Keys)
                {
                    var config_value = config[config_var];
                    compiled_function_call cfc = new compiled_function_call(compiled_function_node.get_compiled_method(NetHelper.NetHelper.AddToDictionaryMethod), 
                        (conf_nv != null) ? (expression_node)new namespace_variable_reference(conf_nv, null) : (expression_node)new static_compiled_variable_reference(conf_cf, null), null);
                    cfc.parameters.AddElement(new string_const_node(config_var, null));
                    switch (Type.GetTypeCode(config_value.GetType()))
                    {
                        case TypeCode.String:
                            cfc.parameters.AddElement(new string_const_node((string)config_value, null));
                            break;
                        case TypeCode.Int32:
                            cfc.parameters.AddElement(new int_const_node((int)config_value, null));
                            break;
                        case TypeCode.Boolean:
                            cfc.parameters.AddElement(new bool_const_node((bool)config_value, null));
                            break;
                        case TypeCode.Double:
                            cfc.parameters.AddElement(new double_const_node((double)config_value, null));
                            break;
                        default:
                            throw new NotSupportedException("Config value type is nort supported");
                    }
                    sl.statements.AddElement(cfc);
                }
            }
            if (units[0].MainArgsParameter!=null)
            {
                _main_function.parameters.AddElement(units[0].MainArgsParameter);
                sl.statements.AddElementFirst(units[0].MainArgsAssignExpr);
            }
            if (units[0].IsConsoleApplicationVariableAssignExpr!=null)
            {
                sl.statements.AddElementFirst(units[0].IsConsoleApplicationVariableAssignExpr);
            }
            else if (SystemLibrary.SystemLibInitializer.ConfigVariable.sym_info is compiled_variable_definition && units[0].IsConsoleApplicationVariableValue.constant_value)
            {
                bool is_console = true;
                
                foreach (var ns in units[0].used_namespaces)
                {
                    if (graph_modules.Contains(ns))
                    {
                        is_console = false;
                        break;
                    }
                }
                if (is_console)
                {
                    var ccnf = SystemLibrary.SystemLibInitializer.ConfigVariable.sym_info as compiled_variable_definition;
                    basic_function_call bbfc = new basic_function_call(SystemLibrary.SystemLibrary.bool_assign as basic_function_node, null);
                    bbfc.parameters.AddElement(new static_compiled_variable_reference(ccnf.cont_type.find_in_type(StringConstants.IsConsoleApplicationVariableName)[0].sym_info as compiled_variable_definition, ccnf.cont_type, null));
                    bbfc.parameters.AddElement(new bool_const_node(true, null));
                    sl.statements.AddElement(bbfc);
                }
                
            }
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].main_function != null)
                {
                	if (units[i].main_function.name != StringConstants.temp_main_function_name)
                	{
                		common_namespace_function_call cnfc = new common_namespace_function_call(units[i].main_function, loc);
                    	sl.statements.AddElement(cnfc);
                	}
                	else
                	{
                		common_namespace_function_call cnfc = new common_namespace_function_call(units[i].main_function, loc);
                    	sl2.statements.AddElement(cnfc);
                	}
                }
            }
            //if (units.Count == 1)
            for (int i = 0; i < used_stand_modules.Length; i++)
            {
                Type t = NetHelper.NetHelper.FindRtlType(used_stand_modules[i] + "." + used_stand_modules[i]);
                if (t == null)
                    continue;
                compiled_type_node ctn = compiled_type_node.get_type_node(t);
                System.Reflection.MethodInfo mi = ctn.compiled_type.GetMethod("__InitModule__");
                if (mi == null)
                {
                    continue;
                }
                compiled_static_method_call csmc = new compiled_static_method_call(compiled_function_node.get_compiled_method(mi), null);
                sl.statements.AddElement(csmc);
            }
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (units[i].finalization_method != null)
                {
                    common_namespace_function_call cnfc = new common_namespace_function_call(units[i].finalization_method, loc);
                    sl2.statements.AddElement(cnfc);
                }
            }
            //if (units.Count == 1)
            for (int i = 0; i < used_stand_modules.Length; i++)
            {
                Type t = NetHelper.NetHelper.FindRtlType(used_stand_modules[i] + "." + used_stand_modules[i]);
                if (t == null)
                    continue;
                compiled_type_node ctn = compiled_type_node.get_type_node(t);
                System.Reflection.MethodInfo mi = ctn.compiled_type.GetMethod("__FinalizeModule__");
                if (mi == null)
                    continue;
                compiled_static_method_call csmc = new compiled_static_method_call(compiled_function_node.get_compiled_method(mi), null);
                sl2.statements.AddElement(csmc);
            }
            sl2 = new statements_list(loc);
            basic_function_call bfc = new basic_function_call(SystemLibrary.SystemLibrary.bool_assign as basic_function_node,null);
            bfc.parameters.AddElement(new namespace_variable_reference(init_var,null));
            bfc.parameters.AddElement(new bool_const_node(true,null));
            sl.statements.AddElementFirst(bfc);
            bfc = new basic_function_call(SystemLibrary.SystemLibrary.bool_not as basic_function_node,null);
            bfc.parameters.AddElement(new namespace_variable_reference(init_var,null));
            sl2.statements.AddElement(new if_node(bfc,sl,null,null));
            init_func.function_code = sl2;
            sl = new statements_list(null);
            
            sl.statements.AddElement(new common_namespace_function_call(init_func,null));
            common_namespace_function_node init_variables_func = new common_namespace_function_node("$_InitVariables_", null, null, (common_namespace_node)_main_function.comprehensive_namespace, null);
            init_variables_func.function_code = new statements_list(null);
            ((common_namespace_node)_main_function.comprehensive_namespace).functions.AddElement(init_variables_func);
            
            sl.statements.AddElement(new common_namespace_function_call(init_variables_func,null));
            _init_code = sl;
            
        }

        public void create_main_function_as_in_module()
        {
            add_needed_cctors();
            statements_list sl = new statements_list(loc);
            statements_list intern_sl = new statements_list(loc);
            statements_list sl2 = sl;
            sl = new statements_list(null);
            common_namespace_function_node init_func = new common_namespace_function_node("$_Init_", null, null, common_namespaces[0], null);
            common_namespaces[0].functions.AddElement(init_func);
            namespace_variable init_var = new namespace_variable("$is_init", SystemLibrary.SystemLibrary.bool_type, common_namespaces[0], null);
            common_namespaces[0].variables.AddElement(init_var);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].main_function != null)
                {
                    common_namespace_function_call cnfc = new common_namespace_function_call(units[i].main_function, loc);
                    sl.statements.AddElement(cnfc);
                }
            }
            /*for (int i = units.Count - 1; i >= 0; i--)
            {
                if (units[i].finalization_method != null)
                {
                    common_namespace_function_call cnfc = new common_namespace_function_call(units[i].finalization_method, loc);
                    sl.statements.AddElement(cnfc);
                }
            }*/
            sl2 = new statements_list(loc);
            basic_function_call bfc = new basic_function_call(SystemLibrary.SystemLibrary.bool_assign as basic_function_node, null);
            bfc.parameters.AddElement(new namespace_variable_reference(init_var, null));
            bfc.parameters.AddElement(new bool_const_node(true, null));
            sl.statements.AddElementFirst(bfc);
            bfc = new basic_function_call(SystemLibrary.SystemLibrary.bool_not as basic_function_node, null);
            bfc.parameters.AddElement(new namespace_variable_reference(init_var, null));
            sl2.statements.AddElement(new if_node(bfc, sl, null, null));
            init_func.function_code = sl2;
            _init_code = new common_namespace_function_call(init_func, null);
            add_initialization_to_cctors();
        }
        
        private void add_needed_cctors()
        {
            foreach (common_namespace_node cnn in common_namespaces)
            {

                foreach (common_type_node ctn in cnn.types)
                {
                    if (!ctn.IsInterface && ctn.static_constr == null)
                    {
                        ctn.static_constr = new common_method_node(StringConstants.static_ctor_prefix + "Create", null, ctn, SemanticTree.polymorphic_state.ps_static, SemanticTree.field_access_level.fal_private, null);
                        ctn.static_constr.is_constructor = true;
                        ctn.static_constr.function_code = new statements_list(null);
                        ctn.methods.AddElement(ctn.static_constr);
                        (ctn.static_constr.function_code as statements_list).statements.AddElement(new return_node(null, null));
                    }

                }
            }
        }
        
        private void add_initialization_to_cctors()
        {
        	foreach (common_namespace_node cnn in common_namespaces)
        	{
                if (cnn.is_main)
                {
                    foreach (common_type_node ctn in cnn.types)
                    {
                        if (ctn.static_constr == null)
                        {
                            ctn.static_constr = new common_method_node(StringConstants.static_ctor_prefix + "Create", null, ctn, SemanticTree.polymorphic_state.ps_static, SemanticTree.field_access_level.fal_private, null);
                            ctn.static_constr.is_constructor = true;
                            ctn.static_constr.function_code = new statements_list(null);
                            ctn.methods.AddElement(ctn.static_constr);
                            (ctn.static_constr.function_code as statements_list).statements.AddElement(new return_node(null, null));
                        }
                        (ctn.static_constr.function_code as statements_list).statements.AddElementFirst(_init_code);
                    }
                }
                else
                {
                    
                }
        	}
        }
        
        private statement_node _init_code;
        
        public statement_node initialization_code
        {
        	get
        	{
        		return _init_code;
        	}
        }
        
        SemanticTree.IStatementNode SemanticTree.IProgramNode.InitializationCode
        {
        	get
        	{
        		return _init_code;
        	}
        }
        
        /// <summary>
        /// Главная функция. Ее выполнение равносильно выполнению программы.
        /// Она включает вызовы методов инициализации модулей (в начале), выполнение основной программы
        /// и вызовы методов финализации модулей.
        /// </summary>
        public common_namespace_function_node main_function
		{
			get
			{
				return _main_function;
			}
            set
            {
                _main_function = value;
            }
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.program_node;
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

		SemanticTree.ICommonNamespaceFunctionNode SemanticTree.IProgramNode.main_function
		{
			get
			{
				return _main_function;
			}
		}

        public System.Collections.Generic.List<SemanticTree.IGenericTypeInstance> generic_type_instances
        {
            get
            {
                return generic_convertions.all_type_instances;
            }
        }

        public System.Collections.Generic.List<SemanticTree.IGenericFunctionInstance> generic_function_instances
        {
            get
            {
                return generic_convertions.all_function_instances;
            }
        }
	}

	[Serializable]
	public class dll_node : program_base, SemanticTree.IDllNode
	{
        /// <summary>
        /// Метод инициализации dll. Он определен только здесь.
        /// Отсюда его и надо компилировать в выходной код.
        /// </summary>
		private common_namespace_function_node _initialization_function;

        /// <summary>
        /// Метод финализации dll. Он определен только здесь.
        /// Отсюда его и надо компилироавть в выходной код.
        /// </summary>
		private common_namespace_function_node _finalization_function;

        /// <summary>
        /// Конструктор корневого узла dll.
        /// </summary>
        /// <param name="initialization_function">Метод инициализации dll.</param>
        /// <param name="finalization_function">Метод финализации dll.</param>
        /// <param name="loc">Расположение заголовка главного unit-а dll.</param>
		public dll_node(common_namespace_function_node initialization_function,
			common_namespace_function_node finalization_function, location loc) : base(loc)
		{
			_initialization_function=initialization_function;
			_finalization_function=finalization_function;
		}

        /// <summary>
        /// Метод инициализации dll.
        /// </summary>
		public common_namespace_function_node initialization_function
		{
			get
			{
				return _initialization_function;
			}
		}

        /// <summary>
        /// Метод финализации dll.
        /// </summary>
		public common_namespace_function_node finalization_function
		{
			get
			{
				return _finalization_function;
			}
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.dll_node;
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

		SemanticTree.ICommonNamespaceFunctionNode SemanticTree.IDllNode.initialization_function
		{
			get
			{
				return _initialization_function;
			}
		}

		SemanticTree.ICommonNamespaceFunctionNode SemanticTree.IDllNode.finalization_function
		{
			get
			{
				return _finalization_function;
			}
		}

	}

}
