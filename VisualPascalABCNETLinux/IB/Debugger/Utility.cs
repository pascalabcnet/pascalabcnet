// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;
using System.Threading;
using VisualPascalABCPlugins;
using VisualPascalABC.Utils;
using Debugger;
using System.Runtime.ExceptionServices;

namespace VisualPascalABC
{

    public static class DebugUtils
    {
        private static Hashtable ht = new Hashtable();
        private static Dictionary<Type, DebugType> type_cache = new Dictionary<Type, DebugType>();

        static DebugUtils()
        {
            ht["System.Collections.ArrayList"] = ht;
            ht["System.Collections.Hashtable"] = ht;
            ht["System.Collections.Queue"] = ht;
            ht["System.Collections.Stack"] = ht;
            ht["System.Collections.SortedList"] = ht;
            ht["System.Collections.Generic.List"] = ht;
            ht["System.Collections.Generic.Dictionary"] = ht;
            ht["System.Collections.Generic.Stack"] = ht;
            ht["System.Collections.Generic.Queue"] = ht;
            ht["System.Collections.Generic.SortedList"] = ht;
            ht["System.Collections.Generic.SortedDictionary"] = ht;
            ht["System.Collections.Generic.LinkedList"] = ht;
        }

        public static bool CheckForCollection(DebugType type)
        {
            string name = type.FullName;
            int ind = name.IndexOf('<');
            if (ind != -1) name = name.Substring(0, ind);
            return ht[name] != null;
        }

        public static bool CheckForCollection(Mono.Debugging.Evaluation.TypeValueReference type)
        {
            string name = type.Name;
            int ind = name.IndexOf('<');
            if (ind != -1) name = name.Substring(0, ind);
            return ht[name] != null;
        }

        public static NamedValue GetNullBasedArray(Value val)
        {
            IList<FieldInfo> flds = val.Type.GetFields();
            if (flds.Count != 3) return null;
            foreach (FieldInfo fi in flds)
                if (fi.Name == "NullBasedArray") return fi.GetValue(val);
            return null;
        }

        public static MethodInfo GetMethod(DebugType t, string name)
        {
            return t.GetMember(name, Debugger.BindingFlags.All)[0] as MethodInfo;
        }

        public static PropertyInfo GetProperty(DebugType t, string name)
        {
            return t.GetMember(name, Debugger.BindingFlags.All)[0] as PropertyInfo;
        }

        public static DebugType GetDebugType(Type t)
        {
            Process p = WorkbenchServiceFactory.DebuggerManager.DebuggedProcess;
            string name;
            if (t.Assembly == typeof(int).Assembly)
                name = "mscorlib.dll";
            else
                name = t.Assembly.ManifestModule.ScopeName;
            DebugType dt = DebugType.Create(p.GetModule(name), (uint)t.MetadataToken);
            return dt;
        }


        public static DebugType GetDebugType(Type t, List<DebugType> gen_args)
        {
            string name;
            if (t.Assembly == typeof(int).Assembly)
                name = "mscorlib.dll";
            else
                name = t.Assembly.ManifestModule.ScopeName;
            return DebugType.Create(WorkbenchServiceFactory.DebuggerManager.DebuggedProcess.GetModule(name), (uint)t.MetadataToken, gen_args.ToArray());
        }

        public static bool IsQueue(DebugType type)
        {
            return type.FullName == "System.Collections.Queue" || type.FullName.StartsWith("System.Collections.Generic.Queue");
        }

        public static bool IsHashtable(DebugType type)
        {
            return type.FullName == "System.Collections.Hashtable";
        }

        public static bool IsLinkedList(DebugType type)
        {
            return type.FullName.StartsWith("System.Collections.Generic.LinkedList");
        }

        public static bool IsDictionary(DebugType type)
        {
            return type.FullName.StartsWith("System.Collections.Generic.Dictionary");
        }

        public static bool IsStack(DebugType type)
        {
            return type.FullName == "System.Collections.Stack" || type.FullName.StartsWith("System.Collections.Generic.Stack");
        }

        public static bool IsSortedList(DebugType type)
        {
            return type.FullName == "System.Collections.SortedList" || type.FullName.StartsWith("System.Collections.Generic.SortedList");
        }

        public static bool IsSortedDictionary(DebugType type)
        {
            return type.FullName.StartsWith("System.Collections.Generic.SortedDictionary");
        }

        public static string WrapTypeName(DebugType type)
        {
            if (type.IsArray)
                if (WorkbenchServiceFactory.DebuggerManager.parser != null)
                    return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetArrayDescription(WrapTypeName(type.GetElementType()), type.GetArrayRank());
                else
                    return "array of " + WrapTypeName(type.GetElementType());
            return internalWrapTypeName(type.FullName);
        }
        [HandleProcessCorruptedStateExceptionsAttribute]
        public static bool IsObject(Value nv)
        {
            try
            {
                return nv.IsObject;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }
        [HandleProcessCorruptedStateExceptionsAttribute]
        public static bool IsEnum(Value nv, out int val)
        {
            val = 0;
            try
            {

                if (IsObject(nv))
                {
                    IList<FieldInfo> fields = nv.Type.GetFields(BindingFlags.All);
                    bool is_enum = true;
                    foreach (FieldInfo fi in fields)
                    {
                        if (!(fi.IsStatic && fi.IsLiteral))
                        {
                            if (fi.Name != "value__")
                            {
                                is_enum = false;
                                break;
                            }
                            else
                                val = (int)fi.GetValue(nv).PrimitiveValue;
                        }
                    }
                    if (fields.Count == 0) is_enum = false;
                    return is_enum;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string WrapTypeName(Type t)
        {
            return internalWrapTypeName(t.FullName);
        }

        private static string internalWrapTypeName(string name)
        {
            if (WorkbenchServiceFactory.DebuggerManager.parser != null)
                switch (name)
                {
                    case "System.Boolean": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(bool));
                    case "System.Int32": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(int));
                    case "System.Byte": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(byte));
                    case "System.Double": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(double));
                    case "System.String": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(string));
                    case "System.Char": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(char));
                    case "System.SByte": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(sbyte));
                    case "System.Int16": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(short));
                    case "System.UInt16": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(ushort));
                    case "System.UInt32": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(uint));
                    case "System.Int64": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(long));
                    case "System.UInt64": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(ulong));
                    case "System.Float32": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(typeof(float));
                    case "System.IntPtr": return WorkbenchServiceFactory.DebuggerManager.parser.LanguageInformation.GetShortTypeName(Type.GetType("System.Void*"));
                    default: return name;
                }
            else
                switch (name)
                {
                    case "System.Boolean": return "boolean";
                    case "System.Int32": return "integer";
                    case "System.Byte": return "byte";
                    case "System.Double": return "real";
                    case "System.String": return "string";
                    case "System.Char": return "char";
                    case "System.SByte": return "shortint";
                    case "System.Int16": return "smallint";
                    case "System.UInt16": return "word";
                    case "System.UInt32": return "longword";
                    case "System.Int64": return "int64";
                    case "System.UInt64": return "uint64";
                    case "System.Float32": return "single";
                    case "System.IntPtr": return "pointer";
                    default: return name;
                }
        }

        public static object GetEnumValue(Mono.Debugging.Client.ObjectValue v)
        {
            var val = v.GetChild("value__");
            if (val != null && val.IsPrimitive)
                return val.GetRawValue();
            return null;
        }

        public static Value unbox(Value v)
        {
            return v.GetMember("m_value");
        }

        public static string MakeViewForCollection(Value v)
        {
            object cnt = v.GetMember("Count").PrimitiveValue;
            return "Count = " + cnt.ToString();
        }

        public static Mono.Debugging.Client.ObjectValue MakeMonoValue(object obj)
        {
            if (obj is Mono.Debugging.Client.ObjectValue)
                return obj as Mono.Debugging.Client.ObjectValue;
            if (obj is string)
                return Mono.Debugging.Client.ObjectValue.CreateString(null, new Mono.Debugging.Client.ObjectPath(""), obj.GetType().Name, obj as string);
            return Mono.Debugging.Client.ObjectValue.CreatePrimitive(null, new Mono.Debugging.Client.ObjectPath(""), obj.GetType().Name, new Mono.Debugging.Backend.EvaluationResult(obj.ToString()), Mono.Debugging.Client.ObjectValueFlags.Literal, obj);
        }

        public static Value MakeValue(object obj)
        {
            Value v = null;
            if (!(obj is string))
            {
                v = Eval.CreateValue(WorkbenchServiceFactory.DebuggerManager.DebuggedProcess, obj);
                v.SetPrimitiveValue(obj);
            }
            else
            {
                v = Eval.NewString(WorkbenchServiceFactory.DebuggerManager.DebuggedProcess, obj as string);
            }
            return v;
        }
    }
}

