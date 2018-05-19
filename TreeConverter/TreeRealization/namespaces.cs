// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;
using System.Linq;

namespace PascalABCCompiler.TreeRealization
{
    /// <summary>
    /// Базовый класс для представления пространств имен.
    /// </summary>
	[Serializable]
	public abstract class namespace_node : definition_node, SemanticTree.INamespaceNode
	{
        /// <summary>
        /// Полное имя пространства имен.
        /// </summary>
		public abstract string namespace_full_name
		{
			get;
		}

        /// <summary>
        /// Поиск символа в пространстве имен.
        /// </summary>
        /// <param name="name">Имя элемента для поиска.</param>
        /// <returns>Информация о найденном символе. null, если ни чего не найдено.</returns>
		public abstract List<SymbolInfo> find(string name);

        /// <summary>
        /// Имя пространства имен.
        /// </summary>
		public abstract string namespace_name
		{
			get;
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

        /// <summary>
        /// Обобщенный тип узла.
        /// </summary>
		public override general_node_type general_node_type
		{
			get
			{
				return general_node_type.namespace_node;
			}
		}

	}

    /// <summary>
    /// Класс, представляющий обычное пространство имен.
    /// </summary>
	[Serializable]
	public class common_namespace_node : namespace_node, SemanticTree.ICommonNamespaceNode
	{
        public override string ToString()
        {
            if(is_main)
                return "main";

            if (namespace_full_name == "")
                return GetType().Name;
            return namespace_full_name;
        }
        /// <summary>
        /// Список типов, вложенных в пространство имен.
        /// </summary>
		private readonly common_type_node_list _types=new common_type_node_list();

        //ssyy добавил
        /// <summary>
        /// Список шаблонных классов, вложенных в пространство имен.
        /// </summary>
        private readonly template_class_list _templates = new template_class_list();
        //\ssyy

        //ssyy добавил
        /// <summary>
        /// Список определённых в пространстве имен синонимов типов.
        /// </summary>
        private readonly List<type_synonym> _type_synonyms = new List<type_synonym>();
        //\ssyy

        /// <summary>
        /// Список определённых в пространстве имен типов-указателей.
        /// </summary>
        private readonly List<ref_type_node> _ref_types = new List<ref_type_node>();

        /// <summary>
        /// Список runtime типов, вложенных в пространство имен.
        /// </summary>
        private readonly compiled_type_node_list _runtime_types = new compiled_type_node_list();
        
        /// <summary>
        /// Список переменных, определенных в прстранстве имен.
        /// </summary>
        private readonly namespace_variable_list _variables = new namespace_variable_list();

        private readonly namespace_event_list _events = new namespace_event_list();

        /// <summary>
        /// Список меток, определенных в прстранстве имен.
        /// </summary>
        private readonly List<label_node> _labels = new List<label_node>();

        /// <summary>
        /// Список функций, определенных в пространстве имен.
        /// </summary>
        private readonly common_namespace_function_node_list _functions = new common_namespace_function_node_list();

        /// <summary>
        /// Список констант, определенных в пространстве имен.
        /// </summary>
		private readonly namespace_constant_definition_list _constants=new namespace_constant_definition_list();

        /// <summary>
        /// Список пространств имен, вложенных в пространство имен.
        /// </summary>
		private readonly common_namespace_node_list _namespaces= new common_namespace_node_list();

        /// <summary>
        /// Пространство имен, в которое вложено это пространство имен.
        /// </summary>
		private namespace_node _comprehensive_namespace;

        /// <summary>
        /// Модуль, в котором содержится это пространство имен.
        /// При генерации кода, когда возможна ситуация при которой простраство имен расположено в нескольких модулях,
        /// этот элемент указывает на один из модулей в котором встретилось это пространство имен.
        /// </summary>
		private unit_node _cont_unit;

        /// <summary>
        /// Имя пространства имен.
        /// </summary>
		private string _name;

        /// <summary>
        /// Область видимости пространства имен.
        /// </summary>
		private SymbolTable.Scope _scope;

        /// <summary>
        /// Расположения заголовка пространства имен.
        /// </summary>
		private location _loc;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="comprehensive_namespace">Пространство имен, в которое вложено это пространство имен.</param>
        /// <param name="cont_unit">Модуль, в котором содержится это пространство имен.</param>
        /// <param name="name">Имя пространства имен.</param>
        /// <param name="scope">Область видимости пространства имен.</param>
        /// <param name="loc">Расположения заголовка пространства имен.</param>
        public common_namespace_node(namespace_node comprehensive_namespace, unit_node cont_unit, string name, 
            SymbolTable.Scope scope, location loc)
        {
            _comprehensive_namespace = comprehensive_namespace;
            _cont_unit = cont_unit;
            _name = name;
            _scope = scope;
            _loc = loc;
        }

        /// <summary>
        /// Клонирует пространство имен.
        /// </summary>
        /// <returns>Копия пространства иен.</returns>
        public common_namespace_node clone()
        {
            common_namespace_node cnn = new common_namespace_node(this._comprehensive_namespace, this._cont_unit,
                this._name, this._scope, this._loc);
            cnn.merge_with_namespace(this);
            return cnn;
        }

        /// <summary>
        /// Добавляет к текущему пространству имен указанное пространство имен.
        /// </summary>
        /// <param name="merge_with">Пространство имен для добавления.</param>
        public void merge_with_namespace(common_namespace_node merge_with)
        {
            _types.AddRange(merge_with._types);
            //ssyy
            _templates.AddRange(merge_with._templates);
            _type_synonyms.AddRange(merge_with._type_synonyms);
            //\ssyy
            _variables.AddRange(merge_with._variables);
            _functions.AddRange(merge_with._functions);
            _constants.AddRange(merge_with._constants);
            _namespaces.AddRange(merge_with._namespaces);
        }

        private bool _is_main;
        public bool is_main
        {
            get
            {
                return _is_main;
            }
            set
            {
                _is_main = value;
            }
        }
        
        bool SemanticTree.ICommonNamespaceNode.IsMain
        {
        	get
        	{
        		return _is_main;
        	}
        }
        /// <summary>
        /// Область видимости пространства имен.
        /// </summary>
		public SymbolTable.Scope scope
		{
			get
			{
				return _scope;
			}
            set
            {
                _scope = value;
            }
		}

        /// <summary>
        /// Расположения заголовка пространства имен.
        /// </summary>
		public location loc
		{
			get
			{
				return _loc;
			}
		}

        /// <summary>
        /// Расположения заголовка пространства имен.
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
        /// Имя пространства имен.
        /// </summary>
		public override string namespace_name
		{
			get
			{
				return _name;
			}
		}
		
		public void SetNamespaceName(string name)
		{
			_name = name;
		}
		
        /// <summary>
        /// Пространство имен, в которое вложено это пространство имен.
        /// </summary>
		public namespace_node comprehensive_namespace
		{
			get
			{
				return _comprehensive_namespace;
			}
		}

        /// <summary>
        /// Модуль, в котором содержится это пространство имен.
        /// При генерации кода, когда возможна ситуация при которой простраство имен расположено в нескольких модулях,
        /// этот элемент указывает на один из модулей в котором встретилось это пространство имен.
        /// </summary>
		public unit_node cont_unit
		{
			get
			{
				return _cont_unit;
			}
		}

        /// <summary>
        /// Список типов, вложенных в пространство имен.
        /// </summary>
		public common_type_node_list types
		{
			get
			{
				return _types;
			}
        }

        //ssyy добавил
        /// <summary>
        /// Список шаблонных классов, вложенных в пространство имен.
        /// </summary>
        public template_class_list templates
        {
            get
            {
                return _templates;
            }
        }

        /// <summary>
        /// Список определённых в пространстве имен синонимов типов.
        /// </summary>
        public List<type_synonym> type_synonyms
        {
            get
            {
                return _type_synonyms;
            }
        }
        //\ssyy

        /// <summary>
        /// Список определённых в пространстве имен типов-указателей.
        /// </summary>
        public List<ref_type_node> ref_types
        {
            get
            {
                return _ref_types;
            }
        }


        /// <summary>
        /// Список типов, вложенных в пространство имен.
        /// </summary>
        public compiled_type_node_list runtime_types
        {
            get
            {
                return _runtime_types;
            }
        }

        /// <summary>
        /// Список пространств имен, вложенных в пространство имен.
        /// </summary>
		public common_namespace_node_list namespaces
		{
			get
			{
				return _namespaces;
			}
		}

        /// <summary>
        /// Список переменных, определенных в прстранстве имен.
        /// </summary>
		public namespace_variable_list variables
		{
			get
			{
				return _variables;
			}
		}

        public namespace_event_list events
        {
            get
            {
                return _events;
            }
        }

        /// <summary>
        /// Список меток, определенных в прстранстве имен.
        /// </summary>
        public List<label_node> labels
        {
            get
            {
                return _labels;
            }
        }

        /// <summary>
        /// Список функций, определенных в пространстве имен.
        /// </summary>
		public common_namespace_function_node_list functions
		{
			get
			{
				return _functions;
			}
		}

        /// <summary>
        /// Список констант, определенных в пространстве имен.
        /// </summary>
		public namespace_constant_definition_list constants
		{
			get
			{
				return _constants;
			}
		}

        /// <summary>
        /// Полное имя пространства имен.
        /// </summary>
		public override string namespace_full_name
		{
			get
			{
				if (_comprehensive_namespace==null)
				{
					return _name;
				}
				return (_comprehensive_namespace.namespace_full_name+"."+_name);
			}
		}

        private bool _from_pcu;
        public bool from_pcu
        {
            get
            {
                return _from_pcu;
            }
            set
            {
                _from_pcu = value;
            }
        }

        /// <summary>
        /// Поиск символа в этом пространстве имен.
        /// </summary>
        /// <param name="name">Искомое имя.</param>
        /// <returns>Информация о найденом символе. null, если ни чего не найдена.</returns>
		public override List<SymbolInfo> find(string name)
		{
            return _scope.Find(name);//c,cc,c,cc
        }
        public SymbolInfo findFirstOnlyInNamespace(string name)
        {
            var temp = _scope.FindOnlyInScope(name);//c,cc,c,cc
            return temp?.First();
        }
        public List<SymbolInfo> findOnlyInNamespace(string name)
        {
            return _scope.FindOnlyInScope(name);//c,cc,c,cc
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.common_namespace_node;
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

        SemanticTree.ICommonNamespaceNode[] SemanticTree.ICommonNamespaceNode.nested_namespaces
		{
			get
			{
				return (this._namespaces.ToArray());
			}
		}

		SemanticTree.ICommonTypeNode[] SemanticTree.ICommonNamespaceNode.types
		{
			get
			{
				return (this._types.ToArray());
			}
		}
		
		SemanticTree.ITypeSynonym[] SemanticTree.ICommonNamespaceNode.type_synonims
		{
			get
			{
				return this._type_synonyms.ToArray();
			}
		}

        SemanticTree.ITemplateClass[] SemanticTree.ICommonNamespaceNode.templates
        {
            get
            {
                return this.templates.ToArray();
            }
        }

		SemanticTree.ICommonNamespaceVariableNode[] SemanticTree.ICommonNamespaceNode.variables
		{
			get
			{
				return (this._variables.ToArray());
			}
		}

        SemanticTree.ICommonNamespaceEventNode[] SemanticTree.ICommonNamespaceNode.events
        {
            get
            {
                return (this._events.ToArray());
            }
        }

		SemanticTree.ICommonNamespaceFunctionNode[] SemanticTree.ICommonNamespaceNode.functions
		{
			get
			{
				return (this._functions.ToArray());
			}
		}

		SemanticTree.INamespaceNode SemanticTree.ICommonNamespaceNode.comprehensive_namespace
		{
			get
			{
				return (this._comprehensive_namespace);
			}
		}

        SemanticTree.INamespaceConstantDefinitionNode[] SemanticTree.ICommonNamespaceNode.constants
        {
            get
            {
                return (this._constants.ToArray());
            }
        }

        //(ssyy) Флаг, чтобы не проверять второй раз наличие тел всех описанных функций
        public bool predefinitions_checked = false;

        //(ssyy) Набор типов для сохранения на диск
        private common_type_node_list _non_template_types = null;

        public common_type_node_list non_template_types
        {
            get
            {
                return _non_template_types;
            }
        }

        public void MakeNonTemplateTypesList()
        {
            _non_template_types = new common_type_node_list();
            foreach (common_type_node ctn in _types)
            {
                if (ctn.original_template == null)
                {
                    _non_template_types.AddElement(ctn);
                }
            }
        }
    }

    /// <summary>
    /// Класс, представляющий пространство имен в откомпилированном коде.
    /// </summary>
	[Serializable]
	public class compiled_namespace_node : namespace_node, SemanticTree.ICompiledNamespaceNode
	{
        /// <summary>
        /// Имя (полное) пространства имен.
        /// </summary>
		private string _name;
        private common_namespace_node _common_namespace;

        private SymbolTable.TreeConverterSymbolTable _tcst;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="name">Полное имя пространства имен.</param>
        public compiled_namespace_node(string name, SymbolTable.TreeConverterSymbolTable tcst)
		{
			_name=name;
            _tcst = tcst;
		}

        /// <summary>
        /// Получение имени пространства имен по его полному имени.
        /// </summary>
        /// <param name="full_name">Полное имя пространства имен.</param>
        /// <returns>Имя пространства имен.</returns>
        private string get_namespace_name(string full_name)
        {
            int last_dot_ind = full_name.LastIndexOf('.');
            if (last_dot_ind < 0)
            {
                return _name;
            }
            return new string(_name.ToCharArray(last_dot_ind,_name.Length-last_dot_ind));
        }

        private static Dictionary<string, compiled_namespace_node> compiled_namespaces = new Dictionary<string, compiled_namespace_node>();
        public static compiled_namespace_node get_compiled_namespace(string full_name, SymbolTable.TreeConverterSymbolTable tcst)
        {
            compiled_namespace_node cnn = null;
            if (!compiled_namespaces.TryGetValue(full_name, out cnn))
            {
                cnn = new compiled_namespace_node(full_name, tcst);
                compiled_namespaces.Add(full_name, cnn);
            }
            cnn._tcst = tcst;
            return cnn;
        }

        /// <summary>
        /// Имя пространства имен (не полное).
        /// </summary>
		public override string namespace_name
		{
			get
			{
                return get_namespace_name(_name);
			}
		}

        /// <summary>
        /// Полное имя пространства имен.
        /// </summary>
		public override string namespace_full_name
		{
			get
			{
				return _name;
			}
		}

        public common_namespace_node common_namespace
        {
            get
            {
                return _common_namespace;
            }
            set
            {
                _common_namespace = value;
            }
        }

        /// <summary>
        /// Поиск символа в пространстве имен.
        /// </summary>
        /// <param name="name">Имя для поиска.</param>
        /// <returns>Первый элемент списка найденных имен. null если ни чего не найдено.</returns>
		public override List<SymbolInfo> find(string name)
		{
            bool is_ns = NetHelper.NetHelper.IsNetNamespace(_name + "." + name);
            List<SymbolInfo> sil = null;
            if (is_ns)
            {
                compiled_namespace_node cnn = compiled_namespace_node.get_compiled_namespace(_name + "." + name, _tcst);
                sil = new List<SymbolInfo> { new SymbolInfo(cnn) };
            }
            else
            {
                //Kolay changed next string.   throwOnError=false  ignoreCase=true,   .
                //Type t = Type.GetType(_name+"."+name,false,true);
                if (common_namespace != null)
                {
                    sil = common_namespace.scope.FindOnlyInScope(name);
                    if (sil != null)
                        return sil;
                }
                Type t = NetHelper.NetHelper.FindType(_name + "." + name);
                if (t != null)
                {
                    sil = new List<SymbolInfo> { new SymbolInfo(compiled_type_node.get_type_node(t, _tcst)) };
                }
                else
                {
                	t = NetHelper.NetHelper.FindType(_name+"."+_name);
                	if (t != null && NetHelper.NetHelper.IsEntryType(t))
                	{
                		sil = NetHelper.NetHelper.FindName(t,name);
                        if (sil == null)
                        {
                            type_node tn = NetHelper.NetHelper.FindCompiledPascalType(_name + "." + name);
                            if (tn != null)
                                sil = new List<SymbolInfo> { new SymbolInfo(tn) };
                            else
                            {
                                template_class tc = NetHelper.NetHelper.FindCompiledTemplateType(_name + "." + name);
                                if (tc != null)
                                    sil = new List<SymbolInfo> { new SymbolInfo(tc) };
                            }
                        }
                	}
                }
            }
            return sil;

		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.compiled_namespace_node;
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

namespace PascalABCCompiler.SyntaxTree
{
    public class syntax_namespace_node : base_syntax_namespace_node
    {
        PascalABCCompiler.TreeRealization.unit_node_list _referenced_units;

        public syntax_namespace_node(string name):base(name)
        {

        }

        public unit_node_list referenced_units { get => _referenced_units; set => _referenced_units = value; }
    }
}
