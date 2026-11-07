// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace PascalABCCompiler.NETGenerator
{
    /// <summary>
    /// Emits assembly-local helpers for generic initialization.
    /// Create one emitter per output module and register its type with the other types
    /// that the generator finalizes before saving the assembly.
    /// </summary>
    internal sealed class GenericInitializationEmitter
    {
        // Same values as the corresponding cases in PABCSystem.
        private const byte NoInitialization = 0;
        private const byte RecordInitialization = 1;
        private const byte StringInitialization = 2;

        private readonly ModuleBuilder module;
        private readonly Action<TypeBuilder> registerType;
        private TypeBuilder helperType;
        private MethodBuilder determineTypeMethod;
        private MethodBuilder initializeMethod;
        private MethodBuilder initializeArrayMethod;
        private TypeBuilder kindCacheType;
        private FieldBuilder kindCacheField;

        internal GenericInitializationEmitter(ModuleBuilder module, Action<TypeBuilder> registerType)
        {
            this.module = module;
            this.registerType = registerType;
        }

        private TypeBuilder GetOrCreateHelperType()
        {
            if (helperType == null)
            {
                helperType = module.DefineType("$GenericInitialization$",
                    TypeAttributes.NotPublic | TypeAttributes.Abstract | TypeAttributes.Sealed);
                registerType(helperType);
            }
            return helperType;
        }

        // The CLR initializes this static field once per closed T. Keeping the
        // cache separate from user classes also works for classes read from PCU.
        internal FieldInfo GetOrCreateKindField(Type argument)
        {
            if (kindCacheType == null)
            {
                kindCacheType = module.DefineType("$GenericInitializationKind$`1",
                    TypeAttributes.NotPublic | TypeAttributes.Abstract | TypeAttributes.Sealed);
                var parameter = kindCacheType.DefineGenericParameters("T")[0];
                kindCacheField = kindCacheType.DefineField("Value", typeof(byte),
                    FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);

                var il = kindCacheType.DefineTypeInitializer().GetILGenerator();
                il.Emit(OpCodes.Ldtoken, parameter);
                il.Emit(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle)));
                il.Emit(OpCodes.Call, GetOrCreateDetermineTypeMethod());
                il.Emit(OpCodes.Stsfld, TypeBuilder.GetField(kindCacheType.MakeGenericType(parameter), kindCacheField));
                il.Emit(OpCodes.Ret);
                registerType(kindCacheType);
            }

            return TypeBuilder.GetField(kindCacheType.MakeGenericType(argument), kindCacheField);
        }

        // byte DetermineType(Type type)
        internal MethodBuilder GetOrCreateDetermineTypeMethod()
        {
            if (determineTypeMethod != null)
                return determineTypeMethod;

            determineTypeMethod = GetOrCreateHelperType().DefineMethod("DetermineType",
                MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.HideBySig,
                typeof(byte), new[] { typeof(Type) });
            var il = determineTypeMethod.GetILGenerator();
            var checkRecord = il.DefineLabel();
            var noInitialization = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldtoken, typeof(string));
            il.Emit(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle)));
            il.Emit(OpCodes.Call, typeof(Type).GetMethod("op_Equality", new[] { typeof(Type), typeof(Type) }));
            il.Emit(OpCodes.Brfalse, checkRecord);
            il.Emit(OpCodes.Ldc_I4, (int)StringInitialization);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(checkRecord);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, typeof(Type).GetProperty(nameof(Type.IsValueType)).GetGetMethod());
            il.Emit(OpCodes.Brfalse, noInitialization);
            il.Emit(OpCodes.Ldarg_0);
            EmitFindRecordInitializer(il);
            il.Emit(OpCodes.Brfalse, noInitialization);
            il.Emit(OpCodes.Ldc_I4, (int)RecordInitialization);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(noInitialization);
            il.Emit(OpCodes.Ldc_I4, (int)NoInitialization);
            il.Emit(OpCodes.Ret);
            return determineTypeMethod;
        }

        // object Initialize(byte kind, object value)
        internal MethodBuilder GetOrCreateInitializeMethod()
        {
            if (initializeMethod != null)
                return initializeMethod;

            initializeMethod = GetOrCreateHelperType().DefineMethod("Initialize",
                MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.HideBySig,
                typeof(object), new[] { typeof(byte), typeof(object) });
            var il = initializeMethod.GetILGenerator();
            var checkRecord = il.DefineLabel();
            var returnValue = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4, (int)StringInitialization);
            il.Emit(OpCodes.Bne_Un, checkRecord);
            il.Emit(OpCodes.Ldstr, string.Empty);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(checkRecord);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4, (int)RecordInitialization);
            il.Emit(OpCodes.Bne_Un, returnValue);

            // Invoke on the box supplied by the caller. Returning that same box
            // lets the caller unbox the record with its initialized fields.
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, typeof(object).GetMethod(nameof(object.GetType)));
            EmitFindRecordInitializer(il);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Callvirt, typeof(MethodBase).GetMethod(nameof(MethodBase.Invoke),
                new[] { typeof(object), typeof(object[]) }));
            il.Emit(OpCodes.Pop);

            il.MarkLabel(returnValue);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ret);
            return initializeMethod;
        }

        // void InitializeArray(Array array)
        internal MethodBuilder GetOrCreateInitializeArrayMethod()
        {
            if (initializeArrayMethod != null)
                return initializeArrayMethod;

            initializeArrayMethod = GetOrCreateHelperType().DefineMethod("InitializeArray",
                MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.HideBySig,
                typeof(void), new[] { typeof(Array) });
            var il = initializeArrayMethod.GetILGenerator();
            var strings = il.DeclareLocal(typeof(string[]));
            var index = il.DeclareLocal(typeof(int));
            var length = il.DeclareLocal(typeof(int));
            var test = il.DefineLabel();
            var loop = il.DefineLabel();
            var checkRecord = il.DefineLabel();
            var done = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Isinst, typeof(string[]));
            il.Emit(OpCodes.Stloc, strings);
            il.Emit(OpCodes.Ldloc, strings);
            il.Emit(OpCodes.Brfalse, checkRecord);
            il.Emit(OpCodes.Ldloc, strings);
            il.Emit(OpCodes.Ldlen);
            il.Emit(OpCodes.Conv_I4);
            il.Emit(OpCodes.Stloc, length);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc, index);
            il.Emit(OpCodes.Br, test);

            il.MarkLabel(loop);
            il.Emit(OpCodes.Ldloc, strings);
            il.Emit(OpCodes.Ldloc, index);
            il.Emit(OpCodes.Ldstr, string.Empty);
            il.Emit(OpCodes.Stelem_Ref);
            il.Emit(OpCodes.Ldloc, index);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc, index);

            il.MarkLabel(test);
            il.Emit(OpCodes.Ldloc, index);
            il.Emit(OpCodes.Ldloc, length);
            il.Emit(OpCodes.Blt, loop);
            il.Emit(OpCodes.Br, done);

            il.MarkLabel(checkRecord);
            EmitInitializeRecordArray(il, index, length, done);

            il.MarkLabel(done);
            il.Emit(OpCodes.Ret);
            return initializeArrayMethod;
        }

        private static void EmitInitializeRecordArray(ILGenerator il, LocalBuilder index, LocalBuilder length, Label done)
        {
            var elementType = il.DeclareLocal(typeof(Type));
            var initializer = il.DeclareLocal(typeof(MethodInfo));
            var boxed = il.DeclareLocal(typeof(object));
            var loop = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, typeof(Array).GetProperty(nameof(Array.Length)).GetGetMethod());
            il.Emit(OpCodes.Stloc, length);
            il.Emit(OpCodes.Ldloc, length);
            il.Emit(OpCodes.Brfalse, done);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, typeof(object).GetMethod(nameof(object.GetType)));
            il.Emit(OpCodes.Callvirt, typeof(Type).GetMethod(nameof(Type.GetElementType)));
            il.Emit(OpCodes.Stloc, elementType);
            il.Emit(OpCodes.Ldloc, elementType);
            il.Emit(OpCodes.Callvirt, typeof(Type).GetProperty(nameof(Type.IsValueType)).GetGetMethod());
            il.Emit(OpCodes.Brfalse, done);
            il.Emit(OpCodes.Ldloc, elementType);
            EmitFindRecordInitializer(il);
            il.Emit(OpCodes.Stloc, initializer);
            il.Emit(OpCodes.Ldloc, initializer);
            il.Emit(OpCodes.Brfalse, done);

            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc, index);
            il.MarkLabel(loop);

            // GetValue boxes the record. Invoke mutates that box, so store it
            // back into the array instead of discarding it.
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc, index);
            il.Emit(OpCodes.Callvirt, typeof(Array).GetMethod(nameof(Array.GetValue), new[] { typeof(int) }));
            il.Emit(OpCodes.Stloc, boxed);
            il.Emit(OpCodes.Ldloc, initializer);
            il.Emit(OpCodes.Ldloc, boxed);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Callvirt, typeof(MethodBase).GetMethod(nameof(MethodBase.Invoke),
                new[] { typeof(object), typeof(object[]) }));
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc, boxed);
            il.Emit(OpCodes.Ldloc, index);
            il.Emit(OpCodes.Callvirt, typeof(Array).GetMethod(nameof(Array.SetValue), new[] { typeof(object), typeof(int) }));

            il.Emit(OpCodes.Ldloc, index);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc, index);
            il.Emit(OpCodes.Ldloc, index);
            il.Emit(OpCodes.Ldloc, length);
            il.Emit(OpCodes.Blt, loop);
        }

        // Takes a Type from the IL stack and leaves its public parameterless
        // instance $Init$ MethodInfo, or null if the type has none.
        private static void EmitFindRecordInitializer(ILGenerator il)
        {
            il.Emit(OpCodes.Ldstr, "$Init$");
            il.Emit(OpCodes.Ldc_I4, (int)(BindingFlags.Public | BindingFlags.Instance));
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ldsfld, typeof(Type).GetField(nameof(Type.EmptyTypes)));
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Callvirt, typeof(Type).GetMethod(nameof(Type.GetMethod),
                new[] { typeof(string), typeof(BindingFlags), typeof(Binder), typeof(Type[]), typeof(ParameterModifier[]) }));
        }
    }
}
