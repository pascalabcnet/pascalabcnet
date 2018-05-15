// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Строковые константы.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{

    public static class compiler_string_consts
    {

        public static Dictionary<string, string> oper_names = new Dictionary<string, string>();

        static int tvnc = 0;
        public static string GetTempVariableName()
        {
            return "$TV" + (tvnc++).ToString() + "$";
        }

        static compiler_string_consts()
        {
            oper_names[plus_name] = "op_Addition";
            oper_names[minus_name] = "op_Subtraction";
            oper_names[mul_name] = "op_Multiply";
            oper_names[div_name] = "op_Division";
            oper_names[idiv_name] = "op_Division";
            oper_names[and_name] = "op_BitwiseAnd";
            oper_names[or_name] = "op_BitwiseOr";
            oper_names[eq_name] = "op_Equality";
            oper_names[gr_name] = "op_GreaterThan";
            oper_names[greq_name] = "op_GreaterThanOrEqual";
            oper_names[sm_name] = "op_LessThan";
            oper_names[smeq_name] = "op_LessThanOrEqual";
            oper_names[mod_name] = "op_Modulus";
            oper_names[not_name] = "op_LogicalNot";
            oper_names[noteq_name] = "op_Inequality";
        }

        public static string GetNETOperName(string name)
        {
            string oper_name = null;
            if (oper_names.TryGetValue(name, out oper_name))
                return oper_name;
            return null;
        }

        public static string GetConnectedFunctionName(string class_name, string func_name)
        {
            if (oper_names.ContainsKey(func_name))
                func_name = oper_names[func_name];
            return class_name + "_" + func_name;
        }

        public static string GetGenericTypeInformation(string name, out int generic_param_count)
        {
            generic_param_count = 0;
            if (name == null)
            {
                return null;
            }
            string rez = name;
            int ind = name.IndexOf(compiler_string_consts.generic_params_infix);
            if (ind > 0)
            {
                rez = name.Substring(0, ind);
                if (name.Length > ind + 1)
                {
                    try
                    {
                        generic_param_count = Convert.ToInt32(name.Substring(ind + 1));
                    }
                    catch
                    {
                        generic_param_count = 0;
                    }
                }
            }
            return rez;
        }


        public static string ImplementationSectionNamespaceName = "_implementation______";

        public static readonly string bool_type_name = "boolean";
        public static readonly string byte_type_name = "byte";
        public static readonly string sbyte_type_name = "shortint";
        public static readonly string short_type_name = "smallint";
        public static readonly string ushort_type_name = "word";
        public static readonly string integer_type_name = "integer";//longint
        public static readonly string uint_type_name = "longword";//cardinal
        public static readonly string long_type_name = "int64";//int64
        public static readonly string ulong_type_name = "uint64";
        //public static readonly string decimal_type_name = "decimal";
        public static readonly string real_type_name = "real";
        public static readonly string float_type_name = "single";
        public static readonly string char_type_name = "char";
        public static readonly string string_type_name = "string";
        public static readonly string pointer_type_name = "pointer";
        public static readonly string object_type_name = "object";
        public static readonly string true_const_name = "true";
        public static readonly string false_const_name = "false";
        public static readonly string base_exception_class_name = "Exception";
        public static readonly string combine_method_name = "Combine";
        public static readonly string remove_method_name = "Remove";
        public static readonly string base_enum_class_name = "Enum";
        public static readonly string void_class_name = "void";
        public static string value = "value";
        public static readonly string object_equals_name = "Equals";

        public static readonly string plus_name = "+";
        public static readonly string minus_name = "-";
        public static readonly string mul_name = "*";
        public static readonly string div_name = "/";
        public static readonly string idiv_name = "div";
        public static readonly string mod_name = "mod";
        public static readonly string explicit_operator_name = "op_Explicit";
        public static readonly string implicit_operator_name = "op_Implicit";

        //public static string CommandLineArgsVariableName = "CommandLineArgs";
        public static string MainArgsParamName = "args";
        public static string IsConsoleApplicationVariableName = "IsConsoleApplication";

        public static readonly string plusassign_name = "+=";
        public static readonly string minusassign_name = "-=";
        public static readonly string multassign_name = "*=";
        public static readonly string divassign_name = "/=";
        public static readonly string power_name = "**";
        public static readonly string gr_name = ">";
        public static readonly string sm_name = "<";
        public static readonly string greq_name = ">=";
        public static readonly string smeq_name = "<=";
        public static readonly string eq_name = "=";
        public static readonly string noteq_name = "<>";

        public static readonly string or_name = "or";
        public static readonly string xor_name = "xor";
        public static readonly string not_name = "not";
        public static readonly string and_name = "and";

        public static readonly string as_name = "as";
        public static readonly string is_name = "is";
        public static readonly string in_name = "in";
        public static readonly string shl_name = "shl";
        public static readonly string shr_name = "shr";

        public static readonly string assign_name = ":=";

        public static readonly string unary_param_name = "param";
        public static readonly string left_param_name = "left";
        public static readonly string right_param_name = "right";

        public static readonly string main_function_name = "Main";
        public static readonly string c_main_function_name = "main";
        public static readonly string temp_main_function_name = "$Main";
        public static readonly string initialization_function_name = "$Initialization";
        public static readonly string finalization_function_name = "$Finalization";

        public static readonly string function_return_value_prefix = "$rv_";
        public static readonly string empty_function_name = "empty_function";

        public static readonly string break_procedure_name = "break";
        public static readonly string continue_procedure_name = "continue";
        public static readonly string exit_procedure_name = "exit";


        public static readonly string temp_for_variable_name = "$tfr_";

        public static readonly string lower_array_const_name = "LowerIndex";
        public static readonly string upper_array_const_name = "UpperIndex";

        public static readonly string pascal_array_name = "$pascal_array";
        public static readonly string indexer_name = "[]";
        public static readonly string internal_array_name = "NullBasedArray";
        public static readonly string simple_array_name = "simple_array";

        public static readonly string get_val_pascal_array_name = "get_val";
        public static readonly string set_val_pascal_array_name = "set_val";
        public static readonly string index_property_pascal_array_name = "index";

        public static readonly string system_unit_name = "PascalABCSystem";

        public static readonly string delegate_type_name_template = "$delegate";

        public static readonly string roof_name = "^";

        public static readonly string string_concat_method_name = "Concat";
        public static readonly string to_string_method_name = "ToString";

        public static readonly string base_delegate_type_name = "delegate";
        public static readonly string base_array_type_name = "array";
        public static readonly string net_constructor_name = ".ctor";

        public static readonly string method_group_type_name = "Method group";

        public static readonly string invoke_method_name = "Invoke";
        public static readonly string begin_invoke_method_name = "BeginInvoke";
        public static readonly string end_invoke_method_name = "EndInvoke";
        public static readonly string callback_string = "callback";
        public static readonly string result_string = "result";
        public static readonly string object_in_par_string = "'object'";

        public static readonly string omp_get_nested = "omp_get_nested";
        public static readonly string omp_set_nested = "omp_set_nested";
        public static readonly string OMP_NESTED = "OMP_NESTED";

        public static readonly string set_length_procedure_name = "SetLength";
        public static readonly string set_length_for_short_string = "SetLengthForShortString";
        public static readonly string self_word = "self";
        public static string get_pointer_type_name_by_type_name(string type_name)
        {
            return (roof_name + type_name);
        }

        public static string format_procedure_name = "FormatValue";
        public static string read_procedure_name = "read";
        public static string write_procedure_name = "write";
        public static string writeln_procedure_name = "writeln";
        public static string readln_procedure_name = "readln";
        public static string text_file_name_type_name = "Text";
        public static string TextFileInitProcedureName = "TextFileInit";
        public static string StringDefaultPropertySetProcedureName = "StringDefaultPropertySet";
        public static string BinaryFileTypeName = "BinaryFile";
        public static string AbstractBinaryFileTypeName = "AbstractBinaryFile";
        public static string BinaryFileInitProcedureName = "BinaryFileInit";
        public static string BinaryFileReadProcedureName = "BinaryFileRead";
        public static string TypedFileTypeName = "TypedFile";
        public static string TypedFileInitProcedureName = "TypedFileInit";
        public static string TypedFileReadProcedureName = "TypedFileRead";
        public static string ShortStringTypeName = "ShortString";
        public static string ShortStringTypeInitProcedure = "InitShortString";
        public static string PointerOutputTypeName = "PointerOutput";

        public static string PascalReadAccessorName = "read";
        public static string PascalWriteAccessorName = "write";
        public static string AssignSetName = "AssignSetFrom";
        public static string TypedSetInitProcedure = "TypedSetInit";
        public static string TypedSetInitProcedureWithBounds = "TypedSetInitWithBounds";
        public static string TypedSetInitWithShortString = "TypedSetInitWithShortString";
        public static string AssignSetProcedure = "AssignSet";
        public static string AssignSetProcedureWithBounds = "AssignSetWithBounds";
        public static string ClipShortStringInSetFunction = "ClipShortStringInSet";
        public static string ClipShortStringInSetProcedure = "ClipShortStringInSetProcedure";
        public static string union_of_set = "Union";
        public static string intersect_of_set = "Intersect";
        public static string subtract_of_set = "Subtract";
        public static string in_set = "InSet";
        public static string CreateSetProcedure = "CreateSet";
        public static string IncludeProcedure = "Include";
        public static string ExcludeProcedure = "Exclude";
        public static string DiapasonType = "Diapason";
        public static string CreateDiapason = "CreateDiapason";
        public static string CreateObjDiapason = "CreateObjDiapason";
        public static string CompareSetEquals = "CompareSetEquals";
        public static string CompareSetInEquals = "CompareSetInEquals";
        public static string CompareSetLess = "CompareSetLess";
        public static string CompareSetLessEqual = "CompareSetLessEqual";
        public static string CompareSetGreater = "CompareSetGreater";
        public static string CompareSetGreaterEqual = "CompareSetGreaterEqual";
        public static string IncProcedure = "Inc";
        public static string DecProcedure = "Dec";
        public static string SuccFunction = "Succ";
        public static string PredFunction = "Pred";
        public static string OrdFunction = "Ord";
        public static string ClipProcedure = "ClipSet";
        public static string ClipFunction = "ClipSetFunc";
        public static string ClipShortString = "ClipShortString";
        public static string GetCharInShortString = "GetCharInShortString";
        public static string SetCharInShortString = "SetCharInShortString";
        public static string read_short_string = "ReadShortString";
        public static string read_short_string_from_file = "ReadShortStringFromFile";
        public static string Insert = "Insert";
        public static string Delete = "Delete";
        public static string InsertInShortString = "InsertInShortString";
        public static string AssertProcedure = "Assert";
        public static string CopyWithSizeFunction = "CopyWithSize";
        public static string Low = "Low";
        public static string High = "High";
        public static string ArrayCopyFunction = "Copy";
        public static string check_in_range = "check_in_range";
        public static string check_in_range_char = "check_in_range_char";
        public static string ShortStringTypeNameTemplate = string_type_name + "[{0}]";
        public static string GetCurrentLineFunction = "__GetCurrentLine__";
        public static string GetCurrentFileFunction = "__GetCurrentFile__";
        public static string GetShortStringTypeName(int length)
        {
            if (length < 256)
                return string.Format(ShortStringTypeNameTemplate, length);
            else return string.Format(ShortStringTypeName);
        }

        public static string TypedFileTypeNameTemplate = "file of {0}";
        public static string TypedSetTypeNameTemplate = "set of {0}";
        public static string GetTypedFileTypeName(string elem_type_name)
        {
            return string.Format(TypedFileTypeNameTemplate, elem_type_name);
        }

        public static string GetSetTypeName(string elem_type_name)
        {
            return string.Format(TypedSetTypeNameTemplate, elem_type_name);
        }

        public static string array_word = "array";
        private static string of_word = "of";
        private static string space = " ";

        public static string result_variable_name = "result";

        public static string event_add_method_nameformat = event_add_method_prefix + "{0}";
        public static string event_remove_method_nameformat = event_remove_method_prefix + "{0}";
        public static string event_add_method_prefix = "add_";
        public static string event_remove_method_prefix = "remove_";

        public static string default_constructor_name = "create";

        // SSM - Константы директив компилятора. Вообще разбросаны по коду. Пусть будут здесь (3.1.2011)
        public static string compiler_directive_apptype = "apptype";
        public static string compiler_directive_reference = "reference";
        public static string include_namespace_directive = "includenamespace";
        public static string compiler_savepcu = "savepcu";
        public static string compiler_directive_nullbasedstrings = "nullbasedstrings";
        public static string compiler_directive_nullbasedstrings_ON = "string_nullbased+";
        public static string compiler_directive_nullbasedstrings_OFF = "string_nullbased-";
        public static string compiler_directive_initstring_as_empty_ON = "string_initempty+";
        public static string compiler_directive_initstring_as_empty_OFF = "string_initempty-";
        public static string compiler_directive_resource = "resource";
        public static string compiler_directive_platformtarget = "platformtarget";
        public static string compiler_directive_faststrings = "faststrings";

        // SSM (3.1.2011) Перенес эти константы сюда. 
        public static string version_string = "version";
        public static string product_string = "product";
        public static string company_string = "company";
        public static string copyright_string = "copyright";
        public static string trademark_string = "trademark";
        public static string main_resource_string = "mainresource";

        public static string system_unit_marker = "__IS_SYSTEM_MODULE";
        public static string system_unit_file_name = "PABCSystem";
        public static string extensions_unit_file_name = "PABCExtensions";

        public static string get_array_type_name(string type_name, int rank)
        {
            if (rank == 1)
                return (array_word + space + of_word + space + type_name);
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(',', rank - 1);
                return array_word + "[" + sb.ToString() + "]" + space + of_word + space + type_name;
            }
        }

        public static readonly string new_array_procedure_name = "__new_array";

        public static readonly string new_procedure_name = "new";
        public static readonly string dispose_procedure_name = "dispose";

        public static string pointer_net_type_name_intptr = "System.IntPtr";
        public static string pointer_net_type_name_void = "System.Void*";

        public static string record_const_type_name = "RecordConst";
        public static string array_const_type_name = "ArrayConst";
        public static string auto_type_name = "AutoType"; // SSM 25.06.16 - тип, который определяется на этапе компиляции при первом присваивании
        public static string ienumerable_auto_type_name = "IEnumerableAutoType"; // SSM 05.07.16 - тип, который определяется на этапе компиляции при первом присваивании
        public static string recort_printable_name_template = "record{0}end";
        public static string set_name = "TypedSet";
        public const string deconstruct_method_name = "deconstruct";
        public const string is_test_function_name = "IsTest";

        public static string bounded_array_printable_name_template = "array [{0}..{1}] of {2}";
        public static string array_printable_name_template = "array of {0}";

        public static readonly string generic_params_infix = "`";

        public static string GetAccessorName(string accessorTemplate, string name)
        {
            return string.Format(accessorTemplate, name);
        }
        public static string GetGetAccessorName(string name)
        {
            return GetAccessorName("get_{0}", name);
        }
        public static string GetSetAccessorName(string name)
        {
            return GetAccessorName("set_{0}", name);
        }
        public static string GetAddHandler(string name)
        {
            return "add_" + name;
        }
        public static string GetRemoveHandler(string name)
        {
            return "remove_" + name;
        }
        public static string IEnumerableInterfaceName = "System.Collections.IEnumerable";
        public static string IGenericEnumerableInterfaceName = "System.Collections.Generic.IEnumerable`1";
        public static string GetEnumeratorMethodName = "GetEnumerator";
        public static string IEnumeratorInterfaceName = "System.Collections.IEnumerator";
        public static string IGenericEnumeratorInterfaceName = "System.Collections.Generic.IEnumerator`1";
        public static string CurrentPropertyName = "Current";

        public static string static_ctor_prefix = "$sctor$";
        public static string generic_param_kind_prefix = "$kind_of$";
        public static string CheckCanUsePointerOnType_proc_name = "CheckCanUsePointerOnType";
        public static string CheckCanUseTypeForBinaryFiles_proc_name = "CheckCanUseTypeForBinaryFiles";
        public static string CheckCanUseTypeForTypedFiles_proc_name = "CheckCanUseTypeForTypedFiles";
        public static string RuntimeDetermineType_func_name = "RuntimeDetermineType";
        public static string RuntimeInitializeFunction_func_name = "RuntimeInitialize";
        public static string PointerToStringFunction_func_name = "PointerToString";
        public static string GetRuntimeSizeFunction_func_name = "GetRuntimeSize";
        public static string StrProcedure_func_name = "Str";
        public static string PascalABCVersion_func_name = "PascalABCVersion";
        public static string ChrUnicodeFunction_func_name = "ChrUnicode";
        public static string ExceptionName = "System.Exception";
        public static string value_in_accessor_name = "$value";
        public static string synonym_value_name = "$synonym";
        public static string file_of_attr_name = "$Attributes.$FileOfAttr";
        public static string set_of_attr_name = "$Attributes.$SetOfAttr";
        public static string type_synonim_attr_name = "$Attributes.$TypeSynonimAttr";
        public static string template_class_attr_name = "$Attributes.$TemplateClassAttr";
        public static string short_string_attr_name = "$Attributes.$ShortStringAttr";
        public static string global_attr_name = "$GlobAttr";
        public static string class_unit_attr_name = "$ClassUnitAttr";
        public static string pabc_rtl_dll_name = "PABCRtl.dll";
        public static string ObjectType = "Object";
        public static string StringType = "string";
        public static string config_variable_name = "__CONFIG__";
    }

}
