// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

namespace PascalABCCompiler.SemanticTree
{

	//Вид объекта.
	//basic-базовый объект, не определяемый в программе, например метод сложения двух целых чисел.
	//common-обычный тип, метод и т.д., определяемый пользователем.
	//compiled-тип, метод или другой узел, определяемый пользователем.
	public enum node_kind {basic,common,compiled,indefinite};

	//Уровень доступа класса. Хотя зачем я это пишу? И так понятно из названия.
	public enum type_access_level {tal_public,tal_internal};

	//Как мы будем представлять ссылочные и размерные типы?
	//public enum reference_or_value_type {reference_type,value_type};

	//Расположение элемента - в функции, в классе, в пространстве имен.
	public enum node_location_kind {in_function_location,in_class_location,in_namespace_location, in_block_location, indefinite};

	//Уровень доступа к элементам класса.
    public enum field_access_level { fal_private, fal_internal, fal_protected, fal_public };
	
	//Обычный, статический или виртуальный элемент класса.
	public enum polymorphic_state {ps_static,ps_common,ps_virtual,ps_virtual_abstract};

    public enum type_special_kind { none_kind, not_set_kind, array_kind, enum_kind, typed_file, binary_file, short_string, array_wrapper, record, set_type, base_set_type, diap_type, text_file };
	
    public enum attribute_qualifier_kind {none_kind, return_kind, assembly_kind, param_kind, type_kind, field_kind, event_kind, property_kind, method_kind}
    //Тип передачи параметра - по ссылке или по значению.
    public enum parameter_type { value, var, cnst };

    //Тип базовой функции.
    public enum basic_function_type
    {
        none,
        iadd, isub, imul, idiv, imod, igr, ism, igreq, ismeq, ieq, inoteq, ishl, ishr, ior, inot, ixor, iand, iunmin, iinc, idec, isinc, isdec, iassign,  //signed integer (4 byte)
        uiadd, uisub, uimul, uidiv, uimod, uigr, uism, uigreq, uismeq, uieq, uinoteq, uishl, uishr, uior, uinot, uixor, uiand, uiunmin, uiinc, uidec, uisinc, uisdec, uiassign, //unsigned integer (4 byte)
        badd, bsub, bmul, bdiv, bmod, bgr, bsm, bgreq, bsmeq, beq, bnoteq, bshl, bshr, bor, bnot, bxor, band, bunmin, binc, bdec, bsinc, bsdec, bassign, //unsigned byte
        sbadd, sbsub, sbmul, sbdiv, sbmod, sbgr, sbsm, sbgreq, sbsmeq, sbeq, sbnoteq, sbshl, sbshr, sbor, sbnot, sbxor, sband, sbunmin, sbinc, sbdec, sbsinc, sbsdec, sbassign, //signed byte
        sadd, ssub, smul, sdiv, smod, sgr, ssm, sgreq, ssmeq, seq, snoteq, sshl, sshr, sor, snot, sxor, sand, sunmin, sinc, sdec, ssinc, ssdec, sassign, //short (2-byte)
        usadd, ussub, usmul, usdiv, usmod, usgr, ussm, usgreq, ussmeq, useq, usnoteq, usshl, usshr, usor, usnot, usxor, usand, usunmin, usinc, usdec, ussinc, ussdec, usassign, //unsigned short (2 byte)
        ladd, lsub, lmul, ldiv, lmod, lgr, lsm, lgreq, lsmeq, leq, lnoteq, lshl, lshr, lor, lnot, lxor, land, lunmin, linc, ldec, lsinc, lsdec, lassign, //long (8 byte)
        uladd, ulsub, ulmul, uldiv, ulmod, ulgr, ulsm, ulgreq, ulsmeq, uleq, ulnoteq, ulshl, ulshr, ulor, ulnot, ulxor, uland, ulunmin, ulinc, uldec, ulsinc, ulsdec, ulassign, //unsigned long (8 byte)
        fadd, fsub, fmul, fdiv, fgr, fsm, fgreq, fsmeq, feq, fnoteq, funmin, fassign,//float
        dadd, dsub, dmul, ddiv, dgr, dsm, dgreq, dsmeq, deq, dnoteq, dunmin, dassign,//double
        boolgr, boolsm, boolgreq, boolsmeq, booleq, boolnoteq, boolor, boolnot, boolxor, booland, boolassign,  //boolean
        chargr, charsm, chargreq, charsmeq, chareq, charnoteq, cinc, cdec, csinc, csdec, charassign,  //char
        chartous, chartoi, chartoui, chartol, chartoul, chartof, chartod, chartob, chartosb, chartos,
        btos, btous, btoi, btoui, btol, btoul, btof, btod, btosb, btochar, //byte to ...
        sbtos, sbtoi, sbtol, sbtof, sbtod, sbtob, sbtous, sbtoui, sbtoul, sbtochar,//signed byte to short, int, long, float, double
        stoi, stol, stof, stod, stob, stosb, stous, stoui, stoul, stochar, //short to ...
        ustoi, ustoui, ustol, ustoul, ustof, ustod, ustob, ustosb, ustos, ustochar,  //unsigned short to ...
        itol, itof, itod, itob, itosb, itos, itous, itoui, itoul, itochar, itobool,    //integer to ...
        uitol, uitoul, uitob, uitosb, uitos, uitous, uitoi, uitof, uitod, uitochar,    //uint to ...
        ltof, ltod, ltob, ltosb, ltos, ltous, ltoi, ltoui, ltoul, ltochar,     //long to ...
        ultob, ultosb, ultos, ultous, ultoi, ultoui, ultol, ultochar, ultof, ultod, //ulong to ...
        ftod, ftob, ftosb, ftos, ftous, ftoi, ftoui, ftol, ftoul, ftochar, //float to ...
        dtob, dtosb, dtos, dtous, dtoi, dtoui, dtol, dtoul, dtof, dtochar,
        objassign, objeq, objnoteq, //присваивание и эквивалентность объектов по ссылке.
                                    //write,writei,writed,writec,writeb,read,readi,readd,readc,readb,expd,absd,absi //temporary functions (Нужны только на начальном этапе отладки. Потом обязательно удалить.)
        objtoobj, boolinc, booldec, boolsinc, boolsdec, booltoi, enumgr, enumgreq, enumsm, enumsmeq,
        booltob, booltosb, booltos, booltous, booltoui, booltol, booltoul,
        ltop, ptol, enumsand, enumsor, enumsxor
    };

    public enum runtime_statement_type { invoke_delegate, ctor_delegate, begin_invoke_delegate, end_invoke_delegate };

    public enum generic_parameter_kind { gpk_none, gpk_class, gpk_value };

	//Документ в котором описан этот узел.
	public interface IDocument
	{
		//Полный путь к файлу.
		string file_name
		{
			get;
		}
	}

	//Документ и позиция в которой описан этот узел.
    public interface ILocation
	{
		//Строка, в которой распологается начало данного элемента.
		int begin_line_num
		{
			get;
		}

		//Столбец, в которой распологается начало данного элемента.
		int begin_column_num
		{
			get;
		}

		//Строка, в которой распологается конец данного элемента.
		int end_line_num
		{
			get;
		}

		//Столбец, в которой распологается конец данного элемента.
		int end_column_num
		{
			get;
		}

		//Документ, в котором определен данный элемент дерева.
		string file_name
		{
			get;
		}
	}

	public interface ILocated
	{
		ILocation Location
		{
			 get;
		}
	}

	//Базовый интерфейс для всех интерфейсов узлов дерева.
	public interface ISemanticNode
	{
		void visit(ISemanticVisitor visitor);
	}

	//Базовый класс, для пердставления определений в программе (определений типов, переменных и т.д.). Никогда не создается.
	public interface IDefinitionNode : ISemanticNode
	{
		string Documentation
		{
			get;
		}
		IAttributeNode[] Attributes
		{
			get;
		}
	}

	//Базовый интерфейс для классов, которые описывают типы. Никогда не создается.
	public interface ITypeNode : IDefinitionNode
	{
		//Вид узла - базовый(basic), обычный (common) или экспортируемый (compiled).
		node_kind node_kind
		{
			get;
		}

		//Имя типа. Для языков не чувствительных к регистрам хранит имя в том виде, в каком тип объявлен.
		string name
		{
			get;
		}

		//Базовый тип для данного типа. Для object-а =null.
		ITypeNode base_type
		{
			get;
		}

        bool is_value_type
        {
            get;
        }
        
        bool is_nullable_type
        {
        	get;
        }
        
        type_special_kind type_special_kind
        {
            get;
        }

        ITypeNode element_type
        {
            get;
        }

        //ssyy
        bool is_class
        {
            get;
        }

        bool IsInterface
        {
            get;
            //set;
        }
		
        bool IsAbstract
        {
        	get;
        }
        
        bool IsEnum
        {
        	get;
        }
        
        bool IsDelegate
        {
        	get;
        }
        
        List<ITypeNode> ImplementingInterfaces
        {
            get;
        }
		List<ITypeNode> ImplementingInterfacesOrEmpty
        {
            get;
        }

        //Является ли generic-параметром
        bool is_generic_parameter
        {
            get;
        }

        bool is_generic_type_definition
        {
            get;
        }

        bool is_generic_type_instance
        {
            get;
        }

        //Зависит ли от некоторого неопределенного типа
        bool depended_from_indefinite
        {
            get;
        }

        //Описание generic-типа, содержащее данный параметр
        ICommonTypeNode generic_type_container
        {
            get;
        }

        ICommonFunctionNode common_generic_function_container
        {
            get;
        }
        //\ssyy
	}

	//Интерфейс для описания базовых типов.
	//При генерации .Net кода этот класс не нужен. Он может пригодится, например для генерации машинного кода.
	//Там он может быть использован, например для представления целых чисел.
	//Хотя возможно в этом случае удобнее использовать compiled_type_node.
	//Для него могут быть не определены свойства name и base_type.
	//В общем когда понадобится, тогда и будем думать. Пока этот интерфейс нигде не должен использоваться.
	public interface IBasicTypeNode : ITypeNode
	{

	}

    public interface IUnsizedArray : ITypeNode
    {
        ITypeNode element_type
        {
            get;
        }
    }

    //Синонимы типов, определяемые пользователем в программе.
    public interface ITypeSynonym : IDefinitionNode, ILocated
    {
        //Имя типа. Для языков не чувствительных к регистрам хранит имя в том виде, в каком тип объявлен.
        string name
        {
            get;
        }

        //Тип, которому даётся синоним
        ITypeNode original_type
        {
            get;
        }

    }

    public interface ITemplateClass : IDefinitionNode
    {
        byte[] serialized_tree
        {
            get;
        }

        string name
        {
            get;
        }
    }

    //Интерфейс для generic-типов
    public interface IGenericInstance
    {
        List<ITypeNode> generic_parameters
        {
            get;
        }
    }

    //Интерфейс для generic-инстанций
    public interface IGenericTypeInstance: IGenericInstance, ICommonTypeNode
    {
        ITypeNode original_generic
        {
            get;
        }

        System.Collections.Hashtable used_members
        {
            get;
        }
    }

    public interface ICommonGenericTypeInstance : IGenericTypeInstance
    {
    }

    public interface ICompiledGenericTypeInstance : IGenericTypeInstance
    {
    }

    public interface IGenericFunctionInstance : IGenericInstance, ICommonFunctionNode
    {
        IFunctionNode original_function
        {
            get;
        }
    }

    public interface ICompiledGenericMethodInstance : IGenericFunctionInstance, ICommonMethodNode
    {
    }

    //Описывет обычные типы, определяемые пользователем в программе.
	public interface ICommonTypeNode : ITypeNode, INamespaceMemberNode, ILocated
	{
        bool IsSealed
        {
            get;
        }
		//Тип public или internal.
		type_access_level type_access_level
		{
			get;
		}

		//Методы типа.
		ICommonMethodNode[] methods
		{
			get;
		}

		//Поля типа.
		ICommonClassFieldNode[] fields
		{
			get;
		}

		//Свойства типа.
		ICommonPropertyNode[] properties
		{
			get;
		}

        //Константы, определенные в типе.
        IClassConstantDefinitionNode[] constants
        {
            get;
        }
		
        ICommonEventNode[] events
        {
        	get;
        }
        
        IPropertyNode default_property
        {
            get;
        }
		
        IConstantNode lower_value
        {
        	get;
        }
        
        IConstantNode upper_value
        {
        	get;
        }
        
        ICommonMethodNode static_constructor
        {
        	get;
        }
        
        int rank
        {
        	get;
        }
        
        //(ssyy) Является ли описанием generic-типа
        bool is_generic_type_definition
        {
            get;
        }

        List<ICommonTypeNode> generic_params
        {
            get;
        }

        ICommonClassFieldNode runtime_initialization_marker
        {
            get;
        }

        bool has_static_constructor
        {
            get;
        }
	}

    //Интерфейс для описания типов, экспортируемых из сборки.
	//Для представления откомпилированных типов я обращаюсь непосредственно к System.Reflection.
	//Это завязывает нашу систему на .Net. Можно избавится от этого с помощью введения дополнительного уровня абстракции.
	//Для этого нужно определить, какие данные нам нужны от откомпилированных типов, и сделать еще одну прослойку
	//между System.Reflection и нашими типами. Тогда при генерации кода под другую платформу (например машинного кода)
	//нужно будет только привязать прослойку к другому источнику данных. Например, информация о типах будет
	//браться не из сборок, а из dll-библиотек. Я не могу сейчас сделать эту прослойку, т.к. я пока не определил
	//какие запросы нужны к откомпилированным модулям. Поэтому пока я использую System.Reflection в котором
	//вроде все есть. Потом нужно будет обсудить вопрос создания этой прослойки.
	public interface ICompiledTypeNode : ITypeNode
	{
		//Откомпилированный тип.
		System.Type compiled_type
		{
			get;
		}
		
		int rank
		{
			get;
		}
	}

	//Интерфейс, представляющий индексируемый с 0 массив.
	public interface IRefTypeNode : ITypeNode
	{
		ITypeNode pointed_type
		{
			get;
		}
	}
	
	public interface IShortStringTypeNode : ITypeNode
	{
		int Length
		{
			get;
		}
	}
	
	public interface ISimpleArrayNode : ITypeNode
	{
		int length
		{
			get;
		}
		
		ITypeNode element_type
		{
			get;
		}
	}
	
	public interface ISimpleArrayIndexingNode : IAddressedExpressionNode
	{
		IExpressionNode array
		{
			get;
		}
		
		IExpressionNode[] indices
		{
			get;
		}
		
		IExpressionNode index
		{
			get;
		}
	}

	//Базовый интерфейс для statement-ов. Объекты, реализующие только этот интерфейс нигде не должны создаваться.
	//Должны создаваться классы, реализующие интерфейсы производные от данного интерфейса.
	//Звучит очень запутано, но вобщем то, что написано выше не очень важно :-).
	public interface IStatementNode : ISemanticNode, ILocated
	{

	}

    public interface IRuntimeManagedMethodBody : IStatementNode
    {
        runtime_statement_type runtime_statement_type
        {
            get;
        }
    }

	//Базовый интерфейс для выражений.
	public interface IExpressionNode : IStatementNode
	{
		//Тип выражения.
		ITypeNode type
		{
			get;
		}
		
		ITypeNode conversion_type
		{
			get;
		}
	}

	//Базовый интерфейс для вызовов функций. Нигде не создается.
	public interface IFunctionCallNode : IExpressionNode
	{
		//Список фактических параметров. Количество и типы формальных и фактических параметров сверяется
		//на этапе построения семантического дерева. При необходимости при построении семантического дерева
		//вставляются узлы преобрызования типов.
		IExpressionNode[] real_parameters
		{
			get;
		}

		//Вызываемый метод.
		IFunctionNode function
		{
			get;
		}

        //ssyy
        //Нужно для генерации унаследованных интерфейсных функций
        bool last_result_function_call
        {
            get;
            set;
        }
        //\ssyy

	}

	//Вызов базового метода.
	public interface IBasicFunctionCallNode : IFunctionCallNode
	{
		//Вызываемый метод.
		IBasicFunctionNode basic_function
		{
			get;
		}
	}

    public interface INonStaticMethodCallNode : IFunctionCallNode
    {
        bool virtual_call
        {
            get;
            set;
        }

    }

	//Вызов функции, определенной в пространстве имен.
	public interface ICommonNamespaceFunctionCallNode : IFunctionCallNode
	{
		//Вызываемый метод.
		ICommonNamespaceFunctionNode namespace_function
		{
			get;
		}
	}

	//Вызов функции, определенной в другой функции.
	public interface ICommonNestedInFunctionFunctionCallNode : IFunctionCallNode
	{
		//Вызываемый метод.
		ICommonNestedInFunctionFunctionNode common_function
		{
			get;
		}

		//Статическая глубина вложенной функции.
		int static_depth
		{
			get;
		}
	}

	//Вызов метода класса.
    public interface ICommonMethodCallNode : INonStaticMethodCallNode
	{
		//Вызываемый метод.
		ICommonMethodNode method
		{
			get;
		}

		//Экземпляр класса, данный метод которого нужно вызвать.
		IExpressionNode obj
		{
			get;
		}
	}

	//Узел, соответствующий указателю this в программе.
	public interface IThisNode : IExpressionNode
	{
	}

    public interface IAsNode : IExpressionNode
    {
        IExpressionNode left
        {
            get;
        }

        ITypeNode right
        {
            get;
        }
    }

    public interface IIsNode : IExpressionNode
    {
        IExpressionNode left
        {
            get;
        }

        ITypeNode right
        {
            get;
        }
    }
    
    public interface ISizeOfOperator : IExpressionNode
    {
        ITypeNode oftype
        {
            get;
        }
    }

    public interface ITypeOfOperator : IExpressionNode
    {
        ITypeNode oftype
        {
            get;
        }
    }

	//Вызов статичекого метода класса.
    public interface ICommonStaticMethodCallNode : IFunctionCallNode
	{
		//Вызываемый метод.
		ICommonMethodNode static_method
		{
			get;
		}

		//Тип, статический метод которого вызываем.
		ICommonTypeNode common_type
		{
			get;
		}
	}

	//Вызов конструктора common-класса.
	public interface ICommonConstructorCall : ICommonStaticMethodCallNode
	{
        //ssyy
        bool new_obj_awaited();
        //\ssyy
	}

	//Вызов откомпилированного метода.
    public interface ICompiledMethodCallNode : INonStaticMethodCallNode
	{
		//Вызываемый метод.
		ICompiledMethodNode compiled_method
		{
			get;
		}

		//Экземпляр класса, данный метод которого нужно вызвать.
		IExpressionNode obj
		{
			get;
		}

    }

	//Вызов статического метода, откомптлированного класса.
    public interface ICompiledStaticMethodCallNode : IFunctionCallNode
	{
		//Вызываемый метод.
		ICompiledMethodNode static_method
		{
			get;
		}

		//Тип, статический метод которого мы вызываем.
		ICompiledTypeNode compiled_type
		{
			get;
		}

        ITypeNode[] template_parametres
        {
            get;
        }
	}

	//Вызов конструктора откомпилированного метода.
	public interface ICompiledConstructorCall : IFunctionCallNode
	{
		ICompiledConstructorNode constructor
		{
			get;
		}

		ICompiledTypeNode compiled_type
		{
			get;
		}

        //ssyy
        bool new_obj_awaited();
        //\ssyy
	}

	//Базовый интерфейс для описания функций. Никогда не создается.
	public interface IFunctionNode : IDefinitionNode
	{
		//Вид узла - базовый (basic), обычный (common) или экспортируемый (compiled).
		node_kind node_kind
		{
			get;
		}

		//Список формальных параметров функции.
		IParameterNode[] parameters
		{
			get;
		}

		//Тип возвращаемого значения функции.
		ITypeNode return_value_type
		{
			get;
		}

		//Имя функции. Для языков не чувствительных к регистру - в том виде, в котором функция определена.
		string name
		{
			get;
		}

		//Расположение функции - в функции, в классе, в пространстве имен.
		node_location_kind node_location_kind
		{
			get;
		}

        //Является ли generic-функцией
        bool is_generic_function
        {
            get;
        }

        //Число типов-параметров generic-функции. 0 для не-generic.
        int generic_parameters_count
        {
            get;
        }
    }

	//Интерфейс члена класса.
	public interface IClassMemberNode
	{
		//Тип, содержащий этот член класса.
		ITypeNode comperehensive_type
		{
			get;
		}

		//Статический, обычный или виртуальный метод.
		polymorphic_state polymorphic_state
		{
			get;
		}

		//Уровень доступа к члену класса.
		field_access_level field_access_level
		{
			get;
		}
	}

	//Интерфейс члена откомпилированного класса.
	public interface ICompiledClassMemberNode : IClassMemberNode
	{
		//Тип, содержащий член класса.
		ICompiledTypeNode comprehensive_type
		{
			get;
		}
	}

	//Интерфейс члена обычного класса.
	public interface ICommonClassMemberNode : IClassMemberNode
	{
		//Тип, содержащий член класса.
		ICommonTypeNode common_comprehensive_type
		{
			get;
		}
	}

	//Интерфейс переменной или функции, определенной внутри функции.
	public interface IFunctionMemberNode
	{
		//Функция, содержащая этот обьъект.
		ICommonFunctionNode function
		{
			get;
		}
	}

	//Интерфейс переменной или функции, определенной в пространстве имен.
	public interface INamespaceMemberNode
	{
		//Пространство имен, в котором определен элемент.
		ICommonNamespaceNode comprehensive_namespace
		{
			get;
		}
	}

	//Класс для описания базовых, нигде не определенных функций (например, сложение двух целых чисел).
	public interface IBasicFunctionNode : IFunctionNode
	{
		//Какая именно это базовая функция.
		basic_function_type basic_function_type
		{
			get;
		}
	}

    public enum SpecialFunctionKind
    {
        None, New, Dispose, NewArray
    }

	//Интерфейс для описания функции, определяемой пользователем.
	public interface ICommonFunctionNode : IFunctionNode, ILocated
	{
        SpecialFunctionKind SpecialFunctionKind
        {
            get;
        }
		
        bool is_overload
        {
        	get;
        }
        
		//Список переменных, определяемых в функции.
		ILocalVariableNode[] var_definition_nodes
		{
			get;
		}

		//Список вложенных функций.
		ICommonNestedInFunctionFunctionNode[] functions_nodes
		{
			get;
		}

		//Код функции.
		IStatementNode function_code
		{
			get;
		}

		//Переменная, которая содержит возвращаемое значение функции. Для процедур - null.
		ILocalVariableNode return_variable
		{
			get;
		}

        //Константы, определенные в функции.
        ICommonFunctionConstantDefinitionNode[] constants
        {
            get;
        }

        //Generic-параметры функции
        List<ICommonTypeNode> generic_params
        {
            get;
        }
    }

	//Функция, определенная непосредственно в пространстве имен.
	public interface ICommonNamespaceFunctionNode : ICommonFunctionNode, INamespaceMemberNode
	{
		//Прстранство имен, в котором определена эта функция.
		ICommonNamespaceNode namespace_node
		{
			get;
		}

        ITypeNode ConnectedToType
        { 
            get;  
        }
	}

	//Функция, определенная в другой функции.
	public interface ICommonNestedInFunctionFunctionNode : ICommonFunctionNode, IFunctionMemberNode
	{
	
	}

	//Метод класса, определяемый пользователем.
	public interface ICommonMethodNode : ICommonFunctionNode, ICommonClassMemberNode
	{
		bool is_constructor
		{
			get;
		}

        IFunctionNode overrided_method
        {
            get;
        }

        bool is_final
        {
            get;
            set;
        }

        bool newslot_awaited
        {
            get;
            set;
        }

    }

	//Класс для описания экспортируемых (compiled) функций.
	public interface ICompiledMethodNode : IFunctionNode, ICompiledClassMemberNode
	{
		//Откомпилированный метод.
		System.Reflection.MethodInfo method_info
		{
			get;
		}

        bool is_extension
        {
            get;
        }
	}

	//Вызов конструктора откомпилированного типа.
	public interface ICompiledConstructorNode : IFunctionNode, ICompiledClassMemberNode
	{
		System.Reflection.ConstructorInfo constructor_info
		{
			get;
		}
	}

	//Интерфейс для описания конструкции if.
	public interface IIfNode : IStatementNode
	{
		//Условие.
		IExpressionNode condition
		{
			get;
		}

		//Тело then.
		IStatementNode then_body
		{
			get;
		}

		//Тело else. Если if без then это свойство = null.
		IStatementNode else_body
		{
			get;
		}
	}

	//Интерфейс для конструкции while.
	public interface IWhileNode : IStatementNode
	{
		//Условие.
		IExpressionNode condition
		{
			get;
		}

		//Тело while.
		IStatementNode body
		{
			get;
		}
	}

	//Интерфейс для конструкции repeat.
	public interface IRepeatNode : IStatementNode
	{
		//Тело do .. while (repeat .. until).
		IStatementNode body
		{
			get;
		}

		//Условие.
		IExpressionNode condition
		{
			get;
		}
	}

	//Класс для описания конструкции for. 
	//For - C++/C# - овский. Для моделирования паскалевского for он преобразуется в эту конструкцию.
	public interface IForNode : IStatementNode
	{
		//Инициализация переменных цикла.
		IStatementNode initialization_statement
		{
			get;
		}

		//Условие продолжения цикла.
		IExpressionNode while_expr
		{
			get;
		}
		
		IExpressionNode init_while_expr
		{
			get;
		}
		
		//Изменение счетчиков цикла.
		IStatementNode increment_statement
		{
			get;
		}

		//Тело цикла.
		IStatementNode body
		{
			get;
		}
		
		bool IsBoolCycle
		{
			get;
		}
	}



	public interface IWhileBreakNode : IStatementNode
	{
		IWhileNode while_node
		{
			get;
		}
	}

	public interface IRepeatBreakNode : IStatementNode
	{
		IRepeatNode repeat_node
		{
			get;
		}
	}

	public interface IForBreakNode : IStatementNode
	{
		IForNode for_node
		{
			get;
		}
	}
	
	public interface IForeachBreakNode : IStatementNode
	{
		IForeachNode foreach_node
		{
			get;
		}
	}
	
	
    public interface IExitProcedure : IStatementNode
    {
    }


	public interface IWhileContinueNode : IStatementNode
	{
		IWhileNode while_node
		{
			get;
		}
	}

	public interface IRepeatContinueNode : IStatementNode
	{
		IRepeatNode repeat_node
		{
			get;
		}
	}

	public interface IForContinueNode : IStatementNode
	{
		IForNode for_node
		{
			get;
		}
	}
	
	public interface IForeachContinueNode : IStatementNode
	{
		IForeachNode foreach_node
		{
			get;
		}
	}
	
	public interface IExternalStatementNode : IStatementNode
	{
		string module_name
		{
			get;
		}
		
		string name
		{
			get;
		}
	}
	
	public interface IPInvokeStatementNode : IStatementNode
	{
		
	}
	
    public interface ISwitchNode : IStatementNode
    {
        IExpressionNode case_expression
        {
            get;
        }

        ICaseVariantNode[] case_variants
        {
            get;
        }

        IStatementNode default_statement
        {
            get;
        }
    }

    public interface ICaseVariantNode : IStatementNode
    {
        IIntConstantNode[] elements
        {
            get;
        }

        ICaseRangeNode[] ranges
        {
            get;
        }

        IStatementNode statement_to_execute
        {
            get;
        }
    }

    public interface ICaseRangeNode : IStatementNode
    {
        IIntConstantNode lower_bound
        {
            get;
        }

        IIntConstantNode high_bound
        {
            get;
        }
    }
	
	/*//    switch.
	public interface ISwitchNode : IStatementNode
	{
	}

	//Один из вариантов констракции swithc.
	public interface ICaseVariantNode : IStatementNode
	{
		//Выражения, при которых нужно исполгить код, соответствующий этому узлу.
		IExpressionNode[] expressions
		{
			get;
		}

		//Диапазоны, при попадании в которые нужно исполнить код, соответствующий этому узлу.
		IRangExpression[] ranges
		{
			get;
		}

		//Код этого узла.
		IStatementNode statement
		{
			get;
		}
	}

	public interface IRangeExpression : IExpressionNode
	{
		IExpressionNode lower_bound
		{
			get;
		}

		IExpressionNode upper_bound
		{
			get;
		}
	}*/

	//Интерфейс для описания списка statement-ов.
	public interface IStatementsListNode : IStatementNode
	{
        ILocalBlockVariableNode[] LocalVariables
        {
            get;
        }
		//Список statement-ов.
		IStatementNode[] statements
		{
			get;
		}
        //Положение левой логической скобки
        ILocation LeftLogicalBracketLocation
        {
			get;
		}
        //Положение правой логической скобки
        ILocation RightLogicalBracketLocation
        {
			get;
		}
        
	}

    public interface IThrowNode : IStatementNode
    {
        IExpressionNode exception_expresion
        {
            get;
        }
    }

    public interface ITryBlockNode : IStatementNode
    {
        IStatementNode TryStatements
        {
            get;
        }

        IStatementNode FinallyStatements
        {
            get;
        }

        IExceptionFilterBlockNode[] ExceptionFilters
        {
            get;
        }
    }

    public interface IExceptionFilterBlockNode : IStatementNode
    {
        ITypeNode ExceptionType
        {
            get;
        }

        ILocalBlockVariableReferenceNode ExceptionInstance
        {
            get;
        }

        IStatementNode ExceptionHandler
        {
            get;
        }
    }

    /*
    public interface ICatchNode : IStatementNode
    {
        ISemanticNode[] catch_body
        {
            get;
        }
    }

    public interface ITryStatementNode : IStatementNode
    {
        IStatementNode[] try_body
        {
            get;
        }

        
    }
    */
     
	//Узел пространства имен.
	public interface INamespaceNode : IDefinitionNode
	{
		string namespace_name
		{
			get;
		}
	}
	
	//Узел пространства имен, определенного пользователем.
	public interface ICommonNamespaceNode : INamespaceNode, ILocated
	{
		//Пространства имен, вложенные в это пространство имен.
		ICommonNamespaceNode[] nested_namespaces
		{
			get;
		}

		//Пространство имен, в которое вложенно это пространство имен.
		INamespaceNode comprehensive_namespace
		{
			get;
		}

		//Типы, описанные в namespace.
		ICommonTypeNode[] types
		{
			get;
		}
		
		ITypeSynonym[] type_synonims
		{
			get;
		}

        ITemplateClass[] templates
        {
            get;
        }

		//Переменные, описанные в этом namespace.
		ICommonNamespaceVariableNode[] variables
		{
			get;
		}

		//Функции, описанные в namespace.
		ICommonNamespaceFunctionNode[] functions
		{
			get;
		}

		//Константы, описанные в namespace. Они должны экспортироваться как нибудь.
		INamespaceConstantDefinitionNode[] constants
		{
			get;
		}

        ICommonNamespaceEventNode[] events
        {
            get;
        }
		
		bool IsMain
		{
			get;
		}
	}

	//Откомпилированное пространство имен.
	public interface ICompiledNamespaceNode : INamespaceNode
	{
		
	}

    /// Базовый интерфейс для программ и dll.
    public interface IProgramBase : IDefinitionNode, ILocated
    {
        //Пространства имен, содержащиеся в программе или dll.
        ICommonNamespaceNode[] namespaces
        {
            get;
        }
        
        string[] UsedNamespaces
        {
        	get;
        }
    }

	//Узел dll библиотеки.
	public interface IDllNode : IProgramBase
	{
		//Метод инициализации dll.
		ICommonNamespaceFunctionNode initialization_function
		{
			get;
		}

		//Метод финализации dll.
		ICommonNamespaceFunctionNode finalization_function
		{
			get;
		}
	}

	//Корневой узел программы.
	public interface IProgramNode : IProgramBase
	{
		//Главная функция. Ее выполнение равносильно выполнению программы.
		//Она включает вызовы методов инициализации модулей (в начале), выполнение основной программы
		//и вызовы методов финализации модулей.
		ICommonNamespaceFunctionNode main_function
		{
			get;
		}

        //Инстанции generic-типов, использующиеся в программе.
        List<IGenericTypeInstance> generic_type_instances
        {
            get;
        }

        //Инстанции generic-типов, использующиеся в программе.
        List<IGenericFunctionInstance> generic_function_instances
        {
            get;
        }
        
        IStatementNode InitializationCode
        {
        	get;
        }
    }

	//Тип выражений, которые могут возвращать адрес (например переменная).
	public interface IAddressedExpressionNode : IExpressionNode
	{
		
	}

	//Оператор return.
	public interface IReturnNode : IStatementNode
	{
		IExpressionNode return_value
		{
			get;
		}
	}

    //ssyy добавил
    //Оператор return из .ctor.
    /*public interface ICtorReturnNode : IStatementNode
    {
    }*/
    //\ssyy

    public interface IReferenceNode : IAddressedExpressionNode
    {
        //Определение локальной переменной.
        IVAriableDefinitionNode Variable
        {
            get;
        }
    }

    //Интерфейс, представляющий обращение к локальной переменной в теле программы.
    public interface ILocalVariableReferenceNode : IReferenceNode
	{
		//Определение локальной переменной.
		ILocalVariableNode variable
		{
			get;
		}

		//Разность статических глубин, между определением и вхождением.
		int static_depth
		{
			get;
		}
	}

    //Интерфейс, представляющий обращение к локальной переменной в блоке.
    public interface ILocalBlockVariableReferenceNode : IReferenceNode
    {
        //Определение локальной переменной.
        ILocalBlockVariableNode Variable
        {
            get;
        }
    }
    
    //Интерфейс, представляющий обращение к переменной, определенной непосредственно в namespace.
    public interface INamespaceVariableReferenceNode : IReferenceNode
	{
		//Переменная.
		ICommonNamespaceVariableNode variable
		{
			get;
		}
	}

	//Интерфейс, представляющий обращение к полю класса.
    public interface ICommonClassFieldReferenceNode : IReferenceNode
	{
		//Поле класса.
		ICommonClassFieldNode field
		{
			get;
		}

		//Объект класса.
		IExpressionNode obj
		{
			get;
		}
	}

	//Интерфейс, представляющий обращение к статическому полю класса.
    public interface IStaticCommonClassFieldReferenceNode : IReferenceNode
	{
		//Статическое поле класса.
		ICommonClassFieldNode static_field
		{
			get;
		}

		//Класс, к статическому методу которого мы обращаемся.
		ICommonTypeNode class_type
		{
			get;
		}
	}

	//Обращение к полю откомпилированного класса.
    public interface ICompiledFieldReferenceNode : IReferenceNode
	{
		//Поле класса.
		ICompiledClassFieldNode field
		{
			get;
		}

		//Объект класса.
		IExpressionNode obj
		{
			get;
		}
	}

	//Интерфейс, представляющий обращение к статическому полю откомпилированного класса.
    public interface IStaticCompiledFieldReferenceNode : IReferenceNode
	{
		//Поле класса.
		ICompiledClassFieldNode static_field
		{
			get;
		}

		//Класс, к статическому полю которого мы обращаемся.
		ICompiledTypeNode class_type
		{
			get;
		}
	}

	//Обращение к параметру метода.
    public interface ICommonParameterReferenceNode : IReferenceNode
	{
		//Параметр метода, к которому мы обращаемся.
		ICommonParameterNode parameter
		{
			get;
		}

		//Разность статических глубин, между обращением к параметру и методом в котором он объявлен.
		int static_depth
		{
			get;
		}
	}

	//Базовый узел для представления констант в теле программы (не именованных констант, а чисел, строк и т.д.).
	public interface IConstantNode : IExpressionNode
	{
        object value
        {
            get;
        }
	}

	//Интерфейс для представления булевских констант.
	public interface IBoolConstantNode : IConstantNode
	{
		//Значение константы.
		bool constant_value
		{
			get;
		}
	}

    //Интерфейс для представления byte констант.
    public interface IByteConstantNode : IConstantNode
    {
        //Значение константы.
        byte constant_value
        {
            get;
        }
    }

    //Интерфейс для представления signed byte констант.
    public interface ISByteConstantNode : IConstantNode
    {
        //Значение константы.
        sbyte constant_value
        {
            get;
        }
    }

    //Интерфейс для представления signed short констант.
    public interface IShortConstantNode : IConstantNode
    {
        //Значение константы.
        short constant_value
        {
            get;
        }
    }

    //Интерфейс для представления unsigned short констант.
    public interface IUShortConstantNode : IConstantNode
    {
        //Значение константы.
        ushort constant_value
        {
            get;
        }
    }

    //Интерфейс для представления int констант.
    public interface IIntConstantNode : IConstantNode
    {
        //Значение константы.
        int constant_value
        {
            get;
        }
    }

    //Интерфейс для представления BigInteger констант.
    public interface IBigIntConstantNode : IConstantNode
    {
        //Значение константы.
        System.Numerics.BigInteger constant_value
        {
            get;
        }
    }

    //Интерфейс для представления unsigned int констант.
    public interface IUIntConstantNode : IConstantNode
    {
        //Значение константы.
        uint constant_value
        {
            get;
        }
    }

    //Интерфейс для представления long констант.
    public interface ILongConstantNode : IConstantNode
    {
        //Значение константы.
        long constant_value
        {
            get;
        }
    }

    //Интерфейс для представления unsigned long констант.
    public interface IULongConstantNode : IConstantNode
    {
        //Значение константы.
        ulong constant_value
        {
            get;
        }
    }

    //Интерфейс для представления float констант.
    public interface IFloatConstantNode : IConstantNode
    {
        //Значение константы.
        float constant_value
        {
            get;
        }
    }

	//Интерфейс для представления double констант.
	public interface IDoubleConstantNode : IConstantNode
	{
		//Значение константы.
		double constant_value
		{
			get;
		}
	}

	//Интерфейс для представления char констант (этот класс для 2-байтных char - widechar в delphi).
	public interface ICharConstantNode : IConstantNode
	{
		//Значение константы.
		char constant_value
		{
			get;
		}
	}

	//Интерфейс для представления string-констант.
	public interface IStringConstantNode : IConstantNode
	{
		//Значение константы.
		string constant_value
		{
			get;
		}
	}

    public interface IEnumConstNode : IConstantNode
    {
        int constant_value
        {
            get;
        }
    }

    public interface IArrayConstantNode : IConstantNode
    {
        IConstantNode[] ElementValues
        {
            get;
        }
        
        ITypeNode ElementType
        {
            get;
        }
    }
    
    public interface IArrayInitializer : IExpressionNode
    {
    	IExpressionNode[] ElementValues
        {
            get;
        }
        
        ITypeNode ElementType
        {
            get;
        }
    }
    
    public interface IRecordConstantNode : IConstantNode
    {
        IConstantNode[] FieldValues
        {
            get;
        }
    }
	
    public interface IRecordInitializer : IExpressionNode
    {
    	IExpressionNode[] FieldValues
    	{
    		get;
    	}
    }
    
	public interface ICommonStaticMethodCallNodeAsConstant : IConstantNode
    {
		ICommonStaticMethodCallNode MethodCall
		{
			get;
		}
	}

	public interface ICompiledStaticMethodCallNodeAsConstant : IConstantNode
    {
        ICompiledStaticMethodCallNode MethodCall
        {
            get;
        }
    }

    public interface ICompiledStaticFieldReferenceNodeAsConstant : IConstantNode
    {
        IStaticCompiledFieldReferenceNode FieldReference
        {
            get;
        }
    }

    public interface ICommonNamespaceFunctionCallNodeAsConstant : IConstantNode
    {
        ICommonNamespaceFunctionCallNode MethodCall
        {
            get;
        }
    }
    
    public interface IBasicFunctionCallNodeAsConstant : IConstantNode
    {
    	IBasicFunctionCallNode MethodCall
    	{
    		get;
    	}
    }

    public interface IDefaultOperatorNodeAsConstant : IConstantNode
    {
        IDefaultOperatorNode DefaultOperator
        {
            get;
        }
    }

	public interface ITypeOfOperatorAsConstant : IConstantNode
	{
		ITypeOfOperator TypeOfOperator
		{
			get;
		}
	}

	public interface ISizeOfOperatorAsConstant : IConstantNode
    {
		ISizeOfOperator SizeOfOperator
        {
			get;
        }
    }

	public interface ICompiledConstructorCallAsConstant : IConstantNode
    {
        ICompiledConstructorCall MethodCall
        {
            get;
        }
    }
    /*public interface IClassConstantNode : IConstantNode
    {

    }*/

	/*//Узел для представления оператора присваивания.
	public interface IAssignNode : IExpressionNode
	{
		//Чему присваиванием.
		IAddressedExpressionNode to
		{
			get;
		}

		//Что присваиваем.
		IExpressionNode from
		{
			get;
		}
	}*/

	//Базовй интерфейс для формальных параметров функций, локальных переменных, глобальных переменных программы и модуля и полей класса. Нигде не создается.
	public interface IVAriableDefinitionNode : IDefinitionNode
	{
		//Имя переменной.
		string name
		{
			get;
		}

		//Тип переменной.
		ITypeNode type
		{
			get;
		}

        IExpressionNode inital_value
        {
            get;
        }

		//Раположение переменной.
		node_location_kind node_location_kind
		{
			get;
		}
	}

	//Интерфейс для описания локальных переменных.
	public interface ILocalVariableNode : IVAriableDefinitionNode, IFunctionMemberNode, ILocated
	{
		//Используется, ли переменная во вложенных функчиях. Исрользуется для оптимизации.
		bool is_used_as_unlocal
		{
			get;
		}
	}
    
    //Интерфейс для описания локальных переменных.
    public interface ILocalBlockVariableNode : IVAriableDefinitionNode, ILocated
    {
        IStatementsListNode Block
        {
            get;
        }
    }

	//Интерфейс, представляющий глобальную переменную, описанную в модуле или программе.
	public interface ICommonNamespaceVariableNode : IVAriableDefinitionNode, INamespaceMemberNode, ILocated
	{
		
	}

	//Интерфейс для описания полей класса.
	public interface ICommonClassFieldNode: IVAriableDefinitionNode, ICommonClassMemberNode, ILocated
	{

	}

	//Переменная, определенная в откомпилированном классе.
	public interface ICompiledClassFieldNode : IVAriableDefinitionNode, ICompiledClassMemberNode
	{
		System.Reflection.FieldInfo compiled_field
		{
			get;
		}
	}

	//Базовый интерфейс для интерфейсов, представляющих параметры базовых, обычных и откомпилированных функций.
	public interface IParameterNode : IVAriableDefinitionNode
	{
		//Тип параметра.
		parameter_type parameter_type
		{
			get;
		}

		//Функция, в которой описан этот праметр.
		IFunctionNode function
		{
			get;
		}

        bool is_params
        {
            get;
        }

        bool is_const
        {
            get;
        }

        IExpressionNode default_value
        {
            get;
        }
	}

	//Интерфейс для представления параметров common функций.
	public interface ICommonParameterNode : IParameterNode, ILocated
	{
		//Функция, в которой определен параметер.
		ICommonFunctionNode common_function
		{
			get;
		}

		//Используется ли параметр во вложенных функциях.
		bool is_used_as_unlocal
		{
			get;
		}
	}

	//Интерфейс, представляющий параметры базовых функций.
	public interface IBasicParameterNode : IParameterNode
	{
		
	}

	//Интерфейс, представляющий параметры откомпилироанных функций.
	public interface ICompiledParameterNode : IParameterNode
	{
		//Функция, в которой определен параметер.
		ICompiledMethodNode compiled_function
		{
			get;
		}
	}

	//Интерфейс, описывающий определение константы.
	public interface IConstantDefinitionNode : IDefinitionNode
	{
		//Имя константы.
		string name
		{
			get;
		}

		//Тип константы.
		ITypeNode type
		{
			get;
		}

		//Значение константы.
		IConstantNode constant_value
		{
			get;
		}
	}

    //Константа, определенная в классе.
    public interface IClassConstantDefinitionNode : IConstantDefinitionNode, IClassMemberNode, ILocated
    {

    }

    public interface ICompiledClassConstantDefinitionNode : IConstantDefinitionNode, IClassMemberNode
    {
        ICompiledTypeNode comprehensive_type
        {
            get;
        }
    }

    //Константа, определенная в пространстве имен.
    public interface INamespaceConstantDefinitionNode : IConstantDefinitionNode, ILocated
    {
        ICommonNamespaceNode comprehensive_namespace
        {
            get;
        }
    }

    //Константа, определенная в функции.
    public interface ICommonFunctionConstantDefinitionNode : IConstantDefinitionNode, ILocated
    {
        ICommonFunctionNode comprehensive_function
        {
            get;
        }
    }

    //Константа, определенная в откомпилированном типе.
    public interface ICompiledConstantNode : IConstantDefinitionNode
    {
        ICompiledTypeNode comprehensive_type
        {
            get;
        }
    }

	//Узел, описывающий свойство класса. Никогда не создается.
	public interface IPropertyNode : IDefinitionNode
	{
		//Вид объекта (basic, common, compiled).
		node_kind node_kind
		{
			get;
		}

		//Имя свойства.
		string name
		{
			get;
		}

		//Тип, который содержит это свойство.
		ITypeNode comprehensive_type
		{
			get;
		}

		//Тип свойства.
		ITypeNode property_type
		{
			get;
		}

		//Функция, которая возвращает значение свойства.
		IFunctionNode get_function
		{
			get;
		}

		//Функция, которая устанавливает значение свойства.
		IFunctionNode set_function
		{
			get;
		}

		IParameterNode[] parameters
		{
			get;
		}
	}

	//Определяемое пользователем свойство.
	public interface ICommonPropertyNode : IPropertyNode, ICommonClassMemberNode, ILocated
	{
		//Тип, который содержит это свойство.
		/*ICommonTypeNode common_comprehensive_type
		{
			get;
		}*/

		//Функция, которая возвращает значение свойства.
		/*ICommonClassMemberNode get_common_function
		{
			get;
		}

		//Функция, которая устанавливает значение свойства.
		ICommonClassMemberNode set_common_function
		{
			get;
		}*/
	}

	//Базовое свойство. Пока нигде не нужно, но для реализации машинного кода может очень пригодится.
	public interface IBasicPropertyNode : IPropertyNode
	{

	}

	//Свойство в откомпилированном типе.
	public interface ICompiledPropertyNode : IPropertyNode, ICompiledClassMemberNode
	{
		//Свойство в сборке.
		System.Reflection.PropertyInfo property_info
		{
			get;
		}

		//Тип, который содержит это свойство.
		ICompiledTypeNode compiled_comprehensive_type
		{
			get;
		}

		//Функция, которая возвращает значение свойства.
		ICompiledMethodNode compiled_get_method
		{
			get;
		}

		//Функция, которая устанавливает значение свойства.
		ICompiledMethodNode compiled_set_method
		{
			get;
		}

	}

	public interface IGetAddrNode : IExpressionNode
	{
		IExpressionNode addr_of_expr
		{
			get;
		}
	}
	
	public interface IDereferenceNode : IAddressedExpressionNode
	{
		IExpressionNode derefered_expr
		{
			get;
		}
	}

	public interface INullConstantNode : IConstantNode
    {
    }

    public interface IStatementsExpressionNode : IExpressionNode
    {
        IStatementNode[] statements
        {
            get;
        }

        IExpressionNode expresion
        {
            get;
        }
    }

    public interface IQuestionColonExpressionNode : IExpressionNode
    {
        IExpressionNode condition
        {
            get;
        }

        IExpressionNode ret_if_true
        {
            get;
        }

        IExpressionNode ret_if_false
        {
            get;
        }
    }

	public interface IDoubleQuestionColonExpressionNode : IExpressionNode
	{
		IExpressionNode condition
		{
			get;
		}

		IExpressionNode ret_if_null
		{
			get;
		}
	}

	public interface ILabelNode : IDefinitionNode, ILocated
    {
        //Имя метки. Для языков не чувствительных к регистрам хранит имя в том виде, в каком тип объявлен.
        string name
        {
            get;
        }

        //встречена ли метка в коде
        /*bool is_defined
        {
            get;
            set;
        }*/

    }

    public interface ILabeledStatementNode : IStatementNode, ILocated
    {
        //Метка, которой помечен statement
        ILabelNode label
        {
            get;
        }

        //Сама инструкция
        IStatementNode statement
        {
            get;
        }

    }

    public interface IGotoStatementNode : IStatementNode, ILocated
    {
        //Метка, на которую происходит переход
        ILabelNode label
        {
            get;
        }
    }

    public interface IForeachNode : IStatementNode, ILocated
    {
        IVAriableDefinitionNode VarIdent
        {
            get;
        }

        IExpressionNode InWhatExpr
        {
            get;
        }

        IStatementNode Body
        {
            get;
        }

        ITypeNode ElementType
        {
            get;
        }

        bool IsGeneric
        {
            get;
        }
    }

    public interface ILockStatement : IStatementNode, ILocated
    {
        IExpressionNode LockObject
        {
            get;
        }
        IStatementNode Body
        {
            get;
        }
    }

    public interface IRethrowStatement : IStatementNode, ILocated
    {
    	
    }
    
    public interface INamespaceConstantReference : IConstantNode, ILocated
    {
    	INamespaceConstantDefinitionNode Constant
    	{
    		get;
    	}
    }
    
    public interface IFunctionConstantReference : IConstantNode, ILocated
    {
    	ICommonFunctionConstantDefinitionNode Constant
    	{
    		get;
    	}
    }
    
    public interface IFunctionConstantDefinitionNode : IConstantDefinitionNode, ILocated
    {
        ICommonFunctionNode function
        {
            get;
        }
    }
    
    public interface ICommonConstructorCallAsConstant : IConstantNode, ILocated
    {
    	ICommonConstructorCall ConstructorCall
    	{
    		get;
    	}
    }
    
    public interface IEventNode : IDefinitionNode
    {
    	
    }
    
    public interface ICompiledEventNode : IEventNode
    {
        System.Reflection.EventInfo CompiledEvent
        {
            get;
        }
    }
    
    public interface ICommonEventNode : IDefinitionNode, IEventNode, ICommonClassMemberNode, ILocated
    {
    	string Name
    	{
    		get;
    	}
    	
    	ITypeNode DelegateType
    	{
    		get;
    	}
    	
    	ICommonMethodNode AddMethod
    	{
    		get;
    	}
    	
    	ICommonMethodNode RemoveMethod
    	{
    		get;
    	}
    	
    	ICommonMethodNode RaiseMethod
    	{
    		get;
    	}

        ICommonClassFieldNode Field
        {
            get;
        }

    	bool IsStatic
    	{
    		get;
    	}
    }

    public interface ICommonNamespaceEventNode : IDefinitionNode, IEventNode, ILocated
    {
        string Name
        {
            get;
        }

        ITypeNode DelegateType
        {
            get;
        }

        ICommonNamespaceFunctionNode AddFunction
        {
            get;
        }

        ICommonNamespaceFunctionNode RemoveFunction
        {
            get;
        }

        ICommonNamespaceFunctionNode RaiseFunction
        {
            get;
        }

        ICommonNamespaceVariableNode Field
        {
            get;
        }
    }

    public interface IStaticEventReference : IAddressedExpressionNode
    {
    	IEventNode Event
    	{
    		get;
    	}
    }
    
    public interface INonStaticEventReference : IStaticEventReference
    {
    	IExpressionNode obj
    	{
    		get;
    	}
    }

    public interface IDefaultOperatorNode : IExpressionNode
    {
    }
	
    public interface IAttributeNode : ISemanticNode, ILocated
    {
    	IFunctionNode AttributeConstructor
    	{
    		get;
    	}
    	
    	attribute_qualifier_kind qualifier
    	{
    		get;
    	}
    	
    	ITypeNode AttributeType
    	{
    		get;
    	}
    	
    	IConstantNode[] Arguments
    	{
    		get;
    	}
    	
    	IPropertyNode[] PropertyNames
    	{
    		get;
    	}
    	
    	IConstantNode[] PropertyInitializers
    	{
    		get;
    	}
    	
    	IVAriableDefinitionNode[] FieldNames
    	{
    		get;
    	}
    	
    	IConstantNode[] FieldInitializers
    	{
    		get;
    	}
    }
    public interface ILambdaFunctionNode : IExpressionNode
    {
        //Вид узла - базовый (basic), обычный (common) или экспортируемый (compiled).
        node_kind node_kind
        {
            get;
        }

        //Список формальных параметров функции.
        IParameterNode[] parameters
        {
            get;
        }

        //Тип возвращаемого значения функции.
        ITypeNode return_value_type
        {
            get;
        }

        IStatementNode body
        {
            get;
        }

        IFunctionNode function
        {
            get;
        }

        //Расположение функции - в функции, в классе, в пространстве имен.
        node_location_kind node_location_kind
        {
            get;
        }

        //Является ли generic-функцией
        bool is_generic_function
        {
            get;
        }

        //Число типов-параметров generic-функции. 0 для не-generic.
        int generic_parameters_count
        {
            get;
        }
    }
    public interface ILambdaFunctionCallNode : IExpressionNode
    {
        //Список фактических параметров. Количество и типы формальных и фактических параметров сверяется
        //на этапе построения семантического дерева. При необходимости при построении семантического дерева
        //вставляются узлы преобрызования типов.
        IExpressionNode[] parameters
        {
            get;
        }

        //Вызываемый метод.
        ILambdaFunctionNode lambda
        {
            get;
        }
    }
    /*public interface ICompiledFunctionNode : IFunctionNode
    {
        string test
        {
            get;
        }      
    }*/
}
