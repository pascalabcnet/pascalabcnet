// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

using PascalABCCompiler.Collections;

namespace PascalABCCompiler.TreeRealization
{

    [Serializable]
    public abstract class base_function_call : expression_node, SemanticTree.IFunctionCallNode
    {
        /// <summary>
        /// Список фактических параметров при вызове функции.
        /// </summary>
        private readonly expressions_list _parametres = new expressions_list();

        public base_function_call(type_node tn, location loc) : base(tn, loc)
        {
        }


        private bool _isExplicitConversion = false;
        public bool IsExplicitConversion
        {
            get
            {
                return _isExplicitConversion;
            }
            set
            {
                _isExplicitConversion = value;
            }
        }

        public virtual base_function_call copy()
        {
            return this;
        }


        /// <summary>
        /// Список фактических параметров. Количество и типы формальных и фактических параметров сверяется
        /// на этапе построения семантического дерева. При необходимости при построении семантического дерева
        /// вставляются узлы преобразования типов.
        /// </summary>
        public expressions_list parameters
        {
            get
            {
                return _parametres;
            }
        }

        /// <summary>
        /// Массив с параметрами функции.
        /// Используется при обходе дерева посетителем.
        /// </summary>
        public SemanticTree.IExpressionNode[] real_parameters
        {
            get
            {
                return (_parametres.ToArray());
            }
        }

        public abstract function_node simple_function_node
        {
            get;
        }

        /// <summary>
        /// Вызываемая функция.
        /// Используется при обходе дерева посетителем.
        /// </summary>
        public SemanticTree.IFunctionNode function
        {
            get
            {
                return simple_function_node;
            }
        }

        private type_node _ret_type = null;
        public type_node ret_type
        {
            get
            {
                if (_ret_type == null)
                    return simple_function_node.return_value_type;
                return _ret_type;
            }
            set
            {
                _ret_type = value;
            }
        }

        public override type_node type
        {
            get
            {
                if (_ret_type == null)
                {
                    return base.type;
                }
                return ret_type;
            }
        }

        //ssyy
        public virtual bool last_result_function_call
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        //\ssyy

        
    }


    /// <summary>
    /// Базовый класс, для представления вызовов функций.
    /// </summary>
    /// <typeparam name="FunctionNodeType">Тип узла функции, которую мы вызываем.</typeparam>
    [Serializable]
    public abstract class base_function_call_with_method<FunctionNodeType> : base_function_call
        where FunctionNodeType : function_node
    {
        /*private type_node __type=null;
        public virtual void SetType(type_node _type)
        {
            __type=__type;
        }
        public override type_node type
        {
            get
            {
                if (__type != null)
                    return __type;
                return base.type;
            }
        }*/


        /// <summary>
        /// Вызываемая функция.
        /// </summary>
        private FunctionNodeType _function_node;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="_function">Вызывамый метод.</param>
        public base_function_call_with_method(FunctionNodeType _function,location loc)
            : base(_function.return_value_type,loc)
        {
            _function_node = _function;
        }

        /// <summary>
        /// Вызываемый метод.
        /// </summary>
        public FunctionNodeType function_node
        {
            get
            {
                return _function_node;
            }
        }

        public override function_node simple_function_node
        {
            get
            {
                return _function_node;
            }
        }
    }

    /// <summary>
    /// Класс, представляющий вызов базового метода.
    /// </summary>
    [Serializable]
    public class basic_function_call : base_function_call_with_method<basic_function_node>, SemanticTree.IBasicFunctionCallNode
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="basic_func">Ссылка на функцию, которую мы вызываем.</param>
        public basic_function_call(basic_function_node basic_func,location loc)
            : base(basic_func,loc)
        {
        }
		
        public basic_function_call(basic_function_node basic_func,location loc, params expression_node[] prms)
        	: base(basic_func,loc)
        {
        	this.parameters.AddRange(prms);
        }
        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.basic_function_call;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.IBasicFunctionCallNode)this);
        }

        /// <summary>
        /// Вызываемый базовый метод.
        /// Используется для обхода дерева посетителем.
        /// </summary>
        SemanticTree.IBasicFunctionNode SemanticTree.IBasicFunctionCallNode.basic_function
        {
            get
            {
                return function_node;
            }
        }
    }

    /// <summary>
    /// Класс, представляющий вызов метода, расположенного в пространстве имен.
    /// </summary>
    [Serializable]
    public class common_namespace_function_call : base_function_call_with_method<common_namespace_function_node>, SemanticTree.ICommonNamespaceFunctionCallNode
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="basic_func">Ссылка на функцию, которую мы вызываем.</param>
        public common_namespace_function_call(common_namespace_function_node namespace_func,location loc)
            : base(namespace_func,loc)
        {
            if (namespace_func is generic_namespace_function_instance_node)
            {
                common_namespace_function_node cnfn = (namespace_func as generic_namespace_function_instance_node).original_function as common_namespace_function_node;
                if (cnfn.function_code is wrapped_statement)
                {
                    wrapped_statement ws = cnfn.function_code as wrapped_statement;
                    cnfn.function_code = new empty_statement(null);
                    cnfn.function_code = ws.restore();
                }
            }
            else if (namespace_func.function_code is wrapped_statement)
            {
                wrapped_statement ws = namespace_func.function_code as wrapped_statement;
                namespace_func.function_code = new empty_statement(null);
                namespace_func.function_code = ws.restore();
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.ICommonNamespaceFunctionCallNode)this);
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.common_namespace_function_call;
            }
        }

        /// <summary>
        /// Вызываемый базовый метод.
        /// Используется для обхода дерева посетителем.
        /// </summary>
        SemanticTree.ICommonNamespaceFunctionNode SemanticTree.ICommonNamespaceFunctionCallNode.namespace_function
        {
            get
            {
                return function_node;
            }
        }

        public override base_function_call copy()
        {
            common_namespace_function_call cnfc_copy = new common_namespace_function_call(this.function_node, this.location);
            cnfc_copy.parameters.AddRange(this.parameters);
            return cnfc_copy;
        }
    }

    /// <summary>
    /// Класс, представляющий вызов метода, вложенного в другой метод.
    /// </summary>
    [Serializable]
    public class common_in_function_function_call : base_function_call_with_method<common_in_function_function_node>,
        SemanticTree.ICommonNestedInFunctionFunctionCallNode
    {
        /// <summary>
        /// Разность между статической глубиной вызываемого метода и статической глубиной вызывающего метода.
        /// </summary>
        private int _static_depth;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="common_func">Вызываемый метод.</param>
        /// <param name="static_depth">Разность между статической глубиной вызываемого метода и статической глубиной вызывающего метода.</param>
        public common_in_function_function_call(common_in_function_function_node common_func, int static_depth,location loc)
            :base(common_func,loc)
        {
            _static_depth = static_depth;
        }

        /// <summary>
        /// Статическая глубина вложенной функции.
        /// </summary>
        public int static_depth
        {
            get
            {
                return _static_depth;
            }
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.common_in_function_function_call;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.ICommonNestedInFunctionFunctionCallNode)this);
        }

        /// <summary>
        /// Вызываемый метод.
        /// Используется при обходе дерева посетителем.
        /// </summary>
        public SemanticTree.ICommonNestedInFunctionFunctionNode common_function
        {
            get
            {
                return function_node;
            }
        }

        public override base_function_call copy()
        {
            common_in_function_function_call cffc_copy = new common_in_function_function_call(this.function_node, this.static_depth, this.location);
            cffc_copy.parameters.AddRange(this.parameters);
            return cffc_copy;
        }
    }

    [Serializable]
    public class common_method_call : base_function_call_with_method<common_method_node>, SemanticTree.ICommonMethodCallNode
    {
        private expression_node _obj;

        public common_method_call(common_method_node method, expression_node obj,location loc)
            : base(method,loc)
        {
            _obj = obj;
            _virtual_call = true;
        }

        //Экземпляр класса, данный метод которого нужно вызвать.
        public expression_node obj
        {
            get
            {
                return _obj;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.common_method_call;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.ICommonMethodCallNode)this);
        }

        SemanticTree.ICommonMethodNode SemanticTree.ICommonMethodCallNode.method
        {
            get
            {
                return function_node;
            }
        }

        SemanticTree.IExpressionNode SemanticTree.ICommonMethodCallNode.obj
        {
            get
            {
                return _obj;
            }
        }

        //ssyy
        private bool _last_result_function_call;

        public override bool last_result_function_call
        {
            get
            {
                return _last_result_function_call;
            }
            set
            {
                _last_result_function_call = value;
            }
        }
        //\ssyy

        bool _virtual_call;
        public bool virtual_call
        {
            get
            {
                return _virtual_call;
            }
            set
            {
                _virtual_call = value;
            }
        }

        public override base_function_call copy()
        {
            common_method_call cmc_copy = new common_method_call(this.function_node, this.obj, this.location);
            cmc_copy.parameters.AddRange(this.parameters);
            return cmc_copy;
        }
    }

    [Serializable]
    public class common_static_method_call : base_function_call_with_method<common_method_node>, SemanticTree.ICommonStaticMethodCallNode
    {
        private common_type_node _common_type;//zachem dublirovat svojstvo iz common_method_node????

        public common_static_method_call(common_method_node static_method,location loc) :
            base(static_method,loc)
        {
            //_method = static_method;
            _common_type = static_method.cont_type;
        }

        //Тип, статический метод которого вызываем.
        public common_type_node common_type
        {
            get
            {
                if (_common_type != null)
                    return _common_type;
                return function_node.cont_type;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.common_static_method_call;
            }
        }


        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.ICommonStaticMethodCallNode)this);
        }

        SemanticTree.ICommonTypeNode SemanticTree.ICommonStaticMethodCallNode.common_type
        {
            get
            {
                if (_common_type != null)
                    return _common_type;
                return function_node.cont_type;
            }
        }

        SemanticTree.ICommonMethodNode SemanticTree.ICommonStaticMethodCallNode.static_method
        {
            get
            {
                return function_node;
            }
        }

        public override base_function_call copy()
        {
            common_static_method_call cmc_copy = new common_static_method_call(this.function_node, this.location);
            cmc_copy.parameters.AddRange(this.parameters);
            return cmc_copy;
        }

    }

    [Serializable]
    public class common_constructor_call : common_static_method_call, SemanticTree.ICommonConstructorCall
    {
        public common_constructor_call(common_method_node constructor_node,location loc) :
            base(constructor_node,loc)
        {
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.ICommonConstructorCall)this);
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.common_constructor_call;
            }
        }
        //ssyy
        //Станет ложью, если это вызов конструктора предка
        public bool _new_obj_awaited = true;

        public bool new_obj_awaited()
        {
            return _new_obj_awaited;
        }
        //\ssyy
    }

    [Serializable]
    public class compiled_function_call : base_function_call_with_method<compiled_function_node>, SemanticTree.ICompiledMethodCallNode
    {
        private expression_node _obj;

        public compiled_function_call(compiled_function_node compiled_method, expression_node obj,location loc) :
            base(compiled_method,loc)
        {
            _obj = obj;
        }

        //Экземпляр класса, данный метод которого нужно вызвать.
        public expression_node obj
        {
            get
            {
                return _obj;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.compiled_function_call;
            }
        }


        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.ICompiledMethodCallNode)this);
        }

        SemanticTree.IExpressionNode SemanticTree.ICompiledMethodCallNode.obj
        {
            get
            {
                return _obj;
            }
        }

        SemanticTree.ICompiledMethodNode SemanticTree.ICompiledMethodCallNode.compiled_method
        {
            get
            {
                return function_node;
            }
        }

        //ssyy
        private bool _last_result_function_call;

        public override bool last_result_function_call
        {
            get
            {
                return _last_result_function_call;
            }
            set
            {
                _last_result_function_call = value;
            }
        }
        //\ssyy

        bool _virtual_call = true;
        public bool virtual_call
        {
            get
            {
                return _virtual_call;
            }
            set
            {
                _virtual_call = value;
            }
        }

        public override base_function_call copy()
        {
            compiled_function_call cfc_copy = new compiled_function_call(this.function_node, this.obj, this.location);
            cfc_copy.parameters.AddRange(this.parameters);
            return cfc_copy;
        }
    }

    //Внимательно простмореть реализацию.
    //Обратить внимание на _compiled_type.
    [Serializable]
    public class compiled_static_method_call : base_function_call_with_method<compiled_function_node>, SemanticTree.ICompiledStaticMethodCallNode
    {
        private compiled_type_node _compiled_type;

        private type_node_list _template_parametres_list = new type_node_list();

        public compiled_static_method_call(compiled_function_node static_method,location loc) :
            base(static_method,loc)
        {
            _compiled_type = static_method.cont_type;
        }

        //Тип, статический метод которого мы вызываем.
        public compiled_type_node compiled_type
        {
            get
            {
                return _compiled_type;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.compiled_static_method_call;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.ICompiledStaticMethodCallNode)this);
        }

        public type_node_list template_parametres_list
        {
            get
            {
                return _template_parametres_list;
            }
        }

        SemanticTree.ICompiledTypeNode SemanticTree.ICompiledStaticMethodCallNode.compiled_type
        {
            get
            {
                return _compiled_type;
            }
        }

        SemanticTree.ICompiledMethodNode SemanticTree.ICompiledStaticMethodCallNode.static_method
        {
            get
            {
                return function_node;
            }
        }

        public SemanticTree.ITypeNode[] template_parametres
        {
            get
            {
                return (_template_parametres_list.ToArray());
            }
        }

        public override base_function_call copy()
        {
            compiled_static_method_call csmc_copy = new compiled_static_method_call(this.function_node, this.location);
            csmc_copy.parameters.AddRange(this.parameters);
            return csmc_copy;
        }
    }

    [Serializable]
    public class compiled_constructor_call : base_function_call_with_method<compiled_constructor_node>, SemanticTree.ICompiledConstructorCall
    {
        public compiled_constructor_call(compiled_constructor_node _constructor,location loc) :
            base(_constructor,loc)
        {
        }

        public compiled_type_node compiled_type
        {
            get
            {
                return function_node.compiled_type;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.ICompiledConstructorCall)this);
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.compiled_constructor_call;
            }
        }

        SemanticTree.ICompiledTypeNode SemanticTree.ICompiledConstructorCall.compiled_type
        {
            get
            {
                return function_node.comprehensive_type;
            }
        }

        SemanticTree.ICompiledConstructorNode SemanticTree.ICompiledConstructorCall.constructor
        {
            get
            {
                return function_node;
            }
        }
        //ssyy
        //Станет ложью, если это вызов конструктора предка
        public bool _new_obj_awaited = true;

        public bool new_obj_awaited()
        {
            return _new_obj_awaited;
        }
        //\ssyy
    }

}
