// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PascalABCCompiler.NETGenerator
{
    public class TypeFactory
    {
        public static TypeInfo int_type;
        public static TypeInfo double_type;
        public static TypeInfo bool_type;
        public static TypeInfo char_type;
        public static TypeInfo string_type;
        public static TypeInfo byte_type;
        public static Type ExceptionType = typeof(Exception);
        public static Type VoidType =   typeof(void);
        public static Type StringType = typeof(string);
        public static Type ObjectType = typeof(object);
        public static Type MonitorType = typeof(System.Threading.Monitor);
        public static Type IntPtr = typeof(System.IntPtr);
        public static Type ArrayType = typeof(System.Array);
        public static Type MulticastDelegateType = typeof(MulticastDelegate);
        public static Type DefaultMemberAttributeType = typeof(DefaultMemberAttribute);
        public static Type EnumType = typeof(Enum);
        public static Type ExtensionAttributeType = typeof(System.Runtime.CompilerServices.ExtensionAttribute);
        public static Type ConvertType = typeof(Convert);

        //primitive
        public static Type BoolType = typeof(Boolean);
        public static Type SByteType = typeof(SByte);
        public static Type ByteType = typeof(Byte);
        public static Type CharType = typeof(Char);
        public static Type Int16Type = typeof(Int16);
        public static Type Int32Type = typeof(Int32);
        public static Type Int64Type = typeof(Int64);
        public static Type UInt16Type = typeof(UInt16);
        public static Type UInt32Type = typeof(UInt32);
        public static Type UInt64Type = typeof(UInt64);
        public static Type SingleType = typeof(Single);
        public static Type DoubleType = typeof(Double);
        public static Type GCHandleType = typeof(GCHandle);
        public static Type MarshalType = typeof(Marshal);
        public static Type TypeType =   typeof(Type);
        public static Type ValueType = typeof(ValueType);
        public static Type IEnumerableType = typeof(System.Collections.IEnumerable);
        public static Type IEnumeratorType = typeof(System.Collections.IEnumerator);
        public static Type IDisposableType = typeof(IDisposable);
        public static Type IEnumerableGenericType = typeof(System.Collections.Generic.IEnumerable<>);
        public static Type IEnumeratorGenericType = typeof(System.Collections.Generic.IEnumerator<>);

        private static Hashtable types;
        private static Hashtable sizes;
        public static MethodInfo ArrayCopyMethod;
        public static MethodInfo GetTypeFromHandleMethod;
		public static MethodInfo ResizeMethod;
        public static MethodInfo GCHandleFreeMethod;
		public static MethodInfo StringNullOrEmptyMethod;
        public static MethodInfo UnsizedArrayCreateMethodTemplate = null;
        public static MethodInfo GCHandleAlloc;
        public static MethodInfo GCHandleAllocPinned;
        public static MethodInfo OffsetToStringDataProperty;
        public static MethodInfo StringLengthMethod;
        public static MethodInfo CharToString;
        public static ConstructorInfo IndexOutOfRangeConstructor;
        public static ConstructorInfo ParamArrayAttributeConstructor;
        public static MethodInfo StringCopyMethod;

        public static MethodInfo GetUnsizedArrayCreateMethod(TypeInfo ti)
        {
            if (UnsizedArrayCreateMethodTemplate == null)
                UnsizedArrayCreateMethodTemplate = ArrayType.GetMethod("Resize");
            return UnsizedArrayCreateMethodTemplate.MakeGenericMethod(ti.tp.GetElementType());
        }

        static TypeFactory()
        {
            int_type = new TypeInfo(typeof(int));
            double_type = new TypeInfo(typeof(double));
            bool_type = new TypeInfo(typeof(bool));
            char_type = new TypeInfo(typeof(char));
            string_type = new TypeInfo(typeof(string));
            byte_type = new TypeInfo(typeof(byte));
            
            types = new Hashtable();
            types[BoolType] = BoolType;
            types[SByteType] = SByteType;
            types[ByteType] = ByteType;
            types[CharType] = CharType;
            types[Int16Type] = Int16Type;
            types[Int32Type] = Int32Type;
            types[Int64Type] = Int64Type;
            types[UInt16Type] = UInt16Type;
            types[UInt32Type] = UInt32Type;
            types[UInt64Type] = UInt64Type;
            types[SingleType] = SingleType;
            types[DoubleType] = DoubleType;

            sizes = new Hashtable();
            sizes[BoolType] = sizeof(Boolean);
            sizes[SByteType] = sizeof(SByte);
            sizes[ByteType] = sizeof(Byte);
            sizes[CharType] = sizeof(Char);
            sizes[Int16Type] = sizeof(Int16);
            sizes[Int32Type] = sizeof(Int32);
            sizes[Int64Type] = sizeof(Int64);
            sizes[UInt16Type] = sizeof(UInt16);
            sizes[UInt32Type] = sizeof(UInt32);
            sizes[UInt64Type] = sizeof(UInt64);
            sizes[SingleType] = sizeof(Single);
            sizes[DoubleType] = sizeof(Double);
            //sizes[UIntPtr] = sizeof(UIntPtr);
            
            //types[TypeType] = TypeType;
            ArrayCopyMethod = typeof(Array).GetMethod("Copy", new Type[3] { typeof(Array), typeof(Array), typeof(int) });
            StringNullOrEmptyMethod = typeof(string).GetMethod("IsNullOrEmpty");
            GCHandleAlloc = typeof(System.Runtime.InteropServices.GCHandle).GetMethod("Alloc",new Type[1]{TypeFactory.ObjectType});
            GCHandleAllocPinned = typeof(System.Runtime.InteropServices.GCHandle).GetMethod("Alloc", new Type[2] { TypeFactory.ObjectType, typeof(GCHandleType) });
            OffsetToStringDataProperty = typeof(System.Runtime.CompilerServices.RuntimeHelpers).GetProperty("OffsetToStringData",BindingFlags.Public|BindingFlags.Static|BindingFlags.Instance).GetGetMethod();
            StringLengthMethod = typeof(string).GetProperty("Length").GetGetMethod();
            IndexOutOfRangeConstructor = typeof(IndexOutOfRangeException).GetConstructor(Type.EmptyTypes);
            ParamArrayAttributeConstructor = typeof(ParamArrayAttribute).GetConstructor(Type.EmptyTypes);
            GCHandleFreeMethod = typeof(GCHandle).GetMethod("Free");
            GetTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle");
            StringCopyMethod = typeof(string).GetMethod("Copy");
            CharToString = typeof(char).GetMethod("ToString", BindingFlags.Static | BindingFlags.Public);
        }

        public static bool IsStandType(Type t)
        {
            return types[t] != null;
        }

        public static int GetPrimitiveTypeSize(Type PrimitiveType)
        {
            return (int)sizes[PrimitiveType];
        }
    }

    class NETGeneratorTools
    {
        public static void PushStind(ILGenerator il, Type elem_type)
        {
            switch (Type.GetTypeCode(elem_type))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.SByte:
                    il.Emit(OpCodes.Stind_I1);
                    break;
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    il.Emit(OpCodes.Stind_I2);
                    break;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    il.Emit(OpCodes.Stind_I4);
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    il.Emit(OpCodes.Stind_I8);
                    break;
                case TypeCode.Single:
                    il.Emit(OpCodes.Stind_R4);
                    break;
                case TypeCode.Double:
                    il.Emit(OpCodes.Stind_R8);
                    break;
                default:
                    if (IsPointer(elem_type))
                        il.Emit(OpCodes.Stind_I);
                    else if (elem_type.IsGenericParameter)
                        il.Emit(OpCodes.Stobj, elem_type);
                    else if (IsEnum(elem_type))
                        il.Emit(OpCodes.Stind_I4);
                    else
                        if (elem_type.IsValueType)
                            il.Emit(OpCodes.Stobj, elem_type);
                        else
                            il.Emit(OpCodes.Stind_Ref);
                    break;
            }
        }
        
        public static void PushStelem(ILGenerator il,Type elem_type)
        {
            switch (Type.GetTypeCode(elem_type))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.SByte:
                    il.Emit(OpCodes.Stelem_I1);
                    break;
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    il.Emit(OpCodes.Stelem_I2);
                    break;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    il.Emit(OpCodes.Stelem_I4);
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    il.Emit(OpCodes.Stelem_I8);
                    break;
                case TypeCode.Single:
                    il.Emit(OpCodes.Stelem_R4);
                    break;
                case TypeCode.Double:
                    il.Emit(OpCodes.Stelem_R8);
                    break;
                default:
                    if (IsPointer(elem_type))
                        il.Emit(OpCodes.Stelem_I);
                    else if (elem_type.IsGenericParameter)
                        il.Emit(OpCodes.Stelem, elem_type);
                    else if (IsEnum(elem_type))
                        il.Emit(OpCodes.Stelem_I4);
                    else 
                        if (elem_type.IsValueType) 
                            il.Emit(OpCodes.Stobj, elem_type);
                        else 
                            il.Emit(OpCodes.Stelem_Ref);
                    break;
            }
        }

        public static void PushParameterDereference(ILGenerator il, Type elem_type)
        {
            switch (Type.GetTypeCode(elem_type))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                    il.Emit(OpCodes.Ldind_U1);
                    break;
                case TypeCode.SByte:
                    il.Emit(OpCodes.Ldind_I1);
                    break;
                case TypeCode.Char:
                case TypeCode.UInt16:
                    il.Emit(OpCodes.Ldind_U2);
                    break;
                case TypeCode.Int16:
                    il.Emit(OpCodes.Ldind_I2);
                    break;
                case TypeCode.UInt32:
                    il.Emit(OpCodes.Ldind_U4);
                    break;
                case TypeCode.Int32:
                    il.Emit(OpCodes.Ldind_I4);
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    il.Emit(OpCodes.Ldind_I8);
                    break;
                case TypeCode.Single:
                    il.Emit(OpCodes.Ldind_R4);
                    break;
                case TypeCode.Double:
                    il.Emit(OpCodes.Ldind_R8);
                    break;
                default:
                    if (IsPointer(elem_type))
                        il.Emit(OpCodes.Ldind_I);
                    else
                        if (elem_type.IsValueType || elem_type.IsGenericParameter)
                            il.Emit(OpCodes.Ldobj, elem_type);
                        else
                            il.Emit(OpCodes.Ldind_Ref);
                    break;
            }
        }

        public static void PushLdelem(ILGenerator il, Type elem_type, bool ldobj)
        {
            switch (Type.GetTypeCode(elem_type))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                    il.Emit(OpCodes.Ldelem_U1);
                    break;
                case TypeCode.SByte:
                    il.Emit(OpCodes.Ldelem_I1);
                    break;
                case TypeCode.Char:
                case TypeCode.Int16:
                    il.Emit(OpCodes.Ldelem_I2);
                    break;
                case TypeCode.UInt16:
                    il.Emit(OpCodes.Ldelem_U2);
                    break;
                case TypeCode.Int32:
                    il.Emit(OpCodes.Ldelem_I4);
                    break;
                case TypeCode.UInt32:
                    il.Emit(OpCodes.Ldelem_U4);
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    il.Emit(OpCodes.Ldelem_I8);
                    break;
                case TypeCode.Single:
                    il.Emit(OpCodes.Ldelem_R4);
                    break;
                case TypeCode.Double:
                    il.Emit(OpCodes.Ldelem_R8);
                    break;
                default:
                    if (IsPointer(elem_type))
                        il.Emit(OpCodes.Ldelem_I);
                    else if (elem_type.IsGenericParameter)
                        il.Emit(OpCodes.Ldelem, elem_type);
                    else
                        if (elem_type.IsValueType)//если это структура
                        {
                            il.Emit(OpCodes.Ldelema, elem_type);//почему a?
                            // проверки нужно ли заменять тип возвр. знач. метода get_val массива на указатель
                            if (ldobj || !(elem_type != TypeFactory.VoidType && elem_type.IsValueType && !TypeFactory.IsStandType(elem_type)))
                                il.Emit(OpCodes.Ldobj, elem_type);
                        }
                        else il.Emit(OpCodes.Ldelem_Ref);
                    break;
            }           
        }
        public static void LdcIntConst(ILGenerator il, int e)
        {
            switch (e)
            {
                case -1: il.Emit(OpCodes.Ldc_I4_M1); break;
                case 0: il.Emit(OpCodes.Ldc_I4_0); break;
                case 1: il.Emit(OpCodes.Ldc_I4_1); break;
                case 2: il.Emit(OpCodes.Ldc_I4_2); break;
                case 3: il.Emit(OpCodes.Ldc_I4_3); break;
                case 4: il.Emit(OpCodes.Ldc_I4_4); break;
                case 5: il.Emit(OpCodes.Ldc_I4_5); break;
                case 6: il.Emit(OpCodes.Ldc_I4_6); break;
                case 7: il.Emit(OpCodes.Ldc_I4_7); break;
                case 8: il.Emit(OpCodes.Ldc_I4_8); break;
                default:
                    if (e < sbyte.MinValue || e > sbyte.MaxValue)
                        il.Emit(OpCodes.Ldc_I4, e);
                    else
                        il.Emit(OpCodes.Ldc_I4_S, (sbyte)e);
                    break;
                /*if (e > sbyte.MinValue && e < sbyte.MaxValue)  //DarkStar Changed
                    il.Emit(OpCodes.Ldc_I4_S,(sbyte)e);
                else if (e > Int32.MinValue && e < Int32.MaxValue)  
                    il.Emit(OpCodes.Ldc_I4, (int)e); break;		*/
            }
        }

        public static void PushLdc(ILGenerator il, Type elem_type, object value)
        {
            switch (Type.GetTypeCode(elem_type))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                    //il.Emit(OpCodes.Ldc_I4_S, Convert.ToByte(value));
                    LdcIntConst(il, Convert.ToByte(value));
                    break;
                case TypeCode.SByte:
                    LdcIntConst(il, Convert.ToSByte(value));
                    //il.Emit(OpCodes.Ldc_I4_S, Convert.ToSByte(value));
                    break;
                case TypeCode.Char:
                    LdcIntConst(il, Convert.ToChar(value));
                    //il.Emit(OpCodes.Ldc_I4, Convert.ToChar(value));
                    break;
                case TypeCode.Int16:
                    LdcIntConst(il, Convert.ToInt32(value));
                    //il.Emit(OpCodes.Ldc_I4, Convert.ToInt32(value));
                    break;
                case TypeCode.UInt16:
                    LdcIntConst(il, Convert.ToUInt16(value));
                    //il.Emit(OpCodes.Ldc_I4, Convert.ToUInt16(value));
                    break;
                case TypeCode.Int32:
                    LdcIntConst(il,Convert.ToInt32(value));
                    break;
                case TypeCode.UInt32:
                    LdcIntConst(il, (Int32)Convert.ToUInt32(value));
                    //il.Emit(OpCodes.Ldc_I4, Convert.ToUInt32(value));
                    break;
                case TypeCode.Int64:
                    il.Emit(OpCodes.Ldc_I8, Convert.ToInt64(value));
                    break;
                case TypeCode.UInt64:
                    UInt64 UInt64 = Convert.ToUInt64(value);
                    if (UInt64 > Int64.MaxValue)
                    {
                        //Это будет медленно работать. Надо переделать.
                        //Надо разобраться как сссделано в C#, там все нормально
                        Int64 tmp = (Int64)(UInt64 - Int64.MaxValue - 1);
                        il.Emit(OpCodes.Ldc_I8, tmp);
                        il.Emit(OpCodes.Conv_U8);
                        il.Emit(OpCodes.Ldc_I8, Int64.MaxValue);
                        il.Emit(OpCodes.Conv_U8);
                        il.Emit(OpCodes.Add);
                        il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Add);
                    }
                    else
                        il.Emit(OpCodes.Ldc_I8, Convert.ToInt64(value));
                    break;
                case TypeCode.Single:
                    il.Emit(OpCodes.Ldc_R4, (Single)value);
                    break;
                case TypeCode.Double:
                    il.Emit(OpCodes.Ldc_R8, (Double)value);
                    break;
                case TypeCode.String:
                    il.Emit(OpCodes.Ldstr, (string)value);
                    break;
                default:
                    if (IsEnum(elem_type))
                        //il.Emit(OpCodes.Ldc_I4, (Int32)value);
                        LdcIntConst(il, (Int32)value);
                    else
                        throw new Exception("Немогу положить PushLdc для " + value.GetType().ToString());
                    break;
            }
        }

        public static void PushCast(ILGenerator il, Type tp, Type from_value_type)
        {
            if (IsPointer(tp))
                return;
            //(ssyy) Вставил 15.05.08
            if (from_value_type != null)
            {
                il.Emit(OpCodes.Box, from_value_type);
            }
            if (tp.IsValueType || tp.IsGenericParameter)
                il.Emit(OpCodes.Unbox_Any, tp);
            else
                il.Emit(OpCodes.Castclass, tp);
        }
        
        public static LocalBuilder CreateLocalAndLoad(ILGenerator il, Type tp)
        {
            LocalBuilder lb = il.DeclareLocal(tp);
            il.Emit(OpCodes.Stloc, lb);
            if (tp.IsValueType)
                il.Emit(OpCodes.Ldloca, lb);
            else
                il.Emit(OpCodes.Ldloc, lb);
            return lb;
        }
        
        public static LocalBuilder CreateLocal(ILGenerator il, Type tp)
        {
            LocalBuilder lb = il.DeclareLocal(tp);
            il.Emit(OpCodes.Stloc, lb);
            return lb;
        }
        
        public static LocalBuilder CreateLocalAndLdloca(ILGenerator il, Type tp)
        {
            LocalBuilder lb = il.DeclareLocal(tp);
            il.Emit(OpCodes.Stloc, lb);
            il.Emit(OpCodes.Ldloca, lb);
            return lb;
        }

        public static void CreateBoundedArray(ILGenerator il, FieldBuilder fb, TypeInfo ti)
        {
            Label lbl = il.DefineLabel();
            if (fb.IsStatic)
                il.Emit(OpCodes.Ldsfld, fb);
            else
            {
                //il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, fb);
            }
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Brfalse, lbl);
            if (!fb.IsStatic)
                il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Newobj, ti.def_cnstr);
            if (fb.IsStatic)
                il.Emit(OpCodes.Stsfld, fb);
            else
                il.Emit(OpCodes.Stfld, fb);
            il.MarkLabel(lbl);
        }

        public static void CreateBoudedArray(ILGenerator il, LocalBuilder lb, TypeInfo ti)
        {
            Label lbl = il.DefineLabel();
            il.Emit(OpCodes.Ldloc, lb);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Brfalse, lbl);
            il.Emit(OpCodes.Newobj, ti.def_cnstr);
            il.Emit(OpCodes.Stloc, lb);
            il.MarkLabel(lbl);
        }

        public static bool IsBoundedArray(TypeInfo ti)
        {
            return ti.arr_fld != null;
        }

        public static void FixField(MethodBuilder mb, FieldBuilder fb, TypeInfo ti)
        {
            ILGenerator il = mb.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fb);
            if (fb.FieldType == TypeFactory.StringType)
            {
                il.Emit(OpCodes.Ldc_I4, (int)GCHandleType.Pinned);
                il.Emit(OpCodes.Call, TypeFactory.GCHandleAllocPinned);
            }
            else
            {
                il.Emit(OpCodes.Call, TypeFactory.GCHandleAlloc);
            }
            il.Emit(OpCodes.Pop);
        }

        public static void CloneField(MethodBuilder clone_meth, FieldBuilder fb, TypeInfo ti)
        {
            ILGenerator il = clone_meth.GetILGenerator();
            il.Emit(OpCodes.Ldloca_S, (byte)0);
            il.Emit(OpCodes.Ldarg_0);
            if (ti.clone_meth != null)
            {
                if (fb.FieldType.IsValueType)
                    il.Emit(OpCodes.Ldflda, fb);
                else
                    il.Emit(OpCodes.Ldfld, fb);
                il.Emit(OpCodes.Call, ti.clone_meth);
            }
            else
            {
                il.Emit(OpCodes.Ldfld, fb);
            }
            il.Emit(OpCodes.Stfld, fb);
        }

        public static void AssignField(MethodBuilder ass_meth, FieldBuilder fb, TypeInfo ti)
        {
            ILGenerator il = ass_meth.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarga_S, (byte)1);
            if (ti.clone_meth != null)
            {
                if (fb.FieldType.IsValueType)
                    il.Emit(OpCodes.Ldflda, fb);
                else
                    il.Emit(OpCodes.Ldfld, fb);
                il.Emit(OpCodes.Call, ti.clone_meth);
            }
            else
            {
                il.Emit(OpCodes.Ldfld, fb);
            }
            il.Emit(OpCodes.Stfld, fb);
        }

        public static void PushTypeOf(ILGenerator il, Type tp)
        {
            il.Emit(OpCodes.Ldtoken, tp);
            il.EmitCall(OpCodes.Call, TypeFactory.GetTypeFromHandleMethod, null);
        }
        
        public static bool IsPointer(Type tp)
        {
            return tp.IsPointer; /*|| tp==TypeFactory.IntPtr; INTPTR TODO*/
        }

        public static bool IsEnum(Type tp)
        {
            return !tp.IsGenericType && !tp.IsGenericTypeDefinition && tp.IsEnum;
        }
        
    }
}
