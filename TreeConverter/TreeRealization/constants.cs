// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

namespace PascalABCCompiler.TreeRealization
{
    /// <summary>
    /// Базовый класс для представления констант в выражениях.
    /// Этот класс представляет только неименованные константы.
    /// </summary>
	[Serializable]
	public abstract class constant_node : expression_node, SemanticTree.IConstantNode
	{
        private type_node _type;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="tn">Тип константы.</param>
        /// <param name="loc">Расположение константы.</param>
		public constant_node(type_node tn, location loc) : base(null,loc)
		{
            _type = tn;
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

        internal virtual object get_object_value()
        {
            return null;
        }

        object SemanticTree.IConstantNode.value
        {
            get
            {
                return get_object_value();
            }
        }
		
        public virtual constant_node get_constant_copy(location loc)
        {
        	return this;
        }
        
        public override type_node type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public virtual void SetType(type_node tn)
        {
            _type = tn;
        }

        public override bool is_addressed
        {
            get
            {
                return false;
            }
        }

        public static constant_node make_constant(object obj)
        {
            return NetHelper.NetHelper.make_constant(obj);
        }
	}


    /// <summary>
    /// Базовый класс для представления констант конкретных типов.
    /// </summary>
    /// <typeparam name="ConstantType">Тип константы.</typeparam>
    [Serializable]
    public abstract class concrete_constant<ConstantType> : constant_node
    {
        /// <summary>
        /// Значение константы.
        /// </summary>
        private ConstantType _constant_value;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="tn">Тип константы.</param>
        public concrete_constant(ConstantType value, location loc) :
            base(compiled_type_node.get_type_node(typeof(ConstantType)), loc)
        {
            _constant_value = value;
        }

        /// <summary>
        /// Значение константы.
        /// </summary>
        public ConstantType constant_value
        {
            get
            {
                return _constant_value;
            }
            set
            {
                _constant_value = value;
            }
        }
         
        internal override object get_object_value()
        {
            return _constant_value;
        }

    }

    /// <summary>
    /// Класс для представления булевских констант.
    /// </summary>
    [Serializable]
    public class bool_const_node : concrete_constant<bool>, SemanticTree.IBoolConstantNode
    {

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
        public bool_const_node(bool value, location loc)
            : base(value,loc)
        {
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.bool_const_node;
            }
        }
		
        public override constant_node get_constant_copy(location loc)
        {
        	return new bool_const_node(this.constant_value,loc);
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
    /// Класс для представления int констант.
    /// </summary>
	[Serializable]
	public class int_const_node : concrete_constant<int>, SemanticTree.IIntConstantNode
	{
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
		public int_const_node(int value,location loc) : base(value,loc)
		{
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.int_const_node;
			}
		}
		
		public override constant_node get_constant_copy(location loc)
        {
        	return new int_const_node(this.constant_value,loc);
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
    /// Класс для представления long констант.
    /// </summary>
    [Serializable]
    public class long_const_node : concrete_constant<long>, SemanticTree.ILongConstantNode
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
        public long_const_node(long value, location loc)
            : base(value, loc)
        {
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.long_const_node;
            }
        }
		
        public override constant_node get_constant_copy(location loc)
        {
        	return new long_const_node(this.constant_value,loc);
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
    /// Класс для представления byte констант.
    /// </summary>
    [Serializable]
    public class byte_const_node : concrete_constant<byte>, SemanticTree.IByteConstantNode
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
        public byte_const_node(byte value, location loc)
            : base(value, loc)
        {
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.byte_const_node;
            }
        }
		
        public override constant_node get_constant_copy(location loc)
        {
        	return new byte_const_node(this.constant_value,loc);
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
    /// Класс для представления signed byte констант.
    /// </summary>
    [Serializable]
    public class sbyte_const_node : concrete_constant<sbyte>, SemanticTree.ISByteConstantNode
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
        public sbyte_const_node(sbyte value, location loc)
            : base(value, loc)
        {
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.sbyte_const_node;
            }
        }
		
        public override constant_node get_constant_copy(location loc)
        {
        	return new sbyte_const_node(this.constant_value,loc);
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
    /// Класс для представления short констант.
    /// </summary>
    [Serializable]
    public class short_const_node : concrete_constant<short>, SemanticTree.IShortConstantNode
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
        public short_const_node(short value, location loc)
            : base(value, loc)
        {
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.short_const_node;
            }
        }
		
        public override constant_node get_constant_copy(location loc)
        {
        	return new short_const_node(this.constant_value,loc);
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
    /// Класс для представления unsigned short констант.
    /// </summary>
    [Serializable]
    public class ushort_const_node : concrete_constant<ushort>, SemanticTree.IUShortConstantNode
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
        public ushort_const_node(ushort value, location loc)
            : base(value, loc)
        {
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.ushort_const_node;
            }
        }
		
        public override constant_node get_constant_copy(location loc)
        {
        	return new ushort_const_node(this.constant_value,loc);
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
    /// Класс для представления unsigned int констант.
    /// </summary>
    [Serializable]
    public class uint_const_node : concrete_constant<uint>, SemanticTree.IUIntConstantNode
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
        public uint_const_node(uint value, location loc)
            : base(value, loc)
        {
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.uint_const_node;
            }
        }
		
        public override constant_node get_constant_copy(location loc)
        {
        	return new uint_const_node(this.constant_value,loc);
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
    /// Класс для представления unsigned long констант.
    /// </summary>
    [Serializable]
    public class ulong_const_node : concrete_constant<ulong>, SemanticTree.IULongConstantNode
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
        public ulong_const_node(ulong value, location loc)
            : base(value, loc)
        {
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.ulong_const_node;
            }
        }
		
        public override constant_node get_constant_copy(location loc)
        {
        	return new ulong_const_node(this.constant_value,loc);
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
    /// Класс для представления double констант.
    /// </summary>
	[Serializable]
	public class double_const_node : concrete_constant<double>, SemanticTree.IDoubleConstantNode
	{
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
		public double_const_node(double value,location loc) : base(value,loc)
		{
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.double_const_node;
			}
		}
		
		public override constant_node get_constant_copy(location loc)
        {
        	return new double_const_node(this.constant_value,loc);
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
    /// Класс для представления float констант.
    /// </summary>
    [Serializable]
    public class float_const_node : concrete_constant<float>, SemanticTree.IFloatConstantNode
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
        public float_const_node(float value, location loc)
            : base(value, loc)
        {
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.float_const_node;
            }
        }
		
        public override constant_node get_constant_copy(location loc)
        {
        	return new float_const_node(this.constant_value,loc);
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
    /// Класс для представления char констант (этот класс для 2-байтных char - widechar в delphi).
    /// </summary>
	[Serializable]
	public class char_const_node : concrete_constant<char>, SemanticTree.ICharConstantNode
	{
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
		public char_const_node(char value,location loc) : base(value,loc)
		{
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.char_const_node;
			}
		}
		
		public override constant_node get_constant_copy(location loc)
        {
        	return new char_const_node(this.constant_value,loc);
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
    /// Класс для представления строковых констант.
    /// </summary>
	public class string_const_node : concrete_constant<string>, SemanticTree.IStringConstantNode
	{

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="const_value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
		public string_const_node(string value,location loc) : base(value,loc)
		{
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.string_const_node;
			}
		}
		
		public override constant_node get_constant_copy(location loc)
        {
        	return new string_const_node(this.constant_value,loc);
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
    /// Класс для представления константы nil.
    /// </summary>
    public class null_const_node : constant_node, SemanticTree.INullConstantNode
    {
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="const_value">Значение константы.</param>
        /// <param name="loc">Расположение контанты.</param>
		public null_const_node(type_node tn,location loc) : base(tn,loc)
		{
        }

        public static null_const_node get_const_node_with_type(type_node tn, null_const_node cnst)
        {
            return new null_const_node(tn, cnst.location);
        }
        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.null_const_node;
			}
		}
		
		public override constant_node get_constant_copy(location loc)
		{
			return new null_const_node(this.type,loc);
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
    /// Класс для представления констaнтной записи
    /// </summary>
    [Serializable]
    public class record_constant : constant_node, SemanticTree.IRecordConstantNode
    {
        private List<constant_node> _field_values = new List<constant_node>();
        
        public List<constant_node> field_values
        {
            get
            {
                return _field_values;
            }
        }

        internal List<SyntaxTree.record_const_definition> record_const_definition_list;

        public record_constant(List<constant_node> field_values, location loc)
            :base(null, loc)
        {
            _field_values = field_values;
        }

        internal record_constant(List<SyntaxTree.record_const_definition> record_const_definition_list, location loc)
            : base(null, loc)
        {
            this.record_const_definition_list = record_const_definition_list;
        }
		
		public override constant_node get_constant_copy(location loc)
		{
			record_constant rc = new record_constant(this.record_const_definition_list,loc);
			rc.type = this.type;
			return rc;
		}
		
        public SemanticTree.IConstantNode[] FieldValues
        {
            get
            {
                return _field_values.ToArray();
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

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.record_const;
            }
        }

    }

    public class enum_const_node : constant_node, SemanticTree.IEnumConstNode
    {
        private int _constant_value;

        public enum_const_node(int value, type_node ctn, location loc):base(ctn,loc)
        {
            this._constant_value = value;
        }

        internal override object get_object_value()
        {
            return _constant_value;
        }

        public int constant_value
        {
            get
            {
                return _constant_value;
            }
        }
		
        public override constant_node get_constant_copy(location loc)
        {
        	return new enum_const_node(this._constant_value,this.type,loc);
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
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.enum_const;
            }
        }
    }

    /// <summary>
    /// Класс для представления константного массива
    /// </summary>
    [Serializable]
    public class array_const : constant_node, SemanticTree.IArrayConstantNode
    {
        private List<constant_node> _element_values;

        public List<constant_node> element_values
        {
            get
            {
                return _element_values;
            }
        }


        SemanticTree.IConstantNode[] SemanticTree.IArrayConstantNode.ElementValues
        {
            get
            {
                return _element_values.ToArray();
            }
        }

        SemanticTree.ITypeNode SemanticTree.IArrayConstantNode.ElementType
        {
            get
            {
                return element_type;
            }
        }

        public array_const(List<constant_node> element_values, location loc)
            :
            base(null, loc)
        {
            this._element_values = element_values;
        }
		
		public override constant_node get_constant_copy(location loc)
		{
			array_const ac = new array_const(this._element_values,loc);
			ac.type = this.type;
			return ac;
		}

        public type_node element_type
        {
            get
            {
                return element_values[0].type;
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

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.array_const;
            }
        }
    }

    public class compiled_static_field_reference_as_constant : constant_node, SemanticTree.ICompiledStaticFieldReferenceNodeAsConstant
    {
        private static_compiled_variable_reference _field_reference;

        public static_compiled_variable_reference field_reference
        {
            get
            {
                return _field_reference;
            }
            set
            {
                _field_reference = value;
            }
        }

        public compiled_static_field_reference_as_constant(static_compiled_variable_reference field_reference, location loc) :
            base(field_reference.type, loc)
        {
            _field_reference = field_reference;
        }

        SemanticTree.IStaticCompiledFieldReferenceNode SemanticTree.ICompiledStaticFieldReferenceNodeAsConstant.FieldReference
        {
            get
            {
                return _field_reference;
            }
        }

        public override constant_node get_constant_copy(location loc)
        {
            return new compiled_static_field_reference_as_constant(this.field_reference, loc);
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
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.compiled_static_field_reference_as_constant;
            }
        }
    }

    public class compiled_static_method_call_as_constant : constant_node, SemanticTree.ICompiledStaticMethodCallNodeAsConstant
    {
        private compiled_static_method_call _method_call;

        public compiled_static_method_call method_call
        {
            get
            {
                return _method_call;
            }
            set
            {
                _method_call = value;
            }
        }

        public compiled_static_method_call_as_constant(compiled_static_method_call method_call, location loc):
            base(method_call.type, loc)
        {
            _method_call = method_call;
        }

        SemanticTree.ICompiledStaticMethodCallNode SemanticTree.ICompiledStaticMethodCallNodeAsConstant.MethodCall
        {
            get
            {
                return _method_call;
            }
        }
		
		public override constant_node get_constant_copy(location loc)
		{
			return new compiled_static_method_call_as_constant(this.method_call,loc);
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
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.compiled_static_method_call_node_as_constant;
            }
        }
    }

    public class common_static_method_call_as_constant : constant_node, SemanticTree.ICommonStaticMethodCallNodeAsConstant
    {
        private common_static_method_call _method_call;

        public common_static_method_call method_call
        {
            get
            {
                return _method_call;
            }
            set
            {
                _method_call = value;
            }
        }

        public common_static_method_call_as_constant(common_static_method_call method_call, location loc) :
            base(method_call.type, loc)
        {
            _method_call = method_call;
        }

        SemanticTree.ICommonStaticMethodCallNode SemanticTree.ICommonStaticMethodCallNodeAsConstant.MethodCall
        {
            get
            {
                return _method_call;
            }
        }

        public override constant_node get_constant_copy(location loc)
        {
            return new common_static_method_call_as_constant(this.method_call, loc);
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
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.common_static_method_call_node_as_constant;
            }
        }
    }

    public class common_namespace_function_call_as_constant : constant_node, SemanticTree.ICommonNamespaceFunctionCallNodeAsConstant
    {
        private common_namespace_function_call _method_call;

        public common_namespace_function_call method_call
        {
            get
            {
                return _method_call;
            }
            set
            {
                _method_call = value;
            }
        }

        public common_namespace_function_call_as_constant(common_namespace_function_call method_call, location loc)
            :
            base(method_call.type, loc)
        {
            _method_call = method_call;
        }

        SemanticTree.ICommonNamespaceFunctionCallNode SemanticTree.ICommonNamespaceFunctionCallNodeAsConstant.MethodCall
        {
            get
            {
                return _method_call;
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

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.common_namespace_function_call_node_as_constant;
            }
        }
    }
	
    public class basic_function_call_as_constant : constant_node, SemanticTree.IBasicFunctionCallNodeAsConstant
    {
        private basic_function_call _method_call;

        public basic_function_call method_call
        {
            get
            {
                return _method_call;
            }
            set
            {
                _method_call = value;
            }
        }

        public basic_function_call_as_constant(basic_function_call method_call, location loc)
            :
            base(method_call.type, loc)
        {
            _method_call = method_call;
        }
        
        public override constant_node get_constant_copy(location loc)
		{
			return new basic_function_call_as_constant(this.method_call,loc);
		}

        SemanticTree.IBasicFunctionCallNode SemanticTree.IBasicFunctionCallNodeAsConstant.MethodCall
        {
            get
            {
                return _method_call;
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

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.basic_function_call_node_as_constant;
            }
        }
    }

    public class default_operator_node_as_constant : constant_node, SemanticTree.IDefaultOperatorNodeAsConstant
    {
        private default_operator_node _default_operator;

        public default_operator_node default_operator
        {
            get
            {
                return _default_operator;
            }
            set
            {
                _default_operator = value;
            }
        }

        public default_operator_node_as_constant(default_operator_node default_operator, location loc)
            :
            base(default_operator.type, loc)
        {
            _default_operator = default_operator;
        }

        SemanticTree.IDefaultOperatorNode SemanticTree.IDefaultOperatorNodeAsConstant.DefaultOperator
        {
            get
            {
                return _default_operator;
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

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.default_operator_node_as_constant;
            }
        }
    }

    public class typeof_operator_as_constant : constant_node, SemanticTree.ITypeOfOperatorAsConstant
    {
        private typeof_operator _typeof_operator;

        public typeof_operator typeof_operator
        {
            get
            {
                return _typeof_operator;
            }
            set
            {
                _typeof_operator = value;
            }
        }

        public typeof_operator_as_constant(typeof_operator typeof_operator, location loc)
            :
            base(typeof_operator.type, loc)
        {
            _typeof_operator = typeof_operator;
        }

        SemanticTree.ITypeOfOperator SemanticTree.ITypeOfOperatorAsConstant.TypeOfOperator
        {
            get
            {
                return _typeof_operator;
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

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.typeof_operator_as_constant;
            }
        }
    }

    public class sizeof_operator_as_constant : constant_node, SemanticTree.ISizeOfOperatorAsConstant
    {
        private sizeof_operator _sizeof_operator;

        public sizeof_operator sizeof_operator
        {
            get
            {
                return _sizeof_operator;
            }
            set
            {
                _sizeof_operator = value;
            }
        }

        public sizeof_operator_as_constant(sizeof_operator sizeof_operator, location loc)
            :
            base(sizeof_operator.type, loc)
        {
            _sizeof_operator = sizeof_operator;
        }

        SemanticTree.ISizeOfOperator SemanticTree.ISizeOfOperatorAsConstant.SizeOfOperator
        {
            get
            {
                return _sizeof_operator;
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

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.sizeof_operator_as_constant;
            }
        }
    }

    public class compiled_constructor_call_as_constant : constant_node, SemanticTree.ICompiledConstructorCallAsConstant
    {
        private compiled_constructor_call _method_call;

        public compiled_constructor_call method_call
        {
            get
            {
                return _method_call;
            }
            set
            {
                _method_call = value;
            }
        }

        public compiled_constructor_call_as_constant(compiled_constructor_call method_call, location loc)
            :
            base(method_call.type, loc)
        {
            _method_call = method_call;
        }

        SemanticTree.ICompiledConstructorCall SemanticTree.ICompiledConstructorCallAsConstant.MethodCall
        {
            get
            {
                return _method_call;
            }
        }

		public override constant_node get_constant_copy(location loc)
		{
			return new compiled_constructor_call_as_constant(this.method_call,loc);
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
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.compiled_constructor_call_as_constant;
            }
        }
    }
    
    public class namespace_constant_reference : constant_node, SemanticTree.INamespaceConstantReference
    {
    	private namespace_constant_definition cdn;
    	
    	public namespace_constant_reference(namespace_constant_definition cdn, location loc):base(cdn.type,loc)
    	{
    		this.cdn = cdn;
    	}
    	
		public namespace_constant_definition constant 
		{
			get { return cdn; }
			set { cdn = value; }
		}
    	
    	SemanticTree.INamespaceConstantDefinitionNode SemanticTree.INamespaceConstantReference.Constant
    	{
    		get
    		{
    			return cdn;
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
        
        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.namespace_constant_reference;
            }
        }
    }
    
    public class function_constant_reference : constant_node, SemanticTree.IFunctionConstantReference
    {
    	private function_constant_definition cdn;
    	
    	public function_constant_reference(function_constant_definition cdn, location loc):base(cdn.type,loc)
    	{
    		this.cdn = cdn;
    	}
    	
		public function_constant_definition constant 
		{
			get { return cdn; }
			set { cdn = value; }
		}
    	
    	SemanticTree.ICommonFunctionConstantDefinitionNode SemanticTree.IFunctionConstantReference.Constant
    	{
    		get
    		{
    			return cdn;
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
        
        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.function_constant_reference;
            }
        }
    }
    
    public class common_constructor_call_as_constant : constant_node, SemanticTree.ICommonConstructorCallAsConstant
    {
        private common_constructor_call _constructor_call;

        public common_constructor_call constructor_call
        {
            get
            {
                return _constructor_call;
            }
            set
            {
                _constructor_call = value;
            }
        }

        public common_constructor_call_as_constant(common_constructor_call constructor_call, location loc)
            :
            base(constructor_call.type, loc)
        {
            _constructor_call = constructor_call;
        }

        SemanticTree.ICommonConstructorCall SemanticTree.ICommonConstructorCallAsConstant.ConstructorCall
        {
            get
            {
                return _constructor_call;
            }
        }

		public override constant_node get_constant_copy(location loc)
		{
			return new common_constructor_call_as_constant(this._constructor_call,loc);
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
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.common_constructor_call_as_constant;
            }
        }
    }
}
