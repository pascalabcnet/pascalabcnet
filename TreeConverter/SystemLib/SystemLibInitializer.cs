// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Инициализация системной библиотеки
using System;
using System.Collections.Generic;
using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.SystemLibrary
{
    public class UnitDefinitionItem
    {
        common_unit_node cmn;
        dot_net_unit_node dnu;
        string name;
        TreeConverter.SymbolInfoList symbolInfo = null;
        TreeConverter.SymbolInfoList _notCreatedSymbolInfo = null;
        TreeConverter.SymbolInfoList notCreatedSymbolInfo
        {
            get
            {
                if (_notCreatedSymbolInfo != null)
                    return _notCreatedSymbolInfo;
                if (cmn != null)
                {
                    _notCreatedSymbolInfo = cmn.scope.SymbolTable.Find(cmn.scope, name);
                }
                else
                {
                    Type t = NetHelper.NetHelper.PABCSystemType.Assembly.GetType("PABCSystem." + name);
                    if (t != null)
                    {
                        _notCreatedSymbolInfo = new TreeConverter.SymbolInfoList(new TreeConverter.SymbolInfoUnit(compiled_type_node.get_type_node(t, SystemLibrary.syn_visitor.SymbolTable)));
                    }
                    else
                    {
                        compiled_type_node ctn = compiled_type_node.get_type_node(NetHelper.NetHelper.PABCSystemType);
                        _notCreatedSymbolInfo = ctn.find_in_type(name);
                        /*if (name == TreeConverter.compiler_string_consts.read_procedure_name || name == TreeConverter.compiler_string_consts.readln_procedure_name)
                        {
                            compiled_type_node ctn2 = compiled_type_node.get_type_node(NetHelper.NetHelper.PT4Type);
                            TreeConverter.SymbolInfo si = ctn2.find_in_type(name);
                            TreeConverter.SymbolInfo tmp = _notCreatedSymbolInfo;
                            while (tmp.Next != null)
                                tmp = tmp.Next;
                            tmp.Next = si;
                        }*/
                        //while (_notCreatedSymbolInfo.Next != null)
                        //    _notCreatedSymbolInfo = _notCreatedSymbolInfo.Next;
                    }
                }
                return _notCreatedSymbolInfo;
                //return SymbolInfo;
            }
        }
        //bool assigned=false;

        public UnitDefinitionItem(common_unit_node cmn, string name)
        {
            this.name = name;
            this.cmn = cmn;            
        }
        public UnitDefinitionItem(dot_net_unit_node dnu, string name)
        {
            this.dnu = dnu;
            this.name = name;
        }
        public bool Restore()
        {
            return SymbolInfo != null;
        }
        public bool FromDll
        {
            get { return dnu != null; }
        }
        public TreeConverter.SymbolInfoList SymbolInfo
        {
            get
            {
                if (symbolInfo != null)
                    return symbolInfo;
                if (cmn != null)
                {
                    symbolInfo = cmn.scope.Find(name);
                }
                else
                {
                    Type t = NetHelper.NetHelper.PABCSystemType.Assembly.GetType("PABCSystem." + name);
                    if (t != null)
                    {
                        symbolInfo = new TreeConverter.SymbolInfoList(new TreeConverter.SymbolInfoUnit(compiled_type_node.get_type_node(t, SystemLibrary.syn_visitor.SymbolTable)));
                    }
                    else
                    {
                        compiled_type_node ctn = compiled_type_node.get_type_node(NetHelper.NetHelper.PABCSystemType);
                        symbolInfo = ctn.find_in_type(name);
                        /*if (name == TreeConverter.compiler_string_consts.read_procedure_name || name == TreeConverter.compiler_string_consts.readln_procedure_name)
                        {
                            compiled_type_node ctn2 = compiled_type_node.get_type_node(NetHelper.NetHelper.PT4Type);
                            TreeConverter.SymbolInfo si = ctn2.find_in_type(name);
                            TreeConverter.SymbolInfo tmp = _notCreatedSymbolInfo;
                            while (tmp.Next != null)
                                tmp = tmp.Next;
                            tmp.Next = si;
                        }*/
                        //while (symbolInfo.Next != null)
                        //    symbolInfo = symbolInfo.Next;
                    }
                }
                if (symbolInfo != null && SystemLibInitializer.TypedSetType.Equal(symbolInfo))
                {
                    if (symbolInfo.First().sym_info is common_type_node)
                    {
                        
                        common_type_node tctn = symbolInfo.First().sym_info as common_type_node;
                        tctn.type_special_kind = SemanticTree.type_special_kind.base_set_type;
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.plus_name, SystemLibInitializer.SetUnionProcedure.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.mul_name, SystemLibInitializer.SetIntersectProcedure.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.in_name, SystemLibInitializer.InSetProcedure.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.minus_name, SystemLibInitializer.SetSubtractProcedure.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.gr_name, SystemLibInitializer.CompareSetGreater.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.greq_name, SystemLibInitializer.CompareSetGreaterEqual.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.sm_name, SystemLibInitializer.CompareSetLess.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.smeq_name, SystemLibInitializer.CompareSetLessEqual.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.eq_name, SystemLibInitializer.CompareSetEquals.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.noteq_name, SystemLibInitializer.CompareSetInEquals.SymbolInfo.First());
                    }
                    else
                    {
                        compiled_type_node tctn = symbolInfo.First().sym_info as compiled_type_node;
                        tctn.type_special_kind = SemanticTree.type_special_kind.base_set_type;
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.plus_name, SystemLibInitializer.SetUnionProcedure.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.mul_name, SystemLibInitializer.SetIntersectProcedure.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.in_name, SystemLibInitializer.InSetProcedure.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.minus_name, SystemLibInitializer.SetSubtractProcedure.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.gr_name, SystemLibInitializer.CompareSetGreater.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.greq_name, SystemLibInitializer.CompareSetGreaterEqual.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.sm_name, SystemLibInitializer.CompareSetLess.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.smeq_name, SystemLibInitializer.CompareSetLessEqual.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.eq_name, SystemLibInitializer.CompareSetEquals.SymbolInfo.First());
                        tctn.scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.noteq_name,SystemLibInitializer.CompareSetInEquals.SymbolInfo.First());
                    }
                }
                else if (symbolInfo != null && SystemLibInitializer.TextFileType.Equal(symbolInfo))
                {
                    if (symbolInfo.First().sym_info is common_type_node)
                    {
                        common_type_node tctn = symbolInfo.First().sym_info as common_type_node;
                        tctn.type_special_kind = SemanticTree.type_special_kind.text_file;
                    }
                    else
                    {
                        compiled_type_node tctn = symbolInfo.First().sym_info as compiled_type_node;
                        tctn.type_special_kind = PascalABCCompiler.SemanticTree.type_special_kind.text_file;
                    }
                }
                else if (symbolInfo != null && string.Compare(name,PascalABCCompiler.TreeConverter.compiler_string_consts.ArrayCopyFunction,true)==0)
                {
                    while ((symbolInfo.InfoUnitList[0].sym_info as function_node).parameters.Count != 1)
                    {
                        symbolInfo.InfoUnitList.RemoveAt(0);
                    }
                }
                //assigned = true;
                return symbolInfo;
            }
        }
        public bool Equal(TreeConverter.SymbolInfoList si)
        {
            if (dnu == null)
                return notCreatedSymbolInfo.First() == si.First();
            else
            {
                foreach (var si_unit in si.InfoUnitList)
                    if (notCreatedSymbolInfo.First().sym_info == si_unit.sym_info)
                        return true;

                foreach(var tmp_si in notCreatedSymbolInfo.InfoUnitList)
                    if (tmp_si.sym_info == si.First().sym_info)
                        return true;

                return false;
            }
        }
        public definition_node sym_info
        {
            get
            {
                return SymbolInfo.First().sym_info;
            }
        }
        public bool NotFound
        {
            get
            {
                return notCreatedSymbolInfo == null;
            }
        }
        public bool Found
        {
            get
            {
                return notCreatedSymbolInfo != null;
            }
        }
        public type_node TypeNode
        {
            get
            {
                return sym_info as type_node;
            }
        }
        
        public type_node GetTypeNodeSpecials()
        {
        	if (symbolInfo != null)
                  return symbolInfo.First().sym_info as type_node;
            symbolInfo = cmn.scope.Find(name);
            return symbolInfo.First().sym_info as type_node;
        }
    }

    public static class SystemLibInitializer
    {
        public class initialization_properties
        {
            public compile_time_executor continue_executor;
            public compile_time_executor break_executor;
            public compile_time_executor exit_executor;
        }

        public static List<UnitDefinitionItem> NeedsToRestore = new List<UnitDefinitionItem>();

        public static UnitDefinitionItem format_function;
        public static UnitDefinitionItem read_procedure;
        public static UnitDefinitionItem write_procedure;
        public static UnitDefinitionItem writeln_procedure;
        public static UnitDefinitionItem readln_procedure;
        public static UnitDefinitionItem readln_from_file_procedure;
        public static UnitDefinitionItem read_short_string_procedure;
        public static UnitDefinitionItem read_short_string_from_file_procedure;
        public static UnitDefinitionItem StringDefaultPropertySetProcedure;
        public static UnitDefinitionItem TextFileType;
        public static UnitDefinitionItem BinaryFileType;
        public static UnitDefinitionItem AbstractBinaryFileType;
        public static UnitDefinitionItem PointerOutputType;
        public static UnitDefinitionItem TextFileInitProcedure;
        public static UnitDefinitionItem BinaryFileInitProcedure;
        public static UnitDefinitionItem BinaryFileReadProcedure;
        public static UnitDefinitionItem TypedFileType;
        public static UnitDefinitionItem TypedFileInitProcedure;
        public static UnitDefinitionItem TypedSetInitProcedure;
        public static UnitDefinitionItem TypedSetInitProcedureWithBounds;
        public static UnitDefinitionItem TypedSetInitWithShortString;
        public static UnitDefinitionItem AssignSetProcedure;
        public static UnitDefinitionItem AssignSetProcedureWithBounds;
        public static UnitDefinitionItem TypedFileReadProcedure;
        public static UnitDefinitionItem ShortStringType;
        public static UnitDefinitionItem ShortStringTypeInitProcedure;
        public static UnitDefinitionItem TypedSetType;
        public static UnitDefinitionItem SetUnionProcedure;
        public static UnitDefinitionItem SetIntersectProcedure;
        public static UnitDefinitionItem SetSubtractProcedure;
        public static UnitDefinitionItem InSetProcedure;
        public static UnitDefinitionItem CreateSetProcedure;
        public static UnitDefinitionItem IncludeProcedure;
        public static UnitDefinitionItem ExcludeProcedure;
        public static UnitDefinitionItem DiapasonType;
        public static UnitDefinitionItem CreateDiapason;
        public static UnitDefinitionItem CreateObjDiapason;
        public static UnitDefinitionItem CompareSetEquals;
        public static UnitDefinitionItem CompareSetInEquals;
        public static UnitDefinitionItem CompareSetLess;
        public static UnitDefinitionItem CompareSetLessEqual;
        public static UnitDefinitionItem CompareSetGreater;
        public static UnitDefinitionItem CompareSetGreaterEqual;
        public static UnitDefinitionItem IncProcedure;
        public static UnitDefinitionItem DecProcedure;
        public static UnitDefinitionItem SuccFunction;
        public static UnitDefinitionItem PredFunction;
        public static UnitDefinitionItem OrdFunction;
        public static UnitDefinitionItem ClipProcedure;
        public static UnitDefinitionItem ClipFunction;
        public static UnitDefinitionItem ClipShortStringProcedure;
        public static UnitDefinitionItem ClipShortStringInSetFunction;
        public static UnitDefinitionItem ClipShortStringInSetProcedure;
        public static UnitDefinitionItem GetCharInShortStringProcedure;
        public static UnitDefinitionItem SetCharInShortStringProcedure;
        public static UnitDefinitionItem SetLengthForShortStringProcedure;
        public static UnitDefinitionItem SetLengthProcedure;
        public static UnitDefinitionItem InsertProcedure;
        public static UnitDefinitionItem InsertInShortStringProcedure;
        public static UnitDefinitionItem DeleteProcedure;
        public static UnitDefinitionItem LowFunction;
        public static UnitDefinitionItem HighFunction;
        public static UnitDefinitionItem CheckCanUsePointerOnTypeProcedure;
        public static UnitDefinitionItem CheckCanUseTypeForBinaryFilesProcedure;
        public static UnitDefinitionItem CheckCanUseTypeForTypedFilesProcedure;
        public static UnitDefinitionItem RuntimeDetermineTypeFunction;
        public static UnitDefinitionItem RuntimeInitializeFunction;
        public static UnitDefinitionItem PointerToStringFunction;
        public static UnitDefinitionItem GetRuntimeSizeFunction;
        public static UnitDefinitionItem StrProcedure;
		public static UnitDefinitionItem ChrUnicodeFunction;
		public static UnitDefinitionItem AssertProcedure;
        public static UnitDefinitionItem CheckRangeFunction;
		public static UnitDefinitionItem CheckCharRangeFunction;
		public static UnitDefinitionItem CopyWithSizeFunction;
		public static UnitDefinitionItem ArrayCopyFunction;
        public static UnitDefinitionItem OMP_Available;
        public static UnitDefinitionItem OMP_ParallelFor;
        public static UnitDefinitionItem OMP_ParallelSections;
        public static UnitDefinitionItem PascalABCVersion;
        //native-->
        public static UnitDefinitionItem ObjectType;
        public static UnitDefinitionItem StringType;
        //<-- native
        public static UnitDefinitionItem ConfigVariable;
        public static function_node PointerOutputConstructor;
        
        private static PascalABCCompiler.TreeConverter.SymbolInfoUnit _NewProcedure;
        private static PascalABCCompiler.TreeConverter.SymbolInfoUnit _NewArrayProcedure;
        private static PascalABCCompiler.TreeConverter.SymbolInfoUnit _DisposeProcedure;

        private static common_namespace_function_node _NewProcedureDecl;
        private static common_namespace_function_node _NewArrayProcedureDecl;
        private static common_namespace_function_node _DisposeProcedureDecl;

        public static void RestoreStandardFunctions()
        {
            foreach (UnitDefinitionItem udn in NeedsToRestore)
            {
                udn.Restore();
            }
            NeedsToRestore.Clear();
        }

        public static type_node AbstractBinaryFileNode
        {
            get
            {
                if (AbstractBinaryFileType != null && AbstractBinaryFileType.Found)
                {
                    return AbstractBinaryFileType.TypeNode;
                }
                return null;
            }
        }

        public static common_namespace_function_node NewProcedureDecl
        {
            get
            {
                return _NewProcedureDecl;
            }
            set
            {
                _NewProcedureDecl = value;
            }
        }

        public static common_namespace_function_node NewArrayProcedureDecl
        {
            get
            {
                return _NewArrayProcedureDecl;
            }
            set
            {
                _NewArrayProcedureDecl = value;
            }
        }
        
        public static common_namespace_function_node DisposeProcedureDecl
        {
            get
            {
                return _DisposeProcedureDecl;
            }
            set
            {
                _DisposeProcedureDecl = value;
            }
        }

        public static PascalABCCompiler.TreeConverter.SymbolInfoUnit NewProcedure
        {
            get
            {
                return _NewProcedure;
            }
            set
            {
                _NewProcedure = value;
            }
        }
        public static PascalABCCompiler.TreeConverter.SymbolInfoUnit DisposeProcedure
        {
            get
            {
                return _DisposeProcedure;
            }
            set
            {
                _DisposeProcedure = value;
            }
        }


        private static void init_temp_methods_and_consts(common_namespace_node system_namespace,SymbolTable.Scope where_add,
            initialization_properties initialization_properties,location system_unit_location)
        {
            //SymbolTable.Scope sc = system_namespace.scope;
            SymbolTable.Scope sc = where_add;
            namespace_constant_definition _true_constant_definition = new namespace_constant_definition(
                PascalABCCompiler.TreeConverter.compiler_string_consts.true_const_name, SystemLibrary.true_constant, system_unit_location, system_namespace);
            system_namespace.constants.AddElement(_true_constant_definition);

            namespace_constant_definition _false_constant_definition = new namespace_constant_definition(
                PascalABCCompiler.TreeConverter.compiler_string_consts.false_const_name, SystemLibrary.false_constant, system_unit_location, system_namespace);
            system_namespace.constants.AddElement(_false_constant_definition);

            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.true_const_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(_true_constant_definition));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.false_const_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(_false_constant_definition));
            

            //TODO: Сделано по быстрому. Переделать. Можно просто один раз сериализовать модуль system и не инициализировать его всякий раз подобным образом. Неплохо-бы использовать NetHelper.GetMethod.
            Type tp = typeof(Console);
            compiled_function_node cfn;
            System.Type[] arr = new System.Type[1];
            System.Reflection.MethodInfo mi;

            //TODO: Сделать узел или базовый метод создания и удаления объекта.
            common_namespace_function_node cnfn = new common_namespace_function_node(TreeConverter.compiler_string_consts.new_procedure_name, null, null, system_namespace, null);
            cnfn.parameters.AddElement(new common_parameter("ptr", SystemLibrary.pointer_type, SemanticTree.parameter_type.value, cnfn,
                concrete_parameter_type.cpt_var, null, null));
            cnfn.SpecialFunctionKind = SemanticTree.SpecialFunctionKind.New;
            _NewProcedure = new PascalABCCompiler.TreeConverter.SymbolInfoUnit(cnfn);
            _NewProcedure.symbol_kind = PascalABCCompiler.TreeConverter.symbol_kind.sk_overload_function;
            _NewProcedure.access_level = PascalABCCompiler.TreeConverter.access_level.al_public;
            _NewProcedureDecl = cnfn;
            sc.AddSymbol(TreeConverter.compiler_string_consts.new_procedure_name,_NewProcedure);

            cnfn = new common_namespace_function_node(TreeConverter.compiler_string_consts.dispose_procedure_name, null, null, system_namespace, null);
            cnfn.parameters.AddElement(new common_parameter("ptr", SystemLibrary.pointer_type, SemanticTree.parameter_type.value,
                cnfn, concrete_parameter_type.cpt_var, null, null));
            _DisposeProcedure = new PascalABCCompiler.TreeConverter.SymbolInfoUnit(cnfn);
            _DisposeProcedure.symbol_kind = PascalABCCompiler.TreeConverter.symbol_kind.sk_overload_function;
            _DisposeProcedure.access_level = PascalABCCompiler.TreeConverter.access_level.al_public;
            _DisposeProcedureDecl = cnfn;
            cnfn.SpecialFunctionKind = SemanticTree.SpecialFunctionKind.Dispose;
            sc.AddSymbol(TreeConverter.compiler_string_consts.dispose_procedure_name, _DisposeProcedure);

            cnfn = new common_namespace_function_node(TreeConverter.compiler_string_consts.new_array_procedure_name, compiled_type_node.get_type_node(typeof(Array)), null, system_namespace, null);
            cnfn.parameters.AddElement(new common_parameter("t", compiled_type_node.get_type_node(typeof(Type)), SemanticTree.parameter_type.value, cnfn,
                concrete_parameter_type.cpt_none, null, null));
            cnfn.parameters.AddElement(new common_parameter("n", SystemLibrary.integer_type, SemanticTree.parameter_type.value, cnfn,
                concrete_parameter_type.cpt_none, null, null));
            cnfn.SpecialFunctionKind = SemanticTree.SpecialFunctionKind.NewArray;
            _NewArrayProcedure = new PascalABCCompiler.TreeConverter.SymbolInfoUnit(cnfn);
            _NewArrayProcedureDecl = cnfn;
            //sc.AddSymbol(TreeConverter.compiler_string_consts.new_procedure_name, _NewProcedure);
            
            basic_function_node break_procedure = new basic_function_node(SemanticTree.basic_function_type.none,
                null, true);
            break_procedure.compile_time_executor = initialization_properties.break_executor;
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.break_procedure_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(break_procedure));
            
            basic_function_node continue_procedure = new basic_function_node(SemanticTree.basic_function_type.none,
                null, true);
            continue_procedure.compile_time_executor = initialization_properties.continue_executor;
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.continue_procedure_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(continue_procedure));

            basic_function_node exit_procedure = new basic_function_node(SemanticTree.basic_function_type.none,
                null, true);
            exit_procedure.compile_time_executor = initialization_properties.exit_executor;
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.exit_procedure_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(exit_procedure));

            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.set_length_procedure_name,
                new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.resize_func, PascalABCCompiler.TreeConverter.access_level.al_public, PascalABCCompiler.TreeConverter.symbol_kind.sk_overload_function));
        }

        public static common_unit_node make_system_unit(SymbolTable.TreeConverterSymbolTable symbol_table,
            initialization_properties initialization_properties)
        {
            //TODO: В качестве location везде в этом методе следует указывать location system_unit-а. Имя файла мы знаем, а место - там где написано, что integer и прочие типы описаны как бы в модуле system.
            location system_unit_location = null;
            SymbolTable.UnitInterfaceScope main_scope = symbol_table.CreateUnitInterfaceScope(new SymbolTable.Scope[0]);
            SymbolTable.UnitImplementationScope impl_scope = symbol_table.CreateUnitImplementationScope(main_scope,
                new SymbolTable.Scope[0]);
            common_unit_node _system_unit = new common_unit_node(main_scope,impl_scope,null,null);
            
            common_namespace_node cnn = new common_namespace_node(null, _system_unit, PascalABCCompiler.TreeConverter.compiler_string_consts.system_unit_name,
                symbol_table.CreateScope(main_scope),system_unit_location);

            main_scope.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.system_unit_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(cnn));

            //SymbolTable.Scope sc = cnn.scope;
            SymbolTable.Scope sc = main_scope;

            //Добавляем типы.
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.byte_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.byte_type));
            //sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.decimal_type_name, new PascalABCCompiler.TreeConverter.SymbolInfo(SystemLibrary.decimal_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.sbyte_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.sbyte_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.short_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.short_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.ushort_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.ushort_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.integer_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.integer_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.uint_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.uint_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.long_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.int64_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.ulong_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.uint64_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.float_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.float_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.real_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.double_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.char_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.char_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.bool_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.bool_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.string_type));
            //sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.object_type_name, new PascalABCCompiler.TreeConverter.SymbolInfo(SystemLibrary.object_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.pointer_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.pointer_type));
            //sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.base_exception_class_name, new PascalABCCompiler.TreeConverter.SymbolInfo(SystemLibrary.exception_base_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.base_array_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.array_base_type));
            sc.AddSymbol(PascalABCCompiler.TreeConverter.compiler_string_consts.base_delegate_type_name, new PascalABCCompiler.TreeConverter.SymbolInfoUnit(SystemLibrary.delegate_base_type));

            //TODO: Переделать. Пусть таблица символов создается одна. Как статическая.
            compiled_type_node comp_byte_type = ((compiled_type_node)SystemLibrary.byte_type);
            compiled_type_node comp_sbyte_type = ((compiled_type_node)SystemLibrary.sbyte_type);
            compiled_type_node comp_short_type = ((compiled_type_node)SystemLibrary.short_type);
            compiled_type_node comp_ushort_type = ((compiled_type_node)SystemLibrary.ushort_type);
            compiled_type_node comp_integer_type = ((compiled_type_node)SystemLibrary.integer_type);
            compiled_type_node comp_uint_type = ((compiled_type_node)SystemLibrary.uint_type);
            compiled_type_node comp_long_type = ((compiled_type_node)SystemLibrary.int64_type);
            compiled_type_node comp_ulong_type = ((compiled_type_node)SystemLibrary.uint64_type);
            compiled_type_node comp_float_type = ((compiled_type_node)SystemLibrary.float_type);
            compiled_type_node comp_real_type = ((compiled_type_node)SystemLibrary.double_type);
            compiled_type_node comp_char_type=((compiled_type_node)SystemLibrary.char_type);
            compiled_type_node comp_bool_type = ((compiled_type_node)SystemLibrary.bool_type);
            compiled_type_node comp_string_type=((compiled_type_node)SystemLibrary.string_type);
            compiled_type_node comp_object_type=((compiled_type_node)SystemLibrary.object_type);
            compiled_type_node comp_pointer_type=((compiled_type_node)SystemLibrary.pointer_type);
            compiled_type_node comp_exception_type=((compiled_type_node)SystemLibrary.exception_base_type);
            compiled_type_node comp_array_type = ((compiled_type_node)SystemLibrary.array_base_type);
            compiled_type_node comp_delegate_type = ((compiled_type_node)SystemLibrary.delegate_base_type);
            comp_byte_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_byte_type.compiled_type, symbol_table);
            comp_sbyte_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_sbyte_type.compiled_type, symbol_table);
            comp_short_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_short_type.compiled_type, symbol_table);
            comp_ushort_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_ushort_type.compiled_type, symbol_table);
            comp_integer_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_integer_type.compiled_type, symbol_table);
            comp_uint_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_uint_type.compiled_type, symbol_table);
            comp_long_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_long_type.compiled_type, symbol_table);
            comp_ulong_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_ulong_type.compiled_type, symbol_table);
            comp_real_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_real_type.compiled_type, symbol_table);
            comp_char_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_char_type.compiled_type, symbol_table);
            comp_bool_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_bool_type.compiled_type, symbol_table);
            comp_string_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_string_type.compiled_type, symbol_table);
            comp_object_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_object_type.compiled_type, symbol_table);
            comp_pointer_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_pointer_type.compiled_type, symbol_table);
            comp_exception_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_exception_type.compiled_type, symbol_table);
            comp_array_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_array_type.compiled_type, symbol_table);
            comp_delegate_type.scope = new PascalABCCompiler.NetHelper.NetTypeScope(comp_delegate_type.compiled_type, symbol_table);

            init_temp_methods_and_consts(cnn,sc, initialization_properties,system_unit_location);
            return _system_unit;
        }
    }
}
